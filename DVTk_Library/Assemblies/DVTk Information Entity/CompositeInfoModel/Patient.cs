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
using DvtkData.Dimse;

namespace Dvtk.Dicom.InformationEntity.CompositeInfoModel
{
    /// <summary>
    /// Patient Composite entity. It is derived from BaseCompositeEntity.
    /// It contains the studies as Children
    /// </summary>
    public class Patient:BaseCompositeInformationEntity
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public Patient():base()
        { 
        }
        /// <summary>
        /// Class constructor with Dicom dataset as parameter
        /// </summary>
        /// <param name="_dataSet">DICOM dataset which contains the Patient attributes</param>
        public Patient(DvtkData.Dimse.DataSet _dataSet)
            : base()
        {
            LoadData(_dataSet);
        }

        protected override void LoadData(DvtkData.Dimse.DataSet _dataSet)
        {
            base.LoadData(_dataSet);
            patientName = BaseCompositeInformationEntity.GetDicomValue(_dataSet.GetAttribute(DvtkData.Dimse.Tag.PATIENTS_NAME));
            patientId =BaseCompositeInformationEntity.GetDicomValue(_dataSet.GetAttribute(DvtkData.Dimse.Tag.PATIENT_ID));
        }
        string patientName = "";
        /// <summary>
        /// gets or sets patient name
        /// </summary>
        public string PatientName
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
        string patientId = "";
        /// <summary>
        /// gets or sets patient ID
        /// </summary>
        public string PatientId
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
    }
}
