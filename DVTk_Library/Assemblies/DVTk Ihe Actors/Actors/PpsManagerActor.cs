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
	/// Summary description for PpsManagerActor.
	/// </summary>
	public class PpsManagerActor : BaseActor, IPpsManagerActor
	{
		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public PpsManagerActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.PerformedProcedureStepManager, id), iheFramework) { }

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Modality Procedure Step In Progress [RAD-6]
			// for receiving Modality Procedure Step Completed [RAD-7]
			AddDicomServer(DicomServerTypeEnum.DicomMppsServer, ActorTypeEnum.AcquisitionModality, commonConfig, peerToPeerConfigCollection);

			// for receiving Creator Procedure Step In Progress [RAD-20]
			// for receiving Creator Procedure Step Completed [RAD-21]
			AddDicomServer(DicomServerTypeEnum.DicomMppsServer, ActorTypeEnum.EvidenceCreator, commonConfig, peerToPeerConfigCollection);

			// for sending Modality Procedure Step In Progress [RAD-6]
			// for sending Modality Procedure Step Completed [RAD-7]
			// for sending Creator Procedure Step In Progress [RAD-20]
			// for sending Creator Procedure Step Completed [RAD-21]
			AddDicomClient(DicomClientTypeEnum.DicomMppsClient, ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);

			// for sending Modality Procedure Step In Progress [RAD-6]
			// for sending Modality Procedure Step Completed [RAD-7]
			// for sending Creator Procedure Step In Progress [RAD-20]
			// for sending Creator Procedure Step Completed [RAD-21]
			AddDicomClient(DicomClientTypeEnum.DicomMppsClient, ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);
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
					// received Modality Procedure Step In Progress [RAD-6] or
					// received Modality Procedur Step Completed [RAD-7]
				case ActorTypeEnum.EvidenceCreator:
				{

					// received Creator Procedure Step In Progress [RAD-20] or
					// received Creator Procedure Step Completed [RAD-21]
					TransactionNameEnum transactionName = dicomTransaction.TransactionName;
					DicomMessage dicomMessage = (DicomMessage)dicomTransaction.DicomMessages[0];

					// ignore the C-ECHO-RQ used in the Verification SOP Class
					// - no need to forward this
					if (dicomMessage.CommandSet.DimseCommand != DvtkData.Dimse.DimseCommand.CECHORQ)
					{
						// make a trigger from the transaction message
						DicomTrigger dicomTrigger = new DicomTrigger(transactionName);
						dicomTrigger.AddItem(dicomMessage,
							"1.2.840.10008.3.1.2.3.3",
							"1.2.840.10008.1.2");

						// trigger the following actors
                        bool triggerResult = TriggerActorInstances(ActorTypeEnum.DssOrderFiller, dicomTrigger, false);
                        if (triggerResult == true)
                        {
                            triggerResult = TriggerActorInstances(ActorTypeEnum.ImageManager, dicomTrigger, false);
                        }
					}
					break;
				}
				default:
					break;
			}
		}

		#region SendModalityProcedureStepInProgress() overloads
		public bool SendModalityProcedureStepInProgress()
		{
			// RAD-6
            return false;
		}

		#endregion SendModalityProcedureStepInProgress() overloads

		#region SendModalityProcedureStepCompleted() overloads
		public bool SendModalityProcedureStepCompleted()
		{
			// RAD-7
            return false;
		}

		#endregion SendModalityProcedureStepCompleted() overloads

		#region SendModalityProcedureStepDiscontinued() overloads
		public bool SendModalityProcedureStepDiscontinued()
		{
			// RAD-7
            return false;
		}

		#endregion SendModalityProcedureStepDiscontinued() overloads

		#region SendCreatorProcedureStepInProgress() overloads
		public bool SendCreatorProcedureStepInProgress()
		{
			// RAD-20
            return false;
		}

		#endregion SendCreatorProcedureStepInProgress() overloads

		#region SendCreatorProcedureStepCompleted() overloads
		public bool SendCreatorProcedureStepCompleted()
		{
			// RAD-21
            return false;
		}

		#endregion SendCreatorProcedureStepCompleted() overloads

		#region SendCreatorProcedureStepDiscontinued() overloads
		public bool SendCreatorProcedureStepDiscontinued()
		{
			// RAD-21
            return false;
		}

		#endregion SendCreatorProcedureStepDiscontinued() overloads
	}
}
