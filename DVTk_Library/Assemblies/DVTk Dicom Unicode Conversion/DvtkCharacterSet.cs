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
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DvtkDicomUnicodeConversion
{
    public class DvtkCharacterSet
    {
        private String _filename = String.Empty;
        private String _eye = String.Empty;
        private String _characterSetname = String.Empty;
        private byte[] _escape1 = new byte[4];
        private byte[] _escape2 = new byte[4];
        private int _multibyte = 0;
        private int _mixed = 0;
        private int _reserved = 0;
        private int _startValue = 0;
        private int _endValue = 0;
        private UInt16[] _table = null;

        public DvtkCharacterSet(String filename)
        {
            _filename = filename;
        }

        public bool Read()
        {
            bool loaded = true;
            FileStream fs = new FileStream(_filename, FileMode.Open);
            try
            {
                int fileLength = (int)fs.Length + 10;
                byte[] buffer = new byte[fileLength];
                int length = fs.Read(buffer, 0, fileLength);
                int index = 0;
                _eye = this.CopyAsString(buffer, index, 8);
                index += 8;
                _characterSetname = this.CopyAsString(buffer, index, 20);
                index += 20;
                for (int i = 0; i < 4; i++)
                {
                    _escape1[i] = buffer[index++];
                }
                for (int i = 0; i < 4; i++)
                {
                    _escape2[i] = buffer[index++];
                }
                _multibyte = CopyAsInt(buffer, index);
                index += 4;
                _mixed = CopyAsInt(buffer, index);
                index += 4;
                _reserved = CopyAsInt(buffer, index);
                index += 4;
                _startValue = CopyAsInt(buffer, index);
                index += 4;
                _endValue = CopyAsInt(buffer, index);
                index += 4;
                int noTableEntries = (length - index) / 2;
                _table = new UInt16[noTableEntries];
                for (int i = 0; i < noTableEntries; i++)
                {
                    _table[i] = CopyAsUInt16(buffer, index);
                    index += 2;
                }
            }
            catch (System.Exception)
            {
                loaded = false;
            }
            fs.Close();

            return loaded;
        }

        public String CharacterSetName
        {
            get
            {
                return _characterSetname;
            }
        }

        public bool IsMultiByte
        {
            get
            {
                return (_multibyte == 0 ? false : true);
            }
        }

        public bool IsMixed
        {
            get
            {
                return (_mixed == 0 ? false : true);
            }
        }

        public byte[] Escape1
        {
            get
            {
                return _escape1;
            }

        }

        public byte[] Escape2
        {
            get
            {
                return _escape2;
            }

        }

        public UInt16 DicomToUnicode(UInt16 dicomCode)
        {
            UInt16 unicode = 0;
            int index = dicomCode - _startValue;
            try
            {
                unicode = _table[index];
            }
            catch (System.Exception)
            {
            }
            return unicode;
        }

        public UInt16 UnicodeToDicom(UInt16 unicode)
        {
            UInt16 dicomCode = 0;
            for (int i = 0; i < _table.Length; i++)
            {
                if (_table[i] == unicode)
                {
                    dicomCode = (UInt16)(_startValue + i);
                    break;
                }
            }
            return dicomCode;
        }

        public void Display()
        {
            Console.WriteLine("Filename: {0}", _filename);
            Console.WriteLine("Eye: {0}", _eye);
            Console.WriteLine("Character Set Name: {0}", _characterSetname);
            Console.WriteLine("Escape 1: {0} {1} {2} {3}", _escape1[0].ToString("X"), _escape1[1].ToString("X"), _escape1[2].ToString("X"), _escape1[3].ToString("X"));
            Console.WriteLine("Escape 2: {0} {1} {2} {3}", _escape2[0].ToString("X"), _escape2[1].ToString("X"), _escape2[2].ToString("X"), _escape2[3].ToString("X"));
            Console.WriteLine("Multibyte: {0}", _multibyte);
            Console.WriteLine("Mixed: {0}", _mixed);
            Console.WriteLine("Reserved: {0}", _reserved);
            Console.WriteLine("Start Value: {0} {1}", _startValue, _startValue.ToString("X"));
            Console.WriteLine("End Value: {0} {1}", _endValue, _endValue.ToString("X"));
            Console.WriteLine("Table Length: {0} {1}", _table.Length, _table.Length.ToString("X"));
            for (int i = 0; i < _table.Length; i++)
            {
                int offset = _startValue + i;
                Console.Write("{0}:{1} ", offset.ToString("X"), _table[i].ToString("X"));
            }
            Console.WriteLine("");
        }

        public void Display(StreamWriter sw)
        {
            sw.WriteLine("Filename: {0}", _filename);
            sw.WriteLine("Eye: {0}", _eye);
            sw.WriteLine("Character Set Name: {0}", _characterSetname);
            sw.WriteLine("Escape 1: {0} {1} {2} {3}", _escape1[0].ToString("X"), _escape1[1].ToString("X"), _escape1[2].ToString("X"), _escape1[3].ToString("X"));
            sw.WriteLine("Escape 2: {0} {1} {2} {3}", _escape2[0].ToString("X"), _escape2[1].ToString("X"), _escape2[2].ToString("X"), _escape2[3].ToString("X"));
            sw.WriteLine("Multibyte: {0}", _multibyte);
            sw.WriteLine("Mixed: {0}", _mixed);
            sw.WriteLine("Reserved: {0}", _reserved);
            sw.WriteLine("Start Value: {0} {1}", _startValue, _startValue.ToString("X"));
            sw.WriteLine("End Value: {0} {1}", _endValue, _endValue.ToString("X"));
            sw.WriteLine("Table Length: {0} {1}", _table.Length, _table.Length.ToString("X"));
            sw.WriteLine("");
        }

        public void WriteXML(StreamWriter sw)
        {
            for (int i = 0; i < _table.Length; i++)
            {
                if (_table[i] != 0)
                {
                    int offset = _startValue + i;
                    sw.Write("&#x{0};", _table[i].ToString("X"));
                }
            }
        }

        private String CopyAsString(byte[] buffer, int offset, int length)
        {
            String localString = String.Empty;
            for (int i = 0; i < length; i++)
            {
                if (buffer[offset + i] == 0x00) break;
                localString += (char)buffer[offset + i];
            }
            return localString;
        }

        private int CopyAsInt(byte[] buffer, int offset)
        {
            int localInt = 0;
            for (int i = 3; i >= 0; i--)
            {
                localInt = (localInt * 256) + (int)buffer[offset + i];
            }
            return localInt;
        }

        private UInt16 CopyAsUInt16(byte[] buffer, int offset)
        {
            UInt16 localUInt16 = 0;
            for (int i = 1; i >= 0; i--)
            {
                localUInt16 = (UInt16)((localUInt16 * 256) + buffer[offset + i]);
            }
            return localUInt16;
        }
    }
}
