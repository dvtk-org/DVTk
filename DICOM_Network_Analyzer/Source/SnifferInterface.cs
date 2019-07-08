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

namespace Sniffer
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>	
	public class PacketInfo
	{
		public byte[] Data;
		public int StartIndex;
		public DateTime TimeStamp;
		public uint Length;
	}

	public delegate void ErrorEvent(string Error) ;
	public delegate void DataReceivedEvent(PacketInfo data) ;

	public interface SnifferInterface
	{
		event ErrorEvent Error;
		event DataReceivedEvent DataReceived;
		string Filter { get;set;}
		string[] AdapterDescriptions {get;}
		string[] AdapterNames {get;}
		int AdapterIndex {get;set;}
		void Start();
		void Stop();
        void StartDump(string capFile);
        void StopDump();
	}
}
