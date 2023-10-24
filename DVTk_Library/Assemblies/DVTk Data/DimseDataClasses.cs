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
using System.ComponentModel;
using System.Globalization;

using DvtkDicomUnicodeConversion;

namespace DvtkData.Dimse
{
    using System.IO;
    using System.Collections;
    using DvtkData.DvtDetailToXml;
    //
    // Set aliases for types.
    //
    using SIGNED_SHORT = System.Int16;
    using UNSIGNED_SHORT = System.UInt16;
    using SIGNED_LONG = System.Int32;
    using SIGNED_VERY_LONG = System.Int64;
    using UNSIGNED_LONG = System.UInt32;
    using UNSIGNED_VERY_LONG = System.UInt64;
    using GROUP_NUMBER = System.UInt16;
    using ELEMENT_NUMBER = System.UInt16;
    using GROUP_ELEMENT = System.UInt32;
    using FPD = System.Double;
    using FPS = System.Single;

    /// <summary>
    /// AttributeSet is the abstract base-class for sub-classes <see cref="DvtkData.Dimse.CommandSet"/> and <see cref="DvtkData.Dimse.DataSet"/>
    /// </summary>
    /// <remarks>
    /// Implements the logic for attribute indexing and settings for 
    /// group length encoding and attribute value population.
    /// </remarks>
    public abstract class AttributeSet : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {
        /// <summary>
        /// Gets or sets the <see cref="Attribute"/> at the specified index.
        /// </summary>
        /// <value>The <see cref="Attribute"/> at the specified <c>index</c>.</value>
        public new Attribute this[int index]
        {
            get
            {
                return (Attribute)base[index];
            }
            set
            {
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Gets first occurence of <see cref="Attribute"/> with <see cref="Tag"/>.
        /// </summary>
        /// <param name="groupElement">tag</param>
        /// <returns>attribute. Returns <see langword="null"/> if not found.</returns>
        public Attribute GetAttribute(GROUP_ELEMENT groupElement)
        {
            Tag tag = groupElement;
            return GetAttribute(tag);
        }

        /// <summary>
        /// Gets first occurence of <see cref="Attribute"/> with <see cref="Tag"/>.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <returns>attribute. Returns <see langword="null"/> if not found.</returns>
        public Attribute GetAttribute(Tag tag)
        {
            foreach (Attribute attribute in this)
            {
                if (attribute.Tag.Equals(tag)) return attribute;
            }
            return null;
        }

        /// <summary>
        /// Gets first occurence of <see cref="Sequence"/> with <see cref="Tag"/>.
        /// </summary>
        /// <param name="groupElement">tag</param>
        /// <returns>sequence. Returns <see langword="null"/> if not found.</returns>
        public Sequence GetSequence(GROUP_ELEMENT groupElement)
        {
            Tag tag = groupElement;
            return GetSequence(tag);
        }

        /// <summary>
        /// Gets first occurence of <see cref="Sequence"/> with <see cref="Tag"/>.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <returns>sequence. Returns <see langword="null"/> if not found.</returns>
        public Sequence GetSequence(Tag tag)
        {
            Sequence sequence = null;
            foreach (Attribute attribute in this)
            {
                if (
                    attribute.Tag.Equals(tag) &&
                    attribute.DicomValue != null &&
                    attribute.DicomValue is DvtkData.Dimse.SequenceOfItems
                    )
                {
                    sequence = (attribute.DicomValue as DvtkData.Dimse.SequenceOfItems).Sequence;
                }
            }
            return sequence;
        }

        /// <summary>
        /// Inserts an <see cref="Attribute"/> to the <see cref="AttributeSet"/> at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The <see cref="Attribute"/> to insert into the <see cref="AttributeSet"/>.</param>
        public void Insert(int index, Attribute value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Make all attributes in this AttributeSet ascending.
        /// This method is non-recursive: attributes contained in sequence attributes will not be sorted.
        /// </summary>
        public void MakeAscending()
        {
            this.InnerList.Sort(new DvtkData.Dimse.Attribute.Comparer());
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="Attribute"/> from the <see cref="AttributeSet"/>.
        /// </summary>
        /// <param name="value">The <see cref="Attribute"/> to remove from the <see cref="AttributeSet"/>.</param>
        public void Remove(Attribute value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="AttributeSet"/> contains a specific <see cref="Attribute"/>.
        /// </summary>
        /// <param name="value">The <see cref="Attribute"/> to locate in the <see cref="AttributeSet"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="Attribute"/> is found in the <see cref="AttributeSet"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(Attribute value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific <see cref="Attribute"/> in the <see cref="AttributeSet"/>.
        /// </summary>
        /// <param name="value">The <see cref="Attribute"/> to locate in the <see cref="AttributeSet"/>.</param>
        /// <returns>The index of <c>value</c> if found in the <see cref="AttributeSet"/>; otherwise, -1.</returns>
        public int IndexOf(Attribute value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an <see cref="Attribute"/> to the <see cref="AttributeSet"/>.
        /// </summary>
        /// <param name="value">The <see cref="Attribute"/> to add to the <see cref="AttributeSet"/>. </param>
        /// <returns>The position into which the new <see cref="Attribute"/> was inserted.</returns>
        /// <example>This sample shows how to call the AddAttribute method.
        /// <code>
        ///   AttributeSet attributeSet = new DataSet();
        ///   Attribute attribute = new Attribute(0x12345678, VR.AE, "String0", "String1", "String2");
        ///   attributeSet.AddAttribute(attribute);
        /// </code>
        /// </example>
        public int Add(Attribute value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Adds an <see cref="Attribute"/> to the <see cref="AttributeSet"/>.
        /// </summary>
        /// <param name="groupNumber">The group number for the newly created <see cref="Attribute"/>.</param>
        /// <param name="elementNumber">The element number for the newly created <see cref="Attribute"/>.</param>
        /// <param name="vr">The value representation for the newly created <see cref="Attribute"/>.</param>
        /// <param name="list">The list of values for the newly create <see cref="Attribute"/></param>
        /// <returns>The position into which the new <see cref="Attribute"/> was inserted.</returns>
        /// <exception cref="System.InvalidCastException">
        /// The <c>list</c> is an <see cref="object"/> array. 
        /// The items in this <c>list</c> are converted to the underlying <c>vr</c> as much as possible. 
        /// However <see cref="System.InvalidCastException"/> may occur during these run-time conversions.
        /// </exception>
        /// <example>This sample shows how to call the AddAttribute method.
        /// <code>
        ///   AttributeSet attributeSet = new DataSet();
        ///   Attribute attribute = new Attribute(0x12345678, VR.AE, "String0", "String1", "String2");
        ///   attributeSet.AddAttribute(attribute);
        /// </code>
        /// </example>
        public int AddAttribute(
            GROUP_NUMBER groupNumber,
            ELEMENT_NUMBER elementNumber,
            VR vr,
            params object[] list)
        {
            return base.Add(new Attribute(groupNumber, elementNumber, vr, list));
        }

        /// <summary>
        /// Adds an <see cref="Attribute"/> to the <see cref="AttributeSet"/>.
        /// </summary>
        /// <param name="groupNumberString">The group number for the newly created <see cref="Attribute"/> in hex string format</param>
        /// <param name="elementNumberString">The element number for the newly created <see cref="Attribute"/> in hex string format</param>
        /// <param name="vr">The value representation for the newly created <see cref="Attribute"/></param>
        /// <param name="list"></param>
        /// <returns>The position into which the new <see cref="Attribute"/> was inserted.</returns>
        /// <exception cref="System.InvalidCastException">
        /// The <c>list</c> is an <see cref="object"/> array. 
        /// The items in this <c>list</c> are converted to the underlying <c>vr</c> as much as possible. 
        /// However <see cref="System.InvalidCastException"/> may occur during these run-time conversions.
        /// </exception>
        /// <example>This sample shows how to call the AddAttribute method.
        /// <code>
        ///   AttributeSet attributeSet = new DataSet();
        ///   Attribute attribute = new Attribute("0x12345678", VR.AE, "String0", "String1", "String2");
        ///   attributeSet.AddAttribute(attribute);
        /// </code>
        /// </example>
        public int AddAttribute(
            string groupNumberString,
            string elementNumberString,
            VR vr,
            params object[] list)
        {
            GROUP_NUMBER groupNumber =
                GROUP_NUMBER.Parse(groupNumberString, System.Globalization.NumberStyles.HexNumber);
            ELEMENT_NUMBER elementNumber =
                ELEMENT_NUMBER.Parse(elementNumberString, System.Globalization.NumberStyles.HexNumber);
            return base.Add(new Attribute(groupNumber, elementNumber, vr, list));
        }

        /// <summary>
        /// Adds an <see cref="Attribute"/> to the <see cref="AttributeSet"/>.
        /// </summary>
        /// <param name="groupElement">The group element hex number for the newly created <see cref="Attribute"/>.</param>
        /// <param name="vr">The value representation for the newly created <see cref="Attribute"/>.</param>
        /// <param name="list">The list of values for the newly create <see cref="Attribute"/></param>
        /// <returns>The position into which the new <see cref="Attribute"/> was inserted.</returns>
        /// <exception cref="System.InvalidCastException">
        /// The <c>list</c> is an <see cref="object"/> array. 
        /// The items in this <c>list</c> are converted to the underlying <c>vr</c> as much as possible. 
        /// However <see cref="System.InvalidCastException"/> may occur during these run-time conversions.
        /// </exception>
        /// <example>This sample shows how to call the AddAttribute method.
        /// <code>
        ///   AttributeSet attributeSet = new DataSet();
        ///   attributeSet.AddAttribute(0x12345678, VR.AE, "String0", "String1", "String2");
        /// </code>
        /// </example>
        public int AddAttribute(
            GROUP_ELEMENT groupElement,
            VR vr,
            params object[] list)
        {
            return base.Add(new Attribute(groupElement, vr, list));
        }
        /// <summary>
        /// Adds an <see cref="Attribute"/> to the <see cref="AttributeSet"/>.
        /// </summary>
        /// <param name="groupElementString">The group element hex number for the newly created <see cref="Attribute"/> in hex string format</param>
        /// <param name="vr">The value representation for the newly created <see cref="Attribute"/></param>
        /// <param name="list">The list of values for the newly create <see cref="Attribute"/></param>
        /// <returns>The position into which the new <see cref="Attribute"/> was inserted.</returns>
        /// <exception cref="System.InvalidCastException">
        /// The <c>list</c> is an <see cref="object"/> array. 
        /// The items in this <c>list</c> are converted to the underlying <c>vr</c> as much as possible. 
        /// However <see cref="System.InvalidCastException"/> may occur during these run-time conversions.
        /// </exception>
        /// <example>This sample shows how to call the AddAttribute method.
        /// <code>
        ///   AttributeSet attributeSet = new DataSet();
        ///   attributeSet.AddAttribute("0x12345678", VR.AE, "String0", "String1", "String2");
        /// </code>
        /// </example>
        public int AddAttribute(
            string groupElementString,
            VR vr,
            params object[] list)
        {
            GROUP_ELEMENT groupElement =
                GROUP_ELEMENT.Parse(groupElementString, System.Globalization.NumberStyles.HexNumber);
            return base.Add(new Attribute(groupElement, vr, list));
        }

        /// <summary>
        /// Remove any Group Length attributes from the AttributeSet.
        /// </summary>
        public void RemoveGroupLengthAttributes()
        {
            bool removingGroupLengths = true;
            bool removedGroupLength = false;
            while (removingGroupLengths == true)
            {
                // Removing an attribute changes the collection
                // - hence the use of these booleans to restart the iteration
                foreach (Attribute attribute in this)
                {
                    if (attribute.Tag.ElementNumber == (ushort)0x0000)
                    {
                        Remove(attribute);
                        removedGroupLength = true;
                        break;
                    }
                }
                removingGroupLengths = false;
                if (removedGroupLength == true)
                {
                    removingGroupLengths = true;
                    removedGroupLength = false;
                }
            }

            // Now handle any sequences
            foreach (Attribute attribute in this)
            {
                if ((attribute.ValueRepresentation == VR.SQ) &&
                    (attribute.DicomValue != null) &&
                    (attribute.DicomValue is DvtkData.Dimse.SequenceOfItems))
                {
                    Sequence sequence = (attribute.DicomValue as DvtkData.Dimse.SequenceOfItems).Sequence;
                    foreach (AttributeSet item in sequence)
                    {
                        item.RemoveGroupLengthAttributes();
                    }
                }
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public abstract bool DvtDetailToXml(StreamWriter streamWriter, int level);

        /// <summary>
        /// Display the AttributeSet to the Console - useful debugging utility
        /// </summary>
        public void ConsoleDisplay()
        {
            Console.Write(Dump(""));
        }

        /// <summary>
        /// Dumps the AttributeSet to a String - useful debugging utility
        /// </summary>
        /// <returns>The dump.</returns>
        public String Dump(String prefix)
        {
            String dumpString = "";

            foreach (DvtkData.Dimse.Attribute attribute in this)
            {
                string group = attribute.Tag.GroupNumber.ToString("X").PadLeft(4, '0');
                string element = attribute.Tag.ElementNumber.ToString("X").PadLeft(4, '0');

                dumpString += prefix + String.Format("({0},{1}), {2}, {3:00000000}", group, element, attribute.ValueRepresentation.ToString(), attribute.Length);
                if (attribute.Length != 0)
                {
                    switch (attribute.ValueRepresentation)
                    {
                        case VR.AE:
                            {
                                ApplicationEntity applicationEntity = (ApplicationEntity)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", applicationEntity.Values[0]);
                                break;
                            }
                        case VR.AS:
                            {
                                AgeString ageString = (AgeString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", ageString.Values[0]);
                                break;
                            }
                        case VR.AT:
                            {
                                AttributeTag attributeTag = (AttributeTag)attribute.DicomValue;
                                Console.WriteLine(", \"{0}\"\r\n", attributeTag.Values[0]);
                                break;
                            }
                        case VR.CS:
                            {
                                CodeString codeString = (CodeString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", codeString.Values[0]);
                                break;
                            }
                        case VR.DA:
                            {
                                Date date = (Date)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", date.Values[0]);
                                break;
                            }
                        case VR.DS:
                            {
                                DecimalString decimalString = (DecimalString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", decimalString.Values[0]);
                                break;
                            }
                        case VR.DT:
                            {
                                DvtkData.Dimse.DateTime dateTime = (DvtkData.Dimse.DateTime)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", dateTime.Values[0]);
                                break;
                            }
                        case VR.FD:
                            {
                                FloatingPointDouble floatingPointDouble = (FloatingPointDouble)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", floatingPointDouble.Values[0]);
                                break;
                            }
                        case VR.FL:
                            {
                                FloatingPointSingle floatingPointSingle = (FloatingPointSingle)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", floatingPointSingle.Values[0]);
                                break;
                            }
                        case VR.IS:
                            {
                                IntegerString integerString = (IntegerString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", integerString.Values[0]);
                                break;
                            }
                        case VR.LO:
                            {
                                LongString longString = (LongString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", longString.Values[0]);
                                break;
                            }
                        case VR.LT:
                            {
                                LongText longText = (LongText)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", longText.Value);
                                break;
                            }
                        case VR.OB:
                            {
                                OtherByteString otherByteString = (OtherByteString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", otherByteString.FileName);
                                break;
                            }
                        case VR.OF:
                            {
                                OtherFloatString otherFloatString = (OtherFloatString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", otherFloatString.FileName);
                                break;
                            }
                        case VR.OW:
                            {
                                OtherWordString otherWordString = (OtherWordString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", otherWordString.FileName);
                                break;
                            }
                        case VR.OL:
                            {
                                OtherLongString otherLongString = (OtherLongString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", otherLongString.FileName);
                                break;
                            }
                        case VR.OD:
                            {
                                OtherDoubleString otherDoubleString = (OtherDoubleString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", otherDoubleString.FileName);
                                break;
                            }
                        case VR.OV:
                            {
                                OtherVeryLongString otherVeryLongString = (OtherVeryLongString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", otherVeryLongString.FileName);
                                break;
                            }
                        case VR.PN:
                            {
                                PersonName personName = (PersonName)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", personName.Values[0]);
                                break;
                            }
                        case VR.SH:
                            {
                                ShortString shortString = (ShortString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", shortString.Values[0]);
                                break;
                            }
                        case VR.SL:
                            {
                                SignedLong signedLong = (SignedLong)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", signedLong.Values[0]);
                                break;
                            }
                        case VR.SQ:
                            {
                                SequenceOfItems sequenceOfItems = (SequenceOfItems)attribute.DicomValue;
                                int itemNumber = 1;
                                dumpString += "\r\n";
                                foreach (SequenceItem item in sequenceOfItems.Sequence)
                                {
                                    dumpString += prefix + String.Format("> Begin Item: {0}\r\n", itemNumber);
                                    dumpString += item.Dump(prefix);
                                    dumpString += prefix + String.Format("> End Item: {0}\r\n", itemNumber++);
                                }
                                break;
                            }
                        case VR.SS:
                            {
                                SignedShort signedShort = (SignedShort)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", signedShort.Values[0]);
                                break;
                            }
                        case VR.ST:
                            {
                                ShortText shortText = (ShortText)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", shortText.Value);
                                break;
                            }
                        case VR.SV:
                            {
                                SignedVeryLongString signedVeryLongString = (SignedVeryLongString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", signedVeryLongString.Values[0]);
                                break;
                            }
                        case VR.TM:
                            {
                                Time time = (Time)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", time.Values[0]);
                                break;
                            }
                        case VR.UI:
                            {
                                UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", uniqueIdentifier.Values[0]);
                                break;
                            }
                        case VR.UL:
                            {
                                UnsignedLong unsignedLong = (UnsignedLong)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", unsignedLong.Values[0]);
                                break;
                            }
                        case VR.UN:
                            {
                                break;
                            }
                        case VR.US:
                            {
                                UnsignedShort unsignedShort = (UnsignedShort)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", unsignedShort.Values[0]);
                                break;
                            }
                        case VR.UV:
                            {
                                UnsignedVeryLongString unsignedVeryLongString = (UnsignedVeryLongString)attribute.DicomValue;
                                dumpString += String.Format(", \"{0}\"\r\n", unsignedVeryLongString.Values[0]);
                                break;
                            }
                        case VR.UT:
                            {
                                break;
                            }
                        case VR.UR:
                            {
                                break;
                            }
                        case VR.UC:
                            {
                                break;
                            }
                        default:
                            dumpString += String.Format("\r\n");
                            break;
                    }
                }
                else
                {
                    dumpString += String.Format("\r\n");
                }
            }

            return (dumpString);
        }
    }

    /// <summary>
    /// DATA SET: Exchanged information consisting of a structured set of 
    /// Attribute values directly or indirectly related to Information Objects.
    /// </summary>
    /// <remarks>
    /// The value of each Attribute in a Data Set is expressed as a Data Element.
    /// A collection of Data Elements ordered by increasing Data Element Tag number that 
    /// is an encoding of the values of Attributes of a real world object.
    /// 
    /// Data Set: Exchanged information consisting of a structured set of Attributes.
    /// The value of each Attribute in a Data Set is expressed as a Data Element.
    /// </remarks>
    public class DataSet : AttributeSet
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataSet() { }

        /// <summary>
        /// Constructor with specific identifier for the Information Object Definition (IOD).
        /// The IOD identifies the definition to be used during the validation of the <see cref="DataSet"/>.
        /// </summary>
        /// <param name="iodId">Identifier for the (DICOM) Information Oject Definition (IOD).</param>
        /// <remarks>The IOD identifier is part of the definition file that is input for the validation process of DVTK.</remarks>
        public DataSet(System.String iodId)
        {
            this._IodId = iodId;
        }

        /// <summary>
        /// IOD identifier forms an DVT application specific identifier that specifies the
        /// Information Object Class Definition to use during the validation of Dicom message exchange.
        /// </summary>
        /// <remarks>
        /// Each Information Object Class definition consists of a description of its purpose and 
        /// the Attributes which define it.
        /// </remarks>
        public System.String IodId
        {
            get
            {
                return _IodId;
            }
            set
            {
                this._IodId = value;
            }
        }
        private System.String _IodId;

        /// <summary>
        /// Filename that contains this DataSet.
        /// </summary>
        public System.String Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                this._Filename = value;
            }
        }
        private System.String _Filename;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            // try to get the specific character set attribute
            // - this is needed to instantiate the DICOM to Unicode converter.
            DicomUnicodeConverter dicomUnicodeConverter = null;
            Attribute specificCharacterSetAttribute = this.GetAttribute(Tag.SPECIFIC_CHARACTER_SET);
            if ((specificCharacterSetAttribute != null) &&
                (specificCharacterSetAttribute.ValueRepresentation == VR.CS))
            {
                CodeString codeString = (CodeString)specificCharacterSetAttribute.DicomValue;
                String specificCharacterSetValue = String.Empty;
                for (int i = 0; i < codeString.Values.Count; i++)
                {
                    specificCharacterSetValue += codeString.Values[i];
                    if ((i + 1) < codeString.Values.Count)
                    {
                        specificCharacterSetValue += "\\";
                    }
                }

                // used fixed location for Unicode character sets based on the application start-up directory
                //String baseDirectory = String.Format("{0}CHARACTERSETS", System.AppDomain.CurrentDomain.BaseDirectory);
                String baseDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "CHARACTERSETS");
                dicomUnicodeConverter = new DicomUnicodeConverter(baseDirectory);
                if (dicomUnicodeConverter.Install(specificCharacterSetValue) == false)
                {
                    dicomUnicodeConverter = null;
                }
            }

            // serialize the dataset
            bool result = false;
            streamWriter.WriteLine("<Dataset Name=\"{0}\">", IodId);
            foreach (Attribute attribute in this)
            {
                attribute.DicomUnicodeConverter = dicomUnicodeConverter;
                result = attribute.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</Dataset>");
            return result;
        }
    }

    /// <summary>
    /// DIMSE Command Identifier
    /// </summary>
    public enum DimseCommand : short  // short maps to .Net ValueType System.Int16
    {
        /// <summary>
        /// 0x0001 Hex
        /// </summary>
        CSTORERQ = 0x0001,
        /// <summary>
        /// 0x8001 Hex
        /// </summary>
        CSTORERSP = unchecked((short)0x8001),
        /// <summary>
        /// 0x0020 Hex
        /// </summary>
        CFINDRQ = 0x0020,
        /// <summary>
        /// 0x8020 Hex
        /// </summary>
        CFINDRSP = unchecked((short)0x8020),
        /// <summary>
        /// 0x0FFF Hex
        /// </summary>
        CCANCELRQ = 0x0FFF,
        /// <summary>
        /// 0x0010 Hex
        /// </summary>
        CGETRQ = 0x0010,
        /// <summary>
        /// 0x8010 Hex
        /// </summary>
        CGETRSP = unchecked((short)0x8010),
        /// <summary>
        /// 0x0021 Hex
        /// </summary>
        CMOVERQ = 0x0021,
        /// <summary>
        /// 0x8021 Hex
        /// </summary>
        CMOVERSP = unchecked((short)0x8021),
        /// <summary>
        /// 0x0030 Hex
        /// </summary>
        CECHORQ = 0x0030,
        /// <summary>
        /// 0x8030 Hex
        /// </summary>
        CECHORSP = unchecked((short)0x8030),
        /// <summary>
        /// 0x0100 Hex
        /// </summary>
        NEVENTREPORTRQ = 0x0100,
        /// <summary>
        /// 0x8100 Hex
        /// </summary>
        NEVENTREPORTRSP = unchecked((short)0x8100),
        /// <summary>
        /// 0x0110 Hex
        /// </summary>
        NGETRQ = 0x0110,
        /// <summary>
        /// 0x8110 Hex
        /// </summary>
        NGETRSP = unchecked((short)0x8110),
        /// <summary>
        /// 0x0120 Hex
        /// </summary>
        NSETRQ = 0x0120,
        /// <summary>
        /// 0x8120 Hex
        /// </summary>
        NSETRSP = unchecked((short)0x8120),
        /// <summary>
        /// 0x0130 Hex
        /// </summary>
        NACTIONRQ = 0x0130,
        /// <summary>
        /// 0x8130 Hex
        /// </summary>
        NACTIONRSP = unchecked((short)0x8130),
        /// <summary>
        /// 0x0140 Hex
        /// </summary>
        NCREATERQ = 0x0140,
        /// <summary>
        /// 0x8140 Hex
        /// </summary>
        NCREATERSP = unchecked((short)0x8140),
        /// <summary>
        /// 0x0150 Hex
        /// </summary>
        NDELETERQ = 0x0150,
        /// <summary>
        /// 0x8150 Hex
        /// </summary>
        NDELETERSP = unchecked((short)0x8150),
        /// <summary>
        /// 0x0000 Hex
        /// </summary>
        UNDEFINED = 0x0000,
    }
    /// <summary>
    /// The Command Set is used to indicate the operations/notifications 
    /// to be performed on or with the Data Set.
    /// </summary>
    /// <remarks>
    /// A Command Set is constructed of Command Elements. 
    /// Command Elements contain the encoded values for each individual field of the 
    /// Command Set per the semantics specified in the DIMSE protocol (see Section 9.2 and 10.2). 
    /// Each Command Element is composed of an explicit Tag, a Value Length, and a Value Field.
    /// </remarks>
    public class CommandSet : AttributeSet
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public CommandSet() { }

        /// <summary>
        /// Constructor with specific DIMSE command identifier specified.
        /// </summary>
        /// <param name="dimseCommand">Specifies the type of message encoded by the <see cref="CommandSet"/></param>
        public CommandSet(DimseCommand dimseCommand)
        {
            switch (dimseCommand)
            {
                case DimseCommand.CCANCELRQ:
                case DimseCommand.CECHORQ:
                case DimseCommand.CECHORSP:
                case DimseCommand.CFINDRQ:
                case DimseCommand.CFINDRSP:
                case DimseCommand.CGETRQ:
                case DimseCommand.CGETRSP:
                case DimseCommand.CMOVERQ:
                case DimseCommand.CMOVERSP:
                case DimseCommand.CSTORERQ:
                case DimseCommand.CSTORERSP:
                case DimseCommand.NACTIONRQ:
                case DimseCommand.NACTIONRSP:
                case DimseCommand.NCREATERQ:
                case DimseCommand.NCREATERSP:
                case DimseCommand.NDELETERQ:
                case DimseCommand.NDELETERSP:
                case DimseCommand.NEVENTREPORTRQ:
                case DimseCommand.NEVENTREPORTRSP:
                case DimseCommand.NGETRQ:
                case DimseCommand.NGETRSP:
                case DimseCommand.NSETRQ:
                case DimseCommand.NSETRSP:
                    {
                        System.Int16 commandFieldInt16 = (System.Int16)dimseCommand;
                        byte[] bytes = System.BitConverter.GetBytes(commandFieldInt16);
                        System.UInt16 commandFieldUInt16 = System.BitConverter.ToUInt16(bytes, 0);
                        this.AddAttribute(
                            0x0000,
                            0x0100,
                            VR.US,
                            commandFieldUInt16);
                        break;
                    }
                case DimseCommand.UNDEFINED:
                    // Do not add command field attribute.
                    break;
                default:
                    throw new System.ArgumentException();
            }
        }

        /// <summary>
        /// Get a string repesentation of the command.
        /// </summary>
        /// <param name="dimseCommand">Specifies the type of message encoded by the <see cref="CommandSet"/></param>
        public string ToString(DimseCommand dimseCommand)
        {
            string command;
            switch (dimseCommand)
            {
                case DimseCommand.CCANCELRQ: command = "C-CANCEL-RQ"; break;
                case DimseCommand.CECHORQ: command = "C-ECHO-RQ"; break;
                case DimseCommand.CECHORSP: command = "C-ECHO-RSP"; break;
                case DimseCommand.CFINDRQ: command = "C-FIND-RQ"; break;
                case DimseCommand.CFINDRSP: command = "C-FIND-RSP"; break;
                case DimseCommand.CGETRQ: command = "C-GET-RQ"; break;
                case DimseCommand.CGETRSP: command = "C-GET-RSP"; break;
                case DimseCommand.CMOVERQ: command = "C-MOVE-RQ"; break;
                case DimseCommand.CMOVERSP: command = "C-MOVE-RSP"; break;
                case DimseCommand.CSTORERQ: command = "C-STORE-RQ"; break;
                case DimseCommand.CSTORERSP: command = "C-STORE-RSP"; break;
                case DimseCommand.NACTIONRQ: command = "N-ACTION-RQ"; break;
                case DimseCommand.NACTIONRSP: command = "N-ACTION-RSP"; break;
                case DimseCommand.NCREATERQ: command = "N-CREATE-RQ"; break;
                case DimseCommand.NCREATERSP: command = "N-CREATE-RSP"; break;
                case DimseCommand.NDELETERQ: command = "N-DELETE-RQ"; break;
                case DimseCommand.NDELETERSP: command = "N-DELETE-RSP"; break;
                case DimseCommand.NEVENTREPORTRQ: command = "N-EVENT-REPORT-RQ"; break;
                case DimseCommand.NEVENTREPORTRSP: command = "N-EVENT-REPORT-RSP"; break;
                case DimseCommand.NGETRQ: command = "N-GET-RQ"; break;
                case DimseCommand.NGETRSP: command = "N-GET-RSP"; break;
                case DimseCommand.NSETRQ: command = "N-SET-RQ"; break;
                case DimseCommand.NSETRSP: command = "N-SET-RSP"; break;
                case DimseCommand.UNDEFINED: command = "UNDEFINED"; break;
                default: throw new System.ArgumentException();
            }
            return command;
        }

        /// <summary>
        /// Specifies the type of DIMSE command encoded by the <see cref="CommandSet"/>
        /// </summary>
        /// <remarks>
        /// <p>
        /// This is a derived property. The property is set by means of a <see cref="Attribute"/> on the <see cref="CommandSet"/>.
        /// </p>
        /// <p>
        /// When the CommandField is set to a value different from <see cref="DimseCommand.UNDEFINED"/>, then an (CommandField) <see cref="Attribute"/> with tag 0x0000, 0x0100is present that specifies the DIMSE command type.
        /// </p>
        /// </remarks>
        public DimseCommand CommandField
        {
            get
            {
                DimseCommand commandField = DimseCommand.UNDEFINED;
                Tag commandFieldTag = new Tag(0x0000, 0x0100);
                foreach (Attribute att in this)
                {
                    if ((att.Tag.Equals(commandFieldTag)) &&
                        (att.DicomValue is UnsignedShort))
                    {
                        UnsignedShort us = (UnsignedShort)att.DicomValue;
                        if (us.Values.Count > 0)
                        {
                            System.UInt16 commandFieldUInt16 = us.Values[0];
                            byte[] bytes = System.BitConverter.GetBytes(commandFieldUInt16);
                            System.Int16 commandFieldInt16 =
                                System.BitConverter.ToInt16(bytes, 0);
                            commandField = _Convert(commandFieldInt16);
                        }
                        break;
                    }
                }
                return commandField;
            }
        }
        /// <summary>
        /// DIMSE-C and DIMSE-N Services
        /// </summary>
        /// <remarks>
        /// DIMSE-C services: A subset of the DIMSE services which supports operations on Composite SOP
        /// Instances related to composite Information Object Definitions with peer DIMSE-service-users.
        /// 
        /// DIMSE-N services: a subset of the DIMSE services which supports operations and notifications on
        /// Normalized SOP Instances related to Normalized Information Object Definitions with peer DIMSEservice-
        /// users.
        /// </remarks>
        internal DimseCommand _Convert(System.Int16 underlyingValue)
        {
            System.Type enumType = typeof(DimseCommand);
            DimseCommand enumValue = DimseCommand.UNDEFINED;
            if (System.Enum.IsDefined(enumType, underlyingValue))
            {
                Object retValue = System.Enum.ToObject(enumType, underlyingValue);
                enumValue = (DimseCommand)retValue;
            }
            return enumValue;
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = false;
            streamWriter.WriteLine("<Command Id=\"{0}\">", this.ToString(CommandField));
            foreach (Attribute attribute in this)
            {
                attribute.DicomUnicodeConverter = null;
                result = attribute.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</Command>");
            return result;
        }
    }

    /// <summary>
    /// Sequence Item
    /// </summary>
    /// <remarks>
    /// ITEM: A component of the Value of a Data Element that is of Value Representation Sequence of Items.
    /// An Item contains a Data Set.
    /// </remarks>
    public class SequenceItem : AttributeSet
    {
        private bool _definedLength = false;
        private System.UInt16 _introducerGroup = 0xFFFE;
        private System.UInt16 _introducerElement = 0xE000;
        private System.UInt32 _introducerLength = 0xFFFFFFFF;
        private System.UInt16 _delimiterGroup = 0xFFFE;
        private System.UInt16 _delimiterElement = 0xE00D;
        private System.UInt32 _delimiterLength = 0;

        /// <summary>
        /// Property - DefinedLength
        /// </summary>
        public bool DefinedLength
        {
            set
            {
                _definedLength = value;
            }
            get
            {
                return _definedLength;
            }
        }

        /// <summary>
        /// Property - IntroducerGroup
        /// </summary>
        public System.UInt16 IntroducerGroup
        {
            set
            {
                _introducerGroup = value;
            }
            get
            {
                return _introducerGroup;
            }
        }

        /// <summary>
        /// Property - IntroducerElement
        /// </summary>
        public System.UInt16 IntroducerElement
        {
            set
            {
                _introducerElement = value;
            }
            get
            {
                return _introducerElement;
            }
        }

        /// <summary>
        /// Property - IntroducerLength
        /// </summary>
        public System.UInt32 IntroducerLength
        {
            set
            {
                _introducerLength = value;
            }
            get
            {
                return _introducerLength;
            }
        }

        /// <summary>
        /// Property - DelimiterGroup
        /// </summary>
        public System.UInt16 DelimiterGroup
        {
            set
            {
                _delimiterGroup = value;
            }
            get
            {
                return _delimiterGroup;
            }
        }

        /// <summary>
        /// Property - DelimiterElement
        /// </summary>
        public System.UInt16 DelimiterElement
        {
            set
            {
                _delimiterElement = value;
            }
            get
            {
                return _delimiterElement;
            }
        }

        /// <summary>
        /// Property - DelimiterLength
        /// </summary>
        public System.UInt32 DelimiterLength
        {
            set
            {
                _delimiterLength = value;
            }
            get
            {
                return _delimiterLength;
            }
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            string group = this.IntroducerGroup.ToString("X").PadLeft(4, '0');
            string element = this.IntroducerElement.ToString("X").PadLeft(4, '0');
            string length = this.IntroducerLength.ToString();
            if (this.IntroducerLength == 0xFFFFFFFF)
            {
                length = "UNDEFINED";
            }
            streamWriter.WriteLine("<ItemIntroducer Group=\"{0}\" Element=\"{1}\" Length=\"{2}\"></ItemIntroducer>",
                group,
                element,
                length);

            bool result = false;
            foreach (Attribute attribute in this)
            {
                result = attribute.DvtDetailToXml(streamWriter, level);
            }

            if (this.DefinedLength == false)
            {
                group = this.DelimiterGroup.ToString("X").PadLeft(4, '0');
                element = this.DelimiterElement.ToString("X").PadLeft(4, '0');
                streamWriter.WriteLine("<ItemDelimiter Group=\"{0}\" Element=\"{1}\" Length=\"{2}\"></ItemDelimiter>",
                    group,
                    element,
                    this.DelimiterLength);
            }
            return result;
        }
    }

    /// <summary>
    /// Sequence is an collection of <see cref="SequenceItem"/>.
    /// The collection is used by <see cref="Attribute"/> with <see cref="DvtkData.Dimse.VR"/> equal to <see cref="DvtkData.Dimse.VR.SQ"/>.
    /// </summary>
    public sealed class Sequence : DvtkData.Collections.NullSafeCollectionBase, IDvtDetailToXml
    {
        private bool _definedLength = false;
        private System.UInt32 _length = 0xFFFFFFFF;
        private System.UInt16 _delimiterGroup = 0xFFFE;
        private System.UInt16 _delimiterElement = 0xE0DD;
        private System.UInt32 _delimiterLength = 0;

        /// <summary>
        /// Property - DefinedLength
        /// </summary>
        public bool DefinedLength
        {
            set
            {
                _definedLength = value;
            }
            get
            {
                return _definedLength;
            }
        }

        /// <summary>
        /// Property - Length
        /// </summary>
        public System.UInt32 Length
        {
            set
            {
                _length = value;
            }
            get
            {
                return _length;
            }
        }

        /// <summary>
        /// Property - DelimiterGroup
        /// </summary>
        public System.UInt16 DelimiterGroup
        {
            set
            {
                _delimiterGroup = value;
            }
            get
            {
                return _delimiterGroup;
            }
        }

        /// <summary>
        /// Property - DelimiterElement
        /// </summary>
        public System.UInt16 DelimiterElement
        {
            set
            {
                _delimiterElement = value;
            }
            get
            {
                return _delimiterElement;
            }
        }

        /// <summary>
        /// Property - DelimiterLength
        /// </summary>
        public System.UInt32 DelimiterLength
        {
            set
            {
                _delimiterLength = value;
            }
            get
            {
                return _delimiterLength;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Sequence() { }

        /// <summary>
        /// Gets or sets the <see cref="SequenceItem"/> at the specified index.
        /// </summary>
        /// <value>The <see cref="SequenceItem"/> at the specified <c>index</c>.</value>
        public new SequenceItem this[int index]
        {
            get
            {
                return (SequenceItem)base[index];
            }
            set
            {
                base.Insert(index, value);
            }
        }

        /// <summary>
        /// Inserts an <see cref="SequenceItem"/> to the <see cref="Sequence"/> at the specified position.
        /// </summary>
        /// <param name="index">The zero-based index at which <c>value</c> should be inserted. </param>
        /// <param name="value">The <see cref="SequenceItem"/> to insert into the <see cref="Sequence"/>.</param>
        public void Insert(int index, SequenceItem value)
        {
            base.Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="SequenceItem"/> from the <see cref="Sequence"/>.
        /// </summary>
        /// <param name="value">The <see cref="SequenceItem"/> to remove from the <see cref="Sequence"/>.</param>
        public void Remove(SequenceItem value)
        {
            base.Remove(value);
        }

        /// <summary>
        /// Determines whether the <see cref="Sequence"/> contains a specific <see cref="SequenceItem"/>.
        /// </summary>
        /// <param name="value">The <see cref="SequenceItem"/> to locate in the <see cref="Sequence"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="SequenceItem"/> is found in the <see cref="Sequence"/>; otherwise, <see langword="false"/>.</returns>
        public bool Contains(SequenceItem value)
        {
            return base.Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific <see cref="SequenceItem"/> in the <see cref="Sequence"/>.
        /// </summary>
        /// <param name="value">The <see cref="SequenceItem"/> to locate in the <see cref="Sequence"/>.</param>
        /// <returns>The index of <c>value</c> if found in the <see cref="Sequence"/>; otherwise, -1.</returns>
        public int IndexOf(SequenceItem value)
        {
            return base.IndexOf(value);
        }

        /// <summary>
        /// Adds an <see cref="SequenceItem"/> to the <see cref="Sequence"/>.
        /// </summary>
        /// <param name="value">The <see cref="SequenceItem"/> to add to the <see cref="Sequence"/>. </param>
        /// <returns>The position into which the new <see cref="SequenceItem"/> was inserted.</returns>
        public int Add(SequenceItem value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = false;

            string length = this.Length.ToString();
            if (this.Length == 0xFFFFFFFF)
            {
                length = "UNDEFINED";
            }
            streamWriter.WriteLine("<Sequence Length=\"{0}\">", length);

            int itemCount = 1;
            foreach (SequenceItem item in base.List)
            {
                streamWriter.WriteLine("<Item Number=\"{0}\">", itemCount++);
                result = item.DvtDetailToXml(streamWriter, level + 1);
                streamWriter.WriteLine("</Item>");
            }

            if (this.DefinedLength == false)
            {
                string group = this.DelimiterGroup.ToString("X").PadLeft(4, '0');
                string element = this.DelimiterElement.ToString("X").PadLeft(4, '0');
                streamWriter.WriteLine("<SequenceDelimiter Group=\"{0}\" Element=\"{1}\" Length=\"{2}\"></SequenceDelimiter>",
                    group,
                    element,
                    this.DelimiterLength);
            }

            streamWriter.WriteLine("</Sequence>");
            return result;
        }
    }

    /// <summary>
    /// Sequence of Items
    /// </summary>
    /// <remarks>
    /// Value is a Sequence of zero or more Items.
    /// </remarks>
    public class SequenceOfItems : DicomValueType
    {
        /// <summary>
        /// The list of <see cref="SequenceItem"/> maintained by the <see cref="SequenceOfItems"/> attribute.
        /// </summary>
        public Sequence Sequence
        {
            get
            {
                return _Sequence;
            }
            set
            {
                _Sequence = value;
            }
        }
        private Sequence _Sequence = new Sequence();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Sequence.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Message: A data unit of the Message Exchange Protocol exchanged between 
    /// two cooperating DICOM Applications.
    /// </summary>
    /// <remarks>
    /// A Message is composed of a Command Stream followed by an optional Data Stream.
    /// </remarks>
    public class DicomMessage : DvtkData.Message, IDvtDetailToXml
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DicomMessage() { }

        /*
        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="commandField">Command field</param>
        public DicomMessage(DimseCommand commandField)
        {
            // this._CommandSet.CommandField = commandField;
        }
        */

        /// <summary>
        /// Apply <see cref="CommandSet"/> and <see cref="DataSet"/> to this message.
        /// </summary>
        /// <param name="commandSet">command set</param>
        /// <param name="dataSet">data set</param>
        /// <param name="encodedPCID">Presentation context ID in which message is encoded</param>
        public void Apply(CommandSet commandSet, DataSet dataSet, Byte encodedPCID)
        {
            if (commandSet == null) throw new System.ArgumentNullException("commandSet");
            if (dataSet == null) throw new System.ArgumentNullException("dataSet");
            this.CommandSet = commandSet;
            this.DataSet = dataSet;
            this.EncodedPresentationContextID = encodedPCID;
        }

        /// <summary>
        /// Apply only a <see cref="CommandSet"/>. This results in NO dataSet.
        /// </summary>
        /// <param name="commandSet">command set</param>
        /// <param name="encodedPCID">Presentation context ID in which message is encoded</param>
        public void Apply(CommandSet commandSet, Byte encodedPCID)
        {
            if (commandSet == null) throw new System.ArgumentNullException("commandSet");
            this.CommandSet = commandSet;
            this.DataSet = null;
            this.EncodedPresentationContextID = encodedPCID;
        }

        /// <summary>
        /// Apply <see cref="CommandSet"/> and <see cref="DataSet"/> to this message.
        /// </summary>
        /// <param name="commandSet"></param>
        /// <param name="dataSet"></param>
        public void Apply(CommandSet commandSet, DataSet dataSet)
        {
            if (commandSet == null) throw new System.ArgumentNullException("commandSet");
            if (dataSet == null) throw new System.ArgumentNullException("dataSet");
            this.CommandSet = commandSet;
            this.DataSet = dataSet;
            this.EncodedPresentationContextID = unchecked((byte)-1);
        }

        /// <summary>
        /// Apply only a <see cref="CommandSet"/>. This results in NO dataSet.
        /// </summary>
        /// <param name="commandSet"></param>
        public void Apply(CommandSet commandSet)
        {
            if (commandSet == null) throw new System.ArgumentNullException("commandSet");
            this.CommandSet = commandSet;
            this.DataSet = null;
            this.EncodedPresentationContextID = unchecked((byte)-1);
        }

        /// <summary>
        /// The CommandField of the <see cref="CommandSet"/> linked to the <see cref="DicomMessage"/>.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Retrieve Only, Derived Property. Derive via CommandSet.
        /// </p>
        /// <p>
        /// TODO: Check new serialization for necessity of this property!!!
        /// Required by Xml serialization. 
        /// The <see cref="CommandSet"/> is a implements the ICollection interface.
        /// This has the side-effect that the Xml Serializer can not serialize any attributes which are present on the <see cref="CommandSet"/>.
        /// The <see cref="DicomMessage"/> will therefor be used to Xml serialize this information.
        /// </p>
        /// <p>
        /// The CommandField is only provided by means of a get method on <see cref="DicomMessage"/>.
        /// </p>
        /// </remarks>
        public DimseCommand CommandField
        {
            get
            {
                return this._CommandSet.CommandField;
            }
        }

        /// <summary>
        /// The presentation context ID represents by which <see cref="DicomMessage"/> is received from the network.
        /// </summary>
        public Byte EncodedPresentationContextID
        {
            get
            {
                return _encodedPCID;
            }
            set
            {
                _encodedPCID = value;
            }
        }
        private Byte _encodedPCID = 0;

        /// <summary>
        /// CommandSet. The <see cref="DicomMessage"/> consists of a mandatory CommandSet and an optional DataSet.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        public CommandSet CommandSet
        {
            get
            {
                return _CommandSet;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                else _CommandSet = value;
            }
        }
        private CommandSet _CommandSet = new CommandSet();

        /// <summary>
        /// DataSet. The <see cref="DicomMessage"/> consists of a mandatory CommandSet and an optional DataSet.
        /// </summary>
        /// <remarks>DataSet may be a <see langword="null"/> reference.</remarks>
        public DataSet DataSet
        {
            get
            {
                return _DataSet;
            }
            set
            {
                _DataSet = value;
            }
        }
        private DataSet _DataSet = new DataSet();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = false;
            streamWriter.WriteLine("<DicomMessage>");
            if (this.CommandSet != null)
            {
                result = this.CommandSet.DvtDetailToXml(streamWriter, level);
            }
            if (this.DataSet != null)
            {
                result = this.DataSet.DvtDetailToXml(streamWriter, level);
            }
            streamWriter.WriteLine("</DicomMessage>");
            return result;
        }
    }

    /// <summary>
    /// DicomValueType is the abstract base-class for all sub-classes that implement
    /// specific value types.
    /// </summary>
    /// <remarks>
    /// The value types are mapped to their corresponding value representations.
    /// 
    /// VALUE FIELD:
    /// The field within a Data Element that contains the Value(s) of that Data Element.
    /// </remarks>
    public abstract class DicomValueType : IDvtDetailToXml
    {
        /// <summary>
        /// DICOM to Unicode Converter
        /// </summary>
        public DicomUnicodeConverter DicomUnicodeConverter
        {
            set
            {
                _dicomUnicodeConverter = value;
            }
        }

        /// <summary>
        /// Class variable
        /// </summary>
        protected DicomUnicodeConverter _dicomUnicodeConverter = null;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public abstract bool DvtDetailToXml(StreamWriter streamWriter, int level);
    }

    /// <summary>
    /// VALUE REPRESENTATION (VR)
    /// </summary>
    /// <remarks>
    /// <p>
    /// Specifies the data type and format of the Value(s) contained in the Value Field of a Data Element.</p>
    /// <p>
    /// The Value Representation of a Data Element describes the data type and format of that Data Element's
    /// Value(s).
    /// </p>
    /// </remarks>
    public enum VR
    {
        /// <summary>
        /// Application Entity
        /// </summary>
        AE,
        /// <summary>
        /// Age String
        /// </summary>
        AS,
        /// <summary>
        /// Attribute Tag
        /// </summary>
        AT,
        /// <summary>
        /// Code String
        /// </summary>
        CS,
        /// <summary>
        /// Date
        /// </summary>
        DA,
        /// <summary>
        /// Decimal String
        /// </summary>
        DS,
        /// <summary>
        /// Date Time
        /// </summary>
        DT,
        /// <summary>
        /// Floating Point Single
        /// </summary>
        FL,
        /// <summary>
        /// Floating Point Double
        /// </summary>
        FD,
        /// <summary>
        /// Integer String
        /// </summary>
        IS,
        /// <summary>
        /// Long String
        /// </summary>
        LO,
        /// <summary>
        /// Long Text
        /// </summary>
        LT,
        /// <summary>
        /// Other Byte String
        /// </summary>
        OB,
        /// <summary>
        /// Other Float String
        /// </summary>
        OF,
        /// <summary>
        /// Other Word String
        /// </summary>
        OW,
        /// <summary>
        /// Other Double String
        /// </summary>
        OD,
        /// <summary>
        /// Other Long String
        /// </summary>
        OL,
        /// <summary>
        /// Other Very Long String
        /// </summary>
        OV,
        /// <summary>
        /// Person Name
        /// </summary>
        PN,
        /// <summary>
        /// Short String
        /// </summary>
        SH,
        /// <summary>
        /// Signed Long
        /// </summary>
        SL,
        /// <summary>
        /// Sequence of Items
        /// </summary>
        SQ,
        /// <summary>
        /// Signed Short
        /// </summary>
        SS,
        /// <summary>
        /// Short Text
        /// </summary>
        ST,
        /// <summary>
        /// Time
        /// </summary>
        SV,
        /// <summary>
        /// Signed Very Long String
        /// </summary>
        TM,
        /// <summary>
        /// Unique Identifier (UID)
        /// </summary>
        UI,
        /// <summary>
        /// Unsigned Long
        /// </summary>
        UL,
        /// <summary>
        /// Unknown
        /// </summary>
        UN,
        /// <summary>
        /// Unsigned Short
        /// </summary>
        US,
        /// <summary>
        /// Unlimited Text
        /// </summary>
        UT,
        /// <summary>
        /// Universal Resource Locator
        /// </summary>
        UR,
        /// <summary>
        /// Unlimited Characters
        /// </summary>
        UV,
        /// <summary>
        /// Unsigned Very Long String
        /// </summary>
        UC
    }

    /// <summary>
    /// Type of the attribute according to the DataElement definition by the Information Object Definition.
    /// </summary>
    public enum AttributeType
    {
        /// <summary>
        /// TYPE 1 REQUIRED DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// IODs and SOP Classes define Type 1 Data Elements that shall be included and are mandatory elements.
        /// The Value Field shall contain valid data as defined by the elements VR and VM as specified in PS 3.6.
        /// The Length of the Value Field shall not be zero. Absence of a valid Value in a Type 1 Data Element is a protocol violation.
        /// </remarks>
        Item1,
        /// <summary>
        /// TYPE 1 CONDITIONAL DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// <p>
        /// IODs and SOP Classes define Data Elements that shall be included under certain specified conditions.
        /// Type 1C elements have the same requirements as Type 1 elements under these conditions. It is a
        /// protocol violation if the specified conditions are met and the Data Element is not included.
        /// </p>
        /// <p>
        /// When the specified conditions are not met, Type 1C elements shall not be included in the Data Set.
        /// </p>
        /// </remarks>
        Item1C,
        /// <summary>
        /// TYPE 2 REQUIRED DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// <p>
        /// IODs and SOP Classes define Type 2 Data Elements that shall be included and are mandatory Data
        /// Elements. However, it is permissible that if a Value for a Type 2 element is unknown it can be encoded
        /// with zero Value Length and no Value. If the Value is known the Value Field shall contain that value as
        /// defined by the elements VR and VM as specified in PS 3.6. These Data Elements shall be included in the
        /// Data Set and their absence is a protocol violation.
        /// </p>
        /// <p>
        /// Note: The intent of Type 2 Data Elements is to allow a zero length to be conveyed when the operator or
        /// application does not know its value or has a specific reason for not specifying its value. It is the intent that
        /// the device should support these Data Elements.
        /// </p>
        /// </remarks>
        Item2,
        /// <summary>
        /// TYPE 2 CONDITIONAL DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// <p>
        /// IODs and SOP Classes define Type 2C elements that have the same requirements as Type 2 elements
        /// under certain specified conditions. It is a protocol violation if the specified conditions are met and the Data
        /// Element is not included.
        /// </p>
        /// <p>
        /// When the specified conditions are not met, Type 2C elements shall not be included in the Data Set.
        /// </p>
        /// <p>
        /// Note: An example of a Type 2C Data Element is Inversion Time (0018,0082). For several SOP Class
        /// Definitions, this Data Element is required only if the Scanning Sequence (0018,0020) has the Value “IR.”
        /// It is not required otherwise. See PS 3.3.
        /// </p>
        /// </remarks>
        Item2C,
        /// <summary>
        /// TYPE 3 OPTIONAL DATA ELEMENTS
        /// </summary>
        /// <remarks>
        /// IODs and SOP Classes define Type 3 Data Elements that are optional Data Elements. Absence of a
        /// Type 3 element from a Data Set does not convey any significance and is not a protocol violation. Type 3
        /// elements may also be encoded with zero length and no Value. The meaning of a zero length Type 3 Data
        /// Element shall be precisely the same as that element being absent from the Data Set.
        /// </remarks>
        Item3,
        /// <summary>
        /// TYPE 3 OPTIONAL DATA ELEMENTS
        /// </summary>
        Item3C,
        /// <summary>
        /// TYPE 3 OPTIONAL DATA ELEMENTS
        /// </summary>
        Item3R,
    }

    /// <summary>
    /// Element is the abstract base-class for CommandElement and DataElement
    /// </summary>
    /// <remarks>
    /// This class implements the generic logic that can be applied to 
    /// Elements such as vr, tag, vm access and value indexing.
    /// 
    /// Command Element: An encoding of a parameter of a command which conveys this parameter's value.
    /// 
    /// DATA ELEMENT: A unit of information as defined by a single entry in the data dictionary.
    /// An encoded Information Object Definition (IOD) Attribute that is composed of, at a minimum, 
    /// three fields: a Data Element Tag, a Value Length, and a Value Field. 
    /// For some specific Transfer Syntaxes, a Data Element also contains a VR Field 
    /// where the Value Representation of that Data Element is specified explicitly.
    /// </remarks>
    public class Attribute : IDvtDetailToXml
    {
        /// <summary>
        /// DICOM to Unicode Converter
        /// </summary>
        public DicomUnicodeConverter DicomUnicodeConverter
        {
            set
            {
                _dicomUnicodeConverter = value;
            }
        }
        private DicomUnicodeConverter _dicomUnicodeConverter = null;

        internal class Comparer : IComparer
        {
            int IComparer.Compare(Object object1, Object object2)
            {
                int returnValue = 0;

                Attribute attribute1 = object1 as Attribute;
                Attribute attribute2 = object2 as Attribute;

                if ((attribute1 != null) && (attribute2 != null))
                {
                    UInt32 attribute1Tag = (UInt32)((attribute1.Tag.GroupNumber) * 65536) + (UInt32)attribute1.Tag.ElementNumber;
                    UInt32 attribute2Tag = (UInt32)((attribute2.Tag.GroupNumber) * 65536) + (UInt32)attribute2.Tag.ElementNumber;

                    if (attribute1Tag < attribute2Tag)
                    {
                        returnValue = -1;
                    }
                    else if (attribute1Tag == attribute2Tag)
                    {
                        returnValue = 0;
                    }
                    else
                    {
                        returnValue = 1;
                    }
                }
                else
                {
                    returnValue = 0;
                }

                return (returnValue);
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Attribute() { }

        /// <summary>
        /// Creates an <see cref="Attribute"/>
        /// </summary>
        /// <param name="groupNumber">The group number.</param>
        /// <param name="elementNumber">The element number.</param>
        /// <param name="vr">The value representation.</param>
        /// <param name="list">The list of values.</param>
        /// <exception cref="System.InvalidCastException">
        /// The <c>list</c> is an <see cref="object"/> array. 
        /// The items in this <c>list</c> are converted to the underlying <c>vr</c> as much as possible. 
        /// However <see cref="System.InvalidCastException"/> may occur during these run-time conversions.
        /// </exception>
        // resolve call ambiguity by making this call internal
        internal Attribute(
            GROUP_NUMBER groupNumber,
            ELEMENT_NUMBER elementNumber,
            VR vr,
            params object[] list)
        {
            _InitializeAttribute(groupNumber, elementNumber, vr, list);
        }

        /// <summary>
        /// Creates an <see cref="Attribute"/>
        /// </summary>
        /// <param name="groupElement">The group element hex number.</param>
        /// <param name="vr">The value representation.</param>
        /// <param name="list">The list of values.</param>
        /// <exception cref="System.InvalidCastException">
        /// The <c>list</c> is an <see cref="object"/> array. 
        /// The items in this <c>list</c> are converted to the underlying <c>vr</c> as much as possible. 
        /// However <see cref="System.InvalidCastException"/> may occur during these run-time conversions.
        /// </exception>
        public Attribute(
            GROUP_ELEMENT groupElement,
            VR vr,
            params object[] list)
        {
            Tag tag = groupElement;
            _InitializeAttribute(tag.GroupNumber, tag.ElementNumber, vr, list);
        }

        #region Cast Helpers
        private string[] _CastToStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                try
                {
                    System.String value = System.Convert.ToString(item);
                    arrayList.Add(value);
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                }
            }
            return (System.String[])arrayList.ToArray(typeof(System.String));
        }

        private string[] _CastToDateStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                try
                {
                    System.String stringValue;
                    if (item is System.DateTime)
                        stringValue = ((System.DateTime)item).ToString("yyyyMMdd");
                    else
                    {
                        stringValue = System.Convert.ToString(item);
                    }
                    arrayList.Add(stringValue);
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                }
            }
            return (System.String[])arrayList.ToArray(typeof(System.String));
        }

        private string[] _CastToTimeStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                try
                {
                    System.String stringValue;
                    if (item is System.DateTime)
                        //stringValue = ((System.DateTime)item).ToString("HHmmss.ffffff");
                        stringValue = ((System.DateTime)item).ToString("HHmmss");
                    else
                    {
                        stringValue = System.Convert.ToString(item);
                    }
                    arrayList.Add(stringValue);
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                }
            }
            return (System.String[])arrayList.ToArray(typeof(System.String));
        }

        private string[] _CastToDateTimeStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                try
                {
                    System.String stringValue;
                    if (item is System.DateTime)
                        stringValue = ((System.DateTime)item).ToString("yyyyMMddHHmmss.ffffffzz00");
                    else
                    {
                        stringValue = System.Convert.ToString(item);
                    }
                    arrayList.Add(stringValue);
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                }
            }
            return (System.String[])arrayList.ToArray(typeof(System.String));
        }

        private Tag[] _CastToTagArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                try
                {
                    Tag tag;
                    if (item is Tag) tag = (Tag)item;
                    //                    else if (item is System.UInt32) tag = (System.UInt32)item;
                    else if (item is System.Int32) tag = (System.Int32)item;
                    else throw new System.InvalidCastException();
                    arrayList.Add(tag);
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                }
            }
            return (Tag[])arrayList.ToArray(typeof(Tag));
        }

        private System.Double[] _CastToFloatingPointDoubleArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        System.Double floatingPointDoubleValue =
                            System.Convert.ToDouble(item);
                        arrayList.Add(floatingPointDoubleValue);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (System.Double[])arrayList.ToArray(typeof(System.Double));
        }

        private System.Single[] _CastToFloatingPointSingleArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        System.Single floatingPointSingleValue =
                            System.Convert.ToSingle(item);
                        arrayList.Add(floatingPointSingleValue);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (System.Single[])arrayList.ToArray(typeof(System.Single));
        }

        private System.Byte[] _CastToByteArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        if (item is System.Byte) arrayList.Add(item);
                        else throw new System.InvalidCastException();
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (System.Byte[])arrayList.ToArray(typeof(System.Byte));
        }

        private SIGNED_SHORT[] _CastToSignedShortArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        SIGNED_SHORT sh = System.Convert.ToInt16(item);
                        arrayList.Add(sh);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (SIGNED_SHORT[])arrayList.ToArray(typeof(SIGNED_SHORT));
        }

        private UNSIGNED_SHORT[] _CastToUnsignedShortArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        UNSIGNED_SHORT ush = System.Convert.ToUInt16(item);
                        arrayList.Add(ush);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (UNSIGNED_SHORT[])arrayList.ToArray(typeof(UNSIGNED_SHORT));
        }

        private SIGNED_LONG[] _CastToSignedLongArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        SIGNED_LONG sl = System.Convert.ToInt32(item);
                        arrayList.Add(sl);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (SIGNED_LONG[])arrayList.ToArray(typeof(SIGNED_LONG));
        }

        private SIGNED_VERY_LONG[] _CastToSignedVeryLongStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        SIGNED_VERY_LONG sl = System.Convert.ToInt64(item);
                        arrayList.Add(sl);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (SIGNED_VERY_LONG[])arrayList.ToArray(typeof(SIGNED_VERY_LONG));
        }

        private System.String[] _CastUniqueIdentifierStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item is DvtkData.Dul.AbstractSyntax)
                    arrayList.Add((item as DvtkData.Dul.AbstractSyntax).UID);
                else if (item is DvtkData.Dul.TransferSyntax)
                    arrayList.Add((item as DvtkData.Dul.TransferSyntax).UID);
                else
                {
                    try
                    {
                        System.String value = System.Convert.ToString(item);
                        arrayList.Add(value);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (System.String[])arrayList.ToArray(typeof(System.String));
        }

        private System.String[] _CastUnlimitedCharactersStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item is DvtkData.Dul.AbstractSyntax)
                    arrayList.Add((item as DvtkData.Dul.AbstractSyntax).UID);
                else if (item is DvtkData.Dul.TransferSyntax)
                    arrayList.Add((item as DvtkData.Dul.TransferSyntax).UID);
                else
                {
                    try
                    {
                        System.String value = System.Convert.ToString(item);
                        arrayList.Add(value);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (System.String[])arrayList.ToArray(typeof(System.String));
        }

        private UNSIGNED_LONG[] _CastToUnsignedLongArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        UNSIGNED_LONG usl = System.Convert.ToUInt32(item);
                        arrayList.Add(usl);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (UNSIGNED_LONG[])arrayList.ToArray(typeof(UNSIGNED_LONG));
        }

        private UNSIGNED_VERY_LONG[] _CastToUnsignedVeryLongStringArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                if (item != null)
                {
                    try
                    {
                        UNSIGNED_VERY_LONG usl = System.Convert.ToUInt64(item);
                        arrayList.Add(usl);
                    }
                    catch (InvalidCastException e)
                    {
                        Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                    }
                }
            }
            return (UNSIGNED_VERY_LONG[])arrayList.ToArray(typeof(UNSIGNED_VERY_LONG));
        }

        private SequenceItem[] _CastToSequenceItemArray(object[] inArray)
        {
            System.Collections.ArrayList arrayList = new ArrayList();
            foreach (object item in inArray)
            {
                try
                {
                    if (item is SequenceItem) arrayList.Add(item);
                    else throw new System.InvalidCastException();
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine("Error casting attribute parameter to VR value. " + e);
                }
            }
            return (SequenceItem[])arrayList.ToArray(typeof(SequenceItem));
        }
        #endregion

        private void _InitializeAttribute(
            GROUP_NUMBER groupNumber,
            ELEMENT_NUMBER elementNumber,
            VR vr,
            params object[] list)
        {
            this.Tag.GroupNumber = groupNumber;
            this.Tag.ElementNumber = elementNumber;
            DicomValueType dicomValue = null;
            if (list != null)
            {
                switch (vr)
                {
                    case VR.AE:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            ApplicationEntity applicationEntity = new ApplicationEntity();
                            applicationEntity.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = applicationEntity;

                            // compute attribute value length
                            Length = 0;
                            if (applicationEntity.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in applicationEntity.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)applicationEntity.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.AS:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            AgeString ageString = new AgeString();
                            ageString.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = ageString;

                            // compute attribute value length
                            Length = (System.UInt32)ageString.Values.Count * 4;
                            break;
                        }
                    case VR.AT:
                        {
                            Tag[] tagArray = _CastToTagArray(list);
                            AttributeTag attributeTag = new AttributeTag();
                            attributeTag.Values =
                                new DvtkData.Collections.TagCollection(tagArray);
                            dicomValue = attributeTag;

                            // compute attribute value length
                            Length = (System.UInt32)attributeTag.Values.Count * 4;
                            break;
                        }
                    case VR.CS:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            CodeString codeString = new CodeString();
                            codeString.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = codeString;

                            // compute attribute value length
                            Length = 0;
                            if (codeString.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in codeString.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)codeString.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.DA:
                        {
                            System.String[] stringArray = _CastToDateStringArray(list);
                            Date date = new Date();
                            date.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = date;

                            // compute attribute value length
                            Length = 0;
                            if (date.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in date.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length;
                            }
                            break;
                        }
                    case VR.DS:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            DecimalString decimalString = new DecimalString();
                            decimalString.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = decimalString;

                            // compute attribute value length
                            Length = 0;
                            if (decimalString.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in decimalString.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)decimalString.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.DT:
                        {
                            System.String[] stringArray = _CastToDateTimeStringArray(list);
                            DateTime dateTime = new DateTime();
                            dateTime.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = dateTime;

                            // compute attribute value length
                            Length = 0;
                            if (dateTime.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in dateTime.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)dateTime.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.FD:
                        {
                            System.Double[] floatingPointDoubleArray = _CastToFloatingPointDoubleArray(list);
                            FloatingPointDouble floatingPointDouble = new FloatingPointDouble();
                            floatingPointDouble.Values =
                                new DvtkData.Collections.DoubleCollection(floatingPointDoubleArray);
                            dicomValue = floatingPointDouble;

                            // compute attribute value length
                            Length = (System.UInt32)floatingPointDouble.Values.Count * 8;
                            break;
                        }
                    case VR.FL:
                        {
                            System.Single[] floatingPointSingleArray = _CastToFloatingPointSingleArray(list);
                            FloatingPointSingle floatingPointSingle = new FloatingPointSingle();
                            floatingPointSingle.Values =
                                new DvtkData.Collections.SingleCollection(floatingPointSingleArray);
                            dicomValue = floatingPointSingle;

                            // compute attribute value length
                            Length = (System.UInt32)floatingPointSingle.Values.Count * 4;
                            break;
                        }
                    case VR.IS:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            IntegerString integerString = new IntegerString();
                            integerString.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = integerString;

                            // compute attribute value length
                            Length = 0;
                            if (integerString.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in integerString.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)integerString.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.LO:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            LongString longString = new LongString();
                            longString.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = longString;

                            // compute attribute value length
                            Length = 0;
                            if (longString.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in longString.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)longString.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.LT:
                        {
                            if (list.Length > 1) throw new System.ArgumentException();
                            LongText longText = new LongText();
                            Length = 0;
                            if (list.Length == 1)
                            {
                                System.String stringValue = System.Convert.ToString(list[0]);
                                longText.Value = stringValue;
                                Length = (System.UInt32)stringValue.Length;
                            }
                            dicomValue = longText;
                            break;
                        }
                    case VR.OB:
                        {
                            OtherByteString otherByteString = new OtherByteString();
                            if (list.Length > 0)
                            {
                                if (list.Length == 1 && list[0] is System.String)
                                {
                                    otherByteString.FileName = (System.String)list[0];
                                    Length = (System.UInt32)otherByteString.FileName.Length;
                                }
                                else if (list.Length == 1 && list[0] is BitmapPatternParameters)
                                {
                                    otherByteString.BitmapPattern = (BitmapPatternParameters)list[0];
                                }
                                else if (list.Length > 0 && list.Length <= 7)
                                {
                                    BitmapPatternParameters pattern = new BitmapPatternParameters();
                                    try
                                    {
                                        int listLength = list.Length;
                                        if (listLength > 0)
                                        {
                                            pattern.NumberOfRows = System.Convert.ToUInt16(list[0]);
                                            if (listLength == 1) pattern.NumberOfColumns = pattern.NumberOfRows;
                                        }
                                        if (listLength > 1) pattern.NumberOfColumns = System.Convert.ToUInt16(list[1]);
                                        if (listLength > 2) pattern.StartValue = System.Convert.ToUInt16(list[2]);
                                        if (listLength > 3) pattern.ValueIncrementPerRowBlock = System.Convert.ToUInt16(list[3]);
                                        if (listLength > 4) pattern.ValueIncrementPerColumnBlock = System.Convert.ToUInt16(list[4]);
                                        if (listLength > 5) pattern.NumberOfIdenticalValueRows = System.Convert.ToUInt16(list[5]);
                                        if (listLength > 6) pattern.NumberOfIdenticalValueColumns = System.Convert.ToUInt16(list[6]);
                                    }
                                    catch
                                    {
                                        throw new System.ArgumentException();
                                    }
                                    otherByteString.BitmapPattern = pattern;
                                }
                                else throw new System.ArgumentException();
                            }
                            dicomValue = otherByteString;
                            break;
                        }
                    case VR.OF:
                        {
                            OtherFloatString otherFloatString = new OtherFloatString();
                            if (list.Length > 0)
                            {
                                if (list.Length == 1 && list[0] is System.String)
                                {
                                    otherFloatString.FileName = (System.String)list[0];
                                    Length = (System.UInt32)otherFloatString.FileName.Length;
                                }
                                else if (list.Length == 1 && list[0] is BitmapPatternParameters)
                                {
                                    otherFloatString.BitmapPattern = (BitmapPatternParameters)list[0];
                                }
                                else if (list.Length > 0 && list.Length <= 7)
                                {
                                    BitmapPatternParameters pattern = new BitmapPatternParameters();
                                    try
                                    {
                                        int listLength = list.Length;
                                        if (listLength > 0)
                                        {
                                            pattern.NumberOfRows = System.Convert.ToUInt16(list[0]);
                                            if (listLength == 1) pattern.NumberOfColumns = pattern.NumberOfRows;
                                        }
                                        if (listLength > 1) pattern.NumberOfColumns = System.Convert.ToUInt16(list[1]);
                                        if (listLength > 2) pattern.StartValue = System.Convert.ToUInt16(list[2]);
                                        if (listLength > 3) pattern.ValueIncrementPerRowBlock = System.Convert.ToUInt16(list[3]);
                                        if (listLength > 4) pattern.ValueIncrementPerColumnBlock = System.Convert.ToUInt16(list[4]);
                                        if (listLength > 5) pattern.NumberOfIdenticalValueRows = System.Convert.ToUInt16(list[5]);
                                        if (listLength > 6) pattern.NumberOfIdenticalValueColumns = System.Convert.ToUInt16(list[6]);
                                    }
                                    catch
                                    {
                                        throw new System.ArgumentException();
                                    }
                                    otherFloatString.BitmapPattern = pattern;
                                }
                                else throw new System.ArgumentException();
                            }
                            dicomValue = otherFloatString;
                            break;
                        }

                    case VR.OD:
                        {
                            OtherDoubleString otherDoubleString = new OtherDoubleString();
                            if (list.Length > 0)
                            {
                                if (list.Length == 1 && list[0] is System.String)
                                {
                                    otherDoubleString.FileName = (System.String)list[0];
                                    Length = (System.UInt32)otherDoubleString.FileName.Length;
                                }
                                else if (list.Length == 1 && list[0] is BitmapPatternParameters)
                                {
                                    otherDoubleString.BitmapPattern = (BitmapPatternParameters)list[0];
                                }
                                else if (list.Length > 0 && list.Length <= 7)
                                {
                                    BitmapPatternParameters pattern = new BitmapPatternParameters();
                                    try
                                    {
                                        int listLength = list.Length;
                                        if (listLength > 0)
                                        {
                                            pattern.NumberOfRows = System.Convert.ToUInt16(list[0]);
                                            if (listLength == 1) pattern.NumberOfColumns = pattern.NumberOfRows;
                                        }
                                        if (listLength > 1) pattern.NumberOfColumns = System.Convert.ToUInt16(list[1]);
                                        if (listLength > 2) pattern.StartValue = System.Convert.ToUInt16(list[2]);
                                        if (listLength > 3) pattern.ValueIncrementPerRowBlock = System.Convert.ToUInt16(list[3]);
                                        if (listLength > 4) pattern.ValueIncrementPerColumnBlock = System.Convert.ToUInt16(list[4]);
                                        if (listLength > 5) pattern.NumberOfIdenticalValueRows = System.Convert.ToUInt16(list[5]);
                                        if (listLength > 6) pattern.NumberOfIdenticalValueColumns = System.Convert.ToUInt16(list[6]);
                                    }
                                    catch
                                    {
                                        throw new System.ArgumentException();
                                    }
                                    otherDoubleString.BitmapPattern = pattern;
                                }
                                else throw new System.ArgumentException();
                            }
                            dicomValue = otherDoubleString;
                            break;
                        }

                    case VR.OV:
                        {
                            OtherVeryLongString otherVeryLongString = new OtherVeryLongString();
                            if (list.Length > 0)
                            {
                                if (list.Length == 1 && list[0] is System.String)
                                {
                                    otherVeryLongString.FileName = (System.String)list[0];
                                    Length = (System.UInt32)otherVeryLongString.FileName.Length;
                                }
                                else if (list.Length == 1 && list[0] is BitmapPatternParameters)
                                {
                                    otherVeryLongString.BitmapPattern = (BitmapPatternParameters)list[0];
                                }
                                else if (list.Length > 0 && list.Length <= 7)
                                {
                                    BitmapPatternParameters pattern = new BitmapPatternParameters();
                                    try
                                    {
                                        int listLength = list.Length;
                                        if (listLength > 0)
                                        {
                                            pattern.NumberOfRows = System.Convert.ToUInt16(list[0]);
                                            if (listLength == 1) pattern.NumberOfColumns = pattern.NumberOfRows;
                                        }
                                        if (listLength > 1) pattern.NumberOfColumns = System.Convert.ToUInt16(list[1]);
                                        if (listLength > 2) pattern.StartValue = System.Convert.ToUInt16(list[2]);
                                        if (listLength > 3) pattern.ValueIncrementPerRowBlock = System.Convert.ToUInt16(list[3]);
                                        if (listLength > 4) pattern.ValueIncrementPerColumnBlock = System.Convert.ToUInt16(list[4]);
                                        if (listLength > 5) pattern.NumberOfIdenticalValueRows = System.Convert.ToUInt16(list[5]);
                                        if (listLength > 6) pattern.NumberOfIdenticalValueColumns = System.Convert.ToUInt16(list[6]);
                                    }
                                    catch
                                    {
                                        throw new System.ArgumentException();
                                    }
                                    otherVeryLongString.BitmapPattern = pattern;
                                }
                                else throw new System.ArgumentException();
                            }
                            dicomValue = otherVeryLongString;
                            break;
                        }

                    case VR.OL:
                        {
                            OtherLongString otherLongString = new OtherLongString();
                            if (list.Length > 0)
                            {
                                if (list.Length == 1 && list[0] is System.String)
                                {
                                    otherLongString.FileName = (System.String)list[0];
                                    Length = (System.UInt32)otherLongString.FileName.Length;
                                }
                                else if (list.Length == 1 && list[0] is BitmapPatternParameters)
                                {
                                    otherLongString.BitmapPattern = (BitmapPatternParameters)list[0];
                                }
                                else if (list.Length > 0 && list.Length <= 7)
                                {
                                    BitmapPatternParameters pattern = new BitmapPatternParameters();
                                    try
                                    {
                                        int listLength = list.Length;
                                        if (listLength > 0)
                                        {
                                            pattern.NumberOfRows = System.Convert.ToUInt16(list[0]);
                                            if (listLength == 1) pattern.NumberOfColumns = pattern.NumberOfRows;
                                        }
                                        if (listLength > 1) pattern.NumberOfColumns = System.Convert.ToUInt16(list[1]);
                                        if (listLength > 2) pattern.StartValue = System.Convert.ToUInt16(list[2]);
                                        if (listLength > 3) pattern.ValueIncrementPerRowBlock = System.Convert.ToUInt16(list[3]);
                                        if (listLength > 4) pattern.ValueIncrementPerColumnBlock = System.Convert.ToUInt16(list[4]);
                                        if (listLength > 5) pattern.NumberOfIdenticalValueRows = System.Convert.ToUInt16(list[5]);
                                        if (listLength > 6) pattern.NumberOfIdenticalValueColumns = System.Convert.ToUInt16(list[6]);
                                    }
                                    catch
                                    {
                                        throw new System.ArgumentException();
                                    }
                                    otherLongString.BitmapPattern = pattern;
                                }
                                else throw new System.ArgumentException();
                            }
                            dicomValue = otherLongString;
                            break;
                        }

                    case VR.OW:
                        {
                            OtherWordString otherWordString = new OtherWordString();
                            if (list.Length > 0)
                            {
                                if (list.Length == 1 && list[0] is System.String)
                                {
                                    otherWordString.FileName = (System.String)list[0];
                                    Length = (System.UInt32)otherWordString.FileName.Length;
                                }
                                else if (list.Length == 1 && list[0] is BitmapPatternParameters)
                                {
                                    otherWordString.BitmapPattern = (BitmapPatternParameters)list[0];
                                }
                                else if (list.Length > 0 && list.Length <= 7)
                                {
                                    BitmapPatternParameters pattern = new BitmapPatternParameters();
                                    try
                                    {
                                        int listLength = list.Length;
                                        if (listLength > 0)
                                        {
                                            pattern.NumberOfRows = System.Convert.ToUInt16(list[0]);
                                            if (listLength == 1) pattern.NumberOfColumns = pattern.NumberOfRows;
                                        }
                                        if (listLength > 1) pattern.NumberOfColumns = System.Convert.ToUInt16(list[1]);
                                        if (listLength > 2) pattern.StartValue = System.Convert.ToUInt16(list[2]);
                                        if (listLength > 3) pattern.ValueIncrementPerRowBlock = System.Convert.ToUInt16(list[3]);
                                        if (listLength > 4) pattern.ValueIncrementPerColumnBlock = System.Convert.ToUInt16(list[4]);
                                        if (listLength > 5) pattern.NumberOfIdenticalValueRows = System.Convert.ToUInt16(list[5]);
                                        if (listLength > 6) pattern.NumberOfIdenticalValueColumns = System.Convert.ToUInt16(list[6]);
                                    }
                                    catch
                                    {
                                        throw new System.ArgumentException();
                                    }
                                    otherWordString.BitmapPattern = pattern;
                                }
                                else throw new System.ArgumentException();
                            }
                            dicomValue = otherWordString;
                            break;
                        }
                    case VR.PN:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            PersonName personName = new PersonName();
                            personName.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = personName;

                            // compute attribute value length
                            Length = 0;
                            if (personName.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in personName.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)personName.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.SH:
                        {
                            System.String[] stringArray = _CastToStringArray(list);
                            ShortString shortString = new ShortString();
                            shortString.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = shortString;

                            // compute attribute value length
                            Length = 0;
                            if (shortString.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in shortString.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)shortString.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.SL:
                        {
                            System.Int32[] signedLongArray = _CastToSignedLongArray(list);
                            SignedLong signedLong = new SignedLong();
                            signedLong.Values =
                                new DvtkData.Collections.Int32Collection(signedLongArray);
                            dicomValue = signedLong;

                            // compute attribute value length
                            Length = (System.UInt32)signedLong.Values.Count * 4;
                            break;
                        }
                    case VR.SQ:
                        {
                            SequenceItem[] sequenceItemArray = _CastToSequenceItemArray(list);
                            SequenceOfItems sequenceOfItems = new SequenceOfItems();
                            sequenceOfItems.Sequence = new Sequence();
                            foreach (SequenceItem item in sequenceItemArray)
                            {
                                sequenceOfItems.Sequence.Add(item);
                            }
                            dicomValue = sequenceOfItems;

                            // set length to Undefined
                            Length = 0xFFFFFFFF;
                            break;
                        }
                    case VR.SS:
                        {
                            System.Int16[] signedShortArray = _CastToSignedShortArray(list);
                            SignedShort signedShort = new SignedShort();
                            signedShort.Values =
                                new DvtkData.Collections.Int16Collection(signedShortArray);
                            dicomValue = signedShort;

                            // compute attribute value length
                            Length = (System.UInt32)signedShort.Values.Count * 2;
                            break;
                        }
                    case VR.ST:
                        {
                            if (list.Length > 1) throw new System.ArgumentException();
                            ShortText shortText = new ShortText();
                            Length = 0;
                            if (list.Length == 1)
                            {
                                System.String stringValue = System.Convert.ToString(list[0]);
                                shortText.Value = stringValue;
                                Length = (System.UInt32)shortText.Value.Length;
                            }
                            dicomValue = shortText;
                            break;
                        }
                    case VR.SV:
                        {
                            System.Int64[] signedVeryLongStringArray = _CastToSignedVeryLongStringArray(list);
                            SignedVeryLongString signedVeryLongString = new SignedVeryLongString();
                            signedVeryLongString.Values =
                                new DvtkData.Collections.Int64Collection(signedVeryLongStringArray);
                            dicomValue = signedVeryLongString;

                            // compute attribute value length
                            Length = (System.UInt32)signedVeryLongString.Values.Count * 4;
                            break;
                        }
                    case VR.TM:
                        {
                            System.String[] stringArray = _CastToTimeStringArray(list);
                            Time time = new Time();
                            time.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = time;

                            // compute attribute value length
                            Length = 0;
                            if (time.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in time.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)time.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.UI:
                        {
                            System.String[] stringArray = _CastUniqueIdentifierStringArray(list);
                            UniqueIdentifier uniqueIdentifier = new UniqueIdentifier();
                            uniqueIdentifier.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = uniqueIdentifier;

                            // compute attribute value length
                            Length = 0;
                            if (uniqueIdentifier.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in uniqueIdentifier.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)uniqueIdentifier.Values.Count - 1;
                            }
                            break;
                        }
                    case VR.UL:
                        {
                            System.UInt32[] unsignedLongArray = _CastToUnsignedLongArray(list);
                            UnsignedLong unsignedLong = new UnsignedLong();
                            unsignedLong.Values =
                                new DvtkData.Collections.UInt32Collection(unsignedLongArray);
                            dicomValue = unsignedLong;

                            // compute attribute value length
                            Length = (System.UInt32)unsignedLong.Values.Count * 4;
                            break;
                        }
                    case VR.UN:
                        {
                            Unknown unknown = new Unknown();
                            Length = 0;
                            if (list.Length == 0 ||
                                list[0] is System.Byte)
                            {
                                System.Byte[] byteArray = _CastToByteArray(list);
                                unknown.ByteArray = byteArray;
                                Length = (System.UInt32)unknown.ByteArray.Length;
                            }
                            else throw new System.ArgumentException();
                            dicomValue = unknown;
                            break;
                        }
                    case VR.US:
                        {
                            System.UInt16[] unsignedShortArray = _CastToUnsignedShortArray(list);
                            UnsignedShort unsignedShort = new UnsignedShort();
                            unsignedShort.Values =
                                new DvtkData.Collections.UInt16Collection(unsignedShortArray);
                            dicomValue = unsignedShort;

                            // compute attribute value length
                            Length = (System.UInt32)unsignedShort.Values.Count * 2;
                            break;
                        }
                    case VR.UT:
                        {
                            if (list.Length > 1) throw new System.ArgumentException();
                            UnlimitedText unlimitedText = new UnlimitedText();
                            Length = 0;
                            if (list.Length == 1)
                            {
                                System.String stringValue = System.Convert.ToString(list[0]);
                                unlimitedText.Value = stringValue;
                                Length = (System.UInt32)unlimitedText.Value.Length;
                            }
                            dicomValue = unlimitedText;
                            break;
                        }
                    case VR.UV:
                        {
                            System.UInt64[] unsignedVeryLongStringArray = _CastToUnsignedVeryLongStringArray(list);
                            UnsignedVeryLongString unsignedVeryLongString = new UnsignedVeryLongString();
                            unsignedVeryLongString.Values =
                                new DvtkData.Collections.UInt64Collection(unsignedVeryLongStringArray);
                            dicomValue = unsignedVeryLongString;

                            // compute attribute value length
                            Length = (System.UInt32)unsignedVeryLongString.Values.Count * 4;
                            break;
                        }
                    case VR.UR:
                        {
                            if (list.Length > 1) throw new System.ArgumentException();
                            UniversalResourceIdentifier universalResourceIdentifier = new UniversalResourceIdentifier();
                            Length = 0;
                            if (list.Length == 1)
                            {
                                System.String stringValue = System.Convert.ToString(list[0]);
                                universalResourceIdentifier.Value = stringValue;
                                Length = (System.UInt32)universalResourceIdentifier.Value.Length;
                            }
                            dicomValue = universalResourceIdentifier;
                            break;
                        }
                    case VR.UC:
                        {
                            System.String[] stringArray = _CastUnlimitedCharactersStringArray(list);
                            UnlimitedCharacters unlimitedCharacters = new UnlimitedCharacters();
                            unlimitedCharacters.Values =
                                new DvtkData.Collections.StringCollection(stringArray);
                            dicomValue = unlimitedCharacters;

                            // compute attribute value length
                            Length = 0;
                            if (unlimitedCharacters.Values.Count > 0)
                            {
                                System.UInt32 length = 0;
                                foreach (String data in unlimitedCharacters.Values)
                                {
                                    length += (System.UInt32)data.Length;
                                }
                                Length = length + (System.UInt32)unlimitedCharacters.Values.Count - 1;
                            }
                            break;
                        }
                    default:
                        dicomValue = null;
                        break;
                }
            }
            else
            {
                dicomValue = null;
            }
            this._ValueField = dicomValue;
        }


        /// <summary>
        /// Attribute Length - only used for serialization.
        /// </summary>
		public System.UInt32 Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
            }
        }
        private System.UInt32 _length = 0;

        /// <summary>
        /// Dicom Tag.
        /// </summary>
        /// <remarks>Contains group and element number.</remarks>
        public Tag Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                _Tag = value;
            }
        }
        private Tag _Tag = new Tag();

        /// <summary>
        /// Dicom Value.
        /// </summary>
        /// <remarks>
        /// <p>
        /// For single-value attributes with value multiplicity == 1 this holds one value.
        /// </p>
        /// For multi-value attributes with value multiplicity != 1 this holds an array of values.
        /// <p>
        /// </p>
        /// </remarks>
        public DicomValueType DicomValue
        {
            get
            {
                return _ValueField;
            }
            set
            {
                _ValueField = value;
            }
        }
        private DicomValueType _ValueField = new Unknown();

        /// <summary>
        /// This is a literal string identifying the attribute by name.
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Name = string.Empty;

        /// <summary>
        /// Value Representation of the DicomValue
        /// </summary>
        public VR ValueRepresentation
        {
            get
            {
                if (_ValueField is ApplicationEntity) return VR.AE;
                if (_ValueField is AgeString) return VR.AS;
                if (_ValueField is AttributeTag) return VR.AT;
                if (_ValueField is CodeString) return VR.CS;
                if (_ValueField is Date) return VR.DA;
                if (_ValueField is DecimalString) return VR.DS;
                if (_ValueField is DateTime) return VR.DT;
                if (_ValueField is FloatingPointSingle) return VR.FL;
                if (_ValueField is FloatingPointDouble) return VR.FD;
                if (_ValueField is IntegerString) return VR.IS;
                if (_ValueField is LongString) return VR.LO;
                if (_ValueField is LongText) return VR.LT;
                if (_ValueField is OtherByteString) return VR.OB;
                if (_ValueField is OtherFloatString) return VR.OF;
                if (_ValueField is OtherWordString) return VR.OW;
                if (_ValueField is OtherLongString) return VR.OL;
                if (_ValueField is OtherDoubleString) return VR.OD;
                if (_ValueField is OtherVeryLongString) return VR.OV;
                if (_ValueField is PersonName) return VR.PN;
                if (_ValueField is ShortString) return VR.SH;
                if (_ValueField is SignedLong) return VR.SL;
                if (_ValueField is SequenceOfItems) return VR.SQ;
                if (_ValueField is SignedShort) return VR.SS;
                if (_ValueField is ShortText) return VR.ST;
                if (_ValueField is SignedVeryLongString) return VR.SV;
                if (_ValueField is Time) return VR.TM;
                if (_ValueField is UniqueIdentifier) return VR.UI;
                if (_ValueField is UnsignedLong) return VR.UL;
                if (_ValueField is Unknown) return VR.UN;
                if (_ValueField is UnsignedShort) return VR.US;
                if (_ValueField is UnsignedVeryLongString) return VR.UV;
                if (_ValueField is UnlimitedText) return VR.UT;
                if (_ValueField is UniversalResourceIdentifier) return VR.UR;
                if (_ValueField is UnlimitedCharacters) return VR.UC;
                // Unknown DicomValueType
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// This property decides about VR display in attribute dump
        /// </summary>
        public bool DisplayVR
        {
            get
            {
                return _Display;
            }
            set
            {
                _Display = value;
            }
        }
        private bool _Display = true;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            string group = Tag.GroupNumber.ToString("X").PadLeft(4, '0');
            string element = Tag.ElementNumber.ToString("X").PadLeft(4, '0');

            if (DisplayVR)
            {
                streamWriter.WriteLine("<Attribute Group=\"{0}\" Element=\"{1}\" VR=\"{2}\" Length=\"{3}\" Name=\"{4}\">", group, element, ValueRepresentation.ToString(), Length, DvtToXml.ConvertString(Name, false));
            }
            else
            {
                streamWriter.WriteLine("<Attribute Group=\"{0}\" Element=\"{1}\" VR=\"\" Length=\"{2}\" Name=\"{3}\">", group, element, Length, DvtToXml.ConvertString(Name, false));
            }
            streamWriter.WriteLine("<Values>");
            DicomValue.DicomUnicodeConverter = _dicomUnicodeConverter;
            DicomValue.DvtDetailToXml(streamWriter, level);
            streamWriter.WriteLine("</Values>");
            streamWriter.WriteLine("</Attribute>");
            return true;
        }
    }

    /// <summary>
    /// Structure that represents a Dicom Tag.
    /// </summary>
    /// <remarks>
    /// DATA ELEMENT TAG: A unique identifier for a Data Element composed of an ordered pair of numbers 
    /// (a Group Number followed by an Element Number).
    /// </remarks>
    public class Tag
    {
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return (
                this._Element.GetHashCode() ^
                this._Group.GetHashCode());
        }
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare with this instance, or a <see langword="null"/> reference.</param>
        /// <returns><see langword="true"/> if other is an instance of <see cref="Tag"/> and equals the value of this instance; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(System.Object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;
            Tag tag = (Tag)obj;
            return (
                this._Element == tag._Element &&
                this._Group == tag._Group);
        }

        /// <summary>
        /// Determines whether two specified Tags are equal.
        /// </summary>
        /// <param name="tag1">A <see cref="Tag"/></param>
        /// <param name="tag2">A <see cref="Tag"/></param>
        /// <returns>
        /// <see langword="true"/> if <c>tag1</c> and <c>tag2</c> 
        /// represent the same <see cref="Tag"/>; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Tag tag1, Tag tag2)
        {
            return (tag1.Equals(tag2));
        }

        /// <summary>
        /// Determines whether two specified Tags are not equal.
        /// </summary>
        /// <param name="tag1">A <see cref="Tag"/></param>
        /// <param name="tag2">A <see cref="Tag"/></param>
        /// <returns>
        /// <see langword="true"/> if <c>tag1</c> and <c>tag2</c> 
        /// do not represent the same <see cref="Tag"/>; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Tag tag1, Tag tag2)
        {
            return (!tag1.Equals(tag2));
        }

        #region DataDictonary
        /// <summary/>
        public static readonly Tag GROUP_0000_LENGTH = new Tag(0x0000, 0x0000);
        /// <summary/>
        public static readonly Tag AFFECTED_SOP_CLASS_UID = new Tag(0x0000, 0x0002);
        /// <summary/>
        public static readonly Tag REQUESTED_SOP_CLASS_UID = new Tag(0x0000, 0x0003);
        /// <summary/>
        public static readonly Tag COMMAND_FIELD = new Tag(0x0000, 0x0100);
        /// <summary/>
        public static readonly Tag MESSAGE_ID = new Tag(0x0000, 0x0110);
        /// <summary/>
        public static readonly Tag MESSAGE_ID_BEING_RESPONDED_TO = new Tag(0x0000, 0x0120);
        /// <summary/>
        public static readonly Tag MOVE_DESTINATION = new Tag(0x0000, 0x0600);
        /// <summary/>
        public static readonly Tag PRIORITY = new Tag(0x0000, 0x0700);
        /// <summary/>
        public static readonly Tag DATA_SET_TYPE = new Tag(0x0000, 0x0800);
        /// <summary/>
        public static readonly Tag STATUS = new Tag(0x0000, 0x0900);
        /// <summary/>
        public static readonly Tag OFFENDING_ELEMENT = new Tag(0x0000, 0x0901);
        /// <summary/>
        public static readonly Tag ERROR_COMMENT = new Tag(0x0000, 0x0902);
        /// <summary/>
        public static readonly Tag ERROR_ID = new Tag(0x0000, 0x0903);
        /// <summary/>
        public static readonly Tag AFFECTED_SOP_INSTANCE_UID = new Tag(0x0000, 0x1000);
        /// <summary/>
        public static readonly Tag REQUESTED_SOP_INSTANCE_UID = new Tag(0x0000, 0x1001);
        /// <summary/>
        public static readonly Tag EVENT_TYPE_ID = new Tag(0x0000, 0x1002);
        /// <summary/>
        public static readonly Tag ATTRIBUTE_IDENTIFIER_LIST = new Tag(0x0000, 0x1005);
        /// <summary/>
        public static readonly Tag ACTION_TYPE_ID = new Tag(0x0000, 0x1008);
        /// <summary/>
        public static readonly Tag NUMBER_OF_REMAINING_SUBOPERATIONS = new Tag(0x0000, 0x1020);
        /// <summary/>
        public static readonly Tag NUMBER_OF_COMPLETED_SUBOPERATIONS = new Tag(0x0000, 0x1021);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FAILED_SUBOPERATIONS = new Tag(0x0000, 0x1022);
        /// <summary/>
        public static readonly Tag NUMBER_OF_WARNING_SUBOPERATIONS = new Tag(0x0000, 0x1023);
        /// <summary/>
        public static readonly Tag MOVE_ORIGINATOR_APPLICATION_ENTITY_TITLE = new Tag(0x0000, 0x1030);
        /// <summary/>
        public static readonly Tag MOVE_ORIGINATOR_MESSAGE_ID = new Tag(0x0000, 0x1031);
        /// <summary/>
        public static readonly Tag GROUP_0002_LENGTH = new Tag(0x0002, 0x0000);
        /// <summary/>
        public static readonly Tag FILE_META_INFORMATION_VERSION = new Tag(0x0002, 0x0001);
        /// <summary/>
        public static readonly Tag MEDIA_STORAGE_SOP_CLASS_UID = new Tag(0x0002, 0x0002);
        /// <summary/>
        public static readonly Tag MEDIA_STORAGE_SOP_INSTANCE_UID = new Tag(0x0002, 0x0003);
        /// <summary/>
        public static readonly Tag TRANSFER_SYNTAX_UID = new Tag(0x0002, 0x0010);
        /// <summary/>
        public static readonly Tag IMPLEMENTATION_CLASS_UID = new Tag(0x0002, 0x0012);
        /// <summary/>
        public static readonly Tag IMPLEMENTATION_VERSION_NAME = new Tag(0x0002, 0x0013);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATION_ENTITY_TITLE = new Tag(0x0002, 0x0016);
        /// <summary/>
        public static readonly Tag PRIVATE_INFORMATION_CREATOR_UID = new Tag(0x0002, 0x0100);
        /// <summary/>
        public static readonly Tag PRIVATE_INFORMATION = new Tag(0x0002, 0x0102);
        /// <summary/>
        public static readonly Tag FILE_SET_ID = new Tag(0x0004, 0x1130);
        /// <summary/>
        public static readonly Tag FILE_SET_DESCRIPTOR_FILE_ID = new Tag(0x0004, 0x1141);
        /// <summary/>
        public static readonly Tag SPECIFIC_CHARACTER_SET_OF_FILE_SET_DESCRIPTOR_FILE = new Tag(0x0004, 0x1142);
        /// <summary/>
        public static readonly Tag OFFSET_OF_THE_FIRST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY = new Tag(0x0004, 0x1200);
        /// <summary/>
        public static readonly Tag OFFSET_OF_THE_LAST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY = new Tag(0x0004, 0x1202);
        /// <summary/>
        public static readonly Tag FILE_SET_CONSISTENCY_FLAG = new Tag(0x0004, 0x1212);
        /// <summary/>
        public static readonly Tag DIRECTORY_RECORD_SEQUENCE = new Tag(0x0004, 0x1220);
        /// <summary/>
        public static readonly Tag OFFSET_OF_THE_NEXT_DIRECTORY_RECORD = new Tag(0x0004, 0x1400);
        /// <summary/>
        public static readonly Tag RECORD_IN_USE_FLAG = new Tag(0x0004, 0x1410);
        /// <summary/>
        public static readonly Tag OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY = new Tag(0x0004, 0x1420);
        /// <summary/>
        public static readonly Tag DIRECTORY_RECORD_TYPE = new Tag(0x0004, 0x1430);
        /// <summary/>
        public static readonly Tag PRIVATE_RECORD_UID = new Tag(0x0004, 0x1432);
        /// <summary/>
        public static readonly Tag REFERENCED_FILE_ID = new Tag(0x0004, 0x1500);
        /// <summary/>
        public static readonly Tag MRDR_DIRECTORY_RECORD_OFFSET = new Tag(0x0004, 0x1504);
        /// <summary/>
        public static readonly Tag REFERENCED_SOP_CLASS_UID_IN_FILE = new Tag(0x0004, 0x1510);
        /// <summary/>
        public static readonly Tag REFERENCED_SOP_INSTANCE_UID_IN_FILE = new Tag(0x0004, 0x1511);
        /// <summary/>
        public static readonly Tag REFERENCED_TRANSFER_SYNTAX_UID_IN_FILE = new Tag(0x0004, 0x1512);
        /// <summary/>
        public static readonly Tag NUMBER_OF_REFERENCES = new Tag(0x0004, 0x1600);
        /// <summary/>
        public static readonly Tag GROUP_0008_LENGTH = new Tag(0x0008, 0x0000);
        /// <summary/>
        public static readonly Tag SPECIFIC_CHARACTER_SET = new Tag(0x0008, 0x0005);
        /// <summary/>
        public static readonly Tag IMAGE_TYPE = new Tag(0x0008, 0x0008);
        /// <summary/>
        public static readonly Tag INSTANCE_CREATION_DATE = new Tag(0x0008, 0x0012);
        /// <summary/>
        public static readonly Tag INSTANCE_CREATION_TIME = new Tag(0x0008, 0x0013);
        /// <summary/>
        public static readonly Tag INSTANCE_CREATOR_UID = new Tag(0x0008, 0x0014);
        /// <summary/>
        public static readonly Tag SOP_CLASS_UID = new Tag(0x0008, 0x0016);
        /// <summary/>
        public static readonly Tag SOP_INSTANCE_UID = new Tag(0x0008, 0x0018);
        /// <summary/>
        public static readonly Tag STUDY_DATE = new Tag(0x0008, 0x0020);
        /// <summary/>
        public static readonly Tag SERIES_DATE = new Tag(0x0008, 0x0021);
        /// <summary/>
        public static readonly Tag ACQUISITION_DATE = new Tag(0x0008, 0x0022);
        /// <summary/>
        public static readonly Tag IMAGE_DATE = new Tag(0x0008, 0x0023);
        /// <summary/>
        public static readonly Tag OVERLAY_DATE = new Tag(0x0008, 0x0024);
        /// <summary/>
        public static readonly Tag CURVE_DATE = new Tag(0x0008, 0x0025);
        /// <summary/>
        public static readonly Tag STUDY_TIME = new Tag(0x0008, 0x0030);
        /// <summary/>
        public static readonly Tag SERIES_TIME = new Tag(0x0008, 0x0031);
        /// <summary/>
        public static readonly Tag ACQUISITION_TIME = new Tag(0x0008, 0x0032);
        /// <summary/>
        public static readonly Tag IMAGE_TIME = new Tag(0x0008, 0x0033);
        /// <summary/>
        public static readonly Tag OVERLAY_TIME = new Tag(0x0008, 0x0034);
        /// <summary/>
        public static readonly Tag CURVE_TIME = new Tag(0x0008, 0x0035);
        /// <summary/>
        public static readonly Tag ACCESSION_NUMBER = new Tag(0x0008, 0x0050);
        /// <summary/>
        public static readonly Tag QUERY_RETRIEVE_LEVEL = new Tag(0x0008, 0x0052);
        /// <summary/>
        public static readonly Tag RETRIEVE_AE_TITLE = new Tag(0x0008, 0x0054);
        /// <summary/>
        public static readonly Tag INSTANCE_AVAILABILITY = new Tag(0x0008, 0x0056);
        /// <summary/>
        public static readonly Tag FAILED_SOP_INSTANCE_UID_LIST = new Tag(0x0008, 0x0058);
        /// <summary/>
        public static readonly Tag MODALITY = new Tag(0x0008, 0x0060);
        /// <summary/>
        public static readonly Tag MODALITIES_IN_STUDY = new Tag(0x0008, 0x0061);
        /// <summary/>
        public static readonly Tag CONVERSION_TYPE = new Tag(0x0008, 0x0064);
        /// <summary/>
        public static readonly Tag MANUFACTURER = new Tag(0x0008, 0x0070);
        /// <summary/>
        public static readonly Tag INSTITUTION_NAME = new Tag(0x0008, 0x0080);
        /// <summary/>
        public static readonly Tag INSTITUTION_ADDRESS = new Tag(0x0008, 0x0081);
        /// <summary/>
        public static readonly Tag INSTITUTION_CODE_SEQUENCE = new Tag(0x0008, 0x0082);
        /// <summary/>
        public static readonly Tag REFERRING_PHYSICIANS_NAME = new Tag(0x0008, 0x0090);
        /// <summary/>
        public static readonly Tag REFERRING_PHYSICIANS_ADDRESS = new Tag(0x0008, 0x0092);
        /// <summary/>
		public static readonly Tag REFERRING_PHYSICIANS_TELEPHONE_NUMBERS = new Tag(0x0008, 0x0094);
        /// <summary/>
        public static readonly Tag REFERRING_PHYSICIAN_IDENTIFICATION_SEQUENCE = new Tag(0x0008, 0x0096);
        /// <summary/>
        public static readonly Tag CODE_VALUE = new Tag(0x0008, 0x0100);
        /// <summary/>
        public static readonly Tag CODING_SCHEME_DESIGNATOR = new Tag(0x0008, 0x0102);
        /// <summary/>
        public static readonly Tag CODE_MEANING = new Tag(0x0008, 0x0104);
        /// <summary/>
        public static readonly Tag STATION_NAME = new Tag(0x0008, 0x1010);
        /// <summary/>
        public static readonly Tag STUDY_DESCRIPTION = new Tag(0x0008, 0x1030);
        /// <summary/>
        public static readonly Tag PROCEDURE_CODE_SEQUENCE = new Tag(0x0008, 0x1032);
        /// <summary/>
        public static readonly Tag SERIES_DESCRIPTION = new Tag(0x0008, 0x103E);
        /// <summary/>
        public static readonly Tag INSTITUTIONAL_DEPARTMENT_NAME = new Tag(0x0008, 0x1040);
        /// <summary/>
        public static readonly Tag PHYSICIANS_OF_RECORD = new Tag(0x0008, 0x1048);
        /// <summary/>
        public static readonly Tag PERFORMING_PHYSICIANS_NAME = new Tag(0x0008, 0x1050);
        /// <summary/>
        public static readonly Tag NAME_OF_PHYSICIANS_READING_STUDY = new Tag(0x0008, 0x1060);
        /// <summary/>
        public static readonly Tag OPERATORS_NAME = new Tag(0x0008, 0x1070);
        /// <summary/>
        public static readonly Tag ADMITTING_DIAGNOSIS_DESCRIPTION = new Tag(0x0008, 0x1080);
        /// <summary/>
        public static readonly Tag ADMITTING_DIAGNOSIS_CODE_SEQUENCE = new Tag(0x0008, 0x1084);
        /// <summary/>
        public static readonly Tag MANUFACTURERS_MODEL_NAME = new Tag(0x0008, 0x1090);
        /// <summary/>
        public static readonly Tag REFERENCED_RESULTS_SEQUENCE = new Tag(0x0008, 0x1100);
        /// <summary/>
        public static readonly Tag REFERENCED_STUDY_SEQUENCE = new Tag(0x0008, 0x1110);
        /// <summary/>
        public static readonly Tag REFERENCED_STUDY_COMPONENT_SEQUENCE = new Tag(0x0008, 0x1111);
        /// <summary/>
        public static readonly Tag REFERENCED_SERIES_SEQUENCE = new Tag(0x0008, 0x1115);
        /// <summary/>
        public static readonly Tag REFERENCED_PATIENT_SEQUENCE = new Tag(0x0008, 0x1120);
        /// <summary/>
        public static readonly Tag REFERENCED_VISIT_SEQUENCE = new Tag(0x0008, 0x1125);
        /// <summary/>
        public static readonly Tag REFERENCED_OVERLAY_SEQUENCE = new Tag(0x0008, 0x1130);
        /// <summary/>
        public static readonly Tag REFERENCED_IMAGE_SEQUENCE = new Tag(0x0008, 0x1140);
        /// <summary/>
        public static readonly Tag REFERENCED_CURVE_SEQUENCE = new Tag(0x0008, 0x1145);
        /// <summary/>
        public static readonly Tag REFERENCED_SOP_CLASS_UID = new Tag(0x0008, 0x1150);
        /// <summary/>
        public static readonly Tag REFERENCED_SOP_INSTANCE_UID = new Tag(0x0008, 0x1155);
        /// <summary/>
        public static readonly Tag REFERENCED_FRAME_NUMBER = new Tag(0x0008, 0x1160);
        /// <summary/>
        public static readonly Tag TRANSACTION_UID = new Tag(0x0008, 0x1195);
        /// <summary/>
        public static readonly Tag FAILURE_REASON = new Tag(0x0008, 0x1197);
        /// <summary/>
        public static readonly Tag FAILED_SOP_SEQUENCE = new Tag(0x0008, 0x1198);
        /// <summary/>
        public static readonly Tag REFERENCED_SOP_SEQUENCE = new Tag(0x0008, 0x1199);
        /// <summary/>
        public static readonly Tag DERIVATION_DESCRIPTION = new Tag(0x0008, 0x2111);
        /// <summary/>
        public static readonly Tag SOURCE_IMAGE_SEQUENCE = new Tag(0x0008, 0x2112);
        /// <summary/>
        public static readonly Tag STAGE_NAME = new Tag(0x0008, 0x2120);
        /// <summary/>
        public static readonly Tag STAGE_NUMBER = new Tag(0x0008, 0x2122);
        /// <summary/>
        public static readonly Tag NUMBER_OF_STAGES = new Tag(0x0008, 0x2124);
        /// <summary/>
        public static readonly Tag VIEW_NUMBER = new Tag(0x0008, 0x2128);
        /// <summary/>
        public static readonly Tag NUMBER_OF_EVENT_TIMERS = new Tag(0x0008, 0x2129);
        /// <summary/>
        public static readonly Tag NUMBER_OF_VIEWS_IN_STAGE = new Tag(0x0008, 0x212A);
        /// <summary/>
        public static readonly Tag EVENT_ELAPSED_TIMES = new Tag(0x0008, 0x2130);
        /// <summary/>
        public static readonly Tag EVENT_TIMER_NAMES = new Tag(0x0008, 0x2132);
        /// <summary/>
        public static readonly Tag START_TRIM = new Tag(0x0008, 0x2142);
        /// <summary/>
        public static readonly Tag STOP_TRIM = new Tag(0x0008, 0x2143);
        /// <summary/>
        public static readonly Tag RECOMMENDED_DISPLAY_FRAME_RATE = new Tag(0x0008, 0x2144);
        /// <summary/>
        public static readonly Tag ANATOMIC_REGION_SEQUENCE = new Tag(0x0008, 0x2218);
        /// <summary/>
        public static readonly Tag ANATOMIC_REGION_MODIFIER_SEQUENCE = new Tag(0x0008, 0x2220);
        /// <summary/>
        public static readonly Tag PRIMARY_ANATOMIC_STRUCTURE_SEQUENCE = new Tag(0x0008, 0x2228);
        /// <summary/>
        public static readonly Tag ANATOMIC_STRUCTURE_SPACE_OR_REGION_SEQUENCE = new Tag(0x0008, 0x2229);
        /// <summary/>
        public static readonly Tag PRIMARY_ANATOMIC_STRUCTURE_MODIFIER_SEQUENCE = new Tag(0x0008, 0x2230);
        /// <summary/>
        public static readonly Tag TRANSDUCER_POSITION_SEQUENCE = new Tag(0x0008, 0x2240);
        /// <summary/>
        public static readonly Tag TRANSDUCER_POSITION_MODIFIER_SEQUENCE = new Tag(0x0008, 0x2242);
        /// <summary/>
        public static readonly Tag TRANSDUCER_ORIENTATION_SEQUENCE = new Tag(0x0008, 0x2244);
        /// <summary/>
        public static readonly Tag TRANSDUCER_ORIENTATION_MODIFIER_SEQUENCE = new Tag(0x0008, 0x2246);
        /// <summary/>
        public static readonly Tag CD_MEDICAL_CREATORCODE_GROUP_9 = new Tag(0x0009, 0x0010);
        /// <summary/>
        public static readonly Tag CD_MEDICAL_FILE_LOCATION = new Tag(0x0009, 0x1000);
        /// <summary/>
        public static readonly Tag CD_MEDICAL_FILE_SIZE = new Tag(0x0009, 0x1001);
        /// <summary/>
        public static readonly Tag CD_MEDICAL_REFERENCED_ALTERNATE_IMAGE_SEQUENCE = new Tag(0x0009, 0x1040);
        /// <summary/>
        public static readonly Tag GROUP_0010_LENGTH = new Tag(0x0010, 0x0000);
        /// <summary/>
        public static readonly Tag PATIENTS_NAME = new Tag(0x0010, 0x0010);
        /// <summary/>
        public static readonly Tag PATIENT_ID = new Tag(0x0010, 0x0020);
        /// <summary/>
        public static readonly Tag ISSUER_OF_PATIENT_ID = new Tag(0x0010, 0x0021);
        /// <summary/>
        public static readonly Tag PATIENTS_BIRTH_DATE = new Tag(0x0010, 0x0030);
        /// <summary/>
        public static readonly Tag PATIENTS_BIRTH_TIME = new Tag(0x0010, 0x0032);
        /// <summary/>
        public static readonly Tag PATIENTS_SEX = new Tag(0x0010, 0x0040);
        /// <summary/>
		public static readonly Tag PATIENTS_INSURANCE_PLAN_CODE_SEQUENCE = new Tag(0x0010, 0x0050);
        /// <summary/>
        public static readonly Tag PATIENTS_PRIMARY_LANGUAGE_CODE_SEQUENCE = new Tag(0x0010, 0x0101);
        /// <summary/>
        public static readonly Tag OTHER_PATIENT_IDS = new Tag(0x0010, 0x1000);
        /// <summary/>
        public static readonly Tag OTHER_PATIENT_NAMES = new Tag(0x0010, 0x1001);
        /// <summary/>
        public static readonly Tag PATIENTS_BIRTH_NAME = new Tag(0x0010, 0x1005);
        /// <summary/>
        public static readonly Tag PATIENTS_AGE = new Tag(0x0010, 0x1010);
        /// <summary/>
        public static readonly Tag PATIENTS_SIZE = new Tag(0x0010, 0x1020);
        /// <summary/>
        public static readonly Tag PATIENTS_WEIGHT = new Tag(0x0010, 0x1030);
        /// <summary/>
        public static readonly Tag PATIENTS_ADDRESS = new Tag(0x0010, 0x1040);
        /// <summary/>
        public static readonly Tag PATIENTS_INSURANCE_PLAN_ID = new Tag(0x0010, 0x1050);
        /// <summary/>
        public static readonly Tag PATIENTS_MOTHERS_BIRTH_NAME = new Tag(0x0010, 0x1060);
        /// <summary/>
        public static readonly Tag MILITARY_RANK = new Tag(0x0010, 0x1080);
        /// <summary/>
        public static readonly Tag BRANCH_OF_SERVICE = new Tag(0x0010, 0x1081);
        /// <summary/>
        public static readonly Tag MEDICAL_RECORD_LOCATOR = new Tag(0x0010, 0x1090);
        /// <summary/>
        public static readonly Tag MEDICAL_ALERTS = new Tag(0x0010, 0x2000);
        /// <summary/>
        public static readonly Tag CONTRAST_ALLERGIES = new Tag(0x0010, 0x2110);
        /// <summary/>
        public static readonly Tag COUNTRY_OF_RESIDENCE = new Tag(0x0010, 0x2150);
        /// <summary/>
        public static readonly Tag REGION_OF_RESIDENCE = new Tag(0x0010, 0x2152);
        /// <summary/>
        public static readonly Tag PATIENTS_TELEPHONE_NUMBERS = new Tag(0x0010, 0x2154);
        /// <summary/>
        public static readonly Tag ETHNIC_GROUP = new Tag(0x0010, 0x2160);
        /// <summary/>
        public static readonly Tag OCCUPATION = new Tag(0x0010, 0x2180);
        /// <summary/>
        public static readonly Tag SMOKING_STATUS = new Tag(0x0010, 0x21A0);
        /// <summary/>
        public static readonly Tag ADDITIONAL_PATIENT_HISTORY = new Tag(0x0010, 0x21B0);
        /// <summary/>
        public static readonly Tag PREGNANCY_STATUS = new Tag(0x0010, 0x21C0);
        /// <summary/>
        public static readonly Tag LAST_MENSTRUAL_DATE = new Tag(0x0010, 0x21D0);
        /// <summary/>
        public static readonly Tag PATIENTS_RELIGIOUS_PREFERENCE = new Tag(0x0010, 0x21F0);
        /// <summary/>
        public static readonly Tag PATIENT_SPECIES_DESCRIPTION = new Tag(0x0010, 0x2201);
        /// <summary/>
        public static readonly Tag PATIENT_SPECIES_CODE_SEQUENCE = new Tag(0x0010, 0x2202);
        /// <summary/>
        public static readonly Tag PATIENTS_SEX_NEUTERED = new Tag(0x0010, 0x2203);
        /// <summary/>
        public static readonly Tag PATIENT_BREED_DESCRIPTION = new Tag(0x0010, 0x2292);
        /// <summary/>
        public static readonly Tag PATIENT_BREED_CODE_SEQUENCE = new Tag(0x0010, 0x2293);
        /// <summary/>
        public static readonly Tag BREED_REGISTRATION_SEQUENCE = new Tag(0x0010, 0x2294);
        /// <summary/>
        public static readonly Tag RESPONSIBLE_PERSON = new Tag(0x0010, 0x2297);
        /// <summary/>
        public static readonly Tag RESPONSIBLE_PERSON_ROLE = new Tag(0x0010, 0x2298);
        /// <summary/>
        public static readonly Tag RESPONSIBLE_ORGANIZATION = new Tag(0x0010, 0x2299);
        /// <summary/>
        public static readonly Tag PATIENT_COMMENTS = new Tag(0x0010, 0x4000);
        /// <summary/>
        public static readonly Tag GROUP_0018_LENGTH = new Tag(0x0018, 0x0000);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_AGENT = new Tag(0x0018, 0x0010);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_AGENT_SEQUENCE = new Tag(0x0018, 0x0012);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_ADMINISTRATION_ROUTE_SEQUENCE = new Tag(0x0018, 0x0014);
        /// <summary/>
        public static readonly Tag BODY_PART_EXAMINED = new Tag(0x0018, 0x0015);
        /// <summary/>
        public static readonly Tag SCANNING_SEQUENCE = new Tag(0x0018, 0x0020);
        /// <summary/>
        public static readonly Tag SEQUENCE_VARIANT = new Tag(0x0018, 0x0021);
        /// <summary/>
        public static readonly Tag SCAN_OPTIONS = new Tag(0x0018, 0x0022);
        /// <summary/>
        public static readonly Tag MR_ACQUISITION_TYPE = new Tag(0x0018, 0x0023);
        /// <summary/>
        public static readonly Tag SEQUENCE_NAME = new Tag(0x0018, 0x0024);
        /// <summary/>
        public static readonly Tag ANGIO_FLAG = new Tag(0x0018, 0x0025);
        /// <summary/>
        public static readonly Tag INTERVENTION_DRUG_INFORMATION_SEQUENCE = new Tag(0x0018, 0x0026);
        /// <summary/>
        public static readonly Tag INTERVENTION_DRUG_STOP_TIME = new Tag(0x0018, 0x0027);
        /// <summary/>
        public static readonly Tag INTERVENTION_DRUG_DOSE = new Tag(0x0018, 0x0028);
        /// <summary/>
        public static readonly Tag INTERVENTIONAL_DRUG_CODE_SEQUENCE = new Tag(0x0018, 0x0029);
        /// <summary/>
        public static readonly Tag ADDITIONAL_DRUG_SEQUENCE = new Tag(0x0018, 0x002A);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL = new Tag(0x0018, 0x0031);
        /// <summary/>
        public static readonly Tag INTERVENTION_DRUG_NAME = new Tag(0x0018, 0x0034);
        /// <summary/>
        public static readonly Tag INTERVENTION_DRUG_START_TIME = new Tag(0x0018, 0x0035);
        /// <summary/>
        public static readonly Tag INTERVENTIONAL_THERAPY_SEQUENCE = new Tag(0x0018, 0x0036);
        /// <summary/>
        public static readonly Tag THERAPY_TYPE = new Tag(0x0018, 0x0037);
        /// <summary/>
        public static readonly Tag INTERVENTIONAL_STATUS = new Tag(0x0018, 0x0038);
        /// <summary/>
        public static readonly Tag THERAPY_DESCRIPTION = new Tag(0x0018, 0x0039);
        /// <summary/>
        public static readonly Tag CINE_RATE = new Tag(0x0018, 0x0040);
        /// <summary/>
        public static readonly Tag SLICE_THICKNESS = new Tag(0x0018, 0x0050);
        /// <summary/>
        public static readonly Tag KVP = new Tag(0x0018, 0x0060);
        /// <summary/>
        public static readonly Tag COUNTS_ACCUMULATED = new Tag(0x0018, 0x0070);
        /// <summary/>
        public static readonly Tag ACQUISITION_TERMINATION_CONDITION = new Tag(0x0018, 0x0071);
        /// <summary/>
        public static readonly Tag EFFECTIVE_SERIES_DURATION = new Tag(0x0018, 0x0072);
        /// <summary/>
        public static readonly Tag ACQUISITION_START_CONDITION = new Tag(0x0018, 0x0073);
        /// <summary/>
        public static readonly Tag ACQUISITION_START_CONDITION_DATA = new Tag(0x0018, 0x0074);
        /// <summary/>
        public static readonly Tag ACQUISITION_TERMINATION_CONDITION_DATA = new Tag(0x0018, 0x0075);
        /// <summary/>
        public static readonly Tag REPETITION_TIME = new Tag(0x0018, 0x0080);
        /// <summary/>
        public static readonly Tag ECHO_TIME = new Tag(0x0018, 0x0081);
        /// <summary/>
        public static readonly Tag INVERSION_TIME = new Tag(0x0018, 0x0082);
        /// <summary/>
        public static readonly Tag NUMBER_OF_AVERAGES = new Tag(0x0018, 0x0083);
        /// <summary/>
        public static readonly Tag IMAGING_FREQUENCY = new Tag(0x0018, 0x0084);
        /// <summary/>
        public static readonly Tag IMAGED_NUCLEUS = new Tag(0x0018, 0x0085);
        /// <summary/>
        public static readonly Tag ECHO_NUMBERS = new Tag(0x0018, 0x0086);
        /// <summary/>
        public static readonly Tag MAGNETIC_FIELD_STRENGTH = new Tag(0x0018, 0x0087);
        /// <summary/>
        public static readonly Tag SPACING_BETWEEN_SLICES = new Tag(0x0018, 0x0088);
        /// <summary/>
        public static readonly Tag NUMBER_OF_PHASE_ENCODING_STEPS = new Tag(0x0018, 0x0089);
        /// <summary/>
        public static readonly Tag DATA_COLLECTION_DIAMETER = new Tag(0x0018, 0x0090);
        /// <summary/>
        public static readonly Tag ECHO_TRAIN_LENGTH = new Tag(0x0018, 0x0091);
        /// <summary/>
        public static readonly Tag PERCENT_SAMPLING = new Tag(0x0018, 0x0093);
        /// <summary/>
        public static readonly Tag PERCENT_PHASE_FIELD_OF_VIEW = new Tag(0x0018, 0x0094);
        /// <summary/>
        public static readonly Tag PIXEL_BANDWIDTH = new Tag(0x0018, 0x0095);
        /// <summary/>
        public static readonly Tag DEVICE_SERIAL_NUMBER = new Tag(0x0018, 0x1000);
        /// <summary/>
        public static readonly Tag PLATE_ID = new Tag(0x0018, 0x1004);
        /// <summary/>
        public static readonly Tag SECONDARY_CAPTURE_DEVICE_ID = new Tag(0x0018, 0x1010);
        /// <summary/>
        public static readonly Tag HARDCOPY_CREATION_DEVICE_ID = new Tag(0x0018, 0x1011);
        /// <summary/>
        public static readonly Tag DATE_OF_SECONDARY_CAPTURE = new Tag(0x0018, 0x1012);
        /// <summary/>
        public static readonly Tag TIME_OF_SECONDARY_CAPTURE = new Tag(0x0018, 0x1014);
        /// <summary/>
        public static readonly Tag SECONDARY_CAPTURE_DEVICE_MANUFACTURER = new Tag(0x0018, 0x1016);
        /// <summary/>
        public static readonly Tag HARDCOPY_DEVICE_MANUFACTURER = new Tag(0x0018, 0x1017);
        /// <summary/>
        public static readonly Tag SECONDARY_CAPTURE_DEVICE_MANUFACTURERS_MODEL_NAME = new Tag(0x0018, 0x1018);
        /// <summary/>
        public static readonly Tag SECONDARY_CAPTURE_DEVICE_SOFTWARE_VERSIONS = new Tag(0x0018, 0x1019);
        /// <summary/>
        public static readonly Tag HARCOPY_DEVICE_SOFTWARE_VERSION = new Tag(0x0018, 0x101A);
        /// <summary/>
        public static readonly Tag HARDCOPY_DEVICE_MANUFACTURERS_MODEL_NAME = new Tag(0x0018, 0x101B);
        /// <summary/>
        public static readonly Tag SOFTWARE_VERSIONS = new Tag(0x0018, 0x1020);
        /// <summary/>
        public static readonly Tag VIDEO_IMAGE_FORMAT_ACQUIRED = new Tag(0x0018, 0x1022);
        /// <summary/>
        public static readonly Tag DIGITAL_IMAGE_FORMAT_ACQUIRED = new Tag(0x0018, 0x1023);
        /// <summary/>
        public static readonly Tag PROTOCOL_NAME = new Tag(0x0018, 0x1030);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_ROUTE = new Tag(0x0018, 0x1040);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_VOLUME = new Tag(0x0018, 0x1041);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_START_TIME = new Tag(0x0018, 0x1042);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_STOP_TIME = new Tag(0x0018, 0x1043);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_TOTAL_DOSE = new Tag(0x0018, 0x1044);
        /// <summary/>
        public static readonly Tag SYRINGE_COUNTS = new Tag(0x0018, 0x1045);
        /// <summary/>
        public static readonly Tag CONTRAST_FLOW_RATES = new Tag(0x0018, 0x1046);
        /// <summary/>
        public static readonly Tag CONTRAST_FLOW_DURATIONS = new Tag(0x0018, 0x1047);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_INGREDIENT = new Tag(0x0018, 0x1048);
        /// <summary/>
        public static readonly Tag CONTRAST_BOLUS_INGREDIENT_CONCENTRATION = new Tag(0x0018, 0x1049);
        /// <summary/>
        public static readonly Tag SPATIAL_RESOLUTION = new Tag(0x0018, 0x1050);
        /// <summary/>
        public static readonly Tag TRIGGER_TIME = new Tag(0x0018, 0x1060);
        /// <summary/>
        public static readonly Tag TRIGGER_SOURCE_OR_TYPE = new Tag(0x0018, 0x1061);
        /// <summary/>
        public static readonly Tag NOMINAL_INTERVAL = new Tag(0x0018, 0x1062);
        /// <summary/>
        public static readonly Tag FRAME_TIME = new Tag(0x0018, 0x1063);
        /// <summary/>
        public static readonly Tag FRAMING_TYPE = new Tag(0x0018, 0x1064);
        /// <summary/>
        public static readonly Tag FRAME_TIME_VECTOR = new Tag(0x0018, 0x1065);
        /// <summary/>
        public static readonly Tag FRAME_DELAY = new Tag(0x0018, 0x1066);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_ROUTE = new Tag(0x0018, 0x1070);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_VOLUME = new Tag(0x0018, 0x1071);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_START_TIME = new Tag(0x0018, 0x1072);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_STOP_TIME = new Tag(0x0018, 0x1073);
        /// <summary/>
        public static readonly Tag RADIONUCLIDE_TOTAL_DOSE = new Tag(0x0018, 0x1074);
        /// <summary/>
        public static readonly Tag RADIONUCLIDE_HALF_LIFE = new Tag(0x0018, 0x1075);
        /// <summary/>
        public static readonly Tag RADIONUCLIDE_POSITRON_FRACTION = new Tag(0x0018, 0x1076);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_SPECIFIC_ACTIVITY = new Tag(0x0018, 0x1077);
        /// <summary/>
        public static readonly Tag BEAT_REJECTION_FLAG = new Tag(0x0018, 0x1080);
        /// <summary/>
        public static readonly Tag LOW_R_R_VALUE = new Tag(0x0018, 0x1081);
        /// <summary/>
        public static readonly Tag HIGH_R_R_VALUE = new Tag(0x0018, 0x1082);
        /// <summary/>
        public static readonly Tag INTERVALS_ACQUIRED = new Tag(0x0018, 0x1083);
        /// <summary/>
        public static readonly Tag INTERVALS_REJECTED = new Tag(0x0018, 0x1084);
        /// <summary/>
        public static readonly Tag PVC_REJECTION = new Tag(0x0018, 0x1085);
        /// <summary/>
        public static readonly Tag SKIP_BEATS = new Tag(0x0018, 0x1086);
        /// <summary/>
        public static readonly Tag HEART_RATE = new Tag(0x0018, 0x1088);
        /// <summary/>
        public static readonly Tag CARDIAC_NUMBER_OF_IMAGES = new Tag(0x0018, 0x1090);
        /// <summary/>
        public static readonly Tag TRIGGER_WINDOW = new Tag(0x0018, 0x1094);
        /// <summary/>
        public static readonly Tag RECONSTRUCTION_DIAMETER = new Tag(0x0018, 0x1100);
        /// <summary/>
        public static readonly Tag DISTANCE_SOURCE_TO_DETECTOR = new Tag(0x0018, 0x1110);
        /// <summary/>
        public static readonly Tag DISTANCE_SOURCE_TO_PATIENT = new Tag(0x0018, 0x1111);
        /// <summary/>
        public static readonly Tag ESTIMATED_RADIOGRAPHIC_MAGNIFICATION_FACTOR = new Tag(0x0018, 0x1114);
        /// <summary/>
        public static readonly Tag GANTRY_DETECTOR_TILT = new Tag(0x0018, 0x1120);
        /// <summary/>
        public static readonly Tag GANTRY_DETECTOR_SLEW = new Tag(0x0018, 0x1121);
        /// <summary/>
        public static readonly Tag TABLE_HEIGHT = new Tag(0x0018, 0x1130);
        /// <summary/>
        public static readonly Tag TABLE_TRAVERSE = new Tag(0x0018, 0x1131);
        /// <summary/>
        public static readonly Tag TABLE_MOTION = new Tag(0x0018, 0x1134);
        /// <summary/>
        public static readonly Tag TABLE_VERTICAL_INCREMENT = new Tag(0x0018, 0x1135);
        /// <summary/>
        public static readonly Tag TABLE_LATERAL_INCREMENT = new Tag(0x0018, 0x1136);
        /// <summary/>
        public static readonly Tag TABLE_LONGITUDINAL_INCREMENT = new Tag(0x0018, 0x1137);
        /// <summary/>
        public static readonly Tag TABLE_ANGLE = new Tag(0x0018, 0x1138);
        /// <summary/>
        public static readonly Tag ROTATION_DIRECTION = new Tag(0x0018, 0x1140);
        /// <summary/>
        public static readonly Tag ANGULAR_POSITION = new Tag(0x0018, 0x1141);
        /// <summary/>
        public static readonly Tag RADIAL_POSITION = new Tag(0x0018, 0x1142);
        /// <summary/>
        public static readonly Tag SCAN_ARC = new Tag(0x0018, 0x1143);
        /// <summary/>
        public static readonly Tag ANGULAR_STEP = new Tag(0x0018, 0x1144);
        /// <summary/>
        public static readonly Tag CENTER_OF_ROTATION_OFFSET = new Tag(0x0018, 0x1145);
        /// <summary/>
        public static readonly Tag FIELD_OF_VIEW_SHAPE = new Tag(0x0018, 0x1147);
        /// <summary/>
        public static readonly Tag FIELD_OF_VIEW_DIMENSIONS = new Tag(0x0018, 0x1149);
        /// <summary/>
        public static readonly Tag EXPOSURE_TIME = new Tag(0x0018, 0x1150);
        /// <summary/>
        public static readonly Tag X_RAY_TUBE_CURRENT = new Tag(0x0018, 0x1151);
        /// <summary/>
        public static readonly Tag EXPOSURE = new Tag(0x0018, 0x1152);
        /// <summary/>
        public static readonly Tag AVERAGE_PULSE_WIDTH = new Tag(0x0018, 0x1154);
        /// <summary/>
        public static readonly Tag RADIATION_SETTING = new Tag(0x0018, 0x1155);
        /// <summary/>
        public static readonly Tag RADIATION_MODE = new Tag(0x0018, 0x115A);
        /// <summary/>
        public static readonly Tag IMAGE_AREA_DOSE_PRODUCT = new Tag(0x0018, 0x115E);
        /// <summary/>
        public static readonly Tag FILTER_TYPE = new Tag(0x0018, 0x1160);
        /// <summary/>
        public static readonly Tag TYPE_OF_FILTERS = new Tag(0x0018, 0x1161);
        /// <summary/>
        public static readonly Tag INTENSIFIER_SIZE = new Tag(0x0018, 0x1162);
        /// <summary/>
        public static readonly Tag IMAGER_PIXEL_SPACING = new Tag(0x0018, 0x1164);
        /// <summary/>
        public static readonly Tag GRID = new Tag(0x0018, 0x1166);
        /// <summary/>
        public static readonly Tag GENERATOR_POWER = new Tag(0x0018, 0x1170);
        /// <summary/>
        public static readonly Tag COLLIMATOR_GRID_NAME = new Tag(0x0018, 0x1180);
        /// <summary/>
        public static readonly Tag COLLIMATOR_TYPE = new Tag(0x0018, 0x1181);
        /// <summary/>
        public static readonly Tag FOCAL_DISTANCE = new Tag(0x0018, 0x1182);
        /// <summary/>
        public static readonly Tag X_FOCUS_CENTER = new Tag(0x0018, 0x1183);
        /// <summary/>
        public static readonly Tag Y_FOCUS_CENTER = new Tag(0x0018, 0x1184);
        /// <summary/>
        public static readonly Tag FOCAL_SPOTS = new Tag(0x0018, 0x1190);
        /// <summary/>
        public static readonly Tag DATE_OF_LAST_CALIBRATION = new Tag(0x0018, 0x1200);
        /// <summary/>
        public static readonly Tag TIME_OF_LAST_CALIBRATION = new Tag(0x0018, 0x1201);
        /// <summary/>
        public static readonly Tag CONVOLUTION_KERNEL = new Tag(0x0018, 0x1210);
        /// <summary/>
        public static readonly Tag ACTUAL_FRAME_DURATION = new Tag(0x0018, 0x1242);
        /// <summary/>
        public static readonly Tag COUNT_RATE = new Tag(0x0018, 0x1243);
        /// <summary/>
        public static readonly Tag PREFERRED_PLAYBACK_SEQUENCING = new Tag(0x0018, 0x1244);
        /// <summary/>
        public static readonly Tag RECEIVING_COIL = new Tag(0x0018, 0x1250);
        /// <summary/>
        public static readonly Tag TRANSMITTING_COIL = new Tag(0x0018, 0x1251);
        /// <summary/>
        public static readonly Tag PLATE_TYPE = new Tag(0x0018, 0x1260);
        /// <summary/>
        public static readonly Tag PHOSPHOR_TYPE = new Tag(0x0018, 0x1261);
        /// <summary/>
        public static readonly Tag SCAN_VELOCITY = new Tag(0x0018, 0x1300);
        /// <summary/>
        public static readonly Tag WHOLE_BODY_TECHNIQUE = new Tag(0x0018, 0x1301);
        /// <summary/>
        public static readonly Tag SCAN_LENGTH = new Tag(0x0018, 0x1302);
        /// <summary/>
        public static readonly Tag ACQUISITION_MATRIX = new Tag(0x0018, 0x1310);
        /// <summary/>
        public static readonly Tag PHASE_ENCODING_DIRECTION = new Tag(0x0018, 0x1312);
        /// <summary/>
        public static readonly Tag FLIP_ANGLE = new Tag(0x0018, 0x1314);
        /// <summary/>
        public static readonly Tag VARIABLE_FLIP_ANGLE_FLAG = new Tag(0x0018, 0x1315);
        /// <summary/>
        public static readonly Tag SAR = new Tag(0x0018, 0x1316);
        /// <summary/>
        public static readonly Tag DB_DT = new Tag(0x0018, 0x1318);
        /// <summary/>
        public static readonly Tag ACQUISITION_DEVICE_PROCESSING_DESCRIPTION = new Tag(0x0018, 0x1400);
        /// <summary/>
        public static readonly Tag ACQUISITION_DEVICE_PROCESSING_CODE = new Tag(0x0018, 0x1401);
        /// <summary/>
        public static readonly Tag CASSETTE_ORIENTATION = new Tag(0x0018, 0x1402);
        /// <summary/>
        public static readonly Tag CASSETTE_SIZE = new Tag(0x0018, 0x1403);
        /// <summary/>
        public static readonly Tag EXPOSURES_ON_PLATE = new Tag(0x0018, 0x1404);
        /// <summary/>
        public static readonly Tag RELATIVE_X_RAY_EXPOSURE = new Tag(0x0018, 0x1405);
        /// <summary/>
        public static readonly Tag COLUMN_ANGULATION = new Tag(0x0018, 0x1450);
        /// <summary/>
        public static readonly Tag TOMO_LAYER_HEIGHT = new Tag(0x0018, 0x1460);
        /// <summary/>
        public static readonly Tag TOMO_ANGLE = new Tag(0x0018, 0x1470);
        /// <summary/>
        public static readonly Tag TOMO_TIME = new Tag(0x0018, 0x1480);
        /// <summary/>
        public static readonly Tag POSITIONER_MOTION = new Tag(0x0018, 0x1500);
        /// <summary/>
        public static readonly Tag POSITIONER_PRIMARY_ANGLE = new Tag(0x0018, 0x1510);
        /// <summary/>
        public static readonly Tag POSITIONER_SECONDARY_ANGLE = new Tag(0x0018, 0x1511);
        /// <summary/>
        public static readonly Tag POSITIONER_PRIMARY_ANGLE_INCREMENT = new Tag(0x0018, 0x1520);
        /// <summary/>
        public static readonly Tag POSITIONER_SECONDARY_ANGLE_INCREMENT = new Tag(0x0018, 0x1521);
        /// <summary/>
        public static readonly Tag DETECTOR_PRIMARY_ANGLE = new Tag(0x0018, 0x1530);
        /// <summary/>
        public static readonly Tag DETECTOR_SECONDARY_ANGLE = new Tag(0x0018, 0x1531);
        /// <summary/>
        public static readonly Tag SHUTTER_SHAPE = new Tag(0x0018, 0x1600);
        /// <summary/>
        public static readonly Tag SHUTTER_LEFT_VERTICAL_EDGE = new Tag(0x0018, 0x1602);
        /// <summary/>
        public static readonly Tag SHUTTER_RIGHT_VERTICAL_EDGE = new Tag(0x0018, 0x1604);
        /// <summary/>
        public static readonly Tag SHUTTER_UPPER_HORIZONTAL_EDGE = new Tag(0x0018, 0x1606);
        /// <summary/>
        public static readonly Tag SHUTTER_LOWER_HORIZONTAL_EDGE = new Tag(0x0018, 0x1608);
        /// <summary/>
        public static readonly Tag CENTER_OF_CIRCULAR_SHUTTER = new Tag(0x0018, 0x1610);
        /// <summary/>
        public static readonly Tag RADIUS_OF_CIRCULAR_SHUTTER = new Tag(0x0018, 0x1612);
        /// <summary/>
        public static readonly Tag VERTICES_OF_POLYGONAL_SHUTTER = new Tag(0x0018, 0x1620);
        /// <summary/>
        public static readonly Tag COLLIMATOR_SHAPE = new Tag(0x0018, 0x1700);
        /// <summary/>
        public static readonly Tag COLLIMATOR_LEFT_VERTICAL_EDGE = new Tag(0x0018, 0x1702);
        /// <summary/>
        public static readonly Tag COLLIMATOR_RIGHT_VERTICAL_EDGE = new Tag(0x0018, 0x1704);
        /// <summary/>
        public static readonly Tag COLLIMATOR_UPPER_HORIZONTAL_EDGE = new Tag(0x0018, 0x1706);
        /// <summary/>
        public static readonly Tag COLLIMATOR_LOWER_HORIZONTAL_EDGE = new Tag(0x0018, 0x1708);
        /// <summary/>
        public static readonly Tag CENTER_OF_CIRCULAR_COLLIMATOR = new Tag(0x0018, 0x1710);
        /// <summary/>
        public static readonly Tag RADIUS_OF_CIRCULAR_COLLIMATOR = new Tag(0x0018, 0x1712);
        /// <summary/>
        public static readonly Tag VERTICES_OF_THE_POLYGONAL_COLLIMATOR = new Tag(0x0018, 0x1720);
        /// <summary/>
        public static readonly Tag OUTPUT_POWER = new Tag(0x0018, 0x5000);
        /// <summary/>
        public static readonly Tag TRANSDUCER_DATA = new Tag(0x0018, 0x5010);
        /// <summary/>
        public static readonly Tag FOCUS_DEPTH = new Tag(0x0018, 0x5012);
        /// <summary/>
        public static readonly Tag PROCESSING_FUNCTION = new Tag(0x0018, 0x5020);
        /// <summary/>
        public static readonly Tag POSTPROCESSING_FUNCTION = new Tag(0x0018, 0x5021);
        /// <summary/>
        public static readonly Tag MECHANICAL_INDEX = new Tag(0x0018, 0x5022);
        /// <summary/>
        public static readonly Tag THERMAL_INDEX = new Tag(0x0018, 0x5024);
        /// <summary/>
        public static readonly Tag CRANIAL_THERMAL_INDEX = new Tag(0x0018, 0x5026);
        /// <summary/>
        public static readonly Tag SOFT_TISSUE_THERMAL_INDEX = new Tag(0x0018, 0x5027);
        /// <summary/>
        public static readonly Tag SOFT_TISSUE_FOCUS_THERMAL_INDEX = new Tag(0x0018, 0x5028);
        /// <summary/>
        public static readonly Tag SOFT_TISSUE_SURFACE_THERMAL_INDEX = new Tag(0x0018, 0x5029);
        /// <summary/>
        public static readonly Tag DEPTH_OF_SCAN_FIELD = new Tag(0x0018, 0x5050);
        /// <summary/>
        public static readonly Tag PATIENT_POSITION = new Tag(0x0018, 0x5100);
        /// <summary/>
        public static readonly Tag VIEW_POSITION = new Tag(0x0018, 0x5101);
        /// <summary/>
        public static readonly Tag IMAGE_TRANSFORMATION_MATRIX = new Tag(0x0018, 0x5210);
        /// <summary/>
        public static readonly Tag IMAGE_TRANSLATION_VECTOR = new Tag(0x0018, 0x5212);
        /// <summary/>
        public static readonly Tag SENSITIVITY = new Tag(0x0018, 0x6000);
        /// <summary/>
        public static readonly Tag SEQUENCE_OF_ULTRASOUND_REGIONS = new Tag(0x0018, 0x6011);
        /// <summary/>
        public static readonly Tag REGION_SPATIAL_FORMAT = new Tag(0x0018, 0x6012);
        /// <summary/>
        public static readonly Tag REGION_DATA_TYPE = new Tag(0x0018, 0x6014);
        /// <summary/>
        public static readonly Tag REGION_FLAGS = new Tag(0x0018, 0x6016);
        /// <summary/>
        public static readonly Tag REGION_LOCATION_MIN_X0 = new Tag(0x0018, 0x6018);
        /// <summary/>
        public static readonly Tag REGION_LOCATION_MIN_Y0 = new Tag(0x0018, 0x601A);
        /// <summary/>
        public static readonly Tag REGION_LOCATION_MAX_X1 = new Tag(0x0018, 0x601C);
        /// <summary/>
        public static readonly Tag REGION_LOCATION_MAX_Y1 = new Tag(0x0018, 0x601E);
        /// <summary/>
        public static readonly Tag REFERENCE_PIXEL_X0 = new Tag(0x0018, 0x6020);
        /// <summary/>
        public static readonly Tag REFERENCE_PIXEL_Y0 = new Tag(0x0018, 0x6022);
        /// <summary/>
        public static readonly Tag PHYSICAL_UNITS_X_DIRECTION = new Tag(0x0018, 0x6024);
        /// <summary/>
        public static readonly Tag PHYSICAL_UNITS_Y_DIRECTION = new Tag(0x0018, 0x6026);
        /// <summary/>
        public static readonly Tag REFERENCE_PIXEL_PHYSICAL_VALUE_X = new Tag(0x0018, 0x6028);
        /// <summary/>
        public static readonly Tag REFERENCE_PIXEL_PHYSICAL_VALUE_Y = new Tag(0x0018, 0x602A);
        /// <summary/>
        public static readonly Tag PHYSICAL_DELTA_X = new Tag(0x0018, 0x602C);
        /// <summary/>
        public static readonly Tag PHYSICAL_DELTA_Y = new Tag(0x0018, 0x602E);
        /// <summary/>
        public static readonly Tag TRANSDUCER_FREQUENCY = new Tag(0x0018, 0x6030);
        /// <summary/>
        public static readonly Tag TRANSDUCER_TYPE = new Tag(0x0018, 0x6031);
        /// <summary/>
        public static readonly Tag PULSE_REPETITION_FREQUENCY = new Tag(0x0018, 0x6032);
        /// <summary/>
        public static readonly Tag DOPPLER_CORRECTION_ANGLE = new Tag(0x0018, 0x6034);
        /// <summary/>
        public static readonly Tag STEERING_ANGLE = new Tag(0x0018, 0x6036);
        /// <summary/>
        public static readonly Tag DOPPLER_SAMPLE_VOLUME_X_POSITION = new Tag(0x0018, 0x6038);
        /// <summary/>
        public static readonly Tag DOPPLER_SAMPLE_VOLUME_Y_POSITION = new Tag(0x0018, 0x603A);
        /// <summary/>
        public static readonly Tag TM_LINE_POSITION_X0 = new Tag(0x0018, 0x603C);
        /// <summary/>
        public static readonly Tag TM_LINE_POSITION_Y0 = new Tag(0x0018, 0x603E);
        /// <summary/>
        public static readonly Tag TM_LINE_POSITION_X1 = new Tag(0x0018, 0x6040);
        /// <summary/>
        public static readonly Tag TM_LINE_POSITION_Y1 = new Tag(0x0018, 0x6042);
        /// <summary/>
        public static readonly Tag PIXEL_COMPONENT_ORGANIZATION = new Tag(0x0018, 0x6044);
        /// <summary/>
        public static readonly Tag PIXEL_COMPONENT_MASK = new Tag(0x0018, 0x6046);
        /// <summary/>
        public static readonly Tag PIXEL_COMPONENT_RANGE_START = new Tag(0x0018, 0x6048);
        /// <summary/>
        public static readonly Tag PIXEL_COMPONENT_RANGE_STOP = new Tag(0x0018, 0x604A);
        /// <summary/>
        public static readonly Tag PIXEL_COMPONENT_PHYSICAL_UNITS = new Tag(0x0018, 0x604C);
        /// <summary/>
        public static readonly Tag PIXEL_COMPONENT_DATA_TYPE = new Tag(0x0018, 0x604E);
        /// <summary/>
        public static readonly Tag NUMBER_OF_TABLE_BREAK_POINTS = new Tag(0x0018, 0x6050);
        /// <summary/>
        public static readonly Tag TABLE_OF_X_BREAK_POINTS = new Tag(0x0018, 0x6052);
        /// <summary/>
        public static readonly Tag TABLE_OF_Y_BREAK_POINTS = new Tag(0x0018, 0x6054);
        /// <summary/>
        public static readonly Tag NUMBER_OF_TABLE_ENTRIES = new Tag(0x0018, 0x6056);
        /// <summary/>
        public static readonly Tag TABLE_OF_PIXEL_VALUES = new Tag(0x0018, 0x6058);
        /// <summary/>
        public static readonly Tag TABLE_OF_PARAMETER_VALUES = new Tag(0x0018, 0x605A);
        /// <summary/>
		public static readonly Tag FILTER_MATERIAL = new Tag(0x0018, 0x7050);
        /// <summary/>
        public static readonly Tag X_RAY_TUBE_CURRENT_IN_UA = new Tag(0x0018, 0x8151);
        /// <summary/>
        public static readonly Tag GROUP_0020_LENGTH = new Tag(0x0020, 0x0000);
        /// <summary/>
        public static readonly Tag STUDY_INSTANCE_UID = new Tag(0x0020, 0x000D);
        /// <summary/>
        public static readonly Tag SERIES_INSTANCE_UID = new Tag(0x0020, 0x000E);
        /// <summary/>
        public static readonly Tag STUDY_ID = new Tag(0x0020, 0x0010);
        /// <summary/>
        public static readonly Tag SERIES_NUMBER = new Tag(0x0020, 0x0011);
        /// <summary/>
        public static readonly Tag ACQUISITION_NUMBER = new Tag(0x0020, 0x0012);
        /// <summary/>
        public static readonly Tag INSTANCE_NUMBER = new Tag(0x0020, 0x0013);
        /// <summary/>
        public static readonly Tag PATIENT_ORIENTATION = new Tag(0x0020, 0x0020);
        /// <summary/>
        public static readonly Tag OVERLAY_NUMBER = new Tag(0x0020, 0x0022);
        /// <summary/>
        public static readonly Tag CURVE_NUMBER = new Tag(0x0020, 0x0024);
        /// <summary/>
        public static readonly Tag LUT_NUMBER = new Tag(0x0020, 0x0026);
        /// <summary/>
        public static readonly Tag IMAGE_POSITION_PATIENT = new Tag(0x0020, 0x0032);
        /// <summary/>
        public static readonly Tag IMAGE_ORIENTATION_PATIENT = new Tag(0x0020, 0x0037);
        /// <summary/>
        public static readonly Tag FRAME_OF_REFERENCE_UID = new Tag(0x0020, 0x0052);
        /// <summary/>
        public static readonly Tag LATERALITY = new Tag(0x0020, 0x0060);
        /// <summary/>
        public static readonly Tag TEMPORAL_POSITION_IDENTIFIER = new Tag(0x0020, 0x0100);
        /// <summary/>
        public static readonly Tag NUMBER_OF_TEMPORAL_POSITIONS = new Tag(0x0020, 0x0105);
        /// <summary/>
        public static readonly Tag TEMPORAL_RESOLUTION = new Tag(0x0020, 0x0110);
        /// <summary/>
        public static readonly Tag SYNCHRONIZATION_FRAME_OF_REFERENCE_UID = new Tag(0x0020, 0x0200);
        /// <summary/>
        public static readonly Tag SERIES_IN_STUDY = new Tag(0x0020, 0x1000);
        /// <summary/>
        public static readonly Tag IMAGES_IN_ACQUISITION = new Tag(0x0020, 0x1002);
        /// <summary/>
        public static readonly Tag ACQUISITIONS_IN_STUDY = new Tag(0x0020, 0x1004);
        /// <summary/>
        public static readonly Tag POSITION_REFERENCE_INDICATOR = new Tag(0x0020, 0x1040);
        /// <summary/>
        public static readonly Tag SLICE_LOCATION = new Tag(0x0020, 0x1041);
        /// <summary/>
        public static readonly Tag OTHER_STUDY_NUMBERS = new Tag(0x0020, 0x1070);
        /// <summary/>
        public static readonly Tag NUMBER_OF_PATIENT_RELATED_STUDIES = new Tag(0x0020, 0x1200);
        /// <summary/>
        public static readonly Tag NUMBER_OF_PATIENT_RELATED_SERIES = new Tag(0x0020, 0x1202);
        /// <summary/>
        public static readonly Tag NUMBER_OF_PATIENT_RELATED_INSTANCES = new Tag(0x0020, 0x1204);
        /// <summary/>
        public static readonly Tag NUMBER_OF_STUDY_RELATED_SERIES = new Tag(0x0020, 0x1206);
        /// <summary/>
        public static readonly Tag NUMBER_OF_STUDY_RELATED_INSTANCES = new Tag(0x0020, 0x1208);
        /// <summary/>
        public static readonly Tag NUMBER_OF_SERIES_RELATED_INSTANCES = new Tag(0x0020, 0x1209);
        /// <summary/>
        public static readonly Tag IMAGE_COMMENTS = new Tag(0x0020, 0x4000);
        /// <summary/>
        public static readonly Tag GROUP_0028_LENGTH = new Tag(0x0028, 0x0000);
        /// <summary/>
        public static readonly Tag SAMPLES_PER_PIXEL = new Tag(0x0028, 0x0002);
        /// <summary/>
        public static readonly Tag PHOTOMETRIC_INTERPRETATION = new Tag(0x0028, 0x0004);
        /// <summary/>
        public static readonly Tag PLANAR_CONFIGURATION = new Tag(0x0028, 0x0006);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FRAMES = new Tag(0x0028, 0x0008);
        /// <summary/>
        public static readonly Tag FRAME_INCREMENT_POINTER = new Tag(0x0028, 0x0009);
        /// <summary/>
        public static readonly Tag ROWS = new Tag(0x0028, 0x0010);
        /// <summary/>
        public static readonly Tag COLUMNS = new Tag(0x0028, 0x0011);
        /// <summary/>
        public static readonly Tag PLANES = new Tag(0x0028, 0x0012);
        /// <summary/>
        public static readonly Tag ULTRASOUND_COLOR_DATA_PRESENT = new Tag(0x0028, 0x0014);
        /// <summary/>
        public static readonly Tag PIXEL_SPACING = new Tag(0x0028, 0x0030);
        /// <summary/>
        public static readonly Tag ZOOM_FACTOR = new Tag(0x0028, 0x0031);
        /// <summary/>
        public static readonly Tag ZOOM_CENTER = new Tag(0x0028, 0x0032);
        /// <summary/>
        public static readonly Tag PIXEL_ASPECT_RATIO = new Tag(0x0028, 0x0034);
        /// <summary/>
        public static readonly Tag CORRECTED_IMAGE = new Tag(0x0028, 0x0051);
        /// <summary/>
        public static readonly Tag BITS_ALLOCATED = new Tag(0x0028, 0x0100);
        /// <summary/>
        public static readonly Tag BITS_STORED = new Tag(0x0028, 0x0101);
        /// <summary/>
        public static readonly Tag HIGH_BIT = new Tag(0x0028, 0x0102);
        /// <summary/>
        public static readonly Tag PIXEL_REPRESENTATION = new Tag(0x0028, 0x0103);
        /// <summary/>
        public static readonly Tag SMALLEST_IMAGE_PIXEL_VALUE = new Tag(0x0028, 0x0106);
        /// <summary/>
        public static readonly Tag LARGEST_IMAGE_PIXEL_VALUE = new Tag(0x0028, 0x0107);
        /// <summary/>
        public static readonly Tag SMALLEST_PIXEL_VALUE_IN_SERIES = new Tag(0x0028, 0x0108);
        /// <summary/>
        public static readonly Tag LARGEST_PIXEL_VALUE_IN_SERIES = new Tag(0x0028, 0x0109);
        /// <summary/>
        public static readonly Tag SMALLEST_IMAGE_PIXEL_VALUE_IN_PLANE = new Tag(0x0028, 0x0110);
        /// <summary/>
        public static readonly Tag LARGEST_IMAGE_PIXEL_VALUE_IN_PLANE = new Tag(0x0028, 0x0111);
        /// <summary/>
        public static readonly Tag PIXEL_PADDING_VALUE = new Tag(0x0028, 0x0120);
        /// <summary/>
        public static readonly Tag PIXEL_INTENSITY_RELATIONSHIP = new Tag(0x0028, 0x1040);
        /// <summary/>
        public static readonly Tag WINDOW_CENTER = new Tag(0x0028, 0x1050);
        /// <summary/>
        public static readonly Tag WINDOW_WIDTH = new Tag(0x0028, 0x1051);
        /// <summary/>
        public static readonly Tag RESCALE_INTERCEPT = new Tag(0x0028, 0x1052);
        /// <summary/>
        public static readonly Tag RESCALE_SLOPE = new Tag(0x0028, 0x1053);
        /// <summary/>
        public static readonly Tag RESCALE_TYPE = new Tag(0x0028, 0x1054);
        /// <summary/>
        public static readonly Tag WINDOW_CENTER_WIDTH_EXPLANATION = new Tag(0x0028, 0x1055);
        /// <summary/>
        public static readonly Tag RECOMMENDED_VIEWING_MODE = new Tag(0x0028, 0x1090);
        /// <summary/>
        public static readonly Tag RED_PALETTE_COLOR_LOOKUP_TABLE_DESCRIPTOR = new Tag(0x0028, 0x1101);
        /// <summary/>
        public static readonly Tag GREEN_PALETTE_COLOR_LOOKUP_TABLE_DESCRIPTOR = new Tag(0x0028, 0x1102);
        /// <summary/>
        public static readonly Tag BLUE_PALETTE_COLOR_LOOKUP_TABLE_DESCRIPTOR = new Tag(0x0028, 0x1103);
        /// <summary/>
        public static readonly Tag PALETTE_COLOR_LOOKUP_TABLE_UID = new Tag(0x0028, 0x1199);
        /// <summary/>
        public static readonly Tag RED_PALETTE_COLOR_LOOKUP_TABLE_DATA = new Tag(0x0028, 0x1201);
        /// <summary/>
        public static readonly Tag GREEN_PALETTE_COLOR_LOOKUP_TABLE_DATA = new Tag(0x0028, 0x1202);
        /// <summary/>
        public static readonly Tag BLUE_PALETTE_COLOR_LOOKUP_TABLE_DATA = new Tag(0x0028, 0x1203);
        /// <summary/>
        public static readonly Tag SEGMENTED_RED_PALETTE_COLOR_LOOKUP_TABLE = new Tag(0x0028, 0x1221);
        /// <summary/>
        public static readonly Tag SEGMENTED_GREEN_PALETTE_COLOR_LOOKUP_TABLE = new Tag(0x0028, 0x1222);
        /// <summary/>
        public static readonly Tag SEGMENTED_BLUE_PALETTE_COLOR_LOOKUP_TABLE = new Tag(0x0028, 0x1223);
        /// <summary/>
        public static readonly Tag LOSSY_IMAGE_COMPRESSION = new Tag(0x0028, 0x2110);
        /// <summary/>
        public static readonly Tag MODALITY_LUT_SEQUENCE = new Tag(0x0028, 0x3000);
        /// <summary/>
        public static readonly Tag LUT_DESCRIPTOR = new Tag(0x0028, 0x3002);
        /// <summary/>
        public static readonly Tag LUT_EXPLANATION = new Tag(0x0028, 0x3003);
        /// <summary/>
        public static readonly Tag MODALITY_LUT_TYPE = new Tag(0x0028, 0x3004);
        /// <summary/>
        public static readonly Tag LUT_DATA = new Tag(0x0028, 0x3006);
        /// <summary/>
        public static readonly Tag VOI_LUT_SEQUENCE = new Tag(0x0028, 0x3010);
        /// <summary/>
        public static readonly Tag BI_PLANE_ACQUISITION_SEQUENCE = new Tag(0x0028, 0x5000);
        /// <summary/>
        public static readonly Tag REPRESENTATIVE_FRAME_NUMBER = new Tag(0x0028, 0x6010);
        /// <summary/>
        public static readonly Tag FRAME_NUMBERS_OF_INTEREST = new Tag(0x0028, 0x6020);
        /// <summary/>
        public static readonly Tag FRAME_OF_INTEREST_DESCRIPTION = new Tag(0x0028, 0x6022);
        /// <summary/>
        public static readonly Tag MASK_POINTERS = new Tag(0x0028, 0x6030);
        /// <summary/>
        public static readonly Tag R_WAVE_POINTER = new Tag(0x0028, 0x6040);
        /// <summary/>
        public static readonly Tag MASK_SUBTRACTION_SEQUENCE = new Tag(0x0028, 0x6100);
        /// <summary/>
        public static readonly Tag MASK_OPERATION = new Tag(0x0028, 0x6101);
        /// <summary/>
        public static readonly Tag APPLICABLE_FRAME_RANGE = new Tag(0x0028, 0x6102);
        /// <summary/>
        public static readonly Tag MASK_FRAME_NUMBERS = new Tag(0x0028, 0x6110);
        /// <summary/>
        public static readonly Tag CONTRAST_FRAME_AVERAGING = new Tag(0x0028, 0x6112);
        /// <summary/>
        public static readonly Tag MASK_SUB_PIXEL_SHIFT = new Tag(0x0028, 0x6114);
        /// <summary/>
        public static readonly Tag TID_OFFSET = new Tag(0x0028, 0x6120);
        /// <summary/>
        public static readonly Tag MASK_OPERATION_EXPLANATION = new Tag(0x0028, 0x6190);
        /// <summary/>
        public static readonly Tag GROUP_0032_LENGTH = new Tag(0x0032, 0x0000);
        /// <summary/>
        public static readonly Tag STUDY_STATUS_ID = new Tag(0x0032, 0x000A);
        /// <summary/>
        public static readonly Tag STUDY_PRIORITY_ID = new Tag(0x0032, 0x000C);
        /// <summary/>
        public static readonly Tag STUDY_ID_ISSUER = new Tag(0x0032, 0x0012);
        /// <summary/>
        public static readonly Tag STUDY_VERIFIED_DATE = new Tag(0x0032, 0x0032);
        /// <summary/>
        public static readonly Tag STUDY_VERIFIED_TIME = new Tag(0x0032, 0x0033);
        /// <summary/>
        public static readonly Tag STUDY_READ_DATE = new Tag(0x0032, 0x0034);
        /// <summary/>
        public static readonly Tag STUDY_READ_TIME = new Tag(0x0032, 0x0035);
        /// <summary/>
        public static readonly Tag SCHEDULED_STUDY_START_DATE = new Tag(0x0032, 0x1000);
        /// <summary/>
        public static readonly Tag SCHEDULED_STUDY_START_TIME = new Tag(0x0032, 0x1001);
        /// <summary/>
        public static readonly Tag SCHEDULED_STUDY_STOP_DATE = new Tag(0x0032, 0x1010);
        /// <summary/>
        public static readonly Tag SCHEDULED_STUDY_STOP_TIME = new Tag(0x0032, 0x1011);
        /// <summary/>
        public static readonly Tag SCHEDULED_STUDY_LOCATION = new Tag(0x0032, 0x1020);
        /// <summary/>
        public static readonly Tag SCHEDULED_STUDY_LOCATION_AE_TITLES = new Tag(0x0032, 0x1021);
        /// <summary/>
        public static readonly Tag REASON_FOR_STUDY = new Tag(0x0032, 0x1030);
        /// <summary/>
        public static readonly Tag REQUESTING_PHYSICIAN_IDENTIFICATION_SEQUENCE = new Tag(0x0032, 0x1031);
        /// <summary/>
        public static readonly Tag REQUESTING_PHYSICIAN = new Tag(0x0032, 0x1032);
        /// <summary/>
        public static readonly Tag REQUESTING_SERVICE = new Tag(0x0032, 0x1033);
        /// <summary/>
        public static readonly Tag STUDY_ARRIVAL_DATE = new Tag(0x0032, 0x1040);
        /// <summary/>
        public static readonly Tag STUDY_ARRIVAL_TIME = new Tag(0x0032, 0x1041);
        /// <summary/>
        public static readonly Tag STUDY_COMPLETION_DATE = new Tag(0x0032, 0x1050);
        /// <summary/>
        public static readonly Tag STUDY_COMPLETION_TIME = new Tag(0x0032, 0x1051);
        /// <summary/>
        public static readonly Tag STUDY_COMPONENT_STATUS_ID = new Tag(0x0032, 0x1055);
        /// <summary/>
        public static readonly Tag REQUESTED_PROCEDURE_DESCRIPTION = new Tag(0x0032, 0x1060);
        /// <summary/>
        public static readonly Tag REQUESTED_PROCEDURE_CODE_SEQUENCE = new Tag(0x0032, 0x1064);
        /// <summary/>
        public static readonly Tag REQUESTED_CONTRAST_AGENT = new Tag(0x0032, 0x1070);
        /// <summary/>
        public static readonly Tag STUDY_COMMENTS = new Tag(0x0032, 0x4000);
        /// <summary/>
        public static readonly Tag GROUP_0038_LENGTH = new Tag(0x0038, 0x0000);
        /// <summary/>
        public static readonly Tag REFERENCED_PATIENT_ALIAS_SEQUENCE = new Tag(0x0038, 0x0004);
        /// <summary/>
        public static readonly Tag VISIT_STATUS_ID = new Tag(0x0038, 0x0008);
        /// <summary/>
        public static readonly Tag ADMISSION_ID = new Tag(0x0038, 0x0010);
        /// <summary/>
        public static readonly Tag ISSUER_OF_ADMISSION_ID = new Tag(0x0038, 0x0011);
        /// <summary/>
        public static readonly Tag ROUTE_OF_ADMISSIONS = new Tag(0x0038, 0x0016);
        /// <summary/>
        public static readonly Tag SCHEDULED_ADMISSION_DATE = new Tag(0x0038, 0x001A);
        /// <summary/>
        public static readonly Tag SCHEDULED_ADMISSION_TIME = new Tag(0x0038, 0x001B);
        /// <summary/>
        public static readonly Tag SCHEDULED_DISCHARGE_DATE = new Tag(0x0038, 0x001C);
        /// <summary/>
        public static readonly Tag SCHEDULED_DISCHARGE_TIME = new Tag(0x0038, 0x001D);
        /// <summary/>
        public static readonly Tag SCHEDULED_PATIENT_INSTITUTION_RESIDENCE = new Tag(0x0038, 0x001E);
        /// <summary/>
        public static readonly Tag ADMITTING_DATE = new Tag(0x0038, 0x0020);
        /// <summary/>
        public static readonly Tag ADMITTING_TIME = new Tag(0x0038, 0x0021);
        /// <summary/>
        public static readonly Tag DISCHARGE_DATE = new Tag(0x0038, 0x0030);
        /// <summary/>
        public static readonly Tag DISCHARGE_TIME = new Tag(0x0038, 0x0032);
        /// <summary/>
        public static readonly Tag DISCHARGE_DIAGNOSIS_DESCRIPTION = new Tag(0x0038, 0x0040);
        /// <summary/>
        public static readonly Tag DISCHARGE_DIAGNOSIS_CODE_SEQUENCE = new Tag(0x0038, 0x0044);
        /// <summary/>
        public static readonly Tag SPECIAL_NEEDS = new Tag(0x0038, 0x0050);
        /// <summary/>
        public static readonly Tag CURRENT_PATIENT_LOCATION = new Tag(0x0038, 0x0300);
        /// <summary/>
        public static readonly Tag PATIENTS_INSTITUTION_RESIDENCE = new Tag(0x0038, 0x0400);
        /// <summary/>
        public static readonly Tag PATIENT_STATE = new Tag(0x0038, 0x0500);
        /// <summary/>
        public static readonly Tag VISIT_COMMENTS = new Tag(0x0038, 0x4000);
        /// <summary/>
        public static readonly Tag GROUP_0040_LENGTH = new Tag(0x0040, 0x0000);
        /// <summary/>
        public static readonly Tag SCHEDULED_STATION_AE_TITLE = new Tag(0x0040, 0x0001);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_START_DATE = new Tag(0x0040, 0x0002);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_START_TIME = new Tag(0x0040, 0x0003);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_END_DATE = new Tag(0x0040, 0x0004);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_END_TIME = new Tag(0x0040, 0x0005);
        /// <summary/>
        public static readonly Tag SCHEDULED_PERFORMING_PHYSICIANS_NAME = new Tag(0x0040, 0x0006);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_DESCRIPTION = new Tag(0x0040, 0x0007);
        /// <summary/>
        public static readonly Tag SCHEDULED_ACTION_ITEM_CODE_SEQUENCE = new Tag(0x0040, 0x0008);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_ID = new Tag(0x0040, 0x0009);
        /// <summary/>
        public static readonly Tag SCHEDULED_PERFORMING_PHYSICIANS_IDENTIFICATION_SEQUENCE = new Tag(0x0040, 0x000B);
        /// <summary/>
        public static readonly Tag SCHEDULED_STATION_NAME = new Tag(0x0040, 0x0010);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_LOCATION = new Tag(0x0040, 0x0011);
        /// <summary/>
        public static readonly Tag PRE_MEDICATION = new Tag(0x0040, 0x0012);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_STATUS = new Tag(0x0040, 0x0020);
        /// <summary/>
        public static readonly Tag SCHEDULED_PROCEDURE_STEP_SEQUENCE = new Tag(0x0040, 0x0100);
        /// <summary/>
		public static readonly Tag REFERENCED_STANDALONE_SOP_INSTANCE_SEQUENCE = new Tag(0x0040, 0x0220);
        /// <summary/>
        public static readonly Tag PERFORMED_STATION_AE_TITLE = new Tag(0x0040, 0x0241);
        /// <summary/>
        public static readonly Tag PERFORMED_STATION_NAME = new Tag(0x0040, 0x0242);
        /// <summary/>
        public static readonly Tag PERFORMED_LOCATION = new Tag(0x0040, 0x0243);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_STEP_START_DATE = new Tag(0x0040, 0x0244);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_STEP_START_TIME = new Tag(0x0040, 0x0245);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_STEP_END_DATE = new Tag(0x0040, 0x0250);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_STEP_END_TIME = new Tag(0x0040, 0x0251);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_STEP_STATUS = new Tag(0x0040, 0x0252);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_STEP_ID = new Tag(0x0040, 0x0253);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_STEP_DESCRIPTION = new Tag(0x0040, 0x0254);
        /// <summary/>
        public static readonly Tag PERFORMED_PROCEDURE_TYPE_DESCRIPTION = new Tag(0x0040, 0x0255);
        /// <summary/>
        public static readonly Tag PERFORMED_ACTION_ITEM_SEQUENCE = new Tag(0x0040, 0x0260);
        /// <summary/>
        public static readonly Tag SCHEDULED_STEP_ATTRIBUTES_SEQUENCE = new Tag(0x0040, 0x0270);
        /// <summary/>
        public static readonly Tag REQUEST_ATTRIBUTES_SEQUENCE = new Tag(0x0040, 0x0275);
        /// <summary/>
        public static readonly Tag COMMENTS_ON_THE_PERFORMED_PROCEDURE_STEPS = new Tag(0x0040, 0x0280);
        /// <summary/>
        public static readonly Tag QUALITY_SEQUENCE = new Tag(0x0040, 0x0293);
        /// <summary/>
        public static readonly Tag QUALITY = new Tag(0x0040, 0x0294);
        /// <summary/>
        public static readonly Tag MEASURING_UNITS_SEQUENCE = new Tag(0x0040, 0x0295);
        /// <summary/>
        public static readonly Tag BILLING_ITEM_SEQUENCE = new Tag(0x0040, 0x0296);
        /// <summary/>
        public static readonly Tag TOTAL_TIME_OF_FLUOROSCOPY = new Tag(0x0040, 0x0300);
        /// <summary/>
        public static readonly Tag TOTAL_NUMBER_OF_EXPOSURES = new Tag(0x0040, 0x0301);
        /// <summary/>
        public static readonly Tag ENTRANCE_DOSE = new Tag(0x0040, 0x0302);
        /// <summary/>
        public static readonly Tag EXPOSED_AREA = new Tag(0x0040, 0x0303);
        /// <summary/>
		public static readonly Tag DISTANCE_SOURCE_TO_ENTRANCE = new Tag(0x0040, 0x0306);
        /// <summary/>
        public static readonly Tag EXPOSURE_DOSE_SEQUENCE = new Tag(0x0040, 0x030E);
        /// <summary/>
        public static readonly Tag COMMENTS_ON_RADIATION_DOSE = new Tag(0x0040, 0x0310);
        /// <summary/>
        public static readonly Tag BILLING_PROCEDURE_STEP_SEQUENCE = new Tag(0x0040, 0x0320);
        /// <summary/>
        public static readonly Tag FILM_CONSUMPTION_SEQUENCE = new Tag(0x0040, 0x0321);
        /// <summary/>
        public static readonly Tag BILLING_SUPPLIES_AND_DEVICES_SEQUENCE = new Tag(0x0040, 0x0324);
        /// <summary/>
        public static readonly Tag REFERENCED_PROCEDURE_STEP_SEQUENCE = new Tag(0x0040, 0x0330);
        /// <summary/>
        public static readonly Tag PERFORMED_SERIES_SEQUENCE = new Tag(0x0040, 0x0340);
        /// <summary/>
        public static readonly Tag COMMENTS_ON_THE_SCHEDULED_PROCEDURE_STEP = new Tag(0x0040, 0x0400);
        /// <summary/>
        public static readonly Tag REQUESTED_PROCEDURE_ID = new Tag(0x0040, 0x1001);
        /// <summary/>
        public static readonly Tag REASON_FOR_THE_REQUESTED_PROCEDURE = new Tag(0x0040, 0x1002);
        /// <summary/>
        public static readonly Tag REQUESTED_PROCEDURE_PRIORITY = new Tag(0x0040, 0x1003);
        /// <summary/>
        public static readonly Tag PATIENT_TRANSPORT_ARRANGEMENTS = new Tag(0x0040, 0x1004);
        /// <summary/>
        public static readonly Tag REQUESTED_PROCEDURE_LOCATION = new Tag(0x0040, 0x1005);
        /// <summary/>
        public static readonly Tag PLACER_ORDER_NUMBER_PROCEDURE = new Tag(0x0040, 0x1006);
        /// <summary/>
        public static readonly Tag FILLER_ORDER_NUMBER_PROCEDURE = new Tag(0x0040, 0x1007);
        /// <summary/>
        public static readonly Tag CONFIDENTIALITY_CODE = new Tag(0x0040, 0x1008);
        /// <summary/>
        public static readonly Tag REPORTING_PRIORITY = new Tag(0x0040, 0x1009);
        /// <summary/>
		public static readonly Tag NAMES_OF_INTENDED_RECIPIENTS_OF_RESULTS = new Tag(0x0040, 0x1010);
        /// <summary/>
        public static readonly Tag INTENDED_RECIPIENTS_OF_RESULTS_IDENTIFICATION_SEQUENCE = new Tag(0x0040, 0x1011);
        /// <summary/>
        public static readonly Tag REQUESTED_PROCEDURE_COMMENTS = new Tag(0x0040, 0x1400);
        /// <summary/>
        public static readonly Tag REASON_FOR_THE_IMAGING_SERVICE_REQUEST = new Tag(0x0040, 0x2001);
        /// <summary/>
        public static readonly Tag ISSUE_DATE_OF_IMAGING_SERVICE_REQUEST = new Tag(0x0040, 0x2004);
        /// <summary/>
        public static readonly Tag ISSUE_TIME_OF_IMAGING_SERVICE_REQUEST = new Tag(0x0040, 0x2005);
        /// <summary/>
        public static readonly Tag PLACER_ORDER_NUMBER_IMAGING_SERVICE_REQUEST = new Tag(0x0040, 0x2006);
        /// <summary/>
        public static readonly Tag FILLER_ORDER_NUMBER_IMAGING_SERVICE_REQUEST = new Tag(0x0040, 0x2007);
        /// <summary/>
        public static readonly Tag ORDER_ENTERED_BY = new Tag(0x0040, 0x2008);
        /// <summary/>
        public static readonly Tag ORDER_ENTERERS_LOCATION = new Tag(0x0040, 0x2009);
        /// <summary/>
        public static readonly Tag ORDER_CALLBACK_PHONE_NUMBER = new Tag(0x0040, 0x2010);
        /// <summary/>
        public static readonly Tag IMAGING_SERVICE_REQUEST_COMMENTS = new Tag(0x0040, 0x2400);
        /// <summary/>
        public static readonly Tag CONFIDENTIALITY_CONSTRAINT_ON_PATIENT_DATA_DESCRIP = new Tag(0x0040, 0x3001);
        /// <summary/>
        public static readonly Tag GROUP_0050_LENGTH = new Tag(0x0050, 0x0000);
        /// <summary/>
        public static readonly Tag CALIBRATION_OBJECT = new Tag(0x0050, 0x0004);
        /// <summary/>
        public static readonly Tag DEVICE_SEQUENCE = new Tag(0x0050, 0x0010);
        /// <summary/>
        public static readonly Tag DEVICE_TYPE = new Tag(0x0050, 0x0012);
        /// <summary/>
        public static readonly Tag DEVICE_LENGTH = new Tag(0x0050, 0x0014);
        /// <summary/>
        public static readonly Tag DEVICE_DIAMETER = new Tag(0x0050, 0x0016);
        /// <summary/>
        public static readonly Tag DEVICE_DIAMETER_UNITS = new Tag(0x0050, 0x0017);
        /// <summary/>
        public static readonly Tag DEVICE_VOLUME = new Tag(0x0050, 0x0018);
        /// <summary/>
        public static readonly Tag INTER_MARKER_DISTANCE = new Tag(0x0050, 0x0019);
        /// <summary/>
        public static readonly Tag DEVICE_DESCRIPTION = new Tag(0x0050, 0x0020);
        /// <summary/>
        public static readonly Tag CODED_INTERVENTIONAL_DEVICE_SEQUENCE = new Tag(0x0050, 0x0030);
        /// <summary/>
        public static readonly Tag GROUP_0054_LENGTH = new Tag(0x0054, 0x0000);
        /// <summary/>
        public static readonly Tag ENERGY_WINDOW_VECTOR = new Tag(0x0054, 0x0010);
        /// <summary/>
        public static readonly Tag NUMBER_OF_ENERGY_WINDOWS = new Tag(0x0054, 0x0011);
        /// <summary/>
        public static readonly Tag ENERGY_WINDOW_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0012);
        /// <summary/>
        public static readonly Tag ENERGY_WINDOW_RANGE_SEQUENCE = new Tag(0x0054, 0x0013);
        /// <summary/>
        public static readonly Tag ENERGY_WINDOW_LOWER_LIMIT = new Tag(0x0054, 0x0014);
        /// <summary/>
        public static readonly Tag ENERGY_WINDOW_UPPER_LIMIT = new Tag(0x0054, 0x0015);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0016);
        /// <summary/>
        public static readonly Tag RESIDUAL_SYRINGE_COUNTS = new Tag(0x0054, 0x0017);
        /// <summary/>
        public static readonly Tag ENERGY_WINDOW_NAME = new Tag(0x0054, 0x0018);
        /// <summary/>
        public static readonly Tag DETECTOR_VECTOR = new Tag(0x0054, 0x0020);
        /// <summary/>
        public static readonly Tag NUMBER_OF_DETECTORS = new Tag(0x0054, 0x0021);
        /// <summary/>
        public static readonly Tag DETECTOR_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0022);
        /// <summary/>
        public static readonly Tag PHASE_VECTOR = new Tag(0x0054, 0x0030);
        /// <summary/>
        public static readonly Tag NUMBER_OF_PHASES = new Tag(0x0054, 0x0031);
        /// <summary/>
        public static readonly Tag PHASE_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0032);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FRAMES_IN_PHASE = new Tag(0x0054, 0x0033);
        /// <summary/>
        public static readonly Tag PHASE_DELAY = new Tag(0x0054, 0x0036);
        /// <summary/>
        public static readonly Tag PAUSE_BETWEEN_FRAMES = new Tag(0x0054, 0x0038);
        /// <summary/>
        public static readonly Tag ROTATION_VECTOR = new Tag(0x0054, 0x0050);
        /// <summary/>
        public static readonly Tag NUMBER_OF_ROTATIONS = new Tag(0x0054, 0x0051);
        /// <summary/>
        public static readonly Tag ROTATION_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0052);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FRAMES_IN_ROTATION = new Tag(0x0054, 0x0053);
        /// <summary/>
        public static readonly Tag R_R_INTERVAL_VECTOR = new Tag(0x0054, 0x0060);
        /// <summary/>
        public static readonly Tag NUMBER_OF_R_R_INTERVALS = new Tag(0x0054, 0x0061);
        /// <summary/>
        public static readonly Tag GATED_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0062);
        /// <summary/>
        public static readonly Tag DATA_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0063);
        /// <summary/>
        public static readonly Tag TIME_SLOT_VECTOR = new Tag(0x0054, 0x0070);
        /// <summary/>
        public static readonly Tag NUMBER_OF_TIME_SLOTS = new Tag(0x0054, 0x0071);
        /// <summary/>
        public static readonly Tag TIME_SLOT_INFORMATION_SEQUENCE = new Tag(0x0054, 0x0072);
        /// <summary/>
        public static readonly Tag TIME_SLOT_TIME = new Tag(0x0054, 0x0073);
        /// <summary/>
        public static readonly Tag SLICE_VECTOR = new Tag(0x0054, 0x0080);
        /// <summary/>
        public static readonly Tag NUMBER_OF_SLICES = new Tag(0x0054, 0x0081);
        /// <summary/>
        public static readonly Tag ANGULAR_VIEW_VECTOR = new Tag(0x0054, 0x0090);
        /// <summary/>
        public static readonly Tag TIME_SLICE_VECTOR = new Tag(0x0054, 0x0100);
        /// <summary/>
        public static readonly Tag NUMBER_OF_TIME_SLICES = new Tag(0x0054, 0x0101);
        /// <summary/>
        public static readonly Tag START_ANGLE = new Tag(0x0054, 0x0200);
        /// <summary/>
        public static readonly Tag TYPE_OF_DETECTOR_MOTION = new Tag(0x0054, 0x0202);
        /// <summary/>
        public static readonly Tag TRIGGER_VECTOR = new Tag(0x0054, 0x0210);
        /// <summary/>
        public static readonly Tag NUMBER_OF_TRIGGERS_IN_PHASE = new Tag(0x0054, 0x0211);
        /// <summary/>
        public static readonly Tag VIEW_CODE_SEQUENCE = new Tag(0x0054, 0x0220);
        /// <summary/>
        public static readonly Tag VIEW_ANGULATION_MODIFIER_CODE_SEQUENCE = new Tag(0x0054, 0x0222);
        /// <summary/>
        public static readonly Tag RADIONUCLIDE_CODE_SEQUENCE = new Tag(0x0054, 0x0300);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_ROUTE_CODE_SEQUENCE = new Tag(0x0054, 0x0302);
        /// <summary/>
        public static readonly Tag RADIOPHARMACEUTICAL_CODE_SEQUENCE = new Tag(0x0054, 0x0304);
        /// <summary/>
        public static readonly Tag CALIBRATION_DATA_SEQUENCE = new Tag(0x0054, 0x0306);
        /// <summary/>
        public static readonly Tag ENERGY_WINDOW_NUMBER = new Tag(0x0054, 0x0308);
        /// <summary/>
        public static readonly Tag IMAGE_ID = new Tag(0x0054, 0x0400);
        /// <summary/>
        public static readonly Tag PATIENT_ORIENTATION_CODE_SEQUENCE = new Tag(0x0054, 0x0410);
        /// <summary/>
        public static readonly Tag PATIENT_ORIENTATION_MODIFIER_CODE_SEQUENCE = new Tag(0x0054, 0x0412);
        /// <summary/>
        public static readonly Tag PATIENT_GANTRY_RELATIONSHIP_CODE_SEQUENCE = new Tag(0x0054, 0x0414);
        /// <summary/>
        public static readonly Tag SERIES_TYPE = new Tag(0x0054, 0x1000);
        /// <summary/>
        public static readonly Tag UNITS = new Tag(0x0054, 0x1001);
        /// <summary/>
        public static readonly Tag COUNTS_SOURCE = new Tag(0x0054, 0x1002);
        /// <summary/>
        public static readonly Tag REPROJECTION_METHOD = new Tag(0x0054, 0x1004);
        /// <summary/>
        public static readonly Tag RANDOMS_CORRECTION_METHOD = new Tag(0x0054, 0x1100);
        /// <summary/>
        public static readonly Tag ATTENUATION_CORRECTION_METHOD = new Tag(0x0054, 0x1101);
        /// <summary/>
        public static readonly Tag DECAY_CORRECTION = new Tag(0x0054, 0x1102);
        /// <summary/>
        public static readonly Tag RECONSTRUCTION_METHOD = new Tag(0x0054, 0x1103);
        /// <summary/>
        public static readonly Tag DETECTOR_LINES_OF_RESPONSE_USED = new Tag(0x0054, 0x1104);
        /// <summary/>
        public static readonly Tag SCATTER_CORRECTION_METHOD = new Tag(0x0054, 0x1105);
        /// <summary/>
        public static readonly Tag AXIAL_ACCEPTANCE = new Tag(0x0054, 0x1200);
        /// <summary/>
        public static readonly Tag AXIAL_MASH = new Tag(0x0054, 0x1201);
        /// <summary/>
        public static readonly Tag TRANSVERSE_MASH = new Tag(0x0054, 0x1202);
        /// <summary/>
        public static readonly Tag DETECTOR_ELEMENT_SIZE = new Tag(0x0054, 0x1203);
        /// <summary/>
        public static readonly Tag COINCIDENCE_WINDOW_WIDTH = new Tag(0x0054, 0x1210);
        /// <summary/>
        public static readonly Tag SECONDARY_COUNTS_TYPE = new Tag(0x0054, 0x1220);
        /// <summary/>
        public static readonly Tag FRAME_REFERENCE_TIME = new Tag(0x0054, 0x1300);
        /// <summary/>
        public static readonly Tag PRIMARY_PROMPTS_COUNTS_ACCUMULATED = new Tag(0x0054, 0x1310);
        /// <summary/>
        public static readonly Tag SECONDARY_COUNTS_ACCUMULATED = new Tag(0x0054, 0x1311);
        /// <summary/>
        public static readonly Tag SLICE_SENSITIVITY_FACTOR = new Tag(0x0054, 0x1320);
        /// <summary/>
        public static readonly Tag DECAY_FACTOR = new Tag(0x0054, 0x1321);
        /// <summary/>
        public static readonly Tag DOSE_CALIBRATION_FACTOR = new Tag(0x0054, 0x1322);
        /// <summary/>
        public static readonly Tag SCATTER_FRACTION_FACTOR = new Tag(0x0054, 0x1323);
        /// <summary/>
        public static readonly Tag DEAD_TIME_FACTOR = new Tag(0x0054, 0x1324);
        /// <summary/>
        public static readonly Tag IMAGE_INDEX = new Tag(0x0054, 0x1330);
        /// <summary/>
        public static readonly Tag COUNTS_INCLUDED = new Tag(0x0054, 0x1400);
        /// <summary/>
        public static readonly Tag DEAD_TIME_CORRECTION_FLAG = new Tag(0x0054, 0x1401);
        /// <summary/>
        public static readonly Tag GROUP_0088_LENGTH = new Tag(0x0088, 0x0000);
        /// <summary/>
        public static readonly Tag STORAGE_MEDIA_FILE_SET_ID = new Tag(0x0088, 0x0130);
        /// <summary/>
        public static readonly Tag STORAGE_MEDIA_FILE_SET_UID = new Tag(0x0088, 0x0140);
        /// <summary/>
        public static readonly Tag ICON_IMAGE = new Tag(0x0088, 0x0200);
        /// <summary/>
        public static readonly Tag TOPIC_TITLE = new Tag(0x0088, 0x0904);
        /// <summary/>
        public static readonly Tag TOPIC_SUBJECT = new Tag(0x0088, 0x0906);
        /// <summary/>
        public static readonly Tag TOPIC_AUTHOR = new Tag(0x0088, 0x0910);
        /// <summary/>
        public static readonly Tag TOPIC_KEY_WORDS = new Tag(0x0088, 0x0912);
        /// <summary/>
        public static readonly Tag GROUP_2000_LENGTH = new Tag(0x2000, 0x0000);
        /// <summary/>
        public static readonly Tag NUMBER_OF_COPIES = new Tag(0x2000, 0x0010);
        /// <summary/>
        public static readonly Tag PRINT_PRIORITY = new Tag(0x2000, 0x0020);
        /// <summary/>
        public static readonly Tag MEDIUM_TYPE = new Tag(0x2000, 0x0030);
        /// <summary/>
        public static readonly Tag FILM_DESTINATION = new Tag(0x2000, 0x0040);
        /// <summary/>
        public static readonly Tag FILM_SESSION_LABEL = new Tag(0x2000, 0x0050);
        /// <summary/>
        public static readonly Tag MEMORY_ALLOCATION = new Tag(0x2000, 0x0060);
        /// <summary/>
        public static readonly Tag COLOR_IMAGE_PRINTING_FLAG = new Tag(0x2000, 0x0062);
        /// <summary/>
        public static readonly Tag COLLATION_FLAG = new Tag(0x2000, 0x0063);
        /// <summary/>
        public static readonly Tag ANNOTATION_FLAG = new Tag(0x2000, 0x0065);
        /// <summary/>
        public static readonly Tag IMAGE_OVERLAY_FLAG = new Tag(0x2000, 0x0067);
        /// <summary/>
        public static readonly Tag PRESENTATION_LUT_FLAG = new Tag(0x2000, 0x0069);
        /// <summary/>
        public static readonly Tag IMAGE_BOX_PRESENTATION_LUT_FLAG = new Tag(0x2000, 0x006A);
        /// <summary/>
        public static readonly Tag REFERENCED_FILM_BOX_SEQUENCE = new Tag(0x2000, 0x0500);
        /// <summary/>
        public static readonly Tag REFERENCED_STORED_PRINT_SEQUENCE = new Tag(0x2000, 0x0510);
        /// <summary/>
        public static readonly Tag GROUP_2010_LENGTH = new Tag(0x2010, 0x0000);
        /// <summary/>
        public static readonly Tag IMAGE_DISPLAY_FORMAT = new Tag(0x2010, 0x0010);
        /// <summary/>
        public static readonly Tag ANNOTATION_DISPLAY_FORMAT_ID = new Tag(0x2010, 0x0030);
        /// <summary/>
        public static readonly Tag FILM_ORIENTATION = new Tag(0x2010, 0x0040);
        /// <summary/>
        public static readonly Tag FILM_SIZE_ID = new Tag(0x2010, 0x0050);
        /// <summary/>
        public static readonly Tag MAGNIFICATION_TYPE = new Tag(0x2010, 0x0060);
        /// <summary/>
        public static readonly Tag SMOOTHING_TYPE = new Tag(0x2010, 0x0080);
        /// <summary/>
        public static readonly Tag BORDER_DENSITY = new Tag(0x2010, 0x0100);
        /// <summary/>
        public static readonly Tag EMPTY_IMAGE_DENSITY = new Tag(0x2010, 0x0110);
        /// <summary/>
        public static readonly Tag MIN_DENSITY = new Tag(0x2010, 0x0120);
        /// <summary/>
        public static readonly Tag MAX_DENSITY = new Tag(0x2010, 0x0130);
        /// <summary/>
        public static readonly Tag TRIM = new Tag(0x2010, 0x0140);
        /// <summary/>
        public static readonly Tag CONFIGURATION_INFORMATION = new Tag(0x2010, 0x0150);
        /// <summary/>
        public static readonly Tag ILLUMINATION = new Tag(0x2010, 0x015E);
        /// <summary/>
        public static readonly Tag REFLECTED_AMBIENT_LIGHT = new Tag(0x2010, 0x0160);
        /// <summary/>
        public static readonly Tag REFERENCED_FILM_SESSION_SEQUENCE = new Tag(0x2010, 0x0500);
        /// <summary/>
        public static readonly Tag REFERENCED_IMAGE_BOX_SEQUENCE = new Tag(0x2010, 0x0510);
        /// <summary/>
        public static readonly Tag REFERENCED_BASIC_ANNOTATION_BOX_SEQUENCE = new Tag(0x2010, 0x0520);
        /// <summary/>
        public static readonly Tag GROUP_2020_LENGTH = new Tag(0x2020, 0x0000);
        /// <summary/>
        public static readonly Tag IMAGE_POSITION = new Tag(0x2020, 0x0010);
        /// <summary/>
        public static readonly Tag POLARITY = new Tag(0x2020, 0x0020);
        /// <summary/>
        public static readonly Tag REQUESTED_IMAGE_SIZE = new Tag(0x2020, 0x0030);
        /// <summary/>
        public static readonly Tag BASIC_GRAYSCALE_IMAGE_SEQUENCE = new Tag(0x2020, 0x0110);
        /// <summary/>
        public static readonly Tag BASIC_COLOR_IMAGE_SEQUENCE = new Tag(0x2020, 0x0111);
        /// <summary/>
        public static readonly Tag REFERENCED_IMAGE_OVERLAY_BOX_SEQUENCE = new Tag(0x2020, 0x0130);
        /// <summary/>
        public static readonly Tag REFERENCED_VOI_LUT_BOX_SEQUENCE = new Tag(0x2020, 0x0140);
        /// <summary/>
        public static readonly Tag GROUP_2030_LENGTH = new Tag(0x2030, 0x0000);
        /// <summary/>
        public static readonly Tag ANNOTATION_POSITION = new Tag(0x2030, 0x0010);
        /// <summary/>
        public static readonly Tag TEXT_STRING = new Tag(0x2030, 0x0020);
        /// <summary/>
        public static readonly Tag GROUP_2040_LENGTH = new Tag(0x2040, 0x0000);
        /// <summary/>
        public static readonly Tag REFERENCED_OVERLAY_PLANE_SEQUENCE = new Tag(0x2040, 0x0010);
        /// <summary/>
        public static readonly Tag REFERENCED_OVERLAY_PLANE_GROUPS = new Tag(0x2040, 0x0011);
        /// <summary/>
        public static readonly Tag OVERLAY_MAGNIFICATION_TYPE = new Tag(0x2040, 0x0060);
        /// <summary/>
        public static readonly Tag OVERLAY_SMOOTHING_TYPE = new Tag(0x2040, 0x0070);
        /// <summary/>
        public static readonly Tag OVERLAY_FOREGROUND_DENSITY = new Tag(0x2040, 0x0080);
        /// <summary/>
        public static readonly Tag OVERLAY_MODE = new Tag(0x2040, 0x0090);
        /// <summary/>
        public static readonly Tag THRESHOLD_DENSITY = new Tag(0x2040, 0x0100);
        /// <summary/>
        public static readonly Tag IMAGE_OVERLAY_BOX_REFERENCED_IMAGE_BOX_SEQUENCE = new Tag(0x2040, 0x0500);
        /// <summary/>
        public static readonly Tag PRESENTATION_LUT_SEQUENCE = new Tag(0x2050, 0x0010);
        /// <summary/>
        public static readonly Tag PRESENTATION_LUT_SHAPE = new Tag(0x2050, 0x0020);
        /// <summary/>
        public static readonly Tag REFERENCED_PRESENTATION_LUT_SEQUENCE = new Tag(0x2050, 0x0500);
        /// <summary/>
        public static readonly Tag GROUP_2100_LENGTH = new Tag(0x2100, 0x0000);
        /// <summary/>
        public static readonly Tag PRINT_JOB_ID = new Tag(0x2100, 0x0010);
        /// <summary/>
        public static readonly Tag EXECUTION_STATUS = new Tag(0x2100, 0x0020);
        /// <summary/>
        public static readonly Tag EXECUTION_STATUS_INFO = new Tag(0x2100, 0x0030);
        /// <summary/>
        public static readonly Tag CREATION_DATE = new Tag(0x2100, 0x0040);
        /// <summary/>
        public static readonly Tag CREATION_TIME = new Tag(0x2100, 0x0050);
        /// <summary/>
        public static readonly Tag ORIGINATOR = new Tag(0x2100, 0x0070);
        /// <summary/>
        public static readonly Tag DESTINATION_AE = new Tag(0x2100, 0x0140);
        /// <summary/>
        public static readonly Tag OWNER_ID = new Tag(0x2100, 0x0160);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FILMS = new Tag(0x2100, 0x0170);
        /// <summary/>
        public static readonly Tag REFERENCED_PRINT_JOB_SEQUENCE = new Tag(0x2100, 0x0500);
        /// <summary/>
        public static readonly Tag GROUP_2110_LENGTH = new Tag(0x2110, 0x0000);
        /// <summary/>
        public static readonly Tag PRINTER_STATUS = new Tag(0x2110, 0x0010);
        /// <summary/>
        public static readonly Tag PRINTER_STATUS_INFO = new Tag(0x2110, 0x0020);
        /// <summary/>
        public static readonly Tag PRINTER_NAME = new Tag(0x2110, 0x0030);
        /// <summary/>
        public static readonly Tag PRINT_QUEUE_ID = new Tag(0x2110, 0x0099);
        /// <summary/>
        public static readonly Tag QUEUE_STATUS = new Tag(0x2120, 0x0010);
        /// <summary/>
        public static readonly Tag PRINT_JOB_DESCRIPTION_SEQUENCE = new Tag(0x2120, 0x0050);
        /// <summary/>
        public static readonly Tag PRINT_MANAGEMENT_CAPABILITIES_SEQUENCE = new Tag(0x2130, 0x0010);
        /// <summary/>
        public static readonly Tag PRINTER_CHARACTERISTICS_SEQUENCE = new Tag(0x2130, 0x0015);
        /// <summary/>
        public static readonly Tag FLIM_BOX_CONTENT_SEQUENCE = new Tag(0x2130, 0x0030);
        /// <summary/>
        public static readonly Tag IMAGE_BOX_CONTENT_SEQUENCE = new Tag(0x2130, 0x0040);
        /// <summary/>
        public static readonly Tag ANNOTATION_CONTENT_SEQUENCE = new Tag(0x2130, 0x0050);
        /// <summary/>
        public static readonly Tag IMAGE_OVERLAY_BOX_CONTENT_SEQUENCE = new Tag(0x2130, 0x0060);
        /// <summary/>
        public static readonly Tag PRESENTATION_LUT_CONTENT_SEQUENCE = new Tag(0x2130, 0x0080);
        /// <summary/>
        public static readonly Tag PROPOSED_STUDY_SEQUENCE = new Tag(0x2130, 0x00A0);
        /// <summary/>
        public static readonly Tag ORIGINAL_IMAGE_SEQUENCE = new Tag(0x2130, 0x00C0);
        /// <summary/>
        public static readonly Tag GROUP_3002_LENGTH = new Tag(0x3002, 0x0000);
        /// <summary/>
        public static readonly Tag RT_IMAGE_LABEL = new Tag(0x3002, 0x0002);
        /// <summary/>
        public static readonly Tag RT_IMAGE_NAME = new Tag(0x3002, 0x0003);
        /// <summary/>
        public static readonly Tag RT_IMAGE_DESCRIPTION = new Tag(0x3002, 0x0004);
        /// <summary/>
        public static readonly Tag REPORTED_VALUES_ORIGIN = new Tag(0x3002, 0x000A);
        /// <summary/>
        public static readonly Tag RT_IMAGE_PLANE = new Tag(0x3002, 0x000C);
        /// <summary/>
        public static readonly Tag X_RAY_IMAGE_RECEPTOR_ANGLE = new Tag(0x3002, 0x000E);
        /// <summary/>
        public static readonly Tag RT_IMAGE_ORIENTATION = new Tag(0x3002, 0x0010);
        /// <summary/>
        public static readonly Tag IMAGE_PLANE_PIXEL_SPACING = new Tag(0x3002, 0x0011);
        /// <summary/>
        public static readonly Tag RT_IMAGE_POSITION = new Tag(0x3002, 0x0012);
        /// <summary/>
        public static readonly Tag RADIATION_MACHINE_NAME = new Tag(0x3002, 0x0020);
        /// <summary/>
        public static readonly Tag RADIATION_MACHINE_SAD = new Tag(0x3002, 0x0022);
        /// <summary/>
        public static readonly Tag RADIATION_MACHINE_SSD = new Tag(0x3002, 0x0024);
        /// <summary/>
        public static readonly Tag RT_IMAGE_SID = new Tag(0x3002, 0x0026);
        /// <summary/>
        public static readonly Tag SOURCE_OF_REFERENCE_OBJECT_DISTANCE = new Tag(0x3002, 0x0028);
        /// <summary/>
        public static readonly Tag FRACTION_NUMBER = new Tag(0x3002, 0x0029);
        /// <summary/>
        public static readonly Tag EXPOSURE_SEQUENCE = new Tag(0x3002, 0x0030);
        /// <summary/>
        public static readonly Tag METERSET_EXPOSURE = new Tag(0x3002, 0x0032);
        /// <summary/>
        public static readonly Tag GROUP_3004_LENGTH = new Tag(0x3004, 0x0000);
        /// <summary/>
        public static readonly Tag DVH_TYPE = new Tag(0x3004, 0x0001);
        /// <summary/>
        public static readonly Tag DOSE_UNITS = new Tag(0x3004, 0x0002);
        /// <summary/>
        public static readonly Tag DOSE_TYPE = new Tag(0x3004, 0x0004);
        /// <summary/>
        public static readonly Tag DOSE_COMMENT = new Tag(0x3004, 0x0006);
        /// <summary/>
        public static readonly Tag NORMALIZATION_POINT = new Tag(0x3004, 0x0008);
        /// <summary/>
        public static readonly Tag DOSE_SUMMATION_TYPE = new Tag(0x3004, 0x000A);
        /// <summary/>
        public static readonly Tag GRID_FRAME_OFFSET_VECTOR = new Tag(0x3004, 0x000C);
        /// <summary/>
        public static readonly Tag DOSE_GRID_SCALING = new Tag(0x3004, 0x000E);
        /// <summary/>
        public static readonly Tag RT_DOSE_ROI_SEQUENCE = new Tag(0x3004, 0x0010);
        /// <summary/>
        public static readonly Tag DOSE_VALUE = new Tag(0x3004, 0x0012);
        /// <summary/>
        public static readonly Tag DVH_NORMALIZATION_POINT = new Tag(0x3004, 0x0040);
        /// <summary/>
        public static readonly Tag DVH_NORMALIZATION_DOSE_VALUE = new Tag(0x3004, 0x0042);
        /// <summary/>
        public static readonly Tag DVH_SEQUENCE = new Tag(0x3004, 0x0050);
        /// <summary/>
        public static readonly Tag DVH_DOSE_SCALING = new Tag(0x3004, 0x0052);
        /// <summary/>
        public static readonly Tag DVH_VOLUME_UNITS = new Tag(0x3004, 0x0054);
        /// <summary/>
        public static readonly Tag DVH_NUMBER_OF_BINS = new Tag(0x3004, 0x0056);
        /// <summary/>
        public static readonly Tag DVH_DATA = new Tag(0x3004, 0x0058);
        /// <summary/>
        public static readonly Tag DVH_REFERENCED_ROI_SEQUENCE = new Tag(0x3004, 0x0060);
        /// <summary/>
        public static readonly Tag DVH_ROI_CONTRIBUTION_TYPE = new Tag(0x3004, 0x0062);
        /// <summary/>
        public static readonly Tag DVH_MINIMUM_DOSE = new Tag(0x3004, 0x0070);
        /// <summary/>
        public static readonly Tag DVH_MAXIMUM_DOSE = new Tag(0x3004, 0x0072);
        /// <summary/>
        public static readonly Tag DVH_MEAN_DOSE = new Tag(0x3004, 0x0074);
        /// <summary/>
        public static readonly Tag GROUP_3006_LENGTH = new Tag(0x3006, 0x0000);
        /// <summary/>
        public static readonly Tag STRUCTURE_SET_LABEL = new Tag(0x3006, 0x0002);
        /// <summary/>
        public static readonly Tag STRUCTURE_SET_NAME = new Tag(0x3006, 0x0004);
        /// <summary/>
        public static readonly Tag STRUCTURE_SET_DESCRIPTION = new Tag(0x3006, 0x0006);
        /// <summary/>
        public static readonly Tag STRUCTURE_SET_DATE = new Tag(0x3006, 0x0008);
        /// <summary/>
        public static readonly Tag STRUCTURE_SET_TIME = new Tag(0x3006, 0x0009);
        /// <summary/>
        public static readonly Tag REFERENCED_FRAME_OF_REFERENCE_SEQUENCE = new Tag(0x3006, 0x0010);
        /// <summary/>
        public static readonly Tag RT_REFERENCED_STUDY_SEQUENCE = new Tag(0x3006, 0x0012);
        /// <summary/>
        public static readonly Tag RT_REFERENCED_SERIES_SEQUENCE = new Tag(0x3006, 0x0014);
        /// <summary/>
        public static readonly Tag CONTOUR_IMAGE_SEQUENCE = new Tag(0x3006, 0x0016);
        /// <summary/>
        public static readonly Tag STRUCTURE_SET_ROI_SEQUENCE = new Tag(0x3006, 0x0020);
        /// <summary/>
        public static readonly Tag ROI_NUMBER = new Tag(0x3006, 0x0022);
        /// <summary/>
        public static readonly Tag REFERENCED_FRAME_OF_REFERENCE_UID = new Tag(0x3006, 0x0024);
        /// <summary/>
        public static readonly Tag ROI_NAME = new Tag(0x3006, 0x0026);
        /// <summary/>
        public static readonly Tag ROI_DESCRIPTION = new Tag(0x3006, 0x0028);
        /// <summary/>
        public static readonly Tag ROI_DISPLAY_COLOR = new Tag(0x3006, 0x002A);
        /// <summary/>
        public static readonly Tag ROI_VOLUME = new Tag(0x3006, 0x002C);
        /// <summary/>
        public static readonly Tag RT_RELATED_ROI_SEQUENCE = new Tag(0x3006, 0x0030);
        /// <summary/>
        public static readonly Tag RT_ROI_RELATIONSHIP = new Tag(0x3006, 0x0033);
        /// <summary/>
        public static readonly Tag ROI_GENERATION_ALGORITHM = new Tag(0x3006, 0x0036);
        /// <summary/>
        public static readonly Tag ROI_GENERATION_DESCRIPTION = new Tag(0x3006, 0x0038);
        /// <summary/>
        public static readonly Tag ROI_CONTOUR_SEQUENCE = new Tag(0x3006, 0x0039);
        /// <summary/>
        public static readonly Tag CONTOUR_SEQUENCE = new Tag(0x3006, 0x0040);
        /// <summary/>
        public static readonly Tag CONTOUR_GEOMETRIC_TYPE = new Tag(0x3006, 0x0042);
        /// <summary/>
        public static readonly Tag CONTOUR_SLAB_THICKNESS = new Tag(0x3006, 0x0044);
        /// <summary/>
        public static readonly Tag CONTOUR_OFFSET_VECTOR = new Tag(0x3006, 0x0045);
        /// <summary/>
        public static readonly Tag NUMBER_OF_CONTOUR_POINTS = new Tag(0x3006, 0x0046);
        /// <summary/>
        public static readonly Tag CONTOUR_DATA = new Tag(0x3006, 0x0050);
        /// <summary/>
        public static readonly Tag RT_ROI_OBERVATIONS_SEQUENCE = new Tag(0x3006, 0x0080);
        /// <summary/>
        public static readonly Tag OBSERVATIONS_NUMBER = new Tag(0x3006, 0x0082);
        /// <summary/>
        public static readonly Tag REFERENCED_ROI_NUMBER = new Tag(0x3006, 0x0084);
        /// <summary/>
        public static readonly Tag ROI_OBSERVATION_LABEL = new Tag(0x3006, 0x0085);
        /// <summary/>
        public static readonly Tag RT_ROI_IDENTIFICATION_CODE_SEQUENCE = new Tag(0x3006, 0x0086);
        /// <summary/>
        public static readonly Tag ROI_OBSERVATION_DESCRIPTION = new Tag(0x3006, 0x0088);
        /// <summary/>
        public static readonly Tag RELATED_RT_ROI_OBSERVATIONS_SEQUENCE = new Tag(0x3006, 0x00A0);
        /// <summary/>
        public static readonly Tag ROI_INTERPRETED_TYPE = new Tag(0x3006, 0x00A4);
        /// <summary/>
        public static readonly Tag ROI_INTERPRETER = new Tag(0x3006, 0x00A6);
        /// <summary/>
        public static readonly Tag ROI_PHYSICAL_PROPERTIES_SEQUENCE = new Tag(0x3006, 0x00B0);
        /// <summary/>
        public static readonly Tag ROI_PHYSICAL_PROPERTY = new Tag(0x3006, 0x00B2);
        /// <summary/>
        public static readonly Tag ROI_PHYSICAL_PROPERTY_VALUE = new Tag(0x3006, 0x00B4);
        /// <summary/>
        public static readonly Tag FRAME_OF_REFERENCE_RELATIONSHIP_SEQUENCE = new Tag(0x3006, 0x00C0);
        /// <summary/>
        public static readonly Tag RELATED_FRAME_OF_REFERENCE_UID = new Tag(0x3006, 0x00C2);
        /// <summary/>
        public static readonly Tag FRAME_OF_REFERENCE_TRANSFORMATION_TYPE = new Tag(0x3006, 0x00C4);
        /// <summary/>
        public static readonly Tag FRAME_OF_REFERENCE_TRANSFORMATION_MATRIX = new Tag(0x3006, 0x00C6);
        /// <summary/>
        public static readonly Tag FRAME_OF_REFERENCE_TRANSFORMATION_COMMENT = new Tag(0x3006, 0x00C8);
        /// <summary/>
        public static readonly Tag GROUP_300A_LENGTH = new Tag(0x300A, 0x0000);
        /// <summary/>
        public static readonly Tag RT_PLAN_LABEL = new Tag(0x300A, 0x0002);
        /// <summary/>
        public static readonly Tag RT_PLAN_NAME = new Tag(0x300A, 0x0003);
        /// <summary/>
        public static readonly Tag RT_PLAN_DESCRIPTION = new Tag(0x300A, 0x0004);
        /// <summary/>
        public static readonly Tag RT_PLAN_DATE = new Tag(0x300A, 0x0006);
        /// <summary/>
        public static readonly Tag RT_PLAN_TIME = new Tag(0x300A, 0x0007);
        /// <summary/>
        public static readonly Tag TREATMENT_PROTOCOLS = new Tag(0x300A, 0x0009);
        /// <summary/>
        public static readonly Tag TREATMENT_INTENT = new Tag(0x300A, 0x000A);
        /// <summary/>
        public static readonly Tag TREATMENT_SITES = new Tag(0x300A, 0x000B);
        /// <summary/>
        public static readonly Tag RT_PLAN_GEOMETRY = new Tag(0x300A, 0x000C);
        /// <summary/>
        public static readonly Tag PRESCRIPTION_DESCRIPTION = new Tag(0x300A, 0x000E);
        /// <summary/>
        public static readonly Tag DOSE_REFERENCE_SEQUENCE = new Tag(0x300A, 0x0010);
        /// <summary/>
        public static readonly Tag DOSE_REFERENCE_NUMBER = new Tag(0x300A, 0x0012);
        /// <summary/>
        public static readonly Tag DOSE_REFERENCE_STRUCTURE_TYPE = new Tag(0x300A, 0x0014);
        /// <summary/>
        public static readonly Tag DOSE_REFERENCE_DESCRIPTION = new Tag(0x300A, 0x0016);
        /// <summary/>
        public static readonly Tag DOSE_REFERENCE_POINT_COORDINATES = new Tag(0x300A, 0x0018);
        /// <summary/>
        public static readonly Tag NOMINAL_PRIOR_DOSE = new Tag(0x300A, 0x001A);
        /// <summary/>
        public static readonly Tag DOSE_REFERENCE_TYPE = new Tag(0x300A, 0x0020);
        /// <summary/>
        public static readonly Tag CONSTRAINT_WEIGHT = new Tag(0x300A, 0x0021);
        /// <summary/>
        public static readonly Tag DELIVERY_WARNING_DOSE = new Tag(0x300A, 0x0022);
        /// <summary/>
        public static readonly Tag DELIVERY_MAXIMUM_DOSE = new Tag(0x300A, 0x0023);
        /// <summary/>
        public static readonly Tag TARGET_MINIMUM_DOSE = new Tag(0x300A, 0x0025);
        /// <summary/>
        public static readonly Tag TARGET_PRESCIPTION_DOSE = new Tag(0x300A, 0x0026);
        /// <summary/>
        public static readonly Tag TARGET_MAXIMUM_DOSE = new Tag(0x300A, 0x0027);
        /// <summary/>
        public static readonly Tag TARGET_UNDERDOSE_VOLUME_FRACTION = new Tag(0x300A, 0x0028);
        /// <summary/>
        public static readonly Tag ORGAN_AT_RISK_FULL_VOLUME_DOSE = new Tag(0x300A, 0x002A);
        /// <summary/>
        public static readonly Tag ORGAN_AT_RISK_LIMIT_DOSE = new Tag(0x300A, 0x002B);
        /// <summary/>
        public static readonly Tag ORGAN_AT_RISK_MAXIMUM_DOSE = new Tag(0x300A, 0x002C);
        /// <summary/>
        public static readonly Tag ORGAN_AT_RISK_OVERDOSE_VOLUME_FRACTION = new Tag(0x300A, 0x002D);
        /// <summary/>
        public static readonly Tag TOLERANCE_TABLE_SEQUENCE = new Tag(0x300A, 0x0040);
        /// <summary/>
        public static readonly Tag TOLERANCE_TABLE_NUMBER = new Tag(0x300A, 0x0042);
        /// <summary/>
        public static readonly Tag TOLERANCE_TABLE_LABEL = new Tag(0x300A, 0x0043);
        /// <summary/>
        public static readonly Tag GANTRY_ANGLE_TOLERANCE = new Tag(0x300A, 0x0044);
        /// <summary/>
        public static readonly Tag BEAM_LIMITING_DEVICE_ANGLE_TOLERANCE = new Tag(0x300A, 0x0046);
        /// <summary/>
        public static readonly Tag BEAM_LIMITING_DEVICE_TOLERANCE_SEQUENCE = new Tag(0x300A, 0x0048);
        /// <summary/>
        public static readonly Tag BEAM_LIMITING_DEVICE_POSITION_TOLERANCE = new Tag(0x300A, 0x004A);
        /// <summary/>
        public static readonly Tag PATIENT_SUPPORT_ANGLE_TOLERANCE = new Tag(0x300A, 0x004C);
        /// <summary/>
        public static readonly Tag TABLE_TOP_ECCENTRIC_ANGLE_TOLERANCE = new Tag(0x300A, 0x004E);
        /// <summary/>
        public static readonly Tag TABLE_TOP_VERTICAL_POSITION_TOLERANCE = new Tag(0x300A, 0x0051);
        /// <summary/>
        public static readonly Tag TABLE_TOP_LONGITUDINAL_POSITION_TOLERANCE = new Tag(0x300A, 0x0052);
        /// <summary/>
        public static readonly Tag TABLE_TOP_LATERAL_POSITION_TOLERANCE = new Tag(0x300A, 0x0053);
        /// <summary/>
        public static readonly Tag RT_PLAN_RELATIONSHIP = new Tag(0x300A, 0x0055);
        /// <summary/>
        public static readonly Tag FRACTION_GROUP_SEQUENCE = new Tag(0x300A, 0x0070);
        /// <summary/>
        public static readonly Tag FRACTION_GROUP_NUMBER = new Tag(0x300A, 0x0071);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FRACTIONS_PLANNED = new Tag(0x300A, 0x0078);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FRACTIONS_PER_DAY = new Tag(0x300A, 0x0079);
        /// <summary/>
        public static readonly Tag REPEAT_FRACTION_CYCLE_LENGTH = new Tag(0x300A, 0x007A);
        /// <summary/>
        public static readonly Tag FRACTION_PATTERN = new Tag(0x300A, 0x007B);
        /// <summary/>
        public static readonly Tag NUMBER_OF_BEAMS = new Tag(0x300A, 0x0080);
        /// <summary/>
        public static readonly Tag BEAM_DOSE_SPECIFICATION_POINT = new Tag(0x300A, 0x0082);
        /// <summary/>
        public static readonly Tag BEAM_DOSE = new Tag(0x300A, 0x0084);
        /// <summary/>
        public static readonly Tag BEAM_METERSET = new Tag(0x300A, 0x0086);
        /// <summary/>
        public static readonly Tag NUMBER_OF_BRACHY_APPLICATION_SETUPS = new Tag(0x300A, 0x00A0);
        /// <summary/>
        public static readonly Tag BRACHY_APPLICATION_SETUP_DOSE_SPECIFICATION_POINT = new Tag(0x300A, 0x00A2);
        /// <summary/>
        public static readonly Tag BRACHY_APPLICATION_SETUP_DOSE = new Tag(0x300A, 0x00A4);
        /// <summary/>
        public static readonly Tag BEAM_SEQUENCE = new Tag(0x300A, 0x00B0);
        /// <summary/>
        public static readonly Tag TREATMENT_MACHINE_NAME = new Tag(0x300A, 0x00B2);
        /// <summary/>
        public static readonly Tag PRIMARY_DOSIMETER_UNIT = new Tag(0x300A, 0x00B3);
        /// <summary/>
        public static readonly Tag SOURCE_AXIS_DISTANCE = new Tag(0x300A, 0x00B4);
        /// <summary/>
        public static readonly Tag BEAM_LIMITING_DEVICE_SEQUENCE = new Tag(0x300A, 0x00B6);
        /// <summary/>
        public static readonly Tag RT_BEAM_LIMITING_DEVICE_TYPE = new Tag(0x300A, 0x00B8);
        /// <summary/>
        public static readonly Tag SOURCE_TO_BEAM_LIMITING_DEVICE_DISTANCE = new Tag(0x300A, 0x00BA);
        /// <summary/>
        public static readonly Tag NUMBER_OF_LEAF_JAW_PAIRS = new Tag(0x300A, 0x00BC);
        /// <summary/>
        public static readonly Tag LEAF_POSITION_BOUNDARIES = new Tag(0x300A, 0x00BE);
        /// <summary/>
        public static readonly Tag BEAM_NUMBER = new Tag(0x300A, 0x00C0);
        /// <summary/>
        public static readonly Tag BEAM_NAME = new Tag(0x300A, 0x00C2);
        /// <summary/>
        public static readonly Tag BEAM_DESCRIPTION = new Tag(0x300A, 0x00C3);
        /// <summary/>
        public static readonly Tag BEAM_TYPE = new Tag(0x300A, 0x00C4);
        /// <summary/>
        public static readonly Tag RADIATION_TYPE = new Tag(0x300A, 0x00C6);
        /// <summary/>
        public static readonly Tag REFERENCE_IMAGE_NUMBER = new Tag(0x300A, 0x00C8);
        /// <summary/>
        public static readonly Tag PLANNED_VERIFICATION_IMAGE_SEQUENCE = new Tag(0x300A, 0x00CA);
        /// <summary/>
        public static readonly Tag IMAGING_DEVICE_SPECIFIC_ACQUISITION_PARAMETERS = new Tag(0x300A, 0x00CC);
        /// <summary/>
        public static readonly Tag TREATMENT_DELIVERY_TYPE = new Tag(0x300A, 0x00CE);
        /// <summary/>
        public static readonly Tag NUMBER_OF_WEDGES = new Tag(0x300A, 0x00D0);
        /// <summary/>
        public static readonly Tag WEDGE_SEQUENCE = new Tag(0x300A, 0x00D1);
        /// <summary/>
        public static readonly Tag WEDGE_NUMBER = new Tag(0x300A, 0x00D2);
        /// <summary/>
        public static readonly Tag WEDGE_TYPE = new Tag(0x300A, 0x00D3);
        /// <summary/>
        public static readonly Tag WEDGE_ID = new Tag(0x300A, 0x00D4);
        /// <summary/>
        public static readonly Tag WEDGE_ANGLE = new Tag(0x300A, 0x00D5);
        /// <summary/>
        public static readonly Tag WEDGE_FACTOR = new Tag(0x300A, 0x00D6);
        /// <summary/>
        public static readonly Tag WEDGE_ORIENTATION = new Tag(0x300A, 0x00D8);
        /// <summary/>
        public static readonly Tag SOURCE_TO_WEDGE_TRAY_DISTANCE = new Tag(0x300A, 0x00DA);
        /// <summary/>
        public static readonly Tag NUMBER_OF_COMPENSATORS = new Tag(0x300A, 0x00E0);
        /// <summary/>
        public static readonly Tag MATERIAL_ID = new Tag(0x300A, 0x00E1);
        /// <summary/>
        public static readonly Tag TOTAL_COMPENSATOR_TRAY_FACTOR = new Tag(0x300A, 0x00E2);
        /// <summary/>
        public static readonly Tag COMPENSATOR_SEQUENCE = new Tag(0x300A, 0x00E3);
        /// <summary/>
        public static readonly Tag COMPENSATOR_NUMBER = new Tag(0x300A, 0x00E4);
        /// <summary/>
        public static readonly Tag COMPENSATOR_ID = new Tag(0x300A, 0x00E5);
        /// <summary/>
        public static readonly Tag SOURCE_TO_COMPENSATOR_TRAY_DISTANCE = new Tag(0x300A, 0x00E6);
        /// <summary/>
        public static readonly Tag COMPENSATOR_ROWS = new Tag(0x300A, 0x00E7);
        /// <summary/>
        public static readonly Tag COMPENSATOR_COLUMNS = new Tag(0x300A, 0x00E8);
        /// <summary/>
        public static readonly Tag COMPENSATOR_PIXEL_SPACING = new Tag(0x300A, 0x00E9);
        /// <summary/>
        public static readonly Tag COMPENSATOR_POSITION = new Tag(0x300A, 0x00EA);
        /// <summary/>
        public static readonly Tag COMPENSATOR_TRANSMISSION_DATA = new Tag(0x300A, 0x00EB);
        /// <summary/>
        public static readonly Tag COMPENSATOR_THICKNESS_DATA = new Tag(0x300A, 0x00EC);
        /// <summary/>
        public static readonly Tag NUMBER_OF_BOLI = new Tag(0x300A, 0x00ED);
        /// <summary/>
        public static readonly Tag NUMBER_OF_BLOCKS = new Tag(0x300A, 0x00F0);
        /// <summary/>
        public static readonly Tag TOTAL_BLOCK_TRAY_FACTOR = new Tag(0x300A, 0x00F2);
        /// <summary/>
        public static readonly Tag BLOCK_SEQUENCE = new Tag(0x300A, 0x00F4);
        /// <summary/>
        public static readonly Tag BLOCK_TRAY_ID = new Tag(0x300A, 0x00F5);
        /// <summary/>
        public static readonly Tag SOURCE_TO_BLOCK_TRAY_DISTANCE = new Tag(0x300A, 0x00F6);
        /// <summary/>
        public static readonly Tag BLOCK_TYPE = new Tag(0x300A, 0x00F8);
        /// <summary/>
        public static readonly Tag BLOCK_DIVERGENCE = new Tag(0x300A, 0x00FA);
        /// <summary/>
        public static readonly Tag BLOCK_NUMBER = new Tag(0x300A, 0x00FC);
        /// <summary/>
        public static readonly Tag BLOCK_NAME = new Tag(0x300A, 0x00FE);
        /// <summary/>
        public static readonly Tag BLOCK_THICKNESS = new Tag(0x300A, 0x0100);
        /// <summary/>
        public static readonly Tag BLOCK_TRANSMISSION = new Tag(0x300A, 0x0102);
        /// <summary/>
        public static readonly Tag BLOCK_NUMBER_OF_POINTS = new Tag(0x300A, 0x0104);
        /// <summary/>
        public static readonly Tag BLOCK_DATA = new Tag(0x300A, 0x0106);
        /// <summary/>
        public static readonly Tag APPLICATOR_SEQUENCE = new Tag(0x300A, 0x0107);
        /// <summary/>
        public static readonly Tag APPLICATOR_ID = new Tag(0x300A, 0x0108);
        /// <summary/>
        public static readonly Tag APPLICATOR_TYPE = new Tag(0x300A, 0x0109);
        /// <summary/>
        public static readonly Tag APPLICATOR_DESCRIPTION = new Tag(0x300A, 0x010A);
        /// <summary/>
        public static readonly Tag CUMULATIVE_DOSE_REFERENCE_COEFFICIENT = new Tag(0x300A, 0x010C);
        /// <summary/>
        public static readonly Tag FINAL_CUMULATIVE_METERSET_WEIGHT = new Tag(0x300A, 0x010E);
        /// <summary/>
        public static readonly Tag NUMBER_OF_CONTROL_POINTS = new Tag(0x300A, 0x0110);
        /// <summary/>
        public static readonly Tag CONTROL_POINT_SEQUENCE = new Tag(0x300A, 0x0111);
        /// <summary/>
        public static readonly Tag CONTROL_POINT_INDEX = new Tag(0x300A, 0x0112);
        /// <summary/>
        public static readonly Tag NOMINAL_BEAM_ENERGY = new Tag(0x300A, 0x0114);
        /// <summary/>
        public static readonly Tag DOSE_RATE_SET = new Tag(0x300A, 0x0115);
        /// <summary/>
        public static readonly Tag WEDGE_POSITION_SEQUENCE = new Tag(0x300A, 0x0116);
        /// <summary/>
        public static readonly Tag WEDGE_POSITION = new Tag(0x300A, 0x0118);
        /// <summary/>
        public static readonly Tag BEAM_LIMITING_DEVICE_POSITION_SEQUENCE = new Tag(0x300A, 0x011A);
        /// <summary/>
        public static readonly Tag LEAF_JAW_POSITIONS = new Tag(0x300A, 0x011C);
        /// <summary/>
        public static readonly Tag GANTRY_ANGLE = new Tag(0x300A, 0x011E);
        /// <summary/>
        public static readonly Tag GANTRY_ROTATION_DIRECTION = new Tag(0x300A, 0x011F);
        /// <summary/>
        public static readonly Tag BEAM_LIMITING_DEVICE_ANGLE = new Tag(0x300A, 0x0120);
        /// <summary/>
        public static readonly Tag BEAM_LIMITING_DEVICE_ROTATION_DIRECTION = new Tag(0x300A, 0x0121);
        /// <summary/>
        public static readonly Tag PATIENT_SUPPORT_ANGLE = new Tag(0x300A, 0x0122);
        /// <summary/>
        public static readonly Tag PATIENT_SUPPORT_ROTATION_DIRECTION = new Tag(0x300A, 0x0123);
        /// <summary/>
        public static readonly Tag TABLE_TOP_ECCENTRIC_AXIS_DISTANCE = new Tag(0x300A, 0x0124);
        /// <summary/>
        public static readonly Tag TABLE_TOP_ECCENTRIC_ANGLE = new Tag(0x300A, 0x0125);
        /// <summary/>
        public static readonly Tag TABLE_TOP_ECCENTRIC_ROTATION_DIRECTION = new Tag(0x300A, 0x0126);
        /// <summary/>
        public static readonly Tag TABLE_TOP_VERTICAL_POSITION = new Tag(0x300A, 0x0128);
        /// <summary/>
        public static readonly Tag TABLE_TOP_LONGITUDINAL_POSITION = new Tag(0x300A, 0x0129);
        /// <summary/>
        public static readonly Tag TABLE_TOP_LATERAL_POSITION = new Tag(0x300A, 0x012A);
        /// <summary/>
        public static readonly Tag ISOCENTER_POSITION = new Tag(0x300A, 0x012C);
        /// <summary/>
        public static readonly Tag SURFACE_ENTRY_POINT = new Tag(0x300A, 0x012E);
        /// <summary/>
        public static readonly Tag SOURCE_TO_SURFACE_DISTANCE = new Tag(0x300A, 0x0130);
        /// <summary/>
        public static readonly Tag CUMULATIVE_METERSET_WEIGHT = new Tag(0x300A, 0x0134);
        /// <summary/>
        public static readonly Tag PATIENT_SETUP_SEQUENCE = new Tag(0x300A, 0x0180);
        /// <summary/>
        public static readonly Tag PATIENT_SETUP_NUMBER = new Tag(0x300A, 0x0182);
        /// <summary/>
        public static readonly Tag PATIENT_ADDITIONAL_POSITION = new Tag(0x300A, 0x0184);
        /// <summary/>
        public static readonly Tag FIXATION_DEVICE_SEQUENCE = new Tag(0x300A, 0x0190);
        /// <summary/>
        public static readonly Tag FIXATION_DEVICE_TYPE = new Tag(0x300A, 0x0192);
        /// <summary/>
        public static readonly Tag FIXATION_DEVICE_LABEL = new Tag(0x300A, 0x0194);
        /// <summary/>
        public static readonly Tag FIXATION_DEVICE_DESCRIPTION = new Tag(0x300A, 0x0196);
        /// <summary/>
        public static readonly Tag FIXATION_DEVICE_POSITION = new Tag(0x300A, 0x0198);
        /// <summary/>
        public static readonly Tag SHIELDING_DEVICE_SEQUENCE = new Tag(0x300A, 0x01A0);
        /// <summary/>
        public static readonly Tag SHIELDING_DEVICE_TYPE = new Tag(0x300A, 0x01A2);
        /// <summary/>
        public static readonly Tag SHIELDING_DEVICE_LABEL = new Tag(0x300A, 0x01A4);
        /// <summary/>
        public static readonly Tag SHIELDING_DEVICE_DESCRIPTION = new Tag(0x300A, 0x01A6);
        /// <summary/>
        public static readonly Tag SHIELDING_DEVICE_POSITION = new Tag(0x300A, 0x01A8);
        /// <summary/>
        public static readonly Tag SETUP_TECHNIQUE = new Tag(0x300A, 0x01B0);
        /// <summary/>
        public static readonly Tag SETUP_TECHNIQUE_DESCRIPTION = new Tag(0x300A, 0x01B2);
        /// <summary/>
        public static readonly Tag SETUP_DEVICE_SEQUENCE = new Tag(0x300A, 0x01B4);
        /// <summary/>
        public static readonly Tag SETUP_DEVICE_TYPE = new Tag(0x300A, 0x01B6);
        /// <summary/>
        public static readonly Tag SETUP_DEVICE_LABEL = new Tag(0x300A, 0x01B8);
        /// <summary/>
        public static readonly Tag SETUP_DEVICE_DESCRIPTION = new Tag(0x300A, 0x01BA);
        /// <summary/>
        public static readonly Tag SETUP_DEVICE_PARAMETER = new Tag(0x300A, 0x01BC);
        /// <summary/>
        public static readonly Tag SETUP_REFERENCE_DESCRIPTION = new Tag(0x300A, 0x01D0);
        /// <summary/>
        public static readonly Tag TABLE_TOP_VERTICAL_SETUP_DISPLACEMENT = new Tag(0x300A, 0x01D2);
        /// <summary/>
        public static readonly Tag TABLE_TOP_LONGITUDINAL_SETUP_DISPLACEMENT = new Tag(0x300A, 0x01D4);
        /// <summary/>
        public static readonly Tag TABLE_TOP_LATERAL_SETUP_DISPLACEMENT = new Tag(0x300A, 0x01D6);
        /// <summary/>
        public static readonly Tag BRACHY_TREATMENT_TECHNIQUE = new Tag(0x300A, 0x0200);
        /// <summary/>
        public static readonly Tag BRACHY_TREATMENT_TYPE = new Tag(0x300A, 0x0202);
        /// <summary/>
        public static readonly Tag TREATMENT_MACHINE_SEQUENCE = new Tag(0x300A, 0x0206);
        /// <summary/>
        public static readonly Tag SOURCE_SEQUENCE = new Tag(0x300A, 0x0210);
        /// <summary/>
        public static readonly Tag SOURCE_NUMBER = new Tag(0x300A, 0x0212);
        /// <summary/>
        public static readonly Tag SOURCE_TYPE = new Tag(0x300A, 0x0214);
        /// <summary/>
        public static readonly Tag SOURCE_MANUFACTURER = new Tag(0x300A, 0x0216);
        /// <summary/>
        public static readonly Tag ACTIVE_SOURCE_DIAMETER = new Tag(0x300A, 0x0218);
        /// <summary/>
        public static readonly Tag ACTIVE_SOURCE_LENGTH = new Tag(0x300A, 0x021A);
        /// <summary/>
        public static readonly Tag SOURCE_ENCAPSULATION_NOMINAL_THICKNESS = new Tag(0x300A, 0x0222);
        /// <summary/>
        public static readonly Tag SOURCE_ENCAPSULATION_NOMINAL_TRANSMISSION = new Tag(0x300A, 0x0224);
        /// <summary/>
        public static readonly Tag SOURCE_ISOTOPE_NAME = new Tag(0x300A, 0x0226);
        /// <summary/>
        public static readonly Tag SOURCE_ISOTOPE_HALF_LIFE = new Tag(0x300A, 0x0228);
        /// <summary/>
        public static readonly Tag REFERENCE_AIR_KERMA_RATE = new Tag(0x300A, 0x022A);
        /// <summary/>
        public static readonly Tag AIR_KERMA_RATE_REFERENCE_DATE = new Tag(0x300A, 0x022C);
        /// <summary/>
        public static readonly Tag AIR_KERMA_RATE_REFERENCE_TIME = new Tag(0x300A, 0x022E);
        /// <summary/>
        public static readonly Tag APPLICATOIN_SETUP_SEQUENCE = new Tag(0x300A, 0x0230);
        /// <summary/>
        public static readonly Tag APPLICATION_SETUP_TYPE = new Tag(0x300A, 0x0232);
        /// <summary/>
        public static readonly Tag APPLICATION_SETUP_NUMBER = new Tag(0x300A, 0x0234);
        /// <summary/>
        public static readonly Tag APPLICATION_SETUP_NAME = new Tag(0x300A, 0x0236);
        /// <summary/>
        public static readonly Tag APPLICATION_SETUP_MANUFACTURER = new Tag(0x300A, 0x0238);
        /// <summary/>
        public static readonly Tag TEMPLATE_NUMBER = new Tag(0x300A, 0x0240);
        /// <summary/>
        public static readonly Tag TEMPLATE_TYPE = new Tag(0x300A, 0x0242);
        /// <summary/>
        public static readonly Tag TEMPLATE_NAME = new Tag(0x300A, 0x0244);
        /// <summary/>
        public static readonly Tag TOTAL_REFERENCE_AIR_KERMA = new Tag(0x300A, 0x0250);
        /// <summary/>
        public static readonly Tag BRACHY_ACCESSORY_DEVICE_SEQUENCE = new Tag(0x300A, 0x0260);
        /// <summary/>
        public static readonly Tag BRACHY_ACCESSORY_DEVICE_NUMBER = new Tag(0x300A, 0x0262);
        /// <summary/>
        public static readonly Tag BRACHY_ACCESSORY_DEVICE_ID = new Tag(0x300A, 0x0263);
        /// <summary/>
        public static readonly Tag BRACHY_ACCESSORY_DEVICE_TYPE = new Tag(0x300A, 0x0264);
        /// <summary/>
        public static readonly Tag BRACHY_ACCESSORY_DEVICE_NAME = new Tag(0x300A, 0x0266);
        /// <summary/>
        public static readonly Tag BRACHY_ACCESSORY_DEVICE_NOMINAL_THICKNESS = new Tag(0x300A, 0x026A);
        /// <summary/>
        public static readonly Tag BRACHY_ACCESSORY_DEVICE_NOMINAL_TRANSMISSION = new Tag(0x300A, 0x026C);
        /// <summary/>
        public static readonly Tag CHANNEL_SEQUENCE = new Tag(0x300A, 0x0280);
        /// <summary/>
        public static readonly Tag CHANNEL_NUMBER = new Tag(0x300A, 0x0282);
        /// <summary/>
        public static readonly Tag CHANNEL_LENGTH = new Tag(0x300A, 0x0284);
        /// <summary/>
        public static readonly Tag CHANNEL_TOTAL_TIME = new Tag(0x300A, 0x0286);
        /// <summary/>
        public static readonly Tag SOURCE_MOVEMENT_TYPE = new Tag(0x300A, 0x0288);
        /// <summary/>
        public static readonly Tag NUMBER_OF_PULSES = new Tag(0x300A, 0x028A);
        /// <summary/>
        public static readonly Tag PULSE_REPETITION_INTERVAL = new Tag(0x300A, 0x028C);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_NUMBER = new Tag(0x300A, 0x0290);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_ID = new Tag(0x300A, 0x0291);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_TYPE = new Tag(0x300A, 0x0292);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_NAME = new Tag(0x300A, 0x0294);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_LENGTH = new Tag(0x300A, 0x0296);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_MANUFACTURER = new Tag(0x300A, 0x0298);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_WALL_NOMINAL_THICKNESS = new Tag(0x300A, 0x029C);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_WALL_NOMINAL_TRANSMISSION = new Tag(0x300A, 0x029E);
        /// <summary/>
        public static readonly Tag SOURCE_APPLICATOR_STEP_SIZE = new Tag(0x300A, 0x02A0);
        /// <summary/>
        public static readonly Tag TRANSFER_TUBE_NUMBER = new Tag(0x300A, 0x02A2);
        /// <summary/>
        public static readonly Tag TRANSFER_TUBE_LENGTH = new Tag(0x300A, 0x02A4);
        /// <summary/>
        public static readonly Tag CHANNEL_SHIELD_SEQUENCE = new Tag(0x300A, 0x02B0);
        /// <summary/>
        public static readonly Tag CHANNEL_SHIELD_NUMBER = new Tag(0x300A, 0x02B2);
        /// <summary/>
        public static readonly Tag CHANNEL_SHIELD_ID = new Tag(0x300A, 0x02B3);
        /// <summary/>
        public static readonly Tag CHANNEL_SHIELD_NAME = new Tag(0x300A, 0x02B4);
        /// <summary/>
        public static readonly Tag CHANNEL_SHIELD_NOMINAL_THICKNESS = new Tag(0x300A, 0x02B8);
        /// <summary/>
        public static readonly Tag CHANNEL_SHIELD_NOMINAL_TRANSMISSION = new Tag(0x300A, 0x02BA);
        /// <summary/>
        public static readonly Tag FINAL_CUMULATIVE_TIME_WEIGHT = new Tag(0x300A, 0x02C8);
        /// <summary/>
        public static readonly Tag BRACHY_CONTROL_POINT_SEQUENCE = new Tag(0x300A, 0x02D0);
        /// <summary/>
        public static readonly Tag CONTROL_POINT_RELATIVE_POSITION = new Tag(0x300A, 0x02D2);
        /// <summary/>
        public static readonly Tag CONTROL_POINT_3D_POSITION = new Tag(0x300A, 0x02D4);
        /// <summary/>
        public static readonly Tag CUMULATIVE_TIME_WEIGHT = new Tag(0x300A, 0x02D6);
        /// <summary/>
        public static readonly Tag GROUP_300C_LENGTH = new Tag(0x300C, 0x0000);
        /// <summary/>
        public static readonly Tag REFERENCED_RT_PLAN_SEQUENCE = new Tag(0x300C, 0x0002);
        /// <summary/>
        public static readonly Tag REFERENCED_BEAM_SEQUENCE = new Tag(0x300C, 0x0004);
        /// <summary/>
        public static readonly Tag REFERENCED_BEAM_NUMBER = new Tag(0x300C, 0x0006);
        /// <summary/>
        public static readonly Tag REFERENCED_REFERENCE_IMAGE_NUMBER = new Tag(0x300C, 0x0007);
        /// <summary/>
        public static readonly Tag START_CUMULATIVE_METERSET_WEIGHT = new Tag(0x300C, 0x0008);
        /// <summary/>
        public static readonly Tag END_CUMULATIVE_METERSET_WEIGHT = new Tag(0x300C, 0x0009);
        /// <summary/>
        public static readonly Tag REFERENCED_BRACHY_APPLICATION_SETUP_SEQUENCE = new Tag(0x300C, 0x000A);
        /// <summary/>
        public static readonly Tag REFERENCED_BRACHY_APPLICATION_SETUP_NUMBER = new Tag(0x300C, 0x000C);
        /// <summary/>
        public static readonly Tag REFERENCED_SOURCE_NUMBER = new Tag(0x300C, 0x000E);
        /// <summary/>
        public static readonly Tag REFERENCED_FRACTION_GROUP_SEQUENCE = new Tag(0x300C, 0x0020);
        /// <summary/>
        public static readonly Tag REFERENCED_FRACTION_GROUP_NUMBER = new Tag(0x300C, 0x0022);
        /// <summary/>
        public static readonly Tag REFERENCED_TREATMENT_RECORD_SEQUENCE = new Tag(0x300C, 0x0030);
        /// <summary/>
        public static readonly Tag REFERENCED_VERIFICATION_IMAGE_SEQUENCE = new Tag(0x300C, 0x0040);
        /// <summary/>
        public static readonly Tag REFERENCED_REFERENCE_IMAGE_SEQUENCE = new Tag(0x300C, 0x0042);
        /// <summary/>
        public static readonly Tag REFERENCED_DOSE_REFERENCE_SEQUENCE = new Tag(0x300C, 0x0050);
        /// <summary/>
        public static readonly Tag REFERENCED_DOSE_REFERENCE_NUMBER = new Tag(0x300C, 0x0051);
        /// <summary/>
        public static readonly Tag BRACHY_REFERENCED_DOSE_REFERENCE_SEQUENCE = new Tag(0x300C, 0x0055);
        /// <summary/>
        public static readonly Tag REFERENCED_STRUCTURE_SET_SEQUENCE = new Tag(0x300C, 0x0060);
        /// <summary/>
        public static readonly Tag REFERENCED_PATIENT_SETUP_NUMBER = new Tag(0x300C, 0x006A);
        /// <summary/>
        public static readonly Tag REFERENCED_DOSE_SEQUENCE = new Tag(0x300C, 0x0080);
        /// <summary/>
        public static readonly Tag REFERENCED_TOLERANCE_TABLE_NUMBER = new Tag(0x300C, 0x00A0);
        /// <summary/>
        public static readonly Tag REFERENCED_BOLUS_SEQUENCE = new Tag(0x300C, 0x00B0);
        /// <summary/>
        public static readonly Tag REFERENCED_WEDGE_NUMBER = new Tag(0x300C, 0x00C0);
        /// <summary/>
        public static readonly Tag REFERENCED_COMPENSATOR_NUMBER = new Tag(0x300C, 0x00D0);
        /// <summary/>
        public static readonly Tag REFERENCED_BLOCK_NUMBER = new Tag(0x300C, 0x00E0);
        /// <summary/>
        public static readonly Tag REFERENCED_CONTROL_POINT = new Tag(0x300C, 0x00F0);
        /// <summary/>
        public static readonly Tag GROUP_300E_LENGTH = new Tag(0x300E, 0x0000);
        /// <summary/>
        public static readonly Tag APPROVAL_STATUS = new Tag(0x300E, 0x0002);
        /// <summary/>
        public static readonly Tag REVIEW_DATE = new Tag(0x300E, 0x0004);
        /// <summary/>
        public static readonly Tag REVIEW_TIME = new Tag(0x300E, 0x0005);
        /// <summary/>
        public static readonly Tag REVIEWER_NAME = new Tag(0x300E, 0x0008);
        /// <summary/>
        public static readonly Tag GROUP_4008_LENGTH = new Tag(0x4008, 0x0000);
        /// <summary/>
        public static readonly Tag RESULTS_ID = new Tag(0x4008, 0x0040);
        /// <summary/>
        public static readonly Tag RESULTS_ID_ISSUER = new Tag(0x4008, 0x0042);
        /// <summary/>
        public static readonly Tag REFERENCED_INTERPRETATION_SEQUENCE = new Tag(0x4008, 0x0050);
        /// <summary/>
        public static readonly Tag INTERPRETATION_RECORDED_DATE = new Tag(0x4008, 0x0100);
        /// <summary/>
        public static readonly Tag INTERPRETATION_RECORDED_TIME = new Tag(0x4008, 0x0101);
        /// <summary/>
        public static readonly Tag INTERPRETATION_RECORDER = new Tag(0x4008, 0x0102);
        /// <summary/>
        public static readonly Tag REFERENCE_TO_RECORDED_SOUND = new Tag(0x4008, 0x0103);
        /// <summary/>
        public static readonly Tag INTERPRETATION_TRANSCRIPTION_DATE = new Tag(0x4008, 0x0108);
        /// <summary/>
        public static readonly Tag INTERPRETATION_TRANSCRIPTION_TIME = new Tag(0x4008, 0x0109);
        /// <summary/>
        public static readonly Tag INTERPRETATION_TRANSCRIBER = new Tag(0x4008, 0x010A);
        /// <summary/>
        public static readonly Tag INTERPRETATION_TEXT = new Tag(0x4008, 0x010B);
        /// <summary/>
        public static readonly Tag INTERPRETATION_AUTHOR = new Tag(0x4008, 0x010C);
        /// <summary/>
        public static readonly Tag INTERPRETATION_APPROVER_SEQUENCE = new Tag(0x4008, 0x0111);
        /// <summary/>
        public static readonly Tag INTERPRETATION_APPROVAL_DATE = new Tag(0x4008, 0x0112);
        /// <summary/>
        public static readonly Tag INTERPRETATION_APPROVAL_TIME = new Tag(0x4008, 0x0113);
        /// <summary/>
        public static readonly Tag PHYSICIAN_APPROVING_INTERPRETATION = new Tag(0x4008, 0x0114);
        /// <summary/>
        public static readonly Tag INTERPRETATION_DIAGNOSIS_DESCRIPTION = new Tag(0x4008, 0x0115);
        /// <summary/>
        public static readonly Tag INTERPRETATION_DIAGNOSIS_CODE_SEQUENCE = new Tag(0x4008, 0x0117);
        /// <summary/>
        public static readonly Tag RESULTS_DISTRIBUTION_LIST_SEQUENCE = new Tag(0x4008, 0x0118);
        /// <summary/>
        public static readonly Tag DISTRIBUTION_NAME = new Tag(0x4008, 0x0119);
        /// <summary/>
        public static readonly Tag DISTRIBUTION_ADDRESS = new Tag(0x4008, 0x011A);
        /// <summary/>
        public static readonly Tag INTERPRETATION_ID = new Tag(0x4008, 0x0200);
        /// <summary/>
        public static readonly Tag INTERPRETATION_ID_ISSUER = new Tag(0x4008, 0x0202);
        /// <summary/>
        public static readonly Tag INTERPRETATION_TYPE_ID = new Tag(0x4008, 0x0210);
        /// <summary/>
        public static readonly Tag INTERPRETATION_STATUS_ID = new Tag(0x4008, 0x0212);
        /// <summary/>
        public static readonly Tag IMPRESSIONS = new Tag(0x4008, 0x0300);
        /// <summary/>
        public static readonly Tag RESULTS_COMMENTS = new Tag(0x4008, 0x4000);
        /// <summary/>
        public static readonly Tag GROUP_5000_LENGTH = new Tag(0x5000, 0x0000);
        /// <summary/>
        public static readonly Tag CURVE_DIMENSIONS = new Tag(0x5000, 0x0005);
        /// <summary/>
        public static readonly Tag NUMBER_OF_POINTS = new Tag(0x5000, 0x0010);
        /// <summary/>
        public static readonly Tag TYPE_OF_DATA = new Tag(0x5000, 0x0020);
        /// <summary/>
        public static readonly Tag CURVE_DESCRIPTION = new Tag(0x5000, 0x0022);
        /// <summary/>
        public static readonly Tag AXIS_UNITS = new Tag(0x5000, 0x0030);
        /// <summary/>
        public static readonly Tag AXIS_LABELS = new Tag(0x5000, 0x0040);
        /// <summary/>
        public static readonly Tag DATA_VALUE_REPRESENTATION = new Tag(0x5000, 0x0103);
        /// <summary/>
        public static readonly Tag MINIMUM_COORDINATE_VALUE = new Tag(0x5000, 0x0104);
        /// <summary/>
        public static readonly Tag MAXIMUM_COORDINATE_VALUE = new Tag(0x5000, 0x0105);
        /// <summary/>
        public static readonly Tag CURVE_RANGE = new Tag(0x5000, 0x0106);
        /// <summary/>
        public static readonly Tag CURVE_DATA_DESCRIPTOR = new Tag(0x5000, 0x0110);
        /// <summary/>
        public static readonly Tag COORDINATE_START_VALUE = new Tag(0x5000, 0x0112);
        /// <summary/>
        public static readonly Tag COORDINATE_STEP_VALUE = new Tag(0x5000, 0x0114);
        /// <summary/>
        public static readonly Tag AUDIO_TYPE = new Tag(0x5000, 0x2000);
        /// <summary/>
        public static readonly Tag AUDIO_SAMPLE_FORMAT = new Tag(0x5000, 0x2002);
        /// <summary/>
        public static readonly Tag NUMBER_OF_CHANNELS = new Tag(0x5000, 0x2004);
        /// <summary/>
        public static readonly Tag NUMBER_OF_SAMPLES = new Tag(0x5000, 0x2006);
        /// <summary/>
        public static readonly Tag SAMPLE_RATE = new Tag(0x5000, 0x2008);
        /// <summary/>
        public static readonly Tag TOTAL_TIME = new Tag(0x5000, 0x200A);
        /// <summary/>
        public static readonly Tag AUDIO_SAMPLE_DATA = new Tag(0x5000, 0x200C);
        /// <summary/>
        public static readonly Tag AUDIO_COMMENTS = new Tag(0x5000, 0x200E);
        /// <summary/>
        public static readonly Tag CURVE_LABEL = new Tag(0x5000, 0x2500);
        /// <summary/>
        public static readonly Tag CURVE_REFERENCED_OVERLAY_SEQUENCE = new Tag(0x5000, 0x2600);
        /// <summary/>
        public static readonly Tag REFERENCED_OVERLAY_GROUP = new Tag(0x5000, 0x2610);
        /// <summary/>
        public static readonly Tag CURVE_DATA = new Tag(0x5000, 0x3000);
        /// <summary/>
        public static readonly Tag WAVEFORM_DATA = new Tag(0x5400, 0x1010);
        /// <summary/>
        public static readonly Tag GROUP_6000_LENGTH = new Tag(0x6000, 0x0000);
        /// <summary/>
        public static readonly Tag OVERLAY_ROWS = new Tag(0x6000, 0x0010);
        /// <summary/>
        public static readonly Tag OVERLAY_COLUMNS = new Tag(0x6000, 0x0011);
        /// <summary/>
        public static readonly Tag OVERLAY_PLANES = new Tag(0x6000, 0x0012);
        /// <summary/>
        public static readonly Tag NUMBER_OF_FRAMES_IN_OVERLAY = new Tag(0x6000, 0x0015);
        /// <summary/>
        public static readonly Tag OVERLAY_DESCRIPTION = new Tag(0x6000, 0x0022);
        /// <summary/>
        public static readonly Tag OVERLAY_TYPE = new Tag(0x6000, 0x0040);
        /// <summary/>
        public static readonly Tag OVERLAY_SUBTYPE = new Tag(0x6000, 0x0045);
        /// <summary/>
        public static readonly Tag ORIGIN = new Tag(0x6000, 0x0050);
        /// <summary/>
        public static readonly Tag IMAGE_FRAME_ORIGIN = new Tag(0x6000, 0x0051);
        /// <summary/>
        public static readonly Tag OVERLAY_PLANE_ORIGIN = new Tag(0x6000, 0x0052);
        /// <summary/>
        public static readonly Tag OVERLAY_BITS_ALLOCATED = new Tag(0x6000, 0x0100);
        /// <summary/>
        public static readonly Tag BIT_POSITION = new Tag(0x6000, 0x0102);
        /// <summary/>
        public static readonly Tag OVERLAY_DESCRIPTOR_GRAY = new Tag(0x6000, 0x1100);
        /// <summary/>
        public static readonly Tag OVERLAY_DESCRIPTOR_RED = new Tag(0x6000, 0x1101);
        /// <summary/>
        public static readonly Tag OVERLAY_DESCRIPTOR_GREEN = new Tag(0x6000, 0x1102);
        /// <summary/>
        public static readonly Tag OVERLAY_DESCRIPTOR_BLUE = new Tag(0x6000, 0x1103);
        /// <summary/>
        public static readonly Tag OVERLAYS_GRAY = new Tag(0x6000, 0x1200);
        /// <summary/>
        public static readonly Tag OVERLAYS_RED = new Tag(0x6000, 0x1201);
        /// <summary/>
        public static readonly Tag OVERLAYS_GREEN = new Tag(0x6000, 0x1202);
        /// <summary/>
        public static readonly Tag OVERLAYS_BLUE = new Tag(0x6000, 0x1203);
        /// <summary/>
        public static readonly Tag ROI_AREA = new Tag(0x6000, 0x1301);
        /// <summary/>
        public static readonly Tag ROI_MEAN = new Tag(0x6000, 0x1302);
        /// <summary/>
        public static readonly Tag ROI_STANDARD_DEVIATION = new Tag(0x6000, 0x1303);
        /// <summary/>
        public static readonly Tag OVERLAY_LABEL = new Tag(0x6000, 0x1500);
        /// <summary/>
        public static readonly Tag OVERLAY_DATA = new Tag(0x6000, 0x3000);
        /// <summary/>
        public static readonly Tag GROUP_7FE0_LENGTH = new Tag(0x7FE0, 0x0000);
        /// <summary/>
        public static readonly Tag PIXEL_DATA = new Tag(0x7FE0, 0x0010);
        /// <summary/>
        public static readonly Tag DATASET_TRAILING_PADDING = new Tag(0xFFFC, 0xFFFC);
        /// <summary/>
        public static readonly Tag UNDEFINED = new Tag(0xFFFF, 0xFFFF);
        #endregion DataDictonary

        /// <summary>
        /// Default constructor is required for serialization!
        /// </summary>
        public Tag() { }

        /// <summary>
        /// Implicit conversion from System.Int32" to <see cref="Tag"/>
        /// </summary>
        /// <param name="value">System.Int32 value</param>
        /// <returns>Converted value</returns>
        public static implicit operator Tag(System.Int32 value)
        {
            System.Byte[] byteArray = System.BitConverter.GetBytes((System.Int32)value);
            if (System.BitConverter.IsLittleEndian)
                return new Tag(
                    System.BitConverter.ToUInt16(byteArray, 2),
                    System.BitConverter.ToUInt16(byteArray, 0));
            else
                return new Tag(
                    System.BitConverter.ToUInt16(byteArray, 0),
                    System.BitConverter.ToUInt16(byteArray, 2));
        }

        /// <summary>
        /// Implicit conversion from System.UInt32" to <see cref="Tag"/>
        /// </summary>
        /// <param name="value">System.UInt32 value</param>
        /// <returns>Converted value</returns>
        public static implicit operator Tag(System.UInt32 value)
        {
            System.Byte[] byteArray = System.BitConverter.GetBytes((System.UInt32)value);
            if (System.BitConverter.IsLittleEndian)
                return new Tag(
                    System.BitConverter.ToUInt16(byteArray, 2),
                    System.BitConverter.ToUInt16(byteArray, 0));
            else
                return new Tag(
                    System.BitConverter.ToUInt16(byteArray, 0),
                    System.BitConverter.ToUInt16(byteArray, 2));
        }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="groupNumber">Group number.</param>
        /// <param name="elementNumber">Element number.</param>
        public Tag(GROUP_NUMBER groupNumber, ELEMENT_NUMBER elementNumber)
        {
            this.GroupNumber = groupNumber;
            this.ElementNumber = elementNumber;
        }

        /// <summary>
        /// Group Hex number
        /// </summary>
        /// <remarks>
        /// The Byte[] is specified in Big Endian Format.
        /// Big Endian Format is used in definition of tags in DICOM standard.
        /// </remarks>
        public System.Byte[] GroupHex
        {
            get
            {
                byte[] byteArray = System.BitConverter.GetBytes(_Group);
                if (System.BitConverter.IsLittleEndian) System.Array.Reverse(byteArray);
                return byteArray;
            }
            set
            {
                if (System.BitConverter.IsLittleEndian) System.Array.Reverse(value);
                GROUP_NUMBER newValue = System.BitConverter.ToUInt16(value, 0);
                _Group = newValue;
            }
        }

        /// <summary>
        /// Group number as integer.
        /// </summary>
        public GROUP_NUMBER GroupNumber
        {
            get
            {
                return _Group;
            }
            set
            {
                _Group = value;
            }
        }
        private GROUP_NUMBER _Group = 0;

        /// <summary>
        /// Element Hex number
        /// </summary>
        /// <remarks>
        /// The Byte[] is specified in Big Endian Format.
        /// Big Endian Format is used in definition of tags in DICOM standard.
        /// </remarks>
        public System.Byte[] ElementHex
        {
            get
            {
                byte[] byteArray = System.BitConverter.GetBytes(_Element);
                if (System.BitConverter.IsLittleEndian) System.Array.Reverse(byteArray);
                return byteArray;
            }
            set
            {
                if (System.BitConverter.IsLittleEndian) System.Array.Reverse(value);
                ELEMENT_NUMBER newValue = System.BitConverter.ToUInt16(value, 0);
                _Element = newValue;
            }
        }

        /// <summary>
        /// Element number as integer.
        /// </summary>
        public ELEMENT_NUMBER ElementNumber
        {
            get { return _Element; }
            set { _Element = value; }
        }
        private ELEMENT_NUMBER _Element = 0;

        /// <summary>
        /// Obtains the <see cref="System.String"/> representation of this instance.
        /// </summary>
        /// <returns>The friendly name of the <see cref="Tag"/>.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Byte[] groupByteArray = System.BitConverter.GetBytes(this.GroupNumber);
            System.Byte[] elementByteArray = System.BitConverter.GetBytes(this.ElementNumber);
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
                "Group : {0}{1}\n",
                hexByteStr0,
                hexByteStr1);

            hexByteStr0 = elementByteArray[0].ToString("x");
            if (hexByteStr0.Length == 1) hexByteStr0 = "0" + hexByteStr0; // prepend with leading zero
            hexByteStr1 = elementByteArray[1].ToString("x");
            if (hexByteStr1.Length == 1) hexByteStr1 = "0" + hexByteStr1; // prepend with leading zero
            sb.AppendFormat(
                "Element : {0}{1}\n",
                hexByteStr0,
                hexByteStr1);
            return sb.ToString();
        }
    }

    /// <summary>
    /// Application Entity
    /// </summary>
    /// <remarks>
    /// A string of characters with leading and trailing
    /// spaces (20H) being non-significant. The value
    /// made of 16 spaces, meaning "no application
    /// name specified", shall not be used.
    /// </remarks>
    public class ApplicationEntity : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Age String
    /// </summary>
    /// <remarks>
    /// A string of characters with one of the following
    /// formats -- nnnD, nnnW, nnnM, nnnY; where
    /// nnn shall contain the number of days for D,
    /// weeks for W, months for M, or years for Y.
    /// Example: “018M” would represent an age of
    /// 18 months.
    /// </remarks>
    public class AgeString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Attribute Tag
    /// </summary>
    /// <remarks>
    /// Ordered pair of 16-bit unsigned integers that is
    /// the value of a Data Element Tag.
    /// Example: A Data Element Tag of (0018,00FF)
    /// would be encoded as a series of 4 bytes in a
    /// Little-Endian Transfer Syntax as
    /// 18H,00H,FFH,00H and in a Big-Endian
    /// Transfer Syntax as 00H,18H,00H,FFH.
    /// Note: The encoding of an AT value is exactly
    /// the same as the encoding of a Data
    /// Element Tag as defined in Section 7.
    /// </remarks>
    public class AttributeTag : DicomValueType
    {

        /// <summary>
        /// Underlying tag collection.
        /// </summary>
        public DvtkData.Collections.TagCollection Values = new DvtkData.Collections.TagCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Code String
    /// </summary>
    /// <remarks>
    /// A string of characters with leading or trailing
    /// spaces (20H) being non-significant.
    /// </remarks>
    public class CodeString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Date
    /// </summary>
    /// <remarks>
    /// A string of characters of the format yyyymmdd;
    /// where yyyy shall contain year, mm shall
    /// contain the month, and dd shall contain the
    /// day. This conforms to the ANSI HISPP
    /// MSDS Date common data type.
    /// Example: “19930822” would represent August 22, 1993.
    /// Notes:
    /// 1. For reasons of backward
    /// compatibility with versions of this
    /// standard prior to V3.0, it is
    /// recommended that implementations
    /// also support a string of characters of
    /// the format yyyy.mm.dd for this VR.
    /// 2. See also DT VR in this table.
    /// </remarks>
    public class Date : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        private const string NewFormat = "yyyyMMdd";
        private const string OldFormat = "yyyy.MM.dd";

        /// <summary>
        /// Formats used to encode the Date.
        /// </summary>
        public enum Format
        {
            /// <summary>
            /// New format yyyyMMdd
            /// </summary>
            New,
            /// <summary>
            /// Old format yyyy.MM.dd
            /// </summary>
            Old,
        }

        /// <summary>
        /// Add value to attribute.
        /// </summary>
        /// <param name="value">value to add.</param>
        /// <param name="format">format of the value.</param>
        public void AddValue(System.DateTime value, Format format)
        {
            string stringFormat;
            switch (format)
            {
                case Format.Old:
                    stringFormat = OldFormat;
                    break;
                case Format.New:
                default:
                    stringFormat = NewFormat;
                    break;
            }
            this.Values.Add(value.ToString(stringFormat));
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Date Time
    /// </summary>
    /// <remarks>
    /// <p>
    /// The Date Time common data type. Indicates a
    /// concatenated date-time ASCII string in the
    /// format: YYYYMMDDHHMMSS.FFFFFF&amp;ZZZZ
    /// The components of this string, from left to
    /// right, are YYYY = Year, MM = Month, DD =
    /// Day, HH = Hour, MM = Minute, SS = Second,
    /// FFFFFF = Fractional Second, &amp; = "+" or "-",
    /// and ZZZZ = Hours and Minutes of offset.
    /// &amp;ZZZZ is an optional suffix for plus/minus
    /// offset from Coordinated Universal Time. A
    /// component that is omitted from the string is
    /// termed a null component. Trailing null
    /// components of Date Time are ignored. Nontrailing
    /// null components are prohibited, given
    /// that the optional suffix is not considered as a
    /// component.
    /// </p>
    /// <p>
    /// Note: For reasons of backward compatibility
    /// with versions of this standard prior to
    /// V3.0, many existing DICOM Data
    /// Elements use the separate DA and TM
    /// VRs. Standard and Private Data
    /// Elements defined in the future should
    /// use DT, when appropriate, to be more
    /// compliant with ANSI HISPP MSDS.
    /// </p> 
    /// </remarks>
    public class DateTime : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        // DICOM "YYYYMMDDHHMMSS.FFFFFF&ZZZZ"
        private const string NewFormat = "yyyyMMddHHmmss.ffffffzz00";

        /// <summary>
        /// Formats used to encode the Date.
        /// </summary>
        public enum Format
        {
            /// <summary>
            /// New format yyyyMMddHHmmss.ffffffzz00
            /// </summary>
            New,
        }

        /// <summary>
        /// Add value to attribute.
        /// </summary>
        /// <param name="value">value to add.</param>
        /// <param name="format">format of the value.</param>
        public void AddValue(System.DateTime value, Format format)
        {
            string stringFormat;
            switch (format)
            {
                case Format.New:
                default:
                    stringFormat = NewFormat;
                    break;
            }
            this.Values.Add(value.ToString(stringFormat));
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Decimal String
    /// </summary>
    /// <remarks>
    /// A string of characters representing either a
    /// fixed point number or a floating point number.
    /// A fixed point number shall contain only the
    /// characters 0-9 with an optional leading "+" or
    /// "-" and an optional "." to mark the decimal
    /// point. A floating point number shall be
    /// conveyed as defined in ANSI X3.9, with an "E"
    /// or "e" to indicate the start of the exponent.
    /// Decimal Strings may be padded with leading
    /// or trailing spaces. Embedded spaces are not
    /// allowed.
    /// </remarks>
    public class DecimalString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Unlimited Text
    /// </summary>
    /// <remarks>
    /// A character string that may contain one or
    /// more paragraphs. It may contain the Graphic
    /// Character set and the Control Characters, CR,
    /// LF, FF, and ESC. It may be padded with
    /// trailing spaces, which may be ignored, but
    /// leading spaces are considered to be
    /// significant. Data Elements with this VR shall
    /// not be multi-valued and therefore character
    /// code 5CH (the BACKSLASH “\” in ISO-IR 6)
    /// may be used.
    /// </remarks>
    public class UnlimitedText : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> value.
        /// </summary>        
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        private string _Value = null;
        // TODO should this be string.Empty?;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<Value>{0}</Value>", DvtToXml.ConvertString(Value, true));

            // try to convert the string to Unicode - if possible
            if (_dicomUnicodeConverter != null)
            {
                String outString = DvtToXml.ConvertStringToXmlUnicode(_dicomUnicodeConverter, Value);
                if (outString != String.Empty)
                {
                    streamWriter.WriteLine("<Unicode>{0}</Unicode>", outString);
                }
            }

            return true;
        }
    }


    /// <summary>
    /// Universal Resource Identifier
    /// </summary>
    /// <remarks>
    /// A string of characters that identifies a URI or a URL as defined in [RFC3986]. 
    /// Leading spaces are not allowed. Trailing spaces shall be ignored. 
    /// Data Elements with this VR shall not be multi-valued.
    /// </remarks>
    public class UniversalResourceIdentifier : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> value.
        /// </summary>        
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        private string _Value = null;
        // TODO should this be string.Empty?;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<Value>{0}</Value>", DvtToXml.ConvertString(Value, true));

            // try to convert the string to Unicode - if possible
            if (_dicomUnicodeConverter != null)
            {
                String outString = DvtToXml.ConvertStringToXmlUnicode(_dicomUnicodeConverter, Value);
                if (outString != String.Empty)
                {
                    streamWriter.WriteLine("<Unicode>{0}</Unicode>", outString);
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Unlimited Characters
    /// </summary>
    /// <remarks>
    /// A character string that may be of unlimited length that may be padded with trailing spaces.
    /// The character code 5CH (the BACKSLASH "\" in ISO-IR 6) shall not be present, as it is used as the delimiter between values in multiple valued data elements.
    /// The string shall not have Control Characters except for ESC.
    /// </remarks>
    public class UnlimitedCharacters : DicomValueType
    {
        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }


    /// <summary>
    /// Unsigned Short
    /// </summary>
    /// <remarks>
    /// Unsigned binary integer 16 bits long.
    /// Represents integer n in the range:<br></br>
    /// 0 &lt;= n &lt; 2^16
    /// </remarks>
    public class UnsignedShort : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.UInt16"/> collection.
        /// </summary>
        public DvtkData.Collections.UInt16Collection Values = new DvtkData.Collections.UInt16Collection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Unsigned Long
    /// </summary>
    /// <remarks>
    /// Unsigned binary integer 32 bits long.
    /// Represents an integer n in the range:<br></br>
    /// 0 &lt;= n &lt; 2^32
    /// </remarks>
    public class UnsignedLong : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.UInt32"/> collection.
        /// </summary>
        public DvtkData.Collections.UInt32Collection Values = new DvtkData.Collections.UInt32Collection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Unique Identifier (UID)
    /// </summary>
    /// <remarks>
    /// A character string containing a UID that is
    /// used to uniquely identify a wide variety of
    /// items. The UID is a series of numeric
    /// components separated by the period "."
    /// character. If a Value Field containing one or
    /// more UIDs is an odd number of bytes in
    /// length, the Value Field shall be padded with a
    /// single trailing NULL (00H) character to ensure
    /// that the Value Field is an even number of
    /// bytes in length. See Section 9 and Annex B
    /// for a complete specification and examples.
    /// </remarks>
    public class UniqueIdentifier : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        private static byte _UidCount = 0;
        private const System.String _DvtUidRoot = "1.2.826.0.1.3680043.2.1545.1.2.1.7";
        /// <summary>
        /// Generates a Unique Identifier value.
        /// </summary>
        /// <remarks>
        /// <p>
        /// This value may be assigned to the list of value items.
        /// </p>
        /// </remarks>
        /// <returns>UID value item</returns>
        /// <example>
        /// <code>
        /// public static void UniqueIdentifierWithGeneratedUid()
        /// {
        ///     UniqueIdentifier uid = new UniqueIdentifier();
        ///     // ...
        ///     uid.Values.Add(UniqueIdentifier.GenerateUidValue());
        ///     // ...
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// public static void AddAttributeWithGeneratedUid()
        /// {
        ///     DataSet dataSet;
        ///     // ...
        ///     dataSet.AddAttribute(0x0010, 0x0010, VR.UI, UniqueIdentifier.GenerateUidValue());
        ///     // ...
        /// }
        /// </code>
        /// </example>
        public static System.String GenerateUidValue()
        {
            // fixed part
            string org_root = _DvtUidRoot;
            // variable part
            _UidCount += 2; // increase as even number
            // Ensure eveness! maybe needed for overflow!?
            if (_UidCount % 2 != 0) _UidCount++;
            System.DateTime now = System.DateTime.Now;
            string suffix =
                string.Format("{0}.{1}.{2}", now.Second, now.Millisecond, _UidCount);
            return string.Format("{0}.{1}", org_root, suffix);
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Time
    /// </summary>
    /// <remarks>
    /// A string of characters of the format
    /// hhmmss.frac; where hh contains hours (range
    /// "00" - "23"), mm contains minutes (range "00" -
    /// "59"), ss contains seconds (range "00" -
    /// "59"), and frac contains a fractional part of a
    /// second as small as 1 millionth of a second
    /// (range “000000” - “999999”). A 24 hour clock
    /// is assumed. Midnight can be represented by
    /// only “0000“ since “2400“ would violate the
    /// hour range. The string may be padded with
    /// trailing spaces. Leading and embedded
    /// spaces are not allowed. One or more of the
    /// components mm, ss, or frac may be
    /// unspecified as long as every component to the
    /// right of an unspecified component is also
    /// unspecified. If frac is unspecified the
    /// preceding “.” may not be included. Frac shall
    /// be held to six decimal places or less to ensure
    /// its format conforms to the ANSI HISPP MSDS
    /// Time common data type.
    /// 
    /// Examples:
    /// 1. “070907.0705 ” represents a time of
    /// 7 hours, 9 minutes and 7.0705
    /// seconds.
    /// 2. “1010” represents a time of 10 hours,
    /// and 10 minutes.
    /// 3. “021 ” is an invalid value.
    /// 
    /// Notes: 1. For reasons of backward
    /// compatibility with versions of this
    /// standard prior to V3.0, it is
    /// recommended that implementations
    /// also support a string of characters of
    /// the format hh:mm:ss.frac for this VR.
    /// 2. See also DT VR in this table.
    /// </remarks>
    public class Time : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        // DICOM "hhmmss.frac"
        private const string NewFormat = "HHmmss.ffffff";
        // DICOM "hh:mm:ss.frac"
        private const string OldFormat = "HH:mm:ss.ffffff";

        /// <summary>
        /// Formats used to encode the Date.
        /// </summary>
        public enum Format
        {
            /// <summary>
            /// New format HHmmss.ffffff
            /// </summary>
            New,
            /// <summary>
            /// Old format HH:mm:ss.ffffff
            /// </summary>
            Old,
        }

        /// <summary>
        /// Add value to attribute.
        /// </summary>
        /// <param name="value">value to add.</param>
        /// <param name="format">format of the value.</param>
        public void AddValue(System.DateTime value, Format format)
        {
            string stringFormat;
            switch (format)
            {
                case Format.Old:
                    stringFormat = OldFormat;
                    break;
                case Format.New:
                default:
                    stringFormat = NewFormat;
                    break;
            }
            this.Values.Add(value.ToString(stringFormat));
        }

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Short Text
    /// </summary>
    /// <remarks>
    /// A character string that may contain one or
    /// more paragraphs. It may contain the Graphic
    /// Character set and the Control Characters, CR,
    /// LF, FF, and ESC. It may be padded with
    /// trailing spaces, which may be ignored, but
    /// leading spaces are considered to be
    /// significant. Data Elements with this VR shall
    /// not be multi-valued and therefore character
    /// code 5CH (the BACKSLASH “\” in ISO-IR 6)
    /// may be used.
    /// </remarks>
    public class ShortText : DicomValueType
    {
        /// <summary>
        /// Underlying <see cref="System.String"/> value.
        /// </summary>        
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        private string _Value = null;
        // TODO should this be string.Empty?;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<Value>{0}</Value>", DvtToXml.ConvertString(Value, true));

            // try to convert the string to Unicode - if possible
            if (_dicomUnicodeConverter != null)
            {
                String outString = DvtToXml.ConvertStringToXmlUnicode(_dicomUnicodeConverter, Value);
                if (outString != String.Empty)
                {
                    streamWriter.WriteLine("<Unicode>{0}</Unicode>", outString);
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Signed Short
    /// </summary>
    /// <remarks>
    /// Signed binary integer 16 bits long in 2's
    /// complement form. Represents an integer n in
    /// the range:<br></br>
    /// -2^15 &lt;= n &lt;= (2^15 - 1)
    /// </remarks>
    public class SignedShort : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.Int16"/> collection.
        /// </summary>
        public DvtkData.Collections.Int16Collection Values = new DvtkData.Collections.Int16Collection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Signed Long
    /// </summary>
    /// <remarks>
    /// Signed binary integer 32 bits long in 2's
    /// complement form.
    /// Represents an integer, n, in the range:<br></br>
    /// -2^31 &lt;= n &lt;= (2^31 - 1)
    /// </remarks>
    public class SignedLong : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.Int32"/> collection.
        /// </summary>
        public DvtkData.Collections.Int32Collection Values = new DvtkData.Collections.Int32Collection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Short String
    /// </summary>
    /// <remarks>
    /// A character string that may be padded with
    /// leading and/or trailing spaces. The character
    /// code 05CH (the BACKSLASH “\” in ISO-IR 6)
    /// shall not be present, as it is used as the
    /// delimiter between values for multiple data
    /// elements. The string shall not have Control
    /// Characters except ESC.
    /// </remarks>
    public class ShortString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Person Name
    /// </summary>
    /// <remarks>
    /// A character string encoded using a 5
    /// component convention. The character code
    /// 5CH (the BACKSLASH “\” in ISO-IR 6) shall
    /// not be present, as it is used as the delimiter
    /// between values in multiple valued data
    /// elements. The string may be padded with
    /// trailing spaces. The five components in their
    /// order of occurrence are: family name complex,
    /// given name complex, middle name, name
    /// prefix, name suffix. Any of the five components
    /// may be an empty string. The component
    /// delimiter shall be the caret “^” character (5EH).
    /// Delimiters are required for interior null
    /// components. Trailing null components and
    /// their delimiters may be omitted. Multiple
    /// entries are permitted in each component and
    /// are encoded as natural text strings, in the
    /// format preferred by the named person. This
    /// conforms to the ANSI HISPP MSDS Person
    /// Name common data type.
    /// This group of five components is referred to as
    /// a Person Name component group.
    /// </remarks>
    public class PersonName : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Definition of the bit-pattern generated by DVTK for OB, OD, OW, OF and OV values.
    /// </summary>
    public class BitmapPatternParameters
    {
        /// <summary>
        /// Number of rows
        /// </summary>
        public System.UInt16 NumberOfRows = 512;
        /// <summary>
        /// Number of columns
        /// </summary>
        public System.UInt16 NumberOfColumns = 512;
        /// <summary>
        /// Start value.
        /// </summary>
        public System.UInt16 StartValue = 0;
        /// <summary>
        /// Value increment per row block.
        /// </summary>
        public System.UInt16 ValueIncrementPerRowBlock = 1;
        /// <summary>
        /// Value increment per column block.
        /// </summary>
        public System.UInt16 ValueIncrementPerColumnBlock = 1;
        /// <summary>
        /// Number of identical value rows.
        /// </summary>
        public System.UInt16 NumberOfIdenticalValueRows = 1;
        /// <summary>
        /// Number of identical value columns.
        /// </summary>
        public System.UInt16 NumberOfIdenticalValueColumns = 1;

        /// <summary>
        /// Default constructor.
        /// </summary>
		public BitmapPatternParameters() { }

        /// <summary>
        /// Specific constructor.
        /// </summary>
        /// <param name="numberOfRows"></param>
        /// <param name="numberOfColumns"></param>
        /// <param name="startValue"></param>
        /// <param name="valueIncrementPerRowBlock"></param>
        /// <param name="valueIncrementPerColumnBlock"></param>
        /// <param name="numberOfIdenticalValueRows"></param>
        /// <param name="numberOfIdenticalValueColumns"></param>
        public BitmapPatternParameters(
            System.UInt16 numberOfRows,
            System.UInt16 numberOfColumns,
            System.UInt16 startValue,
            System.UInt16 valueIncrementPerRowBlock,
            System.UInt16 valueIncrementPerColumnBlock,
            System.UInt16 numberOfIdenticalValueRows,
            System.UInt16 numberOfIdenticalValueColumns)
        {
            this.NumberOfRows = numberOfRows;
            this.NumberOfColumns = numberOfColumns;
            this.StartValue = startValue;
            this.ValueIncrementPerRowBlock = valueIncrementPerRowBlock;
            this.ValueIncrementPerColumnBlock = valueIncrementPerColumnBlock;
            this.NumberOfIdenticalValueRows = numberOfIdenticalValueRows;
            this.NumberOfIdenticalValueColumns = numberOfIdenticalValueColumns;
        }

        /// <summary>
        /// Default bit-pattern. Checker-pattern.
        /// </summary>
        public static readonly BitmapPatternParameters DefaultPattern =
            new BitmapPatternParameters();
    }

    /// <summary>
    /// Other Word String
    /// </summary>
    /// <remarks>
    /// A string of 16-bit words where the encoding of
    /// the contents is specified by the negotiated
    /// Transfer Syntax. OW is a VR which requires
    /// byte swapping within each word when
    /// changing between Little Endian and Big
    /// Endian byte ordering (see Section 7.3).
    /// </remarks>
	public class OtherWordString : DicomValueType
    {

        /// <summary>
        /// Underlying <c>FileName</c> <see cref="System.String"/> value or <c>BitMapPattern</c> <see cref="BitmapPatternParameters"/>.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item = null;
        // TODO should this be string.Empty?

        //        public System.Byte[] ByteArray
        //        {
        //            set { _Item = value; }
        //        }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName
        {
            get
            {
                return (string)_Item;
            }
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Bit pattern.
        /// </summary>
        public BitmapPatternParameters BitmapPattern
        {
            get { return (BitmapPatternParameters)_Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Property Compressed - is OW data compressed Y/N?
        /// </summary>
        public bool Compressed
        {
            get { return _compressed; }
            set { _compressed = value; }
        }
        private bool _compressed = false;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (Item != null)
            {
                if (Item is string)
                {
                    string fileName = (string)Item;
                    streamWriter.WriteLine("<Value>{0}</Value>", fileName);
                }
                else if (Item is BitmapPatternParameters)
                {
                    BitmapPatternParameters bitmapPattern = (BitmapPatternParameters)Item;
                    streamWriter.WriteLine("<BitmapPattern>");
                    streamWriter.WriteLine("<NumberOfRows>{0}</NumberOfRows>", bitmapPattern.NumberOfRows);
                    streamWriter.WriteLine("<NumberOfColumns>{0}</NumberOfColumns>", bitmapPattern.NumberOfColumns);
                    streamWriter.WriteLine("<StartValue>{0}</StartValue>", bitmapPattern.StartValue);
                    streamWriter.WriteLine("<ValueIncrementPerRowBlock>{0}</ValueIncrementPerRowBlock>", bitmapPattern.ValueIncrementPerRowBlock);
                    streamWriter.WriteLine("<ValueIncrementPerColumnBlock>{0}</ValueIncrementPerColumnBlock>", bitmapPattern.ValueIncrementPerColumnBlock);
                    streamWriter.WriteLine("<NumberOfIdenticalValueRows>{0}</NumberOfIdenticalValueRows>", bitmapPattern.NumberOfIdenticalValueRows);
                    streamWriter.WriteLine("<NumberOfIdenticalValueColumns>{0}</NumberOfIdenticalValueColumns>", bitmapPattern.NumberOfIdenticalValueColumns);
                    streamWriter.WriteLine("</BitmapPattern>");
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Other Float String
    /// </summary>
    /// <remarks>
    /// A string of 32-bit IEEE 754:1985 floating point
    /// words. OF is a VR which requires byte
    /// swapping within each 32-bit word when
    /// changing between Little Endian and Big
    /// Endian byte ordering (see Section 7.3).
    /// </remarks>
    public class OtherFloatString : DicomValueType
    {

        /// <summary>
        /// Underlying <c>FileName</c> <see cref="System.String"/> value or <c>BitMapPattern</c> <see cref="BitmapPatternParameters"/>.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item = null;
        // TODO should this be string.Empty?

        //        public System.Byte[] ByteArray
        //        {
        //            set { _Item = value; }
        //        }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName
        {
            get
            {
                return (string)_Item;
            }
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Bit pattern.
        /// </summary>
        public BitmapPatternParameters BitmapPattern
        {
            get { return (BitmapPatternParameters)_Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Property Compressed - is OF data compressed Y/N?
        /// </summary>
        public bool Compressed
        {
            get { return _compressed; }
            set { _compressed = value; }
        }
        private bool _compressed = false;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (Item != null)
            {
                if (Item is string)
                {
                    string fileName = (string)Item;
                    streamWriter.WriteLine("<Value>{0}</Value>", fileName);
                }
                else if (Item is BitmapPatternParameters)
                {
                    BitmapPatternParameters bitmapPattern = (BitmapPatternParameters)Item;
                    streamWriter.WriteLine("<BitmapPattern>");
                    streamWriter.WriteLine("<NumberOfRows>{0}</NumberOfRows>", bitmapPattern.NumberOfRows);
                    streamWriter.WriteLine("<NumberOfColumns>{0}</NumberOfColumns>", bitmapPattern.NumberOfColumns);
                    streamWriter.WriteLine("<StartValue>{0}</StartValue>", bitmapPattern.StartValue);
                    streamWriter.WriteLine("<ValueIncrementPerRowBlock>{0}</ValueIncrementPerRowBlock>", bitmapPattern.ValueIncrementPerRowBlock);
                    streamWriter.WriteLine("<ValueIncrementPerColumnBlock>{0}</ValueIncrementPerColumnBlock>", bitmapPattern.ValueIncrementPerColumnBlock);
                    streamWriter.WriteLine("<NumberOfIdenticalValueRows>{0}</NumberOfIdenticalValueRows>", bitmapPattern.NumberOfIdenticalValueRows);
                    streamWriter.WriteLine("<NumberOfIdenticalValueColumns>{0}</NumberOfIdenticalValueColumns>", bitmapPattern.NumberOfIdenticalValueColumns);
                    streamWriter.WriteLine("</BitmapPattern>");
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Other Long String
    /// </summary>
    /// <remarks>
    /// A string of 32-bit words where the encoding of the contents is specified by the negotiated Transfer Syntax.
    /// OL is a VR that requires byte swapping within each word when changing between Little Endian and Big Endian byte ordering (see Section 7.3).
    /// </remarks>
    public class OtherLongString : DicomValueType
    {

        /// <summary>
        /// Underlying <c>FileName</c> <see cref="System.String"/> value or <c>BitMapPattern</c> <see cref="BitmapPatternParameters"/>.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item = null;
        // TODO should this be string.Empty?

        //        public System.Byte[] ByteArray
        //        {
        //            set { _Item = value; }
        //        }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName
        {
            get
            {
                return (string)_Item;
            }
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Bit pattern.
        /// </summary>
        public BitmapPatternParameters BitmapPattern
        {
            get { return (BitmapPatternParameters)_Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Property Compressed - is OF data compressed Y/N?
        /// </summary>
        public bool Compressed
        {
            get { return _compressed; }
            set { _compressed = value; }
        }
        private bool _compressed = false;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (Item != null)
            {
                if (Item is string)
                {
                    string fileName = (string)Item;
                    streamWriter.WriteLine("<Value>{0}</Value>", fileName);
                }
                else if (Item is BitmapPatternParameters)
                {
                    BitmapPatternParameters bitmapPattern = (BitmapPatternParameters)Item;
                    streamWriter.WriteLine("<BitmapPattern>");
                    streamWriter.WriteLine("<NumberOfRows>{0}</NumberOfRows>", bitmapPattern.NumberOfRows);
                    streamWriter.WriteLine("<NumberOfColumns>{0}</NumberOfColumns>", bitmapPattern.NumberOfColumns);
                    streamWriter.WriteLine("<StartValue>{0}</StartValue>", bitmapPattern.StartValue);
                    streamWriter.WriteLine("<ValueIncrementPerRowBlock>{0}</ValueIncrementPerRowBlock>", bitmapPattern.ValueIncrementPerRowBlock);
                    streamWriter.WriteLine("<ValueIncrementPerColumnBlock>{0}</ValueIncrementPerColumnBlock>", bitmapPattern.ValueIncrementPerColumnBlock);
                    streamWriter.WriteLine("<NumberOfIdenticalValueRows>{0}</NumberOfIdenticalValueRows>", bitmapPattern.NumberOfIdenticalValueRows);
                    streamWriter.WriteLine("<NumberOfIdenticalValueColumns>{0}</NumberOfIdenticalValueColumns>", bitmapPattern.NumberOfIdenticalValueColumns);
                    streamWriter.WriteLine("</BitmapPattern>");
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Other Very Long String
    /// </summary>
    /// <remarks>
    /// A stream of 64-bit words where the encoding of the contents is specified by the negotiated Transfer Syntax. 
    /// OV is a VR that requires byte swapping within each word when changing byte ordering (see Section 7.3).
    /// </remarks>
    public class OtherVeryLongString : DicomValueType
    {

        /// <summary>
        /// Underlying <c>FileName</c> <see cref="System.String"/> value or <c>BitMapPattern</c> <see cref="BitmapPatternParameters"/>.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item = null;
        // TODO should this be string.Empty?

        //        public System.Byte[] ByteArray
        //        {
        //            set { _Item = value; }
        //        }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName
        {
            get
            {
                return (string)_Item;
            }
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Bit pattern.
        /// </summary>
        public BitmapPatternParameters BitmapPattern
        {
            get { return (BitmapPatternParameters)_Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Property Compressed - is OV data compressed Y/N?
        /// </summary>
        public bool Compressed
        {
            get { return _compressed; }
            set { _compressed = value; }
        }
        private bool _compressed = false;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (Item != null)
            {
                if (Item is string)
                {
                    string fileName = (string)Item;
                    streamWriter.WriteLine("<Value>{0}</Value>", fileName);
                }
                else if (Item is BitmapPatternParameters)
                {
                    BitmapPatternParameters bitmapPattern = (BitmapPatternParameters)Item;
                    streamWriter.WriteLine("<BitmapPattern>");
                    streamWriter.WriteLine("<NumberOfRows>{0}</NumberOfRows>", bitmapPattern.NumberOfRows);
                    streamWriter.WriteLine("<NumberOfColumns>{0}</NumberOfColumns>", bitmapPattern.NumberOfColumns);
                    streamWriter.WriteLine("<StartValue>{0}</StartValue>", bitmapPattern.StartValue);
                    streamWriter.WriteLine("<ValueIncrementPerRowBlock>{0}</ValueIncrementPerRowBlock>", bitmapPattern.ValueIncrementPerRowBlock);
                    streamWriter.WriteLine("<ValueIncrementPerColumnBlock>{0}</ValueIncrementPerColumnBlock>", bitmapPattern.ValueIncrementPerColumnBlock);
                    streamWriter.WriteLine("<NumberOfIdenticalValueRows>{0}</NumberOfIdenticalValueRows>", bitmapPattern.NumberOfIdenticalValueRows);
                    streamWriter.WriteLine("<NumberOfIdenticalValueColumns>{0}</NumberOfIdenticalValueColumns>", bitmapPattern.NumberOfIdenticalValueColumns);
                    streamWriter.WriteLine("</BitmapPattern>");
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Other Double String
    /// </summary>
    /// <remarks>
    /// A string of 64-bit IEEE 754:1985 floating point words.
    /// OD is a VR that requires byte swapping within each 64-bit
    /// word when changing between Little Endian and Big Endian
    /// byte ordering (see Section 7.3).
    /// </remarks>
    public class OtherDoubleString : DicomValueType
    {

        /// <summary>
        /// Underlying <c>FileName</c> <see cref="System.String"/> value or <c>BitMapPattern</c> <see cref="BitmapPatternParameters"/>.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item = null;
        // TODO should this be string.Empty?

        //        public System.Byte[] ByteArray
        //        {
        //            set { _Item = value; }
        //        }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName
        {
            get
            {
                return (string)_Item;
            }
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Bit pattern.
        /// </summary>
        public BitmapPatternParameters BitmapPattern
        {
            get { return (BitmapPatternParameters)_Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Property Compressed - is OB data compressed Y/N?
        /// </summary>
        public bool Compressed
        {
            get { return _compressed; }
            set { _compressed = value; }
        }
        private bool _compressed = false;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (Item != null)
            {
                if (Item is string)
                {
                    string fileName = (string)Item;
                    streamWriter.WriteLine("<Value>{0}</Value>", fileName);
                }
                else if (Item is BitmapPatternParameters)
                {
                    BitmapPatternParameters bitmapPattern = (BitmapPatternParameters)Item;
                    streamWriter.WriteLine("<BitmapPattern>");
                    streamWriter.WriteLine("<NumberOfRows>{0}</NumberOfRows>", bitmapPattern.NumberOfRows);
                    streamWriter.WriteLine("<NumberOfColumns>{0}</NumberOfColumns>", bitmapPattern.NumberOfColumns);
                    streamWriter.WriteLine("<StartValue>{0}</StartValue>", bitmapPattern.StartValue);
                    streamWriter.WriteLine("<ValueIncrementPerRowBlock>{0}</ValueIncrementPerRowBlock>", bitmapPattern.ValueIncrementPerRowBlock);
                    streamWriter.WriteLine("<ValueIncrementPerColumnBlock>{0}</ValueIncrementPerColumnBlock>", bitmapPattern.ValueIncrementPerColumnBlock);
                    streamWriter.WriteLine("<NumberOfIdenticalValueRows>{0}</NumberOfIdenticalValueRows>", bitmapPattern.NumberOfIdenticalValueRows);
                    streamWriter.WriteLine("<NumberOfIdenticalValueColumns>{0}</NumberOfIdenticalValueColumns>", bitmapPattern.NumberOfIdenticalValueColumns);
                    streamWriter.WriteLine("</BitmapPattern>");
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Other Byte String
    /// </summary>
    /// <remarks>
    /// A string of bytes where the encoding of the
    /// contents is specified by the negotiated
    /// Transfer Syntax. OB is a VR which is
    /// insensitive to Little/Big Endian byte ordering
    /// (see Section 7.3). The string of bytes shall be
    /// padded with a single trailing NULL byte value
    /// (00H) when necessary to achieve even length.
    /// </remarks>
    public class OtherByteString : DicomValueType
    {

        /// <summary>
        /// Underlying <c>FileName</c> <see cref="System.String"/> value or <c>BitMapPattern</c> <see cref="BitmapPatternParameters"/>.
        /// </summary>
        public object Item
        {
            get
            {
                return _Item;
            }
            set
            {
                _Item = value;
            }
        }
        private object _Item = null;
        // TODO should this be string.Empty?

        //        public System.Byte[] ByteArray
        //        {
        //            set { _Item = value; }
        //        }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName
        {
            get
            {
                return (string)_Item;
            }
            set
            {
                _Item = value;
            }
        }

        /// <summary>
        /// Bit pattern.
        /// </summary>
        public BitmapPatternParameters BitmapPattern
        {
            get { return (BitmapPatternParameters)_Item; }
            set { _Item = value; }
        }

        /// <summary>
        /// Property Compressed - is OB data compressed Y/N?
        /// </summary>
        public bool Compressed
        {
            get { return _compressed; }
            set { _compressed = value; }
        }
        private bool _compressed = false;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            if (Item != null)
            {
                if (Item is string)
                {
                    string fileName = (string)Item;
                    streamWriter.WriteLine("<Value>{0}</Value>", fileName);
                }
                else if (Item is BitmapPatternParameters)
                {
                    BitmapPatternParameters bitmapPattern = (BitmapPatternParameters)Item;
                    streamWriter.WriteLine("<BitmapPattern>");
                    streamWriter.WriteLine("<NumberOfRows>{0}</NumberOfRows>", bitmapPattern.NumberOfRows);
                    streamWriter.WriteLine("<NumberOfColumns>{0}</NumberOfColumns>", bitmapPattern.NumberOfColumns);
                    streamWriter.WriteLine("<StartValue>{0}</StartValue>", bitmapPattern.StartValue);
                    streamWriter.WriteLine("<ValueIncrementPerRowBlock>{0}</ValueIncrementPerRowBlock>", bitmapPattern.ValueIncrementPerRowBlock);
                    streamWriter.WriteLine("<ValueIncrementPerColumnBlock>{0}</ValueIncrementPerColumnBlock>", bitmapPattern.ValueIncrementPerColumnBlock);
                    streamWriter.WriteLine("<NumberOfIdenticalValueRows>{0}</NumberOfIdenticalValueRows>", bitmapPattern.NumberOfIdenticalValueRows);
                    streamWriter.WriteLine("<NumberOfIdenticalValueColumns>{0}</NumberOfIdenticalValueColumns>", bitmapPattern.NumberOfIdenticalValueColumns);
                    streamWriter.WriteLine("</BitmapPattern>");
                }
                else
                {
                    throw new System.NotSupportedException();
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Long Text
    /// </summary>
    /// <remarks>
    /// A character string that may contain one or
    /// more paragraphs. It may contain the Graphic
    /// Character set and the Control Characters, CR,
    /// LF, FF, and ESC. It may be padded with
    /// trailing spaces, which may be ignored, but
    /// leading spaces are considered to be
    /// significant. Data Elements with this VR shall
    /// not be multi-valued and therefore character
    /// code 5CH (the BACKSLASH “\” in ISO-IR 6)
    /// may be used.
    /// </remarks>
    public class LongText : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> value.
        /// </summary>        
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
        private string _Value = null;
        // TODO should this be string.Empty?;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.WriteLine("<Value>{0}</Value>", DvtToXml.ConvertString(Value, true));

            // try to convert the string to Unicode - if possible
            if (_dicomUnicodeConverter != null)
            {
                String outString = DvtToXml.ConvertStringToXmlUnicode(_dicomUnicodeConverter, Value);
                if (outString != String.Empty)
                {
                    streamWriter.WriteLine("<Unicode>{0}</Unicode>", outString);
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Long String
    /// </summary>
    /// <remarks>
    /// A character string that may be padded with
    /// leading and/or trailing spaces. The character
    /// code 5CH (the BACKSLASH “\” in ISO-IR 6)
    /// shall not be present, as it is used as the
    /// delimiter between values in multiple valued
    /// data elements. The string shall not have
    /// Control Characters except for ESC.
    /// </remarks>
    public class LongString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Integer String
    /// </summary>
    /// <remarks>
    /// A string of characters representing an Integer
    /// in base-10 (decimal), shall contain only the
    /// characters 0 - 9, with an optional leading "+" or
    /// "-". It may be padded with leading and/or
    /// trailing spaces. Embedded spaces are not
    /// allowed.
    /// The integer, n, represented shall be in the
    /// range:<br></br>
    /// -2^31 &lt;= n &lt;= (2^31 - 1)
    /// </remarks>
    public class IntegerString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.String"/> collection.
        /// </summary>
        public DvtkData.Collections.StringCollection Values = new DvtkData.Collections.StringCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            Values.DicomUnicodeConverter = _dicomUnicodeConverter;
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Floating Point Double
    /// </summary>
    /// <remarks>
    /// Double precision binary floating point number
    /// represented in IEEE 754:1985 64-bit Floating
    /// Point Number Format.
    /// </remarks>
    public class FloatingPointDouble : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.Double"/> collection.
        /// </summary>
        public DvtkData.Collections.DoubleCollection Values = new DvtkData.Collections.DoubleCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Floating Point Single
    /// </summary>
    /// <remarks>
    /// Single precision binary floating point number
    /// represented in IEEE 754:1985 32-bit Floating
    /// Point Number Format.
    /// </remarks>
    public class FloatingPointSingle : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.Single"/> collection.
        /// </summary>
        public DvtkData.Collections.SingleCollection Values = new DvtkData.Collections.SingleCollection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Unknown
    /// </summary>
    /// <remarks>
    /// A string of bytes where the encoding of the
    /// contents is unknown
    /// </remarks>
    public class Unknown : DicomValueType
    {
        /// <summary>
        /// Underlying <see cref="System.Byte"/> array value.
        /// Note: Use underlying Byte array cautiously because modifying 
        /// the array affects directly the UN attribute value.
        /// </summary>        
        /*public System.Byte[] ByteArray 
        {
            get
            { 
                if (this._ByteArray == null) return null;
                else return (System.Byte[])_ByteArray.ToArray(typeof(Byte)); 
            }
            set 
            { 
                if (value == null) 
                {
                    this._ByteArray = null;
                }
                else
                {
                    _ByteArray = new ArrayList(value);
                }
            }
        }

		public System.Byte[] ByteArray 
		{
			get
			{ 
				if (this._ByteArray.Count == 0) return null;
				else return  (System.Byte[])_ByteArray[0]; 
			}
			set 
			{ 
				if (value == null) 
				{
					this._ByteArray.Clear();
				}
				else
				{
					object newObject = new object();
					newObject = value;
					_ByteArray.Add(newObject);
				}
			}
		} 
		private /*System.Byte[] ArrayList _ByteArray = new ArrayList(1);*/

        public System.Byte[] ByteArray
        {
            get
            {
                return _ByteArray;
            }
            set
            {
                _ByteArray = value;
            }
        }

        private System.Byte[] _ByteArray = null;

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            streamWriter.Write("<Value>");
            if (ByteArray != null)
            {
                int length = ByteArray.Length;
                // truncate output to the first 128 bytes
                if (length > 128) length = 128;
                for (int i = 0; i < length; i++)
                {
                    string byteValueString = ByteArray[i].ToString("X");
                    while (byteValueString.Length < 2)
                    {
                        byteValueString = "0" + byteValueString;
                    }
                    streamWriter.Write(byteValueString);
                }
                if (ByteArray.Length > 128)
                {
                    streamWriter.Write("...");
                }
            }
            streamWriter.WriteLine("</Value>");

            return true;
        }
    }

    /// <summary>
    /// Signed Very Long String
    /// </summary>
    /// <remarks>
    /// Signed binary integer 64 bits long in 2's
    /// complement form.
    /// Represents an integer, n, in the range:<br></br>
    /// -2^63 &lt;= n &lt;= (2^63 - 1)
    /// </remarks>
    public class SignedVeryLongString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.Int64"/> collection.
        /// </summary>
        public DvtkData.Collections.Int64Collection Values = new DvtkData.Collections.Int64Collection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }

    /// <summary>
    /// Unsigned Very Long String
    /// </summary>
    /// <remarks>
    /// Signed binary integer 64 bits long in 2's
    /// complement form.
    /// Represents an integer, n, in the range:<br></br>
    /// -2^63 &lt;= n &lt;= (2^63 - 1)
    /// </remarks>
    public class UnsignedVeryLongString : DicomValueType
    {

        /// <summary>
        /// Underlying <see cref="System.Int64"/> collection.
        /// </summary>
        public DvtkData.Collections.UInt64Collection Values = new DvtkData.Collections.UInt64Collection();

        /// <summary>
        /// Serialize DVT Detail Data to Xml.
        /// </summary>
        /// <param name="streamWriter">Stream writer to serialize to.</param>
        /// <param name="level">Recursion level. 0 = Top.</param> 
        /// <returns>bool - success/failure</returns>
        public override bool DvtDetailToXml(StreamWriter streamWriter, int level)
        {
            bool result = Values.DvtDetailToXml(streamWriter, level);
            return result;
        }
    }
}
