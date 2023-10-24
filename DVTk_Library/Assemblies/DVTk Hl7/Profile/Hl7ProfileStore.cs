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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Dvtk.Hl7
{
	/// <summary>
	/// Summary description for Hl7ProfileStore.
	/// </summary>
	public class Hl7ProfileStore
	{
		private Hashtable _hl7ProfileStore = null;
		private System.String _directory = System.String.Empty;
		private System.String _filename = System.String.Empty;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="directory">HL7 Profile Directory.</param>
		/// <param name="filename">HL7 Profile Filename.</param>
		public Hl7ProfileStore(System.String directory, System.String filename)
		{
			_directory = directory;
			_filename = filename;

			// Load the profile into the profile store
			Load();
		}

		private void Load()
		{
			_hl7ProfileStore = new Hashtable();

			try
			{
				XmlTextReader reader = new XmlTextReader(_directory + "/" + _filename);
				while (reader.EOF == false)
				{
					reader.ReadStartElement("Hl7ProfileStore");

					while ((reader.IsStartElement()) && 
						(reader.Name == "Hl7Profile"))
					{
						reader.ReadStartElement("Hl7Profile");
						reader.ReadStartElement("Hl7ProfileId");
						System.String facility = reader.ReadElementString("Facility");
						System.String version = reader.ReadElementString("Version");
						System.String messageType = reader.ReadElementString("MessageType");
						reader.ReadEndElement();

						System.String filename = reader.ReadElementString("Filename");
						reader.ReadEndElement();

						Hl7Profile hl7Profile = new Hl7Profile(facility, version, messageType, _directory, filename);
						_hl7ProfileStore.Add(hl7Profile.Id, hl7Profile);
					}
					reader.ReadEndElement();
				}
				reader.Close();
			}
			catch (System.Exception e)
			{
				System.String message = System.String.Format("Failed to read HL7 Profile: \"{0}\"", _directory + "/" + _filename);
				throw new System.SystemException(message, e);
			}
		}

		/// <summary>
		/// Get the XmlHl7Profile that corresponds with the Hl7ProfileId.
		/// </summary>
		/// <param name="profileId">HL7 Profile Id.</param>
		/// <returns>XmlHl7Profile.</returns>
		public System.String GetXmlHl7Profile(Hl7ProfileId profileId)
		{
			System.String xmlHl7Profile = System.String.Empty;
			Hl7Profile hl7Profile = (Hl7Profile)_hl7ProfileStore[profileId.Id];
			if (hl7Profile != null)
			{
				xmlHl7Profile = hl7Profile.XmlProfile;
			}
			return xmlHl7Profile;
		}
	}
}
