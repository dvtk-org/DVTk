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

#ifdef _WINDOWS
#pragma warning (disable : 4786)
#endif

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "sessionfile.h"
#include "session.h"


//*****************************************************************************
//  EXTERNAL REFERENCES
//*****************************************************************************
BASE_SESSION_CLASS		*session_ptr;
LOG_CLASS				*logger_ptr;

extern FILE				*sessionin;
extern int				sessionlineno;
extern void				sessionrestart(FILE*);
extern int				sessionparse(void);


//>>===========================================================================

SESSION_FILE_CLASS::SESSION_FILE_CLASS(BASE_SESSION_CLASS *session_ptr, string filename)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// initialise class members
	::session_ptr = session_ptr;
	sessionM_ptr = session_ptr;
	pathnameM = session_ptr->getSessionDirectory();
	filenameM = filename;
	onlyParseFileM = false;
	fdM_ptr = NULL;
}

//>>===========================================================================

SESSION_FILE_CLASS::~SESSION_FILE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// close open DICOM Script
	close();
}

//>>===========================================================================

bool SESSION_FILE_CLASS::execute()

//  DESCRIPTION     : Execute session file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// set up input for YACC parser
	if (!open()) return false;

	// set parser to read a Session File
	session_ptr = sessionM_ptr;
	sessionlineno = 1;
	sessionin = fdM_ptr;
	sessionrestart(sessionin);

	// call YACC parser
	int error = sessionparse();

	// close the file
	close();

	if (error) return false;

	return true;
}

//>>===========================================================================

bool SESSION_FILE_CLASS::save()

//  DESCRIPTION     : Save session file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// open file for writing
	if (!open(false)) return false;

	// serialise the session parameters
	bool result = sessionM_ptr->serialise(fdM_ptr);
	
	// close the file
	close();

	// all done
	return result;
}


