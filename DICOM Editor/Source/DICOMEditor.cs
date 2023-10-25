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
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkApplicationLayer;
using DvtkApplicationLayer.UserInterfaces;
using Dvtk.Sessions;

namespace DCMEditor
{
	using HLI = DvtkHighLevelInterface.Dicom.Other;
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class DICOMEditor : System.Windows.Forms.Form
    {
		private ContextMenuStrip contextMenuDataGrid;
        private ToolStripMenuItem menuItem_AddNewAttribute;
		private MenuStrip mainMenuDCMEditor;
        private ToolStripMenuItem menuItemFile;
        private ToolStripMenuItem menuItemHelp;
        private ToolStripMenuItem menuItemAbout;
        private ToolStripMenuItem menuItemSave;
        private ToolStripMenuItem menuItemExit;
        private ToolStripMenuItem menuItem_DeleteAttribute;
        private ToolStripMenuItem menuItem_CopyItem;
        private IContainer components;
        private ToolStripMenuItem menuItemSaveAs;
        private ToolStripMenuItem menuItemSaveAsILE;
        private ToolStripMenuItem menuItemSaveAsELE;
        private ToolStripMenuItem menuItemSaveAsEBE;
        private BackgroundWorker backgroundWorkerEditor;
        private SplitContainer splitContainer1;
        private RichTextBox richTextBoxLog;
        private Panel panelFMI;
        private CheckBox checkBoxFMI;
        private DataGridView dataGridAttributes;
        private Microsoft.VisualBasic.Compatibility.VB6.FileListBox fileListBox;
        private Microsoft.VisualBasic.Compatibility.VB6.DriveListBox driveListBox;
        private Microsoft.VisualBasic.Compatibility.VB6.DirListBox dirListBox;
        private Panel panelFMIDisplay;
        private DataGridView dataGridFMI;
        private Panel panel1;
        private Button buttonSave;
        private Button buttonExport;
        private ToolStripMenuItem findSearchAttributeToolStripMenuItem;

		public static HLI.DataSet _DCMdataset = null;
		public static FileMetaInformation _FileMetaInfo = null;

        private MainThread _MainThread = null;
		private ArrayList _AttributesInfoForDataGrid;
		private ArrayList _FMIForDataGrid;

        public static ArrayList _RefFileDatasets = new ArrayList();
        public static ArrayList _RefFileFMIs = new ArrayList();
        public static ArrayList _RefFileNames = new ArrayList();

        public bool IsRefFilesReadCompleted = false;

		private Rectangle _PreviousBounds = Rectangle.Empty;
		private string _DCMFileName;
		private string _NewDCMFileName;
		string _TransferSyntax;
		static private string dcmFileOrDirToBeOpened = "";
		private bool _IsDCMFileChanged = false;
		private bool _IsDCMFileLoaded = false;
		private bool _IsSavedInNewDCMFile = false;
		private bool _IsNewDCMFileLoaded = false;
		private bool _IsDICOMDIR = false;
		private bool _IsAttributeGroupLengthDefined = false;
		bool sequenceInSq = false;
		private int levelOfSeq = 0;
		int selectedRow = -1;
		private AttributeProperty theSelectedAttributeProperty = null;
        private HLI.SequenceItem copySequenceItemBuffer = null;
        private HLI.AttributeCollection copyAttributes = null;
		private Dvtk.Events.ActivityReportEventHandler activityReportEventHandler;
        private BindingSource fmiBinding = null;
        private BindingSource datasetBinding = null;
        private ToolStripMenuItem menuItem_PasteItem;
        private ToolStripMenuItem menuItem_InsertAbove;
        private ToolStripMenuItem menuItem_InsertBelow;
        private CheckBox checkBoxEditFMI;
        private ToolStripMenuItem menuItem_CopyAttribute;
        private ToolStripMenuItem menuItem_PasteAttributes;
        private CheckBox privateAttributeMappingBox;
                        							
		delegate void appendTextToActivityLogging_ThreadSafe_Delegate(string theText);
		private appendTextToActivityLogging_ThreadSafe_Delegate activityLoggingDelegate = null;

		public DICOMEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Initialize the Dvtk library
			Dvtk.Setup.Initialize();

			_DCMdataset = new HLI.DataSet();
			_FileMetaInfo = new FileMetaInformation();

			_AttributesInfoForDataGrid = new ArrayList();
			_FMIForDataGrid = new ArrayList();
			InitializeDatasetGrid();
			InitializeFMIGrid();

			// Get the session context 
			ThreadManager threadMgr = new ThreadManager();
			_MainThread = new MainThread();
			_MainThread.Initialize(threadMgr);
	
			// Subscribe to Dvtk Activity report handler
			activityReportEventHandler = new Dvtk.Events.ActivityReportEventHandler(OnActivityReportEvent);

			// Subscribe to Sniffer Activity report handler
			activityLoggingDelegate = new appendTextToActivityLogging_ThreadSafe_Delegate(this.AppendTextToActivityLogging_ThreadSafe);

            // Provide functionality to open application with Media file or Directory
            // which contains Media files
			if(dcmFileOrDirToBeOpened != "")
			{
				DirectoryInfo userDirInfo = new DirectoryInfo(dcmFileOrDirToBeOpened.Trim());
				if(userDirInfo.Exists)
				{
                    //It's a directory contains Media files
					this.dirListBox.Path = userDirInfo.FullName;
					this.fileListBox.Path = this.dirListBox.Path;
				}
				else
				{
                    //It's a Media file
					FileInfo dcmFileInfo = new FileInfo(dcmFileOrDirToBeOpened.Trim());

					this.dirListBox.Path = dcmFileInfo.DirectoryName;
					this.fileListBox.Path = this.dirListBox.Path;
					this.fileListBox.SelectedItem = dcmFileInfo.Name;
				}				
			}
		}

		/// <summary>
		/// Dynamically create the main dataset attributes grid.
		/// </summary>
		private void InitializeDatasetGrid()
		{
			DataGridViewTextBoxColumn theTextColumn = null;

			// Initialize Attribute DataGrid    
            theTextColumn = new DataGridViewTextBoxColumn();
			theTextColumn.DataPropertyName = "AttributeTag";
			theTextColumn.HeaderText = "Tag";
			theTextColumn.Width = 130;
			theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeName";
			theTextColumn.HeaderText = "Attribute Name";
			theTextColumn.Width = 250;
			theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVR";
			theTextColumn.HeaderText = "Def VR";
			theTextColumn.Width = 60;
			theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVM";
			theTextColumn.HeaderText = "VM";
			theTextColumn.Width = 40;
			theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridAttributes.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeValue";
			theTextColumn.HeaderText = "Values";
			theTextColumn.Width = 600;
			theTextColumn.ReadOnly = false;
            dataGridAttributes.Columns.Add(theTextColumn);

			UpdateTitleBarText();
		}

		/// <summary>
		/// Dynamically create the main FMI attributes grid.
		/// </summary>
		private void InitializeFMIGrid()
		{
            DataGridViewTextBoxColumn theTextColumn = null;

			// Initialize Attribute DataGrid
            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeTag";
			theTextColumn.HeaderText = "Tag";
			theTextColumn.Width = 130;
            theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeName";
			theTextColumn.HeaderText = "Attribute Name";
			theTextColumn.Width = 250;
            theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVR";
			theTextColumn.HeaderText = "Def VR";
			theTextColumn.Width = 50;
            theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeVM";
			theTextColumn.HeaderText = "VM";
			theTextColumn.Width = 40;
            theTextColumn.ReadOnly = true;
			//theStyle.GridColumnStyles.Add(theTextColumn);
            dataGridFMI.Columns.Add(theTextColumn);

            theTextColumn = new DataGridViewTextBoxColumn();
            theTextColumn.DataPropertyName = "AttributeValue";
			theTextColumn.HeaderText = "Values";
			theTextColumn.Width = 600;
            theTextColumn.ReadOnly = true;
            dataGridFMI.Columns.Add(theTextColumn);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DICOMEditor));
            this.contextMenuDataGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItem_AddNewAttribute = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_DeleteAttribute = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_CopyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findSearchAttributeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_PasteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_InsertAbove = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_InsertBelow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_CopyAttribute = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_PasteAttributes = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuDCMEditor = new System.Windows.Forms.MenuStrip();
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAsILE = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAsELE = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSaveAsEBE = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorkerEditor = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.fileListBox = new Microsoft.VisualBasic.Compatibility.VB6.FileListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.driveListBox = new Microsoft.VisualBasic.Compatibility.VB6.DriveListBox();
            this.dirListBox = new Microsoft.VisualBasic.Compatibility.VB6.DirListBox();
            this.dataGridAttributes = new System.Windows.Forms.DataGridView();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.panelFMIDisplay = new System.Windows.Forms.Panel();
            this.dataGridFMI = new System.Windows.Forms.DataGridView();
            this.panelFMI = new System.Windows.Forms.Panel();
            this.checkBoxEditFMI = new System.Windows.Forms.CheckBox();
            this.checkBoxFMI = new System.Windows.Forms.CheckBox();
            this.privateAttributeMappingBox = new System.Windows.Forms.CheckBox();
            this.contextMenuDataGrid.SuspendLayout();
            this.mainMenuDCMEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAttributes)).BeginInit();
            this.panelFMIDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFMI)).BeginInit();
            this.panelFMI.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuDataGrid
            // 
            this.contextMenuDataGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_AddNewAttribute,
            this.menuItem_DeleteAttribute,
            this.menuItem_CopyItem,
            this.findSearchAttributeToolStripMenuItem,
            this.menuItem_PasteItem,
            this.menuItem_InsertAbove,
            this.menuItem_InsertBelow,
            this.menuItem_CopyAttribute,
            this.menuItem_PasteAttributes});
            this.contextMenuDataGrid.Name = "contextMenuDataGrid";
            this.contextMenuDataGrid.Size = new System.Drawing.Size(269, 220);
            // 
            // menuItem_AddNewAttribute
            // 
            this.menuItem_AddNewAttribute.Name = "menuItem_AddNewAttribute";
            this.menuItem_AddNewAttribute.Size = new System.Drawing.Size(268, 24);
            this.menuItem_AddNewAttribute.Text = "Add New Attribute";
            this.menuItem_AddNewAttribute.Visible = false;
            this.menuItem_AddNewAttribute.Click += new System.EventHandler(this.menuItem_AddNewAttribute_Click);
            // 
            // menuItem_DeleteAttribute
            // 
            this.menuItem_DeleteAttribute.Name = "menuItem_DeleteAttribute";
            this.menuItem_DeleteAttribute.Size = new System.Drawing.Size(268, 24);
            this.menuItem_DeleteAttribute.Text = "Delete Attribute";
            this.menuItem_DeleteAttribute.Visible = false;
            this.menuItem_DeleteAttribute.Click += new System.EventHandler(this.menuItem_DeleteAttribute_Click);
            // 
            // menuItem_CopyItem
            // 
            this.menuItem_CopyItem.Name = "menuItem_CopyItem";
            this.menuItem_CopyItem.Size = new System.Drawing.Size(268, 24);
            this.menuItem_CopyItem.Text = "Copy Sequence Item";
            this.menuItem_CopyItem.Visible = false;
            this.menuItem_CopyItem.Click += new System.EventHandler(this.menuItem_CopyItem_Click);
            // 
            // findSearchAttributeToolStripMenuItem
            // 
            this.findSearchAttributeToolStripMenuItem.Name = "findSearchAttributeToolStripMenuItem";
            this.findSearchAttributeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findSearchAttributeToolStripMenuItem.Size = new System.Drawing.Size(268, 24);
            this.findSearchAttributeToolStripMenuItem.Text = "Find/Search Attribute";
            this.findSearchAttributeToolStripMenuItem.Visible = false;
            this.findSearchAttributeToolStripMenuItem.Click += new System.EventHandler(this.findSearchAttributeToolStripMenuItem_Click);
            // 
            // menuItem_PasteItem
            // 
            this.menuItem_PasteItem.Name = "menuItem_PasteItem";
            this.menuItem_PasteItem.Size = new System.Drawing.Size(268, 24);
            this.menuItem_PasteItem.Text = "Paste Sequence Item to End";
            this.menuItem_PasteItem.Visible = false;
            this.menuItem_PasteItem.Click += new System.EventHandler(this.menuItem_PasteItem_Click);
            // 
            // menuItem_InsertAbove
            // 
            this.menuItem_InsertAbove.Name = "menuItem_InsertAbove";
            this.menuItem_InsertAbove.Size = new System.Drawing.Size(268, 24);
            this.menuItem_InsertAbove.Text = "Insert Sequence Item Above";
            this.menuItem_InsertAbove.Visible = false;
            this.menuItem_InsertAbove.Click += new System.EventHandler(this.menuItem_InsertAbove_Click);
            // 
            // menuItem_InsertBelow
            // 
            this.menuItem_InsertBelow.Name = "menuItem_InsertBelow";
            this.menuItem_InsertBelow.Size = new System.Drawing.Size(268, 24);
            this.menuItem_InsertBelow.Text = "Insert Sequence Item Below";
            this.menuItem_InsertBelow.Visible = false;
            this.menuItem_InsertBelow.Click += new System.EventHandler(this.menuItem_InsertBelow_Click);
            // 
            // menuItem_CopyAttribute
            // 
            this.menuItem_CopyAttribute.Name = "menuItem_CopyAttribute";
            this.menuItem_CopyAttribute.Size = new System.Drawing.Size(268, 24);
            this.menuItem_CopyAttribute.Text = "Copy Attribute(s)";
            this.menuItem_CopyAttribute.Click += new System.EventHandler(this.menuItem_CopyAttribute_Click);
            // 
            // menuItem_PasteAttributes
            // 
            this.menuItem_PasteAttributes.Name = "menuItem_PasteAttributes";
            this.menuItem_PasteAttributes.Size = new System.Drawing.Size(268, 24);
            this.menuItem_PasteAttributes.Text = "Paste Attribute(s)";
            this.menuItem_PasteAttributes.Click += new System.EventHandler(this.menuItem_PasteAttributes_Click);
            // 
            // mainMenuDCMEditor
            // 
            this.mainMenuDCMEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemHelp});
            this.mainMenuDCMEditor.Location = new System.Drawing.Point(0, 0);
            this.mainMenuDCMEditor.Name = "mainMenuDCMEditor";
            this.mainMenuDCMEditor.Size = new System.Drawing.Size(1110, 28);
            this.mainMenuDCMEditor.TabIndex = 0;
            // 
            // menuItemFile
            // 
            this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSave,
            this.menuItemSaveAs,
            this.menuItemExit});
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(44, 24);
            this.menuItemFile.Text = "&File";
            // 
            // menuItemSave
            // 
            this.menuItemSave.Enabled = false;
            this.menuItemSave.Name = "menuItemSave";
            this.menuItemSave.Size = new System.Drawing.Size(206, 24);
            this.menuItemSave.Text = "&Save DICOM file";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemSaveAs
            // 
            this.menuItemSaveAs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSaveAsILE,
            this.menuItemSaveAsELE,
            this.menuItemSaveAsEBE});
            this.menuItemSaveAs.Enabled = false;
            this.menuItemSaveAs.Name = "menuItemSaveAs";
            this.menuItemSaveAs.Size = new System.Drawing.Size(206, 24);
            this.menuItemSaveAs.Text = "Save DICOM file As";
            // 
            // menuItemSaveAsILE
            // 
            this.menuItemSaveAsILE.Name = "menuItemSaveAsILE";
            this.menuItemSaveAsILE.Size = new System.Drawing.Size(236, 24);
            this.menuItemSaveAsILE.Text = "Implicit VR Little Endian";
            this.menuItemSaveAsILE.Click += new System.EventHandler(this.menuItemSaveAsILE_Click);
            // 
            // menuItemSaveAsELE
            // 
            this.menuItemSaveAsELE.Name = "menuItemSaveAsELE";
            this.menuItemSaveAsELE.Size = new System.Drawing.Size(236, 24);
            this.menuItemSaveAsELE.Text = "Explicit VR Little Endian";
            this.menuItemSaveAsELE.Click += new System.EventHandler(this.menuItemSaveAsELE_Click);
            // 
            // menuItemSaveAsEBE
            // 
            this.menuItemSaveAsEBE.Name = "menuItemSaveAsEBE";
            this.menuItemSaveAsEBE.Size = new System.Drawing.Size(236, 24);
            this.menuItemSaveAsEBE.Text = "Explicit VR Big Endian";
            this.menuItemSaveAsEBE.Click += new System.EventHandler(this.menuItemSaveAsEBE_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(206, 24);
            this.menuItemExit.Text = "&Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(53, 24);
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(215, 24);
            this.menuItemAbout.Text = "About DICOM Editor";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // backgroundWorkerEditor
            // 
            this.backgroundWorkerEditor.WorkerSupportsCancellation = true;
            this.backgroundWorkerEditor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerEditor_DoWork);
            this.backgroundWorkerEditor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerEditor_RunWorkerCompleted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fileListBox);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.driveListBox);
            this.splitContainer1.Panel1.Controls.Add(this.dirListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridAttributes);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBoxLog);
            this.splitContainer1.Panel2.Controls.Add(this.panelFMIDisplay);
            this.splitContainer1.Panel2.Controls.Add(this.panelFMI);
            this.splitContainer1.Size = new System.Drawing.Size(1110, 774);
            this.splitContainer1.SplitterDistance = 254;
            this.splitContainer1.TabIndex = 13;
            // 
            // fileListBox
            // 
            this.fileListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.HorizontalScrollbar = true;
            this.fileListBox.Location = new System.Drawing.Point(0, 335);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Pattern = "*.dcm;*.";
            this.fileListBox.Size = new System.Drawing.Size(248, 180);
            this.fileListBox.TabIndex = 12;
            this.fileListBox.SelectedIndexChanged += new System.EventHandler(this.fileListBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.privateAttributeMappingBox);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonExport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 574);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 200);
            this.panel1.TabIndex = 13;
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(55, 113);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(125, 30);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Save DICOM File";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Enabled = false;
            this.buttonExport.Location = new System.Drawing.Point(55, 53);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(125, 27);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Export to text file";
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // driveListBox
            // 
            this.driveListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.driveListBox.FormattingEnabled = true;
            this.driveListBox.Location = new System.Drawing.Point(4, 305);
            this.driveListBox.Name = "driveListBox";
            this.driveListBox.Size = new System.Drawing.Size(247, 23);
            this.driveListBox.TabIndex = 11;
            this.driveListBox.SelectedIndexChanged += new System.EventHandler(this.driveListBox_SelectedIndexChanged);
            // 
            // dirListBox
            // 
            this.dirListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dirListBox.FormattingEnabled = true;
            this.dirListBox.IntegralHeight = false;
            this.dirListBox.Location = new System.Drawing.Point(4, 3);
            this.dirListBox.Name = "dirListBox";
            this.dirListBox.Size = new System.Drawing.Size(247, 295);
            this.dirListBox.TabIndex = 10;
            this.dirListBox.SelectedIndexChanged += new System.EventHandler(this.dirListBox_SelectedIndexChanged);
            this.dirListBox.DoubleClick += new System.EventHandler(this.dirListBox_DoubleClick);
            // 
            // dataGridAttributes
            // 
            this.dataGridAttributes.ContextMenuStrip = this.contextMenuDataGrid;
            this.dataGridAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridAttributes.Location = new System.Drawing.Point(0, 247);
            this.dataGridAttributes.Name = "dataGridAttributes";
            this.dataGridAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridAttributes.Size = new System.Drawing.Size(852, 394);
            this.dataGridAttributes.TabIndex = 13;
            this.dataGridAttributes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridAttributes_MouseDown);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 641);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(852, 133);
            this.richTextBoxLog.TabIndex = 15;
            this.richTextBoxLog.Text = "";
            // 
            // panelFMIDisplay
            // 
            this.panelFMIDisplay.Controls.Add(this.dataGridFMI);
            this.panelFMIDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFMIDisplay.Location = new System.Drawing.Point(0, 40);
            this.panelFMIDisplay.Name = "panelFMIDisplay";
            this.panelFMIDisplay.Size = new System.Drawing.Size(852, 207);
            this.panelFMIDisplay.TabIndex = 16;
            this.panelFMIDisplay.Visible = false;
            // 
            // dataGridFMI
            // 
            this.dataGridFMI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridFMI.Location = new System.Drawing.Point(0, 0);
            this.dataGridFMI.Name = "dataGridFMI";
            this.dataGridFMI.Size = new System.Drawing.Size(852, 207);
            this.dataGridFMI.TabIndex = 16;
            // 
            // panelFMI
            // 
            this.panelFMI.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelFMI.Controls.Add(this.checkBoxEditFMI);
            this.panelFMI.Controls.Add(this.checkBoxFMI);
            this.panelFMI.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFMI.Location = new System.Drawing.Point(0, 0);
            this.panelFMI.Name = "panelFMI";
            this.panelFMI.Size = new System.Drawing.Size(852, 40);
            this.panelFMI.TabIndex = 12;
            // 
            // checkBoxEditFMI
            // 
            this.checkBoxEditFMI.AutoSize = true;
            this.checkBoxEditFMI.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxEditFMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEditFMI.Location = new System.Drawing.Point(407, 8);
            this.checkBoxEditFMI.Name = "checkBoxEditFMI";
            this.checkBoxEditFMI.Size = new System.Drawing.Size(226, 23);
            this.checkBoxEditFMI.TabIndex = 1;
            this.checkBoxEditFMI.Text = "Edit File Meta Information";
            this.checkBoxEditFMI.UseVisualStyleBackColor = true;
            this.checkBoxEditFMI.Visible = false;
            this.checkBoxEditFMI.CheckedChanged += new System.EventHandler(this.checkBoxEditFMI_CheckedChanged);
            // 
            // checkBoxFMI
            // 
            this.checkBoxFMI.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxFMI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFMI.Location = new System.Drawing.Point(10, 9);
            this.checkBoxFMI.Name = "checkBoxFMI";
            this.checkBoxFMI.Size = new System.Drawing.Size(326, 19);
            this.checkBoxFMI.TabIndex = 0;
            this.checkBoxFMI.Text = "File Meta Information(Media Header)";
            this.checkBoxFMI.CheckedChanged += new System.EventHandler(this.checkBoxFMI_CheckedChanged);
            // 
            // privateAttributeMappingBox
            // 
            this.privateAttributeMappingBox.AutoSize = true;
            this.privateAttributeMappingBox.Checked = true;
            this.privateAttributeMappingBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.privateAttributeMappingBox.Location = new System.Drawing.Point(4, 4);
            this.privateAttributeMappingBox.Name = "privateAttributeMappingBox";
            this.privateAttributeMappingBox.Size = new System.Drawing.Size(216, 21);
            this.privateAttributeMappingBox.TabIndex = 7;
            this.privateAttributeMappingBox.Text = "Use private attribute mapping";
            this.privateAttributeMappingBox.UseVisualStyleBackColor = true;
            // 
            // DICOMEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1110, 802);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mainMenuDCMEditor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuDCMEditor;
            this.MinimumSize = new System.Drawing.Size(1128, 849);
            this.Name = "DICOMEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DICOM Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DCMEditor_Closing);
            this.contextMenuDataGrid.ResumeLayout(false);
            this.mainMenuDCMEditor.ResumeLayout(false);
            this.mainMenuDCMEditor.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAttributes)).EndInit();
            this.panelFMIDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFMI)).EndInit();
            this.panelFMI.ResumeLayout(false);
            this.panelFMI.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
        {
            try
            {
                DvtkApplicationLayer.DefinitionFilesChecker.CheckVersion("1.0.0", "2.0.0");
            }
            catch (Exception exception)
            {
                Exception customException = new Exception("PLEASE INSTALL DVTK DEFINITION FILES FROM www.dvtk.org website", exception);
                CustomExceptionHandler.ShowThreadExceptionDialog(customException);
                return;
            }
#if !DEBUG
			try
			{
#endif

            // Checks the version of both the application and the DVTk library.
            // If one or both are a non-official or Alpha version, a warning message box is displayed.
            DvtkApplicationLayer.VersionChecker.CheckVersion();

			// This code is used for getting DICOM file or Directory
			// from entry point
			if (args.Length == 1)
			{
				dcmFileOrDirToBeOpened = args[0];
            }
            Application.Run(new DICOMEditor());
#if !DEBUG
			}
			catch(Exception exception)
			{
				CustomExceptionHandler.ShowThreadExceptionDialog(exception);
			}
#endif

		}

		#region Message handlers for displaying drive, directory & file list
		private void fileListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(IsDCMFileModified)
			{
                DialogResult theDialogResult = MessageBox.Show("The Media file has unsaved changes.\n\nDo you want to save the changes?", 
					"Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				if (theDialogResult == DialogResult.Yes) 
				{
					SaveDCMFile(sender,null);
				}
				else 
				{
                    // Do nothing, go ahead with reading the selected Media file.
				}
				IsDCMFileModified = false;
			}

			if(fileListBox.Path.EndsWith(@"\"))
			{
				DCMFile = fileListBox.Path + fileListBox.FileName;
			}
			else
			{
				DCMFile = fileListBox.Path + @"\" + fileListBox.FileName;
			}

            selectedRow = -1;

            Cursor.Current = Cursors.WaitCursor;

            //Load the Media file selected by user
			if(!_IsNewDCMFileLoaded)
				LoadDCMFile(DCMFile);

            Cursor.Current = Cursors.Default;
		}

		void LoadDCMFile(string dicomFile)
		{	
			DCMFile = dicomFile;
            string definitionDir = Environment.GetEnvironmentVariable("COMMONPROGRAMFILES") + @"\DVTk\Definition Files\DICOM\";
			try
			{
				// Load the Definition Files
                DirectoryInfo theDefDirectoryInfo = new DirectoryInfo(definitionDir);
				if(theDefDirectoryInfo.Exists)
				{
					FileInfo[] theDefFilesInfo = theDefDirectoryInfo.GetFiles();
					foreach (FileInfo defFile in theDefFilesInfo)
					{
						bool ok = _MainThread.Options.LoadDefinitionFile(defFile.FullName);
						if(!ok)
						{
							string theWarningText = string.Format("The Definition file {0} could not be loaded.\n", defFile.FullName);
							richTextBoxLog.AppendText(theWarningText);
						}
					}
				}				

				//Subscribe the Dvtk activity report event handler for getting all activity logging.
				_MainThread.Options.DvtkScriptSession.ActivityReportEvent += activityReportEventHandler;
				_MainThread.Options.DvtkScriptSession.AddGroupLength = false;
                _MainThread.Options.DvtkScriptSession.UsePrivateAttributeMapping = privateAttributeMappingBox.Checked;

				// Set the Results & Data directory
                string resultsPath = Application.UserAppDataPath + "\\" + "Results";
                if (!Directory.Exists(resultsPath))
                {
                    Directory.CreateDirectory(resultsPath);
                }
                _MainThread.Options.ResultsDirectory = resultsPath;
                _MainThread.Options.DataDirectory = resultsPath;

                // Read the Media file
				DicomFile dcmFile = new DicomFile();
				
				dcmFile.Read(dicomFile, _MainThread);
              // dcmFile.Read(dicomFile);

                // Get the FMI from the selected Media file
				if(_FileMetaInfo == null)
					_FileMetaInfo = new FileMetaInformation();

				_FileMetaInfo = dcmFile.FileMetaInformation;

                string tsStr = "Undefined";
				if(_FileMetaInfo.Exists("0x00020010"))
				{
					// Get the Transfer syntax
					HLI.Attribute tranferSyntaxAttr = _FileMetaInfo["0x00020010"];
					_TransferSyntax = tranferSyntaxAttr.Values[0];
					tsStr = _TransferSyntax;
				}

                menuItemSaveAsELE.Visible = true;
                menuItemSaveAsILE.Visible = true;
                menuItemSaveAsEBE.Visible = true;

                if (_TransferSyntax == "1.2.840.10008.1.2.1")
                    menuItemSaveAsELE.Visible = false;
                else if (_TransferSyntax == "1.2.840.10008.1.2")
                    menuItemSaveAsILE.Visible = false;
                else if (_TransferSyntax == "1.2.840.10008.1.2.2")
                    menuItemSaveAsEBE.Visible = false;
                else
                {
                    menuItemSaveAsELE.Visible = false;
                    menuItemSaveAsILE.Visible = false;
                    menuItemSaveAsEBE.Visible = false;
                }

				// Get the Data set from the selected DICOM file/DICOMDIR
				string sopClassUID = "";
				if(_FileMetaInfo.Exists("0x00020002"))
				{
					// Get the Transfer syntax
					HLI.Attribute sopClassUIDAttr = _FileMetaInfo["0x00020002"];
					sopClassUID = sopClassUIDAttr.Values[0];
				}

				if(_DCMdataset == null)
					_DCMdataset = new HLI.DataSet();
				
				if(sopClassUID == "1.2.840.10008.1.3.10")
				{
					// Read the DICOMDIR dataset
                    _DCMdataset.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(dicomFile);
                    _IsDICOMDIR = true;
                    IsRefFilesReadCompleted = false;

                    backgroundWorkerEditor.RunWorkerAsync();
              
                    string msg = "DICOMDIR read successfully.\n";
                    richTextBoxLog.AppendText(msg);
				}
				else
				{
                    // Read the Media file dataset
					_DCMdataset = dcmFile.DataSet;
					_IsDICOMDIR = false;
				}

				string theInfoText;
				if(_DCMdataset != null)
				{
					_IsDCMFileLoaded = true;
					UpdateAttributeDataGrid();
					UpdateFMIDataGrid();

                    theInfoText = string.Format("Media file {0} read successfully with Transfer Syntax: {1}.\n\n", dicomFile, tsStr);
                    richTextBoxLog.AppendText(theInfoText);
				}
				else
				{
					theInfoText = string.Format("Error in reading Media file {0}\n\n", DCMFile);

					richTextBoxLog.AppendText(theInfoText);
				}				
			}
			catch(Exception exception)
			{
				//Ask user to get more detailed logging
                string theErrorText = string.Format("Media file {0} could not be read:\n{1}\nDo you want to see detail logging?\n", dicomFile, exception.Message);

				DialogResult theDialogResult = MessageBox.Show(theErrorText, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

				if (theDialogResult == DialogResult.Yes) 
				{
					GetDetailedLogging();
				}
				else
				{
                    richTextBoxLog.AppendText("Select another Media file.");
				}

				_IsDCMFileLoaded = false;
				_DCMdataset = null;
				_FileMetaInfo = null;
				UpdateTitleBarText();
                if (datasetBinding != null)
                    datasetBinding.Clear();
			}			
			
			//Reset the variable 
			_IsNewDCMFileLoaded = false;
		}

        private void backgroundWorkerEditor_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadDicomdirRefFiles(DCMFile);
        }

        private void ReadDicomdirRefFiles(string dicomFile)
        {
            FileInfo dicomDir = new FileInfo(dicomFile);
            
            _RefFileDatasets.Clear();
            _RefFileFMIs.Clear();
            _RefFileNames.Clear();

            // Read all reference file datasets
            HLI.Attribute recordSeqAttr = _DCMdataset["0x00041220"];
            int recordCount = recordSeqAttr.ItemCount;

            for (int i = 0; i < recordCount; i++)
            {
                HLI.SequenceItem item = recordSeqAttr.GetItem(i + 1);

                // Read all attributes in a item
                for (int j = 0; j < item.Count; j++)
                {
                    HLI.Attribute recordTypeAttr = item["0x00041430"];
                    if ((recordTypeAttr.Values[0] == "IMAGE") ||
                        (recordTypeAttr.Values[0] == "PRESENTATION"))
                    {
                        HLI.Attribute refFileIDAttr = item["0x00041500"];
                        if (refFileIDAttr != null)
                        {
                            string refFileName = dicomDir.DirectoryName + "\\";
                            if (refFileIDAttr.Values.Count != 0)
                            {
                                for (int k = 0; k < refFileIDAttr.Values.Count; k++)
                                {
                                    refFileName += refFileIDAttr.Values[k];
                                    refFileName += @"\";
                                }
                            }
                            refFileName = refFileName.Remove((refFileName.Length - 1), 1);
                            FileInfo refFileInfo = new FileInfo(refFileName);
                            if (refFileInfo.Exists)
                            {
                                HLI.DataSet refFileDataset = new HLI.DataSet();
                                
                                // Read the ref file
                                DicomFile refFile = new DicomFile();
                                refFile.Read(refFileName, _MainThread);

                                //refFileDataset.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(refFileName);
                                _RefFileDatasets.Add(refFile.DataSet);
                                _RefFileFMIs.Add(refFile.FileMetaInformation);
                                _RefFileNames.Add(refFileName);
                            }
                        }
                    }
                }
            }
        }

        private void backgroundWorkerEditor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            richTextBoxLog.AppendText("Reading of reference files dataset is completed.\n");
            IsRefFilesReadCompleted = true;
        }

		private void GetDetailedLogging()
		{
			ThreadManager threadMgr = new ThreadManager();
			MainThread loggingThread = new MainThread(DCMFile);
			loggingThread.Initialize(threadMgr);

			loggingThread.Options.CopyFrom(_MainThread.Options);
			loggingThread.Options.Identifier = "DICOM Editor";
			loggingThread.Options.DvtkScriptSession.LogLevelFlags = LogLevelFlags.Error | LogLevelFlags.Warning | LogLevelFlags.Info;
			loggingThread.Options.LogThreadStartingAndStoppingInParent = false;
			loggingThread.Options.LogChildThreadsOverview = false;
			loggingThread.Options.StrictValidation = true;
			loggingThread.Options.StartAndStopResultsGatheringEnabled = true;
			loggingThread.Options.ResultsFileNameOnlyWithoutExtension = 
				string.Format("{0:000}", loggingThread.Options.SessionId) +
				"_" + loggingThread.Options.Identifier + "_res";

			string detailXmlFullFileName = loggingThread.Options.DetailResultsFullFileName;
			string summaryXmlFullFileName = loggingThread.Options.SummaryResultsFullFileName;

			//Start the execution
			loggingThread.Start();

			threadMgr.WaitForCompletionThreads();

			//Stop the thread
			if(loggingThread  != null)
			{
				loggingThread.Stop();
				loggingThread = null;
				threadMgr = null;
			}

			//Display the results
			DetailLogging loggingDlg = new DetailLogging();
			loggingDlg.ShowResults(detailXmlFullFileName,summaryXmlFullFileName);
			loggingDlg.ShowDialog();
		}

		private void dirListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            try
			{
				this.fileListBox.Path = this.dirListBox.Path;
			}
			catch(Exception ex)
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
			catch(Exception ex)
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
			catch(Exception ex)
			{
				richTextBoxLog.AppendText("Error:" + ex.Message);
			}		
		}

		public string DCMFile
		{
			get
			{
				return _DCMFileName;
			}
			set
			{
				_DCMFileName = value;
			}
		}

		public bool IsDCMFileModified
		{
			get
			{
				return _IsDCMFileChanged;
			}
			set
			{
				_IsDCMFileChanged = value;
			}
		}

		public bool IsDICOMDIR
		{
			get
			{
				return _IsDICOMDIR;
			}
		}

		public string DCMFileTransferSyntax
		{
			get
			{
				return _TransferSyntax;
			}
		}

		#endregion

		#region Methods for updating DICOM Editor form changes
		/// <summary>
		/// This is the method for updating Dataset grid
		/// </summary>
		void UpdateAttributeDataGrid()
		{
			_AttributesInfoForDataGrid.Clear();

            AddDatasetInfoToDataGrid();

            //datasetBinding.Clear();
            datasetBinding = new BindingSource();
            datasetBinding.DataSource = _AttributesInfoForDataGrid;
            dataGridAttributes.DataSource = datasetBinding;

            //Hide the last HLI attribute object column
            dataGridAttributes.Columns["HLIAttribute"].Visible = false;

            // Actually refresh the data grid.
			dataGridAttributes.Refresh();

            //Select the previous selected row
            if ((selectedRow >= 0) && (selectedRow < dataGridAttributes.RowCount))
            {
                dataGridAttributes.Rows[0].Selected = false;
                dataGridAttributes.Rows[selectedRow].Selected = true;
                dataGridAttributes.FirstDisplayedScrollingRowIndex = selectedRow;
            }

			UpdateTitleBarText();

            buttonSave.Enabled = true;
            menuItemSave.Enabled = true;
            menuItemSaveAs.Enabled = true;
            findSearchAttributeToolStripMenuItem.Visible = true;

			buttonExport.Enabled = true;			
		}

		/// <summary>
		/// This is the method for updating FMI grid
		/// </summary>
		public void UpdateFMIDataGrid()
		{
			_FMIForDataGrid.Clear();

			AddFMIToDataGrid();

            //fmiBinding.Clear();
            fmiBinding = new BindingSource();
            fmiBinding.DataSource = _FMIForDataGrid;
            dataGridFMI.DataSource = fmiBinding;
            if (dataGridFMI.Columns.Contains("HLIAttribute"))
                dataGridFMI.Columns["HLIAttribute"].Visible = false;

			// Actually refresh the data grid.
			dataGridFMI.Refresh();			
		}

		/// <summary>
		/// Update the title bar of the DICOM Editor form
		/// </summary>
		public void UpdateTitleBarText()
		{
			string theNewText = "DICOM Editor Tool - ";

			if (!_IsDCMFileLoaded)
			{
				theNewText+= "<No Media file loaded>";
			}
			else
			{
				if(_IsSavedInNewDCMFile)
				{
					theNewText+= _NewDCMFileName;
					_IsSavedInNewDCMFile = false;
				}
				else
					theNewText+= DCMFile;

				if (IsDCMFileModified)
				{
					theNewText += "*";
				}
			}

			Text = theNewText;
		}
		#endregion

		#region Method for adding FMI & Dataset to the datagrid
		/// <summary>
		/// Helper function for adding dataset to the datagrid
		/// </summary>
		void AddDatasetInfoToDataGrid()
		{			
			for( int i=0; i < _DCMdataset.Count; i++ )
			{
				HLI.Attribute attribute   = _DCMdataset[i];
				string attributesValues = "";
				string attributeName = attribute.Name;
				if(attributeName == "")
					attributeName = "Undefined";
						
				//Check for group length attributes
				if(attribute.ElementNumber == 0x0000)
					_IsAttributeGroupLengthDefined = true;

				if (attribute.VR != VR.SQ)
				{
					if (attribute.Values.Count != 0)
					{
						for( int j=0; j < attribute.Values.Count; j++ )
						{
							attributesValues += attribute.Values[j] + "\\";
						}
						
						if (attributesValues.EndsWith("\\")) 
						{
							// now search for the last "\"...
							int lastLocation = attributesValues.LastIndexOf( "\\" );

							// remove the identified section, if it is a valid region
							if ( lastLocation >= 0 )
								attributesValues =  attributesValues.Substring( 0, lastLocation );
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
					catch(Exception exception)
					{
						string theErrorText;

						theErrorText = string.Format("Media file {0} could not be read:\n{1}\n\n", DCMFile, exception.Message);

						richTextBoxLog.AppendText(theErrorText);
						richTextBoxLog.ScrollToCaret();
						richTextBoxLog.Focus();
					}

					if(sequenceInSq)
					{
						--levelOfSeq;
						sequenceInSq = false;
					}
					continue;
				}

				AttributeProperty theDataGridAttributeInfo = 
					new AttributeProperty(TagString(attribute.GroupNumber,attribute.ElementNumber),
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
			for( int i=0; i < _FileMetaInfo.Count; i++ )
			{
				HLI.Attribute attribute   = _FileMetaInfo[i];
				string attributesValues = "";
				string attributeName = attribute.Name;
				if(attributeName == "")
					attributeName = "Undefined";
								
				if (attribute.Values.Count != 0)
				{
					for( int j=0; j < attribute.Values.Count; j++ )
					{
						attributesValues += attribute.Values[j] + "\\";
					}
					
					if (attributesValues.EndsWith("\\")) 
					{
						// now search for the last "\"...
						int lastLocation = attributesValues.LastIndexOf( "\\" );

						// remove the identified section, if it is a valid region
						if ( lastLocation >= 0 )
							attributesValues =  attributesValues.Substring( 0, lastLocation );
					}
				}
				else
				{
					attributesValues = "";
				}

				AttributeProperty theDataGridAttributeInfo = 
					new AttributeProperty(TagString(attribute.GroupNumber,attribute.ElementNumber),
					attributeName,
					attribute.VR.ToString(),
					attribute.VM.ToString(),
					attributesValues,
					attribute);
				
				_FMIForDataGrid.Add(theDataGridAttributeInfo);
			}

			//Pass the DICOM Editor object for updating FMI grid
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
			
			for(int i=0; i < levelOfSeq; i++)
				displayTagString += ">";
			displayTagString += TagString(attribute.GroupNumber,attribute.ElementNumber);

			// Attribute name 
			string attributeName = attribute.Name;
			if(attributeName == "")
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

			for( int i=0; i < itemCount; i++ )
			{
				string itemString = "";

				for(int j=0; j < levelOfSeq; j++)
					itemString += ">";
				itemString += string.Format(">BEGIN ITEM {0}", i+1);

				AttributeProperty theDataGridItemInfo = 
					new AttributeProperty(itemString,
					"",
					"",
					"",
					"",
					attribute);
				_AttributesInfoForDataGrid.Add(theDataGridItemInfo);

				HLI.SequenceItem item = attribute.GetItem(i+1);
				for( int k=0; k < item.Count; k++ )
				{
					HLI.Attribute seqAttribute   = item[k];
					string attributesValues = "";
					string seqTagString = "";
					string seqAttributeName = seqAttribute.Name;
					if(seqAttributeName == "")
						seqAttributeName = "Undefined";

					if (seqAttribute.VR != VR.SQ)
					{
						for(int m=0; m <= levelOfSeq; m++)
							seqTagString += ">";
						seqTagString += TagString(seqAttribute.GroupNumber,seqAttribute.ElementNumber);

						if (seqAttribute.Values.Count != 0)
						{
							for( int l=0; l < seqAttribute.Values.Count; l++ )
							{
								attributesValues += seqAttribute.Values[l] + "\\";
							}

							if (attributesValues.Trim().EndsWith("\\")) 
							{
								// now search for the last "\"...
								int lastLocation = attributesValues.LastIndexOf( "\\" );

								// remove the identified section, if it is a valid region
								if ( lastLocation >= 0 )
									attributesValues =  attributesValues.Substring( 0, lastLocation );
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
				for(int m=0; m < levelOfSeq; m++)
					itemString += ">";
				itemString += string.Format(">END ITEM {0}", i+1);

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
                    menuItem_CopyAttribute.Visible = false;
					menuItem_DeleteAttribute.Visible = false;
					menuItem_AddNewAttribute.Visible = false;
                    menuItem_PasteItem.Visible = false;
                    menuItem_PasteAttributes.Visible = false;
                    menuItem_InsertAbove.Visible = false;
                    menuItem_InsertBelow.Visible = false;
					menuItem_AddNewAttribute.Text = "Add New Attribute";
					menuItem_DeleteAttribute.Text = "Delete Attribute";                    

					selectedRow = theHitTestInfo.RowIndex;

                    if (_AttributesInfoForDataGrid.Count != 0)
                    {
                        theSelectedAttributeProperty = (AttributeProperty)_AttributesInfoForDataGrid[theHitTestInfo.RowIndex];

                        string tagString = GetHLITagString(theSelectedAttributeProperty.AttributeTag);

                        if (theHitTestInfo.ColumnIndex == 4)
                        {
                            if ((tagString == "0x00041200") || (tagString == "0x00041202") || (tagString == "0x00041400") ||
                                (tagString == "0x00041410") || (tagString == "0x00041420") || (tagString == "0x00041430"))
                            {
                                dataGridAttributes.CurrentCell.ReadOnly = true;
                                dataGridAttributes.CurrentCell.ToolTipText = "This attribute value can't be modified.";
                                menuItem_CopyItem.Visible = false;
                                menuItem_CopyAttribute.Visible = false;
                                menuItem_InsertAbove.Visible = false;
                                menuItem_InsertBelow.Visible = false;
                                menuItem_DeleteAttribute.Visible = false;
                                menuItem_AddNewAttribute.Visible = true;
                                menuItem_PasteAttributes.Visible = false;

                                string msg = "This attribute value can't be modified.\n";

                                richTextBoxLog.AppendText(msg);
                            }
                            else
                            {
                                if (_IsDICOMDIR)
                                {
                                    if (backgroundWorkerEditor.IsBusy)
                                        richTextBoxLog.AppendText("Reading of reference files dataset is in progress in background...\n");
                                }
                            }
                        }

                        //dataGridAttributes.CurrentRow.Selected = true;

                        if (!_IsDICOMDIR)
                        {
                            // Check for BEGIN ITEM & END ITEM rows
                            if ((theSelectedAttributeProperty.AttributeTag.IndexOf("BEGIN") == -1) &&
                                (theSelectedAttributeProperty.AttributeTag.IndexOf("END") == -1))
                            {
                                menuItem_AddNewAttribute.Visible = true;
                                menuItem_DeleteAttribute.Visible = true;
                                menuItem_CopyAttribute.Visible = true;
                                
                                if (copyAttributes != null)
                                    menuItem_PasteAttributes.Visible = true;
                                else
                                    menuItem_PasteAttributes.Visible = false;
                                
                                menuItem_CopyItem.Visible = false;
                                menuItem_InsertAbove.Visible = false;
                                menuItem_InsertBelow.Visible = false;
                            }
                            else
                            {
                                menuItem_AddNewAttribute.Visible = true;
                                menuItem_CopyItem.Visible = true;
                                menuItem_CopyAttribute.Visible = false;
                                menuItem_PasteAttributes.Visible = false;
                                
                                if (copySequenceItemBuffer != null)
                                {
                                    menuItem_InsertAbove.Visible = true;
                                    menuItem_InsertBelow.Visible = true;
                                }
                                menuItem_DeleteAttribute.Visible = true;
                                menuItem_DeleteAttribute.Text = "Delete Sequence Item";
                            }

                            //Check for Sequence attribute
                            if (theSelectedAttributeProperty.AttributeVR == "SQ")
                            {
                                menuItem_AddNewAttribute.Visible = true;
                                menuItem_AddNewAttribute.Text = "Add Sequence Item";
                                if (copySequenceItemBuffer != null)
                                    menuItem_PasteItem.Visible = true;
                                else
                                    menuItem_PasteItem.Visible = false;
                            }

                            //Check for attributes in a sequence item
                            if (theSelectedAttributeProperty.AttributeTag.StartsWith(">") &&
                              (theSelectedAttributeProperty.AttributeTag.IndexOf("BEGIN") == -1))
                            {
                                menuItem_AddNewAttribute.Visible = false;
                            }
                        }
                        
                        if (dataGridAttributes.SelectedRows.Count > 1)
                        {
                            menuItem_AddNewAttribute.Visible = false;
                            menuItem_DeleteAttribute.Visible = true;
                            menuItem_CopyItem.Visible = false;
                            menuItem_CopyAttribute.Visible = true;
                            
                            if (copyAttributes != null)
                                menuItem_PasteAttributes.Visible = true;
                            else
                                menuItem_PasteAttributes.Visible = false;

                            menuItem_DeleteAttribute.Text = "Delete Selected Attributes";
                        }
                    }

					break;
				}
			}		
		}
        
		#endregion

        #region Message handler for handling Media file saving functionality

        public void SaveDCMFile(object sender, System.EventArgs e)
		{
			buttonSave_Click(sender, e);
		}

		private void buttonSave_Click(object sender, System.EventArgs e)
		{
			if(DCMFile == "")
			{
                richTextBoxLog.AppendText("There is no Media file to save, please select Media file.\n");
				return;
			}

			try
			{
				SaveFileDialog saveAsDlg = new SaveFileDialog();
				saveAsDlg.Filter = "DCM files (*.dcm)|*.dcm|All files (*.*)|*.*";
				FileInfo dcmFileInfo = new FileInfo(DCMFile);
				saveAsDlg.InitialDirectory = dcmFileInfo.DirectoryName;
				saveAsDlg.FileName = dcmFileInfo.Name;
                saveAsDlg.Title = "Save Media file...";
				if (saveAsDlg.ShowDialog () == DialogResult.OK)
				{
                    richTextBoxLog.AppendText("Saving the modified Media file.\n");

					_NewDCMFileName = saveAsDlg.FileName.Trim();

					if(DCMFile != _NewDCMFileName)
						_IsSavedInNewDCMFile = true;
                    
                    string theLogText;
                    if (!_IsDICOMDIR)
                    {
                        DicomFile dcmFile = new DicomFile();

                        // Add FMI as it is to Media file
                        dcmFile.FileMetaInformation = _FileMetaInfo;
                        dcmFile.FileMetaInformation.TransferSyntax = _TransferSyntax;

                        // Add modified dataset to Media file
                        dcmFile.DataSet = _DCMdataset;

                        //Set attribute group length to false
                        dcmFile.AddGroupLength = false;

                        dcmFile.Write(_NewDCMFileName);
                    }
                    else
                    {
                        //DicomDir dicomdir = new DicomDir();

                        //// Add FMI as it is to Media file
                        //dicomdir.FileMetaInformation = _FileMetaInfo;
                        //dicomdir.FileMetaInformation.TransferSyntax = _TransferSyntax;

                        //// Add modified dataset to Media file
                        //dicomdir.DataSet = _DCMdataset;

                        //dicomdir.Write(_NewDCMFileName);
                    }
				
					theLogText = string.Format("Media file {0} saved successfully.\n\n", _NewDCMFileName);

					richTextBoxLog.AppendText(theLogText);
					richTextBoxLog.ScrollToCaret();
					richTextBoxLog.Focus();

					//Cleanup the FMI & Dataset
					IsDCMFileModified = false;
					_IsAttributeGroupLengthDefined = false;
					_DCMdataset = null;
					_FileMetaInfo = null;

					if(_IsSavedInNewDCMFile)
					{
						FileInfo newDcmFileInfo = new FileInfo(_NewDCMFileName);

						_IsNewDCMFileLoaded = true;
						this.dirListBox.Path = newDcmFileInfo.DirectoryName;
						this.fileListBox.Path = this.dirListBox.Path;
						this.fileListBox.SelectedItem = newDcmFileInfo.Name;

                        this.dirListBox.Refresh();
						this.fileListBox.Refresh();

                        //Load the new Media file
						LoadDCMFile(_NewDCMFileName);
					}
					else
					{
						_IsNewDCMFileLoaded = false;
						this.dirListBox.Path = dcmFileInfo.DirectoryName;
						this.fileListBox.Path = this.dirListBox.Path;
						this.fileListBox.SelectedItem = dcmFileInfo.Name;

                        //Load the new Media file
						LoadDCMFile(DCMFile);
					}

					UpdateTitleBarText();
				}
			}
			catch(Exception exception)
			{
				string theErrorText;

				theErrorText = string.Format("Media file {0} could not be saved:\n{1}\n\n", _NewDCMFileName, exception.Message);

				richTextBoxLog.AppendText(theErrorText);
				richTextBoxLog.ScrollToCaret();
				richTextBoxLog.Focus();

				IsDCMFileModified = false;
				UpdateTitleBarText();
			}						
		}

        private void menuItemSaveAsILE_Click(object sender, EventArgs e)
        {
            SaveFileAsDefinedTS("1.2.840.10008.1.2");
        }

        private void menuItemSaveAsELE_Click(object sender, EventArgs e)
        {
            SaveFileAsDefinedTS("1.2.840.10008.1.2.1");
        }

        private void menuItemSaveAsEBE_Click(object sender, EventArgs e)
        {
            SaveFileAsDefinedTS("1.2.840.10008.1.2.2");
        }

        void SaveFileAsDefinedTS(string transferSyntax)
        {
            if (DCMFile == "")
            {
                richTextBoxLog.AppendText("There is no Media file to save, please select Media file.\n");
                return;
            }

            try
            {
                SaveFileDialog saveAsDlg = new SaveFileDialog();
                saveAsDlg.Filter = "DCM files (*.dcm) |*.dcm | All files (*.*)|*.*";
                FileInfo dcmFileInfo = new FileInfo(DCMFile);
                saveAsDlg.InitialDirectory = dcmFileInfo.DirectoryName;
                saveAsDlg.FileName = dcmFileInfo.Name;
                saveAsDlg.Title = "Save DICOM file...";
                if (saveAsDlg.ShowDialog() == DialogResult.OK)
                {
                    richTextBoxLog.AppendText("Saving the modified Media file.\n");

                    _NewDCMFileName = saveAsDlg.FileName.Trim();

                    if (DCMFile != _NewDCMFileName)
                        _IsSavedInNewDCMFile = true;

                    DvtkData.Media.DicomFile dicomMediaFile = new DvtkData.Media.DicomFile();

                    // set up the file head
                    DvtkData.Media.FileHead fileHead = new DvtkData.Media.FileHead();

                    // add the Transfer Syntax UID
                    DvtkData.Dul.TransferSyntax tSyntax = new DvtkData.Dul.TransferSyntax(transferSyntax);
                    fileHead.TransferSyntax = tSyntax;

                    // set up the file meta information
                    DvtkData.Media.FileMetaInformation fileMetaInformation = new DvtkData.Media.FileMetaInformation();

                    // add the FMI version
                    fileMetaInformation.AddAttribute(DvtkData.Dimse.Tag.FILE_META_INFORMATION_VERSION.GroupNumber,
                        DvtkData.Dimse.Tag.FILE_META_INFORMATION_VERSION.ElementNumber, VR.OB, 1, 2);

                    // add the SOP Class UID
                    fileMetaInformation.AddAttribute(DvtkData.Dimse.Tag.MEDIA_STORAGE_SOP_CLASS_UID.GroupNumber,
                        DvtkData.Dimse.Tag.MEDIA_STORAGE_SOP_CLASS_UID.ElementNumber, VR.UI, _FileMetaInfo.MediaStorageSOPClassUID);

                    // add the SOP Instance UID
                    fileMetaInformation.AddAttribute(DvtkData.Dimse.Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.GroupNumber,
                        DvtkData.Dimse.Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.ElementNumber, VR.UI, _FileMetaInfo.MediaStorageSOPInstanceUID);

                    // add the Transfer Syntax UID
                    fileMetaInformation.AddAttribute(DvtkData.Dimse.Tag.TRANSFER_SYNTAX_UID.GroupNumber,
                        DvtkData.Dimse.Tag.TRANSFER_SYNTAX_UID.ElementNumber, VR.UI, transferSyntax);

                    // add the Implemenation Class UID
                    string implClassUID = "";
				    if(_FileMetaInfo.Exists("0x00020012"))
				    {
					    // Get the Transfer syntax
					    HLI.Attribute implClassUIDAttr = _FileMetaInfo["0x00020012"];
					    implClassUID = implClassUIDAttr.Values[0];
				    }

                    fileMetaInformation.AddAttribute(DvtkData.Dimse.Tag.IMPLEMENTATION_CLASS_UID.GroupNumber,
                        DvtkData.Dimse.Tag.IMPLEMENTATION_CLASS_UID.ElementNumber, VR.UI, implClassUID);

                    // add the Implementation Version Name
                    string implClassVersion = "";
				    if(_FileMetaInfo.Exists("0x00020013"))
				    {
					    // Get the Transfer syntax
					    HLI.Attribute implClassVersionAttr = _FileMetaInfo["0x00020013"];
					    implClassVersion = implClassVersionAttr.Values[0];
				    }

                    fileMetaInformation.AddAttribute(DvtkData.Dimse.Tag.IMPLEMENTATION_VERSION_NAME.GroupNumber,
                        DvtkData.Dimse.Tag.IMPLEMENTATION_VERSION_NAME.ElementNumber, VR.SH, implClassVersion);

                    // set up the dicomMediaFile contents
                    dicomMediaFile.FileHead = fileHead;
                    dicomMediaFile.FileMetaInformation = fileMetaInformation;
                    dicomMediaFile.DataSet = _DCMdataset.DvtkDataDataSet;

                    // write the dicomMediaFile to file
                    Dvtk.DvtkDataHelper.WriteDataSetToFile(dicomMediaFile, _NewDCMFileName);

                    string theLogText = string.Format("Dataset {0} saved successfully with transfer syntax: {1}.\n\n", _NewDCMFileName, transferSyntax);

                    richTextBoxLog.AppendText(theLogText);
                    richTextBoxLog.ScrollToCaret();
                    richTextBoxLog.Focus();

                    //Cleanup the FMI & Dataset
                    IsDCMFileModified = false;
                    _IsAttributeGroupLengthDefined = false;
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

                        //Load the new Media file
                        LoadDCMFile(_NewDCMFileName);
                    }
                    else
                    {
                        _IsNewDCMFileLoaded = false;
                        this.dirListBox.Path = dcmFileInfo.DirectoryName;
                        this.fileListBox.Path = this.dirListBox.Path;
                        this.fileListBox.SelectedItem = dcmFileInfo.Name;

                        //Load the new Media file
                        LoadDCMFile(DCMFile);
                    }

                    UpdateTitleBarText();
                }
            }
            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("Media file {0} could not be saved:\n{1}\n\n", _NewDCMFileName, exception.Message);

                richTextBoxLog.AppendText(theErrorText);
                richTextBoxLog.ScrollToCaret();
                richTextBoxLog.Focus();

                IsDCMFileModified = false;
                UpdateTitleBarText();
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
			if(datagridTagString.StartsWith(">"))
			{
				int level = 0;
				for(int i=0; i<datagridTagString.Length; i++ )
				{
					if(datagridTagString[i] == '>')
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
		/// <param name="display"></param>
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
		public static DvtkData.Dimse.VR GetVR(string vrString)
		{
			if (vrString == "AE")   return VR.AE;
			if (vrString == "AS")   return VR.AS;
			if (vrString == "AT")   return VR.AT;
			if (vrString == "CS")   return VR.CS;
			if (vrString == "DA")   return VR.DA;
			if (vrString == "DS")   return VR.DS;
			if (vrString == "DT")   return VR.DT;
			if (vrString == "FL")	return VR.FL;
			if (vrString == "FD")	return VR.FD;
			if (vrString == "IS")   return VR.IS;
			if (vrString == "LO")   return VR.LO;
			if (vrString == "LT")   return VR.LT;
			if (vrString == "OB")   return VR.OB;
			if (vrString == "OF")   return VR.OF;
			if (vrString == "OW")   return VR.OW;
            if (vrString == "OL")   return VR.OL;
            if (vrString == "OD")   return VR.OD;
			if (vrString == "PN")   return VR.PN;
			if (vrString == "SH")   return VR.SH;
			if (vrString == "SL")   return VR.SL;
			if (vrString == "SQ")   return VR.SQ;
			if (vrString == "SS")   return VR.SS;
            if (vrString == "ST")   return VR.ST;
            if (vrString == "SV")   return VR.SV;
			if (vrString == "TM")   return VR.TM;
			if (vrString == "UI")   return VR.UI;
			if (vrString == "UL")   return VR.UL;
			if (vrString == "UN")   return VR.UN;
			if (vrString == "US")   return VR.US;
			if (vrString == "UT")   return VR.UT;
            if (vrString == "UC")   return VR.UC;
            if (vrString == "UR")   return VR.UR;
            if (vrString == "UV")   return VR.UV;
			// Unknown DicomValueType
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Activity logging handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="theArgs"></param>
		private void OnActivityReportEvent(object sender, Dvtk.Events.ActivityReportEventArgs e)
		{
			string message;
			switch(e.ReportLevel)
			{
				case Dvtk.Events.ReportLevel.Error:
					message = "Error:" + e.Message + '\n';
					break;

				case Dvtk.Events.ReportLevel.Warning:
					message = "Warning:" + e.Message + '\n';
					break;

				default:
					message = e.Message + '\n';
					break;
			}

			if (richTextBoxLog.InvokeRequired) 
			{
				richTextBoxLog.Invoke(activityLoggingDelegate, new object[] {message});
			}
			else 
			{
				richTextBoxLog.AppendText(e.Message + '\n');
			}		
		}

		private void AppendTextToActivityLogging_ThreadSafe(string theText) 
		{
			richTextBoxLog.AppendText(theText);

			// ScrollToCaret will work, regardless whether the RichTextControl has the focus or not,
			// because the HideSelection property is set to false.
			richTextBoxLog.ScrollToCaret();
			richTextBoxLog.Focus();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="itemStr"></param>
		/// <returns></returns>
		public int GetItemNr(string itemStr)
		{
			// now search for the item nr
			string itemNrStr = "";
			int itemNr = 0;
			itemNrStr =  itemStr.Substring( itemStr.LastIndexOf(" "));
			itemNr = int.Parse(itemNrStr.Trim());
			return itemNr;
		}

        private void FindData(string tag, string value,string name, int searchMode)
        {
            //Check for attribute in the grid and select it
            int index = 0;
            int lastIndex = 0;
            bool matchFound = false;
            
            //Clear previous selection if any
            foreach (DataGridViewRow row in dataGridAttributes.SelectedRows)
                row.Selected = false;

            //if ((tag == "") && (value == ""))
            //{
            //    MessageBox.Show("Please provide appropriate value as search criteria.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            foreach (AttributeProperty attributeProperty in _AttributesInfoForDataGrid)
            {
                if ((attributeProperty.AttributeTag.IndexOf("BEGIN") != -1) ||
                    (attributeProperty.AttributeTag.IndexOf("END") != -1) ||
                    (attributeProperty.AttributeVR == "SQ"))
                {
                    if (attributeProperty.AttributeTag.IndexOf(tag)!=-1)
                    {
                        dataGridAttributes.Rows[index].Selected = true;
                        lastIndex = index;
                        matchFound = true;
                        
                    }
                    
                    index++;
                    continue;
                }

                int i = attributeProperty.AttributeTag.IndexOf(tag);
                if (searchMode == 1)
                {
                    if ((i != -1) &&
                        (string.Compare(attributeProperty.AttributeValue, value, true) == 0))
                    {
                        dataGridAttributes.Rows[index].Selected = true;
                        lastIndex = index;
                        matchFound = true;
                    }
                }
                else if (searchMode == 0)
                {
                    if (i != -1)
                    {
                        dataGridAttributes.Rows[index].Selected = true;
                        lastIndex = index;
                        matchFound = true;
                    }
                }

                else if (searchMode == 3)
                {
                    if ((string.Compare(attributeProperty.AttributeName , name, true) == 0))
                    {
                        dataGridAttributes.Rows[index].Selected = true;
                        lastIndex = index;
                        matchFound = true;
                    }
                }
                else
                {
                    if ((string.Compare(attributeProperty.AttributeValue, value, true) == 0))
                    {
                        dataGridAttributes.Rows[index].Selected = true;
                        lastIndex = index;
                        matchFound = true;
                    }
                }
                index++;
            }
            dataGridAttributes.FirstDisplayedScrollingRowIndex = lastIndex;

            if(!matchFound)
                MessageBox.Show("No match found for provided search criteria.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
		
		#endregion

		#region DICOM Editor form handlers
		private void DCMEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (IsDCMFileModified) 
			{
                DialogResult theDialogResult = MessageBox.Show("The Media file has unsaved changes.\n\nDo you want to save the changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				if (theDialogResult == DialogResult.Yes) 
				{
					SaveDCMFile(sender,null);
				}
				else if (theDialogResult == DialogResult.No) 
				{
					// Do nothing, go ahead with closing the application.
				}
				else 
				{
					e.Cancel = true;
				}
			}

            if (backgroundWorkerEditor != null)
            {
                if (backgroundWorkerEditor.IsBusy)
                    backgroundWorkerEditor.CancelAsync();
                backgroundWorkerEditor.Dispose();
            }

			//Remove all temporary files generated during tool execution
			if(!e.Cancel)
			{
				ArrayList theFilesToRemove = new ArrayList();
				DirectoryInfo theDirectoryInfo = new DirectoryInfo (Application.StartupPath + "\\" + "Results");
				FileInfo[] thePixFilesInfo;
				FileInfo[] theIdxFilesInfo;

				if (theDirectoryInfo.Exists)
				{
					thePixFilesInfo = theDirectoryInfo.GetFiles("*.pix");
					theIdxFilesInfo = theDirectoryInfo.GetFiles("*.idx");

					foreach (FileInfo theFileInfo in thePixFilesInfo)
					{
						string thePixFileName = theFileInfo.Name;

						theFilesToRemove.Add(thePixFileName);
					}
					foreach (FileInfo theFileInfo in theIdxFilesInfo)
					{
						string theIdxFileName = theFileInfo.Name;

						theFilesToRemove.Add(theIdxFileName);
					}
				}

				//Delete all pix & idx files
				foreach(string theFileName in theFilesToRemove)
				{
					string theFullFileName = System.IO.Path.Combine((Application.StartupPath + "\\" + "Results"), theFileName);

					if (File.Exists(theFullFileName))
					{
						try
						{
							File.Delete(theFullFileName);
						}
						catch(Exception exception)
						{
							string theErrorText;

							theErrorText = string.Format("Could not be delete the {0} temporary file.\n due to exception: {1}\n\n", theFullFileName, exception.Message);

							richTextBoxLog.AppendText(theErrorText);
						}
					}				
				}
			}

			//Unsubscribe the activity logging handler
			_MainThread.Options.DvtkScriptSession.ActivityReportEvent -= activityReportEventHandler;
			Dvtk.Setup.Terminate();
		}

		protected override void OnResize(EventArgs e)
		{
			if (Size.Height < 100)
			{
				if (_PreviousBounds != Rectangle.Empty)
				{
					Bounds = _PreviousBounds;
				}
			}

			base.OnResize(e);

			_PreviousBounds = Bounds;
		}
		#endregion

		#region Message handlers for menu items
		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			AboutForm  about = new AboutForm("DICOM Editor Tool");
			about.ShowDialog ();
		}

		private void menuItemSave_Click(object sender, System.EventArgs e)
		{
			SaveDCMFile(sender,e);
		}
		#endregion

        #region Methods for adding/deleting new attributes & items to Media file
        /// <summary>
        /// Add a new attribute to the Media file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuItem_AddNewAttribute_Click(object sender, System.EventArgs e)
		{
			AddAttribute newAttribute = new AddAttribute ();

			if(menuItem_AddNewAttribute.Text == "Add Sequence Item")
			{
				HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;
				attribute.AddItem(new HLI.SequenceItem());
				IsDCMFileModified = true;
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
					if(theSelectedAttributeProperty.AttributeTag.IndexOf("BEGIN") != -1)
					{
						HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;

						int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);

						int itemCount = attribute.ItemCount;
						if(itemCount > 0)
						{
							//Get the attribute tag string
							string attributeTagString = GetHLITagString(newAttribute.NewAttributeTag);

							HLI.SequenceItem item = attribute.GetItem(itemNr);

							for(int i=0; i < item.Count; i++)
							{
								HLI.Attribute itemAttribute = item[i];
								string attrStr = GetHLITagString(TagString(itemAttribute.GroupNumber,itemAttribute.ElementNumber));
								if(attrStr == attributeTagString)
								{
									string msg = "This attribute already exists in Sequence Item.\n";
									MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									return;
								}
							}

							if(newAttribute.NewAttributeVR == "SQ")
							{
								//AddNestedSeqAttribute(newAttribute,index,nestingOfSeq, tagSequence);
								item.Set(attributeTagString,VR.SQ);
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
								else
								{
									values = new string[] {};
								}

								// Get the existing attribute in Dataset
								item.Set(attributeTagString,GetVR(newAttribute.NewAttributeVR),values);									
							}
						}
					}
					else
					{
						//Check for Sequence attribute
						if(theSelectedAttributeProperty.AttributeVR != "SQ")
						{
							//Check for duplication of new attribute
							foreach (AttributeProperty attributeProperty in _AttributesInfoForDataGrid)
							{
								if(attributeProperty.AttributeTag == newAttribute.NewAttributeTag)
								{
                                    string msg = "This attribute already exists in Media file.\n";
									MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									return;
								}
							}
				
							//Handle Sequence attribute seperately
							if(newAttribute.NewAttributeVR == "SQ")
							{
								//Add attribute to the dataset and an empty sequence item
								string tagString = GetHLITagString(newAttribute.NewAttributeTag);
								_DCMdataset.Set(tagString,VR.SQ);
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
								else
								{
									values = new string[] {};
								}

								//Add attribute to the dataset
								string tagString = GetHLITagString(newAttribute.NewAttributeTag);
								_DCMdataset.Set(tagString,GetVR(newAttribute.NewAttributeVR),values);
							}
						}						
					}
				}
				catch(Exception exception)
				{
					string theErrorText;

					theErrorText = string.Format("The new attribute {0} could not be added due to exception:\n {1}\n\n", 
						newAttribute.NewAttributeTag, exception.Message);

					richTextBoxLog.AppendText(theErrorText);
					richTextBoxLog.ScrollToCaret();
					richTextBoxLog.Focus();

					IsDCMFileModified = false;
					UpdateTitleBarText();
					return;
				}

				IsDCMFileModified = true;
				UpdateAttributeDataGrid();

				//Check for new attribute in the grid and select it
                int index = 0;
                foreach (AttributeProperty attributeProperty in _AttributesInfoForDataGrid)
                {
                    if (attributeProperty.AttributeTag == newAttribute.NewAttributeTag)
                    {
                        dataGridAttributes.Rows[selectedRow].Selected = false;
                        dataGridAttributes.Rows[index].Selected = true;
                        dataGridAttributes.FirstDisplayedScrollingRowIndex = index;
                        break;
                    }
                    index++;
                }
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
				if(menuItem_DeleteAttribute.Text == "Delete Sequence Item")
				{
					int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);
					int itemCount = attribute.ItemCount;
					if(itemNr <= itemCount)
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
			catch(Exception exception)
			{
				string theErrorText;

				theErrorText = string.Format("The selected attribute/item could not be deleted.\n\n", 
					exception.Message);

				richTextBoxLog.AppendText(theErrorText);
				richTextBoxLog.ScrollToCaret();
				richTextBoxLog.Focus();

				IsDCMFileModified = false;
				UpdateTitleBarText();
				return;
			}

			IsDCMFileModified = true;
			UpdateAttributeDataGrid();			
		}

		private void menuItem_CopyItem_Click(object sender, System.EventArgs e)
		{
			//try
			{
				HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;

				int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);
				
				int itemCount = attribute.ItemCount;
				if(itemCount > 0)
				{
					// Get the first sequence Item and add the clone of 
					// seq item to seq attribute
                    //HLI.SequenceItem item = attribute.GetItem(itemNr);
                    //attribute.AddItem(item);	
                    copySequenceItemBuffer = attribute.GetItem(itemNr);
				}									
			}
            //catch(Exception exception)
            //{
            //    string theErrorText;

            //    theErrorText = string.Format("The selected sequence item could not be copied to \nsequence attribute due to exception:\n {0}\n\n", 
            //        exception.Message);

            //    richTextBoxLog.AppendText(theErrorText);
            //    richTextBoxLog.ScrollToCaret();
            //    richTextBoxLog.Focus();

            //    IsDCMFileModified = false;
            //    UpdateTitleBarText();
            //    return;
            //}
			
            //IsDCMFileModified = true;
            //UpdateAttributeDataGrid();		
		}

		private void buttonExport_Click(object sender, System.EventArgs e)
		{
			StringBuilder s = new StringBuilder(500);
			foreach (AttributeProperty fmiAttributeProperty in _FMIForDataGrid)
			{
				string attributeValue = fmiAttributeProperty.AttributeValue;
				if(attributeValue == "")
					attributeValue = "\t";
				s.Append("(" + GetHLITagString(fmiAttributeProperty.AttributeTag) + "," + "\t" +
					fmiAttributeProperty.AttributeVR + "," + "\t" +
					attributeValue + ")" + "\t" + 
					"#" + fmiAttributeProperty.AttributeName + "\r\n");
			}

			foreach (AttributeProperty attributeProperty in _AttributesInfoForDataGrid)
			{
				bool isSqAttrItem = attributeProperty.AttributeTag.StartsWith(">");
				if((attributeProperty.AttributeVR != "SQ") && (!isSqAttrItem))
				{
					string attributeValue = attributeProperty.AttributeValue;
					if(attributeValue == "")
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
					if(attributeProperty.AttributeTag.StartsWith(">"))
					{
						for(int i=0; i<attributeProperty.AttributeTag.Length; i++ )
						{
							if(attributeProperty.AttributeTag[i] == '>')
							{
								level++;
							}
						}

						for(int j=0; j<level; j++ )
						{
							attributeTag += "  ";
						}
						
						if((attributeProperty.AttributeTag.IndexOf("BEGIN") != -1) ||
							(attributeProperty.AttributeTag.IndexOf("END") != -1))
						{
							attributeTag += attributeProperty.AttributeTag;
							s.Append(attributeTag + "\r\n");
						}
						else
						{
							for(int k=0; k<level; k++)
							{
								attributeTag += ">";
							}
							attributeTag += GetHLITagString(attributeProperty.AttributeTag);
							string attributeValue = attributeProperty.AttributeValue;
							if(attributeValue == "")
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
			if (saveFileDlg.ShowDialog () == DialogResult.OK)
			{
				txtFileName = saveFileDlg.FileName;
				StreamWriter textFile = File.CreateText(txtFileName);
				if(textFile != null)
				{
					textFile.Write(s);
					textFile.Flush();
				}

				textFile.Close();
				textFile = null;

                string msg = string.Format("The Media file is exported to {0} successfully.\n\n", txtFileName);
				richTextBoxLog.AppendText(msg);
				richTextBoxLog.ScrollToCaret();
				richTextBoxLog.Focus();
			}
		}

		private void checkBoxFMI_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxFMI.Checked)
			{
				panelFMIDisplay.Visible = true;
                checkBoxEditFMI.Visible = true;
			}
			else
			{
				panelFMIDisplay.Visible = false;
                checkBoxEditFMI.Visible = false;
			}
        }

        private void findSearchAttributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindAttribute dlg = new FindAttribute();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int mode = 0;
                if (dlg.IsAnd)
                    mode = 1;
                else if (dlg.IsAttributeValue)
                    mode = 2;
                else if (dlg.IsAttributeName)
                    mode = 3;
                else 
                    mode = 0;

                FindData(dlg.AttributeTag,dlg.AttributeValue,dlg.AttributeName,mode);
            }
        }

        private void menuItem_PasteItem_Click(object sender, EventArgs e)
        {
            try
            {
                HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;
                attribute.AddItem(copySequenceItemBuffer);
            }

            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("The selected sequence item could not be copied to \nsequence attribute due to exception:\n {0}\n\n",
                    exception.Message);

                richTextBoxLog.AppendText(theErrorText);
                richTextBoxLog.ScrollToCaret();
                richTextBoxLog.Focus();

                IsDCMFileModified = false;
                UpdateTitleBarText();
                return;
            }

            IsDCMFileModified = true;
            UpdateAttributeDataGrid();
        }

        private void menuItem_InsertAbove_Click(object sender, EventArgs e)
        {
            try
            {
                HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;

                int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);
                attribute.InsertItem(itemNr, copySequenceItemBuffer);
            }

            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("The selected sequence item could not be copied to \nsequence attribute due to exception:\n {0}\n\n",
                    exception.Message);

                richTextBoxLog.AppendText(theErrorText);
                richTextBoxLog.ScrollToCaret();
                richTextBoxLog.Focus();

                IsDCMFileModified = false;
                UpdateTitleBarText();
                return;
            }
            IsDCMFileModified = true;
            UpdateAttributeDataGrid();
            
        }

        private void menuItem_InsertBelow_Click(object sender, EventArgs e)
        {
           try
            {
                HLI.Attribute attribute = theSelectedAttributeProperty.HLIAttribute;

                int itemNr = GetItemNr(theSelectedAttributeProperty.AttributeTag);
                attribute.InsertItem(itemNr+1, copySequenceItemBuffer);
            }
            catch (Exception exception)
            {
                string theErrorText;

                theErrorText = string.Format("The selected sequence item could not be copied to \nsequence attribute due to exception:\n {0}\n\n",
                    exception.Message);

                richTextBoxLog.AppendText(theErrorText);
                richTextBoxLog.ScrollToCaret();
                richTextBoxLog.Focus();

                IsDCMFileModified = false;
                UpdateTitleBarText();
                return;
            }

            IsDCMFileModified = true;
            UpdateAttributeDataGrid();
            
        }

        private void checkBoxEditFMI_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEditFMI.Checked == true)
            {
                MessageBox.Show("Modifying the File Meta Information can cause several inconsistencies!", "Warning", MessageBoxButtons.OK , MessageBoxIcon.Warning);
                dataGridFMI.Columns[4].ReadOnly = false;

            }
            else
            {
                dataGridFMI.Columns[4].ReadOnly = true;

            }
        }

        private void menuItem_CopyAttribute_Click(object sender, EventArgs e)
        {
            //HLI.AttributeCollection attributeCollection = null;
            copyAttributes = new DvtkHighLevelInterface.Dicom.Other.AttributeCollection();
            foreach (DataGridViewRow row in dataGridAttributes.SelectedRows)
            {
                AttributeProperty attributeProperty = (AttributeProperty)_AttributesInfoForDataGrid[row.Index];

                copyAttributes.Add(attributeProperty.HLIAttribute);
            }
        }

        private void menuItem_PasteAttributes_Click(object sender, EventArgs e)
        {

            bool duplicateAttributesExist = false;

            foreach (HLI.Attribute attribute in copyAttributes)
            {
                if (_DCMdataset.Exists(attribute.TagSequenceString))
                {
                    duplicateAttributesExist = true;
                }
                else
                    _DCMdataset.Add(attribute);
            }
            if (duplicateAttributesExist)
                MessageBox.Show("Some of the copied attributes are already present in the DICOM file.Duplicate attributes have not been added",
                                 "Message",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
            UpdateAttributeDataGrid();
        }                                		
	}
	#endregion

	/// <summary>
	/// Summary description for Main Thread.
	/// </summary>
	public class MainThread : DicomThread
	{
		string dcmFile;
		public MainThread()
		{}

		public MainThread(string dcmfile)
		{
			dcmFile = dcmfile;
		}

		protected override void Execute()
		{
            // Read the Media file
			try
			{
				DicomFile dcmFileObj = new DicomFile();				
				dcmFileObj.Read(dcmFile, this);	
			}
			catch(Exception)
			{
				WriteHtmlInformation("<br />");
				WriteInformation("Error in reading Media file.");
				WriteHtmlInformation("<br />");
			}
		}
	}

	/// <summary>
	/// This class is used to represent each row in the datagrid. Each row
	/// in the datagrid refer to a HLI attribute object & HLI attribute tag sequence.
	/// </summary>
	public class AttributeProperty
	{
		private static DICOMEditor editorObj = null;
		public AttributeProperty(DICOMEditor editor)
		{
			editorObj = editor;
		}

		public AttributeProperty( string displayTag,
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
		
		public string AttributeTag
		{
			get { return this[attributeTag]; }
			set { this[attributeTag] = value; }
		}

		public string AttributeName
		{
			get { return this[attributeName]; }
			set { this[attributeName] = value; }
		}

		public string AttributeVR
		{
			get { return this[attributeVR]; }
			set 
			{ 
				this[attributeVR] = value;
			}
		}

		public string AttributeVM
		{
			get { return this[attributeVM]; }
			set { this[attributeVM] = value; }
		}

		public string AttributeValue
		{
			get { return this[attributeValue];}
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
                                DICOMEditor._FileMetaInfo.Set("0x00020002", VR.UI, values[0]);
                            }
                            else
                            {
                                // Set the modified SOP Instance UID attribute
                                DICOMEditor._FileMetaInfo.Set("0x00020003", VR.UI, values[0]);
                            }

                            if (editorObj != null)
                                editorObj.UpdateFMIDataGrid();
                        }

                        if (editorObj != null)
                        {
                            editorObj.IsDCMFileModified = true;
                            editorObj.UpdateTitleBarText();
                        }
                    }

                    if ((editorObj.IsDICOMDIR) && (editorObj.IsRefFilesReadCompleted))
                    {
                        // Deal dicomdir reference files modifications seperately
                        if ((DICOMEditor._RefFileDatasets.Count != 0))
                        {
                            int i = 0;
                            foreach (HLI.DataSet dataset in DICOMEditor._RefFileDatasets)
                            {
                                if (dataset[this.AttributeTag].Exists)
                                {
                                    dataset.Set(this.AttributeTag, DICOMEditor.GetVR(this.AttributeVR), value);

                                    //Save ref file
                                    DicomFile dicomFile = new DicomFile();

                                    // Add FMI as it is to Media file
                                    dicomFile.FileMetaInformation = (FileMetaInformation)(DICOMEditor._RefFileFMIs[i]);

                                    // Add modified dataset to Media file
                                    dicomFile.DataSet = dataset;

                                    string refFileName = (string)(DICOMEditor._RefFileNames[i]);
                                    dicomFile.Write(refFileName);
                                }
                                i++;
                            }
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
                            DICOMEditor._FileMetaInfo.Set("0x00020002", VR.UI);
                        }
                        else
                        {
                            // Set the modified SOP Instance UID attribute
                            DICOMEditor._FileMetaInfo.Set("0x00020003", VR.UI);
                        }

                        if (editorObj != null)
                            editorObj.UpdateFMIDataGrid();
                    }

                    if (editorObj != null)
                    {
                        
                        editorObj.IsDCMFileModified = true;
                        editorObj.UpdateTitleBarText();
                    }


                    if ((editorObj.IsDICOMDIR) && (editorObj.IsRefFilesReadCompleted))
                    {
                        // Deal dicomdir reference files modifications seperately
                        if ((DICOMEditor._RefFileDatasets.Count != 0))
                        {
                            int i = 0;
                            foreach (HLI.DataSet dataset in DICOMEditor._RefFileDatasets)
                            {
                                if (dataset[this.AttributeTag].Exists)
                                {
                                    dataset.Set(this.AttributeTag, DICOMEditor.GetVR(this.AttributeVR));

                                    //Save ref file
                                    DicomFile dicomFile = new DicomFile();

                                    // Add FMI as it is to Media file
                                    dicomFile.FileMetaInformation = (FileMetaInformation)(DICOMEditor._RefFileFMIs[i]);

                                    // Add modified dataset to Media file
                                    dicomFile.DataSet = dataset;

                                    string refFileName = (string)(DICOMEditor._RefFileNames[i]);
                                    dicomFile.Write(refFileName);
                                }
                                i++;
                            }
                        }
                    }
                }
            }
         }


		public HLI.Attribute HLIAttribute
		{
			get { return this.hliAttributeObj; }			
		}		

		// HLI attribute object referred by each Datagrid row
		private HLI.Attribute hliAttributeObj;
		
		//Used by indexer property
		private string [] properties = new string[5];
		private const int   attributeTag = 0;
		private const int   attributeName = 1;
		private const int   attributeVR = 2;
		private const int   attributeVM = 3;
		private const int   attributeValue = 4;
	}	
}
