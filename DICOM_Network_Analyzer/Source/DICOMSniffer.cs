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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Management;
using TCP;
using DICOM;
using Sniffer;
using Dvtk.Sessions;
using System.Xml;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Threads;
using DICOMSniffer;
using DvtkApplicationLayer;

namespace SnifferUI
{
	/// <summary>
	/// Summary description for DICOMSniffer
	/// </summary>
	public class DICOMSniffer : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer timer2;
		private System.Windows.Forms.TabControl tabControlSniffer;
		private System.Windows.Forms.TabPage tabSnifferInformation;
		private System.Windows.Forms.MainMenu mainMenuDICOMSniffer;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemHelp;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemCapture;
		private System.Windows.Forms.MenuItem menuItemConfig;
		private System.Windows.Forms.MenuItem menuItem_configopen;
		private System.Windows.Forms.MenuItem menuItem_configsave;
		private System.Windows.Forms.MenuItem menuItemSaveResultsAs;
		private System.Windows.Forms.MenuItem menuItem_capopen;
		private System.Windows.Forms.MenuItem menuItem_capaspdus;
        private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.ToolBar toolBarSniffer;
		private System.Windows.Forms.ImageList ImageList;
		private System.Windows.Forms.ToolBarButton SaveResultAs;
		private System.Windows.Forms.ToolBarButton SaveConfig;
		private System.Windows.Forms.ToolBarButton ReadCapture;
		private System.Windows.Forms.MenuItem menuItemCap;
		private System.Windows.Forms.MenuItem menuItemCapStart;
		private System.Windows.Forms.MenuItem menuItemCapStop;
        private System.Windows.Forms.MenuItem menuItemView;
		private System.Windows.Forms.MenuItem menuItemMode;
		private System.Windows.Forms.MenuItem menuItemCaptureMode;
		private System.Windows.Forms.MenuItem menuItemAnalysisMode;
		private System.Windows.Forms.TabPage tabAssoOverview;
        private System.Windows.Forms.TabPage tabPDUsOverview;
		private System.Windows.Forms.MenuItem menuItemEvaluateComm;
        private System.Windows.Forms.ToolBarButton CaptureButton;
        private System.Windows.Forms.ProgressBar progressBarStatusBar;
		private System.Windows.Forms.ImageList imageListBrowser;
		private System.Windows.Forms.ToolBarButton Error;
		private System.Windows.Forms.ToolBarButton Warning;
		private System.Windows.Forms.ToolBarButton Backward;
		private System.Windows.Forms.ToolBarButton Forward;
		private System.Windows.Forms.ToolBarButton Mode;
        private System.Windows.Forms.ToolBarButton CleanUp;
        private System.Windows.Forms.StatusBar statusBarSniffer;
		private System.Windows.Forms.ContextMenu contextMenuSniffer;
		private System.Windows.Forms.MenuItem menuItemSaveDCM;
		private System.Windows.Forms.MenuItem menuItemShowPDU;
		private System.Windows.Forms.MenuItem menuItemShowResult;
        private System.Windows.Forms.TabPage tabSummaryValidationResults;
        private System.Windows.Forms.TabPage tabDetailValidationResults;
		private System.Windows.Forms.MenuItem menuItemOptions;
        private System.Windows.Forms.MenuItem menuItemShowPixelData;
		private System.Windows.Forms.ToolBarButton toolBarSep;
		private System.Windows.Forms.ToolBarButton Explore;
        private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkSummaryWebBrowserSniffer;
        private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkDetailWebBrowserSniffer;
        private MenuItem menuItemDetailValidation;
        private ToolBarButton Filter;
        private SplitContainer splitContainerServiceElement;
        private ListBox reqPduList;
        private Label reqIPAddressSELabel;
        private Label reqIPAddress;
        private Label label8;
        private Label label12;
        private Label accIPAddressSELabel;
        private Label label11;
        private Label accIPAddress;
        private Label label7;
        private ListBox accPduList;
        private SplitContainer splitContainerAssocOverview;
        private Button buttonAssReq;
        private RichTextBox richTextBoxReq;
        private Label label1;
        private Button buttonAssocAcc;
        private RichTextBox richTextBoxAccept;
        private Label label4;
        private Panel panel7;
        private Label calledAETitle;
        private Label callingAETitle;
        private Label label6;
        private Label label5;
        private Label accIPAddressLabel;
        private Label reqIPAddressLabel;
        private Label label3;
        private Label label2;
        private SplitContainer splitContainerCapInfo;
        private MaskedTextBox ipAddress1;
        private MaskedTextBox ipAddress2;
        private Label Portlabel;
        private ComboBox comboBoxConnections;
        private Label connectionsLable;
        private ProgressBar progressBarAnalysis;
        private Label progressbarLabel;
        private Label IP2Lable;
        private Label IP1Lable;
        private GroupBox groupBoxCapture;
        private TextBox connection;
        private Label connectionsLabel;
        private TextBox capturePackets;
        private Label capPacketLable;
        private GroupBox groupBoxFilter;
        private MaskedTextBox Port;
        private ListBox adapterList;
        private Label availableAdapters;
        private TextBox selectedAdapter;
        private Label selectedAdapters;

        /// <summary>
        /// Class member variables
        /// </summary>
        public string CurrentBaseFileName;
        public string summaryXmlFullFileName = "";
        public string detailXmlFullFileName = "";
        public bool evaluateAllAssociations = false;
        public bool generateDetailedValidation = true;
		private SnifferInterface sniffer;
		private SnifferInterface capFileSniffer;
		private StreamWriter textLog;
		private StreamWriter activityLog;        
		private TCPParser tcp;
		private DICOMAnalyser dicom;
		private DateTime start;
		private Association assocHandle;
		private string signature = "";
		public ArrayList connectionList = new ArrayList();
		private SnifferSession dvtkSnifferSession = null;        
		string resultDirectoryname;
		string activityLogFileName;
		private bool captureFromFile = false;
        DateTime startTimeForFirstAssoc;
        string tempCapFile;
        FileStream fsTempCapFile = null;
		//private Dvtk.Events.ActivityReportEventHandler activityReportEventHandler;

        public delegate void SetMinimumProgressBarDelegate(int step);
        public delegate void SetMaximumProgressBarDelegate(int step);
		public delegate void PerformProgressDelegate();
        public delegate void IncrementStepInProgressDelegate(int step);
        public delegate void UpdateUIControlsDelegate();
        public delegate void AddConnectionDelegate(string connection);
        public delegate void SelectConnectionDelegate();
		private SetMinimumProgressBarDelegate minProgressHandler = null;
		private SetMaximumProgressBarDelegate maxProgressHandler = null;
		private PerformProgressDelegate performProgressHandler = null;
        private IncrementStepInProgressDelegate incrementStepInProgressHandler = null;
        private UpdateUIControlsDelegate updateUIControlsHandler = null;
        private AddConnectionDelegate addConnectionHandler = null;
        private SelectConnectionDelegate selectConnectionHandler = null;

		Analysis analysisDlg = null;
		static ASCIIEncoding ASCII = new ASCIIEncoding();
        private ArrayList reqList = new ArrayList();
        static public double totalPhysicalMemoryAvailable;
      //  public bool IsRetainInfoCheckBoxChecked = false;
        private ArrayList accList = new ArrayList();
        private string dataDirectory;

        public DICOMSniffer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            dataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\DICOM Network Analyzer";
            tempCapFile = dataDirectory + "\\pdus.tmp";

			// Provide the Style sheet to the DvtkWebBrowser user control
			this.dvtkSummaryWebBrowserSniffer.XmlStyleSheetFullFileName = Application.StartupPath + "\\DVT_RESULTS.xslt";
			this.dvtkSummaryWebBrowserSniffer.BackwardFormwardEnabledStateChangeEvent +=new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(dvtkSummaryWebBrowserSniffer_BackwardFormwardEnabledStateChangeEvent);
			this.dvtkDetailWebBrowserSniffer.XmlStyleSheetFullFileName = Application.StartupPath + "\\DVT_RESULTS.xslt";
            this.dvtkDetailWebBrowserSniffer.BackwardFormwardEnabledStateChangeEvent += new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(dvtkDetailWebBrowserSniffer_BackwardFormwardEnabledStateChangeEvent);

			analysisDlg = new Analysis();

			// Subscribe to Dvtk Activity report handler
			//activityReportEventHandler = new Dvtk.Events.ActivityReportEventHandler(OnActivityReportEvent);

			minProgressHandler = new SetMinimumProgressBarDelegate(SetMinimumInProgressBar);
			maxProgressHandler = new SetMaximumProgressBarDelegate(SetMaximumInProgressBar);
			performProgressHandler = new PerformProgressDelegate(PerformStepInProgressBar);
            incrementStepInProgressHandler = new IncrementStepInProgressDelegate(IncrementStepInProgressBar);
            updateUIControlsHandler = new UpdateUIControlsDelegate(UpdateUIControls);
            addConnectionHandler = new AddConnectionDelegate(AddConnectionToList);
            selectConnectionHandler = new SelectConnectionDelegate(SelectConnectionFromList);

            totalPhysicalMemoryAvailable = computePhysicalMemoryAvailable();
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

		public SetMinimumProgressBarDelegate MinProgressHandler
		{
			get
			{
				return(this.minProgressHandler);
			}
		}

		public SetMaximumProgressBarDelegate MaxProgressHandler
		{
			get
			{
				return(this.maxProgressHandler);
			}
		}

		public PerformProgressDelegate PerformProgressHandler
		{
			get
			{
				return(this.performProgressHandler);
			}
		}

        public IncrementStepInProgressDelegate IncrementStepInProgressHandler
        {
            get
            {
                return (this.incrementStepInProgressHandler);
            }
        }

        public UpdateUIControlsDelegate UpdateUIControlsHandler
        {
            get
            {
                return (this.updateUIControlsHandler);
            }
        }

        public AddConnectionDelegate AddConnectionHandler
        {
            get
            {
                return (this.addConnectionHandler);
            }
        }

        public SelectConnectionDelegate SelectConnectionHandler
        {
            get
            {
                return (this.selectConnectionHandler);
            }
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DICOMSniffer));
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.tabControlSniffer = new System.Windows.Forms.TabControl();
            this.tabSnifferInformation = new System.Windows.Forms.TabPage();
            this.splitContainerCapInfo = new System.Windows.Forms.SplitContainer();
            this.ipAddress1 = new System.Windows.Forms.MaskedTextBox();
            this.ipAddress2 = new System.Windows.Forms.MaskedTextBox();
            this.Portlabel = new System.Windows.Forms.Label();
            this.comboBoxConnections = new System.Windows.Forms.ComboBox();
            this.connectionsLable = new System.Windows.Forms.Label();
            this.progressBarAnalysis = new System.Windows.Forms.ProgressBar();
            this.progressbarLabel = new System.Windows.Forms.Label();
            this.IP2Lable = new System.Windows.Forms.Label();
            this.IP1Lable = new System.Windows.Forms.Label();
            this.groupBoxCapture = new System.Windows.Forms.GroupBox();
            this.connection = new System.Windows.Forms.TextBox();
            this.connectionsLabel = new System.Windows.Forms.Label();
            this.capturePackets = new System.Windows.Forms.TextBox();
            this.capPacketLable = new System.Windows.Forms.Label();
            this.groupBoxFilter = new System.Windows.Forms.GroupBox();
            this.Port = new System.Windows.Forms.MaskedTextBox();
            this.adapterList = new System.Windows.Forms.ListBox();
            this.availableAdapters = new System.Windows.Forms.Label();
            this.selectedAdapter = new System.Windows.Forms.TextBox();
            this.selectedAdapters = new System.Windows.Forms.Label();
            this.tabAssoOverview = new System.Windows.Forms.TabPage();
            this.splitContainerAssocOverview = new System.Windows.Forms.SplitContainer();
            this.buttonAssReq = new System.Windows.Forms.Button();
            this.richTextBoxReq = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAssocAcc = new System.Windows.Forms.Button();
            this.richTextBoxAccept = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.calledAETitle = new System.Windows.Forms.Label();
            this.callingAETitle = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.accIPAddressLabel = new System.Windows.Forms.Label();
            this.reqIPAddressLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPDUsOverview = new System.Windows.Forms.TabPage();
            this.splitContainerServiceElement = new System.Windows.Forms.SplitContainer();
            this.reqIPAddressSELabel = new System.Windows.Forms.Label();
            this.reqIPAddress = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.reqPduList = new System.Windows.Forms.ListBox();
            this.contextMenuSniffer = new System.Windows.Forms.ContextMenu();
            this.menuItemSaveDCM = new System.Windows.Forms.MenuItem();
            this.menuItemShowPDU = new System.Windows.Forms.MenuItem();
            this.menuItemShowResult = new System.Windows.Forms.MenuItem();
            this.menuItemShowPixelData = new System.Windows.Forms.MenuItem();
            this.accIPAddressSELabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.accIPAddress = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.accPduList = new System.Windows.Forms.ListBox();
            this.tabSummaryValidationResults = new System.Windows.Forms.TabPage();
            this.dvtkSummaryWebBrowserSniffer = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.tabDetailValidationResults = new System.Windows.Forms.TabPage();
            this.dvtkDetailWebBrowserSniffer = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.mainMenuDICOMSniffer = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemCapture = new System.Windows.Forms.MenuItem();
            this.menuItem_capopen = new System.Windows.Forms.MenuItem();
            this.menuItem_capaspdus = new System.Windows.Forms.MenuItem();
            this.menuItemConfig = new System.Windows.Forms.MenuItem();
            this.menuItem_configopen = new System.Windows.Forms.MenuItem();
            this.menuItem_configsave = new System.Windows.Forms.MenuItem();
            this.menuItemSaveResultsAs = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemMode = new System.Windows.Forms.MenuItem();
            this.menuItemCaptureMode = new System.Windows.Forms.MenuItem();
            this.menuItemAnalysisMode = new System.Windows.Forms.MenuItem();
            this.menuItemCap = new System.Windows.Forms.MenuItem();
            this.menuItemCapStart = new System.Windows.Forms.MenuItem();
            this.menuItemCapStop = new System.Windows.Forms.MenuItem();
            this.menuItemView = new System.Windows.Forms.MenuItem();
            this.menuItemEvaluateComm = new System.Windows.Forms.MenuItem();
            this.menuItemDetailValidation = new System.Windows.Forms.MenuItem();
            this.menuItemOptions = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.toolBarSniffer = new System.Windows.Forms.ToolBar();
            this.Mode = new System.Windows.Forms.ToolBarButton();
            this.CaptureButton = new System.Windows.Forms.ToolBarButton();
            this.ReadCapture = new System.Windows.Forms.ToolBarButton();
            this.SaveResultAs = new System.Windows.Forms.ToolBarButton();
            this.SaveConfig = new System.Windows.Forms.ToolBarButton();
            this.CleanUp = new System.Windows.Forms.ToolBarButton();
            this.Explore = new System.Windows.Forms.ToolBarButton();
            this.Filter = new System.Windows.Forms.ToolBarButton();
            this.toolBarSep = new System.Windows.Forms.ToolBarButton();
            this.Error = new System.Windows.Forms.ToolBarButton();
            this.Warning = new System.Windows.Forms.ToolBarButton();
            this.Backward = new System.Windows.Forms.ToolBarButton();
            this.Forward = new System.Windows.Forms.ToolBarButton();
            this.imageListBrowser = new System.Windows.Forms.ImageList(this.components);
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.statusBarSniffer = new System.Windows.Forms.StatusBar();
            this.progressBarStatusBar = new System.Windows.Forms.ProgressBar();
            this.tabControlSniffer.SuspendLayout();
            this.tabSnifferInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCapInfo)).BeginInit();
            this.splitContainerCapInfo.Panel1.SuspendLayout();
            this.splitContainerCapInfo.Panel2.SuspendLayout();
            this.splitContainerCapInfo.SuspendLayout();
            this.groupBoxCapture.SuspendLayout();
            this.groupBoxFilter.SuspendLayout();
            this.tabAssoOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAssocOverview)).BeginInit();
            this.splitContainerAssocOverview.Panel1.SuspendLayout();
            this.splitContainerAssocOverview.Panel2.SuspendLayout();
            this.splitContainerAssocOverview.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabPDUsOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerServiceElement)).BeginInit();
            this.splitContainerServiceElement.Panel1.SuspendLayout();
            this.splitContainerServiceElement.Panel2.SuspendLayout();
            this.splitContainerServiceElement.SuspendLayout();
            this.tabSummaryValidationResults.SuspendLayout();
            this.tabDetailValidationResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // tabControlSniffer
            // 
            this.tabControlSniffer.Controls.Add(this.tabSnifferInformation);
            this.tabControlSniffer.Controls.Add(this.tabAssoOverview);
            this.tabControlSniffer.Controls.Add(this.tabPDUsOverview);
            this.tabControlSniffer.Controls.Add(this.tabSummaryValidationResults);
            this.tabControlSniffer.Controls.Add(this.tabDetailValidationResults);
            this.tabControlSniffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSniffer.Location = new System.Drawing.Point(0, 28);
            this.tabControlSniffer.Name = "tabControlSniffer";
            this.tabControlSniffer.SelectedIndex = 0;
            this.tabControlSniffer.Size = new System.Drawing.Size(852, 554);
            this.tabControlSniffer.TabIndex = 21;
            this.tabControlSniffer.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabSnifferInformation
            // 
            this.tabSnifferInformation.AutoScroll = true;
            this.tabSnifferInformation.Controls.Add(this.splitContainerCapInfo);
            this.tabSnifferInformation.Location = new System.Drawing.Point(4, 25);
            this.tabSnifferInformation.Name = "tabSnifferInformation";
            this.tabSnifferInformation.Size = new System.Drawing.Size(844, 525);
            this.tabSnifferInformation.TabIndex = 0;
            this.tabSnifferInformation.Text = "Capture Information";
            this.tabSnifferInformation.UseVisualStyleBackColor = true;
            // 
            // splitContainerCapInfo
            // 
            this.splitContainerCapInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCapInfo.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCapInfo.Name = "splitContainerCapInfo";
            this.splitContainerCapInfo.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerCapInfo.Panel1
            // 
            this.splitContainerCapInfo.Panel1.Controls.Add(this.ipAddress1);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.ipAddress2);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.Portlabel);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.comboBoxConnections);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.connectionsLable);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.progressBarAnalysis);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.progressbarLabel);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.IP2Lable);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.IP1Lable);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.groupBoxCapture);
            this.splitContainerCapInfo.Panel1.Controls.Add(this.groupBoxFilter);
            // 
            // splitContainerCapInfo.Panel2
            // 
            this.splitContainerCapInfo.Panel2.Controls.Add(this.adapterList);
            this.splitContainerCapInfo.Panel2.Controls.Add(this.availableAdapters);
            this.splitContainerCapInfo.Panel2.Controls.Add(this.selectedAdapter);
            this.splitContainerCapInfo.Panel2.Controls.Add(this.selectedAdapters);
            this.splitContainerCapInfo.Size = new System.Drawing.Size(844, 525);
            this.splitContainerCapInfo.SplitterDistance = 256;
            this.splitContainerCapInfo.TabIndex = 31;
            // 
            // ipAddress1
            // 
            this.ipAddress1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddress1.BeepOnError = true;
            this.ipAddress1.Location = new System.Drawing.Point(700, 22);
            this.ipAddress1.Name = "ipAddress1";
            this.ipAddress1.Size = new System.Drawing.Size(120, 22);
            this.ipAddress1.TabIndex = 42;
            // 
            // ipAddress2
            // 
            this.ipAddress2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ipAddress2.BeepOnError = true;
            this.ipAddress2.Location = new System.Drawing.Point(700, 58);
            this.ipAddress2.Name = "ipAddress2";
            this.ipAddress2.Size = new System.Drawing.Size(120, 22);
            this.ipAddress2.TabIndex = 43;
            // 
            // Portlabel
            // 
            this.Portlabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Portlabel.AutoSize = true;
            this.Portlabel.Location = new System.Drawing.Point(651, 98);
            this.Portlabel.Name = "Portlabel";
            this.Portlabel.Size = new System.Drawing.Size(38, 17);
            this.Portlabel.TabIndex = 51;
            this.Portlabel.Text = "Port:";
            // 
            // comboBoxConnections
            // 
            this.comboBoxConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxConnections.Location = new System.Drawing.Point(7, 175);
            this.comboBoxConnections.Name = "comboBoxConnections";
            this.comboBoxConnections.Size = new System.Drawing.Size(506, 24);
            this.comboBoxConnections.TabIndex = 49;
            this.comboBoxConnections.DropDown += new System.EventHandler(this.comboBoxConnections_DropDown);
            this.comboBoxConnections.SelectedIndexChanged += new System.EventHandler(this.comboBoxConnections_SelectedIndexChanged);
            // 
            // connectionsLable
            // 
            this.connectionsLable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionsLable.Location = new System.Drawing.Point(4, 148);
            this.connectionsLable.Name = "connectionsLable";
            this.connectionsLable.Size = new System.Drawing.Size(184, 24);
            this.connectionsLable.TabIndex = 46;
            this.connectionsLable.Text = "Available DICOM Associations:";
            // 
            // progressBarAnalysis
            // 
            this.progressBarAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarAnalysis.Location = new System.Drawing.Point(7, 45);
            this.progressBarAnalysis.Name = "progressBarAnalysis";
            this.progressBarAnalysis.Size = new System.Drawing.Size(507, 24);
            this.progressBarAnalysis.TabIndex = 47;
            // 
            // progressbarLabel
            // 
            this.progressbarLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressbarLabel.Location = new System.Drawing.Point(4, 23);
            this.progressbarLabel.Name = "progressbarLabel";
            this.progressbarLabel.Size = new System.Drawing.Size(124, 19);
            this.progressbarLabel.TabIndex = 48;
            this.progressbarLabel.Text = "Analysis Progress:";
            // 
            // IP2Lable
            // 
            this.IP2Lable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IP2Lable.Location = new System.Drawing.Point(651, 59);
            this.IP2Lable.Name = "IP2Lable";
            this.IP2Lable.Size = new System.Drawing.Size(35, 21);
            this.IP2Lable.TabIndex = 45;
            this.IP2Lable.Text = "IP2:";
            // 
            // IP1Lable
            // 
            this.IP1Lable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IP1Lable.Location = new System.Drawing.Point(651, 25);
            this.IP1Lable.Name = "IP1Lable";
            this.IP1Lable.Size = new System.Drawing.Size(35, 21);
            this.IP1Lable.TabIndex = 44;
            this.IP1Lable.Text = "IP1:";
            // 
            // groupBoxCapture
            // 
            this.groupBoxCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCapture.Controls.Add(this.connection);
            this.groupBoxCapture.Controls.Add(this.connectionsLabel);
            this.groupBoxCapture.Controls.Add(this.capturePackets);
            this.groupBoxCapture.Controls.Add(this.capPacketLable);
            this.groupBoxCapture.Location = new System.Drawing.Point(631, 149);
            this.groupBoxCapture.Name = "groupBoxCapture";
            this.groupBoxCapture.Size = new System.Drawing.Size(204, 101);
            this.groupBoxCapture.TabIndex = 50;
            this.groupBoxCapture.TabStop = false;
            this.groupBoxCapture.Text = "Capture Progress";
            // 
            // connection
            // 
            this.connection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.connection.Location = new System.Drawing.Point(127, 66);
            this.connection.Name = "connection";
            this.connection.ReadOnly = true;
            this.connection.Size = new System.Drawing.Size(60, 22);
            this.connection.TabIndex = 1;
            // 
            // connectionsLabel
            // 
            this.connectionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionsLabel.Location = new System.Drawing.Point(10, 67);
            this.connectionsLabel.Name = "connectionsLabel";
            this.connectionsLabel.Size = new System.Drawing.Size(124, 18);
            this.connectionsLabel.TabIndex = 0;
            this.connectionsLabel.Text = "Open Connections:";
            // 
            // capturePackets
            // 
            this.capturePackets.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.capturePackets.Location = new System.Drawing.Point(127, 24);
            this.capturePackets.Name = "capturePackets";
            this.capturePackets.ReadOnly = true;
            this.capturePackets.Size = new System.Drawing.Size(60, 22);
            this.capturePackets.TabIndex = 30;
            // 
            // capPacketLable
            // 
            this.capPacketLable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.capPacketLable.Location = new System.Drawing.Point(10, 27);
            this.capPacketLable.Name = "capPacketLable";
            this.capPacketLable.Size = new System.Drawing.Size(124, 20);
            this.capPacketLable.TabIndex = 32;
            this.capPacketLable.Text = "Captured Packets:";
            // 
            // groupBoxFilter
            // 
            this.groupBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFilter.Controls.Add(this.Port);
            this.groupBoxFilter.Location = new System.Drawing.Point(633, 1);
            this.groupBoxFilter.Name = "groupBoxFilter";
            this.groupBoxFilter.Size = new System.Drawing.Size(205, 137);
            this.groupBoxFilter.TabIndex = 52;
            this.groupBoxFilter.TabStop = false;
            this.groupBoxFilter.Text = "Filter packets";
            // 
            // Port
            // 
            this.Port.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Port.BeepOnError = true;
            this.Port.Location = new System.Drawing.Point(67, 95);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(55, 22);
            this.Port.TabIndex = 2;
            // 
            // adapterList
            // 
            this.adapterList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.adapterList.ItemHeight = 16;
            this.adapterList.Location = new System.Drawing.Point(1, 113);
            this.adapterList.Name = "adapterList";
            this.adapterList.Size = new System.Drawing.Size(841, 148);
            this.adapterList.TabIndex = 24;
            this.adapterList.SelectedIndexChanged += new System.EventHandler(this.AdapterList_SelectedIndexChanged);
            // 
            // availableAdapters
            // 
            this.availableAdapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.availableAdapters.Location = new System.Drawing.Point(4, 90);
            this.availableAdapters.Name = "availableAdapters";
            this.availableAdapters.Size = new System.Drawing.Size(182, 15);
            this.availableAdapters.TabIndex = 26;
            this.availableAdapters.Text = "Available Network Adapters:";
            // 
            // selectedAdapter
            // 
            this.selectedAdapter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedAdapter.Location = new System.Drawing.Point(4, 42);
            this.selectedAdapter.Name = "selectedAdapter";
            this.selectedAdapter.ReadOnly = true;
            this.selectedAdapter.Size = new System.Drawing.Size(838, 22);
            this.selectedAdapter.TabIndex = 25;
            // 
            // selectedAdapters
            // 
            this.selectedAdapters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.selectedAdapters.Location = new System.Drawing.Point(4, 18);
            this.selectedAdapters.Name = "selectedAdapters";
            this.selectedAdapters.Size = new System.Drawing.Size(172, 15);
            this.selectedAdapters.TabIndex = 27;
            this.selectedAdapters.Text = "Selected Network Adapter:";
            // 
            // tabAssoOverview
            // 
            this.tabAssoOverview.Controls.Add(this.splitContainerAssocOverview);
            this.tabAssoOverview.Controls.Add(this.panel7);
            this.tabAssoOverview.Location = new System.Drawing.Point(4, 25);
            this.tabAssoOverview.Name = "tabAssoOverview";
            this.tabAssoOverview.Size = new System.Drawing.Size(844, 525);
            this.tabAssoOverview.TabIndex = 1;
            this.tabAssoOverview.Text = "Association Overview";
            this.tabAssoOverview.UseVisualStyleBackColor = true;
            // 
            // splitContainerAssocOverview
            // 
            this.splitContainerAssocOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAssocOverview.Location = new System.Drawing.Point(0, 83);
            this.splitContainerAssocOverview.Name = "splitContainerAssocOverview";
            this.splitContainerAssocOverview.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerAssocOverview.Panel1
            // 
            this.splitContainerAssocOverview.Panel1.Controls.Add(this.buttonAssReq);
            this.splitContainerAssocOverview.Panel1.Controls.Add(this.richTextBoxReq);
            this.splitContainerAssocOverview.Panel1.Controls.Add(this.label1);
            // 
            // splitContainerAssocOverview.Panel2
            // 
            this.splitContainerAssocOverview.Panel2.Controls.Add(this.buttonAssocAcc);
            this.splitContainerAssocOverview.Panel2.Controls.Add(this.richTextBoxAccept);
            this.splitContainerAssocOverview.Panel2.Controls.Add(this.label4);
            this.splitContainerAssocOverview.Size = new System.Drawing.Size(844, 442);
            this.splitContainerAssocOverview.SplitterDistance = 219;
            this.splitContainerAssocOverview.TabIndex = 25;
            // 
            // buttonAssReq
            // 
            this.buttonAssReq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAssReq.Location = new System.Drawing.Point(706, 69);
            this.buttonAssReq.Name = "buttonAssReq";
            this.buttonAssReq.Size = new System.Drawing.Size(126, 39);
            this.buttonAssReq.TabIndex = 22;
            this.buttonAssReq.Text = "Show Assoc Req PDU";
            this.buttonAssReq.Click += new System.EventHandler(this.buttonAssReq_Click);
            // 
            // richTextBoxReq
            // 
            this.richTextBoxReq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxReq.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxReq.Location = new System.Drawing.Point(4, 33);
            this.richTextBoxReq.Name = "richTextBoxReq";
            this.richTextBoxReq.ReadOnly = true;
            this.richTextBoxReq.Size = new System.Drawing.Size(684, 183);
            this.richTextBoxReq.TabIndex = 21;
            this.richTextBoxReq.Text = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 19);
            this.label1.TabIndex = 20;
            this.label1.Text = "Requested Services:";
            // 
            // buttonAssocAcc
            // 
            this.buttonAssocAcc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAssocAcc.Location = new System.Drawing.Point(706, 87);
            this.buttonAssocAcc.Name = "buttonAssocAcc";
            this.buttonAssocAcc.Size = new System.Drawing.Size(126, 40);
            this.buttonAssocAcc.TabIndex = 23;
            this.buttonAssocAcc.Text = "Show Assoc Acc PDU";
            this.buttonAssocAcc.Click += new System.EventHandler(this.buttonAssocAcc_Click);
            // 
            // richTextBoxAccept
            // 
            this.richTextBoxAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxAccept.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxAccept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxAccept.Location = new System.Drawing.Point(4, 30);
            this.richTextBoxAccept.Name = "richTextBoxAccept";
            this.richTextBoxAccept.ReadOnly = true;
            this.richTextBoxAccept.Size = new System.Drawing.Size(684, 184);
            this.richTextBoxAccept.TabIndex = 22;
            this.richTextBoxAccept.Text = "";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(4, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 28);
            this.label4.TabIndex = 21;
            this.label4.Text = "Accepted Services:";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.calledAETitle);
            this.panel7.Controls.Add(this.callingAETitle);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.accIPAddressLabel);
            this.panel7.Controls.Add(this.reqIPAddressLabel);
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(844, 83);
            this.panel7.TabIndex = 24;
            // 
            // calledAETitle
            // 
            this.calledAETitle.Location = new System.Drawing.Point(83, 9);
            this.calledAETitle.Name = "calledAETitle";
            this.calledAETitle.Size = new System.Drawing.Size(253, 28);
            this.calledAETitle.TabIndex = 13;
            // 
            // callingAETitle
            // 
            this.callingAETitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.callingAETitle.Location = new System.Drawing.Point(506, 9);
            this.callingAETitle.Name = "callingAETitle";
            this.callingAETitle.Size = new System.Drawing.Size(314, 28);
            this.callingAETitle.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(4, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 19);
            this.label6.TabIndex = 11;
            this.label6.Text = "Requestor:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(422, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 19);
            this.label5.TabIndex = 12;
            this.label5.Text = "Acceptor:";
            // 
            // accIPAddressLabel
            // 
            this.accIPAddressLabel.Location = new System.Drawing.Point(506, 45);
            this.accIPAddressLabel.Name = "accIPAddressLabel";
            this.accIPAddressLabel.Size = new System.Drawing.Size(324, 27);
            this.accIPAddressLabel.TabIndex = 23;
            // 
            // reqIPAddressLabel
            // 
            this.reqIPAddressLabel.Location = new System.Drawing.Point(86, 45);
            this.reqIPAddressLabel.Name = "reqIPAddressLabel";
            this.reqIPAddressLabel.Size = new System.Drawing.Size(286, 27);
            this.reqIPAddressLabel.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(422, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 19);
            this.label3.TabIndex = 21;
            this.label3.Text = "IP Address:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 19);
            this.label2.TabIndex = 20;
            this.label2.Text = "IP Address:";
            // 
            // tabPDUsOverview
            // 
            this.tabPDUsOverview.Controls.Add(this.splitContainerServiceElement);
            this.tabPDUsOverview.Location = new System.Drawing.Point(4, 25);
            this.tabPDUsOverview.Name = "tabPDUsOverview";
            this.tabPDUsOverview.Size = new System.Drawing.Size(844, 525);
            this.tabPDUsOverview.TabIndex = 2;
            this.tabPDUsOverview.Text = "Service Elements Overview";
            this.tabPDUsOverview.UseVisualStyleBackColor = true;
            // 
            // splitContainerServiceElement
            // 
            this.splitContainerServiceElement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerServiceElement.Location = new System.Drawing.Point(0, 0);
            this.splitContainerServiceElement.Name = "splitContainerServiceElement";
            // 
            // splitContainerServiceElement.Panel1
            // 
            this.splitContainerServiceElement.Panel1.Controls.Add(this.reqIPAddressSELabel);
            this.splitContainerServiceElement.Panel1.Controls.Add(this.reqIPAddress);
            this.splitContainerServiceElement.Panel1.Controls.Add(this.label8);
            this.splitContainerServiceElement.Panel1.Controls.Add(this.label12);
            this.splitContainerServiceElement.Panel1.Controls.Add(this.reqPduList);
            // 
            // splitContainerServiceElement.Panel2
            // 
            this.splitContainerServiceElement.Panel2.Controls.Add(this.accIPAddressSELabel);
            this.splitContainerServiceElement.Panel2.Controls.Add(this.label11);
            this.splitContainerServiceElement.Panel2.Controls.Add(this.accIPAddress);
            this.splitContainerServiceElement.Panel2.Controls.Add(this.label7);
            this.splitContainerServiceElement.Panel2.Controls.Add(this.accPduList);
            this.splitContainerServiceElement.Size = new System.Drawing.Size(844, 525);
            this.splitContainerServiceElement.SplitterDistance = 401;
            this.splitContainerServiceElement.TabIndex = 14;
            // 
            // reqIPAddressSELabel
            // 
            this.reqIPAddressSELabel.Location = new System.Drawing.Point(104, 58);
            this.reqIPAddressSELabel.Name = "reqIPAddressSELabel";
            this.reqIPAddressSELabel.Size = new System.Drawing.Size(258, 26);
            this.reqIPAddressSELabel.TabIndex = 30;
            // 
            // reqIPAddress
            // 
            this.reqIPAddress.Location = new System.Drawing.Point(104, 18);
            this.reqIPAddress.Name = "reqIPAddress";
            this.reqIPAddress.Size = new System.Drawing.Size(258, 28);
            this.reqIPAddress.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(10, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 18);
            this.label8.TabIndex = 27;
            this.label8.Text = "Requestor:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(13, 58);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 18);
            this.label12.TabIndex = 29;
            this.label12.Text = "IP Address:";
            // 
            // reqPduList
            // 
            this.reqPduList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reqPduList.ContextMenu = this.contextMenuSniffer;
            this.reqPduList.ItemHeight = 16;
            this.reqPduList.Location = new System.Drawing.Point(0, 96);
            this.reqPduList.Name = "reqPduList";
            this.reqPduList.Size = new System.Drawing.Size(397, 372);
            this.reqPduList.TabIndex = 11;
            this.reqPduList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.reqPduList_MouseDown);
            // 
            // contextMenuSniffer
            // 
            this.contextMenuSniffer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSaveDCM,
            this.menuItemShowPDU,
            this.menuItemShowResult,
            this.menuItemShowPixelData});
            // 
            // menuItemSaveDCM
            // 
            this.menuItemSaveDCM.Index = 0;
            this.menuItemSaveDCM.Text = "Save As DCM File....";
            this.menuItemSaveDCM.Click += new System.EventHandler(this.menuItemSaveDCM_Click);
            // 
            // menuItemShowPDU
            // 
            this.menuItemShowPDU.Index = 1;
            this.menuItemShowPDU.Text = "Show PDU/PDU List";
            this.menuItemShowPDU.Click += new System.EventHandler(this.menuItemShowPDU_Click);
            // 
            // menuItemShowResult
            // 
            this.menuItemShowResult.Index = 2;
            this.menuItemShowResult.Text = "Show Validation Result";
            this.menuItemShowResult.Visible = false;
            // 
            // menuItemShowPixelData
            // 
            this.menuItemShowPixelData.Index = 3;
            this.menuItemShowPixelData.Text = "Show Pixel Data";
            this.menuItemShowPixelData.Visible = false;
            this.menuItemShowPixelData.Click += new System.EventHandler(this.menuItemShowPixelData_Click);
            // 
            // accIPAddressSELabel
            // 
            this.accIPAddressSELabel.Location = new System.Drawing.Point(110, 58);
            this.accIPAddressSELabel.Name = "accIPAddressSELabel";
            this.accIPAddressSELabel.Size = new System.Drawing.Size(273, 26);
            this.accIPAddressSELabel.TabIndex = 31;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(22, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 18);
            this.label11.TabIndex = 30;
            this.label11.Text = "IP Address:";
            // 
            // accIPAddress
            // 
            this.accIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accIPAddress.Location = new System.Drawing.Point(110, 21);
            this.accIPAddress.Name = "accIPAddress";
            this.accIPAddress.Size = new System.Drawing.Size(297, 27);
            this.accIPAddress.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(20, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 18);
            this.label7.TabIndex = 28;
            this.label7.Text = "Acceptor:";
            // 
            // accPduList
            // 
            this.accPduList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accPduList.ContextMenu = this.contextMenuSniffer;
            this.accPduList.ItemHeight = 16;
            this.accPduList.Location = new System.Drawing.Point(4, 96);
            this.accPduList.Name = "accPduList";
            this.accPduList.Size = new System.Drawing.Size(436, 388);
            this.accPduList.TabIndex = 12;
            this.accPduList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.accPduList_MouseDown);
            // 
            // tabSummaryValidationResults
            // 
            this.tabSummaryValidationResults.Controls.Add(this.dvtkSummaryWebBrowserSniffer);
            this.tabSummaryValidationResults.Location = new System.Drawing.Point(4, 25);
            this.tabSummaryValidationResults.Name = "tabSummaryValidationResults";
            this.tabSummaryValidationResults.Size = new System.Drawing.Size(844, 525);
            this.tabSummaryValidationResults.TabIndex = 3;
            this.tabSummaryValidationResults.Text = "Summary Validation Results";
            this.tabSummaryValidationResults.UseVisualStyleBackColor = true;
            // 
            // dvtkSummaryWebBrowserSniffer
            // 
            this.dvtkSummaryWebBrowserSniffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvtkSummaryWebBrowserSniffer.Location = new System.Drawing.Point(0, 0);
            this.dvtkSummaryWebBrowserSniffer.Name = "dvtkSummaryWebBrowserSniffer";
            this.dvtkSummaryWebBrowserSniffer.Size = new System.Drawing.Size(844, 525);
            this.dvtkSummaryWebBrowserSniffer.TabIndex = 0;
            this.dvtkSummaryWebBrowserSniffer.XmlStyleSheetFullFileName = "";
            // 
            // tabDetailValidationResults
            // 
            this.tabDetailValidationResults.Controls.Add(this.dvtkDetailWebBrowserSniffer);
            this.tabDetailValidationResults.Location = new System.Drawing.Point(4, 25);
            this.tabDetailValidationResults.Name = "tabDetailValidationResults";
            this.tabDetailValidationResults.Size = new System.Drawing.Size(844, 525);
            this.tabDetailValidationResults.TabIndex = 4;
            this.tabDetailValidationResults.Text = "Detail Validation Results";
            this.tabDetailValidationResults.UseVisualStyleBackColor = true;
            // 
            // dvtkDetailWebBrowserSniffer
            // 
            this.dvtkDetailWebBrowserSniffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvtkDetailWebBrowserSniffer.Location = new System.Drawing.Point(0, 0);
            this.dvtkDetailWebBrowserSniffer.Name = "dvtkDetailWebBrowserSniffer";
            this.dvtkDetailWebBrowserSniffer.Size = new System.Drawing.Size(844, 525);
            this.dvtkDetailWebBrowserSniffer.TabIndex = 0;
            this.dvtkDetailWebBrowserSniffer.XmlStyleSheetFullFileName = "";
            // 
            // mainMenuDICOMSniffer
            // 
            this.mainMenuDICOMSniffer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemMode,
            this.menuItemCap,
            this.menuItemView,
            this.menuItemOptions,
            this.menuItemHelp});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCapture,
            this.menuItemConfig,
            this.menuItemSaveResultsAs,
            this.menuItemExit});
            this.menuItemFile.Text = "&File";
            // 
            // menuItemCapture
            // 
            this.menuItemCapture.Index = 0;
            this.menuItemCapture.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_capopen,
            this.menuItem_capaspdus});
            this.menuItemCapture.Text = "Capture File";
            // 
            // menuItem_capopen
            // 
            this.menuItem_capopen.Index = 0;
            this.menuItem_capopen.Text = "Load";
            this.menuItem_capopen.Click += new System.EventHandler(this.menuItem_capopen_Click);
            // 
            // menuItem_capaspdus
            // 
            this.menuItem_capaspdus.Index = 1;
            this.menuItem_capaspdus.Text = "Save in capture file";
            this.menuItem_capaspdus.Click += new System.EventHandler(this.menuItem_capaspdus_Click);
            // 
            // menuItemConfig
            // 
            this.menuItemConfig.Index = 1;
            this.menuItemConfig.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_configopen,
            this.menuItem_configsave});
            this.menuItemConfig.Text = "Setup File";
            // 
            // menuItem_configopen
            // 
            this.menuItem_configopen.Index = 0;
            this.menuItem_configopen.Text = "Load";
            this.menuItem_configopen.Click += new System.EventHandler(this.menuItem_configopen_Click);
            // 
            // menuItem_configsave
            // 
            this.menuItem_configsave.Index = 1;
            this.menuItem_configsave.Text = "Save";
            this.menuItem_configsave.Click += new System.EventHandler(this.menuItem_configsave_Click);
            // 
            // menuItemSaveResultsAs
            // 
            this.menuItemSaveResultsAs.Index = 2;
            this.menuItemSaveResultsAs.Text = "Save Results As...";
            this.menuItemSaveResultsAs.Click += new System.EventHandler(this.menuItemSaveResultsAs_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 3;
            this.menuItemExit.Text = "&Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemMode
            // 
            this.menuItemMode.Index = 1;
            this.menuItemMode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCaptureMode,
            this.menuItemAnalysisMode});
            this.menuItemMode.Text = "Mode";
            // 
            // menuItemCaptureMode
            // 
            this.menuItemCaptureMode.Checked = true;
            this.menuItemCaptureMode.Index = 0;
            this.menuItemCaptureMode.Text = "&Capture";
            this.menuItemCaptureMode.Click += new System.EventHandler(this.menuItemCaptureMode_Click);
            // 
            // menuItemAnalysisMode
            // 
            this.menuItemAnalysisMode.Index = 1;
            this.menuItemAnalysisMode.Text = "&Analysis";
            this.menuItemAnalysisMode.Click += new System.EventHandler(this.menuItemAnalysisMode_Click);
            // 
            // menuItemCap
            // 
            this.menuItemCap.Index = 2;
            this.menuItemCap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCapStart,
            this.menuItemCapStop});
            this.menuItemCap.Text = "&Capture";
            // 
            // menuItemCapStart
            // 
            this.menuItemCapStart.Index = 0;
            this.menuItemCapStart.Text = "&Start";
            this.menuItemCapStart.Click += new System.EventHandler(this.menuItemCapStart_Click);
            // 
            // menuItemCapStop
            // 
            this.menuItemCapStop.Enabled = false;
            this.menuItemCapStop.Index = 1;
            this.menuItemCapStop.Text = "&Stop";
            this.menuItemCapStop.Click += new System.EventHandler(this.menuItemCapStop_Click);
            // 
            // menuItemView
            // 
            this.menuItemView.Index = 3;
            this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEvaluateComm,
            this.menuItemDetailValidation});
            this.menuItemView.Text = "&Analysis";
            // 
            // menuItemEvaluateComm
            // 
            this.menuItemEvaluateComm.Index = 0;
            this.menuItemEvaluateComm.Text = "&Evaluate All DICOM Associations";
            this.menuItemEvaluateComm.Click += new System.EventHandler(this.menuItemEvaluateComm_Click);
            // 
            // menuItemDetailValidation
            // 
            this.menuItemDetailValidation.Checked = true;
            this.menuItemDetailValidation.Index = 1;
            this.menuItemDetailValidation.Text = "Generate Detailed Validation";
            this.menuItemDetailValidation.Click += new System.EventHandler(this.menuItemDetailValidation_Click);
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.Index = 4;
            this.menuItemOptions.Text = "&User Options";
            this.menuItemOptions.Click += new System.EventHandler(this.menuItemOptions_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 5;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "About DICOM Network Analyzer";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // toolBarSniffer
            // 
            this.toolBarSniffer.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarSniffer.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.Mode,
            this.CaptureButton,
            this.ReadCapture,
            this.SaveResultAs,
            this.SaveConfig,
            this.CleanUp,
            this.Explore,
            this.Filter,
            this.toolBarSep,
            this.Error,
            this.Warning,
            this.Backward,
            this.Forward});
            this.toolBarSniffer.DropDownArrows = true;
            this.toolBarSniffer.ImageList = this.imageListBrowser;
            this.toolBarSniffer.Location = new System.Drawing.Point(0, 0);
            this.toolBarSniffer.Name = "toolBarSniffer";
            this.toolBarSniffer.ShowToolTips = true;
            this.toolBarSniffer.Size = new System.Drawing.Size(852, 28);
            this.toolBarSniffer.TabIndex = 22;
            this.toolBarSniffer.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarSniffer_ButtonClick);
            // 
            // Mode
            // 
            this.Mode.ImageIndex = 10;
            this.Mode.Name = "Mode";
            this.Mode.ToolTipText = "Capture mode";
            // 
            // CaptureButton
            // 
            this.CaptureButton.ImageIndex = 8;
            this.CaptureButton.Name = "CaptureButton";
            this.CaptureButton.ToolTipText = "Start Capturing";
            // 
            // ReadCapture
            // 
            this.ReadCapture.ImageIndex = 5;
            this.ReadCapture.Name = "ReadCapture";
            this.ReadCapture.ToolTipText = "Read capture(.cap) file";
            // 
            // SaveResultAs
            // 
            this.SaveResultAs.ImageIndex = 6;
            this.SaveResultAs.Name = "SaveResultAs";
            this.SaveResultAs.ToolTipText = "Save Result As";
            // 
            // SaveConfig
            // 
            this.SaveConfig.ImageIndex = 7;
            this.SaveConfig.Name = "SaveConfig";
            this.SaveConfig.ToolTipText = "Save current configuration";
            // 
            // CleanUp
            // 
            this.CleanUp.ImageIndex = 12;
            this.CleanUp.Name = "CleanUp";
            this.CleanUp.Pushed = true;
            this.CleanUp.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.CleanUp.ToolTipText = "Auto Clean up enabled";
            // 
            // Explore
            // 
            this.Explore.ImageIndex = 13;
            this.Explore.Name = "Explore";
            this.Explore.ToolTipText = "Explore Saved DICOM Objects";
            // 
            // Filter
            // 
            this.Filter.ImageIndex = 14;
            this.Filter.Name = "Filter";
            this.Filter.Pushed = true;
            this.Filter.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.Filter.ToolTipText = "Filter TCP/IP packets";
            // 
            // toolBarSep
            // 
            this.toolBarSep.Name = "toolBarSep";
            this.toolBarSep.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // Error
            // 
            this.Error.Enabled = false;
            this.Error.ImageIndex = 3;
            this.Error.Name = "Error";
            this.Error.ToolTipText = "Navigate Error";
            // 
            // Warning
            // 
            this.Warning.Enabled = false;
            this.Warning.ImageIndex = 4;
            this.Warning.Name = "Warning";
            this.Warning.ToolTipText = "Navigate Warning";
            // 
            // Backward
            // 
            this.Backward.Enabled = false;
            this.Backward.ImageIndex = 0;
            this.Backward.Name = "Backward";
            this.Backward.ToolTipText = "Navigate Back";
            // 
            // Forward
            // 
            this.Forward.Enabled = false;
            this.Forward.ImageIndex = 1;
            this.Forward.Name = "Forward";
            this.Forward.ToolTipText = "Navigate Forward";
            // 
            // imageListBrowser
            // 
            this.imageListBrowser.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBrowser.ImageStream")));
            this.imageListBrowser.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBrowser.Images.SetKeyName(0, "");
            this.imageListBrowser.Images.SetKeyName(1, "");
            this.imageListBrowser.Images.SetKeyName(2, "");
            this.imageListBrowser.Images.SetKeyName(3, "");
            this.imageListBrowser.Images.SetKeyName(4, "");
            this.imageListBrowser.Images.SetKeyName(5, "");
            this.imageListBrowser.Images.SetKeyName(6, "");
            this.imageListBrowser.Images.SetKeyName(7, "");
            this.imageListBrowser.Images.SetKeyName(8, "");
            this.imageListBrowser.Images.SetKeyName(9, "");
            this.imageListBrowser.Images.SetKeyName(10, "");
            this.imageListBrowser.Images.SetKeyName(11, "");
            this.imageListBrowser.Images.SetKeyName(12, "");
            this.imageListBrowser.Images.SetKeyName(13, "");
            this.imageListBrowser.Images.SetKeyName(14, "filter.ico");
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "");
            this.ImageList.Images.SetKeyName(1, "");
            this.ImageList.Images.SetKeyName(2, "");
            this.ImageList.Images.SetKeyName(3, "");
            this.ImageList.Images.SetKeyName(4, "");
            this.ImageList.Images.SetKeyName(5, "");
            this.ImageList.Images.SetKeyName(6, "");
            this.ImageList.Images.SetKeyName(7, "");
            this.ImageList.Images.SetKeyName(8, "");
            this.ImageList.Images.SetKeyName(9, "");
            this.ImageList.Images.SetKeyName(10, "");
            this.ImageList.Images.SetKeyName(11, "");
            this.ImageList.Images.SetKeyName(12, "");
            this.ImageList.Images.SetKeyName(13, "");
            this.ImageList.Images.SetKeyName(14, "");
            this.ImageList.Images.SetKeyName(15, "");
            this.ImageList.Images.SetKeyName(16, "");
            this.ImageList.Images.SetKeyName(17, "");
            this.ImageList.Images.SetKeyName(18, "");
            this.ImageList.Images.SetKeyName(19, "");
            this.ImageList.Images.SetKeyName(20, "");
            this.ImageList.Images.SetKeyName(21, "");
            this.ImageList.Images.SetKeyName(22, "filter.ico");
            // 
            // statusBarSniffer
            // 
            this.statusBarSniffer.Location = new System.Drawing.Point(0, 582);
            this.statusBarSniffer.Name = "statusBarSniffer";
            this.statusBarSniffer.Size = new System.Drawing.Size(852, 23);
            this.statusBarSniffer.TabIndex = 23;
            // 
            // progressBarStatusBar
            // 
            this.progressBarStatusBar.Location = new System.Drawing.Point(367, 591);
            this.progressBarStatusBar.Name = "progressBarStatusBar";
            this.progressBarStatusBar.Size = new System.Drawing.Size(490, 18);
            this.progressBarStatusBar.TabIndex = 24;
            this.progressBarStatusBar.Visible = false;
            // 
            // DICOMSniffer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(852, 605);
            this.Controls.Add(this.progressBarStatusBar);
            this.Controls.Add(this.tabControlSniffer);
            this.Controls.Add(this.toolBarSniffer);
            this.Controls.Add(this.statusBarSniffer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenuDICOMSniffer;
            this.MinimumSize = new System.Drawing.Size(870, 652);
            this.Name = "DICOMSniffer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DICOM Network Analyzer";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DICOMSniffer_Closing);
            this.Load += new System.EventHandler(this.DICOMSniffer_Load);
            this.tabControlSniffer.ResumeLayout(false);
            this.tabSnifferInformation.ResumeLayout(false);
            this.splitContainerCapInfo.Panel1.ResumeLayout(false);
            this.splitContainerCapInfo.Panel1.PerformLayout();
            this.splitContainerCapInfo.Panel2.ResumeLayout(false);
            this.splitContainerCapInfo.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCapInfo)).EndInit();
            this.splitContainerCapInfo.ResumeLayout(false);
            this.groupBoxCapture.ResumeLayout(false);
            this.groupBoxCapture.PerformLayout();
            this.groupBoxFilter.ResumeLayout(false);
            this.groupBoxFilter.PerformLayout();
            this.tabAssoOverview.ResumeLayout(false);
            this.splitContainerAssocOverview.Panel1.ResumeLayout(false);
            this.splitContainerAssocOverview.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAssocOverview)).EndInit();
            this.splitContainerAssocOverview.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tabPDUsOverview.ResumeLayout(false);
            this.splitContainerServiceElement.Panel1.ResumeLayout(false);
            this.splitContainerServiceElement.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerServiceElement)).EndInit();
            this.splitContainerServiceElement.ResumeLayout(false);
            this.tabSummaryValidationResults.ResumeLayout(false);
            this.tabDetailValidationResults.ResumeLayout(false);
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

            // Initialize the Dvtk library
			Dvtk.Setup.Initialize();

            Application.Run(new DICOMSniffer());

			// Terminate the Dvtk library
			Dvtk.Setup.Terminate();

#if !DEBUG
			}
			catch(Exception exception)
			{
				CustomExceptionHandler.ShowThreadExceptionDialog(exception);
			}
#endif
		}

		/// <summary>
		/// Event Handler for loading of main DICOM Sniffer form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DICOMSniffer_Load(object sender, System.EventArgs e)
		{
			// Create DICOM analyzer object instance for analyzing all DICOM PDUs
			// and subscribe for the logging event.
            dicom = new DICOMAnalyser(this);
			dicom.Output += new DICOMAnalyser.OutputEvent(OutputHandler);

			// Create TCP layer object instance for parsing TCP/IP packets and filtering
			// DICOM packets and subscribe for the events passing the DICOM packets to 
			// upper DICOM layer
			tcp = new TCPParser();
			tcp.FragmentAdded += new TCPParser.FragmentAddedEvent(dicom.ReceiveTCPData);
			tcp.EndOfStream += new TCPParser.EndOfStreamEvent(dicom.EndOfStream);
			tcp.Error += new TCPParser.ErrorEvent(ErrorHandler);
			
			// Create low level sniffer object instance for sniffing TCP/IP packets
			// from the network and subscribe for the error event.
			sniffer = new SnifferObject();
			sniffer.Error += new Sniffer.ErrorEvent(ErrorHandler);
			sniffer.DataReceived += new Sniffer.DataReceivedEvent(tcp.AddPacket);

			try
			{
				// Get the list of network adapters(drivers) from the current machine
                string[] descriptions = sniffer.AdapterDescriptions;
                string[] names = sniffer.AdapterNames;

                for (int i = 0; i < descriptions.Length; i++)
                {
                    String desc = descriptions[i].Replace("\0", "");
                    String name = names[i].Replace("\0", "");
                    adapterList.Items.Add(name + " (" + desc + ")");
                }
			}
			catch (Exception except) 
			{
                string msg = string.Format("Error:{0}\n", except.Message);
                MessageBox.Show(this, "Please install the Npcap driver, which can be found on https://nmap.org/npcap/ .\n" + msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
				return;
			}			

			// Select the first adapter from the list by default
			if(adapterList.Items.Count != 0)
			{
				selectedAdapter.Text = adapterList.Items[0].ToString();
                adapterList.SelectedIndex = 0;
			}
			else
			{
                string msg = "No network adapter(driver) detected on the machine.\nPlease install the Npcap driver, which can be found on https://nmap.org/npcap/ .\r\n";
				MessageBox.Show(this, msg, "Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning );
				this.Close();
				return;
			}

			// Load the Dvtk Sniffer session and
            try
            {
                dvtkSnifferSession = SnifferSession.LoadFromFile(dataDirectory + @"\NetworkAnalyzer.ses");
                dvtkSnifferSession.ResultsRootDirectory = dataDirectory + @"\Results\";
                dvtkSnifferSession.DataDirectory = dataDirectory + @"\Dcm\";
                dvtkSnifferSession.SaveToFile();
    			
			    // Get the results directory
			    if(dvtkSnifferSession != null)
			    {
				    resultDirectoryname = dvtkSnifferSession.ResultsRootDirectory;
			    }			    
            }
            catch (Exception except)
            {
                string msg = string.Format("Error in loading Network Analyzer session file\r\n due to {0}",except.Message);
                MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

			//Get the IP Address of the current machine
            try
			{
			    string strHostName = Dns.GetHostName();
			    IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
			    IPAddress [] addr = ipEntry.AddressList;
			    if(addr != null)
			    {
                    foreach (IPAddress address in addr)
                    {
                        if (address.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString())
                        {
                            ipAddress1.Text = address.ToString();
                        }
                    }
			    }
            }
            catch (SocketException)
            {
                //Do nothing in case of no specific DNS configuration present
            }            

			//Default setting for all toolbar buttons & menuitems
			menuItemAnalysisMode.Checked = false;
			menuItem_capopen.Visible = false;
			ReadCapture.Visible = false;
			Explore.Visible = false;
			Error.Visible = false;
			Warning.Visible = false;
			Backward.Visible = false;
			Forward.Visible = false;
			menuItemView.Visible = false;
			menuItemShowResult.Enabled = false;
			tabControlSniffer.Controls.Remove(tabAssoOverview);
			tabControlSniffer.Controls.Remove(tabPDUsOverview);
			tabControlSniffer.Controls.Remove(tabSummaryValidationResults);
			tabControlSniffer.Controls.Remove(tabDetailValidationResults);

			menuItemConfig.Visible = true;
			menuItemSaveResultsAs.Visible = true;
			menuItem_capaspdus.Visible = true;
			menuItem_capaspdus.Enabled = false;
			menuItemCap.Visible = true;

			SaveConfig.Visible = true;
			SaveResultAs.Visible = true;
			CaptureButton.Visible = true;
		}

		#region Event Handlers for logging error and information (Publishers)
		public void OutputHandler(string s, bool Log, bool Screen)
		{
			if(Log)
				WriteToLog(s);

			if(Screen)
			{
				WriteToActivityLog(s);
			}
		}

		public void WriteToLog(string s)
		{
			if(textLog != null)
			{
				lock(textLog)
				{
					textLog.WriteLine(s);
					textLog.Flush();  // this is a low throughut log for monitoring
				}
			}
		}

		public void WriteToActivityLog(string s)
		{
			if(activityLog != null)
			{
				lock(activityLog)
				{
					activityLog.WriteLine(s);
					activityLog.Flush();
				}
			}
		}

		public void ErrorHandler(string s)
		{
			OutputHandler("****ERROR******\r\n" + s, true, true);
		}

//		private void OnActivityReportEvent(object sender, Dvtk.Events.ActivityReportEventArgs theArgs)
//		{
//			WriteToActivityLog(theArgs.Message + '\n');
//		}
		#endregion

		private void menuItemCapStop_Click(object sender, System.EventArgs e)
		{
			ActionCaptureStop();
		}

		private void menuItemCapStart_Click(object sender, System.EventArgs e)
		{
			ActionCaptureStart();
		}

		private void menuItemEvaluateComm_Click(object sender, System.EventArgs e)
		{
			evaluateAllAssociations = true;
			ActionEvaluateAllDICOMAssociations();
		}

		/// <summary>
		/// Event Handler for Start Capture button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ActionCaptureStart()
		{
            CaptureButton.ToolTipText = "Stop Capturing";
            CaptureButton.ImageIndex = 9;
			menuItemCapStart.Enabled = false;
			menuItemCapStop.Enabled = true;
            menuItemSaveResultsAs.Enabled = false;
            menuItem_capaspdus.Enabled = false;
            SaveResultAs.Enabled = false;
			Explore.Visible = false;
            menuItemOptions.Enabled = false;
			capturePackets.Text = "";
			this.Text = "DICOM Network Analyzer";

			//Create the directory for storing PDUs written in files
			CurrentBaseFileName = string.Format(@"{0}{1:yyyyMMdd_HHmmss}",resultDirectoryname,DateTime.Now);
			
            try
            {
                Directory.CreateDirectory(CurrentBaseFileName);
            }
			catch(System.UnauthorizedAccessException)
			{
				string msg = "Please check authorization and log-in with Administrator previlages.";
				MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return ;
			}

			CurrentBaseFileName += @"\";
			textLog = File.AppendText(CurrentBaseFileName + "Main.log");
			activityLogFileName = CurrentBaseFileName + "Activity.log";
			activityLog = File.AppendText(activityLogFileName);
			start = DateTime.Now;
			statusBarSniffer.Text = "Live capturing is in progress.....";

			// Clear the activity logging & connection list
			analysisDlg.ClearLogging();
			comboBoxConnections.Items.Clear();
			CleanUp.ToolTipText = "Auto Clean up enabled";
            if (!(UserOptions.retainOldAssociationsInfo ))
                tcp.listOfConnections.Clear();

			dicom.BaseFileName = CurrentBaseFileName;
			
			// Set the IP address and Port to TCP layer
            tcp.IP1 = ipAddress1.Text;
            tcp.IP2 = ipAddress2.Text;
            ipAddress1.ReadOnly = true;
            ipAddress2.ReadOnly = true;
            Port.ReadOnly = true;
            adapterList.Enabled = false;
            comboBoxConnections.Enabled = false;
            Filter.Enabled = false;
            SaveConfig.Enabled = false;
            Mode.Enabled = false;
            CleanUp.Enabled = false;
            menuItemConfig.Enabled = false;
            menuItemMode.Enabled = false;

            // Create the temp capture file
            fsTempCapFile = new FileStream(tempCapFile, FileMode.Create, FileAccess.ReadWrite);
            fsTempCapFile.Close();

            try
			{
                // Start the low level sniffer thread
                if (Filter.Pushed)
                {
                    sniffer.Filter = FilterString();
                    if (sniffer.Filter == "")
                    {
                        ActionCaptureStop();
                        return;
                    }
                }

				// Start the TCP parser thread
				tcp.TCPStartAnalyse();

				sniffer.Start();
                sniffer.StartDump(fsTempCapFile.Name);
			}
			catch (Exception except) 
			{
				string msg = string.Format("Error:{0}\n", except.Message);
				MessageBox.Show(this, msg, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error );
                
                ActionCaptureStop();
			}			
		}

		/// <summary>
		/// Event Handler for Stop Capture button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ActionCaptureStop()
		{
            CaptureButton.ToolTipText = "Start Capturing";
            CaptureButton.ImageIndex = 8;
			menuItemCapStart.Enabled = true;
			menuItemCapStop.Enabled = false;
			menuItem_capaspdus.Enabled = true;
			menuItemSaveResultsAs.Enabled = true;
			menuItemEvaluateComm.Enabled = true;
			SaveResultAs.Enabled = true;
			Explore.Visible = true;
            ipAddress1.ReadOnly = false;
            ipAddress2.ReadOnly = false;
            Port.ReadOnly = false;
            adapterList.Enabled = true;
            comboBoxConnections.Enabled = true;
            Filter.Enabled = true;
            SaveConfig.Enabled = true;
            Mode.Enabled = true;
            CleanUp.Enabled = true;
            menuItemConfig.Enabled = true;
            menuItemMode.Enabled = true;
            menuItemOptions.Enabled = true;
			statusBarSniffer.Text = "Stopped capturing";
            StopCapture();

            //For avoiding quick start/stop
            System.Threading.Thread.Sleep(500);
		}

		// Helper method
		private void StopCapture()
		{
            try
            {
                // Stop the low level sniffer
                sniffer.Stop();
                sniffer.StopDump();

                // Stop the TCP Analyzing
                tcp.TCPStopAnalyse();
            }
            catch (Exception except)
            {
                string msg = string.Format("Error:{0}\n", except.Message);
                MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

        private string FilterString()
        {
            string result = "ip and tcp";

            if (ipAddress1.Text != "")
            {
                result += " and host " + ipAddress1.Text;
            }

            if (ipAddress2.Text != "")
            {
                result += " and host " + ipAddress2.Text;
            }
            
            try
            {
                if (Port.Text != "")
                {
                    int port = int.Parse(Port.Text);
                    result += " and port " + Port.Text;
                }
            }
            catch (Exception except)
            {
                string msg = except.Message + "\nPlease specify valid Port.";
                MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return "";
            }

            return result;
        }

		/// <summary>
		/// Event Handler for closing of main DICOM Sniffer form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DICOMSniffer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(!captureFromFile)
			{
				StopCapture();
			}
			else
			{
				if(capFileSniffer != null)
					capFileSniffer.Stop();
			}

            if (fsTempCapFile != null)
            {
                fsTempCapFile.Dispose();
                fsTempCapFile = null;
            }

            if (activityLog != null)
            {
                lock (activityLog)
                {
                    activityLog.Close();
                    activityLog.Dispose();
                    activityLog = null;
                }
            }

            if (textLog != null)
            {
                lock (textLog)
                {
                    textLog.Close();
                    textLog.Dispose();
                    textLog = null;
                }
            }

            if (dicom != null)
            {
                foreach (object item in comboBoxConnections.Items)
                {
                    //Get the Association handle
                    string signature = item.ToString();
                    Association assocHandle = (Association)dicom.ConnectionsList[signature];

                    if (assocHandle != null)
                    {
                        assocHandle.Dispose(true);
                    }
                }
            }

			//Deleting temporary directories
			if(CleanUp.Pushed)
			{
				ArrayList theDirectoriesToRemove = new ArrayList();
				DirectoryInfo theResultDirectoryInfo = new DirectoryInfo(dataDirectory + @"\Results\");
				DirectoryInfo theDcmDirectoryInfo = new DirectoryInfo(dataDirectory + @"\Dcm\");
				DirectoryInfo[] theResultDirectoriesInfo = theResultDirectoryInfo.GetDirectories();
				DirectoryInfo[] theDcmDirectoriesInfo =  theDcmDirectoryInfo.GetDirectories();

				foreach (DirectoryInfo theDirInfo in theResultDirectoriesInfo)
				{
					theDirectoriesToRemove.Add(theDirInfo.FullName);
				}
				foreach (DirectoryInfo theDirInfo in theDcmDirectoriesInfo)
				{
					theDirectoriesToRemove.Add(theDirInfo.FullName);
				}

				//Delete all directories
				foreach(string theDirName in theDirectoriesToRemove)
				{
					if (Directory.Exists(theDirName))
					{
						try
						{
							Directory.Delete(theDirName,true);
						}
						catch(Exception)
						{
							//string theWarningText = string.Format("Could not be delete the {0} temporary directory.\n due to exception: {1}\n\n", theDirName, exception.Message);
							//MessageBox.Show(this, theWarningText, "Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning );
                            continue;
						}
					}				
				}
			}
		}

		/// <summary>
		/// Event Handler for sniffer timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer2_Tick(object sender, System.EventArgs e)
		{
			//Display the Nr of open DICOM connections
            connection.Text = tcp.NrOfConnections.ToString();

			//Display captured packets
			capturePackets.Text = tcp.CapturedPackets.ToString();
#if false
			if(!buttonStart.Enabled)
			{
				TimeSpan t = DateTime.Now-start;
				if(t.TotalMinutes > 5)
					StopCapture();
			}
#endif
		}

		/// <summary>
		/// Event Handler for selection of different items on the list box control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AdapterList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			sniffer.AdapterIndex = adapterList.SelectedIndex;
			selectedAdapter.Text = adapterList.SelectedItem.ToString();
		}

		/// <summary>
		/// Event Handler for selection of different tabs on the tab control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControlSniffer.SelectedTab == tabSnifferInformation) 
			{
				tabControlSniffer.SelectedTab = tabSnifferInformation;
				Error.Visible = false;
				Warning.Visible = false;
				Backward.Visible = false;
				Forward.Visible = false;
			}

			if (tabControlSniffer.SelectedTab == tabAssoOverview) 
			{
				tabControlSniffer.SelectedTab = tabAssoOverview;
				Error.Visible = false;
				Warning.Visible = false;
				Backward.Visible = false;
				Forward.Visible = false;
			}

			if (tabControlSniffer.SelectedTab == tabPDUsOverview) 
			{
				tabControlSniffer.SelectedTab = tabPDUsOverview;
				Error.Visible = false;
				Warning.Visible = false;
				Backward.Visible = false;
				Forward.Visible = false;
			}

			if (tabControlSniffer.SelectedTab == tabDetailValidationResults) 
			{
				tabControlSniffer.SelectedTab = tabDetailValidationResults;
				Error.Visible = true;
				Error.Enabled = this.dvtkDetailWebBrowserSniffer.ContainsErrors;
				Warning.Visible = true;
				Warning.Enabled = this.dvtkDetailWebBrowserSniffer.ContainsWarnings;
				Backward.Visible = true;
				Forward.Visible = true;
			}

			if (tabControlSniffer.SelectedTab == tabSummaryValidationResults) 
			{
				tabControlSniffer.SelectedTab = tabSummaryValidationResults;
				Error.Visible = true;
				Error.Enabled = this.dvtkSummaryWebBrowserSniffer.ContainsErrors;
				Warning.Visible = true;
				Warning.Enabled = this.dvtkSummaryWebBrowserSniffer.ContainsWarnings;
				Backward.Visible = true;
				Forward.Visible = true;
			}
		}

		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			DvtkApplicationLayer.UserInterfaces.AboutForm   about = 
				new DvtkApplicationLayer.UserInterfaces.AboutForm("DICOM Network Analyzer");
			about.InfoToDisplay = "Credits:\nDICOM Validation Toolkit  -  www.dvtk.org\n" +
				"Dave Harvey (Medical Connections) -  www.medicalconnections.co.uk\n" +
                "Victor Tan (victor@gmail.com)";
			about.ShowDialog();
		}

		private void menuItemSaveResultsAs_Click(object sender, System.EventArgs e)
		{
            //menuItemSaveResultsAs.Enabled = false;
            //SaveResultAs.Enabled = false;
			FolderBrowserDialog resultDirSaveAsDlg = new FolderBrowserDialog();
			resultDirSaveAsDlg.Description = "Select the directory where the result files should be stored.";
			if (resultDirSaveAsDlg.ShowDialog (this) == DialogResult.OK) 
			{
				resultDirectoryname = resultDirSaveAsDlg.SelectedPath;
				resultDirectoryname += @"\";
				dvtkSnifferSession.ResultsRootDirectory = resultDirectoryname;

				string message =
					string.Format(
					"The results will be saved to {0} directory", 
					resultDirSaveAsDlg.SelectedPath);
				statusBarSniffer.Text = message;
			}
		}

		private void menuItem_configopen_Click(object sender, System.EventArgs e)
		{
			string setupFileName = "";
			OpenFileDialog theOpenFileDialog = new OpenFileDialog();

			theOpenFileDialog.Filter = "Setup files (*.setup) |*.setup";
			theOpenFileDialog.Multiselect = false;
			theOpenFileDialog.Title = "Load a setup file";

            if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                setupFileName = theOpenFileDialog.FileName;

                XmlTextReader reader = null;
                try
                {
                    reader = new XmlTextReader(setupFileName);
                    reader.ReadStartElement("SnifferConfiguration");

                    if(reader.IsStartElement())
                        ipAddress1.Text = reader.ReadElementString("IP1");

                    ipAddress2.Text = reader.ReadElementString("IP2");

                    Port.Text = reader.ReadElementString("Port");
                }
                catch (Exception ex)
                {
                    string msg =
                        string.Format(
                        "Failed to load setup file: {0} due to exception:{1}\n",
                        setupFileName,
                        ex.Message);
                    OutputHandler("Error: " + msg, true, true);
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
            }
		}

		private void menuItem_configsave_Click(object sender, System.EventArgs e)
		{
			string setupFileName = "";
			SaveFileDialog saveSetUpFileDlg = new SaveFileDialog();
			saveSetUpFileDlg.Title = "Save the current configuration";
			saveSetUpFileDlg.Filter = "DICOM Sniffer Setup files (*.setup) |*.setup";
			saveSetUpFileDlg.InitialDirectory = dataDirectory; //CurrentBaseFileName;
            if (saveSetUpFileDlg.ShowDialog() == DialogResult.OK)
            {
                setupFileName = saveSetUpFileDlg.FileName;

                XmlTextWriter writer = null;
                try
                {
                    writer = new XmlTextWriter(setupFileName, System.Text.Encoding.ASCII);
                    // The written .xml file will be more readable
                    writer.Formatting = Formatting.Indented;

                    // Start the document
                    writer.WriteStartDocument(true);

                    // Write the IP Address element containing ip addresses of both machines
                    writer.WriteStartElement("SnifferConfiguration");

                    writer.WriteElementString("IP1", ipAddress1.Text);
                    writer.WriteElementString("IP2", ipAddress2.Text);
                    writer.WriteElementString("Port", Port.Text);

                    // End the root element
                    writer.WriteEndElement();

                    // End the document
                    writer.WriteEndDocument();

                    string message =
                        string.Format(
                        "The current configuration is saved to {0}",
                        setupFileName);
                    statusBarSniffer.Text = message;
                }
                catch (Exception ex)
                {
                    string msg =
                        string.Format(
                        "Failed to write setup file: {0}. Due to exception:{1}\n",
                        setupFileName,
                        ex.Message);
                    OutputHandler("Error: " + msg, true, true);
                }
                finally
                {
                    if (writer != null) writer.Close();
                }
            }
		}
		
		private void menuItem_capopen_Click(object sender, System.EventArgs e)
		{
			string capFileName = "";
			captureFromFile = true;
			this.Text = "DICOM Network Analyzer";
			OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.Filter = "Capture files(*.cap)|*.cap|Pcap Capture files(*.pcap)|*.pcap";
			theOpenFileDialog.Multiselect = false;
			theOpenFileDialog.Title = "Load a capture file";

			if (theOpenFileDialog.ShowDialog () == DialogResult.OK)
			{
				capFileName = theOpenFileDialog.FileName;
                this.Text = string.Format("DICOM Network Analyzer- Reading {0}", capFileName);

				//Create the directory for storing PDUs written in files
				CurrentBaseFileName = string.Format(@"{0}{1:yyyyMMdd_HHmmss}",resultDirectoryname,DateTime.Now);
                try
                {
                    Directory.CreateDirectory(CurrentBaseFileName);
                }
                catch (System.UnauthorizedAccessException)
                {
                    string msg = "Please check authorization and log-in with Administrator previlages.";
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

				CurrentBaseFileName += @"\";
				textLog = File.AppendText(CurrentBaseFileName + "Main.log");
				start = DateTime.Now;
				
				activityLogFileName = CurrentBaseFileName + "Activity.log";
				activityLog = File.AppendText(activityLogFileName);

				// Clear the activity logging, tabs & connection list
				analysisDlg.ClearLogging();
				comboBoxConnections.Items.Clear();
				dicom.ConnectionsList.Clear();
                comboBoxConnections.Enabled = false;
                HideTabs();

				//Disable Read PDUs button
				ReadCapture.Enabled = false;
				menuItem_capopen.Enabled = false;
				Explore.Visible = true;
				//CleanUp.Enabled = true;
				CleanUp.ToolTipText = "Auto Clean up enabled";

				dicom.BaseFileName = CurrentBaseFileName;
			
				try
				{
					// Start the TCP parser thread
					tcp.TCPStartAnalyse();
					tcp.analysisFromCapFile = true;
					
					Cursor.Current = Cursors.WaitCursor;

					// Create WinPcap object instance for receieving TCP/IP packets
					// from the capture file and subscribe for the error event.
                    capFileSniffer = new WinPCapObject(this, capFileName, tcp);
                    //capFileSniffer = new SnifferObject(this, capFileName, tcp);
					capFileSniffer.Error += new Sniffer.ErrorEvent(ErrorHandler);
					capFileSniffer.DataReceived += new Sniffer.DataReceivedEvent(tcp.AddPacket);
					capFileSniffer.Start();
				}
				catch (Exception except)
				{
                    string msg = string.Format("Error:{0}\n", except.Message);
					MessageBox.Show(this, msg, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error );
					tcp.TCPStopAnalyse();
					menuItem_capopen.Enabled = true;
					ReadCapture.Enabled = true;
                    comboBoxConnections.Enabled = true;
				}				
			}			
		}

		private void menuItem_capaspdus_Click(object sender, System.EventArgs e)
		{
			//menuItem_capaspdus.Enabled = false;
			string capFileName = "";
			SaveFileDialog saveCapFileDlg = new SaveFileDialog();
			saveCapFileDlg.Title = "Save the captured packets";
            saveCapFileDlg.Filter = "Capture files(*.cap)|*.cap|Pcap Capture files(*.pcap)|*.pcap";
			if (saveCapFileDlg.ShowDialog () == DialogResult.OK)
			{
				capFileName = saveCapFileDlg.FileName;

                try
                {
                    if (File.Exists(tempCapFile))
                    {
                        File.Copy(tempCapFile, capFileName, true);
                        statusBarSniffer.Text = string.Format("The PDUs are saved successfully in {0}\n", capFileName);
                    }
                    else
                        statusBarSniffer.Text = string.Format("Error in saving the capture file.");
                }
			    catch (Exception except) 
			    {
				    string msg = string.Format("Exception:{0}\n", except.Message);
				    MessageBox.Show("Error in saving the PDUs.\n" +msg, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error );
			    }
			}
		}

		private void ActionEvaluateAllDICOMAssociations()
		{
			foreach(object item in comboBoxConnections.Items)
				connectionList.Add(item);

			//
			// Set the correct settings for the DicomThread.
			//
			String resultsFileBaseName = "DICOMNetworkAnalyzer";
			ThreadManager threadManager = new ThreadManager();

			HLIThread hliThread = new HLIThread(this,dvtkSnifferSession,connectionList);
			hliThread.Initialize(threadManager);
			hliThread.Options.ResultsDirectory = CurrentBaseFileName;
			hliThread.Options.Identifier = resultsFileBaseName;
			hliThread.Options.ResultsFileNameOnlyWithoutExtension = hliThread.Options.Identifier;

			// Set the progress bar to minimum
			//progressBarStatusBar.Visible = true;
			progressBarStatusBar.Minimum = 0;
            progressBarStatusBar.Maximum = 100;
			progressBarStatusBar.Step = 100/(connectionList.Count);

			statusBarSniffer.Text = "Please Wait......, Evaluation is in progress";
            Cursor.Current = Cursors.WaitCursor;

			//
			// Start the DicomThread.
			//
			hliThread.Start();
			
			hliThread.WaitForCompletion();

			// Set the progress bar to maximum
            progressBarStatusBar.Value = progressBarStatusBar.Maximum;

			statusBarSniffer.Text = "Evaluation finished";

            Cursor.Current = Cursors.Default;
			
			activityLog.Close();

			//Display results
			detailXmlFullFileName = hliThread.Options.DetailResultsFullFileName;
			analysisDlg.ShowResults(detailXmlFullFileName, activityLogFileName);
						
			menuItemEvaluateComm.Enabled = false;
			evaluateAllAssociations = false;

			if( captureFromFile && (capFileSniffer != null))
			{
				// Stop the low level sniffer
				capFileSniffer.Stop();				
			}			
		}

		private void ActionEvaluateSelectedDICOMCommunication()
		{
			ArrayList selectedConnection = new ArrayList();
			selectedConnection.Add(comboBoxConnections.SelectedItem);
			//
			// Set the correct settings for the DicomThread.
			//
            String resultsFileBaseName = "DICOMNetworkAnalyzer";
			ThreadManager threadManager = new ThreadManager();

			HLIThread hliThread = new HLIThread(this,dvtkSnifferSession,selectedConnection);
			hliThread.Initialize(threadManager);
			hliThread.Options.ResultsDirectory = CurrentBaseFileName;
			hliThread.Options.Identifier = resultsFileBaseName;
			hliThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
			hliThread.Options.LogChildThreadsOverview = false;
			hliThread.Options.LogThreadStartingAndStoppingInParent = false;
			hliThread.Options.LogWaitingForCompletionChildThreads  = false;

			// Set the progress bar to minimum
			progressBarStatusBar.Minimum = 0;
			progressBarStatusBar.Maximum = 100;
            progressBarStatusBar.Step = 10;

			//
			// Start the DicomThread.
			//
			hliThread.Start();

			hliThread.WaitForCompletion();
		
			// Set the progress bar to maximum
            progressBarStatusBar.Value = progressBarStatusBar.Maximum;
		
			//Display results
			dvtkSummaryWebBrowserSniffer.Navigate(summaryXmlFullFileName);

            if (generateDetailedValidation)
            {
                dvtkDetailWebBrowserSniffer.Navigate(detailXmlFullFileName);
            }

            if(menuItemAnalysisMode.Checked)
                DisplayTabs();
		}

        public void SetMinimumInProgressBar(int min)
		{
			// Sets the progress bar's minimum value to a number of PDUs.
            progressBarAnalysis.Minimum = min;
		}

        public void SetMaximumInProgressBar(int max)
		{
			// Sets the progress bar's maximum value to a number of PDUs.
            progressBarAnalysis.Maximum = max;
		}		

		public void PerformStepInProgressBar()
		{
            progressBarStatusBar.PerformStep();
		}

        public void IncrementStepInProgressBar(int step)
        {
            progressBarAnalysis.Increment(step);
        }

        public void AddConnectionToList(string connection)
        {
            comboBoxConnections.Items.Add(connection);
        }

        public void SelectConnectionFromList()
        {
            if (comboBoxConnections.Items.Count != 0)
            {
                comboBoxConnections.SelectedItem = comboBoxConnections.Items[0].ToString();

                //Get the first Association handle
			    string signature = comboBoxConnections.SelectedItem.ToString();
			    Association assocHandle = (Association) dicom.ConnectionsList[signature];

			    //Fill up the Association overview for selected association
                if (assocHandle != null)
                {
                    foreach (PDU_DETAIL pdu in assocHandle.PDUList)
                    {
                        if (pdu.PduType == 1)
                        {
                            startTimeForFirstAssoc = pdu.startTime;
                        }
                    }
                }
            }
        }

        public void UpdateUIControls()
        {
            menuItem_capopen.Enabled = true;
            ReadCapture.Enabled = true;
            Cursor.Current = Cursors.Default;
            progressBarAnalysis.Value = progressBarAnalysis.Maximum;
            menuItemEvaluateComm.Enabled = true;
            comboBoxConnections.Enabled = true;

            DisplayTabs();
        }

		private void toolBarSniffer_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if( e.Button == SaveResultAs)
			{					
				menuItemSaveResultsAs_Click( sender, null );
			}
			else if( e.Button == SaveConfig)
			{
				menuItem_configsave_Click( sender, null );
			}
			else if( e.Button == CaptureButton)
			{
				if(CaptureButton.ImageIndex == 8)
				{
					ActionCaptureStart();
				}
				else if(CaptureButton.ImageIndex == 9)
				{
					ActionCaptureStop();
				}
				else
				{
				}
			}
			else if( e.Button == ReadCapture)
			{
				menuItem_capopen_Click( sender, null );
			}
			else if( e.Button == Error)
			{
				if (tabControlSniffer.SelectedTab == tabSummaryValidationResults) 
				{	
					this.dvtkSummaryWebBrowserSniffer.FindNextText("Error:", true, true);
				}

				if (tabControlSniffer.SelectedTab == tabDetailValidationResults) 
				{	
					this.dvtkDetailWebBrowserSniffer.FindNextText("Error:", true, true);
				}
			}
			else if( e.Button == Warning)
			{
				if (tabControlSniffer.SelectedTab == tabSummaryValidationResults) 
				{
					this.dvtkSummaryWebBrowserSniffer.FindNextText("Warning:", true, true);
				}

				if (tabControlSniffer.SelectedTab == tabDetailValidationResults) 
				{
					this.dvtkDetailWebBrowserSniffer.FindNextText("Warning:", true, true);
				}
			}
			else if( e.Button == Backward)
			{
				if (tabControlSniffer.SelectedTab == tabSummaryValidationResults) 
				{
					this.dvtkSummaryWebBrowserSniffer.Back();
				}

				if (tabControlSniffer.SelectedTab == tabDetailValidationResults) 
				{
					this.dvtkDetailWebBrowserSniffer.Back();
				}
			}
			else if( e.Button == Forward)
			{
				if (tabControlSniffer.SelectedTab == tabSummaryValidationResults) 
				{
					this.dvtkSummaryWebBrowserSniffer.Forward();
				}

				if (tabControlSniffer.SelectedTab == tabDetailValidationResults) 
				{
					this.dvtkDetailWebBrowserSniffer.Forward();
				}
			}			
			else if( e.Button == CleanUp)
			{
                if(CleanUp.Pushed)
				    CleanUp.ToolTipText = "Auto Clean up enabled";
                else
                    CleanUp.ToolTipText = "Auto Clean up disabled";
				//CleanUp.Enabled = false;				
			}
			else if( e.Button == Explore)
			{
				Process process  = new Process();

				process.StartInfo.FileName= "Explorer.exe";
				process.StartInfo.Arguments = dvtkSnifferSession.DataDirectory;

				process.Start();
			}
			else if( e.Button == Mode)
			{
				if(Mode.ImageIndex == 10)
				{
                    Mode.ToolTipText = "Analysis Mode";
					Mode.ImageIndex = 11;
					menuItemCaptureMode_Click(sender, null);
				}
				else if(Mode.ImageIndex == 11)
				{
                    Mode.ToolTipText = "Capture Mode";
					Mode.ImageIndex = 10;
					menuItemAnalysisMode_Click(sender, null);
				}
				else
				{
				}
			}
            else if (e.Button == Filter)
            {
                if (Filter.Pushed)
                {
                    groupBoxFilter.Enabled = true;
                    ipAddress1.Enabled = true;
                    IP1Lable.Enabled = true;
                    ipAddress2.Enabled = true;
                    IP2Lable.Enabled = true;
                    Port.Enabled = true;
                    Portlabel.Enabled = true;
                }
                else
                {
                    groupBoxFilter.Enabled = false;
                    ipAddress1.Enabled = false;
                    IP1Lable.Enabled = false;
                    ipAddress2.Enabled = false;
                    IP2Lable.Enabled = false;
                    Port.Enabled = false;
                    Portlabel.Enabled = false;
                }
            }
			else
			{
			}
		}

		private void buttonAssReq_Click(object sender, System.EventArgs e)
		{
            if (assocHandle != null)
            {
                PDUOverview pdu = new PDUOverview(assocHandle.AssoRqPDUDetail, "A-ASSOCIATE-RQ", assocHandle.AssoRqPDULength);
                pdu.ShowDialog();
            }
		}

		private void buttonAssocAcc_Click(object sender, System.EventArgs e)
		{
            if (assocHandle != null)
            {
                PDUOverview pdu = new PDUOverview(assocHandle.AssoAcPDUDetail, "A-ASSOCIATE-AC", assocHandle.AssoAcPDULength);
                pdu.ShowDialog();
            }
		}

		private void comboBoxConnections_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            HideTabs();

			//Get the selected Association handle
			signature = comboBoxConnections.SelectedItem.ToString();
			comboBoxConnections.Select(0,signature.Length);
			assocHandle = (Association) dicom.ConnectionsList[signature];

			//Fill up the Association overview for selected association
            if (assocHandle != null)
            {
                callingAETitle.Text = assocHandle.CallingAETitle;
                calledAETitle.Text = assocHandle.CalledAETitle;
                richTextBoxReq.Text = assocHandle.RequestedPresentationContexts;
                richTextBoxAccept.Text = assocHandle.AcceptedPresentationContexts;

                reqIPAddress.Text = assocHandle.CalledAETitle;
                accIPAddress.Text = assocHandle.CallingAETitle;

                DisplayIPAddress(signature);
            }

			//Fill up the PDUs overview for selected association
			ClearLists();

            if (assocHandle != null)
            {
                foreach (PDU_DETAIL pdu in assocHandle.PDUList)
                {
                    PreparePDUList(pdu);
                }

                //Display validation result for selected association
                ActionEvaluateSelectedDICOMCommunication();
            }            
		}

		private void DisplayIPAddress(string assocSignature)
		{
			int index1 = assocSignature.IndexOf("(");
			int index2 = assocSignature.IndexOf(")",index1+1);
			reqIPAddressLabel.Text = assocSignature.Substring(0,index1) + ":" + assocSignature.Substring(index1+1,index2-index1-1);
			reqIPAddressSELabel.Text = reqIPAddressLabel.Text;

			index1 = assocSignature.IndexOf("(",index2+1);
			int index3 = assocSignature.IndexOf(")",index1+1);
			accIPAddressLabel.Text = assocSignature.Substring(index2+2,index1-index2-2) + ":" + assocSignature.Substring(index1+1,index3-index1-1);
			accIPAddressSELabel.Text = accIPAddressLabel.Text;							
		}
	
		private void ClearLists()
		{
			//Clear the list
			if(reqList.Count > 0)
			{
				reqPduList.Items.Clear();
				reqList.Clear();
			}
			if(accList.Count > 0)
			{
				accPduList.Items.Clear();
				accList.Clear();
			}
		}
		
		private string GetPDUString(PDU_DETAIL pdu)
		{
			string pduStr;
			switch(pdu.PduType)
			{
				case 1:
				{
					pduStr = DumpRequestPDU(pdu.PduData, 6, pdu.PduLength);
					break;
				}
				case 2:	
				{
					pduStr = DumpAcceptPDU(pdu.PduData, 6, pdu.PduLength);
					break;
				}
				case 3:
				{
					pduStr = DumpAssoRjPDU(pdu.PduData, 6, pdu.PduLength);
					break;
				}
				case 4:
				{
					pduStr = DumpDataPDU(pdu.PduData, 6, pdu.PduLength);
					break;
				}
				case 5:
				{
					pduStr = DumpAssoReleaseRqPDU(pdu.PduData, 6, pdu.PduLength);
					break;
				}
				case 6:
				{
					pduStr = DumpAssoReleaseRpPDU(pdu.PduData, 6, pdu.PduLength);
					break;
				}
				case 7:
				{
					pduStr = DumpAssoAbortPDU(pdu.PduData, 6, pdu.PduLength);
					break;
				}
				default:
				{
					pduStr = "P-UNKNOWN";
					break;
				}
			}
			return pduStr;
		}

		private string ConvertPduType(byte type)
		{
			string pduType;
			switch(type)
			{
				case 1:
				{
					pduType = "A-ASSOCIATE-RQ";
					break;
				}
				case 2:	
				{
					pduType = "A-ASSOCIATE-AC";
					break;
				}
				case 3:
				{
					pduType = "A-ASSOCIATE-RJ";
					break;
				}
				case 4:
				{
					pduType = "P-DATA-TF";
					break;
				}
				case 5:
				{
					pduType = "A-RELEASE-RQ";
					break;
				}
				case 6:
				{
					pduType = "A-RELEASE-RP";
					break;
				}
				case 7:
				{
					pduType = "A-ABORT";
					break;
				}
				default:
				{
					pduType = "P-UNKNOWN";
					break;
				}
			}
			return pduType;
		}

		/// <summary>
		/// Prepare PDU list for display
		/// </summary>
		/// <param name="pdu"></param>
		private void PreparePDUList(PDU_DETAIL pdu)
		{
			string displayStr = "";
            //TimeSpan ts = pdu.timeStamp - startTimeForFirstAssoc;
            //string timeStampStr = string.Format("{0:000}.{1:000}",(ts.Minutes *60 + ts.Seconds), ts.Milliseconds);
            int pduIndex = pdu.PduIndex + 1;
			switch(pdu.PduType)
			{
				case 1:
				{
                    displayStr = "[" + pduIndex.ToString() + "]" + " A-ASSOCIATE-RQ";
					reqPduList.Items.Add(displayStr);
					reqList.Add(pdu);
					break;
				}
				case 2:	
				{
                    displayStr = "[" + pduIndex.ToString() + "]" + " A-ASSOCIATE-AC";
					accPduList.Items.Add(displayStr);
					accList.Add(pdu);
					break;
				}
				case 3:
				{
                    displayStr = "[" + pduIndex.ToString() + "]" + " A-ASSOCIATE-RJ";
					if(pdu.PduDirection == 1)
					{
						accPduList.Items.Add(displayStr);
						accList.Add(pdu);
					}
					else
					{
						reqPduList.Items.Add(displayStr);
						reqList.Add(pdu);
					}
					break;
				}
				case 4:
				{
                    displayStr = "[" + pduIndex.ToString() + "] " + pdu.CmdType;
					if(pdu.CmdPdusList.Count > 1)
					{
                        //TimeSpan t1 = ((PDU_DETAIL)pdu.CmdPdusList[0]).timeStamp - startTimeForFirstAssoc;
                        //TimeSpan t2 = ((PDU_DETAIL)pdu.CmdPdusList[pdu.CmdPdusList.Count - 1]).timeStamp - startTimeForFirstAssoc;
                        //string t1Str = string.Format("{0:000}.{1:000000}", (t1.Minutes * 60 + t1.Seconds), t1.Milliseconds);
                        //string t2Str = string.Format("{0:000}.{1:000000}", (t2.Minutes * 60 + t2.Seconds), t2.Milliseconds);
                        int p1 = ((PDU_DETAIL)pdu.CmdPdusList[0]).PduIndex + 1;
                        int p2 = ((PDU_DETAIL)pdu.CmdPdusList[pdu.CmdPdusList.Count - 1]).PduIndex + 1;
                        displayStr = "[" + p1.ToString() + "-" + p2.ToString() + "] " + pdu.CmdType;
					}
					if(pdu.PduDirection == 1)
					{
						accPduList.Items.Add(displayStr);
						accList.Add(pdu);
					}
					else
					{
						reqPduList.Items.Add(displayStr);
						reqList.Add(pdu);
					}
					break;
				}
				case 5:
				{
                    displayStr = "[" + pduIndex.ToString() + "]" + " A-RELEASE-RQ";
					reqPduList.Items.Add(displayStr);
					reqList.Add(pdu);
					break;
				}
				case 6:
				{
                    displayStr = "[" + pduIndex.ToString() + "]" + " A-RELEASE-RP";
					accPduList.Items.Add(displayStr);
					accList.Add(pdu);
					break;
				}
				case 7:
				{
                    displayStr = "[" + pduIndex.ToString() + "]" + " A-ABORT";
					if(pdu.PduDirection == 1)
					{
						accPduList.Items.Add(displayStr);
						accList.Add(pdu);
					}
					else
					{
						reqPduList.Items.Add(displayStr);
						reqList.Add(pdu);
					}
					break;
				}
				default:
				{
                    displayStr = "[" + pduIndex.ToString() + "]" + " P-UNKNOWN";
					if(pdu.PduDirection == 1)
					{
						accPduList.Items.Add(displayStr);
						accList.Add(pdu);
					}
					else
					{
						reqPduList.Items.Add(displayStr);
						reqList.Add(pdu);
					}
					break;
				}
			}
		}

		/// <summary>
		/// This helper method for dumping Association Rq PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		private string DumpRequestPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder reqPduDetail = new StringBuilder(4096);
			uint index = Position;
			ushort version = DICOMUtility.Get2Bytes(p, ref index, true);
			Position +=4;
			string CallingAET="", CalledAET="";
			CallingAET = ASCII.GetString(p,(int)Position,16);
			CalledAET  = ASCII.GetString(p,(int)Position+16,16);
			
			Position +=64;

			reqPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "1H" + "\r\n");
			reqPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			reqPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
			reqPduDetail.Append(string.Format("  7-8" + "\t\t" + "Protocol Version" + "\t\t" + "{0}\r\n", version.ToString()));
			reqPduDetail.Append("  9-10" + "\t\t" + "Reserved" + "\t\t\t" + "00 00(not tested)" + "\r\n");
			reqPduDetail.Append(string.Format("  11-26" + "\t\t" + "Called AE Title" + "\t\t" + "{0}\r\n", CallingAET));
			reqPduDetail.Append(string.Format("  27-42" + "\t\t" + "Calling AE Title" + "\t\t" + "{0}\r\n", CalledAET));
			reqPduDetail.Append("  43-74" + "\t\t" + "Reserved" + "\t\t\t" + "00 00...(not tested)" + "\r\n\n");
			
			
			while(Position < length)
			{
				byte Type = p[Position++];
				Position++;
				
				uint indexLen = Position;
				ushort itemLen = DICOMUtility.Get2Bytes(p, ref indexLen, true);
				uint itemEnd = indexLen + itemLen;

				switch(Type)
				{
					case 0x10:
					{
						string applContextName = assocHandle.ReadShortString(p,ref Position);

						reqPduDetail.Append("\t\t" + "  Application Context Item  " + "\r\n\n");
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "10H" + "\r\n",indexLen-3));
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
						reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
						reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Application Context Name" + "\t" + "{2}\r\n\n", indexLen+1,itemEnd,applContextName));
						break;
					}

					case 0x20:
					{
						ushort Len = DICOMUtility.Get2Bytes(p, ref Position, true);
						uint End = Position + Len;

						reqPduDetail.Append("\t\t" + "  Requested Presentation Context Item  " + "\r\n\n");
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "20H" + "\r\n",indexLen-3));
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
						reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
												
						byte PCID = p[Position++];
						Position ++;
						byte result = p[Position++];
						string resultStr = (result == 0)?"Accepted":"Rejected";
						Position ++;

						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Presentation Context ID" + "\t" + "{1}\r\n", indexLen+1,PCID));
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen+2));
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen+3));
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n\n",indexLen+4));

						while(Position < End)
						{
							byte Type2 = p[Position++];
							Position++;

							uint indexSubItem = Position;
							ushort subItemLen = DICOMUtility.Get2Bytes(p, ref indexSubItem, true);
							uint subItemEnd = indexSubItem + subItemLen;

							string UID = assocHandle.ReadShortString(p,ref Position);

							if(Type2==0x30)
							{
								reqPduDetail.Append("\t\t" + "  SOP Class Sub-item  " + "\r\n\n");
								reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "30H" + "\r\n",indexSubItem-3));
								reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
								reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
								reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "SOP Class" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));								
							}
							
							//Transfer Syntax
							if(Type2==0x40)
							{
								reqPduDetail.Append("\t\t" + "  Transfer Syntax Sub-item  " + "\r\n\n");
								reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "40H" + "\r\n",indexSubItem-3));
								reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
								reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
								reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Transfer Syntax" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
							}							
						}
						break;
					}
					case 0x50:
					{
						ushort Len = DICOMUtility.Get2Bytes(p, ref Position, true);
						uint End = Position + Len;

						reqPduDetail.Append("\t\t" + "  User Information Item  " + "\r\n\n");
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "50H" + "\r\n",indexLen-3));
						reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
						reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexLen-1,indexLen,itemLen));
						
						while(Position < End)
						{
							byte Type2 = p[Position++];
							Position++;

							uint indexSubItem = Position;
							ushort subItemLen = DICOMUtility.Get2Bytes(p, ref indexSubItem, true);
							uint subItemEnd = indexSubItem + subItemLen;

							switch(Type2)
							{
								case 0x51: // Max PDU Length
									ushort len = DICOMUtility.Get2Bytes(p, ref Position, true);;
									uint MaxPDU = DICOMUtility.Get4Bytes(p, ref Position, true);
									reqPduDetail.Append("\t\t" + "  Max PDU Length Sub-item  " + "\r\n\n");
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "51H" + "\r\n",indexSubItem-3));
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Max PDU Length" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,MaxPDU));
									break;

								case 0x52:  // Name /UID

									string UID = assocHandle.ReadShortString(p,ref Position);
									reqPduDetail.Append("\t\t" + "  Implementation Class UID Sub-item  " + "\r\n\n");
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "52H" + "\r\n",indexSubItem-3));
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Class UID" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									break;

								case 0x54: // SCU SCP Role Selection

									ushort scuscplen = DICOMUtility.Get2Bytes(p, ref Position, true);
									ushort uidlen = DICOMUtility.Get2Bytes(p, ref Position, true);
									Position -= 2;
									UID = assocHandle.ReadShortString(p,ref Position);
									//Position += uidlen;
									byte scuRole = p[Position++];
									byte scpRole = p[Position++];
									string ReqScuSupport = "Support";
									string ReqScpSupport = "Non Support";
									
									if(scuRole == 0)
									{
										ReqScuSupport = "Non Support";
									}

									if(scpRole == 1)
									{
										ReqScpSupport = "Support";
									}

									reqPduDetail.Append("\t\t" + "  SCU/SCP Role Selection Sub-item  " + "\r\n\n");
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "54H" + "\r\n",indexSubItem-3));
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,scuscplen));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "UID Length" + "\t\t" + "{2}\r\n", indexSubItem+1,indexSubItem+2,uidlen));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "SOP Class UID" + "\t\t" + "{2}\r\n", indexSubItem+3,indexSubItem+2+uidlen,UID));
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCU Role" + "\t\t\t" + "{1}({2})\r\n", indexSubItem+3+uidlen,scuRole,ReqScuSupport));
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCP Role" + "\t\t\t" + "{1}({2})\r\n\n", indexSubItem+4+uidlen,scpRole,ReqScpSupport));
									break;

								case 0x55:

									UID = assocHandle.ReadShortString(p,ref Position);
									reqPduDetail.Append("\t\t" + "  Implementation Version Name Sub-item  " + "\r\n\n");
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "55H" + "\r\n",indexSubItem-3));
									reqPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									reqPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Version Name" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									break;

								default:
									ushort Len2 = DICOMUtility.Get2Bytes(p, ref Position, true);
									Position += Len2;
									break;
							}
						}
						break;
					}
				}
			}
			return reqPduDetail.ToString();
		}

		/// <summary>
		/// This helper method for dumping Association Rq PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		private string DumpAcceptPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder accPduDetail = new StringBuilder(4096);
			uint index = Position;
			ushort version = DICOMUtility.Get2Bytes(p, ref index, true);
			Position +=4;
			string CallingAET="", CalledAET="";
			CallingAET = ASCII.GetString(p,(int)Position,16);
			CalledAET  = ASCII.GetString(p,(int)Position+16,16);
			
			Position +=64;

			accPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "2H" + "\r\n");
			accPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			
			//Add for PDU header
			accPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
			accPduDetail.Append(string.Format("  7-8" + "\t\t" + "Protocol Version" + "\t\t" + "{0}\r\n", version.ToString()));
			accPduDetail.Append("  9-10" + "\t\t" + "Reserved" + "\t\t\t" + "00 00(not tested)" + "\r\n");
			accPduDetail.Append(string.Format("  11-26" + "\t\t" + "Called AE Title" + "\t\t" + "{0}\r\n", CallingAET));
			accPduDetail.Append(string.Format("  27-42" + "\t\t" + "Calling AE Title" + "\t\t" + "{0}\r\n", CalledAET));
			accPduDetail.Append("  43-74" + "\t\t" + "Reserved" + "\t\t\t" + "00 00...(not tested)" + "\r\n\n");

			while(Position < length)
			{
				byte Type = p[Position++];
				Position++;
				
				uint indexLen = Position;
				ushort itemLen = DICOMUtility.Get2Bytes(p, ref indexLen, true);
				uint itemEnd = indexLen + itemLen;

				switch(Type)
				{
					case 0x10:
					{
						string applContextName = assocHandle.ReadShortString(p,ref Position);

						accPduDetail.Append("\t\t" + "  Application Context Item  " + "\r\n\n");
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "10H" + "\r\n",indexLen-3));
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
						accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
						accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Application Context Name" + "\t" + "{2}\r\n\n", indexLen+1,itemEnd,applContextName));
						break;
					}

					case 0x20:
					case 0x21:
					{
						ushort Len = DICOMUtility.Get2Bytes(p, ref Position, true);
						uint End = Position + Len;

						accPduDetail.Append("\t\t" + "  Accepted Presentation Context Item  " + "\r\n\n");
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "21H" + "\r\n",indexLen-3));
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
						accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexLen-1,indexLen,itemLen));
						
						byte PCID = p[Position++];
						Position ++;
						byte result = p[Position++];
						string resultStr = (result == 0)?"Accepted":"Rejected";
						Position ++;

						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Presentation Context ID" + "\t" + "{1}\r\n", indexLen+1,PCID));
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen+2));
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Result/Reason" + "\t\t\t" + "{1}({2})\r\n",indexLen+3,result,resultStr));
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n\n",indexLen+4));
						
						while(Position < End)
						{
							byte Type2 = p[Position++];
							Position++;

							uint indexSubItem = Position;
							ushort subItemLen = DICOMUtility.Get2Bytes(p, ref indexSubItem, true);
							uint subItemEnd = indexSubItem + subItemLen;

							string UID = assocHandle.ReadShortString(p,ref Position);

							//Transfer Syntax
							if(Type2==0x40)
							{
								accPduDetail.Append("\t\t" + "  Transfer Syntax Sub-item  " + "\r\n\n");
								accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "40H" + "\r\n",indexSubItem-3));
								accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
								accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
								accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Transfer Syntax" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
							}							
						}
						break;
					}
					case 0x50:
					{
						ushort Len = DICOMUtility.Get2Bytes(p, ref Position, true);
						uint End = Position + Len;

						accPduDetail.Append("\t\t" + "  User Information Item  " + "\r\n\n");
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "50H" + "\r\n",indexLen-3));
						accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexLen-2));
						accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n\n", indexLen-1,indexLen,itemLen));
						
						while(Position < End)
						{
							byte Type2 = p[Position++];
							Position++;

							uint indexSubItem = Position;
							ushort subItemLen = DICOMUtility.Get2Bytes(p, ref indexSubItem, true);
							uint subItemEnd = indexSubItem + subItemLen;

							switch(Type2)
							{
								case 0x51: // Max PDU Length
									ushort len = DICOMUtility.Get2Bytes(p, ref Position, true);;
									uint MaxPDU = DICOMUtility.Get4Bytes(p, ref Position, true);
									accPduDetail.Append("\t\t" + "  Max PDU Length Sub-item  " + "\r\n\n");
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "51H" + "\r\n",indexSubItem-3));
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Max PDU Length" + "\t\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,MaxPDU));
									break;

								case 0x52:  // Name /UID

									string UID = assocHandle.ReadShortString(p,ref Position);
									accPduDetail.Append("\t\t" + "  Implementation Class UID Sub-item  " + "\r\n\n");
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "52H" + "\r\n",indexSubItem-3));
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Class UID" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									break;
								
								case 0x54: // SCU SCP Role Selection

									ushort scuscplen = DICOMUtility.Get2Bytes(p, ref Position, true);
									ushort uidlen = DICOMUtility.Get2Bytes(p, ref Position, true);
									Position -= 2;
									UID = assocHandle.ReadShortString(p,ref Position);
									//Position += uidlen;
									byte scuRole = p[Position++];
									byte scpRole = p[Position++];
									string AccScuSupport = "Accepted proposed role";
									string AccScpSupport = "Rejected proposed role";

									if(scuRole == 0)
									{
										AccScuSupport = "Rejected proposed role";
									}

									if(scpRole == 1)
									{
										AccScpSupport = "Accepted proposed role";
									}

									accPduDetail.Append("\t\t" + "  SCU/SCP Role Selection Sub-item  " + "\r\n\n");
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "54H" + "\r\n",indexSubItem-3));
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,scuscplen));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "UID Length" + "\t\t" + "{2}\r\n", indexSubItem+1,indexSubItem+2,uidlen));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "SOP Class UID" + "\t\t" + "{2}\r\n", indexSubItem+3,indexSubItem+2+uidlen,UID));
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCU Role" + "\t\t\t" + "{1}({2})\r\n", indexSubItem+3+uidlen,scuRole,AccScuSupport));
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "SCP Role" + "\t\t\t" + "{1}({2})\r\n\n", indexSubItem+4+uidlen,scpRole,AccScpSupport));
									break;

								case 0x55:

									UID = assocHandle.ReadShortString(p,ref Position);
									accPduDetail.Append("\t\t" + "  Implementation Version Name Sub-item  " + "\r\n\n");
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Item Type" + "\t\t\t" + "55H" + "\r\n",indexSubItem-3));
									accPduDetail.Append(string.Format("  {0}" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n",indexSubItem-2));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Item Length" + "\t\t" + "{2}\r\n", indexSubItem-1,indexSubItem,subItemLen));
									accPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Implementation Version Name" + "\t" + "{2}\r\n\n", indexSubItem+1,subItemEnd,UID));
									break;

								default:
									ushort Len2 = DICOMUtility.Get2Bytes(p, ref Position, true);
									Position += Len2;
									break;
							}
						}
						break;
					}
				}
			}
			return accPduDetail.ToString();
		}

		/// <summary>
		/// This helper method for dumping Association Reject PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		private string DumpAssoRjPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder rjPduDetail = new StringBuilder(1024);
			Position += 2;
			byte result = p[Position];
			byte source = p[Position++];
			byte reason = p[Position++];

			rjPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "3H" + "\r\n");
			rjPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			rjPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
			rjPduDetail.Append("  7" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			rjPduDetail.Append(string.Format("  8" + "\t\t" + "Result" + "\t\t\t" + "{0}({1})\r\n",result,MapResultOfAssoRj(result)));
			rjPduDetail.Append(string.Format("  9" + "\t\t" + "Source" + "\t\t\t" + "{0}({1})\r\n",source,MapSourceOfAssoRj(source)));
			rjPduDetail.Append(string.Format("  10" + "\t\t" + "Reason/Diag." + "\t\t\t" + "{0}({1})\r\n",reason,MapReasonOfAssoRj(source,reason)));
			return rjPduDetail.ToString();
		}

		private string MapResultOfAssoRj(byte result)
		{
			string resultStr = "";
			switch(result)
			{
				case 0:
				{
					resultStr = "Permanent Reject";
					break;
				}
				case 1:	
				{
					resultStr = "Transient Reject";
					break;
				}
				default:
				{
					resultStr = "Incorrect result value";
					break;
				}
			}
			return resultStr;
		}

		private string MapSourceOfAssoRj(byte source)
		{
			string sourceStr = "";
			switch(source)
			{
				case 0:
				{
					sourceStr = "Rejected by SCU";
					break;
				}
				case 1:	
				{
					sourceStr = "Rejected by SCP(ACSE related)";
					break;
				}
				case 2:
				{
					sourceStr = "Rejected by SCP(Presentation related)";
					break;
				}
				default:
				{
					sourceStr = "Incorrect source value";
					break;
				}
			}
			return sourceStr;
		}

		private string MapReasonOfAssoRj(byte source,byte reason)
		{
			string reasonStr = "";
			if(source == 0)
			{
				switch(reason)
				{
					case 0:
					{
						reasonStr = "SCU: No reason given";
						break;
					}
					case 1:	
					{
						reasonStr = "SCU: Application Context Name Not Supported";
						break;
					}
					case 2:
					{
						reasonStr = "SCU: Calling AE Title Not Recognized";
						break;
					}
					case 3:
					{
						reasonStr = "SCU: Reserved";
						break;
					}						
					case 6:
					{
						reasonStr = "SCU: Called AE Title Not Recognized";
						break;
					}
					case 4:
					case 5:
					case 7:
					case 8:
					case 9:
					{
						reasonStr = "SCU: Reserved";
						break;
					}
					default:
					{
						reasonStr = "SCU:Incorrect reason value";
						break;
					}
				}
			}
			else if(source == 1)
			{
				switch(reason)
				{
					case 0:
					{
						reasonStr = "SCP ACSE: No reason given";
						break;
					}
					case 1:	
					{
						reasonStr = "SCP ACSE: Protocol Version Not Supported";
						break;
					}
					default:
					{
						reasonStr = "SCP ACSE:Incorrect reason value";
						break;
					}
				}
			}
			else if(source == 2)
			{
				switch(reason)
				{
					case 0:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					{
						reasonStr = "SCP Presentation: Reserved";
						break;
					}
					case 1:	
					{
						reasonStr = "SCP Presentation: Temporary Congestion";
						break;
					}
					case 2:
					{
						reasonStr = "SCP Presentation: Local Limit Exceeded";
						break;
					}						
					default:
					{
						reasonStr = "SCP Presentation: Incorrect reason value";
						break;
					}
				}
			}
			else
			{
				reasonStr = "Incorrect reason value";
			}
			return reasonStr;
		}

		/// <summary>
		/// This helper method for dumping Association Release Rq PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		private string DumpAssoReleaseRqPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder releaseRqPduDetail = new StringBuilder(512);
			
			releaseRqPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "5H" + "\r\n");
			releaseRqPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			releaseRqPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
			releaseRqPduDetail.Append("  7-10" + "\t\t" + "Reserved" + "\t\t\t" + "00 00 00 00(not tested)" + "\r\n");
			return releaseRqPduDetail.ToString();
		}

		/// <summary>
		/// This helper method for dumping Association Release Rp PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		private string DumpAssoReleaseRpPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder releaseRpPduDetail = new StringBuilder(512);
			
			releaseRpPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "6H" + "\r\n");
			releaseRpPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			releaseRpPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
			releaseRpPduDetail.Append("  7-10" + "\t\t" + "Reserved" + "\t\t\t" + "00 00 00 00(not tested)" + "\r\n");
			return releaseRpPduDetail.ToString();
		}

		/// <summary>
		/// This helper method for dumping Association abort PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		private string DumpAssoAbortPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder abortPduDetail = new StringBuilder(1024);
			Position += 2;
			byte source = p[Position++];
			byte reason = p[Position++];

			abortPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "7H" + "\r\n");
			abortPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			abortPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n", length.ToString()));
			abortPduDetail.Append("  7" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			abortPduDetail.Append("  8" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			abortPduDetail.Append(string.Format("  9" + "\t\t" + "Source" + "\t\t\t" + "{0}({1})\r\n",source,MapSourceOfAbort(source)));
			abortPduDetail.Append(string.Format("  10" + "\t\t" + "Reason/Diag." + "\t\t" + "{0}({1})\r\n",reason,MapReasonOfAbort(reason)));
			return abortPduDetail.ToString();
		}

		private string MapSourceOfAbort(byte source)
		{
			string sourceStr = "";
			switch(source)
			{
				case 0:
				{
					sourceStr = "DICOM UL SCU initiated abort";
					break;
				}
				case 1:	
				{
					sourceStr = "Reserved";
					break;
				}
				case 2:
				{
					sourceStr = "DICOM UL SCP initiated abort";
					break;
				}				
				default:
				{
					sourceStr = "Incorrect source value";
					break;
				}
			}
			return sourceStr;
		}

		private string MapReasonOfAbort(byte reason)
		{
			string reasonStr = "";
			switch(reason)
			{
				case 0:
				{
					reasonStr = "Reason not specified";
					break;
				}
				case 1:	
				{
					reasonStr = "Unrecognized PDU";
					break;
				}
				case 2:
				{
					reasonStr = "Unexpected PDU";
					break;
				}
				case 3:
				{
					reasonStr = "Reserved";
					break;
				}
				case 4:
				{
					reasonStr = "Unrecognized PDU parameter";
					break;
				}
				case 5:
				{
					reasonStr = "Unexpected PDU parameter";
					break;
				}
				case 6:
				{
					reasonStr = "Invalid PDU parameter";
					break;
				}
				default:
				{
					reasonStr = "Incorrect reason value";
					break;
				}
			}
			return reasonStr;
		}

		/// <summary>
		/// This helper method for dumping P-DATA PDU
		/// </summary>
		/// <param name="p"></param>
		/// <param name="Position"></param>
		private string DumpDataPDU(byte[] p, uint Position, uint length)
		{
			StringBuilder pDataPduDetail = new StringBuilder(1024);
			
			pDataPduDetail.Append("  1" + "\t\t" + "PDU Type" + "\t\t" + "4H" + "\r\n");
			pDataPduDetail.Append("  2" + "\t\t" + "Reserved" + "\t\t\t" + "00(not tested)" + "\r\n");
			pDataPduDetail.Append(string.Format("  3-6" + "\t\t" + "PDU Length" + "\t\t" + "{0}\r\n\n", length.ToString()));
			
			while(Position < length)
			{
				uint PDVLen = DICOMUtility.Get4BytesBigEndian(p, ref Position);
				byte PCID = p[Position++];
				byte Flags = p[Position++];

                byte[] byteData = new byte[PDVLen - 2];
                Array.Copy(p, Position, byteData, 0, PDVLen - 2);

				string pdvType = (Flags & 0x01)>0?"Command PDV(Type 1)":"Data PDV(Type 0)";
				string pdvState = (Flags & 0x02)>0?"  Last Fragment":"  Data Continues...";

				pDataPduDetail.Append(string.Format("\t\t" + "  {0}  " + "\r\n\n", pdvType));
				pDataPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "PDV Length" + "\t\t" + "{2}\r\n", (Position-6+1),(Position-6+4),PDVLen));
				pDataPduDetail.Append(string.Format("  {0}" + "\t\t" + "Presentation Context ID" + "\t" + "{1}\r\n",(Position-6+5),PCID));
				pDataPduDetail.Append(string.Format("  {0}" + "\t\t" + "Message Control Header" + "\t" + "{1:X2}H\r\n",(Position-6+6),Flags));
				pDataPduDetail.Append(string.Format("  {0}-{1}" + "\t\t" + "Data:" + "\t" + "\r\n\n",(Position-6+7),(Position-6+PDVLen+4)));
                pDataPduDetail.Append(DICOMUtility.GetHexString(byteData, Position));
				pDataPduDetail.Append(string.Format("\t\t" + "  {0}  " + "\r\n\n", pdvState));
				Position += (PDVLen - 2);
			}
			
			return pDataPduDetail.ToString();
		}

		private void menuItemCaptureMode_Click(object sender, System.EventArgs e)
		{
			menuItemCaptureMode.Checked = !menuItemCaptureMode.Checked;			
			if(menuItemCaptureMode.Checked)
			{
				ControlsDisplayedInCaptureMode();
			}
			else
			{
				menuItemAnalysisMode.Checked = true;
				ControlsDisplayedInAnalysisMode();
			}
		}

		private void menuItemAnalysisMode_Click(object sender, System.EventArgs e)
		{
			menuItemAnalysisMode.Checked = !menuItemAnalysisMode.Checked;
			if(menuItemAnalysisMode.Checked)
			{
				ControlsDisplayedInAnalysisMode();
			}
			else
			{
				menuItemCaptureMode.Checked = true;
				ControlsDisplayedInCaptureMode();
			}
		}

		private void ControlsDisplayedInCaptureMode()
		{
			menuItemAnalysisMode.Checked = false;
			this.Text = "DICOM Network Analyzer";

            Mode.ToolTipText = "Capture Mode";
            Mode.ImageIndex = 10;

			//comboBoxConnections.Items.Clear();
			menuItemAnalysisMode.Checked = false;
			menuItem_capopen.Visible = false;
			ReadCapture.Visible = false;

			menuItemView.Visible = false;

            HideTabs();

			menuItemConfig.Visible = true;
			menuItemSaveResultsAs.Visible = true;
			menuItem_capaspdus.Visible = true;
			menuItemCap.Visible = true;

			SaveConfig.Visible = true;
			SaveResultAs.Visible = true;
			CaptureButton.Visible = true;

			IP1Lable.Visible = true;
			IP2Lable.Visible = true;
			ipAddress1.Visible = true;
			ipAddress2.Visible = true;
            Port.Visible = true;
            Portlabel.Visible = true;
            groupBoxFilter.Visible = true;
            Filter.Visible = true;

			connection.Visible = true;
			connectionsLabel.Visible = true;
			capturePackets.Visible = true;
			groupBoxCapture.Visible = true;
			capPacketLable.Visible = true;

			adapterList.Visible = true;
			selectedAdapter.Visible = true;
			selectedAdapters.Visible = true;
			availableAdapters.Visible = true;

			//this.MaximizeBox = false;
		}

        private void ControlsDisplayedInAnalysisMode()
		{
			menuItemCaptureMode.Checked = false;
			progressBarAnalysis.Value = progressBarAnalysis.Minimum;
			this.Text = "DICOM Network Analyzer";

            Mode.ToolTipText = "Analysis Mode";
            Mode.ImageIndex = 11;

			menuItemCaptureMode.Checked = false;
			menuItem_capopen.Visible = true;
			ReadCapture.Visible = true;

			menuItemView.Visible = true;

            DisplayTabs();

			menuItemConfig.Visible = false;
			menuItemSaveResultsAs.Visible = false;
			menuItem_capaspdus.Visible = false;
			menuItemCap.Visible = false;
			menuItemEvaluateComm.Enabled = false;

			SaveConfig.Visible = false;
			SaveResultAs.Visible = false;
			CaptureButton.Visible = false;

			IP1Lable.Visible = false;
			IP2Lable.Visible = false;
			ipAddress1.Visible = false;
			ipAddress2.Visible = false;
            Port.Visible = false;
            Portlabel.Visible = false;
            groupBoxFilter.Visible = false;
            Filter.Visible = false;

			connection.Visible = false;
			connectionsLabel.Visible = false;
			capturePackets.Visible = false;
			groupBoxCapture.Visible = false;
			capPacketLable.Visible = false;

			adapterList.Visible = false;
			selectedAdapter.Visible = false;
			selectedAdapters.Visible = false;
			availableAdapters.Visible = false;

			//this.MaximizeBox = true;
		}

        private void HideTabs()
        {
            if (tabControlSniffer.Contains(tabAssoOverview))
            {
                tabControlSniffer.Controls.Remove(tabAssoOverview);
            }

            if (tabControlSniffer.Contains(tabPDUsOverview))
            {
                tabControlSniffer.Controls.Remove(tabPDUsOverview);
            }

            if (tabControlSniffer.Contains(tabSummaryValidationResults))
            {
                tabControlSniffer.Controls.Remove(tabSummaryValidationResults);
            }

            if (tabControlSniffer.Contains(tabDetailValidationResults))
            {
                tabControlSniffer.Controls.Remove(tabDetailValidationResults);
            }
        }

        private void DisplayTabs()
        {
            if (!tabControlSniffer.Contains(tabAssoOverview))
            {
                tabControlSniffer.Controls.Add(tabAssoOverview);
            }

            if (!tabControlSniffer.Contains(tabPDUsOverview))
            {
                tabControlSniffer.Controls.Add(tabPDUsOverview);
            }

            if (!tabControlSniffer.Contains(tabSummaryValidationResults))
            {
                tabControlSniffer.Controls.Add(tabSummaryValidationResults);
            }

            if (generateDetailedValidation)
            {
                if (!tabControlSniffer.Contains(tabDetailValidationResults))
                {
                    tabControlSniffer.Controls.Add(tabDetailValidationResults);
                }
            }
        }

		private void dvtkSummaryWebBrowserSniffer_BackwardFormwardEnabledStateChangeEvent()
		{
			this.Backward.Enabled = this.dvtkSummaryWebBrowserSniffer.IsBackwardEnabled;
			this.Forward.Enabled = this.dvtkSummaryWebBrowserSniffer.IsForwardEnabled;
		}

		private void dvtkDetailWebBrowserSniffer_BackwardFormwardEnabledStateChangeEvent()
		{
			this.Backward.Enabled = this.dvtkDetailWebBrowserSniffer.IsBackwardEnabled;
			this.Forward.Enabled = this.dvtkDetailWebBrowserSniffer.IsForwardEnabled;
		}

		private void menuItemSaveDCM_Click(object sender, System.EventArgs e)
		{
			PDU_DETAIL pdu = null;
			if(reqPduList.SelectedIndex != -1)
			{
				pdu = (PDU_DETAIL)reqList[reqPduList.SelectedIndex];
			}

			if(accPduList.SelectedIndex != -1)
			{
				pdu = (PDU_DETAIL)accList[accPduList.SelectedIndex];
			}

			if((pdu != null) && (pdu.ByteDataDump.Count != 0))
			{
				string dcmFileName = "";
				SaveFileDialog saveDCMFileDlg = new SaveFileDialog();
				saveDCMFileDlg.Title = "Save the selected DICOM object as DCM File";
				saveDCMFileDlg.Filter = "DICOM files (*.dcm) |*.dcm";
				saveDCMFileDlg.InitialDirectory = CurrentBaseFileName;
                if (saveDCMFileDlg.ShowDialog() == DialogResult.OK)
                {
                    dcmFileName = saveDCMFileDlg.FileName;

                    uint DatasetLength = 0;
                    foreach (byte[] p in pdu.ByteDataDump)
                        DatasetLength += (uint)p.Length;

                    byte[] Bytes = new byte[DatasetLength];

                    DatasetLength = 0;
                    foreach (byte[] p in pdu.ByteDataDump)
                    {
                        p.CopyTo(Bytes, DatasetLength);
                        DatasetLength += (uint)p.Length;
                    }

                    assocHandle.DumpAsDCMFile(Bytes, pdu.TransferSyntaxDataset, dcmFileName);
                }
			}
			else
			{
				MessageBox.Show(this, "The data may contain command PDV or \nCorruption in DICOM object data", 
				"Error in saving the DCM File",MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}

		private void reqPduList_MouseDown(object sender, MouseEventArgs e)
		{
			accPduList.SelectedIndex = -1;
			System.Drawing.Point pt = new Point(e.X,e.Y);
			if(e.Button == MouseButtons.Right)
			{
                if (reqPduList.Items.Count > 0)
                {
                    int index = reqPduList.IndexFromPoint(pt);
                    if (index != -1)
                    {
                        menuItemShowPDU.Visible = true;
                        reqPduList.SetSelected(index, true);
                        if (reqPduList.SelectedItem.ToString().IndexOf("A-") != -1)
                        {
                            menuItemSaveDCM.Visible = false;
                            menuItemShowPixelData.Visible = false;
                        }
                        else
                        {
                            if ((reqPduList.SelectedItem.ToString().IndexOf("Data]") != -1))
                                menuItemSaveDCM.Visible = true;
                            else
                                menuItemSaveDCM.Visible = false;
                        }

                        if ((reqPduList.SelectedItem.ToString().IndexOf("C_STORE_RQ[Command,Data]") != -1) ||
                            (reqPduList.SelectedItem.ToString().IndexOf("C_STORE_RQ[Data]") != -1))
                            menuItemShowPixelData.Visible = true;
                    }
                    else
                    {
                        menuItemSaveDCM.Visible = false;
                        menuItemShowPDU.Visible = false;
                        menuItemShowPixelData.Visible = false;
                    }
                }
                else
                {
                    menuItemSaveDCM.Visible = false;
                    menuItemShowPDU.Visible = false;
                }
			}
		}

		private void accPduList_MouseDown(object sender, MouseEventArgs e)
		{
			reqPduList.SelectedIndex = -1;
            menuItemShowPixelData.Visible = false;
			System.Drawing.Point pt = new Point(e.X,e.Y);
			if(e.Button == MouseButtons.Right)
			{
                if (accPduList.Items.Count > 0)
                {
                    int index = accPduList.IndexFromPoint(pt);
                    if (index != -1)
                    {
                        menuItemShowPDU.Visible = true;
                        accPduList.SetSelected(index, true);
                        if (accPduList.SelectedItem.ToString().IndexOf("A-") != -1)
                        {
                            menuItemSaveDCM.Visible = false;
                        }
                        else
                        {
                            if ((accPduList.SelectedItem.ToString().IndexOf("Data]") != -1))
                                menuItemSaveDCM.Visible = true;
                            else
                                menuItemSaveDCM.Visible = false;
                        }
                    }
                    else
                    {
                        menuItemSaveDCM.Visible = false;
                        menuItemShowPDU.Visible = false;
                    }
                }
                else
                {
                    menuItemSaveDCM.Visible = false;
                    menuItemShowPDU.Visible = false;
                }
			}
		}

		private void menuItemShowPDU_Click(object sender, System.EventArgs e)
		{
			PDU_DETAIL pdu = null;
			if(reqPduList.SelectedIndex != -1)
				pdu = (PDU_DETAIL)reqList[reqPduList.SelectedIndex];
			else
				pdu = (PDU_DETAIL)accList[accPduList.SelectedIndex];

			string pduStr = "";
			if(pdu != null)
			{
				if(pdu.PduType == 4)
				{
					if(pdu.CmdPdusList.Count > 1)
					{
                        ServiceElementPDUs pdusDisplay = new ServiceElementPDUs(pdu, startTimeForFirstAssoc);
						pdusDisplay.ShowDialog();
					}
					else
					{
						pduStr = GetPDUString(pdu);
						PDUOverview pduOverview = new PDUOverview(pduStr, pdu.CmdType, pdu.PduLength.ToString());
						pduOverview.ShowDialog();
					}				
				}
				else
				{
					pduStr = GetPDUString(pdu);
					PDUOverview pduOverview = new PDUOverview(pduStr, ConvertPduType(pdu.PduType), pdu.PduLength.ToString());
					pduOverview.ShowDialog();
				}
			}
		}

		private void menuItemOptions_Click(object sender, System.EventArgs e)
		{
			UserOptions optionDlg = new UserOptions();
			optionDlg.ShowDialog();
		}

		private void menuItemShowPixelData_Click(object sender, System.EventArgs e)
		{
			bool ok = false;
			menuItemShowPixelData.Visible = false;

			//Save the selected PDU in a temp DCM file
			PDU_DETAIL pdu = null;
            string tempFile = dataDirectory + @"\temp.dcm";
			if(reqPduList.SelectedIndex != -1)
			{
				pdu = (PDU_DETAIL)reqList[reqPduList.SelectedIndex];
			}

			if((pdu != null) && (pdu.ByteDataDump.Count != 0))
			{
				uint DatasetLength = 0;
				foreach(byte[] p in pdu.ByteDataDump)
					DatasetLength += (uint)p.Length;

				byte[] Bytes = new byte[DatasetLength];

				DatasetLength = 0;
				foreach(byte[] p in pdu.ByteDataDump)
				{
					p.CopyTo(Bytes,DatasetLength);
					DatasetLength += (uint)p.Length;
				}

                try
                {
                    ok = assocHandle.DumpAsDCMFile(Bytes, pdu.TransferSyntaxDataset, tempFile);
                }
                catch (Exception)
                {
                    ok = false;
                }
			}

			//Start the process & display the temp DCM file to DICOM viewer application
			if(ok)
			{
				UserOptions optionDlg = new UserOptions();
				Process process  = new Process();

				process.StartInfo.FileName= optionDlg.DICOMViewerPath; 

				if (!File.Exists(process.StartInfo.FileName))
				{
					MessageBox.Show("DICOM Viewer not found.\nUnable to display pixel data.");
				}
				else
				{
                    try
                    {
                        process.StartInfo.Arguments = "\"" + tempFile + "\"";
                        process.Start();
                        process.WaitForExit();
                    }
                    catch (Exception except)
                    {
                        string msg = string.Format("The DICOM Viewer is not able to handle DICOM object,{0}\n", except.Message);
                        MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
				}
			}
			else
			{
				string msg = "Error in pixel data saving to a DCM file.\r\n";
				MessageBox.Show(this, msg, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error );
			}

			//Delete the temp file
			FileInfo tempfileInfo = new FileInfo(tempFile);
			if(tempfileInfo.Exists)
				tempfileInfo.Delete();
		}

        private void menuItemDetailValidation_Click(object sender, EventArgs e)
        {
            menuItemDetailValidation.Checked = !menuItemDetailValidation.Checked;
            if(menuItemDetailValidation.Checked)
                generateDetailedValidation = true;
            else
                generateDetailedValidation = false;
        }

        private void comboBoxConnections_DropDown(object sender, EventArgs e)
        {
             if (tcp.listOfConnections.Count != 0)
            {
                if (comboBoxConnections.Items.Count < tcp.listOfConnections.Count)
                {
                    for (int i = 0; i < tcp.listOfConnections.Count; i++)
                    {
                        string signature = tcp.listOfConnections[i];
                        Association assoc = (Association)dicom.ConnectionsList[signature];
                        if(assoc!=null)
                        {
                            if (!comboBoxConnections.Items.Contains(tcp.listOfConnections[i]))
                            {
                               comboBoxConnections.Items.Add(tcp.listOfConnections[i]);
                            }
                        }
                    }
                }
            }
        }

        private double computePhysicalMemoryAvailable()
        {
            double totalMemory=0;
            ManagementObjectSearcher Search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");
            foreach (ManagementObject Mobject in Search.Get())
            {
                totalMemory = (Convert.ToDouble(Mobject["TotalPhysicalMemory"]));

            }
            return totalMemory;
        }

        

            		
	}
}
