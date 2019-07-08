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
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Sniffer
{
	/// <summary>
	/// This encapsulates all the interfacing neede to abstract a sniffer, which there has 
	/// the following inteface:
	/// 
	/// GetAdapterNames
	/// 
	/// Start (this starts up a new thread)
	/// 
	/// Stop (this returns once thread is joined)
	/// 
	/// Event: DataReceived - when data is available
	/// 
	/// </summary>
	public class WinPCapObject : SnifferInterface
	{
		public event ErrorEvent Error;
		public event DataReceivedEvent DataReceived;

        SnifferUI.DICOMSniffer snifferObj = null;
		protected bool running;
		Thread Thread;
		protected Stream file;
		enum Format {WINPCAP,XCP001,XCP002, SUNSNOOP} ;
		Format FileFormat;
		DateTime BaseTime;
		TCP.TCPParser tcp;

        public WinPCapObject(SnifferUI.DICOMSniffer mainObj, string Filename, TCP.TCPParser TCP)
		{
			tcp = TCP;
            snifferObj = mainObj;
			
			file = new FileStream(Filename,FileMode.Open,FileAccess.Read);
			
			uint magicNumber = Function.Get4BytesFromStream(file,Const.NORMAL);
			BaseTime = new DateTime(1970,1,1,1,2,3,123);
			switch(magicNumber)
			{
				case 0xD4C3B2A1:
					FileFormat = Format.WINPCAP;
					file.Position = 24;
					break;

				case 0x58435000: // "XCP"
					file.Position = 6;
					ushort versionNumber = Function.Get2BytesFromStream(file,Const.NORMAL);
					if(versionNumber == 0x312E)
					{
						FileFormat = Format.XCP001;
					}
					else
					{
						FileFormat = Format.XCP002;
					}

					file.Position = 12;
					BaseTime += new TimeSpan(((long)Function.Get4BytesFromStream( file , Const.VALUE ))*10000000);

					file.Position = 52;
					BaseTime -= new TimeSpan(((long)Function.Get8BytesFromStream( file , Const.VALUE ))*10);

					file.Position = 128;
					break;

                case 0x736E6F6F: // Sun snoop
                    FileFormat = Format.SUNSNOOP;
                    file.Position = 16;
                    break;

				default:
                    throw new Exception("Only the winpcap format (NA sniffer (windows 1.1) is supported. Unable to load capture file.");
			}

			// Sets the progress bar's minimum value to a number 
			// representing the current position in file before the files are read in.
            snifferObj.Invoke(snifferObj.MinProgressHandler, new object[] { (int)file.Position });
			// Sets the progress bar's maximum value to a number 
			// representing the total length of the file.
            snifferObj.Invoke(snifferObj.MaxProgressHandler, new object[] { (int)file.Length });
		}

		public void Start()
		{
			if(Thread != null)
				throw new Exception("Sniffer thread is already running");
			
			Thread = new Thread(new ThreadStart(ThreadFunction));
			Thread.Name = "Sniffer Thread";
			Thread.Priority = ThreadPriority.AboveNormal;
			Thread.Start();
		}

		public void Stop()
		{
			if(running)
			{
				running = false;
				Thread.Join();
				Thread = null;
			}
		}

		protected void OnDataReceived(PacketInfo data)
		{
			DataReceived(data);
		}

		protected virtual void ThreadFunction()
		{
			running = true;
            
			try
			{
				while(running && file.Position < file.Length)
				{
                    uint packetRecordLength = 0;
					PACKET_ITEM PItem = new PACKET_ITEM();
					switch(FileFormat)
					{
						case Format.WINPCAP:
							PItem.Seconds = Function.Get4BytesFromStream( file , Const.VALUE );
							PItem.MicroSeconds = Function.Get4BytesFromStream( file ,  Const.VALUE );
							PItem.CaptureLength = Function.Get4BytesFromStream( file , Const.VALUE );
							PItem.PacketLength = Function.Get4BytesFromStream( file , Const.VALUE );
							break;

						case Format.XCP001:
							ulong timestamp = Function.Get4BytesFromStream( file , Const.VALUE );
            
							PItem.Seconds = (uint)(timestamp/1000000000);
							uint millseconds = (uint)((timestamp/1000000) % 1000);
							uint counter = (uint)(timestamp % 1000);
							PItem.MicroSeconds = millseconds * 1000 + counter;
							file.Position +=6;
            
							PItem.CaptureLength = Function.Get2BytesFromStream( file , Const.VALUE );
							PItem.PacketLength = PItem.CaptureLength;
							file.Position +=16;  
							break;

						case Format.XCP002:
							ulong temp2 = Function.Get8BytesFromStream( file , Const.VALUE );
							PItem.Seconds = (uint)(temp2/1000000);
							PItem.MicroSeconds = (uint)(temp2 % 1000000);
							PItem.CaptureLength = Function.Get2BytesFromStream( file , Const.VALUE );
							PItem.PacketLength = Function.Get2BytesFromStream( file , Const.VALUE );
							file.Position +=28;
							break;

                        case Format.SUNSNOOP:
                            PItem.CaptureLength = Function.Get4BytesFromStream(file, Const.NORMAL);
                            PItem.PacketLength = Function.Get4BytesFromStream(file, Const.NORMAL);
                            packetRecordLength = Function.Get4BytesFromStream(file, Const.NORMAL);
                            uint droppedPackets = Function.Get4BytesFromStream(file, Const.NORMAL);
                            ulong temp3 = Function.Get8BytesFromStream(file, Const.NORMAL);
                            PItem.Seconds = (uint)(temp3 / 1000000);
                            PItem.MicroSeconds = (uint)(temp3 % 1000000);                            
                            break;
					}

					PacketInfo data = new PacketInfo();
					data.Data = new byte[PItem.CaptureLength];

                    data.StartIndex = 14;  // skip MAC (6 bytes) x 2 + packet type (2 bytes)

					file.Read(data.Data,0,(int)PItem.CaptureLength);
					data.TimeStamp = BaseTime + new TimeSpan((long)PItem.Seconds * 10000000 + (long)PItem.MicroSeconds * 10);
					data.Length = PItem.CaptureLength;

                    if (FileFormat == Format.SUNSNOOP)
                        file.Position += (packetRecordLength - PItem.CaptureLength - 24);

					// Increases the progress bar's value based on the size of 
					// the file currently being read.
                    snifferObj.Invoke(snifferObj.IncrementStepInProgressHandler, new object[] { (int)data.Length });

					OnDataReceived(data);
				}

				//Close the file after reading file
				file.Close();
			}
			catch(Exception e)
			{
				ErrorHandler( e.Message + "\r\n" + e.TargetSite + "\r\n" + e.StackTrace );
			}
			finally
			{
				bool stopped = tcp.TCPStopAnalyse();
				if(stopped)
				{
                    snifferObj.Invoke(snifferObj.UpdateUIControlsHandler);
				}
			}
		}

        public void StartDump(string capFile)
		{
			return;
		}

        public void StopDump()
        {
            return;
        }

        public string[] AdapterDescriptions
		{
			get
			{
				return new string[] {"File Capture"};
			}
		}

		public string[] AdapterNames
		{
			get
			{
				return new string[] {"File Capture"};
			}
		}

		public int AdapterIndex
		{
			set{}
			get
			{
				return 0;
			}
		}

        public string Filter
        {
            get { return null; }
            set { }
        }

		protected void ErrorHandler(string s)
		{
			if(Error !=null)
				Error(s);
		}
	}
}
