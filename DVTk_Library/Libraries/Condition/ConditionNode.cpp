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
#include "ConditionNode.h"
#include "IAttributeGroup.h"// Attribute Group component interface
#include "Idicom.h"			// DICOM component interface
#include "Iutility.h"		// Utility component interface
#include <math.h>


//*****************************************************************************
//  INTERNAL DEFINITIONS
//*****************************************************************************
static DVT_STATUS CompareAttributeValue(DCM_ATTRIBUTE_CLASS* attr_ptr, UINT16 value_nr, string ref_value);
static CONDITION_RESULT_ENUM GetConditionAndResult(CONDITION_RESULT_ENUM result1, CONDITION_RESULT_ENUM result2);
static CONDITION_RESULT_ENUM GetConditionOrResult(CONDITION_RESULT_ENUM result1, CONDITION_RESULT_ENUM result2);


//*****************************************************************************
//  CONDITION_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NODE_CLASS::~CONDITION_NODE_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// base destructor
}

//>>===========================================================================

void CONDITION_NODE_CLASS::AddMessage(char* format_ptr, ...)

//  DESCRIPTION     : adds message to result message, supports formatting
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	va_list	arguments;

	char buffer[MAX_CONDITION_LENGTH];
	string msg;

	// handle the variable arguments
	va_start(arguments, format_ptr);
	vsprintf(buffer, format_ptr, arguments);
	va_end(arguments);

    msg = buffer;

    result_messageM += msg;
}

//>>===========================================================================

bool CONDITION_NODE_CLASS::HasResultMessage()

//  DESCRIPTION     : Check if there is a result message
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    bool result = (result_messageM.length() > 0) ? true : false;
    return result;
}

//>>===========================================================================

string CONDITION_NODE_CLASS::GetResultMessage()

//  DESCRIPTION     : Get the current result message and clear the string
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string msg = result_messageM;

	// clear message - this is required as this message is only valid for 1 evaluation
    result_messageM.erase();

	return msg;
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_NODE_CLASS::DetermineFinalEvaluationResult(CONDITION_RESULT_ENUM incomingResult)

//  DESCRIPTION     : Determine the final evaluation result taking the condition type into account.
//					: If the incoming result indicates CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION then
//					: this should be preserved.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = incomingResult;

	if (incomingResult == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION)
	{
		result = CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION;
	}
	else if ((incomingResult == CONDITION_TRUE) &&
		(conditionTypeM == CONDITION_TYPE_WARNING))
	{
		result = CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION;
	}

	return result;
}
//*****************************************************************************
//  COND_BINARY_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_BINARY_NODE_CLASS::CONDITION_BINARY_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	leftM_ptr = NULL;
	rightM_ptr = NULL;
}

//>>===========================================================================

CONDITION_BINARY_NODE_CLASS::CONDITION_BINARY_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr, CONDITION_NODE_CLASS* right_ptr) 

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	leftM_ptr = left_ptr;
	rightM_ptr = right_ptr;
}

//>>===========================================================================

CONDITION_BINARY_NODE_CLASS::~CONDITION_BINARY_NODE_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	// - delete left & right nodes if they exist
	if (leftM_ptr)
	{
		delete leftM_ptr;
	}

	if (rightM_ptr)
	{
		delete rightM_ptr;
	}
}

//>>===========================================================================

void CONDITION_BINARY_NODE_CLASS::SetLeft(CONDITION_NODE_CLASS* left_ptr)
 
//  DESCRIPTION     : Sets left branch of node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	leftM_ptr = left_ptr;
}

//>>===========================================================================

void CONDITION_BINARY_NODE_CLASS::SetRight(CONDITION_NODE_CLASS* right_ptr) 

//  DESCRIPTION     : Sets right branch of node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	rightM_ptr = right_ptr;
}

//>>===========================================================================

void CONDITION_BINARY_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		leftM_ptr->Log(logger_ptr);
		rightM_ptr->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_BINARY_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, 
									 DCM_ATTRIBUTE_GROUP_CLASS*,
									 LOG_CLASS*)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return CONDITION_FALSE;
}

//*****************************************************************************
//  CONDITION_AND_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_AND_NODE_CLASS::CONDITION_AND_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_AND_NODE_CLASS::CONDITION_AND_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr,	CONDITION_NODE_CLASS* right_ptr) 
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_AND_NODE_CLASS::~CONDITION_AND_NODE_CLASS()

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

void CONDITION_AND_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetLeft()->Log(logger_ptr);
		logger_ptr->text(LOG_DEBUG, 0, " AND ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_AND_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								  DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clear result message
    result_messageM.erase();

	// evaluate the left node
    CONDITION_RESULT_ENUM left_result = GetLeft()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// evaluate the right node
	CONDITION_RESULT_ENUM right_result = GetRight()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// result is true if left and right are true
	CONDITION_RESULT_ENUM result = GetConditionAndResult(left_result, right_result);
	if ((result == CONDITION_TRUE) ||
		(result == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION))
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[%s AND %s]", GetLeft()->GetResultMessage().c_str(), GetRight()->GetResultMessage().c_str());

	return DetermineFinalEvaluationResult(result);
}


//*****************************************************************************
//  CONDITION_OR_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_OR_NODE_CLASS::CONDITION_OR_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_OR_NODE_CLASS::CONDITION_OR_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr, CONDITION_NODE_CLASS* right_ptr) 
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_OR_NODE_CLASS::~CONDITION_OR_NODE_CLASS()

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

void CONDITION_OR_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetLeft()->Log(logger_ptr);
		logger_ptr->text(LOG_DEBUG, 0, " OR ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_OR_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								 DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								 LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	//clear result message
    result_messageM.erase();

	// evaluate left node
    CONDITION_RESULT_ENUM left_result = GetLeft()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// evaluate right node
	CONDITION_RESULT_ENUM right_result = GetRight()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// result is true if either left or right is true
	CONDITION_RESULT_ENUM result = GetConditionOrResult(left_result, right_result);
	if ((result == CONDITION_TRUE) ||
		(result == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION))
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[%s OR %s]", GetLeft()->GetResultMessage().c_str(), GetRight()->GetResultMessage().c_str());    

	return DetermineFinalEvaluationResult(result);
}


//*****************************************************************************
//  CONDITION_VALUE_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_VALUE_NODE_CLASS::CONDITION_VALUE_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_VALUE_NODE_CLASS::CONDITION_VALUE_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr,	CONDITION_NODE_CLASS* right_ptr) 
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_VALUE_NODE_CLASS::~CONDITION_VALUE_NODE_CLASS()

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

void CONDITION_VALUE_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
	    logger_ptr->text(LOG_DEBUG, 0, "VALUE ");
		GetLeft()->Log(logger_ptr);
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_VALUE_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, 
									DCM_ATTRIBUTE_GROUP_CLASS*,
									LOG_CLASS*)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return CONDITION_FALSE;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *CONDITION_VALUE_NODE_CLASS::GetAttribute(DCM_ATTRIBUTE_GROUP_CLASS *obj1_ptr, DCM_ATTRIBUTE_GROUP_CLASS *obj2_ptr, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Returns the attribute address found via the navigation
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // navigate node
    CONDITION_NAVIGATION_NODE_CLASS *navigationNode_ptr = static_cast<CONDITION_NAVIGATION_NODE_CLASS*>(GetLeft());
    DCM_ATTRIBUTE_CLASS *attribute_ptr = navigationNode_ptr->Navigate(obj1_ptr, obj2_ptr, logger_ptr);

	//clear result message
    result_messageM.erase();

    // simply add underlying message
	AddMessage("%s", GetLeft()->GetResultMessage().c_str());    

    return attribute_ptr;
}

//>>===========================================================================

UINT16 CONDITION_VALUE_NODE_CLASS::GetValueNr()

//  DESCRIPTION     : returns value number
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return static_cast<CONDITION_LEAF_VALUE_NR_CLASS*>(GetRight())->GetValueNr();
}

//*****************************************************************************
//  CONDITION_LESS_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_LESS_NODE_CLASS::CONDITION_LESS_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_LESS_NODE_CLASS::CONDITION_LESS_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr, CONDITION_NODE_CLASS* right_ptr) 
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_LESS_NODE_CLASS::~CONDITION_LESS_NODE_CLASS()

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

void CONDITION_LESS_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetLeft()->Log(logger_ptr);
		logger_ptr->text(LOG_DEBUG, 0, " < ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_LESS_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								   DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								   LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_FALSE;

    DCM_ATTRIBUTE_CLASS *attribute_ptr = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetAttribute(obj1_ptr, obj2_ptr, logger_ptr);
    UINT16 value_nr  = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<CONDITION_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
    if (attribute_ptr != NULL && attribute_ptr->IsPresent())
	{
        UINT16 nr_values = (UINT16) attribute_ptr->GetNrValues();

		if ((value_nr != APPLY_TO_ANY_VALUE) &&
            (value_nr > nr_values) && 
            (logger_ptr))
		{
            logger_ptr->text(LOG_DEBUG, 1, "Value %i for attribute does not exist", value_nr);
            logger_ptr->text(LOG_DEBUG, 1, "Check condition");
		}
		else
		{
            if (value_nr == APPLY_TO_ANY_VALUE)
            {
                // any value must be smaller
                for (UINT16 i = 0; i < nr_values; i++)
                {
                    DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, i+1, const_val);
	        		if (cmp_result == MSG_SMALLER)
		        	{
                        result = CONDITION_TRUE;
                        break;
			        }
                }
            }
            else
            {
                // specific value must be smaller
                DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, value_nr, const_val);
	    		if (cmp_result == MSG_SMALLER)
		    	{
                    result = CONDITION_TRUE;
			    }
            }
		}
	}

	if (result == CONDITION_TRUE)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
    if (value_nr == APPLY_TO_ANY_VALUE)
    {
    	AddMessage("[VALUE %s ANY < %s]", GetLeft()->GetResultMessage().c_str(), const_val.c_str());
    }
    else
    {
    	AddMessage("[VALUE %s %i < %s]", GetLeft()->GetResultMessage().c_str(), value_nr, const_val.c_str());
    }
	return DetermineFinalEvaluationResult(result);
}

//*****************************************************************************
//  CONDITION_LESS_EQ_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_LESS_EQ_NODE_CLASS::CONDITION_LESS_EQ_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_LESS_EQ_NODE_CLASS::CONDITION_LESS_EQ_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr,	CONDITION_NODE_CLASS* right_ptr) 
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_LESS_EQ_NODE_CLASS::~CONDITION_LESS_EQ_NODE_CLASS()

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

void CONDITION_LESS_EQ_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetLeft()->Log(logger_ptr);
		logger_ptr->text(LOG_DEBUG, 0, " <= ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_LESS_EQ_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									  DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_FALSE;

    DCM_ATTRIBUTE_CLASS *attribute_ptr = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetAttribute(obj1_ptr, obj2_ptr, logger_ptr);
    UINT16 value_nr  = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<CONDITION_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();
	
	// check whether the attribute is present in the object
    if (attribute_ptr != NULL && attribute_ptr->IsPresent())
	{
        UINT16 nr_values = (UINT16) attribute_ptr->GetNrValues();

		if ((value_nr != APPLY_TO_ANY_VALUE) &&
            (value_nr > nr_values) && 
            (logger_ptr))
		{
            logger_ptr->text(LOG_DEBUG, 1, "Value %i for attribute does not exist", value_nr);
            logger_ptr->text(LOG_DEBUG, 1, "Check condition");
		}
		else
		{
            if (value_nr == APPLY_TO_ANY_VALUE)
            {
                // any value must be smaller or equal
                for (UINT16 i = 0; i < nr_values; i++)
                {
                    DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, i+1, const_val);
    			    if (cmp_result == MSG_SMALLER ||
	    			    cmp_result == MSG_EQUAL)
		        	{
                        result = CONDITION_TRUE;
                        break;
			        }
                }
            }
            else
            {
                // specific value must be smaller or equal
                DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, value_nr, const_val);
			    if (cmp_result == MSG_SMALLER ||
				    cmp_result == MSG_EQUAL)
			    {
                    result = CONDITION_TRUE;
                }
			}
		}
	}

	if (result == CONDITION_TRUE)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
    if (value_nr == APPLY_TO_ANY_VALUE)
    {
    	AddMessage("[VALUE %s ANY <= %s]", GetLeft()->GetResultMessage().c_str(), const_val.c_str());
    }
    else
    {
    	AddMessage("[VALUE %s %i <= %s]", GetLeft()->GetResultMessage().c_str(), value_nr, const_val.c_str());
    }
	return DetermineFinalEvaluationResult(result);
}

//*****************************************************************************
//  CONDITION_EQ_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_EQ_NODE_CLASS::CONDITION_EQ_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_EQ_NODE_CLASS::CONDITION_EQ_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr, CONDITION_NODE_CLASS* right_ptr) 
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_EQ_NODE_CLASS::~CONDITION_EQ_NODE_CLASS()

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

void CONDITION_EQ_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetLeft()->Log(logger_ptr);
		logger_ptr->text(LOG_DEBUG, 0, " = ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_EQ_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								 DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								 LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_FALSE;
    DCM_ATTRIBUTE_CLASS *attribute_ptr = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetAttribute(obj1_ptr, obj2_ptr, logger_ptr);
    UINT16 value_nr  = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<CONDITION_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
    if ( attribute_ptr != 0 && attribute_ptr->IsPresent())
	{
        UINT16 nr_values = (UINT16) attribute_ptr->GetNrValues();

		if ((value_nr != APPLY_TO_ANY_VALUE) &&
            (value_nr > nr_values) && 
            (logger_ptr))
		{
            logger_ptr->text(LOG_DEBUG, 1, "Value %i for attribute does not exist", value_nr);
            logger_ptr->text(LOG_DEBUG, 1, "Check condition");
		}
		else
		{
            if (value_nr == APPLY_TO_ANY_VALUE)
            {
                // any value must be equal
                for (UINT16 i = 0; i < nr_values; i++)
                {
                    DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, i+1, const_val);
	        		if (cmp_result == MSG_EQUAL)
		        	{
                        result = CONDITION_TRUE;
                        break;
			        }
                }
            }
            else
            {
                // specific value must be equal
                DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, value_nr, const_val);
			    if (cmp_result == MSG_EQUAL)
			    {
                    result = CONDITION_TRUE;
			    }
            }
		}
	}

	if (result == CONDITION_TRUE)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
    if (value_nr == APPLY_TO_ANY_VALUE)
    {
    	AddMessage("[VALUE %s ANY = %s]", GetLeft()->GetResultMessage().c_str(), const_val.c_str());
    }
    else
    {
    	AddMessage("[VALUE %s %i = %s]", GetLeft()->GetResultMessage().c_str(), value_nr, const_val.c_str());
    }
	return DetermineFinalEvaluationResult(result);
}


//*****************************************************************************
//  CONDITION_GREATER_EQ_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_GREATER_EQ_NODE_CLASS::CONDITION_GREATER_EQ_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_GREATER_EQ_NODE_CLASS::CONDITION_GREATER_EQ_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr, CONDITION_NODE_CLASS* right_ptr)
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_GREATER_EQ_NODE_CLASS::~CONDITION_GREATER_EQ_NODE_CLASS()

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

void CONDITION_GREATER_EQ_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetLeft()->Log(logger_ptr);
		logger_ptr->text(LOG_DEBUG, 0, " >= ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_GREATER_EQ_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
										 DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
										 LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_FALSE;
    DCM_ATTRIBUTE_CLASS *attribute_ptr = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetAttribute(obj1_ptr, obj2_ptr, logger_ptr);
    UINT16 value_nr  = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<CONDITION_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
    if (attribute_ptr != NULL && attribute_ptr->IsPresent())
	{
        UINT16 nr_values = (UINT16) attribute_ptr->GetNrValues();

		if ((value_nr != APPLY_TO_ANY_VALUE) &&
            (value_nr > nr_values) && 
            (logger_ptr))
		{
            logger_ptr->text(LOG_DEBUG, 1, "Value %i for attribute does not exist", value_nr);
            logger_ptr->text(LOG_DEBUG, 1, "Check condition");
		}
		else
		{
            if (value_nr == APPLY_TO_ANY_VALUE)
            {
                // any value must be greater or equal
                for (UINT16 i = 0; i < nr_values; i++)
                {
                    DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, i+1, const_val);
    	    		if (cmp_result == MSG_GREATER ||
	    	    		cmp_result == MSG_EQUAL)
		        	{
                        result = CONDITION_TRUE;
                        break;
			        }
                }
            }
            else
            {
                // specific value must be greater or equal
                DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, value_nr, const_val);
	    		if (cmp_result == MSG_GREATER ||
		    		cmp_result == MSG_EQUAL)
			    {
                    result = CONDITION_TRUE;
			    }
            }
		}
	}

	if (result == CONDITION_TRUE)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
    if (value_nr == APPLY_TO_ANY_VALUE)
    {
    	AddMessage("[VALUE %s ANY >= %s]", GetLeft()->GetResultMessage().c_str(), const_val.c_str());
    }
    else
    {
    	AddMessage("[VALUE %s %i >= %s]", GetLeft()->GetResultMessage().c_str(), value_nr, const_val.c_str());
    }
	return DetermineFinalEvaluationResult(result);
}

//*****************************************************************************
//  CONDITION_GREATER_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_GREATER_NODE_CLASS::CONDITION_GREATER_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_GREATER_NODE_CLASS::CONDITION_GREATER_NODE_CLASS(CONDITION_NODE_CLASS* left_ptr,	CONDITION_NODE_CLASS* right_ptr) 
: CONDITION_BINARY_NODE_CLASS(left_ptr, right_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_GREATER_NODE_CLASS::~CONDITION_GREATER_NODE_CLASS()

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

void CONDITION_GREATER_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetLeft()->Log(logger_ptr);
		logger_ptr->text(LOG_DEBUG, 0, " > ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_GREATER_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									  DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_FALSE;
    DCM_ATTRIBUTE_CLASS *attribute_ptr = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetAttribute(obj1_ptr, obj2_ptr, logger_ptr);
    UINT16 value_nr  = static_cast<CONDITION_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<CONDITION_LEAF_CONST_CLASS*>(GetRight())->GetConstant();
	
	// clear result message
    result_messageM.erase();
	
	// check whether the attribute is present in the object
    if (attribute_ptr != NULL && attribute_ptr->IsPresent())
	{
        UINT16 nr_values = (UINT16) attribute_ptr->GetNrValues();

		if ((value_nr != APPLY_TO_ANY_VALUE) &&
            (value_nr > nr_values) && 
            (logger_ptr))
		{
            logger_ptr->text(LOG_DEBUG, 1, "Value %i for attribute does not exist", value_nr);
            logger_ptr->text(LOG_DEBUG, 1, "Check condition");
		}
		else
		{
            if (value_nr == APPLY_TO_ANY_VALUE)
            {
                // any value must be greater
                for (UINT16 i = 0; i < nr_values; i++)
                {
                    DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, i+1, const_val);
    	    		if (cmp_result == MSG_GREATER)
		        	{
                        result = CONDITION_TRUE;
                        break;
			        }
                }
            }
            else
            {
                // specific value must be greater
                DVT_STATUS cmp_result = CompareAttributeValue(attribute_ptr, value_nr, const_val);
    			if (cmp_result == MSG_GREATER)
	    		{
                    result = CONDITION_TRUE;
			    }
            }
		}
	}

	if (result == CONDITION_TRUE)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}

    if (value_nr == APPLY_TO_ANY_VALUE)
    {
    	AddMessage("[VALUE %s ANY > %s]", GetLeft()->GetResultMessage().c_str(), const_val.c_str());
    }
    else
    {
    	AddMessage("[VALUE %s %i > %s]", GetLeft()->GetResultMessage().c_str(), value_nr, const_val.c_str());
    }
	return DetermineFinalEvaluationResult(result);
}

//*****************************************************************************
//  CONDITION_UNARY_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_UNARY_NODE_CLASS::CONDITION_UNARY_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	nodeM_ptr = NULL;
}

//>>===========================================================================

CONDITION_UNARY_NODE_CLASS::CONDITION_UNARY_NODE_CLASS(CONDITION_NODE_CLASS* node_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	nodeM_ptr = node_ptr;
}

//>>===========================================================================

CONDITION_UNARY_NODE_CLASS::~CONDITION_UNARY_NODE_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (nodeM_ptr)
	{
		delete nodeM_ptr;
	}
}

//>>===========================================================================

void CONDITION_UNARY_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		GetNode()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_UNARY_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, 
									DCM_ATTRIBUTE_GROUP_CLASS*,
									LOG_CLASS*)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return CONDITION_TRUE;
}

//*****************************************************************************
//  CONDITION_EMPTY_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_EMPTY_NODE_CLASS::CONDITION_EMPTY_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constuctor activities
}

//>>===========================================================================

CONDITION_EMPTY_NODE_CLASS::CONDITION_EMPTY_NODE_CLASS(CONDITION_NODE_CLASS* node_ptr) 
: CONDITION_UNARY_NODE_CLASS(node_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_EMPTY_NODE_CLASS::~CONDITION_EMPTY_NODE_CLASS()

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

void CONDITION_EMPTY_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "EMPTY ");
		GetNode()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_EMPTY_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_TRUE;

    // clear result message
    result_messageM.erase();

	// navigate node
    CONDITION_NAVIGATION_NODE_CLASS *navigationNode_ptr = static_cast<CONDITION_NAVIGATION_NODE_CLASS*>(GetNode());
    DCM_ATTRIBUTE_CLASS *attribute_ptr = navigationNode_ptr->Navigate(obj1_ptr, obj2_ptr, logger_ptr);

	// if the attribute exists, check whether it is empty. 
	// if the attribute does not exist it certainly is empty!
    if (attribute_ptr != NULL && attribute_ptr->IsPresent())
	{
		UINT nr_values = attribute_ptr->GetNrValues();
		if (nr_values == 0)
		{
            result = CONDITION_TRUE;
		}
		else
		{
			// the attribute has values but they may be empty
			for (UINT i = 0; i < nr_values; ++i)
			{
				BASE_VALUE_CLASS* val_ptr = attribute_ptr->GetValue(i);
				if (val_ptr->GetLength())
				{
					result = CONDITION_FALSE;
				}
			}
		}
	}

	if (result == CONDITION_TRUE)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[EMPTY %s]", navigationNode_ptr->GetResultMessage().c_str());

	return DetermineFinalEvaluationResult(result);
}

//*****************************************************************************
//  CONDITION_NOT_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NOT_NODE_CLASS::CONDITION_NOT_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_NOT_NODE_CLASS::CONDITION_NOT_NODE_CLASS(CONDITION_NODE_CLASS* node_ptr) 
: CONDITION_UNARY_NODE_CLASS(node_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_NOT_NODE_CLASS::~CONDITION_NOT_NODE_CLASS()

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

void CONDITION_NOT_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "NOT ");
		GetNode()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_NOT_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								  DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	//clear result message
    result_messageM.erase();

	CONDITION_RESULT_ENUM result = GetNode()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// this condition is true if the evalution returns false
	if (result == CONDITION_FALSE)
	{
		AddMessage("T");
		result = CONDITION_TRUE;
	}
	else
	{
        AddMessage("F");
		result = CONDITION_FALSE;
	}
	AddMessage("[NOT %s]", GetNode()->GetResultMessage().c_str());

	return result;
}

//*****************************************************************************
//  CONDITION_PRESENT_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_PRESENT_NODE_CLASS::CONDITION_PRESENT_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_PRESENT_NODE_CLASS::CONDITION_PRESENT_NODE_CLASS(CONDITION_NODE_CLASS* node_ptr) 
: CONDITION_UNARY_NODE_CLASS(node_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

CONDITION_PRESENT_NODE_CLASS::~CONDITION_PRESENT_NODE_CLASS()

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

void CONDITION_PRESENT_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "PRESENT ");
		GetNode()->Log(logger_ptr);
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_PRESENT_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									  DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    CONDITION_RESULT_ENUM result = CONDITION_FALSE;

    //clear result message
    result_messageM.erase();

	// navigate node
    CONDITION_NAVIGATION_NODE_CLASS *navigationNode_ptr = static_cast<CONDITION_NAVIGATION_NODE_CLASS*>(GetNode());
    DCM_ATTRIBUTE_CLASS *attribute_ptr = navigationNode_ptr->Navigate(obj1_ptr, obj2_ptr, logger_ptr);

    // condition is true if the attribute is present
	if (attribute_ptr)
	{
        result = CONDITION_TRUE;
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[PRESENT %s]", navigationNode_ptr->GetResultMessage().c_str());    

	return DetermineFinalEvaluationResult(result);
}

//*****************************************************************************
//  CONDITION_NAVIGATION_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NAVIGATION_NODE_CLASS::~CONDITION_NAVIGATION_NODE_CLASS()

//  DESCRIPTION     : Default destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

CONDITION_NAVIGATION_NODE_CLASS *CONDITION_NAVIGATION_NODE_CLASS::GetNavigationNode()

//  DESCRIPTION     : Get the next Navigation Node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // return the next navigation node
    return static_cast<CONDITION_NAVIGATION_NODE_CLASS*>(GetNode());
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_NAVIGATION_NODE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*)

//  DESCRIPTION     : Evaluate method - not applicable to navigation nodes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return result
    return CONDITION_FALSE;
}


//*****************************************************************************
//  CONDITION_NAVIGATION_HERE_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NAVIGATION_HERE_NODE_CLASS::CONDITION_NAVIGATION_HERE_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
}
//>>===========================================================================

CONDITION_NAVIGATION_HERE_NODE_CLASS::CONDITION_NAVIGATION_HERE_NODE_CLASS(bool po):
	parent_only(po)

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
}

//>>===========================================================================

CONDITION_NAVIGATION_HERE_NODE_CLASS::~CONDITION_NAVIGATION_HERE_NODE_CLASS()

//  DESCRIPTION     : Default Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *CONDITION_NAVIGATION_HERE_NODE_CLASS::Navigate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr, LOG_CLASS* logger_ptr, bool po)

//  DESCRIPTION     : Navigate Here node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// navigate Here node
	// clear result message
    result_messageM.erase();

	// navigate node
    DCM_ATTRIBUTE_CLASS *attribute_ptr = GetNavigationNode()->Navigate(obj1_ptr, obj2_ptr, logger_ptr, parent_only);
	if (attribute_ptr)
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[./%s]", GetNavigationNode()->GetResultMessage().c_str());    

	return attribute_ptr;
}

//>>===========================================================================

void CONDITION_NAVIGATION_HERE_NODE_CLASS::Log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log Here node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// log Here node
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "./");
		GetNavigationNode()->Log(logger_ptr);
	}
}


//*****************************************************************************
//  CONDITION_NAVIGATION_UP_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NAVIGATION_UP_NODE_CLASS::CONDITION_NAVIGATION_UP_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
}

//>>===========================================================================

CONDITION_NAVIGATION_UP_NODE_CLASS::~CONDITION_NAVIGATION_UP_NODE_CLASS()

//  DESCRIPTION     : Default Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *CONDITION_NAVIGATION_UP_NODE_CLASS::Navigate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS* logger_ptr, bool po)

//  DESCRIPTION     : Navigate Up node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Make assumption that obj1_ptr is the DCM object to navigate through
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS *attribute_ptr = NULL;

	// clear result message
    result_messageM.erase();

	// navigate Up node
	// - need to move up one level in the DCM object structure - if possible
	DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(obj1_ptr->getParent());
	if (sqValue_ptr != NULL)
	{
		// use sequence value to get to the sequence attribute
		DCM_ATTRIBUTE_CLASS *attributeSq_ptr = sqValue_ptr->getParent();
		if (attributeSq_ptr != NULL)
		{
			// now use the attribute to get to the parent attribute group
			obj1_ptr = attributeSq_ptr->getParent();
			if (obj1_ptr != NULL)
			{
				// navigate from next level up
				attribute_ptr = GetNavigationNode()->Navigate(obj1_ptr, NULL, logger_ptr, true);
			}
		}
	}

	if (attribute_ptr)
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[../%s]", GetNavigationNode()->GetResultMessage().c_str());    

	return attribute_ptr;
}

//>>===========================================================================

void CONDITION_NAVIGATION_UP_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Log Up node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// log Up node
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "../");
		GetNavigationNode()->Log(logger_ptr);
	}
}

//*****************************************************************************
//  CONDITION_NAVIGATION_ROOT_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NAVIGATION_ROOT_NODE_CLASS::CONDITION_NAVIGATION_ROOT_NODE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
}

//>>===========================================================================

CONDITION_NAVIGATION_ROOT_NODE_CLASS::~CONDITION_NAVIGATION_ROOT_NODE_CLASS()

//  DESCRIPTION     : Default Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS* CONDITION_NAVIGATION_ROOT_NODE_CLASS::Navigate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS* logger_ptr, bool po)

//  DESCRIPTION     : Navigate Up node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Make assumption that obj1_ptr is the DCM object to navigate through
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS* attribute_ptr = NULL;

	// clear result message
	result_messageM.erase();

	// navigate root node
	// - need to move up one level in the DCM object structure - if possible
	DCM_VALUE_SQ_CLASS* sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(obj1_ptr->getParent());
	while (sqValue_ptr != NULL)
	{
		// use sequence value to get to the sequence attribute
		DCM_ATTRIBUTE_CLASS* attributeSq_ptr = sqValue_ptr->getParent();
		if (attributeSq_ptr != NULL)
		{
			// now use the attribute to get to the parent attribute group
			obj1_ptr = attributeSq_ptr->getParent();
		}
		sqValue_ptr = dynamic_cast<DCM_VALUE_SQ_CLASS*>(obj1_ptr->getParent());
	}
	if (obj1_ptr != NULL)
	{
		// navigate from next level up
		attribute_ptr = GetNavigationNode()->Navigate(obj1_ptr, NULL, logger_ptr, true);
	}

	if (attribute_ptr)
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[../%s]", GetNavigationNode()->GetResultMessage().c_str());

	return attribute_ptr;
}

//>>===========================================================================

void CONDITION_NAVIGATION_ROOT_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Log Up node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// log Up node
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "../");
		GetNavigationNode()->Log(logger_ptr);
	}
}


//*****************************************************************************
//  CONDITION_NAVIGATION_DOWN_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NAVIGATION_DOWN_NODE_CLASS::CONDITION_NAVIGATION_DOWN_NODE_CLASS(UINT16 group, UINT16 element)

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	tag_groupM = group;
	tag_elementM = element;
}

//>>===========================================================================

CONDITION_NAVIGATION_DOWN_NODE_CLASS::~CONDITION_NAVIGATION_DOWN_NODE_CLASS()

//  DESCRIPTION     : Default Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *CONDITION_NAVIGATION_DOWN_NODE_CLASS::Navigate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS* logger_ptr, bool po)

//  DESCRIPTION     : Navigate Down node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Make assumption that obj1_ptr is the DCM object to navigate through
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS *attribute_ptr = NULL;

	// clear result message
    result_messageM.erase();

	// navigate the down node
	// get the sequence attribute indicated
	DCM_ATTRIBUTE_CLASS *attributeSq_ptr = obj1_ptr->GetMappedAttribute(tag_groupM, tag_elementM);
	if ((attributeSq_ptr) &&
		(attributeSq_ptr->GetVR() == ATTR_VR_SQ))
	{
		// get the sequence value
		DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attributeSq_ptr->GetValue(0));
		if ((sqValue_ptr) &&
			(sqValue_ptr->GetNrItems() > 0))
		{
			// get the first item - evaluation will be done on this
			DCM_ITEM_CLASS *item_ptr = sqValue_ptr->getItem(0);
			attribute_ptr = GetNavigationNode()->Navigate(item_ptr, NULL, logger_ptr, po);
		}
	}

	if (attribute_ptr)
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
    if (GetNavigationNode()->HasResultMessage())
    {
    	AddMessage("[0x%04X%04X/%s]", tag_groupM, tag_elementM, GetNavigationNode()->GetResultMessage().c_str());    
    }
    else
    {
     	AddMessage("[0x%04X%04X/...]", tag_groupM, tag_elementM);    
    }
	return attribute_ptr;
}

//>>===========================================================================

void CONDITION_NAVIGATION_DOWN_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Log Down node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// log Down node
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "0x%04X%04X/", tag_groupM, tag_elementM);
		GetNavigationNode()->Log(logger_ptr);
	}
}

//*****************************************************************************
//  CONDITION_NAVIGATION_TAG_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_NAVIGATION_TAG_CLASS::CONDITION_NAVIGATION_TAG_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	tag_groupM   = 0;
	tag_elementM = 0;
}

//>>===========================================================================

CONDITION_NAVIGATION_TAG_CLASS::CONDITION_NAVIGATION_TAG_CLASS(UINT16 taggroup, UINT16 tagelement)

//  DESCRIPTION     : Constructor with tag arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	tag_groupM   = taggroup;
	tag_elementM = tagelement;
}

//>>===========================================================================

CONDITION_NAVIGATION_TAG_CLASS::~CONDITION_NAVIGATION_TAG_CLASS()

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

void CONDITION_NAVIGATION_TAG_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Logs the Navigation Tag
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "0x%04X%04X", tag_groupM, tag_elementM);
	}
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *CONDITION_NAVIGATION_TAG_CLASS::Navigate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								  DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								  LOG_CLASS*, bool po)

//  DESCRIPTION     : Evaluates the Navigation Tag
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
	DCM_ATTRIBUTE_CLASS* attribute_ptr = obj1_ptr->GetMappedAttribute(tag_groupM, tag_elementM, po);

	if (attribute_ptr == NULL && obj2_ptr != NULL)
	{
		// check whether the attribute is present in the co-object
		attribute_ptr = obj2_ptr->GetMappedAttribute(tag_groupM, tag_elementM, po);
	}

	// we didn't find the attribute
    if (attribute_ptr == NULL || !(attribute_ptr->IsPresent()))
	{
		attribute_ptr = NULL;
		AddMessage("F");
	}
	else
	{
        AddMessage("T");
	}
	AddMessage("[0x%04X%04X]", tag_groupM, tag_elementM);

	return attribute_ptr;
}


//*****************************************************************************
//  CONDITION_LEAF_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_LEAF_CLASS::~CONDITION_LEAF_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}


//*****************************************************************************
//  CONDITION_LEAF_CONST_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_LEAF_CONST_CLASS::CONDITION_LEAF_CONST_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
}

//>>===========================================================================

CONDITION_LEAF_CONST_CLASS::CONDITION_LEAF_CONST_CLASS(string constant)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	constantM = constant;
}

//>>===========================================================================

CONDITION_LEAF_CONST_CLASS::~CONDITION_LEAF_CONST_CLASS()

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

void CONDITION_LEAF_CONST_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs leaf constant
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 0, "\"%s\"", constantM.c_str());
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_LEAF_CONST_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, 
									DCM_ATTRIBUTE_GROUP_CLASS*,
									LOG_CLASS*)

//  DESCRIPTION     : Evaluates leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return CONDITION_TRUE;
}

//*****************************************************************************
//  CONDITION_LEAF_VALUE_NR_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_LEAF_VALUE_NR_CLASS::CONDITION_LEAF_VALUE_NR_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	value_nrM = 0;
}

//>>===========================================================================

CONDITION_LEAF_VALUE_NR_CLASS::CONDITION_LEAF_VALUE_NR_CLASS(UINT16 value_nr)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	value_nrM = value_nr;
}

//>>===========================================================================

CONDITION_LEAF_VALUE_NR_CLASS::~CONDITION_LEAF_VALUE_NR_CLASS()

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

void CONDITION_LEAF_VALUE_NR_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs leaf value nr
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
  		logger_ptr->text(LOG_DEBUG, 0, "%i", value_nrM);
    }
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_LEAF_VALUE_NR_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, 
									   DCM_ATTRIBUTE_GROUP_CLASS*,
									   LOG_CLASS*)

//  DESCRIPTION     : Evaluates leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return CONDITION_TRUE;
}

//*****************************************************************************
//  CONDITION_LEAF_TRUE_CLASS
//*****************************************************************************

//>>===========================================================================

CONDITION_LEAF_TRUE_CLASS::CONDITION_LEAF_TRUE_CLASS(bool value)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetConditionType(CONDITION_TYPE_NORMAL);
	valueM = value;
}

//>>===========================================================================

CONDITION_LEAF_TRUE_CLASS::~CONDITION_LEAF_TRUE_CLASS()

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

void CONDITION_LEAF_TRUE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs leaf true
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		if (valueM)
		{
			logger_ptr->text(LOG_DEBUG, 0, "TRUE");
		}
		else
		{
			logger_ptr->text(LOG_DEBUG, 0, "FALSE");
		}
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_LEAF_TRUE_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*)

//  DESCRIPTION     : Evaluates leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
   	// clear result message
    result_messageM.erase();

    if (valueM)
    {
        AddMessage("TRUE");
    }
    else
    {
        AddMessage("FALSE");
    }

	// return default evaluation result
	return (valueM ? CONDITION_TRUE : CONDITION_FALSE);
}


//*****************************************************************************
//  S T A T I C     F U N C T I O N S
//*****************************************************************************

//>>===========================================================================

static DVT_STATUS CompareAttributeValue(DCM_ATTRIBUTE_CLASS* attr_ptr, UINT16 value_nr, string ref_value)

//  DESCRIPTION     : Compares attribute values specified in conditions and the
//                    actual values
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DVT_STATUS result = MSG_OK;

    // get the indexed source value
	BASE_VALUE_CLASS *src_value_ptr = attr_ptr->GetValue(value_nr - 1);

	// create a reference value
	BASE_VALUE_CLASS *ref_value_ptr = CreateNewValue(attr_ptr->GetVR());

	if ((src_value_ptr) &&
		(ref_value_ptr))
	{
		// set the reference value
		ref_value_ptr->Set(ref_value);

		// compare the values
		result = src_value_ptr->Compare(NULL, ref_value_ptr);
	}

	// return comparision result
	return result;
}

//>>===========================================================================

static CONDITION_RESULT_ENUM GetConditionAndResult(CONDITION_RESULT_ENUM result1, CONDITION_RESULT_ENUM result2)

//  DESCRIPTION     : Return CONDITION_TRUE if both results are CONDITION_TRUE
//					: Return CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION if one of the results is CONDITION_TRUE and the other CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION
//					: Return CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION if both results are CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_FALSE;

	if (result1 == CONDITION_TRUE) 
	{
		if (result2 == CONDITION_TRUE)
		{
			result = CONDITION_TRUE;
		}
		else if (result2 == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION)
		{
			result = CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION;
		}
	}
	else if (result1 == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION) 
	{
		if ((result2 == CONDITION_TRUE) ||
			(result2 == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION))
		{
			result = CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION;
		}
	}

	return result;
}

//>>===========================================================================

static CONDITION_RESULT_ENUM GetConditionOrResult(CONDITION_RESULT_ENUM result1, CONDITION_RESULT_ENUM result2)

//  DESCRIPTION     : Return CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION if one of the results is CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION
//					: or return CONDITION_TRUE if one of the results is CONDITION_TRUE
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	CONDITION_RESULT_ENUM result = CONDITION_FALSE;

	if ((result1 == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION) ||
		(result2 == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION))
	{
		result = CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION;
	}
	else if ((result1 == CONDITION_TRUE) ||
		(result2 == CONDITION_TRUE))
	{
		result = CONDITION_TRUE;
	}

	return result;
}
