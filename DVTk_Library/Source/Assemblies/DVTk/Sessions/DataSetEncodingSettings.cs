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

namespace Dvtk.Sessions
{
    /// <summary>
    /// Properties affecting the DataSet encoding for the DIMSE Dicom communication.
    /// </summary>
    public interface IDataSetEncodingSettings
    {
        /// <summary>
        /// The AutoType2Attributes option,
        /// causes DVT to add any Type 2 Attributes (with a zero-length) 
        /// to the defined Dataset before sending it to the Product. 
        /// </summary>
        /// <remarks>
        /// <p>
        /// DVT consults to Definition File corresponding to the Dataset in 
        /// order to check the Dataset “completeness” before sending it to the Product. 
        /// This feature ensures that the output produced by DVT conforms to 
        /// the Dataset definition without the User having to explicitly set any 
        /// zero-length Attributes.
        /// </p>
        /// </remarks>
        System.Boolean AutoType2Attributes
        {
            get;
            set;
        }
        /// <summary>
        /// The DefineSqLength option, 
        /// is used to make DVT encode explicit length Sequences when sending messages. 
        /// </summary>
        /// <remarks>
        /// <p>
        /// Explicit lengths are computed for both the Sequence and each Item 
        /// present within the Sequence.
        /// </p>
        /// <p>
        /// By default DVT uses the undefined length encoding.
        /// </p>
        /// </remarks>
        System.Boolean DefineSqLength
        {
            get;
            set;
        }
        /// <summary>
        /// The AddGroupLength option, 
        /// is used to have DVT add Group Length attributes to all 
        /// Groups found in each message sent. 
        /// </summary>
        /// <remarks>
        /// By default DVT does not encode Group Length attributes 
        /// (except for the Command Group Length).
        /// </remarks>
        System.Boolean AddGroupLength
        {
            get;
            set;
        }
    }
}