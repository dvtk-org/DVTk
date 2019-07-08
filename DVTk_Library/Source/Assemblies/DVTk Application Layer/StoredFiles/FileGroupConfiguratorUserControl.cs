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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;



namespace DvtkApplicationLayer.StoredFiles
{
    /// <summary>
    /// User control that enables the user to configure one FileGroup instance.
    /// </summary>
    internal partial class FileGroupConfiguratorUserControl : UserControl
    {
        //
        // - Fields -
        //

        /// <summary>
        /// Needed to prevent an infinite loop of radioButton..._CheckedChanged calls.
        /// </summary>
        private bool radioButtonCheckedChangedEventHandlersEnabled = true;

        /// <summary>
        /// The FileGroup instance this user control operates on.
        /// </summary>
        private UserConfigurableFileGroup userConfigurableFileGroup = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private FileGroupConfiguratorUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userConfigurableFileGroup">The FileGroup instance this user control operates on.</param>
        public FileGroupConfiguratorUserControl(UserConfigurableFileGroup userConfigurableFileGroup)
        {
            this.userConfigurableFileGroup = userConfigurableFileGroup;

            InitializeComponent();

            this.labelDescription.Text = userConfigurableFileGroup.Description;

            this.labelWildCards.Text = "";
            for (int index = 0; index < userConfigurableFileGroup.Wildcards.Count; index++)
            {
                if (index == 0)
                {
                    this.labelWildCards.Text += userConfigurableFileGroup.Wildcards[index];
                }
                else
                {
                    this.labelWildCards.Text += ", " + userConfigurableFileGroup.Wildcards[index];
                }
            }

            this.groupBoxCurrentApplicationRun.Text = "Do the following with these files stored during the last run of the " + this.userConfigurableFileGroup.FileGroups.ApplicationName;
            this.groupBoxPreviousApplicationRuns.Text = "Do the following with these files stored during the previous runs of the " + this.userConfigurableFileGroup.FileGroups.ApplicationName;

            CopySettingsFromUserConfigurableFileGroup();
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets or sets the remove mode for the current application run on the UI.
        /// </summary>
        public RemoveMode RemoveModeCurrentApplicationRun
        {
            get
            {
                RemoveMode removeMode = RemoveMode.Keep;

                if (radioButtonCurrentApplicationRunKeep.Checked)
                {
                    removeMode = RemoveMode.Keep;
                }
                else if (radioButtonCurrentApplicationRunRemove.Checked)
                {
                    removeMode = RemoveMode.Remove;
                }

                return (removeMode);
            }

            set
            {
                switch (value)
                {
                    case RemoveMode.Keep:
                        this.radioButtonCheckedChangedEventHandlersEnabled = false;
                        radioButtonCurrentApplicationRunKeep.Checked = true;
                        this.radioButtonCheckedChangedEventHandlersEnabled = true;
                        
                        checkBoxCurrentApplicationRunAskPermissionToRemove.Enabled = false;
                        break;

                    case RemoveMode.Remove:
                        this.radioButtonCheckedChangedEventHandlersEnabled = false;
                        radioButtonCurrentApplicationRunRemove.Checked = true;
                        this.radioButtonCheckedChangedEventHandlersEnabled = true;

                        checkBoxCurrentApplicationRunAskPermissionToRemove.Enabled = true;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the remove mode for the previous application runs on the UI.
        /// </summary>
        public RemoveMode RemoveModePreviousApplicationRuns
        {
            get
            {
                RemoveMode removeMode = RemoveMode.Keep;
                
                if (radioButtonPreviousApplicationRunsKeep.Checked)
                {
                    removeMode = RemoveMode.Keep;
                }
                else if (radioButtonPreviousApplicationRunsRemove.Checked)
                {
                    removeMode = RemoveMode.Remove;
                }
                else if (radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.Checked)
                {
                    removeMode = RemoveMode.RemoveOldestWhenTotalSizeLargerThan;
                }
                else if (radioButtonPreviousApplicationRunsRemoveIfOlderThan.Checked)
                {
                    removeMode = RemoveMode.RemoveIfOlderThan;
                }

                return (removeMode);
            }

            set
            {
                switch (value)
                {        
                    case RemoveMode.Keep:
                        this.radioButtonCheckedChangedEventHandlersEnabled = false;
                        radioButtonPreviousApplicationRunsKeep.Checked = true;
                        this.radioButtonCheckedChangedEventHandlersEnabled = true;

                        textBoxPreviousApplicationRunsSize.Enabled = false;
                        textBoxPreviousApplicationRunsDays.Enabled = false;
                        checkBoxPreviousApplicationRunsAskPermissionToRemove.Enabled = false;

                        break;
                    case RemoveMode.Remove:
                        this.radioButtonCheckedChangedEventHandlersEnabled = false;
                        radioButtonPreviousApplicationRunsRemove.Checked = true;
                        this.radioButtonCheckedChangedEventHandlersEnabled = true;

                        textBoxPreviousApplicationRunsSize.Enabled = false;
                        textBoxPreviousApplicationRunsDays.Enabled = false;
                        checkBoxPreviousApplicationRunsAskPermissionToRemove.Enabled = true;

                        break;
                    case RemoveMode.RemoveIfOlderThan:
                        this.radioButtonCheckedChangedEventHandlersEnabled = false;
                        radioButtonPreviousApplicationRunsRemoveIfOlderThan.Checked = true;
                        this.radioButtonCheckedChangedEventHandlersEnabled = true;

                        textBoxPreviousApplicationRunsSize.Enabled = false;
                        textBoxPreviousApplicationRunsDays.Enabled = true;
                        checkBoxPreviousApplicationRunsAskPermissionToRemove.Enabled = true;

                        break;
                    case RemoveMode.RemoveOldestWhenTotalSizeLargerThan:
                        this.radioButtonCheckedChangedEventHandlersEnabled = false;
                        radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.Checked = true;
                        this.radioButtonCheckedChangedEventHandlersEnabled = true;

                        textBoxPreviousApplicationRunsSize.Enabled = true;
                        textBoxPreviousApplicationRunsDays.Enabled = false;
                        checkBoxPreviousApplicationRunsAskPermissionToRemove.Enabled = true;
                        break;

                    default:
                        break;
                }
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Called when the browse button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonDirectoryBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.SelectedPath = this.textBoxDirectory.Text;

            DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.textBoxDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Called when the explore button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonDirectoryExplore_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process theProcess = new System.Diagnostics.Process();

            theProcess.StartInfo.FileName = "Explorer.exe";
            theProcess.StartInfo.Arguments = this.textBoxDirectory.Text;

            theProcess.Start();
        }

        /// <summary>
        /// Copy settings from the FileGroup instance to this UserControl.
        /// </summary>
        private void CopySettingsFromUserConfigurableFileGroup()
        {
            RemoveModeCurrentApplicationRun = this.userConfigurableFileGroup.RemoveModeCurrentApplicationRun;
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.Checked = this.userConfigurableFileGroup.AskPermissionToRemoveCurrentApplicationRun;

            RemoveModePreviousApplicationRuns = this.userConfigurableFileGroup.RemoveModePreviousApplicationRuns;
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.Checked = this.userConfigurableFileGroup.AskPermissionToRemovePreviousApplicationRuns;
            this.textBoxPreviousApplicationRunsDays.Text = this.userConfigurableFileGroup.DaysPreviousApplicationRuns.ToString();
            this.textBoxPreviousApplicationRunsSize.Text = this.userConfigurableFileGroup.SizePreviousApplicationRuns.ToString();
            this.textBoxDirectory.Text = this.userConfigurableFileGroup.Directory;
        }

        /// <summary>
        /// Copy settings from this UserControl to the FileGroup instance.
        /// </summary>
        public void CopySettingsToUserConfigurableFileGroupAndSave()
        {
            this.userConfigurableFileGroup.RemoveModeCurrentApplicationRun = RemoveModeCurrentApplicationRun;
            this.userConfigurableFileGroup.AskPermissionToRemoveCurrentApplicationRun = this.checkBoxCurrentApplicationRunAskPermissionToRemove.Checked;

            this.userConfigurableFileGroup.RemoveModePreviousApplicationRuns = RemoveModePreviousApplicationRuns;
            this.userConfigurableFileGroup.AskPermissionToRemovePreviousApplicationRuns = this.checkBoxPreviousApplicationRunsAskPermissionToRemove.Checked;
            this.userConfigurableFileGroup.DaysPreviousApplicationRuns = Convert.ToUInt16(this.textBoxPreviousApplicationRunsDays.Text);
            this.userConfigurableFileGroup.SizePreviousApplicationRuns = Convert.ToUInt16(this.textBoxPreviousApplicationRunsSize.Text);
            this.userConfigurableFileGroup.Directory = this.textBoxDirectory.Text;

            this.userConfigurableFileGroup.Save();
        }

        /// <summary>
        /// Called when the radioButton CurrentApplicationRunKeep checked state has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void radioButtonCurrentApplicationRunKeep_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCheckedChangedEventHandlersEnabled && this.radioButtonCurrentApplicationRunKeep.Checked)
            {
                RemoveModeCurrentApplicationRun = RemoveMode.Keep;
            }
        }

        /// <summary>
        /// Called when the radioButton CurrentApplicationRunRemove checked state has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void radioButtonCurrentApplicationRunRemove_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCheckedChangedEventHandlersEnabled && this.radioButtonCurrentApplicationRunRemove.Checked)
            {
                RemoveModeCurrentApplicationRun = RemoveMode.Remove;
            }
        }

        /// <summary>
        /// Called when the radioButton PreviousApplicationRunsKeep checked state has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void radioButtonPreviousApplicationRunsKeep_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCheckedChangedEventHandlersEnabled && this.radioButtonPreviousApplicationRunsKeep.Checked)
            {
                RemoveModePreviousApplicationRuns = RemoveMode.Keep;
            }
        }

        /// <summary>
        /// Called when the radioButton PreviousApplicationRunsRemove checked state has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void radioButtonPreviousApplicationRunsRemove_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCheckedChangedEventHandlersEnabled && this.radioButtonPreviousApplicationRunsRemove.Checked)
            {
                RemoveModePreviousApplicationRuns = RemoveMode.Remove;
            }
        }

        /// <summary>
        /// Called when the radioButton PreviousApplicationRunsRemoveIfOlderThan checked state has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void radioButtonPreviousApplicationRunsRemoveIfOlderThan_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCheckedChangedEventHandlersEnabled && this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.Checked)
            {
                RemoveModePreviousApplicationRuns = RemoveMode.RemoveIfOlderThan;
            }
        }

        /// <summary>
        /// Called when the radioButton PreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan checked state has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonCheckedChangedEventHandlersEnabled && this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.Checked)
            {
                RemoveModePreviousApplicationRuns = RemoveMode.RemoveOldestWhenTotalSizeLargerThan;
            }
        }

        /// <summary>
        /// Called when the input of this text box is validated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The cancel event arguments.</param>
        private void textBoxPreviousApplicationRunsDays_Validating(object sender, CancelEventArgs e)
        {
            ValidateTextBoxInput(textBoxPreviousApplicationRunsDays, e);
        }

        /// <summary>
        /// Called when the input of this text box is validated.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The cancel event arguments.</param>
        private void textBoxPreviousApplicationRunsSize_Validating(object sender, CancelEventArgs e)
        {
            ValidateTextBoxInput(textBoxPreviousApplicationRunsSize, e);
        }

        /// <summary>
        /// Validate if the text box contains un integer between 1 and 65535.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <param name="e">The cancel event arguments.</param>
        private void ValidateTextBoxInput(TextBox textBox, CancelEventArgs e)
        {
            bool correctValue = true;

            try
            {
                UInt16 size = Convert.ToUInt16(textBox.Text);

                if (size < 1)
                {
                    correctValue = false;
                }
            }
            catch
            {
                correctValue = false;
            }

            if (!correctValue)
            {
                MessageBox.Show("Size value must be a value between 1 and 65535");
                e.Cancel = true;
                textBox.Select(0, textBox.Text.Length);
            }
        }
    }
}
