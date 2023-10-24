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

#ifndef BASE_ACTIVITY_REPORTER_H
#define BASE_ACTIVITY_REPORTER_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"				// global component interface

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
enum ReportLevel
{
    ReportLevel_None,
    ReportLevel_Error,
    ReportLevel_Debug,
    ReportLevel_Warning,
    ReportLevel_Information,
    ReportLevel_Scripting,
	ReportLevel_ScriptName,
    ReportLevel_MediaFilename,
    ReportLevel_DicomObjectRelationship,
    ReportLevel_DulpStateMachine,
    ReportLevel_WareHouseLabel,
    ReportLevel_ConditionText,
};

//>>***************************************************************************
//<<abstract>>
class BASE_ACTIVITY_REPORTER

//  DESCRIPTION     : BASE_ACTIVITY_REPORTER class.
//  INVARIANT       :
//  NOTES           : Derived classes should implement the defined methods.
//<<***************************************************************************
{
protected:

public:
    virtual void ReportActivity(ReportLevel level, const char* message) = 0;
public:
	virtual ~BASE_ACTIVITY_REPORTER() = 0;		
};

#endif /* BASE_ACTIVITY_REPORTER_H */

