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
using Dvtk.IheActors.Actors;

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for ToActorNode.
	/// </summary>
	public class ToActorNode : ActorNode
	{
		private System.String _ipAddress = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public ToActorNode() : base()
		{
			_ipAddress = "localhost";
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="actorName">Actor Name.</param>
		/// <param name="aeTitle">Actor AE Title.</param>
		/// <param name="ipAddress">Ip Address.</param>
		public ToActorNode(ActorName actorName, System.String aeTitle, System.String ipAddress) : base(actorName, aeTitle)
		{
			_ipAddress = ipAddress;
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
	}
}
