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

#ifndef ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_H
#define ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_uid.h"
#include "ACSE_role.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class SCP_SCU_ROLE_SELECT_CLASS;
class USER_INFORMATION_CLASS;
class LOG_CLASS;

//>>***************************************************************************
class ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS
//  DESCRIPTION     : SCP/SCU Role Select class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	
public:
	ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS();
	~ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS();
	
	ACSE_PARAMETER_CLASS *getUidParameter();
	ACSE_PARAMETER_CLASS *getScpRoleParameter();
	ACSE_PARAMETER_CLASS *getScuRoleParameter();

	bool validate(SCP_SCU_ROLE_SELECT_CLASS*, USER_INFORMATION_CLASS*);

private:
	ACSE_UID_CLASS	uidM;
	ACSE_ROLE_CLASS	scpRoleM;
	ACSE_ROLE_CLASS	scuRoleM;
};

#endif /* ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_H */