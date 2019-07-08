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
//  DESCRIPTION     :	Storage SCP emulator class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "storage_emulator.h"
#include "storage_scu_emulator.h"
#include "commitment.h"
#include "Idefinition.h"			// Definition component interface
#include "Imedia.h"					// Media component interface
#include "Isession.h"				// Session component interface
#include "Ivalidation.h"			// Validation component interface


//>>===========================================================================

STORAGE_SCP_EMULATOR_CLASS::STORAGE_SCP_EMULATOR_CLASS(EMULATOR_SESSION_CLASS *session_ptr, BASE_SOCKET_CLASS* socket_ptr, bool logEmulation)

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	//
	// constructor activities
	// - setup the logging, etc
	//
	setup(session_ptr, socket_ptr, logEmulation);

	//Only Store C-Store-Req objects
	associationM.setOnlyStoreCSTOREObjects(session_ptr->getStoreCStoreReqOnly());

	storageCommitmentM_ptr = NULL;

	isAsync = false;
}

//>>===========================================================================

STORAGE_SCP_EMULATOR_CLASS::~STORAGE_SCP_EMULATOR_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	//
	// destructor activities
	// - cleanup the logging, etc
	teardown();
}

//>>===========================================================================

bool STORAGE_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(EMULATOR_SESSION_CLASS *session_ptr)

//  DESCRIPTION     : Add the supported Storage presentation contexts.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	AE_SESSION_CLASS ae_session;
	
	// set ae session
	ae_session.SetName(session_ptr->getApplicationEntityName());
	ae_session.SetVersion(session_ptr->getApplicationEntityVersion());

	// Remove previously supported SOP classes
	session_ptr->removeSupportedSopClasses();

	// use loaded definition files
	for (UINT i = 0; i < session_ptr->noDefinitionFiles(); i++) 
	{
		DEFINITION_FILE_CLASS *definitionFile_ptr = session_ptr->getDefinitionFile(i);

		// use the sop class uid
		DEF_DETAILS_CLASS file_details;
		if (definitionFile_ptr->GetDetails(file_details))
		{
			string sopClassUid = file_details.GetSOPClassUID();

			// check if this is a storage sop class
			// - or the Storage Commitment Push Model SOP Class
			if ((DEFINITION->IsStorageSop(sopClassUid, &ae_session)) ||
				(sopClassUid == STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID))
			{
				// add this sop class to the supported sop classes
				session_ptr->addSupportedSopClass((char*) sopClassUid.c_str());
			}
		}
	}

	// return result
	return true;
}

//>>===========================================================================

bool STORAGE_SCP_EMULATOR_CLASS::processCommandDataset(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Process the Storage command and dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result;

	// handle individual commands
	switch(command_ptr->getCommandId())
	{
	case DIMSE_CMD_CSTORE_RQ:
		if (dataset_ptr)
		{
        	UINT16	cs_group;
	        UINT16	cs_element;
	        UINT16	ds_group;
	        UINT16	ds_element;
	        string	ds_attr_name;
	        string	cs_attr_name;
        	string	uid_ds;
        	string	uid_cs;

            // check if the command and dataset SOP Class UIDs are the same
        	if (dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, uid_ds) == true)
        	{
        		if (command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, uid_cs) == true)
        		{
        			if ((loggerM_ptr) && 
						(uid_ds != uid_cs))
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

            // check if the command and dataset SOP Instance UIDs are the same
        	if (dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, uid_ds) == true)
        	{
        		if (command_ptr->getUIValue (TAG_AFFECTED_SOP_INSTANCE_UID, uid_cs) == true)
        		{
        			if ((loggerM_ptr) &&
						(uid_ds != uid_cs))
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
        								  uid_cs.c_str());
        			}
        		}
        	}

			// process the STORE command
			result = processStore(command_ptr->getEncodePresentationContextId(),
				dataset_ptr);
		}
		else
		{
			// missing dataset
			UINT16 status = DCM_STATUS_PROCESSING_FAILURE;
			result = sendResponse(DIMSE_CMD_CSTORE_RSP,
				command_ptr->getEncodePresentationContextId(), 
				status);
		}
		break;

	case DIMSE_CMD_NACTION_RQ:
		if (dataset_ptr)
		{
			// process the ACTION command
			result = processAction(command_ptr->getEncodePresentationContextId(),
				dataset_ptr);			
		}
		else
		{
			// missing dataset
			UINT16 status = DCM_STATUS_PROCESSING_FAILURE;
			result = sendResponse(DIMSE_CMD_NACTION_RSP,
				command_ptr->getEncodePresentationContextId(), 
				status);
		}
		break;

	default:
		{
			// If command type is N-EVENT-REPORT-RSP, it's synchronous Storage
			// commitment so only wait for release request
			if ((command_ptr->getCommandId()) == DIMSE_CMD_NEVENTREPORT_RSP)
			{
				return true;
			}
			else
			{
				// unknown command
				UINT16 status = DCM_STATUS_UNRECOGNIZED_OPERATION;
				result = sendResponse(command_ptr->getCommandId(),
					command_ptr->getEncodePresentationContextId(), 
					status);
			}
		}
		break;
	}

	// return result
	return result;
}

//>>===========================================================================

bool STORAGE_SCP_EMULATOR_CLASS::processStore(BYTE presentationContextId,
											  DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Process the C-STORE-RQ with dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 status = DCM_STATUS_PROCESSING_FAILURE;

	AE_SESSION_CLASS ae_session;
	string analyzeMsg;

	// set ae session
	ae_session.SetName(sessionM_ptr->getApplicationEntityName());
	ae_session.SetVersion(sessionM_ptr->getApplicationEntityVersion());

	// ensure a storage object
	if (DEFINITION->IsStorageSop(sopClassUidM, &ae_session))
	{
		// analyse the dataset
		bool isAcceptDuplicateImage = sessionM_ptr->getAcceptDuplicateImage();
		status = RELATIONSHIP->analyseStorageDataset(dataset_ptr, analyzeMsg, loggerM_ptr, isAcceptDuplicateImage);		
	}

	// return the C-STORE-RSP
	DCM_COMMAND_CLASS command(DIMSE_CMD_CSTORE_RSP);

	// cascade the logger
	command.setLogger(loggerM_ptr);

	// set the presentation context id
	command.setEncodePresentationContextId(presentationContextId);

	// set the status value
	command.setUSValue(TAG_STATUS, status);

	// set the Error comment
	if ((status != DCM_STATUS_SUCCESS) &&
		(loggerM_ptr))
	{
		loggerM_ptr->text(LOG_WARNING, 1, "Returning non-zero status of 0x%04X in DIMSE Response", status);
		command.setLOValue(TAG_ERROR_COMMENT, analyzeMsg);
	}

	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", mapCommandName(command.getCommandId()), timeStamp());
	}

	// return response command only
	bool result = associationM.send(&command);

	return result;
}

//>>===========================================================================

bool STORAGE_SCP_EMULATOR_CLASS::processAction(BYTE presentationContextId,
											   DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Process the N-ACTION-RQ with dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 status = DCM_STATUS_PROCESSING_FAILURE;

	// ensure that we have the Storage Commitment SOP Class
	if (sopClassUidM == STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID)
	{
		// cleanup any old storage commitment
		if (storageCommitmentM_ptr)
		{
			delete storageCommitmentM_ptr;
		}

		// instantiate the storage commitment
		storageCommitmentM_ptr = new STORAGE_COMMITMENT_CLASS();

		// process the N-ACTION-RQ command
		status = storageCommitmentM_ptr->action(dataset_ptr, loggerM_ptr);
	}

	// return the N-ACTION-RSP
	return sendResponse(DIMSE_CMD_NACTION_RSP, presentationContextId, status);
}

//>>===========================================================================

bool STORAGE_SCP_EMULATOR_CLASS::postProcess()

//  DESCRIPTION     : Perform any association release post processing.
//					: For storage do nothing.
//					: For Async storage commitment send an event report.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// for storage commitment - send an event report
	if ((storageCommitmentM_ptr) &&
		(storageCommitmentM_ptr->isEventToSend()))
	{
		DCM_DATASET_CLASS *dataset_ptr = NULL;

		// build the event report message
		storageCommitmentM_ptr->eventReport(&dataset_ptr, loggerM_ptr);

		// instantiate a SCU emulator to send the event
		STORAGE_SCU_EMULATOR_CLASS	storageScu(sessionM_ptr);
		storageScu.setLogger(loggerM_ptr);
		storageScu.setSerializer(serializerM_ptr);

		// send the event report
		UINT16 eventTypeId = (storageCommitmentM_ptr->noFailedInstances() > 0) ? 2 : 1;
		result = storageScu.eventReportStorageCommitment(eventTypeId, dataset_ptr);
		
		// indicate that the event has now been sent
		storageCommitmentM_ptr->setEventToSend(false);
	}

	// return result
	return result;
}

//>>===========================================================================

bool STORAGE_SCP_EMULATOR_CLASS::sendEventReport()

//  DESCRIPTION     : For Sync storage commitment send an event report.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	//Send the N-EVENT-REPORT-RQ
	DCM_DATASET_CLASS *dataset_ptr = NULL;

	// build the event report message
	storageCommitmentM_ptr->eventReport(&dataset_ptr, loggerM_ptr);

	// send the event report
	UINT16 eventTypeId = (storageCommitmentM_ptr->noFailedInstances() > 0) ? 2 : 1;

	// build event report request object
	DCM_COMMAND_CLASS cmd(DIMSE_CMD_NEVENTREPORT_RQ);
	cmd.setLogger(loggerM_ptr);

	// group length is calulated on export
	cmd.setUSValue(TAG_COMMAND_FIELD, N_EVENT_REPORT_RQ);
	cmd.setUIValue(TAG_AFFECTED_SOP_CLASS_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID);
	cmd.setUSValue(TAG_MESSAGE_ID, 0x0001);
	cmd.setUSValue(TAG_DATA_SET_TYPE, DATA_SET);
	cmd.setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, STORAGE_COMMITMENT_PUSH_MODEL_SOP_INSTANCE_UID);
	cmd.setUSValue(TAG_EVENT_TYPE_ID, eventTypeId);

	// log action
	if (loggerM_ptr)
	{
    	loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s %s (%s)", mapCommandName(cmd.getCommandId()), dataset_ptr->getIodName(), timeStamp());
	}

	// send request
	result = associationM.send(&cmd, dataset_ptr);
	
	// indicate that the event has now been sent
	storageCommitmentM_ptr->setEventToSend(false);

	// cleanup any outstanding relationships
	RELATIONSHIP->cleanup();

	// return result
	return result;
}

//>>===========================================================================

void STORAGE_SCP_EMULATOR_CLASS::completeLogging()

//  DESCRIPTION     : Complete the emulation logging by displaying any relationships.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// log the object relationship analysis
	if (sessionM_ptr->isLogLevel(LOG_IMAGE_RELATION))
	{
		RELATIONSHIP->logObjectRelationshipAnalysis(loggerM_ptr);
	}
}

