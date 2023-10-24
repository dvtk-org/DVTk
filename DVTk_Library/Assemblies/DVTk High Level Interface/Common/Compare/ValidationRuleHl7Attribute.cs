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

using Dvtk.Hl7;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Specifies how a HL7 attribute should be validated.
	/// </summary>
	[Serializable]
	public class ValidationRuleHl7Attribute: ValidationRuleBase
	{
		//
		// - Fields -
		//

		private Hl7Tag hl7Tag = null;



		/// <summary>
		/// See static property Empty.
		/// </summary>
		private static ValidationRuleHl7Attribute empty = new ValidationRuleHl7Attribute();



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <remarks><b>Do not use this constructor. It may only be used indirectly for serialization purposes.</b></remarks>
		public ValidationRuleHl7Attribute()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="hl7Tag">HL7 tag indicating the HL7 attribute to validate.</param>
        /// <param name="flagsHl7Attribute">The flag(s) indicating how to validate this HL7 Attribute.</param>
		public ValidationRuleHl7Attribute(Hl7Tag hl7Tag, FlagsHl7Attribute flagsHl7Attribute)
		{
			this.hl7Tag = hl7Tag;
			
			Flags = FlagsConvertor.ConvertToFlagsBase(flagsHl7Attribute);
		}














		internal ValidationRuleHl7Attribute(Hl7Tag hl7Tag, FlagsBase flags)
		{
			this.hl7Tag = hl7Tag;
			
			Flags = flags;
		}



		//
		// - Properties -
		//

        /// <summary>
        /// Gets or sets the HL7 tag.
        /// </summary>
        public Hl7Tag Hl7Tag
		{
			get
			{
				return(this.hl7Tag);
			}
			set
			{
				this.hl7Tag = value;
			}
		}
	}
}
