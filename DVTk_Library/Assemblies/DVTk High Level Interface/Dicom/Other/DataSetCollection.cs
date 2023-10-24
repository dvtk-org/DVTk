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
using System.IO;

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Threads;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Summary description for DataSetCollection.
	/// </summary>
	public sealed class DataSetCollection: GenericCollection<DataSet>
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DataSetCollection()
		{
			// Do nothing.
		}


      
        //
		// - Methods -
		//

        /// <summary>
        /// Clear this collections, read in all DataSet files in the specified path and add them to this collection.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="dicomThread">The DicomThread instance to use.</param>
		public void Read(String path, String searchPattern, DicomThread dicomThread)
		{
			Clear();

			String[] fileNames = Directory.GetFiles(path, searchPattern);

			foreach(String fileName in fileNames)
			{
				DataSet dataSet = new DataSet();
				dataSet.Read(fileName, dicomThread);
				Add(dataSet);
			}
		}

        /// <summary>
        /// Clear this collections, read in all DataSet files in the specified path and add them to this collection.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern to use.</param>
        /// <param name="definitionFilesFullName">The definition files to use.</param>
		public void Read(String path, String searchPattern, params String[] definitionFilesFullName)
		{
			Clear();

			String[] fileNames = Directory.GetFiles(path, searchPattern);

			foreach(String fileName in fileNames)
			{
				DataSet dataSet = new DataSet();
				dataSet.Read(fileName, definitionFilesFullName);
				Add(dataSet);
			}
		}
	}
}
