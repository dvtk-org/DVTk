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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DvtkApplicationLayer.UserInterfaces
{
    /// <summary>
    /// 
    /// </summary>
    public class SopClassesUserControl : System.Windows.Forms.UserControl
    {
        private Panel panel5;
        private ListBox ListBoxSpecifySopClassesDefinitionFileDirectories;
        private Label LabelSpecifySopClassesDefinitionFileDirectories;
        private Panel panel3;
        private Button ButtonUnselectAll;
        private Button ButtonSelectAllDefinitionFiles;
        private Button ButtonSpecifySopClassesAddDirectory;
        private Button ButtonSpecifySopClassesRemoveDirectory;
        private DataGrid DataGridSpecifySopClasses;
        private RichTextBox RichTextBoxSpecifySopClassesInfo;

        private string DefinitionName = null;
        Dvtk.Sessions.Session theSession = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public SopClassesUserControl()
        {
            InitializeComponent();

            _theDefinitionFileTextColumn = new DataGridTextBoxColumn();
            
            Initialize();
            //UpdateDataGrid();            
        }

        private DataGridTextBoxColumn _theDefinitionFileTextColumn;
        private ArrayList _DefinitionFilesInfoForDataGrid = new ArrayList();
        private int _MouseEventInfoForDataGrid_LoadedStateChangedForRow = -1;
        private bool _MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown = true;

        void Initialize()
        {
            DataGridBoolColumn theBoolColumn = null;
            DataGridTextBoxColumn theTextColumn = null;
            DataGridTableStyle theStyle = new DataGridTableStyle();
            theStyle.AllowSorting = true;
            theStyle.RowHeadersVisible = false;
            theStyle.MappingName = "ArrayList";

            // We set the column to readonly and handle the mouse events ourselves
            // in the MouseUp event handler. We want to circumvent the select cell
            // first before you can enable/disable a checkbox.
            theBoolColumn = new DataGridBoolColumn();
            theBoolColumn.MappingName = "Loaded";
            theBoolColumn.HeaderText = "Loaded";
            theBoolColumn.Width = 50;
            theBoolColumn.AllowNull = false;
            theBoolColumn.ReadOnly = false;
            theStyle.GridColumnStyles.Add(theBoolColumn);

            _theDefinitionFileTextColumn.MappingName = "Filename";
            _theDefinitionFileTextColumn.HeaderText = "Definition filename";
            _theDefinitionFileTextColumn.Width = 250;
            _theDefinitionFileTextColumn.ReadOnly = true;
            _theDefinitionFileTextColumn.TextBox.DoubleClick += new System.EventHandler(this.DefinitionFile_DoubleClick);
            theStyle.GridColumnStyles.Add(_theDefinitionFileTextColumn);

            theTextColumn = new DataGridTextBoxColumn();
            theTextColumn.MappingName = "SOPClassName";
            theTextColumn.HeaderText = "SOP class name";
            theTextColumn.Width = 125;
            theTextColumn.ReadOnly = true;
            theStyle.GridColumnStyles.Add(theTextColumn);

            theTextColumn = new DataGridTextBoxColumn();
            theTextColumn.MappingName = "SOPClassUID";
            theTextColumn.HeaderText = "SOP class UID";
            theTextColumn.Width = 125;
            theTextColumn.ReadOnly = true;
            theStyle.GridColumnStyles.Add(theTextColumn);

            theTextColumn = new DataGridTextBoxColumn();
            theTextColumn.MappingName = "AETitle";
            theTextColumn.HeaderText = "AE title";
            theTextColumn.Width = 100;
            theTextColumn.ReadOnly = true;
            theStyle.GridColumnStyles.Add(theTextColumn);

            theTextColumn = new DataGridTextBoxColumn();
            theTextColumn.MappingName = "AEVersion";
            theTextColumn.HeaderText = "AE version";
            theTextColumn.Width = 50;
            theTextColumn.ReadOnly = true;
            theStyle.GridColumnStyles.Add(theTextColumn);

            theTextColumn = new DataGridTextBoxColumn();
            theTextColumn.MappingName = "DefinitionRoot";
            theTextColumn.HeaderText = "Definition root";
            theTextColumn.Width = 200;
            theTextColumn.ReadOnly = true;
            theStyle.GridColumnStyles.Add(theTextColumn);

            DataGridSpecifySopClasses.TableStyles.Add(theStyle);
        }

        private void InitializeComponent()
        {
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ButtonUnselectAll = new System.Windows.Forms.Button();
            this.ButtonSelectAllDefinitionFiles = new System.Windows.Forms.Button();
            this.ButtonSpecifySopClassesAddDirectory = new System.Windows.Forms.Button();
            this.ButtonSpecifySopClassesRemoveDirectory = new System.Windows.Forms.Button();
            this.ListBoxSpecifySopClassesDefinitionFileDirectories = new System.Windows.Forms.ListBox();
            this.LabelSpecifySopClassesDefinitionFileDirectories = new System.Windows.Forms.Label();
            this.DataGridSpecifySopClasses = new System.Windows.Forms.DataGrid();
            this.RichTextBoxSpecifySopClassesInfo = new System.Windows.Forms.RichTextBox();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSpecifySopClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel3);
            this.panel5.Controls.Add(this.ListBoxSpecifySopClassesDefinitionFileDirectories);
            this.panel5.Controls.Add(this.LabelSpecifySopClassesDefinitionFileDirectories);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(825, 140);
            this.panel5.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.ButtonUnselectAll);
            this.panel3.Controls.Add(this.ButtonSelectAllDefinitionFiles);
            this.panel3.Controls.Add(this.ButtonSpecifySopClassesAddDirectory);
            this.panel3.Controls.Add(this.ButtonSpecifySopClassesRemoveDirectory);
            this.panel3.Location = new System.Drawing.Point(719, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(104, 152);
            this.panel3.TabIndex = 9;
            // 
            // ButtonUnselectAll
            // 
            this.ButtonUnselectAll.Location = new System.Drawing.Point(16, 107);
            this.ButtonUnselectAll.Name = "ButtonUnselectAll";
            this.ButtonUnselectAll.Size = new System.Drawing.Size(75, 23);
            this.ButtonUnselectAll.TabIndex = 4;
            this.ButtonUnselectAll.Text = "UnselectAll";
            this.ButtonUnselectAll.Click += new System.EventHandler(this.ButtonUnselectAll_Click);
            // 
            // ButtonSelectAllDefinitionFiles
            // 
            this.ButtonSelectAllDefinitionFiles.Location = new System.Drawing.Point(16, 74);
            this.ButtonSelectAllDefinitionFiles.Name = "ButtonSelectAllDefinitionFiles";
            this.ButtonSelectAllDefinitionFiles.Size = new System.Drawing.Size(75, 23);
            this.ButtonSelectAllDefinitionFiles.TabIndex = 3;
            this.ButtonSelectAllDefinitionFiles.Text = "SelectAll";
            this.ButtonSelectAllDefinitionFiles.Click += new System.EventHandler(this.ButtonSelectAllDefinitionFiles_Click);
            // 
            // ButtonSpecifySopClassesAddDirectory
            // 
            this.ButtonSpecifySopClassesAddDirectory.Location = new System.Drawing.Point(16, 12);
            this.ButtonSpecifySopClassesAddDirectory.Name = "ButtonSpecifySopClassesAddDirectory";
            this.ButtonSpecifySopClassesAddDirectory.Size = new System.Drawing.Size(75, 23);
            this.ButtonSpecifySopClassesAddDirectory.TabIndex = 1;
            this.ButtonSpecifySopClassesAddDirectory.Text = "Add";
            this.ButtonSpecifySopClassesAddDirectory.Click += new System.EventHandler(this.ButtonSpecifySopClassesAddDirectory_Click);
            // 
            // ButtonSpecifySopClassesRemoveDirectory
            // 
            this.ButtonSpecifySopClassesRemoveDirectory.Location = new System.Drawing.Point(16, 43);
            this.ButtonSpecifySopClassesRemoveDirectory.Name = "ButtonSpecifySopClassesRemoveDirectory";
            this.ButtonSpecifySopClassesRemoveDirectory.Size = new System.Drawing.Size(75, 23);
            this.ButtonSpecifySopClassesRemoveDirectory.TabIndex = 2;
            this.ButtonSpecifySopClassesRemoveDirectory.Text = "Remove";
            this.ButtonSpecifySopClassesRemoveDirectory.Click += new System.EventHandler(this.ButtonSpecifySopClassesRemoveDirectory_Click);
            // 
            // ListBoxSpecifySopClassesDefinitionFileDirectories
            // 
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Location = new System.Drawing.Point(138, 20);
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Name = "ListBoxSpecifySopClassesDefinitionFileDirectories";
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Size = new System.Drawing.Size(571, 95);
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.TabIndex = 0;
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.TabStop = false;
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.SelectedIndexChanged += new System.EventHandler(this.ListBoxSpecifySopClassesDefinitionFileDirectories_SelectedIndexChanged);
            // 
            // LabelSpecifySopClassesDefinitionFileDirectories
            // 
            this.LabelSpecifySopClassesDefinitionFileDirectories.Location = new System.Drawing.Point(4, 24);
            this.LabelSpecifySopClassesDefinitionFileDirectories.Name = "LabelSpecifySopClassesDefinitionFileDirectories";
            this.LabelSpecifySopClassesDefinitionFileDirectories.Size = new System.Drawing.Size(131, 23);
            this.LabelSpecifySopClassesDefinitionFileDirectories.TabIndex = 0;
            this.LabelSpecifySopClassesDefinitionFileDirectories.Text = "Definition file directories:";
            // 
            // DataGridSpecifySopClasses
            // 
            this.DataGridSpecifySopClasses.DataMember = "";
            this.DataGridSpecifySopClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridSpecifySopClasses.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.DataGridSpecifySopClasses.Location = new System.Drawing.Point(0, 140);
            this.DataGridSpecifySopClasses.Name = "DataGridSpecifySopClasses";
            this.DataGridSpecifySopClasses.Size = new System.Drawing.Size(825, 539);
            this.DataGridSpecifySopClasses.TabIndex = 10;
            this.DataGridSpecifySopClasses.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGridSpecifySopClasses_MouseUp);
            this.DataGridSpecifySopClasses.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridSpecifySopClasses_MouseMove);
            this.DataGridSpecifySopClasses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridSpecifySopClasses_MouseDown);
            // 
            // RichTextBoxSpecifySopClassesInfo
            // 
            this.RichTextBoxSpecifySopClassesInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RichTextBoxSpecifySopClassesInfo.Location = new System.Drawing.Point(0, 679);
            this.RichTextBoxSpecifySopClassesInfo.Name = "RichTextBoxSpecifySopClassesInfo";
            this.RichTextBoxSpecifySopClassesInfo.Size = new System.Drawing.Size(825, 54);
            this.RichTextBoxSpecifySopClassesInfo.TabIndex = 11;
            this.RichTextBoxSpecifySopClassesInfo.Text = "";
            // 
            // SopClassesUserControl
            // 
            this.Controls.Add(this.DataGridSpecifySopClasses);
            this.Controls.Add(this.RichTextBoxSpecifySopClassesInfo);
            this.Controls.Add(this.panel5);
            this.Name = "SopClassesUserControl";
            this.Size = new System.Drawing.Size(825, 733);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSpecifySopClasses)).EndInit();
            this.ResumeLayout(false);

        }

        private void ButtonSpecifySopClassesAddDirectory_Click(object sender, EventArgs e)
        {
            AddDefinitionFileDirectory();
        }

        private void ButtonSpecifySopClassesRemoveDirectory_Click(object sender, EventArgs e)
        {
            RemoveDefinitionFileDirectory();
        }

        private void ButtonSelectAllDefinitionFiles_Click(object sender, EventArgs e)
        {
            SelectAllDefinitionFiles();
        }

        private void ButtonUnselectAll_Click(object sender, EventArgs e)
        {
            UnSelectAllDefinitionFiles();
        }

        private void DataGridSpecifySopClasses_MouseDown(object sender, MouseEventArgs e)
        {
            DataGrid_MouseDown(e);
        }

        private void DataGridSpecifySopClasses_MouseMove(object sender, MouseEventArgs e)
        {
            DataGrid_MouseMove(e);
        }

        private void DataGridSpecifySopClasses_MouseUp(object sender, MouseEventArgs e)
        {
            DataGrid_MouseUp(e);
        }

        private void ListBoxSpecifySopClassesDefinitionFileDirectories_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRemoveButton();
        }

        private void DefinitionFile_DoubleClick(object sender, System.EventArgs e)
        {
            OpenDefinitionFile();
        }

        /// <summary>
        /// Get the user selected definitions file list from Datagrid
        /// </summary>
        public ArrayList SelectedDefinitionFilesList
        {
            get
            {
                return _DefinitionFilesInfoForDataGrid;
            }
        }

        void OpenDefinitionFile()
        {
            // Open the definition file in notepad.
            System.Diagnostics.Process theProcess = new System.Diagnostics.Process();

            // Get full definition file name.
            string theFileName = DefinitionName;
            string theFullFileName = "";
            foreach (DefinitionFile theDefinitionFile in _DefinitionFilesInfoForDataGrid)
            {
                if (theDefinitionFile.Filename == theFileName)
                {
                    theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
                    break;
                }
            }

            theProcess.StartInfo.FileName = "Notepad.exe";
            theProcess.StartInfo.Arguments = theFullFileName;
            theProcess.Start();
        }

        /// <summary>
        /// Update the Specify SOP classes tab.
        /// </summary>
        public void UpdateDataGrid(Dvtk.Sessions.ScriptSession session)
        {
            theSession = session;

            Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.DefinitionManagement.DefinitionFileDirectoryList;

            // Update the definition file directories list box.
            ListBoxSpecifySopClassesDefinitionFileDirectories.Items.Clear();

            UpdateRemoveButton();

            foreach (string theDefinitionFileDirectory in theDefinitionFileDirectoryList)
            {
                ListBoxSpecifySopClassesDefinitionFileDirectories.Items.Add(theDefinitionFileDirectory);
            }

            // Update the SOP classes data grid.
            // For every definition file directory, use the .def files present in the directory.
            RichTextBoxSpecifySopClassesInfo.Clear();
            _DefinitionFilesInfoForDataGrid.Clear();

            // Add the correct information to the datagrid by inspecting the definition directory.
            // When doing this, also fill the combobox with available ae title - version combinations.
            AddSopClassesToDataGridFromDirectories();

            // Add the correct information to the datagrid by inspecting the loaded definitions.
            // When doing this, also fill the combobox with available ae title - version combinations.
            // Only add an entry if it does not already exist.
            AddSopClassesToDataGridFromLoadedDefinitions();

            // Workaround for following problem:
            // If the SOP classes tab has already been filled, and another session contains less
            // records for the datagrid, windows gets confused and thinks there are more records
            // then actually present in _DefinitionFilesInfoForDataGrid resulting in an assertion.
            DataGridSpecifySopClasses.SetDataBinding(null, "");

            // Actually refresh the data grid.
            DataGridSpecifySopClasses.SetDataBinding(_DefinitionFilesInfoForDataGrid, "");
            //DataGridSpecifySopClasses.DataSource = _DefinitionFilesInfoForDataGrid;

            DataGridSpecifySopClasses.Refresh();
        }

        private void AddSopClassesToDataGridFromDirectories()
        {
            Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.DefinitionManagement.DefinitionFileDirectoryList;

            foreach (string theDefinitionFileDirectory in theDefinitionFileDirectoryList)
            {
                DirectoryInfo theDirectoryInfo = new DirectoryInfo(theDefinitionFileDirectory);

                if (theDirectoryInfo.Exists)
                {
                    FileInfo[] theFilesInfo;

                    theFilesInfo = theDirectoryInfo.GetFiles("*.def");

                    foreach (FileInfo theDefinitionFileInfo in theFilesInfo)
                    {
                        try
                        {
                            AddSopClassToDataGridFromDefinitionFile(theDefinitionFileInfo.FullName);
                        }
                        catch (Exception exception)
                        {
                            string theErrorText;

                            theErrorText = string.Format("Definition file {0} could not be interpreted while reading directory:\n{1}\n\n", theDefinitionFileInfo.FullName, exception.Message);

                            RichTextBoxSpecifySopClassesInfo.AppendText(theErrorText);
                        }
                    }
                }
            }
        }

        private void AddSopClassesToDataGridFromLoadedDefinitions()
        {
            foreach (string theLoadedDefinitionFileName in theSession.DefinitionManagement.LoadedDefinitionFileNames)
            {
                string theLoadedDefinitionFullFileName = theLoadedDefinitionFileName;

                // If theLoadedDefinitionFileName does not contain a '\', this is a file name only.
                // Append the Root directory to get the full path name.
                if (theLoadedDefinitionFileName.LastIndexOf("\\") == -1)
                {
                    theLoadedDefinitionFullFileName = System.IO.Path.Combine(theSession.DefinitionManagement.DefinitionFileRootDirectory, theLoadedDefinitionFileName);
                }

                // Check if this definition file is already present in the data grid.
                // Do this by comparing the full file name of the definition file with the full file names
                // present in the data grid.
                bool IsDefinitionAlreadyPresent = false;

                foreach (DefinitionFile theDefinitionFile in _DefinitionFilesInfoForDataGrid)
                {
                    string defFileForDataGrid = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
                    if (defFileForDataGrid.ToLower() == theLoadedDefinitionFullFileName.ToLower())
                    {
                        IsDefinitionAlreadyPresent = true;
                        break;
                    }
                }

                if (!IsDefinitionAlreadyPresent)
                {
                    try
                    {
                        AddSopClassToDataGridFromDefinitionFile(theLoadedDefinitionFullFileName);
                    }
                    catch (Exception exception)
                    {
                        string theErrorText;

                        theErrorText = string.Format("Definition file {0} could not be interpreted while reading definition present in session:\n{1}\n\n", theLoadedDefinitionFullFileName, exception.Message);

                        RichTextBoxSpecifySopClassesInfo.AppendText(theErrorText);
                    }
                }
            }
        }

        /// <summary>
        /// The information from a definition file is added to the datagrid and possibly to the combo box.
        /// 
        /// An excpetion is thrown when retrieving details for the definition file fails.
        /// </summary>
        /// <param name="theDefinitionFullFileName">the full file name of the definition file.</param>
        private void AddSopClassToDataGridFromDefinitionFile(string theDefinitionFullFileName)
        {
            Dvtk.Sessions.DefinitionFileDetails theDefinitionFileDetails;

            // Try to get detailed information about this definition file.
            theDefinitionFileDetails = theSession.DefinitionManagement.GetDefinitionFileDetails(theDefinitionFullFileName);

            // No excpetion thrown when calling GetDefinitionFileDetails (otherwise this statement would not have been reached)
            // so this is a valid definition file. Add it to the data frid.
            DefinitionFile theDataGridDefinitionFileInfo =
                new DefinitionFile(IsDefinitionFileLoaded(theDefinitionFullFileName),
                System.IO.Path.GetFileName(theDefinitionFullFileName),
                theDefinitionFileDetails.SOPClassName,
                theDefinitionFileDetails.SOPClassUID,
                theDefinitionFileDetails.ApplicationEntityName,
                theDefinitionFileDetails.ApplicationEntityVersion,
                System.IO.Path.GetDirectoryName(theDefinitionFullFileName));

            if (!theDefinitionFullFileName.Contains("AllDimseCommands.def"))
                _DefinitionFilesInfoForDataGrid.Add(theDataGridDefinitionFileInfo);            
        }

        private bool IsDefinitionFileLoaded(string theFullDefinitionFileName)
        {
            bool theReturnValue = false;

            foreach (string theLoadedDefinitionFileName in theSession.DefinitionManagement.LoadedDefinitionFileNames)
            {
                string thetheLoadedDefinitionFullFileName = theLoadedDefinitionFileName;

                // If theLoadedDefinitionFileName does not contain a '\', this is a file name only.
                // Append the Root directory to get the full path name.
                if (theLoadedDefinitionFileName.LastIndexOf("\\") == -1)
                {
                    thetheLoadedDefinitionFullFileName = System.IO.Path.Combine(theSession.DefinitionManagement.DefinitionFileRootDirectory, theLoadedDefinitionFileName);
                }

                if (thetheLoadedDefinitionFullFileName.ToLower() == theFullDefinitionFileName.ToLower())
                {
                    theReturnValue = true;
                    break;
                }
            }

            return (theReturnValue);
        }

        /// <summary>
        /// Add a definition file direcotory.
        /// </summary>
        private void AddDefinitionFileDirectory()
        {
            Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.DefinitionManagement.DefinitionFileDirectoryList;

            FolderBrowserDialog theFolderBrowserDialog = new FolderBrowserDialog();

            theFolderBrowserDialog.Description = "Select the directory where definition files are located.";

            if (theFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                bool isExistingDirectory = false;
                string theNewDirectory = GetDirectoryWithTrailingSlashesRemoved(theFolderBrowserDialog.SelectedPath);

                // Find out if this new directory already exists.
                foreach (string theExistingDirectory in theDefinitionFileDirectoryList)
                {
                    if (theNewDirectory == GetDirectoryWithTrailingSlashesRemoved(theExistingDirectory))
                    {
                        isExistingDirectory = true;
                        break;
                    }
                }

                theNewDirectory = theNewDirectory + "\\";

                // Only add this new directory if it does not already exist.
                if (!isExistingDirectory)
                {
                    DirectoryInfo theDirectoryInfo = new DirectoryInfo(theNewDirectory);

                    // If the new directory is not valid, show an error message.
                    if (!theDirectoryInfo.Exists)
                    {
                        MessageBox.Show("The directory \"" + theNewDirectory + "\" is not a valid directory.",
                            "Directory not added",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        theDefinitionFileDirectoryList.Add(theNewDirectory);
                        UpdateDataGrid((Dvtk.Sessions.ScriptSession)theSession);
                    }
                }
                else
                {
                    MessageBox.Show("The directory \"" + theNewDirectory + "\" is already present in\nthe list of definition file directories.",
                        "Directory not added",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void RemoveDefinitionFileDirectory()
        {
            string theSelectedDirectory;
            Dvtk.Sessions.DefinitionFileDirectoryList theDefinitionFileDirectoryList = theSession.DefinitionManagement.DefinitionFileDirectoryList;
            theSelectedDirectory = (string)ListBoxSpecifySopClassesDefinitionFileDirectories.SelectedItem;
            theDefinitionFileDirectoryList.Remove(theSelectedDirectory);
            UpdateDataGrid((Dvtk.Sessions.ScriptSession)theSession);
        }

        private string GetDirectoryWithTrailingSlashesRemoved(string theDirectory)
        {
            string theReturnValue = theDirectory;
            while (theReturnValue.EndsWith("/") || theReturnValue.EndsWith("\\"))
            {
                theReturnValue = theReturnValue.Substring(0, theReturnValue.Length - 1);
            }
            return (theReturnValue);
        }

        /// <summary>
        /// The reason we handle the mouseup event ourselves, is because with the normal
        /// grid, you first have to select a cell, and only then you can enable/disable a
        /// checkbox. We want to enable/disable the checkbox with 1 click.
        /// </summary>
        /// <param name="e"></param>
        private void DataGrid_MouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo theHitTestInfo;

            // Find out what part of the data grid is below the mouse pointer.
            theHitTestInfo = DataGridSpecifySopClasses.HitTest(e.X, e.Y);

            switch (theHitTestInfo.Type)
            {
                case System.Windows.Forms.DataGrid.HitTestType.Cell:
                    // If this is the "loaded" column...
                    if (theHitTestInfo.Column == 0)
                    {
                        // Remember the cell we've changed. We don't want to change this cell when we move the mouse.
                        // (see DataGrid_MouseMove).
                        _MouseEventInfoForDataGrid_LoadedStateChangedForRow = theHitTestInfo.Row;

                        // Get the column style for the "loaded" column.
                        DataGridColumnStyle theDataGridColumnStyle;
                        theDataGridColumnStyle = DataGridSpecifySopClasses.TableStyles[0].GridColumnStyles[0];

                        // Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
                        DataGridSpecifySopClasses.BeginEdit(theDataGridColumnStyle, theHitTestInfo.Row);
                        DefinitionFile theDefinitionFile = (DefinitionFile)_DefinitionFilesInfoForDataGrid[theHitTestInfo.Row];
                        theDefinitionFile.Loaded = !theDefinitionFile.Loaded;
                        DataGridSpecifySopClasses.EndEdit(theDataGridColumnStyle, theHitTestInfo.Row, false);


                        // Change the "loaded" state in the session object.
                        string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);

                        if (theDefinitionFile.Loaded)
                        {
                            // The definition file was not loaded yet. Load it now.
                            theSession.DefinitionManagement.LoadDefinitionFile(theFullFileName);
                        }
                        else
                        {
                            // The definition file was loaded. Unload it now.
                            theSession.DefinitionManagement.UnLoadDefinitionFile(theFullFileName);
                        }

                        // Remember the new "loaded" state for the case where the mouse is moved over other
                        // "loaded" checkboxes while keeping the left mouse button pressed.
                        _MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown = theDefinitionFile.Loaded;
                    }

                    if (theHitTestInfo.Column == 1)
                    {
                        DefinitionFile theDefinitionFile = (DefinitionFile)_DefinitionFilesInfoForDataGrid[theHitTestInfo.Row];
                        DefinitionName = theDefinitionFile.Filename;
                    }
                    break;
            }
        }

        /// <summary>
        /// When moving the mouse over other "loaded" check box while keeping the left mouse button pressed,
        /// change the "loaded" state to the new state when the left mouse button was pressed.
        /// </summary>
        /// <param name="e"></param>
        private void DataGrid_MouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                System.Windows.Forms.DataGrid.HitTestInfo theHitTestInfo;

                theHitTestInfo = DataGridSpecifySopClasses.HitTest(e.X, e.Y);

                switch (theHitTestInfo.Type)
                {
                    case System.Windows.Forms.DataGrid.HitTestType.Cell:
                        // If this is the "loaded" column...
                        if (theHitTestInfo.Column == 0)
                        {
                            // If another "loaded" check box is pointed to, not the one where the state has
                            // last been changed...
                            if (theHitTestInfo.Row != _MouseEventInfoForDataGrid_LoadedStateChangedForRow)
                            {
                                DefinitionFile theDefinitionFile = (DefinitionFile)_DefinitionFilesInfoForDataGrid[theHitTestInfo.Row];
                                bool theCurrentLoadedState = theDefinitionFile.Loaded;

                                // Only if the state will change...
                                if (theCurrentLoadedState != _MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown)
                                {
                                    // Get the column style for the "loaded" column.
                                    DataGridColumnStyle theDataGridColumnStyle;
                                    theDataGridColumnStyle = DataGridSpecifySopClasses.TableStyles[0].GridColumnStyles[0];

                                    // Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
                                    DataGridSpecifySopClasses.BeginEdit(theDataGridColumnStyle, theHitTestInfo.Row);
                                    theDefinitionFile.Loaded = _MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown;
                                    DataGridSpecifySopClasses.EndEdit(theDataGridColumnStyle, theHitTestInfo.Row, false);

                                    // Change the "loaded" state in the session object.
                                    string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);

                                    if (_MouseEventInfoForDataGrid_LoadedStatedAfterMouseDown)
                                    {
                                        // The definition file was not loaded yet. Load it now.
                                        theSession.DefinitionManagement.LoadDefinitionFile(theFullFileName);
                                    }
                                    else
                                    {
                                        // The definition file was loaded. Unload it now.
                                        theSession.DefinitionManagement.UnLoadDefinitionFile(theFullFileName);
                                    }

                                    // Remember the cell we've changed. We don't want to change the loaded
                                    // state with each minor mouse move.
                                    _MouseEventInfoForDataGrid_LoadedStateChangedForRow = theHitTestInfo.Row;                                    
                                }
                                else
                                // State will not change. The cell under the mouse will not be selected. This
                                // results in scrolling that will not work when the mouse is moved to the bottom
                                // of the datagrid.
                                //
                                // To solve this, explicitly make the cell under the mouse selected.
                                {
                                    DataGridCell currentSelectedCell = DataGridSpecifySopClasses.CurrentCell;
                                    DataGridCell newSelectedCell = new DataGridCell(theHitTestInfo.Row, currentSelectedCell.ColumnNumber);

                                    DataGridSpecifySopClasses.CurrentCell = newSelectedCell;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void DataGrid_MouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            _MouseEventInfoForDataGrid_LoadedStateChangedForRow = -1;
        }

        private void UpdateRemoveButton()
        {
            bool removeButtonEnabled = false;

            if (ListBoxSpecifySopClassesDefinitionFileDirectories.SelectedItem == null)
            {
                removeButtonEnabled = false;
            }
            else
            {
                if (ListBoxSpecifySopClassesDefinitionFileDirectories.Items.Count > 1)
                {
                    removeButtonEnabled = true;
                }
                else
                {
                    removeButtonEnabled = false;
                }
            }
            ButtonSpecifySopClassesRemoveDirectory.Enabled = removeButtonEnabled;
        }

        private void SelectAllDefinitionFiles()
        {
            int tempCount = 0;
            DataGridBoolColumn dataBoolColumn = new DataGridBoolColumn();
            foreach (DefinitionFile theDefinitionFile in _DefinitionFilesInfoForDataGrid)
            {
                // Get the column style for the "loaded" column.
                DataGridColumnStyle theColStyle = DataGridSpecifySopClasses.TableStyles[0].GridColumnStyles[0];

                // Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
                DataGridSpecifySopClasses.BeginEdit(theColStyle, tempCount);
                theDefinitionFile.Loaded = true;
                DataGridSpecifySopClasses.EndEdit(theColStyle, tempCount, false);

                string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
                theSession.DefinitionManagement.LoadDefinitionFile(theFullFileName);
                tempCount++;
            }

            DataGridSpecifySopClasses.SetDataBinding(_DefinitionFilesInfoForDataGrid, "");            
        }

        private void UnSelectAllDefinitionFiles()
        {
            int tempCount = 0;
            DataGridBoolColumn dataBoolColumn = new DataGridBoolColumn();
            foreach (DefinitionFile theDefinitionFile in _DefinitionFilesInfoForDataGrid)
            {
                string theFullFileName = System.IO.Path.Combine(theDefinitionFile.DefinitionRoot, theDefinitionFile.Filename);
                theSession.DefinitionManagement.UnLoadDefinitionFile(theFullFileName);

                // Get the column style for the "loaded" column.
                DataGridColumnStyle theColStyle = DataGridSpecifySopClasses.TableStyles[0].GridColumnStyles[0];

                // Change the "loaded" stated in _DefinitionFilesInfoForDataGrid and the data grid itself.
                DataGridSpecifySopClasses.BeginEdit(theColStyle, tempCount);
                theDefinitionFile.Loaded = false;
                DataGridSpecifySopClasses.EndEdit(theColStyle, tempCount, false);

                tempCount++;
            }

            DataGridSpecifySopClasses.SetDataBinding(_DefinitionFilesInfoForDataGrid, "");            
        }        
    }    
}