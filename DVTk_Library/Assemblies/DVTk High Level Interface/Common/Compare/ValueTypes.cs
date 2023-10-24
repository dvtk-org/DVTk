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



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Possible value types used during comparing.
	/// </summary>
	public enum CompareValueTypes: short
	{
        /// <summary>
        /// Identical.
        /// </summary>
		Identical = 1<<0,
        /// <summary>
        /// Date.
        /// </summary>
		Date = 1<<1,
        /// <summary>
        /// ID.
        /// </summary>
		ID = 1<<2,
        /// <summary>
        /// Name.
        /// </summary>
		Name = 1<<3,
        /// <summary>
        /// String.
        /// </summary>
		String = 1<<4,
        /// <summary>
        /// Time.
        /// </summary>
		Time = 1<<5,
        /// <summary>
        /// UID.
        /// </summary>
		UID = 1<<6
	}
}
