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


//  Handle the big/little endian translations.

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "endian.h"


//>>===========================================================================

ENDIAN_CLASS::~ENDIAN_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	// nothing to do explicitly
}


//>>===========================================================================

bool ENDIAN_CLASS::operator >> (BYTE& x)

//  DESCRIPTION     : Read BYTE operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	BYTE	s[2];

	// read required bytes
	if (readBinary(s, 1) != 1) return false;

	x = s[0];

	return true;
}


//>>===========================================================================

bool ENDIAN_CLASS::operator >> (UINT16& x)

//  DESCRIPTION     : Read UINT16 operator according to defined endian
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[US_LENGTH];
	INT offset = 0;

	// read required bytes
	INT length = US_LENGTH;
	while (length)
	{
		INT lengthRead = readBinary(&s[offset], length);
		if (lengthRead <= 0) return false;
		offset += lengthRead;
		length-= lengthRead;
	}

	// check required endianess
	if (endianM == NATIVE_ENDIAN)
	{
		// stream in native endian
		for (i = 0; i < US_LENGTH; i++)
			op[i] = s[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = US_LENGTH; i < US_LENGTH; i++)
			op[i] = s[--j];
	}

	return true;
}


//>>===========================================================================

bool ENDIAN_CLASS::operator >> (UINT32& x)

//  DESCRIPTION     : Read UINT32 operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[UL_LENGTH];
	INT offset = 0;

	// read required bytes
	INT length = UL_LENGTH;
	while (length)
	{
		INT lengthRead = readBinary(&s[offset], length);
		if (lengthRead <= 0) return false;
		offset += lengthRead;
		length-= lengthRead;
	}

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < UL_LENGTH; i++)
			op[i] = s[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = UL_LENGTH; i < UL_LENGTH; i++)
			op[i] = s[--j];
	}

	return true;
}

//>>===========================================================================

bool ENDIAN_CLASS::operator >> (UINT64& x)

//  DESCRIPTION     : Read UINT64 operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[UV_LENGTH];
	INT offset = 0;

	// read required bytes
	INT length = UV_LENGTH;
	while (length)
	{
		INT lengthRead = readBinary(&s[offset], length);
		if (lengthRead <= 0) return false;
		offset += lengthRead;
		length-= lengthRead;
	}

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < UV_LENGTH; i++)
			op[i] = s[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = UV_LENGTH; i < UV_LENGTH; i++)
			op[i] = s[--j];
	}

	return true;
}


//>>===========================================================================

bool ENDIAN_CLASS::operator >> (float& x)

//  DESCRIPTION     : Read float operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[FL_LENGTH];
	INT offset = 0;

	// read required bytes
	INT length = FL_LENGTH;
	while (length)
	{
		INT lengthRead = readBinary(&s[offset], length);
		if (lengthRead <= 0) return false;
		offset += lengthRead;
		length-= lengthRead;
	}

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < FL_LENGTH; i++)
			op[i] = s[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = FL_LENGTH; i < FL_LENGTH; i++)
			op[i] = s[--j];
	}

	return true;
}


//>>===========================================================================

bool ENDIAN_CLASS::operator >> (double& x)

//  DESCRIPTION     : Read double operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[FD_LENGTH];
	INT offset = 0;

	// read required bytes
	INT length = FD_LENGTH;
	while (length)
	{
		INT lengthRead = readBinary(&s[offset], length);
		if (lengthRead <= 0) return false;
		offset += lengthRead;
		length-= lengthRead;
	}

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < FD_LENGTH; i++)
			op[i] = s[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = FD_LENGTH; i < FD_LENGTH; i++)
			op[i] = s[--j];
	}

	return true;
}


//>>===========================================================================

bool ENDIAN_CLASS::operator << (BYTE& x)

//  DESCRIPTION     : Write BYTE operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	BYTE	s[2];

	s[0] = x;

	// write required bytes
	return writeBinary(s, 1);
}


//>>===========================================================================

bool ENDIAN_CLASS::operator << (UINT16& x)

//  DESCRIPTION     : Write UINT16 operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[US_LENGTH];

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < US_LENGTH; i++)
			s[i] = op[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = US_LENGTH; i < US_LENGTH; i++)
			s[--j] = op[i];
	}

	// write required bytes
	return writeBinary(s, US_LENGTH);
}


//>>===========================================================================

bool ENDIAN_CLASS::operator << (UINT32& x)

//  DESCRIPTION     : Write UINT32 operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[UL_LENGTH];

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < UL_LENGTH; i++)
			s[i] = op[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = UL_LENGTH; i < UL_LENGTH; i++)
			s[--j] = op[i];
	}

	// write required bytes
	return writeBinary(s, UL_LENGTH);
}

//>>===========================================================================

bool ENDIAN_CLASS::operator << (UINT64& x)

//  DESCRIPTION     : Write UINT64 operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[UV_LENGTH];

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < UV_LENGTH; i++)
			s[i] = op[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = UV_LENGTH; i < UV_LENGTH; i++)
			s[--j] = op[i];
	}

	// write required bytes
	return writeBinary(s, UV_LENGTH);
}


//>>===========================================================================

bool ENDIAN_CLASS::operator << (float& x)

//  DESCRIPTION     : Write float operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[FL_LENGTH];

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < FL_LENGTH; i++)
			s[i] = op[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = FL_LENGTH; i < FL_LENGTH; i++)
			s[--j] = op[i];
	}

	// write required bytes
	return writeBinary(s, FL_LENGTH);
}


//>>===========================================================================

bool ENDIAN_CLASS::operator << (double& x)

//  DESCRIPTION     : Write double operator according to defined endian.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Abstract Base class.
//<<===========================================================================
{
	int		i, j;
	BYTE	*op = (BYTE *) &x;
	BYTE	s[FD_LENGTH];

	// check required endianess
	if (endianM == NATIVE_ENDIAN) 
	{
		// stream in native endian
		for (i = 0; i < FD_LENGTH; i++)
			s[i] = op[i];
	}
	else 
	{
		// stream in reverse endian
		for (i = 0, j = FD_LENGTH; i < FD_LENGTH; i++)
			s[--j] = op[i];
	}

	// write required bytes
	return writeBinary(s, FD_LENGTH);
}
