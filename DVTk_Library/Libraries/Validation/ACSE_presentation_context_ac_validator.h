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

#ifndef ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_H
#define ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_pc_id.h"
#include "ACSE_uid.h"
#include "ACSE_ac_result.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class PRESENTATION_CONTEXT_AC_CLASS;
class ASSOCIATE_AC_CLASS;
class LOG_CLASS;

//>>***************************************************************************

class ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS

//  DESCRIPTION     : Presentation Context Accept class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS();
	~ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS();
	
	string getPresentationContextId();
	
	string getAbstractSyntaxName();
	
	string getResult();
	
	string getTransferSyntaxName();
	
	ACSE_PARAMETER_CLASS *getPresentationContextIdParameter();
	ACSE_PARAMETER_CLASS *getAbstractSyntaxNameParameter();
	ACSE_PARAMETER_CLASS *getResultParameter();
	ACSE_PARAMETER_CLASS *getTransferSyntaxNameParameter();

	bool validate(PRESENTATION_CONTEXT_AC_CLASS*, ASSOCIATE_AC_CLASS*);
	
    void tooManyPresentationContexts();

	void notReferenced();
	
	void notUniquePcId();
	
	void notUniqueSOP(string, string);
	
	void notSourced(PRESENTATION_CONTEXT_AC_CLASS*);
	
private:
	ACSE_PC_ID_CLASS		presentationContextIdM;
	ACSE_UID_CLASS			abstractSyntaxNameM;
	ACSE_AC_RESULT_CLASS	resultM;
	ACSE_UID_CLASS			transferSyntaxNameM;
};

#endif /* ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_H */