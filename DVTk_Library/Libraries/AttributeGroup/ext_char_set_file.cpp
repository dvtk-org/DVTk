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
#include "ext_char_set_file.h"


//*****************************************************************************
//  EXTERNAL REFERENCES
//*****************************************************************************
extern FILE		*extcharsetin;
extern int		extcharsetlineno;
extern void		extcharsetrestart(FILE*);
extern int		extcharsetparse(void);


//>>===========================================================================

EXT_CHAR_SET_FILE_CLASS::EXT_CHAR_SET_FILE_CLASS(string filename)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// initialise class members
	filenameM = filename;
	onlyParseFileM = false;
	fdM_ptr = NULL;
}

//>>===========================================================================

EXT_CHAR_SET_FILE_CLASS::~EXT_CHAR_SET_FILE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// close open character set file
	close();
}

//>>===========================================================================

bool EXT_CHAR_SET_FILE_CLASS::execute()

//  DESCRIPTION     : Execute character set file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// set up input for YACC parser
	if (!open()) return false;

	// set parser to read a character set file
	extcharsetlineno = 1;
	extcharsetin = fdM_ptr;
	extcharsetrestart(extcharsetin);

	// call YACC parser
	int error = extcharsetparse();

	// close the file
	close();

	if (error) return false;

	return true;
}
