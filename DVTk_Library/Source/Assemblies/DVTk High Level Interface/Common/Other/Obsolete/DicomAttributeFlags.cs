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



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// </summary>
	public enum DicomAttributeFlags: short
	{
        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		None = 1<<0,
        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		Present = 1<<1,
        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		Not_present = 1<<2,
        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		Values = 1<<3,
        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		No_values = 1<<4,
        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		Include_sequence_items = 1<<5
	}
}
