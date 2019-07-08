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
#include "script_session.h"
#include "script_context.h"
#include "Ivalidation.h"		// Validation component interface


//>>===========================================================================

SCRIPT_EXECUTION_CONTEXT_CLASS::SCRIPT_EXECUTION_CONTEXT_CLASS(SCRIPT_SESSION_CLASS *scriptSession_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	validationFlagM = ALL;
	strictValidationM = true;
	defineSqLengthM = false;
	addGroupLengthM = false;
	autoType2AttributesM = true;
	populateWithAttributesM = true;
    applicationEntityNameM = APPLICATION_ENTITY_NAME;
    applicationEntityVersionM = APPLICATION_ENTITY_VERSION;

	// take defaults from script session
	if (scriptSession_ptr != NULL)
	{
		strictValidationM = scriptSession_ptr->getStrictValidation();
		defineSqLengthM = scriptSession_ptr->getDefineSqLength();
		addGroupLengthM = scriptSession_ptr->getAddGroupLength();
		autoType2AttributesM = scriptSession_ptr->getAutoType2Attributes();
        applicationEntityNameM = scriptSession_ptr->getApplicationEntityName();
        applicationEntityVersionM = scriptSession_ptr->getApplicationEntityVersion();
	}
}

//>>===========================================================================

SCRIPT_EXECUTION_CONTEXT_CLASS::~SCRIPT_EXECUTION_CONTEXT_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setValidationFlag(VALIDATION_CONTROL_FLAG_ENUM flag)

//  DESCRIPTION     : Set the Validation flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	validationFlagM = flag;
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setStrictValidation(bool flag)

//  DESCRIPTION     : Set the Strict Validation flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	strictValidationM = flag;
    VALIDATION->setStrictValidation(flag);
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setDefineSqLength(bool flag)

//  DESCRIPTION     : Set the Define SQ Length flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	defineSqLengthM = flag; 
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setAddGroupLength(bool flag)

//  DESCRIPTION     : Set the Add Group Length flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	addGroupLengthM = flag; 
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setAutoType2Attributes(bool flag)

//  DESCRIPTION     : Set the Auto Type 2 Attributes flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	autoType2AttributesM = flag; 
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setPopulateWithAttributes(bool flag)

//  DESCRIPTION     : Set the Populate With Attributes flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	populateWithAttributesM = flag; 
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setApplicationEntityName(string name)

//  DESCRIPTION     : Set the Application Entity Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	applicationEntityNameM = name;
}

//>>===========================================================================

void SCRIPT_EXECUTION_CONTEXT_CLASS::setApplicationEntityVersion(string version)

//  DESCRIPTION     : Set the Application Entity Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    applicationEntityVersionM = version;
}

//>>===========================================================================

VALIDATION_CONTROL_FLAG_ENUM SCRIPT_EXECUTION_CONTEXT_CLASS::getValidationFlag()

//  DESCRIPTION     : Get the Validation flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return validationFlagM;
}

//>>===========================================================================

bool SCRIPT_EXECUTION_CONTEXT_CLASS::getStrictValidation()

//  DESCRIPTION     : Get the Strict Validation flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return strictValidationM; 
}

//>>===========================================================================

bool SCRIPT_EXECUTION_CONTEXT_CLASS::getDefineSqLength()

//  DESCRIPTION     : Get the Define SQ Length flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return defineSqLengthM; 
}

//>>===========================================================================

bool SCRIPT_EXECUTION_CONTEXT_CLASS::getAddGroupLength()

//  DESCRIPTION     : Get the Add Group Length flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return addGroupLengthM; 
}

//>>===========================================================================

bool SCRIPT_EXECUTION_CONTEXT_CLASS::getPopulateWithAttributes()

//  DESCRIPTION     : Get the Populate With Attributes flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// the return value depends on whether the populateWithAttributes is true or not
	// - if true return the session value defined by autoType2Attributes
	// - if false return false
	bool flag = populateWithAttributesM;
	if (populateWithAttributesM)
	{
		flag = autoType2AttributesM;
	}

	return flag; 
}

//>>===========================================================================

string SCRIPT_EXECUTION_CONTEXT_CLASS::getApplicationEntityName()

//  DESCRIPTION     : Get the Application Entity Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    return applicationEntityNameM;
}

//>>===========================================================================

string SCRIPT_EXECUTION_CONTEXT_CLASS::getApplicationEntityVersion()

//  DESCRIPTION     : Get the Application Entity Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    return applicationEntityVersionM;
}
