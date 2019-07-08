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
using Dvtk.DvtkDicomEmulators.Bases;
using Dvtk.DvtkDicomEmulators.WorklistClientServers;
using Dvtk.DvtkDicomEmulators.WorklistMessageHandlers;
using Dvtk.IheActors.Bases;
using VR = DvtkData.Dimse.VR;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomWorklistServer.
	/// </summary>
	public class DicomWorklistServer : DicomServer
	{
		private ModalityWorklistInformationModel _modalityWorklistInformationModel  = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - containing actor.</param>
		/// <param name="actorName">Destination Actor Name.</param>
		public DicomWorklistServer(BaseActor parentActor, ActorName actorName) : base (parentActor, actorName) {}

		/// <summary>
		/// Apply the Dicom Configuration to the Client,
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">Dicom Configuration.</param>
		public override void ApplyConfig(CommonConfig commonConfig, DicomPeerToPeerConfig config)
		{
			// set up the worklist SCP
			WorklistScp worklistScp = new WorklistScp();

			// update supported transfer syntaxes here
			//worklistScp.ClearTransferSyntaxes();
			//worklistScp.AddTransferSyntax(HliScp.IMPLICIT_VR_LITTLE_ENDIAN);

			// Save the SCP and apply the configuration
			Scp = worklistScp;
			base.ApplyConfig(commonConfig, config);

			// set up the Modality Worklist information models
			_modalityWorklistInformationModel = new ModalityWorklistInformationModel();

			// load the information models
			_modalityWorklistInformationModel.LoadInformationModel(RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, config.SourceDataDirectory));

			// add any default attribute values to the information models
			bool overWriteExistingValue = true;
			_modalityWorklistInformationModel.AddDefaultAttributeToInformationModel(overWriteExistingValue, "0x00400001", VR.AE, config.FromActorAeTitle);
			_modalityWorklistInformationModel.AddDefaultAttributeToInformationModel(overWriteExistingValue, "0x00400002", VR.DA, System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
			_modalityWorklistInformationModel.AddDefaultAttributeToInformationModel(overWriteExistingValue, "0x00400003", VR.TM, System.DateTime.Now.ToString("HHmmss", System.Globalization.CultureInfo.InvariantCulture));

			// add any additional attribute values to the information models
			//modalityWorklistInformationModel.AddAdditionalAttributeToInformationModel(overWriteExistingValue, "0x00080054", VR.AE, config.DvtAeTitle);

			// add the default message handlers with the information model
			worklistScp.AddDefaultMessageHandlers(_modalityWorklistInformationModel);
		}

		#region Transaction handlers
		/// <summary>
		/// Patient Registration request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient registration attributes.</param>
		public void PatientRegistration(DvtkData.Dimse.DataSet dataset)
		{
			_modalityWorklistInformationModel.PatientRegistration(dataset);
		}

		/// <summary>
		/// Patient Update request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient update attributes.</param>
		public void PatientUpdate(DvtkData.Dimse.DataSet dataset)
		{
			_modalityWorklistInformationModel.PatientUpdate(dataset);
		}

		/// <summary>
		/// Patient merge request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient merge attributes.</param>
		public void PatientMerge(DvtkData.Dimse.DataSet dataset)
		{
			_modalityWorklistInformationModel.PatientMerge(dataset);
		}

		/// <summary>
		/// Placer order management request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing placer order management attributes.</param>
		public void PlacerOrderManagement(DvtkData.Dimse.DataSet dataset)
		{
			_modalityWorklistInformationModel.PlacerOrderManagement(dataset);
		}
		#endregion Transaction handlers
	}
}
