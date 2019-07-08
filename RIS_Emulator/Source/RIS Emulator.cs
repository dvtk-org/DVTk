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
using System.Diagnostics;

using DvtkApplicationLayer;
using DvtkApplicationLayer.UserInterfaces;
using DvtkApplicationLayer.StoredFiles;

using DvtkHighLevelInterface.Common.UserInterfaces;
using DvtkHighLevelInterface.Common.Threads;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Dicom.UserInterfaces;
using DvtkHighLevelInterface.InformationModel;
using DataSet = DvtkHighLevelInterface.Dicom.Other.DataSet;

namespace RIS_Emulator
{
	/// <summary>
	/// The main RIS emulator Form.
	/// </summary>
	public class RisEmulator : System.Windows.Forms.Form
    {
        private IContainer components;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Button buttonForward;
		private System.Windows.Forms.Button buttonBackward;
		private System.Windows.Forms.Button buttonTop;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageWorklist;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonExploreDicomFiles;
		private System.Windows.Forms.Button buttonShowInformationModel;
		private DvtkHighLevelInterface.Dicom.UserInterfaces.DicomThreadOptionsUserControl worklistOptionsControl;
		private System.Windows.Forms.TabPage tabPageMPPS;
		private DvtkHighLevelInterface.Dicom.UserInterfaces.DicomThreadOptionsUserControl mppsOptionsControl;
		private System.Windows.Forms.TabPage tabPageDCMEditor;
		private DvtkApplicationLayer.UserInterfaces.DCMEditor dcmEditorRISEmulator;
		private System.Windows.Forms.TabPage tabPageResults;
		private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkWebBrowserResults;
		private System.Windows.Forms.TabPage tabPageActivityLogging;
		private DvtkHighLevelInterface.Common.UserInterfaces.UserControlActivityLogging userControlActivityLogging;
		private System.Windows.Forms.MainMenu mainMenuRISEmulator;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemExit;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.CheckBox checkBoxSelectDir;
		private System.Windows.Forms.TextBox textBoxDataDir;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.Button buttonSelectMode;
        private MenuItem menuConfig;
        private MenuItem menuConfigLoad;
        private MenuItem menuConfigSave;
        private MenuItem menuItemStoredFiles;
        private MenuItem menuItemStoredFilesExploreValidationResults;
        private MenuItem menuItem5;
        private MenuItem menuItemStoredFilesOptions;
        private CheckBox checkBoxSPSD;
		private System.Windows.Forms.Button buttonTS;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RisEmulator));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonTS = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonBackward = new System.Windows.Forms.Button();
            this.buttonTop = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonSelectMode = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageWorklist = new System.Windows.Forms.TabPage();
            this.checkBoxSPSD = new System.Windows.Forms.CheckBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxDataDir = new System.Windows.Forms.TextBox();
            this.checkBoxSelectDir = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExploreDicomFiles = new System.Windows.Forms.Button();
            this.buttonShowInformationModel = new System.Windows.Forms.Button();
            this.tabPageMPPS = new System.Windows.Forms.TabPage();
            this.tabPageDCMEditor = new System.Windows.Forms.TabPage();
            this.dcmEditorRISEmulator = new DvtkApplicationLayer.UserInterfaces.DCMEditor();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.dvtkWebBrowserResults = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.tabPageActivityLogging = new System.Windows.Forms.TabPage();
            this.mainMenuRISEmulator = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuConfig = new System.Windows.Forms.MenuItem();
            this.menuConfigLoad = new System.Windows.Forms.MenuItem();
            this.menuConfigSave = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFiles = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesExploreValidationResults = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemStoredFilesOptions = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.worklistOptionsControl = new DvtkHighLevelInterface.Dicom.UserInterfaces.DicomThreadOptionsUserControl();
            this.mppsOptionsControl = new DvtkHighLevelInterface.Dicom.UserInterfaces.DicomThreadOptionsUserControl();
            this.userControlActivityLogging = new DvtkHighLevelInterface.Common.UserInterfaces.UserControlActivityLogging();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageWorklist.SuspendLayout();
            this.tabPageMPPS.SuspendLayout();
            this.tabPageDCMEditor.SuspendLayout();
            this.tabPageResults.SuspendLayout();
            this.tabPageActivityLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonTS);
            this.panel1.Controls.Add(this.buttonStop);
            this.panel1.Controls.Add(this.buttonForward);
            this.panel1.Controls.Add(this.buttonBackward);
            this.panel1.Controls.Add(this.buttonTop);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonStart);
            this.panel1.Controls.Add(this.buttonSelectMode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(144, 530);
            this.panel1.TabIndex = 0;
            // 
            // buttonTS
            // 
            this.buttonTS.Location = new System.Drawing.Point(19, 222);
            this.buttonTS.Name = "buttonTS";
            this.buttonTS.Size = new System.Drawing.Size(96, 26);
            this.buttonTS.TabIndex = 3;
            this.buttonTS.Text = "Specify TS";
            this.buttonTS.Click += new System.EventHandler(this.buttonTS_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(24, 166);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(90, 27);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonForward
            // 
            this.buttonForward.Image = ((System.Drawing.Image)(resources.GetObject("buttonForward.Image")));
            this.buttonForward.Location = new System.Drawing.Point(72, 360);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(48, 27);
            this.buttonForward.TabIndex = 7;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // buttonBackward
            // 
            this.buttonBackward.Image = ((System.Drawing.Image)(resources.GetObject("buttonBackward.Image")));
            this.buttonBackward.Location = new System.Drawing.Point(24, 360);
            this.buttonBackward.Name = "buttonBackward";
            this.buttonBackward.Size = new System.Drawing.Size(48, 27);
            this.buttonBackward.TabIndex = 6;
            this.buttonBackward.Click += new System.EventHandler(this.buttonBackward_Click);
            // 
            // buttonTop
            // 
            this.buttonTop.Image = ((System.Drawing.Image)(resources.GetObject("buttonTop.Image")));
            this.buttonTop.Location = new System.Drawing.Point(48, 332);
            this.buttonTop.Name = "buttonTop";
            this.buttonTop.Size = new System.Drawing.Size(48, 27);
            this.buttonTop.TabIndex = 5;
            this.buttonTop.Click += new System.EventHandler(this.buttonTop_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(24, 277);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(90, 26);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(24, 111);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(90, 26);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonSelectMode
            // 
            this.buttonSelectMode.Location = new System.Drawing.Point(19, 55);
            this.buttonSelectMode.Name = "buttonSelectMode";
            this.buttonSelectMode.Size = new System.Drawing.Size(96, 27);
            this.buttonSelectMode.TabIndex = 0;
            this.buttonSelectMode.Text = "Select Mode";
            this.buttonSelectMode.Click += new System.EventHandler(this.buttonSelectMode_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(842, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 530);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(144, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 530);
            this.panel2.TabIndex = 10;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageWorklist);
            this.tabControl.Controls.Add(this.tabPageMPPS);
            this.tabControl.Controls.Add(this.tabPageDCMEditor);
            this.tabControl.Controls.Add(this.tabPageResults);
            this.tabControl.Controls.Add(this.tabPageActivityLogging);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(698, 530);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageWorklist
            // 
            this.tabPageWorklist.AutoScroll = true;
            this.tabPageWorklist.Controls.Add(this.worklistOptionsControl);
            this.tabPageWorklist.Controls.Add(this.checkBoxSPSD);
            this.tabPageWorklist.Controls.Add(this.buttonBrowse);
            this.tabPageWorklist.Controls.Add(this.textBoxDataDir);
            this.tabPageWorklist.Controls.Add(this.checkBoxSelectDir);
            this.tabPageWorklist.Controls.Add(this.label3);
            this.tabPageWorklist.Controls.Add(this.label1);
            this.tabPageWorklist.Controls.Add(this.buttonExploreDicomFiles);
            this.tabPageWorklist.Controls.Add(this.buttonShowInformationModel);
            this.tabPageWorklist.Location = new System.Drawing.Point(4, 25);
            this.tabPageWorklist.Name = "tabPageWorklist";
            this.tabPageWorklist.Size = new System.Drawing.Size(690, 501);
            this.tabPageWorklist.TabIndex = 3;
            this.tabPageWorklist.Text = "Worklist";
            // 
            // checkBoxSPSD
            // 
            this.checkBoxSPSD.AutoSize = true;
            this.checkBoxSPSD.Checked = true;
            this.checkBoxSPSD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSPSD.Location = new System.Drawing.Point(29, 443);
            this.checkBoxSPSD.Name = "checkBoxSPSD";
            this.checkBoxSPSD.Size = new System.Drawing.Size(421, 21);
            this.checkBoxSPSD.TabIndex = 8;
            this.checkBoxSPSD.Text = "Set Scheduled Procedure Step Date\\Time to current date\\time";
            this.checkBoxSPSD.UseVisualStyleBackColor = true;
            this.checkBoxSPSD.CheckedChanged += new System.EventHandler(this.checkBoxSPSD_CheckedChanged);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(557, 531);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(86, 26);
            this.buttonBrowse.TabIndex = 4;
            this.buttonBrowse.Text = "Browse....";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxDataDir
            // 
            this.textBoxDataDir.Location = new System.Drawing.Point(29, 531);
            this.textBoxDataDir.Name = "textBoxDataDir";
            this.textBoxDataDir.Size = new System.Drawing.Size(518, 22);
            this.textBoxDataDir.TabIndex = 3;
            // 
            // checkBoxSelectDir
            // 
            this.checkBoxSelectDir.Location = new System.Drawing.Point(29, 494);
            this.checkBoxSelectDir.Name = "checkBoxSelectDir";
            this.checkBoxSelectDir.Size = new System.Drawing.Size(336, 28);
            this.checkBoxSelectDir.TabIndex = 7;
            this.checkBoxSelectDir.Text = "Select data directory for sending WLM responses";
            this.checkBoxSelectDir.CheckedChanged += new System.EventHandler(this.checkBoxSelectDir_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(211, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(413, 27);
            this.label3.TabIndex = 5;
            this.label3.Text = "View the MWL information model constructed from the Dicom files.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(211, 372);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Import DICOM files to default data directory for emulating WLM responses.";
            // 
            // buttonExploreDicomFiles
            // 
            this.buttonExploreDicomFiles.Location = new System.Drawing.Point(29, 369);
            this.buttonExploreDicomFiles.Name = "buttonExploreDicomFiles";
            this.buttonExploreDicomFiles.Size = new System.Drawing.Size(173, 27);
            this.buttonExploreDicomFiles.TabIndex = 2;
            this.buttonExploreDicomFiles.Text = "Import Dicom files...";
            this.buttonExploreDicomFiles.Click += new System.EventHandler(this.buttonExploreDicomFiles_Click);
            // 
            // buttonShowInformationModel
            // 
            this.buttonShowInformationModel.Location = new System.Drawing.Point(29, 312);
            this.buttonShowInformationModel.Name = "buttonShowInformationModel";
            this.buttonShowInformationModel.Size = new System.Drawing.Size(173, 26);
            this.buttonShowInformationModel.TabIndex = 1;
            this.buttonShowInformationModel.Text = "View information model...";
            this.buttonShowInformationModel.Click += new System.EventHandler(this.buttonShowInformationModel_Click);
            // 
            // tabPageMPPS
            // 
            this.tabPageMPPS.AutoScroll = true;
            this.tabPageMPPS.Controls.Add(this.mppsOptionsControl);
            this.tabPageMPPS.Location = new System.Drawing.Point(4, 25);
            this.tabPageMPPS.Name = "tabPageMPPS";
            this.tabPageMPPS.Size = new System.Drawing.Size(690, 513);
            this.tabPageMPPS.TabIndex = 2;
            this.tabPageMPPS.Text = "MPPS";
            this.tabPageMPPS.Visible = false;
            // 
            // tabPageDCMEditor
            // 
            this.tabPageDCMEditor.Controls.Add(this.dcmEditorRISEmulator);
            this.tabPageDCMEditor.Location = new System.Drawing.Point(4, 25);
            this.tabPageDCMEditor.Name = "tabPageDCMEditor";
            this.tabPageDCMEditor.Size = new System.Drawing.Size(690, 513);
            this.tabPageDCMEditor.TabIndex = 5;
            this.tabPageDCMEditor.Text = "Edit DCM Files";
            this.tabPageDCMEditor.Visible = false;
            // 
            // dcmEditorRISEmulator
            // 
            this.dcmEditorRISEmulator.AutoScroll = true;
            this.dcmEditorRISEmulator.DCMFile = "";
            this.dcmEditorRISEmulator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmEditorRISEmulator.Location = new System.Drawing.Point(0, 0);
            this.dcmEditorRISEmulator.Name = "dcmEditorRISEmulator";
            this.dcmEditorRISEmulator.Size = new System.Drawing.Size(690, 513);
            this.dcmEditorRISEmulator.TabIndex = 0;
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.dvtkWebBrowserResults);
            this.tabPageResults.Location = new System.Drawing.Point(4, 25);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Size = new System.Drawing.Size(690, 513);
            this.tabPageResults.TabIndex = 0;
            this.tabPageResults.Text = "Results";
            this.tabPageResults.Visible = false;
            // 
            // dvtkWebBrowserResults
            // 
            this.dvtkWebBrowserResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvtkWebBrowserResults.Location = new System.Drawing.Point(0, 0);
            this.dvtkWebBrowserResults.Name = "dvtkWebBrowserResults";
            this.dvtkWebBrowserResults.Size = new System.Drawing.Size(690, 513);
            this.dvtkWebBrowserResults.TabIndex = 0;
            this.dvtkWebBrowserResults.XmlStyleSheetFullFileName = "";
            // 
            // tabPageActivityLogging
            // 
            this.tabPageActivityLogging.Controls.Add(this.userControlActivityLogging);
            this.tabPageActivityLogging.Location = new System.Drawing.Point(4, 25);
            this.tabPageActivityLogging.Name = "tabPageActivityLogging";
            this.tabPageActivityLogging.Size = new System.Drawing.Size(690, 513);
            this.tabPageActivityLogging.TabIndex = 4;
            this.tabPageActivityLogging.Text = "Activity Logging";
            this.tabPageActivityLogging.Visible = false;
            // 
            // mainMenuRISEmulator
            // 
            this.mainMenuRISEmulator.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItemStoredFiles,
            this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuConfig,
            this.menuItemExit});
            this.menuItem1.Text = "File";
            // 
            // menuConfig
            // 
            this.menuConfig.Index = 0;
            this.menuConfig.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuConfigLoad,
            this.menuConfigSave});
            this.menuConfig.Text = "Config File";
            // 
            // menuConfigLoad
            // 
            this.menuConfigLoad.Index = 0;
            this.menuConfigLoad.Text = "Load";
            this.menuConfigLoad.Click += new System.EventHandler(this.menuConfigLoad_Click);
            // 
            // menuConfigSave
            // 
            this.menuConfigSave.Index = 1;
            this.menuConfigSave.Text = "Save";
            this.menuConfigSave.Click += new System.EventHandler(this.menuConfigSave_Click);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 1;
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemStoredFiles
            // 
            this.menuItemStoredFiles.Index = 1;
            this.menuItemStoredFiles.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemStoredFilesExploreValidationResults,
            this.menuItem5,
            this.menuItemStoredFilesOptions});
            this.menuItemStoredFiles.Text = "Stored Files";
            // 
            // menuItemStoredFilesExploreValidationResults
            // 
            this.menuItemStoredFilesExploreValidationResults.Index = 0;
            this.menuItemStoredFilesExploreValidationResults.Text = "Explore Validation Results...";
            this.menuItemStoredFilesExploreValidationResults.Click += new System.EventHandler(this.menuItemStoredFilesExploreValidationResults_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "-";
            // 
            // menuItemStoredFilesOptions
            // 
            this.menuItemStoredFilesOptions.Index = 2;
            this.menuItemStoredFilesOptions.Text = "Options...";
            this.menuItemStoredFilesOptions.Click += new System.EventHandler(this.menuItemStoredFilesOptions_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItem3.Text = "About";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "About RIS Emulator";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // worklistOptionsControl
            // 
            this.worklistOptionsControl.DicomThreadOptions = null;
            this.worklistOptionsControl.LocalAeTitle = "";
            this.worklistOptionsControl.LocalAeTitleVisible = false;
            this.worklistOptionsControl.LocalPort = "";
            this.worklistOptionsControl.LocalPortVisible = false;
            this.worklistOptionsControl.Location = new System.Drawing.Point(10, 19);
            this.worklistOptionsControl.Name = "worklistOptionsControl";
            this.worklistOptionsControl.RemoteAeTitle = "";
            this.worklistOptionsControl.RemoteAeTitleVisible = false;
            this.worklistOptionsControl.RemoteIpAddress = "";
            this.worklistOptionsControl.RemoteIpAddressVisible = false;
            this.worklistOptionsControl.RemotePort = "";
            this.worklistOptionsControl.RemotePortVisible = false;
            this.worklistOptionsControl.Size = new System.Drawing.Size(401, 204);
            this.worklistOptionsControl.TabIndex = 0;
            // 
            // mppsOptionsControl
            // 
            this.mppsOptionsControl.DicomThreadOptions = null;
            this.mppsOptionsControl.LocalAeTitle = "";
            this.mppsOptionsControl.LocalAeTitleVisible = false;
            this.mppsOptionsControl.LocalPort = "";
            this.mppsOptionsControl.LocalPortVisible = false;
            this.mppsOptionsControl.Location = new System.Drawing.Point(10, 9);
            this.mppsOptionsControl.Name = "mppsOptionsControl";
            this.mppsOptionsControl.RemoteAeTitle = "";
            this.mppsOptionsControl.RemoteAeTitleVisible = false;
            this.mppsOptionsControl.RemoteIpAddress = "";
            this.mppsOptionsControl.RemoteIpAddressVisible = false;
            this.mppsOptionsControl.RemotePort = "";
            this.mppsOptionsControl.RemotePortVisible = false;
            this.mppsOptionsControl.Size = new System.Drawing.Size(489, 198);
            this.mppsOptionsControl.TabIndex = 0;
            // 
            // userControlActivityLogging
            // 
            this.userControlActivityLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlActivityLogging.Interval = 250;
            this.userControlActivityLogging.Location = new System.Drawing.Point(0, 0);
            this.userControlActivityLogging.Name = "userControlActivityLogging";
            this.userControlActivityLogging.Size = new System.Drawing.Size(690, 513);
            this.userControlActivityLogging.TabIndex = 0;
            // 
            // RisEmulator
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(846, 530);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenuRISEmulator;
            this.MinimumSize = new System.Drawing.Size(864, 577);
            this.Name = "RisEmulator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RIS Emulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RisEmulator_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageWorklist.ResumeLayout(false);
            this.tabPageWorklist.PerformLayout();
            this.tabPageMPPS.ResumeLayout(false);
            this.tabPageDCMEditor.ResumeLayout(false);
            this.tabPageResults.ResumeLayout(false);
            this.tabPageActivityLogging.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		//
		// - Fields -
		//
		/// <summary>
		/// Indicates if unsaved option changes exists.
		/// </summary>
		private bool hasUnsavedChanges = false;

		/// <summary>
		/// HliForm that may be useful for debugging purpose.
		/// Normally commented out.
		/// See also buttonStart_Click with commented out code.
		/// </summary>
		// private HliForm hliForm = null;

		/// <summary>
		/// Indicates if the emulator is running (i.e. handling incoming associations)
		/// </summary>
		private bool isRunning = false;

		/// <summary>
		/// The options for MPPS as SCP.
		/// </summary>
		private DicomThreadOptions mppsOptions = null;

		/// <summary>
		/// Parent thread of the other threads.
		/// </summary>
		private OverviewThread overviewThread = null;

		/// <summary>
		/// The root of this application (the application directory).
		/// </summary>
		private String rootPath = "";

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
		
		/// <summary>
		/// The options for Worklist as SCP.
		/// </summary>
		private DicomThreadOptions worklistOptions = null;

        IteratorClass worklistDicomThread = null;

		/// <summary>
		/// All configurable items.
		/// </summary>
		private Config config = null;

		private ArrayList selectedTS = new ArrayList();

		public static string dataDirectory = "";

        public static string dataDirectoryForTempFiles = "";

        public static bool isCurrentSPSD = true;

		int nrOfRsps = 1;

		bool modeOfRsp = true;

        private FileGroups fileGroups = null;

        private ValidationResultsFileGroup validationResultsFileGroup = null;

		//
		// - Delegates -
		//
		/// <summary>
		/// The handler that will be called when all threads have stopped.
		/// </summary>
		private delegate void ExecutionCompletedHandler();

		//
		// - Entry point -
		//
		/// <summary>
		/// The main entry point for this application.
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

            //
            // Initialize DVTK library.
            //
            Dvtk.Setup.Initialize();

			Application.Run(new RisEmulator());

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
		/// Default constructor.
		/// </summary>
		public RisEmulator()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            if (!Directory.Exists(ConfigurationsDirectory))
            {
                Directory.CreateDirectory(ConfigurationsDirectory);
            }

            this.config = Config.Deserialize(Path.Combine(ConfigurationsDirectory, "Config.xml"));
            Config.ConfigFullFileName = Path.Combine(ConfigurationsDirectory, "Config.xml");

			//
			// Initialize the browser control.
			//
            rootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\RIS Emulator";

			this.dvtkWebBrowserResults.XmlStyleSheetFullFileName = Path.Combine(Application.StartupPath, "DVT_RESULTS.xslt");

			if(config.TSILESupport)
				selectedTS.Add("1.2.840.10008.1.2");
			if(config.TSELESupport)
				selectedTS.Add("1.2.840.10008.1.2.1");
			if(config.TSEBESupport)
				selectedTS.Add("1.2.840.10008.1.2.2");

            //if (config.SPSDChecked)
            //    checkBoxSPSD.Checked = true;
            //else
            //    checkBoxSPSD.Checked = false;

			//
			// Set the .Net thread name for debugging purposes.
			//
			System.Threading.Thread.CurrentThread.Name = "RIS Emulator";

            //
            // Stored files options.
            //
            this.fileGroups = new FileGroups("RIS Emulator");

            this.validationResultsFileGroup = new ValidationResultsFileGroup();
            this.validationResultsFileGroup.DefaultFolder = "Results";
            this.fileGroups.Add(validationResultsFileGroup);

            this.fileGroups.CreateDirectories();

            this.fileGroups.CheckIsConfigured("\"Stored Files\\Options...\" menu item");

            Initialize();

			// Save the config so next time no attempt will be made to again try to load the same settings
			this.config.Serialize();

			//
			// Set the Backward/forward button handler.
			//
			this.dvtkWebBrowserResults.BackwardFormwardEnabledStateChangeEvent+= new DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(BackwardFormwardEnabledStateChangeEvent);

			UpdateButtons();

			//
			// Other.
			//
			this.threadsStateChangeEventHandler = new ThreadManager.ThreadsStateChangeEventHandler(this.HandleThreadsStateChangeEvent);

			// Load definition and DCM files
            dcmEditorRISEmulator.DCMFileDataDirectory = validationResultsFileGroup.Directory;
            dcmEditorRISEmulator.DefFile = this.worklistOptions.DvtkScriptSession.DefinitionManagement.DefinitionFileRootDirectory + "Modality Worklist Information Model - Find.def";
            dcmEditorRISEmulator.DefFile = this.worklistOptions.DvtkScriptSession.DefinitionManagement.DefinitionFileRootDirectory + "Modality Performed Procedure Step.def";
			dcmEditorRISEmulator.DCMFile = rootPath + @"\Data\Worklist\d1I00001.dcm";
			dataDirectory = this.worklistOptions.DataDirectory;

            //Update settings from the config XML
            if (config.DataDirectoryForEmulation != "")
                dataDirectory = config.DataDirectoryForEmulation;
            else
                config.DataDirectoryForEmulation = dataDirectory;

			buttonBrowse.Visible = false;
			textBoxDataDir.Visible = false;
			buttonStart.Enabled = true;
            modeOfRsp = true;

            dataDirectoryForTempFiles = validationResultsFileGroup.Directory;

            this.WindowState = FormWindowState.Maximized;
		}

        private void Initialize()
        {
            //
            // Construct the MPPS DicomOptions implicitly by constructing a DicomThread.
            //
            IteratorClass mppsDicomThread = new IteratorClass(rootPath, validationResultsFileGroup.Directory, "MPPS_SCP.ses", "MPPS");
            this.mppsOptions = mppsDicomThread.Options;

            //
            // Construct the Worklist DicomOptions implicitly by constructing a DicomThread.
            //
            worklistDicomThread = new IteratorClass(rootPath, validationResultsFileGroup.Directory, "WLM_SCP.ses", "Worklist");
            this.worklistOptions = worklistDicomThread.Options;

            //
            // Initialize the DicomThreadOptionsControls
            //
            this.mppsOptionsControl.DicomThreadOptions = this.mppsOptions;
            this.mppsOptionsControl.LocalAeTitleVisible = true;
            this.mppsOptionsControl.LocalPortVisible = true;
            this.mppsOptionsControl.RemoteAeTitleVisible = true;
            this.mppsOptionsControl.OptionChangedEvent += new DicomThreadOptionsUserControl.OptionChangedEventHandler(this.HandleOptionChanged);
            this.mppsOptionsControl.UpdateUserControl();

            this.worklistOptionsControl.DicomThreadOptions = this.worklistOptions;
            this.worklistOptionsControl.LocalAeTitleVisible = true;
            this.worklistOptionsControl.LocalPortVisible = true;
            this.worklistOptionsControl.RemoteAeTitleVisible = true;
            this.worklistOptionsControl.OptionChangedEvent += new DicomThreadOptionsUserControl.OptionChangedEventHandler(this.HandleOptionChanged);
            this.worklistOptionsControl.UpdateUserControl();
			
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scuAeTitle"></param>
        /// <param name="randomizeFirst"></param>
        /// <returns></returns>
        public static ModalityWorklistInformationModel CreateMWLInformationModel(bool randomizeFirst, DicomThread dicomThread)
        {
            dicomThread.WriteInformation(string.Format("Creating the MWL information model based on data directory : {0}", dataDirectory));
            ModalityWorklistInformationModel modalityWorklistInformationModel = new ModalityWorklistInformationModel();

            //Specify directory for temp DCM files
            
            modalityWorklistInformationModel.DataDirectory = dataDirectoryForTempFiles;

            DirectoryInfo directoryInfo = new DirectoryInfo(dataDirectory);

            FileInfo[] fileInfos = directoryInfo.GetFiles();

            if (fileInfos.Length != 0)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    try
                    {
                        DataSet dataSet = new DataSet();

                        dataSet.Read(fileInfo.FullName);

                        if (randomizeFirst)
                        {
                            dataSet.Randomize("@");
                        }

                        modalityWorklistInformationModel.AddToInformationModel(dataSet, true);
                    }
                    catch (Exception )
                    {
                        string theErrorText = string.Format("Invalid DICOM File - {0} will be skiped from MWL information model.\n\n", fileInfo.FullName);
                        dicomThread.WriteInformation(theErrorText);
                    }
                }
            }

            if (isCurrentSPSD)
            {
                
                //modalityWorklistInformationModel.AddDefaultAttributeToInformationModel(true, "0x00400001", DvtkData.Dimse.VR.AE, scuAeTitle);
                modalityWorklistInformationModel.AddDefaultAttributeToInformationModel(true, "0x00400100[1]/0x00400002", DvtkData.Dimse.VR.DA, System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                modalityWorklistInformationModel.AddDefaultAttributeToInformationModel(true, "0x00400100[1]/0x00400003", DvtkData.Dimse.VR.TM, System.DateTime.Now.ToString("HHmmss", System.Globalization.CultureInfo.InvariantCulture));
            }

            return modalityWorklistInformationModel;
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

		//
		// - Methods -
		//
		/// <summary>
		/// Is called when the enable state of the back and forward buttons should change.
		/// </summary>
		private void BackwardFormwardEnabledStateChangeEvent()
		{
			UpdateButtons();
		}            

		/// <summary>
		/// Go backward in the web browser.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void buttonBackward_Click(object sender, System.EventArgs e)
		{
			this.dvtkWebBrowserResults.Back();
		}

		/// <summary>
		/// Start explorer displaying the data directory in which the DCM files are present.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void buttonExploreDicomFiles_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog openMediaFileDialog = new OpenFileDialog();
			openMediaFileDialog.Filter = "DICOM media files (*.dcm)|*.dcm|All files (*.*)|*.*";
			openMediaFileDialog.Multiselect = true;
			openMediaFileDialog.Title = "Select DICOM files to import";
           // openMediaFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
				
			if (openMediaFileDialog.ShowDialog (this) == DialogResult.OK) 
			{
				string[] files = openMediaFileDialog.FileNames;
				foreach(string file in files) 
				{
					FileInfo dcmFile = new FileInfo(file);
					string destFileName = Path.Combine(dataDirectory, dcmFile.Name);
                    try
                    {
                        dcmFile.CopyTo(destFileName, true);
                    }
                    catch (Exception exception)
                    {
                        string theErrorText = string.Format("Illegal operation.\n {1}\n\n", destFileName, exception.Message);
                        MessageBox.Show(theErrorText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }					
				}
			}			
		}	

		/// <summary>
		/// Go forward in the web browser.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void buttonForward_Click(object sender, System.EventArgs e)
		{
			this.dvtkWebBrowserResults.Forward();
		}

		/// <summary>
		/// Save the options to file.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event.</param>
		private void buttonSave_Click(object sender, System.EventArgs e)
		{
            SaveToSessionFiles(this.worklistOptions, this.mppsOptions);

			this.hasUnsavedChanges = false;
			UpdateButtons();				
		}

		/// <summary>
		/// Show the information model that is derived from the DCM files in the data directory.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void buttonShowInformationModel_Click(object sender, System.EventArgs e)
		{
            ModalityWorklistInformationModel modalityWorklistInformationModel = CreateMWLInformationModel(false, worklistDicomThread);

			FormInformationModel formInformationModel = new FormInformationModel(modalityWorklistInformationModel.Dump());

			formInformationModel.ShowDialog();				
		}

		/// <summary>
		/// Select the mode for emulation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonSelectMode_Click(object sender, System.EventArgs e)
		{
			SelectMode mode = new SelectMode();
			if(mode.ShowDialog() == DialogResult.OK)
			{
				modeOfRsp = mode.C_FIND_RSP_MODE;
				if(!modeOfRsp)
				{
					nrOfRsps = mode.NrOfResponses;
					if(nrOfRsps == 0)
						return;
					tabControl.SelectedTab = this.tabPageDCMEditor;
				}
				buttonStart.Enabled = true;
			}
			else
			{
				//
				// Make the other Tabs visible again.
				//
				this.tabControl.Controls.Clear();
				this.tabControl.Controls.Add(this.tabPageWorklist);
				this.tabControl.Controls.Add(this.tabPageMPPS);
				this.tabControl.Controls.Add(this.tabPageDCMEditor);
				this.tabControl.Controls.Add(this.tabPageActivityLogging);
				this.tabControl.Controls.Add(this.tabPageResults);
			}
		}

		/// <summary>
		/// Start the emulator.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void buttonStart_Click(object sender, System.EventArgs e)
        {
            if (selectedTS.Contains("1.2.840.10008.1.2"))
                this.config.TSILESupport = true;
            else
                this.config.TSILESupport = false;

            if (selectedTS.Contains("1.2.840.10008.1.2.1"))
                this.config.TSELESupport = true;
            else
                this.config.TSELESupport = false;

            if (selectedTS.Contains("1.2.840.10008.1.2.2"))
                this.config.TSEBESupport = true;
            else
                this.config.TSEBESupport = false;

            this.config.DataDirectoryForEmulation = this.textBoxDataDir.Text;
            this.config.SPSDChecked = checkBoxSPSD.Checked;
            this.config.WorklistLocalAeTitle = this.worklistOptionsControl.LocalAeTitle;
            this.config.WorklistLocalPort = this.worklistOptionsControl.LocalPort;
            this.config.WorklistRemoteAeTitle = this.worklistOptionsControl.RemoteAeTitle;
            // config.WorkListRemoteIpAddress = this.worklistOptionsControl.RemoteIpAddress;
            this.config.MppsLocalAeTitle = this.mppsOptionsControl.LocalAeTitle;
            this.config.MppsLocalPort = this.mppsOptionsControl.LocalPort;
            this.config.MppsRemoteAeTitle = this.mppsOptionsControl.RemoteAeTitle;

            if (this.hasUnsavedChanges)
            {
                SaveToSessionFiles(this.worklistOptions, this.mppsOptions);
                this.hasUnsavedChanges = false;
                this.buttonSave.Enabled = false;
            }

			//
			// Make the Activity Logging Tab the onky Tab visible and clean it.
			//
			this.tabControl.Controls.Clear();
			this.tabControl.Controls.Add(this.tabPageActivityLogging);
			this.userControlActivityLogging.Clear();

			//
			// Set the correct settings for the overview DicomThread.
			//
			String resultsFileBaseName = "RIS_Emulator" + System.DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);

			this.threadManager = new ThreadManager();
			this.threadManager.ThreadsStateChangeEvent += this.threadsStateChangeEventHandler;
            Initialize();
            
            this.overviewThread = new OverviewThread(this.mppsOptions, this.worklistDicomThread, this.selectedTS, 
				modeOfRsp, dcmEditorRISEmulator.DCMFile, nrOfRsps);
			this.overviewThread.Initialize(threadManager);
            this.overviewThread.Options.ResultsDirectory = validationResultsFileGroup.Directory;
			this.overviewThread.Options.Identifier = resultsFileBaseName;
			this.overviewThread.Options.AttachChildsToUserInterfaces = true;
			this.overviewThread.Options.LogThreadStartingAndStoppingInParent = false;
			this.overviewThread.Options.LogWaitingForCompletionChildThreads = false;
			this.userControlActivityLogging.Attach(overviewThread);

            this.userControlActivityLogging.Attach(worklistDicomThread);
            //worklistDicomThread.WriteInformation(dcmEditorRISEmulator.DCMFile);
			//
			// Attach the HliForm to the emulator if needed for debugging purposed.
			// Normally commented out.
			//
			// this.hliForm = new HliForm();
			// hliForm.Attach(overviewThread);

			//
			// Start the DicomThread.
			//
			this.overviewThread.Start();

			this.isRunning = true;
			UpdateButtons();			

			// When all threads have stopped (because the user pressed the Stop button), the method HandleThreadsStateChangeEvent will take care
			// that the correct buttons and tabs will be enabled again. 
		}

		/// <summary>
		/// Called when the Stop button is pressed. Stops all running threads.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event.</param>
		private void buttonStop_Click(object sender, System.EventArgs e)
		{
			this.buttonStop.Enabled = false;
			Cursor.Current = Cursors.WaitCursor;
			this.threadManager.Stop();
		}

		/// <summary>
		/// Navigate to the main results.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		private void buttonTop_Click(object sender, System.EventArgs e)
		{
			this.dvtkWebBrowserResults.Navigate(this.topXmlResults);
		}

		/// <summary>
		/// Called when all threads have stopped.
		/// Takes care that buttons and tabs are enabled/disabled.
		/// </summary>
		private void HandleExecutionCompleted()
		{
			//
			// Make the other Tabs visible again.
			//
			this.tabControl.Controls.Clear();
			this.tabControl.Controls.Add(this.tabPageWorklist);
			this.tabControl.Controls.Add(this.tabPageMPPS);
			this.tabControl.Controls.Add(this.tabPageDCMEditor);
			this.tabControl.Controls.Add(this.tabPageActivityLogging);
			this.tabControl.Controls.Add(this.tabPageResults);			

			Cursor.Current = Cursors.Default;
			//
			// Show the new results.
			//
			this.topXmlResults = overviewThread.Options.DetailResultsFullFileName;
			this.dvtkWebBrowserResults.Navigate(topXmlResults);
			
			this.tabControl.SelectedTab = this.tabPageResults;

			this.isRunning = false;
			UpdateButtons();
		
			Cleanup();
		}

		/// <summary>
		/// Handle the event that an option has changed.
		/// This event is received from dicomThreadOptionsUserControl.
		/// </summary>
		private void HandleOptionChanged()
		{
			this.hasUnsavedChanges = true;
			UpdateButtons();
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

		/// <summary>
		/// Cleanup all temp pix files
		/// </summary>
		private void Cleanup()
		{
			//Remove all temporary files generated during tool execution
			ArrayList theFilesToRemove = new ArrayList();
			DirectoryInfo theDirectoryInfo = new DirectoryInfo(validationResultsFileGroup.Directory);
			FileInfo[] thePixFilesInfo;
			FileInfo[] theIdxFilesInfo;
            FileInfo[] thedcmFilesInfo;

			if (theDirectoryInfo.Exists)
			{
				thePixFilesInfo = theDirectoryInfo.GetFiles("*.pix");
				theIdxFilesInfo = theDirectoryInfo.GetFiles("*.idx");
                thedcmFilesInfo = theDirectoryInfo.GetFiles("*.dcm");

				foreach (FileInfo theFileInfo in thePixFilesInfo)
				{
					string thePixFileName = theFileInfo.FullName;

					theFilesToRemove.Add(thePixFileName);
				}

				foreach (FileInfo theFileInfo in theIdxFilesInfo)
				{
					string theIdxFileName = theFileInfo.FullName;

					theFilesToRemove.Add(theIdxFileName);
				}

                foreach (FileInfo theFileInfo in thedcmFilesInfo)
                {
                    string thedcmFileName = theFileInfo.FullName;

                    theFilesToRemove.Add(thedcmFileName);
                }                
			}

			//Delete all pix, dcm & idx files
			foreach(string theFileName in theFilesToRemove)
			{
				if (File.Exists(theFileName))
				{
					try
					{
						File.Delete(theFileName);
					}
					catch(Exception exception)
					{
						string theErrorText = string.Format("Could not be delete the {0} temporary file.\n due to exception: {1}\n\n", theFileName, exception.Message);
						MessageBox.Show(theErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}				
			}
		}

		/// <summary>
		/// Update the buttons.
		/// </summary>
		private void UpdateButtons()
		{
			if (this.isRunning)
			{
				this.buttonStart.Enabled = false;
				this.buttonStop.Enabled = true;
				this.buttonSave.Enabled = false;

                this.buttonTS.Enabled = false;
                this.buttonSelectMode.Enabled = false;

				this.buttonTop.Enabled = false;
				this.buttonBackward.Enabled = false;
				this.buttonForward.Enabled = false;
			}
			else
			{
				this.buttonStart.Enabled = true;
				this.buttonStop.Enabled = false;
				this.buttonSave.Enabled = this.hasUnsavedChanges;

                this.buttonTS.Enabled = true;
                this.buttonSelectMode.Enabled = true;

				this.buttonTop.Enabled = (this.topXmlResults.Length > 0);
				this.buttonBackward.Enabled = this.dvtkWebBrowserResults.IsBackwardEnabled;
				this.buttonForward.Enabled = this.dvtkWebBrowserResults.IsForwardEnabled;
			}
		}

		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			AboutForm about = new AboutForm("DVTk RIS Emulator");
			about.ShowDialog();
		}

		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			//If RIS Emulator is running then stop it
			if(isRunning)
				this.threadManager.Stop();

			this.Close();
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
				config.TSILESupport = false;
				config.TSELESupport = false;
				config.TSEBESupport = false;

				foreach (DvtkData.Dul.TransferSyntax ts in theSelectTransferSyntaxesForm.SupportedTransferSyntaxes)
				{
					selectedTS.Add(ts.UID);

					//Update the config XML
					if( ts.UID == DvtkData.Dul.TransferSyntax.Implicit_VR_Little_Endian.UID)
						config.TSILESupport = true;
					else if( ts.UID == DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian.UID)
						config.TSELESupport = true;
					else if( ts.UID == DvtkData.Dul.TransferSyntax.Explicit_VR_Big_Endian.UID)
						config.TSEBESupport = true;
					else
					{}
				}
				config.Serialize();
			}
		}

		private void checkBoxSelectDir_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxSelectDir.Checked)
			{
				buttonBrowse.Visible = true;
				textBoxDataDir.Visible = true;
				textBoxDataDir.Text = config.DataDirectoryForEmulation;
			}
			else
			{
				buttonBrowse.Visible = false;
				textBoxDataDir.Visible = false;
			}
		}

		private void buttonBrowse_Click(object sender, System.EventArgs e)
		{			
			FolderBrowserDialog theFolderBrowserDialog = new FolderBrowserDialog();
			theFolderBrowserDialog.Description = "Select Data directory:";
            theFolderBrowserDialog.ShowNewFolderButton = false;

			// Only if the current directory exists, set this directory in the dialog browser.
			if (textBoxDataDir.Text != "") 
			{
				DirectoryInfo theDirectoryInfo = new DirectoryInfo(textBoxDataDir.Text);

				if (theDirectoryInfo.Exists) 
				{
					theFolderBrowserDialog.SelectedPath = textBoxDataDir.Text;
				}

				if (theFolderBrowserDialog.ShowDialog (this) == DialogResult.OK) 
				{
					textBoxDataDir.Text = theFolderBrowserDialog.SelectedPath;
					DirectoryInfo theSelectedDir = new DirectoryInfo(textBoxDataDir.Text);
					dataDirectory = textBoxDataDir.Text;

					//Update the config XML
                    config.DataDirectoryForEmulation = textBoxDataDir.Text;
                    config.Serialize();

					if(theSelectedDir.GetFiles().Length != 0)
					{
						FileInfo file = (FileInfo)(theSelectedDir.GetFiles().GetValue(0));
						dcmEditorRISEmulator.DCMFile = file.FullName;
					}
				}
			}			
		}

        private String ConfigurationsDirectory
        {
            get
            {
                return (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"DVTk\RIS Emulator\Configurations"));
            }
        }

        private void menuConfigLoad_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(ConfigurationsDirectory))
            {
                Directory.CreateDirectory(ConfigurationsDirectory);
            }

			OpenFileDialog theOpenFileDialog = new OpenFileDialog();

            theOpenFileDialog.Filter = "Config files (*.xml) |*.xml";
			theOpenFileDialog.Multiselect = false;
			theOpenFileDialog.Title = "Load a config file";

            if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                Config.ConfigFullFileName = theOpenFileDialog.FileName;
                this.config=Config.Deserialize(Config.ConfigFullFileName);

                selectedTS.Clear();
                if (this.config.TSILESupport)
                    selectedTS.Add("1.2.840.10008.1.2");
                if (this.config.TSELESupport)
                    selectedTS.Add("1.2.840.10008.1.2.1");
                if (this.config.TSEBESupport)
                    selectedTS.Add("1.2.840.10008.1.2.2");

                this.textBoxDataDir.Text = this.config.DataDirectoryForEmulation;

                checkBoxSPSD.Checked = this.config.SPSDChecked;
                this.worklistOptionsControl.LocalAeTitle = this.config.WorklistLocalAeTitle;
                this.worklistOptionsControl.LocalPort = this.config.WorklistLocalPort;
                this.worklistOptionsControl.RemoteAeTitle = this.config.WorklistRemoteAeTitle;
                //this.worklistOptionsControl.RemoteIpAddress = config.WorkListRemoteIpAddress;
                this.mppsOptionsControl.LocalAeTitle = this.config.MppsLocalAeTitle;
                this.mppsOptionsControl.LocalPort = this.config.MppsLocalPort;
                this.mppsOptionsControl.RemoteAeTitle = this.config.MppsRemoteAeTitle;
               // this.mppsOptionsControl.RemoteIpAddress = config.MppsRemoteIpAddress;
                
            }
            

        }

        private void menuConfigSave_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(ConfigurationsDirectory))
            {
                Directory.CreateDirectory(ConfigurationsDirectory);
            }

            if (selectedTS.Contains("1.2.840.10008.1.2"))
                this.config.TSILESupport = true;
            else
                this.config.TSILESupport = false;

            if (selectedTS.Contains("1.2.840.10008.1.2.1"))
                this.config.TSELESupport = true;
            else
                this.config.TSELESupport = false;

            if (selectedTS.Contains("1.2.840.10008.1.2.2"))
                this.config.TSEBESupport = true;
            else
                this.config.TSEBESupport = false;
            
            this.config.DataDirectoryForEmulation = this.textBoxDataDir.Text ;
            this.config.SPSDChecked = checkBoxSPSD.Checked;
            this.config.WorklistLocalAeTitle = this.worklistOptionsControl.LocalAeTitle;
            this.config.WorklistLocalPort = this.worklistOptionsControl.LocalPort;
            this.config.WorklistRemoteAeTitle = this.worklistOptionsControl.RemoteAeTitle;
           // config.WorkListRemoteIpAddress = this.worklistOptionsControl.RemoteIpAddress;
            this.config.MppsLocalAeTitle = this.mppsOptionsControl.LocalAeTitle;
            this.config.MppsLocalPort = this.mppsOptionsControl.LocalPort;
            this.config.MppsRemoteAeTitle = this.mppsOptionsControl.RemoteAeTitle;
            //config.MppsRemoteIpAddress = this.mppsOptionsControl.RemoteIpAddress;

			SaveFileDialog saveSetUpFileDlg = new SaveFileDialog();
            saveSetUpFileDlg.InitialDirectory = ConfigurationsDirectory;
			saveSetUpFileDlg.Title = "Save the current configuration";
            saveSetUpFileDlg.Filter = "Config files (*.xml) |*.xml";
            if (saveSetUpFileDlg.ShowDialog() == DialogResult.OK)
            {
                Config.ConfigFullFileName = saveSetUpFileDlg.FileName;
                this.config.Serialize();
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
                this.worklistOptions.ResultsDirectory = validationResultsFileGroup.Directory;

                this.mppsOptions.ResultsDirectory = validationResultsFileGroup.Directory;

                // Make sure the session files contain the same information as the Stored Files user settings.
                SaveToSessionFiles(this.worklistOptions, this.mppsOptions);
            }
        }

        private void SaveToSessionFiles(DicomThreadOptions storageOptions, DicomThreadOptions storageCommitOptions)
        {
            this.worklistOptions.SaveToFile(Path.Combine(rootPath, "WLM_SCP.ses"));
            this.mppsOptions.SaveToFile(Path.Combine(rootPath, "MPPS_SCP.ses"));
        }

        private void checkBoxSPSD_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSPSD.Checked)
                isCurrentSPSD = true;
            else
                isCurrentSPSD = false;


        }

        private void RisEmulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If RIS Emulator is running then stop it
            if (isRunning)
                this.threadManager.Stop();

            Cleanup();

            fileGroups.RemoveFiles();
        }
	}	
}
