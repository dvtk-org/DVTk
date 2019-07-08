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
using System.IO;

namespace Dvtk.Hl7.Messages
{
    /// <summary>
    /// Class that reads a directory for files with a given extension and returns HL7 messages.
    /// </summary>
    public class Hl7DirectoryReader
    {
        private FileInfo[] _fileInfo = null;
        private int _iterator = 0;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="directory">Full directory name containing HL7 message files.</param>
        /// <param name="fileExtension">Hl7 message file extension.</param>
        public Hl7DirectoryReader(String directory, String fileExtension)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            _fileInfo = directoryInfo.GetFiles(fileExtension);
        }

        /// <summary>
        /// Property - Count
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                if (_fileInfo != null)
                {
                    count = _fileInfo.Length;
                }
                return count;
            }
        }

        /// <summary>
        /// Get the HL7 message read from the indexed message file.
        /// </summary>
        /// <param name="index">Zero based index.</param>
        /// <returns>HL7 message read from indexed file.</returns>
        public Hl7Message GetHl7Message(int index)
        {
            Hl7Message hl7Message = null;

            if ((_fileInfo != null) &&
                (index < _fileInfo.Length))
            {
                String filename = _fileInfo[index].FullName;
                Hl7FileStream hl7FileStream = new Hl7FileStream();
                hl7Message = hl7FileStream.In(filename);
            }

            return hl7Message;
        }

        /// <summary>
        /// Get the next HL7 message read from the HL7 message directory.
        /// </summary>
        /// <returns>Next HL7 message read from message directory files.</returns>
        public Hl7Message GetNextHl7Message()
        {
            Hl7Message hl7Message = GetHl7Message(_iterator);
            if (hl7Message != null)
            {
                _iterator++;
            }

            return hl7Message;
        }
    }
}
