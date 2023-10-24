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
	/// Summary description for Hl7Profile.
	/// </summary>
	public class Hl7Profile
	{
		private Hl7ProfileId _profileId = null;
		private System.String _directory = System.String.Empty;
		private System.String _filename = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="facility">HL7 Facility.</param>
		/// <param name="version">HL7 Version.</param>
		/// <param name="messageType">HL7 Message Type.</param>
		/// <param name="directory">HL7 Profile Directory.</param>
		/// <param name="filename">HL7 Profile Filename.</param>
		public Hl7Profile(System.String facility, System.String version, System.String messageType, System.String directory, System.String filename)
		{
			_profileId = new Hl7ProfileId(facility, version, messageType);
			_directory = directory;
			_filename = filename;
		}

		/// <summary>
		/// Property - Id.
		/// </summary>
		public System.String Id
		{
			get
			{
				System.String id = System.String.Empty;
				if (_profileId != null)
				{
					id = _profileId.Id;
				}
				return id;
			}
		}

		/// <summary>
		/// Write Profile in XML format to the XML Text Writer.
		/// </summary>
		/// <param name="writer">XML Text Writer</param>
		public void WriteXml(XmlTextWriter writer)
		{
			writer.WriteStartElement("Hl7Profile");
			if (_profileId != null)
			{
				_profileId.WriteXml(writer);
			}
			writer.WriteElementString("Filename", _filename);
			writer.WriteEndElement();
		}

		/// <summary>
		/// Property XmlProfile.
		/// </summary>
		public System.String XmlProfile
		{
			get
			{
				System.String xmlProfile = null;
				try
				{
					System.String input = null;
					System.IO.StreamReader sr = new System.IO.StreamReader(_directory + "/" + _filename);
					while ((input = sr.ReadLine()) != null)
					{
						xmlProfile += input;
					}
					sr.Close();
				}
				catch (System.Exception e)
				{
					System.String message = System.String.Format("Failed to read XML HL7 Profile: \"{0}\"", _directory + "/" + _filename);
					throw new System.SystemException(message, e);
				}

				return xmlProfile;
			}
		}
	}
}
