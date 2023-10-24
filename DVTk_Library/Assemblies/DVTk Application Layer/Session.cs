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
using System.Windows.Forms;
using DvtkSession = Dvtk.Sessions;
using System.Diagnostics;


namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for Session.
    /// </summary>
    public abstract class Session
    {
        /// <summary>
        /// 
        /// </summary>
        public enum SessionType
        {
            /// <summary>
            /// Unknown
            /// </summary>
            ST_UNKNOWN,

            /// <summary>
            /// Script Session
            /// </summary>
            ST_SCRIPT,

            /// <summary>
            /// Emulator Session
            /// </summary>
            ST_EMULATOR,

            /// <summary>
            /// Media Session
            /// </summary>
            ST_MEDIA
        }

        /// <summary>
        /// 
        /// </summary>
        public enum LogLevelFlags : uint
        { // System.UInt32
            /// <summary>
            /// 
            /// </summary>
            None = 1 << 0,            // 0x00000001
            /// <summary>
            /// 
            /// </summary>
            Error = 1 << 1,           // 0x00000002
            /// <summary>
            /// 
            /// </summary>
            Debug = 1 << 2,           // 0x00000004
            /// <summary>
            /// 
            /// </summary>
            Warning = 1 << 3,         // 0x00000008
            /// <summary>
            /// 
            /// </summary>
            Info = 1 << 4,            // 0x00000010
            /// <summary>
            /// 
            /// </summary>
            Script = 1 << 5,          // 0x00000020
            /// <summary>
            /// 
            /// </summary>
            ScriptName = 1 << 6,      // 0x00000040
            /// <summary>
            /// 
            /// </summary>
            PduBytes = 1 << 7,        // 0x00000080
            /// <summary>
            /// 
            /// </summary>
            DulpFsm = 1 << 8,         // 0x00000100
            /// <summary>
            /// 
            /// </summary>
            ImageRelation = 1 << 9,   // 0x00000200
            /// <summary>
            /// 
            /// </summary>
            Print = 1 << 10,          // 0x00000400
            /// <summary>
            /// 
            /// </summary>
            Label = 1 << 11,          // 0x00000800
            /// <summary>
            /// 
            /// </summary>
            MediaFilename = 1 << 12,  // 0x00001000
        }

        /// <summary>
        /// 
        /// </summary>
        public enum StorageMode
        {
            /// <summary>
            /// 
            /// </summary>
            AsMedia,
            /// <summary>
            /// 
            /// </summary>
            AsMediaOnly,
            /// <summary>
            /// 
            /// </summary>
            AsDataSet,
            /// <summary>
            /// 
            /// </summary>
            TemporaryPixelOnly,
            /// <summary>
            /// 
            /// </summary>
            NoStorage
        }

        #region Protected Fields

        private SessionType sessionType;
        private DvtkSession.Session implementation = null;
        private string sessionFileName;

        /// <summary>
        /// 
        /// </summary>
        protected bool isLoaded = false;

        private string baseLocation;
        private bool result = false;
        private ArrayList summaryFiles = new ArrayList();
        private ArrayList detailFiles = new ArrayList();
        private string resultsRootDirectory;
        //private string descriptionDirectory;        
        private string definitionRootDirectory;
        private DateTime date = DateTime.Today;
        private string sessionTitle = "";
        private UInt16 sessionId = 0;
        private string softwareVersions = "2.1.001";
        private string testedBy = "DVT";
        private string manufacturer = "DVT";
        private string modelname = "DVT";
        private Boolean autoCreateDirectory = true;
        private Boolean continueOnError = true;
        private StorageMode storageMode = StorageMode.AsMedia;
        private int logLevelFlagsMask = (int)(LogLevelFlags.Error | LogLevelFlags.Warning | LogLevelFlags.Info);
        private IList results = new ArrayList();
        private UInt16 nrOfErrors = 0;
        private UInt16 nrOfWarnings = 0;
        private bool optionVerbose = false;
        private bool detailedValidationResults = true;
        private bool testLogValidationResults = false;
        private bool summaryValidationResults = true;
        private bool isExecuting = false;
        private bool backUpResults = true;
        private string dataDirectory;

        /// <summary>
        /// 
        /// </summary>
        public DvtkSession.Session tempSession;

        private bool hasSessionchanged = false;
        private Project parentProject = null;
        private SecuritySettings securitySettings = new SecuritySettings();
        private bool validateReferencedFile = true;
        private bool displayConditionText = true;

        #endregion

        #region Constructors

        static Session()
        {
            Dvtk.Setup.Initialize();
        }
        /// <summary>
        /// Constructor
        /// </summary>

        public Session()
        {
            sessionFileName = null;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">Represnts the session file name.</param>
        public Session(string fileName)
        {
            sessionFileName = fileName;
        }
        /// <summary>
        /// Method to Terminate the session.
        /// </summary>
        public static void TerminateSession()
        {
            Dvtk.Setup.Terminate();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Property that determines the project to which the session belongs.
        /// </summary>
        public Project ParentProject
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return parentProject;
            }
            set
            {
                parentProject = value;
            }
        }

        /// <summary>
        /// Property that represents the security setting class for a session.
        /// </summary>
        public SecuritySettings SecuritySetting
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return securitySettings;
            }
            set
            {
                securitySettings = value;
            }
        }

        /// <summary>
        /// Property to access the Dvtk.Sessions.Session
        /// </summary>
        public DvtkSession.Session Implementation
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return implementation;
            }
            set
            {
                implementation = value;
            }
        }

        /// <summary>
        /// Collection of Results for a particular session.
        /// </summary>
        public IList Results
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return results;
            }
            set
            {
                results = value;
            }
        }
        /// <summary>
        /// Identification for the session.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        public UInt16 SessionId
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sessionId;
            }
            set
            {
                sessionId = value;
                hasSessionchanged = true;
            }
        }

        /// <summary>
        /// Directory used to store results output.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">Directory may not be an empty string. Use ".\" for current directory."</exception>
        public String ResultsRootDirectory
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return resultsRootDirectory;
            }
            set
            {
                resultsRootDirectory = value;
                hasSessionchanged = true;
            }
        }

        /// <summary>
        /// Property that represents the Definition directory of a session.
        /// </summary>
        public String DefinitionRootDirectory
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return definitionRootDirectory;
            }
            set
            {
                definitionRootDirectory = value;
                hasSessionchanged = true;
            }
        }

        /// <summary>
        /// Date of test.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>                    
        public DateTime Date
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return date;
            }

            set
            {
                date = value;
                hasSessionchanged = true;
            }
        }

        /// <summary>
        /// Title for the session.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        public String SessionTitle
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sessionTitle;
            }
            set
            {
                sessionTitle = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Software versions for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>

        public String SoftwareVersions
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return softwareVersions;
            }
            set
            {
                softwareVersions = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Name of the tester.
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>

        public String TestedBy
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return testedBy;
            }

            set
            {
                testedBy = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// If the storage mode is set to as-media, 
        /// DVT will store a received Image Dataset (Group 0008 up to and including Group 7FE0) 
        /// in a file with the extension .DCM. 
        /// </summary>
        /// <remarks>
        /// <p>
        /// The Image Dataset is stored in the .DCM file in the format 
        /// described in DICOM - part 10. 
        /// </p>
        /// <p>
        /// The File Preamble, DICOM Prefix and File Meta Information are added by DVT.
        /// </p>
        /// <p>
        /// The filename is generated from the Session ID and a media storage file index. 
        /// The filename used for the media storage is recorded in the corresponding Results File. 
        /// The following filenames are generated:
        /// <c>nnnIiiii.DCM</c> where <c>nnn</c> is the Session ID, 
        /// <c>I</c> signifies image information and <c>iiii</c> is the file index.
        /// </p>
        /// Examples:
        /// <list type="bullet">
        /// <item>
        /// <term>1I0123.DCM</term>
        /// <description>Media Storage File 123 created in Test Session 1.</description>
        /// </item>
        /// <item>
        /// <term>4I0012.DCM</term>
        /// <description>Media Storage File 12 created in Test Session 4.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public StorageMode Mode
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return storageMode;
            }
            set
            {
                storageMode = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Manufacturer for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        public String Manufacturer
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return manufacturer;
            }

            set
            {
                manufacturer = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Model name for the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Reported in the results output.
        /// </remarks>
        public String ModelName
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return modelname;
            }

            set
            {
                modelname = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Proerty that set and get the LogLevelFlags for a session.
        /// </summary>           
        public int LogLevelMask
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return logLevelFlagsMask;
            }

            set
            {
                logLevelFlagsMask = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Determines if a directory is created when this does not yet existing, when writing a Dicom file.
        /// </summary>         
        public Boolean AutoCreateDirectory
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return autoCreateDirectory;
            }
            set
            {
                autoCreateDirectory = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Determines of execution should continue when an error has occured. 
        /// </summary>
        public Boolean ContinueOnError
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return continueOnError;
            }

            set
            {
                continueOnError = value;
                hasSessionchanged = true;
            }
        }
        /// <summary>
        /// Determines the Base Location Of the Project . 
        /// </summary>
        public string BaseLocation
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return baseLocation;
            }
            set
            {
                baseLocation = value;
            }
        }
        /// <summary>
        /// Determine the execution of the Session. 
        /// </summary>
        public bool Result
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return result;
            }
            set
            {
                result = value;
            }
        }
        /// <summary>
        ///  Number of Errors.
        /// </summary>

        public System.UInt32 NrOfErrors
        {
            get
            {
                return implementation.NrOfErrors;
            }
        }
        /// <summary>
        ///  Number of Warnings.
        /// </summary>
        public System.UInt32 NrOfWarnings
        {
            get
            {
                return implementation.NrOfWarnings;
            }
        }
        #endregion

        /// <summary>
        /// Determine the Session Type.
        /// </summary>
        public SessionType Type
        {
            get
            {
                //                if(!isLoaded) {
                //                    LoadSession();
                //                }
                return sessionType;
            }
            set
            {
                sessionType = value;
            }
        }

        /// <summary>
        /// File name with extension <c>.ses</c> used during load and save.
        /// </summary>
        public string SessionFileName
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sessionFileName;
            }
            set
            {
                sessionFileName = value;
            }
        }

        /// <summary>
        /// Determine the existence of BackUpFiles
        /// </summary>
        public bool BackUpFiles
        {
            get
            {
                return backUpResults;
            }
            set
            {
                backUpResults = value;
            }
        }

        /// <summary>
        /// Determine Verbose On/Off
        /// </summary>
        public bool OptionVerbose
        {
            get
            {
                return optionVerbose;
            }
            set
            {
                optionVerbose = value;
            }
        }

        /// <summary>
        /// Directory used to store data generated by DVT - DCM files, etc.
        /// </summary>
        public System.String DataDirectory
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return dataDirectory;
            }
            set
            {
                dataDirectory = value;
                hasSessionchanged = true;
            }
        }

        /// <summary>
        /// Determines whether private attribute tags will be mapped to an internal value or not.
        /// </summary>
        public bool UsePrivateAttributeMapping
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return Wrappers.MBaseSession.UsePrivateAttributeMapping;
            }
            set
            {
                Wrappers.MBaseSession.UsePrivateAttributeMapping = value;
            }
        }

        /// <summary>
        /// Determines whether the detailed validation results should be generated while executing.
        /// </summary>
        public bool DetailedValidationResults
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return detailedValidationResults;
            }
            set
            {
                detailedValidationResults = value;
            }
        }

        /// <summary>
        /// Determines whether the test log validation results should be generated while executing.
        /// </summary>
        public bool TestLogValidationResults
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return testLogValidationResults;
            }
            set
            {
                testLogValidationResults = value;
            }
        }

        /// <summary>
        /// Determine whether the current session is Executing.
        /// </summary>
        public bool IsExecute
        {
            get
            {
                return isExecuting;
            }
            set
            {
                isExecuting = value;
            }
        }

        /// <summary>
        /// Property to determine whether the session has been changed
        /// </summary>
        public bool HasSessionChanged
        {
            get
            {
                return hasSessionchanged;
            }
            set
            {
                hasSessionchanged = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean ValidateReferencedFile
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return validateReferencedFile;
            }
            set
            {
                validateReferencedFile = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean DisplayConditionText
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return displayConditionText;

            }
            set
            {
                displayConditionText = value;
            }
        }

        #region Abstract Methods    
        /// <summary>
        /// Method for executing a session in a synchronous manner.
        /// </summary>
        /// <param name="baseInput">baseInput Object </param>
        /// <returns></returns>
        public abstract Result Execute(BaseInput baseInput);
        /// <summary>
        /// Method for executing a session in an Asynchronous manner.
        /// </summary>
        /// <param name="baseInput">baseInput Object</param>
        public abstract void BeginExecute(BaseInput baseInput);

        #endregion

        #region Public Methods
        /// <summary>
        /// set and save session settings to file with extension <c>.ses</c>.
        /// </summary>
        public virtual bool Save()
        {
            return implementation.SaveToFile();
        }

        /// <summary>
        /// Method to create result file name.
        /// </summary>
        /// <param name="filename">baseName of the executed thing (i.e mediaFile , script , emulator) </param>
        /// <returns></returns>
        public string CreateResultFileName(string filename)
        {
            string date = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return sessionId.ToString("000") + "_" + filename + "_res.xml";
        }

        /// <summary>
        /// Method to get the basename for a resultFile.
        /// </summary>
        /// <param name="theResultsFileName"> FullName of a resultFile.</param>
        /// <returns></returns>
        public string GetBaseNameNoCheck(string theResultsFileName)
        {
            string theBaseName = theResultsFileName;

            // Remove first prefix.
            theBaseName = theBaseName.Substring(theBaseName.IndexOf("_") + 1);

            // Remove second prefix: session ID and '_'.
            theBaseName = theBaseName.Substring(theBaseName.IndexOf("_") + 1);

            // Remove postfix.
            int indexOfLastUnderline = theBaseName.LastIndexOf("_");
            theBaseName = theBaseName.Substring(0, indexOfLastUnderline);

            return (theBaseName);
        }

        /// <summary>
        /// Method to get the sessionId of the session from a resultFile.
        /// </summary>
        /// <param name="theResultsName">FullName of a resultFile</param>
        /// <returns></returns>
        public string GetSessionId(string theResultsName)
        {
            string theSessionId = theResultsName;

            // Remove first prefix.
            theSessionId = theSessionId.Substring(theSessionId.IndexOf("_") + 1);
            theSessionId = theSessionId.Substring(0, 3);
            return theSessionId;
        }

        /// <summary>
        /// Save the session.
        /// </summary>
        /// <param name="theSessionFileName">File name of the session.</param>
        public void SaveSession(string theSessionFileName)
        {
            foreach (Session theSession in parentProject.Sessions)
            {
                // If this is the session corresponding to the supplied session file naam, save it.
                if (theSession.SessionFileName == theSessionFileName)
                {
                    bool isSaved = false;

                    if ((File.GetAttributes(theSessionFileName) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                    {
                        isSaved = theSession.Save();
                        if (!isSaved)
                        {
                            MessageBox.Show(string.Format("File \"{0}\" is not saved successfully.", theSessionFileName));
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("File \"{0}\" is not saved successfully.\nIt is Read only", theSessionFileName));
                    }

                    if (isSaved)
                    {
                        theSession.HasSessionChanged = false;
                    }
                }
            }
        }

        /// <summary>
        /// Save the session under a new file name.
        /// 
        /// A new session object will be created from this new saved file (and returned by this method) 
        /// and added to the project. The original session wil not be saved.
        /// </summary>
        /// <param name="theCurrentSession"></param>
        /// <returns>The new created session object, null if save as a new session has been cancelled or failed.</returns>
        public Session SaveSessionAs(Session theCurrentSession)
        {
            Session theNewSession = null;

            SaveFileDialog theSaveFileDialog = new SaveFileDialog();
            theSaveFileDialog.AddExtension = true;
            theSaveFileDialog.DefaultExt = ".ses";
            theSaveFileDialog.OverwritePrompt = false;
            theSaveFileDialog.Filter = "All session files (*.ses)|*.ses";

            DialogResult theDialogResult = theSaveFileDialog.ShowDialog();

            // User has specified a file name and has pressed the OK button.
            if (theDialogResult == DialogResult.OK)
            {
                if (File.Exists(theSaveFileDialog.FileName))
                {
                    MessageBox.Show(string.Format("File name \"{0}\" already exists.\nOperation cancelled", theSaveFileDialog.FileName));
                }
                else
                {
                    // Save the current session to a new file.
                    string theCurrentSessionFullFileName = theCurrentSession.SessionFileName;

                    theCurrentSession.SessionFileName = theSaveFileDialog.FileName;
                    theCurrentSession.Save();
                    Session.SessionType sessionType = theCurrentSession.sessionType;
                    // Create a new session object from this new saved file and replace the current session.
                    switch (sessionType)
                    {
                        case Session.SessionType.ST_MEDIA:
                            theNewSession = new MediaSession(theSaveFileDialog.FileName);
                            LoadSession();
                            break;
                        case Session.SessionType.ST_SCRIPT:
                            theNewSession = new ScriptSession(theSaveFileDialog.FileName);
                            LoadSession();
                            break;
                        case Session.SessionType.ST_EMULATOR:
                            theNewSession = new EmulatorSession(theSaveFileDialog.FileName);
                            LoadSession();
                            break;
                        case Session.SessionType.ST_UNKNOWN:
                            break;
                    }

                    // Create a new session object from this new saved file and replace the current session.
                    if (theNewSession != null)
                    {
                        int theCurrentIndex = GetLoadedSessionsIndex(theCurrentSession);
                        parentProject.Sessions[theCurrentIndex] = theNewSession;
                        parentProject.HasProjectChanged = true;
                    }
                }
            }

            return (theNewSession);
        }

        /// <summary>
        /// Method to set the bool hasSessionchanged 
        /// </summary>
        /// <param name="theSession"> Represents the session name.</param>
        /// <param name="changed">boolean value that determines whether the session has been changed.</param>
        public void SetSessionChanged(Session theSession, bool changed)
        {
            if (theSession == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                theSession.HasSessionChanged = changed;
            }
        }

        /// <summary>
        /// Method to determine the number of sessions in a project.
        /// </summary>
        /// <returns></returns>
        public int GetNrSessions()
        {
            return parentProject.Sessions.Count;
        }

        /// <summary>
        /// Method to get the Session Information from the Collection of sessions with a particular index.
        /// </summary>
        /// <param name="index"> Index number of a session in a collection of sessions.</param>
        /// <returns></returns>
        public Session GetSession(int index)
        {
            return (GetSessionInformation(index));
        }

        /// <summary>
        /// Method to get the value of bool hasSessionchanged.
        /// </summary>
        /// <param name="theSession">Represents the session name.</param>
        /// <returns></returns>
        public bool GetSessionChanged(Session theSession)
        {
            bool theReturnValue = false;
            if (theSession == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                theReturnValue = theSession.HasSessionChanged;
            }

            return (theReturnValue);
        }

        /// <summary>
        /// Method to remove a session from a collection of sessions.
        /// </summary>
        /// <param name="theSession">Represents the session name.</param>
        public void RemoveSession(Session theSession)
        {
            if (theSession == null)
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                ParentProject.Sessions.Remove(theSession);
                ParentProject.HasProjectChanged = true;

            }
        }

        # endregion

        #region Protected Methods
        /// <summary>
        /// Method to Determine the detail name of a resultFile.
        /// </summary>
        /// <param name="name">it represents the string whose detailed file needs to be determined. </param>
        /// <returns></returns>
        protected string DetermineDetailName(string name)
        {
            string fileName = name;
            int index = fileName.IndexOf("{DS}");
            if (index < 0)
            {
                fileName += "D";
            }
            else
            {
                fileName = fileName.Substring(0, index) + "D" + fileName.Substring(index + 4);
            }
            int subIndex = fileName.IndexOf("{S}");
            if (subIndex > 0)
            {
                fileName = fileName.Substring(0, subIndex) + fileName.Substring(subIndex + 5);
            }
            return fileName;
        }

        /// <summary>
        /// Method to Determine the summary name of a resultFile.
        /// </summary>
        /// <param name="name">Tt represents the string whose summary file needs to be determined. </param>
        /// <returns></returns>
        protected string DetermineSummaryName(string name)
        {
            string fileName = name;
            int index = fileName.IndexOf("{DS}");
            if (index < 0)
            {
                fileName += "S";
            }
            else
            {
                fileName = fileName.Substring(0, index) + "S" + fileName.Substring(index + 4);
            }
            int subIndex = fileName.IndexOf("{S}");
            if (subIndex > 0)
            {
                fileName = fileName.Substring(0, subIndex) + fileName.Substring(subIndex + 5);
            }
            return fileName;
        }

        /// <summary>
        /// Method to create result object .
        /// </summary>
        /// <param name="name">Full Name of the ResultFile.</param>
        /// <returns></returns>
        protected Result CreateResults(string name)
        {
            Result result = new Result(
                nrOfErrors,
                nrOfWarnings,
                resultsRootDirectory + DetermineDetailName(name),
                resultsRootDirectory + DetermineSummaryName(name),
                GetFileNames(implementation, resultsRootDirectory + DetermineDetailName(name))
                );
            results.Add(result);
            return result;
        }

        /// <summary>
        /// Load a session from file.
        /// </summary>
        protected void LoadSession()
        {
            FileInfo sessionFileInfo = new FileInfo(sessionFileName);

            if (!sessionFileInfo.Exists)
            {
                sessionFileInfo = new FileInfo(
                    baseLocation + sessionFileName
                    );

                if (!sessionFileInfo.Exists)
                {
                    isLoaded = false;
                    return;
                }
                else
                {
                    sessionFileName = baseLocation + sessionFileName;
                }
            }

            try
            {
                implementation =
                    DvtkSession.SessionFactory.TheInstance.Load(sessionFileName);
                isLoaded = true;
                GetAllProperties();
                if (resultsRootDirectory.IndexOf("#") > 0)
                {
                    MessageBox.Show("Result directory contains '#' which may cause undesired results.\nThe session directory will be set as new result directory.", "Warning");
                    FileInfo fileinfo = new FileInfo(implementation.SessionFileName);
                    implementation.ResultsRootDirectory = fileinfo.DirectoryName;
                    implementation.SaveToFile();
                    implementation =
                        DvtkSession.SessionFactory.TheInstance.Load(sessionFileName);
                    isLoaded = true;
                    resultsRootDirectory = fileinfo.DirectoryName;

                }
            }
            catch (Exception e)
            {
                string msg = string.Empty;
                msg += string.Format("Failed to load session file: {0}.\n", sessionFileName);
                msg += e.Message;
                implementation = null;
                isLoaded = false;
                Exception excp;
                throw excp = new Exception(msg);
            }
        }

        /// <summary>
        /// Method to create the instance of a session.
        /// </summary>
        protected virtual void CreateSessionInstance() { }
        /// <summary>
        /// Method to create the instance of a session. 
        /// </summary>
        /// <param name="sessionFileName"> FileName of the session.</param>
        protected virtual void CreateSessionInstance(string sessionFileName) { }
        /// <summary>
        /// Occurs when a activity report is generated by the application.
        /// </summary>
        /// <remarks>
        /// Users may register a callback to listen for this event.
        /// </remarks>
        protected static void ActivityReportEventHandler(object sender, Dvtk.Events.ActivityReportEventArgs e)
        {
            // format -15 equals left-alignment with a preferred width of 15 characters.
            System.Console.WriteLine(
                string.Format("{0,-15}: {1}", e.ReportLevel, e.Message));
        }

        /// <summary>
        /// Method to get the Session Properties from a loaded session.
        /// </summary>
        protected virtual void GetAllProperties()
        {
            sessionFileName = implementation.SessionFileName;
            resultsRootDirectory = implementation.ResultsRootDirectory;
            definitionRootDirectory =
                implementation.DefinitionManagement.DefinitionFileRootDirectory;
            //descriptionDirectory = implementation.DescriptionDirectory;
            date = implementation.Date;
            sessionTitle = implementation.SessionTitle;
            sessionId = implementation.SessionId; ;
            softwareVersions = implementation.SoftwareVersions;
            testedBy = implementation.TestedBy;
            manufacturer = implementation.Manufacturer;
            modelname = implementation.ModelName;
            autoCreateDirectory = implementation.AutoCreateDirectory;
            continueOnError = implementation.ContinueOnError;
            logLevelFlagsMask = (int)implementation.LogLevelFlags;
            storageMode = Convert(implementation.StorageMode);
            dataDirectory = implementation.DataDirectory;
            validateReferencedFile = implementation.ValidateReferencedFile;
            displayConditionText = implementation.DisplayConditionText;
            detailedValidationResults = implementation.DetailedValidationResults;
            testLogValidationResults = implementation.TestLogValidationResults;
            summaryValidationResults = implementation.SummaryValidationResults;
        }

        /// <summary>
        /// Method to set the values of Session Properties in a Session.
        /// </summary>
        public virtual void SetAllProperties()
        {
            implementation.SessionFileName = sessionFileName;
            implementation.ResultsRootDirectory = resultsRootDirectory;
            if (dataDirectory != null)
            {
                implementation.DataDirectory = dataDirectory;
            }
            //implementation.DescriptionDirectory = descriptionDirectory;
            implementation.StorageMode = (DvtkSession.StorageMode)storageMode;
            implementation.Date = date;
            implementation.SessionTitle = sessionTitle;
            implementation.SessionId = sessionId;
            implementation.SoftwareVersions = softwareVersions;
            implementation.TestedBy = testedBy;
            implementation.Manufacturer = manufacturer;
            implementation.ModelName = modelname;
            implementation.AutoCreateDirectory = autoCreateDirectory;
            implementation.ContinueOnError = continueOnError;
            implementation.LogLevelFlags = (DvtkSession.LogLevelFlags)logLevelFlagsMask;
            implementation.ValidateReferencedFile = validateReferencedFile;
            implementation.DisplayConditionText = displayConditionText;
            implementation.DetailedValidationResults = detailedValidationResults;
            implementation.TestLogValidationResults = testLogValidationResults;
            implementation.SummaryValidationResults = summaryValidationResults;
        }

        /// <summary>
        /// Get the base names from a list of results file names.
        /// </summary>
        /// <returns>The resultFiles for a session.</returns>
        protected ArrayList GetFileNamesforSession()
        {
            ArrayList resultsFiles = new ArrayList();
            DirectoryInfo directoryInfo;
            FileInfo[] filesInfo;
            directoryInfo = new DirectoryInfo(implementation.ResultsRootDirectory);
            if (directoryInfo.Exists)
            {
                filesInfo = directoryInfo.GetFiles("*.xml");
                foreach (FileInfo fileInfo in filesInfo)
                {
                    string theResultsFileName = fileInfo.Name;

                    if (DvtkApplicationLayer.Result.IsValid(theResultsFileName))
                    {
                        resultsFiles.Add(theResultsFileName);
                    }
                }
            }
            return resultsFiles;
        }
        /// <summary>
        /// Get the base names from a list of results file names.
        /// </summary>
        /// <param name="theResultsFiles">The results file names.</param>
        /// <returns>The unique base names.</returns>
        protected ArrayList GetBaseNamesForResultsFiles(ArrayList theResultsFiles)
        {
            ArrayList theBaseNames = new ArrayList();

            foreach (string theResultsFile in theResultsFiles)
            {
                string theBaseName = GetBaseNameNoCheck(theResultsFile);

                if (!theBaseNames.Contains(theBaseName))
                {
                    theBaseNames.Add(theBaseName);
                }
            }

            theBaseNames.Sort();

            return (theBaseNames);
        }

        #endregion

        #region Private Methods

        private StorageMode Convert(DvtkSession.StorageMode value)
        {
            switch (value)
            {
                case DvtkSession.StorageMode.AsDataSet:
                    return StorageMode.AsDataSet;
                case DvtkSession.StorageMode.AsMedia:
                    return StorageMode.AsMedia;
                case DvtkSession.StorageMode.AsMediaOnly:
                    return StorageMode.AsMediaOnly;
                case DvtkSession.StorageMode.NoStorage:
                    return StorageMode.NoStorage;
                case DvtkSession.StorageMode.TemporaryPixelOnly:
                    return StorageMode.TemporaryPixelOnly;
                default:
                    throw new System.NotSupportedException();
            }
        }

        private ArrayList GetFileNames(Dvtk.Sessions.Session session, string detailFile)
        {
            string resultFileName = System.IO.Path.GetFileNameWithoutExtension(detailFile);
            ArrayList resultsFiles = new ArrayList();
            DirectoryInfo directoryInfo;
            FileInfo[] filesInfo;
            directoryInfo = new DirectoryInfo(session.ResultsRootDirectory);
            if (directoryInfo.Exists)
            {
                filesInfo = directoryInfo.GetFiles(
                    resultFileName.Substring(0, resultFileName.Length - 3) + "*.xml");
                foreach (FileInfo fileInfo in filesInfo)
                {
                    resultsFiles.Add(fileInfo.Name);
                }
            }
            return resultsFiles;
        }


        private int GetLoadedSessionsIndex(Session theSession)
        {
            int theReturnValue = -1;

            for (int theIndex = 0; theIndex < parentProject.Sessions.Count; theIndex++)
            {
                Session tempSession = (Session)ParentProject.Sessions[theIndex];
                if (tempSession == theSession)
                {
                    theReturnValue = theIndex;
                    break;
                }
            }
            return (theReturnValue);
        }


        private Session GetSessionInformation(Session theSession)
        {
            Session session = null;

            int theIndex = GetLoadedSessionsIndex(theSession);

            if (theIndex != -1)
            {
                session = GetSessionInformation(theIndex);
            }

            return (session);
        }

        private Session GetSessionInformation(int theIndex)
        {
            Session session = null;

            if ((theIndex < 0) || (theIndex >= ParentProject.Sessions.Count))
            {
                // Sanity check.
                Debug.Assert(false);
            }
            else
            {
                session = (Session)(ParentProject.Sessions[theIndex]);
            }

            return (session);
        }

        #endregion
    }
}


