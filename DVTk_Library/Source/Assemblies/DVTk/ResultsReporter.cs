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

using Dvtk.Sessions;
using DvtkData.ComparisonResults;

namespace Dvtk.Results
{
	/// <summary>
	/// Summary description for ResultsReporter.
	/// </summary>
	public class ResultsReporter
	{
		private Dvtk.Sessions.ScriptSession _scriptSession = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="resultsDirectory">Directory where the results files are to be written.</param>
		public ResultsReporter(System.String resultsDirectory)
		{
			_scriptSession = new ScriptSession();
			_scriptSession.ResultsRootDirectory = resultsDirectory;
			_scriptSession.DetailedValidationResults = true;
            _scriptSession.SummaryValidationResults = true;
            _scriptSession.TestLogValidationResults = false;
		}

		/// <summary>
		/// Start the results reporting to the given file.
		/// </summary>
		/// <param name="filename">Results filename.</param>
		public void Start(System.String filename)
		{
			_scriptSession.StartResultsGathering(filename);
		}

		/// <summary>
		/// Stop the results reporting.
		/// </summary>
		public void Stop()
		{
			_scriptSession.EndResultsGathering();
		}

		/// <summary>
		/// Write the Message Comparision Results to the results reporter.
		/// </summary>
		/// <param name="messageComparisonResults">Message comparison results.</param>
		public void WriteMessageComparisonResults(MessageComparisonResults messageComparisonResults)
		{
			_scriptSession.WriteMessageComparisonResults(messageComparisonResults);
		}

		/// <summary>
		/// Write the Validation Error message to the results reporter.
		/// </summary>
		/// <param name="message">Error message.</param>
		public void WriteValidationError(String message)
		{
			_scriptSession.WriteValidationError(message);
		}

		/// <summary>
		/// Write the Validation Information message to the results reporter.
		/// </summary>
		/// <param name="message">Information message.</param>
		public void WriteValidationInformation(String message)
		{
			_scriptSession.WriteValidationInformation(message);
		}

		/// <summary>
		/// Write the HTML Information message to the results reporter.
		/// </summary>
		/// <param name="htmlMessage">HTML message.</param>
		public void WriteHtmlInformation(String htmlMessage)
		{
			_scriptSession.WriteHtmlInformation(htmlMessage);
		}

		/// <summary>
		/// Write the Validation Warning message to the results reporter.
		/// </summary>
		/// <param name="message">Warning message.</param>
		public void WriteValidationWarning(String message)
		{
			_scriptSession.WriteValidationWarning(message);
		}

		/// <summary>
		/// Property - Total Number of Errors in this session.
		/// </summary>
		public System.UInt32 NrErrors
		{
			get
			{
				return _scriptSession.NrOfErrors;
			}
		}

        /// <summary>
        /// Add the given number of errors to the total validation error count.
        /// </summary>
        /// <param name="errors">Number of errors to add.</param>
        public void AddValidationErrors(System.UInt32 errors)
        {
            _scriptSession.NrOfValidationErrors += errors;
        }

		/// <summary>
		/// Property - Total Number of Warnings in this session.
		/// </summary>
		public System.UInt32 NrWarnings
		{
			get
			{
				return _scriptSession.NrOfWarnings;
			}
		}

        /// <summary>
        /// Add the given number of warnings to the total validation warning count.
        /// </summary>
        /// <param name="warnings">Number of warnings to add.</param>
        public void AddValidationWarnings(System.UInt32 warnings)
        {
            _scriptSession.NrOfValidationWarnings += warnings;
        }
	}

	/// <summary>
	/// Summary description of the Results XSLT class.
	/// </summary>
	public class Xslt
	{
        /// <summary>
        /// The full file name of the stylesheet used to convert results .xml files to .html files.
        /// This stylesheet is used e.g. when the property ShowResults is set to true.
        /// 
        /// The default value for this property is the executable path append with the file name "DVT_RESULTS.xslt".
        /// </summary>
        public static String StyleSheetFullFileName
        {
            get
            {
                return (styleSheetFullFileName);
            }
            set
            {
                styleSheetFullFileName = value;
            }
        }
        static String styleSheetFullFileName = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "DVT_RESULTS.xslt");

		/// <summary>
		/// Transform the XML file into HTML. Try to do both the Summary and Detailed files.
		/// </summary>
		/// <param name="directory">Results directory.</param>
		/// <param name="filename">Results filename.</param>
		/// <returns>Detailed or Summary HTML Results filename.</returns>
		public static System.String Transform(System.String directory, System.String filename)
		{
			System.String resultsFilename = System.String.Empty;

			try 
			{
				// set up transformer
                XslCompiledTransform xslt = new XslCompiledTransform();
                XsltSettings settings = new XsltSettings();
                settings.EnableDocumentFunction = true;

                xslt.Load(StyleSheetFullFileName, settings, null);

				try
				{
					// transform summary results
					if(directory.EndsWith("\\"))
					{
						resultsFilename = directory + "Summary_" + filename;
					}
					else
					{
						resultsFilename = directory + "\\Summary_" + filename;
					}
                    
                    XmlReader reader = XmlReader.Create(resultsFilename);
                    resultsFilename = resultsFilename.Replace(".xml", ".html");
                    XmlTextWriter writer = new XmlTextWriter(resultsFilename, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.None;
                    xslt.Transform(reader, null, writer);
                    writer.Flush();
                    writer.Close();
                    reader.Close();
				}
				catch
				{
					// can't transform summary results - summary XML file not found
				}

				try
				{
					// transform detailed results
					if(directory.EndsWith("\\"))
					{
						resultsFilename = directory + "Detail_" + filename;
					}
					else
					{
						resultsFilename = directory + "\\Detail_" + filename;
					}

                    XmlReader reader = XmlReader.Create(resultsFilename);
                    resultsFilename = resultsFilename.Replace(".xml", ".html");
                    XmlTextWriter writer = new XmlTextWriter(resultsFilename, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.None;
                    xslt.Transform(reader, null, writer);
                    writer.Flush();
                    writer.Close();
                    reader.Close();
				}
				catch
				{
					// can't transform detailed results - detailed XML file not found
				}
			}
			catch (System.Exception e)
			{
				// can't setup transformer - style sheet not found
				Console.WriteLine("XML transformation exception: {0}", e.Message);
			}

			// return the detailed results filename
			// - otherwise return the summary results
			// - otherwise return empty string
			return resultsFilename;
		}
	}
}
