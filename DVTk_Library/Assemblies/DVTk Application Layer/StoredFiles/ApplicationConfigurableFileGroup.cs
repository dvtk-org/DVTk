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
using System.Text;



namespace DvtkApplicationLayer.StoredFiles
{
    /// <summary>
    /// Specifies a group of files and determines if and when files from this group will be removed.
    /// </summary>
    /// <remarks>
    /// Instances of this, or a derived, class are intended to only be configured within the application,
    /// that is without supplying an User Interface to do so.
    /// </remarks>
    public class ApplicationConfigurableFileGroup : FileGroup
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property AskPermissionToRemoveCurrentApplicationRun.
        /// </summary>
        private bool askPermissionToRemoveCurrentApplicationRun = false;

        /// <summary>
        /// See property AskPermissionToRemovePreviousApplicationRuns.
        /// </summary>
        private bool askPermissionToRemovePreviousApplicationRuns = false;

        /// <summary>
        /// See property DaysPreviousApplicationRuns.
        /// </summary>
        private uint daysPreviousApplicationRuns = 365;

        /// <summary>
        /// See property Directory.
        /// </summary>
        private String directory = "";

        /// <summary>
        /// See property RemoveModeCurrentApplicationRun.
        /// </summary>
        RemoveMode removeModeCurrentApplicationRun = RemoveMode.Keep;

        /// <summary>
        /// See property RemoveModePreviousApplicationRuns.
        /// </summary>
        RemoveMode removeModePreviousApplicationRuns = RemoveMode.Keep;

        /// <summary>
        /// See property SizePreviousApplicationRuns.
        /// </summary>
        private uint sizePreviousApplicationRuns = 1024;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private ApplicationConfigurableFileGroup() : base("No name")
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of this instance, which must be unique for each UserConfigurableFileGroup instance used within an application.</param>
        public ApplicationConfigurableFileGroup(String name) : base(name)
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
        public override bool AskPermissionToRemoveCurrentApplicationRun
        {
            get
            {
                return (this.askPermissionToRemoveCurrentApplicationRun);
            }

            set
            {
                this.askPermissionToRemoveCurrentApplicationRun = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if permission needs to be asked to removed files
        /// during the previous application runs.
        /// </summary>
        public override bool AskPermissionToRemovePreviousApplicationRuns
        {
            get
            {
                return (this.askPermissionToRemovePreviousApplicationRuns);
            }

            set
            {
                this.askPermissionToRemovePreviousApplicationRuns = value;
            }
        }

        /// <summary>
        /// Days.
        /// </summary>
        public override UInt16 DaysPreviousApplicationRuns
        {
            get
            {
                return ((UInt16)this.daysPreviousApplicationRuns);
            }

            set
            {
                this.daysPreviousApplicationRuns = value;
            }
        }

        /// <summary>
        /// Gets or sets the directory this instance refers to.
        /// </summary>
        public override String Directory
        {
            get
            {
                String directory = this.directory;

                if (directory.Length == 0)
                {

                }

                return (directory);
            }

            set
            {
                this.directory = value;
            }
        }

        /// <summary>
        /// Gets or sets the remove mode for files created during the current application run.
        /// </summary>
        public override RemoveMode RemoveModeCurrentApplicationRun
        {
            get
            {
                return (this.removeModeCurrentApplicationRun);
            }

            set
            {
                this.removeModeCurrentApplicationRun = value;
            }
        }

        /// <summary>
        /// Gets or sets the remove mode for files created during previous application runs.
        /// </summary>
        public override RemoveMode RemoveModePreviousApplicationRuns
        {
            get
            {
                return (this.removeModePreviousApplicationRuns);
            }

            set
            {
                this.removeModePreviousApplicationRuns = value;
            }
        }

        /// <summary>
        /// Size in MB.
        /// </summary>
        public override UInt16 SizePreviousApplicationRuns
        {
            get
            {
                return ((UInt16)this.sizePreviousApplicationRuns);
            }

            set
            {
                this.sizePreviousApplicationRuns = value;
            }
        }
    }
}
