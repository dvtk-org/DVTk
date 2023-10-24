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
#include "study_data.h"
#include "series_data.h"
#include "Ilog.h"			// Log component interface

//>>===========================================================================

STUDY_DATA_CLASS::STUDY_DATA_CLASS(string instanceUid)

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

STUDY_DATA_CLASS::~STUDY_DATA_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any series data
	while (seriesDataM.getSize()) 
	{
		delete seriesDataM[0];
		seriesDataM.removeAt(0);
	}
}

//>>===========================================================================

SERIES_DATA_CLASS *STUDY_DATA_CLASS::search(string instanceUid)

//  DESCRIPTION     : Search Study for Series Data with an instance uid
//					: matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search series data
	for (UINT i = 0; i < seriesDataM.getSize(); i++)
	{
		SERIES_DATA_CLASS *seriesData_ptr = seriesDataM[i];

		// check for match
		if ((seriesData_ptr != NULL) &&
			(instanceUid == seriesData_ptr->getInstanceUid())) 
		{
			// match found - return it
			return seriesData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

void STUDY_DATA_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log study data.
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

	// display study data
	logger_ptr->text(LOG_NONE, 1, "\t\t(0020,000D) Study Instance UID: %s", instanceUidM.c_str());

	// series data
	for (UINT i = 0; i < seriesDataM.getSize(); i++)
	{
		SERIES_DATA_CLASS *seriesData_ptr = seriesDataM[i];

		// dump the series data
		if (seriesData_ptr != NULL)
		{
			logger_ptr->text(LOG_NONE, 1, "\t\t\tSERIES %d of %d", i + 1, seriesDataM.getSize());
			seriesData_ptr->log(logger_ptr);
		}
	}

	// restore original log level
	logger_ptr->logLevel(oldLogLevel);
}
