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



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
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

		/// <summary>
		/// Width of a comments column in pixels.
		/// </summary>
		private const int PIXEL_WIDTH_COMMENTS = 160;



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
		/// If displayed, contains the table column index of the comments column.
		/// </summary>
		private int columnIndexComments = -1;

		/// <summary>
		/// If displayed, contains the table column index of the common name column.
		/// </summary>
		private int columnIndexCommonName = -1;

		/// <summary>
		/// If displayed, contains the table column index of the common tag column.
		/// </summary>
		private int columnIndexCommonTag = -1;

		/// <summary>
		/// Indicates if a difference has been found that has not been added to the difference total.
		/// </summary>
		protected bool differenceFound = false;

		/// <summary>
		/// See property DisplayAttributeName.
		/// </summary>
		private bool displayAttributeName = true;

		/// <summary>
		/// See property DisplayAttributePresent.
		/// </summary>
		private bool displayAttributePresent = true;

		/// <summary>
		/// See property DisplayAttributeTag.
		/// </summary>
		private bool displayAttributeTag = true;

		/// <summary>
		/// See property DisplayAttributeValues.
		/// </summary>
		private bool displayAttributeValues = true;

		/// <summary>
		/// See property DisplayAttributeVR.
		/// </summary>
		private bool displayAttributeVR = true;

		/// <summary>
		/// See property DisplayComments.
		/// </summary>
		private bool displayComments = true;

		/// <summary>
		/// See property DisplayCommonName.
		/// </summary>
		private bool displayCommonName = true;

		/// <summary>
		/// See property DisplayCommonTag.
		/// </summary>
		private bool displayCommonTag = true;

		/// <summary>
		/// See property DisplayCompareValueType.
		/// </summary>
		private bool displayCompareValueType = false;

		/// <summary>
		/// See property DisplayFlags.
		/// </summary>
		private bool displayFlags = false;

        /// <summary>
        /// Number of columns.
        /// </summary>
        private int numberOfColumns = 0;



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CompareBase()
		{
            // Do nothing.
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Property indicating whether the attribute name column should be displayed.
		/// </summary>
		public bool DisplayAttributeName
		{
			get
			{
				return(this.displayAttributeName);
			}

			set
			{
				this.displayAttributeName = value;
			}
		}

		/// <summary>
		/// Property indicating whether the attribute present column should be displayed.
		/// </summary>
		public bool DisplayAttributePresent
		{
			get
			{
				return(this.displayAttributePresent);
			}

			set
			{
				this.displayAttributePresent = value;
			}
		}

		/// <summary>
		/// Property indicating whether the attribute tag column should be displayed.
		/// </summary>
		public bool DisplayAttributeTag
		{
			get
			{
				return(this.displayAttributeTag);
			}

			set
			{
				this.displayAttributeTag = value;
			}
		}

		/// <summary>
		/// Property indicating whether the attribute values column should be displayed.
		/// </summary>
		public bool DisplayAttributeValues
		{
			get
			{
				return(this.displayAttributeValues);
			}

			set
			{
				this.displayAttributeValues = value;
			}
		}

		/// <summary>
		/// Property indicating whether the attribute VR column should be displayed.
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

		/// <summary>
		/// Property indicating whether the comments column should be displayed.
		/// </summary>
		public bool DisplayComments
		{
			get
			{
				return(this.displayComments);
			}

			set
			{
				this.displayComments = value;
			}
		}

		/// <summary>
		/// Property indicating whether the common name column should be displayed.
		/// </summary>
		public bool DisplayCommonName
		{
			get
			{
				return(this.displayCommonName);
			}

			set
			{
				this.displayCommonName = value;
			}
		}

		/// <summary>
        /// Gets or sets a boolean indicating whether the common tag column should be displayed.
		/// </summary>
		public bool DisplayCommonTag
		{
			get
			{
				return(this.displayCommonTag);
			}

			set
			{
				this.displayCommonTag = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating whether the compare value type should be displayed.
        /// </summary>
		public bool DisplayCompareValueType
		{
			get
			{
				return(this.displayCompareValueType);
			}
			set
			{
				this.displayCompareValueType = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating whether the flags should be displayed.
        /// </summary>
        public bool DisplayFlags
		{
			get
			{
				return(this.displayFlags);
			}
			set
			{
				this.displayFlags = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating whether the group length should be displayed.
        /// </summary>
        public bool DisplayGroupLength
		{
			get
			{
				return(this.displayGroupLength);
			}
			set
			{
				this.displayGroupLength = value;
			}
		}



		//
		// - Methods -
		//

		private void SetTagCellForDicomAttribute(DicomAttribute dicomAttribute, int columnIndex)
		{
			if (dicomAttribute != null)
			{
				TagSequence tagSequence = new TagSequence(dicomAttribute.ValidationRuleDicomAttribute.TagSequenceString);

				if (dicomAttribute.DisplayFullTagSequence)
				{
					ArrayList tags = tagSequence.Tags;

					for (int tagIndex = 0; tagIndex < tags.Count; tagIndex++)
					{
						Tag tag = tags[tagIndex] as Tag;
						if (tag.ContainsIndex)
						{
							SetCellOK(columnIndex, "".PadRight(tagIndex, '>') + tag.DicomNotation + "(" + tag.IndexNumber + ")");
						}
						else
						{
							SetCellOK(columnIndex, "".PadRight(tagIndex, '>') + tag.DicomNotation);
						}
					}
				}
				else
				{
					SetCellOK(columnIndex, tagSequence.DicomNotation);
				}
			}

			if (this.displayFlags)
			{
				SetCellOK(columnIndex, "<br><i>Flags:</i>");
				SetCellOK(columnIndex, "<i>" + dicomAttribute.ValidationRule.FlagsString + "</i>"); 
			}
		}


		private void SetTagCellForHl7Attribute(AttributeList attributeList, int zeroBasedIndex)
		{
			int columnIndex = (int)this.columnIndexAttributeTag[zeroBasedIndex];

			Hl7Attribute hl7Attribute = attributeList[zeroBasedIndex] as Hl7Attribute;

			Dvtk.Hl7.Hl7Tag hl7Tag = hl7Attribute.ValidationRuleHl7Attribute.Hl7Tag;

			SetCellOK(columnIndex, hl7Tag.SegmentId.Id + "-" + hl7Tag.FieldIndex.ToString());
			
			if (this.displayFlags)
			{
				SetCellOK(columnIndex, "<br><i>Flags:</i>");
				SetCellOK(columnIndex, "<i>" + hl7Attribute.ValidationRule.FlagsString + "</i>"); 
			}
		}

		private void SetCommentsCell(CompareRule compareRule)
		{
			if (compareRule.ConditionText.Length > 0)
			{
				SetCellOK(this.columnIndexComments, "<i><b><font color=\"#6699cc\">" + compareRule.ConditionText + "</font></b></i>");
			}

			if (this.displayCompareValueType && this.displayAttributeValues)
			{
				SetCellOK(this.columnIndexComments, "<i>" + "Compare value type: " + compareRule.CompareValueType.ToString() + "</i>");
			}
		}

		private void SetNameCellForDicomAttribute(DicomAttribute dicomAttribute, int columnIndex)
		{
			if (dicomAttribute != null)
			{
				if (dicomAttribute.IsPresent)
				{
					String name = "-";

					if ((dicomAttribute.AttributeOnly.Name != "") && !(dicomAttribute.AttributeOnly.Name.StartsWith(" : private mapped to")))
					{
						name = "\"" + dicomAttribute.AttributeOnly.Name + "\"";
					}

					SetCellOK(columnIndex, name);
				}
				else
				{
					SetCellOK(columnIndex, "-");
				}
			}
		}

		private void SetPresentCellForAttribute(AttributeList attributeList, int zeroBasedIndex)
		{
			int columnIndex = (int)this.columnIndexAttributePresent[zeroBasedIndex];

			AttributeBase attributebase = attributeList[zeroBasedIndex];

			if (attributebase != null)
			{
				String presentString = "";
				bool containsError = false;

				if (attributebase.IsPresent)
				{
					presentString = "+";
				}
				else
				{
					presentString = "-";
				}

				if ((attributebase.ValidationRule.Flags & FlagsBase.Compare_present) == FlagsBase.Compare_present)
				{
					if (attributeList.ContainsComparePresentErrors)
					{
						containsError = true;
					}
				}

				if ((attributebase.ValidationRule.Flags & FlagsBase.Present) == FlagsBase.Present)
				{
					if (!attributebase.IsPresent)
					{
						containsError = true;
					}
				}
	
				if ((attributebase.ValidationRule.Flags & FlagsBase.Not_present) == FlagsBase.Not_present)
				{
					if (attributebase.IsPresent)
					{
						containsError = true;
					}
				}

				// If this is a Dicom attribute which is a group length, only compare when the compareGroupLength
				// field is true.
				if ((!this.displayGroupLength) && (attributebase is DicomAttribute))
				{
					DicomAttribute dicomAttribute = attributebase as DicomAttribute;

					TagSequence tagSequence = new TagSequence(dicomAttribute.ValidationRuleDicomAttribute.TagSequenceString);
 
					if (tagSequence.LastTag.ElementNumber == 0)
					{
						containsError = false;
					}
				}

				if (containsError)
				{
					SetCellError(columnIndex, presentString);
				}
				else
				{
					SetCellOK(columnIndex, presentString);
				}
			}
		}


		private void SetVrCellForDicomAttribute(AttributeList attributeList, int zeroBasedIndex)
		{
			int columnIndex = (int)this.columnIndexAttributeVR[zeroBasedIndex];

			DicomAttribute dicomAttribute = attributeList[zeroBasedIndex] as DicomAttribute;

			if (dicomAttribute.IsPresent)
			{
				bool containsError = false;

				if ((dicomAttribute.ValidationRule.Flags  & FlagsBase.Compare_VR) == FlagsBase.Compare_VR)
				{
					if (attributeList.DicomContainsCompareVRErrors)
					{
						containsError = true;
					}
				}
				
				if (containsError)
				{
					SetCellError(columnIndex, dicomAttribute.AttributeOnly.VR.ToString());
				}
				else
				{
					SetCellOK(columnIndex, dicomAttribute.AttributeOnly.VR.ToString());
				}
			}
		}

		private void SetValuesCellForAttribute(AttributeList attributeList, int zeroBasedIndex)
		{
			int columnIndex = (int)this.columnIndexAttributeValues[zeroBasedIndex];

			AttributeBase attributebase = attributeList[zeroBasedIndex];

			if (attributebase.IsPresent)
			{
				bool isDicomSequenceAttribute = false;

				if (attributebase is DicomAttribute)
				{
					if ((attributebase as DicomAttribute).AttributeOnly.VR == VR.SQ)
					{
						isDicomSequenceAttribute = true;
					}
				}

				if (!isDicomSequenceAttribute)
				{
					bool containsError = false;

					String valuesString = attributebase.ValuesToString();

					if ((attributebase.ValidationRule.Flags & FlagsBase.Compare_values) == FlagsBase.Compare_values)
					{
						if (attributeList.ContainsCompareValuesErrors)
						{
							containsError = true;
						}
					}

					if ((attributebase.ValidationRule.Flags & FlagsBase.Values) == FlagsBase.Values)
					{
						if (valuesString.Length == 0)
						{
							containsError = true;
						}
					}
	
					if ((attributebase.ValidationRule.Flags & FlagsBase.No_values) == FlagsBase.No_values)
					{
						if (valuesString.Length > 0)
						{
							containsError = true;
						}
					}

					// If this is a Dicom attribute which is a group length, only compare when the compareGroupLength
					// field is true.
					if ((!this.displayGroupLength) && (attributebase is DicomAttribute))
					{
						DicomAttribute dicomAttribute = attributebase as DicomAttribute;

						TagSequence tagSequence = new TagSequence(dicomAttribute.ValidationRuleDicomAttribute.TagSequenceString);
 
						if (tagSequence.LastTag.ElementNumber == 0)
						{
							containsError = false;
						}
					}

					// If the attribute contains no values, display this with the text "No values" in italic.
					if (valuesString.Length == 0)
					{
						valuesString = "<i>No values</i>";
					}

					if (containsError)
					{
						SetCellError(columnIndex, valuesString);
					}
					else
					{
						SetCellOK(columnIndex, valuesString);
					}
				}
				else
					// Is Sequence attribute.
				{
					if ((attributebase.ValidationRule.Flags & FlagsBase.Include_sequence_items) == 0)
						// If sequence item will not be displayed.
					{
						SetCellOK(columnIndex, "<i>Items will not be displayed.</i>");
					}
				}
			}
		}























		/// <summary>
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










		// Precondition: DicomAttribute.
		private void SetTagCellForDicomBeginOrEndSequenceItem(DicomAttribute dicomAttribute, int sequenceItemIndex, bool isBeginItem, int columnIndex)
		{				
			if (dicomAttribute.IsPresent)
			{
				if (dicomAttribute.AttributeOnly.VR == VR.SQ)
				{
					if (sequenceItemIndex <= dicomAttribute.AttributeOnly.ItemCount)
					{
						int level = (dicomAttribute.AttributeOnly as ValidAttribute).TagSequence.Tags.Count;

						String tagText = "";

						if (isBeginItem)
						{
							tagText = "".PadRight(level, '>') + "BEGIN ITEM";
						}
						else
						{
							tagText = "".PadRight(level, '>') + "END ITEM";
						}


						SetCellOK(columnIndex, tagText);
					}
				}
			}
		}



		// Precondition: DicomAttribute.
		private void SetNameCellForDicomBeginOrEndSequenceItem(DicomAttribute dicomAttribute, int sequenceItemIndex, bool isBeginItem, int columnIndex)
		{
			if (dicomAttribute.IsPresent)
			{
				if (dicomAttribute.AttributeOnly.VR == VR.SQ)
				{
					if (sequenceItemIndex <= dicomAttribute.AttributeOnly.ItemCount)
					{
						SetCellOK(columnIndex, "Item " + sequenceItemIndex.ToString()); 
					}
				}
			}
		}


		// Precondition: DicomAttribute.
		private void SetPresentCellForDicomBeginOrEndSequenceItem(AttributeList attributeList, int zeroBasedAttributesIndex, int sequenceItemIndex, bool isBeginItem)
		{
			int columnIndex = (int)this.columnIndexAttributePresent[zeroBasedAttributesIndex];

			DicomAttribute dicomAttribute = attributeList[zeroBasedAttributesIndex] as DicomAttribute;

			String presentText = "";

			if (dicomAttribute.IsPresent)
			{
				if (dicomAttribute.AttributeOnly.VR == VR.SQ)
				{
					if (sequenceItemIndex > dicomAttribute.AttributeOnly.ItemCount)
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

			if (attributeList.DicomContainsCompareSequenceItemsErrors(sequenceItemIndex))
			{
				SetCellError(columnIndex, presentText);
			}
			else
			{
				SetCellOK(columnIndex, presentText);
			}
		}

		internal void AddAttributeCollectionsInformationUsingDynamicCompare(AttributeCollections attributeCollections, CompareRules compareRules)
		{
			//
			// Iterate through all attributes of all AttributeSets.
			//


			GeneratorDynamicCompare generatorDynamicCompare = new GeneratorDynamicCompare(attributeCollections, compareRules);

			AttributeList attributeList = null;

			while ((attributeList = generatorDynamicCompare.GetNextAttributes()) != null)
			{
				AddAttributesInformation(attributeList);

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
        /// Add rows to the tables in which the results are displayed for comparing attribute collections
        /// using static compare.
        /// </summary>
        /// <param name="attributeCollections">The attribute collections.</param>
 		internal protected void AddAttributeCollectionsInformationUsingStaticDicomCompare(AttributeCollections attributeCollections)
		{
			//
			// Iterate through all attributes of all AttributeCollections. Only AttributeCollections containing
			// Dicom attributes will be taken into account.
			//

			GeneratorStaticDicomCompare generatorStaticDicomCompare = new GeneratorStaticDicomCompare(attributeCollections);

			AttributeList attributeList = null;
			TagSequence lowestTagSequence = null;

			while ((attributeList = generatorStaticDicomCompare.GetNextAttributes(out lowestTagSequence)) != null)
			{
				bool displayAttributes = true;

				if (!this.displayGroupLength)
				{
					if (lowestTagSequence.LastTag.ElementNumber == 0)
					{
						displayAttributes = false;
					}
				}
				
				if (displayAttributes)
				{
					AddAttributesInformation(attributeList);
				}
			}
		}

		private void AddAttributesInformation(AttributeList attributeList)
		{
			NewRow();


			//
			// Fill in the common information.
			//

			if (this.displayCommonTag)
			{
				SetTagCellForDicomAttribute(attributeList.LeadingDicomAttribute, this.columnIndexCommonTag);
			}

			if (this.displayCommonName)
			{
				SetNameCellForDicomAttribute(attributeList.LeadingDicomAttribute, this.columnIndexCommonName);
			}


			//
			// Fill in the information specific for each attribute.
			//

			for (int attributeIndex = 0; attributeIndex < attributeList.Count; attributeIndex++)
			{
				// Attribute Tag column.
				if (this.displayAttributeTag)
				{
					if (attributeList[attributeIndex] is DicomAttribute)
					{
						DicomAttribute dicomAttribute = attributeList[attributeIndex] as DicomAttribute;

						int columnIndex = (int)this.columnIndexAttributeTag[attributeIndex];

						SetTagCellForDicomAttribute(dicomAttribute, columnIndex);
					}
					else if (attributeList[attributeIndex] is Hl7Attribute)
					{
						SetTagCellForHl7Attribute(attributeList, attributeIndex);
					}
				}

				// Attribute Name column.
				if (this.displayAttributeName)
				{
					if (attributeList[attributeIndex] is DicomAttribute)
					{
						DicomAttribute dicomAttribute = attributeList[attributeIndex] as DicomAttribute;

						int columnIndex = (int)this.columnIndexAttributeName[attributeIndex];

						SetNameCellForDicomAttribute(dicomAttribute, columnIndex);
					}
				}

				// Attribute Present column.
				if (this.displayAttributePresent && (attributeList[attributeIndex] != null))
				{
					SetPresentCellForAttribute(attributeList, attributeIndex);
				}

				// Attribute VR column.
				if (this.displayAttributeVR)
				{
					if (attributeList[attributeIndex] is DicomAttribute)
					{
						SetVrCellForDicomAttribute(attributeList, attributeIndex);
					}
				}

				// Attribute Values column.
				if (this.displayAttributeValues && (attributeList[attributeIndex] != null))
				{
					SetValuesCellForAttribute(attributeList, attributeIndex);
				}
			}


			// Fill in the Comments.
			if (this.displayComments && (attributeList.CompareRule != null))
			{
				SetCommentsCell(attributeList.CompareRule);
			}


			//
			// If at least one attribute is a Dicom sequence attribute, do a comparison of the content.
			//

			if (attributeList.DicomContainsSequenceAttribute)
			{
				AddSequenceAttributesContentInformation(attributeList);
			}
		}

		internal CompareResults CreateCompareResults(int numberOfColumns, AttributeCollections attributeCollections, String tableDescription, StringCollection attributeSetDescriptions)
		{
			CompareResults compareResults = new CompareResults(numberOfColumns);
			compareResults.Table.CellItemSeperator = "<br>";
			compareResults.Table.EmptyCellPrefix = BACKGROUND_GREY;

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

			for (int attributeSetsIndex = 0; attributeSetsIndex < attributeCollections.Count; attributeSetsIndex++)
			{
				if (this.displayAttributeTag)
				{
					header1[index] = tableDescription;
					header2[index] = attributeSetDescriptions[attributeSetsIndex];
					header3[index] = TAG_STRING;
					columnWidths[index] = PIXEL_WIDTH_TAG;
					index++;
				}

				if (this.displayAttributeName && (attributeCollections[attributeSetsIndex] is DicomAttributeCollection))
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

				if (this.displayAttributeVR && (attributeCollections[attributeSetsIndex] is DicomAttributeCollection))
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

			if (this.displayComments)
			{
				header1[index] = tableDescription;
				header2[index] = "-";
				header3[index] = "Comments";
				columnWidths[index] = PIXEL_WIDTH_COMMENTS;
				index++;
			}

			compareResults.Table.AddHeader(header1);
			compareResults.Table.AddHeader(header2);
			compareResults.Table.AddHeader(header3);
			compareResults.Table.SetColumnPixelWidths(columnWidths);
			return(compareResults);
		}

		internal int DetermineColumnIndices(AttributeCollections attributeCollections)
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

			for (int index = 0; index < attributeCollections.Count; index++)
			{
				if (this.displayAttributeTag)
				{
					this.columnIndexAttributeTag.Add(columnIndex);
					columnIndex++;
				}

				if (this.displayAttributeName && (attributeCollections[index] is DicomAttributeCollection))
				{
					this.columnIndexAttributeName.Add(columnIndex);
					columnIndex++;
				}

				if (this.displayAttributePresent)
				{
					this.columnIndexAttributePresent.Add(columnIndex);
					columnIndex++;
				}

				if (this.displayAttributeVR && (attributeCollections[index] is DicomAttributeCollection))
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

			if (this.displayComments)
			{
				this.columnIndexComments = columnIndex;
				columnIndex++;
			}

			this.numberOfColumns = columnIndex - 1;

			return(columnIndex - 1);
		}




		/// <summary>
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
		/// Fill the cell with text indicating no error.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="text">The text.</param>
		private void SetCellOK(int column, String text)
		{
			this.compareResults.Table.AddBlackItem(column, text);
		}












































		internal String NAME_STRING = "Name";

		internal String TAG_STRING = "Tag";

		internal String PRESENT_STRING = "Pr";

		internal String VALUES_STRING = "Values";

		internal String VR_STRING = "VR";



		internal bool displayGroupLength = true;


		/// <summary>
		/// The field used to store the table that is the result of comparing.
		/// </summary>
		internal protected CompareResults compareResults = null;


		private bool addEmptyRowAfterEachDynamicComparedList = false;

        /// <summary>
        /// Sets a boolean indicating if an empty row should be added after each compared list.
        /// </summary>
		public bool AddEmptyRowAfterEachDynamicComparedList
		{
			set
			{
				this.addEmptyRowAfterEachDynamicComparedList = value;
			}
		}
























		














		private void AddSequenceItemRow(AttributeList attributeList, int itemIndex, bool isBeginItem)
		{
			NewRow();


			//
			// Common cells.
			//

			if (this.displayCommonTag)
			{
				SetTagCellForDicomBeginOrEndSequenceItem(attributeList.LeadingDicomAttribute, itemIndex, isBeginItem, this.columnIndexCommonTag);
			}

			if (this.displayCommonName)
			{
				SetNameCellForDicomBeginOrEndSequenceItem(attributeList.LeadingDicomAttribute, itemIndex, isBeginItem, this.columnIndexCommonName);
			}


			//
			// Attribute specific cells.
			//

			for (int attributeIndex = 0; attributeIndex < attributeList.Count; attributeIndex++)
			{
				if (attributeList[attributeIndex] is DicomAttribute)
				{
					DicomAttribute dicomAttribute = attributeList[attributeIndex] as DicomAttribute;

					if ((dicomAttribute.ValidationRule.Flags & FlagsBase.Include_sequence_items) == FlagsBase.Include_sequence_items)
					{
						if (this.displayAttributeTag)
						{
							int columnIndex = (int)this.columnIndexAttributeTag[attributeIndex];

							SetTagCellForDicomBeginOrEndSequenceItem(dicomAttribute, itemIndex, isBeginItem, columnIndex);
						}

						if (this.displayAttributeName)
						{
							int columnIndex = (int)this.columnIndexAttributeName[attributeIndex];

							SetNameCellForDicomBeginOrEndSequenceItem(dicomAttribute, itemIndex, isBeginItem, columnIndex);
						}

						if (this.displayAttributePresent)
						{
							SetPresentCellForDicomBeginOrEndSequenceItem(attributeList, attributeIndex, itemIndex, isBeginItem);
						}
					}
				}
			}
		}







		private void AddSequenceAttributesContentInformation(AttributeList attributeList)
		{
			//
			// Determine the maximum amount of sequence items for all supplied sequence attributes
			// that have the include_sequence_items flag enabled.
			//

			int maxItemCount = attributeList.DicomMaxItemCount;
			

			//
			// Fill the table with the information.
			//

			for (int itemIndex = 1; itemIndex <= maxItemCount; itemIndex++)
			{
				//
				// Add begin item row.
				//

				AddSequenceItemRow(attributeList, itemIndex, true);

				
				//
				// Add the comparison of attributes within the sequence items.
				//
	
				AttributeCollections attributeCollections = new AttributeCollections();

				for (int attributeIndex = 0; attributeIndex < attributeList.Count; attributeIndex++)
				{
					if (attributeList[attributeIndex] == null)
					{
						attributeCollections.AddNull();
					}
					else if (attributeList[attributeIndex] is Hl7Attribute)
					{
						attributeCollections.AddNull();
					}
					else if (attributeList[attributeIndex] is DicomAttribute)
					{
						DicomAttribute dicomAttribute = attributeList[attributeIndex] as DicomAttribute;

						if ((dicomAttribute.ValidationRule.Flags & FlagsBase.Include_sequence_items) == FlagsBase.Include_sequence_items)
						{
							if (dicomAttribute.AttributeOnly is ValidAttribute)
								// Attribute is valid attribute.
							{
								ValidAttribute validAttribute = dicomAttribute.AttributeOnly as ValidAttribute;

								if (validAttribute.VR == VR.SQ)
									// Attribute is valid SQ attribute.
								{
									if (validAttribute.ItemCount < itemIndex)
									{
										// Item index to high for actual item count.
										attributeCollections.Add(new DicomAttributeCollection(new SequenceItem(), dicomAttribute.ValidationRule.Flags));
									}
									else
										// Item exists.
									{
										attributeCollections.Add(new DicomAttributeCollection(validAttribute.GetItem(itemIndex), dicomAttribute.ValidationRule.Flags));
									}
								}
								else
									// Attribute is valid attribute but not a SQ attribute.
								{
									attributeCollections.Add(new DicomAttributeCollection(new SequenceItem(), dicomAttribute.ValidationRule.Flags));
								}
							}
							else
								// Attribute does not exist.
							{
								attributeCollections.Add(new DicomAttributeCollection(new SequenceItem(), dicomAttribute.ValidationRule.Flags));
							}
						}
						else
							// Flags specify sequence items must not be included.
						{
							attributeCollections.AddNull();
						}
					}
				}

				AddAttributeCollectionsInformationUsingStaticDicomCompare(attributeCollections);


				//
				// Add end item row.
				//

				AddSequenceItemRow(attributeList, itemIndex, false);			
				
			}
		}
	}
}
