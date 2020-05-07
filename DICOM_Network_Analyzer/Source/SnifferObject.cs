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
using System.Collections;
using System.Windows.Forms;
using SnifferUI;

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
	public class SnifferObject : SnifferInterface
	{
		public event ErrorEvent Error;
		public event DataReceivedEvent DataReceived;

        private dotnetWinpCap wincap = new dotnetWinpCap();

        public int m_index = 0;
        string filename;
        int nrOfCapturedPackets = 0;
        ArrayList deviceList = new ArrayList();

        string m_filter = null;

        //bool isFile;
        ////string capFilename;
        //FileStream fsTempCapFile = null;
        //TCP.TCPParser tcp;
        //SnifferUI.DICOMSniffer snifferObj;

        public string Filter
        {
            get { return m_filter; }
            set { m_filter = value; }
        }

        public SnifferObject()
        {
            wincap.OnReceivePacket += new dotnetWinpCap.ReceivePacket(wincap_OnReceivePacket);

            // Find only Devices which can be used for capturing
            ArrayList allDevices = dotnetWinpCap.FindAllDevs();

            if (allDevices != null)
            {
                foreach (Device dev in allDevices)
                {
                    if (dev.Netmask != null)
                        deviceList.Add(dev);
                }
            }
        }

        //public SnifferObject(SnifferUI.DICOMSniffer snifferobject, string file, TCP.TCPParser Tcp)
        //{           
        //    wincap.OnReceivePacket += new dotnetWinpCap.ReceivePacket(wincap_OnReceivePacket);

        //    wincap.OnFileEnded += new dotnetWinpCap.FileEnded(wincap_OnFileEnded);
        //    //capFilename = File;// wincap.CreateSourceString(File);
        //    isFile = true;
        //    tcp = Tcp;
        //    snifferObj = snifferobject;

        //    // Create the temp capture file
        //    fsTempCapFile = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
        //    fsTempCapFile.Close();

        //    // Find only Devices which can be used for capturing
        //    //ArrayList allDevices = dotnetWinpCap.FindAllDevs();
        //    //foreach (Device dev in allDevices)
        //    //{
        //    //    if(dev.Netmask != null)
        //    //        deviceList.Add(dev);
        //    //}
        //}

        //void wincap_OnFileEnded(object sender)
        //{
        //    Stop();
        //    bool stopped = tcp.TCPStopAnalyse(); // force analysis to happen
        //    if (stopped)
        //    {
        //        snifferObj.Invoke(snifferObj.UpdateUIControlsHandler);
        //    }
        //}

        public int CapturedPackets
        {
            get
            {
                // no need to lock - this is informational only
                return nrOfCapturedPackets;
            }
        }

        public void Start()
		{
            if (wincap.IsListening)
                throw new Exception("Sniffer thread is already running");

            //int bufferSizeInBytes = (SnifferUI.UserOptions.kernelBufferSize) * 1024 * 1024;
            int bufferSizeInBytes = 100 * 1024 * 1024;
            wincap.Open(filename, 65536, 1, 1000);
            wincap.SetKernelBuffer(bufferSizeInBytes);
            
            if (Filter != null)
                wincap.SetFilter(Filter);

            nrOfCapturedPackets = 0;

            wincap.StartListen();            
		}

		public void Stop()
		{
            if (wincap.IsListening)
            {
                wincap.StopListen();

                Filter = null;

                wincap.Close();                
            }            
		}

        void wincap_OnReceivePacket(object sender, PacketHeader p, byte[] s)
        {
            if (p.Caplength >= p.Length)  // no point in trying to handle incomplete packets
            {
                PacketInfo data = new PacketInfo();

                data.Data = s;
                data.TimeStamp = p.TimeStamp;
                data.Length = (uint)s.Length;
                data.StartIndex = 14;

                nrOfCapturedPackets++;

                if (DataReceived != null)
                    DataReceived(data);
            }
        }

        public void StartDump(string capFile)
        {
            wincap.StartDump(capFile);
        }

        public void StopDump()
        {
            wincap.StopDump();
        }
		
        public string[] AdapterDescriptions
		{
			get
			{
                string[] result = null;
                if (deviceList.Count == 0)
                    result = new string []{"No network adapters detected on the machine."};
                else
                {
                    result = new string[deviceList.Count];
                    int i = 0;
                    foreach (Device dev in deviceList)
                    {
                        result[i++] = dev.Description;
                    }
                }
                return result;
			}
		}

		public string[] AdapterNames
		{
			get
			{
                string[] result = null;
                if (deviceList.Count == 0)
                    result = new string[] {"No network adapters detected on the machine."};
                else
                {
                    result = new string[deviceList.Count];
                    int i = 0;
                    foreach (Device dev in deviceList)
                    {
                        result[i++] = dev.Name;
                    }
                }
                return result;
			}
		}

		public int AdapterIndex
		{
			set
			{
                m_index = value;
                filename = AdapterNames[m_index];
			}
			get
			{
                return m_index; 
			}
		}

        void ErrorHandler(string s)
		{
			if(Error !=null)
				Error(s);
		}
	}
}
