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
using Dvtk.DvtkDicomEmulators.QueryRetrieveClientServers;
using Dvtk.DvtkDicomEmulators.QueryRetrieveMessageHandlers;
using Dvtk.IheActors.Bases;
using VR = DvtkData.Dimse.VR;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomQueryRetrieveServer.
	/// </summary>
	public class DicomQueryRetrieveServer : DicomServer
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - containing actor.</param>
		/// <param name="actorName">Destination Actor Name.</param>
		public DicomQueryRetrieveServer(BaseActor parentActor, ActorName actorName) : base (parentActor, actorName) {}

		/// <summary>
		/// Apply the Dicom Configuration to the Client,
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">Dicom Configuration.</param>
		public override void ApplyConfig(CommonConfig commonConfig, DicomPeerToPeerConfig config)
		{
			// set up the Query/Retrieve information models
			QueryRetrieveInformationModels informationModels = new QueryRetrieveInformationModels();

			// Do not load the Information Models here as they are loaded (refreshed) on each query
			// - just define the data directory
			informationModels.DataDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, config.SourceDataDirectory);

			// add any default attribute values to the information models
			bool overWriteExistingValue = true;
			informationModels.AddDefaultAttribute(overWriteExistingValue, "0x00080005", VR.CS, "ISO IR 6");
			informationModels.AddDefaultAttribute(overWriteExistingValue, "0x00080090", VR.PN, "Referring^Physician^Dr");

			// add any additional attribute values to the information models
			informationModels.AddAdditionalAttribute(overWriteExistingValue, "0x00080054", VR.AE, config.ToActorAeTitle);

			// set up the query/retrieve SCP
			QueryRetrieveScp queryRetrieveScp = new QueryRetrieveScp();

			// update supported transfer syntaxes here
			//queryRetrieveScp.ClearTransferSyntaxes();
			//queryRetrieveScp.AddTransferSyntax(HliScp.IMPLICIT_VR_LITTLE_ENDIAN);

			// Save the SCP and apply the configuration
			Scp = queryRetrieveScp;
			base.ApplyConfig(commonConfig, config);

			// add the default message handlers with the information models
			queryRetrieveScp.AddDefaultMessageHandlers(informationModels);
		}
	}
}
