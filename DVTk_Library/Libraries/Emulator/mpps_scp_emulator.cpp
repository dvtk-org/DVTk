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
//  DESCRIPTION     :	Mpps SCP emulator class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "mpps_scp_emulator.h"
#include "Idefinition.h"			// Definition component interface
#include "Imedia.h"					// Media component interface
#include "Isession.h"				// Session component interface
#include "Ivalidation.h"			// Validation component interface


//>>===========================================================================

MPPS_SCP_EMULATOR_CLASS::MPPS_SCP_EMULATOR_CLASS(EMULATOR_SESSION_CLASS *session_ptr, BASE_SOCKET_CLASS* socket_ptr, bool logEmulation)

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
}

//>>===========================================================================

MPPS_SCP_EMULATOR_CLASS::~MPPS_SCP_EMULATOR_CLASS()

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

bool MPPS_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(EMULATOR_SESSION_CLASS *session_ptr)

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

	// use loaded definition files
	for (UINT i = 0; i < session_ptr->noDefinitionFiles(); i++) 
	{
		DEFINITION_FILE_CLASS *definitionFile_ptr = session_ptr->getDefinitionFile(i);

		// use the sop class uid
		DEF_DETAILS_CLASS file_details;
		if (definitionFile_ptr->GetDetails(file_details))
		{
			string sopClassUid = file_details.GetSOPClassUID();

			// check if this is a mpps sop class
			if (DEFINITION->IsMppsSop(sopClassUid, &ae_session))
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

bool MPPS_SCP_EMULATOR_CLASS::processCommandDataset(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr)

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
	case DIMSE_CMD_NCREATE_RQ:
		if (dataset_ptr)
		{
			// process the CREATE command
			result = processCreate(command_ptr->getEncodePresentationContextId(),
				dataset_ptr);
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

	case DIMSE_CMD_NSET_RQ:
		if (dataset_ptr)
		{
			// process the SET command
			result = processSet(command_ptr->getEncodePresentationContextId(),
				dataset_ptr);
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

bool MPPS_SCP_EMULATOR_CLASS::processCreate(BYTE presentationContextId, DCM_DATASET_CLASS*)

//  DESCRIPTION     : Process the N-CREATE-RQ with dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 status = DCM_STATUS_PROCESSING_FAILURE;

	// set status successful
	status = DCM_STATUS_SUCCESS;

	// return the N-CREATE-RSP
	return sendResponse(DIMSE_CMD_NCREATE_RSP, presentationContextId, status);
}

//>>===========================================================================

bool MPPS_SCP_EMULATOR_CLASS::processSet(BYTE presentationContextId, DCM_DATASET_CLASS*)

//  DESCRIPTION     : Process the N-SET-RQ with dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 status = DCM_STATUS_PROCESSING_FAILURE;

	// set status successful
	status = DCM_STATUS_SUCCESS;

	// return the N-SET-RSP
	return sendResponse(DIMSE_CMD_NSET_RSP, presentationContextId, status);
}

//>>===========================================================================

void MPPS_SCP_EMULATOR_CLASS::completeLogging()

//  DESCRIPTION     : Complete the emulation logging.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
}

