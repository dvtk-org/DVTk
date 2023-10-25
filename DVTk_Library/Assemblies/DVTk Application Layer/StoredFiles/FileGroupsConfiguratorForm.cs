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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace DvtkApplicationLayer.StoredFiles
{
    internal partial class FileGroupsConfiguratorForm : Form
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Contains all fileGroupConfiguratorUserControls that are created by this form.
        /// </summary>
        private Collection<FileGroupConfiguratorUserControl> fileGroupConfiguratorUserControls = new Collection<FileGroupConfiguratorUserControl>();

        /// <summary>
        /// The FileGroups instance this form is created for.
        /// </summary>
        private FileGroups fileGroups = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default construction.
        /// </summary>
        private FileGroupsConfiguratorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileGroups">The file groups to configure for which this form will be created.</param>
        public FileGroupsConfiguratorForm(FileGroups fileGroups)
        {
            InitializeComponent();

            this.fileGroups = fileGroups;

            foreach (UserConfigurableFileGroup userConfigurableFileGroup in fileGroups.UserConfigurableFileGroups)
            {
                TabPage tabPage = CreateTabPage(userConfigurableFileGroup);

                tabControl.Controls.Add(tabPage);
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Called when the Cancel button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Called when the OK button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            Exception firstException = null;

            foreach (FileGroupConfiguratorUserControl fileGroupConfiguratorUserControl in this.fileGroupConfiguratorUserControls)
            {
                try
                {
                    fileGroupConfiguratorUserControl.CopySettingsToUserConfigurableFileGroupAndSave();
                }
                catch (Exception exception)
                {
                    firstException = exception;
                }
            }

            if (firstException != null)
            {
                MessageBox.Show("Exception(s) occured while saving the settings. Some of the settings may not have been stored.\n\nFirst exception:\n" + firstException.Message);
            }

            Close();
        }

        /// <summary>
        /// Create a new tab page for the tab control.
        /// </summary>
        /// <param name="userConfigurableFileGroup">The FileGroup instance to create a new tab page for.</param>
        /// <returns></returns>
        private TabPage CreateTabPage(UserConfigurableFileGroup userConfigurableFileGroup)
        {
            //
            // Create new fileGroupConfiguratorUserControl instance.
            //

            FileGroupConfiguratorUserControl fileGroupConfiguratorUserControl = new FileGroupConfiguratorUserControl(userConfigurableFileGroup);

            fileGroupConfiguratorUserControl.Dock = System.Windows.Forms.DockStyle.Fill;

            fileGroupConfiguratorUserControls.Add(fileGroupConfiguratorUserControl);



            //
            // Create new tab page containing the newly created FileGroup instance.
            //

            TabPage tabPage = new TabPage();

            tabPage.Text = userConfigurableFileGroup.Name;
            tabPage.Controls.Add(fileGroupConfiguratorUserControl);

            return (tabPage);
        }
    }
}