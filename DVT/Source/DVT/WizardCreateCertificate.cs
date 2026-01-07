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

namespace Dvt
{
    /// <summary>
	/// Summary description for WizardCreateCertificate.
	/// </summary>
	public class WizardCreateCertificate : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TabPage Page1;
        private System.Windows.Forms.TabPage Page2;
        private System.Windows.Forms.TabPage Page3;
        private System.Windows.Forms.TabPage Page4;
        private System.Windows.Forms.Panel PanelPage1;
        private System.Windows.Forms.Label TitlePage1;
        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.TextBox TextBoxDepartment;
        private System.Windows.Forms.TextBox TextBoxOrganization;
        private System.Windows.Forms.Label LabelDepartment;
        private System.Windows.Forms.Label LabelOrganization;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonNext;
        private System.Windows.Forms.Button ButtonPrev;
        private System.Windows.Forms.TextBox TextBoxCity;
        private System.Windows.Forms.TextBox TextBoxState;
        private System.Windows.Forms.Label LabelCity;
        private System.Windows.Forms.Label LabelState;
        private System.Windows.Forms.TextBox TextBoxCountry;
        private System.Windows.Forms.Label LabelCountry;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage Page5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ComboBoxKeyType;
        private System.Windows.Forms.CheckBox CheckBoxSelfSignCertificate;
        private System.Windows.Forms.Label LabelFilename;
        private System.Windows.Forms.Label LabelCredentialsPassword;
        private System.Windows.Forms.TextBox TextBoxCredentialsFilename;
        private System.Windows.Forms.TextBox TextBoxCredentialsPassword;
        private System.Windows.Forms.Button ButtonCredentialsBrowse;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button ButtonCertificateBrowse;
        private System.Windows.Forms.Button ButtonCertificatePathBrowse;
        private System.Windows.Forms.CheckBox CheckBoxUseGeneratedCertificate;
        private System.Windows.Forms.TextBox TextBoxCertificateFileRoot;
        private System.Windows.Forms.TextBox TextBoxCertificateFilename;
        private System.Windows.Forms.TextBox TextBoxKeyFile;
        private System.Windows.Forms.TextBox TextBoxCertificatePassword;
        private System.Windows.Forms.TextBox TextBoxCertificateReEnterPassword;
        private System.Windows.Forms.TabControl WizardPages;
        private System.Windows.Forms.ComboBox ComboBoxKeyLength;
        private System.Windows.Forms.OpenFileDialog OpenCertificateFileDialog;
        private System.Windows.Forms.FolderBrowserDialog OpenCertificatePathFileDialog;
        private System.Windows.Forms.DateTimePicker DateTimeFrom;
        private System.Windows.Forms.DateTimePicker DateTimeTo;
        private System.Windows.Forms.Label labelCN;
        private System.Windows.Forms.Label labelON;
        private System.Windows.Forms.Label labelO;
        private System.Windows.Forms.Label labelL;
        private System.Windows.Forms.Label labelST;
        private System.Windows.Forms.Label labelC;
        private System.Windows.Forms.ComboBox comboBoxCountry;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public string GetTextBoxCertificateFilename()
		{
			return TextBoxCertificateFilename.Text;
        }
        public string GetTextBoxCredentialFilename()
        {
            return TextBoxKeyFile.Text;
        }
		public bool GetUseGeneratedCertificate()
		{ 
			return CheckBoxUseGeneratedCertificate.Checked; 
		}
        public void SetUseGeneratedCertificate(bool value)
        {
            CheckBoxUseGeneratedCertificate.Checked = value;
        }
        public WizardCreateCertificate()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.current_page = 1;
            this.ComboBoxKeyType.SelectedIndex = 0;
            this.ComboBoxKeyLength.SelectedIndex = 0;
            this.CheckBoxSelfSignCertificate.Checked = true;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.WizardPages = new System.Windows.Forms.TabControl();
			this.Page1 = new System.Windows.Forms.TabPage();
			this.labelO = new System.Windows.Forms.Label();
			this.labelON = new System.Windows.Forms.Label();
			this.labelCN = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.LabelName = new System.Windows.Forms.Label();
			this.TextBoxName = new System.Windows.Forms.TextBox();
			this.PanelPage1 = new System.Windows.Forms.Panel();
			this.TitlePage1 = new System.Windows.Forms.Label();
			this.TextBoxDepartment = new System.Windows.Forms.TextBox();
			this.TextBoxOrganization = new System.Windows.Forms.TextBox();
			this.LabelDepartment = new System.Windows.Forms.Label();
			this.LabelOrganization = new System.Windows.Forms.Label();
			this.Page2 = new System.Windows.Forms.TabPage();
			this.comboBoxCountry = new System.Windows.Forms.ComboBox();
			this.labelC = new System.Windows.Forms.Label();
			this.labelST = new System.Windows.Forms.Label();
			this.labelL = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.TextBoxCity = new System.Windows.Forms.TextBox();
			this.TextBoxState = new System.Windows.Forms.TextBox();
			this.LabelCity = new System.Windows.Forms.Label();
			this.LabelState = new System.Windows.Forms.Label();
			this.TextBoxCountry = new System.Windows.Forms.TextBox();
			this.LabelCountry = new System.Windows.Forms.Label();
			this.Page3 = new System.Windows.Forms.TabPage();
			this.ComboBoxKeyType = new System.Windows.Forms.ComboBox();
			this.DateTimeFrom = new System.Windows.Forms.DateTimePicker();
			this.label7 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.DateTimeTo = new System.Windows.Forms.DateTimePicker();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.ComboBoxKeyLength = new System.Windows.Forms.ComboBox();
			this.Page4 = new System.Windows.Forms.TabPage();
			this.ButtonCredentialsBrowse = new System.Windows.Forms.Button();
			this.TextBoxCredentialsFilename = new System.Windows.Forms.TextBox();
			this.LabelFilename = new System.Windows.Forms.Label();
			this.CheckBoxSelfSignCertificate = new System.Windows.Forms.CheckBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label5 = new System.Windows.Forms.Label();
			this.LabelCredentialsPassword = new System.Windows.Forms.Label();
			this.TextBoxCredentialsPassword = new System.Windows.Forms.TextBox();
			this.Page5 = new System.Windows.Forms.TabPage();
			this.TextBoxCertificateFileRoot = new System.Windows.Forms.TextBox();
			this.ButtonCertificateBrowse = new System.Windows.Forms.Button();
			this.ButtonCertificatePathBrowse = new System.Windows.Forms.Button();
			this.CheckBoxUseGeneratedCertificate = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.TextBoxCertificateFilename = new System.Windows.Forms.TextBox();
			this.TextBoxKeyFile = new System.Windows.Forms.TextBox();
			this.TextBoxCertificatePassword = new System.Windows.Forms.TextBox();
			this.TextBoxCertificateReEnterPassword = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ButtonCancel = new System.Windows.Forms.Button();
			this.ButtonNext = new System.Windows.Forms.Button();
			this.ButtonPrev = new System.Windows.Forms.Button();
			this.OpenCertificateFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.OpenCertificatePathFileDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.WizardPages.SuspendLayout();
			this.Page1.SuspendLayout();
			this.PanelPage1.SuspendLayout();
			this.Page2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.Page3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.Page4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.Page5.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// WizardPages
			// 
			this.WizardPages.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			this.WizardPages.Controls.Add(this.Page1);
			this.WizardPages.Controls.Add(this.Page2);
			this.WizardPages.Controls.Add(this.Page3);
			this.WizardPages.Controls.Add(this.Page4);
			this.WizardPages.Controls.Add(this.Page5);
			this.WizardPages.Location = new System.Drawing.Point(8, -24);
			this.WizardPages.Name = "WizardPages";
			this.WizardPages.SelectedIndex = 0;
			this.WizardPages.Size = new System.Drawing.Size(416, 232);
			this.WizardPages.TabIndex = 0;
			// 
			// Page1
			// 
			this.Page1.Controls.Add(this.labelO);
			this.Page1.Controls.Add(this.labelON);
			this.Page1.Controls.Add(this.labelCN);
			this.Page1.Controls.Add(this.label2);
			this.Page1.Controls.Add(this.LabelName);
			this.Page1.Controls.Add(this.TextBoxName);
			this.Page1.Controls.Add(this.PanelPage1);
			this.Page1.Controls.Add(this.TextBoxDepartment);
			this.Page1.Controls.Add(this.TextBoxOrganization);
			this.Page1.Controls.Add(this.LabelDepartment);
			this.Page1.Controls.Add(this.LabelOrganization);
			this.Page1.Location = new System.Drawing.Point(4, 25);
			this.Page1.Name = "Page1";
			this.Page1.Size = new System.Drawing.Size(408, 203);
			this.Page1.TabIndex = 0;
			// 
			// labelO
			// 
			this.labelO.Location = new System.Drawing.Point(112, 120);
			this.labelO.Name = "labelO";
			this.labelO.Size = new System.Drawing.Size(24, 23);
			this.labelO.TabIndex = 10;
			this.labelO.Text = "O";
			// 
			// labelON
			// 
			this.labelON.Location = new System.Drawing.Point(112, 88);
			this.labelON.Name = "labelON";
			this.labelON.Size = new System.Drawing.Size(24, 23);
			this.labelON.TabIndex = 9;
			this.labelON.Text = "ON";
			// 
			// labelCN
			// 
			this.labelCN.Location = new System.Drawing.Point(112, 56);
			this.labelCN.Name = "labelCN";
			this.labelCN.Size = new System.Drawing.Size(24, 23);
			this.labelCN.TabIndex = 8;
			this.labelCN.Text = "CN";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 168);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(376, 24);
			this.label2.TabIndex = 7;
			this.label2.Text = "The above information is optional.";
			// 
			// LabelName
			// 
			this.LabelName.Location = new System.Drawing.Point(8, 56);
			this.LabelName.Name = "LabelName";
			this.LabelName.TabIndex = 6;
			this.LabelName.Text = "Name:";
			// 
			// TextBoxName
			// 
			this.TextBoxName.Location = new System.Drawing.Point(136, 56);
			this.TextBoxName.Name = "TextBoxName";
			this.TextBoxName.Size = new System.Drawing.Size(256, 20);
			this.TextBoxName.TabIndex = 0;
			this.TextBoxName.Text = "";
			// 
			// PanelPage1
			// 
			this.PanelPage1.BackColor = System.Drawing.SystemColors.HighlightText;
			this.PanelPage1.Controls.Add(this.TitlePage1);
			this.PanelPage1.Location = new System.Drawing.Point(0, -8);
			this.PanelPage1.Name = "PanelPage1";
			this.PanelPage1.Size = new System.Drawing.Size(408, 40);
			this.PanelPage1.TabIndex = 4;
			// 
			// TitlePage1
			// 
			this.TitlePage1.BackColor = System.Drawing.Color.Transparent;
			this.TitlePage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.TitlePage1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.TitlePage1.Location = new System.Drawing.Point(8, 8);
			this.TitlePage1.Name = "TitlePage1";
			this.TitlePage1.Size = new System.Drawing.Size(392, 24);
			this.TitlePage1.TabIndex = 0;
			this.TitlePage1.Text = "User Information for new certificate (1)";
			this.TitlePage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TextBoxDepartment
			// 
			this.TextBoxDepartment.Location = new System.Drawing.Point(136, 88);
			this.TextBoxDepartment.Name = "TextBoxDepartment";
			this.TextBoxDepartment.Size = new System.Drawing.Size(256, 20);
			this.TextBoxDepartment.TabIndex = 1;
			this.TextBoxDepartment.Text = "";
			// 
			// TextBoxOrganization
			// 
			this.TextBoxOrganization.Location = new System.Drawing.Point(136, 120);
			this.TextBoxOrganization.Name = "TextBoxOrganization";
			this.TextBoxOrganization.Size = new System.Drawing.Size(256, 20);
			this.TextBoxOrganization.TabIndex = 2;
			this.TextBoxOrganization.Text = "";
			// 
			// LabelDepartment
			// 
			this.LabelDepartment.Location = new System.Drawing.Point(8, 88);
			this.LabelDepartment.Name = "LabelDepartment";
			this.LabelDepartment.Size = new System.Drawing.Size(104, 23);
			this.LabelDepartment.TabIndex = 6;
			this.LabelDepartment.Text = "Department:";
			// 
			// LabelOrganization
			// 
			this.LabelOrganization.Location = new System.Drawing.Point(8, 120);
			this.LabelOrganization.Name = "LabelOrganization";
			this.LabelOrganization.Size = new System.Drawing.Size(104, 23);
			this.LabelOrganization.TabIndex = 6;
			this.LabelOrganization.Text = "Organization:";
			// 
			// Page2
			// 
			this.Page2.Controls.Add(this.comboBoxCountry);
			this.Page2.Controls.Add(this.labelC);
			this.Page2.Controls.Add(this.labelST);
			this.Page2.Controls.Add(this.labelL);
			this.Page2.Controls.Add(this.label3);
			this.Page2.Controls.Add(this.panel1);
			this.Page2.Controls.Add(this.TextBoxCity);
			this.Page2.Controls.Add(this.TextBoxState);
			this.Page2.Controls.Add(this.LabelCity);
			this.Page2.Controls.Add(this.LabelState);
			this.Page2.Controls.Add(this.TextBoxCountry);
			this.Page2.Controls.Add(this.LabelCountry);
			this.Page2.Location = new System.Drawing.Point(4, 25);
			this.Page2.Name = "Page2";
			this.Page2.Size = new System.Drawing.Size(408, 203);
			this.Page2.TabIndex = 1;
			// 
			// comboBoxCountry
			// 
			this.comboBoxCountry.Items.AddRange(new object[] {
																 "US United States of America",
																 "CA Canada",
																 "AD Andorra",
																 "AE United Arab Emirates",
																 "AF Afghanistan",
																 "AG Antigua and Barbuda",
																 "AI Anguilla",
																 "AL Albania",
																 "AM Armenia",
																 "AN Netherlands Antilles",
																 "AO Angola",
																 "AQ Antarctica",
																 "AR Argentina",
																 "AS American Samoa",
																 "AT Austria",
																 "AU Australia",
																 "AW Aruba",
																 "AZ Azerbaijan",
																 "BA Bosnia and Herzegovina",
																 "BB Barbados",
																 "BD Bangladesh",
																 "BE Belgium",
																 "BF Burkina Faso",
																 "BG Bulgaria",
																 "BH Bahrain",
																 "BI Burundi",
																 "BJ Benin",
																 "BM Bermuda",
																 "BN Brunei Darussalam",
																 "BO Bolivia",
																 "BR Brazil",
																 "BS Bahamas",
																 "BT Bhutan",
																 "BV Bouvet Island",
																 "BW Botswana",
																 "BY Belarus",
																 "BZ Belize",
																 "CA Canada",
																 "CC Cocos (Keeling) Islands",
																 "CF Central African Republic",
																 "CG Congo",
																 "CH Switzerland",
																 "CI Cote D\'Ivoire (Ivory Coast)",
																 "CK Cook Islands",
																 "CL Chile",
																 "CM Cameroon",
																 "CN China",
																 "CO Colombia",
																 "CR Costa Rica",
																 "CS Czechoslovakia (former)",
																 "CU Cuba",
																 "CV Cape Verde",
																 "CX Christmas Island",
																 "CY Cyprus",
																 "CZ Czech Republic",
																 "DE Germany",
																 "DJ Djibouti",
																 "DK Denmark",
																 "DM Dominica",
																 "DO Dominican Republic",
																 "DZ Algeria",
																 "EC Ecuador",
																 "EE Estonia",
																 "EG Egypt",
																 "EH Western Sahara",
																 "ER Eritrea",
																 "ES Spain",
																 "ET Ethiopia",
																 "FI Finland",
																 "FJ Fiji",
																 "FK Falkland Islands (Malvinas)",
																 "FM Micronesia",
																 "FO Faroe Islands",
																 "FR France",
																 "FX France, Metropolitan",
																 "GA Gabon",
																 "GB Great Britain (UK)",
																 "GD Grenada",
																 "GE Georgia",
																 "GF French Guiana",
																 "GH Ghana",
																 "GI Gibraltar",
																 "GL Greenland",
																 "GM Gambia",
																 "GN Guinea",
																 "GP Guadeloupe",
																 "GQ Equatorial Guinea",
																 "GR Greece",
																 "GS S. Georgia and S. Sandwich Isls.",
																 "GT Guatemala",
																 "GU Guam",
																 "GW Guinea-Bissau",
																 "GY Guyana",
																 "HK Hong Kong",
																 "HM Heard and McDonald Islands",
																 "HN Honduras",
																 "HR Croatia (Hrvatska)",
																 "HT Haiti",
																 "HU Hungary",
																 "ID Indonesia",
																 "IE Ireland",
																 "IL Israel",
																 "IN India",
																 "IO British Indian Ocean Territory",
																 "IQ Iraq",
																 "IR Iran",
																 "IS Iceland",
																 "IT Italy",
																 "JM Jamaica",
																 "JO Jordan",
																 "JP Japan",
																 "KE Kenya",
																 "KG Kyrgyzstan",
																 "KH Cambodia",
																 "KI Kiribati",
																 "KM Comoros",
																 "KN Saint Kitts and Nevis",
																 "KP Korea (North)",
																 "KR Korea (South)",
																 "KW Kuwait",
																 "KY Cayman Islands",
																 "KZ Kazakhstan",
																 "LA Laos",
																 "LB Lebanon",
																 "LC Saint Lucia",
																 "LI Liechtenstein",
																 "LK Sri Lanka",
																 "LR Liberia",
																 "LS Lesotho",
																 "LT Lithuania",
																 "LU Luxembourg",
																 "LV Latvia",
																 "LY Libya",
																 "MA Morocco",
																 "MC Monaco",
																 "MD Moldova",
																 "MG Madagascar",
																 "MH Marshall Islands",
																 "MK Macedonia",
																 "ML Mali",
																 "MM Myanmar",
																 "MN Mongolia",
																 "MO Macau",
																 "MP Northern Mariana Islands",
																 "MQ Martinique",
																 "MR Mauritania",
																 "MS Montserrat",
																 "MT Malta",
																 "MU Mauritius",
																 "MV Maldives",
																 "MW Malawi",
																 "MX Mexico",
																 "MY Malaysia",
																 "MZ Mozambique",
																 "NA Namibia",
																 "NC New Caledonia",
																 "NE Niger",
																 "NF Norfolk Island",
																 "NG Nigeria",
																 "NI Nicaragua",
																 "NL Netherlands",
																 "NO Norway",
																 "NP Nepal",
																 "NR Nauru",
																 "NT Neutral Zone",
																 "NU Niue",
																 "NZ New Zealand (Aotearoa)",
																 "OM Oman",
																 "PA Panama",
																 "PE Peru",
																 "PF French Polynesia",
																 "PG Papua New Guinea",
																 "PH Philippines",
																 "PK Pakistan",
																 "PL Poland",
																 "PM St. Pierre and Miquelon",
																 "PN Pitcairn",
																 "PR Puerto Rico",
																 "PT Portugal",
																 "PW Palau",
																 "PY Paraguay",
																 "QA Qatar",
																 "RE Reunion",
																 "RO Romania",
																 "RU Russian Federation",
																 "RW Rwanda",
																 "SA Saudi Arabia",
																 "Sb Solomon Islands",
																 "SC Seychelles",
																 "SD Sudan",
																 "SE Sweden",
																 "SG Singapore",
																 "SH St. Helena",
																 "SI Slovenia",
																 "SJ Svalbard and Jan Mayen Islands",
																 "SK Slovak Republic",
																 "SL Sierra Leone",
																 "SM San Marino",
																 "SN Senegal",
																 "SO Somalia",
																 "SR Suriname",
																 "ST Sao Tome and Principe",
																 "SU USSR (former)",
																 "SV El Salvador",
																 "SY Syria",
																 "SZ Swaziland",
																 "TC Turks and Caicos Islands",
																 "TD Chad",
																 "TF French Southern Territories",
																 "TG Togo",
																 "TH Thailand",
																 "TJ Tajikistan",
																 "TK Tokelau",
																 "TM Turkmenistan",
																 "TN Tunisia",
																 "TO Tonga",
																 "TP East Timor",
																 "TR Turkey",
																 "TT Trinidad and Tobago",
																 "TV Tuvalu",
																 "TW Taiwan",
																 "TZ Tanzania",
																 "UA Ukraine",
																 "UG Uganda",
																 "UK United Kingdom",
																 "UM US Minor Outlying Islands",
																 "US United States",
																 "UY Uruguay",
																 "UZ Uzbekistan",
																 "VA Vatican City State (Holy See)",
																 "VC Saint Vincent and the Grenadines",
																 "VE Venezuela",
																 "VG Virgin Islands (British)",
																 "VI Virgin Islands (U.S.)",
																 "VN Viet Nam",
																 "VU Vanuatu",
																 "WF Wallis and Futuna Islands",
																 "WS Samoa",
																 "YE Yemen",
																 "YT Mayotte",
																 "YU Yugoslavia",
																 "ZA South Africa",
																 "ZM Zambia",
																 "ZR Zaire",
																 "ZW Zimbabwe"});
			this.comboBoxCountry.Location = new System.Drawing.Point(208, 120);
			this.comboBoxCountry.Name = "comboBoxCountry";
			this.comboBoxCountry.Size = new System.Drawing.Size(184, 21);
			this.comboBoxCountry.TabIndex = 18;
			this.comboBoxCountry.Text = "Country";
			this.comboBoxCountry.SelectedIndexChanged += new System.EventHandler(this.comboBoxCountry_SelectedIndexChanged);
			// 
			// labelC
			// 
			this.labelC.Location = new System.Drawing.Point(112, 120);
			this.labelC.Name = "labelC";
			this.labelC.Size = new System.Drawing.Size(24, 23);
			this.labelC.TabIndex = 17;
			this.labelC.Text = "C";
			// 
			// labelST
			// 
			this.labelST.Location = new System.Drawing.Point(112, 88);
			this.labelST.Name = "labelST";
			this.labelST.Size = new System.Drawing.Size(24, 23);
			this.labelST.TabIndex = 16;
			this.labelST.Text = "ST";
			// 
			// labelL
			// 
			this.labelL.Location = new System.Drawing.Point(112, 56);
			this.labelL.Name = "labelL";
			this.labelL.Size = new System.Drawing.Size(24, 23);
			this.labelL.TabIndex = 15;
			this.labelL.Text = "L";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 168);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(376, 24);
			this.label3.TabIndex = 14;
			this.label3.Text = "The above information is optional.";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.HighlightText;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(0, -8);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(408, 40);
			this.panel1.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(392, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "User Information for new certificate (2)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TextBoxCity
			// 
			this.TextBoxCity.Location = new System.Drawing.Point(136, 56);
			this.TextBoxCity.Name = "TextBoxCity";
			this.TextBoxCity.Size = new System.Drawing.Size(256, 20);
			this.TextBoxCity.TabIndex = 0;
			this.TextBoxCity.Text = "";
			// 
			// TextBoxState
			// 
			this.TextBoxState.Location = new System.Drawing.Point(136, 88);
			this.TextBoxState.Name = "TextBoxState";
			this.TextBoxState.Size = new System.Drawing.Size(256, 20);
			this.TextBoxState.TabIndex = 1;
			this.TextBoxState.Text = "";
			// 
			// LabelCity
			// 
			this.LabelCity.Location = new System.Drawing.Point(8, 56);
			this.LabelCity.Name = "LabelCity";
			this.LabelCity.Size = new System.Drawing.Size(104, 23);
			this.LabelCity.TabIndex = 11;
			this.LabelCity.Text = "City:";
			// 
			// LabelState
			// 
			this.LabelState.Location = new System.Drawing.Point(8, 88);
			this.LabelState.Name = "LabelState";
			this.LabelState.Size = new System.Drawing.Size(104, 23);
			this.LabelState.TabIndex = 12;
			this.LabelState.Text = "State:";
			// 
			// TextBoxCountry
			// 
			this.TextBoxCountry.Location = new System.Drawing.Point(136, 120);
			this.TextBoxCountry.Name = "TextBoxCountry";
			this.TextBoxCountry.ReadOnly = true;
			this.TextBoxCountry.Size = new System.Drawing.Size(56, 20);
			this.TextBoxCountry.TabIndex = 2;
			this.TextBoxCountry.Text = "";
			// 
			// LabelCountry
			// 
			this.LabelCountry.Location = new System.Drawing.Point(8, 120);
			this.LabelCountry.Name = "LabelCountry";
			this.LabelCountry.Size = new System.Drawing.Size(104, 23);
			this.LabelCountry.TabIndex = 10;
			this.LabelCountry.Text = "Country:";
			// 
			// Page3
			// 
			this.Page3.Controls.Add(this.ComboBoxKeyType);
			this.Page3.Controls.Add(this.DateTimeFrom);
			this.Page3.Controls.Add(this.label7);
			this.Page3.Controls.Add(this.panel2);
			this.Page3.Controls.Add(this.label8);
			this.Page3.Controls.Add(this.DateTimeTo);
			this.Page3.Controls.Add(this.label9);
			this.Page3.Controls.Add(this.label10);
			this.Page3.Controls.Add(this.ComboBoxKeyLength);
			this.Page3.Location = new System.Drawing.Point(4, 25);
			this.Page3.Name = "Page3";
			this.Page3.Size = new System.Drawing.Size(408, 203);
			this.Page3.TabIndex = 2;
			// 
			// ComboBoxKeyType
			// 
			this.ComboBoxKeyType.Items.AddRange(new object[] {
																 "RSA",
																 "DSA"});
			this.ComboBoxKeyType.Location = new System.Drawing.Point(136, 120);
			this.ComboBoxKeyType.Name = "ComboBoxKeyType";
			this.ComboBoxKeyType.Size = new System.Drawing.Size(256, 21);
			this.ComboBoxKeyType.TabIndex = 2;
			this.ComboBoxKeyType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxKeyType_SelectedIndexChanged);
			// 
			// DateTimeFrom
			// 
			this.DateTimeFrom.Location = new System.Drawing.Point(136, 56);
			this.DateTimeFrom.Name = "DateTimeFrom";
			this.DateTimeFrom.Size = new System.Drawing.Size(256, 20);
			this.DateTimeFrom.TabIndex = 0;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 152);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 23);
			this.label7.TabIndex = 16;
			this.label7.Text = "Key Length";
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.HighlightText;
			this.panel2.Controls.Add(this.label4);
			this.panel2.Location = new System.Drawing.Point(0, -8);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(408, 40);
			this.panel2.TabIndex = 14;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(392, 24);
			this.label4.TabIndex = 0;
			this.label4.Text = "Key Information";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 56);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 23);
			this.label8.TabIndex = 16;
			this.label8.Text = "Valid From:";
			// 
			// DateTimeTo
			// 
			this.DateTimeTo.Location = new System.Drawing.Point(136, 88);
			this.DateTimeTo.Name = "DateTimeTo";
			this.DateTimeTo.Size = new System.Drawing.Size(256, 20);
			this.DateTimeTo.TabIndex = 1;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(8, 88);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 23);
			this.label9.TabIndex = 16;
			this.label9.Text = "Valid To:";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(8, 120);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 23);
			this.label10.TabIndex = 16;
			this.label10.Text = "Key Type:";
			// 
			// ComboBoxKeyLength
			// 
			this.ComboBoxKeyLength.Items.AddRange(new object[] {
																   "512",
																   "768",
																   "1024"});
			this.ComboBoxKeyLength.Location = new System.Drawing.Point(136, 152);
			this.ComboBoxKeyLength.Name = "ComboBoxKeyLength";
			this.ComboBoxKeyLength.Size = new System.Drawing.Size(256, 21);
			this.ComboBoxKeyLength.TabIndex = 3;
			// 
			// Page4
			// 
			this.Page4.Controls.Add(this.ButtonCredentialsBrowse);
			this.Page4.Controls.Add(this.TextBoxCredentialsFilename);
			this.Page4.Controls.Add(this.LabelFilename);
			this.Page4.Controls.Add(this.CheckBoxSelfSignCertificate);
			this.Page4.Controls.Add(this.panel3);
			this.Page4.Controls.Add(this.LabelCredentialsPassword);
			this.Page4.Controls.Add(this.TextBoxCredentialsPassword);
			this.Page4.Location = new System.Drawing.Point(4, 25);
			this.Page4.Name = "Page4";
			this.Page4.Size = new System.Drawing.Size(408, 203);
			this.Page4.TabIndex = 3;
			// 
			// ButtonCredentialsBrowse
			// 
			this.ButtonCredentialsBrowse.Enabled = false;
			this.ButtonCredentialsBrowse.Location = new System.Drawing.Point(320, 104);
			this.ButtonCredentialsBrowse.Name = "ButtonCredentialsBrowse";
			this.ButtonCredentialsBrowse.TabIndex = 19;
			this.ButtonCredentialsBrowse.Text = "Browse";
			this.ButtonCredentialsBrowse.Click += new System.EventHandler(this.ButtonCredentialsBrowse_Click);
			// 
			// TextBoxCredentialsFilename
			// 
			this.TextBoxCredentialsFilename.Enabled = false;
			this.TextBoxCredentialsFilename.Location = new System.Drawing.Point(72, 104);
			this.TextBoxCredentialsFilename.Name = "TextBoxCredentialsFilename";
			this.TextBoxCredentialsFilename.Size = new System.Drawing.Size(240, 20);
			this.TextBoxCredentialsFilename.TabIndex = 1;
			this.TextBoxCredentialsFilename.Text = "";
			// 
			// LabelFilename
			// 
			this.LabelFilename.Location = new System.Drawing.Point(8, 104);
			this.LabelFilename.Name = "LabelFilename";
			this.LabelFilename.Size = new System.Drawing.Size(64, 23);
			this.LabelFilename.TabIndex = 17;
			this.LabelFilename.Text = "Filename:";
			// 
			// CheckBoxSelfSignCertificate
			// 
			this.CheckBoxSelfSignCertificate.Location = new System.Drawing.Point(8, 56);
			this.CheckBoxSelfSignCertificate.Name = "CheckBoxSelfSignCertificate";
			this.CheckBoxSelfSignCertificate.Size = new System.Drawing.Size(288, 24);
			this.CheckBoxSelfSignCertificate.TabIndex = 0;
			this.CheckBoxSelfSignCertificate.Text = "Self sign certificate";
			this.CheckBoxSelfSignCertificate.CheckedChanged += new System.EventHandler(this.CheckBoxSelfSignCertificate_CheckedChanged);
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.HighlightText;
			this.panel3.Controls.Add(this.label5);
			this.panel3.Location = new System.Drawing.Point(0, -8);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(408, 40);
			this.panel3.TabIndex = 15;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label5.Location = new System.Drawing.Point(8, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(392, 24);
			this.label5.TabIndex = 0;
			this.label5.Text = "Signer Credentials File Information:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelCredentialsPassword
			// 
			this.LabelCredentialsPassword.Location = new System.Drawing.Point(8, 136);
			this.LabelCredentialsPassword.Name = "LabelCredentialsPassword";
			this.LabelCredentialsPassword.Size = new System.Drawing.Size(64, 23);
			this.LabelCredentialsPassword.TabIndex = 17;
			this.LabelCredentialsPassword.Text = "Password:";
			// 
			// TextBoxCredentialsPassword
			// 
			this.TextBoxCredentialsPassword.Enabled = false;
			this.TextBoxCredentialsPassword.Location = new System.Drawing.Point(72, 136);
			this.TextBoxCredentialsPassword.Name = "TextBoxCredentialsPassword";
			this.TextBoxCredentialsPassword.PasswordChar = '*';
			this.TextBoxCredentialsPassword.Size = new System.Drawing.Size(240, 20);
			this.TextBoxCredentialsPassword.TabIndex = 2;
			this.TextBoxCredentialsPassword.Text = "";
			// 
			// Page5
			// 
			this.Page5.Controls.Add(this.TextBoxCertificateFileRoot);
			this.Page5.Controls.Add(this.ButtonCertificateBrowse); 
			this.Page5.Controls.Add(this.ButtonCertificatePathBrowse); 
			this.Page5.Controls.Add(this.label11);
			this.Page5.Controls.Add(this.panel4);
			this.Page5.Controls.Add(this.label12);
			this.Page5.Controls.Add(this.label13);
			this.Page5.Controls.Add(this.label14);
			this.Page5.Controls.Add(this.label15);
			this.Page5.Controls.Add(this.TextBoxCertificateFilename);
			this.Page5.Controls.Add(this.TextBoxKeyFile);
			this.Page5.Controls.Add(this.TextBoxCertificatePassword);
			this.Page5.Controls.Add(this.TextBoxCertificateReEnterPassword);
            this.Page5.Controls.Add(this.CheckBoxUseGeneratedCertificate);
            this.Page5.Location = new System.Drawing.Point(4, 25);
			this.Page5.Name = "Page5";
			this.Page5.Size = new System.Drawing.Size(408, 203);
			this.Page5.TabIndex = 4;
			// 
			// TextBoxCertificateFileRoot
			// 
			this.TextBoxCertificateFileRoot.Location = new System.Drawing.Point(120, 48);
			this.TextBoxCertificateFileRoot.Name = "TextBoxCertificateFileRoot";
			this.TextBoxCertificateFileRoot.ReadOnly = true;
			this.TextBoxCertificateFileRoot.Size = new System.Drawing.Size(192, 20);
			this.TextBoxCertificateFileRoot.TabIndex = 0;
			this.TextBoxCertificateFileRoot.Text = "";
			// 
			// ButtonCertificateBrowse
			// 
			this.ButtonCertificateBrowse.Location = new System.Drawing.Point(320, 48);
			this.ButtonCertificateBrowse.Name = "ButtonCertificateBrowse";
			this.ButtonCertificateBrowse.TabIndex = 20;
			this.ButtonCertificateBrowse.Text = "Browse";
			this.ButtonCertificateBrowse.Click += new System.EventHandler(this.ButtonCertificateBrowse_Click);

			// 
			// label11
			// 
            this.ButtonCertificatePathBrowse.Location = new System.Drawing.Point(320, 80);
            this.ButtonCertificatePathBrowse.Name = "ButtonCertificatePathBrowse";
            this.ButtonCertificatePathBrowse.TabIndex = 21;
            this.ButtonCertificatePathBrowse.Text = "Browse";
            this.ButtonCertificatePathBrowse.Click += new System.EventHandler(this.ButtonCertificatePathBrowse_Click);
            // 
            // CheckBoxUseGeneratedCerticate
            // 
            this.CheckBoxUseGeneratedCertificate.Location = new System.Drawing.Point(320, 74);
            this.CheckBoxUseGeneratedCertificate.Name = "CheckBoxUseGeneratedCertificate";
            this.CheckBoxUseGeneratedCertificate.Size = new System.Drawing.Size(320, 100);
            this.CheckBoxUseGeneratedCertificate.TabIndex = 0;
            this.CheckBoxUseGeneratedCertificate.Text = "Use";
			this.CheckBoxUseGeneratedCertificate.Enabled = true;
			this.CheckBoxUseGeneratedCertificate.Checked = true;
            this.CheckBoxUseGeneratedCertificate.CheckedChanged += new System.EventHandler(this.CheckBoxUseGeneratedCertificate_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(8, 48);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(104, 23);
			this.label11.TabIndex = 17;
			this.label11.Text = "Fileroot:";
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.SystemColors.HighlightText;
			this.panel4.Controls.Add(this.label6);
			this.panel4.Location = new System.Drawing.Point(0, -8);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(408, 40);
			this.panel4.TabIndex = 16;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label6.Location = new System.Drawing.Point(8, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(392, 24);
			this.label6.TabIndex = 0;
			this.label6.Text = "Output Files";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(8, 80);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(104, 23);
			this.label12.TabIndex = 17;
			this.label12.Text = "Certificate File:";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(8, 112);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(104, 23);
			this.label13.TabIndex = 17;
			this.label13.Text = "Credential File:";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 144);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 23);
			this.label14.TabIndex = 17;
			this.label14.Text = "Password:";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(8, 176);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(104, 23);
			this.label15.TabIndex = 17;
			this.label15.Text = "Re-enter Password:";
			// 
			// TextBoxCertificateFilename
			// 
			this.TextBoxCertificateFilename.Location = new System.Drawing.Point(120, 80);
			this.TextBoxCertificateFilename.Name = "TextBoxCertificateFilename";
			this.TextBoxCertificateFilename.Size = new System.Drawing.Size(192, 20);
			this.TextBoxCertificateFilename.TabIndex = 1;
			this.TextBoxCertificateFilename.Text = "";
			// 
			// TextBoxKeyFile
			// 
			this.TextBoxKeyFile.Location = new System.Drawing.Point(120, 112);
			this.TextBoxKeyFile.Name = "TextBoxKeyFile";
			this.TextBoxKeyFile.Size = new System.Drawing.Size(192, 20);
			this.TextBoxKeyFile.TabIndex = 2;
			this.TextBoxKeyFile.Text = "";
			// 
			// TextBoxCertificatePassword
			// 
			this.TextBoxCertificatePassword.Location = new System.Drawing.Point(120, 144);
			this.TextBoxCertificatePassword.Name = "TextBoxCertificatePassword";
			this.TextBoxCertificatePassword.PasswordChar = '*';
			this.TextBoxCertificatePassword.Size = new System.Drawing.Size(192, 20);
			this.TextBoxCertificatePassword.TabIndex = 3;
			this.TextBoxCertificatePassword.Text = "";
			// 
			// TextBoxCertificateReEnterPassword
			// 
			this.TextBoxCertificateReEnterPassword.Location = new System.Drawing.Point(120, 176);
			this.TextBoxCertificateReEnterPassword.Name = "TextBoxCertificateReEnterPassword";
			this.TextBoxCertificateReEnterPassword.PasswordChar = '*';
			this.TextBoxCertificateReEnterPassword.Size = new System.Drawing.Size(192, 20);
			this.TextBoxCertificateReEnterPassword.TabIndex = 4;
			this.TextBoxCertificateReEnterPassword.Text = "";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(8, 208);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(416, 8);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			// 
			// ButtonCancel
			// 
			this.ButtonCancel.Location = new System.Drawing.Point(344, 232);
			this.ButtonCancel.Name = "ButtonCancel";
			this.ButtonCancel.TabIndex = 3;
			this.ButtonCancel.Text = "Cancel";
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
			// 
			// ButtonNext
			// 
			this.ButtonNext.Location = new System.Drawing.Point(248, 232);
			this.ButtonNext.Name = "ButtonNext";
			this.ButtonNext.TabIndex = 2;
			this.ButtonNext.Text = "Next >>";
			this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
			// 
			// ButtonPrev
			// 
			this.ButtonPrev.Enabled = false;
			this.ButtonPrev.Location = new System.Drawing.Point(168, 232);
			this.ButtonPrev.Name = "ButtonPrev";
			this.ButtonPrev.TabIndex = 1;
			this.ButtonPrev.Text = "<< Prev";
			this.ButtonPrev.Click += new System.EventHandler(this.ButtonPrev_Click);
			// 
			// OpenCertificateFileDialog
			// 
			this.OpenCertificateFileDialog.DefaultExt = "pem";
			this.OpenCertificateFileDialog.Filter = "Certificate files (*.pem)|*.pem";			
            // 
            // WizardCreateCertificate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(430, 270);
			this.Controls.Add(this.WizardPages);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.ButtonPrev);
			this.Controls.Add(this.ButtonNext);
			this.Controls.Add(this.ButtonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WizardCreateCertificate";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Create New Certificate Wizard";
			this.Load += new System.EventHandler(this.WizardCreateCertificate_Load);
			this.WizardPages.ResumeLayout(false);
			this.Page1.ResumeLayout(false);
			this.PanelPage1.ResumeLayout(false);
			this.Page2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.Page3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.Page4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.Page5.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

        private int         current_page;
        private Dvtk.Certificates.CertificateGenerator  cert = null;

        private void ButtonPrev_Click(object sender, System.EventArgs e)
        {
            switch (this.current_page)
            {
                case 1:     //
                    // This should never happen.
                    break;
                case 2:     //
                    this.current_page = 1;
                    this.ButtonNext.Enabled = true;
                    this.ButtonPrev.Enabled = false;
                    this.WizardPages.SelectedTab = this.Page1;
                    break;
                case 3:     //
                    this.current_page = 2;
                    this.ButtonNext.Enabled = true;
                    this.ButtonPrev.Enabled = true;
                    this.WizardPages.SelectedTab = this.Page2;
                    break;
                case 4:     //
                    this.current_page = 3;
                    this.ButtonNext.Enabled = true;
                    this.ButtonPrev.Enabled = true;
                    this.WizardPages.SelectedTab = this.Page3;
                    break;
                case 5:     //
                    this.current_page = 4;
                    this.ButtonNext.Enabled = true;
                    this.ButtonNext.Text = "Next >>";
                    this.ButtonPrev.Enabled = true;
                    this.WizardPages.SelectedTab = this.Page4;
                    break;
            }
        }

        private void ButtonNext_Click(object sender, System.EventArgs e)
        {
            switch (this.current_page)
            {
                case 1:     //
                    if (this.CheckPage1Settings())
                    {
                        this.current_page = 2;
                        this.ButtonNext.Enabled = true;
                        this.ButtonPrev.Enabled = true;
                        this.WizardPages.SelectedTab = this.Page2;
                    }
                    break;
                case 2:     //
                    if (this.CheckPage2Settings())
                    {
                        this.current_page = 3;
                        this.ButtonNext.Enabled = true;
                        this.ButtonPrev.Enabled = true;
                        this.WizardPages.SelectedTab = this.Page3;
                    }
                    break;
                case 3:     //
                    this.current_page = 4;
                    this.ButtonNext.Enabled = true;
                    this.ButtonPrev.Enabled = true;
                    this.WizardPages.SelectedTab = this.Page4;
                    break;
                case 4:     //
                    this.current_page = 5;
                    this.ButtonNext.Enabled = true;
                    this.ButtonNext.Text = "Generate";
                    this.ButtonPrev.Enabled = true;
                    this.WizardPages.SelectedTab = this.Page5;
                    break;
                case 5:     //
                    if (this.CheckPage5Settings())
                    {
                        if (this.GenerateCertificate ())
                            this.Close ();
                    }
                    break;
            }
        }

        private void ButtonCancel_Click(object sender, System.EventArgs e)
        {
            this.Close ();
        }

        private void ComboBoxKeyType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.ComboBoxKeyType.SelectedIndex == 0)
            {
                // Selected RSA
                this.ComboBoxKeyLength.Items.Add ("1536");
                this.ComboBoxKeyLength.Items.Add ("2048");
            }
            else
            {
                // Selected DSA
                if (this.ComboBoxKeyLength.SelectedIndex > 2)
                    this.ComboBoxKeyLength.SelectedIndex = 2;
                this.ComboBoxKeyLength.Items.Remove ("1536");
                this.ComboBoxKeyLength.Items.Remove ("2048");
            }
        }

        private void CheckBoxSelfSignCertificate_CheckedChanged(object sender, System.EventArgs e)
        {
            this.TextBoxCredentialsPassword.Enabled = !this.CheckBoxSelfSignCertificate.Checked;
            this.ButtonCredentialsBrowse.Enabled = !this.CheckBoxSelfSignCertificate.Checked;
        }

        private void ButtonCredentialsBrowse_Click(object sender, System.EventArgs e)
        {
            this.OpenCertificateFileDialog.Title = "Select Credentials File";
            this.OpenCertificateFileDialog.CheckFileExists = true;
            if (this.OpenCertificateFileDialog.ShowDialog (this) ==  DialogResult.OK)
            {
                this.TextBoxCredentialsFilename.Text = this.OpenCertificateFileDialog.FileName;
            }
        }

        private void ButtonCertificateBrowse_Click(object sender, System.EventArgs e)
        {
            this.OpenCertificateFileDialog.Title = "Select Certificate File Root";
            this.OpenCertificateFileDialog.CheckFileExists = false;
            if (this.OpenCertificateFileDialog.ShowDialog (this) ==  DialogResult.OK)
            {
                FileInfo file = new FileInfo (this.OpenCertificateFileDialog.FileName);
                this.TextBoxCertificateFileRoot.Text = file.Name;
                this.TextBoxCertificateFilename.Text = file.FullName;
                this.TextBoxKeyFile.Text = file.DirectoryName + "\\key" + file.Name;
            }
        }
        private void ButtonCertificatePathBrowse_Click(object sender, System.EventArgs e)
        {
            if (this.OpenCertificatePathFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.TextBoxCertificateFilename.Text = this.OpenCertificatePathFileDialog.SelectedPath + @"\" + this.TextBoxCertificateFilename.Text;
				this.TextBoxKeyFile.Text = this.OpenCertificatePathFileDialog.SelectedPath + @"\" + this.TextBoxKeyFile.Text;
            }
        }
        private void CheckBoxUseGeneratedCertificate_CheckedChanged(object sender, System.EventArgs e)
        {
			SetUseGeneratedCertificate(CheckBoxUseGeneratedCertificate.Checked);
        }

        private bool CheckPage1Settings ()
        {
            char[]  not_allowed= "/=".ToCharArray();
            if ((this.TextBoxName.Text.IndexOfAny (not_allowed) >= 0) ||
                (this.TextBoxDepartment.Text.IndexOfAny (not_allowed) >= 0) ||
                (this.TextBoxOrganization.Text.IndexOfAny (not_allowed) >= 0))
            {
                MessageBox.Show (this, "The characters '/' and '=' are not allowed in any of the fields.",
                                 "Invalid values",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool CheckPage2Settings ()
        {
            char[]  not_allowed= "/=".ToCharArray();
            if ((this.TextBoxCity.Text.IndexOfAny (not_allowed) >= 0) ||
                (this.TextBoxState.Text.IndexOfAny (not_allowed) >= 0) ||
                (this.TextBoxCountry.Text.IndexOfAny (not_allowed) >= 0))
            {
                MessageBox.Show (this, "The characters '/' and '=' are not allowed in any of the fields.",
                    "Invalid values",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool CheckPage5Settings ()
        {
            if (this.TextBoxCertificatePassword.Text != this.TextBoxCertificateReEnterPassword.Text)
            {
                MessageBox.Show (this, "Certificate password mismatch.",
                                 "Password mismatch",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool GenerateCertificate ()
        {
            this.cert = new Dvtk.Certificates.CertificateGenerator();
            cert.CertificateFile = this.TextBoxCertificateFilename.Text;
            cert.CommonName = this.TextBoxName.Text;
            cert.Country = this.TextBoxCountry.Text;
            cert.SelfSign = this.CheckBoxSelfSignCertificate.Checked;
            if (this.CheckBoxSelfSignCertificate.Checked)
                cert.CredentialsFile = null;
            else
                cert.CredentialsFile = this.TextBoxCredentialsFilename.Text;
            if (this.TextBoxCredentialsPassword.Text == "")
                cert.CredentialsFilePassword = MainForm.DEFAULT_CERTIFICATE_FILE_PASSWORD;
            cert.CredentialsFilePassword = this.TextBoxCredentialsPassword.Text;
            cert.PrivateKeyFile = this.TextBoxKeyFile.Text;
            cert.SignatureKeyLength = Convert.ToInt32 (this.ComboBoxKeyLength.SelectedItem);
            cert.PrivateKeyPassword = this.TextBoxCertificatePassword.Text;
            if (this.ComboBoxKeyType.SelectedIndex == 0)
                cert.SignatureAlgorithm = Dvtk.Certificates.SignatureAlgorithm.RSA;
            else
                cert.SignatureAlgorithm = Dvtk.Certificates.SignatureAlgorithm.DSA;
            cert.Locality = this.TextBoxCity.Text;
            cert.Organization = this.TextBoxOrganization.Text;
            cert.OrganizationalUnit = this.TextBoxDepartment.Text;
            cert.State = this.TextBoxState.Text;
            cert.ValidFromDate = this.DateTimeFrom.Value;
            cert.ValidToDate = this.DateTimeTo.Value;

            try
            {
                cert.Generate ();
            }
            catch (System.ApplicationException e)
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "An error occurred during generation of the certificate.\n"+
                    "Due to error:{0}",
                    e.ToString());
                const string caption = "Error generating certificate.";
                MessageBox.Show(
                    this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void WizardCreateCertificate_Load(object sender, System.EventArgs e)
        {
            this.TextBoxCertificateFilename.Text = 
                Dvtk.Certificates.CertificateGenerator.DefaultCertificateFile;
            this.TextBoxKeyFile.Text = 
                Dvtk.Certificates.CertificateGenerator.DefaultPrivateKeyFile;
        }

        private void comboBoxCountry_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            System.Windows.Forms.ComboBox list = (System.Windows.Forms.ComboBox)sender;
            this.TextBoxCountry.Text = list.SelectedItem.ToString().Substring(0, 2);
        }
	}
}
