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

namespace RIS_Emulator
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
        private static string configFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\RIS Emulator", CONFIG_FILE_NAME_ONLY);

		//
		// - Fields -
		//
		/// <summary>
		/// See property Data directory for emulation.
		/// </summary>
		private string dataDirectoryForEmulation = "";

		/// <summary>
		/// See property Nr of Rsps.
		/// </summary>
		private int nrOfRsps = 1;

		/// <summary>
		/// See property DCM File for Randomized responses.
		/// </summary>
        private string dcmFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\RIS Emulator\Data\Worklist\d1I00001.dcm";

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


        private bool spsdChecked = true;

        private string worklistLocalAeTitle = "DVTK_MWL_SCP";

        private string worklistLocalPort = "107";

        private string worklistRemoteAeTitle = "";

       // private string workListRemoteIpAddress = "";

        private string mppsLocalAeTitle = "DVTK_MPPS_SCP";

        private string mppsLocalPort = "108";

        private string mppsRemoteAeTitle = "";

       // private string mppsRemoteIpAddress = "";

       

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
        public static string ConfigFullFileName
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
		public int NrOfRandomReps
		{
			get
			{
				return(this.nrOfRsps);
			}
			set
			{
				this.nrOfRsps = value;
			}
		}

		/// <summary>
		/// Indicates if the menu item is checked.
		/// </summary>
		public string DCMFileForRandomRsps
		{
			get
			{
				return(this.dcmFileName);
			}
			set
			{
				this.dcmFileName = value;
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

        public bool SPSDChecked
        {
            get
            {
                return (this.spsdChecked);
            }

            set
            {
                this.spsdChecked = value;
            }

        }

        public string WorklistLocalAeTitle
        {
            get { return worklistLocalAeTitle; }
            set { worklistLocalAeTitle = value; }
        }

        public string WorklistLocalPort
        {
            get { return worklistLocalPort; }
            set { worklistLocalPort = value; }
        }

        public string WorklistRemoteAeTitle
        {
            get { return worklistRemoteAeTitle; }
            set { worklistRemoteAeTitle = value; }
        }

        //public string WorkListRemoteIpAddress
        //{
        //    get { return workListRemoteIpAddress; }
        //    set { workListRemoteIpAddress = value; }
        //}

        public string MppsLocalAeTitle
        {
            get { return mppsLocalAeTitle; }
            set { mppsLocalAeTitle = value; }
        }

        public string MppsLocalPort
        {
            get { return mppsLocalPort; }
            set { mppsLocalPort = value; }
        }

        public string MppsRemoteAeTitle
        {
            get { return mppsRemoteAeTitle; }
            set { mppsRemoteAeTitle = value; }
        }

        //public string MppsRemoteIpAddress
        //{
        //    get { return mppsRemoteIpAddress; }
        //    set { mppsRemoteIpAddress = value; }
        //}


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
