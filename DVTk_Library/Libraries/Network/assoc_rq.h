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

#ifndef ASSOC_RQ_H
#define ASSOC_RQ_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"        // Global component interface
#include "Iwarehouse.h"     // Warehouse component interface
#include "pdu_items.h"      // PDU Items


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class PDU_CLASS;


//>>***************************************************************************

class PRESENTATION_CONTEXT_RQ_CLASS : public PDU_ITEM_CLASS

//  DESCRIPTION     : Presentation Context Request Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16								lengthM;
	BYTE								presentationContextIdM;
	BYTE								reserved1M;
	BYTE								reserved2M;
	BYTE								reserved3M;
	ABSTRACT_SYNTAX_NAME_CLASS			abstractSyntaxNameM;
	ARRAY<TRANSFER_SYNTAX_NAME_CLASS>	transferSyntaxNameM;

	void setSubLength();

public:
	PRESENTATION_CONTEXT_RQ_CLASS();
	PRESENTATION_CONTEXT_RQ_CLASS(PRESENTATION_CONTEXT_RQ_CLASS&);
	PRESENTATION_CONTEXT_RQ_CLASS(ABSTRACT_SYNTAX_NAME_CLASS&, TRANSFER_SYNTAX_NAME_CLASS&);

	~PRESENTATION_CONTEXT_RQ_CLASS();

	void cleanup();

	void setReserved1(BYTE value)
		{ reserved1M = value; }

	void setPresentationContextId(BYTE id)
		{ presentationContextIdM = id; }

	void setAbstractSyntaxName(ABSTRACT_SYNTAX_NAME_CLASS& name)
		{ abstractSyntaxNameM = name; }
	void setAbstractSyntaxName(UID_CLASS &uid)
		{ ABSTRACT_SYNTAX_NAME_CLASS name(uid);
		  abstractSyntaxNameM = name; }

	void addTransferSyntaxName(TRANSFER_SYNTAX_NAME_CLASS& name)
		{ transferSyntaxNameM.add(name); }
	void addTransferSyntaxName(UID_CLASS &uid)
		{ TRANSFER_SYNTAX_NAME_CLASS name(uid);
		  transferSyntaxNameM.add(name); }

	BYTE getReserved1()
		{ return reserved1M; }

	BYTE getPresentationContextId()
		{ return presentationContextIdM; }

	UID_CLASS& getAbstractSyntaxName()
		{ return abstractSyntaxNameM.getUid(); }

	UINT noTransferSyntaxNames()
		{ return transferSyntaxNameM.getSize(); }

	UID_CLASS& getTransferSyntaxName(UINT i)
		{ return transferSyntaxNameM[i].getUid(); }

	bool isTransferSyntaxName(UID_CLASS);

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);
	bool decodeBody(PDU_CLASS&);

	UINT32 getLength();

	bool operator = (PRESENTATION_CONTEXT_RQ_CLASS&);
	bool operator == (PRESENTATION_CONTEXT_RQ_CLASS&);

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);
};


//>>***************************************************************************

class ASSOCIATE_RQ_CLASS : public PDU_ITEM_CLASS, public BASE_WAREHOUSE_ITEM_DATA_CLASS

//  DESCRIPTION     : Associate Request Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT32									lengthM;
	UINT16									protocolVersionM;
	UINT16									reserved1M;
	AE_TITLE_CLASS							calledAeTitleM;
	AE_TITLE_CLASS							callingAeTitleM;
	BYTE									reserved2M[32];
	APPLICATION_CONTEXT_NAME_CLASS			applicationContextNameM;
	ARRAY<PRESENTATION_CONTEXT_RQ_CLASS>	presentationContextM;
	USER_INFORMATION_CLASS					userInformationM;

	void setSubLength();

public:
	ASSOCIATE_RQ_CLASS();
	ASSOCIATE_RQ_CLASS(char*, char*);

	~ASSOCIATE_RQ_CLASS();		

	void setProtocolVersion(UINT16 protocolVersion)
		{ protocolVersionM = protocolVersion; }

	void setCalledAeTitle(char*);

	void setCallingAeTitle(char*);

	void setApplicationContextName(UID_CLASS& uid)
		{ applicationContextNameM.setUid(uid); }
	void setApplicationContextName(BYTE* uid_ptr, UINT32 length)
		{ applicationContextNameM.setUid(uid_ptr, length); }
	void setApplicationContextName(char* uid_ptr)
		{ applicationContextNameM.setUid(uid_ptr); }

	UINT16 getProtocolVersion()
		{ return protocolVersionM; }

	char* getCalledAeTitle()
		{ return (char*) calledAeTitleM.get(); }

	char* getCallingAeTitle()
		{ return (char*) callingAeTitleM.get(); }

	UID_CLASS& getApplicationContextName()
		{ return applicationContextNameM.getUid(); }

	void addPresentationContext(PRESENTATION_CONTEXT_RQ_CLASS& context)
		{ presentationContextM.add(context); }

	UINT noPresentationContexts()
		{ return presentationContextM.getSize(); }

	PRESENTATION_CONTEXT_RQ_CLASS& getPresentationContext(UINT i)
		{ return presentationContextM[i]; }

	void deletePresentationContextId(UINT i)
		{ presentationContextM.removeAt(i); }

	void sortPresentationContexts();

	void setUserInformation(USER_INFORMATION_CLASS& user_information)
		{ userInformationM = user_information; }

	USER_INFORMATION_CLASS& getUserInformation()
		{ return userInformationM; }

	void setMaximumLengthReceived(UINT32 maximumLengthReceived)
		{ userInformationM.setMaximumLengthReceived(maximumLengthReceived); }

	UINT32 getMaximumLengthReceived()
		{ return userInformationM.getMaximumLengthReceived(); }

	void setImplementationClassUid(char *implementationClassUid_ptr)
		{ userInformationM.setImplementationClassUid(implementationClassUid_ptr); }

	char *getImplementationClassUid()
		{ 
			char *uid_ptr = NULL;
			UID_CLASS uid = userInformationM.getImplementationClassUid();
			if (uid.getLength())
			{
				uid_ptr = (char*) uid.get();
			}
			return uid_ptr;
		}

	void setImplementationVersionName(char *implementationVersionName_ptr)
		{ userInformationM.setImplementationVersionName(implementationVersionName_ptr); }

	char *getImplementationVersionName()
		{ return userInformationM.getImplementationVersionName(); }

	void setScpScuRoleSelect(UID_CLASS&, BYTE, BYTE);

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);

	UINT32 getLength();

	bool checkPresentationContextIds();

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);

	void updateDefaults();

	bool updatePresentationContext(ASSOCIATE_RQ_CLASS*);
};

#endif /* ASSOC_RQ_H */
