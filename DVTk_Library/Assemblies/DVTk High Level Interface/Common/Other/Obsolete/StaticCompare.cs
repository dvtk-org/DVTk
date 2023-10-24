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

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// <br></br>
    /// This class implements a static compare of attribute sets, i.e. (absent) attributes from different
	/// attribute sets with the same tags are compared with each other.
	/// </summary>
	public class StaticCompare: CompareBase
	{
		//
		// - Constructors -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Default constructor.
		/// </summary>
		public StaticCompare()
		{
			// Determine which columns need to be displayed.
			this.displayAttributeName = false;
			this.displayAttributePresent = true;
			this.displayAttributeTag = false;
			this.displayAttributeValues = true;
			this.displayAttributeVR = true;
			this.displayCommonName = true;
			this.displayCommonTag = true;
			this.displayCommonVR = false;
		}



		//
		// - Methods -
		//
		
		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Do a static compare for the attribute sets supplied.
		/// 
		/// Note that the parameters attributeSets and attributeSetDescriptions must have the same size and
		/// must be at least size 2.
		/// </summary>
		/// <param name="tableDescription">Description of the table.</param>
		/// <param name="attributeSets">The attribute sets to compare with each other.</param>
		/// <param name="attributeSetDescriptions">The descriptions of the attribute sets.</param>
		/// <param name="compareFlags">
		/// The compare flags that may be supplied. The following combination of flags may be supplied (using bitwise Or):
		/// - CompareFlags.None: when only supplying this flag, the attributes are only displayed and no compare is performed.
		/// - CompareFlags.Compare_present: a check is performed if all attributes with the same tag are present.
		/// - CompareFlags.Compare_values: a check is performed if all attributes with the same tag have the same values.
		/// - CompareFlags.Compare_VR: a check is performed if all attributes with the same tag have the same VR.
		/// 
		/// The compare flags are applied to all supplied attribute sets.
		/// </param>
		/// <returns>The results of the static compare presented as a table (that may be converted to HTML).</returns>
		public CompareResults CompareAttributeSets(String tableDescription, ArrayList attributeSets, StringCollection attributeSetDescriptions, CompareFlags compareFlags)
		{
			ArrayList compareFlagsForAttributeSets = new ArrayList();

			// Use the supplied compare flags for all supplied attribute sets.
			for (int index = 0; index < attributeSets.Count; index++)
			{
				compareFlagsForAttributeSets.Add(compareFlags);
			}

			// Do a sanity check for the supplied parameters and do the actual compare.
			return(CompareAttributeSets(tableDescription, attributeSets, attributeSetDescriptions, compareFlagsForAttributeSets));
		}
		
		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Do a static compare for the attribute sets supplied.
		/// 
		/// Note that the parameters attributeSets, attributeSetDescriptions and compareFlagsForAttributeSets
		/// must have the same size and must be at least size 2.
		/// </summary>
		/// <param name="tableDescription">Description of the table.</param>
		/// <param name="attributeSets">The attribute sets to compare with each other.</param>
		/// <param name="attributeSetDescriptions">The descriptions of the attribute sets.</param>
		/// <param name="compareFlagsForAttributeSets">
		/// The compare flags that may be supplied. The following combination of flags may be supplied (using bitwise Or):
		/// - CompareFlags.None: when only supplying this flag, the attributes are only displayed and no compare is performed.
		/// - CompareFlags.Compare_present: a check is performed if all attributes with the same tag are present.
		/// - CompareFlags.Compare_values: a check is performed if all attributes with the same tag have the same values.
		/// - CompareFlags.Compare_VR: a check is performed if all attributes with the same tag have the same VR.
		/// </param>
		/// <returns>The results of the static compare presented as a table (that may be converted to HTML).</returns>
		private CompareResults CompareAttributeSets(String tableDescription, ArrayList attributeSets, StringCollection attributeSetDescriptions, ArrayList compareFlagsForAttributeSets)
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

			if (attributeSets.Count != compareFlagsForAttributeSets.Count)
			{
				throw new System.Exception("Parameters attributeSets and compareFlagsForAttributeSets supplied to the method StaticCompare.CompareAttributeSets have different size.");
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

			ArrayList dicomAttributeFlagsForAttributeSets = new ArrayList();
			ArrayList displayAttributeSets = new ArrayList();

			for (int index = 0; index < attributeSets.Count; index++)
			{
				dicomAttributeFlagsForAttributeSets.Add(DicomAttributeFlags.Include_sequence_items);
				displayAttributeSets.Add(true);
			}

			// Do the actual comparison.
			AddAttributeSetsInformationUsingStaticCompare(attributeSets, compareFlagsForAttributeSets, dicomAttributeFlagsForAttributeSets, displayAttributeSets);

			if (this.differenceFound)
			{
				this.compareResults.DifferencesCount++;
				this.differenceFound = false;
			}

			return(this.compareResults);
		}
	}
}
