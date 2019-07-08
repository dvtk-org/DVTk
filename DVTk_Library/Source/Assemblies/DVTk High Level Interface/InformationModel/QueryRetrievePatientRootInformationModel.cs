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
using Dvtk.Dicom.InformationEntity.QueryRetrieve;
using DvtkHighLevelInterface.Dicom.Messages;



namespace DvtkHighLevelInterface.InformationModel
{
	/// <summary>
	/// Summary description for QueryRetrievePatientRootInformationModel.
	/// Provides a wrapper class around the Dvtk.Dicom.InformationEntity.QueryRetrieve.PatientRootInformationModel class.
	/// </summary>
    public class QueryRetrievePatientRootInformationModel : QueryRetrieveInformationModel, ICommitInformationModel, IRetrieveInformationModel
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		public QueryRetrievePatientRootInformationModel() : base(new PatientRootInformationModel()) {}

		#region ICommitInformationModel
		/// <summary>
		/// Check if the given instance is present in the Information Model. The instance will be at the leaf nodes of the Information Model.
		/// </summary>
		/// <param name="sopClassUid">SOP Class UID to search for.</param>
		/// <param name="sopInstanceUid">SOP Instance UID to search for.</param>
		/// <returns>Boolean - true if instance found in the Information Model, otherwise false.</returns>
		public bool IsInstanceInInformationModel(System.String sopClassUid, System.String sopInstanceUid)
		{
			PatientRootInformationModel root = (PatientRootInformationModel)Root;
			return root.IsInstanceInInformationModel(sopClassUid, sopInstanceUid);
		}
		#endregion

		#region IRetrieveInformationModel
		/// <summary>
		/// Retrieve data from the Information Model using the given retrieve message.
		/// </summary>
		/// <param name="retrieveMessage">Message used to retrieve the Information Model.</param>
		/// <returns>File list - containing the filenames of all instances matching the retrieve dataset attributes.</returns>
		public DvtkData.Collections.StringCollection RetrieveInformationModel(DicomMessage retrieveMessage)
		{
			PatientRootInformationModel root = (PatientRootInformationModel)Root;
			return root.RetrieveInformationModel(retrieveMessage.DataSet.DvtkDataDataSet);
		}
		#endregion
	}
}
