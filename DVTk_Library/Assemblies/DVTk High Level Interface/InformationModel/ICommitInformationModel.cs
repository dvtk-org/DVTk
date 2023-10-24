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



namespace DvtkHighLevelInterface.InformationModel
{
	/// <summary>
	/// Summary description for ICommitInformationModel.
	/// </summary>
	public interface ICommitInformationModel
	{
		/// <summary>
		/// Check if the given instance is present in the Information Model. The instance will be at the leaf nodes of the Information Model.
		/// </summary>
		/// <param name="sopClassUid">SOP Class UID to search for.</param>
		/// <param name="sopInstanceUid">SOP Instance UID to search for.</param>
		/// <returns>Boolean - true if instance found in the Information Model, otherwise false.</returns>
		bool IsInstanceInInformationModel(System.String sopClassUid, System.String sopInstanceUid);
	}
}
