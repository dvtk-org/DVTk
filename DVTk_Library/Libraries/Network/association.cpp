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
#include "base_socket.h"
#include "association.h"
#include "assoc_states.h"
#include "abort_rq.h"
#include "assoc_ac.h"
#include "assoc_rj.h"
#include "assoc_rq.h"
#include "rel_rp.h"
#include "rel_rq.h"
#include "unknown_pdu.h"
#include "message_union.h"

#include "Idicom.h"			// DICOM component interface
#include "Idefinition.h"	// Definition component interface
#include "Imedia.h"			// Media component interface


//>>===========================================================================

ASSOCIATION_CLASS::ASSOCIATION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	socketM_ptr = NULL;
	calledAeTitleM = CALLED_AE_TITLE;
	callingAeTitleM = CALLING_AE_TITLE;
	setMaximumLengthReceived(MAXIMUM_LENGTH_RECEIVED);
	actualMaximumLengthToBeReceivedM = 0;
	implementationClassUidM.set(IMPLEMENTATION_CLASS_UID);
	implementationVersionNameM = IMPLEMENTATION_VERSION_NAME;
	scpScuRoleUidM.set("");
	scpRoleM = 0;
	scuRoleM = 0;
	associateRqM_ptr = NULL;
	associateAcM_ptr = NULL;
	associateRjM_ptr = NULL;
	releaseRqM_ptr = NULL;
	releaseRpM_ptr = NULL;
	abortRqM_ptr = NULL;
	unknownPduM_ptr = NULL;
	eventNumberM = DULP_INVALID_PDU_RECEIVED;
	sopClassUidM = "";
	sopInstanceUidM = "";
	messageIdM = 1;
	messageIdBeingRespondedToM = 1;
    commandRqFieldSentM = 0;
    pcIdUsedToSendRequestM = 0;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	strictMatchM = false;
	storeCSTOREObjectsM = false;
	setSessionId(1);
	setStorageMode(SM_NO_STORAGE);
	setLogger(NULL);
    setSerializer(NULL);
	pc_idM = 0;
	startClockM = 0;

	// initialise the state machine
	dulpStateM_ptr = STATE1_CLASS::instance();
}

//>>===========================================================================

ASSOCIATION_CLASS::~ASSOCIATION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	// - reset the logger
	setLogger(NULL);

	// clean up any PDUs from the underlying state machine
	cleanup();

	supportedPCM.clear();

	// delete the socket
	if (socketM_ptr)
	{
		delete socketM_ptr;
	}
}

//>>===========================================================================

void ASSOCIATION_CLASS::cleanup(bool acceptedToo)

//  DESCRIPTION     : Free up any PDUs from the underlying state machine.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clear up any accepted presentation contexts
	if (acceptedToo)
	{
		acceptedPCM.clear();
	}

	// reset the message counters
	pc_idM = 0;
	messageIdM = 1;
	messageIdBeingRespondedToM = 1;

	// free up any valid pointers
	if (associateRqM_ptr) delete associateRqM_ptr;
	associateRqM_ptr = NULL;

	if (associateAcM_ptr) delete associateAcM_ptr;
	associateAcM_ptr = NULL;

	if (associateRjM_ptr) delete associateRjM_ptr;
	associateRjM_ptr = NULL;

	if (releaseRqM_ptr) delete releaseRqM_ptr;
	releaseRqM_ptr = NULL;

	if (releaseRpM_ptr) delete releaseRpM_ptr;
	releaseRpM_ptr = NULL;

	if (abortRqM_ptr) delete abortRqM_ptr;
	abortRqM_ptr = NULL;

	if (unknownPduM_ptr) delete unknownPduM_ptr;
	unknownPduM_ptr = NULL;
}

//>>===========================================================================

void ASSOCIATION_CLASS::setRemoteHostname(char *remoteHostname_ptr)

//  DESCRIPTION     : Set the remote hostname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (socketM_ptr)
	{
		socketM_ptr->setRemoteHostname(remoteHostname_ptr);
	}
}

//>>===========================================================================

void ASSOCIATION_CLASS::setRemoteConnectPort(UINT16 remoteConnectPort)

//  DESCRIPTION     : Set the remote connect port
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (socketM_ptr)
	{
		socketM_ptr->setRemoteConnectPort(remoteConnectPort);
	}
}

//>>===========================================================================

void ASSOCIATION_CLASS::setLocalListenPort(UINT16 localListenPort)

//  DESCRIPTION     : Set the local listen port
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (socketM_ptr)
	{
		socketM_ptr->setLocalListenPort(localListenPort);
	}
}

//>>===========================================================================

void ASSOCIATION_CLASS::erase()

//  DESCRIPTION     : Erases all set association parameters
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{

	// clean up any PDUs from the underlying state machine
	cleanup();

	// constructor activities
	calledAeTitleM = CALLED_AE_TITLE;
	callingAeTitleM = CALLING_AE_TITLE;
	setMaximumLengthReceived(MAXIMUM_LENGTH_RECEIVED);
	actualMaximumLengthToBeReceivedM = 0;
	implementationClassUidM.set(IMPLEMENTATION_CLASS_UID);
	implementationVersionNameM = IMPLEMENTATION_VERSION_NAME;

	// clear presentation contexts
	acceptedPCM.clear();
	supportedPCM.clear();
	pc_idM = 0;
	
	associateRqM_ptr = NULL;
	associateAcM_ptr = NULL;
	associateRjM_ptr = NULL;
	releaseRqM_ptr = NULL;
	releaseRpM_ptr = NULL;
	abortRqM_ptr = NULL;
	unknownPduM_ptr = NULL;
	eventNumberM = DULP_INVALID_PDU_RECEIVED;
	sopClassUidM = "";
	sopInstanceUidM = "";
	messageIdM = 1;
	messageIdBeingRespondedToM = 1;
	strictMatchM = false;
	storeCSTOREObjectsM = false;
	setSessionId(1);
	setStorageMode(SM_NO_STORAGE);
	setLogger(NULL);
}
	
//>>===========================================================================

bool ASSOCIATION_CLASS::createSocket(SOCKET_PARAMETERS& socketParams)

//  DESCRIPTION     : Create the socket to be used.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : If a socket already exists, it will continue to be used unless the socket 
//					  parameters have changed
//<<===========================================================================
{
	if (socketM_ptr)
	{
		// check to see if the socket parameters have changed
		if (socketM_ptr->socketParametersChanged(socketParams))
		{
			// socket parameters have changed, reset the association and delete the old socket
			// note that this will close the socket if it is open!
			if (socketM_ptr->isConnected() && loggerM_ptr)
			{
				loggerM_ptr->text(LOG_WARNING, 1, "Closing socket because socket parameters have changed");
			}
			reset();
			delete socketM_ptr;
			socketM_ptr = NULL;

			// fall through to create a new socket
		}
		else
		{
			// socket parameters have not changed, continue to use the existing socket
			socketM_ptr->setOwnerThread(getThreadId()); // set this thread as the owner of the thread
			return true;
		}
	}

	// create a new socket
	socketM_ptr = BASE_SOCKET_CLASS::createSocket(socketParams, loggerM_ptr);
	if (socketM_ptr)
	{
		return true;
	}
	else
	{
		return false;
	}
}
	
//>>===========================================================================

void ASSOCIATION_CLASS::setSocket(BASE_SOCKET_CLASS* socket_ptr)

//  DESCRIPTION     : Sets the socket to be used.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : If a socket already exists, it will be freed (which could close the association) 
//<<===========================================================================
{
	if (socketM_ptr)
	{
		// socket exists, reset the association and delete the old socket
		// note that this will close the socket if it is open!
		reset();
		delete socketM_ptr;
	}

	// set the socket
	socketM_ptr = socket_ptr;
	if (socketM_ptr)
	{
		socketM_ptr->setLogger(loggerM_ptr);
	}
}
	
//>>===========================================================================

void ASSOCIATION_CLASS::changeState(DULP_STATE_CLASS *dulp_state_ptr)

//  DESCRIPTION     : Change internal state to that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// see if association is starting
	if (dulpStateM_ptr == STATE1_CLASS::instance())
	{
		if (dulp_state_ptr != STATE1_CLASS::instance())
		{
			// new association, save the start time
			startClockM = clock();
		}
	}

	// see if association is ending
	if ((dulpStateM_ptr != STATE1_CLASS::instance()) && 
		(dulpStateM_ptr != STATE13_CLASS::instance()))
	{
		if ((dulp_state_ptr == STATE1_CLASS::instance()) || 
			(dulp_state_ptr == STATE13_CLASS::instance()))
		{
			// end of association, log the time
			if ((startClockM != 0) && (loggerM_ptr))
			{
				loggerM_ptr->text(LOG_INFO, 1, "Association open %.2f seconds", 
					((double)(clock() - startClockM) / CLOCKS_PER_SEC));
			}
		}
	}

	// change the state to that given
	dulpStateM_ptr = dulp_state_ptr;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::associateRequestLocal()

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ASSOCIATE_REQUEST_LOCAL;

	// use the underlying state machine classes
	return dulpStateM_ptr->associateRequestLocal(this);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::transportConfirmLocal(ASSOCIATE_RQ_CLASS *associateRq_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_TRANSPORT_CONFIRM_LOCAL;

	// link the PDU for transmission
	associateRqM_ptr = associateRq_ptr;

	// use the underlying state machine classes
	bool result = dulpStateM_ptr->transportConfirmLocal(this);

	// unlink the PDU from the association - allows caller to clean up
	associateRqM_ptr = NULL;

	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::associateAcceptPduReceived(PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->associateAcceptPduReceived(this, pdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::associateRejectPduReceived(PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ASSOCIATE_REJECT_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->associateRejectPduReceived(this, pdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::transportIndicationLocal()

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_TRANSPORT_INDICATION_LOCAL;

	// use the underlying state machine classes
	return dulpStateM_ptr->transportIndicationLocal(this);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::associateRequestPduReceived(PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ASSOCIATE_REQUEST_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->associateRequestPduReceived(this, pdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::associateResponseAcceptLocal(ASSOCIATE_AC_CLASS *associateAc_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ASSOCIATE_RESPONSE_ACCEPT_LOCAL;

	// link the PDU for transmission
	associateAcM_ptr = associateAc_ptr;

	// use the underlying state machine classes
	bool result = dulpStateM_ptr->associateResponseAcceptLocal(this);

	// unlink the PDU from the association - allows caller to clean up
	associateAcM_ptr = NULL;

	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::associateResponseRejectLocal(ASSOCIATE_RJ_CLASS *associateRj_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ASSOCIATE_RESPONSE_REJECT_LOCAL;

	// link the PDU for transmission
	associateRjM_ptr = associateRj_ptr;

	// use the underlying state machine classes
	bool result = dulpStateM_ptr->associateResponseRejectLocal(this);

	// unlink the PDU from the association - allows caller to clean up
	associateRjM_ptr = NULL;

	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::dataTransferRequestLocal()

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// record event
	eventNumberM = DULP_DATA_TRANSFER_REQUEST_LOCAL;

	// send all the local data transfer requests
	for (UINT i = 0; i < networkTransferM.noDataTransferPdus(); i++)
	{
		DATA_TF_PDU_CLASS *dataTfPdu_ptr = networkTransferM.getDataTransferPdu(i);

		// send one at a time using the underlying state machine classes
		if (!dulpStateM_ptr->dataTransferRequestLocal(this, dataTfPdu_ptr))
		{
			// on failure - return
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Can't export Data Transfer PDU(s) - connection has been closed.");
			}
			result = false;
			break;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::dataTransferPduReceived(DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_DATA_TRANSFER_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->dataTransferPduReceived(this, dataTfPdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::releaseRequestLocal(RELEASE_RQ_CLASS *releaseRq_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_RELEASE_REQUEST_LOCAL;

	// link the PDU for transmission
	releaseRqM_ptr = releaseRq_ptr;

	// use the underlying state machine classes
	bool result = dulpStateM_ptr->releaseRequestLocal(this);

	// unlink the PDU from the association - allows caller to clean up
	releaseRqM_ptr = NULL;

	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::releaseRequestPduReceived(PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_RELEASE_REQUEST_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->releaseRequestPduReceived(this, pdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::releaseResponsePduReceived(PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_RELEASE_RESPONSE_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->releaseResponsePduReceived(this, pdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::releaseResponseLocal(RELEASE_RP_CLASS *releaseRp_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_RELEASE_RESPONSE_LOCAL;

	// link the PDU for transmission
	releaseRpM_ptr = releaseRp_ptr;

	// use the underlying state machine classes
	bool result = dulpStateM_ptr->releaseResponseLocal(this);

	// unlink the PDU from the association - allows caller to clean up
	releaseRpM_ptr = NULL;

	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::abortRequestLocal(ABORT_RQ_CLASS *abortRq_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ABORT_REQUEST_LOCAL;

	// link the PDU for transmission
	abortRqM_ptr = abortRq_ptr;

	// use the underlying state machine classes
	bool result = dulpStateM_ptr->abortRequestLocal(this);

	// unlink the PDU from the association - allows caller to clean up
	abortRqM_ptr = NULL;

	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::abortRequestPduReceived(PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ABORT_REQUEST_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->abortRequestPduReceived(this, pdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::transportClosedLocal()

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_TRANSPORT_CLOSED_LOCAL;

	// use the underlying state machine classes
	return dulpStateM_ptr->transportClosedLocal(this);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::artimTimerExpired()

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_ARTIM_TIMER_EXPIRED;

	// use the underlying state machine classes
	return dulpStateM_ptr->artimTimerExpired(this);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::invalidPduReceived(PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_INVALID_PDU_RECEIVED;

	// use the underlying state machine classes
	return dulpStateM_ptr->invalidPduReceived(this, pdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::invalidPduLocal(UNKNOWN_PDU_CLASS *unknownPdu_ptr)

//  DESCRIPTION     : DULP (test) event.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// record event
	eventNumberM = DULP_INVALID_PDU_LOCAL;

	// link the PDU for transmission
	unknownPduM_ptr = unknownPdu_ptr;

	// use the underlying state machine classes
	bool result = dulpStateM_ptr->invalidPduLocal(this);

	// unlink the PDU from the association - allows caller to clean up
	unknownPduM_ptr = NULL;

	return result;
}


//>>===========================================================================

void ASSOCIATION_CLASS::setSupportedPresentationContext(char *abstractSyntaxName_ptr, char *transferSyntaxName_ptr)

//  DESCRIPTION     : Set the supported presentation context using the abstract syntax name
//					: and transfer syntax given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UID_CLASS	abstractSyntaxName(abstractSyntaxName_ptr);
	UID_CLASS	transferSyntaxName(transferSyntaxName_ptr);

	// set the supported presentation context
	supportedPCM.setPresentationContext(abstractSyntaxName, transferSyntaxName);

	// log support
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_NONE, 1, "Added support for \"%s\" SOP Class with TS \"%s\"", abstractSyntaxName_ptr, transferSyntaxName_ptr);
	}
}

//>>===========================================================================

bool ASSOCIATION_CLASS::isSupportedPresentationContext(char *abstractSyntaxName_ptr, char *transferSyntaxName_ptr)

//  DESCRIPTION     : Check if the given presentation context (SOP Class UID) is
//					: in the current list with the given transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UID_CLASS	abstractSyntaxName(abstractSyntaxName_ptr);
    UID_CLASS   transferSyntaxName(transferSyntaxName_ptr);
	int index = 0;

	// check if the presentation context is supported (set up already)
    index = supportedPCM.isPresentationContext(abstractSyntaxName, transferSyntaxName);
	if (index != -1)
	{
		// the presentation context is already set up
		return true;
	}
	return false;
}

//>>===========================================================================

int ASSOCIATION_CLASS::getAcceptedPresentationContextId(char *abstractSyntaxName_ptr, char *transferSyntaxName_ptr)

//  DESCRIPTION     : Get the presentation context id for SOP Class UID and transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UID_CLASS	abstractSyntaxName(abstractSyntaxName_ptr);
    UID_CLASS   transferSyntaxName(transferSyntaxName_ptr);

	// return the presentation context id. -1 returned if not supported
    return acceptedPCM.getPresentationContextIdWithTransferSyntax(abstractSyntaxName, transferSyntaxName);
}

//>>===========================================================================

void ASSOCIATION_CLASS::addRqPresentationContexts(ASSOCIATE_RQ_CLASS *associateRq_ptr)

//  DESCRIPTION     : Set up the requested presentation contexts from those supported.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through the supported presentation contexts
	for (int i = 0; i < supportedPCM.noPresentationContexts(); i++)
	{
		UID_CLASS abstractSyntaxName;
		UID_CLASS transferSyntaxName;
		
		// get the supported presentation contexts
		if (supportedPCM.getPresentationContext(i, abstractSyntaxName, transferSyntaxName))
		{
			// set up the requested presentation context
			PRESENTATION_CONTEXT_RQ_CLASS	presentationContextRq;
			presentationContextRq.setPresentationContextId(generatePCID());
			presentationContextRq.setAbstractSyntaxName(abstractSyntaxName);
			presentationContextRq.addTransferSyntaxName(transferSyntaxName);

			// add the requested presentation context
			associateRq_ptr->addPresentationContext(presentationContextRq);
		}
	}
}


//>>===========================================================================

void ASSOCIATION_CLASS::addAcPresentationContexts(ASSOCIATE_RQ_CLASS *associateRq_ptr, ASSOCIATE_AC_CLASS *associateAc_ptr)

//  DESCRIPTION     : Indicate which of the requested presentation contexts are
//					  supported from those supported.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through the associate request
	for (UINT i = 0; i < associateRq_ptr->noPresentationContexts(); i++)
	{
		// get the requested presentation context details
		PRESENTATION_CONTEXT_RQ_CLASS presentationContextRq = associateRq_ptr->getPresentationContext(i);

		BYTE pcId = presentationContextRq.getPresentationContextId();
		UID_CLASS abstractSyntaxName = presentationContextRq.getAbstractSyntaxName();
		UID_CLASS transferSyntaxName;
		int index = 0;

		// loop through the proposed transfer syntaxes to see if any are supported
		for (UINT j = 0; j < presentationContextRq.noTransferSyntaxNames(); j++)
		{
			// get transfer syntax
			transferSyntaxName = presentationContextRq.getTransferSyntaxName(j);

			// check if supported
			index = supportedPCM.isPresentationContext(abstractSyntaxName, transferSyntaxName);
			if (index != -1)
			{
				// we have found a combination that is acceptable
				break;
			}
		}

		// set up the accepted presentation context
		PRESENTATION_CONTEXT_AC_CLASS	presentationContextAc;
		presentationContextAc.setPresentationContextId(pcId);
		presentationContextAc.setAbstractSyntaxName(abstractSyntaxName);

		// check for accepted transfer syntax
		if (index == -1)
		{
			// find out why the presentation context is not supported
			if (supportedPCM.isAbstractSyntaxName(abstractSyntaxName))
			{
				// abstract syntax name is supported with other transfer syntax
				// than that proposed in the associate request
				presentationContextAc.setResultReason(TRANSFER_SYNTAXES_NOT_SUPPORTED);
			}
			else
			{
				// abstract syntax name is not supported with any transfer syntax
				presentationContextAc.setResultReason(ABSTRACT_SYNTAX_NOT_SUPPORTED);
			}

			// reset the transfer syntax
			presentationContextAc.setTransferSyntaxName("");
		}
		else
		{
			// set the result and the accepted transfer syntax
			presentationContextAc.setResultReason(ACCEPTANCE);
			presentationContextAc.setTransferSyntaxName(transferSyntaxName);
		}

		// add the accepted presentation context
		associateAc_ptr->addPresentationContext(presentationContextAc);
	}
}


//>>===========================================================================

bool ASSOCIATION_CLASS::makeAssociation()

//  DESCRIPTION     : Make an association - as SCU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ASSOCIATE_RQ_CLASS associateRq((char*) calledAeTitleM.c_str(), (char*) callingAeTitleM.c_str());

	// cascade the logger
	associateRq.setLogger(loggerM_ptr);

	// add the requested presentation contexts
	addRqPresentationContexts(&associateRq);

	// set up the user information
	associateRq.setMaximumLengthReceived(maximumLengthReceivedM);
	associateRq.setImplementationClassUid((char*) implementationClassUidM.get());
	associateRq.setImplementationVersionName((char*) implementationVersionNameM.c_str());

	// check if any scp & scu role selection defined
	if (scpScuRoleUidM.getLength())
	{
		associateRq.setScpScuRoleSelect(scpScuRoleUidM, scpRoleM, scuRoleM);
	}

	// reset the associated flag
	associatedM = false;

	// try to connect to the remote peer
	if (!connect()) return false;

	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", WIDName(associateRq.getWidType()), timeStamp());
	}

	// request the association
	if (!send(&associateRq))
	{
		close();
		return false;
	}

	// now wait for a response from the peer
	if (receivePdu() != RECEIVE_MSG_SUCCESSFUL)
	{
		close();
		return false;
	}

	// check which PDU we have received
	switch (eventNumberM)
	{
	case DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED:
		// update the accepted Presentation Context list
		acceptedPCM.updateAcceptedPCsOnReceive(associateAcM_ptr);

		// set the maximum length of P-DATA-TF PDU to transmit
		networkTransferM.setMaxTxLength(associateAcM_ptr->getMaximumLengthReceived());

		// indicate that we are associated
		associatedM = true;

		// indicate that the association has been accepted
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(associateAcM_ptr->getWidType()), timeStamp());

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(associateAcM_ptr);
            }

			loggerM_ptr->text(LOG_INFO, 2, "Requested association has been accepted");
		}
		
		// check if any scp & scu role selection defined
		if (scpScuRoleUidM.getLength())
		{
			BYTE scpRole;
			BYTE scuRole;
			
			// check if anything returned for the same SOP Class UID
			if (associateAcM_ptr->getScpScuRoleSelect(scpScuRoleUidM, &scpRole, &scuRole))
			{
				// check that the roles match those requested
				if ((scpRoleM != scpRole) ||
					(scuRoleM != scuRole))
				{
					// nothing returned
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "SCP/SCU Role Selection mis-match for SOP Class UID %s in Associate Accept", (char*) scpScuRoleUidM.get());
						loggerM_ptr->text(LOG_NONE, 1, "Requested ScpRole: %d, ScuRole: %d - Accepted ScpRole %d, ScuRole: %d", scpRoleM, scuRoleM, scpRole, scuRole);
					}
				}
			}
			else
			{
				// nothing returned
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "SCP/SCU Role Selection not present for SOP Class UID %s in Associate Accept", (char*) scpScuRoleUidM.get());
				}
			}
		}
		break;
	case DULP_ASSOCIATE_REJECT_PDU_RECEIVED:
		// indicate that the association has been rejected
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(associateRjM_ptr->getWidType()), timeStamp());

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(associateRjM_ptr);
            }

			loggerM_ptr->text(LOG_ERROR, 2, "Requested association has been rejected");
		}
		break;
	case DULP_ABORT_REQUEST_PDU_RECEIVED:
		// indicate that the association has been aborted
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(abortRqM_ptr->getWidType()), timeStamp());

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(abortRqM_ptr);
            }

			loggerM_ptr->text(LOG_ERROR, 2, "Requested association has been aborted");
		}
		break;
	default:
		// indicate that an unknown PDU has been received
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Unknown PDU received when requesting an association");
		}
		break;
	}

	// clean up any PDUs from the underlying state machine
	cleanup(false);

	// return association acceptance flag
	return associatedM;
}


//>>===========================================================================

bool ASSOCIATION_CLASS::releaseAssociation()

//  DESCRIPTION     : Release the current association - as SCU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RELEASE_RQ_CLASS releaseRq;
	bool normalRelease = false;

	// cascade the logger
	releaseRq.setLogger(loggerM_ptr);

	// check if we are already associated
	if (!associatedM) return false;

	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", WIDName(releaseRq.getWidType()), timeStamp());
	}

	// release the association
	if (!send(&releaseRq))
	{
		close();
		return false;
	}

	// now wait for a response from the peer
	if (receivePdu() != RECEIVE_MSG_SUCCESSFUL)
	{
		close();
		return false;
	}

	// check which PDU we have received
	switch (eventNumberM)
	{
	case DULP_RELEASE_RESPONSE_PDU_RECEIVED:
		// indicate that we are associated
		normalRelease = true;

		// indicate that the association has been released
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(releaseRpM_ptr->getWidType()), timeStamp());

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(releaseRpM_ptr);
            }

			loggerM_ptr->text(LOG_INFO, 2, "Association has been released");
		}
		break;
	case DULP_ABORT_REQUEST_PDU_RECEIVED:
		// indicate that the association has been aborted
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(abortRqM_ptr->getWidType()), timeStamp());

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(abortRqM_ptr);
            }

			loggerM_ptr->text(LOG_ERROR, 2, "Association has been aborted after release request");
		}
		break;
	default:
		// indicate that an unknown PDU has been received
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 2, "Unknown PDU received after release request");
		}
		break;
	}

	// reset the association
	reset();

	// force to unassociated state
	associatedM = false;

	// return release flag
	return normalRelease;
}


//>>===========================================================================

bool ASSOCIATION_CLASS::abortAssociation()

//  DESCRIPTION     : Abort the current association - as SCU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ABORT_RQ_CLASS abortRq(DICOM_UL_SERVICE_USER_INITIATED, REASON_NOT_SPECIFIED);

	// cascade the logger
	abortRq.setLogger(loggerM_ptr);

	// check if we are already associated
	if (!associatedM) return false;

	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", WIDName(abortRq.getWidType()), timeStamp());
	}

	// abort the association
	if (!send(&abortRq))
	{
		close();
		return false;
	}

	// reset the association
	reset();

	// force to unassociated state
	associatedM = false;

	// return successful abortion
	return true;
}


//>>===========================================================================

RECEIVE_MSG_ENUM ASSOCIATION_CLASS::waitForAssociation()

//  DESCRIPTION     : Wait for an association to be established - as SCP.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : We already have a connected socket - we now expect the
//					: peer to send an Associate Request PDU.
//<<===========================================================================
{
	ASSOCIATE_RQ_CLASS *associateRq_ptr;
	BYTE result = REJECTED_PERMANENT;
	BYTE source = DICOM_UL_SERVICE_PROVIDER_ACSE;
	BYTE reason = PROTOCOL_VERSION_NOT_SUPPORTED;

	UINT32 logLevel = (strictMatchM ? LOG_ERROR : LOG_WARNING);

	// we need to move the FSM state to STATE2
	dulpStateM_ptr = STATE2_CLASS::instance();

	// new association, save the start time
	startClockM = clock();

	// once a connection is made we expect to receive an associate request pdu
	if (!receive(&associateRq_ptr)) return RECEIVE_MSG_FAILURE;

	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(associateRq_ptr->getWidType()), timeStamp());

        // serialize it
        if (serializerM_ptr)
        {
            serializerM_ptr->SerializeReceive(associateRq_ptr);
        }
	}

	// we have an associate request - let's check it's contents
	UID_CLASS applicationContextName = associateRq_ptr->getApplicationContextName();

	// indicate that we are associated - this may change...
	associatedM = true;

	// - start with the protocol version
	if (associateRq_ptr->getProtocolVersion() != PROTOCOL_VERSION)
	{
		if (strictMatchM)
		{
			// reject association
			associatedM = false;
			result = REJECTED_PERMANENT;
			source = DICOM_UL_SERVICE_PROVIDER_ACSE;
			reason = PROTOCOL_VERSION_NOT_SUPPORTED;
		}

		// log result
		if (loggerM_ptr)
		{
			loggerM_ptr->text(logLevel, 1, "Association Protocol Version not correct - expected %d - received %d", PROTOCOL_VERSION, associateRq_ptr->getProtocolVersion());
		}
	}

	// check called ae title
	else if (!stringValuesEqual((BYTE*) calledAeTitleM.c_str(), (BYTE*) associateRq_ptr->getCalledAeTitle(), AE_LENGTH, false, false))
	{
		if (strictMatchM)
		{
			// reject association
			associatedM = false;
			result = REJECTED_PERMANENT;
			source = DICOM_UL_SERVICE_USER;
			reason = CALLED_AE_TITLE_NOT_RECOGNISED;
		}

		// log result
		if (loggerM_ptr)
		{
			loggerM_ptr->text(logLevel, 1, "Called AE Title not correct - expected \"%s\" - received \"%s\"", calledAeTitleM.c_str(), associateRq_ptr->getCalledAeTitle());
		}
	}

	// check calling ae title
	else if (!stringValuesEqual((BYTE*) callingAeTitleM.c_str(), (BYTE*) associateRq_ptr->getCallingAeTitle(), AE_LENGTH, false, false))
	{
		if (strictMatchM)
		{
			// reject association
			associatedM = false;
			result = REJECTED_PERMANENT;
			source = DICOM_UL_SERVICE_USER;
			reason = CALLING_AE_TITLE_NOT_RECOGNISED;
		}

		// log result
		if (loggerM_ptr)
		{
			loggerM_ptr->text(logLevel, 1, "Calling AE Title not correct - expected \"%s\" - received \"%s\"", callingAeTitleM.c_str(), associateRq_ptr->getCallingAeTitle());
		}
	}

	// check application context name
	else if (strcmp((char*) applicationContextName.get(), APPLICATION_CONTEXT_NAME) != 0)
	{
		if (strictMatchM)
		{
			// reject association
			associatedM = false;
			result = REJECTED_PERMANENT;
			source = DICOM_UL_SERVICE_USER;
			reason = APPLICATION_CONTEXT_NAME_NOT_SUPPORTED;
		}

		// log result
		if (loggerM_ptr)
		{
			loggerM_ptr->text(logLevel, 1, "Application Context Name not correct - expected \"%s\" - received \"%s\"", APPLICATION_CONTEXT_NAME, (char*) applicationContextName.get());
		}
	}

	// check the presentation context ids - must be odd and unique
	else if (!associateRq_ptr->checkPresentationContextIds())
	{
		// reject association
		associatedM = false;
		result = REJECTED_PERMANENT;
		source = DICOM_UL_SERVICE_USER;
		reason = NO_REASON_GIVEN;
	}

	RECEIVE_MSG_ENUM status = RECEIVE_MSG_SUCCESSFUL;

	// check if the association should be rejected
	if (!associatedM)
	{
		// set up rejection
		ASSOCIATE_RJ_CLASS associateRj(result, source, reason);

		// cascade the logger
		associateRj.setLogger(loggerM_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", WIDName(associateRj.getWidType()), timeStamp());
		}

		// reject the association
		(void) send(&associateRj);

		status = RECEIVE_MSG_ASSOC_REJECTED;
	}
	else
	{
		// so far so good - now check the presentation contexts in detail
		ASSOCIATE_AC_CLASS associateAc(associateRq_ptr->getCalledAeTitle(), associateRq_ptr->getCallingAeTitle());

		// add the accepted presentation contexts
		addAcPresentationContexts(associateRq_ptr, &associateAc);

		// set up the user information
		associateAc.setMaximumLengthReceived(maximumLengthReceivedM);
		associateAc.setImplementationClassUid((char*) implementationClassUidM.get());
		associateAc.setImplementationVersionName((char*) implementationVersionNameM.c_str());

		// Set up the SCU/SCP Role selection
		for (UINT i = 0; i < associateRq_ptr->getUserInformation().noScpScuRoleSelects(); i++)
		{
			SCP_SCU_ROLE_SELECT_CLASS scpScuRoleSelect = associateRq_ptr->getUserInformation().getScpScuRoleSelect(i);
			string scpScuRoleSopUid = (char*)(scpScuRoleSelect.getUid().get());
			if (scpScuRoleSopUid == STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID)
			{
				// give the presentation context id an odd value
				associateAc.setScpScuRoleSelect(scpScuRoleSelect.getUid(), scpScuRoleSelect.getScpRole(), scpScuRoleSelect.getScuRole());
			}
		}

		// cascade the logger
		associateAc.setLogger(loggerM_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", WIDName(associateAc.getWidType()), timeStamp());
		}

		// accept the association
		(void) send(&associateAc);
	}

	// return association status
	return status;
}


//>>===========================================================================

bool ASSOCIATION_CLASS::connect()

//  DESCRIPTION     : Make connection with TCP/IP peer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (loggerM_ptr && socketM_ptr)
	{
		if (socketM_ptr->isSecure())
		{
			loggerM_ptr->text(LOG_INFO, 2, "TCP/IP - Connecting to remote host \"%s\" using secure port number %d ...", socketM_ptr->getRemoteHostname().c_str(), socketM_ptr->getRemoteConnectPort());
		}
		else
		{
			loggerM_ptr->text(LOG_INFO, 2, "TCP/IP - Connecting to remote host \"%s\" using unsecure port number %d ...", socketM_ptr->getRemoteHostname().c_str(), socketM_ptr->getRemoteConnectPort());
		}
	}

	// issue a connect primitive
	return associateRequestLocal();
}

//>>===========================================================================

bool ASSOCIATION_CLASS::listen()

//  DESCRIPTION     : Listen for connection from TCP/IP peer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (loggerM_ptr && socketM_ptr)
	{
		if (socketM_ptr->isSecure())
		{
			loggerM_ptr->text(LOG_INFO, 2, "TCP/IP - Listening for connection using secure port number %d ...", socketM_ptr->getLocalListenPort());
		}
		else
		{
			loggerM_ptr->text(LOG_INFO, 2, "TCP/IP - Listening for connection using unsecure port number %d ...", socketM_ptr->getLocalListenPort());
		}
	}

	// issue a transport indication primitive
	return transportIndicationLocal();
}

//>>===========================================================================

bool ASSOCIATION_CLASS::close()

//  DESCRIPTION     : Close the connection with TCP/IP peer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// issue a transport close primitive
	return transportClosedLocal();
}

//>>===========================================================================

void ASSOCIATION_CLASS::reset()

//  DESCRIPTION     : Reset the association.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// see if association is ending
	if ((dulpStateM_ptr != STATE1_CLASS::instance()) && 
		(dulpStateM_ptr != STATE13_CLASS::instance()))
	{
		// end of association, log the time
		if ((startClockM != 0) && (loggerM_ptr))
		{
			loggerM_ptr->text(LOG_INFO, 1, "Association open %.2f seconds", 
				((double)(clock() - startClockM) / CLOCKS_PER_SEC));
		}
	}

	// force state machine to wait for closure - state 13
	dulpStateM_ptr = STATE13_CLASS::instance();

	// close the connection
	close();

	// clean up any PDUs from the underlying state machine
	cleanup();
}

//>>===========================================================================

bool ASSOCIATION_CLASS::checkForPendingDataInNetworkInputBuffer()

//  DESCRIPTION     : Close the connection with TCP/IP peer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (loggerM_ptr && socketM_ptr)
	{
		return socketM_ptr->isPendingDataInNetworkInputBuffer();
	}
	return false;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::logUnexpectedPdu()

//  DESCRIPTION     : Receive a PDU from peer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (loggerM_ptr)
	{
		switch (eventNumberM)
		{
			case DULP_ASSOCIATE_REQUEST_PDU_RECEIVED:
				if (associateRqM_ptr) 
				{
					// associate request PDU received
					loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected %s - connection is closed.", WIDName(associateRqM_ptr->getWidType()));

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(associateRqM_ptr);
                    }
				}
				break;
	
			case DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED:
				if (associateAcM_ptr) 
				{
					// associate accept PDU received
					loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected %s - connection is closed.", WIDName(associateAcM_ptr->getWidType()));

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(associateAcM_ptr);
                    }
				}
				break;

			case DULP_ASSOCIATE_REJECT_PDU_RECEIVED:
				if (associateRjM_ptr) 
				{
					// associate reject PDU received
					loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected %s - connection is closed.", WIDName(associateRjM_ptr->getWidType()));

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(associateRjM_ptr);
                    }
				}
				break;

			case DULP_DATA_TRANSFER_PDU_RECEIVED:
				// data transfer PDU received
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected DATA TRANSFER PDU - connection is closed.");
				break;

			case DULP_RELEASE_REQUEST_PDU_RECEIVED:
				if (releaseRqM_ptr) 
				{
					// release request PDU received
					loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected association %s - connection is closed.", WIDName(releaseRqM_ptr->getWidType()));

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(releaseRqM_ptr);
                    }
				}
				break;

			case DULP_RELEASE_RESPONSE_PDU_RECEIVED:
				if (releaseRpM_ptr) 
				{
					// release response PDU received
					loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected association %s - connection is closed.", WIDName(releaseRpM_ptr->getWidType()));

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(releaseRpM_ptr);
                    }
				}
				break;

			case DULP_ABORT_REQUEST_PDU_RECEIVED:
				if (abortRqM_ptr) 
				{
					// abort request PDU received
					loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected association %s - connection is closed.", WIDName(abortRqM_ptr->getWidType()));

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(abortRqM_ptr);
                    }
				}
				break;
			case DULP_INVALID_PDU_RECEIVED:
				if (unknownPduM_ptr) 
				{
					// unrecognized/invalid PDU received
					loggerM_ptr->text(LOG_ERROR, 1, "DULP - Received unexpected(invalid) %s - connection is closed.", WIDName(unknownPduM_ptr->getWidType()));
				}
				break;
			case DULP_ASSOCIATE_REQUEST_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to request association.");
				break;
			case DULP_TRANSPORT_CONFIRM_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to confirm transport.");
				break;
			case DULP_TRANSPORT_INDICATION_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to indicate transport.");
				break;
			case DULP_ASSOCIATE_RESPONSE_ACCEPT_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to accept association.");
				break;
			case DULP_ASSOCIATE_RESPONSE_REJECT_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to reject association.");
				break;
			case DULP_DATA_TRANSFER_REQUEST_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to request data transfer.");
				break;
			case DULP_RELEASE_REQUEST_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to release association.");
				break;
			case DULP_RELEASE_RESPONSE_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to respond to association release.");
				break;
			case DULP_ABORT_REQUEST_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to abort association.");
				break;
			case DULP_TRANSPORT_CLOSED_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - trying to close transport.");
				break;
			case DULP_ARTIM_TIMER_EXPIRED:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - ARTIM timer expired.");
				break;
			case DULP_INVALID_PDU_LOCAL:
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - invalid pdu.");
				break;
			default:
				// something else has happened
				loggerM_ptr->text(LOG_ERROR, 1, "DULP - communication failure (internal event %d)", eventNumberM);
				break;
		}
	}

	// clean up all pdu pointers
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
	if (unknownPduM_ptr) 
	{
		delete unknownPduM_ptr;
		unknownPduM_ptr = NULL;
	}

	// release any P-DATA-TF pdus in the Network Transfer
	networkTransferM.cleanup();

	// return failure
	return false;
}

//>>===========================================================================

RECEIVE_MSG_ENUM ASSOCIATION_CLASS::receivePdu()

//  DESCRIPTION     : Receive a PDU from peer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	PDU_CLASS	pdu;
	bool		morePDUs;
	bool		result = true;

	// loop getting at least one PDU
	do
	{
		// reset loop condition
		morePDUs = false;

		// read the PDU Type
		if (!pdu.readType(socketM_ptr))
        {
			// socket failure
			if (loggerM_ptr)
			{
				// log the details
				loggerM_ptr->text(LOG_ERROR, 1, "Can't read from TCP/IP socket - connection has been closed.");
			}
            return RECEIVE_MSG_CONNECTION_CLOSED;
        }

        // check for Data Transfer PDU
        if (pdu.getType() == PDU_PDATA)
        {
	        // allocate a data transfer pdu
	        DATA_TF_PDU_CLASS *dataTfPdu_ptr = new DATA_TF_PDU_CLASS();
	        if (dataTfPdu_ptr == NULL) return RECEIVE_MSG_FAILURE;

	        // set logger
	        dataTfPdu_ptr->setLogger(loggerM_ptr);

	        // read the data transfer pdu
	        if (!dataTfPdu_ptr->readBody(socketM_ptr))
            {
		        // socket failure
		        if (loggerM_ptr)
		        {
			        // log the details
			        loggerM_ptr->text(LOG_ERROR, 1, "Can't read from TCP/IP socket - connection has been closed.");
		        }
                return RECEIVE_MSG_CONNECTION_CLOSED;
            }

    		// check if maximum expected length has been exceeded
	    	if ((loggerM_ptr) &&
		    	(actualMaximumLengthToBeReceivedM) &&
			    (dataTfPdu_ptr->getLength() > actualMaximumLengthToBeReceivedM))
		    {
			    // log the details
			    loggerM_ptr->text(LOG_ERROR, 1, "Maximum DATA-TF PDU length exceeded - expected 0x%X(=%d) - actual 0x%X(=%d)", actualMaximumLengthToBeReceivedM, actualMaximumLengthToBeReceivedM, dataTfPdu_ptr->getLength(), dataTfPdu_ptr->getLength());
		    }

			// log the PDU
		    dataTfPdu_ptr->logRaw(loggerM_ptr);

		    // data transfer PDU received
		    result = dataTransferPduReceived(dataTfPdu_ptr);
		    if (result)
		    {
			    // check if more Data Transfer PDUs expected
			    morePDUs = !dataTfPdu_ptr->isLast();
		    }
	    }
        else
        {
	        // read the pdu
	        if (!pdu.readBody(socketM_ptr))
            {
			    // socket failure
			    if (loggerM_ptr)
			    {
				    // log the details
				    loggerM_ptr->text(LOG_ERROR, 1, "Can't read from TCP/IP socket - connection has been closed.");
			    }
                return RECEIVE_MSG_CONNECTION_CLOSED;
            }

            // log the PDU
		    pdu.logRaw(loggerM_ptr);

	    	switch (pdu.getType())
		    {
			    case PDU_ASSOCIATE_RQ:
		    		// associate request PDU received
	    			result = associateRequestPduReceived(&pdu);
    				break;

			    case PDU_ASSOCIATE_AC:
		    		// associate accept PDU received
	    			result = associateAcceptPduReceived(&pdu);
    				break;

			    case PDU_ASSOCIATE_RJ:
		    		// associate reject PDU received
	    			result = associateRejectPduReceived(&pdu);
    				break;

			    case PDU_RELEASE_RQ:
		    		// release request PDU received
	    			result = releaseRequestPduReceived(&pdu);
    				break;

			    case PDU_RELEASE_RP:
			    	// release response PDU received
		    		result = releaseResponsePduReceived(&pdu);
	    			break;

    			case PDU_ABORT_RQ:
			    	// abort request PDU received
		    		result = abortRequestPduReceived(&pdu);
	    			break;

    			default:
			    	// unrecognized/invalid PDU received
		    		result = invalidPduReceived(&pdu);
		    		break;
	    	}
        }
	} while ((morePDUs) && (result));

	// return status
    RECEIVE_MSG_ENUM status = (result) ? RECEIVE_MSG_SUCCESSFUL : RECEIVE_MSG_FAILURE;
	return status;
}

//>>===========================================================================

ASSOCIATE_RQ_CLASS *ASSOCIATION_CLASS::processReceivedAssociateRqPdu()

//  DESCRIPTION     : Process the received Associate Request Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ASSOCIATE_RQ_CLASS *associateRq_ptr = NULL;

	// check that we have a valid PDU
	if (associateRqM_ptr)
	{
		// initialise the accepted Presentation Context list
		acceptedPCM.initialiseAcceptedPCs(associateRqM_ptr);

		// set the maximum length of P-DATA-TF PDU to transmit
		networkTransferM.setMaxTxLength(associateRqM_ptr->getMaximumLengthReceived());

		// save the address
		associateRq_ptr = associateRqM_ptr;

		// unlink the PDU from the association - allows caller to clean up
		associateRqM_ptr = NULL;
	}

	// return associate request
	return associateRq_ptr;
}

//>>===========================================================================

ASSOCIATE_AC_CLASS *ASSOCIATION_CLASS::processReceivedAssociateAcPdu()

//  DESCRIPTION     : Process the received Associate Accept Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ASSOCIATE_AC_CLASS *associateAc_ptr = NULL;

	// check that we have a valid PDU
	if (associateAcM_ptr)
	{
		// update the accepted Presentation Context list
		acceptedPCM.updateAcceptedPCsOnReceive(associateAcM_ptr);

		// set the maximum length of P-DATA-TF PDU to transmit
		networkTransferM.setMaxTxLength(associateAcM_ptr->getMaximumLengthReceived());

		// save the address
		associateAc_ptr = associateAcM_ptr;

		// unlink the PDU from the association - allows caller to clean up
		associateAcM_ptr = NULL;
	}

	// return associate accept
	return associateAc_ptr;
}

//>>===========================================================================

ASSOCIATE_RJ_CLASS *ASSOCIATION_CLASS::processReceivedAssociateRjPdu()

//  DESCRIPTION     : Process the received Associate Reject Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ASSOCIATE_RJ_CLASS *associateRj_ptr = NULL;

	// check that we have a valid PDU
	if (associateRjM_ptr)
	{
		// save the address
		associateRj_ptr = associateRjM_ptr;

		// unlink the PDU from the association - allows caller to clean up
		associateRjM_ptr = NULL;

		// close connection
		close();
	}

	// return associate reject
	return associateRj_ptr;
}

//>>===========================================================================

RELEASE_RQ_CLASS *ASSOCIATION_CLASS::processReceivedReleaseRqPdu()

//  DESCRIPTION     : Process the received Release Request Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RELEASE_RQ_CLASS *releaseRq_ptr = NULL;

	// check that we have a valid PDU
	if (releaseRqM_ptr)
	{		
		// save the address
		releaseRq_ptr = releaseRqM_ptr;

		// unlink the PDU from the association - allows caller to clean up
		releaseRqM_ptr = NULL;
	}

	// return release request
	return releaseRq_ptr;
}

//>>===========================================================================

RELEASE_RP_CLASS *ASSOCIATION_CLASS::processReceivedReleaseRpPdu()

//  DESCRIPTION     : Process the received Release Response Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RELEASE_RP_CLASS *releaseRp_ptr = NULL;

	// check that we have a valid PDU
	if (releaseRpM_ptr)
	{
		// save the address
		releaseRp_ptr = releaseRpM_ptr;

		// unlink the PDU from the association - allows caller to clean up
		releaseRpM_ptr = NULL;

		// close connection
		close();
	}

	// return release request
	return releaseRp_ptr;
}

//>>===========================================================================

ABORT_RQ_CLASS *ASSOCIATION_CLASS::processReceivedAbortRqPdu()

//  DESCRIPTION     : Process the received Abort Request Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ABORT_RQ_CLASS *abortRq_ptr = NULL;

	// check that we have a valid PDU
	if (abortRqM_ptr)
	{
		// save the address
		abortRq_ptr = abortRqM_ptr;

		// unlink the PDU from the association - allows caller to clean up
		abortRqM_ptr = NULL;

		// close connection
		close();
	}

	// return abort request
	return abortRq_ptr;
}

//>>===========================================================================

UNKNOWN_PDU_CLASS *ASSOCIATION_CLASS::processUnknownPdu()

//  DESCRIPTION     : Process the received Unknown Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UNKNOWN_PDU_CLASS *unknownPdu_ptr = NULL;

	// check that we have a valid PDU
	if (unknownPduM_ptr)
	{
		// save the address
		unknownPdu_ptr = unknownPduM_ptr;

		// unlink the PDU from the association - allows caller to clean up
		unknownPduM_ptr = NULL;
	}

	// return unknown pdu
	return unknownPdu_ptr;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::receive(ASSOCIATE_RQ_CLASS **associateRq_ptr_ptr)

//  DESCRIPTION     : Receive Associate Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// receive a PDU
    bool result = (receivePdu() == RECEIVE_MSG_SUCCESSFUL) ? true : false;

	// on success - check that we have the correct PDU
	if ((result)
		&& (eventNumberM == DULP_ASSOCIATE_REQUEST_PDU_RECEIVED))
	{
		// process the pdu
		*associateRq_ptr_ptr = processReceivedAssociateRqPdu();
	}
	else
	{
		// didn't get what we expected
		result = logUnexpectedPdu();
	}

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::receive(ASSOCIATE_AC_CLASS **associateAc_ptr_ptr)

//  DESCRIPTION     : Receive Associate Accept.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// receive a PDU
    bool result = (receivePdu() == RECEIVE_MSG_SUCCESSFUL) ? true : false;

	// on success - check that we have the correct PDU
	if ((result)
		&& (eventNumberM == DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED))
	{
		// process the pdu
		*associateAc_ptr_ptr = processReceivedAssociateAcPdu();
	}
	else
	{
		// didn't get what we expected
		result = logUnexpectedPdu();
	}

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::receive(ASSOCIATE_RJ_CLASS **associateRj_ptr_ptr)

//  DESCRIPTION     : Receive Associate Reject.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// receive a PDU
    bool result = (receivePdu() == RECEIVE_MSG_SUCCESSFUL) ? true : false;

	// on success - check that we have the correct PDU
	if ((result)
		&& (eventNumberM == DULP_ASSOCIATE_REJECT_PDU_RECEIVED))
	{
		// process the pdu
		*associateRj_ptr_ptr = processReceivedAssociateRjPdu();
	}
	else
	{
		// didn't get what we expected
		result = logUnexpectedPdu();
	}

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::receive(RELEASE_RQ_CLASS **releaseRq_ptr_ptr)

//  DESCRIPTION     : Receive Release Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// receive a PDU
    bool result = (receivePdu() == RECEIVE_MSG_SUCCESSFUL) ? true : false;

	// on success - check that we have the correct PDU
	if ((result)
		&& (eventNumberM == DULP_RELEASE_REQUEST_PDU_RECEIVED))
	{
		// process the pdu
		*releaseRq_ptr_ptr = processReceivedReleaseRqPdu();
	}
	else
	{
		// didn't get what we expected
		result = logUnexpectedPdu();
	}

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::receive(RELEASE_RP_CLASS **releaseRp_ptr_ptr)

//  DESCRIPTION     : Receive Release Response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// receive a PDU
    bool result = (receivePdu() == RECEIVE_MSG_SUCCESSFUL) ? true : false;

	// on success - check that we have the correct PDU
	if ((result)
		&& (eventNumberM == DULP_RELEASE_RESPONSE_PDU_RECEIVED))
	{
		// process the pdu
		*releaseRp_ptr_ptr = processReceivedReleaseRpPdu();
	}
	else
	{
		// didn't get what we expected
		result = logUnexpectedPdu();
	}

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::receive(ABORT_RQ_CLASS **abortRq_ptr_ptr)

//  DESCRIPTION     : Receive Abort Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// receive a PDU
    bool result = (receivePdu() == RECEIVE_MSG_SUCCESSFUL) ? true : false;

	// on success - check that we have the correct PDU
	if ((result)
		&& (eventNumberM == DULP_ABORT_REQUEST_PDU_RECEIVED))
	{
		// process the pdu
		*abortRq_ptr_ptr = processReceivedAbortRqPdu();
	}
	else
	{
		// didn't get what we expected
		result = logUnexpectedPdu();
	}

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::receive(UNKNOWN_PDU_CLASS **unknownPdu_ptr_ptr)

//  DESCRIPTION     : Receive Unknown PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// receive a PDU
    bool result = (receivePdu() == RECEIVE_MSG_SUCCESSFUL) ? true : false;

	// on success - check that we have the correct PDU
	if ((result)
		&& (eventNumberM == DULP_INVALID_PDU_RECEIVED))
	{
		// process the pdu
		*unknownPdu_ptr_ptr = processUnknownPdu();
	}
	else
	{
		// didn't get what we expected
		result = logUnexpectedPdu();
	}

	// return result
	return result;
}

//>>===========================================================================

RECEIVE_MSG_ENUM ASSOCIATION_CLASS::receiveCommand(DCM_COMMAND_CLASS **command_ptr_ptr, bool needToGetPduData, bool dimseOnly)

//  DESCRIPTION     : Receive a command over the current association.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : We may already have the PDU data - see needToGetPduData boolean.
//<<===========================================================================
{
	RECEIVE_MSG_ENUM status = RECEIVE_MSG_SUCCESSFUL;

	// check if we need to get the PDU data
	if (needToGetPduData)
	{
		// receive a PDU
		status = receivePdu();
	}

	// on success check that we have the correct PDU
	if ((status == RECEIVE_MSG_SUCCESSFUL) &&
		(eventNumberM == DULP_DATA_TRANSFER_PDU_RECEIVED))
	{
		// initialise the data transfer decode
		if (!networkTransferM.initialiseDecode(false)) return RECEIVE_MSG_FAILURE;

        BYTE receivePcId = networkTransferM.getPresentationContextId();

		// log maximum length received
		if (loggerM_ptr)
		{
			// log the P-DATA-TF pdu details received
			loggerM_ptr->text(LOG_INFO, 2, "Maximum length of Command DATA-TF PDU received (with pcId %d) is 0x%X=%d", receivePcId, networkTransferM.getMaxRxLength(), networkTransferM.getMaxRxLength());
		}

		// set the Transfer Syntax Code to use for decode
		UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);
		networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

		// allocate a new command object
		DCM_COMMAND_CLASS *command_ptr = new DCM_COMMAND_CLASS();

		// cascade the logger
		command_ptr->setLogger(loggerM_ptr);

		// set the UN VR definition look-up flag
		command_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

		// set the EnsureEvenAttributeValueLength flag
		command_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

		// save the received pcId
		command_ptr->setEncodePresentationContextId((BYTE)receivePcId);

		// decode the command over the association - network transfer
		status = (command_ptr->decode(networkTransferM)) ? RECEIVE_MSG_SUCCESSFUL : RECEIVE_MSG_FAILURE;
		if (status == RECEIVE_MSG_SUCCESSFUL)
		{
			// try to identify the command
			UINT16 commandField = CMD_UNKNOWN;
			if (command_ptr->getUSValue(TAG_COMMAND_FIELD, &commandField))
			{
				command_ptr->setCommandId(mapCommandField(commandField));

                // check to see if the send and receive presentation contexts for the command RQ and RSP are the same
                // - first check that we have received a response
                if (commandField & 0x8000)
                {
                    // check if this is a response to the request sent
                    if ((commandRqFieldSentM | 0x8000) == commandField)
                    {
                        // we expect the presentation contexts to be the same
                        if ((receivePcId != pcIdUsedToSendRequestM)
                            && (loggerM_ptr))
                        {
            			    // log the presentation context ids
			                loggerM_ptr->text(LOG_ERROR, 1, "Presentation Context ID mis-match - PCID %d used to send RQ - PCID %d received in RSP", pcIdUsedToSendRequestM, receivePcId);
                        }
                    }
                }
			}

			// save return address
			*command_ptr_ptr = command_ptr;
		}

		// terminate the data transfer decode
		networkTransferM.terminateDecode();
	}
	else
	{
		// check if we are only expecting a Command
		if (dimseOnly)
		{
			// didn't get what we expected
			status = (logUnexpectedPdu()) ? RECEIVE_MSG_SUCCESSFUL : RECEIVE_MSG_FAILURE;
		}
		else
		{
			// handle the incoming PDU more appropriately
			switch (eventNumberM)
			{
			case DULP_RELEASE_REQUEST_PDU_RECEIVED:
				{
					// association has been released by peer
					// - send release response
					RELEASE_RP_CLASS releaseRp;
					
					// cascade the logger
					releaseRp.setLogger(loggerM_ptr);

					// log action
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(releaseRqM_ptr->getWidType()), timeStamp());

                        // serialize it
                        if (serializerM_ptr)
                        {
                            serializerM_ptr->SerializeReceive(releaseRqM_ptr);
                        }

						loggerM_ptr->text(LOG_SCRIPT, 2, "SENT %s (%s)", WIDName(releaseRp.getWidType()), timeStamp());
					}

					// send release
					(void) send(&releaseRp);

					// set status
					status = RECEIVE_MSG_ASSOC_RELEASED;
				}
				break;
			case DULP_ABORT_REQUEST_PDU_RECEIVED:
				// association has been aborted by peer
				// - log action
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(abortRqM_ptr->getWidType()), timeStamp());
				}

				// reset connection
				reset();

				// set status
				status = RECEIVE_MSG_ASSOC_ABORTED;
				break;
			default:
				// didn't get what we expected
				status = (logUnexpectedPdu()) ? RECEIVE_MSG_SUCCESSFUL : RECEIVE_MSG_FAILURE;
				break;		
			}
		}

		// clean up
		cleanup();
	}

	// return status
	return status;
}

//>>===========================================================================

RECEIVE_MSG_ENUM ASSOCIATION_CLASS::receiveDataset(DCM_DATASET_CLASS **dataset_ptr_ptr, bool dimseOnly, bool cStoreRqCmd, AE_SESSION_CLASS*)

//  DESCRIPTION     : Receive a dataset over the current association.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RECEIVE_MSG_ENUM status = RECEIVE_MSG_FAILURE;

	// check to see if any PDU data is left from the Command receive
	if (networkTransferM.isTherePduData())
	{
		// initialise the data transfer decode
		if (!networkTransferM.initialiseDecode(true)) return RECEIVE_MSG_FAILURE;

		// check if we already have the last PDV
		bool isLast;
		bool remainingPdvData = networkTransferM.isRemainingPdvDataInPdu(&isLast);
		if (!remainingPdvData) return RECEIVE_MSG_FAILURE;

		// if we don't have the last PDV - we need to read more PDUs
		if (!isLast)
		{
			// we have some remaining data in the current PDU but not
			// all required data
			// - receive remaining PDU(s)
			// - on success check that we have the correct PDU
			if ((receivePdu() != RECEIVE_MSG_SUCCESSFUL) ||
				(eventNumberM != DULP_DATA_TRANSFER_PDU_RECEIVED))
			{
				return RECEIVE_MSG_FAILURE;
			}
		}
	}
	else
	{
		// receive PDU(s)
		// - on success check that we have the correct PDU
		if ((receivePdu() == RECEIVE_MSG_SUCCESSFUL) &&
			(eventNumberM == DULP_DATA_TRANSFER_PDU_RECEIVED))
		{
			// initialise the data transfer decode
			if (!networkTransferM.initialiseDecode(true)) return RECEIVE_MSG_FAILURE;
		}
		else
		{
			return RECEIVE_MSG_FAILURE;
		}
	}

	// use the PDV data returned
	if (eventNumberM == DULP_DATA_TRANSFER_PDU_RECEIVED)
	{
		// get the current Presentation Context Id
		BYTE pcId = networkTransferM.getPresentationContextId();

		// log maximum length received
		if (loggerM_ptr)
		{
			// log the P-DATA-TF pdu details received
			loggerM_ptr->text(LOG_INFO, 1, "Maximum length of Dataset DATA-TF PDU received (with pcId %d) is 0x%X=%d", pcId, networkTransferM.getMaxRxLength(), networkTransferM.getMaxRxLength());
		}

		// check if Presentation Context is accepted
		UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);
		if (acceptedPCM.getTransferSyntaxUid(pcId, transferSyntaxUid))
		{
			// set the Transfer Syntax Code to use for decode
			networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

			// check if we should serialise the dataset
			if (networkTransferM.getStorageMode() != SM_NO_STORAGE)
			{
				string	filename;
				bool	appendToFile;

				// serialise the dataset
				if(storeCSTOREObjectsM)
				{
					//Only store C-Store-Rq objects
					if(cStoreRqCmd)
					{
						// check storage mode to see if we should include a media header or not
						if ((networkTransferM.getStorageMode() == SM_AS_MEDIA) ||
							(networkTransferM.getStorageMode() == SM_AS_MEDIA_ONLY))
						{
							// set up the media header
							MEDIA_FILE_HEADER_CLASS *mediaHeader_ptr = new MEDIA_FILE_HEADER_CLASS(networkTransferM.getSessionId(), sopClassUidM, sopInstanceUidM, (char*) transferSyntaxUid.get(), loggerM_ptr);

							// write the media header
							if (mediaHeader_ptr->write())
							{
								if (loggerM_ptr)
								{
									loggerM_ptr->text(LOG_DEBUG, 1, "Generating Media Storage File: - %s", mediaHeader_ptr->getFilename());
									loggerM_ptr->text(LOG_MEDIA_FILENAME, 1, "%s", mediaHeader_ptr->getFilename());
								}
							}
							else 
							{
								if (loggerM_ptr)
								{
									loggerM_ptr->text(LOG_INFO, 1, "Failed to generate Media Storage File: - %s", mediaHeader_ptr->getFilename());
								}
							}

							// append the dataset to the header
							appendToFile = true;
							filename = mediaHeader_ptr->getFilename();

							// cleanup
							delete mediaHeader_ptr;
						}
						else
						{
							// generate a filename for the raw storage
							appendToFile = false;
							string storageRoot;
							if (loggerM_ptr)
							{
								// get the storage root
								storageRoot = loggerM_ptr->getStorageRoot();
							}
							getStorageFilename(storageRoot, networkTransferM.getSessionId(), filename, SFE_DOT_RAW);
							if (loggerM_ptr)
							{
								// log filename used for RAW dataset storage
								loggerM_ptr->text(LOG_INFO, 1, "Generating Storage Dataset File: - %s", filename.c_str());
							}
						}

						if (!networkTransferM.serialise(filename, appendToFile))
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_ERROR, 1, "Failed to store Dataset in File: - %s", filename.c_str());
							}

							// return error
							return RECEIVE_MSG_FAILURE;
						}
					}
				}
				else
				{
					// check storage mode to see if we should include a media header or not
					if ((networkTransferM.getStorageMode() == SM_AS_MEDIA) ||
						(networkTransferM.getStorageMode() == SM_AS_MEDIA_ONLY))
					{
						// set up the media header
						MEDIA_FILE_HEADER_CLASS *mediaHeader_ptr = new MEDIA_FILE_HEADER_CLASS(networkTransferM.getSessionId(), sopClassUidM, sopInstanceUidM, (char*) transferSyntaxUid.get(), loggerM_ptr);

						// write the media header
						if (mediaHeader_ptr->write())
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_DEBUG, 1, "Generating Media Storage File: - %s", mediaHeader_ptr->getFilename());
								loggerM_ptr->text(LOG_MEDIA_FILENAME, 1, "%s", mediaHeader_ptr->getFilename());
							}
						}
						else 
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_INFO, 1, "Failed to generate Media Storage File: - %s", mediaHeader_ptr->getFilename());
							}
						}

						// append the dataset to the header
						appendToFile = true;
						filename = mediaHeader_ptr->getFilename();

						// cleanup
						delete mediaHeader_ptr;
					}
					else
					{
						// generate a filename for the raw storage
						appendToFile = false;
						string storageRoot;
						if (loggerM_ptr)
						{
							// get the storage root
							storageRoot = loggerM_ptr->getStorageRoot();
						}
						getStorageFilename(storageRoot, networkTransferM.getSessionId(), filename, SFE_DOT_RAW);
						if (loggerM_ptr)
						{
							// log filename used for RAW dataset storage
							loggerM_ptr->text(LOG_INFO, 1, "Generating Storage Dataset File: - %s", filename.c_str());
						}
					}

					if (!networkTransferM.serialise(filename, appendToFile))
					{
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_ERROR, 1, "Failed to store Dataset in File: - %s", filename.c_str());
						}

						// return error
						return RECEIVE_MSG_FAILURE;
					}
				}
			}
			
			// allocate a new dataset object
			DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

			// cascade the logger
			dataset_ptr->setLogger(loggerM_ptr);

			// set the UN VR definition look-up flag
			dataset_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

			// set the EnsureEvenAttributeValueLength flag
			dataset_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

			// save the received pcId
			dataset_ptr->setEncodePresentationContextId((BYTE)pcId);

			// decode the dataset over the association - network transfer
			status = (dataset_ptr->decode(networkTransferM)) ? RECEIVE_MSG_SUCCESSFUL : RECEIVE_MSG_FAILURE;
			if (status == RECEIVE_MSG_SUCCESSFUL)
			{
				// save return address
				*dataset_ptr_ptr = dataset_ptr;
			}

			// terminate the data transfer decode
			networkTransferM.terminateDecode();
		}
		else
		{
			// error - can't find Presentation Context Id is accepted list
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Cannot find Presentation Context ID of %d in Accepted list", pcId);
			}
		}
	}
	else
	{
		// check if we are only expecting a Dataset
		if (dimseOnly)
		{
			// didn't get what we expected
			status = (logUnexpectedPdu()) ? RECEIVE_MSG_SUCCESSFUL : RECEIVE_MSG_FAILURE;
		}
		else
		{
			// handle the incoming PDU more appropriately
			switch (eventNumberM)
			{
			case DULP_ABORT_REQUEST_PDU_RECEIVED:
				// association has been aborted by peer
				// - log action
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVED %s (%s)", WIDName(abortRqM_ptr->getWidType()), timeStamp());
				}

				// reset connection
				reset();

				// set status
				status = RECEIVE_MSG_ASSOC_ABORTED;
				break;
			default:
				// didn't get what we expected
				status = (logUnexpectedPdu()) ? RECEIVE_MSG_SUCCESSFUL : RECEIVE_MSG_FAILURE;
				break;		
			}
		}

		// clean up
		cleanup();
	}

	// return status
	return status;
}

//>>===========================================================================

RECEIVE_MSG_ENUM ASSOCIATION_CLASS::receiveCommandDataset(DCM_COMMAND_CLASS **command_ptr_ptr, DCM_DATASET_CLASS **dataset_ptr_ptr, AE_SESSION_CLASS *ae_session_ptr, bool needToGetPduData, bool dimseOnly)

//  DESCRIPTION     : Receive DICOM command and dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RECEIVE_MSG_ENUM status = RECEIVE_MSG_FAILURE;
	bool isCStoreCmd = false;

	// initialise return values
	*command_ptr_ptr = NULL;
	*dataset_ptr_ptr = NULL;

	// try to get the DICOM command
	if ((status = receiveCommand(command_ptr_ptr, needToGetPduData, dimseOnly)) != RECEIVE_MSG_SUCCESSFUL)
	{
		// failed to decode the DICOM command
		if ((dimseOnly) &&
			(loggerM_ptr))
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to import DICOM command");
		}

		// return status
		return status;
	}

	// check if command field attribute is available
	UINT16 commandField = CMD_UNKNOWN;
	if (!(*command_ptr_ptr)->getUSValue(TAG_COMMAND_FIELD, &commandField))
	{
		// no command field attribute present - can't continue
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "No Command Field (0000,0100) attribute value present in DICOM command");

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(*command_ptr_ptr, NULL);
            }
		}
	}

	// check if data set type attribute is available
	UINT16 dataSetType = NO_DATA_SET;
	if (!(*command_ptr_ptr)->getUSValue(TAG_DATA_SET_TYPE, &dataSetType))
	{
		// no data set type attribute present - can't continue
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "No Data Set Type (0000,0800) attribute value present in DICOM command");

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(*command_ptr_ptr, NULL);
            }
		}

		return RECEIVE_MSG_FAILURE;
	}

	// check if SOP Class UID attribute is available - try both affected and requested
	// sop class uids - the command may be validated later to ensuer the correct one defined
	if (!(*command_ptr_ptr)->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM)) 
	{
		if (!(*command_ptr_ptr)->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM))
		{
			// SOP Class UID not available
			sopClassUidM = "";
		}
	}

	// check if SOP Instance UID attribute is available - try both affected and requested
	// sop instance uids - the command may be validated later to ensure the correct one defined
	if (!(*command_ptr_ptr)->getUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUidM)) 
	{
		if (!(*command_ptr_ptr)->getUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUidM))
		{
			// SOP Instance UID not available
			sopInstanceUidM = "";
		}
	}

	// is this a request ?
	if (!(commandField & 0x8000)) 
	{
		// get the message id attribute
		if ((*command_ptr_ptr)->getUSValue(TAG_MESSAGE_ID, &messageIdM))
		{ 
			// save message id for possible response
			messageIdBeingRespondedToM = messageIdM;
		}
	}

	// is this a C-Store-Rq
	if(commandField == C_STORE_RQ)
		isCStoreCmd = true;

	// check if a dataset is present
	if (dataSetType != NO_DATA_SET)
	{
		// try to decode dataset
		if ((status = receiveDataset(dataset_ptr_ptr, dimseOnly, isCStoreCmd, ae_session_ptr)) != RECEIVE_MSG_SUCCESSFUL)
		{
			// failed to decode the DICOM dataset
			if ((dimseOnly) &&
				(loggerM_ptr))
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to import DICOM dataset");
			}

			return status;
		}
	}

	// return status
	return RECEIVE_MSG_SUCCESSFUL;
}

//>>===========================================================================

RECEIVE_MSG_ENUM ASSOCIATION_CLASS::receive(RECEIVE_MESSAGE_UNION_CLASS **receiveMessageUnion_ptr_ptr, AE_SESSION_CLASS *ae_session_ptr)

//  DESCRIPTION     : Receive any kind of message across the associaton.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The message type must be read from the union in order to determine
//					: the actual message received.
//<<===========================================================================
{
	// receive the PDU(s)
    RECEIVE_MSG_ENUM status = receivePdu();
	if (status != RECEIVE_MSG_SUCCESSFUL) return status;

	// instantiate the return union class
	RECEIVE_MESSAGE_UNION_CLASS *receiveMessageUnion_ptr = new RECEIVE_MESSAGE_UNION_CLASS();

	// successful receive - switch on event number
	switch(eventNumberM)
	{
		case DULP_ASSOCIATE_REQUEST_PDU_RECEIVED:
            {
			    // received associate request
                ASSOCIATE_RQ_CLASS *associateRq_ptr = processReceivedAssociateRqPdu();
			    receiveMessageUnion_ptr->setAssociateRequest(associateRq_ptr);

                // serialize it
                if (serializerM_ptr)
                {
                    serializerM_ptr->SerializeReceive(associateRq_ptr);
                }
            }
			break;

		case DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED:
            {
			    // received associate accept
                ASSOCIATE_AC_CLASS *associateAc_ptr = processReceivedAssociateAcPdu();
                receiveMessageUnion_ptr->setAssociateAccept(associateAc_ptr);

                // serialize it
                if (serializerM_ptr)
                {
                    serializerM_ptr->SerializeReceive(associateAc_ptr);
                }
            }
			break;

		case DULP_ASSOCIATE_REJECT_PDU_RECEIVED:
            {
			    // received associate reject
                ASSOCIATE_RJ_CLASS *associateRj_ptr = processReceivedAssociateRjPdu();
			    receiveMessageUnion_ptr->setAssociateReject(associateRj_ptr);

                // serialize it
                if (serializerM_ptr)
                {
                    serializerM_ptr->SerializeReceive(associateRj_ptr);
                }
            }
            break;

		case DULP_RELEASE_REQUEST_PDU_RECEIVED:
            {
			    // received release request
                RELEASE_RQ_CLASS *releaseRq_ptr = processReceivedReleaseRqPdu();
			    receiveMessageUnion_ptr->setReleaseRequest(releaseRq_ptr);

                // serialize it
                if (serializerM_ptr)
                {
                    serializerM_ptr->SerializeReceive(releaseRq_ptr);
                }
            }
            break;

		case DULP_RELEASE_RESPONSE_PDU_RECEIVED:
            {
			    // received release response
                RELEASE_RP_CLASS *releaseRp_ptr = processReceivedReleaseRpPdu();
			    receiveMessageUnion_ptr->setReleaseResponse(releaseRp_ptr);

                // serialize it
                if (serializerM_ptr)
                {
                    serializerM_ptr->SerializeReceive(releaseRp_ptr);
                }
            }
            break;

		case DULP_ABORT_REQUEST_PDU_RECEIVED:
            {
			    // received abort request
                ABORT_RQ_CLASS *abortRq_ptr = processReceivedAbortRqPdu();
			    receiveMessageUnion_ptr->setAbortRequest(abortRq_ptr);

                // serialize it
                if (serializerM_ptr)
                {
                    serializerM_ptr->SerializeReceive(abortRq_ptr);
                }
            }
            break;

		case DULP_DATA_TRANSFER_PDU_RECEIVED:
			{
				// received data transfer pdu
				// could be command only
				// or command and dataset
				DCM_COMMAND_CLASS *command_ptr = NULL; 
				DCM_DATASET_CLASS *dataset_ptr = NULL;
				status = receiveCommandDataset(&command_ptr, &dataset_ptr, ae_session_ptr, false, false);
				if (status == RECEIVE_MSG_SUCCESSFUL)
				{
					// received command [and dataset]
					receiveMessageUnion_ptr->setCommandDataset(command_ptr, dataset_ptr);

                    // serialize it
                    if (serializerM_ptr)
                    {
                        serializerM_ptr->SerializeReceive(command_ptr, dataset_ptr);
                    }
				}
				else
				{
					// decode failure
					receiveMessageUnion_ptr->setFailure();
				}
			}
			break;

		default:
			// receive failure
			receiveMessageUnion_ptr->setFailure();
			status = RECEIVE_MSG_FAILURE;
			break;
	}

	// return the union
	*receiveMessageUnion_ptr_ptr = receiveMessageUnion_ptr;

	// return status
	return status;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(ASSOCIATE_RQ_CLASS *associateRq_ptr)

//  DESCRIPTION     : Send Associate Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateRq_ptr == NULL) return false;

	// Reset the PCs counter
	pc_idM = 0;

	// ensure that the presentation context id is defined
	for (UINT i = 0; i < associateRq_ptr->noPresentationContexts(); i++)
	{
		if (associateRq_ptr->getPresentationContext(i).getPresentationContextId() == 0)
		{
			// give the presentation context id an odd value
			associateRq_ptr->getPresentationContext(i).setPresentationContextId(generatePCID());
		}
	}

	// initialise the accepted Presentation Context list
	acceptedPCM.initialiseAcceptedPCs(associateRq_ptr);

	// save the actual maximum length of P-DATA-TF PDU that we wish to receive
	actualMaximumLengthToBeReceivedM = associateRq_ptr->getMaximumLengthReceived();

	// sort the presentation contexts
	associateRq_ptr->sortPresentationContexts();

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(associateRq_ptr);
    }

	// issue an associate request primitive
	return transportConfirmLocal(associateRq_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(ASSOCIATE_AC_CLASS *associateAc_ptr)

//  DESCRIPTION     : Send Associate Accept.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateAc_ptr == NULL) return false;

	// update the accepted Presentation Context list
	acceptedPCM.updateAcceptedPCsOnSend(associateAc_ptr);

	// save the actual maximum length of P-DATA-TF PDU that we wish to receive
	actualMaximumLengthToBeReceivedM = associateAc_ptr->getMaximumLengthReceived();

	// sort the presentation contexts
	associateAc_ptr->sortPresentationContexts();

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(associateAc_ptr);
    }

	// issue an associate response accept primitive
	return associateResponseAcceptLocal(associateAc_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(ASSOCIATE_RJ_CLASS *associateRj_ptr)

//  DESCRIPTION     : Send Associate Reject.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateRj_ptr == NULL) return false;

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(associateRj_ptr);
    }

	// issue an associate response reject primitive
	bool result = associateResponseRejectLocal(associateRj_ptr);

	// close connection
	close();

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(RELEASE_RQ_CLASS *releaseRq_ptr)

//  DESCRIPTION     : Send Release Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (releaseRq_ptr == NULL) return false;

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(releaseRq_ptr);
    }

	// issue a release request primitive
	return releaseRequestLocal(releaseRq_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(RELEASE_RP_CLASS *releaseRp_ptr)

//  DESCRIPTION     : Send Release Response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (releaseRp_ptr == NULL) return false;

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(releaseRp_ptr);
    }

	// issue a release response primitive
	bool result = releaseResponseLocal(releaseRp_ptr);

	// close connection
	close();

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(ABORT_RQ_CLASS *abortRq_ptr)

//  DESCRIPTION     : Send Abort Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (abortRq_ptr == NULL) return false;

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(abortRq_ptr);
    }

	// issue an abort request primitive
	bool result = abortRequestLocal(abortRq_ptr);

	// close connection
	close();

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(UNKNOWN_PDU_CLASS *unknownPdu_ptr)

//  DESCRIPTION     : Send Unknown PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (unknownPdu_ptr == NULL) return false;

	// issue an "unknown" request primitive
	return invalidPduLocal(unknownPdu_ptr);
}

//>>===========================================================================

bool ASSOCIATION_CLASS::sendCommand(DCM_COMMAND_CLASS *command_ptr, UINT16 dataSetType)

//  DESCRIPTION     : Send DICOM Command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// build in C-ECHO-RQ handling
	if (command_ptr->getCommandId() == DIMSE_CMD_CECHO_RQ) 
	{
		// initialise SOP Class UID
		sopClassUidM = VERIFICATION_SOP_CLASS_UID;
	}

	// complete any undefined command attributes
	completeCommandOnSend(command_ptr, dataSetType);

	// initialise the data transfer encode
	if (!networkTransferM.initialiseEncode())
	{
		return false;
	}

    // get the Command Field
	UINT16 commandField = 0;
	command_ptr->getUSValue(TAG_COMMAND_FIELD, &commandField);

	// check if presentation context id provided by in DICOMScript or Emulator
	BYTE pcId = command_ptr->getEncodePresentationContextId();
	if (pcId == 0)
	{
		// check if dealing with a C-CANCEL-RQ
		// - this does not have an Affected or Requested SOP Class UID so we
		// have to set the pcId to 1
		if (commandField == C_CANCEL_RQ) 
		{
			// fix this to first pcId
			pcId = 1;
		}
		else 
		{
			// get the Presentation Context Id from the SOP Class UID
			if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM) &&
				!command_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM))
			{
				// can't encode without the SOP Class UID
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Can't find SOP Class UID in Command");
				}
				return false;
			}

			UID_CLASS uid;
			uid.set((char*) sopClassUidM.c_str());
			pcId = acceptedPCM.getPresentationContextId(uid);

			// check if we have a presentation context id
			if (pcId == 0) 
			{
				// can't find the transfer syntax in accepted list - continue with default
				if (loggerM_ptr)
				{
                    if (sopClassUidM.length() == 0)
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Can't find Requested/Affected SOP Class UID in Command. Don't know how to lookup Accepted Presentation Context in Association.");
						loggerM_ptr->text(LOG_INFO, 1, "Add the appropriate Requested/Affected SOP Class UID to the Command.");
                    }
                    else
                    {
					    loggerM_ptr->text(LOG_ERROR, 1, "Can't find Accepted Presentation Context in Association for SOP Class \"%s\". Has the PC been negotiated successfully?", sopClassUidM.c_str());
					    if (!isUID((char*) sopClassUidM.c_str()))
					    {
						    loggerM_ptr->text(LOG_INFO, 1, "The SOP Class UID string is not translated into a real UID. Has the required Definition File been loaded?");
					    }
                    }
				}
				return false;
			}
		}
	}

    // save the Command RQ type sent and the Presentation Context ID used
    // - this is tested in the command receive to ensure that the same PCID is used when
    // receiving the RSP matching this RQ
    commandRqFieldSentM = 0;
    pcIdUsedToSendRequestM = 0;
    if (!(commandField & 0x8000))
    {
        commandRqFieldSentM = commandField;
        pcIdUsedToSendRequestM = pcId;
    }

	// set the Presentation Context Id
	networkTransferM.setPresentationContextId(pcId);
	networkTransferM.setIsCommandContent(true);

	// set the Transfer Syntax Code to use for encode
	UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);
	networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

	// set the EnsureEvenAttributeValueLength flag
	command_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

	// encode the command to the data transfer
	if (!command_ptr->encode(networkTransferM))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to send Command");
		}
		return false;
	}

	// terminate the data transfer encode
	if (!networkTransferM.terminateEncode())
	{
		return false;
	}

	// send the data transfer requests
	bool result = dataTransferRequestLocal();

	// cleanup the data transfer
	networkTransferM.cleanup();

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::sendCommand(DCM_COMMAND_CLASS *command_ptr, BYTE pcId, UINT16 dataSetType)

//  DESCRIPTION     : Send DICOM Command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// complete any undefined command attributes
	completeCommandOnSend(command_ptr, dataSetType);

	// initialise the data transfer encode
	if (!networkTransferM.initialiseEncode())
	{
		return false;
	}

    // get the Command Field
	UINT16 commandField = 0;
	command_ptr->getUSValue(TAG_COMMAND_FIELD, &commandField);

	// save the Command RQ type sent and the Presentation Context ID used
    // - this is tested in the command receive to ensure that the same PCID is used when
    // receiving the RSP matching this RQ
    commandRqFieldSentM = 0;
    pcIdUsedToSendRequestM = 0;
    if (!(commandField & 0x8000))
    {
        commandRqFieldSentM = commandField;
        pcIdUsedToSendRequestM = pcId;
    }

	// set the Presentation Context Id
	networkTransferM.setPresentationContextId(pcId);
	networkTransferM.setIsCommandContent(true);

	// set the Transfer Syntax Code to use for encode
	UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);
	networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

	// set the EnsureEvenAttributeValueLength flag
	command_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

	// encode the command to the data transfer
	if (!command_ptr->encode(networkTransferM))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to send Command");
		}
		return false;
	}

	// terminate the data transfer encode
	if (!networkTransferM.terminateEncode())
	{
		return false;
	}

	// send the data transfer requests
	bool result = dataTransferRequestLocal();

	// cleanup the data transfer
	networkTransferM.cleanup();

	// return result
	return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::sendDataset(DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Send DICOM Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise the data transfer encode
	if (!networkTransferM.initialiseEncode())
	{
		return false;
	}

	// check if presentation context id provided by in DICOMScript or Emulator
	BYTE pcId = dataset_ptr->getEncodePresentationContextId();
	if (pcId == 0)
	{
		// get the Presentation Context Id from the SOP Class UID
		UID_CLASS uid;
		uid.set((char*) sopClassUidM.c_str());
		pcId = acceptedPCM.getPresentationContextId(uid);
	}
	networkTransferM.setPresentationContextId(pcId);
	networkTransferM.setIsCommandContent(false);

	// set the Transfer Syntax Code to use for encode
	UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);

	if (!acceptedPCM.getTransferSyntaxUid(pcId, transferSyntaxUid))
	{
		// can't find the transfer syntax in accepted list - continue with default
		if ((pcId) &&
			(loggerM_ptr))
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't find accepted Transfer Syntax for PC ID: %d in agreed list", pcId);
		}
		return false;
	}
	networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

	// set the EnsureEvenAttributeValueLength flag
	dataset_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

	// encode the dataset to the data transfer
	if (!dataset_ptr->encode(networkTransferM))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to send Dataset");
		}
		return false;
	}

	// terminate the data transfer encode
	if (!networkTransferM.terminateEncode())
	{
		return false;
	}

	// send the data transfer requests
	bool result = dataTransferRequestLocal();

	// cleanup the data transfer
	networkTransferM.cleanup();

	// return result
	return result;
}


//>>===========================================================================

bool ASSOCIATION_CLASS::send(DCM_COMMAND_CLASS *command_ptr)

//  DESCRIPTION     : Send DICOM Command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool sopClassUidPresentInCommand = true;
	bool sopInstanceUidPresentInCommand = true;

	// check for valid pointer
	if (command_ptr == NULL) return false;

	// get the SOP Class UID
	// first try and get it either from the Command Affected or Requested SOP Class UID
	string lastSopClassUid = sopClassUidM;
	if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM)) 
	{
		if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM))
		{
			// indicate that the SOP Class UID was not originally in the command
			// - we need to remove it after sending the command
			sopClassUidPresentInCommand = false;

			// can't find the SOP Class UID anywhere !
			// - let's use the last one stored
			sopClassUidM = lastSopClassUid;
		}
	}

	// get the SOP Instance UID
	// first try and get it from either the Command Affected or Requested SOP Instance UID
	string lastSopInstanceUid = sopInstanceUidM;
	if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUidM))
	{
		if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUidM))
		{
			// indicate that the SOP Instance UID was not originally in the command
			// - we need to remove it after sending the command
			sopInstanceUidPresentInCommand = false;

			// check specifically for the N-CREATE-RQ
			if (command_ptr->getCommandId() == DIMSE_CMD_NCREATE_RQ)
			{
				// the SOP Instance UID is not specified - that's OK
				sopInstanceUidM = "";
			}
			else
			{
				// can't find the SOP Instance UID anywhere !
				// - let's use the last one stored
				sopInstanceUidM = lastSopInstanceUid;
			}
		}
	}

	// send the DICOM Command
    bool result = sendCommand(command_ptr);

    // serialize it after sending to get the complete command - attributes are added by the sendCommand method
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(command_ptr, NULL);
    }

	// check if we need to remove the SOP Class UID from the command
	// - the command might be used again with a different UID
	if (!sopClassUidPresentInCommand)
	{
		command_ptr->DeleteAttribute(COMMAND_GROUP, AFFECTED_SOP_CLASS_UID);
		command_ptr->DeleteAttribute(COMMAND_GROUP, REQUESTED_SOP_CLASS_UID);
	}

	// check if we need to remove the SOP Instance UID from the command
	// - the command might be used again with a different UID
	if (!sopInstanceUidPresentInCommand)
	{
		command_ptr->DeleteAttribute(COMMAND_GROUP, AFFECTED_SOP_INSTANCE_UID);
		command_ptr->DeleteAttribute(COMMAND_GROUP, REQUESTED_SOP_INSTANCE_UID);
	}

    // return result
    return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(DCM_COMMAND_CLASS *command_ptr, int pcId)

//  DESCRIPTION     : Send DICOM Command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (command_ptr == NULL) return false;

	// get the SOP Class UID
	// first try and get it either from the Command Affected or Requested SOP Class UID
	string lastSopClassUid = sopClassUidM;
	if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM)) 
	{
		if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM))
		{
			// can't find the SOP Class UID anywhere !
			// - let's use the last one stored
			sopClassUidM = lastSopClassUid;
		}
	}

	// get the SOP Instance UID
	// first try and get it from either the Command Affected or Requested SOP Instance UID
	string lastSopInstanceUid = sopInstanceUidM;
	if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUidM))
	{
		if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUidM))
		{
			// check specifically for the N-CREATE-RQ
			if (command_ptr->getCommandId() == DIMSE_CMD_NCREATE_RQ)
			{
				// the SOP Instance UID is not specified - that's OK
				sopInstanceUidM = "";
			}
			else
			{
				// can't find the SOP Instance UID anywhere !
				// - let's use the last one stored
				sopInstanceUidM = lastSopInstanceUid;
			}
		}
	}

	// send the DICOM Command
    bool result = sendCommand(command_ptr, (BYTE)pcId);

    // serialize it after sending to get the complete command - attributes are added by the sendCommand method
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(command_ptr, NULL);
    }

	// return result
    return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Send DICOM Command and Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool sopClassUidPresentInCommand = true;
	bool sopInstanceUidPresentInCommand = true;

    //
    // check for valid pointers
    //
    if ((command_ptr == NULL) || 
		(dataset_ptr == NULL)) return false;

    //
    // get the SOP Class UID
    // first try and get it either from the Command Affected or Requested SOP Class UID
    // then try the Dataset - for storage classes this should be set
    //
    string lastSopClassUid = sopClassUidM;
    if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM)) 
    {
        if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM))
        {
			// indicate that the SOP Class UID was not originally in the command
			// - we need to remove it after sending the command
			sopClassUidPresentInCommand = false;

            if (!dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, sopClassUidM))
            {
                //
                // try to get sop class uid from iod name
                //
                if (dataset_ptr->getIodName())
                {
                    string sopClassUid = DEFINITION->GetSopUid(dataset_ptr->getIodName());
                    if (sopClassUid.length())
                    {
                        //
                        // use value found in definition
                        //
                        sopClassUidM = sopClassUid;
                    }
                    else
                    {
                        //
                        // can't find the SOP Class UID anywhere !
                        // - let's use the last one stored
                        //
                        sopClassUidM = lastSopClassUid;
                    }
                }
                else 
                {
                    //
                    // can't find the SOP Class UID anywhere !
                    // - let's use the last one stored
                    //
                    sopClassUidM = lastSopClassUid;
                }
            }
        }
    }
    //
    // get the SOP Instance UID
    // first try and get it from either the Command Affected or Requested SOP Instance UID
    // finally try the Dataset itself - for storage classes this should be set
    //
    string lastSopInstanceUid = sopInstanceUidM;
    if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUidM))
    {
        if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUidM))
        {
			// indicate that the SOP Instance UID was not originally in the command
			// - we need to remove it after sending the command
			sopInstanceUidPresentInCommand = false;

            //
            // finally try from SOP instance uid in dataset - don't worry if this fails
            // take whatever value is currently in member
            //
            if (!dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, sopInstanceUidM))
            {
                //
                // check specifically for the N-CREATE-RQ
                //
                if (command_ptr->getCommandId() == DIMSE_CMD_NCREATE_RQ)
                {
                    //
                    // the SOP Instance UID is not specified - that's OK
                    //
                    sopInstanceUidM = "";
                }
                else
                {
                    //
                    // can't find the SOP Instance UID anywhere !
                    // - let's use the last one stored
                    //
                    sopInstanceUidM = lastSopInstanceUid;
                }
            }
        }
    }

    //
    // Apply proper datasettype tag.
    //
    UINT16 datasettype;
    //
    // Either: Apply value as supplied in command set
    //
    bool datasettypePresent = command_ptr->getUSValue(TAG_DATA_SET_TYPE, &datasettype);
    //
    // Or: Apply value as predefined by dicom standard
    //
    if (!datasettypePresent)
    {
        datasettype = (dataset_ptr == NULL) ? NO_DATA_SET : DATA_SET;
    }
    //
    // send the DICOM command
    //
    bool result = sendCommand(command_ptr, datasettype);
    //
    // send the DICOM dataset
    //
    if (result)
    {
        result = sendDataset(dataset_ptr);
    }
    //
    // serialize it after sending to get the complete command - 
    // attributes are added by the sendCommand method
    //
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(command_ptr, dataset_ptr);
    }

	// check if we need to remove the SOP Class UID from the command
	// - the command might be used again with a different UID
	if (!sopClassUidPresentInCommand)
	{
		command_ptr->DeleteAttribute(COMMAND_GROUP, AFFECTED_SOP_CLASS_UID);
		command_ptr->DeleteAttribute(COMMAND_GROUP, REQUESTED_SOP_CLASS_UID);
	}

	// check if we need to remove the SOP Instance UID from the command
	// - the command might be used again with a different UID
	if (!sopInstanceUidPresentInCommand)
	{
		command_ptr->DeleteAttribute(COMMAND_GROUP, AFFECTED_SOP_INSTANCE_UID);
		command_ptr->DeleteAttribute(COMMAND_GROUP, REQUESTED_SOP_INSTANCE_UID);
	}

    return result;
}

//>>===========================================================================

bool ASSOCIATION_CLASS::send(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr, int pcId)

//  DESCRIPTION     : Send DICOM Command and Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    //
    // check for valid pointers
    //
    if ((command_ptr == NULL) || 
		(dataset_ptr == NULL)) return false;

	BYTE pcIdRecd = (BYTE)pcId;

	//
    // get the SOP Class UID
    // first try and get it either from the Command Affected or Requested SOP Class UID
    // then try the Dataset - for storage classes this should be set
    //
    string lastSopClassUid = sopClassUidM;
    if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM)) 
    {
        if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM))
        {
			if (!dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, sopClassUidM))
            {
                //
                // try to get sop class uid from iod name
                //
                if (dataset_ptr->getIodName())
                {
                    string sopClassUid = DEFINITION->GetSopUid(dataset_ptr->getIodName());
                    if (sopClassUid.length())
                    {
                        //
                        // use value found in definition
                        //
                        sopClassUidM = sopClassUid;
                    }
                    else
                    {
                        //
                        // can't find the SOP Class UID anywhere !
                        // - let's use the last one stored
                        //
                        sopClassUidM = lastSopClassUid;
                    }
                }
                else 
                {
                    //
                    // can't find the SOP Class UID anywhere !
                    // - let's use the last one stored
                    //
                    sopClassUidM = lastSopClassUid;
                }
            }
        }
    }
    //
    // get the SOP Instance UID
    // first try and get it from either the Command Affected or Requested SOP Instance UID
    // finally try the Dataset itself - for storage classes this should be set
    //
    string lastSopInstanceUid = sopInstanceUidM;
    if (!command_ptr->getUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUidM))
    {
        if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUidM))
        {
			//
            // finally try from SOP instance uid in dataset - don't worry if this fails
            // take whatever value is currently in member
            //
            if (!dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, sopInstanceUidM))
            {
                //
                // check specifically for the N-CREATE-RQ
                //
                if (command_ptr->getCommandId() == DIMSE_CMD_NCREATE_RQ)
                {
                    //
                    // the SOP Instance UID is not specified - that's OK
                    //
                    sopInstanceUidM = "";
                }
                else
                {
                    //
                    // can't find the SOP Instance UID anywhere !
                    // - let's use the last one stored
                    //
                    sopInstanceUidM = lastSopInstanceUid;
                }
            }
        }
    }

	//
    // Apply proper datasettype tag.
    //
    UINT16 datasettype;
    //
    // Either: Apply value as supplied in command set
    //
    bool datasettypePresent = command_ptr->getUSValue(TAG_DATA_SET_TYPE, &datasettype);
    //
    // Or: Apply value as predefined by dicom standard
    //
    if (!datasettypePresent)
    {
        datasettype = (dataset_ptr == NULL) ? NO_DATA_SET : DATA_SET;
    }

    //
    // send the DICOM command
    //
    bool result = sendCommand(command_ptr, pcIdRecd, datasettype);
    
	//
    // send the DICOM dataset
    //
	if(result)
	{
		// initialise the data transfer encode
		if (!networkTransferM.initialiseEncode())
		{
			return false;
		}

		networkTransferM.setPresentationContextId(pcIdRecd);
		networkTransferM.setIsCommandContent(false);

		// set the Transfer Syntax Code to use for encode
		UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);

		if (!acceptedPCM.getTransferSyntaxUid(pcIdRecd, transferSyntaxUid))
		{
			// can't find the transfer syntax in accepted list - continue with default
			if ((pcId) &&
				(loggerM_ptr))
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Can't find Transfer Syntax for PC ID: %d in accepted list", pcId);
			}
			return false;
		}
		networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

		// set the EnsureEvenAttributeValueLength flag
		dataset_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

		// encode the dataset to the data transfer
		if (!dataset_ptr->encode(networkTransferM))
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to send Dataset");
			}
			return false;
		}

		// terminate the data transfer encode
		if (!networkTransferM.terminateEncode())
		{
			return false;
		}

		// send the data transfer requests
		result = dataTransferRequestLocal();

		// cleanup the data transfer
		networkTransferM.cleanup();
	}

	//
    // serialize it after sending to get the complete command - 
    // attributes are added by the sendCommand method
    //
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeSend(command_ptr, dataset_ptr);
    }

	// return result
	return result;
}

//>>===========================================================================

void ASSOCIATION_CLASS::completeCommandOnSend(DCM_COMMAND_CLASS *command_ptr, UINT16 dataSetType)

//  DESCRIPTION     : Method to ensure that the DIMSE Command sent is
//					  complete - any undefined command fields will be filled
//					  in.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The setxxValue fails if the attribute has already
//					  been set - we don't need to check the return status as
//					  we are only interested in completing the commands - if
//					  attribute values have already been set then fine.
//<<===========================================================================
{
	// get the Command Field from the Command Id
	UINT16 commandField = mapCommandId(command_ptr->getCommandId());

	// set the Command Field
	(void) command_ptr->setUSValue(TAG_COMMAND_FIELD, commandField, false);

	// fill in request / response specific values
	if (commandField & 0x8000) 
	{
		// set the Message Id Being Responded To
		(void) command_ptr->setUSValue(TAG_MESSAGE_ID_BEING_RESPONDED_TO, messageIdBeingRespondedToM, false);

		// set the Status - default it to OK
		(void) command_ptr->setUSValue(TAG_STATUS, DCM_STATUS_SUCCESS, false);
	}
	else if (commandField == C_CANCEL_RQ)
	{
		// set the Message Id Being Responded To as last Message Id used
		// - it will have been incremented if it has already been used to
		// send a command
		if (messageIdM == 1)
		{
			(void) command_ptr->setUSValue(TAG_MESSAGE_ID_BEING_RESPONDED_TO, messageIdM, false);
		}
		else
		{
			(void) command_ptr->setUSValue(TAG_MESSAGE_ID_BEING_RESPONDED_TO, messageIdM - 1, false);
		}
	}
	else {
		// set the Message Id
		(void) command_ptr->setUSValue(TAG_MESSAGE_ID, messageIdM++, false);

		// for some Composite Commands - set the priority
		switch(commandField) 
		{
		case C_STORE_RQ:
		case C_FIND_RQ:
		case C_GET_RQ:
		case C_MOVE_RQ:
			(void) command_ptr->setUSValue(TAG_PRIORITY, MEDIUM_PRIORITY, false);
			break;

		default:
			break;
		}
	}

	// set SOP Class UID
	switch(commandField)
	{
	case C_ECHO_RQ:
	case C_ECHO_RSP:
	case C_FIND_RQ:
	case C_FIND_RSP:
	case C_GET_RQ:
	case C_GET_RSP:
	case C_MOVE_RQ:
	case C_MOVE_RSP:
		// set the Affected SOP Class UID
		(void) command_ptr->setUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM);
		break;

	case C_STORE_RQ:
	case C_STORE_RSP:
	case N_CREATE_RQ:
	case N_CREATE_RSP:
	case N_SET_RSP:
	case N_GET_RSP:
	case N_ACTION_RSP:
	case N_DELETE_RSP:
	case N_EVENT_REPORT_RQ:
	case N_EVENT_REPORT_RSP:
		// set the Affected SOP Class UID & Affected SOP Instance UID
		(void) command_ptr->setUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM);
		(void) command_ptr->setUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUidM);
		break;

	case N_SET_RQ:
	case N_GET_RQ:
	case N_ACTION_RQ:
	case N_DELETE_RQ:
		// set the Requested SOP Class UID & Requested SOP Instance UID
		(void) command_ptr->setUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM);
		(void) command_ptr->setUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUidM);
		break;

	default:
		break;
	}
	
	// set the Data Set Type
	(void) command_ptr->setUSValue(TAG_DATA_SET_TYPE, dataSetType);
}

//>>===========================================================================

BYTE ASSOCIATION_CLASS::generatePCID()

//  DESCRIPTION     : Generates a unique odd presentation context id.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// generate next odd number
	if (pc_idM & 0x01) 
	{
		// make counter even
		pc_idM++;
	}

	// make counter odd
	pc_idM++;

	return (BYTE) (pc_idM & 0x000000ff);
}

//>>===========================================================================

void ASSOCIATION_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    loggerM_ptr = logger_ptr; 
		
    if (socketM_ptr)
    {
        socketM_ptr->setLogger(logger_ptr); 
    }

    acceptedPCM.setLogger(logger_ptr);
	supportedPCM.setLogger(logger_ptr); 
	networkTransferM.setLogger(logger_ptr); 
}

//>>===========================================================================

void ASSOCIATION_CLASS::setSerializer(BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Set the serializer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    serializerM_ptr = serializer_ptr;
}
