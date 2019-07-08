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
	/// Summary description for ActorNode.
	/// </summary>
	public class ActorNode
	{
		private ActorName _actorName;
		private System.String _aeTitle;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public ActorNode()
		{
			_actorName = null;
			_aeTitle = System.String.Empty;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="actorName">Actor Name.</param>
		/// <param name="aeTitle">Actor AE Title.</param>
		public ActorNode(ActorName actorName, System.String aeTitle)
		{
			_actorName = actorName;
			_aeTitle = aeTitle;
		}

		/// <summary>
		/// Property - Actor Name.
		/// </summary>
		public ActorName ActorName
		{
			get
			{
				return _actorName;
			}
			set
			{
				_actorName = value;
			}
		}

		/// <summary>
		/// Property - Actor AE Title.
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
