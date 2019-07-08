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

#ifndef ASSOC_STATES_H
#define ASSOC_STATES_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"        // Global component interface
#include "association.h"    // Association


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DATA_TF_PDU_CLASS;
class LOG_CLASS;
class PDU_CLASS;


//>>***************************************************************************

class DULP_STATE_CLASS

//  DESCRIPTION     : DULP State Base Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	int	stateNumberM;

	bool logInvalidEventState(LOG_CLASS*, int);

	void logEventState(LOG_CLASS*, int);

	void logAction(LOG_CLASS*, int);

	void logNextState(LOG_CLASS*, int);

	int getStateNumber()
		{ return stateNumberM; }

	// association establishment actions
	bool ae1(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ae2(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ae3(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool ae4(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool ae5(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ae6(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool ae7(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ae8(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);

	// data transfer actions
	bool dt1(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, DATA_TF_PDU_CLASS*);
	bool dt2(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, DATA_TF_PDU_CLASS*);

	// association release related actions
	bool ar1(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ar2(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool ar3(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool ar4(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ar5(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ar6(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool ar7(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, DATA_TF_PDU_CLASS*);
	bool ar8(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool ar9(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool ar10(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);

	// association abort related actions
	bool aa1(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS *pdu_ptr = NULL);
	bool aa2(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS *pdu_ptr = NULL);
	bool aa3(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool aa4(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool aa5(ASSOCIATION_CLASS*, DULP_STATE_CLASS*);
	bool aa6(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool aa7(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);
	bool aa8(ASSOCIATION_CLASS*, DULP_STATE_CLASS*, PDU_CLASS*);

	// test only actions
	bool to1(ASSOCIATION_CLASS*);

public:
	DULP_STATE_CLASS();
	virtual ~DULP_STATE_CLASS();

	// DULP events
	virtual bool associateRequestLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_LOCAL);
	}

	virtual bool transportConfirmLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_TRANSPORT_CONFIRM_LOCAL);
	}

	virtual bool associateAcceptPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED);
	}

	virtual bool associateRejectPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REJECT_PDU_RECEIVED);
	}

	virtual bool transportIndicationLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_TRANSPORT_INDICATION_LOCAL);
	}

	virtual bool associateRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ASSOCIATE_REQUEST_PDU_RECEIVED);
	}

	virtual bool associateResponseAcceptLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ASSOCIATE_RESPONSE_ACCEPT_LOCAL);
	}

	virtual bool associateResponseRejectLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ASSOCIATE_RESPONSE_REJECT_LOCAL);
	}

	virtual bool dataTransferRequestLocal(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_REQUEST_LOCAL);
	}

	virtual bool dataTransferPduReceived(ASSOCIATION_CLASS *association_ptr, DATA_TF_PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_DATA_TRANSFER_PDU_RECEIVED);
	}

	virtual bool releaseRequestLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_LOCAL);
	}

	virtual bool releaseRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_RELEASE_REQUEST_PDU_RECEIVED);
	}

	virtual bool releaseResponsePduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_PDU_RECEIVED);
	}

	virtual bool releaseResponseLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_RELEASE_RESPONSE_LOCAL);
	}

	virtual bool abortRequestLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_LOCAL);
	}

	virtual bool abortRequestPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ABORT_REQUEST_PDU_RECEIVED);
	}

	virtual bool transportClosedLocal(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_TRANSPORT_CLOSED_LOCAL);
	}

	virtual bool artimTimerExpired(ASSOCIATION_CLASS *association_ptr)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_ARTIM_TIMER_EXPIRED);
	}

	virtual bool invalidPduReceived(ASSOCIATION_CLASS *association_ptr, PDU_CLASS*)
	{	// no actions defined in base class
		return logInvalidEventState(association_ptr->getLogger(), DULP_INVALID_PDU_RECEIVED);
	}

	virtual bool invalidPduLocal(ASSOCIATION_CLASS*);
};


//>>***************************************************************************

class STATE1_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 1 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE1_CLASS();
	~STATE1_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateRequestLocal(ASSOCIATION_CLASS*);

	bool transportIndicationLocal(ASSOCIATION_CLASS*);
};


//>>***************************************************************************

class STATE2_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 2 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE2_CLASS();
	~STATE2_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool artimTimerExpired(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE3_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 3 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE3_CLASS();
	~STATE3_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateResponseAcceptLocal(ASSOCIATION_CLASS*);

	bool associateResponseRejectLocal(ASSOCIATION_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE4_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 4 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE4_CLASS();
	~STATE4_CLASS();

	static DULP_STATE_CLASS *instance();

	bool transportConfirmLocal(ASSOCIATION_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);
};


//>>***************************************************************************

class STATE5_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 5 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE5_CLASS();
	~STATE5_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE6_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 6 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE6_CLASS();
	~STATE6_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferRequestLocal(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestLocal(ASSOCIATION_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE7_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 7 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE7_CLASS();
	~STATE7_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE8_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 8 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE8_CLASS();
	~STATE8_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferRequestLocal(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponseLocal(ASSOCIATION_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE9_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 9 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE9_CLASS();
	~STATE9_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponseLocal(ASSOCIATION_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE10_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 10 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE10_CLASS();
	~STATE10_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE11_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 11 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE11_CLASS();
	~STATE11_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE12_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 12 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE12_CLASS();
	~STATE12_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponseLocal(ASSOCIATION_CLASS*);

	bool abortRequestLocal(ASSOCIATION_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


//>>***************************************************************************

class STATE13_CLASS : DULP_STATE_CLASS

//  DESCRIPTION     : DULP State 13 Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static DULP_STATE_CLASS	*instanceM_ptr;	// Singleton

public:
	STATE13_CLASS();
	~STATE13_CLASS();

	static DULP_STATE_CLASS *instance();

	bool associateAcceptPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRejectPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool associateRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool dataTransferPduReceived(ASSOCIATION_CLASS*, DATA_TF_PDU_CLASS*);

	bool releaseRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool releaseResponsePduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool abortRequestPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);

	bool transportClosedLocal(ASSOCIATION_CLASS*);

	bool artimTimerExpired(ASSOCIATION_CLASS*);

	bool invalidPduReceived(ASSOCIATION_CLASS*, PDU_CLASS*);
};


#endif /* ASSOC_STATES_H */