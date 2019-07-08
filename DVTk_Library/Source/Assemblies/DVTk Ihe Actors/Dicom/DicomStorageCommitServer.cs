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
using Dvtk.DvtkDicomEmulators.StorageCommitClientServers;
using Dvtk.DvtkDicomEmulators.StorageCommitMessageHandlers;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;
using VR = DvtkData.Dimse.VR;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomStorageCommitServer.
	/// </summary>
	public class DicomStorageCommitServer : DicomServer
	{
		private QueryRetrieveInformationModels _informationModels = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - containing actor.</param>
		/// <param name="actorName">Destination Actor Name.</param>
		public DicomStorageCommitServer(BaseActor parentActor, ActorName actorName) : base (parentActor, actorName)
		{
			// set up the Query/Retrieve information models
			_informationModels = new QueryRetrieveInformationModels();
		}

		/// <summary>
		/// Apply the Dicom Configuration to the Client,
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">Dicom Configuration.</param>
		public override void ApplyConfig(CommonConfig commonConfig, DicomPeerToPeerConfig config)
		{
			// Do not load the Information Models here as hey are loaded (refreshed) on each storage commitment event
			// - just define the data directory
			_informationModels.DataDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, config.SourceDataDirectory);

			// add any default attribute values to the information models
			bool overWriteExistingValue = true;
			_informationModels.AddDefaultAttribute(overWriteExistingValue, "0x00080005", VR.CS, "ISO IR 6");
			_informationModels.AddDefaultAttribute(overWriteExistingValue, "0x00080090", VR.PN, "Referring^Physician^Dr");

			// set up the storage commit SCP
			StorageCommitScp storageCommitScp = new StorageCommitScp();

			// update supported transfer syntaxes here
			//storageCommitScp.ClearTransferSyntaxes();
			//storageCommitScp.AddTransferSyntax(HliScp.IMPLICIT_VR_LITTLE_ENDIAN);

			// Save the SCP and apply the configuration
			Scp = storageCommitScp;
			base.ApplyConfig(commonConfig, config);

            // use the common config option 1 to determine how the storage commitment should be handled
            if (ConfigOption1.Equals("DO_STORAGE_COMMITMENT_ON_SINGLE_ASSOCIATION") == true)
            {
                // add the default message handlers with the information model
                // - allows support for the NActionNEventReportHandler which
                // returns an N-EVENT-REPORT-RQ on the same association as the
                // N-ACTION-RQ was received.
                storageCommitScp.AddDefaultMessageHandlers(_informationModels, 5000);
            }
            else
            {
			    // add the default message handlers
    			storageCommitScp.AddDefaultMessageHandlers();
            }

        }

        /// <summary>
        /// Handle the Mesage Received Event for DICOM N-ACTION-RQ messages.
        /// </summary>
        /// <param name="dicomProtocolMessage">Received DICOM Protocol Message.</param>
        public override void ScpMessageReceivedEventHandler(DicomProtocolMessage dicomProtocolMessage)
        {
            // use the common config option 1 to determine how the storage commitment should be handled
            if (ConfigOption1.Equals("DO_STORAGE_COMMITMENT_ON_SINGLE_ASSOCIATION") == false)
            {
                if (dicomProtocolMessage is DicomMessage)
                {
                    DicomMessage dicomMessage = (DicomMessage)dicomProtocolMessage;

                    // check for the N-ACTION-RQ
                    if (dicomMessage.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.NACTIONRQ)
                    {
                        // produce a DICOM trigger for the Storage Commitment SCU - N-EVENT-REPORT-RQ
                        DicomTrigger storageCommitTrigger = GenerateTrigger(dicomMessage);
                        bool triggerResult = ParentActor.TriggerActorInstances(ActorName.Type, storageCommitTrigger, false);
                    }
                }
            }

            // call base implementation to generate event, update transaction log and cleanup data warehouse
            base.ScpMessageReceivedEventHandler(dicomProtocolMessage);
        }

		private DicomTrigger GenerateTrigger(DicomMessage dicomMessage)
		{
			DicomTrigger storageCommitTrigger = new DicomTrigger(TransactionNameEnum.RAD_10);
			storageCommitTrigger.AddItem(GenerateTriggers.MakeStorageCommitEvent(_informationModels, dicomMessage),
										"1.2.840.10008.1.20.1",
										"1.2.840.10008.1.2");

			return storageCommitTrigger;
		}
	}
}
