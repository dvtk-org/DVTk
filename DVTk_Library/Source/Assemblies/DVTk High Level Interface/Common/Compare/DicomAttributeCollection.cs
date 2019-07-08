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
	/// Class representing a collection of Dicom attributes
	/// in combination with some possible validation flags.
	/// </summary>
	internal class DicomAttributeCollection: AttributeCollectionBase
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property AttributeSetOnly.
		/// </summary>
		private DvtkHighLevelInterface.Dicom.Other.AttributeSet attributeSetOnly = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private DicomAttributeCollection()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="attributeSetOnly">The actual collection of Dicom Attributes.</param>
		internal DicomAttributeCollection(DvtkHighLevelInterface.Dicom.Other.AttributeSet attributeSetOnly)
		{
			this.attributeSetOnly = attributeSetOnly;
			Flags = FlagsConvertor.ConvertToFlagsBase(FlagsDicomAttribute.None);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="attributeSetOnly">The actual collection of Dicom Attributes.</param>
		/// <param name="flagsDicomAttribute">The flags used for this collection.</param>
		internal DicomAttributeCollection(DvtkHighLevelInterface.Dicom.Other.AttributeSet attributeSetOnly, FlagsDicomAttribute flagsDicomAttribute)
		{
			this.attributeSetOnly = attributeSetOnly;
			Flags = FlagsConvertor.ConvertToFlagsBase(flagsDicomAttribute);
		}




		internal DicomAttributeCollection(DvtkHighLevelInterface.Dicom.Other.AttributeSet attributeSetOnly, FlagsBase flags)
		{
			this.attributeSetOnly = attributeSetOnly;
			Flags = flags;
		}




		//
		// - Properties -
		//

		/// <summary>
		/// Property to get the actual Dicom Attribute Set.
		/// </summary>
		public DvtkHighLevelInterface.Dicom.Other.AttributeSet AttributeSetOnly
		{
			get
			{
				return(this.attributeSetOnly);
			}
		}
	}
}
