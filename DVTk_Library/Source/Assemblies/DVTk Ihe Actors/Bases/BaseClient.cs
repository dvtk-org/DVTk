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

using DvtkHighLevelInterface.Common.Messages;
using Dvtk.IheActors.Actors;

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for BaseClient.
	/// </summary>
	public abstract class BaseClient
	{
		private BaseActor _parentActor;
		private ActorName _actorName;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="parentActor">Parent Actor Name (containing Actor).</param>
		/// <param name="actorName">Destination Actor Name.</param>
		public BaseClient(BaseActor parentActor, ActorName actorName)
		{
			_parentActor = parentActor;
			_actorName = actorName;
		}

		/// <summary>
		/// Property - ActorName - Destination Actor Name.
		/// </summary>
		public ActorName ActorName
		{
			get
			{
				return _actorName;
			}
		}

		/// <summary>
		/// Property - ParentActor - containing Actor Name.
		/// </summary>
		public BaseActor ParentActor
		{
			get
			{
				return _parentActor;
			}
		}

        /// <summary>
        /// Delegate the Transaction Available Event to a Handler.
        /// </summary>
        public delegate void TransactionAvailableHandler(object server, TransactionAvailableEventArgs transactionAvailable);

        /// <summary>
        /// Transaction Available Event
        /// </summary>
        public event TransactionAvailableHandler OnTransactionAvailable;

        /// <summary>
        /// Publish the Transaction Available Event.
        /// </summary>
        /// <param name="actorName">Name of Actor from which the Transaction was received.</param>
        /// <param name="transaction">Transaction.</param>
        public void PublishTransactionAvailableEvent(ActorName actorName, ActorsTransaction transaction)
        {
            TransactionAvailableEventArgs transactionAvailableEvent = new TransactionAvailableEventArgs(actorName, transaction);

            if (OnTransactionAvailable != null)
            {
                OnTransactionAvailable(this, transactionAvailableEvent);
            }
        }

        /// <summary>
        /// Delegate the Message Available Event to a Handler.
        /// Available messages are either messages about to be sent from this server to the destination server or
        /// messages received by this server from the source server.
        /// </summary>
        public delegate void MessageAvailableHandler(object server, MessageAvailableEventArgs messageAvailable);

        /// <summary>
        /// Message Available Event
        /// </summary>
        public event MessageAvailableHandler OnMessageAvailable;

        /// <summary>
        /// Publish the Message Available Event.
        /// </summary>
        /// <param name="localActorName">Local Actor Name.</param>
        /// <param name="remoteActorName">Remote Actor Name.</param>
        /// <param name="message">Message.</param>
        /// <param name="direction">Message direction.></param>
        public void PublishMessageAvailableEvent(ActorName localActorName, ActorName remoteActorName, Message message, MessageDirectionEnum direction)
        {
            BaseMessage baseMessage = new BaseMessage(message, direction);
            MessageAvailableEventArgs messageAvailableEvent = new MessageAvailableEventArgs(localActorName, remoteActorName, baseMessage);

            if (OnMessageAvailable != null)
            {
                OnMessageAvailable(this, messageAvailableEvent);
            }
        }
	}
}
