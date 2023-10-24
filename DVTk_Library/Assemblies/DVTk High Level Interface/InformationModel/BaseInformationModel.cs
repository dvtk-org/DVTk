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

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using VR = DvtkData.Dimse.VR;
using Dvtk.Dicom.InformationEntity.CompositeInfoModel;
using System.Collections.Generic;



namespace DvtkHighLevelInterface.InformationModel
{
	/// <summary>
	/// Summary description for BaseInformationModel.
	/// </summary>
	public abstract class BaseInformationModel
	{
		protected Dvtk.Dicom.InformationEntity.BaseInformationModel _root = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="root">The encapsulated Dvtk.Dicom.InformationEntity information model.</param>
		public BaseInformationModel(Dvtk.Dicom.InformationEntity.BaseInformationModel root)
		{
			_root = root;
		}

        /// <summary>
        /// Gets the encapsulated Dvtk.Dicom.InformationEntity information model.
        /// </summary>
		protected Dvtk.Dicom.InformationEntity.BaseInformationModel Root
		{
			get
			{
				return _root;
			}
		}

        /// <summary>
        /// Dumps to information model to text.
        /// </summary>
        /// <returns></returns>
		public String Dump()
		{
			return(this._root.Dump(""));
		}
        /// <summary>
        /// Gets the Composite Information Data model from the root.
        /// </summary>
        /// <returns>Returns collection of Composite datas</returns>
        public List<BaseCompositeInformationEntity> GetCompositeDataModel()
        {
            return this.Root.GetCompositeDataModel();
        }
		/// <summary>
		/// Property - DataDirectory
		/// </summary>
		public System.String DataDirectory
		{
			set
			{
				_root.DataDirectory = value;
			}
		}

		/// <summary>
		/// Loads the Information Model with the appropriate data from .DCM and .RAW
		/// files found in the given dataDirectory.
		/// </summary>
		/// <param name="dataDirectory">Location of the .DCM and .RAW files used to populate the Information Model.</param>
		public void LoadInformationModel(System.String dataDirectory)
		{
			_root.LoadInformationModel(dataDirectory);
		}

		/// <summary>
		/// Refresh the Information Model contents.
		/// </summary>
		public void RefreshInformationModel()
		{
			_root.RefreshInformationModel();
		}

		/// <summary>
		/// Copy the default dataset attributes to the Information Entities in the Information
		/// Models that define them.
		/// </summary>
		/// <param name="overWriteExistingValue">Boolean to indicate whether any already existing value should be overwritten or not.</param>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="vR">The VR.</param>
        /// <param name="parameters">The value(s).</param>
		public void AddDefaultAttributeToInformationModel(bool overWriteExistingValue, String tagSequence, VR vR, params Object[] parameters)
		{
			// need DicomMessage to be able to set the attribute in the dataset
			DicomMessage dicomMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.UNDEFINED);
			dicomMessage.Set(tagSequence, vR, parameters);
			_root.AddDefaultAttributesToInformationModel(overWriteExistingValue, dicomMessage.DataSet.DvtkDataDataSet);
		}

		/// <summary>
		/// Add the attributes in this additional dataset to all Information Entities in the Information
		/// Models.
		/// </summary>
		/// <param name="overWriteExistingValue">Boolean to indicate whether any already existing value should be overwritten or not.</param>
        /// <param name="tagSequence">The tag sequence.</param>
        /// <param name="vR">The VR.</param>
        /// <param name="parameters">The value(s).</param>
        public void AddAdditionalAttributeToInformationModel(bool overWriteExistingValue, String tagSequence, VR vR, params Object[] parameters)
		{
			// need DicomMessage to be able to set the attribute in the dataset
			DicomMessage dicomMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.UNDEFINED);
			dicomMessage.Set(tagSequence, vR, parameters);
			_root.AddAdditionalAttributesToInformationModel(overWriteExistingValue, dicomMessage.DataSet.DvtkDataDataSet);
		}

		/// <summary>
		/// Query the Information Model using the given query message.
		/// </summary>
		/// <param name="queryMessage">Message used to query the Information Model.</param>
		/// <returns>DicomMessageCollection - containing the query responses. The final query response (without a dataset) is also included.</returns>
		public DicomMessageCollection QueryInformationModel(DicomMessage queryMessage)
		{
			DicomMessageCollection responseMessages = new DicomMessageCollection();

			DvtkHighLevelInterface.Dicom.Other.Values values = queryMessage.CommandSet["0x00000002"].Values;
			System.String sopClassUid = values[0];

			DataSet queryDataset = queryMessage.DataSet;
			Dvtk.Dicom.InformationEntity.DataSetCollection queryResponses = _root.QueryInformationModel(queryDataset.DvtkDataDataSet);

			DvtkData.Dimse.DicomMessage dvtkDicomMessage = null;
			DvtkData.Dimse.CommandSet dvtkCommand = null;
			DicomMessage responseMessage = null;

			if (queryResponses != null)
			{
				foreach (DvtkData.Dimse.DataSet dvtkDataset in queryResponses)
				{
					dvtkDicomMessage = new DvtkData.Dimse.DicomMessage();
					dvtkCommand = new DvtkData.Dimse.CommandSet(DvtkData.Dimse.DimseCommand.CFINDRSP);
					dvtkCommand.AddAttribute(0x0000, 0x0002, DvtkData.Dimse.VR.UI, sopClassUid);
					dvtkCommand.AddAttribute(0x0000, 0x0900, DvtkData.Dimse.VR.US, 0xFF00);

                    dvtkDicomMessage.Apply(dvtkCommand, dvtkDataset, queryMessage.EncodedPresentationContextID);
					responseMessage = new DicomMessage(dvtkDicomMessage);
					responseMessages.Add(responseMessage);
				}
			}

			dvtkDicomMessage = new DvtkData.Dimse.DicomMessage();
			dvtkCommand = new DvtkData.Dimse.CommandSet(DvtkData.Dimse.DimseCommand.CFINDRSP);
			dvtkCommand.AddAttribute(0x0000, 0x0002, DvtkData.Dimse.VR.UI, sopClassUid);
			dvtkCommand.AddAttribute(0x0000, 0x0900, DvtkData.Dimse.VR.US, 0x0000);

            dvtkDicomMessage.Apply(dvtkCommand, queryMessage.EncodedPresentationContextID);
			responseMessage = new DicomMessage(dvtkDicomMessage);
			responseMessages.Add(responseMessage);

			return responseMessages;
		}

		#region Transaction handlers
		/// <summary>
		/// Patient Registration request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient registration attributes.</param>
		public void PatientRegistration(DvtkData.Dimse.DataSet dataset)
		{
			_root.PatientRegistration(dataset);
		}

		/// <summary>
		/// Patient Update request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient update attributes.</param>
		public void PatientUpdate(DvtkData.Dimse.DataSet dataset)
		{
			_root.PatientUpdate(dataset);
		}

		/// <summary>
		/// Patient merge request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient merge attributes.</param>
		public void PatientMerge(DvtkData.Dimse.DataSet dataset)
		{
			_root.PatientMerge(dataset);
		}

		/// <summary>
		/// Placer order management request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing placer order management attributes.</param>
		public void PlacerOrderManagement(DvtkData.Dimse.DataSet dataset)
		{
			_root.PlacerOrderManagement(dataset);
		}
		#endregion Transaction handlers
	}
}
