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

#ifndef ACSE_ASSOCIATE_ACCEPT_VALIDATOR_H
#define ACSE_ASSOCIATE_ACCEPT_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_protocol_version.h"
#include "ACSE_ae_title.h"
#include "ACSE_appl_ctx_name.h"
#include "ACSE_presentation_context_ac_validator.h"
#include "ACSE_user_information_validator.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ASSOCIATE_AC_CLASS;
class PRESENTATION_CONTEXT_AC_CLASS;
class ACSE_PROPERTIES_CLASS;
class LOG_CLASS;

//>>***************************************************************************
class ASSOCIATE_AC_VALIDATOR_CLASS
//  DESCRIPTION     : Associate Accept validation class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ASSOCIATE_AC_VALIDATOR_CLASS();
	~ASSOCIATE_AC_VALIDATOR_CLASS();
	
	ACSE_PARAMETER_CLASS *getProtocolVersionParameter();
	ACSE_PARAMETER_CLASS *getCalledAeTitleParameter();
	ACSE_PARAMETER_CLASS *getCallingAeTitleParameter();

	ACSE_APPL_CTX_NAME_CLASS& getApplicationContextName();

	UINT noPresentationContexts();	
	ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS& getPresentationContext(UINT);

	ACSE_USER_INFORMATION_VALIDATOR_CLASS& getUserInformation();
	
	bool validate(ASSOCIATE_AC_CLASS * srcAssocAc_ptr,
		ASSOCIATE_AC_CLASS * refAssocAc_ptr,
		ACSE_PROPERTIES_CLASS * acseProp_ptr);
	
private:
	ACSE_PROTOCOL_VERSION_CLASS							protocolVersionM;
	ACSE_AE_TITLE_CLASS									calledAeTitleM;
	ACSE_AE_TITLE_CLASS									callingAeTitleM;
	ACSE_APPL_CTX_NAME_CLASS							applicationContextNameM;
	ARRAY<ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS>	presentationContextM;
	bool												usedPcIdM[MAX_PC_ID];
	ACSE_USER_INFORMATION_VALIDATOR_CLASS				userInformationM;
	
	void setUnvalidatedFlags(ASSOCIATE_AC_CLASS*, ASSOCIATE_AC_CLASS*, bool);
	
	bool checkIfValidated(ASSOCIATE_AC_CLASS*, ASSOCIATE_AC_CLASS*);
	
	void updatePresentationContext(PRESENTATION_CONTEXT_AC_CLASS*);
	
	bool checkUniquePresentationContextId();
	
	bool checkUniqueSOPClasses(void);
};

#endif /* ACSE_ASSOCIATE_ACCEPT_VALIDATOR_H */