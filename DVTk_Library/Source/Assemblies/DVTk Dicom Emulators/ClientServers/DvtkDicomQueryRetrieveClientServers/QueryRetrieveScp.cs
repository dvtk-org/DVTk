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
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Common.UserInterfaces;
using Dvtk.DvtkDicomEmulators.Bases;
using Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers;

namespace Dvtk.DvtkDicomEmulators.QueryRetrieveClientServers
{
	/// <summary>
	/// Summary description for QueryRetrieveScp.
	/// </summary>
	public class QueryRetrieveScp : HliScp
	{
		/// <summary>
		/// Class Constructor.
		/// </summary>
		public QueryRetrieveScp() : base()		
		{
			// Do nothing.
		}

		/// <summary>
		/// Add the default message handlers - include the Information Models that should be used.
		/// </summary>
		/// <param name="informationModels">Query Retrieve Information Models.</param>
		public void AddDefaultMessageHandlers(QueryRetrieveInformationModels informationModels)
		{
			// add the CFindHandler with the Information Models
			CFindHandler cFindHandler = new CFindHandler();
			if (informationModels.PatientRoot != null)
			{
				cFindHandler.PatientRootInformationModel = informationModels.PatientRoot;
			}
			if (informationModels.StudyRoot != null)
			{
				cFindHandler.StudyRootInformationModel = informationModels.StudyRoot;
			}
			if (informationModels.PatientStudyOnly != null)
			{
				cFindHandler.PatientStudyOnlyInformationModel = informationModels.PatientStudyOnly;
			}
			cFindHandler.RefreshInformationModelBeforeUse = true;
			AddToBack(cFindHandler);

			// add the CMoveHandler with the Information Models
			CMoveHandler cMoveHandler = new CMoveHandler();
			if (informationModels.PatientRoot != null)
			{
				cMoveHandler.PatientRootInformationModel = informationModels.PatientRoot;
			}
			if (informationModels.StudyRoot != null)
			{
				cMoveHandler.StudyRootInformationModel = informationModels.StudyRoot;
			}
			if (informationModels.PatientStudyOnly != null)
			{
				cMoveHandler.PatientStudyOnlyInformationModel = informationModels.PatientStudyOnly;
			}
			cMoveHandler.RefreshInformationModelBeforeUse = true;
			AddToBack(cMoveHandler);

			// add the CGetHandler with the Information Models
			CGetHandler cGetHandler = new CGetHandler();
			if (informationModels.PatientRoot != null)
			{
				cGetHandler.PatientRootInformationModel = informationModels.PatientRoot;
			}
			if (informationModels.StudyRoot != null)
			{
				cGetHandler.StudyRootInformationModel = informationModels.StudyRoot;
			}
			if (informationModels.PatientStudyOnly != null)
			{
				cGetHandler.PatientStudyOnlyInformationModel = informationModels.PatientStudyOnly;
			}
			cGetHandler.RefreshInformationModelBeforeUse = true;
			AddToBack(cGetHandler);
		}
	}
}
