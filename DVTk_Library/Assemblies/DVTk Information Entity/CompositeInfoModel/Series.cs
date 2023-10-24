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
    /// Series Composite entity. It is derived from BaseCompositeEntity.
    /// It contains the Images as Children
    /// </summary>
   public class Series:BaseCompositeInformationEntity
    {
       /// <summary>
       /// Class constructor
       /// </summary>
        public Series()
            : base()
        {
        }
       /// <summary>
       /// Class constructor with DICOM dataset as parameter.
       /// </summary>
       /// <param name="_dataSet">DICOM dataset which contains series attributes</param>
        public Series(DvtkData.Dimse.DataSet _dataSet)
            : base()
        {
            LoadData(_dataSet);
        }
        protected override void LoadData(DvtkData.Dimse.DataSet _dataSet)
        {
            base.LoadData(_dataSet);
            InstanceNumber =BaseCompositeInformationEntity.GetDicomValue(_dataSet.GetAttribute(DvtkData.Dimse.Tag.SERIES_NUMBER));
            SeriesInstanceUID =BaseCompositeInformationEntity.GetDicomValue(_dataSet.GetAttribute(DvtkData.Dimse.Tag.SERIES_INSTANCE_UID));
        }
        string _instanceNumber = "";
       /// <summary>
       /// gets or sets instance number
       /// </summary>
        public string InstanceNumber
        {
            get
            {
                return _instanceNumber;
            }
            set
            {
                _instanceNumber = value;
            }
        }
        string _seriesUID = "";
       /// <summary>
       /// gets or sets SeriesInstanceUID
       /// </summary>
        public string SeriesInstanceUID
        {
            get
            {
                return _seriesUID;
            }
            set
            {
                _seriesUID = value;
            }
        }
    }
}
