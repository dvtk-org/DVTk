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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;
using System.Data;
using System.IO;
using DvtkHighLevelInterface.Common.Threads;
using DvtkApplicationLayer.UserInterfaces;

namespace DCMCompare
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class DCMCompareForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem MenuItem_Help;
		private System.Windows.Forms.MenuItem MenuItem_FileExit;
		private System.Windows.Forms.MenuItem MenuItem_File;
		private System.Windows.Forms.MenuItem MenuItem_AboutDCMCompare;
		private System.Windows.Forms.TabControl TabControl;
		private System.Windows.Forms.TabPage TabCompareDetailResults;
		private System.Windows.Forms.TabPage TabComparisonFilter;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.StatusBar statusBarCompare;
		private DCMCompare.StatusBarProgressPanel statusBarPanel;
		private System.Windows.Forms.MenuItem MenuItemCompare;
		private System.Windows.Forms.MenuItem menuItemCompareAgain;
		private System.Windows.Forms.MenuItem MenuItemCompareFilter;
		private System.Windows.Forms.MenuItem MenuItemCompareNoFilter;
		private System.Windows.Forms.MenuItem menuItemCompareAgainFilter;
		private System.Windows.Forms.MenuItem menuItemCompareAgainNoFilter;
		private System.Windows.Forms.ListBox listBoxFilterAttr;
		private System.Windows.Forms.Button buttonAddAttr;
        private System.Windows.Forms.Button buttonRemoveAttr;
        private IContainer components;
		public static string firstDCMFile;
		public static string secondDCMFile;
		public MainThread theMainSessionThread = null;
		private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkWebBrowser;
		public ThreadManager dvtThreadMgr = null;
		string detailXmlFullFileName = "";
		string summaryXmlFullFileName = "";
        string filteredAttrFilePath;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button buttonAddSeqItemAttr;
		private System.Windows.Forms.CheckBox checkBoxFilterGroupLength;		
		public static ArrayList attributesTagList = new ArrayList();
		public static bool filterGroupLengthAttributes = false;

        private string configFileDirectory;

		public DCMCompareForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            configFileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\DVTk\DICOM Compare";
            if (!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }
            filteredAttrFilePath = configFileDirectory + @"\FilteredAttributes.txt";

			// Set the DrawItem event handler
			statusBarCompare.DrawItem +=new StatusBarDrawItemEventHandler(statusBarPanel.ParentDrawItemHandler);

			this.dvtkWebBrowser.XmlStyleSheetFullFileName = Application.StartupPath + "\\DVT_RESULTS.xslt";

			menuItemCompareAgain.Visible = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DCMCompareForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.MenuItem_File = new System.Windows.Forms.MenuItem();
            this.MenuItemCompare = new System.Windows.Forms.MenuItem();
            this.MenuItemCompareFilter = new System.Windows.Forms.MenuItem();
            this.MenuItemCompareNoFilter = new System.Windows.Forms.MenuItem();
            this.menuItemCompareAgain = new System.Windows.Forms.MenuItem();
            this.menuItemCompareAgainFilter = new System.Windows.Forms.MenuItem();
            this.menuItemCompareAgainNoFilter = new System.Windows.Forms.MenuItem();
            this.MenuItem_FileExit = new System.Windows.Forms.MenuItem();
            this.MenuItem_Help = new System.Windows.Forms.MenuItem();
            this.MenuItem_AboutDCMCompare = new System.Windows.Forms.MenuItem();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabCompareDetailResults = new System.Windows.Forms.TabPage();
            this.dvtkWebBrowser = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
            this.TabComparisonFilter = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonAddSeqItemAttr = new System.Windows.Forms.Button();
            this.buttonAddAttr = new System.Windows.Forms.Button();
            this.buttonRemoveAttr = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxFilterGroupLength = new System.Windows.Forms.CheckBox();
            this.listBoxFilterAttr = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusBarCompare = new System.Windows.Forms.StatusBar();
            this.statusBarPanel = new DCMCompare.StatusBarProgressPanel();
            this.TabControl.SuspendLayout();
            this.TabCompareDetailResults.SuspendLayout();
            this.TabComparisonFilter.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItem_File,
            this.MenuItem_Help});
            // 
            // MenuItem_File
            // 
            this.MenuItem_File.Index = 0;
            this.MenuItem_File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItemCompare,
            this.menuItemCompareAgain,
            this.MenuItem_FileExit});
            this.MenuItem_File.Text = "File";
            // 
            // MenuItemCompare
            // 
            this.MenuItemCompare.Index = 0;
            this.MenuItemCompare.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItemCompareFilter,
            this.MenuItemCompareNoFilter});
            this.MenuItemCompare.Text = "&Compare";
            // 
            // MenuItemCompareFilter
            // 
            this.MenuItemCompareFilter.Index = 0;
            this.MenuItemCompareFilter.Text = "With Filter";
            this.MenuItemCompareFilter.Click += new System.EventHandler(this.MenuItemCompareFilter_Click);
            // 
            // MenuItemCompareNoFilter
            // 
            this.MenuItemCompareNoFilter.Index = 1;
            this.MenuItemCompareNoFilter.Text = "Without Filter";
            this.MenuItemCompareNoFilter.Click += new System.EventHandler(this.MenuItemCompareNoFilter_Click);
            // 
            // menuItemCompareAgain
            // 
            this.menuItemCompareAgain.Index = 1;
            this.menuItemCompareAgain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCompareAgainFilter,
            this.menuItemCompareAgainNoFilter});
            this.menuItemCompareAgain.Text = "&Compare Again";
            // 
            // menuItemCompareAgainFilter
            // 
            this.menuItemCompareAgainFilter.Index = 0;
            this.menuItemCompareAgainFilter.Text = "With Filter";
            this.menuItemCompareAgainFilter.Click += new System.EventHandler(this.menuItemCompareAgainFilter_Click);
            // 
            // menuItemCompareAgainNoFilter
            // 
            this.menuItemCompareAgainNoFilter.Index = 1;
            this.menuItemCompareAgainNoFilter.Text = "Without Filter";
            this.menuItemCompareAgainNoFilter.Click += new System.EventHandler(this.menuItemCompareAgainNoFilter_Click);
            // 
            // MenuItem_FileExit
            // 
            this.MenuItem_FileExit.Index = 2;
            this.MenuItem_FileExit.Text = "&Exit";
            this.MenuItem_FileExit.Click += new System.EventHandler(this.MenuItem_FileExit_Click);
            // 
            // MenuItem_Help
            // 
            this.MenuItem_Help.Index = 1;
            this.MenuItem_Help.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItem_AboutDCMCompare});
            this.MenuItem_Help.Text = "Help";
            // 
            // MenuItem_AboutDCMCompare
            // 
            this.MenuItem_AboutDCMCompare.Index = 0;
            this.MenuItem_AboutDCMCompare.Text = "About DCM Compare";
            this.MenuItem_AboutDCMCompare.Click += new System.EventHandler(this.MenuItem_AboutDCMCompare_Click);
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.TabCompareDetailResults);
            this.TabControl.Controls.Add(this.TabComparisonFilter);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(894, 620);
            this.TabControl.TabIndex = 0;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // TabCompareDetailResults
            // 
            this.TabCompareDetailResults.AutoScroll = true;
            this.TabCompareDetailResults.Controls.Add(this.dvtkWebBrowser);
            this.TabCompareDetailResults.Location = new System.Drawing.Point(4, 25);
            this.TabCompareDetailResults.Name = "TabCompareDetailResults";
            this.TabCompareDetailResults.Size = new System.Drawing.Size(886, 591);
            this.TabCompareDetailResults.TabIndex = 0;
            this.TabCompareDetailResults.Text = "Compare Results Overview";
            // 
            // dvtkWebBrowser
            // 
            this.dvtkWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvtkWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.dvtkWebBrowser.Name = "dvtkWebBrowser";
            this.dvtkWebBrowser.Size = new System.Drawing.Size(886, 591);
            this.dvtkWebBrowser.TabIndex = 0;
            this.dvtkWebBrowser.XmlStyleSheetFullFileName = "";
            // 
            // TabComparisonFilter
            // 
            this.TabComparisonFilter.Controls.Add(this.panel1);
            this.TabComparisonFilter.Controls.Add(this.panel2);
            this.TabComparisonFilter.Location = new System.Drawing.Point(4, 25);
            this.TabComparisonFilter.Name = "TabComparisonFilter";
            this.TabComparisonFilter.Size = new System.Drawing.Size(904, 599);
            this.TabComparisonFilter.TabIndex = 1;
            this.TabComparisonFilter.Text = "Comparison Filter";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonAddSeqItemAttr);
            this.panel1.Controls.Add(this.buttonAddAttr);
            this.panel1.Controls.Add(this.buttonRemoveAttr);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 498);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(904, 101);
            this.panel1.TabIndex = 6;
            // 
            // buttonAddSeqItemAttr
            // 
            this.buttonAddSeqItemAttr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddSeqItemAttr.Location = new System.Drawing.Point(307, 28);
            this.buttonAddSeqItemAttr.Name = "buttonAddSeqItemAttr";
            this.buttonAddSeqItemAttr.Size = new System.Drawing.Size(213, 26);
            this.buttonAddSeqItemAttr.TabIndex = 6;
            this.buttonAddSeqItemAttr.Text = "Add Sequence Item Attribute";
            this.buttonAddSeqItemAttr.Click += new System.EventHandler(this.buttonAddSeqItemAttr_Click);
            // 
            // buttonAddAttr
            // 
            this.buttonAddAttr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddAttr.Location = new System.Drawing.Point(125, 28);
            this.buttonAddAttr.Name = "buttonAddAttr";
            this.buttonAddAttr.Size = new System.Drawing.Size(125, 26);
            this.buttonAddAttr.TabIndex = 4;
            this.buttonAddAttr.Text = "Add Attribute";
            this.buttonAddAttr.Click += new System.EventHandler(this.buttonAddAttr_Click);
            // 
            // buttonRemoveAttr
            // 
            this.buttonRemoveAttr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveAttr.Location = new System.Drawing.Point(559, 28);
            this.buttonRemoveAttr.Name = "buttonRemoveAttr";
            this.buttonRemoveAttr.Size = new System.Drawing.Size(182, 26);
            this.buttonRemoveAttr.TabIndex = 5;
            this.buttonRemoveAttr.Text = "Remove Selected Attribute";
            this.buttonRemoveAttr.Click += new System.EventHandler(this.buttonRemoveAttr_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBoxFilterGroupLength);
            this.panel2.Controls.Add(this.listBoxFilterAttr);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(904, 498);
            this.panel2.TabIndex = 5;
            // 
            // checkBoxFilterGroupLength
            // 
            this.checkBoxFilterGroupLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxFilterGroupLength.Location = new System.Drawing.Point(684, 0);
            this.checkBoxFilterGroupLength.Name = "checkBoxFilterGroupLength";
            this.checkBoxFilterGroupLength.Size = new System.Drawing.Size(211, 28);
            this.checkBoxFilterGroupLength.TabIndex = 4;
            this.checkBoxFilterGroupLength.Text = "Filter Group Length Attributes";
            this.checkBoxFilterGroupLength.CheckedChanged += new System.EventHandler(this.checkBoxFilterGroupLength_CheckedChanged);
            // 
            // listBoxFilterAttr
            // 
            this.listBoxFilterAttr.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxFilterAttr.ItemHeight = 16;
            this.listBoxFilterAttr.Location = new System.Drawing.Point(0, 46);
            this.listBoxFilterAttr.Name = "listBoxFilterAttr";
            this.listBoxFilterAttr.Size = new System.Drawing.Size(904, 452);
            this.listBoxFilterAttr.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Filter attributes List:";
            // 
            // statusBarCompare
            // 
            this.statusBarCompare.Location = new System.Drawing.Point(0, 620);
            this.statusBarCompare.Name = "statusBarCompare";
            this.statusBarCompare.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel});
            this.statusBarCompare.ShowPanels = true;
            this.statusBarCompare.Size = new System.Drawing.Size(894, 25);
            this.statusBarCompare.SizingGrip = false;
            this.statusBarCompare.TabIndex = 1;
            // 
            // statusBarPanel
            // 
            this.statusBarPanel.Name = "statusBarPanel";
            this.statusBarPanel.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
            this.statusBarPanel.Width = 500;
            // 
            // DCMCompareForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(894, 645);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.statusBarCompare);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(912, 692);
            this.Name = "DCMCompareForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DCM Compare";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DCMCompareForm_Closing);
            this.Load += new System.EventHandler(this.DCMCompareForm_Load);
            this.TabControl.ResumeLayout(false);
            this.TabCompareDetailResults.ResumeLayout(false);
            this.TabComparisonFilter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void Compare(bool compareAgain , bool withFilter)
		{
			statusBarPanel.ProgressBar.Value = statusBarPanel.ProgressBar.Minimum;

			if(!compareAgain)
			{
				OpenFileDialog theOpenFileDialog = new OpenFileDialog();

				theOpenFileDialog.Filter = "DCM files (*.dcm) |*.dcm|All files (*.*)|*.*";
				theOpenFileDialog.Title = "Select first DCM file";
				theOpenFileDialog.Multiselect = false;
				theOpenFileDialog.ReadOnlyChecked = true;
			
				// Show the file dialog.
				// If the user pressed the OK button...
				if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
				{
					// Add all DCM files selected.
					firstDCMFile = theOpenFileDialog.FileName;
					theOpenFileDialog.Filter = "DCM files (*.dcm) |*.dcm|All files (*.*)|*.*";
					theOpenFileDialog.Title = "Select second DCM file";
					theOpenFileDialog.Multiselect = false;
					theOpenFileDialog.ReadOnlyChecked = true;

					if (theOpenFileDialog.ShowDialog() == DialogResult.OK)
					{
						secondDCMFile = theOpenFileDialog.FileName;
					}
					else
					{
						return;
					}
				}
				else
				{
					return;
				}
			}
	
			//Update the title of the form
			string theNewText = "DCMCompare Tool - ";
			theNewText+= string.Format("Comparing {0} and {1}", firstDCMFile, secondDCMFile);
			Text = theNewText;
			
			//Initialize and Execute the script session
			if(theMainSessionThread == null)
			{
				dvtThreadMgr = new ThreadManager();

				theMainSessionThread = new MainThread();
				theMainSessionThread.Initialize(dvtThreadMgr);
				theMainSessionThread.Options.Identifier = "DCM_Compare";
				theMainSessionThread.Options.LogThreadStartingAndStoppingInParent = false;
				theMainSessionThread.Options.LogChildThreadsOverview = false;

				// Load the Dvtk Script session 
				theMainSessionThread.Options.LoadFromFile(Application.StartupPath + @"\Script.ses");
                theMainSessionThread.Options.ResultsDirectory = configFileDirectory + @"\results";
                theMainSessionThread.Options.DataDirectory = configFileDirectory + @"\results";
				DirectoryInfo resultDirectory = new DirectoryInfo(theMainSessionThread.Options.ResultsDirectory);
				if(!resultDirectory.Exists)
				{
					resultDirectory.Create();
				}

				theMainSessionThread.Options.StrictValidation = true;

				theMainSessionThread.Options.StartAndStopResultsGatheringEnabled = true;
				theMainSessionThread.Options.ResultsFileNameOnlyWithoutExtension = 
					string.Format("{0:000}", theMainSessionThread.Options.SessionId) +
					"_" + theMainSessionThread.Options.Identifier + "_res";

				detailXmlFullFileName = theMainSessionThread.Options.DetailResultsFullFileName;
				summaryXmlFullFileName = theMainSessionThread.Options.SummaryResultsFullFileName;				

				menuItemCompareAgain.Visible = true;
			}

			if(withFilter)
			{
				//Populate the Filtered Attributes List
				if(listBoxFilterAttr.Items.Count != 0)
				{
					foreach( object listItem in listBoxFilterAttr.Items)
					{
						string attributeStr = listItem.ToString().Trim();
						attributesTagList.Add(attributeStr);						
					}
				}
			}

			//Start the execution
			theMainSessionThread.Start();
			for(int i=0; i< 10; i++)
			{
				statusBarPanel.ProgressBar.PerformStep();
				System.Threading.Thread.Sleep(250);
			}

			dvtThreadMgr.WaitForCompletionThreads();

			//Stop the thread
			if(theMainSessionThread  != null)
			{
				theMainSessionThread.Stop();
				theMainSessionThread = null;
				dvtThreadMgr = null;
			}

			//Display the results
			this.ShowResults();
		}

		private void MenuItem_FileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ShowResults()
		{
			string detailhtmlFullFileName = detailXmlFullFileName.ToLower().Replace(".xml", ".html");
			string summaryhtmlFullFileName = summaryXmlFullFileName.ToLower().Replace(".xml", ".html");
				
			// Do the actual conversion from XML to HTML.
			dvtkWebBrowser.ConvertXmlToHtml(detailXmlFullFileName, detailhtmlFullFileName);
		
			//Also convert summary result xml to html for further use
			dvtkWebBrowser.ConvertXmlToHtml(summaryXmlFullFileName, summaryhtmlFullFileName);
			
			dvtkWebBrowser.Navigate(detailhtmlFullFileName);
		}

		private void MenuItem_AboutDCMCompare_Click(object sender, System.EventArgs e)
		{
			AboutForm   about = new AboutForm("DCM Compare Tool");
			about.ShowDialog ();
		}

		private void TabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (TabControl.SelectedTab == TabComparisonFilter) 
			{
				TabControl.SelectedTab = TabComparisonFilter;
			}

			if (TabControl.SelectedTab == TabCompareDetailResults) 
			{
				TabControl.SelectedTab = TabCompareDetailResults;
			}
		}

		private void DCMCompareForm_Load(object sender, System.EventArgs e)
		{
			StreamReader reader = new StreamReader(filteredAttrFilePath);
			while (reader.Peek() != -1) 
			{
				string line = reader.ReadLine().Trim();
				if(line != "")
					listBoxFilterAttr.Items.Add(line);
			}
			reader.Close();
		}

		private void DCMCompareForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			StreamWriter writer = new StreamWriter(filteredAttrFilePath);
			foreach( object listItem in listBoxFilterAttr.Items)
			{
				writer.WriteLine(listItem.ToString());
			}
			writer.Close();

			//Clear all temporary files
			ArrayList theFilesToRemove = new ArrayList();
			DirectoryInfo theDirectoryInfo = new DirectoryInfo (configFileDirectory + @"\results\");
			FileInfo[] thePixFilesInfo;

			if (theDirectoryInfo.Exists)
			{
				thePixFilesInfo = theDirectoryInfo.GetFiles("*.pix");

				foreach (FileInfo theFileInfo in thePixFilesInfo)
				{
					string thePixFileName = theFileInfo.Name;

					theFilesToRemove.Add(thePixFileName);
				}				
			}

			//Delete all pix & idx files
			foreach(string theFileName in theFilesToRemove)
			{
				string theFullFileName = System.IO.Path.Combine(theDirectoryInfo.FullName, theFileName);

				if (File.Exists(theFullFileName))
				{
					try
					{
						File.Delete(theFullFileName);
					}
					catch(Exception exception)
					{
						string theWarningText = string.Format("Could not be delete the {0} temporary file.\n due to exception: {1}\n\n", theFullFileName, exception.Message);

						MessageBox.Show(theWarningText, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}				
			}
		}

		private void MenuItemCompareFilter_Click(object sender, System.EventArgs e)
		{
			TabControl.SelectedTab = TabCompareDetailResults;
			Compare(false , true);
		}

		private void MenuItemCompareNoFilter_Click(object sender, System.EventArgs e)
		{
			TabControl.SelectedTab = TabCompareDetailResults;
			Compare(false , false);
		}

		private void menuItemCompareAgainFilter_Click(object sender, System.EventArgs e)
		{
			TabControl.SelectedTab = TabCompareDetailResults;
			Compare(true , true);
		}

		private void menuItemCompareAgainNoFilter_Click(object sender, System.EventArgs e)
		{
			TabControl.SelectedTab = TabCompareDetailResults;
			Compare(true , false);
		}

		private void buttonAddAttr_Click(object sender, System.EventArgs e)
		{
			AddAttribute addAttr = new AddAttribute();
			addAttr.ShowDialog();
			listBoxFilterAttr.Items.Add(TagString(addAttr.AttributeGroup,addAttr.AttributeElement));
		}

		private void buttonRemoveAttr_Click(object sender, System.EventArgs e)
		{
			listBoxFilterAttr.Items.Remove(listBoxFilterAttr.SelectedItem);
		}

		private void buttonAddSeqItemAttr_Click(object sender, System.EventArgs e)
		{
			AddSeqItemAttr addSeqItemAttr = new AddSeqItemAttr();
			addSeqItemAttr.ShowDialog();
			string attrStr = TagString(addSeqItemAttr.SeqAttributeGroup, addSeqItemAttr.SeqAttributeElement) + "[" + 
						addSeqItemAttr.SeqItemNr + "]" + "/" + TagString(addSeqItemAttr.AttributeGroup, addSeqItemAttr.AttributeElement);
			listBoxFilterAttr.Items.Add(attrStr);		
		}
	
		public static string TagString(UInt16 group, UInt16 element)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			System.Byte[] groupByteArray = System.BitConverter.GetBytes(group);
			System.Byte[] elementByteArray = System.BitConverter.GetBytes(element);
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
			sb.AppendFormat(
				"0x{0}{1}", 
				hexByteStr0,
				hexByteStr1);

			hexByteStr0 = elementByteArray[0].ToString("x");
			if (hexByteStr0.Length == 1) hexByteStr0 = "0" + hexByteStr0; // prepend with leading zero
			hexByteStr1 = elementByteArray[1].ToString("x");
			if (hexByteStr1.Length == 1) hexByteStr1 = "0" + hexByteStr1; // prepend with leading zero
			sb.AppendFormat(
				"{0}{1}", 
				hexByteStr0,
				hexByteStr1);
			return sb.ToString();
		}

		private void checkBoxFilterGroupLength_CheckedChanged(object sender, System.EventArgs e)
		{
			if(checkBoxFilterGroupLength.Checked)
				filterGroupLengthAttributes = true;
			else
				filterGroupLengthAttributes = false;
		}		
	}

	/// <summary>
	/// For displaying Status Progress bar in tool
	/// </summary>
	public class StatusBarProgressPanel : StatusBarPanel
	{
		private bool isAdded = false;

		private ProgressBar progressBar = new ProgressBar();
		[Category("Progress")]
		public ProgressBar ProgressBar
		{
			get { return progressBar; }
		}

		public StatusBarProgressPanel() : base()
		{
			// Just to be safe
			this.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
		}

		public void ParentDrawItemHandler(object sender, StatusBarDrawItemEventArgs sbdevent)
		{
			// Only add this once to the parent's control container
			if (isAdded == false)
			{
				this.Parent.Controls.Add(this.progressBar);
				this.isAdded = true;
			}

			// Get the bounds of this panel and copy to the progress bar's bounds
			if (sbdevent.Panel == this)
				progressBar.Bounds = sbdevent.Bounds;
		}
	}
}
