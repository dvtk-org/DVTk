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

using DvtkHighLevelInterface.Common.Messages;

namespace Dvtk.IheActors.Bases
{
    #region transaction directions
    public enum MessageDirectionEnum
    {
        MessageSent,
        MessageReceived
    }
    #endregion

    /// <summary>
    /// Summary description for BaseMessage Class.
    /// </summary>
    public class BaseMessage
    {
        private Message _message;
        private MessageDirectionEnum _direction;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="message">Base message.</param>
        /// <param name="direction">Message direction.</param>
        public BaseMessage(Message message, MessageDirectionEnum direction)
        {
            _message = message;
            _direction = direction;
        }

        /// <summary>
        /// Property Message - base message.
        /// </summary>
        public Message Message
        {
            get
            {
                return _message;
            }
        }

        /// <summary>
        /// Property - Direction.
        /// </summary>
        public MessageDirectionEnum Direction
        {
            get
            {
                return _direction;
            }
        }
    }
}
