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

#ifndef CCTYPES_H
#define CCTYPES_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ucdavis.h"


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#ifdef	SOLARIS
#define	SYSTEM_V
#endif

typedef unsigned int     UINT;
typedef unsigned short   UINT16;
typedef unsigned char    UINT8;

#ifndef	_WINDOWS
typedef unsigned char    BYTE;
#endif

typedef signed char     INT8;
typedef signed short    INT16;

#ifndef	_WINDOWS
typedef unsigned long   UINT32;
typedef signed long     INT32;
typedef signed int      INT;
#endif

#ifdef _WINDOWS
typedef DWORD			THREAD_TYPE;
#else
typedef pthread_t		THREAD_TYPE;
#endif

#ifdef	LITTLE_ENDIAN
#undef	LITTLE_ENDIAN
#endif

#ifdef	BIG_ENDIAN
#undef	BIG_ENDIAN
#endif

#define LITTLE_ENDIAN   1
#define BIG_ENDIAN      2

#ifndef	NATIVE_ENDIAN
#define	NATIVE_ENDIAN	LITTLE_ENDIAN
#endif

#endif /* CCTYPES_H */


