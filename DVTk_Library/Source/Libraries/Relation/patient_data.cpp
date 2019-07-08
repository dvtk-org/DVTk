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
#include "patient_data.h"
#include "study_data.h"
#include "Ilog.h"


//>>===========================================================================

PATIENT_DATA_CLASS::PATIENT_DATA_CLASS(string id, string name)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	patientIdM = id;
	patientNameM = name;
}
	
//>>===========================================================================

PATIENT_DATA_CLASS::~PATIENT_DATA_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up any study data
	while (studyDataM.getSize())
	{
		delete studyDataM[0];
		studyDataM.removeAt(0);
	}
}

//>>===========================================================================

STUDY_DATA_CLASS *PATIENT_DATA_CLASS::search(string instanceUid)

//  DESCRIPTION     : Search the Patient for Study Data with an instance
//					: uid matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// search study data
	for (UINT i = 0; i < studyDataM.getSize(); i++)
	{
		STUDY_DATA_CLASS *studyData_ptr = studyDataM[i];

		// check for match
		if ((studyData_ptr != NULL) && 
			(instanceUid == studyData_ptr->getInstanceUid()))
		{
			// match found - return it
			return studyData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

void PATIENT_DATA_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Constructor.
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

	// display patient data
	logger_ptr->text(LOG_NONE, 1, "\t(0010,0020) Patient ID: %s", patientIdM.c_str());

	if (patientNameM.length())
	{
		logger_ptr->text(LOG_NONE, 1, "\t(0010,0010) Patient Name: %s", patientNameM.c_str());
	}
	else
	{
		logger_ptr->text(LOG_NONE, 1, "\t(0010,0010) Patient Name: Not Present");
	}

	// study data
	for (UINT i = 0; i < studyDataM.getSize(); i++) 
	{
		STUDY_DATA_CLASS *studyData_ptr = studyDataM[i];

		// dump the study data
		if (studyData_ptr != NULL)
		{
			logger_ptr->text(LOG_NONE, 1, "\t\tSTUDY %d of %d", i + 1, studyDataM.getSize());
			studyData_ptr->log(logger_ptr);
		}
	}

	// restore original log level
	logger_ptr->logLevel(oldLogLevel);
}
