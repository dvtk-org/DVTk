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

using AttributeSet = DvtkHighLevelInterface.Dicom.Other.AttributeSet;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// <br></br>
	/// This class contains the shared functionality for doing a static and dynamic compare of
	/// multiple datasets.
	/// </summary>
	public class CompareBase
	{
		//
		// - Constant fields -
		//

		/// <summary>
		/// Grey color.
		/// </summary>
		private const String BACKGROUND_GREY = "bgcolor=\"#BBBBBB\"";

		/// <summary>
		/// Red color.
		/// </summary>
		private const String BACKGROUND_RED = "bgcolor=\"#FF0000\"";

		/// <summary>
		/// Width of a name column in pixels.
		/// </summary>
		private const int PIXEL_WIDTH_NAME = 260;

		/// <summary>
		/// Width of a present column in pixels.
		/// </summary>
		private const int PIXEL_WIDTH_PRESENT = 15;

		/// <summary>
		/// Width of a tag column in pixels.
		/// </summary>
		private const int PIXEL_WIDTH_TAG = 180;

		/// <summary>
		/// Width of a values column in pixels.
		/// </summary>
		private const int PIXEL_WIDTH_VALUES = 160;

		/// <summary>
		/// Width of a VR column in pixels.
		/// </summary>
		private const int PIXEL_WIDTH_VR = 30;



		//
		// - Fields -
		//

		/// <summary>
		/// If displayed, contains the table column index of the name column for an attribute.
		/// </summary>
		private ArrayList columnIndexAttributeName = new ArrayList();

		/// <summary>
		/// If displayed, contains the table column index of the present column for an attribute.
		/// </summary>
		private ArrayList columnIndexAttributePresent = new ArrayList();

		/// <summary>
		/// If displayed, contains the table column index of the tag column for an attribute.
		/// </summary>
		private ArrayList columnIndexAttributeTag = new ArrayList();

		/// <summary>
		/// If displayed, contains the table column index of the values column for an attribute.
		/// </summary>
		private ArrayList columnIndexAttributeValues = new ArrayList();

		/// <summary>
		/// If displayed, contains the table column index of the VR column for an attribute.
		/// </summary>
		private ArrayList columnIndexAttributeVR = new ArrayList();

		/// <summary>
		/// If displayed, contains the table column index of the common name column.
		/// </summary>
		private int columnIndexCommonName = -1;

		/// <summary>
		/// If displayed, contains the table column index of the common tag column.
		/// </summary>
		private int columnIndexCommonTag = -1;

		/// <summary>
		/// If displayed, contains the table column index of the common VR column.
		/// </summary>
		private int columnIndexCommonVR = -1;

		/// <summary>
		/// Indicates if a difference has been found that has not been added to the difference total.
		/// </summary>
		protected bool differenceFound = false;


		//
		// - Constructors -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Default constructor.
		/// </summary>
		public CompareBase()
		{
		}



		//
		// - Methods -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Add the common information for the begin or end of a sequence item.
		/// Do this only for those columns that should be displayed.
		/// </summary>
		/// <param name="oneBasedLevel">The nested level.</param>
		/// <param name="isBeginItem">If true, this is the begin item. If false, this is the end item.</param>
		/// <param name="oneBasedSequenceItemIndex">The sequence item index.</param>
		private void AddCommonInformationForBeginOrEndSequenceItems(int oneBasedLevel, bool isBeginItem, int oneBasedSequenceItemIndex)
		{
			// If the table column "Common tag" should be displayed, display the text "BEGIN ITEM" or "END ITEM" in the cell.
			if (this.displayCommonTag)
			{
				SetCellOK(this.columnIndexCommonTag, GetTagCellTextForBeginOrEndItem(oneBasedLevel, isBeginItem));
			}

			// If the table column "Common name" should be displayed, display the sequence item number in the cell.
			if (this.displayCommonName)
			{
				SetCellOK(this.columnIndexCommonName, GetNameCellTextForBeginOrEndItem(oneBasedSequenceItemIndex));
			}

			// If the table column "Common VR" should be displayed, display the cell as not applicable.
			if (this.displayCommonVR)
			{
				SetCellNotApplicable(this.columnIndexCommonVR);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Get the text for the name cell for a begin or end item.
		/// </summary>
		/// <param name="oneBasedSequenceItemIndex">One based seuqnece item index.</param>
		/// <returns>The text for the name cell.</returns>
		private String GetNameCellTextForBeginOrEndItem(int oneBasedSequenceItemIndex)
		{
			return("Item " + oneBasedSequenceItemIndex.ToString());
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Get the text for the tag cell for a begin or end item.
		/// </summary>
		/// <param name="oneBasedLevel">The zero based nested level.</param>
		/// <param name="isBeginItem">Indicates if this is the begin or end of an item.</param>
		/// <returns>The text for the tag cell.</returns>
		private String GetTagCellTextForBeginOrEndItem(int oneBasedLevel, bool isBeginItem)
		{
			String tagText = "";

			if (isBeginItem)
			{
				tagText = "".PadRight(oneBasedLevel, '>') + "BEGIN ITEM";
			}
			else
			{
				tagText = "".PadRight(oneBasedLevel, '>') + "END ITEM";
			}
			
			return(tagText);
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
		/// </summary>
		/// <param name="dicomAttributesToValidate">-</param>
		/// <param name="zeroBasedIndex">-</param>
		private void AddAttributeInformationForAttribute(DicomAttributesToValidate dicomAttributesToValidate, int zeroBasedIndex)
		{
			bool isAttributePresent = false;
			DicomAttributeToValidate dicomAttributeToValidate = dicomAttributesToValidate[zeroBasedIndex] as DicomAttributeToValidate;
			CompareFlags compareFlags = dicomAttributeToValidate.ValidationRuleDicomAttribute.CompareFlags;
			DicomAttributeFlags dicomAttributeFlags = dicomAttributeToValidate.ValidationRuleDicomAttribute.DicomAttributeFlags;

			if (dicomAttributeToValidate.Attribute is ValidAttribute)
			{
				isAttributePresent = true;
			}
			else
			{
				isAttributePresent = false;
			}

			// Attribute Tag column.
			if (this.displayAttributeTag)
			{
				if (isAttributePresent)
				{
					if (dicomAttributeToValidate.ValidationRuleDicomAttribute.DisplayFullTagSequence)
					{
						ArrayList tags = (dicomAttributeToValidate.Attribute as ValidAttribute).TagSequence.Tags;

						for (int tagIndex = 0; tagIndex < tags.Count; tagIndex++)
						{
							Tag tag = tags[tagIndex] as Tag;
							if (tag.ContainsIndex)
							{
								SetCellOK((int)this.columnIndexAttributeTag[zeroBasedIndex], "".PadRight(tagIndex, '>') + tag.DicomNotation + "(" + tag.IndexNumber + ")");
							}
							else
							{
								SetCellOK((int)this.columnIndexAttributeTag[zeroBasedIndex], "".PadRight(tagIndex, '>') + tag.DicomNotation);
							}
						}
					}
					else
					{
						SetCellOK((int)this.columnIndexAttributeTag[zeroBasedIndex], ((ValidAttribute)dicomAttributeToValidate.Attribute).TagSequence.DicomNotation);
					}
				}
				else
				{
					if (dicomAttributeToValidate.ValidationRuleDicomAttribute.TagSequence == "")
					{
						SetCellNotApplicable((int)this.columnIndexAttributeTag[zeroBasedIndex]);
					}
					else
					{
						TagSequence tagSequence = new TagSequence(dicomAttributeToValidate.ValidationRuleDicomAttribute.TagSequence);
						ArrayList tags = tagSequence.Tags;

						for (int tagIndex = 0; tagIndex < tags.Count; tagIndex++)
						{
							Tag tag = tags[tagIndex] as Tag;
							if (tag.ContainsIndex)
							{
								SetCellOK((int)this.columnIndexAttributeTag[zeroBasedIndex], "".PadRight(tagIndex, '>') + tag.DicomNotation + "(" + tag.IndexNumber + ")");
							}
							else
							{
								SetCellOK((int)this.columnIndexAttributeTag[zeroBasedIndex], "".PadRight(tagIndex, '>') + tag.DicomNotation);
							}
						}
					}
				}
			}

			// Attribute Name column.
			if (this.displayAttributeName)
			{
				if (isAttributePresent)
				{
					SetCellOK((int)this.columnIndexAttributeName[zeroBasedIndex], GetAttributeName(dicomAttributeToValidate.Attribute));
				}
				else
				{
					if (dicomAttributeToValidate.ValidationRuleDicomAttribute.TagSequence == "")
					{
						SetCellNotApplicable((int)this.columnIndexAttributeName[zeroBasedIndex]);
					}
					else
					{
						SetCellOK((int)this.columnIndexAttributeName[zeroBasedIndex], "-");
					}
				}
			}


			// Attribute Present column.
			if (this.displayAttributePresent)
			{
				if (dicomAttributeToValidate.ValidationRuleDicomAttribute.TagSequence == "")
				{
					SetCellNotApplicable((int)this.columnIndexAttributePresent[zeroBasedIndex]);
				}
				else
				{
					String presentString = "";
					bool containsError = false;

					if (isAttributePresent)
					{
						presentString = "+";
					}
					else
					{
						presentString = "-";
					}

					if ((compareFlags & CompareFlags.Compare_present) == CompareFlags.Compare_present)
					{
						if (dicomAttributesToValidate.ContainsComparePresentErrors)
						{
							containsError = true;
						}
					}

					if ((dicomAttributeFlags & DicomAttributeFlags.Present) == DicomAttributeFlags.Present)
					{
						if (!isAttributePresent)
						{
							containsError = true;
						}
					}
	
					if ((dicomAttributeFlags & DicomAttributeFlags.Not_present) == DicomAttributeFlags.Not_present)
					{
						if (isAttributePresent)
						{
							containsError = true;
						}
					}

					if (containsError)
					{
						SetCellError((int)this.columnIndexAttributePresent[zeroBasedIndex], presentString);
					}
					else
					{
						SetCellOK((int)this.columnIndexAttributePresent[zeroBasedIndex], presentString);
					}
				}
			}

			// Attribute VR column.
			if (this.displayAttributeVR)
			{
				if (isAttributePresent)
				{
					bool containsError = false;

					if ((compareFlags & CompareFlags.Compare_VR) == CompareFlags.Compare_VR)
					{
						if (dicomAttributesToValidate.ContainsCompareVRErrors)
						{
							containsError = true;
						}
					}
				
					if (containsError)
					{
						SetCellError((int)this.columnIndexAttributeVR[zeroBasedIndex], dicomAttributeToValidate.Attribute.VR.ToString());
					}
					else
					{
						SetCellOK((int)this.columnIndexAttributeVR[zeroBasedIndex], dicomAttributeToValidate.Attribute.VR.ToString());
					}
				}
				else
				{
					SetCellNotApplicable((int)this.columnIndexAttributeVR[zeroBasedIndex]);
				}
			}

			// Attribute Values column.
			if (this.displayAttributeValues)
			{
				int columnIndex = (int)this.columnIndexAttributeValues[zeroBasedIndex];

				if (isAttributePresent)
				{
					if (dicomAttributeToValidate.Attribute.VR == VR.SQ)
					{
						SetCellNotApplicable(columnIndex);
					}
					else
					{
						bool containsError = false;

						if ((compareFlags & CompareFlags.Compare_values) == CompareFlags.Compare_values)
						{
							if (dicomAttributesToValidate.ContainsCompareValuesErrors)
							{
								containsError = true;
							}
						}

						if ((dicomAttributeFlags & DicomAttributeFlags.Values) == DicomAttributeFlags.Values)
						{
							if (dicomAttributeToValidate.Attribute.Values.Count == 0)
							{
								containsError = true;
							}
						}
	
						if ((dicomAttributeFlags & DicomAttributeFlags.No_values) == DicomAttributeFlags.No_values)
						{
							if (dicomAttributeToValidate.Attribute.Values.Count > 0)
							{
								containsError = true;
							}
						}

						if (containsError)
						{
							SetCellError(columnIndex, dicomAttributeToValidate.Attribute.Values.ToString());
						}
						else
						{
							SetCellOK(columnIndex, dicomAttributeToValidate.Attribute.Values.ToString());
						}
					}
				}
				else
				{
					SetCellNotApplicable(columnIndex);
				}
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Add a new row to the CompareResults table and check if the DifferenceCount needs
		/// to be increased.
		/// </summary>
		private void NewRow()
		{
			this.compareResults.Table.NewRow();
			
			if (this.differenceFound)
			{
				this.compareResults.DifferencesCount++;
				this.differenceFound = false;
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
		/// </summary>
		private void AddAttributeInformationForBeginOrEndSequenceItem(DicomAttributesToValidate sequenceAttributes, int zeroBasedSequenceAttributesIndex, int sequenceItemIndex, bool isBeginItem)
		{
			DicomAttributeToValidate sequenceAttributeToValidate = sequenceAttributes[zeroBasedSequenceAttributesIndex] as DicomAttributeToValidate;

			if (this.displayAttributeTag)
			{
				int columnIndex = (int)this.columnIndexAttributeTag[zeroBasedSequenceAttributesIndex];
				
				if (sequenceAttributeToValidate.Display)
				{
					if (sequenceAttributeToValidate.Attribute.VR == VR.SQ)
					{
						if (sequenceItemIndex > sequenceAttributeToValidate.Attribute.ItemCount)
						{
							SetCellNotApplicable(columnIndex);
						}
						else
						{
							int level = (sequenceAttributeToValidate.Attribute as ValidAttribute).TagSequence.Tags.Count - 1;

							SetCellOK(columnIndex, GetTagCellTextForBeginOrEndItem(level, isBeginItem));
						}
					}
					else
					{
						SetCellNotApplicable(columnIndex);
					}
				}
				else
				{
					SetCellNotApplicable(columnIndex);
				}
			}

			if (this.displayAttributeName)
			{
				int columnIndex = (int)this.columnIndexAttributeName[zeroBasedSequenceAttributesIndex];

				if (sequenceAttributeToValidate.Display)
				{
					if (sequenceItemIndex > sequenceAttributeToValidate.Attribute.ItemCount)
					{
						SetCellNotApplicable(columnIndex);
					}
					else
					{
						SetCellOK(columnIndex, GetNameCellTextForBeginOrEndItem( sequenceItemIndex)); 
					}
				}
				else
				{
					SetCellNotApplicable(columnIndex);
				}
			}

			if (this.displayAttributePresent)
			{
				int columnIndex = (int)this.columnIndexAttributePresent[zeroBasedSequenceAttributesIndex];

				if (sequenceAttributeToValidate.Display)
				{
					String presentText = "";

					if (sequenceAttributeToValidate.Attribute is ValidAttribute)
					{
						if (sequenceAttributeToValidate.Attribute.VR == VR.SQ)
						{
							if (sequenceItemIndex > sequenceAttributeToValidate.Attribute.ItemCount)
							{
								presentText = "-";
							}
							else
							{
								presentText = "+";
							}
						}
						else
						{
							presentText = "-";
						}
					}
					else
					{
						presentText = "-";
					}

					if (sequenceAttributes.ContainsCompareSequenceItemsErrors(sequenceItemIndex))
					{
						SetCellError(columnIndex, presentText);
					}
					else
					{
						SetCellOK(columnIndex, presentText);
					}
				}
				else
				{
					SetCellNotApplicable(columnIndex);
				}
			}

			if (this.displayAttributeVR)
			{
				int columnIndex = (int)this.columnIndexAttributeVR[zeroBasedSequenceAttributesIndex];

				SetCellNotApplicable(columnIndex);
			}

			if (this.displayAttributeValues)
			{
				int columnIndex = (int)this.columnIndexAttributeValues[zeroBasedSequenceAttributesIndex];

				SetCellNotApplicable(columnIndex);
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        /// <param name="attributeSets">-</param>
        /// <param name="validationRuleLists">-</param>
		internal protected void AddAttributeSetsInformationUsingDynamicCompare(ArrayList attributeSets, ArrayList validationRuleLists)
		{
			//
			// Iterate through all attributes of all AttributeSets.
			//

			GeneratorDynamicCompare generatorDynamicCompare = new GeneratorDynamicCompare(attributeSets, validationRuleLists);

			DicomAttributesToValidate dicomAttributesToValidate = null;

			while ((dicomAttributesToValidate = generatorDynamicCompare.GetNextAttributes()) != null)
			{
				AddAttributesInformation(dicomAttributesToValidate);

				if (this.addEmptyRowAfterEachDynamicComparedList)
				{
					NewRow();

					for (int columnIndex = 0; columnIndex < this.numberOfColumns; columnIndex++)
					{
						SetCellOK(columnIndex + 1, ".");
					}
				}
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        /// <param name="attributeSets">-</param>
        /// <param name="compareFlagsForAttributeSets">-</param>
        /// <param name="dicomAttributeFlagsForAttributeSets">-</param>
        /// <param name="displayAttributeSets">-</param>
		internal protected void AddAttributeSetsInformationUsingStaticCompare(ArrayList attributeSets, ArrayList compareFlagsForAttributeSets, ArrayList dicomAttributeFlagsForAttributeSets, ArrayList displayAttributeSets)
		{
			//
			// Iterate through all attributes of all AttributeSets.
			//

			GeneratorStaticCompare generatorStaticCompare = new GeneratorStaticCompare(attributeSets);

			DicomAttributesToValidate dicomAttributesToValidate = null;

			while ((dicomAttributesToValidate = generatorStaticCompare.GetNextAttributes(compareFlagsForAttributeSets, dicomAttributeFlagsForAttributeSets, displayAttributeSets)) != null)
			{
				AddAttributesInformation(dicomAttributesToValidate);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
		/// </summary>
		/// <param name="dicomAttributesToValidate">-</param>
		private void AddAttributesInformation(DicomAttributesToValidate dicomAttributesToValidate)
		{
			NewRow();


			//
			// Fill in the common information.
			//

			AddCommonInformationForAttributes(dicomAttributesToValidate);


			//
			// Fill in the information specific for each attribute.
			//

			for (int index = 0; index < dicomAttributesToValidate.Count; index++)
			{
				AddAttributeInformationForAttribute(dicomAttributesToValidate, index);
			}


			//
			// If at least one attribute is a sequence attribute, do a comparison of the content.
			//

			if (dicomAttributesToValidate.ContainsSequenceAttribute)
			{
				// If a sequence attribute is present, also a leading attribute must be present of type ValidAttribute.
				int oneBasedLevel = (dicomAttributesToValidate.LeadingAttribute.Attribute as ValidAttribute).TagSequence.Tags.Count;

				// !!!!! DicomAttributesToValidate convertedToSequenceAttributes = dicomAttributesToValidate.ConvertedToSequenceAttributes;
				// !!!!! AddSequenceAttributesContentInformation(convertedToSequenceAttributes, oneBasedLevel);

				AddSequenceAttributesContentInformation(dicomAttributesToValidate, oneBasedLevel);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
		/// </summary>
		/// <param name="dicomAttributesToValidate">-</param>
		private void AddCommonInformationForAttributes(DicomAttributesToValidate dicomAttributesToValidate)
		{
			bool isLeadingAttributePresent = false;
			DicomAttributeToValidate leadingAttribute = dicomAttributesToValidate.LeadingAttribute;

			if (leadingAttribute.Attribute is ValidAttribute)
			{
				isLeadingAttributePresent = true;
			}
			else
			{
				isLeadingAttributePresent = false;
			}

			
			//
			// Fill in the common information.
			//

			// Common Tag column.
			if (this.displayCommonTag)
			{
				if (isLeadingAttributePresent)
				{
					SetCellOK(this.columnIndexCommonTag, ((ValidAttribute)leadingAttribute.Attribute).TagSequence.DicomNotation);
				}
				else
				{
					SetCellNotApplicable(this.columnIndexCommonTag);
				}
			}

			// Common Name column.
			if (this.displayCommonName)
			{
				if (isLeadingAttributePresent)
				{
					SetCellOK(this.columnIndexCommonName, GetAttributeName(leadingAttribute.Attribute));
				}
				else
				{
					SetCellNotApplicable(this.columnIndexCommonName);
				}
			}
		
			// Common VR column.
			if (this.displayCommonVR)
			{
				if (isLeadingAttributePresent)
				{
					SetCellOK(this.columnIndexCommonVR, leadingAttribute.Attribute.VR.ToString());
				}
				else
				{
					SetCellNotApplicable(this.columnIndexCommonVR);
				}
			}
		}


		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
		/// </summary>
		/// <param name="numberOfColumns">-</param>
		/// <param name="numberOfAttributeSets">-</param>
		/// <param name="tableDescription">-</param>
		/// <param name="attributeSetDescriptions">-</param>
		/// <returns>-</returns>
		internal protected CompareResults CreateCompareResults(int numberOfColumns, int numberOfAttributeSets, String tableDescription, StringCollection attributeSetDescriptions)
		{
			CompareResults compareResults = new CompareResults(numberOfColumns);
			compareResults.Table.CellItemSeperator = "<br>";

			int index = 0;
			String[] header1 = new String[numberOfColumns];
			String[] header2 = new String[numberOfColumns];
			String[] header3 = new String[numberOfColumns];
			int[] columnWidths = new int[numberOfColumns];

			if (this.displayCommonTag)
			{
				header1[index] = tableDescription;
				header2[index] = "Common info";
				header3[index] = TAG_STRING;
				columnWidths[index] = PIXEL_WIDTH_TAG;
				index++;
			}

			if (this.displayCommonName)
			{
				header1[index] = tableDescription;
				header2[index] = "Common info";
				header3[index] = NAME_STRING;
				columnWidths[index] = PIXEL_WIDTH_NAME;
				index++;
			}

			if (this.displayCommonVR)
			{
				header1[index] = tableDescription;
				header2[index] = "Common info";
				header3[index] = VR_STRING;
				columnWidths[index] = PIXEL_WIDTH_VR;
				index++;
			}

			for (int attributeSetsIndex = 0; attributeSetsIndex < numberOfAttributeSets; attributeSetsIndex++)
			{
				if (this.displayAttributeTag)
				{
					header1[index] = tableDescription;
					header2[index] = attributeSetDescriptions[attributeSetsIndex];
					header3[index] = TAG_STRING;
					columnWidths[index] = PIXEL_WIDTH_TAG;
					index++;
				}

				if (this.displayAttributeName)
				{
					header1[index] = tableDescription;
					header2[index] = attributeSetDescriptions[attributeSetsIndex];
					header3[index] = NAME_STRING;
					columnWidths[index] = PIXEL_WIDTH_NAME;
					index++;
				}

				if (this.displayAttributePresent)
				{
					header1[index] = tableDescription;
					header2[index] = attributeSetDescriptions[attributeSetsIndex];
					header3[index] = PRESENT_STRING;
					columnWidths[index] = PIXEL_WIDTH_PRESENT;
					index++;
				}

				if (this.displayAttributeVR)
				{
					header1[index] = tableDescription;
					header2[index] = attributeSetDescriptions[attributeSetsIndex];
					header3[index] = VR_STRING;
					columnWidths[index] = PIXEL_WIDTH_VR;
					index++;
				}

				if (this.displayAttributeValues)
				{
					header1[index] = tableDescription;
					header2[index] = attributeSetDescriptions[attributeSetsIndex];
					header3[index] = VALUES_STRING;
					columnWidths[index] = PIXEL_WIDTH_VALUES;
					index++;
				}
			}
			
			compareResults.Table.AddHeader(header1);
			compareResults.Table.AddHeader(header2);
			compareResults.Table.AddHeader(header3);
			compareResults.Table.SetColumnPixelWidths(columnWidths);
			return(compareResults);
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
		/// </summary>
		/// <param name="numberOfAttributeSets">-</param>
		/// <returns>-</returns>
		internal protected int DetermineColumnIndices(int numberOfAttributeSets)
		{
			int columnIndex = 1;

			if (this.displayCommonTag)
			{
				this.columnIndexCommonTag = columnIndex;
				columnIndex++;
			}

			if (this.displayCommonName)
			{
				this.columnIndexCommonName = columnIndex;
				columnIndex++;
			}

			if (this.displayCommonVR)
			{
				this.columnIndexCommonVR = columnIndex;
				columnIndex++;
			}

			for (int index = 0; index < numberOfAttributeSets; index++)
			{
				if (this.displayAttributeTag)
				{
					this.columnIndexAttributeTag.Add(columnIndex);
					columnIndex++;
				}

				if (this.displayAttributeName)
				{
					this.columnIndexAttributeName.Add(columnIndex);
					columnIndex++;
				}

				if (this.displayAttributePresent)
				{
					this.columnIndexAttributePresent.Add(columnIndex);
					columnIndex++;
				}

				if (this.displayAttributeVR)
				{
					this.columnIndexAttributeVR.Add(columnIndex);
					columnIndex++;
				}

				if (this.displayAttributeValues)
				{
					this.columnIndexAttributeValues.Add(columnIndex);
					columnIndex++;
				}
			}

			this.numberOfColumns = columnIndex - 1;

			return(columnIndex - 1);
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
		/// </summary>
		/// <param name="attribute">-</param>
		/// <returns>-</returns>
		private String GetAttributeName(Attribute attribute)
		{
			String name = "-";

			if ((attribute.Name != "") && !(attribute.Name.StartsWith(" : private mapped to")))
			{
				name = "\"" + attribute.Name + "\"";
			}

			return(name);
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Fill the cell with text indicating error.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="text">The text.</param>
		private void SetCellError(int column, String text)
		{
			this.compareResults.Table.AddBlackItem(column, text);
			this.compareResults.Table.SetCellPrefix(column, BACKGROUND_RED);
			differenceFound = true;
		}
		
		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Set the cell content to indicate that it is not relevant.
		/// </summary>
		/// <param name="column">The column.</param>
		private void SetCellNotApplicable(int column)
		{
			this.compareResults.Table.SetCellPrefix(column, BACKGROUND_GREY);
		}
		
		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Fill the cell with text indicating no error.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="text">The text.</param>
		private void SetCellOK(int column, String text)
		{
			this.compareResults.Table.AddBlackItem(column, text);
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		internal protected bool displayAttributeName = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected bool displayAttributePresent = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected bool displayAttributeTag = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected bool displayAttributeValues = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected bool displayAttributeVR = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected bool displayCommonName = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected bool displayCommonTag = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected bool displayCommonVR = true;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected String NAME_STRING = "______Name______";

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected String TAG_STRING = "_______Tag_______";

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected String PRESENT_STRING = "Pr";

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected String VALUES_STRING = "________Values________";

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        internal protected String VR_STRING = "VR";

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// The field used to store the table that is the result of comparing.
		/// </summary>
		internal protected CompareResults compareResults = null;

		private int numberOfColumns = 0;

		private bool addEmptyRowAfterEachDynamicComparedList = false;

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        public bool AddEmptyRowAfterEachDynamicComparedList
		{
			set
			{
				this.addEmptyRowAfterEachDynamicComparedList = value;
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// </summary>
		public bool DisplayAttributeVR
		{
			get
			{
				return(this.displayAttributeVR);
			}

			set
			{
				this.displayAttributeVR = value;
			}
		}

		private void AddSequenceAttributesContentInformation(DicomAttributesToValidate dicomAttributesToValidate, int oneBasedLevel)
		{
			//
			// Determine the maximum amount of sequence items for all supplied sequence attributes.
			//

			int maxItemCount = dicomAttributesToValidate.MaxItemCount;
			

			//
			// Fill the table with the information.
			//

			for (int itemIndex = 1; itemIndex <= maxItemCount; itemIndex++)
			{
				//
				// Add begin item row.
				//

				NewRow();

				AddCommonInformationForBeginOrEndSequenceItems(oneBasedLevel, true, itemIndex);

				for (int attributeIndex = 0; attributeIndex < dicomAttributesToValidate.Count; attributeIndex++)
				{
					AddAttributeInformationForBeginOrEndSequenceItem(dicomAttributesToValidate, attributeIndex, itemIndex, true);
				}

				
				//
				// Add the comparison of attributes within the sequence items.
				//
	
				ArrayList sequenceItemsToCompare = new ArrayList();
				ArrayList compareFlagsForSequenceItems = new ArrayList();
				ArrayList dicomAttributeFlagsForSequenceItems = new ArrayList();
				ArrayList displaySequenceItems = new ArrayList();

				for (int attributeSetsIndex = 0; attributeSetsIndex < dicomAttributesToValidate.Count; attributeSetsIndex++)
				{
					DicomAttributeToValidate dicomAttributeToValidate = dicomAttributesToValidate[attributeSetsIndex] as DicomAttributeToValidate;
					Attribute attribute = dicomAttributeToValidate.Attribute;

					if (attribute is ValidAttribute)
					{
						if (attribute.VR == VR.SQ)
						{
							if ((dicomAttributeToValidate.ValidationRuleDicomAttribute.DicomAttributeFlags & DicomAttributeFlags.Include_sequence_items) == DicomAttributeFlags.Include_sequence_items)
							{
								if (attribute.ItemCount < itemIndex)
								{
									// Add empty sequence item.
									sequenceItemsToCompare.Add(new SequenceItem());
								}
								else
								{
									sequenceItemsToCompare.Add(attribute.GetItem(itemIndex));
								}
							}
							else
							{
								// Add empty sequence item.
								sequenceItemsToCompare.Add(new SequenceItem());
							}
						}
						else
						{
							// Add empty sequence item.
							sequenceItemsToCompare.Add(new SequenceItem());
						}
					}
					else
					{
						// Add empty sequence item.
						sequenceItemsToCompare.Add(new SequenceItem());
					}

					compareFlagsForSequenceItems.Add(dicomAttributeToValidate.ValidationRuleDicomAttribute.CompareFlags);
					dicomAttributeFlagsForSequenceItems.Add(dicomAttributeToValidate.ValidationRuleDicomAttribute.DicomAttributeFlags & DicomAttributeFlags.Include_sequence_items);

					if (dicomAttributeToValidate.Display)
					{
						displaySequenceItems.Add(true);
					}
					else
					{
						displaySequenceItems.Add(false);
					}
				}

				AddAttributeSetsInformationUsingStaticCompare(sequenceItemsToCompare, compareFlagsForSequenceItems, dicomAttributeFlagsForSequenceItems, displaySequenceItems);


				//
				// Add end item row.
				//

				NewRow();

				AddCommonInformationForBeginOrEndSequenceItems(oneBasedLevel, false, itemIndex);

				for (int attributeIndex = 0; attributeIndex < dicomAttributesToValidate.Count; attributeIndex++)
				{
					AddAttributeInformationForBeginOrEndSequenceItem(dicomAttributesToValidate, attributeIndex, itemIndex, false);
				}
			}
		}
	}
}
