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
    /// Summary description of the DicomScu class.
    /// </summary>
    public class DicomScu : HliScu
    {
        private DicomClient _dicomClient = null;
        private DicomTransaction _currentDicomTransaction = null;
        private int _transactionNumber = 0;
        private bool _signalCompletion = false;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="dicomClient"></param>
        public DicomScu(DicomClient dicomClient)
        {
            _dicomClient = dicomClient;
        }

        /// <summary>
        /// Property - DicomClient
        /// </summary>
        protected DicomClient DicomClient
        {
            get
            {
                return _dicomClient;
            }
        }

        /// <summary>
        /// Property - Signal SCU completion.
        /// </summary>
        public bool SignalCompletion
        {
            set
            {
                _signalCompletion = value;
            }
        }

        /// <summary>
        /// Do this before processing the Client Trigger.
        /// </summary>
        /// <param name="trigger">Trigger message.</param>
        public override void BeforeProcessTrigger(Object trigger)
        {
            // call the base implementation
            base.BeforeProcessTrigger(trigger);

            // get the next transaction number - needed to sort the
            // transactions correctly
            _transactionNumber = TransactionNumber.GetNextTransactionNumber();
        }

        /// <summary>
        /// Do this after processing the Client Trigger.
        /// </summary>
        /// <param name="trigger">Trigger message.</param>
        public override void AfterProcessTrigger(Object trigger)
        {
            // call the base implementation
            base.AfterProcessTrigger(trigger);

            // Check if we need to signal the client that we have finished.
            if (_signalCompletion == true)
            {
                _dicomClient.Signal();
            }
        }

        /// <summary>
        /// Subscribe to the message sending and received events.
        /// </summary>
        public void SubscribeEvent()
        {
            this.SendingMessageEvent += new DicomScu.SendingMessageEventHandler(ScuSendingMessageEventHandler);
            this.MessageReceivedEvent += new DicomScu.MessageReceivedEventHandler(ScuMessageReceivedEventHandler);
        }

        /// <summary>
        /// Un-subscribe to the message sending and received events.
        /// </summary>
        public void UnSubscribeEvent()
        {
            this.SendingMessageEvent -= new DicomScu.SendingMessageEventHandler(ScuSendingMessageEventHandler);
            this.MessageReceivedEvent -= new DicomScu.MessageReceivedEventHandler(ScuMessageReceivedEventHandler);
        }

        /// <summary>
        /// Handle the Mesage Sending Event for all messages.
        /// </summary>
        /// <param name="dicomProtocolMessage">Received DICOM Protocol Message.</param>
        public virtual void ScuSendingMessageEventHandler(DicomProtocolMessage dicomProtocolMessage)
        {
            // Inform any interested parties that a message is being sent
            _dicomClient.PublishMessageAvailableEvent(_dicomClient.ParentActor.ActorName, _dicomClient.ActorName, dicomProtocolMessage, MessageDirectionEnum.MessageSent);

            if (dicomProtocolMessage is AssociateRq)
            {
                // on sending an associate request set up a new transaction store
                _currentDicomTransaction = new DicomTransaction(TransactionNameEnum.RAD_10, TransactionDirectionEnum.TransactionSent);

            }
            else if (dicomProtocolMessage is Abort)
            {
                // this has aborted the association
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
        public virtual void ScuMessageReceivedEventHandler(DicomProtocolMessage dicomProtocolMessage)
        {
            // Inform any interested parties that a message has been received
            _dicomClient.PublishMessageAvailableEvent(_dicomClient.ParentActor.ActorName, _dicomClient.ActorName, dicomProtocolMessage, MessageDirectionEnum.MessageReceived);

            if (dicomProtocolMessage is AssociateRj)
            {
                // peer has rejected the association
                LogTransaction();
            }
            else if (dicomProtocolMessage is Abort)
            {
                // peer has aborted the association
                LogTransaction();
            }
            else if (dicomProtocolMessage is ReleaseRp)
            {
                // peer has released the association
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

            // save the transaction
            ActorsTransaction actorsTransaction = new ActorsTransaction(_transactionNumber,
                _dicomClient.ActorName, // from actor
                _dicomClient.ParentActor.ActorName,  // to actor
                _currentDicomTransaction,
                this.Options.ResultsFileNameOnly,
                this.Options.ResultsFullFileName,
                (uint)dicomThread.NrOfErrors,
                (uint)dicomThread.NrOfWarnings);

            // save the transaction in the Actor log
            _dicomClient.ParentActor.ActorsTransactionLog.Add(actorsTransaction);

            // publish the transaction event to any interested parties
            _dicomClient.PublishTransactionAvailableEvent(_dicomClient.ActorName, actorsTransaction);

            // remove any messages from the dicom thread
            dicomThread.ClearMessages();

            _currentDicomTransaction = null;
        }
    }
}
