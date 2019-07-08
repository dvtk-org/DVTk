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
using System.Windows.Forms;
using System.Diagnostics;


namespace Dvt
{
	/// <summary>
	/// Class adding extra methods concerning paths not present in the .NET Path class.
	/// </summary>
	class PathUtils
	{
		/// <summary>
		/// Returns a boolean indicating if the path is relative.
		/// </summary>
		/// <param name="thePath">The path.</param>
		/// <returns>Boolean indicating if the path is relative.</returns>
		public static bool IsRelativePath(string thePath)
		{
			bool isRelativePath = false;

			if (thePath.StartsWith(".\\"))
			{
				isRelativePath = true;
			}
			else if (thePath.StartsWith("..\\"))
			{
				isRelativePath = true;
			}
			else
			{
				isRelativePath = false;
			}

			return(isRelativePath);
		}

		/// <summary>
		/// Converts a relative path to an absolute path.
		/// </summary>
		/// <param name="theBasePath">The path to which the relative path is relative to.</param>
		/// <param name="theRelativePath">The relative path (must start with ".\" or "..\".</param>
		/// <returns>
		/// Returns the absolute path.
		/// If "theBasePath" directory does not exist or if "theRelativePath" directory is not relative,
		/// an empty string is returned.
		/// </returns>
		public static string ConvertToAbsolutePath(string theBasePath, string theRelativePath)
		{
			string theAbsolutePath = "";

			if (!IsRelativePath(theRelativePath))
			{
				// Sanity check.
				Debug.Assert(false, "Relative path expected!");

				theAbsolutePath = "";
			}
			else
			{
				try
				{
					string theOldCurrentDirectory = System.IO.Directory.GetCurrentDirectory();

					System.IO.Directory.SetCurrentDirectory(theBasePath);
					theAbsolutePath = System.IO.Path.GetFullPath(theRelativePath);
					System.IO.Directory.SetCurrentDirectory(theOldCurrentDirectory);
				}
				catch (Exception theException)
				{
					Debug.Assert(false, theException.Message);

					theAbsolutePath = "";
				}
			}

			return(theAbsolutePath);
		}
	}
}
