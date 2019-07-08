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
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using DvtkApplicationLayer;
using DvtkApplicationLayer.UserInterfaces;

namespace Dvt 
{
	/// <summary>
	/// Summary description for ProjectForm2.
	/// </summary>
	public class ProjectForm2 : System.Windows.Forms.Form 
	{
		private System.Windows.Forms.TabPage TabActivityLogging;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel PanelGeneralPropertiesTitle;
		private System.Windows.Forms.PictureBox MinGSPSettings;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.PictureBox MaxGSPSettings;
		private System.Windows.Forms.Panel PanelGeneralPropertiesContent;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.NumericUpDown NumericSessonID;
		private System.Windows.Forms.Label LabelResultsDir;
		private System.Windows.Forms.Label LabelSessionType;
		private System.Windows.Forms.Label LabelDate;
		private System.Windows.Forms.Label LabelSessionTitle;
		private System.Windows.Forms.Label LabelTestedBy;
		private System.Windows.Forms.TextBox TextBoxTestedBy;
		private System.Windows.Forms.TextBox TextBoxResultsRoot;
		private System.Windows.Forms.TextBox TextBoxScriptRoot;
		private System.Windows.Forms.Label LabelScriptsDir;
		private System.Windows.Forms.TextBox TextBoxSessionTitle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label LabelSpecifyTransferSyntaxes;
		private System.Windows.Forms.Label LabelDescriptionDir;
		private System.Windows.Forms.TextBox TextBoxDescriptionRoot;
		private System.Windows.Forms.Button ButtonBrowseResultsDir;
		private System.Windows.Forms.Button ButtonBrowseScriptsDir;
		private System.Windows.Forms.Button ButtonSpecifyTransferSyntaxes;
		private System.Windows.Forms.Button ButtonBrowseDescriptionDir;
		private System.Windows.Forms.Panel PanelDVTRoleSettingsTitle;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox MinDVTRoleSettings;
		private System.Windows.Forms.PictureBox MaxDVTRoleSettings;
		private System.Windows.Forms.Panel PanelDVTRoleSettingsContent;
		private System.Windows.Forms.NumericUpDown NumericDVTListenPort;
		private System.Windows.Forms.TextBox TextBoxDVTAETitle;
		private System.Windows.Forms.Label LabelDVTAETitle;
		private System.Windows.Forms.Label LabelDVTListenPort;
		private System.Windows.Forms.Label LabelDVTSocketTimeOut;
		private System.Windows.Forms.Label LabelDVTMaxPDU;
		private System.Windows.Forms.NumericUpDown NumericDVTTimeOut;
		private System.Windows.Forms.NumericUpDown NumericDVTMaxPDU;
		private System.Windows.Forms.TextBox TextBoxDVTImplClassUID;
		private System.Windows.Forms.TextBox TextBoxDVTImplVersionName;
		private System.Windows.Forms.Label LabelDVTImplClassUID;
		private System.Windows.Forms.Label LabelDVTImplVersionName;
		private System.Windows.Forms.Panel PanelSUTSettingTitle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox MinSUTSettings;
		private System.Windows.Forms.PictureBox MaxSUTSettings;
		private System.Windows.Forms.Panel PanelSUTSettingContent;
		private System.Windows.Forms.Label LabelSUTMaxPDU;
		private System.Windows.Forms.Label LabelSUTAETitle;
		private System.Windows.Forms.Label LabelSUTTCPIPAddress;
		private System.Windows.Forms.Label LabelSUTListenPort;
		private System.Windows.Forms.TextBox TextBoxSUTAETitle;
        private System.Windows.Forms.TextBox TextBoxSUTTCPIPAddress;
		private System.Windows.Forms.NumericUpDown NumericSUTListenPort;
		private System.Windows.Forms.NumericUpDown NumericSUTMaxPDU;
		private System.Windows.Forms.Label LabelSUTImplClassUID;
		private System.Windows.Forms.Label LabelSUTImplVersionName;
		private System.Windows.Forms.TextBox TextBoxSUTImplClassUID;
		private System.Windows.Forms.TextBox TextBoxSUTImplVersionName;
		private System.Windows.Forms.Panel PanelSecuritySettingsTitle;
		private System.Windows.Forms.PictureBox MinSecuritySettings;
		private System.Windows.Forms.PictureBox MaxSecuritySettings;
		private System.Windows.Forms.CheckBox CheckBoxSecureConnection;
		private System.Windows.Forms.Panel PanelSecuritySettingsContent;
		private System.Windows.Forms.GroupBox GroupSecurityVersion;
		private System.Windows.Forms.CheckBox CheckBoxTLS;
		private System.Windows.Forms.CheckBox CheckBoxSSL;
		private System.Windows.Forms.GroupBox GroupSecurityKeyExchange;
		private System.Windows.Forms.RadioButton RadioButtonKeyExchangeRSA;
        private System.Windows.Forms.RadioButton RadioButtonKeyExchangeDH;
		private System.Windows.Forms.GroupBox GroupSecurityGeneral;
		private System.Windows.Forms.CheckBox CheckBoxCheckRemoteCertificates;
		private System.Windows.Forms.CheckBox CheckBoxCacheSecureSessions;
		private System.Windows.Forms.GroupBox GroupSecurityEncryption;
        private System.Windows.Forms.RadioButton RadioButtonEncryptionNone;
        private System.Windows.Forms.RadioButton RadioButtonEncryptionTripleDES;
        private System.Windows.Forms.RadioButton RadioButtonEncryptionAES128;
        private System.Windows.Forms.RadioButton RadioButtonEncryptionAES256;
		private System.Windows.Forms.GroupBox GroupSecurityDataIntegrity;
        private System.Windows.Forms.RadioButton RadioButtonDataIntegritySHA;
        private System.Windows.Forms.RadioButton RadioButtonDataIntegrityMD5;
		private System.Windows.Forms.GroupBox GroupSecurityAuthentication;
        private System.Windows.Forms.RadioButton RadioButtonAuthenticationRSA;
        private System.Windows.Forms.RadioButton RadioButtonAuthenticationDSA;
		private System.Windows.Forms.ListBox ListBoxSecuritySettings;
		private System.Windows.Forms.Label LabelCategories;
		private System.Windows.Forms.Label LabelSelect1ItemMsg;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Button ButtonViewCertificates;
		private System.Windows.Forms.Button ButtonViewCredentials;
		private System.Windows.Forms.Button ButtonCreateCertificate;
		public System.Windows.Forms.TabPage TabSessionInformation;
		private System.Windows.Forms.TextBox TextBoxSessionType;
		protected internal System.Windows.Forms.TabControl TabControl;
		private System.Windows.Forms.TabPage TabNoInformationAvailable;
		private System.Windows.Forms.Label LabelNoInformationAvailable;
		private System.Windows.Forms.TabPage TabScript;
        private System.Windows.Forms.RichTextBox RichTextBoxActivityLogging;
		public System.Windows.Forms.TabPage TabValidationResults;
        private System.Windows.Forms.CheckBox CheckBoxGenerateDetailedValidationResults;
		private System.Windows.Forms.RichTextBox RichTextBoxScript;
		private System.Windows.Forms.FolderBrowserDialog TheFolderBrowserDialog;
		private System.Windows.Forms.TabPage TabSpecifySopClasses;
		private System.Windows.Forms.DataGrid DataGridSpecifySopClasses;
		private System.Windows.Forms.RichTextBox RichTextBoxSpecifySopClassesInfo;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.ListBox ListBoxSpecifySopClassesDefinitionFileDirectories;
		private System.Windows.Forms.Label LabelSpecifySopClassesDefinitionFileDirectories;
		private System.Windows.Forms.Label LabelSpecifySopClassesSelectAeTitle;
		private System.Windows.Forms.ComboBox ComboBoxSpecifySopClassesAeTitle;
		private System.Windows.Forms.Button ButtonSpecifySopClassesAddDirectory;
		private System.Windows.Forms.Button ButtonSpecifySopClassesRemoveDirectory;
        private System.Windows.Forms.Panel panel3;
        private IContainer components;
		private System.Windows.Forms.MenuItem ContextMenu_Edit;
		private System.Windows.Forms.MenuItem ContextMenu_Execute;
		private System.Windows.Forms.MenuItem ContextMenu_None;
		private System.Windows.Forms.MenuItem ContextMenu_Remove;
		private System.Windows.Forms.MenuItem ContextMenu_AddNewSession;
		private System.Windows.Forms.MenuItem ContextMenu_RemoveAllResultsFiles;
		private System.Windows.Forms.MenuItem ContextMenu_RemoveSessionFromProject;
		private System.Windows.Forms.MenuItem ContextMenu_AddExistingSessions;
		private System.Windows.Forms.MenuItem ContextMenu_GenerateDICOMDIR;
		private System.Windows.Forms.MenuItem ContextMenu_GenerateDICOMDIRWithDirectory;
		private System.Windows.Forms.MenuItem ContextMenu_SaveAs;
		private System.Windows.Forms.MenuItem ContextMenu_Save;
		private System.Windows.Forms.MenuItem ContextMenu_ValidateMediaFiles;
		private System.Windows.Forms.GroupBox GroupSecurityFiles;
		private System.Windows.Forms.Label LabelTrustedCertificatesFile;
		private System.Windows.Forms.Label LabelSecurityCredentialsFile;
		private System.Windows.Forms.Button ButtonSecurityCredentialsFile;
		private System.Windows.Forms.TextBox TextBoxTrustedCertificatesFile;
		private System.Windows.Forms.TextBox TextBoxSecurityCredentialsFile;
		private System.Windows.Forms.Button ButtonTrustedCertificatesFile;
		private System.Windows.Forms.Label labelStorageMode;
		private System.Windows.Forms.CheckBox CheckBoxLogRelation;
		private System.Windows.Forms.ComboBox ComboBoxStorageMode;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.MenuItem ContextMenu_ViewExpandedScript;
		private System.Windows.Forms.ContextMenu ContextMenuRichTextBox;
		private System.Windows.Forms.MenuItem ContextMenu_Copy;
		private System.Windows.Forms.CheckBox CheckBoxDefineSQLength;
		private System.Windows.Forms.CheckBox CheckBoxAddGroupLengths;
		private Dvt.UserControlSessionTree userControlSessionTree;
		private System.Windows.Forms.ContextMenu contextMenuUserControlSessionTree;
		private System.Windows.Forms.MenuItem ContextMenu_ValidateDicomdirWithoutRefFile;
		private System.Windows.Forms.TabPage AddIssue;
		private System.Windows.Forms.TabPage RemoveIssue;
		private System.Windows.Forms.Panel WebBrowserPanel;
		public System.Windows.Forms.TabPage TabResultsManager;
        private System.Windows.Forms.Panel PanelWebBrowserResultsManager;
		private System.Windows.Forms.TabControl TabControlIssues;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TextBox TextBoxPr;
		private System.Windows.Forms.TextBox TextBoxComment;
		private System.Windows.Forms.CheckBox checkBoxIgnoreResult;
		private System.Windows.Forms.CheckBox checkBoxIgnoreResultAll;
		private System.Windows.Forms.Label LabelPr;
		private System.Windows.Forms.Label LabelComment;
		private System.Windows.Forms.Button ButtonSaveIssue;
		private System.Windows.Forms.ContextMenu ContextMenuDataGrid;
		private System.Windows.Forms.MenuItem ContextMenu_SelectAllDefinitionFiles;
		private System.Windows.Forms.MenuItem ContextMenu_UnSelectAllDefinitionFiles;
		private System.Windows.Forms.Button ButtonSelectAllDefinitionFiles;
		private System.Windows.Forms.Button ButtonUnselectAll;
		private System.Windows.Forms.MenuItem ContextMenu_OpenDefFile;
		private System.Windows.Forms.Panel PanelResultsManagerTiltle;
		private System.Windows.Forms.PictureBox PictureBoxMinResultsTab;
		private System.Windows.Forms.PictureBox PictureBoxMaximizeResultTab;
		private System.Windows.Forms.Label LabelIssueText;
		private System.Windows.Forms.Button ButtonAddIssue;
		private System.Windows.Forms.TextBox TextBoxAddPR;
		private System.Windows.Forms.TextBox TextBoxAddComent;
		private System.Windows.Forms.Label LabelAddPr;
		private System.Windows.Forms.Label LabelAddComent;
		private System.Windows.Forms.TextBox TextBoxAddMessageText;
		private System.Windows.Forms.Label LabelAddMessageText;
		private System.Windows.Forms.Label LabelErrorMessage;
		private System.Windows.Forms.MainMenu MainMenu_ProjectForm;
		private System.Windows.Forms.CheckBox CheckBoxDisplayCondition;
		public System.Windows.Forms.TabPage TabEmpty;

		public static DvtkApplicationLayer.Project projectApp = null;
		SopClassesManager _SopClassesManager;

		// Boolean indicating if the tabs are shown.
		private bool _TCM_SessionInformationTabShown = true;
		private bool _TCM_ActivityLoggingTabShown = true;
		private bool _TCM_DetailedValidationTabShown = true;
		private bool _TCM_ScriptTabShown = true;
		private bool _TCM_SpecifySopClassesTabShown = true;
		private bool _TCM_NoInformationAvailableTabShown = true;
		private bool _TCM_ResultsManagerTabShown = true;
		private bool _TCM_EmptyTabShown = true;
        
        //Boolean indicating whether the link in the Validation Page(for Emulator) was clicked.
        private bool _IsSubIndexClicked = false;

		private Rectangle _PreviousBounds = Rectangle.Empty;

		string _HtmlFullFileNameToShow;
		string messageIndex = null ;
		string messageText = null ;
		string fullResultFileName = null ;
		string name = null ;
		bool nameHasValue = false ;
		string resultProjectXml = null;
		bool folderIndicator = false ;
		string theFullFileName = null ;
		public bool filterIndicator = false;
		string resultFileNameForAddIssue = null;
        
        //These variables keep track of the sub index results page for a Emulator Session(art of the solution to Ticket 1201)
        string subIndexFileName = null;
        Uri subIndexUri = null;
		
		// Manages the validation results tab.
        private ValidationResultsManager _TCM_ValidationResultsBrowser = null;
		//private ValidationResultsManager _TCM_ValidationResultsManagerBrowser = null;

		public enum ProjectFormState {IDLE, EXECUTING_SCRIPT, EXECUTING_STORAGE_SCP, EXECUTING_PRINT_SCP, EXECUTING_STORAGE_SCU, EXECUTING_MEDIA_VALIDATION};
		private ProjectFormState _State = ProjectFormState.IDLE;

		delegate void TCM_AppendTextToActivityLogging_ThreadSafe_Delegate(string theText);
		private TCM_AppendTextToActivityLogging_ThreadSafe_Delegate _TCM_AppendTextToActivityLogging_ThreadSafe_Delegate = null;
		private System.Windows.Forms.MenuItem ContextMenu_ExploreResultsDir;
		private System.Windows.Forms.MenuItem ContextMenu_ExploreScriptsDir;

		// Needed to be able to differentiate between controls changed by the user
		// and controls changed by an update method.
		// private bool _TCM_IsUpdatingTabControl = false;
		private int _TCM_UpdateCount = 0;
		private int _TCM_CountForControlsChange = 0;

        //Boolean indicating whether the Media Session is validating a Media Directory
       // public  bool _IS_MediaDirectory_BeingValidated = false;

		// Used to be able to tell which session is used to fill the Session Information Tab.
		private DvtkApplicationLayer.Session _TCM_SessionUsedForContentsOfTabSessionInformation = null;
		private Dvtk.Events.ActivityReportEventHandler _ActivityReportEventHandler;

        bool _TCM_ShowMinSecuritySettings = true;
        private WebBrowser webBrowserValResult;
        private WebBrowser webBrowserStartScreen;
        private WebBrowser webBrowserResultMgr;
        private WebBrowser webBrowserScript;
        private MenuItem ContextMenu_ValidateMediaDirectory;
        private Button ButtonBrowseDataDirectory;
        private TextBox TextBoxDataRoot;
        private Label label4;
		public MainForm _MainForm = null;

		/// <summary>
		/// Get the state.
		/// </summary>
		/// <returns>The state.</returns>
		public ProjectFormState GetState() 
		{
			return _State;
		}

		/// <summary>
		/// Boolean FilterIndicator.
		/// </summary>
		public Boolean FilterIndicator
		{
			get
			{
				return filterIndicator;
			}
			set 
			{
				filterIndicator = value;
			}
		}
		
		/// <summary>
		/// Method to check for execution
		/// </summary>
		/// <returns></returns>
		public bool IsExecuting() 
		{
			bool isExecuting = false;
			if ( (GetState() == ProjectFormState.EXECUTING_SCRIPT) ||
				(GetState() == ProjectFormState.EXECUTING_STORAGE_SCP) ||
				(GetState() == ProjectFormState.EXECUTING_PRINT_SCP) ||
				(GetState() == ProjectFormState.EXECUTING_STORAGE_SCU)
				) 
			{
				isExecuting = true;
			}
			return(isExecuting);
		}

		public enum ProjectFormActiveTab {SESSION_INFORMATION_TAB, VALIDATION_RESULTS_TAB, SPECIFY_SOP_CLASSES_TAB, ACTIVITY_LOGGING_TAB, SCRIPT_TAB, RESULTS_MANAGER_TAB , OTHER_TAB};

		/// <summary>
		/// Get the active tab of the tab control.
		/// </summary>
		/// <returns>The active tab.</returns>
		public ProjectFormActiveTab GetActiveTab() 
		{
			ProjectFormActiveTab theActiveTab = ProjectFormActiveTab.OTHER_TAB;

			if (TabControl.SelectedTab == TabSessionInformation) 
			{
				theActiveTab = ProjectFormActiveTab.SESSION_INFORMATION_TAB;
			}

			if (TabControl.SelectedTab == TabActivityLogging) 
			{
				theActiveTab = ProjectFormActiveTab.ACTIVITY_LOGGING_TAB;
			}

			if (TabControl.SelectedTab == TabScript) 
			{
				theActiveTab = ProjectFormActiveTab.SCRIPT_TAB;
			}

			if (TabControl.SelectedTab == TabValidationResults) 
			{
				theActiveTab = ProjectFormActiveTab.VALIDATION_RESULTS_TAB;
			}

			if (TabControl.SelectedTab == TabSpecifySopClasses) 
			{
				theActiveTab = ProjectFormActiveTab.SPECIFY_SOP_CLASSES_TAB;
			}

			if (TabControl.SelectedTab == TabResultsManager)
			{
				theActiveTab = ProjectFormActiveTab.RESULTS_MANAGER_TAB;
			}
			
			return theActiveTab;
		}
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="theProject"></param>
        /// <param name="theMainForm"></param>
		public ProjectForm2(DvtkApplicationLayer.Project theProject, MainForm theMainForm) 
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.userControlSessionTree.projectApp = theProject;
			userControlSessionTree.ProjectForm = this;
			_ActivityReportEventHandler = new Dvtk.Events.ActivityReportEventHandler(TCM_OnActivityReportEvent);
			projectApp = theProject;
			_MainForm = theMainForm;
			ListBoxSecuritySettings.SelectedIndex = 0;
			_SopClassesManager = new SopClassesManager(this, DataGridSpecifySopClasses, ComboBoxSpecifySopClassesAeTitle, ListBoxSpecifySopClassesDefinitionFileDirectories, RichTextBoxSpecifySopClassesInfo, userControlSessionTree, ButtonSpecifySopClassesRemoveDirectory);
            _TCM_ValidationResultsBrowser = new ValidationResultsManager(webBrowserValResult);
            //_TCM_ValidationResultsManagerBrowser = new ValidationResultsManager(webBrowserResultMgr);
			
            // Because the webbrowser navigation is "cancelled" when browsing to an .xml file
			// first another html file has to be shown to make this work under Windows 2000.
            _TCM_ValidationResultsBrowser.ShowHtml("about:blank");
			_TCM_AppendTextToActivityLogging_ThreadSafe_Delegate = new TCM_AppendTextToActivityLogging_ThreadSafe_Delegate(this.TCM_AppendTextToActivityLogging_ThreadSafe);
			
            resultProjectXml  = projectApp.ProjectFileName + ".xml";			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) 
		{
			if( disposing ) 
			{
				if(components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public ValidationResultsManager TCM_GetValidationResultsManager() 
		{
            return (_TCM_ValidationResultsBrowser);
		}

        public WebBrowser getWebBrowserScript() 
		{
            return (webBrowserScript);
		}

        public WebBrowser getValidationResultsManagerBrowser() 
		{
            return (webBrowserResultMgr);
		}
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() 
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectForm2));
            this.ContextMenu_AddExistingSessions = new System.Windows.Forms.MenuItem();
            this.ContextMenu_AddNewSession = new System.Windows.Forms.MenuItem();
            this.ContextMenu_Edit = new System.Windows.Forms.MenuItem();
            this.ContextMenu_Execute = new System.Windows.Forms.MenuItem();
            this.ContextMenu_ExploreResultsDir = new System.Windows.Forms.MenuItem();
            this.ContextMenu_ExploreScriptsDir = new System.Windows.Forms.MenuItem();
            this.ContextMenu_None = new System.Windows.Forms.MenuItem();
            this.ContextMenu_Remove = new System.Windows.Forms.MenuItem();
            this.ContextMenu_RemoveAllResultsFiles = new System.Windows.Forms.MenuItem();
            this.ContextMenu_RemoveSessionFromProject = new System.Windows.Forms.MenuItem();
            this.ContextMenu_Save = new System.Windows.Forms.MenuItem();
            this.ContextMenu_SaveAs = new System.Windows.Forms.MenuItem();
            this.ContextMenu_ValidateMediaFiles = new System.Windows.Forms.MenuItem();
            this.ContextMenu_ViewExpandedScript = new System.Windows.Forms.MenuItem();
            this.ContextMenu_GenerateDICOMDIR = new System.Windows.Forms.MenuItem();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabSessionInformation = new System.Windows.Forms.TabPage();
            this.PanelSecuritySettingsContent = new System.Windows.Forms.Panel();
            this.ListBoxSecuritySettings = new System.Windows.Forms.ListBox();
            this.LabelCategories = new System.Windows.Forms.Label();
            this.LabelSelect1ItemMsg = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.GroupSecurityFiles = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxSecurityCredentialsFile = new System.Windows.Forms.TextBox();
            this.TextBoxTrustedCertificatesFile = new System.Windows.Forms.TextBox();
            this.ButtonSecurityCredentialsFile = new System.Windows.Forms.Button();
            this.ButtonTrustedCertificatesFile = new System.Windows.Forms.Button();
            this.LabelSecurityCredentialsFile = new System.Windows.Forms.Label();
            this.LabelTrustedCertificatesFile = new System.Windows.Forms.Label();
            this.ButtonCreateCertificate = new System.Windows.Forms.Button();
            this.ButtonViewCertificates = new System.Windows.Forms.Button();
            this.ButtonViewCredentials = new System.Windows.Forms.Button();
            this.GroupSecurityVersion = new System.Windows.Forms.GroupBox();
            this.CheckBoxTLS = new System.Windows.Forms.CheckBox();
            this.CheckBoxSSL = new System.Windows.Forms.CheckBox();
            this.GroupSecurityKeyExchange = new System.Windows.Forms.GroupBox();
            this.RadioButtonKeyExchangeRSA = new System.Windows.Forms.RadioButton();
            this.RadioButtonKeyExchangeDH = new System.Windows.Forms.RadioButton();
            this.GroupSecurityGeneral = new System.Windows.Forms.GroupBox();
            this.CheckBoxCheckRemoteCertificates = new System.Windows.Forms.CheckBox();
            this.CheckBoxCacheSecureSessions = new System.Windows.Forms.CheckBox();
            this.GroupSecurityEncryption = new System.Windows.Forms.GroupBox();
            this.RadioButtonEncryptionNone = new System.Windows.Forms.RadioButton();
            this.RadioButtonEncryptionTripleDES = new System.Windows.Forms.RadioButton();
            this.RadioButtonEncryptionAES128 = new System.Windows.Forms.RadioButton();
            this.RadioButtonEncryptionAES256 = new System.Windows.Forms.RadioButton();
            this.GroupSecurityDataIntegrity = new System.Windows.Forms.GroupBox();
            this.RadioButtonDataIntegritySHA = new System.Windows.Forms.RadioButton();
            this.RadioButtonDataIntegrityMD5 = new System.Windows.Forms.RadioButton();
            this.GroupSecurityAuthentication = new System.Windows.Forms.GroupBox();
            this.RadioButtonAuthenticationRSA = new System.Windows.Forms.RadioButton();
            this.RadioButtonAuthenticationDSA = new System.Windows.Forms.RadioButton();
            this.PanelSecuritySettingsTitle = new System.Windows.Forms.Panel();
            this.CheckBoxSecureConnection = new System.Windows.Forms.CheckBox();
            this.MinSecuritySettings = new System.Windows.Forms.PictureBox();
            this.MaxSecuritySettings = new System.Windows.Forms.PictureBox();
            this.PanelSUTSettingContent = new System.Windows.Forms.Panel();
            this.LabelSUTMaxPDU = new System.Windows.Forms.Label();
            this.LabelSUTAETitle = new System.Windows.Forms.Label();
            this.LabelSUTTCPIPAddress = new System.Windows.Forms.Label();
            this.LabelSUTListenPort = new System.Windows.Forms.Label();
            this.TextBoxSUTAETitle = new System.Windows.Forms.TextBox();
            this.TextBoxSUTTCPIPAddress = new System.Windows.Forms.TextBox();
            this.NumericSUTListenPort = new System.Windows.Forms.NumericUpDown();
            this.NumericSUTMaxPDU = new System.Windows.Forms.NumericUpDown();
            this.LabelSUTImplClassUID = new System.Windows.Forms.Label();
            this.LabelSUTImplVersionName = new System.Windows.Forms.Label();
            this.TextBoxSUTImplClassUID = new System.Windows.Forms.TextBox();
            this.TextBoxSUTImplVersionName = new System.Windows.Forms.TextBox();
            this.PanelSUTSettingTitle = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.MinSUTSettings = new System.Windows.Forms.PictureBox();
            this.MaxSUTSettings = new System.Windows.Forms.PictureBox();
            this.PanelDVTRoleSettingsContent = new System.Windows.Forms.Panel();
            this.NumericDVTListenPort = new System.Windows.Forms.NumericUpDown();
            this.TextBoxDVTAETitle = new System.Windows.Forms.TextBox();
            this.LabelDVTAETitle = new System.Windows.Forms.Label();
            this.LabelDVTListenPort = new System.Windows.Forms.Label();
            this.LabelDVTSocketTimeOut = new System.Windows.Forms.Label();
            this.LabelDVTMaxPDU = new System.Windows.Forms.Label();
            this.NumericDVTTimeOut = new System.Windows.Forms.NumericUpDown();
            this.NumericDVTMaxPDU = new System.Windows.Forms.NumericUpDown();
            this.TextBoxDVTImplClassUID = new System.Windows.Forms.TextBox();
            this.TextBoxDVTImplVersionName = new System.Windows.Forms.TextBox();
            this.LabelDVTImplClassUID = new System.Windows.Forms.Label();
            this.LabelDVTImplVersionName = new System.Windows.Forms.Label();
            this.PanelDVTRoleSettingsTitle = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.MinDVTRoleSettings = new System.Windows.Forms.PictureBox();
            this.MaxDVTRoleSettings = new System.Windows.Forms.PictureBox();
            this.PanelGeneralPropertiesContent = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.ButtonBrowseDataDirectory = new System.Windows.Forms.Button();
            this.TextBoxDataRoot = new System.Windows.Forms.TextBox();
            this.ButtonBrowseDescriptionDir = new System.Windows.Forms.Button();
            this.CheckBoxDisplayCondition = new System.Windows.Forms.CheckBox();
            this.CheckBoxAddGroupLengths = new System.Windows.Forms.CheckBox();
            this.CheckBoxDefineSQLength = new System.Windows.Forms.CheckBox();
            this.ComboBoxStorageMode = new System.Windows.Forms.ComboBox();
            this.CheckBoxLogRelation = new System.Windows.Forms.CheckBox();
            this.labelStorageMode = new System.Windows.Forms.Label();
            this.CheckBoxGenerateDetailedValidationResults = new System.Windows.Forms.CheckBox();
            this.TextBoxSessionType = new System.Windows.Forms.TextBox();
            this.NumericSessonID = new System.Windows.Forms.NumericUpDown();
            this.LabelResultsDir = new System.Windows.Forms.Label();
            this.LabelSessionType = new System.Windows.Forms.Label();
            this.LabelDate = new System.Windows.Forms.Label();
            this.LabelSessionTitle = new System.Windows.Forms.Label();
            this.LabelTestedBy = new System.Windows.Forms.Label();
            this.TextBoxTestedBy = new System.Windows.Forms.TextBox();
            this.TextBoxResultsRoot = new System.Windows.Forms.TextBox();
            this.TextBoxScriptRoot = new System.Windows.Forms.TextBox();
            this.LabelScriptsDir = new System.Windows.Forms.Label();
            this.TextBoxSessionTitle = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.LabelSpecifyTransferSyntaxes = new System.Windows.Forms.Label();
            this.LabelDescriptionDir = new System.Windows.Forms.Label();
            this.TextBoxDescriptionRoot = new System.Windows.Forms.TextBox();
            this.ButtonBrowseResultsDir = new System.Windows.Forms.Button();
            this.ButtonBrowseScriptsDir = new System.Windows.Forms.Button();
            this.ButtonSpecifyTransferSyntaxes = new System.Windows.Forms.Button();
            this.PanelGeneralPropertiesTitle = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.MinGSPSettings = new System.Windows.Forms.PictureBox();
            this.MaxGSPSettings = new System.Windows.Forms.PictureBox();
            this.TabSpecifySopClasses = new System.Windows.Forms.TabPage();
            this.DataGridSpecifySopClasses = new System.Windows.Forms.DataGrid();
            this.ContextMenuDataGrid = new System.Windows.Forms.ContextMenu();
            this.ContextMenu_SelectAllDefinitionFiles = new System.Windows.Forms.MenuItem();
            this.ContextMenu_UnSelectAllDefinitionFiles = new System.Windows.Forms.MenuItem();
            this.ContextMenu_OpenDefFile = new System.Windows.Forms.MenuItem();
            this.RichTextBoxSpecifySopClassesInfo = new System.Windows.Forms.RichTextBox();
            this.ContextMenuRichTextBox = new System.Windows.Forms.ContextMenu();
            this.ContextMenu_Copy = new System.Windows.Forms.MenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ButtonUnselectAll = new System.Windows.Forms.Button();
            this.ButtonSelectAllDefinitionFiles = new System.Windows.Forms.Button();
            this.ButtonSpecifySopClassesAddDirectory = new System.Windows.Forms.Button();
            this.ButtonSpecifySopClassesRemoveDirectory = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ListBoxSpecifySopClassesDefinitionFileDirectories = new System.Windows.Forms.ListBox();
            this.LabelSpecifySopClassesDefinitionFileDirectories = new System.Windows.Forms.Label();
            this.LabelSpecifySopClassesSelectAeTitle = new System.Windows.Forms.Label();
            this.ComboBoxSpecifySopClassesAeTitle = new System.Windows.Forms.ComboBox();
            this.TabActivityLogging = new System.Windows.Forms.TabPage();
            this.RichTextBoxActivityLogging = new System.Windows.Forms.RichTextBox();
            this.TabValidationResults = new System.Windows.Forms.TabPage();
            this.WebBrowserPanel = new System.Windows.Forms.Panel();
            this.webBrowserValResult = new System.Windows.Forms.WebBrowser();
            this.TabResultsManager = new System.Windows.Forms.TabPage();
            this.PanelWebBrowserResultsManager = new System.Windows.Forms.Panel();
            this.webBrowserResultMgr = new System.Windows.Forms.WebBrowser();
            this.PanelResultsManagerTiltle = new System.Windows.Forms.Panel();
            this.LabelIssueText = new System.Windows.Forms.Label();
            this.PictureBoxMinResultsTab = new System.Windows.Forms.PictureBox();
            this.PictureBoxMaximizeResultTab = new System.Windows.Forms.PictureBox();
            this.TabControlIssues = new System.Windows.Forms.TabControl();
            this.AddIssue = new System.Windows.Forms.TabPage();
            this.LabelAddMessageText = new System.Windows.Forms.Label();
            this.TextBoxAddMessageText = new System.Windows.Forms.TextBox();
            this.ButtonAddIssue = new System.Windows.Forms.Button();
            this.TextBoxAddPR = new System.Windows.Forms.TextBox();
            this.TextBoxAddComent = new System.Windows.Forms.TextBox();
            this.LabelAddPr = new System.Windows.Forms.Label();
            this.LabelAddComent = new System.Windows.Forms.Label();
            this.RemoveIssue = new System.Windows.Forms.TabPage();
            this.LabelErrorMessage = new System.Windows.Forms.Label();
            this.ButtonSaveIssue = new System.Windows.Forms.Button();
            this.TextBoxPr = new System.Windows.Forms.TextBox();
            this.TextBoxComment = new System.Windows.Forms.TextBox();
            this.checkBoxIgnoreResult = new System.Windows.Forms.CheckBox();
            this.checkBoxIgnoreResultAll = new System.Windows.Forms.CheckBox();
            this.LabelPr = new System.Windows.Forms.Label();
            this.LabelComment = new System.Windows.Forms.Label();
            this.TabScript = new System.Windows.Forms.TabPage();
            this.RichTextBoxScript = new System.Windows.Forms.RichTextBox();
            this.webBrowserScript = new System.Windows.Forms.WebBrowser();
            this.TabEmpty = new System.Windows.Forms.TabPage();
            this.webBrowserStartScreen = new System.Windows.Forms.WebBrowser();
            this.TabNoInformationAvailable = new System.Windows.Forms.TabPage();
            this.LabelNoInformationAvailable = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.TheFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuUserControlSessionTree = new System.Windows.Forms.ContextMenu();
            this.ContextMenu_GenerateDICOMDIRWithDirectory = new System.Windows.Forms.MenuItem();
            this.ContextMenu_ValidateDicomdirWithoutRefFile = new System.Windows.Forms.MenuItem();
            this.ContextMenu_ValidateMediaDirectory = new System.Windows.Forms.MenuItem();
            this.MainMenu_ProjectForm = new System.Windows.Forms.MainMenu(this.components);
            this.userControlSessionTree = new Dvt.UserControlSessionTree();
            this.TabControl.SuspendLayout();
            this.TabSessionInformation.SuspendLayout();
            this.PanelSecuritySettingsContent.SuspendLayout();
            this.GroupSecurityFiles.SuspendLayout();
            this.GroupSecurityVersion.SuspendLayout();
            this.GroupSecurityKeyExchange.SuspendLayout();
            this.GroupSecurityGeneral.SuspendLayout();
            this.GroupSecurityEncryption.SuspendLayout();
            this.GroupSecurityDataIntegrity.SuspendLayout();
            this.GroupSecurityAuthentication.SuspendLayout();
            this.PanelSecuritySettingsTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinSecuritySettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxSecuritySettings)).BeginInit();
            this.PanelSUTSettingContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTListenPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTMaxPDU)).BeginInit();
            this.PanelSUTSettingTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinSUTSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxSUTSettings)).BeginInit();
            this.PanelDVTRoleSettingsContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTListenPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTTimeOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTMaxPDU)).BeginInit();
            this.PanelDVTRoleSettingsTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinDVTRoleSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDVTRoleSettings)).BeginInit();
            this.PanelGeneralPropertiesContent.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSessonID)).BeginInit();
            this.PanelGeneralPropertiesTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinGSPSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxGSPSettings)).BeginInit();
            this.TabSpecifySopClasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSpecifySopClasses)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.TabActivityLogging.SuspendLayout();
            this.TabValidationResults.SuspendLayout();
            this.WebBrowserPanel.SuspendLayout();
            this.TabResultsManager.SuspendLayout();
            this.PanelWebBrowserResultsManager.SuspendLayout();
            this.PanelResultsManagerTiltle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMinResultsTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMaximizeResultTab)).BeginInit();
            this.TabControlIssues.SuspendLayout();
            this.AddIssue.SuspendLayout();
            this.RemoveIssue.SuspendLayout();
            this.TabScript.SuspendLayout();
            this.TabEmpty.SuspendLayout();
            this.TabNoInformationAvailable.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContextMenu_AddExistingSessions
            // 
            this.ContextMenu_AddExistingSessions.Index = 0;
            this.ContextMenu_AddExistingSessions.Text = "Add Existing Session(s)...";
            this.ContextMenu_AddExistingSessions.Click += new System.EventHandler(this.ContextMenu_AddExistingSession_Click);
            // 
            // ContextMenu_AddNewSession
            // 
            this.ContextMenu_AddNewSession.Index = 1;
            this.ContextMenu_AddNewSession.Text = "Add New Session...";
            this.ContextMenu_AddNewSession.Click += new System.EventHandler(this.ContextMenu_AddNewSession_Click);
            // 
            // ContextMenu_Edit
            // 
            this.ContextMenu_Edit.Index = 2;
            this.ContextMenu_Edit.Text = "Edit Script with Notepad...";
            this.ContextMenu_Edit.Click += new System.EventHandler(this.ContextMenu_Edit_Click);
            // 
            // ContextMenu_Execute
            // 
            this.ContextMenu_Execute.Index = 3;
            this.ContextMenu_Execute.Text = "Execute";
            this.ContextMenu_Execute.Click += new System.EventHandler(this.ContextMenu_Execute_Click);
            // 
            // ContextMenu_ExploreResultsDir
            // 
            this.ContextMenu_ExploreResultsDir.Index = 4;
            this.ContextMenu_ExploreResultsDir.Text = "Explore Results Directory...";
            this.ContextMenu_ExploreResultsDir.Click += new System.EventHandler(this.ContextMenu_ExploreResultsDir_Click);
            // 
            // ContextMenu_ExploreScriptsDir
            // 
            this.ContextMenu_ExploreScriptsDir.Index = 5;
            this.ContextMenu_ExploreScriptsDir.Text = "Explore Scripts Directory...";
            this.ContextMenu_ExploreScriptsDir.Click += new System.EventHandler(this.ContextMenu_ExploreScriptsDir_Click);
            // 
            // ContextMenu_None
            // 
            this.ContextMenu_None.Enabled = false;
            this.ContextMenu_None.Index = 6;
            this.ContextMenu_None.Text = "None";
            // 
            // ContextMenu_Remove
            // 
            this.ContextMenu_Remove.Index = 7;
            this.ContextMenu_Remove.Text = "Remove";
            this.ContextMenu_Remove.Click += new System.EventHandler(this.ContextMenu_Remove_Click);
            // 
            // ContextMenu_RemoveAllResultsFiles
            // 
            this.ContextMenu_RemoveAllResultsFiles.Index = 8;
            this.ContextMenu_RemoveAllResultsFiles.Text = "Remove all Results Files";
            this.ContextMenu_RemoveAllResultsFiles.Click += new System.EventHandler(this.ContextMenu_RemoveAllResultFiles_Click);
            // 
            // ContextMenu_RemoveSessionFromProject
            // 
            this.ContextMenu_RemoveSessionFromProject.Index = 9;
            this.ContextMenu_RemoveSessionFromProject.Text = "Remove from Project";
            this.ContextMenu_RemoveSessionFromProject.Click += new System.EventHandler(this.ContextMenu_RemoveSessionFromProject_Click);
            // 
            // ContextMenu_Save
            // 
            this.ContextMenu_Save.Index = 10;
            this.ContextMenu_Save.Text = "Save";
            this.ContextMenu_Save.Click += new System.EventHandler(this.ContextMenu_Save_Click);
            // 
            // ContextMenu_SaveAs
            // 
            this.ContextMenu_SaveAs.Index = 11;
            this.ContextMenu_SaveAs.Text = "Save As...";
            this.ContextMenu_SaveAs.Click += new System.EventHandler(this.ContextMenu_SaveAs_Click);
            // 
            // ContextMenu_ValidateMediaFiles
            // 
            this.ContextMenu_ValidateMediaFiles.Index = 12;
            this.ContextMenu_ValidateMediaFiles.Text = "Validate Media File(s)...";
            this.ContextMenu_ValidateMediaFiles.Click += new System.EventHandler(this.ContextMenu_ValidateMediaFiles_Click);
            // 
            // ContextMenu_ViewExpandedScript
            // 
            this.ContextMenu_ViewExpandedScript.Index = 15;
            this.ContextMenu_ViewExpandedScript.Text = "View Expanded Script with Notepad...";
            this.ContextMenu_ViewExpandedScript.Click += new System.EventHandler(this.ContextMenu_ViewExpandedScript_Click);
            // 
            // ContextMenu_GenerateDICOMDIR
            // 
            this.ContextMenu_GenerateDICOMDIR.Index = 13;
            this.ContextMenu_GenerateDICOMDIR.Text = "Create DICOMDIR";
            this.ContextMenu_GenerateDICOMDIR.Click += new System.EventHandler(this.ContextMenu_GenerateDICOMDIR_Click);
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.TabSessionInformation);
            this.TabControl.Controls.Add(this.TabSpecifySopClasses);
            this.TabControl.Controls.Add(this.TabActivityLogging);
            this.TabControl.Controls.Add(this.TabValidationResults);
            this.TabControl.Controls.Add(this.TabResultsManager);
            this.TabControl.Controls.Add(this.TabScript);
            this.TabControl.Controls.Add(this.TabEmpty);
            this.TabControl.Controls.Add(this.TabNoInformationAvailable);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(212, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(752, 725);
            this.TabControl.TabIndex = 2;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // TabSessionInformation
            // 
            this.TabSessionInformation.AutoScroll = true;
            this.TabSessionInformation.Controls.Add(this.PanelSecuritySettingsContent);
            this.TabSessionInformation.Controls.Add(this.PanelSecuritySettingsTitle);
            this.TabSessionInformation.Controls.Add(this.PanelSUTSettingContent);
            this.TabSessionInformation.Controls.Add(this.PanelSUTSettingTitle);
            this.TabSessionInformation.Controls.Add(this.PanelDVTRoleSettingsContent);
            this.TabSessionInformation.Controls.Add(this.PanelDVTRoleSettingsTitle);
            this.TabSessionInformation.Controls.Add(this.PanelGeneralPropertiesContent);
            this.TabSessionInformation.Controls.Add(this.PanelGeneralPropertiesTitle);
            this.TabSessionInformation.Location = new System.Drawing.Point(4, 25);
            this.TabSessionInformation.Name = "TabSessionInformation";
            this.TabSessionInformation.Padding = new System.Windows.Forms.Padding(15);
            this.TabSessionInformation.Size = new System.Drawing.Size(744, 696);
            this.TabSessionInformation.TabIndex = 0;
            this.TabSessionInformation.Text = "Session Information";
            this.TabSessionInformation.UseVisualStyleBackColor = true;
            // 
            // PanelSecuritySettingsContent
            // 
            this.PanelSecuritySettingsContent.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PanelSecuritySettingsContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSecuritySettingsContent.Controls.Add(this.ListBoxSecuritySettings);
            this.PanelSecuritySettingsContent.Controls.Add(this.LabelCategories);
            this.PanelSecuritySettingsContent.Controls.Add(this.LabelSelect1ItemMsg);
            this.PanelSecuritySettingsContent.Controls.Add(this.label28);
            this.PanelSecuritySettingsContent.Controls.Add(this.GroupSecurityFiles);
            this.PanelSecuritySettingsContent.Controls.Add(this.GroupSecurityVersion);
            this.PanelSecuritySettingsContent.Controls.Add(this.GroupSecurityKeyExchange);
            this.PanelSecuritySettingsContent.Controls.Add(this.GroupSecurityGeneral);
            this.PanelSecuritySettingsContent.Controls.Add(this.GroupSecurityEncryption);
            this.PanelSecuritySettingsContent.Controls.Add(this.GroupSecurityDataIntegrity);
            this.PanelSecuritySettingsContent.Controls.Add(this.GroupSecurityAuthentication);
            this.PanelSecuritySettingsContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelSecuritySettingsContent.Location = new System.Drawing.Point(15, 884);
            this.PanelSecuritySettingsContent.Name = "PanelSecuritySettingsContent";
            this.PanelSecuritySettingsContent.Size = new System.Drawing.Size(693, 231);
            this.PanelSecuritySettingsContent.TabIndex = 6;
            this.PanelSecuritySettingsContent.TabStop = true;
            this.PanelSecuritySettingsContent.Click += new System.EventHandler(this.PanelSecuritySettingsContent_Click);
            // 
            // ListBoxSecuritySettings
            // 
            this.ListBoxSecuritySettings.ItemHeight = 16;
            this.ListBoxSecuritySettings.Items.AddRange(new object[] {
            "General",
            "Version",
            "Authentication",
            "Key Exchange",
            "Data Integrity",
            "Encryption",
            "Keys"});
            this.ListBoxSecuritySettings.Location = new System.Drawing.Point(10, 37);
            this.ListBoxSecuritySettings.Name = "ListBoxSecuritySettings";
            this.ListBoxSecuritySettings.Size = new System.Drawing.Size(144, 132);
            this.ListBoxSecuritySettings.TabIndex = 0;
            this.ListBoxSecuritySettings.SelectedIndexChanged += new System.EventHandler(this.ListBoxSecuritySettings_SelectedIndexChanged);
            // 
            // LabelCategories
            // 
            this.LabelCategories.Location = new System.Drawing.Point(10, 9);
            this.LabelCategories.Name = "LabelCategories";
            this.LabelCategories.Size = new System.Drawing.Size(120, 27);
            this.LabelCategories.TabIndex = 0;
            this.LabelCategories.Text = "Categories:";
            // 
            // LabelSelect1ItemMsg
            // 
            this.LabelSelect1ItemMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelSelect1ItemMsg.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelSelect1ItemMsg.ForeColor = System.Drawing.Color.Red;
            this.LabelSelect1ItemMsg.Location = new System.Drawing.Point(307, 9);
            this.LabelSelect1ItemMsg.Name = "LabelSelect1ItemMsg";
            this.LabelSelect1ItemMsg.Size = new System.Drawing.Size(376, 19);
            this.LabelSelect1ItemMsg.TabIndex = 0;
            this.LabelSelect1ItemMsg.Text = "At least 1 item must be selected!";
            this.LabelSelect1ItemMsg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LabelSelect1ItemMsg.Visible = false;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(163, 9);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(58, 19);
            this.label28.TabIndex = 0;
            this.label28.Text = "Settings:";
            // 
            // GroupSecurityFiles
            // 
            this.GroupSecurityFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSecurityFiles.Controls.Add(this.label3);
            this.GroupSecurityFiles.Controls.Add(this.TextBoxSecurityCredentialsFile);
            this.GroupSecurityFiles.Controls.Add(this.TextBoxTrustedCertificatesFile);
            this.GroupSecurityFiles.Controls.Add(this.ButtonSecurityCredentialsFile);
            this.GroupSecurityFiles.Controls.Add(this.ButtonTrustedCertificatesFile);
            this.GroupSecurityFiles.Controls.Add(this.LabelSecurityCredentialsFile);
            this.GroupSecurityFiles.Controls.Add(this.LabelTrustedCertificatesFile);
            this.GroupSecurityFiles.Controls.Add(this.ButtonCreateCertificate);
            this.GroupSecurityFiles.Controls.Add(this.ButtonViewCertificates);
            this.GroupSecurityFiles.Controls.Add(this.ButtonViewCredentials);
            this.GroupSecurityFiles.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupSecurityFiles.Location = new System.Drawing.Point(163, 28);
            this.GroupSecurityFiles.Name = "GroupSecurityFiles";
            this.GroupSecurityFiles.Size = new System.Drawing.Size(509, 184);
            this.GroupSecurityFiles.TabIndex = 1;
            this.GroupSecurityFiles.TabStop = false;
            this.GroupSecurityFiles.Text = "Files";
            this.GroupSecurityFiles.Visible = false;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(269, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Create Public\\Private key pair:";
            // 
            // TextBoxSecurityCredentialsFile
            // 
            this.TextBoxSecurityCredentialsFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSecurityCredentialsFile.Location = new System.Drawing.Point(19, 120);
            this.TextBoxSecurityCredentialsFile.Name = "TextBoxSecurityCredentialsFile";
            this.TextBoxSecurityCredentialsFile.ReadOnly = true;
            this.TextBoxSecurityCredentialsFile.Size = new System.Drawing.Size(365, 22);
            this.TextBoxSecurityCredentialsFile.TabIndex = 0;
            this.TextBoxSecurityCredentialsFile.TabStop = false;
            // 
            // TextBoxTrustedCertificatesFile
            // 
            this.TextBoxTrustedCertificatesFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxTrustedCertificatesFile.Location = new System.Drawing.Point(19, 53);
            this.TextBoxTrustedCertificatesFile.Name = "TextBoxTrustedCertificatesFile";
            this.TextBoxTrustedCertificatesFile.ReadOnly = true;
            this.TextBoxTrustedCertificatesFile.Size = new System.Drawing.Size(365, 22);
            this.TextBoxTrustedCertificatesFile.TabIndex = 0;
            this.TextBoxTrustedCertificatesFile.TabStop = false;
            // 
            // ButtonSecurityCredentialsFile
            // 
            this.ButtonSecurityCredentialsFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonSecurityCredentialsFile.Location = new System.Drawing.Point(317, 84);
            this.ButtonSecurityCredentialsFile.Name = "ButtonSecurityCredentialsFile";
            this.ButtonSecurityCredentialsFile.Size = new System.Drawing.Size(90, 27);
            this.ButtonSecurityCredentialsFile.TabIndex = 2;
            this.ButtonSecurityCredentialsFile.Text = "Browse";
            this.ButtonSecurityCredentialsFile.Click += new System.EventHandler(this.ButtonSecurityCredentialsFile_Click);
            // 
            // ButtonTrustedCertificatesFile
            // 
            this.ButtonTrustedCertificatesFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonTrustedCertificatesFile.Location = new System.Drawing.Point(317, 20);
            this.ButtonTrustedCertificatesFile.Name = "ButtonTrustedCertificatesFile";
            this.ButtonTrustedCertificatesFile.Size = new System.Drawing.Size(90, 26);
            this.ButtonTrustedCertificatesFile.TabIndex = 0;
            this.ButtonTrustedCertificatesFile.Text = "Browse";
            this.ButtonTrustedCertificatesFile.Click += new System.EventHandler(this.ButtonTrustedCertificatesFile_Click);
            // 
            // LabelSecurityCredentialsFile
            // 
            this.LabelSecurityCredentialsFile.Location = new System.Drawing.Point(19, 90);
            this.LabelSecurityCredentialsFile.Name = "LabelSecurityCredentialsFile";
            this.LabelSecurityCredentialsFile.Size = new System.Drawing.Size(298, 27);
            this.LabelSecurityCredentialsFile.TabIndex = 0;
            this.LabelSecurityCredentialsFile.Text = "File containing DVT Private Keys (credentials):";
            // 
            // LabelTrustedCertificatesFile
            // 
            this.LabelTrustedCertificatesFile.Location = new System.Drawing.Point(19, 25);
            this.LabelTrustedCertificatesFile.Name = "LabelTrustedCertificatesFile";
            this.LabelTrustedCertificatesFile.Size = new System.Drawing.Size(298, 37);
            this.LabelTrustedCertificatesFile.TabIndex = 0;
            this.LabelTrustedCertificatesFile.Text = "File containing SUT Public Keys (certificates):";
            // 
            // ButtonCreateCertificate
            // 
            this.ButtonCreateCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCreateCertificate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonCreateCertificate.Location = new System.Drawing.Point(403, 150);
            this.ButtonCreateCertificate.Name = "ButtonCreateCertificate";
            this.ButtonCreateCertificate.Size = new System.Drawing.Size(90, 27);
            this.ButtonCreateCertificate.TabIndex = 4;
            this.ButtonCreateCertificate.Text = "Create";
            this.ButtonCreateCertificate.Click += new System.EventHandler(this.ButtonCreateCertificate_Click);
            // 
            // ButtonViewCertificates
            // 
            this.ButtonViewCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonViewCertificates.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonViewCertificates.Location = new System.Drawing.Point(403, 51);
            this.ButtonViewCertificates.Name = "ButtonViewCertificates";
            this.ButtonViewCertificates.Size = new System.Drawing.Size(90, 26);
            this.ButtonViewCertificates.TabIndex = 1;
            this.ButtonViewCertificates.Text = "Edit";
            this.ButtonViewCertificates.Click += new System.EventHandler(this.ButtonViewCertificates_Click);
            // 
            // ButtonViewCredentials
            // 
            this.ButtonViewCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonViewCredentials.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonViewCredentials.Location = new System.Drawing.Point(403, 117);
            this.ButtonViewCredentials.Name = "ButtonViewCredentials";
            this.ButtonViewCredentials.Size = new System.Drawing.Size(90, 26);
            this.ButtonViewCredentials.TabIndex = 3;
            this.ButtonViewCredentials.Text = "Edit";
            this.ButtonViewCredentials.Click += new System.EventHandler(this.ButtonViewCredentials_Click);
            // 
            // GroupSecurityVersion
            // 
            this.GroupSecurityVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSecurityVersion.Controls.Add(this.CheckBoxTLS);
            this.GroupSecurityVersion.Controls.Add(this.CheckBoxSSL);
            this.GroupSecurityVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupSecurityVersion.Location = new System.Drawing.Point(163, 28);
            this.GroupSecurityVersion.Name = "GroupSecurityVersion";
            this.GroupSecurityVersion.Size = new System.Drawing.Size(357, 147);
            this.GroupSecurityVersion.TabIndex = 1;
            this.GroupSecurityVersion.TabStop = false;
            this.GroupSecurityVersion.Text = "Version";
            this.GroupSecurityVersion.Visible = false;
            // 
            // CheckBoxTLS
            // 
            this.CheckBoxTLS.Location = new System.Drawing.Point(19, 28);
            this.CheckBoxTLS.Name = "CheckBoxTLS";
            this.CheckBoxTLS.Size = new System.Drawing.Size(77, 27);
            this.CheckBoxTLS.TabIndex = 0;
            this.CheckBoxTLS.Text = "TLS v1";
            this.CheckBoxTLS.CheckedChanged += new System.EventHandler(this.CheckBoxTLS_CheckedChanged);
            // 
            // CheckBoxSSL
            // 
            this.CheckBoxSSL.Location = new System.Drawing.Point(19, 55);
            this.CheckBoxSSL.Name = "CheckBoxSSL";
            this.CheckBoxSSL.Size = new System.Drawing.Size(77, 28);
            this.CheckBoxSSL.TabIndex = 0;
            this.CheckBoxSSL.Text = "SSL v3";
            this.CheckBoxSSL.CheckedChanged += new System.EventHandler(this.CheckBoxSSL_CheckedChanged);
            // 
            // GroupSecurityKeyExchange
            // 
            this.GroupSecurityKeyExchange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSecurityKeyExchange.Controls.Add(this.RadioButtonKeyExchangeRSA);
            this.GroupSecurityKeyExchange.Controls.Add(this.RadioButtonKeyExchangeDH);
            this.GroupSecurityKeyExchange.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupSecurityKeyExchange.Location = new System.Drawing.Point(163, 28);
            this.GroupSecurityKeyExchange.Name = "GroupSecurityKeyExchange";
            this.GroupSecurityKeyExchange.Size = new System.Drawing.Size(357, 147);
            this.GroupSecurityKeyExchange.TabIndex = 1;
            this.GroupSecurityKeyExchange.TabStop = false;
            this.GroupSecurityKeyExchange.Text = "Key Exchange";
            this.GroupSecurityKeyExchange.Visible = false;
            // 
            // RadioButtonKeyExchangeRSA
            // 
            this.RadioButtonKeyExchangeRSA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioButtonKeyExchangeRSA.Location = new System.Drawing.Point(19, 28);
            this.RadioButtonKeyExchangeRSA.Name = "RadioButtonKeyExchangeRSA";
            this.RadioButtonKeyExchangeRSA.Size = new System.Drawing.Size(328, 27);
            this.RadioButtonKeyExchangeRSA.TabIndex = 0;
            this.RadioButtonKeyExchangeRSA.Text = "RSA";
            this.RadioButtonKeyExchangeRSA.CheckedChanged += new System.EventHandler(this.RadioButtonKeyExchangeRSA_CheckedChanged);
            this.RadioButtonKeyExchangeRSA.Click += new System.EventHandler(this.RadioButtonKeyExchangeRSA_Click);
            // 
            // RadioButtonKeyExchangeDH
            // 
            this.RadioButtonKeyExchangeDH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioButtonKeyExchangeDH.Location = new System.Drawing.Point(19, 55);
            this.RadioButtonKeyExchangeDH.Name = "RadioButtonKeyExchangeDH";
            this.RadioButtonKeyExchangeDH.Size = new System.Drawing.Size(328, 28);
            this.RadioButtonKeyExchangeDH.TabIndex = 0;
            this.RadioButtonKeyExchangeDH.Text = "DH";
            this.RadioButtonKeyExchangeDH.CheckedChanged += new System.EventHandler(this.RadioButtonKeyExchangeDH_CheckedChanged);
            this.RadioButtonKeyExchangeDH.Click += new System.EventHandler(this.RadioButtonKeyExchangeDH_Click);
            // 
            // GroupSecurityGeneral
            // 
            this.GroupSecurityGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSecurityGeneral.Controls.Add(this.CheckBoxCheckRemoteCertificates);
            this.GroupSecurityGeneral.Controls.Add(this.CheckBoxCacheSecureSessions);
            this.GroupSecurityGeneral.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupSecurityGeneral.Location = new System.Drawing.Point(163, 28);
            this.GroupSecurityGeneral.Name = "GroupSecurityGeneral";
            this.GroupSecurityGeneral.Size = new System.Drawing.Size(357, 147);
            this.GroupSecurityGeneral.TabIndex = 1;
            this.GroupSecurityGeneral.TabStop = false;
            this.GroupSecurityGeneral.Text = "General";
            // 
            // CheckBoxCheckRemoteCertificates
            // 
            this.CheckBoxCheckRemoteCertificates.Location = new System.Drawing.Point(19, 28);
            this.CheckBoxCheckRemoteCertificates.Name = "CheckBoxCheckRemoteCertificates";
            this.CheckBoxCheckRemoteCertificates.Size = new System.Drawing.Size(221, 27);
            this.CheckBoxCheckRemoteCertificates.TabIndex = 0;
            this.CheckBoxCheckRemoteCertificates.Text = "Check remote certificates";
            this.CheckBoxCheckRemoteCertificates.CheckedChanged += new System.EventHandler(this.CheckBoxCheckRemoteCertificates_CheckedChanged);
            // 
            // CheckBoxCacheSecureSessions
            // 
            this.CheckBoxCacheSecureSessions.Location = new System.Drawing.Point(19, 55);
            this.CheckBoxCacheSecureSessions.Name = "CheckBoxCacheSecureSessions";
            this.CheckBoxCacheSecureSessions.Size = new System.Drawing.Size(221, 28);
            this.CheckBoxCacheSecureSessions.TabIndex = 0;
            this.CheckBoxCacheSecureSessions.Text = "Cache secure sessions";
            this.CheckBoxCacheSecureSessions.CheckedChanged += new System.EventHandler(this.CheckBoxCacheSecureSessions_CheckedChanged);
            // 
            // GroupSecurityEncryption
            // 
            this.GroupSecurityEncryption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSecurityEncryption.Controls.Add(this.RadioButtonEncryptionNone);
            this.GroupSecurityEncryption.Controls.Add(this.RadioButtonEncryptionTripleDES);
            this.GroupSecurityEncryption.Controls.Add(this.RadioButtonEncryptionAES128);
            this.GroupSecurityEncryption.Controls.Add(this.RadioButtonEncryptionAES256);
            this.GroupSecurityEncryption.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupSecurityEncryption.Location = new System.Drawing.Point(163, 28);
            this.GroupSecurityEncryption.Name = "GroupSecurityEncryption";
            this.GroupSecurityEncryption.Size = new System.Drawing.Size(347, 147);
            this.GroupSecurityEncryption.TabIndex = 1;
            this.GroupSecurityEncryption.TabStop = false;
            this.GroupSecurityEncryption.Text = "Encryption";
            this.GroupSecurityEncryption.Visible = false;
            // 
            // RadioButtonEncryptionNone
            // 
            this.RadioButtonEncryptionNone.Location = new System.Drawing.Point(19, 28);
            this.RadioButtonEncryptionNone.Name = "RadioButtonEncryptionNone";
            this.RadioButtonEncryptionNone.Size = new System.Drawing.Size(221, 27);
            this.RadioButtonEncryptionNone.TabIndex = 0;
            this.RadioButtonEncryptionNone.Text = "None";
            this.RadioButtonEncryptionNone.CheckedChanged += new System.EventHandler(this.RadioButtonEncryptionNone_CheckedChanged);
            this.RadioButtonEncryptionNone.Click += new System.EventHandler(this.RadioButtonEncryptionNone_Click);
            // 
            // RadioButtonEncryptionTripleDES
            // 
            this.RadioButtonEncryptionTripleDES.Location = new System.Drawing.Point(19, 55);
            this.RadioButtonEncryptionTripleDES.Name = "RadioButtonEncryptionTripleDES";
            this.RadioButtonEncryptionTripleDES.Size = new System.Drawing.Size(221, 28);
            this.RadioButtonEncryptionTripleDES.TabIndex = 0;
            this.RadioButtonEncryptionTripleDES.Text = "Triple DES";
            this.RadioButtonEncryptionTripleDES.CheckedChanged += new System.EventHandler(this.RadioButtonEncryptionTripleDES_CheckedChanged);
            this.RadioButtonEncryptionTripleDES.Click += new System.EventHandler(this.RadioButtonEncryptionTripleDES_Click);
            // 
            // RadioButtonEncryptionAES128
            // 
            this.RadioButtonEncryptionAES128.Location = new System.Drawing.Point(19, 83);
            this.RadioButtonEncryptionAES128.Name = "RadioButtonEncryptionAES128";
            this.RadioButtonEncryptionAES128.Size = new System.Drawing.Size(221, 28);
            this.RadioButtonEncryptionAES128.TabIndex = 0;
            this.RadioButtonEncryptionAES128.Text = "AES 128-bit";
            this.RadioButtonEncryptionAES128.CheckedChanged += new System.EventHandler(this.RadioButtonEncryptionAES128_CheckedChanged);
            this.RadioButtonEncryptionAES128.Click += new System.EventHandler(this.RadioButtonEncryptionAES128_Click);
            // 
            // RadioButtonEncryptionAES256
            // 
            this.RadioButtonEncryptionAES256.Location = new System.Drawing.Point(19, 111);
            this.RadioButtonEncryptionAES256.Name = "RadioButtonEncryptionAES256";
            this.RadioButtonEncryptionAES256.Size = new System.Drawing.Size(221, 27);
            this.RadioButtonEncryptionAES256.TabIndex = 0;
            this.RadioButtonEncryptionAES256.Text = "AES 256-bit";
            this.RadioButtonEncryptionAES256.CheckedChanged += new System.EventHandler(this.RadioButtonEncryptionAES256_CheckedChanged);
            this.RadioButtonEncryptionAES256.Click += new System.EventHandler(this.RadioButtonEncryptionAES256_Click);
            // 
            // GroupSecurityDataIntegrity
            // 
            this.GroupSecurityDataIntegrity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSecurityDataIntegrity.Controls.Add(this.RadioButtonDataIntegritySHA);
            this.GroupSecurityDataIntegrity.Controls.Add(this.RadioButtonDataIntegrityMD5);
            this.GroupSecurityDataIntegrity.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupSecurityDataIntegrity.Location = new System.Drawing.Point(163, 28);
            this.GroupSecurityDataIntegrity.Name = "GroupSecurityDataIntegrity";
            this.GroupSecurityDataIntegrity.Size = new System.Drawing.Size(357, 147);
            this.GroupSecurityDataIntegrity.TabIndex = 1;
            this.GroupSecurityDataIntegrity.TabStop = false;
            this.GroupSecurityDataIntegrity.Text = "Data Integrity";
            this.GroupSecurityDataIntegrity.Visible = false;
            // 
            // RadioButtonDataIntegritySHA
            // 
            this.RadioButtonDataIntegritySHA.Location = new System.Drawing.Point(19, 28);
            this.RadioButtonDataIntegritySHA.Name = "RadioButtonDataIntegritySHA";
            this.RadioButtonDataIntegritySHA.Size = new System.Drawing.Size(221, 27);
            this.RadioButtonDataIntegritySHA.TabIndex = 0;
            this.RadioButtonDataIntegritySHA.Text = "SHA";
            this.RadioButtonDataIntegritySHA.CheckedChanged += new System.EventHandler(this.RadioButtonDataIntegritySHA_CheckedChanged);
            this.RadioButtonDataIntegritySHA.Click += new System.EventHandler(this.RadioButtonDataIntegritySHA_Click);
            // 
            // RadioButtonDataIntegrityMD5
            // 
            this.RadioButtonDataIntegrityMD5.Location = new System.Drawing.Point(19, 55);
            this.RadioButtonDataIntegrityMD5.Name = "RadioButtonDataIntegrityMD5";
            this.RadioButtonDataIntegrityMD5.Size = new System.Drawing.Size(221, 28);
            this.RadioButtonDataIntegrityMD5.TabIndex = 0;
            this.RadioButtonDataIntegrityMD5.Text = "MD5";
            this.RadioButtonDataIntegrityMD5.CheckedChanged += new System.EventHandler(this.RadioButtonDataIntegrityMD5_CheckedChanged);
            this.RadioButtonDataIntegrityMD5.Click += new System.EventHandler(this.RadioButtonDataIntegrityMD5_Click);
            // 
            // GroupSecurityAuthentication
            // 
            this.GroupSecurityAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupSecurityAuthentication.Controls.Add(this.RadioButtonAuthenticationRSA);
            this.GroupSecurityAuthentication.Controls.Add(this.RadioButtonAuthenticationDSA);
            this.GroupSecurityAuthentication.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GroupSecurityAuthentication.Location = new System.Drawing.Point(163, 28);
            this.GroupSecurityAuthentication.Name = "GroupSecurityAuthentication";
            this.GroupSecurityAuthentication.Size = new System.Drawing.Size(357, 147);
            this.GroupSecurityAuthentication.TabIndex = 1;
            this.GroupSecurityAuthentication.TabStop = false;
            this.GroupSecurityAuthentication.Text = "Authentication";
            this.GroupSecurityAuthentication.Visible = false;
            // 
            // RadioButtonAuthenticationRSA
            // 
            this.RadioButtonAuthenticationRSA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioButtonAuthenticationRSA.Location = new System.Drawing.Point(19, 28);
            this.RadioButtonAuthenticationRSA.Name = "RadioButtonAuthenticationRSA";
            this.RadioButtonAuthenticationRSA.Size = new System.Drawing.Size(328, 27);
            this.RadioButtonAuthenticationRSA.TabIndex = 0;
            this.RadioButtonAuthenticationRSA.Text = "RSA";
            this.RadioButtonAuthenticationRSA.CheckedChanged += new System.EventHandler(this.RadioButtonAuthenticationRSA_CheckedChanged);
            this.RadioButtonAuthenticationRSA.Click += new System.EventHandler(this.RadioButtonAuthenticationRSA_Click);
            // 
            // RadioButtonAuthenticationDSA
            // 
            this.RadioButtonAuthenticationDSA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RadioButtonAuthenticationDSA.Location = new System.Drawing.Point(19, 55);
            this.RadioButtonAuthenticationDSA.Name = "RadioButtonAuthenticationDSA";
            this.RadioButtonAuthenticationDSA.Size = new System.Drawing.Size(328, 28);
            this.RadioButtonAuthenticationDSA.TabIndex = 0;
            this.RadioButtonAuthenticationDSA.Text = "DSA";
            this.RadioButtonAuthenticationDSA.CheckedChanged += new System.EventHandler(this.RadioButtonAuthenticationDSA_CheckedChanged);
            this.RadioButtonAuthenticationDSA.Click += new System.EventHandler(this.RadioButtonAuthenticationDSA_Click);
            // 
            // PanelSecuritySettingsTitle
            // 
            this.PanelSecuritySettingsTitle.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PanelSecuritySettingsTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSecuritySettingsTitle.Controls.Add(this.CheckBoxSecureConnection);
            this.PanelSecuritySettingsTitle.Controls.Add(this.MinSecuritySettings);
            this.PanelSecuritySettingsTitle.Controls.Add(this.MaxSecuritySettings);
            this.PanelSecuritySettingsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelSecuritySettingsTitle.Location = new System.Drawing.Point(15, 847);
            this.PanelSecuritySettingsTitle.Name = "PanelSecuritySettingsTitle";
            this.PanelSecuritySettingsTitle.Size = new System.Drawing.Size(693, 37);
            this.PanelSecuritySettingsTitle.TabIndex = 5;
            // 
            // CheckBoxSecureConnection
            // 
            this.CheckBoxSecureConnection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.CheckBoxSecureConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckBoxSecureConnection.Location = new System.Drawing.Point(10, 5);
            this.CheckBoxSecureConnection.Name = "CheckBoxSecureConnection";
            this.CheckBoxSecureConnection.Size = new System.Drawing.Size(249, 27);
            this.CheckBoxSecureConnection.TabIndex = 0;
            this.CheckBoxSecureConnection.Text = "Security Settings";
            this.CheckBoxSecureConnection.CheckedChanged += new System.EventHandler(this.CheckBoxSecureConnection_CheckedChanged);
            // 
            // MinSecuritySettings
            // 
            this.MinSecuritySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinSecuritySettings.Image = ((System.Drawing.Image)(resources.GetObject("MinSecuritySettings.Image")));
            this.MinSecuritySettings.Location = new System.Drawing.Point(640, 0);
            this.MinSecuritySettings.Name = "MinSecuritySettings";
            this.MinSecuritySettings.Size = new System.Drawing.Size(38, 28);
            this.MinSecuritySettings.TabIndex = 0;
            this.MinSecuritySettings.TabStop = false;
            this.MinSecuritySettings.Click += new System.EventHandler(this.MinSecuritySettings_Click);
            // 
            // MaxSecuritySettings
            // 
            this.MaxSecuritySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxSecuritySettings.Image = ((System.Drawing.Image)(resources.GetObject("MaxSecuritySettings.Image")));
            this.MaxSecuritySettings.Location = new System.Drawing.Point(640, 0);
            this.MaxSecuritySettings.Name = "MaxSecuritySettings";
            this.MaxSecuritySettings.Size = new System.Drawing.Size(38, 28);
            this.MaxSecuritySettings.TabIndex = 0;
            this.MaxSecuritySettings.TabStop = false;
            this.MaxSecuritySettings.Visible = false;
            this.MaxSecuritySettings.Click += new System.EventHandler(this.MaxSecuritySettings_Click);
            // 
            // PanelSUTSettingContent
            // 
            this.PanelSUTSettingContent.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PanelSUTSettingContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSUTSettingContent.Controls.Add(this.LabelSUTMaxPDU);
            this.PanelSUTSettingContent.Controls.Add(this.LabelSUTAETitle);
            this.PanelSUTSettingContent.Controls.Add(this.LabelSUTTCPIPAddress);
            this.PanelSUTSettingContent.Controls.Add(this.LabelSUTListenPort);
            this.PanelSUTSettingContent.Controls.Add(this.TextBoxSUTAETitle);
            this.PanelSUTSettingContent.Controls.Add(this.TextBoxSUTTCPIPAddress);
            this.PanelSUTSettingContent.Controls.Add(this.NumericSUTListenPort);
            this.PanelSUTSettingContent.Controls.Add(this.NumericSUTMaxPDU);
            this.PanelSUTSettingContent.Controls.Add(this.LabelSUTImplClassUID);
            this.PanelSUTSettingContent.Controls.Add(this.LabelSUTImplVersionName);
            this.PanelSUTSettingContent.Controls.Add(this.TextBoxSUTImplClassUID);
            this.PanelSUTSettingContent.Controls.Add(this.TextBoxSUTImplVersionName);
            this.PanelSUTSettingContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelSUTSettingContent.Location = new System.Drawing.Point(15, 663);
            this.PanelSUTSettingContent.Name = "PanelSUTSettingContent";
            this.PanelSUTSettingContent.Size = new System.Drawing.Size(693, 184);
            this.PanelSUTSettingContent.TabIndex = 4;
            this.PanelSUTSettingContent.Click += new System.EventHandler(this.PanelSUTSettingContent_Click);
            // 
            // LabelSUTMaxPDU
            // 
            this.LabelSUTMaxPDU.Location = new System.Drawing.Point(10, 145);
            this.LabelSUTMaxPDU.Name = "LabelSUTMaxPDU";
            this.LabelSUTMaxPDU.Size = new System.Drawing.Size(120, 37);
            this.LabelSUTMaxPDU.TabIndex = 0;
            this.LabelSUTMaxPDU.Text = "Maximum PDU length to receive:";
            // 
            // LabelSUTAETitle
            // 
            this.LabelSUTAETitle.Location = new System.Drawing.Point(10, 9);
            this.LabelSUTAETitle.Name = "LabelSUTAETitle";
            this.LabelSUTAETitle.Size = new System.Drawing.Size(120, 27);
            this.LabelSUTAETitle.TabIndex = 0;
            this.LabelSUTAETitle.Text = "AE Title:";
            // 
            // LabelSUTTCPIPAddress
            // 
            this.LabelSUTTCPIPAddress.Location = new System.Drawing.Point(10, 115);
            this.LabelSUTTCPIPAddress.Name = "LabelSUTTCPIPAddress";
            this.LabelSUTTCPIPAddress.Size = new System.Drawing.Size(120, 38);
            this.LabelSUTTCPIPAddress.TabIndex = 0;
            this.LabelSUTTCPIPAddress.Text = "Remote TCP/IP address:";
            // 
            // LabelSUTListenPort
            // 
            this.LabelSUTListenPort.Location = new System.Drawing.Point(10, 92);
            this.LabelSUTListenPort.Name = "LabelSUTListenPort";
            this.LabelSUTListenPort.Size = new System.Drawing.Size(120, 27);
            this.LabelSUTListenPort.TabIndex = 0;
            this.LabelSUTListenPort.Text = "Listen port:";
            // 
            // TextBoxSUTAETitle
            // 
            this.TextBoxSUTAETitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSUTAETitle.Location = new System.Drawing.Point(144, 9);
            this.TextBoxSUTAETitle.MaxLength = 16;
            this.TextBoxSUTAETitle.Name = "TextBoxSUTAETitle";
            this.TextBoxSUTAETitle.Size = new System.Drawing.Size(373, 22);
            this.TextBoxSUTAETitle.TabIndex = 0;
            this.TextBoxSUTAETitle.TextChanged += new System.EventHandler(this.TextBoxSUTAETitle_TextChanged);
            // 
            // TextBoxSUTTCPIPAddress
            // 
            this.TextBoxSUTTCPIPAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSUTTCPIPAddress.Location = new System.Drawing.Point(144, 120);
            this.TextBoxSUTTCPIPAddress.MaxLength = 0;
            this.TextBoxSUTTCPIPAddress.Name = "TextBoxSUTTCPIPAddress";
            this.TextBoxSUTTCPIPAddress.Size = new System.Drawing.Size(373, 22);
            this.TextBoxSUTTCPIPAddress.TabIndex = 4;
            this.TextBoxSUTTCPIPAddress.TextChanged += new System.EventHandler(this.TextBoxSUTTCPIPAddress_TextChanged);
            // 
            // NumericSUTListenPort
            // 
            this.NumericSUTListenPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericSUTListenPort.Location = new System.Drawing.Point(144, 92);
            this.NumericSUTListenPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumericSUTListenPort.Name = "NumericSUTListenPort";
            this.NumericSUTListenPort.Size = new System.Drawing.Size(373, 22);
            this.NumericSUTListenPort.TabIndex = 3;
            this.NumericSUTListenPort.ValueChanged += new System.EventHandler(this.NumericSUTListenPort_ValueChanged);
            // 
            // NumericSUTMaxPDU
            // 
            this.NumericSUTMaxPDU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericSUTMaxPDU.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.NumericSUTMaxPDU.Location = new System.Drawing.Point(144, 149);
            this.NumericSUTMaxPDU.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.NumericSUTMaxPDU.Name = "NumericSUTMaxPDU";
            this.NumericSUTMaxPDU.Size = new System.Drawing.Size(373, 22);
            this.NumericSUTMaxPDU.TabIndex = 5;
            this.NumericSUTMaxPDU.ValueChanged += new System.EventHandler(this.NumericSUTMaxPDU_ValueChanged);
            // 
            // LabelSUTImplClassUID
            // 
            this.LabelSUTImplClassUID.Location = new System.Drawing.Point(10, 37);
            this.LabelSUTImplClassUID.Name = "LabelSUTImplClassUID";
            this.LabelSUTImplClassUID.Size = new System.Drawing.Size(120, 26);
            this.LabelSUTImplClassUID.TabIndex = 0;
            this.LabelSUTImplClassUID.Text = "Impl. Class UID:";
            // 
            // LabelSUTImplVersionName
            // 
            this.LabelSUTImplVersionName.Location = new System.Drawing.Point(10, 65);
            this.LabelSUTImplVersionName.Name = "LabelSUTImplVersionName";
            this.LabelSUTImplVersionName.Size = new System.Drawing.Size(134, 26);
            this.LabelSUTImplVersionName.TabIndex = 0;
            this.LabelSUTImplVersionName.Text = "Impl. Version Name:";
            // 
            // TextBoxSUTImplClassUID
            // 
            this.TextBoxSUTImplClassUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSUTImplClassUID.Location = new System.Drawing.Point(144, 37);
            this.TextBoxSUTImplClassUID.MaxLength = 64;
            this.TextBoxSUTImplClassUID.Name = "TextBoxSUTImplClassUID";
            this.TextBoxSUTImplClassUID.Size = new System.Drawing.Size(373, 22);
            this.TextBoxSUTImplClassUID.TabIndex = 1;
            this.TextBoxSUTImplClassUID.TextChanged += new System.EventHandler(this.TextBoxSUTImplClassUID_TextChanged);
            // 
            // TextBoxSUTImplVersionName
            // 
            this.TextBoxSUTImplVersionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSUTImplVersionName.Location = new System.Drawing.Point(144, 65);
            this.TextBoxSUTImplVersionName.MaxLength = 16;
            this.TextBoxSUTImplVersionName.Name = "TextBoxSUTImplVersionName";
            this.TextBoxSUTImplVersionName.Size = new System.Drawing.Size(373, 22);
            this.TextBoxSUTImplVersionName.TabIndex = 2;
            this.TextBoxSUTImplVersionName.TextChanged += new System.EventHandler(this.TextBoxSUTImplVersionName_TextChanged);
            // 
            // PanelSUTSettingTitle
            // 
            this.PanelSUTSettingTitle.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PanelSUTSettingTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSUTSettingTitle.Controls.Add(this.label2);
            this.PanelSUTSettingTitle.Controls.Add(this.MinSUTSettings);
            this.PanelSUTSettingTitle.Controls.Add(this.MaxSUTSettings);
            this.PanelSUTSettingTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelSUTSettingTitle.Location = new System.Drawing.Point(15, 626);
            this.PanelSUTSettingTitle.Name = "PanelSUTSettingTitle";
            this.PanelSUTSettingTitle.Size = new System.Drawing.Size(693, 37);
            this.PanelSUTSettingTitle.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "System Under Test Settings";
            // 
            // MinSUTSettings
            // 
            this.MinSUTSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinSUTSettings.Image = ((System.Drawing.Image)(resources.GetObject("MinSUTSettings.Image")));
            this.MinSUTSettings.Location = new System.Drawing.Point(640, 0);
            this.MinSUTSettings.Name = "MinSUTSettings";
            this.MinSUTSettings.Size = new System.Drawing.Size(38, 28);
            this.MinSUTSettings.TabIndex = 0;
            this.MinSUTSettings.TabStop = false;
            this.MinSUTSettings.Click += new System.EventHandler(this.MinSUTSettings_Click);
            // 
            // MaxSUTSettings
            // 
            this.MaxSUTSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxSUTSettings.Image = ((System.Drawing.Image)(resources.GetObject("MaxSUTSettings.Image")));
            this.MaxSUTSettings.Location = new System.Drawing.Point(640, 0);
            this.MaxSUTSettings.Name = "MaxSUTSettings";
            this.MaxSUTSettings.Size = new System.Drawing.Size(38, 28);
            this.MaxSUTSettings.TabIndex = 0;
            this.MaxSUTSettings.TabStop = false;
            this.MaxSUTSettings.Visible = false;
            this.MaxSUTSettings.Click += new System.EventHandler(this.MaxSUTSettings_Click);
            // 
            // PanelDVTRoleSettingsContent
            // 
            this.PanelDVTRoleSettingsContent.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PanelDVTRoleSettingsContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelDVTRoleSettingsContent.Controls.Add(this.NumericDVTListenPort);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.TextBoxDVTAETitle);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.LabelDVTAETitle);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.LabelDVTListenPort);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.LabelDVTSocketTimeOut);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.LabelDVTMaxPDU);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.NumericDVTTimeOut);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.NumericDVTMaxPDU);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.TextBoxDVTImplClassUID);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.TextBoxDVTImplVersionName);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.LabelDVTImplClassUID);
            this.PanelDVTRoleSettingsContent.Controls.Add(this.LabelDVTImplVersionName);
            this.PanelDVTRoleSettingsContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDVTRoleSettingsContent.Location = new System.Drawing.Point(15, 441);
            this.PanelDVTRoleSettingsContent.Name = "PanelDVTRoleSettingsContent";
            this.PanelDVTRoleSettingsContent.Size = new System.Drawing.Size(693, 185);
            this.PanelDVTRoleSettingsContent.TabIndex = 3;
            this.PanelDVTRoleSettingsContent.Click += new System.EventHandler(this.PanelDVTRoleSettingsContent_Click);
            // 
            // NumericDVTListenPort
            // 
            this.NumericDVTListenPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericDVTListenPort.Location = new System.Drawing.Point(144, 92);
            this.NumericDVTListenPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumericDVTListenPort.Name = "NumericDVTListenPort";
            this.NumericDVTListenPort.Size = new System.Drawing.Size(373, 22);
            this.NumericDVTListenPort.TabIndex = 3;
            this.NumericDVTListenPort.ValueChanged += new System.EventHandler(this.NumericDVTListenPort_ValueChanged);
            // 
            // TextBoxDVTAETitle
            // 
            this.TextBoxDVTAETitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDVTAETitle.Location = new System.Drawing.Point(144, 9);
            this.TextBoxDVTAETitle.MaxLength = 16;
            this.TextBoxDVTAETitle.Name = "TextBoxDVTAETitle";
            this.TextBoxDVTAETitle.Size = new System.Drawing.Size(373, 22);
            this.TextBoxDVTAETitle.TabIndex = 0;
            this.TextBoxDVTAETitle.TextChanged += new System.EventHandler(this.TextBoxDVTAETitle_TextChanged);
            // 
            // LabelDVTAETitle
            // 
            this.LabelDVTAETitle.Location = new System.Drawing.Point(10, 9);
            this.LabelDVTAETitle.Name = "LabelDVTAETitle";
            this.LabelDVTAETitle.Size = new System.Drawing.Size(120, 27);
            this.LabelDVTAETitle.TabIndex = 0;
            this.LabelDVTAETitle.Text = "AE Title:";
            // 
            // LabelDVTListenPort
            // 
            this.LabelDVTListenPort.Location = new System.Drawing.Point(10, 93);
            this.LabelDVTListenPort.Name = "LabelDVTListenPort";
            this.LabelDVTListenPort.Size = new System.Drawing.Size(120, 24);
            this.LabelDVTListenPort.TabIndex = 0;
            this.LabelDVTListenPort.Text = "Listen port:";
            // 
            // LabelDVTSocketTimeOut
            // 
            this.LabelDVTSocketTimeOut.Location = new System.Drawing.Point(10, 120);
            this.LabelDVTSocketTimeOut.Name = "LabelDVTSocketTimeOut";
            this.LabelDVTSocketTimeOut.Size = new System.Drawing.Size(120, 23);
            this.LabelDVTSocketTimeOut.TabIndex = 0;
            this.LabelDVTSocketTimeOut.Text = "Socket time-out:";
            // 
            // LabelDVTMaxPDU
            // 
            this.LabelDVTMaxPDU.Location = new System.Drawing.Point(10, 142);
            this.LabelDVTMaxPDU.Name = "LabelDVTMaxPDU";
            this.LabelDVTMaxPDU.Size = new System.Drawing.Size(120, 37);
            this.LabelDVTMaxPDU.TabIndex = 0;
            this.LabelDVTMaxPDU.Text = "Maximum PDU length to receive:";
            // 
            // NumericDVTTimeOut
            // 
            this.NumericDVTTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericDVTTimeOut.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericDVTTimeOut.Location = new System.Drawing.Point(144, 120);
            this.NumericDVTTimeOut.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.NumericDVTTimeOut.Name = "NumericDVTTimeOut";
            this.NumericDVTTimeOut.Size = new System.Drawing.Size(373, 22);
            this.NumericDVTTimeOut.TabIndex = 4;
            this.NumericDVTTimeOut.ValueChanged += new System.EventHandler(this.NumericDVTTimeOut_ValueChanged);
            // 
            // NumericDVTMaxPDU
            // 
            this.NumericDVTMaxPDU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericDVTMaxPDU.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.NumericDVTMaxPDU.Location = new System.Drawing.Point(144, 148);
            this.NumericDVTMaxPDU.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.NumericDVTMaxPDU.Name = "NumericDVTMaxPDU";
            this.NumericDVTMaxPDU.Size = new System.Drawing.Size(373, 22);
            this.NumericDVTMaxPDU.TabIndex = 5;
            this.NumericDVTMaxPDU.ValueChanged += new System.EventHandler(this.NumericDVTMaxPDU_ValueChanged);
            // 
            // TextBoxDVTImplClassUID
            // 
            this.TextBoxDVTImplClassUID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDVTImplClassUID.Location = new System.Drawing.Point(144, 37);
            this.TextBoxDVTImplClassUID.MaxLength = 64;
            this.TextBoxDVTImplClassUID.Name = "TextBoxDVTImplClassUID";
            this.TextBoxDVTImplClassUID.Size = new System.Drawing.Size(373, 22);
            this.TextBoxDVTImplClassUID.TabIndex = 1;
            this.TextBoxDVTImplClassUID.TextChanged += new System.EventHandler(this.TextBoxDVTImplClassUID_TextChanged);
            // 
            // TextBoxDVTImplVersionName
            // 
            this.TextBoxDVTImplVersionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDVTImplVersionName.Location = new System.Drawing.Point(144, 65);
            this.TextBoxDVTImplVersionName.MaxLength = 16;
            this.TextBoxDVTImplVersionName.Name = "TextBoxDVTImplVersionName";
            this.TextBoxDVTImplVersionName.Size = new System.Drawing.Size(373, 22);
            this.TextBoxDVTImplVersionName.TabIndex = 2;
            this.TextBoxDVTImplVersionName.TextChanged += new System.EventHandler(this.TextBoxDVTImplVersionName_TextChanged);
            // 
            // LabelDVTImplClassUID
            // 
            this.LabelDVTImplClassUID.Location = new System.Drawing.Point(10, 37);
            this.LabelDVTImplClassUID.Name = "LabelDVTImplClassUID";
            this.LabelDVTImplClassUID.Size = new System.Drawing.Size(120, 26);
            this.LabelDVTImplClassUID.TabIndex = 0;
            this.LabelDVTImplClassUID.Text = "Impl. Class UID:";
            // 
            // LabelDVTImplVersionName
            // 
            this.LabelDVTImplVersionName.Location = new System.Drawing.Point(10, 65);
            this.LabelDVTImplVersionName.Name = "LabelDVTImplVersionName";
            this.LabelDVTImplVersionName.Size = new System.Drawing.Size(134, 26);
            this.LabelDVTImplVersionName.TabIndex = 0;
            this.LabelDVTImplVersionName.Text = "Impl. Version Name:";
            // 
            // PanelDVTRoleSettingsTitle
            // 
            this.PanelDVTRoleSettingsTitle.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PanelDVTRoleSettingsTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelDVTRoleSettingsTitle.Controls.Add(this.label1);
            this.PanelDVTRoleSettingsTitle.Controls.Add(this.MinDVTRoleSettings);
            this.PanelDVTRoleSettingsTitle.Controls.Add(this.MaxDVTRoleSettings);
            this.PanelDVTRoleSettingsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelDVTRoleSettingsTitle.Location = new System.Drawing.Point(15, 404);
            this.PanelDVTRoleSettingsTitle.Name = "PanelDVTRoleSettingsTitle";
            this.PanelDVTRoleSettingsTitle.Size = new System.Drawing.Size(693, 37);
            this.PanelDVTRoleSettingsTitle.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "DVT Role Settings";
            // 
            // MinDVTRoleSettings
            // 
            this.MinDVTRoleSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinDVTRoleSettings.Image = ((System.Drawing.Image)(resources.GetObject("MinDVTRoleSettings.Image")));
            this.MinDVTRoleSettings.Location = new System.Drawing.Point(637, 0);
            this.MinDVTRoleSettings.Name = "MinDVTRoleSettings";
            this.MinDVTRoleSettings.Size = new System.Drawing.Size(39, 28);
            this.MinDVTRoleSettings.TabIndex = 0;
            this.MinDVTRoleSettings.TabStop = false;
            this.MinDVTRoleSettings.Click += new System.EventHandler(this.MinDVTRoleSettings_Click);
            // 
            // MaxDVTRoleSettings
            // 
            this.MaxDVTRoleSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxDVTRoleSettings.Image = ((System.Drawing.Image)(resources.GetObject("MaxDVTRoleSettings.Image")));
            this.MaxDVTRoleSettings.Location = new System.Drawing.Point(637, 0);
            this.MaxDVTRoleSettings.Name = "MaxDVTRoleSettings";
            this.MaxDVTRoleSettings.Size = new System.Drawing.Size(39, 28);
            this.MaxDVTRoleSettings.TabIndex = 0;
            this.MaxDVTRoleSettings.TabStop = false;
            this.MaxDVTRoleSettings.Visible = false;
            this.MaxDVTRoleSettings.Click += new System.EventHandler(this.MaxDVTRoleSettings_Click);
            // 
            // PanelGeneralPropertiesContent
            // 
            this.PanelGeneralPropertiesContent.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PanelGeneralPropertiesContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelGeneralPropertiesContent.Controls.Add(this.panel1);
            this.PanelGeneralPropertiesContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelGeneralPropertiesContent.Location = new System.Drawing.Point(15, 52);
            this.PanelGeneralPropertiesContent.Name = "PanelGeneralPropertiesContent";
            this.PanelGeneralPropertiesContent.Size = new System.Drawing.Size(693, 352);
            this.PanelGeneralPropertiesContent.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ButtonBrowseDataDirectory);
            this.panel1.Controls.Add(this.TextBoxDataRoot);
            this.panel1.Controls.Add(this.ButtonBrowseDescriptionDir);
            this.panel1.Controls.Add(this.CheckBoxDisplayCondition);
            this.panel1.Controls.Add(this.CheckBoxAddGroupLengths);
            this.panel1.Controls.Add(this.CheckBoxDefineSQLength);
            this.panel1.Controls.Add(this.ComboBoxStorageMode);
            this.panel1.Controls.Add(this.CheckBoxLogRelation);
            this.panel1.Controls.Add(this.labelStorageMode);
            this.panel1.Controls.Add(this.CheckBoxGenerateDetailedValidationResults);
            this.panel1.Controls.Add(this.TextBoxSessionType);
            this.panel1.Controls.Add(this.NumericSessonID);
            this.panel1.Controls.Add(this.LabelResultsDir);
            this.panel1.Controls.Add(this.LabelSessionType);
            this.panel1.Controls.Add(this.LabelDate);
            this.panel1.Controls.Add(this.LabelSessionTitle);
            this.panel1.Controls.Add(this.LabelTestedBy);
            this.panel1.Controls.Add(this.TextBoxTestedBy);
            this.panel1.Controls.Add(this.TextBoxResultsRoot);
            this.panel1.Controls.Add(this.TextBoxScriptRoot);
            this.panel1.Controls.Add(this.LabelScriptsDir);
            this.panel1.Controls.Add(this.TextBoxSessionTitle);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.LabelSpecifyTransferSyntaxes);
            this.panel1.Controls.Add(this.LabelDescriptionDir);
            this.panel1.Controls.Add(this.TextBoxDescriptionRoot);
            this.panel1.Controls.Add(this.ButtonBrowseResultsDir);
            this.panel1.Controls.Add(this.ButtonBrowseScriptsDir);
            this.panel1.Controls.Add(this.ButtonSpecifyTransferSyntaxes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 350);
            this.panel1.TabIndex = 0;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Data dir:";
            // 
            // ButtonBrowseDataDirectory
            // 
            this.ButtonBrowseDataDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonBrowseDataDirectory.Location = new System.Drawing.Point(540, 144);
            this.ButtonBrowseDataDirectory.Name = "ButtonBrowseDataDirectory";
            this.ButtonBrowseDataDirectory.Size = new System.Drawing.Size(133, 26);
            this.ButtonBrowseDataDirectory.TabIndex = 19;
            this.ButtonBrowseDataDirectory.Text = "Browse";
            this.ButtonBrowseDataDirectory.UseVisualStyleBackColor = true;
            this.ButtonBrowseDataDirectory.Click += new System.EventHandler(this.ButtonBrowseDataDirectory_Click);
            // 
            // TextBoxDataRoot
            // 
            this.TextBoxDataRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDataRoot.BackColor = System.Drawing.SystemColors.Control;
            this.TextBoxDataRoot.Location = new System.Drawing.Point(144, 147);
            this.TextBoxDataRoot.Name = "TextBoxDataRoot";
            this.TextBoxDataRoot.ReadOnly = true;
            this.TextBoxDataRoot.Size = new System.Drawing.Size(374, 22);
            this.TextBoxDataRoot.TabIndex = 18;
            // 
            // ButtonBrowseDescriptionDir
            // 
            this.ButtonBrowseDescriptionDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonBrowseDescriptionDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonBrowseDescriptionDir.Location = new System.Drawing.Point(539, 172);
            this.ButtonBrowseDescriptionDir.Name = "ButtonBrowseDescriptionDir";
            this.ButtonBrowseDescriptionDir.Size = new System.Drawing.Size(134, 23);
            this.ButtonBrowseDescriptionDir.TabIndex = 6;
            this.ButtonBrowseDescriptionDir.Text = "Browse";
            this.ButtonBrowseDescriptionDir.Click += new System.EventHandler(this.ButtonBrowseDescriptionDir_Click);
            // 
            // CheckBoxDisplayCondition
            // 
            this.CheckBoxDisplayCondition.Checked = true;
            this.CheckBoxDisplayCondition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxDisplayCondition.Location = new System.Drawing.Point(500, 316);
            this.CheckBoxDisplayCondition.Name = "CheckBoxDisplayCondition";
            this.CheckBoxDisplayCondition.Size = new System.Drawing.Size(288, 28);
            this.CheckBoxDisplayCondition.TabIndex = 17;
            this.CheckBoxDisplayCondition.Text = "Display Condition text in Summary Results";
            this.CheckBoxDisplayCondition.CheckedChanged += new System.EventHandler(this.CheckBoxDisplayCondition_CheckedChanged);
            // 
            // CheckBoxAddGroupLengths
            // 
            this.CheckBoxAddGroupLengths.Location = new System.Drawing.Point(298, 318);
            this.CheckBoxAddGroupLengths.Name = "CheckBoxAddGroupLengths";
            this.CheckBoxAddGroupLengths.Size = new System.Drawing.Size(163, 28);
            this.CheckBoxAddGroupLengths.TabIndex = 12;
            this.CheckBoxAddGroupLengths.Text = "Add Group Lengths";
            this.CheckBoxAddGroupLengths.CheckedChanged += new System.EventHandler(this.CheckBoxAddGroupLengths_CheckedChanged);
            // 
            // CheckBoxDefineSQLength
            // 
            this.CheckBoxDefineSQLength.Location = new System.Drawing.Point(298, 290);
            this.CheckBoxDefineSQLength.Name = "CheckBoxDefineSQLength";
            this.CheckBoxDefineSQLength.Size = new System.Drawing.Size(144, 27);
            this.CheckBoxDefineSQLength.TabIndex = 11;
            this.CheckBoxDefineSQLength.Text = "Define SQ Length";
            this.CheckBoxDefineSQLength.CheckedChanged += new System.EventHandler(this.CheckBoxDefineSQLength_CheckedChanged);
            // 
            // ComboBoxStorageMode
            // 
            this.ComboBoxStorageMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboBoxStorageMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxStorageMode.Items.AddRange(new object[] {
            "As Data Set",
            "As Media",
            "As Media (without PIX files)",
            "No Storage"});
            this.ComboBoxStorageMode.Location = new System.Drawing.Point(142, 261);
            this.ComboBoxStorageMode.Name = "ComboBoxStorageMode";
            this.ComboBoxStorageMode.Size = new System.Drawing.Size(379, 24);
            this.ComboBoxStorageMode.TabIndex = 8;
            this.ComboBoxStorageMode.SelectedIndexChanged += new System.EventHandler(this.ComboBoxStorageMode_SelectedIndexChanged);
            // 
            // CheckBoxLogRelation
            // 
            this.CheckBoxLogRelation.Location = new System.Drawing.Point(10, 288);
            this.CheckBoxLogRelation.Name = "CheckBoxLogRelation";
            this.CheckBoxLogRelation.Size = new System.Drawing.Size(124, 28);
            this.CheckBoxLogRelation.TabIndex = 9;
            this.CheckBoxLogRelation.Text = "Log Relation";
            this.CheckBoxLogRelation.CheckedChanged += new System.EventHandler(this.CheckBoxLogRelation_CheckedChanged);
            // 
            // labelStorageMode
            // 
            this.labelStorageMode.Location = new System.Drawing.Point(10, 258);
            this.labelStorageMode.Name = "labelStorageMode";
            this.labelStorageMode.Size = new System.Drawing.Size(120, 27);
            this.labelStorageMode.TabIndex = 0;
            this.labelStorageMode.Text = "Storage Mode:";
            // 
            // CheckBoxGenerateDetailedValidationResults
            // 
            this.CheckBoxGenerateDetailedValidationResults.Location = new System.Drawing.Point(10, 321);
            this.CheckBoxGenerateDetailedValidationResults.Name = "CheckBoxGenerateDetailedValidationResults";
            this.CheckBoxGenerateDetailedValidationResults.Size = new System.Drawing.Size(249, 26);
            this.CheckBoxGenerateDetailedValidationResults.TabIndex = 10;
            this.CheckBoxGenerateDetailedValidationResults.Text = "Generate detailed validation results";
            this.CheckBoxGenerateDetailedValidationResults.CheckedChanged += new System.EventHandler(this.CheckBoxGenerateDetailedValidationResults_CheckedChanged);
            // 
            // TextBoxSessionType
            // 
            this.TextBoxSessionType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSessionType.Location = new System.Drawing.Point(144, 9);
            this.TextBoxSessionType.MaxLength = 0;
            this.TextBoxSessionType.Name = "TextBoxSessionType";
            this.TextBoxSessionType.ReadOnly = true;
            this.TextBoxSessionType.Size = new System.Drawing.Size(374, 22);
            this.TextBoxSessionType.TabIndex = 0;
            // 
            // NumericSessonID
            // 
            this.NumericSessonID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NumericSessonID.Location = new System.Drawing.Point(144, 65);
            this.NumericSessonID.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.NumericSessonID.Name = "NumericSessonID";
            this.NumericSessonID.Size = new System.Drawing.Size(374, 22);
            this.NumericSessonID.TabIndex = 2;
            this.NumericSessonID.ValueChanged += new System.EventHandler(this.NumericSessonID_ValueChanged);
            // 
            // LabelResultsDir
            // 
            this.LabelResultsDir.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelResultsDir.Location = new System.Drawing.Point(10, 120);
            this.LabelResultsDir.Name = "LabelResultsDir";
            this.LabelResultsDir.Size = new System.Drawing.Size(120, 27);
            this.LabelResultsDir.TabIndex = 16;
            this.LabelResultsDir.Text = "Results dir:";
            // 
            // LabelSessionType
            // 
            this.LabelSessionType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelSessionType.Location = new System.Drawing.Point(10, 9);
            this.LabelSessionType.Name = "LabelSessionType";
            this.LabelSessionType.Size = new System.Drawing.Size(120, 27);
            this.LabelSessionType.TabIndex = 0;
            this.LabelSessionType.Text = "Session type:";
            // 
            // LabelDate
            // 
            this.LabelDate.Location = new System.Drawing.Point(0, 0);
            this.LabelDate.Name = "LabelDate";
            this.LabelDate.Size = new System.Drawing.Size(120, 27);
            this.LabelDate.TabIndex = 0;
            // 
            // LabelSessionTitle
            // 
            this.LabelSessionTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelSessionTitle.Location = new System.Drawing.Point(10, 37);
            this.LabelSessionTitle.Name = "LabelSessionTitle";
            this.LabelSessionTitle.Size = new System.Drawing.Size(120, 26);
            this.LabelSessionTitle.TabIndex = 0;
            this.LabelSessionTitle.Text = "Session title:";
            // 
            // LabelTestedBy
            // 
            this.LabelTestedBy.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelTestedBy.Location = new System.Drawing.Point(10, 92);
            this.LabelTestedBy.Name = "LabelTestedBy";
            this.LabelTestedBy.Size = new System.Drawing.Size(120, 27);
            this.LabelTestedBy.TabIndex = 0;
            this.LabelTestedBy.Text = "Tested by:";
            // 
            // TextBoxTestedBy
            // 
            this.TextBoxTestedBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxTestedBy.Location = new System.Drawing.Point(144, 92);
            this.TextBoxTestedBy.MaxLength = 0;
            this.TextBoxTestedBy.Name = "TextBoxTestedBy";
            this.TextBoxTestedBy.Size = new System.Drawing.Size(374, 22);
            this.TextBoxTestedBy.TabIndex = 3;
            this.TextBoxTestedBy.TextChanged += new System.EventHandler(this.TextBoxTestedBy_TextChanged);
            // 
            // TextBoxResultsRoot
            // 
            this.TextBoxResultsRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxResultsRoot.Location = new System.Drawing.Point(144, 120);
            this.TextBoxResultsRoot.MaxLength = 0;
            this.TextBoxResultsRoot.Name = "TextBoxResultsRoot";
            this.TextBoxResultsRoot.ReadOnly = true;
            this.TextBoxResultsRoot.Size = new System.Drawing.Size(374, 22);
            this.TextBoxResultsRoot.TabIndex = 0;
            this.TextBoxResultsRoot.TabStop = false;
            // 
            // TextBoxScriptRoot
            // 
            this.TextBoxScriptRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxScriptRoot.Location = new System.Drawing.Point(144, 198);
            this.TextBoxScriptRoot.MaxLength = 0;
            this.TextBoxScriptRoot.Name = "TextBoxScriptRoot";
            this.TextBoxScriptRoot.ReadOnly = true;
            this.TextBoxScriptRoot.Size = new System.Drawing.Size(374, 22);
            this.TextBoxScriptRoot.TabIndex = 0;
            this.TextBoxScriptRoot.TabStop = false;
            // 
            // LabelScriptsDir
            // 
            this.LabelScriptsDir.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelScriptsDir.Location = new System.Drawing.Point(10, 202);
            this.LabelScriptsDir.Name = "LabelScriptsDir";
            this.LabelScriptsDir.Size = new System.Drawing.Size(120, 26);
            this.LabelScriptsDir.TabIndex = 0;
            this.LabelScriptsDir.Text = "Scripts dir:";
            // 
            // TextBoxSessionTitle
            // 
            this.TextBoxSessionTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSessionTitle.Location = new System.Drawing.Point(144, 37);
            this.TextBoxSessionTitle.MaxLength = 0;
            this.TextBoxSessionTitle.Name = "TextBoxSessionTitle";
            this.TextBoxSessionTitle.Size = new System.Drawing.Size(374, 22);
            this.TextBoxSessionTitle.TabIndex = 1;
            this.TextBoxSessionTitle.TextChanged += new System.EventHandler(this.TextBoxSessionTitle_TextChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label7.Location = new System.Drawing.Point(10, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 26);
            this.label7.TabIndex = 0;
            this.label7.Text = "Session ID:";
            // 
            // LabelSpecifyTransferSyntaxes
            // 
            this.LabelSpecifyTransferSyntaxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelSpecifyTransferSyntaxes.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelSpecifyTransferSyntaxes.Location = new System.Drawing.Point(10, 232);
            this.LabelSpecifyTransferSyntaxes.Name = "LabelSpecifyTransferSyntaxes";
            this.LabelSpecifyTransferSyntaxes.Size = new System.Drawing.Size(502, 25);
            this.LabelSpecifyTransferSyntaxes.TabIndex = 0;
            this.LabelSpecifyTransferSyntaxes.Text = "(Un)select the supported Transfer Syntaxes:";
            // 
            // LabelDescriptionDir
            // 
            this.LabelDescriptionDir.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LabelDescriptionDir.Location = new System.Drawing.Point(10, 175);
            this.LabelDescriptionDir.Name = "LabelDescriptionDir";
            this.LabelDescriptionDir.Size = new System.Drawing.Size(120, 27);
            this.LabelDescriptionDir.TabIndex = 0;
            this.LabelDescriptionDir.Text = "Description dir:";
            // 
            // TextBoxDescriptionRoot
            // 
            this.TextBoxDescriptionRoot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxDescriptionRoot.Location = new System.Drawing.Point(144, 172);
            this.TextBoxDescriptionRoot.MaxLength = 0;
            this.TextBoxDescriptionRoot.Name = "TextBoxDescriptionRoot";
            this.TextBoxDescriptionRoot.ReadOnly = true;
            this.TextBoxDescriptionRoot.Size = new System.Drawing.Size(374, 22);
            this.TextBoxDescriptionRoot.TabIndex = 0;
            this.TextBoxDescriptionRoot.TabStop = false;
            // 
            // ButtonBrowseResultsDir
            // 
            this.ButtonBrowseResultsDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonBrowseResultsDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonBrowseResultsDir.Location = new System.Drawing.Point(540, 120);
            this.ButtonBrowseResultsDir.Name = "ButtonBrowseResultsDir";
            this.ButtonBrowseResultsDir.Size = new System.Drawing.Size(134, 23);
            this.ButtonBrowseResultsDir.TabIndex = 4;
            this.ButtonBrowseResultsDir.Text = "Browse";
            this.ButtonBrowseResultsDir.Click += new System.EventHandler(this.ButtonBrowseResultsDir_Click);
            // 
            // ButtonBrowseScriptsDir
            // 
            this.ButtonBrowseScriptsDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonBrowseScriptsDir.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonBrowseScriptsDir.Location = new System.Drawing.Point(540, 196);
            this.ButtonBrowseScriptsDir.Name = "ButtonBrowseScriptsDir";
            this.ButtonBrowseScriptsDir.Size = new System.Drawing.Size(134, 26);
            this.ButtonBrowseScriptsDir.TabIndex = 5;
            this.ButtonBrowseScriptsDir.Text = "Browse ";
            this.ButtonBrowseScriptsDir.Click += new System.EventHandler(this.ButtonBrowseScriptsDir_Click);
            // 
            // ButtonSpecifyTransferSyntaxes
            // 
            this.ButtonSpecifyTransferSyntaxes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSpecifyTransferSyntaxes.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButtonSpecifyTransferSyntaxes.Location = new System.Drawing.Point(540, 232);
            this.ButtonSpecifyTransferSyntaxes.Name = "ButtonSpecifyTransferSyntaxes";
            this.ButtonSpecifyTransferSyntaxes.Size = new System.Drawing.Size(134, 25);
            this.ButtonSpecifyTransferSyntaxes.TabIndex = 7;
            this.ButtonSpecifyTransferSyntaxes.Text = "Specify TS";
            this.ButtonSpecifyTransferSyntaxes.Click += new System.EventHandler(this.ButtonSpecifyTransferSyntaxes_Click);
            // 
            // PanelGeneralPropertiesTitle
            // 
            this.PanelGeneralPropertiesTitle.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PanelGeneralPropertiesTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelGeneralPropertiesTitle.Controls.Add(this.label5);
            this.PanelGeneralPropertiesTitle.Controls.Add(this.MinGSPSettings);
            this.PanelGeneralPropertiesTitle.Controls.Add(this.MaxGSPSettings);
            this.PanelGeneralPropertiesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelGeneralPropertiesTitle.Location = new System.Drawing.Point(15, 15);
            this.PanelGeneralPropertiesTitle.Name = "PanelGeneralPropertiesTitle";
            this.PanelGeneralPropertiesTitle.Size = new System.Drawing.Size(693, 37);
            this.PanelGeneralPropertiesTitle.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(220, 27);
            this.label5.TabIndex = 0;
            this.label5.Text = "General Session Properties";
            // 
            // MinGSPSettings
            // 
            this.MinGSPSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinGSPSettings.Image = ((System.Drawing.Image)(resources.GetObject("MinGSPSettings.Image")));
            this.MinGSPSettings.Location = new System.Drawing.Point(637, 0);
            this.MinGSPSettings.Name = "MinGSPSettings";
            this.MinGSPSettings.Size = new System.Drawing.Size(39, 28);
            this.MinGSPSettings.TabIndex = 0;
            this.MinGSPSettings.TabStop = false;
            this.MinGSPSettings.Click += new System.EventHandler(this.MinGSPSettings_Click);
            // 
            // MaxGSPSettings
            // 
            this.MaxGSPSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxGSPSettings.Image = ((System.Drawing.Image)(resources.GetObject("MaxGSPSettings.Image")));
            this.MaxGSPSettings.Location = new System.Drawing.Point(637, 0);
            this.MaxGSPSettings.Name = "MaxGSPSettings";
            this.MaxGSPSettings.Size = new System.Drawing.Size(39, 28);
            this.MaxGSPSettings.TabIndex = 0;
            this.MaxGSPSettings.TabStop = false;
            this.MaxGSPSettings.Visible = false;
            this.MaxGSPSettings.Click += new System.EventHandler(this.MaxGSPSettings_Click);
            // 
            // TabSpecifySopClasses
            // 
            this.TabSpecifySopClasses.Controls.Add(this.DataGridSpecifySopClasses);
            this.TabSpecifySopClasses.Controls.Add(this.RichTextBoxSpecifySopClassesInfo);
            this.TabSpecifySopClasses.Controls.Add(this.panel3);
            this.TabSpecifySopClasses.Controls.Add(this.panel5);
            this.TabSpecifySopClasses.Location = new System.Drawing.Point(4, 25);
            this.TabSpecifySopClasses.Name = "TabSpecifySopClasses";
            this.TabSpecifySopClasses.Size = new System.Drawing.Size(744, 696);
            this.TabSpecifySopClasses.TabIndex = 0;
            this.TabSpecifySopClasses.Text = "Specify SOP Classes";
            this.TabSpecifySopClasses.UseVisualStyleBackColor = true;
            // 
            // DataGridSpecifySopClasses
            // 
            this.DataGridSpecifySopClasses.ContextMenu = this.ContextMenuDataGrid;
            this.DataGridSpecifySopClasses.DataMember = "";
            this.DataGridSpecifySopClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridSpecifySopClasses.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.DataGridSpecifySopClasses.Location = new System.Drawing.Point(0, 175);
            this.DataGridSpecifySopClasses.Name = "DataGridSpecifySopClasses";
            this.DataGridSpecifySopClasses.Size = new System.Drawing.Size(744, 461);
            this.DataGridSpecifySopClasses.TabIndex = 9;
            this.DataGridSpecifySopClasses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridSpecifySopClasses_MouseDown);
            this.DataGridSpecifySopClasses.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridSpecifySopClasses_MouseMove);
            this.DataGridSpecifySopClasses.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGridSpecifySopClasses_MouseUp);
            // 
            // ContextMenuDataGrid
            // 
            this.ContextMenuDataGrid.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ContextMenu_SelectAllDefinitionFiles,
            this.ContextMenu_UnSelectAllDefinitionFiles,
            this.ContextMenu_OpenDefFile});
            // 
            // ContextMenu_SelectAllDefinitionFiles
            // 
            this.ContextMenu_SelectAllDefinitionFiles.Index = 0;
            this.ContextMenu_SelectAllDefinitionFiles.Text = "SelectAllDefinitionFiles....";
            this.ContextMenu_SelectAllDefinitionFiles.Click += new System.EventHandler(this.ContextMenu_SelectAllDefinitionFiles_Click);
            // 
            // ContextMenu_UnSelectAllDefinitionFiles
            // 
            this.ContextMenu_UnSelectAllDefinitionFiles.Index = 1;
            this.ContextMenu_UnSelectAllDefinitionFiles.Text = "UnSelectAllDefinitionFiles....";
            this.ContextMenu_UnSelectAllDefinitionFiles.Click += new System.EventHandler(this.ContextMenu_UnSelectAllDefinitionFiles_Click);
            // 
            // ContextMenu_OpenDefFile
            // 
            this.ContextMenu_OpenDefFile.Index = 2;
            this.ContextMenu_OpenDefFile.Text = "Open Definition File with NotePad";
            this.ContextMenu_OpenDefFile.Click += new System.EventHandler(this.ContextMenu_OpenDefFile_Click);
            // 
            // RichTextBoxSpecifySopClassesInfo
            // 
            this.RichTextBoxSpecifySopClassesInfo.ContextMenu = this.ContextMenuRichTextBox;
            this.RichTextBoxSpecifySopClassesInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RichTextBoxSpecifySopClassesInfo.Location = new System.Drawing.Point(0, 636);
            this.RichTextBoxSpecifySopClassesInfo.Name = "RichTextBoxSpecifySopClassesInfo";
            this.RichTextBoxSpecifySopClassesInfo.Size = new System.Drawing.Size(744, 60);
            this.RichTextBoxSpecifySopClassesInfo.TabIndex = 0;
            this.RichTextBoxSpecifySopClassesInfo.Text = "";
            // 
            // ContextMenuRichTextBox
            // 
            this.ContextMenuRichTextBox.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ContextMenu_Copy});
            this.ContextMenuRichTextBox.Popup += new System.EventHandler(this.ContextMenuRichTextBox_Popup);
            // 
            // ContextMenu_Copy
            // 
            this.ContextMenu_Copy.Index = 0;
            this.ContextMenu_Copy.Text = "Copy";
            this.ContextMenu_Copy.Click += new System.EventHandler(this.ContextMenu_Copy_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.ButtonUnselectAll);
            this.panel3.Controls.Add(this.ButtonSelectAllDefinitionFiles);
            this.panel3.Controls.Add(this.ButtonSpecifySopClassesAddDirectory);
            this.panel3.Controls.Add(this.ButtonSpecifySopClassesRemoveDirectory);
            this.panel3.Location = new System.Drawing.Point(617, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(125, 175);
            this.panel3.TabIndex = 8;
            // 
            // ButtonUnselectAll
            // 
            this.ButtonUnselectAll.Location = new System.Drawing.Point(19, 138);
            this.ButtonUnselectAll.Name = "ButtonUnselectAll";
            this.ButtonUnselectAll.Size = new System.Drawing.Size(90, 27);
            this.ButtonUnselectAll.TabIndex = 4;
            this.ButtonUnselectAll.Text = "UnselectAll";
            this.ButtonUnselectAll.Click += new System.EventHandler(this.ButtonUnselectAll_Click);
            // 
            // ButtonSelectAllDefinitionFiles
            // 
            this.ButtonSelectAllDefinitionFiles.Location = new System.Drawing.Point(19, 102);
            this.ButtonSelectAllDefinitionFiles.Name = "ButtonSelectAllDefinitionFiles";
            this.ButtonSelectAllDefinitionFiles.Size = new System.Drawing.Size(90, 26);
            this.ButtonSelectAllDefinitionFiles.TabIndex = 3;
            this.ButtonSelectAllDefinitionFiles.Text = "SelectAll";
            this.ButtonSelectAllDefinitionFiles.Click += new System.EventHandler(this.ButtonSelectAllDefinitionFiles_Click);
            // 
            // ButtonSpecifySopClassesAddDirectory
            // 
            this.ButtonSpecifySopClassesAddDirectory.Location = new System.Drawing.Point(19, 28);
            this.ButtonSpecifySopClassesAddDirectory.Name = "ButtonSpecifySopClassesAddDirectory";
            this.ButtonSpecifySopClassesAddDirectory.Size = new System.Drawing.Size(90, 26);
            this.ButtonSpecifySopClassesAddDirectory.TabIndex = 1;
            this.ButtonSpecifySopClassesAddDirectory.Text = "Add";
            this.ButtonSpecifySopClassesAddDirectory.Click += new System.EventHandler(this.ButtonSpecifySopClassesAddDirectory_Click);
            // 
            // ButtonSpecifySopClassesRemoveDirectory
            // 
            this.ButtonSpecifySopClassesRemoveDirectory.Location = new System.Drawing.Point(19, 65);
            this.ButtonSpecifySopClassesRemoveDirectory.Name = "ButtonSpecifySopClassesRemoveDirectory";
            this.ButtonSpecifySopClassesRemoveDirectory.Size = new System.Drawing.Size(90, 26);
            this.ButtonSpecifySopClassesRemoveDirectory.TabIndex = 2;
            this.ButtonSpecifySopClassesRemoveDirectory.Text = "Remove";
            this.ButtonSpecifySopClassesRemoveDirectory.Click += new System.EventHandler(this.ButtonSpecifySopClassesRemoveDirectory_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.ListBoxSpecifySopClassesDefinitionFileDirectories);
            this.panel5.Controls.Add(this.LabelSpecifySopClassesDefinitionFileDirectories);
            this.panel5.Controls.Add(this.LabelSpecifySopClassesSelectAeTitle);
            this.panel5.Controls.Add(this.ComboBoxSpecifySopClassesAeTitle);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(744, 175);
            this.panel5.TabIndex = 7;
            // 
            // ListBoxSpecifySopClassesDefinitionFileDirectories
            // 
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.ItemHeight = 16;
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Location = new System.Drawing.Point(221, 18);
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Name = "ListBoxSpecifySopClassesDefinitionFileDirectories";
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.Size = new System.Drawing.Size(379, 68);
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.TabIndex = 0;
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.TabStop = false;
            this.ListBoxSpecifySopClassesDefinitionFileDirectories.SelectedIndexChanged += new System.EventHandler(this.ListBoxSpecifySopClassesDefinitionFileDirectories_SelectedIndexChanged);
            // 
            // LabelSpecifySopClassesDefinitionFileDirectories
            // 
            this.LabelSpecifySopClassesDefinitionFileDirectories.Location = new System.Drawing.Point(19, 28);
            this.LabelSpecifySopClassesDefinitionFileDirectories.Name = "LabelSpecifySopClassesDefinitionFileDirectories";
            this.LabelSpecifySopClassesDefinitionFileDirectories.Size = new System.Drawing.Size(154, 26);
            this.LabelSpecifySopClassesDefinitionFileDirectories.TabIndex = 0;
            this.LabelSpecifySopClassesDefinitionFileDirectories.Text = "Definition file directories:";
            // 
            // LabelSpecifySopClassesSelectAeTitle
            // 
            this.LabelSpecifySopClassesSelectAeTitle.Location = new System.Drawing.Point(19, 138);
            this.LabelSpecifySopClassesSelectAeTitle.Name = "LabelSpecifySopClassesSelectAeTitle";
            this.LabelSpecifySopClassesSelectAeTitle.Size = new System.Drawing.Size(192, 27);
            this.LabelSpecifySopClassesSelectAeTitle.TabIndex = 0;
            this.LabelSpecifySopClassesSelectAeTitle.Text = "Select AE title - version to use";
            // 
            // ComboBoxSpecifySopClassesAeTitle
            // 
            this.ComboBoxSpecifySopClassesAeTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComboBoxSpecifySopClassesAeTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxSpecifySopClassesAeTitle.Location = new System.Drawing.Point(221, 138);
            this.ComboBoxSpecifySopClassesAeTitle.Name = "ComboBoxSpecifySopClassesAeTitle";
            this.ComboBoxSpecifySopClassesAeTitle.Size = new System.Drawing.Size(379, 24);
            this.ComboBoxSpecifySopClassesAeTitle.TabIndex = 0;
            this.ComboBoxSpecifySopClassesAeTitle.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSpecifySopClassesAeTitle_SelectedIndexChanged);
            // 
            // TabActivityLogging
            // 
            this.TabActivityLogging.Controls.Add(this.RichTextBoxActivityLogging);
            this.TabActivityLogging.Location = new System.Drawing.Point(4, 25);
            this.TabActivityLogging.Name = "TabActivityLogging";
            this.TabActivityLogging.Padding = new System.Windows.Forms.Padding(15);
            this.TabActivityLogging.Size = new System.Drawing.Size(744, 696);
            this.TabActivityLogging.TabIndex = 0;
            this.TabActivityLogging.Text = " Activity Logging ";
            this.TabActivityLogging.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxActivityLogging
            // 
            this.RichTextBoxActivityLogging.ContextMenu = this.ContextMenuRichTextBox;
            this.RichTextBoxActivityLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxActivityLogging.HideSelection = false;
            this.RichTextBoxActivityLogging.Location = new System.Drawing.Point(15, 15);
            this.RichTextBoxActivityLogging.Name = "RichTextBoxActivityLogging";
            this.RichTextBoxActivityLogging.ReadOnly = true;
            this.RichTextBoxActivityLogging.Size = new System.Drawing.Size(714, 666);
            this.RichTextBoxActivityLogging.TabIndex = 0;
            this.RichTextBoxActivityLogging.Text = "";
            // 
            // TabValidationResults
            // 
            this.TabValidationResults.AutoScroll = true;
            this.TabValidationResults.Controls.Add(this.WebBrowserPanel);
            this.TabValidationResults.Location = new System.Drawing.Point(4, 25);
            this.TabValidationResults.Name = "TabValidationResults";
            this.TabValidationResults.Padding = new System.Windows.Forms.Padding(15);
            this.TabValidationResults.Size = new System.Drawing.Size(744, 696);
            this.TabValidationResults.TabIndex = 0;
            this.TabValidationResults.Text = "Validation Results";
            this.TabValidationResults.UseVisualStyleBackColor = true;
            // 
            // WebBrowserPanel
            // 
            this.WebBrowserPanel.Controls.Add(this.webBrowserValResult);
            this.WebBrowserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebBrowserPanel.Location = new System.Drawing.Point(15, 15);
            this.WebBrowserPanel.Name = "WebBrowserPanel";
            this.WebBrowserPanel.Size = new System.Drawing.Size(714, 666);
            this.WebBrowserPanel.TabIndex = 0;
            // 
            // webBrowserValResult
            // 
            this.webBrowserValResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserValResult.Location = new System.Drawing.Point(0, 0);
            this.webBrowserValResult.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserValResult.Name = "webBrowserValResult";
            this.webBrowserValResult.Size = new System.Drawing.Size(714, 666);
            this.webBrowserValResult.TabIndex = 0;
            this.webBrowserValResult.CanGoBackChanged += new System.EventHandler(this.webBrowserValResult_CanGoBackChanged);
            this.webBrowserValResult.CanGoForwardChanged += new System.EventHandler(this.webBrowserValResult_CanGoForwardChanged);
            this.webBrowserValResult.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserValResult_DocumentCompleted);
            this.webBrowserValResult.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserValResult_Navigated);
            this.webBrowserValResult.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserValResult_Navigating);
            // 
            // TabResultsManager
            // 
            this.TabResultsManager.Controls.Add(this.PanelWebBrowserResultsManager);
            this.TabResultsManager.Controls.Add(this.TabControlIssues);
            this.TabResultsManager.Location = new System.Drawing.Point(4, 25);
            this.TabResultsManager.Name = "TabResultsManager";
            this.TabResultsManager.Size = new System.Drawing.Size(744, 696);
            this.TabResultsManager.TabIndex = 0;
            this.TabResultsManager.Text = "Results Manager";
            this.TabResultsManager.UseVisualStyleBackColor = true;
            // 
            // PanelWebBrowserResultsManager
            // 
            this.PanelWebBrowserResultsManager.Controls.Add(this.webBrowserResultMgr);
            this.PanelWebBrowserResultsManager.Controls.Add(this.PanelResultsManagerTiltle);
            this.PanelWebBrowserResultsManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelWebBrowserResultsManager.Location = new System.Drawing.Point(0, 0);
            this.PanelWebBrowserResultsManager.Name = "PanelWebBrowserResultsManager";
            this.PanelWebBrowserResultsManager.Size = new System.Drawing.Size(744, 502);
            this.PanelWebBrowserResultsManager.TabIndex = 0;
            // 
            // webBrowserResultMgr
            // 
            this.webBrowserResultMgr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserResultMgr.Location = new System.Drawing.Point(0, 0);
            this.webBrowserResultMgr.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserResultMgr.Name = "webBrowserResultMgr";
            this.webBrowserResultMgr.Size = new System.Drawing.Size(744, 465);
            this.webBrowserResultMgr.TabIndex = 1;
            this.webBrowserResultMgr.CanGoBackChanged += new System.EventHandler(this.webBrowserResultMgr_CanGoBackChanged);
            this.webBrowserResultMgr.CanGoForwardChanged += new System.EventHandler(this.webBrowserResultMgr_CanGoForwardChanged);
            this.webBrowserResultMgr.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserResultMgr_Navigating);
            // 
            // PanelResultsManagerTiltle
            // 
            this.PanelResultsManagerTiltle.BackColor = System.Drawing.SystemColors.Control;
            this.PanelResultsManagerTiltle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelResultsManagerTiltle.Controls.Add(this.LabelIssueText);
            this.PanelResultsManagerTiltle.Controls.Add(this.PictureBoxMinResultsTab);
            this.PanelResultsManagerTiltle.Controls.Add(this.PictureBoxMaximizeResultTab);
            this.PanelResultsManagerTiltle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelResultsManagerTiltle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PanelResultsManagerTiltle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.PanelResultsManagerTiltle.Location = new System.Drawing.Point(0, 465);
            this.PanelResultsManagerTiltle.Name = "PanelResultsManagerTiltle";
            this.PanelResultsManagerTiltle.Size = new System.Drawing.Size(744, 37);
            this.PanelResultsManagerTiltle.TabIndex = 0;
            // 
            // LabelIssueText
            // 
            this.LabelIssueText.Location = new System.Drawing.Point(0, 9);
            this.LabelIssueText.Name = "LabelIssueText";
            this.LabelIssueText.Size = new System.Drawing.Size(154, 28);
            this.LabelIssueText.TabIndex = 0;
            this.LabelIssueText.Text = "Issue Panel";
            // 
            // PictureBoxMinResultsTab
            // 
            this.PictureBoxMinResultsTab.Dock = System.Windows.Forms.DockStyle.Right;
            this.PictureBoxMinResultsTab.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxMinResultsTab.Image")));
            this.PictureBoxMinResultsTab.Location = new System.Drawing.Point(644, 0);
            this.PictureBoxMinResultsTab.Name = "PictureBoxMinResultsTab";
            this.PictureBoxMinResultsTab.Size = new System.Drawing.Size(48, 33);
            this.PictureBoxMinResultsTab.TabIndex = 0;
            this.PictureBoxMinResultsTab.TabStop = false;
            this.PictureBoxMinResultsTab.Click += new System.EventHandler(this.PictureBoxResultsManager_Click);
            // 
            // PictureBoxMaximizeResultTab
            // 
            this.PictureBoxMaximizeResultTab.BackColor = System.Drawing.SystemColors.Control;
            this.PictureBoxMaximizeResultTab.Dock = System.Windows.Forms.DockStyle.Right;
            this.PictureBoxMaximizeResultTab.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxMaximizeResultTab.Image")));
            this.PictureBoxMaximizeResultTab.Location = new System.Drawing.Point(692, 0);
            this.PictureBoxMaximizeResultTab.Name = "PictureBoxMaximizeResultTab";
            this.PictureBoxMaximizeResultTab.Size = new System.Drawing.Size(48, 33);
            this.PictureBoxMaximizeResultTab.TabIndex = 0;
            this.PictureBoxMaximizeResultTab.TabStop = false;
            this.PictureBoxMaximizeResultTab.Visible = false;
            this.PictureBoxMaximizeResultTab.Click += new System.EventHandler(this.PictureBoxMaximizeResultTab_Click);
            // 
            // TabControlIssues
            // 
            this.TabControlIssues.Controls.Add(this.AddIssue);
            this.TabControlIssues.Controls.Add(this.RemoveIssue);
            this.TabControlIssues.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TabControlIssues.Location = new System.Drawing.Point(0, 502);
            this.TabControlIssues.Name = "TabControlIssues";
            this.TabControlIssues.SelectedIndex = 0;
            this.TabControlIssues.Size = new System.Drawing.Size(744, 194);
            this.TabControlIssues.TabIndex = 2;
            // 
            // AddIssue
            // 
            this.AddIssue.Controls.Add(this.LabelAddMessageText);
            this.AddIssue.Controls.Add(this.TextBoxAddMessageText);
            this.AddIssue.Controls.Add(this.ButtonAddIssue);
            this.AddIssue.Controls.Add(this.TextBoxAddPR);
            this.AddIssue.Controls.Add(this.TextBoxAddComent);
            this.AddIssue.Controls.Add(this.LabelAddPr);
            this.AddIssue.Controls.Add(this.LabelAddComent);
            this.AddIssue.Location = new System.Drawing.Point(4, 25);
            this.AddIssue.Name = "AddIssue";
            this.AddIssue.Size = new System.Drawing.Size(736, 165);
            this.AddIssue.TabIndex = 0;
            this.AddIssue.Text = "Add Issue";
            // 
            // LabelAddMessageText
            // 
            this.LabelAddMessageText.Location = new System.Drawing.Point(18, 21);
            this.LabelAddMessageText.Name = "LabelAddMessageText";
            this.LabelAddMessageText.Size = new System.Drawing.Size(106, 16);
            this.LabelAddMessageText.TabIndex = 0;
            this.LabelAddMessageText.Text = "Message Text :";
            // 
            // TextBoxAddMessageText
            // 
            this.TextBoxAddMessageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAddMessageText.Location = new System.Drawing.Point(144, 18);
            this.TextBoxAddMessageText.Name = "TextBoxAddMessageText";
            this.TextBoxAddMessageText.Size = new System.Drawing.Size(563, 22);
            this.TextBoxAddMessageText.TabIndex = 0;
            this.TextBoxAddMessageText.TextChanged += new System.EventHandler(this.TextBoxAddMessageText_TextChanged);
            this.TextBoxAddMessageText.Leave += new System.EventHandler(this.TextBoxAddMessageText_Leave);
            // 
            // ButtonAddIssue
            // 
            this.ButtonAddIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAddIssue.Location = new System.Drawing.Point(621, 92);
            this.ButtonAddIssue.Name = "ButtonAddIssue";
            this.ButtonAddIssue.Size = new System.Drawing.Size(87, 37);
            this.ButtonAddIssue.TabIndex = 3;
            this.ButtonAddIssue.Text = "Add";
            this.ButtonAddIssue.Click += new System.EventHandler(this.ButtonAddIssue_Click);
            // 
            // TextBoxAddPR
            // 
            this.TextBoxAddPR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAddPR.Location = new System.Drawing.Point(144, 92);
            this.TextBoxAddPR.Name = "TextBoxAddPR";
            this.TextBoxAddPR.Size = new System.Drawing.Size(0, 22);
            this.TextBoxAddPR.TabIndex = 2;
            // 
            // TextBoxAddComent
            // 
            this.TextBoxAddComent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAddComent.Location = new System.Drawing.Point(144, 55);
            this.TextBoxAddComent.Name = "TextBoxAddComent";
            this.TextBoxAddComent.Size = new System.Drawing.Size(563, 22);
            this.TextBoxAddComent.TabIndex = 1;
            // 
            // LabelAddPr
            // 
            this.LabelAddPr.Location = new System.Drawing.Point(20, 99);
            this.LabelAddPr.Name = "LabelAddPr";
            this.LabelAddPr.Size = new System.Drawing.Size(96, 30);
            this.LabelAddPr.TabIndex = 0;
            this.LabelAddPr.Text = "PR :";
            // 
            // LabelAddComent
            // 
            this.LabelAddComent.Location = new System.Drawing.Point(19, 60);
            this.LabelAddComent.Name = "LabelAddComent";
            this.LabelAddComent.Size = new System.Drawing.Size(106, 23);
            this.LabelAddComent.TabIndex = 0;
            this.LabelAddComent.Text = "Comment :";
            // 
            // RemoveIssue
            // 
            this.RemoveIssue.Controls.Add(this.LabelErrorMessage);
            this.RemoveIssue.Controls.Add(this.ButtonSaveIssue);
            this.RemoveIssue.Controls.Add(this.TextBoxPr);
            this.RemoveIssue.Controls.Add(this.TextBoxComment);
            this.RemoveIssue.Controls.Add(this.checkBoxIgnoreResult);
            this.RemoveIssue.Controls.Add(this.checkBoxIgnoreResultAll);
            this.RemoveIssue.Controls.Add(this.LabelPr);
            this.RemoveIssue.Controls.Add(this.LabelComment);
            this.RemoveIssue.Location = new System.Drawing.Point(4, 25);
            this.RemoveIssue.Name = "RemoveIssue";
            this.RemoveIssue.Size = new System.Drawing.Size(929, 165);
            this.RemoveIssue.TabIndex = 0;
            this.RemoveIssue.Text = " Issue Details";
            // 
            // LabelErrorMessage
            // 
            this.LabelErrorMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.LabelErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.LabelErrorMessage.Location = new System.Drawing.Point(0, 0);
            this.LabelErrorMessage.Name = "LabelErrorMessage";
            this.LabelErrorMessage.Size = new System.Drawing.Size(929, 28);
            this.LabelErrorMessage.TabIndex = 0;
            this.LabelErrorMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ButtonSaveIssue
            // 
            this.ButtonSaveIssue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonSaveIssue.Location = new System.Drawing.Point(824, 120);
            this.ButtonSaveIssue.Name = "ButtonSaveIssue";
            this.ButtonSaveIssue.Size = new System.Drawing.Size(77, 37);
            this.ButtonSaveIssue.TabIndex = 5;
            this.ButtonSaveIssue.Text = "Save";
            this.ButtonSaveIssue.Click += new System.EventHandler(this.ButtonSaveIssue_Click);
            // 
            // TextBoxPr
            // 
            this.TextBoxPr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxPr.Location = new System.Drawing.Point(125, 67);
            this.TextBoxPr.Name = "TextBoxPr";
            this.TextBoxPr.Size = new System.Drawing.Size(785, 22);
            this.TextBoxPr.TabIndex = 2;
            // 
            // TextBoxComment
            // 
            this.TextBoxComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxComment.Location = new System.Drawing.Point(125, 30);
            this.TextBoxComment.Name = "TextBoxComment";
            this.TextBoxComment.Size = new System.Drawing.Size(785, 22);
            this.TextBoxComment.TabIndex = 1;
            // 
            // checkBoxIgnoreResult
            // 
            this.checkBoxIgnoreResult.Location = new System.Drawing.Point(29, 148);
            this.checkBoxIgnoreResult.Name = "checkBoxIgnoreResult";
            this.checkBoxIgnoreResult.Size = new System.Drawing.Size(374, 18);
            this.checkBoxIgnoreResult.TabIndex = 4;
            this.checkBoxIgnoreResult.Text = "Ignore This Result in This result File";
            this.checkBoxIgnoreResult.Click += new System.EventHandler(this.checkBoxIgnoreResult_Click);
            // 
            // checkBoxIgnoreResultAll
            // 
            this.checkBoxIgnoreResultAll.Location = new System.Drawing.Point(29, 111);
            this.checkBoxIgnoreResultAll.Name = "checkBoxIgnoreResultAll";
            this.checkBoxIgnoreResultAll.Size = new System.Drawing.Size(374, 18);
            this.checkBoxIgnoreResultAll.TabIndex = 3;
            this.checkBoxIgnoreResultAll.Text = "Ignore This Result in all result Files";
            this.checkBoxIgnoreResultAll.Click += new System.EventHandler(this.checkBoxIgnoreResultAll_Click);
            // 
            // LabelPr
            // 
            this.LabelPr.Location = new System.Drawing.Point(29, 74);
            this.LabelPr.Name = "LabelPr";
            this.LabelPr.Size = new System.Drawing.Size(67, 18);
            this.LabelPr.TabIndex = 0;
            this.LabelPr.Text = "PR :";
            // 
            // LabelComment
            // 
            this.LabelComment.Location = new System.Drawing.Point(19, 37);
            this.LabelComment.Name = "LabelComment";
            this.LabelComment.Size = new System.Drawing.Size(77, 18);
            this.LabelComment.TabIndex = 0;
            this.LabelComment.Text = "Comment :";
            // 
            // TabScript
            // 
            this.TabScript.Controls.Add(this.RichTextBoxScript);
            this.TabScript.Controls.Add(this.webBrowserScript);
            this.TabScript.Location = new System.Drawing.Point(4, 25);
            this.TabScript.Name = "TabScript";
            this.TabScript.Size = new System.Drawing.Size(744, 696);
            this.TabScript.TabIndex = 0;
            this.TabScript.Text = "      Script      ";
            this.TabScript.UseVisualStyleBackColor = true;
            // 
            // RichTextBoxScript
            // 
            this.RichTextBoxScript.ContextMenu = this.ContextMenuRichTextBox;
            this.RichTextBoxScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RichTextBoxScript.Location = new System.Drawing.Point(0, 0);
            this.RichTextBoxScript.Name = "RichTextBoxScript";
            this.RichTextBoxScript.ReadOnly = true;
            this.RichTextBoxScript.Size = new System.Drawing.Size(744, 696);
            this.RichTextBoxScript.TabIndex = 0;
            this.RichTextBoxScript.Text = "";
            // 
            // webBrowserScript
            // 
            this.webBrowserScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserScript.Location = new System.Drawing.Point(0, 0);
            this.webBrowserScript.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserScript.Name = "webBrowserScript";
            this.webBrowserScript.Size = new System.Drawing.Size(744, 696);
            this.webBrowserScript.TabIndex = 1;
            this.webBrowserScript.CanGoBackChanged += new System.EventHandler(this.webBrowserScript_CanGoBackChanged);
            this.webBrowserScript.CanGoForwardChanged += new System.EventHandler(this.webBrowserScript_CanGoForwardChanged);
            this.webBrowserScript.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserScript_DocumentCompleted);
            this.webBrowserScript.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserScript_Navigated);
            this.webBrowserScript.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserScript_Navigating);
            // 
            // TabEmpty
            // 
            this.TabEmpty.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TabEmpty.Controls.Add(this.webBrowserStartScreen);
            this.TabEmpty.Location = new System.Drawing.Point(4, 25);
            this.TabEmpty.Name = "TabEmpty";
            this.TabEmpty.Size = new System.Drawing.Size(744, 696);
            this.TabEmpty.TabIndex = 1;
            this.TabEmpty.Text = "Welcome";
            this.TabEmpty.UseVisualStyleBackColor = true;
            // 
            // webBrowserStartScreen
            // 
            this.webBrowserStartScreen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserStartScreen.Location = new System.Drawing.Point(0, 0);
            this.webBrowserStartScreen.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserStartScreen.Name = "webBrowserStartScreen";
            this.webBrowserStartScreen.Size = new System.Drawing.Size(744, 696);
            this.webBrowserStartScreen.TabIndex = 0;
            // 
            // TabNoInformationAvailable
            // 
            this.TabNoInformationAvailable.Controls.Add(this.LabelNoInformationAvailable);
            this.TabNoInformationAvailable.Location = new System.Drawing.Point(4, 25);
            this.TabNoInformationAvailable.Name = "TabNoInformationAvailable";
            this.TabNoInformationAvailable.Size = new System.Drawing.Size(744, 696);
            this.TabNoInformationAvailable.TabIndex = 0;
            this.TabNoInformationAvailable.UseVisualStyleBackColor = true;
            // 
            // LabelNoInformationAvailable
            // 
            this.LabelNoInformationAvailable.Location = new System.Drawing.Point(355, 37);
            this.LabelNoInformationAvailable.Name = "LabelNoInformationAvailable";
            this.LabelNoInformationAvailable.Size = new System.Drawing.Size(173, 18);
            this.LabelNoInformationAvailable.TabIndex = 0;
            this.LabelNoInformationAvailable.Text = "No information available";
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(112, 432);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 40);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(212, 0);
            this.splitter1.MinSize = 177;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 725);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // contextMenuUserControlSessionTree
            // 
            this.contextMenuUserControlSessionTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ContextMenu_AddExistingSessions,
            this.ContextMenu_AddNewSession,
            this.ContextMenu_Edit,
            this.ContextMenu_Execute,
            this.ContextMenu_ExploreResultsDir,
            this.ContextMenu_ExploreScriptsDir,
            this.ContextMenu_None,
            this.ContextMenu_Remove,
            this.ContextMenu_RemoveAllResultsFiles,
            this.ContextMenu_RemoveSessionFromProject,
            this.ContextMenu_Save,
            this.ContextMenu_SaveAs,
            this.ContextMenu_ValidateMediaFiles,
            this.ContextMenu_GenerateDICOMDIR,
            this.ContextMenu_GenerateDICOMDIRWithDirectory,
            this.ContextMenu_ViewExpandedScript,
            this.ContextMenu_ValidateDicomdirWithoutRefFile,
            this.ContextMenu_ValidateMediaDirectory});
            this.contextMenuUserControlSessionTree.Popup += new System.EventHandler(this.contextMenuUserControlSessionTree_Popup);
            // 
            // ContextMenu_GenerateDICOMDIRWithDirectory
            // 
            this.ContextMenu_GenerateDICOMDIRWithDirectory.Index = 14;
            this.ContextMenu_GenerateDICOMDIRWithDirectory.Text = "Create DICOMDIR using Media Directory";
            this.ContextMenu_GenerateDICOMDIRWithDirectory.Click += new System.EventHandler(this.ContextMenu_GenerateDICOMDIRWithDirectory_Click);
            // 
            // ContextMenu_ValidateDicomdirWithoutRefFile
            // 
            this.ContextMenu_ValidateDicomdirWithoutRefFile.Index = 16;
            this.ContextMenu_ValidateDicomdirWithoutRefFile.Text = "Validate DICOMDIR without Reference File";
            this.ContextMenu_ValidateDicomdirWithoutRefFile.Click += new System.EventHandler(this.ContextMenu_ValidateDicomdirWithoutRefFile_Click);
            // 
            // ContextMenu_ValidateMediaDirectory
            // 
            this.ContextMenu_ValidateMediaDirectory.Index = 17;
            this.ContextMenu_ValidateMediaDirectory.Text = "Validate Media Directory...";
            this.ContextMenu_ValidateMediaDirectory.Click += new System.EventHandler(this.ContextMenu_ValidateMediaDirectory_Click);
            // 
            // userControlSessionTree
            // 
            this.userControlSessionTree.ContextMenu = this.contextMenuUserControlSessionTree;
            this.userControlSessionTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.userControlSessionTree.Location = new System.Drawing.Point(0, 0);
            this.userControlSessionTree.Name = "userControlSessionTree";
            this.userControlSessionTree.ProjectForm = null;
            this.userControlSessionTree.Size = new System.Drawing.Size(212, 725);
            this.userControlSessionTree.TabIndex = 0;
            this.userControlSessionTree.TabStop = false;
            // 
            // ProjectForm2
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(964, 725);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.userControlSessionTree);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.MainMenu_ProjectForm;
            this.Name = "ProjectForm2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.ProjectForm2_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ProjectForm2_Closing);
            this.Load += new System.EventHandler(this.ProjectForm2_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProjectForm2_MouseDown);
            this.TabControl.ResumeLayout(false);
            this.TabSessionInformation.ResumeLayout(false);
            this.PanelSecuritySettingsContent.ResumeLayout(false);
            this.GroupSecurityFiles.ResumeLayout(false);
            this.GroupSecurityFiles.PerformLayout();
            this.GroupSecurityVersion.ResumeLayout(false);
            this.GroupSecurityKeyExchange.ResumeLayout(false);
            this.GroupSecurityGeneral.ResumeLayout(false);
            this.GroupSecurityEncryption.ResumeLayout(false);
            this.GroupSecurityDataIntegrity.ResumeLayout(false);
            this.GroupSecurityAuthentication.ResumeLayout(false);
            this.PanelSecuritySettingsTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinSecuritySettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxSecuritySettings)).EndInit();
            this.PanelSUTSettingContent.ResumeLayout(false);
            this.PanelSUTSettingContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTListenPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTMaxPDU)).EndInit();
            this.PanelSUTSettingTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinSUTSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxSUTSettings)).EndInit();
            this.PanelDVTRoleSettingsContent.ResumeLayout(false);
            this.PanelDVTRoleSettingsContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTListenPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTTimeOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTMaxPDU)).EndInit();
            this.PanelDVTRoleSettingsTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinDVTRoleSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDVTRoleSettings)).EndInit();
            this.PanelGeneralPropertiesContent.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSessonID)).EndInit();
            this.PanelGeneralPropertiesTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MinGSPSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxGSPSettings)).EndInit();
            this.TabSpecifySopClasses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridSpecifySopClasses)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.TabActivityLogging.ResumeLayout(false);
            this.TabValidationResults.ResumeLayout(false);
            this.WebBrowserPanel.ResumeLayout(false);
            this.TabResultsManager.ResumeLayout(false);
            this.PanelWebBrowserResultsManager.ResumeLayout(false);
            this.PanelResultsManagerTiltle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMinResultsTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxMaximizeResultTab)).EndInit();
            this.TabControlIssues.ResumeLayout(false);
            this.AddIssue.ResumeLayout(false);
            this.AddIssue.PerformLayout();
            this.RemoveIssue.ResumeLayout(false);
            this.RemoveIssue.PerformLayout();
            this.TabScript.ResumeLayout(false);
            this.TabEmpty.ResumeLayout(false);
            this.TabNoInformationAvailable.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theEvent"></param>
		public void Notify(object theEvent) 
		{
			if (theEvent is EndExecution) 
			{
				_State = ProjectFormState.IDLE;
			}
			if (theEvent is StartExecution) 
			{
				if (userControlSessionTree.GetSelectedTag() is DvtkApplicationLayer.Script) 
				{
					_State = ProjectFormState.EXECUTING_SCRIPT;
				}
				else if (userControlSessionTree.GetSelectedTag() is DvtkApplicationLayer.Emulator) 
				{
					DvtkApplicationLayer.Emulator emulator = new DvtkApplicationLayer.Emulator();
					emulator = (DvtkApplicationLayer.Emulator)userControlSessionTree.GetSelectedTag();
					//
					switch(emulator.EmulatorType) 
					{
						case DvtkApplicationLayer.Emulator.EmulatorTypes.PRINT_SCP:
							_State = ProjectFormState.EXECUTING_PRINT_SCP;
							break;

						case DvtkApplicationLayer.Emulator.EmulatorTypes.STORAGE_SCP:
							_State = ProjectFormState.EXECUTING_STORAGE_SCP;
							break;

						case DvtkApplicationLayer.Emulator.EmulatorTypes.STORAGE_SCU:
							_State = ProjectFormState.EXECUTING_STORAGE_SCU;
							break;
					}
				}
				else if (userControlSessionTree.GetSelectedTag() is DvtkApplicationLayer.MediaSession) 
				{
					_State = ProjectFormState.EXECUTING_MEDIA_VALIDATION;
				}
				else 
				{
					// Sanity check.
					throw new System.ApplicationException("Not supposed to get here.");
				}
			}			

			if (this.ParentForm is MainForm) 
			{
				((MainForm)this.ParentForm).Notify(this, theEvent);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSource"></param>
		/// <param name="theEvent"></param>
		public void Update(object theSource, object theEvent) 
		{
			// This event is required to get the focus on selected project form view
			// from multiple views of project form
			this.Enter += new EventHandler(ProjectForm2_Enter);
			// Update the Session Tree View.
			userControlSessionTree.Update(theSource, theEvent);
			// Update the Tab Control.
			TCM_Update(theSource, theEvent);
						
			// Update the text (caption) of this Project Form for certain events.
			if ( (theEvent is SessionTreeViewSelectionChange) ||
				(theEvent is SessionRemovedEvent)
				) 
			{
				if (userControlSessionTree.GetSelectedTag() is DvtkApplicationLayer.Project)
				{
					Text = projectApp.ProjectFileName ;
				}
				else 
                {
					if (userControlSessionTree.GetSession() == null) 
					{
						Text = "";
					}
					else 
					{
						Text = userControlSessionTree.GetSession().SessionFileName;
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="theSource"></param>
		/// <param name="theEvent"></param>
		public void TCM_Update(object theSource, object theEvent) 
		{
			_TCM_UpdateCount++;

			if (theEvent is UpdateAll) 
			{
				// Don't do anything.
				// The Session tree view will be completely updated, and
				// as an indirect result, this tab control will be updated
				// (if another tree view node is selected).
			}
			if (theEvent is SessionTreeViewSelectionChange) 
			{
				// Update the tab control only if this event is from the
				// associated tree view.
				if (theSource == this) 
				{		
					_SopClassesManager.RemoveDynamicDataBinding();
					TCM_UpdateTabsShown();
					TCM_UpdateTabContents();
				}
			}

			if (theEvent is SessionChange) 
			{			
				SessionChange theSessionChange = (SessionChange)theEvent;

				// The contents of a text box or numeric control has changed directly by the user.
				if (theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.OTHER) 
				{
					// Invalidate the session used for updating the Session Information tab only if 
					// this event is from another tab control.
					if (theSource != this) 
					{
						if (_TCM_SessionUsedForContentsOfTabSessionInformation == theSessionChange.SessionApp) 
						{
							// Invalidate this session, so the tab control will redraw when set as the active tab control.
							_TCM_SessionUsedForContentsOfTabSessionInformation = null;
						}
					}
				}

				// The session has changed by something else then a direct contents change of a text box or numeric control.
				if ( (theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.DESCRIPTION_DIR) ||
					(theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.RESULTS_DIR ) ||
					(theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.SCRIPTS_DIR ) ) 
				{
					if (_TCM_SessionUsedForContentsOfTabSessionInformation == theSessionChange.SessionApp) 
					{
						// Invalidate this session, so the tab control will redraw when set as the active tab control.
						_TCM_SessionUsedForContentsOfTabSessionInformation = null;
					}			
				}

				// The SOP classes manager is only interested in changes of SOP classes related settings.
				// This is not important for the other tabs.
				if (theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_OTHER) 
				{
					_SopClassesManager.SetSessionChanged(theSessionChange.SessionApp);
				}

				// The SOP classes manager is only interested in changes of SOP classes related settings.
				// This is not important for the other tabs.
				if (theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_AE_TITLE_VERSION) 
				{
					if (theSource != this) 
					{
						_SopClassesManager.SetSessionChanged(theSessionChange.SessionApp);
					}
				}

				// The SOP classes manager is only interested in changes of SOP classes related settings.
				// This is not important for the other tabs.
				if (theSessionChange.SessionChangeSubTyp == SessionChange.SessionChangeSubTypEnum.SOP_CLASSES_LOADED_STATE) 
				{
					if (theSource != this) 
					{
						_SopClassesManager.SetSessionChanged(theSessionChange.SessionApp);
					}
				}

				// Update the current visible tab control, only if necessary.
				TCM_UpdateTabContents();
			}

			if (theEvent is StartExecution) 
			{
				if (theSource == this) 
				{
					DvtkApplicationLayer.Session tempSession = null ;
					StartExecution theStartExecution = (StartExecution)theEvent;
					// The tree node that is being executed.
					TreeNode theTreeNode = ((StartExecution)theEvent).TreeNode;

					// The tag of the tree node that is being executed.
					Object theTreeNodeTag = theTreeNode.Tag;
					if (theTreeNodeTag is DvtkApplicationLayer.PartOfSession) 
					{
						DvtkApplicationLayer.PartOfSession partOfSession = theTreeNodeTag as DvtkApplicationLayer.PartOfSession;
						tempSession = partOfSession.ParentSession;
					}
					else 
					{ 
						tempSession = (DvtkApplicationLayer.MediaSession)theTreeNodeTag;
					}
					TCM_ClearActivityLogging();
					tempSession.Implementation.ActivityReportEvent += _ActivityReportEventHandler;

					// For some strange reason, if the session tree control is disabled and this statement
					// is not present, switching to another application and back will result in a disabled
					// application (when the StartEmulatorExecution is received).
					TabControl.Focus();
				}
			}

			if (theEvent is EndExecution) 
			{
				if (theSource == this) 
				{
					DvtkApplicationLayer.Session tempSession = null ;
					EndExecution theEndExecution = (EndExecution)theEvent;
					Object theTreeNodeTag = ((EndExecution)theEndExecution)._Tag;

					if (theTreeNodeTag is DvtkApplicationLayer.PartOfSession) 
					{
						DvtkApplicationLayer.PartOfSession partOfSession = theTreeNodeTag as DvtkApplicationLayer.PartOfSession;
						tempSession = partOfSession.ParentSession ;
					}
					else 
					{ 
						tempSession = (DvtkApplicationLayer.MediaSession)theTreeNodeTag;
					}
					tempSession.Implementation.ActivityReportEvent -= _ActivityReportEventHandler;
				}
			}
			if (theEvent is SessionRemovedEvent) 
			{		
				TCM_UpdateTabsShown();

				TCM_UpdateTabContents();
			}
			_TCM_UpdateCount--;
		}

		#region Min and max handling

		private void MinGSPSettings_Click(object sender, System.EventArgs e) 
		{
			MinGSPSettings.Visible = false;
			MaxGSPSettings.Visible = true;
			PanelGeneralPropertiesContent.Visible = false;
		}

		private void MaxGSPSettings_Click(object sender, System.EventArgs e) 
		{
			MaxGSPSettings.Visible = false;
			MinGSPSettings.Visible = true;
			PanelGeneralPropertiesContent.Visible = true;
		}


		private void MinDVTRoleSettings_Click(object sender, System.EventArgs e) 
		{
			MinDVTRoleSettings.Visible = false;
			MaxDVTRoleSettings.Visible = true;
			PanelDVTRoleSettingsContent.Visible = false;
		}

		private void MaxDVTRoleSettings_Click(object sender, System.EventArgs e) 
		{
			MaxDVTRoleSettings.Visible = false;
			MinDVTRoleSettings.Visible = true;
			PanelDVTRoleSettingsContent.Visible = true;		
		}

		private void MinSUTSettings_Click(object sender, System.EventArgs e) 
		{
			MinSUTSettings.Visible = false;
			MaxSUTSettings.Visible = true;
			PanelSUTSettingContent.Visible = false;
		}
		private void MaxSUTSettings_Click(object sender, System.EventArgs e) 
		{
			MaxSUTSettings.Visible = false;
			MinSUTSettings.Visible = true;
			PanelSUTSettingContent.Visible = true;
		}

		private void MinSecuritySettings_Click(object sender, System.EventArgs e) 
		{
			_TCM_ShowMinSecuritySettings = false;
			MinSecuritySettings.Visible = false;
			MaxSecuritySettings.Visible = true;
			PanelSecuritySettingsContent.Visible = false;
		
		}

		private void MaxSecuritySettings_Click(object sender, System.EventArgs e) 
		{
			_TCM_ShowMinSecuritySettings = true;
			MaxSecuritySettings.Visible = false;
			MinSecuritySettings.Visible = true;
			PanelSecuritySettingsContent.Visible = true;
		}

		#endregion

		private void ListBoxSecuritySettings_SelectedIndexChanged(object sender, System.EventArgs e) 
		{
			if (_TCM_UpdateCount == 0) 
			{
				// Make all security setting groups invisible.
				GroupSecurityGeneral.Visible = false;
				GroupSecurityVersion.Visible = false;
				GroupSecurityAuthentication.Visible = false;
				GroupSecurityKeyExchange.Visible = false;
				GroupSecurityDataIntegrity.Visible = false;
				GroupSecurityEncryption.Visible = false;
				GroupSecurityFiles.Visible = false;

				// Make the selected security setting group visible only.
				switch (this.ListBoxSecuritySettings.SelectedIndex) 
				{
					case 0:
						GroupSecurityGeneral.Visible = true;
						break;
					case 1:
						GroupSecurityVersion.Visible = true;
						break;
					case 2:
						GroupSecurityAuthentication.Visible = true;
						break;
					case 3:
						GroupSecurityKeyExchange.Visible = true;
						break;
					case 4:
						GroupSecurityDataIntegrity.Visible = true;
						break;
					case 5:
						GroupSecurityEncryption.Visible = true;
						break;
					case 6:
						GroupSecurityFiles.Visible = true;
						break;
				}
			}
		}

		private void TCM_UpdateTabsShown() 
		{
			_TCM_UpdateCount++;

			bool theSessionInformationTabShown = false;
			bool theActivityLoggingTabShown = false;
			bool theScriptTabShown = false;
			bool theDetailedValidationTabShown = false;
			bool theSpecifySopClassesTabShown = false;
			bool theNoInformationAvailableTabShown = false;
			bool theResultsManagerTabShown = false ;
			bool theEmptyTabShown = false ;

			Object theSelectedTag = GetSelectedTag();
			TreeNode theSelectedNode = GetSelectedUserNode();
			DvtkApplicationLayer.Session theSelectedSession = GetSession();
			System.Windows.Forms.TabPage theTabWithFocus = null;

			// If no node (and indirectly a tag) is selected...
			if (theSelectedTag == null) 
			{
				theNoInformationAvailableTabShown = true;
				theTabWithFocus = TabNoInformationAvailable;
			}
				// If the selected session is executing...
			else if (((theSelectedTag is DvtkApplicationLayer.Session) ||(theSelectedTag is DvtkApplicationLayer.Emulator) || (theSelectedTag is DvtkApplicationLayer.Script) ) && (theSelectedSession.IsExecute)) 
			{  				
				// If it is executing in this project form...
				if (theSelectedSession == GetExecutingSession()) 
				{
					theActivityLoggingTabShown = true;
					theTabWithFocus = TabActivityLogging;
				}				
				// If it is not executing in this project form...
				else 
				{
					theNoInformationAvailableTabShown = true;
					theTabWithFocus = TabNoInformationAvailable;
				}				
			}
			else 
			{
				// Selected session is not executing.
				if ( (theSelectedTag is DvtkApplicationLayer.Session) ||
					(theSelectedTag is DvtkApplicationLayer.MediaFile) ||
					(theSelectedTag is DvtkApplicationLayer.Emulator)
					) 
				{
					theSessionInformationTabShown = true;
					theSpecifySopClassesTabShown = true;
					theActivityLoggingTabShown = true;
					theTabWithFocus = TabSessionInformation;
				}

				if (theSelectedTag is DvtkApplicationLayer.Script) 
				{
					theSessionInformationTabShown = true;
					theSpecifySopClassesTabShown = true;
					theActivityLoggingTabShown = true;
					theScriptTabShown = true;
					theTabWithFocus = TabScript;
				}

				if (theSelectedTag is DvtkApplicationLayer.Result) 
				{
					// If the parent of this result file node is a script file, show a script tab.
					if (theSelectedNode.Parent.Tag is DvtkApplicationLayer.Script) 
					{
						theScriptTabShown = true;
					}
					theSessionInformationTabShown = true;
					theSpecifySopClassesTabShown = true;
					theActivityLoggingTabShown = true;
					theDetailedValidationTabShown = true;
					theTabWithFocus = TabValidationResults;
				}
				if (theSelectedTag is DvtkApplicationLayer.Project)	
				{
					if (projectApp.Sessions.Count == 0)
					{
						theNoInformationAvailableTabShown = true ;
						theTabWithFocus = TabNoInformationAvailable ;
					}
					else
					{
						//theActivityLoggingTabShown = true;
						theEmptyTabShown = true;
						theResultsManagerTabShown = true;
						theTabWithFocus = TabEmpty;
                        webBrowserStartScreen.Navigate(Application.StartupPath + "\\" + "dvtStartScreen.html");
					}
				}
			}

			// Sanity check.
			if (theTabWithFocus == null) 
			{
				throw new System.ApplicationException("tab with focus expected to be set.");
			}

			// Determine if the tabs shown need to be changed.
			if ((_TCM_SessionInformationTabShown != theSessionInformationTabShown) ||
				(_TCM_ActivityLoggingTabShown != theActivityLoggingTabShown) ||
				(_TCM_ScriptTabShown != theScriptTabShown) ||
				(_TCM_DetailedValidationTabShown != theDetailedValidationTabShown) ||
				(_TCM_SpecifySopClassesTabShown != theSpecifySopClassesTabShown) ||
				(_TCM_ResultsManagerTabShown != theResultsManagerTabShown) ||
				(_TCM_NoInformationAvailableTabShown != theNoInformationAvailableTabShown) ||
				(_TCM_EmptyTabShown != theEmptyTabShown)) 
			{
				TabControl.Visible = false;

				TabControl.Controls.Clear();	
				
				if (theSessionInformationTabShown) 
				{
					TabControl.Controls.Add(TabSessionInformation);
				}

				if (theSpecifySopClassesTabShown) 
				{
					TabControl.Controls.Add(TabSpecifySopClasses);
				}

				if (theActivityLoggingTabShown) 
				{
					TabControl.Controls.Add(TabActivityLogging);
				}

				if (theScriptTabShown) 
				{
					TabControl.Controls.Add(TabScript);
				}

				if (theDetailedValidationTabShown) 
				{
					TabControl.Controls.Add(TabValidationResults);
				}

				if (theEmptyTabShown) 
				{
					TabControl.Controls.Add(TabEmpty);
				}

				if (theResultsManagerTabShown) 
				{
					TabControl.Controls.Add(TabResultsManager);
				}

				if (theNoInformationAvailableTabShown) 
				{
					TabControl.Controls.Add(TabNoInformationAvailable);
				}				

				if (theTabWithFocus != null) 
				{
					TabControl.SelectedTab = theTabWithFocus;
				}

				TabControl.Visible = true;

				_TCM_SessionInformationTabShown = theSessionInformationTabShown;
				_TCM_ActivityLoggingTabShown = theActivityLoggingTabShown;
				_TCM_ScriptTabShown = theScriptTabShown;
				_TCM_DetailedValidationTabShown = theDetailedValidationTabShown;
				_TCM_SpecifySopClassesTabShown = theSpecifySopClassesTabShown;
				_TCM_NoInformationAvailableTabShown = theNoInformationAvailableTabShown;
				_TCM_ResultsManagerTabShown = theResultsManagerTabShown;
				_TCM_EmptyTabShown = theEmptyTabShown;
			}

			if (theTabWithFocus != null) 
			{
				TabControl.SelectedTab = theTabWithFocus;
			}

			_TCM_UpdateCount--;
		}

		public void TCM_UpdateTabContents() 
		{
			_TCM_UpdateCount++;

			if (TabControl.SelectedTab == TabSessionInformation) 
			{
				TCM_UpdateTabSessionInformation();
			}

			if (TabControl.SelectedTab == TabActivityLogging) 
			{
				// The tab activity logging doesn't have to be updated explicitly.
				// It will be cleared when a script is started and text will be appended
				// during script execution.
			}

			if (TabControl.SelectedTab == TabScript) 
			{
				TCM_UpdateTabScript();
			}

			if (TabControl.SelectedTab == TabValidationResults) 
			{
				TCM_UpdateTabDetailedValidation();
			}
			if (TabControl.SelectedTab == TabResultsManager)
			{
				if(TabControl.Controls.Contains(TabEmpty))
					TabControl.Controls.Remove(TabEmpty);
				TCM_UpdateTabResultsManager();
			}

			if (TabControl.SelectedTab == TabSpecifySopClasses) 
			{
				_SopClassesManager.Update();
			}

			_TCM_UpdateCount--;
		}

		private void TCM_UpdateTabSessionInformation() 
		{
            
			_TCM_UpdateCount++;
			bool Update = false;
			DvtkApplicationLayer.Session theSelectedSession = GetSession();
			// If this session tab has not yet been filled, fill it.
			if (_TCM_SessionUsedForContentsOfTabSessionInformation == null) 
			{
				Update = true;
			}
			else 
			{
				// If the current session is not equal to the session used to fill the session information tab
				// last time, fill it again.
				if (_TCM_SessionUsedForContentsOfTabSessionInformation != theSelectedSession) 
				{
					Update = true;
				}
			}

			// Update the complete contents of the session information tab.
			if (Update) 
			{
				// General session properties.
				if (theSelectedSession is DvtkApplicationLayer.ScriptSession) 
				{
					TextBoxSessionType.Text = "Script";
				}
				if (theSelectedSession is DvtkApplicationLayer.MediaSession) 
				{
					TextBoxSessionType.Text = "Media";
				}
				if (theSelectedSession is DvtkApplicationLayer.EmulatorSession) 
				{
					TextBoxSessionType.Text = "Emulator";
				}
				TextBoxSessionTitle.Text = theSelectedSession.SessionTitle;
				NumericSessonID.Value = theSelectedSession.SessionId;
				TextBoxTestedBy.Text = theSelectedSession.TestedBy;
				TextBoxResultsRoot.Text = theSelectedSession.ResultsRootDirectory;
                TextBoxDataRoot.Text = theSelectedSession.DataDirectory;
				if (theSelectedSession is DvtkApplicationLayer.ScriptSession) 
				{
					DvtkApplicationLayer.ScriptSession theSelectedScriptSession;
					theSelectedScriptSession = (DvtkApplicationLayer.ScriptSession)theSelectedSession;

					LabelScriptsDir.Visible = true;
					TextBoxScriptRoot.Visible = true;
					ButtonBrowseScriptsDir.Visible = true;
					LabelDescriptionDir.Visible = true;
					TextBoxDescriptionRoot.Visible = true;
					ButtonBrowseDescriptionDir.Visible = true;

					TextBoxScriptRoot.Text = theSelectedScriptSession.DicomScriptRootDirectory;
					TextBoxDescriptionRoot.Text = theSelectedScriptSession.DescriptionDirectory;
					CheckBoxDefineSQLength.Checked = theSelectedScriptSession.DefineSqLength;
					CheckBoxAddGroupLengths.Checked = theSelectedScriptSession.AddGroupLength;
					TextBoxDVTImplClassUID.Text = theSelectedScriptSession.DvtImplementationClassUid;
					TextBoxDVTImplVersionName.Text = theSelectedScriptSession.DvtImplementationVersionName;
					TextBoxDVTAETitle.Text = theSelectedScriptSession.DvtAeTitle;
					NumericDVTListenPort.Value = theSelectedScriptSession.DvtPort;
					NumericDVTTimeOut.Value = theSelectedScriptSession.DvtSocketTimeout;
					NumericDVTMaxPDU.Value = theSelectedScriptSession.DvtMaximumLengthReceived;
					PanelDVTRoleSettingsTitle.Visible = true;
					if (MinDVTRoleSettings.Visible) 
					{
						PanelDVTRoleSettingsContent.Visible = true;
					}
					TextBoxSUTImplClassUID.Text = theSelectedScriptSession.SutImplementationClassUid;
					TextBoxSUTImplVersionName.Text = theSelectedScriptSession.SutImplementationVersionName;
					TextBoxSUTAETitle.Text = theSelectedScriptSession.SutAeTitle;
					NumericSUTListenPort.Value = theSelectedScriptSession.SutPort;
					TextBoxSUTTCPIPAddress.Text = theSelectedScriptSession.SutHostName;
					NumericSUTMaxPDU.Value = theSelectedScriptSession.SutMaximumLengthReceived;
					PanelSUTSettingTitle.Visible = true;
					if (MinSUTSettings.Visible) 
					{
						PanelSUTSettingContent.Visible = true;
					}
				}
				else 
				{
					LabelScriptsDir.Visible = false;
					TextBoxScriptRoot.Visible = false;
					ButtonBrowseScriptsDir.Visible = false;
					LabelDescriptionDir.Visible = false;
					TextBoxDescriptionRoot.Visible = false;
					ButtonBrowseDescriptionDir.Visible = false;
                
					PanelDVTRoleSettingsTitle.Visible = false;
					PanelDVTRoleSettingsContent.Visible = false;
				}

				CheckBoxGenerateDetailedValidationResults.Checked = theSelectedSession.DetailedValidationResults;

				if (theSelectedSession is DvtkApplicationLayer.MediaSession) 
				{
					CheckBoxDefineSQLength.Visible = false;
					CheckBoxAddGroupLengths.Visible = false;
					PanelDVTRoleSettingsContent.Visible = false;
					PanelDVTRoleSettingsTitle.Visible = false;
					PanelSUTSettingContent.Visible = false;
					PanelSUTSettingTitle.Visible = false;
				}
				else 
				{
					CheckBoxDefineSQLength.Visible = true;
					CheckBoxAddGroupLengths.Visible = true;
				}

				switch(theSelectedSession.Mode) 
				{
					case DvtkApplicationLayer.Session.StorageMode.AsDataSet:
						ComboBoxStorageMode.SelectedIndex = 0;								
						break;

					case DvtkApplicationLayer.Session.StorageMode.AsMedia:
						ComboBoxStorageMode.SelectedIndex = 1;								
						break;

					case DvtkApplicationLayer.Session.StorageMode.AsMediaOnly:
						ComboBoxStorageMode.SelectedIndex = 2;								
						break;

					case DvtkApplicationLayer.Session.StorageMode.NoStorage:
						ComboBoxStorageMode.SelectedIndex = 3;								
						break;

					default:
						// Not supposed to get here.
						Debug.Assert(false);
						break;
				}

				if (theSelectedSession is DvtkApplicationLayer.EmulatorSession) 
				{
					DvtkApplicationLayer.EmulatorSession theSelectedEmulatorSession;
					theSelectedEmulatorSession = (DvtkApplicationLayer.EmulatorSession)theSelectedSession;

					ButtonSpecifyTransferSyntaxes.Visible = true;
					LabelSpecifyTransferSyntaxes.Visible = true;
					CheckBoxDefineSQLength.Checked = theSelectedEmulatorSession.DefineSqLength;
					CheckBoxAddGroupLengths.Checked = theSelectedEmulatorSession.AddGroupLength;
					TextBoxDVTImplClassUID.Text = theSelectedEmulatorSession.DvtImplementationClassUid;   
					TextBoxDVTImplVersionName.Text = theSelectedEmulatorSession.DvtImplementationVersionName;
					TextBoxDVTAETitle.Text = theSelectedEmulatorSession.DvtAeTitle;
					NumericDVTListenPort.Value = theSelectedEmulatorSession.DvtPort;
					NumericDVTTimeOut.Value = theSelectedEmulatorSession.DvtSocketTimeout;
					NumericDVTMaxPDU.Value = theSelectedEmulatorSession.DvtMaximumLengthReceived;
					PanelDVTRoleSettingsTitle.Visible = true;
					if (MinDVTRoleSettings.Visible) 
					{
						PanelDVTRoleSettingsContent.Visible = true;
					}
					TextBoxSUTImplClassUID.Text = theSelectedEmulatorSession.SutImplementationClassUid;
					TextBoxSUTImplVersionName.Text = theSelectedEmulatorSession.SutImplementationVersionName;
					TextBoxSUTAETitle.Text = theSelectedEmulatorSession.SutAeTitle;
					NumericSUTListenPort.Value = theSelectedEmulatorSession.SutPort;
					TextBoxSUTTCPIPAddress.Text = theSelectedEmulatorSession.SutHostName;
					NumericSUTMaxPDU.Value = theSelectedEmulatorSession.SutMaximumLengthReceived;
                
					PanelSUTSettingTitle.Visible = true;
					if (MinSUTSettings.Visible) 
					{
						PanelSUTSettingContent.Visible = true;
					}
				}
				else 
				{
					ButtonSpecifyTransferSyntaxes.Visible = false;
					LabelSpecifyTransferSyntaxes.Visible = false;
				}
                Dvtk.Sessions.LogLevelFlags flag = (Dvtk.Sessions.LogLevelFlags)((GetSelectedSessionNew().LogLevelMask));
				CheckBoxLogRelation.Checked = ((flag & Dvtk.Sessions.LogLevelFlags.ImageRelation) == Dvtk.Sessions.LogLevelFlags.ImageRelation);
                CheckBoxDisplayCondition.Checked = theSelectedSession.DisplayConditionText;

				// Security settings.
				if (theSelectedSession.Implementation is Dvtk.Sessions.ISecure) 
				{
					Dvtk.Sessions.ISecuritySettings theISecuritySettings = null;

					theISecuritySettings = (theSelectedSession.Implementation as Dvtk.Sessions.ISecure).SecuritySettings;

					CheckBoxSecureConnection.Checked = theISecuritySettings.SecureSocketsEnabled;

					if (CheckBoxSecureConnection.Checked) 
					{
						if (_TCM_ShowMinSecuritySettings) 
						{
							PanelSecuritySettingsTitle.Visible = true;
							PanelSecuritySettingsContent.Visible = true;

							MinSecuritySettings.Visible = true;
							MaxSecuritySettings.Visible = false;

							CheckBoxCheckRemoteCertificates.Checked = theISecuritySettings.CheckRemoteCertificate;
							CheckBoxCacheSecureSessions.Checked = theISecuritySettings.CacheTlsSessions;
							CheckBoxTLS.Checked = ((theISecuritySettings.TlsVersionFlags & Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1) != 0);
							CheckBoxSSL.Checked = ((theISecuritySettings.TlsVersionFlags & Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_SSLv3) != 0);

                            Dvtk.Sessions.CipherFlags currentCipherFlags = theISecuritySettings.CipherFlags;
                            bool areSecuritySettingsChanged = theISecuritySettings.securitySettingsChanged;

                            if (areSecuritySettingsChanged)
                            {
                                if ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES) != 0)
                                {
                                    RadioButtonEncryptionTripleDES.Checked = true;
                                    UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, RadioButtonEncryptionTripleDES);
                                }
                                else
                                {
                                    RadioButtonEncryptionTripleDES.Checked = false;
                                    UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, RadioButtonEncryptionTripleDES);
                                }
                            }
                            
							RadioButtonAuthenticationDSA.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_DSA) != 0);
							RadioButtonAuthenticationRSA.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_RSA) != 0);
							RadioButtonDataIntegrityMD5.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5) != 0);
							RadioButtonDataIntegritySHA.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1) != 0);
							RadioButtonEncryptionNone.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE) != 0);
							RadioButtonEncryptionTripleDES.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES) != 0);
							RadioButtonEncryptionAES128.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128) != 0);
							RadioButtonEncryptionAES256.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256) != 0);
							RadioButtonKeyExchangeDH.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH) != 0);
							RadioButtonKeyExchangeRSA.Checked = ((currentCipherFlags & Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA) != 0);
							TextBoxTrustedCertificatesFile.Text = theISecuritySettings.CertificateFileName;
							TextBoxSecurityCredentialsFile.Text = theISecuritySettings.CredentialsFileName;
						}
						else 
						{
							PanelSecuritySettingsContent.Visible = false;

							MinSecuritySettings.Visible = false;
							MaxSecuritySettings.Visible = true;
						}
					}
					else 
					{
						PanelSecuritySettingsTitle.Visible = true;
						PanelSecuritySettingsContent.Visible = false;

						MinSecuritySettings.Visible = false;
						MaxSecuritySettings.Visible = false;
					}
				}
				else 
				{
					PanelSecuritySettingsTitle.Visible = false;
					PanelSecuritySettingsContent.Visible = false;
				}

				_TCM_SessionUsedForContentsOfTabSessionInformation = theSelectedSession;
			}
           
			_TCM_UpdateCount-- ;
		}

		// May only be called from UpdateTabContents.
		private void TCM_UpdateTabScript() 
		{
			_TCM_UpdateCount++;

			bool theHtmlDescriptionExists = false;
			DvtkApplicationLayer.Script scriptFileTag = null;
			string theHtmlDescriptionFileName = null;
			DvtkApplicationLayer.ScriptSession scriptSession = null;
			Object selectedTag = GetSelectedTag();
			TreeNode selectedNode = GetSelectedUserNode();
			if (selectedTag is DvtkApplicationLayer.Script) 
			{
				scriptFileTag = (DvtkApplicationLayer.Script)selectedTag;
			}
			else if (selectedTag is DvtkApplicationLayer.Result) 
			{
				if (selectedNode.Parent.Tag is DvtkApplicationLayer.Script) 
				{
					scriptFileTag = (DvtkApplicationLayer.Script)selectedNode.Parent.Tag;
				}
			}

			if (scriptFileTag == null) 
			{
				// Not supposed to get here.
				throw new System.ApplicationException("Error: not expected to get here.");
			}

			scriptSession = (DvtkApplicationLayer.ScriptSession)GetSession();

			// Is the description directory not empty?
			if (scriptSession.DescriptionDirectory != "") 
			{
				DirectoryInfo theDirectoryInfo = new DirectoryInfo(scriptSession.DescriptionDirectory);

				// Does the description directory exist?
				if (theDirectoryInfo.Exists) 
				{
					theHtmlDescriptionFileName = Path.Combine(scriptSession.DescriptionDirectory, scriptFileTag.ScriptFileName);
					theHtmlDescriptionFileName = theHtmlDescriptionFileName.Replace ('.', '_') + ".html";

					// Does the html description file exists?
					if (File.Exists(theHtmlDescriptionFileName)) 
					{
						// Now we know the html description file exists.
						theHtmlDescriptionExists = true;
					}
				}
			}
	
			if (theHtmlDescriptionExists) 
			{
				RichTextBoxScript.Visible = false;
                this.webBrowserScript.Dispose();
                this.webBrowserScript = new System.Windows.Forms.WebBrowser();
                SetProperties_webBrowserScript();
                this.TabScript.Controls.Add(this.webBrowserScript);
				webBrowserScript.Visible = true;

                webBrowserScript.Navigate(theHtmlDescriptionFileName);
			}
			else 
			{
				webBrowserScript.Visible = false;
				RichTextBoxScript.Visible = true;

				RichTextBoxScript.Clear();
					
				// If this is a Visual Basic Script...
				if (scriptFileTag.ScriptFileName.ToLower().EndsWith(".vbs")) 
				{
					DvtkApplicationLayer.VisualBasicScript applicationLayerVisualBasicScript = 
						new DvtkApplicationLayer.VisualBasicScript(scriptSession.ScriptSessionImplementation, scriptFileTag.ScriptFileName);

					if (_MainForm._UserSettings.ExpandVisualBasicScript) 
					{
						String includeErrors = "";
						String expandedContent = applicationLayerVisualBasicScript.GetExpandedContent(out includeErrors);

						// If include errors exist...
						if (includeErrors.Length > 0) 
						{
							RichTextBoxScript.Text = includeErrors;
							RichTextBoxScript.SelectAll();
							RichTextBoxScript.SelectionColor = Color.Red;
							RichTextBoxScript.Select(0, 0);
							RichTextBoxScript.SelectionColor = Color.Black;
							RichTextBoxScript.AppendText(expandedContent);
						}
						// If no include errors exist...
						else 
						{
							RichTextBoxScript.Text = expandedContent;
						}
					}
					else 
					{
						RichTextBoxScript.Text = applicationLayerVisualBasicScript.GetContent();
					}
				}
				// If this is not a visual basic script...
				else 
				{
					string theFullScriptFileName;

					theFullScriptFileName = Path.Combine(scriptSession.DicomScriptRootDirectory, scriptFileTag.ScriptFileName);

					RichTextBoxScript.LoadFile(theFullScriptFileName, RichTextBoxStreamType.PlainText);
				}
			}

			_TCM_UpdateCount--;
		}

		// May only be called from UpdateTabContents.
		private void TCM_UpdateTabDetailedValidation() 
		{
			this.TabControlIssues.TabPages.Clear();
			this.TabControlIssues.Controls.Add(this.AddIssue);
			this.TabControlIssues.Controls.Add(this.RemoveIssue);
			this.TabControlIssues.Visible = false ;
			this.PictureBoxMaximizeResultTab.Visible = true ;
			this.PictureBoxMinResultsTab.Visible = false;
			this.TabValidationResults.Controls.Add(this.PanelResultsManagerTiltle);
			this.TabValidationResults.Controls.Add(this.TabControlIssues);
			
			if (_TCM_CountForControlsChange == 0) 
			{
				_TCM_UpdateCount++;

				DvtkApplicationLayer.Result result = userControlSessionTree.GetSelectedTag() as DvtkApplicationLayer.Result;
				if (result != null) 
				{
					String selectedText = GetSelectedUserNode().Text;
					string theHtmlFileNameOnly = "";
					if (selectedText.StartsWith("Detail_")) 
					{
						if(selectedText.IndexOf(result.DetailFile) != -1)
						{
                            if (selectedText.IndexOf(result.DetailFile) != -1)
							{
                                theHtmlFileNameOnly = result.DetailFile.ToLower().Replace(".xml", ".html");
							}
							else
							{
								foreach ( string resultName in result.SubDetailResultFiles)
								{
									if(resultName != "")
									{
										if(selectedText.IndexOf(resultName) != -1)
										{
											theHtmlFileNameOnly = resultName.ToLower().Replace(".xml", ".html");
											break;
										}
									}
								}
							}
						}
						else
						{
							foreach ( string resultName in result.SubDetailResultFiles)
							{
								if(selectedText.IndexOf(resultName) != -1)
								{
									theHtmlFileNameOnly = resultName.ToLower().Replace(".xml", ".html");
									break;
								}
							}
						}
					}
					else
					{
						if(selectedText.IndexOf(result.SummaryFile) != -1)
						{
                            if (selectedText.IndexOf(result.SummaryFile) != -1)
							{
                                theHtmlFileNameOnly = result.SummaryFile.ToLower().Replace(".xml", ".html");
							}
							else
							{
								foreach ( string resultName in result.SubSummaryResultFiles)
								{
									if(resultName != "")
									{
										if(selectedText.IndexOf(resultName) != -1)
										{
											theHtmlFileNameOnly = resultName.ToLower().Replace(".xml", ".html");
											break;
										}
									}
								}
							}
						}
						else
						{
							foreach ( string resultName in result.SubSummaryResultFiles)
							{
								if(selectedText.IndexOf(resultName) != -1)
								{
									theHtmlFileNameOnly = resultName.ToLower().Replace(".xml", ".html");
									break;
								}
							}
						}
					}					
					
					string theHtmlFullFileName = Path.Combine(result.ParentSession.ResultsRootDirectory, theHtmlFileNameOnly);
					string theXmlFullFileName = Path.Combine(result.ParentSession.ResultsRootDirectory, theHtmlFileNameOnly.Replace(".html" , ".xml"));

					// Show the HTML file.
					// The actual conversion from XML to HTML will be performed in the WebDescriptionView_BeforeNavigate2 method.
                    webBrowserValResult.Dispose();
                    this.webBrowserValResult = new System.Windows.Forms.WebBrowser();
                    this.webBrowserValResult.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.webBrowserValResult.Location = new System.Drawing.Point(0, 0);
                    this.webBrowserValResult.MinimumSize = new System.Drawing.Size(20, 20);
                    this.webBrowserValResult.Name = "webBrowserValResult";
                    this.webBrowserValResult.Size = new System.Drawing.Size(749, 669);
                    this.webBrowserValResult.TabIndex = 0;
                    this.webBrowserValResult.CanGoForwardChanged += new System.EventHandler(this.webBrowserValResult_CanGoForwardChanged);
                    this.webBrowserValResult.CanGoBackChanged += new System.EventHandler(this.webBrowserValResult_CanGoBackChanged);
                    this.webBrowserValResult.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserValResult_Navigating);
                    this.webBrowserValResult.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserValResult_DocumentCompleted);
                    this.webBrowserValResult.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserValResult_Navigated);

                    this.WebBrowserPanel.Controls.Add(this.webBrowserValResult);

                    _TCM_ValidationResultsBrowser = new ValidationResultsManager(webBrowserValResult);
                    //GC.Collect();
                    _TCM_ValidationResultsBrowser.ShowHtml(theXmlFullFileName);
				}

				_TCM_UpdateCount--;
			}            
		}

		public void FilteringResults()
		{
			if (TabControl.SelectedTab == this.TabValidationResults)
			{	
				Session session = GetSession();
				TreeNode selectedNode = GetSelectedUserNode();
				string resultFilename = selectedNode.Text;
				// Do the actual conversion from XML to HTML.
                _TCM_ValidationResultsBrowser.ConvertXmlToHtml(session.ResultsRootDirectory, resultFilename, resultFilename.ToLower().Replace(".xml", ".html"), filterIndicator, resultProjectXml);	
				string _HtmlFullFileNameToShow = session.ResultsRootDirectory + resultFilename.ToLower().Replace(".xml", ".html");
                _TCM_ValidationResultsBrowser.ShowHtml(_HtmlFullFileNameToShow);
			}
			else if (TabControl.SelectedTab == this.TabResultsManager)
			{
				TCM_UpdateTabResultsManager();
			}
			else 
			{
			}
			this.checkBoxIgnoreResult.Checked = false ;
			this.checkBoxIgnoreResultAll.Checked = false;
			this.TextBoxComment.Text = "";
			this.TextBoxPr.Text = "";
		}

		public void TCM_UpdateTabResultsManager() 
		{			
			this.tabControl1.Controls.Add(this.AddIssue);
			this.TabResultsManager.Controls.Add(this.PanelResultsManagerTiltle);
			this.TabResultsManager.Controls.Add(this.TabControlIssues);
			this.PictureBoxMaximizeResultTab.Visible = false ;
			this.PictureBoxMinResultsTab.Visible = true;
			this.TabControlIssues.Visible = true ;

			if (_TCM_CountForControlsChange == 0) 
			{
				_TCM_UpdateCount++;
				FileInfo file = new FileInfo(projectApp.ProjectFileName);
				string projectStructureFile = Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + ".xml");
				//string projectStructureFile = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + ".xml";
				string finalHtmlFile = projectStructureFile.ToLower().Replace(".xml", ".html");
				try 
				{
					ProjectXmlFile(projectStructureFile);
					string tempFile1 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "1.xml";
					string tempFile2 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "2.xml";
					string tempFile3 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "3.xml";
					if (!_MainForm.ToolBarFilterResults.Pushed)
					{
						if(projectApp.Sessions.Count != 0)
						{
							Convert(Path.GetFileName(projectStructureFile), tempFile1, "createsum.xsl");
							Convert(tempFile1,tempFile2,"sortontype.xsl");
							Convert(tempFile2,tempFile3, "sortonmes.xsl");
							Convert(tempFile3,  finalHtmlFile, "viewonmes.xsl");
						}
					}
					else 
					{	
						FileInfo fileInfo = new FileInfo(resultProjectXml);
						if (fileInfo.Exists)
						{
							XmlDocument xmldoc= null;
							try 
							{
								xmldoc = new XmlDocument();
								xmldoc.Load(fileInfo.FullName);
							}
							catch(Exception e)
							{
								MessageBox.Show(e.Message+ "-----"+ xmldoc.ToString());
								CreateXmlTemplate();
							}
						}
						if ( !fileInfo.Exists)
						{	
							CreateXmlTemplate();
						}
						ProjectXmlFile(projectStructureFile);
						if(projectApp.Sessions.Count != 0)
						{
							Convert(Path.GetFileName(projectStructureFile), tempFile1, "createsum.xsl");
							Convert(tempFile1,tempFile2,"sortontype.xsl");
							Convert(tempFile2,tempFile3, "sortonmes.xsl");
							Convert(tempFile3,  finalHtmlFile, "viewonmesfiltered.xsl");
						}
					}
                    webBrowserResultMgr.Navigate(finalHtmlFile);
				}
				catch (Exception e) 
				{
					MessageBox.Show(e.Message);
				}
				                       
				_TCM_UpdateCount--;
			}
		}

        /// <summary>
		/// This method is used to convert xml to another xml using stylesheet
		/// </summary>
		/// <param name="inputFile"> name of the xml file which is used as an input </param>
		/// <param name="outputFile">name of the xml file which is the output file</param>
		/// <param name="styleSheet">name of the stylesheet</param>
		private static void Convert(string inputFile , string outputFile , string styleSheet) 
		{
			string fullInputFileName = Path.Combine(Path.GetDirectoryName(projectApp.ProjectFileName), inputFile);
			string fullOutputFileName = Path.Combine(Path.GetDirectoryName(projectApp.ProjectFileName), outputFile);
			string styleSheetName = Path.Combine(Application.StartupPath, styleSheet);

            XslCompiledTransform xslt = new XslCompiledTransform();
            XsltSettings settings = new XsltSettings();
            settings.EnableDocumentFunction = true;
            settings.EnableScript = true;
            
            // Create an XmlUrlResolver with default credentials.
            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

            xslt.Load(styleSheetName, settings, resolver);
            XmlReader reader = XmlReader.Create(fullInputFileName);
            
            StreamWriter writer = File.CreateText(fullOutputFileName);

            xslt.Transform(reader, null, writer);
            
            writer.Flush();
            writer.Close();
            reader.Close();
		}

		private void TabControl_SelectedIndexChanged(object sender, System.EventArgs e) 
		{
			if (_TCM_UpdateCount == 0) 
			{
				_SopClassesManager.RemoveDynamicDataBinding();

				TCM_UpdateTabContents();

				Notify(new SelectedTabChangeEvent());
			}
		}

		private void TextBoxSessionTitle_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				GetSession().SessionTitle = TextBoxSessionTitle.Text;

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}
		}

		private void SessionTreeView_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e) 
		{
			// Workaround, to make sure that the NumericUpDown controls _Leave method is called
			// (when selected) before a new session is selected in the session tree.
			if (TabControl.SelectedTab == TabSessionInformation) 
			{
				//TextBoxSessionTitle.Focus();
				//TextBoxDVTAETitle.Focus();
				//TextBoxSUTAETitle.Focus();
				TabControl.SelectedTab.Focus();                
			}
		}

		private void TextBoxTestedBy_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				GetSession().TestedBy  = TextBoxTestedBy.Text;

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}
		}

		private void TextBoxDVTAETitle_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).DvtAeTitle = TextBoxDVTAETitle.Text;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).DvtAeTitle = TextBoxDVTAETitle.Text;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}
		}

		private void TextBoxDVTImplClassUID_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).DvtImplementationClassUid = TextBoxDVTImplClassUID.Text;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).DvtImplementationClassUid = TextBoxDVTImplClassUID.Text;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}
		}

		private void TextBoxDVTImplVersionName_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).DvtImplementationVersionName = TextBoxDVTImplVersionName.Text;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).DvtImplementationVersionName = TextBoxDVTImplVersionName.Text;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}				
		}

		private void TextBoxSUTAETitle_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutAeTitle = TextBoxSUTAETitle.Text;;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutAeTitle = TextBoxSUTAETitle.Text;;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}							
		}

		private void TextBoxSUTImplClassUID_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutImplementationClassUid = TextBoxSUTImplClassUID.Text;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutImplementationClassUid = TextBoxSUTImplClassUID.Text;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}						
		}

		private void TextBoxSUTImplVersionName_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutImplementationVersionName = TextBoxSUTImplVersionName.Text;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutImplementationVersionName = TextBoxSUTImplVersionName.Text;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}											
		}

        private void NumericSUTListenPort_ValueChanged(object sender, System.EventArgs e) 
		{
			// touch it to repair known MS bug
			object obj = NumericSUTListenPort.Value;
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutPort = (ushort)NumericSUTListenPort.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutPort = (ushort)NumericSUTListenPort.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}													
		}

		private void TextBoxSUTTCPIPAddress_TextChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutHostName = TextBoxSUTTCPIPAddress.Text;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutHostName = TextBoxSUTTCPIPAddress.Text;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}													
		}

        private void NumericSUTMaxPDU_ValueChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
            if (NumericSUTMaxPDU.Value > 0 && NumericSUTMaxPDU.Value < 512)
            {
                MessageBox.Show("Numeric value cannot be less than 512 except zero.");
                NumericSUTMaxPDU.Value = 512;
            }

			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutMaximumLengthReceived = (uint)NumericSUTMaxPDU.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutMaximumLengthReceived = (uint)NumericSUTMaxPDU.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}																
		}

		private void PanelSecuritySettingsContent_Click(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		private void PanelSUTSettingContent_Click(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		private void PanelDVTRoleSettingsContent_Click(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		private void panel1_Click(object sender, System.EventArgs e)
		{
			this.Focus();		
		}

		private void CheckBoxDisplayCondition_CheckedChanged(object sender, System.EventArgs e)
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				// Update the session member.
				GetSession().DisplayConditionText = CheckBoxDisplayCondition.Checked;
				// Notify the rest of the world that the session has been changed.
				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}
		}

		private void TextBoxAddMessageText_TextChanged(object sender, System.EventArgs e)
		{
			string text = this.TextBoxAddMessageText.Text ;
			if(text.IndexOf("#") >0)
			{
				MessageBox.Show("Not Allowed to add Text Message with  '#' in it .Please add another Text Message.", "Warning");
				this.TextBoxAddMessageText.Text = "";
			}
		}		

		private void NumericSUTMaxPDU_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.

			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutMaximumLengthReceived = (uint)NumericSUTMaxPDU.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutMaximumLengthReceived = (uint)NumericSUTMaxPDU.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}			
		}

		private void NumericSUTListenPort_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// touch it to repair known MS bug
			object obj = NumericSUTListenPort.Value;
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).SutPort = (ushort)NumericSUTListenPort.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).SutPort = (ushort)NumericSUTListenPort.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
		}

		private void NumericDVTMaxPDU_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
                    (session as DvtkApplicationLayer.ScriptSession).DvtMaximumLengthReceived = (uint)NumericDVTMaxPDU.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
                    (session as DvtkApplicationLayer.EmulatorSession).DvtMaximumLengthReceived = (uint)NumericDVTMaxPDU.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
		}

		private void NumericDVTTimeOut_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).DvtSocketTimeout = (ushort)NumericDVTTimeOut.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).DvtSocketTimeout = (ushort)NumericDVTTimeOut.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
		}

        private void NumericDVTTimeOut_ValueChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).DvtSocketTimeout = (ushort)NumericDVTTimeOut.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
					(session as DvtkApplicationLayer.EmulatorSession).DvtSocketTimeout = (ushort)NumericDVTTimeOut.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}						
		}

        private void NumericDVTMaxPDU_ValueChanged(object sender, System.EventArgs e) 
		{
		    if(NumericDVTMaxPDU.Value >0 && NumericDVTMaxPDU.Value < 512)
			{
				MessageBox.Show("Numeric value cannot be less than 512 except zero.");
				NumericDVTMaxPDU.Value = 512 ;
			}
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
					(session as DvtkApplicationLayer.ScriptSession).DvtMaximumLengthReceived = (uint)NumericDVTMaxPDU.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
                    (session as DvtkApplicationLayer.EmulatorSession).DvtMaximumLengthReceived = (uint)NumericDVTMaxPDU.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}				
		}

        private void CheckBoxGenerateDetailedValidationResults_CheckedChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				// Update the session member.
				GetSession().DetailedValidationResults = CheckBoxGenerateDetailedValidationResults.Checked;
				// Notify the rest of the world that the session has been changed.
				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}
		}

		public void TCM_AppendTextToActivityLogging(string theText) 
		{
			// More then one thread may be writting indirectly to the activity logging tab,
			// when a Visual Basic Script is executed.

			if (RichTextBoxActivityLogging.InvokeRequired) 
			{
				RichTextBoxActivityLogging.Invoke(_TCM_AppendTextToActivityLogging_ThreadSafe_Delegate, new object[] {theText});
			}
			else 
			{
				TCM_AppendTextToActivityLogging_ThreadSafe(theText);
			}
		}

		private void TCM_AppendTextToActivityLogging_ThreadSafe(string theText) 
		{
			RichTextBoxActivityLogging.AppendText(theText);
			// ScrollToCaret will work, regardless whether the RichTextControl has the focus or not,
			// because the HideSelection property is set to false.
			RichTextBoxActivityLogging.ScrollToCaret();
		}

		public void TCM_ClearActivityLogging() 
		{
			RichTextBoxActivityLogging.Clear();
		}

		private void TCM_OnActivityReportEvent(object sender, Dvtk.Events.ActivityReportEventArgs theArgs) 
		{
			TCM_AppendTextToActivityLogging(theArgs.Message + '\n');
		}

		public bool IsActive() 
		{
			return(ParentForm.ActiveMdiChild == this);
		}		

		private void ButtonBrowseResultsDir_Click(object sender, System.EventArgs e) 
		{			
			TheFolderBrowserDialog.Description = "Select the root directory where the result files should be stored.";

			// Only if the current directory exists, set this directory in the dialog browser.
			if (TextBoxResultsRoot.Text != "") 
			{
				DirectoryInfo theDirectoryInfo = new DirectoryInfo(TextBoxResultsRoot.Text);

				if (theDirectoryInfo.Exists) 
				{
					TheFolderBrowserDialog.SelectedPath = TextBoxResultsRoot.Text;
				}
			}

			if (TheFolderBrowserDialog.ShowDialog (this) == DialogResult.OK) 
			{
				int index = TheFolderBrowserDialog.SelectedPath.IndexOf("#");
				if (index < 0)
				{
					// MK!!! TextBoxResultsRoot.Text = TheFolderBrowserDialog.SelectedPath;
					if (GetSession().ResultsRootDirectory != TheFolderBrowserDialog.SelectedPath) 
					{
                        
                        GetSession().ResultsRootDirectory = TheFolderBrowserDialog.SelectedPath;
                        
						// Notify the rest of the world of the change.
						SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.RESULTS_DIR);
						Notify(theSessionChange);
					}
				}
				else
				{
					MessageBox.Show("Result Directory with '#' in it is not allowed . Please select another result directory.", "Warning");
				}
			}
		}

		private void ButtonBrowseScriptsDir_Click(object sender, System.EventArgs e) 
		{	
			DvtkApplicationLayer.ScriptSession theScriptSession = null;

			if (GetSelectedSessionNew() is DvtkApplicationLayer.ScriptSession) 
			{
				theScriptSession = (DvtkApplicationLayer.ScriptSession)GetSelectedSessionNew();
			}
			else 
			{
				// Not supposed to get here.
				throw new System.ApplicationException("Error: not expected to get here.");
			}

			TheFolderBrowserDialog.Description = "Select the root directory containing the script files.";

			// Only if the current directory exists, set this directory in the dialog browser.
			if (TextBoxScriptRoot.Text != "") 
			{
				DirectoryInfo theDirectoryInfo = new DirectoryInfo(TextBoxScriptRoot.Text);

				if (theDirectoryInfo.Exists) 
				{
					TheFolderBrowserDialog.SelectedPath = TextBoxScriptRoot.Text;
				}
			}

			if (TheFolderBrowserDialog.ShowDialog (this) == DialogResult.OK) 
			{
				//MK!! TextBoxScriptRoot.Text = TheFolderBrowserDialog.SelectedPath;
				if (theScriptSession.DicomScriptRootDirectory != TheFolderBrowserDialog.SelectedPath) 
				{
					theScriptSession.DicomScriptRootDirectory = TheFolderBrowserDialog.SelectedPath;

					// Notify the rest of the world of the change.
					SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.SCRIPTS_DIR);
					Notify(theSessionChange);
				}
			}		
		}

		private void ButtonBrowseDescriptionDir_Click(object sender, System.EventArgs e) 
		{
			DvtkApplicationLayer.ScriptSession theScriptSession = null;

			if (GetSelectedSessionNew() is DvtkApplicationLayer.ScriptSession) 
			{
				theScriptSession = (DvtkApplicationLayer.ScriptSession)GetSelectedSessionNew();
			}
			else 
			{
				// Not supposed to get here.
				throw new System.ApplicationException("Error: not expected to get here.");
			}

			TheFolderBrowserDialog.Description = "Select the root directory containing the description (html) files.";

			// Only if the current directory exists, set this directory in the dialog browser.
			if (TextBoxDescriptionRoot.Text != "") 
			{
				DirectoryInfo theDirectoryInfo = new DirectoryInfo(TextBoxDescriptionRoot.Text);

				if (theDirectoryInfo.Exists) 
				{
					TheFolderBrowserDialog.SelectedPath = TextBoxDescriptionRoot.Text;
				}
			}

			if (TheFolderBrowserDialog.ShowDialog (this) == DialogResult.OK) 
			{
				//MK!! TextBoxDescriptionRoot.Text = TheFolderBrowserDialog.SelectedPath;
				if (theScriptSession.DescriptionDirectory != TheFolderBrowserDialog.SelectedPath) 
				{
					theScriptSession.DescriptionDirectory = TheFolderBrowserDialog.SelectedPath;

					// Notify the rest of the world of the change.
                   
					SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.DESCRIPTION_DIR);
					Notify(theSessionChange);
				}
			}				
		}

		private void ButtonSpecifySopClassesAddDirectory_Click(object sender, System.EventArgs e) 
		{
			_SopClassesManager.AddDefinitionFileDirectory();
		}

		private void ButtonSpecifySopClassesRemoveDirectory_Click(object sender, System.EventArgs e) 
		{
			_SopClassesManager.RemoveDefinitionFileDirectory();
		}

		private void ComboBoxSpecifySopClassesAeTitle_SelectedIndexChanged(object sender, System.EventArgs e) 
		{
			_SopClassesManager.SelectedAeTitleVersionChanged();
		}

		private void DataGridSpecifySopClasses_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) 
		{
			_SopClassesManager.DataGrid_MouseDown(e);
		}

		private void DataGridSpecifySopClasses_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) 
		{
			_SopClassesManager.DataGrid_MouseMove(e);		
		}

		private void DataGridSpecifySopClasses_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) 
		{
			_SopClassesManager.DataGrid_MouseUp(e);
		}
        
		/// <summary>
		/// Get the selected session of the session tree view of the project form.
		/// </summary>
		/// <returns>Selected session.</returns>
		public DvtkApplicationLayer.Session GetSelectedSessionNew()
		{
			return userControlSessionTree.GetSelectedSessionNew();
		}
		public TreeNode GetSelectedUserNode() 
		{
			return userControlSessionTree.GetSelectedUserNode();
		}
		public UserControlSessionTree GetSessionTreeViewManager() 
		{
			return(userControlSessionTree);
		}
		/// <summary>
		/// Get the selected session of the session tree view of the project form.
		/// </summary>
		/// <returns>Selected session.</returns>
		public DvtkApplicationLayer.Session GetSession() 
		{
			return userControlSessionTree.GetSession();
		}
		/// <summary>
		/// Get the selected tree node object of the session tree view of the project form.
		/// </summary>
		/// <returns>Selected tree object.</returns>
		public Object GetSelectedTag() 
		{
			return userControlSessionTree.GetSelectedTag();
		}

		/// <summary>
		/// Get the selected tree node tag of the session tree view of the project form.
		/// </summary>
		/// <returns>Selected tree node tag.</returns>		
		private void ProjectForm2_Activated(object sender, System.EventArgs e) 
		{
			this.Focus();
			Notify(new ProjectFormGetsFocusEvent());		
		}

        private void ButtonSpecifyTransferSyntaxes_Click(object sender, System.EventArgs e) 
		{
			DvtkApplicationLayer.EmulatorSession session = (DvtkApplicationLayer.EmulatorSession)userControlSessionTree.GetSelectedSessionNew();
			
			SelectTransferSyntaxesForm  theSelectTransferSyntaxesForm = new SelectTransferSyntaxesForm();

			ArrayList tsList = new ArrayList();
			tsList.Add (DvtkData.Dul.TransferSyntax.Implicit_VR_Little_Endian);
			tsList.Add (DvtkData.Dul.TransferSyntax.Explicit_VR_Big_Endian);
			tsList.Add (DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Baseline_Process_1);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Extended_Hierarchical_16_And_18);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Extended_Hierarchical_17_And_19);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Extended_Process_2_And_4);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Extended_Process_3_And_5);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Hierarchical_24_And_26);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Hierarchical_25_And_27);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Non_Hierarchical_10_And_12);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Full_Progression_Non_Hierarchical_11_And_13);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Lossless_Hierarchical_28);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Lossless_Hierarchical_29);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Lossless_Non_Hierarchical_14);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Lossless_Non_Hierarchical_15);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Lossless_Non_Hierarchical_1st_Order_Prediction);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_LS_Lossless_Image_Compression);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_LS_Lossy_Image_Compression);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_2000_IC_Lossless_Only);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_2000_IC);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Hierarchical_20_And_22);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Hierarchical_21_And_23);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Non_Hierarchical_6_And_8);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_Spectral_Selection_Non_Hierarchical_7_And_9);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_2000_Multicomponent_lossless2);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPEG_2000_Multicomponent2);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPIP_Referenced);
			tsList.Add (DvtkData.Dul.TransferSyntax.JPIP_Referenced_Deflate);
			tsList.Add (DvtkData.Dul.TransferSyntax.MPEG2_Main_Profile_Level);
			tsList.Add (DvtkData.Dul.TransferSyntax.RFC_2557_Mime_Encapsulation);
			tsList.Add (DvtkData.Dul.TransferSyntax.Deflated_Explicit_VR_Little_Endian);
			tsList.Add (DvtkData.Dul.TransferSyntax.RLE_Lossless);

			foreach (DvtkData.Dul.TransferSyntax ts in session.EmulatorSessionImplementation.SupportedTransferSyntaxSettings.SupportedTransferSyntaxes)
			{
				theSelectTransferSyntaxesForm.SupportedTransferSyntaxes.Add(ts);
			}

			theSelectTransferSyntaxesForm.DefaultTransferSyntaxesList = tsList;
			theSelectTransferSyntaxesForm.DisplaySelectAllButton = true;
			theSelectTransferSyntaxesForm.DisplayDeSelectAllButton = true;

			theSelectTransferSyntaxesForm.selectSupportedTS();
			theSelectTransferSyntaxesForm.ShowDialog (this);

			if(theSelectTransferSyntaxesForm.SupportedTransferSyntaxes.Count == 0)
			{
				session.EmulatorSessionImplementation.SupportedTransferSyntaxSettings.SupportedTransferSyntaxes.Clear();

				string theWarningText = "No Transfer Syntax is selected, default ILE will be supported.";
				MessageBox.Show(theWarningText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				session.EmulatorSessionImplementation.SupportedTransferSyntaxSettings.SupportedTransferSyntaxes.Add(new DvtkData.Dul.TransferSyntax( "1.2.840.10008.1.2" ));
			}
			else
			{
				session.EmulatorSessionImplementation.SupportedTransferSyntaxSettings.SupportedTransferSyntaxes.Clear();

				foreach (DvtkData.Dul.TransferSyntax ts in theSelectTransferSyntaxesForm.SupportedTransferSyntaxes)
				{
					session.EmulatorSessionImplementation.SupportedTransferSyntaxSettings.SupportedTransferSyntaxes.Add(ts);
				}
			}

			if (theSelectTransferSyntaxesForm._SelectionChanged) 
			{
				Notify(new SessionChange(userControlSessionTree.GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER));
			}
		}
      
		/// <summary>
		/// This method is called before the Web control will navigate to a HTML page.
		/// In this method, the conversion from XML to HTML will be performed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void webBrowserValResult_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {           			
            try
			{                
                Session tempSession = this.GetSession();
                
                int tempSubIndex; 
                bool _IsSubIndexPage=false;             
                GC.Collect();
				this.TextBoxPr.Text = "";
				this.TextBoxComment.Text = "";
                this.LabelErrorMessage.Text = "";
				this.checkBoxIgnoreResult.Checked = false ;
				if (!_MainForm.ToolBarFilterResults.Pushed)
				{
					_MainForm.MainStatusBar.Text = "";
				}
                                            
                theFullFileName = _TCM_ValidationResultsBrowser.GetFullFileNameFromHtmlLink(e.Url.ToString());

                //Check whether the page being navigated is the Details page
                if (theFullFileName.Contains("Detail"))
                {
                    string tempProperFileName = theFullFileName;

                    if (tempProperFileName.IndexOf(@"\") != -1)
                    {
                        tempProperFileName = tempProperFileName.Replace(@"\", @"/");
                    }

                    if (tempProperFileName.Contains(".xml"))
                    {
                        tempSubIndex = tempProperFileName.IndexOf(".xml");

                        //Check whether the sub-index in the detailed results page was clicked
                        if (char.IsDigit(tempProperFileName[tempSubIndex - 1]))
                        {
                            _IsSubIndexPage = true;
                        }
                    }

                    if (_IsSubIndexClicked && !_IsSubIndexPage)
                    {
                        theFullFileName = subIndexFileName;
                    }

                    if (_IsSubIndexPage)
                    {
                        _IsSubIndexClicked = true;
                        subIndexFileName = tempProperFileName;
                        subIndexUri = new Uri(e.Url.ToString());
                    }
                }
                else
                {
                    _IsSubIndexClicked = false;
                }                             
                
                if (theFullFileName.IndexOf(@"/") != -1)
                    theFullFileName = theFullFileName.Replace(@"/", @"\");

				resultFileNameForAddIssue = theFullFileName;
				string theDirectory = Path.GetDirectoryName(theFullFileName);
				string theFileNameOnly = Path.GetFileName(theFullFileName);

				int indexResultFile = theFullFileName.IndexOf("*",0);
				if (indexResultFile != -1) 
				{	
					if (theFullFileName.StartsWith(@"C:\ADD") || theFullFileName.StartsWith(@"C:\DEL"))
					{
						this.TabControlIssues.SelectedTab = this.AddIssue;
						this.TabControlIssues.Visible = true;
						this.PictureBoxMaximizeResultTab.Visible = false;
						this.PictureBoxMinResultsTab.Visible = true;
						resultFileNameForAddIssue = theFullFileName;
						name = resultFileNameForAddIssue.Substring(indexResultFile+1).Trim();
						string temptheFullFileName = resultFileNameForAddIssue.Substring(indexResultFile+1);
						if (temptheFullFileName.IndexOf("*",0) != -1)
							temptheFullFileName = temptheFullFileName.Substring(0 , temptheFullFileName.IndexOf("*",0));
						theFileNameOnly = temptheFullFileName.Trim();
                        if (theFullFileName.StartsWith(@"C:\DEL"))
						{
							DeleteAddIssue(name);												
						}

                        if (theFullFileName.StartsWith(@"C:\ADD"))
						{
							this.TabControlIssues.SelectedTab = this.RemoveIssue;
							this.TabControlIssues.Visible = true;
							FileInfo tempFileInfo = new FileInfo(resultProjectXml);
							if (tempFileInfo.Exists)
							{
								XmlDocument xmldoc = new XmlDocument();
								xmldoc.Load(tempFileInfo.FullName);
								XmlNode root = xmldoc.DocumentElement;
								XmlTextReader tempReader = new XmlTextReader(tempFileInfo.FullName);
								XmlNodeList nodeToRead = null ;
								nodeToRead= root.SelectNodes("/Nodes//Name");
								if (nodeToRead.Count >0 )
								{
									foreach (XmlNode nodeRead in nodeToRead)
									{	
										string tempText = nodeRead.InnerText ;
										if (tempText.ToLower().Equals(name.ToLower()))
										{
											this.checkBoxIgnoreResultAll.Enabled = false;
											int index = tempText.IndexOf("*");
											string temp = tempText.Substring(index + 1);
											fullResultFileName = tempText.Substring(0,index);
											this.TextBoxComment.Visible = true ;
											this.TextBoxPr.Visible = true ;
											this.LabelErrorMessage.Text = tempText ;
											this.TextBoxComment.Text = nodeRead.NextSibling.InnerText;
											this.TextBoxPr.Text = nodeRead.NextSibling.NextSibling.InnerText;
											if (nodeRead.ParentNode.ParentNode.Name == "skipped")
											{							
												this.checkBoxIgnoreResult.Checked = true ;
											}
											else if (nodeRead.ParentNode.ParentNode.Name == "Problem")
											{
												this.checkBoxIgnoreResult.Checked = false ;
											}
											else
											{
											}
										}																															
									}						
								}
							}
						}
					}				
					else 
					{					
						name = "";
						this.checkBoxIgnoreResultAll.Enabled = true;
						this.checkBoxIgnoreResultAll.Checked = false;
						this.checkBoxIgnoreResult.Enabled = true;
						this.checkBoxIgnoreResult.Checked = false;
						this.TabControlIssues.SelectedTab = this.RemoveIssue ;
						this.TabControlIssues.Visible = true;
						this.PictureBoxMaximizeResultTab.Visible = false;
						this.PictureBoxMinResultsTab.Visible = true;
						fullResultFileName = theFullFileName ;
						string indexStr = fullResultFileName.Substring(0,indexResultFile);
						int tempIndex = indexStr.LastIndexOf(@"\");
						messageIndex = indexStr.Substring(tempIndex + 1).Trim();
						fullResultFileName = fullResultFileName.Substring(indexResultFile + 1).Trim();
						theFileNameOnly = fullResultFileName ;
						fullResultFileName = Path.Combine(theDirectory ,fullResultFileName);
						CreatingProjectXmlFile(fullResultFileName , messageIndex, false);
						this.LabelErrorMessage.Text = name ;
						ReadProjectResultManagerXml(false );
					}

					if(this.TabControlIssues.SelectedTab.Name == "AddIssue")
					{
					}
					else if ((TabControl.SelectedTab == TabResultsManager) && (this.TabControlIssues.SelectedTab.Name =="RemoveIssue"))
					{
						ReadProjectValidationTabXml();
					}
					else
					{
					}
					e.Cancel = true ;
				}					
				else 
				{
                    if (e.Url.ToString().ToLower().IndexOf(".xml") != -1) 
					{
						// The user has selected a results file tag or has pressed a link in a HTML file.
						// Convert the XML file to HTML, cancel this request and request viewing of the generated HTML file.
						// Cancel it. We want to show the generated HTML file.
                        // As a result of calling _TCM_ValidationResultsBrowser.ShowHtml(e.uRL.ToString()), this method will
						// be called again.
						e.Cancel = true;

						string theHtmlFileNameOnly = theFileNameOnly.ToLower().Replace(".xml", ".html");
						string theHtmlFullFileName = Path.Combine(theDirectory, theHtmlFileNameOnly);
				
						// Do the actual conversion from XML to HTML.
                        _TCM_ValidationResultsBrowser.ConvertXmlToHtml(theDirectory, theFileNameOnly, theHtmlFileNameOnly, filterIndicator, resultProjectXml);

                        //Check whether the user is navigating back to the Details Tab after having clicked on the Sub Index and then toggling tabs
                        if (_IsSubIndexClicked  && !_IsSubIndexPage )
                        {
                            _HtmlFullFileNameToShow = subIndexUri.ToString().ToLower().Replace(".xml", ".html");
                           
                        }                                             

                        else
                        {
                            _HtmlFullFileNameToShow = e.Url.ToString().ToLower().Replace(".xml", ".html");
                        }

                        _TCM_ValidationResultsBrowser.ShowHtml(_HtmlFullFileNameToShow);
					}
					else 
					{
						// Do nothing, This is a HTML file and will be automatically shown.				
					}            
				}				
			}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}

        void webBrowserValResult_CanGoForwardChanged(object sender, EventArgs e)
        {
            _TCM_ValidationResultsBrowser.ForwardEnabled();
        }

        void webBrowserValResult_CanGoBackChanged(object sender, EventArgs e)
        {
            _TCM_ValidationResultsBrowser.BackEnabled();
        }

        void webBrowserResultMgr_CanGoBackChanged(object sender, EventArgs e)
        {
            //_TCM_ValidationResultsManagerBrowser.BackEnabled();
            if (webBrowserResultMgr.CanGoBack)
                webBrowserResultMgr.GoBack();
        }

        void webBrowserResultMgr_CanGoForwardChanged(object sender, EventArgs e)
        {
            //_TCM_ValidationResultsManagerBrowser.ForwardEnabled();
            if (webBrowserResultMgr.CanGoForward)
                webBrowserResultMgr.GoForward();
        }

        void webBrowserScript_CanGoBackChanged(object sender, EventArgs e)
        {
            //_TCM_ValidationResultsManager.BackEnabled();
            /*if (webBrowserScript.CanGoBack)
                webBrowserScript.GoBack();*/
        }

        void webBrowserScript_CanGoForwardChanged(object sender, EventArgs e)
        {
            //_TCM_ValidationResultsManager.ForwardEnabled();
            /*if (webBrowserScript.CanGoForward)
                webBrowserScript.GoForward();*/
        }

        private void SetProperties_webBrowserScript()
        {
            this.webBrowserScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserScript.Location = new System.Drawing.Point(0, 0);
            this.webBrowserScript.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserScript.Name = "webBrowserScript";
            this.webBrowserScript.Size = new System.Drawing.Size(779, 699);
            this.webBrowserScript.TabIndex = 1;
            this.webBrowserScript.CanGoForwardChanged += new System.EventHandler(this.webBrowserScript_CanGoForwardChanged);
            this.webBrowserScript.CanGoBackChanged += new System.EventHandler(this.webBrowserScript_CanGoBackChanged);
            this.webBrowserScript.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserScript_Navigating);
            this.webBrowserScript.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserScript_DocumentCompleted);
            this.webBrowserScript.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserScript_Navigated);

        }
        private void webBrowserScript_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string s = e.Url.ToString();
            string directory = Path.GetDirectoryName(s);
            directory = directory + @"\";
            if (directory.StartsWith("file:\\"))
            {
                directory = directory.Remove(0, "file:\\".Length);
            }

            string resultFileName = Path.GetFileName(s);
            int index = resultFileName.IndexOf(".");
            resultFileName = resultFileName.Substring(0, index);
            /* Super scripts */
            int extIndex = 0;
            string resultFileWithExt = null;
            extIndex = resultFileName.IndexOf("_dss");
            if (extIndex == -1) /* Script */
            {
                extIndex = resultFileName.IndexOf("_ds");
                resultFileWithExt = resultFileName.Substring(0, extIndex) + ".ds";
            }
            else
            {
                resultFileWithExt = resultFileName.Substring(0, extIndex)+ ".dss";
            }
            
            //resultFileName = resultFileName.Replace(".html", ".xml");
            //userControlSessionTree.SearchAndSelectHTMLNode(directory, resultFileWithExt);


        }
        private void webBrowserValResult_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            _TCM_ValidationResultsBrowser.NavigateComplete2Handler();
            Notify(new WebNavigationComplete());
        }

		MainForm GetMainForm() 
		{
			MainForm theMainForm = null;

			theMainForm = ParentForm as MainForm;

			if (theMainForm == null) 
			{
				// Sanity check.
				Debug.Assert(false);
			}

			return theMainForm;
		}

		#region Context Menu

		private void SessionTreeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) 
		{
			if (e.Button == MouseButtons.Right) 
			{
				// Right button has been pressed.
				// Make the node that is below the mouse the selected one.
				//!!!SessionTreeView.SelectedNode = SessionTreeView.GetNodeAt(e.X, e.Y);
				Refresh();
			}
		}

		private void ContextMenu_AddExistingSession_Click(object sender, System.EventArgs e) 
		{
			GetMainForm().AddExistingSessions();
		}

		private void ContextMenu_AddNewSession_Click(object sender, System.EventArgs e) 
		{
			GetMainForm().AddNewSession();
		}

		private void ContextMenu_Edit_Click(object sender, System.EventArgs e) 
		{
			userControlSessionTree.EditSelectedScriptFile();
		}

		private void ContextMenu_Execute_Click(object sender, System.EventArgs e) 
		{
			userControlSessionTree.Execute();
		}

		private void ContextMenu_Remove_Click(object sender, System.EventArgs e) 
		{
			DvtkApplicationLayer.Result resultfiletag = (DvtkApplicationLayer.Result)GetSelectedTag();
			ArrayList theResultsFilesToRemove = new ArrayList();
			if (resultfiletag == null) 
			{
				// Sanity check.
				System.Diagnostics.Debug.Assert(false, "No selected result file.");
			}
			else 
			{
				string selectedResultFileName = GetSelectedUserNode().Text ;
				string resultFileName = "";
				if (selectedResultFileName.StartsWith("Detail_")) 
				{
					resultFileName = resultfiletag.DetailFile;
				}
				else 
				{
					resultFileName = resultfiletag.SummaryFile;
				}
				string theWarningText = string.Format("Are you sure you want to remove the ResultFile and its associated result files\n\n{0}\n\nfrom the Session?",resultFileName);
				if (MessageBox.Show (this,
					theWarningText,
					"Remove ResultFile from Session?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2)== DialogResult.Yes)
				{

					DirectoryInfo theDirectoryInfo;
					FileInfo[] theFilesInfo = new FileInfo[0];

					theDirectoryInfo = new DirectoryInfo (resultfiletag.ParentSession.ResultsRootDirectory);

					if (theDirectoryInfo.Exists) 
					{
						string baseName = DvtkApplicationLayer.Result.GetBaseNameNoCheck(resultFileName);

						// If the baseNamePrefix cannot be found, no files will be removed.
						string baseNamePrefix = "__________________________________";

						if (resultFileName.ToLower().StartsWith("summary")) 
						{
							baseNamePrefix = resultFileName.Substring(0, 12); 
						}
						else if (resultFileName.ToLower().StartsWith("detail")) 
						{
							baseNamePrefix = resultFileName.Substring(0, 11); 
						}
						else 
						{
							// Sanity check.
							Debug.Assert(false);
						}

						theFilesInfo = theDirectoryInfo.GetFiles (baseNamePrefix + baseName + "*");

						foreach (FileInfo theFileInfo in theFilesInfo) 
						{
							string theResultsFileName = theFileInfo.Name;

							theResultsFilesToRemove.Add(theResultsFileName);						
						}
					}
			
					DvtkApplicationLayer.Result.Remove(resultfiletag.ParentSession, theResultsFilesToRemove);
			
					this._MainForm.ActionRefreshSessionTree();			
				}
			}
		}

		private void ContextMenu_RemoveAllResultFiles_Click(object sender, System.EventArgs e) 
		{	
			Object theSelectedTreeNodeTag = GetSelectedTag();
			ArrayList theResultsFilesToRemove = new ArrayList();
			if (theSelectedTreeNodeTag == null) 
			{
				// Sanity check.
				System.Diagnostics.Debug.Assert(false, "No selected results file.");
			}
			else 
			{
				string theWarningText = "Are you sure you want to remove the ResultFiles ?";
				if (MessageBox.Show (this,
					theWarningText,
					"Remove ResultFiles ",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2)== DialogResult.Yes) 
				{
					if (theSelectedTreeNodeTag is DvtkApplicationLayer.Script) 
					{				
						DvtkApplicationLayer.Script scriptTag = (DvtkApplicationLayer.Script)GetSelectedTag();
						theResultsFilesToRemove = DvtkApplicationLayer.Result.GetAllNamesForSession(scriptTag.ParentSession);
						theResultsFilesToRemove = DvtkApplicationLayer.Result.GetNamesForScriptFile(scriptTag.ScriptFileName, theResultsFilesToRemove);
						DvtkApplicationLayer.Result.Remove(scriptTag.ParentSession, theResultsFilesToRemove);
						
					}
					else if (theSelectedTreeNodeTag is DvtkApplicationLayer.Emulator) 
					{
						DvtkApplicationLayer.Emulator emulatorTag = (DvtkApplicationLayer.Emulator)GetSelectedTag();
						theResultsFilesToRemove = DvtkApplicationLayer.Result.GetAllNamesForSession(emulatorTag.ParentSession);
						string theEmulatorBaseName = DvtkApplicationLayer.Result.GetBaseNameForEmulator(emulatorTag.EmulatorType);
						theResultsFilesToRemove = DvtkApplicationLayer.Result.GetNamesForBaseName(theEmulatorBaseName, theResultsFilesToRemove);
						DvtkApplicationLayer.Result.Remove(emulatorTag.ParentSession, theResultsFilesToRemove);
					}					
					else if (theSelectedTreeNodeTag is DvtkApplicationLayer.MediaFile) 
					{
						DvtkApplicationLayer.MediaFile mediaFileTag = (DvtkApplicationLayer.MediaFile)GetSelectedTag();
						theResultsFilesToRemove = DvtkApplicationLayer.Result.GetAllNamesForSession(mediaFileTag.ParentSession);
						string mediaFileName = GetSelectedUserNode().Text.Replace("_DCM","");
						string theMediaFullFileName = Path.Combine(mediaFileTag.ParentSession.ResultsRootDirectory, mediaFileName);
						string theMediaFileBaseName = DvtkApplicationLayer.Result.GetBaseNameForMediaFile(theMediaFullFileName);
						theResultsFilesToRemove = DvtkApplicationLayer.Result.GetNamesForBaseName(theMediaFileBaseName, theResultsFilesToRemove);
						DvtkApplicationLayer.Result.Remove(mediaFileTag.ParentSession, theResultsFilesToRemove);
					}
					else 
					{
					}

					this._MainForm.ActionRefreshSessionTree();
				}
			}
		}

		private void ContextMenu_RemoveSessionFromProject_Click(object sender, System.EventArgs e) 
		{
			GetMainForm().RemoveSelectedSession();
			projectApp.HasProjectChanged = true ;
		}

		private void ContextMenu_ValidateFiles_Click(object sender, System.EventArgs e) 
		{
			userControlSessionTree.Execute();		
		}

		#endregion


		/// <summary>
		/// This method will be called is this project form is closed or when
		/// the main form is closed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ProjectForm2_Closing(object sender, System.ComponentModel.CancelEventArgs e) 
		{
			bool theMainFormIsClosing = true;

			// We don't know if calling of this method this is a consequence of closing this project form or
			// as a consequence of closing the main form.
			// The following code is needed to find this out (see the Form class Closing event in MSDN)
			if (((MainForm)ParentForm)._IsClosing) 
			{
				// Closing is done through the Exit menu item.
				theMainFormIsClosing = true;
			}
			else 
			{
				// The position of the cursor in screen coordinates.
				Point theCursorPosition = Cursor.Position;

				if ((ParentForm.Top + SystemInformation.CaptionHeight) < theCursorPosition.Y) 
				{
					theMainFormIsClosing = false;
				}
				else 
				{
					theMainFormIsClosing = true;
				}
			}

			// Close if allowed.
			if (theMainFormIsClosing) 
			{
				// Don't do anything.
				// The Main form will also receive a closing event and handle the closing of the application.
			}
			else 
			{
				e.Cancel = false;
				MainForm theMainForm = (MainForm)ParentForm;
				DialogResult theDialogResult;

				if (IsExecuting()) 
				{
					// Ask the user if execution needs to be stopped.
					theDialogResult = MessageBox.Show("Executing is still going on.\nStop execution?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

					if (theDialogResult == DialogResult.Yes) 
					{
						// Stop the execution. 
						Notify(new StopExecution(this));

						// Because execution is performed in a different thread, wait some
						// time to enable the execution to stop. Also give feedback to the user
						// of this waiting by showing a form stating this.
						TimerMessageForm theTimerMessageForm = new TimerMessageForm();

						theTimerMessageForm.ShowDialogSpecifiedTime("Stopping execution...", 3000);

						// Find out if execution really has stopped.
						if (IsExecuting()) 
						{
							theDialogResult = MessageBox.Show("Failed to stop execution.\n This form will not be closed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);	

							e.Cancel = true;
						}
					}
					else 
					{
						e.Cancel = true;
					}
				}

				if (ParentForm.MdiChildren.Length == 1) 
				{
					// This is the last project form that is being closed,
					// so the project needs to be closed.
					if (!e.Cancel) 
					{
						// e.Cancel is false, so no execution takes place any more.
						// Close the project.
						// If unsaved changes exist, give the user the possibility to save them.
						if (projectApp.AreProjectOrSessionsChanged()) 
						{
							_MainForm.Save(true);
						}
						
						if (projectApp.HasUserCancelledLastOperation()) 
						{
							// The user has cancelled the close operation.
							// Cancel the closing of this last project form.
							e.Cancel = true;
						}
						else 
						{
							// The user has not cancelled the close operation.
                            projectApp.Close(true);
							projectApp = null;
							Notify(new ProjectClosed());
						}
					}
				}
			}
		}

		private void ButtonViewCertificates_Click(object sender, System.EventArgs e) 
		{            
			try 
			{
				CredentialsCertificatesForm cred_cert_form = null;
            
				cred_cert_form = new CredentialsCertificatesForm (GetSession(), false);
            
				if (cred_cert_form.ShowDialog () == DialogResult.OK) 
				{
					SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
					Notify(theSessionChange);
				}		
			}
			catch (Exception theException) 
			{
				MessageBox.Show(theException.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
		}

		private void ButtonViewCredentials_Click(object sender, System.EventArgs e) 
		{
			bool passed = false;
            bool displayPassword = true;
			DialogResult result = DialogResult.None;
			(GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings.TlsPassword = ":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA";
			while(!passed) 
			{
				try 
				{
					CredentialsCertificatesForm cred_cert_form = null;
            
					cred_cert_form = new CredentialsCertificatesForm (GetSelectedSessionNew(), true);
					result = cred_cert_form.ShowDialog();
					passed = true;
                    displayPassword = false;
				} 
				catch (Exception theException) 
				{
					if (theException.GetType().FullName == "Wrappers.Exceptions.PasswordExpection")
					{
						passed = false;
                        PassWordForm passWordForm = new PassWordForm(GetSelectedSessionNew(), displayPassword);
                        displayPassword = false;
						if (passWordForm.ShowDialog()!= DialogResult.OK)
						{
							passed = true;
                            displayPassword = false;
						}
					}
					else 
					{
						MessageBox.Show(theException.Message ,"Warning", MessageBoxButtons.OK,MessageBoxIcon.Warning);
						passed = true;
					}
				}
			}
			if (result == DialogResult.OK) 
			{
				(GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings.TlsPassword = ":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA";
				SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}			
		}

		private void ButtonCreateCertificate_Click(object sender, System.EventArgs e) 
		{
			WizardCreateCertificate wizard = new WizardCreateCertificate();
			wizard.ShowDialog(this);		
		}

		private void CheckBoxSecureConnection_CheckedChanged(object sender, System.EventArgs e) 
		{     
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				Dvtk.Sessions.ISecure theISecure = GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure;
            
				if (theISecure == null) 
				{
					// Sanity check.
					Debug.Assert(false);
				}
				else 
				{
					theISecure.SecuritySettings.SecureSocketsEnabled = CheckBoxSecureConnection.Checked;
            
					_TCM_SessionUsedForContentsOfTabSessionInformation = null;
					TCM_UpdateTabSessionInformation();
            
					SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
					Notify(theSessionChange);
				}
			}										
		}

		private void CheckBoxCheckRemoteCertificates_CheckedChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				if (GetSelectedSessionNew().Implementation is Dvtk.Sessions.ISecure) 
				{
					Dvtk.Sessions.ISecuritySettings theISecuritySettings = null;
					theISecuritySettings = (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings;
            
					bool isCurrentlyChecked = theISecuritySettings.CheckRemoteCertificate;
            
					try 
					{
						theISecuritySettings.CheckRemoteCertificate = CheckBoxCheckRemoteCertificates.Checked;
            
						SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
						Notify(theSessionChange);
					}
					catch (Exception theException) 
					{
						// Put the state of the check box back to the unchanged setting of the session.
						CheckBoxCheckRemoteCertificates.Checked = isCurrentlyChecked;
            
						MessageBox.Show(theException.Message + "\n\nThe change for checkbox \"" + CheckBoxCheckRemoteCertificates.Text + "\" is not allowed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
				else 
				{
					// Sanity check.
					Debug.Assert(false);
				}	
			}
		}

		private void CheckBoxCacheSecureSessions_CheckedChanged(object sender, System.EventArgs e) 
		{		
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				if (GetSelectedSessionNew().Implementation is Dvtk.Sessions.ISecure) 
				{
					Dvtk.Sessions.ISecuritySettings theISecuritySettings = null;
					theISecuritySettings = (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings;
            
					bool isCurrentlyChecked = theISecuritySettings.CacheTlsSessions;
            
					try 
					{
						theISecuritySettings.CacheTlsSessions = CheckBoxCacheSecureSessions.Checked;
            
						SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
						Notify(theSessionChange);
					}
					catch (Exception theException) 
					{
						// Put the state of the check box back to the unchanged setting of the session.
						CheckBoxCacheSecureSessions.Checked = isCurrentlyChecked;
            
						MessageBox.Show(theException.Message + "\n\nThe change for checkbox \"" + CheckBoxCacheSecureSessions.Text + "\" is not allowed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
				else 
				{
					// Sanity check.
					Debug.Assert(false);
				}	
			}
		}

		private void UpdateCipherFlag(Dvtk.Sessions.CipherFlags theCipherFlag, RadioButton theCheckBox) 
		{		
			if (GetSelectedSessionNew().Implementation is Dvtk.Sessions.ISecure) 
			{
				Dvtk.Sessions.ISecuritySettings theISecuritySettings = null;
				theISecuritySettings = (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings;
            
				bool isCurrentlyChecked = ((theISecuritySettings.CipherFlags & theCipherFlag) != 0);
            
				try 
				{
					if (theCheckBox.Checked) 
					{
						theISecuritySettings.CipherFlags |= theCipherFlag;
					}
					else 
					{
						theISecuritySettings.CipherFlags &= ~theCipherFlag;
					}
            
					SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
					Notify(theSessionChange);
				}
				catch (Exception theException) 
				{
					// Put the state of the check box back to the unchanged setting of the session.
					theCheckBox.Checked = isCurrentlyChecked;

                    int account_num = 149;

                    
                    ////uit
                    //theISecuritySettings.CipherFlags &= ~Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE;
                    //theISecuritySettings.CipherFlags &= ~Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128;
                    //theISecuritySettings.CipherFlags &= ~Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256;

                    ////aan
                    //theISecuritySettings.CipherFlags |= ~Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES;

                    //RadioButtonEncryptionTripleDES.Checked = true;
                    //RadioButtonEncryptionNone.Checked = false;
                    //RadioButtonEncryptionAES128.Checked = false;
                    //RadioButtonEncryptionAES256.Checked = false;

					MessageBox.Show(theException.Message + "\n\nThe change for checkbox \"" + theCheckBox.Text + "\" is not allowed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else 
			{
				// Sanity check.
				Debug.Assert(false);
			}
		}										

		private void UpdateTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags theVersionFlag, CheckBox theCheckBox) 
		{
			if (GetSelectedSessionNew().Implementation is Dvtk.Sessions.ISecure) 
			{
				Dvtk.Sessions.ISecuritySettings theISecuritySettings = null;
				theISecuritySettings = (GetSelectedSessionNew().Implementation as Dvtk.Sessions.ISecure).SecuritySettings;
            
				bool isCurrentlyChecked = ((theISecuritySettings.TlsVersionFlags & theVersionFlag) != 0);
            
				try 
				{
					if (theCheckBox.Checked) 
					{
						theISecuritySettings.TlsVersionFlags |= theVersionFlag;
					}
					else 
					{
						theISecuritySettings.TlsVersionFlags &= ~theVersionFlag;
					}
            
					SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
					Notify(theSessionChange);
				}
				catch (Exception theException) 
				{
					// Put the state of the check box back to the unchanged setting of the session.
					theCheckBox.Checked = isCurrentlyChecked;
            
					MessageBox.Show(theException.Message + "\n\nThe change for checkbox \"" + theCheckBox.Text + "\" is not allowed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else 
			{
				// Sanity check.
				Debug.Assert(false);
			}
		}										

		private void CheckBoxTLS_CheckedChanged(object sender, System.EventArgs e) 
		{
			if (_TCM_UpdateCount == 0) 
			{
				UpdateTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1, CheckBoxTLS);
			}										
		}

		private void CheckBoxSSL_CheckedChanged(object sender, System.EventArgs e) 
		{
			if (_TCM_UpdateCount == 0) 
			{
				UpdateTlsVersionFlag(Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_SSLv3, CheckBoxSSL);
			}										
		}

        // Aut RSA
        private void RadioButtonAuthenticationRSA_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonAuthenticationRSA.Checked)
                {
                    RadioButtonAuthenticationRSAisChecked = true;
                }
            }
            else if (!RadioButtonAuthenticationRSA.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_RSA, RadioButtonAuthenticationRSA);
                RadioButtonAuthenticationRSAisChecked = false;
            }
        }

        bool RadioButtonAuthenticationRSAisChecked = false;

        private void RadioButtonAuthenticationRSA_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonAuthenticationRSA.Checked && RadioButtonAuthenticationRSAisChecked)
                {
                    RadioButtonAuthenticationRSA.Checked = false;
                    RadioButtonAuthenticationRSAisChecked = false;
                }
                else
                {
                    RadioButtonAuthenticationRSA.Checked = true;
                    RadioButtonAuthenticationRSAisChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_RSA, RadioButtonAuthenticationRSA);
            }
        }

        // Aut DSA
        private void RadioButtonAuthenticationDSA_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonAuthenticationDSA.Checked)
                {
                    RadioButtonAuthenticationDSAisChecked = true;
                }
            }
            else if (!RadioButtonAuthenticationDSA.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_DSA, RadioButtonAuthenticationDSA);
                RadioButtonAuthenticationDSAisChecked = false;
            }
        }

        bool RadioButtonAuthenticationDSAisChecked = false;

        private void RadioButtonAuthenticationDSA_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonAuthenticationDSA.Checked && RadioButtonAuthenticationDSAisChecked)
                {
                    RadioButtonAuthenticationDSA.Checked = false;
                    RadioButtonAuthenticationDSAisChecked = false;
                }
                else
                {
                    RadioButtonAuthenticationDSA.Checked = true;
                    RadioButtonAuthenticationDSAisChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_DSA, RadioButtonAuthenticationDSA);
            }
        }

        // KE RSA
		private void RadioButtonKeyExchangeRSA_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonKeyExchangeRSA.Checked)
                {
                    RadioButtonKeyExchangeRSAisChecked = true;
                }
            }
            else if (!RadioButtonKeyExchangeRSA.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA, RadioButtonKeyExchangeRSA);
                RadioButtonKeyExchangeRSAisChecked = false;
            }	
		}

        bool RadioButtonKeyExchangeRSAisChecked = false;

        private void RadioButtonKeyExchangeRSA_Click(object sender, System.EventArgs e) 
		{
			if (_TCM_UpdateCount == 0) 
			{
                if (RadioButtonKeyExchangeRSA.Checked && RadioButtonKeyExchangeRSAisChecked)
                {
                    RadioButtonKeyExchangeRSA.Checked = false;
                    RadioButtonKeyExchangeRSAisChecked = false;
                }
                else
                {
                    RadioButtonKeyExchangeRSA.Checked = true;
                    RadioButtonKeyExchangeRSAisChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA, RadioButtonKeyExchangeRSA);
			}										
		}

        // KE DH

        private void RadioButtonKeyExchangeDH_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonKeyExchangeDH.Checked)
                {
                    RadioButtonKeyExchangeDHisChecked = true;
                }
            }
            else if (!RadioButtonKeyExchangeDH.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH, RadioButtonKeyExchangeDH);
                RadioButtonKeyExchangeDHisChecked = false;
            }
        }

        bool RadioButtonKeyExchangeDHisChecked = false;

        private void RadioButtonKeyExchangeDH_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonKeyExchangeDH.Checked && RadioButtonKeyExchangeDHisChecked)
                {
                    RadioButtonKeyExchangeDH.Checked = false;
                    RadioButtonKeyExchangeDHisChecked = false;
                }
                else
                {
                    RadioButtonKeyExchangeDH.Checked = true;
                    RadioButtonKeyExchangeDHisChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH, RadioButtonKeyExchangeDH);
            }
        }


        // DI SHA
        private void RadioButtonDataIntegritySHA_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonDataIntegritySHA.Checked)
                {
                    RadioButtonDataIntegritySHAisChecked = true;
                }
            }
            else if (!RadioButtonDataIntegritySHA.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1, RadioButtonDataIntegritySHA);
                RadioButtonDataIntegritySHAisChecked = false;
            }
        }

        bool RadioButtonDataIntegritySHAisChecked = false;

        private void RadioButtonDataIntegritySHA_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonDataIntegritySHA.Checked && RadioButtonDataIntegritySHAisChecked)
                {
                    RadioButtonDataIntegritySHA.Checked = false;
                    RadioButtonDataIntegritySHAisChecked = false;
                }
                else
                {
                    RadioButtonDataIntegritySHA.Checked = true;
                    RadioButtonDataIntegritySHAisChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1, RadioButtonDataIntegritySHA);
            }
        }

        // DI MD5
        private void RadioButtonDataIntegrityMD5_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonDataIntegrityMD5.Checked)
                {
                    RadioButtonDataIntegrityMD5isChecked = true;
                }
            }
            else if (!RadioButtonDataIntegrityMD5.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5, RadioButtonDataIntegrityMD5);
                RadioButtonDataIntegrityMD5isChecked = false;
            }
        }

        bool RadioButtonDataIntegrityMD5isChecked = false;

        private void RadioButtonDataIntegrityMD5_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonDataIntegrityMD5.Checked && RadioButtonDataIntegrityMD5isChecked)
                {
                    RadioButtonDataIntegrityMD5.Checked = false;
                    RadioButtonDataIntegrityMD5isChecked = false;
                }
                else
                {
                    RadioButtonDataIntegrityMD5.Checked = true;
                    RadioButtonDataIntegrityMD5isChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5, RadioButtonDataIntegrityMD5);
            }
        }

        // En None
        private void RadioButtonEncryptionNone_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonEncryptionNone.Checked)
                {
                    RadioButtonEncryptionNoneisChecked = true;
                }
            }
            else if (!RadioButtonEncryptionNone.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE, RadioButtonEncryptionNone);
                RadioButtonEncryptionNoneisChecked = false;
            }
        }

        bool RadioButtonEncryptionNoneisChecked = false;

        private void RadioButtonEncryptionNone_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonEncryptionNone.Checked && RadioButtonEncryptionNoneisChecked)
                {
                    RadioButtonEncryptionNone.Checked = false;
                    RadioButtonEncryptionNoneisChecked = false;
                }
                else
                {
                    RadioButtonEncryptionNone.Checked = true;
                    RadioButtonEncryptionNoneisChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE, RadioButtonEncryptionNone);
            }
        }

        // En DES
        private void RadioButtonEncryptionTripleDES_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonEncryptionTripleDES.Checked)
                {
                    CheckBoxEncryption3DESisChecked = true;
                }
            }
            else if (!RadioButtonEncryptionTripleDES.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, RadioButtonEncryptionTripleDES);
                CheckBoxEncryption3DESisChecked = false;
            }
        }

        bool CheckBoxEncryption3DESisChecked = false;

        private void RadioButtonEncryptionTripleDES_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonEncryptionTripleDES.Checked && CheckBoxEncryption3DESisChecked)
                {
                    RadioButtonEncryptionTripleDES.Checked = false;
                    CheckBoxEncryption3DESisChecked = false;
                }
                else
                {
                    RadioButtonEncryptionTripleDES.Checked = true;
                    CheckBoxEncryption3DESisChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, RadioButtonEncryptionTripleDES);
            }
        }

        // En AES128
        private void RadioButtonEncryptionAES128_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonEncryptionAES128.Checked)
                {
                    RadioButtonEncryptionAES128isChecked = true;
                }
            }
            else if (!RadioButtonEncryptionAES128.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128, RadioButtonEncryptionAES128);
                RadioButtonEncryptionAES128isChecked = false;
            }
        }

        bool RadioButtonEncryptionAES128isChecked = false;

        private void RadioButtonEncryptionAES128_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonEncryptionAES128.Checked && RadioButtonEncryptionAES128isChecked)
                {
                    RadioButtonEncryptionAES128.Checked = false;
                    RadioButtonEncryptionAES128isChecked = false;
                }
                else
                {
                    RadioButtonEncryptionAES128.Checked = true;
                    RadioButtonEncryptionAES128isChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128, RadioButtonEncryptionAES128);
            }
        }

        // En AES256
        private void RadioButtonEncryptionAES256_CheckedChanged(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount != 0)
            {
                if (RadioButtonEncryptionAES256.Checked)
                {
                    RadioButtonEncryptionAES256isChecked = true;
                }
            }
            else if (!RadioButtonEncryptionAES256.Checked)
            {
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256, RadioButtonEncryptionAES256);
                RadioButtonEncryptionAES256isChecked = false;
            }
        }

        bool RadioButtonEncryptionAES256isChecked = false;

        private void RadioButtonEncryptionAES256_Click(object sender, System.EventArgs e)
        {
            if (_TCM_UpdateCount == 0)
            {
                if (RadioButtonEncryptionAES256.Checked && RadioButtonEncryptionAES256isChecked)
                {
                    RadioButtonEncryptionAES256.Checked = false;
                    RadioButtonEncryptionAES256isChecked = false;
                }
                else
                {
                    RadioButtonEncryptionAES256.Checked = true;
                    RadioButtonEncryptionAES256isChecked = true;
                }
                UpdateCipherFlag(Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256, RadioButtonEncryptionAES256);
            }
        }


		public void TCM_CopySelectedTextToClipboard() 
		{
			switch(GetActiveTab()) 
			{
				case ProjectForm2.ProjectFormActiveTab.SCRIPT_TAB: 
				{
					if (webBrowserScript.Visible) 
					{
						mshtml.HTMLDocument theHtmlDocument = (mshtml.HTMLDocument)webBrowserScript.Document.DomDocument;
						mshtml.IHTMLTxtRange theHtmlTextRange = (mshtml.IHTMLTxtRange)theHtmlDocument.selection.createRange ();

						if (theHtmlTextRange.text != null) 
						{
							if (theHtmlTextRange.text != "") 
							{
								// Sets the data into the Clipboard.
								Clipboard.SetDataObject(theHtmlTextRange.text);
							}
						}
					}
					else 
					{
						RichTextBoxScript.Copy();
						//Clipboard.SetDataObject(RichTextBoxScript.SelectedText);
					}
				}
				break;
				case ProjectForm2.ProjectFormActiveTab.VALIDATION_RESULTS_TAB: 
				{
                    mshtml.HTMLDocument theHtmlDocument = (mshtml.HTMLDocument)webBrowserValResult.Document.DomDocument;
					mshtml.IHTMLTxtRange theHtmlTextRange = (mshtml.IHTMLTxtRange)theHtmlDocument.selection.createRange ();

					if (theHtmlTextRange.text != null) 
					{
						if (theHtmlTextRange.text != "") 
						{
							Clipboard.SetDataObject(theHtmlTextRange.text);
						}
					}
				}
				break;
				case ProjectForm2.ProjectFormActiveTab.ACTIVITY_LOGGING_TAB:
					RichTextBoxActivityLogging.Copy();
					break;
				case ProjectForm2.ProjectFormActiveTab.SPECIFY_SOP_CLASSES_TAB:
					RichTextBoxSpecifySopClassesInfo.Copy();
					break;
				default:
					// Do nothing.
					break;
			}
		}

		public void TCM_FilterResults() 
		{
			FileInfo fileInfo = new FileInfo(resultProjectXml);
			if (fileInfo.Exists)
			{
				try 
				{
					XmlDocument xmldoc = new XmlDocument();
					xmldoc.Load(fileInfo.FullName);
				}
				catch 
				{
					CreateXmlTemplate();
				}
			}
			if ( !fileInfo.Exists)
			{	
				CreateXmlTemplate();
			}
			try 
			{
				FileInfo file = new FileInfo(projectApp.ProjectFileName);
				string projectStructureFile = Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + ".xml");
				ProjectXmlFile(projectStructureFile);
				String finalHtmlFile = projectStructureFile.ToLower().Replace(".xml", ".html");

				string tempFile1 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "1.xml";
				string tempFile2 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "2.xml";
				string tempFile3 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "3.xml";

				Convert(Path.GetFileName(projectStructureFile), tempFile1, "createsum.xsl");
				Convert(tempFile1,tempFile2,"sortontype.xsl");
				Convert(tempFile2,tempFile3, "sortonmes.xsl");
				Convert(tempFile3,  finalHtmlFile, "viewonmesfiltered.xsl");

                webBrowserResultMgr.Navigate(finalHtmlFile);
			}
			catch ( Exception fileException)
			{
				MessageBox.Show(fileException.Message);
			}
		}

		private void ContextMenu_ExploreResultsDir_Click(object sender, System.EventArgs e) 
		{
			if (GetSession() != null) 
			{
				System.Diagnostics.Process theProcess  = new System.Diagnostics.Process();

				theProcess.StartInfo.FileName= "Explorer.exe";
				theProcess.StartInfo.Arguments = GetSession().ResultsRootDirectory;

				theProcess.Start();
			}
		}

		private void ContextMenu_ExploreScriptsDir_Click(object sender, System.EventArgs e) 
		{
			if (GetSession() != null) 
			{
				DvtkApplicationLayer.ScriptSession theScriptSession = GetSession() as DvtkApplicationLayer.ScriptSession;

				if (theScriptSession != null) 
				{
					System.Diagnostics.Process theProcess  = new System.Diagnostics.Process();

					theProcess.StartInfo.FileName= "Explorer.exe";
					theProcess.StartInfo.Arguments = theScriptSession.DicomScriptRootDirectory;

					theProcess.Start();
				}
			}		
		}

		/// <summary>
		/// Get (if exisiting) the session that is executed by this Project Form.
		/// </summary>
		/// <returns>Session that is executed by this Project Form, null if this Project Form is not executing a session.</returns>
		public DvtkApplicationLayer.Session GetExecutingSession() 
		{
			return(userControlSessionTree.GetExecutingSession());
		}

		private void ContextMenu_SaveAs_Click(object sender, System.EventArgs e) 
		{
			DvtkApplicationLayer.Session theCurrentSession = GetSelectedSessionNew();
			DvtkApplicationLayer.Session theNewSession = theCurrentSession.SaveSessionAs(theCurrentSession);

			if (theNewSession != null) 
			{
				Notify(new SessionReplaced(theCurrentSession, theNewSession));
			}		
		}

		private void ContextMenu_Save_Click(object sender, System.EventArgs e) 
		{
			Object theSelectedTreeNodeTag = GetSelectedTag();
			DvtkApplicationLayer.Session tempSession = GetSession();            
			if (theSelectedTreeNodeTag is DvtkApplicationLayer.Session) 
			{
				tempSession.SaveSession(((DvtkApplicationLayer.Session)theSelectedTreeNodeTag).SessionFileName);
				Notify(new Saved());
				_MainForm.UpdateUIControls();
			}
			else 
			{
				// Sanity check.
				Debug.Assert(false);
			}
		}

		private void ContextMenu_ValidateMediaFiles_Click(object sender, System.EventArgs e) 
		{
			// Set the session object member to default value.
            try
            {
                Session tempSession = GetSelectedSessionNew();
                tempSession.Implementation.ValidateReferencedFile = true;
                userControlSessionTree.Execute();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
		}

		private void ButtonTrustedCertificatesFile_Click(object sender, System.EventArgs e) 
		{           
			OpenFileDialog theOpenFileDialog = new OpenFileDialog();
            
			theOpenFileDialog.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer";
            
			theOpenFileDialog.Title = "Select the file containing the SUT Public Keys (certificates)";
            
			theOpenFileDialog.CheckFileExists = false;
            
			// Only if the current file exists, set this file in the file browser.
			if (TextBoxTrustedCertificatesFile.Text != "") 
			{
				if (File.Exists(TextBoxTrustedCertificatesFile.Text)) 
				{
					theOpenFileDialog.FileName = TextBoxTrustedCertificatesFile.Text;
				}
			}
            
			if (theOpenFileDialog.ShowDialog() == DialogResult.OK) 
			{
				TextBoxTrustedCertificatesFile.Text = theOpenFileDialog.FileName;
				((Dvtk.Sessions.ISecure)GetSelectedSessionNew().Implementation).SecuritySettings.CertificateFileName = theOpenFileDialog.FileName;
            
				// Notify the rest of the world of the change.
				SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}
		}

		private void ButtonSecurityCredentialsFile_Click(object sender, System.EventArgs e) 
		{
			OpenFileDialog theOpenFileDialog = new OpenFileDialog();

			theOpenFileDialog.Filter = "PEM Certificate files (*.pem;*.cer)|*.pem;*.cer";

			theOpenFileDialog.Title = "Select the file containing the DVT Private Keys (credentials)";

			theOpenFileDialog.CheckFileExists = false;

			// Only if the current file exists, set this file in the file browser.
			if (TextBoxSecurityCredentialsFile.Text != "") 
			{
				if (File.Exists(TextBoxSecurityCredentialsFile.Text)) 
				{
					theOpenFileDialog.FileName = TextBoxSecurityCredentialsFile.Text;
				}
			}

			if (theOpenFileDialog.ShowDialog() == DialogResult.OK) 
			{
				TextBoxSecurityCredentialsFile.Text = theOpenFileDialog.FileName;
				((Dvtk.Sessions.ISecure)GetSelectedSessionNew().Implementation).SecuritySettings.CredentialsFileName = theOpenFileDialog.FileName;

				// Notify the rest of the world of the change.
				SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
		}

		private void ComboBoxStorageMode_SelectedIndexChanged(object sender, System.EventArgs e) 
		{
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0) 
			{
				switch(ComboBoxStorageMode.SelectedIndex) 
				{
					case 0:
						GetSelectedSessionNew().Mode = DvtkApplicationLayer.Session.StorageMode.AsDataSet; 
						break;
            
					case 1:
						GetSelectedSessionNew().Mode = DvtkApplicationLayer.Session.StorageMode.AsMedia; 
						break;
            
					case 2:
						GetSelectedSessionNew().Mode = DvtkApplicationLayer.Session.StorageMode.AsMediaOnly; 
						break;
            
					case 3:
						GetSelectedSessionNew().Mode = DvtkApplicationLayer.Session.StorageMode.NoStorage; 
						break;
            
					default:
						// Not supposed to get here.
						Debug.Assert(false);
						break;
				}
            
				SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}			
		}

		private void CheckBoxLogRelation_CheckedChanged(object sender, System.EventArgs e) 
		{           
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0)
			{
                Dvtk.Sessions.LogLevelFlags flag = (Dvtk.Sessions.LogLevelFlags)((GetSelectedSessionNew().LogLevelMask));
				if (CheckBoxLogRelation.Checked)
				{
                    flag |= Dvtk.Sessions.LogLevelFlags.ImageRelation;
				}
				else
				{
                    flag ^= Dvtk.Sessions.LogLevelFlags.ImageRelation;
				}

                GetSelectedSessionNew().LogLevelMask = (int)flag;
				SessionChange theSessionChange = new SessionChange(GetSelectedSessionNew(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
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

		private void ListBoxSpecifySopClassesDefinitionFileDirectories_SelectedIndexChanged(object sender, System.EventArgs e) 
		{
			_SopClassesManager.UpdateRemoveButton();
		}

        private void webBrowserScript_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            string theFullFileName = DvtkWebBrowserNew.GetFullFileNameFromHtmlLink(e.Url.ToString());
        }

        private void webBrowserScript_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Notify(new scriptWebNavigationComplete());
        }

		private void ContextMenu_ViewExpandedScript_Click(object sender, System.EventArgs e) 
		{
			userControlSessionTree.ViewExpandedScriptFile();
		}

		private void ContextMenuRichTextBox_Popup(object sender, System.EventArgs e) 
		{
			ContextMenu_Copy.Visible = false;
			if (GetActiveTab()==ProjectForm2.ProjectFormActiveTab.SCRIPT_TAB||GetActiveTab()==ProjectForm2.ProjectFormActiveTab.ACTIVITY_LOGGING_TAB||GetActiveTab()==ProjectForm2.ProjectFormActiveTab.SPECIFY_SOP_CLASSES_TAB) 
			{
				ContextMenu_Copy.Visible = true;
			}
		}

		private void ContextMenu_Copy_Click(object sender, System.EventArgs e) 
		{
			GetMainForm().ActionEditCopy();		
		}

		private void CheckBoxDefineSQLength_CheckedChanged(object sender, System.EventArgs e) 
		{          
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0)
			{
				DvtkApplicationLayer.Session theSelectedSession = GetSelectedSessionNew();
            
				// Update the session member.
				if (theSelectedSession is DvtkApplicationLayer.ScriptSession)
				{
					DvtkApplicationLayer.ScriptSession theScriptSession = (DvtkApplicationLayer.ScriptSession)theSelectedSession;
					theScriptSession.DefineSqLength = CheckBoxDefineSQLength.Checked;
				}
				if (theSelectedSession is DvtkApplicationLayer.EmulatorSession)
				{
					DvtkApplicationLayer.EmulatorSession theEmulatorSession = (DvtkApplicationLayer.EmulatorSession)theSelectedSession;
					theEmulatorSession.DefineSqLength = CheckBoxDefineSQLength.Checked;
				}
            
				// Notify the rest of the world that the session has been changed.
				SessionChange theSessionChange = new SessionChange(theSelectedSession, SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
		}

		private void CheckBoxAddGroupLengths_CheckedChanged(object sender, System.EventArgs e) 
		{            
			// Only react when the user has made changes, not when the TCM_Update method has been called.
			if (_TCM_UpdateCount == 0)
			{
				DvtkApplicationLayer.Session theSelectedSession = GetSelectedSessionNew();
            
				// Update the session member.
				if (theSelectedSession is DvtkApplicationLayer.ScriptSession)
				{
					DvtkApplicationLayer.ScriptSession theScriptSession = (DvtkApplicationLayer.ScriptSession)theSelectedSession;
					theScriptSession.AddGroupLength = CheckBoxAddGroupLengths.Checked;
				}
				if (theSelectedSession is DvtkApplicationLayer.EmulatorSession)
				{
					DvtkApplicationLayer.EmulatorSession theEmulatorSession = (DvtkApplicationLayer.EmulatorSession)theSelectedSession;
					theEmulatorSession.AddGroupLength = CheckBoxAddGroupLengths.Checked;
				}
            
				// Notift the rest of the world that the session has been changed.
				SessionChange theSessionChange = new SessionChange(theSelectedSession, SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
		}

		private void ProjectForm2_Enter(object sender, System.EventArgs e) 
		{
			Select();
		}

		private void contextMenuUserControlSessionTree_Popup(object sender, System.EventArgs e) 
		{
			Object theSelectedTreeNodeTag = userControlSessionTree.GetSelectedTag();
			ContextMenu_AddNewSession.Visible = false;
			ContextMenu_AddNewSession.DefaultItem = false;
			ContextMenu_AddExistingSessions.Visible = false;
			ContextMenu_AddExistingSessions.DefaultItem = false;
			ContextMenu_Edit.Visible = false;
			ContextMenu_Edit.DefaultItem = false;
			ContextMenu_Execute.Visible = false;
			ContextMenu_Execute.DefaultItem = false;
			ContextMenu_ExploreResultsDir.Visible = false;
			ContextMenu_ExploreResultsDir.DefaultItem = false;
			ContextMenu_ExploreScriptsDir.Visible = false;
			ContextMenu_ExploreScriptsDir.DefaultItem = false;
			ContextMenu_None.Visible = false;
			ContextMenu_None.DefaultItem = false;
			ContextMenu_Remove.Visible = false;
			ContextMenu_Remove.DefaultItem = false;
			ContextMenu_RemoveAllResultsFiles.Visible = false;
			ContextMenu_RemoveAllResultsFiles.DefaultItem = false;
			ContextMenu_RemoveSessionFromProject.Visible = false;
			ContextMenu_RemoveSessionFromProject.DefaultItem = false;
			ContextMenu_Save.Visible = false;
			ContextMenu_Save.DefaultItem = false;
			ContextMenu_SaveAs.Visible = false;
			ContextMenu_SaveAs.DefaultItem = false;
			ContextMenu_ValidateMediaFiles.Visible = false;
			ContextMenu_ValidateMediaFiles.DefaultItem = false;
            ContextMenu_ValidateMediaDirectory.Visible = false;
            ContextMenu_ValidateMediaDirectory.DefaultItem = false;
			ContextMenu_ValidateDicomdirWithoutRefFile.Visible = false ;
			ContextMenu_ValidateDicomdirWithoutRefFile.DefaultItem = false ;
			ContextMenu_ViewExpandedScript.Visible = false;
			ContextMenu_ViewExpandedScript.DefaultItem = false;
			ContextMenu_GenerateDICOMDIR.Visible = false;
			ContextMenu_GenerateDICOMDIR.DefaultItem = false;
			ContextMenu_GenerateDICOMDIRWithDirectory.Visible = false;
			ContextMenu_GenerateDICOMDIRWithDirectory.DefaultItem = false;

			if (theSelectedTreeNodeTag == null) 
			{
				_MainForm.MenuItem_FileSessionRemove.Enabled = false;
			}
			else if (theSelectedTreeNodeTag is DvtkApplicationLayer.Session) 
			{
				ContextMenu_ExploreResultsDir.Visible = true;
				ContextMenu_RemoveSessionFromProject.Visible = true;
				ContextMenu_Save.Visible = true;
				ContextMenu_SaveAs.Visible = true;
				DvtkApplicationLayer.Session tempSession = (DvtkApplicationLayer.Session)theSelectedTreeNodeTag; 
				
				if (tempSession.GetSessionChanged(tempSession)) 
				{
					ContextMenu_Save.Enabled = true;
				}
				else 
				{
					ContextMenu_Save.Enabled = false;
				}

				if (theSelectedTreeNodeTag is DvtkApplicationLayer.MediaSession) 
				{
					ContextMenu_ValidateMediaFiles.Visible = true;
					ContextMenu_ValidateMediaFiles.DefaultItem = true;
					ContextMenu_GenerateDICOMDIR.Visible = true;
					ContextMenu_GenerateDICOMDIRWithDirectory.Visible = true;
					ContextMenu_ValidateDicomdirWithoutRefFile.Visible = true ;
                    ContextMenu_ValidateMediaDirectory.Visible = true;
				}

				if (theSelectedTreeNodeTag is DvtkApplicationLayer.ScriptSession) 
				{
					ContextMenu_ExploreScriptsDir.Visible = true;
				}
			}
			else if (theSelectedTreeNodeTag is DvtkApplicationLayer.Emulator) 
			{
				ContextMenu_Execute.Visible = true;
				ContextMenu_Execute.DefaultItem = true;
				ContextMenu_RemoveAllResultsFiles.Visible = true;
			}
			else if (theSelectedTreeNodeTag is DvtkApplicationLayer.Script) 
			{
				ContextMenu_Edit.Visible = true;
				ContextMenu_Execute.Visible = true;
				ContextMenu_Execute.DefaultItem = true;
				ContextMenu_RemoveAllResultsFiles.Visible = true;
				DvtkApplicationLayer.Script script = theSelectedTreeNodeTag as DvtkApplicationLayer.Script;
				if (script.ScriptFileName.ToLower().EndsWith(".vbs")) 
				{
					ContextMenu_ViewExpandedScript.Visible = true;
				}				
			}
			else if (theSelectedTreeNodeTag is DvtkApplicationLayer.Result) 
			{
				ContextMenu_Remove.Visible = true;
				ContextMenu_Remove.DefaultItem = true;
			}
			else if (theSelectedTreeNodeTag is DvtkApplicationLayer.MediaFile) 
			{
				ContextMenu_RemoveAllResultsFiles.Visible = true;
			}
			else if ( theSelectedTreeNodeTag is DvtkApplicationLayer.Project) 
			{
                DvtkApplicationLayer.Project projNode = (DvtkApplicationLayer.Project)theSelectedTreeNodeTag;
                if (projNode.IsProjectConstructed)
                {
                    ContextMenu_AddExistingSessions.Visible = true;
                    ContextMenu_AddNewSession.Visible = true;
                }
			}
			else 
			{
				// Sanity check.
				Debug.Assert(false);
			}        
		}

		private void ContextMenu_GenerateDICOMDIR_Click(object sender, System.EventArgs e) 
		{
			userControlSessionTree.GenerateDICOMDIR();		
		}

        private void webBrowserValResult_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string s = e.Url.ToString();
            string directory = Path.GetDirectoryName(s);
            directory = directory + @"\";
            if(directory.StartsWith("file:\\"))
            {
                directory = directory.Remove(0, "file:\\".Length);
            }
            string resultFileName = Path.GetFileName(s);
            resultFileName = resultFileName.Replace(".html", ".xml");
            userControlSessionTree.SearchAndSelectResultNode(directory, resultFileName);
        } 

		private void ContextMenu_ValidateDicomdirWithoutRefFile_Click(object sender, System.EventArgs e) 
		{
			//Update the session member.
			GetSelectedSessionNew().Implementation.ValidateReferencedFile = false;
			userControlSessionTree.Execute();	
		}

        private void webBrowserResultMgr_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
			try
			{
                this.LabelErrorMessage.Text = "";
				resultProjectXml  = projectApp.ProjectFileName + ".xml";

				// Copy necessary files to project folder
				FileInfo projFile = new FileInfo(projectApp.ProjectFileName);
				DirectoryInfo applnDir = new DirectoryInfo(Application.StartupPath);
				foreach(FileInfo file in applnDir.GetFiles("*.gif"))
				{
					file.CopyTo(projFile.DirectoryName + "\\" + file.Name, true);
				}
				FileInfo scriptFile = new FileInfo(Application.StartupPath + "\\" + "script.js");
				scriptFile.CopyTo(projFile.DirectoryName + "\\" + scriptFile.Name, true);

				name = "";
                fullResultFileName = DvtkWebBrowserNew.GetFullFileNameFromHtmlLink(e.Url.ToString());
				folderIndicator = false ;
				this.checkBoxIgnoreResult.Enabled = true ;
				this.checkBoxIgnoreResultAll.Enabled = true ;
				this.checkBoxIgnoreResultAll.Checked = false ;
				this.checkBoxIgnoreResult.Checked = false ;
				int indexResultFile = fullResultFileName.IndexOf("*" ,0);			
				if (indexResultFile != -1) 
				{
					string temptheFullFileName = fullResultFileName.Substring(0,indexResultFile);
					int tempIndex = temptheFullFileName.LastIndexOf("/");
					messageIndex = temptheFullFileName.Substring(tempIndex + 1).Trim();
					if(fullResultFileName.EndsWith("FOLDERLINK"))
					{
						this.checkBoxIgnoreResult.Enabled = false ;
						fullResultFileName = fullResultFileName.Substring(indexResultFile + 1);	
						int index = fullResultFileName.IndexOf("FOLDERLINK");
						fullResultFileName = fullResultFileName.Substring(0,index-1).Trim();
						folderIndicator = true ;
					}
					else 
					{
						this.checkBoxIgnoreResultAll.Enabled = false ;
						fullResultFileName = fullResultFileName.Substring(indexResultFile + 1).Trim();
						name = fullResultFileName + " * ";
					}
					CreatingProjectXmlFile(fullResultFileName , messageIndex, true );
					this.LabelErrorMessage.Text = name ;
					ReadProjectResultManagerXml(folderIndicator );
					e.Cancel = true ;
				}
			}
			catch ( Exception emessage)
			{
				MessageBox.Show(emessage.Message);
			}
		}

		private void CreatingProjectXmlFile(string resultXmlFileName , string messageIndex, bool folderIndicator) 
		{	
			try 
			{
				// Check that the file exists.
				FileInfo resultXmlFileInfo = new FileInfo(resultXmlFileName);
				XmlNodeList nodeList = null ;
				if (!resultXmlFileInfo.Exists) 
				{
					string msg =
						string.Format(
						"Failed to load result xml file: {0}\n. File does not exist.", resultXmlFileName);
					this.LabelErrorMessage.Text = "" ;
					this.checkBoxIgnoreResultAll.Checked = false ;
					this.checkBoxIgnoreResult.Checked = false ;
					this.TextBoxComment.Text = "" ;	
					this.TextBoxPr.Text = "";
					MessageBox.Show(msg);
				} 
				else 
				{
					XmlDocument xmldoc = new XmlDocument();
					xmldoc.Load(resultXmlFileName);
					XmlNode root = xmldoc.DocumentElement;
					ArrayList errorTypes = new ArrayList();
					errorTypes.Add("//Activity");
					errorTypes.Add("//UserActivity");
					
					if (messageIndex != "")
					{
						nameHasValue = false;
						nodeList = xmldoc.SelectNodes("descendant::Message[@Index =" + messageIndex + "]/Meaning");
						if ( nodeList.Count == 0 )
						{
							foreach (String errorType in errorTypes)
							{
								nodeList = xmldoc.SelectNodes (errorType);
								CreatingTextMessage(nodeList,folderIndicator);
							}
						}
						if (!nameHasValue)
						{
							CreatingTextMessage(nodeList,folderIndicator);
						}
					}
				}						
			}					
			catch (Exception e) 
			{
				MessageBox.Show(e.Message);
			}
		}

		private void CreatingTextMessage(XmlNodeList nodeList, bool folderIndicator)
		{
			foreach (XmlNode message in nodeList)
			{	
				if (message.Attributes.Count == 0 )
				{
					messageText = message.InnerText ;

					if(this.checkBoxIgnoreResultAll.Checked)
						name += "ALL * " + messageText;// + " " + location;
					else
						name += messageText;// + " " + location;
				}
				else 
				{
					XmlAttributeCollection attrColl = message.Attributes;
					if (attrColl[0].Value == messageIndex.Trim())
					{
						messageText = message.InnerText ;
						if(this.checkBoxIgnoreResultAll.Checked)
							name += "ALL * " + messageText;
						else
							name += messageText;
						nameHasValue = true;
					}
				}
			}
		}

		private void ButtonSaveIssue_Click(object sender, System.EventArgs e)
		{
			try
			{
				string comment = TextBoxComment.Text;
				string prDetails = TextBoxPr.Text;
				string msg = name;
				string textType = null;
				if (this.checkBoxIgnoreResult.Checked)
				{
					if (!name.StartsWith(fullResultFileName))
					{
						if (fullResultFileName.ToLower().StartsWith("detail"))
						{
							fullResultFileName = fullResultFileName.ToLower().Replace("detail" ,"summary");
						}
						name = fullResultFileName + " * " + name ;
					}
				}

				if (this.checkBoxIgnoreResultAll.Checked)
				{
					name = "ALL" + " * " + name ;
				}

				if(this.checkBoxIgnoreResult.Checked || this.checkBoxIgnoreResultAll.Checked )
				{
					textType = "skipped";
				}
				if((!this.checkBoxIgnoreResult.Checked ) && (!this.checkBoxIgnoreResultAll.Checked)) 
				{
					textType = "Problem";
				}
				AddSaveOnIssueClick(name , msg, comment , prDetails , textType);	
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void CreateNewXml(string name , string comment ,string prDetails ,string textType )
		{
			XmlTextWriter writer  = new XmlTextWriter(resultProjectXml  , System.Text.Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			// Start the document
			writer.WriteStartDocument (true);
			writer.WriteStartElement ("Nodes");
			writer.WriteStartElement (textType);
			writer.WriteStartElement("Message");
			writer.WriteElementString ("Name",name.Trim());
			writer.WriteElementString ("Comment", comment);
			writer.WriteElementString ("PRnr", prDetails);
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			writer.WriteEndElement ();
			// End the document
			writer.WriteEndDocument ();

			// Close Flushes the document to the stream
			if (writer != null) writer.Close();
		}

		private void CreateXmlTemplate()
		{
			XmlTextWriter writerXmlTemplate  = new XmlTextWriter(resultProjectXml , System.Text.Encoding.UTF8);
			writerXmlTemplate.Formatting = Formatting.Indented;
			// Start the document
			writerXmlTemplate.WriteStartDocument (true);
			writerXmlTemplate.WriteStartElement ("Nodes");
			writerXmlTemplate.WriteElementString("Problem","");
			writerXmlTemplate.WriteElementString("skipped","");
			writerXmlTemplate.WriteElementString ("added","");
			writerXmlTemplate.WriteEndElement ();
			writerXmlTemplate.WriteEndDocument ();
			// Close Flushes the document to the stream
			if (writerXmlTemplate != null) writerXmlTemplate.Close();
		}

		private void ReadProjectResultManagerXml (bool indicator)
		{	
			FileInfo tempFileInfo = new FileInfo(resultProjectXml);
			if (tempFileInfo.Exists)
			{
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.Load(tempFileInfo.FullName);
				XmlNode root = xmldoc.DocumentElement;
				XmlTextReader tempReader = new XmlTextReader(tempFileInfo.FullName);
				XmlNodeList nodeToRead = null ;
				nodeToRead= root.SelectNodes("/Nodes//Name");
				foreach (XmlNode nodeRead in nodeToRead)
				{	
					string tempText = nodeRead.InnerText ;
					if(tempText.IndexOf("ALL") != -1)
					{
						indicator = true;
						tempText = tempText.Substring(tempText.IndexOf("*") + 1).Trim();
					}

					if(tempText.IndexOf(fullResultFileName) != -1)
					{
						tempText = tempText.Substring(tempText.IndexOf("*") + 1).Trim();
					}

					if(name.IndexOf("ALL") != -1)
					{
						indicator = true;
						name = name.Substring(name.IndexOf("*") + 1).Trim();
					}

					if(name.IndexOf(fullResultFileName) != -1)
					{
						name = name.Substring(name.IndexOf("*") + 1).Trim();
					}
					
					if (tempText.Equals(name))
					{
						this.TextBoxComment.Text = nodeRead.NextSibling.InnerText ;	
						this.TextBoxPr.Text = nodeRead.NextSibling.NextSibling.InnerText;
						if (nodeRead.ParentNode.ParentNode.Name == "skipped")
						{	
							if (indicator)
							{							
								this.checkBoxIgnoreResultAll.Checked = true ;
							}
							else 
							{
								this.checkBoxIgnoreResult.Checked = true ;
							}
						}
						else if (nodeRead.ParentNode.ParentNode.Name == "Problem")
						{
							if (indicator)
							{							
								this.checkBoxIgnoreResultAll.Checked = false ;
							}
							else 
							{
								this.checkBoxIgnoreResult.Checked = false ;
							}
						}
						break ;
					}
					else 
					{
						this.TextBoxComment.Text = "";
						this.TextBoxPr.Text = "";	
					}
				}
				tempReader.Close();
			}
		}

		private void ReadProjectValidationTabXml()
		{
			FileInfo tempFileInfo = new FileInfo(resultProjectXml);
			if (tempFileInfo.Exists)
			{
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.Load(tempFileInfo.FullName);
				XmlNode root = xmldoc.DocumentElement;
				XmlTextReader tempReader = new XmlTextReader(tempFileInfo.FullName);
				XmlNodeList nodeToRead = null ;
				this.TextBoxPr.Text = "";
				this.TextBoxComment.Text = "";
				if (name !="" || name != null)
				{
					nodeToRead= root.SelectNodes("/Nodes//Name");
				}
				if (nodeToRead.Count > 0)
				{
					foreach (XmlNode nodeRead in nodeToRead)
					{	
						string tempText = nodeRead.InnerText ;
						if (tempText.EndsWith(name))
						{
						 
						}
						else 
						{
							XmlNode tempParentNode = nodeRead.ParentNode.ParentNode ;
							XmlNode tempNode =  nodeRead.ParentNode ;
							tempNode.RemoveAll();
							tempParentNode.RemoveChild(tempNode);
						}
					}
					foreach (XmlNode tempNode in nodeToRead)
					{
						string combinedNames = fullResultFileName + " * " + name ;
						if ( tempNode.InnerText.Equals(name))
						{
							this.TextBoxComment.Text = tempNode.NextSibling.InnerText ;	
							if (this.TextBoxPr.Text == "" || this.TextBoxPr.Text == null)
							{
								this.TextBoxPr.Text = tempNode.NextSibling.NextSibling.InnerText;
							}
							else 
							{
								this.TextBoxPr.Text = this.TextBoxPr.Text + " * " + tempNode.NextSibling.NextSibling.InnerText;
							}
							if (tempNode.ParentNode.ParentNode.Name == "skipped")
							{
								this.checkBoxIgnoreResultAll.Checked = true ;
							}
							else if (tempNode.ParentNode.ParentNode.Name == "Problem")
							{
								this.checkBoxIgnoreResultAll.Checked = false ;
							}
						}
						else if ((tempNode.InnerText.EndsWith(name)) && (!tempNode.InnerText.Equals(name)) && (tempNode.InnerText.Equals(combinedNames)))
						{
							if (this.TextBoxPr.Text == "" || this.TextBoxPr.Text == null)
							{
								this.TextBoxPr.Text = tempNode.NextSibling.NextSibling.InnerText;
							}
							else 
							{
								this.TextBoxPr.Text = this.TextBoxPr.Text + " * " + tempNode.NextSibling.NextSibling.InnerText;
							}
							if(this.TextBoxComment.Text == "")
							{						
								this.TextBoxComment.Text = tempNode.NextSibling.InnerText ;	
							}
							if (tempNode.ParentNode.ParentNode.Name == "skipped")
							{
								this.checkBoxIgnoreResult.Checked = true ;
							}
							else if (tempNode.ParentNode.ParentNode.Name == "Problem")
							{
								this.checkBoxIgnoreResult.Checked = false ;
							}
						}				
						else 
						{
						}				
					}
				}
				tempReader.Close();
			}
		}

		private void ProjectXmlFile (string projectStructureFile)
		{
			XmlTextWriter writer = null;
			writer = new XmlTextWriter(projectStructureFile, System.Text.Encoding.UTF8);
			
			// The written .xml file will be more readable
			writer.Formatting = Formatting.Indented;
        
			// Start the document
			writer.WriteStartDocument (true);
        
			// Write the session element containing all session files
			writer.WriteStartElement ("Collection");
			writer.WriteStartElement ("Role");
			writer.WriteElementString ("RoleName", projectApp.ProjectFileName); 
			try 
			{
				if (projectApp.Sessions.Count==0) 
				{
					Exception th ;
					throw th = new Exception("There are no sessions in the project file.");
				}
				foreach ( DvtkApplicationLayer.Session session in projectApp.Sessions) 
				{
					if (session is DvtkApplicationLayer.MediaSession) 
					{
						DvtkApplicationLayer.MediaSession mediaSession = session as MediaSession;

						writer.WriteStartElement ("Sop");
						writer.WriteElementString ("Sopname", mediaSession.SessionFileName);

						if (mediaSession.MediaFiles.Count !=0) 
						{
							foreach ( DvtkApplicationLayer.MediaFile mediafile in mediaSession.MediaFiles) 
							{
								writer.WriteStartElement ("file");
								writer.WriteElementString ("filename", mediafile.MediaFileName);
								if (mediafile.Result.Count !=0) 
								{
									foreach (Result result in mediafile.Result) 
									{	
										if ((result.SummaryFile != "") && (result.SummaryFile != null))
										{
											string resultFile = Path.Combine(mediaSession.ResultsRootDirectory, result.SummaryFile);
											FileInfo resultFileInfo = new FileInfo(resultFile);
                                            if (resultFileInfo.Exists)
                                            {
                                                if (IsAValidXML(resultFileInfo.FullName))
                                                    writer.WriteElementString("result", resultFile);                                                
                                            }
										}
										foreach (string subsummaryfile in result.SubSummaryResultFiles) 
										{
											if ((subsummaryfile != "") && (subsummaryfile != null))
											{
												string resultFile = Path.Combine(mediaSession.ResultsRootDirectory, subsummaryfile);
												FileInfo resultFileInfo = new FileInfo(resultFile);
                                                if (resultFileInfo.Exists)
                                                {
                                                    if (IsAValidXML(resultFileInfo.FullName))
                                                        writer.WriteElementString("result", resultFile);
                                                }
											}
										}
									}                        
									writer.WriteEndElement ();
								}
							}
						}
						writer.WriteEndElement ();
					} 
					else if (session is DvtkApplicationLayer.EmulatorSession) 
					{
						DvtkApplicationLayer.EmulatorSession emulatorSession = session as EmulatorSession;

						writer.WriteStartElement ("Sop");
						writer.WriteElementString ("Sopname", emulatorSession.SessionFileName); 

						if (emulatorSession.Emulators.Count !=0) 
						{
							foreach ( Emulator emulator in emulatorSession.Emulators) 
							{
								writer.WriteStartElement ("file");

								writer.WriteElementString ("filename", emulator.EmulatorName);
            
								if (emulator.Result.Count !=0) 
								{
									foreach (Result result in emulator.Result) 
									{
										if ((result.SummaryFile != "") && (result.SummaryFile != null))
										{			
											string resultFile = Path.Combine(emulatorSession.ResultsRootDirectory, result.SummaryFile);
											FileInfo resultFileInfo = new FileInfo(resultFile);
                                            if (resultFileInfo.Exists)
                                            {
                                                if (IsAValidXML(resultFileInfo.FullName))
                                                    writer.WriteElementString("result", resultFile);
                                            }
										}
		
										foreach (string subsummaryfile in result.SubSummaryResultFiles) 
										{
											if ((subsummaryfile != "") && (subsummaryfile != null))
											{
												string resultFile = Path.Combine(emulatorSession.ResultsRootDirectory, subsummaryfile);
												FileInfo resultFileInfo = new FileInfo(resultFile);
                                                if (resultFileInfo.Exists)
                                                {
                                                    if (IsAValidXML(resultFileInfo.FullName))
                                                        writer.WriteElementString("result", resultFile);
                                                }
											}
										}
									}
								}
								writer.WriteEndElement ();
							}
						}
						writer.WriteEndElement ();
					} 
					else 
					{ 
						DvtkApplicationLayer.ScriptSession scriptSession = session as ScriptSession;

						writer.WriteStartElement ("Sop");
						writer.WriteElementString ("Sopname", scriptSession.SessionFileName); 

						if (scriptSession.ScriptFiles.Count !=0) 
						{
							foreach ( Script script in scriptSession.ScriptFiles) 
							{
								if (((_MainForm._UserSettings.ShowDicomScripts) && (script.ScriptFileName.IndexOf(".ds") != -1))
									|| ((_MainForm._UserSettings.ShowDicomSuperScripts) && (script.ScriptFileName.IndexOf(".dss") != -1))
									|| ((_MainForm._UserSettings.ShowVisualBasicScripts) && (script.ScriptFileName.IndexOf(".vbs") != -1)))
								{
									writer.WriteStartElement ("file");

									writer.WriteElementString ("filename", script.ScriptFileName);
						
									if (script.Result.Count !=0) 
									{
										foreach (Result result in script.Result) 
										{	
											if ((result.SummaryFile != "") && (result.SummaryFile != null))
											{
												string resultFile = Path.Combine(scriptSession.ResultsRootDirectory, result.SummaryFile);
												FileInfo resultFileInfo = new FileInfo(resultFile);
                                                if (resultFileInfo.Exists)
                                                {
                                                    if (IsAValidXML(resultFileInfo.FullName))
                                                        writer.WriteElementString("result", resultFile);
                                                }
											}
							
											foreach (string subsummaryfile in result.SubSummaryResultFiles) 
											{	
												if ((subsummaryfile != "") && (subsummaryfile != null))
												{
													string resultFile = Path.Combine(scriptSession.ResultsRootDirectory, subsummaryfile);
													FileInfo resultFileInfo = new FileInfo(resultFile);
                                                    if (resultFileInfo.Exists)
                                                    {
                                                        if (IsAValidXML(resultFileInfo.FullName))
                                                            writer.WriteElementString("result", resultFile);
                                                    }
												}												
											}
										}
									}
									writer.WriteEndElement ();
								}
							}
						}
						writer.WriteEndElement ();
					}
				}
				writer.WriteEndElement ();
				writer.WriteElementString ("Directory", Path.GetDirectoryName(projectApp.ProjectFileName));
				writer.WriteElementString ("FileName" , projectApp.ProjectFileName + ".xml" );
				writer.WriteEndElement ();

				// End the document
				writer.WriteEndDocument ();

				// Close Flushes the document to the stream
				if (writer != null) writer.Close();
			}
			catch ( Exception projException)
			{ 
				MessageBox.Show(projException.Message);
			}
		}

        private bool IsAValidXML(string xmlFile)
        {
            bool valid = true;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFile);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Skipping the result file {0} due to " + ex.Message, xmlFile);
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                valid = false;
            }
            return valid;
        }

        private void ContextMenu_UnSelectAllDefinitionFiles_Click(object sender, System.EventArgs e)
		{
			_SopClassesManager.UnSelectAllDefinitionFiles();		
		}

		private void ContextMenu_SelectAllDefinitionFiles_Click(object sender, System.EventArgs e)
		{
			_SopClassesManager.SelectAllDefinitionFiles();
		}

		private void ButtonSelectAllDefinitionFiles_Click(object sender, System.EventArgs e)
		{
			_SopClassesManager.SelectAllDefinitionFiles();
		}

		private void ButtonUnselectAll_Click(object sender, System.EventArgs e)
		{
			_SopClassesManager.UnSelectAllDefinitionFiles();
		}

		private void ContextMenu_OpenDefFile_Click(object sender, System.EventArgs e)
		{	
			_SopClassesManager.OpenDefinitionFile();
		}

		private void ProjectForm2_Load(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.Default;
            
		}

		private void PictureBoxResultsManager_Click(object sender, System.EventArgs e)
		{
			this.TabControlIssues.Visible = false ;
			PictureBoxMinResultsTab.Visible = false ;
			PictureBoxMaximizeResultTab.Visible = true ;		
		}

		private void PictureBoxMaximizeResultTab_Click(object sender, System.EventArgs e)
		{
			this.TabControlIssues.Visible = true ;
			PictureBoxMinResultsTab.Visible = true ;
			PictureBoxMaximizeResultTab.Visible = false ;
		}		

		private void ButtonAddIssue_Click(object sender, System.EventArgs e)
		{
			Session session = GetSession();
			TreeNode selectedNode = GetSelectedUserNode();

			string resultFileNamePath = Path.GetFileName(resultFileNameForAddIssue);
			int resultFileNameIndex = resultFileNamePath.IndexOf("*",0);
			string resultFilename;
			if(resultFileNameIndex != -1)
			{
				resultFilename = resultFileNamePath.Substring(0,resultFileNameIndex-1);
			}
			else
			{
				resultFilename = resultFileNamePath;
			}

			if (resultFilename.StartsWith("detail"))
			{
				resultFilename = resultFilename.Replace("detail" ,"summary");
			}
			else if (resultFilename.StartsWith("Detail"))
			{
				resultFilename = resultFilename.Replace("Detail" ,"Summary");
			}
			string fileName = session.ResultsRootDirectory + resultFilename ;
			name = "";
			string comment = TextBoxAddComent.Text ;
			string prDetails = TextBoxAddPR.Text ;
			string textType = "added" ;
			string msg = TextBoxAddMessageText.Text;
			name = fileName.Replace(".html" , ".xml") + " * " + msg;
			ReadAddXml(name, msg, comment, prDetails, textType);
			
			this.TextBoxAddComent.Text = "";
			this.TextBoxAddMessageText.Text = "";
			this.TextBoxAddPR.Text = "";	
			this.LabelErrorMessage.Text = "";
		}

		private void ReplaceOnIssueClick(string name , string msg, string comment , string prDetails , string textType)
		{
			try 
			{
				FileInfo resultFileInfo = new FileInfo(resultProjectXml);
				if (resultFileInfo.Exists) 
				{	
					XmlDocument xmldoc = new XmlDocument();
					try 
					{
						xmldoc.Load(resultFileInfo.FullName);
						XmlTextWriter tempWriter = new XmlTextWriter(resultFileInfo.FullName , System.Text.Encoding.UTF8);
						tempWriter.Formatting = Formatting.Indented;
						XmlNode root = xmldoc.DocumentElement;
						XmlNodeList nodeToRemove = null ;
						nodeToRemove = root.SelectNodes("/Nodes//Name");
						foreach (XmlNode nodeRemove in nodeToRemove)
						{	
							//if (nodeRemove.InnerText == name )
							if (nodeRemove.InnerText.IndexOf(msg) != -1)
							{	
								XmlNode tempParentNode = nodeRemove.ParentNode.ParentNode ;
								XmlNode tempNode =  nodeRemove.ParentNode ;
								tempNode.RemoveAll();
								tempParentNode.RemoveChild(tempNode);
								break;
							}
						}
						XmlNode node = root.SelectSingleNode("/Nodes");
						XmlNodeList nodeType = node.ChildNodes ;
						XmlNode tempMessageNode = null;
						foreach (XmlNode message in nodeType)
						{
							if (message.Name == textType)
							{
								tempMessageNode = message ;
								break ;
							}
						} 
						if (tempMessageNode == null )
						{
							tempMessageNode = xmldoc.CreateNode(XmlNodeType.Element, textType ,xmldoc.NamespaceURI); 
							root.AppendChild(tempMessageNode);
						}
						XmlNode nodeMessage = xmldoc.CreateNode(XmlNodeType.Element, "Message" ,xmldoc.NamespaceURI); 
						XmlElement elemName = xmldoc.CreateElement("Name");
						elemName.InnerText = name.Trim();
						nodeMessage.AppendChild(elemName);
						XmlElement elemComment = xmldoc.CreateElement("Comment");
						elemComment.InnerText = comment ;
						nodeMessage.AppendChild(elemComment);
						XmlElement elemPrDetails = xmldoc.CreateElement("PRnr");
						elemPrDetails.InnerText = prDetails ;
						nodeMessage.AppendChild(elemPrDetails);
						tempMessageNode.AppendChild(nodeMessage);
						xmldoc.Save(tempWriter);
						// Close Flushes the document to the stream
						if (tempWriter != null) tempWriter.Close();
						if (_MainForm.ToolBarFilterResults.Pushed)
						{
							_MainForm.MainStatusBar.Text = "HTML Content is not up-to-date";
						}
					}
					catch ( Exception asa)
					{
						MessageBox.Show(asa.Message);
						CreateNewXml(name , comment , prDetails , textType );
					}
				} 
				else 
				{
					CreateNewXml(name , comment , prDetails , textType );
				}
			}
			catch ( Exception excep )
			{
				MessageBox.Show(excep.Message);
			}
		}

		private void AddSaveOnIssueClick(string name , string msg, string comment , string prDetails , string textType)
		{
			try 
			{
				FileInfo resultFileInfo = new FileInfo(resultProjectXml);
				if (resultFileInfo.Exists) 
				{	
					XmlDocument xmldoc = new XmlDocument();
					try 
					{
						xmldoc.Load(resultFileInfo.FullName);
						XmlTextWriter tempWriter = new XmlTextWriter(resultFileInfo.FullName , System.Text.Encoding.UTF8);
						tempWriter.Formatting = Formatting.Indented;
						XmlNode root = xmldoc.DocumentElement;
						XmlNodeList nodeToRemove = null ;
						nodeToRemove = root.SelectNodes("/Nodes//Name");
						foreach (XmlNode nodeRemove in nodeToRemove)
						{	
							//if (nodeRemove.InnerText == name )
							if (nodeRemove.InnerText.IndexOf(msg) != -1)
							{	
								XmlNode tempParentNode = nodeRemove.ParentNode.ParentNode ;
								XmlNode tempNode =  nodeRemove.ParentNode ;
								tempNode.RemoveAll();
								tempParentNode.RemoveChild(tempNode);
							}
						}
						XmlNode node = root.SelectSingleNode("/Nodes");
						XmlNode tempMessageNode = null;
						
						XmlNodeList nodeType = node.ChildNodes ;
						
						foreach (XmlNode message in nodeType)
						{
							if (message.Name == textType)
							{
								tempMessageNode = message ;
								break ;
							}
						} 							
						if (tempMessageNode == null )
						{
							tempMessageNode = xmldoc.CreateNode(XmlNodeType.Element, textType ,xmldoc.NamespaceURI); 
							root.AppendChild(tempMessageNode);
						}

						XmlNode nodeMessage = xmldoc.CreateNode(XmlNodeType.Element, "Message" ,xmldoc.NamespaceURI); 
						XmlElement elemName = xmldoc.CreateElement("Name");
						elemName.InnerText = name.Trim();
						nodeMessage.AppendChild(elemName);
						XmlElement elemComment = xmldoc.CreateElement("Comment");
						elemComment.InnerText = comment ;
						nodeMessage.AppendChild(elemComment);
						XmlElement elemPrDetails = xmldoc.CreateElement("PRnr");
						elemPrDetails.InnerText = prDetails ;
						nodeMessage.AppendChild(elemPrDetails);
						tempMessageNode.AppendChild(nodeMessage);
						xmldoc.Save(tempWriter);
						// Close Flushes the document to the stream
						if (tempWriter != null) tempWriter.Close();
						if (_MainForm.ToolBarFilterResults.Pushed)
						{
							_MainForm.MainStatusBar.Text = "HTML Content is not up-to-date";
						}
					}
					catch ( Exception asa)
					{
						MessageBox.Show(asa.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						CreateNewXml(name , comment , prDetails , textType );
					}
				} 
				else 
				{
					CreateNewXml(name , comment , prDetails , textType );
				}
			}
			catch ( Exception excep )
			{
				MessageBox.Show(excep.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void checkBoxIgnoreResultAll_Click(object sender, System.EventArgs e)
		{
			if(this.checkBoxIgnoreResultAll.Checked )
			{
				this.checkBoxIgnoreResult.Enabled = false;
				this.checkBoxIgnoreResult.Checked = false;
			}
			else 
			{
				this.checkBoxIgnoreResult.Enabled = true;
			}
		}

		private void checkBoxIgnoreResult_Click(object sender, System.EventArgs e)
		{
			if (this.checkBoxIgnoreResult.Checked)
			{
				this.checkBoxIgnoreResultAll.Enabled = false;
				this.checkBoxIgnoreResultAll.Checked = false;
			}
			else 
			{
				this.checkBoxIgnoreResultAll.Enabled = true;
			}		
		}

		private void TextBoxAddMessageText_Leave(object sender, System.EventArgs e)
		{
			string text = this.TextBoxAddMessageText.Text ;
			if(text.IndexOf("#") >0)
			{
				MessageBox.Show("Not Allowed to add Text Message with  '#' in it .Please add another Text Message.", "Warning");
				this.TextBoxAddMessageText.Text = "";
			}
		}
		
		private void ContextMenu_GenerateDICOMDIRWithDirectory_Click(object sender, System.EventArgs e)
		{
			userControlSessionTree.GenerateDICOMDIRWithDirectory();
		}

		private void ReadAddXml(string name, string msg, string comment, string prDetails, string textType)
		{
			bool bFound = false; 
			FileInfo tempFileInfo = new FileInfo(resultProjectXml);
			if (tempFileInfo.Exists)
			{
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.Load(tempFileInfo.FullName);
				XmlNode root = xmldoc.DocumentElement;
				XmlTextReader tempReader = new XmlTextReader(tempFileInfo.FullName);
				XmlNodeList nodeToRead = null ;
				nodeToRead= root.SelectNodes("/Nodes//Name");
				if (nodeToRead.Count >0 )
				{	
					foreach (XmlNode nodeRead in nodeToRead)
					{
						string tempText = nodeRead.InnerText ;
						if (tempText.Equals(name))
						{
							bFound = true;
							string theWarningText = string.Format("The MessageText  for the AddIssue already exists. Are you sure you want to replace the Message Text");
							DialogResult theDialogResult = MessageBox.Show (this,
								theWarningText,
								"Replace the ErrorMessageText",
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Question,
								MessageBoxDefaultButton.Button2);

							if ( theDialogResult == DialogResult.Yes) 
							{
								ReplaceOnIssueClick(name, msg, comment, prDetails, textType);
							}		
							else
							{
								break;
							}
						}						
					}
					if(false == bFound)
					{
						AddSaveOnIssueClick(name, msg, comment, prDetails, textType);
					}					
				}
				else 
				{
					AddSaveOnIssueClick(name, msg, comment, prDetails, textType);
				}			
				tempReader.Close();
			}
			else 
			{
				CreateNewXml(name , comment , prDetails , textType );
			}
			FilteringResults();
		}

		private void DeleteAddIssue(string name)
		{
			FileInfo tempFileInfo = new FileInfo(resultProjectXml);
			if (tempFileInfo.Exists)
			{
				XmlDocument xmldoc = new XmlDocument();
				xmldoc.Load(tempFileInfo.FullName);
				XmlNode root = xmldoc.DocumentElement;
				XmlTextReader tempReader = new XmlTextReader(tempFileInfo.FullName);
				XmlNodeList nodeToRead = null ;
				nodeToRead= root.SelectNodes("/Nodes//Name");
				foreach (XmlNode nodeRead in nodeToRead)
				{	
					string tempText = nodeRead.InnerText ;
					int startIndex = tempText.IndexOf("*")+1 ;
					int endIndex = tempText.Length;
					string errorMessage = tempText.Substring(startIndex);                    

					if (tempText.ToLower().Equals(name.ToLower()))
					{
						string theWarningText = string.Format("Are you sure you want to delete the error : " + errorMessage);
						DialogResult theDialogResult = MessageBox.Show (this,
							theWarningText,
							"Remove the error.",
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2);

						if ( theDialogResult == DialogResult.Yes) 
						{
							//nodeRead.ParentNode.RemoveAll();
							XmlNode tempParentNode = nodeRead.ParentNode.ParentNode ;
							XmlNode tempNode =  nodeRead.ParentNode ;
							tempNode.RemoveAll();
							tempParentNode.RemoveChild(tempNode);
							xmldoc.Save(tempFileInfo.FullName);							
							tempReader.Close();	
							FilteringResults();			
						}						
						else // in case of dialog result is NO 
						{
							tempReader.Close();
						}
					}					
				}
				//tempReader.Close();				
			}
			this.checkBoxIgnoreResult.Checked = false ;
			this.checkBoxIgnoreResultAll.Checked = false;
			this.TextBoxComment.Text = "";
			this.TextBoxPr.Text = "";
		}

		public void GenerateFilterReport()
		{
			try 
			{
				SaveFileDialog saveAsDlg = new SaveFileDialog();
				saveAsDlg.Title = "Save Report As...";
				saveAsDlg.Filter = "Report file(*.html)|*.html";
				if (saveAsDlg.ShowDialog () == DialogResult.OK)
				{
					FileInfo file = new FileInfo(projectApp.ProjectFileName);
					string projectStructureFile = Path.Combine(file.DirectoryName, Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + ".xml");
					ProjectXmlFile(projectStructureFile);

					string tempFile1 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "1.xml";
					string tempFile2 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "2.xml";
					string tempFile3 = Path.GetFileNameWithoutExtension(projectApp.ProjectFileName) + "3.xml";

					FileInfo fileInfo = new FileInfo(resultProjectXml);
					if (!fileInfo.Exists)
					{
						CreateXmlTemplate();
					}
					if(projectApp.Sessions.Count != 0)
					{
						Convert(Path.GetFileName(projectStructureFile), tempFile1, "createsum.xsl");
						Convert(tempFile1,tempFile2,"sortontype.xsl");
						Convert(tempFile2,tempFile3, "sortonmes.xsl");
					}
					Convert(Path.GetFileName(projectStructureFile),"report.xml","createreport1.xsl");
					Convert("report.xml",saveAsDlg.FileName,"createreport.xsl");				
				}
			}
			catch (Exception except)
			{
				MessageBox.Show(except.Message);
			}
		}		

		private void ProjectForm2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.Focus();
		}

		private void ProjectForm2_Move(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		private void NumericDVTListenPort_ValueChanged(object sender, System.EventArgs e)
		{
			if (_TCM_UpdateCount == 0) 
			{
				userControlSessionTree.Focus();
				DvtkApplicationLayer.Session session = GetSession();

				if (session is DvtkApplicationLayer.ScriptSession) 
				{
                    (session as DvtkApplicationLayer.ScriptSession).DvtPort = (ushort)NumericDVTListenPort.Value;
				}
				else if(session is DvtkApplicationLayer.EmulatorSession) 
				{
                    (session as DvtkApplicationLayer.EmulatorSession).DvtPort = (ushort)NumericDVTListenPort.Value;
				}
				else 
				{
					// Not supposed to get here.
					throw new System.ApplicationException("Error: not expected to get here.");
				}

				SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
				Notify(theSessionChange);
			}		
		}

        private void NumericSessonID_ValueChanged(object sender, EventArgs e)
        {
            // Only react when the user has made changes, not when the TCM_Update method has been called.
            if (_TCM_UpdateCount == 0)
            {
                GetSession().SessionId = System.Convert.ToUInt16(NumericSessonID.Value);

                SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
                Notify(theSessionChange);
            }
        }

        private void ContextMenu_ValidateMediaDirectory_Click(object sender, EventArgs e)
        {
            DirectoryInfo MediaDirectoryInfo = null;
            //_IS_MediaDirectory_BeingValidated = true;
            GetSelectedSessionNew().Implementation.ValidateReferencedFile = true;
            
            FolderBrowserDialog MediaDirectoryDialog = new FolderBrowserDialog();
            MediaDirectoryDialog.ShowNewFolderButton = false;

            MediaDirectoryDialog.Description = "Select the Media Directory to be validated";

            if (MediaDirectoryDialog.ShowDialog(this) == DialogResult.OK)
            {
                MediaDirectoryInfo = new DirectoryInfo(MediaDirectoryDialog.SelectedPath.ToString());
                userControlSessionTree.Execute( MediaDirectoryInfo);
            }
        }

        private void ButtonBrowseDataDirectory_Click(object sender, EventArgs e)
        {
            TheFolderBrowserDialog.Description = "Select the data directory where the data files should be stored.";

            // Only if the current directory exists, set this directory in the dialog browser.
            if (TextBoxDataRoot.Text != "")
            {
                DirectoryInfo theDirectoryInfo = new DirectoryInfo(TextBoxDataRoot.Text);

                if (theDirectoryInfo.Exists)
                {
                    TheFolderBrowserDialog.SelectedPath = TextBoxDataRoot.Text;
                }
            }

            if (TheFolderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                TextBoxDataRoot.ReadOnly = false;
                int index = TheFolderBrowserDialog.SelectedPath.IndexOf("#");
                if (index < 0)
                {
                    if (GetSession().DataDirectory != TheFolderBrowserDialog.SelectedPath)
                    {
                        GetSession().DataDirectory = TheFolderBrowserDialog.SelectedPath;
                        TextBoxDataRoot.Text = TheFolderBrowserDialog.SelectedPath;
                        TextBoxDataRoot.ReadOnly = true;

                        // Notify the rest of the world of the change.
                        SessionChange theSessionChange = new SessionChange(GetSession(), SessionChange.SessionChangeSubTypEnum.OTHER);
                        Notify(theSessionChange);
                    }
                }
                else
                {
                    MessageBox.Show("Data Directory with '#' in it is not allowed . Please select another result directory.", "Warning");
                }
            }
        }
	}
}
