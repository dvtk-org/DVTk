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

using AttributeSet = DvtkHighLevelInterface.Dicom.Other.AttributeSet;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// <br></br>
    /// For now, this class contains one method to compare two attribute sets. The result is a Table
	/// in which for all attributes in the two datasets, one row is present describing if they are equal
	/// with respect to the attribute or not.
	/// </summary>
	public class Compare
	{
		//
		// - Constant fields -
		//

		private String BACKGROUND_GREY = "bgcolor=\"#BBBBBB\"";

		private String BACKGROUND_RED = "bgcolor=\"#FF0000\"";

		private int PIXEL_WIDTH_TAG = 120;

		private int PIXEL_WIDTH_NAME = 260;

		private int PIXEL_WIDTH_PRESENT = 15;

		private int PIXEL_WIDTH_VR = 30;

		private int PIXEL_WIDTH_VALUES = 180;



		//
		// - Fields -
		//

		/// <summary>
		/// Column index to use for the attribute name.
		/// </summary>
		private int columnIndexName = -1;

		/// <summary>
		/// Column index to use for the attribute1 presence.
		/// </summary>
		private int columnIndexPresent1 = -1;

		/// <summary>
		/// Column index to use for the attribute2 presence.
		/// </summary>
		private int columnIndexPresent2 = -1;
		
		/// <summary>
		/// Column index to use for the attribute tag.
		/// </summary>
		private int columnIndexTag = -1;

		/// <summary>
		/// Column index to use for the attribute1 values.
		/// </summary>
		private int columnIndexValues1 = -1;

		/// <summary>
		/// Column index to use for the attribute2 values .
		/// </summary>
		private int columnIndexValues2 = -1;

		/// <summary>
		/// Column index to use for the attribute1 VR.
		/// </summary>
		private int columnIndexVr1 = -1;

		/// <summary>
		/// Column index to use for the attribute2 VR.
		/// </summary>
		private int columnIndexVr2 = -1;

		/// <summary>
		/// See property differencesCount.
		/// </summary>
		private uint differencesCount = 0;

		/// <summary>
		/// See property DisplayVR.
		/// </summary>
		private bool displayVR = true;

		/// <summary>
		/// Dummy data set containing only the dummyEmptySequenceAttribute.
		/// </summary>
		private DataSet dummyDataSet = null;

		/// <summary>
		/// Dummy sequence attribute containing no sequence items.
		/// </summary>
		private Attribute dummyEmptySequenceAttribute = null;


		/// <summary>
		/// The field used to store the table that is the result of comparing.
		/// </summary>
		private Table table = null;



		//
		// - Constructors -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Default constructor.
		/// </summary>
		public Compare()
		{
			// Add a dummy sequence attribute containing no sequence items.
			this.dummyDataSet = new DataSet();
			dummyDataSet.Set("0x00800080", VR.SQ);
			this.dummyEmptySequenceAttribute = dummyDataSet["0x00800080"];
		}



		//
		// - Properties -
		//

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		public uint DifferencesCount
		{
			get
			{
				return(this.differencesCount);
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		public bool DisplayVR
		{
			get
			{
				return(this.displayVR);
			}
			set
			{
				this.displayVR = value;
			}
		}



		//
		// - Methods -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Add attribute information for one attribute in the current row.
		/// </summary>
		/// <param name="sourceAttribute">The attribute for which the information needs to be displayed.</param>
		/// <param name="referenceAttribute">The attribute to compare with.</param>
		/// <param name="isFromFirstAttributeSet">Indicates if the attribute is from the first attribute set.</param>
		/// <returns>Indicates if an compare error was encountered.</returns>
		private bool AddAttributeInformation(ValidAttribute sourceAttribute, ValidAttribute referenceAttribute, bool isFromFirstAttributeSet)
		{
			bool areEqual = true;
			int columnIndexPresent = 0;
			int columnIndexVR = 0;
			int columnIndexValues = 0;

			if (isFromFirstAttributeSet)
			{
				columnIndexPresent = this.columnIndexPresent1;
				columnIndexVR = this.columnIndexVr1;
				columnIndexValues = this.columnIndexValues1;
			}
			else
			{
				columnIndexPresent = this.columnIndexPresent2;
				columnIndexVR = this.columnIndexVr2;
				columnIndexValues = this.columnIndexValues2;
			}

			if (sourceAttribute == null)
				// source attribute is not present, reference attribute is.
			{
				SetCellError(columnIndexPresent, "-", true);
				SetCellNotApplicable(columnIndexVR, this.displayVR);
				SetCellNotApplicable(columnIndexValues, true);
				areEqual = false;
			}
			else
				// Source attribute is present.
			{
				if (referenceAttribute == null)
					// Source attribute is present, reference attribute is not.
				{
					SetCellError(columnIndexPresent, "+", true);
					SetCellOK(columnIndexVR, sourceAttribute.VR.ToString(), this.displayVR);
					SetCellOK(columnIndexValues, sourceAttribute.Values.ToString(), true);
					areEqual = false;
				}
				else
					// Source attribute and reference attribute are both present.
				{
					SetCellOK(columnIndexPresent, "+", true);

					if (sourceAttribute.VR == referenceAttribute.VR)
					{
						SetCellOK(columnIndexVR, sourceAttribute.VR.ToString(), this.displayVR);
					}
					else
					{
						SetCellError(columnIndexVR, sourceAttribute.VR.ToString(), this.displayVR);
						areEqual = false;
					}

					if (sourceAttribute.Values.Equals(referenceAttribute.Values))
					{
						SetCellOK(columnIndexValues, sourceAttribute.Values.ToString(), true);
					}
					else
					{
						SetCellError(columnIndexValues, sourceAttribute.Values.ToString(), true);
						areEqual = false;
					}
				}
			}

			return(areEqual);
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Add a row containing the result of comparing the two supplied attributes.
		/// </summary>
		/// <param name="attribute1">Attribute 1.</param>
		/// <param name="attribute2">Attribute 2.</param>
        /// <param name="level">-</param>
        private void AddAttributesComparison(ValidAttribute attribute1, ValidAttribute attribute2, int level)
		{
			table.NewRow();


			//
			// Fill in the general tag information.
			//

			ValidAttribute attribute = null;

			if (attribute1 != null)
			{
				attribute = attribute1;
			}
			else
			{
				attribute = attribute2;
			}

			String name = "-";

			if ((attribute.Name != "") && !(attribute.Name.StartsWith(" : private mapped to")))
			{
				name = "\"" + attribute.Name + "\"";
			}

			SetCellOK(this.columnIndexTag, attribute.TagSequence.DicomNotation, true);
			SetCellOK(this.columnIndexName, name, true);
			

			//
			// Fill in the information for the two attributes.
			//

			bool areEqual = true;

			areEqual = AddAttributeInformation(attribute1, attribute2, true);
			AddAttributeInformation(attribute2, attribute1, false);


			//
			// Find out if at least one of the attributes is a sequence attribute.
			//

			bool containsAtLeastOneSequenceAttribute = false;

			if (attribute1 != null)
			{
				if (attribute1.VR == VR.SQ)
				{
					containsAtLeastOneSequenceAttribute = true;
				}
			}

			if (attribute2 != null)
			{
				if (attribute2.VR == VR.SQ)
				{
					containsAtLeastOneSequenceAttribute = true;
				}
			}


			//
			// If at least one attribute is a sequence attribute, do a comparison of the content.
			// If the other attribute is null or is not a sequence attribute, use an
			// empty sequence attribute instead.
			//

			if (containsAtLeastOneSequenceAttribute)
			{
				Attribute sequenceAttribute1 = ConvertToSequenceAttribute(attribute1);
				Attribute sequenceAttribute2 = ConvertToSequenceAttribute(attribute2);

				AddSequenceAttributesContentComparison(sequenceAttribute1, sequenceAttribute2, level);
			}


			//
			// Update the differences count.
			//

			if (!areEqual)
			{
				this.differencesCount++;
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Add the results of comparing the two AttributeSets (possibly within multiple nested Sequence Attributes)
		/// to the the already filled table.
		/// </summary>
		/// <param name="attributeSet1">The first AttributeSet.</param>
		/// <param name="attributeSet2">The second AttributeSet.</param> 
        /// <param name="level">-</param>  
		private void AddAttributeSetsComparison(AttributeSet attributeSet1, AttributeSet attributeSet2, int level)
		{
			//
			// Iterate through all attributes of both AttributeSets at the same time.
			//

			int index1 = 0;
			int index2 = 0;

			while ((index1 < attributeSet1.Count) || (index2 < attributeSet2.Count))
			{
				if (index1 >= attributeSet1.Count)
					// No more attributes left in attributeSet1.
				{
					ValidAttribute validAttribute2 = attributeSet2[index2] as ValidAttribute;

					AddAttributesComparison(null, validAttribute2, level);

					index2++;
				}
				else if (index2 >= attributeSet2.Count)
					// No more attributes left in attributeSet2.
				{
					ValidAttribute validAttribute1 = attributeSet1[index1] as ValidAttribute;

					AddAttributesComparison(validAttribute1, null, level);

					index1++;
				}
				else
					// Still attributes left in both AttributeSets.
				{
					ValidAttribute validAttribute1 = attributeSet1[index1] as ValidAttribute;
					ValidAttribute validAttribute2 = attributeSet2[index2] as ValidAttribute;

					Tag lastTag1 = validAttribute1.TagSequence[validAttribute1.TagSequence.Tags.Count - 1] as Tag;
					Tag lastTag2 = validAttribute2.TagSequence[validAttribute2.TagSequence.Tags.Count - 1] as Tag;

					if (lastTag1.AsUInt32 < lastTag2.AsUInt32)
						// Attribute not present in AttributeSet2.
					{
						AddAttributesComparison(validAttribute1, null, level);

						index1++;
					}
					else if (lastTag1.AsUInt32 > lastTag2.AsUInt32)
						// Attribute not present in AttributeSet1.
					{
						AddAttributesComparison(null, validAttribute2, level);

						index2++;
					}
					else
						// Attribute exists in both AttributeSets.
					{
						AddAttributesComparison(validAttribute1, validAttribute2, level);

						index1++;
						index2++;
					}
				}
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        /// <param name="sequenceAttribute1">-</param>
        /// <param name="sequenceAttribute2">-</param>
        /// <param name="level">-</param>
		private void AddSequenceAttributesContentComparison(Attribute sequenceAttribute1, Attribute sequenceAttribute2, int level)
		{
			int maxItemCount = Math.Max(sequenceAttribute1.ItemCount, sequenceAttribute2.ItemCount);

			for (int itemIndex = 1; itemIndex <= maxItemCount; itemIndex++)
			{
				//
				// Get the two sequence items to compare. If one sequence item doesn't
				// exist for the itemIndex, use an empty sequence item.
				//

				SequenceItem sequenceItem1 = null;
				SequenceItem sequenceItem2 = null;
				bool sequenceItem1Present = true;
				bool sequenceItem2Present = true;


				if (itemIndex <= sequenceAttribute1.ItemCount)
				{
					sequenceItem1 = sequenceAttribute1.GetItem(itemIndex);
					sequenceItem1Present = true;
				}
				else
				{
					sequenceItem1 = new SequenceItem();
					sequenceItem1Present = false;
				}

				if (itemIndex <= sequenceAttribute2.ItemCount)
				{
					sequenceItem2 = sequenceAttribute2.GetItem(itemIndex);
					sequenceItem2Present = true;
				}
				else
				{
					sequenceItem2 = new SequenceItem();
					sequenceItem2Present = false;
				}


				//
				// Write the BEGIN ITEM row, compare the two sequence items and write the END ITEM row.
				//

				AddBeginOrEndSequenceItem(sequenceItem1Present, sequenceItem2Present, itemIndex, level + 1, true);

				AddAttributeSetsComparison(sequenceItem1, sequenceItem2, level + 1);

				AddBeginOrEndSequenceItem(sequenceItem1Present, sequenceItem2Present, itemIndex, level + 1, false);			


				//
				// Update the differences count.
				//

				if ((!sequenceItem1Present) || (!sequenceItem2Present))
				{
					this.differencesCount++;
				}
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Compare two attributeSets by putting the compare results in a Table.
		/// </summary>
		/// <param name="tableDescription">The description of the complete table (put in the first table header).</param>
		/// <param name="attributeSet1">The first AttributeSet.</param>
		/// <param name="attributeSet1Description">Description of the first AttributeSet (put in the second table header).</param>
		/// <param name="attributeSet2">The second AttributeSet.</param>
		/// <param name="attributeSet2Description">Description of the second AttributeSet (put in the second table header).</param>
		/// <returns>The result of comparing the two AttributeSets.</returns>
		public Table CompareAttributeSets(String tableDescription, AttributeSet attributeSet1, String attributeSet1Description, AttributeSet attributeSet2, String attributeSet2Description)
		{
			// Start with a new empty table.
			this.table = null; 
			this.differencesCount = 0;

			// To be able to compare correctly, we first make sure the attributes in both attributesets are ascending.
			attributeSet1.MakeAscending(true);
			attributeSet2.MakeAscending(true);
			
			// Set the correct column indices, headers and pixel widths.
			columnIndexTag = 1;
			columnIndexName = 2;

			if (this.displayVR)
			{
				this.table = new Table(8);

				columnIndexPresent1 = 3;
				columnIndexVr1 = 4;
				columnIndexValues1 = 5;
				columnIndexPresent2 = 6;
				columnIndexVr2 = 7;		
				columnIndexValues2 = 8;

				// Set the headers of the table.
				this.table.AddHeader(tableDescription, tableDescription, tableDescription, tableDescription, tableDescription, tableDescription, tableDescription, tableDescription);
				this.table.AddHeader("Attribute Info", "Attribute Info", attributeSet1Description, attributeSet1Description, attributeSet1Description, attributeSet2Description, attributeSet2Description, attributeSet2Description);
				this.table.AddHeader("Tag", "Name", "Pr", "VR", "Values", "Pr", "VR", "Values");

				// If the caller of this methods wants to display the table as HTML, then these are already good
				// default values for the widths of the columns.
				this.table.SetColumnPixelWidths(PIXEL_WIDTH_TAG, PIXEL_WIDTH_NAME, PIXEL_WIDTH_PRESENT, PIXEL_WIDTH_VR, PIXEL_WIDTH_VALUES, PIXEL_WIDTH_PRESENT, PIXEL_WIDTH_VR, PIXEL_WIDTH_VALUES);			
			}
			else
			{
				this.table = new Table(6);

				columnIndexPresent1 = 3;
				columnIndexValues1 = 4;
				columnIndexPresent2 = 5;	
				columnIndexValues2 = 6;

				// Set the headers of the table.
				this.table.AddHeader(tableDescription, tableDescription, tableDescription, tableDescription, tableDescription, tableDescription);
				this.table.AddHeader("Attribute Info", "Attribute Info", attributeSet1Description, attributeSet1Description, attributeSet2Description, attributeSet2Description);
				this.table.AddHeader("Tag", "Name", "Pr", "Values", "Pr", "Values");

				// If the caller of this methods wants to display the table as HTML, then these are already good
				// default values for the widths of the columns.
				this.table.SetColumnPixelWidths(PIXEL_WIDTH_TAG, PIXEL_WIDTH_NAME, PIXEL_WIDTH_PRESENT, PIXEL_WIDTH_VALUES, PIXEL_WIDTH_PRESENT, PIXEL_WIDTH_VALUES);
			}

			// Do the actual comparison.
			AddAttributeSetsComparison(attributeSet1, attributeSet2, 0);

			return(this.table);
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// When the supplied attribute is a sequence attribute, return it.
		/// If it is not a sequence attribute or does not exist, return a dummy sequence
		/// attribute containing no items.
		/// </summary>
		/// <param name="attribute">The attribute.</param>
		/// <returns>A (dummy) sequence attribute.</returns>
		private Attribute ConvertToSequenceAttribute(Attribute attribute)
		{
			Attribute sequenceAttribute = null;
			
			if (attribute == null)
				// Attribute does not exist.
			{
				sequenceAttribute = this.dummyEmptySequenceAttribute;
			}
			else
			{
				if (attribute.VR == VR.SQ)
					// Attribute is a sequence attribute.
				{
					sequenceAttribute = attribute;
				}
				else
					// Attribute is not a sequence attribute.
				{
					sequenceAttribute = this.dummyEmptySequenceAttribute;
				}
			}

			return(sequenceAttribute);
		}

		private void AddBeginOrEndSequenceItem(bool sequenceItem1Present, bool sequenceItem2Present, int sequenceItemIndex, int level, bool isBeginItem)
		{
			String sequenceItemDescription;

			this.table.NewRow();

			// Set the tag column.
			if (isBeginItem)
			{
				SetCellOK(this.columnIndexTag, "".PadRight(level + 1, '>') + "BEGIN ITEM", true);
			}
			else
			{
				SetCellOK(this.columnIndexTag, "".PadRight(level + 1, '>') + "END ITEM", true);
			}

			// Set the name column.
			if (isBeginItem)
			{
				sequenceItemDescription = "Begin of sequence item " + sequenceItemIndex.ToString();
			}
			else
			{
				sequenceItemDescription = "End of sequence item " + sequenceItemIndex.ToString();
			}
			SetCellOK(this.columnIndexName, sequenceItemDescription, true);

			// Set the present columns.
			if (sequenceItem1Present)
			{
				if (sequenceItem2Present)
					// Both sequence items are present.
				{
					SetCellOK(this.columnIndexPresent1, "+", true);
					SetCellOK(this.columnIndexPresent2, "+", true);
				}
				else
					// Sequence item 1 present but sequence item 2 not present.
				{
					SetCellError(this.columnIndexPresent1, "+", true);
					SetCellError(this.columnIndexPresent2, "-", true);
				}
			}
			else
			{
				if (sequenceItem2Present)
					// Sequence item 2 present but sequence item 1 not present.
				{
					SetCellError(this.columnIndexPresent1, "-", true);
					SetCellError(this.columnIndexPresent2, "+", true);
				}
				else
				{
					// Sanity check, may not occur.
					throw new HliException("Both sequence items not present.");
				}
			}

			// Set the VR columns.
			SetCellNotApplicable(this.columnIndexVr1, this.displayVR);
			SetCellNotApplicable(this.columnIndexVr2, this.displayVR);

			// Set the Values columns.
			SetCellNotApplicable(this.columnIndexValues1, true);
			SetCellNotApplicable(this.columnIndexValues2, true);	
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Fill the cell with text indicating error.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="text">The text.</param>
		/// <param name="enabled">If true, the cell is set. If false, nothing happens.</param>
		private void SetCellError(int column, String text, bool enabled)
		{
			if (enabled)
			{
				this.table.AddBlackItem(column, text);
				this.table.SetCellPrefix(column, BACKGROUND_RED);
			}
		}
		
		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Set the cell content to indicate that it is not relevant.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="enabled">If true, the cell is set. If false, nothing happens.</param>
		private void SetCellNotApplicable(int column, bool enabled)
		{
			if (enabled)
			{
				this.table.SetCellPrefix(column, BACKGROUND_GREY);
			}
		}
		
		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
		/// Fill the cell with text indicating no error.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <param name="text">The text.</param>
		/// <param name="enabled">If true, the cell is set. If false, nothing happens.</param>
		private void SetCellOK(int column, String text, bool enabled)
		{
			if (enabled)
			{
				this.table.AddBlackItem(column, text);
			}
		}
	}
}
