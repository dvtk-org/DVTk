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

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;

namespace Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers
{
	/// <summary>
	/// Summary description for QueryRetrieveHandler.
	/// </summary>
	public abstract class QueryRetrieveHandler : MessageHandler
	{
		private QueryRetrievePatientRootInformationModel _queryRetrievePatientRootInformationModel = null;
		private QueryRetrieveStudyRootInformationModel _queryRetrieveStudyRootInformationModel = null;
		private QueryRetrievePatientStudyOnlyInformationModel _queryRetrievePatientStudyOnlyInformationModel = null;
		private bool _refreshInformationModelBeforeUse = false;

		public QueryRetrieveHandler() {}

		/// <summary>
		/// Property - PatientRootInformationModel
		/// </summary>
		public QueryRetrievePatientRootInformationModel PatientRootInformationModel
		{
			set
			{
				_queryRetrievePatientRootInformationModel = value;
			}
			get
			{
				return _queryRetrievePatientRootInformationModel;
			}
		}

		/// <summary>
		/// Property - StudyRootInformationModel
		/// </summary>
		public QueryRetrieveStudyRootInformationModel StudyRootInformationModel
		{
			set
			{
				_queryRetrieveStudyRootInformationModel = value;
			}
			get
			{
				return _queryRetrieveStudyRootInformationModel;
			}
		}

		/// <summary>
		/// Property - PatientStudyOnlyInformationModel
		/// </summary>
		public QueryRetrievePatientStudyOnlyInformationModel PatientStudyOnlyInformationModel
		{
			set
			{
				_queryRetrievePatientStudyOnlyInformationModel = value;
			}
			get
			{
				return _queryRetrievePatientStudyOnlyInformationModel;
			}
		}

		/// <summary>
		/// Property - RefreshInformationModelBeforeUse
		/// </summary>
		public bool RefreshInformationModelBeforeUse
		{
			set
			{
				_refreshInformationModelBeforeUse = value;
			}
			get
			{
				return _refreshInformationModelBeforeUse;
			}
		}
	}
}
