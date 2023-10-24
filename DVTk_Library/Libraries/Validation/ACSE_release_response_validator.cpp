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
#include "ACSE_release_response_validator.h"
#include "Iglobal.h"      // Global component interface file
#include "Ilog.h"         // Logging component interface file

//>>===========================================================================		

RELEASE_RP_VALIDATOR_CLASS::RELEASE_RP_VALIDATOR_CLASS()

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

RELEASE_RP_VALIDATOR_CLASS::~RELEASE_RP_VALIDATOR_CLASS()

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
bool
RELEASE_RP_VALIDATOR_CLASS::validate(RELEASE_RP_CLASS *srcReleaseRp_ptr,
                                     RELEASE_RP_CLASS *)
									 //  DESCRIPTION     : Validate Release Response.
									 //  PRECONDITIONS   :
									 //  POSTCONDITIONS  :
									 //  EXCEPTIONS      : 
									 //  NOTES           :
									 //<<===========================================================================		
{
	// check for valid source
	if (srcReleaseRp_ptr == NULL) return false;
	
	// nothing to validate
	return true;
} 
