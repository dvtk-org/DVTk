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

using Dvtk.CommonDataFormat;
using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Messages;
using Dvtk.IheActors.Actors;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;
using Dvtk.Dicom.InformationEntity.DefaultValues;
using Dvtk.Comparator;

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for BaseActor.
	/// </summary>
	public abstract class BaseActor
	{
		#region actor states
		private enum ActorStateEnum
		{
			ActorCreated,	// created - but not configured
			ActorStarted,	// started - but configured
			ActorStopped,	// stopped - but configured
		}
		#endregion

		private ActorName _actorName;
        private Dvtk.IheActors.IheFramework.IheFramework _iheFramework = null;
		private ActorStateEnum _actorState;
		private ActorConnectionCollection _actorConnectionCollection = new ActorConnectionCollection();
		private System.Collections.Hashtable _hl7Servers = null;
		private System.Collections.Hashtable _hl7Clients = null;
		private System.Collections.Hashtable _dicomServers = null;
		private System.Collections.Hashtable _dicomClients = null;
		private ThreadManager _threadManager = null;
		private ArrayList _attachedUserInterfaces = new ArrayList();
		private DefaultValueManager _defaultValueManager = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="actorName">Actor Name.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public BaseActor(ActorName actorName, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
		{
			_actorName = actorName;
            _iheFramework = iheFramework;
			InitActor();
		}

		/// <summary>
		/// Property - ActorName
		/// </summary>
		public ActorName ActorName
		{
			get
			{
				return _actorName;
			}
		}

		/// <summary>
        /// Property - IheFramework
		/// </summary>
        public Dvtk.IheActors.IheFramework.IheFramework IheFramework
		{
			get
			{
                return _iheFramework;
			}
		}

		/// <summary>
		/// Property - ActorConnectionCollection
		/// </summary>
		public ActorConnectionCollection ActorConnectionCollection
		{
			get
			{
				return _actorConnectionCollection;
			}
		}

		/// <summary>
		/// Property - ActorsTransactionLog
		/// </summary>
		public ActorsTransactionLog ActorsTransactionLog
		{
			get
			{
                return _iheFramework.TransactionLog;
			}
		}

		/// <summary>
		/// Property - DvtThreadManager
		/// </summary>
		public ThreadManager ThreadManager
		{
			get
			{
				return _threadManager;
			}
		}

		/// <summary>
		/// Property - DefaultValueManager
		/// </summary>
		public DefaultValueManager DefaultValueManager
		{
			get
			{
				return _defaultValueManager;
			}
		}

		/// <summary>
		/// Initialize the Actor
		/// </summary>
		private void InitActor()
		{
			_hl7Servers = new Hashtable();
			_hl7Clients = new Hashtable();
			_dicomServers = new Hashtable();
			_dicomClients = new Hashtable();
			_actorState = ActorStateEnum.ActorCreated;
			_threadManager = new ThreadManager();
		}

		internal void Attach(Dvtk.IheActors.UserInterfaces.IActorUserInterface actorUserInterface)
		{
			this._attachedUserInterfaces.Add(actorUserInterface);
		}

		/// <summary>
		/// This object has zero or more attached User Interfaces.
		/// Attach the supplied thread to all the User Interfaces that also implement the IThreadUserInterface.
		/// </summary>
        /// <param name="thread">HLI Thread</param>
		private void AttachThreadToThreadUserInterfaces(DvtkHighLevelInterface.Common.Threads.Thread thread)
		{
			foreach (Dvtk.IheActors.UserInterfaces.IActorUserInterface actorUserInterface in this._attachedUserInterfaces)
			{
				if (actorUserInterface is DvtkHighLevelInterface.Common.UserInterfaces.IThreadUserInterface)
				{
					DvtkHighLevelInterface.Common.UserInterfaces.IThreadUserInterface threadUserInterface = actorUserInterface as DvtkHighLevelInterface.Common.UserInterfaces.IThreadUserInterface;

					threadUserInterface.Attach(thread);
				}
			}
		}

		/// <summary>
		/// Configure the Actor
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		public void ConfigActor(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			switch (_actorState)
			{
				case ActorStateEnum.ActorCreated:
				case ActorStateEnum.ActorStopped:
					// initialize the actor - this allows an actor to be re-configured after being used
					InitActor();

					// call sub-class method to apply the actor specific config
					ApplyConfig(commonConfig, peerToPeerConfigCollection);

					// set state to stopped
					_actorState = ActorStateEnum.ActorStopped;
					break;
				default:
					// actor is started - so cannot be (re)configured
					break;
			}
		}

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected abstract void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection);

		/// <summary>
		/// Start the Actor.
		/// </summary>
		public void StartActor(DefaultValueManager defaultValueManager)
		{
			switch (_actorState)
			{
				case ActorStateEnum.ActorStopped:
				{
					ICollection hl7Servers = _hl7Servers.Values;
					foreach (Hl7Server hl7Server in hl7Servers)
					{
						// Each HL7 Server uses a HLI Thread internally.
						// If this actor is attached to UserInterfaces that implement the
						// IThreadUserInterface, attach the HLI thread to them.
						AttachThreadToThreadUserInterfaces(hl7Server.Hl7Thread);

						hl7Server.StartServer();
					}

					ICollection hl7Clients = _hl7Clients.Values;
					foreach (Hl7Client hl7Client in hl7Clients)
					{
						// Each HL7 Client uses a HLI Thread internally.
						// If this actor is attached to UserInterfaces that implement the
						// IThreadUserInterface, attach the HLI thread to them.
						AttachThreadToThreadUserInterfaces(hl7Client.Hl7Thread);
						
						hl7Client.StartClient();
					}

					ICollection dicomServers = _dicomServers.Values;
					foreach (DicomServer dicomServer in dicomServers)
					{
						// Each Dicom Server uses a HLI Thread internally.
						// If this actor is attached to UserInterfaces that implement the
						// IThreadUserInterface, attach the HLI thread to them.
						AttachThreadToThreadUserInterfaces(dicomServer.Scp);

						dicomServer.StartServer();
					}

					ICollection dicomClients = _dicomClients.Values;
					foreach (DicomClient dicomClient in dicomClients)
					{
						// Each Dicom Client uses a HLI Thread internally.
						// If this actor is attached to UserInterfaces that implement the
						// IThreadUserInterface, attach the HLI thread to them.
						AttachThreadToThreadUserInterfaces(dicomClient.Scu);

						dicomClient.StartClient();
					}

					_actorState = ActorStateEnum.ActorStarted;

					// Save the default value manager
					_defaultValueManager = defaultValueManager;
					break;
				}
				default:
					// actor either not configured or already started
					break;
			}
		}

		/// <summary>
		/// Stop the Actor.
		/// </summary>
		public void StopActor()
		{
			switch (_actorState)
			{
				case ActorStateEnum.ActorStarted:
				{
					ICollection hl7Servers = _hl7Servers.Values;
					foreach (Hl7Server hl7Server in hl7Servers)
					{
						hl7Server.StopServer();
					}

					ICollection hl7Clients = _hl7Clients.Values;
					foreach (Hl7Client hl7Client in hl7Clients)
					{
						hl7Client.StopClient();
					}

					ICollection dicomServers = _dicomServers.Values;
					foreach (DicomServer dicomServer in dicomServers)
					{
						dicomServer.StopServer();
					}

					ICollection dicomClients = _dicomClients.Values;
					foreach (DicomClient dicomClient in dicomClients)
					{
						dicomClient.StopClient();
					}

					_threadManager.WaitForCompletionThreads();
					_actorState = ActorStateEnum.ActorStopped;
					_defaultValueManager = null;
					break;
				}
				default:
					// actor either not configured or already stopped
					break;
			}
		}

		/// <summary>
		/// Trigger the Actor Instances of the given Actor Type.
		/// </summary>
		/// <param name="actorType">Destination Actor Type.</param>
		/// <param name="trigger">Trigger message.</param>
		/// <param name="awaitCompletion">Boolean indicating whether this a synchronous call or not.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool TriggerActorInstances(ActorTypeEnum actorType, BaseTrigger trigger, bool awaitCompletion)
		{
            bool triggerResult = false;

			// can only trigger an actor that is started
			if (_actorState == ActorStateEnum.ActorStarted)
			{
                triggerResult = true;

				ActorNameCollection activeDestinationActors = ActorConnectionCollection.IsEnabled(actorType);
				foreach(ActorName activeActorName in activeDestinationActors)
				{
					if (trigger is Hl7Trigger)
					{
						Hl7Client hl7Client = GetHl7Client(activeActorName);
						if (hl7Client != null)
						{
                            if (hl7Client.TriggerClient(activeActorName, trigger, awaitCompletion) == false)
                            {
                                // set returned result - but continue with other triggers
                                triggerResult = false;
                            }
						}
					}
					else
					{
						DicomClient dicomClient = GetDicomClient(activeActorName);
						if (dicomClient != null)
						{
                            if (dicomClient.TriggerClient(activeActorName, trigger, awaitCompletion) == false)
                            {
                                // set returned result - but continue with other triggers
                                triggerResult = false;
                            }
						}
					}
				}
			}

            return triggerResult;
        }

		/// <summary>
		/// Trigger the DICOM Client Verification (E-ECHO-RQ).
		/// </summary>
		/// <param name="actorType">Destination Actor Type.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool TriggerActorDicomClientVerificationInstances(ActorTypeEnum actorType)
		{
            bool triggerResult = false;

			// can only trigger an actor that is started
			if (_actorState == ActorStateEnum.ActorStarted)
			{
                triggerResult = true;

                ActorNameCollection activeDestinationActors = ActorConnectionCollection.IsEnabled(actorType);
				foreach(ActorName activeActorName in activeDestinationActors)
				{
					DicomClient dicomClient = GetDicomClient(activeActorName);
					if (dicomClient != null)
					{
                        if (dicomClient.TriggerClientVerification(activeActorName) == false)
                        {
                            // set returned result - but continue with other triggers
                            triggerResult = false;
                        }
					}
				}
			}

            return triggerResult;
		}

		/// <summary>
		/// Add a reponse trigger to the Actor.
		/// </summary>
		/// <param name="actorType">Destination Actor Type.</param>
		/// <param name="trigger">Trigger message.</param>
		public void AddResponseTriggerToActor(ActorTypeEnum actorType, BaseTrigger trigger)
		{
			// can only load and actor that is started
			if (_actorState == ActorStateEnum.ActorStarted)
			{
				ActorNameCollection activeDestinationActors = ActorConnectionCollection.IsEnabled(actorType);
				foreach(ActorName activeActorName in activeDestinationActors)
				{
					if (trigger is Hl7Trigger)
					{
						Hl7Server hl7Server = GetHl7Server(activeActorName);
						if ((hl7Server != null) &&
							(hl7Server is Hl7QueryServer))
						{
							Hl7QueryServer hl7QueryServer = (Hl7QueryServer)hl7Server;
							hl7QueryServer.AddResponseTrigger(activeActorName, trigger);
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the HL7Server that corresponds with the Destination Actor Name.
		/// </summary>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <returns>Hl7Server.</returns>
		public Hl7Server GetHl7Server(ActorName actorName)
		{
			return (Hl7Server)_hl7Servers[actorName.TypeId];
		}

		/// <summary>
		/// Add an HL7Server for the given Destination Actor Name and Configuration.
		/// </summary>
		/// <param name="hl7ServerType">Hl7 Server Type.</param>
		/// <param name="fromActorType">From Actor Type.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected void AddHl7Server(Hl7ServerTypeEnum hl7ServerType, ActorTypeEnum fromActorType, CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			foreach (BasePeerToPeerConfig basePeerToPeerConfig in peerToPeerConfigCollection)
			{
				if ((basePeerToPeerConfig is Hl7PeerToPeerConfig) &&
					(basePeerToPeerConfig.ToActorName.TypeId == _actorName.TypeId) &&
					(basePeerToPeerConfig.FromActorName.Type == fromActorType))
				{
					Hl7Server hl7Server = ClientServerFactory.CreateHl7Server(hl7ServerType, this, basePeerToPeerConfig.FromActorName, commonConfig, (Hl7PeerToPeerConfig)basePeerToPeerConfig);
					if (hl7Server != null)
					{
						SubscribeEvent(hl7Server);
						_hl7Servers.Add(hl7Server.ActorName.TypeId, hl7Server);

						// Initialize the connection with the from actor as being active.
						// - this can always be overruled by the application later.
						SetActorDefaultConnectionActive(basePeerToPeerConfig.FromActorName);
					}
				}
			}
		}

		/// <summary>
		/// Get the HL7Client that corresponds with the Destination Actor Name.
		/// </summary>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <returns>Hl7Vlient.</returns>
		public Hl7Client GetHl7Client(ActorName actorName)
		{
			return (Hl7Client)_hl7Clients[actorName.TypeId];
		}

		/// <summary>
		/// Add an HL7Client for the given Destination Actor Name and Configuration.
		/// </summary>
		/// <param name="hl7ClientType">Hl7 Client Type.</param>
		/// <param name="toActorType">To Actor Type.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected void AddHl7Client(Hl7ClientTypeEnum hl7ClientType, ActorTypeEnum toActorType, CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			foreach (BasePeerToPeerConfig basePeerToPeerConfig in peerToPeerConfigCollection)
			{
				if ((basePeerToPeerConfig is Hl7PeerToPeerConfig) &&
					(basePeerToPeerConfig.FromActorName.TypeId == _actorName.TypeId) &&
					(basePeerToPeerConfig.ToActorName.Type == toActorType))
				{
					Hl7Client hl7Client = ClientServerFactory.CreateHl7Client(hl7ClientType, this, basePeerToPeerConfig.ToActorName, commonConfig, (Hl7PeerToPeerConfig)basePeerToPeerConfig);
					if (hl7Client != null)
					{
                        SubscribeEvent(hl7Client);
						_hl7Clients.Add(hl7Client.ActorName.TypeId, hl7Client);

						// Initialize the connection with the to actor as being active.
						// - this can always be overruled by the application later.
						SetActorDefaultConnectionActive(basePeerToPeerConfig.ToActorName);
					}
				}
			}
		}

        /// <summary>
        /// Get the Actor Id of the first instance of the given peer actor type DicomServer
        /// in this actor.
        /// </summary>
        /// <param name="actorType">Peer actor type.</param>
        /// <returns>String - Actor Id.</returns>
        public String GetFirstActorIdFromDicomServer(ActorTypeEnum actorType)
        {
            foreach (DicomServer dicomServer in _dicomServers.Values)
            {
                if (dicomServer.ActorName.Type == actorType)
                {
                    return dicomServer.ActorName.Id;
                }
            }
            return String.Empty;
        }

		/// <summary>
		/// Get the DicomServer that corresponds with the Destination Actor Name.
		/// </summary>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <returns>DicomServer.</returns>
		public DicomServer GetDicomServer(ActorName actorName)
		{
			return (DicomServer)_dicomServers[actorName.TypeId];
		}

		/// <summary>
		/// Add the given DicomServer using the Destination Actor Configuration.
		/// </summary>
		/// <param name="dicomServerType">Dicom Server Type.</param>
		/// <param name="fromActorType">From Actor Type.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected void AddDicomServer(DicomServerTypeEnum dicomServerType, ActorTypeEnum fromActorType, CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			foreach (BasePeerToPeerConfig basePeerToPeerConfig in peerToPeerConfigCollection)
			{
				if ((basePeerToPeerConfig is DicomPeerToPeerConfig) &&
					(basePeerToPeerConfig.ToActorName.TypeId == _actorName.TypeId) &&
					(basePeerToPeerConfig.FromActorName.Type == fromActorType))
				{
					DicomServer dicomServer = ClientServerFactory.CreateDicomServer(dicomServerType, this, basePeerToPeerConfig.FromActorName);
					if (dicomServer != null)
					{
						dicomServer.ApplyConfig(commonConfig, (DicomPeerToPeerConfig)basePeerToPeerConfig);
						SubscribeEvent(dicomServer);
						_dicomServers.Add(dicomServer.ActorName.TypeId, dicomServer);

						// Initialize the connection with the from actor as being active.
						// - this can always be overruled by the application later.
						SetActorDefaultConnectionActive(basePeerToPeerConfig.FromActorName);
					}
				}
			}
		}

		/// <summary>
		/// Update the given DicomServer using the Destination Actor Configuration.
		/// </summary>
		/// <param name="serverActorName">DicomServer Actor Name.</param>
		/// <param name="clientActorType">DicomClient Actor Type.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
        protected void UpdateDicomServer(ActorName serverActorName, ActorTypeEnum clientActorType, CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			foreach (BasePeerToPeerConfig basePeerToPeerConfig in peerToPeerConfigCollection)
			{
				if ((basePeerToPeerConfig is DicomPeerToPeerConfig) &&
					(basePeerToPeerConfig.FromActorName.TypeId == serverActorName.TypeId) &&
					(basePeerToPeerConfig.ToActorName.Type == clientActorType))
				{
                    DicomServer dicomServer = GetDicomServer(basePeerToPeerConfig.ToActorName);
					if (dicomServer != null)
					{
						dicomServer.UpdateConfig(commonConfig, (DicomPeerToPeerConfig)basePeerToPeerConfig);
					}
				}
			}
		}

		/// <summary>
		/// Get the DicomClient that corresponds with the Destination Actor Name.
		/// </summary>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <returns>DicomClient.</returns>
		public DicomClient GetDicomClient(ActorName actorName)
		{
			return (DicomClient)_dicomClients[actorName.TypeId];
		}

		/// <summary>
		/// Add the given DicomClient using the Destination Actor Configuration.
		/// </summary>
		/// <param name="dicomClientType">Dicom Client Type.</param>
		/// <param name="toActorType">To Actor Type.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected void AddDicomClient(DicomClientTypeEnum dicomClientType, ActorTypeEnum toActorType, CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			foreach (BasePeerToPeerConfig basePeerToPeerConfig in peerToPeerConfigCollection)
			{
				if ((basePeerToPeerConfig is DicomPeerToPeerConfig) &&
					(basePeerToPeerConfig.FromActorName.TypeId == _actorName.TypeId) &&
					(basePeerToPeerConfig.ToActorName.Type == toActorType))
				{
					DicomClient dicomClient = ClientServerFactory.CreateDicomClient(dicomClientType, this, basePeerToPeerConfig.ToActorName);
					if (dicomClient != null)
					{
						dicomClient.ApplyConfig(commonConfig, (DicomPeerToPeerConfig)basePeerToPeerConfig);
                        SubscribeEvent(dicomClient);
						_dicomClients.Add(dicomClient.ActorName.TypeId, dicomClient);

						// Initialize the connection with the to actor as being active.
						// - this can always be overruled by the application later.
						SetActorDefaultConnectionActive(basePeerToPeerConfig.ToActorName);
					}
				}
			}
		}

		private void SetActorDefaultConnectionActive(ActorName actorName)
		{
			// check if the actor has already been enabled by default
			if (ActorConnectionCollection.IsEnabled(actorName) == false)
			{
				// enable the connection by default
				ActorConnectionCollection.Add(new ActorConnection(actorName, true));
			}
		}

        private void SubscribeEvent(DicomClient dicomClient)
        {
            dicomClient.OnTransactionAvailable += new DicomClient.TransactionAvailableHandler(TransactionIsAvailable);
            dicomClient.OnMessageAvailable += new DicomClient.MessageAvailableHandler(MessageIsAvailable);
        }

		private void SubscribeEvent(DicomServer dicomServer)
		{
			dicomServer.OnTransactionAvailable += new DicomServer.TransactionAvailableHandler(TransactionIsAvailable);
            dicomServer.OnMessageAvailable += new DicomServer.MessageAvailableHandler(MessageIsAvailable);
		}

        private void SubscribeEvent(Hl7Client hl7Client)
        {
            hl7Client.OnTransactionAvailable += new Hl7Client.TransactionAvailableHandler(TransactionIsAvailable);
            hl7Client.OnMessageAvailable += new Hl7Client.MessageAvailableHandler(MessageIsAvailable);
        }

		private void SubscribeEvent(Hl7Server hl7Server)
		{
			hl7Server.OnTransactionAvailable += new Hl7Server.TransactionAvailableHandler(TransactionIsAvailable);
            hl7Server.OnMessageAvailable += new Hl7Server.MessageAvailableHandler(MessageIsAvailable);
        }

		/// <summary>
		/// Transaction is available.
		/// </summary>
		/// <param name="server">Event source.</param>
		/// <param name="transactionAvailableEvent">Transaction Available Event Details.</param>
		private void TransactionIsAvailable(object server, TransactionAvailableEventArgs transactionAvailableEvent)
		{
			// handle the new transaction
			if (transactionAvailableEvent.Transaction.Transaction is Hl7Transaction)
			{
                // handle the HL7 transaction in any derived Actor class
                HandleTransactionFrom(transactionAvailableEvent.ActorName, (Hl7Transaction)transactionAvailableEvent.Transaction.Transaction);
			}
			else
			{
                // handle the DICOM transaction in any derived Actor class
                HandleTransactionFrom(transactionAvailableEvent.ActorName, (DicomTransaction)transactionAvailableEvent.Transaction.Transaction);
			}

            // Publish the event to any interested parties.
            PublishTransactionAvailableEvent(transactionAvailableEvent);
		}

        /// <summary>
        /// Delegate the Transaction Available Event to a Handler.
        /// </summary>
        public delegate void TransactionAvailableHandler(object server, TransactionAvailableEventArgs transactionAvailableEvent);

        /// <summary>
        /// Transaction Available Event
        /// </summary>
        public event TransactionAvailableHandler OnTransactionAvailable;

        /// <summary>
        /// Publish the Transaction Available Event.
        /// </summary>
        /// <param name="transactionAvailableEvent">Available transaction.</param>
        private void PublishTransactionAvailableEvent(TransactionAvailableEventArgs transactionAvailableEvent)
        {
//            DisplayTransactionDetails(transactionAvailableEvent);

            if (OnTransactionAvailable != null)
            {
                OnTransactionAvailable(this, transactionAvailableEvent);
            }
        }
        
        /// <summary>
		/// Handle an HL7 Transation from the given Actor Name.
		/// </summary>
		/// <param name="actorName">Source Actor Name.</param>
		/// <param name="hl7Transaction">HL7 Transaction.</param>
		protected virtual void HandleTransactionFrom(ActorName actorName, Hl7Transaction hl7Transaction) {}

		/// <summary>
		/// Handle a Dicom Transaction from the given Actor Name.
		/// </summary>
		/// <param name="actorName">Source Actor Name.</param>
		/// <param name="dicomTransaction">Dicom Transaction.</param>
		protected virtual void HandleTransactionFrom(ActorName actorName, DicomTransaction dicomTransaction) {}

        private void DisplayTransactionDetails(TransactionAvailableEventArgs transactionAvailableEvent)
        {
             System.Console.WriteLine("Transaction {0} Actor: {1} - ResultsPathname: \"{2}\" - Errors: {3} - Warnings: {4}",
                                transactionAvailableEvent.Transaction.TransactionNumber,
                                transactionAvailableEvent.ActorName.TypeId, 
                                transactionAvailableEvent.Transaction.ResultsPathname, 
                                transactionAvailableEvent.Transaction.NrErrors, 
                                transactionAvailableEvent.Transaction.NrWarnings);
        }

        /// <summary>
        /// Message is available.
        /// </summary>
        /// <param name="server">Event source.</param>
        /// <param name="messageAvailableEvent">Message Available Event Details.</param>
        private void MessageIsAvailable(object server, MessageAvailableEventArgs messageAvailableEvent)
        {
            // Publish the event to any interested parties.
            PublishMessageAvailableEvent(messageAvailableEvent);
        }

        /// <summary>
        /// Delegate the Message Available Event to a Handler.
        /// Available messages are either messages about to be sent from this server to the destination server or
        /// messages received by this server from the source server.
        /// </summary>
        public delegate void MessageAvailableHandler(object server, MessageAvailableEventArgs messageAvailableEvent);

        /// <summary>
        /// Message Available Event
        /// </summary>
        public event MessageAvailableHandler OnMessageAvailable;

        /// <summary>
        /// Publish the Message Available Event.
        /// </summary>
        /// <param name="messageAvailableEvent">Available message.</param>
        private void PublishMessageAvailableEvent(MessageAvailableEventArgs messageAvailableEvent)
        {
//            DisplayMessageDetails(messageAvailableEvent);

            if (OnMessageAvailable != null)
            {
                OnMessageAvailable(this, messageAvailableEvent);
            }
        }

        private void DisplayMessageDetails(MessageAvailableEventArgs messageAvailableEvent)
        {
            String messageType = String.Empty;
            ActorName localActorName = messageAvailableEvent.LocalActorName;
            ActorName remoteActorName = messageAvailableEvent.RemoteActorName;
            if (messageAvailableEvent.Message.Message is DicomProtocolMessage)
            {
                DicomProtocolMessage dicomProtocolMessage = (DicomProtocolMessage)messageAvailableEvent.Message.Message;

                if (dicomProtocolMessage is AssociateRq)
                {
                    messageType = "AssociateRq";
                }
                else if (dicomProtocolMessage is AssociateAc)
                {
                    messageType = "AssociateAc";
                }
                else if (dicomProtocolMessage is AssociateRj)
                {
                    messageType = "AssociateRj";
                }
                else if (dicomProtocolMessage is Abort)
                {
                    messageType = "AbortRq";
                }
                else if (dicomProtocolMessage is ReleaseRq)
                {
                    messageType = "ReleaseRq";
                }
                else if (dicomProtocolMessage is ReleaseRp)
                {
                    messageType = "ReleaseRp";
                }
                else if (dicomProtocolMessage is DicomMessage)
                {
                    messageType = "DICOM Message";
                }

                if (messageAvailableEvent.Message.Direction == MessageDirectionEnum.MessageReceived)
                {
                    System.Console.WriteLine("Local: {0} - Received Message: {1} - From Remote: {2}",
                                            localActorName.TypeId,
                                            messageType,
                                            remoteActorName.TypeId);
                }
                else
                {
                    System.Console.WriteLine("Local: {0} - Sending Message: {1} - To Remote: {2} ",
                                            localActorName.TypeId,
                                            messageType,
                                            remoteActorName.TypeId);
                }
            }
        }

		protected DvtkData.Dimse.DataSet Hl7ToDicom(Hl7Message message)
		{
			DvtkData.Dimse.DataSet dataset = new DvtkData.Dimse.DataSet("Transient");

			if (message != null)
			{
				// try to get the patient id
				CommonIdFormat patientId = new CommonIdFormat();
				patientId.FromHl7Format(message.Value(new Hl7Tag("PID", 3)));
				if (patientId.ToDicomFormat() != System.String.Empty)
				{
				   dataset.AddAttribute(DvtkData.Dimse.Tag.PATIENT_ID.GroupNumber, DvtkData.Dimse.Tag.PATIENT_ID.ElementNumber, DvtkData.Dimse.VR.LO, patientId.ToDicomFormat());
				}

				// try to get the patient's name
				CommonNameFormat patientName = new CommonNameFormat();
				patientName.FromHl7Format(message.Value(new Hl7Tag("PID", 5)));
				if (patientName.ToDicomFormat() != System.String.Empty)
				{
					dataset.AddAttribute(DvtkData.Dimse.Tag.PATIENTS_NAME.GroupNumber, DvtkData.Dimse.Tag.PATIENTS_NAME.ElementNumber, DvtkData.Dimse.VR.PN, patientName.ToDicomFormat());
				}

				// try to get the patient's birth date
				CommonDateFormat patientBirthDate = new CommonDateFormat();
				patientBirthDate.FromHl7Format(message.Value(new Hl7Tag("PID", 7)));
				if (patientBirthDate.ToDicomFormat() != System.String.Empty)
				{
					dataset.AddAttribute(DvtkData.Dimse.Tag.PATIENTS_BIRTH_DATE.GroupNumber, DvtkData.Dimse.Tag.PATIENTS_BIRTH_DATE.ElementNumber, DvtkData.Dimse.VR.DA, patientBirthDate.ToDicomFormat());
				}

				// try to get the patient's sex
				CommonStringFormat patientSex = new CommonStringFormat();
				patientSex.FromHl7Format(message.Value(new Hl7Tag("PID", 8)));
				if (patientSex.ToDicomFormat() != System.String.Empty)
				{
					dataset.AddAttribute(DvtkData.Dimse.Tag.PATIENTS_SEX.GroupNumber, DvtkData.Dimse.Tag.PATIENTS_SEX.ElementNumber, DvtkData.Dimse.VR.CS, patientSex.ToDicomFormat());
				}

				// try to get the merge patient id
				CommonIdFormat mergePatientId = new CommonIdFormat();
				mergePatientId.FromHl7Format(message.Value(new Hl7Tag("MRG", 1)));
				if (mergePatientId.ToDicomFormat() != System.String.Empty)
				{
					dataset.AddAttribute(DvtkData.Dimse.Tag.OTHER_PATIENT_IDS.GroupNumber, DvtkData.Dimse.Tag.OTHER_PATIENT_IDS.ElementNumber, DvtkData.Dimse.VR.LO, mergePatientId.ToDicomFormat());
				}
			}

			return dataset;
		}

		protected Hl7Message DicomtoHl7(DvtkData.Dimse.DataSet dataset)
		{
			Hl7Message message = null;

			return message;
		}
	}  
}
