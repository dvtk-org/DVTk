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
using Dvtk.Dicom.InformationEntity;

namespace Dvtk.Dicom.InformationEntity.Worklist
{
	/// <summary>
	/// Summary description for ModalityWorklistInformationModel.
	/// </summary>
	public class ModalityWorklistInformationModel : BaseInformationModel
	{
		/// <summary>
		/// Class Constructor.
		/// </summary>
		public ModalityWorklistInformationModel() : base("ModalityWorklistInformationModel") {}

		#region BaseInformationModel Overrides
		/// <summary>
		/// Add the given Dataset to the Information Model. The data is normalised into the Information Model.
		/// </summary>
		/// <param name="dataset">Dataset to add to Informatio Model.</param>
		/// <param name="storeDataset">Boolean indicating whether or not the dataset should also be stored to file for possible retrieval.</param>
		public void AddToInformationModel(DataSet dataset, bool storeDataset)
		{
			// PATIENT level
			PatientInformationEntity patientInformationEntity = null;

			// check if the patient IE is already in the patientRootList
			foreach (PatientInformationEntity lPatientInformationEntity in Root)
			{
				if (lPatientInformationEntity.IsFoundIn(dataset))
				{
					patientInformationEntity = lPatientInformationEntity;
					break;
				}
			}

			// patient IE is not already in the patientRootList
			if (patientInformationEntity == null)
			{
				// create a new patient IE from the dataset and add to the patientRootList
				patientInformationEntity = new PatientInformationEntity();
				patientInformationEntity.CopyFrom(dataset);
				//Root.Add(patientInformationEntity);

                // Modified by RB 20090128 - when handling an order scheduled event from an actor
                // we want to insert the order as the first entry in the information model so that
                // it is returned as the first entry in the worklist query
                Root.Insert(0, patientInformationEntity); 
            }

			// VISIT level
			VisitInformationEntity visitInformationEntity = null;

			// check if the visit IE is already in the patient IE children
			foreach (VisitInformationEntity lVisitInformationEntity in patientInformationEntity.Children)
			{
				if (lVisitInformationEntity.IsFoundIn(dataset))
				{
					visitInformationEntity = lVisitInformationEntity;
					break;
				}
			}

			// visit IE is not already in the patient IE children
			if (visitInformationEntity == null)
			{
				// create a new visit IE from the dataset and add to the patient IE children
				visitInformationEntity = new VisitInformationEntity();
				visitInformationEntity.CopyFrom(dataset);
				patientInformationEntity.AddChild(visitInformationEntity);
			}

			// IMAGING SERVICE REQUEST level
			ImagingServiceRequestInformationEntity imagingServiceRequestInformationEntity = null;

			// check if the imaging service request IE is already in the visit IE children
			foreach (ImagingServiceRequestInformationEntity lImagingServiceRequestInformationEntity in visitInformationEntity.Children)
			{
				if (lImagingServiceRequestInformationEntity.IsFoundIn(dataset))
				{
					imagingServiceRequestInformationEntity = lImagingServiceRequestInformationEntity;
					break;
				}
			}

			// imaging service request IE is not already in the visit IE children
			if (imagingServiceRequestInformationEntity == null)
			{
				// create a new imaging service request IE from the dataset and add to the visit IE children
				imagingServiceRequestInformationEntity = new ImagingServiceRequestInformationEntity();
				imagingServiceRequestInformationEntity.CopyFrom(dataset);
				visitInformationEntity.AddChild(imagingServiceRequestInformationEntity);
			}

			// REQUESTED PROCEDURE level
			RequestedProcedureInformationEntity requestedProcedureInformationEntity = null;

			// check if the requested procedure IE is already in the imaging service request IE children
			foreach (RequestedProcedureInformationEntity lRequestedProcedureInformationEntity in imagingServiceRequestInformationEntity.Children)
			{
				if (lRequestedProcedureInformationEntity.IsFoundIn(dataset))
				{
					requestedProcedureInformationEntity = lRequestedProcedureInformationEntity;
					break;
				}
			}

			// requested procedure IE is not already in the imaging service request IE children
			if (requestedProcedureInformationEntity == null)
			{
				// create a new requested procedure IE from the dataset and add to the imaging service request IE children
				requestedProcedureInformationEntity = new RequestedProcedureInformationEntity();
				requestedProcedureInformationEntity.CopyFrom(dataset);
				imagingServiceRequestInformationEntity.AddChild(requestedProcedureInformationEntity);
			}

			// SCHEDULED PROCEDURE STEP level
			ScheduledProcedureStepInformationEntity scheduledProcedureStepInformationEntity = null;

			// check if the scheduled procedure step IE is already in the requested procedure IE children
			foreach (ScheduledProcedureStepInformationEntity lScheduledProcedureStepInformationEntity in requestedProcedureInformationEntity.Children)
			{
				if (lScheduledProcedureStepInformationEntity.IsFoundIn(dataset))
				{
					scheduledProcedureStepInformationEntity = lScheduledProcedureStepInformationEntity;
					break;
				}
			}

			// scheduled procedure step IE is not already in the requested procedure IE children
			if (scheduledProcedureStepInformationEntity == null)
			{
				// create a new scheduled procedure step IE from the dataset and add to the requested procedure IE children
				scheduledProcedureStepInformationEntity = new ScheduledProcedureStepInformationEntity();
				scheduledProcedureStepInformationEntity.CopyFrom(dataset);
				requestedProcedureInformationEntity.AddChild(scheduledProcedureStepInformationEntity);
			}
		}

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
                        DataSet dataset = Dvtk.DvtkDataHelper.ReadDataSetFromFile(fileInfo.FullName);

                        // Add dataset to Information Model - but do not re-save in file
                        AddToInformationModel(dataset, false);
                    }
                    catch (Exception)
                    {
                        //Invalid DICOM File - will be skiped from QR information model.
                    }
                }
            }
        }

        /// <summary>
        /// Add the given Dataset to the Information Model. The data is normalised into the Information Model.
        /// </summary>
        /// <param name="dataSet">Dataset to add to the informartion model</param>
        /// <param name="transferSyntax">The transfer syntax specified in the dcm file</param>
        /// <param name="fMI">The File Meta Information of the dcm file.</param>
        /// <param name="storeFile">Boolean indicating whether the or not the data set should be stored.</param>
        //public override void AddToInformationModel(DvtkData.Media.DicomFile dicomFile, bool storeFile)
        //{
        //    // PATIENT level
        //    PatientInformationEntity patientInformationEntity = null;

        //    // check if the patient IE is already in the patientRootList
        //    foreach (PatientInformationEntity lPatientInformationEntity in Root)
        //    {
        //        if (lPatientInformationEntity.IsFoundIn(dicomFile.DataSet))
        //        {
        //            patientInformationEntity = lPatientInformationEntity;
        //            break;
        //        }
        //    }

        //    // patient IE is not already in the patientRootList
        //    if (patientInformationEntity == null)
        //    {
        //        // create a new patient IE from the dataset and add to the patientRootList
        //        patientInformationEntity = new PatientInformationEntity();
        //        patientInformationEntity.CopyFrom(dicomFile.DataSet);
        //        //Root.Add(patientInformationEntity);

        //        // Modified by RB 20090128 - when handling an order scheduled event from an actor
        //        // we want to insert the order as the first entry in the information model so that
        //        // it is returned as the first entry in the worklist query
        //        Root.Insert(0, patientInformationEntity);
        //    }

        //    // VISIT level
        //    VisitInformationEntity visitInformationEntity = null;

        //    // check if the visit IE is already in the patient IE children
        //    foreach (VisitInformationEntity lVisitInformationEntity in patientInformationEntity.Children)
        //    {
        //        if (lVisitInformationEntity.IsFoundIn(dicomFile.DataSet))
        //        {
        //            visitInformationEntity = lVisitInformationEntity;
        //            break;
        //        }
        //    }

        //    // visit IE is not already in the patient IE children
        //    if (visitInformationEntity == null)
        //    {
        //        // create a new visit IE from the dataset and add to the patient IE children
        //        visitInformationEntity = new VisitInformationEntity();
        //        visitInformationEntity.CopyFrom(dicomFile.DataSet);
        //        patientInformationEntity.AddChild(visitInformationEntity);
        //    }

        //    // IMAGING SERVICE REQUEST level
        //    ImagingServiceRequestInformationEntity imagingServiceRequestInformationEntity = null;

        //    // check if the imaging service request IE is already in the visit IE children
        //    foreach (ImagingServiceRequestInformationEntity lImagingServiceRequestInformationEntity in visitInformationEntity.Children)
        //    {
        //        if (lImagingServiceRequestInformationEntity.IsFoundIn(dicomFile.DataSet))
        //        {
        //            imagingServiceRequestInformationEntity = lImagingServiceRequestInformationEntity;
        //            break;
        //        }
        //    }

        //    // imaging service request IE is not already in the visit IE children
        //    if (imagingServiceRequestInformationEntity == null)
        //    {
        //        // create a new imaging service request IE from the dataset and add to the visit IE children
        //        imagingServiceRequestInformationEntity = new ImagingServiceRequestInformationEntity();
        //        imagingServiceRequestInformationEntity.CopyFrom(dicomFile.DataSet);
        //        visitInformationEntity.AddChild(imagingServiceRequestInformationEntity);
        //    }

        //    // REQUESTED PROCEDURE level
        //    RequestedProcedureInformationEntity requestedProcedureInformationEntity = null;

        //    // check if the requested procedure IE is already in the imaging service request IE children
        //    foreach (RequestedProcedureInformationEntity lRequestedProcedureInformationEntity in imagingServiceRequestInformationEntity.Children)
        //    {
        //        if (lRequestedProcedureInformationEntity.IsFoundIn(dicomFile.DataSet))
        //        {
        //            requestedProcedureInformationEntity = lRequestedProcedureInformationEntity;
        //            break;
        //        }
        //    }

        //    // requested procedure IE is not already in the imaging service request IE children
        //    if (requestedProcedureInformationEntity == null)
        //    {
        //        // create a new requested procedure IE from the dataset and add to the imaging service request IE children
        //        requestedProcedureInformationEntity = new RequestedProcedureInformationEntity();
        //        requestedProcedureInformationEntity.CopyFrom(dicomFile.DataSet);
        //        imagingServiceRequestInformationEntity.AddChild(requestedProcedureInformationEntity);
        //    }

        //    // SCHEDULED PROCEDURE STEP level
        //    ScheduledProcedureStepInformationEntity scheduledProcedureStepInformationEntity = null;

        //    // check if the scheduled procedure step IE is already in the requested procedure IE children
        //    foreach (ScheduledProcedureStepInformationEntity lScheduledProcedureStepInformationEntity in requestedProcedureInformationEntity.Children)
        //    {
        //        if (lScheduledProcedureStepInformationEntity.IsFoundIn(dicomFile.DataSet))
        //        {
        //            scheduledProcedureStepInformationEntity = lScheduledProcedureStepInformationEntity;
        //            break;
        //        }
        //    }

        //    // scheduled procedure step IE is not already in the requested procedure IE children
        //    if (scheduledProcedureStepInformationEntity == null)
        //    {
        //        // create a new scheduled procedure step IE from the dataset and add to the requested procedure IE children
        //        scheduledProcedureStepInformationEntity = new ScheduledProcedureStepInformationEntity();
        //        scheduledProcedureStepInformationEntity.CopyFrom(dicomFile.DataSet);
        //        requestedProcedureInformationEntity.AddChild(scheduledProcedureStepInformationEntity);
        //    }
        //}

		/// <summary>
		/// Query the Information Model using the given Query Dataset.
		/// </summary>
		/// <param name="queryDataset">Query Dataset.</param>
		/// <returns>A collection of zero or more query reponse datasets.</returns>
		public override DataSetCollection QueryInformationModel(DataSet queryDataset)
		{
			DataSetCollection queryResponses = new DataSetCollection();

            BaseInformationEntityList matchingPatients = new BaseInformationEntityList();
            BaseInformationEntityList matchingVisits = new BaseInformationEntityList();
            BaseInformationEntityList matchingImagingServiceRequests = new BaseInformationEntityList();
            BaseInformationEntityList matchingRequestedProcedures = new BaseInformationEntityList();
            BaseInformationEntityList matchingScheduledProcedureSteps = new BaseInformationEntityList();

			TagTypeList queryTagTypeList = new TagTypeList();
			TagTypeList returnTagTypeList = new TagTypeList();
            foreach (DvtkData.Dimse.Attribute attribute in queryDataset)
            {
                // special check for the Scheduled Procedure Step Sequence
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
                // Query attribute must be present with an attribute value
                // - Do not include the Specific Character Set attribute and group length as a query attribute
                else if ((attribute.Length != 0) &&
                        (attribute.Tag != Tag.SPECIFIC_CHARACTER_SET) &&
                        (attribute.Tag.ElementNumber != 0x0000) &&
                        (attribute.ValueRepresentation != VR.SQ))
                {
                    queryTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagRequired));
                }

                // Add all attributes as return attributes
                returnTagTypeList.Add(new TagType(attribute.Tag, TagTypeEnum.TagOptional));

            }
			
			// iterate over the Modality Worklist Information Model and save all the matching
			// Scheduled Procedure Steps
			// iterate of all Information Entities
			foreach (PatientInformationEntity patientInformationEntity in Root)
			{
				if (patientInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
					
				{
                    Console.WriteLine("-----PatientInformationEntity------");
                    Console.WriteLine(patientInformationEntity.DataSet.Dump("-"));
                    matchingPatients.Add(patientInformationEntity);

					foreach (VisitInformationEntity visitInformationEntity in patientInformationEntity.Children)
					{
                        Console.WriteLine("-----VisitInformationEntity------");
                        Console.WriteLine(visitInformationEntity.DataSet.Dump("--"));
						if (visitInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
						{
                            matchingVisits.Add(visitInformationEntity);

							foreach (ImagingServiceRequestInformationEntity imagingServiceRequestInformationEntity in visitInformationEntity.Children)
							{
                                Console.WriteLine("-----ImagingServiceRequestInformationEntity------");
                                Console.WriteLine(imagingServiceRequestInformationEntity.DataSet.Dump("---"));
								if (imagingServiceRequestInformationEntity.IsFoundIn(queryTagTypeList, queryDataset)) 
								{
                                    matchingImagingServiceRequests.Add(imagingServiceRequestInformationEntity);

									foreach (RequestedProcedureInformationEntity requestedProcedureInformationEntity in imagingServiceRequestInformationEntity.Children)
									{
                                        Console.WriteLine("-----RequestedProcedureInformationEntity------");
                                        Console.WriteLine(requestedProcedureInformationEntity.DataSet.Dump("----"));
										if (requestedProcedureInformationEntity.IsFoundIn(queryTagTypeList, queryDataset)) 
										{
                                            matchingRequestedProcedures.Add(requestedProcedureInformationEntity);

                                           // if (queryItem != null)
                                            {
                                                
                                                foreach (ScheduledProcedureStepInformationEntity scheduledProcedureStepInformationEntity in requestedProcedureInformationEntity.Children)
                                                {
                                                    Console.WriteLine("-----ScheduledProcedureStepInformationEntity------");
                                                    Console.WriteLine(scheduledProcedureStepInformationEntity.DataSet.Dump("------"));
                                                    if (scheduledProcedureStepInformationEntity.IsFoundIn(queryTagTypeList, queryDataset))
                                                    {
                                                        // add the scheduled procedure step to the matched list
                                                        matchingScheduledProcedureSteps.Add(scheduledProcedureStepInformationEntity);
                                                    }
                                                }
                                            }
										}
									}
								}
							}
						}
					}
				}
			}

            if (matchingScheduledProcedureSteps.Count > 0)
            {
                // we now have a list of all the matching scheduled procedure steps
                foreach (ScheduledProcedureStepInformationEntity matchingScheduledProcedureStepInformationEntity in matchingScheduledProcedureSteps)
                {
                    //SequenceItem responseItem = new SequenceItem();
                    //matchingScheduledProcedureStepInformationEntity.CopyTo(returnTagTypeList, responseItem);

                    //// remove the specific character set from the responseItem - it is only present in the scheduled procedure step as a helper...
                    //DvtkData.Dimse.Attribute specificChararcterSet = responseItem.GetAttribute(Tag.SPECIFIC_CHARACTER_SET);
                    //if (specificChararcterSet != null)
                    //{
                    //    responseItem.Remove(specificChararcterSet);
                    //}

                    //DvtkData.Dimse.Attribute attribute = new DvtkData.Dimse.Attribute(0x00400100, VR.SQ, responseItem);

                    DataSet queryResponse = new DataSet();
                   // queryResponse.Add(attribute);

                    // if the specific character set attribute has been stored in the sps IE - return it in the query response
                    DvtkData.Dimse.Attribute specificCharacterSetAttribute = matchingScheduledProcedureStepInformationEntity.GetSpecificCharacterSet();
                    if (specificCharacterSetAttribute != null)
                    {
                        queryResponse.Add(specificCharacterSetAttribute);
                    }

                    RequestedProcedureInformationEntity matchingRequestedProcedureInformationEntity
                        = (RequestedProcedureInformationEntity)matchingScheduledProcedureStepInformationEntity.Parent;
                    matchingRequestedProcedureInformationEntity.CopyTo(returnTagTypeList, queryResponse);

                    ImagingServiceRequestInformationEntity matchingImagingServiceRequestInformationEntity
                        = (ImagingServiceRequestInformationEntity)matchingRequestedProcedureInformationEntity.Parent;
                    matchingImagingServiceRequestInformationEntity.CopyTo(returnTagTypeList, queryResponse);

                    VisitInformationEntity matchingVisitInformationEntity
                        = (VisitInformationEntity)matchingImagingServiceRequestInformationEntity.Parent;
                    matchingVisitInformationEntity.CopyTo(returnTagTypeList, queryResponse);

                    PatientInformationEntity matchingPatientInformationEntity
                        = (PatientInformationEntity)matchingVisitInformationEntity.Parent;
                    matchingPatientInformationEntity.CopyTo(returnTagTypeList, queryResponse);
                    matchingScheduledProcedureStepInformationEntity.CopyTo(returnTagTypeList, queryResponse);
                    queryResponses.Add(queryResponse);
                }
            }
            //else if (matchingRequestedProcedures.Count > 0)
            //{
            //    // we now have a list of all the matching requested procedures
            //    foreach (RequestedProcedureInformationEntity matchingRequestedProcedureInformationEntity in matchingRequestedProcedures)
            //    {
            //        DataSet queryResponse = new DataSet();

            //        matchingRequestedProcedureInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        ImagingServiceRequestInformationEntity matchingImagingServiceRequestInformationEntity
            //            = (ImagingServiceRequestInformationEntity)matchingRequestedProcedureInformationEntity.Parent;
            //        matchingImagingServiceRequestInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        VisitInformationEntity matchingVisitInformationEntity
            //            = (VisitInformationEntity)matchingImagingServiceRequestInformationEntity.Parent;
            //        matchingVisitInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        PatientInformationEntity matchingPatientInformationEntity
            //            = (PatientInformationEntity)matchingVisitInformationEntity.Parent;
            //        matchingPatientInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        queryResponses.Add(queryResponse);
            //    }
            //}
            //else if (matchingImagingServiceRequests.Count > 0)
            //{
            //    // we now have a list of all the matching image service requests
            //    foreach (ImagingServiceRequestInformationEntity matchingImagingServiceRequestInformationEntity in matchingImagingServiceRequests)
            //    {
            //        DataSet queryResponse = new DataSet();

            //        matchingImagingServiceRequestInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        VisitInformationEntity matchingVisitInformationEntity
            //            = (VisitInformationEntity)matchingImagingServiceRequestInformationEntity.Parent;
            //        matchingVisitInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        PatientInformationEntity matchingPatientInformationEntity
            //            = (PatientInformationEntity)matchingVisitInformationEntity.Parent;
            //        matchingPatientInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        queryResponses.Add(queryResponse);
            //    }
            //}
            //else if (matchingVisits.Count > 0)
            //{
            //    // we now have a list of all the matching visits
            //    foreach (VisitInformationEntity matchingVisitInformationEntity in matchingVisits)
            //    {
            //        DataSet queryResponse = new DataSet();

            //        matchingVisitInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        PatientInformationEntity matchingPatientInformationEntity
            //            = (PatientInformationEntity)matchingVisitInformationEntity.Parent;
            //        matchingPatientInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        queryResponses.Add(queryResponse);
            //    }
            //}
            //else if (matchingPatients.Count > 0)
            //{
            //    // we now have a list of all the matching patients
            //    foreach (PatientInformationEntity matchingPatientInformationEntity in matchingPatients)
            //    {
            //        DataSet queryResponse = new DataSet();

            //        matchingPatientInformationEntity.CopyTo(returnTagTypeList, queryResponse);

            //        queryResponses.Add(queryResponse);
            //    }
            //}

			return queryResponses;
		}
		#endregion

		#region Transaction Handler Overrides
		/// <summary>
		/// Patient Registration request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient registration attributes.</param>
		public override void PatientRegistration(DvtkData.Dimse.DataSet dataset)
		{
			// update the first patient IE with the registration details
			// - check that there is at least one.
			if (Root.Count == 0) return;

			PatientInformationEntity patientInformationEntity = (PatientInformationEntity) Root[0];
			patientInformationEntity.CopyFrom(dataset);
		}

		/// <summary>
		/// Patient Update request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient update attributes.</param>
		public override void PatientUpdate(DvtkData.Dimse.DataSet dataset)
		{
			// check each patient IE in the patientRootList
			foreach (PatientInformationEntity lPatientInformationEntity in Root)
			{
				// if IE matches the unique patient id field
				if (lPatientInformationEntity.IsUniqueTagFoundIn(dataset))
				{
					// update the other patient demographics
					lPatientInformationEntity.CopyFrom(dataset);
					break;
				}
			}
		}

		/// <summary>
		/// Patient merge request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing patient merge attributes.</param>
		public override void PatientMerge(DvtkData.Dimse.DataSet dataset)
		{
			// Get the merge patient - need to use this to search for the corresponding patient IE
			DvtkData.Dimse.Attribute mergePatientId = dataset.GetAttribute(Tag.OTHER_PATIENT_IDS);
			if (mergePatientId.Length != 0)
			{
				LongString longString = (LongString)mergePatientId.DicomValue;
				System.String attributeValue = longString.Values[0];

				DataSet queryDataset = new DataSet("Transient");
				queryDataset.AddAttribute(Tag.PATIENT_ID.GroupNumber, Tag.PATIENT_ID.ElementNumber, VR.LO, attributeValue);

				// check each patient IE in the patientRootList
				foreach (PatientInformationEntity lPatientInformationEntity in Root)
				{
					// if IE matches the unique (merge) patient id field in the query dataset
					if (lPatientInformationEntity.IsUniqueTagFoundIn(queryDataset))
					{
						// update the patient demographics - including the patient id
						lPatientInformationEntity.CopyFrom(dataset);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Placer order management request - update modality worklist information model.
		/// </summary>
		/// <param name="dataset">Dataset containing placer order management attributes.</param>
		public override void PlacerOrderManagement(DvtkData.Dimse.DataSet dataset)
		{
			AddToInformationModel(dataset, false);

			/*
			// check each patient IE in the patientRootList
			foreach (PatientInformationEntity patientInformationEntity in Root)
			{
				// if IE matches the unique patient id field
				if (patientInformationEntity.IsUniqueTagFoundIn(dataset))
				{
					// update the first visit IE with the order details
					// - check that there is at least one.
					if (patientInformationEntity.Children.Count == 0) return;
					VisitInformationEntity visitInformationEntity = (VisitInformationEntity)patientInformationEntity.Children[0];
					visitInformationEntity.CopyFrom(dataset);

					// update the first image service request IE with the order details
					// - check that there is at least one.
					if (visitInformationEntity.Children.Count == 0) return;
					ImagingServiceRequestInformationEntity imagingServiceRequestInformationEntity = (ImagingServiceRequestInformationEntity)visitInformationEntity.Children[0];
					imagingServiceRequestInformationEntity.CopyFrom(dataset);

					// update the first requested procedure IE with the order details
					// - check that there is at least one.
					if (imagingServiceRequestInformationEntity.Children.Count == 0) return;
					RequestedProcedureInformationEntity requestedProcedureInformationEntity = (RequestedProcedureInformationEntity)imagingServiceRequestInformationEntity.Children[0];
					requestedProcedureInformationEntity.CopyFrom(dataset);

					// update the first scheduled procedure step IE with the order details
					// - check that there is at least one.
					if (requestedProcedureInformationEntity.Children.Count == 0) return;
					ScheduledProcedureStepInformationEntity scheduledProcedureStepInformationEntity = (ScheduledProcedureStepInformationEntity)requestedProcedureInformationEntity.Children[0];
					scheduledProcedureStepInformationEntity.CopyFrom(dataset);
					break;
				}
			}
			*/
		}
		#endregion Transaction Handler Overrides
	}
}
