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

using DvtkHighLevelInterface.Dicom.Other;



namespace DvtkHighLevelInterface.Dicom.Files
{
    /// <summary>
    /// Under construction!
    /// </summary>
    internal class DicomDir
    {
        //
        // - Fields -
        //

        /// <summary>
        /// See property DataSet.
        /// </summary>
        private DataSet dataSet = new DataSet();

        /// <summary>
        /// See property FileMetaInformation.
        /// </summary>
        private FileMetaInformation fileMetaInformation = new FileMetaInformation();



        //
        // - Constructors -
        //

        /// <summary>
        /// Default constructor. Creates an empty DicomDir.
        /// </summary>
        public DicomDir()
        {

        }



        //
        // - Properties -
        //

        /// <summary>
        /// Gets or sets the Data Set.
        /// </summary>
        public DataSet DataSet
        {
            get
            {
                return (this.dataSet);
            }
            set
            {
                this.dataSet = value;
            }
        }

        /// <summary>
        /// Gets or sets the File Meta Information.
        /// </summary>
        public FileMetaInformation FileMetaInformation
        {
            get
            {
                return (this.fileMetaInformation);
            }
            set
            {
                this.fileMetaInformation = value;
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Adds the mandatory DICOMDIR records for the supplied DICOM file.
        /// </summary>
        /// <remarks>
        /// Only the mandatory attributes will be present in the added DICOMDIR records.
        /// </remarks>
        /// <param name="fullFileName">The full name of the DICOM file.</param>
        /// <param name="referencedFileId">The different components (values) of the "referenced file ID" attribute.</param>
        public void AddDicomDirRecordsForDicomFile(String fullFileName, params String[] referencedFileId)
        {

        }

        /// <summary>
        /// Reads a DICOM directory.
        /// </summary>
        /// <param name="fullFileName">The full file name</param>
        public void Read(String fullFileName)
        {
            // TODO DvtkData.Media.DicomDir dvtkDataDicomDir = ReadDicomdir(fullFileName);

            // TODO this.fileMetaInformation = new FileMetaInformation(dvtkDataDicomDir.FileMetaInformation, dvtkDataDicomDir.FileHead);


            // TODO: how to use the DataSet and DirectoryRecords property of the encapsulated dvtkDataDicomDir?
        }

        /// <summary>
        /// Writes a DICOM directory.
        /// </summary>
        /// <param name="fullFileName">The full file name</param>
        public void Write(String fullFileName)
        {
            DvtkData.Media.DicomDir dvtkDataDicomDir = new DvtkData.Media.DicomDir();

            // In the DvtkData assembly, the file header is not part of the file meta information.
            // In the HLI assembly, the file header is part of the file meta information, just like
            // the DICOM standard.
            dvtkDataDicomDir.FileMetaInformation = this.FileMetaInformation.DvtkDataFileMetaInformation;
            dvtkDataDicomDir.FileHead = this.FileMetaInformation.DvtkDataFileHead;


            // TODO: How should the DataSet and DirectoryRecords proporties be used?
           

            // TODO WriteDicomDir(dvtkDataDicomDir, fullFileName);
        }
    }
}
