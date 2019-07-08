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
using System.Windows.Forms;
using System.Data;
using System.Media;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using System.Xml.XPath;
using System.Xml.Xsl;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtIheAcquisitionModalityWrapper;
using Dvtk.IheActors.Dicom;
using Dvtk.IheActors.Bases;
using DvtkApplicationLayer;
using DvtkApplicationLayer.UserInterfaces;

namespace ModalityEmulator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class ModalityEmulator : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenuEmulator;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemHelp;
        private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.MenuItem menuItemConfigEmulator;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.TabControl tabControlEmulator;
        private System.Windows.Forms.TabPage tabPageControl;
		private System.Windows.Forms.TabPage tabPageWLM;
		private System.Windows.Forms.TabPage tabPageMPPSCreate;
		private System.Windows.Forms.TabPage tabPageMPPSSet;
        private System.Windows.Forms.TabPage tabPageImageStorage;
        private System.Windows.Forms.TabPage tabPageResults;
		private System.Windows.Forms.TabPage tabPageActivityLogging;
		private Dvtk.IheActors.UserInterfaces.UserControlActivityLogging userControlActivityLoggingEmulator;
		private DCMEditor dcmEditorWLM;
		private DCMEditor dcmEditorMPPSCreate;
		private DCMEditor dcmEditorMPPSSet;
		private DCMEditor dcmEditorStorage;
        private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkWebBrowserEmulator;
		private System.Windows.Forms.ToolBar toolBarEmulator;
		private System.Windows.Forms.ImageList imageListEmulator;
		private System.Windows.Forms.ToolBarButton toolBarButtonLeft;
		private System.Windows.Forms.ToolBarButton toolBarButtonUp;
		private System.Windows.Forms.ToolBarButton toolBarButtonright;
		private System.Windows.Forms.ToolBarButton toolBarButtonResults;
		private System.Windows.Forms.ToolBarButton toolBarButtonLog;
		private System.Windows.Forms.ToolBarButton toolBarButtonStop;
		private System.Windows.Forms.TabPage tabPageDummyPatient;
        private DCMEditor dcmEditorDummyPatient;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolBarButton toolBarButtonConfigSys;
        private System.Windows.Forms.ToolBarButton toolBarButtonSep;
        private System.Windows.Forms.ToolBarButton toolBarButtonConfigEmulator;
        private TabPage tabPageMPPSDiscontinued;
        private DCMEditor dcmEditorDiscontinued;

		AcquisitionModalityWrapper wrapper = null;
		string wlmRqDataDirectory = "";
		string wlmRspDataDirectory = "";
		string mppsDataDirectory = "";
		string storageDataDirectory = "";
		bool isCreated = false;
		bool isInitialized = false;
		bool isTerminated = false;
        bool isUIUpdateReqd = true;
        bool isSyncCommit = true;
        bool progressCompleted = false;
		DicomQueryItem selectedMWLItem = null;
        private ArrayList selectedTS = new ArrayList();

        string echoResultFile = "";
        string lastRISIPAddress;
        string lastRISAETitle;
        string lastMPPSIPAddress;
        string lastMPPSAETitle;
        string lastRetrieveAETitle;
        string lastPACSIPAddress;
        string lastPACSAETitle;
        string lastPACSCommitIPAddress;
        string lastPACSCommitAETitle;
        string lastPort;
        string lastRISPort;
        string lastPACSPort;
        string lastPACSCommitPort;
        string lastMPPSPort;

        // Needed to be able to differentiate between controls changed by the user
        // and controls changed by an UpdateConfig method.
        private int updateCount = 0;
        private TabPage configureTab;
        private MaskedTextBox textBoxPort;
        private MaskedTextBox textBoxTimeOut;
        private Label label13;
        private ComboBox emulatorIPAddress;
        private Label label14;
        private TextBox textBoxSystemName;
        private Label label15;
        private TextBox textBoxImplName;
        private Label label16;
        private TextBox textBoxAETitle;
        private TextBox textBoxImplUID;
        private Label label17;
        private Label label18;
        private Label label19;
        private GroupBox groupBox2;
        private GroupBox groupBox5;
        private MaskedTextBox textBoxRISPort;
        private TextBox textBoxRISAETitle;
        private MaskedTextBox textBoxRISIPAddress;
        private Label label26;
        private Label label27;
        private Label label28;
        private GroupBox groupBox4;
        private MaskedTextBox textBoxMPPSPort;
        private TextBox textBoxMPPSAETitle;
        private MaskedTextBox textBoxMPPSIPAddress;
        private Label label23;
        private Label label24;
        private Label label25;
        private GroupBox groupBox3;
        private MaskedTextBox textBoxPACSPort;
        private TextBox textBoxPACSAETitle;
        private MaskedTextBox textBoxPACSIPAddress;
        private Label label20;
        private Label label21;
        private Label label22;
        private TextBox retrieveAETitleBox;
        private Label label29;
        private PictureBox emulatorHeader;
        private Button selectExamButton;
        private Label label33;
        private Label label34;
        private Label label35;
        private Button finalizeExamButton;
        private Button performScanButton;
        private TextBox activityLogBox;
        private MenuItem menuItemConfig;
        private Button testConnectivityButton;
        private TextBox connectivityLog;
        
		string userConfigFilePath = Application.StartupPath + @"\" + "UserConfig.txt";

		public ModalityEmulator()
		{
			//
			// Required for Windows Form Designer support
			//
            InitializeComponent();
            isSyncCommit = true;
            retrieveAETitleBox.Text = Properties.Settings.Default.RetrieveAETitle;

			//Hide the tab pages
			tabControlEmulator.Controls.Remove(configureTab);
			tabControlEmulator.Controls.Remove(tabPageWLM);
			tabControlEmulator.Controls.Remove(tabPageMPPSCreate);
			tabControlEmulator.Controls.Remove(tabPageMPPSSet);
            tabControlEmulator.Controls.Remove(tabPageMPPSDiscontinued);
			tabControlEmulator.Controls.Remove(tabPageImageStorage);
			tabControlEmulator.Controls.Remove(tabPageResults);
			tabControlEmulator.Controls.Remove(tabPageActivityLogging);
			tabControlEmulator.Controls.Remove(tabPageDummyPatient);

            string definitionDir = Environment.GetEnvironmentVariable("COMMONPROGRAMFILES") + @"\DVTk\Definition Files\DICOM\";

			wlmRqDataDirectory = Application.StartupPath + @"\data\worklist\WLM RQ\";
			wlmRspDataDirectory = Application.StartupPath + @"\data\worklist\WLM RSP\";
			mppsDataDirectory = Application.StartupPath + @"\data\mpps\";
			storageDataDirectory = Application.StartupPath + @"\data\acquisitionModality\default\";

			// Load definition files
            dcmEditorWLM.DefFile = definitionDir + "Modality Worklist Information Model - Find.def";
            dcmEditorMPPSCreate.DefFile = definitionDir + "Modality Performed Procedure Step.def";
            dcmEditorMPPSSet.DefFile = definitionDir + "Modality Performed Procedure Step.def";
            dcmEditorDiscontinued.DefFile = definitionDir + "Modality Performed Procedure Step.def";
            dcmEditorStorage.DefFile = definitionDir + "Secondary Capture Image Storage.def";
            dcmEditorDummyPatient.DefFile = definitionDir + "Modality Worklist Information Model - Find.def";

            //dcmEditorWLM.DefFile = Application.StartupPath + @"\definitions\Allotherattributes.def";
            //dcmEditorMPPSCreate.DefFile = Application.StartupPath + @"\definitions\Allotherattributes.def";
            //dcmEditorMPPSSet.DefFile = Application.StartupPath + @"\definitions\Allotherattributes.def";
            //dcmEditorDiscontinued.DefFile = Application.StartupPath + @"\definitions\Allotherattributes.def";
            //dcmEditorStorage.DefFile = Application.StartupPath + @"\definitions\Allotherattributes.def";
            //dcmEditorDummyPatient.DefFile = Application.StartupPath + @"\definitions\Allotherattributes.def";

			// Set data directory for temp pix files
			dcmEditorWLM.DCMFileDataDirectory = Application.StartupPath + @"\results\";
			dcmEditorMPPSCreate.DCMFileDataDirectory = Application.StartupPath + @"\results\";
			dcmEditorMPPSSet.DCMFileDataDirectory = Application.StartupPath + @"\results\";
            dcmEditorDiscontinued.DCMFileDataDirectory = Application.StartupPath + @"\results\";
			dcmEditorStorage.DCMFileDataDirectory = Application.StartupPath + @"\results\";
			dcmEditorDummyPatient.DCMFileDataDirectory = Application.StartupPath + @"\results\";

			// Display the DCM file
			dcmEditorDummyPatient.DCMFile = wlmRspDataDirectory + "DummyPatient.dcm";

			// Set dcm file from user config file
			StreamReader reader = new StreamReader(userConfigFilePath);
			if(reader != null)
			{
				string line = reader.ReadLine().Trim();
				if(line != "")
				{
					if(Path.IsPathRooted(line))
						dcmEditorWLM.DCMFile = line;
					else
						dcmEditorWLM.DCMFile = Application.StartupPath + line;
				}
				else
					dcmEditorWLM.DCMFile = wlmRqDataDirectory + "worklistquery2.dcm";

				line = reader.ReadLine().Trim();
				if(line != "")
				{
					if(Path.IsPathRooted(line))
						dcmEditorMPPSCreate.DCMFile = line;
					else
						dcmEditorMPPSCreate.DCMFile = Application.StartupPath + line;
				}
				else
					dcmEditorMPPSCreate.DCMFile = mppsDataDirectory + "mpps-inprogress1.dcm";

				line = reader.ReadLine().Trim();
				if(line != "")
				{
					if(Path.IsPathRooted(line))
						dcmEditorStorage.DCMFile = line;
					else
						dcmEditorStorage.DCMFile = Application.StartupPath + line;
				}
				else
					dcmEditorStorage.DCMFile = storageDataDirectory + "1I00001.dcm";

				line = reader.ReadLine().Trim();
				if(line != "")
				{
					if(Path.IsPathRooted(line))
						dcmEditorMPPSSet.DCMFile = line;
					else
						dcmEditorMPPSSet.DCMFile = Application.StartupPath + line;
				}
				else
					dcmEditorMPPSSet.DCMFile = mppsDataDirectory + "mpps-completed1.dcm";

                line = reader.ReadLine().Trim();
                if (line != "")
                {
                    if (Path.IsPathRooted(line))
                        dcmEditorDiscontinued.DCMFile = line;
                    else
                        dcmEditorDiscontinued.DCMFile = Application.StartupPath + line;
                }
                else
                    dcmEditorDiscontinued.DCMFile = mppsDataDirectory + "mpps-discontinued1.dcm";

				reader.Close();
			}

			this.dvtkWebBrowserEmulator.XmlStyleSheetFullFileName = Application.StartupPath + "\\DVT_RESULTS.xslt";
            this.dvtkWebBrowserEmulator.BackwardFormwardEnabledStateChangeEvent += new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(dvtkWebBrowserEmulator_BackwardFormwardEnabledStateChangeEvent);

            selectedTS.Add("1.2.840.10008.1.2");

            //Get the IP Address of the current machine
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            if (addr != null)
            {
                foreach (IPAddress address in addr)
                {
                    emulatorIPAddress.Items.Add(address.ToString());
                }
                emulatorIPAddress.SelectedItem = emulatorIPAddress.Items[0];
            }

			//Create instance of integration profile
            CreateIntegrationProfile();
			isCreated = true;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModalityEmulator));
            this.mainMenuEmulator = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemConfig = new System.Windows.Forms.MenuItem();
            this.menuItemConfigEmulator = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.tabControlEmulator = new System.Windows.Forms.TabControl();
            this.tabPageControl = new System.Windows.Forms.TabPage();
            this.activityLogBox = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.finalizeExamButton = new System.Windows.Forms.Button();
            this.performScanButton = new System.Windows.Forms.Button();
            this.selectExamButton = new System.Windows.Forms.Button();
            this.emulatorHeader = new System.Windows.Forms.PictureBox();
            this.configureTab = new System.Windows.Forms.TabPage();
            this.connectivityLog = new System.Windows.Forms.TextBox();
            this.testConnectivityButton = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxRISPort = new System.Windows.Forms.MaskedTextBox();
            this.textBoxRISAETitle = new System.Windows.Forms.TextBox();
            this.textBoxRISIPAddress = new System.Windows.Forms.MaskedTextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBoxMPPSPort = new System.Windows.Forms.MaskedTextBox();
            this.textBoxMPPSAETitle = new System.Windows.Forms.TextBox();
            this.textBoxMPPSIPAddress = new System.Windows.Forms.MaskedTextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxPACSPort = new System.Windows.Forms.MaskedTextBox();
            this.textBoxPACSAETitle = new System.Windows.Forms.TextBox();
            this.textBoxPACSIPAddress = new System.Windows.Forms.MaskedTextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.retrieveAETitleBox = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.textBoxSystemName = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.MaskedTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.textBoxTimeOut = new System.Windows.Forms.MaskedTextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.emulatorIPAddress = new System.Windows.Forms.ComboBox();
            this.textBoxImplUID = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxAETitle = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxImplName = new System.Windows.Forms.TextBox();
            this.tabPageWLM = new System.Windows.Forms.TabPage();
            this.dcmEditorWLM = new DvtkApplicationLayer.UserInterfaces.DCMEditor();
            this.tabPageMPPSCreate = new System.Windows.Forms.TabPage();
            this.dcmEditorMPPSCreate = new DvtkApplicationLayer.UserInterfaces.DCMEditor();
            this.tabPageMPPSSet = new System.Windows.Forms.TabPage();
            this.dcmEditorMPPSSet = new DvtkApplicationLayer.UserInterfaces.DCMEditor();
            this.tabPageMPPSDiscontinued = new System.Windows.Forms.TabPage();
            this.dcmEditorDiscontinued = new DvtkApplicationLayer.UserInterfaces.DCMEditor();
            this.tabPageImageStorage = new System.Windows.Forms.TabPage();
            this.dcmEditorStorage = new DvtkApplicationLayer.UserInterfaces.DCMEditor();
            this.tabPageDummyPatient = new System.Windows.Forms.TabPage();
            this.dcmEditorDummyPatient = new DvtkApplicationLayer.UserInterfaces.DCMEditor();
            this.tabPageActivityLogging = new System.Windows.Forms.TabPage();
            this.userControlActivityLoggingEmulator = new Dvtk.IheActors.UserInterfaces.UserControlActivityLogging();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.dvtkWebBrowserEmulator = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.toolBarEmulator = new System.Windows.Forms.ToolBar();
            this.toolBarButtonConfigEmulator = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonConfigSys = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonResults = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonLog = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSep = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonLeft = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonUp = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonright = new System.Windows.Forms.ToolBarButton();
            this.imageListEmulator = new System.Windows.Forms.ImageList(this.components);
            this.tabControlEmulator.SuspendLayout();
            this.tabPageControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.emulatorHeader)).BeginInit();
            this.configureTab.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageWLM.SuspendLayout();
            this.tabPageMPPSCreate.SuspendLayout();
            this.tabPageMPPSSet.SuspendLayout();
            this.tabPageMPPSDiscontinued.SuspendLayout();
            this.tabPageImageStorage.SuspendLayout();
            this.tabPageDummyPatient.SuspendLayout();
            this.tabPageActivityLogging.SuspendLayout();
            this.tabPageResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuEmulator
            // 
            this.mainMenuEmulator.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemHelp});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemConfig,
            this.menuItemConfigEmulator,
            this.menuItemExit});
            this.menuItemFile.Text = "&File";
            // 
            // menuItemConfig
            // 
            this.menuItemConfig.Index = 0;
            this.menuItemConfig.Text = "Configure Network";
            this.menuItemConfig.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItemConfigEmulator
            // 
            this.menuItemConfigEmulator.Index = 1;
            this.menuItemConfigEmulator.Text = "Configure Emulator";
            this.menuItemConfigEmulator.Click += new System.EventHandler(this.menuItemConfigEmulator_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 2;
            this.menuItemExit.Text = "&Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 1;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "About Modality Emulator";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // tabControlEmulator
            // 
            this.tabControlEmulator.Controls.Add(this.tabPageControl);
            this.tabControlEmulator.Controls.Add(this.configureTab);
            this.tabControlEmulator.Controls.Add(this.tabPageWLM);
            this.tabControlEmulator.Controls.Add(this.tabPageMPPSCreate);
            this.tabControlEmulator.Controls.Add(this.tabPageMPPSSet);
            this.tabControlEmulator.Controls.Add(this.tabPageMPPSDiscontinued);
            this.tabControlEmulator.Controls.Add(this.tabPageImageStorage);
            this.tabControlEmulator.Controls.Add(this.tabPageDummyPatient);
            this.tabControlEmulator.Controls.Add(this.tabPageActivityLogging);
            this.tabControlEmulator.Controls.Add(this.tabPageResults);
            this.tabControlEmulator.Location = new System.Drawing.Point(0, 34);
            this.tabControlEmulator.Name = "tabControlEmulator";
            this.tabControlEmulator.SelectedIndex = 0;
            this.tabControlEmulator.Size = new System.Drawing.Size(954, 620);
            this.tabControlEmulator.TabIndex = 1;
            // 
            // tabPageControl
            // 
            this.tabPageControl.BackColor = System.Drawing.Color.Black;
            this.tabPageControl.Controls.Add(this.activityLogBox);
            this.tabPageControl.Controls.Add(this.label33);
            this.tabPageControl.Controls.Add(this.label34);
            this.tabPageControl.Controls.Add(this.label35);
            this.tabPageControl.Controls.Add(this.finalizeExamButton);
            this.tabPageControl.Controls.Add(this.performScanButton);
            this.tabPageControl.Controls.Add(this.selectExamButton);
            this.tabPageControl.Controls.Add(this.emulatorHeader);
            this.tabPageControl.Location = new System.Drawing.Point(4, 22);
            this.tabPageControl.Name = "tabPageControl";
            this.tabPageControl.Size = new System.Drawing.Size(946, 594);
            this.tabPageControl.TabIndex = 0;
            this.tabPageControl.Text = "Control";
            this.tabPageControl.UseVisualStyleBackColor = true;
            // 
            // activityLogBox
            // 
            this.activityLogBox.Location = new System.Drawing.Point(8, 549);
            this.activityLogBox.Multiline = true;
            this.activityLogBox.Name = "activityLogBox";
            this.activityLogBox.ReadOnly = true;
            this.activityLogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.activityLogBox.Size = new System.Drawing.Size(930, 40);
            this.activityLogBox.TabIndex = 23;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(244)))));
            this.label33.Location = new System.Drawing.Point(732, 514);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(110, 20);
            this.label33.TabIndex = 22;
            this.label33.Text = "DICOM MPPS";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(244)))));
            this.label34.Location = new System.Drawing.Point(400, 514);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(145, 20);
            this.label34.TabIndex = 21;
            this.label34.Text = "DICOM STORAGE";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(244)))));
            this.label35.Location = new System.Drawing.Point(80, 514);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(155, 20);
            this.label35.TabIndex = 20;
            this.label35.Text = "DICOM MWL & MPPS";
            // 
            // finalizeExamButton
            // 
            this.finalizeExamButton.BackColor = System.Drawing.Color.Black;
            this.finalizeExamButton.BackgroundImage = global::ModalityEmulator.Properties.Resources.discontinue_exam;
            this.finalizeExamButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.finalizeExamButton.Enabled = false;
            this.finalizeExamButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.finalizeExamButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(106)))), ((int)(((byte)(146)))));
            this.finalizeExamButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.finalizeExamButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.finalizeExamButton.Location = new System.Drawing.Point(632, 252);
            this.finalizeExamButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.finalizeExamButton.Name = "finalizeExamButton";
            this.finalizeExamButton.Size = new System.Drawing.Size(311, 259);
            this.finalizeExamButton.TabIndex = 19;
            this.finalizeExamButton.UseVisualStyleBackColor = false;
            this.finalizeExamButton.Click += new System.EventHandler(this.finalizeExamButton_Click);
            // 
            // performScanButton
            // 
            this.performScanButton.BackColor = System.Drawing.Color.Black;
            this.performScanButton.BackgroundImage = global::ModalityEmulator.Properties.Resources.perform_scan;
            this.performScanButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.performScanButton.Enabled = false;
            this.performScanButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.performScanButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(106)))), ((int)(((byte)(146)))));
            this.performScanButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.performScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.performScanButton.Location = new System.Drawing.Point(317, 252);
            this.performScanButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.performScanButton.Name = "performScanButton";
            this.performScanButton.Size = new System.Drawing.Size(311, 259);
            this.performScanButton.TabIndex = 18;
            this.performScanButton.UseVisualStyleBackColor = false;
            this.performScanButton.Click += new System.EventHandler(this.buttonStoreImage_Click);
            // 
            // selectExamButton
            // 
            this.selectExamButton.BackColor = System.Drawing.Color.Black;
            this.selectExamButton.BackgroundImage = global::ModalityEmulator.Properties.Resources.select_exam;
            this.selectExamButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.selectExamButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.selectExamButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(106)))), ((int)(((byte)(146)))));
            this.selectExamButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.selectExamButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectExamButton.Location = new System.Drawing.Point(2, 252);
            this.selectExamButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.selectExamButton.Name = "selectExamButton";
            this.selectExamButton.Size = new System.Drawing.Size(311, 259);
            this.selectExamButton.TabIndex = 17;
            this.selectExamButton.UseVisualStyleBackColor = false;
            this.selectExamButton.Click += new System.EventHandler(this.buttonReqWL_Click);
            // 
            // emulatorHeader
            // 
            this.emulatorHeader.Image = global::ModalityEmulator.Properties.Resources.header;
            this.emulatorHeader.Location = new System.Drawing.Point(2, 10);
            this.emulatorHeader.Name = "emulatorHeader";
            this.emulatorHeader.Size = new System.Drawing.Size(942, 234);
            this.emulatorHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.emulatorHeader.TabIndex = 16;
            this.emulatorHeader.TabStop = false;
            // 
            // configureTab
            // 
            this.configureTab.BackColor = System.Drawing.Color.Black;
            this.configureTab.Controls.Add(this.connectivityLog);
            this.configureTab.Controls.Add(this.testConnectivityButton);
            this.configureTab.Controls.Add(this.groupBox5);
            this.configureTab.Controls.Add(this.groupBox4);
            this.configureTab.Controls.Add(this.groupBox3);
            this.configureTab.Controls.Add(this.groupBox2);
            this.configureTab.Location = new System.Drawing.Point(4, 22);
            this.configureTab.Name = "configureTab";
            this.configureTab.Padding = new System.Windows.Forms.Padding(3);
            this.configureTab.Size = new System.Drawing.Size(946, 594);
            this.configureTab.TabIndex = 12;
            this.configureTab.Text = "Configuration";
            this.configureTab.UseVisualStyleBackColor = true;
            // 
            // connectivityLog
            // 
            this.connectivityLog.Location = new System.Drawing.Point(5, 327);
            this.connectivityLog.Multiline = true;
            this.connectivityLog.Name = "connectivityLog";
            this.connectivityLog.ReadOnly = true;
            this.connectivityLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.connectivityLog.Size = new System.Drawing.Size(930, 256);
            this.connectivityLog.TabIndex = 56;
            // 
            // testConnectivityButton
            // 
            this.testConnectivityButton.BackColor = System.Drawing.SystemColors.Control;
            this.testConnectivityButton.Location = new System.Drawing.Point(6, 298);
            this.testConnectivityButton.Name = "testConnectivityButton";
            this.testConnectivityButton.Size = new System.Drawing.Size(136, 23);
            this.testConnectivityButton.TabIndex = 55;
            this.testConnectivityButton.Text = "Test Connectivity";
            this.testConnectivityButton.UseVisualStyleBackColor = false;
            this.testConnectivityButton.Click += new System.EventHandler(this.testConnectivityButton_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(32)))), ((int)(((byte)(96)))));
            this.groupBox5.Controls.Add(this.textBoxRISPort);
            this.groupBox5.Controls.Add(this.textBoxRISAETitle);
            this.groupBox5.Controls.Add(this.textBoxRISIPAddress);
            this.groupBox5.Controls.Add(this.label26);
            this.groupBox5.Controls.Add(this.label27);
            this.groupBox5.Controls.Add(this.label28);
            this.groupBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.groupBox5.Location = new System.Drawing.Point(494, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(441, 101);
            this.groupBox5.TabIndex = 50;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "RIS Configuration";
            // 
            // textBoxRISPort
            // 
            this.textBoxRISPort.BeepOnError = true;
            this.textBoxRISPort.Location = new System.Drawing.Point(273, 45);
            this.textBoxRISPort.Name = "textBoxRISPort";
            this.textBoxRISPort.Size = new System.Drawing.Size(157, 20);
            this.textBoxRISPort.TabIndex = 12;
            this.textBoxRISPort.Text = "105";
            this.textBoxRISPort.TextChanged += new System.EventHandler(this.textBoxRISPort_TextChanged);
            // 
            // textBoxRISAETitle
            // 
            this.textBoxRISAETitle.Location = new System.Drawing.Point(273, 71);
            this.textBoxRISAETitle.Name = "textBoxRISAETitle";
            this.textBoxRISAETitle.Size = new System.Drawing.Size(157, 20);
            this.textBoxRISAETitle.TabIndex = 8;
            this.textBoxRISAETitle.Text = "RIS";
            this.textBoxRISAETitle.TextChanged += new System.EventHandler(this.textBoxRISAETitle_TextChanged);
            // 
            // textBoxRISIPAddress
            // 
            this.textBoxRISIPAddress.Location = new System.Drawing.Point(273, 19);
            this.textBoxRISIPAddress.Name = "textBoxRISIPAddress";
            this.textBoxRISIPAddress.Size = new System.Drawing.Size(157, 20);
            this.textBoxRISIPAddress.TabIndex = 7;
            this.textBoxRISIPAddress.Text = "localhost";
            this.textBoxRISIPAddress.TextChanged += new System.EventHandler(this.textBoxRISIPAddress_TextChanged);
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(6, 48);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(72, 16);
            this.label26.TabIndex = 10;
            this.label26.Text = "Remote Port:";
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(6, 74);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(72, 16);
            this.label27.TabIndex = 11;
            this.label27.Text = "AE Title:";
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(6, 22);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(72, 16);
            this.label28.TabIndex = 9;
            this.label28.Text = "IP Address:";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(32)))), ((int)(((byte)(96)))));
            this.groupBox4.Controls.Add(this.textBoxMPPSPort);
            this.groupBox4.Controls.Add(this.textBoxMPPSAETitle);
            this.groupBox4.Controls.Add(this.textBoxMPPSIPAddress);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.label25);
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.groupBox4.Location = new System.Drawing.Point(494, 113);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(441, 101);
            this.groupBox4.TabIndex = 49;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "MPPS Configuration";
            // 
            // textBoxMPPSPort
            // 
            this.textBoxMPPSPort.BeepOnError = true;
            this.textBoxMPPSPort.Location = new System.Drawing.Point(273, 45);
            this.textBoxMPPSPort.Name = "textBoxMPPSPort";
            this.textBoxMPPSPort.Size = new System.Drawing.Size(157, 20);
            this.textBoxMPPSPort.TabIndex = 12;
            this.textBoxMPPSPort.Text = "106";
            this.textBoxMPPSPort.TextChanged += new System.EventHandler(this.textBoxMPPSPort_TextChanged);
            // 
            // textBoxMPPSAETitle
            // 
            this.textBoxMPPSAETitle.Location = new System.Drawing.Point(273, 71);
            this.textBoxMPPSAETitle.Name = "textBoxMPPSAETitle";
            this.textBoxMPPSAETitle.Size = new System.Drawing.Size(157, 20);
            this.textBoxMPPSAETitle.TabIndex = 8;
            this.textBoxMPPSAETitle.Text = "MPPS";
            this.textBoxMPPSAETitle.TextChanged += new System.EventHandler(this.textBoxMPPSAETitle_TextChanged);
            // 
            // textBoxMPPSIPAddress
            // 
            this.textBoxMPPSIPAddress.Location = new System.Drawing.Point(273, 19);
            this.textBoxMPPSIPAddress.Name = "textBoxMPPSIPAddress";
            this.textBoxMPPSIPAddress.Size = new System.Drawing.Size(157, 20);
            this.textBoxMPPSIPAddress.TabIndex = 7;
            this.textBoxMPPSIPAddress.Text = "localhost";
            this.textBoxMPPSIPAddress.TextChanged += new System.EventHandler(this.textBoxMPPSIPAddress_TextChanged);
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(6, 48);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(72, 16);
            this.label23.TabIndex = 10;
            this.label23.Text = "Remote Port:";
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(6, 74);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(72, 16);
            this.label24.TabIndex = 11;
            this.label24.Text = "AE Title:";
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(6, 22);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(72, 16);
            this.label25.TabIndex = 9;
            this.label25.Text = "IP Address:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(32)))), ((int)(((byte)(96)))));
            this.groupBox3.Controls.Add(this.textBoxPACSPort);
            this.groupBox3.Controls.Add(this.textBoxPACSAETitle);
            this.groupBox3.Controls.Add(this.textBoxPACSIPAddress);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.groupBox3.Location = new System.Drawing.Point(494, 220);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(441, 101);
            this.groupBox3.TabIndex = 48;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PACS Configuration";
            // 
            // textBoxPACSPort
            // 
            this.textBoxPACSPort.BeepOnError = true;
            this.textBoxPACSPort.Location = new System.Drawing.Point(273, 45);
            this.textBoxPACSPort.Name = "textBoxPACSPort";
            this.textBoxPACSPort.Size = new System.Drawing.Size(157, 20);
            this.textBoxPACSPort.TabIndex = 12;
            this.textBoxPACSPort.Text = "107";
            this.textBoxPACSPort.TextChanged += new System.EventHandler(this.textBoxPACSPort_TextChanged);
            // 
            // textBoxPACSAETitle
            // 
            this.textBoxPACSAETitle.Location = new System.Drawing.Point(273, 71);
            this.textBoxPACSAETitle.Name = "textBoxPACSAETitle";
            this.textBoxPACSAETitle.Size = new System.Drawing.Size(157, 20);
            this.textBoxPACSAETitle.TabIndex = 8;
            this.textBoxPACSAETitle.Text = "PACS";
            this.textBoxPACSAETitle.TextChanged += new System.EventHandler(this.textBoxPACSAETitle_TextChanged);
            // 
            // textBoxPACSIPAddress
            // 
            this.textBoxPACSIPAddress.Location = new System.Drawing.Point(273, 19);
            this.textBoxPACSIPAddress.Name = "textBoxPACSIPAddress";
            this.textBoxPACSIPAddress.Size = new System.Drawing.Size(157, 20);
            this.textBoxPACSIPAddress.TabIndex = 7;
            this.textBoxPACSIPAddress.Text = "localhost";
            this.textBoxPACSIPAddress.TextChanged += new System.EventHandler(this.textBoxPACSIPAddress_TextChanged);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(6, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 16);
            this.label20.TabIndex = 10;
            this.label20.Text = "Remote Port:";
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(6, 74);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(72, 16);
            this.label21.TabIndex = 11;
            this.label21.Text = "AE Title:";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(6, 22);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(72, 16);
            this.label22.TabIndex = 9;
            this.label22.Text = "IP Address:";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(32)))), ((int)(((byte)(96)))));
            this.groupBox2.Controls.Add(this.retrieveAETitleBox);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.textBoxSystemName);
            this.groupBox2.Controls.Add(this.textBoxPort);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.textBoxTimeOut);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.emulatorIPAddress);
            this.groupBox2.Controls.Add(this.textBoxImplUID);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.textBoxAETitle);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.textBoxImplName);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.groupBox2.Location = new System.Drawing.Point(8, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(441, 235);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Emulator Configuration";
            // 
            // retrieveAETitleBox
            // 
            this.retrieveAETitleBox.Location = new System.Drawing.Point(272, 202);
            this.retrieveAETitleBox.Name = "retrieveAETitleBox";
            this.retrieveAETitleBox.Size = new System.Drawing.Size(157, 20);
            this.retrieveAETitleBox.TabIndex = 47;
            this.retrieveAETitleBox.Text = "PACS";
            this.retrieveAETitleBox.TextChanged += new System.EventHandler(this.retrieveAETitleBox_TextChanged);
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(5, 205);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(101, 16);
            this.label29.TabIndex = 48;
            this.label29.Text = "Retrieve AE Title:";
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxSystemName.Location = new System.Drawing.Point(273, 19);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.Size = new System.Drawing.Size(157, 20);
            this.textBoxSystemName.TabIndex = 31;
            this.textBoxSystemName.Text = "Modality";
            // 
            // textBoxPort
            // 
            this.textBoxPort.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxPort.BeepOnError = true;
            this.textBoxPort.Location = new System.Drawing.Point(272, 150);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(157, 20);
            this.textBoxPort.TabIndex = 46;
            this.textBoxPort.Text = "104";
            this.textBoxPort.TextChanged += new System.EventHandler(this.textBoxPort_TextChanged);
            // 
            // label19
            // 
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.label19.Location = new System.Drawing.Point(6, 77);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(144, 16);
            this.label19.TabIndex = 37;
            this.label19.Text = "Implementation Class UID:";
            // 
            // textBoxTimeOut
            // 
            this.textBoxTimeOut.BeepOnError = true;
            this.textBoxTimeOut.Location = new System.Drawing.Point(273, 176);
            this.textBoxTimeOut.Mask = "00";
            this.textBoxTimeOut.Name = "textBoxTimeOut";
            this.textBoxTimeOut.Size = new System.Drawing.Size(156, 20);
            this.textBoxTimeOut.TabIndex = 45;
            this.textBoxTimeOut.Text = "1";
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.label18.Location = new System.Drawing.Point(6, 52);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(72, 16);
            this.label18.TabIndex = 36;
            this.label18.Text = "AE Title:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.label13.Location = new System.Drawing.Point(6, 182);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(258, 13);
            this.label13.TabIndex = 44;
            this.label13.Text = "Wait time for N-EVENT-REPORT from PACS (in sec):";
            // 
            // label17
            // 
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.label17.Location = new System.Drawing.Point(6, 156);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(134, 20);
            this.label17.TabIndex = 40;
            this.label17.Text = "Listen Port:";
            // 
            // emulatorIPAddress
            // 
            this.emulatorIPAddress.BackColor = System.Drawing.SystemColors.Control;
            this.emulatorIPAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emulatorIPAddress.FormattingEnabled = true;
            this.emulatorIPAddress.Location = new System.Drawing.Point(272, 123);
            this.emulatorIPAddress.Name = "emulatorIPAddress";
            this.emulatorIPAddress.Size = new System.Drawing.Size(157, 21);
            this.emulatorIPAddress.TabIndex = 42;
            // 
            // textBoxImplUID
            // 
            this.textBoxImplUID.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxImplUID.Location = new System.Drawing.Point(273, 71);
            this.textBoxImplUID.Name = "textBoxImplUID";
            this.textBoxImplUID.ReadOnly = true;
            this.textBoxImplUID.Size = new System.Drawing.Size(157, 20);
            this.textBoxImplUID.TabIndex = 33;
            this.textBoxImplUID.Text = "1.2.826.0.1.3680043.2.1545.6";
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.label14.Location = new System.Drawing.Point(6, 129);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 16);
            this.label14.TabIndex = 39;
            this.label14.Text = "Local IP Address:";
            // 
            // textBoxAETitle
            // 
            this.textBoxAETitle.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxAETitle.Location = new System.Drawing.Point(272, 45);
            this.textBoxAETitle.Name = "textBoxAETitle";
            this.textBoxAETitle.Size = new System.Drawing.Size(158, 20);
            this.textBoxAETitle.TabIndex = 32;
            this.textBoxAETitle.Text = "MODALITY";
            // 
            // label16
            // 
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.label16.Location = new System.Drawing.Point(6, 103);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(160, 16);
            this.label16.TabIndex = 38;
            this.label16.Text = "Implementation Version Name:";
            // 
            // label15
            // 
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(184)))), ((int)(((byte)(209)))));
            this.label15.Location = new System.Drawing.Point(6, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 16);
            this.label15.TabIndex = 35;
            this.label15.Text = "System Name:";
            // 
            // textBoxImplName
            // 
            this.textBoxImplName.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxImplName.Location = new System.Drawing.Point(273, 97);
            this.textBoxImplName.Name = "textBoxImplName";
            this.textBoxImplName.ReadOnly = true;
            this.textBoxImplName.Size = new System.Drawing.Size(157, 20);
            this.textBoxImplName.TabIndex = 34;
            this.textBoxImplName.Text = "ModalityEmulator";
            // 
            // tabPageWLM
            // 
            this.tabPageWLM.Controls.Add(this.dcmEditorWLM);
            this.tabPageWLM.Location = new System.Drawing.Point(4, 22);
            this.tabPageWLM.Name = "tabPageWLM";
            this.tabPageWLM.Size = new System.Drawing.Size(946, 594);
            this.tabPageWLM.TabIndex = 3;
            this.tabPageWLM.Text = "Worklist Query";
            this.tabPageWLM.UseVisualStyleBackColor = true;
            // 
            // dcmEditorWLM
            // 
            this.dcmEditorWLM.AutoScroll = true;
            this.dcmEditorWLM.DCMFile = "";
            this.dcmEditorWLM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmEditorWLM.Location = new System.Drawing.Point(0, 0);
            this.dcmEditorWLM.Name = "dcmEditorWLM";
            this.dcmEditorWLM.Size = new System.Drawing.Size(946, 594);
            this.dcmEditorWLM.TabIndex = 0;
            // 
            // tabPageMPPSCreate
            // 
            this.tabPageMPPSCreate.Controls.Add(this.dcmEditorMPPSCreate);
            this.tabPageMPPSCreate.Location = new System.Drawing.Point(4, 22);
            this.tabPageMPPSCreate.Name = "tabPageMPPSCreate";
            this.tabPageMPPSCreate.Size = new System.Drawing.Size(946, 594);
            this.tabPageMPPSCreate.TabIndex = 4;
            this.tabPageMPPSCreate.Text = "MPPS-Progress";
            this.tabPageMPPSCreate.UseVisualStyleBackColor = true;
            // 
            // dcmEditorMPPSCreate
            // 
            this.dcmEditorMPPSCreate.AutoScroll = true;
            this.dcmEditorMPPSCreate.DCMFile = "";
            this.dcmEditorMPPSCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmEditorMPPSCreate.Location = new System.Drawing.Point(0, 0);
            this.dcmEditorMPPSCreate.Name = "dcmEditorMPPSCreate";
            this.dcmEditorMPPSCreate.Size = new System.Drawing.Size(946, 594);
            this.dcmEditorMPPSCreate.TabIndex = 0;
            // 
            // tabPageMPPSSet
            // 
            this.tabPageMPPSSet.Controls.Add(this.dcmEditorMPPSSet);
            this.tabPageMPPSSet.Location = new System.Drawing.Point(4, 22);
            this.tabPageMPPSSet.Name = "tabPageMPPSSet";
            this.tabPageMPPSSet.Size = new System.Drawing.Size(946, 594);
            this.tabPageMPPSSet.TabIndex = 5;
            this.tabPageMPPSSet.Text = "MPPS-Completed";
            this.tabPageMPPSSet.UseVisualStyleBackColor = true;
            // 
            // dcmEditorMPPSSet
            // 
            this.dcmEditorMPPSSet.AutoScroll = true;
            this.dcmEditorMPPSSet.DCMFile = "";
            this.dcmEditorMPPSSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmEditorMPPSSet.Location = new System.Drawing.Point(0, 0);
            this.dcmEditorMPPSSet.Name = "dcmEditorMPPSSet";
            this.dcmEditorMPPSSet.Size = new System.Drawing.Size(946, 594);
            this.dcmEditorMPPSSet.TabIndex = 0;
            // 
            // tabPageMPPSDiscontinued
            // 
            this.tabPageMPPSDiscontinued.Controls.Add(this.dcmEditorDiscontinued);
            this.tabPageMPPSDiscontinued.Location = new System.Drawing.Point(4, 22);
            this.tabPageMPPSDiscontinued.Name = "tabPageMPPSDiscontinued";
            this.tabPageMPPSDiscontinued.Size = new System.Drawing.Size(946, 594);
            this.tabPageMPPSDiscontinued.TabIndex = 10;
            this.tabPageMPPSDiscontinued.Text = "MPPS-Discontinued";
            this.tabPageMPPSDiscontinued.UseVisualStyleBackColor = true;
            // 
            // dcmEditorDiscontinued
            // 
            this.dcmEditorDiscontinued.AutoScroll = true;
            this.dcmEditorDiscontinued.DCMFile = "";
            this.dcmEditorDiscontinued.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmEditorDiscontinued.Location = new System.Drawing.Point(0, 0);
            this.dcmEditorDiscontinued.Name = "dcmEditorDiscontinued";
            this.dcmEditorDiscontinued.Size = new System.Drawing.Size(946, 594);
            this.dcmEditorDiscontinued.TabIndex = 0;
            // 
            // tabPageImageStorage
            // 
            this.tabPageImageStorage.Controls.Add(this.dcmEditorStorage);
            this.tabPageImageStorage.Location = new System.Drawing.Point(4, 22);
            this.tabPageImageStorage.Name = "tabPageImageStorage";
            this.tabPageImageStorage.Size = new System.Drawing.Size(946, 594);
            this.tabPageImageStorage.TabIndex = 6;
            this.tabPageImageStorage.Text = "Image Storage";
            this.tabPageImageStorage.UseVisualStyleBackColor = true;
            // 
            // dcmEditorStorage
            // 
            this.dcmEditorStorage.AutoScroll = true;
            this.dcmEditorStorage.DCMFile = "";
            this.dcmEditorStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmEditorStorage.Location = new System.Drawing.Point(0, 0);
            this.dcmEditorStorage.Name = "dcmEditorStorage";
            this.dcmEditorStorage.Size = new System.Drawing.Size(946, 594);
            this.dcmEditorStorage.TabIndex = 0;
            // 
            // tabPageDummyPatient
            // 
            this.tabPageDummyPatient.Controls.Add(this.dcmEditorDummyPatient);
            this.tabPageDummyPatient.Location = new System.Drawing.Point(4, 22);
            this.tabPageDummyPatient.Name = "tabPageDummyPatient";
            this.tabPageDummyPatient.Size = new System.Drawing.Size(946, 594);
            this.tabPageDummyPatient.TabIndex = 9;
            this.tabPageDummyPatient.Text = "Dummy Patient";
            this.tabPageDummyPatient.UseVisualStyleBackColor = true;
            // 
            // dcmEditorDummyPatient
            // 
            this.dcmEditorDummyPatient.AutoScroll = true;
            this.dcmEditorDummyPatient.DCMFile = "";
            this.dcmEditorDummyPatient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmEditorDummyPatient.Location = new System.Drawing.Point(0, 0);
            this.dcmEditorDummyPatient.Name = "dcmEditorDummyPatient";
            this.dcmEditorDummyPatient.Size = new System.Drawing.Size(946, 594);
            this.dcmEditorDummyPatient.TabIndex = 0;
            // 
            // tabPageActivityLogging
            // 
            this.tabPageActivityLogging.Controls.Add(this.userControlActivityLoggingEmulator);
            this.tabPageActivityLogging.Location = new System.Drawing.Point(4, 22);
            this.tabPageActivityLogging.Name = "tabPageActivityLogging";
            this.tabPageActivityLogging.Size = new System.Drawing.Size(946, 594);
            this.tabPageActivityLogging.TabIndex = 8;
            this.tabPageActivityLogging.Text = "Activity Logging";
            this.tabPageActivityLogging.UseVisualStyleBackColor = true;
            // 
            // userControlActivityLoggingEmulator
            // 
            this.userControlActivityLoggingEmulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlActivityLoggingEmulator.Interval = 250;
            this.userControlActivityLoggingEmulator.Location = new System.Drawing.Point(0, 0);
            this.userControlActivityLoggingEmulator.Name = "userControlActivityLoggingEmulator";
            this.userControlActivityLoggingEmulator.Size = new System.Drawing.Size(946, 594);
            this.userControlActivityLoggingEmulator.TabIndex = 0;
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.dvtkWebBrowserEmulator);
            this.tabPageResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Size = new System.Drawing.Size(946, 594);
            this.tabPageResults.TabIndex = 7;
            this.tabPageResults.Text = "Final Result";
            this.tabPageResults.UseVisualStyleBackColor = true;
            // 
            // dvtkWebBrowserEmulator
            // 
            this.dvtkWebBrowserEmulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvtkWebBrowserEmulator.Location = new System.Drawing.Point(0, 0);
            this.dvtkWebBrowserEmulator.Name = "dvtkWebBrowserEmulator";
            this.dvtkWebBrowserEmulator.Size = new System.Drawing.Size(946, 594);
            this.dvtkWebBrowserEmulator.TabIndex = 0;
            this.dvtkWebBrowserEmulator.XmlStyleSheetFullFileName = "";
            // 
            // toolBarEmulator
            // 
            this.toolBarEmulator.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarEmulator.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonConfigEmulator,
            this.toolBarButtonConfigSys,
            this.toolBarButtonResults,
            this.toolBarButtonLog,
            this.toolBarButtonStop,
            this.toolBarButtonSep,
            this.toolBarButtonLeft,
            this.toolBarButtonUp,
            this.toolBarButtonright});
            this.toolBarEmulator.ButtonSize = new System.Drawing.Size(39, 22);
            this.toolBarEmulator.DropDownArrows = true;
            this.toolBarEmulator.ImageList = this.imageListEmulator;
            this.toolBarEmulator.Location = new System.Drawing.Point(0, 0);
            this.toolBarEmulator.Name = "toolBarEmulator";
            this.toolBarEmulator.ShowToolTips = true;
            this.toolBarEmulator.Size = new System.Drawing.Size(954, 28);
            this.toolBarEmulator.TabIndex = 1;
            this.toolBarEmulator.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarEmulator_ButtonClick);
            // 
            // toolBarButtonConfigEmulator
            // 
            this.toolBarButtonConfigEmulator.ImageIndex = 7;
            this.toolBarButtonConfigEmulator.Name = "toolBarButtonConfigEmulator";
            this.toolBarButtonConfigEmulator.ToolTipText = "Configure Emulator";
            // 
            // toolBarButtonConfigSys
            // 
            this.toolBarButtonConfigSys.ImageIndex = 3;
            this.toolBarButtonConfigSys.Name = "toolBarButtonConfigSys";
            this.toolBarButtonConfigSys.ToolTipText = "Configure Systems";
            // 
            // toolBarButtonResults
            // 
            this.toolBarButtonResults.Enabled = false;
            this.toolBarButtonResults.ImageIndex = 4;
            this.toolBarButtonResults.Name = "toolBarButtonResults";
            this.toolBarButtonResults.ToolTipText = "Display results";
            // 
            // toolBarButtonLog
            // 
            this.toolBarButtonLog.ImageIndex = 5;
            this.toolBarButtonLog.Name = "toolBarButtonLog";
            this.toolBarButtonLog.ToolTipText = "Show logging";
            // 
            // toolBarButtonStop
            // 
            this.toolBarButtonStop.Enabled = false;
            this.toolBarButtonStop.ImageIndex = 6;
            this.toolBarButtonStop.Name = "toolBarButtonStop";
            this.toolBarButtonStop.ToolTipText = "Stop emulator";
            // 
            // toolBarButtonSep
            // 
            this.toolBarButtonSep.Name = "toolBarButtonSep";
            this.toolBarButtonSep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonLeft
            // 
            this.toolBarButtonLeft.Enabled = false;
            this.toolBarButtonLeft.ImageIndex = 0;
            this.toolBarButtonLeft.Name = "toolBarButtonLeft";
            this.toolBarButtonLeft.ToolTipText = "Go back";
            // 
            // toolBarButtonUp
            // 
            this.toolBarButtonUp.Enabled = false;
            this.toolBarButtonUp.ImageIndex = 2;
            this.toolBarButtonUp.Name = "toolBarButtonUp";
            this.toolBarButtonUp.ToolTipText = "Go to top";
            // 
            // toolBarButtonright
            // 
            this.toolBarButtonright.Enabled = false;
            this.toolBarButtonright.ImageIndex = 1;
            this.toolBarButtonright.Name = "toolBarButtonright";
            this.toolBarButtonright.ToolTipText = "Go forward";
            // 
            // imageListEmulator
            // 
            this.imageListEmulator.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListEmulator.ImageStream")));
            this.imageListEmulator.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListEmulator.Images.SetKeyName(0, "");
            this.imageListEmulator.Images.SetKeyName(1, "");
            this.imageListEmulator.Images.SetKeyName(2, "");
            this.imageListEmulator.Images.SetKeyName(3, "");
            this.imageListEmulator.Images.SetKeyName(4, "");
            this.imageListEmulator.Images.SetKeyName(5, "");
            this.imageListEmulator.Images.SetKeyName(6, "");
            this.imageListEmulator.Images.SetKeyName(7, "");
            // 
            // ModalityEmulator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(954, 651);
            this.Controls.Add(this.toolBarEmulator);
            this.Controls.Add(this.tabControlEmulator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mainMenuEmulator;
            this.Name = "ModalityEmulator";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modality Emulator";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ModalityEmulator_Closing);
            this.tabControlEmulator.ResumeLayout(false);
            this.tabPageControl.ResumeLayout(false);
            this.tabPageControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.emulatorHeader)).EndInit();
            this.configureTab.ResumeLayout(false);
            this.configureTab.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageWLM.ResumeLayout(false);
            this.tabPageMPPSCreate.ResumeLayout(false);
            this.tabPageMPPSSet.ResumeLayout(false);
            this.tabPageMPPSDiscontinued.ResumeLayout(false);
            this.tabPageImageStorage.ResumeLayout(false);
            this.tabPageDummyPatient.ResumeLayout(false);
            this.tabPageActivityLogging.ResumeLayout(false);
            this.tabPageResults.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
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

			Application.Run(new ModalityEmulator());

#if !DEBUG
			}
			catch(Exception exception)
			{
				CustomExceptionHandler.ShowThreadExceptionDialog(exception);
			}
#endif
		}

		private void CreateIntegrationProfile()
		{
			wrapper = new AcquisitionModalityWrapper(Application.StartupPath + "\\IheAcquisitionModality.xml");

            wrapper.OnMessageAvailable += new AcquisitionModalityWrapper.MessageAvailableHandler(wrapper_OnMessageAvailable);
            wrapper.OnTransactionAvailable += new AcquisitionModalityWrapper.TransactionAvailableHandler(wrapper_OnTransactionAvailable);

			//Used for debugging
			//Dvtk.IheActors.UserInterfaces.Form form = new Dvtk.IheActors.UserInterfaces.Form();
			//form.Attach(wrapper.ModalityIntegrationProfile);
			userControlActivityLoggingEmulator.Attach(wrapper.ModalityIntegrationProfile);

            if (isUIUpdateReqd)
            {
                //Update UI from the values read from config XML file
                foreach (DicomPeerToPeerConfig peerToPeerConfig in wrapper.ModalityIntegrationProfile.Config.PeerToPeerConfig)
                {
                    //peerToPeerConfig.FromActorName fromActor = new ActorName
                    if ((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) &&
                        (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.DssOrderFiller))
                    {
                        textBoxAETitle.Text = peerToPeerConfig.FromActorAeTitle;
                        textBoxImplUID.Text = peerToPeerConfig.ActorOption2;
                        textBoxImplName.Text = peerToPeerConfig.ActorOption3;
                        textBoxRISAETitle.Text = peerToPeerConfig.ToActorAeTitle;
                        textBoxRISIPAddress.Text = peerToPeerConfig.ToActorIpAddress;
                        textBoxRISPort.Text = peerToPeerConfig.PortNumber.ToString();
                    }
                    
                    if ((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) &&
                        (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.PerformedProcedureStepManager))
                    {
                        textBoxMPPSAETitle.Text = peerToPeerConfig.ToActorAeTitle;
                        textBoxMPPSIPAddress.Text = peerToPeerConfig.ToActorIpAddress;
                        textBoxMPPSPort.Text = peerToPeerConfig.PortNumber.ToString();
                    }

                    if ((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) &&
                        (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.ImageArchive))
                    {
                        textBoxPACSAETitle.Text = peerToPeerConfig.ToActorAeTitle;
                        textBoxPACSIPAddress.Text = peerToPeerConfig.ToActorIpAddress;
                        textBoxPACSPort.Text = peerToPeerConfig.PortNumber.ToString();
                    }

                    if ((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) &&
                        (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.ImageManager))
                    {
                        //textBoxPACSCommitAETitle.Text = peerToPeerConfig.ToActorAeTitle;
                        //textBoxPACSCommitIPAddress.Text = peerToPeerConfig.ToActorIpAddress;
                        //textBoxPACSCommitPort.Text = peerToPeerConfig.PortNumber.ToString();
                    }

                    if (((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.ImageManager) ||
                        (peerToPeerConfig.FromActorName.Type == ActorTypeEnum.ImageArchive)) &&
                        (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.AcquisitionModality))
                    {
                        string localIpAddr = emulatorIPAddress.SelectedItem.ToString();
                        if (localIpAddr != peerToPeerConfig.ToActorIpAddress)
                        {
                            emulatorIPAddress.Items.Add(peerToPeerConfig.ToActorIpAddress);
                        }
                        textBoxPort.Text = peerToPeerConfig.PortNumber.ToString();
                    }
                }
            }
		}

        void wrapper_OnTransactionAvailable(object server, TransactionAvailableEventArgs transactionAvailableEvent)
        {
            if (transactionAvailableEvent.Transaction.Transaction is DicomTransaction)
            {
                DicomTransaction dicomTransaction = (DicomTransaction)transactionAvailableEvent.Transaction.Transaction;
                for (int i = 0; i < dicomTransaction.DicomMessages.Count; i++)
                {
                    DicomMessage dicomMessage = (DicomMessage)dicomTransaction.DicomMessages[i];
                    if (dicomMessage.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.CECHORSP)
                    {
                        echoResultFile = transactionAvailableEvent.Transaction.ResultsFilename;
                    }
                }
            }
        }

        void wrapper_OnMessageAvailable(object server, MessageAvailableEventArgs messageAvailableEvent)
        {
        }		

		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItemConfigEmulator_Click(object sender, System.EventArgs e)
		{
			menuItemConfigEmulator.Checked = !menuItemConfigEmulator.Checked;
			if(menuItemConfigEmulator.Checked)
			{
				tabControlEmulator.Controls.Add(tabPageWLM);
				tabControlEmulator.Controls.Add(tabPageMPPSCreate);
                tabControlEmulator.Controls.Add(tabPageMPPSDiscontinued);
				tabControlEmulator.Controls.Add(tabPageMPPSSet);
				tabControlEmulator.Controls.Add(tabPageImageStorage);
				tabControlEmulator.Controls.Add(tabPageDummyPatient);
			}
			else
			{
				tabControlEmulator.Controls.Remove(tabPageWLM);
				tabControlEmulator.Controls.Remove(tabPageMPPSCreate);
                tabControlEmulator.Controls.Remove(tabPageMPPSDiscontinued);
				tabControlEmulator.Controls.Remove(tabPageMPPSSet);
				tabControlEmulator.Controls.Remove(tabPageImageStorage);
				tabControlEmulator.Controls.Remove(tabPageDummyPatient);
			}
		}

        private bool checkRISConfig()
        {
            bool ok = true;

            if (textBoxRISAETitle.Text == "")
            {
                MessageBox.Show("Specify RIS AE Title", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            if (textBoxRISIPAddress.Text == "")
            {
                MessageBox.Show("Specify RIS IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false; 
            }

            if (textBoxRISPort.Text == "")
            {
                MessageBox.Show("Specify RIS Port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            return ok;
        }

        private bool checkPACSConfig()
        {
            bool ok = true;

            if (textBoxPACSAETitle.Text == "")
            {
                MessageBox.Show("Specify PACS AE Title", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            if (textBoxPACSIPAddress.Text == "")
            {
                MessageBox.Show("Specify PACS IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            if (textBoxPACSPort.Text == "")
            {
                MessageBox.Show("Specify PACS Port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            return ok;
        }

		private void ProcessPingResponse(string hostname)
		{
            string msg = "";
            PingReply reply = null;
            bool ok = false;
            string ipAddr = "";

            System.Threading.Thread.Sleep(250);

            try
            {
                ipAddr = hostname.Trim();

                Ping pingSender = new Ping();
                reply = pingSender.Send(ipAddr, 4);
            }
            catch (Exception exp)
            {
                msg = string.Format("Error in pinging to {0}: {1}", hostname, exp.Message);                
            }

            if (reply != null)
            {
                switch (reply.Status)
                {
                    case IPStatus.Success:
                        msg = "Ping successful.";
                        ok = true;
                        break;
                    case IPStatus.TimedOut:
                        msg = "Ping Timeout.";
                        break;
                    case IPStatus.IcmpError:
                        msg = "The ICMP echo request failed because of an ICMP protocol error.";
                        break;
                    case IPStatus.BadRoute:
                        msg = "The ICMP echo request failed because there is no valid route between the source and destination computers.";
                        break;
                    case IPStatus.DestinationProhibited:
                        msg = "The ICMP echo request failed because contact with the destination computer is administratively prohibited.";
                        break;
                    case IPStatus.DestinationNetworkUnreachable:
                    case IPStatus.DestinationHostUnreachable:
                    case IPStatus.DestinationPortUnreachable:
                    case IPStatus.DestinationUnreachable:
                        msg = "Destination host Unreachable.";
                        break;
                    case IPStatus.Unknown:
                        msg = "The ICMP echo request failed for an unknown reason.";
                        break;
                }
            }

            if (ok)
                connectivityLog.AppendText(msg + "\r\n");
            else
            {
                connectivityLog.AppendText("Ping failed.\r\n");
                SystemSounds.Beep.Play();
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

		private bool ProcessEchoCommand(ActorTypeEnum actorType)
		{
			bool isOk = true;
			toolBarButtonLog.Enabled = true;
			toolBarButtonStop.Enabled = true;

            System.Threading.Thread.Sleep(250);

			if(!isCreated)
			{
				CreateIntegrationProfile();
				isCreated = true;
			}

            if(!isInitialized)
			{
                //Apply updated settings
                if (!UpdateConfig())
                    return false;

				wrapper.Initialize();
				isInitialized = true;
				isTerminated = false;
			}
			
			try
			{
                isOk = wrapper.SendVerification(actorType);
			}
			catch(Exception except)
			{
				string msg = string.Format("Error in DICOM Echo from due to {0}.",except.Message);
				isOk = false;
			}

            return isOk;
		}

        private bool UpdateConfig()
		{
            updateCount++;
            string configFileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\Modality Emulator";
            wrapper.ModalityIntegrationProfile.Config.CommonConfig.RootedBaseDirectory = configFileDirectory;
		    foreach(DicomPeerToPeerConfig peerToPeerConfig in wrapper.ModalityIntegrationProfile.Config.PeerToPeerConfig)
		    {
			    if((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) && 
				    (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.DssOrderFiller))
			    {
				    peerToPeerConfig.FromActorAeTitle = textBoxAETitle.Text;
				    peerToPeerConfig.ToActorAeTitle = textBoxRISAETitle.Text;
				    peerToPeerConfig.ToActorIpAddress = textBoxRISIPAddress.Text;
                    
                    try
                    {
                        peerToPeerConfig.PortNumber = System.UInt16.Parse(textBoxRISPort.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Specify proper port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

				    peerToPeerConfig.ActorOption2 = textBoxImplUID.Text;
				    peerToPeerConfig.ActorOption3 = textBoxImplName.Text;
				    peerToPeerConfig.SourceDataDirectory = wlmRqDataDirectory;
			    }

			    if((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) && 
				    (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.PerformedProcedureStepManager))
			    {
				    peerToPeerConfig.FromActorAeTitle = textBoxAETitle.Text;
				    peerToPeerConfig.ToActorAeTitle = textBoxMPPSAETitle.Text;
				    peerToPeerConfig.ActorOption2 = textBoxImplUID.Text;
				    peerToPeerConfig.ActorOption3 = textBoxImplName.Text;
				    peerToPeerConfig.ToActorIpAddress = textBoxMPPSIPAddress.Text;

                    try
                    {
                        peerToPeerConfig.PortNumber = System.UInt16.Parse(textBoxMPPSPort.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Specify proper port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

				    peerToPeerConfig.SourceDataDirectory = mppsDataDirectory;
			    }

                if ((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) &&
                    (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.ImageArchive))
                {
                    peerToPeerConfig.FromActorAeTitle = textBoxAETitle.Text;
                    peerToPeerConfig.ToActorAeTitle = textBoxPACSAETitle.Text;
                    peerToPeerConfig.ActorOption2 = textBoxImplUID.Text;
                    peerToPeerConfig.ActorOption3 = textBoxImplName.Text;
                    peerToPeerConfig.ToActorIpAddress = textBoxPACSIPAddress.Text;

                    try
                    {
                        peerToPeerConfig.PortNumber = System.UInt16.Parse(textBoxPACSPort.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Specify proper port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    peerToPeerConfig.SourceDataDirectory = storageDataDirectory;
                }

                if ((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.AcquisitionModality) &&
                    (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.ImageManager))
                {
                    peerToPeerConfig.FromActorAeTitle = textBoxAETitle.Text;
                    //peerToPeerConfig.ToActorAeTitle = textBoxPACSCommitAETitle.Text;

                    if (isSyncCommit)
                        peerToPeerConfig.ActorOption1 = "DO_STORAGE_COMMITMENT_ON_SINGLE_ASSOCIATION";
                    else
                        peerToPeerConfig.ActorOption1 = "";

                    peerToPeerConfig.ActorOption2 = textBoxImplUID.Text;
                    peerToPeerConfig.ActorOption3 = textBoxImplName.Text;
                    //peerToPeerConfig.ToActorIpAddress = textBoxPACSCommitIPAddress.Text;

                    try
                    {
                        //peerToPeerConfig.PortNumber = System.UInt16.Parse(textBoxPACSCommitPort.Text);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Specify proper port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

			    if(((peerToPeerConfig.FromActorName.Type == ActorTypeEnum.ImageManager) ||
				    (peerToPeerConfig.FromActorName.Type == ActorTypeEnum.ImageArchive)) && 
				    (peerToPeerConfig.ToActorName.Type == ActorTypeEnum.AcquisitionModality))
			    {
                    peerToPeerConfig.ToActorIpAddress = emulatorIPAddress.SelectedItem.ToString();

                    if (textBoxPort.Text == "")
                        MessageBox.Show("Specify Emulator Port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        try
                        {
                            peerToPeerConfig.PortNumber = System.UInt16.Parse(textBoxPort.Text);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Specify proper port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }

                    if (isSyncCommit)
                        peerToPeerConfig.ActorOption1 = "DO_STORAGE_COMMITMENT_ON_SINGLE_ASSOCIATION";
                    else
                        peerToPeerConfig.ActorOption1 = "";

			    }
		    }

            lastRISIPAddress = textBoxRISIPAddress.Text;
            lastRISAETitle = textBoxRISAETitle.Text;
            lastMPPSIPAddress = textBoxMPPSIPAddress.Text;
            lastMPPSAETitle = textBoxMPPSAETitle.Text;
            lastRetrieveAETitle = retrieveAETitleBox.Text;
            lastPACSIPAddress = textBoxPACSIPAddress.Text;
            lastPACSAETitle = textBoxPACSAETitle.Text;
            lastPort = textBoxPort.Text;
            lastRISPort = textBoxRISPort.Text;
            lastPACSPort = textBoxPACSPort.Text;
            lastMPPSPort = textBoxMPPSPort.Text;

		    //Save the config
		    wrapper.ModalityIntegrationProfile.Config.Save(Application.StartupPath + "\\IheAcquisitionModality.xml");
            updateCount--;

            return true;
		}

		private void buttonReqWL_Click(object sender, System.EventArgs e)
        {
            toolBarButtonLog.Enabled = true;
            toolBarButtonStop.Enabled = true;
            performScanButton.Enabled = false;
            finalizeExamButton.Enabled = false;
            activityLogBox.Text = "Requesting Worklist...";

            bool isSendOk = false;			

			//If result tab is present, remove it
			if(tabControlEmulator.Controls.Contains(tabPageResults))
			{
				tabControlEmulator.Controls.Remove(tabPageResults);
			}

			try
			{
                if (!checkRISConfig())
                    return;

                if (!isCreated)
                {
                    CreateIntegrationProfile();
                    isCreated = true;
                }

                if (!isInitialized)
                {
                    //Apply updated settings
                    if (!UpdateConfig())
                        return;

                    wrapper.Initialize();
                    isInitialized = true;
                    isTerminated = false;
                }

				string wlmRqDcmFile = dcmEditorWLM.DCMFile;
				string scheduledProcStepStartDate = System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
				if(wlmRqDcmFile != "")
                    isSendOk = wrapper.GetWorklist(wlmRqDcmFile, scheduledProcStepStartDate);

				//isRISConnected = true;
                if (isSendOk)
                {
                    if (wrapper.ModalityActor.ModalityWorklistItems.Count > 0)
                    {
                        activityLogBox.Text = string.Format("Received {0} worklist items successfully.", wrapper.ModalityActor.ModalityWorklistItems.Count);
                        toolBarButtonResults.Enabled = true;
                        foreach (DicomQueryItem mwlItem in wrapper.ModalityActor.ModalityWorklistItems)
                        {
                            DicomFile dcmFile = new DicomFile();

                            // Save dataset to DCM file
                            dcmFile.DataSet = mwlItem.DicomMessage.DataSet;
                            string mwlRspFileName = string.Format("wlmRsp{0:0000}", mwlItem.Id);

                            // create the sub mwlrsp directory
                            string subMwlRspDirectory = System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            string fullPath = wlmRspDataDirectory + subMwlRspDirectory;
                            DirectoryInfo directoryInfo = Directory.CreateDirectory(fullPath);

                            dcmFile.Write(fullPath + "\\" + mwlRspFileName);
                        }
                    }
                    else
                    {
                        activityLogBox.Text = "No worklist item received.";
                        isSendOk = false;
                        selectExamButton.Enabled = true;
                    }                    
                }
                else
                {
                    activityLogBox.Text = "Failed to send C-FIND-RQ message to RIS.";
                    selectExamButton.Enabled = true;
                    SystemSounds.Beep.Play();
                }

                // Select patient
                if (selectPatient())
                {
                    startMPPS();
                }
			}
			catch(Exception except)
			{
				string msg = string.Format("Error: No worklist items received from {0} due to {1}.",textBoxRISAETitle.Text, except.Message);
                activityLogBox.Text = msg;
                performScanButton.Enabled = false;
                finalizeExamButton.Enabled = false;
                selectExamButton.Enabled = true;
			}
		}

        private bool selectPatient()
        {
            if (wrapper.ModalityActor.ModalityWorklistItems.Count < 1)
            {
                MessageBox.Show(this, "No patients returned from the RIS.", "No Patient Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            // Pop up MWL rsp dialog
            MWLResponse dlg = new MWLResponse(wrapper.ModalityActor.ModalityWorklistItems);
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                selectExamButton.Enabled = true;
                toolBarButtonStop.Enabled = true;
                performScanButton.Enabled = false;
                finalizeExamButton.Enabled = false;
                toolBarButtonConfigSys.Enabled = true;

                return false;
            }
            else
            {
                selectedMWLItem = dlg.SelectedPatient;

                if (selectedMWLItem == null)
                {
                    selectExamButton.Enabled = true;
                    toolBarButtonLog.Enabled = false;
                    toolBarButtonStop.Enabled = false;
                    performScanButton.Enabled = false;
                    finalizeExamButton.Enabled = false;
                    toolBarButtonConfigSys.Enabled = true;
                    toolBarButtonResults.Enabled = false;

                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool checkMPPSConfig()
        {
            bool ok = true;

            if (textBoxMPPSAETitle.Text == "")
            {
                MessageBox.Show("Specify MPPS AE Title", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            if (textBoxMPPSIPAddress.Text == "")
            {
                MessageBox.Show("Specify MPPS IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            if (textBoxMPPSPort.Text == "")
            {
                MessageBox.Show("Specify MPPS Port", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ok = false;
            }

            return ok;
        }

        private void startMPPS()
        {
            toolBarButtonConfigSys.Enabled = true;
            activityLogBox.Text = "Sending MPPS Progress...";

            //this.Invoke(new CleanUp(ClearInformation), new string[] { buttonMPPSMsg.Text });

            try
            {
                if (!checkMPPSConfig())
                    return;

                string mppsCreateDcmFile = dcmEditorMPPSCreate.DCMFile;
                if (mppsCreateDcmFile != "")
                {
                    if (wrapper.SendMppsInProgress(selectedMWLItem, mppsCreateDcmFile))
                    {
                        activityLogBox.Text = "Send MPPS progress message successfully.";
                        performScanButton.Enabled = true;
                        finalizeExamButton.Enabled = true;
                        progressCompleted = false;
                        selectExamButton.Enabled = false;
                        this.finalizeExamButton.BackgroundImage = global::ModalityEmulator.Properties.Resources.discontinue_exam;
                    }
                    else
                    {
                        activityLogBox.Text = "Fail to send N-CREATE-RQ message to MPPS.";
                    }
                }
            }
            catch (Exception except)
            {
                throw except;
            }
        }

		private void buttonStoreImage_Click(object sender, System.EventArgs e)
        {
            performScanButton.Enabled = false;
            progressCompleted = false;
            toolBarButtonConfigSys.Enabled = true;
            activityLogBox.Text = "Sending images...";

            //this.Invoke(new CleanUp(ClearInformation), new string[] { "Storage Image" });
			try
			{
                if (!checkPACSConfig())
                    return;

				// Set up worklist item - storage directory mapping
				FileInfo storageDCMFile = new FileInfo(dcmEditorStorage.DCMFile);
				if((storageDCMFile.DirectoryName + "\\") != storageDataDirectory)
				{
					if(wrapper.ModalityActor.MapWorklistItemToStorageDirectory.IsExistingMapping("Default"))
					{
						wrapper.ModalityActor.MapWorklistItemToStorageDirectory.RemoveMapping("Default");					
					}
					wrapper.ModalityActor.MapWorklistItemToStorageDirectory.AddMapping("Default", storageDCMFile.DirectoryName);
				}

                ActorName actor = new ActorName(ActorTypeEnum.ImageArchive, textBoxPACSAETitle.Text);
				wrapper.ClearTransferSyntaxProposalForDicomClient(actor);

                if (wrapper.SendImages(selectedMWLItem, true, retrieveAETitleBox.Text))
                {
                    activityLogBox.Text = string.Format("Send Images to {0} sucessfully.", textBoxPACSAETitle.Text);
                    progressCompleted = true;
                    this.finalizeExamButton.BackgroundImage = global::ModalityEmulator.Properties.Resources.finalize_exam;
                }
                else
                {
                    activityLogBox.Text = "Fail to send C-STORE-RQ message to PACS.";
                    performScanButton.Enabled = true;
                    progressCompleted = false;
                    SystemSounds.Beep.Play();
                }
			}
			catch(Exception except)
			{
				string msg = string.Format("Error: No response from {0} due to {1}.",textBoxPACSAETitle.Text, except.Message);
                activityLogBox.Text = msg;
                performScanButton.Enabled = true;
                progressCompleted = false;
			}
		}

		private void buttonStorageCommit_Click(object sender, System.EventArgs e)
		{
            //buttonStorageCommit.Enabled = false;	
            //toolBarButtonStop.Enabled = true;
            //toolBarButtonConfigSys.Enabled = true;
            //buttonReqWL.Enabled = true;
            //buttonStoreImage.Enabled = false;
            //buttonMPPSMsg.Enabled = false;
            //labelStoreCommit.Text = "";
            //labelStoreImage.Text = "";
            //labelMPPSMsg.Text = "";

            //this.Invoke(new CleanUp(ClearInformation), new string[] { "Storage Commit" });
            //try
            //{
            //    if (!checkCommitConfig())
            //        return;

            //    ActorName actor = new ActorName(ActorTypeEnum.ImageManager, "IMCommit");

            //    //Clear TS support
            //    if (isSyncCommit)
            //        wrapper.ClearTransferSyntaxProposalForDicomClient(actor);
            //    else
            //        wrapper.ClearTransferSyntaxSupportForDicomServer(actor);

            //    //Configure selected TS support
            //    foreach (string ts in selectedTS)
            //    {
            //        if (isSyncCommit)
            //            wrapper.AddTransferSyntaxProposalForDicomClient(actor, ts);
            //        else
            //            wrapper.AddTransferSyntaxSupportForDicomServer(actor, ts);
            //    }

            //    int timeout = 1;
            //    try
            //    {
            //        timeout = int.Parse(textBoxTimeOut.Text);
            //    }
            //    catch (Exception)
            //    {
            //        MessageBox.Show("Specify proper timeout.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }

            //    if (wrapper.SendStorageCommitment(timeout))
            //    {
            //        labelStoreCommit.Text = "Send Storage Commitment message successfully.";
            //        //buttonMPPSMsg.Enabled = true;
            //    }
            //    else
            //    {
            //        labelStoreCommit.Text = "Fail to send Storage Commitment message.";
            //        labelPACS.Text = "Check the network configuration?";
            //    }
            //}
            //catch(Exception except)
            //{
            //    string msg = string.Format("Error: No response from {0} due to {1}.",textBoxPACSAETitle.Text, except.Message);
            //    labelStoreCommit.Text = msg;
            //}
		}

        private void toolBarEmulator_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if( e.Button == toolBarButtonLeft)
			{
				this.dvtkWebBrowserEmulator.Back();
			}
			else if( e.Button == toolBarButtonright)
			{
				this.dvtkWebBrowserEmulator.Forward();
			}			
			else if( e.Button == toolBarButtonUp)
			{
				this.dvtkWebBrowserEmulator.Navigate(wrapper.FinalResultFile);				
			}
			else if( e.Button == toolBarButtonLog)
			{
				bool isTabAdded = tabControlEmulator.Controls.Contains(tabPageActivityLogging);
				if(!isTabAdded)
					tabControlEmulator.Controls.Add(tabPageActivityLogging);
				else
					tabControlEmulator.Controls.Remove(tabPageActivityLogging);
			}
			else if( e.Button == toolBarButtonResults)
			{
				if(!isTerminated)
				{
					wrapper.Terminate();
					isTerminated = true;
					isInitialized = false;
					isCreated = false;
					toolBarButtonStop.Enabled = false;
					performScanButton.Enabled = false;
					finalizeExamButton.Enabled = false;

					activityLogBox.Text = "";
				}

				bool isTabAdded = tabControlEmulator.Controls.Contains(tabPageResults);
				if(!isTabAdded)
				{
					toolBarButtonLeft.Enabled = true;
					toolBarButtonUp.Enabled = true;
					toolBarButtonright.Enabled = true;

					tabControlEmulator.Controls.Add(tabPageResults);
					dvtkWebBrowserEmulator.Navigate(wrapper.FinalResultFile);
				}
				else
				{
					toolBarButtonLeft.Enabled = false;
					toolBarButtonUp.Enabled = false;
					toolBarButtonright.Enabled = false;

					tabControlEmulator.Controls.Remove(tabPageResults);
				}
			}
			else if( e.Button == toolBarButtonConfigSys)
			{
				tabControlEmulator.SelectedTab = configureTab;
                menuItem1_Click(sender, null);				
			}
			else if( e.Button == toolBarButtonConfigEmulator)
			{
				menuItemConfigEmulator_Click(sender, null);
			}
			else if( e.Button == toolBarButtonStop)
			{
				toolBarButtonResults.Enabled = true;
				toolBarButtonStop.Enabled = false;
				toolBarButtonConfigSys.Enabled = true;
				selectExamButton.Enabled = true;
				performScanButton.Enabled = false;
				finalizeExamButton.Enabled = false;

				activityLogBox.Text = "";

				if(!isTerminated)
				{
					wrapper.Terminate();
					isTerminated = true;
					isInitialized = false;
					isCreated = false;
                    isUIUpdateReqd = false;

                    //Apply updated settings
                    UpdateConfig();
				}				
			}
			else{}
		}

		private void dvtkWebBrowserEmulator_BackwardFormwardEnabledStateChangeEvent()
		{
			this.toolBarButtonLeft.Enabled = this.dvtkWebBrowserEmulator.IsBackwardEnabled;
			this.toolBarButtonright.Enabled = this.dvtkWebBrowserEmulator.IsForwardEnabled;
		}

		private void ModalityEmulator_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//Save the current config parameters
			UpdateConfig();
			
			if(isInitialized && (!isTerminated))
				wrapper.Terminate();

			//Save the last DCM files used by the user
			StreamWriter writer = new StreamWriter(userConfigFilePath);
			writer.WriteLine(dcmEditorWLM.DCMFile);
			writer.WriteLine(dcmEditorMPPSCreate.DCMFile);
			writer.WriteLine(dcmEditorStorage.DCMFile);
			writer.WriteLine(dcmEditorMPPSSet.DCMFile);
            writer.WriteLine(dcmEditorDiscontinued.DCMFile);
			writer.Close();

			//Remove all temporary files generated during execution
			cleanUp();
		}

		private void cleanUp()
		{
			ArrayList theFilesToRemove = new ArrayList();
            string resultDir = Application.StartupPath + @"\results";
            DirectoryInfo theDirectoryInfo = new DirectoryInfo(resultDir);
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
                string theFullFileName = Path.Combine(resultDir, theFileName);

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
					}
				}				
			}
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			AboutForm about = new AboutForm("Modality Emulator");
            //about.ver
			about.ShowDialog();
		}

		private void buttonTS_Click(object sender, System.EventArgs e)
		{
			SelectTransferSyntaxesForm  theSelectTransferSyntaxesForm = new SelectTransferSyntaxesForm();

			ArrayList tsList = new ArrayList();
			tsList.Add (DvtkData.Dul.TransferSyntax.Implicit_VR_Little_Endian);
			tsList.Add (DvtkData.Dul.TransferSyntax.Explicit_VR_Big_Endian);
			tsList.Add (DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian);

			if(selectedTS.Count != 0)
			{
				foreach (string ts in selectedTS)
				{
					theSelectTransferSyntaxesForm.SupportedTransferSyntaxes.Add(new DvtkData.Dul.TransferSyntax(ts));
				}
			}
			else
			{
				theSelectTransferSyntaxesForm.SupportedTransferSyntaxes.Add(new DvtkData.Dul.TransferSyntax("1.2.840.10008.1.2"));
			}

			theSelectTransferSyntaxesForm.DefaultTransferSyntaxesList = tsList;
			theSelectTransferSyntaxesForm.DisplaySelectAllButton = false;
			theSelectTransferSyntaxesForm.DisplayDeSelectAllButton = false;
			
			theSelectTransferSyntaxesForm.selectSupportedTS();
			theSelectTransferSyntaxesForm.ShowDialog (this);

			if(theSelectTransferSyntaxesForm.SupportedTransferSyntaxes.Count == 0)
			{
				string theWarningText = "No Transfer Syntax is selected, default ILE will be supported.";
				MessageBox.Show(theWarningText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			else
			{
				selectedTS.Clear();
				
				foreach (DvtkData.Dul.TransferSyntax ts in theSelectTransferSyntaxesForm.SupportedTransferSyntaxes)
				{
					selectedTS.Add(ts.UID);					
				}
			}
        }

        private void textBoxRISIPAddress_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                textBoxRISIPAddress.BeepOnError = true;
                //textBoxRISIPAddress.Mask = "999.999.999.999";

                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxRISIPAddress.Text != lastRISIPAddress)
                    {
                        updateCount++;
                        textBoxRISIPAddress.Text = lastRISIPAddress;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxRISAETitle_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxRISAETitle.Text != lastRISAETitle)
                    {
                        updateCount++;
                        textBoxRISAETitle.Text = lastRISAETitle;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxMPPSIPAddress_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                textBoxMPPSIPAddress.BeepOnError = true;
                //textBoxMPPSIPAddress.Mask = "000.000.000.000";

                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxMPPSIPAddress.Text != lastMPPSIPAddress)
                    {
                        updateCount++;
                        textBoxMPPSIPAddress.Text = lastMPPSIPAddress;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxMPPSAETitle_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxMPPSAETitle.Text != lastMPPSAETitle)
                    {
                        updateCount++;
                        textBoxMPPSAETitle.Text = lastMPPSAETitle;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxPACSIPAddress_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                textBoxPACSIPAddress.BeepOnError = true;
                //textBoxPACSIPAddress.Mask = "000.000.000.000";

                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxPACSIPAddress.Text != lastPACSIPAddress)
                    {
                        updateCount++;
                        textBoxPACSIPAddress.Text = lastPACSIPAddress;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxPACSAETitle_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxPACSAETitle.Text != lastPACSAETitle)
                    {
                        updateCount++;
                        textBoxPACSAETitle.Text = lastPACSAETitle;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxPort.Text != lastPort)
                    {
                        updateCount++;
                        textBoxPort.Text = lastPort;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxRISPort_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxRISPort.Text != lastRISPort)
                    {
                        updateCount++;
                        textBoxRISPort.Text = lastRISPort;
                        updateCount--;
                    }
                }
            }            
        }

        private void textBoxMPPSPort_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxMPPSPort.Text != lastMPPSPort)
                    {
                        updateCount++;
                        textBoxMPPSPort.Text = lastMPPSPort;
                        updateCount--;
                    }
                }
            }
        }

        private void textBoxPACSPort_TextChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the UpdateConfig method has been called.
            if (updateCount == 0)
            {
                if (isInitialized)
                {
                    string msg = "Restart the emulator to take effect configuration changes\n by pressing Stop button.";
                    MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBoxPACSPort.Text != lastPACSPort)
                    {
                        updateCount++;
                        textBoxPACSPort.Text = lastPACSPort;
                        updateCount--;
                    }
                }
            }
        }

        private void finalizeExamButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkMPPSConfig())
                    return;

                if (progressCompleted)
                {
                    toolBarButtonConfigSys.Enabled = true;

                    string mppsSetDcmFile = dcmEditorMPPSSet.DCMFile;
                    if (mppsSetDcmFile != "")
                    {
                        if (wrapper.SendMppsCompleted(mppsSetDcmFile, retrieveAETitleBox.Text))
                        {
                            activityLogBox.Text = "Send MPPS completed message successfully.";
                            selectExamButton.Enabled = true;
                            performScanButton.Enabled = false;
                            finalizeExamButton.Enabled = false;
                            progressCompleted = false;
                        }
                        else
                        {
                            activityLogBox.Text = "Fail to send N-SET Completed message to MPPS.";
                        }
                    }
                }
                else
                {
                    toolBarButtonConfigSys.Enabled = true;

                    string mppsDiscontinuedDcmFile = dcmEditorDiscontinued.DCMFile;
                    if (mppsDiscontinuedDcmFile != "")
                    {
                        if (wrapper.SendMppsDiscontinued(mppsDiscontinuedDcmFile))
                        {
                            activityLogBox.Text = "Send MPPS discontinued message successfully.";
                            selectExamButton.Enabled = true;
                            performScanButton.Enabled = false;
                            finalizeExamButton.Enabled = false;

                            toolBarButtonResults.Enabled = true;
                            toolBarButtonStop.Enabled = true;
                        }
                        else
                        {
                            activityLogBox.Text = "Fail to send N-SET Discontinued message to MPPS.";
                        }
                    }
                }
            }
            catch (Exception except)
            {
                string msg = string.Format("Error: No response from {0} due to {1}.", textBoxRISAETitle.Text, except.Message);
                activityLogBox.Text = msg;
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            menuItemConfig.Checked = !menuItemConfig.Checked;
            if (menuItemConfig.Checked)
            {
                tabControlEmulator.Controls.Add(configureTab);
            }
            else
            {
                tabControlEmulator.Controls.Remove(configureTab);
            }
        }

        private void testConnectivityButton_Click(object sender, EventArgs e)
        {
            testConnectivityButton.Enabled = false;
            connectivityLog.Clear();

            // Ping PACS
            if (textBoxPACSIPAddress.Text == null || textBoxPACSIPAddress.Text.Length == 0)
            {
                MessageBox.Show("Specify PACS IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ProcessPingResponse(textBoxPACSIPAddress.Text);
            }
            // Echo PACS
            if (checkPACSConfig())
            {
                if (ProcessEchoCommand(ActorTypeEnum.ImageArchive))
                    connectivityLog.AppendText("PACS DICOM Echo successful.\r\n");
                else
                {
                    connectivityLog.AppendText("PACS DICOM Echo failed.\r\n");
                }
            }
            else
            {
                connectivityLog.AppendText("PACS not configured correctly.\r\n");
            }
            // PING RIS
            if (textBoxRISIPAddress.Text == null || textBoxRISIPAddress.Text.Length == 0)
            {
                MessageBox.Show("Specify RIS IP Address", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ProcessPingResponse(textBoxRISIPAddress.Text);
            }
            // Echo RIS
            if (checkRISConfig())
            {
                if (ProcessEchoCommand(ActorTypeEnum.DssOrderFiller))
                {
                    connectivityLog.AppendText("RIS DICOM Echo successful.\r\n");
                }
                else
                {
                    connectivityLog.AppendText("RIS DICOM Echo failed.\r\n");
                }
            }
            else
            {
                connectivityLog.AppendText("RIS not configured correctly.\r\n");
            }

            testConnectivityButton.Enabled = true;
        }

        private void retrieveAETitleBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RetrieveAETitle = retrieveAETitleBox.Text;
            Properties.Settings.Default.Save();
        }
	}

}
