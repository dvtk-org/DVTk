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
using DvtkData;
using System.Diagnostics;
using System.Windows.Forms;

namespace DvtkApplicationLayer {
    /// <summary>
    /// Represents a summary and/or detail results file.
    /// </summary>
    public class Result : PartOfSession {
        const string _SUMMARY_PREFIX = "summary_";
        const string _DETAIL_PREFIX = "detail_";

        #region Private Member Variables
        private ArrayList resultFiles = new ArrayList();
        private string summaryFile = "";
        private string detailFile = "";
        private ArrayList subSummaryResultFiles = new ArrayList();
        private ArrayList subDetailResultFiles = new ArrayList();
        private string date;
        private UInt32 nrOfErrors;
        private UInt32 nrOfWarnings;

		/// <summary>
		/// 
		/// </summary>
        public string sessionId;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errors">Nos of errors</param>
        /// <param name="warnings">Nos of warnings</param>
        /// <param name="detailFileName">Name of the detailed resultfile.</param>
        /// <param name="summaryFileName">Name of the summary resultfile.</param>
        /// <param name="resultfiles">Collection of resultFiles.</param>
        public Result(
            UInt32 errors, 
            UInt32 warnings, 
            string detailFileName,
            string summaryFileName, 
            ArrayList resultfiles
            ) {
            nrOfErrors = errors;
            nrOfWarnings = warnings;
            detailFile = detailFileName ;
            summaryFile = summaryFileName ;
            resultFiles = resultfiles;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public Result(Session session): base(session) {
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Represents the date when the result was formed.
        /// </summary>
        public string Date {
            get {
                return date;
            }
            set {date = value;
            }
        }
        /// <summary>
        /// Collection of sub summary results of  validation.
        /// </summary>
        public ArrayList SubSummaryResultFiles {
            get {
                return subSummaryResultFiles;
            }
            set{
                subSummaryResultFiles = value;
            }

        }
        /// <summary>
        /// Collection of sub detail results of  validation.
        /// </summary>
        public ArrayList SubDetailResultFiles {
            get {
                return subDetailResultFiles;
            }
            set{
                subDetailResultFiles = value;
            }

        }
        /// <summary>
        /// Collection of ResultFiles.
        /// </summary>
        public ArrayList ResultFiles {
            get {
                return resultFiles;
            }
            set{
                resultFiles = value;
            }

        }
        /// <summary>
        /// Represents the summary result of validation.
        /// </summary>
        public string SummaryFile {
            get {
                return summaryFile;
            }
            set {summaryFile = value;
            }
        }
        /// <summary>
        /// Represents the detail result of validation.
        /// </summary>
        public string DetailFile {
            get {
                return detailFile;
            }
            set {detailFile = value;
            }
        }
        /// <summary>
        /// Number of errors.
        /// </summary>
        public System.UInt32 NrOfErrors {
            get { 
                return nrOfErrors;
            }
            set {nrOfErrors = value;
            }
        } 
        /// <summary>
        /// Number of warnings.
        /// </summary>
        public System.UInt32 NrOfWarnings {
            get { 
                return nrOfWarnings;
            }
            set {nrOfWarnings = value;
            }
        }
        /// <summary>
        /// Represents the sessionId.
        /// </summary>
        public string SessionId {
            get {
                return sessionId;
            }
            set {
                sessionId = value;
            }
        }
        #endregion

        # region Private Functions 

        private static string GetBaseNameForScriptFile(string theScriptFileName) {
            return(theScriptFileName.Replace(".", "_"));
        }
       
        /// <summary>
        /// Converts a results file name to its corresponding base name.
        /// The base name is the result of removing "....._xxx_" at the beginning and removing
        /// "_res?.xml" at the end.
        /// </summary>
        /// <param name="theBaseName">The results file name</param>
        /// <returns>Indicates if the supplied results file name is a valid one.</returns>
        private static bool GetBaseNameForResultsFile(string theBaseName) 
		{
            // Remove the first prefix.
            if (theBaseName.ToLower().StartsWith(_SUMMARY_PREFIX)) 
			{
                theBaseName = theBaseName.Remove(0, _SUMMARY_PREFIX.Length);
            }
            else if (theBaseName.ToLower().StartsWith(_DETAIL_PREFIX)) 
			{
                theBaseName = theBaseName.Remove(0, _DETAIL_PREFIX.Length);
            }
            else 
			{
                return false;
            }

            // Remove the second prefix: session ID and underscore.
            if (theBaseName.Length > 3) 
			{
                if (theBaseName.ToLower().EndsWith(".xml"))
                {
                    theBaseName = theBaseName.Substring(0, (theBaseName.Length - 4));
                }

                // Is long enough to contain the substring "xxx_".
                Int16 theInt16 = Convert.ToInt16(theBaseName.Substring(0, 3));

                theBaseName = theBaseName.Substring(3);

                if (theBaseName[0] == '_') 
				{	
                    theBaseName = theBaseName.Substring(1);                    
                }

                int index = theBaseName.IndexOf("_res");
                if (index != -1)
                {
                    theBaseName = theBaseName.Substring(0, index);
                    return true;
                }
            }

            //// Remove all digits at the end of the string.
            //if (isValidResultsFileName) 
            //{
            //    bool continueRemovingDigit = true;

            //    while (continueRemovingDigit) 
            //    {
            //        try 
            //        {
            //            Int16 theInt16 = Convert.ToInt16(theBaseName.Substring(theBaseName.Length - 1));
            //        }
            //        catch 
            //        {
            //            continueRemovingDigit = false;
            //        }

            //        if (continueRemovingDigit) 
            //        {
            //            theBaseName = theBaseName.Substring(0, theBaseName.Length - 1);
            //        }
            //    }
            //}

            //// Remove the "_res" at the end.
            //if (isValidResultsFileName) 
            //{
            //    if (theBaseName.ToLower().EndsWith("_res")) 
            //    {
            //        theBaseName = theBaseName.Substring(0, theBaseName.Length - 4);
            //    }
            //    else 
            //    {
            //        isValidResultsFileName = false;
            //    }
            //}
			
            //// Base name is only valid if it is not empty.
            //if (isValidResultsFileName) 
            //{
            //    if (theBaseName.Length == 0) 
            //    {
            //        isValidResultsFileName = false;
            //    }
            //}

            //if (!isValidResultsFileName) 
            //{
            //    theBaseName = "";
            //}

            return false;
        }


        # endregion

        /// <summary>
        /// Method to get the basename for a resultFile.
        /// </summary>
        /// <param name="theResultsFileName"> FullName of a resultFile.</param>
        /// <returns></returns>
        public static string GetBaseNameNoCheck(string theResultsFileName) 
        {
            string theBaseName = theResultsFileName;

            // Remove first prefix.
            theBaseName = theBaseName.Substring(theBaseName.IndexOf("_") + 1);

            // Remove second prefix: session ID and '_'.
            theBaseName = theBaseName.Substring(theBaseName.IndexOf("_") + 1);

            // Remove postfix.
            int indexOfLastUnderline = theBaseName.LastIndexOf("_");
            theBaseName = theBaseName.Substring(0, indexOfLastUnderline);

            return(theBaseName);
        }

        /// <summary>
        /// Get the results file names of the files that need to be shown (in)directly under the tree node indicated by the tree node tag.
        /// </summary>
        /// <param name="theScriptFileName">The tree node tag.</param>
        /// <param name="theResultsFileNames">Valid results file names to choose from.</param>
        /// <returns>The results file names to display under the tree node.</returns>
        public static ArrayList GetNamesForScriptFile(string theScriptFileName, ArrayList theResultsFileNames) 
        {
            ArrayList theNames = new ArrayList();

            string theLowerCaseScriptFileBaseName = GetBaseNameForScriptFile(theScriptFileName).ToLower();

            foreach (string theResultsFileName in theResultsFileNames) {
                string theLowerCaseResultsFileName = theResultsFileName.ToLower();
                string theLowerCaseResultsFileBaseName = GetBaseNameNoCheck(theLowerCaseResultsFileName);

                if ( (theLowerCaseResultsFileBaseName.IndexOf(theLowerCaseScriptFileBaseName) != -1) &&
                    (theLowerCaseResultsFileName.IndexOf("_" + theLowerCaseScriptFileBaseName + "_") != -1)
                    ) 
                {
                    theNames.Add(theResultsFileName);
                }
            }

            return theNames;
        }

        /// <summary>
        /// From a list of results file names, return those results file names with the same session ID
        /// as the session ID of the supplied session.
        /// </summary>
        /// <param name="theSession">The session.</param>
        /// <param name="theResultsFileNames">Valid results file names.</param>
        /// <returns>List of results file names.</returns>
        public static ArrayList GetNamesForCurrentSessionId(DvtkApplicationLayer.Session  theSession, ArrayList theResultsFileNames) {
            ArrayList theNamesForCurrentSession = new ArrayList();
            string theSessionIdAsString = theSession.SessionId.ToString("000");

            foreach(string theResultsFileName in theResultsFileNames) {
                string theResultsFileSessionIdAsString = "";

                if (theResultsFileName.ToLower().StartsWith(_SUMMARY_PREFIX)) {
                    theResultsFileSessionIdAsString = theResultsFileName.Substring(_SUMMARY_PREFIX.Length, 3);
                }
                else if (theResultsFileName.ToLower().StartsWith(_DETAIL_PREFIX)) {
                    theResultsFileSessionIdAsString = theResultsFileName.Substring(_DETAIL_PREFIX.Length, 3);
                }
                else 
				{
                    // Sanity check.
                    Debug.Assert(false);
                }
	
                if (theResultsFileSessionIdAsString == theSessionIdAsString) 
				{
                    theNamesForCurrentSession.Add(theResultsFileName);
                }
            }

            return(theNamesForCurrentSession);
        }

        /// <summary>
        /// Remove .xml files, and if exisiting, the corresponding .html files.
        /// </summary>
        /// <param name="theSession">The session.</param>
        /// <param name="theResultsFileNames">The results file names.</param>
        public static void Remove(DvtkApplicationLayer.Session theSession, ArrayList theResultsFileNames) {
            foreach(string theResultsFileName in theResultsFileNames) {
                string theXmlResultsFullFileName = System.IO.Path.Combine(theSession.ResultsRootDirectory, theResultsFileName);
                string theHtmlResultsFullFileName = theXmlResultsFullFileName.ToLower().Replace(".xml", ".html");

                if (!File.Exists(theXmlResultsFullFileName)) 
				{
                    // Sanity check.
                    Debug.Assert(false);
                }
                else 
				{
                    try 
					{
                        File.Delete(theXmlResultsFullFileName);
                    }
                    catch(Exception ex) 
					{
                        MessageBox.Show(ex.Message);
                        // In release mode, just continue.
                        Debug.Assert(false);
                    }
                }

                if (File.Exists(theHtmlResultsFullFileName)) 
				{
                    try 
					{
                        File.Delete(theHtmlResultsFullFileName);
                    }
                    catch(Exception ex) 
					{
                        // In release mode, just continue.
                        MessageBox.Show(ex.Message);
                        Debug.Assert(false);
                    }
                }
            }
        }

        /// <summary>
        /// Method to backUp the Arraylist of files in a session.
        /// </summary>
        /// <param name="theSession">represents the Session.</param>
        /// <param name="theFilesToBackup"></param>
        public static void BackupFiles(DvtkApplicationLayer.Session theSession, ArrayList theFilesToBackup)
		{
            foreach (string theFileToBackup in theFilesToBackup) 
			{
                BackupFile(theSession, theFileToBackup);
            }
        }

        private static void BackupFile(DvtkApplicationLayer.Session theSession, string theFileToBackup) 
		{
            string theSourceFullFileName = System.IO.Path.Combine(theSession.ResultsRootDirectory, theFileToBackup);
            string theDestinyFullFileName = theSourceFullFileName + "_backup";
            int theCounter = 1;

            try 
			{
                while (File.Exists(theDestinyFullFileName + theCounter.ToString())) 
				{
                    theCounter++;
                }

                File.Copy(theSourceFullFileName, theDestinyFullFileName + theCounter.ToString());
            }
            catch 
			{
                // Don't do anything in the release version.
                Debug.Assert(false);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theMediaFullFileName"></param>
		/// <returns></returns>
        public static string GetBaseNameForMediaFile(string theMediaFullFileName) {
            string theMediaFileName = System.IO.Path.GetFileName(theMediaFullFileName);
            string theBaseName = "";

            if (theMediaFileName.ToLower().IndexOf("dicomdir") != -1) {
                theBaseName = theMediaFileName;
            }
            else {
                theBaseName = theMediaFileName + "_DCM";
                theBaseName = theBaseName.Replace(".", "_");			
            }

            return(theBaseName);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theBaseName"></param>
		/// <param name="theResultsFileNames"></param>
		/// <returns></returns>
        public static ArrayList GetNamesForBaseName(string theBaseName, ArrayList theResultsFileNames) {
            ArrayList theNames = new ArrayList();
            string theLowerCaseBaseName = theBaseName.ToLower();

            foreach (string theResultsFileName in theResultsFileNames) {
                string theLowerCaseResultsFileBaseName = GetBaseNameNoCheck(theResultsFileName).ToLower();

                if (theLowerCaseBaseName == theLowerCaseResultsFileBaseName) {
                    theNames.Add(theResultsFileName);
                }
            }

            return theNames;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <returns></returns>
        public static ArrayList GetAllNamesForSession(DvtkApplicationLayer.Session theSession) {
            ArrayList theResultsFiles = new ArrayList();
            DirectoryInfo theDirectoryInfo;
            FileInfo[] theFilesInfo;

            theDirectoryInfo = new DirectoryInfo (theSession.ResultsRootDirectory);

            if (theDirectoryInfo.Exists) {
                theFilesInfo = theDirectoryInfo.GetFiles ("*.xml");

                foreach (FileInfo theFileInfo in theFilesInfo) {
                    string theResultsFileName = theFileInfo.Name;

                    if (IsValid(theResultsFileName)) {
                        theResultsFiles.Add(theResultsFileName);
                    }
                }
            }

            return theResultsFiles;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <param name="theScriptFileName"></param>
		/// <returns></returns>
        public static string GetSummaryNameForScriptFile(DvtkApplicationLayer.Session theSession, string theScriptFileName) {
            return("Summary_" + GetExpandedNameForScriptFile(theSession, theScriptFileName));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <param name="theEmulatorType"></param>
		/// <returns></returns>
        public static string GetSummaryNameForEmulator(DvtkApplicationLayer.Session theSession, Emulator.EmulatorTypes theEmulatorType) {
            return("Summary_" + GetExpandedNameForEmulator(theSession, theEmulatorType));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <param name="theMediaFullFileName"></param>
		/// <returns></returns>
        public static string GetSummaryNameForMediaFile(DvtkApplicationLayer.Session theSession, string theMediaFullFileName) {
            return("Summary_" + GetExpandedNameForMediaFile(theSession, theMediaFullFileName));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <param name="theScriptFileName"></param>
		/// <returns></returns>
        public static string GetExpandedNameForScriptFile(DvtkApplicationLayer.Session theSession, string theScriptFileName) {
            return(GetExpandedName(theSession, GetBaseNameForScriptFile(theScriptFileName)));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <param name="theEmulatorType"></param>
		/// <returns></returns>
        public static string GetExpandedNameForEmulator(DvtkApplicationLayer.Session theSession, Emulator.EmulatorTypes theEmulatorType) {
            return(GetExpandedName(theSession, GetBaseNameForEmulator(theEmulatorType)));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <param name="theMediaFullFileName"></param>
		/// <returns></returns>
        public static string GetExpandedNameForMediaFile(DvtkApplicationLayer.Session theSession, string theMediaFullFileName) {
            return(GetExpandedName(theSession, GetBaseNameForMediaFile(theMediaFullFileName)));
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSession"></param>
		/// <param name="theString"></param>
		/// <returns></returns>
        private static string GetExpandedName(DvtkApplicationLayer.Session theSession, string theString) {
            return(theSession.SessionId.ToString("000") + '_' + theString + "_res.xml");
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theEmulatorType"></param>
		/// <returns></returns>
        public static string GetBaseNameForEmulator(Emulator.EmulatorTypes theEmulatorType) 
		{
            string theBaseName = "";

            switch(theEmulatorType) 
			{
                case Emulator.EmulatorTypes.PRINT_SCP:
                    theBaseName = "Pr_Scp_Em";
                    break;

                case Emulator.EmulatorTypes.STORAGE_SCP:
                    theBaseName = "St_Scp_Em";
                    break;

                case Emulator.EmulatorTypes.STORAGE_SCU:
                    theBaseName = "St_Scu_Em";
                    break;

                default:
                    // Not implemented.
                    Debug.Assert(false);
                    break;
            }

            return(theBaseName);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resultFileName"></param>
		/// <returns></returns>
        public bool ContainsResultsFile(String resultFileName) 
        {
            bool containsResultsFile = false;
            foreach ( string resultName in resultFiles) 
            {
                if (resultName == resultFileName) 
                {
                    containsResultsFile = true;
                    break;
                }
            }

            return(containsResultsFile);
        }

        /// <summary>
        /// Returns a boolean indicating if the supplied results file name is a valid one.
        /// </summary>
        /// <param name="theResultsFileName">The results file name.</param>
        /// <returns>Indicating if the supplied results file name is a valid one.</returns>
        public static bool IsValid(string theResultsFileName) 
        {
            return GetBaseNameForResultsFile(theResultsFileName);
        }
    }
}
