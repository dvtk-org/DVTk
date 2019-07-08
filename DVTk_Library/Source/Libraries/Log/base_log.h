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

#ifndef BASE_LOG_HPP
#define BASE_LOG_HPP

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"				// global component interface
#include "base_activity_reporter.h"	// base activity reporter
#include "base_serializer.h"        // base serializer


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
//
// maximum debug line length
//
#define MAXIMUM_LINE_LENGTH		2048

//
// maximuim number of attriute values to display when logging
//
#define MAXIMUM_LOGGED_VALUES	32

//
// maximum number of bytes values to display when logging attribute values
//
#define MAXIMUM_LOGGED_BYTES	128

//
// Log levels - add a new bit mask when a new log level should be defined
//
const UINT32 LOG_NONE							= 0x00000001;
const UINT32 LOG_ERROR							= 0x00000002;
const UINT32 LOG_DEBUG							= 0x00000004;
const UINT32 LOG_WARNING						= 0x00000008;
const UINT32 LOG_INFO							= 0x00000010;
const UINT32 LOG_SCRIPT							= 0x00000020;
const UINT32 LOG_SCRIPT_NAME					= 0x00000040;

const UINT32 LOG_PDU_BYTES					    = 0x00000080;
const UINT32 LOG_DULP_FSM						= 0x00000100;

const UINT32 LOG_IMAGE_RELATION					= 0x00000200;
const UINT32 LOG_PRINT							= 0x00000400;
const UINT32 LOG_LABEL							= 0x00000800;
const UINT32 LOG_MEDIA_FILENAME                 = 0x00001000;
const UINT32 LOG_CONDITION_TEXT                 = 0x00002000;


//>>***************************************************************************

class LOG_CLASS

//  DESCRIPTION     : Base logging class.
//  INVARIANT       :
//  NOTES           : Derived classes should implement the display text method
//					  according to the sort of display required.
//<<***************************************************************************
{
protected:
	BASE_ACTIVITY_REPORTER	*activityReporterM_ptr;
    BASE_SERIALIZER         *serializerM_ptr;
	UINT32					logMaskM;
	UINT32					logLevelM;
	char                    bufferM[MAXIMUM_LINE_LENGTH];
	string					storageRootM;
	string					resultsRootM;

public:
	virtual ~LOG_CLASS() = 0;		

	void setLogMask(UINT32);

	UINT32 getLogMask();

	UINT32 logLevel(UINT32);

	void text(BYTE, char*, ...);
	void text(UINT32, BYTE, char*, ...);

	void setActivityReporter(BASE_ACTIVITY_REPORTER*);

	BASE_ACTIVITY_REPORTER* getActivityReporter();

    void setSerializer(BASE_SERIALIZER*);

    BASE_SERIALIZER* getSerializer();

	void setStorageRoot(string);

	const char *getStorageRoot();

	void setResultsRoot(string);

	const char *getResultsRoot();

	virtual void displayText() = 0;
};


#endif /* BASE_LOG_H */

