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

using VR = DvtkData.Dimse.VR;
using Attribute = DvtkHighLevelInterface.Dicom.Other.Attribute;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Common.Other
{
	/// <summary>
    /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
    /// <br></br>
    /// Collection of DicomAttributeToValidate instances.
	/// </summary>
	internal class DicomAttributesToValidate: ArrayList
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property ContainsComparePresentErrors.
		/// </summary>
		private bool containsComparePresentErrors = false;

		/// <summary>
		/// See property ContainsCompareSequenceItemsErrors
		/// </summary>
		private ArrayList containsCompareSequenceItemsErrors = new ArrayList();

		/// <summary>
		/// See property ContainsCompareValuesErrors.
		/// </summary>
		private bool containsCompareValuesErrors = false;

		/// <summary>
		/// See property ContainsCompareVRErrors.
		/// </summary>
		private bool containsCompareVRErrors = false;

		/// <summary>
		/// Used to make sure that the logic for determining the ContainsCompareSequenceItemsErrors
		/// is only performed once.
		/// </summary>
		private bool isContainsCompareSequenceItemsErrorsDetermined = false;

		/// <summary>
		/// Used to make sure that the logic for determining the ContainsComparePresentErrors
		/// is only performed once.
		/// </summary>
		private bool isContainsComparePresentErrorsDetermined = false;

		/// <summary>
		/// Used to make sure that the logic for determining the ContainsCompareValuesErrors
		/// is only performed once.
		/// </summary>
		private bool isContainsCompareValuesErrorsDetermined = false;

		/// <summary>
		/// Used to make sure that the logic for determining the ContainsCompareVRErrors
		/// is only performed once.
		/// </summary>
		private bool isContainsCompareVRErrorsDetermined = false;



		//
		// - Properties -
		//

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Indicates if a difference exists in existence (yes or no) for all attributes that have
		/// the flag Compare_present enabled.
		/// </summary>
		public bool ContainsComparePresentErrors
		{
			get
			{
				if (!this.isContainsComparePresentErrorsDetermined)
				{
					int numberOfAttributesPresent = 0; // Only count the attributes with the Compare_present flag enabled.
					int numberOfAttributesNotPresent = 0; // Only count the attributes with the Compare_present flag enabled.

					foreach (DicomAttributeToValidate dicomAttributeToValidate in this)
					{
						if ( (dicomAttributeToValidate.ValidationRuleDicomAttribute.CompareFlags & CompareFlags.Compare_present) == CompareFlags.Compare_present)
							if (dicomAttributeToValidate.Attribute is InvalidAttribute)
							{
								numberOfAttributesNotPresent++;
							}
							else
							{
								numberOfAttributesPresent++;
							}
					}

					if ((numberOfAttributesPresent > 0) && (numberOfAttributesNotPresent > 0))
					{
						this.containsComparePresentErrors = true;
					}
					else
					{
						this.containsComparePresentErrors = false;
					}

					this.isContainsComparePresentErrorsDetermined = true;
				}

				return(this.containsComparePresentErrors);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// For all attributes present that have the Compare_values flag, find out if differences
		/// exist for their values.
		/// </summary>
		public bool ContainsCompareValuesErrors
		{
			get
			{
				if (!this.isContainsCompareValuesErrorsDetermined)
				{
					DicomAttributeToValidate leadingAttribute = LeadingAttribute;

					// If the leading attribute is present (i.e. at least one attribute is present),
					// find out if errors exist.
					if (leadingAttribute.Attribute is ValidAttribute)
					{
						foreach (DicomAttributeToValidate dicomAttributeToValidate in this)
						{	
							if ((dicomAttributeToValidate != leadingAttribute) && (dicomAttributeToValidate.Attribute is ValidAttribute))
							{
								if ( (dicomAttributeToValidate.ValidationRuleDicomAttribute.CompareFlags & CompareFlags.Compare_values) == CompareFlags.Compare_values)
								{
									if (!leadingAttribute.Attribute.Values.Equals(dicomAttributeToValidate.Attribute.Values))
									{
										this.containsCompareValuesErrors = true;
										break;
									}
								}
							}
						}
					}

					this.isContainsCompareValuesErrorsDetermined = true;
				}

				return(this.containsCompareValuesErrors);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// For all attributes present that have the Compare_VR flag, find out if differences
		/// exist for their VR.
		/// </summary>
		public bool ContainsCompareVRErrors
		{
			get
			{
				if (!this.isContainsCompareVRErrorsDetermined)
				{
					DicomAttributeToValidate leadingAttribute = LeadingAttribute;

					// If the leading attribute is present (i.e. at least one attribute is present),
					// find out if errors exist.
					if (leadingAttribute.Attribute is ValidAttribute)
					{
						foreach (DicomAttributeToValidate dicomAttributeToValidate in this)
						{	
							if ((dicomAttributeToValidate != leadingAttribute) && (dicomAttributeToValidate.Attribute is ValidAttribute))
							{
								if ( (dicomAttributeToValidate.ValidationRuleDicomAttribute.CompareFlags & CompareFlags.Compare_VR) == CompareFlags.Compare_VR)
								{
									if (leadingAttribute.Attribute.VR != dicomAttributeToValidate.Attribute.VR)
									{
										this.containsCompareVRErrors = true;
										break;
									}
								}
							}
						}
					}

					this.isContainsCompareVRErrorsDetermined = true;
				}

				return(this.containsCompareVRErrors);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Indicates if this collection contains at least one sequence attribute.
		/// </summary>
		public bool ContainsSequenceAttribute
		{
			get
			{
				bool containsSequenceAttribute = false;

				AttributeCollection validAttributes = ValidAttributes;

				foreach(ValidAttribute validAttribute in validAttributes)
				{
					if (validAttribute.VR == VR.SQ)
					{
						containsSequenceAttribute = true;
						break;
					}
				}

				return(containsSequenceAttribute);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// The first DicomAttributeToValidate instance in this collection with a ValidAttributes
		/// contained. If no ValidAttribute is contained in this collection, return a newly created
		/// DicomAttributeToValidate instance containng a InvalidAttribute.
		/// </summary>
		public DicomAttributeToValidate LeadingAttribute
		{
			get
			{
				// Set it default to a non present attribute.
				DicomAttributeToValidate leadingAttribute = new DicomAttributeToValidate();

				foreach(DicomAttributeToValidate dicomAttributeToValidate in this)
				{
					if (dicomAttributeToValidate.Attribute is ValidAttribute)
					{
						leadingAttribute = dicomAttributeToValidate;
						break;
					}
				}

				return(leadingAttribute);
			}
		}

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
		public int MaxItemCount
		{
			get
			{
				int maxItemCount = 0;
			
				foreach (DicomAttributeToValidate dicomAttributeToValidate in this)
				{
					if (dicomAttributeToValidate.Attribute is ValidAttribute)
					{
						Attribute attribute = dicomAttributeToValidate.Attribute;

						if (attribute.VR == VR.SQ)
						{
							if ((dicomAttributeToValidate.ValidationRuleDicomAttribute.DicomAttributeFlags & DicomAttributeFlags.Include_sequence_items) == DicomAttributeFlags.Include_sequence_items)
							{
								maxItemCount = Math.Max(maxItemCount, attribute.ItemCount);
							}
						}
					}
				}

				return(maxItemCount);
			}
		}

		/// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// <br></br>
        /// Returns the collection of ValidAttribute instances (i.e. those attributes that are present)
		/// contained in this collection.
		/// </summary>
		public AttributeCollection ValidAttributes
		{
			get
			{
				AttributeCollection validAttributes = new AttributeCollection();

				foreach(DicomAttributeToValidate dicomAttributeToValidate in this)
				{
					if (dicomAttributeToValidate.Attribute is ValidAttribute)
					{
						validAttributes.Add(dicomAttributeToValidate.Attribute);
					}
				}

				return(validAttributes);
			}
		}



		//
		// - Methods -
		//

        /// <summary>
        /// Obsolete class, use the classes in the namespace DvtkHighLevelInterface.Common.Compare instead.
        /// </summary>
        /// <param name="oneBasedSequenceItemIndex">-</param>
        /// <returns>-</returns>
		public bool ContainsCompareSequenceItemsErrors(int oneBasedSequenceItemIndex)
		{
			if (!this.isContainsCompareSequenceItemsErrorsDetermined)
			{
				int maxItemCount = MaxItemCount;

				for (int sequenceItemIndex = 1; sequenceItemIndex <= maxItemCount; sequenceItemIndex++)
				{
					int sequenceItemsPresent = 0;
					int sequenceItemsNotPresent = 0;

					for (int index = 0; index < Count; index++)
					{
						DicomAttributeToValidate dicomAttributeToValidate = this[index] as DicomAttributeToValidate;

						if ((dicomAttributeToValidate.ValidationRuleDicomAttribute.CompareFlags & CompareFlags.Compare_values) == CompareFlags.Compare_values)
						{
							if (dicomAttributeToValidate.Attribute is ValidAttribute)
							{
								if (dicomAttributeToValidate.Attribute.VR == VR.SQ)
								{
									if (dicomAttributeToValidate.Attribute.ItemCount < sequenceItemIndex)
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
					}

					if ((sequenceItemsPresent > 0) && (sequenceItemsNotPresent > 0))
					{
						this.containsCompareSequenceItemsErrors.Add(true);
					}
					else
					{
						this.containsCompareSequenceItemsErrors.Add(false);
					}
				}

				this.isContainsCompareSequenceItemsErrorsDetermined = true;
			}

			return((bool)this.containsCompareSequenceItemsErrors[oneBasedSequenceItemIndex - 1]);
		}
	}
}
