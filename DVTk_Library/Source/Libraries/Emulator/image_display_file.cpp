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
#include "image_display_file.h"


//*****************************************************************************
//  EXTERNAL REFERENCES
//*****************************************************************************
extern FILE		*imagedisplayin;
extern int		imagedisplaylineno;
extern void		imagedisplayrestart(FILE*);
extern int		imagedisplayparse(void);


//>>===========================================================================

IMAGE_DISPLAY_FILE_CLASS::IMAGE_DISPLAY_FILE_CLASS(string filename)

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

IMAGE_DISPLAY_FILE_CLASS::~IMAGE_DISPLAY_FILE_CLASS()

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

bool IMAGE_DISPLAY_FILE_CLASS::execute()

//  DESCRIPTION     : Execute image display file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// set up input for YACC parser
	if (!open()) return false;

	// set parser to read a character set file
	imagedisplaylineno = 1;
	imagedisplayin = fdM_ptr;
	imagedisplayrestart(imagedisplayin);

	// call YACC parser
	int error = imagedisplayparse();

	// close the file
	close();

	if (error) return false;

	return true;
}
