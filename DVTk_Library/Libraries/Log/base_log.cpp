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
#include "base_log.h"		// Base Log Classes


//>>===========================================================================

LOG_CLASS::~LOG_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// empty virtual destructor
}

//>>===========================================================================

void LOG_CLASS::setLogMask(UINT32 logMask)

//  DESCRIPTION     : Set the Log Mask.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    logMaskM = logMask; 
}

//>>===========================================================================

UINT32 LOG_CLASS::getLogMask()

//  DESCRIPTION     : Get the Log Mask.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    return logMaskM; 
}

//>>===========================================================================

UINT32 LOG_CLASS::logLevel(UINT32 newLogLevel)

//  DESCRIPTION     : Set/Get the log level.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// set log level to given value
	UINT32 oldLogLevel = logLevelM;
	logLevelM = newLogLevel; 
	
	// return old log level
	return oldLogLevel;
}

//>>===========================================================================

void LOG_CLASS::text(BYTE, char* format_ptr, ...)

//  DESCRIPTION     : Log text.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	va_list	arguments;

	// check if global log level enabled
	if (logLevelM & logMaskM) 
	{
		// handle the variable arguments
		va_start(arguments, format_ptr);
		vsprintf(bufferM, format_ptr, arguments);
		va_end(arguments);

		// call the corresponding display method
		displayText();
	}
}

//>>===========================================================================

void LOG_CLASS::text(UINT32 logLevel, BYTE, char* format_ptr, ...)

//  DESCRIPTION     : Log text.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	va_list	arguments;

	// check if given log level enabled
	if (logLevel & logMaskM) 
	{
		// handle the variable arguments
		va_start(arguments, format_ptr);
		vsprintf(bufferM, format_ptr, arguments);
		va_end(arguments);

		// temporarily set the global log level to that given for call to derrived class::displayText()
		UINT32 oldLogLevel = logLevelM;
		logLevelM = logLevel;

		// call the corresponding display method
		displayText();

		// restore old log level
		logLevelM = oldLogLevel;
	}
}

//>>===========================================================================

void LOG_CLASS::setActivityReporter(BASE_ACTIVITY_REPORTER *activityReporter_ptr)

//  DESCRIPTION     : Set the Activity Reporter.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	activityReporterM_ptr = activityReporter_ptr;
}

//>>===========================================================================

BASE_ACTIVITY_REPORTER* LOG_CLASS::getActivityReporter()

//  DESCRIPTION     : Get the Activity Reporter.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return activityReporterM_ptr;
}

//>>===========================================================================

void LOG_CLASS::setSerializer(BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Set the Serializer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	serializerM_ptr = serializer_ptr;
}

//>>===========================================================================

BASE_SERIALIZER* LOG_CLASS::getSerializer()

//  DESCRIPTION     : Get the Serializer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    return serializerM_ptr;
}

//>>===========================================================================

void LOG_CLASS::setStorageRoot(string storageRoot)

//  DESCRIPTION     : Get the Storage Root directory.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    storageRootM = storageRoot; 
}

//>>===========================================================================

const char *LOG_CLASS::getStorageRoot()

//  DESCRIPTION     : Get the Storage Root directory.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    return storageRootM.c_str(); 
}

//>>===========================================================================

void LOG_CLASS::setResultsRoot(string resultsRoot)

//  DESCRIPTION     : Set the Results Root directory.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    //
    // Add backslash at the end of the resultsRoot directory path
    //
    //if (resultsRoot[resultsRoot.length()-1] != '\\')
	if (resultsRoot.length()==0 || resultsRoot[resultsRoot.length()-1] != '\\') //MIGRATION_IN_PROGRESS 
    {
        resultsRoot += "\\";
    }
    resultsRootM = resultsRoot; 
}

//>>===========================================================================

const char *LOG_CLASS::getResultsRoot()

//  DESCRIPTION     : Get the Results Root directory.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
    return resultsRootM.c_str(); 
}
