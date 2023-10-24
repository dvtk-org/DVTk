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
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using Dvtk;

namespace DvtkApplicationLayer
{
    /// <summary>
    /// Summary description for project.
    /// </summary>
    public class Project
    {
        #region Private Members

        private string projectFileName;
        private IList sessions = new ArrayList();
        private bool hasProjectChanged = false;
        private int nosOfSessionsChanged;
        SessionManager sessionManager = SessionManager.Instance;
        private bool isProjectConstructed = false;

        /// <summary>
        /// 
        /// </summary>
        public bool hasUserCancelledLastOperation = false;

        /// <summary>
        /// 
        /// </summary>
        public CallBackMessageDisplay display_message;

        /// <summary>
        /// 
        /// </summary>
        public delegate void CallBackMessageDisplay(string message);

        /// <summary>
        /// 
        /// </summary>
        public bool tempButtonNo = false;

        /// <summary>
        /// 
        /// </summary>
        public bool tempButtonCancel = false;

        #endregion

        #region Properties
        /// <summary>
        /// Represents the Project File Name.
        /// </summary>
        public string ProjectFileName
        {
            get
            {
                return projectFileName;
            }

            set
            {
                projectFileName = value;
            }
        }
        /// <summary>
        /// Represents a collection of Sessions in a Project.
        /// </summary>
        public IList Sessions
        {
            get { return sessions; }
        }
        /// <summary>
        /// Boolean that determines whether the last operation has been cancelled be the user.
        /// </summary>
        public bool HasCancelledLastOperation
        {
            get
            {
                return hasUserCancelledLastOperation;
            }

            set
            {
                hasUserCancelledLastOperation = value;
            }
        }

        /// <summary>
        /// Boolean that represents whether the project has been changed.
        /// </summary>
        public bool HasProjectChanged
        {
            get
            {
                return hasProjectChanged;
            }

            set
            {
                hasProjectChanged = value;
            }
        }
        /// <summary>
        /// Boolean that represents whether the project has been Constructed.
        /// </summary>
        public bool IsProjectConstructed
        {
            get
            {
                return isProjectConstructed;
            }
        }
        /// <summary>
        /// Property that represents the number of changed sessions.
        /// </summary>
        public int NosOfSessionsChanged
        {
            get
            {
                return nosOfSessionsChanged;
            }
            set
            {
                nosOfSessionsChanged = value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Project()
        {
            projectFileName = "";
            hasProjectChanged = false;
            hasUserCancelledLastOperation = false;
            isProjectConstructed = false;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Method to Load the Project.
        /// </summary>
        /// <param name="projFileName">FullFileName of the Project.</param>
        /// <returns></returns>
        public bool Load(string projFileName)
        {
            bool success = false;
            projectFileName = projFileName;
            // Check that the file exists.
            FileInfo projectFileInfo = new FileInfo(projectFileName);
            if (!projectFileInfo.Exists)
            {
                string msg =
                    string.Format(
                    "Failed to load project file: {0}\n. File does not exist.", projectFileName);
                success = false;
            }
            else
            {
                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(projectFileName);
                    string sessionFileName = "";
                    reader.WhitespaceHandling = WhitespaceHandling.None;
                    reader.MoveToContent();
                    do
                    {
                        sessionFileName = reader.ReadElementString();
                        if (sessionFileName != "")
                        {
                            if (PathUtils.IsRelativePath(sessionFileName))
                            {
                                sessionFileName = PathUtils.ConvertToAbsolutePath(Path.GetDirectoryName(this.projectFileName), sessionFileName);
                            }
                            // If the session file name was already a full file name, or the conversion
                            // from a relative filename to an absolute one succeeded, load the session.
                            if (sessionFileName != "")
                            {
                                Session session = sessionManager.CreateSession(sessionFileName);
                                if (session != null)
                                {
                                    session.ParentProject = this;
                                    session.BaseLocation =
                                        System.IO.Path.GetDirectoryName(projectFileName);
                                    sessions.Add(session);
                                }
                                else
                                {
                                    string msg =
                                    string.Format(
                                    "Failed to load invalid session file: {0}.\n",
                                    sessionFileName);
                                    this.display_message(msg);
                                }
                            }
                        }
                    } while (reader.Name == "Session");
                    success = true;
                }
                catch (Exception e)
                {
                    string msg =
                        string.Format(
                        "Failed to load project file: {0}. Due to exception:{1}\n",
                        this.projectFileName,
                        e.Message);
                    this.display_message(msg);
                    success = false;
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
            }

            if (success)
            {
                isProjectConstructed = true;
                hasProjectChanged = false;
                hasUserCancelledLastOperation = false;
            }
            else
            {
                Close(false);
            }
            return success;
        }

        /// <summary>
        /// Save the Project to the specified Project file name. 
        /// The current project file name will also change to the specified project file name.
        /// Sessions will not be saved.
        /// </summary>
        /// <param name="theProjectFileName">The name of the Project file ot save to.</param>
        /// <returns>Boolean indicating if the saving was successfull.</returns>
        public bool SaveProject(string theProjectFileName)
        {
            projectFileName = theProjectFileName;
            return (SaveProject());
        }

        /// <summary>
        /// Save the Project to the current Project file name.
        /// Sessions will not be saved.
        /// </summary>
        /// <returns>Boolean indicating if the saving was successfull.</returns>
        public bool SaveProject()
        {
            bool success = false;
            XmlTextWriter writer = null;
            try
            {
                writer = new XmlTextWriter(this.projectFileName, System.Text.Encoding.ASCII);
                // The written .xml file will be more readable
                writer.Formatting = Formatting.Indented;
                // Start the document
                writer.WriteStartDocument(true);
                // Write the session element containing all session files
                writer.WriteStartElement("Sessions");
                //Write the session filenames to the document
                for (int i = 0; i < sessions.Count; i++)
                {
                    //writer.WriteElementString ("Session", ((Session)sessions[i]).SessionFileName);
                    string sessionFileName = ((Session)sessions[i]).SessionFileName;
                    if (PathUtils.IsRelativePath(sessionFileName))
                    {
                        writer.WriteElementString("Session", sessionFileName);
                    }
                    else
                    {
                        string projectDirectoryName = Path.GetDirectoryName(this.projectFileName);
                        string sessionDirectoryName = Path.GetDirectoryName(sessionFileName);
                        string relativePath = GetRelativePath(projectDirectoryName, sessionDirectoryName);
                        writer.WriteElementString("Session", relativePath + Path.GetFileName(sessionFileName));
                    }
                }
                // End the sessions element
                writer.WriteEndElement();
                // End the document
                writer.WriteEndDocument();
                success = true;
            }
            catch (Exception e)
            {
                string msg =
                    string.Format(
                    "Failed to write project file: {0}. Due to exception:{1}\n",
                    this.projectFileName,
                    e.Message);
                success = false;
            }
            finally
            {
                if (writer != null) writer.Close();
            }

            if (success)
            {
                hasProjectChanged = false;
                hasUserCancelledLastOperation = false;
            }
            return success;
        }

        /// <summary>
        /// Converts the absolute path to relative path of session file.
        /// </summary>
        /// <param name="projectPath">The project file path</param>
        /// <param name="filePath">The project file path</param>
        /// <returns>The relative path of session file.</returns>
        public string GetRelativePath(string projectPath, string filePath)
        {
            projectPath = projectPath.TrimEnd(new char[] { Path.DirectorySeparatorChar });
            filePath = filePath.TrimEnd(new char[] { Path.DirectorySeparatorChar });

            string relativePath = "";
            string relativePathPart1 = "";
            string relativePathPart2 = "";

            if (projectPath == filePath)
            {
                return ".\\";
            }

            string[] projectPathArray = projectPath.Split(
                new char[] { Path.DirectorySeparatorChar });
            string[] filePathArray = filePath.Split(
                new char[] { Path.DirectorySeparatorChar });

            int maxFolderCount = (projectPathArray.Length < filePathArray.Length) ?
                filePathArray.Length : projectPathArray.Length;

            if (filePathArray[0] == projectPathArray[0])
            {
                for (int index = 0; index < maxFolderCount; index++)
                {
                    if (index < projectPathArray.Length &&
                        index < filePathArray.Length)
                    {
                        if (projectPathArray[index] != filePathArray[index])
                        {
                            relativePathPart1 += ".." + Path.DirectorySeparatorChar;
                            relativePathPart2 += filePathArray[index] + Path.DirectorySeparatorChar;
                        }
                    }
                    else if (index < projectPathArray.Length)
                    {
                        relativePathPart1 += ".." + Path.DirectorySeparatorChar;
                    }

                    else if (index < filePathArray.Length)
                    {
                        relativePathPart2 += filePathArray[index] + Path.DirectorySeparatorChar;
                    }
                }
            }

            relativePath = relativePathPart1 + relativePathPart2;
            if (relativePath.Length == 0)
            {
                relativePath = filePath + Path.DirectorySeparatorChar;
            }
            else
            {
                relativePath = "." + Path.DirectorySeparatorChar + relativePath;
            }

            return relativePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasUserCancelledLastOperation()
        {
            return (hasUserCancelledLastOperation);
        }

        /// <summary>
        /// Method to close the Project.
        /// </summary>
        /// <param name="saveChanges"></param>
        public void Close(bool saveChanges)
        {
            hasUserCancelledLastOperation = false;
            if (AreProjectOrSessionsChanged() && saveChanges)
            {
                if (tempButtonCancel)
                {
                    hasUserCancelledLastOperation = true;
                }
            }
            if (!hasUserCancelledLastOperation)
            {
                hasProjectChanged = false;
                hasUserCancelledLastOperation = false;
                isProjectConstructed = false;
                sessions.Clear();
                projectFileName = "";
            }
        }

        /// <summary>
        /// Construct a new empty Project that has not been saved yet to the supplied Project file.
        /// 
        /// Precondition: no project is constructed.
        /// </summary>
        /// <param name="theProjectFileName">The name of the Project file.</param>
        public void New(string theProjectFileName)
        {
            Debug.Assert(isProjectConstructed == false, "Project is constructed when calling Project.New(...)");

            hasProjectChanged = true;
            hasUserCancelledLastOperation = false;
            isProjectConstructed = true;
            sessions.Clear();
            projectFileName = theProjectFileName;
        }

        /// <summary>
        /// Method to add a session to an existing Project.
        /// </summary>
        /// <param name="theSessionFullFileName"></param>
        public void AddSession(string theSessionFullFileName)
        {
            try
            {
                Session theLoadedSession = sessionManager.CreateSession(theSessionFullFileName);
                if (theLoadedSession == null)
                {
                    string msg =
                        string.Format(
                        "Session Type is not supported by DVT");
                    MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                if (theLoadedSession != null)
                {
                    theLoadedSession.ParentProject = this;
                    theLoadedSession.SessionFileName = theSessionFullFileName;
                    // Search the sessions loaded in project file for same ResultsRootDirectory 
                    foreach (Session session in sessions)
                    {
                        if (session.ResultsRootDirectory == theLoadedSession.ResultsRootDirectory)
                        {
                            session.ParentProject = this;
                            string msg =
                                string.Format(
                                "The {0} have the same results directory \n as session {1} in the project.",
                                theLoadedSession.SessionFileName, session.SessionFileName);

                            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        }
                    }
                    Sessions.Add(theLoadedSession);
                    hasProjectChanged = true;
                    //isProjectConstructed = true;
                }
            }
            catch (Exception ex)
            {
                //isProjectConstructed = false;
                hasProjectChanged = false;
                Exception excp;
                throw excp = new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Method to determine whether the project or session has been changed.
        /// </summary>
        /// <returns>True : if project or session has been changed else false.</returns>
        public bool AreProjectOrSessionsChanged()
        {
            bool areChanged = false;

            if (HasProjectChanged)
            {
                areChanged = true;
            }
            else if (GetNumberOfChangedSessions() != 0)
            {
                areChanged = true;
            }

            return (areChanged);
        }

        /// <summary>
        /// Method to determine whether a particular session exist in a list of given sessions.
        /// </summary>
        /// <param name="session_file"></param>
        /// <returns></returns>
        public bool ContainsSession(string session_file)
        {
            bool theReturnValue = false;

            foreach (Session session in sessions)
            {
                if (session.SessionFileName == session_file)
                {
                    theReturnValue = true;
                }
            }

            return theReturnValue;
        }

        /// <summary>
        /// Method to determine the number of changed sessions
        /// </summary>
        /// <returns>Returns the number of changed sessions</returns>
        private int GetNumberOfChangedSessions()
        {
            int theNumberOfChangedSessions = 0;

            for (int i = 0; i < sessions.Count; i++)
            {
                Session tempSession = (Session)sessions[i];
                tempSession.ParentProject = this;
                if (tempSession.GetSessionChanged(tempSession))
                {
                    theNumberOfChangedSessions++;
                }
            }
            nosOfSessionsChanged = theNumberOfChangedSessions;

            return (theNumberOfChangedSessions);
        }

        #endregion
    }
}
