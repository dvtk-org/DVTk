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
#include "basefile.h"
#include "utility.h"


//>>===========================================================================

BASE_FILE_CLASS::~BASE_FILE_CLASS()

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

bool BASE_FILE_CLASS::open(bool forReading)

//  DESCRIPTION     : Open base file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	string filename;

    if (filenameM.length() == 0) return false; // No filename supplied.

    // generate absolute filename
    if (getAbsoluteFilename() == NULL) return false; // No pathname supplied.

	// open file
	if (forReading)
	{
		// open file for reading
		fdM_ptr = fopen(absoluteFilenameM.c_str(), "r");
	}
	else
	{
		// open file for writing
		fdM_ptr = fopen(absoluteFilenameM.c_str(), "w");
	}

	// return success or otherwise of the file open
	return (fdM_ptr == NULL) ? false : true;
}

//>>===========================================================================

bool BASE_FILE_CLASS::save()

//  DESCRIPTION     : Save base file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	return false;
}

//>>===========================================================================

void BASE_FILE_CLASS::close()

//  DESCRIPTION     : Close file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// check if file is open
	if (fdM_ptr != NULL)
	{
		// close file
		fclose(fdM_ptr);
	}

	fdM_ptr = NULL;
}

//>>===========================================================================

bool BASE_FILE_CLASS::isFullPathname()

//  DESCRIPTION     : Determine if the filename is the full pathname
//					: relative or absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	return isAbsolutePath(filenameM);
}

//>>===========================================================================

const char *BASE_FILE_CLASS::getAbsoluteFilename()

//  DESCRIPTION     : Get the absolute pathname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Might be necessary to generate this from the component parts.
//<<===========================================================================
{
	// check if we need to append the pathname to the filename
	if (isFullPathname())
	{
		// filename is absolute
		absoluteFilenameM = filenameM;
	}
    else
	{
        if (pathnameM.length() == 0) return NULL; // No pathname supplied.
        absoluteFilenameM = pathnameM;
        if (pathnameM[pathnameM.length()-1] != '\\') absoluteFilenameM += "\\";
        absoluteFilenameM += filenameM;
	}

    return absoluteFilenameM.c_str();
}
