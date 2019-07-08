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

#ifndef EXT_CHAR_SET_FILE_H
#define EXT_CHAR_SET_FILE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Iutility.h"		// Utility component interface


//>>***************************************************************************

class EXT_CHAR_SET_FILE_CLASS : public BASE_FILE_CLASS

//  DESCRIPTION     : Extended Character Set File Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	EXT_CHAR_SET_FILE_CLASS(string filename);

	~EXT_CHAR_SET_FILE_CLASS();

	bool execute();
};


#endif /* EXT_CHAR_SET_FILE_H */
