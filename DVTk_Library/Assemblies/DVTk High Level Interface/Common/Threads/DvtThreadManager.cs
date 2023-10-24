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

using DvtkHighLevelInterface.Dicom.Threads;



namespace DvtkHighLevelInterface.Common.Threads
{
	/// <summary>
	/// Summary description for DvtThreadManager.
	/// </summary>
	public class DvtThreadManager: ThreadManager
	{
		private String baseName = "";


		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="baseName">The base name part of results files. For a VBS, this would be the VBS file name.</param>
		public DvtThreadManager(String baseName)
		{
			this.baseName = baseName;
		}

		public override void SetResultsOptions(Thread thread)
		{
			Thread topmostThread = thread.TopmostThread;

			if ((thread is DicomThread) && (topmostThread is DicomThread))
			{
				DicomThread dicomThread = thread as DicomThread;
				DicomThread topmostDicomThread = topmostThread as DicomThread;

				if (topmostDicomThread == dicomThread)
				{
					if (dicomThread.Options.ResultsFileName == null)
					{
						dicomThread.Options.ResultsFileName = String.Format("{0:000}_{1}_res.xml", dicomThread.Options.SessionId, this.baseName.Replace(".", "_"));
					}
				}
				else
				{
					dicomThread.Options.ResultsDirectory = topmostDicomThread.Options.ResultsDirectory;

					if (dicomThread.Options.ResultsFileName == null)
					{
						dicomThread.Options.ResultsFileName = String.Format("{0:000}_{1}_{2}_res.xml", dicomThread.Options.SessionId, this.baseName.Replace(".", "_"), dicomThread.Options.Identifier);
					}
				}
			}
		}
	}
}
