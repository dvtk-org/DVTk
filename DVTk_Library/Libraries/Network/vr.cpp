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
//  DESCRIPTION     :	Basic VR Type Classes.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "vr.h"
#include "pdu.h"
#include "Iutility.h"

//>>===========================================================================

BYTE_STRING_CLASS::BYTE_STRING_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	valueM_ptr = NULL;
	lengthM = 0;
}

//>>===========================================================================

BYTE_STRING_CLASS::BYTE_STRING_CLASS(BYTE_STRING_CLASS &string)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = string;
}

//>>===========================================================================

BYTE_STRING_CLASS::BYTE_STRING_CLASS(BYTE* value_ptr, UINT32 length)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	valueM_ptr = NULL;
	lengthM = 0;
	set(value_ptr, length);	
}

//>>===========================================================================

BYTE_STRING_CLASS::BYTE_STRING_CLASS(char* value_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	valueM_ptr = NULL;
	lengthM = 0;
	set(value_ptr);	
}

//>>===========================================================================

BYTE_STRING_CLASS::~BYTE_STRING_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// free up resources
	cleanup();	
}

//>>===========================================================================

void BYTE_STRING_CLASS::cleanup()

//  DESCRIPTION     : Free up resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// free up resources
	if (valueM_ptr) {
		delete [] valueM_ptr;
		valueM_ptr = NULL;
		lengthM = 0;
	}
}

//>>===========================================================================

void BYTE_STRING_CLASS::set(BYTE* value_ptr, UINT32 length)

//  DESCRIPTION     : Set string value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// first free old string
	cleanup();

	// check for valid aet
	if ((!value_ptr) || (!length)) return;

	// allocate new storage
	valueM_ptr = new BYTE [length + 1];

	// copy string
	if (valueM_ptr)
	{
		// save the length
		lengthM = length;

		// copy value and null terminate it for convenience
		byteCopy(valueM_ptr, value_ptr, lengthM);
		valueM_ptr[lengthM] = NULLCHAR;
	}	
}

//>>===========================================================================

void BYTE_STRING_CLASS::set(char* value_ptr)

//  DESCRIPTION     : Set string value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// first free old string
	cleanup();

	// check for valid string
	if (!value_ptr) return;

	// get length of string
	UINT32 length = strlen(value_ptr);
	if (!length) return;	

	// allocate new storage
	valueM_ptr = new BYTE [length + 1];

	// copy string
	if (valueM_ptr)
	{
		// save the length
		lengthM = length;

		// copy value and null terminate it for convenience
		byteCopy(valueM_ptr, (BYTE*) value_ptr, lengthM);
		valueM_ptr[lengthM] = NULLCHAR;
	}	
}

//>>===========================================================================

bool BYTE_STRING_CLASS::operator = (BYTE_STRING_CLASS& string)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// first free old string
	cleanup();

	// copy empty string
	if (!string.get()) return true;

	// allocate new storage
	valueM_ptr = new BYTE [string.getLength() + 1];

	// copy string
	if (valueM_ptr)
	{
		// save the length
		lengthM = string.getLength();

		// copy value and null terminate it for convenience
		byteCopy(valueM_ptr, string.get(), lengthM);
		valueM_ptr[lengthM] = NULLCHAR;

		result = true;
	}

	return result;
}


//>>===========================================================================

AE_TITLE_CLASS::AE_TITLE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

AE_TITLE_CLASS::AE_TITLE_CLASS(AE_TITLE_CLASS &aeTitle)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = aeTitle;
}

//>>===========================================================================

AE_TITLE_CLASS::AE_TITLE_CLASS(BYTE* aet_ptr, UINT32 length)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	set(aet_ptr, length);
}

//>>===========================================================================

AE_TITLE_CLASS::AE_TITLE_CLASS(char* aet_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	set(aet_ptr);
}

//>>===========================================================================

AE_TITLE_CLASS::~AE_TITLE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

void AE_TITLE_CLASS::set(BYTE* aet_ptr, UINT32 length)

//  DESCRIPTION     : Set AET value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// first free old AET
	cleanup();

	// check for valid aet
	if ((!aet_ptr) || (!length)) return;

	// compute length to allocate
	UINT32 valueLength = (length < AE_LENGTH) ? AE_LENGTH : length;

	// allocate new storage
	valueM_ptr = new BYTE [valueLength + 1];

	// copy UID
	if (valueM_ptr)
	{
		// save the length
		lengthM = valueLength;

		// ensure that value is max filled with SPACES
		byteFill(valueM_ptr, SPACECHAR, valueLength);

		// copy value and null terminate it for convenience
		byteCopy(valueM_ptr, aet_ptr, length);
		valueM_ptr[lengthM] = NULLCHAR;
	}
}

//>>===========================================================================

void AE_TITLE_CLASS::set(char* aet_ptr)

//  DESCRIPTION     : Set AET value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// first free old AET
	cleanup();

	// check for valid aet address
	if (!aet_ptr) return;

	// get length of AET
	UINT32 length = strlen(aet_ptr);
	if (!length) return;

	// compute length to allocate
	UINT32 valueLength = (length < AE_LENGTH) ? AE_LENGTH : length;

	// allocate new storage
	valueM_ptr = new BYTE [valueLength + 1];

	// copy UID
	if (valueM_ptr)
	{
		// save the length
		lengthM = valueLength;

		// ensure that value is max filled with SPACES
		byteFill(valueM_ptr, SPACECHAR, valueLength);

		// copy value and null terminate it for convenience
		byteCopy(valueM_ptr, (BYTE*) aet_ptr, length);
		valueM_ptr[lengthM] = NULLCHAR;
	}
}

//>>===========================================================================

bool AE_TITLE_CLASS::operator = (AE_TITLE_CLASS& aeTitle)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// first free old string
	cleanup();

	// copy empty aet
	if (!aeTitle.get()) return true;

	// allocate new storage
	valueM_ptr = new BYTE [aeTitle.getLength() + 1];

	// copy string
	if (valueM_ptr)
	{
		// save the length
		lengthM = aeTitle.getLength();

		// copy value and null terminate it for convenience
		byteCopy(valueM_ptr, aeTitle.get(), lengthM);
		valueM_ptr[lengthM] = NULLCHAR;

		result = true;
	}

	return result;
}

//>>===========================================================================

bool AE_TITLE_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode AET to pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode the AE title
	return pdu.writeBinary(valueM_ptr, lengthM);
}

//>>===========================================================================

bool AE_TITLE_CLASS::decode(PDU_CLASS& pdu, UINT32 length)

//  DESCRIPTION     : Decode AET from pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// allocate new storage
	valueM_ptr = new BYTE [length + 1];

	// copy UID
	if (valueM_ptr)
	{
		// save the length
		lengthM = length;

		// decode the AE title
		INT lengthIn = pdu.readBinary(valueM_ptr, lengthM);

		// on success - null terminate string for convenience
		if (lengthIn == (INT) lengthM) 
		{
			valueM_ptr[lengthM] = NULLCHAR;
			result = true;
		}
		else
		{
			// on failure - cleanup
			cleanup();
		}
	}

	return result;
}


//>>===========================================================================

UID_CLASS::UID_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
		// constructor activities
}

//>>===========================================================================

UID_CLASS::UID_CLASS(UID_CLASS &uid)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = uid;
}

//>>===========================================================================

UID_CLASS::UID_CLASS(BYTE* uid_ptr, UINT32 length)
	: BYTE_STRING_CLASS(uid_ptr, length)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

UID_CLASS::UID_CLASS(char* uid_ptr)
	: BYTE_STRING_CLASS(uid_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

UID_CLASS::~UID_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

void UID_CLASS::set(UID_CLASS& uid)

//  DESCRIPTION     : Set UID value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// copy the UID
	(*this) = uid;
}

//>>===========================================================================

bool UID_CLASS::validate()

//  DESCRIPTION     : Validate UID value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return false;
}

//>>===========================================================================

bool UID_CLASS::operator = (UID_CLASS& uid)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// first free old UID
	cleanup();

	// allocate new storage
	valueM_ptr = new BYTE [uid.getLength() + 1];

	// copy UID
	if (valueM_ptr)
	{
		// save the length
		lengthM = uid.getLength();

		// copy value and null terminate it for convenience
		byteCopy(valueM_ptr, uid.get(), lengthM);
		valueM_ptr[lengthM] = NULLCHAR;

		result = true;
	}

	return result;
}

//>>===========================================================================

bool UID_CLASS::operator == (UID_CLASS& uid)

//  DESCRIPTION     : Operator equal.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	UINT length = 0;

	// compare the UID strings for equality
	// - be careful with trailing NULL
	if (lengthM == (uid.getLength() - 1))
	{
		BYTE *uidValue_ptr = uid.get();
		if (uidValue_ptr[uid.getLength() - 1] == NULLCHAR)
		{
			length = lengthM;
			result = true;
		}
	}
	else if ((lengthM - 1) == uid.getLength())
	{
		if (valueM_ptr[lengthM - 1] == NULLCHAR)
		{
			length = uid.getLength();
			result = true;
		}
	}
	else if (lengthM == uid.getLength())
	{
		length = lengthM;
		result = true;
	}

	// OK - we now know the length to compare
	if (result)
	{
		result = byteCompare(valueM_ptr, uid.get(), length);
	}

	return result;
}

//>>===========================================================================

bool UID_CLASS::operator == (const char *uid_ptr)

//  DESCRIPTION     : Operator equal.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// make simple comparision
	return (strcmp((char*) valueM_ptr, uid_ptr) == 0) ? true : false;
}

//>>===========================================================================

bool UID_CLASS::operator != (UID_CLASS& uid)

//  DESCRIPTION     : Operator not equal.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compare the UID strings for inequality
	return !((*this) == uid);
}


