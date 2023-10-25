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
#include "ACSE_async_operation_window_validator.h"
#include "Iglobal.h"      // Global component interface file
#include "Ilog.h"         // Logging component interface file
#include "Inetwork.h"     // Network component interface file

//>>===========================================================================		

ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS::ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS()

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

ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS::~ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS()

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

ACSE_PARAMETER_CLASS *ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS::getOperationsInvokedParameter()

//  DESCRIPTION     : Get Operations Invoked
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &operationsInvokedM; 
}

//>>===========================================================================		

ACSE_PARAMETER_CLASS *ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS::getOperationsPerformedParameter()

//  DESCRIPTION     : Get Operations Performed
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return &operationsPerformedM; 
}

//>>===========================================================================		

bool ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS::validate(UINT16 srcInvokedOperation, UINT16 srcPerformedOperation, USER_INFORMATION_CLASS *refUserInfo_ptr)

//  DESCRIPTION     : Validate Asynchronous Operation Window.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	string refInvoked;
	string refPerformed;
	
	// check for matching reference values
	UINT16 refInvokedOperation;
	UINT16 refPerformedOperation;
	if ((refUserInfo_ptr) &&
		(refUserInfo_ptr->getAsynchronousOperationWindow(&refInvokedOperation, &refPerformedOperation)))
	{
		sprintf(buffer, "%d", refInvokedOperation);
		refInvoked = buffer;
		sprintf(buffer, "%d", refPerformedOperation);
		refPerformed = buffer;
	}
	
	// validate parameters
	sprintf(buffer, "%d", srcInvokedOperation);
	bool result1 = operationsInvokedM.validate(buffer, refInvoked);
	sprintf(buffer, "%d", srcPerformedOperation);
	bool result2 = operationsPerformedM.validate(buffer, refPerformed);
	
	// set result
	bool result = true;
	if ((!result1) ||
		(!result2))
	{
		result = false;
	}
	
	// return result
	return result;
}
