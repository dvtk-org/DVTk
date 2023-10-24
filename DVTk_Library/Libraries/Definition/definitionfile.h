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

#ifndef DEFINITIONFILE_H
#define DEFINITIONFILE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Iutility.h"		// Utility component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_DETAILS_CLASS;
class DEF_FILE_CONTENT_CLASS;
class BASE_SESSION_CLASS;
class LOG_CLASS;


//>>***************************************************************************

class DEFINITION_FILE_CLASS : public BASE_FILE_CLASS

//  DESCRIPTION     : Definition File Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEFINITION_FILE_CLASS(const string);
	DEFINITION_FILE_CLASS(BASE_SESSION_CLASS*, const string);

	~DEFINITION_FILE_CLASS();

	bool IsLoaded() { return loadedM; }

	void SetLogger(LOG_CLASS* logger_ptr) { loggerM_ptr = logger_ptr;}

	LOG_CLASS* GetLogger() { return loggerM_ptr; }

	bool Load();

	bool Unload();

	bool GetDetails(DEF_DETAILS_CLASS&);

private:
	DEF_FILE_CONTENT_CLASS	*fileContentM_ptr;
	bool					loadedM;
	BASE_SESSION_CLASS		*sessionM_ptr;
	LOG_CLASS				*loggerM_ptr;

	bool execute();
	bool execute(int& lineNumber);
};

#endif /* DEFINITIONFILE_H */


