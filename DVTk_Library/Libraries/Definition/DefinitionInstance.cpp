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
#include "DefFileContent.h"
#include "DefinitionInstance.h"

//>>===========================================================================

DEF_INSTANCE_CLASS::DEF_INSTANCE_CLASS(const string filename, DEF_FILE_CONTENT_CLASS *fileContent_ptr)

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	filenameM = filename;
	fileContentM_ptr = fileContent_ptr;
	referenceIndexM = 1;
}

//>>===========================================================================

DEF_INSTANCE_CLASS::~DEF_INSTANCE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (fileContentM_ptr)
	{
		delete fileContentM_ptr;
	}
}

//>>===========================================================================

string DEF_INSTANCE_CLASS::GetFilename()

//  DESCRIPTION     : Get filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return filenameM;
}

//>>===========================================================================

DEF_FILE_CONTENT_CLASS *DEF_INSTANCE_CLASS::GetFileContent()

//  DESCRIPTION     : Get file content.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return fileContentM_ptr;
}

//>>===========================================================================

int DEF_INSTANCE_CLASS::GetReferenceIndex()

//  DESCRIPTION     : Get Reference Index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return referenceIndexM;
}

//>>===========================================================================

void DEF_INSTANCE_CLASS::IncrementReferenceIndex()

//  DESCRIPTION     : Increment the Reference Index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	referenceIndexM++;
}

//>>===========================================================================

void DEF_INSTANCE_CLASS::DecrementReferenceIndex()

//  DESCRIPTION     : Decrement the Reference Index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	referenceIndexM--;
}
