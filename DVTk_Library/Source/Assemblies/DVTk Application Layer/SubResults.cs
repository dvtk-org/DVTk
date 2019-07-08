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

namespace DvtkApplicationLayer
{
	/// <summary>
	/// Summary description for SubResults.
	/// </summary>
	public class SubResults: Results
	{
		public SubResults(String baseName, String date, String time, String sutVersion, PassedStateEnum passedState): base(baseName, date, time, sutVersion, passedState)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Remove the sub results files (one summary and/or detail sub results files this object represents).
		/// </summary>
		public void Remove()
		{
			// TODO.
		}
	}
}
