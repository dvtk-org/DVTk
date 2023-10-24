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

namespace Dvtk.Dicom.InformationEntity.CompositeInfoModel
{
    /// <summary>
    /// /// Study Composite entity. It is derived from BaseCompositeEntity.
    /// It contains the Series as Children
    /// </summary>
    public class Study:BaseCompositeInformationEntity
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public Study()
            : base()
        {
 
        }
        /// <summary>
        /// Class constructor with DICOM dataset which contains study attributes
        /// </summary>
        /// <param name="_dataSet">DICOM dataset which contains study attributes</param>
        public Study(DvtkData.Dimse.DataSet _dataSet)
            : base()
        {
            LoadData(_dataSet);
        }
        protected override void LoadData(DvtkData.Dimse.DataSet _dataSet)
        {
            base.LoadData(_dataSet);
            StudyID =BaseCompositeInformationEntity.GetDicomValue(_dataSet.GetAttribute(DvtkData.Dimse.Tag.STUDY_ID));
            StudyInstanceUID =BaseCompositeInformationEntity.GetDicomValue( _dataSet.GetAttribute(DvtkData.Dimse.Tag.STUDY_INSTANCE_UID));
        }
        string _studyID = "";
        /// <summary>
        /// Gets or sets study ID
        /// </summary>
        public string StudyID
        {
            get { return _studyID; }
            set { _studyID = value; }
        }
        string _studyUID = "";
        /// <summary>
        /// Gets or sets StudyInstanceUID
        /// </summary>
        public string StudyInstanceUID
        {
            get { return _studyUID; }
            set { _studyUID = value; }
        }
    }
}
