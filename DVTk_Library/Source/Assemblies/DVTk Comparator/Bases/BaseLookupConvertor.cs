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

namespace Dvtk.Comparator.Bases
{
	/// <summary>
	/// Summary description for BaseLookupConvertor.
	/// </summary>
	public abstract class BaseLookupConvertor : BaseValueConvertor
	{
		private LookupEntryCollection _lookupTable = new LookupEntryCollection();

		protected void AddLookupEntry(System.String value1, System.String value2)
		{
			// Dicom value = value 1
			// Hl7 value = value 2
			_lookupTable.Add(new LookupEntry(value1, value2));
		}

		public override System.String FromHl7ToDicom(System.String hl7Value)
		{
			// Dicom value = value 1
			// Hl7 value = value 2
			System.String dicomValue = hl7Value;

			foreach (LookupEntry lookupEntry in _lookupTable)
			{
				if (lookupEntry.Value2 == hl7Value)
				{
					dicomValue = lookupEntry.Value1;
					break;
				}
			}

			return dicomValue;
		}

		public override System.String FromDicomToHl7(System.String dicomValue)
		{
			// Dicom value = value 1
			// Hl7 value = value 2
			System.String hl7Value = dicomValue;

			foreach (LookupEntry lookupEntry in _lookupTable)
			{
				if (lookupEntry.Value1 == dicomValue)
				{
					hl7Value = lookupEntry.Value2;
					break;
				}
			}

			return hl7Value;
		}
	}
}
