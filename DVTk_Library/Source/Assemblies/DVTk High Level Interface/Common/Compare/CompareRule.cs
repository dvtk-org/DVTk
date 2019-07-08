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
using System.IO;
using System.Xml.Serialization;

using DvtkHighLevelInterface.Common.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Contains one or more ValidationRule instances. Specifies how a list of attributes
	/// (Dicom and/or HL7) should be validated together.
	/// </summary>
	[XmlInclude(typeof(ValidationRuleDicomAttribute)), XmlInclude(typeof(ValidationRuleHl7Attribute))]
	public class CompareRule
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property ManualCondition.
		/// </summary>
		private String conditionText = "";

		/// <summary>
		/// Internal list that stores the ordered list of validation rules.
		/// </summary>
		private ArrayList validationRules = new ArrayList();

		/// <summary>
		/// See property ValueType.
		/// </summary>
		private CompareValueTypes compareValueType = CompareValueTypes.Identical;



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		public CompareRule()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="validationRules">The ordered list of validation rules.</param>
		public CompareRule(params ValidationRuleBase[] validationRules)
		{
			this.validationRules.AddRange(validationRules);
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Number of validation rules.
		/// </summary>
		public int Count
		{
			get
			{
				return(this.validationRules.Count);
			}
		}

        /// <summary>
        /// Gets or sets the condition text.
        /// </summary>
		public String ConditionText
		{
			get
			{
				return(this.conditionText);
			}
			set
			{
				this.conditionText = value;
			}
		}

		/// <summary>
		/// Property to get a specific validation rule.
		/// </summary>
		public ValidationRuleBase this[int zeroBasedIndex]
		{
			get
			{
				if ((zeroBasedIndex < 0) || (zeroBasedIndex >= Count))
				{
					throw new HliException("Wrong index used for CompareRule.");
				}
				
				return(this.validationRules[zeroBasedIndex] as ValidationRuleBase);
			}
		}

        /// <summary>
        /// Gets or sets the compare value type.
        /// </summary>
		public CompareValueTypes CompareValueType
		{
			get
			{
				return(this.compareValueType);
			}
			set
			{
				this.compareValueType = value;
			}
		}

        /// <summary>
        /// Adds a validation rule.
        /// </summary>
        /// <param name="validationRule">The validation rule.</param>
		public void Add(ValidationRuleBase validationRule)
		{
			this.validationRules.Add(validationRule);
		}

        /// <summary>
        /// Gets or sets the array representation of this instance (used for serialisation from/to xml).
        /// </summary>
		public ValidationRuleBase[] ArrayRepresentation
		{
			get
			{
				return((ValidationRuleBase[])this.validationRules.ToArray(typeof(ValidationRuleBase)));
			}
			set
			{
				this.validationRules.AddRange(value);
			}
		}

        /// <summary>
        /// Removes a validation rule at a specified index.
        /// </summary>
        /// <param name="index">The index.</param>
		internal void RemoveAt(int index)
		{
			this.validationRules.RemoveAt(index);
		}

        /// <summary>
        /// Clone this CompareRule instance.
        /// </summary>
        /// <returns>The cloned CompareRule instance.</returns>
		internal CompareRule Clone()
		{
			CompareRule clonedCompareRule = new CompareRule();
			clonedCompareRule.validationRules = this.validationRules.Clone() as ArrayList;

			return(clonedCompareRule);
		}
	}
}
