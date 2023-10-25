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

#ifndef SCRIPT_CONTEXT_H
#define SCRIPT_CONTEXT_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"	// Global component interface
#include "Ilog.h"       // Logging component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class SCRIPT_SESSION_CLASS;


//>>***************************************************************************

class SCRIPT_EXECUTION_CONTEXT_CLASS

//  DESCRIPTION     : Script Execution Context Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	// Script execution context flags
	VALIDATION_CONTROL_FLAG_ENUM validationFlagM;
	bool strictValidationM;
	bool defineSqLengthM;
	bool addGroupLengthM;
	bool autoType2AttributesM;
	bool populateWithAttributesM;
    string applicationEntityNameM;
    string applicationEntityVersionM;

public:
	SCRIPT_EXECUTION_CONTEXT_CLASS(SCRIPT_SESSION_CLASS*);

	~SCRIPT_EXECUTION_CONTEXT_CLASS();
		
	void setValidationFlag(VALIDATION_CONTROL_FLAG_ENUM);

	void setStrictValidation(bool);

	void setDefineSqLength(bool);

	void setAddGroupLength(bool);
	
	void setAutoType2Attributes(bool);

	void setPopulateWithAttributes(bool);

    void setApplicationEntityName(string);

    void setApplicationEntityVersion(string);

	VALIDATION_CONTROL_FLAG_ENUM getValidationFlag();

	bool getStrictValidation();

	bool getDefineSqLength();

	bool getAddGroupLength();

	bool getPopulateWithAttributes();

    string getApplicationEntityName();

    string getApplicationEntityVersion();
};

#endif /* SCRIPT_CONTEXT_H */


