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
using DvtkData.Collections;
using DvtkData.Dimse;
using Dvtk.Dicom.InformationEntity;

namespace Dvtk.Dicom.InformationEntity.QueryRetrieve
{
	/// <summary>
	/// Summary description for PatientRootInformationModel.
	/// </summary>
    public class PatientRootInformationModel : QueryRetrieveInformationModel, ICommitInformationModel, IRetrieveInformationModel
	{
		/// <summary>
		/// Class Constructor.
		/// </summary>
		public PatientRootInformationModel() : base("PatientRootInformationModel") {}

		#region BaseInformationModel Overrides

        /// <summary>
        /// Add the given dataset present in a Dicom File to the Information Model. The data is normalised into the Information Model.
        /// </summary>
        /// <param name="dicomFile">The dicom File containing the dataset to be added.</param>
        /// <param name="storeFile">Boolean indicating whether the dataset should be stored or not.</param>
        public override void AddToInformationModel(DvtkData.Media.DicomFile dicomFile, bool storeFile)
        {
            // PATIENT level
            PatientInformationEntity patientInformationEntity = null;

            this.IsDataStored = storeFile;

            // check if the patient IE is already in the patientRootList
            foreach (PatientInformationEntity lPatientInformationEntity in Root)
            {
                if (lPatientInformationEntity.IsUniqueTagFoundIn(dicomFile.DataSet))
                {
                    patientInformationEntity = lPatientInformationEntity;
                    //patientInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
                    break;
                }
            }

            // patient IE is not already in the patientRootList
            if (patientInformationEntity == null)
            {
                // create a new patient IE from the dataset and add to the patientRootList
                patientInformationEntity = new PatientInformationEntity();
                patientInformationEntity.CopyFrom(dicomFile.DataSet);
                Root.Add(patientInformationEntity);
            }

            // STUDY level
            StudyInformationEntity studyInformationEntity = null;

            // check if the study IE is already in the patient IE children
            foreach (StudyInformationEntity lStudyInformationEntity in patientInformationEntity.Children)
            {
                if (lStudyInformationEntity.IsUniqueTagFoundIn(dicomFile.DataSet))
                {
                    studyInformationEntity = lStudyInformationEntity;
                    //studyInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
                    break;
                }
            }

            // study IE is not already in the patient IE children
            if (studyInformationEntity == null)
            {
                // create a new study IE from the dataset and add to the patient IE children
                studyInformationEntity = new StudyInformationEntity();
                studyInformationEntity.CopyFrom(dicomFile.DataSet);
                patientInformationEntity.AddChild(studyInformationEntity);
            }

            // SERIES level
            SeriesInformationEntity seriesInformationEntity = null;

            // check if the series IE is already in the study IE children
            foreach (SeriesInformationEntity lSeriesInformationEntity in studyInformationEntity.Children)
            {
                if (lSeriesInformationEntity.IsUniqueTagFoundIn(dicomFile.DataSet))
                {
                    seriesInformationEntity = lSeriesInformationEntity;
                    //seriesInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
                    break;
                }
            }

            // series IE is not already in the study IE children
            if (seriesInformationEntity == null)
            {
                // create a new series IE from the dataset and add to the study IE children
                seriesInformationEntity = new SeriesInformationEntity();
                seriesInformationEntity.CopyFrom(dicomFile.DataSet);
                studyInformationEntity.AddChild(seriesInformationEntity);
            }

            // IMAGE (Instance) level
            InstanceInformationEntity instanceInformationEntity = null;

            // check if the instance IE is already in the series IE children
            foreach (InstanceInformationEntity lInstanceInformationEntity in seriesInformationEntity.Children)
            {
                if (lInstanceInformationEntity.IsUniqueTagFoundIn(dicomFile.DataSet))
                {
                    instanceInformationEntity = lInstanceInformationEntity;
                    //instanceInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
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
            patientInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
            studyInformationEntity.CheckForSpecialTags(dicomFile.DataSet);
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
			DataSetCollection queryResponses = new DataSetCollection();

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

			// query at the PATIENT level
			if (queryRetrieveLevel == "PATIENT")
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
                    if (attribute.Tag != Tag.SPECIFIC_CHARACTER_SET)
					    returnTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagOptional));
				}

				foreach (PatientInformationEntity patientInformationEntity in Root)
				{
					if (patientInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
					{
						// PATIENT level matches
						DataSet queryResponse = new DataSet();

						// if the specific character set attribute has been stored in the patient IE - return it in the query response
						DvtkData.Dimse.Attribute specificCharacterSetAttribute = patientInformationEntity.GetSpecificCharacterSet();
						if (specificCharacterSetAttribute != null)
						{
							queryResponse.Add(specificCharacterSetAttribute);
						}

						patientInformationEntity.CopyTo(returnTagTypeList, queryResponse);
						patientInformationEntity.CopyAdditionalAttributes(queryResponse);
						queryResponses.Add(queryResponse);
					}
				}
			}
			else
			{
				// find the matching PATIENT
				PatientInformationEntity patientInformationEntity = null;
				foreach (PatientInformationEntity lPatientInformationEntity in Root)
				{
					if (lPatientInformationEntity.IsUniqueTagFoundIn(queryDataset))
					{
						patientInformationEntity = lPatientInformationEntity;
						break;
					}
				}
				if (patientInformationEntity != null)
				{
					// query at the STUDY level
					if (queryRetrieveLevel == "STUDY")
					{
						TagTypeList queryTagTypeList = new TagTypeList();
						TagTypeList returnTagTypeList = new TagTypeList();
                        foreach (DvtkData.Dimse.Attribute attribute in queryDataset)
                        {
                            // do not add higher level tag
                            if (attribute.Tag == Tag.PATIENT_ID) continue;

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

						foreach (StudyInformationEntity studyInformationEntity in patientInformationEntity.Children)
						{
							if (studyInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
							{
								// STUDY level matches
								DataSet queryResponse = new DataSet();

								// if the specific character set attribute has been stored in the study IE - return it in the query response
								DvtkData.Dimse.Attribute specificCharacterSetAttribute = studyInformationEntity.GetSpecificCharacterSet();
								if (specificCharacterSetAttribute != null)
								{
									queryResponse.Add(specificCharacterSetAttribute);
								}

								patientInformationEntity.CopyUniqueTagTo(queryResponse);
								studyInformationEntity.CopyTo(returnTagTypeList, queryResponse);
								studyInformationEntity.CopyAdditionalAttributes(queryResponse);
								queryResponses.Add(queryResponse);
							}
						}
					}
					else
					{
						// find the matching STUDY
						StudyInformationEntity studyInformationEntity = null;
						foreach (StudyInformationEntity lStudyInformationEntity in patientInformationEntity.Children)
						{
							if (lStudyInformationEntity.IsUniqueTagFoundIn(queryDataset))
							{
								studyInformationEntity = lStudyInformationEntity;
								break;
							}
						}

						if (studyInformationEntity != null)
						{
							// query at the SERIES level
							if (queryRetrieveLevel == "SERIES")
							{
								TagTypeList queryTagTypeList = new TagTypeList();
								TagTypeList returnTagTypeList = new TagTypeList();
                                foreach (DvtkData.Dimse.Attribute attribute in queryDataset)
                                {
                                    // do not add higher level tags
                                    if ((attribute.Tag == Tag.PATIENT_ID) ||
                                        (attribute.Tag == Tag.STUDY_INSTANCE_UID)) continue;

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

								foreach (SeriesInformationEntity seriesInformationEntity in studyInformationEntity.Children)
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

										patientInformationEntity.CopyUniqueTagTo(queryResponse);
										studyInformationEntity.CopyUniqueTagTo(queryResponse);
										seriesInformationEntity.CopyTo(returnTagTypeList, queryResponse);
										seriesInformationEntity.CopyAdditionalAttributes(queryResponse);
										queryResponses.Add(queryResponse);
									}
								}
							}
							else
							{
								// find the matching SERIES
								SeriesInformationEntity seriesInformationEntity = null;
								foreach (SeriesInformationEntity lSeriesInformationEntity in studyInformationEntity.Children)
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
                                            if ((attribute.Tag == Tag.PATIENT_ID) ||
                                                (attribute.Tag == Tag.STUDY_INSTANCE_UID) ||
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

												patientInformationEntity.CopyUniqueTagTo(queryResponse);
												studyInformationEntity.CopyUniqueTagTo(queryResponse);
												seriesInformationEntity.CopyUniqueTagTo(queryResponse);
												instanceInformationEntity.CopyTo(returnTagTypeList, queryResponse);
												instanceInformationEntity.CopyAdditionalAttributes(queryResponse);
												queryResponses.Add(queryResponse);
											}
										}
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
			foreach (PatientInformationEntity patientInformationEntity in Root)
			{
				foreach (StudyInformationEntity studyInformationEntity in patientInformationEntity.Children)
				{
					foreach (SeriesInformationEntity seriesInformationEntity in studyInformationEntity.Children)
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

			// Find the matching PATIENT. 
			PatientInformationEntity patientInformationEntity = null;
			foreach (PatientInformationEntity lPatientInformationEntity in Root)
			{
				if (lPatientInformationEntity.IsUniqueTagFoundIn(retrieveDataset))
				{
					patientInformationEntity = lPatientInformationEntity;
					break;
				}
			}

			if (patientInformationEntity != null)
			{
				// retrieve at the PATIENT level
				if (queryRetrieveLevel == "PATIENT")
				{
                    fileList = patientInformationEntity.FileNames;
				}
				else
				{
					// Find the matching STUDIES.
                    BaseInformationEntityList studyInformationEntities = patientInformationEntity.ChildrenWithUniqueTagFoundIn(retrieveDataset);

                    if (studyInformationEntities.Count > 0)
					{
						// Retrieve at the STUDY level
						if (queryRetrieveLevel == "STUDY")
						{
                            foreach (StudyInformationEntity studyInformationEntity in studyInformationEntities)
                            {
                                foreach (String fileName in studyInformationEntity.FileNames)
                                {
                                    fileList.Add(fileName);
                                }
                            }
						}
						else
						{
							// Find the matching SERIES.
                            BaseInformationEntityList seriesInformationEntities = studyInformationEntities[0].ChildrenWithUniqueTagFoundIn(retrieveDataset);

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
									// Find the matching IMAGE
                                    BaseInformationEntityList instanceInformationEntities = seriesInformationEntities[0].ChildrenWithUniqueTagFoundIn(retrieveDataset);
                                    
									// retrieve at the IMAGE level
                                    if ((instanceInformationEntities.Count > 0) &&
										(queryRetrieveLevel == "IMAGE"))
									{
                                        foreach (InstanceInformationEntity instanceInformationEntity in instanceInformationEntities)
                                        {
                                            if (instanceInformationEntity.Filename != null)
                                                fileList.Add(instanceInformationEntity.Filename);
                                        }
									}
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
