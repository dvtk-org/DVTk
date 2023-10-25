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

//  Session file class.

#ifndef SESSION_FILE_H
#define SESSION_FILE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Iutility.h"		// Utility component interface

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_SESSION_CLASS;


//>>***************************************************************************

class SESSION_FILE_CLASS : public BASE_FILE_CLASS

//  DESCRIPTION     : Session File Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	BASE_SESSION_CLASS	*sessionM_ptr;

public:
	SESSION_FILE_CLASS(BASE_SESSION_CLASS*, string);

	~SESSION_FILE_CLASS();

	bool execute();

	bool save();
};


#endif /* SESSION_FILE_H */


