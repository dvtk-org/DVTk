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
#include "ACSE_implementation_version_name.h"
#include "valrules.h"


//>>===========================================================================		

ACSE_IMPLEMENTATION_VERSION_NAME_CLASS::ACSE_IMPLEMENTATION_VERSION_NAME_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
	nameM = "Implementation Version Name";
	quotedValueM = true;
} 

//>>===========================================================================		

ACSE_IMPLEMENTATION_VERSION_NAME_CLASS::~ACSE_IMPLEMENTATION_VERSION_NAME_CLASS()

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

bool ACSE_IMPLEMENTATION_VERSION_NAME_CLASS::checkSyntax()

//  DESCRIPTION     : Check parameter syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// check parameter syntax
	bool result = true;
	int length = valueM.length();
    if (length > 16)
	{
		addMessage(VAL_RULE_D_AE_1, "Value length %d exceeds maximum length %d",
			length, 16);
		result = false;
	}
	
	// return result
	return result;
} 

//>>===========================================================================		

bool ACSE_IMPLEMENTATION_VERSION_NAME_CLASS::checkRange()

//  DESCRIPTION     : Check parameter range.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// nothing to check
	return true;
} 

//>>===========================================================================		

bool ACSE_IMPLEMENTATION_VERSION_NAME_CLASS::checkReference(string refValue)

//  DESCRIPTION     : Check parameter against reference value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool result = true;
	
	// check parameter against reference value
	if (refValue.length())
	{
		result = checkStringDifferences((char*) valueM.c_str(), (char*) refValue.c_str(), 16, false, false);
	}
	
	// return result
	return result;
} 

