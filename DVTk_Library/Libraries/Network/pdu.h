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
//  DESCRIPTION     :	RAW PDU class.
//*****************************************************************************
#ifndef PDU_H
#define PDU_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"        // Global component interface
#include "Iutility.h"       // Utility component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_IO_CLASS;


//>>***************************************************************************

class PDU_CLASS : public ENDIAN_CLASS

//  DESCRIPTION     : PDU Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	BYTE*	bodyM_ptr;
	UINT32	maxLengthM;
	UINT32	offsetM;
	UINT32	logLengthM;
    UINT32  maxLengthToReceiveM;


protected:
	BYTE	pduTypeM;
	BYTE	reservedM;
	UINT32	lengthM;

	void setOffset(UINT32 offset)
		{ offsetM = offset; }


	BYTE* getData()
		{ return bodyM_ptr; }

public:
	PDU_CLASS();
	PDU_CLASS(BYTE);

	virtual ~PDU_CLASS();		

	bool write(BASE_IO_CLASS*);

	bool readType(BASE_IO_CLASS*);

	virtual bool readBody(BASE_IO_CLASS*);

	bool writeBinary(const BYTE *, UINT);
		
	INT	readBinary(BYTE *, UINT);

	void setType(BYTE type)
		{ pduTypeM = type; }

	void setReserved(BYTE reserved)
		{ reservedM = reserved; }

	bool allocateBody(UINT32);

	bool setLength(UINT32);

    void setMaxLengthToReceive(UINT32 length);

	BYTE getType()
		{ return pduTypeM; }

	BYTE getReserved()
		{ return reservedM; }

	UINT32 getLength()
		{ return lengthM; }

	void setLogLength(UINT32 length)
		{ logLengthM = length; }

	UINT32 getOffset()
		{ return offsetM; }

	void logRaw(LOG_CLASS*);
};

#endif /* PDU_H */