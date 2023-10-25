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
#include "abort_rq.h"
#include "pdu.h"			// PDU

//>>===========================================================================

ABORT_RQ_CLASS::ABORT_RQ_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ABORT_RQ;
	itemTypeM = PDU_ABORT_RQ;
	reservedM = 0;
	lengthM = sizeof(reserved1M) + sizeof(reserved2M) + sizeof(sourceM) + sizeof(reasonM);
	reserved1M = 0;
	reserved2M = 0;
	sourceM = UNDEFINED_ABORT_SOURCE;
	reasonM = UNDEFINED_ABORT_REASON;
}

//>>===========================================================================

ABORT_RQ_CLASS::ABORT_RQ_CLASS(BYTE source, BYTE reason)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ABORT_RQ;
	itemTypeM = PDU_ABORT_RQ;
	reservedM = 0;
	lengthM = sizeof(reserved1M) + sizeof(reserved2M) + sizeof(sourceM) + sizeof(reasonM);
	reserved1M = 0;
	reserved2M = 0;
	sourceM = source;
	reasonM = reason;
}

//>>===========================================================================

ABORT_RQ_CLASS::~ABORT_RQ_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing explicit to do
}		

//>>===========================================================================

bool ABORT_RQ_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode abort request as PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode the pdu type and length
	pdu.setType(itemTypeM);
	pdu.setReserved(reservedM);
	if (!pdu.allocateBody(lengthM)) return false;

	pdu << reserved1M;			
	pdu << reserved2M;

	// encode the source and reason
	pdu << sourceM;
	pdu << reasonM;

	return true;
}

//>>===========================================================================

bool ABORT_RQ_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode abort request from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the Abort Request PDU
	itemTypeM = pdu.getType();
	reservedM = pdu.getReserved();			
	lengthM = pdu.getLength();			

	pdu >> reserved1M;
	pdu >> reserved2M;
	
	// decode the source & reason
	pdu >> sourceM;
	pdu >> reasonM;

	return true;
}

//>>===========================================================================

UINT32 ABORT_RQ_CLASS::getLength()

//  DESCRIPTION     : Get PDU length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute PDU length
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

bool ABORT_RQ_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this object with the contents of the object given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// ensure update WID is an abort request
	if (wid_ptr->getWidType() == widTypeM)
	{
		ABORT_RQ_CLASS *updateAbortRq_ptr = static_cast<ABORT_RQ_CLASS*>(wid_ptr);

		// update parameters
		sourceM = updateAbortRq_ptr->getSource();
		reasonM = updateAbortRq_ptr->getReason();

		// result is OK
		result = true;
	}

	// return result
	return result;
}
