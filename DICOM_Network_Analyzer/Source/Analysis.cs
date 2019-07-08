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

namespace DICOMSniffer
{
	/// <summary>
	/// Summary description for Analysis.
	/// </summary>
	public class Analysis : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TabControl tabControlAnalysis;
		public System.Windows.Forms.TabPage tabPageLogging;
		public System.Windows.Forms.TabPage tabPageValidation;
		public System.Windows.Forms.RichTextBox activityLogging;
		public DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkWebBrowser;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton errorButton;
		private System.Windows.Forms.ImageList imageListAnalysis;
		private System.Windows.Forms.ToolBarButton warningButton;
		private System.Windows.Forms.ToolBarButton backwardButton;
		private System.Windows.Forms.ToolBarButton upwardButton;
		private System.Windows.Forms.ToolBarButton forwardButton;

		string xmlResultFile = "";
		string activityLogFile = "";

		public Analysis()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Provide the Style sheet to the DvtkWebBrowser user control
			this.dvtkWebBrowser.XmlStyleSheetFullFileName = Application.StartupPath + "\\DVT_RESULTS.xslt";

			this.dvtkWebBrowser.BackwardFormwardEnabledStateChangeEvent +=new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew.BackwardFormwardEnabledStateChangeEventHandler(dvtkWebBrowser_BackwardFormwardEnabledStateChangeEvent);
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Analysis));
			this.tabControlAnalysis = new System.Windows.Forms.TabControl();
			this.tabPageLogging = new System.Windows.Forms.TabPage();
			this.activityLogging = new System.Windows.Forms.RichTextBox();
			this.tabPageValidation = new System.Windows.Forms.TabPage();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.errorButton = new System.Windows.Forms.ToolBarButton();
			this.warningButton = new System.Windows.Forms.ToolBarButton();
			this.backwardButton = new System.Windows.Forms.ToolBarButton();
			this.upwardButton = new System.Windows.Forms.ToolBarButton();
			this.forwardButton = new System.Windows.Forms.ToolBarButton();
			this.imageListAnalysis = new System.Windows.Forms.ImageList(this.components);
			this.dvtkWebBrowser = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
			this.tabControlAnalysis.SuspendLayout();
			this.tabPageLogging.SuspendLayout();
			this.tabPageValidation.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlAnalysis
			// 
			this.tabControlAnalysis.Controls.Add(this.tabPageLogging);
			this.tabControlAnalysis.Controls.Add(this.tabPageValidation);
			this.tabControlAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlAnalysis.Location = new System.Drawing.Point(0, 28);
			this.tabControlAnalysis.Name = "tabControlAnalysis";
			this.tabControlAnalysis.SelectedIndex = 0;
			this.tabControlAnalysis.Size = new System.Drawing.Size(776, 642);
			this.tabControlAnalysis.TabIndex = 0;
			this.tabControlAnalysis.SelectedIndexChanged += new System.EventHandler(this.tabControlAnalysis_SelectedIndexChanged);
			// 
			// tabPageLogging
			// 
			this.tabPageLogging.Controls.Add(this.activityLogging);
			this.tabPageLogging.Location = new System.Drawing.Point(4, 22);
			this.tabPageLogging.Name = "tabPageLogging";
			this.tabPageLogging.Size = new System.Drawing.Size(768, 616);
			this.tabPageLogging.TabIndex = 0;
			this.tabPageLogging.Text = "Activity Logging";
			// 
			// activityLogging
			// 
			this.activityLogging.Dock = System.Windows.Forms.DockStyle.Fill;
			this.activityLogging.HideSelection = false;
			this.activityLogging.Location = new System.Drawing.Point(0, 0);
			this.activityLogging.Name = "activityLogging";
			this.activityLogging.Size = new System.Drawing.Size(768, 616);
			this.activityLogging.TabIndex = 0;
			this.activityLogging.Text = "";
			// 
			// tabPageValidation
			// 
			this.tabPageValidation.Controls.Add(this.dvtkWebBrowser);
			this.tabPageValidation.Location = new System.Drawing.Point(4, 22);
			this.tabPageValidation.Name = "tabPageValidation";
			this.tabPageValidation.Size = new System.Drawing.Size(768, 616);
			this.tabPageValidation.TabIndex = 1;
			this.tabPageValidation.Text = "Validation Results";
			// 
			// toolBar1
			// 
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.errorButton,
																						this.warningButton,
																						this.backwardButton,
																						this.upwardButton,
																						this.forwardButton});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.ImageList = this.imageListAnalysis;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(776, 28);
			this.toolBar1.TabIndex = 1;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// errorButton
			// 
			this.errorButton.ImageIndex = 3;
			// 
			// warningButton
			// 
			this.warningButton.ImageIndex = 4;
			// 
			// backwardButton
			// 
			this.backwardButton.ImageIndex = 0;
			// 
			// upwardButton
			// 
			this.upwardButton.ImageIndex = 2;
			// 
			// forwardButton
			// 
			this.forwardButton.ImageIndex = 1;
			// 
			// imageListAnalysis
			// 
			this.imageListAnalysis.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListAnalysis.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListAnalysis.ImageStream")));
			this.imageListAnalysis.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// dvtkWebBrowser
			// 
			this.dvtkWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dvtkWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.dvtkWebBrowser.Name = "dvtkWebBrowser";
			this.dvtkWebBrowser.Size = new System.Drawing.Size(768, 616);
			this.dvtkWebBrowser.TabIndex = 0;
			this.dvtkWebBrowser.XmlStyleSheetFullFileName = "";
			// 
			// Analysis
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(776, 670);
			this.Controls.Add(this.tabControlAnalysis);
			this.Controls.Add(this.toolBar1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "Analysis";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DICOM Analysis ";
			this.tabControlAnalysis.ResumeLayout(false);
			this.tabPageLogging.ResumeLayout(false);
			this.tabPageValidation.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void ClearLogging() 
		{
			activityLogging.Clear();
		}

		public void SaveLogging(string logFile) 
		{
			activityLogging.SaveFile(logFile,RichTextBoxStreamType.PlainText);
		}

		public void LoadLogging(string logFile) 
		{
			if(logFile != "")
				activityLogging.LoadFile(logFile,RichTextBoxStreamType.PlainText);
		}

		private void dvtkWebBrowser_BackwardFormwardEnabledStateChangeEvent()
		{
			this.upwardButton.Enabled = (this.xmlResultFile.Length > 0);
			this.backwardButton.Enabled = this.dvtkWebBrowser.IsBackwardEnabled;
			this.forwardButton.Enabled = this.dvtkWebBrowser.IsForwardEnabled;
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if( e.Button == errorButton)
			{					
				this.dvtkWebBrowser.FindNextText("Error:", true, true);
			}
			else if( e.Button == warningButton)
			{
				this.dvtkWebBrowser.FindNextText("Warning:", true, true);
			}
			else if( e.Button == backwardButton)
			{
				this.dvtkWebBrowser.Back();
			}
			else if( e.Button == upwardButton)
			{
				this.dvtkWebBrowser.Navigate(this.xmlResultFile);
			}
			else if( e.Button == forwardButton)
			{
				this.dvtkWebBrowser.Forward();
			}
			else
			{
			}		
		}
		
		/// <summary>
		/// This method is used to display Detail HTML Validation report.
		/// </summary>
		/// <param name="xmlFile"></param>
		public void ShowResults(string xmlFile, string logFile)
		{
			xmlResultFile = xmlFile;
			activityLogFile = logFile;
			tabControlAnalysis.SelectedTab = tabPageValidation;
			if(xmlFile != "")
				dvtkWebBrowser.Navigate(xmlFile);
			else
				dvtkWebBrowser.Navigate("about:blank");

			this.ShowDialog();
		}

		private void tabControlAnalysis_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControlAnalysis.SelectedTab == tabPageLogging) 
			{
				LoadLogging(activityLogFile);
			}
		}
	}
}
