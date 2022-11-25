// ------------------------------------------------------
// Original author: Marco Kemper
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
using System.Collections.Specialized;

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Dicom.Threads
{
	/// <summary>
	/// Summary description for DicomThreadOptions.
	/// </summary>
	public class DicomThreadOptions: ThreadOptions
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property AutoValidate.
		/// </summary>
		private bool autoValidate = true;

		private DicomThread dicomThread = null;


		private bool logThreadStartingAndStoppingInParent = true;

		private bool logChildThreadsOverview = true;

		private int subResultsFileNameIndex = 0;

		/// <summary>
		/// See property StyleSheetFullFileName.
		/// </summary>
		private String styleSheetFullFileName = "";



		//
		// - Constructors -
		//

		/// <summary>
		/// Hidden default constructor.
		/// </summary>
		private DicomThreadOptions()
		{
			this.styleSheetFullFileName = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "DVT_RESULTS.xslt");
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="dicomThread">The DicomThread these options belong to.</param>
		public DicomThreadOptions(DicomThread dicomThread)
		{
			this.dicomThread = dicomThread;
			this.styleSheetFullFileName = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "DVT_RESULTS.xslt");
		}



		//
		// - Properties -
		//

        /// <summary>
        /// This name is used to select the appropriate definition file from the set of loaded
        /// definition files during validation.
        /// </summary>
        public String DefinitionFileApplicationEntityName
        {
            get
            {
                return this.DvtkScriptSession.DefinitionManagement.ApplicationEntityName;
            }

            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.DefinitionManagement.ApplicationEntityName = value;
            }
        }

        /// <summary>
        /// This version is used to select the appropriate definition file from the set of loaded
        /// definition files during validation.
        /// </summary>
        public String DefinitionFileApplicationEntityVersion
        {
            get
            {
                return this.DvtkScriptSession.DefinitionManagement.ApplicationEntityVersion;
            }

            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.DefinitionManagement.ApplicationEntityVersion = value;
            }
        }

        /// <summary>
        /// Gets or sets the AE title used by the DicomThread instance.
        /// </summary>
        public String LocalAeTitle
        {
            get
            {
                return this.DvtkScriptSession.DvtSystemSettings.AeTitle;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.DvtSystemSettings.AeTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the implementation class UID used by the DicomThread instance.
        /// </summary>
        public String LocalImplementationClassUid
        {
            get
            {
                return this.DvtkScriptSession.DvtSystemSettings.ImplementationClassUid;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.DvtSystemSettings.ImplementationClassUid = value;
            }
        }

        /// <summary>
        /// Gets or sets the implementation version name used by the DicomThread instance.
        /// </summary>
        public String LocalImplementationVersionName
        {
            get
            {
                return this.DvtkScriptSession.DvtSystemSettings.ImplementationVersionName;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.DvtSystemSettings.ImplementationVersionName = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of message fragment (P-DATA-TF PDU) that the 
        /// DicomThread instance can receive from the remote peer.
        /// </summary>
        public System.UInt32 LocalMaximumLength
        {
            get
            {
                return this.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived;
            }
            set
            {
                this.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = value;
            }
        }

        /// <summary>
        /// Gets or sets the port used by the DicomThread instance when listening for an incoming
        /// connection.
        /// </summary>
        public System.UInt16 LocalPort
        {
            get
            {
                return this.DvtkScriptSession.DvtSystemSettings.Port;
            }
            set
            {
                this.DvtkScriptSession.DvtSystemSettings.Port = value;
            }
        }

        /// <summary>
        /// Gets or sets the AE title used by the remote peer.
        /// </summary>
        public String RemoteAeTitle
        {
            get
            {
                return this.DvtkScriptSession.SutSystemSettings.AeTitle;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.SutSystemSettings.AeTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets the name that should be used when making a connection to the remote peer.
        /// It is best to enter the Internet Address of the SUT (in dot notation).
        /// </summary>
        public String RemoteHostName
        {
            get
            {
                return this.DvtkScriptSession.SutSystemSettings.HostName;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.SutSystemSettings.HostName = value;
            }
        }

        /// <summary>
        /// Gets or sets the implementation class UID used by the remote peer.
        /// </summary>
        public String RemoteImplementationClassUid
        {
            get
            {
                return this.DvtkScriptSession.SutSystemSettings.ImplementationClassUid;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.SutSystemSettings.ImplementationClassUid = value;
            }
        }

        /// <summary>
        /// Gets or sets the implementation version name used by the remote peer.
        /// </summary>
        public String RemoteImplementationVersionName
        {
            get
            {
                return this.DvtkScriptSession.SutSystemSettings.ImplementationVersionName;
            }
            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException();
                }

                this.DvtkScriptSession.SutSystemSettings.ImplementationVersionName = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of message fragment (P-DATA-TF PDU) that the 
        /// remote peer can receive from the DicomThread instance.
        /// </summary>
        public UInt32 RemoteMaximumLength
        {
            get
            {
                return this.DvtkScriptSession.SutSystemSettings.MaximumLengthReceived;
            }
            set
            {
                this.DvtkScriptSession.SutSystemSettings.MaximumLengthReceived = value;
            }
        }

        /// <summary>
        /// Gets or sets the port used when making a connection to a remote peer.
        /// </summary>
        public System.UInt16 RemotePort
        {
            get
            {
                return this.DvtkScriptSession.SutSystemSettings.Port;
            }
            set
            {
                this.DvtkScriptSession.SutSystemSettings.Port = value;
            }
        }

        /// <summary>
        /// Gets of sets the remote role.
        /// </summary>
        public Dvtk.Sessions.SutRole RemoteRole
        {
            get
            {
                return (DvtkScriptSession.SutSystemSettings.CommunicationRole);
            }
            set
            {
                DvtkScriptSession.SutSystemSettings.CommunicationRole = value;
            }
        }

		/// <summary>
		/// The directory in which the Dicom test scripts are present.
		/// </summary>
		public String ScriptDirectory
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.DicomScriptRootDirectory);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.DicomScriptRootDirectory = value;
			}
		}

        /// <summary>
        /// The time-out in seconds to wait for incoming data over a connected socket.
        /// </summary>
        public System.UInt16 SocketTimeout
        {
            get
            {
                return this.DvtkScriptSession.DvtSystemSettings.SocketTimeout;
            }
            set
            {
                this.DvtkScriptSession.DvtSystemSettings.SocketTimeout = value;
            }
        }

		/// <summary>
		/// The full file name of the stylesheet used to convert results .xml files to .html files.
		/// This stylesheet is used e.g. when the property ShowResults is set to true.
		/// 
		/// The default value for this property is the executable path append with the file name "DVT_RESULTS.xslt".
		/// </summary>
		public String StyleSheetFullFileName
		{
			get
			{
				return(this.styleSheetFullFileName);
			}
			set
			{
				this.styleSheetFullFileName = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if starting and stopping of the thread should be logged in the parent thread.
        /// </summary>
		public bool LogThreadStartingAndStoppingInParent
		{
			get
			{
				return(this.logThreadStartingAndStoppingInParent);
			}
			set
			{
				this.logThreadStartingAndStoppingInParent = value;
			}
		}


        /// <summary>
        /// Gets or sets a boolean indicating if a child thread overview should be logged when stopping the thread.
        /// </summary>
		public bool LogChildThreadsOverview
		{
			get
			{
				return(this.logChildThreadsOverview);
			}
			set
			{
				this.logChildThreadsOverview = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if incoming messages should be automatically validated.
        /// </summary>
		public bool AutoValidate
		{
			get
			{
				return(this.autoValidate);
			}
			set
			{
				this.autoValidate = value;
			}
		}

        /// <summary>
        /// Gets the summary full file name.
        /// </summary>
		public String SummaryResultsFullFileName
		{
			get
			{
				return(Path.Combine(ResultsDirectory, SummaryResultsFileNameOnly));
			}
		}

        /// <summary>
        /// Gets the summary file name only.
        /// </summary>
        public String SummaryResultsFileNameOnly
		{
			get
			{
				return("Summary_" + ResultsFileNameOnly);
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if waiting for child threads to complete should be logged.
        /// </summary>
	    public bool LogWaitingForCompletionChildThreads
	    {
		    get
		    {
			    return(this.logWaitingForCompletionChildThreads);
		    }
		    set
		    {
			    this.logWaitingForCompletionChildThreads = value;
		    }
	    }

        /// <summary>
        /// Gets a list of loaded definition files.
        /// </summary>
		public String[] LoadedDefinitionFileNames
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.DefinitionManagement.LoadedDefinitionFileNames);
			}
		}

        /// <summary>
        /// Gets the next results file name index.
        /// </summary>
		public int NextSubResultsFileNameIndex
		{
			get
			{
				return this.subResultsFileNameIndex++;
			}
		}

        /// <summary>
        /// Gets or sets the session ID.
        /// </summary>
		public override ushort SessionId
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.SessionId);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.SessionId = value;
			}
		}

		/// <summary>
		/// The directory in which the results file(s) will be written.
		/// </summary>
		public override String ResultsDirectory
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.ResultsRootDirectory);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.ResultsRootDirectory = value;
			}
		}

        /// <summary>
        /// Gets or sets the encapsulated DVTk ScriptSession instance.
        /// </summary>
        public Dvtk.Sessions.ScriptSession DvtkScriptSession
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession);
			}
			set
			{
				this.dicomThread.DvtkScriptSession = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if detailed results should be generated.
        /// </summary>
		public bool GenerateDetailedResults
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.DetailedValidationResults);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.DetailedValidationResults = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if detailed results should be generated.
        /// </summary>
        public bool GenerateSummary
        {
            get
            {
                return (this.dicomThread.DvtkScriptSession.SummaryValidationResults);
            }
            set
            {
                this.dicomThread.DvtkScriptSession.SummaryValidationResults = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if test log should be generated.
        /// </summary>
        public bool GenerateTestLog
        {
            get
            {
                return (this.dicomThread.DvtkScriptSession.TestLogValidationResults);
            }
            set
            {
                this.dicomThread.DvtkScriptSession.TestLogValidationResults = value;
            }
        }

		private static Object loadFromAndSaveToFileLock = new Object();

		/// <summary>
		/// Load the options from a .ses file.
		/// </summary>
		/// <param name="sessionFileName">The full file name.</param>
		public void LoadFromFile(String sessionFileName)
		{
			if (!File.Exists(sessionFileName))
			{
				throw new HliException("Script session file \"" + sessionFileName + "\" does not exist.");
			}

			// Because of the use of statics in Dvtk.Sessions.ScriptSession.LoadFromFile, only one thread at a time
			// should load a session file.
			lock(loadFromAndSaveToFileLock)
			{
				this.dicomThread.DvtkScriptSession = Dvtk.Sessions.ScriptSession.LoadFromFile(sessionFileName);
			}
		}

        /// <summary>
        /// Save the options to a .ses file.
        /// </summary>
        /// <param name="sessionFileName">The full file name.</param>
        public void SaveToFile(String sessionFileName)
		{
			lock(loadFromAndSaveToFileLock)
			{
                this.dicomThread.DvtkScriptSession.SessionFileName = sessionFileName;
				this.dicomThread.DvtkScriptSession.SaveToFile();
			}
		}
		
        /// <summary>
        /// Use property LocalPort instead.
        /// </summary>
        [Obsolete("Use property LocalPort instead.")]
		public Int32 DvtPort
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.DvtSystemSettings.Port);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.DvtSystemSettings.Port = Convert.ToUInt16(value);
			}
		}

        /// <summary>
        /// Use property RemotePort instead.
        /// </summary>
        [Obsolete("Use property RemotePort instead.")]
		public Int32 SutPort
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.SutSystemSettings.Port);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.SutSystemSettings.Port = Convert.ToUInt16(value);
			}
		}

        /// <summary>
        /// Use property RemoteHostName instead.
        /// </summary>
        [Obsolete("Use property RemoteHostName instead.")]
		public System.String SutIpAddress
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.SutSystemSettings.HostName);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.SutSystemSettings.HostName = value;
			}
		}

        /// <summary>
        /// Gets or sets the directory in which received DICOM messages will be stored as
        /// (pseudo) DICOM files.
        /// </summary>		
        public String DataDirectory
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.DataDirectory);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.DataDirectory = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if strict validation should be performed.
        /// </summary>
        public bool StrictValidation
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.StrictValidation);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.StrictValidation = value;
			}
		}

        /// <summary>
        /// Use property LocalAeTitle instead.
        /// </summary>
        [Obsolete("Use property LocalAeTitle instead.")]
		public String DvtAeTitle
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.DvtSystemSettings.AeTitle);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.DvtSystemSettings.AeTitle = value;
			}
		}

        /// <summary>
        /// Use property RemoteAeTitle instead.
        /// </summary>
        [Obsolete("Use property RemoteAeTitle instead.")]
		public String SutAeTitle
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.SutSystemSettings.AeTitle);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.SutSystemSettings.AeTitle = value;
			}
		}

        /// <summary>
        /// Gets or sets the storage mode.
        /// </summary>
		public Dvtk.Sessions.StorageMode StorageMode
		{
			get
			{
				return(this.dicomThread.DvtkScriptSession.StorageMode);
			}
			set
			{
				this.dicomThread.DvtkScriptSession.StorageMode = value;
			}
		}

		private bool logWaitingForCompletionChildThreads = true;

        /// <summary>
        /// Load a definition file.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns>Boolean indicating of the call succeeded.</returns>
		public bool LoadDefinitionFile(System.String filename)
		{
			return this.dicomThread.DvtkScriptSession.DefinitionManagement.LoadDefinitionFile(filename);
		}

        /// <summary>
        /// Unload a definition file.
        /// </summary>
        /// <param name="filename">The file name.</param>
        /// <returns>Boolean indicating of the call succeeded.</returns>
		public bool UnLoadDefinitionFile(System.String filename)
		{
			return this.dicomThread.DvtkScriptSession.DefinitionManagement.UnLoadDefinitionFile(filename);
		}
		
        /// <summary>
        /// Gets or sets a boolean indicating if secure sockets is enabled.
        /// </summary>
		public bool SecureConnection
		{
			get
			{
				return this.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled;
			}
			set
			{
				this.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = value;
				if (this.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled == true)
				{
					this.DvtkScriptSession.SecuritySettings.ApplyChangedSocketParameters();
				}
			}
		}

        /// <summary>
        /// Gets or sets the credentials file name.
        /// </summary>
		public System.String CredentialsFilename
		{
			get
			{
				return this.DvtkScriptSession.SecuritySettings.CredentialsFileName;
			}
			set
			{
				this.DvtkScriptSession.SecuritySettings.CredentialsFileName = value;
			}
		}

        /// <summary>
        /// Gets or sets the certificate file name.
        /// </summary>
		public System.String CertificateFilename
		{
			get
			{
				return this.DvtkScriptSession.SecuritySettings.CertificateFileName;
			}
			set
			{
				this.DvtkScriptSession.SecuritySettings.CertificateFileName = value;
			}
		}

        /// <summary>
        /// Copy the options from another instance.
        /// </summary>
        /// <param name="dicomThreadOptions">The other instance to copy from.</param>
		public void CopyFrom(DicomThreadOptions dicomThreadOptions)
		{
			base.CopyFrom(dicomThreadOptions);

			// Copy all settings from the encapsulated DvtkScriptSession.
			DvtkScriptSession.CopySettingsFrom(dicomThreadOptions.DvtkScriptSession);

			// Copy all other settings not contained in the Dvtk ScriptSession object that are contained in the DicomThreadOptions class.
			AutoValidate = dicomThreadOptions.autoValidate;
			LogChildThreadsOverview = dicomThreadOptions.LogChildThreadsOverview;
			LogThreadStartingAndStoppingInParent = dicomThreadOptions.LogThreadStartingAndStoppingInParent;
			LogWaitingForCompletionChildThreads = dicomThreadOptions.LogWaitingForCompletionChildThreads;
			ShowResults = dicomThreadOptions.ShowResults;
            ShowTestLog = dicomThreadOptions.ShowTestLog;
			StyleSheetFullFileName = dicomThreadOptions.StyleSheetFullFileName;
		}

        /// <summary>
        /// Copy the options from another instance.
        /// </summary>
        /// <param name="dicomThreadOptions">The other instance to copy from.</param>
        public void DeepCopyFrom(DicomThreadOptions dicomThreadOptions)
		{
			CopyFrom(dicomThreadOptions);
		}

        /// <summary>
        /// Gets or sets a boolean indicating if the detail results should be shown after the DicomThread instance stops.
        /// </summary>
		public override bool ShowResults
		{
			get
			{
				return(this.showResults);
			}
			set
			{
				this.showResults = value;

				if (this.showResults)
				{
					this.DvtkScriptSession.DetailedValidationResults = true;
				}
			}
		}

        public override bool ShowSummary
        {
            get
            {
                return (this.showSummary);
            }
            set
            {
                this.showSummary = value;

                if (this.showSummary)
                {
                    this.DvtkScriptSession.SummaryValidationResults = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the detail results should be shown after the DicomThread instance stops.
        /// </summary>
        public override bool ShowTestLog
        {
            get
            {
                return (this.showTestLog);
            }
            set
            {
                this.showTestLog = value;

                if (this.showTestLog)
                {
                    this.DvtkScriptSession.TestLogValidationResults = true;
                }
            }
        }

	}
}
