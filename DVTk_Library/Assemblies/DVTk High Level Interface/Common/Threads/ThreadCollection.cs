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

using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Hl7.Threads;



namespace DvtkHighLevelInterface.Common.Threads
{
	/// <summary>
	/// A collection of threads.
	/// </summary>
	public sealed class ThreadCollection : DvtkData.Collections.NullSafeCollectionBase
	{
		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <value>The item at the specified <c>index</c>.</value>
		public new Thread this[int index]
		{
			get 
			{
				return ((Thread)base[index]);
			}
			set
			{
				base.Insert(index, value);
			}
		}

		/// <summary>
		/// Inserts an item to the IList at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
		/// <param name="value">The item to insert into the <see cref="System.Collections.IList"/>.</param>
		public void Insert(int index, Thread value)
		{
			base.Insert(index, value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the IList.
		/// </summary>
		/// <param name="value">The item to remove from the <see cref="System.Collections.IList"/>.</param>
		public void Remove(Thread value)
		{
			base.Remove(value);
		}

		/// <summary>
		/// Determines whether the <see cref="System.Collections.IList"/> contains a specific item.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns><see langword="true"/> if the item is found in the <see cref="System.Collections.IList"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(Thread value)
		{
			return (base.Contains(value));
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to locate in the <see cref="System.Collections.IList"/>.</param>
		/// <returns>The index of <c>value</c> if found in the list; otherwise, -1.</returns>
		public int IndexOf(Thread value)
		{
			return (base.IndexOf(value));
		}

		/// <summary>
		/// Adds an item to the <see cref="System.Collections.IList"/>.
		/// </summary>
		/// <param name="value">The item to add to the <see cref="System.Collections.IList"/>. </param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(Thread value)
		{
			return (base.Add(value));
		}

		/// <summary>
		/// Get a HTML table containing an overview of all started Threads.
        /// <br></br><br></br>
		/// When no started Thread exists, the supplied noStartedThreadsText is returned.
		/// </summary>
		/// <param name="headerText">Text to be displayed in the table header.</param>
		/// <param name="noStartedThreadsText">Text that is returned when no started Threads exist.</param>
		/// <param name="hyperlinkFromDirectory">If hyperlinks to results are in the same directory as specified here, a relative path is used.</param>
		/// <returns></returns>
		public String GetStartedThreadsOverviewAsHTML(String headerText, String noStartedThreadsText, String hyperlinkFromDirectory)
		{
			// Start of table.
			int rowCount = 0;
			String returnValue = "";
			String htmlTable = "<br /><table border=\"1\" width=\"100%\" cellpadding=\"3\">\r\n";
			htmlTable+= "<font color=\"#000080\">\r\n";

			// Header row 1.
			htmlTable+= "<tr><td align=\"center\" valign=\"top\" class=\"item\" colspan=\"3\"><b>" + headerText + "</b></td></tr>\r\n";

			// Header row 2.
			htmlTable+= "<tr>\r\n";
			htmlTable+= "<td valign=\"top\" width=\"200\" class=\"item\"><b>Thread<b></td>\r\n";
			htmlTable+= "<td valign=\"top\" width=\"200\" class=\"item\"><b>Hyperlinks</b></td>\r\n";
			htmlTable+= "<td valign=\"top\" class=\"item\"><b>Comments</b></td>\r\n";
			htmlTable+= "</tr>\r\n";

			// For each sub Thread, a table row.
			foreach (Thread childThread in this)
			{
				// Only if the child Thread has been started, show it in the table.
				if (childThread.HasBeenStarted)
				{
					String type = "-";
					String hyperlinks = "-";
					String comments = "";

					htmlTable+= "<tr>\r\n";

					// If the child Thread is a DicomThread...
					if (childThread is DicomThread)
					{
						DicomThread childDicomThread = childThread as DicomThread;
						UInt32 errorCount = childDicomThread.DvtkScriptSession.NrOfErrors;
						UInt32 warningCount = childDicomThread.DvtkScriptSession.NrOfWarnings;

						// Type.
						type = "Dicom Thread \"" + childThread.ThreadOptions.Identifier + "\"";

						// Comments
						if (errorCount > 0)
						{
							comments+= "<font color=\"#FF0000\">";
						}
						comments+= "Number of Errors: " + errorCount.ToString() + "<br />";
						if (errorCount > 0)
						{
							comments+= "</font>";
						}

						if (warningCount > 0)
						{
							comments+= "<font color=\"#FF0000\">";
						}
						comments+= "Number of Warnings: " + warningCount.ToString() + "\r\n";
						if (warningCount > 0)
						{
							comments+= "</font>";
						}

						if (comments.Length == 0)
						{
							comments = "-";
						}

						// Hyperlinks.
						// If the results directories are the same, use a relative path.
						if (hyperlinkFromDirectory == childDicomThread.Options.ResultsDirectory)
						{
							if (childDicomThread.Options.GenerateDetailedResults)
							{
								hyperlinks = "View <a href=\"" + childDicomThread.Options.DetailResultsFileNameOnly + "\">detail results</a><br />\r\n";
                            }
                            if (childDicomThread.Options.GenerateTestLog)
                            {
                                hyperlinks += "View <a href=\"" + childDicomThread.Options.TestLogFileNameOnly + "\">test log</a><br />\r\n";
                            }
                            if (childDicomThread.Options.GenerateSummary)
                            {
                                hyperlinks += "View <a href=\"" + childDicomThread.Options.SummaryResultsFileNameOnly + "\">summary results</a><br />\r\n";
                            }
						}
							// If the results directories are not the same, use an absolute path.
						else
						{
							if (childDicomThread.Options.GenerateDetailedResults)
							{
								hyperlinks = "View <a href=\"" + childDicomThread.Options.DetailResultsFullFileName + "\">detail results</a><br />\r\n";
                            }
                            if (childDicomThread.Options.GenerateTestLog)
                            {
                                hyperlinks += "View <a href=\"" + childDicomThread.Options.TestLogFullFileName + "\">test log</a><br />\r\n";
                            }
                            if (childDicomThread.Options.GenerateSummary)
                            {
                                hyperlinks += "View <a href=\"" + childDicomThread.Options.SummaryResultsFullFileName + "\">summary results</a><br />\r\n";
                            }
						}
					}
		
						// If the child Thread is a HL7Thread...
						//						else if (childThread is Hl7Thread)
						//						{
						//							type = "HL7 Thread \"" + childThread.Options.Identifier + "\"";
						//						}
						// In all other cases...
					else
					{
						// Do nothing.
					}
					
					htmlTable+= "<td valign=\"top\" width=\"200\" class=\"item\">" + type + "</td>\r\n";
					htmlTable+= "<td valign=\"top\" width=\"200\" class=\"item\">" + hyperlinks + "</td>\r\n";
					htmlTable+= "<td valign=\"top\" class=\"item\">" + comments + "</td>\r\n";

					htmlTable+= "</tr>\r\n";
					rowCount++;
				}
			}
		
																																																																				  
			// End of table.
			htmlTable+= "</td></tr></font></table>\r\n";

			if (rowCount > 0)
			{
				returnValue = htmlTable;
			}
			else
			{
				returnValue = "<br /><b>" + noStartedThreadsText + "</b><br />";
			}

			return(returnValue);
		}
	}
}
