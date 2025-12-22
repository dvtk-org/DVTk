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
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace QR_SCU_Emulator
{
    [DefaultPropertyAttribute("none")]
    public class Patient  
    {
        private string patientName;
        private string patientId;
        private string patientBirthDate;

        public ArrayList studyList = null;

        public Patient() 
        {
            studyList = new ArrayList();
        }

        [ReadOnlyAttribute(true), CategoryAttribute("Patient Name"),
        DescriptionAttribute("Name of the patient (Required)")]
        public String PatientName
        {
            get 
            {
                return patientName;
            }
            set
            {
                patientName = value;
            }
        }

        [ReadOnlyAttribute(true), CategoryAttribute("Patient ID"),
        DescriptionAttribute("ID of the patient (Unique)")]
        public String PatientId
        {
            get 
            {
                return patientId;
            }
            set
            {
                patientId = value;
            }
        }

        [ReadOnlyAttribute(true), CategoryAttribute("Patients Birth Date"),
        DescriptionAttribute("Date of Birth of the Patient (Optional)")]
        public string PatientBirthDate
        {
            get 
            {
                return patientBirthDate;
            }
            set
            {
                patientBirthDate = value;
            }
        }

        public int StudyExists(string studyInstanceUid) 
        {
            int studyExists = -1;

            for (int i = 0; i < studyList.Count; i++) 
            {
                if (((Study)studyList[i]).StudyInstanceUID == studyInstanceUid) 
                {
                    studyExists = i;
                    break;
                }
            }
            return studyExists;
        }
    }
}
