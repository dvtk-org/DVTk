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
using System.Collections.Specialized;
using System.Text;

using DvtkHighLevelInterface.Dicom.Threads;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
	/// The purpose for this class right now is to construct a table, set the contents of the seperate
	/// cells and convert it to a HTML table. In the future, this class may be enhanced to convert it to
	/// other formats.
	/// 
	/// The structure of the table without the header(s) is as follows:
	/// - A Table contains zero or more rows.
	/// - A row contains one or more cells.
	/// - A cell contains zero or more items.
	/// </summary>
	public class Table
	{
		//
		// - Fields -
		//

		/// <summary>
		/// The string that will be prefixed to any item.
		/// </summary>
		private String cellItemPrefix = "";

		/// <summary>
		/// A seperator that will be put between every item in a cell.
		/// </summary>
		private String cellItemSeperator = "<b> | </b>";

		/// <summary>
		/// Text may be added between the "td" brackets.
		/// </summary>
		private ArrayList cellPrefixes = new ArrayList();

		/// <summary>
		/// See method SetColumnPixelWidths.
		/// </summary>
		private ArrayList columnPixelWidths = new ArrayList();

		/// <summary>
		/// The current row, for which the columns may be filled with the various Add... methods.
		/// </summary>
		private ArrayList currentRow = null;

		/// <summary>
		/// the cell prefixes for the current row.
		/// </summary>
		private StringCollection cellPrefixesForCurrentRow = null;

		/// <summary>
		/// See property EmptyCellPrefix.
		/// </summary>
		private String emptyCellPrefix = "";

		/// <summary>
		/// All headers defined for this table. A single header should contain a string for each
		/// user defined column. 
		/// </summary>
		private ArrayList headers = new ArrayList();

		/// <summary>
		/// Number of user defined columns in the table. 
		/// </summary>
		private int numberOfColumns = 0;

		/// <summary>
		/// All rows currently present in the table.
		/// 
		/// The structure is as follows:
		/// - A Table contains zero or more rows.
		///   The table content is implemented by an ArrayList.
		/// - A row contains one or more cells.
		///   The row content is implemented by an ArrayList.
		/// - A cell contains zero or more items.
		///   The cell content is implemented by an StringCollection.
		/// </summary>
		private ArrayList rows = new ArrayList();



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private Table()
		{

		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="numberOfColumns">Number of user defined columns present in the table.</param>
		public Table(int numberOfColumns)
		{
			this.numberOfColumns = numberOfColumns;
		}



		//
		// - Fields -
		//

        /// <summary>
        /// Gets or sets a seperator that will be put between every item in a cell.
        /// </summary>
		public String CellItemSeperator
		{
			get
			{
				return(this.cellItemSeperator);
			}
			set
			{
				this.cellItemSeperator = value;
			}
		}

		/// <summary>
		/// When a cell is empty (has no cell prefix and no items), this string is used
		/// as prefix.
		/// </summary>
		public String EmptyCellPrefix
		{
			get
			{
				return(this.emptyCellPrefix);
			}
			set
			{
				this.emptyCellPrefix = value;
			}
		}


		//
		// - Methods -
		//

		/// <summary>
		/// Add a header to the table.
		/// </summary>
		/// <param name="header">
		/// A collection of strings describing the columns.
		/// The same amount of strings should be supplied as the number of user defined columns.
		/// </param>
		public void AddHeader(params String[] header)
		{
			StringCollection headerAsStringCollection = new StringCollection();

			headerAsStringCollection.AddRange(header);

			this.headers.Add(headerAsStringCollection);
		}

		/// <summary>
		/// Add a prefix for a cell.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="text">The prefix to add.</param>
		public void SetCellPrefix(int column, String text)
		{
			this.cellPrefixesForCurrentRow[column - 1]+= text;
			
		}

		/// <summary>
		/// Add an item to the cell in the current row, indicated by the supplied column number.
		/// </summary>
		/// <param name="column">One-based column number.</param>
		/// <param name="text">The item text.</param>
		public void AddItem(int column, String text)
		{
			StringCollection cell = (this.currentRow[column - 1] as StringCollection);

			cell.Add(text);
		}

		/// <summary>
		/// Add an item to the cell in the current row, indicated by the supplied column number.
		/// The item text will be made black.
		/// </summary>
		/// <param name="column">One-based column number.</param>
		/// <param name="text">The item text.</param>
		public void AddBlackItem(int column, String text)
		{
			String extendedText = "<font color=\"#000000\">" + text + "</font>";

			AddItem(column, extendedText);
		}

		/// <summary>
		/// Add an item to the cell in the current row, indicated by the supplied column number.
		/// The item text will be made red.
		/// </summary>
		/// <param name="column">One-based column number.</param>
		/// <param name="text">The item text.</param>
		public void AddRedItem(int column, String text)
		{
			String extendedText = "<font color=\"#FF0000\">" + text + "</font>";

			AddItem(column, extendedText);
		}

		/// <summary>
		/// Convert the table to HTML.
		/// </summary>
		/// <returns>The table convert to a HTML string.</returns>
		public String ConvertToHtml()
		{
			// Because a lot of adding is performed before getting the actual HTML table, it is more
			// efficient to use the StringBuilder class then to use to String class.
			StringBuilder htmlTable = new StringBuilder(100000);

			//
			// Start of table.
			//

			// htmlTable = "<br /><table border=\"1\" width=\"100%\" cellpadding=\"3\">\r\n";
			htmlTable.Append("<br /><table style=\"border-collapse: collapse\" border=\"1\" cellpadding=\"3\">");
			htmlTable.Append("<font color=\"#000080\">");


			//
			// Headers.
			//

			for (int headerIndex = 0; headerIndex < this.headers.Count; headerIndex++)
			{
				StringCollection header = this.headers[headerIndex] as StringCollection;

				htmlTable.Append("<tr>");

				int maxColumnIndex = Math.Min(this.numberOfColumns, header.Count);
				int numberOfPreviousIdenticalColumns = 0;

				for (int columnIndex = 1; columnIndex <= maxColumnIndex; columnIndex++)
				{
					String widthText = "";
					bool putColumnText = false;

					//
					// If this is the last header row and a width has been supplied for this column,
					// use it.
					//

					if (headerIndex == (this.headers.Count - 1))
					{
						if (columnIndex <= this.columnPixelWidths.Count)
						{
							widthText = " width=\"" + this.columnPixelWidths[columnIndex - 1].ToString() + "\"";
						}
					}


					//
					// If the next column has different text, put it in the html string, otherwise wait.
					//

					if (columnIndex == maxColumnIndex)
					{
						putColumnText = true;
					}
					else
					{
						if (header[columnIndex - 1] == header[columnIndex])
						{
							numberOfPreviousIdenticalColumns++;
							putColumnText = false;
						}
						else
						{
							putColumnText = true;
						}
					}

					if (putColumnText)
					{
						htmlTable.Append("<td align=\"center\" valign=\"top\"" + widthText + " class=\"item\" colspan=\"" + (numberOfPreviousIdenticalColumns + 1).ToString() + "\"><b>" + header[columnIndex - 1] + "</b></td>");
						numberOfPreviousIdenticalColumns = 0;
					}
				}

				htmlTable.Append("</tr>");
			}



			//
			// All other rows.
			//

			for (int rowIndex = 0; rowIndex < this.rows.Count; rowIndex++)
			{
				ArrayList row = this.rows[rowIndex] as ArrayList;

				StringCollection cellPrefixesForRow = this.cellPrefixes[rowIndex] as StringCollection;

				htmlTable.Append("<tr>");

				for(int cellIndex = 0; cellIndex < row.Count; cellIndex ++)
				{
					StringCollection cell = row[cellIndex] as StringCollection;

					String cellPrefix = cellPrefixesForRow[cellIndex];

					if ((cellPrefix.Length == 0) && (cell.Count == 0))
					{
						cellPrefix = this.emptyCellPrefix;
					}

					htmlTable.Append("<td " + cellPrefix + ">");

					for (int cellItemIndex = 0; cellItemIndex < cell.Count; cellItemIndex++)
					{
						if (cellItemIndex == (cell.Count - 1))
						{
							htmlTable.Append(this.cellItemPrefix + cell[cellItemIndex]);
						}
						else
						{
							htmlTable.Append(this.cellItemPrefix + cell[cellItemIndex] + this.cellItemSeperator);
						}
					}

					htmlTable.Append("</td>");
				}

				htmlTable.Append("</tr>");
			}


			//
			// End of the table.
			//

			htmlTable.Append("</font></table>");

			return(htmlTable.ToString());
		}

		/// <summary>
		/// Add a new row to the table.
		/// </summary>
		public void NewRow()
		{
			currentRow = new ArrayList();

			for (int index = 0; index < this.numberOfColumns; index++)
			{
				currentRow.Add(new StringCollection());
			}

			this.rows.Add(currentRow);

			// Initialize the arraylist used to store prefixes for the cells.
			cellPrefixesForCurrentRow = new StringCollection();

			for (int index = 0; index < this.numberOfColumns; index++)
			{
				cellPrefixesForCurrentRow.Add("");
			}

			this.cellPrefixes.Add(cellPrefixesForCurrentRow);
		}

		/// <summary>
		/// Set the width of the user defined columns in pixels.
		/// This method must be called before calling the method ConvertToHtml.
		/// </summary>
		/// <param name="columnPixelWidths">
		/// The width of the columns in pixels. The number of arguments must be the same as the number
		/// of user defined columns.
		/// </param>
		public void SetColumnPixelWidths(params int[] columnPixelWidths)
		{
			this.columnPixelWidths.AddRange(columnPixelWidths);
		}
	}
}
