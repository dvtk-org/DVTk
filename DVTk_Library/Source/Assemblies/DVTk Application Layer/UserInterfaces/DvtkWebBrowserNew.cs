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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Diagnostics;

namespace DvtkApplicationLayer.UserInterfaces
{
	/// <summary>
	/// Summary description for DvtkWebBrowser.
	/// </summary>
	public class DvtkWebBrowserNew : System.Windows.Forms.UserControl
    {
		private const int FIND_MATCH_WHOLE_WORD = 2;
		private const int FIND_MATCH_CASE = 4;
		private const string ERROR = "Error:";
		private const string WARNING = "Warning:";

		private bool containsErrors = false;
		private bool containsWarnings = false;

		/// <summary>
		/// The Html document that is currently shown.
		/// </summary>
		private mshtml.HTMLDocument htmlDocument = null;
	
		/// <summary>
		/// The body of the Html document that is currently shown.
		/// </summary>
		private mshtml.HTMLBody htmlBody = null;

		/// <summary>
		/// The remaining text part of the displayed Html in which to find a next occurence of a string.
		/// </summary>
		private mshtml.IHTMLTxtRange findRemainingText = null;
        private WebBrowser webBrowserUserControl;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
        public DvtkWebBrowserNew()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			Navigate("about:blank");
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			// Following should solve the "Windowless ActiveX controls are not supported" bug.
			if( disposing )
			{
                webBrowserUserControl.Dispose();
                if (null != webBrowserUserControl && null != components)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.webBrowserUserControl = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserUserControl
            // 
            this.webBrowserUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserUserControl.Location = new System.Drawing.Point(0, 0);
            this.webBrowserUserControl.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserUserControl.Name = "webBrowserUserControl";
            this.webBrowserUserControl.Size = new System.Drawing.Size(392, 328);
            this.webBrowserUserControl.TabIndex = 0;
            this.webBrowserUserControl.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowserUserControl_Navigated);
            this.webBrowserUserControl.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowserUserControl_Navigating);
            this.webBrowserUserControl.CanGoBackChanged += new EventHandler(webBrowserUserControl_CanGoBackChanged);
            this.webBrowserUserControl.CanGoForwardChanged += new EventHandler(webBrowserUserControl_CanGoForwardChanged);
            // 
            // DvtkWebBrowser2
            // 
            this.Controls.Add(this.webBrowserUserControl);
            this.Name = "DvtkWebBrowserNew";
            this.Size = new System.Drawing.Size(392, 328);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Public method
		/// </summary>
		/// <param name="fullFileName"></param>
		public void Navigate(string fullFileName)
		{
            this.webBrowserUserControl.Navigate(fullFileName);
		}

        private void webBrowserUserControl_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			string theFullFileName = GetFullFileNameFromHtmlLink(e.Url.ToString());
			int indexResultFile = -1 ;
			indexResultFile = theFullFileName.IndexOf("*" ,0);
			if (indexResultFile != -1) 
			{
				e.Cancel = true ;
			}
			else 
			{
				if (theFullFileName.ToLower().IndexOf(".xml") != -1)
					// The user has selected a results file tag or has pressed a link in a HTML file.
					// Convert the XML file to HTML, cancel this request and request viewing of the generated HTML file.
				{
					// Cancel it. We want to show the generated HTML file.
					// As a result of calling _TCM_ValidationResultsManager.ShowHtml(e.uRL.ToString()), this method will
					// be called again.				
					e.Cancel = true;

					string htmlFullFileName = theFullFileName.ToLower().Replace(".xml", ".html");
				
					// Do the actual conversion from XML to HTML.
					ConvertXmlToHtml(theFullFileName, htmlFullFileName);
		
					Navigate(htmlFullFileName);
				}
				else
				{
					// Do nothing, This is a HTML file and will be automatically shown.				
				}
			}
		}

        void webBrowserUserControl_CanGoForwardChanged(object sender, EventArgs e)
        {
            bool backwardFormwardEnabledStateChange = false;
            if (this.isForwardEnabled != webBrowserUserControl.CanGoForward)
            {
                backwardFormwardEnabledStateChange = true;
            }

            this.isForwardEnabled = webBrowserUserControl.CanGoForward;

            if (backwardFormwardEnabledStateChange)
            {
                if (BackwardFormwardEnabledStateChangeEvent != null)
                {
                    BackwardFormwardEnabledStateChangeEvent();
                }
            }
        }

        void webBrowserUserControl_CanGoBackChanged(object sender, EventArgs e)
        {
            bool backwardFormwardEnabledStateChange = false;
            if (this.isBackwardEnabled != webBrowserUserControl.CanGoBack)
            {
                backwardFormwardEnabledStateChange = true;
            }

            this.isBackwardEnabled = webBrowserUserControl.CanGoBack;

            if (backwardFormwardEnabledStateChange)
            {
                if (BackwardFormwardEnabledStateChangeEvent != null)
                {
                    BackwardFormwardEnabledStateChangeEvent();
                }
            }
        }

		private bool isForwardEnabled = false;

		private bool isBackwardEnabled = false;

		/// <summary>
		/// 
		/// </summary>
		public bool IsForwardEnabled
		{
			get
			{
				return(this.isForwardEnabled);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsBackwardEnabled
		{
			get
			{
				return(this.isBackwardEnabled);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public String XmlStyleSheetFullFileName
		{
			get
			{
				return(this.xmlStyleSheetFullFileName);
			}
			set
			{
				this.xmlStyleSheetFullFileName = value;
			}
		}

		private String xmlStyleSheetFullFileName = "";


		/// <summary>
		/// 
		/// </summary>
		public delegate void BackwardFormwardEnabledStateChangeEventHandler();

		/// <summary>
		/// 
		/// </summary>
		public event BackwardFormwardEnabledStateChangeEventHandler BackwardFormwardEnabledStateChangeEvent;

		/// <summary>
		/// Show the previous URL link.
		/// </summary>
		public void Back()
		{
			if (this.isBackwardEnabled)
			{
                this.webBrowserUserControl.GoBack();
               
			}
		}

		/// <summary>
		/// Show the next URL link.
		/// </summary>
		public void Forward()
		{
			if (this.isForwardEnabled)
			{
                this.webBrowserUserControl.GoForward();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="xmlFullFileName"></param>
		/// <param name="htmlFullFileName"></param>
		public void ConvertXmlToHtml(String xmlFullFileName, String htmlFullFileName)
		{
            XslCompiledTransform xslt = new XslCompiledTransform();
            XsltSettings settings = new XsltSettings();
            settings.EnableDocumentFunction = true;

			xslt.Load(xmlStyleSheetFullFileName,settings,null);

            XmlReader reader = XmlReader.Create(xmlFullFileName);
            XmlTextWriter writer = new XmlTextWriter(htmlFullFileName, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.None;

            xslt.Transform(reader, null, writer);
            writer.Flush();
            writer.Close();
            reader.Close();
		}

		/// <summary>
		/// Remove, if needed, characters from a Html link to get a valid full file name.
		/// </summary>
		/// <param name="theHtmlLink">The HTML link</param>
		/// <returns>The full file name.</returns>
		public static string GetFullFileNameFromHtmlLink(string theHtmlLink)
		{
			string theReturnValue = theHtmlLink;

			// If the string starts with "file:///c:\\", remove it.
			if (theReturnValue.StartsWith("file:///c:\\"))
			{
				theReturnValue = theReturnValue.Remove(0, "file:///c:\\".Length);
			}

			// If the string starts with "file:///", remove it.
			if (theReturnValue.StartsWith("file:///"))
			{
				theReturnValue = theReturnValue.Remove(0, "file:///".Length);
			}

			// If the string starts with "http:/", remove it.
			if (theReturnValue.StartsWith("http://"))
			{
				theReturnValue = theReturnValue.Remove(0, "http://".Length);
			}

			// If the string contains a '#', remove it and all following characters.
			int theCrossIndex = theReturnValue.IndexOf('#');

			if (theCrossIndex != -1)
			{
				theReturnValue = theReturnValue.Substring(0, theCrossIndex);
			}

			theReturnValue = theReturnValue.Replace("%20", " ");

			return(theReturnValue);
		}

		/// <summary>
		/// Handler of the NavigateComplete2 event.
		/// </summary>
		public void NavigateComplete2Handler()
		{
			mshtml.IHTMLTxtRange theIHTMLTxtRange = null;

            htmlDocument = (mshtml.HTMLDocument)webBrowserUserControl.Document.DomDocument;
			htmlBody = (mshtml.HTMLBody)htmlDocument.body;

			// Determine if errors exist in the displayed Html.
			theIHTMLTxtRange = htmlBody.createTextRange();
			if(theIHTMLTxtRange.text == null) 
			{
			}
			else 
			{
				containsErrors = theIHTMLTxtRange.findText(ERROR, theIHTMLTxtRange.text.Length, FIND_MATCH_CASE | FIND_MATCH_WHOLE_WORD);
			}
 
			// Determine if warnings exist in the displayed Html.
			theIHTMLTxtRange = htmlBody.createTextRange();
			if(theIHTMLTxtRange.text == null) 
			{
			}
			else 
			{
				containsWarnings= theIHTMLTxtRange.findText(WARNING, theIHTMLTxtRange.text.Length, FIND_MATCH_CASE | FIND_MATCH_WHOLE_WORD);
			}

			findRemainingText = htmlBody.createTextRange();
			((mshtml.IHTMLSelectionObject)htmlDocument.selection).empty();
		}

		/// <summary>
		/// 
		/// </summary>
		public bool ContainsErrors
		{
			get
			{
				return containsErrors;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool ContainsWarnings
		{
			get
			{
				return containsWarnings;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        public void FindNextWarning()
        {
            FindNextText(WARNING, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void FindNextError()
        {
            FindNextText(ERROR, true, true);
        }

		/// <summary>
		/// Public method
		/// </summary>
		/// <param name="theText"></param>
		/// <param name="mustMatchWholeWord"></param>
		/// <param name="mustMatchCase"></param>
		public void FindNextText(string theText, bool mustMatchWholeWord, bool mustMatchCase)
		{
			// define the search options
			int theSearchOption = 0;

			if (mustMatchWholeWord)
			{
				theSearchOption += FIND_MATCH_WHOLE_WORD;
			}

			if (mustMatchCase)
			{
				theSearchOption += FIND_MATCH_CASE;
			}

			if ( (findRemainingText == null) || (findRemainingText.text == null) )
			{
				// Sanity check.
				Debug.Assert(false);
			}
			else
			{

				// perform the search operation
				if (findRemainingText.findText(theText, findRemainingText.text.Length, theSearchOption))
					// String has been found.
				{
					// Select the found text within the document
					findRemainingText.select();

					// Limit the new find range to be from the newly found text
					mshtml.IHTMLTxtRange theFoundRange = (mshtml.IHTMLTxtRange)htmlDocument.selection.createRange();
					findRemainingText = (mshtml.IHTMLTxtRange)htmlBody.createTextRange();
					findRemainingText.setEndPoint("StartToEnd", theFoundRange);
				}
				else
				{
					// Reset the find ranges
					findRemainingText = htmlBody.createTextRange();
					((mshtml.IHTMLSelectionObject)htmlDocument.selection).empty();

					MessageBox.Show("Finished searching the document", string.Format("Find text \"{0}\"", theText), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
		}

        /// <summary>
		/// 
		/// </summary>
		public void MakeBlank()
		{
			Navigate("about:blank");
		}

        /// <summary>
        /// 
        /// </summary>
        public delegate void ErrorWarningEnabledStateChangeEventHandler();

        /// <summary>
        /// 
        /// </summary>
        public event ErrorWarningEnabledStateChangeEventHandler ErrorWarningEnabledStateChangeEvent;


        private void webBrowserUserControl_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            NavigateComplete2Handler();

            if (ErrorWarningEnabledStateChangeEvent != null)
            {
                ErrorWarningEnabledStateChangeEvent();
            }
        }        
	}
}
