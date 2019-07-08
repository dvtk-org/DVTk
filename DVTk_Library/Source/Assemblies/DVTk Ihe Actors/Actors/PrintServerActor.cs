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
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Dicom;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for PrintServerActor.
	/// </summary>
	public class PrintServerActor : BaseActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public PrintServerActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.PrintServer, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Print Request with Presentation LUT [RAD-23]
			AddDicomServer(DicomServerTypeEnum.DicomPrintServer, ActorTypeEnum.PrintComposer, commonConfig, peerToPeerConfigCollection);
		}

		/// <summary>
		/// Handle a Dicom Transaction from the given Actor Name.
		/// </summary>
		/// <param name="actorName">Source Actor Name.</param>
		/// <param name="dicomTransaction">Dicom Transaction.</param>
        protected override void HandleTransactionFrom(ActorName actorName, DicomTransaction dicomTransaction)
		{
			switch (actorName.Type)
			{
				case ActorTypeEnum.PrintComposer:
					// received Print Request with Presentation LUT [RAD-23]
					break;
				default:
					break;
			}
		}	
	}
}
