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
	/// List of one or more SOP classes.
	/// </summary>
	public class SopClasses
	{
        //
        // - Fields -
        //
		private StringCollection list = new StringCollection();



        //
        // - Constructors -
        //

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">List of SOP classes.</param>
		public SopClasses(params String[] list)
		{
			this.list.AddRange(list);
		}



        //
        // - Methods -
        //		

        /// <summary>
        /// Adds a single Sop Class to the list.
        /// </summary>
        /// <param name="sopClass">SOP class.</param>
        public void Add(System.String sopClass)
        {
            // add the single Sop Class UID.
            this.list.Add(sopClass);
        }

        internal StringCollection List
        {
            get
            {
                return (this.list);
            }
        }

	}
}
