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
using System.Configuration;
using System.Text;
using System.IO;



namespace DvtkApplicationLayer.StoredFiles
{
    /// <summary>
    /// Specifies a group of files and determines if and when files from this group will be removed.
    /// </summary>
    /// <remarks>
    /// Instances of this, or a derived, class are intended to (partly) be configured be the user,
    /// making use of a dedicated User Interface.
    /// </remarks>
    public class UserConfigurableFileGroup : FileGroup
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property description.
        /// </summary>
        private String description;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private UserConfigurableFileGroup() : base("No name")
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of this instance, which must be unique for each UserConfigurableFileGroup instance used within an application.</param>
        public UserConfigurableFileGroup(String name) : base(name)
        {
            // Do nothing.
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets or sets a boolean indicating if permission needs to be asked to removed files
        /// during the current application run.
        /// </summary>
        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public override bool AskPermissionToRemoveCurrentApplicationRun
        {
            get
            {
                return ((bool)this["AskPermissionToRemoveCurrentApplicationRun"]);
            }

            set
            {
                this["AskPermissionToRemoveCurrentApplicationRun"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if permission needs to be asked to removed files
        /// during the previous application runs.
        /// </summary>
        [UserScopedSetting()]
        [DefaultSettingValue("false")]
        public override bool AskPermissionToRemovePreviousApplicationRuns
        {
            get
            {
                return ((bool)this["AskPermissionToRemovePreviousApplicationRuns"]);
            }

            set
            {
                this["AskPermissionToRemovePreviousApplicationRuns"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if this instance has been configured in the past.
        /// </summary>
        /// <remarks>
        /// This setting will be saved and loaded as a user setting on disc.
        /// <br></br><br></br>
        /// DO NOT USE THIS PROPERTY OUTSIDE THE LIBRARY.
        /// </remarks>
        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool IsConfigured
        {
            get
            {
                return (bool)this["IsConfigured"];
            }

            set
            {
                this["IsConfigured"] = value;
            }
        }

        /// <summary>
        /// Days.
        /// </summary>
        [UserScopedSetting()]
        [DefaultSettingValue("365")]
        public override UInt16 DaysPreviousApplicationRuns
        {
            get
            {
                return ((UInt16)this["DaysPreviousApplicationRuns"]);
            }

            set
            {
                this["DaysPreviousApplicationRuns"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the description of this FileGroup.
        /// </summary>
        /// <remarks>
        /// This description will be displayed to the user.
        /// </remarks>
        public String Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Gets or sets the directory this instance refers to.
        /// </summary>
        /// <remarks>
        /// This setting will be saved and loaded as a user setting on disc.
        /// </remarks>
        [UserScopedSetting]
        [DefaultSettingValue("")]
        public override String Directory
        {
            get
            {
                String directory = (String)this["Directory"];

                if (directory.Length == 0)
                // Directory has not yet been user defined...
                {
                    directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DVTk\\" + FileGroups.ApplicationName);
                    directory = Path.Combine(directory, DefaultFolder);
                }

                return directory;
            }

            set
            {
                this["Directory"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the remove mode for files created during the current application run.
        /// </summary>
        [UserScopedSetting()]
        [DefaultSettingValue("Keep")]
        public override RemoveMode RemoveModeCurrentApplicationRun
        {
            get
            {
                return (RemoveMode)this["RemoveModeCurrentApplicationRun"];
            }

            set
            {
                this["RemoveModeCurrentApplicationRun"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the remove mode for files created during previous application runs.
        /// </summary>
        [UserScopedSetting()]
        [DefaultSettingValue("Keep")]
        public override RemoveMode RemoveModePreviousApplicationRuns
        {
            get
            {
                return (RemoveMode)this["RemoveModePreviousApplicationRuns"];
            }

            set
            {
                this["RemoveModePreviousApplicationRuns"] = value;
            }
        }

        /// <summary>
        /// Size in MB.
        /// </summary>
        [UserScopedSetting()]
        [DefaultSettingValue("1024")]
        public override UInt16 SizePreviousApplicationRuns
        {
            get
            {
                return ((UInt16)this["SizePreviousApplicationRuns"]);
            }

            set
            {
                this["SizePreviousApplicationRuns"] = value;
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Open Windows explorer with the directory specified in this instance.
        /// </summary>
        public void Explore()
        {
            System.Diagnostics.Process theProcess = new System.Diagnostics.Process();

            theProcess.StartInfo.FileName = "Explorer.exe";
            theProcess.StartInfo.Arguments = Directory;
            theProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;

            theProcess.Start();
        }
    }
}
