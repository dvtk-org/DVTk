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
#include "rel_rp.h"
#include "pdu.h"

//>>===========================================================================

RELEASE_RP_CLASS::RELEASE_RP_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_RELEASE_RP;
	itemTypeM = PDU_RELEASE_RP;
	reservedM = 0;
	lengthM = sizeof(reserved1M);
	reserved1M = 0;
}

//>>===========================================================================

RELEASE_RP_CLASS::~RELEASE_RP_CLASS()

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

bool RELEASE_RP_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode release response as PDU.
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

	return true;
}

//>>===========================================================================

bool RELEASE_RP_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode release response from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the Release Response PDU
	itemTypeM = pdu.getType();
	reservedM = pdu.getReserved();			
	lengthM = pdu.getLength();
				
	pdu >> reserved1M;

	return true;
}

//>>===========================================================================

UINT32 RELEASE_RP_CLASS::getLength()

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

bool RELEASE_RP_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*)

//  DESCRIPTION     : Update this object with the contents of the object given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// return result
	return result;
}
