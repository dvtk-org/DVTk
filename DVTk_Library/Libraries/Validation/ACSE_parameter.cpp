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
#include "ACSE_parameter.h"
#include "valrules.h"
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"           // Log component interface

//>>===========================================================================		

ACSE_PARAMETER_CLASS::~ACSE_PARAMETER_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// empty virtual destructor
} 

//>>===========================================================================		

int ACSE_PARAMETER_CLASS::noMessages() 

//  DESCRIPTION     : Get number of Messages
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return messagesM.size(); 
}

//>>===========================================================================		

UINT32 ACSE_PARAMETER_CLASS::getIndex(const int i)

//  DESCRIPTION     : Get indexed message index
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	UINT32 index = 0;

	if (i < (int)messagesM.size())
	{
		index = messagesM[i].index;
	}

	return index;
}

//>>===========================================================================		

UINT32 ACSE_PARAMETER_CLASS::getCode(const int i)

//  DESCRIPTION     : Get indexed code
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	UINT32 code = 0;

	if (i < (int)messagesM.size())
	{
		code = messagesM[i].code;
	}

	return code;
}

//>>===========================================================================		

string ACSE_PARAMETER_CLASS::getMessage(const int i)

//  DESCRIPTION     : Get indexed message
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	string message;

	if (i < (int)messagesM.size())
	{
		message = messagesM[i].message;
	}

	return message;
}

//>>===========================================================================		

void ACSE_PARAMETER_CLASS::setName(const string name)

//  DESCRIPTION     : Set Name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	nameM = name; 
}
	
//>>===========================================================================		

void ACSE_PARAMETER_CLASS::setValue(const string value)

//  DESCRIPTION     : Set Value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	valueM = value;
}	

//>>===========================================================================		

void ACSE_PARAMETER_CLASS::setValue(char *value, ...)

//  DESCRIPTION     : Save the value from the arguments provided.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	char buffer[MAXIMUM_LINE_LENGTH];
	
	va_list	arguments;
	
	// handle the variable arguments
	va_start(arguments, value);
	vsprintf(buffer, value, arguments);
	va_end(arguments);
	
	// save the value
    valueM = buffer;
}

//>>===========================================================================		

string ACSE_PARAMETER_CLASS::getValue()

//  DESCRIPTION     : Get Value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return valueM; 
}

//>>===========================================================================		

void ACSE_PARAMETER_CLASS::setMeaning(const string meaning)

//  DESCRIPTION     : Set Meaning
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	meaningM = meaning; 
}

//>>===========================================================================		

string ACSE_PARAMETER_CLASS::getMeaning()

//  DESCRIPTION     : Get Meaning
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	return meaningM; 
}

//>>===========================================================================		

void ACSE_PARAMETER_CLASS::addMessage(const string message)

//  DESCRIPTION     : Add the message.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The message is automatically assigned an index here.
//<<===========================================================================		
{
	CODE_MESSAGE_STRUCT result;
	
	// save the error message
    result.index = GetNextMessageIndex();
	result.code = 0;
	result.message = message;
	
	messagesM.push_back(result);
}

//>>===========================================================================		

void ACSE_PARAMETER_CLASS::addMessage(const int code, const string message)

//  DESCRIPTION     : Add the code and message.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The message is automatically assigned an index here.
//<<===========================================================================		
{
	CODE_MESSAGE_STRUCT result;
	
	// save the code and message
    result.index = GetNextMessageIndex();
	result.code = code;
	result.message = message;
	
	messagesM.push_back(result);
}

//>>===========================================================================		

void ACSE_PARAMETER_CLASS::addMessage(char *message, ...)

//  DESCRIPTION     : Add the message from the arguments provided.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The message is automatically assigned an index here.
//<<===========================================================================		
{
	CODE_MESSAGE_STRUCT result;
	char buffer[MAXIMUM_LINE_LENGTH];
	
	va_list	arguments;
	
	// handle the variable arguments
	va_start(arguments, message);
	vsprintf(buffer, message, arguments);
	va_end(arguments);
	
	// save the message
    result.index = GetNextMessageIndex();
	result.code = 0;
	result.message = buffer;
	
	messagesM.push_back(result);
}

//>>===========================================================================		

void ACSE_PARAMETER_CLASS::addMessage(const int code, char *message, ...)

//  DESCRIPTION     : Add the code and message from the arguments provided.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The message is automatically assigned an index here.
//<<===========================================================================		
{
	CODE_MESSAGE_STRUCT result;
	char buffer[MAXIMUM_LINE_LENGTH];
	
	va_list	arguments;
	
	// handle the variable arguments
	va_start(arguments, message);
	vsprintf(buffer, message, arguments);
	va_end(arguments);
	
	// save the code and message
    result.index = GetNextMessageIndex();
	result.code = code;
	result.message = buffer;
	
	messagesM.push_back(result);
}

//>>===========================================================================		

bool ACSE_PARAMETER_CLASS::checkIntegerSyntax(int length)

//  DESCRIPTION     : Check integer parameter syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool result = true;
	
	assert (length >= 0);
	// check parameter syntax
	if (valueM.length() > (unsigned int) length) 
	{
		addMessage(VAL_RULE_D_PARAM_1, "Value length %d exceeds maximum length %d", valueM.size(), length);
		result = false;
	}
	
	for (UINT i = 0; i < valueM.length(); i++)
	{
		if (!isdigit(valueM[i]))
		{
			addMessage(VAL_RULE_D_PARAM_2, "Value %s contains non-digit characters", valueM.c_str());
			result = false;
			break;
		}
	}
	
	// return result
	return result;
}

//>>===========================================================================

bool ACSE_PARAMETER_CLASS::checkStringDifferences(char *srcValue,
												  char *refValue,
												  int   maxSize,
												  bool  leadingSpace,
												  bool  trailingSpace)
												  
//  DESCRIPTION     :
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// if 1 or both of the values is missing they are not equal
	if (!srcValue || !refValue)
	{
		return false;
	}
	
	if (!StringValuesEqual(srcValue, refValue,
		maxSize, leadingSpace, trailingSpace)) 
	{
		addMessage(VAL_RULE_R_VALUE_1, "Value of \"%s\" different from reference/session property value of \"%s\"", srcValue, refValue);
		return false;
	}
	
	return true;
}

//>>===========================================================================		

bool ACSE_PARAMETER_CLASS::checkIntegerReference(string refValue)

//  DESCRIPTION     : Check parameter against integer reference value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	bool result = true;
	
	// check parameter against reference value
	if ((refValue.length()) &&
		(atoi(valueM.c_str()) != atoi(refValue.c_str())))
	{
		addMessage(VAL_RULE_R_VALUE_1, "Value of %s different from reference value of %s", valueM.c_str(), refValue.c_str());
		result = false;
	}
	
	// return result
	return result;
} 

//>>===========================================================================		

bool ACSE_PARAMETER_CLASS::validate(string srcValue, string refValue)

//  DESCRIPTION     : Main parameter validation method.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
    // check if we have a value
    if ((srcValue.length() == 0) &&
        (refValue.length() == 0))
    {
        // assume that zero length string are OK
        return true;
    }
    else if ((srcValue.length() == 0) &&
        (refValue.length()))
    {
        // reference defined without source
		addMessage(VAL_RULE_R_VALUE_2, "No source value to compare with reference value of \"%s\"", refValue.c_str());
        return false;
    }

	// first save the source value
	setValue(srcValue);
	
	// check the value syntax
	bool result1 = checkSyntax();
	
	// check the value range
	bool result2 = checkRange();
	
	// finally check the source value against the reference value - if any
	bool result3 = checkReference(refValue);
	
	// set result
	bool result = true;
	if ((!result1) ||
		(!result2) ||
		(!result3))
	{
		result = false;
	}
	
	// resturn result
	return result;
}
