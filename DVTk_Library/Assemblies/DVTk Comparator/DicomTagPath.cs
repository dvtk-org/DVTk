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

namespace Dvtk.Comparator
{
    /// <summary>
    /// Summary description for DicomTagPath.
    /// </summary>
    public class DicomTagPath
    {
        private DvtkData.Dimse.Tag _tag = null;
        private DicomTagPath _nextTag = null;

        public DicomTagPath(DvtkData.Dimse.Tag tag)
        {
            _tag = tag;
        }

        public DicomTagPath(DvtkData.Dimse.Tag parentTag, DicomTagPath dicomTagPath)
        {
            _tag = parentTag;
            _nextTag = dicomTagPath;
        }

        public DicomTagPath(DvtkData.Dimse.Tag parentTag, DvtkData.Dimse.Tag tag)
        {
            _tag = parentTag;
            _nextTag = new DicomTagPath(tag);
        }

        public DicomTagPath(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag parent2Tag, DvtkData.Dimse.Tag tag)
        {
            _tag = parent1Tag;
            _nextTag = new DicomTagPath(parent2Tag, tag);
        }

        public DicomTagPath(DvtkData.Dimse.Tag parent1Tag, DvtkData.Dimse.Tag parent2Tag, DvtkData.Dimse.Tag parent3Tag, DvtkData.Dimse.Tag tag)
        {
            _tag = parent1Tag;
            _nextTag = new DicomTagPath(parent2Tag, parent3Tag, tag);
        }

        public DvtkData.Dimse.Tag Tag
        {
            get
            {
                return _tag;
            }
        }

        public DicomTagPath Next
        {
            get
            {
                return _nextTag;
            }
        }
    }
}
