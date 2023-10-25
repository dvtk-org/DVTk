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
//  DESCRIPTION     :	Print SCP emulator class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "print_emulator.h"
#include "print.h"
#include "Idefinition.h"			// Definition component interface
#include "Isession.h"				// Session component interface


//>>===========================================================================

PRINT_SCP_EMULATOR_CLASS::PRINT_SCP_EMULATOR_CLASS(EMULATOR_SESSION_CLASS *session_ptr, BASE_SOCKET_CLASS* socket_ptr, bool logEmulation)

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
	filmSessionM_ptr = NULL;
}

//>>===========================================================================

PRINT_SCP_EMULATOR_CLASS::~PRINT_SCP_EMULATOR_CLASS()

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

	if (filmSessionM_ptr)
	{
		// free the film session
		delete filmSessionM_ptr;
	}
}

//>>===========================================================================

bool PRINT_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(EMULATOR_SESSION_CLASS *session_ptr)

//  DESCRIPTION     : Add the supported Print presentation contexts.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	AE_SESSION_CLASS ae_session;

	// set the ae session
	ae_session.SetName(session_ptr->getApplicationEntityName());
	ae_session.SetVersion(session_ptr->getApplicationEntityVersion());

	// use loaded definition files
	for (UINT i = 0; i < session_ptr->noDefinitionFiles(); i++) 
	{
		DEFINITION_FILE_CLASS *definitionFile_ptr = session_ptr->getDefinitionFile(i);

		// use the sop class uid
		DEF_DETAILS_CLASS file_details;
		if (definitionFile_ptr->GetDetails(file_details))
		{
			string sopClassUid = file_details.GetSOPClassUID();

			// check if this is a print sop class
			if (DEFINITION->IsPrintSop(sopClassUid, &ae_session))
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

bool PRINT_SCP_EMULATOR_CLASS::processCommandDataset(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Process the Print command (and dataset).
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
	case DIMSE_CMD_NACTION_RQ:
		// process the ACTION command
		result = processAction(command_ptr->getEncodePresentationContextId());
		break;
	case DIMSE_CMD_NCREATE_RQ:
	    if ((sopClassUidM == FILM_SESSION_SOP_CLASS_UID) ||
		    (dataset_ptr))
		{
			// process the CREATE command
            // - Basic Film Session may not define a dataset - but that is OK
			result = processCreate(command_ptr->getEncodePresentationContextId(), dataset_ptr);
		}
		else
		{
			// missing dataset
			UINT16 status = DCM_STATUS_PROCESSING_FAILURE;
			result = sendResponse(DIMSE_CMD_NCREATE_RSP,
				command_ptr->getEncodePresentationContextId(),
				status);
		}
		break;
	case DIMSE_CMD_NDELETE_RQ:
		// process the DELETE command
		result = processDelete(command_ptr->getEncodePresentationContextId());
		break;
	case DIMSE_CMD_NEVENTREPORT_RSP:
		// nothing to do here
		result = true;
		break;
	case DIMSE_CMD_NGET_RQ:
		// process the GET command
		result = processGet(command_ptr);
		break;
	case DIMSE_CMD_NSET_RQ:
		if (dataset_ptr)
		{
			// process the SET dataset
			result = processSet(command_ptr->getEncodePresentationContextId(), dataset_ptr);
		}
		else
		{
			// missing dataset
			UINT16 status = DCM_STATUS_PROCESSING_FAILURE;
			result = sendResponse(DIMSE_CMD_NSET_RSP,
				command_ptr->getEncodePresentationContextId(), 
				status);
		}
		break;
	default:
		{
			// unknown command
			UINT16 status = DCM_STATUS_UNRECOGNIZED_OPERATION;
			result = sendResponse(command_ptr->getCommandId(),
				command_ptr->getEncodePresentationContextId(), 
				status);
		}
		break;
	}

	// return result
	return result;
}

//>>===========================================================================

bool PRINT_SCP_EMULATOR_CLASS::processAction(BYTE presentationContextId)

//  DESCRIPTION     : Process the Print ACTION command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_DATASET_CLASS	*response_ptr = NULL;
	UINT16			status = DCM_STATUS_SUCCESS;

	// check if Print Job negotiated
	bool queueJob = false;
	UID_CLASS	uid(PRINT_JOB_SOP_CLASS_UID);
	if (associationM.getPresentationContextId(uid))
	{
		queueJob = true;
	}

	// action (print) Film Session ?
	if (sopClassUidM == FILM_SESSION_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL) 
		{
			status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
		}
		else 
		{
			status = filmSessionM_ptr->action(queueJob, &response_ptr);
		}
	}
	// action (print) Film Box ?
	else if (sopClassUidM == FILM_BOX_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			int index;
			if ((index = filmSessionM_ptr->isFilmBox(sopInstanceUidM)) != -1) 
			{
				BASIC_FILM_BOX_CLASS *filmBox_ptr = filmSessionM_ptr->getFilmBox((UINT) index);
				status = filmBox_ptr->action(filmSessionM_ptr, index, queueJob, &response_ptr);
			}
			else 
			{
				status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
			}
		}
	}
	else 
	{
		status = DCM_STATUS_UNRECOGNIZED_OPERATION;
	}

	// return the response
	return sendResponse(DIMSE_CMD_NACTION_RSP, presentationContextId, status, response_ptr);
}

//>>===========================================================================

bool PRINT_SCP_EMULATOR_CLASS::processCreate(BYTE presentationContextId, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Process the Print CREATE dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_DATASET_CLASS *response_ptr = NULL;
	UINT16 status = DCM_STATUS_SUCCESS;

	// check if the SOP instance is defined by the SCU
	if (!sopInstanceUidM.length())
	{
		// generate our own
		makeSopInstanceUid();
	}

	// create Film Session ?
	if (sopClassUidM == FILM_SESSION_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr != NULL)
		{
			status = DCM_STATUS_DUPLICATE_INVOCATION;
		}
		else
		{
			filmSessionM_ptr = new BASIC_FILM_SESSION_CLASS(sessionM_ptr, loggerM_ptr, sopInstanceUidM, dataset_ptr);
		}
	}
	// create Film Box ?
	else if (sopClassUidM == FILM_BOX_SOP_CLASS_UID) 
	{
		if (filmSessionM_ptr == NULL) 
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			if (filmSessionM_ptr->isFilmBox(sopInstanceUidM) == -1) 
			{
				BASIC_FILM_BOX_CLASS *filmBox_ptr = new BASIC_FILM_BOX_CLASS(sessionM_ptr, loggerM_ptr, filmSessionM_ptr->noFilmBoxes() + 1, sopInstanceUidM, dataset_ptr);
				UID_CLASS abstractSyntaxName;

				// need to establish the correct Image Box SOP Class UID
				char *imageBoxSopClassUid_ptr = GRAY_IMAGE_BOX_SOP_CLASS_UID;

				// get the abstract syntax name from the current presentation context
				if (associationM.getCurrentAbstractSyntaxName(abstractSyntaxName))
				{
					if ((abstractSyntaxName == REFERENCED_GRAY_PRINT_META) ||
						(abstractSyntaxName == REFERENCED_COLOR_PRINT_META))
					{
						imageBoxSopClassUid_ptr = REFERENCED_IMAGE_BOX_SOP_CLASS_UID;
					}
					else if (abstractSyntaxName == BASIC_COLOR_PRINT_META)
					{
						imageBoxSopClassUid_ptr = COLOR_IMAGE_BOX_SOP_CLASS_UID;
					}
				}

				status = filmBox_ptr->create(imageBoxSopClassUid_ptr, &response_ptr);
				filmSessionM_ptr->addFilmBox(filmBox_ptr);
			}
			else 
			{
				status = DCM_STATUS_DUPLICATE_INVOCATION;		
			}
		}
	}
	// create Image Overlay ?
	else if (sopClassUidM == IMAGE_OVERLAY_SOP_CLASS_UID) 
	{
		if (filmSessionM_ptr == NULL) 
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			if (filmSessionM_ptr->isImageOverlay(sopInstanceUidM) == -1)
			{
				IMAGE_OVERLAY_CLASS *overlay_ptr = new IMAGE_OVERLAY_CLASS(sopInstanceUidM, dataset_ptr);
				filmSessionM_ptr->addImageOverlay(overlay_ptr);
			}
			else 
			{
				status = DCM_STATUS_DUPLICATE_INVOCATION;		
			}
		}
	}
	// create VOI Lut Box ?
	else if (sopClassUidM == VOI_LUT_BOX_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			if (filmSessionM_ptr->isVoiLutBox(sopInstanceUidM) == -1)
			{
				VOI_LUT_BOX_CLASS *voiLut_ptr = new VOI_LUT_BOX_CLASS(sopInstanceUidM, dataset_ptr);
				filmSessionM_ptr->addVoiLutBox(voiLut_ptr);
			}
			else 
			{
				status = DCM_STATUS_DUPLICATE_INVOCATION;		
			}
		}
	}
	// create Presentation LUT ?
	else if (sopClassUidM == PRESENTATION_LUT_SOP_CLASS_UID)
	{
		if (MYPRINTER->isPresentationLut(sopInstanceUidM) == -1)
		{
			PRESENTATION_LUT_CLASS *presentationLut_ptr = new PRESENTATION_LUT_CLASS(sopInstanceUidM, dataset_ptr);
			MYPRINTER->addPresentationLut(presentationLut_ptr);
		}
		else 
		{
			status = DCM_STATUS_DUPLICATE_INVOCATION;		
		}
	}
	else 
	{
		status = DCM_STATUS_UNRECOGNIZED_OPERATION;
	}

	// return response
	return sendResponse(DIMSE_CMD_NCREATE_RSP, presentationContextId, status, response_ptr);
}

//>>===========================================================================

bool PRINT_SCP_EMULATOR_CLASS::processDelete(BYTE presentationContextId)

//  DESCRIPTION     : Process the Print DELETE command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 status = DCM_STATUS_SUCCESS;
	int index;

	// delete Film Session ?
	if (sopClassUidM == FILM_SESSION_SOP_CLASS_UID) 
	{
		if (filmSessionM_ptr == NULL) 
		{
			status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
		}
		else 
		{
			delete filmSessionM_ptr;
			filmSessionM_ptr = NULL;
		}
	}
	// delete Film Box ?
	else if (sopClassUidM == FILM_BOX_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			if ((index = filmSessionM_ptr->isFilmBox(sopInstanceUidM)) != -1)
			{
				filmSessionM_ptr->removeFilmBox((UINT) index);
			}
			else 
			{
				status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
			}
		}
	}
	// delete Image Overlay ?
	else if (sopClassUidM == IMAGE_OVERLAY_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			if ((index = filmSessionM_ptr->isImageOverlay(sopInstanceUidM)) != -1)
			{
				filmSessionM_ptr->removeImageOverlay((UINT) index);
			}
			else 
			{
				status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
			}
		}
	}
	// delete VOI Lut Box ?
	else if (sopClassUidM == VOI_LUT_BOX_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else
		{
			if ((index = filmSessionM_ptr->isVoiLutBox(sopInstanceUidM)) != -1)
			{
				filmSessionM_ptr->removeVoiLutBox((UINT) index);
			}
			else 
			{
				status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
			}
		}
	}
	// delete Presentation Lut ?
	else if (sopClassUidM == PRESENTATION_LUT_SOP_CLASS_UID)
	{
		if ((index = MYPRINTER->isPresentationLut(sopInstanceUidM)) != -1)
		{
			MYPRINTER->removePresentationLut((UINT) index);
		}
		else 
		{
			status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
		}
	}
	else 
	{
		status = DCM_STATUS_UNRECOGNIZED_OPERATION;
	}

	// return response
	return sendResponse(DIMSE_CMD_NDELETE_RSP, presentationContextId, status);
}

//>>===========================================================================

bool PRINT_SCP_EMULATOR_CLASS::processGet(DCM_COMMAND_CLASS *command_ptr)

//  DESCRIPTION     : Process the Print GET command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_DATASET_CLASS	*response_ptr = NULL;
	UINT16			status;

	// get Printer information ?
	if (sopClassUidM == PRINTER_SOP_CLASS_UID)
	{
		// get the printer instance
		status = MYPRINTER->get(command_ptr, &response_ptr);
	}
	// get Print Job information ?
	else if (sopClassUidM == PRINT_JOB_SOP_CLASS_UID)
	{
		// get the print job instance from the print queue
		status = MYPRINTQUEUE->get(command_ptr, &response_ptr);			
	}
	else 
	{
		status = DCM_STATUS_UNRECOGNIZED_OPERATION;
	}

	// return response
	return sendResponse(DIMSE_CMD_NGET_RSP, 
		command_ptr->getEncodePresentationContextId(), 
		status, 
		response_ptr);
}

//>>===========================================================================

bool PRINT_SCP_EMULATOR_CLASS::processSet(BYTE presentationContextId, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Process the Print SET dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 status;
	int index;

	// set Film Session ?
	if (sopClassUidM == FILM_SESSION_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL) 
		{
			status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
		}
		else 
		{
			status = filmSessionM_ptr->set(dataset_ptr);
		}
	}
	// set Film Box ?
	else if (sopClassUidM == FILM_BOX_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else
		{
			if ((index = filmSessionM_ptr->isFilmBox(sopInstanceUidM)) != -1)
			{
				BASIC_FILM_BOX_CLASS *filmBox_ptr = filmSessionM_ptr->getFilmBox((UINT) index);
				status = filmBox_ptr->set(dataset_ptr);
			}
			else 
			{
				status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
			}
		}
	}
	// set Image Box ?
	else if ((sopClassUidM == GRAY_IMAGE_BOX_SOP_CLASS_UID) ||
			(sopClassUidM == COLOR_IMAGE_BOX_SOP_CLASS_UID) ||
			(sopClassUidM == REFERENCED_IMAGE_BOX_SOP_CLASS_UID))
	{
		if (filmSessionM_ptr == NULL) 
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;

			for (int i = 0; i < filmSessionM_ptr->noFilmBoxes(); i++)
			{
				BASIC_FILM_BOX_CLASS *filmBox_ptr = filmSessionM_ptr->getFilmBox(i);
				if ((index = filmBox_ptr->isImageBox(sopInstanceUidM)) != -1) 
				{
					IMAGE_BOX_CLASS *imageBox_ptr = filmBox_ptr->getImageBox(index);
					status = imageBox_ptr->set(dataset_ptr);
					break;
				}
			}
		}
	}
	// set Annotation Box ?
	else if (sopClassUidM == ANNOTATION_BOX_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;

			for (int i = 0; i < filmSessionM_ptr->noFilmBoxes(); i++) 
			{
				BASIC_FILM_BOX_CLASS *filmBox_ptr = filmSessionM_ptr->getFilmBox(i);
				if ((index = filmBox_ptr->isAnnotationBox(sopInstanceUidM)) != -1)
				{
					ANNOTATION_BOX_CLASS *annotationBox_ptr = filmBox_ptr->getAnnotationBox(index);
					status = annotationBox_ptr->set(dataset_ptr);
					break;
				}
			}
		}
	}
	// set Image Overlay ?
	else if (sopClassUidM == IMAGE_OVERLAY_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else
		{
			if ((index = filmSessionM_ptr->isImageOverlay(sopInstanceUidM)) != -1)
			{
				IMAGE_OVERLAY_CLASS *overlay_ptr = filmSessionM_ptr->getImageOverlay((UINT) index);
				status = overlay_ptr->set(dataset_ptr);
			}
			else 
			{
				status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
			}
		}
	}
	// set VOI Lut Box ?
	else if (sopClassUidM == VOI_LUT_BOX_SOP_CLASS_UID)
	{
		if (filmSessionM_ptr == NULL)
		{
			status = DCM_STATUS_PROCESSING_FAILURE;
		}
		else 
		{
			if ((index = filmSessionM_ptr->isVoiLutBox(sopInstanceUidM)) != -1)
			{
				VOI_LUT_BOX_CLASS *voiLut_ptr = filmSessionM_ptr->getVoiLutBox((UINT) index);
				status = voiLut_ptr->set(dataset_ptr);
			}
			else 
			{
				status = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
			}
		}
	}
	else 
	{
		status = DCM_STATUS_UNRECOGNIZED_OPERATION;
	}

	// return response
	return sendResponse(DIMSE_CMD_NSET_RSP, presentationContextId, status);
}

//>>===========================================================================

void PRINT_SCP_EMULATOR_CLASS::makeSopInstanceUid()

//  DESCRIPTION     : Generate a sop instance uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char buffer[UI_LENGTH + 1];

	createUID(buffer, (char*) sessionM_ptr->getImplementationClassUid());
	sopInstanceUidM = buffer;
}

//>>===========================================================================

bool PRINT_SCP_EMULATOR_CLASS::sendStatusEvent()

//  DESCRIPTION     : Send Printer Status Event over association.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool			result;
	UINT16			eventTypeId = 1;
	DCM_DATASET_CLASS	*dataset_ptr = NULL;

	// instantiate the event request
	DCM_COMMAND_CLASS *command_ptr = new DCM_COMMAND_CLASS(DIMSE_CMD_NEVENTREPORT_RQ);

	// cascade the logger
	command_ptr->setLogger(loggerM_ptr);

	// set the command sop class // instance uids
	string printerSopClassUid = PRINTER_SOP_CLASS_UID;
	string printerSopInstanceUid = PRINTER_SOP_INSTANCE_UID;

	// set the sop class and instance uids in the association
	associationM.setSopClassUid(printerSopClassUid);
	associationM.setSopInstanceUid(printerSopInstanceUid);

	// set the N-EVENT-REPORT-RQ command attribute values
	(void) command_ptr->setUIValue(TAG_AFFECTED_SOP_CLASS_UID, printerSopClassUid);
	(void) command_ptr->setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, printerSopInstanceUid);
	(void) command_ptr->setUSValue(TAG_MESSAGE_ID, 0x0001);

	// check whether or not we need to send more detail in the dataset
	if (strcmp(MYPRINTER->getStatus(), "NORMAL") != 0) 
	{		
		// instantiate the printer dataset
		dataset_ptr = new DCM_DATASET_CLASS();

		// set up the printer detail
		(void) dataset_ptr->setLOValue(TAG_PRINTER_NAME, MYPRINTER->getName());
		(void) dataset_ptr->setCSValue(TAG_PRINTER_STATUS_INFO, MYPRINTER->getStatusInfo());

		// indicate WARNING
		eventTypeId = 2;

		// check for ERROR
		if (strcmp(MYPRINTER->getStatus(), "FAILURE") == 0)
		{
			// indicate ERROR
			eventTypeId = 3;
		}
	}
	
	// set the event type id
	(void) command_ptr->setUSValue(TAG_EVENT_TYPE_ID, eventTypeId);

	// return the response
	if (dataset_ptr)
	{
		// cascade the logger
		dataset_ptr->setLogger(loggerM_ptr);

		// log action
		if (loggerM_ptr)
		{
			string sopName = DEFINITION->GetSopName(printerSopClassUid);
			loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s %s (%s)", mapCommandName(command_ptr->getCommandId()), sopName.c_str(), timeStamp());
		}

		// return response command with dataset
		result = associationM.send(command_ptr, dataset_ptr);

		// clean up the dataset
		delete dataset_ptr;
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", mapCommandName(command_ptr->getCommandId()), timeStamp());
		}

		// return response command only
		result = associationM.send(command_ptr);
	}

	// clean up the command
	delete command_ptr;

	// return result
	return result;
}


