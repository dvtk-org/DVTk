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

using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for ActorConnection.
	/// </summary>
	public class ActorConnection
	{
		ActorName _actorName = null;
		bool _active = false;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="actorName">Actor Name.</param>
		/// <param name="active">Active flag - true/false.</param>
		public ActorConnection(ActorName actorName, bool active)
		{
			_actorName = actorName;
			_active = active;
		}

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="actorType">Actor Type.</param>
		/// <param name="id">Actor Id.</param>
		/// <param name="active">Active flag - true/false.</param>
		public ActorConnection(ActorTypeEnum actorType, System.String id, bool active)
		{
			_actorName = new ActorName(actorType, id);
			_active =  active;
		}

		/// <summary>
		/// Property - ActorName.
		/// </summary>
		public ActorName ActorName
		{
			get
			{
				return _actorName;
			}
		}

		/// <summary>
		/// Property - IsActive.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return _active;
			}

			set
			{
				_active = value;
			}
		}
	}
}
