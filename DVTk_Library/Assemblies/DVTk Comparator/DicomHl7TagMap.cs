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

using DvtkData.Dimse;
using Dvtk.Hl7;
using Dvtk.Comparator.Bases;

namespace Dvtk.Comparator
{
    /// <summary>
    /// Summary description for DicomHl7TagMap.
    /// </summary>
    public class DicomHl7TagMap
    {
        private DicomTagPath _dicomTagPath = null;
        private Hl7TagPath _hl7TagPath = null;
        private BaseValueConvertor _valueConvertor = null;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dicomTagPath">DICOM Tag Path.</param>
        /// <param name="hl7TagPath">HL7 Tag Path.</param>
        public DicomHl7TagMap(DicomTagPath dicomTagPath, Hl7TagPath hl7TagPath)
        {
            _dicomTagPath = dicomTagPath;
            _hl7TagPath = hl7TagPath;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dicomTagPath">DICOM Tag Path.</param>
        /// <param name="hl7TagPath">HL7 Tag Path.</param>
        /// <param name="valueConvertor"></param>
        public DicomHl7TagMap(DicomTagPath dicomTagPath, Hl7TagPath hl7TagPath, BaseValueConvertor valueConvertor)
        {
            _dicomTagPath = dicomTagPath;
            _hl7TagPath = hl7TagPath;
            _valueConvertor = valueConvertor;
        }

        #region Properties
        /// <summary>
        /// Property - DicomTagPath.
        /// </summary>
        public DicomTagPath DicomTagPath
        {
            get
            {
                return _dicomTagPath;
            }
        }

        /// <summary>
        /// Property - DICOM Tag.
        /// </summary>
        public DvtkData.Dimse.Tag DicomTag
        {
            get
            {
                return _dicomTagPath.Tag;
            }
        }

        /// <summary>
        /// Property - HL7 Tag.
        /// </summary>
        public Hl7Tag Hl7Tag
        {
            get
            {
                return _hl7TagPath.Tag;
            }
        }

        /// <summary>
        /// Property - HL7 Component Index.
        /// </summary>
        public int Hl7ComponentIndex
        {
            get
            {
                return _hl7TagPath.ComponentIndex;
            }
        }

        /// <summary>
        /// Property - HL7 Name.
        /// </summary>
        public System.String Hl7Name
        {
            get
            {
                return _hl7TagPath.Name;
            }
        }

        public BaseValueConvertor ValueConvertor
        {
            get
            {
                return _valueConvertor;
            }
        }
        #endregion
    }
}
