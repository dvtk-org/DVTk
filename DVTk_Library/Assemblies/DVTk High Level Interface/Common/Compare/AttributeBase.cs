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
	/// In the context of this namespace,
	/// abstract class representing an actual attribute to validate in combination with
	/// its validation flags.
	/// </summary>
	internal abstract class AttributeBase
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property ValidationRule.
		/// </summary>
		private ValidationRuleBase validationRule = null;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AttributeBase()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="validationRule">The validation rule.</param>
		public AttributeBase(ValidationRuleBase validationRule)
		{
			this.validationRule = validationRule;
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Indicates if the actual attribute is present.
		/// </summary>
		public abstract bool IsPresent
		{
			get;
		}

		/// <summary>
		/// Property to get the validation rule.
		/// </summary>
		public ValidationRuleBase ValidationRule
		{
			get
			{
				return(this.validationRule);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Converts the values of the actual attribute to a string representation.
		/// </summary>
		/// <returns>The string representation of the actual attribute.</returns>
		public abstract String ValuesToString();
	}
}
