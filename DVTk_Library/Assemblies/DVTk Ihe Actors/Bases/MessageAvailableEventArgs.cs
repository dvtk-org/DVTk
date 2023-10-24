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

namespace Dvtk.IheActors.Bases
{
	/// <summary>
    /// Summary description for MessageAvailableEventArgs.
	/// </summary>
	public class MessageAvailableEventArgs : EventArgs
    {
        private ActorName _localActorName;
        private ActorName _remoteActorName;
        private BaseMessage _message;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="localActorName">Local Actor Name.</param>
        /// <param name="remoteActorName">Remote Actor Name.</param>
        /// <param name="message">Message.</param>
        public MessageAvailableEventArgs(ActorName localActorName, ActorName remoteActorName, BaseMessage message)
		{
            _localActorName = localActorName;
            _remoteActorName = remoteActorName;
            _message = message;
		}

		/// <summary>
		/// Property - LocalActorName.
		/// </summary>
		public ActorName LocalActorName
		{
			get
			{
				return _localActorName;
			}
		}

        /// <summary>
        /// Property - RemoteActorName.
        /// </summary>
        public ActorName RemoteActorName
        {
            get
            {
                return _remoteActorName;
            }
        }

		/// <summary>
        /// Property - Message.
		/// </summary>
        public BaseMessage Message
		{
			get
			{
                return _message;
			}
		}
    }
}
