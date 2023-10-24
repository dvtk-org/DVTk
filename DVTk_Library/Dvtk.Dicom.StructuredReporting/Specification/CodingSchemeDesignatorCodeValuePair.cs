// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2010 DVTk
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

using VR = DvtkData.Dimse.VR;

namespace Dvtk.Dicom.StructuredReporting.Specification
{
    /// <summary>
    /// Pair of CodingSchemeDesignator and CodeValue that can be used as a key in a Dictionary
    /// instance.
    /// </summary>
    class CodingSchemeDesignatorCodeValuePair : IEquatable<CodingSchemeDesignatorCodeValuePair>
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property CodingSchemeDesignator.
        /// </summary>
        private string codingSchemeDesignator = null;

        /// <summary>
        /// See property CodeValue.
        /// </summary>
        private string codeValue = null;



        //
        // - Constructors -
        //

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private CodingSchemeDesignatorCodeValuePair()
        {
            // Do nothing.
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="codingSchemeDesignator">Coding Scheme Designator.</param>
        /// <param name="codeValue">Code Value.</param>
        public CodingSchemeDesignatorCodeValuePair(string codingSchemeDesignator, string codeValue)
        {
            this.codingSchemeDesignator = Convert.ToTrimmedString(codingSchemeDesignator, VR.SH);
            this.codeValue = Convert.ToTrimmedString(codeValue, VR.SH);
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets the Coding Scheme Designator.
        /// </summary>
        /// <remarks>
        /// Leading and trailing spaces from the codingSchemeDesignator that were supplied in the
        /// constructor will not be present anymore in the returned value.
        /// </remarks>
        public string CodingSchemeDesignator
        {
            get
            {
                return (this.codingSchemeDesignator);
            }
        }

        /// <summary>
        /// Gets the Code Value.
        /// </summary>
        /// <remarks>
        /// Leading and trailing spaces from the codeValue that were supplied in the
        /// constructor will not be present anymore in the returned value.
        /// </remarks>
        public string CodeValue
        {
            get
            {
                return (this.codeValue);
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Indicates whether the current instance is equal to another instance of the same type.
        /// </summary>
        /// <remarks>
        /// Needed to make sure that when an instance of this class is used as a key in a
        /// Dictionary instance, comparison of keys will always work.
        /// </remarks>
        /// <param name="other">An instance to compare with this instance.</param>
        /// <returns>true if the current instance is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(CodingSchemeDesignatorCodeValuePair other)
        {
            return ((this.codingSchemeDesignator == other.codingSchemeDesignator) && (this.codeValue == other.codeValue));
        }

        /// <summary>
        /// Gets the hash code of this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.codeValue.GetHashCode());
        }
    }
}
