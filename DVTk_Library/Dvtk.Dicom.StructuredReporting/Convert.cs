// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2010 DVTk
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

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Dicom.Other;

namespace Dvtk.Dicom.StructuredReporting
{
    /// <summary>
    /// Class containing conversion functionality.
    /// </summary>
    internal static class Convert
    {
        /// <summary>
        /// Convert the first value of a specified attribute to a String.
        /// </summary>
        /// <param name="attributeSet">The Attribute Set containing the DICOM Attribute.</param>
        /// <param name="tag">The tag of the attribute.</param>
        /// <returns>
        /// If the associated DICOM attribute is not present, null is returned.
        /// If the associated DICOM attribute is present and has no values, "" is returned.
        /// If the associated DICOM attribute is present and has values, the first value is returned.
        /// </returns>
        public static String FirstAttributeValueToString(AttributeSet attributeSet, String tag)
        {
            String stringForFirstAttributeValue = null;

            DvtkHighLevelInterface.Dicom.Other.Attribute attribute = attributeSet[tag];

            if (attribute.Exists)
            {
                if (attribute.Values.Count == 0)
                {
                    stringForFirstAttributeValue = "";
                }
                else
                {
                    stringForFirstAttributeValue = Convert.ToTrimmedString(attribute.Values[0], attribute.VR);
                }
            }

            return (stringForFirstAttributeValue);
        }

        /// <summary>
        /// A null safe string dump, taking care of the case when null is supplied as the first
        /// argument.
        /// </summary>
        /// <param name="stringToDump">A String representing the first value of a DICOM attribute.</param>
        /// <param name="prefix">Will be added to the beginning of each line.</param>
        /// <param name="name">The name of the string dump.</param>
        /// <returns>The string dump.</returns>
        public static String NullSafeStringDump(String stringToDump, String prefix, String name)
        {
            String stringDump = prefix + name;

            if (stringToDump == null)
            {
                stringDump += ": no value (associated DICOM attribute not present)";
            }
            else if (stringToDump == "")
            {
                stringDump += ": no value (associated DICOM attribute present but contains no values";
            }
            else
            {
                stringDump += ": \"" + stringToDump + "\"";
            }

            return (stringDump);
        }

        /// <summary>
        /// Removed all non-significant characters, as defined by the specified VR.
        /// </summary>
        /// <param name="stringToTrim">
        /// The string from which to remove the non-significant characters.
        /// </param>
        /// <param name="vr">The VR to take into account.</param>
        /// <returns>The trimmed String.</returns>
        public static string ToTrimmedString(string stringToTrim, VR vr)
        {
            string trimmedString = null;

            switch (vr)
            {
                case VR.SH:
                case VR.LO:
                    trimmedString = stringToTrim.Trim();
                    break;

                default:

                    // TODO: still to implement for other VR's.
                    trimmedString = stringToTrim;
                    break;
            }

            return (trimmedString);
        }
    }
}
