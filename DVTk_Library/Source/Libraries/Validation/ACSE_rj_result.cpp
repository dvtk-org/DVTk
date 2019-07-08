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
#include "ACSE_rj_result.h"
#include "valrules.h"


//>>===========================================================================		

ACSE_RJ_RESULT_CLASS::ACSE_RJ_RESULT_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
	nameM = "Result";
	quotedValueM = false;
} 

//>>===========================================================================		

ACSE_RJ_RESULT_CLASS::~ACSE_RJ_RESULT_CLASS()

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

bool ACSE_RJ_RESULT_CLASS::checkSyntax()

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

bool ACSE_RJ_RESULT_CLASS::checkRange()

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
	case REJECTED_PERMANENT:
	case REJECTED_TRANSIENT:
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

bool ACSE_RJ_RESULT_CLASS::checkReference(string refValue)

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
