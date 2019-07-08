// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

using System;
using System.Diagnostics;

namespace TCP
{
	/// <summary>
	/// Summary description for TCPState.
	/// </summary>
	public class SeqNumber // Basically an uint64, but with modulu 2^32 comparisons
	{		
		private UInt32 N;
		public  SeqNumber(UInt32 x) { N=x;}
		public static explicit operator UInt32(SeqNumber x) { return x.N;}
		public static implicit operator SeqNumber(UInt32 x) { return new SeqNumber(x);}
		public static Int64 operator- (SeqNumber x, SeqNumber y)
		{
			if((x.N>=y.N && x.N-y.N < UInt32.MaxValue/2) || (y.N>=x.N && y.N-x.N < UInt32.MaxValue/2))
				return (Int64)x.N-(Int64)y.N;
			else if(x.N>=y.N)
				return y.N+UInt32.MaxValue+1-x.N;
			else
				return x.N+UInt32.MaxValue+1-y.N;
		}

		public static bool operator< (SeqNumber x, SeqNumber y)
		{
			return (x-y)<0;
		}

		public static bool operator> (SeqNumber x, SeqNumber y)
		{
			//return (x-y)>0;
			Int64 z = x-y;
			bool r = z>0;
			return r;
		}

		public static bool operator<= (SeqNumber x, SeqNumber y)
		{
			return (x-y)<=0;
		}

		public static bool operator>= (SeqNumber x, SeqNumber y)
		{
			return (x-y)>=0;
		}

		public override string ToString()
		{
			return N.ToString ();
		}

		public static SeqNumber operator+ (SeqNumber x, int y)
		{
			return new SeqNumber((uint)(x.N + y));
		}

		public static SeqNumber operator- (SeqNumber x, int y)
		{
			return new SeqNumber((uint)(x.N - y));
		}

		public static bool operator== (SeqNumber x, int y)
		{
			return x.N==y;
		}

		public static bool operator!= (SeqNumber x, int y)
		{
			return x.N!=y;
		}	

		public override bool Equals(object o)
		{
			return N==(o as SeqNumber).N;
		}
	
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class TCPState
	{
		System.Collections.ArrayList[] Packets= new System.Collections.ArrayList[2];
		System.Collections.ArrayList[] Times= new System.Collections.ArrayList[2];
		public SeqNumber[] Sequence = new SeqNumber[]{new SeqNumber(0),new SeqNumber(0)};

		public uint[] Position = new uint[2];
		public byte[][] data = new byte[2][];
		public enum States {NEW,SYN1,RUNNING,FINISHED,REPORTED};
		public DateTime LastAddedTime;

		//public States State = States.NEW;
		public States[] State = new States[]{States.NEW,States.NEW};
		public Object UserInfo;
		public enum PacketAction { NONE, DATA, FIN, RST, DEAD};
		public string[] Signature = new string[2];
		public TCPState(TCPPacket packet, string signature0, string signature1)
		{
			for(int i=0;i<2;i++)
			{
				data[i]=new byte[80];//0000];
				Position[i]=0;
				Packets[i]=new System.Collections.ArrayList();
				Times[i]=new System.Collections.ArrayList();
				Signature[0] = signature0;
				Signature[1] = signature1;
			}
			LastAddedTime = packet.TimeStamp;
		}

		public void Clear() // say we are no longer interested in this stream
		{	
			lock(this)
			{
				State[0] = States.REPORTED;
				State[1] = States.REPORTED;
				Packets[0].Clear();
				Packets[1].Clear();

                //data[0]=null;
                //data[1]=null;
                data = null;

				UserInfo = null;
			}
		}

		public int PacketCount
		{
			get
			{
				return Packets[0].Count + Packets[1].Count;
			}
		}
		
		public void AddPacket(int direction, TCPPacket packet)
		{
			if(State[direction] == States.NEW && direction==0 && packet.SYN) 
			{
				FirstPacket(packet,direction);
				State[direction] = States.SYN1;
			}
			if(State[direction] == States.NEW && direction==1 && packet.SYN) // should check SYNs as well
			{
				FirstPacket(packet,direction);
				State[0] = States.RUNNING;
				State[1] = States.RUNNING;
			}

			//Added irrespective of whether we can handle it now, unless known to be "dead"
			if(State[direction] != States.FINISHED && State[direction] != States.REPORTED)
				Packets[direction].Add(packet);

			LastAddedTime = packet.TimeStamp;
		}

		public PacketAction Defragment(int direction)
		{
            
			PacketAction result = PacketAction.NONE;
            
            bool LoopAgain = true;
            while (LoopAgain)
            {
                LoopAgain = false;
                foreach (TCPPacket packet in Packets[direction])
                {
                    if (packet.MinSequence <= Sequence[direction])
                    {
                        Packets[direction].Remove(packet);
                        if (packet.MaxSequence > Sequence[direction])  // i.e. if adds anything new
                        {
                            long srcIndex = packet.DataStart + (Sequence[direction] - packet.MinSequence);
                            long length = packet.MaxSequence - Sequence[direction];
                            long packetDataLength = packet.Packet.Length;

                            if (length > packetDataLength)
                            {
                                //ErrorLog("Unreasonable TCP/IP packet.Setting the packet length as received data length");

                                //Setting the packet length as received data length
                                //length = packetDataLength - srcIndex;
                                return PacketAction.DEAD;
                            }
                            long stPos = Position[direction];
                            long requiredLengh = stPos + length+10;//10 is approximately added... because sometimes system is throwing exception
                            long arLength = data[direction].Length;
                            if (requiredLengh > arLength)
                                Array.Resize(ref data[direction], (int)(arLength + (requiredLengh - arLength)));


                            Array.Copy(packet.Packet, srcIndex, data[direction], Position[direction], length);
                            Position[direction] += (uint)(packet.MaxSequence - Sequence[direction]);
                            Sequence[direction] = packet.MaxSequence;
                            Times[direction].Add(new PacketTime(packet.TimeStamp, packet.MinSequence));
                            return PacketAction.DATA;
                        }

                        if (packet.FIN)
                        {
                            State[direction] = States.FINISHED;
                            return PacketAction.FIN;
                        }

                        if (packet.RST)
                        {
                            State[direction] = States.FINISHED;
                            return PacketAction.RST;
                        }

                        LoopAgain = true;  // here for repeats etc.
                        break;  // to restart iterator
                    }
                    else  // beyond current position
                    {
                        if (packet.MaxSequence > Sequence[direction] + packet.PTcp.WindowSize * 2 && Sequence[direction] != 0) // more than twice ahead of window means likely missed data
                        {
                            result = PacketAction.DEAD;
                        }
                    }
                }
            }
            
            
			return result;

		}

		void FirstPacket(TCPPacket packet, int direction)
		{
			Sequence[direction]=packet.PTcp.SequenceNumber  + 1 ; //+1 removes SYN from stream
		}

		public DateTime GetTime(int direction, long Pos)
		{
			PacketTime best = null;//Times[direction][0] as PacketTime;
			// initial sequence number = current max sequence (sequence) - position
			// wanted position = that + Pos
			SeqNumber s = Sequence[direction] + (int)(Pos- Position[direction]) ;

			foreach (PacketTime pt in Times[direction])
			{
                if (pt != null)
                {
                    if ((pt.sequence <= s) && ((best == null) || (pt.sequence > best.sequence)))
                    {
                        best = pt;
                    }
                }
			}

            if (best != null)
                return best.time;
            else
                return new DateTime(1970, 1, 1, 0, 0, 0);
		}

		public void UsedData(uint Length, int direction)
		{
			Position[direction] -= Length;
			Array.Copy(data[direction], Length, data[direction], 0, (int)Position[direction]);
			// now tidy up "times" data

			lock(Times[direction])
			{
				int count = 0;
				SeqNumber s = Sequence[direction]-(int)Position[direction];
				s -= 1500;  // safety to prevent unwanted removals (should really be done with a max value!)
				for(int i = 0; i<Times[direction].Count;i++)
				{
					PacketTime pt = Times[direction][i] as PacketTime;
					if(pt.sequence < s)
						count = i +1 ;
				}
				if(count > 0)
					Times[direction].RemoveRange(0,count);
			}
		}
	}
}
