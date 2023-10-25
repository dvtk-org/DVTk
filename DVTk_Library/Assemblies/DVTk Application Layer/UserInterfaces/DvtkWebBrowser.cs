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
    public class DvtkWebBrowser : System.Windows.Forms.UserControl
    {
        private AxSHDocVw.AxWebBrowser axWebBrowser;

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
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// 
        /// </summary>
        public DvtkWebBrowser()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            Navigate("about:blank");
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // Following should solve the "Windowless ActiveX controls are not supported" bug.
            if (disposing)
            {
                axWebBrowser.Dispose();
                axWebBrowser.ContainingControl = null;
                if (null != axWebBrowser && null != components)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DvtkWebBrowser));
            this.axWebBrowser = new AxSHDocVw.AxWebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // axWebBrowser
            // 
            this.axWebBrowser.AllowDrop = true;
            this.axWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWebBrowser.Enabled = true;
            this.axWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.axWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser.OcxState")));
            this.axWebBrowser.Size = new System.Drawing.Size(392, 328);
            this.axWebBrowser.TabIndex = 0;
            this.axWebBrowser.CommandStateChange += new AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEventHandler(this.axWebBrowser_CommandStateChange);
            this.axWebBrowser.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser_NavigateComplete2);
            this.axWebBrowser.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.axWebBrowser_BeforeNavigate2);
            // 
            // DvtkWebBrowser
            // 
            this.Controls.Add(this.axWebBrowser);
            this.Name = "DvtkWebBrowser";
            this.Size = new System.Drawing.Size(392, 328);
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Public method
        /// </summary>
        /// <param name="fullFileName"></param>
        public void Navigate(string fullFileName)
        {
            object Zero = 0;
            object EmptyString = "";

            this.axWebBrowser.Navigate(fullFileName, ref Zero, ref EmptyString, ref EmptyString, ref EmptyString);
        }

        private void axWebBrowser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
        {
            string theFullFileName = GetFullFileNameFromHtmlLink(e.uRL.ToString());
            int indexResultFile = -1;
            indexResultFile = theFullFileName.IndexOf("*", 0);
            if (indexResultFile != -1)
            {
                e.cancel = true;
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
                    e.cancel = true;

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

        private void axWebBrowser_CommandStateChange(object sender, AxSHDocVw.DWebBrowserEvents2_CommandStateChangeEvent e)
        {
            bool backwardFormwardEnabledStateChange = false;

            switch (e.command)
            {
                case 1:
                    if (this.isForwardEnabled != e.enable)
                    {
                        backwardFormwardEnabledStateChange = true;
                    }

                    this.isForwardEnabled = e.enable;
                    break;

                case 2:
                    if (this.isBackwardEnabled != e.enable)
                    {
                        backwardFormwardEnabledStateChange = true;
                    }

                    this.isBackwardEnabled = e.enable;
                    break;

                default:
                    // Do nothing.
                    break;
            }

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
                return (this.isForwardEnabled);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBackwardEnabled
        {
            get
            {
                return (this.isBackwardEnabled);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String XmlStyleSheetFullFileName
        {
            get
            {
                return (this.xmlStyleSheetFullFileName);
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
                this.axWebBrowser.GoBack();

            }
        }

        /// <summary>
        /// Show the next URL link.
        /// </summary>
        public void Forward()
        {
            if (this.isForwardEnabled)
            {
                this.axWebBrowser.GoForward();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFullFileName"></param>
        /// <param name="htmlFullFileName"></param>
        public void ConvertXmlToHtml(String xmlFullFileName, String htmlFullFileName)
        {
            XslTransform xslt = new XslTransform();

            xslt.Load(xmlStyleSheetFullFileName);

            XPathDocument xpathdocument = new XPathDocument(xmlFullFileName);

            /* This code does not work with the style sheet needed for the Session.WriteHtmlInformation method.
			XmlTextWriter writer = new XmlTextWriter (theHtmlFullFileName, System.Text.Encoding.UTF8);
			writer.Formatting = Formatting.None;
			xslt.Transform (xpathdocument, null, writer, null);
			writer.Flush ();
			writer.Close ();
			*/

            FileStream fileStream = new FileStream(htmlFullFileName, FileMode.Create, FileAccess.ReadWrite);

            xslt.Transform(xpathdocument, null, fileStream, null);
            fileStream.Close();
        }

        /// <summary>
        /// Remove, if needed, characters from a Html link to get a valid full file name.
        /// </summary>
        /// <param name="theHtmlLink">The HTML link</param>
        /// <returns>The full file name.</returns>
        public string GetFullFileNameFromHtmlLink(string theHtmlLink)
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

            return (theReturnValue);
        }

        /// <summary>
        /// Handler of the NavigateComplete2 event.
        /// </summary>
        public void NavigateComplete2Handler()
        {
            mshtml.IHTMLTxtRange theIHTMLTxtRange = null;

            htmlDocument = (mshtml.HTMLDocument)axWebBrowser.Document;
            htmlBody = (mshtml.HTMLBody)htmlDocument.body;

            // Determine if errors exist in the displayed Html.
            theIHTMLTxtRange = htmlBody.createTextRange();
            if (theIHTMLTxtRange.text == null)
            {
            }
            else
            {
                containsErrors = theIHTMLTxtRange.findText(ERROR, theIHTMLTxtRange.text.Length, FIND_MATCH_CASE | FIND_MATCH_WHOLE_WORD);
            }

            // Determine if warnings exist in the displayed Html.
            theIHTMLTxtRange = htmlBody.createTextRange();
            if (theIHTMLTxtRange.text == null)
            {
            }
            else
            {
                containsWarnings = theIHTMLTxtRange.findText(WARNING, theIHTMLTxtRange.text.Length, FIND_MATCH_CASE | FIND_MATCH_WHOLE_WORD);
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

            if ((findRemainingText == null) || (findRemainingText.text == null))
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

        private void axWebBrowser_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e)
        {
            NavigateComplete2Handler();
        }

        /// <summary>
        /// 
        /// </summary>
        public void MakeBlank()
        {
            Navigate("about:blank");
        }
    }
}
