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

namespace Dvtk.CommonDataFormat
{
    /// <summary>
    /// Summary description for BaseCommonDataFormat.
    /// </summary>
    public abstract class BaseCommonDataFormat
    {
        /// <summary>
        /// Convert from Common Data Format to DICOM format.
        /// </summary>
        /// <returns>DICOM format.</returns>
        public abstract System.String ToDicomFormat();

        /// <summary>
        /// Convert from DICOM format to Common Data Format.
        /// </summary>
        /// <param name="dicomFormat">DICOM format.</param>
        public abstract void FromDicomFormat(System.String dicomFormat);

        /// <summary>
        /// Convert from Common Data Format to HL7 format.
        /// </summary>
        /// <returns>HL7 format.</returns>
        public abstract System.String ToHl7Format();

        /// <summary>
        /// Convert from HL7 format to Common Data Format.
        /// </summary>
        /// <param name="hl7Format">HL7 format.</param>
        public abstract void FromHl7Format(System.String hl7Format);

        /// <summary>
        /// Console Display the Common Data Format content - for debugging purposes.
        /// </summary>
        public abstract void ConsoleDisplay();
    }
}
