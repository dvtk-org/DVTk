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
using System.Xml;
using Dvtk.IheActors.Bases;

namespace Dvtk.IheActors.Actors
{
	#region actor states

	public enum ActorConfigStateEnum
	{
		ActorDisabled,
		ActorBeingEmulated,
		ActorIsSut
	}

	public class ActorConfigState
	{
		public static System.String ConfigState(ActorConfigStateEnum actorConfigState)
		{
			System.String configState = "ActorDisabled";

			switch(actorConfigState)
			{
				case ActorConfigStateEnum.ActorDisabled: configState = "ActorDisabled"; break;
				case ActorConfigStateEnum.ActorBeingEmulated: configState = "ActorBeingEmulated"; break;
				case ActorConfigStateEnum.ActorIsSut: configState = "ActorIsSut"; break;
				default:
					break;
			}

			return configState;
		}

		public static ActorConfigStateEnum ConfigStateEnum(System.String configState)
		{
			ActorConfigStateEnum configStateEnum = ActorConfigStateEnum.ActorDisabled;

			if (configState == "ActorDisabled")
			{
				configStateEnum = ActorConfigStateEnum.ActorDisabled;
			}
			else if (configState == "ActorBeingEmulated")
			{
				configStateEnum = ActorConfigStateEnum.ActorBeingEmulated;
			}
			else if (configState == "ActorIsSut")
			{
				configStateEnum = ActorConfigStateEnum.ActorIsSut;
			}

			return configStateEnum;
		}
	}
	#endregion

	/// <summary>
	/// Summary description for ActorConfig.
	/// </summary>
	public class ActorConfig
	{
		private ActorName _actorName;
		private ActorConfigStateEnum _actorConfigState;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="actorName">Actor Name.</param>
		/// <param name="actorConfigState">Actor Config State.</param>
		public ActorConfig(ActorName actorName, ActorConfigStateEnum actorConfigState)
		{
			_actorName = actorName;
			_actorConfigState = actorConfigState;
		}

		/// <summary>
		/// Property - ActorName
		/// </summary>
		public ActorName ActorName
		{
			get
			{
				return _actorName;
			}
		}

		/// <summary>
		/// Property - Actor Config State.
		/// </summary>
		public ActorConfigStateEnum ConfigState
		{
			get
			{
				return _actorConfigState;
			}
		}

		/// <summary>
		/// Write the configuration out in XML.
		/// </summary>
		/// <param name="writer">XML Text Writer.</param>
		public void WriteXmlConfig(XmlTextWriter writer)
		{
			writer.WriteStartElement("ActorConfiguration");
			writer.WriteStartElement("ActorName");
			writer.WriteElementString("ActorType", ActorTypes.Type(_actorName.Type));
			writer.WriteElementString("ActorId", _actorName.Id);
			writer.WriteEndElement();
			writer.WriteElementString("ConfigState", ActorConfigState.ConfigState(_actorConfigState));
			writer.WriteEndElement();
		}
	}
}
