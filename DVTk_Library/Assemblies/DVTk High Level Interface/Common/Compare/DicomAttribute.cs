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

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// In the context of this namespace,
	/// class representing an actual Dicom attribute to validate in combination with
	/// its validation flags.
	/// </summary>
	internal class DicomAttribute: AttributeBase
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property AttributeOnly.
		/// </summary>
		private DvtkHighLevelInterface.Dicom.Other.Attribute attributeOnly = null;

		private bool displayFullTagSequence = false;

		/// <summary>
		/// See property ValidationRuleDicomAttribute.
		/// </summary>
		private ValidationRuleDicomAttribute validationRuleDicomAttribute = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private DicomAttribute()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="attributeOnly">The attribute without the validation rule.</param>
		/// <param name="validationRuleDicomAttribute">The validation rule that needs to be applied to the attribute.</param>
		public DicomAttribute(DvtkHighLevelInterface.Dicom.Other.Attribute attributeOnly, ValidationRuleDicomAttribute validationRuleDicomAttribute): base(validationRuleDicomAttribute)
		{
			this.attributeOnly = attributeOnly;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Property to get the attribute without the validation rule.
		/// </summary>
		internal DvtkHighLevelInterface.Dicom.Other.Attribute AttributeOnly
		{
			get
			{
				return(this.attributeOnly);
			}
			set
			{
				this.attributeOnly = value;
			}
		}

		internal bool DisplayFullTagSequence
		{
			get
			{
				return(this.displayFullTagSequence);
			}
			set
			{
				this.displayFullTagSequence = value;
			}
		}

		/// <summary>
		/// Indicates if the actual attribute is present.
		/// </summary>
		public override bool IsPresent
		{
			get
			{
				bool isPresent = false;

				if (this.attributeOnly is ValidAttribute)
				{
					isPresent = true;
				}
				
				return(isPresent);
			}
		}

		/// <summary>
		/// Property to get the validation rule that needs to be applied to the attribute.
		/// </summary>
		internal ValidationRuleDicomAttribute ValidationRuleDicomAttribute
		{
			get
			{
				return(ValidationRule as ValidationRuleDicomAttribute);
			}
			set
			{
				this.validationRuleDicomAttribute = value;
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Converts the values of the actual attribute to a string representation.
		/// </summary>
		/// <returns>The string representation of the actual attribute.</returns>
		public override String ValuesToString()
		{
			return(this.attributeOnly.Values.ToString());
		}
	}
}
