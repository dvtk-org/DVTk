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
	/// Specifies how a Dicom attribute should be validated.
	/// </summary>
	[Serializable]
	public class ValidationRuleDicomAttribute: ValidationRuleBase
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property DisplayFullTagSequenceString.
		/// </summary>
		private bool displayFullTagSequenceString = false;

		/// <summary>
		/// See static property Empty.
		/// </summary>
		private static ValidationRuleDicomAttribute empty = new ValidationRuleDicomAttribute();

		/// <summary>
		/// See property TagSequenceString.
		/// </summary>
		private String tagSequenceString = "";



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks><b>Do not use this constructor. It may only be used indirectly for serialization purposes.</b></remarks>
		public ValidationRuleDicomAttribute()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="tagSequenceString">TagSequenceString indicating the Dicom attribute to validate.</param>
		/// <param name="flagsDicomAttribute">The flag(s) indicating how to validate this Dicom Attribute.</param>
		public ValidationRuleDicomAttribute(String tagSequenceString, FlagsDicomAttribute flagsDicomAttribute)
		{
			this.tagSequenceString = tagSequenceString;

			Flags = FlagsConvertor.ConvertToFlagsBase(flagsDicomAttribute);
		}










		internal ValidationRuleDicomAttribute(String tagSequenceString, FlagsBase flags)
		{
			this.tagSequenceString = tagSequenceString;

			Flags = flags;
		}










		//
		// - Properties -
		//

		/// <summary>
		/// Indicates if the full TagSequence String should be displayed or only the tag.
		/// </summary>
		internal bool DisplayFullTagSequenceString
		{
			get
			{
				return(this.displayFullTagSequenceString);
			}
			set
			{
				this.displayFullTagSequenceString = value;
			}
		}









		/// <summary>
		/// The TagSequence String 
		/// </summary>
		public String TagSequenceString
		{
			get
			{
				return(this.tagSequenceString);
			}
			set
			{
				this.tagSequenceString = value;
			}
		}
	}
}
