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
using Dvtk.DvtkDicomEmulators.Bases;
using Dvtk.DvtkDicomEmulators.PrintClientServers;
using Dvtk.IheActors.Bases;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomPrintServer.
	/// </summary>
	public class DicomPrintServer : DicomServer
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - containing actor.</param>
		/// <param name="actorName">Destination Actor Name.</param>
		public DicomPrintServer(BaseActor parentActor, ActorName actorName) : base (parentActor, actorName) {}

		/// <summary>
		/// Apply the Dicom Configuration to the Client,
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">Dicom Configuration.</param>
		public override void ApplyConfig(CommonConfig commonConfig, DicomPeerToPeerConfig config)
		{
			// Instantiate a new Print SCP and add the default message handlers
			PrintScp printScp = new PrintScp();
			printScp.AddDefaultMessageHandlers();

			// update supported transfer syntaxes here
			//printScp.ClearTransferSyntaxes();
			//printScp.AddTransferSyntax(HliScp.IMPLICIT_VR_LITTLE_ENDIAN);

			// Save the SCP and apply the configuration
			Scp = printScp;
			base.ApplyConfig(commonConfig, config);
		}
	}
}
