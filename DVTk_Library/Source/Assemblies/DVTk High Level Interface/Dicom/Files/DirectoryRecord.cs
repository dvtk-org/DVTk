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

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Dicom.Other;



//namespace DvtkHighLevelInterface.Dicom.Files
//{
//    class DirectoryRecord: AttributeSet
//    {
//        //
//        // - Constructors -
//        //

//        /// <summary>
//        /// Default constructor. Creates an empty directory record.
//        /// </summary>
//        internal DirectoryRecord()
//        {

//        }



//        //
//        // - Properties -
//        //

//        internal DvtkData.Validation.DirectoryRecordType DirectoryRecordType
//        {
//            get
//            {
//                DvtkData.Validation.DirectoryRecordType dvtkDataDirectoryRecordType = DvtkData.Validation.DirectoryRecordType.UNKNOWN;

//                return (dvtkDataDirectoryRecordType);
//            }

//            set
//            {

//            }
//        }



//        //
//        // - Methods -
//        //

//        /// <summary>
//        /// Adds a single attribute with the tag, VR and value specified.
//        /// </summary>
//        /// <remarks>
//        /// Only use this method for setting an attribute with VR OB, OF or OW.
//        /// <br></br><br></br>
//        /// If an attribute already exists with this tag, it is removed first before it is again
//        /// added.
//        /// </remarks>
//        /// <param name="dvtkDataTag">The tag of the attribute.</param>
//        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
//        /// <param name="value">The value of the attribute.</param>
//        /// <exception cref="System.ArgumentException">
//        /// <paramref name="dvtkDataTag"/> is not valid for setting a DirectoryRecord attribute.<br>
//        /// -or-<br>
//        /// <paramref name="vR"/> is unequal to OB, OF or OW.
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// <paramref name="value"/> is a null reference.
//        /// </exception>
//        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Byte[] value)
//        {
//            TagSequence internalTagSequence = new TagSequence();

//            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


//            //
//            // Sanity checks.
//            //

//            // Check if the tag supplied is valid for a DirectoryRecord.
//            if (!internalTagSequence.IsValidForDirectoryRecord)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DirectoryRecord attribute.", "dvtkDataTag");
//            }

//            // Check the supplied VR.

//            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
//            {
//                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
//            }

//            if (value == null)
//            {
//                throw new ArgumentNullException("value");
//            }


//            //
//            // Perform the actual operation in the base class.
//            //

//            Set(internalTagSequence, vR, value);
//        }

//        /// <summary>
//        /// Adds a single attribute with the tag, VR and values specified.
//        /// </summary>
//        /// <remarks>
//        /// If an attribute already exists with this tag, it is removed first before it is again
//        /// added.
//        /// </remarks>
//        /// <param name="dvtkDataTag">The tag of the attribute.</param>
//        /// <param name="vR">The VR of the attribute.</param>
//        /// <param name="values">
//        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
//        /// multiple values arguments for this method when adding a single attribute with multiple values.
//        /// </param>
//        /// <exception cref="System.ArgumentException">
//        /// <paramref name="dvtkDataTag"/> is not valid for setting a DirectoryRecord attribute.<br>
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// <paramref name="values"/> is a null reference.
//        /// </exception>
//        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, params Object[] values)
//        {
//            TagSequence internalTagSequence = new TagSequence();

//            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


//            //
//            // Sanity checks.
//            //

//            // Check if the tag supplied is valid for a DirectoryRecord.
//            if (!internalTagSequence.IsValidForDirectoryRecord)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DirectoryRecord attribute.", "dvtkDataTag");
//            }

//            if (values == null)
//            {
//                throw new ArgumentNullException("values");
//            }


//            //
//            // Perform the actual operation.
//            //

//            Set(internalTagSequence, vR, values);
//        }

//        /// <summary>
//        /// Adds a single attribute with the tag, VR and values specified.
//        /// </summary>
//        /// <remarks>
//        /// If an attribute already exists with this tag, it is removed first before it is again
//        /// added.
//        /// </remarks>
//        /// <param name="dvtkDataTag">The tag of the attribute.</param>
//        /// <param name="vR">The VR of the attribute.</param>
//        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
//        /// <exception cref="System.ArgumentException">
//        /// <paramref name="dvtkDataTag"/> is not valid for setting a DirectoryRecord attribute.<br>
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// <paramref name="values"/> is a null reference.
//        /// </exception>
//        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Values values)
//        {
//            TagSequence internalTagSequence = new TagSequence();

//            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


//            //
//            // Sanity checks.
//            //

//            // Check if the tag supplied is valid for a DirectoryRecord.
//            if (!internalTagSequence.IsValidForDirectoryRecord)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DirectoryRecord attribute.", "dvtkDataTag");
//            }

//            if (values == null)
//            {
//                throw new ArgumentNullException("values");
//            }


//            //
//            // Perform the actual operation.
//            //

//            Set(internalTagSequence, vR, values);
//        }

//        /// <summary>
//        /// Adds a single attribute with the tag sequence, VR and value specified.
//        /// </summary>
//        /// <remarks>
//        /// Only use this method for setting an attribute with VR OB, OF or OW.
//        /// <br></br><br></br>
//        /// If an attribute already exists with this tag, it is removed first before it is again
//        /// added.
//        /// <br></br><br></br>
//        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
//        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
//        /// attributes.
//        /// </remarks>
//        /// <param name="tagSequence">The tag sequence of the attribute.</param>
//        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
//        /// <param name="value">The value of the attribute.</param>
//        /// <exception cref="System.ArgumentException">
//        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br>
//        /// -or-<br>
//        /// <paramref name="dvtkDataTag"/> is not valid for setting a DirectoryRecord attribute.<br>
//        /// -or-<br>
//        /// <paramref name="vR"/> is unequal to OB, OF or OW.
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// <paramref name="value"/> is a null reference.
//        /// </exception>
//        public override void Set(String tagSequence, VR vR, Byte[] value)
//        {
//            TagSequence internalTagSequence = new TagSequence(tagSequence);


//            //
//            // Sanity checks.
//            //

//            if (!internalTagSequence.IsSingleAttributeMatching)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
//            }

//            // Check if the tag supplied is valid for a FileMetaInformation.
//            if (!internalTagSequence.IsValidForDirectoryRecord)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DirectoryRecord attribute.", "dvtkDataTag");
//            }

//            // Check the supplied VR.

//            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
//            {
//                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
//            }

//            if (value == null)
//            {
//                throw new ArgumentNullException("value");
//            }


//            //
//            // Perform the actual operation in the base class.
//            //

//            Set(internalTagSequence, vR, value);
//        }

//        /// <summary>
//        /// Adds a single attribute with the tag sequence, VR and values specified.
//        /// </summary>
//        /// <remarks>
//        /// If an attribute already exists with this tag, it is removed first before it is again
//        /// added.
//        /// <br></br><br></br>
//        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
//        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
//        /// attributes.
//        /// </remarks>
//        /// <param name="tagSequence">The tag sequence of the attribute.</param>
//        /// <param name="vR">The VR of the attribute.</param>
//        /// <param name="values">
//        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
//        /// multiple values arguments for this method when adding a single attribute with multiple values.
//        /// </param>
//        /// <exception cref="System.ArgumentException">
//        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br>
//        /// -or-<br>
//        /// <paramref name="dvtkDataTag"/> is not valid for setting a DirectoryRecord attribute.<br>
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// <paramref name="values"/> is a null reference.
//        /// </exception>
//        public override void Set(String tagSequence, VR vR, params Object[] values)
//        {
//            TagSequence internalTagSequence = new TagSequence(tagSequence);


//            //
//            // Sanity checks.
//            //

//            if (!internalTagSequence.IsSingleAttributeMatching)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
//            }

//            // Check if the tag supplied is valid for a DirectoryRecord.
//            if (!internalTagSequence.IsValidForDirectoryRecord)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DirectoryRecord attribute.", "dvtkDataTag");
//            }

//            if (values == null)
//            {
//                throw new ArgumentNullException("parameters");
//            }


//            //
//            // Perform the actual operation.
//            //

//            Set(internalTagSequence, vR, values);
//        }

//        /// <summary>
//        /// Adds a single attribute with the tag sequence, VR and values specified.
//        /// </summary>
//        /// <remarks>
//        /// If an attribute already exists with this tag, it is removed first before it is again
//        /// added.
//        /// <br></br><br></br>
//        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
//        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
//        /// attributes.
//        /// </remarks>
//        /// <param name="tagSequence">The tag sequence of the attribute.</param>
//        /// <param name="vR">The VR of the attribute.</param>
//        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
//        /// <exception cref="System.ArgumentException">
//        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br>
//        /// -or-<br>
//        /// <paramref name="dvtkDataTag"/> is not valid for setting a DirectoryRecord attribute.<br>
//        /// </exception>
//        /// <exception cref="System.ArgumentNullException">
//        /// <paramref name="values"/> is a null reference.
//        /// </exception>
//        public override void Set(String tagSequence, VR vR, Values values)
//        {
//            TagSequence internalTagSequence = new TagSequence(tagSequence);


//            //
//            // Sanity checks.
//            //

//            if (!internalTagSequence.IsSingleAttributeMatching)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
//            }

//            // Check if the tag supplied is valid for a DirectoryRecord.
//            if (!internalTagSequence.IsValidForDirectoryRecord)
//            {
//                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DirectoryRecord attribute.", "dvtkDataTag");
//            }

//            if (values == null)
//            {
//                throw new ArgumentNullException("values");
//            }


//            //
//            // Perform the actual operation.
//            //

//            Set(internalTagSequence, vR, values);
//        } 
//    }
//}
