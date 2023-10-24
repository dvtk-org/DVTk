namespace DvtkApplicationLayer.StoredFiles
{
    partial class FileGroupConfiguratorUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove = new System.Windows.Forms.CheckBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.textBoxPreviousApplicationRunsDays = new System.Windows.Forms.TextBox();
            this.groupBoxPreviousApplicationRuns = new System.Windows.Forms.GroupBox();
            this.labelDays = new System.Windows.Forms.Label();
            this.textBoxPreviousApplicationRunsSize = new System.Windows.Forms.TextBox();
            this.radioButtonPreviousApplicationRunsRemove = new System.Windows.Forms.RadioButton();
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan = new System.Windows.Forms.RadioButton();
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan = new System.Windows.Forms.RadioButton();
            this.radioButtonPreviousApplicationRunsKeep = new System.Windows.Forms.RadioButton();
            this.checkBoxCurrentApplicationRunAskPermissionToRemove = new System.Windows.Forms.CheckBox();
            this.radioButtonCurrentApplicationRunKeep = new System.Windows.Forms.RadioButton();
            this.groupBoxCurrentApplicationRun = new System.Windows.Forms.GroupBox();
            this.radioButtonCurrentApplicationRunRemove = new System.Windows.Forms.RadioButton();
            this.buttonDirectoryBrowse = new System.Windows.Forms.Button();
            this.groupBoxWhenClosingThisApplication = new System.Windows.Forms.GroupBox();
            this.buttonDirectoryExplore = new System.Windows.Forms.Button();
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.labelDirectory = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelWildCards = new System.Windows.Forms.Label();
            this.groupBoxPreviousApplicationRuns.SuspendLayout();
            this.groupBoxCurrentApplicationRun.SuspendLayout();
            this.groupBoxWhenClosingThisApplication.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxPreviousApplicationRunsAskPermissionToRemove
            // 
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.AutoSize = true;
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.Enabled = false;
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.Location = new System.Drawing.Point(7, 112);
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.Name = "checkBoxPreviousApplicationRunsAskPermissionToRemove";
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.Size = new System.Drawing.Size(154, 17);
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.TabIndex = 3;
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.Text = "Ask confirmation to remove";
            this.checkBoxPreviousApplicationRunsAskPermissionToRemove.UseVisualStyleBackColor = true;
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Enabled = false;
            this.labelSize.Location = new System.Drawing.Point(301, 68);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(23, 13);
            this.labelSize.TabIndex = 6;
            this.labelSize.Text = "MB";
            // 
            // textBoxPreviousApplicationRunsDays
            // 
            this.textBoxPreviousApplicationRunsDays.Enabled = false;
            this.textBoxPreviousApplicationRunsDays.Location = new System.Drawing.Point(131, 88);
            this.textBoxPreviousApplicationRunsDays.Name = "textBoxPreviousApplicationRunsDays";
            this.textBoxPreviousApplicationRunsDays.Size = new System.Drawing.Size(50, 20);
            this.textBoxPreviousApplicationRunsDays.TabIndex = 5;
            this.textBoxPreviousApplicationRunsDays.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxPreviousApplicationRunsDays_Validating);
            // 
            // groupBoxPreviousApplicationRuns
            // 
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.checkBoxPreviousApplicationRunsAskPermissionToRemove);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.labelDays);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.labelSize);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.textBoxPreviousApplicationRunsDays);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.textBoxPreviousApplicationRunsSize);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.radioButtonPreviousApplicationRunsRemove);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.radioButtonPreviousApplicationRunsRemoveIfOlderThan);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan);
            this.groupBoxPreviousApplicationRuns.Controls.Add(this.radioButtonPreviousApplicationRunsKeep);
            this.groupBoxPreviousApplicationRuns.Location = new System.Drawing.Point(6, 137);
            this.groupBoxPreviousApplicationRuns.Name = "groupBoxPreviousApplicationRuns";
            this.groupBoxPreviousApplicationRuns.Size = new System.Drawing.Size(592, 140);
            this.groupBoxPreviousApplicationRuns.TabIndex = 5;
            this.groupBoxPreviousApplicationRuns.TabStop = false;
            this.groupBoxPreviousApplicationRuns.Text = "Do the following with these files stored during the previous runs of the ...";
            // 
            // labelDays
            // 
            this.labelDays.AutoSize = true;
            this.labelDays.Enabled = false;
            this.labelDays.Location = new System.Drawing.Point(187, 91);
            this.labelDays.Name = "labelDays";
            this.labelDays.Size = new System.Drawing.Size(29, 13);
            this.labelDays.TabIndex = 7;
            this.labelDays.Text = "days";
            // 
            // textBoxPreviousApplicationRunsSize
            // 
            this.textBoxPreviousApplicationRunsSize.Enabled = false;
            this.textBoxPreviousApplicationRunsSize.Location = new System.Drawing.Point(237, 63);
            this.textBoxPreviousApplicationRunsSize.Name = "textBoxPreviousApplicationRunsSize";
            this.textBoxPreviousApplicationRunsSize.Size = new System.Drawing.Size(58, 20);
            this.textBoxPreviousApplicationRunsSize.TabIndex = 4;
            this.textBoxPreviousApplicationRunsSize.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxPreviousApplicationRunsSize_Validating);
            // 
            // radioButtonPreviousApplicationRunsRemove
            // 
            this.radioButtonPreviousApplicationRunsRemove.AutoSize = true;
            this.radioButtonPreviousApplicationRunsRemove.Location = new System.Drawing.Point(7, 43);
            this.radioButtonPreviousApplicationRunsRemove.Name = "radioButtonPreviousApplicationRunsRemove";
            this.radioButtonPreviousApplicationRunsRemove.Size = new System.Drawing.Size(65, 17);
            this.radioButtonPreviousApplicationRunsRemove.TabIndex = 3;
            this.radioButtonPreviousApplicationRunsRemove.Text = "Remove";
            this.radioButtonPreviousApplicationRunsRemove.UseVisualStyleBackColor = true;
            this.radioButtonPreviousApplicationRunsRemove.CheckedChanged += new System.EventHandler(this.radioButtonPreviousApplicationRunsRemove_CheckedChanged);
            // 
            // radioButtonPreviousApplicationRunsRemoveIfOlderThan
            // 
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.AutoSize = true;
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.Location = new System.Drawing.Point(7, 89);
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.Name = "radioButtonPreviousApplicationRunsRemoveIfOlderThan";
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.Size = new System.Drawing.Size(123, 17);
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.TabIndex = 2;
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.Text = "Remove if older than";
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.UseVisualStyleBackColor = true;
            this.radioButtonPreviousApplicationRunsRemoveIfOlderThan.CheckedChanged += new System.EventHandler(this.radioButtonPreviousApplicationRunsRemoveIfOlderThan_CheckedChanged);
            // 
            // radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan
            // 
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.AutoSize = true;
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.Location = new System.Drawing.Point(6, 66);
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.Name = "radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan";
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.Size = new System.Drawing.Size(225, 17);
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.TabIndex = 1;
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.Text = "Remove oldest when total size larger than ";
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.UseVisualStyleBackColor = true;
            this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan.CheckedChanged += new System.EventHandler(this.radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan_CheckedChanged);
            // 
            // radioButtonPreviousApplicationRunsKeep
            // 
            this.radioButtonPreviousApplicationRunsKeep.AutoSize = true;
            this.radioButtonPreviousApplicationRunsKeep.Checked = true;
            this.radioButtonPreviousApplicationRunsKeep.Location = new System.Drawing.Point(7, 20);
            this.radioButtonPreviousApplicationRunsKeep.Name = "radioButtonPreviousApplicationRunsKeep";
            this.radioButtonPreviousApplicationRunsKeep.Size = new System.Drawing.Size(50, 17);
            this.radioButtonPreviousApplicationRunsKeep.TabIndex = 0;
            this.radioButtonPreviousApplicationRunsKeep.TabStop = true;
            this.radioButtonPreviousApplicationRunsKeep.Text = "Keep";
            this.radioButtonPreviousApplicationRunsKeep.UseVisualStyleBackColor = true;
            this.radioButtonPreviousApplicationRunsKeep.CheckedChanged += new System.EventHandler(this.radioButtonPreviousApplicationRunsKeep_CheckedChanged);
            // 
            // checkBoxCurrentApplicationRunAskPermissionToRemove
            // 
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.AutoSize = true;
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.Enabled = false;
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.Location = new System.Drawing.Point(6, 65);
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.Name = "checkBoxCurrentApplicationRunAskPermissionToRemove";
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.Size = new System.Drawing.Size(154, 17);
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.TabIndex = 2;
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.TabStop = false;
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.Text = "Ask confirmation to remove";
            this.checkBoxCurrentApplicationRunAskPermissionToRemove.UseVisualStyleBackColor = true;
            // 
            // radioButtonCurrentApplicationRunKeep
            // 
            this.radioButtonCurrentApplicationRunKeep.AutoSize = true;
            this.radioButtonCurrentApplicationRunKeep.Checked = true;
            this.radioButtonCurrentApplicationRunKeep.Location = new System.Drawing.Point(6, 19);
            this.radioButtonCurrentApplicationRunKeep.Name = "radioButtonCurrentApplicationRunKeep";
            this.radioButtonCurrentApplicationRunKeep.Size = new System.Drawing.Size(50, 17);
            this.radioButtonCurrentApplicationRunKeep.TabIndex = 0;
            this.radioButtonCurrentApplicationRunKeep.TabStop = true;
            this.radioButtonCurrentApplicationRunKeep.Text = "Keep";
            this.radioButtonCurrentApplicationRunKeep.UseVisualStyleBackColor = true;
            this.radioButtonCurrentApplicationRunKeep.CheckedChanged += new System.EventHandler(this.radioButtonCurrentApplicationRunKeep_CheckedChanged);
            // 
            // groupBoxCurrentApplicationRun
            // 
            this.groupBoxCurrentApplicationRun.Controls.Add(this.checkBoxCurrentApplicationRunAskPermissionToRemove);
            this.groupBoxCurrentApplicationRun.Controls.Add(this.radioButtonCurrentApplicationRunKeep);
            this.groupBoxCurrentApplicationRun.Controls.Add(this.radioButtonCurrentApplicationRunRemove);
            this.groupBoxCurrentApplicationRun.Location = new System.Drawing.Point(6, 29);
            this.groupBoxCurrentApplicationRun.Name = "groupBoxCurrentApplicationRun";
            this.groupBoxCurrentApplicationRun.Size = new System.Drawing.Size(592, 91);
            this.groupBoxCurrentApplicationRun.TabIndex = 4;
            this.groupBoxCurrentApplicationRun.TabStop = false;
            this.groupBoxCurrentApplicationRun.Text = "Do the following with these files stored during the last run of the ...";
            // 
            // radioButtonCurrentApplicationRunRemove
            // 
            this.radioButtonCurrentApplicationRunRemove.AutoSize = true;
            this.radioButtonCurrentApplicationRunRemove.Location = new System.Drawing.Point(6, 42);
            this.radioButtonCurrentApplicationRunRemove.Name = "radioButtonCurrentApplicationRunRemove";
            this.radioButtonCurrentApplicationRunRemove.Size = new System.Drawing.Size(65, 17);
            this.radioButtonCurrentApplicationRunRemove.TabIndex = 1;
            this.radioButtonCurrentApplicationRunRemove.Text = "Remove";
            this.radioButtonCurrentApplicationRunRemove.UseVisualStyleBackColor = true;
            this.radioButtonCurrentApplicationRunRemove.CheckedChanged += new System.EventHandler(this.radioButtonCurrentApplicationRunRemove_CheckedChanged);
            // 
            // buttonDirectoryBrowse
            // 
            this.buttonDirectoryBrowse.Location = new System.Drawing.Point(504, 379);
            this.buttonDirectoryBrowse.Name = "buttonDirectoryBrowse";
            this.buttonDirectoryBrowse.Size = new System.Drawing.Size(63, 30);
            this.buttonDirectoryBrowse.TabIndex = 18;
            this.buttonDirectoryBrowse.Text = "Browse...";
            this.buttonDirectoryBrowse.UseVisualStyleBackColor = true;
            this.buttonDirectoryBrowse.Click += new System.EventHandler(this.buttonDirectoryBrowse_Click);
            // 
            // groupBoxWhenClosingThisApplication
            // 
            this.groupBoxWhenClosingThisApplication.Controls.Add(this.groupBoxPreviousApplicationRuns);
            this.groupBoxWhenClosingThisApplication.Controls.Add(this.groupBoxCurrentApplicationRun);
            this.groupBoxWhenClosingThisApplication.Location = new System.Drawing.Point(29, 74);
            this.groupBoxWhenClosingThisApplication.Name = "groupBoxWhenClosingThisApplication";
            this.groupBoxWhenClosingThisApplication.Size = new System.Drawing.Size(604, 295);
            this.groupBoxWhenClosingThisApplication.TabIndex = 15;
            this.groupBoxWhenClosingThisApplication.TabStop = false;
            this.groupBoxWhenClosingThisApplication.Text = "When exiting";
            // 
            // buttonDirectoryExplore
            // 
            this.buttonDirectoryExplore.Location = new System.Drawing.Point(573, 379);
            this.buttonDirectoryExplore.Name = "buttonDirectoryExplore";
            this.buttonDirectoryExplore.Size = new System.Drawing.Size(60, 30);
            this.buttonDirectoryExplore.TabIndex = 19;
            this.buttonDirectoryExplore.Text = "Explore...";
            this.buttonDirectoryExplore.UseVisualStyleBackColor = true;
            this.buttonDirectoryExplore.Click += new System.EventHandler(this.buttonDirectoryExplore_Click);
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Location = new System.Drawing.Point(87, 385);
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.ReadOnly = true;
            this.textBoxDirectory.Size = new System.Drawing.Size(411, 20);
            this.textBoxDirectory.TabIndex = 17;
            // 
            // labelDirectory
            // 
            this.labelDirectory.AutoSize = true;
            this.labelDirectory.Location = new System.Drawing.Point(29, 385);
            this.labelDirectory.Name = "labelDirectory";
            this.labelDirectory.Size = new System.Drawing.Size(52, 13);
            this.labelDirectory.TabIndex = 16;
            this.labelDirectory.Text = "Directory:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(26, 23);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(60, 13);
            this.labelDescription.TabIndex = 20;
            this.labelDescription.Text = "Description";
            // 
            // labelWildCards
            // 
            this.labelWildCards.AutoSize = true;
            this.labelWildCards.Location = new System.Drawing.Point(26, 47);
            this.labelWildCards.Name = "labelWildCards";
            this.labelWildCards.Size = new System.Drawing.Size(28, 13);
            this.labelWildCards.TabIndex = 21;
            this.labelWildCards.Text = "Files";
            // 
            // FileGroupConfiguratorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelWildCards);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.buttonDirectoryBrowse);
            this.Controls.Add(this.groupBoxWhenClosingThisApplication);
            this.Controls.Add(this.buttonDirectoryExplore);
            this.Controls.Add(this.textBoxDirectory);
            this.Controls.Add(this.labelDirectory);
            this.Name = "FileGroupConfiguratorUserControl";
            this.Size = new System.Drawing.Size(657, 445);
            this.groupBoxPreviousApplicationRuns.ResumeLayout(false);
            this.groupBoxPreviousApplicationRuns.PerformLayout();
            this.groupBoxCurrentApplicationRun.ResumeLayout(false);
            this.groupBoxCurrentApplicationRun.PerformLayout();
            this.groupBoxWhenClosingThisApplication.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxPreviousApplicationRunsAskPermissionToRemove;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.TextBox textBoxPreviousApplicationRunsDays;
        private System.Windows.Forms.GroupBox groupBoxPreviousApplicationRuns;
        private System.Windows.Forms.Label labelDays;
        private System.Windows.Forms.TextBox textBoxPreviousApplicationRunsSize;
        private System.Windows.Forms.RadioButton radioButtonPreviousApplicationRunsRemove;
        private System.Windows.Forms.RadioButton radioButtonPreviousApplicationRunsRemoveIfOlderThan;
        private System.Windows.Forms.RadioButton radioButtonPreviousApplicationRunsRemoveOldestWhenTotalSizeLargerThan;
        private System.Windows.Forms.RadioButton radioButtonPreviousApplicationRunsKeep;
        private System.Windows.Forms.CheckBox checkBoxCurrentApplicationRunAskPermissionToRemove;
        private System.Windows.Forms.RadioButton radioButtonCurrentApplicationRunKeep;
        private System.Windows.Forms.GroupBox groupBoxCurrentApplicationRun;
        private System.Windows.Forms.RadioButton radioButtonCurrentApplicationRunRemove;
        private System.Windows.Forms.Button buttonDirectoryBrowse;
        private System.Windows.Forms.GroupBox groupBoxWhenClosingThisApplication;
        private System.Windows.Forms.Button buttonDirectoryExplore;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Label labelDirectory;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelWildCards;
    }
}
