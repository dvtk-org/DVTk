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
#include "ACSE_user_information_validator.h"
#include "Iglobal.h"      // Global component interface file
#include "Ilog.h"         // Logging component interface file
#include "Inetwork.h"     // Network component interface file
#include "ACSE_implementation_version_name.h"
#include "ACSE_properties.h"

//>>===========================================================================		

ACSE_USER_INFORMATION_VALIDATOR_CLASS::ACSE_USER_INFORMATION_VALIDATOR_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
	implementationVersionNameM_ptr = NULL;
	asynchronousOperationWindowM_ptr = NULL;
	userIdentityNegotiationM_ptr = NULL;
} 

//>>===========================================================================		

ACSE_USER_INFORMATION_VALIDATOR_CLASS::~ACSE_USER_INFORMATION_VALIDATOR_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// destructor activities
	if (implementationVersionNameM_ptr)
	{
		delete implementationVersionNameM_ptr;
	}
	
	while (scpScuRoleSelectM.getSize())
	{
		scpScuRoleSelectM.removeAt(0);
	}
	
	if (asynchronousOperationWindowM_ptr)
	{
		delete asynchronousOperationWindowM_ptr;
	}
	
	while (sopClassExtendedM.getSize())
	{
		sopClassExtendedM.removeAt(0);
	}

	if (userIdentityNegotiationM_ptr)
	{
		delete userIdentityNegotiationM_ptr;
	}
} 

//>>===========================================================================		

UINT ACSE_USER_INFORMATION_VALIDATOR_CLASS::noScpScuRoleSelects()

//  DESCRIPTION     : Get number of SCP SCU Role Selects
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return scpScuRoleSelectM.getSize();
}

//>>===========================================================================		

ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS& ACSE_USER_INFORMATION_VALIDATOR_CLASS::getScpScuRoleSelect(UINT i)

//  DESCRIPTION     : Get SCP SCU Role Select
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return scpScuRoleSelectM[i];
}

//>>===========================================================================		

UINT ACSE_USER_INFORMATION_VALIDATOR_CLASS::noSopClassExtendeds()

//  DESCRIPTION     : Get number of SOP Class Extendeds
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return sopClassExtendedM.getSize();
}

//>>===========================================================================		

ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS& ACSE_USER_INFORMATION_VALIDATOR_CLASS::getSopClassExtended(UINT i)

//  DESCRIPTION     : Get SOP Class Extended
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return sopClassExtendedM[i];
}

//>>===========================================================================		

ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS *ACSE_USER_INFORMATION_VALIDATOR_CLASS::getAsynchronousOperationWindow()

//  DESCRIPTION     : Get Asynchronous Operations Window
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return asynchronousOperationWindowM_ptr;
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_USER_INFORMATION_VALIDATOR_CLASS::getMaximumLengthReceivedParameter()

//  DESCRIPTION     : Get Maximum Length Received
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &maximumLengthReceivedM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_USER_INFORMATION_VALIDATOR_CLASS::getImplementationClassUidParameter()

//  DESCRIPTION     : Get Implementation Class UID
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &implementationClassUidM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_USER_INFORMATION_VALIDATOR_CLASS::getImplementationVersionNameParameter()

//  DESCRIPTION     : Get Implementation Version Name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return implementationVersionNameM_ptr;
}

//>>===========================================================================		

ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS *ACSE_USER_INFORMATION_VALIDATOR_CLASS::getUserIdentityNegotiation()

//  DESCRIPTION     : Get User Identity Negotiation
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	return userIdentityNegotiationM_ptr;
}

//>>===========================================================================		

bool ACSE_USER_INFORMATION_VALIDATOR_CLASS::validate(USER_INFORMATION_CLASS *srcUserInfo_ptr,
													 USER_INFORMATION_CLASS *refUserInfo_ptr,
													 ACSE_PROPERTIES_CLASS *acseProp_ptr)
													 
//  DESCRIPTION     : Validate User Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	string refMaximumLengthReceived;
	string refImplementationClassUid;
	string refImplementationVersionName;
	
	// check for valid source
	if (srcUserInfo_ptr == NULL) return false;
	
	// check if reference provided
	if (refUserInfo_ptr)
	{
		// set up reference values
		if (refUserInfo_ptr->getMaximumLengthReceived() != UNDEFINED_MAXIMUM_LENGTH_RECEIVED)
		{
			sprintf(buffer, "%d", refUserInfo_ptr->getMaximumLengthReceived());
			refMaximumLengthReceived = buffer;
		}
		
		UID_CLASS implementationClassUid = refUserInfo_ptr->getImplementationClassUid();
		if (implementationClassUid.getLength())
		{
			refImplementationClassUid = (char*) implementationClassUid.get();
		}
		
		if (refUserInfo_ptr->getImplementationVersionName())
		{
			refImplementationVersionName = refUserInfo_ptr->getImplementationVersionName();
		}
	}
	
	// validate the parameters
	sprintf(buffer, "%d", srcUserInfo_ptr->getMaximumLengthReceived());
	bool result1 = maximumLengthReceivedM.validate(buffer, refMaximumLengthReceived);
	
	bool result2 = true;
	UID_CLASS implementationClassUid = srcUserInfo_ptr->getImplementationClassUid();
	if (refImplementationClassUid.length() > 0)
	{
		result2 = implementationClassUidM.validate((char*) implementationClassUid.get(), refImplementationClassUid);
	}
	else
	{
		result2 = implementationClassUidM.validate((char*) implementationClassUid.get(), acseProp_ptr->getImplementationClassUid());
	}
	
	bool result3 = true;
	if (srcUserInfo_ptr->getImplementationVersionName())
	{
		// instantiate new validator
		implementationVersionNameM_ptr = new ACSE_IMPLEMENTATION_VERSION_NAME_CLASS();
		if (refImplementationVersionName.length() > 0)
		{
			result3 = implementationVersionNameM_ptr->validate(srcUserInfo_ptr->getImplementationVersionName(), refImplementationVersionName);
		}
		else
		{
			result3 = implementationVersionNameM_ptr->validate(srcUserInfo_ptr->getImplementationVersionName(), acseProp_ptr->getImplementationVersionName());
		}
	}
    else if (refImplementationVersionName.length())
    {
        // no source, only a reference provided
		// instantiate new validator
		implementationVersionNameM_ptr = new ACSE_IMPLEMENTATION_VERSION_NAME_CLASS();
		result3 = implementationVersionNameM_ptr->validate("", refImplementationVersionName);
    }
	
	// validate the optional SCP/SCU role selection
	bool result4 = true;
	for (UINT i = 0; i < srcUserInfo_ptr->noScpScuRoleSelects(); i++)
	{
		ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS scpScuValidator;
		
		if (!scpScuValidator.validate(&srcUserInfo_ptr->getScpScuRoleSelect(i), refUserInfo_ptr))
		{
			result4 = false;
		}
		
		scpScuRoleSelectM.add(scpScuValidator);
	}
	
	// validate the optional asynchronous operation window
	UINT16 invokedOperation;
	UINT16 performedOperation;
	bool result5 = true;
	if (srcUserInfo_ptr->getAsynchronousOperationWindow(&invokedOperation, &performedOperation))
	{
		asynchronousOperationWindowM_ptr = new ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS();
		result5 = asynchronousOperationWindowM_ptr->validate(invokedOperation, performedOperation, refUserInfo_ptr);
	}
	
	// validate the optional SOP class extended
	bool result6 = true;
	for (UINT j = 0; j < srcUserInfo_ptr->noSopClassExtendeds(); j++)
	{
		ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS sopExtendedValidator;
		
		if (!sopExtendedValidator.validate(&srcUserInfo_ptr->getSopClassExtended(j), refUserInfo_ptr))
		{
			result6 = false;
		}
		
		sopClassExtendedM.add(sopExtendedValidator);
	}
	
	// validate the optional user identity negotiation
	bool result7 = true;
	if (srcUserInfo_ptr->getUserIdentityNegotiationPrimaryField() != NULL)
	{
		// validate associate request - user identity negotiation
		userIdentityNegotiationM_ptr = new ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS();
		result7 = userIdentityNegotiationM_ptr->validate(
									srcUserInfo_ptr->getUserIdentityNegotiationUserIdentityType(), 
									srcUserInfo_ptr->getUserIdentityNegotiationPositiveResponseRequested(),
									srcUserInfo_ptr->getUserIdentityNegotiationPrimaryField(),
									srcUserInfo_ptr->getUserIdentityNegotiationSecondaryField(), 
									refUserInfo_ptr);
	}
	else if (srcUserInfo_ptr->getUserIdentityNegotiationServerResponse() != NULL)
	{
		// validate associate accept - user identity negotiation
		userIdentityNegotiationM_ptr = new ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS();
		result7 = userIdentityNegotiationM_ptr->validate(
									srcUserInfo_ptr->getUserIdentityNegotiationServerResponse(), 
									refUserInfo_ptr);
	}

	// set result
	bool result = true;
	if ((!result1) ||
		(!result2) ||
		(!result3) ||
		(!result4) ||
		(!result5) ||
		(!result6) ||
		(!result7))
	{
		result = false;
	}
	
	// return result
	return result;
}
