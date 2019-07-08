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
using DvtkData.Dimse;
using Dvtk.Dicom.InformationEntity;

namespace Dvtk.Dicom.InformationEntity.QueryRetrieve
{
	/// <summary>
	/// Summary description for StudyRootInformationModel.
	/// </summary>
    public class StudyRootInformationModel : QueryRetrieveInformationModel, ICommitInformationModel, IRetrieveInformationModel
	{
		/// <summary>
		/// Class Constructor.
		/// </summary>
		public StudyRootInformationModel() : base("StudyRootInformationModel") {}

		#region BaseInformationModel Overrides

        /// <summary>
        /// Add the given dataset present in a Dicom File to the Information Model. The data is normalised into the Information Model.
        /// </summary>
        /// <param name="dicomFile">The dicom File containing the dataset to be added.</param>
        /// <param name="storeFile">Boolean indicating whether the dataset should be stored or not.</param>
        public override void AddToInformationModel(DvtkData.Media.DicomFile dicomFile, bool storeFile)
        {
            // STUDY level
            PatientStudyInformationEntity patientStudyInformationEntity = null;

            this.IsDataStored = storeFile;

            // check if the patient/study IE is already in the studyRootList
            foreach (PatientStudyInformationEntity lPatientStudyInformationEntity in Root)
            {
                if (lPatientStudyInformationEntity.IsUniqueTagFoundIn(dicomFile.DataSet))
                {
                    patientStudyInformationEntity = lPatientStudyInformationEntity;
                    patientStudyInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
                    break;
                }
            }

            // patient/study IE is not already in the studyRootList
            if (patientStudyInformationEntity == null)
            {
                // create a new patient/study IE from the dataset and add to the studyRootList
                patientStudyInformationEntity = new PatientStudyInformationEntity();
                patientStudyInformationEntity.CopyFrom(dicomFile.DataSet);
                Root.Add(patientStudyInformationEntity);
            }

            // SERIES level
            SeriesInformationEntity seriesInformationEntity = null;

            // check if the series IE is already in the study IE children
            foreach (SeriesInformationEntity lSeriesInformationEntity in patientStudyInformationEntity.Children)
            {
                if (lSeriesInformationEntity.IsUniqueTagFoundIn(dicomFile.DataSet))
                {
                    seriesInformationEntity = lSeriesInformationEntity;
                    seriesInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
                    break;
                }
            }

            // series IE is not already in the study IE children
            if (seriesInformationEntity == null)
            {
                // create a new series IE from the dataset and add to the study IE children
                seriesInformationEntity = new SeriesInformationEntity();
                seriesInformationEntity.CopyFrom(dicomFile.DataSet);
                patientStudyInformationEntity.AddChild(seriesInformationEntity);
            }

            // IMAGE (Instance) level
            InstanceInformationEntity instanceInformationEntity = null;

            // check if the instance IE is already in the series IE children
            foreach (InstanceInformationEntity lInstanceInformationEntity in seriesInformationEntity.Children)
            {
                if (lInstanceInformationEntity.IsUniqueTagFoundIn(dicomFile.DataSet))
                {
                    instanceInformationEntity = lInstanceInformationEntity;
                    instanceInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
                    break;
                }
            }

            // instance IE is not already in the series IE children
            if (instanceInformationEntity == null)
            {
                // Store the dicom File as a DCM file if requested.
                if (storeFile == true)
                {
                    StoreDicomFile(dicomFile);
                }

                // create a new instance IE from the dataset and add to the series IE children
                instanceInformationEntity = new InstanceInformationEntity(dicomFile.DataSet.Filename);
                instanceInformationEntity.CopyFrom(dicomFile.DataSet);
                seriesInformationEntity.AddChild(instanceInformationEntity);
            }

            patientStudyInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
            seriesInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
            instanceInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
        }

		/// <summary>
		/// Query the Information Model using the given Query Dataset.
		/// </summary>
		/// <param name="queryDataset">Query Dataset.</param>
		/// <returns>A collection of zero or more query reponse datasets.</returns>
		public override DataSetCollection QueryInformationModel(DataSet queryDataset)
		{
			DataSetCollection queryResponses = new DataSetCollection();;

			// get the query/retrieve level
			String queryRetrieveLevel = "UNKNOWN";
			DvtkData.Dimse.Attribute queryRetrieveLevelAttribute = queryDataset.GetAttribute(Tag.QUERY_RETRIEVE_LEVEL);
			if (queryRetrieveLevelAttribute != null)
			{
				CodeString codeString = (CodeString)queryRetrieveLevelAttribute.DicomValue;
				if (codeString.Values.Count == 1)
				{
					queryRetrieveLevel = codeString.Values[0].Trim();
				}
			}

			// query at the STUDY level
			if (queryRetrieveLevel == "STUDY")
			{
				TagTypeList queryTagTypeList = new TagTypeList();
				TagTypeList returnTagTypeList = new TagTypeList();
				foreach (DvtkData.Dimse.Attribute attribute in queryDataset)
				{
                    if (attribute.ValueRepresentation == VR.SQ)
                    {
                        foreach (SequenceItem s in ((SequenceOfItems)attribute.DicomValue).Sequence)
                        {
                            if (IsSequenceHavingValue(s))
                            {
                                queryTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagRequired));
                            }
                        }
                    }

                    else if ((attribute.Length != 0) &&
						(attribute.Tag.ElementNumber != 0x0000))
					{
						queryTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagRequired));
					}
                    if(attribute.Tag!=Tag.SPECIFIC_CHARACTER_SET)
					    returnTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagOptional));
				}

				foreach (PatientStudyInformationEntity patientStudyInformationEntity in Root)
				{
					if (patientStudyInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
					{
						// STUDY level matches
						DataSet queryResponse = new DataSet();

						// if the specific character set attribute has been stored in the patient/study IE - return it in the query response
						DvtkData.Dimse.Attribute specificCharacterSetAttribute = patientStudyInformationEntity.GetSpecificCharacterSet();
						if (specificCharacterSetAttribute != null)
						{
                            queryResponse.Add(specificCharacterSetAttribute);
						}

						patientStudyInformationEntity.CopyTo(returnTagTypeList, queryResponse);
						queryResponses.Add(queryResponse);
					}
				}
			}
			else
			{
				// find the matching STUDY
				PatientStudyInformationEntity patientStudyInformationEntity = null;
				foreach (PatientStudyInformationEntity lPatientStudyInformationEntity in Root)
				{
					if (lPatientStudyInformationEntity.IsUniqueTagFoundIn(queryDataset))
					{
						patientStudyInformationEntity = lPatientStudyInformationEntity;
						break;
					}
				}

				if (patientStudyInformationEntity != null)
				{
					// query at the SERIES level
					if (queryRetrieveLevel == "SERIES")
					{
						TagTypeList queryTagTypeList = new TagTypeList();
						TagTypeList returnTagTypeList = new TagTypeList();
						foreach (DvtkData.Dimse.Attribute attribute in queryDataset)
						{
							// do not add higher level tags
							if (attribute.Tag == Tag.STUDY_INSTANCE_UID) continue;

                            if (attribute.ValueRepresentation == VR.SQ)
                            {
                                foreach (SequenceItem s in ((SequenceOfItems)attribute.DicomValue).Sequence)
                                {
                                    if (IsSequenceHavingValue(s))
                                    {
                                        queryTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagRequired));
                                    }
                                }
                            }

							else if ((attribute.Length != 0) &&
								(attribute.Tag.ElementNumber != 0x0000))
							{
								queryTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagRequired));
							}
							returnTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagOptional));
						}

						foreach (SeriesInformationEntity seriesInformationEntity in patientStudyInformationEntity.Children)
						{
							if (seriesInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
							{
								// SERIES level matches
								DataSet queryResponse = new DataSet();

								// if the specific character set attribute has been stored in the series IE - return it in the query response
								DvtkData.Dimse.Attribute specificCharacterSetAttribute = seriesInformationEntity.GetSpecificCharacterSet();
								if (specificCharacterSetAttribute != null)
								{
									queryResponse.Add(specificCharacterSetAttribute);
								}

								patientStudyInformationEntity.CopyUniqueTagTo(queryResponse);
								seriesInformationEntity.CopyTo(returnTagTypeList, queryResponse);
								queryResponses.Add(queryResponse);
							}
						}
					}
					else
					{
						// find the matching SERIES
						SeriesInformationEntity seriesInformationEntity = null;
						foreach (SeriesInformationEntity lSeriesInformationEntity in patientStudyInformationEntity.Children)
						{
							if (lSeriesInformationEntity.IsUniqueTagFoundIn(queryDataset))
							{
								seriesInformationEntity = lSeriesInformationEntity;
								break;
							}
						}

						if (seriesInformationEntity != null)
						{
							// query at the IMAGE level
							if (queryRetrieveLevel == "IMAGE")
							{
								TagTypeList queryTagTypeList = new TagTypeList();
								TagTypeList returnTagTypeList = new TagTypeList();
								foreach (DvtkData.Dimse.Attribute attribute in queryDataset)
								{
									// do not add higher level tags
									if ((attribute.Tag == Tag.STUDY_INSTANCE_UID) ||
										(attribute.Tag == Tag.SERIES_INSTANCE_UID)) continue;

                                    if (attribute.ValueRepresentation == VR.SQ)
                                    {
                                        foreach (SequenceItem s in ((SequenceOfItems)attribute.DicomValue).Sequence)
                                        {
                                            if (IsSequenceHavingValue(s))
                                            {
                                                queryTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagRequired));
                                            }
                                        }
                                    }
                                    else if ((attribute.Length != 0) &&
										(attribute.Tag.ElementNumber != 0x0000))
									{
										queryTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagRequired));
									}
									returnTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagOptional));
								}

								foreach (InstanceInformationEntity instanceInformationEntity in seriesInformationEntity.Children)
								{
									if (instanceInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
									{
										// IMAGE level matches
										DataSet queryResponse = new DataSet();

										// if the specific character set attribute has been stored in the instance IE - return it in the query response
										DvtkData.Dimse.Attribute specificCharacterSetAttribute = instanceInformationEntity.GetSpecificCharacterSet();
										if (specificCharacterSetAttribute != null)
										{
											queryResponse.Add(specificCharacterSetAttribute);
										}

										patientStudyInformationEntity.CopyUniqueTagTo(queryResponse);
										seriesInformationEntity.CopyUniqueTagTo(queryResponse);
										instanceInformationEntity.CopyTo(returnTagTypeList, queryResponse);
										queryResponses.Add(queryResponse);
									}
								}
							}
						}
					}
				}
			}

			return queryResponses;
		}
		#endregion

        

		#region ICommitInformationModel
		/// <summary>
		/// Check if the given instance is present in the Information Model. The instance will be at the leaf nodes of the Information Model.
		/// </summary>
		/// <param name="sopClassUid">SOP Class UID to search for.</param>
		/// <param name="sopInstanceUid">SOP Instance UID to search for.</param>
		/// <returns>Boolean - true if instance found in the Information Model, otherwise false.</returns>
		public bool IsInstanceInInformationModel(System.String sopClassUid, System.String sopInstanceUid)
		{
			bool isInstanceInInformationModel = false;

			// set up the commit tag list for comparing with the leaf
			TagTypeList commitTagTypeList = new TagTypeList();
			commitTagTypeList.Add(new TagType(Tag.SOP_INSTANCE_UID, TagTypeEnum.TagRequired));
			commitTagTypeList.Add(new TagType(Tag.SOP_CLASS_UID, TagTypeEnum.TagRequired));

			// set up the commit dataset
			DvtkData.Dimse.DataSet commitDataset = new DvtkData.Dimse.DataSet();
			DvtkData.Dimse.Attribute attribute = new DvtkData.Dimse.Attribute(0x00080016, DvtkData.Dimse.VR.UI, sopClassUid);
			commitDataset.Add(attribute);
			attribute = new DvtkData.Dimse.Attribute(0x00080018, DvtkData.Dimse.VR.UI, sopInstanceUid);
			commitDataset.Add(attribute);

			// iterate over the whole information model - we are interested in the leaf nodes
			foreach (PatientStudyInformationEntity patientStudyInformationEntity in Root)
			{
				foreach (SeriesInformationEntity seriesInformationEntity in patientStudyInformationEntity.Children)
				{
					foreach (InstanceInformationEntity instanceInformationEntity in seriesInformationEntity.Children)
					{
						if (instanceInformationEntity.IsFoundIn(commitTagTypeList, commitDataset))
						{
							// an instance has been found with the matching commit uids
							isInstanceInInformationModel = true;
							break;
						}
					}
				}
			}

			return isInstanceInInformationModel;
		}
		#endregion

		#region IRetrieveInformationModel
		/// <summary>
		/// Retrieve a list of filenames from the Information Model. The filenames match the
		/// individual instances matching the retrieve dataset attributes.
		/// </summary>
		/// <param name="retrieveDataset">Retrive dataset.</param>
		/// <returns>File list - containing the filenames of all instances matching the retrieve dataset attributes.</returns>
		public DvtkData.Collections.StringCollection RetrieveInformationModel(DataSet retrieveDataset)
		{
			DvtkData.Collections.StringCollection fileList = new DvtkData.Collections.StringCollection();

			// get the query/retrieve level
			String queryRetrieveLevel = "UNKNOWN";
			DvtkData.Dimse.Attribute queryRetrieveLevelAttribute = retrieveDataset.GetAttribute(Tag.QUERY_RETRIEVE_LEVEL);
			if (queryRetrieveLevelAttribute != null)
			{
				CodeString codeString = (CodeString)queryRetrieveLevelAttribute.DicomValue;
				if (codeString.Values.Count == 1)
				{
					queryRetrieveLevel = codeString.Values[0].Trim();
				}
			}

			// find the matching STUDY
            BaseInformationEntityList patientStudyInformationEntities = new BaseInformationEntityList();

			foreach (PatientStudyInformationEntity lPatientStudyInformationEntity in Root)
			{
				if (lPatientStudyInformationEntity.IsUniqueTagFoundIn(retrieveDataset))
				{
                    patientStudyInformationEntities.Add(lPatientStudyInformationEntity);
				}
			}

            if (patientStudyInformationEntities.Count > 0)
			{
				// retrieve at the STUDY level
				if (queryRetrieveLevel == "STUDY")
				{
                    foreach (PatientStudyInformationEntity patientStudyInformationEntity in patientStudyInformationEntities)
                    {
                        foreach (String fileName in patientStudyInformationEntity.FileNames)
                        {
                            fileList.Add(fileName);
                        }
                    }
				}
				else
				{
					// find the matching SERIES
                    BaseInformationEntityList seriesInformationEntities = patientStudyInformationEntities[0].ChildrenWithUniqueTagFoundIn(retrieveDataset);
                    
                    if (seriesInformationEntities.Count > 0)
					{
						// retrieve at the SERIES level
						if (queryRetrieveLevel == "SERIES")
						{
                            foreach (SeriesInformationEntity seriesInformationEntity in seriesInformationEntities)
                            {
                                foreach (String fileName in seriesInformationEntity.FileNames)
                                {
                                    fileList.Add(fileName);
                                }
                            }
						}
						else
						{
							// find the matching IMAGE
                            BaseInformationEntityList instanceInformationEntities = seriesInformationEntities[0].ChildrenWithUniqueTagFoundIn(retrieveDataset);

							// retrieve at the IMAGE level
							if ((instanceInformationEntities.Count > 0) &&
								(queryRetrieveLevel == "IMAGE"))
							{
                                foreach (InstanceInformationEntity instanceInformationEntity in instanceInformationEntities)
                                {
                                    fileList.Add(instanceInformationEntity.Filename);
                                }
							}
						}
					}
				}
			}

			return fileList;
		}
		#endregion
	}
}
