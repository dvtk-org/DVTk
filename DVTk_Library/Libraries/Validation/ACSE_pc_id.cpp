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
#include "ACSE_pc_id.h"
#include "Iglobal.h"		// Global component interface
#include "valrules.h"


//>>===========================================================================		

ACSE_PC_ID_CLASS::ACSE_PC_ID_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
	nameM = " Presentation Context ID";
	quotedValueM = false;
} 

//>>===========================================================================		

ACSE_PC_ID_CLASS::~ACSE_PC_ID_CLASS()

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

bool ACSE_PC_ID_CLASS::checkSyntax()

//  DESCRIPTION     : Check parameter syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// check parameter syntax
	bool result = checkIntegerSyntax(3);
	if (result)
	{
		// check that the value is odd
		int value = atoi((char*) valueM.c_str());
		if (!(value & 1))
		{
			addMessage(VAL_RULE_D_PARAM_4, "Value must be odd", valueM.c_str());
			result = false;
		}
	}
	
	// return result
	return result;
} 

//>>===========================================================================		

bool ACSE_PC_ID_CLASS::checkRange()

//  DESCRIPTION     : Check parameter range.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool result = true;
	
	// check parameter range
	if (atoi(valueM.c_str()) > 255)
	{
		addMessage(VAL_RULE_D_PARAM_3, "Value %s out of range", valueM.c_str());
		result = false;
	}
	
	// return result
	return result;
} 

//>>===========================================================================		

bool ACSE_PC_ID_CLASS::checkReference(string refValue)

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
