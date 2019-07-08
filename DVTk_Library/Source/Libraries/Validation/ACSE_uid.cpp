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
#include "ACSE_uid.h"
#include "valrules.h"
#include "Iglobal.h"		// Global component interface


//>>===========================================================================		

ACSE_UID_CLASS::ACSE_UID_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// constructor activities
	quotedValueM = true;
} 

//>>===========================================================================		

ACSE_UID_CLASS::~ACSE_UID_CLASS()

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

bool ACSE_UID_CLASS::checkSyntax()

//  DESCRIPTION     : Check parameter syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// check parameter syntax
    UINT length = valueM.length();     	            
	bool result = true;
	
    if (length > UI_LENGTH)
	{
		addMessage(VAL_RULE_D_UI_1, "Value length %d exceeds maximum length %d",
			length, UI_LENGTH);
		result = false;
	}
	else if (length)
	{
		result = checkUidSyntax((char*) valueM.c_str());
	}
	
	// return result
	return result;
} 

//<<===========================================================================		

bool ACSE_UID_CLASS::checkUidSyntax(char *uid_ptr)

//  DESCRIPTION     : Check if UID syntax is OK.					
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Format: 
//                    n.n[.n]* 
//                    where:						
//			          n	= "number" (one or more digits)		
//		              . = Decimal Point "."	
//<<===========================================================================		
{
	bool	firstNum = true;
	char	firstDigit = '0'; 
	int 	index = 0;
	
	// if there is no value, we assume it's valid;
	if (!uid_ptr)
	{
		return true;
	}
	
	if ((*uid_ptr < '0') || (*uid_ptr > '9')) 
	{
		addMessage(VAL_RULE_D_UI_2, "Value should start with digit(s)");
		return false;
	}
	
	while ((*uid_ptr != NullChar) && (index++ < UI_LENGTH)) 
	{
		if (('0' <= *uid_ptr) && (*uid_ptr <= '9')) 
		{
			if (firstNum == true) // first digit of a new component
			{
				firstDigit = *uid_ptr;
				firstNum = false;
			}
			else if (firstDigit == '0') 
			{
				addMessage(VAL_RULE_D_UI_6, "No leading zeros allowed in value");
				return false; 
			}
		}
		else if (*uid_ptr == '.')
		{
			if (firstNum) // two dots -> empty component
			{
				addMessage(VAL_RULE_D_UI_4, "Digits expected between periods");
				return false; 
			}
			firstNum = true; // start new component
		}
		else
		{
			addMessage(VAL_RULE_D_UI_5, "Unexpected Character %c=0x%02X at offset %d",
				(int)(*uid_ptr), (int)(*uid_ptr), index);
			return false; 
		}
		uid_ptr++;
	}
	
	if (firstNum) 
	{
		addMessage(VAL_RULE_D_UI_3, "Value should end with digit(s)");
		return false;
	}
	
	return true;
}

//>>===========================================================================		

bool ACSE_UID_CLASS::checkRange()

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

bool ACSE_UID_CLASS::checkReference(string refValue)

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
		result = checkStringDifferences((char*) valueM.c_str(), (char*) refValue.c_str(), UI_LENGTH, false, false);
	}
	
	// return result
	return result;
} 
