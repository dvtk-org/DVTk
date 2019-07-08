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

using DvtkHighLevelInterface.Common.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Contains the results of comparing two or more data sets.
	/// </summary>
	public class CompareResults
	{
        //
        // - Fields -
        //

        /// <summary>
        /// See property DifferencesCount.
        /// </summary>
		private int differencesCount = 0;

        /// <summary>
        /// See property Table.
        /// </summary>
		private Table table = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private CompareResults()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="numberOfColumns">The number of columns present in the table.</param>
		internal CompareResults(int numberOfColumns)
		{
			this.table = new Table(numberOfColumns);
			this.differencesCount = 0;
		}

        /// <summary>
        /// Gets or sets the total number of differences found.
        /// </summary>
		public int DifferencesCount
		{
			get
			{
				return(this.differencesCount);
			}

			set
			{
				this.differencesCount = value;
			}
		}

        /// <summary>
        /// Gets the table containing the results for comparing two or more data sets.
        /// </summary>
		public Table Table
		{
			get
			{
				return(this.table);
			}
		}
	}
}
