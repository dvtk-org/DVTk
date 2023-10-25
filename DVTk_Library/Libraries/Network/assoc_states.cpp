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
#include "assoc_states.h"
#include "abort_rq.h"
#include "assoc_ac.h"
#include "assoc_rj.h"
#include "assoc_rq.h"
#include "rel_rp.h"
#include "rel_rq.h"
#include "unknown_pdu.h"

#include "Ilog.h"				// Log component interface


//*****************************************************************************
// initialise static pointers
//*****************************************************************************
DULP_STATE_CLASS *STATE1_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE2_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE3_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE4_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE5_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE6_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE7_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE8_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE9_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE10_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE11_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE12_CLASS::instanceM_ptr = NULL;
DULP_STATE_CLASS *STATE13_CLASS::instanceM_ptr = NULL;


//*****************************************************************************
//  STATIC DECLARATIONS
//*****************************************************************************
static char *EVENT_DESCRIPTION_TABLE[] = {
{"A-ASSOCIATE request (local)"},
{"Transport Connection confirmation (local)"},
{"A-ASSOCIATE-AC PDU (received on transport)"},
{"A-ASSOCIATE-RJ PDU (received on transport)"},
{"Transport Connection indication"},
{"A-ASSOCIATE-RQ PDU (received on tranport)"},
{"A-ASSOCIATE response primitive (accept)"},
{"A-ASSOCIATE response primitive (reject)"},
{"P-DATA request primitive"},
{"P-DATA-TF PDU (received on transport)"},
{"A-RELEASE request primitive"},
{"A-RELEASE-RQ PDU (received on transport)"},
{"A-RELEASE-RP PDU (received on transport)"},
{"A-RELEASE response primitive"},
{"A-ABORT request primitive"},
{"A-ABORT PDU (received on transport)"},
{"Transport Connection closed indication"},
{"ARTIM timer expired (reject/release)"},
{"Unrecognized or invalid PDU received"},
{"Unknown PDU primitive - Test Only"}
};


static char *STATE_DESCRIPTION_TABLE[] = {
{"State 0: Invalid"},
{"State 1: Idle"},
{"State 2: Transport Connection open (Awaiting A-ASSOCIATE-RQ PDU)"},
{"State 3: Awaiting local A-ASSOCIATE response primitive"},
{"State 4: Awaiting transport connection opening to complete"},
{"State 5: Awaiting A-ASSOCIATE-AC or A-ASSOCIATE-RJ PDU"},
{"State 6: Association established and ready for data transfer"},
{"State 7: Awaiting A-RELEASE-RP PDU"},
{"State 8: Awaiting local A-RELEASE response primitive"},
{"State 9: Release collision requestor side; awaiting A-RELEASE response primitive"},
{"State 10: Release collision acceptor side; awaiting A-RELEASE-RP PDU"},
{"State 11: Release collision requestor side; awaiting A-RELEASE-RP PDU"},
{"State 12: Release collision acceptor side; awaiting A-RELEASE response primitive"},
{"State 13: Awaiting Transport Connection close indication"},
};


static char *ACTION_DESCRIPTION_TABLE[] = {
{"AE-1: Issue Transport Connect request primitive"},
{"AE-2: Send A-ASSOCIATE-PDU"},
{"AE-3: Issue A-ASSOCIATE confirmation (accept) primitive"},
{"AE-4: Issue A-ASSOCIATE confirmation (reject) primitive"},
{"AE-5: Issue Transport Connection response primitive"},
{"AE-6: Stop ARTIM timer and examine A-ASSOCIATE-RQ PDU"},
{"AE-7: Send A-ASSOCIATE-AC PDU"},
{"AE-8: Send A-ASSOCIATE-RJ PDU"},
{"DT-1: Send P-DATA-TF PDU"},
{"DT-2: Send P-DATA indication primitive"},
{"AR-1: Send A-RELEASE-RQ PDU"},
{"AR-2: Issue A-RELEASE indication primitive"},
{"AR-3: Issue A-RELEASE confirmation primitive"},
{"AR-4: Send A-RELEASE-RP PDU"},
{"AR-5: Stop ARTIM timer"},
{"AR-6: Issue P-DATA indication"},
{"AR-7: Send P-DATA-TF PDU"},
{"AR-8: Issue A-RELEASE indication"},
{"AR-9: Send A-RELEASE-RP PDU"},
{"AR-10: Issue A-RELEASE confirmation primitive"},
{"AA-1: Send A-ABORT PDU"},
{"AA-2: Close Transport Connection"},
{"AA-3: Issue A-(P-)ABORT indication primitive"},
{"AA-4: Issue A-P-ABORT indication primitive"},
{"AA-5: Stop ARTIM timer"},
{"AA-6: Ignore PDU"},
{"AA-7: Send A-ABORT PDU"},
{"AA-8: Send A-ABORT PDU"},
{"TO-1: Send Unknown PDU"}
};


//>>===========================================================================

DULP_STATE_CLASS::DULP_STATE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activiites
	stateNumberM = 0;
}

//>>===========================================================================

DULP_STATE_CLASS::~DULP_STATE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

bool DULP_STATE_CLASS::logInvalidEventState(LOG_CLASS *logger_ptr, int eventNumber)

//  DESCRIPTION     : Log the given Invalid Event and State combination.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if logging enabled
	if (logger_ptr)
	{
		// log invalid event / state combination
		logger_ptr->text(LOG_DULP_FSM, 1, "DULP - Invalid event - %s", EVENT_DESCRIPTION_TABLE[eventNumber]);
		logger_ptr->text(LOG_DULP_FSM, 1, "DULP - in %s", STATE_DESCRIPTION_TABLE[stateNumberM]);
	}

	return true;
}

//>>===========================================================================

void DULP_STATE_CLASS::logEventState(LOG_CLASS *logger_ptr, int eventNumber)

//  DESCRIPTION     : Log the given Event and State combination.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if logging enabled
	if (logger_ptr)
	{
		// log invalid event / state combination
		logger_ptr->text(LOG_DULP_FSM, 1, "DULP - Event - %s", EVENT_DESCRIPTION_TABLE[eventNumber]);
		logger_ptr->text(LOG_DULP_FSM, 1, "DULP - in %s", STATE_DESCRIPTION_TABLE[stateNumberM]);
	}
}

//>>===========================================================================

void DULP_STATE_CLASS::logAction(LOG_CLASS *logger_ptr, int actionNumber)

//  DESCRIPTION     : Log the given action.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if logging enabled
	if (logger_ptr)
	{
		// log action
		logger_ptr->text(LOG_DULP_FSM, 1, "DULP - Action - %s", ACTION_DESCRIPTION_TABLE[actionNumber]);
	}
}

//>>===========================================================================

void DULP_STATE_CLASS::logNextState(LOG_CLASS *logger_ptr, int nextStateNumber)

//  DESCRIPTION     : Log the given next state change.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if logging enabled
	if (logger_ptr)
	{
		// log next state
		logger_ptr->text(LOG_DULP_FSM, 1, "DULP - Next state - %s", STATE_DESCRIPTION_TABLE[nextStateNumber]);
	}
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae1(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AE-1 - Issue Transport Connect request primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AE_1);

	if (association_ptr->socketM_ptr == NULL)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "TCP/IP: Cannot connect. Socket class not defined");
		}
		return false;
	}

	// connect to hostname on port number
	result = association_ptr->socketM_ptr->connect();

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae2(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AE-2 - Send A-ASSOCIATE-RQ PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	ASSOCIATE_RQ_CLASS	*associateRq_ptr = association_ptr->associateRqM_ptr;
	PDU_CLASS			pdu;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, AE_2);

	// encode the Associate Request into a PDU
	if (associateRq_ptr)
	{
		// encode into a PDU
		result = associateRq_ptr->encode(pdu);
	}

	// send Associate Request PDU
	if (result)
	{
		// log the Associate Request PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae3(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AE-3 - Issue A-ASSOCIATE confirmation (accept) primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AE_3);

	// instantiate a new Associate Accept
	ASSOCIATE_AC_CLASS	*associateAc_ptr = new ASSOCIATE_AC_CLASS();

	// decode the Associate Accept from the PDU
	if (associateAc_ptr)
	{
		// cascade the logger
		associateAc_ptr->setLogger(logger_ptr);

		// decode from the PDU
		result = associateAc_ptr->decode(*pdu_ptr);

		// on success...
		if (result)
		{
			// link the Associate Accept
			association_ptr->associateAcM_ptr = associateAc_ptr;
		}
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae4(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AE-4 - Issue A-ASSOCIATE confirmation (reject) primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AE_4);

	// instantiate a new Associate Reject
	ASSOCIATE_RJ_CLASS	*associateRj_ptr = new ASSOCIATE_RJ_CLASS();

	// decode the Associate Reject from the PDU
	if (associateRj_ptr)
	{
		// cascade the logger
		associateRj_ptr->setLogger(logger_ptr);

		// decode from the PDU
		result = associateRj_ptr->decode(*pdu_ptr);

		// on success...
		if (result)
		{
			// link the Associate Reject
			association_ptr->associateRjM_ptr = associateRj_ptr;
		}
	}

	// close connection
	if (association_ptr->socketM_ptr)
	{
		association_ptr->socketM_ptr->close();
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae5(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AE-5 - Issue Transport Connection response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	BASE_SOCKET_CLASS	*newSocket_ptr = NULL;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, AE_5);

	if (association_ptr->socketM_ptr == NULL)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "TCP/IP: Cannot listen to port. Socket class not defined");
		}
		return false;
	}

	// listen for connection from peer
	result = association_ptr->socketM_ptr->listen();
	if (!result)
	{
	}

	// accept connection from peer
	result = association_ptr->socketM_ptr->accept(&newSocket_ptr);

	// on success change state
	if (result) 
	{
		// throw away the listen socket and use the new accepted socket
		delete association_ptr->socketM_ptr;
		association_ptr->socketM_ptr = newSocket_ptr;

		// start ARTIM timer

		// log the next state
		logNextState(association_ptr->getLogger(), dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}
	else
	{
		// close the socket to make sure it is closed
		association_ptr->socketM_ptr->close();

		if (association_ptr->getLogger())
		{
			logger_ptr->text(LOG_INFO, 1, "No longer accepting connections on TCP/IP port: %d", association_ptr->socketM_ptr->getLocalListenPort());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae6(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AE-6 - Issue A-ASSOCIATE indication primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AE_6);

	// stop ARTIM timer

	// instantiate a new Associate Request
	ASSOCIATE_RQ_CLASS	*associateRq_ptr = new ASSOCIATE_RQ_CLASS();

	// decode the Associate Request from the PDU
	if (associateRq_ptr)
	{
		// cascade the logger
		associateRq_ptr->setLogger(logger_ptr);

		// decode from the PDU
		result = associateRq_ptr->decode(*pdu_ptr);

		// on success...
		if (result)
		{
			// link the Associate Request
			association_ptr->associateRqM_ptr = associateRq_ptr;
		}
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae7(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AE-7 - Send A-ASSOCIATE-AC PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	ASSOCIATE_AC_CLASS	*associateAc_ptr = association_ptr->associateAcM_ptr;
	PDU_CLASS			pdu;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, AE_7);

	// encode the Associate Accept into a PDU
	if (associateAc_ptr)
	{
		// encode into a PDU
		result = associateAc_ptr->encode(pdu);
	}

	// send Associate Accept PDU
	if (result)
	{
		// log the Associate Accept PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ae8(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AE-8 - Send A-ASSOCIATE-RJ PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	ASSOCIATE_RJ_CLASS	*associateRj_ptr = association_ptr->associateRjM_ptr;
	PDU_CLASS			pdu;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, AE_8);

	// encode the Associate Reject into a PDU
	if (associateRj_ptr)
	{
		// encode into a PDU
		result = associateRj_ptr->encode(pdu);
	}

	// send Associate Reject PDU
	if (result)
	{
		// log the Associate Reject PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// start ARTIM timer

		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::dt1(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : Action DT-1 - Send P-DATA-TF PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, DT_1);

	// check that we have a PDU to send
	if (dataTfPdu_ptr) 
	{
		// log the data transfer PDU
		dataTfPdu_ptr->logRaw(logger_ptr);

		// send PDU
		result = dataTfPdu_ptr->write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::dt2(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : Action DT-2 - Issue P-DATA indication primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();

	// log the requested Action
	logAction(logger_ptr, DT_2);

	// save the data transfer PDU
	association_ptr->networkTransferM.addDataTransferPdu(dataTfPdu_ptr);

	// log the next state
	logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

	// change state
	association_ptr->changeState(dulpNextState_ptr);

	// return result
	return true;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar1(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AR-1 - Send A-RELEASE-RQ PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	RELEASE_RQ_CLASS	*releaseRq_ptr = association_ptr->releaseRqM_ptr;
	PDU_CLASS			pdu;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, AR_1);

	// encode the Release Request into a PDU
	if (releaseRq_ptr)
	{
		// encode into a PDU
		result = releaseRq_ptr->encode(pdu);
	}

	// send Release Request PDU
	if (result)
	{
		// log the Request Request PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar2(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AR-2 - Issue A-RELEASE indication primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AR_2);

	// instantiate a new Release Request
	RELEASE_RQ_CLASS	*releaseRq_ptr = new RELEASE_RQ_CLASS();

	// decode the Release Request from the PDU
	if (releaseRq_ptr)
	{
		// cascade the logger
		releaseRq_ptr->setLogger(logger_ptr);

		// decode from the PDU
		result = releaseRq_ptr->decode(*pdu_ptr);

		// on success...
		if (result)
		{
			// link the Release Request
			association_ptr->releaseRqM_ptr = releaseRq_ptr;
		}
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar3(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AR-3 - Issue A-RELEASE confirmation primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AR_3);

	// instantiate a new Release Response
	RELEASE_RP_CLASS	*releaseRp_ptr = new RELEASE_RP_CLASS();

	// decode the Release Response from the PDU
	if (releaseRp_ptr)
	{
		// cascade the logger
		releaseRp_ptr->setLogger(logger_ptr);

		// decode from the PDU
		result = releaseRp_ptr->decode(*pdu_ptr);

		// on success...
		if (result)
		{
			// link the Release Response
			association_ptr->releaseRpM_ptr = releaseRp_ptr;
		}
	}

	// close connection
	if (association_ptr->socketM_ptr)
	{
		association_ptr->socketM_ptr->close();
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar4(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AR-4 - Send A-RELEASE-RP PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	RELEASE_RP_CLASS	*releaseRp_ptr = association_ptr->releaseRpM_ptr;
	PDU_CLASS			pdu;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, AR_4);

	// encode the Release Response into a PDU
	if (releaseRp_ptr)
	{
		// encode into a PDU
		result = releaseRp_ptr->encode(pdu);
	}

	// send Release Response PDU
	if (result)
	{
		// log the Release Response PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// start ARTIM timer

		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar5(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AR-5 - Stop ARTIM timer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();

	// log the requested Action
	logAction(logger_ptr, AR_5);

	// stop ARTIM timer

	// close connection
	if (association_ptr->socketM_ptr)
	{
		association_ptr->socketM_ptr->close();
	}

	// log the next state
	logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

	// change state
	association_ptr->changeState(dulpNextState_ptr);

	// return result
	return true;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar6(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS*)

//  DESCRIPTION     : Action AR-6 - Issue P-DATA Indication.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();

	// log the requested Action
	logAction(logger_ptr, AR_6);

	// log the next state
	logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

	// change state
	association_ptr->changeState(dulpNextState_ptr);

	// return result
	return true;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar7(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : Action AR-7 - Send P-DATA-TF PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AR_7);

	// check that we have a PDU to send
	if (dataTfPdu_ptr) 
	{
		// log the data transfer PDU
		dataTfPdu_ptr->logRaw(logger_ptr);

		// send PDU
		result = dataTfPdu_ptr->write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar8(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS*)

//  DESCRIPTION     : Action AR-8 - Issue A-RELEASE indication.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();

	// log the requested Action
	logAction(logger_ptr, AR_8);

	// log the next state
	logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

	// change state
	association_ptr->changeState(dulpNextState_ptr);

	// return result
	return true;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar9(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AR-9 - Send A-RELEASE-RP PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	RELEASE_RP_CLASS	*releaseRp_ptr = association_ptr->releaseRpM_ptr;
	PDU_CLASS			pdu;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, AR_9);

	// encode the Release Response into a PDU
	if (releaseRp_ptr)
	{
		// encode into a PDU
		result = releaseRp_ptr->encode(pdu);
	}

	// send Release Response PDU
	if (result)
	{
		// log the Release Response PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::ar10(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AR-10 - Issue A-RELEASE confirmation.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AR_10);

	// instantiate a new Release Response
	RELEASE_RP_CLASS	*releaseRp_ptr = new RELEASE_RP_CLASS();

	// decode the Release Response from the PDU
	if (releaseRp_ptr)
	{
		// cascade the logger
		releaseRp_ptr->setLogger(logger_ptr);

		// decode from the PDU
		result = releaseRp_ptr->decode(*pdu_ptr);

		// on success...
		if (result)
		{
			// link the Release Response
			association_ptr->releaseRpM_ptr = releaseRp_ptr;
		}
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa1(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AA-1 - Send A-ABORT-RQ PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS		*logger_ptr = association_ptr->getLogger();
	ABORT_RQ_CLASS	*abortRq_ptr = association_ptr->abortRqM_ptr;
	PDU_CLASS		pdu;
	bool			result = false;

	// log the requested Action
	logAction(logger_ptr, AA_1);

	// check if a PDU has been received
	if (pdu_ptr) 
	{
		// instantiate a new Unknown PDU
		UNKNOWN_PDU_CLASS *unknownPdu_ptr = new UNKNOWN_PDU_CLASS();

		// decode the Unknown data from the PDU
		if (unknownPdu_ptr)
		{
			// cascade the logger
			unknownPdu_ptr->setLogger(logger_ptr);

			// decode from the PDU
			result = unknownPdu_ptr->decode(*pdu_ptr);

			// on success...
			if (result)
			{
				// link the Abort Request
				association_ptr->unknownPduM_ptr = unknownPdu_ptr;
			}
		}
	}

	// encode the Abort Request into a PDU
	if (abortRq_ptr)
	{
		// encode into a PDU
		result = abortRq_ptr->encode(pdu);
	}

	// send Abort Request PDU
	if (result)
	{
		// log the Abort Request PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// (re)start ARTIM timer

		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa2(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AA-2 - Stop ARTIM timer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = true;

	// log the requested Action
	logAction(logger_ptr, AA_2);

	// check if a PDU has been received
	if (pdu_ptr) 
	{
		// instantiate a new Unknown PDU
		UNKNOWN_PDU_CLASS *unknownPdu_ptr = new UNKNOWN_PDU_CLASS();

		// decode the Unknown data from the PDU
		if (unknownPdu_ptr)
		{
			// cascade the logger
			unknownPdu_ptr->setLogger(logger_ptr);

			// decode from the PDU
			result = unknownPdu_ptr->decode(*pdu_ptr);

			// on success...
			if (result)
			{
				// link the Abort Request
				association_ptr->unknownPduM_ptr = unknownPdu_ptr;
			}
		}
	}

	// stop ARTIM timer

	// close connection
	if (association_ptr->socketM_ptr)
	{
		association_ptr->socketM_ptr->close();
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa3(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AA-3 - Issue A-(P-)Abort indication.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();
	bool		result = false;

	// log the requested Action
	logAction(logger_ptr, AA_3);

	// instantiate a new Abort Request
	ABORT_RQ_CLASS	*abortRq_ptr = new ABORT_RQ_CLASS();
	
	// decode the Abort Request from the PDU
	if (abortRq_ptr)
	{
		// cascade the logger
		abortRq_ptr->setLogger(logger_ptr);

		// decode from the PDU
		result = abortRq_ptr->decode(*pdu_ptr);

		// on success...
		if (result)
		{
			// link the Abort Request
			association_ptr->abortRqM_ptr = abortRq_ptr;
		}
	}

	// close connection
	if (association_ptr->socketM_ptr)
	{
		association_ptr->socketM_ptr->close();
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa4(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AA-4 - Issue A-P-Abort indication primitive.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();

	// log the requested Action
	logAction(logger_ptr, AA_4);

	// log the next state
	logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

	// change state
	association_ptr->changeState(dulpNextState_ptr);

	// return result
	return true;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa5(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr)

//  DESCRIPTION     : Action AA-5 - Stop ARTIM timer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();

	// log the requested Action
	logAction(logger_ptr, AA_5);

	// stop ARTIM timer

	// log the next state
	logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

	// change state
	association_ptr->changeState(dulpNextState_ptr);

	// return result
	return true;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa6(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS*)

//  DESCRIPTION     : Action AA-6 - Ignore PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS	*logger_ptr = association_ptr->getLogger();

	// log the requested Action
	logAction(logger_ptr, AA_6);

	// log the next state
	logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

	// change state
	association_ptr->changeState(dulpNextState_ptr);

	// return result
	return true;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa7(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AA-7 - Send A-ABORT-RQ PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS		*logger_ptr = association_ptr->getLogger();
	ABORT_RQ_CLASS	*abortRq_ptr = association_ptr->abortRqM_ptr;
	PDU_CLASS		pdu;
	bool			result = false;

	// log the requested Action
	logAction(logger_ptr, AA_7);

	// check if a PDU has been received
	if (pdu_ptr) 
	{
		// instantiate a new Unknown PDU
		UNKNOWN_PDU_CLASS *unknownPdu_ptr = new UNKNOWN_PDU_CLASS();

		// decode the Unknown data from the PDU
		if (unknownPdu_ptr)
		{
			// cascade the logger
			unknownPdu_ptr->setLogger(logger_ptr);

			// decode from the PDU
			result = unknownPdu_ptr->decode(*pdu_ptr);

			// on success...
			if (result)
			{
				// link the Abort Request
				association_ptr->unknownPduM_ptr = unknownPdu_ptr;
			}
		}
	}

	// encode the Abort Request into a PDU
	if (abortRq_ptr)
	{
		// encode into a PDU
		result = abortRq_ptr->encode(pdu);
	}

	// send Abort Request PDU
	if (result)
	{
		// log the Abort Request PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::aa8(ASSOCIATION_CLASS *association_ptr, DULP_STATE_CLASS *dulpNextState_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : Action AA-8 - Send A-ABORT-RQ PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS		*logger_ptr = association_ptr->getLogger();
	ABORT_RQ_CLASS	*abortRq_ptr = association_ptr->abortRqM_ptr;
	PDU_CLASS		pdu;
	bool			result = false;

	// log the requested Action
	logAction(logger_ptr, AA_8);

	// check if a PDU has been received
	if (pdu_ptr) 
	{
		// instantiate a new Unknown PDU
		UNKNOWN_PDU_CLASS *unknownPdu_ptr = new UNKNOWN_PDU_CLASS();
			
		// decode the Unknown data from the PDU
		if (unknownPdu_ptr)
		{
			// cascade the logger
			unknownPdu_ptr->setLogger(logger_ptr);

			// decode from the PDU
			result = unknownPdu_ptr->decode(*pdu_ptr);

			// on success...
			if (result)
			{
				// link the Abort Request
				association_ptr->unknownPduM_ptr = unknownPdu_ptr;
			}
		}
	}

	// encode the Abort Request into a PDU
	if (abortRq_ptr)
	{
		// encode into a PDU
		result = abortRq_ptr->encode(pdu);
	}

	// send Abort Request PDU
	if (result)
	{
		// log the Abort Request PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// on success change state
	if (result) 
	{
		// (re)start ARTIM timer

		// log the next state
		logNextState(logger_ptr, dulpNextState_ptr->getStateNumber());

		// change state
		association_ptr->changeState(dulpNextState_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::to1(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : Action TO-1 - Test Only - Send Unknown PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS			*logger_ptr = association_ptr->getLogger();
	UNKNOWN_PDU_CLASS	*unknownPdu_ptr = association_ptr->unknownPduM_ptr;
	PDU_CLASS			pdu;
	bool				result = false;

	// log the requested Action
	logAction(logger_ptr, TO_1);

	// encode the Unknown data into a PDU
	if (unknownPdu_ptr)
	{
		// encode into a PDU
		result = unknownPdu_ptr->encode(pdu);
	}

	// send Unknown PDU
	if (result)
	{
		// log the Unknown PDU
		pdu.logRaw(logger_ptr);

		// send PDU
		result = pdu.write(association_ptr->socketM_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

bool DULP_STATE_CLASS::invalidPduLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : Invalid PDU local - send it.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_LOCAL);

	// handle event
	return to1(association_ptr);
}


//>>===========================================================================

STATE1_CLASS::STATE1_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 1;
}

//>>===========================================================================

STATE1_CLASS::~STATE1_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE1_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE1_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE1_CLASS::associateRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_LOCAL);

	// handle event and switch state
	return ae1(association_ptr, STATE4_CLASS::instance());	
}

//>>===========================================================================

bool STATE1_CLASS::transportIndicationLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_INDICATION_LOCAL);

	// handle event and switch state
	return ae5(association_ptr, STATE2_CLASS::instance());
}


//>>===========================================================================

STATE2_CLASS::STATE2_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 2;
}

//>>===========================================================================

STATE2_CLASS::~STATE2_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE2_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE2_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE2_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE2_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE2_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return ae6(association_ptr, STATE3_CLASS::instance(), pdu_ptr);
//or
//		return ae6(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE2_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE2_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE2_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE2_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa2(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE2_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa5(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE2_CLASS::artimTimerExpired(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ARTIM_TIMER_EXPIRED);

	// handle event and switch state
	return aa2(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE2_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE3_CLASS::STATE3_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 3;
}

//>>===========================================================================

STATE3_CLASS::~STATE3_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE3_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE3_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE3_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE3_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE3_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE3_CLASS::associateResponseAcceptLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_RESPONSE_ACCEPT_LOCAL);

	// handle event and switch state
	return ae7(association_ptr, STATE6_CLASS::instance());
}

//>>===========================================================================

bool STATE3_CLASS::associateResponseRejectLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_RESPONSE_REJECT_LOCAL);

	// handle event and switch state
	return ae8(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE3_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE3_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE3_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE3_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE3_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE3_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE3_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE4_CLASS::STATE4_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 4;
}

//>>===========================================================================

STATE4_CLASS::~STATE4_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE4_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE4_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE4_CLASS::transportConfirmLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CONFIRM_LOCAL);

	// handle event and switch state
	return ae2(association_ptr, STATE5_CLASS::instance());
}

//>>===========================================================================

bool STATE4_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa2(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE4_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}


//>>===========================================================================

STATE5_CLASS::STATE5_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 5;
}

//>>===========================================================================

STATE5_CLASS::~STATE5_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE5_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE5_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE5_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return ae3(association_ptr, STATE6_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE5_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return ae4(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE5_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE5_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE5_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE5_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE5_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE5_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE5_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE5_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE6_CLASS::STATE6_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 6;
}

//>>===========================================================================

STATE6_CLASS::~STATE6_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE6_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE6_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE6_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::dataTransferRequestLocal(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_REQUEST_LOCAL);

	// handle event and switch state
	return dt1(association_ptr, STATE6_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return dt2(association_ptr, STATE6_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::releaseRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_LOCAL);

	// handle event and switch state
	return ar1(association_ptr, STATE7_CLASS::instance());
}

//>>===========================================================================

bool STATE6_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return ar2(association_ptr, STATE8_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE6_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE6_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE6_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE7_CLASS::STATE7_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 7;
}

//>>===========================================================================

STATE7_CLASS::~STATE7_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE7_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE7_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE7_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE7_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE7_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE7_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return ar6(association_ptr, STATE7_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE7_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return ar8(association_ptr, STATE9_CLASS::instance(), pdu_ptr);
//or
//	return ar8(association_ptr, STATE10_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE7_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return ar3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE7_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE7_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE7_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE7_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE8_CLASS::STATE8_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 8;
}

//>>===========================================================================

STATE8_CLASS::~STATE8_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE8_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE8_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE8_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::dataTransferRequestLocal(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_REQUEST_LOCAL);

	// handle event and switch state
	return ar7(association_ptr, STATE8_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::releaseResponseLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_LOCAL);

	// handle event and switch state
	return ar4(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE8_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE8_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE8_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE8_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE9_CLASS::STATE9_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 9;
}

//>>===========================================================================

STATE9_CLASS::~STATE9_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE9_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE9_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE9_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE9_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE9_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE9_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE9_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE9_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE9_CLASS::releaseResponseLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_LOCAL);

	// handle event and switch state
	return ar9(association_ptr, STATE11_CLASS::instance());
}

//>>===========================================================================

bool STATE9_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE9_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE9_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE9_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE10_CLASS::STATE10_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 10;
}

//>>===========================================================================

STATE10_CLASS::~STATE10_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE10_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE10_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE10_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE10_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE10_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE10_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE10_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE10_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return ar10(association_ptr, STATE12_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE10_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE10_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE10_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE10_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE11_CLASS::STATE11_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 11;
}

//>>===========================================================================

STATE11_CLASS::~STATE11_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE11_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE11_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE11_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE11_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE11_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE11_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE11_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE11_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return ar3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE11_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE11_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE11_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE11_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE12_CLASS::STATE12_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 12;
}

//>>===========================================================================

STATE12_CLASS::~STATE12_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE12_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE12_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE12_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE12_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE12_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE12_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE12_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE12_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE12_CLASS::releaseResponseLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_LOCAL);

	// handle event and switch state
	return ar4(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE12_CLASS::abortRequestLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);

	// handle event and switch state
	return aa1(association_ptr, STATE13_CLASS::instance());
}

//>>===========================================================================

bool STATE12_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa3(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE12_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return aa4(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE12_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa8(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


//>>===========================================================================

STATE13_CLASS::STATE13_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	stateNumberM = 13;
}

//>>===========================================================================

STATE13_CLASS::~STATE13_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (instanceM_ptr)
	{
		delete instanceM_ptr;
	}
}

//>>===========================================================================

DULP_STATE_CLASS *STATE13_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new STATE13_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool STATE13_CLASS::associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);

	// handle event and switch state
	return aa6(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE13_CLASS::associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);

	// handle event and switch state
	return aa6(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE13_CLASS::associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa7(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE13_CLASS::dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS *dataTfPdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);

	// handle event and switch state
	return aa6(association_ptr, STATE13_CLASS::instance(), dataTfPdu_ptr);
}

//>>===========================================================================

bool STATE13_CLASS::releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa6(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE13_CLASS::releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);

	// handle event and switch state
	return aa6(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE13_CLASS::abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);

	// handle event and switch state
	return aa2(association_ptr, STATE1_CLASS::instance(), pdu_ptr);
}

//>>===========================================================================

bool STATE13_CLASS::transportClosedLocal(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);

	// handle event and switch state
	return ar5(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE13_CLASS::artimTimerExpired(ASSOCIATION_CLASS *association_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_ARTIM_TIMER_EXPIRED);

	// handle event and switch state
	return aa2(association_ptr, STATE1_CLASS::instance());
}

//>>===========================================================================

bool STATE13_CLASS::invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS *pdu_ptr)

//  DESCRIPTION     : DULP state machine - event handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// log event / state information
	logEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);

	// handle event and switch state
	return aa7(association_ptr, STATE13_CLASS::instance(), pdu_ptr);
}


