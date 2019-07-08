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

namespace DvtkApplicationLayer
{
	/// <summary>
	/// Represents all results files for a test execution: main results file(s) and maybe some sub results file(s).
	/// </summary>
	public class MainResults: Results
	{
		private ArrayList subResultsList = new ArrayList();

		public MainResults(String baseName, String date, String time, String sutVersion, PassedStateEnum passedState): base(baseName, date, time, sutVersion, passedState)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Remove all results files (also the sub results files).
		/// </summary>
		public void Remove()
		{
			// TODO

			foreach(SubResults subResults in subResultsList)
			{
				subResults.Remove();
			}
		}

		/// <summary>
		/// Add a sub results to this main results.
		/// </summary>
		/// <param name="subResults"></param>
		public void Add(SubResults subResults)
		{
			subResultsList.Add(subResults);
		}
	}
}
