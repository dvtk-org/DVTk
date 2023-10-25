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
//  DESCRIPTION     :	Commit SCP emulator class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "commit_scp_emulator.h"
#include "Idefinition.h"			// Definition component interface
#include "Imedia.h"					// Media component interface
#include "Isession.h"				// Session component interface
#include "Ivalidation.h"			// Validation component interface


//>>===========================================================================

COMMIT_SCP_EMULATOR_CLASS::COMMIT_SCP_EMULATOR_CLASS(EMULATOR_SESSION_CLASS *session_ptr, BASE_SOCKET_CLASS* socket_ptr, bool logEmulation)

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

COMMIT_SCP_EMULATOR_CLASS::~COMMIT_SCP_EMULATOR_CLASS()

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

bool COMMIT_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(EMULATOR_SESSION_CLASS *session_ptr)

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

	// add this sop class to the supported sop classes
	session_ptr->addSupportedSopClass(STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID);

	// return result
	return true;
}

//>>===========================================================================

bool COMMIT_SCP_EMULATOR_CLASS::processCommandDataset(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS*)

//  DESCRIPTION     : Process the Storage command and dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// handle individual commands
	switch(command_ptr->getCommandId())
	{
	case DIMSE_CMD_NEVENTREPORT_RQ:
		{
			// set status successful
			UINT16 status = DCM_STATUS_SUCCESS;

			// return the N-EVENTREPORT-RSP
			result =  sendResponse(DIMSE_CMD_NEVENTREPORT_RSP, 
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

void COMMIT_SCP_EMULATOR_CLASS::completeLogging()

//  DESCRIPTION     : Complete the emulation logging.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
}

