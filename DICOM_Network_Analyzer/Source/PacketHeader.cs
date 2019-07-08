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
    /// Free to use / modify as long as this summary note is attached
    /// to any derived source.
    /// For binaries, original author acknowledgments are encouraged to 
    /// be placed in suitables places e.g. the About box or release notes
    /// Author: Victor Tan (emailvictor@gmail.com)
    /// </summary>
    public class PacketHeader
    {
        public dotnetWinpCap.timeval ts;
        int caplen = 0;
        int len = 0;


        public int Caplength
        {
            get
            {
                return this.caplen;
            }
            set
            {
                this.caplen = value;
            }
        }

        public int Length
        {
            get
            {
                return this.len;
            }
            set
            {
                this.len = value;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                DateTime d = new DateTime(1970, 1, 1).AddSeconds(this.ts.tv_sec);
                d.AddMilliseconds(ts.tv_usec);
                return d;
            }
        }

        public PacketHeader(dotnetWinpCap.timeval ts, int caplen, int len)
        {
            this.ts = ts;
            this.caplen = caplen;
            this.len = len;
        }

        public PacketHeader()
        {
            // 
            // TODO: Add constructor logic here
            //
        }
    }
}

