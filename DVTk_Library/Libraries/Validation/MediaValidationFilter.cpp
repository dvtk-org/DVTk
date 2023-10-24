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
//  DESCRIPTION     :   Media Validation Filter class.
//                      Used to determine which DICOMDIR records can be filtered
//						out. The class is instantiated with a directory record type
//						and a maximum number of records to validate at that level.
//						Example: "IMAGE" 2 - validate a maximum of 2 IMAGE records
//						for each "SERIES" record present in the DICOMDIR.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Idefinition.h"
#include "Idicom.h"
#include "Imedia.h"
#include "MediaValidationFilter.h"

//*****************************************************************************
//  INTERNAL DECLARATIONS
//*****************************************************************************

//>>===========================================================================

MEDIA_VALIDATION_FILTER_CLASS::MEDIA_VALIDATION_FILTER_CLASS(string directoryRecordType, int maximumRecordsToValidate)

//  DESCRIPTION     : Class constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	filterDirectoryRecordTypeM = directoryRecordType;
	maximumRecordsToValidateM = maximumRecordsToValidate;
	recordsValidatedM = 0;
}

//>>===========================================================================

MEDIA_VALIDATION_FILTER_CLASS::~MEDIA_VALIDATION_FILTER_CLASS()

//  DESCRIPTION     : Class destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

bool MEDIA_VALIDATION_FILTER_CLASS::ShouldFilterOut(string directoryRecordType)

//  DESCRIPTION     : Check if the given record should be filtered out or not.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool filterOut = true;

	// check if this record type matches the filter record types
	if (strstr(filterDirectoryRecordTypeM.c_str(), directoryRecordType.c_str()) != NULL)
	{
		// check if the maximum number to process has been exceeded yet
		if (recordsValidatedM < maximumRecordsToValidateM)
		{
			recordsValidatedM++;
			filterOut = false;
		}
	}
	else
	{
		// reset the records of the directoryRecordTypeM already validated when a different record is given.
		recordsValidatedM = 0;
		filterOut = false;
	}

	return filterOut;
}
