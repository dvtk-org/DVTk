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
using DvtkHighLevelInterface.Common.Messages;
using DvtkHighLevelInterface.Dicom.Messages;
using DvtkHighLevelInterface.Dicom.Other;
using VR = DvtkData.Dimse.VR;



namespace DvtkHighLevelInterface.InformationModel
{
	#region query retrieve information model names / levels

    /// <summary>
    /// All possible query retrieve information models.
    /// </summary>
	public enum QueryRetrieveInformationModelEnum
	{
        /// <summary>
        /// Patient Root Query Retrieve Information Model.
        /// </summary>
		PatientRootQueryRetrieveInformationModel,
        /// <summary>
        /// Study Root Query Retrieve Information Model.
        /// </summary>
		StudyRootQueryRetrieveInformationModel,
        /// <summary>
        /// Patient Study Only Query Retrieve Information Model.
        /// </summary>
		PatientStudyOnlyQueryRetrieveInformationModel,
        /// <summary>
        /// Unknown Information Model.
        /// </summary>
		Unknown
	}

    /// <summary>
    /// Enumerates all different query retrieve levels.
    /// </summary>
	public enum QueryRetrieveLevelEnum
	{
        /// <summary>
        /// Patient Query Retrieve Level.
        /// </summary>
		PatientQueryRetrieveLevel,
        /// <summary>
        /// Study Query Retrieve Level.
        /// </summary>
		StudyQueryRetrieveLevel,
        /// <summary>
        /// Series Query Retrieve Level.
        /// </summary>
		SeriesQueryRetrieveLevel,
        /// <summary>
        /// Instance Query Retrieve Level.
        /// </summary>
		InstanceQueryRetrieveLevel,
        /// <summary>
        /// Unknown Query Retrieve Level.
        /// </summary>
		Unknown
	}

	#endregion

	/// <summary>
	/// Summary description for GenerateTriggers.
	/// </summary>
	public class GenerateTriggers
	{
	    #region ModalityWorklistTriggers
        /// <summary>
        /// Generate a Modality Worklist Item from the user defined tags provided and
        /// the Default Value Manager.
        /// </summary>
        /// <param name="defaultValueManager">Default Value Manager.</param>
        /// <param name="userDefinedTags">User defined Tags.</param>
        /// <returns>CFind Modality Worklist Response message.</returns>
        public static DvtkHighLevelInterface.Dicom.Messages.DicomMessage MakeModalityWorklistItem(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager, 
            TagValueCollection userDefinedTags)
        {
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage worklistResponseMessage = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRSP);
			worklistResponseMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.31");

			DvtkData.Dimse.DataSet worklistItemDataset = Dvtk.Dicom.InformationEntity.Worklist.WorklistQueryDataset.CreateWorklistItemDataset(userDefinedTags);
            worklistResponseMessage.DataSet.DvtkDataDataSet = worklistItemDataset;

            // Add the default values
            AddDefaultWorklistResponseValues(defaultValueManager, worklistItemDataset);

            return worklistResponseMessage;
        }

		/// <summary>
		/// Generate a CFind Modality Worklist Query trigger from the given queryTags.
		/// All other return keys are taken from the supported return key attributes
		/// in the Worklist Information Model.
		/// </summary>
		/// <param name="queryTags">List of Query Tags.</param>
		/// <returns>CFind Modality Worklist Query trigger.</returns>
		public static DvtkHighLevelInterface.Dicom.Messages.DicomMessage MakeCFindModalityWorklist(TagValueCollection queryTags)
		{
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage queryMessage = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRQ);
			queryMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.31");

			DvtkData.Dimse.DataSet queryDataset = Dvtk.Dicom.InformationEntity.Worklist.WorklistQueryDataset.CreateWorklistQueryDataset(queryTags);
			queryMessage.DataSet.DvtkDataDataSet = queryDataset;

			return queryMessage;
		}

		/// <summary>
		/// Generate a CFind Modality Worklist Query trigger by reading the query
		/// dataset from the given mwlQueryDcmFilename. If the scheduledProcedureStepStartDate
		/// is defined (not string empty) then if a value for this attribute is present in the
		/// read datset it will be overwritten by the scheduledProcedureStepStartDate value.
		/// </summary>
		/// <param name="mwlQueryDcmFilename">DCM file containing the MWL Query Dataset.</param>
		/// <param name="scheduledProcedureStepStartDate">Optional (not sting empty) start date to overwrite dataset value.</param>
		/// <returns>CFind Modality Worklist Query trigger.</returns>
		public static DvtkHighLevelInterface.Dicom.Messages.DicomMessage MakeCFindModalityWorklist(System.String mwlQueryDcmFilename, System.String scheduledProcedureStepStartDate)
		{
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage queryMessage = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRQ);
			queryMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.31");

			DvtkData.Dimse.DataSet queryDataset = Dvtk.Dicom.InformationEntity.Worklist.WorklistQueryDataset.CreateWorklistQueryDataset(mwlQueryDcmFilename, scheduledProcedureStepStartDate);
			queryMessage.DataSet.DvtkDataDataSet = queryDataset;

			return queryMessage;
		}

        private static void AddDefaultWorklistResponseValues(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager, DvtkData.Dimse.DataSet dataset)
        {
            DvtkData.Dimse.SequenceItem scheduledProcedureStepSequenceItem = null;

            // try to get the scheduled procedure step sequence from the dataset
			DvtkData.Dimse.Attribute sequenceAttribute = dataset.GetAttribute(DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE);
            if ((sequenceAttribute != null) &&
                (sequenceAttribute.ValueRepresentation == DvtkData.Dimse.VR.SQ))
            {
                SequenceOfItems sequenceOfItems = (SequenceOfItems)sequenceAttribute.DicomValue;
                if (sequenceOfItems.Sequence.Count == 1)
                {
                    scheduledProcedureStepSequenceItem = sequenceOfItems.Sequence[0];
                }
            }

            // if the scheduled procedure step sequence is not present - add an empty one
            if (scheduledProcedureStepSequenceItem == null)
            {
                scheduledProcedureStepSequenceItem = new DvtkData.Dimse.SequenceItem();
                dataset.AddAttribute(DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE.GroupNumber,
                    DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_SEQUENCE.ElementNumber,
                    DvtkData.Dimse.VR.SQ, scheduledProcedureStepSequenceItem);
            }

            // Patient Entity default values
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_NAME, VR.PN, dataset);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.PATIENT_ID, VR.LO, dataset);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_BIRTH_DATE, VR.DA, dataset);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_SEX, VR.CS, dataset);

            // Imaging Service Request Entity default values
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.ACCESSION_NUMBER, VR.SH, dataset);

            // Requested Procedure Entity default values
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.REQUESTED_PROCEDURE_ID, VR.SH, dataset);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.REQUESTED_PROCEDURE_DESCRIPTION, VR.LO, dataset);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.STUDY_INSTANCE_UID, VR.UI, dataset);

            // Scheduled Procedure Step Entity default values
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.MODALITY, VR.CS, scheduledProcedureStepSequenceItem);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH, scheduledProcedureStepSequenceItem);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO, scheduledProcedureStepSequenceItem);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_START_DATE, VR.DA, scheduledProcedureStepSequenceItem);
            AddDefaultValueEvenIfZeroLength(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_START_TIME, VR.TM, scheduledProcedureStepSequenceItem);
        }

		#endregion ModalityWorklistTriggers

		#region MppsInProgressTriggers

		/// <summary>
		/// Generate an N-CREATE/N-SET MPPS IN-PROGRESS trigger from the default value manager.
		/// Use the given mppsInstanceUid.
		/// </summary>
		/// <param name="defaultValueManager">Default Value Manager.</param>
		/// <param name="mppsInProgressMessage">Mpps In-Progress message.</param>
		/// <param name="mppsInstanceUid">Mpps Instance UID.</param>
		public static void MakeMppsInProgress(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage mppsInProgressMessage,
			System.String mppsInstanceUid)
		{
			if (mppsInProgressMessage.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.NCREATERQ)
			{
				// Set Affected UIDs
				mppsInProgressMessage.Set("0x00000002", VR.UI, "1.2.840.10008.3.1.2.3.3");
				mppsInProgressMessage.Set("0x00001000", VR.UI, mppsInstanceUid);
			}
			else
			{
				// Set Requested UIDs
				mppsInProgressMessage.Set("0x00000003", VR.UI, "1.2.840.10008.3.1.2.3.3");
				mppsInProgressMessage.Set("0x00001001", VR.UI, mppsInstanceUid);
			}

			// Add the default values to the MppsInProgress message
			AddDefaultMppsInProgressValues(defaultValueManager, mppsInProgressMessage.DataSet.DvtkDataDataSet);
		}

		private static void AddDefaultMppsInProgressValues(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager, DvtkData.Dimse.DataSet dataset)
		{
			DvtkData.Dimse.SequenceItem scheduledStepAttributesSequenceItem = new DvtkData.Dimse.SequenceItem();

			// Patient Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_NAME, VR.PN, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENT_ID, VR.LO, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_BIRTH_DATE, VR.DA, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_SEX, VR.CS, dataset);

			// Study Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.STUDY_ID, VR.SH, dataset);

			// Requested Procedure Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.REQUESTED_PROCEDURE_ID, VR.SH, scheduledStepAttributesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.REQUESTED_PROCEDURE_DESCRIPTION, VR.LO, scheduledStepAttributesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.STUDY_INSTANCE_UID, VR.UI, scheduledStepAttributesSequenceItem);

			// Scheduled Procedure Step Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.MODALITY, VR.CS, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH, scheduledStepAttributesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO, scheduledStepAttributesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_START_DATE, VR.DA, scheduledStepAttributesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_START_TIME, VR.TM, scheduledStepAttributesSequenceItem);

			// Performed Procedure Step Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_ID, VR.SH, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_DESCRIPTION, VR.LO, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_DATE, VR.DA, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_TIME, VR.TM, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_STATION_AE_TITLE, VR.AE, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_STATION_NAME, VR.SH, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_LOCATION, VR.SH, dataset);

			// Add in zero length attributes
			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_DATE.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_DATE.ElementNumber,
				DvtkData.Dimse.VR.DA);
			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_TIME.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_TIME.ElementNumber,
				DvtkData.Dimse.VR.TM);
			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_TYPE_DESCRIPTION.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_TYPE_DESCRIPTION.ElementNumber,
				DvtkData.Dimse.VR.LO);
			dataset.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_PATIENT_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.REFERENCED_PATIENT_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ);
			dataset.AddAttribute(DvtkData.Dimse.Tag.PROCEDURE_CODE_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.PROCEDURE_CODE_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ);
			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_ACTION_ITEM_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_ACTION_ITEM_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ);
			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ);

			scheduledStepAttributesSequenceItem.AddAttribute(DvtkData.Dimse.Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.SCHEDULED_ACTION_ITEM_CODE_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ);
			scheduledStepAttributesSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_STUDY_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.REFERENCED_STUDY_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ);

			dataset.AddAttribute(DvtkData.Dimse.Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.SCHEDULED_STEP_ATTRIBUTES_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ, scheduledStepAttributesSequenceItem);
			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_STATUS.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_STATUS.ElementNumber,
				DvtkData.Dimse.VR.CS, "IN PROGRESS");
		}

		/// <summary>
		/// Generate an N-CREATE/N-SET MPPS IN-PROGRESS trigger from the given mppsInProgressDcmFilename.
		/// Use the given mppsInstanceUid.
		/// </summary>
		/// <param name="mppsInProgressDcmFilename"></param>
		/// <param name="defaultValueManager">Default Value Manager.</param>
		/// <param name="mppsInProgressMessage">Mpps In-Progress message.</param>
		/// <param name="mppsInstanceUid">Mpps Instance UID.</param>
		public static void MakeMppsInProgress(System.String mppsInProgressDcmFilename,
			Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage mppsInProgressMessage,
			System.String mppsInstanceUid)
		{
			if (mppsInProgressMessage.CommandSet.DimseCommand == DvtkData.Dimse.DimseCommand.NCREATERQ)
			{
				// Set Affected UIDs
				mppsInProgressMessage.Set("0x00000002", VR.UI, "1.2.840.10008.3.1.2.3.3");
				mppsInProgressMessage.Set("0x00001000", VR.UI, mppsInstanceUid);
			}
			else
			{
				// Set Requested UIDs
				mppsInProgressMessage.Set("0x00000003", VR.UI, "1.2.840.10008.3.1.2.3.3");
				mppsInProgressMessage.Set("0x00001001", VR.UI, mppsInstanceUid);
			}

			// Read the DCM file
			mppsInProgressMessage.DataSet.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(mppsInProgressDcmFilename);

			// Update the procedure step start date & time values
			UpdateStartDateTimeValues(defaultValueManager, mppsInProgressMessage.DataSet.DvtkDataDataSet);
		}

		private static void UpdateStartDateTimeValues(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager, DvtkData.Dimse.DataSet dataset)
		{
			DvtkData.Dimse.Attribute ppsStartDate = dataset.GetAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_DATE);
			if (ppsStartDate != null)
			{
				dataset.Remove(ppsStartDate);
			}
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_DATE, VR.DA, dataset);

			DvtkData.Dimse.Attribute ppsStartTime = dataset.GetAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_TIME);
			if (ppsStartTime != null)
			{
				dataset.Remove(ppsStartTime);
			}
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_TIME, VR.TM, dataset);
		}

		#endregion MppsInProgressTriggers

		#region MppsCompletedDiscontinuedTriggers
	
		/// <summary>
		/// Generate an NSet MPPS Completed/Discontinued by taking the default values from the Default
		/// Value Manager.
		/// </summary>
		/// <param name="defaultValueManager">Used to get the default values.</param>
		/// <param name="storageCommitItems">Corresponding Storage Commitment items.</param>
		/// <param name="mppsCompletedDiscontinuedMessage">MPPS Completed/Discontinued message being populated.</param>
		/// <param name="mppsInstanceUid">MPPS Instance UID.</param>
		/// <param name="mppsStatus"></param>
		public static void MakeNSetMppsCompletedDiscontinued(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			ReferencedSopItemCollection storageCommitItems,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage mppsCompletedDiscontinuedMessage,
			System.String mppsInstanceUid,
			System.String mppsStatus)
		{
			mppsCompletedDiscontinuedMessage.Set("0x00000003", VR.UI, "1.2.840.10008.3.1.2.3.3");
			mppsCompletedDiscontinuedMessage.Set("0x00001001", VR.UI, mppsInstanceUid);

			// Add the default values to the MppsCompletedDiscontinued message
			AddDefaultMppsCompletedDiscontinuedValues(defaultValueManager, storageCommitItems, mppsCompletedDiscontinuedMessage.DataSet.DvtkDataDataSet, mppsStatus);
		}

        /// <summary>
        /// Generate an NSet MPPS Completed/Discontinued by taking the default values from the Default
        /// Value Manager.
        /// </summary>
        /// <param name="mppsCompletedDiscontinuedDcmFilename"></param>
        /// <param name="defaultValueManager">Used to get the default values.</param>
        /// <param name="storageCommitItems">Corresponding Storage Commitment items.</param>
        /// <param name="mppsCompletedDiscontinuedMessage">MPPS Completed/Discontinued message being populated.</param>
        /// <param name="mppsInstanceUid">MPPS Instance UID.</param>
        /// <param name="retrieveAETitle">Value of RestrieveAETitle attribute</param>
        public static void MakeNSetMppsCompletedDiscontinued(System.String mppsCompletedDiscontinuedDcmFilename,
            Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
            ReferencedSopItemCollection storageCommitItems,
            DvtkHighLevelInterface.Dicom.Messages.DicomMessage mppsCompletedDiscontinuedMessage,
            System.String mppsInstanceUid, string retrieveAETitle, string seriesDescription)
        {
            mppsCompletedDiscontinuedMessage.Set("0x00000003", VR.UI, "1.2.840.10008.3.1.2.3.3");
            mppsCompletedDiscontinuedMessage.Set("0x00001001", VR.UI, mppsInstanceUid);

            // Read the DCM file
            mppsCompletedDiscontinuedMessage.DataSet.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(mppsCompletedDiscontinuedDcmFilename);

            // Update the procedure step end date & time values
            UpdateEndDateTimeValues(defaultValueManager, mppsCompletedDiscontinuedMessage.DataSet.DvtkDataDataSet);

            // Add the storage commitment items
            AddStorageCommitItems(defaultValueManager, storageCommitItems, mppsCompletedDiscontinuedMessage.DataSet.DvtkDataDataSet, retrieveAETitle, seriesDescription);
        }

		private static void AddDefaultMppsCompletedDiscontinuedValues(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			ReferencedSopItemCollection storageCommitItems,
			DvtkData.Dimse.DataSet dataset,
			System.String mppsStatus)
		{
			DvtkData.Dimse.SequenceItem performedSeriesSequenceItem = new DvtkData.Dimse.SequenceItem();

			// Series Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PROTOCOL_NAME, VR.LO, performedSeriesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.RETRIEVE_AE_TITLE, VR.AE, performedSeriesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SERIES_DESCRIPTION, VR.LO, performedSeriesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMING_PHYSICIANS_NAME, VR.PN, performedSeriesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.OPERATORS_NAME, VR.PN, performedSeriesSequenceItem);

			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SERIES_INSTANCE_UID, VR.UI, performedSeriesSequenceItem);

			performedSeriesSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_STANDALONE_SOP_INSTANCE_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.REFERENCED_STANDALONE_SOP_INSTANCE_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ);

			// Performed Procedure Step Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_DATE, VR.DA, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_TIME, VR.TM, dataset);

			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.COMMENTS_ON_THE_PERFORMED_PROCEDURE_STEPS) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.COMMENTS_ON_THE_PERFORMED_PROCEDURE_STEPS, VR.ST, dataset);
			}

			AddReferencedSopSequence(storageCommitItems, 0x00081140, performedSeriesSequenceItem, InstanceStateEnum.InstanceMppsCompleted);

            DvtkData.Dimse.Attribute attribute = performedSeriesSequenceItem.GetAttribute(DvtkData.Dimse.Tag.SERIES_DESCRIPTION);
            performedSeriesSequenceItem.Remove(attribute);
            System.String lValue = defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.SERIES_DESCRIPTION);
            performedSeriesSequenceItem.AddAttribute(DvtkData.Dimse.Tag.SERIES_DESCRIPTION.GroupNumber, DvtkData.Dimse.Tag.SERIES_DESCRIPTION.ElementNumber, (DvtkData.Dimse.VR)VR.LO, lValue);

			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ, performedSeriesSequenceItem);

			dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_STATUS.GroupNumber,
				DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_STATUS.ElementNumber,
				VR.CS, mppsStatus);

			// Add any RadiationDoseDefaultValues default values
			AddRadiationDoseDefaultValues(defaultValueManager, dataset);
		}

		private static void AddRadiationDoseDefaultValues(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			DvtkData.Dimse.DataSet dataset)
		{
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.DISTANCE_SOURCE_TO_DETECTOR) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.DISTANCE_SOURCE_TO_DETECTOR, VR.DS, dataset);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.IMAGE_AREA_DOSE_PRODUCT) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.IMAGE_AREA_DOSE_PRODUCT, VR.DS, dataset);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.TOTAL_TIME_OF_FLUOROSCOPY) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.TOTAL_TIME_OF_FLUOROSCOPY, VR.US, dataset);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.TOTAL_NUMBER_OF_EXPOSURES) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.TOTAL_NUMBER_OF_EXPOSURES, VR.US, dataset);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.ENTRANCE_DOSE) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.ENTRANCE_DOSE, VR.US, dataset);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.EXPOSED_AREA) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.EXPOSED_AREA, VR.US, dataset);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.DISTANCE_SOURCE_TO_ENTRANCE) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.DISTANCE_SOURCE_TO_ENTRANCE, VR.DS, dataset);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.COMMENTS_ON_RADIATION_DOSE) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.COMMENTS_ON_RADIATION_DOSE, VR.ST, dataset);
			}

			DvtkData.Dimse.SequenceItem exposeureDoseSequenceItem = new DvtkData.Dimse.SequenceItem();

			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.KVP) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.KVP, VR.DS, exposeureDoseSequenceItem);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.EXPOSURE_TIME) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.EXPOSURE_TIME, VR.IS, exposeureDoseSequenceItem);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.RADIATION_MODE) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.RADIATION_MODE, VR.CS, exposeureDoseSequenceItem);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.FILTER_TYPE) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.FILTER_TYPE, VR.SH, exposeureDoseSequenceItem);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.FILTER_MATERIAL) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.FILTER_MATERIAL, VR.CS, exposeureDoseSequenceItem);
			}
			if (defaultValueManager.GetInstantiatedValue(DvtkData.Dimse.Tag.X_RAY_TUBE_CURRENT_IN_UA) != System.String.Empty)
			{
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.X_RAY_TUBE_CURRENT_IN_UA, VR.DS, exposeureDoseSequenceItem);
			}

			if (exposeureDoseSequenceItem.Count > 0)
			{
				dataset.AddAttribute(DvtkData.Dimse.Tag.EXPOSURE_DOSE_SEQUENCE.GroupNumber,
					DvtkData.Dimse.Tag.EXPOSURE_DOSE_SEQUENCE.ElementNumber,
					VR.SQ, exposeureDoseSequenceItem);
			}
		}
		
		/// <summary>
		/// Generate an NSet MPPS Completed/Discontinued by taking the default values from the given
		/// DCM file. The default manager is required to get the appropriate Series Instance UID.
		/// </summary>
		/// <param name="mppsCompletedDiscontinuedDcmFilename">Used to get the dataset default values.</param>
		/// <param name="defaultValueManager">For the Series Instance UID.</param>
		/// <param name="storageCommitItems">Corresponding Storage Commitment items.</param>
		/// <param name="mppsCompletedDiscontinuedMessage">MPPS Completed/Discontinued message being populated.</param>
		/// <param name="mppsInstanceUid">MPPS Instance UID.</param>
		public static void MakeNSetMppsCompletedDiscontinued(System.String mppsCompletedDiscontinuedDcmFilename,
			Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			ReferencedSopItemCollection storageCommitItems,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage mppsCompletedDiscontinuedMessage,
			System.String mppsInstanceUid)
		{
			mppsCompletedDiscontinuedMessage.Set("0x00000003", VR.UI, "1.2.840.10008.3.1.2.3.3");
			mppsCompletedDiscontinuedMessage.Set("0x00001001", VR.UI, mppsInstanceUid);

			// Read the DCM file
			mppsCompletedDiscontinuedMessage.DataSet.DvtkDataDataSet = Dvtk.DvtkDataHelper.ReadDataSetFromFile(mppsCompletedDiscontinuedDcmFilename);

			// Update the procedure step end date & time values
			UpdateEndDateTimeValues(defaultValueManager, mppsCompletedDiscontinuedMessage.DataSet.DvtkDataDataSet);

			// Add the storage commitment items
			AddStorageCommitItems(defaultValueManager, storageCommitItems, mppsCompletedDiscontinuedMessage.DataSet.DvtkDataDataSet);
		}

		private static void UpdateEndDateTimeValues(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager, DvtkData.Dimse.DataSet dataset)
		{
			DvtkData.Dimse.Attribute ppsEndDate = dataset.GetAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_DATE);
			if (ppsEndDate != null)
			{
				dataset.Remove(ppsEndDate);
			}
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_DATE, VR.DA, dataset);

			DvtkData.Dimse.Attribute ppsEndTime = dataset.GetAttribute(DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_TIME);
			if (ppsEndTime != null)
			{
				dataset.Remove(ppsEndTime);
			}
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_END_TIME, VR.TM, dataset);
		}

        private static void AddStorageCommitItems(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
            ReferencedSopItemCollection storageCommitItems,
            DvtkData.Dimse.DataSet dataset, string retrieveAETitle, string seriesDescription)
        {
            DvtkData.Dimse.SequenceItem performedSeriesSequenceItem = null;

            // Try to get the Performed Series Sequence
            DvtkData.Dimse.Attribute performedSeriesSequence = dataset.GetAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE);
            if (performedSeriesSequence != null)
            {
                DvtkData.Dimse.SequenceOfItems sequenceOfItems = (DvtkData.Dimse.SequenceOfItems)performedSeriesSequence.DicomValue;
                if (sequenceOfItems.Sequence.Count == 1)
                {
                    performedSeriesSequenceItem = sequenceOfItems.Sequence[0];
                }
                else
                {
                    dataset.Remove(performedSeriesSequence);

                    performedSeriesSequenceItem = new DvtkData.Dimse.SequenceItem();

                    dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.GroupNumber,
                        DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.ElementNumber,
                        DvtkData.Dimse.VR.SQ, performedSeriesSequenceItem);
                }
            }
            else
            {
                performedSeriesSequenceItem = new DvtkData.Dimse.SequenceItem();

                dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.GroupNumber,
                    DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.ElementNumber,
                    DvtkData.Dimse.VR.SQ, performedSeriesSequenceItem);
            }

            if (performedSeriesSequenceItem != null)
            {
                // Series Entity default values
                DvtkData.Dimse.Attribute seriesInstanceUid = performedSeriesSequenceItem.GetAttribute(DvtkData.Dimse.Tag.SERIES_INSTANCE_UID);
                if (seriesInstanceUid != null)
                {
                    performedSeriesSequenceItem.Remove(seriesInstanceUid);
                }
                AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SERIES_INSTANCE_UID, VR.UI, performedSeriesSequenceItem);

                AddReferencedSopSequence(storageCommitItems, 0x00081140, performedSeriesSequenceItem, InstanceStateEnum.InstanceMppsCompleted);

                DvtkData.Dimse.Attribute retrieveAE = performedSeriesSequenceItem.GetAttribute(DvtkData.Dimse.Tag.RETRIEVE_AE_TITLE);
                if (retrieveAE != null)
                {
                    performedSeriesSequenceItem.Remove(retrieveAE);
                }
                performedSeriesSequenceItem.AddAttribute(DvtkData.Dimse.Tag.RETRIEVE_AE_TITLE.GroupNumber, DvtkData.Dimse.Tag.RETRIEVE_AE_TITLE.ElementNumber, DvtkData.Dimse.VR.AE, retrieveAETitle);

                DvtkData.Dimse.Attribute seriesDescriptionAttr = performedSeriesSequenceItem.GetAttribute(DvtkData.Dimse.Tag.SERIES_DESCRIPTION);
                if (seriesDescriptionAttr != null)
                {
                    performedSeriesSequenceItem.Remove(seriesDescriptionAttr);
                }
                performedSeriesSequenceItem.AddAttribute(DvtkData.Dimse.Tag.SERIES_DESCRIPTION.GroupNumber, DvtkData.Dimse.Tag.SERIES_DESCRIPTION.ElementNumber, DvtkData.Dimse.VR.LO, seriesDescription);
            }
        }

		private static void AddStorageCommitItems(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			ReferencedSopItemCollection storageCommitItems,
			DvtkData.Dimse.DataSet dataset)
		{
			DvtkData.Dimse.SequenceItem performedSeriesSequenceItem = null;

			// Try to get the Performed Series Sequence
			DvtkData.Dimse.Attribute performedSeriesSequence = dataset.GetAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE);
			if (performedSeriesSequence != null)
			{
				DvtkData.Dimse.SequenceOfItems sequenceOfItems = (DvtkData.Dimse.SequenceOfItems)performedSeriesSequence.DicomValue;
				if (sequenceOfItems.Sequence.Count == 1)
				{
					performedSeriesSequenceItem = sequenceOfItems.Sequence[0];
				}
				else
				{
					dataset.Remove(performedSeriesSequence);

					performedSeriesSequenceItem = new DvtkData.Dimse.SequenceItem();

					dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.GroupNumber,
						DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.ElementNumber,
						DvtkData.Dimse.VR.SQ, performedSeriesSequenceItem);
				}
			}
			else
			{
				performedSeriesSequenceItem = new DvtkData.Dimse.SequenceItem();

				dataset.AddAttribute(DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.GroupNumber,
					DvtkData.Dimse.Tag.PERFORMED_SERIES_SEQUENCE.ElementNumber,
					DvtkData.Dimse.VR.SQ, performedSeriesSequenceItem);
			}

			if (performedSeriesSequenceItem != null)
			{
				// Series Entity default values
				DvtkData.Dimse.Attribute seriesInstanceUid = performedSeriesSequenceItem.GetAttribute(DvtkData.Dimse.Tag.SERIES_INSTANCE_UID);
				if (seriesInstanceUid != null)
				{
					performedSeriesSequenceItem.Remove(seriesInstanceUid);
				}
				AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SERIES_INSTANCE_UID, VR.UI, performedSeriesSequenceItem);

				AddReferencedSopSequence(storageCommitItems, 0x00081140, performedSeriesSequenceItem, InstanceStateEnum.InstanceMppsCompleted);
			}
		}

		#endregion MppsCompletedDiscontinuedTriggers

		#region CStoreInstanceTriggers

        /// <summary>
        /// Make a C-STORE instance.
        /// </summary>
        /// <param name="defaultValueManager">The default value manager.</param>
        /// <param name="storeInstanceMessage">The C-STORE message instance.</param>
		public static void MakeCStoreInstance(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage storeInstanceMessage)
		{
			// Add the required values to the StoreInstance message
			AddStoreInstanceValues(defaultValueManager,
				storeInstanceMessage.DataSet.DvtkDataDataSet, true);
		}

        /// <summary>
        /// Make a C-STORE instance.
        /// </summary>
        /// <param name="defaultValueManager">The default value manager.</param>
        /// <param name="storeInstanceMessage">The C-STORE message instance.</param>
        /// <param name="dataset">The dataset.</param>
		public static void MakeCStoreInstance(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage storeInstanceMessage,
			DvtkData.Dimse.DataSet dataset)
		{
			// Add the dataset to the storage message
			storeInstanceMessage.DataSet.DvtkDataDataSet = dataset;

			// Update these dates (if present in the dataset)
			System.String dateNow = System.DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
			UpdateValue(dateNow, DvtkData.Dimse.Tag.STUDY_DATE, VR.DA, dataset);
			UpdateValue(dateNow, DvtkData.Dimse.Tag.SERIES_DATE, VR.DA, dataset);
			UpdateValue(dateNow, DvtkData.Dimse.Tag.ACQUISITION_DATE, VR.DA, dataset);
			UpdateValue(dateNow, DvtkData.Dimse.Tag.IMAGE_DATE, VR.DA, dataset);
			UpdateValue(dateNow, DvtkData.Dimse.Tag.OVERLAY_DATE, VR.DA, dataset);
			UpdateValue(dateNow, DvtkData.Dimse.Tag.CURVE_DATE, VR.DA, dataset);
			UpdateValue(dateNow, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_DATE, VR.DA, dataset);

			// Update these times (if present in the dataset)
			System.String timeNow = System.DateTime.Now.ToString("HHmmss", System.Globalization.CultureInfo.InvariantCulture);
			UpdateValue(timeNow, DvtkData.Dimse.Tag.STUDY_TIME, VR.TM, dataset);
			UpdateValue(timeNow, DvtkData.Dimse.Tag.SERIES_TIME, VR.TM, dataset);
			UpdateValue(timeNow, DvtkData.Dimse.Tag.ACQUISITION_TIME, VR.TM, dataset);
			UpdateValue(timeNow, DvtkData.Dimse.Tag.IMAGE_TIME, VR.TM, dataset);
			UpdateValue(timeNow, DvtkData.Dimse.Tag.OVERLAY_TIME, VR.TM, dataset);
			UpdateValue(timeNow, DvtkData.Dimse.Tag.CURVE_TIME, VR.TM, dataset);
			UpdateValue(timeNow, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_TIME, VR.TM, dataset);

			// Add the required default values to the dataset
			AddStoreInstanceValues(defaultValueManager, dataset, false);
		}

		private static void AddStoreInstanceValues(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			DvtkData.Dimse.DataSet dataset,
			bool generateImageData)
		{
			DvtkData.Dimse.SequenceItem requestAttributesSequenceItem = new DvtkData.Dimse.SequenceItem();

			// Patient Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_NAME, VR.PN, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENT_ID, VR.LO, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_BIRTH_DATE, VR.DA, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PATIENTS_SEX, VR.CS, dataset);

			// Study Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.STUDY_DESCRIPTION, VR.LO, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.STUDY_ID, VR.SH, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.STUDY_DATE, VR.DA, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.STUDY_TIME, VR.TM, dataset);

			// Series Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PROTOCOL_NAME, VR.LO, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SERIES_DESCRIPTION, VR.LO, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMING_PHYSICIANS_NAME, VR.PN, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.OPERATORS_NAME, VR.PN, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SERIES_INSTANCE_UID, VR.UI, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SERIES_NUMBER, VR.IS, dataset);

			// Instance Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SOP_INSTANCE_UID, VR.UI, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.INSTANCE_NUMBER, VR.IS, dataset);

			// Image Service Request Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.ACCESSION_NUMBER, VR.SH, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.REFERRING_PHYSICIANS_NAME, VR.PN, dataset);

			// Requested Procedure Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.REQUESTED_PROCEDURE_ID, VR.SH, requestAttributesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.STUDY_INSTANCE_UID, VR.UI, dataset);

			// Scheduled Procedure Step Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.MODALITY, VR.CS, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_ID, VR.SH, requestAttributesSequenceItem);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.SCHEDULED_PROCEDURE_STEP_DESCRIPTION, VR.LO, requestAttributesSequenceItem);

			// Performed Procedure Step Entity default values
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_ID, VR.SH, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_DESCRIPTION, VR.LO, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_DATE, VR.DA, dataset);
			AddDefaultValue(defaultValueManager, DvtkData.Dimse.Tag.PERFORMED_PROCEDURE_STEP_START_TIME, VR.TM, dataset);

			// Check if the Request Attribute Sequence is already present
			DvtkData.Dimse.Attribute requestAttributesSequence = dataset.GetAttribute(DvtkData.Dimse.Tag.REQUEST_ATTRIBUTES_SEQUENCE);
			if (requestAttributesSequence != null)
			{
				// Remove the existing (old) sequence
				dataset.Remove(requestAttributesSequence);
			}

			// Add the new sequence
			dataset.AddAttribute(DvtkData.Dimse.Tag.REQUEST_ATTRIBUTES_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.REQUEST_ATTRIBUTES_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ, requestAttributesSequenceItem);

			// Check if we need to generate the image data too (including pixel data)
			if (generateImageData == true)
			{
				// Add Secondary Capture image data
				dataset.AddAttribute(DvtkData.Dimse.Tag.SOP_CLASS_UID.GroupNumber,
					DvtkData.Dimse.Tag.SOP_CLASS_UID.ElementNumber,
					DvtkData.Dimse.VR.UI, "1.2.840.10008.5.1.4.1.1.7");

				dataset.AddAttribute(DvtkData.Dimse.Tag.CONVERSION_TYPE.GroupNumber,
					DvtkData.Dimse.Tag.CONVERSION_TYPE.ElementNumber,
					DvtkData.Dimse.VR.CS, "DV");
				dataset.AddAttribute(DvtkData.Dimse.Tag.PATIENT_ORIENTATION.GroupNumber,
					DvtkData.Dimse.Tag.PATIENT_ORIENTATION.ElementNumber,
					DvtkData.Dimse.VR.CS);

				dataset.AddAttribute(DvtkData.Dimse.Tag.SAMPLES_PER_PIXEL.GroupNumber,
					DvtkData.Dimse.Tag.SAMPLES_PER_PIXEL.ElementNumber,
					DvtkData.Dimse.VR.US, 1);
				dataset.AddAttribute(DvtkData.Dimse.Tag.PHOTOMETRIC_INTERPRETATION.GroupNumber,
					DvtkData.Dimse.Tag.PHOTOMETRIC_INTERPRETATION.ElementNumber,
					DvtkData.Dimse.VR.CS, "MONOCHROME2");
				dataset.AddAttribute(DvtkData.Dimse.Tag.ROWS.GroupNumber,
					DvtkData.Dimse.Tag.ROWS.ElementNumber,
					DvtkData.Dimse.VR.US, 512);
				dataset.AddAttribute(DvtkData.Dimse.Tag.COLUMNS.GroupNumber,
					DvtkData.Dimse.Tag.COLUMNS.ElementNumber,
					DvtkData.Dimse.VR.US, 512);
				dataset.AddAttribute(DvtkData.Dimse.Tag.BITS_ALLOCATED.GroupNumber,
					DvtkData.Dimse.Tag.BITS_ALLOCATED.ElementNumber,
					DvtkData.Dimse.VR.US, 8);
				dataset.AddAttribute(DvtkData.Dimse.Tag.BITS_STORED.GroupNumber,
					DvtkData.Dimse.Tag.BITS_STORED.ElementNumber,
					DvtkData.Dimse.VR.US, 8);
				dataset.AddAttribute(DvtkData.Dimse.Tag.HIGH_BIT.GroupNumber,
					DvtkData.Dimse.Tag.HIGH_BIT.ElementNumber,
					DvtkData.Dimse.VR.US, 7);
				dataset.AddAttribute(DvtkData.Dimse.Tag.PIXEL_REPRESENTATION.GroupNumber,
					DvtkData.Dimse.Tag.PIXEL_REPRESENTATION.ElementNumber,
					DvtkData.Dimse.VR.US, 0);

				dataset.AddAttribute(DvtkData.Dimse.Tag.PIXEL_DATA.GroupNumber,
					DvtkData.Dimse.Tag.PIXEL_DATA.ElementNumber,
					DvtkData.Dimse.VR.OB, 512);
			}
		}

        /// <summary>
        /// Handle C-STORE responses.
        /// </summary>
        /// <param name="storageCommitItems">The storage commitment items.</param>
        /// <param name="cStoreResponses">The C-STORE responses.</param>
		public static void HandleCStoreResponses(ReferencedSopItemCollection storageCommitItems, DvtkHighLevelInterface.Dicom.Messages.DicomMessageCollection cStoreResponses)
		{
			foreach(DvtkHighLevelInterface.Dicom.Messages.DicomMessage cStoreRsp in cStoreResponses)
			{
				AddSopData(storageCommitItems, cStoreRsp);
			}
		}

		private static void AddSopData(ReferencedSopItemCollection storageCommitItems, DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage)
		{
			Values values = dicomMessage.CommandSet["0x00000002"].Values;
			System.String sopClassUid = values[0];
			values = dicomMessage.CommandSet["0x00001000"].Values;
			System.String sopInstanceUid = values[0];
			ReferencedSopItem storageCommitItem = storageCommitItems.Find(sopClassUid, sopInstanceUid);
			if (storageCommitItem == null)
			{
				storageCommitItem = new ReferencedSopItem(sopClassUid, sopInstanceUid);
				storageCommitItem.InstanceState = InstanceStateEnum.InstanceStored;
				storageCommitItems.Add(storageCommitItem);
			}
		}

		#endregion CStoreInstanceTriggers

		#region StorageCommitTriggers

        /// <summary>
        /// Cretae a storage commitment event.
        /// </summary>
        /// <param name="informationModels">The information models.</param>
        /// <param name="actionMessage">The action message.</param>
        /// <returns>The created event.</returns>
		public static DvtkHighLevelInterface.Dicom.Messages.DicomMessage MakeStorageCommitEvent(QueryRetrieveInformationModels informationModels, DvtkHighLevelInterface.Dicom.Messages.DicomMessage actionMessage)
		{
			// refresh the information models
			informationModels.Refresh();

			DvtkHighLevelInterface.Dicom.Messages.DicomMessage eventMessage = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.NEVENTREPORTRQ);
			eventMessage.Set("0x00000002", VR.UI, "1.2.840.10008.1.20.1");
			eventMessage.Set("0x00001000", VR.UI, "1.2.840.10008.1.20.1.1");
			
			DvtkData.Dimse.DataSet actionDataset = actionMessage.DataSet.DvtkDataDataSet;

			DvtkData.Dimse.DataSet eventDataset = new DvtkData.Dimse.DataSet();

			DvtkData.Dimse.Attribute eventReferenceSopSequence = new DvtkData.Dimse.Attribute(0x00081199, DvtkData.Dimse.VR.SQ);
			SequenceOfItems eventReferenceSopSequenceOfItems = new SequenceOfItems();
			eventReferenceSopSequence.DicomValue = eventReferenceSopSequenceOfItems;

			DvtkData.Dimse.Attribute eventFailedSopSequence = new DvtkData.Dimse.Attribute(0x00081198, DvtkData.Dimse.VR.SQ);
			SequenceOfItems eventFailedSopSequenceOfItems = new SequenceOfItems();
			eventFailedSopSequence.DicomValue = eventFailedSopSequenceOfItems;

			if (actionDataset != null)
			{
				DvtkData.Dimse.Attribute transactionUid = actionDataset.GetAttribute(DvtkData.Dimse.Tag.TRANSACTION_UID);
				if (transactionUid != null)
				{
					eventDataset.Add(transactionUid);
				}

				DvtkData.Dimse.Attribute referencedSopSequence = actionDataset.GetAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_SEQUENCE);
				if (referencedSopSequence != null)
				{
					SequenceOfItems sequenceOfItems = (SequenceOfItems)referencedSopSequence.DicomValue;
					foreach(DvtkData.Dimse.SequenceItem item in sequenceOfItems.Sequence)
					{
						System.String sopClassUid = "";
						System.String sopInstanceUid = "";

						DvtkData.Dimse.Attribute attribute = item.GetAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID);
						if (attribute != null)
						{
							UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
							if (uniqueIdentifier.Values.Count > 0)
							{
								sopClassUid = uniqueIdentifier.Values[0];
							}
						}

						attribute = item.GetAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID);
						if (attribute != null)
						{
							UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
							if (uniqueIdentifier.Values.Count > 0)
							{
								sopInstanceUid = uniqueIdentifier.Values[0];
							}
						}

						if (informationModels.PatientRoot.IsInstanceInInformationModel(sopClassUid, sopInstanceUid))
						{
							DvtkData.Dimse.SequenceItem itemOk = new DvtkData.Dimse.SequenceItem();
							itemOk.AddAttribute(0x00081150, DvtkData.Dimse.VR.UI, sopClassUid);
							itemOk.AddAttribute(0x00081155, DvtkData.Dimse.VR.UI, sopInstanceUid);

							// add instance to committed list
							eventReferenceSopSequenceOfItems.Sequence.Add(itemOk);
						}
						else
						{
							DvtkData.Dimse.SequenceItem itemNotOk = new DvtkData.Dimse.SequenceItem();
							itemNotOk.AddAttribute(0x00081150, DvtkData.Dimse.VR.UI, sopClassUid);
							itemNotOk.AddAttribute(0x00081155, DvtkData.Dimse.VR.UI, sopInstanceUid);
							itemNotOk.AddAttribute(0x00081197, DvtkData.Dimse.VR.US, 0x0110);

							// add instance to failed list
							eventFailedSopSequenceOfItems.Sequence.Add(itemNotOk);
						}
					}
				}

				if (eventReferenceSopSequenceOfItems.Sequence.Count > 0)
				{
                    eventMessage.Set("0x00001002", VR.US, 1);
					eventDataset.Add(eventReferenceSopSequence);
				}

				if (eventFailedSopSequenceOfItems.Sequence.Count > 0)
				{
                    eventMessage.Set("0x00001002", VR.US, 2);
					eventDataset.Add(eventFailedSopSequence);
				}
			}

			eventMessage.DataSet.DvtkDataDataSet = eventDataset;

			return eventMessage;
		}

        /// <summary>
        /// Handle N-EVENT-REPORT for storage commitment.
        /// </summary>
        /// <param name="storageCommitItems">The storage commitment items.</param>
        /// <param name="storageCommitmentMessage">The storage commitment message.</param>
		public static void HandleNEventReportStorageCommitment(ReferencedSopItemCollection storageCommitItems,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage storageCommitmentMessage)
		{
			DvtkData.Dimse.DataSet dataset = storageCommitmentMessage.DataSet.DvtkDataDataSet;
			if (dataset != null)
			{
				// Try to get the successfully stored instances
				DvtkData.Dimse.Attribute eventReferenceSopSequence = dataset.GetAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_SEQUENCE);
				if (eventReferenceSopSequence != null)
				{
					SequenceOfItems sequenceOfItems = (SequenceOfItems)eventReferenceSopSequence.DicomValue;
					foreach (DvtkData.Dimse.SequenceItem item in sequenceOfItems.Sequence)
					{
						UpdateReferencedSopItem(storageCommitItems, item, InstanceStateEnum.InstanceStorageCommitReportedSuccess);
					}
				}

				// try to get the unsuccessfully stored instances
				DvtkData.Dimse.Attribute eventFailedSopSequence = dataset.GetAttribute(DvtkData.Dimse.Tag.FAILED_SOP_SEQUENCE);
				if (eventFailedSopSequence != null)
				{
					SequenceOfItems sequenceOfItems = (SequenceOfItems)eventFailedSopSequence.DicomValue;
					foreach (DvtkData.Dimse.SequenceItem item in sequenceOfItems.Sequence)
					{
						UpdateReferencedSopItem(storageCommitItems, item, InstanceStateEnum.InstanceStorageCommitReportedFailure);
					}
				}
			}
		}

		private static void UpdateReferencedSopItem(ReferencedSopItemCollection storageCommitItems, DvtkData.Dimse.SequenceItem item, InstanceStateEnum newInstanceState)
		{
			if (item != null)
			{
				System.String sopClassUid = System.String.Empty;
				System.String sopInstanceUid = System.String.Empty;

				// Try to get the SOP Class UID and SOP Instance UID out of the item
				DvtkData.Dimse.Attribute attribute = item.GetAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID);
				if (attribute != null)
				{
					UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
					if (uniqueIdentifier.Values.Count > 0)
					{
						sopClassUid = uniqueIdentifier.Values[0];
					}
				}

				attribute = item.GetAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID);
				if (attribute != null)
				{
					UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
					if (uniqueIdentifier.Values.Count > 0)
					{
						sopInstanceUid = uniqueIdentifier.Values[0];
					}
				}
				ReferencedSopItem referencedSopItem = storageCommitItems.Find(sopClassUid, sopInstanceUid);
				if ((referencedSopItem != null) &&
					(referencedSopItem.InstanceState == InstanceStateEnum.InstanceStorageCommitRequested))
				{
					referencedSopItem.InstanceState = newInstanceState;
				}
			}
		}

        /// <summary>
        /// Make a N-ACTION storage commitment.
        /// </summary>
        /// <param name="storageCommitItems">The storage commitment items.</param>
        /// <param name="storageCommitmentMessage">The storage commitment message.</param>
        /// <param name="storageCommitTransactionUid">The storage commitment transaction UID.</param>
        /// <param name="mppsInstanceUid">The MPPS instance UID.</param>
		public static void MakeNActionStorageCommitment(ReferencedSopItemCollection storageCommitItems,
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage storageCommitmentMessage,
			System.String storageCommitTransactionUid,
			System.String mppsInstanceUid)
		{
			storageCommitmentMessage.Set("0x00000003", VR.UI, "1.2.840.10008.1.20.1");
			storageCommitmentMessage.Set("0x00001001", VR.UI, "1.2.840.10008.1.20.1.1"); // Well known Instance UID
			storageCommitmentMessage.Set("0x00001008", VR.US, 1);

			// Add the required values to the StorageCommitment message
			AddStorageCommitmentValues(storageCommitItems,
				storageCommitmentMessage.DataSet.DvtkDataDataSet,
				storageCommitTransactionUid,
				mppsInstanceUid);
		}

		private static void AddStorageCommitmentValues(ReferencedSopItemCollection storageCommitItems,
			DvtkData.Dimse.DataSet dataset,
			System.String storageCommitTransactionUid,
			System.String mppsInstanceUid)
		{
			DvtkData.Dimse.SequenceItem referencedStudyComponentSequenceItem = new DvtkData.Dimse.SequenceItem();
			referencedStudyComponentSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.GroupNumber,
				DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.ElementNumber,
				DvtkData.Dimse.VR.UI, "1.2.840.10008.3.1.2.3.3");
			referencedStudyComponentSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.GroupNumber,
				DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.ElementNumber,
				DvtkData.Dimse.VR.UI, mppsInstanceUid);
			dataset.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_STUDY_COMPONENT_SEQUENCE.GroupNumber,
				DvtkData.Dimse.Tag.REFERENCED_STUDY_COMPONENT_SEQUENCE.ElementNumber,
				DvtkData.Dimse.VR.SQ, referencedStudyComponentSequenceItem);

			AddReferencedSopSequence(storageCommitItems, 0x00081199, dataset, InstanceStateEnum.InstanceStorageCommitRequested);

			dataset.AddAttribute(DvtkData.Dimse.Tag.TRANSACTION_UID.GroupNumber,
				DvtkData.Dimse.Tag.TRANSACTION_UID.ElementNumber,
				DvtkData.Dimse.VR.UI, storageCommitTransactionUid);
		}

		private static void AddReferencedSopSequence(ReferencedSopItemCollection storageCommitItems,
			uint tag,
			DvtkData.Dimse.AttributeSet attributeSet,
			InstanceStateEnum newInstanceState)
		{
			ushort group = (ushort)(tag >> 16);
			ushort element = (ushort)(tag & 0x0000FFFF);
			DvtkData.Dimse.Tag tagValue = new DvtkData.Dimse.Tag(group, element);

			DvtkData.Dimse.Attribute referencedSopSequence = attributeSet.GetAttribute(tagValue);
			if (referencedSopSequence != null)
			{
				attributeSet.Remove(referencedSopSequence);
			}

			referencedSopSequence = new DvtkData.Dimse.Attribute(tag, DvtkData.Dimse.VR.SQ);
			SequenceOfItems referencedSopSequenceOfItems = new SequenceOfItems();
			referencedSopSequence.DicomValue = referencedSopSequenceOfItems;

			foreach(ReferencedSopItem  referencedSopItem in storageCommitItems)
			{
				if (((referencedSopItem.InstanceState == InstanceStateEnum.InstanceStored) &&
					(newInstanceState == InstanceStateEnum.InstanceMppsCompleted)) ||
					((referencedSopItem.InstanceState == InstanceStateEnum.InstanceMppsCompleted) &&
					(newInstanceState == InstanceStateEnum.InstanceStorageCommitRequested)))
				{

					DvtkData.Dimse.SequenceItem referencedSopSequenceItem = new DvtkData.Dimse.SequenceItem();
					referencedSopSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.GroupNumber,
						DvtkData.Dimse.Tag.REFERENCED_SOP_CLASS_UID.ElementNumber,
						DvtkData.Dimse.VR.UI, referencedSopItem.SopClassUid);
					referencedSopSequenceItem.AddAttribute(DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.GroupNumber,
						DvtkData.Dimse.Tag.REFERENCED_SOP_INSTANCE_UID.ElementNumber,
						DvtkData.Dimse.VR.UI, referencedSopItem.SopInstanceUid);
					referencedSopItem.InstanceState = newInstanceState;
					referencedSopSequenceOfItems.Sequence.Add(referencedSopSequenceItem);
				}
			}
			attributeSet.Add(referencedSopSequence);
		}

		#endregion StorageCommitTriggers

		#region QueryRetrieveTriggers

        /// <summary>
        /// Make C-FIND query.
        /// </summary>
        /// <param name="informationModel">The information model.</param>
        /// <param name="queryTags">The query tags.</param>
        /// <returns>The create C-FIND message.</returns>
		public static DvtkHighLevelInterface.Dicom.Messages.DicomMessage MakeCFindQuery(QueryRetrieveInformationModelEnum informationModel, TagValueCollection queryTags)
		{
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage queryMessage = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CFINDRQ);
			DvtkData.Dimse.DataSet queryDataset = null;

			switch(informationModel)
			{
				case QueryRetrieveInformationModelEnum.PatientRootQueryRetrieveInformationModel:
				default:
					queryMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.1");
					queryDataset = new DvtkData.Dimse.DataSet("Patient Root Query/Retrieve Information Model - FIND SOP Class");
					break;
				case QueryRetrieveInformationModelEnum.StudyRootQueryRetrieveInformationModel:
					queryMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.2.1");
					queryDataset = new DvtkData.Dimse.DataSet("Study Root Query/Retrieve Information Model - FIND SOP Class");
					break;
				case QueryRetrieveInformationModelEnum.PatientStudyOnlyQueryRetrieveInformationModel:
					queryMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.3.1");
					queryDataset = new DvtkData.Dimse.DataSet("Patient/Study Only Query/Retrieve Info. Model - FIND SOP Class");
					break;
			}

			// add the query keys
			AddQueryRetrieveKeys(queryTags, queryDataset);
			queryMessage.DataSet.DvtkDataDataSet = queryDataset;

			return queryMessage;
		}

		private static void AddQueryRetrieveKeys(TagValueCollection queryTags, DvtkData.Dimse.DataSet dataset)
		{
			// use script session to get to definition singleton
			Dvtk.Sessions.ScriptSession scriptSession = new Dvtk.Sessions.ScriptSession();

			// iterate over the query tags
			foreach(DicomTagValue queryTag in queryTags)
			{
				if (queryTag.ParentSequenceTag != DvtkData.Dimse.Tag.UNDEFINED)
				{
					// try to get the sequence tag in the dataset
					DvtkData.Dimse.Attribute sequenceAttribute = dataset.GetAttribute(queryTag.ParentSequenceTag);
					if ((sequenceAttribute != null) &&
						(sequenceAttribute.ValueRepresentation == DvtkData.Dimse.VR.SQ))
					{
						SequenceOfItems sequenceOfItems = (SequenceOfItems)sequenceAttribute.DicomValue;
						if (sequenceOfItems.Sequence.Count == 1)
						{
							DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];

							if (item != null)
							{
								VR vr = scriptSession.DefinitionManagement.GetAttributeVrFromDefinition(queryTag.Tag);

								// add the query value
								item.AddAttribute(queryTag.Tag.GroupNumber,
									queryTag.Tag.ElementNumber,
									vr,
									queryTag.Value);
							}
						}
					}
				}
				else
				{
					VR vr = scriptSession.DefinitionManagement.GetAttributeVrFromDefinition(queryTag.Tag);

					// add the query value
					dataset.AddAttribute(queryTag.Tag.GroupNumber,
						queryTag.Tag.ElementNumber,
						vr,
						queryTag.Value);
				}
			}
		}

        /// <summary>
        /// Create C-Move retrieve.
        /// </summary>
        /// <param name="informationModel">The information model.</param>
        /// <param name="moveDestination">The move destination.</param>
        /// <param name="retrieveTags">The retrieve tags.</param>
        /// <returns>teh C-MOVE message.</returns>
		public static DvtkHighLevelInterface.Dicom.Messages.DicomMessage MakeCMoveRetrieve(QueryRetrieveInformationModelEnum informationModel, System.String moveDestination, TagValueCollection retrieveTags)
		{
			DvtkHighLevelInterface.Dicom.Messages.DicomMessage retrieveMessage = new DvtkHighLevelInterface.Dicom.Messages.DicomMessage(DvtkData.Dimse.DimseCommand.CMOVERQ);
			DvtkData.Dimse.DataSet retrieveDataset = null;

			switch(informationModel)
			{
				case QueryRetrieveInformationModelEnum.PatientRootQueryRetrieveInformationModel:
				default:
					retrieveMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.1.2");
					retrieveDataset = new DvtkData.Dimse.DataSet("Patient Root Query/Retrieve Information Model - MOVE SOP Class");
					break;
				case QueryRetrieveInformationModelEnum.StudyRootQueryRetrieveInformationModel:
					retrieveMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.2.2");
					retrieveDataset = new DvtkData.Dimse.DataSet("Study Root Query/Retrieve Information Model - MOVE SOP Class");
					break;
				case QueryRetrieveInformationModelEnum.PatientStudyOnlyQueryRetrieveInformationModel:
					retrieveMessage.Set("0x00000002", VR.UI, "1.2.840.10008.5.1.4.1.2.3.2");
					retrieveDataset = new DvtkData.Dimse.DataSet("Patient/Study Only Query/Retrieve Info. Model - MOVE SOP Class");
					break;
			}

			retrieveMessage.Set("0x00000600", VR.AE, moveDestination);

			// add the query keys
			AddQueryRetrieveKeys(retrieveTags, retrieveDataset);
			retrieveMessage.DataSet.DvtkDataDataSet = retrieveDataset;

			return retrieveMessage;
		}

		#endregion QueryRetrieveTriggers

		#region Helpers
		private static void AddDefaultValue(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
			DvtkData.Dimse.Tag tag,
			VR vr,
			DvtkData.Dimse.AttributeSet attributeSet)
		{
			// Only add a default value if the attribute does not already exist
			DvtkData.Dimse.Attribute attribute = attributeSet.GetAttribute(tag);
			if (attribute == null)
			{
				// Attribute does not exist so add a default value
				System.String lValue = defaultValueManager.GetInstantiatedValue(tag);
				attributeSet.AddAttribute(tag.GroupNumber, tag.ElementNumber, (DvtkData.Dimse.VR)vr, lValue);
			}
		}

        private static void AddDefaultValueEvenIfZeroLength(Dvtk.Dicom.InformationEntity.DefaultValues.DefaultValueManager defaultValueManager,
            DvtkData.Dimse.Tag tag,
            VR vr,
            DvtkData.Dimse.AttributeSet attributeSet)
        {
            // Only add a default value if the attribute does not already exist or has a zero length
            DvtkData.Dimse.Attribute attribute = attributeSet.GetAttribute(tag);
            if (attribute == null)
            {
                // Attribute does not exist so add a default value
                System.String lValue = defaultValueManager.GetInstantiatedValue(tag);
                attributeSet.AddAttribute(tag.GroupNumber, tag.ElementNumber, (DvtkData.Dimse.VR)vr, lValue);
            }
            else if (attribute.Length == 0)
            {
                // Remove the existing attribute
                attributeSet.Remove(attribute);

                // Attribute had zero length so add a default value
                System.String lValue = defaultValueManager.GetInstantiatedValue(tag);
                attributeSet.AddAttribute(tag.GroupNumber, tag.ElementNumber, (DvtkData.Dimse.VR)vr, lValue);
            }
        }

		private static void UpdateValue(System.String attributeValue,
			DvtkData.Dimse.Tag tag,
			VR vr,
			DvtkData.Dimse.AttributeSet attributeSet)
		{
			// Only update the value if the dataset contains an attribute with the same tag
			DvtkData.Dimse.Attribute attribute = attributeSet.GetAttribute(tag);
			if (attribute != null)
			{
				// Remove the existing attribute
				attributeSet.Remove(attribute);

				// Add the new (updated) value
				attributeSet.AddAttribute(tag.GroupNumber, tag.ElementNumber, (DvtkData.Dimse.VR)vr, attributeValue);
			}
		}

        /// <summary>
        /// Get the value from the message.
        /// </summary>
        /// <param name="dicomMessage">The DICOM message.</param>
        /// <param name="sequenceTag">The sequence tag.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The value.</returns>
		public static String GetValueFromMessageUsingTag(DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage,
			DvtkData.Dimse.Tag sequenceTag, DvtkData.Dimse.Tag tag)
		{
			String lValue = String.Empty;

			DvtkData.Dimse.DataSet dataset = dicomMessage.DataSet.DvtkDataDataSet;
			if (dataset != null)
			{
				DvtkData.Dimse.Attribute sequence = dataset.GetAttribute(sequenceTag);
				if (sequence != null)
				{
					SequenceOfItems sequenceOfItems = (SequenceOfItems)sequence.DicomValue;
					if (sequenceOfItems.Sequence.Count == 1)
					{
						DvtkData.Dimse.SequenceItem item = sequenceOfItems.Sequence[0];
						DvtkData.Dimse.Attribute attribute = item.GetAttribute(tag);
                        lValue = GetValueFromAttribute(attribute);
					}
				}
			}

			return lValue;
		}

        /// <summary>
        /// Get value from a DICOM message.
        /// </summary>
        /// <param name="dicomMessage">The DICOM message.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The value.</returns>
		public static String GetValueFromMessageUsingTag(DvtkHighLevelInterface.Dicom.Messages.DicomMessage dicomMessage,
			DvtkData.Dimse.Tag tag)
		{
            String lValue = String.Empty;

            // first try to find the Tag in the dataset
			DvtkData.Dimse.DataSet dataset = dicomMessage.DataSet.DvtkDataDataSet;
			if (dataset != null)
			{
				DvtkData.Dimse.Attribute attribute = dataset.GetAttribute(tag);
                lValue = GetValueFromAttribute(attribute);
			}

            // - otherwise try to find the Tag in the command
            if (lValue == String.Empty)
            {
                DvtkData.Dimse.CommandSet command = dicomMessage.CommandSet.DvtkDataCommandSet;
                if (command != null)
                {
                    DvtkData.Dimse.Attribute attribute = command.GetAttribute(tag);
                    lValue = GetValueFromAttribute(attribute);
                }
            }

			return lValue;
		}

        private static String GetValueFromAttribute(DvtkData.Dimse.Attribute attribute)
        {
            String lValue = String.Empty;

            if (attribute != null)
            {
                switch (attribute.ValueRepresentation)
                {
                    case VR.LO:
                        {
                            LongString longString = (LongString)attribute.DicomValue;
                            if (longString.Values.Count > 0)
                            {
                                lValue = longString.Values[0].Trim();
                            }
                        }
                        break;
                    case VR.PN:
                        {
                            PersonName personName = (PersonName)attribute.DicomValue;
                            if (personName.Values.Count > 0)
                            {
                                lValue = personName.Values[0].Trim();
                            }
                        }
                        break;
                    case VR.UI:
                        {
                            UniqueIdentifier uniqueIdentifier = (UniqueIdentifier)attribute.DicomValue;
                            if (uniqueIdentifier.Values.Count > 0)
                            {
                                lValue = uniqueIdentifier.Values[0].Trim();
                            }
                        }
                        break;
                    case VR.US:
                        {
                            UnsignedShort unsignedShort = (UnsignedShort)attribute.DicomValue;
                            if (unsignedShort.Values.Count > 0)
                            {
                                lValue = unsignedShort.Values[0].ToString();
                            }
                        }
                        break;
                    case VR.UL:
                        {
                            UnsignedLong unsignedLong = (UnsignedLong)attribute.DicomValue;
                            if (unsignedLong.Values.Count > 0)
                            {
                                lValue = unsignedLong.Values[0].ToString();
                            }
                        }
                        break;
                    case VR.UC:
                        {
                            UnlimitedCharacters ulimitedCharacters = (UnlimitedCharacters)attribute.DicomValue;
                            if (ulimitedCharacters.Values.Count > 0)
                            {
                                lValue = ulimitedCharacters.Values[0].ToString();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return lValue;
        }

		#endregion Helpers
	}
}
