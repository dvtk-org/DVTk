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
using System.Windows.Forms;
using System.IO;
using System.Collections;

using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Dicom.Files;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using Dvtk.DvtkDicomEmulators.StorageMessageHandlers;
using Dvtk.DvtkDicomEmulators.StorageClientServers;
using Dvtk.DvtkDicomEmulators.Bases;
using DvtkHighLevelInterface.Common.Threads;

namespace StorageSCPEmulator
{
	/// <summary>
	/// Summary description for OverviewThread.
	/// </summary>
	public class OverviewThread: DicomThread
	{
        public OverviewThread(DicomThreadOptions storageOptions, DicomThreadOptions commitOption, ArrayList selectedSOP, ArrayList selectedTS, ArrayList selectedTSCommit, int delay, String startDateTime)
		{
            this.storeOptions = storageOptions;
            this.commitOptions = commitOption;
            this.selectedSOPList = selectedSOP;

            foreach (string ts in selectedTS)
            {
                selectedTSList.Add(ts);
            }

            this.selectedTSCommitList = selectedTSCommit;
            this.eventDelay = delay*1000;
            this.startDateTime = startDateTime;
		}

		private DicomThreadOptions storeOptions = null;
		private DicomThreadOptions commitOptions = null;
        private ArrayList selectedSOPList = null;
		private ArrayList selectedTSList = new ArrayList();
        private ArrayList selectedTSCommitList = null;
        private int eventDelay = 0;
        private String startDateTime = String.Empty;
		
		protected override void Execute()
		{
            //
			// The Storage message handler.
			//
            CStoreHandler storeCStoreHandler = new CStoreHandler();

            //
			// Create the actual SCP's
			//
            if (this.storeOptions.LocalPort == this.commitOptions.LocalPort)
			{
				WriteInformation("Storage and Storage Commitment are configured to listen on same port.");

                StoreCommitScp storeCommitScp = new StoreCommitScp("Storage_and_Storage_Commitment_SCP_association_", this.startDateTime + "_Storage_and_Storage_Commitment_SCP_association_");
                storeCommitScp.StartDateTime = this.startDateTime;
                storeCommitScp.Initialize(this);
                storeCommitScp.Options.DeepCopyFrom(this.storeOptions);
                storeCommitScp.Options.Identifier = "Storage_and_Storage_Commitment_SCP";
                storeCommitScp.Options.ResultsFileNameOnlyWithoutExtension = this.startDateTime + "_Storage_and_Storage_Commitment_SCP";
                storeCommitScp.Options.StorageMode = Dvtk.Sessions.StorageMode.AsMedia;

                storeCommitScp.setSupportedStoreTS(selectedTSList);
                storeCommitScp.setSupportedCommitTS(selectedTSCommitList);
                storeCommitScp.setSupportedSops(selectedSOPList);
                storeCommitScp.DataDirectory = this.storeOptions.DataDirectory;
                storeCommitScp.RemoteAETitleForCommitSCU = this.commitOptions.RemoteAeTitle;
                storeCommitScp.RemoteHostNameForCommitSCU = this.commitOptions.RemoteHostName;
                storeCommitScp.RemotePortForCommitSCU = this.commitOptions.RemotePort;
                storeCommitScp.Delay = eventDelay;

                storeCommitScp.AddToBack(storeCStoreHandler);

                storeCommitScp.Start();
			}
			else
			{
                StorageScp storeScp = new StorageScp("Storage_SCP_association_", this.startDateTime + "_Storage_SCP_association_");
                storeScp.Initialize(this);
                storeScp.Options.DeepCopyFrom(this.storeOptions);
                storeScp.Options.StorageMode = Dvtk.Sessions.StorageMode.AsMedia;
                storeScp.Options.Identifier = "Storage_SCP";
                storeScp.Options.ResultsFileNameOnlyWithoutExtension = this.startDateTime + "_Storage_SCP";

                storeScp.setSupportedTSForStore(selectedTSList);
                storeScp.setSupportedSops(selectedSOPList);

                StoreCommitScp commitScp = new StoreCommitScp("Storage_Commitment_SCP_association_", this.startDateTime + "_Storage_Commitment_SCP_association_");
                commitScp.StartDateTime = this.startDateTime;
                commitScp.Initialize(this);
                commitScp.Options.DeepCopyFrom(this.commitOptions);
                commitScp.Options.Identifier = "Storage_Commitment_SCP";
                commitScp.Options.ResultsFileNameOnlyWithoutExtension = this.startDateTime + "_Storage_Commitment_SCP";

                commitScp.Options.DataDirectory = this.storeOptions.DataDirectory;
                commitScp.DataDirectory = this.storeOptions.DataDirectory;
                commitScp.Options.StorageMode = Dvtk.Sessions.StorageMode.NoStorage;
                commitScp.Delay = eventDelay;
                commitScp.RemoteAETitleForCommitSCU = this.commitOptions.RemoteAeTitle;
                commitScp.RemoteHostNameForCommitSCU = this.commitOptions.RemoteHostName;
                commitScp.RemotePortForCommitSCU = this.commitOptions.RemotePort;
                commitScp.setSupportedCommitTS(selectedTSCommitList);

                ArrayList sopList = new ArrayList();
                sopList.Add("1.2.840.10008.1.20.1");
                commitScp.setSupportedSops(sopList);

				// Add the message handlers.
                storeScp.AddToBack(storeCStoreHandler);

                storeScp.Start();
                commitScp.Start();			
			}
		}        
	}    
}
