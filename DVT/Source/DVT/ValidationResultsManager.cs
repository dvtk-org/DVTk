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
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace Dvt
{
	/// <summary>
	/// Class implementing the functionality of the validation results tab.
	/// </summary>
	public class ValidationResultsManager
	{
		private const int _FIND_MATCH_WHOLE_WORD = 2;
		private const int _FIND_MATCH_CASE = 4;
		private const string _ERROR = "Error:";
		private const string _WARNING = "Warning:";

        private WebBrowser _webBrowser;
		private bool _ContainsErrors = false;
		private bool _ContainsWarnings = false;

		/// <summary>
		/// The Html document that is currently shown.
		/// </summary>
		private mshtml.HTMLDocument _HTMLDocument = null;
	
		/// <summary>
		/// The body of the Html document that is currently shown.
		/// </summary>
		private mshtml.HTMLBody _HTMLBody = null;

		/// <summary>
		/// The remaining text part of the displayed Html in which to find a next occurence of a string.
		/// </summary>
		private mshtml.IHTMLTxtRange _FindRemainingText = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="theAxWebBrowser">The web browser activeX control.</param>
		public ValidationResultsManager(WebBrowser theWebBrowser)
		{
            _webBrowser = theWebBrowser;
		}

		/// <summary>
		/// Convert a XML file into a HTML file. This method should also be able to display the HTML results properly when
		/// the Session.WriteHtmlInformation has been called.
		/// </summary>
		/// <param name="theDirectory">The directory in which the XML file resides and in which the HTML file will be written.</param>
		/// <param name="theXmlFileNameOnly">The XML file name only.</param>
		/// <param name="theHtmlFileNameOnly">The HTNL file name only.</param>
		public void ConvertXmlToHtml(string theDirectory, string theXmlFileNameOnly, string theHtmlFileNameOnly , bool filterIndicator , string filterXMLfilename)
		{
			string theXmlFullFileName;
			string theHtmlFullFileName;
			string theResultsStyleSheetFullFileName;

			theXmlFullFileName = System.IO.Path.Combine(theDirectory, theXmlFileNameOnly);
			theHtmlFullFileName = theXmlFullFileName.Replace(".xml", ".html");
			theResultsStyleSheetFullFileName = System.IO.Path.Combine(Application.StartupPath, "DVT_RESULTS.xslt");
			XsltArgumentList xslArgList = new XsltArgumentList();
			string filtertemp = "";
			if(filterIndicator)
			{
				filtertemp = "yes" ;
			}
			else 
			{
				filtertemp = "no" ;
			}

            FileInfo filterFile = new FileInfo(filterXMLfilename);
			string filterXMLfilenametemp = "" ;
            if (filterFile.Exists)
                filterXMLfilenametemp = filterXMLfilename;

			xslArgList.AddParam("filter" ,"" ,filtertemp);
			xslArgList.AddParam("filterXMLfilename" ,"",filterXMLfilenametemp);
			
            //XslTransform xslt = new XslTransform ();
            //xslt.Load(theResultsStyleSheetFullFileName);
            //XPathDocument xpathdocument = new XPathDocument(theXmlFullFileName);
            //FileStream fileStream = new FileStream(theHtmlFullFileName, FileMode.Create, FileAccess.ReadWrite);
            //xslt.Transform(xpathdocument, xslArgList, fileStream);
            //fileStream.Close();

            XslCompiledTransform xslt = new XslCompiledTransform();
            XsltSettings settings = new XsltSettings();
            settings.EnableDocumentFunction = true;

            xslt.Load(theResultsStyleSheetFullFileName, settings, null);
            XmlReader reader = XmlReader.Create(theXmlFullFileName);
            XmlTextWriter writer = new XmlTextWriter(theHtmlFullFileName, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.None;
            xslt.Transform(reader, xslArgList, writer);
            writer.Flush();
            writer.Close();
            reader.Close();
		}
		
		/// <summary>
		/// Show a HTML link.
		/// </summary>
		/// <param name="theHtmlFullFileName">The HTML link.</param>
		/// <param name="isNewURL">Indicates if this is a new link or an existing link (back/forward button is used)</param>
		public void ShowHtml(string theHtmlFullFileName)
		{
            _webBrowser.Navigate(theHtmlFullFileName);
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

			//If the string contains a '#', remove it and all following characters.
            int theCrossIndex = theReturnValue.IndexOf('#');

            if (theCrossIndex != -1)
            {
                theReturnValue = theReturnValue.Substring(0, theCrossIndex);
            }

			theReturnValue = theReturnValue.Replace("%20", " ");

			return(theReturnValue);
		}

		/// <summary>
		/// Indicates if it is possible to navigate back.
		/// </summary>
		/// <returns></returns>
		public bool BackEnabled()
		{
            return (_webBrowser.CanGoBack);
		}

		/// <summary>
		/// Indicates if it is possible to navigate forward.
		/// </summary>
		/// <returns></returns>
		public bool ForwardEnabled()
		{
            return (_webBrowser.CanGoForward);
		}

		/// <summary>
		/// Show the previous URL link.
		/// </summary>
		public void Back()
		{
			if (BackEnabled())
			{
				_webBrowser.GoBack();
			}
		}

		/// <summary>
		/// Show the next URL link.
		/// </summary>
		public void Forward()
		{
			if (ForwardEnabled())
			{
				_webBrowser.GoForward();
			}
		}

		/// <summary>
		/// Handler of the NavigateComplete2 event.
		/// </summary>
		public void NavigateComplete2Handler()
		{
            mshtml.IHTMLTxtRange theIHTMLTxtRange = null;

            if (_webBrowser.Document == null)
                return;

            _HTMLDocument = (mshtml.HTMLDocument)_webBrowser.Document.DomDocument;
            _HTMLBody = (mshtml.HTMLBody)_HTMLDocument.body;

            // Determine if errors exist in the displayed Html.
            theIHTMLTxtRange = _HTMLBody.createTextRange();
            if (theIHTMLTxtRange.text == null)
            {
            }
            else
            {
                _ContainsErrors = theIHTMLTxtRange.findText(_ERROR, theIHTMLTxtRange.text.Length, _FIND_MATCH_CASE | _FIND_MATCH_WHOLE_WORD);
            }

            // Determine if warnings exist in the displayed Html.
            theIHTMLTxtRange = _HTMLBody.createTextRange();
            if (theIHTMLTxtRange.text == null)
            {
            }
            else
            {
                _ContainsWarnings = theIHTMLTxtRange.findText(_WARNING, theIHTMLTxtRange.text.Length, _FIND_MATCH_CASE | _FIND_MATCH_WHOLE_WORD);
            }

            _FindRemainingText = _HTMLBody.createTextRange();
            ((mshtml.IHTMLSelectionObject)_HTMLDocument.selection).empty();
		}

		public bool ContainsErrors
		{
			get
			{
				return _ContainsErrors;
			}
		}

		public bool ContainsWarnings
		{
			get
			{
				return _ContainsWarnings;
			}
		}

		public void FindNextWarning()
		{
			FindNextText(_WARNING, true, true);
		}

		public void FindNextError()
		{
			FindNextText(_ERROR, true, true);
		}

		public void FindNextText(string theText, bool mustMatchWholeWord, bool mustMatchCase)
		{
            // define the search options
			int theSearchOption = 0;

			if (mustMatchWholeWord)
			{
				theSearchOption += _FIND_MATCH_WHOLE_WORD;
			}

			if (mustMatchCase)
			{
				theSearchOption += _FIND_MATCH_CASE;
			}

			if ( (_FindRemainingText == null) || (_FindRemainingText.text == null) )
			{
				// Sanity check.
				Debug.Assert(false);
			}
			else
			{
				// perform the search operation
				if (_FindRemainingText.findText(theText, _FindRemainingText.text.Length, theSearchOption))
				{
					// Select the found text within the document
					_FindRemainingText.select();

					// Limit the new find range to be from the newly found text
					mshtml.IHTMLTxtRange theFoundRange = (mshtml.IHTMLTxtRange)_HTMLDocument.selection.createRange();
					_FindRemainingText = (mshtml.IHTMLTxtRange)_HTMLBody.createTextRange();
					_FindRemainingText.setEndPoint("StartToEnd", theFoundRange);
				}
				else
				{
					// Reset the find ranges
					_FindRemainingText = _HTMLBody.createTextRange();
					((mshtml.IHTMLSelectionObject)_HTMLDocument.selection).empty();

					MessageBox.Show("Finished searching the document", string.Format("Find text \"{0}\"", theText), MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
		}
	}
}
