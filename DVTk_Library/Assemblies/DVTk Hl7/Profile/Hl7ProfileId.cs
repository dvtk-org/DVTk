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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Dvtk.Hl7
{
	/// <summary>
	/// Summary description for Hl7ProfileId.
	/// </summary>
	public class Hl7ProfileId
	{
		private System.String _facility = System.String.Empty;
		private System.String _version = System.String.Empty;
		private System.String _messageType = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="facility">HL7 Facility.</param>
		/// <param name="version">HL7 Version.</param>
		/// <param name="messageType">HL7 Message Type.</param>
		public Hl7ProfileId(System.String facility, System.String version, System.String messageType)
		{
			_facility = facility;
			_version = version;
			_messageType = messageType;
		}

		/// <summary>
		/// Property - Facility.
		/// </summary>
		public System.String Facility
		{
			get
			{
				return _facility;
			}
		}

		/// <summary>
		/// Property - Version.
		/// </summary>
		public System.String Version
		{
			get
			{
				return _version;
			}
		}

		/// <summary>
		/// Property - MessageType.
		/// </summary>
		public System.String MessageType
		{
			get
			{
				return _messageType;
			}
		}

		/// <summary>
		/// Property - Id.
		/// </summary>
		public System.String Id
		{
			get
			{
				return _facility + ":" + _version + ":" + _messageType;
			}
		}

		/// <summary>
		/// Write Profile Id in XML format to the XML Text Writer.
		/// </summary>
		/// <param name="writer">XML Text Writer</param>
		public void WriteXml(XmlTextWriter writer)
		{
			writer.WriteStartElement("Hl7ProfileId");
			writer.WriteElementString("Facility", _facility);
			writer.WriteElementString("Version", _version);
			writer.WriteElementString("MessageType", _messageType);
			writer.WriteEndElement();
		}
	}
}
