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
#include "AttributeDefinition.h"
#include "AttributeRegister.h"


//>>===========================================================================

DEF_ATTRIBUTE_REGISTER_CLASS::DEF_ATTRIBUTE_REGISTER_CLASS(DEF_ATTRIBUTE_CLASS *referenceAttribute_ptr)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	referenceAttributeM = *referenceAttribute_ptr;
	referenceCountM = 1;
}

//>>===========================================================================

DEF_ATTRIBUTE_REGISTER_CLASS::~DEF_ATTRIBUTE_REGISTER_CLASS()

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
		
DEF_ATTRIBUTE_CLASS *DEF_ATTRIBUTE_REGISTER_CLASS::GetReferenceAttribute()

//  DESCRIPTION     : Get the referenced attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return referenced attribute
	return &referenceAttributeM;
}

//>>===========================================================================

void DEF_ATTRIBUTE_REGISTER_CLASS::IncrementReferenceCount()

//  DESCRIPTION     : Increment the reference attribute count
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// increment the reference count
	referenceCountM++;
}

//>>===========================================================================
		
void DEF_ATTRIBUTE_REGISTER_CLASS::DecrementReferenceCount()

//  DESCRIPTION     : Decrement the reference attribute count
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// decrement the reference count - don't let it go below zero
	if (referenceCountM)
	{
		referenceCountM--;
	}
}

//>>===========================================================================
		
int DEF_ATTRIBUTE_REGISTER_CLASS::GetReferenceCount()

//  DESCRIPTION     : Get the reference attribute count
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return the reference count
	return referenceCountM;
}
