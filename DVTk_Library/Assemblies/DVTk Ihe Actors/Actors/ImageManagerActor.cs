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

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using DvtkHighLevelInterface.Dicom.Messages;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for ImageManagerActor.
	/// </summary>
	public class ImageManagerActor : BaseActor, IImageManagerActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public ImageManagerActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.ImageManager, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Procedure Scheduled [RAD-4]
			// for receiving Patient Update [Rad-12]
			// for receiving Procedure Updated [RAD-13]
			AddHl7Server(Hl7ServerTypeEnum.Hl7Server, ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);

			// for receiving Image Availability Query [RAD-11]
			// for receiving Performed Work Status Update [RAD-42]
//			AddDicomServer(DicomServerTypeEnum., ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);

			// for receiving Modality Procedure Step In Progress [RAD-6]
			// for receiving Modality Procedure Step Completed [RAD-7]
			// for receiving Creator Procedure Step In Progress [RAD-20]
			// for receiving Creator Procedure Step Completed [RAD-21]
			AddDicomServer(DicomServerTypeEnum.DicomMppsServer, ActorTypeEnum.PerformedProcedureStepManager, commonConfig, peerToPeerConfigCollection);

			// for receiving Storage Commitment [RAD-10]
			AddDicomServer(DicomServerTypeEnum.DicomStorageCommitServer, ActorTypeEnum.AcquisitionModality, commonConfig, peerToPeerConfigCollection);

			// for receiving Storage Commitment [RAD-10]
			AddDicomServer(DicomServerTypeEnum.DicomStorageCommitServer, ActorTypeEnum.EvidenceCreator, commonConfig, peerToPeerConfigCollection);

			// for receiving Image Availability Query [RAD-11]
			AddDicomServer(DicomServerTypeEnum.DicomQueryRetrieveServer, ActorTypeEnum.ReportManager, commonConfig, peerToPeerConfigCollection);

			// for sending Performed Work Status Update [RAD-42]
			// for sending Instance Availability Notification [RAD-49]
//			AddDicomClient(DicomClientTypeEnum., ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);

			// for sending Storage Commitment [RAD-10]
			AddDicomClient(DicomClientTypeEnum.DicomStorageCommitClient, ActorTypeEnum.AcquisitionModality, commonConfig, peerToPeerConfigCollection);

			// for sending Storage Commitment [RAD-10]
			AddDicomClient(DicomClientTypeEnum.DicomStorageCommitClient, ActorTypeEnum.EvidenceCreator, commonConfig, peerToPeerConfigCollection);
		}

		/// <summary>
		/// Handle an HL7 Transation from the given Actor Name.
		/// </summary>
		/// <param name="actorName">Source Actor Name.</param>
		/// <param name="hl7Transaction">HL7 Transaction.</param>
        protected override void HandleTransactionFrom(ActorName actorName, Hl7Transaction hl7Transaction)
		{
			Hl7Message request = hl7Transaction.Request;

			switch (actorName.Type)
			{
				case ActorTypeEnum.DssOrderFiller:
					// received Procedure Scheduled [RAD-4] or
					// received Patient Update [Rad-12] or
					// received Procedure Updated [RAD-13]
					UpdateImageArchiveActor(request);
					break;
				default:
					break;
			}
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
				case ActorTypeEnum.DssOrderFiller:
					// received Image Availability Query [RAD-11] or
					// received Performed Work Status Update [RAD-42]
					break;
				case ActorTypeEnum.PerformedProcedureStepManager:
					// received Modality Procedure Step In Progress [RAD-6] or
					// received Modality Procedure Step Completed [RAD-7] or
					// received Creator Procedure Step In Progress [RAD-20] or
					// received Creator Procedure Step Completed [RAD-21]
					break;
				case ActorTypeEnum.AcquisitionModality:
					// received Storage Commitment [RAD-10]
					break;
				case ActorTypeEnum.EvidenceCreator:
					// received Storage Commitment [RAD-10]
					break;
				case ActorTypeEnum.ReportManager:
					// received Image Availability Query [RAD-11]
					break;
				default:
					break;
			}
		}	

		#region private transaction handlers
		private void UpdateImageArchiveActor(Hl7Message message)
		{
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.ImageArchive);
            ImageArchiveActor imageArchiveActor = (ImageArchiveActor)IheFramework.GetActor(new ActorName(ActorTypeEnum.ImageArchive, actorId));
			if (imageArchiveActor != null)
			{
				imageArchiveActor.HandleUpdateFromImageManagerActor(message);
			}
		}

		#endregion private transaction handlers

		#region SendPerformedWorkStatusUpdate() overloads
		public bool SendPerformedWorkStatusUpdate()
		{
			// RAD-42
            return false;
        }

		#endregion SendPerformedWorkStatusUpdate() overloads

		#region SendInstanceAvailabilityNotification() overloads
		public bool SendInstanceAvailabilityNotification()
		{
			// RAD-49
            return false;
        }

		#endregion SendInstanceAvailabilityNotification() overloads
	}
}



