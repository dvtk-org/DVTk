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
	/// Summary description for EvidenceCreatorActor.
	/// </summary>
	public class EvidenceCreatorActor : BaseActor, IEvidenceCreatorActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public EvidenceCreatorActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.EvidenceCreator, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Storage Commitment [RAD-10]
			AddDicomServer(DicomServerTypeEnum.DicomStorageCommitServer, ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);

			// for sending Creator Procedure Step In Progress [RAD-20]
			// for sending Creator Procedure Step Completed [RAD-21]
			AddDicomClient(DicomClientTypeEnum.DicomMppsClient, ActorTypeEnum.PerformedProcedureStepManager, commonConfig, peerToPeerConfigCollection);

			// for sending Creator Images Stored [RAD-18]
			AddDicomClient(DicomClientTypeEnum.DicomStorageClient, ActorTypeEnum.ImageArchive, commonConfig, peerToPeerConfigCollection);

			// for sending Storage Commitment [RAD-10]
			AddDicomClient(DicomClientTypeEnum.DicomStorageCommitClient, ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);
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
				case ActorTypeEnum.ImageManager:
					// received Storage Commitment [RAD-10]
					break;
				default:
					break;
			}
		}	

		#region SendCreatorProcedureStepInProgress() overloads
		public bool SendNCreateCreatorProcedureStepInProgress()
		{
			// RAD-20
            return false;
		}

		public bool SendNSetCreatorProcedureStepInProgress()
		{
			// RAD-20
            return false;
        }

		#endregion SendCreatorProcedureStepInProgress() overloads

		#region SendCreatorProcedureStepCompletedDiscontinued() overloads
		public bool SendNSetCreatorProcedureStepCompleted()
		{
			// RAD-21
            return false;
        }

		public bool SendNSetCreatorProcedureStepDiscontinued()
		{
			// RAD-21
            return false;
        }

		#endregion SendCreatorProcedureStepCompletedDiscontinued()

		#region SendCreatorImagesStored() overloads
		public bool SendCreatorImagesStored()
		{
			// RAD-18
            return false;
        }

		#endregion SendCreatorImagesStored() overloads

		#region SendStorageCommitment() overloads
		public bool SendStorageCommitment()
		{
			// RAD-10
            return false;
        }

		#endregion SendStorageCommitment() overloads

	}
}
