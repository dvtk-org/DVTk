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

#ifndef ACSE_SOP_CLASS_EXTENDED_VALIDATOR_H
#define ACSE_SOP_CLASS_EXTENDED_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_uid.h"
#include "ACSE_byte.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class SOP_CLASS_EXTENDED_CLASS;
class USER_INFORMATION_CLASS;
class LOG_CASS;

//>>***************************************************************************
class ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS
//  DESCRIPTION     : SOP Class Extended class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS();
	ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS(ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS&);
	~ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS();
	
	ACSE_PARAMETER_CLASS *getUidParameter();

	UINT noAapplicationInformations();
	ACSE_PARAMETER_CLASS *getAapplicationInformationParameter(UINT);

	bool operator = (ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS&);
	
	bool validate(SOP_CLASS_EXTENDED_CLASS*, USER_INFORMATION_CLASS*);

private:
	ACSE_UID_CLASS			uidM;
	ARRAY<ACSE_BYTE_CLASS>	applicationInformationM;
};

#endif /* ACSE_SOP_CLASS_EXTENDED_VALIDATOR_H */