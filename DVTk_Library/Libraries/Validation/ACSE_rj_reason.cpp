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
#include "ACSE_rj_reason.h"
#include "valrules.h"


//>>===========================================================================		

ACSE_RJ_REASON_CLASS::ACSE_RJ_REASON_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
	nameM = "Reason";
	quotedValueM = false;
} 

//>>===========================================================================		

ACSE_RJ_REASON_CLASS::~ACSE_RJ_REASON_CLASS()

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

bool ACSE_RJ_REASON_CLASS::checkSyntax()

//  DESCRIPTION     : Check parameter syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// check parameter syntax
	return checkIntegerSyntax(1);
} 

//>>===========================================================================		

bool ACSE_RJ_REASON_CLASS::checkRange()

//  DESCRIPTION     : Check parameter range.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool result = true;
	
	// check parameter range
	int value = atoi(valueM.c_str());
	switch(value)
	{
	case NO_REASON_GIVEN:
		// case TEMPORARY_CONGESTION:
	case APPLICATION_CONTEXT_NAME_NOT_SUPPORTED:
		// case PROTOCOL_VERSION_NOT_SUPPORTED:
		// case LOCAL_LIMIT_EXCEEDED:
	case CALLING_AE_TITLE_NOT_RECOGNISED:
	case CALLED_AE_TITLE_NOT_RECOGNISED:
		break;
	default:
		addMessage(VAL_RULE_D_PARAM_3, "Value %d out of range", value);
		result = false;
		break;
	}
	
	// return result
	return result;
} 

//>>===========================================================================		

bool ACSE_RJ_REASON_CLASS::checkReference(string refValue)

//  DESCRIPTION     : Check parameter against reference value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// check parameter against reference value
	return checkIntegerReference(refValue);
} 
