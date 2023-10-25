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



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// Summary description for StaticDicomCompare.
	/// </summary>
	public class StaticDicomCompare: CompareBase
	{
		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public StaticDicomCompare()
		{
			// Determine which columns need to be displayed.
			DisplayAttributeName = false;
			DisplayAttributePresent = true;
			DisplayAttributeTag = false;
			DisplayAttributeValues = true;
			DisplayAttributeVR = true;
			DisplayCommonName = true;
			DisplayCommonTag = true;
		}



		//
		// - Properties -
		//




		//
		// - Methods -
		//
		







		/// <summary>
		/// Do a static compare for the attribute sets supplied.
		/// 
		/// Note that the parameters attributeSets, attributeSetDescriptions and compareFlagsForAttributeSets
		/// must have the same size and must be at least size 2.
		/// </summary>
		/// <param name="tableDescription">Description of the table.</param>
        /// <param name="attributeCollections">The attribute sets to compare with each other.</param>
        /// <param name="attributeCollectionDescriptions">The descriptions of the attribute sets.</param>
        /// <param name="flags">
		/// The compare flags that may be supplied. The following combination of flags may be supplied (using bitwise Or):
		/// - CompareFlags.None: when only supplying this flag, the attributes are only displayed and no compare is performed.
		/// - CompareFlags.Compare_present: a check is performed if all attributes with the same tag are present.
		/// - CompareFlags.Compare_values: a check is performed if all attributes with the same tag have the same values.
		/// - CompareFlags.Compare_VR: a check is performed if all attributes with the same tag have the same VR.
		/// </param>
		/// <returns>The results of the static compare presented as a table (that may be converted to HTML).</returns>
		public CompareResults CompareAttributeSets(String tableDescription, AttributeCollections attributeCollections, StringCollection attributeCollectionDescriptions, FlagsDicomAttribute flags)
		{
			CompareResults compareResults = null;

			for (int index = 0; index < attributeCollections.Count; index++)
			{
				(attributeCollections[index] as DicomAttributeCollection).Flags |= FlagsConvertor.ConvertToFlagsBase(flags);
			}

			compareResults = CompareAttributeSets(tableDescription, attributeCollections, attributeCollectionDescriptions);

			return(compareResults);
		}


        /// <summary>
        /// Compare the supplied attribute sets.
        /// </summary>
        /// <param name="tableDescription">The table description.</param>
        /// <param name="attributeCollections">The attribute collections.</param>
        /// <param name="attributeCollectionDescriptions">The description of the attribute collections.</param>
        /// <returns></returns>
		public CompareResults CompareAttributeSets(String tableDescription, AttributeCollections attributeCollections, StringCollection attributeCollectionDescriptions)
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


			//
			// Do the actual compare.
			//

			this.compareResults = null; 

			// Determine the index of the different columns in the table.
			int numberOfColumns = DetermineColumnIndices(attributeCollections);

			// To be able to compare correctly, we first make sure the attributes in all attributesets are ascending.
			attributeCollections.DicomMakeAscending();
			
			// Determine the table headers and comlumn widths.
			this.compareResults = CreateCompareResults(numberOfColumns, attributeCollections, tableDescription, attributeCollectionDescriptions);

			ArrayList dicomAttributeFlagsForAttributeSets = new ArrayList();
			ArrayList displayAttributeSets = new ArrayList();

			// Do the actual comparison.
			AddAttributeCollectionsInformationUsingStaticDicomCompare(attributeCollections);

			if (this.differenceFound)
			{
				this.compareResults.DifferencesCount++;
				this.differenceFound = false;
			}

			return(this.compareResults);
		}

	}
}
