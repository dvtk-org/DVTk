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

using ThreeStateBoolean = DvtkHighLevelInterface.Common.Other.ThreeStateBoolean;
using VR = DvtkData.Dimse.VR;
using Dvtk.CommonDataFormat;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Compare
{
	/// <summary>
	/// A list of attributes (including validation rule) that needs to be displayed/validated together.
	/// </summary>
	internal class AttributeList
	{

		//
		// - Fields -
		//

		/// <summary>
		/// Internal list that stores the ordered list of attributes to validate together.
		/// </summary>
		private ArrayList attributeList = new ArrayList();

		/// <summary>
		/// See property CompareRule.
		/// </summary>
		private CompareRule compareRule = null;

		/// <summary>
		/// See property ContainsComparePresentErrors.
		/// </summary>
		private ThreeStateBoolean containsComparePresentErrors = ThreeStateBoolean.UNKNOWN;

		/// <summary>
		/// See property ContainsCompareSequenceItemsErrors
		/// </summary>
		private ArrayList dicomContainsCompareSequenceItemsErrors = new ArrayList();

		/// <summary>
		/// See property ContainsCompareValuesErrors.
		/// </summary>
		private ThreeStateBoolean containsCompareValuesErrors = ThreeStateBoolean.UNKNOWN;

		/// <summary>
		/// See property ContainsCompareVRErrors.
		/// </summary>
		private ThreeStateBoolean dicomContainsCompareVRErrors = ThreeStateBoolean.UNKNOWN;

		/// <summary>
		/// Used to make sure that the logic for determining the ContainsCompareSequenceItemsErrors
		/// is only performed once.
		/// </summary>
		private bool isDicomContainsCompareSequenceItemsErrorsDetermined = false;



		//
		// - Constructors -
		//

		/// <summary>
		/// Constructor creating an empty list.
		/// </summary>
		public AttributeList()
		{
			// Do nothing.
		}



		//
		// - Properties -
		//

		/// <summary>
		/// Gets or sets the compare rule used to generate this instance.
		/// </summary>
		/// <remarks>
		/// If no compare rule is used to generate this instance, null is returned.
		/// </remarks>
		public CompareRule CompareRule
		{
			get
			{
				return(this.compareRule);
			}
			set
			{
				this.compareRule = value;
			}
		}

		/// <summary>
		/// Indicates if a difference exists in existence (yes or no) for all attributes that have
		/// the flag Compare_present enabled.
		/// </summary>
		public bool ContainsComparePresentErrors
		{
			get
			{
				if (this.containsComparePresentErrors == ThreeStateBoolean.UNKNOWN)
				{
					ArrayList attributesToCompare = Filter(FilterOnNonEmpty(this.attributeList), FlagsBase.Compare_present);

					int numberOfAttributesPresent = FilterOnPresent(attributesToCompare).Count; // Only count the attributes with the Compare_present flag enabled.
					int numberOfAttributesNotPresent = attributesToCompare.Count - numberOfAttributesPresent; // Only count the attributes with the Compare_present flag enabled.

					if ((numberOfAttributesPresent > 0) && (numberOfAttributesNotPresent > 0))
					{
						this.containsComparePresentErrors = ThreeStateBoolean.TRUE;
					}
					else
					{
						this.containsComparePresentErrors = ThreeStateBoolean.FALSE;
					}
				}

				return(this.containsComparePresentErrors == ThreeStateBoolean.TRUE);
			}
		}

		/// <summary>
		/// Needed to be able to tell if a present error exists for a specific sequence item.
		/// </summary>
		/// <param name="oneBasedSequenceItemIndex">One based sequence item index.</param>
		/// <returns>Boolean indicating if there is a present error.</returns>
		public bool DicomContainsCompareSequenceItemsErrors(int oneBasedSequenceItemIndex)
		{
			if (!this.isDicomContainsCompareSequenceItemsErrorsDetermined)
			{
				int maxItemCount = DicomMaxItemCount;

				ArrayList attributesToCompare = Filter(FilterOnDicomAttribute(FilterOnNonEmpty(this.attributeList)), FlagsBase.Compare_present);

				for (int sequenceItemIndex = 1; sequenceItemIndex <= maxItemCount; sequenceItemIndex++)
				{
					int sequenceItemsPresent = 0;
					int sequenceItemsNotPresent = 0;

					foreach (DicomAttribute dicomAttribute in attributesToCompare)
					{
						if (dicomAttribute.AttributeOnly is ValidAttribute)
						{
							if (dicomAttribute.AttributeOnly.VR == VR.SQ)
							{
								if (dicomAttribute.AttributeOnly.ItemCount < sequenceItemIndex)
								{
									sequenceItemsNotPresent++;
								}
								else
								{
									sequenceItemsPresent++;
								}
							}
							else
							{
								sequenceItemsNotPresent++;
							}
						}
						else
						{
							sequenceItemsNotPresent++;
						}	
					}

					if ((sequenceItemsPresent > 0) && (sequenceItemsNotPresent > 0))
					{
						this.dicomContainsCompareSequenceItemsErrors.Add(true);
					}
					else
					{
						this.dicomContainsCompareSequenceItemsErrors.Add(false);
					}
				}

				this.isDicomContainsCompareSequenceItemsErrorsDetermined = true;
			}

			return((bool)this.dicomContainsCompareSequenceItemsErrors[oneBasedSequenceItemIndex - 1]);
		}

		/// <summary>
		/// For all attributes present that have the Compare_values flag, find out if differences
		/// exist for their values.
		/// </summary>
		public bool ContainsCompareValuesErrors
		{
			get
			{
				if (this.containsCompareValuesErrors == ThreeStateBoolean.UNKNOWN)
				{
					this.containsCompareValuesErrors = ThreeStateBoolean.FALSE;

					ArrayList attributesToCompare = FilterOnPresent(Filter(FilterOnNonEmpty(this.attributeList), FlagsBase.Compare_values));

					if (attributesToCompare.Count > 1)
					{
						ArrayList dicomObOwOfAttributes = FilterOnDicomOBOWOF(attributesToCompare);

						if (dicomObOwOfAttributes.Count == 0)
							// Contains no Dicom attributes with VR OB, OW or OF.
						{
							if (this.compareRule.CompareValueType == CompareValueTypes.Identical)
								// Do a identical compare of all values.
							{

								AttributeBase firstAttributeToCompare = attributesToCompare[0] as AttributeBase;

								for (int index = 1; index < attributesToCompare.Count; index++)
								{
									AttributeBase otherAttributeToCompare = attributesToCompare[index] as AttributeBase;

									if (firstAttributeToCompare.ValuesToString() != otherAttributeToCompare.ValuesToString())
									{
										this.containsCompareValuesErrors = ThreeStateBoolean.TRUE;
										break;
									}
								}
							}
							else
							{
								// Do a compare of all values using the Common Data Format.
								BaseCommonDataFormat firstCommonDataFormat = CreateCommonDateFormat(attributesToCompare[0] as AttributeBase);

								for (int index = 1; index < attributesToCompare.Count; index++)
								{
									BaseCommonDataFormat otherCommonDataFormat = CreateCommonDateFormat(attributesToCompare[index] as AttributeBase);

									if (!firstCommonDataFormat.Equals(otherCommonDataFormat))
									{
										this.containsCompareValuesErrors = ThreeStateBoolean.TRUE;
										break;
									}
								}
							}
						}
						else if (dicomObOwOfAttributes.Count != attributesToCompare.Count)
							// Contains both Dicom attributes with VR OB, OW or OF and other types of attributes.
						{
							this.containsCompareValuesErrors = ThreeStateBoolean.TRUE;
						}
						else
							// Contains only Dicom attributes with VR OB, OW or OF.
						{
							DicomAttribute firstDicomAttributeToCompare = attributesToCompare[0] as DicomAttribute;

							for (int index = 1; index < attributesToCompare.Count; index++)
							{
								DicomAttribute otherDicomAttributeToCompare = attributesToCompare[index] as DicomAttribute;

								if (!firstDicomAttributeToCompare.AttributeOnly.Values.Equals(otherDicomAttributeToCompare.AttributeOnly.Values))
								{
									this.containsCompareValuesErrors = ThreeStateBoolean.TRUE;
									break;
								}
							}
						}
					}
				}

				return(this.containsCompareValuesErrors == ThreeStateBoolean.TRUE);
			}
		}

		/// <summary>
		/// For all attributes present that have the Compare_VR flag and are present, find out if differences
		/// exist for their VR.
		/// </summary>
		public bool DicomContainsCompareVRErrors
		{
			get
			{
				if (this.dicomContainsCompareVRErrors == ThreeStateBoolean.UNKNOWN)
				{
					this.dicomContainsCompareVRErrors = ThreeStateBoolean.FALSE;

					ArrayList attributesToCompare = FilterOnPresent(Filter(FilterOnNonEmpty(this.attributeList), FlagsBase.Compare_VR));

					if (attributesToCompare.Count > 1)
					{
						// The flag Compare_VR is only available for DicomAttributes.
						DicomAttribute firstAttributeToCompare = attributesToCompare[0] as DicomAttribute;

						for (int index = 1; index < attributesToCompare.Count; index++)
						{
							DicomAttribute otherAttributeToCompare = attributesToCompare[index] as DicomAttribute;

							if (firstAttributeToCompare.AttributeOnly.VR != otherAttributeToCompare.AttributeOnly.VR)
							{
								this.dicomContainsCompareVRErrors = ThreeStateBoolean.TRUE;
								break;
							}
						}
					}
				}

				return(this.dicomContainsCompareVRErrors == ThreeStateBoolean.TRUE);
			}
		}

		/// <summary>
		/// Number of attributes.
		/// </summary>
		internal int Count
		{
			get
			{
				return(this.attributeList.Count);
			}
		}

		/// <summary>
		/// Indicates if this collection contains at least one sequence attribute.
		/// </summary>
		public bool DicomContainsSequenceAttribute
		{
			get
			{
				bool containsSequenceAttribute = false;

				ArrayList attributesToCheck = FilterOnPresent(FilterOnDicomAttribute(FilterOnNonEmpty(this.attributeList)));

				foreach(DicomAttribute dicomAttribute in attributesToCheck)
				{
					if (dicomAttribute.AttributeOnly.VR == VR.SQ)
					{
						containsSequenceAttribute = true;
						break;
					}
				}

				return(containsSequenceAttribute);
			}
		}

		/// <summary>
		/// Property to get the maximum number of items in any Dicom sequence attribute
		/// that have t he include_sequence_items flag enabled.
		/// </summary>
		public int DicomMaxItemCount
		{
			get
			{
				int maxItemCount = 0;

				ArrayList attributesToCheck = Filter(FilterOnPresent(FilterOnDicomAttribute(FilterOnNonEmpty(this.attributeList))), FlagsBase.Include_sequence_items);
			
				foreach (DicomAttribute dicomAttribute in attributesToCheck)
				{
					DvtkHighLevelInterface.Dicom.Other.Attribute attributeOnly = dicomAttribute.AttributeOnly;

					if (attributeOnly.VR == VR.SQ)
					{
						maxItemCount = Math.Max(maxItemCount, attributeOnly.ItemCount);
					}
				}

				return(maxItemCount);
			}
		}



		public DicomAttribute LeadingDicomAttribute
		{
			get
			{
				DicomAttribute leadingDicomAttribute = null;

				ArrayList dicomAttributes = FilterOnDicomAttribute(FilterOnNonEmpty(this.attributeList));

				if (dicomAttributes.Count > 0)
				{
					leadingDicomAttribute = dicomAttributes[0] as DicomAttribute;
				}
				else
				{
					leadingDicomAttribute = null;
				}

				return(leadingDicomAttribute);
			}
		}

















		/// <summary>
		/// Property to get a specific attribute.
		/// </summary>
		internal AttributeBase this[int zeroBasedIndex]
		{
			get
			{
				if ((zeroBasedIndex < 0) || (zeroBasedIndex >= Count))
				{
					throw new HliException("Wrong index used for CompareRule.");
				}
				
				return(this.attributeList[zeroBasedIndex] as AttributeBase);
			}
		}



		//
		// - Methods -
		//

		/// <summary>
		/// Add an attribute to this list.
		/// </summary>
		/// <param name="attributeBase">The attribute to add.</param>
		public void Add(AttributeBase attributeBase)
		{
			this.attributeList.Add(attributeBase);
		}

		/// <summary>
		/// Filter on a given flag.
		/// </summary>
		/// <param name="attributes">The attributes to filter.</param>
		/// <param name="flag">The flag to filter on.</param>
		/// <returns>The attributes that have the supplied flag.</returns>
		private static ArrayList Filter(ArrayList attributes, FlagsBase flag)
		{
			ArrayList filteredAttributes = new ArrayList();

			foreach(AttributeBase attribute in attributes)
			{
				if ((attribute.ValidationRule.Flags & flag) == flag)
				{
					filteredAttributes.Add(attribute);
				}
			}

			return(filteredAttributes);
		}

		/// <summary>
		/// Filter on the attributes being a DicomAttribute.
		/// </summary>
		/// <param name="attributes">The attributes to filter.</param>
		/// <returns>The attribute that are of type DicomAttribute.</returns>
		private static ArrayList FilterOnDicomAttribute(ArrayList attributes)
		{
			ArrayList filteredAttributes = new ArrayList();

			foreach(AttributeBase attribute in attributes)
			{
				if (attribute is DicomAttribute)
				{
					filteredAttributes.Add(attribute);
				}
			}

			return(filteredAttributes);
		}




		private static ArrayList FilterOnDicomOBOWOF(ArrayList attributes)
		{
			ArrayList filteredAttributes = new ArrayList();

			foreach(AttributeBase attribute in attributes)
			{
				if (attribute is DicomAttribute)
				{
					DicomAttribute dicomAttribute = attribute as DicomAttribute;

					if ((dicomAttribute.AttributeOnly.VR == VR.OB) || (dicomAttribute.AttributeOnly.VR == VR.OW)
                        || (dicomAttribute.AttributeOnly.VR == VR.OF) || (dicomAttribute.AttributeOnly.VR == VR.OV))
					{
						filteredAttributes.Add(attribute);
					}
				}
			}

			return(filteredAttributes);
		}







		/// <summary>
		/// Filter on the presence of actual the attribute.
		/// </summary>
		/// <param name="attributes">The attributes to filter.</param>
		/// <returns>The attributes containing actuals attributes that are present.</returns>
		private static ArrayList FilterOnPresent(ArrayList attributes)
		{
			ArrayList filteredAttributes = new ArrayList();

			foreach(AttributeBase attribute in attributes)
			{
				if (attribute.IsPresent)
				{
					filteredAttributes.Add(attribute);
				}
			}

			return(filteredAttributes);
		}








		private static ArrayList FilterOnNonEmpty(ArrayList attributes)
		{
			ArrayList filteredAttributes = new ArrayList();

			for (int index = 0; index < attributes.Count; index++)
			{
				if (attributes[index] != null)
				{
					filteredAttributes.Add(attributes[index]);
				}
			}

			return(filteredAttributes);
		}

		private BaseCommonDataFormat CreateCommonDateFormat(AttributeBase attribute)
		{
			BaseCommonDataFormat commonDataFormat = new CommonStringFormat();

			switch (this.compareRule.CompareValueType)
			{
				case CompareValueTypes.Date:
					commonDataFormat = new CommonDateFormat();
					break;

				case CompareValueTypes.ID:
					commonDataFormat = new CommonIdFormat();
					break;

				case CompareValueTypes.Name:
					commonDataFormat = new CommonNameFormat();
					break;

				case CompareValueTypes.String:
					commonDataFormat = new CommonStringFormat();
					break;

				case CompareValueTypes.Time:
					commonDataFormat = new CommonTimeFormat();
					break;

				case CompareValueTypes.UID:
					commonDataFormat = new CommonUidFormat();
					break;

				default:
					// Do nothing.
					break;
			}

			if (attribute is DicomAttribute)
			{
				DvtkHighLevelInterface.Dicom.Other.Attribute dicomAttributeOnly = (attribute as DicomAttribute).AttributeOnly;

				if (dicomAttributeOnly.VM == 0)
				{
					commonDataFormat.FromDicomFormat("");
				}
				else
				{
					commonDataFormat.FromDicomFormat(dicomAttributeOnly.Values[0]);
				}
				
			}
			else if (attribute is Hl7Attribute)
			{
				commonDataFormat.FromHl7Format((attribute as Hl7Attribute).AttributeOnly);
			}
			else
			{
				throw new System.Exception("Not supposed to get here.");
			}

			return(commonDataFormat);
		}



	}
}
