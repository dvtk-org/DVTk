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
	/// Summary description for Hl7AttributeCollection.
	/// </summary>
	internal class Hl7AttributeCollection: AttributeCollectionBase
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See static property Empty.
		/// </summary>
		private static Hl7AttributeCollection empty = new Hl7AttributeCollection();

		/// <summary>
		/// See property AttributeSetOnly.
		/// </summary>
		private Dvtk.Hl7.Messages.Hl7Message hl7MessageOnly = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private Hl7AttributeCollection()
		{
			// Do nothing.
		}




		public Hl7AttributeCollection(Dvtk.Hl7.Messages.Hl7Message hl7MessageOnly)
		{
			this.hl7MessageOnly = hl7MessageOnly;
			Flags = FlagsConvertor.ConvertToFlagsBase(FlagsHl7Attribute.None);
		}



		public Hl7AttributeCollection(Dvtk.Hl7.Messages.Hl7Message hl7MessageOnly, FlagsHl7Attribute flagsHl7Attribute)
		{
			this.hl7MessageOnly = hl7MessageOnly;
			Flags = FlagsConvertor.ConvertToFlagsBase(flagsHl7Attribute);
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Property to get the actual Dicom Attribute Set.
		/// </summary>
		public Dvtk.Hl7.Messages.Hl7Message Hl7MessageOnly
		{
			get
			{
				return(this.hl7MessageOnly);
			}
		}
	}
}
