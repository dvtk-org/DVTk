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
using Dvtk.Hl7.Messages;

namespace Dvtk.IheActors.Hl7
{
	/// <summary>
	/// Summary description for Hl7PeerToPeerConfig.
	/// </summary>
	public class Hl7PeerToPeerConfig : BasePeerToPeerConfig
	{
		private System.UInt16 _sessionId = 0;
		private Hl7MessageDelimiters _messageDelimiters = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public Hl7PeerToPeerConfig() : base()
		{
			//
			// Constructor activities
			//
			_sessionId = 0;
			_messageDelimiters = new Hl7MessageDelimiters();
		}

		/// <summary>
		/// Proprty - SessionId.
		/// </summary>
		public System.UInt16 SessionId
		{
			get 
			{ 
				return _sessionId; 
			}
			set 
			{ 
				_sessionId = value; 
			}
		}

		/// <summary>
		/// Proprty - MessageDelimiters.
		/// </summary>
		public Hl7MessageDelimiters MessageDelimiters
		{
			get 
			{ 
				return _messageDelimiters; 
			}
		}

		/// <summary>
		/// Write Configuration data in XML format to the XML Text Writer.
		/// </summary>
		/// <param name="writer">XML Text Writer</param>
		public override void WriteXmlConfig(XmlTextWriter writer)
		{
			writer.WriteStartElement("Hl7PeerToPeerConfiguration");
			writer.WriteStartElement("FromActor");
			writer.WriteStartElement("ActorName");
			writer.WriteElementString("ActorType", ActorTypes.Type(FromActorName.Type));
			writer.WriteElementString("ActorId", FromActorName.Id);
			writer.WriteEndElement();
			writer.WriteElementString("AeTitle", FromActorAeTitle);
			writer.WriteEndElement();
			writer.WriteStartElement("ToActor");
			writer.WriteStartElement("ActorName");
			writer.WriteElementString("ActorType", ActorTypes.Type(ToActorName.Type));
			writer.WriteElementString("ActorId", ToActorName.Id);
			writer.WriteEndElement();
			writer.WriteElementString("AeTitle", ToActorAeTitle);
			writer.WriteElementString("IpAddress", ToActorIpAddress);
			writer.WriteEndElement();
			writer.WriteElementString("PortNumber", PortNumber.ToString());
			writer.WriteElementString("MessageDelimiters", MessageDelimiters.ToString());
			writer.WriteElementString("SecureConnection", SecureConnection.ToString());
            writer.WriteElementString("AutoValidate", AutoValidate.ToString());
            writer.WriteElementString("ActorOption1", ActorOption1);
			writer.WriteElementString("ActorOption2", ActorOption2);
			writer.WriteElementString("ActorOption3", ActorOption3);
			writer.WriteElementString("SessionId", _sessionId.ToString());
			writer.WriteEndElement();
		}
	}
}
