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
#include "stored_sop_instance.h"
#include "Ilog.h"					// Log component interface

//>>===========================================================================

STORED_SOP_INSTANCE_CLASS::STORED_SOP_INSTANCE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	countM = 1;
	committedM = false;
}

//>>===========================================================================

STORED_SOP_INSTANCE_CLASS::STORED_SOP_INSTANCE_CLASS(string sopClassUid, string sopInstanceUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sopClassUidM = sopClassUid;
	sopInstanceUidM = sopInstanceUid;
	countM = 1;
	committedM = false;
}

//>>===========================================================================

STORED_SOP_INSTANCE_CLASS::~STORED_SOP_INSTANCE_CLASS()

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

void STORED_SOP_INSTANCE_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log the SOP Instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid logger
	if (logger_ptr == NULL) return;

	// display sop data
	if (committedM)
	{
		logger_ptr->text(LOG_INFO, 1, "\t\tSOP Instance has been COMMITTED for storage");
	}
	logger_ptr->text(LOG_INFO, 1, "\t\t(0008,0016) Object(Image) SOP Class UID: %s", sopClassUidM.c_str());
	logger_ptr->text(LOG_INFO, 1, "\t\t(0008,0018) Object(Image) SOP Instance UID: %s", sopInstanceUidM.c_str());
	
	if (countM > 1) 
	{
		logger_ptr->text(LOG_INFO, 1, "%d objects with SOP Instance UID: %s have duplicate instance UID", countM, sopInstanceUidM.c_str());
	}
}
