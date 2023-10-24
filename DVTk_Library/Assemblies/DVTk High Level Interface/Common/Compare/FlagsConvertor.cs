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
	/// Class used to convert from one type of flags to another type.
	/// </summary>
	internal class FlagsConvertor
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// Only allow the static methods of this class to be used.
		/// </summary>
		private FlagsConvertor()
		{
			// Do nothing.
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Convert FlagsDicomAttribute to FlagsBase.
		/// </summary>
		/// <param name="flagsDicomAttribute">The Dicom Attribute flags.</param>
		/// <returns>The base flags.</returns>
		public static FlagsBase ConvertToFlagsBase(FlagsDicomAttribute flagsDicomAttribute)
		{
			FlagsBase flags = FlagsBase.None;
			
			if ((flagsDicomAttribute & FlagsDicomAttribute.Compare_present) == FlagsDicomAttribute.Compare_present)
			{
				flags |= FlagsBase.Compare_present;
			}		

			if ((flagsDicomAttribute & FlagsDicomAttribute.Compare_values) == FlagsDicomAttribute.Compare_values)
			{
				flags |= FlagsBase.Compare_values;
			}		

			if ((flagsDicomAttribute & FlagsDicomAttribute.Present) == FlagsDicomAttribute.Present)
			{
				flags |= FlagsBase.Present;
			}		

			if ((flagsDicomAttribute & FlagsDicomAttribute.Not_present) == FlagsDicomAttribute.Not_present)
			{
				flags |= FlagsBase.Not_present;
			}		

			if ((flagsDicomAttribute & FlagsDicomAttribute.Compare_VR) == FlagsDicomAttribute.Compare_VR)
			{
				flags |= FlagsBase.Compare_VR;
			}		

			if ((flagsDicomAttribute & FlagsDicomAttribute.Values) == FlagsDicomAttribute.Values)
			{
				flags |= FlagsBase.Values;
			}		

			if ((flagsDicomAttribute & FlagsDicomAttribute.No_values) == FlagsDicomAttribute.No_values)
			{
				flags |= FlagsBase.No_values;
			}		

			if ((flagsDicomAttribute & FlagsDicomAttribute.Include_sequence_items) == FlagsDicomAttribute.Include_sequence_items)
			{
				flags |= FlagsBase.Include_sequence_items;
			}		

			return(flags);
		}

		/// <summary>
		/// Convert FlagsHl7Attribute to FlagsBase.
		/// </summary>
		/// <param name="flagsHl7Attribute">The HL7 Attribute flags.</param>
		/// <returns>The base flags.</returns>
		public static FlagsBase ConvertToFlagsBase(FlagsHl7Attribute flagsHl7Attribute)
		{
			FlagsBase flags = FlagsBase.None;
			
			if ((flagsHl7Attribute & FlagsHl7Attribute.Compare_present) == FlagsHl7Attribute.Compare_present)
			{
				flags |= FlagsBase.Compare_present;
			}		

			if ((flagsHl7Attribute & FlagsHl7Attribute.Compare_values) == FlagsHl7Attribute.Compare_values)
			{
				flags |= FlagsBase.Compare_values;
			}		

			if ((flagsHl7Attribute & FlagsHl7Attribute.Present) == FlagsHl7Attribute.Present)
			{
				flags |= FlagsBase.Present;
			}		

			if ((flagsHl7Attribute & FlagsHl7Attribute.Not_present) == FlagsHl7Attribute.Not_present)
			{
				flags |= FlagsBase.Not_present;
			}		

			return(flags);
		}

		public static FlagsDicomAttribute ConvertToFlagsDicomAttribute(FlagsBase flagsBase)
		{
			FlagsDicomAttribute flags = FlagsDicomAttribute.None;
			
			if ((flagsBase & FlagsBase.Compare_present) == FlagsBase.Compare_present)
			{
				flags |= FlagsDicomAttribute.Compare_present;
			}		

			if ((flagsBase & FlagsBase.Compare_values) == FlagsBase.Compare_values)
			{
				flags |= FlagsDicomAttribute.Compare_values;
			}		

			if ((flagsBase & FlagsBase.Present) == FlagsBase.Present)
			{
				flags |= FlagsDicomAttribute.Present;
			}		

			if ((flagsBase & FlagsBase.Not_present) == FlagsBase.Not_present)
			{
				flags |= FlagsDicomAttribute.Not_present;
			}		

			if ((flagsBase & FlagsBase.Compare_VR) == FlagsBase.Compare_VR)
			{
				flags |= FlagsDicomAttribute.Compare_VR;
			}		

			if ((flagsBase & FlagsBase.Values) == FlagsBase.Values)
			{
				flags |= FlagsDicomAttribute.Values;
			}		

			if ((flagsBase & FlagsBase.No_values) == FlagsBase.No_values)
			{
				flags |= FlagsDicomAttribute.No_values;
			}		

			if ((flagsBase & FlagsBase.Include_sequence_items) == FlagsBase.Include_sequence_items)
			{
				flags |= FlagsDicomAttribute.Include_sequence_items;
			}		

			return(flags);

		}

	}
}
