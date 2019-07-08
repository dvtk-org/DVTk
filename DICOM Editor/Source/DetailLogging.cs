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

namespace DCMEditor
{
	/// <summary>
	/// Summary description for DetailLogging.
	/// </summary>
	public class DetailLogging : System.Windows.Forms.Form
	{
		private DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew dvtkWebBrowser;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DetailLogging()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//Load Style sheet
			this.dvtkWebBrowser.XmlStyleSheetFullFileName = Application.StartupPath + "\\DVT_RESULTS.xslt";
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DetailLogging));
			this.dvtkWebBrowser = new DvtkApplicationLayer.UserInterfaces.DvtkWebBrowserNew();
			this.SuspendLayout();
			// 
			// dvtkWebBrowser
			// 
			this.dvtkWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dvtkWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.dvtkWebBrowser.Name = "dvtkWebBrowser";
			this.dvtkWebBrowser.Size = new System.Drawing.Size(744, 694);
			this.dvtkWebBrowser.TabIndex = 0;
			this.dvtkWebBrowser.XmlStyleSheetFullFileName = "";
			// 
			// DetailLogging
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(744, 694);
			this.Controls.Add(this.dvtkWebBrowser);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DetailLogging";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Detail Logging";
			this.ResumeLayout(false);

		}
		#endregion

		public void ShowResults(string detailXmlFullFileName, string summaryXmlFullFileName)
		{
			string detailhtmlFullFileName = detailXmlFullFileName.ToLower().Replace(".xml", ".html");
			string summaryhtmlFullFileName = summaryXmlFullFileName.ToLower().Replace(".xml", ".html");
				
			// Do the actual conversion from XML to HTML.
			dvtkWebBrowser.ConvertXmlToHtml(detailXmlFullFileName, detailhtmlFullFileName);
		
			//Also convert summary result xml to html for further use
			dvtkWebBrowser.ConvertXmlToHtml(summaryXmlFullFileName, summaryhtmlFullFileName);
			
			dvtkWebBrowser.Navigate(detailhtmlFullFileName);
		}
	}
}
