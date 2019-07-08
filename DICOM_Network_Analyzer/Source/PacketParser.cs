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
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace Sniffer
{
	public struct PACKET_ITEM
	{
		public uint Seconds;         // seconds 
		public uint MicroSeconds;    // and microseconds 
		public uint CaptureLength;	 // length of portion present
		public string CaptureTimeStr;
		public uint PacketLength;	 // length this packet (off wire)
		public uint Reserved;
		public string TypeInfo;
		public byte [] FrameData;
		public byte [] Data;

	}
}
