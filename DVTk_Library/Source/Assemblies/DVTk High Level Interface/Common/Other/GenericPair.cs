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
using System.Collections.Generic;
using System.Text;



namespace DvtkHighLevelInterface.Common.Other
{
    /// <summary>
    /// Represents an ordered pair of elements.
    /// </summary>
    /// <typeparam name="T1">The type of the first element of this pair.</typeparam>
    /// <typeparam name="T2">The type of the second element of this pair.</typeparam>
    public class GenericPair<T1, T2>
        where T1: class
        where T2 : class
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property Element1.
        /// </summary>
        private T1 element1 = null;

        /// <summary>
        /// See property Element2.
        /// </summary>
        private T2 element2 = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private GenericPair()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="element1">The first element of this pair.</param>
        /// <param name="element2">The second element of this pair.</param>
        public GenericPair(T1 element1, T2 element2)
        {
            this.element1 = element1;
            this.element2 = element2;
        }



        //
        // - Properties -
        //

        /// <summary>
        /// The first element of this pair.
        /// </summary>
        public T1 Element1
        {
            get
            {
                return (this.element1);
            }
        }

        /// <summary>
        /// The second element of this pair.
        /// </summary>
        public T2 Element2
        {
            get
            {
                return (this.element2);
            }
        }

    }
}
