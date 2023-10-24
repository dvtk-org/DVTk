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



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// AttributeSets to compare with each other using a static compare.
	/// An instance of this class helps in determine which attributes need to be compared to each other.
	/// </summary>
	internal class GeneratorStaticDicomCompare
	{
		//
		// - Fields -
		//

		/// <summary>
		/// Array of attribute sets to compare with each other.
		/// </summary>
		private AttributeCollections attributeCollections = null;

		/// <summary>
		/// Indicates for each attribute set the index of the current attribute.
		/// </summary>
		private ArrayList currentAttributeIndices = new ArrayList();



		//
		// - Constructors -
		//

		/// <summary>
		/// Hide default constructor.
		/// </summary>
		private GeneratorStaticDicomCompare()
		{
			// Do nothing.
		}

		/// <summary>
		/// Constructor.
		/// </summary>
        /// <param name="attributeCollections">The attribute sets to compare.</param>
		public GeneratorStaticDicomCompare(AttributeCollections attributeCollections)
		{
			this.attributeCollections = attributeCollections;

			for (int index = 0; index < attributeCollections.Count; index++)
			{
				this.currentAttributeIndices.Add(0);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Get the next collection of attributes to compare to each other.
		/// If an attribute is not present, a null pointer is returned in the AttributeCollection.
		/// If all attributes have been compared, null is returned.
		/// </summary>
		/// <returns>The attributes to validate.</returns>
		/// 







		private AttributeList DetermineNextAttributes(TagSequence lowestTagSequence)
		{
			AttributeList nextAttributes = new AttributeList();

			nextAttributes.CompareRule = new CompareRule();

			for (int index = 0; index < this.attributeCollections.Count; index++)
			{
				int attributeSetIndex = (int)this.currentAttributeIndices[index];
				AttributeCollectionBase attributeCollectionBase = this.attributeCollections[index];

				if (attributeCollectionBase == null)
				{
					nextAttributes.Add(null);
					nextAttributes.CompareRule.Add(null);
				}
				else if (attributeCollectionBase is Hl7AttributeCollection)
				{
					nextAttributes.Add(null);
					nextAttributes.CompareRule.Add(null);
				}
				else if (attributeCollectionBase is DicomAttributeCollection)
				{
					DicomAttributeCollection dicomAttributeCollection = attributeCollectionBase as DicomAttributeCollection;

					if (attributeSetIndex < dicomAttributeCollection.AttributeSetOnly.Count)
						// Still unprocessed attributes left in this dataset.
					{
						ValidAttribute attributeOnly = dicomAttributeCollection.AttributeSetOnly[attributeSetIndex] as ValidAttribute;

						if (attributeOnly.TagSequence.LastTag.AsUInt32 == lowestTagSequence.LastTag.AsUInt32)
						{
							// Add entry with existing attribute.
							ValidationRuleDicomAttribute validationRuleDicomAttribute = new ValidationRuleDicomAttribute(lowestTagSequence.ToString(), dicomAttributeCollection.Flags);
							nextAttributes.CompareRule.Add(validationRuleDicomAttribute);
							DicomAttribute dicomAttribute = new DicomAttribute(attributeOnly, validationRuleDicomAttribute);
							nextAttributes.Add(dicomAttribute);

							// This attribute will be returned. Increase the attribute index.
							this.currentAttributeIndices[index] = attributeSetIndex + 1;
						}
						else
						{
							// Add entry with non-existing attribute.
							ValidationRuleDicomAttribute validationRuleDicomAttribute = new ValidationRuleDicomAttribute(lowestTagSequence.ToString(), dicomAttributeCollection.Flags);
							DicomAttribute dicomAttribute = new DicomAttribute(new DvtkHighLevelInterface.Dicom.Other.InvalidAttribute(), validationRuleDicomAttribute);
							nextAttributes.Add(dicomAttribute);
						}
					}
					else
						// No more unprocessed attributes left in this dataset.
					{
						// Add entry with non-existing attribute.
						ValidationRuleDicomAttribute validationRuleDicomAttribute = new ValidationRuleDicomAttribute(lowestTagSequence.ToString(), dicomAttributeCollection.Flags);
						DicomAttribute dicomAttribute = new DicomAttribute(new DvtkHighLevelInterface.Dicom.Other.InvalidAttribute(), validationRuleDicomAttribute);
						nextAttributes.Add(dicomAttribute);
					}
				}
			}				

			return(nextAttributes);
		}







		private TagSequence DetermineLowestTagSequence()
		{
			TagSequence lowestTagSequence = null; // Lowest in this context means lowest LastTag.

			//
			// Determine what the lowest tag is.
			// If no more attribute exists anymore to process, this tag will be null.
			//

			for (int index = 0; index < this.attributeCollections.Count; index++)
			{
				int attributeSetIndex = (int)this.currentAttributeIndices[index];
				AttributeCollectionBase attributeCollectionBase = this.attributeCollections[index];

				if (attributeCollectionBase is DicomAttributeCollection)
				{
					DicomAttributeCollection dicomAttributeCollection = attributeCollectionBase as DicomAttributeCollection;

					if (attributeSetIndex < dicomAttributeCollection.AttributeSetOnly.Count)
						// Still unprocessed attributes left in this dataset.
					{
						ValidAttribute attributeOnly = dicomAttributeCollection.AttributeSetOnly[attributeSetIndex] as ValidAttribute;

						if (lowestTagSequence == null)
						{
							lowestTagSequence = attributeOnly.TagSequence;
						}
						else
						{
							if (attributeOnly.TagSequence.LastTag.AsUInt32 < lowestTagSequence.LastTag.AsUInt32)
							{
								lowestTagSequence = attributeOnly.TagSequence;
							}
						}
					}
				}
			}

			return(lowestTagSequence);
		}


		public AttributeList GetNextAttributes(out TagSequence lowestTagSequence)
		{
			AttributeList nextAttributes = null;
			
			lowestTagSequence = DetermineLowestTagSequence();
	

			//
			// If unprocessed attributes still exist, create and fill the AttributeList to return.
			//

			if (lowestTagSequence != null)
				// Still attributes to process.
			{
				nextAttributes = DetermineNextAttributes(lowestTagSequence);
			}
			else
				// No more attributes left to process.
			{
				nextAttributes = null;
			}
				
			return(nextAttributes);
		}
	}
}
