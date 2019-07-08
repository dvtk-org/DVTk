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
using System.Diagnostics;


namespace Dvt
{
	/// <summary>
	/// This class represents the User settings of DVT.
	/// The settings may be loaded and saved to the setting file in the application data folder of the logged in User. 
	/// </summary>
	public class UserSettings
	{
		// The actual user settings.
		private bool _AskForBackupResultsFile;
		private bool _ShowEmptySessions;
		private bool _ShowDicomScripts;
		private bool _ShowDicomSuperScripts;
		private bool _ShowVisualBasicScripts;
		private bool _ExpandVisualBasicScript;

		private string _UserSettingsFullFileName = "";
		private string _UserSettingsFullPath = "";
		private bool _DoUnsavedChangesExist = false;

		
		/// <summary>
		/// Constructor
		/// </summary>
		public UserSettings()
		{
			string theApplicationDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			_UserSettingsFullPath = Path.Combine(theApplicationDataFolder, "Dvt");
			_UserSettingsFullFileName = Path.Combine(_UserSettingsFullPath, "User Settings.xml");

			SetDefaultValues();
		}

		/// <summary>
		/// Load the User settings from the setting file.
		/// </summary>
		public void Load()
		{
			if (File.Exists(_UserSettingsFullFileName))
			{
				XmlTextReader theXmlTextReader = null;

				try
				{
					// Use the default settings as a starting point.
					SetDefaultValues();

					theXmlTextReader = new XmlTextReader(_UserSettingsFullFileName);

					theXmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
					theXmlTextReader.MoveToContent();

					_AskForBackupResultsFile = ReadBoolean(theXmlTextReader);
					_ShowEmptySessions = ReadBoolean(theXmlTextReader);
					_ShowDicomScripts = ReadBoolean(theXmlTextReader);
					_ShowDicomSuperScripts = ReadBoolean(theXmlTextReader);
					_ShowVisualBasicScripts = ReadBoolean(theXmlTextReader);
					_ExpandVisualBasicScript = ReadBoolean(theXmlTextReader);
				}
				catch (Exception theException)
				{
					Debug.Assert(false, theException.Message + "\n May be OK if user settings have been added\nWhen the user settings are saved this problem should not occur anymore");

					// If reading the file fails, use the default settings.
					SetDefaultValues();
				}
				finally
				{
					if (theXmlTextReader != null)
					{
						theXmlTextReader.Close();
					}
				}	
			}
			else
			{
				// Use the default settings.
				SetDefaultValues();
			}
		}

		/// <summary>
		/// Save the User settings to the setting file.
		/// </summary>
		public void Save()
		{
			XmlTextWriter theXmlTextWriter = null;

			try
			{
				if (!Directory.Exists(_UserSettingsFullPath))
				{
					Directory.CreateDirectory(_UserSettingsFullPath);
				}

				theXmlTextWriter = new XmlTextWriter(_UserSettingsFullFileName, System.Text.Encoding.ASCII);

				// The written .xml file will be more readable
				theXmlTextWriter.Formatting = Formatting.Indented;

				// Start the document
				theXmlTextWriter.WriteStartDocument (true);

				theXmlTextWriter.WriteStartElement ("Settings");

				WriteBoolean(theXmlTextWriter, "AskForBackupResultsFile", _AskForBackupResultsFile);
				WriteBoolean(theXmlTextWriter, "ShowEmptySessions", ShowEmptySessions);
				WriteBoolean(theXmlTextWriter, "ShowDicomScripts", _ShowDicomScripts);
				WriteBoolean(theXmlTextWriter, "ShowDicomSuperScripts", _ShowDicomSuperScripts);
				WriteBoolean(theXmlTextWriter, "ShowVisualBasicScripts", _ShowVisualBasicScripts);
				WriteBoolean(theXmlTextWriter, "ExpandVisualBasicScript", _ExpandVisualBasicScript);

				theXmlTextWriter.WriteEndElement ();

				// End the document
				theXmlTextWriter.WriteEndDocument ();
			}
			catch
			{

			}
			finally
			{
				if (theXmlTextWriter != null)
				{
					theXmlTextWriter.Close();
				}
			}
		}

		/// <summary>
		/// Read the value of a boolean from the setting file.
		/// </summary>
		/// <param name="theXmlTextReader">The XmlTextReader object reading the setting file.</param>
		/// <returns>Return the value of the read boolean.</returns>
		private bool ReadBoolean(XmlTextReader theXmlTextReader)
		{
			bool theBoolean = true;

			if (theXmlTextReader.ReadElementString() == "true")
			{
				theBoolean = true;
			}
			else
			{
				theBoolean = false;
			}

			return(theBoolean);
		}

		/// <summary>
		/// Write a boolean to the setting file.
		/// </summary>
		/// <param name="theXmlTextWriter">The XmlTextWriter object writing the setting file.</param>
		/// <param name="theValueName">The name of the boolean value.</param>
		/// <param name="theValue">The boolean value.</param>
		private void WriteBoolean(XmlTextWriter theXmlTextWriter, string theValueName, bool theValue)
		{
			string theBooleanString = "true";

			if (theValue)
			{
				theBooleanString = "true";
			}
			else
			{
				theBooleanString = "false";
			}

			theXmlTextWriter.WriteElementString(theValueName, theBooleanString);
		}

		/// <summary>
		/// Set the default values of the User settings.
		/// </summary>
		private void SetDefaultValues()
		{
			
            _AskForBackupResultsFile = true;
			_ShowEmptySessions = true;
			_ShowDicomScripts = true;
			_ShowDicomSuperScripts = true;
			_ShowVisualBasicScripts = true;
			_ExpandVisualBasicScript = false;
		}

		/// <summary>
		/// This property indicates if DVT should ask to backup the results file before removing/overwriting it.
		/// </summary>
		public bool AskForBackupResultsFile
		{
			get
			{
				return _AskForBackupResultsFile;
			}
			set
			{
				_AskForBackupResultsFile = value;
				_DoUnsavedChangesExist = true;
			}
		}

		/// <summary>
		/// This property indicates of DVT must show empty sessions. 
		/// An empty session is a script session containing no visible scripts. 
		/// </summary>
		public bool ShowEmptySessions
		{
			get
			{
				return _ShowEmptySessions;
			}
			set
			{
				_ShowEmptySessions = value;
				_DoUnsavedChangesExist = true;
			}
		}

		/// <summary>
		/// This property indicates if .ds files must be visible in a script session.
		/// </summary>
		public bool ShowDicomScripts
		{
			get
			{
				return _ShowDicomScripts;
			}
			set
			{
				_ShowDicomScripts = value;
				_DoUnsavedChangesExist = true;
			}
		}

		/// <summary>
		/// This property indicates if .dss files must be visible in a script session.
		/// </summary>
		public bool ShowDicomSuperScripts
		{
			get
			{
				return _ShowDicomSuperScripts;
			}
			set
			{
				_ShowDicomSuperScripts = value;
				_DoUnsavedChangesExist = true;
			}
		}


		/// <summary>
		/// This property indicates if DVT should ask to backup the results file before removing/overwriting it.
		/// </summary>
		public bool ShowVisualBasicScripts
		{
			get
			{
				return _ShowVisualBasicScripts;
			}
			set
			{
				_ShowVisualBasicScripts = value;
				_DoUnsavedChangesExist = true;
			}
		}

		/// <summary>
		/// This property indicates, when displaying a .vbs file, if the #include statements must be expanded.
		/// </summary>
		public bool ExpandVisualBasicScript
		{
			get
			{
				return _ExpandVisualBasicScript;
			}
			set
			{
				_ExpandVisualBasicScript = value;
				_DoUnsavedChangesExist = true;
			}
		}

		/// <summary>
		/// This property indicates if unsaved changes exist.
		/// </summary>
		public bool DoUnsavedChangesExist
		{
			get
			{
				return _DoUnsavedChangesExist;
			}
		}
	}
}
