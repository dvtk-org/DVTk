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
    /// Image Compisite Entity. It is derived from the base composite entity
    /// </summary>
   public  class Image:BaseCompositeInformationEntity
    {
       /// <summary>
       /// Class Constructor
       /// </summary>
        public Image()
            : base()
        { 
        }
       /// <summary>
       /// Class constructer with Dataset as parameter
       /// </summary>
       /// <param name="_dataSet">DICOM dataset which is having Image attributes</param>
        public Image(DvtkData.Dimse.DataSet _dataSet)
            : base()
        {
            LoadData(_dataSet);
        }
        protected override void LoadData(DvtkData.Dimse.DataSet _dataSet)
        {
            base.LoadData(_dataSet);
            instanceNumber = BaseCompositeInformationEntity.GetDicomValue(_dataSet.GetAttribute(DvtkData.Dimse.Tag.INSTANCE_NUMBER));
            SOPInstanceUID =BaseCompositeInformationEntity.GetDicomValue(_dataSet.GetAttribute(DvtkData.Dimse.Tag.SOP_INSTANCE_UID));
        }
        string instanceNumber = "";
       /// <summary>
       /// gets or sets the instance number
       /// </summary>
        public string InstanceNumber
        {
            get { return instanceNumber;  }
            set { instanceNumber = value; }
        }
        string sopInstanceUID = "";
       /// <summary>
       /// gets or sets the SOP instance UID
       /// </summary>
        public string SOPInstanceUID
        {
            get { return sopInstanceUID; }
            set { sopInstanceUID = value; }
        }
    }
}
