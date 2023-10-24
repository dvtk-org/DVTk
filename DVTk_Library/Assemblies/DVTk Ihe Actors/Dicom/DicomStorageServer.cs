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
using Dvtk.DvtkDicomEmulators.StorageClientServers;
using Dvtk.DvtkDicomEmulators.StorageMessageHandlers;
using Dvtk.IheActors.Bases;
using VR = DvtkData.Dimse.VR;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomStorageServer.
	/// </summary>
	public class DicomStorageServer : DicomServer
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - containing actor.</param>
		/// <param name="actorName">Destination Actor Name.</param>
		public DicomStorageServer(BaseActor parentActor, ActorName actorName) : base (parentActor, actorName) {}

		/// <summary>
		/// Apply the Dicom Configuration to the Client,
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">Dicom Configuration.</param>
		public override void ApplyConfig(CommonConfig commonConfig, DicomPeerToPeerConfig config)
		{
			// set up the storage SCP
			StorageScp storageScp = new StorageScp();

			// update supported transfer syntaxes here
			//storageScp.ClearTransferSyntaxes();
			//storageScp.AddTransferSyntax(HliScp.IMPLICIT_VR_LITTLE_ENDIAN);

			// Save the SCP and apply the configuration
			Scp = storageScp;
			base.ApplyConfig(commonConfig, config);

			// add the default message handlers
			storageScp.AddDefaultMessageHandlers();
		}
	}
}
