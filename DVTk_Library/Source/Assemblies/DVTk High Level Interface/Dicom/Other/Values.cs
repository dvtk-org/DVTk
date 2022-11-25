// ------------------------------------------------------
// Original author: Marco Kemper
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
using System.Globalization;
using System.IO;
using System.Text;

using DvtkHighLevelInterface.Common.Other;
using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Threads;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
    /// Represents Dicom attribute values.
	/// </summary>
    /// <remarks>
    /// All changes made to an instance of this class are reflected in the actual Dicom values
    /// of the associated Attribute.
    /// </remarks>
	public class Values
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property Attribute.
		/// </summary>
		private Attribute attribute = null;



		//
		// - Constructors -
		//

        /// <summary>
        /// Hide default constructor.
        /// </summary>
        private Values()
        {
            // Do nothing.
        }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="attribute">The attribute this instance belongs to.</param>
		internal Values(Attribute attribute)
		{
			this.attribute = attribute;
		}



		//
		// - Properties -
		//

        /// <summary>
        /// Gets the Attribute this instance belongs to.
        /// </summary>
        internal Attribute Attribute
        {
            get
            {
                return (this.attribute);
            }
        }

        /// <summary>
        /// Gets or sets the underlying BitmapPatternParameters that may be used to store the values.
        /// </summary>
        /// <remarks>
        /// Only use this property when:<br></br>
        /// - The associated Attribute is of type ValidAttribute.<br></br>
        /// - The VR of the associated attribute is OB, OF or OW.<br></br><br></br>
        /// 
        /// If BitmapPatternParameters have not been used to store the values, get returns null.
        /// </remarks>
        internal DvtkData.Dimse.BitmapPatternParameters BitmapPatternParametersImplementation
        {
            get
            {
                DvtkData.Dimse.BitmapPatternParameters bitmapPatternParameters = null;

                ValidAttribute validAttribute = this.attribute as ValidAttribute;

                if (validAttribute != null)
                {
                    if (this.attribute.VR == VR.OB)
                    {
                        DvtkData.Dimse.OtherByteString otherByteString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherByteString;

                        bitmapPatternParameters = otherByteString.Item as DvtkData.Dimse.BitmapPatternParameters;
                    }
                    else if (this.attribute.VR == VR.OF)
                    {
                        DvtkData.Dimse.OtherFloatString otherFloatString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherFloatString;

                        bitmapPatternParameters = otherFloatString.Item as DvtkData.Dimse.BitmapPatternParameters;
                    }
                    else if (this.attribute.VR == VR.OW)
                    {
                        DvtkData.Dimse.OtherWordString otherWordString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherWordString;

                        bitmapPatternParameters = otherWordString.Item as DvtkData.Dimse.BitmapPatternParameters;
                    }
                }

                return (bitmapPatternParameters);
            }
            set
            {
                ValidAttribute validAttribute = this.attribute as ValidAttribute;

                if (validAttribute != null)
                {
                    if (this.attribute.VR == VR.OB)
                    {
                        DvtkData.Dimse.OtherByteString otherByteString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherByteString;

                        otherByteString.BitmapPattern = value;
                    }
                    else if (this.attribute.VR == VR.OF)
                    {
                        DvtkData.Dimse.OtherFloatString otherFloatString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherFloatString;

                        otherFloatString.BitmapPattern = value;
                    }
                    else if (this.attribute.VR == VR.OW)
                    {
                        DvtkData.Dimse.OtherWordString otherWordString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherWordString;

                        otherWordString.BitmapPattern = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the underlying byte array used to store the values.
        /// </summary>
        /// <remarks>
        /// Only use this property when:<br></br>
        /// - The associated Attribute is of type ValidAttribute.<br></br>
        /// - The Attribute associated with this instance has VR UN.
        /// </remarks>
        internal byte[] ByteArrayImplementation
        {
            get
            {
                ValidAttribute validAttribute = this.attribute as ValidAttribute;

                if (validAttribute == null)
                {
                    return (new byte[0]);
                }
                else
                {
                    DvtkData.Dimse.Unknown dvtkDataUnknownForThisAttribute = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.Unknown;

                    if (dvtkDataUnknownForThisAttribute.ByteArray == null)
                    {
                        return (new byte[0]);
                    }
                    else
                    {
                        return (dvtkDataUnknownForThisAttribute.ByteArray);
                    }
                }
            }
            set
            {
                ValidAttribute validAttribute = this.attribute as ValidAttribute;

                if (validAttribute == null)
                {
                    // Do nothing.
                }
                else
                {
                    DvtkData.Dimse.Unknown dvtkDataUnknownForThisAttribute = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.Unknown;

                    dvtkDataUnknownForThisAttribute.ByteArray = (byte[])value.Clone();
                }
            }
        }

		/// <summary>
		/// Gets the underlying collection used to store the values.
		/// </summary>
        /// <remarks>
        /// Only use this property when:<br></br>
        /// - The associated Attribute is of type ValidAttribute.<br></br>
        /// - The property IsImplementedWithCollection returns true.
        /// </remarks>
		internal DvtkData.Collections.NullSafeCollectionBase CollectionImplementation
		{
			get
			{
				DvtkData.Collections.NullSafeCollectionBase collection = null;

				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute == null)
				{
					throw new System.Exception("Internal error: only use property CollectionImplementation for Values with associated ValidAttribute instance.");
				}

				DvtkData.Dimse.DicomValueType dicomValue = validAttribute.DvtkDataAttribute.DicomValue;

				switch(this.attribute.VR)
				{
					case VR.AE: // Application Entity
						DvtkData.Dimse.ApplicationEntity theApplicationEntity = dicomValue as DvtkData.Dimse.ApplicationEntity;
						collection = theApplicationEntity.Values;
						break;

					case VR.AS: // Age String
						DvtkData.Dimse.AgeString theAgeString = dicomValue as DvtkData.Dimse.AgeString;
						collection = theAgeString.Values;
						break;

					case VR.AT: // Attribute Tag
						DvtkData.Dimse.AttributeTag theAttributeTag = dicomValue as DvtkData.Dimse.AttributeTag;
						collection = theAttributeTag.Values;
						break;

					case VR.CS: // Code String
						DvtkData.Dimse.CodeString theCodeString = dicomValue as DvtkData.Dimse.CodeString;
						collection = theCodeString.Values;
						break;

					case VR.DA: // Date
						DvtkData.Dimse.Date theDate = dicomValue as DvtkData.Dimse.Date;
						collection = theDate.Values;
						break;

					case VR.DS: // Decimal String
						DvtkData.Dimse.DecimalString theDecimalString = dicomValue as DvtkData.Dimse.DecimalString;
						collection = theDecimalString.Values;
						break;

					case VR.DT: // Date Time
						DvtkData.Dimse.DateTime theDateTime = dicomValue as DvtkData.Dimse.DateTime;
						collection = theDateTime.Values;
						break;

					case VR.FD: // Floating Point Double
						DvtkData.Dimse.FloatingPointDouble theFloatingPointDouble = dicomValue as DvtkData.Dimse.FloatingPointDouble;
						collection = theFloatingPointDouble.Values;
						break;

					case VR.FL: // Floating Point Single
						DvtkData.Dimse.FloatingPointSingle theFloatingPointSingle = dicomValue as DvtkData.Dimse.FloatingPointSingle;
						collection = theFloatingPointSingle.Values;
						break;

					case VR.IS: // Integer String
						DvtkData.Dimse.IntegerString theIntegerString = dicomValue as DvtkData.Dimse.IntegerString;
						collection = theIntegerString.Values;
						break;

					case VR.LO: // Long String
						DvtkData.Dimse.LongString theLongString = dicomValue as DvtkData.Dimse.LongString;
						collection = theLongString.Values;
						break;

					case VR.PN: // Person Name
						DvtkData.Dimse.PersonName thePersonName = dicomValue as DvtkData.Dimse.PersonName;
						collection = thePersonName.Values;
						break;

					case VR.SH: // Short String
						DvtkData.Dimse.ShortString theShortString = dicomValue as DvtkData.Dimse.ShortString;
						collection = theShortString.Values;
						break;

					case VR.SL: // Signed Long
						DvtkData.Dimse.SignedLong theSignedLong = dicomValue as DvtkData.Dimse.SignedLong;
						collection = theSignedLong.Values;
						break;

					case VR.SS: // Signed Short
						DvtkData.Dimse.SignedShort theSignedShort = dicomValue as DvtkData.Dimse.SignedShort;
						collection = theSignedShort.Values;
						break;

					case VR.TM: // Time
						DvtkData.Dimse.Time theTime = dicomValue as DvtkData.Dimse.Time;
						collection = theTime.Values;
						break;

					case VR.UI: // Unique Identifier (UID)
						DvtkData.Dimse.UniqueIdentifier theUniqueIdentifier = dicomValue as DvtkData.Dimse.UniqueIdentifier;
						collection = theUniqueIdentifier.Values;
						break;

					case VR.UL: // Unsigned Long
						DvtkData.Dimse.UnsignedLong theUnsignedLong = dicomValue as DvtkData.Dimse.UnsignedLong;
						collection = theUnsignedLong.Values;
						break;

					case VR.US: // Unsigned Short
						DvtkData.Dimse.UnsignedShort theUnsignedShort = dicomValue as DvtkData.Dimse.UnsignedShort;
						collection = theUnsignedShort.Values;
						break;

                    case VR.UC: // Unlimited Characters
                        DvtkData.Dimse.UnlimitedCharacters theUnlimitedCharacters = dicomValue as DvtkData.Dimse.UnlimitedCharacters;
                        collection = theUnlimitedCharacters.Values;
                        break;

					default:
						collection = null;
						break;
				}

				return(collection);
			}
		}

		/// <summary>
		/// Gets the number of values.
		/// </summary>
        /// <remarks>
        /// This property is only meaningfull if the Attribute this instance belongs to
        /// does not have VR SQ.
        /// </remarks>
		public int Count
		{
			get
			{
				int count = 0;

				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute == null)
					// Associated Attribute is InvalidAttribute.
				{
					count = 0;
				}
				else if (IsImplementedWithCollection)
					// Collection implementation.
				{
					count = CollectionImplementation.Count;
				}
				else if (IsImplementedWithString)
					// String implementation.
				{
					String stringImplementation = StringImplementation;
	
					if (stringImplementation == null)
					{
						count = 0;
					}
					else
					{
						count = 1;
					}
				}
                else if ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW) || (attribute.VR == VR.OD) || (attribute.VR == VR.OL) || (attribute.VR == VR.OV))
					// File or pattern implementation.
				{
					if ((FileNameImplementation != null) || (BitmapPatternParametersImplementation != null))
					{
						count = 1;
					}
					else
					{
						count = 0;
					}
				}
				else if (this.attribute.VR == VR.UN)
					// VR UN.
				{
					if (ByteArrayImplementation.Length == 0)
					{
						count = 0;
					}
					else
					{
						count = 1;
					}
				}
				else if (this.attribute.VR == VR.SQ)
					// VR SQ
				{
					count = 0;
				}
				else
					// Other cases.
				{
					count = 0;
				}

				return(count);
			}
		}

        /// <summary>
        /// Gets or sets the underlying file name of the file that may be used to store the values.
        /// </summary>
        /// <remarks>
        /// Only use this property when:<br></br>
        /// - The associated Attribute is of type ValidAttribute.<br></br>
        /// - The VR of the associated attribute is OB, OF, OW, OD, OL or OV.<br></br><br></br>
        /// 
        /// If a file has not been used to store the values, get returns null.
        /// </remarks>
        internal String FileNameImplementation
		{
			get
			{
				String fileName = null;

				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute != null)
				{
					if (this.attribute.VR == VR.OB)
					{
						DvtkData.Dimse.OtherByteString otherByteString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherByteString;

						fileName = otherByteString.Item as String;
					}
					else if (this.attribute.VR == VR.OF)
					{
						DvtkData.Dimse.OtherFloatString otherFloatString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherFloatString;

						fileName = otherFloatString.Item as String;
					}
					else if (this.attribute.VR == VR.OW)
					{
						DvtkData.Dimse.OtherWordString otherWordString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherWordString;

						fileName = otherWordString.Item as String;
                    }
                    else if (this.attribute.VR == VR.OD)
                    {
                        DvtkData.Dimse.OtherDoubleString otherDoubleString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherDoubleString;

                        fileName = otherDoubleString.Item as String;
                    }
                    else if (this.attribute.VR == VR.OL)
                    {
                        DvtkData.Dimse.OtherLongString otherLongString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherLongString;

                        fileName = otherLongString.Item as String;
                    }
                    else if (this.attribute.VR == VR.OV)
                    {
                        DvtkData.Dimse.OtherVeryLongString otherVeryLongString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherVeryLongString;

                        fileName = otherVeryLongString.Item as String;
                    }
				}

				return(fileName);
			}
			set
			{
				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute != null)
				{
					if (this.attribute.VR == VR.OB)
					{
						DvtkData.Dimse.OtherByteString otherByteString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherByteString;

						otherByteString.FileName = value;
					}
					else if (this.attribute.VR == VR.OF)
					{
						DvtkData.Dimse.OtherFloatString otherFloatString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherFloatString;

						otherFloatString.FileName = value;
					}
					else if (this.attribute.VR == VR.OW)
					{
						DvtkData.Dimse.OtherWordString otherWordString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherWordString;

						otherWordString.FileName = value;
                    }
                    else if (this.attribute.VR == VR.OD)
                    {
                        DvtkData.Dimse.OtherDoubleString otherDoubleString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherDoubleString;

                        otherDoubleString.FileName = value;
                    }
                    else if (this.attribute.VR == VR.OL)
                    {
                        DvtkData.Dimse.OtherLongString otherLongString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherLongString;

                        otherLongString.FileName = value;
                    }
                    else if (this.attribute.VR == VR.OV)
                    {
                        DvtkData.Dimse.OtherVeryLongString otherVeryLongString = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.OtherVeryLongString;

                        otherVeryLongString.FileName = value;
                    }
				}
			}
		}

		/// <summary>
		/// Indicates if values are stored in a collection.
		/// </summary>
        /// <remarks>
        /// Only use this property when the associated Attribute is of type ValidAttribute.
        /// </remarks>
		internal bool IsImplementedWithCollection
		{
			get
			{
				bool isCollectionImplementation = true;

				switch(this.attribute.VR)
				{
					case VR.AE: // Application Entity
					case VR.AS: // Age String
					case VR.AT: // Attribute Tag
					case VR.CS: // Code String
					case VR.DA: // Date
					case VR.DS: // Decimal String
					case VR.DT: // Date Time
					case VR.FD: // Floating Point Double
					case VR.FL: // Floating Point Single
					case VR.IS: // Integer String
					case VR.LO: // Long String
					case VR.PN: // Person Name
					case VR.SH: // Short String
					case VR.SL: // Signed Long
					case VR.SS: // Signed Short
					case VR.TM: // Time
					case VR.UI: // Unique Identifier (UID)
					case VR.UL: // Unsigned Long
					case VR.US: // Unsigned Short
                    case VR.UC: // Unlimited Characters
						isCollectionImplementation = true;
						break;

					default:
						isCollectionImplementation = false;
						break;
				}

				return(isCollectionImplementation);
			}
		}

        /// <summary>
        /// Indicates if values are stored in a String.
        /// </summary>
        /// <remarks>
        /// Only use this property when the associated Attribute is of type ValidAttribute.
        /// </remarks>
        internal bool IsImplementedWithString
		{
			get
			{
				bool isImplementedWithString = true;

				switch(this.attribute.VR)
				{
					case VR.LT: // Long Text
					case VR.ST: // Short Text
					case VR.UT: // Unlimited Text
                    case VR.UR: // 
						isImplementedWithString = true;
						break;

					default:
						isImplementedWithString = false;
						break;
				}

				return(isImplementedWithString);
			}
		}

        /// <summary>
        /// Gets the underlying String used to store the values.
        /// </summary>
        /// <remarks>
        /// Only use this property when:<br></br>
        /// - The associated Attribute is of type ValidAttribute.<br></br>
        /// - The property IsImplementedWithString returns true.<br></br><br></br>
        /// 
        /// When no values exist, get returns null.
        /// </remarks>        
        internal String StringImplementation
		{
			get
			{
				String stringImplementation = "";

				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute == null)
				{
					throw new System.Exception("Internal error: only use property StringImplementation for Values with associated ValidAttribute instance.");
				}

				DvtkData.Dimse.DicomValueType dicomValue = validAttribute.DvtkDataAttribute.DicomValue;

				switch(this.attribute.VR)
				{
					case VR.LT: // Long Text
						DvtkData.Dimse.LongText theLongText = dicomValue as DvtkData.Dimse.LongText;
						stringImplementation = theLongText.Value;
						break;

					case VR.ST: // Short Text
						DvtkData.Dimse.ShortText theShortText = dicomValue as DvtkData.Dimse.ShortText;
						stringImplementation = theShortText.Value;
						break;

					case VR.UT: // Unlimited Text
						DvtkData.Dimse.UnlimitedText theUnlimitedText = dicomValue as DvtkData.Dimse.UnlimitedText;
						stringImplementation = theUnlimitedText.Value;
						break;
                    
                    case VR.UR:
                        DvtkData.Dimse.UniversalResourceIdentifier theUniversalResourceIdentifier = dicomValue as DvtkData.Dimse.UniversalResourceIdentifier;
                        stringImplementation = theUniversalResourceIdentifier.Value;
                        break;

					default:
						stringImplementation = "";
						break;
				}

				return(stringImplementation);
			}
			set
			{
				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute == null)
				{
					throw new HliException("Only use this property for Values with associated ValidAttribute object.");
				}

				DvtkData.Dimse.DicomValueType dicomValue = validAttribute.DvtkDataAttribute.DicomValue;

				switch(this.attribute.VR)
				{
					case VR.LT: // Long Text
						DvtkData.Dimse.LongText theLongText = dicomValue as DvtkData.Dimse.LongText;
						theLongText.Value = value;
						break;

					case VR.ST: // Short Text
						DvtkData.Dimse.ShortText theShortText = dicomValue as DvtkData.Dimse.ShortText;
						theShortText.Value = value;
						break;

					case VR.UT: // Unlimited Text
						DvtkData.Dimse.UnlimitedText theUnlimitedText = dicomValue as DvtkData.Dimse.UnlimitedText;
						theUnlimitedText.Value = value;
						break;

                    case VR.UR:
                        DvtkData.Dimse.UniversalResourceIdentifier theUniversalResourceIdentifier = dicomValue as DvtkData.Dimse.UniversalResourceIdentifier;
                        theUniversalResourceIdentifier.Value = value;
                        break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Get or sets a single value given the zero based index. The get converts the actual
        /// Dicom value to the returned string, the set converts the supplied string to the actual
        /// Dicom value.
		/// </summary>
        /// <remarks>
        /// When spaces are non-significant according to part 5, 
        /// they are left out, when using the get, like specified below.<br></br><br></br>
        /// 
        /// When the attribute, this instance belongs to, has VR AE, CS, DS, IS, LO or SH,
        /// all leading and trailing spaces are removed before returning the String.<br></br><br></br>
        /// 
        /// When the attribute, this instance belongs to, has VR LT, PN, ST, TM or UT,
        /// all trailing spaces are removed before returning the String.<br></br><br></br>
        /// 
        /// When the attribute, this instance belongs to, has VR DA or DT,
        /// all trailing spaces are removed before returning the String although
        /// nothing is mentioned in part 5 about non-significant spaces. This is
        /// because trailing spaces may be present in queries with range matching.<br></br><br></br>
        /// 
        /// The set may only be used to replace an existing value.
        /// When new values need to be inserted, use one of the Insert methods.
        /// </remarks>
		public String this[int zeroBasedIndex]
		{
			get
			{
				String stringValue = "";

				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute == null)
					// Associated Attribute is InvalidAttribute.
				{
					stringValue = "";
				}
				else if (IsImplementedWithCollection)
					// Collection implementation.
				{
					if ((zeroBasedIndex < 0) || (zeroBasedIndex >= Count))
					{
						stringValue = "";
					}
					else
					{
						stringValue = GetCollectionValue(zeroBasedIndex);
					}
				}
				else if (IsImplementedWithString)
					// String implementation.
				{
					String stringImplementation = StringImplementation;
	
					if (stringImplementation == null)
					{
						stringValue = "";
					}
					else
					{
						stringValue = stringImplementation;
					}
				}
                else if ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW))
					// VR of associated attribute is OB, OF or OW.
				{
					if (FileNameImplementation != null)
					{
						stringValue = FileNameImplementation;
					}
					else if (BitmapPatternParametersImplementation != null)
					{
						DvtkData.Dimse.BitmapPatternParameters bitmapPatternParameters = BitmapPatternParametersImplementation;

						stringValue =
							bitmapPatternParameters.NumberOfRows.ToString() + ", " +
							bitmapPatternParameters.NumberOfColumns.ToString() + ", " + 
							bitmapPatternParameters.StartValue.ToString() + ", " +  
							bitmapPatternParameters.ValueIncrementPerRowBlock.ToString() + ", " +  
							bitmapPatternParameters.ValueIncrementPerColumnBlock.ToString() + ", " +  
							bitmapPatternParameters.NumberOfIdenticalValueRows.ToString() + ", " +  
							bitmapPatternParameters.NumberOfIdenticalValueColumns.ToString();
					}
					else
					{
						stringValue = "";
					}
				}
				else if (this.attribute.VR == VR.UN)
					// VR UN.
				{
					if ((zeroBasedIndex < 0) || (zeroBasedIndex >= Count))
					{
						stringValue = "";
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();

						DvtkData.Dimse.Unknown dvtkDataUnknown = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.Unknown;

						for (int byteIndex = 0; byteIndex < dvtkDataUnknown.ByteArray.Length; byteIndex++)
						{
							Byte theByte = dvtkDataUnknown.ByteArray[byteIndex];

							String byteAsString = theByte.ToString("X");
							if (byteAsString.Length == 1)
							{
								byteAsString = "0" + byteAsString;
							}

							stringBuilder.Append(byteAsString);
						}

						stringValue = stringBuilder.ToString();
					}
				}
				else if (this.attribute.VR == VR.SQ)
					// VR SQ
				{
					stringValue = "";
				}
				else
					// Other cases.
				{
					stringValue = "";
				}

				return(RemoveNonSignificantSpaces(stringValue, this.attribute.VR));
			}
			set
			{
				ValidAttribute validAttribute = this.attribute as ValidAttribute;

				if (validAttribute == null)
					// Associated Attribute is InvalidAttribute.
				{
					// Do nothing.
				}
				else
				{
					if ((zeroBasedIndex < 0) || (zeroBasedIndex >= Count))
					{
						// Do nothing.
					}
					else
					{
						RemoveAt(zeroBasedIndex);
						Insert(zeroBasedIndex, value);
					}
				}
			}
		}



		//
		// - Methods -
		//

        /// <summary>
        /// Adds values to the end of this instance.
        /// </summary>
        /// <param name="parameters">Values to add.</param>
        public void Add(params Object[] parameters)
        {
            if (this.attribute is InvalidAttribute)
            {
                // Do nothing.
            }
            else
            {
                Insert(this.Count, parameters);
            }
        }

		/// <summary>
		/// Adds values to the end of this instance.
		/// </summary>
		/// <param name="values">Values to add.</param>
        /// 
		public void Add(Values values)
		{
			Insert(this.Count, values);
		}

        /// <summary>
        /// Removes all values from this instance.
        /// </summary>
        public void Clear()
		{
			ValidAttribute validAttribute = this.attribute as ValidAttribute;

			if (validAttribute == null)
				// Associated Attribute is InvalidAttribute.
			{
				// Do nothing.
			}
			else if (IsImplementedWithCollection)
				// Collection implementation.
			{
				CollectionImplementation.Clear();
				SetLength();
			}
			else if (IsImplementedWithString)
				// String implementation.
			{
				StringImplementation = null;
				SetLength();
			}
            else if ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW))
				// VR of associated attribute is OB, OF or OW.
			{
				FileNameImplementation = null;
				BitmapPatternParametersImplementation = null;
			}
			else if (this.attribute.VR == VR.UN)
				// VR UN.
			{
				// Do nothing.
			}
			else if (this.attribute.VR == VR.SQ)
				// VR SQ
			{
				// Do nothing.
			}
			else
				// Other cases.
			{
				// Do nothing.
			}
		}

		/// <summary>
		/// Converts the supplied array to a DvtkData DoubleCollection.
		/// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a 0.
        /// </remarks>
		/// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData DoubleCollection.</returns>
		private DvtkData.Collections.DoubleCollection ConvertToDoubleCollection(object[] objects)
		{
			DvtkData.Collections.DoubleCollection doubleCollection = new DvtkData.Collections.DoubleCollection();

			foreach (object item in objects)
			{
				if (item != null)
				{
					Double doubleValue = 0;

					try
					{
						doubleValue = System.Convert.ToDouble(item);
					}
					catch
					{
						doubleValue = 0;
					}

					doubleCollection.Add(doubleValue);
				}
			}

			return (doubleCollection);
		}

        /// <summary>
        /// Converts the supplied array to a DvtkData Int16Collection.
        /// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a 0.
        /// </remarks>
        /// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData Int16Collection.</returns>
        private DvtkData.Collections.Int16Collection ConvertToInt16Collection(object[] objects)
        {
            DvtkData.Collections.Int16Collection int16Collection = new DvtkData.Collections.Int16Collection();

            foreach (object item in objects)
            {
                if (item != null)
                {
                    Int16 int16Value = 0;

                    try
                    {
                        int16Value = System.Convert.ToInt16(item);
                    }
                    catch
                    {
                        int16Value = 0;
                    }

                    int16Collection.Add(int16Value);
                }
            }
            return (int16Collection);
        }

        /// <summary>
        /// Converts the supplied array to a DvtkData Int32Collection.
        /// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a 0.
        /// </remarks>
        /// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData Int32Collection.</returns>
        private DvtkData.Collections.Int32Collection ConvertToInt32Collection(object[] objects)
        {
            DvtkData.Collections.Int32Collection int32Collection = new DvtkData.Collections.Int32Collection();

            foreach (object item in objects)
            {
                if (item != null)
                {

                    Int32 int32Value = 0;

                    try
                    {
                        int32Value = System.Convert.ToInt32(item);
                    }
                    catch
                    {
                        int32Value = 0;
                    }

                    int32Collection.Add(int32Value);
                }
            }
            return (int32Collection);
        }

        /// <summary>
        /// Converts the supplied array to a DvtkData SingleCollection.
        /// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a 0.
        /// </remarks>
        /// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData SingleCollection.</returns>
		private DvtkData.Collections.SingleCollection ConvertToSingleCollection(object[] objects)
		{
			DvtkData.Collections.SingleCollection singleCollection = new DvtkData.Collections.SingleCollection();

			foreach (object item in objects)
			{
				if (item != null)
				{
					Single singleValue = 0;

					try
					{
						singleValue = System.Convert.ToSingle(item);
					}
					catch
					{
						singleValue = 0;
					}

					singleCollection.Add(singleValue);
				}
			}

			return (singleCollection);
		}

        /// <summary>
        /// Converts the supplied array to a DvtkData StringCollection.
        /// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a "".
        /// </remarks>
        /// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData StringCollection.</returns>
        private DvtkData.Collections.StringCollection ConvertToStringCollection(object[] objects)
        {
            DvtkData.Collections.StringCollection stringCollection = new DvtkData.Collections.StringCollection();

            foreach (object item in objects)
            {
                if (item != null)
                {
                    String stringValue = "";

                    try
                    {
                        stringValue = System.Convert.ToString(item);
                    }
                    catch
                    {
                        stringValue = "";
                    }

                    stringCollection.Add(stringValue);
                }
            }

            return (stringCollection);
        }

        /// <summary>
        /// Converts the supplied array to a DvtkData TagCollection.
        /// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a tag with value 0x00000000.
        /// </remarks>
        /// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData TagCollection.</returns>
        private DvtkData.Collections.TagCollection ConvertToTagCollection(object[] objects)
        {
            DvtkData.Collections.TagCollection tagCollection = new DvtkData.Collections.TagCollection();

            foreach (object item in objects)
            {
                if (item != null)
                {
                    DvtkData.Dimse.Tag dvtkDataTag = null;

                    try
                    {
                        if (item is DvtkData.Dimse.Tag)
                        {
                            DvtkData.Dimse.Tag sourceDvtkDataTag = item as DvtkData.Dimse.Tag;

                            dvtkDataTag = new DvtkData.Dimse.Tag(sourceDvtkDataTag.GroupNumber, sourceDvtkDataTag.ElementNumber);
                        }
                        else if (item is System.Int32)
                        {
                            dvtkDataTag = (System.Int32)item;
                        }
                        else if (item is System.UInt32)
                        {
                            dvtkDataTag = (System.UInt32)item;
                        }
                        else if (item is String)
                        {
                            TagSequence tagSequence = new TagSequence(item as String);

                            if (tagSequence.IsSingleAttributeMatching)
                            {
                                if (tagSequence.Tags.Count == 1)
                                {
                                    Tag tag = tagSequence.Tags[0] as Tag;

                                    dvtkDataTag = new DvtkData.Dimse.Tag(tag.GroupNumber, tag.ElementNumber);
                                }
                                else
                                {
                                    throw new ArgumentException("One of the Array elements represents a nested tag.");
                                }
                            }
                            else
                            {
                                throw new ArgumentException("One of the Array elements represents an indexed tag.");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("One of the Array elements is of an unexpected type.");
                        }
                    }
                    catch
                    {
                        dvtkDataTag = new DvtkData.Dimse.Tag(0x0000, 0x0000);
                    }

                    tagCollection.Add(dvtkDataTag);
                }
            }

            return (tagCollection);
        }

        /// <summary>
        /// Converts the supplied array to a DvtkData UInt16Collection.
        /// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a 0.
        /// </remarks>
        /// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData UInt16Collection.</returns>
        private DvtkData.Collections.UInt16Collection ConvertToUInt16Collection(object[] objects)
        {
            DvtkData.Collections.UInt16Collection uInt16Collection = new DvtkData.Collections.UInt16Collection();

            foreach (object item in objects)
            {
                if (item != null)
                {
                    UInt16 uInt16Value = 0;

                    try
                    {
                        uInt16Value = System.Convert.ToUInt16(item);
                    }
                    catch
                    {
                        uInt16Value = 0;
                    }

                    uInt16Collection.Add(uInt16Value);
                }
            }
            return (uInt16Collection);
        }

        /// <summary>
        /// Converts the supplied array to a DvtkData UInt32Collection.
        /// </summary>
        /// <remarks>
        /// When an array element is null, it will be skipped.<br></br><br></br>
        /// When an array element cannot be converted, it will be interpreted as a 0.
        /// </remarks>
        /// <param name="objects">The array to convert.</param>
        /// <returns>The returned DvtkData UInt32Collection.</returns>
        private DvtkData.Collections.UInt32Collection ConvertToUInt32Collection(object[] objects)
        {
            DvtkData.Collections.UInt32Collection uInt32Collection = new DvtkData.Collections.UInt32Collection();

            foreach (object item in objects)
            {
                if (item != null)
                {
                    UInt32 uInt32Value = 0;

                    try
                    {
                        uInt32Value = System.Convert.ToUInt32(item);
                    }
                    catch
                    {
                        uInt32Value = 0;
                    }

                    uInt32Collection.Add(uInt32Value);
                }
            }
            return (uInt32Collection);
        }

        /// <summary>
        /// Compares this instance with an array.
        /// </summary>
        /// <remarks>
        /// This instance and the array are considered equal when:<br></br>
        /// - Both contain the same number of values/elements.<br></br>
        /// - The String representations of the individual values/elements are the same.<br></br><br></br>
        /// 
        /// When spaces are non-significant according to part 5, 
        /// they are left out, when comparing, like specified below.<br></br><br></br>
        /// 
        /// When the attribute, this instance belongs to, has VR AE, CS, DS, IS, LO or SH,
        /// all leading and trailing spaces are removed before comparing.<br></br><br></br>
        /// 
        /// When the attribute, this instance belongs to, has VR LT, PN, ST, TM or UT,
        /// all trailing spaces are removed before comparing.<br></br><br></br>
        /// 
        /// When the attribute, this instance belongs to, has VR DA or DT,
        /// all trailing spaces are removed before comparing although
        /// nothing is mentioned in part 5 about non-significant spaces. This is
        /// because trailing spaces may be present in queries with range matching.<br></br><br></br>
        /// </remarks>
        /// <param name="objects">The array to compare with.</param>
        /// <returns>Boolean indicating if they are equal.</returns>
        public bool Equals(params Object[] objects)
        {
            bool equals = true;

            ValidAttribute validAttribute = this.attribute as ValidAttribute;

            if (validAttribute == null)
            // Associated Attribute is InvalidAttribute.
            {
                equals = false;
            }
            else
            {
                if (Count != objects.Length)
                {
                    equals = false;
                }
                else
                {
                    for (int index = 0; index < Count; index++)
                    {
                        if (this[index] != RemoveNonSignificantSpaces(objects.GetValue(index).ToString(), validAttribute.VR))
                        {
                            equals = false;
                            break;
                        }
                    }
                }
            }

            return (equals);
        }

        /// <summary>
        /// Compares this instance with another Values instance.
        /// </summary>
        /// <remarks>
        /// Two Values instances are considered equal when:<br></br>
        /// - Both contain the same number of values.<br></br>
        /// - The String representations of the individual values are the same.<br></br><br></br>
        /// 
        /// When spaces are non-significant according to part 5, 
        /// they are left out, when comparing, like specified below.<br></br><br></br>
        /// 
        /// When the attribute, a Values instance belongs to, has VR AE, CS, DS, IS, LO or SH,
        /// all leading and trailing spaces are removed before comparing.<br></br><br></br>
        /// 
        /// When the attribute, a Values instance belongs to, has VR LT, PN, ST, TM or UT,
        /// all trailing spaces are removed before comparing.<br></br><br></br>
        /// 
        /// When the attribute, a Values instance belongs to, has VR DA or DT,
        /// all trailing spaces are removed before comparing although
        /// nothing is mentioned in part 5 about non-significant spaces. This is
        /// because trailing spaces may be present in queries with range matching.<br></br><br></br>
        /// </remarks>
        /// <param name="values">The Values instance to compare with.</param>
        /// <returns>Boolean indicating if they are equal.</returns>
        public bool Equals(Values values)
        {
            bool equals = true;

            ValidAttribute validAttribute1 = Attribute as ValidAttribute;
            ValidAttribute validAttribute2 = values.Attribute as ValidAttribute;

            if ((validAttribute1 != null) && 
                (validAttribute2 != null) && 
                ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW)) && 
                ((values.Attribute.VR == VR.OB) || (values.Attribute.VR == VR.OF) || (values.Attribute.VR == VR.OW)))
            {
                equals = Dvtk.DvtkDataHelper.ComparePixelAttributes(validAttribute1.DvtkDataAttribute, validAttribute2.DvtkDataAttribute);
            }
            else
            {
                if (Count != values.Count)
                {
                    equals = false;
                }
                else
                {
                    for (int index = 0; index < Count; index++)
                    {
                        if (this[index] != values[index])
                        {
                            equals = false;
                            break;
                        }
                    }
                }
            }

            return (equals);
        }

        /// <summary>
        /// Gets the String representation of a single value from a DvtkData collection.
        /// </summary>
        /// <param name="zeroBasedIndex">The zero based index.</param>
        /// <returns>The String representation of a value.</returns>
        private String GetCollectionValue(int zeroBasedIndex)
        {
            String singleValue = "";

            switch (this.attribute.VR)
            {
                case VR.AE: // Application Entity
                case VR.AS: // Age String
                case VR.CS: // Code String
                case VR.DA: // Date
                case VR.DS: // Decimal String
                case VR.DT: // Date Time
                case VR.IS: // Integer String
                case VR.LO: // Long String
                case VR.PN: // Person Name
                case VR.SH: // Short String
                case VR.TM: // Time
                case VR.UI: // Unique Identifier (UID)
                case VR.UC:
                    singleValue = (CollectionImplementation as DvtkData.Collections.StringCollection)[zeroBasedIndex].ToString();
                    break;

                case VR.AT: // Attribute Tag
                    DvtkData.Dimse.Tag tag = (CollectionImplementation as DvtkData.Collections.TagCollection)[zeroBasedIndex];
					singleValue = TagString(tag.GroupNumber,tag.ElementNumber);
                    break;

                case VR.FD: // Floating Point Double
                    singleValue = (CollectionImplementation as DvtkData.Collections.DoubleCollection)[zeroBasedIndex].ToString();
                    break;

                case VR.FL: // Floating Point Single
                    singleValue = (CollectionImplementation as DvtkData.Collections.SingleCollection)[zeroBasedIndex].ToString();
                    break;

                case VR.SL: // Signed Long
                    singleValue = (CollectionImplementation as DvtkData.Collections.Int32Collection)[zeroBasedIndex].ToString();
                    break;

                case VR.SS: // Signed Short
                    singleValue = (CollectionImplementation as DvtkData.Collections.Int16Collection)[zeroBasedIndex].ToString();

                    break;
                case VR.UL: // Unsigned Long
                    singleValue = (CollectionImplementation as DvtkData.Collections.UInt32Collection)[zeroBasedIndex].ToString();
                    break;

                case VR.US: // Unsigned Short
                    singleValue = (CollectionImplementation as DvtkData.Collections.UInt16Collection)[zeroBasedIndex].ToString();
                    break;

                default:
                    singleValue = "";
                    break;
            }

            return (singleValue);
        }

		/// <summary>
		/// Obtains the <see cref="System.String"/> representation of Tag.
		/// </summary>
		/// <returns>The friendly name of the <see cref="Tag"/>.</returns>
		private string TagString(UInt16 group, UInt16 element)
		{
			StringBuilder sb = new StringBuilder();
			System.Byte[] groupByteArray = System.BitConverter.GetBytes(group);
			System.Byte[] elementByteArray = System.BitConverter.GetBytes(element);
			if (System.BitConverter.IsLittleEndian)
			{
				// Display as Big Endian
				System.Array.Reverse(groupByteArray);
				System.Array.Reverse(elementByteArray);
			}
			string hexByteStr0, hexByteStr1;

			/*
			 * H00 -ToString("x")-> "0" is prepended to "00"
			 * H01 -ToString("x")-> "1" is prepended to "01"
			 * HAA -ToString("x")-> "AA"
			 */
			hexByteStr0 = groupByteArray[0].ToString("x");
			if (hexByteStr0.Length == 1) hexByteStr0 = "0" + hexByteStr0; // prepend with leading zero
			hexByteStr1 = groupByteArray[1].ToString("x");
			if (hexByteStr1.Length == 1) hexByteStr1 = "0" + hexByteStr1; // prepend with leading zero
			sb.AppendFormat(
				"0x{0}{1}", 
				hexByteStr0,
				hexByteStr1);

			hexByteStr0 = elementByteArray[0].ToString("x");
			if (hexByteStr0.Length == 1) hexByteStr0 = "0" + hexByteStr0; // prepend with leading zero
			hexByteStr1 = elementByteArray[1].ToString("x");
			if (hexByteStr1.Length == 1) hexByteStr1 = "0" + hexByteStr1; // prepend with leading zero
			sb.AppendFormat(
				"{0}{1}", 
				hexByteStr0,
				hexByteStr1);
			return sb.ToString();
		}

        /// <summary>
        /// Inserts an array in this instance at a specified position.
        /// </summary>
        /// <param name="zeroBasedIndex">The zero based index to insert.</param>
        /// <param name="parameters">The array to insert.</param>
		/// <exception cref="System.ArgumentException"><paramref name="parameters"/> cannot be interpreted.</exception>
        public void Insert(int zeroBasedIndex, params Object[] parameters)
        {
            ValidAttribute validAttribute = this.attribute as ValidAttribute;

            if (validAttribute == null)
            // Associated Attribute is InvalidAttribute.
            {
                // Do nothing.
            }
            else if (IsImplementedWithCollection)
            // Collection implementation.
            {
                if ((zeroBasedIndex < 0) || (zeroBasedIndex > Count))
                {
                    // Do nothing.
                }
                else
                {
                    InsertCollectionValues(zeroBasedIndex, parameters);
                    SetLength();
                }
            }
            else if (IsImplementedWithString)
            // String implementation.
            {
                DvtkData.Collections.StringCollection stringCollection = ConvertToStringCollection(parameters);

                if ((stringCollection.Count > 0) && (zeroBasedIndex == 0))
                {
                    StringImplementation = stringCollection[0];
                    SetLength();
                }
            }
            else if ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW))
            // File or pattern implementation.
            {
                if (zeroBasedIndex == 0)
                {
                    String fileName = null;
                    DvtkData.Dimse.BitmapPatternParameters bitmapPatternParameters = null;

                    if (parameters.Length > 0)
                    {
                        if ((parameters.Length == 1) && (parameters[0] is String))
                        {
                            fileName = parameters[0] as String;
                            validAttribute.DvtkDataAttribute.Length = (UInt32)fileName.Length;
                        }
                        else if (parameters.Length > 0 && parameters.Length <= 7)
                        {
                            bitmapPatternParameters = new DvtkData.Dimse.BitmapPatternParameters();

                            try
                            {
                                int parametersLength = parameters.Length;

                                if (parametersLength > 0)
                                {
                                    bitmapPatternParameters.NumberOfRows = System.Convert.ToUInt16(parameters[0]);

                                    if (parametersLength == 1)
                                    {
                                        bitmapPatternParameters.NumberOfColumns = bitmapPatternParameters.NumberOfRows;
                                    }
                                }
                                if (parametersLength > 1)
                                {
                                    bitmapPatternParameters.NumberOfColumns = System.Convert.ToUInt16(parameters[1]);
                                }
                                if (parametersLength > 2)
                                {
                                    bitmapPatternParameters.StartValue = System.Convert.ToUInt16(parameters[2]);
                                }
                                if (parametersLength > 3)
                                {
                                    bitmapPatternParameters.ValueIncrementPerRowBlock = System.Convert.ToUInt16(parameters[3]);
                                }
                                if (parametersLength > 4)
                                {
                                    bitmapPatternParameters.ValueIncrementPerColumnBlock = System.Convert.ToUInt16(parameters[4]);
                                }
                                if (parametersLength > 5)
                                {
                                    bitmapPatternParameters.NumberOfIdenticalValueRows = System.Convert.ToUInt16(parameters[5]);
                                }
                                if (parametersLength > 6)
                                {
                                    bitmapPatternParameters.NumberOfIdenticalValueColumns = System.Convert.ToUInt16(parameters[6]);
                                }
                            }
                            catch
                            {
                                bitmapPatternParameters = null;
                            }
                        }
                        else
                        {
                            // Do nothing.	
                        }
                    }

                    if (fileName != null)
                    {
                        FileNameImplementation = fileName;
                    }
                    else if (bitmapPatternParameters != null)
                    {
                        BitmapPatternParametersImplementation = bitmapPatternParameters;
                    }
                    else
                    {
                        // Do nothing.
                    }
                }
            }
            else if (this.attribute.VR == VR.UN)
            // VR UN.
            {
                if (parameters.Length > 0)
                {
                    if (zeroBasedIndex == 0)
                    {
                        // Contains a string hexadecimal representation of the bytes.
                        String parameterAsString = parameters[0].ToString();

                        if ((parameterAsString.Length % 2) != 0)
                        {
                            Thread.WriteWarningCurrentThread("Unable to interpret " + parameterAsString + " as a value for an attribute with VR UN because it has an unequal length.");
                        }
                        else
                        {
                            try
                            {
                                ArrayList bytesAsArrayList = new ArrayList();

                                int bytesLength = parameterAsString.Length / 2;

                                for (int byteIndex = 0; byteIndex < bytesLength; byteIndex++)
                                {
                                    Byte mostSignificantBits = byte.Parse(parameterAsString.Substring(byteIndex * 2, 1), NumberStyles.AllowHexSpecifier);
                                    Byte leastSignificantBits = byte.Parse(parameterAsString.Substring((byteIndex * 2) + 1, 1), NumberStyles.AllowHexSpecifier);
                                    Byte completeByte = (Byte)((mostSignificantBits * 16) + leastSignificantBits);
                                    bytesAsArrayList.Add(completeByte);
                                }

                                ByteArrayImplementation = (System.Byte[])bytesAsArrayList.ToArray(typeof(System.Byte));

                            }
                            catch
                            {
                                throw new System.ArgumentException("Unable to interpret " + parameterAsString + " as a value for an attribute with VR UN.");
                            }
                        }
                    }
                    else
                    {
                        Thread.WriteWarningCurrentThread("Values.Insert(...): unable to insert value in attribute with VR UN at index " + zeroBasedIndex.ToString());
                    }
                }
            }
            else if (this.attribute.VR == VR.SQ)
            // VR SQ
            {
                // Do nothing.
            }
            else
            // Other cases.
            {
                // Do nothing.
            }
        }

        /// <summary>
        /// Inserts another Values instance in this instance at a specified position.
        /// </summary>
        /// <param name="zeroBasedIndex">The zero based index to insert.</param>
        /// <param name="values">The Values instance to insert.</param>
        public void Insert(int zeroBasedIndex, Values values)
        {
            if ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW))
            {
                ValidAttribute thisAttribute = this.attribute as ValidAttribute;
                ValidAttribute attributeToCopyFrom = values.Attribute as ValidAttribute;

                if ((thisAttribute != null) && (attributeToCopyFrom != null))
                {
                    // It is safe to use the same other...String object because it is never
                    // changed through the HLI.
                    thisAttribute.DvtkDataAttribute.DicomValue = attributeToCopyFrom.DvtkDataAttribute.DicomValue;
                    thisAttribute.DvtkDataAttribute.Length = attributeToCopyFrom.DvtkDataAttribute.Length;
                }
            }
            else
            {
                String[] valuesAsStringArray = new String[values.Count];

                for (int index = 0; index < values.Count; index++)
                {
                    valuesAsStringArray[index] = values[index];
                }

                Insert(zeroBasedIndex, valuesAsStringArray);
            }
        }

        /// <summary>
        /// Inserts an array in a DvtkData collection.
        /// </summary>
        /// <param name="zeroBasedIndex">The zero based index.</param>
        /// <param name="parameters">The array.</param>
        internal void InsertCollectionValues(int zeroBasedIndex, params Object[] parameters)
        {
            switch (this.attribute.VR)
            {
                case VR.AE: // Application Entity
                case VR.AS: // Age String
                case VR.CS: // Code String
                case VR.DA: // Date
                case VR.DS: // Decimal String
                case VR.DT: // Date Time
                case VR.IS: // Integer String
                case VR.LO: // Long String
                case VR.PN: // Person Name
                case VR.SH: // Short String
                case VR.TM: // Time
                case VR.UI: // Unique Identifier (UID)
                case VR.UC:
                    {
                        DvtkData.Collections.StringCollection collectionToAdd = ConvertToStringCollection(parameters);
                        DvtkData.Collections.StringCollection currentCollection = (CollectionImplementation as DvtkData.Collections.StringCollection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                case VR.AT: // Attribute Tag
                    {
                        DvtkData.Collections.TagCollection collectionToAdd = ConvertToTagCollection(parameters);
                        DvtkData.Collections.TagCollection currentCollection = (CollectionImplementation as DvtkData.Collections.TagCollection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                case VR.FD: // Floating Point Double
                    {
                        DvtkData.Collections.DoubleCollection collectionToAdd = ConvertToDoubleCollection(parameters);
                        DvtkData.Collections.DoubleCollection currentCollection = (CollectionImplementation as DvtkData.Collections.DoubleCollection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                case VR.FL: // Floating Point Single
                    {
                        DvtkData.Collections.SingleCollection collectionToAdd = ConvertToSingleCollection(parameters);
                        DvtkData.Collections.SingleCollection currentCollection = (CollectionImplementation as DvtkData.Collections.SingleCollection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                case VR.SL: // Signed Long
                    {
                        DvtkData.Collections.Int32Collection collectionToAdd = ConvertToInt32Collection(parameters);
                        DvtkData.Collections.Int32Collection currentCollection = (CollectionImplementation as DvtkData.Collections.Int32Collection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                case VR.SS: // Signed Short
                    {
                        DvtkData.Collections.Int16Collection collectionToAdd = ConvertToInt16Collection(parameters);
                        DvtkData.Collections.Int16Collection currentCollection = (CollectionImplementation as DvtkData.Collections.Int16Collection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                case VR.UL: // Unsigned Long
                    {
                        DvtkData.Collections.UInt32Collection collectionToAdd = ConvertToUInt32Collection(parameters);
                        DvtkData.Collections.UInt32Collection currentCollection = (CollectionImplementation as DvtkData.Collections.UInt32Collection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                case VR.US: // Unsigned Short
                    {
                        DvtkData.Collections.UInt16Collection collectionToAdd = ConvertToUInt16Collection(parameters);
                        DvtkData.Collections.UInt16Collection currentCollection = (CollectionImplementation as DvtkData.Collections.UInt16Collection);
                        for (int index = 0; index < collectionToAdd.Count; index++)
                        {
                            currentCollection.Insert(zeroBasedIndex + index, collectionToAdd[index]);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Randomizes this instance.
        /// </summary>
        /// <remarks>
        /// Randomization is performed by replacing each <paramref name="stringToReplace"/> with a random digit between 0 and 9.
        /// </remarks>
        /// <param name="stringToReplace">The String to replace (may not be empty).</param>
        /// <param name="random">The Random instance used to create a random String (may not be null).</param>
        internal void Randomize(String stringToReplace, Random random)
        {
            ValidAttribute validAttribute = this.attribute as ValidAttribute;

            if (validAttribute == null)
            // Associated Attribute is InvalidAttribute.
            {
                // Do nothing.
            }
            else
            {
                switch (validAttribute.VR)
                {
                    case VR.AE:
                    case VR.CS:
                    case VR.DS:
                    case VR.DT:
                    case VR.IS:
                    case VR.LO:
                    case VR.PN:
                    case VR.SH:
                    case VR.TM:
                    case VR.UI:
                        for (int index = 0; index < Count; index++)
                        {
                            String singleValue = this[index];
                            bool changed = false;

                            int indexOfFirstStringToReplace = singleValue.IndexOf(stringToReplace);

                            while (indexOfFirstStringToReplace != -1)
                            {
                                String digit = (Convert.ToUInt32(random.NextDouble() * 9)).ToString();

                                singleValue = singleValue.Remove(indexOfFirstStringToReplace, stringToReplace.Length);
                                singleValue = singleValue.Insert(indexOfFirstStringToReplace, digit);

                                indexOfFirstStringToReplace = singleValue.IndexOf(stringToReplace);

                                changed = true;
                            }

                            if (changed)
                            {
                                this[index] = singleValue;
                            }
                        }

                        break;

                    default:
                        // Do nothing.
                        break;
                }
            }
        }

        /// <summary>
        /// Removed a single value in this instance at the specified position.
        /// </summary>
        /// <param name="zeroBasedIndex">The zero based index.</param>
        public void RemoveAt(int zeroBasedIndex)
        {
            ValidAttribute validAttribute = this.attribute as ValidAttribute;

            if (validAttribute == null)
            // Associated Attribute is InvalidAttribute.
            {
                // Do nothing.
            }
            else if (IsImplementedWithCollection)
            // Collection implementation.
            {
                if ((zeroBasedIndex < 0) || (zeroBasedIndex > Count))
                {
                    // Do nothing.
                }
                else
                {
                    CollectionImplementation.RemoveAt(zeroBasedIndex);
                    SetLength();
                }
            }
            else if (IsImplementedWithString)
            // String implementation.
            {
                if (zeroBasedIndex == 0)
                {
                    StringImplementation = null;
                    SetLength();
                }
            }
            else if ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW))
            // File or pattern implementation.
            {
                if (zeroBasedIndex == 0)
                {
                    FileNameImplementation = null;
                    BitmapPatternParametersImplementation = null;
                }
            }
            else if (this.attribute.VR == VR.UN)
            // VR UN.
            {
                // Do nothing.
            }
            else if (this.attribute.VR == VR.SQ)
            // VR SQ
            {
                // Do nothing.
            }
            else
            // Other cases.
            {
                // Do nothing.
            }
        }

        /// <summary>
        /// Removes non significant spaces according to the Dicom standard.
        /// </summary>
        /// <remarks>
        /// In part 5 of the Dicom standard, for specific VR's leading and/or traling spaces are non significant.
        /// This methods returns a String in which the non significant spaces have been removed.
        /// Also for VR DA and DT, traling spaces are removed because they may be present in C-Find queries.
        /// </remarks>
        /// <param name="theString">The String to (possibly) remove the spaces from.</param>
        /// <param name="vR">The VR of the associated Attribute.</param>
        /// <returns>The String with the non significant spaces removed.</returns>
        private static String RemoveNonSignificantSpaces(String theString, VR vR)
        {
            bool removeLeadingSpaces = false;
            bool removeTrailingSpaces = false;

            switch (vR)
            {
                case VR.AE:
                case VR.CS:
                case VR.DS:
                case VR.IS:
                case VR.LO:
                case VR.SH:
                    removeLeadingSpaces = true;
                    removeTrailingSpaces = true;
                    break;

                case VR.LT:
                case VR.PN:
                case VR.ST:
                case VR.TM:
                case VR.UT:
                case VR.UC:
                case VR.UR:
                    removeTrailingSpaces = true;
                    break;

                // For the VR's below, nothing is mentioned about non significant spaces in part 5.
                // In queries however, when using range matching, spaces may be appended at the end to make
                // it an even amount of characters. For this reason, the traling spaces are also removed for
                // these VR's.
                case VR.DA:
                case VR.DT:
                    removeTrailingSpaces = true;
                    break;
            }

            // If removeLeadingSpaces is true, remove all leading spaces.
            if (removeLeadingSpaces)
            {
                bool continueRemoving = true;

                while (continueRemoving)
                {
                    if (theString.Length > 0)
                    {
                        if (theString[0] == ' ')
                        {
                            // Remove the first space of this string.
                            theString = theString.Substring(1);
                        }
                        else
                        {
                            continueRemoving = false;
                        }
                    }
                    else
                    {
                        continueRemoving = false;
                    }
                }
            }

            // If removeTrailingSpaces is true, remove all trailing spaces.
            if (removeTrailingSpaces)
            {
                bool continueRemoving = true;

                while (continueRemoving)
                {
                    if (theString.Length > 0)
                    {
                        if (theString[theString.Length - 1] == ' ')
                        {
                            // Remove the last space of this string.
                            theString = theString.Substring(0, theString.Length - 1);
                        }
                        else
                        {
                            continueRemoving = false;
                        }
                    }
                    else
                    {
                        continueRemoving = false;
                    }
                }
            }

            return (theString);
        }

        internal void Set(Byte[] value)
        {
            ValidAttribute validAttribute = this.attribute as ValidAttribute;

            if (validAttribute == null)
            // Associated Attribute is InvalidAttribute.
            {
                // Do nothing.
            }
            else
            {
                //
                // Write the content of the Byte array to a temp file.
                //

                String tempFileFullPath = Path.GetTempFileName();
                FileStream fileStream = new FileStream(tempFileFullPath, FileMode.Create);
                BinaryWriter binaryWriter = new BinaryWriter(fileStream);

                binaryWriter.Write(value);

                binaryWriter.Close();
                fileStream.Close();


                //
                //// Use the created temp file and make sure it will be removed at some point.
                ////

                //// Make the attribute point to this temp file for its value.
                validAttribute.DvtkDataAttribute.Length = (UInt32)tempFileFullPath.Length;
                FileNameImplementation = tempFileFullPath;

                //// Make sure that the temp file is removed when the executable ends.
                HighLevelInterface.TempFileCollection.AddFile(tempFileFullPath, false);
            }
        }

        /// <summary>
        /// Sets the new DvtkData Attribute length for this values instance.
        /// </summary>
        /// <remarks>
        /// Use this when the values content of this instance has changed. The implementation below is
        /// copied from the DimseDataClasses._InitializeAttribute method and adjusted.
        /// </remarks>
        private void SetLength()
        {
            ValidAttribute validAttribute = this.attribute as ValidAttribute;

            if (validAttribute == null)
            // Associated Attribute is InvalidAttribute.
            {
                // Do nothing.
            }
            else
            {
                UInt32 newDvtkDataLength = 0;

                switch (validAttribute.VR)
                {
                    case VR.AE:
                    case VR.CS:
                    case VR.DS:
                    case VR.DT:
                    case VR.IS:
                    case VR.LO:
                    case VR.PN:
                    case VR.SH:
                    case VR.TM:
                    case VR.UI:
                        {
                            DvtkData.Collections.StringCollection stringCollection = (CollectionImplementation as DvtkData.Collections.StringCollection);

                            if (stringCollection.Count > 0)
                            {
                                foreach (String data in stringCollection)
                                {
                                    newDvtkDataLength += (System.UInt32)data.Length;
                                }
                                newDvtkDataLength = newDvtkDataLength + (System.UInt32)stringCollection.Count - 1;
                            }
                            break;
                        }

                    case VR.AS:
                    case VR.AT:
                        {
                            newDvtkDataLength = (System.UInt32)CollectionImplementation.Count * 4;
                            break;
                        }

                    case VR.DA:
                        {
                            DvtkData.Collections.StringCollection stringCollection = (CollectionImplementation as DvtkData.Collections.StringCollection);

                            if (stringCollection.Count > 0)
                            {
                                foreach (String data in stringCollection)
                                {
                                    newDvtkDataLength += (System.UInt32)data.Length;
                                }
                            }
                            break;
                        }

                    case VR.FD:
                        {
                            newDvtkDataLength = (System.UInt32)CollectionImplementation.Count * 8;
                            break;
                        }

                    case VR.FL:
                    case VR.SL:
                    case VR.UL:
                        {
                            newDvtkDataLength = (System.UInt32)CollectionImplementation.Count * 4;
                            break;
                        }

                    case VR.LT:
                    case VR.ST:
                    case VR.UT:
                    case VR.UR:
                        {
                            if (StringImplementation != null)
                            {
                                newDvtkDataLength = (System.UInt32)StringImplementation.Length;
                            }
                            break;
                        }

                    case VR.OB:
                    case VR.OF:
                    case VR.OW:
                    case VR.OD:
                    case VR.OL:
                    case VR.OV:
                    case VR.SQ:
                        {
                            // Do nothing.
                            break;
                        }

                    case VR.UN:
                        {
                            DvtkData.Dimse.Unknown unknown = validAttribute.DvtkDataAttribute.DicomValue as DvtkData.Dimse.Unknown;
                            newDvtkDataLength = (System.UInt32)unknown.ByteArray.Length;
                            break;
                        }

                    case VR.SS:
                    case VR.US:
                    case VR.UC:
                        {
                            newDvtkDataLength = (System.UInt32)CollectionImplementation.Count * 2;
                            break;
                        }

                    default:
                        newDvtkDataLength = 0;
                        break;
                }

                validAttribute.DvtkDataAttribute.Length = newDvtkDataLength;
            }
        }

        /// <summary>
        /// Returns a String that represents this instance.
        /// </summary>
        /// <returns>A String that represents this instance.</returns>
        public override String ToString()
        {
            String toString = "";

            if ((attribute.VR == VR.OB) || (attribute.VR == VR.OF) || (attribute.VR == VR.OW))
            {
                ValidAttribute validAttribute = this.attribute as ValidAttribute;

                if (validAttribute == null)
                {
                    toString = "";
                }
                else
                {
                    if (FileNameImplementation != null)
                    {
                        toString += "\"" + this[0] + "\"";
                    }
                    else if (BitmapPatternParametersImplementation != null)
                    {
                        toString = this[0];
                    }
                    else
                    {
                        toString = "";
                    }
                }
            }
            else
            {
                for (int index = 0; index < Count; index++)
                {
                    toString += "\"" + this[index] + "\"";

                    if (index < Count - 1)
                    {
                        toString += ", ";
                    }
                }
            }

            return (toString);
        }
	}
}
