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
using System.Collections;
using System.Text;
using System.IO;

namespace DvtkDicomUnicodeConversion
{
    public class DicomUnicodeConverter
    {
        private String _characterSetDirectory = String.Empty;
        private ArrayList _specificCharacterSets = new ArrayList();
        private ArrayList _dvtkCharacterSets = new ArrayList();

        public DicomUnicodeConverter(String characterSetDirectory)
        {
            _characterSetDirectory = characterSetDirectory;
        }

        public bool Install(String specificCharacterSetsString)
        {
            bool result = true;
            String[] specificCharacterSets = null;
            try
            {
                // ensure the default character set value is set up
                // - if not explicitly defined
                if (specificCharacterSetsString == String.Empty)
                {
                    specificCharacterSets = "ISO_IR 6".Split('\\');
                }
                else
                {
                    specificCharacterSets = specificCharacterSetsString.Split('\\');
                    if (specificCharacterSets.Length == 0)
                    {
                        specificCharacterSets = "ISO_IR 6".Split('\\');
                    }
                    else if ((specificCharacterSets.Length > 1) &&
                        (specificCharacterSets[0] == String.Empty))
                    {
                        specificCharacterSets[0] = "ISO_IR 6";
                    }
                }

                // trim any padding spaces from the specific character sets values
                // - also check to see if the default repertoire with code extensions
                // has been defined
                bool defaultRepertoireDefined = false;
                for (int i = 0; i < specificCharacterSets.Length; i++)
                {
                    String localValue = specificCharacterSets[i].Trim();
                    _specificCharacterSets.Add(localValue);
                    if (localValue == "ISO 2022 IR 6")
                    {
                        defaultRepertoireDefined = true;
                    }
                }

                // add the default repertoire with code extensions if necessary
                if (defaultRepertoireDefined == false)
                {
                    _specificCharacterSets.Add("ISO 2022 IR 6");
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(_characterSetDirectory);
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    DvtkCharacterSet dvtkCharacterSet = new DvtkCharacterSet(fileInfo.FullName);
                    if (dvtkCharacterSet.Read() == true)
                    {
                        foreach (String characterSetName in _specificCharacterSets)
                        {
                            if (characterSetName == dvtkCharacterSet.CharacterSetName)
                            {
                                _dvtkCharacterSets.Add(dvtkCharacterSet);
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                result = false;
            }

            return result;
        }

        public UInt16[] Unicode(byte[] dicomString)
        {
            ArrayList localBuffer = new ArrayList();
            try
            {
                bool isMixedG0G1CharacterSet = false;
                DvtkCharacterSet specificCharacterSet0 = (DvtkCharacterSet)GetDvtkCharacterSet((String)_specificCharacterSets[0]);
                DvtkCharacterSet currentCharacterSet = (DvtkCharacterSet)GetDvtkCharacterSet((String)_specificCharacterSets[0]);
                for (int i = 0; i < dicomString.Length; i++)
                {
                    UInt16 dicomCode = 0;

                    // check for escape sequences
                    if (dicomString[i] == 0x1B)
                    {
                        byte[] escapeSequence = new byte[] { 0, 0, 0, 0 };
                        // get the escape sequence from the dicomString
                        // - try a 3 char escape sequence first
                        int localIndex = i;
                        escapeSequence[0] = dicomString[i++];
                        escapeSequence[1] = dicomString[i++];
                        escapeSequence[2] = dicomString[i++];

                        // check if the escape sequence matches a Dvtk character set escape sequence
                        DvtkCharacterSet nextCharacterSet = GetDvtkCharacterSet(escapeSequence);
                        if (nextCharacterSet == null)
                        {
                            // try with the 4 char escape sequence
                            escapeSequence[3] = dicomString[i++];
                            nextCharacterSet = GetDvtkCharacterSet(escapeSequence);
                        }
                        if (nextCharacterSet != null)
                        {
                            // switch the current character set to the next one
                            currentCharacterSet = nextCharacterSet;

                            // set the isMixed boolean
                            // - this indicates if the current Character Set can be mixed
                            // with the specificCharacterSet0 in the G0 code space and 
                            // the current Character Set in the G1 code space.
                            isMixedG0G1CharacterSet = nextCharacterSet.IsMixed;
                        }
                        else
                        {
                            // failed to translate the escape sequence
                            // - reset the index to the beginning of the escape sequence and continue
                            // - escape sequence will simply be translated into unicode in the current character set
                            i = localIndex;
                        }
                        if (i == dicomString.Length) continue;
                    }

                    // check if the escaped character set is a mixed one in the G1 code space that requires
                    // the use of the specificCharacterSet0 in the G0 code space (ie 7 bit chars)
                    if ((isMixedG0G1CharacterSet == true) &&
                        (dicomString[i] < 128))
                    {
                        // create single byte dicom code
                        dicomCode = (UInt16)dicomString[i];

                        // add the converted Unicode char from the specificCharacterSet0 character set
                        localBuffer.Add(specificCharacterSet0.DicomToUnicode(dicomCode));
                    }
                    else if (currentCharacterSet.IsMultiByte == true)
                    {
                        // check on the number of bytes per character in the current character set
                        // create multi-byte dicom code
                        dicomCode = (UInt16)((dicomString[i] * 256) + dicomString[i + 1]);
                        i++;

                        // add the converted Unicode multi-byte char from the current character set
                        localBuffer.Add(currentCharacterSet.DicomToUnicode(dicomCode));
                    }
                    else
                    {
                        // create single byte dicom code
                        dicomCode = (UInt16)dicomString[i];

                        // add the converted Unicode char from the current character set
                        localBuffer.Add(currentCharacterSet.DicomToUnicode(dicomCode));
                    }
                }
            }
            catch (System.Exception)
            {
            }

            // convert the local buffer into the returned UInt16 array
            UInt16[] unicode = new UInt16[localBuffer.Count];
            for (int i = 0; i < localBuffer.Count; i++)
            {
                unicode[i] = (UInt16)localBuffer[i];
            }

            return unicode;
        }

        public String UnicodeAsXml(UInt16[] unicodeString)
        {
            String xmlUnicode = String.Empty;

            for (int i = 0; i < unicodeString.Length; i++)
            {
                // check for the ESCape character - it should not appear!
                if (unicodeString[i] == 0x1B)
                {
                    xmlUnicode += "[ESC]";
                }
                else if (unicodeString[i] == 0x00)
                {
                    // check for the NULl character - it should not appear!
                    xmlUnicode += "[NUL]";
                }
                else
                {
                    xmlUnicode += String.Format("&#x{0};", unicodeString[i].ToString("X"));
                }
            }

            return xmlUnicode;
        }

        private DvtkCharacterSet GetDvtkCharacterSet(String characterSetName)
        {
            String localCharacterSetName = characterSetName.Trim();
            DvtkCharacterSet dvtkCharacterSet = null;
            foreach (DvtkCharacterSet localCharacterSet in _dvtkCharacterSets)
            {
                // if name matches then character set has been found
                if (localCharacterSet.CharacterSetName == localCharacterSetName)
                {
                    dvtkCharacterSet = localCharacterSet;
                    break;
                }
            }
            return dvtkCharacterSet;
        }

        private DvtkCharacterSet GetDvtkCharacterSet(byte[] escapeSequence)
        {
            // fix for the Japanese character set ISO 2022 IR 13 which has two
            // different escape sequences for the GO and G1 code space
            // - check for the escape sequence for the G1 code space
            if ((escapeSequence[0] == 0x1B) &&
                (escapeSequence[1] == 0x29) &&
                (escapeSequence[2] == 0x49) &&
                (escapeSequence[3] == 0))
            {
                // swap this to the escape sequence for the G0 code space
                escapeSequence[1] = 0x28;
                escapeSequence[2] = 0x4A;
            }

            DvtkCharacterSet dvtkCharacterSet = null;
            foreach (DvtkCharacterSet localCharacterSet in _dvtkCharacterSets)
            {
                // if escape sequence matches then character set has been found
                if ((localCharacterSet.Escape1[0] == escapeSequence[0]) &&
                    (localCharacterSet.Escape1[1] == escapeSequence[1]) &&
                    (localCharacterSet.Escape1[2] == escapeSequence[2]) &&
                    (localCharacterSet.Escape1[3] == escapeSequence[3]))
                {
                    dvtkCharacterSet = localCharacterSet;
                    break;
                }
            }
            return dvtkCharacterSet;
        }

    }
}
