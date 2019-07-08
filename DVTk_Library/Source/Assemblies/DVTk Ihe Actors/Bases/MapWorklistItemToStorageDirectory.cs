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

using DvtkData.Dimse;

namespace Dvtk.IheActors.Bases
{
	/// <summary>
	/// Summary description for MapWorklistItemToStorageDirectory.
	/// </summary>
	public class MapWorklistItemToStorageDirectory
	{
		private DvtkData.Dimse.Tag _tag = null;
		private Hashtable _mapper = new Hashtable();

		/// <summary>
		/// Class constructor.
		/// </summary>
		public MapWorklistItemToStorageDirectory() {}

		/// <summary>
		/// Property - MapTag.
		/// </summary>
		public DvtkData.Dimse.Tag MapTag
		{
			get
			{
				return _tag;
			}
			set
			{
				_tag = value;
			}
		}

		/// <summary>
		/// Clear the Worklist Item to Storage Directory Mappings 
		/// - the MapTag remains set (and can be changed by the MapTag property).
		/// </summary>
		public void ClearMappings()
		{
			_mapper = new Hashtable();
		}

		/// <summary>
		/// Add a new Worklist Item to Storage Directory Mapping. 
		/// </summary>
		/// <param name="description">Worklist Item Description.</param>
		/// <param name="storageDirectory">Absolute Storage Directory name.</param>
		public void AddMapping(System.String description, System.String storageDirectory)
		{
			_mapper.Add(description, storageDirectory);
		}

		/// <summary>
		/// Remove a Worklist Item to Storage Directory Mapping.
		/// </summary>
		/// <param name="description">Worklist Item Description.</param>
		public void RemoveMapping(System.String description)
		{
			_mapper.Remove(description);
		}

		/// <summary>
		/// Check if the given Worklist Item Description already exists as a the mapping.
		/// </summary>
		/// <param name="description">Worklist Item Description.</param>
		/// <returns>bool - true if mapping already exists - false otherwise.</returns>
		public bool IsExistingMapping(System.String description)
		{
			return _mapper.Contains(description);
		}

		/// <summary>
		/// Return the Storage Directory matching the given Worklist Item Description.
		/// </summary>
		/// <param name="description">Worklist Item Description.</param>
		/// <returns>Returns the matching Storage Directory - empty string if no match found.</returns>
		public System.String GetStorageDirectory(System.String description)
		{
			System.String storageDirectory = (System.String)_mapper[description];
			if (storageDirectory == null)
			{
				storageDirectory = System.String.Empty;
			}

			return storageDirectory;
		}
	}
}
