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

#ifndef ENDIAN_H
#define ENDIAN_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// C Header Files / Base Data Templates 


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************


//>>***************************************************************************

class ENDIAN_CLASS

//  DESCRIPTION     : Class used to handle the big/little endian translations.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT	endianM;

public:

	virtual	~ENDIAN_CLASS() = 0;		

	void setEndian(UINT endian) { endianM = endian; }
		
	inline UINT	getEndian() { return endianM; }
	
	bool operator >> (BYTE&);
	bool operator >> (UINT16&);
	bool operator >> (UINT32&);
	bool operator >> (UINT64&);
	bool operator >> (float&);
	bool operator >> (double&);

	inline bool operator >> (char& x) { return ((*this) >> (BYTE&) x); }
	inline bool operator >> (INT16& x) { return ((*this) >> (UINT16&) x); }
	inline bool operator >> (INT32& x) { return ((*this) >> (UINT32&) x); }
	inline bool operator >> (INT64& x) { return ((*this) >> (UINT64&) x); }

	bool operator << (BYTE&);
	bool operator << (UINT16&);
	bool operator << (UINT32&);
	bool operator << (UINT64&);
	bool operator << (float&);
	bool operator << (double&);

	inline bool operator << (char& x) { return ((*this) << (BYTE&) x); }
	inline bool operator << (INT16& x) { return ((*this) << (UINT16&) x); }
	inline bool operator << (INT32& x) { return ((*this) << (UINT32&) x); }
	inline bool operator << (INT64& x) { return ((*this) << (UINT64&) x); }

	virtual INT	readBinary(BYTE*, UINT) = 0;

	virtual bool writeBinary(const BYTE*, UINT) = 0;
};


#endif /* ENDIAN_H */


