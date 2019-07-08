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
using System.Collections.Specialized;

using Attribute = DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// This class implements a dynamic compare of attribute sets, i.e. (absent) attributes from different
	/// attribute sets with possible different tags are compared with each other.
	/// </summary>
	public class DynamicCompare: CompareBase
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DynamicCompare()
		{
			// Determine which columns need to be displayed.
			DisplayAttributeName = true;
			DisplayAttributePresent = true;
			DisplayAttributeTag = true;
			DisplayAttributeValues = true;
			DisplayAttributeVR = true;
			DisplayCommonName = false;
			DisplayCommonTag = false;
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Do a dynamic compare for the attribute sets supplied.
		/// 
		/// Note that the parameters attributeSets, attributeSetDescriptions and each compareRule instance in
		/// the parameter compareRules must have the same size and have a size at least 2.
		/// </summary>
		/// <param name="tableDescription">Description of the table.</param>
        /// <param name="attributeCollections">The attribute sets to compare with each other.</param>
        /// <param name="attributeCollectionDescriptions">The descriptions of the attribute sets.</param>
		/// <param name="compareRules">
		/// Specifies which attributes with what tags should be compared with each other.
		/// Also specifies how the attributes should be compared with each other.
		/// </param>
		/// <returns>The results of the dynamic compare presented as a table (that may be converted to HTML).</returns>
		public CompareResults CompareAttributeCollections(String tableDescription, AttributeCollections attributeCollections, StringCollection attributeCollectionDescriptions, CompareRules compareRules)
		{
			//
			// Sanity check. 
			//

			if (attributeCollections.Count < 2)
			{
				throw new System.Exception("Parameter attributeSets supplied to the method StaticCompare.CompareAttributeSets has size smaller than 2.");
			}

			if (attributeCollections.Count != attributeCollectionDescriptions.Count)
			{
				throw new System.Exception("Parameters attributeSets and attributeSetDescriptions supplied to the method StaticCompare.CompareAttributeSets have different size.");
			}

			for (int index = 0; index < compareRules.Count; index++)
			{
				CompareRule compareRule = compareRules[index];

				if (attributeCollections.Count != compareRule.Count)
				{
					throw new System.Exception("Method StaticCompare.CompareAttributeSets: each CompareRule instance present in the parameter compareRules must have the same size as the parameter attributeCollections.");
				}
			}



			// Todo change taking the possibility of null into account for the attribute collections.

			//
			// Remove the attribute collections that are null and adjust the compare rules.
			//

			for (int index = attributeCollections.Count - 1; index >= 0; index--)
			{
				AttributeCollectionBase attributeCollection = attributeCollections[index];

				if (attributeCollection == null)
				{
					// Clone the collections because we don't want to change the supplied collections.
					attributeCollections = attributeCollections.Clone();
					attributeCollections.RemoveAt(index);

					StringCollection newAttributeCollectionDescriptions = new StringCollection();
					foreach(String description in attributeCollectionDescriptions)
					{
						newAttributeCollectionDescriptions.Add(description);
					}
					attributeCollectionDescriptions = newAttributeCollectionDescriptions;
					attributeCollectionDescriptions.RemoveAt(index);

#pragma warning disable 0618
                    CompareRules newCompareRules = new CompareRules();
#pragma warning restore 0618

                    for (int compareRuleIndex = 0; compareRuleIndex < compareRules.Count; compareRuleIndex++)
					{
						CompareRule compareRule = compareRules[compareRuleIndex].Clone();
						compareRule.RemoveAt(index);
						newCompareRules.Add(compareRule);
					}

					compareRules = newCompareRules;
				}
			}



			//
			// Do the actual compare.
			//
			
			this.compareResults = null; 

			// Determine the index of the different columns in the table.
			int numberOfColumns = DetermineColumnIndices(attributeCollections);

			// To be able to compare correctly, we first make sure the attributes in all DicomAttributeCollections are ascending.
			attributeCollections.DicomMakeAscending();
			
			// Determine the table headers and comlumn widths.
			this.compareResults = CreateCompareResults(numberOfColumns, attributeCollections, tableDescription, attributeCollectionDescriptions);

			// Do the actual comparison.
			AddAttributeCollectionsInformationUsingDynamicCompare(attributeCollections, compareRules);

			if (this.differenceFound)
			{
				this.compareResults.DifferencesCount++;
				this.differenceFound = false;
			}

			return(this.compareResults);
		}
	}
}
