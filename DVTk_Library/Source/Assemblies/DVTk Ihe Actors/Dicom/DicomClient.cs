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
using System.Collections;

using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Dicom.Other;
using Dvtk.DvtkDicomEmulators.Bases;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomClient.
	/// </summary>
	public abstract class DicomClient : BaseClient, IClient
	{
		protected PresentationContext[] _presentationContexts = null;
		private TransferSyntaxes _transferSyntaxes = null;

        private Semaphore _semaphore = new Semaphore(200);
		private DicomScu _scu = null;

        private String _configOption1 = String.Empty;
        private String _configOption2 = String.Empty;
        private String _configOption3 = String.Empty;

		/// <summary>
		/// Property - Scu
		/// </summary>
		internal HliScu Scu
		{
			get
			{
				return _scu;
			}
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - (containing actor).</param>
		/// <param name="actorName">Destination Actor Name.</param>
        /// <param name="storageCommitmentScu">Boolean indicating if this is a Storage Commitment SCU or not.</param>
		public DicomClient(BaseActor parentActor, ActorName actorName, bool storageCommitmentScu) : base(parentActor, actorName) 
		{
            if (storageCommitmentScu == true)
            {
                _scu = new DicomStorageCommitmentScu(this);
            }
            else
            {
                _scu = new DicomScu(this);
            }

			// set up the default transfer syntaxes proposed
			_transferSyntaxes = new TransferSyntaxes(HliScp.IMPLICIT_VR_LITTLE_ENDIAN, 
				HliScp.EXPLICIT_VR_LITTLE_ENDIAN, 
				HliScp.EXPLICIT_VR_BIG_ENDIAN);
		}

        /// <summary>
        /// Property ConfigOption1
        /// </summary>
        public String ConfigOption1
        {
            get
            {
                return _configOption1;
            }
        }

        /// <summary>
        /// Property ConfigOption2
        /// </summary>
        public String ConfigOption2
        {
            get
            {
                return _configOption2;
            }
        }

        /// <summary>
        /// Property ConfigOption3
        /// </summary>
        public String ConfigOption3
        {
            get
            {
                return _configOption3;
            }
        }

		/// <summary>
		/// Signal the semaphore.
		/// </summary>
		public void Signal()
		{
			_semaphore.Signal();
		}

		/// <summary>
		/// Apply the Dicom Configuration to the Client,
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">Dicom Configuration.</param>
		public void ApplyConfig(CommonConfig commonConfig, DicomPeerToPeerConfig config)
		{
			_scu.Initialize(ParentActor.ThreadManager);
			_scu.Options.Identifier = String.Format("From_{0}_To_{1}", 
				ParentActor.ActorName.TypeId,
				ActorName.TypeId);

			if (commonConfig.ResultsDirectory != System.String.Empty)
			{
				_scu.Options.StartAndStopResultsGatheringEnabled = true;
				_scu.ResultsFilePerTrigger = true;

				if (commonConfig.ResultsSubdirectory != System.String.Empty)
				{
					_scu.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory + "\\" + commonConfig.ResultsSubdirectory);
				}
				else
				{
					_scu.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory);
				}
			}
			else
			{
				_scu.Options.StartAndStopResultsGatheringEnabled = false;
			}

			_scu.Options.CredentialsFilename = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.CredentialsFilename);
			_scu.Options.CertificateFilename = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.CertificateFilename);
			_scu.Options.SecureConnection = config.SecureConnection;

			_scu.Options.LocalAeTitle = config.FromActorAeTitle;
			_scu.Options.LocalPort = config.PortNumber;

			_scu.Options.RemoteAeTitle = config.ToActorAeTitle;
			_scu.Options.RemotePort = config.PortNumber;
			_scu.Options.RemoteHostName = config.ToActorIpAddress;

            _scu.Options.AutoValidate = config.AutoValidate;

			_scu.Options.DataDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, config.StoreDataDirectory);
			if (config.StoreData == true)
			{
				_scu.Options.StorageMode = Dvtk.Sessions.StorageMode.AsMediaOnly;
			}
			else
			{
				_scu.Options.StorageMode = Dvtk.Sessions.StorageMode.NoStorage;
			}

			foreach (System.String filename in config.DefinitionFiles)
			{
				_scu.Options.LoadDefinitionFile(RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, filename));
			}

            // finally copy any config options
            _configOption1 = config.ActorOption1;
            _configOption2 = config.ActorOption2;
            _configOption3 = config.ActorOption3;

			_scu.LoopDelay = 3000;
		}

		/// <summary>
		/// Start the Client.
		/// </summary>
		public void StartClient()
		{
			_scu.SubscribeEvent();
			_scu.Start();
		}

		/// <summary>
		/// Trigger the Client.
		/// </summary>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <param name="trigger">Trigger message.</param>
		/// <param name="awaitCompletion">Boolean indicating whether this a synchronous call or not.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool TriggerClient(ActorName actorName, BaseTrigger trigger, bool awaitCompletion)
		{
			DicomTrigger dicomTrigger = (DicomTrigger) trigger;

			_scu.SignalCompletion = awaitCompletion;

			// determine how the handle the trigger - in a single association or not
			if (dicomTrigger.HandleInSingleAssociation == true)
			{
				// set the dicom messages to send
				DicomMessageCollection dicomMessageCollection = new DicomMessageCollection();
				foreach (DicomTriggerItem dicomTriggerItem in dicomTrigger.TriggerItems)
				{
					dicomMessageCollection.Add(dicomTriggerItem.Message);
				}

				// set the presentation contexts for the association
				PresentationContext[] presentationContexts = SetPresentationContexts(dicomTrigger);

				// send in single association
				_scu.TriggerSendAssociation(dicomMessageCollection, presentationContexts);

				// Check if this is a synchronous call or not
                // - timeout of 0 means "no timeout".
				if (awaitCompletion == true)
				{
					_semaphore.Wait(0);
				}
			}
			else
			{
				// send the triggers in separate associations
				foreach (DicomTriggerItem dicomTriggerItem in dicomTrigger.TriggerItems)
				{
					// check if the sop class uid and transfer syntax are being explicitly defined
					if ((dicomTriggerItem.SopClassUid != System.String.Empty) &&
						(dicomTriggerItem.TransferSyntaxes.Length != 0))
					{
						// use the given presentation context
						PresentationContext[] presentationContexts = new PresentationContext[1];
						presentationContexts[0] = new PresentationContext(dicomTriggerItem.SopClassUid,
							MergeTransferSyntaxes(dicomTriggerItem.TransferSyntaxes));
						_scu.TriggerSendAssociation(dicomTriggerItem.Message, presentationContexts);
					}
					else
					{
						// use the initial presentation context
						_scu.TriggerSendAssociation(dicomTriggerItem.Message, _presentationContexts);
					}

					// Check if this is a synchronous call or not
                    // - timeout of 0 means "no timeout".
                    if (awaitCompletion == true)
					{
						_semaphore.Wait(0);
					}
				}
			}

            // return a boolean indicating if the trigger was processed successfully or not
            return _scu.ProcessTriggerResult;
		}

		/// <summary>
		/// Trigger the Client to send a Verification (DICOM C-ECHO-RQ).
		/// </summary>
		/// <param name="actorName">Destination Actor Name.</param>
        /// <returns>Boolean indicating success or failure.</returns>
		public bool TriggerClientVerification(ActorName actorName)
		{
			// Set up a Verification SOP Class - C-ECHO-RQ trigger
			DicomTrigger dicomTrigger = new DicomTrigger(Dvtk.IheActors.Bases.TransactionNameEnum.RAD_UNKNOWN);
			dicomTrigger.AddItem(new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORQ),
								HliScp.VERIFICATION_SOP_CLASS_UID,
								HliScp.IMPLICIT_VR_LITTLE_ENDIAN);
			DicomTriggerItem dicomTriggerItem = dicomTrigger.TriggerItems[0];

			// thread must wait for association completion
			_scu.SignalCompletion = true;

			// set up the presentation context from the trigger
			PresentationContext[] presentationContexts = new PresentationContext[1];
			presentationContexts[0] = new PresentationContext(dicomTriggerItem.SopClassUid,
					dicomTriggerItem.TransferSyntaxes);
			_scu.TriggerSendAssociation(dicomTriggerItem.Message, presentationContexts);

			// wait for association completion
            // - timeout of 0 means "no timeout".
            _semaphore.Wait(0);

            // return a boolean indicating if the trigger was processed successfully or not
            return _scu.ProcessTriggerResult;
		}

		/// <summary>
		/// Stop the Client.
		/// </summary>
		public void StopClient()
		{
			_scu.UnSubscribeEvent();
			_scu.Stop();
		}

		#region workflow settings
		/// <summary>
		/// Clear the current transfer syntax list - reset contents to empty.
		/// </summary>
		public void ClearTransferSyntaxes()
		{
			_transferSyntaxes = null;
		}

		/// <summary>
		/// Add a single transfer syntax to the list.
		/// </summary>
		/// <param name="transferSyntax">Transfer Syntax UID.</param>
		public void AddTransferSyntax(System.String transferSyntax)
		{
			// first check if the class has been instantiated
			if (_transferSyntaxes == null)
			{
				_transferSyntaxes = new TransferSyntaxes();
			}

			// Add the transfer syntax
			_transferSyntaxes.Add(transferSyntax);
		}
		#endregion workflow settings

		private PresentationContext[] SetPresentationContexts(DicomTrigger dicomTrigger)
		{
			DicomTriggerItemCollection localTriggerItems = new DicomTriggerItemCollection();

			// use the local trigger items to establish a list of presentation contexts that
			// only appear once
			foreach (DicomTriggerItem triggerItem in dicomTrigger.TriggerItems)
			{
				if (localTriggerItems.FindMatchingPresentationContext(triggerItem) == false)
				{
					localTriggerItems.Add(triggerItem);
				}
			}

			// now set up the returned presentation contexts from the local trigger collection
			PresentationContext[] presentationContexts = new PresentationContext[localTriggerItems.Count];
			int index = 0;
			foreach (DicomTriggerItem triggerItem in localTriggerItems)
			{
				// save the presentation context
				presentationContexts[index++] = new PresentationContext(triggerItem.SopClassUid,
					MergeTransferSyntaxes(triggerItem.TransferSyntaxes));
			}

			return presentationContexts;
		}

		private System.String[] MergeTransferSyntaxes(System.String[] inTransferSyntaxes)
		{
			// save all the input transfer syntaxes
			TransferSyntaxes localTransferSyntaxes = new TransferSyntaxes();
			for (int i = 0; i < inTransferSyntaxes.Length; i++)
			{
				localTransferSyntaxes.Add(inTransferSyntaxes[i]);
			}

			// add any from the proposed client transfer syntaxes that are not in the input list
			if(_transferSyntaxes != null)
			{
				for (int i = 0; i < _transferSyntaxes.Length(); i++)
				{
					bool found = false;
					for (int j = 0; j < inTransferSyntaxes.Length; j++)
					{
						if (_transferSyntaxes.Get(i) == inTransferSyntaxes[j])
						{
							found = true;
							break;
						}
					}

					if (found == false)
					{
						localTransferSyntaxes.Add(_transferSyntaxes.Get(i));
					}
				}
			}

			// copy them to the merged list
			System.String[] mergedTransferSyntaxes = new System.String[localTransferSyntaxes.Length()];
			for (int i = 0; i < localTransferSyntaxes.Length(); i++)
			{
				mergedTransferSyntaxes[i] = localTransferSyntaxes.Get(i);
			}

			return mergedTransferSyntaxes;
		}
	}
}
