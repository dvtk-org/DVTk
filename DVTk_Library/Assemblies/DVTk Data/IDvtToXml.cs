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
using System.Text;

using DvtkDicomUnicodeConversion;

namespace DvtkData.DvtDetailToXml
{
    /// <summary>
    /// Dvt Test Log To Xml Interface. Defines a method to serialize DVT Test Log Data to Xml.
    /// </summary>
    public interface IDvtTestLogToXml
    {
        /// <summary>
        /// Serialize DVT Test Log to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        void DvtTestLogToXml(StreamWriter streamWriter, string name);
    }

    /// <summary>
    /// Dvt Detail To Xml Interface. Defines a method to serialize DVT Detail Data to Xml.
    /// </summary>
    public interface IDvtDetailToXml
    {
        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns> 
        bool DvtDetailToXml(StreamWriter streamWriter, int level);
    }

    /// <summary>
    /// Dvt Summary To Xml Interface. Defines a method to serialize DVT Summary Data to Xml.
    /// </summary>
    public interface IDvtSummaryToXml
    {
        /// <summary>
        /// Serialize DVT Summary Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns> 
        bool DvtSummaryToXml(StreamWriter streamWriter, int level);
    }

    /// <summary>
    /// DvtToXml class provides static conversion methods for XML output.
    /// </summary>
    public class DvtToXml
    {
        /// <summary>
        /// Convert the incoming string into an XML format by replacing some characters by their
        /// XML coding.
        /// </summary>
        /// <param name="inString">String to be converted.</param>
        /// <param name="displaySpaces">Display spaces correctly for attribute values.</param>
        /// <returns>Converted string.</returns>
        public static System.String ConvertString(System.String inString, bool displaySpaces)
        {
            // convert character values so that they can be displayed in XML
            StringBuilder outString = new StringBuilder();

            if ((inString != null) &&
                (inString != System.String.Empty))
            {
                for (int i = 0; i < inString.Length; i++)
                {
                    System.String valueString = String.Empty;
                    System.Int32 charValue = Convert.ToInt32(inString[i]);

                    if ((charValue >= 0) &&
                        (charValue < 32))
                    {
                        // char in range 0..31
                        switch (charValue)
                        {
                            case 9: valueString = "&#x09;"; break;
                            case 10: valueString = "[LF]"; break;
                            case 12: valueString = "[FF]"; break;
                            case 13: valueString = "[CR]"; break;
                            case 14: valueString = "[SO]"; break;
                            case 15: valueString = "[SI]"; break;
                            case 27: valueString = "[ESC]"; break;
                            default:
                                {
                                    System.String charValueString = charValue.ToString("X");
                                    while (charValueString.Length < 2)
                                    {
                                        charValueString = "0" + charValueString;
                                    }

                                    valueString = "\\" + charValueString;
                                    break;
                                }
                        }
                    }
                    else if ((charValue > 126) &&
                        (charValue <= 255))
                    {
                        // char in range 127..255
                        System.String charValueString = charValue.ToString("X");
                        while (charValueString.Length < 2)
                        {
                            charValueString = "0" + charValueString;
                        }

                        valueString = "\\" + charValueString;
                    }
                    else if (charValue > 255)
                    {
                        // the internal compiler marshalling used to convert the strings
                        // from unmanaged to managed results in UNICODE values for these characters
                        // - use a simple switch statement to display the required values
                        switch (charValue)
                        {
                            case 8364: valueString = "\\80"; break;
                            case 8218: valueString = "\\82"; break;
                            case 402: valueString = "\\83"; break;
                            case 8222: valueString = "\\84"; break;
                            case 8230: valueString = "\\85"; break;
                            case 8224: valueString = "\\86"; break;
                            case 8225: valueString = "\\87"; break;
                            case 710: valueString = "\\88"; break;
                            case 8240: valueString = "\\89"; break;
                            case 352: valueString = "\\8A"; break;
                            case 8249: valueString = "\\8B"; break;
                            case 338: valueString = "\\8C"; break;
                            case 381: valueString = "\\8E"; break;
                            case 8216: valueString = "\\91"; break;
                            case 8217: valueString = "\\92"; break;
                            case 8220: valueString = "\\93"; break;
                            case 8221: valueString = "\\94"; break;
                            case 8226: valueString = "\\95"; break;
                            case 8211: valueString = "\\96"; break;
                            case 8212: valueString = "\\97"; break;
                            case 732: valueString = "\\98"; break;
                            case 8482: valueString = "\\99"; break;
                            case 353: valueString = "\\9A"; break;
                            case 8250: valueString = "\\9B"; break;
                            case 339: valueString = "\\9C"; break;
                            case 382: valueString = "\\9E"; break;
                            case 376: valueString = "\\9F"; break;
                            default: break;
                        }
                    }
                    else
                    {
                        switch (charValue)
                        {
                            case 32: // Space 
                                if (displaySpaces)
                                {
                                    valueString = "&#160;";
                                }
                                else
                                {
                                    valueString = inString[i].ToString();
                                }
                                break;
                            case 38: valueString = "&#x26;"; break; // &
                            case 60: valueString = "&#x3C;"; break; // <
                            case 62: valueString = "&#x3E;"; break; // >
                            default:
                                // char in range 32..127
                                valueString = inString[i].ToString();
                                break;
                        }
                    }

                    // add the character value to the string
                    outString.Append(valueString);
                }
            }
            return outString.ToString();//outString;
        }

        /// <summary>
        /// Convert the incoming string into a Unicode representation in XML format.
        /// The conversion is only done if 8-bit chars are present in the inString - otherwise
        /// an empty string is returned.
        /// </summary>
        /// <param name="dicomUnicodeConvertor">Instantiated DICOM to Unicode Convertor.</param>
        /// <param name="inString">String to be converted.</param>
        /// <returns>Converted Unicode XML string.</returns>
        public static System.String ConvertStringToXmlUnicode(DicomUnicodeConverter dicomUnicodeConvertor, System.String inString)
        {
            String outString = String.Empty;
            if ((dicomUnicodeConvertor == null) ||
                (inString == null) ||
                (inString == System.String.Empty))
            {
                return outString;
            }

            bool only7BitCharsPresent = true;
            byte[] dicomString = new byte[inString.Length];
            for (int i = 0; i < inString.Length; i++)
            {
                dicomString[i] = (byte)inString[i];
                if ((dicomString[i] > 0x80) ||
                    (dicomString[i] == 0x1B))
                {
                    only7BitCharsPresent = false;
                }
            }
            if (only7BitCharsPresent == false)
            {
                UInt16[] unicode = dicomUnicodeConvertor.Unicode(dicomString);
                outString = dicomUnicodeConvertor.UnicodeAsXml(unicode);
                if (outString == String.Empty)
                {
                    outString = "Specific Character Set not supported for display.";
                }
            }
            return outString;
        }
    }
}
