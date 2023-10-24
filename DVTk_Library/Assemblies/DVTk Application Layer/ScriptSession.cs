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
using System.Threading;
using DvtkSession = Dvtk.Sessions;
using System.Collections;

namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for ScriptSession.
    /// </summary>
    public class ScriptSession : Session
    {
        # region Private members

        private string dvtAeTitle = "DVT_AE";
        private UInt16 dvtPort = 104;
        private UInt16 dvtSocketTimeOut = 90;
        private UInt32 dvtMaximumLengthReceived = 16384;
        private string sutAeTitle = "SUT_AE";
        private UInt16 sutPort = 104;
        private String sutHostName = "localhost";
        private UInt32 sutMaximumLengthReceived = 16384;
        private string dicomScriptRootDirectory;
        private string descriptionDirectory = "";
        private Boolean secureSocketsEnabled;
        private string sutimplementationVersionName = "DVT 2.1";
        private string sutimplementationClassUid = "";
        private string dvtimplementationVersionName = "";
        private string dvtimplementationClassUid = "";
        private AsyncCallback callBackDelegate = null;
        private Thread scriptThread = null;
        private string scriptFileName = "";
        private IList scriptFiles = null;
        private bool addGroupLength = false;
        private bool defineSqLength = false;

        # endregion

        # region Properties
        /// <summary>
        /// The DVT AE Title is the application entity name of the DVT machine in the test.
        /// </summary>
        public string DvtAeTitle
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return dvtAeTitle;
            }
            set
            {
                dvtAeTitle = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The port that the Dicom Validation Tool (DVT) should use when making a connection to 
        /// System Under Test (SUT).
        /// </summary>
        public UInt16 DvtPort
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return dvtPort;
            }
            set
            {
                dvtPort = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The period that the Dicom Validation Tool (DVT) will listen for incomming messages
        /// on the TCP/IP connection before automatically aborting the session.
        /// </summary>
        public UInt16 DvtSocketTimeout
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return dvtSocketTimeOut;
            }
            set
            {
                dvtSocketTimeOut = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The maximum length of message fragment (P-DATA-TF PDU) 
        /// that the Dicom Validation Tool (DVT) can receive from the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// DICOM DIMSE-messages are split into P-DATA-TF PDU fragments - e.g., C-STORE-RQ of a modality image.
        /// </remarks>
        public UInt32 DvtMaximumLengthReceived
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return dvtMaximumLengthReceived;
            }
            set
            {
                dvtMaximumLengthReceived = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// This is the implementation version name for the Dicom Validation Tool (DVT) implementation.
        /// </summary>
        /// <remarks>
        /// The version is composed of the folloqing items<br></br>
        /// [dvt][version_major].[version_minor]<br></br>
        /// <c>dvtx.x</c>
        /// </remarks>
        public String DvtImplementationVersionName
        {
            get
            {
                return dvtimplementationVersionName;
            }
            set
            {
                dvtimplementationVersionName = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// This is the unique identifier (UID) for the Dicom Validation Tool (DVT) implementation.
        /// </summary>
        /// <remarks>
        /// <p>
        /// The UID identifies the release of the Dicom Validation Tool (DVT).
        /// </p>
        /// <p>
        /// The Dicom Validation Tool (DVT) sents this UID during communication with the System Under Test (SUT).
        /// </p>
        /// <p>
        /// The number starts is composed of the following items<br></br>
        /// [ASCII(d)].[ASCII(v)].[ASCII(t)].[year].[version_major].[version_minor]<br></br>
        /// <c>1.2.826.0.1.3680043.2.1545.1.2.1.7</c>
        /// </p>
        /// </remarks>
        public String DvtImplementationClassUid
        {
            get
            {
                return dvtimplementationClassUid;
            }
            set
            {
                dvtimplementationClassUid = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// Enable or disable secure socket communication
        /// </summary>
        public Boolean SecureSocketsEnabled
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return secureSocketsEnabled;
            }
            set
            {
                secureSocketsEnabled = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The AE Title is the application entity name of the SUT machine in the test.
        /// </summary>
        public string SutAeTitle
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sutAeTitle;
            }
            set
            {
                sutAeTitle = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The port number that the Dicom Validation Tool (DVT) should use when making a 
        /// connection to the product machine of the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Also known as the remote connect port.
        /// </remarks>
        public UInt16 SutPort
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sutPort;
            }
            set
            {
                sutPort = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The name that the Dicom Validation Tool (DVT) should use when making a connection to the 
        /// product machine of the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// It is best to enter the Internet Address of the Product (in dot notation). 
        /// </remarks>
        public String SutHostName
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sutHostName;
            }
            set
            {
                sutHostName = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The maximum length of message fragment (P-DATA-TF PDU) 
        /// that the System Under Test (SUT) can receive from the Dicom Validation Tool (DVT).
        /// </summary>
        /// <remarks>
        /// DICOM DIMSE-messages are split into P-DATA-TF PDU fragments - e.g., C-STORE-RQ of a modality image.
        /// </remarks>
        public UInt32 SutMaximumLengthReceived
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sutMaximumLengthReceived;
            }
            set
            {
                sutMaximumLengthReceived = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// Represents root directory of the scripts in a session.
        /// </summary>
        public string DicomScriptRootDirectory
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return dicomScriptRootDirectory;
            }
            set
            {
                dicomScriptRootDirectory = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The version name given by the Manufacturer to the Product implementation to identify it internally. 
        /// DVT checks that the value sent by the Product matches the values given here. 
        /// </summary>
        /// <remarks>
        /// The implementation version name is an optional field - 
        /// when the Product does not send this value leave this entry blank.
        /// </remarks>
        public String SutImplementationVersionName
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sutimplementationVersionName;
            }
            set
            {
                sutimplementationVersionName = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// This is the unique identifier (UID) for the Product's implementation of the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// <p>
        /// The UID is assigned by the Manufacturer to the Product implementation to identify it.
        /// The manufacturer publishes this UID in the product DICOM conformance statement.
        /// </p>
        /// <p>
        /// The Dicom Validation Tool (DVT) checks that the value sent by the Product matches the value given here.
        /// </p>
        /// </remarks>
        public String SutImplementationClassUid
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return sutimplementationClassUid;
            }
            set
            {
                sutimplementationClassUid = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// Representa a Collection of scripts in a session.
        /// </summary>
        public IList ScriptFiles
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return scriptFiles;
            }
            set
            {
                scriptFiles = value;
            }
        }

        /// <summary>
        /// The DefineSqLength option, 
        /// is used to make DVT encode explicit length Sequences when sending messages. 
        /// </summary>
        /// <remarks>
        /// <p>
        /// Explicit lengths are computed for both the Sequence and each Item 
        /// present within the Sequence.
        /// </p>
        /// <p>
        /// By default DVT uses the undefined length encoding.
        /// </p>
        /// </remarks>
        public bool DefineSqLength
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return defineSqLength;
            }
            set
            {
                defineSqLength = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The AddGroupLength option, 
        /// is used to have DVT add Group Length attributes to all 
        /// Groups found in each message sent. 
        /// </summary>
        /// <remarks>
        /// By default DVT does not encode Group Length attributes 
        /// (except for the Command Group Length).
        /// </remarks>
        public bool AddGroupLength
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return addGroupLength;
            }
            set
            {
                addGroupLength = value;
                HasSessionChanged = true;
            }
        }

        /// <summary>
        /// The directory containing instructive and explanatory description files for the scripts.
        /// </summary>
        public string DescriptionDirectory
        {
            get
            {
                if (!isLoaded)
                {
                    LoadSession();
                }
                return descriptionDirectory;
            }
            set
            {
                descriptionDirectory = value;
                HasSessionChanged = true;
            }
        }

        # endregion

        # region Constructor
        /// <summary>
        /// Constructor for ScriptSession.
        /// </summary>
        public ScriptSession()
        {
            Type = SessionType.ST_SCRIPT;
        }
        /// <summary>
        /// Constructor for ScriptSession.
        /// </summary>
        /// <param name="fileName">fileName represents the Session Name.</param>
        public ScriptSession(string fileName)
        {
            Type = SessionType.ST_SCRIPT;
            SessionFileName = fileName;
            CreateSessionInstance(SessionFileName);
        }
        /// <summary>
        /// Property to access the Dvtk.Session.ScriptSession
        /// </summary>
        public DvtkSession.ScriptSession ScriptSessionImplementation
        {
            get
            {
                return (DvtkSession.ScriptSession)Implementation;
            }
        }
        # endregion

        # region Public Methods
        /// <summary>
        /// Method for executing a session in a synchronous manner.
        /// </summary>
        /// <param name="baseInput">baseInput Object (in this case ScriptInput Object) </param>
        /// <returns></returns>
        public override Result Execute(BaseInput baseInput)
        {
            CreateSessionInstance(SessionFileName);
            ScriptInput scriptInput = (ScriptInput)baseInput;
            string extension = Path.GetExtension(scriptInput.FileName);
            string tempName = Path.GetFileName((scriptInput.FileName).Replace(".", "_"));
            string name = CreateResultFileName(tempName);
            string fileName = Path.GetFileName(scriptInput.FileName);
            string scriptFullFileName = "";
            if (Path.GetDirectoryName(scriptInput.FileName) != "")
            {
                scriptFullFileName = scriptInput.FileName;
            }
            else
            {
                scriptFullFileName = Path.Combine(((DvtkSession.ScriptSession)Implementation).DicomScriptRootDirectory, scriptInput.FileName);
            }
            //scriptInput.Arguments[2] = fileName;
            if (OptionVerbose)
            {
                Implementation.ActivityReportEvent += new Dvtk.Events.ActivityReportEventHandler(ActivityReportEventHandler);
            }
            if (
                (string.Compare(extension, ".dss", true) == 0) ||
                (string.Compare(extension, ".ds", true) == 0)
                )
            {
                if ((Path.GetDirectoryName(scriptInput.FileName) != "") && (Path.GetDirectoryName(scriptInput.FileName) != null))
                {
                    ((DvtkSession.ScriptSession)Implementation).DicomScriptRootDirectory = Path.GetDirectoryName(scriptInput.FileName);
                }
                Implementation.StartResultsGathering(name);
                Result = ((DvtkSession.ScriptSession)Implementation).ExecuteScript(
                    fileName,
                    scriptInput.ContinueOnError
                    );
                Implementation.EndResultsGathering();

            }
            else if (string.Compare(extension, ".vbs", true) == 0)
            {
                VisualBasicScript Vbs = new VisualBasicScript(
                    (DvtkSession.ScriptSession)Implementation, Path.GetDirectoryName(scriptFullFileName), Path.GetFileName(scriptFullFileName));

                // An array of an array is needed!! Otherwise it won't work.
                ArrayList arraylist = new ArrayList();
                arraylist.Add(scriptInput.Arguments);
                Vbs.Execute();
                // Won't work.Vbs.Execute(scriptInput.Arguments);
            }
            else
            {
                Console.WriteLine(" Not a valid Script File");
            }
            return CreateResults(name);
        }

        /// <summary>
        /// Method for executing a session in an Asynchronous manner.
        /// </summary>
        /// <param name="baseInput">baseInput Object</param>
        public override void BeginExecute(BaseInput baseInput)
        {
            CreateSessionInstance(SessionFileName);
            ScriptInput scriptInput = (ScriptInput)baseInput;
            string extension = Path.GetExtension(scriptInput.FileName);
            // Remove the current results files for this script file.
            // If results files exists that will be removed, ask the user what to do with them.
            ArrayList resultsFilesToRemove = DvtkApplicationLayer.Result.GetAllNamesForSession(this);
            resultsFilesToRemove = DvtkApplicationLayer.Result.GetNamesForScriptFile(scriptInput.FileName, resultsFilesToRemove);
            resultsFilesToRemove = DvtkApplicationLayer.Result.GetNamesForCurrentSessionId(this, resultsFilesToRemove);
            if (BackUpFiles == true)
            {
                DvtkApplicationLayer.Result.BackupFiles(this, resultsFilesToRemove);
                DvtkApplicationLayer.Result.Remove(this, resultsFilesToRemove);
            }
            else
            {
                DvtkApplicationLayer.Result.Remove(this, resultsFilesToRemove);
            }

            if (
                (string.Compare(extension, ".dss", true) == 0) ||
                (string.Compare(extension, ".ds", true) == 0)
                )
            {
                ExecuteDicomScriptInThread(scriptInput);
            }
            else if (string.Compare(extension, ".vbs", true) == 0)
            {
                scriptFileName = scriptInput.FileName;
                ExecuteVisualBasicScriptInThread(scriptInput);
            }
            Script script = new Script(this, scriptFileName);
            script.CreateScriptResult(script);

        }
        /// <summary>
        /// set and save ScriptSession settings to file with extension <c>.ses</c>.
        /// </summary>
        public override bool Save()
        {
            CreateSessionInstance();
            SetAllProperties();
            return base.Save();
        }
        /// <summary>
        /// Execute the Visual Basic Script.
        /// </summary>
        public void ExecuteVisualBasicScript()
        {
            VisualBasicScript Vbs = new VisualBasicScript(
                (DvtkSession.ScriptSession)Implementation, scriptFileName);

            String[] emptyArray = { };
            ArrayList listContainingExmptyArray = new ArrayList();
            listContainingExmptyArray.Add(emptyArray);

            Vbs.Execute();
        }
        /// <summary>
        /// Method to create result for a script and set its properties.
        /// </summary>
		public void CreateScriptFiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(((DvtkSession.ScriptSession)Implementation).DicomScriptRootDirectory);
            if (directoryInfo.Exists)
            {
                FileInfo[] filesInfo = directoryInfo.GetFiles("*.*");

                if (scriptFiles == null)
                {
                    scriptFiles = new ArrayList();
                }
                else
                {
                    scriptFiles.Clear();
                }
                Script script = null;
                ArrayList scriptBaseNames = new ArrayList();
                foreach (FileInfo fileInfo in filesInfo)
                {
                    String extension = fileInfo.Extension.ToLower();

                    if ((extension == ".ds") || (extension == ".dss"))
                    {
                        script = new DicomScript(this, fileInfo.Name);
                        scriptBaseNames.Add(script.ScriptFileName);
                        scriptFiles.Add(script);
                    }
                    if ((extension == ".vb") || (extension == ".vbs"))
                    {
                        // VisualBasicScript needs to be adjusted:
                        // - Make it a descendant of Script.
                        // - Change the implementation so it will make use of the Script members.
                        // For the time being, create a DicomScript object instead.
                        script = new DicomScript(this, fileInfo.Name);

                        scriptBaseNames.Add(script.ScriptFileName);
                        scriptFiles.Add(script);
                    }
                }

                foreach (Script scriptFile in scriptFiles)
                {
                    ArrayList scripts = new ArrayList();
                    DirectoryInfo directoryInf = new DirectoryInfo(((DvtkSession.ScriptSession)Implementation).ResultsRootDirectory);
                    if (directoryInf.Exists)
                    {
                        FileInfo[] filesInf = directoryInf.GetFiles("*" + "_" + scriptFile.ScriptFileName.Replace(".", "_") + "_" + "*.xml");

                        foreach (FileInfo fileInf in filesInf)
                        {
                            Result correctResult = null;
                            String sessionId = GetSessionId(fileInf.Name);

                            foreach (Result result in scriptFile.Result)
                            {
                                if (result.SessionId == sessionId)
                                {
                                    correctResult = result;
                                    break;
                                }
                            }

                            if (correctResult == null)
                            {
                                correctResult = new Result(this);
                                correctResult.SessionId = sessionId;

                                scriptFile.Result.Add(correctResult);
                            }

                            bool isSummaryFile = true;
                            bool isMainResult = true;

                            if (fileInf.Name.ToLower().StartsWith("summary_"))
                            {
                                isSummaryFile = true;
                            }
                            else
                            {
                                isSummaryFile = false;
                            }

                            if (fileInf.Name.ToLower().EndsWith(scriptFile.ScriptFileName.Replace(".", "_").ToLower() + "_res.xml"))
                            {
                                isMainResult = true;
                            }
                            else
                            {
                                isMainResult = false;
                            }

                            if (isSummaryFile)
                            {
                                if (isMainResult)
                                {
                                    correctResult.SummaryFile = fileInf.Name;
                                    correctResult.ResultFiles.Add(fileInf.Name);
                                }
                                else
                                {
                                    correctResult.SubSummaryResultFiles.Add(fileInf.Name);
                                    //correctResult.ResultFiles.Add(fileInf.Name);
                                }
                            }
                            else
                            {
                                if (isMainResult)
                                {
                                    correctResult.DetailFile = fileInf.Name;
                                    correctResult.ResultFiles.Add(fileInf.Name);
                                }
                                else
                                {
                                    correctResult.SubDetailResultFiles.Add(fileInf.Name);
                                }
                            }
                        }
                    }
                }
            }
        }


        #endregion

        #region Protected Methods
        /// <summary>
        /// Method to create the instance of a EmulatorSession.
        /// </summary>
        protected override void CreateSessionInstance()
        {
            if (Implementation == null)
            {
                Implementation = new DvtkSession.ScriptSession();
            }
        }

        /// <summary>
        /// Method to create the instance of a session. 
        /// </summary>
        /// <param name="sessionFileName"> FileName of the ScriptSession.</param>
        protected override void CreateSessionInstance(string sessionFileName)
        {
            if (Implementation == null)
            {
                Implementation = new DvtkSession.ScriptSession();
                LoadSession();
            }
        }

        /// <summary>
        /// Method to get the Session Properties from a loaded session.
        /// </summary>
        protected override void GetAllProperties()
        {
            base.GetAllProperties();
            dvtAeTitle = ((DvtkSession.ScriptSession)Implementation).DvtSystemSettings.AeTitle;
            dvtPort = ((DvtkSession.ScriptSession)Implementation).DvtSystemSettings.Port;
            dvtSocketTimeOut = ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.SocketTimeout;
            dvtMaximumLengthReceived = ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.MaximumLengthReceived;
            sutAeTitle =
                ((DvtkSession.ScriptSession)Implementation).SutSystemSettings.AeTitle;
            sutPort = ((DvtkSession.ScriptSession)Implementation).SutSystemSettings.Port;
            sutHostName = ((DvtkSession.ScriptSession)Implementation).
                SutSystemSettings.HostName;
            sutMaximumLengthReceived = ((DvtkSession.ScriptSession)Implementation).
                SutSystemSettings.MaximumLengthReceived;
            sutimplementationVersionName = ((DvtkSession.ScriptSession)Implementation).
                SutSystemSettings.ImplementationVersionName;
            sutimplementationClassUid = ((DvtkSession.ScriptSession)Implementation).
                SutSystemSettings.ImplementationClassUid;
            dvtimplementationClassUid = ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.ImplementationClassUid;
            dvtimplementationVersionName = ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.ImplementationVersionName;
            dicomScriptRootDirectory = ((DvtkSession.ScriptSession)Implementation).
                DicomScriptRootDirectory;
            defineSqLength = ((DvtkSession.ScriptSession)Implementation).DefineSqLength;
            addGroupLength = ((DvtkSession.ScriptSession)Implementation).AddGroupLength;
            descriptionDirectory = ((DvtkSession.ScriptSession)Implementation).DescriptionDirectory;
        }

        /// <summary>
        /// Method to set the values of Session Properties in a SessionFile.
        /// </summary>
        public override void SetAllProperties()
        {
            base.SetAllProperties();
            ((DvtkSession.ScriptSession)Implementation).DvtSystemSettings.AeTitle = dvtAeTitle;
            ((DvtkSession.ScriptSession)Implementation).DvtSystemSettings.Port = dvtPort;
            ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.SocketTimeout = dvtSocketTimeOut;
            ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.MaximumLengthReceived = dvtMaximumLengthReceived;
            ((DvtkSession.ScriptSession)Implementation).SutSystemSettings.AeTitle = sutAeTitle;
            ((DvtkSession.ScriptSession)Implementation).SutSystemSettings.Port = sutPort;
            ((DvtkSession.ScriptSession)Implementation).SutSystemSettings.HostName = sutHostName;
            ((DvtkSession.ScriptSession)Implementation).
                SutSystemSettings.MaximumLengthReceived = sutMaximumLengthReceived;
            ((DvtkSession.ScriptSession)Implementation).
                DicomScriptRootDirectory = dicomScriptRootDirectory;
            ((DvtkSession.ScriptSession)Implementation).
                SutSystemSettings.ImplementationVersionName = sutimplementationVersionName;
            ((DvtkSession.ScriptSession)Implementation).
                SutSystemSettings.ImplementationClassUid = sutimplementationClassUid;
            ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.ImplementationVersionName = dvtimplementationVersionName;
            ((DvtkSession.ScriptSession)Implementation).
                DvtSystemSettings.ImplementationClassUid = dvtimplementationClassUid;
            ((DvtkSession.ScriptSession)Implementation).DefineSqLength = defineSqLength;
            ((DvtkSession.ScriptSession)Implementation).AddGroupLength = addGroupLength;
            ((DvtkSession.ScriptSession)Implementation).DescriptionDirectory = descriptionDirectory;
        }

        #endregion

        # region Private Methods

        private void ExecuteDicomScriptInThread(ScriptInput scriptInput)
        {
            string resultFileName = CreateResultFileName(scriptInput.FileName.Replace(".", "_"));
            Implementation.StartResultsGathering(resultFileName);
            AsyncCallback executeScriptAsyncCallback =
                new AsyncCallback(this.ResultsFromScriptAsyn);
            ((DvtkSession.ScriptSession)Implementation).BeginExecuteScript(
                scriptInput.FileName,
                false,
                executeScriptAsyncCallback
                );
        }
        private void ResultsFromScriptAsyn(IAsyncResult iAsyncResult)
        {
            ((DvtkSession.ScriptSession)Implementation).EndExecuteScript(iAsyncResult);
            Implementation.EndResultsGathering();
            callBackDelegate(null);
        }
        private void ExecuteVisualBasicScriptInThread(ScriptInput scriptInput)
        {
            scriptThread = new Thread(new ThreadStart(ExecuteVisualBasicScript));
            scriptThread.Start();
        }
        # endregion
    }

}




