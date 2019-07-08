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

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
	/// </summary>
	internal class GeneratorDynamicCompare
	{
		/// <summary>
		/// Array of attribute sets to get the attributes from to compare with each other.
		/// </summary>
		private ArrayList attributeSets;

		private ArrayList validationRuleLists;

		private int validationRuleListsIndex = 0;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        /// <param name="attributeSets">-</param>
        /// <param name="validationRuleLists">-</param>
		public GeneratorDynamicCompare(ArrayList attributeSets, ArrayList validationRuleLists)
		{
			this.attributeSets = attributeSets;
			this.validationRuleLists = validationRuleLists;
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Get the next collection of attributes to compare to each other.
		/// If an attribute is not present, a null pointer is returned in the AttributeCollection.
		/// If all attributes have been compared, null is returned.
		/// </summary>
		/// <returns>The attributes to compare.</returns>
		public DicomAttributesToValidate GetNextAttributes()
		{
			DicomAttributesToValidate dicomAttributesToValidate = null;

			if (this.validationRuleListsIndex >= validationRuleLists.Count)
			{
				dicomAttributesToValidate = null;
			}
			else
			{
				dicomAttributesToValidate = new DicomAttributesToValidate();

				ValidationRuleList validationRuleList = this.validationRuleLists[this.validationRuleListsIndex] as ValidationRuleList;

				for (int attributeSetsIndex = 0; attributeSetsIndex < this.attributeSets.Count; attributeSetsIndex++)
				{
					DicomAttributeToValidate dicomAttributeToValidate = new DicomAttributeToValidate();
					AttributeSet attributeSet = this.attributeSets[attributeSetsIndex] as AttributeSet;
					ValidationRuleDicomAttribute validationRuleDicomAttribute = validationRuleList[attributeSetsIndex] as ValidationRuleDicomAttribute;

					if (validationRuleDicomAttribute.TagSequence != "")
					{
						dicomAttributeToValidate.Attribute = attributeSet[validationRuleDicomAttribute.TagSequence];
					}
					dicomAttributeToValidate.ValidationRuleDicomAttribute = validationRuleDicomAttribute;
					dicomAttributeToValidate.ValidationRuleDicomAttribute.DisplayFullTagSequence = true;

					dicomAttributesToValidate.Add(dicomAttributeToValidate);
				}

				this.validationRuleListsIndex++;
			}

			return(dicomAttributesToValidate);
		}
	}
}
