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
#include "series_data.h"
#include "sop_instance_data.h"
#include "Ilog.h"					// Log component interface


//>>===========================================================================

SERIES_DATA_CLASS::SERIES_DATA_CLASS(string instanceUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	instanceUidM = instanceUid;
}
	
//>>===========================================================================

SERIES_DATA_CLASS::~SERIES_DATA_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any sop instance data
	while (sopInstanceDataM.getSize())
	{
		delete sopInstanceDataM[0];
		sopInstanceDataM.removeAt(0);
	}
}

//>>===========================================================================

SOP_INSTANCE_DATA_CLASS *SERIES_DATA_CLASS::search(string instanceUid)

//  DESCRIPTION     : Serach the series for SOP Instance Data with an instance uid
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search sop instance data
	for (UINT i = 0; i < sopInstanceDataM.getSize(); i++) 
	{
		SOP_INSTANCE_DATA_CLASS *sopInstanceData_ptr = sopInstanceDataM[i];

		// check for match
		if ((sopInstanceData_ptr != NULL) &&
			(instanceUid == sopInstanceData_ptr->getInstanceUid()))
		{
			// match found - return it
			return sopInstanceData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

void SERIES_DATA_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log series data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for valid logger
	if (logger_ptr == NULL) return;

	// save old log level - and set new
	UINT32 oldLogLevel = logger_ptr->logLevel(LOG_IMAGE_RELATION);

	// display series data
	logger_ptr->text(LOG_NONE, 1, "\t\t\t(0020,000E) Series Instance UID: %s", instanceUidM.c_str());

	// sop instance data
	for (UINT i = 0; i < sopInstanceDataM.getSize(); i++) 
	{
		SOP_INSTANCE_DATA_CLASS *sopInstanceData_ptr = sopInstanceDataM[i];

		// dump the sop instance data
		if (sopInstanceData_ptr != NULL)
		{
			logger_ptr->text(LOG_NONE, 1, "\t\t\t\tOBJECT(IMAGE) %d of %d", i + 1, sopInstanceDataM.getSize());
			sopInstanceData_ptr->log(logger_ptr);
		}
	}

	// restore original log level
	logger_ptr->logLevel(oldLogLevel);
}
