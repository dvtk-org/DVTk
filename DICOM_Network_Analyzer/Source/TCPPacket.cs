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

namespace TCP
{
	/// <summary>
	/// Summary description for TCPPacket.
	/// </summary>
	public class TCPPacket
	{
		public byte[] Packet;
		public int Index;
		public DateTime TimeStamp;
		public PacketTCP.PACKET_TCP PTcp;
		public PacketINTERNET.PACKET_INTERNET PInternet;

		public TCPPacket(byte[] packet,int index, PacketINTERNET.PACKET_INTERNET pInternet, PacketTCP.PACKET_TCP pTcp, DateTime Time)
		{
			Packet = packet;
			Index = index;

			PTcp = pTcp;
			PInternet = pInternet;
			TimeStamp = Time;
		}

		public bool SYN
		{
			get
			{
				return (PTcp.Flags & 0x02) > 0;
			}
		}

		public bool FIN
		{
			get
			{
				return (PTcp.Flags & 0x01) > 0;
			}
		}

		public bool ACK
		{
			get
			{
				return (PTcp.Flags & 0x10) > 0;
			}
		}

		public bool RST
		{
			get
			{
				return (PTcp.Flags & 0x04) > 0;
			}
		}		

		public uint DataLength
		{
			get
			{
				return (uint)(PInternet.Length - PInternet.HeaderLength - PTcp.HeaderLength);
			}
		}

		public int DataStart
		{
			get
			{
				return Index + PInternet.HeaderLength + PTcp.HeaderLength;
			}
		}

		public SeqNumber MinSequence
		{
			get
			{
				return PTcp.SequenceNumber;
			}
		}

		public SeqNumber MaxSequence
		{
			get
			{
				return (uint)(PTcp.SequenceNumber + DataLength);
			}
		}

		public override string ToString()
		{
			return "TCP: " + MinSequence.ToString() + " to " + MaxSequence.ToString() + (SYN?" SYN":"") + (FIN?" FIN":"") + (ACK?" ACK":"");
		}		
	}
}
