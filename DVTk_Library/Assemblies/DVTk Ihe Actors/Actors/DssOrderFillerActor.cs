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
using Dvtk.Comparator.Convertors;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for DssOrderFillerActor.
	/// </summary>
	public class DssOrderFillerActor : BaseActor, IDssOrderFillerActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public DssOrderFillerActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.DssOrderFiller, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Patient Registration [RAD-1]
			// for receiving Patient Update [RAD-12]
			AddHl7Server(Hl7ServerTypeEnum.Hl7Server, ActorTypeEnum.AdtPatientRegistration, commonConfig, peerToPeerConfigCollection);

			// for receiving Placer Order Management [RAD-2]
			AddHl7Server(Hl7ServerTypeEnum.Hl7Server, ActorTypeEnum.OrderPlacer, commonConfig, peerToPeerConfigCollection);

			// for receiving Query Modality Worklist [RAD-5]
			AddDicomServer(DicomServerTypeEnum.DicomWorklistServer, ActorTypeEnum.AcquisitionModality, commonConfig, peerToPeerConfigCollection);

			// for receiving Modality Procedure Step In Progress [RAD-6]
			// for receiving Modality Procedure Step Completed [RAD-7]
			// for receiving Creator Procedure Step In Progress [RAD-20]
			// for receiving Creator Procedure Step Completed [RAD-21]
			AddDicomServer(DicomServerTypeEnum.DicomMppsServer, ActorTypeEnum.PerformedProcedureStepManager, commonConfig, peerToPeerConfigCollection);

			// for receiving Instance Availability Notification [RAD-49]
//			AddDicomServer(DicomServerTypeEnum., ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);

			// for sending Filler Order Management [RAD-3]
			// for sending Appointment Notification [RAD-48]
			AddHl7Client(Hl7ClientTypeEnum.Hl7Client, ActorTypeEnum.OrderPlacer, commonConfig, peerToPeerConfigCollection);

			// for sending Procedure Scheduled [RAD-4]
			// for sending Patient Update [Rad-12]
			// for sending Procedure Updated [RAD-13]
			AddHl7Client(Hl7ClientTypeEnum.Hl7Client, ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);

			// for sending Image Availability Query [RAD-11]
			// for sending Performed Work Status Update [RAD-42]
//			AddDicomClient(DicomClientTypeEnum., ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);

			// for sending Patient Query
			AddHl7Client(Hl7ClientTypeEnum.Hl7Client, ActorTypeEnum.AdtPatientRegistration, commonConfig, peerToPeerConfigCollection);
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
				case ActorTypeEnum.AdtPatientRegistration:
					// received Patient Registration [RAD-1] or
					// received Patient Update [RAD-12]
					if (request.MessageType == "ADT")
					{
						switch(request.MessageSubType)
						{
							case "A01" :
								{
//									HandlePatientRegistration(request);
								}
								break;

							case "A08" :
								{
									HandlePatientUpdate(request);

									// Generate trigger
									Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_12);
									trigger.Trigger = request;

									// RAD-12 - trigger the ImageManager
                                    bool triggerResult = TriggerActorInstances(ActorTypeEnum.ImageManager, trigger, false);
								}
								break;

							case "A40" :
								{
									HandlePatientMerge(request);

									// Generate trigger
									Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_12);
									trigger.Trigger = request;

									// RAD-12 - trigger the ImageManager
                                    bool triggerResult = TriggerActorInstances(ActorTypeEnum.ImageManager, trigger, false);
								}
								break;
						}
					}
					break;
				case ActorTypeEnum.OrderPlacer:
					// received Placer Order Management [RAD-2]
					if (request.MessageType == "ORM")
					{
						switch(request.MessageSubType)
						{
							case "O01" :
								HandlePlacerOrderManagement(request);
								break;
						}
					}
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
				case ActorTypeEnum.AcquisitionModality:
					// received Query Modality Worklist [RAD-5]
					break;
				case ActorTypeEnum.PerformedProcedureStepManager:
					// received Modality Procedure Step In Progress [RAD-6] or
					// received Modality Procedure Step Completed [RAD-7] or
					// received Creator Procedure Step In Progress [RAD-20] or
					// received Creator Procedure Step Completed [RAD-21]
					break;
				case ActorTypeEnum.ImageManager:
					// received Instance Availability Notification [RAD-49]
					break;
				default:
					break;
			}
		}

		#region private transaction handlers
		private void HandlePatientRegistration(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Worklist Server
			// - try to get the Dicom Worklist Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.AcquisitionModality);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.AcquisitionModality, actorId));
			if (dicomServer == null) return;

			// register the patient with the Dicom Worklist Server
			DicomWorklistServer dicomWorklistServer = (DicomWorklistServer) dicomServer;
			dicomWorklistServer.PatientRegistration(Hl7MessageToDicomMessageConvertor.Convert(message));
		}

		private void HandlePatientUpdate(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Worklist Server
			// - try to get the Dicom Worklist Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.AcquisitionModality);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.AcquisitionModality, actorId));
			if (dicomServer == null) return;

			// update the patient with the Dicom Worklist Server
			DicomWorklistServer dicomWorklistServer = (DicomWorklistServer) dicomServer;
			dicomWorklistServer.PatientUpdate(Hl7MessageToDicomMessageConvertor.Convert(message));
		}

		private void HandlePatientMerge(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Worklist Server
			// - try to get the Dicom Worklist Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.AcquisitionModality);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.AcquisitionModality, actorId));
			if (dicomServer == null) return;

			// merge the patient with the Dicom Worklist Server
			DicomWorklistServer dicomWorklistServer = (DicomWorklistServer) dicomServer;
			dicomWorklistServer.PatientMerge(Hl7MessageToDicomMessageConvertor.Convert(message));
		}

		private void HandlePlacerOrderManagement(Hl7Message message)
		{
			// affects the information model maintained by the Dicom Worklist Server
			// - try to get the Dicom Worklist Server
            String actorId = GetFirstActorIdFromDicomServer(ActorTypeEnum.AcquisitionModality);
            DicomServer dicomServer = GetDicomServer(new ActorName(ActorTypeEnum.AcquisitionModality, actorId));
			if (dicomServer == null) return;

			// placer order management with the Dicom Worklist Server
			DicomWorklistServer dicomWorklistServer = (DicomWorklistServer) dicomServer;
			dicomWorklistServer.PlacerOrderManagement(Hl7MessageToDicomMessageConvertor.Convert(message));
		}

		#endregion private transaction handlers

		#region SendFillerOrderManagement() overloads
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ormMessage"></param>
        /// <returns></returns>
		public bool SendFillerOrderManagement(OrmMessage ormMessage)
		{
			// RAD-3
            return false;
		}
		#endregion

		#region SendProcedureScheduled() overloads
		public bool SendProcedureScheduled(OrmMessage ormMessage)
		{
			HandlePlacerOrderManagement(ormMessage);

			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_4);
			trigger.Trigger = ormMessage;

			// RAD-4 - trigger the ImageManager
            return TriggerActorInstances(ActorTypeEnum.ImageManager, trigger, true);
		}

		public bool SendObservation(OruMessage oruMessage)
		{
			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_4);
			trigger.Trigger = oruMessage;

			// RAD-4 - trigger the ImageManager
			return TriggerActorInstances(ActorTypeEnum.ImageManager, trigger, true);
		}

		#endregion

		#region SendImageAvailabilityQuery() overloads
		public bool SendImageAvailabilityQuery()
		{
			// RAD-11
            return false;
		}
		#endregion

		#region SendPatientUpdate() overloads
		public bool SendPatientUpdate(AdtMessage adtMessage)
		{
			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_12);
			trigger.Trigger = adtMessage;

			// RAD-12 - trigger the ImageManager
			return TriggerActorInstances(ActorTypeEnum.ImageManager, trigger, true);
		}
		#endregion

		#region SendProcedureUpdated() overloads
		public bool SendProcedureUpdated(OrmMessage ormMessage)
		{
			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_13);
			trigger.Trigger = ormMessage;

			// RAD-13 - trigger the ImageManager
			return TriggerActorInstances(ActorTypeEnum.ImageManager, trigger, true);
		}
		#endregion

		#region SendPerformedWorkStatusUpdate() overloads
		public bool SendPerformedWorkStatusUpdate()
		{
			// RAD-42
            return false;
		}
		#endregion

		#region SendAppointmentNotification() overloads
		public bool SendAppointmentNotification()
		{
			// RAD-48
            return false;
		}
		#endregion

		#region SendPatientQuery() overloads
		public bool SendPatientQuery(QryMessage qryMessage)
		{
			// Generate trigger
			Hl7Trigger trigger = new Hl7Trigger(TransactionNameEnum.RAD_UNKNOWN);
			trigger.Trigger = qryMessage;

			// RAD-12 - trigger the AdtPatientRegistration
			return TriggerActorInstances(ActorTypeEnum.AdtPatientRegistration, trigger, true);
		}
		#endregion

	}
}
