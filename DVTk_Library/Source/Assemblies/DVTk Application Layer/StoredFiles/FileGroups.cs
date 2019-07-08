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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace DvtkApplicationLayer.StoredFiles
{
    /// <summary>
    /// Contains and controls all contained FileGroup instances.
    /// </summary>
    public class FileGroups
    {
        /// <summary>
        /// The name of the application, needed for display in the FileGroupsConfiguratorForm.
        /// </summary>
        private String applicationName = "";

        /// <summary>
        /// The internal collection containing the different FileGroup instances.
        /// </summary>
        private Collection<FileGroup> collection = new Collection<FileGroup>();

        private DateTime constructionTime = DateTime.MinValue;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private FileGroups()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationName">The name of the application, needed for display in the FileGroupsConfiguratorForm.</param>
        public FileGroups(String applicationName)
        {
            this.applicationName = applicationName;
            this.constructionTime = DateTime.Now;
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the application name.
        /// </summary>
        internal String ApplicationName
        {
            get
            {
                return (this.applicationName);
            }
        }

        /// <summary>
        /// Gets the time this instance was constructed.
        /// </summary>
        internal DateTime ConstructionTime
        {
            get
            {
                return (this.constructionTime);
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if all contained user configurable file groups have
        /// been configured.
        /// </summary>
        public bool IsConfigured
        {
            get
            {
                bool isConfigured = true;

                foreach (UserConfigurableFileGroup userConfigurableFileGroup in UserConfigurableFileGroups)
                {
                    if (!userConfigurableFileGroup.IsConfigured)
                    {
                        isConfigured = false;
                        break;
                    }
                }

                return (isConfigured);
            }

            set
            {
                foreach (UserConfigurableFileGroup userConfigurableFileGroup in UserConfigurableFileGroups)
                {
                    userConfigurableFileGroup.IsConfigured = true;
                }
            }
        }

        /// <summary>
        /// Gets all user configurable file group instances.
        /// </summary>
        internal Collection<UserConfigurableFileGroup> UserConfigurableFileGroups
        {
            get
            {
                Collection<UserConfigurableFileGroup> userConfigurableFileGroups = new Collection<UserConfigurableFileGroup>();

                foreach (FileGroup fileGroup in this.collection)
                {
                    if (fileGroup is UserConfigurableFileGroup)
                    {
                        userConfigurableFileGroups.Add(fileGroup as UserConfigurableFileGroup);
                    }
                }

                return (userConfigurableFileGroups);
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Add a FileGroup instance.
        /// </summary>
        /// <remarks>
        /// The supplied FileGroup instance may only be added to one FileGroups instance.
        /// </remarks>
        /// <param name="fileGroup">The FileGroup instance to add.</param>
        public void Add(FileGroup fileGroup)
        {
            this.collection.Add(fileGroup);
            fileGroup.FileGroups = this;
        }

        /// <summary>
        /// Check if the stored files options have been configured. If not, the user is given a choice to do now or use the default options.
        /// </summary>
        /// <param name="userInterfaceDescription">Description how to configure the stored files options in the User Interface.</param>
        public void CheckIsConfigured(String userInterfaceDescription)
        {
            if (!IsConfigured)
            {
                /*DialogResult dialogResult = MessageBox.Show(
                    "The stored files options have not yet been fully configured. Do you want to do this now?\n\nClick Yes to configure now\nClick No to use the default options.\n\nIt may be configured at a later time using the " + userInterfaceDescription + ".",
                    "Store files options",
                    MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    ShowDialogFileGroupsConfigurator();
                }*/

                IsConfigured = true;
                Save();
            }
        }

        /// <summary>
        /// Checks if the directories for all contained FileGroups exist.
        /// If not, they will be created.
        /// </summary>
        public void CreateDirectories()
        {
            foreach (FileGroup fileGroup in this.collection)
            {
                fileGroup.CreateDirectory();
            }
        }

        /// <summary>
        /// Cleanup the system by removing files that are allowed to be removed according to the
        /// contained FileGroup instances.
        /// </summary>
        public void RemoveFiles()
        {
            foreach (FileGroup fileGroup in this.collection)
            {
                StringCollection filesToRemove = fileGroup.DetermineFilesToRemove();

                foreach (String fileToRemove in filesToRemove)
                {
                    try
                    {
                        File.Delete(fileToRemove);
                    }
                    catch
                    {
                        // Do nothing. File may be locked.
                    }
                }
            }
        }

        /// <summary>
        /// Save all user configurable settings.
        /// </summary>
        public void Save()
        {
            foreach (UserConfigurableFileGroup userConfigurableFileGroup in UserConfigurableFileGroups)
            {
                userConfigurableFileGroup.Save();
            }
        }

        /// <summary>
        /// Display a Form in which the user configurable FileGroup instances can be configured.
        /// </summary>
        public DialogResult ShowDialogFileGroupsConfigurator()
        {
            FileGroupsConfiguratorForm fileGroupsConfiguratorForm = new FileGroupsConfiguratorForm(this);

            DialogResult dialogResult = fileGroupsConfiguratorForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                Save();
            }

            return (dialogResult);
        }
    }
}
