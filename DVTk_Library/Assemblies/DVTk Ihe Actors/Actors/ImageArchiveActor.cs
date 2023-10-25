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

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for ImageArchiveActor.
	/// </summary>
	public class ImageArchiveActor : BaseActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public ImageArchiveActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.ImageArchive, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Modality Images Stored [RAD-8]
			AddDicomServer(DicomServerTypeEnum.DicomStorageServer, ActorTypeEnum.AcquisitionModality, commonConfig, peerToPeerConfigCollection);

			// for receiving Creator Images Stored [RAD-18]
			AddDicomServer(DicomServerTypeEnum.DicomStorageServer, ActorTypeEnum.EvidenceCreator, commonConfig, peerToPeerConfigCollection);

			// for receiving Query Images [RAD-14]
			// for receiving Retrieve Images [RAD-16]
			AddDicomServer(DicomServerTypeEnum.DicomQueryRetrieveServer, ActorTypeEnum.ImageDisplay, commonConfig, peerToPeerConfigCollection);

			// for sending Retrieve Images [RAD-16]
			UpdateDicomServer(this.ActorName, ActorTypeEnum.ImageDisplay, commonConfig, peerToPeerConfigCollection);
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
				case ActorTypeEnum.AcquisitionModality:
					// received Modality Images Stored [RAD-8]
					break;
				case ActorTypeEnum.EvidenceCreator:
					// received Creator Images Stored [RAD-18]
					break;
				case ActorTypeEnum.ImageDisplay:
					// received Query Images [RAD-14] or
					// received Retrieve Images [RAD-16]
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Handle the HL7 message update from the Image Manager Actor.
		/// </summary>
		/// <param name="message">HL7 message from Image Manager Actor.</param>
		public void HandleUpdateFromImageManagerActor(Hl7Message message)
		{
			switch(message.MessageType)
			{
				case "ADT" :
					{
						switch(message.MessageSubType)
						{
							case "A08" :
								HandlePatientUpdate(message);
								break;
							case "A40" :
								HandlePatientMerge(message);
								break;
						}
					}
					break;

				case "ORM" :
					{
						switch(message.MessageSubType)
						{
							case "O01" :
								HandleProcedureScheduled(message);
								HandleProcedureUpdated(message);
								break;
						}
					}
					break;

				default:
					break;
			}
		}

		#region private transaction handlers
		private void HandlePatientUpdate(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Query Retrive Server
			// - try to get the Dicom Query Retrive Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.ImageDisplay);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.ImageDisplay, actorId));
			if (dicomServer == null) return;

			// update the patient with the Dicom Query Retrive Server
			DicomQueryRetrieveServer dicomQueryRetrieveServer = (DicomQueryRetrieveServer) dicomServer;
			Console.WriteLine("dicomQueryRetrieveServer.PatientUpdate(Hl7ToDicom(message));");
		}

		private void HandlePatientMerge(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Query Retrive Server
			// - try to get the Dicom Query Retrive Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.ImageDisplay);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.ImageDisplay, actorId));
			if (dicomServer == null) return;

			// merge the patient with the Dicom Query Retrive Server
			DicomQueryRetrieveServer dicomQueryRetrieveServer = (DicomQueryRetrieveServer) dicomServer;
			Console.WriteLine("dicomQueryRetrieveServer.PatientMerge(Hl7ToDicom(message));");
		}

		private void HandleProcedureScheduled(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Query Retrive Server
			// - try to get the Dicom Query Retrive Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.ImageDisplay);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.ImageDisplay, actorId));
			if (dicomServer == null) return;

			// merge the patient with the Dicom Query Retrive Server
			DicomQueryRetrieveServer dicomQueryRetrieveServer = (DicomQueryRetrieveServer) dicomServer;
			Console.WriteLine("dicomQueryRetrieveServer.ProcedureScheduled(Hl7ToDicom(message));");
		}

		private void HandleProcedureUpdated(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Query Retrive Server
			// - try to get the Dicom Query Retrive Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.ImageDisplay);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.ImageDisplay, actorId));
			if (dicomServer == null) return;

			// merge the patient with the Dicom Query Retrive Server
			DicomQueryRetrieveServer dicomQueryRetrieveServer = (DicomQueryRetrieveServer) dicomServer;
			Console.WriteLine("dicomQueryRetrieveServer.ProcedureUpdated(Hl7ToDicom(message));");
		}

		#endregion private transaction handlers
	}
}
