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



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Flags the are relevant for HL7 Attributes.
	/// </summary>
	public enum FlagsHl7Attribute: short
	{
        /// <summary>
        /// None.
        /// </summary>
		None = 1<<0,
        /// <summary>
        /// Check if all attributes with this flag are either all present or are all not present.
        /// </summary>
		Compare_present = 1<<1,
        /// <summary>
        /// Check if all attributes with this flag have the same values.
        /// </summary>
		Compare_values = 1<<2,
        /// <summary>
        /// Check if an attribute with this flag is present.
        /// </summary>
		Present = 1<<3,
        /// <summary>
        /// Check if an attribute with this flag is not present.
        /// </summary>
		Not_present = 1<<4,
	}
}
