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
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Xml;

using Dvtk.Sessions;
using DvtkApplicationLayer;
using DvtkApplicationLayer.UserInterfaces;
using DvtkApplicationLayer.StoredFiles;

// Emulator based on HLI
using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.UserInterfaces;
// Emulator based on HLI

namespace StorageSCPEmulator
{
	using DvtkSession = Dvtk.Sessions;

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class StorageSCPEmulator : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemRun;
		private System.Windows.Forms.MenuItem menuItemStop;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.MenuItem menuItemAboutEmulator;
		private System.Windows.Forms.ImageList imageListStorageSCP;
		private System.Windows.Forms.ToolBar toolBarSCPEmulator;
		private System.Windows.Forms.ToolBarButton toolBarButtonRun;
		private System.Windows.Forms.ToolBarButton toolBarButtonError;
		private System.Windows.Forms.ToolBarButton toolBarButtonWarning;
		private System.Windows.Forms.ToolBarButton toolBarButtonLeft;
		private System.Windows.Forms.ToolBarButton toolBarButtonRight;
		private System.Windows.Forms.TabControl tabControlStorageSCP;
		private System.Windows.Forms.Panel panelSCPSettings;
		private System.Windows.Forms.TextBox textBoxSCPPort;
		private System.Windows.Forms.TextBox textBoxSCPAETitle;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabPage tabPageResults;
        private System.Windows.Forms.TabPage tabPageLogging;
        private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkWebBrowserSCPEmulator;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxImplVersion;
		private System.Windows.Forms.TextBox textBoxImplClassUID;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton toolBarButtonTS;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TabPage tabPageStorageSCPConfig;
		private System.Windows.Forms.TabPage tabPageCommitSCPConfig;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TextBox textBoxCommitSCPAETitle;
        private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textBoxCommitSCPPort;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown numericCommitSCP;
		private System.Windows.Forms.NumericUpDown numericStorageSCP;
		private System.Windows.Forms.TextBox textBoxStorageCommitPort;
		private System.Windows.Forms.TextBox textBoxStorageCommitIPAdd;
        private System.Windows.Forms.TextBox textBoxStorageCommitAETitle;
		private System.Windows.Forms.Label labelRemotePort;
		private System.Windows.Forms.Label labelRemoteIP;
		private System.Windows.Forms.Label labelRemoteAET;
		private System.Windows.Forms.TextBox textBoxDelay;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ToolBarButton toolBarButtonAbort;
		private System.Windows.Forms.Button buttonCommitEcho;
        private System.Windows.Forms.Button buttonCommitPing;
		private System.Windows.Forms.Label labelPing;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemLoad;
		private System.Windows.Forms.MenuItem menuItemSave;
		private System.Windows.Forms.MainMenu mainMenuStorageSCP;
		private System.Windows.Forms.Label labelEcho;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelMaxTlsVersion;
        private System.Windows.Forms.Label labelMinTlsVersion;
        private System.Windows.Forms.CheckBox checkBoxSecurity;
        private System.Windows.Forms.GroupBox groupBoxSecurity;
        private System.Windows.Forms.ComboBox comboboxMaxTlsVersion;
        private System.Windows.Forms.ComboBox comboboxMinTlsVersion;
        private System.Windows.Forms.Label labelCertificate;
        private System.Windows.Forms.TextBox textBoxCertificate;
        private System.Windows.Forms.Button buttonCertificate;
        private System.Windows.Forms.Label labelCredential;
        private System.Windows.Forms.TextBox textBoxCredential;
        private System.Windows.Forms.Button buttonCredential;
        private System.Windows.Forms.Label labelMaxTlsVersionCommit;
        private System.Windows.Forms.Label labelMinTlsVersionCommit;
        private System.Windows.Forms.GroupBox groupBoxSecurityCommit;
        private System.Windows.Forms.ComboBox comboboxMaxTlsVersionCommit;
        private System.Windows.Forms.ComboBox comboboxMinTlsVersionCommit;
        private System.Windows.Forms.Label labelCertificateCommit;
        private System.Windows.Forms.TextBox textBoxCertificateCommit;
        private System.Windows.Forms.Button buttonCertificateCommit;
        private System.Windows.Forms.Label labelCredentialCommit;
        private System.Windows.Forms.TextBox textBoxCredentialCommit;
        private System.Windows.Forms.Button buttonCredentialCommit;
        private System.Windows.Forms.CheckBox checkBoxSecurityCommit;
        private TabPage tabPageSopClass;
        private SopClassesUserControl sopClassesUserControlStoreSCP;
        private ToolBarButton toolBarButtonValidation;
        private Button buttonSpecifyTS;
        private Button buttonCommitTS;
        private MenuItem menuItemStoredFiles;
        private MenuItem menuItemStoredFilesOptions;
        private MenuItem menuItemStoredFilesExploreValidationResults;
        private MenuItem menuItemStoredFilesExploreReceivedDicomMessages;
        private MenuItem menuItem4;
        private MenuItem menuItemCreateDicomdirForReceivedDicomMessages;
        private MenuItem menuItem3;

		string applDirectory = Application.StartupPath;

		bool isStopped = true;
        uint eventDelay;

        DicomThread storageSCPDicomThread = null;
        DicomThread storageCommitSCPDicomThread = null;

        /// <summary>
        /// The options for Storage as SCP.
        /// </summary>
        private DicomThreadOptions storageOptions = null;
        private DicomThreadOptions storageCommitOptions = null;

        /// <summary>
        /// Parent thread of the other threads.
        /// </summary>
        private OverviewThread overviewThread = null;

        /// <summary>
        /// The ThreadManager of the latest constructed OverviewThread.
        /// </summary>
        private ThreadManager threadManager = null;

        /// <summary>
        /// The handler that handles thread state change events of the ThreadManager that is constructed for the Overview Thread.
        /// </summary>
        private ThreadManager.ThreadsStateChangeEventHandler threadsStateChangeEventHandler = null;

        /// <summary>
        /// Needed to be able to jump to the main results.
        /// </summary>
        private String topXmlResults = "";

        private ArrayList selectedTS = new ArrayList();
        private ArrayList selectedTSCommit = new ArrayList();

        private UserControlActivityLogging userControlActivityLogging;
                               
        private ArrayList selectedSops = new ArrayList();

        private FileGroups fileGroups = null;

        private ValidationResultsFileGroup validationResultsFileGroup = null;
        private MenuItem menuItemStoredFilesRemoveAllReceivedDicomMessages;

        private ReceivedDicomMessagesFileGroup receivedDicomMessagesFileGroup = null;
        private TextBox storageSCUAETitleBox;
        private Label label1;

        /// <summary>
        /// The handler that will be called when all threads have stopped.
        /// </summary>
        private delegate void ExecutionCompletedHandler();

        private string documentsRootPath;

		public StorageSCPEmulator()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            documentsRootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\Storage SCP Emulator";

			this.dvtkWebBrowserSCPEmulator.XmlStyleSheetFullFileName = applDirectory + "\\DVT_RESULTS.xslt";
            this.dvtkWebBrowserSCPEmulator.BackwardFormwardEnabledStateChangeEvent += new DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(dvtkWebBrowserSCPEmulator_BackwardFormwardEnabledStateChangeEvent);
            this.dvtkWebBrowserSCPEmulator.ErrorWarningEnabledStateChangeEvent += new DvtkWebBrowserNew.ErrorWarningEnabledStateChangeEventHandler(dvtkWebBrowserSCPEmulator_ErrorWarningEnabledStateChangeEvent);

            selectedTS.Add("1.2.840.10008.1.2");
            selectedTS.Add("1.2.840.10008.1.2.1");
            selectedTS.Add("1.2.840.10008.1.2.2");
            selectedTS.Add("1.2.840.10008.1.2.4.70");
            selectedTS.Add("1.2.840.10008.1.2.4.50");
            selectedTS.Add("1.2.840.10008.1.2.5");

            selectedSops.Add("1.2.840.10008.1.1");

            selectedTSCommit.Add("1.2.840.10008.1.2");
            selectedTSCommit.Add("1.2.840.10008.1.2.1");
            selectedTSCommit.Add("1.2.840.10008.1.2.2");

            //
            // Set the .Net thread name for debugging purposes.
            //
            System.Threading.Thread.CurrentThread.Name = "Storage SCP Emulator";

            //
            // The ThreadManager.
            //
            this.threadManager = new ThreadManager();

            //
            // Construct the Storage DicomOptions implicitly by constructing a DicomThread.
            //
            storageSCPDicomThread = new SCP();
            storageSCPDicomThread.Initialize(this.threadManager);
            this.storageOptions = storageSCPDicomThread.Options;
            this.storageOptions.LoadFromFile(Path.Combine(documentsRootPath, "StorageSCPEmulator.ses"));
            this.storageOptions.Identifier = "Storage SCP";
            this.storageOptions.AttachChildsToUserInterfaces = true;
            this.storageOptions.StorageMode = Dvtk.Sessions.StorageMode.AsMedia;
            this.storageOptions.LogThreadStartingAndStoppingInParent = false;
            this.storageOptions.LogWaitingForCompletionChildThreads = false;

            if (this.storageOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled == true)
            {
                checkBoxSecurity.Checked = true;
                groupBoxSecurity.Enabled = true;
            }
            else
            {
                checkBoxSecurity.Checked = false;
                groupBoxSecurity.Enabled = false;
            }
            switch (this.storageOptions.DvtkScriptSession.SecuritySettings.MaxTlsVersionFlags)
            {
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0:
                    comboboxMaxTlsVersion.SelectedIndex = 0;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1:
                    comboboxMaxTlsVersion.SelectedIndex = 1;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2:
                    comboboxMaxTlsVersion.SelectedIndex = 2;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3:
                    comboboxMaxTlsVersion.SelectedIndex = 3;
                    break;
            }
            switch (this.storageOptions.DvtkScriptSession.SecuritySettings.MinTlsVersionFlags)
            {
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0:
                    comboboxMinTlsVersion.SelectedIndex = 0;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1:
                    comboboxMinTlsVersion.SelectedIndex = 1;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2:
                    comboboxMinTlsVersion.SelectedIndex = 2;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3:
                    comboboxMinTlsVersion.SelectedIndex = 3;
                    break;
            }
            this.textBoxCertificate.Text = this.storageOptions.DvtkScriptSession.SecuritySettings.CertificateFileName;
            this.textBoxCredential.Text = this.storageOptions.DvtkScriptSession.SecuritySettings.CredentialsFileName;

            //
            // Construct the Storage DicomOptions implicitly by constructing a DicomThread.
            //
            storageCommitSCPDicomThread = new SCP();
            storageCommitSCPDicomThread.Initialize(this.threadManager);
            this.storageCommitOptions = storageCommitSCPDicomThread.Options;
            this.storageCommitOptions.LoadFromFile(Path.Combine(documentsRootPath, "CommitSCPEmulator.ses"));
            this.storageCommitOptions.Identifier = "Storage Commit SCP";
            this.storageCommitOptions.AttachChildsToUserInterfaces = true;
            this.storageCommitOptions.StorageMode = Dvtk.Sessions.StorageMode.NoStorage;
            this.storageCommitOptions.LogThreadStartingAndStoppingInParent = false;
            this.storageCommitOptions.LogWaitingForCompletionChildThreads = false;
            if (this.storageCommitOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled == true)
            {
                checkBoxSecurityCommit.Checked = true;
                groupBoxSecurityCommit.Enabled = true;
            }
            else
            {
                checkBoxSecurityCommit.Checked = false;
                groupBoxSecurityCommit.Enabled = false;
            }
            switch (this.storageCommitOptions.DvtkScriptSession.SecuritySettings.MaxTlsVersionFlags)
            {
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0:
                    comboboxMaxTlsVersionCommit.SelectedIndex = 0;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1:
                    comboboxMaxTlsVersionCommit.SelectedIndex = 1;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2:
                    comboboxMaxTlsVersionCommit.SelectedIndex = 2;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3:
                    comboboxMaxTlsVersionCommit.SelectedIndex = 3;
                    break;
            }
            switch (this.storageCommitOptions.DvtkScriptSession.SecuritySettings.MinTlsVersionFlags)
            {
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0:
                    comboboxMinTlsVersionCommit.SelectedIndex = 0;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1:
                    comboboxMinTlsVersionCommit.SelectedIndex = 1;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2:
                    comboboxMinTlsVersionCommit.SelectedIndex = 2;
                    break;
                case Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3:
                    comboboxMinTlsVersionCommit.SelectedIndex = 3;
                    break;
            }
            this.textBoxCertificateCommit.Text = this.storageCommitOptions.DvtkScriptSession.SecuritySettings.CertificateFileName;
            this.textBoxCredentialCommit.Text = this.storageCommitOptions.DvtkScriptSession.SecuritySettings.CredentialsFileName;

            this.threadsStateChangeEventHandler = new ThreadManager.ThreadsStateChangeEventHandler(this.HandleThreadsStateChangeEvent);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StorageSCPEmulator));
            this.mainMenuStorageSCP = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemRun = new System.Windows.Forms.MenuItem();
            this.menuItemStop = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemLoad = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFiles = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesExploreValidationResults = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesExploreReceivedDicomMessages = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemCreateDicomdirForReceivedDicomMessages = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesRemoveAllReceivedDicomMessages = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesOptions = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.menuItemAboutEmulator = new System.Windows.Forms.MenuItem();
            this.imageListStorageSCP = new System.Windows.Forms.ImageList(this.components);
            this.toolBarSCPEmulator = new System.Windows.Forms.ToolBar();
            this.toolBarButtonRun = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonAbort = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonTS = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonValidation = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonError = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonWarning = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonLeft = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonRight = new System.Windows.Forms.ToolBarButton();
            this.tabControlStorageSCP = new System.Windows.Forms.TabControl();
            this.tabPageStorageSCPConfig = new System.Windows.Forms.TabPage();
            this.panelSCPSettings = new System.Windows.Forms.Panel();
            this.storageSCUAETitleBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSpecifyTS = new System.Windows.Forms.Button();
            this.numericStorageSCP = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxImplVersion = new System.Windows.Forms.TextBox();
            this.textBoxImplClassUID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSCPPort = new System.Windows.Forms.TextBox();
            this.textBoxSCPAETitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageCommitSCPConfig = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelEcho = new System.Windows.Forms.Label();
            this.labelPing = new System.Windows.Forms.Label();
            this.buttonCommitEcho = new System.Windows.Forms.Button();
            this.buttonCommitPing = new System.Windows.Forms.Button();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxStorageCommitPort = new System.Windows.Forms.TextBox();
            this.textBoxStorageCommitIPAdd = new System.Windows.Forms.TextBox();
            this.textBoxStorageCommitAETitle = new System.Windows.Forms.TextBox();
            this.labelRemotePort = new System.Windows.Forms.Label();
            this.labelRemoteIP = new System.Windows.Forms.Label();
            this.numericCommitSCP = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxCommitSCPPort = new System.Windows.Forms.TextBox();
            this.textBoxCommitSCPAETitle = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCommitTS = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelRemoteAET = new System.Windows.Forms.Label();
            this.tabPageSopClass = new System.Windows.Forms.TabPage();
            this.sopClassesUserControlStoreSCP = new DvtkApplicationLayer.UserInterfaces.SopClassesUserControl();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.dvtkWebBrowserSCPEmulator = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.tabPageLogging = new System.Windows.Forms.TabPage();
            this.userControlActivityLogging = new DvtkHighLevelInterface.Common.UserInterfaces.UserControlActivityLogging();
            this.checkBoxSecurity = new System.Windows.Forms.CheckBox();
            this.labelMaxTlsVersion = new System.Windows.Forms.Label();
            this.labelMinTlsVersion = new System.Windows.Forms.Label();
            this.groupBoxSecurity = new System.Windows.Forms.GroupBox();
            this.comboboxMaxTlsVersion = new System.Windows.Forms.ComboBox();
            this.comboboxMinTlsVersion = new System.Windows.Forms.ComboBox();
            this.labelCertificate = new System.Windows.Forms.Label();
            this.textBoxCertificate = new System.Windows.Forms.TextBox();
            this.buttonCertificate = new System.Windows.Forms.Button();
            this.labelCredential = new System.Windows.Forms.Label();
            this.textBoxCredential = new System.Windows.Forms.TextBox();
            this.buttonCredential = new System.Windows.Forms.Button();
            this.checkBoxSecurityCommit = new System.Windows.Forms.CheckBox();
            this.labelMaxTlsVersionCommit = new System.Windows.Forms.Label();
            this.labelMinTlsVersionCommit = new System.Windows.Forms.Label();
            this.groupBoxSecurityCommit = new System.Windows.Forms.GroupBox();
            this.comboboxMaxTlsVersionCommit = new System.Windows.Forms.ComboBox();
            this.comboboxMinTlsVersionCommit = new System.Windows.Forms.ComboBox();
            this.labelCertificateCommit = new System.Windows.Forms.Label();
            this.textBoxCertificateCommit = new System.Windows.Forms.TextBox();
            this.buttonCertificateCommit = new System.Windows.Forms.Button();
            this.labelCredentialCommit = new System.Windows.Forms.Label();
            this.textBoxCredentialCommit = new System.Windows.Forms.TextBox();
            this.buttonCredentialCommit = new System.Windows.Forms.Button();
            this.tabControlStorageSCP.SuspendLayout();
            this.tabPageStorageSCPConfig.SuspendLayout();
            this.panelSCPSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericStorageSCP)).BeginInit();
            this.tabPageCommitSCPConfig.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCommitSCP)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageSopClass.SuspendLayout();
            this.tabPageResults.SuspendLayout();
            this.tabPageLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStorageSCP
            // 
            this.mainMenuStorageSCP.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemStoredFiles,
            this.menuItemAbout});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRun,
            this.menuItemStop,
            this.menuItem1,
            this.menuItemExit});
            this.menuItemFile.Text = "File";
            // 
            // menuItemRun
            // 
            this.menuItemRun.Index = 0;
            this.menuItemRun.Text = "Run Emulator";
            this.menuItemRun.Click += new System.EventHandler(this.menuItemRun_Click);
            // 
            // menuItemStop
            // 
            this.menuItemStop.Enabled = false;
            this.menuItemStop.Index = 1;
            this.menuItemStop.Text = "Stop Emulator";
            this.menuItemStop.Click += new System.EventHandler(this.menuItemStop_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLoad,
            this.menuItemSave});
            this.menuItem1.Text = "Config File";
            // 
            // menuItemLoad
            // 
            this.menuItemLoad.Index = 0;
            this.menuItemLoad.Text = "Load";
            this.menuItemLoad.Click += new System.EventHandler(this.menuItemLoad_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 1;
            this.menuItemSave.Text = "Save As..";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 3;
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemStoredFiles
            // 
            this.menuItemStoredFiles.Index = 1;
            this.menuItemStoredFiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemStoredFilesExploreValidationResults,
            this.menuItemStoredFilesExploreReceivedDicomMessages,
            this.menuItem4,
            this.menuItemCreateDicomdirForReceivedDicomMessages,
            this.menuItemStoredFilesRemoveAllReceivedDicomMessages,
            this.menuItem3,
            this.menuItemStoredFilesOptions});
            this.menuItemStoredFiles.Text = "Stored Files";
            // 
            // menuItemStoredFilesExploreValidationResults
            // 
            this.menuItemStoredFilesExploreValidationResults.Index = 0;
            this.menuItemStoredFilesExploreValidationResults.Text = "Explore Validation Results...";
            this.menuItemStoredFilesExploreValidationResults.Click += new System.EventHandler(this.menuItemStoredFilesExploreValidationResults_Click);
            // 
            // menuItemStoredFilesExploreReceivedDicomMessages
            // 
            this.menuItemStoredFilesExploreReceivedDicomMessages.Index = 1;
            this.menuItemStoredFilesExploreReceivedDicomMessages.Text = "Explore Received DICOM Messages...";
            this.menuItemStoredFilesExploreReceivedDicomMessages.Click += new System.EventHandler(this.menuItemStoredFilesExploreReceivedDicomMessages_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // menuItemCreateDicomdirForReceivedDicomMessages
            // 
            this.menuItemCreateDicomdirForReceivedDicomMessages.Index = 3;
            this.menuItemCreateDicomdirForReceivedDicomMessages.Text = "Create DICOMDIR for Received DICOM Messages";
            this.menuItemCreateDicomdirForReceivedDicomMessages.Click += new System.EventHandler(this.menuItemCreateDicomdirForReceivedDicomMessages_Click);
            // 
            // menuItemStoredFilesRemoveAllReceivedDicomMessages
            // 
            this.menuItemStoredFilesRemoveAllReceivedDicomMessages.Index = 4;
            this.menuItemStoredFilesRemoveAllReceivedDicomMessages.Text = "Remove all Received DICOM Messages";
            this.menuItemStoredFilesRemoveAllReceivedDicomMessages.Click += new System.EventHandler(this.menuItemStoredFilesRemoveAllReceivedDicomMessages_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 5;
            this.menuItem3.Text = "-";
            // 
            // menuItemStoredFilesOptions
            // 
            this.menuItemStoredFilesOptions.Index = 6;
            this.menuItemStoredFilesOptions.Text = "Options...";
            this.menuItemStoredFilesOptions.Click += new System.EventHandler(this.menuItemOptionsStoredFiles_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 2;
            this.menuItemAbout.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAboutEmulator});
            this.menuItemAbout.Text = "About";
            // 
            // menuItemAboutEmulator
            // 
            this.menuItemAboutEmulator.Index = 0;
            this.menuItemAboutEmulator.Text = "About Emulator";
            this.menuItemAboutEmulator.Click += new System.EventHandler(this.menuItemAboutEmulator_Click);
            // 
            // imageListStorageSCP
            // 
            this.imageListStorageSCP.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStorageSCP.ImageStream")));
            this.imageListStorageSCP.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStorageSCP.Images.SetKeyName(0, "");
            this.imageListStorageSCP.Images.SetKeyName(1, "");
            this.imageListStorageSCP.Images.SetKeyName(2, "");
            this.imageListStorageSCP.Images.SetKeyName(3, "");
            this.imageListStorageSCP.Images.SetKeyName(4, "");
            this.imageListStorageSCP.Images.SetKeyName(5, "");
            this.imageListStorageSCP.Images.SetKeyName(6, "");
            this.imageListStorageSCP.Images.SetKeyName(7, "");
            this.imageListStorageSCP.Images.SetKeyName(8, "");
            this.imageListStorageSCP.Images.SetKeyName(9, "");
            this.imageListStorageSCP.Images.SetKeyName(10, "validate.ico");
            // 
            // toolBarSCPEmulator
            // 
            this.toolBarSCPEmulator.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarSCPEmulator.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonRun,
            this.toolBarButtonAbort,
            this.toolBarButtonTS,
            this.toolBarButtonValidation,
            this.toolBarButton1,
            this.toolBarButtonError,
            this.toolBarButtonWarning,
            this.toolBarButtonLeft,
            this.toolBarButtonRight});
            this.toolBarSCPEmulator.ButtonSize = new System.Drawing.Size(39, 24);
            this.toolBarSCPEmulator.DropDownArrows = true;
            this.toolBarSCPEmulator.ImageList = this.imageListStorageSCP;
            this.toolBarSCPEmulator.Location = new System.Drawing.Point(0, 0);
            this.toolBarSCPEmulator.Name = "toolBarSCPEmulator";
            this.toolBarSCPEmulator.ShowToolTips = true;
            this.toolBarSCPEmulator.Size = new System.Drawing.Size(794, 28);
            this.toolBarSCPEmulator.TabIndex = 2;
            this.toolBarSCPEmulator.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarSCPEmulator_ButtonClick);
            // 
            // toolBarButtonRun
            // 
            this.toolBarButtonRun.ImageIndex = 0;
            this.toolBarButtonRun.Name = "toolBarButtonRun";
            this.toolBarButtonRun.ToolTipText = "Run Emulator";
            // 
            // toolBarButtonAbort
            // 
            this.toolBarButtonAbort.ImageIndex = 2;
            this.toolBarButtonAbort.Name = "toolBarButtonAbort";
            this.toolBarButtonAbort.ToolTipText = "Send Abort Rq";
            this.toolBarButtonAbort.Visible = false;
            // 
            // toolBarButtonTS
            // 
            this.toolBarButtonTS.ImageIndex = 3;
            this.toolBarButtonTS.Name = "toolBarButtonTS";
            this.toolBarButtonTS.ToolTipText = "Specify TS";
            this.toolBarButtonTS.Visible = false;
            // 
            // toolBarButtonValidation
            // 
            this.toolBarButtonValidation.ImageIndex = 10;
            this.toolBarButtonValidation.Name = "toolBarButtonValidation";
            this.toolBarButtonValidation.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButtonValidation.ToolTipText = "Validation enabled";
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonError
            // 
            this.toolBarButtonError.Enabled = false;
            this.toolBarButtonError.ImageIndex = 4;
            this.toolBarButtonError.Name = "toolBarButtonError";
            // 
            // toolBarButtonWarning
            // 
            this.toolBarButtonWarning.Enabled = false;
            this.toolBarButtonWarning.ImageIndex = 5;
            this.toolBarButtonWarning.Name = "toolBarButtonWarning";
            // 
            // toolBarButtonLeft
            // 
            this.toolBarButtonLeft.Enabled = false;
            this.toolBarButtonLeft.ImageIndex = 6;
            this.toolBarButtonLeft.Name = "toolBarButtonLeft";
            // 
            // toolBarButtonRight
            // 
            this.toolBarButtonRight.Enabled = false;
            this.toolBarButtonRight.ImageIndex = 7;
            this.toolBarButtonRight.Name = "toolBarButtonRight";
            // 
            // tabControlStorageSCP
            // 
            this.tabControlStorageSCP.Controls.Add(this.tabPageStorageSCPConfig);
            this.tabControlStorageSCP.Controls.Add(this.tabPageCommitSCPConfig);
            this.tabControlStorageSCP.Controls.Add(this.tabPageSopClass);
            this.tabControlStorageSCP.Controls.Add(this.tabPageResults);
            this.tabControlStorageSCP.Controls.Add(this.tabPageLogging);
            this.tabControlStorageSCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlStorageSCP.Location = new System.Drawing.Point(0, 28);
            this.tabControlStorageSCP.Name = "tabControlStorageSCP";
            this.tabControlStorageSCP.SelectedIndex = 0;
            this.tabControlStorageSCP.Size = new System.Drawing.Size(794, 545);
            this.tabControlStorageSCP.TabIndex = 3;
            this.tabControlStorageSCP.SelectedIndexChanged += new System.EventHandler(this.tabControlStorageSCP_SelectedIndexChanged);
            // 
            // tabPageStorageSCPConfig
            // 
            this.tabPageStorageSCPConfig.Controls.Add(this.panelSCPSettings);
            this.tabPageStorageSCPConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageStorageSCPConfig.Name = "tabPageStorageSCPConfig";
            this.tabPageStorageSCPConfig.Size = new System.Drawing.Size(786, 519);
            this.tabPageStorageSCPConfig.TabIndex = 0;
            this.tabPageStorageSCPConfig.Text = "Storage Config";
            this.tabPageStorageSCPConfig.UseVisualStyleBackColor = true;
            // 
            // panelSCPSettings
            // 
            this.panelSCPSettings.Controls.Add(this.storageSCUAETitleBox);
            this.panelSCPSettings.Controls.Add(this.label1);
            this.panelSCPSettings.Controls.Add(this.buttonSpecifyTS);
            this.panelSCPSettings.Controls.Add(this.numericStorageSCP);
            this.panelSCPSettings.Controls.Add(this.label7);
            this.panelSCPSettings.Controls.Add(this.textBoxImplVersion);
            this.panelSCPSettings.Controls.Add(this.textBoxImplClassUID);
            this.panelSCPSettings.Controls.Add(this.label3);
            this.panelSCPSettings.Controls.Add(this.label5);
            this.panelSCPSettings.Controls.Add(this.textBoxSCPPort);
            this.panelSCPSettings.Controls.Add(this.textBoxSCPAETitle);
            this.panelSCPSettings.Controls.Add(this.label4);
            this.panelSCPSettings.Controls.Add(this.label2);
            this.panelSCPSettings.Controls.Add(this.checkBoxSecurity);
            this.panelSCPSettings.Controls.Add(this.groupBoxSecurity);
            this.panelSCPSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSCPSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSCPSettings.Name = "panelSCPSettings";
            this.panelSCPSettings.Size = new System.Drawing.Size(786, 519);
            this.panelSCPSettings.TabIndex = 0;
            // 
            // storageSCUAETitleBox
            // 
            this.storageSCUAETitleBox.Location = new System.Drawing.Point(184, 51);
            this.storageSCUAETitleBox.Name = "storageSCUAETitleBox";
            this.storageSCUAETitleBox.Size = new System.Drawing.Size(192, 20);
            this.storageSCUAETitleBox.TabIndex = 21;
            this.storageSCUAETitleBox.Text = "SCU";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 23);
            this.label1.TabIndex = 22;
            this.label1.Text = "Remote AE Title:";
            // 
            // buttonSpecifyTS
            // 
            this.buttonSpecifyTS.Location = new System.Drawing.Point(301, 179);
            this.buttonSpecifyTS.Name = "buttonSpecifyTS";
            this.buttonSpecifyTS.Size = new System.Drawing.Size(75, 23);
            this.buttonSpecifyTS.TabIndex = 20;
            this.buttonSpecifyTS.Text = "Specify TS";
            this.buttonSpecifyTS.UseVisualStyleBackColor = true;
            this.buttonSpecifyTS.Click += new System.EventHandler(this.buttonSpecifyTS_Click);
            // 
            // numericStorageSCP
            // 
            this.numericStorageSCP.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numericStorageSCP.Location = new System.Drawing.Point(184, 230);
            this.numericStorageSCP.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.numericStorageSCP.Name = "numericStorageSCP";
            this.numericStorageSCP.Size = new System.Drawing.Size(64, 20);
            this.numericStorageSCP.TabIndex = 4;
            this.numericStorageSCP.ValueChanged += new System.EventHandler(this.numericStorageSCP_ValueChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 23);
            this.label7.TabIndex = 14;
            this.label7.Text = "Max PDU Size";
            // 
            // textBoxImplVersion
            // 
            this.textBoxImplVersion.Location = new System.Drawing.Point(184, 133);
            this.textBoxImplVersion.Name = "textBoxImplVersion";
            this.textBoxImplVersion.ReadOnly = true;
            this.textBoxImplVersion.Size = new System.Drawing.Size(192, 20);
            this.textBoxImplVersion.TabIndex = 2;
            this.textBoxImplVersion.Text = "DVTSCP3.2.0";
            // 
            // textBoxImplClassUID
            // 
            this.textBoxImplClassUID.Location = new System.Drawing.Point(184, 83);
            this.textBoxImplClassUID.Name = "textBoxImplClassUID";
            this.textBoxImplClassUID.ReadOnly = true;
            this.textBoxImplClassUID.Size = new System.Drawing.Size(192, 20);
            this.textBoxImplClassUID.TabIndex = 1;
            this.textBoxImplClassUID.Text = "1.2.826.0.1.3680043.2.1545.5";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 24);
            this.label3.TabIndex = 12;
            this.label3.Text = "Implementation Version name:";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 24);
            this.label5.TabIndex = 11;
            this.label5.Text = "Implementation Class UID:";
            // 
            // textBoxSCPPort
            // 
            this.textBoxSCPPort.Location = new System.Drawing.Point(184, 182);
            this.textBoxSCPPort.Name = "textBoxSCPPort";
            this.textBoxSCPPort.Size = new System.Drawing.Size(48, 20);
            this.textBoxSCPPort.TabIndex = 3;
            this.textBoxSCPPort.Text = "104";
            this.textBoxSCPPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxSCPPort_Validating);
            // 
            // textBoxSCPAETitle
            // 
            this.textBoxSCPAETitle.Location = new System.Drawing.Point(184, 27);
            this.textBoxSCPAETitle.Name = "textBoxSCPAETitle";
            this.textBoxSCPAETitle.Size = new System.Drawing.Size(192, 20);
            this.textBoxSCPAETitle.TabIndex = 0;
            this.textBoxSCPAETitle.Text = "DVTK_STR_SCP";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Listen Port:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "AE Title:";
            // 
            // tabPageCommitSCPConfig
            // 
            this.tabPageCommitSCPConfig.Controls.Add(this.panel1);
            this.tabPageCommitSCPConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageCommitSCPConfig.Name = "tabPageCommitSCPConfig";
            this.tabPageCommitSCPConfig.Size = new System.Drawing.Size(786, 519);
            this.tabPageCommitSCPConfig.TabIndex = 3;
            this.tabPageCommitSCPConfig.Text = "Storage Commitment Config";
            this.tabPageCommitSCPConfig.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelEcho);
            this.panel1.Controls.Add(this.labelPing);
            this.panel1.Controls.Add(this.buttonCommitEcho);
            this.panel1.Controls.Add(this.buttonCommitPing);
            this.panel1.Controls.Add(this.textBoxDelay);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.textBoxStorageCommitPort);
            this.panel1.Controls.Add(this.textBoxStorageCommitIPAdd);
            this.panel1.Controls.Add(this.textBoxStorageCommitAETitle);
            this.panel1.Controls.Add(this.labelRemotePort);
            this.panel1.Controls.Add(this.labelRemoteIP);
            this.panel1.Controls.Add(this.numericCommitSCP);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBoxCommitSCPPort);
            this.panel1.Controls.Add(this.textBoxCommitSCPAETitle);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.checkBoxSecurityCommit);
            this.panel1.Controls.Add(this.groupBoxSecurityCommit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(786, 519);
            this.panel1.TabIndex = 1;
            // 
            // labelEcho
            // 
            this.labelEcho.Location = new System.Drawing.Point(461, 331);
            this.labelEcho.Name = "labelEcho";
            this.labelEcho.Size = new System.Drawing.Size(212, 32);
            this.labelEcho.TabIndex = 17;
            // 
            // labelPing
            // 
            this.labelPing.Location = new System.Drawing.Point(460, 280);
            this.labelPing.Name = "labelPing";
            this.labelPing.Size = new System.Drawing.Size(212, 40);
            this.labelPing.TabIndex = 16;
            // 
            // buttonCommitEcho
            // 
            this.buttonCommitEcho.Location = new System.Drawing.Point(376, 333);
            this.buttonCommitEcho.Name = "buttonCommitEcho";
            this.buttonCommitEcho.Size = new System.Drawing.Size(80, 23);
            this.buttonCommitEcho.TabIndex = 8;
            this.buttonCommitEcho.Text = "DICOM Echo";
            this.buttonCommitEcho.Click += new System.EventHandler(this.buttonCommitEcho_Click);
            // 
            // buttonCommitPing
            // 
            this.buttonCommitPing.Location = new System.Drawing.Point(377, 286);
            this.buttonCommitPing.Name = "buttonCommitPing";
            this.buttonCommitPing.Size = new System.Drawing.Size(75, 23);
            this.buttonCommitPing.TabIndex = 7;
            this.buttonCommitPing.Text = "Ping";
            this.buttonCommitPing.Click += new System.EventHandler(this.buttonCommitPing_Click);
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(160, 188);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(40, 20);
            this.textBoxDelay.TabIndex = 3;
            this.textBoxDelay.Text = "5";
            this.textBoxDelay.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDelay_Validating);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 174);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 40);
            this.label8.TabIndex = 12;
            this.label8.Text = "Delay before Event Report(in sec):";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxStorageCommitPort
            // 
            this.textBoxStorageCommitPort.Location = new System.Drawing.Point(160, 387);
            this.textBoxStorageCommitPort.Name = "textBoxStorageCommitPort";
            this.textBoxStorageCommitPort.Size = new System.Drawing.Size(48, 20);
            this.textBoxStorageCommitPort.TabIndex = 6;
            this.textBoxStorageCommitPort.Text = "115";
            this.textBoxStorageCommitPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStorageCommitPort_Validating);
            // 
            // textBoxStorageCommitIPAdd
            // 
            this.textBoxStorageCommitIPAdd.Location = new System.Drawing.Point(160, 339);
            this.textBoxStorageCommitIPAdd.Name = "textBoxStorageCommitIPAdd";
            this.textBoxStorageCommitIPAdd.Size = new System.Drawing.Size(120, 20);
            this.textBoxStorageCommitIPAdd.TabIndex = 5;
            this.textBoxStorageCommitIPAdd.Text = "localhost";
            // 
            // textBoxStorageCommitAETitle
            // 
            this.textBoxStorageCommitAETitle.Location = new System.Drawing.Point(160, 288);
            this.textBoxStorageCommitAETitle.Name = "textBoxStorageCommitAETitle";
            this.textBoxStorageCommitAETitle.Size = new System.Drawing.Size(192, 20);
            this.textBoxStorageCommitAETitle.TabIndex = 4;
            this.textBoxStorageCommitAETitle.Text = "SCU";
            // 
            // labelRemotePort
            // 
            this.labelRemotePort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemotePort.Location = new System.Drawing.Point(8, 384);
            this.labelRemotePort.Name = "labelRemotePort";
            this.labelRemotePort.Size = new System.Drawing.Size(100, 23);
            this.labelRemotePort.TabIndex = 15;
            this.labelRemotePort.Text = "Remote Port:";
            // 
            // labelRemoteIP
            // 
            this.labelRemoteIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemoteIP.Location = new System.Drawing.Point(8, 336);
            this.labelRemoteIP.Name = "labelRemoteIP";
            this.labelRemoteIP.Size = new System.Drawing.Size(144, 23);
            this.labelRemoteIP.TabIndex = 14;
            this.labelRemoteIP.Text = "Remote TCP/IP Address:";
            // 
            // numericCommitSCP
            // 
            this.numericCommitSCP.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numericCommitSCP.Location = new System.Drawing.Point(160, 138);
            this.numericCommitSCP.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.numericCommitSCP.Name = "numericCommitSCP";
            this.numericCommitSCP.Size = new System.Drawing.Size(56, 20);
            this.numericCommitSCP.TabIndex = 2;
            this.numericCommitSCP.ValueChanged += new System.EventHandler(this.numericCommitSCP_ValueChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Max PDU Size:";
            // 
            // textBoxCommitSCPPort
            // 
            this.textBoxCommitSCPPort.Location = new System.Drawing.Point(160, 90);
            this.textBoxCommitSCPPort.Name = "textBoxCommitSCPPort";
            this.textBoxCommitSCPPort.Size = new System.Drawing.Size(40, 20);
            this.textBoxCommitSCPPort.TabIndex = 1;
            this.textBoxCommitSCPPort.Text = "105";
            this.textBoxCommitSCPPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxCommitSCPPort_Validating);
            // 
            // textBoxCommitSCPAETitle
            // 
            this.textBoxCommitSCPAETitle.Location = new System.Drawing.Point(160, 41);
            this.textBoxCommitSCPAETitle.Name = "textBoxCommitSCPAETitle";
            this.textBoxCommitSCPAETitle.Size = new System.Drawing.Size(192, 20);
            this.textBoxCommitSCPAETitle.TabIndex = 0;
            this.textBoxCommitSCPAETitle.Text = "DVTK_STRC_SCP";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(8, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 10;
            this.label13.Text = "Local Port:";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(8, 39);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 23);
            this.label14.TabIndex = 9;
            this.label14.Text = "Local AE Title:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonCommitTS);
            this.groupBox1.Location = new System.Drawing.Point(5, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 231);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SCP Setting";
            // 
            // buttonCommitTS
            // 
            this.buttonCommitTS.Location = new System.Drawing.Point(273, 82);
            this.buttonCommitTS.Name = "buttonCommitTS";
            this.buttonCommitTS.Size = new System.Drawing.Size(75, 23);
            this.buttonCommitTS.TabIndex = 41;
            this.buttonCommitTS.Text = "Specify TS";
            this.buttonCommitTS.UseVisualStyleBackColor = true;
            this.buttonCommitTS.Click += new System.EventHandler(this.buttonCommitTS_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelRemoteAET);
            this.groupBox2.Location = new System.Drawing.Point(4, 257);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 175);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Remote Node Setting";
            // 
            // labelRemoteAET
            // 
            this.labelRemoteAET.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemoteAET.Location = new System.Drawing.Point(6, 30);
            this.labelRemoteAET.Name = "labelRemoteAET";
            this.labelRemoteAET.Size = new System.Drawing.Size(120, 23);
            this.labelRemoteAET.TabIndex = 13;
            this.labelRemoteAET.Text = "Remote AE Title:";
            // 
            // tabPageSopClass
            // 
            this.tabPageSopClass.Controls.Add(this.sopClassesUserControlStoreSCP);
            this.tabPageSopClass.Location = new System.Drawing.Point(4, 22);
            this.tabPageSopClass.Name = "tabPageSopClass";
            this.tabPageSopClass.Size = new System.Drawing.Size(786, 519);
            this.tabPageSopClass.TabIndex = 4;
            this.tabPageSopClass.Text = "SOP Class Selection";
            this.tabPageSopClass.UseVisualStyleBackColor = true;
            // 
            // sopClassesUserControlStoreSCP
            // 
            this.sopClassesUserControlStoreSCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sopClassesUserControlStoreSCP.Location = new System.Drawing.Point(0, 0);
            this.sopClassesUserControlStoreSCP.Name = "sopClassesUserControlStoreSCP";
            this.sopClassesUserControlStoreSCP.Size = new System.Drawing.Size(786, 519);
            this.sopClassesUserControlStoreSCP.TabIndex = 0;
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.dvtkWebBrowserSCPEmulator);
            this.tabPageResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Size = new System.Drawing.Size(786, 519);
            this.tabPageResults.TabIndex = 1;
            this.tabPageResults.Text = "Validation Results";
            this.tabPageResults.UseVisualStyleBackColor = true;
            this.tabPageResults.Visible = false;
            // 
            // dvtkWebBrowserSCPEmulator
            // 
            this.dvtkWebBrowserSCPEmulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvtkWebBrowserSCPEmulator.Location = new System.Drawing.Point(0, 0);
            this.dvtkWebBrowserSCPEmulator.Name = "dvtkWebBrowserSCPEmulator";
            this.dvtkWebBrowserSCPEmulator.Size = new System.Drawing.Size(786, 519);
            this.dvtkWebBrowserSCPEmulator.TabIndex = 0;
            this.dvtkWebBrowserSCPEmulator.XmlStyleSheetFullFileName = "";
            // 
            // tabPageLogging
            // 
            this.tabPageLogging.Controls.Add(this.userControlActivityLogging);
            this.tabPageLogging.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogging.Name = "tabPageLogging";
            this.tabPageLogging.Size = new System.Drawing.Size(786, 519);
            this.tabPageLogging.TabIndex = 2;
            this.tabPageLogging.Text = "Logging";
            this.tabPageLogging.UseVisualStyleBackColor = true;
            this.tabPageLogging.Visible = false;
            // 
            // userControlActivityLogging
            // 
            this.userControlActivityLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlActivityLogging.Interval = 100;
            this.userControlActivityLogging.Location = new System.Drawing.Point(0, 0);
            this.userControlActivityLogging.Name = "userControlActivityLogging";
            this.userControlActivityLogging.Size = new System.Drawing.Size(786, 519);
            this.userControlActivityLogging.TabIndex = 0;
            // 
            // groupBoxSecurity
            // 
            this.groupBoxSecurity.Controls.Add(this.labelMaxTlsVersion);
            this.groupBoxSecurity.Controls.Add(this.labelMinTlsVersion);
            this.groupBoxSecurity.Controls.Add(this.comboboxMaxTlsVersion);
            this.groupBoxSecurity.Controls.Add(this.comboboxMinTlsVersion);
            this.groupBoxSecurity.Controls.Add(this.labelCertificate);
            this.groupBoxSecurity.Controls.Add(this.textBoxCertificate);
            this.groupBoxSecurity.Controls.Add(this.buttonCertificate);
            this.groupBoxSecurity.Controls.Add(this.labelCredential);
            this.groupBoxSecurity.Controls.Add(this.textBoxCredential);
            this.groupBoxSecurity.Controls.Add(this.buttonCredential);
            this.groupBoxSecurity.Enabled = false;
            this.groupBoxSecurity.Location = new System.Drawing.Point(0, 275);
            this.groupBoxSecurity.Name = "groupBoxSecurity";
            this.groupBoxSecurity.Size = new System.Drawing.Size(600, 75);
            this.groupBoxSecurity.TabStop = false;
            this.groupBoxSecurity.Text = "Security Options ";
            // 
            // checkBoxSecurity
            // 
            this.checkBoxSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSecurity.Location = new System.Drawing.Point(8, 250);
            this.checkBoxSecurity.Name = "checkBoxSecurity";
            this.checkBoxSecurity.Size = new System.Drawing.Size(136, 24);
            this.checkBoxSecurity.TabIndex = 8;
            this.checkBoxSecurity.Text = "Secure Connection";
            this.checkBoxSecurity.CheckedChanged += new System.EventHandler(this.checkBoxSecurity_CheckedChanged);
            // 
            // labelMaxTlsVersion
            // 
            this.labelMaxTlsVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxTlsVersion.Location = new System.Drawing.Point(8, 25);
            this.labelMaxTlsVersion.Name = "labelMaxTlsVersion";
            this.labelMaxTlsVersion.Size = new System.Drawing.Size(110, 24);
            this.labelMaxTlsVersion.TabIndex = 20;
            this.labelMaxTlsVersion.Text = "Max TLS Version: ";
            // 
            // labelMinTlsVersion
            // 
            this.labelMinTlsVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinTlsVersion.Location = new System.Drawing.Point(8, 50);
            this.labelMinTlsVersion.Name = "labelMinTlsVersion";
            this.labelMinTlsVersion.Size = new System.Drawing.Size(110, 24);
            this.labelMinTlsVersion.TabIndex = 21;
            this.labelMinTlsVersion.Text = "Min TLS Version: ";
            // 
            // comboboxMaxTlsVersion
            // 
            this.comboboxMaxTlsVersion.FormattingEnabled = true;
            this.comboboxMaxTlsVersion.Items.AddRange(new object[] {
            "TLS1.0",
            "TLS1.1",
            "TLS1.2",
            "TLS1.3"});
            this.comboboxMaxTlsVersion.Location = new System.Drawing.Point(125, 25);
            this.comboboxMaxTlsVersion.Size = new System.Drawing.Size(100, 28);
            this.comboboxMaxTlsVersion.SelectedIndexChanged += new System.EventHandler(this.comboboxMaxTlsVersion_SelectedIndexChanged);

            // 
            // comboboxMinTlsVersion
            // 
            this.comboboxMinTlsVersion.FormattingEnabled = true;
            this.comboboxMinTlsVersion.Items.AddRange(new object[] {
            "TLS1.0",
            "TLS1.1",
            "TLS1.2",
            "TLS1.3"});
            this.comboboxMinTlsVersion.Location = new System.Drawing.Point(125, 50);
            this.comboboxMinTlsVersion.Size = new System.Drawing.Size(100, 28);
            this.comboboxMinTlsVersion.SelectedIndexChanged += new System.EventHandler(this.comboboxMinTlsVersion_SelectedIndexChanged);
            // 
            // labelcertificate
            // 
            this.labelCertificate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCertificate.Location = new System.Drawing.Point(230, 25);
            this.labelCertificate.Name = "labelCertificate";
            this.labelCertificate.Size = new System.Drawing.Size(80, 24);
            this.labelCertificate.Text = "Certificate: ";
            // 
            // labelcredential
            // 
            this.labelCredential.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCredential.Location = new System.Drawing.Point(230, 50);
            this.labelCredential.Name = "labelCredential";
            this.labelCredential.Size = new System.Drawing.Size(80, 24);
            this.labelCredential.Text = "Credential: ";
            // 
            // textboxcertificate
            // 
            this.textBoxCertificate.Location = new System.Drawing.Point(320, 25);
            this.textBoxCertificate.Name = "textBoxCertificate";
            this.textBoxCertificate.ReadOnly = true;
            this.textBoxCertificate.Size = new System.Drawing.Size(180, 28);
            // 
            // textboxcertificate
            // 
            this.textBoxCredential.Location = new System.Drawing.Point(320, 50);
            this.textBoxCredential.Name = "textBoxCredential";
            this.textBoxCredential.ReadOnly = true;
            this.textBoxCredential.Size = new System.Drawing.Size(180, 28);
            // 
            // buttonCertificate
            // 
            this.buttonCertificate.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCertificate.Location = new System.Drawing.Point(510, 25);
            this.buttonCertificate.Name = "buttonCertificate";
            this.buttonCertificate.Size = new System.Drawing.Size(75, 20);
            this.buttonCertificate.Text = "Browse";
            this.buttonCertificate.UseVisualStyleBackColor = true;
            this.buttonCertificate.Click += new System.EventHandler(this.buttonCertificate_Click);
            // 
            // buttonCredential
            // 
            this.buttonCredential.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCredential.Location = new System.Drawing.Point(510, 50);
            this.buttonCredential.Name = "buttonCredential";
            this.buttonCredential.Size = new System.Drawing.Size(75, 20);
            this.buttonCredential.Text = "Browse";
            this.buttonCredential.UseVisualStyleBackColor = true;
            this.buttonCredential.Click += new System.EventHandler(this.buttonCredential_Click);

            // 
            // groupBoxSecurityCommit
            // 
            this.groupBoxSecurityCommit.Controls.Add(this.labelMaxTlsVersionCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.labelMinTlsVersionCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.comboboxMaxTlsVersionCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.comboboxMinTlsVersionCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.labelCertificateCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.textBoxCertificateCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.buttonCertificateCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.labelCredentialCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.textBoxCredentialCommit);
            this.groupBoxSecurityCommit.Controls.Add(this.buttonCredentialCommit);
            this.groupBoxSecurityCommit.Enabled = false;
            this.groupBoxSecurityCommit.Location = new System.Drawing.Point(0, 460);
            this.groupBoxSecurityCommit.Name = "groupBoxSecurity";
            this.groupBoxSecurityCommit.Size = new System.Drawing.Size(600, 75);
            this.groupBoxSecurityCommit.TabStop = false;
            this.groupBoxSecurityCommit.Text = "Security Options ";
            // 
            // checkBoxSecurityCommit
            // 
            this.checkBoxSecurityCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSecurityCommit.Location = new System.Drawing.Point(8, 430);
            this.checkBoxSecurityCommit.Name = "checkBoxSecurity";
            this.checkBoxSecurityCommit.Size = new System.Drawing.Size(136, 24);
            this.checkBoxSecurityCommit.TabIndex = 8;
            this.checkBoxSecurityCommit.Text = "Secure Connection";
            this.checkBoxSecurityCommit.CheckedChanged += new System.EventHandler(this.checkBoxSecurityCommit_CheckedChanged);
            // 
            // labelMaxTlsVersionCommit
            // 
            this.labelMaxTlsVersionCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxTlsVersionCommit.Location = new System.Drawing.Point(8, 25);
            this.labelMaxTlsVersionCommit.Name = "labelMaxTlsVersion";
            this.labelMaxTlsVersionCommit.Size = new System.Drawing.Size(110, 24);
            this.labelMaxTlsVersionCommit.TabIndex = 20;
            this.labelMaxTlsVersionCommit.Text = "Max TLS Version: ";
            // 
            // labelMinTlsVersionCommit
            // 
            this.labelMinTlsVersionCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinTlsVersionCommit.Location = new System.Drawing.Point(8, 50);
            this.labelMinTlsVersionCommit.Name = "labelMinTlsVersion";
            this.labelMinTlsVersionCommit.Size = new System.Drawing.Size(110, 24);
            this.labelMinTlsVersionCommit.TabIndex = 21;
            this.labelMinTlsVersionCommit.Text = "Min TLS Version: ";
            // 
            // comboboxMaxTlsVersionCommit
            // 
            this.comboboxMaxTlsVersionCommit.FormattingEnabled = true;
            this.comboboxMaxTlsVersionCommit.Items.AddRange(new object[] {
            "TLS1.0",
            "TLS1.1",
            "TLS1.2",
            "TLS1.3"});
            this.comboboxMaxTlsVersionCommit.Location = new System.Drawing.Point(125, 25);
            this.comboboxMaxTlsVersionCommit.Size = new System.Drawing.Size(100, 28);
            this.comboboxMaxTlsVersionCommit.SelectedIndexChanged += new System.EventHandler(this.comboboxMaxTlsVersionCommit_SelectedIndexChanged);

            // 
            // comboboxMinTlsVersionCommit
            // 
            this.comboboxMinTlsVersionCommit.FormattingEnabled = true;
            this.comboboxMinTlsVersionCommit.Items.AddRange(new object[] {
            "TLS1.0",
            "TLS1.1",
            "TLS1.2",
            "TLS1.3"});
            this.comboboxMinTlsVersionCommit.Location = new System.Drawing.Point(125, 50);
            this.comboboxMinTlsVersionCommit.Size = new System.Drawing.Size(100, 28);
            this.comboboxMinTlsVersionCommit.SelectedIndexChanged += new System.EventHandler(this.comboboxMinTlsVersionCommit_SelectedIndexChanged);
            // 
            // labelcertificateCommit
            // 
            this.labelCertificateCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCertificateCommit.Location = new System.Drawing.Point(230, 25);
            this.labelCertificateCommit.Name = "labelCertificate";
            this.labelCertificateCommit.Size = new System.Drawing.Size(80, 24);
            this.labelCertificateCommit.Text = "Certificate: ";
            // 
            // labelcredentialCommit
            // 
            this.labelCredentialCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCredentialCommit.Location = new System.Drawing.Point(230, 50);
            this.labelCredentialCommit.Name = "labelCredential";
            this.labelCredentialCommit.Size = new System.Drawing.Size(80, 24);
            this.labelCredentialCommit.Text = "Credential: ";
            // 
            // textboxcertificateCommit
            // 
            this.textBoxCertificateCommit.Location = new System.Drawing.Point(320, 25);
            this.textBoxCertificateCommit.Name = "textBoxCertificate";
            this.textBoxCertificateCommit.ReadOnly = true;
            this.textBoxCertificateCommit.Size = new System.Drawing.Size(180, 28);
            // 
            // textboxcertificateCommit
            // 
            this.textBoxCredentialCommit.Location = new System.Drawing.Point(320, 50);
            this.textBoxCredentialCommit.Name = "textBoxCredential";
            this.textBoxCredentialCommit.ReadOnly = true;
            this.textBoxCredentialCommit.Size = new System.Drawing.Size(180, 28);
            // 
            // buttonCertificateCommit
            // 
            this.buttonCertificateCommit.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCertificateCommit.Location = new System.Drawing.Point(510, 25);
            this.buttonCertificateCommit.Name = "buttonCertificate";
            this.buttonCertificateCommit.Size = new System.Drawing.Size(75, 20);
            this.buttonCertificateCommit.Text = "Browse";
            this.buttonCertificateCommit.UseVisualStyleBackColor = true;
            this.buttonCertificateCommit.Click += new System.EventHandler(this.buttonCertificateCommit_Click);
            // 
            // buttonCredentialCommit
            // 
            this.buttonCredentialCommit.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCredentialCommit.Location = new System.Drawing.Point(510, 50);
            this.buttonCredentialCommit.Name = "buttonCredential";
            this.buttonCredentialCommit.Size = new System.Drawing.Size(75, 20);
            this.buttonCredentialCommit.Text = "Browse";
            this.buttonCredentialCommit.UseVisualStyleBackColor = true;
            this.buttonCredentialCommit.Click += new System.EventHandler(this.buttonCredentialCommit_Click);
            // 
            // StorageSCPEmulator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(794, 573);
            this.Controls.Add(this.tabControlStorageSCP);
            this.Controls.Add(this.toolBarSCPEmulator);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenuStorageSCP;
            this.MinimumSize = new System.Drawing.Size(675, 530);
            this.Name = "StorageSCPEmulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Storage SCP Emulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StorageSCPEmulator_FormClosing);
            this.Load += new System.EventHandler(this.StorageSCPEmulator_Load);
            this.tabControlStorageSCP.ResumeLayout(false);
            this.tabPageStorageSCPConfig.ResumeLayout(false);
            this.panelSCPSettings.ResumeLayout(false);
            this.panelSCPSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericStorageSCP)).EndInit();
            this.tabPageCommitSCPConfig.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCommitSCP)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPageSopClass.ResumeLayout(false);
            this.tabPageResults.ResumeLayout(false);
            this.tabPageLogging.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Size = new System.Drawing.Size(794, 650);

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
            DvtkApplicationLayer.VersionChecker.CheckVersion();
			// Initialize the Dvtk library
			Dvtk.Setup.Initialize();

			//Application.EnableVisualStyles();
			Application.Run(new StorageSCPEmulator());

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

		private void menuItemAboutEmulator_Click(object sender, System.EventArgs e)
		{
			AboutForm about = new AboutForm("DVTk Storage SCP Emulator");
			about.ShowDialog();
		}

		private void StorageSCPEmulator_Load(object sender, System.EventArgs e)
		{
			toolBarButtonError.Visible = false;
			toolBarButtonWarning.Visible = false;
			toolBarButtonLeft.Visible = false;
			toolBarButtonRight.Visible = false;
			toolBarButtonAbort.Enabled = false;

			//Hide the result tab page
			tabControlStorageSCP.Controls.Remove(tabPageResults);
			tabControlStorageSCP.SelectedTab = tabPageStorageSCPConfig;

			try
			{
				// Load the Emulator session
                LoadFromSessionFiles(this.storageOptions, this.storageCommitOptions);

                //
                // Stored files options.
                //
                this.fileGroups = new FileGroups("Storage SCP Emulator");

                this.validationResultsFileGroup = new ValidationResultsFileGroup();
                this.validationResultsFileGroup.DefaultFolder = "Results";
                this.fileGroups.Add(validationResultsFileGroup);

                this.receivedDicomMessagesFileGroup = new ReceivedDicomMessagesFileGroup();
                this.receivedDicomMessagesFileGroup.DefaultFolder = "Received DICOM Messages";
                this.fileGroups.Add(receivedDicomMessagesFileGroup);

                PixFileGroup pixFileGroup = new PixFileGroup(receivedDicomMessagesFileGroup);
                this.fileGroups.Add(pixFileGroup);

                this.fileGroups.CreateDirectories();

                this.fileGroups.CheckIsConfigured("\"Stored Files\\Options...\" menu item");

                this.storageOptions.ResultsDirectory = validationResultsFileGroup.Directory;
                this.storageOptions.DataDirectory = receivedDicomMessagesFileGroup.Directory;

                this.storageCommitOptions.ResultsDirectory = validationResultsFileGroup.Directory;
                this.storageCommitOptions.DataDirectory = receivedDicomMessagesFileGroup.Directory;

                // Make sure the session files contain the same information as the Stored Files user settings.
                SaveToSessionFiles(this.storageOptions, this.storageCommitOptions);

                sopClassesUserControlStoreSCP.UpdateDataGrid(this.storageOptions.DvtkScriptSession);

                this.storageOptions.StorageMode = Dvtk.Sessions.StorageMode.AsMedia;

                textBoxSCPAETitle.Text = this.storageOptions.LocalAeTitle;
                textBoxSCPPort.Text = this.storageOptions.LocalPort.ToString();
                textBoxImplClassUID.Text = this.storageOptions.DvtkScriptSession.DvtSystemSettings.ImplementationClassUid;
                textBoxImplVersion.Text = this.storageOptions.DvtkScriptSession.DvtSystemSettings.ImplementationVersionName;
                storageSCUAETitleBox.Text = this.storageOptions.RemoteAeTitle;

                textBoxCommitSCPAETitle.Text = this.storageCommitOptions.LocalAeTitle;
                textBoxCommitSCPPort.Text = this.storageCommitOptions.LocalPort.ToString();

                textBoxStorageCommitPort.Text = this.storageCommitOptions.RemotePort.ToString();
                textBoxStorageCommitIPAdd.Text = this.storageCommitOptions.RemoteHostName;
                textBoxStorageCommitAETitle.Text = this.storageCommitOptions.RemoteAeTitle;

                numericCommitSCP.Value = this.storageCommitOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived;
                numericStorageSCP.Value = this.storageOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived;

                this.eventDelay = uint.Parse(textBoxDelay.Text);
			}
			catch (Exception except) 
			{
				string msg = string.Format("Exception:{0}\n", except.Message);
				MessageBox.Show(this, "Application error:" +msg, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error );
				this.Close();
				return;
			}
		}

        private void menuItemLoad_Click(object sender, System.EventArgs e)
		{
            if (!Directory.Exists(ConfigurationsDirectory))
            {
                Directory.CreateDirectory(ConfigurationsDirectory);
            }

			string configFileName = "";
			OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.InitialDirectory = ConfigurationsDirectory;
			theOpenFileDialog.Filter = "Config files (*.xml) |*.xml";
			theOpenFileDialog.Multiselect = false;
			theOpenFileDialog.Title = "Load a Config file";

			if (theOpenFileDialog.ShowDialog () == DialogResult.OK)
			{
				configFileName = theOpenFileDialog.FileName;
				LoadConfig(configFileName);
			}
		}

        private String ConfigurationsDirectory
        {
            get
            {
                return (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"DVTk\Storage SCP Emulator\Configurations"));
            }
        }

		private void menuItemSave_Click(object sender, System.EventArgs e)
		{
            if (!Directory.Exists(ConfigurationsDirectory))
            {
                Directory.CreateDirectory(ConfigurationsDirectory);
            }

			string configFileName = "";
			SaveFileDialog saveSetUpFileDlg = new SaveFileDialog();
            saveSetUpFileDlg.InitialDirectory = ConfigurationsDirectory;
			saveSetUpFileDlg.Title = "Save the current configuration";
			saveSetUpFileDlg.Filter = "Config files (*.xml) |*.xml";
			if (saveSetUpFileDlg.ShowDialog () == DialogResult.OK)
			{
				configFileName = saveSetUpFileDlg.FileName;
				SaveConfig(configFileName);
			}
		}

		/// <summary>
		/// Load the Emulator Configuration into the UI.
		/// </summary>
		/// <param name="configurationFilename">Configuration Filename</param>
		private void LoadConfig(System.String configurationFilename)
		{
			XmlTextReader reader = null;
			try
			{
				reader = new XmlTextReader(configurationFilename);
				
				reader.ReadStartElement("SCPEmulatorConfig");
				textBoxImplClassUID.Text = reader.ReadElementString("ImplClassUID");
				textBoxImplVersion.Text = reader.ReadElementString("ImplVersionName");

				reader.ReadStartElement("StorageConfiguration");
				textBoxSCPAETitle.Text = reader.ReadElementString("AETitle");
                storageSCUAETitleBox.Text = reader.ReadElementString("RemoteAETitle");
				textBoxSCPPort.Text = reader.ReadElementString("ListenPort");
				numericStorageSCP.Value = decimal.Parse(reader.ReadElementString("MaxPDUSize"));
                reader.ReadEndElement();

				reader.ReadStartElement("CommitConfiguration");
				textBoxCommitSCPAETitle.Text = reader.ReadElementString("AETitle");
				textBoxCommitSCPPort.Text = reader.ReadElementString("ListenPort");
				numericCommitSCP.Value = decimal.Parse(reader.ReadElementString("MaxPDUSize"));
				textBoxDelay.Text = reader.ReadElementString("Delay");
				textBoxStorageCommitAETitle.Text = reader.ReadElementString("RemoteAETitle");
				textBoxStorageCommitIPAdd.Text = reader.ReadElementString("RemoteIpAddress");
				textBoxStorageCommitPort.Text = reader.ReadElementString("RemotePort");
				reader.ReadEndElement();

				reader.ReadEndElement();
			}
			catch (System.Exception e)
			{
				System.String message = System.String.Format("Failed to read configuration file: \"{0}\"", configurationFilename);
				throw new System.SystemException(message, e);
			}
			finally 
			{
				if (reader != null) reader.Close();
			}
		}

		/// <summary>
		/// Save the emulator Configuration into the given file.
		/// </summary>
		/// <param name="configurationFilename">Configuration Filename</param>
		private void SaveConfig(System.String configurationFilename)
		{
			XmlTextWriter writer = new XmlTextWriter(configurationFilename, System.Text.Encoding.ASCII);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument(true);

			writer.WriteStartElement("SCPEmulatorConfig");
			writer.WriteElementString("ImplClassUID", textBoxImplClassUID.Text);
			writer.WriteElementString("ImplVersionName", textBoxImplVersion.Text);

			writer.WriteStartElement("StorageConfiguration");
			writer.WriteElementString("AETitle", textBoxSCPAETitle.Text);
            writer.WriteElementString("RemoteAETitle", storageSCUAETitleBox.Text);
			writer.WriteElementString("ListenPort", textBoxSCPPort.Text);
			writer.WriteElementString("MaxPDUSize", numericStorageSCP.Value.ToString());
			//writer.WriteElementString("AcceptDuplicateImage", duplicateStr);
			writer.WriteEndElement();

			writer.WriteStartElement("CommitConfiguration");
			writer.WriteElementString("AETitle", textBoxCommitSCPAETitle.Text);
			writer.WriteElementString("ListenPort", textBoxCommitSCPPort.Text);
			writer.WriteElementString("MaxPDUSize", numericCommitSCP.Value.ToString());
			writer.WriteElementString("Delay", textBoxDelay.Text);
			writer.WriteElementString("RemoteAETitle", textBoxStorageCommitAETitle.Text);
			writer.WriteElementString("RemoteIpAddress", textBoxStorageCommitIPAdd.Text);
			writer.WriteElementString("RemotePort", textBoxStorageCommitPort.Text);
			writer.WriteEndElement();

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
		}

        private void UpdateConfig()
        {
            //Update DVT & SUT settings
            this.storageOptions.LocalAeTitle = textBoxSCPAETitle.Text;
            this.storageOptions.RemoteAeTitle = storageSCUAETitleBox.Text;
            this.storageOptions.LocalPort = UInt16.Parse(textBoxSCPPort.Text);
            //this.storageOptions.AutoValidate = false;
            this.eventDelay = uint.Parse(textBoxDelay.Text);

            //Update and save commit settings
            this.storageCommitOptions.RemoteAeTitle = textBoxStorageCommitAETitle.Text;
            this.storageCommitOptions.RemoteHostName = textBoxStorageCommitIPAdd.Text;
            this.storageCommitOptions.RemotePort = UInt16.Parse(textBoxStorageCommitPort.Text);
            this.storageCommitOptions.LocalAeTitle = textBoxCommitSCPAETitle.Text;
            this.storageCommitOptions.LocalPort = UInt16.Parse(textBoxCommitSCPPort.Text);
            //this.storageCommitOptions.AutoValidate = false;

            this.storageCommitOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (uint)numericCommitSCP.Value;
            this.storageOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (uint)numericStorageSCP.Value;

            // Save changed session settings before running emulator
            SaveToSessionFiles(this.storageOptions, this.storageCommitOptions);
        }

        private void textBoxCommitSCPPort_Validating(object sender, CancelEventArgs e)
        {
            UInt16 newPortNumber = 0;

            try
            {
                newPortNumber = Convert.ToUInt16(this.textBoxCommitSCPPort.Text);
            }
            catch
            {
                MessageBox.Show("Invalid port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void textBoxStorageCommitPort_Validating(object sender, CancelEventArgs e)
        {
            UInt16 newPortNumber = 0;

            try
            {
                newPortNumber = Convert.ToUInt16(this.textBoxStorageCommitPort.Text);
            }
            catch
            {
                MessageBox.Show("Invalid port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void textBoxDelay_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                this.eventDelay = UInt16.Parse(textBoxDelay.Text);
            }
            catch
            {
                MessageBox.Show("Invalid delay.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }            
        }

        private void textBoxSCPPort_Validating(object sender, CancelEventArgs e)
        {
            UInt16 newPortNumber = 0;

            try
            {
                newPortNumber = Convert.ToUInt16(this.textBoxSCPPort.Text);
            }
            catch
            {
                MessageBox.Show("Invalid port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void LoadFromSessionFiles(DicomThreadOptions storageOptions, DicomThreadOptions storageCommitOptions)
        {
            storageOptions.LoadFromFile(Path.Combine(documentsRootPath, "StorageSCPEmulator.ses"));
            storageCommitOptions.LoadFromFile(Path.Combine(documentsRootPath, "CommitSCPEmulator.ses"));
        }

        private void SaveToSessionFiles(DicomThreadOptions storageOptions, DicomThreadOptions storageCommitOptions)
        {
            storageOptions.SaveToFile(Path.Combine(documentsRootPath, "StorageSCPEmulator.ses"));
            storageCommitOptions.SaveToFile(Path.Combine(documentsRootPath, "CommitSCPEmulator.ses"));
        }

		private void menuItemRun_Click(object sender, System.EventArgs e)
		{
			menuItemStop.Enabled = true;
			menuItemRun.Enabled = false;
			toolBarButtonRun.ToolTipText = "Stop Emulator";
			toolBarButtonRun.ImageIndex = 1;

			//toolBarButtonTS.Enabled = false;
            buttonSpecifyTS.Enabled = false;
            buttonCommitTS.Enabled = false;
			toolBarButtonAbort.Enabled = true;
			//isResultGatheringStarted = false;
			isStopped = false;

			//If result tab is present, remove it
			if(tabControlStorageSCP.Controls.Contains(tabPageResults))
			{
				tabControlStorageSCP.Controls.Remove(tabPageResults);
			}

			try
			{
                UpdateConfig();

                // Display activity logging tab
                //
                // Make the Activity Logging Tab the only Tab visible and clean it.
                //
                this.tabControlStorageSCP.Controls.Clear();
                this.tabControlStorageSCP.Controls.Add(this.tabPageLogging);
                this.userControlActivityLogging.Clear();

                // Update loaded definitions files
                this.selectedSops.Clear();
                foreach (DefinitionFile theDefinitionFile in sopClassesUserControlStoreSCP.SelectedDefinitionFilesList)
                {
                    if (theDefinitionFile.Loaded)
                    {
                        this.selectedSops.Add(theDefinitionFile.SOPClassUID);
                    }
                }

                //
                // Set the correct settings for the overview DicomThread and some settings for its child threads.
                //
                String startDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss", System.Globalization.CultureInfo.InvariantCulture);

                this.storageOptions.ResultsDirectory = this.validationResultsFileGroup.Directory;
                this.storageCommitOptions.ResultsDirectory = this.validationResultsFileGroup.Directory;

                this.threadManager.ThreadsStateChangeEvent += this.threadsStateChangeEventHandler;

                this.overviewThread = new OverviewThread(this.storageOptions, this.storageCommitOptions, this.selectedSops, this.selectedTS, this.selectedTSCommit, (int)eventDelay, startDateTime);
                this.overviewThread.Initialize(threadManager);
                this.overviewThread.Options.ResultsDirectory = this.validationResultsFileGroup.Directory;
                this.overviewThread.Options.Identifier = "Storage_SCP_Emulator";
                this.overviewThread.Options.ResultsFileNameOnlyWithoutExtension = startDateTime + "_Storage_SCP_Emulator"; 
                this.overviewThread.Options.AttachChildsToUserInterfaces = true;
                this.overviewThread.Options.LogThreadStartingAndStoppingInParent = false;
                this.overviewThread.Options.LogWaitingForCompletionChildThreads = false;

                this.userControlActivityLogging.Attach(overviewThread);
                
                //
                // Start the DicomThread.
                //
                this.overviewThread.Start();				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolBarButtonRun.ToolTipText = "Run Emulator";
                toolBarButtonRun.ImageIndex = 0;
			}
		}

        /// <summary>
        /// Handles a thread state change event of a Thread contained in a ThreadManager.
        /// Takes care that when all threads are stopped, the HandleExecutionCompleted method is called.
        /// </summary>
        /// <param name="thread">The thread which state changes.</param>
        /// <param name="oldThreadState">Old state of the thread.</param>
        /// <param name="newThreadState">New state of the thread.</param>
        /// <param name="numberOfUnStarted">Number of unstarted threads of the ThreadManager.</param>
        /// <param name="numberOfRunning">Number of running threads of the ThreadManager.</param>
        /// <param name="numberOfStopping">Number of stopping threads of the ThreadManager.</param>
        /// <param name="numberOfStopped">Number of stopped threads of the ThreadManager.</param>
        private void HandleThreadsStateChangeEvent(Thread thread, DvtkHighLevelInterface.Common.Threads.ThreadState oldThreadState, DvtkHighLevelInterface.Common.Threads.ThreadState newThreadState, int numberOfUnStarted, int numberOfRunning, int numberOfStopping, int numberOfStopped)
        {
            if ((numberOfRunning == 0) && (numberOfStopping == 0) && (numberOfStopped > 0))
            {
                if ((this.InvokeRequired) && (this.IsHandleCreated))
                    Invoke(new ExecutionCompletedHandler(this.HandleExecutionCompleted));
            }
        }

        private void menuItemStop_Click(object sender, System.EventArgs e)
		{
			menuItemStop.Enabled = false;
			menuItemRun.Enabled = true;
			toolBarButtonRun.ToolTipText = "Run Emulator";
			toolBarButtonRun.ImageIndex = 0;

			//toolBarButtonTS.Enabled = true;
            buttonSpecifyTS.Enabled = true;
            buttonCommitTS.Enabled = true;
			toolBarButtonAbort.Enabled = false;
			isStopped = true;

			//Clear all tabs
            this.tabControlStorageSCP.Controls.Clear();

			// Add required tabs
            tabControlStorageSCP.Controls.Add(tabPageStorageSCPConfig);
            tabControlStorageSCP.Controls.Add(tabPageCommitSCPConfig);
            tabControlStorageSCP.Controls.Add(tabPageSopClass);
            //tabControlStorageSCP.Controls.Add(tabPageTS);
            tabControlStorageSCP.Controls.Add(tabPageLogging);
            tabControlStorageSCP.Controls.Add(tabPageResults);

            try
			{
				//emulatorSession.TerminateConnection();
                Cursor.Current = Cursors.WaitCursor;
                this.threadManager.Stop();
				tabControlStorageSCP.SelectedTab = tabPageStorageSCPConfig;
			}
			catch (Exception ex)
			{
				MessageBox.Show( "Couldn't Stop the SCP:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}			
		}

        /// <summary>
        /// Called when all threads have stopped.
        /// Takes care that buttons and tabs are enabled/disabled.
        /// </summary>
        private void HandleExecutionCompleted()
        {
            Cursor.Current = Cursors.Default;            

            this.tabControlStorageSCP.SelectedTab = this.tabPageResults;
            isStopped = true;
            Cleanup();
        }

		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void tabControlStorageSCP_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			labelPing.Text = "";
			labelEcho.Text = "";
			if (tabControlStorageSCP.SelectedTab == tabPageResults) 
			{	
            	toolBarButtonError.Visible = true;
				toolBarButtonWarning.Visible = true;
				toolBarButtonLeft.Visible = true;
				toolBarButtonRight.Visible = true;

                toolBarButtonError.Enabled = this.dvtkWebBrowserSCPEmulator.ContainsErrors;
                toolBarButtonWarning.Enabled = this.dvtkWebBrowserSCPEmulator.ContainsWarnings;
				
				this.topXmlResults = overviewThread.Options.DetailResultsFullFileName;
                this.dvtkWebBrowserSCPEmulator.Navigate(topXmlResults);                
			}
			else
			{
				toolBarButtonError.Visible = false;
				toolBarButtonWarning.Visible = false;
				toolBarButtonLeft.Visible = false;
				toolBarButtonRight.Visible = false;
			}		
		}

		private void toolBarSCPEmulator_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if( e.Button == toolBarButtonRun)
			{
				if(toolBarButtonRun.ImageIndex == 0)
				{
					toolBarButtonRun.ToolTipText = "Stop Emulator";
					toolBarButtonRun.ImageIndex = 1;
					menuItemRun_Click( sender, null );
				}
				else if(toolBarButtonRun.ImageIndex == 1)
				{
					toolBarButtonRun.ToolTipText = "Run Emulator";
					toolBarButtonRun.ImageIndex = 0;
					menuItemStop_Click( sender, null );
				}
				else
				{
				}
			}			
			else if( e.Button == toolBarButtonError)
			{
				this.dvtkWebBrowserSCPEmulator.FindNextText("Error:", true, true);
			}
			else if( e.Button == toolBarButtonWarning)
			{
				this.dvtkWebBrowserSCPEmulator.FindNextText("Warning:", true, true);
			}
			else if( e.Button == toolBarButtonLeft)
			{
				this.dvtkWebBrowserSCPEmulator.Back();
			}
			else if( e.Button == toolBarButtonRight)
			{
				this.dvtkWebBrowserSCPEmulator.Forward();
			}
            else if (e.Button == toolBarButtonValidation)
			{
                //toolBarButtonValidation.Enabled = !toolBarButtonValidation.Enabled;
                if (!toolBarButtonValidation.Pushed)
                {
                    toolBarButtonValidation.ToolTipText = "Validation enabled";
                    this.storageOptions.AutoValidate = true;
                    this.storageCommitOptions.AutoValidate = true;
                }
                else
                {
                    toolBarButtonValidation.ToolTipText = "Validation disabled";
                    this.storageOptions.AutoValidate = false;
                    this.storageCommitOptions.AutoValidate = false;
                }
			}
			else if( e.Button == toolBarButtonAbort)
			{
                //Abort the emulation
				try
				{
                    if (this.storageSCPDicomThread.HasBeenStarted)
                        this.storageSCPDicomThread.Stop();
					else
						MessageBox.Show( "There is no association exists.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				catch (Exception ex)
				{
					MessageBox.Show( "Couldn't Abort the SCP:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}			
			else{}
		}

        private void checkBoxSecurity_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxSecurity.Checked)
            {
                this.storageOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = true;
                this.groupBoxSecurity.Enabled = true;
            }
            else
            {
                this.storageOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = false;
                this.groupBoxSecurity.Enabled = false;
            }
        }

        private void checkBoxSecurityCommit_CheckedChanged(object sender, System.EventArgs e)
        {
            if (checkBoxSecurityCommit.Checked)
            {
                this.storageCommitOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = true;
                this.groupBoxSecurityCommit.Enabled = true;
            }
            else
            {
                this.storageCommitOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = false;
                this.groupBoxSecurityCommit.Enabled = false;
            }
        }

        private void comboboxMaxTlsVersion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Dvtk.Sessions.ISecuritySettings theISecuritySettings = this.storageOptions.DvtkScriptSession.SecuritySettings;


            switch (comboboxMaxTlsVersion.SelectedIndex)
            {
                case 0:
                    updateMaxTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0, theISecuritySettings);
                    break;
                case 1:
                    updateMaxTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1, theISecuritySettings);
                    break;
                case 2:
                    updateMaxTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2, theISecuritySettings);
                    break;
                case 3:
                    updateMaxTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3, theISecuritySettings);
                    break;
            }
        }

        private void updateMaxTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags theVersionFlag, Dvtk.Sessions.ISecuritySettings theISecuritySettings)
        {
            Dvtk.Sessions.TlsVersionFlags currentMaxVersion = theISecuritySettings.MaxTlsVersionFlags;
            Dvtk.Sessions.TlsVersionFlags currentMinVersion = theISecuritySettings.MinTlsVersionFlags;
            if (theVersionFlag < currentMinVersion)
            {
                comboboxMaxTlsVersion.SelectedIndex = (int)currentMaxVersion;
                MessageBox.Show("Max version cannot be smaller than Min version.");
            }
            else
            {
                theISecuritySettings.MaxTlsVersionFlags = theVersionFlag;
            }
        }

        private void comboboxMinTlsVersion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Dvtk.Sessions.ISecuritySettings theISecuritySettings = this.storageOptions.DvtkScriptSession.SecuritySettings;

            switch (comboboxMinTlsVersion.SelectedIndex)
            {
                case 0:
                    updateMinTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0, theISecuritySettings);
                    break;
                case 1:
                    updateMinTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1, theISecuritySettings);
                    break;
                case 2:
                    updateMinTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2, theISecuritySettings);
                    break;
                case 3:
                    updateMinTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3, theISecuritySettings);
                    break;
            }
        }

        private void updateMinTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags theVersionFlag, Dvtk.Sessions.ISecuritySettings theISecuritySettings)
        {
            Dvtk.Sessions.TlsVersionFlags currentMaxVersion = theISecuritySettings.MaxTlsVersionFlags;
            Dvtk.Sessions.TlsVersionFlags currentMinVersion = theISecuritySettings.MinTlsVersionFlags;
            if (theVersionFlag > currentMaxVersion)
            {
                comboboxMinTlsVersion.SelectedIndex = (int)currentMinVersion;
                MessageBox.Show("Min version cannot be bigger than Max version.");
            }
            else
            {
                theISecuritySettings.MinTlsVersionFlags = theVersionFlag;
            }
        }

        private void updateMaxTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags theVersionFlag, Dvtk.Sessions.ISecuritySettings theISecuritySettings)
        {
            Dvtk.Sessions.TlsVersionFlags currentMaxVersion = theISecuritySettings.MaxTlsVersionFlags;
            Dvtk.Sessions.TlsVersionFlags currentMinVersion = theISecuritySettings.MinTlsVersionFlags;
            if (theVersionFlag < currentMinVersion)
            {
                comboboxMaxTlsVersionCommit.SelectedIndex = (int)currentMaxVersion;
                MessageBox.Show("Max version cannot be smaller than Min version.");
            }
            else
            {
                theISecuritySettings.MaxTlsVersionFlags = theVersionFlag;
            }
        }

        private void updateMinTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags theVersionFlag, Dvtk.Sessions.ISecuritySettings theISecuritySettings)
        {
            Dvtk.Sessions.TlsVersionFlags currentMaxVersion = theISecuritySettings.MaxTlsVersionFlags;
            Dvtk.Sessions.TlsVersionFlags currentMinVersion = theISecuritySettings.MinTlsVersionFlags;
            if (theVersionFlag > currentMaxVersion)
            {
                comboboxMinTlsVersionCommit.SelectedIndex = (int)currentMinVersion;
                MessageBox.Show("Min version cannot be bigger than Max version.");
            }
            else
            {
                theISecuritySettings.MinTlsVersionFlags = theVersionFlag;
            }
        }

        private void comboboxMaxTlsVersionCommit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Dvtk.Sessions.ISecuritySettings theISecuritySettings = this.storageCommitOptions.DvtkScriptSession.SecuritySettings;


            switch (comboboxMaxTlsVersionCommit.SelectedIndex)
            {
                case 0:
                    updateMaxTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0, theISecuritySettings);
                    break;
                case 1:
                    updateMaxTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1, theISecuritySettings);
                    break;
                case 2:
                    updateMaxTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2, theISecuritySettings);
                    break;
                case 3:
                    updateMaxTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3, theISecuritySettings);
                    break;
            }
        }

        private void comboboxMinTlsVersionCommit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Dvtk.Sessions.ISecuritySettings theISecuritySettings = this.storageCommitOptions.DvtkScriptSession.SecuritySettings;

            switch (comboboxMinTlsVersionCommit.SelectedIndex)
            {
                case 0:
                    updateMinTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_0, theISecuritySettings);
                    break;
                case 1:
                    updateMinTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_1, theISecuritySettings);
                    break;
                case 2:
                    updateMinTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_2, theISecuritySettings);
                    break;
                case 3:
                    updateMinTlsVersionFlagCommit(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1_3, theISecuritySettings);
                    break;
            }
        }

        private void buttonCertificate_Click(object sender, EventArgs e)
        {
            OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer";

            theOpenFileDialog.Title = "Select the file containing the SUT Public Keys (certificates)";

            theOpenFileDialog.CheckFileExists = false;

            // Only if the current file exists, set this file in the file browser.
            if (textBoxCertificate.Text != "")
            {
                if (File.Exists(textBoxCertificate.Text))
                {
                    theOpenFileDialog.FileName = textBoxCertificate.Text;
                }
            }

            if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxCertificate.Text = theOpenFileDialog.FileName;
                this.storageOptions.DvtkScriptSession.SecuritySettings.CertificateFileName = theOpenFileDialog.FileName;
            }
        }

        private void buttonCertificateCommit_Click(object sender, EventArgs e)
        {
            OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer";

            theOpenFileDialog.Title = "Select the file containing the SUT Public Keys (certificates)";

            theOpenFileDialog.CheckFileExists = false;

            // Only if the current file exists, set this file in the file browser.
            if (textBoxCertificateCommit.Text != "")
            {
                if (File.Exists(textBoxCertificateCommit.Text))
                {
                    theOpenFileDialog.FileName = textBoxCertificateCommit.Text;
                }
            }

            if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxCertificateCommit.Text = theOpenFileDialog.FileName;
                this.storageCommitOptions.DvtkScriptSession.SecuritySettings.CertificateFileName = theOpenFileDialog.FileName;
            }
        }

        private void buttonCredential_Click(object sender, EventArgs e)
        {
            OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer";

            theOpenFileDialog.Title = "Select the file containing the DVT Private Keys (credentials)";

            theOpenFileDialog.CheckFileExists = false;

            // Only if the current file exists, set this file in the file browser.
            if (textBoxCredential.Text != "")
            {
                if (File.Exists(textBoxCredential.Text))
                {
                    theOpenFileDialog.FileName = textBoxCredential.Text;
                }
            }

            if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxCredential.Text = theOpenFileDialog.FileName;
                this.storageOptions.DvtkScriptSession.SecuritySettings.CredentialsFileName = theOpenFileDialog.FileName;
            }
        }

        private void buttonCredentialCommit_Click(object sender, EventArgs e)
        {
            OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer";

            theOpenFileDialog.Title = "Select the file containing the DVT Private Keys (credentials)";

            theOpenFileDialog.CheckFileExists = false;

            // Only if the current file exists, set this file in the file browser.
            if (textBoxCredentialCommit.Text != "")
            {
                if (File.Exists(textBoxCredentialCommit.Text))
                {
                    theOpenFileDialog.FileName = textBoxCredentialCommit.Text;
                }
            }

            if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxCredentialCommit.Text = theOpenFileDialog.FileName;
                this.storageCommitOptions.DvtkScriptSession.SecuritySettings.CredentialsFileName = theOpenFileDialog.FileName;
            }
        }
        private void buttonSpecifyTS_Click(object sender, EventArgs e)
        {
            SelectTransferSyntaxesForm theSelectTransferSyntaxesForm = new SelectTransferSyntaxesForm();

            ArrayList tsList = new ArrayList();
            tsList.Add(DvtkData.Dul.TransferSyntax.Implicit_VR_Little_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.Explicit_VR_Big_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Baseline_Process_1);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Extended_Hierarchical_16_And_18);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Extended_Hierarchical_17_And_19);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Extended_Process_2_And_4);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Extended_Process_3_And_5);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Hierarchical_24_And_26);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Hierarchical_25_And_27);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Non_Hierarchical_10_And_12);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Non_Hierarchical_11_And_13);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Lossless_Hierarchical_28);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Lossless_Hierarchical_29);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Lossless_Non_Hierarchical_14);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Lossless_Non_Hierarchical_15);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Lossless_Non_Hierarchical_1st_Order_Prediction);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_LS_Lossless_Image_Compression);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_LS_Lossy_Image_Compression);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_2000_IC_Lossless_Only);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_2000_IC);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Hierarchical_20_And_22);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Hierarchical_21_And_23);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Non_Hierarchical_6_And_8);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Non_Hierarchical_7_And_9);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_2000_Multicomponent_lossless2);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPEG_2000_Multicomponent2);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPIP_Referenced);
            tsList.Add(DvtkData.Dul.TransferSyntax.JPIP_Referenced_Deflate);
            tsList.Add(DvtkData.Dul.TransferSyntax.MPEG2_Main_Profile_Level);
            tsList.Add(DvtkData.Dul.TransferSyntax.MPEG2_High_Profile_Level);
            tsList.Add(DvtkData.Dul.TransferSyntax.RFC_2557_Mime_Encapsulation);
            tsList.Add(DvtkData.Dul.TransferSyntax.XML_Encoding);
            tsList.Add(DvtkData.Dul.TransferSyntax.Deflated_Explicit_VR_Little_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.RLE_Lossless);
            tsList.Add(DvtkData.Dul.TransferSyntax.MPEG4_AVC_H_264_High_Profile_Level_4_1);
            tsList.Add(DvtkData.Dul.TransferSyntax.MPEG4_AVC_H_264_BD_compatible_High_Profile_Level_4_1);
            tsList.Add(DvtkData.Dul.TransferSyntax.MPEG4_AVC_H_264_High_Profile_Level_4_2_For_2D_Video);
            tsList.Add(DvtkData.Dul.TransferSyntax.MPEG4_AVC_H_264_High_Profile_Level_4_2_For_3D_Video);
            tsList.Add(DvtkData.Dul.TransferSyntax.MPEG4_AVC_H_264_Stereo_High_Profile_Level_4_2);
            tsList.Add(DvtkData.Dul.TransferSyntax.HEVC_H_265_Main_Profile_Level_5_1);
            tsList.Add(DvtkData.Dul.TransferSyntax.HEVC_H_265_Main_10_Profile_Level_5_1);

            if (selectedTS.Count != 0)
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
            theSelectTransferSyntaxesForm.DisplaySelectAllButton = true;
            theSelectTransferSyntaxesForm.DisplayDeSelectAllButton = true;

            theSelectTransferSyntaxesForm.selectSupportedTS();
            theSelectTransferSyntaxesForm.ShowDialog(this);

            if (theSelectTransferSyntaxesForm.SupportedTransferSyntaxes.Count == 0)
            {
                selectedTS.Clear();

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

        private void buttonCommitTS_Click(object sender, EventArgs e)
        {
            SelectTransferSyntaxesForm theSelectTransferSyntaxesForm = new SelectTransferSyntaxesForm();

            ArrayList tsList = new ArrayList();
            tsList.Add(DvtkData.Dul.TransferSyntax.Implicit_VR_Little_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.Explicit_VR_Big_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian);

            if (selectedTSCommit.Count != 0)
            {
                foreach (string ts in selectedTSCommit)
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
            theSelectTransferSyntaxesForm.ShowDialog(this);

            if (theSelectTransferSyntaxesForm.SupportedTransferSyntaxes.Count == 0)
            {
                selectedTSCommit.Clear();

                string theWarningText = "No Transfer Syntax is selected, default ILE will be supported.";
                MessageBox.Show(theWarningText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                selectedTSCommit.Clear();

                foreach (DvtkData.Dul.TransferSyntax ts in theSelectTransferSyntaxesForm.SupportedTransferSyntaxes)
                {
                    selectedTSCommit.Add(ts.UID);
                }
            }
        }

		private void dvtkWebBrowserSCPEmulator_BackwardFormwardEnabledStateChangeEvent()
		{
			this.toolBarButtonLeft.Enabled = this.dvtkWebBrowserSCPEmulator.IsBackwardEnabled;
			this.toolBarButtonRight.Enabled = this.dvtkWebBrowserSCPEmulator.IsForwardEnabled;
		}

        void dvtkWebBrowserSCPEmulator_ErrorWarningEnabledStateChangeEvent()
        {
            toolBarButtonError.Enabled = this.dvtkWebBrowserSCPEmulator.ContainsErrors;
            toolBarButtonWarning.Enabled = this.dvtkWebBrowserSCPEmulator.ContainsWarnings;
        }

		private void Cleanup()
		{
			//Deleting all temporary pix files from result and data directories
            ArrayList theFilesToRemove = new ArrayList();
            DirectoryInfo theResultDirectoryInfo = new DirectoryInfo(validationResultsFileGroup.Directory);
            DirectoryInfo theDataDirectoryInfo = new DirectoryInfo(receivedDicomMessagesFileGroup.Directory);

            //String tempDir = Path.Combine(Path.GetTempPath(), "DVTkStorageSCP");
            //DirectoryInfo theTempDirInfo = new DirectoryInfo(tempDir); 

            if (theResultDirectoryInfo.Exists)
            {
                FileInfo[] dcmTempFiles = theResultDirectoryInfo.GetFiles("*.dcm");
                FileInfo[] pixTempFiles = theResultDirectoryInfo.GetFiles("*.pix");
                FileInfo[] idxTempFiles = theResultDirectoryInfo.GetFiles("*.idx");

                foreach (FileInfo theFileInfo in dcmTempFiles)
                {
                    string thedcmFileName = theFileInfo.FullName;

                    theFilesToRemove.Add(thedcmFileName);
                }

                foreach (FileInfo theFileInfo in pixTempFiles)
                {
                    string thePixFileName = theFileInfo.FullName;

                    theFilesToRemove.Add(thePixFileName);
                }

                foreach (FileInfo theFileInfo in idxTempFiles)
                {
                    string theIdxFileName = theFileInfo.FullName;

                    theFilesToRemove.Add(theIdxFileName);
                }
            }

            if (theDataDirectoryInfo.Exists)
            {
                FileInfo[] pixTempFiles = theDataDirectoryInfo.GetFiles("*.pix");

                foreach (FileInfo theFileInfo in pixTempFiles)
                {
                    string thePixFileName = theFileInfo.FullName;

                    theFilesToRemove.Add(thePixFileName);
                }                
            }

            //if (theTempDirInfo.Exists)
            //{
            //    theTempDirInfo.Delete(true);
            //}

			//Delete all pix, dcm & idx files
            foreach (string theFileName in theFilesToRemove)
            {
                if (File.Exists(theFileName))
                {
                    try
                    {
                        File.Delete(theFileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Couldn't remove the file :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
		}
	
		private void numericStorageSCP_ValueChanged(object sender, System.EventArgs e)
		{
            this.storageOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (uint)numericStorageSCP.Value;
		}

		private void numericCommitSCP_ValueChanged(object sender, System.EventArgs e)
		{
            this.storageCommitOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (uint)numericCommitSCP.Value;
		}

		private void buttonCommitPing_Click(object sender, System.EventArgs e)
		{
			string msg = "";
			labelPing.Text = "";
            PingReply reply = null;
            bool ok = false;
			if ((textBoxStorageCommitIPAdd.Text != null) && (textBoxStorageCommitIPAdd.Text.Length != 0))
			{
                string ipAddr = "";
				try
				{
					ipAddr = textBoxStorageCommitIPAdd.Text.Trim();
					
                    Ping pingSender = new Ping();
                    reply = pingSender.Send(ipAddr, 4);                    
				}
				catch(PingException exp)
				{
                    msg = string.Format("Error in pinging to {0} due to exception:{1}", ipAddr, exp);
				}
			}
			else
				msg = "Pl Specify SCP IP Address.";

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
                labelPing.Text = msg;
            else
            {
                labelPing.Text = "Ping failed.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

		private void buttonCommitEcho_Click(object sender, System.EventArgs e)
		{
			labelEcho.Text = "";

            if ((textBoxStorageCommitIPAdd.Text == null) || (textBoxStorageCommitIPAdd.Text.Length == 0))
            {
                MessageBox.Show("Pl Specify SCP IP Address.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SCU storageScu = new SCU();

            storageScu.Initialize(this.threadManager);

            storageScu.Options.Identifier = "CommitSCPEchoMessage";

            storageScu.Options.ResultsFileNameOnlyWithoutExtension = "CommitSCPCEcho_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            storageScu.Options.ResultsDirectory = this.storageOptions.ResultsDirectory;

            storageScu.Options.LocalAeTitle = textBoxCommitSCPAETitle.Text;

            storageScu.Options.RemoteAeTitle = textBoxStorageCommitAETitle.Text;
            storageScu.Options.RemotePort = UInt16.Parse(textBoxStorageCommitPort.Text);
            storageScu.Options.RemoteHostName = textBoxStorageCommitIPAdd.Text;
            storageScu.Options.AutoValidate = false;
            this.userControlActivityLogging.Attach(storageScu);

            PresentationContext presentationContext = new PresentationContext("1.2.840.10008.1.1", // Abstract Syntax Name
                                                                            "1.2.840.10008.1.2"); // Transfer Syntax Name(s)
            PresentationContext[] presentationContexts = new PresentationContext[1];
            presentationContexts[0] = presentationContext;

            DicomMessage echoMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORQ);

            storageScu.Start();

            bool sendResult = storageScu.TriggerSendAssociationAndWait(echoMessage, presentationContexts);

            if (!sendResult)
            {
                labelEcho.Text = "DICOM Echo failed (See logging for details)";
            }
            else
                labelEcho.Text = "DICOM Echo successful";

            storageScu.Stop();            
		}

		private ArrayList GetFilesRecursively(DirectoryInfo directory) 
		{
			ArrayList allDCMFiles = new ArrayList();
			try
			{
				// Get all the subdirectories/files
				FileSystemInfo[] infos = directory.GetFileSystemInfos();
				foreach (FileSystemInfo f in infos)
				{
					if (f is FileInfo) 
					{
                        if((f.Extension != ".pix") && (f.Extension != ".idx"))
                        {
                            if ((f.Name.ToLower() == "dicomdir")&&
                                ((f.Extension == null) || (f.Extension == "")))
						    {
							     //do nothing.
						    }
						    else 
						    {
							    allDCMFiles.Add (f.FullName);
						    }
                        }
					} 
					else 
					{
						allDCMFiles.AddRange(GetFilesRecursively((DirectoryInfo)f));
					}
				}
			}
			catch ( Exception exp)
			{
				MessageBox.Show( "Error in recursing:" + exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return allDCMFiles;
		}

        private void menuItemOptionsStoredFiles_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = fileGroups.ShowDialogFileGroupsConfigurator();

            if (dialogResult == DialogResult.OK)
            {
                this.storageOptions.ResultsDirectory = validationResultsFileGroup.Directory;
                this.storageOptions.DataDirectory = receivedDicomMessagesFileGroup.Directory;

                this.storageCommitOptions.ResultsDirectory = validationResultsFileGroup.Directory;
                this.storageCommitOptions.DataDirectory = receivedDicomMessagesFileGroup.Directory;

                // Make sure the session files contain the same information as the Stored Files user settings.
                SaveToSessionFiles(this.storageOptions, this.storageCommitOptions);
            }
        }

        private void menuItemStoredFilesExploreValidationResults_Click(object sender, EventArgs e)
        {
            validationResultsFileGroup.Explore();
        }

        private void menuItemStoredFilesExploreReceivedDicomMessages_Click(object sender, EventArgs e)
        {
            receivedDicomMessagesFileGroup.Explore();
        }

        private void menuItemCreateDicomdirForReceivedDicomMessages_Click(object sender, EventArgs e)
        {
            DirectoryInfo theDataDirectoryInfo = new DirectoryInfo(receivedDicomMessagesFileGroup.Directory);
            if (theDataDirectoryInfo.Exists)
            {
                ArrayList dataFiles = GetFilesRecursively(theDataDirectoryInfo);
                if (dataFiles.Count != 0)
                {
                    try
                    {
                        DvtkSession.MediaSession mediaSession = DvtkSession.MediaSession.LoadFromFile(documentsRootPath + "\\media.ses");
                        mediaSession.DataDirectory = receivedDicomMessagesFileGroup.Directory;
                        mediaSession.ResultsRootDirectory = receivedDicomMessagesFileGroup.Directory; //validationResultsFileGroup.Directory;

                        // Copy all selected DCM files to directory "DICOM" in a directory selected by user.
                        int i = 0;

                        DirectoryInfo theDIOCMDirectoryInfo = theDIOCMDirectoryInfo = new DirectoryInfo(Path.Combine(receivedDicomMessagesFileGroup.Directory, "DICOM"));

                        string[] dcmFiles = new string[dataFiles.Count];

                        // Create "DICOM" directory if it doesn't exist
                        if (!theDIOCMDirectoryInfo.Exists)
                        {
                            theDIOCMDirectoryInfo.Create();
                        }
                        else
                        {
                            // Remove existing DCM files from "DICOM" directory
                            FileInfo[] files = theDIOCMDirectoryInfo.GetFiles();
                            foreach (FileInfo file in files)
                            {
                                file.Delete();
                            }
                        }

                        foreach (string dcmFileName in dataFiles)
                        {
                            FileInfo dcmFile = new FileInfo(dcmFileName);
                            string fileName = string.Format("I{0:00000}", i); //dcmFile.Name.Replace(".dcm","");
                            string destFileName = theDIOCMDirectoryInfo.FullName + "\\" + fileName;
                            dcmFile.MoveTo(destFileName);
                            dcmFiles.SetValue(destFileName, i);
                            i++;
                        }

                        if (mediaSession.GenerateDICOMDIR(dcmFiles))
                        {
                            string msg = string.Format("DICOMDIR is created successfully in {0}", receivedDicomMessagesFileGroup.Directory);
                            MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        //Deleting all temporary files/directories
                        FileSystemInfo[] infos = theDataDirectoryInfo.GetFileSystemInfos();

                        if (infos.Length != 0)
                        {
                            try
                            {
                                foreach (FileSystemInfo f in infos)
                                {
                                    if (f is FileInfo)
                                    {
                                        FileInfo file = (FileInfo)f;
                                        if (file.Name != "DICOMDIR")
                                            f.Delete();
                                    }

                                    if (f is DirectoryInfo)
                                    {
                                        DirectoryInfo dir = (DirectoryInfo)f;
                                        if (dir.Name != "DICOM")
                                            dir.Delete(true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Couldn't remove the File/Directory :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to create the DICOMDIR :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("There is no dicom file to create DICOMDIR", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                theDataDirectoryInfo.Create();
            }
        }

        private void StorageSCPEmulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save the current config
            UpdateConfig();

            if (!isStopped)
            {
                //Stop the emulator
                try
                {
                    //emulatorSession.TerminateConnection();
                    this.threadManager.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't Stop the SCP:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Cleanup();
            fileGroups.RemoveFiles();
        }

        private void menuItemStoredFilesRemoveAllReceivedDicomMessages_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to remove all received DICOM messages?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (dialogResult == DialogResult.Yes)
            {
                receivedDicomMessagesFileGroup.RemoveAllFiles();

                DirectoryInfo theDataDirectoryInfo = new DirectoryInfo(receivedDicomMessagesFileGroup.Directory);

                if (theDataDirectoryInfo.Exists)
                {
                    FileInfo[] idxTempFiles = theDataDirectoryInfo.GetFiles("*.idx");

                    foreach (FileInfo theFileInfo in idxTempFiles)
                    {
                        string theIdxFileName = theFileInfo.FullName;

                        try
                        {
                            File.Delete(theIdxFileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't remove the file :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }                
	}
}
