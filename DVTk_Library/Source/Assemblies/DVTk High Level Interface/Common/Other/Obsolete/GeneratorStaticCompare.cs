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

using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// <br></br>
    /// AttributeSets to compare with each other using a static compare.
	/// An instance of this class helps in determine which attributes need to be compared to each other.
	/// </summary>
	internal class GeneratorStaticCompare
	{
		//
		// - Fields -
		//

		/// <summary>
		/// Array of attribute sets to compare with each other.
		/// </summary>
		private ArrayList attributeSets;

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
		private GeneratorStaticCompare()
		{
			// Do nothing.
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Constructor.
		/// </summary>
		/// <param name="attributeSets">The attribute sets to compare.</param>
		public GeneratorStaticCompare(ArrayList attributeSets)
		{
			// Sanity check.
			if (attributeSets.Count < 2)
			{
				throw new System.Exception("Number of supplied attribute sets must be at least 2.");
			}

			this.attributeSets = attributeSets;

			for (int index = 0; index < attributeSets.Count; index++)
			{
				this.currentAttributeIndices.Add(0);
			}
		}



		//
		// - Methods -
		//

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        /// <param name="compareFlagsForAttributeSets">-</param>
        /// <param name="dicomAttributeFlagsForAttributeSets">-</param>
        /// <param name="displayAttributeSets">-</param>
        /// <returns>-</returns>
        public DicomAttributesToValidate GetNextAttributes(ArrayList compareFlagsForAttributeSets, ArrayList dicomAttributeFlagsForAttributeSets, ArrayList displayAttributeSets)
		{
			DicomAttributesToValidate dicomAttributesToValidate = new DicomAttributesToValidate();
			Tag lowestTag = null;
			String lowestTagSequenceAsString = "";

			// Make first collection of attributes, containing ValidAttribute instances (if an
			// attribute set still contains unprocessed attributes) and InvalidAttribute 
			// instances (if no more attributes are left in an attribute set).
			//
			// At the same time, determine what the lowest tag is.
			for (int index = 0; index < this.attributeSets.Count; index++)
			{
				int attributeSetIndex = (int)this.currentAttributeIndices[index];
				AttributeSet attributeSet = this.attributeSets[index] as AttributeSet;

				if (attributeSetIndex < attributeSet.Count)
					// Still unprocessed attributes left in this dataset.
				{
					ValidAttribute attribute = attributeSet[attributeSetIndex] as ValidAttribute;

					DicomAttributeToValidate dicomAttributeToValidate = new DicomAttributeToValidate();
					dicomAttributeToValidate.Attribute = attribute;
					dicomAttributesToValidate.Add(dicomAttributeToValidate);

					TagSequence tagSequence = attribute.TagSequence;
					Tag lastTag = tagSequence.Tags[tagSequence.Tags.Count - 1] as Tag;

					if (lowestTag == null)
					{
						lowestTag = lastTag;
						lowestTagSequenceAsString = tagSequence.ToString();
					}
					else
					{
						if (lastTag.AsUInt32 < lowestTag.AsUInt32)
						{
							lowestTag = lastTag;
							lowestTagSequenceAsString = tagSequence.ToString();
						}
					}
				}
				else
					// No more unprocessed attributes left in this dataset.
				{
					dicomAttributesToValidate.Add(new DicomAttributeToValidate());
				}
			}

			if (lowestTag != null)
			{
				// Now the lowest tag is known.
				// Make all ValidAttributes present that have a tag higher then the lowest tag InvalidAttributes.
				for (int index = 0; index < dicomAttributesToValidate.Count; index++)
				{
					if (((DicomAttributeToValidate)dicomAttributesToValidate[index]).Attribute is ValidAttribute)
					{
						ValidAttribute validAttribute = ((DicomAttributeToValidate)dicomAttributesToValidate[index]).Attribute as ValidAttribute;
						TagSequence tagSequence = validAttribute.TagSequence;
						Tag lastTag = tagSequence.Tags[tagSequence.Tags.Count - 1] as Tag;

						if (lastTag.AsUInt32 > lowestTag.AsUInt32)
						{
							dicomAttributesToValidate[index] = new DicomAttributeToValidate();
						}
						else
						{
							// This attribute will be returned. Increase the attribute index.
							int newAttributeIndex = (int)(this.currentAttributeIndices[index]);
							newAttributeIndex++;
							this.currentAttributeIndices[index] = newAttributeIndex;
						}
					}
				}

				// Make sure all compare flags are set.
				// Also whenever an attribute is not present, make sure the tag is filled in in the
				// ValidationRuleDicomAttribute.TagSequence, because this will indicate that it needs to
				// be displayed.
				for (int index = 0; index < dicomAttributesToValidate.Count; index++)
				{
					DicomAttributeToValidate dicomAttributeToValidate = dicomAttributesToValidate[index] as DicomAttributeToValidate;
					if ((bool)(displayAttributeSets[index]))
					{
						dicomAttributeToValidate.ValidationRuleDicomAttribute.TagSequence = lowestTagSequenceAsString;
					}
					dicomAttributeToValidate.ValidationRuleDicomAttribute.CompareFlags = (CompareFlags)compareFlagsForAttributeSets[index];
					dicomAttributeToValidate.ValidationRuleDicomAttribute.DicomAttributeFlags = (DicomAttributeFlags)dicomAttributeFlagsForAttributeSets[index];
				}
			}
			else
			{
				// Assign the null pointer to indicate no attributes are unprocessed anymore.
				dicomAttributesToValidate = null;
			}

			return(dicomAttributesToValidate);
		}
	}
}
