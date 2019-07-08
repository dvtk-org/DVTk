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
	/// Summary description for Hl7Attribute.
	/// </summary>
	internal class Hl7Attribute: AttributeBase
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property AttributeOnly.
		/// </summary>
		private String attributeOnly = null;

		/// <summary>
		/// See property ValidationRuleDicomAttribute.
		/// </summary>
		private ValidationRuleHl7Attribute validationRuleHl7Attribute = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private Hl7Attribute()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="attributeOnly">The attribute without the validation rule.</param>
        /// <param name="validationRuleHl7ttribute">The validation rule that needs to be applied to the attribute.</param>
		public Hl7Attribute(String attributeOnly, ValidationRuleHl7Attribute validationRuleHl7ttribute): base(validationRuleHl7ttribute)
		{
			this.attributeOnly = attributeOnly;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Property to get the attribute without the validation rule.
		/// For now, only the String representing the value is stored.
		/// An empty String means the absence of a HL7 attribute.
		/// </summary>
		internal String AttributeOnly
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

		/// <summary>
		/// Indicates if the actual attribute is present.
		/// </summary>
		public override bool IsPresent
		{
			get
			{
				return(this.attributeOnly.Length > 0);
			}
		}

		/// <summary>
		/// Property to get the validation rule that needs to be applied to the attribute.
		/// </summary>
		internal ValidationRuleHl7Attribute ValidationRuleHl7Attribute
		{
			get
			{
				return(ValidationRule as ValidationRuleHl7Attribute);
			}
			set
			{
				this.validationRuleHl7Attribute = value;
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
			return("\"" + this.attributeOnly + "\"");
		}
	}
}
