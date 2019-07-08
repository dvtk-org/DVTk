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

#ifndef PDU_ITEMS_H
#define PDU_ITEMS_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"        // Global component interface
#include "vr.h"             // Value Representation


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class PDU_CLASS;


//>>***************************************************************************

class PDU_ITEM_CLASS

//  DESCRIPTION     : PDU Item Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	BYTE	itemTypeM;
	BYTE	reservedM;

	virtual	~PDU_ITEM_CLASS() = 0;

	virtual bool encode(PDU_CLASS&)
		{ return false; }

	virtual bool decode(PDU_CLASS&)
		{ return false; }
	virtual bool decodeBody(PDU_CLASS&)
		{ return false; }

	virtual UINT32 getLength() = 0;
};


//>>***************************************************************************

class APPLICATION_CONTEXT_NAME_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Application Context Name Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16		lengthM;
	UID_CLASS	uidM;

public:
	APPLICATION_CONTEXT_NAME_CLASS();

	~APPLICATION_CONTEXT_NAME_CLASS();

	void setUid(UID_CLASS&);
	void setUid(BYTE*, UINT32); 
	void setUid(char*);

	bool isUidValid()
		{ return uidM.isValid(); }

	UID_CLASS& getUid()
		{ return uidM; }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();

	void updateDefaults();
};


//>>***************************************************************************

class ABSTRACT_SYNTAX_NAME_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Abstract Syntax Name Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16		lengthM;
	UID_CLASS	uidM;

public:
	ABSTRACT_SYNTAX_NAME_CLASS();
	ABSTRACT_SYNTAX_NAME_CLASS(UID_CLASS&);
	ABSTRACT_SYNTAX_NAME_CLASS(char*);

	~ABSTRACT_SYNTAX_NAME_CLASS();

	void setUid(UID_CLASS&);
	void setUid(BYTE*, UINT32);
	void setUid(char*);

	UID_CLASS& getUid()
		{ return uidM; }

	bool isUidValid()
		{ return uidM.isValid(); }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
};


//>>***************************************************************************

class TRANSFER_SYNTAX_NAME_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Transfer Syntax Name Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16		lengthM;
	UID_CLASS	uidM;

public:
	TRANSFER_SYNTAX_NAME_CLASS();
	TRANSFER_SYNTAX_NAME_CLASS(UID_CLASS&);
	TRANSFER_SYNTAX_NAME_CLASS(char*);

	~TRANSFER_SYNTAX_NAME_CLASS();

	void setUid(UID_CLASS&);
	void setUid(BYTE*, UINT32);
	void setUid(char*);

	UID_CLASS& getUid()
		{ return uidM; }

	bool isUidValid()
		{ return uidM.isValid(); }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
};


//>>***************************************************************************

class MAXIMUM_LENGTH_RECEIVED_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Maximum Length Received Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16	lengthM;
	UINT32	maximumLengthReceivedM;

public:
	MAXIMUM_LENGTH_RECEIVED_CLASS();
	MAXIMUM_LENGTH_RECEIVED_CLASS(UINT32);

	~MAXIMUM_LENGTH_RECEIVED_CLASS();

	void setMaximumLengthReceived(UINT32 length)
		{ maximumLengthReceivedM = length; }

	UINT32 getMaximumLengthReceived()
		{ return maximumLengthReceivedM; }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
};


//>>***************************************************************************

class IMPLEMENTATION_CLASS_UID_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Implementation Class UID Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16		lengthM;
	UID_CLASS	uidM;

public:
	IMPLEMENTATION_CLASS_UID_CLASS();
	IMPLEMENTATION_CLASS_UID_CLASS(UID_CLASS&);
	IMPLEMENTATION_CLASS_UID_CLASS(char*);

	~IMPLEMENTATION_CLASS_UID_CLASS();

	void setUid(UID_CLASS&);
	void setUid(BYTE*, UINT32);
	void setUid(char*);

	bool isUidValid()
		{ return uidM.isValid(); }

	UID_CLASS& getUid()
		{ return uidM; }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
};


//>>***************************************************************************

class IMPLEMENTATION_VERSION_NAME_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Implementation Version Name Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16				lengthM;
	BYTE_STRING_CLASS	nameM;

public:
	IMPLEMENTATION_VERSION_NAME_CLASS();
	IMPLEMENTATION_VERSION_NAME_CLASS(char*);

	~IMPLEMENTATION_VERSION_NAME_CLASS();

	void setName(BYTE*, UINT32);
	void setName(char*);

	char* getName()
		{ return (char*) nameM.get(); }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
};


//>>***************************************************************************

class SCP_SCU_ROLE_SELECT_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : SCP SCU Role Select Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16		lengthM;
	UINT16		uidLengthM;
	UID_CLASS	uidM;
	BYTE		scpRoleM;
	BYTE		scuRoleM;

public:
	SCP_SCU_ROLE_SELECT_CLASS();

	~SCP_SCU_ROLE_SELECT_CLASS();

	void setUid(UID_CLASS&);
	void setUid(BYTE*, UINT32);
	void setUid(char*);

	bool isUidValid()
		{ return uidM.isValid(); }

	UID_CLASS& getUid()
		{ return uidM; }

	void setScpRole(BYTE role) 
		{ scpRoleM = role; }

	void setScuRole(BYTE role)
		{ scuRoleM = role; }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
	BYTE   getScpRole() 
		{ return scpRoleM; }

	BYTE   getScuRole()
		{ return scuRoleM; }
};


//>>***************************************************************************

class ASYNCHRONOUS_OPERATION_WINDOW_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Asynchronous Operation Window Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16	lengthM;
	UINT16	operationsInvokedM;
	UINT16	operationsPerformedM;

public:
	ASYNCHRONOUS_OPERATION_WINDOW_CLASS();

	~ASYNCHRONOUS_OPERATION_WINDOW_CLASS();

	void setOperationsInvoked(UINT16 operations)
		{ operationsInvokedM = operations; }

	UINT16 getOperationsInvoked()
		{ return operationsInvokedM; }

	void setOperationsPerformed(UINT16 operations)
		{ operationsPerformedM = operations; }

	UINT16 getOperationsPerformed()
		{ return operationsPerformedM; }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
};


//>>***************************************************************************

class SOP_CLASS_EXTENDED_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : SOP Class Extended Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16		lengthM;
	UINT16		uidLengthM;
	UID_CLASS	uidM;
	ARRAY<BYTE>	applicationInformationM;

public:
	SOP_CLASS_EXTENDED_CLASS();
	SOP_CLASS_EXTENDED_CLASS(SOP_CLASS_EXTENDED_CLASS&);

	~SOP_CLASS_EXTENDED_CLASS();

	void cleanup();

	void setUid(UID_CLASS&);
	void setUid(BYTE*, UINT32);
	void setUid(char*);

	bool isUidValid()
		{ return uidM.isValid(); }
	
	UID_CLASS& getUid()
		{ return uidM; }

	void addApplicationInformation(BYTE);

	UINT getNoApplicationInformation()
		{ return applicationInformationM.getSize(); }

	BYTE getApplicationInformation(UINT i)
		{ return applicationInformationM[i]; }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();

	bool operator = (SOP_CLASS_EXTENDED_CLASS&);
};


//
// Enum required to determine if the USER_IDENTITY_NEGOTIATION_CLASS belongs to an
// Associate Request or an Associate Accept.
//
enum USER_IDENTITY_NEGOTIATION_TYPE
{
   UIN_RQ,	// User Identity Negotiation for Associate Request
   UIN_AC	// User Identity Negotiation for Associate Accept
};


//>>***************************************************************************

class USER_IDENTITY_NEGOTIATION_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : User Identity Negotiation Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	USER_IDENTITY_NEGOTIATION_TYPE	uinTypeM;
	UINT16							lengthM;
	BYTE							userIdentityTypeM;
	BYTE							positiveResponseRequestedM;
	UINT16							primaryFieldLengthM;
	BYTE_STRING_CLASS				primaryFieldM;
	UINT16							secondaryFieldLengthM;
	BYTE_STRING_CLASS				secondaryFieldM;
	UINT16							serverResponseLengthM;
	BYTE_STRING_CLASS				serverResponseM;

public:
	USER_IDENTITY_NEGOTIATION_CLASS();
	USER_IDENTITY_NEGOTIATION_CLASS(USER_IDENTITY_NEGOTIATION_CLASS&);

	~USER_IDENTITY_NEGOTIATION_CLASS();

	void setUserIdentityType(BYTE);

	void setPositiveResponseRequested(BYTE);

	void setPrimaryField(char*);

	void setSecondaryField(char*);

	void setServerResponse(char*);

	BYTE getUserIdentityType();

	BYTE getPositiveResponseRequested();

	char *getPrimaryField();

	char *getSecondaryField();

	char *getServerResponse();

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();

	bool operator = (USER_IDENTITY_NEGOTIATION_CLASS&);
};


//>>***************************************************************************

class USER_INFORMATION_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : User Information Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16									lengthM;
	MAXIMUM_LENGTH_RECEIVED_CLASS			maximumLengthReceivedM;
	IMPLEMENTATION_CLASS_UID_CLASS			implementationClassUidM;
	IMPLEMENTATION_VERSION_NAME_CLASS*		implementationVersionNameM_ptr;
	ARRAY<SCP_SCU_ROLE_SELECT_CLASS>		scpScuRoleSelectM;
	ASYNCHRONOUS_OPERATION_WINDOW_CLASS*	asynchronousOperationWindowM_ptr;
	ARRAY<SOP_CLASS_EXTENDED_CLASS>			sopClassExtendedM;
	USER_IDENTITY_NEGOTIATION_CLASS*		userIdentityNegotiationM_ptr;

	void setSubLength();

public:
	USER_INFORMATION_CLASS();
	USER_INFORMATION_CLASS(USER_INFORMATION_CLASS&);

	~USER_INFORMATION_CLASS();

	void cleanup();

	void setMaximumLengthReceived(UINT32 length)
		{ maximumLengthReceivedM.setMaximumLengthReceived(length); }

	void setImplementationClassUid(UID_CLASS& uid)
		{ implementationClassUidM.setUid(uid); }
	void setImplementationClassUid(BYTE* uid_ptr, UINT32 length)
		{ implementationClassUidM.setUid(uid_ptr, length); }
	void setImplementationClassUid(char* uid_ptr)
		{ implementationClassUidM.setUid(uid_ptr); }

	void setImplementationVersionName(BYTE*, UINT32);
	void setImplementationVersionName(char*);

	void setAsynchronousOperationWindow(UINT16, UINT16);

	UINT32 getMaximumLengthReceived()
		{ return maximumLengthReceivedM.getMaximumLengthReceived(); }

	UID_CLASS& getImplementationClassUid()
		{ return implementationClassUidM.getUid(); }

	char *getImplementationVersionName()
	{
		char* name_ptr = NULL;
		if (implementationVersionNameM_ptr)
		{
			name_ptr = implementationVersionNameM_ptr->getName();
		}

		return name_ptr;
	}

	bool getAsynchronousOperationWindow(UINT16*, UINT16*);

	void addScpScuRoleSelect(SCP_SCU_ROLE_SELECT_CLASS&);

	UINT noScpScuRoleSelects()
		{ return scpScuRoleSelectM.getSize(); }

	SCP_SCU_ROLE_SELECT_CLASS& getScpScuRoleSelect(UINT i)
		{ return scpScuRoleSelectM[i]; }

	void addSopClassExtended(SOP_CLASS_EXTENDED_CLASS&);

	UINT noSopClassExtendeds()
		{ return sopClassExtendedM.getSize(); }

	SOP_CLASS_EXTENDED_CLASS& getSopClassExtended(UINT i)
		{ return sopClassExtendedM[i]; }

	void setUserIdentityNegotiation(BYTE, BYTE, char*);
	void setUserIdentityNegotiation(BYTE, BYTE, char*, char*);
	void setUserIdentityNegotiation(char*);

	BYTE getUserIdentityNegotiationUserIdentityType();
	BYTE getUserIdentityNegotiationPositiveResponseRequested();
	char *getUserIdentityNegotiationPrimaryField();
	char *getUserIdentityNegotiationSecondaryField();
	char *getUserIdentityNegotiationServerResponse();

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();
	UINT32 getRawLength();

	bool operator = (USER_INFORMATION_CLASS&);

	bool update(USER_INFORMATION_CLASS&);

	void updateDefaults();
};


#endif /* PDU_ITEMS_H */
