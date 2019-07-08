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
using Sniffer;
using System.Diagnostics;
using System.Threading;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TCP
{
	/// <summary>
	/// This is a GENERIC TCP parser routine, which should be able to handle packets coming from 
	/// ANY osrt of interface (WinPCap, native .NET etc.) It assembles fragments into incoming/outgoing
	/// byte streams and fires an event every time a new fragment is added.  It is up to the event
	/// consumer to decide whether to "use now" or leave in the buffer
	/// 
	/// Method of Operation:
	/// 
	/// All available data is taken from the packets and the data is passed on via the TCPDataAvailable 
	/// event.   This may be done either explicitly via a call to TCPAnalyse, or a thread may be set up 
	/// using TCPStartAnalyse (and stopped using TCPStopAnalyse).
	/// 
	/// Input:  Bytes passed asynchronously into the "Packets" Array (normally by the Sniffer object)
	/// 
	/// Output: The TCPDataAvailable event is fired each time data is added to a TCP data
	/// </summary>		
	public class TCPParser
	{
		public const int LENGTH_OF_INTERNET			= 20;
		public const int LENGTH_OF_TCP				= 20;
		public const int IPPROTO_TCP                = 6;
		
		public delegate void FragmentAddedEvent(TCPState state, int direction);
		public delegate void EndOfStreamEvent(TCPState state, int direction, string result);
		public delegate void ErrorEvent(string Error);
		public FragmentAddedEvent FragmentAdded;
		public EndOfStreamEvent EndOfStream;
		public ErrorEvent Error;
        public List<string> listOfConnections = new List<string>();
		private ListDictionary[] List = new ListDictionary[2];
		public string IP1="", IP2="";
		public ushort Port1=0, Port2=0;
		DateTime LastParsedPacketTime;
		public Thread tcpThread;
		public bool analysisFromCapFile = false;
		bool running = false;
		Queue Packets= new Queue();
		int nrOfCapturedPackets = 0;
		System.Threading.Timer timer;
		public enum CleanupType {BACKGROUND, UPTODATE, TERMINAL};

		public TCPParser()
		{
			for(int i=0;i<2;i++)
				List[i] = new System.Collections.Specialized.ListDictionary();
		}

		~TCPParser()
		{}

		public override string ToString()
		{
			StringBuilder s = new StringBuilder();
			for(int i=0;i<2;i++)
			{
				s.Append("List"+ i.ToString() + "\r\n");
				foreach (DictionaryEntry item in List[i])
				{
					TCPState state = item.Value as TCPState;
					s.Append(item.Key.ToString() + ":" + state.State.ToString() + "\r\n");
				}
			}
			return s.ToString();
		}

		public int QueuedPackets
		{
			get
			{
				// no need to lock - this is informational only
				return Packets.Count;
			}
		}

		public int CapturedPackets
		{
			get
			{
				// no need to lock - this is informational only
				return nrOfCapturedPackets;
			}
		}

		public Int64 QueuedBytes
		{
			get
			{
				Int64 n=0;
				lock(Packets)
				{
					foreach(PacketInfo b in Packets)
						n+=b.Length;
				}
				return n;
			}
		}

		public int NrOfConnections
		{
			get
			{
				return List[0].Count;
			}
		}

		public void AddPacket(PacketInfo p)
		{
			// NOTE - this is designed to be called asynchronously from the
			// packet sniffing thread
			lock(Packets)
			{
				Packets.Enqueue(p);
			}
		}

		public void TCPStartAnalyse()
		{
			if(tcpThread != null)
				throw new Exception("Analysis thread is already running");
			
			tcpThread = new Thread(new ThreadStart(AnalyseThreadFunction));
			tcpThread.Name = "TCP Analysis Thread";
			tcpThread.Priority = ThreadPriority.BelowNormal;
			nrOfCapturedPackets = 0;

			tcpThread.Start();
			timer = new Timer(new TimerCallback(CleanupOnTimer),null,200,200);
		}

		public bool TCPStopAnalyse()
		{
			bool stopped = false;
			if(!analysisFromCapFile)
			{
				if(timer != null)
				{
					timer.Dispose();
					timer = null;
				}
				running = false;

				if(tcpThread != null)
				{
					tcpThread.Join();				
					tcpThread = null;
				}
				stopped = true;
			}
			else
			{
				if(timer != null)
				{
					timer.Dispose();
					timer = null;
				}
				running = false;

				if(tcpThread != null)
				{
					tcpThread.Join();
					tcpThread = null;
				}
				
				stopped = true;
				analysisFromCapFile = false;
			}

			return stopped;
		}
		
		private void AnalyseThreadFunction()
		{			
			running = true;
			try
			{
				while(running || (QueuedPackets > 0))
				{
					if(!TCPAnalyse())
					{
						Thread.Sleep(250);
					}
                    //else
                    //{
                    //    if ((analysisFromCapFile) && (QueuedPackets == 0))
                    //        running = false;
                    //}
				}

				// close all open associations
				Cleanup(CleanupType.TERMINAL);
			}
			catch(Exception e)
			{
				ErrorLog(e.Message + "\r\n" + e.TargetSite + "\r\n" + e.StackTrace);
			}
			
			return;
		}

		public bool TCPAnalyse()
		{
			if(QueuedPackets == 0)
			{
				this.LastParsedPacketTime = DateTime.UtcNow;
				return false;
			}

			while(QueuedPackets > 0)
			{
				PacketInfo p;
				lock(Packets)
				{
					p = Packets.Dequeue() as PacketInfo;
				}

				AnalysePacket(p);
			}	
			return true;
		}

		static string Signature(string s, ushort sp, string d, ushort dp)
		{
			return s + "(" + sp.ToString() + ")-" + d + "(" + dp.ToString() + ")";
		}

		public void AnalysePacket(PacketInfo data)
		{
			byte [] PacketData = data.Data;
            int StartIndex = data.StartIndex;

			int Index = StartIndex;

			// Start by eliminating non IP and non TCP packets
			if( ( Index + LENGTH_OF_INTERNET + LENGTH_OF_TCP ) > PacketData.Length )
			{
				return ;
			}

			PacketINTERNET.PACKET_INTERNET PInternet = new PacketINTERNET.PACKET_INTERNET();
			PInternet.Version = PacketData[ Index++ ];
			PInternet.HeaderLength = (byte) ( ( (int) PInternet.Version & 0x0f ) * 4 );
			PInternet.Version = (byte) ( (int) PInternet.Version >> 4 );
			PInternet.DifferentiatedServicesField = PacketData[ Index++ ];
			PInternet.Length = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PInternet.Identification = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PInternet.FragmentOffset = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PInternet.Flags = (byte)( (int) PInternet.FragmentOffset >> 12 );
			PInternet.FragmentOffset = (ushort) ( (int) PInternet.FragmentOffset & 0x0f );
			PInternet.TimeToLive = PacketData[ Index++ ];
			PInternet.Protocol = PacketData[ Index++ ];
			PInternet.HeaderChecksum = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PInternet.Source = Function.GetIpAddress( PacketData , ref Index );
			PInternet.Destination  = Function.GetIpAddress( PacketData , ref Index );
			if(PInternet.Protocol  != IPPROTO_TCP )
				return;

			// Check IPs
            //if(!analysisFromCapFile)
            //{
				if(((PInternet.Source == IP1) && (PInternet.Destination == IP1)) ||
					(PInternet.Source == PInternet.Destination))
				{
					return;
				}
            //}

			PacketTCP.PACKET_TCP PTcp = new PacketTCP.PACKET_TCP();

			PTcp.SourcePort = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PTcp.DestinationPort = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PTcp.SequenceNumber = Function.Get4Bytes( PacketData , ref Index , Const.NORMAL );
			PTcp.Acknowledgement = Function.Get4Bytes( PacketData , ref Index , Const.NORMAL );
			PTcp.HeaderLength = PacketData[ Index++ ];
			PTcp.HeaderLength = (byte) ( ( (int) PTcp.HeaderLength >> 4 ) * 4 );
			PTcp.Flags = PacketData[ Index++ ];
			PTcp.WindowSize = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PTcp.Checksum = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );
			PTcp.Options = Function.Get2Bytes( PacketData , ref Index , Const.NORMAL );

            //if(!analysisFromCapFile)
            //{
				if(((PTcp.SourcePort == Port1) && (PTcp.DestinationPort == Port1)) ||
					((PTcp.SourcePort == Port2) && (PTcp.DestinationPort == Port2)))
				{
					return;
				}
            //}

			String signature = Signature(PInternet.Source, PTcp.SourcePort, PInternet.Destination, PTcp.DestinationPort);
            //Keep track of the connections which have been established.(The list will be used later to populate the Associations Combo Box.)
            if (!listOfConnections.Contains(signature))
            {
                listOfConnections.Add(signature);
            }
			TCPPacket packet = new TCPPacket(PacketData, StartIndex , PInternet, PTcp, data.TimeStamp);
			
			LastParsedPacketTime = packet.TimeStamp;

            int match = -1;
			TCPState state;
			for(int i=0;i<2;i++)
			{
				if(List[i].Contains(signature))
				{
					match=i;
				}
			}

			// add as new item if necessary
			if(match==(-1))
			{
				//Need to check here that we have SYNs
				String signature1 = Signature(PInternet.Destination, PTcp.DestinationPort,PInternet.Source, PTcp.SourcePort);
				state = new TCPState(packet,signature, signature1);
				List[0].Add(signature,state);
				List[1].Add(signature1,state);
				match=0;
			}
			else
			{
				state = (TCPState) List[match][signature];
			}

			lock(state)
			{
				if(state.State[match] != TCPState.States.REPORTED)
				{
					state.AddPacket(match,packet);
					TCPState.PacketAction LastAction;
					while((LastAction = state.Defragment(match)) == TCPState.PacketAction.DATA)
					{
						if(FragmentAdded != null)
						{
							FragmentAdded(state, match);							
						}
						nrOfCapturedPackets++;
					}
					if(LastAction == TCPState.PacketAction.FIN)
					{
						if(EndOfStream != null)
							EndOfStream(state, match, "FIN Seen (" + signature + ")");						
					}
					if(LastAction == TCPState.PacketAction.RST)
					{
						if(EndOfStream != null)
							EndOfStream(state, match, "RST Seen (" + signature + ")");
					}
					if(LastAction == TCPState.PacketAction.DEAD)
					{
						//Error("DEAD seen" + signature);
						if(EndOfStream != null)
							EndOfStream(state, match, "DEAD data seen (" + signature + ")");
					}
				}
			}
		}

		void ErrorLog(string s)
		{
			if(Error != null)
				Error(s);
		}

		void CleanupOnTimer(object o)
		{
			Cleanup(CleanupType.BACKGROUND);
		}

		public void Cleanup(CleanupType type)
		{
			// Remove any "stale" connections - i.e. those
			// 1) In state REPORTED, FINISHED or NEW with no traffic for 20 seconds
			// 2) In any other state with no traffic for 5 minutes

			// Also, make FINISHED any connection with > 200 packets outstanding unanalysed
			// (i.e. there has been a "hole" in the traffic :-( )

			// only need to do this for direction 0 - List is just a double index
				
			//Non_DICOM ongoing connections
			foreach (TCPState state in List[0].Values)
			{
				if(state.PacketCount > 200)
				{
					if(state.State[0] == TCPState.States.NEW)
					{
						EndOfStream(state,0," >200 packets");
						state.Clear();
					}
				}
			}

			bool moretodo = true;
			while(moretodo)
			{
				moretodo = false;
				foreach (TCPState state in List[0].Values)
				{
					//This is based on packet time, not current time					
					TimeSpan ts = LastParsedPacketTime - state.LastAddedTime; 
					string err = "";
					
					if(type == CleanupType.TERMINAL)
						err = "Forced Close";
					else if( ts.TotalSeconds > 20 && state.State[0] != TCPState.States.RUNNING)
						err = " > 20 seconds and not running : State was " + state.State.ToString();
//					else if( ts.TotalSeconds > 300)  // &&&& 300
//						err = " > 300 seconds since last activity";

					if(err != "")
					{
						List[0].Remove(state.Signature[0]);
						List[1].Remove(state.Signature[1]);
						if(running && state.State[0] != TCPState.States.REPORTED)
						//if((state.State[0] != TCPState.States.REPORTED) && (state.State[0] != TCPState.States.FINISHED))
						{
							EndOfStream(state,0,err);
						}
						moretodo = true;
						break;
					}
				}
			}
		}
	}
}
