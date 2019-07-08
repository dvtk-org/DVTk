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
using System.IO;
using System.Text;

using DvtkData.Dimse;

namespace Dvtk.Dicom.InformationEntity.QueryRetrieve
{
    public abstract class QueryRetrieveInformationModel : BaseInformationModel
    {
        public QueryRetrieveInformationModel(System.String name)
            : base(name)
        {
            // Do nothing.
        }

        public abstract void AddToInformationModel(DvtkData.Media.DicomFile dicomFile, bool storeFile);

        /// <summary>
        /// Load the Information Model by reading all the .DCM and .RAW files
        /// present in the given directory. The data read is normalised into the
        /// Information Model.
        /// </summary>
        /// <param name="dataDirectory">Source data directory containing the .DCm and .RAW files.</param>
        public override void LoadInformationModel(System.String dataDirectory)
        {
            DataDirectory = dataDirectory;
            DirectoryInfo directoryInfo = new DirectoryInfo(DataDirectory);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if ((fileInfo.Extension.ToLower().Equals(".dcm")) ||
                    (fileInfo.Extension.ToLower().Equals(".raw")) ||
                    (fileInfo.Extension == "") || (fileInfo.Extension == null))
                {
                    try
                    {
                        // read the DCM file

                        Dvtk.Sessions.ScriptSession dvtkScriptSession = new Dvtk.Sessions.ScriptSession();

                        String tempPath = Path.GetTempPath();

                        dvtkScriptSession.StorageMode = Dvtk.Sessions.StorageMode.AsMedia;
                        dvtkScriptSession.DataDirectory = tempPath;
                        dvtkScriptSession.ResultsRootDirectory = tempPath;

                        DvtkData.Media.DicomFile dvtkDataDicomFile = dvtkScriptSession.ReadFile(fileInfo.FullName);

                        // Add DICOM file to Information Model - but do not re-save in file
                        AddToInformationModel(dvtkDataDicomFile, false);
                    }
                    catch (Exception)
                    {
                        //Invalid DICOM File - will be skiped from QR information model.
                    }
                }
            }
        }

        /// <summary>
        /// Store the DICOM file into a DICOM Media File in the current Information Model directory.
        /// The file contents might be later used for retrieval.The transfer syntax is also passed so that the TS specified
        /// in the dcm file is used while moving the file to a Storage SCP
        /// </summary>
        /// <param name="dicomFile">The DICOM file to store</param>
        protected void StoreDicomFile(DvtkData.Media.DicomFile dicomFile)
        {
            System.String iom = System.String.Empty;
            if (Name.Equals("PatientRootInformationModel"))
            {
                iom = "PR";
            }
            else if (Name.Equals("StudyRootInformationModel"))
            {
                iom = "SR";
            }
            else if (Name.Equals("PatientStudyOnlyInformationModel"))
            {
                iom = "PS";
            }

            // generate a filename
            System.String filename;
            filename = System.String.Format("{0}\\DVTK_IOM_TMP_{1}_{2:00000000}.DCM", DataDirectory, iom, _fileIndex++);
            DvtkData.Media.DicomFile dicomMediaFile = new DvtkData.Media.DicomFile();

            // set up the file head
            DvtkData.Media.FileHead FileHead = new DvtkData.Media.FileHead();

            // add the Transfer Syntax UID
            //DvtkData.Dul.TransferSyntax transferSyntax = new DvtkData.Dul.TransferSyntax(DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian.UID);
            FileHead.TransferSyntax = dicomFile.FileHead.TransferSyntax;

            // set up the file meta information
            DvtkData.Media.FileMetaInformation fileMetaInformation = new DvtkData.Media.FileMetaInformation();

            // add the FMI version
            fileMetaInformation.AddAttribute(Tag.FILE_META_INFORMATION_VERSION.GroupNumber,
                Tag.FILE_META_INFORMATION_VERSION.ElementNumber, VR.OB, 1, 2);

            // add the SOP Class UID
            System.String sopClassUid = "";

            DvtkData.Dimse.Attribute attribute = dicomFile.DataSet.GetAttribute(DvtkData.Dimse.Tag.SOP_CLASS_UID);
            if (attribute != null)
            {
                UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
                if (uniqueIdentifier.Values.Count > 0)
                {
                    sopClassUid = uniqueIdentifier.Values[0];
                }
            }
            fileMetaInformation.AddAttribute(Tag.MEDIA_STORAGE_SOP_CLASS_UID.GroupNumber,
                Tag.MEDIA_STORAGE_SOP_CLASS_UID.ElementNumber, VR.UI, sopClassUid);

            // add the SOP Instance UID
            System.String sopInstanceUid = "";
            attribute = dicomFile.DataSet.GetAttribute(DvtkData.Dimse.Tag.SOP_INSTANCE_UID);
            if (attribute != null)
            {
                UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
                if (uniqueIdentifier.Values.Count > 0)
                {
                    sopInstanceUid = uniqueIdentifier.Values[0];
                }
            }
            fileMetaInformation.AddAttribute(Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.GroupNumber,
                Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.ElementNumber, VR.UI, sopInstanceUid);

            // add the Transfer Syntax UID
            fileMetaInformation.AddAttribute(Tag.TRANSFER_SYNTAX_UID.GroupNumber,
                Tag.TRANSFER_SYNTAX_UID.ElementNumber, VR.UI, dicomFile.FileHead.TransferSyntax);

            // add the Implemenation Class UID
            DvtkData.Dimse.Attribute implentationUID = dicomFile.FileMetaInformation.GetAttribute(0x00020012);
            fileMetaInformation.AddAttribute(Tag.IMPLEMENTATION_CLASS_UID.GroupNumber,
                Tag.IMPLEMENTATION_CLASS_UID.ElementNumber, VR.UI, implentationUID);

            // add the Implementation Version Name
            DvtkData.Dimse.Attribute implementationVersion = dicomFile.FileMetaInformation.GetAttribute(0x00020013);
            fileMetaInformation.AddAttribute(Tag.IMPLEMENTATION_VERSION_NAME.GroupNumber,
                Tag.IMPLEMENTATION_VERSION_NAME.ElementNumber, VR.SH, implementationVersion);

            // set up the dicomMediaFile contents
            dicomMediaFile.FileHead = FileHead;
            dicomMediaFile.FileMetaInformation = fileMetaInformation;
            dicomMediaFile.DataSet = dicomFile.DataSet;

            // write the dicomMediaFile to file
            Dvtk.DvtkDataHelper.WriteDataSetToFile(dicomMediaFile, filename);

            // save the filename in the dataset
            dicomFile.DataSet.Filename = filename;
        }
    }
}
