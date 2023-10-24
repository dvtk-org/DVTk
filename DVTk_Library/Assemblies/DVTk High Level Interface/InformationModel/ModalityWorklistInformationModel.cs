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

using Dvtk.Dicom.InformationEntity;
using Dvtk.Dicom.InformationEntity.Worklist;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.InformationModel
{
	/// <summary>
	/// Summary description for ModalityWorklistInformationModel.
	/// Provides a wrapper class around the Dvtk.Dicom.InformationEntity.Worklist.ModalityWorklistInformationModel class.
	/// </summary>
	public class ModalityWorklistInformationModel : BaseInformationModel
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public ModalityWorklistInformationModel() : base(new Dvtk.Dicom.InformationEntity.Worklist.ModalityWorklistInformationModel()) {}


        /// <summary>
        /// Add data to the Information Model from the given dataset.
        /// </summary>
        /// <param name="dataset">Dataset used to populate the Information Model.</param>
        public void AddToInformationModel(DataSet dataset)
        {
            // Add dataset to Information Model and store dataset in DCM file for possible retrieval.
            Root.AddToInformationModel(dataset.DvtkDataDataSet, true);
        }

        /// <summary>
        /// Add data to the Information Model from the given dataset with Storage option.
        /// </summary>
        /// <param name="dataset">Dataset used to populate the Information Model.</param>
        /// <param name="storeDataset">Boolean indicating whether or not the dataset should also be stored to file for possible retrieval.</param>
        public void AddToInformationModel(DataSet dataset, bool storeDataset)
        {
            // Add dataset to Information Model and store dataset in DCM file for possible retrieval.
            Root.AddToInformationModel(dataset.DvtkDataDataSet, storeDataset);
        }

        protected Dvtk.Dicom.InformationEntity.Worklist.ModalityWorklistInformationModel Root
        {
            get
            {
                return (_root as Dvtk.Dicom.InformationEntity.Worklist.ModalityWorklistInformationModel);
            }
        }
	}
}
