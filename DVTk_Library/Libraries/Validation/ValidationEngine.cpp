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

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ValidationEngine.h"
#include "ACSE_abort_request_validator.h"
#include "ACSE_associate_accept_validator.h"
#include "ACSE_associate_reject_validator.h"
#include "ACSE_associate_request_validator.h"
#include "ACSE_release_request_validator.h"
#include "ACSE_release_response_validator.h"
#include "record_uid.h"
#include "validator.h"
#include "val_attribute.h"
#include "val_attribute_group.h"
#include "val_object_results.h"
#include "val_base_value.h"
#include "val_value_sq.h"
#include "media_validator.h"
#include "qr_validator.h"

#include "Idefinition.h"    // Definition component interface
#include "Idicom.h"         // Dicom component interface
#include "Iglobal.h"        // Global component interface
#include "Imedia.h"         // Media component interface

//*****************************************************************************
//  INTERNAL DECLARATIONS
//*****************************************************************************

// initialise static pointer
VALIDATION_ENGINE_CLASS* VALIDATION_ENGINE_CLASS::instanceM_ptr = NULL;


//>>===========================================================================

VALIDATION_ENGINE_CLASS::VALIDATION_ENGINE_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	optionsM = VAL_OPTIONS_INITIAL;
    loggerM_ptr = NULL;
	includeType3NotPresentInResultsM = false;
}

//>>===========================================================================

VALIDATION_ENGINE_CLASS *VALIDATION_ENGINE_CLASS::instance()

//  DESCRIPTION     : Get Validation instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) 
	{
		instanceM_ptr = new VALIDATION_ENGINE_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

void VALIDATION_ENGINE_CLASS::setOptions(UINT options)

//  DESCRIPTION     : Sets the validation options set in the user interface.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    optionsM = options;
}

//>>===========================================================================

void VALIDATION_ENGINE_CLASS::setStrictValidation(bool flag)	

//  DESCRIPTION     : Enables/Disables strict validation.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (flag)
	{
		// enable
		optionsM |= VAL_OPTIONS_STRICT;
	}
	else
	{
		// disable
		UINT mask = ~VAL_OPTIONS_STRICT;
		optionsM &= mask;
	}
}

//>>===========================================================================

void VALIDATION_ENGINE_CLASS::setIncludeType3NotPresentInResults(bool flag)

//  DESCRIPTION     : Set the IncludeType3NotPresentInResults flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	includeType3NotPresentInResultsM = flag;
}

//>>===========================================================================

void VALIDATION_ENGINE_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    loggerM_ptr = logger_ptr;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(ABORT_RQ_CLASS *abortRq_ptr,
								  ABORT_RQ_CLASS *refAbortRq_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr)  

//  DESCRIPTION     : Validates an abort request object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ABORT_RQ_VALIDATOR_CLASS abortRqValidator;
	
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

	// validate the abort request
	bool result = abortRqValidator.validate(abortRq_ptr, refAbortRq_ptr);

    // check on strict validation flag
	if (!(optionsM & VAL_OPTIONS_STRICT))
	{
		// always return success
		result = true;
	}

    // serialize the results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(&abortRqValidator);
    }

	// return result
	return result;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(ASSOCIATE_RQ_CLASS *assocRq_ptr,
								  ASSOCIATE_RQ_CLASS *refAssocRq_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr,
								  ACSE_PROPERTIES_CLASS *acseProp_ptr)

//  DESCRIPTION     : Validates an associate request object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    ASSOCIATE_RQ_VALIDATOR_CLASS associateRqValidator;
	
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    // validate the associate request
	bool result = associateRqValidator.validate(assocRq_ptr, refAssocRq_ptr, acseProp_ptr);

	// check on strict validation flag
	if (!(optionsM & VAL_OPTIONS_STRICT))
	{
		// always return success
		result = true;
	}

    // serialize the results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(&associateRqValidator);
    }

    // return result
	return result;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(ASSOCIATE_AC_CLASS *assocAc_ptr,
								  ASSOCIATE_AC_CLASS *refAssocAc_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr,
								  ACSE_PROPERTIES_CLASS *acseProp_ptr)

//  DESCRIPTION     : Validates an associate accept object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    ASSOCIATE_AC_VALIDATOR_CLASS associateAcValidator;
	
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    // validate the associate accept
	bool result = associateAcValidator.validate(assocAc_ptr, refAssocAc_ptr, acseProp_ptr);

	// check on strict validation flag
	if (!(optionsM & VAL_OPTIONS_STRICT))
	{
		// always return success
		result = true;
	}

    // serialize the results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(&associateAcValidator);
    }

	// return result
	return result;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(ASSOCIATE_RJ_CLASS *assocRj_ptr,
								  ASSOCIATE_RJ_CLASS *refAssocRj_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Validates an associate reject object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    ASSOCIATE_RJ_VALIDATOR_CLASS associateRjValidator;
	
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    // validate the associate reject
	bool result = associateRjValidator.validate(assocRj_ptr, refAssocRj_ptr);

    // check on strict validation flag
	if (!(optionsM & VAL_OPTIONS_STRICT))
	{
		// always return success
		result = true;
	}

    // serialize the results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(&associateRjValidator);
    }

    // return result
	return result;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(RELEASE_RQ_CLASS *releaseRq_ptr,
								  RELEASE_RQ_CLASS *refReleaseRq_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Validates an release request object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    RELEASE_RQ_VALIDATOR_CLASS releaseRqValidator;
	
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    // validate the release request
	bool result = releaseRqValidator.validate(releaseRq_ptr, refReleaseRq_ptr);

    // check on strict validation flag
	if (!(optionsM & VAL_OPTIONS_STRICT))
	{
		// always return success
		result = true;
	}

    // serialize the results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(&releaseRqValidator);
    }

    // return result
	return result;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(RELEASE_RP_CLASS *releaseRp_ptr,
								  RELEASE_RP_CLASS *refReleaseRp_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Validates a release response object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    RELEASE_RP_VALIDATOR_CLASS releaseRpValidator;
	
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    // validate the release response
	bool result = releaseRpValidator.validate(releaseRp_ptr, refReleaseRp_ptr);

	// check on strict validation flag
	if (!(optionsM & VAL_OPTIONS_STRICT))
	{
		// always return success
		result = true;
	}

    // serialize the results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(&releaseRpValidator);
    }

    // return result
	return result;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(UNKNOWN_PDU_CLASS*,
								  UNKNOWN_PDU_CLASS*,
                                  VALIDATION_CONTROL_FLAG_ENUM,
                                  BASE_SERIALIZER*)

//  DESCRIPTION     : Validates an unknown PDU object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// no validator for unknown PDUs
	return false;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(DCM_COMMAND_CLASS *command_ptr,
									DCM_COMMAND_CLASS *refCommand_ptr,
									DCM_COMMAND_CLASS *, //lastSentCommand_ptr,
									VALIDATION_CONTROL_FLAG_ENUM validationFlag,
									BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Validates a command object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{

    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

	// check for valid input
    if (command_ptr == NULL) return false;

    // select a command validator
    VALIDATOR_CLASS *validator_ptr = SelectValidator(command_ptr, NULL);
    if (validator_ptr == NULL) return false;

    validator_ptr->SetLogger(loggerM_ptr);

    bool status = validator_ptr->CreateCommandResultsFromDef(command_ptr);
    if (status)
    {
        validator_ptr->SetModuleResultsFromDcm(command_ptr, true, true);
        if (refCommand_ptr != NULL)
		{
            validator_ptr->SetModuleResultsFromDcm(refCommand_ptr, false, true);
		}

        // We don't need to add specific information to the results structure
        // if we're dealing with a C-FIND-RSP. We're dealing with a command
        // only, so there's nothing specific for QR in that case.

        // Validate the object.
        validator_ptr->ValidateResults(validationFlag);
    }

    // serialise the result
    validator_ptr->Serialize(serializer_ptr);

    delete validator_ptr;

    return status;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(DCM_COMMAND_CLASS *command_ptr,
									DCM_DATASET_CLASS *dataset_ptr,
									DCM_DATASET_CLASS *refDataset_ptr,
									DCM_COMMAND_CLASS *lastSentCommand_ptr,
									DCM_DATASET_CLASS *lastSentDataset_ptr,
									VALIDATION_CONTROL_FLAG_ENUM validationFlag,
									BASE_SERIALIZER *serializer_ptr,
									AE_SESSION_CLASS *aeSession_ptr)

//  DESCRIPTION     : Validates a dataset object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    // check for valid input
    if (dataset_ptr == NULL) return false;

    // select a validator
    VALIDATOR_CLASS *validator_ptr = SelectValidator(command_ptr, dataset_ptr);
    if (validator_ptr == NULL) return false;

    validator_ptr->SetLogger(loggerM_ptr);

    if (validator_ptr->CreateResultsObject() == false) return false;

    bool status = validator_ptr->CreateDatasetResultsFromDef(command_ptr, dataset_ptr, aeSession_ptr);
	if (status)
	{
		validator_ptr->SetModuleResultsFromDcm(dataset_ptr, true, false);

		if (refDataset_ptr != NULL)
		{
		    validator_ptr->SetModuleResultsFromDcm(refDataset_ptr, false, false);
		}
		
		validator_ptr->UpdateDatasetResultsFromLastSent(command_ptr, lastSentCommand_ptr, lastSentDataset_ptr);

		validator_ptr->ValidateResults(validationFlag);
	}

    // serialise the result
    validator_ptr->Serialize(serializer_ptr);

    delete validator_ptr;

    return status;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(FILE_DATASET_CLASS *fileDataset_ptr,
                                  DCM_DATASET_CLASS *refDataset_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr,
                                  AE_SESSION_CLASS *aeSession_ptr)

//  DESCRIPTION     : Validates a media dataset object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    MEDIA_VALIDATOR_CLASS *validator_ptr = new MEDIA_VALIDATOR_CLASS();
    if (validator_ptr == NULL) return false;

    validator_ptr->SetLogger(loggerM_ptr);

    RECORD_UID_CLASS *uidLinks_ptr = new RECORD_UID_CLASS();
    if (uidLinks_ptr == NULL)
    {
        delete validator_ptr;
        return false;
    }

    bool status = validator_ptr->Validate(fileDataset_ptr, 
                        refDataset_ptr, 
                        uidLinks_ptr, 
                        validationFlag, 
                        serializer_ptr, 
                        aeSession_ptr);

    delete validator_ptr;
    delete uidLinks_ptr;

    return status;
}

//>>===========================================================================

bool VALIDATION_ENGINE_CLASS::validate(DCM_DATASET_CLASS *dataset_ptr,
                                  DCM_DATASET_CLASS *refDataset_ptr,
                                  VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                  BASE_SERIALIZER *serializer_ptr,
                                  AE_SESSION_CLASS*)

//  DESCRIPTION     : Validates a dataset object without a command set.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{

    // check if validation disabled
    // - if disabled return successful validation
    if (validationFlag == NONE) return true;

    // check for valid input
    if (dataset_ptr == NULL) return false;

    VALIDATOR_CLASS *validator_ptr = new VALIDATOR_CLASS();
    if (validator_ptr == NULL) return false;

    validator_ptr->SetLogger(loggerM_ptr);

    validator_ptr->SetModuleResultsFromDcm(dataset_ptr, true, false);
    if (refDataset_ptr != NULL)
	{
        validator_ptr->SetModuleResultsFromDcm(refDataset_ptr, false, false);
	}

    validator_ptr->ValidateResults(validationFlag);

    // serialise the result
    validator_ptr->Serialize(serializer_ptr);

    delete validator_ptr;

    return true;
}

//>>===========================================================================

VALIDATOR_CLASS *VALIDATION_ENGINE_CLASS::SelectValidator(DCM_COMMAND_CLASS *command_ptr,
                                          DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Instantiates the correct Validator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    VALIDATOR_CLASS *validator_ptr = NULL;

    // check for valid input
    if (command_ptr == NULL) return NULL;

    // check if dataset available
    if (dataset_ptr == NULL)
    {
       validator_ptr = new VALIDATOR_CLASS();
    }
	else
    {
        string sopClassUid = "";

        // Try to get the SOP class UID. First through the affected
        // SOP class UID. If that's not successful, use the requested SOP
        // class UID.
        bool status = command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUid);
        if (status == false)
		{
            status = command_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUid);
		}

        // Use the SOP class UID to determine which validator to use.
        if (sopClassUid == MEDIA_STORAGE_DIRECTORY_SOP_CLASS_UID)
        {
            // Use the media validator class.
            validator_ptr = new MEDIA_VALIDATOR_CLASS();
        }
		else if ((command_ptr->getCommandId() == DIMSE_CMD_CFIND_RQ) ||
				 (command_ptr->getCommandId() == DIMSE_CMD_CMOVE_RQ) ||
				 (command_ptr->getCommandId() == DIMSE_CMD_CGET_RQ))
        {
            // Use the query/retrieve validator class.
            validator_ptr = new QR_VALIDATOR_CLASS();
		}
        else
		{
            // Use the default validator class.
            validator_ptr = new VALIDATOR_CLASS();
        }
    }

    UINT32 flags = ATTR_FLAG_NONE;
    if (optionsM & VAL_OPTIONS_STRICT)
    {
        flags |= ATTR_FLAG_STRICT_VALIDATION;
    }
	if (includeType3NotPresentInResultsM)
	{
		validator_ptr->SetFlags(flags);
	}
	else
	{
		validator_ptr->SetFlags(flags | ATTR_FLAG_DO_NOT_INCLUDE_TYPE3);
	}

    // return the validator.
    return validator_ptr;
}
