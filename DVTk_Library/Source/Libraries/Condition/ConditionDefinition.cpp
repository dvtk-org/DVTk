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
#include "ConditionDefinition.h"
#include "ConditionNode.h"
#include "Idicom.h"			// DICOM component interface


//>>===========================================================================

CONDITION_CLASS::CONDITION_CLASS() 

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	primaryNodeM_ptr = NULL;
	secondaryNodeM_ptr = NULL;
}

//>>===========================================================================

CONDITION_CLASS::~CONDITION_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (primaryNodeM_ptr)
	{
		delete primaryNodeM_ptr;
	}

	if (secondaryNodeM_ptr)
	{
		delete secondaryNodeM_ptr;
	}
}

//>>===========================================================================

CONDITION_RESULT_ENUM CONDITION_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates condition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Condition can evaluate to TRUE, FALSE or UNIMPORTANT (don't care).
//<<===========================================================================
{
	CONDITION_RESULT_ENUM conditionResult = CONDITION_FALSE;

    // check that we have at least one incoming object
    if (obj1_ptr == NULL) return conditionResult;

	if (primaryNodeM_ptr != NULL)
	{
		// evaluate the primary condition
		conditionResult = primaryNodeM_ptr->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);
		if ((conditionResult == CONDITION_FALSE) &&
			(secondaryNodeM_ptr != NULL))
		{
			// if primary condition evaluates false and there is a secondary
			// condition then the secondary condition should be evaluated too
			CONDITION_RESULT_ENUM secondaryConditionResult = secondaryNodeM_ptr->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);
			if ((secondaryConditionResult == CONDITION_TRUE) ||
				(secondaryConditionResult == CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION))
			{
				// secondary condition is true
				// - condition is unimportant
				conditionResult = CONDITION_UNIMPORTANT;
			}
		}
	}

	return conditionResult;
}

//>>===========================================================================

void CONDITION_CLASS::Log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log the condition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    if (logger_ptr == NULL) return;

	if (primaryNodeM_ptr != NULL)
	{
        logger_ptr->text(LOG_DEBUG, 1, "Primary Condition");

		// log the primary condition
		primaryNodeM_ptr->Log(logger_ptr);
	}

    if (secondaryNodeM_ptr != NULL)
	{
        logger_ptr->text(LOG_DEBUG, 1, "Secondary Condition");

        // log the secondary condition
		secondaryNodeM_ptr->Log(logger_ptr);
	}
}

//>>===========================================================================

string CONDITION_CLASS::GetResultMessage()

//  DESCRIPTION     : Returns overall message for this condition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string resultMessage;

	if (primaryNodeM_ptr != NULL)
	{
		resultMessage = primaryNodeM_ptr->GetResultMessage(); 
	}

	if (secondaryNodeM_ptr != NULL)
	{
		resultMessage = "WEAK ( " + resultMessage + ", " + secondaryNodeM_ptr->GetResultMessage() + " )";
	}

	return resultMessage;
}
