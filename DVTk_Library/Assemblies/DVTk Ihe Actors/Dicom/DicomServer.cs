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
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomServer.
	/// </summary>
	public abstract class DicomServer : BaseServer, IServer
	{
		private HliScp _scp = null;
        private DicomTransaction _currentDicomTransaction = null;

        private String _configOption1 = String.Empty;
        private String _configOption2 = String.Empty;
        private String _configOption3 = String.Empty;
        
        /// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - (containing actor).</param>
		/// <param name="actorName">Destination Actor Name.</param>
		public DicomServer(BaseActor parentActor, ActorName actorName) : base(parentActor, actorName) {}

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
		/// Apply the Dicom Configuration to the Server.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">Dicom Configuration.</param>
		public virtual void ApplyConfig(CommonConfig commonConfig, DicomPeerToPeerConfig config)
		{
			if (_scp != null)
			{
				_scp.Initialize(ParentActor.ThreadManager);
				_scp.Options.SessionId = config.SessionId;
				_scp.Options.Identifier = String.Format("To_{0}_From_{1}", 
					ParentActor.ActorName.TypeId,
					ActorName.TypeId);

				if (commonConfig.ResultsDirectory != System.String.Empty)
				{
					_scp.Options.StartAndStopResultsGatheringEnabled = true;
					_scp.ResultsFilePerAssociation = true;

					if (commonConfig.ResultsSubdirectory != System.String.Empty)
					{
						_scp.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory + "\\" + commonConfig.ResultsSubdirectory);
					}
					else
					{
						_scp.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory);
					}
				}
				else
				{
					_scp.Options.StartAndStopResultsGatheringEnabled = false;
				}
				
				_scp.Options.CredentialsFilename = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.CredentialsFilename);
				_scp.Options.CertificateFilename = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.CertificateFilename);
				_scp.Options.SecureConnection = config.SecureConnection;

				_scp.Options.LocalAeTitle = config.ToActorAeTitle;
				_scp.Options.LocalPort = config.PortNumber;

				_scp.Options.RemoteAeTitle = config.FromActorAeTitle;
				_scp.Options.RemotePort = config.PortNumber;
				_scp.Options.RemoteHostName = config.ToActorIpAddress;

                _scp.Options.AutoValidate = config.AutoValidate;

                _scp.Options.DataDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, config.StoreDataDirectory);
				if (config.StoreData == true)
				{
					_scp.Options.StorageMode = Dvtk.Sessions.StorageMode.AsMediaOnly;
				}
				else
				{
					_scp.Options.StorageMode = Dvtk.Sessions.StorageMode.NoStorage;
				}

				foreach (System.String filename in config.DefinitionFiles)
				{
					_scp.Options.LoadDefinitionFile(RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, filename));
				}

                // finally copy any config options
                _configOption1 = config.ActorOption1;
                _configOption2 = config.ActorOption2;
                _configOption3 = config.ActorOption3;
            }
		}

		/// <summary>
		/// Update the Dicom Configuration of the Server.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="clientConfig">Dicom Client Configuration.</param>
		public void UpdateConfig(CommonConfig commonConfig, DicomPeerToPeerConfig clientConfig)
		{
			_scp.Options.RemotePort = clientConfig.PortNumber;
			_scp.Options.RemoteHostName = clientConfig.ToActorIpAddress;

			foreach (System.String filename in clientConfig.DefinitionFiles)
			{
				_scp.Options.LoadDefinitionFile(RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, filename));
			}
		}

        /// <summary>
        /// Get the directory where the DicomServer stores the DCM files.
        /// </summary>
        public String StoreDataDirectory
        {
            get
            {
                return _scp.Options.DataDirectory;
            }
        }

		/// <summary>
		/// Property - Scp
		/// </summary>
		internal HliScp Scp
		{
			set
			{
				_scp = value;
			}
			get
			{
				return _scp;
			}
		}

		/// <summary>
		/// Start the Server.
		/// </summary>
		public void StartServer()
		{		
			SubscribeEvent();

			int delay = 0;
			if (_scp != null)
			{
				_scp.Start(delay);
			}
		}

		/// <summary>
		/// Stop the Server.
		/// </summary>
		public void StopServer()
		{
			UnSubscribeEvent();

			if (_scp != null)
			{
				_scp.Stop();		
			}
		}

        /// <summary>
        /// Subscribe to the message sending and received events.
        /// </summary>
        private void SubscribeEvent()
		{
			if (_scp != null)
			{
                _scp.SendingMessageEvent += new HliScp.SendingMessageEventHandler(ScpSendingMessageEventHandler);
                _scp.MessageReceivedEvent += new HliScp.MessageReceivedEventHandler(ScpMessageReceivedEventHandler);
			}
		}

        /// <summary>
        /// Un-subscribe to the message sending and received events.
        /// </summary>
        private void UnSubscribeEvent()
		{
			if (_scp != null)
			{
                _scp.SendingMessageEvent -= new HliScp.SendingMessageEventHandler(ScpSendingMessageEventHandler);
                _scp.MessageReceivedEvent -= new HliScp.MessageReceivedEventHandler(ScpMessageReceivedEventHandler);
			}
		}

        /// <summary>
        /// Handle the Mesage Sending Event for all messages.
        /// </summary>
        /// <param name="dicomProtocolMessage">Received DICOM Protocol Message.</param>
        public virtual void ScpSendingMessageEventHandler(DicomProtocolMessage dicomProtocolMessage)
		{
            // Inform any interested parties that a message is being sent
            PublishMessageAvailableEvent(ParentActor.ActorName, ActorName, dicomProtocolMessage, MessageDirectionEnum.MessageSent);

            if (dicomProtocolMessage is AssociateRj)
			{
                // this has rejected the association
                LogTransaction();
            }
			else if (dicomProtocolMessage is Abort)
			{
                // this has aborted the association
                LogTransaction();
            }
			else if (dicomProtocolMessage is ReleaseRp)
			{
                // this has released the association
                LogTransaction();
            }
            else if (dicomProtocolMessage is DicomMessage)
            {
                // add the DICOM message to the transaction
                if (_currentDicomTransaction != null)
                {
                    _currentDicomTransaction.DicomMessages.Add((DicomMessage)dicomProtocolMessage);
                }
            }
		}

        /// <summary>
        /// Handle the Mesage Received Event for all messages.
        /// </summary>
        /// <param name="dicomProtocolMessage">Received DICOM Protocol Message.</param>
        public virtual void ScpMessageReceivedEventHandler(DicomProtocolMessage dicomProtocolMessage)
		{
            // Inform any interested parties that a message has been received
            PublishMessageAvailableEvent(ParentActor.ActorName, ActorName, dicomProtocolMessage, MessageDirectionEnum.MessageReceived);

            if (dicomProtocolMessage is AssociateRq)
			{
                // on receiving an associate request set up a new transaction store
                _currentDicomTransaction = new DicomTransaction(TransactionNameEnum.RAD_10, TransactionDirectionEnum.TransactionReceived);
			}
			else if (dicomProtocolMessage is Abort)
			{
                // peer has aborted the association
                LogTransaction();
			}
            else if (dicomProtocolMessage is DicomMessage)
            {
                // add the DICOM message to the transaction
                if (_currentDicomTransaction != null)
                {
                    _currentDicomTransaction.DicomMessages.Add((DicomMessage)dicomProtocolMessage);
                }
            }
		}

        private void LogTransaction()
        {
            DicomThread dicomThread = (DicomThread)DvtkHighLevelInterface.Common.Threads.Thread.CurrentThread;
            if (dicomThread == null) return;

            if (_currentDicomTransaction == null) return;

			// get the next transaction number - needed to sort the
			// transactions correctly
			int transactionNumber = TransactionNumber.GetNextTransactionNumber();

			// save the transaction
			ActorsTransaction actorsTransaction = new ActorsTransaction(transactionNumber,
				ActorName,  // from actor
				ParentActor.ActorName,  // to actor
                _currentDicomTransaction, 
				dicomThread.Options.ResultsFileNameOnly,
                dicomThread.Options.ResultsFullFileName,
                (uint)dicomThread.NrOfErrors,
				(uint)dicomThread.NrOfWarnings);

            // save the transaction in the Actor log
			ParentActor.ActorsTransactionLog.Add(actorsTransaction);

            // publish the transaction event to any interested parties
            PublishTransactionAvailableEvent(ActorName, actorsTransaction);

            // remove any messages from the dicom thread
            dicomThread.ClearMessages();

            _currentDicomTransaction = null;
        }

		#region workflow settings
		/// <summary>
		/// Clear the current transfer syntax list - reset contents to empty.
		/// </summary>
		public void ClearTransferSyntaxes()
		{
			if (_scp != null)
			{
				_scp.ClearTransferSyntaxes();
			}
		}

		/// <summary>
		/// Add a single transfer syntax to the list.
		/// </summary>
		/// <param name="transferSyntax">Transfer Syntax UID.</param>
		public void AddTransferSyntax(System.String transferSyntax)
		{
			if (_scp != null)
			{
				_scp.AddTransferSyntax(transferSyntax);
			}
		}

		/// <summary>
		/// SetRespondToAssociateRequest - define how the SCP should react to the Associate Request.
		/// </summary>
		/// <param name="scpRespondToAssociateRequest">scpRespondToAssociateRequest enum.</param>
		public void SetRespondToAssociateRequest(HliScp.ScpRespondToAssociateRequestEnum scpRespondToAssociateRequest)
		{
			if (_scp != null)
			{
				_scp.RespondToAssociateRequest = scpRespondToAssociateRequest;
			}
		}

		/// <summary>
		/// SetRespondToReleaseRequest - define how the SCP should react to the Release Request.
		/// </summary>
		/// <param name="scpRespondToReleaseRequest">scpRespondToReleaseRequest enum.</param>
		public void SetRespondToReleaseRequest(HliScp.ScpRespondToReleaseRequestEnum scpRespondToReleaseRequest)
		{
			if (_scp != null)
			{
				_scp.RespondToReleaseRequest = scpRespondToReleaseRequest;
			}
		}

		/// <summary>
		///  Set Associate Reject parameters.
		/// </summary>
		/// <param name="result">DULP reject result.</param>
		/// <param name="source">DULP reject source.</param>
		/// <param name="reason">DULP reject reason.</param>
		public void SetRejectParameters(Byte result, Byte source, Byte reason)
		{
			if (_scp != null)
			{
				_scp.SetRejectParameters(result, source, reason);
			}
		}

		/// <summary>
		/// Set the Abort Request parameters.
		/// </summary>
		/// <param name="source">DULP abort source.</param>
		/// <param name="reason">DULP abort reason.</param>
		public void SetAbortParameters(Byte source, Byte reason)
		{
			if (_scp != null)
			{
				_scp.SetAbortParameters(source, reason);
			}
		}

		/// <summary>
		/// Set the ResultsFilePerAssociation parameter.
		/// </summary>
		/// <param name="resultsFilePerAssociation">Boolean indicating whether the results files are generated per association or not.</param>
		public void SetResultsFilePerAssociation(bool resultsFilePerAssociation)
		{
			if (_scp != null)
			{
				_scp.ResultsFilePerAssociation = resultsFilePerAssociation;
			}
		}
		#endregion workflow settings
	}
}
