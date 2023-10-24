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



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// List of transfer syntaxes.
	/// </summary>
	public class TransferSyntaxes
	{
		/// <summary>
		/// List of transfer syntax UIDs.
		/// </summary>
		private StringCollection list = new StringCollection();



        //
        // - Constructors -
        //

		/// <summary>
		/// Default constructor.
		/// </summary>
		public TransferSyntaxes()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="list">List of transfer syntaxes.</param>
		public TransferSyntaxes(params String[] list)
		{
			// Fill the array with the given list of Transfer Syntax UIDs.
			this.list.AddRange(list);
		}



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the list of transfer syntax UID's.
        /// </summary>
		internal StringCollection List
		{
			get
			{
				return(this.list);
			}
		}



        //
        // - Methods -
        //		

		/// <summary>
		/// Adds a single transfer syntax to the list.
		/// </summary>
		/// <param name="transferSyntax"></param>
		public void Add(System.String transferSyntax)
		{
			// add the single Transfer Syntax UID.
			this.list.Add(transferSyntax);
		}

        /// <summary>
        /// Gets a single transfer syntax given a zero-based index.
        /// </summary>
        /// <param name="index">Zero-based index.</param>
        /// <returns>Transfer Syntax UID.</returns>
        public String Get(int index)
        {
            return this.list[index];
        }

		/// <summary>
        /// Gets the number of transfer syntaxes.
		/// </summary>
        /// <returns>Number of transfer syntaxes.</returns>
		public int Length()
		{
			return this.list.Count;
		}
	}
}
