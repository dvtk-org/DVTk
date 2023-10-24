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
using System.Threading;

using Dvtk.Hl7;
using Dvtk.Hl7.Messages;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;
using Dvtk.IheActors.Hl7.WebService;
using DvtkHighLevelInterface.Hl7.Threads;

namespace Dvtk.IheActors.Hl7
{
	/// <summary>
	/// Summary description for Hl7Client.
	/// </summary>
	public class Hl7Client : BaseClient, IClient
	{
		private bool _looping = true;
		private int _loopDelay = 2000;
		private Hl7PeerToPeerConfig _config = null;
        private Dvtk.IheActors.Bases.Semaphore _semaphore = new Dvtk.IheActors.Bases.Semaphore(200);
		private bool _awaitCompletion = false;
		private System.Collections.Queue _triggerQueue = null;
		private Hl7Mllp _hl7Mllp = null;
		private int _messageControlId = 0;
		private Hl7ThreadForHl7Client _hl7ThreadForHl7Client = null;
		private Hl7ProfileStore _hl7ProfileStore = null;
		private Hl7ValidationContext _hl7ValidationContext = null;
		private NistWebServiceClient _nistWebServiceClient = null;

		/// <summary>
		/// Get the HLI Hl7Thread used within this Hl7Client.
		/// </summary>
		internal Hl7Thread Hl7Thread
		{
			get
			{
				return(this._hl7ThreadForHl7Client);
			}
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - (containing actor).</param>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">HL7 Configuration.</param>
		public Hl7Client(BaseActor parentActor, ActorName actorName, CommonConfig commonConfig, Hl7PeerToPeerConfig config) : base(parentActor, actorName)
		{
			_hl7ThreadForHl7Client = new Hl7ThreadForHl7Client(this);
			DvtkHighLevelInterface.Common.Threads.ThreadManager threadManager = new DvtkHighLevelInterface.Common.Threads.ThreadManager();
			_hl7ThreadForHl7Client.Initialize(threadManager);
			_hl7ThreadForHl7Client.Options.UseResultsFileNameIndex = true;
			_hl7ThreadForHl7Client.Options.SessionId = config.SessionId;
			_hl7ThreadForHl7Client.Options.Identifier = String.Format("From_{0}_To_{1}", 
				ParentActor.ActorName.TypeId,
				ActorName.TypeId);

			_triggerQueue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
			_config = config;


			if (commonConfig.ResultsDirectory != System.String.Empty)
			{
				if (commonConfig.ResultsSubdirectory != System.String.Empty)
				{
					_hl7ThreadForHl7Client.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory + "\\" + commonConfig.ResultsSubdirectory);
				}
				else
				{
					_hl7ThreadForHl7Client.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory);
				}
			}

			// Set up the HL7 Validation Profile Store
			if ((commonConfig.Hl7ProfileDirectory != System.String.Empty) &&
				(commonConfig.Hl7ProfileStoreName != System.String.Empty))
			{
				_hl7ProfileStore = new Hl7ProfileStore(RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.Hl7ProfileDirectory), commonConfig.Hl7ProfileStoreName);
			}

			// Set up the HL7 Validation Context
			if (commonConfig.Hl7ValidationContextFilename != System.String.Empty)
			{
				_hl7ValidationContext = new Hl7ValidationContext(RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.Hl7ValidationContextFilename));
			}

			// Set up the validation Web Service
			if (commonConfig.NistWebServiceUrl != System.String.Empty)
			{
				_nistWebServiceClient = new NistWebServiceClient(commonConfig.NistWebServiceUrl);
			}
		}

		/// <summary>
		/// Start the Client.
		/// </summary>
		public void StartClient()
		{
			_hl7ThreadForHl7Client.Start();
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
			_awaitCompletion = awaitCompletion;

			Hl7Trigger hl7Trigger = (Hl7Trigger) trigger;
			_triggerQueue.Enqueue(hl7Trigger);

			// Check if this is a synchronous call or not
			if (_awaitCompletion == true)
			{
                // Timeout of 0 means "no timeout".
                _semaphore.Wait(0);
			}

            return true;
		}

		/// <summary>
		/// Stop the Client.
		/// </summary>
		public void StopClient()
		{
			_looping = false;

			if (_hl7Mllp != null)
			{
				_hl7Mllp.Stop();
			}

			_hl7ThreadForHl7Client.Stop();
		}

		/// <summary>
		/// Process Hl7 requests and responses.
		/// </summary>
		public void ProcessMessages()
		{
			_looping = true;
			while(_looping == true)
			{
				// Check if anything has been queued
				while (_triggerQueue.Count != 0)
				{
					// Get the trigger
					Hl7Trigger trigger = (Hl7Trigger)_triggerQueue.Dequeue();
					if (trigger != null)
					{
						// Process the trigger
						ProcessTrigger(trigger);

						// Wait awhile before processing new Trigger
						System.Threading.Thread.Sleep(1000);
					}
				}

				System.Threading.Thread.Sleep(_loopDelay);
			}
		}

		private void ProcessTrigger(Hl7Trigger trigger)
		{
			_hl7Mllp = new Hl7Mllp();

			// get the next transaction number - needed to sort the
			// transactions correctly
			int transactionNumber = TransactionNumber.GetNextTransactionNumber();

			System.String message = System.String.Format("HL7 Client thread - connecting to \"{0}\" on port {1}...", _config.ToActorIpAddress, _config.PortNumber);
			_hl7ThreadForHl7Client.LogInformation(message);

			if (_hl7Mllp.Connect(_config.ToActorIpAddress, _config.PortNumber))
			{
				Hl7Message hl7RequestMessage = trigger.Trigger;
				
				// Set the sending and receiving applications
				hl7RequestMessage.SendingApplication = _config.FromActorAeTitle;
				hl7RequestMessage.ReceivingApplication = _config.ToActorAeTitle;

				// Add the control id and date/time of message
				hl7RequestMessage.MessageControlId = _messageControlId.ToString();
				_messageControlId++;
				hl7RequestMessage.DateTimeOfMessage = System.DateTime.Now.ToString("yyyyMMddhhmmss", System.Globalization.CultureInfo.InvariantCulture);

				// get initial HL7 message delimiters from the config
				Hl7MessageDelimiters messageDelimiters = _config.MessageDelimiters;

				System.String messageType = hl7RequestMessage.Value(Hl7SegmentEnum.MSH, 9);
				_hl7ThreadForHl7Client.LogInformation(System.String.Format("HL7 Client thread - send message \"{0}\".", messageType));
				_hl7ThreadForHl7Client.LogInformation(hl7RequestMessage.ToString(messageDelimiters));

				if (_hl7Mllp.SendMessage(hl7RequestMessage, messageDelimiters) == true)
				{
					Hl7Message hl7ResponseMessage = _hl7Mllp.ReceiveMessage(out messageDelimiters);

                    if (hl7ResponseMessage != null)
                    {
                        messageType = hl7ResponseMessage.Value(Hl7SegmentEnum.MSH, 9);
                        _hl7ThreadForHl7Client.LogInformation(System.String.Format("HL7 Client thread - received message \"{0}\".", messageType));
                        _hl7ThreadForHl7Client.LogInformation(hl7ResponseMessage.ToString(messageDelimiters));

                        // Validate the message
                        if (_config.AutoValidate == true)
                        {
                            ValidateMessage(hl7ResponseMessage, messageDelimiters);
                        }

                        // save the transaction
                        Hl7Transaction transaction = new Hl7Transaction(TransactionNameEnum.RAD_1, TransactionDirectionEnum.TransactionSent);
                        transaction.Request = hl7RequestMessage;
                        transaction.Response = hl7ResponseMessage;

                        ActorsTransaction actorsTransaction = new ActorsTransaction(transactionNumber,
                            ActorName, // from actor
                            ParentActor.ActorName,  // to actor
                            transaction,
                            _hl7ThreadForHl7Client.Options.ResultsFileNameOnly,
                            _hl7ThreadForHl7Client.Options.ResultsFullFileName,
                            (uint)_hl7ThreadForHl7Client.NrErrors,
                            (uint)_hl7ThreadForHl7Client.NrWarnings);

                        // save the transaction in the Actor log
                        ParentActor.ActorsTransactionLog.Add(actorsTransaction);

                        // publish the transaction event to any interested parties
                        PublishTransactionAvailableEvent(ActorName, actorsTransaction);
                    }
				}

				_hl7Mllp.Stop();

				_hl7ThreadForHl7Client.StopResultsGathering();
				_hl7ThreadForHl7Client.StartResultsGathering();
			}

			if (_awaitCompletion == true)
			{
				_semaphore.Signal();
			}
		}

		private void ValidateMessage(Hl7Message hl7ResponseMessage, Hl7MessageDelimiters messageDelimiters)
		{
			try
			{
				// validate the HL7 message
				System.String facility = hl7ResponseMessage.SendingFacility;
				System.String version = hl7ResponseMessage.VersionId;
				if (_hl7ProfileStore != null)
				{
					// get the validation profile - keyed off the facility, version and messageType
					System.String messageType = hl7ResponseMessage.MessageType;
					if (hl7ResponseMessage.MessageSubType != System.String.Empty)
					{
						messageType += ("^" + hl7ResponseMessage.MessageSubType);
					}

					System.String xmlProfile = _hl7ProfileStore.GetXmlHl7Profile(new Hl7ProfileId(facility, version, messageType));

					if (_hl7ValidationContext != null)
					{
						// get the validation context
						System.String xmlValidationContext = _hl7ValidationContext.XmlValidationContext;

						if ((xmlProfile != System.String.Empty) &&
							(xmlValidationContext != System.String.Empty) &&
							(_nistWebServiceClient != null))
						{
							System.String errorDescription = System.String.Empty;
							System.String xmlValidationResult = _nistWebServiceClient.Validate(xmlProfile, xmlValidationContext, hl7ResponseMessage.ToString(messageDelimiters), false, out errorDescription);
							if (errorDescription != System.String.Empty)
							{
								_hl7ThreadForHl7Client.LogError(errorDescription);
							}
							NistXmlResultsParser xmlParser = new NistXmlResultsParser(xmlValidationResult);
							_hl7ThreadForHl7Client.UpdateValidationErrorCount(xmlParser.ErrorCount);
							_hl7ThreadForHl7Client.UpdateValidationWarningCount(xmlParser.WarningCount);
							_hl7ThreadForHl7Client.WriteXmlStringToResults(xmlParser.RemoveHeader(xmlValidationResult));
						}
					}
				}
			}
			catch (System.Exception e)
			{
				System.String message = System.String.Format("NIST Validation Exception: {0} - {1}",
					e.Message, e.StackTrace);
				Console.WriteLine(message);
				_hl7ThreadForHl7Client.LogInformation(message);
			}
		}
	}
}
