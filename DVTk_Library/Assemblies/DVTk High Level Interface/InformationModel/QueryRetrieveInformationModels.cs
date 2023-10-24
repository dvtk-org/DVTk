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

using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.Dicom.Other;
using VR = DvtkData.Dimse.VR;



namespace DvtkHighLevelInterface.InformationModel
{
	/// <summary>
	/// Summary description for QueryRetrieveInformationModels.
	/// </summary>
	public class QueryRetrieveInformationModels
	{
		private QueryRetrievePatientRootInformationModel _patientRootInformationModel = null;
		private QueryRetrieveStudyRootInformationModel _studyRootInformationModel = null;
		private QueryRetrievePatientStudyOnlyInformationModel _patientStudyOnlyInformationModel = null;

		/// <summary>
		/// Class Constructor.
		/// </summary>
		public QueryRetrieveInformationModels()
		{
			_patientRootInformationModel = new QueryRetrievePatientRootInformationModel();
			_studyRootInformationModel = new QueryRetrieveStudyRootInformationModel();
			_patientStudyOnlyInformationModel = new QueryRetrievePatientStudyOnlyInformationModel();
		}

		/// <summary>
		/// Get Query/Retrieve Patient Root Information Model.
		/// </summary>
		public QueryRetrievePatientRootInformationModel PatientRoot
		{
			get
			{
				return _patientRootInformationModel;
			}
		}

		/// <summary>
		/// Get Query/Retrieve Study Root Information Model
		/// </summary>
		public QueryRetrieveStudyRootInformationModel StudyRoot
		{
			get
			{
				return _studyRootInformationModel;
			}
		}

		/// <summary>
		/// Get Query/Retrieve Patient/Study Only Information Model
		/// </summary>
		public QueryRetrievePatientStudyOnlyInformationModel PatientStudyOnly
		{
			get
			{
				return _patientStudyOnlyInformationModel;
			}
		}


		/// <summary>
		/// Property - DataDirectory.
		/// </summary>
		public System.String DataDirectory
		{
			set
			{
				_patientRootInformationModel.DataDirectory = value;
				_studyRootInformationModel.DataDirectory = value;
				_patientStudyOnlyInformationModel.DataDirectory = value;
			}
		}

		/// <summary>
		/// Load the Information Model from the contents of the Data Directory. Look for all .DCM and .RAW files
		/// and load them.
		/// </summary>
		/// <param name="dataDirectory">Data directory containing the .DCM and .RAW files.</param>
		public void Load(System.String dataDirectory)
		{
			// set up and load the information models
			_patientRootInformationModel.LoadInformationModel(dataDirectory);
			_studyRootInformationModel.LoadInformationModel(dataDirectory);
			_patientStudyOnlyInformationModel.LoadInformationModel(dataDirectory);
		}

		/// <summary>
		/// Refresh the Information Model contents
		/// </summary>
		public void Refresh()
		{
			// refresh the information models
			_patientRootInformationModel.RefreshInformationModel();
			_studyRootInformationModel.RefreshInformationModel();
			_patientStudyOnlyInformationModel.RefreshInformationModel();
		}

		/// <summary>
		/// Add data to the Information Model from the given dataset.
		/// </summary>
		/// <param name="dataset">Dataset used to populate the Information Model.</param>
		public void Add(DicomFile dicomFile)
		{
            if (dicomFile != null)
			{
				// add the dataset details to the information models
                _patientRootInformationModel.AddToInformationModel(dicomFile);
                _studyRootInformationModel.AddToInformationModel(dicomFile);
                _patientStudyOnlyInformationModel.AddToInformationModel(dicomFile);
			}
		}

        /// <summary>
        /// Add data to the Information Model from the given dataset with Store data option.
        /// </summary>
        /// <param name="dataset">Dataset used to populate the Information Model.</param>
        /// <param name="storeDataset">Boolean indicating whether or not the dataset should also be stored to file for possible retrieval.</param>
        public void Add(DicomFile dicomFile, bool storeDataset)
        {
            if (dicomFile != null)
            {
                // add the dataset details to the information models
                _patientRootInformationModel.AddToInformationModel(dicomFile, storeDataset);
                _studyRootInformationModel.AddToInformationModel(dicomFile, storeDataset);
                _patientStudyOnlyInformationModel.AddToInformationModel(dicomFile, storeDataset);
            }
        }

        /// <summary>
        /// Copy the default dataset attributes to the Information Entities in the Information Models that define them.
        /// </summary>
        /// <param name="overWriteExistingValue">Boolean to indicate whether any already existing value should be overwritten or not.</param>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="vR">The VR.</param>
        /// <param name="parameters">The values.</param>
		public void AddDefaultAttribute(bool overWriteExistingValue, String tagSequence, VR vR, params Object[] parameters)
		{
			// add default attributes to the information models
			_patientRootInformationModel.AddDefaultAttributeToInformationModel(overWriteExistingValue, tagSequence, vR, parameters);
			_studyRootInformationModel.AddDefaultAttributeToInformationModel(overWriteExistingValue, tagSequence, vR, parameters);
			_patientStudyOnlyInformationModel.AddDefaultAttributeToInformationModel(overWriteExistingValue, tagSequence, vR, parameters);
		}

		/// <summary>
		/// Add the attributes in this additional dataset to all Information Entities in the Information
		/// Models. 
		/// </summary>
        /// <param name="overWriteExistingValue">Boolean to indicate whether any already existing value should be overwritten or not.</param>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="vR">The VR.</param>
        /// <param name="parameters">The values.</param>
        public void AddAdditionalAttribute(bool overWriteExistingValue, String tagSequence, VR vR, params Object[] parameters)
		{
			// add additional attributes to the information models

			_patientRootInformationModel.AddAdditionalAttributeToInformationModel(overWriteExistingValue, tagSequence, vR, parameters);
			_studyRootInformationModel.AddAdditionalAttributeToInformationModel(overWriteExistingValue, tagSequence, vR, parameters);
			_patientStudyOnlyInformationModel.AddAdditionalAttributeToInformationModel(overWriteExistingValue, tagSequence, vR, parameters);
		}
	}
}
