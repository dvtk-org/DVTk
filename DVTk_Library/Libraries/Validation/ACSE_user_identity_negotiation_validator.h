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

#ifndef ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_H
#define ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_operation.h"
#include "ACSE_user_identity_type.h"
#include "ACSE_positive_response_requested.h"
#include "ACSE_primary_field.h"
#include "ACSE_secondary_field.h"
#include "ACSE_server_response.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class USER_INFORMATION_CLASS;
class LOG_CLASS;

//>>***************************************************************************

class ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS

//  DESCRIPTION     : User Identity Negotiation class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS();
	~ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS();

	ACSE_PARAMETER_CLASS *getUserIdentityTypeParameter();

	ACSE_PARAMETER_CLASS *getPositiveResponseRequestedParameter();

	ACSE_PARAMETER_CLASS *getPrimaryFieldParameter();

	ACSE_PARAMETER_CLASS *getSecondaryFieldParameter();

	ACSE_PARAMETER_CLASS *getServerResponseParameter();

	bool validate(BYTE, BYTE, char*, char*, USER_INFORMATION_CLASS*);
	bool validate(char*, USER_INFORMATION_CLASS*);

private:
	ACSE_USER_IDENTITY_TYPE_CLASS			userIdentityTypeM;
	ACSE_POSITIVE_RESPONSE_REQUESTED_CLASS	positiveResponseRequestedM;
	ACSE_PRIMARY_FIELD_CLASS				primaryFieldM;
	ACSE_SECONDARY_FIELD_CLASS				secondaryFieldM;
	ACSE_SERVER_RESPONSE_CLASS				serverResponseM;
};

#endif /* ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_H */