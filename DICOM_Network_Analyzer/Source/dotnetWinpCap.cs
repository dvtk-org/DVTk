// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
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
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sniffer
{
    /// <summary>
    /// Free to use / modify as long as this summary note is attached
    /// to any derived source.
    /// For binaries, original author acknowledgments are encouraged to 
    /// be placed in suitables places e.g. the About box or release notes
    /// Author: Victor Tan (emailvictor@gmail.com)
    /// </summary>
    public class dotnetWinpCap
    {
        public class AlreadyOpenException : System.Exception
        {
            public override string Message
            {
                get
                {
                    return "Device attached to object already open. Close first before reopening";
                }
            }
        }

        System.Threading.Thread ListenThread = null;
        bool disposed = false;
        private string fname = "";
        private int maxb = 0;
        private int maxp = 0;
        private bool m_islistening = false;
        private bool m_isopen = false;
        private bool isDumpingStopped = false;
        private string m_attachedDevice = null;

        private delegate void packet_handler(IntPtr param, IntPtr header, IntPtr pkt_data);
        //private delegate void EOF();

        public enum PCAP_NEXT_EX_STATE
        {
            SUCCESS = 1,
            TIMEOUT = 0,
            ERROR = -1,
            EOF = -2,
            UNKNOWN = -3
        }

        public enum PCAP_SRC_TYPE
        {
            PCAP_SRC_FILE = 2,
            PCAP_SRC_IFLOCAL = 3,
            PCAP_SRC_IFREMOTE = 4
        }

        // this is the pointer to an adapter of the instance
        private IntPtr pcap_t = IntPtr.Zero;
        private IntPtr dumper = IntPtr.Zero;

        public delegate void ReceivePacket(object sender, PacketHeader p, byte[] s);
        public event ReceivePacket OnReceivePacket;

        private delegate void DumpEnded(object sender); // not used yet
        private event DumpEnded OnDumpEnded; // not used yet
        public delegate void FileEnded(object sender);
        public event FileEnded OnFileEnded;

        private delegate void ReceivePacketInternal(object sender, IntPtr header, IntPtr data);
        private event ReceivePacketInternal OnReceivePacketInternal;

        private StringBuilder errbuf = new StringBuilder(256);

        [DllImport("wpcap.dll")]
        private static extern int pcap_findalldevs(ref IntPtr devicelist,
            StringBuilder errbuf);
        [DllImport("wpcap.dll")]
        private static extern int pcap_setbuff(IntPtr p, int kernelbufferbytes);
        [DllImport("wpcap.dll")]
        private static extern int pcap_live_dump(IntPtr p, string filename, int maxsize, int maxpacks);
        [DllImport("wpcap.dll")]
        private static extern int pcap_live_dump_ended(IntPtr p, int sync);
        [DllImport("wpcap.dll")]
        private static extern IntPtr pcap_dump_open(IntPtr p, string filename);
        [DllImport("wpcap.dll")]
        private static extern void pcap_dump(IntPtr dumper, IntPtr h, IntPtr data);
        [DllImport("wpcap.dll")]
        private static extern void pcap_dump_close(IntPtr dumper);

        [DllImport("wpcap.dll")]
        private static extern int pcap_sendpacket(IntPtr p, byte[] buff, int size);


        // this has been deprecated since wpcap3.0
        //[DllImport("wpcap.dll")]
        //private static extern int pcap_loop(IntPtr p, int cnt, packet_handler callback, IntPtr user);

        [DllImport("wpcap.dll")]
        private static extern IntPtr pcap_open_live(string device, int snaplen,
            int promisc, int to_ms, StringBuilder ebuf);
        [DllImport("wpcap.dll")]
        private static extern byte[] pcap_next(IntPtr p, IntPtr pkt_header);
        [DllImport("wpcap.dll")]
        private static extern int pcap_setmintocopy(IntPtr p, int size);

        [DllImport("wpcap.dll")]
        private static extern void pcap_freealldevs(IntPtr devicelist);
        [DllImport("wpcap.dll")]
        private static extern IntPtr pcap_open(string source, int snaplen,
            int flags, int read_timeout, IntPtr auth, StringBuilder errbuf);
        [DllImport("wpcap.dll")]
        private static extern int pcap_next_ex(IntPtr p, ref IntPtr pkt_header, ref IntPtr packetdata);

        [DllImport("wpcap.dll")]
        private static extern void pcap_close(IntPtr p);

        [DllImport("wpcap.dll")]
        private static extern void pcap_createsrcstr(StringBuilder source, int type, IntPtr host,
            IntPtr port, IntPtr name, StringBuilder error);

        [DllImport("wpcap.dll")]
        private static extern IntPtr pcap_open_offline(string fname, StringBuilder errbuf);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string dllName);

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct bpf_program
        {
            public uint bf_len;
            public IntPtr bf_insns;
        }

        [DllImport("wpcap.dll")]
        private static extern int pcap_compile(IntPtr p,
              IntPtr fp, string str, int optimize, uint netmask);

        [DllImport("wpcap.dll")]
        private static extern int pcap_setfilter(IntPtr p, IntPtr fp);

        [DllImport("wpcap.dll")]
        private static extern void pcap_freecode(IntPtr fp);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct pcap_pkthdr
        {
            public timeval ts;
            public System.UInt32 caplen;
            public System.UInt32 len;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct timeval
        {
            public System.UInt32 tv_sec; // sec
            public System.UInt32 tv_usec; // microsec
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct pcap_rmtauth
        {
            int type;
            string username;
            string password;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct in_addr
        {
            public char b1;
            public char b2;
            public char b3;
            public char b4;

            public ushort w1;
            public ushort w2;
            public ulong addr;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct sockaddr
        {
            public short family;
            public ushort port;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct pcap_addr
        {
            public IntPtr next;
            public IntPtr addr;
            public IntPtr netmask;
            public IntPtr broadaddr;
            public IntPtr dstaddr;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct pcap_if
        {
            public IntPtr next;
            public string name;
            public string description;
            public IntPtr addresses;
            public uint flags;
        }

        public static ArrayList FindAllDevs()
        {
            ArrayList devarraylist = new ArrayList();
            pcap_if devlist;
            devlist.addresses = IntPtr.Zero;
            devlist.description = (new StringBuilder()).ToString();
            devlist.flags = 0;
            devlist.name = (new StringBuilder()).ToString();
            devlist.next = IntPtr.Zero;

            IntPtr tmpptr = IntPtr.Zero;
            StringBuilder err = new StringBuilder(256);

            IntPtr head = IntPtr.Zero;

            if (LoadLibrary("wpcap.dll") == IntPtr.Zero)
            {
                MessageBox.Show(null, "Please install the Npcap driver, which can be found at https://nmap.org/npcap/ .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (pcap_findalldevs(ref tmpptr, err) == -1)
            {
                return null;
            }
            else
            {
                head = tmpptr;
                while (tmpptr.ToInt32() != 0)
                {
                    Device tmpDevice = new Device();
                    devarraylist.Add(tmpDevice);
                    devlist = (pcap_if)Marshal.PtrToStructure(tmpptr, typeof(pcap_if));
                    tmpDevice.Name = devlist.name;
                    tmpDevice.Description = devlist.description;
                    if (devlist.addresses.ToInt32() != 0)
                    {
                        pcap_addr addr = (pcap_addr)Marshal.PtrToStructure(devlist.addresses, typeof(pcap_addr));
                        if (addr.addr.ToInt32() != 0)
                        {
                            sockaddr ip = (sockaddr)Marshal.PtrToStructure(addr.addr, typeof(sockaddr));
                            tmpDevice.Address = ip.addr[0].ToString() + "." + ip.addr[1].ToString() + "." + ip.addr[2].ToString() + "." + ip.addr[3].ToString();
                        }
                        if (addr.netmask.ToInt32() != 0)
                        {
                            sockaddr netmask = (sockaddr)Marshal.PtrToStructure(addr.netmask, typeof(sockaddr));
                            tmpDevice.Netmask = netmask.addr[0].ToString() + "." + netmask.addr[1].ToString() + "." + netmask.addr[2].ToString() + "." + netmask.addr[3].ToString();
                        }
                    }
                    tmpptr = devlist.next;
                }

                pcap_freealldevs(head);
            }
            return devarraylist;
        }

        public string AttachedDevice
        {
            get
            {
                return m_attachedDevice;
            }
        }

        public void OpenOffline(string filename)
        {
            this.pcap_t = pcap_open_offline(filename, errbuf);

            if (pcap_t.ToInt32() == 0)
            {
                throw new Exception("Failed to open file: " + LastError);
            }
        }

        public string CreateSourceString(string filename)
        {
            StringBuilder source = new StringBuilder(256);
            pcap_createsrcstr(source, (int)(PCAP_SRC_TYPE.PCAP_SRC_FILE), IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero, errbuf);

            return source.ToString();
        }

        public void SetFilter(string filter)
        {
            Debug.Assert(pcap_t != IntPtr.Zero);
            if (filter != null && filter != "")
            {
                IntPtr fp = Marshal.AllocHGlobal(12);  // 8 for Win32 but might need to be 12 on a 64 bit machine?
                int result;
                result = pcap_compile(pcap_t, fp, filter, 1, 0);
                if (result != 0)
                    throw new Exception("Invalid filter");
                result = pcap_setfilter(pcap_t, fp);
                if (result != 0)
                    throw new Exception("Failed to apply filter");
                pcap_freecode(fp);

            }
        }

        // only open promiscuous for local for now
        /// <summary>
        /// Opens a device for capture.
        /// </summary>
        /// <param name="source">Can be one of the following:
        /// file://filename OR
        /// rpcap://adaptername</param>
        /// <param name="snaplen">Length of packet to be retained for each packet received</param>
        /// <param name="flags">1=Promiscuous, 0=Non-promiscuous</param>
        /// <param name="read_timeout">Time in milliseconds to wait before 
        /// ReadNext returns.</param>
        /// <returns>true if opened successfully, false otherwise</returns>
        public bool Open(string source, int snaplen, int flags, int read_timeout)
        {
            if (pcap_t != IntPtr.Zero)
            {
                throw new dotnetWinpCap.AlreadyOpenException();

                // close current one
            }
            pcap_t = pcap_open(source, snaplen, flags, read_timeout, IntPtr.Zero, this.errbuf);
            if (pcap_t.ToInt32() != 0)
            {
                this.m_isopen = true;
                m_attachedDevice = source;
                return true;
            }
            else
            {
                this.m_isopen = false;
                m_attachedDevice = null;
                return false;
            }
        }

        //private void Loop() 
        //{
        //    callback=new packet_handler(LoopCallback);
        //    // might have to do something with this call back
        //    // so that it doesn't get GC'ed when it goes out of scope!
        //    // read one of the saved articles
        //    IntPtr h=IntPtr.Zero;
        //    HandleRef hr=new HandleRef(callback,h );
        //    pcap_loop(this.pcap_t, 0, callback, IntPtr.Zero);
        //}


        //private void LoopCallback(IntPtr param, IntPtr header, IntPtr pkt_data) 
        //{
        //    string sparam=Marshal.PtrToStringAnsi(param);
        //    pcap_pkthdr h=(pcap_pkthdr)Marshal.PtrToStructure(header, typeof(pcap_pkthdr));
        //    byte[] packetdata=new byte[h.caplen];
        //    Marshal.Copy(pkt_data,packetdata,0,(int)h.caplen);

        //    string spkt_data=Marshal.PtrToStringAnsi(pkt_data);

        //}


        private bool OpenLive(string source, int snaplen, int promisc, int to_ms)
        {
            this.pcap_t = pcap_open_live(source, snaplen, promisc, to_ms, this.errbuf);
            if (pcap_t.ToInt32() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // out param so that we can change assign p and s to new mem locations,
        // since ReadNext calls 'new' to p and s.
        /// <summary>
        /// Read the next available packet from the capture driver.
        /// Returns if timeout specified in Open elapsed, or a packet has been
        /// forwarded by the capture driver.
        /// </summary>
        /// <param name="p">Packet header of the packet returned</param>
        /// <param name="packet_data">Packet data of the packet returned</param>
        /// <returns></returns>
        private PCAP_NEXT_EX_STATE ReadNextInternal(out PacketHeader p, out byte[] packet_data, out IntPtr pkthdr, out IntPtr pktdata)
        {
            pkthdr = IntPtr.Zero;
            pktdata = IntPtr.Zero;
            p = null;
            packet_data = null;
            if (this.pcap_t.ToInt32() == 0)
            {
                this.errbuf = new StringBuilder("No adapter is currently open");
                return PCAP_NEXT_EX_STATE.ERROR;
            }

            int ret = pcap_next_ex(this.pcap_t, ref pkthdr, ref pktdata);
            //System.Diagnostics.Debug.WriteLine(this.pcap_t);
            if (ret == 0)
            {
                return PCAP_NEXT_EX_STATE.TIMEOUT;
            }
            else if (ret == 1)
            {
                pcap_pkthdr packetheader = (pcap_pkthdr)Marshal.PtrToStructure(pkthdr,
                    typeof(pcap_pkthdr));
                p = new PacketHeader();
                p.Caplength = (int)packetheader.caplen;
                p.Length = (int)packetheader.len;
                p.ts = packetheader.ts;
                packet_data = new byte[p.Length];
                Marshal.Copy(pktdata, packet_data, 0, p.Caplength);
                return PCAP_NEXT_EX_STATE.SUCCESS;
            }
            else if (ret == -1)
            {
                return PCAP_NEXT_EX_STATE.ERROR;
            }
            else if (ret == -2)
            {
                return PCAP_NEXT_EX_STATE.EOF;
            }
            else
            {
                return PCAP_NEXT_EX_STATE.UNKNOWN;
            }
        }

        /// <summary>
        /// Read the next available packet from the capture driver.
        /// Returns if timeout specified in Open elapsed, or a packet has been
        /// forwarded by the capture driver.
        /// </summary>
        /// <param name="p">Packet header of the packet returned</param>
        /// <param name="packet_data">Packet data of the packet returned</param>
        /// <returns></returns>
        public PCAP_NEXT_EX_STATE ReadNextInternal(out PacketHeader p, out byte[] packet_data)
        {
            IntPtr dummy;
            return this.ReadNextInternal(out p, out packet_data, out dummy, out dummy);
        }

        public bool SendPacket(byte[] packet_data)
        {
            int i = pcap_sendpacket(this.pcap_t, packet_data, packet_data.Length);
            if (i == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MonitorDump()
        {
            // a non-zero 2nd input means it'll block till 
            // the dump returns
            if (pcap_live_dump_ended(this.pcap_t, 1) != 0)
            {
                if (this.OnDumpEnded != null)
                {
                    this.OnDumpEnded(this);
                }
            }
        }

        private void DumpPacket(object sender, IntPtr header, IntPtr data)
        {
            if (dumper != IntPtr.Zero)
            {
                pcap_dump(this.dumper, header, data);
            }
        }

        public void StopDump()
        {
            if (!isDumpingStopped)
            {
                this.OnReceivePacketInternal -= new dotnetWinpCap.ReceivePacketInternal(DumpPacket);
                if (this.dumper != IntPtr.Zero)
                {
                    pcap_dump_close(this.dumper);
                    dumper = IntPtr.Zero;
                }
                isDumpingStopped = true;
            }
        }

        public bool StartDump(string filename)
        {
            // open file
            if (pcap_t != IntPtr.Zero)
            {
                try
                {
                    this.dumper = pcap_dump_open(this.pcap_t, filename);
                }
                catch
                {
                    return false;
                }
                // file open successful
                // attach DumpPacket to ReceivePacket event
                this.OnReceivePacketInternal += new dotnetWinpCap.ReceivePacketInternal(DumpPacket);
                isDumpingStopped = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Doesn't work yet
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="maxbytes"></param>
        /// <param name="maxpackets"></param>
        /// <returns></returns>
        private bool StartLiveDump(string filename, int maxbytes, int maxpackets)
        {
            //IntPtr str=Marshal.StringToBSTR(filename);
            fname = filename;
            maxb = maxbytes;
            maxp = maxpackets;

            if (pcap_live_dump(this.pcap_t, fname, maxb, maxp) == 0)
            {
                System.Threading.Thread th = new Thread(new ThreadStart(MonitorDump));
                return true;
            }
            else
            {
                return false;
            }
        }

        // Default unknown
        /// <summary>
        /// Sets the minimum number of bytes in the kernel buffer before
        /// triggering the OnReceivePacket event, or the minimum number of 
        /// bytes in the kernel buffer before a packet is being passed to
        /// ReadNext
        /// </summary>
        /// <param name="size">Size in bytes</param>
        /// <returns></returns>
        public bool SetMinToCopy(int size)
        {
            if (pcap_setmintocopy(this.pcap_t, size) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ReadNextLoop()
        {
            // enter into infinite loop of listening
            while (true)
            {
                // only really readnext if at least one delegate is attached to event
                PacketHeader p = null;
                byte[] s = null;
                IntPtr internalheader;
                IntPtr internaldata;
                PCAP_NEXT_EX_STATE state = ReadNextInternal(out p, out s, out internalheader, out internaldata);
                if (state == PCAP_NEXT_EX_STATE.SUCCESS)
                {
                    if (OnReceivePacket != null)
                    {
                        this.OnReceivePacket(this, p, s);
                    }
                    if (this.OnReceivePacketInternal != null)
                    {
                        this.OnReceivePacketInternal(this, internalheader, internaldata);
                    }
                }
                if (state == PCAP_NEXT_EX_STATE.EOF)
                {
                    if (this.OnFileEnded != null)
                    {
                        this.OnFileEnded(this);
                    }
                    return;  // we're finished here
                }
                if (!IsListening)
                    return;
            }
        }

        /// <summary>
        /// Sets the kernel buffer size
        /// </summary>
        /// <param name="bytes">Kernel buffer size in number of bytes. Default is 1MB</param>
        /// <returns></returns>
        public bool SetKernelBuffer(int bytes)
        {
            if (pcap_setbuff(this.pcap_t, bytes) != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the most recent error message returned by capture driver.
        /// </summary>
        public string LastError
        {
            get
            {
                return errbuf.ToString();
            }
        }

        // this if for threading start of interface listening\
        /// <summary>
        /// Starts capturing of packets
        /// </summary>
        public void StartListen()
        {
            StopListen();

            ListenThread = new Thread(new ThreadStart(ReadNextLoop));
            ListenThread.Priority = ThreadPriority.AboveNormal;

            //System.Diagnostics.Debug.WriteLine("starting listening thread. . .");
            this.m_islistening = true;
            ListenThread.Start();
        }

        /// <summary>
        /// Stops capturing of packets
        /// </summary>
        public void StopListen()
        {
            this.m_islistening = false;
            if (ListenThread != null)
            {
                ListenThread.Join(2000);  // give it 2 seconds to finish nicely
                if (ListenThread.IsAlive)
                {
                    ListenThread.Abort();
                }
                ListenThread = null;
            }
        }

        /// <summary>
        /// Indicates whether capture has been started
        /// </summary>
        public bool IsListening
        {
            get
            {
                return this.m_islistening;
            }
        }


        public bool IsOpen
        {
            get
            {
                return this.m_isopen;
            }
        }

        // clean up all resources, including unmanaged ones.
        /// <summary>
        /// Cleans up all resources associated with this instance.
        /// </summary>
        public void Close()
        {
            this.StopDump();

            if (this.IsListening)
            {
                this.StopListen();
            }
            this.m_isopen = false;
            this.m_attachedDevice = null;
            if (pcap_t != IntPtr.Zero)
            {
                pcap_close(pcap_t); // this is very important to release handles!!
                pcap_t = IntPtr.Zero;
            }
        }

        public dotnetWinpCap()
        {
            // 
            // TODO: Add constructor logic here
            //
        }

        // implementation of IDisposable
        private void Dispose()
        {
            System.Diagnostics.Debug.WriteLine("disposing " + this.ToString());
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("privately disposing " + this.ToString());
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (ListenThread != null)
                    {
                        if (ListenThread.IsAlive)
                        {
                            ListenThread.Abort();
                        }
                        ListenThread = null;
                    }
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.
                if (pcap_t != IntPtr.Zero)
                {
                    pcap_close(pcap_t); // this is very important to release handles!!
                    pcap_t = IntPtr.Zero;
                }
            }
            disposed = true;
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~dotnetWinpCap()
        {
            System.Diagnostics.Debug.WriteLine("destroying " + this.ToString());
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
    }
}

