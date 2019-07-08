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
//  FILENAME        :	ConditionDefinition.cpp
//  PACKAGE         :	DVT
//  COMPONENT       :	DEFINITION
//  DESCRIPTION     :	Condition Definition Class
//  COPYRIGHT(c)    :   2000, Philips Electronics N.V.
//                      2000, Agfa Gevaert N.V.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ConditionDefinition.h"
#include "ConditionNode.h"
#include "Idicom.h"			// DICOM component interface


//>>===========================================================================

DEF_CONDITION_CLASS::DEF_CONDITION_CLASS() 

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

DEF_CONDITION_CLASS::~DEF_CONDITION_CLASS()

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

bool DEF_CONDITION_CLASS::Evaluate(DCM_ATTRIBUTE_GROUP_CLASS* obj1_ptr, 
								DCM_ATTRIBUTE_GROUP_CLASS* obj2_ptr,
								LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Evaluates condition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	if (nodeM_ptr != NULL)
	{
		result = nodeM_ptr->Evaluate(obj1_ptr, obj2_ptr, logger_ptr);
	}

	return result;
}

//>>===========================================================================

string DEF_CONDITION_CLASS::GetResultMessage()

//  DESCRIPTION     : Returns overall message for this condition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string resultMessage;

	if (nodeM_ptr != NULL)
	{
		resultMessage = nodeM_ptr->GetResultMessage(); 
	}

	return resultMessage;
}
