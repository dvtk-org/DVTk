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
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_scp_scu_role_select_validator.h"
#include "Iglobal.h"      // Global component interface file
#include "Ilog.h"         // Logging component interface file
#include "Inetwork.h"     // Network component interface file

//>>===========================================================================		

ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS::ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
} 

//>>===========================================================================		

ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS::~ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// destructor activities
} 

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS::getUidParameter()

//  DESCRIPTION     : Get UID
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &uidM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS::getScpRoleParameter()

//  DESCRIPTION     : Get SCP Role
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &scpRoleM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS::getScuRoleParameter()

//  DESCRIPTION     : Get SCU Role
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &scuRoleM; 
}

//>>===========================================================================		

bool ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS::validate(SCP_SCU_ROLE_SELECT_CLASS *srcRole_ptr, USER_INFORMATION_CLASS *refUserInfo_ptr)

//  DESCRIPTION     : Validate SCP/SCU Role Selection.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	string refUid;
	string refScpRole;
	string refScuRole;
	
	// check for valid role
	if (srcRole_ptr == NULL) return false;
	
	UID_CLASS srcUid = srcRole_ptr->getUid();
	
	// check for matching reference values
	if (refUserInfo_ptr)
	{
		for (UINT i = 0; i < refUserInfo_ptr->noScpScuRoleSelects(); i++)
		{
			SCP_SCU_ROLE_SELECT_CLASS refScpScuRole = refUserInfo_ptr->getScpScuRoleSelect(i);
			if (srcUid == refScpScuRole.getUid())
			{
				refUid = (char*) refScpScuRole.getUid().get();
				sprintf(buffer, "%d", refScpScuRole.getScpRole());
				refScpRole = buffer;
				sprintf(buffer, "%d", refScpScuRole.getScuRole());
				refScuRole = buffer;
				break;
			}
		}
	}
	
	// validate the parameters
	bool result1 = uidM.validate((char*) srcUid.get(), refUid);
	sprintf(buffer, "%d", srcRole_ptr->getScpRole());
	bool result2 = scpRoleM.validate(buffer, refScpRole);
    if (srcRole_ptr->getScpRole())
    {
        scpRoleM.setMeaning("Support of SCP role");
    }
    else
    {
        scpRoleM.setMeaning("Non-support of SCP role");
    }
	sprintf(buffer, "%d", srcRole_ptr->getScuRole());
	bool result3 = scuRoleM.validate(buffer, refScuRole);
    if (srcRole_ptr->getScuRole())
    {
        scuRoleM.setMeaning("Support of SCU role");
    }
    else
    {
        scuRoleM.setMeaning("Non-support of SCU role");
    }
	
	// set result
	bool result = true;
	if ((!result1) ||
		(!result2) ||
		(!result3))
	{
		result = false;
	}
	
	// return result
	return result;
}
