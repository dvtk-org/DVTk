// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright  2009 DVTk
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace DvtkApplicationLayer.StoredFiles
{
    /// <summary>
    /// Specifies a group of files and determines if and when files from this group will be removed.
    /// </summary>
    abstract public class FileGroup : ApplicationSettingsBase
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property DefaultDirectory.
        /// </summary>
        protected String defaultFolder = "";

        /// <summary>
        /// The FileGroups instance this FileGroup instance belongs to.
        /// </summary>
        private FileGroups fileGroups = null;

        /// <summary>
        /// See property Name.
        /// </summary>
        private String name = "";

        /// <summary>
        /// See property Wildcards.
        /// </summary>
        private StringCollection wildcards = new StringCollection();



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private FileGroup()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of this instance.</param>
        public FileGroup(String name) : base(name)
        {
            this.name = name;
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets or sets a boolean indicating if permission needs to be asked to removed files
        /// during the current application run.
        /// </summary>
        public abstract bool AskPermissionToRemoveCurrentApplicationRun
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a boolean indicating if permission needs to be asked to removed files
        /// during the previous application runs.
        /// </summary>
        public abstract bool AskPermissionToRemovePreviousApplicationRuns
        {
            get;
            set;
        }

        /// <summary>
        /// Days.
        /// </summary>
        public abstract UInt16 DaysPreviousApplicationRuns
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default folder (set by application code and not user configurable).
        /// </summary>
        /// <remarks>
        /// The default directory will be constructed as follows:
        /// My Documents directory + DVTk + application name + default folder.
        /// <br></br><br></br>
        /// This default determined directory can be overriden using the Directory set property.
        /// </remarks>
        public String DefaultFolder
        {
            get
            {
                return (this.defaultFolder);
            }

            set
            {

                this.defaultFolder = value;
            }
        }

        /// <summary>
        /// Gets or sets the directory this instance refers to.
        /// </summary>
        public abstract String Directory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the FileGroups instance this FileGroup instance belongs to.
        /// </summary>
        internal FileGroups FileGroups
        {
            get
            {
                return (this.fileGroups);
            }

            set
            {
                this.fileGroups = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of this instance (used for serialization and deserialization).
        /// </summary>
        public String Name
        {
            get
            {
                return (this.name);
            }
        }

        /// <summary>
        /// Gets or sets the remove mode for files created during the current application run.
        /// </summary>
        public abstract RemoveMode RemoveModeCurrentApplicationRun
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the remove mode for files created during previous application runs.
        /// </summary>
        public abstract RemoveMode RemoveModePreviousApplicationRuns
        {
            get;
            set;
        }

        /// <summary>
        /// Size in MB.
        /// </summary>
        public abstract UInt16 SizePreviousApplicationRuns
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file wildcards that determines which files this instance refers to.
        /// </summary>
        public StringCollection Wildcards
        {
            get
            {
                return (this.wildcards);
            }

            set
            {
                this.wildcards = value;
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Determine all files that need to be removed.
        /// </summary>
        /// <returns></returns>
        public StringCollection DetermineFilesToRemove()
        {
            StringCollection filesToRemove = new StringCollection();

            if ((RemoveModeCurrentApplicationRun == RemoveMode.Keep) && (RemoveModePreviousApplicationRuns == RemoveMode.Keep))
            {
                // Do nothing.
            }
            else
            {
                StringCollection filesCurrentApplicationRun = new StringCollection();
                Collection<FileInfo> filesPreviousApplicationRuns = new Collection<FileInfo>();

                GetFiles(filesCurrentApplicationRun, filesPreviousApplicationRuns);

                StringCollection filesToRemoveCurrentApplicationRun = DetermineFilesToRemoveCurrentApplicationRun(filesCurrentApplicationRun);

                foreach (String fileToRemoveCurrentApplicationRun in filesToRemoveCurrentApplicationRun)
                {
                    filesToRemove.Add(fileToRemoveCurrentApplicationRun);
                }

                StringCollection filesToRemovePreviousApplicationRuns = DetermineFilesToRemovePreviousApplicationRuns(filesPreviousApplicationRuns);

                foreach (String fileToRemovePreviousApplicationRuns in filesToRemovePreviousApplicationRuns)
                {
                    filesToRemove.Add(fileToRemovePreviousApplicationRuns);
                }
            }

            return (filesToRemove);
        }

        /// <summary>
        /// Checks if the directory for this FileGroup exist.
        /// If not, it will be created.
        /// </summary>
        public void CreateDirectory()
        {
            if (Directory.Length > 0)
            {
                if (!System.IO.Directory.Exists(Directory))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(Directory);
                    }
                    catch
                    {
                        MessageBox.Show("Unable to create directory \"" + Directory + "\"");
                    }
                }
            }
        }

        private StringCollection DetermineFilesToRemoveCurrentApplicationRun(StringCollection filesCurrentApplicationRun)
        {
            StringCollection filesToRemoveCurrentApplicationRun = new StringCollection();

            if (filesCurrentApplicationRun.Count > 0)
            {
                switch (RemoveModeCurrentApplicationRun)
                {
                    case RemoveMode.Keep:
                        // Do nothing.
                        break;

                    case RemoveMode.Remove:
                        foreach (String fullFileName in filesCurrentApplicationRun)
                        {
                            filesToRemoveCurrentApplicationRun.Add(fullFileName);
                        }

                        if (AskPermissionToRemoveCurrentApplicationRun && (filesToRemoveCurrentApplicationRun.Count > 0))
                        {
                            DialogResult dialogResult = MessageBox.Show("Remove all " + Name + " from the current application run?", "Confirm removal", MessageBoxButtons.YesNo);

                            if (dialogResult == DialogResult.No)
                            {
                                filesToRemoveCurrentApplicationRun = new StringCollection();
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            return (filesToRemoveCurrentApplicationRun);
        }

        private StringCollection DetermineFilesToRemovePreviousApplicationRuns(Collection<FileInfo> filesPreviousApplicationRuns)
        {
            StringCollection filesToRemovePreviousApplicationRuns = new StringCollection();

            switch (RemoveModePreviousApplicationRuns)
            {
                case RemoveMode.Keep:
                    // Do nothing.
                    break;

                case RemoveMode.Remove:
                    foreach (FileInfo fileInfo in filesPreviousApplicationRuns)
                    {
                        filesToRemovePreviousApplicationRuns.Add(fileInfo.FullName);
                    }

                    if (AskPermissionToRemovePreviousApplicationRuns && (filesToRemovePreviousApplicationRuns.Count > 0))
                    {
                        DialogResult dialogResult = MessageBox.Show("Remove all " + Name + " from the previous application runs?", "Confirm removal", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.No)
                        {
                            filesToRemovePreviousApplicationRuns = new StringCollection();
                        }
                    }

                    break;

                case RemoveMode.RemoveIfOlderThan:
                    foreach (FileInfo fileInfo in filesPreviousApplicationRuns)
                    {
                        if ((fileInfo.LastWriteTime + new TimeSpan(DaysPreviousApplicationRuns, 0, 0, 0)) < FileGroups.ConstructionTime)
                        {
                            filesToRemovePreviousApplicationRuns.Add(fileInfo.FullName);
                        }
                    }

                    if (AskPermissionToRemovePreviousApplicationRuns && (filesToRemovePreviousApplicationRuns.Count > 0))
                    {
                        DialogResult dialogResult = MessageBox.Show("Remove all " + Name + " from the previous application runs that are older than " + DaysPreviousApplicationRuns.ToString() + " days?", "Confirm removal", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.No)
                        {
                            filesToRemovePreviousApplicationRuns = new StringCollection();
                        }
                    }

                    break;

                case RemoveMode.RemoveOldestWhenTotalSizeLargerThan:
                    long totalSizeInBytes = 0;
                    long maximumSizeInBytes = SizePreviousApplicationRuns * 1024 * 1024;
                    int removeFilesUntilIndex = -1;

                    // Newest files have the highest index, so start from the higher index and go to the lower indices.
                    for (int index = filesPreviousApplicationRuns.Count - 1; index >= 0; index--)
                    {
                        FileInfo fileInfo = filesPreviousApplicationRuns[index];

                        totalSizeInBytes += fileInfo.Length;

                        if (totalSizeInBytes > maximumSizeInBytes)
                        {
                            removeFilesUntilIndex = index;
                            break;
                        }
                    }

                    for (int indexFileToRemove = 0; indexFileToRemove <= removeFilesUntilIndex; indexFileToRemove++)
                    {
                        filesToRemovePreviousApplicationRuns.Add(filesPreviousApplicationRuns[indexFileToRemove].FullName);
                    }

                    if (AskPermissionToRemovePreviousApplicationRuns && (filesToRemovePreviousApplicationRuns.Count > 0))
                    {
                        DialogResult dialogResult = MessageBox.Show("Remove oldest " + Name + " from the previous application runs (because exceeding " + SizePreviousApplicationRuns.ToString() + " MB)?", "Confirm removal", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.No)
                        {
                            filesToRemovePreviousApplicationRuns = new StringCollection();
                        }
                    }

                    break;

                default:
                    break;
            }

            return (filesToRemovePreviousApplicationRuns);
        }

        /// <summary>
        /// Get all files in the Directory specified by the wildcards and devide them in two collections.
        /// </summary>
        /// <param name="filesCurrentApplicationRun">All files from the current application run.</param>
        /// <param name="filesPreviousApplicationRuns">All files from the previous application runs.</param>
        private void GetFiles(StringCollection filesCurrentApplicationRun, Collection<FileInfo> filesPreviousApplicationRuns)
        {
            SortedDictionary<String, FileInfo> allFiles = new SortedDictionary<string, FileInfo>(StringComparer.InvariantCultureIgnoreCase);

            DirectoryInfo directoryInfo = new DirectoryInfo(Directory);

            foreach (String wildcard in Wildcards)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles(wildcard);

                foreach (FileInfo fileInfo in fileInfos)
                {
                    allFiles.Add(fileInfo.LastWriteTime.ToString("u") + fileInfo.Name, fileInfo);
                }
            }

            foreach (KeyValuePair<String, FileInfo> keyValuePair in allFiles)
            {
                if (keyValuePair.Value.LastWriteTime < FileGroups.ConstructionTime)
                {
                    filesPreviousApplicationRuns.Add(keyValuePair.Value);
                }
                else
                {
                    filesCurrentApplicationRun.Add(keyValuePair.Value.FullName);
                }
            }
        }

        /// <summary>
        /// Remove all files in the Directory specified by the wildcards.
        /// </summary>
        public void RemoveAllFiles()
        {
            bool problemWhileRemoving = false;

            DirectoryInfo directoryInfo = new DirectoryInfo(Directory);

            foreach (String wildcard in Wildcards)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles(wildcard);

                foreach (FileInfo fileInfo in fileInfos)
                {
                    try
                    {
                        File.Delete(fileInfo.FullName);
                    }
                    catch (Exception)
                    {
                        problemWhileRemoving = true;
                    }
                }
            }

            if (problemWhileRemoving)
            {
                MessageBox.Show("Not all files could be removed!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
