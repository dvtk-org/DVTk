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
#include "ACSE_properties.h"
#include "Iglobal.h"		// Global component interface

//*****************************************************************************
//  EXTERNAL DEFINITIONS
//*****************************************************************************

//>>===========================================================================
ACSE_PROPERTIES_CLASS::ACSE_PROPERTIES_CLASS()
//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================
ACSE_PROPERTIES_CLASS::~ACSE_PROPERTIES_CLASS()
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
void ACSE_PROPERTIES_CLASS::setCalledAeTitle (const string calledAeTitle)
//  DESCRIPTION     : set the CalledAeTitle member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	this->calledAeTitleM = calledAeTitle;
}

//>>===========================================================================
string ACSE_PROPERTIES_CLASS::getCalledAeTitle (void)
//  DESCRIPTION     : returns the CalledAeTitle member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (this->calledAeTitleM);
}

//>>===========================================================================
void ACSE_PROPERTIES_CLASS::setCallingAeTitle (const string callingAeTitle)
//  DESCRIPTION     : set the CalledAeTitle member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	this->callingAeTitleM = callingAeTitle;
}

//>>===========================================================================
string ACSE_PROPERTIES_CLASS::getCallingAeTitle (void)
//  DESCRIPTION     : returns the CalledAeTitle member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (this->callingAeTitleM);
}

//>>===========================================================================
void ACSE_PROPERTIES_CLASS::setMaximumLengthReceived (UINT32 maximumLengthReceived)
//  DESCRIPTION     : set the MaximumLengthReceived member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	this->maximumLengthReceivedM = maximumLengthReceived;
}

//>>===========================================================================
UINT32 ACSE_PROPERTIES_CLASS::getMaximumLengthReceived (void)
//  DESCRIPTION     : returns the MaximumLengthReceived member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (this->maximumLengthReceivedM);
}

//>>===========================================================================
void ACSE_PROPERTIES_CLASS::setImplementationClassUid (const string implementationClassUid)
//  DESCRIPTION     : set the ImplementationClassUid member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	this->implementationClassUidM = implementationClassUid;
}

//>>===========================================================================
string ACSE_PROPERTIES_CLASS::getImplementationClassUid (void)
//  DESCRIPTION     : returns the ImplementationClassUid member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (this->implementationClassUidM);
}

//>>===========================================================================
void ACSE_PROPERTIES_CLASS::setImplementationVersionName (const string implementationVersionName)
//  DESCRIPTION     : set the ImplementationVersionName member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	this->implementationVersionNameM = implementationVersionName;
}

//>>===========================================================================
string ACSE_PROPERTIES_CLASS::getImplementationVersionName (void)
//  DESCRIPTION     : returns the ImplementationVersionName member variable
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (this->implementationVersionNameM);
}
