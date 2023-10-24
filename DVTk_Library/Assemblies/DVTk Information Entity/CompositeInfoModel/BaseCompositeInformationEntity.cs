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
using System.Collections.Generic;
using System.Text;
using System.Collections;
using DvtkData.Dimse;
using DvtkData.Collections;

namespace Dvtk.Dicom.InformationEntity.CompositeInfoModel
{
    /// <summary>
    /// Base composite Information class, provides base functionalites for Composite object model
    /// </summary>
    public abstract class BaseCompositeInformationEntity
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public BaseCompositeInformationEntity()
        {
            children = new List<BaseCompositeInformationEntity>();
            attributes = new List<DvtkData.Dimse.Attribute>();
            
        }
        protected virtual void LoadData(DataSet _dataSet)
        {
            foreach (DvtkData.Dimse.Attribute a in _dataSet)
            {
                Attributes.Add(a);
            }
        }
        protected string ConvertToString(StringCollection str)
        {
            String s="";
            foreach (string sr in str)
                s = s + sr;
            return s;
        }
        List<BaseCompositeInformationEntity> children;
        /// <summary>
        /// gets the Collection of childrens of the objects
        /// </summary>
        public List<BaseCompositeInformationEntity> Children
        {
            get
            {
                return children;
            }
        }

        List<DvtkData.Dimse.Attribute> attributes;
        /// <summary>
        /// gets the attributes of the objects
        /// </summary>
        public List<DvtkData.Dimse.Attribute> Attributes
        {
            get
            {
                return attributes;
            }
        }
        /// <summary>
        /// This static function returns the Re-Construted value of the particular Dicom attribute.
        /// </summary>
        /// <param name="attribute">Dicom attribute contains tag vr and value</param>
        /// <returns>Returns the value of the Dicom attribute</returns>
        public static String GetDicomValue(DvtkData.Dimse.Attribute attribute)
        {
            String dumpString = "";
            if (attribute!=null&& attribute.Length != 0)
            {
                switch (attribute.ValueRepresentation)
                {
                    case VR.AE:
                        {
                            ApplicationEntity applicationEntity = (ApplicationEntity)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(applicationEntity.Values));
                            break;
                        }
                    case VR.AS:
                        {
                            AgeString ageString = (AgeString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(ageString.Values));
                            break;
                        }
                    case VR.AT:
                        {
                            AttributeTag attributeTag = (AttributeTag)attribute.DicomValue;
                            Console.WriteLine("{0}", GetValues(attributeTag.Values));
                            break;
                        }
                    case VR.CS:
                        {
                            CodeString codeString = (CodeString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(codeString.Values));
                            break;
                        }
                    case VR.DA:
                        {
                            Date date = (Date)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(date.Values));
                            break;
                        }
                    case VR.DS:
                        {
                            DecimalString decimalString = (DecimalString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(decimalString.Values));
                            break;
                        }
                    case VR.DT:
                        {
                            DvtkData.Dimse.DateTime dateTime = (DvtkData.Dimse.DateTime)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(dateTime.Values));
                            break;
                        }
                    case VR.FD:
                        {
                            FloatingPointDouble floatingPointDouble = (FloatingPointDouble)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(floatingPointDouble.Values));
                            break;
                        }
                    case VR.FL:
                        {
                            FloatingPointSingle floatingPointSingle = (FloatingPointSingle)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(floatingPointSingle.Values));
                            break;
                        }
                    case VR.IS:
                        {
                            IntegerString integerString = (IntegerString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(integerString.Values));
                            break;
                        }
                    case VR.LO:
                        {
                            LongString longString = (LongString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(longString.Values));
                            break;
                        }
                    case VR.LT:
                        {
                            LongText longText = (LongText)attribute.DicomValue;
                            dumpString += String.Format("{0}", longText.Value);
                            break;
                        }
                    case VR.OB:
                        {
                            OtherByteString otherByteString = (OtherByteString)attribute.DicomValue;
                            dumpString += String.Format("{0}", otherByteString.FileName);
                            break;
                        }
                    case VR.OF:
                        {
                            OtherFloatString otherFloatString = (OtherFloatString)attribute.DicomValue;
                            dumpString += String.Format("{0}", otherFloatString.FileName);
                            break;
                        }
                    case VR.OW:
                        {
                            OtherWordString otherWordString = (OtherWordString)attribute.DicomValue;
                            dumpString += String.Format("{0}", otherWordString.FileName);
                            break;
                        }
                    case VR.OV:
                        {
                            OtherVeryLongString otherVeryLongString = (OtherVeryLongString)attribute.DicomValue;
                            dumpString += String.Format("{0}", otherVeryLongString.FileName);
                            break;
                        }
                    case VR.PN:
                        {
                            PersonName personName = (PersonName)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(personName.Values));
                            break;
                        }
                    case VR.SH:
                        {
                            ShortString shortString = (ShortString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(shortString.Values));
                            break;
                        }
                    case VR.SL:
                        {
                            SignedLong signedLong = (SignedLong)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(signedLong.Values));
                            break;
                        }
                    case VR.SQ:
                        {
                            //SequenceOfItems sequenceOfItems = (SequenceOfItems)attribute.DicomValue;
                            //int itemNumber = 1;
                            //dumpString += "\r\n";
                            //foreach (SequenceItem item in sequenceOfItems.Sequence)
                            //{
                            //    dumpString += String.Format("> Begin Item: {0}\r\n", itemNumber);
                            //    dumpString += item.Dump(prefix);
                            //    dumpString += prefix + String.Format("> End Item: {0}\r\n", itemNumber++);
                            //}
                            break;
                        }
                    case VR.SS:
                        {
                            SignedShort signedShort = (SignedShort)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(signedShort.Values));
                            break;
                        }
                    case VR.ST:
                        {
                            ShortText shortText = (ShortText)attribute.DicomValue;
                            dumpString += String.Format("{0}", shortText.Value);
                            break;
                        }
                    case VR.SV:
                        {
                            SignedVeryLongString signedVeryLongString = (SignedVeryLongString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(signedVeryLongString.Values));
                            break;
                        }
                    case VR.TM:
                        {
                            Time time = (Time)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(time.Values));
                            break;
                        }
                    case VR.UI:
                        {
                            UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(uniqueIdentifier.Values));
                            break;
                        }
                    case VR.UL:
                        {
                            UnsignedLong unsignedLong = (UnsignedLong)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(unsignedLong.Values));
                            break;
                        }
                    case VR.UN:
                        {
                            break;
                        }
                    case VR.US:
                        {
                            UnsignedShort unsignedShort = (UnsignedShort)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(unsignedShort.Values));
                            break;
                        }
                    case VR.UV:
                        {
                            UnsignedVeryLongString unsignedVeryLongString = (UnsignedVeryLongString)attribute.DicomValue;
                            dumpString += String.Format("{0}", GetValues(unsignedVeryLongString.Values));
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
                        dumpString += String.Format("\'  \'");
                        break;
                }
            }
            else
            {
                dumpString += String.Format("\'  \'");
            }


            return (dumpString);

        }

        static string GetValues(DvtkData.Collections.NullSafeCollectionBase collections)
        {
            string retstring="";
            foreach( object v in collections)
            {
                retstring = retstring+"\\" + v.ToString();
            }
            return retstring.Remove(0,1);
        }
        static string GetValues(DvtkData.Collections.StringCollection collections)
        {
            string retstring = "";
            foreach (object v in collections)
            {
                retstring = retstring + "\\" + v.ToString();
            }
            return retstring.Remove(0, 1);
        }
    }
}
