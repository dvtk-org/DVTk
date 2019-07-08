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
//  DESCRIPTION     :	Accepted (Negotiated) SOP Classes Class.
//*****************************************************************************
#ifndef ACCEPTED_H
#define ACCEPTED_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ASSOCIATE_AC_CLASS;
class ASSOCIATE_RQ_CLASS;
class LOG_CLASS;
class PRESENTATION_CONTEXT_AC_CLASS;
class UID_CLASS;


//>>***************************************************************************

class BASE_PRESENTATION_CONTEXT_CLASS

//  DESCRIPTION     : Base Presentation Context Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	ARRAY<PRESENTATION_CONTEXT_AC_CLASS>	presentationContextM;
	LOG_CLASS								*loggerM_ptr;

public:
	BASE_PRESENTATION_CONTEXT_CLASS();
	~BASE_PRESENTATION_CONTEXT_CLASS();

	bool getTransferSyntaxUid(BYTE, UID_CLASS&);

	BYTE getPresentationContextId(UID_CLASS);

	void setPresentationContextId(int, BYTE);

	bool getAbstractSyntaxName(BYTE, UID_CLASS&);

	void setLogger(LOG_CLASS *logger_ptr)
		{ loggerM_ptr = logger_ptr; }

	void clear();
};


//>>***************************************************************************

class ACCEPTED_PC_CLASS : public BASE_PRESENTATION_CONTEXT_CLASS

//  DESCRIPTION     : Accepted Presentation Context Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ACCEPTED_PC_CLASS();
	~ACCEPTED_PC_CLASS();

	void initialiseAcceptedPCs(ASSOCIATE_RQ_CLASS*);

	void updateAcceptedPCsOnSend(ASSOCIATE_AC_CLASS*);

	void updateAcceptedPCsOnReceive(ASSOCIATE_AC_CLASS*);

	void getPresentationContext(ASSOCIATE_AC_CLASS*);

    int getPresentationContextIdWithTransferSyntax(UID_CLASS, UID_CLASS);
};


//>>***************************************************************************

class SUPPORTED_PC_CLASS : public BASE_PRESENTATION_CONTEXT_CLASS

//  DESCRIPTION     : Supported Presentation Context Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	SUPPORTED_PC_CLASS();
	~SUPPORTED_PC_CLASS();

	int noPresentationContexts()
		{ return presentationContextM.getSize(); }

	void setPresentationContext(UID_CLASS, UID_CLASS);

	bool getPresentationContext(int, UID_CLASS&, UID_CLASS&);

	int isPresentationContext(UID_CLASS, UID_CLASS);

	bool isAbstractSyntaxName(UID_CLASS);
};


TS_CODE transferSyntaxUidToCode(UID_CLASS&);

TS_CODE transferSyntaxUidToCode(string);

string transferSyntaxUidToName(string uid);

#endif /* ACCEPTED_H */