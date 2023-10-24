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
using System.IO;

using VR = DvtkData.Dimse.VR;
using DvtkHighLevelInterface.Common.Other;
using DvtkHighLevelInterface.Dicom.Threads;
using DvtkHighLevelInterface.Dicom.Files;



namespace DvtkHighLevelInterface.Dicom.Other
{
	/// <summary>
	/// Represents a Dicom Data Set.
	/// </summary>
	public class DataSet: AttributeSet
	{
		//
		// - Fields -
		//

		/// <summary>
		/// See property StoreOBOFOWValuesWhenReading
		/// </summary>
		private bool storeOBOFOWValuesWhenReading = true;

		/// <summary>
		/// See property UnVrDefinitionLookUpWhenReading.
		/// </summary>
		private bool unVrDefinitionLookUpWhenReading = true; 



		//
		// - Constructors -
		//

		/// <summary>
        /// Default Constructor. Creates an empty Data Set.
		/// </summary>
		public DataSet(): base(new DvtkData.Dimse.DataSet())
		{
            // Do nothing.
		}

		/// <summary>
        /// Constructor. Encapsulates a DvtkData.Dimse.AttributeSet instance.
		/// </summary>
        /// <param name="dvtkDataDataSet">The DvtkData.Dimse.DataSet instance to encapsulate.</param>
		internal DataSet(DvtkData.Dimse.DataSet dvtkDataDataSet): base(dvtkDataDataSet)
		{
			if (dvtkDataAttributeSet == null)
			{
				throw new HliException("Parameter may not be null/Nothing.");
			}
		}



		//
		// - Properties -
		//
        
        /// <summary>
        /// Gets or sets the encapsulated DvtkData.Dimse.DataSet instance.
        /// </summary>
        /// <remarks>
        /// <b>Avoid using this property if possible because it reveals the
        /// internal structure of this class!</b>
        /// </remarks>
        public DvtkData.Dimse.DataSet DvtkDataDataSet
		{
			set
			{
				this.dvtkDataAttributeSet = value;
			}
			get
			{
				return this.dvtkDataAttributeSet as DvtkData.Dimse.DataSet;
			}
		}

		/// <summary>
		/// Gets the IOD of this instance.
		/// </summary>
        /// <remarks>
        /// This property may be used in combination with the DVTk Validation functionality.
        /// </remarks>
		internal String IodId
		{
			set
			{
				this.DvtkDataDataSet.IodId = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if OB, OF and OW values will be available after
        /// calling the Read method.
        /// </summary>
        /// <remarks>
        /// Default value is true.<br></br><br></br>
        /// 
        /// Only when set to true, OB, OF and OW values will be stored in .pix files after calling
        /// the read method.
        /// </remarks>
		public bool StoreOBOFOWValuesWhenReading
		{
			get
			{
				return(this.storeOBOFOWValuesWhenReading);
			}
			set
			{
				this.storeOBOFOWValuesWhenReading = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if information from loaded definition files will be 
        /// used to replace VR's UN when calling the Read method.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        public bool UnVrDefinitionLookUpWhenReading
		{
			get 
			{ 
				return this.unVrDefinitionLookUpWhenReading; 
			}
			set 
			{ 
				this.unVrDefinitionLookUpWhenReading = value; 
			}
		}



		//
		// - Methods -
		//

		/// <summary>
        /// Creates a deep copy of this instance.
		/// </summary>
        /// <returns>A deep copy of this instance.</returns>
		public DataSet Clone()
		{
            // Create an empty Data Set.
			DataSet cloneDataSet = new DataSet();

            // Copy all attributes.
			for (int index = 0; index < this.Count; index++)
			{
				// The Add method will take care of the deep copy.
				cloneDataSet.Add(this[index]);
			}

            // Return the deep copy.
			return(cloneDataSet);
		}

		/// <summary>
        /// Makes this instance a deep copy of <paramref name="dataSet"/>.
		/// </summary>
        /// <param name="dataSet">The Data Set to copy from.</param>
		public void CloneFrom(DataSet dataSet)
		{
			Clear();

			for (int index = 0; index < dataSet.Count; index++)
			{
				// The Add method will take care of the deep copy.
				Add(dataSet[index]);
			}
		}

		/// <summary>
		/// Reads a Dicom File or Data Set file.
		/// </summary>
        /// <remarks>
        /// The previous attributes of this instance will be removed.
        /// <br></br><br></br>
        /// NOTE:
        /// The intention of this method is to use only the supplied definition files.
        /// The current implementation however uses all already loaded definition files outside this 
        /// method and the supplied definition files!<br></br><br></br>
        /// <br></br><br></br>
        /// Also see properties UnVrDefinitionLookUpWhenReading and StoreOBOFOWValuesWhenReading.
        /// <br></br><br></br>
        /// If something goes wrong while reading the file, an exception is thrown.
        /// </remarks>
		/// <param name="fullFileName">The full file name.</param>
		/// <param name="definitionFilesFullName">The definition files to use for determining the attribute names.</param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			'Example: Read the dataset from a specified DICOM file
		///			
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///			
		///			If File.Exists("") Then
        ///				Try
		///					myDataSet.Read("c:\Somefile.dcm")
        ///   			 	Catch ex As DvtkHighLevelInterface.Common.Other.HliException
		///					' Error reading the file, Maybe the file format is wrong?
        ///    			End Try
		///			End If
		///		</code>
		/// </example>		
		public void Read(String fullFileName, params String[] definitionFilesFullName)
		{
			DicomFile dicomFile = new DicomFile();

			dicomFile.StoreOBOFOWValuesWhenReading = this.storeOBOFOWValuesWhenReading;
			dicomFile.UnVrDefinitionLookUpWhenReading = this.unVrDefinitionLookUpWhenReading;

			// Make sure that the DicomFile object is using this object as the DataSet to put the
			// attributes in during reading.
			dicomFile.CreateNewDataSetObjectWhenReading = false;
			dicomFile.DataSet = this;

			dicomFile.Read(fullFileName, definitionFilesFullName);
		}

		/// <summary>
        /// Reads a Dicom File or Data Set file.
		/// </summary>
        /// <remarks>
        /// The previous attributes of this instance will be removed.
        /// <br></br><br></br>
        /// NOTE:<br></br>
        /// The intention of this method is to use only the definition files loaded in the supplied
        /// DicomThread. The current implementation however uses all already loaded definition files outside this 
        /// method!
        /// <br></br><br></br>
        /// Also see properties UnVrDefinitionLookUpWhenReading and StoreOBOFOWValuesWhenReading.
        /// <br></br><br></br>
        /// If something goes wrong while reading the file, an exception is thrown.
        /// </remarks>
		/// <param name="fullFileName">The full file name.</param>
		/// <param name="dicomThread">The DicomThread, from which the definition files to use for determining the attribute names are used.</param>
		public void Read(String fullFileName, DicomThread dicomThread)
		{
			DicomFile dicomFile = new DicomFile();

			dicomFile.StoreOBOFOWValuesWhenReading = this.storeOBOFOWValuesWhenReading;
			dicomFile.UnVrDefinitionLookUpWhenReading = this.unVrDefinitionLookUpWhenReading;

			// Make sure that the DicomFile object is using this object as the DataSet to put the
			// attributes in during reading.
			dicomFile.CreateNewDataSetObjectWhenReading = false;
			dicomFile.DataSet = this;

			dicomFile.Read(fullFileName, dicomThread);
		}

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
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
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

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DataSet attribute.", "dvtkDataTag");
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
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
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

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
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
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.
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

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " is not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
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
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
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

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DataSet attribute.", "dvtkDataTag");
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
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
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

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("parameters");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
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
        /// <param name="values">The values, which will be copied from another attribute, for this attribute.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="dvtkDataTag"/> is not valid for setting an attribute.<br></br>
        /// -or-<br></br>
        /// <paramref name="dvtkDataTag"/> is not valid for setting a DataSet attribute.<br></br>
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

            // Check if the tag supplied is valid for a DataSet.
            if (!internalTagSequence.IsValidForDataSet)
            {
                throw new ArgumentException(internalTagSequence.ToString() + " not valid for setting a DataSet attribute.", "dvtkDataTag");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }


            //
            // Perform the actual operation in the base class.
            //

            Set(internalTagSequence, vR, values);
        }

		/// <summary>
		/// Writes all attributes of this instance to a Data Set file. 
		/// </summary>
		/// <param name="fullFileName">The full file name.</param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			' Example: Read the dataset from a specified DICOM file and 
		///			'write it to a DataSet file (no FileMetaInformation written).
		///			
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///			
		///			If File.Exists("c:\somefile.dcm") Then
        ///				Try
		///					myDataSet.Read("c:\Somefile.dcm")
        ///   			 	Catch ex As DvtkHighLevelInterface.Common.Other.HliException
		///					' Error reading the file, Maybe the file format is wrong?
        ///    			End Try
		///				'here is where you can manipulate the dataset
		///				'write the dataset to a file
		///				myDataSet.Write("c:\newfile.dcm")
		///			End If
		///		</code>
		/// </example>
		/// <remarks>
        /// No File Meta Information will be written. If this is needed, consider using the class
        /// DicomFile.<br></br><br></br>
        /// 
        /// If something goes wrong while writing the file, an exception is thrown.<br></br><br></br>
        /// 
		///	The following transfer syntax will be used: "1.2.840.10008.1.2.1" (Explicit VR Little Endian).
		/// I you want to use another transfer syntax then use the Write(fullFileName as String, transferSyntax as String)
        /// method.
		/// </remarks>
		public void Write(String fullFileName)
		{
			Write(fullFileName, "1.2.840.10008.1.2.1");
		}

		/// <summary>
        /// Writes all attributes of this instance to a Data Set file.
		/// </summary>
		/// <param name="fullFileName">The full file name.</param>
		/// <param name="transferSyntax">The transfer syntax.</param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			' Example: Read the dataset from a specified DICOM file and 
		///			'write it to a DataSet file (no FileMetaInformation written) 
		///			'with a specified transfersyntax.
		///			
        ///			Dim myDataSet As DvtkHighLevelInterface.Dicom.Other.DataSet
		///			
		///			If File.Exists("c:\somefile.dcm") Then
        ///				Try
		///					myDataSet.Read("c:\Somefile.dcm")
        ///   			 	Catch ex As DvtkHighLevelInterface.Common.Other.HliException
		///					' Error reading the file, Maybe the file format is wrong?
        ///    			End Try
		///				'here is where you can manipulate the dataset
		///				'write the dataset to a file
		///				myDataSet.Write("c:\newfile.dcm","1.2.840.10008.1.2.1")
		///			End If
		///		</code>
		/// </example>
		/// <remarks>
        /// No File Meta Information will be written. If this is needed, consider using the class
        /// DicomFile.<br></br><br></br>
        /// 
        /// If something goes wrong while writing the file, an exception is thrown.
        /// </remarks>
		public void Write(String fullFileName, String transferSyntax)
		{
			Dvtk.DvtkDataHelper.WriteDataSet(this.DvtkDataDataSet, fullFileName, transferSyntax);
		}
	}
}
