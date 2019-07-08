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
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace QR_Emulator
{
	/// <summary>
	/// Class  containing all serializable configuration items.
	/// </summary>
	public class Config
	{
		//
		// - Constant fields -
		//
		private const String CONFIG_FILE_NAME_ONLY = "Config.xml";

        private static string configFileName = Path.Combine(Application.StartupPath, CONFIG_FILE_NAME_ONLY);

		//
		// - Fields -
		//
		/// <summary>
		/// See property Data directory for emulation.
		/// </summary>
		private string dataDirectoryForEmulation = "";

		/// <summary>
		/// See property PatientRootInfoModelSupport for Q/R.
		/// </summary>
		private bool patientInfoModelChecked = true;

		/// <summary>
		/// See property PatientStudyRootInfoModelSupport for Q/R.
		/// </summary>
		private bool patientStudyInfoModelChecked = false;

		/// <summary>
		/// See property StudyRootInfoModelSupport for Q/R.
		/// </summary>
		private bool studyInfoModelChecked = false;

		/// <summary>
		/// See property TSILESupport Checked.
		/// </summary>
		private bool tsILEChecked = true;

		/// <summary>
		/// See property TSELESupport.
		/// </summary>
		private bool tsELEChecked = false;

		/// <summary>
		/// See property TSEBESupport.
		/// </summary>
		private bool tsEBEChecked = false;

        private string localAeTitle = "DVTK_QR_SCP";

        private string localPort = "106";

        private string remoteAeTitle = "";

        private string remotePort = "";

        private string remoteIpAddress = "";

        bool isCaseSensitiveAE = false;
        bool isCaseSensitiveCS = false;
        bool isCaseSensitivePN = false;
        bool isCaseSensitiveSH = false;
        bool isCaseSensitiveLO = false;
        //
        // - Constructors -
        //
        /// <summary>
		/// Default constructor.
		/// </summary>
		public Config()
		{
			// Do nothing.
		}

		//
		// - Properties -
		//
		public string DataDirectoryForEmulation
		{
			get
			{
				return(this.dataDirectoryForEmulation);
			}
			set
			{
				this.dataDirectoryForEmulation = value;
			}
		}

		/// <summary>
		/// The full file name of the config xml file.
		/// </summary>
        public static String ConfigFullFileName
		{
			get
			{
                return configFileName;
			}
            set
            {
                configFileName = value;
            }
		}

		/// <summary>
		/// Indicates if the menu item is checked.
		/// </summary>
		public bool PatientRootInfoModelSupport
		{
			get
			{
				return(this.patientInfoModelChecked);
			}
			set
			{
				this.patientInfoModelChecked = value;
			}
		}

		/// <summary>
		/// Indicates if the menu item is checked.
		/// </summary>
		public bool PatientStudyRootInfoModelSupport
		{
			get
			{
				return(this.patientStudyInfoModelChecked);
			}
			set
			{
				this.patientStudyInfoModelChecked = value;
			}
		}

		/// <summary>
		/// Indicates if the menu item is checked.
		/// </summary>
		public bool StudyRootInfoModelSupport
		{
			get
			{
				return(this.studyInfoModelChecked);
			}
			set
			{
				this.studyInfoModelChecked = value;
			}
		}

		/// <summary>
		/// Indicates if the menu item is checked.
		/// </summary>
		public bool TSILESupport
		{
			get
			{
				return(this.tsILEChecked);
			}
			set
			{
				this.tsILEChecked = value;
			}
		}

		/// <summary>
		/// Indicates if the menu item is checked.
		/// </summary>
		public bool TSELESupport
		{
			get
			{
				return(this.tsELEChecked);
			}
			set
			{
				this.tsELEChecked = value;
			}
		}

		/// <summary>
		/// Indicates if the menu item is checked.
		/// </summary>
		public bool TSEBESupport
		{
			get
			{
				return(this.tsEBEChecked);
			}
			set
			{
				this.tsEBEChecked = value;
			}
		}

        /// <summary>
        /// The AE Title of the local machine.
        /// </summary>
        public string LocalAeTitle
        {
            get
            {
                return (this.localAeTitle);
            }
            set
            {
                this.localAeTitle = value;
            }
        }

        /// <summary>
        /// The port on which the QR Emulator is listening to for new associations.
        /// </summary>
        public string LocalPort
        {
            get
            {
                return (this.localPort);
            }
            set
            {
                this.localPort = value;
            }
        }
        
        /// <summary>
        /// AE title of the SUT.
        /// </summary>
        public string RemoteAeTitle
        {
            get
            {
                return (this.remoteAeTitle);
            }
            set
            {
                this.remoteAeTitle = value;
            }
        }

        /// <summary>
        /// The Port on which the SUT is
        /// </summary>
        public string RemotePort
        {
            get
            {
                return (this.remotePort);
            }
            set
            {
                this.remotePort = value;
            }
        }

        /// <summary>
        /// IP address of the SUT.
        /// </summary>
        public string RemoteIpAddress
        {
            get
            {
                return (this.remoteIpAddress);
            }
            set
            {
                this.remoteIpAddress = value;
            }
        }
        /// <summary>
        /// Get or Set the case sensitivity of AE
        /// </summary>
        public bool IsCaseSensitiveAE
        {
            get
            {
                return isCaseSensitiveAE;
            }
            set
            {
                isCaseSensitiveAE = value;
            }
        }
        /// <summary>
        /// Get or set the case sensitivity query of CS
        /// </summary>
        public bool IsCaseSensitiveCS
        {
            get
            {
                return isCaseSensitiveCS;
            }
            set
            {
                isCaseSensitiveCS = value;
            }
        }
        /// <summary>
        /// Get or set the case sensitivity query of PN
        /// </summary>
        public bool IsCaseSensitivePN
        {
            get
            {
                return isCaseSensitivePN;
            }
            set
            {
                isCaseSensitivePN = value;
            }
        }
        /// <summary>
        /// Get or set the case sensitivity query of SH
        /// </summary>
        public bool IsCaseSensitiveSH
        {
            get
            {
                return isCaseSensitiveSH;
            }
            set
            {
                isCaseSensitiveSH = value;
            }
        }
        /// <summary>
        /// Get or set the case sensitivity of LO
        /// </summary>
        public bool IsCaseSensitiveLO
        {
            get
            {
                return isCaseSensitiveLO;
            }
            set
            {
                isCaseSensitiveLO = value;
            }
        }
		//
		// - Methods -
		//
		/// <summary>
		/// Deserialize an xml file to an Config instance.
		/// </summary>
		/// <returns>The deserialized Config instance.</returns>
        public static Config Deserialize(string configFile)
		{
			FileStream myFileStream = null;

			XmlSerializer mySerializer = new XmlSerializer(typeof(Config));

			Config config = null;

			try
			{
				// To read the file, creates a FileStream.
                myFileStream = new FileStream(configFile, FileMode.Open);

				// Calls the Deserialize method and casts to the object type.
				config = (Config)mySerializer.Deserialize(myFileStream);
			}
			catch
			{
				// If deserializing the config file fails, use the default settings.
				config = new Config();
			}
			finally
			{
				if (myFileStream != null)
				{
					myFileStream.Close();
				}
			}

			return(config);
		}

		/// <summary>
		/// Serialize this Config instance to an xml file.
		/// </summary>
		public void Serialize()
		{
			StreamWriter myWriter = null;

			try
			{
				XmlSerializer mySerializer = new XmlSerializer(typeof(Config));

				// To write to a file, create a StreamWriter object.
				myWriter = new StreamWriter(ConfigFullFileName);

				mySerializer.Serialize(myWriter, this);

			}
			catch
			{
				// Do nothing.
			}
			finally
			{
				if (myWriter != null)
				{
					myWriter.Close();
				}
			}
		}
	}
}
