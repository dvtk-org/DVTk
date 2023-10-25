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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Data;
using DvtkData.Dimse;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkApplicationLayer;
//using Dvtk;

namespace DvtkApplicationLayer.UserInterfaces
{
    using HLI = DvtkHighLevelInterface.Dicom.Other;
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class DCMEditor : System.Windows.Forms.UserControl
    {
        private DataGridView dataGridAttributes;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private Microsoft.VisualBasic.Compatibility.VB6.DirListBox dirListBox;
        private Microsoft.VisualBasic.Compatibility.VB6.DriveListBox driveListBox;
        private Microsoft.VisualBasic.Compatibility.VB6.FileListBox fileListBox;
        private ContextMenuStrip contextMenuDataGrid;
        private ToolStripMenuItem menuItem_AddNewAttribute;
        private ToolStripMenuItem menuItem_DeleteAttribute;
        private ToolStripMenuItem menuItem_CopyItem;
        private System.Windows.Forms.Panel panelFMI;
        private System.Windows.Forms.CheckBox checkBoxFMI;
        private System.Windows.Forms.Panel panelFMIDisplay;
        private DataGridView dataGridFMI;
        private IContainer components;
        private MainThread _MainThread = null;
        private ArrayList _AttributesInfoForDataGrid;
        private Rectangle _PreviousBounds = Rectangle.Empty;
        private ArrayList _FMIForDataGrid;
        private string _DCMFileName = "";
        private string _NewDCMFileName = "";
        private string _DataDirectory = Application.StartupPath;
        string _TransferSyntax = "";
        private ArrayList _DefFiles = new ArrayList();
        private bool _IsDCMFileChanged = false;
        private bool _IsSavedInNewDCMFile = false;
        private bool _IsNewDCMFileLoaded = false;
        bool sequenceInSq = false;
        private int levelOfSeq = 0;
        //int selectedRow = -1;
        private AttributeProperty theSelectedAttributeProperty = null;
        private BindingSource fmiBinding = null;
        private BindingSource datasetBinding = null;

        /// <summary>
        /// DCM File Dataset
        /// </summary>
        public static HLI.DataSet _DCMdataset = null;
        private ComboBox fileTypeCombo;
        private Label label1;

        /// <summary>
        /// DCM File FMI
        /// </summary>
        public static FileMetaInformation _FileMetaInfo = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public DCMEditor()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            fileTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            fileTypeCombo.SelectedIndex = 0;

            _DCMdataset = new HLI.DataSet();
            _FileMetaInfo = new FileMetaInformation();

            _AttributesInfoForDataGrid = new ArrayList();
            _FMIForDataGrid = new ArrayList();
            InitializeDatasetGrid();
            InitializeFMIGrid();

            // Get the session context & load definition file
            ThreadManager threadMgr = new ThreadManager();
            _MainThread = new MainThread();
            _MainThread.Initialize(threadMgr);
        }

        /// <summary>
        /// Dynamically create the main data attribute grid.
        /// </summary>
        private void InitializeDatasetGrid()
        {
            //DataGridBoolColumn theBoolColumn = null;
            DataGridViewTextBoxColumn theTextColumn = null;

            // Initialize Attribute DataGrid
            //DataGridTableStyle theStyle = new DataGridTableStyle();

            //theStyle.AllowSorting = true;
            //theStyle.RowHeadersVisible = false;
            //theStyle.MappingName = "ArrayList";

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeTag";
            theTextColumn.HeaderText = "Tag";
            theTextColumn.Width = 110;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeName";
            theTextColumn.HeaderText = "Attribute Name";
            theTextColumn.Width = 200;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVR";
            theTextColumn.HeaderText = "VR";
            theTextColumn.Width = 40;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVM";
            theTextColumn.HeaderText = "VM";
            theTextColumn.Width = 30;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeValue";
            theTextColumn.HeaderText = "Values";
            theTextColumn.Width = 420;
            theTextColumn.ReadOnly = false;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            //dataGridAttributes.TableStyles.Add(theStyle);
        }

        private void InitializeFMIGrid()
        {
            DataGridViewTextBoxColumn theTextColumn = null;

            // Initialize Attribute DataGrid
            //DataGridTableStyle theStyle = new DataGridTableStyle();

            //theStyle.AllowSorting = true;
            //theStyle.RowHeadersVisible = false;
            //theStyle.MappingName = "ArrayList";

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeTag";
            theTextColumn.HeaderText = "Tag";
            theTextColumn.Width = 110;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeName";
            theTextColumn.HeaderText = "Attribute Name";
            theTextColumn.Width = 200;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVR";
            theTextColumn.HeaderText = "VR";
            theTextColumn.Width = 40;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVM";
            theTextColumn.HeaderText = "VM";
            theTextColumn.Width = 30;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeValue";
            theTextColumn.HeaderText = "Values";
            theTextColumn.Width = 420;
            theTextColumn.ReadOnly = true;
            //theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);
            //dataGridFMI.TableStyles.Add(theStyle);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridAttributes = new System.Windows.Forms.DataGridView();
            this.contextMenuDataGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItem_AddNewAttribute = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_DeleteAttribute = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_CopyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dirListBox = new Microsoft.VisualBasic.Compatibility.VB6.DirListBox();
            this.driveListBox = new Microsoft.VisualBasic.Compatibility.VB6.DriveListBox();
            this.fileListBox = new Microsoft.VisualBasic.Compatibility.VB6.FileListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.fileTypeCombo = new System.Windows.Forms.ComboBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.panelFMI = new System.Windows.Forms.Panel();
            this.checkBoxFMI = new System.Windows.Forms.CheckBox();
            this.dataGridFMI = new System.Windows.Forms.DataGridView();
            this.panelFMIDisplay = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAttributes)).BeginInit();
            this.contextMenuDataGrid.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelFMI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFMI)).BeginInit();
            this.panelFMIDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridAttributes
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridAttributes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridAttributes.ContextMenuStrip = this.contextMenuDataGrid;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridAttributes.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridAttributes.Location = new System.Drawing.Point(208, 136);
            this.dataGridAttributes.Name = "dataGridAttributes";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridAttributes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridAttributes.Size = new System.Drawing.Size(605, 520);
            this.dataGridAttributes.TabIndex = 0;
            this.dataGridAttributes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridAttributes_MouseDown);
            // 
            // contextMenuDataGrid
            // 
            this.contextMenuDataGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_AddNewAttribute,
            this.menuItem_DeleteAttribute,
            this.menuItem_CopyItem});
            this.contextMenuDataGrid.Name = "contextMenuDataGrid";
            this.contextMenuDataGrid.Size = new System.Drawing.Size(175, 70);
            // 
            // menuItem_AddNewAttribute
            // 
            this.menuItem_AddNewAttribute.Name = "menuItem_AddNewAttribute";
            this.menuItem_AddNewAttribute.Size = new System.Drawing.Size(174, 22);
            this.menuItem_AddNewAttribute.Text = "Add New Attribute";
            this.menuItem_AddNewAttribute.Visible = false;
            this.menuItem_AddNewAttribute.Click += new System.EventHandler(this.menuItem_AddNewAttribute_Click);
            // 
            // menuItem_DeleteAttribute
            // 
            this.menuItem_DeleteAttribute.Name = "menuItem_DeleteAttribute";
            this.menuItem_DeleteAttribute.Size = new System.Drawing.Size(174, 22);
            this.menuItem_DeleteAttribute.Text = "Delete Attribute";
            this.menuItem_DeleteAttribute.Visible = false;
            this.menuItem_DeleteAttribute.Click += new System.EventHandler(this.menuItem_DeleteAttribute_Click);
            // 
            // menuItem_CopyItem
            // 
            this.menuItem_CopyItem.Name = "menuItem_CopyItem";
            this.menuItem_CopyItem.Size = new System.Drawing.Size(174, 22);
            this.menuItem_CopyItem.Text = "Copy Sequence Item";
            this.menuItem_CopyItem.Visible = false;
            this.menuItem_CopyItem.Click += new System.EventHandler(this.menuItem_CopyItem_Click);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxLog.Location = new System.Drawing.Point(208, 656);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(605, 80);
            this.richTextBoxLog.TabIndex = 1;
            this.richTextBoxLog.Text = "";
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(48, 624);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(104, 32);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save DCM File";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(8, 736);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // dirListBox
            // 
            this.dirListBox.FormattingEnabled = true;
            this.dirListBox.IntegralHeight = false;
            this.dirListBox.Location = new System.Drawing.Point(8, 8);
            this.dirListBox.Name = "dirListBox";
            this.dirListBox.Size = new System.Drawing.Size(192, 240);
            this.dirListBox.TabIndex = 7;
            this.dirListBox.SelectedIndexChanged += new System.EventHandler(this.dirListBox_SelectedIndexChanged);
            this.dirListBox.DoubleClick += new System.EventHandler(this.dirListBox_DoubleClick);
            // 
            // driveListBox
            // 
            this.driveListBox.FormattingEnabled = true;
            this.driveListBox.Location = new System.Drawing.Point(8, 256);
            this.driveListBox.Name = "driveListBox";
            this.driveListBox.Size = new System.Drawing.Size(192, 21);
            this.driveListBox.TabIndex = 8;
            this.driveListBox.SelectedIndexChanged += new System.EventHandler(this.driveListBox_SelectedIndexChanged);
            // 
            // fileListBox
            // 
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.Location = new System.Drawing.Point(8, 288);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Pattern = "*.dcm;*.";
            this.fileListBox.Size = new System.Drawing.Size(192, 290);
            this.fileListBox.TabIndex = 9;
            this.fileListBox.SelectedIndexChanged += new System.EventHandler(this.fileListBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.fileTypeCombo);
            this.panel1.Controls.Add(this.buttonExport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(8, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 736);
            this.panel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 591);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File type";
            // 
            // fileTypeCombo
            // 
            this.fileTypeCombo.DisplayMember = "1";
            this.fileTypeCombo.FormattingEnabled = true;
            this.fileTypeCombo.Items.AddRange(new object[] {
            "DCM files(*.dcm)",
            "All files(*.*)"});
            this.fileTypeCombo.Location = new System.Drawing.Point(59, 588);
            this.fileTypeCombo.Name = "fileTypeCombo";
            this.fileTypeCombo.Size = new System.Drawing.Size(121, 21);
            this.fileTypeCombo.TabIndex = 1;
            this.fileTypeCombo.SelectedIndexChanged += new System.EventHandler(this.filettypeCombo_SelectionChanged);
            // 
            // buttonExport
            // 
            this.buttonExport.Enabled = false;
            this.buttonExport.Location = new System.Drawing.Point(40, 680);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(104, 23);
            this.buttonExport.TabIndex = 0;
            this.buttonExport.Text = "Export to text file";
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // panelFMI
            // 
            this.panelFMI.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelFMI.Controls.Add(this.checkBoxFMI);
            this.panelFMI.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFMI.Location = new System.Drawing.Point(208, 0);
            this.panelFMI.Name = "panelFMI";
            this.panelFMI.Size = new System.Drawing.Size(605, 32);
            this.panelFMI.TabIndex = 11;
            // 
            // checkBoxFMI
            // 
            this.checkBoxFMI.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxFMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFMI.Location = new System.Drawing.Point(16, 8);
            this.checkBoxFMI.Name = "checkBoxFMI";
            this.checkBoxFMI.Size = new System.Drawing.Size(264, 16);
            this.checkBoxFMI.TabIndex = 0;
            this.checkBoxFMI.Text = "File Meta Information(Media Header)";
            this.checkBoxFMI.CheckedChanged += new System.EventHandler(this.checkBoxFMI_CheckedChanged);
            // 
            // dataGridFMI
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridFMI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridFMI.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridFMI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridFMI.Location = new System.Drawing.Point(0, 0);
            this.dataGridFMI.Name = "dataGridFMI";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridFMI.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridFMI.Size = new System.Drawing.Size(605, 104);
            this.dataGridFMI.TabIndex = 1;
            // 
            // panelFMIDisplay
            // 
            this.panelFMIDisplay.Controls.Add(this.dataGridFMI);
            this.panelFMIDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFMIDisplay.Location = new System.Drawing.Point(208, 32);
            this.panelFMIDisplay.Name = "panelFMIDisplay";
            this.panelFMIDisplay.Size = new System.Drawing.Size(605, 104);
            this.panelFMIDisplay.TabIndex = 2;
            this.panelFMIDisplay.Visible = false;
            // 
            // DCMEditor
            // 
            this.AutoScroll = true;
            this.Controls.Add(this.dataGridAttributes);
            this.Controls.Add(this.panelFMIDisplay);
            this.Controls.Add(this.panelFMI);
            this.Controls.Add(this.dirListBox);
            this.Controls.Add(this.fileListBox);
            this.Controls.Add(this.driveListBox);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Name = "DCMEditor";
            this.Size = new System.Drawing.Size(813, 714);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAttributes)).EndInit();
            this.contextMenuDataGrid.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelFMI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFMI)).EndInit();
            this.panelFMIDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Message handlers for displaying drive, directory & file list
        private void fileListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_IsDCMFileChanged)
            {
                DialogResult theDialogResult = MessageBox.Show("The Previous DCM File has unsaved changes.\n\nDo you want to save the changes?",
                    "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                if (theDialogResult == DialogResult.Yes)
                {
                    SaveDCMFile(sender, null);
                }
                else
                {
                    // Do nothing, go ahead with reading the selected DCM file.
                }
                _IsDCMFileChanged = false;
            }

            if (fileListBox.Path.EndsWith(@"\"))
            {
                _DCMFileName = fileListBox.Path + fileListBox.FileName;
            }
            else
            {
                _DCMFileName = fileListBox.Path + @"\" + fileListBox.FileName;
            }

            //Load the DCM File selected by user
            if (!_IsNewDCMFileLoaded)
                LoadDCMFile(DCMFile);
        }

        void LoadDCMFile(string dicomFile)
        {
            DCMFile = dicomFile;

            try
            {
                // Load the Definition Files
                foreach (string defFile in _DefFiles)
                {
                    if (!_MainThread.Options.LoadDefinitionFile(defFile))
                    {
                        string theWarningText = string.Format("The Definition file {0} could not be loaded.\n", defFile);

                        richTextBoxLog.AppendText(theWarningText);
                    }
                }

                _DefFiles.Clear();

                // Set the Data directory
                _MainThread.Options.DataDirectory = _DataDirectory;

                // Read the DCM File
                DicomFile dcmFile = new DicomFile();
                dcmFile.DataSet.UnVrDefinitionLookUpWhenReading = false;
                dcmFile.Read(_DCMFileName, _MainThread);

                // Get the FMI from the selected DCM file
                // Get the FMI from the selected DCM file
                if (_FileMetaInfo == null)
                    _FileMetaInfo = new FileMetaInformation();

                _FileMetaInfo = dcmFile.FileMetaInformation;

                string tsStr;
                if (_FileMetaInfo.Exists("0x00020010"))
                {
                    // Get the Transfer syntax
                    HLI.Attribute tranferSyntaxAttr = _FileMetaInfo["0x00020010"];
                    _TransferSyntax = tranferSyntaxAttr.Values[0];
                    tsStr = _TransferSyntax;
                }
                else
                {
                    _TransferSyntax = "1.2.840.10008.1.2.1";
                    tsStr = "Undefined, the default transfer syntax will Explicit VR Little Endian";
                    _FileMetaInfo.Set("0x00020010", VR.UI, "1.2.840.10008.1.2.1");
                }

                // Get the Data set from the selected DCM file
                if (_DCMdataset == null)
                    _DCMdataset = new HLI.DataSet();

                _DCMdataset = dcmFile.DataSet;

                string theInfoText;
                if (_DCMdataset != null)
                {
                    UpdateAttributeDataGrid();
                    UpdateFMIDataGrid();

                    theInfoText = string.Format("DCM file {0} read successfully with Transfer Syntax {1}.\n\n", _DCMFileName, tsStr);

                    richTextBoxLog.AppendText(theInfoText);
                }
                else
                {
                    theInfoText = string.Format("Error in reading DCM file {0}\n\n", _DCMFileName);

                    richTextBoxLog.AppendText(theInfoText);
                }
            }
            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("DCM file {0} could not be read:\n{1}\n\n", _DCMFileName, exception.Message);

                richTextBoxLog.AppendText(theErrorText);

                _DCMdataset = null;
                _FileMetaInfo = null;
                //dataGridAttributes.SetDataBinding(null, "");
            }


            //Reset the variable 
            _IsNewDCMFileLoaded = false;
        }

        private void dirListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.fileListBox.Path = this.dirListBox.Path;
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText("Error:" + ex.Message);
            }
        }

        private void driveListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.dirListBox.Path = this.driveListBox.Drive;
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText("Error:" + ex.Message);
            }
        }

        private void dirListBox_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                this.fileListBox.Path = this.dirListBox.Path;
                this.fileListBox.Pattern = "*.dcm;*.";
            }
            catch (Exception ex)
            {
                richTextBoxLog.AppendText("Error:" + ex.Message);
            }
        }

        /// <summary>
        /// Loaded DCM File
        /// </summary>
        public string DCMFile
        {
            get
            {
                return _DCMFileName;
            }
            set
            {
                _DCMFileName = value;

                // Provide functionality to open application with DCM File
                if (_DCMFileName != "")
                {
                    //It's a DCM file
                    FileInfo dcmFileInfo = new FileInfo(_DCMFileName.Trim());

                    this.dirListBox.Path = dcmFileInfo.DirectoryName;
                    this.fileListBox.Path = this.dirListBox.Path;
                    this.fileListBox.SelectedItem = dcmFileInfo.Name;
                }
            }
        }

        /// <summary>
        /// Loaded Definition File
        /// </summary>
        public string DefFile
        {
            set
            {
                _DefFiles.Add(value);
            }
        }

        /// <summary>
        /// Boolean for DCM File change
        /// </summary>
        public bool IsDCMFileChanged
        {
            get
            {
                return _IsDCMFileChanged;
            }
        }

        /// <summary>
        /// Transfer syntax of DCM file
        /// </summary>
        public string DCMFileTransferSyntax
        {
            get
            {
                return _TransferSyntax;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DCMFileDataDirectory
        {
            set
            {
                _DataDirectory = value;
            }
        }

        #endregion

        #region Methods for updating Data Grid changes
        /// <summary>
        /// This is the method for updating UI changes
        /// </summary>
        void UpdateAttributeDataGrid()
        {
            _AttributesInfoForDataGrid.Clear();

            AddDatasetInfoToDataGrid();

            //dataGridAttributes.SetDataBinding(null, "");

            //dataGridAttributes.SetDataBinding(_AttributesInfoForDataGrid, "");
            datasetBinding = new BindingSource();
            datasetBinding.DataSource = _AttributesInfoForDataGrid;
            dataGridAttributes.DataSource = datasetBinding;

            //Hide the last HLI attribute object column
            dataGridAttributes.Columns[5].Visible = false;

            // Actually refresh the data grid.
            dataGridAttributes.Refresh();

            // Place focus on the selected attribute
            //if(selectedRow != -1)
            //{
            //    //dataGridAttributes.Select(index);
            //    dataGridAttributes.CurrentCell = new DataGridCell(selectedRow,0);
            //}

            buttonSave.Enabled = true;
            buttonExport.Enabled = true;
        }

        /// <summary>
        /// This is the method for updating FMI grid
        /// </summary>
        public void UpdateFMIDataGrid()
        {
            _FMIForDataGrid.Clear();

            AddFMIToDataGrid();

            //dataGridFMI.SetDataBinding(null, "");

            //dataGridFMI.SetDataBinding(_FMIForDataGrid, "");
            fmiBinding = new BindingSource();
            fmiBinding.DataSource = _FMIForDataGrid;
            dataGridFMI.DataSource = fmiBinding;

            //Hide the last HLI attribute object column
            dataGridFMI.Columns[5].Visible = false;

            // Actually refresh the data grid.
            dataGridFMI.Refresh();
        }
        #endregion

        #region Method for adding Dataset to the datagrid
        /// <summary>
        /// Helper function for adding dataset to the datagrid
        /// </summary>
        void AddDatasetInfoToDataGrid()
        {
            for (int i = 0; i < _DCMdataset.Count; i++)
            {
                HLI.Attribute attribute = _DCMdataset[i];
                string attributesValues = "";
                string attributeName = attribute.Name;
                if (attributeName == "")
                    attributeName = "Undefined";

                if (attribute.VR != VR.SQ)
                {
                    if (attribute.Values.Count != 0)
                    {
                        for (int j = 0; j < attribute.Values.Count; j++)
                        {
                            attributesValues += attribute.Values[j] + "\\";
                        }

                        if (attributesValues.EndsWith("\\"))
                        {
                            // now search for the last "\"...
                            int lastLocation = attributesValues.LastIndexOf("\\");

                            // remove the identified section, if it is a valid region
                            if (lastLocation >= 0)
                                attributesValues = attributesValues.Substring(0, lastLocation);
                        }
                    }
                    else
                    {
                        attributesValues = "";
                    }
                }
                else
                {
                    try
                    {
                        GetSeqAttributesValues(attribute);
                    }
                    catch (Exception exception)
                    {
                        string theErrorText;

                        theErrorText = string.Format("DCM file {0} could not be read:\n{1}\n\n", _DCMFileName, exception.Message);

                        richTextBoxLog.AppendText(theErrorText);
                        richTextBoxLog.ScrollToCaret();
                    }

                    if (sequenceInSq)
                    {
                        --levelOfSeq;
                        sequenceInSq = false;
                    }
                    continue;
                }

                AttributeProperty theDataGridAttributeInfo =
                    new AttributeProperty(TagString(attribute.GroupNumber, attribute.ElementNumber),
                    attributeName,
                    attribute.VR.ToString(),
                    attribute.VM.ToString(),
                    attributesValues,
                    attribute);

                _AttributesInfoForDataGrid.Add(theDataGridAttributeInfo);
            }
        }

        /// <summary>
        /// Helper function for adding FMI to the datagrid
        /// </summary>
        void AddFMIToDataGrid()
        {
            for (int i = 0; i < _FileMetaInfo.Count; i++)
            {
                HLI.Attribute attribute = _FileMetaInfo[i];
                string attributesValues = "";
                string attributeName = attribute.Name;
                if (attributeName == "")
                    attributeName = "Undefined";

                if (attribute.Values.Count != 0)
                {
                    for (int j = 0; j < attribute.Values.Count; j++)
                    {
                        attributesValues += attribute.Values[j] + "\\";
                    }

                    if (attributesValues.EndsWith("\\"))
                    {
                        // now search for the last "\"...
                        int lastLocation = attributesValues.LastIndexOf("\\");

                        // remove the identified section, if it is a valid region
                        if (lastLocation >= 0)
                            attributesValues = attributesValues.Substring(0, lastLocation);
                    }
                }
                else
                {
                    attributesValues = "";
                }

                AttributeProperty theDataGridAttributeInfo =
                    new AttributeProperty(TagString(attribute.GroupNumber, attribute.ElementNumber),
                    attributeName,
                    attribute.VR.ToString(),
                    attribute.VM.ToString(),
                    attributesValues,
                    attribute);

                _FMIForDataGrid.Add(theDataGridAttributeInfo);
            }

            //Pass the DCM Editor object for updating FMI grid
            AttributeProperty attrProperty = new AttributeProperty(this);
        }

        /// <summary>
        /// Recursive function for handling Seq attributes
        /// </summary>
        /// <param name="attribute"></param>
        void GetSeqAttributesValues(HLI.Attribute attribute)
        {
            int itemCount = attribute.ItemCount;
            int sqVm = 1;
            string displayTagString = "";

            for (int i = 0; i < levelOfSeq; i++)
                displayTagString += ">";
            displayTagString += TagString(attribute.GroupNumber, attribute.ElementNumber);

            // Attribute name 
            string attributeName = attribute.Name;
            if (attributeName == "")
                attributeName = "Undefined";

            // Add the sequence attribute to the DataGrid
            AttributeProperty theSeqAttributeInfo =
                new AttributeProperty(displayTagString,
                attributeName,
                "SQ",
                sqVm.ToString(),
                "",
                attribute);

            _AttributesInfoForDataGrid.Add(theSeqAttributeInfo);

            for (int i = 0; i < itemCount; i++)
            {
                string itemString = "";

                for (int j = 0; j < levelOfSeq; j++)
                    itemString += ">";
                itemString += string.Format(">BEGIN ITEM {0}", i + 1);

                AttributeProperty theDataGridItemInfo =
                    new AttributeProperty(itemString,
                    "",
                    "",
                    "",
                    "",
                    attribute);
                _AttributesInfoForDataGrid.Add(theDataGridItemInfo);

                HLI.SequenceItem item = attribute.GetItem(i + 1);
                for (int k = 0; k < item.Count; k++)
                {
                    HLI.Attribute seqAttribute = item[k];
                    string attributesValues = "";
                    string seqTagString = "";
                    string seqAttributeName = seqAttribute.Name;
                    if (seqAttributeName == "")
                        seqAttributeName = "Undefined";

                    if (seqAttribute.VR != VR.SQ)
                    {
                        for (int m = 0; m <= levelOfSeq; m++)
                            seqTagString += ">";
                        seqTagString += TagString(seqAttribute.GroupNumber, seqAttribute.ElementNumber);

                        if (seqAttribute.Values.Count != 0)
                        {
                            for (int l = 0; l < seqAttribute.Values.Count; l++)
                            {
                                attributesValues += seqAttribute.Values[l] + "\\";
                            }

                            if (attributesValues.Trim().EndsWith("\\"))
                            {
                                // now search for the last "\"...
                                int lastLocation = attributesValues.LastIndexOf("\\");

                                // remove the identified section, if it is a valid region
                                if (lastLocation >= 0)
                                    attributesValues = attributesValues.Substring(0, lastLocation);
                            }
                        }
                        else
                        {
                            attributesValues = "";
                        }
                    }
                    else
                    {
                        sequenceInSq = true;
                        levelOfSeq++;
                        GetSeqAttributesValues(seqAttribute);
                        sequenceInSq = false;
                        --levelOfSeq;
                        continue;
                    }

                    AttributeProperty theDataGridSeqAttributeInfo =
                        new AttributeProperty(seqTagString,
                        seqAttributeName,
                        seqAttribute.VR.ToString(),
                        seqAttribute.VM.ToString(),
                        attributesValues,
                        seqAttribute);

                    _AttributesInfoForDataGrid.Add(theDataGridSeqAttributeInfo);
                }

                itemString = "";
                for (int m = 0; m < levelOfSeq; m++)
                    itemString += ">";
                itemString += string.Format(">END ITEM {0}", i + 1);

                AttributeProperty theDataGridEndItemInfo =
                    new AttributeProperty(itemString,
                    "",
                    "",
                    "",
                    "",
                    null);

                _AttributesInfoForDataGrid.Add(theDataGridEndItemInfo);
            }
        }
        #endregion

        #region Message handlers for mouse events in datagrid
        private void dataGridAttributes_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DataGridView.HitTestInfo theHitTestInfo;

            // Find out what part of the data grid is below the mouse pointer.
            theHitTestInfo = dataGridAttributes.HitTest(e.X, e.Y);

            switch (theHitTestInfo.Type)
            {
                case DataGridViewHitTestType.Cell:
                    {
                        menuItem_CopyItem.Visible = false;
                        menuItem_DeleteAttribute.Visible = false;
                        menuItem_AddNewAttribute.Visible = false;
                        menuItem_AddNewAttribute.Text = "Add New Attribute";
                        menuItem_DeleteAttribute.Text = "Delete Attribute";

                        //if(selectedRow != -1)
                        //{
                        //    if(dataGridAttributes.IsSelected(selectedRow))
                        //    {
                        //        dataGridAttributes.UnSelect(selectedRow);
                        //    }
                        //}

                        //selectedRow = theHitTestInfo.Row;
                        theSelectedAttributeProperty = (AttributeProperty)_AttributesInfoForDataGrid[theHitTestInfo.RowIndex];
                        //dataGridAttributes.Select(theHitTestInfo.Row);

                        // Check for BEGIN ITEM & END ITEM rows
                        if ((theSelectedAttributeProperty.AttributeTag.IndexOf("BEGIN") == -1) &&
                            (theSelectedAttributeProperty.AttributeTag.IndexOf("END") == -1))
                        {
                            menuItem_AddNewAttribute.Visible = true;
                            menuItem_DeleteAttribute.Visible = true;
                            menuItem_CopyItem.Visible = false;
                        }
                        else
                        {
                            menuItem_AddNewAttribute.Visible = true;
                            menuItem_CopyItem.Visible = true;
                            menuItem_DeleteAttribute.Visible = true;
                            menuItem_DeleteAttribute.Text = "Delete Sequence Item";
                        }
                        //Check for Sequence attribute
                        if (theSelectedAttributeProperty.AttributeVR == "SQ")
                        {
                            menuItem_AddNewAttribute.Visible = true;
                            menuItem_AddNewAttribute.Text = "Add Sequence Item";
                        }

                        //Check for attributes in a sequence item
                        if (theSelectedAttributeProperty.AttributeTag.StartsWith(">") &&
                          (theSelectedAttributeProperty.AttributeTag.IndexOf("BEGIN") == -1))
                        {
                            menuItem_AddNewAttribute.Visible = false;
                        }

                        if (dataGridAttributes.SelectedRows.Count > 1)
                        {
                            menuItem_AddNewAttribute.Visible = false;
                            menuItem_DeleteAttribute.Visible = true;
                            menuItem_CopyItem.Visible = false;
                            menuItem_DeleteAttribute.Text = "Delete Selected Attributes";
                        }

                        break;
                    }
            }
        }
        #endregion

        #region Message handler for handling DCM file saving functionality

        /// <summary>
        /// Save the DCM File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SaveDCMFile(object sender, System.EventArgs e)
        {
            buttonSave_Click(sender, e);
        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            if (_DCMFileName == "")
            {
                richTextBoxLog.AppendText("There is no DCM File to save, please select DCM file.\n");
                return;
            }

            richTextBoxLog.AppendText("Saving the modified DCM file.\n");

            try
            {
                SaveFileDialog saveAsDlg = new SaveFileDialog();
                saveAsDlg.Filter = "DCM files (*.dcm)|*.dcm|All files (*.*)|*.*";
                FileInfo dcmFileInfo = new FileInfo(_DCMFileName);
                saveAsDlg.InitialDirectory = dcmFileInfo.DirectoryName;
                saveAsDlg.FileName = dcmFileInfo.Name;
                saveAsDlg.Title = "Save DCM File...";
                if (saveAsDlg.ShowDialog() == DialogResult.OK)
                {
                    _NewDCMFileName = saveAsDlg.FileName.Trim();

                    if (_DCMFileName != _NewDCMFileName)
                        _IsSavedInNewDCMFile = true;

                    _DCMFileName = saveAsDlg.FileName;

                    DicomFile dcmFile = new DicomFile();

                    // Add FMI as it is to DCM file
                    dcmFile.FileMetaInformation = _FileMetaInfo;

                    // Add modified dataset to DCM file
                    dcmFile.DataSet = _DCMdataset;

                    string theLogText;

                    dcmFile.Write(_NewDCMFileName);

                    theLogText = string.Format("DCM file {0} saved successfully.\n\n", _NewDCMFileName);
                    richTextBoxLog.AppendText(theLogText);
                    richTextBoxLog.ScrollToCaret();
                    _IsDCMFileChanged = false;
                    _DCMdataset = null;
                    _FileMetaInfo = null;

                    if (_IsSavedInNewDCMFile)
                    {
                        FileInfo newDcmFileInfo = new FileInfo(_NewDCMFileName);

                        _IsNewDCMFileLoaded = true;
                        this.dirListBox.Path = newDcmFileInfo.DirectoryName;
                        this.fileListBox.Path = this.dirListBox.Path;
                        this.fileListBox.SelectedItem = newDcmFileInfo.Name;

                        this.dirListBox.Refresh();
                        this.fileListBox.Refresh();

                        //Load the new DCM file
                        LoadDCMFile(_NewDCMFileName);
                    }
                    else
                    {
                        _IsNewDCMFileLoaded = false;
                        this.dirListBox.Path = dcmFileInfo.DirectoryName;
                        this.fileListBox.Path = this.dirListBox.Path;
                        this.fileListBox.SelectedItem = dcmFileInfo.Name;

                        //Load the new DCM file
                        LoadDCMFile(_DCMFileName);
                    }
                }
            }
            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("DCM file {0} could not be saved:\n{1}\n\n", _NewDCMFileName, exception.Message);

                richTextBoxLog.AppendText(theErrorText);
                richTextBoxLog.ScrollToCaret();

                _IsDCMFileChanged = false;
            }
        }
        #endregion

        #region Helper functions
        /// <summary>
        /// Conversion function for Datagrid tag string format to HLI 
        /// tag sequence string format.
        /// </summary>
        /// <param name="datagridTagString"></param>
        /// <returns></returns>
        string GetHLITagString(string datagridTagString)
        {
            string tagString;
            string hliTagString = "";
            if (datagridTagString.StartsWith(">"))
            {
                int level = 0;
                for (int i = 0; i < datagridTagString.Length; i++)
                {
                    if (datagridTagString[i] == '>')
                    {
                        level++;
                    }
                }
                tagString = datagridTagString.Substring(level);
            }
            else
            {
                tagString = datagridTagString;
            }
            if ((tagString.Trim().StartsWith("(")) &&
                (tagString.Trim().EndsWith(")")))
            {
                string groupString = tagString.Substring(1, 4);
                string elementString = tagString.Substring(6, 4);
                hliTagString = "0x" + groupString + elementString;
            }

            return hliTagString;
        }

        /// <summary>
        /// Conversion function for Tag to string format
        /// </summary>
        /// <param name="groupNr"></param>
        /// <param name="elementNr"></param>
        /// <returns></returns>
        string TagString(UInt16 groupNr, UInt16 elementNr)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Byte[] groupByteArray = System.BitConverter.GetBytes(groupNr);
            System.Byte[] elementByteArray = System.BitConverter.GetBytes(elementNr);
            if (System.BitConverter.IsLittleEndian)
            {
                // Display as Big Endian
                System.Array.Reverse(groupByteArray);
                System.Array.Reverse(elementByteArray);
            }
            string hexByteStr0, hexByteStr1;

            hexByteStr0 = groupByteArray[0].ToString("x");
            if (hexByteStr0.Length == 1) hexByteStr0 = "0" + hexByteStr0; // prepend with leading zero
            hexByteStr1 = groupByteArray[1].ToString("x");
            if (hexByteStr1.Length == 1) hexByteStr1 = "0" + hexByteStr1; // prepend with leading zero

            sb.AppendFormat("({0}{1},", hexByteStr0, hexByteStr1);

            hexByteStr0 = elementByteArray[0].ToString("x");
            if (hexByteStr0.Length == 1) hexByteStr0 = "0" + hexByteStr0; // prepend with leading zero
            hexByteStr1 = elementByteArray[1].ToString("x");
            if (hexByteStr1.Length == 1) hexByteStr1 = "0" + hexByteStr1; // prepend with leading zero

            sb.AppendFormat("{0}{1})", hexByteStr0, hexByteStr1);
            return sb.ToString();
        }

        /// <summary>
        /// Conversion function for VR to string format
        /// </summary>
        /// <param name="vrString"></param>
        /// <returns></returns>
        DvtkData.Dimse.VR GetVR(string vrString)
        {
            if (vrString == "AE") return VR.AE;
            if (vrString == "AS") return VR.AS;
            if (vrString == "AT") return VR.AT;
            if (vrString == "CS") return VR.CS;
            if (vrString == "DA") return VR.DA;
            if (vrString == "DS") return VR.DS;
            if (vrString == "DT") return VR.DT;
            if (vrString == "FL") return VR.FL;
            if (vrString == "FD") return VR.FD;
            if (vrString == "IS") return VR.IS;
            if (vrString == "LO") return VR.LO;
            if (vrString == "LT") return VR.LT;
            if (vrString == "OB") return VR.OB;
            if (vrString == "OF") return VR.OF;
            if (vrString == "OW") return VR.OW;
            if (vrString == "OD") return VR.OD;
            if (vrString == "OL") return VR.OL;
            if (vrString == "OV") return VR.OV;
            if (vrString == "PN") return VR.PN;
            if (vrString == "SH") return VR.SH;
            if (vrString == "SL") return VR.SL;
            if (vrString == "SQ") return VR.SQ;
            if (vrString == "SS") return VR.SS;
            if (vrString == "ST") return VR.ST;
            if (vrString == "SV") return VR.SV;
            if (vrString == "TM") return VR.TM;
            if (vrString == "UI") return VR.UI;
            if (vrString == "UL") return VR.UL;
            if (vrString == "UN") return VR.UN;
            if (vrString == "US") return VR.US;
            if (vrString == "UT") return VR.UT;
            if (vrString == "UV") return VR.UV;
            // Unknown DicomValueType
            throw new System.NotImplementedException();
        }

        private int GetItemNr(string itemStr)
        {
            // now search for the item nr
            string itemNrStr = "";
            int itemNr = 0;
            itemNrStr = itemStr.Substring(12);
            itemNr = int.Parse(itemNrStr.Trim());
            return itemNr;
        }
        #endregion

        #region Methods for adding/deleting new attributes & items to DCM file
        /// <summary>
        /// Add a new attribute to the DCM File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_AddNewAttribute_Click(object sender, System.EventArgs e)
        {
            AddAttribute newAttribute = new AddAttribute();

            if (menuItem_AddNewAttribute.Text == "Add Sequence Item")
            {
                HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;
                attribute.AddItem(new HLI.SequenceItem());
                _IsDCMFileChanged = true;
                UpdateAttributeDataGrid();
                return;
            }

            // Show the save form to the user.
            DialogResult theDialogResult = newAttribute.ShowDialog();

            if (theDialogResult == DialogResult.OK)
            // User pressed ok.
            {
                int valueMultiplier = 0;
                string[] values = null;

                try
                {
                    //Check for sequence item
                    if (theSelectedAttributeProperty.AttributeTag.IndexOf("BEGIN") != -1)
                    {
                        HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;

                        int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);

                        int itemCount = attribute.ItemCount;
                        if (itemCount > 0)
                        {
                            //Get the attribute tag string
                            string attributeTagString = GetHLITagString(newAttribute.NewAttributeTag);

                            HLI.SequenceItem item = attribute.GetItem(itemNr);

                            if (newAttribute.NewAttributeVR == "SQ")
                            {
                                //AddNestedSeqAttribute(newAttribute,index,nestingOfSeq, tagSequence);
                                item.Set(attributeTagString, VR.SQ);
                                HLI.Attribute nestedSeqAttribute = item[attributeTagString];
                                nestedSeqAttribute.AddItem(new HLI.SequenceItem());
                            }
                            else
                            {
                                if (newAttribute.NewAttributeValue.Length != 0)
                                {
                                    // Split attribute values string 
                                    values = newAttribute.NewAttributeValue.Split('\\');
                                    valueMultiplier = values.Length;
                                }

                                // Get the existing attribute in Dataset
                                item.Set(attributeTagString, GetVR(newAttribute.NewAttributeVR), values);
                            }
                        }
                    }
                    else
                    {
                        //Check for Sequence attribute
                        if (theSelectedAttributeProperty.AttributeVR != "SQ")
                        {
                            //Check for duplication of new attribute
                            foreach (AttributeProperty attributeProperty in _AttributesInfoForDataGrid)
                            {
                                if (attributeProperty.AttributeTag == newAttribute.NewAttributeTag)
                                {
                                    string msg = "This attribute is already exist in DCM File.\n";
                                    MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }

                            //Handle Sequence attribute seperately
                            if (newAttribute.NewAttributeVR == "SQ")
                            {
                                //Add attribute to the dataset and an empty sequence item
                                string tagString = GetHLITagString(newAttribute.NewAttributeTag);
                                _DCMdataset.Set(tagString, VR.SQ);
                                HLI.Attribute attribute = _DCMdataset[tagString];
                            }
                            else
                            {
                                if (newAttribute.NewAttributeValue.Length != 0)
                                {
                                    // Split attribute values string 
                                    values = newAttribute.NewAttributeValue.Split('\\');
                                    valueMultiplier = values.Length;
                                }

                                //Add attribute to the dataset
                                string tagString = GetHLITagString(newAttribute.NewAttributeTag);
                                _DCMdataset.Set(tagString, GetVR(newAttribute.NewAttributeVR), values);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    string theErrorText;

                    theErrorText = string.Format("The new attribute {0} could not be added due to exception:\n {1}\n\n",
                        newAttribute.NewAttributeTag, exception.Message);

                    richTextBoxLog.AppendText(theErrorText);
                    richTextBoxLog.ScrollToCaret();

                    _IsDCMFileChanged = false;
                    return;
                }

                _IsDCMFileChanged = true;
                UpdateAttributeDataGrid();

                //Check for new attribute in the grid and select it
                //int index = 0;
                //foreach (AttributeProperty attributeProperty in _AttributesInfoForDataGrid)
                //{
                //    if(attributeProperty.AttributeTag == newAttribute.NewAttributeTag)
                //    {
                //        DataGridViewCell cell = dataGridAttributes[0, index];
                //        break;
                //    }
                //    index++;
                //}
            }
            else
            // User pressed cancel.
            {
                return;
            }
        }

        private void menuItem_DeleteAttribute_Click(object sender, System.EventArgs e)
        {
            try
            {
                HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;
                if (menuItem_DeleteAttribute.Text == "Delete Sequence Item")
                {
                    int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);
                    int itemCount = attribute.ItemCount;
                    if (itemNr <= itemCount)
                    {
                        attribute.RemoveItemAt(itemNr);
                    }
                }
                else if (menuItem_DeleteAttribute.Text == "Delete Selected Attributes")
                {
                    foreach (DataGridViewRow row in dataGridAttributes.SelectedRows)
                    {
                        AttributeProperty selectedAttributeProperty = (AttributeProperty)_AttributesInfoForDataGrid[row.Index];
                        selectedAttributeProperty.HLIAttribute.Delete();
                    }
                }
                else
                {
                    attribute.Delete();
                }
            }
            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("The selected attribute/item could not be deleted.\n\n",
                    exception.Message);

                richTextBoxLog.AppendText(theErrorText);
                richTextBoxLog.ScrollToCaret();

                _IsDCMFileChanged = false;
                return;
            }

            _IsDCMFileChanged = true;
            UpdateAttributeDataGrid();
        }

        private void menuItem_CopyItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;

                int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);

                int itemCount = attribute.ItemCount;
                if (itemCount > 0)
                {
                    // Get the first sequence Item and add the clone of 
                    // seq item to seq attribute
                    HLI.SequenceItem item = attribute.GetItem(itemNr);
                    attribute.AddItem(item);
                }
            }
            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("The selected sequence item could not be copied to \nsequence attribute due to exception:\n {0}\n\n",
                    exception.Message);

                richTextBoxLog.AppendText(theErrorText);
                richTextBoxLog.ScrollToCaret();

                _IsDCMFileChanged = false;
                return;
            }

            _IsDCMFileChanged = true;
            UpdateAttributeDataGrid();
        }

        private void buttonExport_Click(object sender, System.EventArgs e)
        {
            StringBuilder s = new StringBuilder(500);
            foreach (AttributeProperty fmiAttributeProperty in _FMIForDataGrid)
            {
                string attributeValue = fmiAttributeProperty.AttributeValue;
                if (attributeValue == "")
                    attributeValue = "\t";
                s.Append("(" + GetHLITagString(fmiAttributeProperty.AttributeTag) + "," + "\t" +
                    fmiAttributeProperty.AttributeVR + "," + "\t" +
                    attributeValue + ")" + "\t" +
                    "#" + fmiAttributeProperty.AttributeName + "\r\n");
            }

            foreach (AttributeProperty attributeProperty in _AttributesInfoForDataGrid)
            {
                bool isSqAttrItem = attributeProperty.AttributeTag.StartsWith(">");
                if ((attributeProperty.AttributeVR != "SQ") && (!isSqAttrItem))
                {
                    string attributeValue = attributeProperty.AttributeValue;
                    if (attributeValue == "")
                        attributeValue = "\t";
                    s.Append("(" + GetHLITagString(attributeProperty.AttributeTag) + "," + "\t" +
                        attributeProperty.AttributeVR + "," + "\t" +
                        attributeValue + ")" + "\t" +
                        "#" + attributeProperty.AttributeName + "\r\n");
                }
                else
                {
                    string attributeTag = "";
                    int level = 0;
                    if (attributeProperty.AttributeTag.StartsWith(">"))
                    {
                        for (int i = 0; i < attributeProperty.AttributeTag.Length; i++)
                        {
                            if (attributeProperty.AttributeTag[i] == '>')
                            {
                                level++;
                            }
                        }

                        for (int j = 0; j < level; j++)
                        {
                            attributeTag += "  ";
                        }

                        if ((attributeProperty.AttributeTag.IndexOf("BEGIN") != -1) ||
                            (attributeProperty.AttributeTag.IndexOf("END") != -1))
                        {
                            attributeTag += attributeProperty.AttributeTag;
                            s.Append(attributeTag + "\r\n");
                        }
                        else
                        {
                            for (int k = 0; k < level; k++)
                            {
                                attributeTag += ">";
                            }
                            attributeTag += GetHLITagString(attributeProperty.AttributeTag);
                            string attributeValue = attributeProperty.AttributeValue;
                            if (attributeValue == "")
                                attributeValue = "\t";
                            s.Append(attributeTag + "," + "\t" +
                                attributeProperty.AttributeVR + "," + "\t" +
                                attributeValue + "\t" +
                                "#" + attributeProperty.AttributeName + "\r\n");
                        }
                    }
                    else
                    {
                        s.Append("(" + GetHLITagString(attributeProperty.AttributeTag) + "," + "\t" +
                            attributeProperty.AttributeVR + "\t\t" +
                            "#" + attributeProperty.AttributeName + "\r\n");
                    }
                }
            }

            string txtFileName = "";
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Title = "Export to the text file";
            saveFileDlg.Filter = "Text file (*.txt) |*.txt";
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                txtFileName = saveFileDlg.FileName;
                StreamWriter textFile = File.CreateText(txtFileName);
                if (textFile != null)
                {
                    textFile.Write(s);
                    textFile.Flush();
                }

                textFile.Close();
                textFile = null;

                string msg = string.Format("The DCM File is exported to {0} successfully.\n\n", txtFileName);
                richTextBoxLog.AppendText(msg);
                richTextBoxLog.ScrollToCaret();
            }
        }

        private void checkBoxFMI_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxFMI.Checked)
            {
                panelFMIDisplay.Visible = true;
            }
            else
            {
                panelFMIDisplay.Visible = false;
            }
        }

        private void filettypeCombo_SelectionChanged(object sender, EventArgs e)
        {
            if (fileTypeCombo.SelectedItem.ToString() == "DCM files(*.dcm)")
                fileListBox.Pattern = "*.dcm";
            else
                fileListBox.Pattern = "*.*";
        }
    }
    #endregion

    /// <summary>
    /// Summary description for Main Thread.
    /// </summary>
    public class MainThread : DicomThread
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainThread()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void Execute()
        { }
    }

    /// <summary>
    /// This class is used to represent each row in the datagrid. Each row
    /// in the datagrid refer to a HLI attribute object and HLI attribute tag sequence.
    /// </summary>
    public class AttributeProperty
    {
        private static DCMEditor editorObj = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="editor"></param>
        public AttributeProperty(DCMEditor editor)
        {
            editorObj = editor;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="displayTag"></param>
        /// <param name="name"></param>
        /// <param name="vr"></param>
        /// <param name="vm"></param>
        /// <param name="val"></param>
        /// <param name="refAttributeObj"></param>
        public AttributeProperty(string displayTag,
            string name,
            string vr,
            string vm,
            string val,
            HLI.Attribute refAttributeObj)
        {
            this.properties[attributeTag] = displayTag;
            this.properties[attributeName] = name;
            this.properties[attributeVR] = vr;
            this.properties[attributeVM] = vm;
            this.properties[attributeValue] = val;

            this.hliAttributeObj = refAttributeObj;
        }

        /// <summary>
        /// Indexer property
        /// </summary>
        public string this[int propertyIndex]
        {
            get
            {
                return properties[propertyIndex];
            }
            set
            {
                properties[propertyIndex] = value;
            }
        }

        /// <summary>
        /// Attribute Tag
        /// </summary>
        public string AttributeTag
        {
            get { return this[attributeTag]; }
            set { this[attributeTag] = value; }
        }

        /// <summary>
        /// Attribute Name
        /// </summary>
        public string AttributeName
        {
            get { return this[attributeName]; }
            set { this[attributeName] = value; }
        }

        /// <summary>
        /// Attribute VR
        /// </summary>
        public string AttributeVR
        {
            get { return this[attributeVR]; }
            set
            {
                this[attributeVR] = value;
            }
        }

        /// <summary>
        /// Attribute VM
        /// </summary>
        public string AttributeVM
        {
            get { return this[attributeVM]; }
            set { this[attributeVM] = value; }
        }

        /// <summary>
        /// Attribute Value
        /// </summary>
        public string AttributeValue
        {
            get { return this[attributeValue]; }
            set
            {
                if (value != null)
                {
                    string oldValue = this[attributeValue];

                    //Set the new values only when it is changed
                    if (oldValue != value)
                    {
                        this[attributeValue] = value;

                        string[] values = null;

                        //Clear previous values and add new values
                        this.hliAttributeObj.Values.Clear();

                        // Split attribute values string except pixel data attribute
                        if ((this.hliAttributeObj.GroupNumber == 0x7FE0) &&
                            (this.hliAttributeObj.ElementNumber == 0x0010))
                        {
                            this.hliAttributeObj.Values.Add(value);
                        }
                        else
                        {
                            values = value.Split('\\');
                            for (int i = 0; i < values.Length; i++)
                            {
                                this.hliAttributeObj.Values.Add(values[i]);
                            }
                        }

                        //If attributes SOP class UID and SOP Instance UID's 
                        //are modified in dataset, update the attribute values in FMI also.
                        if ((this.hliAttributeObj.GroupNumber == 0x0008) &&
                            ((this.hliAttributeObj.ElementNumber == 0x0016) ||
                            (this.hliAttributeObj.ElementNumber == 0x0018)))
                        {
                            if (this.hliAttributeObj.ElementNumber == 0x0016)
                            {
                                // Set the modified SOP class UID attribute
                                DCMEditor._FileMetaInfo.Set("0x00020002", VR.UI, values[0]);
                            }
                            else
                            {
                                // Set the modified SOP Instance UID attribute
                                DCMEditor._FileMetaInfo.Set("0x00020003", VR.UI, values[0]);
                            }

                            if (editorObj != null)
                                editorObj.UpdateFMIDataGrid();
                        }
                    }
                }
                else
                {
                    this[attributeValue] = "";
                    this.hliAttributeObj.Values.Clear();
                    this.hliAttributeObj.Values.Add();

                    if ((this.hliAttributeObj.GroupNumber == 0x0008) &&
                           ((this.hliAttributeObj.ElementNumber == 0x0016) ||
                           (this.hliAttributeObj.ElementNumber == 0x0018)))
                    {
                        if (this.hliAttributeObj.ElementNumber == 0x0016)
                        {
                            // Set the modified SOP class UID attribute
                            DCMEditor._FileMetaInfo.Set("0x00020002", VR.UI);
                        }
                        else
                        {
                            // Set the modified SOP Instance UID attribute
                            DCMEditor._FileMetaInfo.Set("0x00020003", VR.UI);
                        }

                        if (editorObj != null)
                            editorObj.UpdateFMIDataGrid();
                    }
                }
            }
        }

        /// <summary>
        /// HLI Attribute
        /// </summary>
        public HLI.Attribute HLIAttribute
        {
            get { return this.hliAttributeObj; }
        }

        // HLI attribute object referred by each Datagrid row
        private HLI.Attribute hliAttributeObj;

        //Used by indexer property
        private string[] properties = new string[5];
        private const int attributeTag = 0;
        private const int attributeName = 1;
        private const int attributeVR = 2;
        private const int attributeVM = 3;
        private const int attributeValue = 4;
    }
}
