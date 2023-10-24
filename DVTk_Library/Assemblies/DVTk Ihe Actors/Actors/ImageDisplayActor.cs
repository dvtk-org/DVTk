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
using System.Collections;
using System.IO;

using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.InformationModel;
using DvtkHighLevelInterface.Comparator;
using Dvtk.Dicom.InformationEntity.DefaultValues;
using DvtkData.Dimse;
using Dvtk.Comparator;
using Dvtk.IheActors.Bases;
using Dvtk.IheActors.Dicom;

namespace Dvtk.IheActors.Actors
{
	/// <summary>
	/// Summary description for ImageDisplayActor.
	/// </summary>
	public class ImageDisplayActor : BaseActor, IImageDisplayActor
	{
		private DicomQueryItemCollection _patientLevelQueryItems = null;
		private DicomQueryItemCollection _studyLevelQueryItems = null;
		private DicomQueryItemCollection _seriesLevelQueryItems = null;
		private DicomQueryItemCollection _instanceLevelQueryItems = null;
		private DicomQueryItemCollection _retrieveItems = null;

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public ImageDisplayActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.ImageDisplay, id), iheFramework) 
		{
		}

        /// <summary>
        /// Property - QueryItems
        /// </summary>
        /// <param name="level">Q/R Information Model that was used in the corresponding SendQueryImages() method call.</param>
        /// <returns>Collection of C-FIND-RSP messages that contain a C-FIND-RSP Dataset. The number of entries in the collection
        /// indicates the number of query matches.</returns>
        public DicomQueryItemCollection QueryItems(QueryRetrieveLevelEnum level)
		{
			DicomQueryItemCollection queryItems = null;
			switch(level)
			{
				case QueryRetrieveLevelEnum.PatientQueryRetrieveLevel:
					queryItems = _patientLevelQueryItems;
					break;
				case QueryRetrieveLevelEnum.StudyQueryRetrieveLevel:
					queryItems = _studyLevelQueryItems;
					break;
				case QueryRetrieveLevelEnum.SeriesQueryRetrieveLevel:
					queryItems = _seriesLevelQueryItems;
					break;
				case QueryRetrieveLevelEnum.InstanceQueryRetrieveLevel:
					queryItems = _instanceLevelQueryItems;
					break;
				default:
					break;
			}
			return queryItems;
		}

        /// <summary>
        /// Property - RetrieveItems.
        /// Collection of C-MOVE-RSP messages returned. There may be more than one entry in the collection if the retrieve SCP
        /// supports intermediate C-MOVE-RSP messages.
        /// Use DicomQueryItem.GetValue(Tag.NUMBER_OF_COMPLETED_SUBOPERATIONS) to get completed count.
        /// Use DicomQueryItem.GetValue(Tag.NUMBER_OF_FAILED_SUBOPERATIONS) to get failed count.
        /// Use DicomQueryItem.GetValue(Tag.NUMBER_OF_REMAINING_SUBOPERATIONS) to get remaining count.
        /// Use DicomQueryItem.GetValue(Tag.NUMBER_OF_WARNING_SUBOPERATIONS) to get warning count.
        /// </summary>
        public DicomQueryItemCollection RetrieveItems
		{
			get
			{
				return _retrieveItems;
			}
		}

		/// <summary>
		/// Apply the Actor Configuration.
		/// </summary>
		/// <param name="commonConfig">Common Configuration.</param>
		/// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
		protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
		{
			// for receiving Retrieve Images [RAD-16]
			AddDicomServer(DicomServerTypeEnum.DicomStorageServer, ActorTypeEnum.ImageArchive, commonConfig, peerToPeerConfigCollection);

			// for sending Query Images [RAD-14]
			// for sending Retrieve Images [RAD-16]
			AddDicomClient(DicomClientTypeEnum.DicomQueryRetrieveClient, ActorTypeEnum.ImageArchive, commonConfig, peerToPeerConfigCollection);
		}

		/// <summary>
		/// Handle a Dicom Transaction from the given Actor Name.
		/// </summary>
		/// <param name="actorName">Source Actor Name.</param>
		/// <param name="dicomTransaction">Dicom Transaction.</param>
        protected override void HandleTransactionFrom(ActorName actorName, DicomTransaction dicomTransaction)
		{
			switch (actorName.Type)
			{
				case ActorTypeEnum.ImageArchive:
					// received Retrieve Images [RAD-16]
					break;
				default:
					break;
			}
		}	

		#region SendQueryImages() overloads

        /// <summary>
        /// Send a C-FIND-RQ Information Model Query.
        /// Query based on the informationModel provided and the query/retrieve level. Take
        /// the query tags from the queryTags provided.
        /// 
        /// The C-FIND-RSP messages returned are stored in a DicomQueryItemCollection named QueryItems.
        /// </summary>
        /// <param name="informationModel">Q/R Information Model to be used in the query operation.</param>
        /// <param name="level">Query / retrieve level.</param>
        /// <param name="queryTags">List of Query Tags.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendQueryImages(QueryRetrieveInformationModelEnum informationModel, QueryRetrieveLevelEnum level, TagValueCollection queryTags)
		{
			System.String queryRetrieveLevel = System.String.Empty;
			switch(level)
			{
				case QueryRetrieveLevelEnum.PatientQueryRetrieveLevel:
					_patientLevelQueryItems = new DicomQueryItemCollection();
					queryRetrieveLevel = "PATIENT";
					break;
				case QueryRetrieveLevelEnum.StudyQueryRetrieveLevel:
					_studyLevelQueryItems = new DicomQueryItemCollection();
					queryRetrieveLevel = "STUDY";
					break;
				case QueryRetrieveLevelEnum.SeriesQueryRetrieveLevel:
					_seriesLevelQueryItems = new DicomQueryItemCollection();
					queryRetrieveLevel = "SERIES";
					break;
				case QueryRetrieveLevelEnum.InstanceQueryRetrieveLevel:
					_instanceLevelQueryItems = new DicomQueryItemCollection();
					queryRetrieveLevel = "IMAGE";
					break;
				default:
					return false;
			}
			if (queryTags.Find(Tag.QUERY_RETRIEVE_LEVEL) == null)
			{
				queryTags.Add(new DicomTagValue(Tag.QUERY_RETRIEVE_LEVEL, queryRetrieveLevel));
			}

			DicomQueryItemCollection queryItems = QueryItems(level);

			DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_14);
			System.String sopClassUid = System.String.Empty;
			switch(informationModel)
			{
				case QueryRetrieveInformationModelEnum.PatientRootQueryRetrieveInformationModel:
					sopClassUid = "1.2.840.10008.5.1.4.1.2.1.1";
					break;
				case QueryRetrieveInformationModelEnum.StudyRootQueryRetrieveInformationModel:
					sopClassUid = "1.2.840.10008.5.1.4.1.2.2.1";
					break;
				case QueryRetrieveInformationModelEnum.PatientStudyOnlyQueryRetrieveInformationModel:
					sopClassUid = "1.2.840.10008.5.1.4.1.2.3.1";
					break;
				default:
                    return false;
			}
			trigger.AddItem(GenerateTriggers.MakeCFindQuery(informationModel, queryTags),
							sopClassUid,
							"1.2.840.10008.1.2");

			// RAD-14 - trigger the ImageArchive
            bool triggerResult = TriggerActorInstances(ActorTypeEnum.ImageArchive, trigger, true);

			// Get the query items returned
            if (triggerResult == true)
            {
                foreach (ActorsTransaction actorsTransaction in ActorsTransactionLog)
                {
                    if (actorsTransaction.FromActorName.Type == ActorTypeEnum.ImageArchive)
                    {
                        BaseTransaction baseTransaction = actorsTransaction.Transaction;
                        if (baseTransaction is DicomTransaction)
                        {
                            DicomTransaction dicomTransaction = (DicomTransaction)baseTransaction;
                            if (dicomTransaction.Processed == false)
                            {
                                DicomMessageCollection cFindResponses = dicomTransaction.DicomMessages.CFindResponses;
                                int index = 0;
                                foreach (DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage in cFindResponses)
                                {
                                    if (dicomMessage.DataSet.Count != 0)
                                    {
                                        DicomQueryItem dicomQueryItem = new DicomQueryItem(index++, dicomMessage);
                                        queryItems.Add(dicomQueryItem);
                                    }
                                }
                                dicomTransaction.Processed = true;
                            }
                        }
                    }
                }
            }

            return triggerResult;
		}
		#endregion

		#region SendRetrieveImages() overloads
        /// <summary>
        /// Send a C-MOVE-RQ Information Model Retrieve.
        /// Retrieve based on the informationModel provided and the query/retrieve level. Take
        /// the retrieve tags from the retrieveTags provided. The retrieve is done to the move
        /// destination.
        /// 
        /// The C-MOVE-RSP messages returned are stored in a DicomQueryItemCollection named RetrieveItems.
        /// </summary>
        /// <param name="informationModel">Q/R Information Model to be used in the retrieve operation.</param>
        /// <param name="level">Query / retrieve level.</param>
        /// <param name="moveDestination">AE Title of the "move" destination.</param>
        /// <param name="retrieveTags">List of Retrieve Tags.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendRetrieveImages(QueryRetrieveInformationModelEnum informationModel, QueryRetrieveLevelEnum level, System.String moveDestination, TagValueCollection retrieveTags)
		{
			System.String queryRetrieveLevel = System.String.Empty;
			switch(level)
			{
				case QueryRetrieveLevelEnum.PatientQueryRetrieveLevel:
					queryRetrieveLevel = "PATIENT";
					break;
				case QueryRetrieveLevelEnum.StudyQueryRetrieveLevel:
					queryRetrieveLevel = "STUDY";
					break;
				case QueryRetrieveLevelEnum.SeriesQueryRetrieveLevel:
					queryRetrieveLevel = "SERIES";
					break;
				case QueryRetrieveLevelEnum.InstanceQueryRetrieveLevel:
					queryRetrieveLevel = "IMAGE";
					break;
				default:
                    return false;
			}
			if (retrieveTags.Find(Tag.QUERY_RETRIEVE_LEVEL) == null)
			{
				retrieveTags.Add(new DicomTagValue(Tag.QUERY_RETRIEVE_LEVEL, queryRetrieveLevel));
			}
			_retrieveItems = new DicomQueryItemCollection();

			DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_16);
			System.String sopClassUid = System.String.Empty;
			switch(informationModel)
			{
				case QueryRetrieveInformationModelEnum.PatientRootQueryRetrieveInformationModel:
					sopClassUid = "1.2.840.10008.5.1.4.1.2.1.2";
					break;
				case QueryRetrieveInformationModelEnum.StudyRootQueryRetrieveInformationModel:
					sopClassUid = "1.2.840.10008.5.1.4.1.2.2.2";
					break;
				case QueryRetrieveInformationModelEnum.PatientStudyOnlyQueryRetrieveInformationModel:
					sopClassUid = "1.2.840.10008.5.1.4.1.2.3.2";
					break;
				default:
                    return false;
			}
			trigger.AddItem(GenerateTriggers.MakeCMoveRetrieve(informationModel, moveDestination, retrieveTags),
							sopClassUid,
							"1.2.840.10008.1.2");

			// RAD-16 - trigger the ImageArchive
            bool triggerResult = TriggerActorInstances(ActorTypeEnum.ImageArchive, trigger, true);

			// Get the retrieve items returned
            if (triggerResult == true)
            {
                foreach (ActorsTransaction actorsTransaction in ActorsTransactionLog)
                {
                    if (actorsTransaction.FromActorName.Type == ActorTypeEnum.ImageArchive)
                    {
                        BaseTransaction baseTransaction = actorsTransaction.Transaction;
                        if (baseTransaction is DicomTransaction)
                        {
                            DicomTransaction dicomTransaction = (DicomTransaction)baseTransaction;
                            if (dicomTransaction.Processed == false)
                            {
                                DicomMessageCollection cMoveResponses = dicomTransaction.DicomMessages.CMoveResponses;
                                int index = 0;
                                foreach (DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage in cMoveResponses)
                                {
                                    // store all C-MOVE-RSP messages
                                    // - use DicomQueryItem.GetValue(Tag.NUMBER_OF_COMPLETED_SUBOPERATIONS) to get completed count
                                    // - use DicomQueryItem.GetValue(Tag.NUMBER_OF_FAILED_SUBOPERATIONS) to get failed count
                                    // - use DicomQueryItem.GetValue(Tag.NUMBER_OF_REMAINING_SUBOPERATIONS) to get remaining count
                                    // - use DicomQueryItem.GetValue(Tag.NUMBER_OF_WARNING_SUBOPERATIONS) to get warning count
                                    DicomQueryItem dicomRetriveItem = new DicomQueryItem(index++, dicomMessage);
                                    _retrieveItems.Add(dicomRetriveItem);
                                }
                                dicomTransaction.Processed = true;
                            }
                        }
                    }
                }
            }

            return triggerResult;
		}
		#endregion

        #region Store Data Directory methods
        /// <summary>
        /// Delete all the files in the DICOM Store Data Directory used by
        /// the Dicom Server for objects received fromActorName.
        /// </summary>
        /// <param name="fromActorName">From actor name.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool ClearDicomStoreDataDirectory(ActorName fromActorName)
        {
            bool cleared = false;

            try
            {
                String storeDataDirectory = GetDicomStoreDataDirectory(fromActorName);
                DirectoryInfo directoryInfo = new DirectoryInfo(storeDataDirectory);
                if (directoryInfo != null)
                {
                    // delete the directory and all its contents
                    directoryInfo.Delete(true);

                    // re-create the directory
                    directoryInfo.Create();
                    cleared = true;
                }
            }
            catch (System.Exception)
            {
            }

            return cleared;
        }

        /// <summary>
        /// Get the number of files currently stored in the DICOM
        /// Storage Directory fromActorName.
        /// </summary>
        /// <param name="fromActorName">From actor name.</param>
        /// <returns>Number of files in directory.</returns>
        public int GetNoDicomStoreDataFiles(ActorName fromActorName)
        {
            int noDicomStoreDataFiles = 0;

            try
            {
                String storeDataDirectory = GetDicomStoreDataDirectory(fromActorName);
                DirectoryInfo directoryInfo = new DirectoryInfo(storeDataDirectory);
                if (directoryInfo != null)
                {
                    // get the number of files stored in the directory
                    FileInfo [] fileInfo = directoryInfo.GetFiles();
                    noDicomStoreDataFiles = fileInfo.Length;
                }
            }
            catch (System.Exception)
            {
            }

            return noDicomStoreDataFiles;
        }

        /// <summary>
        /// Get the name of the indexed file in the DICOM Storage Directory
        /// fromActorName. The filename can then be used to further access
        /// the file.
        /// </summary>
        /// <param name="fromActorName">From actor name.</param>
        /// <param name="index">Zero based index from directory System.IO.FileInfo[].</param>
        /// <returns>Full filename for indexed DICOM file.</returns>
        public String GetDicomStoreDataFilename(ActorName fromActorName, int index)
        {
            String dicomStoreDataFilename = String.Empty;

            try
            {
                String storeDataDirectory = GetDicomStoreDataDirectory(fromActorName);
                DirectoryInfo directoryInfo = new DirectoryInfo(storeDataDirectory);
                if (directoryInfo != null)
                {
                    // get the indexed filename
                    FileInfo[] fileInfo = directoryInfo.GetFiles();
                    dicomStoreDataFilename = fileInfo[index].FullName;
                }
            }
            catch (System.Exception)
            {
            }

            return dicomStoreDataFilename;
        }

        /// <summary>
        /// Get the DICOM Store Data Directory for the given fromActorName.
        /// </summary>
        /// <param name="fromActorName">From Actor Name.</param>
        /// <returns>Full directory name for DICOM Store Data.</returns>
        public String GetDicomStoreDataDirectory(ActorName fromActorName)
        {
            String dicomStoreDataDirectory = String.Empty;

            // get the DICOM storage data directory for the given from actor name.
            DicomServer dicomServer = GetDicomServer(fromActorName);
            if (dicomServer != null)
            {
                dicomStoreDataDirectory = dicomServer.StoreDataDirectory;
            }

            return dicomStoreDataDirectory;
        }
        #endregion
    }
}
