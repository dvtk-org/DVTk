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
using System.Collections;
using DvtkSession = Dvtk.Sessions;


namespace DvtkApplicationLayer 
{
	/// <summary>
	/// Summary description for MediaSession.
	/// </summary>
	public class MediaSession : Session 
	{
		#region Private Members

		// The queue of media files to be validated
		private Queue mediaFilesToBeValidated = Queue.Synchronized(new Queue());
		private Queue mediaFilesToBeValidated1 = Queue.Synchronized(new Queue());
		private AsyncCallback callBackDelegate = null;
		private IList mediaFiles = new ArrayList();
		const string _SUMMARY_PREFIX = "summary_";
		const string _DETAIL_PREFIX = "detail_";
		private static int _exitCode = 0;

		#endregion
        
		#region Constructors
        /// <summary>
        /// Constructor of MediaSession.
        /// </summary>
        ///  <remarks>
        /// Validate DICOM media files.
        /// The Dicom Validation Tool (DVT) can also create and validate DICOM media files.
        /// </remarks>
		public MediaSession() 
		{
			Type = SessionType.ST_MEDIA;            
		}

        /// <summary>
        /// Constructor of MediaSession.
        /// </summary>
        /// <param name="fileName">fileName represents the Session Name.</param>
        /// <remarks>
        /// Validate DICOM media files.
        /// The Dicom Validation Tool (DVT) can also create and validate DICOM media files.
        /// </remarks>
		public MediaSession(string fileName) {
			Type = SessionType.ST_MEDIA;
			SessionFileName = fileName;
            CreateSessionInstance(SessionFileName);            
		}

        /// <summary>
        /// Property to access the Dvtk.Session.MediaSession 
        /// </summary>
        public DvtkSession.MediaSession MediaSessionImplementation {
            get {
                return (DvtkSession.MediaSession)Implementation;
            }
        }

		#endregion

		#region Public Properties
        /// <summary>
        /// Represents a collection of mediaFiles in a Media Session.
        /// </summary>
		public IList MediaFiles 
		{
			get{ return mediaFiles ;}
			set { mediaFiles = value;}
		}

		#endregion

		#region Public Methods
        /// <summary>
        /// Method for executing a session in a synchronous manner.
        /// </summary>
        /// <param name="baseInput">baseInput Object (in this case mediaInput Object) </param>
        /// <returns></returns>
       	public override Result Execute(BaseInput baseInput ) 
		{
			CreateSessionInstance(SessionFileName);
			MediaInput mediaInput = (MediaInput)baseInput;
			string baseName = "";
			string[] mediaFileAsArray = 
				(string[])mediaInput.FileNames.ToArray(typeof(string)); 
			if (OptionVerbose) 
			{
				Implementation.ActivityReportEvent +=new Dvtk.Events.ActivityReportEventHandler(ActivityReportEventHandler);
			}
			string fileName = Path.GetFileName((string)mediaInput.FileNames[0]);
			if (fileName.ToLower() == "dicomdir") 
			{
				baseName = fileName;
			}
			else 
			{
				baseName = fileName + "_DCM";
				baseName.Replace(".", "_");			
			}
			string resultName = CreateResultFileName(baseName);
			Implementation.StartResultsGathering(resultName);
			Result = 
				((DvtkSession.MediaSession)Implementation).ValidateMediaFiles(mediaFileAsArray);
			Implementation.EndResultsGathering();
			return CreateResults(resultName);
		}

		/// <summary>
		/// Method for executing a session in a synchronous manner.
		/// </summary>
		/// <param name="baseInput">baseInput Object (in this case mediaInput Object) </param>
		/// <returns></returns>
		public int ExecuteDir(BaseInput baseInput ) 
		{
			CreateSessionInstance(SessionFileName);
			MediaInput mediaInput = (MediaInput)baseInput;
			long TotalNrOfValidationErrors = 0 ;
			long TotalNrOfGeneralErrors = 0 ;
			long TotalNrOfUserErrors = 0 ;
			long TotalNrOfValidationWarnings = 0;
			long TotalNrOfGeneralWarnings = 0;
			long TotalNrOfUserWarnings = 0;
			
			string[] mediaFileAsArray = 
				(string[])mediaInput.FileNames.ToArray(typeof(string)); 
			string baseName = "";
			if (OptionVerbose) 
			{
				Implementation.ActivityReportEvent +=new Dvtk.Events.ActivityReportEventHandler(ActivityReportEventHandler);
			}
			for (int i =0 ; i < mediaInput.FileNames.Count ; i++)
				{
				string fullFileName = (string)mediaInput.FileNames[i];
				string fileName = Path.GetFileName(fullFileName);
				if (fileName.ToLower() == "dicomdir") 
				{
					baseName = fileName;
				}
				else 
				{
					baseName = fileName + "_DCM";
					baseName.Replace(".", "_");			
				}
				Implementation.StartResultsGathering(CreateResultFileName(baseName));        
				string[] mediaFilesToValidate = new string[] {fullFileName};
				Result = 
					((DvtkSession.MediaSession)Implementation).ValidateMediaFiles(mediaFilesToValidate);
				Implementation.EndResultsGathering();
				TotalNrOfValidationErrors = TotalNrOfValidationErrors + Implementation.CountingTarget.TotalNrOfValidationErrors ;
				TotalNrOfUserErrors = TotalNrOfValidationErrors + Implementation.CountingTarget.TotalNrOfUserErrors ;
				TotalNrOfGeneralErrors = TotalNrOfValidationErrors + Implementation.CountingTarget.TotalNrOfGeneralErrors ;
				TotalNrOfValidationWarnings = TotalNrOfValidationWarnings + Implementation.CountingTarget.TotalNrOfValidationWarnings ;
				TotalNrOfGeneralWarnings = TotalNrOfValidationErrors + Implementation.CountingTarget.TotalNrOfGeneralWarnings ;
				TotalNrOfUserWarnings = TotalNrOfUserWarnings + Implementation.CountingTarget.TotalNrOfUserWarnings ;
			}
			DisplayResultCounters(TotalNrOfValidationErrors , TotalNrOfGeneralErrors , TotalNrOfUserErrors ,TotalNrOfValidationWarnings , TotalNrOfGeneralWarnings ,TotalNrOfUserWarnings) ;
			return DetermineExitCode(TotalNrOfValidationErrors , TotalNrOfGeneralErrors , TotalNrOfUserErrors ,TotalNrOfValidationWarnings , TotalNrOfGeneralWarnings ,TotalNrOfUserWarnings) ;
		}

		private static void DisplayResultCounters(long TotalNrOfValidationErrors , long TotalNrOfGeneralErrors , long TotalNrOfUserErrors ,long TotalNrOfValidationWarnings ,long TotalNrOfGeneralWarnings , long TotalNrOfUserWarnings) 
		{
			
			if ((TotalNrOfValidationErrors == 0) &&
				(TotalNrOfUserErrors == 0) &&
				(TotalNrOfGeneralErrors == 0)) 
			{
				Console.WriteLine ("");
				Console.WriteLine("RESULT: PASSED");
			}
			else 
			{
				Console.WriteLine ("");
				Console.WriteLine("RESULT: FAILED");
			}
			Console.WriteLine("Validation Errors in all the Files: {0}\nValidation Warnings in all the Files: {1}",
				TotalNrOfValidationErrors, 
				TotalNrOfValidationWarnings);
			Console.WriteLine("User Errors in all the Files: {0}\nUser Warnings in all the Files: {1}",
				TotalNrOfUserErrors, 
				TotalNrOfUserWarnings);
			Console.WriteLine("General Errors in all the Files: {0}\nGeneral Warnings in all the Files: {1}",
				TotalNrOfGeneralErrors, 
				TotalNrOfGeneralWarnings);
			Console.WriteLine("");
		}

		private static int DetermineExitCode(long TotalNrOfValidationErrors , long TotalNrOfGeneralErrors , long TotalNrOfUserErrors ,long TotalNrOfValidationWarnings ,long TotalNrOfGeneralWarnings , long TotalNrOfUserWarnings) 
		{
			if ((TotalNrOfValidationErrors == 0) &&
				(TotalNrOfUserErrors == 0) &&
				(TotalNrOfGeneralErrors == 0) &&
				(TotalNrOfValidationWarnings == 0) &&
				(TotalNrOfUserWarnings == 0) &&
				(TotalNrOfGeneralWarnings == 0)) 
			{
				// No errors / no warnings
				_exitCode = 0;
			}
			else if ((TotalNrOfValidationErrors == 0) &&
				(TotalNrOfUserErrors == 0) &&
				(TotalNrOfGeneralErrors == 0))
			{
				// No errors / some warnings
				_exitCode = -1;
			}
			else if ((TotalNrOfValidationWarnings == 0) &&
				(TotalNrOfUserWarnings == 0) &&
				(TotalNrOfGeneralWarnings == 0))
			{
				// Some errors / no warnings
				_exitCode = -2;
			}
			else
			{
				// Some errors and warnings
				_exitCode = -3;
			}
			return _exitCode;
		}

        /// <summary>
        /// Method for executing a session in an Asynchronous manner.
        /// </summary>
        /// <param name="baseInput">baseInput Object</param>
		public override void BeginExecute(BaseInput baseInput ) 
		{ 
			CreateSessionInstance(SessionFileName);
			MediaInput mediaInput = (MediaInput)baseInput;
			string[] mediaFileAsArray = 
				(string[])mediaInput.FileNames.ToArray(typeof(string)); 
			foreach(string fileName in mediaFileAsArray) 
			{
				mediaFilesToBeValidated.Enqueue(fileName);
			}
			ValidateMediaFiles();
		}        
        
        /// <summary>
        /// set and save MediaSession settings to file with extension <c>.ses</c>.
        /// </summary>
		public override bool Save () 
		{
			CreateSessionInstance();
            base.SetAllProperties();
			return base.Save();
		}

        /// <summary>
        /// Method to create result for a mediaFile and set its properties.
        /// </summary>
        public void CreateMediaFiles()
		{
            if(mediaFiles == null) 
			{
                mediaFiles = new ArrayList();
            } else 
			{
                mediaFiles.Clear();
            }
            ArrayList allMediaFiles = GetFileNamesforSession();
            ArrayList mediaFilesBaseNames = GetBaseNamesForResultsFiles(allMediaFiles);
            DirectoryInfo directoryInfo = new DirectoryInfo(((DvtkSession.MediaSession)Implementation).ResultsRootDirectory);
            foreach ( string mediaFilesBaseName in mediaFilesBaseNames) {	
                MediaFile mFile = new MediaFile(this , mediaFilesBaseName );
                mFile.MediaFileName = mediaFilesBaseName;
                ArrayList mediaFile = new ArrayList();
                FileInfo[] filesInfo = directoryInfo.GetFiles("*" + mediaFilesBaseName + "*.xml");
					
                foreach (FileInfo fileInfo in filesInfo) {
                    Result correctResult = null;
                    String sessionId = GetSessionId (fileInfo.Name);

                    foreach(Result result in mFile.Result) 
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

                        mFile.Result.Add(correctResult);
                    }

                    bool isSummaryFile = true;
                    bool isMainResult = true;

                    if (fileInfo.Name.ToLower().StartsWith("summary_")) 
					{
                        isSummaryFile = true;
                    }
                    else 
					{
                        isSummaryFile = false;
                    }
		
                    if (fileInfo.Name.ToLower().EndsWith(mediaFilesBaseName.ToLower() +"_res.xml")) 
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
                            correctResult.SummaryFile = fileInfo.Name;
                            correctResult.ResultFiles.Add(fileInfo.Name);
                        }
                        else 
						{
                            correctResult.SubSummaryResultFiles.Add(fileInfo.Name);
                        }
                    }
                    else 
					{
                        if (isMainResult) 
						{
                            correctResult.DetailFile = fileInfo.Name;
                            correctResult.ResultFiles.Add(fileInfo.Name);
                        }
                        else 
						{
                            correctResult.SubDetailResultFiles.Add(fileInfo.Name);
                        }
                    }
                }
                MediaFiles.Add(mFile);
            }
        }
        #endregion

		#region Protected Methods
        /// <summary>
        /// Method to create the instance of a MediaSession.
        /// </summary>
        protected override void CreateSessionInstance() {
            if (Implementation == null) {
                Implementation = new DvtkSession.MediaSession();
            }
        }

        /// <summary>
        /// Method to create the instance of a session. 
        /// </summary>
        /// <param name="sessionFileName"> FileName of the MediaSession.</param>
		protected override void CreateSessionInstance(string sessionFileName) {
            if (Implementation == null) {
                Implementation = new DvtkSession.MediaSession();
                LoadSession();
            }
        }		

		#endregion
        
		#region Private Methods
        /// <summary>
        /// Validate Media Storage Files.
        /// </summary>
        /// <remarks>
        /// Typically these files have the file-extension DCM. DVT does not check the file-extension.
        /// The file should have an internal byte-prefix with byte-values 'DICOM'.
        /// </remarks>
		private void ValidateMediaFiles() {
            lock (this) {
                if (mediaFilesToBeValidated.Count > 0) {
                    string fullFileName = (string)mediaFilesToBeValidated.Dequeue();
                    string baseName = "";
                    string fileName = Path.GetFileName(fullFileName);
                    if (fileName.ToLower() == "dicomdir") {
                        baseName = fileName;
                    }
                    else {
                        baseName = fileName + "_DCM";
                        baseName.Replace(".", "_");			
                    }
                    Implementation.StartResultsGathering(CreateResultFileName(baseName));        
                    string[] mediaFilesToValidate = new string[] {fullFileName};
                    // Perform the actual execution of the script.
                    AsyncCallback mediaFilesAsyncCallback = 
                        new AsyncCallback(this.ResultsFromAsyncValidation);
                    ((DvtkSession.MediaSession)Implementation).BeginValidateMediaFiles(
                        mediaFilesToValidate, mediaFilesAsyncCallback
                        );
                }
            }
        }

		private void ResultsFromAsyncValidation(IAsyncResult iAsyncResult) 
		{
			((DvtkSession.MediaSession)Implementation).EndValidateMediaFiles(iAsyncResult);
			Implementation.EndResultsGathering();
			if (mediaFilesToBeValidated.Count > 0) 
			{
				ValidateMediaFiles();
			}
			callBackDelegate(null);
		}
		
		#endregion
	}

}
    

