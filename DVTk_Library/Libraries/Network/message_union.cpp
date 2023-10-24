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
//  DESCRIPTION     :	Received Mesage Union class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "base_socket.h"
#include "association.h"
#include "assoc_states.h"
#include "abort_rq.h"
#include "assoc_ac.h"
#include "assoc_rj.h"
#include "assoc_rq.h"
#include "rel_rp.h"
#include "rel_rq.h"
#include "message_union.h"

#include "Idicom.h"			// DICOM component interface


//>>===========================================================================

RECEIVE_MESSAGE_UNION_CLASS::RECEIVE_MESSAGE_UNION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	rxMessageTypeM = RX_MSG_FAILURE;
	associateRqM_ptr = NULL;
	associateAcM_ptr = NULL;
	associateRjM_ptr = NULL;
	releaseRqM_ptr = NULL;
	releaseRpM_ptr = NULL;
	abortRqM_ptr = NULL;
	commandM_ptr = NULL;
	datasetM_ptr = NULL;
}

//>>===========================================================================

RECEIVE_MESSAGE_UNION_CLASS::~RECEIVE_MESSAGE_UNION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	cleanup();
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::cleanup()

//  DESCRIPTION     : Clean up the resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up the resources
	rxMessageTypeM = RX_MSG_FAILURE;
	if (associateRqM_ptr)
	{
		delete associateRqM_ptr;
		associateRqM_ptr = NULL;
	}

	if (associateAcM_ptr) 
	{
		delete associateAcM_ptr;
		associateAcM_ptr = NULL;
	}

	if (associateRjM_ptr) 
	{
		delete associateRjM_ptr;
		associateRjM_ptr = NULL;
	}

	if (releaseRqM_ptr) 
	{
		delete releaseRqM_ptr;
		releaseRqM_ptr = NULL;
	}

	if (releaseRpM_ptr) 
	{
		delete releaseRpM_ptr;
		releaseRpM_ptr = NULL;
	}

	if (abortRqM_ptr) 
	{
		delete abortRqM_ptr;
		abortRqM_ptr = NULL;
	}

	if (commandM_ptr) 
	{
		delete commandM_ptr;
		commandM_ptr = NULL;
	}

	if (datasetM_ptr) 
	{
		delete datasetM_ptr;
		datasetM_ptr = NULL;
	}
}

//>>===========================================================================

enum RX_MSG_TYPE RECEIVE_MESSAGE_UNION_CLASS::getRxMsgType()

//  DESCRIPTION     : Get the received message type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return the received message type
	return rxMessageTypeM;
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setAssociateRequest(ASSOCIATE_RQ_CLASS *associateRq_ptr)

//  DESCRIPTION     : Set the received message as an Associate Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

    // check method argument
    if (associateRq_ptr != NULL)
    { 
        // set message type
        rxMessageTypeM = RX_MSG_ASSOCIATE_REQUEST;
    	associateRqM_ptr = associateRq_ptr;
    }
}

//>>===========================================================================

ASSOCIATE_RQ_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getAssociateRequest()

//  DESCRIPTION     : Get the received message - Associate Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ASSOCIATE_RQ_CLASS *associateRq_ptr = NULL;

	// return the message
	if (rxMessageTypeM == RX_MSG_ASSOCIATE_REQUEST)
	{
		associateRq_ptr = associateRqM_ptr;
	}

	return associateRq_ptr;
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setAssociateAccept(ASSOCIATE_AC_CLASS *associateAc_ptr)

//  DESCRIPTION     : Set the received message as an Associate Accept.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

    // check method argument
    if (associateAc_ptr != NULL)
    { 
        // set message type
        rxMessageTypeM = RX_MSG_ASSOCIATE_ACCEPT;
        associateAcM_ptr = associateAc_ptr;
    }
}

//>>===========================================================================

ASSOCIATE_AC_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getAssociateAccept()

//  DESCRIPTION     : Get the received message - Associate Accept.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ASSOCIATE_AC_CLASS *associateAc_ptr = NULL;

	// return the message
	if (rxMessageTypeM == RX_MSG_ASSOCIATE_ACCEPT)
	{
		associateAc_ptr = associateAcM_ptr;
	}

	return associateAc_ptr;
}
	
//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setAssociateReject(ASSOCIATE_RJ_CLASS *associateRj_ptr)

//  DESCRIPTION     : Set the received message as an Associate Reject.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

    // check method argument
    if (associateRj_ptr != NULL)
    { 
        // set message type
        rxMessageTypeM = RX_MSG_ASSOCIATE_REJECT;
        associateRjM_ptr = associateRj_ptr;
    }
}

//>>===========================================================================

ASSOCIATE_RJ_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getAssociateReject()

//  DESCRIPTION     : Get the received message - Associate Reject.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ASSOCIATE_RJ_CLASS *associateRj_ptr = NULL;

	// return the message
	if (rxMessageTypeM == RX_MSG_ASSOCIATE_REJECT)
	{
		associateRj_ptr = associateRjM_ptr;
	}

	return associateRj_ptr;
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setReleaseRequest(RELEASE_RQ_CLASS *releaseRq_ptr)

//  DESCRIPTION     : Set the received message as a Release Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

    // check method argument
    if (releaseRq_ptr != NULL)
    { 
        // set message type
        rxMessageTypeM = RX_MSG_RELEASE_REQUEST;
        releaseRqM_ptr = releaseRq_ptr;
    }
}

//>>===========================================================================

RELEASE_RQ_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getReleaseRequest()

//  DESCRIPTION     : Get the received message - Release Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RELEASE_RQ_CLASS *releaseRq_ptr = NULL;

	// return the message
	if (rxMessageTypeM == RX_MSG_RELEASE_REQUEST)
	{
		releaseRq_ptr = releaseRqM_ptr;
	}

	return releaseRq_ptr;
}
	
//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setReleaseResponse(RELEASE_RP_CLASS *releaseRp_ptr)

//  DESCRIPTION     : Set the received message as a Release Reponse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

    // check method argument
    if (releaseRp_ptr != NULL)
    { 
        // set message type
        rxMessageTypeM = RX_MSG_RELEASE_RESPONSE;
        releaseRpM_ptr = releaseRp_ptr;
    }
}

//>>===========================================================================

RELEASE_RP_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getReleaseResponse()

//  DESCRIPTION     : Get the received message - Release Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RELEASE_RP_CLASS *releaseRp_ptr = NULL;

	// return the message
	if (rxMessageTypeM == RX_MSG_RELEASE_RESPONSE)
	{
		releaseRp_ptr = releaseRpM_ptr;
	}

	return releaseRp_ptr;
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setAbortRequest(ABORT_RQ_CLASS *abortRq_ptr)

//  DESCRIPTION     : Set the received message as an Abort Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

    // check method argument
    if (abortRq_ptr != NULL)
    { 
        // set message type
        rxMessageTypeM = RX_MSG_ABORT_REQUEST;
        abortRqM_ptr = abortRq_ptr;
    }
}

//>>===========================================================================

ABORT_RQ_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getAbortRequest()

//  DESCRIPTION     : Get the received message - Abort Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ABORT_RQ_CLASS *abortRq_ptr = NULL;

	// return the message
	if (rxMessageTypeM == RX_MSG_ABORT_REQUEST)
	{
		abortRq_ptr = abortRqM_ptr;
	}

	return abortRq_ptr;
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setCommandDataset(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Set the received message as a DICOM Message.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

	// check on input parameters
	if ((command_ptr) &&
		(dataset_ptr))
	{
		// set message type
		rxMessageTypeM = RX_MSG_DICOM_COMMAND_DATASET;
		commandM_ptr = command_ptr;
		datasetM_ptr = dataset_ptr;
	}
	else if (command_ptr)
	{
		// set message type
		rxMessageTypeM = RX_MSG_DICOM_COMMAND;
		commandM_ptr = command_ptr;
	}
	else
	{
		// set message type
		rxMessageTypeM = RX_MSG_FAILURE;
		if (dataset_ptr) delete dataset_ptr;
	}
}

//>>===========================================================================

DCM_COMMAND_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getCommand()

//  DESCRIPTION     : Get the received message - DICOM Command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_COMMAND_CLASS *command_ptr = NULL;

	// return the message
	if ((rxMessageTypeM == RX_MSG_DICOM_COMMAND_DATASET) ||
		(rxMessageTypeM == RX_MSG_DICOM_COMMAND))
	{
		command_ptr = commandM_ptr;
	}

	return command_ptr;
}

//>>===========================================================================

DCM_DATASET_CLASS *RECEIVE_MESSAGE_UNION_CLASS::getDataset()

//  DESCRIPTION     : Get the received message - DICOM Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_DATASET_CLASS *dataset_ptr = NULL;

	// return the message
	if (rxMessageTypeM == RX_MSG_DICOM_COMMAND_DATASET)
	{
		dataset_ptr = datasetM_ptr;
	}

	return dataset_ptr;
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setFailure()

//  DESCRIPTION     : Set the received message as a failure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

	// set message type
	rxMessageTypeM = RX_MSG_FAILURE;
}

//>>===========================================================================

void RECEIVE_MESSAGE_UNION_CLASS::setIncompleteByteStreamFailure()

//  DESCRIPTION     : Set the received message as a failure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any old messages
	cleanup();

	// set message type
	rxMessageTypeM = RX_INCOMPLETE_BYTE_STREAM;
}
