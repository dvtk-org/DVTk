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
using System.Xml;

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for BasePeerToPeerConfig.
	/// </summary>
	public abstract class BasePeerToPeerConfig
	{
		private ActorNode _fromActor = new ActorNode();
		private ToActorNode _toActor = new ToActorNode();
		private System.UInt16 _portNumber = 0;
		private bool _secureConnection = false;
        private bool _autoValidate = true;
		private System.String _actorOption1 = System.String.Empty;
		private System.String _actorOption2 = System.String.Empty;
		private System.String _actorOption3 = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public BasePeerToPeerConfig()
		{
			//
			// Constructor activities
			//
			_portNumber = 104;
			_secureConnection = false;
		}


		/// <summary>
		/// Property - PortNumber.
		/// </summary>
		public System.UInt16 PortNumber
		{
			get 
			{ 
				return _portNumber; 
			}
			set 
			{ 
				_portNumber = value; 
			}
		}

		/// <summary>
		/// Property - SecureConnection.
		/// </summary>
		public bool SecureConnection
		{
			get 
			{ 
				return _secureConnection; 
			}
			set 
			{ 
				_secureConnection = value; 
			}
		}

        /// <summary>
        /// Property - AutoValidate.
        /// </summary>
        public bool AutoValidate
        {
            get
            {
                return _autoValidate;
            }
            set
            {
                _autoValidate = value;
            }
        }

		/// <summary>
		/// Property - ToActorName.
		/// </summary>
		public ActorName ToActorName
		{
			get
			{
				return _toActor.ActorName;
			}
			set
			{
				_toActor.ActorName = value;
			}
		}

		/// <summary>
		/// Property - ToActorAeTitle.
		/// </summary>
		public System.String ToActorAeTitle
		{
			get
			{
				return _toActor.AeTitle;
			}
			set
			{
				_toActor.AeTitle = value;
			}
		}

		/// <summary>
		/// Property - ToActorIpAddress.
		/// </summary>
		public System.String ToActorIpAddress
		{
			get
			{
				return _toActor.IpAddress;
			}
			set
			{
				_toActor.IpAddress = value;
			}
		}

		/// <summary>
		/// Property - FromActorName.
		/// </summary>
		public ActorName FromActorName
		{
			get
			{
				return _fromActor.ActorName;
			}
			set
			{
				_fromActor.ActorName = value;
			}
		}

		/// <summary>
		/// Property - FromActorAeTitle.
		/// </summary>
		public System.String FromActorAeTitle
		{
			get
			{
				return _fromActor.AeTitle;
			}
			set
			{
				_fromActor.AeTitle = value;
			}
		}

		/// <summary>
		/// Property - ActorOption1.
		/// </summary>
		public System.String ActorOption1
		{
			get
			{
				return _actorOption1;
			}
			set
			{
				_actorOption1 = value;
			}
		}

		/// <summary>
		/// Property - ActorOption2.
		/// </summary>
		public System.String ActorOption2
		{
			get
			{
				return _actorOption2;
			}
			set
			{
				_actorOption2 = value;
			}
		}

		/// <summary>
		/// Property - ActorOption3.
		/// </summary>
		public System.String ActorOption3
		{
			get
			{
				return _actorOption3;
			}
			set
			{
				_actorOption3 = value;
			}
		}

		/// <summary>
		/// Write the Configuration in XML format to the XML Text Writer.
		/// </summary>
		/// <param name="writer">XML Text Writer</param>
		public abstract void WriteXmlConfig(XmlTextWriter writer);
	}
}
