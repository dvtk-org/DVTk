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

#ifndef BASE_H
#define BASE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
extern "C"
{
#include <errno.h>
#include <stdio.h>
#include <stdarg.h>
#include <stdlib.h>
#include <ctype.h>
#include <string.h>
#include <malloc.h>
#include <memory.h>
};

#ifdef	_WINDOWS
#include <windows.h>
//#include <winsock2.h>
#include <share.h>
#pragma warning (disable : 4786)
#else
#include "unixsock.h"
#endif

#include <iostream>
#include <string>
#include <vector>
#include <map>
#include <stack>

using namespace std;

#endif /* BASE_H */

