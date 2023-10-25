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

namespace Dvtk.IheActors.Dicom
{
	/// <summary>
	/// Summary description for DicomPeerToPeerConfig.
	/// </summary>
	public class DicomPeerToPeerConfig : BasePeerToPeerConfig
	{
		private System.UInt16 _sessionId = 0;
		private System.Collections.ArrayList _definitionFiles = new ArrayList();
		private System.String _sourceDataDirectory = System.String.Empty;
		private System.String _storeDataDirectory = System.String.Empty;
		private bool _storeData = false;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public DicomPeerToPeerConfig() : base()
		{
			//
			// Constructor activities
			//
			_sessionId = 0;
			_definitionFiles = new ArrayList();
			_sourceDataDirectory = "";
			_storeDataDirectory = "";
			_storeData = false;
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
		/// Add Definition Files.
		/// </summary>
		/// <param name="definitionFile">Definition Filename.</param>
		public void AddDefinitionFile(System.String definitionFile)
		{
			_definitionFiles.Add(definitionFile);
		}

		/// <summary>
		/// Property - DefinitionFiles.
		/// </summary>
		public System.Collections.ArrayList DefinitionFiles
		{
			get
			{
				return _definitionFiles;
			}
		}

		/// <summary>
		/// Property - SourceDataDirectory.
		/// </summary>
		public System.String SourceDataDirectory
		{
			get 
			{ 
				return _sourceDataDirectory; 
			}
			set 
			{ 
				_sourceDataDirectory = value; 
			}
		}

		/// <summary>
		/// Property - StoreDataDirectory.
		/// </summary>
		public System.String StoreDataDirectory
		{
			get 
			{ 
				return _storeDataDirectory; 
			}
			set 
			{ 
				_storeDataDirectory = value; 
			}
		}

		/// <summary>
		/// Property - StoreData.
		/// </summary>
		public bool StoreData
		{
			get 
			{ 
				return _storeData; 
			}
			set 
			{ 
				_storeData = value; 
			}
		}

		/// <summary>
		/// Write Configuration data in XML format to the XML Text Writer.
		/// </summary>
		/// <param name="writer">XML Text Writer</param>
		public override void WriteXmlConfig(XmlTextWriter writer)
		{
			writer.WriteStartElement("DicomPeerToPeerConfiguration");
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
			writer.WriteElementString("SecureConnection", SecureConnection.ToString());
            writer.WriteElementString("AutoValidate", AutoValidate.ToString());
            writer.WriteElementString("ActorOption1", ActorOption1);
			writer.WriteElementString("ActorOption2", ActorOption2);
			writer.WriteElementString("ActorOption3", ActorOption3);
			writer.WriteElementString("SessionId", _sessionId.ToString());
			writer.WriteElementString("SourceDataDirectory", _sourceDataDirectory);
			writer.WriteElementString("StoreDataDirectory", _storeDataDirectory);
			writer.WriteElementString("StoreData", _storeData.ToString());
			writer.WriteStartElement("DefinitionFiles");
			foreach (System.String defintionFilename in _definitionFiles)
			{
				writer.WriteElementString("DefinitionFile", defintionFilename);
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}
}
