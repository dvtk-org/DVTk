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
	/// Summary description for CommunicationNode.
	/// </summary>
	public class CommunicationNode
	{
		private System.String _ipAddress;
		private System.UInt16 _portNumber;
		private System.String _aeTitle;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public CommunicationNode()
		{
			//
			// Constructor activities
			//
			_ipAddress = "localhost";
			_portNumber = 104;
			_aeTitle = "AE_TITLE";
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="ipAddress">IP Address.</param>
		/// <param name="portNumber">Port Number.</param>
		/// <param name="aeTitle">AE Title.</param>
		public CommunicationNode(System.String ipAddress, System.UInt16 portNumber, System.String aeTitle)
		{
			//
			// Constructor activities
			//
			_ipAddress = ipAddress;
			_portNumber = portNumber;
			_aeTitle = aeTitle;
		}

		/// <summary>
		/// Property - IpAddress.
		/// </summary>
		public System.String IpAddress
		{
			get 
			{ 
				return _ipAddress; 
			}
			set 
			{ 
				_ipAddress = value; 
			}
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
		/// Property - AeTitle.
		/// </summary>
		public System.String AeTitle
		{
			get 
			{ 
				return _aeTitle; 
			}
			set 
			{ 
				_aeTitle = value; 
			}
		}
	}
}
