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
using System.Xml;
using System.Diagnostics;

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
using DvtkHighLevelInterface.Dicom.Files;
using DvtkHighLevelInterface.InformationModel;
using Dvtk.IheActors.Bases;
using Dvtk.DvtkDicomEmulators.Bases;

namespace StorageSCUEmulator
{
	using DvtkSession = Dvtk.Sessions;
    using HLI = DvtkHighLevelInterface.Dicom.Other;
    using System.Collections.Generic;

	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class StorageSCUEmulator : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenuStorageSCU;
		private System.Windows.Forms.TabControl tabControlStorageSCU;
		private System.Windows.Forms.TabPage tabPageResults;
        private System.Windows.Forms.TabPage tabPageLogging;
		private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkWebBrowserSCUEmulator;
		private System.Windows.Forms.Panel panelSCPSettings;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelMaxTlsVersion;
        private System.Windows.Forms.Label labelMinTlsVersion;
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
        private System.Windows.Forms.ToolBar toolBarSCUEmulator;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.MenuItem menuItemSelectFiles;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItemAboutEmulator;
		private System.Windows.Forms.MenuItem menuItemStorageCommit;
		private System.Windows.Forms.ImageList imageListStorageSCU;
		private System.Windows.Forms.ToolBarButton toolBarButtonStoreImages;
		private System.Windows.Forms.ToolBarButton toolBarButtonStoreCommit;
		private System.Windows.Forms.ToolBarButton toolBarButtonError;
		private System.Windows.Forms.ToolBarButton toolBarButtonWarning;
		private System.Windows.Forms.ToolBarButton toolBarButtonLeft;
		private System.Windows.Forms.ToolBarButton toolBarButtonRight;
		private System.Windows.Forms.ToolBarButton toolBarButtonStop;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabPage tabPageStorageConfig;
		private System.Windows.Forms.TextBox textBoxStorageSCPPort;
		private System.Windows.Forms.TextBox textBoxStorageSCPIPAdd;
		private System.Windows.Forms.TextBox textBoxStorageSCPAETitle;
		private System.Windows.Forms.TextBox textBoxCommitSCPPort;
		private System.Windows.Forms.TextBox textBoxCommitSCPIPAddr;
		private System.Windows.Forms.TabPage tabPageCommitConfig;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox textBoxCommitSCPAETitle;
		private System.Windows.Forms.Button buttonEcho;
		private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NumericStorageMaxPDU;
        private System.Windows.Forms.TextBox textBoxSCUAETitle;
		private System.Windows.Forms.CheckBox checkBoxSecurity;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown NumericCommitMaxPDU;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxCommitSCUPort;
		private System.Windows.Forms.TextBox textBoxCommitSCUAETitle;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button buttonCommitEcho;
		private System.Windows.Forms.Button buttonCommitPing;
        private System.Windows.Forms.ToolTip toolTipSCU;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label labelStoragePing;
		private System.Windows.Forms.Label labelCommitPing;
		private System.Windows.Forms.MenuItem menuItemLoad;
		private System.Windows.Forms.MenuItem menuItemSave;
		private System.Windows.Forms.MenuItem menuItemConfig;
		private System.Windows.Forms.ToolBarButton toolBarButtonAbort;
		private System.Windows.Forms.Label labelStorageEcho;
		private System.Windows.Forms.Label labelCommitEcho;
		private System.Windows.Forms.MenuItem menuItemExportDir;
		private System.Windows.Forms.MenuItem menuItemExportFiles;
        private System.Windows.Forms.MenuItem menuItemExportDICOMDIR;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private TextBox textBoxImplClassUID;
        private Label label1;
        private ComboBox textBoxCommitSCUIPAdd;
        private Button specifyTS;
        private GroupBox GroupBoxNrAssociations;
        private RadioButton RadioButtonSingleAssoc;
        private RadioButton RadioButtonMultAssoc;
        private ToolBarButton toolBarButtonResult;
        private BackgroundWorker backgroundWorkerSCU;
        private CheckBox checkBoxStoreCommit;
        private MenuItem menuItemStoredFiles;
        private MenuItem menuItemStoredFilesExploreValidationResults;
        private MenuItem menuItem1;
        private MenuItem menuItemStoredFilesOptions;

		string applDirectory = Application.StartupPath;

        private string lastSelectedDir = Application.StartupPath;

        /// <summary>
        /// The options for Storage as SCU.
        /// </summary>
        private DicomThreadOptions storageOptions = null;
        private DicomThreadOptions storageCommitSCUOptions = null;

        /// <summary>
        /// Parent thread of the other threads.
        /// </summary>
        private OverviewThread overviewThread = null;

        /// <summary>
        /// The ThreadManager of the latest constructed OverviewThread.
        /// </summary>
        private ThreadManager threadManager = null;

        public static bool allThreadsFinished = false;

        /// <summary>
        /// Used to lock the overview thread
        /// </summary>
        public static Object lockObject = new Object();

        /// <summary>
        /// The handler that handles thread state change events of the ThreadManager that is constructed for the Overview Thread.
        /// </summary>
        private ThreadManager.ThreadsStateChangeEventHandler threadsStateChangeEventHandler = null;

        /// <summary>
        /// Needed to be able to jump to the main results.
        /// </summary>
        private String topXmlResults = "";

        private ArrayList selectedTS = new ArrayList();

        private ReferencedSopItemCollection storageCommitItems = null;

        private UserControlActivityLogging userControlActivityLogging;

        /// <summary>
        /// The handler that will be called when all threads have stopped.
        /// </summary>
        private delegate void ExecutionCompletedHandler();

        public delegate void UpdateUIControlsDelegate();

        private UpdateUIControlsDelegate updateUIControlsHandler = null;

        public delegate void UpdateUIControlsFromBGThreadDelegate();

        private UpdateUIControlsFromBGThreadDelegate updateUIControlsFromBGThreadHandler = null;

        Int16 delay = 0;
                        
        bool isStopped = true;
        bool isImageExported = false;

        private FileGroups fileGroups = null;

        private ValidationResultsFileGroup validationResultsFileGroup = null;
        private CheckBox checkBoxTS;
        private GroupBox groupBoxTS;
        private Label label8;
        private RadioButton radioButtonEBE;
        private RadioButton radioButtonELE;
        private RadioButton radioButtonILE;
        private TextBox textBoxDelay;

        private string sessionFolder;
        private ToolBarButton toolBarToggleValidation;

        bool isSingleAssoc = true;

        private bool autoValidate;

		public StorageSCUEmulator()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            InitializeBackgoundWorker();

            autoValidate = false;
	
			this.dvtkWebBrowserSCUEmulator.XmlStyleSheetFullFileName = applDirectory + "\\DVT_RESULTS.xslt";
			this.dvtkWebBrowserSCUEmulator.BackwardFormwardEnabledStateChangeEvent +=new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(dvtkWebBrowserSCUEmulator_BackwardFormwardEnabledStateChangeEvent);
            this.dvtkWebBrowserSCUEmulator.ErrorWarningEnabledStateChangeEvent += new DvtkWebBrowserNew.ErrorWarningEnabledStateChangeEventHandler(dvtkWebBrowserSCUEmulator_ErrorWarningEnabledStateChangeEvent);

            selectedTS.Add("1.2.840.10008.1.2");
            selectedTS.Add("1.2.840.10008.1.2.1");
            selectedTS.Add("1.2.840.10008.1.2.2");
            sessionFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\Storage SCU Emulator";
            //
            // Set the .Net thread name for debugging purposes.
            //
            System.Threading.Thread.CurrentThread.Name = "Storage SCU Emulator";

            this.threadsStateChangeEventHandler = new ThreadManager.ThreadsStateChangeEventHandler(this.HandleThreadsStateChangeEvent);

            updateUIControlsHandler = new UpdateUIControlsDelegate(UpdateUIControls);

            updateUIControlsFromBGThreadHandler = new UpdateUIControlsFromBGThreadDelegate(UpdateUIControlsFromBGThread);

            this.threadManager = new ThreadManager();
            this.threadManager.ThreadsStateChangeEvent += this.threadsStateChangeEventHandler;

            //
            // Stored files options.
            //
            this.fileGroups = new FileGroups("Storage SCU Emulator");

            this.validationResultsFileGroup = new ValidationResultsFileGroup();
            this.validationResultsFileGroup.DefaultFolder = "Results";
            this.fileGroups.Add(validationResultsFileGroup);

            this.fileGroups.CreateDirectories();

            this.fileGroups.CheckIsConfigured("\"Stored Files\\Options...\" menu item");

            // Set Storage thread
            StoreScu storageScu = new StoreScu();

            storageScu.Initialize(this.threadManager);
            this.storageOptions = storageScu.Options;
            this.storageOptions.LoadFromFile(Path.Combine(sessionFolder, "StorageSCUEmulator.ses"));
            this.storageOptions.StorageMode = Dvtk.Sessions.StorageMode.AsMediaOnly;
            this.storageOptions.ResultsDirectory = validationResultsFileGroup.Directory;
            this.storageOptions.DataDirectory = validationResultsFileGroup.Directory;

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
            this.textBoxCertificate.BackColor = File.Exists(this.textBoxCertificate.Text) ? SystemColors.Control : Color.Red;
            this.textBoxCredential.Text = this.storageOptions.DvtkScriptSession.SecuritySettings.CredentialsFileName;
            this.textBoxCredential.BackColor = File.Exists(this.textBoxCredential.Text) ? SystemColors.Control : Color.Red;
            //Environment.CurrentDirectory = this.storageOptions.ResultsDirectory;
            //Set the Commit thread
            CommitScu commitScu = new CommitScu(this, autoValidate);

            commitScu.Initialize(this.threadManager);
            this.storageCommitSCUOptions = commitScu.Options;
            this.storageCommitSCUOptions.LoadFromFile(Path.Combine(sessionFolder, "CommitSCUEmulator.ses"));
            this.storageCommitSCUOptions.ResultsDirectory = validationResultsFileGroup.Directory;
            this.storageCommitSCUOptions.DataDirectory = validationResultsFileGroup.Directory;
            if (this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled == true)
            {
                checkBoxSecurityCommit.Checked = true;
                groupBoxSecurityCommit.Enabled = true;
            }
            else
            {
                checkBoxSecurityCommit.Checked = false;
                groupBoxSecurityCommit.Enabled = false;
            }
            switch (this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.MaxTlsVersionFlags)
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
            switch (this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.MinTlsVersionFlags)
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
            this.textBoxCertificateCommit.Text = this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.CertificateFileName;
            this.textBoxCredentialCommit.Text = this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.CredentialsFileName;
            // Make sure the session files contain the same information as the Stored Files user settings.
            SaveToSessionFiles(this.storageOptions, this.storageCommitSCUOptions);

            this.storageCommitItems = new ReferencedSopItemCollection();                                
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StorageSCUEmulator));
            this.mainMenuStorageSCU = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemSelectFiles = new System.Windows.Forms.MenuItem();
            this.menuItemExportDir = new System.Windows.Forms.MenuItem();
            this.menuItemExportFiles = new System.Windows.Forms.MenuItem();
            this.menuItemExportDICOMDIR = new System.Windows.Forms.MenuItem();
            this.menuItemStorageCommit = new System.Windows.Forms.MenuItem();
            this.menuItemConfig = new System.Windows.Forms.MenuItem();
            this.menuItemLoad = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFiles = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesExploreValidationResults = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesOptions = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.menuItemAboutEmulator = new System.Windows.Forms.MenuItem();
            this.tabControlStorageSCU = new System.Windows.Forms.TabControl();
            this.tabPageStorageConfig = new System.Windows.Forms.TabPage();
            this.panelSCPSettings = new System.Windows.Forms.Panel();
            this.checkBoxTS = new System.Windows.Forms.CheckBox();
            this.groupBoxTS = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.radioButtonEBE = new System.Windows.Forms.RadioButton();
            this.radioButtonELE = new System.Windows.Forms.RadioButton();
            this.radioButtonILE = new System.Windows.Forms.RadioButton();
            this.GroupBoxNrAssociations = new System.Windows.Forms.GroupBox();
            this.RadioButtonSingleAssoc = new System.Windows.Forms.RadioButton();
            this.RadioButtonMultAssoc = new System.Windows.Forms.RadioButton();
            this.labelStorageEcho = new System.Windows.Forms.Label();
            this.labelStoragePing = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NumericStorageMaxPDU = new System.Windows.Forms.NumericUpDown();
            this.textBoxSCUAETitle = new System.Windows.Forms.TextBox();
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
            this.label7 = new System.Windows.Forms.Label();
            this.buttonEcho = new System.Windows.Forms.Button();
            this.buttonPing = new System.Windows.Forms.Button();
            this.textBoxStorageSCPPort = new System.Windows.Forms.TextBox();
            this.textBoxStorageSCPIPAdd = new System.Windows.Forms.TextBox();
            this.textBoxStorageSCPAETitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxStoreCommit = new System.Windows.Forms.CheckBox();
            this.textBoxImplClassUID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageCommitConfig = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelCommitEcho = new System.Windows.Forms.Label();
            this.labelCommitPing = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonCommitEcho = new System.Windows.Forms.Button();
            this.buttonCommitPing = new System.Windows.Forms.Button();
            this.NumericCommitMaxPDU = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.textBoxCommitSCUPort = new System.Windows.Forms.TextBox();
            this.textBoxCommitSCUAETitle = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxCommitSCPPort = new System.Windows.Forms.TextBox();
            this.textBoxCommitSCPIPAddr = new System.Windows.Forms.TextBox();
            this.textBoxCommitSCPAETitle = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.specifyTS = new System.Windows.Forms.Button();
            this.textBoxCommitSCUIPAdd = new System.Windows.Forms.ComboBox();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.dvtkWebBrowserSCUEmulator = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.tabPageLogging = new System.Windows.Forms.TabPage();
            this.userControlActivityLogging = new DvtkHighLevelInterface.Common.UserInterfaces.UserControlActivityLogging();
            this.toolBarSCUEmulator = new System.Windows.Forms.ToolBar();
            this.toolBarButtonStoreImages = new System.Windows.Forms.ToolBarButton();
            this.toolBarToggleValidation = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonStoreCommit = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonAbort = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonResult = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonError = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonWarning = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonLeft = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonRight = new System.Windows.Forms.ToolBarButton();
            this.imageListStorageSCU = new System.Windows.Forms.ImageList(this.components);
            this.toolTipSCU = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorkerSCU = new System.ComponentModel.BackgroundWorker();
            this.tabControlStorageSCU.SuspendLayout();
            this.tabPageStorageConfig.SuspendLayout();
            this.panelSCPSettings.SuspendLayout();
            this.groupBoxTS.SuspendLayout();
            this.GroupBoxNrAssociations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericStorageMaxPDU)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabPageCommitConfig.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericCommitMaxPDU)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabPageResults.SuspendLayout();
            this.tabPageLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStorageSCU
            // 
            this.mainMenuStorageSCU.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemStoredFiles,
            this.menuItemAbout});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSelectFiles,
            this.menuItemStorageCommit,
            this.menuItemConfig,
            this.menuItemExit});
            this.menuItemFile.Text = "File";
            // 
            // menuItemSelectFiles
            // 
            this.menuItemSelectFiles.Index = 0;
            this.menuItemSelectFiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemExportDir,
            this.menuItemExportFiles,
            this.menuItemExportDICOMDIR});
            this.menuItemSelectFiles.Text = "Export DICOM Data";
            // 
            // menuItemExportDir
            // 
            this.menuItemExportDir.Index = 0;
            this.menuItemExportDir.Text = "Select Source Directory";
            this.menuItemExportDir.Click += new System.EventHandler(this.menuItemExportDir_Click);
            // 
            // menuItemExportFiles
            // 
            this.menuItemExportFiles.Index = 1;
            this.menuItemExportFiles.Text = "Select DICOM files";
            this.menuItemExportFiles.Click += new System.EventHandler(this.menuItemExportFiles_Click);
            // 
            // menuItemExportDICOMDIR
            // 
            this.menuItemExportDICOMDIR.Index = 2;
            this.menuItemExportDICOMDIR.Text = "Select DICOMDIR";
            this.menuItemExportDICOMDIR.Click += new System.EventHandler(this.menuItemExportDICOMDIR_Click);
            // 
            // menuItemStorageCommit
            // 
            this.menuItemStorageCommit.Index = 1;
            this.menuItemStorageCommit.Text = "Send Storage Commit";
            this.menuItemStorageCommit.Click += new System.EventHandler(this.menuItemStorageCommit_Click);
            // 
            // menuItemConfig
            // 
            this.menuItemConfig.Index = 2;
            this.menuItemConfig.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLoad,
            this.menuItemSave});
            this.menuItemConfig.Text = "Config File";
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
            this.menuItem1,
            this.menuItemStoredFilesOptions});
            this.menuItemStoredFiles.Text = "Stored Files";
            // 
            // menuItemStoredFilesExploreValidationResults
            // 
            this.menuItemStoredFilesExploreValidationResults.Index = 0;
            this.menuItemStoredFilesExploreValidationResults.Text = "Explore Validation Results...";
            this.menuItemStoredFilesExploreValidationResults.Click += new System.EventHandler(this.menuItemStoredFilesExploreValidationResults_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // menuItemStoredFilesOptions
            // 
            this.menuItemStoredFilesOptions.Index = 2;
            this.menuItemStoredFilesOptions.Text = "Options...";
            this.menuItemStoredFilesOptions.Click += new System.EventHandler(this.menuItemStoredFilesOptions_Click);
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
            // tabControlStorageSCU
            // 
            this.tabControlStorageSCU.Controls.Add(this.tabPageStorageConfig);
            this.tabControlStorageSCU.Controls.Add(this.tabPageCommitConfig);
            this.tabControlStorageSCU.Controls.Add(this.tabPageResults);
            this.tabControlStorageSCU.Controls.Add(this.tabPageLogging);
            this.tabControlStorageSCU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlStorageSCU.Location = new System.Drawing.Point(0, 28);
            this.tabControlStorageSCU.Name = "tabControlStorageSCU";
            this.tabControlStorageSCU.SelectedIndex = 0;
            this.tabControlStorageSCU.Size = new System.Drawing.Size(812, 591);
            this.tabControlStorageSCU.TabIndex = 0;
            this.tabControlStorageSCU.SelectedIndexChanged += new System.EventHandler(this.tabControlStorageSCU_SelectedIndexChanged);
            // 
            // tabPageStorageConfig
            // 
            this.tabPageStorageConfig.Controls.Add(this.panelSCPSettings);
            this.tabPageStorageConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageStorageConfig.Name = "tabPageStorageConfig";
            this.tabPageStorageConfig.Size = new System.Drawing.Size(804, 565);
            this.tabPageStorageConfig.TabIndex = 0;
            this.tabPageStorageConfig.Text = "Storage Config";
            // 
            // panelSCPSettings
            // 
            this.panelSCPSettings.BackColor = System.Drawing.Color.Transparent;
            this.panelSCPSettings.Controls.Add(this.checkBoxTS);
            this.panelSCPSettings.Controls.Add(this.groupBoxTS);
            this.panelSCPSettings.Controls.Add(this.GroupBoxNrAssociations);
            this.panelSCPSettings.Controls.Add(this.labelStorageEcho);
            this.panelSCPSettings.Controls.Add(this.labelStoragePing);
            this.panelSCPSettings.Controls.Add(this.label6);
            this.panelSCPSettings.Controls.Add(this.NumericStorageMaxPDU);
            this.panelSCPSettings.Controls.Add(this.textBoxSCUAETitle);
            this.panelSCPSettings.Controls.Add(this.groupBoxSecurity);
            this.panelSCPSettings.Controls.Add(this.checkBoxSecurity);
            this.panelSCPSettings.Controls.Add(this.label7);
            this.panelSCPSettings.Controls.Add(this.buttonEcho);
            this.panelSCPSettings.Controls.Add(this.buttonPing);
            this.panelSCPSettings.Controls.Add(this.textBoxStorageSCPPort);
            this.panelSCPSettings.Controls.Add(this.textBoxStorageSCPIPAdd);
            this.panelSCPSettings.Controls.Add(this.textBoxStorageSCPAETitle);
            this.panelSCPSettings.Controls.Add(this.label4);
            this.panelSCPSettings.Controls.Add(this.label3);
            this.panelSCPSettings.Controls.Add(this.label2);
            this.panelSCPSettings.Controls.Add(this.groupBox1);
            this.panelSCPSettings.Controls.Add(this.groupBox2);
            this.panelSCPSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSCPSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSCPSettings.Name = "panelSCPSettings";
            this.panelSCPSettings.Size = new System.Drawing.Size(804, 565);
            this.panelSCPSettings.TabIndex = 0;
            // 
            // checkBoxTS
            // 
            this.checkBoxTS.AutoSize = true;
            this.checkBoxTS.Location = new System.Drawing.Point(368, 284);
            this.checkBoxTS.Name = "checkBoxTS";
            this.checkBoxTS.Size = new System.Drawing.Size(192, 17);
            this.checkBoxTS.TabIndex = 3;
            this.checkBoxTS.Text = "Enable Transfer Syntax Conversion";
            this.checkBoxTS.UseVisualStyleBackColor = true;
            this.checkBoxTS.CheckedChanged += new System.EventHandler(this.checkBoxTS_CheckedChanged);
            // 
            // groupBoxTS
            // 
            this.groupBoxTS.Controls.Add(this.label8);
            this.groupBoxTS.Controls.Add(this.radioButtonEBE);
            this.groupBoxTS.Controls.Add(this.radioButtonELE);
            this.groupBoxTS.Controls.Add(this.radioButtonILE);
            this.groupBoxTS.Enabled = false;
            this.groupBoxTS.Location = new System.Drawing.Point(368, 306);
            this.groupBoxTS.Name = "groupBoxTS";
            this.groupBoxTS.Size = new System.Drawing.Size(200, 143);
            this.groupBoxTS.TabIndex = 44;
            this.groupBoxTS.TabStop = false;
            this.groupBoxTS.Text = "Convert Dicom File to ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Enabled = false;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(188, 26);
            this.label8.TabIndex = 3;
            this.label8.Text = "(Conversion will happen only between \r\nILE, ELE and EBE)";
            // 
            // radioButtonEBE
            // 
            this.radioButtonEBE.AutoSize = true;
            this.radioButtonEBE.Location = new System.Drawing.Point(15, 75);
            this.radioButtonEBE.Name = "radioButtonEBE";
            this.radioButtonEBE.Size = new System.Drawing.Size(46, 17);
            this.radioButtonEBE.TabIndex = 2;
            this.radioButtonEBE.TabStop = true;
            this.radioButtonEBE.Text = "EBE";
            this.radioButtonEBE.UseVisualStyleBackColor = true;
            // 
            // radioButtonELE
            // 
            this.radioButtonELE.AutoSize = true;
            this.radioButtonELE.Location = new System.Drawing.Point(15, 52);
            this.radioButtonELE.Name = "radioButtonELE";
            this.radioButtonELE.Size = new System.Drawing.Size(45, 17);
            this.radioButtonELE.TabIndex = 1;
            this.radioButtonELE.TabStop = true;
            this.radioButtonELE.Text = "ELE";
            this.radioButtonELE.UseVisualStyleBackColor = true;
            // 
            // radioButtonILE
            // 
            this.radioButtonILE.AutoSize = true;
            this.radioButtonILE.Checked = true;
            this.radioButtonILE.Location = new System.Drawing.Point(15, 28);
            this.radioButtonILE.Name = "radioButtonILE";
            this.radioButtonILE.Size = new System.Drawing.Size(41, 17);
            this.radioButtonILE.TabIndex = 0;
            this.radioButtonILE.TabStop = true;
            this.radioButtonILE.Text = "ILE";
            this.radioButtonILE.UseVisualStyleBackColor = true;
            // 
            // GroupBoxNrAssociations
            // 
            this.GroupBoxNrAssociations.Controls.Add(this.RadioButtonSingleAssoc);
            this.GroupBoxNrAssociations.Controls.Add(this.RadioButtonMultAssoc);
            this.GroupBoxNrAssociations.Location = new System.Drawing.Point(368, 196);
            this.GroupBoxNrAssociations.Name = "GroupBoxNrAssociations";
            this.GroupBoxNrAssociations.Size = new System.Drawing.Size(154, 78);
            this.GroupBoxNrAssociations.TabIndex = 43;
            this.GroupBoxNrAssociations.TabStop = false;
            this.GroupBoxNrAssociations.Text = "Number of associations";
            // 
            // RadioButtonSingleAssoc
            // 
            this.RadioButtonSingleAssoc.Checked = true;
            this.RadioButtonSingleAssoc.Location = new System.Drawing.Point(16, 16);
            this.RadioButtonSingleAssoc.Name = "RadioButtonSingleAssoc";
            this.RadioButtonSingleAssoc.Size = new System.Drawing.Size(123, 24);
            this.RadioButtonSingleAssoc.TabIndex = 0;
            this.RadioButtonSingleAssoc.TabStop = true;
            this.RadioButtonSingleAssoc.Text = "Single association";
            // 
            // RadioButtonMultAssoc
            // 
            this.RadioButtonMultAssoc.Location = new System.Drawing.Point(16, 40);
            this.RadioButtonMultAssoc.Name = "RadioButtonMultAssoc";
            this.RadioButtonMultAssoc.Size = new System.Drawing.Size(129, 24);
            this.RadioButtonMultAssoc.TabIndex = 1;
            this.RadioButtonMultAssoc.Text = "Multiple associations";
            // 
            // labelStorageEcho
            // 
            this.labelStorageEcho.Location = new System.Drawing.Point(446, 96);
            this.labelStorageEcho.Name = "labelStorageEcho";
            this.labelStorageEcho.Size = new System.Drawing.Size(226, 32);
            this.labelStorageEcho.TabIndex = 40;
            // 
            // labelStoragePing
            // 
            this.labelStoragePing.Location = new System.Drawing.Point(447, 51);
            this.labelStoragePing.Name = "labelStoragePing";
            this.labelStoragePing.Size = new System.Drawing.Size(229, 33);
            this.labelStoragePing.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 330);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 29);
            this.label6.TabIndex = 16;
            this.label6.Text = "Max PDU Size Receive:";
            // 
            // NumericStorageMaxPDU
            // 
            this.NumericStorageMaxPDU.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.NumericStorageMaxPDU.Location = new System.Drawing.Point(167, 333);
            this.NumericStorageMaxPDU.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.NumericStorageMaxPDU.Name = "NumericStorageMaxPDU";
            this.NumericStorageMaxPDU.Size = new System.Drawing.Size(72, 20);
            this.NumericStorageMaxPDU.TabIndex = 4;
            // 
            // textBoxSCUAETitle
            // 
            this.textBoxSCUAETitle.Location = new System.Drawing.Point(165, 229);
            this.textBoxSCUAETitle.Name = "textBoxSCUAETitle";
            this.textBoxSCUAETitle.Size = new System.Drawing.Size(175, 20);
            this.textBoxSCUAETitle.TabIndex = 3;
            this.textBoxSCUAETitle.Text = "DVTK_STR_SCU";
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
            this.groupBoxSecurity.Location = new System.Drawing.Point(0, 475);
            this.groupBoxSecurity.Name = "groupBoxSecurity";
            this.groupBoxSecurity.Size = new System.Drawing.Size(600, 75);
            this.groupBoxSecurity.TabStop = false;
            this.groupBoxSecurity.Text = "Security Options ";
            // 
            // checkBoxSecurity
            // 
            this.checkBoxSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSecurity.Location = new System.Drawing.Point(8, 450);
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
            this.groupBoxSecurityCommit.Location = new System.Drawing.Point(0, 475);
            this.groupBoxSecurityCommit.Name = "groupBoxSecurity";
            this.groupBoxSecurityCommit.Size = new System.Drawing.Size(600, 75);
            this.groupBoxSecurityCommit.TabStop = false;
            this.groupBoxSecurityCommit.Text = "Security Options ";
            // 
            // checkBoxSecurityCommit
            // 
            this.checkBoxSecurityCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSecurityCommit.Location = new System.Drawing.Point(8, 450);
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
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 229);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 24);
            this.label7.TabIndex = 15;
            this.label7.Text = "Local AE Title:";
            // 
            // buttonEcho
            // 
            this.buttonEcho.Location = new System.Drawing.Point(360, 94);
            this.buttonEcho.Name = "buttonEcho";
            this.buttonEcho.Size = new System.Drawing.Size(80, 23);
            this.buttonEcho.TabIndex = 7;
            this.buttonEcho.Text = "DICOM Echo";
            this.buttonEcho.Click += new System.EventHandler(this.buttonEcho_Click);
            // 
            // buttonPing
            // 
            this.buttonPing.BackColor = System.Drawing.SystemColors.Control;
            this.buttonPing.Location = new System.Drawing.Point(360, 48);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(75, 23);
            this.buttonPing.TabIndex = 5;
            this.buttonPing.Text = "Ping SCP";
            this.buttonPing.UseVisualStyleBackColor = true;
            this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
            // 
            // textBoxStorageSCPPort
            // 
            this.textBoxStorageSCPPort.Location = new System.Drawing.Point(163, 144);
            this.textBoxStorageSCPPort.Name = "textBoxStorageSCPPort";
            this.textBoxStorageSCPPort.Size = new System.Drawing.Size(56, 20);
            this.textBoxStorageSCPPort.TabIndex = 2;
            this.textBoxStorageSCPPort.Text = "104";
            this.textBoxStorageSCPPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxStorageSCPPort_Validating);
            // 
            // textBoxStorageSCPIPAdd
            // 
            this.textBoxStorageSCPIPAdd.Location = new System.Drawing.Point(162, 96);
            this.textBoxStorageSCPIPAdd.Name = "textBoxStorageSCPIPAdd";
            this.textBoxStorageSCPIPAdd.Size = new System.Drawing.Size(136, 20);
            this.textBoxStorageSCPIPAdd.TabIndex = 1;
            this.textBoxStorageSCPIPAdd.Text = "localhost";
            // 
            // textBoxStorageSCPAETitle
            // 
            this.textBoxStorageSCPAETitle.Location = new System.Drawing.Point(161, 48);
            this.textBoxStorageSCPAETitle.Name = "textBoxStorageSCPAETitle";
            this.textBoxStorageSCPAETitle.Size = new System.Drawing.Size(179, 20);
            this.textBoxStorageSCPAETitle.TabIndex = 0;
            this.textBoxStorageSCPAETitle.Text = "DVTK_STR_SCP";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 14;
            this.label4.Text = "Remote Port:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "Remote TCP/IP Address:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Remote AE Title:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 176);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SCP Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxStoreCommit);
            this.groupBox2.Controls.Add(this.textBoxImplClassUID);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(0, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(354, 254);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SCU Settings";
            // 
            // checkBoxStoreCommit
            // 
            this.checkBoxStoreCommit.AutoSize = true;
            this.checkBoxStoreCommit.Checked = true;
            this.checkBoxStoreCommit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStoreCommit.Location = new System.Drawing.Point(6, 180);
            this.checkBoxStoreCommit.Name = "checkBoxStoreCommit";
            this.checkBoxStoreCommit.Size = new System.Drawing.Size(123, 17);
            this.checkBoxStoreCommit.TabIndex = 14;
            this.checkBoxStoreCommit.Text = "Storage Commitment";
            this.checkBoxStoreCommit.UseVisualStyleBackColor = true;
            this.checkBoxStoreCommit.CheckedChanged += new System.EventHandler(this.checkBoxStoreCommit_CheckedChanged);
            // 
            // textBoxImplClassUID
            // 
            this.textBoxImplClassUID.Location = new System.Drawing.Point(167, 85);
            this.textBoxImplClassUID.Name = "textBoxImplClassUID";
            this.textBoxImplClassUID.ReadOnly = true;
            this.textBoxImplClassUID.Size = new System.Drawing.Size(174, 20);
            this.textBoxImplClassUID.TabIndex = 12;
            this.textBoxImplClassUID.Text = "1.2.826.0.1.3680043.2.1545.4";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 21);
            this.label1.TabIndex = 13;
            this.label1.Text = "Implementation Class UID:";
            // 
            // tabPageCommitConfig
            // 
            this.tabPageCommitConfig.Controls.Add(this.panel1);
            this.tabPageCommitConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageCommitConfig.Name = "tabPageCommitConfig";
            this.tabPageCommitConfig.Size = new System.Drawing.Size(669, 486);
            this.tabPageCommitConfig.TabIndex = 3;
            this.tabPageCommitConfig.Text = "Storage Commitment Config";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.labelCommitEcho);
            this.panel1.Controls.Add(this.labelCommitPing);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.buttonCommitEcho);
            this.panel1.Controls.Add(this.buttonCommitPing);
            this.panel1.Controls.Add(this.NumericCommitMaxPDU);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBoxDelay);
            this.panel1.Controls.Add(this.textBoxCommitSCUPort);
            this.panel1.Controls.Add(this.textBoxCommitSCUAETitle);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.textBoxCommitSCPPort);
            this.panel1.Controls.Add(this.textBoxCommitSCPIPAddr);
            this.panel1.Controls.Add(this.textBoxCommitSCPAETitle);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.checkBoxSecurityCommit);
            this.panel1.Controls.Add(this.groupBoxSecurityCommit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 486);
            this.panel1.TabIndex = 1;
            // 
            // labelCommitEcho
            // 
            this.labelCommitEcho.Location = new System.Drawing.Point(467, 98);
            this.labelCommitEcho.Name = "labelCommitEcho";
            this.labelCommitEcho.Size = new System.Drawing.Size(210, 37);
            this.labelCommitEcho.TabIndex = 19;
            // 
            // labelCommitPing
            // 
            this.labelCommitPing.Location = new System.Drawing.Point(467, 51);
            this.labelCommitPing.Name = "labelCommitPing";
            this.labelCommitPing.Size = new System.Drawing.Size(210, 31);
            this.labelCommitPing.TabIndex = 18;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(207, 337);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(96, 32);
            this.label17.TabIndex = 17;
            this.label17.Text = "Commit max reply waiting time (sec)";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCommitEcho
            // 
            this.buttonCommitEcho.Location = new System.Drawing.Point(381, 96);
            this.buttonCommitEcho.Name = "buttonCommitEcho";
            this.buttonCommitEcho.Size = new System.Drawing.Size(80, 23);
            this.buttonCommitEcho.TabIndex = 9;
            this.buttonCommitEcho.Text = "DICOM Echo";
            this.buttonCommitEcho.Click += new System.EventHandler(this.buttonCommitEcho_Click);
            // 
            // buttonCommitPing
            // 
            this.buttonCommitPing.Location = new System.Drawing.Point(383, 48);
            this.buttonCommitPing.Name = "buttonCommitPing";
            this.buttonCommitPing.Size = new System.Drawing.Size(75, 23);
            this.buttonCommitPing.TabIndex = 8;
            this.buttonCommitPing.Text = "Ping SCP";
            this.buttonCommitPing.Click += new System.EventHandler(this.buttonCommitPing_Click);
            // 
            // NumericCommitMaxPDU
            // 
            this.NumericCommitMaxPDU.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.NumericCommitMaxPDU.Location = new System.Drawing.Point(152, 399);
            this.NumericCommitMaxPDU.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.NumericCommitMaxPDU.Name = "NumericCommitMaxPDU";
            this.NumericCommitMaxPDU.Size = new System.Drawing.Size(80, 20);
            this.NumericCommitMaxPDU.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 395);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "Max PDU Size:";
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(304, 345);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(40, 20);
            this.textBoxDelay.TabIndex = 6;
            this.textBoxDelay.Text = "0";
            this.toolTipSCU.SetToolTip(this.textBoxDelay, "The suggested values  are:  -1 --> no waiting time (asynchronous mode),  0 --> in" +
        "finite waiting time,  1 ..n ---> max waiting time = n sec");
            this.textBoxDelay.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDelay_Validating);
            // 
            // textBoxCommitSCUPort
            // 
            this.textBoxCommitSCUPort.Location = new System.Drawing.Point(152, 344);
            this.textBoxCommitSCUPort.Name = "textBoxCommitSCUPort";
            this.textBoxCommitSCUPort.Size = new System.Drawing.Size(50, 20);
            this.textBoxCommitSCUPort.TabIndex = 5;
            this.textBoxCommitSCUPort.Text = "115";
            this.textBoxCommitSCUPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxCommitSCUPort_Validating);
            // 
            // textBoxCommitSCUAETitle
            // 
            this.textBoxCommitSCUAETitle.Location = new System.Drawing.Point(152, 240);
            this.textBoxCommitSCUAETitle.Name = "textBoxCommitSCUAETitle";
            this.textBoxCommitSCUAETitle.Size = new System.Drawing.Size(192, 20);
            this.textBoxCommitSCUAETitle.TabIndex = 3;
            this.textBoxCommitSCUAETitle.Text = "DVTK_STRC_SCU";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 342);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 23);
            this.label9.TabIndex = 15;
            this.label9.Text = "Listen Port:";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(8, 288);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(144, 23);
            this.label10.TabIndex = 14;
            this.label10.Text = "Local TCP/IP Address:";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(8, 240);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 23);
            this.label11.TabIndex = 13;
            this.label11.Text = "Local AE Title:";
            // 
            // textBoxCommitSCPPort
            // 
            this.textBoxCommitSCPPort.Location = new System.Drawing.Point(152, 144);
            this.textBoxCommitSCPPort.Name = "textBoxCommitSCPPort";
            this.textBoxCommitSCPPort.Size = new System.Drawing.Size(48, 20);
            this.textBoxCommitSCPPort.TabIndex = 2;
            this.textBoxCommitSCPPort.Text = "105";
            this.textBoxCommitSCPPort.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxCommitSCPPort_Validating);
            // 
            // textBoxCommitSCPIPAddr
            // 
            this.textBoxCommitSCPIPAddr.Location = new System.Drawing.Point(152, 96);
            this.textBoxCommitSCPIPAddr.Name = "textBoxCommitSCPIPAddr";
            this.textBoxCommitSCPIPAddr.Size = new System.Drawing.Size(140, 20);
            this.textBoxCommitSCPIPAddr.TabIndex = 1;
            this.textBoxCommitSCPIPAddr.Text = "localhost";
            // 
            // textBoxCommitSCPAETitle
            // 
            this.textBoxCommitSCPAETitle.Location = new System.Drawing.Point(152, 48);
            this.textBoxCommitSCPAETitle.Name = "textBoxCommitSCPAETitle";
            this.textBoxCommitSCPAETitle.Size = new System.Drawing.Size(192, 20);
            this.textBoxCommitSCPAETitle.TabIndex = 0;
            this.textBoxCommitSCPAETitle.Text = "DVTK_STRC_SCP";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(8, 144);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 23);
            this.label13.TabIndex = 12;
            this.label13.Text = "Remote Port:";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(8, 96);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(144, 23);
            this.label14.TabIndex = 11;
            this.label14.Text = "Remote TCP/IP Address:";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(8, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 23);
            this.label15.TabIndex = 10;
            this.label15.Text = "Remote AE Title:";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(1, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(376, 184);
            this.groupBox3.TabIndex = 43;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SCP Settings";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.specifyTS);
            this.groupBox4.Controls.Add(this.textBoxCommitSCUIPAdd);
            this.groupBox4.Location = new System.Drawing.Point(2, 202);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(376, 248);
            this.groupBox4.TabIndex = 44;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SCU Settings";
            // 
            // specifyTS
            // 
            this.specifyTS.Location = new System.Drawing.Point(270, 195);
            this.specifyTS.Name = "specifyTS";
            this.specifyTS.Size = new System.Drawing.Size(75, 23);
            this.specifyTS.TabIndex = 1;
            this.specifyTS.Text = "Specify TS";
            this.specifyTS.UseVisualStyleBackColor = true;
            this.specifyTS.Click += new System.EventHandler(this.specifyTS_Click);
            // 
            // textBoxCommitSCUIPAdd
            // 
            this.textBoxCommitSCUIPAdd.FormattingEnabled = true;
            this.textBoxCommitSCUIPAdd.Location = new System.Drawing.Point(152, 91);
            this.textBoxCommitSCUIPAdd.Name = "textBoxCommitSCUIPAdd";
            this.textBoxCommitSCUIPAdd.Size = new System.Drawing.Size(193, 21);
            this.textBoxCommitSCUIPAdd.TabIndex = 0;
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.dvtkWebBrowserSCUEmulator);
            this.tabPageResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Size = new System.Drawing.Size(669, 486);
            this.tabPageResults.TabIndex = 1;
            this.tabPageResults.Text = "Validation Results";
            // 
            // dvtkWebBrowserSCUEmulator
            // 
            this.dvtkWebBrowserSCUEmulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvtkWebBrowserSCUEmulator.Location = new System.Drawing.Point(0, 0);
            this.dvtkWebBrowserSCUEmulator.Name = "dvtkWebBrowserSCUEmulator";
            this.dvtkWebBrowserSCUEmulator.Size = new System.Drawing.Size(669, 486);
            this.dvtkWebBrowserSCUEmulator.TabIndex = 0;
            this.dvtkWebBrowserSCUEmulator.XmlStyleSheetFullFileName = "";
            // 
            // tabPageLogging
            // 
            this.tabPageLogging.Controls.Add(this.userControlActivityLogging);
            this.tabPageLogging.Location = new System.Drawing.Point(4, 22);
            this.tabPageLogging.Name = "tabPageLogging";
            this.tabPageLogging.Size = new System.Drawing.Size(669, 486);
            this.tabPageLogging.TabIndex = 2;
            this.tabPageLogging.Text = "Logging";
            // 
            // userControlActivityLogging
            // 
            this.userControlActivityLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlActivityLogging.Interval = 100;
            this.userControlActivityLogging.Location = new System.Drawing.Point(0, 0);
            this.userControlActivityLogging.Name = "userControlActivityLogging";
            this.userControlActivityLogging.Size = new System.Drawing.Size(669, 486);
            this.userControlActivityLogging.TabIndex = 0;
            // 
            // toolBarSCUEmulator
            // 
            this.toolBarSCUEmulator.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarSCUEmulator.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonStoreImages,
            this.toolBarToggleValidation,
            this.toolBarButtonStoreCommit,
            this.toolBarButtonAbort,
            this.toolBarButtonStop,
            this.toolBarButtonResult,
            this.toolBarButton1,
            this.toolBarButtonError,
            this.toolBarButtonWarning,
            this.toolBarButtonLeft,
            this.toolBarButtonRight});
            this.toolBarSCUEmulator.ButtonSize = new System.Drawing.Size(39, 24);
            this.toolBarSCUEmulator.DropDownArrows = true;
            this.toolBarSCUEmulator.ImageList = this.imageListStorageSCU;
            this.toolBarSCUEmulator.Location = new System.Drawing.Point(0, 0);
            this.toolBarSCUEmulator.Name = "toolBarSCUEmulator";
            this.toolBarSCUEmulator.ShowToolTips = true;
            this.toolBarSCUEmulator.Size = new System.Drawing.Size(812, 28);
            this.toolBarSCUEmulator.TabIndex = 1;
            this.toolBarSCUEmulator.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarSCUEmulator_ButtonClick);
            // 
            // toolBarButtonStoreImages
            // 
            this.toolBarButtonStoreImages.ImageIndex = 0;
            this.toolBarButtonStoreImages.Name = "toolBarButtonStoreImages";
            this.toolBarButtonStoreImages.ToolTipText = "Export DICOM Data";
            // 
            // toolBarToggleValidation
            // 
            this.toolBarToggleValidation.ImageIndex = 10;
            this.toolBarToggleValidation.Name = "toolBarToggleValidation";
            this.toolBarToggleValidation.Pushed = true;
            this.toolBarToggleValidation.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarToggleValidation.ToolTipText = "DICOM validation disabled.";
            // 
            // toolBarButtonStoreCommit
            // 
            this.toolBarButtonStoreCommit.ImageIndex = 1;
            this.toolBarButtonStoreCommit.Name = "toolBarButtonStoreCommit";
            this.toolBarButtonStoreCommit.ToolTipText = "Send Storage Commit";
            // 
            // toolBarButtonAbort
            // 
            this.toolBarButtonAbort.ImageIndex = 8;
            this.toolBarButtonAbort.Name = "toolBarButtonAbort";
            this.toolBarButtonAbort.ToolTipText = "Abort export";
            // 
            // toolBarButtonStop
            // 
            this.toolBarButtonStop.Enabled = false;
            this.toolBarButtonStop.ImageIndex = 3;
            this.toolBarButtonStop.Name = "toolBarButtonStop";
            this.toolBarButtonStop.ToolTipText = "Stop Commit SCP";
            // 
            // toolBarButtonResult
            // 
            this.toolBarButtonResult.Enabled = false;
            this.toolBarButtonResult.ImageIndex = 9;
            this.toolBarButtonResult.Name = "toolBarButtonResult";
            this.toolBarButtonResult.ToolTipText = "Display Validation Result";
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
            // imageListStorageSCU
            // 
            this.imageListStorageSCU.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStorageSCU.ImageStream")));
            this.imageListStorageSCU.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStorageSCU.Images.SetKeyName(0, "");
            this.imageListStorageSCU.Images.SetKeyName(1, "");
            this.imageListStorageSCU.Images.SetKeyName(2, "");
            this.imageListStorageSCU.Images.SetKeyName(3, "");
            this.imageListStorageSCU.Images.SetKeyName(4, "");
            this.imageListStorageSCU.Images.SetKeyName(5, "");
            this.imageListStorageSCU.Images.SetKeyName(6, "");
            this.imageListStorageSCU.Images.SetKeyName(7, "");
            this.imageListStorageSCU.Images.SetKeyName(8, "");
            this.imageListStorageSCU.Images.SetKeyName(9, "SHOWRESULT.ico");
            this.imageListStorageSCU.Images.SetKeyName(10, "validate.ico");
            // 
            // toolTipSCU
            // 
            this.toolTipSCU.AutomaticDelay = 0;
            this.toolTipSCU.AutoPopDelay = 5000;
            this.toolTipSCU.InitialDelay = 1;
            this.toolTipSCU.ReshowDelay = 100;
            this.toolTipSCU.ShowAlways = true;
            // 
            // backgroundWorkerSCU
            // 
            this.backgroundWorkerSCU.WorkerSupportsCancellation = true;
            // 
            // StorageSCUEmulator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(812, 619);
            this.Controls.Add(this.tabControlStorageSCU);
            this.Controls.Add(this.toolBarSCUEmulator);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenuStorageSCU;
            this.MinimumSize = new System.Drawing.Size(690, 570);
            this.Name = "StorageSCUEmulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Storage SCU Emulator";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.StorageSCUEmulator_Closing);
            this.Load += new System.EventHandler(this.StorageSCUEmulator_Load);
            this.tabControlStorageSCU.ResumeLayout(false);
            this.tabPageStorageConfig.ResumeLayout(false);
            this.panelSCPSettings.ResumeLayout(false);
            this.panelSCPSettings.PerformLayout();
            this.groupBoxTS.ResumeLayout(false);
            this.groupBoxTS.PerformLayout();
            this.GroupBoxNrAssociations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumericStorageMaxPDU)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageCommitConfig.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericCommitMaxPDU)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.tabPageResults.ResumeLayout(false);
            this.tabPageLogging.ResumeLayout(false);
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
            DvtkApplicationLayer.VersionChecker.CheckVersion();
			// Initialize the Dvtk library
			Dvtk.Setup.Initialize();

			//Application.EnableVisualStyles();
			Application.Run(new StorageSCUEmulator());

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

        public UpdateUIControlsDelegate UpdateUIControlsHandler
        {
            get
            {
                return (this.updateUIControlsHandler);
            }
        }

        public UpdateUIControlsFromBGThreadDelegate UpdateUIControlsFromBGThreadHandler
        {
            get
            {
                return (this.updateUIControlsFromBGThreadHandler);
            }
        }
        

        public void UpdateUIControls()
        {
            toolBarButtonStop.Enabled = true;            
        }

        public void UpdateUIControlsFromBGThread()
        {
            toolBarButtonResult.Enabled = true;

            if (checkBoxStoreCommit.Checked)
            {
                toolBarButtonStoreCommit.Enabled = true;
                menuItemStorageCommit.Enabled = true;
            }
        }

        private void InitializeBackgoundWorker()
        {
            backgroundWorkerSCU.DoWork +=
                new DoWorkEventHandler(backgroundWorker_DoWork);            
        }

        private void StorageSCUEmulator_Load(object sender, System.EventArgs e)
		{
			toolBarButtonError.Visible = false;
			toolBarButtonWarning.Visible = false;
			toolBarButtonLeft.Visible = false;
			toolBarButtonRight.Visible = false;
			toolBarButtonAbort.Enabled = false;

			toolBarButtonStoreCommit.Enabled = false;
			menuItemStorageCommit.Enabled = false;

			//Hide the tab pages
			//tabControlStorageSCU.Controls.Remove(tabPageCommitConfig);
			tabControlStorageSCU.Controls.Remove(tabPageResults);
			tabControlStorageSCU.SelectedTab = tabPageStorageConfig;

			//Get the IP Address of the current machine
            try
            {
                string strHostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                if (addr != null)
                {
                    foreach (IPAddress address in addr)
                    {
                        textBoxCommitSCUIPAdd.Items.Add(address.ToString());
                    }
                    textBoxCommitSCUIPAdd.SelectedItem = textBoxCommitSCUIPAdd.Items[0];
                }
            }
            catch (Exception)
            {
                //Do nothing for DNS not configured
            }
			
			try
			{
                textBoxStorageSCPAETitle.Text = this.storageOptions.RemoteAeTitle;
                textBoxStorageSCPIPAdd.Text = this.storageOptions.RemoteHostName;
                textBoxStorageSCPPort.Text = this.storageOptions.RemotePort.ToString();
                textBoxSCUAETitle.Text = this.storageOptions.LocalAeTitle;
                NumericStorageMaxPDU.Value = this.storageOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived;
                NumericCommitMaxPDU.Value = this.storageCommitSCUOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived;

                textBoxCommitSCPAETitle.Text = this.storageCommitSCUOptions.RemoteAeTitle;
                textBoxCommitSCPIPAddr.Text = this.storageCommitSCUOptions.RemoteHostName;
                textBoxCommitSCPPort.Text = this.storageCommitSCUOptions.RemotePort.ToString();
                textBoxCommitSCUPort.Text = this.storageCommitSCUOptions.LocalPort.ToString();
                textBoxCommitSCUAETitle.Text = this.storageCommitSCUOptions.LocalAeTitle;
                delay = Int16.Parse(textBoxDelay.Text);		
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
                return (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"DVTk\Storage SCU Emulator\Configurations"));
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
				
				reader.ReadStartElement("SCUEmulatorConfig");
				string secureStr = reader.ReadElementString("SecureConnection");
                string assocStr = reader.ReadElementString("NrOfAssociations");
				if(secureStr == "True")
					checkBoxSecurity.Checked = true;
				else
					checkBoxSecurity.Checked = false;

                if (assocStr == "Single")
                    RadioButtonSingleAssoc.Checked = true;
                else
                    RadioButtonMultAssoc.Checked = true;
                
				reader.ReadStartElement("StorageConfiguration");
				textBoxStorageSCPAETitle.Text = reader.ReadElementString("RemoteAETitle");
				textBoxStorageSCPIPAdd.Text = reader.ReadElementString("RemoteIpAddress");
				textBoxStorageSCPPort.Text = reader.ReadElementString("RemotePort");
				textBoxSCUAETitle.Text = reader.ReadElementString("LocalAETitle");
				NumericStorageMaxPDU.Value = decimal.Parse(reader.ReadElementString("MaxPDUSize"));
				reader.ReadEndElement();

				reader.ReadStartElement("CommitConfiguration");
				textBoxCommitSCPAETitle.Text = reader.ReadElementString("RemoteAETitle");
				textBoxCommitSCPIPAddr.Text = reader.ReadElementString("RemoteIpAddress");
				textBoxCommitSCPPort.Text = reader.ReadElementString("RemotePort");
				textBoxCommitSCUAETitle.Text = reader.ReadElementString("LocalAETitle");
				textBoxCommitSCUIPAdd.Text = reader.ReadElementString("LocalIpAddress");
				textBoxCommitSCUPort.Text = reader.ReadElementString("ListenPort");
				textBoxDelay.Text = reader.ReadElementString("CommitTime");
				NumericCommitMaxPDU.Value = decimal.Parse(reader.ReadElementString("MaxPDUSize"));
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

			string secureStr = "False";
			if(checkBoxSecurity.Checked)
				secureStr = "True";

            string assocModeStr = "Single";
            if (RadioButtonMultAssoc.Checked)
                assocModeStr = "Multi";
            else
                assocModeStr = "Single";

            if (RadioButtonSingleAssoc.Checked)
                assocModeStr = "Single";
            else
                assocModeStr = "Multi";
			
			writer.WriteStartElement("SCUEmulatorConfig");
			writer.WriteElementString("SecureConnection", secureStr);
            writer.WriteElementString("NrOfAssociations", assocModeStr);

			writer.WriteStartElement("StorageConfiguration");
			writer.WriteElementString("RemoteAETitle", textBoxStorageSCPAETitle.Text);
			writer.WriteElementString("RemoteIpAddress", textBoxStorageSCPIPAdd.Text);
			writer.WriteElementString("RemotePort", textBoxStorageSCPPort.Text);
			writer.WriteElementString("LocalAETitle", textBoxSCUAETitle.Text);
			writer.WriteElementString("MaxPDUSize", NumericStorageMaxPDU.Value.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("CommitConfiguration");
			writer.WriteElementString("RemoteAETitle", textBoxCommitSCPAETitle.Text);
			writer.WriteElementString("RemoteIpAddress", textBoxCommitSCPIPAddr.Text);
			writer.WriteElementString("RemotePort", textBoxCommitSCPPort.Text);
			writer.WriteElementString("LocalAETitle", textBoxCommitSCUAETitle.Text);
			writer.WriteElementString("LocalIpAddress", textBoxCommitSCUIPAdd.Text);
			writer.WriteElementString("ListenPort", textBoxCommitSCUPort.Text);
			writer.WriteElementString("CommitTime", textBoxDelay.Text);
			writer.WriteElementString("MaxPDUSize", NumericCommitMaxPDU.Value.ToString());
			writer.WriteEndElement();

			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Flush();
			writer.Close();
		}

		private void buttonPing_Click(object sender, System.EventArgs e)
		{
            string msg = "";
            labelStoragePing.Text = "";
            PingReply reply = null;
            bool ok = false;
            if ((textBoxStorageSCPIPAdd.Text != null) && (textBoxStorageSCPIPAdd.Text.Length != 0))
            {
                string ipAddr = "";
                try
                {
                    ipAddr = textBoxStorageSCPIPAdd.Text.Trim();

                    Ping pingSender = new Ping();
                    reply = pingSender.Send(ipAddr, 4);
                }
                catch (PingException exp)
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
                labelStoragePing.Text = msg;
            else
            {
                labelStoragePing.Text = "Ping failed.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);        
            }
		}

		private void buttonCommitPing_Click(object sender, System.EventArgs e)
		{
            string msg = "";
            labelCommitPing.Text = "";
            PingReply reply = null;
            bool ok = false;
            if ((textBoxCommitSCPIPAddr.Text != null) && (textBoxCommitSCPIPAddr.Text.Length != 0))
            {
                string ipAddr = "";
                try
                {
                    ipAddr = textBoxCommitSCPIPAddr.Text.Trim();

                    Ping pingSender = new Ping();
                    reply = pingSender.Send(ipAddr, 4);
                }
                catch (PingException exp)
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
                labelCommitPing.Text = msg;
            else
            {
                labelCommitPing.Text = "Ping failed.";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }					
		}

		private void buttonEcho_Click(object sender, System.EventArgs e)
		{
			//isStorageEcho = true;
			labelStorageEcho.Text = "";
            toolBarButtonResult.Enabled = false;

            if ((textBoxStorageSCPIPAdd.Text == null) || (textBoxStorageSCPIPAdd.Text.Length == 0))
            {
                MessageBox.Show("Pl Specify SCP IP Address.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

			//Update DVT & SUT settings
            this.storageOptions.RemoteAeTitle = textBoxStorageSCPAETitle.Text;
            this.storageOptions.RemoteHostName = textBoxStorageSCPIPAdd.Text;
            this.storageOptions.RemotePort = UInt16.Parse(textBoxStorageSCPPort.Text);
            this.storageOptions.LocalAeTitle = textBoxSCUAETitle.Text;
            this.storageOptions.ResultsDirectory = validationResultsFileGroup.Directory;

			SCU echoScu = new SCU();

            if (isStopped)
            {
                this.overviewThread = new OverviewThread();
                this.overviewThread.Initialize(threadManager);
                this.overviewThread.Options.ResultsDirectory = validationResultsFileGroup.Directory;
                this.overviewThread.Options.Identifier = "Storage_SCU_Emulator";
                this.overviewThread.Options.AttachChildsToUserInterfaces = true;
                this.overviewThread.Options.LogThreadStartingAndStoppingInParent = false;
                this.overviewThread.Options.LogWaitingForCompletionChildThreads = false;

                this.userControlActivityLogging.Attach(overviewThread);

                String resultsFileName = "StoreSCUEmulator_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                this.overviewThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileName;
                
                //
                // Start the Dicom Overview Thread
                //
                this.overviewThread.Start();
                isStopped = false;
            }

            echoScu.Initialize(this.overviewThread);
            echoScu.Options.DeepCopyFrom(this.storageOptions);

            String resultsFileBaseName = "StoreSCUEcho_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            echoScu.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
            echoScu.Options.Identifier = resultsFileBaseName;

            echoScu.Options.LogThreadStartingAndStoppingInParent = false;
            echoScu.Options.LogWaitingForCompletionChildThreads = false;
            echoScu.Options.AutoValidate = autoValidate;

            echoScu.Options.ResultsDirectory = this.storageOptions.ResultsDirectory;

            echoScu.Options.LocalAeTitle = textBoxSCUAETitle.Text;

            echoScu.Options.RemoteAeTitle = textBoxStorageSCPAETitle.Text;
            echoScu.Options.RemotePort = UInt16.Parse(textBoxStorageSCPPort.Text);
            echoScu.Options.RemoteHostName = textBoxStorageSCPIPAdd.Text;

            //this.userControlActivityLogging.Attach(echoScu);

            PresentationContext presentationContext = new PresentationContext("1.2.840.10008.1.1", // Abstract Syntax Name
                                                                            "1.2.840.10008.1.2"); // Transfer Syntax Name(s)
            PresentationContext[] presentationContexts = new PresentationContext[1];
            presentationContexts[0] = presentationContext;

            DicomMessage echoMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORQ);

            echoScu.Start();

            bool sendResult = echoScu.TriggerSendAssociationAndWait(echoMessage, presentationContexts);

            if (!sendResult)
            {
                labelStorageEcho.Text = "DICOM Echo failed (See logging for details)";
            }
            else
                labelStorageEcho.Text = "DICOM Echo successful";

            echoScu.Stop();

            toolBarButtonResult.Enabled = true;
		}

		private void buttonCommitEcho_Click(object sender, System.EventArgs e)
		{
			//isCommitEcho = true;
			labelCommitEcho.Text = "";
            toolBarButtonResult.Enabled = false;

            if ((textBoxCommitSCPIPAddr.Text == null) || (textBoxCommitSCPIPAddr.Text.Length == 0))
            {
                MessageBox.Show("Pl Specify SCP IP Address.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

			//Update DVT & SUT settings
            this.storageOptions.RemoteAeTitle = textBoxCommitSCPAETitle.Text;
            this.storageOptions.RemoteHostName = textBoxCommitSCPIPAddr.Text;
            this.storageOptions.RemotePort = UInt16.Parse(textBoxCommitSCPPort.Text);
            this.storageOptions.LocalAeTitle = textBoxCommitSCUAETitle.Text;
            this.storageOptions.ResultsDirectory = validationResultsFileGroup.Directory;

			//Update DVT & SUT settings
            this.storageCommitSCUOptions.RemoteHostName = textBoxCommitSCPIPAddr.Text;
            this.storageCommitSCUOptions.RemotePort = UInt16.Parse(textBoxCommitSCPPort.Text);
            this.storageCommitSCUOptions.RemoteAeTitle = textBoxCommitSCPAETitle.Text;
            this.storageCommitSCUOptions.LocalAeTitle = textBoxCommitSCUAETitle.Text;

            SCU echoScu = new SCU();

            if (isStopped)
            {
                this.overviewThread = new OverviewThread();
                this.overviewThread.Initialize(threadManager);
                this.overviewThread.Options.ResultsDirectory = validationResultsFileGroup.Directory;
                this.overviewThread.Options.Identifier = "Storage_SCU_Emulator";
                this.overviewThread.Options.AttachChildsToUserInterfaces = true;
                this.overviewThread.Options.LogThreadStartingAndStoppingInParent = false;
                this.overviewThread.Options.LogWaitingForCompletionChildThreads = false;

                this.userControlActivityLogging.Attach(overviewThread);

                String resultsFileName = "StoreSCUEmulator_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                this.overviewThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileName;
                
                //
                // Start the Dicom Overview Thread
                //
                this.overviewThread.Start();
                isStopped = false;
            }

            echoScu.Initialize(this.overviewThread);
            echoScu.Options.DeepCopyFrom(this.storageCommitSCUOptions);

            String resultsFileBaseName = "CommitSCUCEcho_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            echoScu.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
            echoScu.Options.Identifier = resultsFileBaseName;

            echoScu.Options.ResultsDirectory = this.storageOptions.ResultsDirectory;

            echoScu.Options.LogThreadStartingAndStoppingInParent = false;
            echoScu.Options.LogWaitingForCompletionChildThreads = false;
            echoScu.Options.AutoValidate = autoValidate;

            echoScu.Options.LocalAeTitle = textBoxSCUAETitle.Text;

            echoScu.Options.RemoteAeTitle = textBoxStorageSCPAETitle.Text;
            echoScu.Options.RemotePort = UInt16.Parse(textBoxStorageSCPPort.Text);
            echoScu.Options.RemoteHostName = textBoxStorageSCPIPAdd.Text;

            //this.userControlActivityLogging.Attach(echoScu);

            PresentationContext presentationContext = new PresentationContext("1.2.840.10008.1.1", // Abstract Syntax Name
                                                                            "1.2.840.10008.1.2"); // Transfer Syntax Name(s)
            PresentationContext[] presentationContexts = new PresentationContext[1];
            presentationContexts[0] = presentationContext;

            DicomMessage echoMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CECHORQ);

            echoScu.Start();

            bool sendResult = echoScu.TriggerSendAssociationAndWait(echoMessage, presentationContexts);

            if (!sendResult)
            {
                labelCommitEcho.Text = "DICOM Echo failed (See logging for details)";
            }
            else
                labelCommitEcho.Text = "DICOM Echo successful";

            echoScu.Stop();

            toolBarButtonResult.Enabled = true;
		}

        private void menuItemExportDir_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog theFolderBrowserDialog = new FolderBrowserDialog();
            theFolderBrowserDialog.ShowNewFolderButton = false;
            theFolderBrowserDialog.SelectedPath = lastSelectedDir;
			theFolderBrowserDialog.Description = "Select the source directory";
			
			//Get all files from source directory
			try
			{
				if (theFolderBrowserDialog.ShowDialog (this) == DialogResult.OK) 
				{
					DirectoryInfo theDirectoryInfo = new DirectoryInfo(theFolderBrowserDialog.SelectedPath);
                    lastSelectedDir = theFolderBrowserDialog.SelectedPath;
					ArrayList dcmFiles = GetFilesRecursively(theDirectoryInfo);
					string[] mediaFiles = (System.String[])dcmFiles.ToArray(typeof(System.String));

					//Send the DICOM files
					ExportDICOMData(mediaFiles);
				}
			}
			catch (Exception theException) 
			{
				MessageBox.Show(theException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void menuItemExportFiles_Click(object sender, System.EventArgs e)
		{
			try
			{
				OpenFileDialog openMediaFileDialog = new OpenFileDialog();
				openMediaFileDialog.Filter = "DICOM media files (*.dcm)|*.dcm|All files (*.*)|*.*";
				openMediaFileDialog.Multiselect = true;
				openMediaFileDialog.Title = "Select DICOM files to export";
				
				if (openMediaFileDialog.ShowDialog (this) == DialogResult.OK) 
				{
                    openMediaFileDialog.RestoreDirectory = true;

					//Sort the files i.e. PR 1067
                    ArrayList list = new ArrayList(openMediaFileDialog.FileNames.Length);
                    foreach (string filename in openMediaFileDialog.FileNames)
                    {
                        list.Add(filename);
                    }
                    list.Sort();

                    //Send the DICOM files
                    string[] mediaFiles = (System.String[])list.ToArray(typeof(System.String));
                    ExportDICOMData(mediaFiles);
				}
			}		
			catch (InvalidOperationException ioe) 
			{
				//
				// Catch InvalidOperationException thrown by OpenFileDialog.ShowDialog method in
				// case too many files (more than approx. 800) are selected in the OpenFileDialog.
				//
				MessageBox.Show(ioe.Message ,"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void menuItemExportDICOMDIR_Click(object sender, System.EventArgs e)
		{
			try
			{
				OpenFileDialog openMediaFileDialog = new OpenFileDialog();
				openMediaFileDialog.Filter = "DICOMDIR files (DICOMDIR)|*DICOMDIR*|All files (*.*)|*.*";
				openMediaFileDialog.Multiselect = false;
				openMediaFileDialog.Title = "Select DICOMDIR to export";
				
				if (openMediaFileDialog.ShowDialog (this) == DialogResult.OK) 
				{
                    openMediaFileDialog.RestoreDirectory = true;
                    //FileInfo f = new FileInfo(openMediaFileDialog.FileName);
                    //if (f.Exists)
                    //{
                    //    FileStream dicomDIR = new FileStream(openMediaFileDialog.FileName, FileMode.Open);
                    //}
                   
					//Send the DICOM files
					ExportDICOMData(openMediaFileDialog.FileNames);
				}
			}		
			catch (Exception exp) 
			{
				MessageBox.Show(exp.Message ,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}		
		}

        private void UpdateConfig()
        {
            //Update DVT & SUT settings
            this.storageOptions.RemoteAeTitle = textBoxStorageSCPAETitle.Text;
            this.storageOptions.RemoteHostName = textBoxStorageSCPIPAdd.Text;
            this.storageOptions.RemotePort = UInt16.Parse(textBoxStorageSCPPort.Text);
            this.storageOptions.LocalAeTitle = textBoxSCUAETitle.Text;
            this.storageOptions.ResultsDirectory = validationResultsFileGroup.Directory;
            toolBarButtonAbort.Enabled = true;

            //Also update commit session
            this.storageCommitSCUOptions.RemoteHostName = textBoxCommitSCPIPAddr.Text;
            this.storageCommitSCUOptions.RemotePort = UInt16.Parse(textBoxCommitSCPPort.Text);
            this.storageCommitSCUOptions.RemoteAeTitle = textBoxCommitSCPAETitle.Text;
            this.storageCommitSCUOptions.LocalAeTitle = textBoxCommitSCUAETitle.Text;
            this.storageCommitSCUOptions.LocalPort = UInt16.Parse(textBoxCommitSCUPort.Text);
            delay = Int16.Parse(textBoxDelay.Text);

            this.storageOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (uint)NumericStorageMaxPDU.Value;
            this.storageCommitSCUOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (uint)NumericCommitMaxPDU.Value;

            if (checkBoxSecurity.Checked)
            {
                this.storageOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = true;
            }
            else
            {
                this.storageOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = false;
            }
        }

        private void textBoxStorageSCPPort_Validating(object sender, CancelEventArgs e)
        {
            UInt16 newPortNumber = 0;

            try
            {
                newPortNumber = Convert.ToUInt16(this.textBoxStorageSCPPort.Text);
            }
            catch
            {
                MessageBox.Show("Invalid port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }            
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

        private void textBoxCommitSCUPort_Validating(object sender, CancelEventArgs e)
        {
            UInt16 newPortNumber = 0;

            try
            {
                newPortNumber = Convert.ToUInt16(this.textBoxCommitSCUPort.Text);
            }
            catch
            {
                MessageBox.Show("Invalid port number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

		private void ExportDICOMData(string[] mediaFiles)
		{
            String[] convertedDicomFiles= new string[mediaFiles.Length];
            if(mediaFiles.Length == 0)
			{
				MessageBox.Show("There is no DICOM Data to export." ,"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

            //Storage commit should be available only after successful storage
			toolBarButtonStoreCommit.Enabled = false;
			menuItemStorageCommit.Enabled = false;
            toolBarButtonResult.Enabled = false;
            isSingleAssoc = RadioButtonSingleAssoc.Checked;
            isImageExported = false;

            //
            // Make the Activity Logging Tab the only Tab visible and clean it.
            //
            //this.userControlActivityLogging.Clear();

            if(backgroundWorkerSCU.IsBusy)
                backgroundWorkerSCU.CancelAsync();

			try 
			{
                UpdateConfig();

                // Clear the commit items
                storageCommitItems.Clear();

                this.tabControlStorageSCU.Controls.Clear();
                this.tabControlStorageSCU.Controls.Add(this.tabPageStorageConfig);
                this.tabControlStorageSCU.Controls.Add(this.tabPageCommitConfig);
                this.tabControlStorageSCU.Controls.Add(this.tabPageLogging);
                tabControlStorageSCU.SelectedTab = tabPageLogging;

                if (isStopped)
                {
                    this.overviewThread = new OverviewThread();
                    this.overviewThread.Initialize(threadManager);
                    this.overviewThread.Options.ResultsDirectory = validationResultsFileGroup.Directory;
                    //this.overviewThread.Options.DataDirectory = validationResultsFileGroup.Directory;
                    this.overviewThread.Options.Identifier = "Storage_SCU_Emulator";
                    this.overviewThread.Options.AttachChildsToUserInterfaces = true;
                    this.overviewThread.Options.LogThreadStartingAndStoppingInParent = false;
                    this.overviewThread.Options.LogWaitingForCompletionChildThreads = false;

                    this.userControlActivityLogging.Attach(overviewThread);

                    String resultsFileBaseName = "StoreSCUEmulator_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    this.overviewThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
                    
                    //
                    // Start the Dicom Overview Thread
                    //
                    this.overviewThread.Start();
                    isStopped = false;
                }
                if (checkBoxTS.Checked)
                {
                    //Convert the DICOM files to the preferred TS
                    convertedDicomFiles = convertDicomFiles(mediaFiles);
                    backgroundWorkerSCU.RunWorkerAsync(convertedDicomFiles);

                }
                else
                {
                    backgroundWorkerSCU.RunWorkerAsync(mediaFiles);
                }

                //isStopped = false;
                isImageExported = true;
			}			
			catch (Exception theException) 
			{
				MessageBox.Show(theException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void backgroundWorker_DoWork(object sender,
            DoWorkEventArgs e)
        {
            string[] mediaFiles = e.Argument as string[];

            if (isSingleAssoc)
                SendDICOMDataInSingleAssociation(mediaFiles);
            else
                SendDICOMDataInMultipleAssociation(mediaFiles);

            //Enable Stop button in UI
            this.Invoke(this.UpdateUIControlsFromBGThreadHandler);            
        }        

        private void SendDICOMDataInMultipleAssociation(string[] mediaFiles)
        {
            int index = 0;
            // Set the dicom messages to send
            DicomMessageCollection dicomMessageCollection = new DicomMessageCollection();
            PresentationContextCollection pcCollection = new PresentationContextCollection();
            
            //The Current Directory is being set to the Results Directory because
            // when DICOMDIRs(or DICOM Files)Media are exported, the Environment.CurrentDirectory
            //is set to the Directory in which the DCM objects are present and the export will fail
            // if the DICOMDIR is present on a Physical Media.
            Environment.CurrentDirectory = this.storageOptions.ResultsDirectory;

            foreach (string dcmFilename in mediaFiles)
			{
                HliScu storageScuSubThread = new HliScu();

                storageScuSubThread.Initialize(this.overviewThread);
                storageScuSubThread.Options.CopyFrom(this.storageOptions);

                storageScuSubThread.Options.Identifier = string.Format("StorageOperation_{0}",index);

                String resultsFileBaseName = string.Format("StorageOperation_{0}_", index) + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                storageScuSubThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;

                storageScuSubThread.Options.LogThreadStartingAndStoppingInParent = false;
                storageScuSubThread.Options.LogWaitingForCompletionChildThreads = false;
                storageScuSubThread.Options.AutoValidate = autoValidate;

                string msg = string.Format("Reading and exporting the DICOM object - {0}", dcmFilename);
                storageScuSubThread.WriteInformation(msg);
                

                try
                {
                    // Read the DCM File
                    DicomFile dcmFile = new DicomFile();
                    dcmFile.Read(dcmFilename, storageScuSubThread);

                    FileMetaInformation fMI = dcmFile.FileMetaInformation;

                    // Get the transfer syntax and SOP class UID
                    System.String transferSyntax = "1.2.840.10008.1.2.1";
                    if ((fMI != null) && fMI.Exists("0x00020010"))
                    {
                        // Get the Transfer syntax
                        HLI.Attribute tranferSyntaxAttr = fMI["0x00020010"];
                        transferSyntax = tranferSyntaxAttr.Values[0];
                    }

                    Values values = fMI["0x00020002"].Values;
                    System.String sopClassUid = values[0];

                    //Check for DICOMDIR
                    if (sopClassUid == "1.2.840.10008.1.3.10")
                    {
                        // Read the DICOMDIR dataset
                        dcmFile.DataSet.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(dcmFilename);
                        ArrayList refFiles = ImportRefFilesFromDicomdir(dcmFilename, dcmFile.DataSet);

                        foreach (string refFilename in refFiles)
                        {
                            // Read the Reference File
                            DicomFile refFile = new DicomFile();
                            refFile.Read(refFilename, storageScuSubThread);

                            FileMetaInformation refFMI = refFile.FileMetaInformation;

                            // Get the transfer syntax and SOP class UID
                            System.String refTransferSyntax = "1.2.840.10008.1.2.1";
                            if ((refFMI != null) && refFMI.Exists("0x00020010"))
                            {
                                // Get the Transfer syntax
                                HLI.Attribute refTranferSyntaxAttr = refFMI["0x00020010"];
                                refTransferSyntax = refTranferSyntaxAttr.Values[0];
                            }

                            Values sopValues = refFile.DataSet["0x00080016"].Values;
                            System.String refSopClassUid = sopValues[0];

                            PresentationContext refPresentationContext = new PresentationContext(refSopClassUid, // Abstract Syntax Name
                                                                                        refTransferSyntax); // Transfer Syntax Name(s)
                            pcCollection.Add(refPresentationContext);

                            DicomMessage refStorageMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ);
                            refStorageMessage.DataSet.CloneFrom(refFile.DataSet);
                            dicomMessageCollection.Add(refStorageMessage);

                            // set the presentation contexts for the association
                            PresentationContext[] presentationContexts = SetPresentationContexts(pcCollection);

                            storageScuSubThread.Start();

                            storageScuSubThread.TriggerSendAssociationAndWait(dicomMessageCollection, presentationContexts);
                        }
                    }
                    else
                    {
                        PresentationContext presentationContext = new PresentationContext(sopClassUid, // Abstract Syntax Name
                                                                                        transferSyntax); // Transfer Syntax Name(s)
                        PresentationContext[] presentationContexts = new PresentationContext[1];
                        presentationContexts[0] = presentationContext;

                        DicomMessage storageMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ);
                        storageMessage.DataSet.CloneFrom(dcmFile.DataSet);

                        storageScuSubThread.Start();

                        storageScuSubThread.TriggerSendAssociationAndWait(storageMessage, presentationContexts);
                    }

                    if (storageScuSubThread.HasExceptionOccured)
                    {
                        storageScuSubThread.WriteError(string.Format("Store operation of {0} failed", dcmFilename));
                    }
                }
                catch (Exception e)
                {
                    storageScuSubThread.WriteError(e.Message + "\nSkipping the DCM File.");
                }

                index++;

                storageScuSubThread.Stop();
			}

            DicomMessageCollection cStoreResponses = threadManager.Messages.DicomProtocolMessages.DicomMessages.CStoreResponses;

            foreach (DicomMessage cStoreRsp in cStoreResponses)
            {
                Int32 statusVal = Int32.Parse(cStoreRsp.CommandSet.GetValues("0x00000900")[0]);
                String sopInstUid = cStoreRsp.CommandSet.GetValues("0x00001000")[0];
                if (statusVal == 0)
                {
                    string infoMsg = string.Format("Image with SOP Instance UID{0} stored successfully.", sopInstUid);
                    overviewThread.WriteInformation(infoMsg);
                }
                else
                {
                    string warnMsg = string.Format("Non-zero status returned. Image with SOP Instance UID{0} storage failed.", sopInstUid);
                    overviewThread.WriteWarning(warnMsg);
                }
            }

            HandleCStoreResponses(storageCommitItems, cStoreResponses);

            threadManager.Messages.DicomProtocolMessages.DicomMessages.CStoreResponses.Clear();
        }

        private void SendDICOMDataInSingleAssociation(string[] mediaFiles)
        {
            // Set the dicom messages to send
            List<DicomFile> dicomMessageCollection = new List<DicomFile>();
            PresentationContextCollection pcCollection = new PresentationContextCollection();

            StoreScu storageScuThread = new StoreScu();

            storageScuThread.Initialize(this.overviewThread);
            storageScuThread.Options.CopyFrom(this.storageOptions);

            String resultsFileBaseName = "StorageOperation_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            storageScuThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
            storageScuThread.Options.Identifier = resultsFileBaseName;

            storageScuThread.Options.LogThreadStartingAndStoppingInParent = false;
            storageScuThread.Options.LogWaitingForCompletionChildThreads = false;
            storageScuThread.Options.AutoValidate = autoValidate;

            //The Current Directory is being set to the Results Directory because
            // when DICOMDIRs(or DICOM Files)Media are exported, the Environment.CurrentDirectory
            //is set to the Directory in which the DCM objects are present and the export will fail
            // if the DICOMDIR is present on a Physical Media.
            Environment.CurrentDirectory = this.storageOptions.ResultsDirectory;

            foreach (string dcmFilename in mediaFiles)
            {
                string msg = string.Format("Reading the DICOM object - {0}", dcmFilename);
                storageScuThread.WriteInformation(msg);

                try
                {
                    // Read the DCM File
                    DicomFile dcmFile = new DicomFile();
                    
                    dcmFile.Read(dcmFilename, storageScuThread);
                    
                    FileMetaInformation fMI =  dcmFile.FileMetaInformation;

                    // Get the transfer syntax and SOP class UID
                    System.String transferSyntax = "1.2.840.10008.1.2.1";
                    System.String sopClassUid = "";
                    if ((fMI != null))
                    {
                        // Get the Transfer syntax
                        HLI.Attribute tranferSyntaxAttr = fMI["0x00020010"];
                        transferSyntax = tranferSyntaxAttr.Values[0];

                        // Get the SOP Class UID
                        Values values = fMI["0x00020002"].Values;
                        sopClassUid = values[0];
                    }
                    else
                    {
                        // Get the SOP Class UID
                        Values values =  dcmFile.DataSet["0x00080016"].Values;
                        sopClassUid = values[0];
                    }

                    //Check for DICOMDIR
                    if(sopClassUid == "")
                    {
                        storageScuThread.WriteError("Can't determine SOP Class UID for DCM file. \nSkipping the DCM File.");
                        continue;
                    }
                    else if (sopClassUid == "1.2.840.10008.1.3.10")
                    {                                               
                        
                            HLI.DataSet tempDataSet = new HLI.DataSet();

                            // Read the DICOMDIR dataset
                            dcmFile.DataSet.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(dcmFilename);
                            Dvtk.Sessions.ScriptSession session = this.storageOptions.DvtkScriptSession;
                            ArrayList refFiles = ImportRefFilesFromDicomdir(dcmFilename, dcmFile.DataSet);

                            foreach (string refFilename in refFiles)
                            {
                                // Read the Reference File
                                DicomFile refFile = new DicomFile();
                                refFile.Read(refFilename, storageScuThread);

                                FileMetaInformation refFMI = refFile.FileMetaInformation;

                                // Get the transfer syntax and SOP class UID
                                System.String refTransferSyntax = "1.2.840.10008.1.2.1";
                                if ((refFMI != null) && refFMI.Exists("0x00020010"))
                                {
                                    // Get the Transfer syntax
                                    HLI.Attribute refTranferSyntaxAttr = refFMI["0x00020010"];
                                    refTransferSyntax = refTranferSyntaxAttr.Values[0];
                                }

                                Values sopValues = refFile.DataSet["0x00080016"].Values;
                                System.String refSopClassUid = sopValues[0];

                                PresentationContext refPresentationContext = new PresentationContext(refSopClassUid, // Abstract Syntax Name
                                                                                                     refTransferSyntax); // Transfer Syntax Name(s)
                                pcCollection.Add(refPresentationContext);

                                //DicomMessage refStorageMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ);
                                //refStorageMessage.DataSet.CloneFrom(refFile.DataSet);
                                dicomMessageCollection.Add(refFile);
                            }
                    }
                    else
                    {
                        PresentationContext presentationContext = new PresentationContext(sopClassUid, // Abstract Syntax Name
                                                                                          transferSyntax); // Transfer Syntax Name(s)
                        pcCollection.Add(presentationContext);

                        //DicomMessage storageMessage = new DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ);
                        //storageMessage.DataSet.CloneFrom(dcmFile.DataSet);
                        dicomMessageCollection.Add(dcmFile);
                    }
                }
                catch (Exception e)
                {
                    storageScuThread.WriteError(e.Message + "\nSkipping the DCM File.");
                }
            }

            // set the presentation contexts for the association
            PresentationContext[] presentationContexts = SetPresentationContexts(pcCollection);

            storageScuThread.DICOMFileCollection = dicomMessageCollection;
            storageScuThread.PresentationContexts = presentationContexts;

            storageScuThread.Start();            

            storageScuThread.WaitForCompletion();

            DicomMessageCollection cStoreResponses = storageScuThread.Messages.DicomMessages.CStoreResponses;

            //storageScuThread.Stop();

            HandleCStoreResponses(storageCommitItems, cStoreResponses);

            storageScuThread.Messages.DicomMessages.CStoreResponses.Clear();
        }

        private ArrayList ImportRefFilesFromDicomdir(string dicomDir, DvtkData.Dimse.DataSet dataset)
        {
            int recordsCount = 0;
            ArrayList refFileNames = new ArrayList();
            FileInfo dicomDirInfo = new FileInfo(dicomDir);
            DvtkData.Dimse.Attribute recordSeqAttr = dataset.GetAttribute(DvtkData.Dimse.Tag.DIRECTORY_RECORD_SEQUENCE);
            if (recordSeqAttr!=null)
            {
                DvtkData.Dimse.SequenceOfItems sequenceOfItems = recordSeqAttr.DicomValue as DvtkData.Dimse.SequenceOfItems;
                
                foreach(DvtkData.Dimse.SequenceItem item in sequenceOfItems.Sequence)
                {
                    DvtkData.Media.DirectoryRecord record = (DvtkData.Media.DirectoryRecord)(item as DvtkData.Dimse.AttributeSet);
                    DvtkData.Dimse.Attribute attributeRecType = item.GetAttribute(DvtkData.Dimse.Tag.DIRECTORY_RECORD_TYPE);
                    if (attributeRecType != null)
                    {
                        DvtkData.Dimse.CodeString recType = (DvtkData.Dimse.CodeString)attributeRecType.DicomValue;
                        record.DirectoryRecordType = recType.Values[0];
                    }


                    if ((record.DirectoryRecordType == "IMAGE") || (record.DirectoryRecordType == "PRESENTATION"))
                    {
                        string refFile = "";
                        DvtkData.Dimse.Attribute attributeRefFile = item.GetAttribute(DvtkData.Dimse.Tag.REFERENCED_FILE_ID);
                        if (attributeRefFile != null)
                        {
                            DvtkData.Dimse.CodeString attrValues = (DvtkData.Dimse.CodeString)attributeRefFile.DicomValue;
                            
                            foreach (DvtkData.Dimse.CodeString value in attrValues.Values)
                            {
                                refFile += value;
                                refFile += @"\";
                            }
                        }
                        

                        refFile = refFile.Remove((refFile.Length - 1), 1);
                        refFileNames.Add(Path.Combine(dicomDirInfo.DirectoryName, refFile));
                    }
                }
            }

            return refFileNames;
        }
        
        
        
        
        private ArrayList ImportRefFilesFromDicomdir(string dicomDir, HLI.DataSet dataset)
        {
            int recordsCount = 0;
            ArrayList refFileNames = new ArrayList();
            FileInfo dicomDirInfo = new FileInfo(dicomDir);
            HLI.Attribute recordSeqAttr = dataset["0x00041220"];
            if (recordSeqAttr.Exists)
            {
                recordsCount = recordSeqAttr.ItemCount;

                for (int i = 0; i < recordsCount; i++)
                {
                    HLI.SequenceItem record = recordSeqAttr.GetItem(i + 1);
                    Values values = record["0x00041430"].Values;
                    System.String recType = values[0];
                    if ((recType == "IMAGE") || (recType == "PRESENTATION"))
                    {
                        Values refFileIds = record["0x00041500"].Values;

                        string refFile = "";
                        for (int j = 0; j < refFileIds.Count; j++)
                        {
                            refFile += refFileIds[j];
                            refFile += @"\";
                        }

                        refFile = refFile.Remove((refFile.Length - 1), 1);
                        refFileNames.Add(Path.Combine(dicomDirInfo.DirectoryName, refFile));
                    }
                }
            }

            return refFileNames;
        }

        private void HandleCStoreResponses(ReferencedSopItemCollection storageCommitItems, DicomMessageCollection cStoreResponses)
        {
            foreach (DicomMessage cStoreRsp in cStoreResponses)
            {
                AddSopData(storageCommitItems, cStoreRsp);
            }
        }

        private void AddSopData(ReferencedSopItemCollection storageCommitItems, DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage)
        {
            Values values = dicomMessage.CommandSet["0x00000002"].Values;
            System.String sopClassUid = values[0];
            values = dicomMessage.CommandSet["0x00001000"].Values;
            System.String sopInstanceUid = values[0];
            ReferencedSopItem storageCommitItem = storageCommitItems.Find(sopClassUid, sopInstanceUid);
            if (storageCommitItem == null)
            {
                storageCommitItem = new ReferencedSopItem(sopClassUid, sopInstanceUid);
                storageCommitItem.InstanceState = InstanceStateEnum.InstanceStored;
                storageCommitItems.Add(storageCommitItem);
            }
        }

        private ReferencedSopItem Find(System.String sopClassUid, System.String sopInstanceUid)
        {
            ReferencedSopItem referencedSopItem = null;

            foreach (ReferencedSopItem lReferencedSopItem in storageCommitItems)
            {
                if ((lReferencedSopItem.SopClassUid == sopClassUid) &&
                    (lReferencedSopItem.SopInstanceUid == sopInstanceUid))
                {
                    referencedSopItem = lReferencedSopItem;
                    break;
                }
            }

            return referencedSopItem;
        }

        private PresentationContext[] SetPresentationContexts(PresentationContextCollection pcs)
        {
            PresentationContextCollection localPCs = new PresentationContextCollection();

            // use the local trigger items to establish a list of presentation contexts that
            // only appear once
            foreach (PresentationContext pc in pcs)
            {
                if (FindMatchingPresentationContext(localPCs,pc) == false)
                {
                    localPCs.Add(pc);
                }
            }

            // now set up the returned presentation contexts from the local trigger collection
            PresentationContext[] presentationContexts = new PresentationContext[localPCs.Count];
            int index = 0;
            foreach (PresentationContext pc in localPCs)
            {
                // save the presentation context
                string[] transferSyntaxes = new string[pc.TransferSyntaxes.Count];
                int i = 0;
                foreach(string ts in pc.TransferSyntaxes)
                {
                    transferSyntaxes.SetValue(ts,i);
                    i++;
                }
                presentationContexts[index++] = new PresentationContext(pc.AbstractSyntax, transferSyntaxes);
            }

            return presentationContexts;
        }

        private bool FindMatchingPresentationContext(PresentationContextCollection pcs,PresentationContext value)
        {
            bool found = false;
            foreach (PresentationContext pc in pcs)
            {
                if (IsPCEquals(pc,value))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        private bool IsPCEquals(PresentationContext pc1,PresentationContext pc2)
        {
            // Only interested in comparing the SOP Class UID and Transfer Syntaxes
            if (pc1.AbstractSyntax != pc2.AbstractSyntax) return false;
            if (pc1.TransferSyntaxes.Count != pc2.TransferSyntaxes.Count) return false;

            // The transfer syntaxes may be defined in a different order
            bool equal = true;
            for (int i = 0; i < pc1.TransferSyntaxes.Count; i++)
            {
                bool matchFound = false;
                for (int j = 0; j < pc2.TransferSyntaxes.Count; j++)
                {
                    if (pc1.TransferSyntaxes[i] == pc2.TransferSyntaxes[j])
                    {
                        matchFound = true;
                        break;
                    }
                }
                if (matchFound == false)
                {
                    equal = false;
                    break;
                }
            }

            return equal;
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
            if (!allThreadsFinished)
            {
                if ((numberOfRunning == 0) && (numberOfStopping == 0) && (numberOfStopped > 0))
                {
                    if ((this.InvokeRequired) && (this.IsHandleCreated))
                        Invoke(new ExecutionCompletedHandler(this.HandleExecutionCompleted));
                }
            }
        }

        /// <summary>
        /// Called when all threads have stopped.
        /// Takes care that buttons and tabs are enabled/disabled.
        /// </summary>
        private void HandleExecutionCompleted()
        {
            Cursor.Current = Cursors.Default;
                        
            if (!isStopped)
            {
                lock (lockObject)
                {
                    allThreadsFinished = true;
                }

                isStopped = true;
            }

            CleanUp();
        }

		private void menuItemStorageCommit_Click(object sender, System.EventArgs e)
		{
			toolBarButtonStoreCommit.Enabled = false;
			menuItemStorageCommit.Enabled = false;
            toolBarButtonResult.Enabled = false;
            
			//If result tab is present, remove it
			if(tabControlStorageSCU.Controls.Contains(tabPageResults))
			{
				tabControlStorageSCU.Controls.Remove(tabPageResults);
			}

            tabControlStorageSCU.SelectedTab = tabPageLogging;

            //Update DVT & SUT settings
            UpdateConfig();

			this.storageCommitSCUOptions.ResultsDirectory = validationResultsFileGroup.Directory;
            this.storageCommitSCUOptions.DataDirectory = this.storageOptions.DataDirectory;
            toolBarButtonAbort.Enabled = false;

            CommitScu commitScuThread = new CommitScu(this, autoValidate);

            if (isStopped)
            {
                this.overviewThread = new OverviewThread();
                this.overviewThread.Initialize(threadManager);
                this.overviewThread.Options.ResultsDirectory = validationResultsFileGroup.Directory;
                this.overviewThread.Options.Identifier = "Storage_SCU_Emulator";
                this.overviewThread.Options.AttachChildsToUserInterfaces = true;
                this.overviewThread.Options.LogThreadStartingAndStoppingInParent = false;
                this.overviewThread.Options.LogWaitingForCompletionChildThreads = false;

                this.userControlActivityLogging.Attach(overviewThread);

                String resultsFileName = "StoreSCUEmulator_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                this.overviewThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileName;
                
                //
                // Start the Dicom Overview Thread
                //
                this.overviewThread.Start();
                isStopped = false;
            }

            commitScuThread.Initialize(this.overviewThread);
            commitScuThread.Options.CopyFrom(this.storageCommitSCUOptions);

            String resultsFileBaseName = "StorageCommitOperationAsSCU_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            commitScuThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
            commitScuThread.Options.Identifier = resultsFileBaseName;

            commitScuThread.Options.LogThreadStartingAndStoppingInParent = false;
            commitScuThread.Options.LogWaitingForCompletionChildThreads = false;
            commitScuThread.Options.AutoValidate = autoValidate;

            DicomMessage nActionStorageCommitment = new DicomMessage(DvtkData.Dimse.DimseCommand.NACTIONRQ);

            nActionStorageCommitment.Set("0x00000003", DvtkData.Dimse.VR.UI, "1.2.840.10008.1.20.1");
            nActionStorageCommitment.Set("0x00001001", DvtkData.Dimse.VR.UI, "1.2.840.10008.1.20.1.1"); // Well known Instance UID
            nActionStorageCommitment.Set("0x00001008", DvtkData.Dimse.VR.US, 1);

            DvtkData.Dimse.SequenceItem referencedStudyComponentSequenceItem = new DvtkData.Dimse.SequenceItem();
            referencedStudyComponentSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.GroupNumber,
                DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.ElementNumber,
                DvtkData.Dimse.VR.UI, "1.2.840.10008.3.1.2.3.3");
            string refSopInstanceUID = UID.Create();
            referencedStudyComponentSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.GroupNumber,
                DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.ElementNumber,
                DvtkData.Dimse.VR.UI, refSopInstanceUID);
            nActionStorageCommitment.DataSet.DvtkDataDataSet.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_STUDY_COMPONENT_SEQUENCE.GroupNumber,
                DvtkData.Dimse.Tag.REFERENCED_STUDY_COMPONENT_SEQUENCE.ElementNumber,
                DvtkData.Dimse.VR.SQ, referencedStudyComponentSequenceItem);

            AddReferencedSopSequence(storageCommitItems, 0x00081199, nActionStorageCommitment.DataSet.DvtkDataDataSet, InstanceStateEnum.InstanceStorageCommitRequested);

            string transUID = UID.Create();
            nActionStorageCommitment.DataSet.DvtkDataDataSet.AddAttribute(DvtkData.Dimse.Tag.TRANSACTION_UID.GroupNumber,
                DvtkData.Dimse.Tag.TRANSACTION_UID.ElementNumber,
                DvtkData.Dimse.VR.UI, transUID);

            if (delay < 0)
            {
                //toolBarButtonStop.Enabled = true;
                commitScuThread.setSupportedTS(selectedTS);
            }

            commitScuThread.ThreadSettings = this.storageCommitSCUOptions;
            commitScuThread.Timeout = delay;
            commitScuThread.NActionMessage = nActionStorageCommitment;
            commitScuThread.Start();
            
		}

        private void AddReferencedSopSequence(ReferencedSopItemCollection storageCommitItems,
            uint tag,
            DvtkData.Dimse.AttributeSet attributeSet,
            InstanceStateEnum newInstanceState)
        {
            ushort group = (ushort)(tag >> 16);
            ushort element = (ushort)(tag & 0x0000FFFF);
            DvtkData.Dimse.Tag tagValue = new DvtkData.Dimse.Tag(group, element);

            DvtkData.Dimse.Attribute referencedSopSequence = attributeSet.GetAttribute(tagValue);
            if (referencedSopSequence != null)
            {
                attributeSet.Remove(referencedSopSequence);
            }

            referencedSopSequence = new DvtkData.Dimse.Attribute(tag, DvtkData.Dimse.VR.SQ);
            DvtkData.Dimse.SequenceOfItems referencedSopSequenceOfItems = new DvtkData.Dimse.SequenceOfItems();
            referencedSopSequence.DicomValue = referencedSopSequenceOfItems;

            foreach (ReferencedSopItem referencedSopItem in storageCommitItems)
            {
                if ((referencedSopItem.InstanceState == InstanceStateEnum.InstanceStored) &&                    
                    (newInstanceState == InstanceStateEnum.InstanceStorageCommitRequested))
                {
                    DvtkData.Dimse.SequenceItem referencedSopSequenceItem = new DvtkData.Dimse.SequenceItem();
                    referencedSopSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.GroupNumber,
                        DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.ElementNumber,
                        DvtkData.Dimse.VR.UI, referencedSopItem.SopClassUid);
                    referencedSopSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.GroupNumber,
                        DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.ElementNumber,
                        DvtkData.Dimse.VR.UI, referencedSopItem.SopInstanceUid);
                    referencedSopItem.InstanceState = newInstanceState;
                    referencedSopSequenceOfItems.Sequence.Add(referencedSopSequenceItem);
                }
            }
            attributeSet.Add(referencedSopSequence);
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
						if ((f.Extension.ToLower() == ".dcm")||(f.Extension == null) || (f.Extension == ""))
						{
							if ((f.Name.ToLower() == "dicomdir")&&((f.Extension == null) || (f.Extension == "")))
							{
								// do nothing.
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

		private void checkBoxSecurity_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxSecurity.Checked)
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
                this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = true;
                this.groupBoxSecurityCommit.Enabled = true;
            }
            else
            {
                this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.SecureSocketsEnabled = false;
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
            Dvtk.Sessions.ISecuritySettings theISecuritySettings = this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings;


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
            Dvtk.Sessions.ISecuritySettings theISecuritySettings = this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings;

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
                this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.CertificateFileName = theOpenFileDialog.FileName;
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
                this.storageCommitSCUOptions.DvtkScriptSession.SecuritySettings.CredentialsFileName = theOpenFileDialog.FileName;
            }
        }

        private void checkBoxStoreCommit_CheckedChanged(object sender, EventArgs e)
        {
            this.tabControlStorageSCU.Controls.Clear();
            if (checkBoxStoreCommit.Checked)
            {
                if (isImageExported)
                {
                    toolBarButtonStoreCommit.Enabled = true;
                    menuItemStorageCommit.Enabled = true;
                }
                this.tabControlStorageSCU.Controls.Add(this.tabPageStorageConfig);
                this.tabControlStorageSCU.Controls.Add(this.tabPageCommitConfig);
                this.tabControlStorageSCU.Controls.Add(this.tabPageLogging);
            }
            else
            {
                toolBarButtonStoreCommit.Enabled = false;
                menuItemStorageCommit.Enabled = false;
                this.tabControlStorageSCU.Controls.Add(this.tabPageStorageConfig);
                this.tabControlStorageSCU.Controls.Add(this.tabPageLogging);
            }
        }

		private void toolBarSCUEmulator_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if( e.Button == toolBarButtonStoreImages)
			{
				DataSource dataType = new DataSource();
				if(dataType.ShowDialog()== DialogResult.OK)
				{
					switch(dataType.GetDataSouceMode())
					{
						case 0:
							menuItemExportDir_Click( sender, null );
							break;
						case 1:
							menuItemExportFiles_Click( sender, null );
							break;
						case 2:
							menuItemExportDICOMDIR_Click( sender, null );
							break;
						default:
							menuItemExportFiles_Click( sender, null );
							break;
					}
				}
			}
			else if( e.Button == toolBarButtonStoreCommit)
			{
				menuItemStorageCommit_Click( sender, null );

                toolBarButtonResult.Enabled = true;
			}
			else if( e.Button == toolBarButtonError)
			{
				this.dvtkWebBrowserSCUEmulator.FindNextText("Error:", true, true);
			}
			else if( e.Button == toolBarButtonWarning)
			{
				this.dvtkWebBrowserSCUEmulator.FindNextText("Warning:", true, true);
			}
			else if( e.Button == toolBarButtonLeft)
			{
				this.dvtkWebBrowserSCUEmulator.Back();
			}
			else if( e.Button == toolBarButtonRight)
			{
				this.dvtkWebBrowserSCUEmulator.Forward();
			}
			else if( e.Button == toolBarButtonStop)
			{
				toolBarButtonStop.Enabled = false;
                isStopped = true;
                this.threadManager.Stop();
			}
			else if( e.Button == toolBarButtonAbort)
			{
				//toolBarButtonAbort.Enabled = false;
                toolBarButtonResult.Enabled = false;
				//Abort the export
				try
				{
                    //If Abort export button is clicked while export is in progress, we are stopping 
                    //background thread and scuDicomThread
                    if (backgroundWorkerSCU.IsBusy)
                    {
                        if (backgroundWorkerSCU.CancellationPending)
                        {
                            MessageBox.Show("Aborting Export...Please Wait...", "Busy", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                        else
                        {
                            backgroundWorkerSCU.CancelAsync();

                            this.threadManager.Stop();
                            this.Cursor = Cursors.WaitCursor;
                            this.overviewThread.WaitForCompletionChildThreads();
                            isStopped = true;
                            this.Cursor = Cursors.Default;
                            userControlActivityLogging.AddWriteActionToQueue("Stopped : ", "User aborted export", Color.Red);
                        }
                    }
				}
				catch (Exception ex)
				{
					MessageBox.Show( "Couldn't Abort the export:" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
            else if (e.Button == toolBarButtonResult)
            {
                toolBarButtonResult.Enabled = false;

                if ((!tabControlStorageSCU.Controls.Contains(tabPageResults)))
                {
                    tabControlStorageSCU.Controls.Add(tabPageResults);
                }

                if (!isStopped)
                {
                    lock (lockObject)
                    {
                        this.threadManager.Stop();
                        allThreadsFinished = true;
                        this.threadManager.WaitForCompletionThreads();  
                        //this.overviewThread.Stop();
                        //this.overviewThread.WaitForCompletionChildThreads();
                        isStopped = true;
                    }
                }

                //
                // Show the new results.
                //
                this.topXmlResults = overviewThread.Options.DetailResultsFullFileName;
                this.dvtkWebBrowserSCUEmulator.Navigate(topXmlResults);

                this.tabControlStorageSCU.SelectedTab = this.tabPageResults;
            }
            else if (e.Button == toolBarToggleValidation)
            {
                autoValidate = !toolBarToggleValidation.Pushed;
                if (autoValidate)
                {
                    toolBarToggleValidation.ToolTipText = "DICOM validation enabled.";
                }
                else
                {
                    toolBarToggleValidation.ToolTipText = "DICOM validation disabled.";
                }
            }
            else { }
		}

		private void tabControlStorageSCU_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			labelStoragePing.Text = "";
			labelCommitPing.Text = "";
			labelStorageEcho.Text = "";
			labelCommitEcho.Text = "";
			if (tabControlStorageSCU.SelectedTab == tabPageResults) 
			{	
				toolBarButtonError.Visible = true;
				toolBarButtonWarning.Visible = true;
				toolBarButtonLeft.Visible = true;
				toolBarButtonRight.Visible = true;
				
				toolBarButtonError.Enabled = this.dvtkWebBrowserSCUEmulator.ContainsErrors;
				toolBarButtonWarning.Enabled = this.dvtkWebBrowserSCUEmulator.ContainsWarnings;
			}
			else
			{
				toolBarButtonError.Visible = false;
				toolBarButtonWarning.Visible = false;
				toolBarButtonLeft.Visible = false;
				toolBarButtonRight.Visible = false;
			}
		}

        private void menuItemStoredFilesExploreValidationResults_Click(object sender, EventArgs e)
        {
            validationResultsFileGroup.Explore();
        }

        private void menuItemStoredFilesOptions_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = fileGroups.ShowDialogFileGroupsConfigurator();

            if (dialogResult == DialogResult.OK)
            {
                this.storageOptions.ResultsDirectory = validationResultsFileGroup.Directory;
                this.storageOptions.DataDirectory = validationResultsFileGroup.Directory;

                this.storageCommitSCUOptions.ResultsDirectory = validationResultsFileGroup.Directory;
                this.storageCommitSCUOptions.DataDirectory = validationResultsFileGroup.Directory;

                // Make sure the session files contain the same information as the Stored Files user settings.
                SaveToSessionFiles(this.storageOptions, this.storageCommitSCUOptions);
            }
        }

        private void SaveToSessionFiles(DicomThreadOptions storageOptions, DicomThreadOptions storageCommitOptions)
        {
            storageOptions.SaveToFile(Path.Combine(sessionFolder, "StorageSCUEmulator.ses"));
            storageCommitSCUOptions.SaveToFile(Path.Combine(sessionFolder, "CommitSCUEmulator.ses"));
        }

		private void dvtkWebBrowserSCUEmulator_BackwardFormwardEnabledStateChangeEvent()
		{
			this.toolBarButtonLeft.Enabled = this.dvtkWebBrowserSCUEmulator.IsBackwardEnabled;
			this.toolBarButtonRight.Enabled = this.dvtkWebBrowserSCUEmulator.IsForwardEnabled;
		}

        void dvtkWebBrowserSCUEmulator_ErrorWarningEnabledStateChangeEvent()
        {
            toolBarButtonError.Enabled = this.dvtkWebBrowserSCUEmulator.ContainsErrors;
            toolBarButtonWarning.Enabled = this.dvtkWebBrowserSCUEmulator.ContainsWarnings;
        }

		private void textBoxDelay_Validating(object sender, CancelEventArgs e)
        {
            bool exceptionThrown = false;

            try
            {
                delay = Int16.Parse(textBoxDelay.Text);
            }
            catch
            {
                exceptionThrown = true;
            }

            if (!exceptionThrown)
            {
                if (delay < -1)
                {
                    string msg = "Please check the help for specifying delay.";
                    MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Invalid delay.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

		private void NumericStorageMaxPDU_ValueChanged(object sender, System.EventArgs e)
		{
            this.storageOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (ushort)NumericStorageMaxPDU.Value;
		}

		private void NumericCommitMaxPDU_ValueChanged(object sender, System.EventArgs e)
		{
            this.storageOptions.DvtkScriptSession.DvtSystemSettings.MaximumLengthReceived = (ushort)NumericCommitMaxPDU.Value;
		}

		private void StorageSCUEmulator_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{			
            // Cleanup temporary files
            CleanUp();

            if (!isStopped)
            {
                lock (lockObject)
                {
                    allThreadsFinished = true;
                    this.threadManager.Stop();
                    isStopped = true;
                }
            }

            if (backgroundWorkerSCU != null)
                backgroundWorkerSCU.Dispose();

			// Update & Save final session settings
            try
            {
                UpdateConfig();
                SaveToSessionFiles(this.storageOptions, this.storageCommitSCUOptions);

                fileGroups.RemoveFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

		private void CleanUp()
		{
			//Deleting all temporary pix files
            DirectoryInfo theDataDirectoryInfo = new DirectoryInfo(this.storageOptions.ResultsDirectory);
			if(theDataDirectoryInfo.Exists)
			{
				FileInfo[] tempPixFiles = theDataDirectoryInfo.GetFiles("*.pix");
                FileInfo[] tempIdxFiles = theDataDirectoryInfo.GetFiles("*.idx");
                FileInfo[] tempDcmFiles = theDataDirectoryInfo.GetFiles("*.dcm");

                if (tempPixFiles.Length != 0)
				{
					try
					{
                        foreach (FileInfo file in tempPixFiles)
						{
							file.Delete();
						}

                        foreach (FileInfo file in tempIdxFiles)
                        {
                            file.Delete();
                        }

                        foreach (FileInfo file in tempDcmFiles)
                        {
                            file.Delete();
                        }
					}
					catch (Exception ex)
					{
						MessageBox.Show( "Couldn't remove the file :" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}			
		}

		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			this.Close();            			
		}

		private void menuItemAboutEmulator_Click(object sender, System.EventArgs e)
		{
			AboutForm about = new AboutForm("DVTk Storage SCU Emulator");
			about.ShowDialog();
		}

        private void specifyTS_Click(object sender, EventArgs e)
        {
            SelectTransferSyntaxesForm theSelectTransferSyntaxesForm = new SelectTransferSyntaxesForm();

            ArrayList tsList = new ArrayList();
            tsList.Add(DvtkData.Dul.TransferSyntax.Implicit_VR_Little_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.Explicit_VR_Big_Endian);
            tsList.Add(DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian);

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

        /// <summary>
        /// Summary description for OverviewThread.
        /// </summary>
        private class OverviewThread : DicomThread
        {
            public OverviewThread()
            {
            }

            protected override void Execute()
            {
                //
                // Keep looping until the last child thread has been stopped.
                // 
                bool endLoop = false;

                while (!endLoop)
                {
                    Sleep(500);

                    lock (lockObject)
                    {
                        endLoop = allThreadsFinished;
                    }
                }
            }
        }

        private void checkBoxTS_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTS.Checked)
            {
                groupBoxTS.Enabled = true;
            }
            else
            {
                groupBoxTS.Enabled = false;
            }
        }

        private string[] convertDicomFiles(string[] mediaFiles)
        {
            ArrayList listOfConvertedFiles = new ArrayList(mediaFiles.Length);
            string finalPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\DVTk\Storage SCU Emulator\Temp\";
            if(Directory.Exists(finalPath))
            {
               Directory.Delete(finalPath,true);
            }
            Directory.CreateDirectory(finalPath);

            DicomThread conversionThread = new ConversionThread();
            conversionThread.Initialize(this.overviewThread);
            conversionThread.Options.CopyFrom(this.storageOptions);

            String resultsFileBaseName = "StorageOperation_" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            conversionThread.Options.ResultsFileNameOnlyWithoutExtension = resultsFileBaseName;
            conversionThread.Options.Identifier = resultsFileBaseName;

            conversionThread.Options.LogThreadStartingAndStoppingInParent = false;
            conversionThread.Options.LogWaitingForCompletionChildThreads = false;
            conversionThread.Options.AutoValidate = autoValidate;
            //string[] listOfConvertedFiles = new string[mediaFiles.Length];
                       
                foreach (string dcmFileName in mediaFiles)
                {
                    string argumentString = "";
                    System.String transferSyntax = "";
                    System.String sopClassUid = "";

                    try
                    {

                        DicomFile dcmFile = new DicomFile();
                        dcmFile.Read(dcmFileName, conversionThread);
                        FileMetaInformation fMI = dcmFile.FileMetaInformation;
                        if ((fMI != null))
                        {
                            // Get the Transfer syntax
                            HLI.Attribute tranferSyntaxAttr = fMI["0x00020010"];
                            transferSyntax = tranferSyntaxAttr.Values[0];

                            // Get the SOP Class UID
                            Values values = fMI["0x00020002"].Values;
                            sopClassUid = values[0];

                        }
                        if (sopClassUid == "")
                        {
                            conversionThread.WriteError("Error while performing conversion.Can't determine SOP Class UID for DCM file. \nSkipping the DCM File.");
                            continue;
                        }
                        //Retrieve the files from the DICOMDIR
                        else if (sopClassUid == "1.2.840.10008.1.3.10")
                        {
                            System.String refTransferSyntax = "";

                            HLI.DataSet tempDataSet = new HLI.DataSet();

                            // Read the DICOMDIR dataset
                            dcmFile.DataSet.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(dcmFileName);
                            Dvtk.Sessions.ScriptSession session = this.storageOptions.DvtkScriptSession;
                            ArrayList refFiles = ImportRefFilesFromDicomdir(dcmFileName, dcmFile.DataSet);

                            foreach (string refFilename in refFiles)
                            {
                                // Read the Reference File
                                DicomFile refFile = new DicomFile();
                                refFile.Read(refFilename, conversionThread);

                                FileMetaInformation refFMI = refFile.FileMetaInformation;

                                // Get the transfer syntax and SOP class UID

                                if ((refFMI != null) && refFMI.Exists("0x00020010"))
                                {
                                    // Get the Transfer syntax
                                    HLI.Attribute refTranferSyntaxAttr = refFMI["0x00020010"];
                                    refTransferSyntax = refTranferSyntaxAttr.Values[0];
                                }
                                if (transferSyntax == "1.2.840.10008.1.2" || transferSyntax == "1.2.840.10008.1.2.1" || transferSyntax == "1.2.840.10008.1.2.2")
                                {
                                    try
                                    {
                                        if (radioButtonILE.Checked)
                                        {
                                            if (refTransferSyntax != "1.2.840.10008.1.2")
                                            {
                                                //Perform conversion
                                                string msg = string.Format("Converting the DICOM object - {0} from Transfer Syntax:{1} to Transfer Syntax:{2}", dcmFileName, transferSyntax, "1.2.840.10008.1.2");
                                                conversionThread.WriteInformation(msg);
                                                string outputFileName = finalPath + refFilename.Substring(refFilename.LastIndexOf(@"\") + 1);
                                                argumentString = "+ti" + " \"" + String.Format("{0}", refFilename) + "\" \"" + String.Format("{0}", outputFileName) + "\"";
                                                convertFile(argumentString);
                                                listOfConvertedFiles.Add(outputFileName);
                                            }
                                            else
                                            {   //If the File is encoded in the same TS as the selected TS, no need to convert.
                                                listOfConvertedFiles.Add(refFilename);
                                            }
                                        }
                                        else if (radioButtonELE.Checked)
                                        {
                                            if (refTransferSyntax != "1.2.840.10008.1.2.1")
                                            {
                                                //Perform conversion
                                                string msg = string.Format("Converting the DICOM object - {0} from Transfer Syntax:{1} to Transfer Syntax:{2}", dcmFileName, transferSyntax, "1.2.840.10008.1.2.1");
                                                conversionThread.WriteInformation(msg);
                                                string outputFileName = finalPath + refFilename.Substring(refFilename.LastIndexOf(@"\") + 1);
                                                argumentString = "+te" + " \"" + String.Format("{0}", refFilename) + "\" \"" + String.Format("{0}", outputFileName) + "\"";
                                                convertFile(argumentString);
                                                listOfConvertedFiles.Add(outputFileName);
                                            }
                                            else
                                            {   //If the File is encoded in the same TS as the selected TS, no need to convert.
                                                listOfConvertedFiles.Add(refFilename);
                                            }
                                        }
                                        else
                                        {
                                            if (refTransferSyntax != "1.2.840.10008.1.2.2")
                                            {
                                                //Perform conversion
                                                string msg = string.Format("Converting the DICOM object - {0} from Transfer Syntax:{1} to Transfer Syntax:{2}", dcmFileName, transferSyntax, "1.2.840.10008.1.2.2");
                                                conversionThread.WriteInformation(msg);
                                                string outputFileName = finalPath + refFilename.Substring(refFilename.LastIndexOf(@"\") + 1);
                                                argumentString = "+tb" + " \"" + String.Format("{0}", refFilename) + "\" \"" + String.Format("{0}", outputFileName) + "\"";
                                                convertFile(argumentString);
                                                listOfConvertedFiles.Add(outputFileName);
                                            }
                                            else
                                            {   //If the File is encoded in the same TS as the selected TS, no need to convert.
                                                listOfConvertedFiles.Add(refFilename);
                                            }
                                        }
                                    }
                                    catch(Exception e)
                                    {
                                        conversionThread.WriteWarning("Error! The file was not converted due to the following reason:" + e.Message);
                                        listOfConvertedFiles.Add(dcmFileName);
                                    }
                                }
                                else
                                {
                                    listOfConvertedFiles.Add(refFilename);
                                }
                            }
                        }
                        else
                        {
                            if (transferSyntax == "1.2.840.10008.1.2" || transferSyntax == "1.2.840.10008.1.2.1" || transferSyntax == "1.2.840.10008.1.2.2")
                            {
                                try
                                {
                                    if (radioButtonILE.Checked)
                                    {
                                        if (transferSyntax != "1.2.840.10008.1.2")
                                        {
                                            //Perform conversion
                                            string msg = string.Format("Converting the DICOM object - {0} from Transfer Syntax:{1} to Transfer Syntax:{2}", dcmFileName, transferSyntax, "1.2.840.10008.1.2");
                                            conversionThread.WriteInformation(msg);
                                            string outputFileName = finalPath + dcmFileName.Substring(dcmFileName.LastIndexOf(@"\") + 1);
                                            argumentString = "+ti" + " \"" + String.Format("{0}", dcmFileName) + "\" \"" + String.Format("{0}", outputFileName) + "\"";
                                            convertFile(argumentString);
                                            listOfConvertedFiles.Add(outputFileName);
                                        }
                                        else
                                        {   //If the File is encoded in the same TS as the selected TS, no need to convert.
                                            listOfConvertedFiles.Add(dcmFileName);
                                        }
                                    }
                                    else if (radioButtonELE.Checked)
                                    {
                                        if (transferSyntax != "1.2.840.10008.1.2.1")
                                        {
                                            //Perform conversion
                                            string msg = string.Format("Converting the DICOM object - {0} from Transfer Syntax:{1} to Transfer Syntax:{2}", dcmFileName, transferSyntax, "1.2.840.10008.1.2.1");
                                            conversionThread.WriteInformation(msg);
                                            string outputFileName = finalPath + @"\" + dcmFileName.Substring(dcmFileName.LastIndexOf(@"\") + 1);
                                            argumentString = "+te" + " \"" + String.Format("{0}", dcmFileName) + "\" \"" + String.Format("{0}", outputFileName) + "\"";
                                            convertFile(argumentString);
                                            listOfConvertedFiles.Add(outputFileName);
                                        }
                                        else
                                        {   //If the File is encoded in the same TS as the selected TS, no need to convert.
                                            listOfConvertedFiles.Add(dcmFileName);
                                        }
                                    }
                                    else if (radioButtonEBE.Checked)
                                    {
                                        if (transferSyntax != "1.2.840.10008.1.2.2")
                                        {
                                            //Perform conversion
                                            string msg = string.Format("Converting the DICOM object - {0} from Transfer Syntax:{1} to Transfer Syntax:{2}", dcmFileName, transferSyntax, "1.2.840.10008.1.2.2");
                                            conversionThread.WriteInformation(msg);
                                            string outputFileName = finalPath + dcmFileName.Substring(dcmFileName.LastIndexOf(@"\") + 1);
                                            argumentString = "+tb" + " \"" + String.Format("{0}", dcmFileName) + "\" \"" + String.Format("{0}", outputFileName) + "\"";
                                            convertFile(argumentString);
                                            listOfConvertedFiles.Add(outputFileName);
                                        }
                                        else
                                        {   //If the File is encoded in the same TS as the selected TS, no need to convert.
                                            listOfConvertedFiles.Add(dcmFileName);
                                        }
                                    }
                                }
                                catch(Exception e)
                                {
                                    conversionThread.WriteWarning("Error! The file was not converted due to the following reason:" + e.Message);
                                    listOfConvertedFiles.Add(dcmFileName);
                                }
                            }
                            else
                            {
                                listOfConvertedFiles.Add(dcmFileName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Error! The file was not converted due to the following reason:" + ex.Message);
                    }
                }

                string[] convertedMediaFiles = (System.String[])listOfConvertedFiles.ToArray(typeof(System.String));
            return (convertedMediaFiles);
        }

       


        private void convertFile(string argument)
        {
            
            Process convert = new Process();
            convert.StartInfo.FileName = Path.Combine(Application.StartupPath, "dcmconv.exe");
            convert.StartInfo.Arguments = argument;
            convert.StartInfo.CreateNoWindow = true;
            convert.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            convert.Start();
            convert.WaitForExit();
        }

	}
}
