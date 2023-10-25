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

using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Actors;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Hl7;

namespace Dvtk.IheActors.IheFramework
{
	/// <summary>
	/// Summary description for IheFrameworkConfig.
	/// </summary>
	public class IheFrameworkConfig
	{
		private System.String _profileName;
		private CommonConfig _commonConfig;
		private ActorConfigCollection _actorConfigCollection;
		private BasePeerToPeerConfigCollection _peerToPeerConfigCollection;

		/// <summary>
		/// Class Constructor.
		/// </summary>
		/// <param name="profileName">Integration Profile Name.</param>
		public IheFrameworkConfig(System.String profileName)
		{
			_profileName = profileName;
			_commonConfig = new CommonConfig();
			_actorConfigCollection = new ActorConfigCollection();
			_peerToPeerConfigCollection = new BasePeerToPeerConfigCollection();
		}

		/// <summary>
		/// Property - Profile Name.
		/// </summary>
		public System.String ProfileName
		{
			get
			{
				return _profileName;
			}
			set
			{
				_profileName = value;
			}
		}

		/// <summary>
		/// Property Common Configuration.
		/// </summary>
		public CommonConfig CommonConfig
		{
			get
			{
				return _commonConfig;
			}
		}

		/// <summary>
		/// Property Actor Configuration Collection.
		/// </summary>
		public ActorConfigCollection ActorConfig
		{
			get
			{
				return _actorConfigCollection;
			}
		}

		/// <summary>
		/// Property Peer To Peer Configuration Collection.
		/// </summary>
		public BasePeerToPeerConfigCollection PeerToPeerConfig
		{
			get
			{
				return _peerToPeerConfigCollection;
			}
		}

		/// <summary>
		/// Load the Actor Configuration into the Integration Profile.
		/// </summary>
		/// <param name="configurationFilename">Configuration Filename</param>
		public void Load(System.String configurationFilename)
		{
			_actorConfigCollection = new ActorConfigCollection();
			_peerToPeerConfigCollection = new BasePeerToPeerConfigCollection();

			try
			{
				XmlTextReader reader = new XmlTextReader(configurationFilename);
				while (reader.EOF == false)
				{
					reader.ReadStartElement("IheIntegrationProfile");
                    _profileName = reader.ReadElementString("IntegrationProfileName");
					_commonConfig.RootedBaseDirectory = reader.ReadElementString("RootedBaseDirectory");
					_commonConfig.ResultsDirectory = reader.ReadElementString("ResultsDirectory");

                    // read OverwriteResults - added later - some config files may not contain this parameter
                    reader.ReadString();
                    if (reader.Name == "OverwriteResults")
                    {
                        System.String overwriteResults = reader.ReadElementString("OverwriteResults");
                        if (overwriteResults == "True")
                        {
                            _commonConfig.OverwriteResults = true;
                        }
                        else
                        {
                            _commonConfig.OverwriteResults = false;
                        }
                    }

					_commonConfig.CredentialsFilename = reader.ReadElementString("CredentialsFilename");
				    _commonConfig.CertificateFilename = reader.ReadElementString("CertificateFilename");
				    _commonConfig.NistWebServiceUrl = reader.ReadElementString("NistWebServiceUrl");
				    _commonConfig.Hl7ProfileDirectory = reader.ReadElementString("Hl7ProfileDirectory");
				    _commonConfig.Hl7ProfileStoreName = reader.ReadElementString("Hl7ProfileStoreName");
				    _commonConfig.Hl7ValidationContextFilename = reader.ReadElementString("Hl7ValidationContextFilename");
				    System.String interactive = reader.ReadElementString("Interactive");
				    if (interactive == "True")
				    {
					    _commonConfig.Interactive = true;
				    }
				    else
				    {
					    _commonConfig.Interactive = false;
				    }

					while ((reader.IsStartElement()) && 
						(reader.Name == "ActorConfiguration"))
					{
						reader.ReadStartElement("ActorConfiguration");
						reader.ReadStartElement("ActorName");
						ActorTypeEnum actorType = ActorTypes.TypeEnum(reader.ReadElementString("ActorType"));
						System.String id = reader.ReadElementString("ActorId");
						ActorName actorName = new ActorName(actorType, id);
						reader.ReadEndElement();
						ActorConfigStateEnum configState = ActorConfigState.ConfigStateEnum(reader.ReadElementString("ConfigState"));
						ActorConfig actorConfig = new ActorConfig(actorName, configState);
						_actorConfigCollection.Add(actorConfig);
						reader.ReadEndElement();
					}

					while ((reader.IsStartElement()) && 
						((reader.Name == "DicomPeerToPeerConfiguration") ||
						(reader.Name == "Hl7PeerToPeerConfiguration")))
					{
						if (reader.Name == "DicomPeerToPeerConfiguration")
						{
							DicomPeerToPeerConfig dicomPeerToPeerConfig = new DicomPeerToPeerConfig();

							reader.ReadStartElement("DicomPeerToPeerConfiguration");
							reader.ReadStartElement("FromActor");
							reader.ReadStartElement("ActorName");
							ActorTypeEnum actorType = ActorTypes.TypeEnum(reader.ReadElementString("ActorType"));
							System.String id = reader.ReadElementString("ActorId");
							ActorName actorName = new ActorName(actorType, id);
							dicomPeerToPeerConfig.FromActorName = actorName;
							reader.ReadEndElement();
							dicomPeerToPeerConfig.FromActorAeTitle = reader.ReadElementString("AeTitle");
							reader.ReadEndElement();
							reader.ReadStartElement("ToActor");
							reader.ReadStartElement("ActorName");
							actorType = ActorTypes.TypeEnum(reader.ReadElementString("ActorType"));
							id = reader.ReadElementString("ActorId");
							actorName = new ActorName(actorType, id);
							dicomPeerToPeerConfig.ToActorName = actorName;
							reader.ReadEndElement();
							dicomPeerToPeerConfig.ToActorAeTitle = reader.ReadElementString("AeTitle");
							dicomPeerToPeerConfig.ToActorIpAddress = reader.ReadElementString("IpAddress");
							reader.ReadEndElement();
							dicomPeerToPeerConfig.PortNumber = UInt16.Parse(reader.ReadElementString("PortNumber"));
							System.String secureConnection = reader.ReadElementString("SecureConnection");
							if (secureConnection == "True")
							{
								dicomPeerToPeerConfig.SecureConnection = true;
							}
							else
							{
								dicomPeerToPeerConfig.SecureConnection = false;
							}

                            // read AutoValidate - added later - some config files may not contain this parameter
                            reader.ReadString();
                            if (reader.Name == "AutoValidate")
                            {
                                System.String autoValidate = reader.ReadElementString("AutoValidate");
                                if (autoValidate == "True")
                                {
                                    dicomPeerToPeerConfig.AutoValidate = true;
                                }
                                else
                                {
                                    dicomPeerToPeerConfig.AutoValidate = false;
                                }
                            }

							dicomPeerToPeerConfig.ActorOption1 = reader.ReadElementString("ActorOption1");
							dicomPeerToPeerConfig.ActorOption2 = reader.ReadElementString("ActorOption2");
							dicomPeerToPeerConfig.ActorOption3 = reader.ReadElementString("ActorOption3");
							dicomPeerToPeerConfig.SessionId = UInt16.Parse(reader.ReadElementString("SessionId"));
							dicomPeerToPeerConfig.SourceDataDirectory = reader.ReadElementString("SourceDataDirectory");
							dicomPeerToPeerConfig.StoreDataDirectory = reader.ReadElementString("StoreDataDirectory");
							System.String storeData = reader.ReadElementString("StoreData");
							if (storeData == "True")
							{
								dicomPeerToPeerConfig.StoreData = true;
							}
							else
							{
								dicomPeerToPeerConfig.StoreData = false;
							}

							reader.ReadStartElement("DefinitionFiles");

							bool readingDefinitionFiles = true;
							while (readingDefinitionFiles == true)
							{
								dicomPeerToPeerConfig.AddDefinitionFile(reader.ReadElementString("DefinitionFile"));
								reader.Read();
								if ((reader.NodeType == XmlNodeType.EndElement) &&
									(reader.Name == "DefinitionFiles"))
								{
									reader.Read();
									readingDefinitionFiles = false;
								}
							}

							_peerToPeerConfigCollection.Add(dicomPeerToPeerConfig);

							reader.ReadEndElement();
						}
						else
						{
							Hl7PeerToPeerConfig hl7PeerToPeerConfig = new Hl7PeerToPeerConfig();

							reader.ReadStartElement("Hl7PeerToPeerConfiguration");
							reader.ReadStartElement("FromActor");
							reader.ReadStartElement("ActorName");
							ActorTypeEnum actorType = ActorTypes.TypeEnum(reader.ReadElementString("ActorType"));
							System.String id = reader.ReadElementString("ActorId");
							ActorName actorName = new ActorName(actorType, id);
							hl7PeerToPeerConfig.FromActorName = actorName;
							reader.ReadEndElement();
							hl7PeerToPeerConfig.FromActorAeTitle = reader.ReadElementString("AeTitle");
							reader.ReadEndElement();
							reader.ReadStartElement("ToActor");
							reader.ReadStartElement("ActorName");
							actorType = ActorTypes.TypeEnum(reader.ReadElementString("ActorType"));
							id = reader.ReadElementString("ActorId");
							actorName = new ActorName(actorType, id);
							hl7PeerToPeerConfig.ToActorName = actorName;
							reader.ReadEndElement();
							hl7PeerToPeerConfig.ToActorAeTitle = reader.ReadElementString("AeTitle");
							hl7PeerToPeerConfig.ToActorIpAddress = reader.ReadElementString("IpAddress");
							reader.ReadEndElement();
							hl7PeerToPeerConfig.PortNumber = UInt16.Parse(reader.ReadElementString("PortNumber"));
							hl7PeerToPeerConfig.MessageDelimiters.FromString(reader.ReadElementString("MessageDelimiters"));
							System.String secureConnection = reader.ReadElementString("SecureConnection");
							if (secureConnection == "True")
							{
								hl7PeerToPeerConfig.SecureConnection = true;
							}
							else
							{
								hl7PeerToPeerConfig.SecureConnection = false;
							}

                            // read AutoValidate - added later - some config files may not contain this parameter
                            reader.ReadString();
                            if (reader.Name == "AutoValidate")
                            {
                                System.String autoValidate = reader.ReadElementString("AutoValidate");
                                if (autoValidate == "True")
                                {
                                    hl7PeerToPeerConfig.AutoValidate = true;
                                }
                                else
                                {
                                    hl7PeerToPeerConfig.AutoValidate = false;
                                }
                            }

                            hl7PeerToPeerConfig.ActorOption1 = reader.ReadElementString("ActorOption1");
							hl7PeerToPeerConfig.ActorOption2 = reader.ReadElementString("ActorOption2");
							hl7PeerToPeerConfig.ActorOption3 = reader.ReadElementString("ActorOption3");
							hl7PeerToPeerConfig.SessionId = UInt16.Parse(reader.ReadElementString("SessionId"));

							_peerToPeerConfigCollection.Add(hl7PeerToPeerConfig);

							reader.ReadEndElement();
						}
					}

					reader.ReadEndElement();
				}

				reader.Close();
			}
			catch (System.Exception e)
			{
				System.String message = System.String.Format("Failed to read configuration file: \"{0}\". Error: \"{1}\"", configurationFilename, e.Message);
				throw new System.SystemException(message, e);
			}
		}

		/// <summary>
		/// Save the Actor Configuration into the given file.
		/// </summary>
		/// <param name="configurationFilename">Configuration Filename</param>
		public void Save(System.String configurationFilename)
		{
			XmlTextWriter writer = new XmlTextWriter(configurationFilename, System.Text.Encoding.ASCII);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument(true);
			writer.WriteStartElement("IheIntegrationProfile");
			writer.WriteElementString("IntegrationProfileName", _profileName);
			writer.WriteElementString("RootedBaseDirectory", _commonConfig.RootedBaseDirectory);
			writer.WriteElementString("ResultsDirectory", _commonConfig.ResultsDirectory);
			writer.WriteElementString("CredentialsFilename", _commonConfig.CredentialsFilename);
			writer.WriteElementString("CertificateFilename", _commonConfig.CertificateFilename);
			writer.WriteElementString("NistWebServiceUrl", _commonConfig.NistWebServiceUrl);
			writer.WriteElementString("Hl7ProfileDirectory", _commonConfig.Hl7ProfileDirectory);
			writer.WriteElementString("Hl7ProfileStoreName", _commonConfig.Hl7ProfileStoreName);
			writer.WriteElementString("Hl7ValidationContextFilename", _commonConfig.Hl7ValidationContextFilename);
			writer.WriteElementString("Interactive", _commonConfig.Interactive.ToString());

			foreach(ActorConfig actorConfig in _actorConfigCollection)
			{
				actorConfig.WriteXmlConfig(writer);
			}

			foreach(BasePeerToPeerConfig peerToPeerConfig in _peerToPeerConfigCollection)
			{
				peerToPeerConfig.WriteXmlConfig(writer);
			}

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
		}
	}
}
