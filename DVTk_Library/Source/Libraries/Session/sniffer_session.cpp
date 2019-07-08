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

//  Media Test Session class.

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "sniffer_session.h"
#include "Idefinition.h"
#include "Idicom.h"
#include "Ivalidation.h"

#include <time.h>

//>>===========================================================================

SNIFFER_SESSION_CLASS::SNIFFER_SESSION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // constructor activities
    ACTIVITY_LOG_CLASS *activityLogger_ptr = new ACTIVITY_LOG_CLASS();
    logMaskM = LOG_ERROR | LOG_WARNING | LOG_INFO;
    setLogger(activityLogger_ptr);
    setActivityReporter(NULL);
    setSerializer(NULL);

	// set logger
	sniffPdusM.setLogger(loggerM_ptr);

	runtimeSessionTypeM = ST_SNIFFER;
    sessionTypeM = ST_SNIFFER;
    sessionFileVersionM = 0;
    sessionTitleM = "";
    isOpenM = false;
    filenameM = "";
    setSessionId(1);
    manufacturerM = MANUFACTURER;
    modelNameM = MODEL_NAME;
    softwareVersionsM = IMPLEMENTATION_VERSION_NAME;
    applicationEntityNameM = APPLICATION_ENTITY_NAME;
    applicationEntityVersionM = APPLICATION_ENTITY_VERSION;
    testedByM = TESTED_BY;
    dateM = DATE;
    detailedValidationResultsM = true;
    summaryValidationResultsM = true;
	testLogValidationResultsM = false;
    includeType3NotPresentInResultsM = false;

	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;

	sniffPdusM.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);
	sniffPdusM.setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);
    setDefinitionFileRoot(".\\");
    resultsRootM = ".\\";
}

//>>===========================================================================

SNIFFER_SESSION_CLASS::~SNIFFER_SESSION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // destructor activities
    // call the base class cleanup()
    BASE_SESSION_CLASS::cleanup();
    setLogger(NULL);
	sniffPdusM.removeFileStream();	
}

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::serialise(FILE *file_ptr)

//  DESCRIPTION     : Serialise the media session to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // write the file contents
    fprintf(file_ptr, "SESSION\n\n");
    fprintf(file_ptr, "SESSION-TYPE sniffer\n");
    fprintf(file_ptr, "SESSION-FILE-VERSION %d\n", CURRENT_SESSION_FILE_VERSION);

    fprintf(file_ptr, "\n# Product Test Session Properties\n");
    fprintf(file_ptr, "SESSION-TITLE \"%s\"\n", getSessionTitle());
    fprintf(file_ptr, "SESSION-ID %03d\n", getSessionId());
    fprintf(file_ptr, "MANUFACTURER \"%s\"\n", getManufacturer());
    fprintf(file_ptr, "MODEL-NAME \"%s\"\n", getModelName());
    fprintf(file_ptr, "SOFTWARE-VERSIONS \"%s\"\n", getSoftwareVersions());
    fprintf(file_ptr, "APPLICATION-ENTITY-NAME \"%s\"\n", getApplicationEntityName());
    fprintf(file_ptr, "APPLICATION-ENTITY-VERSION \"%s\"\n", getApplicationEntityVersion());
    fprintf(file_ptr, "TESTED-BY \"%s\"\n", getTestedBy());
    fprintf(file_ptr, "DATE \"%s\"\n", getDate());

    fprintf(file_ptr, "\n# Test Session Properties\n");
    fprintf(file_ptr, "LOG-ERROR %s\n", (isLogLevel(LOG_ERROR)) ? "true" : "false");
    fprintf(file_ptr, "LOG-WARNING %s\n", (isLogLevel(LOG_WARNING)) ? "true" : "false");
    fprintf(file_ptr, "LOG-INFO %s\n", (isLogLevel(LOG_INFO)) ? "true" : "false");
    fprintf(file_ptr, "LOG-RELATION %s\n", (isLogLevel(LOG_IMAGE_RELATION)) ? "true" : "false");
    fprintf(file_ptr, "LOG-DEBUG %s\n", (isLogLevel(LOG_DEBUG)) ? "true" : "false");

    fprintf(file_ptr, "PDU-DUMP %s\n", (isLogLevel(LOG_PDU_BYTES)) ? "true" : "false");

	fprintf(file_ptr, "STORAGE-MODE ");
    switch(getStorageMode())
    {
    case SM_AS_MEDIA:
        fprintf(file_ptr, "as-media\n");
        break;
	case SM_AS_MEDIA_ONLY:
		fprintf(file_ptr, "as-media-only\n");
		break;
    case SM_AS_DATASET:
        fprintf(file_ptr, "as-dataset\n");
        break;
    case SM_NO_STORAGE:
    default:
        fprintf(file_ptr, "no-storage\n");
        break;
    }

    fprintf(file_ptr, "DETAILED-VALIDATION-RESULTS %s\n", (getDetailedValidationResults()) ? "true" : "false");
    fprintf(file_ptr, "SUMMARY-VALIDATION-RESULTS %s\n", (getSummaryValidationResults()) ? "true" : "false");
    fprintf(file_ptr, "INCLUDE-TYPE-3-NOTPRESENT-INRESULTS %s\n", (getIncludeType3NotPresentInResults()) ? "true" : "false");
	fprintf(file_ptr, "ENSURE-EVEN-ATTRIBUTE-VALUE-LENGTH %s\n", (getEnsureEvenAttributeValueLength()) ? "true" : "false");

    fprintf(file_ptr, "\n# Definitions\n");
    for (UINT i = 0; i < noDefinitionDirectories(); i++)
    {
        fprintf(file_ptr, "DEFINITION-DIRECTORY \"%s\"\n", getDefinitionDirectory(i).c_str());
    }

    for (UINT i = 0; i < noDefinitionFiles(); i++)
    {
        DEFINITION_FILE_CLASS *definitionFile_ptr = getDefinitionFile(i);
        fprintf(file_ptr, "DEFINITION \"%s\"\n", definitionFile_ptr->getFilename());
    }

	fprintf(file_ptr, "\n# Results\n");
    fprintf(file_ptr, "RESULTS-ROOT \"%s\"\n", getResultsRoot());

	fprintf(file_ptr, "\n# Data Directory\n");
	fprintf(file_ptr, "DATA-DIRECTORY \"%s\"\n", getDataDirectory());
    
    fprintf(file_ptr, "\nENDSESSION\n");

    return true;
}

//>>===========================================================================

void SNIFFER_SESSION_CLASS::readPduFiles(vector<string>* filenames)

//  DESCRIPTION     : Add the list of PDU files to the file stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// Set storage mode
	sniffPdusM.setStorageMode(storageModeM);

	// Clear the file stream if exists
	sniffPdusM.removeFileStream();

	// Read PDU files
	vector<string>::iterator it;
	for (it = filenames->begin(); it < filenames->end(); ++it)
	{
		sniffPdusM.addFileToStream(*it);
	}
}

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::receive(RECEIVE_MESSAGE_UNION_CLASS **rx_msg_union_ptr_ptr)

//  DESCRIPTION     : Receive any kind of message during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The union message type will indicate which message has been received.
//<<===========================================================================
{
	*rx_msg_union_ptr_ptr = NULL;
	
    bool status = false;

	// set the UN VR definition look-up flag
	sniffPdusM.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

    // try to get message from the file stream
    status = sniffPdusM.getMessage(rx_msg_union_ptr_ptr);

	if (*rx_msg_union_ptr_ptr != NULL) 
    {
        switch ((*rx_msg_union_ptr_ptr)->getRxMsgType())
        {
			case RX_MSG_DICOM_COMMAND:
            {
                // save the address of this command - being that last command sent
				// - it may be needed when a command is received for validation purposes
				// Get the command field
				/*DCM_COMMAND_CLASS* command_ptr = (*rx_msg_union_ptr_ptr)->getCommand();
				UINT16	commandField = 0;
				command_ptr->getUSValue(TAG_COMMAND_FIELD, &commandField);

				if (commandField == C_FIND_RQ) 
				{
					lastCommandSentM_ptr = command_ptr->cloneAttributes();
				}
				
				lastDatasetSentM_ptr = NULL;*/
            }
            break;
			case RX_MSG_DICOM_COMMAND_DATASET:
            {
                // save the address of this command/dataset - being that last command/dataset sent
				// - it may be needed when a command/dataset is received for validation purposes
				// - example C-FIND-RSP validation is better if we know what the C-FIND-RQ identifier contained
				/*DCM_COMMAND_CLASS* command_ptr = (*rx_msg_union_ptr_ptr)->getCommand();
				UINT16	commandField = 0;
				command_ptr->getUSValue(TAG_COMMAND_FIELD, &commandField);

				if (commandField == C_FIND_RQ) 
				{
					lastCommandSentM_ptr = command_ptr->cloneAttributes();
					lastDatasetSentM_ptr = (*rx_msg_union_ptr_ptr)->getDataset()->cloneAttributes();
				}*/
            }
            break;
			default:
            {
				//Do nothing
            }
            break;
        }
	}
    
	return status;
}

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::validate(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag, AE_SESSION_CLASS *ae_session_ptr)

//  DESCRIPTION     : Validate DICOM Command and Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The DICOM refCommand/refDataset may be NULL.
//<<===========================================================================
{
	UINT16			cs_group;
	UINT16			cs_element;
	UINT16			ds_group;
	UINT16			ds_element;
	string			ds_attr_name;
	string			cs_attr_name;

	// check for valid pointers
	if ((command_ptr == NULL) ||
		(dataset_ptr == NULL)) return false;

	// serialize the command & dataset (as display)
    BASE_SERIALIZER *serializer_ptr = getSerializer();
    if (serializer_ptr)
    {
        serializer_ptr->SerializeDisplay(command_ptr,dataset_ptr);
    }

	// log action
	if (loggerM_ptr)
	{
		if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()) &&
			(dataset_ptr->getIdentifier()))
		{
			loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s %s %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName(), dataset_ptr->getIdentifier());
		}
		else if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()))
		{
			loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName());
		}
		else if (dataset_ptr->getIodName())
		{
			loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName());
		}
		else if (command_ptr->getIdentifier())
		{
			loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier());
		}
		else
		{
			loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s", mapCommandName(command_ptr->getCommandId()));
		}
	}		

	string	uid_ds;
	string	uid_cs;

	if (dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, uid_ds) == true)
	{
		if (command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, uid_cs) == true)
		{
			if (uid_ds != uid_cs)
			{
				cs_group = ((UINT16) (TAG_AFFECTED_SOP_CLASS_UID >> 16));
				cs_element = ((UINT16) (TAG_AFFECTED_SOP_CLASS_UID & 0x0000FFFF));
				ds_group = ((UINT16) (TAG_SOP_CLASS_UID >> 16));
				ds_element = ((UINT16) (TAG_SOP_CLASS_UID & 0x0000FFFF));

				ds_attr_name = DEFINITION->GetAttributeName (ds_group, ds_element);
				cs_attr_name = DEFINITION->GetAttributeName (cs_group, cs_element);
				loggerM_ptr->text(LOG_ERROR, 1,
								  "\"%s\" (%s) is not equal to \"%s\" (%s)",
								  ds_attr_name.c_str(),
								  uid_ds.c_str(),
								  cs_attr_name.c_str(),
								  uid_cs.c_str());
			}
		}
	}

	if (dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, uid_ds) == true)
	{
		if (command_ptr->getUIValue (TAG_AFFECTED_SOP_INSTANCE_UID, uid_cs) == true)
		{
			if (uid_ds != uid_cs)
			{
				cs_group = ((UINT16) (TAG_AFFECTED_SOP_INSTANCE_UID >> 16));
				cs_element = ((UINT16) (TAG_AFFECTED_SOP_INSTANCE_UID & 0x0000FFFF));
				ds_group = ((UINT16) (TAG_SOP_INSTANCE_UID >> 16));
				ds_element = ((UINT16) (TAG_SOP_INSTANCE_UID & 0x0000FFFF));

				ds_attr_name = DEFINITION->GetAttributeName (ds_group, ds_element);
				cs_attr_name = DEFINITION->GetAttributeName (cs_group, cs_element);
				loggerM_ptr->text(LOG_ERROR, 1,
								  "\"%s\" (%s) is not equal to \"%s\" (%s)",
								  ds_attr_name.c_str(),
								  uid_ds.c_str(),
								  cs_attr_name.c_str(),
								  uid_cs.c_str());;
			}
		}
	}

	// validate the command - refCommand_ptr maybe NULL
	bool strictValidation = getStrictValidation();
	
   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
	VALIDATION->setIncludeType3NotPresentInResults(getIncludeType3NotPresentInResults());
	bool result1 = VALIDATION->validate(command_ptr, NULL, NULL, validationFlag, serializerM_ptr);

	// validate the dataset - refDataset_ptr maybe NULL
	bool result2 = VALIDATION->validate(command_ptr, dataset_ptr, NULL, NULL, NULL, validationFlag, serializerM_ptr, ae_session_ptr);

	// If any of the previous validations went wrong, the total validation has failed.
	bool result = true;
	if ((!result1) ||
		(!result2))
	{
		result = false;
	}

	// return result
	return result;
}

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::validate(ASSOCIATE_AC_CLASS *associateAc_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Associate Accept - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s", WIDName(associateAc_ptr->getWidType()));
	}

    // Validate the Associate Accept - reference object is optional
    ACSE_PROPERTIES_CLASS	acseProperties;

    // This validation is done on the Associate Accept that has been received.
    // We can therefore assume that DVT is the Requester and the SUT the Accepter.
    acseProperties.setCallingAeTitle(this->dvtAeTitleM);
    acseProperties.setCalledAeTitle(this->sutAeTitleM);

    acseProperties.setMaximumLengthReceived(this->sutMaximumLengthReceivedM);
    acseProperties.setImplementationClassUid(this->sutImplementationClassUidM);
    acseProperties.setImplementationVersionName(this->sutImplementationVersionNameM);

	// now validate the received associate accept
	bool result = VALIDATION->validate(associateAc_ptr, NULL, validationFlag, serializerM_ptr, &acseProperties);

    return result;
};

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::validate(ASSOCIATE_RJ_CLASS *associateRj_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Associate Reject - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s", WIDName(associateRj_ptr->getWidType()));
	}

    // Validate the Associate Reject - reference object is optional
	bool result = VALIDATION->validate(associateRj_ptr, NULL, validationFlag, serializerM_ptr);

    return result;
};

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::validate(ASSOCIATE_RQ_CLASS *associateRq_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Associate Request - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s", WIDName(associateRq_ptr->getWidType()));
	}

    // Validate the Associate Request - reference object is optional
	ACSE_PROPERTIES_CLASS	acseProperties;

    // This validation is done on the Associate Request that has been received.
    // We can therefore assume that DVT is the Accepter and the SUT the Requester.
    acseProperties.setCallingAeTitle(this->sutAeTitleM);
    acseProperties.setCalledAeTitle(this->dvtAeTitleM);

	acseProperties.setMaximumLengthReceived(this->sutMaximumLengthReceivedM);
	acseProperties.setImplementationClassUid(this->sutImplementationClassUidM);
	acseProperties.setImplementationVersionName(this->sutImplementationVersionNameM);

	// now validate the received associate request
	bool result = VALIDATION->validate(associateRq_ptr, NULL, validationFlag, serializerM_ptr, &acseProperties);

    return result;
};

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::validate(RELEASE_RP_CLASS *releaseRp_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Release Response - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s", WIDName(releaseRp_ptr->getWidType()));
	}

    // Validate the Release Response - reference object is optional
	bool result = VALIDATION->validate(releaseRp_ptr, NULL, validationFlag, serializerM_ptr);

    return result;
};

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::validate(RELEASE_RQ_CLASS *releaseRq_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Release Request - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s", WIDName(releaseRq_ptr->getWidType()));
	}

    // Validate the Release Request - reference object is optional
	bool result = VALIDATION->validate(releaseRq_ptr, NULL, validationFlag, serializerM_ptr);

    return result;
};

//>>===========================================================================

bool SNIFFER_SESSION_CLASS::validate(ABORT_RQ_CLASS *abortRq_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Abort Request - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 2, "VALIDATE %s", WIDName(abortRq_ptr->getWidType()));
	}

    // Validate the Abort Request - reference object is optional
   	bool result = VALIDATION->validate(abortRq_ptr, NULL, validationFlag, serializerM_ptr);

    return result;
};