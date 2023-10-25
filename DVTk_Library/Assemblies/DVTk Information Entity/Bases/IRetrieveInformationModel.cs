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
using DvtkData.Dimse;
using System.IO;

namespace Dvtk.Dicom.InformationEntity
{
	/// <summary>
	/// Summary description for IRetrieveInformationModel.
	/// </summary>
	public interface IRetrieveInformationModel
	{
		/// <summary>
		/// Retrieve a list of filenames from the Information Model. The filenames match the
		/// individual instances matching the retrieve dataset attributes.
		/// </summary>
		/// <param name="retrieveDataset">Retrive dataset.</param>
		/// <returns>File list - containing the filenames of all instances matching the retrieve dataset attributes.</returns>
		DvtkData.Collections.StringCollection RetrieveInformationModel(DataSet retrieveDataset);
	}
}
