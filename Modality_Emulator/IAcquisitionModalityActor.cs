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

namespace ModalityEmulator
{
    /// <summary>
    /// Summary description for IAcquisitionModalityActor.
    /// </summary>
    interface IAcquisitionModalityActor
    {
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
        DicomQueryItem CreateUnscheduledWorklistItem(TagValueCollection userDefinedTags);

        /// <summary>
        /// Send a CFind Modality Worklist Query trigger from the given queryTags.
        /// All other return keys are taken from the supported return key attributes
        /// in the Worklist Information Model.
        /// </summary>
        /// <param name="queryTags">List of Query Tags.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendQueryModalityWorklist(TagValueCollection queryTags);

        /// <summary>
        /// Send a CFind Modality Worklist Query trigger by reading the query
        /// dataset from the given mwlQueryDcmFilename. If the scheduledProcedureStepStartDate
        /// is defined (not string empty) then if a value for this attribute is present in the
        /// read datset it will be overwritten by the scheduledProcedureStepStartDate value.
        /// </summary>
        /// <param name="mwlQueryDcmFilename">DCM file containing the MWL Query Dataset.</param>
        /// <param name="scheduledProcedureStepStartDate">Optional (not sting empty) start date to overwrite dataset value.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendQueryModalityWorklist(System.String mwlQueryDcmFilename, System.String scheduledProcedureStepStartDate);

        #endregion SendQueryModalityWorklist

        #region SendModalityProcedureStepInProgress() overloads

        /// <summary>
        /// Send an N-CREATE MPPS IN-PROGRESS message by using the Default Value Manager.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// </summary>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNCreateModalityProcedureStepInProgress(DicomQueryItem modalityWorklistItem);

        /// <summary>
        /// Send an N-CREATE MPPS IN-PROGRESS message from the given mppsInProgressDcmFilename.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// </summary>
        /// <param name="mppsInProgressDcmFilename">DCM Filename - contains MPPS IN-PROGRESS dataset.</param>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNCreateModalityProcedureStepInProgress(System.String mppsInProgressDcmFilename, DicomQueryItem modalityWorklistItem);

        /// <summary>
        /// Send an N-SET MPPS IN-PROGRESS message by using the Default Value Manager.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// NOTE: This call should only be made after the SendModalityProcedureStepInProgress() (using default N-CREATE) 
        /// has been made.
        /// </summary>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNSetModalityProcedureStepInProgress(DicomQueryItem modalityWorklistItem);

        /// <summary>
        /// Send an N-SET MPPS IN-PROGRESS message from the given mppsInProgressDcmFilename.
        /// Use the modalityWorklistItem to overwrite the appropriate attribute values.
        /// NOTE: This call should only be made after the SendModalityProcedureStepInProgress() (using default N-CREATE) 
        /// has been made.
        /// </summary>
        /// <param name="mppsInProgressDcmFilename">DCM Filename - contains MPPS IN-PROGRESS dataset.</param>
        /// <param name="modalityWorklistItem">Modality Worklist Item used to update MPPS IN-PROGRESS dataset.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNSetModalityProcedureStepInProgress(System.String mppsInProgressDcmFilename, DicomQueryItem modalityWorklistItem);

        #endregion SendModalityProcedureStepInProgress

        #region SendModalityProcedureStepCompletedDiscontinued() overloads

        /// <summary>
        /// Send an N-SET MPPS COMPLETED message. Take the default values from the Default Value Manager.
        /// </summary>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNSetModalityProcedureStepCompleted();

        /// <summary>
        /// Send an N-SET MPPS COMPLETED message. Take the default values from the given DCM file contents.
        /// </summary>
        /// <param name="mppsCompletedDcmFilename">DCM file for default MPPS Completed atribute values.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNSetModalityProcedureStepCompleted(System.String mppsCompletedDcmFilename);

        /// <summary>
        /// Send an N-SET MPPS DISCONTINUED message. Take the default values from the Default Value Manager.
        /// </summary>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNSetModalityProcedureStepDiscontinued();

        /// <summary>
        /// Send an N-SET MPPS DISCONTINUED message. Take the default values from the given DCM file contents.
        /// </summary>
        /// <param name="mppsDiscontinuedDcmFilename">DCM file for default MPPS Discontinued atribute values.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendNSetModalityProcedureStepDiscontinued(System.String mppsDiscontinuedDcmFilename);

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
        bool SendModalityImagesStored(bool startNewSeries, DicomQueryItem modalityWorklistItem);

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
        bool SendModalityImagesStored(DicomQueryItem modalityWorklistItem, bool withSingleAssociation);

        /// <summary>
        /// Send all the images found in the given storage directory.
        /// </summary>
        /// <param name="storageDirectory">Given storage directory - containing storage DCM files.</param>
        /// <param name="modalityWorklistItem">Worklist Item used to provide overruling values for
        /// the Image headers.</param>
        /// <param name="withSingleAssociation">Boolean indicating whether the images should be sent in a single
        /// association or not.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        bool SendModalityImagesStored(System.String storageDirectory, DicomQueryItem modalityWorklistItem, bool withSingleAssociation);
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
        bool SendStorageCommitment(bool awaitNEventReport, int timeOut);

        #endregion SendStorageCommitment
    }
}
