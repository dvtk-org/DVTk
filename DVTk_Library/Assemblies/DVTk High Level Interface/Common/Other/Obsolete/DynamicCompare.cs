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



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// <br></br>
    /// This class implements a dynamic compare of attribute sets, i.e. (absent) attributes from different
	/// attribute sets with possible different tags are compared with each other.
	/// </summary>
	public class DynamicCompare: CompareBase
	{
		//
		// - Constructors -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Default constructor.
		/// </summary>
		public DynamicCompare()
		{
			// Determine which columns need to be displayed.
			this.displayAttributeName = true;
			this.displayAttributePresent = true;
			this.displayAttributeTag = true;
			this.displayAttributeValues = true;
			this.displayAttributeVR = true;
			this.displayCommonName = false;
			this.displayCommonTag = false;
			this.displayCommonVR = false;
		}



		//
		// - Methods -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Do a dynamic compare for the attribute sets supplied.
		/// 
		/// Note that the parameters attributeSets, attributeSetDescriptions and each validationRuleList instance in
		/// the parameter validationRuleLists must have the same size and have a size at least 2.
		/// </summary>
		/// <param name="tableDescription">Description of the table.</param>
		/// <param name="attributeSets">The attribute sets to compare with each other.</param>
		/// <param name="attributeSetDescriptions">The descriptions of the attribute sets.</param>
		/// <param name="validationRuleLists">
		/// Specifies which attributes with what tags should be compared with each other.
		/// Also specifies how the attributes should be compared with each other.
		/// </param>
		/// <returns>The results of the dynamic compare presented as a table (that may be converted to HTML).</returns>
		public CompareResults CompareAttributeSets(String tableDescription, ArrayList attributeSets, StringCollection attributeSetDescriptions, ArrayList validationRuleLists)
		{
			//
			// Sanity check.
			//

			if (attributeSets.Count < 2)
			{
				throw new System.Exception("Parameter attributeSets supplied to the method StaticCompare.CompareAttributeSets has size smaller than 2.");
			}

			if (attributeSets.Count != attributeSetDescriptions.Count)
			{
				throw new System.Exception("Parameters attributeSets and attributeSetDescriptions supplied to the method StaticCompare.CompareAttributeSets have different size.");
			}

			foreach(ValidationRuleList validationRuleList in validationRuleLists)
			{
				if (attributeSets.Count != validationRuleList.Count)
				{
					throw new System.Exception("Method StaticCompare.CompareAttributeSets: each ValidationRuleList instance present in the parameter validationRuleLists must have the same size as the parameter attributeSets.");
				}
			}


			//
			// Do the actual compare.
			//
			
			// Start with a new empty table.
			this.compareResults = null; 

			// Determine the index of the different columns in the table.
			int numberOfColumns = DetermineColumnIndices(attributeSets.Count);

			// To be able to compare correctly, we first make sure the attributes in all attributesets are ascending.
			foreach (AttributeSet attributeSet in attributeSets)
			{
				attributeSet.MakeAscending(true);
			}
			
			// Determine the table headers and comlumn widths.
			this.compareResults = CreateCompareResults(numberOfColumns, attributeSets.Count, tableDescription, attributeSetDescriptions);

			// Do the actual comparison.
			AddAttributeSetsInformationUsingDynamicCompare(attributeSets, validationRuleLists);

			if (this.differenceFound)
			{
				this.compareResults.DifferencesCount++;
				this.differenceFound = false;
			}

			return(this.compareResults);
		}
	}
}
