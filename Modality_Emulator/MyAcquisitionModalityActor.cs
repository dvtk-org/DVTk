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
using Dvtk.IheActors.Actors;

namespace ModalityEmulator
{
    /// <summary>
    /// Summary description for MyAcquisitionModalityActor.
    /// </summary>
    public class MyAcquisitionModalityActor : BaseActor, IAcquisitionModalityActor
    {
        private DicomQueryItemCollection _modalityWorklistItems = null;
        private ReferencedSopItemCollection _storageCommitItems = null;
        private System.String _mppsInstanceUid = System.String.Empty;
        private System.String _storageCommitTransactionUid = System.String.Empty;
        private DvtkHighLevelInterface.Dicom.Messages.DicomMessage _nCreateSetMppsInProgress = null;
        private MapWorklistItemToStorageDirectory _mapWorklistItemToStorageDirectory = new MapWorklistItemToStorageDirectory();
        private Semaphore _storageCommitmentNEventReportResponseSemaphore = null;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="id">Actor Id.</param>
        /// <param name="iheFramework">Ihe Framework container.</param>
        public MyAcquisitionModalityActor(System.String id, Dvtk.IheActors.IheFramework.IheFramework iheFramework)
            : base(new ActorName(ActorTypeEnum.AcquisitionModality, id), iheFramework)
        {
            _modalityWorklistItems = new DicomQueryItemCollection();
            _storageCommitItems = new ReferencedSopItemCollection();
        }

        /// <summary>
        /// Property - ModalityWorklistItems
        /// </summary>
        public DicomQueryItemCollection ModalityWorklistItems
        {
            get
            {
                return _modalityWorklistItems;
            }
        }

        /// <summary>
        /// Property - MapWorklistItemToStorageDirectory
        /// </summary>
        public MapWorklistItemToStorageDirectory MapWorklistItemToStorageDirectory
        {
            get
            {
                return _mapWorklistItemToStorageDirectory;
            }
        }

        /// <summary>
        /// Apply the Actor Configuration.
        /// </summary>
        /// <param name="commonConfig">Common Configuration.</param>
        /// <param name="peerToPeerConfigCollection">Peer to Peer Configuration collection.</param>
        protected override void ApplyConfig(CommonConfig commonConfig, BasePeerToPeerConfigCollection peerToPeerConfigCollection)
        {
            // for receiving Storage Commitment [RAD-10]
            AddDicomServer(DicomServerTypeEnum.DicomStorageCommitServer, ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);

            // for sending Query Modality Worklist [RAD-5]
            AddDicomClient(DicomClientTypeEnum.DicomWorklistClient, ActorTypeEnum.DssOrderFiller, commonConfig, peerToPeerConfigCollection);

            // for sending Modality Procedure Step In Progress [RAD-6]
            // for sending Modality Procedure Step Completed [RAD-7]
            AddDicomClient(DicomClientTypeEnum.DicomMppsClient, ActorTypeEnum.PerformedProcedureStepManager, commonConfig, peerToPeerConfigCollection);

            // for sending Modality Images Stored [RAD-8]
            AddDicomClient(DicomClientTypeEnum.DicomStorageClient, ActorTypeEnum.ImageArchive, commonConfig, peerToPeerConfigCollection);

            // for sending Storage Commitment [RAD-10]
            AddDicomClient(DicomClientTypeEnum.DicomStorageCommitClient, ActorTypeEnum.ImageManager, commonConfig, peerToPeerConfigCollection);
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
                case ActorTypeEnum.ImageManager:
                    // received Storage Commitment [RAD-10]
                    DicomMessageCollection nEventReportRequests = dicomTransaction.DicomMessages.NEventReportRequests;

                    foreach (DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage in nEventReportRequests)
                    {
                        // Update the storage commit items with the appropriate status - as received in the
                        // event report request
                        GenerateTriggers.HandleNEventReportStorageCommitment(_storageCommitItems, dicomMessage);
                    }
                    break;
                default:
                    break;
            }
        }

        #region Events - Message Handling
        /// <summary>
        /// Subscribe to the StorageCommitmentNEventReportResponse Event.
        /// </summary>
        private void SubscribeStorageCommitmentNEventReportResponseEvent()
        {
            _storageCommitmentNEventReportResponseSemaphore = new Semaphore(100);

            this.OnMessageAvailable += new MyAcquisitionModalityActor.MessageAvailableHandler(HandleStorageCommitmentNEventReportResponse);
        }

        /// <summary>
        /// Unsubscribe to the StorageCommitmentNEventReportResponse Event.
        /// </summary>
        private void UnsubscribeStorageCommitmentNEventReportResponseEvent()
        {
            this.OnMessageAvailable -= new MyAcquisitionModalityActor.MessageAvailableHandler(HandleStorageCommitmentNEventReportResponse);

            _storageCommitmentNEventReportResponseSemaphore = null;
        }

        /// <summary>
        /// Only intereseted in Storage Commitment N-Event-Report Response messages.
        /// Signal the local N-Event-Report Response semaphore on receipt of this message type.
        /// </summary>
        /// <param name="server">Event source.</param>
        /// <param name="messageAvailableEvent">Message Available Event Details.</param>
        private void HandleStorageCommitmentNEventReportResponse(object server, MessageAvailableEventArgs messageAvailableEvent)
        {
            if (messageAvailableEvent.Message.Message is DicomProtocolMessage)
            {
                DicomProtocolMessage dicomProtocolMessage = (DicomProtocolMessage)messageAvailableEvent.Message.Message;

                if (dicomProtocolMessage is DvtkHighLevelInterface.Dicom.Messages.DicomMessage)
                {
                    DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage = (DvtkHighLevelInterface.Dicom.Messages.DicomMessage)dicomProtocolMessage;
                    if (dicomMessage.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.NEVENTREPORTRSP)
                    {
                        if (_storageCommitmentNEventReportResponseSemaphore != null)
                        {
                            _storageCommitmentNEventReportResponseSemaphore.Signal();
                        }
                    }
                }
            }
        }

        private bool WaitStorageCommitmentNEventReportResponse(int timeout)
        {
            bool receivedEvent = false;
            if (_storageCommitmentNEventReportResponseSemaphore != null)
            {
                receivedEvent = _storageCommitmentNEventReportResponseSemaphore.Wait((uint)timeout);
            }

            return receivedEvent;
        }
        #endregion Events - Message Handling

        #region SendQueryModalityWorklist() overloads

        /// <summary>
        /// Create an unscheduled worklist item by adding the given user defined tags to those
        /// of the Default Value Manager. This worklist item can then be used in the
        /// SendNCreateModalityProcedureStepInProgress() and SendModalityImagesStored() method
        /// calls.
        /// This simulates the IHE Unscheduled Use Case.
        /// </summary>
        /// <param name="userDefinedTags">User defined Tags.</param>
        /// <returns>Modality worklist item populated from the user defined tags and default values.</returns>
        public DicomQueryItem CreateUnscheduledWorklistItem(TagValueCollection userDefinedTags)
        {
            return new DicomQueryItem(1, GenerateTriggers.MakeModalityWorklistItem(DefaultValueManager, userDefinedTags));
        }

        /// <summary>
        /// Send a CFind Modality Worklist Query trigger from the given queryTags.
        /// All other return keys are taken from the supported return key attributes
        /// in the Worklist Information Model.
        /// </summary>
        /// <param name="queryTags">List of Query Tags.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendQueryModalityWorklist(TagValueCollection queryTags)
        {
            // Generate trigger using the query tags
            DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_5);
            trigger.AddItem(GenerateTriggers.MakeCFindModalityWorklist(queryTags),
                            "1.2.840.10008.5.1.4.31",
                            "1.2.840.10008.1.2");
            return TriggerModalityWorklistQuery(trigger);
        }

        /// <summary>
        /// Send a CFind Modality Worklist Query trigger by reading the query
        /// dataset from the given mwlQueryDcmFilename. If the scheduledProcedureStepStartDate
        /// is defined (not string empty) then if a value for this attribute is present in the
        /// read datset it will be overwritten by the scheduledProcedureStepStartDate value.
        /// </summary>
        /// <param name="mwlQueryDcmFilename">DCM file containing the MWL Query Dataset.</param>
        /// <param name="scheduledProcedureStepStartDate">Optional (not sting empty) start date to overwrite dataset value.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendQueryModalityWorklist(System.String mwlQueryDcmFilename, System.String scheduledProcedureStepStartDate)
        {
            // Generate trigger using the mwl dcm file contents and the given scheduledProcedureStepStartDate
            DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_5);
            trigger.AddItem(GenerateTriggers.MakeCFindModalityWorklist(mwlQueryDcmFilename, scheduledProcedureStepStartDate),
                "1.2.840.10008.5.1.4.31",
                "1.2.840.10008.1.2");
            return TriggerModalityWorklistQuery(trigger);
        }

        private bool TriggerModalityWorklistQuery(DicomTrigger trigger)
        {
            // Start with empty worklist and storage commit items
            _modalityWorklistItems = new DicomQueryItemCollection();
            _storageCommitItems = new ReferencedSopItemCollection();
            _nCreateSetMppsInProgress = null;

            // RAD-5 - trigger the DssOrderFiller
            bool triggerResult = TriggerActorInstances(ActorTypeEnum.DssOrderFiller, trigger, true);

            // Get the worklist items returned
            if (triggerResult == true)
            {
                foreach (ActorsTransaction actorsTransaction in ActorsTransactionLog)
                {
                    if (actorsTransaction.FromActorName.Type == ActorTypeEnum.DssOrderFiller)
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
                                        DicomQueryItem dicomWorklistItem = new DicomQueryItem(index++, dicomMessage);
                                        _modalityWorklistItems.Add(dicomWorklistItem);
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

        #endregion SendQueryModalityWorklist

        #region SendModalityProcedureStepInProgress() overloads

        /// <summary>
        /// Send an N-CREATE MPPS IN-PROGRESS message by using the Default Value Manager.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// </summary>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNCreateModalityProcedureStepInProgress(DicomQueryItem modalityWorklistItem)
        {
            // Initialize the N-CREATE MPPS IN-PROGRESS
            InitializeModalityProcedureStepInProgress(DvtkData.Dimse.DimseCommand.NCREATERQ);

            // Generate the N-CREATE MPPS IN-PROGRESS from the Default Value Manager
            GenerateTriggers.MakeMppsInProgress(DefaultValueManager, _nCreateSetMppsInProgress, _mppsInstanceUid);

            return TriggerModalityProcedureStepInProgress(modalityWorklistItem);
        }

        /// <summary>
        /// Send an N-CREATE MPPS IN-PROGRESS message from the given mppsInProgressDcmFilename.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// </summary>
        /// <param name="mppsInProgressDcmFilename">DCM Filename - contains MPPS IN-PROGRESS dataset.</param>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNCreateModalityProcedureStepInProgress(System.String mppsInProgressDcmFilename, DicomQueryItem modalityWorklistItem)
        {
            // Initialize the N-CREATE MPPS IN-PROGRESS
            InitializeModalityProcedureStepInProgress(DvtkData.Dimse.DimseCommand.NCREATERQ);

            // Generate the N-CREATE MPPS IN-PROGRESS from the DCM file contents (default values)
            GenerateTriggers.MakeMppsInProgress(mppsInProgressDcmFilename, DefaultValueManager, _nCreateSetMppsInProgress, _mppsInstanceUid);

            return TriggerModalityProcedureStepInProgress(modalityWorklistItem);
        }

        /// <summary>
        /// Send an N-SET MPPS IN-PROGRESS message by using the Default Value Manager.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// NOTE: This call should only be made after the SendModalityProcedureStepInProgress() (using default N-CREATE) 
        /// has been made.
        /// </summary>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNSetModalityProcedureStepInProgress(DicomQueryItem modalityWorklistItem)
        {
            // Initialize the N-SET MPPS IN-PROGRESS
            InitializeModalityProcedureStepInProgress(DvtkData.Dimse.DimseCommand.NSETRQ);

            // Generate the N-SET MPPS IN-PROGRESS from the Default Value Manager
            GenerateTriggers.MakeMppsInProgress(DefaultValueManager, _nCreateSetMppsInProgress, _mppsInstanceUid);

            return TriggerModalityProcedureStepInProgress(modalityWorklistItem);
        }

        /// <summary>
        /// Send an N-SET MPPS IN-PROGRESS message from the given mppsInProgressDcmFilename.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// NOTE: This call should only be made after the SendModalityProcedureStepInProgress() (using default N-CREATE) 
        /// has been made.
        /// </summary>
        /// <param name="mppsInProgressDcmFilename">DCM Filename - contains MPPS IN-PROGRESS dataset.</param>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNSetModalityProcedureStepInProgress(System.String mppsInProgressDcmFilename, DicomQueryItem modalityWorklistItem)
        {
            // Initialize the N-SET MPPS IN-PROGRESS
            InitializeModalityProcedureStepInProgress(DvtkData.Dimse.DimseCommand.NSETRQ);

            // Generate the N-SET MPPS IN-PROGRESS from the DCM file contents (default values)
            GenerateTriggers.MakeMppsInProgress(mppsInProgressDcmFilename, DefaultValueManager, _nCreateSetMppsInProgress, _mppsInstanceUid);

            return TriggerModalityProcedureStepInProgress(modalityWorklistItem);
        }

        private void InitializeModalityProcedureStepInProgress(DvtkData.Dimse.DimseCommand command)
        {
            // Generate a new MPPS Instance UID
            InBuiltDefaultTagValues inbuiltDefaultTagValues = new InBuiltDefaultTagValues();
            System.String uidRoot = inbuiltDefaultTagValues.UidRoot;
            DicomTagValueAutoSetUid tagValueUid = new DicomTagValueAutoSetUid(AffectedEntityEnum.PerformedProcedureStepEntity,
                Tag.AFFECTED_SOP_INSTANCE_UID, uidRoot, 1);
            _mppsInstanceUid = tagValueUid.Value;

            // Create the MPPS In-Progress message
            _nCreateSetMppsInProgress = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(command);
        }

        private bool TriggerModalityProcedureStepInProgress(DicomQueryItem modalityWorklistItem)
        {
            // Use modality Worklist Item to help construct the MPPS InProgress Message
            // Get the attribute values to copy
            DvtkHighLevelInterface.Comparator.Comparator worklistItemComparator = new DvtkHighLevelInterface.Comparator.Comparator("WorklistItemComparator");
            DicomComparator dicomWorklistItemComparator = worklistItemComparator.InitializeDicomComparator(modalityWorklistItem.DicomMessage);

            DvtkHighLevelInterface.Comparator.Comparator mppsInProgressComparator = new DvtkHighLevelInterface.Comparator.Comparator("MppsInProgressComparator");
            mppsInProgressComparator.PopulateDicomMessage(_nCreateSetMppsInProgress, dicomWorklistItemComparator);

            // Trigger the N-CREATE-RQ MPPS InProgress
            DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_6);
            trigger.AddItem(_nCreateSetMppsInProgress,
                "1.2.840.10008.3.1.2.3.3",
                "1.2.840.10008.1.2");

            // Update the default PerformedProcedureStepEntity values for the next MPPS In-Progress
            DefaultValueManager.UpdateInstantiatedDefaultTagValues(AffectedEntityEnum.PerformedProcedureStepEntity);

            // RAD-6 - trigger the PerformedProcedureStepManager
            return TriggerActorInstances(ActorTypeEnum.PerformedProcedureStepManager, trigger, true);
        }

        #endregion SendModalityProcedureStepInProgress

        #region SendModalityProcedureStepCompletedDiscontinued() overloads

        /// <summary>
        /// Send an N-SET MPPS COMPLETED message. Take the default values from the Default Value Manager.
        /// </summary>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNSetModalityProcedureStepCompleted()
        {
            // Initialize the mpps completed
            InitializeModalityProcedureStepCompletedDiscontinued();

            // Generate the mpps completed from the Default Value Manager
            DvtkHighLevelInterface.Dicom.Messages.DicomMessage nSetMppsCompleted = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.NSETRQ);
            GenerateTriggers.MakeNSetMppsCompletedDiscontinued(DefaultValueManager, _storageCommitItems, nSetMppsCompleted, _mppsInstanceUid, "COMPLETED");

            return TriggerModalityProcedureStepCompletedDiscontinued(nSetMppsCompleted);
        }

        /// <summary>
        /// Send an N-SET MPPS COMPLETED message. Take the default values from the given DCM file contents.
        /// </summary>
        /// <param name="mppsCompletedDcmFilename">DCM file for default MPPS Completed atribute values.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNSetModalityProcedureStepCompleted(System.String mppsCompletedDcmFilename)
        {
            // Initialize the mpps completed
            InitializeModalityProcedureStepCompletedDiscontinued();

            // Generate the mpps completed from the DCM file content
            DvtkHighLevelInterface.Dicom.Messages.DicomMessage nSetMppsCompleted = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.NSETRQ);
            GenerateTriggers.MakeNSetMppsCompletedDiscontinued(mppsCompletedDcmFilename, DefaultValueManager, _storageCommitItems, nSetMppsCompleted, _mppsInstanceUid);

            return TriggerModalityProcedureStepCompletedDiscontinued(nSetMppsCompleted);
        }

        /// <summary>
        /// Send an N-SET MPPS DISCONTINUED message. Take the default values from the Default Value Manager.
        /// </summary>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNSetModalityProcedureStepDiscontinued()
        {
            // Initialize the mpps discontinued
            InitializeModalityProcedureStepCompletedDiscontinued();

            // Generate the mpps discontinued from the Default Value Manager
            DvtkHighLevelInterface.Dicom.Messages.DicomMessage nSetMppsDiscontinued = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.NSETRQ);
            GenerateTriggers.MakeNSetMppsCompletedDiscontinued(DefaultValueManager, _storageCommitItems, nSetMppsDiscontinued, _mppsInstanceUid, "DISCONTINUED");

            return TriggerModalityProcedureStepCompletedDiscontinued(nSetMppsDiscontinued);
        }

        /// <summary>
        /// Send an N-SET MPPS DISCONTINUED message. Take the default values from the given DCM file contents.
        /// </summary>
        /// <param name="mppsDiscontinuedDcmFilename">DCM file for default MPPS Discontinued atribute values.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendNSetModalityProcedureStepDiscontinued(System.String mppsDiscontinuedDcmFilename)
        {
            // Initialize the mpps discontinued
            InitializeModalityProcedureStepCompletedDiscontinued();

            // Generate the mpps discontinued from the DCM file content
            DvtkHighLevelInterface.Dicom.Messages.DicomMessage nSetMppsDiscontinued = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.NSETRQ);
            GenerateTriggers.MakeNSetMppsCompletedDiscontinued(mppsDiscontinuedDcmFilename, DefaultValueManager, _storageCommitItems, nSetMppsDiscontinued, _mppsInstanceUid);

            return TriggerModalityProcedureStepCompletedDiscontinued(nSetMppsDiscontinued);
        }

        private void InitializeModalityProcedureStepCompletedDiscontinued()
        {
            // Get the C-STORE-RSPs returned
            foreach (ActorsTransaction actorsTransaction in ActorsTransactionLog)
            {
                if (actorsTransaction.FromActorName.Type == ActorTypeEnum.ImageArchive)
                {
                    BaseTransaction baseTransaction = actorsTransaction.Transaction;
                    if (baseTransaction is DicomTransaction)
                    {
                        DicomTransaction dicomTransaction = (DicomTransaction)baseTransaction;

                        DicomMessageCollection cStoreResponses = dicomTransaction.DicomMessages.CStoreResponses;
                        GenerateTriggers.HandleCStoreResponses(_storageCommitItems, cStoreResponses);
                    }
                }
            }
        }

        private bool TriggerModalityProcedureStepCompletedDiscontinued(DvtkHighLevelInterface.Dicom.Messages.DicomMessage nSetMppsCompletedDiscontinued)
        {
            // Trigger the N-SET-RQ MPPS Completed/Discontinued
            DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_7);
            trigger.AddItem(nSetMppsCompletedDiscontinued,
                            "1.2.840.10008.3.1.2.3.3",
                            "1.2.840.10008.1.2");

            // RAD-7 - trigger the PerformedProcedureStepManager
            return TriggerActorInstances(ActorTypeEnum.PerformedProcedureStepManager, trigger, true);
        }

        #endregion SendModalityProcedureStepCompletedDiscontinued() overloads

        #region SendModalityImagesStored() overloads

        /// <summary>
        /// Send a single image generated from the Default Value Manager and
        /// the given Modality Worklist Item.
        /// </summary>
        /// <param name="startNewSeries">Boolean indicating if this image is part of a new
        /// Series or not.</param>
        /// <param name="modalityWorklistItem">Worklist Item used to provide overruling values for
        /// the Image header.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendModalityImagesStored(bool startNewSeries, DicomQueryItem modalityWorklistItem)
        {
            if (modalityWorklistItem == null) return false;

            // Update the default SeriesEntity values for the next Storage Message
            if (startNewSeries == true)
            {
                DefaultValueManager.UpdateInstantiatedDefaultTagValues(AffectedEntityEnum.SeriesEntity);
            }

            // Use modality Worklist Item to help construct the Storage Message
            // Get the attribute values to copy
            DvtkHighLevelInterface.Comparator.Comparator worklistItemComparator = new DvtkHighLevelInterface.Comparator.Comparator("WorklistItemComparator");
            DicomComparator dicomWorklistItemComparator = worklistItemComparator.InitializeDicomComparator(modalityWorklistItem.DicomMessage);

            // Also try to use the MPPS InProgress Message to help construct the Storage Message
            DvtkHighLevelInterface.Comparator.Comparator mppsInProgressComparator = new DvtkHighLevelInterface.Comparator.Comparator("MppsInProgressComparator");
            DicomComparator dicomMppsInProgressComparator = null;
            if (_nCreateSetMppsInProgress != null)
            {
                dicomMppsInProgressComparator = mppsInProgressComparator.InitializeDicomComparator(_nCreateSetMppsInProgress);
            }

            // Create the Storage message
            DvtkHighLevelInterface.Comparator.Comparator storageComparator = new DvtkHighLevelInterface.Comparator.Comparator("StorageComparator");
            DvtkHighLevelInterface.Dicom.Messages.DicomMessage cStoreInstance = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ);
            GenerateTriggers.MakeCStoreInstance(DefaultValueManager, cStoreInstance);
            storageComparator.PopulateDicomMessage(cStoreInstance, dicomWorklistItemComparator);
            if (dicomMppsInProgressComparator != null)
            {
                storageComparator.PopulateDicomMessage(cStoreInstance, dicomMppsInProgressComparator);
            }

            // Trigger the C-STORE-RQ Instance
            DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_8);
            trigger.HandleInSingleAssociation = true;
            trigger.AddItem(cStoreInstance,
                            "1.2.840.10008.5.1.4.1.1.7",
                            "1.2.840.10008.1.2");

            // Update the default InstanceEntity values for the next Storage Message
            DefaultValueManager.UpdateInstantiatedDefaultTagValues(AffectedEntityEnum.InstanceEntity);

            // RAD-8 - trigger the ImageArchive
            return TriggerActorInstances(ActorTypeEnum.ImageArchive, trigger, true);
        }

        /// <summary>
        /// Send all the images found in the data directory mapped from the given
        /// Modality Worklist item - the MapWorklistItemToStorageDirectory defines
        /// the data directory / worklist item attribute value mapping.
        /// </summary>
        /// <param name="modalityWorklistItem">Worklist Item used to provide overruling values for
        /// the Image headers.</param>
        /// <param name="withSingleAssociation">Boolean indicating whether the images should be sent in a single
        /// association or not.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendModalityImagesStored(DicomQueryItem modalityWorklistItem, bool withSingleAssociation)
        {
            if (modalityWorklistItem == null) return false;

            // use the MapWorklistItemToStorageDirectory to get the MapTag
            DvtkData.Dimse.Tag mapTag = _mapWorklistItemToStorageDirectory.MapTag;
            if (mapTag == null)
            {
                throw new System.Exception("No MapTag defined for MapWorklistItemToStorageDirectory.");
            }

            // The MapTag is then used to search the worklist item for a matching description
            // - first try in the Scheduled Procedure Step Sequence
            System.String description = modalityWorklistItem.GetValue(Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE, mapTag);
            if (description == "Default")
            {
                // - now try in the remaining dataset
                description = modalityWorklistItem.GetValue(mapTag);
            }

            // Use the description to get the storage directory containing the DCM files that should be sent
            System.String storageDirectory = _mapWorklistItemToStorageDirectory.GetStorageDirectory(description);
            if (storageDirectory == System.String.Empty)
            {
                // Try the default mapping
                storageDirectory = _mapWorklistItemToStorageDirectory.GetStorageDirectory("Default");
            }
            if (storageDirectory == System.String.Empty)
            {
                System.String message = System.String.Format("No storageDirectory mapping found for description \"{0}\" or \"Default\".", description);
                throw new System.Exception(message);
            }

            // Send the contents of the storage directory - updated by the modality worklist item contents
            return SendModalityImagesStored(storageDirectory, modalityWorklistItem, withSingleAssociation);
        }

        /// <summary>
        /// Send all the images found in the given storage directory.
        /// </summary>
        /// <param name="storageDirectory">Given storage directory - containing storage DCM files.</param>
        /// <param name="modalityWorklistItem">Worklist Item used to provide overruling values for
        /// the Image headers.</param>
        /// <param name="withSingleAssociation">Boolean indicating whether the images should be sent in a single
        /// association or not.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendModalityImagesStored(System.String storageDirectory, DicomQueryItem modalityWorklistItem, bool withSingleAssociation)
        {
            if ((storageDirectory.Length == 0) ||
                (modalityWorklistItem == null)) return false;

            // Use a hash table to store the instance uid mappings
            // - the instance uids for the study, series and sop are going to be updated for all the datasets
            // read from the storageDirectory
            Hashtable instanceMapper = new Hashtable();

            // Get the directory info for the storage directory - and make sure that it exists
            DirectoryInfo directoryInfo = new DirectoryInfo(storageDirectory);
            if (directoryInfo.Exists == false)
            {
                System.String message = System.String.Format("storageDirectory:\"{0}\" - does not exist.", storageDirectory);
                throw new System.Exception(message);
            }

            // Get a trigger
            DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_8);
            trigger.HandleInSingleAssociation = withSingleAssociation;

            // Interate over all the DCM files found in the storage directory
            // - update the instances found with the worklist item contents and
            // set up the triggers for the Image Archive
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if ((fileInfo.Extension.ToLower().Equals(".dcm")) ||
                    (fileInfo.Extension == System.String.Empty))
                {
                    // Read the file meta information - it must be present for this actor
                    DvtkData.Media.FileMetaInformation fileMetaInformation = Dvtk.DvtkDataHelper.ReadFMIFromFile(fileInfo.FullName);
                    if (fileMetaInformation == null) continue;

                    // Try to get the transfer syntax uid
                    // - start with Implicit VR Little Endian
                    System.String transferSyntaxUid = "1.2.840.10008.1.2";
                    DvtkData.Dimse.Attribute attribute = fileMetaInformation.GetAttribute(DvtkData.Dimse.Tag.TRANSFER_SYNTAX_UID);
                    if (attribute != null)
                    {
                        UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
                        if (uniqueIdentifier.Values.Count > 0)
                        {
                            transferSyntaxUid = uniqueIdentifier.Values[0];
                        }
                    }

                    // Read the dataset from the DCM file
                    DvtkData.Dimse.DataSet dataset = Dvtk.DvtkDataHelper.ReadDataSetFromFile(fileInfo.FullName);
                    if (dataset == null) continue;

                    // Remove any Group Lengths from the dataset
                    dataset.RemoveGroupLengthAttributes();

                    // Try to get the series instance uid
                    System.String oldSeriesInstanceUid = System.String.Empty;
                    DvtkData.Dimse.Attribute seriesInstanceUidAttribute = dataset.GetAttribute(Tag.SERIES_INSTANCE_UID);
                    if (seriesInstanceUidAttribute != null)
                    {
                        UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)seriesInstanceUidAttribute.DicomValue;
                        if (uniqueIdentifier.Values.Count > 0)
                        {
                            oldSeriesInstanceUid = uniqueIdentifier.Values[0];
                        }

                        // Remove the old series instance from the dataset
                        dataset.Remove(seriesInstanceUidAttribute);
                    }

                    // See if a mapping exists
                    System.String newSeriesInstanceUid = (System.String)instanceMapper[oldSeriesInstanceUid];
                    if (newSeriesInstanceUid == null)
                    {
                        // Add a mapping
                        DefaultValueManager.UpdateInstantiatedDefaultTagValues(AffectedEntityEnum.SeriesEntity);
                        newSeriesInstanceUid = DefaultValueManager.GetInstantiatedValue(Tag.SERIES_INSTANCE_UID);
                        instanceMapper.Add(oldSeriesInstanceUid, newSeriesInstanceUid);
                    }

                    // Add the new series instance uid to the dataset
                    dataset.AddAttribute(Tag.SERIES_INSTANCE_UID.GroupNumber, Tag.SERIES_INSTANCE_UID.ElementNumber, VR.UI, newSeriesInstanceUid);

                    // Try to get the sop instance uid
                    System.String oldSopInstanceUid = System.String.Empty;
                    DvtkData.Dimse.Attribute sopInstanceUidAttribute = dataset.GetAttribute(Tag.SOP_INSTANCE_UID);
                    if (sopInstanceUidAttribute != null)
                    {
                        UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)sopInstanceUidAttribute.DicomValue;
                        if (uniqueIdentifier.Values.Count > 0)
                        {
                            oldSopInstanceUid = uniqueIdentifier.Values[0];
                        }

                        // Remove the old sop instance from the dataset
                        dataset.Remove(sopInstanceUidAttribute);
                    }

                    // See if a mapping exists
                    System.String newSopInstanceUid = (System.String)instanceMapper[oldSopInstanceUid];
                    if (newSopInstanceUid == null)
                    {
                        // Add a mapping
                        DefaultValueManager.UpdateInstantiatedDefaultTagValues(AffectedEntityEnum.InstanceEntity);
                        newSopInstanceUid = DefaultValueManager.GetInstantiatedValue(Tag.SOP_INSTANCE_UID);
                        instanceMapper.Add(oldSopInstanceUid, newSopInstanceUid);
                    }

                    // Add the new sop instance uid to the dataset
                    dataset.AddAttribute(Tag.SOP_INSTANCE_UID.GroupNumber, Tag.SOP_INSTANCE_UID.ElementNumber, VR.UI, newSopInstanceUid);

                    // Use modality Worklist Item to help construct the Storage Message
                    // Get the attribute values to copy
                    DvtkHighLevelInterface.Comparator.Comparator worklistItemComparator = new DvtkHighLevelInterface.Comparator.Comparator("WorklistItemComparator");
                    DicomComparator dicomWorklistItemComparator = worklistItemComparator.InitializeDicomComparator(modalityWorklistItem.DicomMessage);

                    // Also try to use the MPPS InProgress Message to help construct the Storage Message
                    DvtkHighLevelInterface.Comparator.Comparator mppsInProgressComparator = new DvtkHighLevelInterface.Comparator.Comparator("MppsInProgressComparator");
                    DicomComparator dicomMppsInProgressComparator = null;
                    if (_nCreateSetMppsInProgress != null)
                    {
                        dicomMppsInProgressComparator = mppsInProgressComparator.InitializeDicomComparator(_nCreateSetMppsInProgress);
                    }

                    // Create the Storage message
                    DvtkHighLevelInterface.Comparator.Comparator storageComparator = new DvtkHighLevelInterface.Comparator.Comparator("StorageComparator");
                    DvtkHighLevelInterface.Dicom.Messages.DicomMessage cStoreInstance = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CSTORERQ);
                    GenerateTriggers.MakeCStoreInstance(DefaultValueManager, cStoreInstance, dataset);
                    storageComparator.PopulateDicomMessage(cStoreInstance, dicomWorklistItemComparator);
                    if (dicomMppsInProgressComparator != null)
                    {
                        storageComparator.PopulateDicomMessage(cStoreInstance, dicomMppsInProgressComparator);
                    }

                    // Try to get the sop class uid
                    System.String sopClassUid = System.String.Empty;
                    DvtkData.Dimse.Attribute sopClassUidAttribute = dataset.GetAttribute(Tag.SOP_CLASS_UID);
                    if (sopClassUidAttribute != null)
                    {
                        UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)sopClassUidAttribute.DicomValue;
                        if (uniqueIdentifier.Values.Count > 0)
                        {
                            sopClassUid = uniqueIdentifier.Values[0];
                        }
                    }

                    // Add trigger item to the trigger
                    trigger.AddItem(cStoreInstance,
                                    sopClassUid,
                                    transferSyntaxUid);
                }
            }

            // RAD-8 - trigger the ImageArchive
            return TriggerActorInstances(ActorTypeEnum.ImageArchive, trigger, true);
        }
        #endregion SendModalityImagesStored

        #region SendStorageCommitment() overloads

        /// <summary>
        /// Send the Storage Commitment message - use the storage commitment details
        /// built up during any previous SendModalityImagesStored() operations.
        /// The N-ACTION-RQ will be sent to the Image Manager.
        /// 
        /// If the configured ActorOption1 for the DicomPeerToPeerConfiguration from the
        /// AcquisitionModalityActor to the ImageManagerActor is set to the string
        /// "DO_STORAGE_COMMITMENT_ON_SINGLE_ASSOCIATION" then the framework expects the
        /// Image Manager to return the N-EVENT-REPORT details over the same association
        /// as the N-ACTION details were sent. If the ActorOption1 configuration parameter
        /// is not set as above then the framework will expect the Image Manger to return
        /// the N-EVENT-REPORT details over a different association than the one used for the
        /// N-ACTION details.
        /// </summary>
        /// <param name="awaitNEventReport">Boolean indicating whether to wait for the
        /// N-EVENT-REPORT - either over the same or different association before returning
        /// to the caller.</param>
        /// <param name="timeOut">Time (in Seconds) to wait for the N-EVENT-REPORT to be sent
        /// from the Image Manager - only used if "awaitNEventReport" is set true.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public bool SendStorageCommitment(bool awaitNEventReport, int timeOut)
        {
            // Check if we need to wait for the Event Report
            if (awaitNEventReport == true)
            {
                // Subscribe to the event
                SubscribeStorageCommitmentNEventReportResponseEvent();
            }

            // Generate a new Storage Commitment Transaction UID
            InBuiltDefaultTagValues inbuiltDefaultTagValues = new InBuiltDefaultTagValues();
            System.String uidRoot = inbuiltDefaultTagValues.UidRoot;
            DicomTagValueAutoSetUid tagValueUid = new DicomTagValueAutoSetUid(AffectedEntityEnum.AnyEntity,
                Tag.TRANSACTION_UID, uidRoot, 1);
            _storageCommitTransactionUid = tagValueUid.Value;

            DvtkHighLevelInterface.Dicom.Messages.DicomMessage nActionStorageCommitment = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.NACTIONRQ);
            GenerateTriggers.MakeNActionStorageCommitment(_storageCommitItems, nActionStorageCommitment, _storageCommitTransactionUid, _mppsInstanceUid);

            // Trigger the N-ACTION-RQ Storage Commitment
            DicomTrigger trigger = new DicomTrigger(TransactionNameEnum.RAD_10);
            trigger.AddItem(nActionStorageCommitment,
                            "1.2.840.10008.1.20.1",
                            "1.2.840.10008.1.2");

            // RAD-10 - trigger the ImageManager
            bool result = TriggerActorInstances(ActorTypeEnum.ImageManager, trigger, true);
            if (result == true)
            {
                // Check if we need to wait for the Event Report
                if (awaitNEventReport == true)
                {
                    // Wait for the N-Event-Report response
                    // for the given timeout
                    result = WaitStorageCommitmentNEventReportResponse(timeOut);

                    // Unsubscribe to the event
                    UnsubscribeStorageCommitmentNEventReportResponseEvent();
                }
            }
            return result;
        }

        #endregion SendStorageCommitment
    }
}
