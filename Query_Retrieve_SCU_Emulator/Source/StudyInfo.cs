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
using System.Collections;
using System.Text;
using System.ComponentModel;

namespace QR_SCU_Emulator
{
    [DefaultPropertyAttribute("none")]
    public class Study  
    {
        private string accessionNo;
        private string studyId;
        private string studyInstanceUid;

        public ArrayList seriesList = null;

        public Study() { 
            seriesList = new ArrayList();            
        }

        [ReadOnlyAttribute(true), CategoryAttribute("Accession Number"),
        DescriptionAttribute("Accession Number")]
        public String AccessionNumber
        {
            get 
            {
                return accessionNo;
            }
            set
            {
                accessionNo = value;
            }
        }

        [ReadOnlyAttribute(true), CategoryAttribute("Study ID"),
        DescriptionAttribute("ID of the Study (Requiered)")]
        public String StudyID
        {
            get 
            {
                return studyId;
            }
            set
            {
                studyId = value;
            }
        }

        [ReadOnlyAttribute(true), CategoryAttribute("Study Instance UID"),
        DescriptionAttribute("Study Instance UID (Unique)")]
        public String StudyInstanceUID
        {
            get 
            {
                return studyInstanceUid;
            }
            set
            {
                studyInstanceUid = value;
            }
        }

        public int SeriesExists(string seriesUid) {
            int seriesExists = -1;

            for (int i = 0; i < seriesList.Count; i++) {
                if (((Series)seriesList[i]).SeriesUID == seriesUid) {
                    seriesExists = i;
                    break;
                }
            }
            return seriesExists;
        }
    }
}
