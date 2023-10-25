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

#pragma warning( disable : 4244 )

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "pdu_items.h"
#include "pdu.h"
#include "Idefinition.h"	// Definition component interface
#include "Iwarehouse.h"		// Warehouse component interface


//>>===========================================================================

PDU_ITEM_CLASS::~PDU_ITEM_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// empty virtual destructor
}		


//>>===========================================================================

APPLICATION_CONTEXT_NAME_CLASS::APPLICATION_CONTEXT_NAME_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_APPLICATION_CONTEXT_NAME;
	reservedM = 0;
	setUid(NULL);
}

//>>===========================================================================

APPLICATION_CONTEXT_NAME_CLASS::~APPLICATION_CONTEXT_NAME_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

void APPLICATION_CONTEXT_NAME_CLASS::setUid(UID_CLASS& uid) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid); 
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void APPLICATION_CONTEXT_NAME_CLASS::setUid(BYTE* uid_ptr, UINT32 length) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr, length); 
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void APPLICATION_CONTEXT_NAME_CLASS::setUid(char* uid_ptr) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr); 
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

bool APPLICATION_CONTEXT_NAME_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode item length and Uid value
	pdu << itemTypeM;
	pdu << reservedM;
	pdu << lengthM;
	pdu.writeBinary(uidM.get(), lengthM);

	return true;
}

//>>===========================================================================

bool APPLICATION_CONTEXT_NAME_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool APPLICATION_CONTEXT_NAME_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
	
	// allocate temporary storage
	buffer_ptr = new BYTE [lengthM];
	if (buffer_ptr) {
		// decode the item value into temporary storage
		pdu.readBinary(buffer_ptr, lengthM);

		// save item value
		uidM.set(buffer_ptr, lengthM);

		// clean up temporary storage
		delete [] buffer_ptr;

		result = true;
	}

	return result;
}

//>>===========================================================================

UINT32 APPLICATION_CONTEXT_NAME_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

void APPLICATION_CONTEXT_NAME_CLASS::updateDefaults()

//  DESCRIPTION     : Method to check if parameters are defined and if not to set
//					: the default values.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update defaults
	if (!uidM.getLength())
	{
		setUid(APPLICATION_CONTEXT_NAME);
	}
}


//>>===========================================================================

ABSTRACT_SYNTAX_NAME_CLASS::ABSTRACT_SYNTAX_NAME_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_ABSTRACT_SYNTAX_NAME;
	reservedM = 0;
	setUid(NULL);
}

//>>===========================================================================

ABSTRACT_SYNTAX_NAME_CLASS::ABSTRACT_SYNTAX_NAME_CLASS(UID_CLASS& uid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_ABSTRACT_SYNTAX_NAME;
	reservedM = 0;
	setUid(uid);
}

//>>===========================================================================

ABSTRACT_SYNTAX_NAME_CLASS::ABSTRACT_SYNTAX_NAME_CLASS(char* uid_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_ABSTRACT_SYNTAX_NAME;
	reservedM = 0;
	setUid(uid_ptr);
}

//>>===========================================================================

ABSTRACT_SYNTAX_NAME_CLASS::~ABSTRACT_SYNTAX_NAME_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

void ABSTRACT_SYNTAX_NAME_CLASS::setUid(UID_CLASS& uid) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid);
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void ABSTRACT_SYNTAX_NAME_CLASS::setUid(BYTE* uid_ptr, UINT32 length) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr, length);
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void ABSTRACT_SYNTAX_NAME_CLASS::setUid(char* uid_ptr) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr);
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

bool ABSTRACT_SYNTAX_NAME_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode item length and Uid value
	pdu << itemTypeM;
	pdu << reservedM;
	pdu << lengthM;
	pdu.writeBinary(uidM.get(), lengthM);

	return true;
}

//>>===========================================================================

bool ABSTRACT_SYNTAX_NAME_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool ABSTRACT_SYNTAX_NAME_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
	
	// allocate temporary storage
	buffer_ptr = new BYTE [lengthM];
	if (buffer_ptr) {
		// decode the item value into temporary storage
		pdu.readBinary(buffer_ptr, lengthM);

		// save item value
		uidM.set(buffer_ptr, lengthM);

		// clean up temporary storage
		delete [] buffer_ptr;

		result = true;
	}

	return result;
}

//>>===========================================================================

UINT32 ABSTRACT_SYNTAX_NAME_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

TRANSFER_SYNTAX_NAME_CLASS::TRANSFER_SYNTAX_NAME_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_TRANSFER_SYNTAX_NAME;
	reservedM = 0;
	setUid(IMPLICIT_VR_LITTLE_ENDIAN);
}

//>>===========================================================================

TRANSFER_SYNTAX_NAME_CLASS::TRANSFER_SYNTAX_NAME_CLASS(UID_CLASS& uid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_TRANSFER_SYNTAX_NAME;
	reservedM = 0;
	setUid(uid);
}

//>>===========================================================================

TRANSFER_SYNTAX_NAME_CLASS::TRANSFER_SYNTAX_NAME_CLASS(char* uid_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_TRANSFER_SYNTAX_NAME;
	reservedM = 0;
	setUid(uid_ptr);
}

//>>===========================================================================

TRANSFER_SYNTAX_NAME_CLASS::~TRANSFER_SYNTAX_NAME_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

void TRANSFER_SYNTAX_NAME_CLASS::setUid(UID_CLASS& uid) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid);
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void TRANSFER_SYNTAX_NAME_CLASS::setUid(BYTE* uid_ptr, UINT32 length) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr, length);
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void TRANSFER_SYNTAX_NAME_CLASS::setUid(char* uid_ptr) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr);
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

bool TRANSFER_SYNTAX_NAME_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode item length and Uid value
	pdu << itemTypeM;
	pdu << reservedM;
	pdu << lengthM;
	pdu.writeBinary(uidM.get(), lengthM);

	return true;
}

//>>===========================================================================

bool TRANSFER_SYNTAX_NAME_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool TRANSFER_SYNTAX_NAME_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
	
	// allocate temporary storage
	buffer_ptr = new BYTE [lengthM];
	if (buffer_ptr) {
		// decode the item value into temporary storage
		pdu.readBinary(buffer_ptr, lengthM);

		// save item value
		uidM.set(buffer_ptr, lengthM);

		// clean up temporary storage
		delete [] buffer_ptr;

		result = true;
	}

	return result;
}

//>>===========================================================================

UINT32 TRANSFER_SYNTAX_NAME_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}


//>>===========================================================================

MAXIMUM_LENGTH_RECEIVED_CLASS::MAXIMUM_LENGTH_RECEIVED_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_MAXIMUM_LENGTH;
	reservedM = 0;
	lengthM = sizeof(maximumLengthReceivedM);
	maximumLengthReceivedM = UNDEFINED_MAXIMUM_LENGTH_RECEIVED;
}

//>>===========================================================================

MAXIMUM_LENGTH_RECEIVED_CLASS::MAXIMUM_LENGTH_RECEIVED_CLASS(UINT32 length)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_MAXIMUM_LENGTH;
	reservedM = 0;
	lengthM = sizeof(maximumLengthReceivedM);
	maximumLengthReceivedM = length;
}

//>>===========================================================================

MAXIMUM_LENGTH_RECEIVED_CLASS::~MAXIMUM_LENGTH_RECEIVED_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

bool MAXIMUM_LENGTH_RECEIVED_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode the item length and value
	pdu << itemTypeM;		
	pdu << reservedM;		
	pdu << lengthM;
	pdu << maximumLengthReceivedM;
	
	return true;
}

//>>===========================================================================

bool MAXIMUM_LENGTH_RECEIVED_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool MAXIMUM_LENGTH_RECEIVED_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item length and value
	pdu >> reservedM;		
	pdu >> lengthM;			
	pdu >> maximumLengthReceivedM;	

	return true;
}

//>>===========================================================================

UINT32 MAXIMUM_LENGTH_RECEIVED_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}


//>>===========================================================================

IMPLEMENTATION_CLASS_UID_CLASS::IMPLEMENTATION_CLASS_UID_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_IMPLEMENTATION_CLASS_UID;
	reservedM = 0;
	setUid(NULL);
}

//>>===========================================================================

IMPLEMENTATION_CLASS_UID_CLASS::IMPLEMENTATION_CLASS_UID_CLASS(UID_CLASS& uid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_IMPLEMENTATION_CLASS_UID;
	reservedM = 0;
	setUid(uid);
}

//>>===========================================================================

IMPLEMENTATION_CLASS_UID_CLASS::IMPLEMENTATION_CLASS_UID_CLASS(char* uid_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_IMPLEMENTATION_CLASS_UID;
	reservedM = 0;
	setUid(uid_ptr);
}

//>>===========================================================================

IMPLEMENTATION_CLASS_UID_CLASS::~IMPLEMENTATION_CLASS_UID_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

void IMPLEMENTATION_CLASS_UID_CLASS::setUid(UID_CLASS& uid) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid); 
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void IMPLEMENTATION_CLASS_UID_CLASS::setUid(BYTE* uid_ptr, UINT32 length) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr, length); 
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

void IMPLEMENTATION_CLASS_UID_CLASS::setUid(char* uid_ptr) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr);
	lengthM = (UINT16) uidM.getLength();
}

//>>===========================================================================

bool IMPLEMENTATION_CLASS_UID_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode item length and Uid value
	pdu << itemTypeM;
	pdu << reservedM;
	pdu << lengthM;
	pdu.writeBinary(uidM.get(), lengthM);

	return true;
}

//>>===========================================================================

bool IMPLEMENTATION_CLASS_UID_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool IMPLEMENTATION_CLASS_UID_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
	
	// allocate temporary storage
	buffer_ptr = new BYTE [lengthM];
	if (buffer_ptr) {
		// decode the item value into temporary storage
		pdu.readBinary(buffer_ptr, lengthM);

		// save item value
		uidM.set(buffer_ptr, lengthM);

		// clean up temporary storage
		delete [] buffer_ptr;

		result = true;
	}

	return result;
}

//>>===========================================================================

UINT32 IMPLEMENTATION_CLASS_UID_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}


//>>===========================================================================

IMPLEMENTATION_VERSION_NAME_CLASS::IMPLEMENTATION_VERSION_NAME_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_IMPLEMENTATION_VERSION_NAME;
	reservedM = 0;
	setName("");
}

//>>===========================================================================

IMPLEMENTATION_VERSION_NAME_CLASS::IMPLEMENTATION_VERSION_NAME_CLASS(char* name_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_IMPLEMENTATION_VERSION_NAME;
	reservedM = 0;
	setName(name_ptr);
}

//>>===========================================================================

IMPLEMENTATION_VERSION_NAME_CLASS::~IMPLEMENTATION_VERSION_NAME_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

void IMPLEMENTATION_VERSION_NAME_CLASS::setName(BYTE* name_ptr, UINT32 length)

//  DESCRIPTION     : Set name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	nameM.set(name_ptr, length);
	lengthM = (UINT16) nameM.getLength();
}

//>>===========================================================================

void IMPLEMENTATION_VERSION_NAME_CLASS::setName(char* name_ptr) 

//  DESCRIPTION     : Set name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	nameM.set(name_ptr);
	lengthM = (UINT16) nameM.getLength();
}

//>>===========================================================================

bool IMPLEMENTATION_VERSION_NAME_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode item length and name value
	pdu << itemTypeM;
	pdu << reservedM;
	pdu << lengthM;
	pdu.writeBinary(nameM.get(), lengthM);

	return true;
}

//>>===========================================================================

bool IMPLEMENTATION_VERSION_NAME_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool IMPLEMENTATION_VERSION_NAME_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
	
	// allocate temporary storage
	buffer_ptr = new BYTE [lengthM];
	if (buffer_ptr) {
		// decode the item value into temporary storage
		pdu.readBinary(buffer_ptr, lengthM);

		// save item value
		nameM.set(buffer_ptr, lengthM);

		// clean up temporary storage
		delete [] buffer_ptr;

		result = true;
	}

	return result;
}

//>>===========================================================================

UINT32 IMPLEMENTATION_VERSION_NAME_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}


//>>===========================================================================

SCP_SCU_ROLE_SELECT_CLASS::SCP_SCU_ROLE_SELECT_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_SCPSCU_ROLE_SELECTION;
	reservedM = 0;
	setUid(NULL);
	scpRoleM = SCP_ROLE_SELECT;
	scuRoleM = SCU_ROLE_SELECT;
}

//>>===========================================================================

SCP_SCU_ROLE_SELECT_CLASS::~SCP_SCU_ROLE_SELECT_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

void SCP_SCU_ROLE_SELECT_CLASS::setUid(UID_CLASS& uid) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid); 
	uidLengthM = (UINT16) uidM.getLength();
	lengthM = sizeof(uidLengthM) + uidLengthM + sizeof(scpRoleM) + sizeof(scuRoleM);
}

//>>===========================================================================

void SCP_SCU_ROLE_SELECT_CLASS::setUid(BYTE* uid_ptr, UINT32 length) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr, length); 
	uidLengthM = (UINT16) uidM.getLength();
	lengthM = sizeof(uidLengthM) + uidLengthM + sizeof(scpRoleM) + sizeof(scuRoleM);
}

//>>===========================================================================

void SCP_SCU_ROLE_SELECT_CLASS::setUid(char* uid_ptr) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr); 
	uidLengthM = (UINT16) uidM.getLength();
	lengthM = sizeof(uidLengthM) + uidLengthM + sizeof(scpRoleM) + sizeof(scuRoleM);
}

//>>===========================================================================

bool SCP_SCU_ROLE_SELECT_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// encode item length, Uid and roles
	pdu << itemTypeM;	
	pdu << reservedM;	
	pdu << lengthM;		
	pdu << uidLengthM;			
	pdu.writeBinary(uidM.get(), uidLengthM);
	pdu << scuRoleM;
	pdu << scpRoleM;

	return true;
}

//>>===========================================================================

bool SCP_SCU_ROLE_SELECT_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool SCP_SCU_ROLE_SELECT_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
	pdu >> uidLengthM;
	
	// allocate temporary storage
	buffer_ptr = new BYTE [uidLengthM];
	if (buffer_ptr) {
		// decode the item value into temporary storage
		pdu.readBinary(buffer_ptr, uidLengthM);

		// save item value
		uidM.set(buffer_ptr, uidLengthM);

		// clean up temporary storage
		delete [] buffer_ptr;

		// decode the roles
		pdu >> scuRoleM;
		pdu >> scpRoleM;

		result = true;
	}

	return result;
}

//>>===========================================================================

UINT32 SCP_SCU_ROLE_SELECT_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}


//>>===========================================================================

ASYNCHRONOUS_OPERATION_WINDOW_CLASS::ASYNCHRONOUS_OPERATION_WINDOW_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_ASYNCH_OPERATIONS_WINDOW;
	reservedM = 0;
	lengthM = sizeof(operationsInvokedM) + sizeof(operationsPerformedM);
	operationsInvokedM = OPERATIONS_INVOKED;
	operationsPerformedM = OPERATIONS_PERFORMED;
}

//>>===========================================================================

ASYNCHRONOUS_OPERATION_WINDOW_CLASS::~ASYNCHRONOUS_OPERATION_WINDOW_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// nothing to de-allocate specifically
}

//>>===========================================================================

bool ASYNCHRONOUS_OPERATION_WINDOW_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode length and window operations
	pdu << itemTypeM;	
	pdu << reservedM;
	pdu << lengthM;
	pdu << operationsInvokedM;
	pdu << operationsPerformedM;

	return true;
}

//>>===========================================================================

bool ASYNCHRONOUS_OPERATION_WINDOW_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool ASYNCHRONOUS_OPERATION_WINDOW_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode length and window operations	
	pdu >> reservedM;
	pdu >> lengthM;
	pdu >> operationsInvokedM;
	pdu >> operationsPerformedM;

	return true;
}

//>>===========================================================================

UINT32 ASYNCHRONOUS_OPERATION_WINDOW_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}


//>>===========================================================================

SOP_CLASS_EXTENDED_CLASS::SOP_CLASS_EXTENDED_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_SOP_CLASS_EXTENDED_NEGOTIATION;
	setUid(NULL);
}

//>>===========================================================================

SOP_CLASS_EXTENDED_CLASS::SOP_CLASS_EXTENDED_CLASS(SOP_CLASS_EXTENDED_CLASS& sopClassExtended)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = sopClassExtended;
}

//>>===========================================================================

SOP_CLASS_EXTENDED_CLASS::~SOP_CLASS_EXTENDED_CLASS()

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

void SOP_CLASS_EXTENDED_CLASS::cleanup()

//  DESCRIPTION     : Cleanup resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// free resources
	setUid(NULL);
	while (applicationInformationM.getSize())
	{
		applicationInformationM.removeAt(0);
	}
}

//>>===========================================================================

void SOP_CLASS_EXTENDED_CLASS::setUid(UID_CLASS& uid) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid);
	uidLengthM = (UINT16) uidM.getLength();
	lengthM = (UINT16) sizeof(uidLengthM) + uidLengthM + applicationInformationM.getSize();
}

//>>===========================================================================

void SOP_CLASS_EXTENDED_CLASS::setUid(BYTE* uid_ptr, UINT32 length) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr, length); 
	uidLengthM = (UINT16) uidM.getLength();
	lengthM = (UINT16) sizeof(uidLengthM) + uidLengthM + applicationInformationM.getSize();
}

//>>===========================================================================

void SOP_CLASS_EXTENDED_CLASS::setUid(char* uid_ptr) 

//  DESCRIPTION     : Set Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	uidM.set(uid_ptr); 
	uidLengthM = (UINT16) uidM.getLength();
	lengthM = (UINT16) sizeof(uidLengthM) + uidLengthM + applicationInformationM.getSize();
}

//>>===========================================================================

void SOP_CLASS_EXTENDED_CLASS::addApplicationInformation(BYTE info)

//  DESCRIPTION     : Add application information to item.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	applicationInformationM.add(info);
	lengthM = (UINT16) sizeof(uidLengthM) + uidLengthM + applicationInformationM.getSize();
}

//>>===========================================================================

bool SOP_CLASS_EXTENDED_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode length, Uid and application information
	pdu << itemTypeM;	
	pdu << reservedM;	
	pdu << lengthM;
	pdu << uidLengthM;			
	pdu.writeBinary(uidM.get(), uidLengthM);
	for (UINT i = 0; i < applicationInformationM.getSize(); i++) 
	{
		pdu << ((BYTE) applicationInformationM[i]);
	}

	return true;
}

//>>===========================================================================

bool SOP_CLASS_EXTENDED_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool SOP_CLASS_EXTENDED_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
	pdu >> uidLengthM;
	
	// allocate temporary storage
	buffer_ptr = new BYTE [uidLengthM];
	if (buffer_ptr) {
		// decode the item value into temporary storage
		pdu.readBinary(buffer_ptr, uidLengthM);

		// save item value
		uidM.set(buffer_ptr, uidLengthM);

		// clean up temporary storage
		delete [] buffer_ptr;

		// decode the application information
		for (UINT32 i = 0; i < lengthM - (sizeof(uidLengthM) + uidLengthM); i++) 
		{
			BYTE info;
			pdu >> info;
			applicationInformationM.add(info);
		}

		result = true;
	}

	return result;
}

//>>===========================================================================

UINT32 SOP_CLASS_EXTENDED_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

bool SOP_CLASS_EXTENDED_CLASS::operator = (SOP_CLASS_EXTENDED_CLASS& sopClassExtended)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// copy individual fields
	cleanup();
	itemTypeM = sopClassExtended.itemTypeM;
	reservedM = sopClassExtended.reservedM;		
	lengthM = sopClassExtended.lengthM;
	uidLengthM = sopClassExtended.uidLengthM;
	uidM = sopClassExtended.uidM;

	// copy application information
	for (UINT i = 0; i < sopClassExtended.applicationInformationM.getSize(); i++)
	{
		addApplicationInformation(sopClassExtended.applicationInformationM[i]);
	}

	return result;
}


//>>===========================================================================

USER_IDENTITY_NEGOTIATION_CLASS::USER_IDENTITY_NEGOTIATION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_UI_USER_IDENTITY_NEGOTIATION;
	setUserIdentityType(0);
	setPositiveResponseRequested(0);
	setPrimaryField(NULL);
	setSecondaryField(NULL);
	setServerResponse(NULL);
}

//>>===========================================================================

USER_IDENTITY_NEGOTIATION_CLASS::USER_IDENTITY_NEGOTIATION_CLASS(USER_IDENTITY_NEGOTIATION_CLASS &userIdentityNegotiation)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = userIdentityNegotiation;
}

//>>===========================================================================

USER_IDENTITY_NEGOTIATION_CLASS::~USER_IDENTITY_NEGOTIATION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

void USER_IDENTITY_NEGOTIATION_CLASS::setUserIdentityType(BYTE userIdentityType)

//  DESCRIPTION     : Set User Identity Type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	uinTypeM = UIN_RQ;
	userIdentityTypeM = userIdentityType;
	lengthM = (UINT16) sizeof(userIdentityTypeM) + sizeof(positiveResponseRequestedM) +
					   sizeof(primaryFieldLengthM) + primaryFieldLengthM + 
					   sizeof(secondaryFieldLengthM) + secondaryFieldLengthM;
}

//>>===========================================================================

void USER_IDENTITY_NEGOTIATION_CLASS::setPositiveResponseRequested(BYTE positiveResponseRequested)

//  DESCRIPTION     : Set Positive Response Requested.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	uinTypeM = UIN_RQ;
	positiveResponseRequestedM = positiveResponseRequested;
	lengthM = (UINT16) sizeof(userIdentityTypeM) + sizeof(positiveResponseRequestedM) +
					   sizeof(primaryFieldLengthM) + primaryFieldLengthM + 
					   sizeof(secondaryFieldLengthM) + secondaryFieldLengthM;
}

//>>===========================================================================

void USER_IDENTITY_NEGOTIATION_CLASS::setPrimaryField(char *primaryField_ptr)

//  DESCRIPTION     : Set Primary Field.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	uinTypeM = UIN_RQ;
	primaryFieldM.set(primaryField_ptr); 
	primaryFieldLengthM = (UINT16) primaryFieldM.getLength();
	lengthM = (UINT16) sizeof(userIdentityTypeM) + sizeof(positiveResponseRequestedM) +
					   sizeof(primaryFieldLengthM) + primaryFieldLengthM + 
					   sizeof(secondaryFieldLengthM) + secondaryFieldLengthM;
}

//>>===========================================================================

void USER_IDENTITY_NEGOTIATION_CLASS::setSecondaryField(char *secondaryField_ptr)

//  DESCRIPTION     : Set Secondary Field.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	uinTypeM = UIN_RQ;
	secondaryFieldM.set(secondaryField_ptr); 
	secondaryFieldLengthM = (UINT16) secondaryFieldM.getLength();
	lengthM = (UINT16) sizeof(userIdentityTypeM) + sizeof(positiveResponseRequestedM) +
					   sizeof(primaryFieldLengthM) + primaryFieldLengthM + 
					   sizeof(secondaryFieldLengthM) + secondaryFieldLengthM;
}

//>>===========================================================================

void USER_IDENTITY_NEGOTIATION_CLASS::setServerResponse(char *serverResponse_ptr)

//  DESCRIPTION     : Set Server Response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	uinTypeM = UIN_AC;
	serverResponseM.set(serverResponse_ptr); 
	serverResponseLengthM = (UINT16) serverResponseM.getLength();
	lengthM = (UINT16) sizeof(serverResponseLengthM) + serverResponseLengthM;
}

//>>===========================================================================

BYTE USER_IDENTITY_NEGOTIATION_CLASS::getUserIdentityType()

//  DESCRIPTION     : Get User Identity Type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return userIdentityTypeM;
}

//>>===========================================================================

BYTE USER_IDENTITY_NEGOTIATION_CLASS::getPositiveResponseRequested()

//  DESCRIPTION     : Get Positive Response Requested.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return positiveResponseRequestedM;
}

//>>===========================================================================

char *USER_IDENTITY_NEGOTIATION_CLASS::getPrimaryField()

//  DESCRIPTION     : Get Primary Field.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *primaryField_ptr = NULL;

	if (primaryFieldM.getLength())
	{
		primaryField_ptr = (char*) primaryFieldM.get();
	}

	return primaryField_ptr;
}

//>>===========================================================================

char *USER_IDENTITY_NEGOTIATION_CLASS::getSecondaryField()

//  DESCRIPTION     : Get Secondary Field.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *secondaryField_ptr = NULL;

	if (secondaryFieldM.getLength())
	{
		secondaryField_ptr = (char*) secondaryFieldM.get();
	}

	return secondaryField_ptr;
}

//>>===========================================================================

char *USER_IDENTITY_NEGOTIATION_CLASS::getServerResponse()

//  DESCRIPTION     : Get Server Response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *serverResponse_ptr = NULL;

	if (serverResponseM.getLength())
	{
		serverResponse_ptr = (char*) serverResponseM.get();
	}

	return serverResponse_ptr;
}

//>>===========================================================================

bool USER_IDENTITY_NEGOTIATION_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// encode length and the appropriate user identity information
	pdu << itemTypeM;	
	pdu << reservedM;	
	pdu << lengthM;
	switch(uinTypeM)
	{
	case UIN_RQ:
		// user identity information for associate request
		pdu << userIdentityTypeM;
		pdu << positiveResponseRequestedM;
		pdu << primaryFieldLengthM;			
		if (primaryFieldLengthM > 0)
		{
			pdu.writeBinary(primaryFieldM.get(), primaryFieldLengthM);
		}
		pdu << secondaryFieldLengthM;
		if (secondaryFieldLengthM > 0)
		{
			pdu.writeBinary(secondaryFieldM.get(), secondaryFieldLengthM);
		}
		break;
	case UIN_AC:
		// user identity information for associate accept
		pdu << serverResponseLengthM;
		if (serverResponseLengthM > 0)
		{
			pdu.writeBinary(serverResponseM.get(), serverResponseLengthM);
		}
		break;
	default:
		break;
	}

	return true;
}

//>>===========================================================================

bool USER_IDENTITY_NEGOTIATION_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool USER_IDENTITY_NEGOTIATION_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE*	buffer_ptr;
	bool	result = false;

	// decode the item length
	pdu >> reservedM;
	pdu >> lengthM;
/*
	// User Identity Negotiation for an Associate Request
	uinTypeM = UIN_RQ;

	pdu >> userIdentityTypeM;
	pdu >> positiveResponseRequestedM;

	pdu >> primaryFieldLengthM;
	if (primaryFieldLengthM)
	{
		// allocate temporary storage
		buffer_ptr = new BYTE [primaryFieldLengthM];
		if (buffer_ptr) 
		{
			// decode the item value into temporary storage
			pdu.readBinary(buffer_ptr, primaryFieldLengthM);

			// save item value
			primaryFieldM.set(buffer_ptr, primaryFieldLengthM);

			// clean up temporary storage
			delete [] buffer_ptr;
		}
	}

	pdu >> secondaryFieldLengthM;

	if (secondaryFieldLengthM)
	{
		// allocate temporary storage
		buffer_ptr = new BYTE [secondaryFieldLengthM];
		if (buffer_ptr) 
		{
			// decode the item value into temporary storage
			pdu.readBinary(buffer_ptr, secondaryFieldLengthM);

			// save item value
			secondaryFieldM.set(buffer_ptr, secondaryFieldLengthM);

			// clean up temporary storage
			delete [] buffer_ptr;
		}
	}
*/
	// User Identity Negotiation for an Associate Accept
	uinTypeM = UIN_AC;

	pdu >> serverResponseLengthM;
	if (serverResponseLengthM)
	{

		// allocate temporary storage
		buffer_ptr = new BYTE [serverResponseLengthM];
		if (buffer_ptr) 
		{
			// decode the item value into temporary storage
			pdu.readBinary(buffer_ptr, serverResponseLengthM);

			// save item value
			serverResponseM.set(buffer_ptr, serverResponseLengthM);

			// clean up temporary storage
			delete [] buffer_ptr;
		}
	}

	return result;
}

//>>===========================================================================

UINT32 USER_IDENTITY_NEGOTIATION_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

bool USER_IDENTITY_NEGOTIATION_CLASS::operator = (USER_IDENTITY_NEGOTIATION_CLASS& userIdentityNegotiation)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// copy individual fields
	uinTypeM = userIdentityNegotiation.uinTypeM;
	itemTypeM = userIdentityNegotiation.itemTypeM;
	reservedM = userIdentityNegotiation.reservedM;		
	lengthM = userIdentityNegotiation.lengthM;
	userIdentityTypeM = userIdentityNegotiation.userIdentityTypeM;
	positiveResponseRequestedM = userIdentityNegotiation.positiveResponseRequestedM;
	primaryFieldLengthM = userIdentityNegotiation.primaryFieldLengthM;
	primaryFieldM = userIdentityNegotiation.primaryFieldM;
	secondaryFieldLengthM = userIdentityNegotiation.secondaryFieldLengthM;
	secondaryFieldM = userIdentityNegotiation.secondaryFieldM;
	serverResponseLengthM = userIdentityNegotiation.serverResponseLengthM;
	serverResponseM = userIdentityNegotiation.serverResponseM;

	return result;
}


//>>===========================================================================

USER_INFORMATION_CLASS::USER_INFORMATION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	itemTypeM = ITEM_USER_INFORMATION;
	reservedM = 0;
	implementationVersionNameM_ptr = NULL;
	asynchronousOperationWindowM_ptr = NULL;
	userIdentityNegotiationM_ptr = NULL;
	setSubLength();
}

//>>===========================================================================

USER_INFORMATION_CLASS::USER_INFORMATION_CLASS(USER_INFORMATION_CLASS& userInformation)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = userInformation;
	setSubLength();
}

//>>===========================================================================

USER_INFORMATION_CLASS::~USER_INFORMATION_CLASS()

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

void USER_INFORMATION_CLASS::cleanup()

//  DESCRIPTION     : Free up resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// free up the resources
	if (implementationVersionNameM_ptr) 
	{
		delete implementationVersionNameM_ptr;
	}
	implementationVersionNameM_ptr = NULL;

	if (asynchronousOperationWindowM_ptr) 
	{
		delete asynchronousOperationWindowM_ptr;
	}
	asynchronousOperationWindowM_ptr = NULL;

	while (scpScuRoleSelectM.getSize()) 
	{
		scpScuRoleSelectM.removeAt(0);
	}

	while (sopClassExtendedM.getSize()) 
	{
		sopClassExtendedM.removeAt(0);
	}

	if (userIdentityNegotiationM_ptr) 
	{
		delete userIdentityNegotiationM_ptr;
	}
	userIdentityNegotiationM_ptr = NULL;
}

//>>===========================================================================

void USER_INFORMATION_CLASS::setImplementationVersionName(BYTE* name_ptr, UINT32 length)

//  DESCRIPTION     : Set implementation version name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if the implementation version name is being defined
	if (length == 0)
	{
		if (implementationVersionNameM_ptr != NULL)
		{
			// remove current value
			delete implementationVersionNameM_ptr;
			implementationVersionNameM_ptr = NULL;
		}
	}
	else
	{
		// set the implementation version name
		if (implementationVersionNameM_ptr == NULL) 
		{
			// allocate a new implementation version name
			implementationVersionNameM_ptr = new IMPLEMENTATION_VERSION_NAME_CLASS();
		}
		implementationVersionNameM_ptr->setName(name_ptr, length);
	}
}

//>>===========================================================================

void USER_INFORMATION_CLASS::setImplementationVersionName(char* name_ptr)

//  DESCRIPTION     : Set implementation version name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if the implementation version name is being defined
	if (strlen(name_ptr) == 0)
	{
		if (implementationVersionNameM_ptr != NULL)
		{
			// remove current value
			delete implementationVersionNameM_ptr;
			implementationVersionNameM_ptr = NULL;
		}
	}
	else
	{
		// set the implementation version name
		if (implementationVersionNameM_ptr == NULL) 
		{
			// allocate a new implementation version name
			implementationVersionNameM_ptr = new IMPLEMENTATION_VERSION_NAME_CLASS();
		}
		implementationVersionNameM_ptr->setName(name_ptr);
	}
}

//>>===========================================================================

void USER_INFORMATION_CLASS::setAsynchronousOperationWindow(UINT16 invoked, UINT16 performed)

//  DESCRIPTION     : Set asynchronous operation window.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the operations window
	if (asynchronousOperationWindowM_ptr == NULL) 
	{
		// allocate a new operations window
		asynchronousOperationWindowM_ptr = new ASYNCHRONOUS_OPERATION_WINDOW_CLASS();
	}
	asynchronousOperationWindowM_ptr->setOperationsInvoked(invoked);
	asynchronousOperationWindowM_ptr->setOperationsPerformed(performed);
}

//>>===========================================================================

bool USER_INFORMATION_CLASS::getAsynchronousOperationWindow(UINT16 *invoked_ptr, UINT16 *performed_ptr)

//  DESCRIPTION     : Get asynchronous operation window.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// get the operations window
	if (asynchronousOperationWindowM_ptr) 
	{
		*invoked_ptr = asynchronousOperationWindowM_ptr->getOperationsInvoked();
		*performed_ptr = asynchronousOperationWindowM_ptr->getOperationsPerformed();
		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================

void USER_INFORMATION_CLASS::addScpScuRoleSelect(SCP_SCU_ROLE_SELECT_CLASS& scpScuRoleSelect)

//  DESCRIPTION     : Add SCP/SCU Role Select to User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// only allow one instance of for a given SOP Class
	for (UINT i = 0; i < scpScuRoleSelectM.getSize(); i++)
	{
		// check for matching SOP Class UID
		if (scpScuRoleSelect.getUid() == scpScuRoleSelectM[i].getUid())
		{
			// remove existing entry
			scpScuRoleSelectM.removeAt(i);
			break;
		}
	}

	// add the new entry
	scpScuRoleSelectM.add(scpScuRoleSelect); 
}

//>>===========================================================================

void USER_INFORMATION_CLASS::addSopClassExtended(SOP_CLASS_EXTENDED_CLASS& sopClassExtended)

//  DESCRIPTION     : Add SOP Class Extended to User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// only allow one instance of for a given SOP Class
	for (UINT i = 0; i < sopClassExtendedM.getSize(); i++)
	{
		// check for matching SOP Class UID
		if (sopClassExtended.getUid() == sopClassExtendedM[i].getUid())
		{
			// remove existing entry
			sopClassExtendedM.removeAt(i);
			break;
		}
	}

	// add the new entry
	sopClassExtendedM.add(sopClassExtended); 
}

//>>===========================================================================

void USER_INFORMATION_CLASS::setUserIdentityNegotiation(BYTE userIdentityType, BYTE positiveResponseRequested, char *primaryField_ptr)

//  DESCRIPTION     : Set User Identity Negotiation in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the user identity negotiation
	if (userIdentityNegotiationM_ptr == NULL)
	{
		// allocate a new user identity negotiation
		userIdentityNegotiationM_ptr = new USER_IDENTITY_NEGOTIATION_CLASS();
	}
	userIdentityNegotiationM_ptr->setUserIdentityType(userIdentityType);
	userIdentityNegotiationM_ptr->setPositiveResponseRequested(positiveResponseRequested);
	userIdentityNegotiationM_ptr->setPrimaryField(primaryField_ptr);
}

//>>===========================================================================

void USER_INFORMATION_CLASS::setUserIdentityNegotiation(BYTE userIdentityType, BYTE positiveResponseRequested, char *primaryField_ptr, char *secondaryField_ptr)

//  DESCRIPTION     : Set User Identity Negotiation in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the user identity negotiation
	setUserIdentityNegotiation(userIdentityType, positiveResponseRequested, primaryField_ptr);
	userIdentityNegotiationM_ptr->setSecondaryField(secondaryField_ptr);
}

//>>===========================================================================

void USER_INFORMATION_CLASS::setUserIdentityNegotiation(char *serverResponse_ptr)

//  DESCRIPTION     : Set User Identity Negotiation in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the user identity negotiation
	if (userIdentityNegotiationM_ptr == NULL)
	{
		// allocate a new user identity negotiation
		userIdentityNegotiationM_ptr = new USER_IDENTITY_NEGOTIATION_CLASS();
	}
	userIdentityNegotiationM_ptr->setServerResponse(serverResponse_ptr);
}

//>>===========================================================================

BYTE USER_INFORMATION_CLASS::getUserIdentityNegotiationUserIdentityType()

//  DESCRIPTION     : Get User Identity Negotiation User Identity Type in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE userIdentityType = 0;
	if (userIdentityNegotiationM_ptr)
	{
		userIdentityType = userIdentityNegotiationM_ptr->getUserIdentityType();
	}
	
	return userIdentityType;
}

//>>===========================================================================

BYTE USER_INFORMATION_CLASS::getUserIdentityNegotiationPositiveResponseRequested()

//  DESCRIPTION     : Get User Identity Negotiation Positive Response Requested in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE positiveResponseRequested = 0;
	if (userIdentityNegotiationM_ptr)
	{
		positiveResponseRequested = userIdentityNegotiationM_ptr->getPositiveResponseRequested();
	}
	
	return positiveResponseRequested;
}

//>>===========================================================================

char *USER_INFORMATION_CLASS::getUserIdentityNegotiationPrimaryField()

//  DESCRIPTION     : Get User Identity Negotiation Primary Field in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *primaryField_ptr = NULL;
	if (userIdentityNegotiationM_ptr)
	{
		primaryField_ptr = userIdentityNegotiationM_ptr->getPrimaryField();
	}
	
	return primaryField_ptr;
}

//>>===========================================================================

char *USER_INFORMATION_CLASS::getUserIdentityNegotiationSecondaryField()

//  DESCRIPTION     : Get User Identity Negotiation Secondary Field in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *secondaryField_ptr = NULL;
	if (userIdentityNegotiationM_ptr)
	{
		secondaryField_ptr = userIdentityNegotiationM_ptr->getSecondaryField();
	}
	
	return secondaryField_ptr;
}

//>>===========================================================================

char *USER_INFORMATION_CLASS::getUserIdentityNegotiationServerResponse()

//  DESCRIPTION     : Get User Identity Negotiation Server Response in the User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *serverResponse_ptr = NULL;
	if (userIdentityNegotiationM_ptr)
	{
		serverResponse_ptr = userIdentityNegotiationM_ptr->getServerResponse();
	}
	
	return serverResponse_ptr;
}

//>>===========================================================================

bool USER_INFORMATION_CLASS::encode(PDU_CLASS& pdu)

//  DESCRIPTION     : Encode item to PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// ensure length is valid
	setSubLength();

	// encode user information items
 	pdu << itemTypeM;	
	pdu << reservedM;	
	pdu << lengthM;		

	// encode the maximum length received
	maximumLengthReceivedM.encode(pdu);

	// encode the implementation class Uid
	implementationClassUidM.encode(pdu);

	// encode the implementation version name
	if (implementationVersionNameM_ptr) 
	{
		implementationVersionNameM_ptr->encode(pdu);
	}

	// encode asynchronous operations window
	if (asynchronousOperationWindowM_ptr) 
	{
		asynchronousOperationWindowM_ptr->encode(pdu);
	}

	// encode scp & scu role select
	for (UINT i = 0; i < scpScuRoleSelectM.getSize(); i++) 
	{
		scpScuRoleSelectM[i].encode(pdu);
	}
	
	// encode SOP class extended
	for (UINT i = 0; i < sopClassExtendedM.getSize(); i++) 
	{
		sopClassExtendedM[i].encode(pdu);
	}

	// encode user identity negotiation
	if (userIdentityNegotiationM_ptr)
	{
		userIdentityNegotiationM_ptr->encode(pdu);
	}

	return true;
}

//>>===========================================================================

bool USER_INFORMATION_CLASS::decode(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the item type
	pdu >> itemTypeM;

	// decode the remainder of the item
	return this->decodeBody(pdu);
}

//>>===========================================================================

bool USER_INFORMATION_CLASS::decodeBody(PDU_CLASS& pdu)

//  DESCRIPTION     : Decode sub-item from PDU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decode the user information items
	pdu >> reservedM;	
	pdu >> lengthM;
	
	INT32 remainingLength = lengthM;

	while (remainingLength > 0)
	{
		BYTE	itemType;

		pdu >> itemType;	
		switch(itemType)
		{
			case ITEM_UI_MAXIMUM_LENGTH: // Maximum Length Received
				maximumLengthReceivedM.decodeBody(pdu);
				remainingLength -= maximumLengthReceivedM.getLength();
				break;

			case ITEM_UI_IMPLEMENTATION_CLASS_UID: // Implementation Class Uid
				implementationClassUidM.decodeBody(pdu);
				remainingLength -= implementationClassUidM.getLength();
				break;

			case ITEM_UI_SCPSCU_ROLE_SELECTION: // SCP SCU Role Selection
				{
					SCP_SCU_ROLE_SELECT_CLASS roleSelect;
					
					roleSelect.decodeBody(pdu);
					remainingLength -= roleSelect.getLength();
					addScpScuRoleSelect(roleSelect);
				}
				break;

			case ITEM_UI_IMPLEMENTATION_VERSION_NAME: // Implementation Version Name
				implementationVersionNameM_ptr = new IMPLEMENTATION_VERSION_NAME_CLASS();

				implementationVersionNameM_ptr->decodeBody(pdu);
				remainingLength -= implementationVersionNameM_ptr->getLength();
				break;

			case ITEM_UI_ASYNCH_OPERATIONS_WINDOW:	// Asynchronous Operations Window
				asynchronousOperationWindowM_ptr = new ASYNCHRONOUS_OPERATION_WINDOW_CLASS();

				asynchronousOperationWindowM_ptr->decodeBody(pdu);
				remainingLength -= asynchronousOperationWindowM_ptr->getLength();
				break;

			case ITEM_UI_SOP_CLASS_EXTENDED_NEGOTIATION: // SOP Class Extended Negotiation
				{
					SOP_CLASS_EXTENDED_CLASS sopClassExtended;
					
					sopClassExtended.decodeBody(pdu);
					remainingLength -= sopClassExtended.getLength();
					addSopClassExtended(sopClassExtended);
				}
				break;

			case ITEM_UI_USER_IDENTITY_NEGOTIATION: // User Identity Negotiation
				{
					userIdentityNegotiationM_ptr = new USER_IDENTITY_NEGOTIATION_CLASS();

					userIdentityNegotiationM_ptr->decodeBody(pdu);
					remainingLength -= userIdentityNegotiationM_ptr->getLength();
				}
				break;

			default: // unknown item type
				{
					// skip over the unknown sub-item
					BYTE reserved;
					UINT16 length;

					// decode the item length
					pdu >> reserved;
					pdu >> length;

					// allocate temporary storage
					BYTE *buffer_ptr = new BYTE [length];
					if (buffer_ptr) 
					{
						// decode the item value into temporary storage
						pdu.readBinary(buffer_ptr, length);
	
						// clean up temporary storage
						delete [] buffer_ptr;
					}
					remainingLength -= (sizeof(reserved) + sizeof(length) + length);
				}
				break;
		}
	}

	return (remainingLength == 0) ? true : false;
}

//>>===========================================================================

UINT32 USER_INFORMATION_CLASS::getLength()

//  DESCRIPTION     : Get item length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update length
	setSubLength();

	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

UINT32 USER_INFORMATION_CLASS::getRawLength()

//  DESCRIPTION     : Get item length without recalculating the length of the
//                  : sub items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total length of the item
	return sizeof(itemTypeM) + sizeof(reservedM) + sizeof(lengthM) + lengthM;
}

//>>===========================================================================

void USER_INFORMATION_CLASS::setSubLength()

//  DESCRIPTION     : Set item sub-length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// compute the total sub-item length
	lengthM = (UINT16) maximumLengthReceivedM.getLength();

	lengthM += (UINT16) implementationClassUidM.getLength();

	if (implementationVersionNameM_ptr) 
	{
		lengthM += (UINT16) implementationVersionNameM_ptr->getLength();
	}

	if (asynchronousOperationWindowM_ptr) 
	{
		lengthM += (UINT16) asynchronousOperationWindowM_ptr->getLength();
	}

	for (UINT i = 0; i < scpScuRoleSelectM.getSize(); i++) 
	{
		lengthM += (UINT16) scpScuRoleSelectM[i].getLength();
	}
	
	for (UINT i = 0; i < sopClassExtendedM.getSize(); i++) 
	{
		lengthM += (UINT16) sopClassExtendedM[i].getLength();
	}

	if (userIdentityNegotiationM_ptr) 
	{
		lengthM += (UINT16) userIdentityNegotiationM_ptr->getLength();
	}
}

//>>===========================================================================

bool USER_INFORMATION_CLASS::operator = (USER_INFORMATION_CLASS& userInformation)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// update length
	userInformation.setSubLength();

	// copy individual fields
	itemTypeM = userInformation.itemTypeM;
	reservedM = userInformation.reservedM;		
	lengthM = userInformation.lengthM;
	maximumLengthReceivedM = userInformation.maximumLengthReceivedM;
	implementationClassUidM = userInformation.implementationClassUidM;
	implementationVersionNameM_ptr = NULL;
	asynchronousOperationWindowM_ptr = NULL;
	userIdentityNegotiationM_ptr = NULL;
	cleanup();

	// copy implementation version name
	if (userInformation.implementationVersionNameM_ptr)
	{
		setImplementationVersionName(userInformation.implementationVersionNameM_ptr->getName());
	}

	// copy scp scu role select
	for (UINT i = 0; i < userInformation.scpScuRoleSelectM.getSize(); i++)
	{
		addScpScuRoleSelect(userInformation.scpScuRoleSelectM[i]);
	}

	// copy asynchronous operations window
	if (userInformation.asynchronousOperationWindowM_ptr)
	{
		UINT16 invoked = userInformation.asynchronousOperationWindowM_ptr->getOperationsInvoked();
		UINT16 performed = userInformation.asynchronousOperationWindowM_ptr->getOperationsPerformed();
		setAsynchronousOperationWindow(invoked, performed);
	}

	// copy sop class extended
	for (UINT i = 0; i < userInformation.sopClassExtendedM.getSize(); i++)
	{
		addSopClassExtended(userInformation.sopClassExtendedM[i]);
	}

	// copy user identity negotiation
	if (userInformation.userIdentityNegotiationM_ptr)
	{
		BYTE userIdentityType = userInformation.userIdentityNegotiationM_ptr->getUserIdentityType();
		BYTE positiveResponseRequested = userInformation.userIdentityNegotiationM_ptr->getPositiveResponseRequested();
		BYTE_STRING_CLASS primaryField = userInformation.userIdentityNegotiationM_ptr->getPrimaryField();
		BYTE_STRING_CLASS secondaryField = userInformation.userIdentityNegotiationM_ptr->getSecondaryField();
		BYTE_STRING_CLASS serverResponse = userInformation.userIdentityNegotiationM_ptr->getServerResponse();
		
		if (primaryField.getLength())
		{
			if (secondaryField.getLength())
			{
				setUserIdentityNegotiation(userIdentityType, positiveResponseRequested, (char*)primaryField.get(), (char*)secondaryField.get());
			}
			else
			{
				setUserIdentityNegotiation(userIdentityType, positiveResponseRequested, (char*)primaryField.get());
			}
		}
		else if (serverResponse.getLength())
		{
			setUserIdentityNegotiation((char*)serverResponse.get());
		}
	}

	// update copied length
	setSubLength();

	return result;
}

//>>===========================================================================

bool USER_INFORMATION_CLASS::update(USER_INFORMATION_CLASS& updateUserInformation)

//  DESCRIPTION     : Update this user information with the contents of the update
//					: user information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update this user information
	// - maximum length received
	if (updateUserInformation.getMaximumLengthReceived() != UNDEFINED_MAXIMUM_LENGTH_RECEIVED)
	{
		setMaximumLengthReceived(updateUserInformation.getMaximumLengthReceived());
	}

	// - implementation class uid
	UID_CLASS implementationClassUid = updateUserInformation.getImplementationClassUid();
	if (implementationClassUid.getLength())
	{
		setImplementationClassUid(implementationClassUid);
	}

	// - implementation version name
	if (updateUserInformation.getImplementationVersionName())
	{
		// set the implementation version name
		setImplementationVersionName(updateUserInformation.getImplementationVersionName());
	}

	// - scp scu role select
	for (UINT i = 0; i < updateUserInformation.noScpScuRoleSelects(); i++)
	{
		// set the value straight away
		addScpScuRoleSelect(updateUserInformation.getScpScuRoleSelect(i));
	}

	// - asynchronous operation window
	UINT16 invoked, performed;
	if (updateUserInformation.getAsynchronousOperationWindow(&invoked, &performed))
	{
		// set values straight away
		setAsynchronousOperationWindow(invoked, performed);
	}

	// - sop class extended
	for (UINT i = 0; i < updateUserInformation.noSopClassExtendeds(); i++)
	{
		// set the value straight away
		addSopClassExtended(updateUserInformation.getSopClassExtended(i));
	}

	// - user identity information
	if (updateUserInformation.userIdentityNegotiationM_ptr)
	{
		BYTE userIdentityType = updateUserInformation.userIdentityNegotiationM_ptr->getUserIdentityType();
		BYTE positiveResponseRequested = updateUserInformation.userIdentityNegotiationM_ptr->getPositiveResponseRequested();
		BYTE_STRING_CLASS primaryField = updateUserInformation.userIdentityNegotiationM_ptr->getPrimaryField();
		BYTE_STRING_CLASS secondaryField = updateUserInformation.userIdentityNegotiationM_ptr->getSecondaryField();
		BYTE_STRING_CLASS serverResponse = updateUserInformation.userIdentityNegotiationM_ptr->getServerResponse();
		
		if (primaryField.getLength())
		{
			if (secondaryField.getLength())
			{
				setUserIdentityNegotiation(userIdentityType, positiveResponseRequested, (char*)primaryField.get(), (char*)secondaryField.get());
			}
			else
			{
				setUserIdentityNegotiation(userIdentityType, positiveResponseRequested, (char*)primaryField.get());
			}
		}
		else if (serverResponse.getLength())
		{
			setUserIdentityNegotiation((char*)serverResponse.get());
		}
	}

	// return result
	return true;
}

//>>===========================================================================

void USER_INFORMATION_CLASS::updateDefaults()

//  DESCRIPTION     : Method to check if parameters are defined and if not use the
//					: default values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// update defaults
	// - maximum length received
	//if (getMaximumLengthReceived() != UNDEFINED_MAXIMUM_LENGTH_RECEIVED)
    if (getMaximumLengthReceived() == UNDEFINED_MAXIMUM_LENGTH_RECEIVED)
	{
		setMaximumLengthReceived(MAXIMUM_LENGTH_RECEIVED);
	}

	// - implementation class uid
	IMPLEMENTATION_CLASS_UID_CLASS implementationClassUid = getImplementationClassUid();
	UID_CLASS uid = implementationClassUid.getUid();
	if (!uid.getLength())
	{
		setImplementationClassUid(IMPLEMENTATION_CLASS_UID);
	}
}
