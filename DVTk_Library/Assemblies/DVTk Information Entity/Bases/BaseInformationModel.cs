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
using DvtkData.Dimse;
using Dvtk.Dicom.InformationEntity.CompositeInfoModel;
using System.Collections.Generic;

namespace Dvtk.Dicom.InformationEntity
{
	/// <summary>
	/// Summary description for BaseInformationModel.
	/// </summary>
	public abstract class BaseInformationModel
	{
		private System.String _name;
		private BaseInformationEntityList _root = null;
		private System.String _dataDirectory;
        private bool _isDataStored;
		private DataSet _defaultDatasetNoOverWrite = null;
		private DataSet _defaultDatasetOverWrite = null;
		private DataSet _additionalDatasetNoOverWrite = null;
		private DataSet _additionalDatasetOverWrite = null;
		internal int _fileIndex = 1;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="name">Information Model Name.</param>
		public BaseInformationModel(System.String name)
		{
			_name = name;
			_root = new BaseInformationEntityList();
			_dataDirectory = ".";
            _isDataStored = true;
			_defaultDatasetNoOverWrite = new DataSet("Default Dataset - NoOverWrite");
			_defaultDatasetOverWrite = new DataSet("Default Dataset - OverWrite");
			_additionalDatasetNoOverWrite = new DataSet("Additional Dataset - NoOverWrite");
			_additionalDatasetOverWrite = new DataSet("Additional Dataset - OverWrite");
			_fileIndex = 1;
		}

		protected BaseInformationEntityList Root
		{
			get
			{
				return _root;
			}
		}

		/// <summary>
		/// Property - Data Directory.
		/// </summary>
		public System.String DataDirectory
		{
            get
            {
                return (_dataDirectory);
            }

			set
			{
				_dataDirectory = value;
			}
		}

        public String Name
        {
            get
            {
                return (_name);
            }
        }

        /// <summary>
        /// Property - Stored Data.
        /// </summary>
        public bool IsDataStored
        {
            set
            {
                _isDataStored = value;
            }
        }


   		/// <summary>
		/// Load the Information Model by reading all the .DCM and .RAW files
		/// present in the given directory. The data read is normalised into the
		/// Information Model.
		/// </summary>
		/// <param name="dataDirectory">Source data directory containing the .DCm and .RAW files.</param>
        public abstract void LoadInformationModel(System.String dataDirectory);

		/// <summary>
		/// Refresh the Information Model by re-loading the contents.
		/// </summary>
		public void RefreshInformationModel()
		{
			// re-load the information model only when data is stored temporarily
            if (_isDataStored)
            {
                _root = new BaseInformationEntityList();
                LoadInformationModel(_dataDirectory);
            }

			// apply any default and additional values
			// iterate of all Information Entities
			foreach (BaseInformationEntity baseInformationEntity in Root)
			{
				baseInformationEntity.AddDefaultAttributes(false, _defaultDatasetNoOverWrite);
				baseInformationEntity.AddDefaultAttributes(true, _defaultDatasetOverWrite);
				baseInformationEntity.AddAdditionalAttributes(false, _additionalDatasetNoOverWrite);
				baseInformationEntity.AddAdditionalAttributes(true, _additionalDatasetOverWrite);
			}
		}

		/// <summary>
		/// Add the given Dataset to the Information Model. The data is normalised into the Information Model.
		/// </summary>
		/// <param name="dataset">Dataset to add to Information Model.</param>
		/// <param name="storeDataset">Boolean indicating whether or not the dataset should also be stored to file for possible retrieval.</param>
        //public virtual void AddToInformationModel(DataSet dataset, bool storeDataset)
        //{
        //}

       /// <summary>
        /// Add the given Dataset to the Information Model. The data is normalised into the Information Model.
       /// </summary>
       /// <param name="dataSet">Dataset to add to the informartion model</param>
       /// <param name="transferSyntax">The transfer syntax specified in the dcm file</param>
       /// <param name="fMI">The File Meta Information of the dcm file.</param>
       /// <param name="storeFile">Boolean indicating whether the or not the data set should be stored.</param>
        //public virtual void AddToInformationModel(DvtkData.Media.DicomFile dicomFile, bool storeFile)
        //{
        //}

		/// <summary>
		/// Query the Information Model using the given Query Dataset.
		/// </summary>
		/// <param name="queryDataset">Query Dataset.</param>
		/// <returns>A collection of zero or more query reponse datasets.</returns>
		public abstract DataSetCollection QueryInformationModel(DataSet queryDataset);

        ///// <summary>
        ///// Store the dataset into a DICOM Media File in the current Information Model directory.
        ///// The file contents might be later used for retrieval.
        ///// </summary>
        ///// <param name="dataset">DICOM Dataset to store.</param>
        //protected void StoreDataset(DataSet dataset)
        //{
        //    System.String iom = System.String.Empty;
        //    if (_name.Equals("PatientRootInformationModel"))
        //    {
        //        iom = "PR";
        //    }
        //    else if (_name.Equals("StudyRootInformationModel"))
        //    {
        //        iom = "SR";
        //    } 
        //    else if (_name.Equals("PatientStudyOnlyInformationModel"))
        //    {
        //        iom = "PS";
        //    }

        //    // generate a filename
        //    System.String filename;
        //    filename = System.String.Format("{0}\\DVTK_IOM_TMP_{1}_{2:00000000}.DCM", _dataDirectory, iom, _fileIndex++);
        //    DvtkData.Media.DicomFile dicomMediaFile = new DvtkData.Media.DicomFile();

        //    // set up the file head
        //    DvtkData.Media.FileHead fileHead = new DvtkData.Media.FileHead();

        //    // add the Transfer Syntax UID
        //    DvtkData.Dul.TransferSyntax transferSyntax = new DvtkData.Dul.TransferSyntax(DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian.UID);
        //    fileHead.TransferSyntax = transferSyntax;

        //    // set up the file meta information
        //    DvtkData.Media.FileMetaInformation fileMetaInformation = new DvtkData.Media.FileMetaInformation();

        //    // add the FMI version
        //    fileMetaInformation.AddAttribute(Tag.FILE_META_INFORMATION_VERSION.GroupNumber,
        //        Tag.FILE_META_INFORMATION_VERSION.ElementNumber, VR.OB, 1, 2);

        //    // add the SOP Class UID
        //    System.String sopClassUid = "";
        //    DvtkData.Dimse.Attribute attribute = dataset.GetAttribute(DvtkData.Dimse.Tag.SOP_CLASS_UID);
        //    if (attribute != null)
        //    {
        //        UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
        //        if (uniqueIdentifier.Values.Count > 0)
        //        {
        //            sopClassUid = uniqueIdentifier.Values[0];
        //        }
        //    }
        //    fileMetaInformation.AddAttribute(Tag.MEDIA_STORAGE_SOP_CLASS_UID.GroupNumber,
        //        Tag.MEDIA_STORAGE_SOP_CLASS_UID.ElementNumber, VR.UI, sopClassUid);

        //    // add the SOP Instance UID
        //    System.String sopInstanceUid = "";
        //    attribute = dataset.GetAttribute(DvtkData.Dimse.Tag.SOP_INSTANCE_UID);
        //    if (attribute != null)
        //    {
        //        UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
        //        if (uniqueIdentifier.Values.Count > 0)
        //        {
        //            sopInstanceUid = uniqueIdentifier.Values[0];
        //        }
        //    }
        //    fileMetaInformation.AddAttribute(Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.GroupNumber,
        //        Tag.MEDIA_STORAGE_SOP_INSTANCE_UID.ElementNumber, VR.UI, sopInstanceUid);

        //    // add the Transfer Syntax UID
        //    fileMetaInformation.AddAttribute(Tag.TRANSFER_SYNTAX_UID.GroupNumber,
        //        Tag.TRANSFER_SYNTAX_UID.ElementNumber, VR.UI, DvtkData.Dul.TransferSyntax.Explicit_VR_Little_Endian.UID);

        //    // add the Implemenation Class UID
        //    fileMetaInformation.AddAttribute(Tag.IMPLEMENTATION_CLASS_UID.GroupNumber,
        //        Tag.IMPLEMENTATION_CLASS_UID.ElementNumber, VR.UI, "1.2.826.0.1.3680043.2.1545.1.2.1.7");

        //    // add the Implementation Version Name
        //    fileMetaInformation.AddAttribute(Tag.IMPLEMENTATION_VERSION_NAME.GroupNumber,
        //        Tag.IMPLEMENTATION_VERSION_NAME.ElementNumber, VR.SH, "dvt2.1");

        //    // set up the dicomMediaFile contents
        //    dicomMediaFile.FileHead = fileHead;
        //    dicomMediaFile.FileMetaInformation = fileMetaInformation;
        //    dicomMediaFile.DataSet = dataset;

        //    // write the dicomMediaFile to file
        //    Dvtk.DvtkDataHelper.WriteDataSetToFile(dicomMediaFile, filename);

        //    // save the filename in the dataset
        //    dataset.Filename = filename;
        //}


		/// <summary>
		/// Copy the default dataset attributes to the Information Entities in the Information
		/// Model that define them.
		/// </summary>
		/// <param name="overWriteExistingValue">Boolean to indicate whether any already existing value should be overwritten or not.</param>
		/// <param name="defaultDataset">Dataset containing all the default values.</param>
		public void AddDefaultAttributesToInformationModel(bool overWriteExistingValue, DataSet defaultDataset)
		{
			if (defaultDataset == null) return;

			// iterate of all Information Entities
			foreach (BaseInformationEntity baseInformationEntity in Root)
			{
				baseInformationEntity.AddDefaultAttributes(overWriteExistingValue, defaultDataset);
			}

			// save these default attributes - in case of refresh
			if (overWriteExistingValue == true)
			{
				foreach (DvtkData.Dimse.Attribute defaultAttribute in defaultDataset)
				{
					DvtkData.Dimse.Attribute lDefaultAttribute = _defaultDatasetOverWrite.GetAttribute(defaultAttribute.Tag);
					if (lDefaultAttribute == null)
					{
						_defaultDatasetOverWrite.Add(defaultAttribute);
					}
				}
			}
			else
			{
				foreach (DvtkData.Dimse.Attribute defaultAttribute in defaultDataset)
				{
					DvtkData.Dimse.Attribute lDefaultAttribute = _defaultDatasetNoOverWrite.GetAttribute(defaultAttribute.Tag);
					if (lDefaultAttribute == null)
					{
						_defaultDatasetNoOverWrite.Add(defaultAttribute);
					}
				}
			}
		}

		/// <summary>
		/// Add the attributes in this additional dataset to all Information Entities in the Information
		/// Model. 
		/// </summary>
		/// <param name="overWriteExistingValue">Boolean to indicate whether any already existing value should be overwritten or not.</param>
		/// <param name="additionalDataset">Dataset containing the additional values.</param>
		public void AddAdditionalAttributesToInformationModel(bool overWriteExistingValue, DataSet additionalDataset)
		{
			if (additionalDataset == null) return;

			// iterate of all Information Entities
			foreach (BaseInformationEntity baseInformationEntity in Root)
			{
				baseInformationEntity.AddAdditionalAttributes(overWriteExistingValue, additionalDataset);
			}

			// save these additional attributes - in case of refresh
			if (overWriteExistingValue == true)
			{
				foreach (DvtkData.Dimse.Attribute additionalAttribute in additionalDataset)
				{
					DvtkData.Dimse.Attribute lAdditionalAttribute = _additionalDatasetOverWrite.GetAttribute(additionalAttribute.Tag);
					if (lAdditionalAttribute == null)
					{
						_additionalDatasetOverWrite.Add(additionalAttribute);
					}
				}
			}
			else
			{
				foreach (DvtkData.Dimse.Attribute additionalAttribute in additionalDataset)
				{
					DvtkData.Dimse.Attribute lAdditionalAttribute = _additionalDatasetNoOverWrite.GetAttribute(additionalAttribute.Tag);
					if (lAdditionalAttribute == null)
					{
						_additionalDatasetNoOverWrite.Add(additionalAttribute);
					}
				}
			}
		}

		#region Transaction handlers
		/// <summary>
		/// Patient Registration request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient registration attributes.</param>
		public virtual void PatientRegistration(DvtkData.Dimse.DataSet dataset) {}

		/// <summary>
		/// Patient Update request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient update attributes.</param>
		public virtual void PatientUpdate(DvtkData.Dimse.DataSet dataset) {}

		/// <summary>
		/// Patient merge request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient merge attributes.</param>
		public virtual void PatientMerge(DvtkData.Dimse.DataSet dataset) {}

		/// <summary>
		/// Placer order management request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing placer order management attributes.</param>
		public virtual void PlacerOrderManagement(DvtkData.Dimse.DataSet dataset) {}
		#endregion Transaction handlers

		/// <summary>
		/// Display the Information Model to the Console - for debugging purposes.
		/// </summary>
		public void ConsoleDisplay()
		{
			Console.Write(Dump(""));
		}

		/// <summary>
		/// Dump the Information Model to a String - for debugging purposes.
		/// </summary>
		/// <returns></returns>
		public String Dump(String prefix)
		{
			String dumpString = "";

			dumpString+= prefix + String.Format("{0} - {1} entries\r\n", _name, Root.Count);

			int entry = 1;
			foreach (BaseInformationEntity baseInformationEntity in Root)
			{
				dumpString+= prefix + "   " + String.Format("Entry: {0}\r\n", entry++);
				dumpString+= baseInformationEntity.Dump(prefix + "   ");
			}

			return(dumpString);
		}
        /// <summary>
        /// returns the composite datamodel
        /// </summary>
        /// <returns>Returns the collection of composite data model</returns>
        public List<BaseCompositeInformationEntity> GetCompositeDataModel()
        {
            List<BaseCompositeInformationEntity> dataModel = new List<BaseCompositeInformationEntity>();
            foreach (BaseInformationEntity baseInformationEntity in Root)
            {
                dataModel.Add(baseInformationEntity.GetCompositeDataModel());
            }
            return (dataModel);
        }

        /// <summary>
        /// Checks the SQ item having value or not
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        internal bool IsSequenceHavingValue(SequenceItem items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                DvtkData.Dimse.Attribute attrib = items[i];
                if (attrib.ValueRepresentation == VR.SQ)
                {
                    foreach (SequenceItem s in ((SequenceOfItems)attrib.DicomValue).Sequence)
                    {
                        if (IsSequenceHavingValue(s))
                            return true;
                    }
                }
                else if ((attrib.Length != 0) && (attrib.Tag.ElementNumber != 0x0000))
                {
                    return true;
                }
            }
            return false;
        }
	}
}
