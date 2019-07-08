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
using System.IO;
using System.Windows.Forms;
using Dvtk;
using System.Reflection;

namespace Dvt
{
	/// <summary>
	/// Summary description for WizardNew.
	/// </summary>
    public class WizardNew : System.Windows.Forms.Form
    {
        public System.Windows.Forms.TabControl WizardPages;
        private System.Windows.Forms.TabPage Page1;
        private System.Windows.Forms.TabPage Page2;
        private System.Windows.Forms.TabPage Page3;
        private System.Windows.Forms.Button ButtonPrev;
        private System.Windows.Forms.Button ButtonNext;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label TitlePage1;
        private System.Windows.Forms.RadioButton RadioNewProject;
        private System.Windows.Forms.RadioButton RadioNewSession;
        private System.Windows.Forms.Panel PanelPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonBrowseProject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog DialogOpenProjectFile;
        private System.Windows.Forms.TextBox ProjectFileName;
        private System.Windows.Forms.Button ButtonAddSession;
        private System.Windows.Forms.Button ButtonRemoveSession;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog DialogAddExistingSessions;
        private System.Windows.Forms.ListBox ListBoxSessions;
        private System.Windows.Forms.TabPage Page4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox SessionFileName;
        private System.Windows.Forms.Button ButtonBrowseSession;
        private System.Windows.Forms.OpenFileDialog DialogOpenSessionFile;
        private System.Windows.Forms.TabPage Page5;
        private System.Windows.Forms.TabPage Page6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox ComboBoxSessionType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage Page7;
        private System.Windows.Forms.TabPage Page8;
        private System.Windows.Forms.TabPage Page9;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.GroupBox GroupSecurityVersion;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.GroupBox GroupSecurityGeneral;
        private System.Windows.Forms.GroupBox GroupSecurityAuthentication;
        private System.Windows.Forms.GroupBox GroupSecurityKeyExchange;
        private System.Windows.Forms.GroupBox GroupSecurityDataIntegrity;
        private System.Windows.Forms.GroupBox GroupSecurityEncryption;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ListBox ListBoxCredentials;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TabPage Page10;
        private System.Windows.Forms.TabPage Page11;
        private System.Windows.Forms.TabPage Page12;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ListBox ListBoxCertificates;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Button ButtonRemoveDefFiles;
        private System.Windows.Forms.Button ButtonAddDefFiles;
        private System.Windows.Forms.ListBox ListBoxDefinitionFiles;
        private System.Windows.Forms.ComboBox ComboBoxAENameVersion;
        private System.Windows.Forms.Label LabelAENameVersion;
        private System.Windows.Forms.CheckBox CheckboxSecureConnection;
        private System.Windows.Forms.ListBox ListBoxSecuritySettings;
        private System.Windows.Forms.CheckBox CheckBoxCacheSecureSessions;
        private System.Windows.Forms.CheckBox CheckBoxKeyExchangeRSA;
        private System.Windows.Forms.CheckBox CheckBoxKeyExchangeDH;
        private System.Windows.Forms.CheckBox CheckBoxAuthenticationRSA;
        private System.Windows.Forms.CheckBox CheckBoxAuthenticationDSA;
        private System.Windows.Forms.CheckBox CheckBoxTLS;
        private System.Windows.Forms.CheckBox CheckBoxSSL;
        private System.Windows.Forms.CheckBox CheckBoxEncryptionNone;
        private System.Windows.Forms.CheckBox CheckBoxEncryptionTripleDES;
        private System.Windows.Forms.CheckBox CheckBoxEncryptionAES128;
        private System.Windows.Forms.CheckBox CheckBoxEncryptionAES256;
        private System.Windows.Forms.CheckBox CheckBoxDataIntegritySHA;
        private System.Windows.Forms.CheckBox CheckBoxDataIntegrityMD5;
        private System.Windows.Forms.Label LabelSelect1ItemMsg;
        private System.Windows.Forms.TextBox TextBoxDVTAeTitle;
        private System.Windows.Forms.TextBox TextBoxTCPIP;
        private System.Windows.Forms.TextBox TextBoxSUTAETitle;
        private System.Windows.Forms.Button ButtonRemoveCredentials;
        private System.Windows.Forms.Button ButtonAddCredentials;
        private System.Windows.Forms.OpenFileDialog DialogAddDefinitionFiles;
        private System.Windows.Forms.OpenFileDialog DialogAddCredentialFiles;
        private System.Windows.Forms.Button ButtonRemoveCertificates;
        private System.Windows.Forms.Button ButtonAddCertificates;
        private System.Windows.Forms.TextBox TextBoxDefinitionRoot;
        private System.Windows.Forms.Button ButtonBrowseScriptRoot;
        private System.Windows.Forms.Button ButtonBrowseResultsRoot;
        private System.Windows.Forms.Button ButtonBrowseDefinitionRoot;
        private System.Windows.Forms.TextBox TextBoxResultsRoot;
        private System.Windows.Forms.TextBox TextBoxScriptRoot;
        private System.Windows.Forms.FolderBrowserDialog DialogBrowseFolder;
        private System.Windows.Forms.TextBox TextBoxUserName;
        private System.Windows.Forms.TextBox TextBoxSessionTitle;
        //private System.Windows.Forms.DateTimePicker DateSession;
        private System.Windows.Forms.NumericUpDown NumericSocketTimeOut;
        private System.Windows.Forms.NumericUpDown NumericDVTListenPort;
        private System.Windows.Forms.NumericUpDown NumericSUTListenPort;
        private System.Windows.Forms.NumericUpDown NumericDVTPDULength;
        private System.Windows.Forms.NumericUpDown NumericSUTPDULength;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox CheckBoxCheckRemoteCertificates;
		private System.Windows.Forms.Label label9;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public WizardNew(StartPage page_nr)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.start_page = page_nr;

            switch (page_nr)
            {
                case StartPage.all:
                    this.current_page = 1;
					ActiveControl = RadioNewProject;
                    break;

                case StartPage.project:
                    this.current_page = 2;
					if (this.ProjectFileName.Text != "")
					{
						this.ButtonNext.Enabled = true;
					}
					else
					{
						this.ButtonNext.Enabled = false;
					}
                    this.WizardPages.SelectedTab = this.Page2;
					ActiveControl = ButtonBrowseProject;
                    break;

                case StartPage.session:
                    this.current_page = 4;
					if (this.SessionFileName.Text != "")
					{
						this.ButtonNext.Enabled = true;
					}
					else
					{
						this.ButtonNext.Enabled = false;
					}
                    this.WizardPages.SelectedTab = this.Page4;
					ActiveControl = ButtonBrowseSession;
                    break;
            }
            // Initialize Page 2
            // Initialize Page 3
            this.created_project = false;
            // Initialize Page 4
            // Initialize Page 5
            this.TextBoxUserName.Text = SystemInformation.UserName;
            //this.DateSession.Value = DateTime.Today;
            // Initialize Page 6
            // Initialize Page 7
            // Initialize Page 8
            this.ListBoxSecuritySettings.SelectedIndex = 0;
            // Initialize Page 9
            // Initialize Page 10
            // Initialize Page 11
            // Initialize Page 12
            this.created_session = false;
        }

        public bool has_created_project
        {
            get { return this.created_project; }
        }
        public bool has_created_session
        {
            get { return this.created_session; }
        }

        public void ConstructAndSaveProject(DvtkApplicationLayer.Project theProject)
        {
			theProject.display_message = new DvtkApplicationLayer.Project.CallBackMessageDisplay (this.CallBackMessageDisplay);

			theProject.New(this.ProjectFileName.Text);

			foreach (object session in this.ListBoxSessions.Items)
			{
				theProject.AddSession(session.ToString());
			}

			theProject.SaveProject();
        }

        public string GetSession()
        {
            return this.SessionFileName.Text;
        }

        public enum StartPage : byte { project, session, all };

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

        private     int                     current_page;
        private     bool                    created_project;
        private     bool                    created_session;
        private     StartPage               start_page;
        private     DvtkApplicationLayer.Session sessionApp ;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WizardPages = new System.Windows.Forms.TabControl();
            this.Page1 = new System.Windows.Forms.TabPage();
            this.PanelPage1 = new System.Windows.Forms.Panel();
            this.TitlePage1 = new System.Windows.Forms.Label();
            this.RadioNewSession = new System.Windows.Forms.RadioButton();
            this.RadioNewProject = new System.Windows.Forms.RadioButton();
            this.Page9 = new System.Windows.Forms.TabPage();
            this.label31 = new System.Windows.Forms.Label();
            this.ButtonRemoveCredentials = new System.Windows.Forms.Button();
            this.ButtonAddCredentials = new System.Windows.Forms.Button();
            this.ListBoxCredentials = new System.Windows.Forms.ListBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label30 = new System.Windows.Forms.Label();
            this.Page4 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.SessionFileName = new System.Windows.Forms.TextBox();
            this.ButtonBrowseSession = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.Page5 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.TextBoxUserName = new System.Windows.Forms.TextBox();
            this.TextBoxSessionTitle = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ComboBoxSessionType = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.Page11 = new System.Windows.Forms.TabPage();
            this.label34 = new System.Windows.Forms.Label();
            this.TextBoxDefinitionRoot = new System.Windows.Forms.TextBox();
            this.ButtonBrowseScriptRoot = new System.Windows.Forms.Button();
            this.ButtonBrowseResultsRoot = new System.Windows.Forms.Button();
            this.ButtonBrowseDefinitionRoot = new System.Windows.Forms.Button();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label33 = new System.Windows.Forms.Label();
            this.TextBoxResultsRoot = new System.Windows.Forms.TextBox();
            this.TextBoxScriptRoot = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.Page2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.ProjectFileName = new System.Windows.Forms.TextBox();
            this.ButtonBrowseProject = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Page3 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ButtonRemoveSession = new System.Windows.Forms.Button();
            this.ButtonAddSession = new System.Windows.Forms.Button();
            this.ListBoxSessions = new System.Windows.Forms.ListBox();
            this.Page7 = new System.Windows.Forms.TabPage();
            this.NumericSUTListenPort = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.CheckboxSecureConnection = new System.Windows.Forms.CheckBox();
            this.TextBoxTCPIP = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.TextBoxSUTAETitle = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.NumericSUTPDULength = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.Page10 = new System.Windows.Forms.TabPage();
            this.ButtonRemoveCertificates = new System.Windows.Forms.Button();
            this.ButtonAddCertificates = new System.Windows.Forms.Button();
            this.ListBoxCertificates = new System.Windows.Forms.ListBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label32 = new System.Windows.Forms.Label();
            this.Page6 = new System.Windows.Forms.TabPage();
            this.NumericDVTPDULength = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.TextBoxDVTAeTitle = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.NumericSocketTimeOut = new System.Windows.Forms.NumericUpDown();
            this.NumericDVTListenPort = new System.Windows.Forms.NumericUpDown();
            this.Page8 = new System.Windows.Forms.TabPage();
            this.LabelSelect1ItemMsg = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.ListBoxSecuritySettings = new System.Windows.Forms.ListBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label26 = new System.Windows.Forms.Label();
            this.GroupSecurityVersion = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.CheckBoxTLS = new System.Windows.Forms.CheckBox();
            this.CheckBoxSSL = new System.Windows.Forms.CheckBox();
            this.GroupSecurityAuthentication = new System.Windows.Forms.GroupBox();
            this.CheckBoxAuthenticationDSA = new System.Windows.Forms.CheckBox();
            this.CheckBoxAuthenticationRSA = new System.Windows.Forms.CheckBox();
            this.GroupSecurityKeyExchange = new System.Windows.Forms.GroupBox();
            this.CheckBoxKeyExchangeRSA = new System.Windows.Forms.CheckBox();
            this.CheckBoxKeyExchangeDH = new System.Windows.Forms.CheckBox();
            this.GroupSecurityDataIntegrity = new System.Windows.Forms.GroupBox();
            this.CheckBoxDataIntegritySHA = new System.Windows.Forms.CheckBox();
            this.CheckBoxDataIntegrityMD5 = new System.Windows.Forms.CheckBox();
            this.GroupSecurityEncryption = new System.Windows.Forms.GroupBox();
            this.CheckBoxEncryptionNone = new System.Windows.Forms.CheckBox();
            this.CheckBoxEncryptionTripleDES = new System.Windows.Forms.CheckBox();
            this.CheckBoxEncryptionAES128 = new System.Windows.Forms.CheckBox();
            this.CheckBoxEncryptionAES256 = new System.Windows.Forms.CheckBox();
            this.GroupSecurityGeneral = new System.Windows.Forms.GroupBox();
            this.CheckBoxCheckRemoteCertificates = new System.Windows.Forms.CheckBox();
            this.CheckBoxCacheSecureSessions = new System.Windows.Forms.CheckBox();
            this.Page12 = new System.Windows.Forms.TabPage();
            this.ComboBoxAENameVersion = new System.Windows.Forms.ComboBox();
            this.LabelAENameVersion = new System.Windows.Forms.Label();
            this.ButtonRemoveDefFiles = new System.Windows.Forms.Button();
            this.ButtonAddDefFiles = new System.Windows.Forms.Button();
            this.ListBoxDefinitionFiles = new System.Windows.Forms.ListBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label37 = new System.Windows.Forms.Label();
            this.ButtonPrev = new System.Windows.Forms.Button();
            this.ButtonNext = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DialogOpenProjectFile = new System.Windows.Forms.OpenFileDialog();
            this.DialogAddExistingSessions = new System.Windows.Forms.OpenFileDialog();
            this.DialogOpenSessionFile = new System.Windows.Forms.OpenFileDialog();
            this.DialogBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.DialogAddDefinitionFiles = new System.Windows.Forms.OpenFileDialog();
            this.DialogAddCredentialFiles = new System.Windows.Forms.OpenFileDialog();
            this.WizardPages.SuspendLayout();
            this.Page1.SuspendLayout();
            this.PanelPage1.SuspendLayout();
            this.Page9.SuspendLayout();
            this.panel8.SuspendLayout();
            this.Page4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.Page5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.Page11.SuspendLayout();
            this.panel10.SuspendLayout();
            this.Page2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Page3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.Page7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTListenPort)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTPDULength)).BeginInit();
            this.Page10.SuspendLayout();
            this.panel9.SuspendLayout();
            this.Page6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTPDULength)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSocketTimeOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTListenPort)).BeginInit();
            this.Page8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.GroupSecurityVersion.SuspendLayout();
            this.GroupSecurityAuthentication.SuspendLayout();
            this.GroupSecurityKeyExchange.SuspendLayout();
            this.GroupSecurityDataIntegrity.SuspendLayout();
            this.GroupSecurityEncryption.SuspendLayout();
            this.GroupSecurityGeneral.SuspendLayout();
            this.Page12.SuspendLayout();
            this.panel11.SuspendLayout();
            this.SuspendLayout();
            // 
            // WizardPages
            // 
            this.WizardPages.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.WizardPages.Controls.Add(this.Page1);
            this.WizardPages.Controls.Add(this.Page9);
            this.WizardPages.Controls.Add(this.Page4);
            this.WizardPages.Controls.Add(this.Page5);
            this.WizardPages.Controls.Add(this.Page11);
            this.WizardPages.Controls.Add(this.Page2);
            this.WizardPages.Controls.Add(this.Page3);
            this.WizardPages.Controls.Add(this.Page7);
            this.WizardPages.Controls.Add(this.Page10);
            this.WizardPages.Controls.Add(this.Page6);
            this.WizardPages.Controls.Add(this.Page8);
            this.WizardPages.Controls.Add(this.Page12);
            this.WizardPages.ItemSize = new System.Drawing.Size(58, 18);
            this.WizardPages.Location = new System.Drawing.Point(8, -30);
            this.WizardPages.Name = "WizardPages";
            this.WizardPages.SelectedIndex = 0;
            this.WizardPages.Size = new System.Drawing.Size(416, 232);
            this.WizardPages.TabIndex = 0;
            this.WizardPages.TabIndexChanged += new System.EventHandler(this.WizardPages_TabIndexChanged);
            // 
            // Page1
            // 
            this.Page1.Controls.Add(this.PanelPage1);
            this.Page1.Controls.Add(this.RadioNewSession);
            this.Page1.Controls.Add(this.RadioNewProject);
            this.Page1.Location = new System.Drawing.Point(4, 22);
            this.Page1.Name = "Page1";
            this.Page1.Size = new System.Drawing.Size(408, 206);
            this.Page1.TabIndex = 0;
            // 
            // PanelPage1
            // 
            this.PanelPage1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.PanelPage1.Controls.Add(this.TitlePage1);
            this.PanelPage1.Location = new System.Drawing.Point(0, 0);
            this.PanelPage1.Name = "PanelPage1";
            this.PanelPage1.Size = new System.Drawing.Size(408, 40);
            this.PanelPage1.TabIndex = 3;
            // 
            // TitlePage1
            // 
            this.TitlePage1.BackColor = System.Drawing.Color.Transparent;
            this.TitlePage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitlePage1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.TitlePage1.Location = new System.Drawing.Point(8, 8);
            this.TitlePage1.Name = "TitlePage1";
            this.TitlePage1.Size = new System.Drawing.Size(392, 24);
            this.TitlePage1.TabIndex = 0;
            this.TitlePage1.Text = "Create a new Project or Session";
            this.TitlePage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RadioNewSession
            // 
            this.RadioNewSession.Location = new System.Drawing.Point(112, 128);
            this.RadioNewSession.Name = "RadioNewSession";
            this.RadioNewSession.Size = new System.Drawing.Size(192, 24);
            this.RadioNewSession.TabIndex = 2;
            this.RadioNewSession.TabStop = true;
            this.RadioNewSession.Text = "Create new Session";
            this.RadioNewSession.CheckedChanged += new System.EventHandler(this.RadioNewSession_CheckedChanged);
            // 
            // RadioNewProject
            // 
            this.RadioNewProject.Location = new System.Drawing.Point(112, 96);
            this.RadioNewProject.Name = "RadioNewProject";
            this.RadioNewProject.Size = new System.Drawing.Size(192, 24);
            this.RadioNewProject.TabIndex = 1;
            this.RadioNewProject.TabStop = true;
            this.RadioNewProject.Text = "Create new Project";
            this.RadioNewProject.CheckedChanged += new System.EventHandler(this.RadioNewProject_CheckedChanged);
            // 
            // Page9
            // 
            this.Page9.Controls.Add(this.label31);
            this.Page9.Controls.Add(this.ButtonRemoveCredentials);
            this.Page9.Controls.Add(this.ButtonAddCredentials);
            this.Page9.Controls.Add(this.ListBoxCredentials);
            this.Page9.Controls.Add(this.panel8);
            this.Page9.Location = new System.Drawing.Point(4, 22);
            this.Page9.Name = "Page9";
            this.Page9.Size = new System.Drawing.Size(408, 206);
            this.Page9.TabIndex = 8;
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(240, 128);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(160, 16);
            this.label31.TabIndex = 15;
            this.label31.Text = "Credentials are private keys.";
            // 
            // ButtonRemoveCredentials
            // 
            this.ButtonRemoveCredentials.Location = new System.Drawing.Point(240, 88);
            this.ButtonRemoveCredentials.Name = "ButtonRemoveCredentials";
            this.ButtonRemoveCredentials.Size = new System.Drawing.Size(160, 23);
            this.ButtonRemoveCredentials.TabIndex = 3;
            this.ButtonRemoveCredentials.Text = "Remove selected credentials";
            this.ButtonRemoveCredentials.Click += new System.EventHandler(this.ButtonRemoveCredentials_Click);
            // 
            // ButtonAddCredentials
            // 
            this.ButtonAddCredentials.Location = new System.Drawing.Point(240, 56);
            this.ButtonAddCredentials.Name = "ButtonAddCredentials";
            this.ButtonAddCredentials.Size = new System.Drawing.Size(160, 23);
            this.ButtonAddCredentials.TabIndex = 2;
            this.ButtonAddCredentials.Text = "Add credentials";
            this.ButtonAddCredentials.Click += new System.EventHandler(this.ButtonAddCredentials_Click);
            // 
            // ListBoxCredentials
            // 
            this.ListBoxCredentials.HorizontalScrollbar = true;
            this.ListBoxCredentials.Location = new System.Drawing.Point(8, 56);
            this.ListBoxCredentials.Name = "ListBoxCredentials";
            this.ListBoxCredentials.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ListBoxCredentials.Size = new System.Drawing.Size(216, 147);
            this.ListBoxCredentials.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel8.Controls.Add(this.label30);
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(408, 40);
            this.panel8.TabIndex = 11;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label30.Location = new System.Drawing.Point(8, 8);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(392, 24);
            this.label30.TabIndex = 0;
            this.label30.Text = "Add credentials";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Page4
            // 
            this.Page4.Controls.Add(this.label7);
            this.Page4.Controls.Add(this.SessionFileName);
            this.Page4.Controls.Add(this.ButtonBrowseSession);
            this.Page4.Controls.Add(this.panel3);
            this.Page4.Location = new System.Drawing.Point(4, 22);
            this.Page4.Name = "Page4";
            this.Page4.Size = new System.Drawing.Size(408, 206);
            this.Page4.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(320, 23);
            this.label7.TabIndex = 10;
            this.label7.Text = "Create the session file:";
            // 
            // SessionFileName
            // 
            this.SessionFileName.Location = new System.Drawing.Point(16, 112);
            this.SessionFileName.Name = "SessionFileName";
            this.SessionFileName.ReadOnly = true;
            this.SessionFileName.Size = new System.Drawing.Size(288, 20);
            this.SessionFileName.TabIndex = 1;
            this.SessionFileName.TabStop = false;
            // 
            // ButtonBrowseSession
            // 
            this.ButtonBrowseSession.Location = new System.Drawing.Point(312, 112);
            this.ButtonBrowseSession.Name = "ButtonBrowseSession";
            this.ButtonBrowseSession.Size = new System.Drawing.Size(75, 23);
            this.ButtonBrowseSession.TabIndex = 2;
            this.ButtonBrowseSession.Text = "Browse";
            this.ButtonBrowseSession.Click += new System.EventHandler(this.ButtonBrowseSession_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(408, 40);
            this.panel3.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Location = new System.Drawing.Point(8, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(392, 24);
            this.label6.TabIndex = 0;
            this.label6.Text = "Create a new session";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Page5
            // 
            this.Page5.Controls.Add(this.label9);
            this.Page5.Controls.Add(this.TextBoxUserName);
            this.Page5.Controls.Add(this.TextBoxSessionTitle);
            this.Page5.Controls.Add(this.label12);
            this.Page5.Controls.Add(this.label10);
            this.Page5.Controls.Add(this.ComboBoxSessionType);
            this.Page5.Controls.Add(this.panel4);
            this.Page5.Location = new System.Drawing.Point(4, 22);
            this.Page5.Name = "Page5";
            this.Page5.Size = new System.Drawing.Size(408, 206);
            this.Page5.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(16, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 23);
            this.label9.TabIndex = 13;
            this.label9.Text = "Session type:";
            // 
            // TextBoxUserName
            // 
            this.TextBoxUserName.Location = new System.Drawing.Point(144, 128);
            this.TextBoxUserName.Name = "TextBoxUserName";
            this.TextBoxUserName.Size = new System.Drawing.Size(248, 20);
            this.TextBoxUserName.TabIndex = 3;
            // 
            // TextBoxSessionTitle
            // 
            this.TextBoxSessionTitle.Location = new System.Drawing.Point(144, 88);
            this.TextBoxSessionTitle.Name = "TextBoxSessionTitle";
            this.TextBoxSessionTitle.Size = new System.Drawing.Size(248, 20);
            this.TextBoxSessionTitle.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(16, 128);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 23);
            this.label12.TabIndex = 12;
            this.label12.Text = "Tested by:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(16, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 23);
            this.label10.TabIndex = 10;
            this.label10.Text = "Session title:";
            // 
            // ComboBoxSessionType
            // 
            this.ComboBoxSessionType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ComboBoxSessionType.Items.AddRange(new object[] {
            "Script",
            "Media",
            "Emulator"});
            this.ComboBoxSessionType.Location = new System.Drawing.Point(144, 48);
            this.ComboBoxSessionType.Name = "ComboBoxSessionType";
            this.ComboBoxSessionType.Size = new System.Drawing.Size(248, 21);
            this.ComboBoxSessionType.TabIndex = 1;
            this.ComboBoxSessionType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ComboBoxSessionType_KeyPress);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel4.Controls.Add(this.label8);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(408, 40);
            this.panel4.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Location = new System.Drawing.Point(8, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(392, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = "Session properties";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Page11
            // 
            this.Page11.Controls.Add(this.label34);
            this.Page11.Controls.Add(this.TextBoxDefinitionRoot);
            this.Page11.Controls.Add(this.ButtonBrowseScriptRoot);
            this.Page11.Controls.Add(this.ButtonBrowseResultsRoot);
            this.Page11.Controls.Add(this.ButtonBrowseDefinitionRoot);
            this.Page11.Controls.Add(this.panel10);
            this.Page11.Controls.Add(this.TextBoxResultsRoot);
            this.Page11.Controls.Add(this.TextBoxScriptRoot);
            this.Page11.Controls.Add(this.label35);
            this.Page11.Controls.Add(this.label36);
            this.Page11.Location = new System.Drawing.Point(4, 22);
            this.Page11.Name = "Page11";
            this.Page11.Size = new System.Drawing.Size(408, 206);
            this.Page11.TabIndex = 10;
            this.Page11.Text = "`";
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(8, 64);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(136, 23);
            this.label34.TabIndex = 10;
            this.label34.Text = "Definition file directory:";
            // 
            // TextBoxDefinitionRoot
            // 
            this.TextBoxDefinitionRoot.Location = new System.Drawing.Point(144, 64);
            this.TextBoxDefinitionRoot.Name = "TextBoxDefinitionRoot";
            this.TextBoxDefinitionRoot.ReadOnly = true;
            this.TextBoxDefinitionRoot.Size = new System.Drawing.Size(168, 20);
            this.TextBoxDefinitionRoot.TabIndex = 1;
            this.TextBoxDefinitionRoot.TabStop = false;
            this.TextBoxDefinitionRoot.Text = ".";
            // 
            // ButtonBrowseScriptRoot
            // 
            this.ButtonBrowseScriptRoot.Location = new System.Drawing.Point(328, 160);
            this.ButtonBrowseScriptRoot.Name = "ButtonBrowseScriptRoot";
            this.ButtonBrowseScriptRoot.Size = new System.Drawing.Size(75, 23);
            this.ButtonBrowseScriptRoot.TabIndex = 6;
            this.ButtonBrowseScriptRoot.Text = "Browse";
            this.ButtonBrowseScriptRoot.Click += new System.EventHandler(this.ButtonBrowseScriptRoot_Click);
            // 
            // ButtonBrowseResultsRoot
            // 
            this.ButtonBrowseResultsRoot.Location = new System.Drawing.Point(328, 112);
            this.ButtonBrowseResultsRoot.Name = "ButtonBrowseResultsRoot";
            this.ButtonBrowseResultsRoot.Size = new System.Drawing.Size(75, 23);
            this.ButtonBrowseResultsRoot.TabIndex = 4;
            this.ButtonBrowseResultsRoot.Text = "Browse";
            this.ButtonBrowseResultsRoot.Click += new System.EventHandler(this.ButtonBrowseResultsRoot_Click);
            // 
            // ButtonBrowseDefinitionRoot
            // 
            this.ButtonBrowseDefinitionRoot.Location = new System.Drawing.Point(328, 64);
            this.ButtonBrowseDefinitionRoot.Name = "ButtonBrowseDefinitionRoot";
            this.ButtonBrowseDefinitionRoot.Size = new System.Drawing.Size(75, 23);
            this.ButtonBrowseDefinitionRoot.TabIndex = 2;
            this.ButtonBrowseDefinitionRoot.Text = "Browse";
            this.ButtonBrowseDefinitionRoot.Click += new System.EventHandler(this.ButtonBrowseDefinitionRoot_Click);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel10.Controls.Add(this.label33);
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(408, 40);
            this.panel10.TabIndex = 5;
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label33.Location = new System.Drawing.Point(8, 8);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(392, 24);
            this.label33.TabIndex = 0;
            this.label33.Text = "Environment settings";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxResultsRoot
            // 
            this.TextBoxResultsRoot.Location = new System.Drawing.Point(144, 112);
            this.TextBoxResultsRoot.Name = "TextBoxResultsRoot";
            this.TextBoxResultsRoot.ReadOnly = true;
            this.TextBoxResultsRoot.Size = new System.Drawing.Size(168, 20);
            this.TextBoxResultsRoot.TabIndex = 3;
            this.TextBoxResultsRoot.TabStop = false;
            this.TextBoxResultsRoot.Text = ".";
            // 
            // TextBoxScriptRoot
            // 
            this.TextBoxScriptRoot.Location = new System.Drawing.Point(144, 160);
            this.TextBoxScriptRoot.Name = "TextBoxScriptRoot";
            this.TextBoxScriptRoot.ReadOnly = true;
            this.TextBoxScriptRoot.Size = new System.Drawing.Size(168, 20);
            this.TextBoxScriptRoot.TabIndex = 5;
            this.TextBoxScriptRoot.TabStop = false;
            this.TextBoxScriptRoot.Text = ".";
            // 
            // label35
            // 
            this.label35.Location = new System.Drawing.Point(8, 112);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(136, 23);
            this.label35.TabIndex = 10;
            this.label35.Text = "Results file directory:";
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(8, 160);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(136, 23);
            this.label36.TabIndex = 10;
            this.label36.Text = "Scripts directory:";
            // 
            // Page2
            // 
            this.Page2.Controls.Add(this.label2);
            this.Page2.Controls.Add(this.ProjectFileName);
            this.Page2.Controls.Add(this.ButtonBrowseProject);
            this.Page2.Controls.Add(this.panel1);
            this.Page2.Location = new System.Drawing.Point(4, 22);
            this.Page2.Name = "Page2";
            this.Page2.Size = new System.Drawing.Size(408, 206);
            this.Page2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(320, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Create the project file:";
            // 
            // ProjectFileName
            // 
            this.ProjectFileName.Location = new System.Drawing.Point(16, 112);
            this.ProjectFileName.Name = "ProjectFileName";
            this.ProjectFileName.ReadOnly = true;
            this.ProjectFileName.Size = new System.Drawing.Size(288, 20);
            this.ProjectFileName.TabIndex = 1;
            this.ProjectFileName.TabStop = false;
            // 
            // ButtonBrowseProject
            // 
            this.ButtonBrowseProject.Location = new System.Drawing.Point(312, 112);
            this.ButtonBrowseProject.Name = "ButtonBrowseProject";
            this.ButtonBrowseProject.Size = new System.Drawing.Size(75, 23);
            this.ButtonBrowseProject.TabIndex = 2;
            this.ButtonBrowseProject.Text = "Browse";
            this.ButtonBrowseProject.Click += new System.EventHandler(this.ButtonBrowseProject_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 40);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(392, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Create a new Project";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Page3
            // 
            this.Page3.Controls.Add(this.label5);
            this.Page3.Controls.Add(this.label4);
            this.Page3.Controls.Add(this.panel2);
            this.Page3.Controls.Add(this.ButtonRemoveSession);
            this.Page3.Controls.Add(this.ButtonAddSession);
            this.Page3.Controls.Add(this.ListBoxSessions);
            this.Page3.Location = new System.Drawing.Point(4, 22);
            this.Page3.Name = "Page3";
            this.Page3.Size = new System.Drawing.Size(408, 206);
            this.Page3.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(248, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 32);
            this.label5.TabIndex = 7;
            this.label5.Text = "In the newly created project, you can add new sessions.";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(248, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 40);
            this.label4.TabIndex = 6;
            this.label4.Text = "To create a new session file, you must first finish this wizard.";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 40);
            this.panel2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(392, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "Add existing session files";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ButtonRemoveSession
            // 
            this.ButtonRemoveSession.Location = new System.Drawing.Point(248, 88);
            this.ButtonRemoveSession.Name = "ButtonRemoveSession";
            this.ButtonRemoveSession.Size = new System.Drawing.Size(152, 23);
            this.ButtonRemoveSession.TabIndex = 3;
            this.ButtonRemoveSession.Text = "Remove selected sessions";
            this.ButtonRemoveSession.Click += new System.EventHandler(this.ButtonRemoveSession_Click);
            // 
            // ButtonAddSession
            // 
            this.ButtonAddSession.Location = new System.Drawing.Point(248, 56);
            this.ButtonAddSession.Name = "ButtonAddSession";
            this.ButtonAddSession.Size = new System.Drawing.Size(152, 23);
            this.ButtonAddSession.TabIndex = 2;
            this.ButtonAddSession.Text = "Add sessions";
            this.ButtonAddSession.Click += new System.EventHandler(this.ButtonAddSession_Click);
            // 
            // ListBoxSessions
            // 
            this.ListBoxSessions.HorizontalScrollbar = true;
            this.ListBoxSessions.Location = new System.Drawing.Point(8, 56);
            this.ListBoxSessions.Name = "ListBoxSessions";
            this.ListBoxSessions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ListBoxSessions.Size = new System.Drawing.Size(224, 147);
            this.ListBoxSessions.TabIndex = 1;
            // 
            // Page7
            // 
            this.Page7.Controls.Add(this.NumericSUTListenPort);
            this.Page7.Controls.Add(this.label25);
            this.Page7.Controls.Add(this.CheckboxSecureConnection);
            this.Page7.Controls.Add(this.TextBoxTCPIP);
            this.Page7.Controls.Add(this.label24);
            this.Page7.Controls.Add(this.label22);
            this.Page7.Controls.Add(this.TextBoxSUTAETitle);
            this.Page7.Controls.Add(this.label23);
            this.Page7.Controls.Add(this.panel6);
            this.Page7.Controls.Add(this.NumericSUTPDULength);
            this.Page7.Controls.Add(this.label11);
            this.Page7.Location = new System.Drawing.Point(4, 22);
            this.Page7.Name = "Page7";
            this.Page7.Size = new System.Drawing.Size(408, 206);
            this.Page7.TabIndex = 6;
            // 
            // NumericSUTListenPort
            // 
            this.NumericSUTListenPort.Location = new System.Drawing.Point(264, 80);
            this.NumericSUTListenPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumericSUTListenPort.Name = "NumericSUTListenPort";
            this.NumericSUTListenPort.Size = new System.Drawing.Size(72, 20);
            this.NumericSUTListenPort.TabIndex = 2;
            this.NumericSUTListenPort.Value = new decimal(new int[] {
            104,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(24, 184);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(344, 23);
            this.label25.TabIndex = 21;
            this.label25.Text = "(When checked, the next page allows you to change the settings.)";
            // 
            // CheckboxSecureConnection
            // 
            this.CheckboxSecureConnection.Location = new System.Drawing.Point(8, 160);
            this.CheckboxSecureConnection.Name = "CheckboxSecureConnection";
            this.CheckboxSecureConnection.Size = new System.Drawing.Size(168, 24);
            this.CheckboxSecureConnection.TabIndex = 5;
            this.CheckboxSecureConnection.Text = "Use secure connection";
            // 
            // TextBoxTCPIP
            // 
            this.TextBoxTCPIP.Location = new System.Drawing.Point(200, 144);
            this.TextBoxTCPIP.MaxLength = 0;
            this.TextBoxTCPIP.Name = "TextBoxTCPIP";
            this.TextBoxTCPIP.Size = new System.Drawing.Size(200, 20);
            this.TextBoxTCPIP.TabIndex = 4;
            this.TextBoxTCPIP.Text = "localhost";
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(8, 144);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(136, 23);
            this.label24.TabIndex = 18;
            this.label24.Text = "Remote TCP/IP address:";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(8, 80);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(100, 23);
            this.label22.TabIndex = 17;
            this.label22.Text = "Listen port:";
            // 
            // TextBoxSUTAETitle
            // 
            this.TextBoxSUTAETitle.Location = new System.Drawing.Point(264, 48);
            this.TextBoxSUTAETitle.Name = "TextBoxSUTAETitle";
            this.TextBoxSUTAETitle.Size = new System.Drawing.Size(136, 20);
            this.TextBoxSUTAETitle.TabIndex = 1;
            this.TextBoxSUTAETitle.Text = "SUT_AE";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(8, 48);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(64, 23);
            this.label23.TabIndex = 14;
            this.label23.Text = "AE title:";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel6.Controls.Add(this.label21);
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(408, 40);
            this.panel6.TabIndex = 9;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label21.Location = new System.Drawing.Point(8, 8);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(392, 24);
            this.label21.TabIndex = 0;
            this.label21.Text = "System Under Test settings";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumericSUTPDULength
            // 
            this.NumericSUTPDULength.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.NumericSUTPDULength.Location = new System.Drawing.Point(264, 112);
            this.NumericSUTPDULength.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.NumericSUTPDULength.Name = "NumericSUTPDULength";
            this.NumericSUTPDULength.Size = new System.Drawing.Size(72, 20);
            this.NumericSUTPDULength.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(8, 112);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(176, 32);
            this.label11.TabIndex = 23;
            this.label11.Text = "Maximum PDU length to receive: (0 = unlimited)";
            // 
            // Page10
            // 
            this.Page10.Controls.Add(this.ButtonRemoveCertificates);
            this.Page10.Controls.Add(this.ButtonAddCertificates);
            this.Page10.Controls.Add(this.ListBoxCertificates);
            this.Page10.Controls.Add(this.panel9);
            this.Page10.Location = new System.Drawing.Point(4, 22);
            this.Page10.Name = "Page10";
            this.Page10.Size = new System.Drawing.Size(408, 206);
            this.Page10.TabIndex = 9;
            // 
            // ButtonRemoveCertificates
            // 
            this.ButtonRemoveCertificates.Location = new System.Drawing.Point(240, 88);
            this.ButtonRemoveCertificates.Name = "ButtonRemoveCertificates";
            this.ButtonRemoveCertificates.Size = new System.Drawing.Size(160, 23);
            this.ButtonRemoveCertificates.TabIndex = 17;
            this.ButtonRemoveCertificates.Text = "Remove selected certificates";
            this.ButtonRemoveCertificates.Click += new System.EventHandler(this.ButtonRemoveCertificates_Click);
            // 
            // ButtonAddCertificates
            // 
            this.ButtonAddCertificates.Location = new System.Drawing.Point(240, 56);
            this.ButtonAddCertificates.Name = "ButtonAddCertificates";
            this.ButtonAddCertificates.Size = new System.Drawing.Size(160, 23);
            this.ButtonAddCertificates.TabIndex = 16;
            this.ButtonAddCertificates.Text = "Add certificates";
            this.ButtonAddCertificates.Click += new System.EventHandler(this.ButtonAddCertificates_Click);
            // 
            // ListBoxCertificates
            // 
            this.ListBoxCertificates.HorizontalScrollbar = true;
            this.ListBoxCertificates.Location = new System.Drawing.Point(8, 56);
            this.ListBoxCertificates.Name = "ListBoxCertificates";
            this.ListBoxCertificates.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ListBoxCertificates.Size = new System.Drawing.Size(216, 147);
            this.ListBoxCertificates.TabIndex = 15;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel9.Controls.Add(this.label32);
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(408, 40);
            this.panel9.TabIndex = 4;
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label32.Location = new System.Drawing.Point(8, 8);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(392, 24);
            this.label32.TabIndex = 0;
            this.label32.Text = "Add trusted certificates";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Page6
            // 
            this.Page6.Controls.Add(this.NumericDVTPDULength);
            this.Page6.Controls.Add(this.label20);
            this.Page6.Controls.Add(this.label19);
            this.Page6.Controls.Add(this.label18);
            this.Page6.Controls.Add(this.label17);
            this.Page6.Controls.Add(this.label16);
            this.Page6.Controls.Add(this.TextBoxDVTAeTitle);
            this.Page6.Controls.Add(this.label15);
            this.Page6.Controls.Add(this.panel5);
            this.Page6.Controls.Add(this.NumericSocketTimeOut);
            this.Page6.Controls.Add(this.NumericDVTListenPort);
            this.Page6.Location = new System.Drawing.Point(4, 22);
            this.Page6.Name = "Page6";
            this.Page6.Size = new System.Drawing.Size(408, 206);
            this.Page6.TabIndex = 5;
            // 
            // NumericDVTPDULength
            // 
            this.NumericDVTPDULength.Increment = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.NumericDVTPDULength.Location = new System.Drawing.Point(264, 144);
            this.NumericDVTPDULength.Maximum = new decimal(new int[] {
            1048576,
            0,
            0,
            0});
            this.NumericDVTPDULength.Name = "NumericDVTPDULength";
            this.NumericDVTPDULength.Size = new System.Drawing.Size(72, 20);
            this.NumericDVTPDULength.TabIndex = 4;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(344, 144);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 23);
            this.label20.TabIndex = 19;
            this.label20.Text = "bytes";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(344, 112);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 23);
            this.label19.TabIndex = 18;
            this.label19.Text = "seconds";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(8, 144);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(176, 32);
            this.label18.TabIndex = 16;
            this.label18.Text = "Maximum PDU length to receive: (0 = unlimited)";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(8, 112);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(100, 23);
            this.label17.TabIndex = 15;
            this.label17.Text = "Socket time-out:";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(8, 80);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 23);
            this.label16.TabIndex = 13;
            this.label16.Text = "Listen port:";
            // 
            // TextBoxDVTAeTitle
            // 
            this.TextBoxDVTAeTitle.Location = new System.Drawing.Point(264, 48);
            this.TextBoxDVTAeTitle.Name = "TextBoxDVTAeTitle";
            this.TextBoxDVTAeTitle.Size = new System.Drawing.Size(136, 20);
            this.TextBoxDVTAeTitle.TabIndex = 1;
            this.TextBoxDVTAeTitle.Text = "DVT_AE";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(8, 48);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 23);
            this.label15.TabIndex = 10;
            this.label15.Text = "AE title:";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel5.Controls.Add(this.label14);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(408, 40);
            this.panel5.TabIndex = 8;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label14.Location = new System.Drawing.Point(8, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(392, 24);
            this.label14.TabIndex = 0;
            this.label14.Text = "DVT Role settings";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumericSocketTimeOut
            // 
            this.NumericSocketTimeOut.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericSocketTimeOut.Location = new System.Drawing.Point(264, 112);
            this.NumericSocketTimeOut.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NumericSocketTimeOut.Name = "NumericSocketTimeOut";
            this.NumericSocketTimeOut.Size = new System.Drawing.Size(72, 20);
            this.NumericSocketTimeOut.TabIndex = 3;
            this.NumericSocketTimeOut.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // NumericDVTListenPort
            // 
            this.NumericDVTListenPort.Location = new System.Drawing.Point(264, 80);
            this.NumericDVTListenPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumericDVTListenPort.Name = "NumericDVTListenPort";
            this.NumericDVTListenPort.Size = new System.Drawing.Size(72, 20);
            this.NumericDVTListenPort.TabIndex = 2;
            this.NumericDVTListenPort.Value = new decimal(new int[] {
            104,
            0,
            0,
            0});
            // 
            // Page8
            // 
            this.Page8.Controls.Add(this.LabelSelect1ItemMsg);
            this.Page8.Controls.Add(this.label28);
            this.Page8.Controls.Add(this.label27);
            this.Page8.Controls.Add(this.ListBoxSecuritySettings);
            this.Page8.Controls.Add(this.panel7);
            this.Page8.Controls.Add(this.GroupSecurityVersion);
            this.Page8.Controls.Add(this.GroupSecurityAuthentication);
            this.Page8.Controls.Add(this.GroupSecurityKeyExchange);
            this.Page8.Controls.Add(this.GroupSecurityDataIntegrity);
            this.Page8.Controls.Add(this.GroupSecurityEncryption);
            this.Page8.Controls.Add(this.GroupSecurityGeneral);
            this.Page8.Location = new System.Drawing.Point(4, 22);
            this.Page8.Name = "Page8";
            this.Page8.Size = new System.Drawing.Size(408, 206);
            this.Page8.TabIndex = 7;
            // 
            // LabelSelect1ItemMsg
            // 
            this.LabelSelect1ItemMsg.ForeColor = System.Drawing.Color.Red;
            this.LabelSelect1ItemMsg.Location = new System.Drawing.Point(232, 48);
            this.LabelSelect1ItemMsg.Name = "LabelSelect1ItemMsg";
            this.LabelSelect1ItemMsg.Size = new System.Drawing.Size(168, 23);
            this.LabelSelect1ItemMsg.TabIndex = 16;
            this.LabelSelect1ItemMsg.Text = "At least 1 item must be selected!";
            this.LabelSelect1ItemMsg.Visible = false;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(176, 48);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(48, 23);
            this.label28.TabIndex = 14;
            this.label28.Text = "Settings:";
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(8, 48);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(100, 23);
            this.label27.TabIndex = 13;
            this.label27.Text = "Categories:";
            // 
            // ListBoxSecuritySettings
            // 
            this.ListBoxSecuritySettings.Items.AddRange(new object[] {
            "General",
            "Version",
            "Authentication",
            "Key Exchange",
            "Data Integrity",
            "Encryption"});
            this.ListBoxSecuritySettings.Location = new System.Drawing.Point(8, 72);
            this.ListBoxSecuritySettings.Name = "ListBoxSecuritySettings";
            this.ListBoxSecuritySettings.Size = new System.Drawing.Size(144, 121);
            this.ListBoxSecuritySettings.TabIndex = 1;
            this.ListBoxSecuritySettings.SelectedIndexChanged += new System.EventHandler(this.ListBoxSecuritySettings_SelectedIndexChanged);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel7.Controls.Add(this.label26);
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(408, 40);
            this.panel7.TabIndex = 10;
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label26.Location = new System.Drawing.Point(8, 8);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(392, 24);
            this.label26.TabIndex = 0;
            this.label26.Text = "Security Settings";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GroupSecurityVersion
            // 
            this.GroupSecurityVersion.Controls.Add(this.label29);
            this.GroupSecurityVersion.Controls.Add(this.CheckBoxTLS);
            this.GroupSecurityVersion.Controls.Add(this.CheckBoxSSL);
            this.GroupSecurityVersion.Location = new System.Drawing.Point(176, 72);
            this.GroupSecurityVersion.Name = "GroupSecurityVersion";
            this.GroupSecurityVersion.Size = new System.Drawing.Size(216, 128);
            this.GroupSecurityVersion.TabIndex = 11;
            this.GroupSecurityVersion.TabStop = false;
            this.GroupSecurityVersion.Text = "Version";
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(16, 80);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(192, 40);
            this.label29.TabIndex = 1;
            this.label29.Text = "When selecting both versions, DVT will attemt to make a connection with either on" +
                "e.";
            // 
            // CheckBoxTLS
            // 
            this.CheckBoxTLS.Checked = true;
            this.CheckBoxTLS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxTLS.Location = new System.Drawing.Point(16, 24);
            this.CheckBoxTLS.Name = "CheckBoxTLS";
            this.CheckBoxTLS.Size = new System.Drawing.Size(64, 24);
            this.CheckBoxTLS.TabIndex = 2;
            this.CheckBoxTLS.Text = "TLS v1";
            this.CheckBoxTLS.CheckedChanged += new System.EventHandler(this.CheckBoxTLS_CheckedChanged);
            // 
            // CheckBoxSSL
            // 
            this.CheckBoxSSL.Location = new System.Drawing.Point(16, 48);
            this.CheckBoxSSL.Name = "CheckBoxSSL";
            this.CheckBoxSSL.Size = new System.Drawing.Size(64, 24);
            this.CheckBoxSSL.TabIndex = 3;
            this.CheckBoxSSL.Text = "SSL v3";
            this.CheckBoxSSL.CheckedChanged += new System.EventHandler(this.CheckBoxSSL_CheckedChanged);
            // 
            // GroupSecurityAuthentication
            // 
            this.GroupSecurityAuthentication.Controls.Add(this.CheckBoxAuthenticationDSA);
            this.GroupSecurityAuthentication.Controls.Add(this.CheckBoxAuthenticationRSA);
            this.GroupSecurityAuthentication.Location = new System.Drawing.Point(176, 72);
            this.GroupSecurityAuthentication.Name = "GroupSecurityAuthentication";
            this.GroupSecurityAuthentication.Size = new System.Drawing.Size(216, 128);
            this.GroupSecurityAuthentication.TabIndex = 13;
            this.GroupSecurityAuthentication.TabStop = false;
            this.GroupSecurityAuthentication.Text = "Authentication";
            // 
            // CheckBoxAuthenticationDSA
            // 
            this.CheckBoxAuthenticationDSA.Location = new System.Drawing.Point(16, 48);
            this.CheckBoxAuthenticationDSA.Name = "CheckBoxAuthenticationDSA";
            this.CheckBoxAuthenticationDSA.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxAuthenticationDSA.TabIndex = 0;
            this.CheckBoxAuthenticationDSA.Text = "DSA";
            this.CheckBoxAuthenticationDSA.CheckedChanged += new System.EventHandler(this.CheckBoxAuthenticationDSA_CheckedChanged);
            // 
            // CheckBoxAuthenticationRSA
            // 
            this.CheckBoxAuthenticationRSA.Checked = true;
            this.CheckBoxAuthenticationRSA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxAuthenticationRSA.Location = new System.Drawing.Point(16, 24);
            this.CheckBoxAuthenticationRSA.Name = "CheckBoxAuthenticationRSA";
            this.CheckBoxAuthenticationRSA.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxAuthenticationRSA.TabIndex = 0;
            this.CheckBoxAuthenticationRSA.Text = "RSA";
            this.CheckBoxAuthenticationRSA.CheckedChanged += new System.EventHandler(this.CheckBoxAuthenticationRSA_CheckedChanged);
            // 
            // GroupSecurityKeyExchange
            // 
            this.GroupSecurityKeyExchange.Controls.Add(this.CheckBoxKeyExchangeRSA);
            this.GroupSecurityKeyExchange.Controls.Add(this.CheckBoxKeyExchangeDH);
            this.GroupSecurityKeyExchange.Location = new System.Drawing.Point(176, 72);
            this.GroupSecurityKeyExchange.Name = "GroupSecurityKeyExchange";
            this.GroupSecurityKeyExchange.Size = new System.Drawing.Size(216, 128);
            this.GroupSecurityKeyExchange.TabIndex = 15;
            this.GroupSecurityKeyExchange.TabStop = false;
            this.GroupSecurityKeyExchange.Text = "Key Exchange";
            // 
            // CheckBoxKeyExchangeRSA
            // 
            this.CheckBoxKeyExchangeRSA.Checked = true;
            this.CheckBoxKeyExchangeRSA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxKeyExchangeRSA.Location = new System.Drawing.Point(16, 24);
            this.CheckBoxKeyExchangeRSA.Name = "CheckBoxKeyExchangeRSA";
            this.CheckBoxKeyExchangeRSA.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxKeyExchangeRSA.TabIndex = 0;
            this.CheckBoxKeyExchangeRSA.Text = "RSA";
            this.CheckBoxKeyExchangeRSA.CheckedChanged += new System.EventHandler(this.CheckBoxKeyExchangeRSA_CheckedChanged);
            // 
            // CheckBoxKeyExchangeDH
            // 
            this.CheckBoxKeyExchangeDH.Location = new System.Drawing.Point(16, 48);
            this.CheckBoxKeyExchangeDH.Name = "CheckBoxKeyExchangeDH";
            this.CheckBoxKeyExchangeDH.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxKeyExchangeDH.TabIndex = 0;
            this.CheckBoxKeyExchangeDH.Text = "DH";
            this.CheckBoxKeyExchangeDH.CheckedChanged += new System.EventHandler(this.CheckBoxKeyExchangeDH_CheckedChanged);
            // 
            // GroupSecurityDataIntegrity
            // 
            this.GroupSecurityDataIntegrity.Controls.Add(this.CheckBoxDataIntegritySHA);
            this.GroupSecurityDataIntegrity.Controls.Add(this.CheckBoxDataIntegrityMD5);
            this.GroupSecurityDataIntegrity.Location = new System.Drawing.Point(176, 72);
            this.GroupSecurityDataIntegrity.Name = "GroupSecurityDataIntegrity";
            this.GroupSecurityDataIntegrity.Size = new System.Drawing.Size(216, 128);
            this.GroupSecurityDataIntegrity.TabIndex = 13;
            this.GroupSecurityDataIntegrity.TabStop = false;
            this.GroupSecurityDataIntegrity.Text = "Data Integrity";
            // 
            // CheckBoxDataIntegritySHA
            // 
            this.CheckBoxDataIntegritySHA.Checked = true;
            this.CheckBoxDataIntegritySHA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxDataIntegritySHA.Location = new System.Drawing.Point(16, 24);
            this.CheckBoxDataIntegritySHA.Name = "CheckBoxDataIntegritySHA";
            this.CheckBoxDataIntegritySHA.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxDataIntegritySHA.TabIndex = 0;
            this.CheckBoxDataIntegritySHA.Text = "SHA";
            this.CheckBoxDataIntegritySHA.CheckedChanged += new System.EventHandler(this.CheckBoxDataIntegritySHA_CheckedChanged);
            // 
            // CheckBoxDataIntegrityMD5
            // 
            this.CheckBoxDataIntegrityMD5.Location = new System.Drawing.Point(16, 48);
            this.CheckBoxDataIntegrityMD5.Name = "CheckBoxDataIntegrityMD5";
            this.CheckBoxDataIntegrityMD5.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxDataIntegrityMD5.TabIndex = 0;
            this.CheckBoxDataIntegrityMD5.Text = "MD5";
            this.CheckBoxDataIntegrityMD5.CheckedChanged += new System.EventHandler(this.CheckBoxDataIntegrityMD5_CheckedChanged);
            // 
            // GroupSecurityEncryption
            // 
            this.GroupSecurityEncryption.Controls.Add(this.CheckBoxEncryptionNone);
            this.GroupSecurityEncryption.Controls.Add(this.CheckBoxEncryptionTripleDES);
            this.GroupSecurityEncryption.Controls.Add(this.CheckBoxEncryptionAES128);
            this.GroupSecurityEncryption.Controls.Add(this.CheckBoxEncryptionAES256);
            this.GroupSecurityEncryption.Location = new System.Drawing.Point(176, 72);
            this.GroupSecurityEncryption.Name = "GroupSecurityEncryption";
            this.GroupSecurityEncryption.Size = new System.Drawing.Size(216, 128);
            this.GroupSecurityEncryption.TabIndex = 13;
            this.GroupSecurityEncryption.TabStop = false;
            this.GroupSecurityEncryption.Text = "Encryption";
            // 
            // CheckBoxEncryptionNone
            // 
            this.CheckBoxEncryptionNone.Checked = true;
            this.CheckBoxEncryptionNone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxEncryptionNone.Location = new System.Drawing.Point(16, 24);
            this.CheckBoxEncryptionNone.Name = "CheckBoxEncryptionNone";
            this.CheckBoxEncryptionNone.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxEncryptionNone.TabIndex = 0;
            this.CheckBoxEncryptionNone.Text = "None";
            this.CheckBoxEncryptionNone.CheckedChanged += new System.EventHandler(this.CheckBoxEncryptionNone_CheckedChanged);
            // 
            // CheckBoxEncryptionTripleDES
            // 
            this.CheckBoxEncryptionTripleDES.Location = new System.Drawing.Point(16, 48);
            this.CheckBoxEncryptionTripleDES.Name = "CheckBoxEncryptionTripleDES";
            this.CheckBoxEncryptionTripleDES.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxEncryptionTripleDES.TabIndex = 0;
            this.CheckBoxEncryptionTripleDES.Text = "Triple DES";
            this.CheckBoxEncryptionTripleDES.CheckedChanged += new System.EventHandler(this.CheckBoxEncryptionTripleDES_CheckedChanged);
            // 
            // CheckBoxEncryptionAES128
            // 
            this.CheckBoxEncryptionAES128.Location = new System.Drawing.Point(16, 72);
            this.CheckBoxEncryptionAES128.Name = "CheckBoxEncryptionAES128";
            this.CheckBoxEncryptionAES128.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxEncryptionAES128.TabIndex = 0;
            this.CheckBoxEncryptionAES128.Text = "AES 128-bit";
            this.CheckBoxEncryptionAES128.CheckedChanged += new System.EventHandler(this.CheckBoxEncryptionAES128_CheckedChanged);
            // 
            // CheckBoxEncryptionAES256
            // 
            this.CheckBoxEncryptionAES256.Location = new System.Drawing.Point(16, 96);
            this.CheckBoxEncryptionAES256.Name = "CheckBoxEncryptionAES256";
            this.CheckBoxEncryptionAES256.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxEncryptionAES256.TabIndex = 0;
            this.CheckBoxEncryptionAES256.Text = "AES 256-bit";
            this.CheckBoxEncryptionAES256.CheckedChanged += new System.EventHandler(this.CheckBoxEncryptionAES256_CheckedChanged);
            // 
            // GroupSecurityGeneral
            // 
            this.GroupSecurityGeneral.Controls.Add(this.CheckBoxCheckRemoteCertificates);
            this.GroupSecurityGeneral.Controls.Add(this.CheckBoxCacheSecureSessions);
            this.GroupSecurityGeneral.Location = new System.Drawing.Point(176, 72);
            this.GroupSecurityGeneral.Name = "GroupSecurityGeneral";
            this.GroupSecurityGeneral.Size = new System.Drawing.Size(216, 128);
            this.GroupSecurityGeneral.TabIndex = 12;
            this.GroupSecurityGeneral.TabStop = false;
            this.GroupSecurityGeneral.Text = "General";
            // 
            // CheckBoxCheckRemoteCertificates
            // 
            this.CheckBoxCheckRemoteCertificates.Location = new System.Drawing.Point(16, 24);
            this.CheckBoxCheckRemoteCertificates.Name = "CheckBoxCheckRemoteCertificates";
            this.CheckBoxCheckRemoteCertificates.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxCheckRemoteCertificates.TabIndex = 0;
            this.CheckBoxCheckRemoteCertificates.Text = "Check remote certificates";
            this.CheckBoxCheckRemoteCertificates.CheckedChanged += new System.EventHandler(this.CheckBoxCheckRemoteCertificates_CheckedChanged);
            // 
            // CheckBoxCacheSecureSessions
            // 
            this.CheckBoxCacheSecureSessions.Location = new System.Drawing.Point(16, 48);
            this.CheckBoxCacheSecureSessions.Name = "CheckBoxCacheSecureSessions";
            this.CheckBoxCacheSecureSessions.Size = new System.Drawing.Size(184, 24);
            this.CheckBoxCacheSecureSessions.TabIndex = 0;
            this.CheckBoxCacheSecureSessions.Text = "Cache secure sessions";
            this.CheckBoxCacheSecureSessions.CheckedChanged += new System.EventHandler(this.CheckBoxCacheSecureSessions_CheckedChanged);
            // 
            // Page12
            // 
            this.Page12.Controls.Add(this.ComboBoxAENameVersion);
            this.Page12.Controls.Add(this.LabelAENameVersion);
            this.Page12.Controls.Add(this.ButtonRemoveDefFiles);
            this.Page12.Controls.Add(this.ButtonAddDefFiles);
            this.Page12.Controls.Add(this.ListBoxDefinitionFiles);
            this.Page12.Controls.Add(this.panel11);
            this.Page12.Location = new System.Drawing.Point(4, 22);
            this.Page12.Name = "Page12";
            this.Page12.Size = new System.Drawing.Size(408, 206);
            this.Page12.TabIndex = 11;
            // 
            // ComboBoxAENameVersion
            // 
            this.ComboBoxAENameVersion.Enabled = false;
            this.ComboBoxAENameVersion.Location = new System.Drawing.Point(240, 176);
            this.ComboBoxAENameVersion.Name = "ComboBoxAENameVersion";
            this.ComboBoxAENameVersion.Size = new System.Drawing.Size(160, 21);
            this.ComboBoxAENameVersion.TabIndex = 4;
            // 
            // LabelAENameVersion
            // 
            this.LabelAENameVersion.Location = new System.Drawing.Point(240, 144);
            this.LabelAENameVersion.Name = "LabelAENameVersion";
            this.LabelAENameVersion.Size = new System.Drawing.Size(168, 32);
            this.LabelAENameVersion.TabIndex = 18;
            this.LabelAENameVersion.Text = "Application Entity Name - Version:";
            // 
            // ButtonRemoveDefFiles
            // 
            this.ButtonRemoveDefFiles.Location = new System.Drawing.Point(240, 88);
            this.ButtonRemoveDefFiles.Name = "ButtonRemoveDefFiles";
            this.ButtonRemoveDefFiles.Size = new System.Drawing.Size(160, 23);
            this.ButtonRemoveDefFiles.TabIndex = 3;
            this.ButtonRemoveDefFiles.Text = "Remove selected files";
            this.ButtonRemoveDefFiles.Click += new System.EventHandler(this.ButtonRemoveDefFiles_Click);
            // 
            // ButtonAddDefFiles
            // 
            this.ButtonAddDefFiles.Location = new System.Drawing.Point(240, 56);
            this.ButtonAddDefFiles.Name = "ButtonAddDefFiles";
            this.ButtonAddDefFiles.Size = new System.Drawing.Size(160, 23);
            this.ButtonAddDefFiles.TabIndex = 2;
            this.ButtonAddDefFiles.Text = "Add definition files";
            this.ButtonAddDefFiles.Click += new System.EventHandler(this.ButtonAddDefFiles_Click);
            // 
            // ListBoxDefinitionFiles
            // 
            this.ListBoxDefinitionFiles.HorizontalScrollbar = true;
            this.ListBoxDefinitionFiles.Location = new System.Drawing.Point(8, 56);
            this.ListBoxDefinitionFiles.Name = "ListBoxDefinitionFiles";
            this.ListBoxDefinitionFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ListBoxDefinitionFiles.Size = new System.Drawing.Size(216, 147);
            this.ListBoxDefinitionFiles.TabIndex = 1;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel11.Controls.Add(this.label37);
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(408, 40);
            this.panel11.TabIndex = 6;
            // 
            // label37
            // 
            this.label37.BackColor = System.Drawing.Color.Transparent;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label37.Location = new System.Drawing.Point(8, 8);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(392, 24);
            this.label37.TabIndex = 0;
            this.label37.Text = "Load definition files";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ButtonPrev
            // 
            this.ButtonPrev.Enabled = false;
            this.ButtonPrev.Location = new System.Drawing.Point(168, 232);
            this.ButtonPrev.Name = "ButtonPrev";
            this.ButtonPrev.Size = new System.Drawing.Size(75, 23);
            this.ButtonPrev.TabIndex = 11;
            this.ButtonPrev.Text = "<< Prev";
            this.ButtonPrev.Click += new System.EventHandler(this.ButtonPrev_Click);
            // 
            // ButtonNext
            // 
            this.ButtonNext.Enabled = false;
            this.ButtonNext.Location = new System.Drawing.Point(248, 232);
            this.ButtonNext.Name = "ButtonNext";
            this.ButtonNext.Size = new System.Drawing.Size(75, 23);
            this.ButtonNext.TabIndex = 12;
            this.ButtonNext.Text = "Next >>";
            this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(344, 232);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 13;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(8, 208);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 8);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // DialogOpenProjectFile
            // 
            this.DialogOpenProjectFile.CheckFileExists = false;
            this.DialogOpenProjectFile.DefaultExt = "pdvt";
            this.DialogOpenProjectFile.Filter = "Project Files (*.pdvt)|*.pdvt";
            this.DialogOpenProjectFile.Title = "Create new project file";
            // 
            // DialogAddExistingSessions
            // 
            this.DialogAddExistingSessions.DefaultExt = "ses";
            this.DialogAddExistingSessions.Filter = "Session files (*.ses)|*.ses";
            this.DialogAddExistingSessions.Multiselect = true;
            this.DialogAddExistingSessions.Title = "Load existing session files";
            // 
            // DialogOpenSessionFile
            // 
            this.DialogOpenSessionFile.CheckFileExists = false;
            this.DialogOpenSessionFile.Filter = "Session files (*.ses) |*.ses";
            this.DialogOpenSessionFile.Title = "Create new session file";
            // 
            // DialogAddDefinitionFiles
            // 
            this.DialogAddDefinitionFiles.Filter = "Definition files (*.def)|*.def";
            this.DialogAddDefinitionFiles.Multiselect = true;
            this.DialogAddDefinitionFiles.Title = "Load definition files";
            // 
            // DialogAddCredentialFiles
            // 
            this.DialogAddCredentialFiles.Filter = "PEM Certificate files (*.pem;*.cer)|*.def;*.cer|DER Certificate files (*.cer)|*.c" +
                "er|PKCS#12 files (*.p12;*.pfx)|*.p12;*.pfx|PKCS#7 files (*.p7b;*.p7c)|*.p7b;*.p7" +
                "c";
            // 
            // WizardNew
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(432, 272);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonNext);
            this.Controls.Add(this.ButtonPrev);
            this.Controls.Add(this.WizardPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardNew";
            this.ShowInTaskbar = false;
            this.Text = "New Project or Session Wizard";
            this.WizardPages.ResumeLayout(false);
            this.Page1.ResumeLayout(false);
            this.PanelPage1.ResumeLayout(false);
            this.Page9.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.Page4.ResumeLayout(false);
            this.Page4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.Page5.ResumeLayout(false);
            this.Page5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.Page11.ResumeLayout(false);
            this.Page11.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.Page2.ResumeLayout(false);
            this.Page2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.Page3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.Page7.ResumeLayout(false);
            this.Page7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTListenPort)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumericSUTPDULength)).EndInit();
            this.Page10.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.Page6.ResumeLayout(false);
            this.Page6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTPDULength)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumericSocketTimeOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericDVTListenPort)).EndInit();
            this.Page8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.GroupSecurityVersion.ResumeLayout(false);
            this.GroupSecurityAuthentication.ResumeLayout(false);
            this.GroupSecurityKeyExchange.ResumeLayout(false);
            this.GroupSecurityDataIntegrity.ResumeLayout(false);
            this.GroupSecurityEncryption.ResumeLayout(false);
            this.GroupSecurityGeneral.ResumeLayout(false);
            this.Page12.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        #endregion
       
        private void ButtonPrev_Click(object sender, System.EventArgs e)
        {
            switch (this.current_page)
            {
                case 1:     // Create new Project or Session
                    // This should never happen.
                    break;
                case 2:     // Create new Project file
                    this.current_page = 1;
                    this.ButtonNext.Enabled = true;
                    this.ButtonPrev.Enabled = false;
                    this.WizardPages.SelectedTab = this.Page1;
                    break;
                case 3:     // Select Session Files
                    this.current_page = 2;
                    this.ButtonNext.Enabled = true;
                    this.ButtonNext.Text = "Next >>";
                    if (this.start_page == StartPage.project)
                        this.ButtonPrev.Enabled = false;
                    else
                        this.ButtonPrev.Enabled = true;
                    this.WizardPages.SelectedTab = this.Page2;
                    break;
                case 4:     // Create new Session file
                    this.current_page = 1;
                    this.ButtonNext.Enabled = true;
                    this.ButtonPrev.Enabled = false;
                    this.WizardPages.SelectedTab = this.Page1;
                    break;
                case 5:     // Session properties
                    this.current_page = 4;
                    if (this.start_page == StartPage.session)
                        this.ButtonPrev.Enabled = false;
                    else
                        this.ButtonPrev.Enabled = true;
                    this.WizardPages.SelectedTab = this.Page4;
                    break;
                case 6:     // DVT Role settings
                    this.current_page = 5;
                    this.WizardPages.SelectedTab = this.Page5;
                    break;
                case 7:     // System Under Test settings
                    this.current_page = 6;
                    this.WizardPages.SelectedTab = this.Page6;
                    break;
                case 8:     // Security settings
                    this.current_page = 7;
                    this.WizardPages.SelectedTab = this.Page7;
                    break;
                case 9:     // Add Credentials
                    this.current_page = 8;
                    this.WizardPages.SelectedTab = this.Page8;
                    break;
                case 10:    // Add trusted certificates
                    this.current_page = 9;
                    this.WizardPages.SelectedTab = this.Page9;
                    break;
                case 11:    // Environment settings
					if (ComboBoxSessionType.Text == "Media")
					{
						this.current_page = 5;
						this.WizardPages.SelectedTab = this.Page5;	
					}
					else
					{
						if (this.CheckboxSecureConnection.Checked)
						{
							this.current_page = 10;
							this.WizardPages.SelectedTab = this.Page10;
						}
						else
						{
							this.current_page = 7;
							this.WizardPages.SelectedTab = this.Page7;
						}
					}
                    break;
                case 12:    // Load definition files
                    this.current_page = 11;
                    this.ButtonNext.Text = "Next >>";
                    this.WizardPages.SelectedTab = this.Page11;
                    break;
            }

			UpdateTab(current_page);
        }

        private void ButtonNext_Click(object sender, System.EventArgs e)
        {
			Assembly ThisAssembly = Assembly.GetExecutingAssembly();
			AssemblyName ThisAssemblyName = ThisAssembly.GetName();

			string dvtVersion = 
				string.Format(
				"{0:D}.{1:D}.{2:D}" ,
				ThisAssemblyName.Version.Major,
				ThisAssemblyName.Version.Minor,
				ThisAssemblyName.Version.Build
				);

            switch (this.current_page)
            {
                case 1:     // Create new Project or Session
                    if (this.RadioNewProject.Checked)
                    {
                        if (MessageBox.Show (this,
                            "The currently opened project will be closed. Are you sure you want to continue?",
                            "Replace currently opened project?",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            this.current_page = 2;
                            if (this.ProjectFileName.Text != "")
                                this.ButtonNext.Enabled = true;
                            else
                                this.ButtonNext.Enabled = false;
                            this.WizardPages.SelectedTab = this.Page2;
                            this.ButtonPrev.Enabled = true;
                        }
                    }
                    else
                    {
                        // User selected to create a new session.
                        this.current_page = 4;
                        if (this.SessionFileName.Text != "")
                            this.ButtonNext.Enabled = true;
                        else
                            this.ButtonNext.Enabled = false;
                        this.WizardPages.SelectedTab = this.Page4;
                        this.ButtonPrev.Enabled = true;
                    }
                    break;
                case 2:     // Create new Project file
                    // Next step is the Add Session Files wizard page
                    this.current_page = 3;
                    this.ButtonNext.Text = "Finish";
                    this.ButtonNext.Enabled = true;
                    this.ButtonPrev.Enabled = true;
                    if (this.ListBoxSessions.Items.Count == 0)
                        this.ButtonRemoveSession.Enabled = false;
                    this.WizardPages.SelectedTab = this.Page3;
                    break;
                case 3:     // Select Session Files
                    this.created_project = true;
                    this.Close();
                    break;
                case 4:     // Create new Session file
                    this.current_page = 5;
                    this.ComboBoxSessionType.SelectedIndex = 0;
                    this.WizardPages.SelectedTab = this.Page5;
                    this.ButtonPrev.Enabled = true;
                    break;
                case 5:     // Session properties
                    switch (this.ComboBoxSessionType.SelectedIndex)
                    {
                        case 0:     // Script
                            sessionApp = new DvtkApplicationLayer.ScriptSession (this.SessionFileName.Text);
                            this.current_page = 6;
                            this.WizardPages.SelectedTab = this.Page6;
                            break;
                        case 1:     // Media
                            sessionApp = new DvtkApplicationLayer.MediaSession (this.SessionFileName.Text);
                            this.current_page = 11;
                            this.WizardPages.SelectedTab = this.Page11;
                            break;
                        case 2:     // Emulator
                            sessionApp = new DvtkApplicationLayer.EmulatorSession (this.SessionFileName.Text);
                            this.current_page = 6;
                            this.WizardPages.SelectedTab = this.Page6;
                            break;
                    }
                    sessionApp.SessionTitle = this.TextBoxSessionTitle.Text;
					sessionApp.SoftwareVersions = dvtVersion;
                    sessionApp.TestedBy = this.TextBoxUserName.Text;
                    sessionApp.LogLevelMask = (int)(DvtkApplicationLayer.Session.LogLevelFlags.Error | DvtkApplicationLayer.Session.LogLevelFlags.Warning |
                    DvtkApplicationLayer.Session.LogLevelFlags.Info);
                    sessionApp.Mode = DvtkApplicationLayer.Session.StorageMode.AsMedia;
        			sessionApp.AutoCreateDirectory = true;
        			sessionApp.ContinueOnError = true;

                    // Store the session filename which has been set in the previous
                    // step.
                    sessionApp.SessionFileName = this.SessionFileName.Text;
                    break;
                case 6:     // DVT Role settings
                    if (this.ComboBoxSessionType.SelectedIndex == 0) // script session
                    {
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).DvtAeTitle =               this.TextBoxDVTAeTitle.Text;
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).DvtPort =                  (ushort)this.NumericDVTListenPort.Value;
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).DvtSocketTimeout =         (ushort)this.NumericSocketTimeOut.Value;
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).DvtMaximumLengthReceived = (uint)this.NumericDVTPDULength.Value;
                    }
                    else // emulator session
                    {
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).DvtAeTitle =               this.TextBoxDVTAeTitle.Text;
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).DvtPort =                  (ushort)this.NumericDVTListenPort.Value;
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).DvtSocketTimeout =         (ushort)this.NumericSocketTimeOut.Value;
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).DvtMaximumLengthReceived = (uint)this.NumericDVTPDULength.Value;
						((DvtkApplicationLayer.EmulatorSession)sessionApp).DvtImplementationClassUid= "1.2.826.0.1.3680043.2.1545.1";
						((DvtkApplicationLayer.EmulatorSession)sessionApp).DvtImplementationVersionName= "dvt" + dvtVersion;
                    }
                    this.current_page = 7;
                    this.WizardPages.SelectedTab = this.Page7;
                    break;
                case 7:     // System Under Test settings
                    if (this.ComboBoxSessionType.SelectedIndex == 0) // script session
                    {
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).SutAeTitle =               this.TextBoxSUTAETitle.Text;
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).SutPort =                  (ushort)this.NumericSUTListenPort.Value;
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).SutHostName =              this.TextBoxTCPIP.Text;
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).SutMaximumLengthReceived = (uint)this.NumericSUTPDULength.Value;
                        //((DvtkApplicationLayer.ScriptSession)sessionApp).SecureSocketsEnabled =   this.CheckboxSecureConnection.Checked;
                        ((Dvtk.Sessions.ScriptSession)sessionApp.Implementation).SecuritySettings.SecureSocketsEnabled = this.CheckboxSecureConnection.Checked;
                    }
                    else // emulator session
                    {
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).SutAeTitle =               this.TextBoxSUTAETitle.Text;
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).SutPort =                  (ushort)this.NumericSUTListenPort.Value;
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).SutHostName =              this.TextBoxTCPIP.Text;
                        ((DvtkApplicationLayer.EmulatorSession)sessionApp).SutMaximumLengthReceived = (uint)this.NumericSUTPDULength.Value;
                        //((DvtkApplicationLayer.EmulatorSession)sessionApp).SecureSocketsEnabled =   this.CheckboxSecureConnection.Checked;
                        ((Dvtk.Sessions.EmulatorSession)sessionApp.Implementation).SecuritySettings.SecureSocketsEnabled = this.CheckboxSecureConnection.Checked;
                    }
                    if (this.CheckboxSecureConnection.Checked)
                    {
                        // Initialize the security settings panel
                        if (sessionApp.Implementation is Dvtk.Sessions.ISecure) 
						{
                            Dvtk.Sessions.ISecuritySettings security_settings = null;                            

                            security_settings = (sessionApp.Implementation as Dvtk.Sessions.ISecure).SecuritySettings;

                            if ((security_settings.TlsVersionFlags & Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1) != 0)
                                this.CheckBoxTLS.Checked = true;
                            if ((security_settings.TlsVersionFlags & Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_SSLv3) != 0)
                                this.CheckBoxSSL.Checked = true;
                            this.CheckBoxCacheSecureSessions.Checked = security_settings.CacheTlsSessions;
                            this.CheckBoxCheckRemoteCertificates.Checked = security_settings.CheckRemoteCertificate;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_DSA) != 0)
                                this.CheckBoxAuthenticationDSA.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_RSA) != 0)
                                this.CheckBoxAuthenticationRSA.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5) != 0)
                                this.CheckBoxDataIntegrityMD5.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1) != 0)
                                this.CheckBoxDataIntegritySHA.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES) != 0)
                                this.CheckBoxEncryptionTripleDES.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128) != 0)
                                this.CheckBoxEncryptionAES128.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256) != 0)
                                this.CheckBoxEncryptionAES256.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE) != 0)
                                this.CheckBoxEncryptionNone.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH) != 0)
                                this.CheckBoxKeyExchangeDH.Checked = true;
                            if ((security_settings.CipherFlags & Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA) != 0)
                                this.CheckBoxKeyExchangeRSA.Checked = true;
                        }

                        this.current_page = 8;
                        this.WizardPages.SelectedTab = this.Page8;
                    }
                    else
                    {
                        this.current_page = 11;
                        this.WizardPages.SelectedTab = this.Page11;
                    }
                    break;
                case 8:     // Security settings
                    // The settings need not be updated from the UI to the session settings.
                    // This has been done already when a checkbox has been checked/unchecked.
                    this.current_page = 9;
                    this.WizardPages.SelectedTab = this.Page9;
                    break;
                case 9:     // Add Credentials
                    // Still todo. Can be implemented when the credentials part is accessible from the component.
                    this.current_page = 10;
                    this.WizardPages.SelectedTab = this.Page10;
                    break;
                case 10:    // Add trusted certificates
                    // Still todo. Can be implemented when the credentials part is accessible from the component.
                    this.current_page = 11;
                    this.WizardPages.SelectedTab = this.Page11;
                    break;
                case 11:    // Environment settings
                    this.sessionApp.ResultsRootDirectory = this.TextBoxResultsRoot.Text;
                    this.sessionApp.Implementation.DefinitionManagement.DefinitionFileRootDirectory = this.TextBoxDefinitionRoot.Text;
                    if (this.ComboBoxSessionType.SelectedIndex == 0) // Script session
                        ((DvtkApplicationLayer.ScriptSession)sessionApp).DicomScriptRootDirectory = this.TextBoxScriptRoot.Text;
                    this.current_page = 12;
                    this.ButtonNext.Text = "Finish";
                    this.WizardPages.SelectedTab = this.Page12;
                    break;
                case 12:    // Load definition files
                    foreach (string def_file in this.ListBoxDefinitionFiles.Items)
                    {
                        this.sessionApp.Implementation.DefinitionManagement.LoadDefinitionFile (def_file);
                    }
                    sessionApp.Save();
                    this.created_session = true;
                    this.Close();
                    break;
            }

        	UpdateTab(current_page);
        }

        private void ButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.created_project = false;
            this.created_session = false;
            this.Close();
        }

        // Event handlers for the first step in the New Project/Session Wizard.
        private void RadioNewSession_CheckedChanged(object sender, System.EventArgs e)
        {
            this.ButtonNext.Enabled = true;
        }

        private void RadioNewProject_CheckedChanged(object sender, System.EventArgs e)
        {
            this.ButtonNext.Enabled = true;
        }

        // Event handlers for the Select a project file name Wizard page
        private void ButtonBrowseProject_Click(object sender, System.EventArgs e)
        {
            this.DialogOpenProjectFile.ShowDialog (this);
            if (this.DialogOpenProjectFile.FileName != "")
            {
                FileInfo file = new FileInfo (this.DialogOpenProjectFile.FileName);
				if (file.Exists)
				{
					MessageBox.Show (this, "You're not allowed to select an existing project file.",
						"Invalid file selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
//				else if(this.DialogOpenProjectFile.FileName.IndexOf("-")>0)
//				{
//					MessageBox.Show (this, "You're not allowed to select a project file with '-' in its name.Please select another project file name.",
//						"Invalid file selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
//				}
				else
				{
					this.ProjectFileName.Text = this.DialogOpenProjectFile.FileName;
					this.ButtonNext.Enabled = true;
				}
            }
        }

        private void ButtonAddSession_Click(object sender, System.EventArgs e)
        {
            ArrayList skipped_files = new ArrayList ();
            if (this.DialogAddExistingSessions.ShowDialog (this) == DialogResult.OK)
            {
                foreach (object file in this.DialogAddExistingSessions.FileNames)
                {
                    if (this.ListBoxSessions.Items.Contains (file))
                        skipped_files.Add (file);
                    else
                        this.ListBoxSessions.Items.Add (file);
                }
                if (skipped_files.Count > 0)
                {
                    string text = "Skipped the following session files because they already exist:";
                    foreach (object file in skipped_files)
                        text += "\n" + file.ToString();
                    MessageBox.Show (this, text, "Skipped files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (this.ListBoxSessions.Items.Count > 0)
                    this.ButtonRemoveSession.Enabled = true;
            }
        }

        private void ButtonRemoveSession_Click(object sender, System.EventArgs e)
        {
            int nr_items = this.ListBoxSessions.SelectedItems.Count;
            if (nr_items > 0)
            {
                if (MessageBox.Show (this,
                    "Are you sure you want to delete the selected session files?",
                    "Remove selected session files?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Create a copy of the selected items. This is needed because the
                    // selected items list is dynamically updated.
                    ArrayList list = new ArrayList (this.ListBoxSessions.SelectedItems);
                    foreach (object item in list)
                        this.ListBoxSessions.Items.Remove (item);

                    if (this.ListBoxSessions.Items.Count == 0)
                        this.ButtonRemoveSession.Enabled = false;
                }
            }
        }

        private void ButtonBrowseSession_Click(object sender, System.EventArgs e)
        {
            this.DialogOpenSessionFile.ShowDialog (this);
            if (this.DialogOpenSessionFile.FileName != "")
            {
                FileInfo    file = new FileInfo (this.DialogOpenSessionFile.FileName);
                if (file.Exists)
                {
                    MessageBox.Show (this, "You're not allowed to select an existing session file.",
                        "Invalid file selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.SessionFileName.Text = this.DialogOpenSessionFile.FileName;
                    this.ButtonNext.Enabled = true;
                }
            }
        }

        private void ListBoxSecuritySettings_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Make all security setting groups invisible.
            this.GroupSecurityGeneral.Visible = false;
            this.GroupSecurityVersion.Visible = false;
            this.GroupSecurityAuthentication.Visible = false;
            this.GroupSecurityKeyExchange.Visible = false;
            this.GroupSecurityDataIntegrity.Visible = false;
            this.GroupSecurityEncryption.Visible = false;

            // Make the selected security setting group visible only.
            switch (this.ListBoxSecuritySettings.SelectedIndex)
            {
                case 0:
                    this.GroupSecurityGeneral.Visible = true;
                    break;
                case 1:
                    this.GroupSecurityVersion.Visible = true;
                    break;
                case 2:
                    this.GroupSecurityAuthentication.Visible = true;
                    break;
                case 3:
                    this.GroupSecurityKeyExchange.Visible = true;
                    break;
                case 4:
                    this.GroupSecurityDataIntegrity.Visible = true;
                    break;
                case 5:
                    this.GroupSecurityEncryption.Visible = true;
                    break;
            }
        }

        private void DisableNavigation ()
        {
            this.LabelSelect1ItemMsg.Visible = true;
            this.ButtonNext.Enabled = false;
            this.ButtonPrev.Enabled = false;
            this.ListBoxSecuritySettings.Enabled = false;
        }

        private void EnableNavigation ()
        {
            this.LabelSelect1ItemMsg.Visible = false;
            this.ButtonNext.Enabled = true;
            this.ButtonPrev.Enabled = true;
            this.ListBoxSecuritySettings.Enabled = true;
        }

        // Security Settings - Key Exchange group settings
        private void CheckBoxKeyExchangeRSA_CheckedChanged(object sender, System.EventArgs e) {
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA, this.CheckBoxKeyExchangeRSA.Checked))
                UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH, this.CheckBoxKeyExchangeDH.Checked);
        }

        private void CheckBoxKeyExchangeDH_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_DH, this.CheckBoxKeyExchangeDH.Checked))
                UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_KEY_EXCHANGE_METHOD_RSA, this.CheckBoxKeyExchangeRSA.Checked);
        }

        // Security Settings - Authentication group settings
        private void CheckBoxAuthenticationRSA_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_RSA, this.CheckBoxAuthenticationRSA.Checked))
                UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_DSA, this.CheckBoxAuthenticationDSA.Checked);
        }

        private void CheckBoxAuthenticationDSA_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_DSA, this.CheckBoxAuthenticationDSA.Checked))
                UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_AUTHENICATION_METHOD_RSA, this.CheckBoxAuthenticationRSA.Checked);
        }

        // Security Settings - Version group settings
        private void CheckBoxTLS_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (this.UpdateVersionFlag (Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1, this.CheckBoxTLS.Checked))
                this.UpdateVersionFlag (Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_SSLv3, this.CheckBoxSSL.Checked);
        }

        private void CheckBoxSSL_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (this.UpdateVersionFlag (Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_SSLv3, this.CheckBoxSSL.Checked))
                this.UpdateVersionFlag (Dvtk.Sessions.TlsVersionFlags.TLS_VERSION_TLSv1, this.CheckBoxTLS.Checked);
        }

        // Security Settings - Encryption group settings
        private void CheckBoxEncryptionNone_CheckedChanged(object sender, System.EventArgs e) {
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE, this.CheckBoxEncryptionNone.Checked))
                if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, this.CheckBoxEncryptionTripleDES.Checked))
                    if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128, this.CheckBoxEncryptionAES128.Checked))
                        UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256, this.CheckBoxEncryptionAES256.Checked);
        }

        private void CheckBoxEncryptionTripleDES_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, this.CheckBoxEncryptionTripleDES.Checked))
                if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE, this.CheckBoxEncryptionNone.Checked))
                    if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128, this.CheckBoxEncryptionAES128.Checked))
                        UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256, this.CheckBoxEncryptionAES256.Checked);
        }

        private void CheckBoxEncryptionAES128_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128, this.CheckBoxEncryptionAES128.Checked))
                if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE, this.CheckBoxEncryptionNone.Checked))
                    if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, this.CheckBoxEncryptionTripleDES.Checked))
                        UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256, this.CheckBoxEncryptionAES256.Checked);
        }

        private void CheckBoxEncryptionAES256_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES256, this.CheckBoxEncryptionAES256.Checked))
                if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_NONE, this.CheckBoxEncryptionNone.Checked))
                    if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_3DES, this.CheckBoxEncryptionTripleDES.Checked))
                        UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_ENCRYPTION_METHOD_AES128, this.CheckBoxEncryptionAES128.Checked);
        }

        // Security Settings - Data Integrity group settings
        private void CheckBoxDataIntegritySHA_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1, this.CheckBoxDataIntegritySHA.Checked))
                UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5, this.CheckBoxDataIntegrityMD5.Checked);
        }

        private void CheckBoxDataIntegrityMD5_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_MD5, this.CheckBoxDataIntegrityMD5.Checked))
                UpdateSecurityFlag (Dvtk.Sessions.CipherFlags.TLS_DATA_INTEGRITY_METHOD_SHA1, this.CheckBoxDataIntegritySHA.Checked);
        }

        private void UpdateAENameVersionBox ()
        {
            foreach (string filename in this.ListBoxDefinitionFiles.Items)
            {
                Dvtk.Sessions.DefinitionFileDetails def_details;
                try
                {
                    def_details = this.sessionApp.Implementation.DefinitionManagement.GetDefinitionFileDetails (filename);
                    if (!this.ComboBoxAENameVersion.Items.Contains (def_details.ApplicationEntityName + '-' +def_details.ApplicationEntityVersion))
                    {
                        this.ComboBoxAENameVersion.Items.Add (def_details.ApplicationEntityName + '-' +def_details.ApplicationEntityVersion);
                        if (this.ComboBoxAENameVersion.Items.Count == 1)
                            this.ComboBoxAENameVersion.SelectedIndex = 0;
                    }
                }
                catch
                {
                }
            }
        }

        private void ButtonRemoveDefFiles_Click(object sender, System.EventArgs e)
        {
            int nr_items = this.ListBoxDefinitionFiles.SelectedItems.Count;
            if (nr_items > 0)
            {
                if (MessageBox.Show (this,
                    "Are you sure you want to delete the selected definition files?",
                    "Remove selected definition files?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Create a copy of the selected items. This is needed because the
                    // selected items list is dynamically updated.
                    ArrayList list = new ArrayList (this.ListBoxDefinitionFiles.SelectedItems);
                    foreach (object item in list)
                        this.ListBoxDefinitionFiles.Items.Remove (item);

                    if (this.ListBoxDefinitionFiles.Items.Count == 0)
                    {
                        this.ButtonRemoveDefFiles.Enabled = false;
                        this.ComboBoxAENameVersion.Enabled = false;
                    }
                    else
                        this.UpdateAENameVersionBox ();
                }
            }
        }

        private void ButtonAddDefFiles_Click(object sender, System.EventArgs e)
        {
            ArrayList skipped_files = new ArrayList ();
            if (this.TextBoxDefinitionRoot.Text != "")
                this.DialogAddDefinitionFiles.InitialDirectory = this.TextBoxDefinitionRoot.Text;

            if (this.DialogAddDefinitionFiles.ShowDialog (this) == DialogResult.OK)
            {
                foreach (object file in this.DialogAddDefinitionFiles.FileNames)
                {
                    if (this.ListBoxDefinitionFiles.Items.Contains (file))
                        skipped_files.Add (file);
                    else
                        this.ListBoxDefinitionFiles.Items.Add (file);
                }
                if (skipped_files.Count > 0)
                {
                    string text = "Skipped the following definition files because they already exist:";
                    foreach (object file in skipped_files)
                        text += "\n" + file.ToString();
                    MessageBox.Show (this, text, "Skipped files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (this.ListBoxDefinitionFiles.Items.Count > 0)
                {
                    this.ButtonRemoveDefFiles.Enabled = true;
                    this.ComboBoxAENameVersion.Enabled = true;
                    this.UpdateAENameVersionBox ();
                }
            }
        }

        private void ButtonAddCredentials_Click(object sender, System.EventArgs e)
        {
            ArrayList skipped_files = new ArrayList ();
            this.DialogAddCredentialFiles.Title = "Load Credential Files";
            if (this.DialogAddCredentialFiles.ShowDialog (this) == DialogResult.OK)
            {
                foreach (object file in this.DialogAddCredentialFiles.FileNames)
                {
                    if (this.ListBoxCredentials.Items.Contains (file))
                        skipped_files.Add (file);
                    else
                        this.ListBoxCredentials.Items.Add (file);
                }
                if (skipped_files.Count > 0)
                {
                    string text = "Skipped the following credential files because they already exist:";
                    foreach (object file in skipped_files)
                        text += "\n" + file.ToString();
                    MessageBox.Show (this, text, "Skipped files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (this.ListBoxCredentials.Items.Count > 0)
                    this.ButtonRemoveCredentials.Enabled = true;
            }
        }

        private void ButtonRemoveCredentials_Click(object sender, System.EventArgs e)
        {
            int nr_items = this.ListBoxCredentials.SelectedItems.Count;
            if (nr_items > 0)
            {
                if (MessageBox.Show (this,
                    "Are you sure you want to delete the selected credential files?",
                    "Remove selected credential files?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Create a copy of the selected items. This is needed because the
                    // selected items list is dynamically updated.
                    ArrayList list = new ArrayList (this.ListBoxCredentials.SelectedItems);
                    foreach (object item in list)
                        this.ListBoxCredentials.Items.Remove (item);

                    if (this.ListBoxCredentials.Items.Count == 0)
                        this.ButtonRemoveCredentials.Enabled = false;
                }
            }
        }

        private void ButtonAddCertificates_Click(object sender, System.EventArgs e)
        {
            ArrayList skipped_files = new ArrayList ();
            this.DialogAddCredentialFiles.Title = "Load Certificate Files";
            if (this.DialogAddCredentialFiles.ShowDialog (this) == DialogResult.OK)
            {
                foreach (object file in this.DialogAddCredentialFiles.FileNames)
                {
                    if (this.ListBoxCertificates.Items.Contains (file))
                        skipped_files.Add (file);
                    else
                        this.ListBoxCertificates.Items.Add (file);
                }
                if (skipped_files.Count > 0)
                {
                    string text = "Skipped the following certificate files because they already exist:";
                    foreach (object file in skipped_files)
                        text += "\n" + file.ToString();
                    MessageBox.Show (this, text, "Skipped files", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (this.ListBoxCertificates.Items.Count > 0)
                    this.ButtonRemoveCertificates.Enabled = true;
            }
        }

        private void ButtonRemoveCertificates_Click(object sender, System.EventArgs e)
        {
            int nr_items = this.ListBoxCertificates.SelectedItems.Count;
            if (nr_items > 0)
            {
                if (MessageBox.Show (this,
                    "Are you sure you want to delete the selected certificate files?",
                    "Remove selected certificate files?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    // Create a copy of the selected items. This is needed because the
                    // selected items list is dynamically updated.
                    ArrayList list = new ArrayList (this.ListBoxCertificates.SelectedItems);
                    foreach (object item in list)
                        this.ListBoxCertificates.Items.Remove (item);

                    if (this.ListBoxCertificates.Items.Count == 0)
                        this.ButtonRemoveCertificates.Enabled = false;
                }
            }
        }

        private void ButtonBrowseDefinitionRoot_Click(object sender, System.EventArgs e)
        {
            this.DialogBrowseFolder.Description = "Select the root directory containing the definition files.";
            if (this.TextBoxDefinitionRoot.Text != "")
                this.DialogBrowseFolder.SelectedPath = this.TextBoxDefinitionRoot.Text;

            if (this.DialogBrowseFolder.ShowDialog (this) == DialogResult.OK)
                this.TextBoxDefinitionRoot.Text = this.DialogBrowseFolder.SelectedPath;
        }

        private void ButtonBrowseResultsRoot_Click(object sender, System.EventArgs e)
        {
            this.DialogBrowseFolder.Description = "Select the root directory where the result files should be stored.";
            if (this.TextBoxResultsRoot.Text != "")
                this.DialogBrowseFolder.SelectedPath = this.TextBoxResultsRoot.Text;

            if (this.DialogBrowseFolder.ShowDialog (this) == DialogResult.OK)
                this.TextBoxResultsRoot.Text = this.DialogBrowseFolder.SelectedPath;
        }

        private void ButtonBrowseScriptRoot_Click(object sender, System.EventArgs e)
        {
            this.DialogBrowseFolder.Description = "Select the root directory containing the script files.";
            if (this.TextBoxScriptRoot.Text != "")
                this.DialogBrowseFolder.SelectedPath = this.TextBoxScriptRoot.Text;

            if (this.DialogBrowseFolder.ShowDialog (this) == DialogResult.OK)
                this.TextBoxScriptRoot.Text = this.DialogBrowseFolder.SelectedPath;
        }

        private void ComboBoxSessionType_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Do not allow the content of the combobox to be changed.
            e.Handled = true;
        }

        private void CheckBoxCheckRemoteCertificates_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (this.ComboBoxSessionType.SelectedIndex == 0) // script session
                (sessionApp.Implementation as Dvtk.Sessions.ISecure).SecuritySettings.CheckRemoteCertificate = this.CheckBoxCheckRemoteCertificates.Checked;
            else    // emulator session
                (sessionApp.Implementation as Dvtk.Sessions.ISecure).SecuritySettings.CheckRemoteCertificate = this.CheckBoxCheckRemoteCertificates.Checked;
        }

        private void CheckBoxCacheSecureSessions_CheckedChanged(object sender, System.EventArgs e) 
		{
            if (this.ComboBoxSessionType.SelectedIndex == 0) // script session
                (sessionApp.Implementation as Dvtk.Sessions.ISecure).SecuritySettings.CacheTlsSessions = this.CheckBoxCacheSecureSessions.Checked;
            else    // emulator session
                (sessionApp.Implementation as Dvtk.Sessions.ISecure).SecuritySettings.CacheTlsSessions = this.CheckBoxCacheSecureSessions.Checked;
        }

        // Security Settings - Version group settings
        private bool UpdateVersionFlag (Dvtk.Sessions.TlsVersionFlags flag, bool enabled)
		{
            bool    success = true;
            if (sessionApp.Implementation is Dvtk.Sessions.ISecure) {
                Dvtk.Sessions.ISecuritySettings security_settings = null;

                security_settings = (sessionApp.Implementation as Dvtk.Sessions.ISecure).SecuritySettings;

                try {
                    if (enabled)
                        security_settings.TlsVersionFlags |= flag;
                    else
                        security_settings.TlsVersionFlags &= ~flag;
                    this.EnableNavigation ();
                }
                catch (Exception e) {
                    this.LabelSelect1ItemMsg.Text = e.Message;
                    this.DisableNavigation();
                    success = false;
                }
            }
            return success;
        }

        private bool UpdateSecurityFlag (Dvtk.Sessions.CipherFlags flag, bool enabled) 
		{
            bool    success = true;
            if (sessionApp.Implementation is Dvtk.Sessions.ISecure) {
                Dvtk.Sessions.ISecuritySettings security_settings = null;

                security_settings = (sessionApp.Implementation as Dvtk.Sessions.ISecure).SecuritySettings;

                try {
                    if (enabled)
                        security_settings.CipherFlags |= flag;
                    else
                        security_settings.CipherFlags &= ~flag;
                    this.EnableNavigation ();
                }
                catch (Exception e) {
                    this.LabelSelect1ItemMsg.Text = e.Message;
                    this.DisableNavigation();
                    success = false;
                }
            }
            return success;
        }

        private void CallBackMessageDisplay(string message)
        {
            MessageBox.Show (this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

		private void WizardPages_TabIndexChanged(object sender, System.EventArgs e)
		{
			MessageBox.Show("hello");		
		}

		private void UpdateTab(int thePageToDisplay)
		{
			switch(thePageToDisplay)
			{
				case 11:
				{
					if (ComboBoxSessionType.Text == "Script")
					{
						label36.Visible = true;
						TextBoxScriptRoot.Visible = true;
						ButtonBrowseScriptRoot.Visible = true;	
					}
					else
					{
						label36.Visible = false;
						TextBoxScriptRoot.Visible = false;
						ButtonBrowseScriptRoot.Visible = false;
					}
				}
				break;

				default:
					break;
			}
		}
    }
}
