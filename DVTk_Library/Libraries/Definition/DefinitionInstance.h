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

#ifndef DEF_INSTANCE_H
#define DEF_INSTANCE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_FILE_CONTENT_CLASS;

//>>***************************************************************************

class DEF_INSTANCE_CLASS

//  DESCRIPTION     : Definition instance class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_INSTANCE_CLASS(const string, DEF_FILE_CONTENT_CLASS*);

	~DEF_INSTANCE_CLASS();

	string GetFilename();

	DEF_FILE_CONTENT_CLASS *GetFileContent();

	int GetReferenceIndex();

	void IncrementReferenceIndex();

	void DecrementReferenceIndex();

private:
	DEF_FILE_CONTENT_CLASS	*fileContentM_ptr;
	string					filenameM;
	int						referenceIndexM;
};		

#endif /* DEF_INSTANCE_H */
