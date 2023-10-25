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

using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// When supplied with attribute sets (Dicom and/or HL7) and validation rules,
	/// generates AttributeLists.
	/// </summary>
	internal class GeneratorDynamicCompare
	{
		//
		// - Fields -
		//

		/// <summary>
		/// All the AttributeCollections on which the CcompareRules will be applied.
		/// </summary>		
		private AttributeCollections attributeCollections = null;
		
		/// <summary>
		/// All the CompareRules that will be applied to the attributeCollections.
		/// </summary>
		private CompareRules compareRules = null;

		/// <summary>
		/// Index of current CompareRule.
		/// </summary>
		private int compareRulesIndex = 0;
		


		//
		// - Constructors -
		//
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="compareRules">All the CcompareRules that will be applied to the attributeCollections.</param>
		/// <param name="attributeCollections">All the AttributeCollections on which the CcompareRules will be applied.</param>
		/// 


		public GeneratorDynamicCompare(AttributeCollections attributeCollections, CompareRules compareRules)
		{
			this.compareRules = compareRules;
			this.attributeCollections = attributeCollections;
		}



		/// <summary>
		/// Get the next list of attributes to compare to each other.
		/// If an attribute is not present, a null pointer is returned in the AttributeCollection.
		/// If all attributes have been compared, null is returned.
		/// </summary>
		/// <returns>The attributes to compare.</returns>



		public AttributeList GetNextAttributes()
		{
			AttributeList nextAttributes = null;

			if (this.compareRulesIndex >= this.compareRules.Count)
			{
				nextAttributes = null;
			}
			else
			{
				nextAttributes = new AttributeList();

				CompareRule compareRule = this.compareRules[this.compareRulesIndex] as CompareRule;

				nextAttributes.CompareRule = compareRule;

				// Use the attributeCollectionsIndex to iterate through both the attributeCollections and compareRule.
				for (int validationRuleListIndex = 0; validationRuleListIndex < compareRule.Count; validationRuleListIndex++)
				{
					ValidationRuleBase validationRule = compareRule[validationRuleListIndex];

					if (validationRule is ValidationRuleDicomAttribute)
					{
						ValidationRuleDicomAttribute validationRuleDicomAttribute = validationRule as ValidationRuleDicomAttribute;
						DicomAttributeCollection dicomAttributeCollection = this.attributeCollections[validationRuleListIndex] as DicomAttributeCollection;

						DicomAttribute dicomAttribute = null;

						if (validationRuleDicomAttribute == null)
							// If nothing needs to be validated.
						{
							dicomAttribute = null;
						}
						else
						{
							DvtkHighLevelInterface.Dicom.Other.Attribute dicomAttributeOnly = null;

							if (dicomAttributeCollection.AttributeSetOnly.Exists(validationRuleDicomAttribute.TagSequenceString))
							{
								dicomAttributeOnly = dicomAttributeCollection.AttributeSetOnly[validationRuleDicomAttribute.TagSequenceString];
							}
							else
							{
								dicomAttributeOnly = new InvalidAttribute();
							}

							// Merge the validation flags from the validation rule and the attribute collection.
							ValidationRuleDicomAttribute mergedValidationRuleDicomAttribute = new ValidationRuleDicomAttribute(validationRuleDicomAttribute.TagSequenceString, validationRuleDicomAttribute.Flags | dicomAttributeCollection.Flags);

							dicomAttribute = new DicomAttribute(dicomAttributeOnly, mergedValidationRuleDicomAttribute);

							dicomAttribute.DisplayFullTagSequence = true;
						}

						nextAttributes.Add(dicomAttribute);
					}
					else if (validationRule is ValidationRuleHl7Attribute)
					{
						ValidationRuleHl7Attribute validationRuleHl7Attribute = validationRule as ValidationRuleHl7Attribute;
						Hl7AttributeCollection hl7AttributeCollection = this.attributeCollections[validationRuleListIndex] as Hl7AttributeCollection;

						Hl7Attribute hl7Attribute = null;

						if (validationRuleHl7Attribute == null)
							// If nothing needs to be validated.
						{
							hl7Attribute = null;
						}
						else
						{
							// Merge the validation flags from the validation rule and the attribute collection.
							ValidationRuleHl7Attribute mergedValidationRuleHl7Attribute = new ValidationRuleHl7Attribute(validationRuleHl7Attribute.Hl7Tag, validationRuleHl7Attribute.Flags | hl7AttributeCollection.Flags);

							hl7Attribute = new Hl7Attribute(hl7AttributeCollection.Hl7MessageOnly.Value(validationRuleHl7Attribute.Hl7Tag), mergedValidationRuleHl7Attribute);
						}

						nextAttributes.Add(hl7Attribute);
					}
					else
					{
						nextAttributes.Add(null);
					}	
				}

				this.compareRulesIndex++;
			}

			return(nextAttributes);
		}
	}
}
