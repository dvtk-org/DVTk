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
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using DvtkHighLevelInterface.Dicom.Threads;



namespace DvtkHighLevelInterface.Dicom.Files
{
	/// <summary>
	/// Represents a Dicom File.
	/// </summary>
    /// <remarks>
    /// An instance of this class contains both File Meta Information and a Data Set.
    /// </remarks>
	public class DicomFile
	{
		//
		// - Fields -
		//

        /// <summary>
        /// See property AddGroupLength.
        /// </summary>
        private bool addGroupLength = false;

		/// <summary>
		/// See property CreateNewDataSetObjectWhenReading.
		/// </summary>
		private bool createNewDataSetObjectWhenReading = true;

		/// <summary>
		/// See property DataSet.
		/// </summary>
		private DataSet dataSet = new DataSet();

		/// <summary>
		/// See property FileMetaInformation.
		/// </summary>
		private FileMetaInformation fileMetaInformation = new FileMetaInformation();

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
        /// Default constructor. Creates an empty Dicom File.
		/// </summary>
		public DicomFile()
		{
		}



		//
		// - Properties -
		//

        /// <summary>
        /// Gets or sets a boolean indicating if group length attributes will be added to the 
        /// dataset when calling the Write method.
        /// </summary>
        /// <remarks>
        /// Default value is false.
        /// </remarks>
        public bool AddGroupLength
        {
            get
            {
                return this.addGroupLength;
            }
            set
            {
                this.addGroupLength = value;
            }
        }

		/// <summary>
        /// Gets or sets a boolean indicating if a new DataSet instance is created when calling the
        /// Read method.
		/// </summary>
		internal bool CreateNewDataSetObjectWhenReading
		{
			get
			{
				return(this.createNewDataSetObjectWhenReading);
			}
			set
			{
				this.createNewDataSetObjectWhenReading = value;
			}
		}

		/// <summary>
		/// Gets or sets the Data Set.
		/// </summary>
		public DataSet DataSet
		{
			get
			{
				return(this.dataSet);
			}
			set
			{
				this.dataSet = value;
			}
		}

        /// <summary>
        /// Gets a DvtkData DicomFile instance containing the same Attributes as this instance.
        /// </summary>
        public DvtkData.Media.DicomFile DvtkDataDicomFile
        {
            get
            {
                DvtkData.Media.DicomFile dvtkDataDicomFile = new DvtkData.Media.DicomFile();

                dvtkDataDicomFile.DataSet = this.dataSet.DvtkDataDataSet;
                dvtkDataDicomFile.FileMetaInformation = this.fileMetaInformation.DvtkDataFileMetaInformation;
                dvtkDataDicomFile.FileHead = this.fileMetaInformation.DvtkDataFileHead;

                return (dvtkDataDicomFile);
            }
        }
	
		/// <summary>
		/// Gets or sets the File Meta Information.
		/// </summary>
		public FileMetaInformation FileMetaInformation
		{
			get
			{
				return(this.fileMetaInformation);
			}
			set
			{
				this.fileMetaInformation = value;
			}
		}

        /// <summary>
        /// Gets or sets a boolean indicating if OB, OF and OW values will be available after
        /// calling the Read method.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// <br></br><br></br>
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
		/// Gets a summary of the DicomFile.
		/// </summary>
		/// <returns>The summary.</returns>
		public String Summary
		{
			get
			{
				String returnValue = String.Empty;

				returnValue+= "Media Storage SOP Class UID: " + this.fileMetaInformation.MediaStorageSOPClassUID + ".\r\n";
				returnValue+= "Media Storage SOP Instance UID: " + this.fileMetaInformation.MediaStorageSOPInstanceUID + ".\r\n";
				returnValue+= "Transfer syntax UID: " + this.fileMetaInformation.TransferSyntax + ".\r\n";
				returnValue+= "FileMetaInformation contains " + this.fileMetaInformation.Count.ToString() + " attributes.\r\n";
				returnValue+= "DataSet contains " + this.dataSet.Count.ToString() + " attributes.\r\n";

				return(returnValue);
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
        /// Creates an Dvtk.Sessions.ScriptSession instance in which no definiton files are loaded.
		/// </summary>
        /// <returns>An Dvtk.Sessions.ScriptSession instance.</returns>
		private Dvtk.Sessions.ScriptSession CreateEmptyDvtkScriptSession()
		{
			Dvtk.Sessions.ScriptSession dvtkScriptSession = new Dvtk.Sessions.ScriptSession();

			String tempPath = Path.GetTempPath();
			
			dvtkScriptSession.StorageMode = Dvtk.Sessions.StorageMode.AsMedia;
			dvtkScriptSession.DataDirectory = tempPath;
			dvtkScriptSession.ResultsRootDirectory = tempPath;
			
			return(dvtkScriptSession);
		}

		/// <summary>
		/// Returns the content of this instance as a String using Visual Basic notation.
		/// </summary>
		/// <param name="objectName">Name of the DicomFile variable to display.</param>
        /// <returns>The content of this instance as a String using Visual Basic notation.</returns>
		public String DumpUsingVisualBasicNotation(String objectName)
		{
			String dump = "";

			dump+= this.fileMetaInformation.DumpUsingVisualBasicNotation(objectName + ".FileMetaInformation");
			dump+= this.dataSet.DumpUsingVisualBasicNotation(objectName + ".DataSet");

			return(dump);
		}

		/// <summary>
		/// Reads a file.
		/// </summary>
        /// <remarks>
        /// A new FileMetaInformation and DataSet object will be created inside this object. They will be
        /// filled with the content of the specified file.
        /// The FileMetaInformation and DataSet object previously used will not change (they will not be
        /// used anymore by this object) and can still be used outside this object if needed.
        /// <br></br><br></br>
        /// NOTE:
        /// The intention of this method is to use only the supplied definition files.
        /// The current implementation however uses all already loaded definition files outside this 
        /// method and the supplied definition files!
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
		///			'Example: Read a specified DICOM file
		///			
        ///			Dim myDicomFile As DvtkHighLevelInterface.Dicom.Files.DicomFile 
		///			
		///			If File.Exists("") Then
        ///				Try
		///					myDicomFile.Read("c:\Somefile.dcm")
        ///   			 	Catch ex As DvtkHighLevelInterface.Common.Other.HliException
		///					' Error reading the file, Maybe the file format is wrong?
        ///    			End Try
		///			End If
		///		</code>
		/// </example>	
		public void Read(String fullFileName, params String[] definitionFilesFullName)
		{
			// Create a new dvtk session to read the DicomFile.
			Dvtk.Sessions.ScriptSession dvtkScriptSession = CreateEmptyDvtkScriptSession();

			// Add the definition files to this dvtk session.
			foreach(String definitionFileFullName in definitionFilesFullName)
			{
				dvtkScriptSession.DefinitionManagement.LoadDefinitionFile(definitionFileFullName);
			}

			Read(fullFileName, dvtkScriptSession);

			// Remove the definition files again to make sure the singleton doesn't keep them in memory.
			foreach(String definitionFileFullName in definitionFilesFullName)
			{
				dvtkScriptSession.DefinitionManagement.UnLoadDefinitionFile(definitionFileFullName);
			}
		}

		/// <summary>
		/// Reads a file.
		/// </summary>
        /// <remarks>
        /// A new FileMetaInformation and DataSet object will be created inside this object. They will be
        /// filled with the content of the specified file.
        /// The FileMetaInformation and DataSet object previously used will not change (they will not be
        /// used anymore by this object) and can still be used outside this object if needed.
        /// <br></br><br></br>
        /// NOTE:<br></br>
        /// The intention of this method is to use only the definition files loaded in the supplied
        /// DicomThread.
        /// The current implementation however uses all already loaded definition files outside this 
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
			Read(fullFileName, dicomThread.DvtkScriptSession);
		}

		/// <summary>
		/// Reads a file.
		/// </summary>
        /// <remarks>
        /// A new FileMetaInformation and DataSet object will be created inside this object. They will be
        /// filled with the content of the specified file.
        /// The FileMetaInformation and DataSet object previously used will not change (they will not be
        /// used anymore by this object) and can still be used outside this object if needed.
        /// 
        /// Also see properties UnVrDefinitionLookUpWhenReading and StoreOBOFOWValuesWhenReading.
        /// 
        /// If something goes wrong while reading the file, an exception is thrown.
        /// </remarks>
		/// <param name="fullFileName">The full file name.</param>
		/// <param name="dvtkScriptSession">The dvtk ScriptSession, from which the definition files to use for determining the attribute names are used.</param>
		private void Read(String fullFileName, Dvtk.Sessions.ScriptSession dvtkScriptSession)
		{
			// Create new FileMetaInformation and DataSet (depending on setting) objects.
			if (this.createNewDataSetObjectWhenReading)
			{
				this.dataSet = new DataSet();
			}

			// Throw an excpetion if the file doesn't exist.
			if (!File.Exists(fullFileName))
			{
				throw new HliException("File \"" + fullFileName + "\" not found.");
			}

			// Read the DicomFile using the supplied DicomThread.
			bool originalUnVrDefinitionLookUp = dvtkScriptSession.UnVrDefinitionLookUp;
			Dvtk.Sessions.StorageMode originalDvtkStorageMode = dvtkScriptSession.StorageMode;

			dvtkScriptSession.UnVrDefinitionLookUp = this.unVrDefinitionLookUpWhenReading;

			if (this.storeOBOFOWValuesWhenReading)
			{
				dvtkScriptSession.StorageMode = Dvtk.Sessions.StorageMode.AsDataSet;
			}
			else
			{
				dvtkScriptSession.StorageMode = Dvtk.Sessions.StorageMode.NoStorage;
			}

			DvtkData.Media.DicomFile dvtkDataDicomFile = dvtkScriptSession.ReadFile(fullFileName);

			if (dvtkDataDicomFile == null)
			{
				throw new HliException("Error while reading file \"" + fullFileName + "\".");
			}
			else
			{
				this.dataSet.DvtkDataDataSet = dvtkDataDicomFile.DataSet;
                this.FileMetaInformation = new FileMetaInformation(dvtkDataDicomFile.FileMetaInformation, dvtkDataDicomFile.FileHead);
			}

			dvtkScriptSession.UnVrDefinitionLookUp = originalUnVrDefinitionLookUp;
			dvtkScriptSession.StorageMode = originalDvtkStorageMode;
		}

        /// <summary>
        /// Adds a single attribute with the tag, VR and values specified.
        /// </summary>
        /// <remarks>
        /// Depending on the group number of the tag, the attribute
        /// is set in the FileMetaInformation or DataSet of this instance.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag, it is removed first before it is 
        /// again set.
        /// </remarks>
        /// <param name="dvtkDataTag">The tag that uniquely identifies the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="parameters">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead use
        /// multiple parameter arguments for this method when adding a single attribute with multiple values.
        /// </param>
		public void Set(DvtkData.Dimse.Tag dvtkDataTag, VR vR, params Object[] parameters)
		{
			TagSequence tagSequence = new TagSequence();

			tagSequence.Add(new Tag(dvtkDataTag.GroupNumber, dvtkDataTag.ElementNumber));

			Set(tagSequence, vR, parameters);
		}

        /// <summary>
        /// Adds a single attribute with the tag sequence string, VR and values specified.
        /// </summary>
        /// <remarks>
        /// Depending on the group number of the last tag in the tag sequence string, the attribute
        /// is set in the FileMetaInformation or DataSet of this instance.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag sequence string, it is removed first before it is 
        /// again set.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence string,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequenceString">The tag sequence string that uniquely identifies the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="parameters">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead use
        /// multiple parameter arguments for this method when adding a single attribute with multiple values.
        /// </param>
		public void Set(String tagSequenceString, VR vR, params Object[] parameters)
		{
			TagSequence tagSequence = new TagSequence(tagSequenceString);

			Set(tagSequence, vR, parameters);
		}

        /// <summary>
        /// Adds a single attribute with the tag sequence, VR and values specified.
        /// </summary>
        /// <remarks>
        /// Depending on the group number of the last tag in the tag sequence, the attribute
        /// is set in the FileMetaInformation or DataSet of this instance.
        /// <br></br><br></br>
        /// If an attribute already exists with this tag sequence, it is removed first before it is 
        /// again set.
        /// <br></br><br></br>
        /// If sequence items (each with a sequence item index) are specified in the tag sequence,
        /// empty sequence items will be added automatically to avoid gaps in the sequence items of sequence
        /// attributes.
        /// </remarks>
        /// <param name="tagSequence">The tag sequence that uniquely identifies the attribute.</param>
        /// <param name="vR">The VR of the attribute.</param>
        /// <param name="parameters">
        /// The values of the attribute. Do not use the DICOM delimeter '\' directly. Instead use
        /// multiple parameter arguments for this method when adding a single attribute with multiple values.
        /// </param>
        internal void Set(TagSequence tagSequence, VR vR, params Object[] parameters)
		{
			// Check if the TagSequence supplied uniquely identifies one attribute.
			if (!tagSequence.IsSingleAttributeMatching)
			{
				throw new HliException(tagSequence.ToString() + " not valid for setting an attribute.");
			}

			if (tagSequence.IsValidForFileMetaInformation)
			{
				FileMetaInformation.Set(tagSequence, vR, parameters);
			}
			else if (tagSequence.IsValidForDataSet)
			{
				DataSet.Set(tagSequence, vR, parameters);
			}
			else
			{
				throw new HliException(tagSequence.ToString() + " not valid for setting a DicomFile attribute.");
			}
		}

		/// <summary>
		/// Writes a Dicom file.
		/// </summary>
        /// <remarks>
        /// The group length attribute (0002,0000) will be set with the correct value
        /// automatically before writing to file begins.
        /// </remarks>
        /// <exception cref="HliException">
        ///	Writing to the file fails.
        /// </exception> 
        /// <param name="fullFileName">The full file name.</param>
		/// <example>
		///		<b>VB .NET</b>
		///		<code>
		///			' Example: Read a specified DICOM file and 
		///			'write it to back to a DICOM file 
		///			
        ///			Dim myDicomFile As DvtkHighLevelInterface.Dicom.Files.DicomFile
		///			
		///			If File.Exists("c:\somefile.dcm") Then
        ///				Try
		///					myDicomFile.Read("c:\Somefile.dcm")
        ///   			 	Catch ex As DvtkHighLevelInterface.Common.Other.HliException
		///					' Error reading the file, Maybe the file format is wrong?
        ///    			End Try
		///				'here is where you can manipulate the file
		///				'write the dataset to a file
		///				myDicomFile.Write("c:\newfile.dcm")
		///			End If
		///		</code>
		/// </example>
		public void Write(String fullFileName)
		{

			if (FileMetaInformation.TransferSyntax == "")
			{
				FileMetaInformation.TransferSyntax = "1.2.840.10008.1.2.1";
			}

			Dvtk.Sessions.ScriptSession dvtkScriptSession = CreateEmptyDvtkScriptSession();

			if (this.AddGroupLength)
				dvtkScriptSession.AddGroupLength = false;

			DvtkData.Media.DicomFile dvtkDataDicomFile = new DvtkData.Media.DicomFile();

			dvtkDataDicomFile.DataSet = this.dataSet.DvtkDataDataSet;
			dvtkDataDicomFile.FileMetaInformation = this.fileMetaInformation.DvtkDataFileMetaInformation;
			dvtkDataDicomFile.FileHead = this.fileMetaInformation.DvtkDataFileHead;

			if (!dvtkScriptSession.WriteFile(dvtkDataDicomFile, fullFileName))
			{
				throw new HliException("Error while writing file \"" + fullFileName + "\".");
			}
		}
	}
}
