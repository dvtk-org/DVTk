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
//  FILENAME        : ConditionNode.cpp
//  PACKAGE         : DVT
//  COMPONENT       : DEFINITION
//  DESCRIPTION     : Condition node classes.
//  COPYRIGHT(c)    : 2003, Philips Electronics N.V.
//                    2003, Agfa Gevaert N.V.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ConditionNode.h"
#include "IAttributeGroup.h"
#include "Iutility.h"
#include <math.h>


//*****************************************************************************
//  INTERNAL DEFINITIONS
//*****************************************************************************
//
// Search direction map
//
static T_SEARCH_DIRECTION_MAP	TSearchDirectionMap[] =
{
{SEARCH_DIRECTION_ALL,  ""},
{SEARCH_DIRECTION_UP,   "UP"},
{SEARCH_DIRECTION_DOWN, "DOWN"},
{SEARCH_DIRECTION_HERE, "HERE"}
};

static const char* MapSearchDirection(SEARCH_DIRECTION_ENUM direction);

static DVT_STATUS CompareAttributeValue(ATTRIBUTE_CLASS* attr_ptr, UINT16 value_nr, string ref_value);


//*****************************************************************************
//  DEF_COND_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_NODE_CLASS::~DEF_COND_NODE_CLASS()

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

void DEF_COND_NODE_CLASS::AddMessage(char* format_ptr, ...)

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

string DEF_COND_NODE_CLASS::GetResultMessage()

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

//*****************************************************************************
//  COND_BINARY_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_BINARY_NODE_CLASS::DEF_COND_BINARY_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	leftM_ptr = NULL;
	rightM_ptr = NULL;
}

//>>===========================================================================

DEF_COND_BINARY_NODE_CLASS::DEF_COND_BINARY_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr, DEF_COND_NODE_CLASS* right_ptr) 

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	leftM_ptr = left_ptr;
	rightM_ptr = right_ptr;
}

//>>===========================================================================

DEF_COND_BINARY_NODE_CLASS::~DEF_COND_BINARY_NODE_CLASS()

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

void DEF_COND_BINARY_NODE_CLASS::SetLeft(DEF_COND_NODE_CLASS* left_ptr)
 
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

void DEF_COND_BINARY_NODE_CLASS::SetRight(DEF_COND_NODE_CLASS* right_ptr) 

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

void DEF_COND_BINARY_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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

bool DEF_COND_BINARY_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									 ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									 LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return false;
}

//*****************************************************************************
//  DEF_COND_AND_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_AND_NODE_CLASS::DEF_COND_AND_NODE_CLASS()

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

DEF_COND_AND_NODE_CLASS::DEF_COND_AND_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr,	DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_AND_NODE_CLASS::~DEF_COND_AND_NODE_CLASS()

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

void DEF_COND_AND_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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
		logger_ptr->text(LOG_NONE, 0, " AND ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_AND_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								  ATTRIBUTE_GROUP_CLASS* obj2_ptr,
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
    bool left_result = GetLeft()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// evaluate the right node
	bool right_result = GetRight()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// result is true if left and right are true
	bool result = (left_result && right_result);
	if (result)
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[%s AND %s]", GetLeft()->GetResultMessage().c_str(), GetRight()->GetResultMessage().c_str());

	return result;
}


//*****************************************************************************
//  DEF_COND_OR_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_OR_NODE_CLASS::DEF_COND_OR_NODE_CLASS()

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

DEF_COND_OR_NODE_CLASS::DEF_COND_OR_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr, DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_OR_NODE_CLASS::~DEF_COND_OR_NODE_CLASS()

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

void DEF_COND_OR_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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
		logger_ptr->text(LOG_NONE, 0, " OR ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_OR_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								 ATTRIBUTE_GROUP_CLASS* obj2_ptr,
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
    bool left_result = GetLeft()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// evaluate right node
	bool right_result = GetRight()->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);

	// result is true if either left or right is true
	bool result = (left_result || right_result);
	if (result)
	{
		AddMessage("T");
	}
	else
	{
		AddMessage("F");
	}
	AddMessage("[%s OR %s]", GetLeft()->GetResultMessage().c_str(), GetRight()->GetResultMessage().c_str());    

	return result;
}

//*****************************************************************************
//  DEF_COND_PRESENT_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_PRESENT_NODE_CLASS::DEF_COND_PRESENT_NODE_CLASS()

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

DEF_COND_PRESENT_NODE_CLASS::DEF_COND_PRESENT_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr,	DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_PRESENT_NODE_CLASS::~DEF_COND_PRESENT_NODE_CLASS()

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

void DEF_COND_PRESENT_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_NONE, 0, "PRESENT ");
		GetRight()->Log(logger_ptr);
		GetLeft()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_PRESENT_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									  ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   : The left branch contains the tag
//                    The right branch contains the search direction
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool result = true;
	UINT16 group = static_cast<DEF_COND_LEAF_TAG_CLASS*>(GetLeft())->GetGroup();
	UINT16 element = static_cast<DEF_COND_LEAF_TAG_CLASS*>(GetLeft())->GetElement();

	SEARCH_DIRECTION_ENUM search_direction = static_cast<DEF_COND_LEAF_DIRECTION_CLASS*>(GetRight())->GetSearchDirection();
	
	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
	ATTRIBUTE_CLASS* attr_ptr = obj1_ptr->GetAttribute(group, element);

	// only if there are not any restrictions on the search direction
	// check the co-object
	if (search_direction == SEARCH_DIRECTION_ALL)
	{
		if (attr_ptr == NULL && obj2_ptr != NULL)
		{
			// check whether the attribute is present in the co-object
			attr_ptr = obj2_ptr->GetAttribute(group, element);
		}
	}

	// we didn't find the attribute
    if (attr_ptr == NULL || !(attr_ptr->IsPresent()))
	{
		result = false;
		AddMessage("F");
	}
	else
	{
        AddMessage("T");
	}
	AddMessage("[PRESENT %s 0x%04X%04X]", MapSearchDirection(search_direction), group, element);

	return result;
}

//*****************************************************************************
//  DEF_COND_VALUE_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_VALUE_NODE_CLASS::DEF_COND_VALUE_NODE_CLASS()

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

DEF_COND_VALUE_NODE_CLASS::DEF_COND_VALUE_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr,	DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_VALUE_NODE_CLASS::~DEF_COND_VALUE_NODE_CLASS()

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

void DEF_COND_VALUE_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
	    logger_ptr->text(LOG_NONE, 0, "VALUE ");
		GetLeft()->Log(logger_ptr);
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_VALUE_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return true;
}

//>>===========================================================================

UINT16 DEF_COND_VALUE_NODE_CLASS::GetGroup()

//  DESCRIPTION     : returns group 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return static_cast<DEF_COND_LEAF_TAG_CLASS*>(GetLeft())->GetGroup();
}

//>>===========================================================================

UINT16 DEF_COND_VALUE_NODE_CLASS::GetElement()

//  DESCRIPTION     : returns element
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return static_cast<DEF_COND_LEAF_TAG_CLASS*>(GetLeft())->GetElement();
}

//>>===========================================================================

UINT16 DEF_COND_VALUE_NODE_CLASS::GetValueNr()

//  DESCRIPTION     : returns value number
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return static_cast<DEF_COND_LEAF_VALUE_NR_CLASS*>(GetRight())->GetValueNr();
}

//*****************************************************************************
//  DEF_COND_LESS_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_LESS_NODE_CLASS::DEF_COND_LESS_NODE_CLASS()

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

DEF_COND_LESS_NODE_CLASS::DEF_COND_LESS_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr, DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_LESS_NODE_CLASS::~DEF_COND_LESS_NODE_CLASS()

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

void DEF_COND_LESS_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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
		logger_ptr->text(LOG_NONE, 0, " < ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_LESS_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								   ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								   LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	UINT16 group     = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetGroup();
	UINT16 element   = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetElement();
    UINT16 value_nr  = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<DEF_COND_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
	ATTRIBUTE_CLASS* attr_ptr = obj1_ptr->GetAttribute(group, element);
	if (attr_ptr == NULL && obj2_ptr != NULL)
	{
		//check whether the attribute is present in the co-object
		attr_ptr = obj2_ptr->GetAttribute(group, element);
	}

    if (attr_ptr != NULL && attr_ptr->IsPresent())
	{
        UINT16 nr_values = attr_ptr->GetNrValues();

		if (value_nr > nr_values && logger_ptr)
		{
            logger_ptr->text(LOG_INFO, 1, "Value %i for attribute (%04X,%04X) does not exist",
				              value_nr, group, element);
            logger_ptr->text(LOG_NONE, 1, "Check condition");
		}
		else
		{
            DVT_STATUS cmp_result = CompareAttributeValue(attr_ptr, value_nr, const_val);
			if (cmp_result == MSG_SMALLER)
			{
                result = true;
			}
		}
	}

	if (result)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[VALUE 0x%04X%04X %i < %s]", group, element, value_nr, const_val.c_str());

	return result;
}

//*****************************************************************************
//  DEF_COND_LESS_EQ_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_LESS_EQ_NODE_CLASS::DEF_COND_LESS_EQ_NODE_CLASS()

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

DEF_COND_LESS_EQ_NODE_CLASS::DEF_COND_LESS_EQ_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr,	DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_LESS_EQ_NODE_CLASS::~DEF_COND_LESS_EQ_NODE_CLASS()

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

void DEF_COND_LESS_EQ_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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
		logger_ptr->text(LOG_NONE, 0, " <= ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_LESS_EQ_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									  ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	UINT16 group     = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetGroup();
	UINT16 element   = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetElement();
    UINT16 value_nr  = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<DEF_COND_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();
	
	// check whether the attribute is present in the object
	ATTRIBUTE_CLASS* attr_ptr = obj1_ptr->GetAttribute(group, element);

	if (attr_ptr == NULL && obj2_ptr != NULL)
	{
		// check whether the attribute is present in the co-object
		attr_ptr = obj2_ptr->GetAttribute(group, element);
	}

    if (attr_ptr != NULL && attr_ptr->IsPresent())
	{
        UINT16 nr_values = attr_ptr->GetNrValues();
		if (value_nr > nr_values && logger_ptr)
		{
            logger_ptr->text(LOG_INFO, 1, "Value %i for attribute (%04X,%04X) does not exist",
				              value_nr, group, element);
            logger_ptr->text(LOG_NONE, 1, "Check condition");
		}
		else
		{
            DVT_STATUS cmp_result = CompareAttributeValue(attr_ptr, value_nr, const_val);
			if (cmp_result == MSG_SMALLER ||
				cmp_result == MSG_EQUAL)
			{
                result = true;
			}
		}
	}

	if (result)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[VALUE 0x%04X%04X %i <= %s]", group, element, value_nr, const_val.c_str());

	return result;
}

//*****************************************************************************
//  DEF_COND_EQ_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_EQ_NODE_CLASS::DEF_COND_EQ_NODE_CLASS()

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

DEF_COND_EQ_NODE_CLASS::DEF_COND_EQ_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr, DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_EQ_NODE_CLASS::~DEF_COND_EQ_NODE_CLASS()

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

void DEF_COND_EQ_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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
		logger_ptr->text(LOG_NONE, 0, " = ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_EQ_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								 ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								 LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	UINT16 group     = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetGroup();
	UINT16 element   = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetElement();
    UINT16 value_nr  = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<DEF_COND_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
	ATTRIBUTE_CLASS* attr_ptr = obj1_ptr->GetAttribute(group, element);
	if (attr_ptr == 0 && obj2_ptr != 0)
	{
		// check whether the attribute is present in the co-object
		attr_ptr = obj2_ptr->GetAttribute(group, element);
	}

    if ( attr_ptr != 0 && attr_ptr->IsPresent())
	{
        UINT16 nr_values = attr_ptr->GetNrValues();
		if (value_nr > nr_values && logger_ptr)
		{
            logger_ptr->text(LOG_INFO, 1, "Value %i for attribute (%04X,%04X) does not exist",
				              value_nr, group, element);
            logger_ptr->text(LOG_NONE, 1, "Check condition");
		}
		else
		{
            DVT_STATUS cmp_result = CompareAttributeValue(attr_ptr, value_nr, const_val);
			if (cmp_result == MSG_EQUAL)
			{
                result = true;
			}
		}
	}

	if (result)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[VALUE 0x%04X%04X %i = %s]", group, element, value_nr, const_val.c_str());

	return result;
}


//*****************************************************************************
//  DEF_COND_GREATER_EQ_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_GREATER_EQ_NODE_CLASS::DEF_COND_GREATER_EQ_NODE_CLASS()

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

DEF_COND_GREATER_EQ_NODE_CLASS::DEF_COND_GREATER_EQ_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr, DEF_COND_NODE_CLASS* right_ptr)
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_GREATER_EQ_NODE_CLASS::~DEF_COND_GREATER_EQ_NODE_CLASS()

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

void DEF_COND_GREATER_EQ_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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
		logger_ptr->text(LOG_NONE, 0, " >= ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_GREATER_EQ_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
										 ATTRIBUTE_GROUP_CLASS* obj2_ptr,
										 LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	UINT16 group     = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetGroup();
	UINT16 element   = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetElement();
    UINT16 value_nr  = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<DEF_COND_LEAF_CONST_CLASS*>(GetRight())->GetConstant();

	// clear result message
    result_messageM.erase();

	// check whether the attribute is present in the object
	ATTRIBUTE_CLASS* attr_ptr = obj1_ptr->GetAttribute(group, element);
	if (attr_ptr == NULL && obj2_ptr != NULL)
	{
		// check whether the attribute is present in the co-object
		attr_ptr = obj2_ptr->GetAttribute(group, element);
	}

    if ( attr_ptr != NULL && attr_ptr->IsPresent())
	{
        UINT16 nr_values = attr_ptr->GetNrValues();
		if (value_nr > nr_values && logger_ptr)
		{
            logger_ptr->text(LOG_INFO, 1, "Value %i for attribute (%04X,%04X) does not exist",
				              value_nr, group, element);
            logger_ptr->text(LOG_NONE, 1, "Check condition");
		}
		else
		{
            DVT_STATUS cmp_result = CompareAttributeValue(attr_ptr, value_nr, const_val);
			if (cmp_result == MSG_GREATER ||
				cmp_result == MSG_EQUAL)
			{
                result = true;
			}
		}
	}

	if (result)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[VALUE 0x%04X%04X %i >= %s]", group, element, value_nr, const_val.c_str());

	return result;
}

//*****************************************************************************
//  DEF_COND_GREATER_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_GREATER_NODE_CLASS::DEF_COND_GREATER_NODE_CLASS()

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

DEF_COND_GREATER_NODE_CLASS::DEF_COND_GREATER_NODE_CLASS(DEF_COND_NODE_CLASS* left_ptr,	DEF_COND_NODE_CLASS* right_ptr) 
: DEF_COND_BINARY_NODE_CLASS(left_ptr, right_ptr)

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

DEF_COND_GREATER_NODE_CLASS::~DEF_COND_GREATER_NODE_CLASS()

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

void DEF_COND_GREATER_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

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
		logger_ptr->text(LOG_NONE, 0, " > ");
		GetRight()->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_GREATER_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									  ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	UINT16 group     = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetGroup();
	UINT16 element   = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetElement();
    UINT16 value_nr  = static_cast<DEF_COND_VALUE_NODE_CLASS*>(GetLeft())->GetValueNr();
	string const_val = static_cast<DEF_COND_LEAF_CONST_CLASS*>(GetRight())->GetConstant();
	
	// clear result message
    result_messageM.erase();
	
	// check whether the attribute is present in the object
	ATTRIBUTE_CLASS* attr_ptr = obj1_ptr->GetAttribute(group, element);
	if (attr_ptr == NULL && obj2_ptr != NULL)
	{
		// check whether the attribute is present in the co-object
		attr_ptr = obj2_ptr->GetAttribute(group, element);
	}

    if ( attr_ptr != NULL && attr_ptr->IsPresent())
	{
        UINT16 nr_values = attr_ptr->GetNrValues();
		if (value_nr > nr_values && logger_ptr)
		{
            logger_ptr->text(LOG_INFO, 1, "Value %i for attribute (%04X,%04X) does not exist",
				              value_nr, group, element);
            logger_ptr->text(LOG_NONE, 1, "Check condition");
		}
		else
		{
            DVT_STATUS cmp_result = CompareAttributeValue(attr_ptr, value_nr, const_val);
			if (cmp_result == MSG_GREATER)
			{
                result = true;
			}
		}
	}

	if (result)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[VALUE 0x%04X%04X %i > %s]", group, element, value_nr, const_val.c_str());

	return result;
}

//*****************************************************************************
//  DEF_COND_UNARY_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_UNARY_NODE_CLASS::DEF_COND_UNARY_NODE_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	nodeM_ptr = NULL;
}

//>>===========================================================================

DEF_COND_UNARY_NODE_CLASS::DEF_COND_UNARY_NODE_CLASS(DEF_COND_NODE_CLASS* node_ptr)

//  DESCRIPTION     : Constructor with arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	nodeM_ptr = node_ptr;
}

//>>===========================================================================

DEF_COND_UNARY_NODE_CLASS::~DEF_COND_UNARY_NODE_CLASS()

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

void DEF_COND_UNARY_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		nodeM_ptr->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_UNARY_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return true;
}

//*****************************************************************************
//  DEF_COND_EMPTY_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_EMPTY_NODE_CLASS::DEF_COND_EMPTY_NODE_CLASS()

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

DEF_COND_EMPTY_NODE_CLASS::DEF_COND_EMPTY_NODE_CLASS(DEF_COND_NODE_CLASS* node_ptr) 
: DEF_COND_UNARY_NODE_CLASS(node_ptr)

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

DEF_COND_EMPTY_NODE_CLASS::~DEF_COND_EMPTY_NODE_CLASS()

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

void DEF_COND_EMPTY_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_NONE, 0, "EMPTY ");
		nodeM_ptr->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_EMPTY_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	UINT16 group     = static_cast<DEF_COND_LEAF_TAG_CLASS*>(nodeM_ptr)->GetGroup();
	UINT16 element   = static_cast<DEF_COND_LEAF_TAG_CLASS*>(nodeM_ptr)->GetElement();
	
	// clear result message
    result_messageM.erase();
	
	// check whether the attribute is present in the object
	ATTRIBUTE_CLASS* attr_ptr = obj1_ptr->GetAttribute(group, element);
	if (attr_ptr == NULL && obj2_ptr != NULL)
	{
		// check whether the attribute is present in the co-object
		attr_ptr = obj2_ptr->GetAttribute(group, element);
	}

	// if the attribute exists, check whether it is empty. 
	// if the attribute does not exist it certainly is empty!
    if ( attr_ptr != NULL && attr_ptr->IsPresent())
	{
		UINT nr_values = attr_ptr->GetNrValues();
		if (nr_values == 0)
		{
            result = true;
		}
		else
		{
			// the attribute has values but they may be empty
			for (UINT i = 0; i < nr_values; ++i)
			{
				BASE_VALUE_CLASS* val_ptr = attr_ptr->GetValue(i);
				if (val_ptr->GetLength())
				{
					result = false;
				}
			}
		}
	}

	if (result)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[EMPTY 0x%04X%04X]", group, element);

	return result;
}

//*****************************************************************************
//  DEF_COND_NOT_NODE_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_NOT_NODE_CLASS::DEF_COND_NOT_NODE_CLASS()

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

DEF_COND_NOT_NODE_CLASS::DEF_COND_NOT_NODE_CLASS(DEF_COND_NODE_CLASS* node_ptr) 
: DEF_COND_UNARY_NODE_CLASS(node_ptr)

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

DEF_COND_NOT_NODE_CLASS::~DEF_COND_NOT_NODE_CLASS()

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

void DEF_COND_NOT_NODE_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs node
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_NONE, 0, "NOT ");
		nodeM_ptr->Log(logger_ptr);
	}
}

//>>===========================================================================

bool DEF_COND_NOT_NODE_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								  ATTRIBUTE_GROUP_CLASS* obj2_ptr,
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

	bool result = !(nodeM_ptr->Evaluate(obj1_ptr, obj2_ptr, logger_ptr));

	if (result)
	{
		AddMessage("T");
	}
	else
	{
        AddMessage("F");
	}
	AddMessage("[NOT %s]", nodeM_ptr->GetResultMessage().c_str());

	return result;
}

//*****************************************************************************
//  DEF_COND_LEAF_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_LEAF_CLASS::~DEF_COND_LEAF_CLASS()

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
//  DEF_COND_LEAF_TAG_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_LEAF_TAG_CLASS::DEF_COND_LEAF_TAG_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	tag_groupM   = 0;
	tag_elementM = 0;
}

//>>===========================================================================

DEF_COND_LEAF_TAG_CLASS::DEF_COND_LEAF_TAG_CLASS(UINT16 taggroup, UINT16 tagelement)

//  DESCRIPTION     : Constructor with tag arguments
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	tag_groupM   = taggroup;
	tag_elementM = tagelement;
}

//>>===========================================================================

DEF_COND_LEAF_TAG_CLASS::~DEF_COND_LEAF_TAG_CLASS()

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

void DEF_COND_LEAF_TAG_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_NONE, 0, "(%04X,%04X)", tag_groupM, tag_elementM);
	}
}

//>>===========================================================================

bool DEF_COND_LEAF_TAG_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								  ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// resurn default evaluation result
	return true;
}

//*****************************************************************************
//  DEF_COND_LEAF_CONST_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_LEAF_CONST_CLASS::DEF_COND_LEAF_CONST_CLASS()

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

DEF_COND_LEAF_CONST_CLASS::DEF_COND_LEAF_CONST_CLASS(string constant)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	constantM = constant;
}

//>>===========================================================================

DEF_COND_LEAF_CONST_CLASS::~DEF_COND_LEAF_CONST_CLASS()

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

void DEF_COND_LEAF_CONST_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs leaf constant
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_NONE, 0, "\"%s\"", constantM.c_str());
	}
}

//>>===========================================================================

bool DEF_COND_LEAF_CONST_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return true;
}

//*****************************************************************************
//  DEF_COND_LEAF_VALUE_NR_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_LEAF_VALUE_NR_CLASS::DEF_COND_LEAF_VALUE_NR_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	value_nrM = 0;
}

//>>===========================================================================

DEF_COND_LEAF_VALUE_NR_CLASS::DEF_COND_LEAF_VALUE_NR_CLASS(UINT16 value_nr)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	value_nrM = value_nr;
}

//>>===========================================================================

DEF_COND_LEAF_VALUE_NR_CLASS::~DEF_COND_LEAF_VALUE_NR_CLASS()

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

void DEF_COND_LEAF_VALUE_NR_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs leaf value nr
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_NONE, 0, "%i", value_nrM);
	}
}

//>>===========================================================================

bool DEF_COND_LEAF_VALUE_NR_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
									   ATTRIBUTE_GROUP_CLASS* obj2_ptr,
									   LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return true;
}

//*****************************************************************************
//  DEF_COND_LEAF_DIRECTION_CLASS
//*****************************************************************************

//>>===========================================================================

DEF_COND_LEAF_DIRECTION_CLASS::DEF_COND_LEAF_DIRECTION_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activity
	directionM = SEARCH_DIRECTION_ALL;
}

//>>===========================================================================

DEF_COND_LEAF_DIRECTION_CLASS::DEF_COND_LEAF_DIRECTION_CLASS(SEARCH_DIRECTION_ENUM direction)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activity
	directionM = direction;
}

//>>===========================================================================

DEF_COND_LEAF_DIRECTION_CLASS::~DEF_COND_LEAF_DIRECTION_CLASS()

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

void DEF_COND_LEAF_DIRECTION_CLASS::Log(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : logs leaf search direction
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_NONE, 0, "%s", MapSearchDirection(directionM));
	}
}

//>>===========================================================================

bool DEF_COND_LEAF_DIRECTION_CLASS::Evaluate(ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
										ATTRIBUTE_GROUP_CLASS* obj2_ptr,
										LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates leaf
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return default evaluation result
	return true;
}

//*****************************************************************************
//  S T A T I C     F U N C T I O N S
//*****************************************************************************

//>>===========================================================================

static const char* MapSearchDirection(SEARCH_DIRECTION_ENUM direction)

//  DESCRIPTION     : Function to map the search direction value 
//                    to a meaningfull string
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int i = 0;
	
	while (TSearchDirectionMap[i].direction != direction)
	{
		i++;
	}

	// return matching command name
	return TSearchDirectionMap[i].directionName;
}

//>>===========================================================================

static DVT_STATUS CompareAttributeValue(ATTRIBUTE_CLASS* attr_ptr, UINT16 value_nr, string ref_value)

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
		result = src_value_ptr->Compare(ref_value_ptr);
	}

	// return comparision result
	return result;
}


