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

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Files
{
	/// <summary>
	/// Represents Dicom File Meta Information.
	/// </summary>
	public class FileMetaInformation: AttributeSet
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property DvtkDataFileHead.
		/// </summary>
		private DvtkData.Media.FileHead dvtkDataFileHead = new DvtkData.Media.FileHead();



		//
		// - Constructors -
		//

		/// <summary>
		/// Default constructor.
		/// </summary>
		public FileMetaInformation(): this(new DvtkData.Media.FileMetaInformation(), new DvtkData.Media.FileHead())
		{
            // Make sure a default transfer syntax is available, otherwise you may get a crash when writing a DicomFile instance to file
            // if not set explicitly.
            TransferSyntax = "1.2.840.10008.1.2.1";

            Byte[] fileMetaInformationVersionValue = new Byte[2] { 0x00, 0x01 };
            Set("0x00020001", VR.OB, fileMetaInformationVersionValue); // File Meta Information Version.

            // Set the Implementation Class UID incase this FileMetaInformation is constructed from scratch.
            System.Reflection.Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Reflection.AssemblyName thisAssemblyName = thisAssembly.GetName();
            Set("0x00020012", VR.UI, "1.2.826.0.1.3680043.2.1545.1." + thisAssemblyName.Version.Major + "." + thisAssemblyName.Version.Minor + "." + thisAssemblyName.Version.Build + "." + thisAssemblyName.Version.Revision);
		}

        /// <summary>
        /// Constructor. Encapsulated existing DvtkData FileMetaInformation instance and DvtkData FileHead instance.
        /// </summary>
        /// <param name="dvtkDataFileMetaInformation">The encapsulated DvtkData FileMetaInformation instance.</param>
        /// <param name="dvtkDataFileHead">The encapsulated FileHead instance.</param>
        internal FileMetaInformation(DvtkData.Media.FileMetaInformation dvtkDataFileMetaInformation, DvtkData.Media.FileHead dvtkDataFileHead): base(dvtkDataFileMetaInformation)
        {
            this.dvtkDataFileHead = dvtkDataFileHead;
        }



		//
		// - Properties -
		//

		/// <summary>
		/// Get or set the file prefix.
		/// </summary>
		public byte[] DicomPrefix
		{
			get
			{
				return(this.dvtkDataFileHead.DicomPrefix); 
			}
			set
			{
				this.dvtkDataFileHead.DicomPrefix = value;
			}
		}

		/// <summary>
		/// The internal available encapsulated DvtkData FileHead object.
		/// </summary>
		internal DvtkData.Media.FileHead DvtkDataFileHead
		{
			get
			{
				return(this.dvtkDataFileHead);
			}
			set
			{
				this.dvtkDataFileHead = value;
			}
		}

		/// <summary>
		/// The internal available encapsulated DvtkData FileMetaInformation object.
		/// </summary>
		internal DvtkData.Media.FileMetaInformation DvtkDataFileMetaInformation
		{
			set
			{
				this.dvtkDataAttributeSet = value;
			}
			get
			{
				return this.dvtkDataAttributeSet as DvtkData.Media.FileMetaInformation;
			}
		}

		/// <summary>
		/// Get or set the file preamble.
		/// </summary>
		public byte[] FilePreamble
		{
			get
			{
				return(this.dvtkDataFileHead.FilePreamble); 
			}
			set
			{
				this.dvtkDataFileHead.FilePreamble = value;
			}
		}

		/// <summary>
		/// Get or set the Media Storage SOP Class UID ((0002,0002)).
		/// </summary>
		public String MediaStorageSOPClassUID
		{
			get
			{
				return(this["0x00020002"].Values[0]);
			}
			set
			{
				this.Set("0x00020002", VR.UI, value);
			}
		}

		/// <summary>
		/// Get or set the Media Storage SOP Instance UID ((0002,0003)).
		/// </summary>
		public String MediaStorageSOPInstanceUID
		{
			get
			{
				return(this["0x00020003"].Values[0]);
			}
			set
			{
				this.Set("0x00020003", VR.UI, value);
			}
		}

		/// <summary>
		/// Get or set the transfer syntax.
		/// 
		/// Default is 1.2.840.10008.1.2.1 (Explict Little Endian).
		/// </summary>
		public String TransferSyntax
		{
			get
			{
				return(this["0x00020010"].Values[0]);
			}
			set
			{
				this.dvtkDataFileHead.TransferSyntax = new DvtkData.Dul.TransferSyntax(value);
				this.Set("0x00020010", VR.UI, value);
			}
		}



		//
		// - Methods -
		//

        /// <summary>
        /// Adds a single attribute with the tag, VR and value specified.
        /// </summary>
        /// <remarks>
        /// Only use this method for setting an attribute with VR OB, OF or OW.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a FileMetaInformation attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="vR"/> is unequal to OB, OF or OW.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Byte[] value)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a FileMetaInformation.
            if (!internalTagSequence.IsValidForFileMetaInformation)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a FileMetaInformation attribute.", "dvtkDataTag");
            }

            // Check the supplied VR.

            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
            {
                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, value);
        }

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
        /// multiple values arguments for this method when adding a single attribute with multiple values.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a FileMetaInformation attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, params Object[] values)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a FileMetaInformation.
            if (!internalTagSequence.IsValidForFileMetaInformation)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a FileMetaInformation attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation.
            //

            Set(internalTagSequence, vR, values);

            if ((internalTagSequence.ToString() == "0x00020010") && (values.Length == 1))
            {
                this.dvtkDataFileHead.TransferSyntax = new DvtkData.Dul.TransferSyntax(values[0].ToString());
            }

        }

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting a FileMetaInformation attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, Values values)
        {
            TagSequence internalTagSequence = new TagSequence();

            internalTagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));


            //
            // Sanity checks.
            //

            // Check if the tag supplied is valid for a FileMetaInformation.
            if (!internalTagSequence.IsValidForFileMetaInformation)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a FileMetaInformation attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation.
            //

            Set(internalTagSequence, vR, values);

            if ((internalTagSequence.ToString() == "0x00020010") && (values.Count == 1))
            {
                this.dvtkDataFileHead.TransferSyntax = new DvtkData.Dul.TransferSyntax(values[0]);
            }
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and value specified.
        /// </summary>
        /// <remarks>
        /// Only use this method for setting an attribute with VR OB, OF or OW.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR (may only be OB, OF or OW) of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a FileMetaInformation attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="vR"/> is unequal to OB, OF or OW.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, Byte[] value)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a FileMetaInformation.
            if (!internalTagSequence.IsValidForFileMetaInformation)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a FileMetaInformation attribute.", "dvtkDataTag");
            }

            // Check the supplied VR.

            if ((vR != VR.OB) && (vR != VR.OF) && (vR != VR.OW))
            {
                throw new ArgumentException("Supplied VR is " + vR.ToString() + ". VR may only be OB, OF or OW.", "vR");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, value);
        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead supply
        /// multiple values arguments for this method when adding a single attribute with multiple values.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a FileMetaInformation attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, params Object[] values)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a FileMetaInformation.
            if (!internalTagSequence.IsValidForFileMetaInformation)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a FileMetaInformation attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("parameters");
            }


            //
            // Perform the actual operation.
            //

            Set(internalTagSequence, vR, values);

            if ((internalTagSequence.ToString() == "0x00020010") && (values.Length == 1))
            {
                this.dvtkDataFileHead.TransferSyntax = new DvtkData.Dul.TransferSyntax(values[0].ToString());
            }

        }

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// If an attribute already exists with this tag, it is removed first before it is again
        /// added.
        /// <br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence of the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a FileMetaInformation attribute.<br></br>
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="values"/> is a null reference.
        /// </exception>
        public override void Set(String tagSequence, VR vR, Values values)
        {
            TagSequence internalTagSequence = new TagSequence(tagSequence);


            //
            // Sanity checks.
            //

            if (!internalTagSequence.IsSingleAttributeMatching)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting an attribute.");
            }

            // Check if the tag supplied is valid for a FileMetaInformation.
            if (!internalTagSequence.IsValidForFileMetaInformation)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a FileMetaInformation attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation.
            //

            Set(internalTagSequence, vR, values);

            if ((internalTagSequence.ToString() == "0x00020010") && (values.Count == 1))
            {
                this.dvtkDataFileHead.TransferSyntax = new DvtkData.Dul.TransferSyntax(values[0]);
            }
        } 
	}
}
