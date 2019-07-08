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
	/// Summary description for Hl7Server.
	/// </summary>
	public class Hl7Server : BaseServer, IServer
	{
//		private int _resultsFileIndex = 0;
		private Hl7PeerToPeerConfig _config = null;
		private Hl7Mllp _hl7Mllp = null;
		private Hl7ThreadForHl7Server _hl7ThreadForHl7Server = null;
		private Hl7ProfileStore _hl7ProfileStore = null;
		private Hl7ValidationContext _hl7ValidationContext = null;
		private NistWebServiceClient _nistWebServiceClient = null;

		/// <summary>
		/// Get the HLI Hl7Thread used within this Hl7Server.
		/// </summary>
		internal Hl7Thread Hl7Thread
		{
			get
			{
				return(this._hl7ThreadForHl7Server);
			}
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name - (containing actor).</param>
		/// <param name="actorName">Destination Actor Name.</param>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="config">HL7 Configuration.</param>
		public Hl7Server(BaseActor parentActor, ActorName actorName, CommonConfig commonConfig, Hl7PeerToPeerConfig config) : base(parentActor, actorName)
		{

			_hl7ThreadForHl7Server = new Hl7ThreadForHl7Server(this);
			DvtkHighLevelInterface.Common.Threads.ThreadManager threadManager = new DvtkHighLevelInterface.Common.Threads.ThreadManager();
			_hl7ThreadForHl7Server.Initialize(threadManager);
			_hl7ThreadForHl7Server.Options.UseResultsFileNameIndex = true;
			_hl7ThreadForHl7Server.Options.SessionId = config.SessionId;
			_hl7ThreadForHl7Server.Options.Identifier = String.Format("To_{0}_From_{1}", 
				ParentActor.ActorName.TypeId,
				ActorName.TypeId);

			_config = config;

			if (commonConfig.ResultsDirectory != System.String.Empty)
			{
				if (commonConfig.ResultsSubdirectory != System.String.Empty)
				{
					_hl7ThreadForHl7Server.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory + "\\" + commonConfig.ResultsSubdirectory);
				}
				else
				{
					_hl7ThreadForHl7Server.Options.ResultsDirectory = RootedBaseDirectory.GetFullPathname(commonConfig.RootedBaseDirectory, commonConfig.ResultsDirectory);
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
		/// Start Server
		/// </summary>
		public void StartServer()
		{
			_hl7ThreadForHl7Server.Start();
		}

		/// <summary>
		/// Stop Server
		/// </summary>
		public void StopServer()
		{
			if (_hl7Mllp != null)
			{
				_hl7Mllp.Stop();
			}
			_hl7ThreadForHl7Server.Stop();
		}

		/// <summary>
		/// Process Hl7 requests and responses.
		/// </summary>
		public void ProcessMessages()
		{
			_hl7Mllp = new Hl7Mllp();
			
			for(;;)
			{
				System.String message = System.String.Format("Listening for incoming HL7 connection on port {0}...", _config.PortNumber);
				_hl7ThreadForHl7Server.LogInformation(message);

				try
				{
					// listen for a connection
					_hl7Mllp.Listen(_config.PortNumber);

					for(;;)
					{
						// accept the connection - only one at a time
						_hl7Mllp.Accept();

						// the message delimiters will be filled in by the ReceiveMessage method
						Hl7MessageDelimiters messageDelimiters = null;

						// receive an HL7 message
						Hl7Message hl7RequestMessage = _hl7Mllp.ReceiveMessage(out messageDelimiters);
						if (hl7RequestMessage != null)
						{
							System.String messageType = hl7RequestMessage.Value(Hl7SegmentEnum.MSH, 9);
							message = System.String.Format("HL7 Server thread - received message \"{0}\".", messageType);
							_hl7ThreadForHl7Server.LogInformation(message);
							_hl7ThreadForHl7Server.LogInformation(hl7RequestMessage.ToString(messageDelimiters));

							// Validate the message
                            if (_config.AutoValidate == true)
                            {
                                ValidateMessage(hl7RequestMessage, messageDelimiters);
                            }

							// call the message handler to get a response
							Hl7Message hl7ResponseMessage = MessageHandler(hl7RequestMessage);

							// copy some fields from the request
							hl7ResponseMessage.SendingApplication = hl7RequestMessage.ReceivingApplication;
							hl7ResponseMessage.SendingFacility = hl7RequestMessage.ReceivingFacility;
							hl7ResponseMessage.ReceivingApplication = hl7RequestMessage.SendingApplication;
							hl7ResponseMessage.ReceivingFacility = hl7RequestMessage.SendingFacility;
							hl7ResponseMessage.MessageControlId = hl7RequestMessage.MessageControlId;

							// set the date/time of message
							hl7ResponseMessage.DateTimeOfMessage = System.DateTime.Now.ToString("yyyyMMddhhmmss", System.Globalization.CultureInfo.InvariantCulture);

							// send the response
							_hl7Mllp.SendMessage(hl7ResponseMessage, messageDelimiters);

							messageType = hl7ResponseMessage.Value(Hl7SegmentEnum.MSH, 9);
							message = System.String.Format("HL7 Server thread - sent message \"{0}\".", messageType);
							_hl7ThreadForHl7Server.LogInformation(message);
							_hl7ThreadForHl7Server.LogInformation(hl7ResponseMessage.ToString(messageDelimiters));

							// get the next transaction number - needed to sort the
							// transactions correctly
							int transactionNumber = TransactionNumber.GetNextTransactionNumber();

							// save the transaction
							Hl7Transaction transaction = new Hl7Transaction(TransactionNameEnum.RAD_1, TransactionDirectionEnum.TransactionReceived);
							transaction.Request = hl7RequestMessage;
							transaction.Response = hl7ResponseMessage;

							_hl7ThreadForHl7Server.LogInformation(String.Format("{0} - received Hl7 event from {1}", ParentActor.ActorName, ActorName));

							ActorsTransaction actorsTransaction = new ActorsTransaction(transactionNumber,
								ActorName,  // from actor
								ParentActor.ActorName,  // to actor
								transaction, 
								_hl7ThreadForHl7Server.Options.ResultsFileNameOnly,
                                _hl7ThreadForHl7Server.Options.ResultsFullFileName,
                                (uint)_hl7ThreadForHl7Server.NrErrors,
								(uint)_hl7ThreadForHl7Server.NrWarnings);

                            // save the transaction in the Actor log
                            ParentActor.ActorsTransactionLog.Add(actorsTransaction);

                            // publish the transaction event to any interested parties
                            PublishTransactionAvailableEvent(ActorName, actorsTransaction);
                        }

						_hl7Mllp.Close();

						_hl7ThreadForHl7Server.StopResultsGathering();
						_hl7ThreadForHl7Server.StartResultsGathering();
					}
				}
				catch
				{
					// System.Net.Sockets.Socket.Accept() exception thrown when the server is stopped by closing the listen socket.
					break;
				}		
			}
		}

		private void ValidateMessage(Hl7Message hl7RequestMessage, Hl7MessageDelimiters messageDelimiters)
		{
			try
			{
				// validate the HL7 message
				System.String facility = hl7RequestMessage.SendingFacility;
				System.String version = hl7RequestMessage.VersionId;
				if (_hl7ProfileStore != null)
				{
					// get the validation profile - keyed off the facility, version and messageType
					System.String messageType = hl7RequestMessage.MessageType;
					if (hl7RequestMessage.MessageSubType != System.String.Empty)
					{
						messageType += ("^" + hl7RequestMessage.MessageSubType);
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
							System.String xmlValidationResult = _nistWebServiceClient.Validate(xmlProfile, xmlValidationContext, hl7RequestMessage.ToString(messageDelimiters), false, out errorDescription);
							if (errorDescription != System.String.Empty)
							{
								_hl7ThreadForHl7Server.LogError(errorDescription);
							}
							
							NistXmlResultsParser xmlParser = new NistXmlResultsParser(xmlValidationResult);
							_hl7ThreadForHl7Server.UpdateValidationErrorCount(xmlParser.ErrorCount);
							_hl7ThreadForHl7Server.UpdateValidationWarningCount(xmlParser.WarningCount);
							_hl7ThreadForHl7Server.WriteXmlStringToResults(xmlParser.RemoveHeader(xmlValidationResult));
						}
					}
				}
			}
			catch (System.Exception e)
			{
				System.String message = System.String.Format("NIST Validation Exception: {0} - {1}",
					e.Message, e.StackTrace);
				Console.WriteLine(message);
				_hl7ThreadForHl7Server.LogInformation(message);
			}
		}

		/// <summary>
		/// Default message handler for the received Hl7 message - just send an ACK.
		/// </summary>
		/// <param name="hl7RequestMessage">Received HL7 message.</param>
		/// <returns>Hl7Message response.</returns>
		public virtual Hl7Message MessageHandler(Hl7Message hl7RequestMessage)
		{
			// Set up an ACK message to return
			AckMessage hl7ResponseMessage = new AckMessage(hl7RequestMessage.MessageSubType);

			// fill in the MSA segment
			hl7ResponseMessage.MSA[1] = "AA";
			hl7ResponseMessage.MSA[2] = hl7RequestMessage.MessageControlId;

			return hl7ResponseMessage;
		}
	}
}
