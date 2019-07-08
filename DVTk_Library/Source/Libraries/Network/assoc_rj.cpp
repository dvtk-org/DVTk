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
#include "assoc_rj.h"
#include "pdu.h"			// PDU


//>>===========================================================================

ASSOCIATE_RJ_CLASS::ASSOCIATE_RJ_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ASSOCIATE_RJ;
	itemTypeM = PDU_ASSOCIATE_RJ;
	reservedM = 0;
	lengthM = sizeof(reserved1M) + sizeof(resultM) + sizeof(sourceM) + sizeof(reasonM);
	reserved1M = 0;
	resultM = UNDEFINED_REJECT_RESULT;
	sourceM = UNDEFINED_REJECT_SOURCE;
	reasonM = UNDEFINED_REJECT_REASON;
}

//>>===========================================================================

ASSOCIATE_RJ_CLASS::ASSOCIATE_RJ_CLASS(BYTE result, BYTE source, BYTE reason)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ASSOCIATE_RJ;
	itemTypeM = PDU_ASSOCIATE_RJ;
	reservedM = 0;
	lengthM = sizeof(reserved1M) + sizeof(resultM) + sizeof(sourceM) + sizeof(reasonM);
	reserved1M = 0;
	resultM = result;
	sourceM = source;
	reasonM = reason;
}

//>>===========================================================================

ASSOCIATE_RJ_CLASS::~ASSOCIATE_RJ_CLASS()

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

bool ASSOCIATE_RJ_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode associate reject as PDU.
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

	// encode the result source and reason
	pdu << resultM;
	pdu << sourceM;
	pdu << reasonM;

	return true;
}

//>>===========================================================================

bool ASSOCIATE_RJ_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode associate reject from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the Associate Reject PDU
	itemTypeM = pdu.getType();
	reservedM = pdu.getReserved();			
	lengthM = pdu.getLength();				

	pdu >> reserved1M;
	
	// decode the result, source & reason
	pdu >> resultM;
	pdu >> sourceM;
	pdu >> reasonM;

	return true;
}

//>>===========================================================================

UINT32 ASSOCIATE_RJ_CLASS::getLength()

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

bool ASSOCIATE_RJ_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this object with the contents of the object given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// ensure update WID is an associate reject
	if (wid_ptr->getWidType() == widTypeM)
	{
		ASSOCIATE_RJ_CLASS *updateAssociateRj_ptr = static_cast<ASSOCIATE_RJ_CLASS*>(wid_ptr);

		// update parameters
		resultM = updateAssociateRj_ptr->getResult();
		sourceM = updateAssociateRj_ptr->getSource();
		reasonM = updateAssociateRj_ptr->getReason();

		// result is OK
		result = true;
	}

	// return result
	return result;
}
