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

#ifndef DATA_TF_H
#define DATA_TF_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global Component Interface
#include "endian.h"			// Endian


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//
// define a simple VR mapping table
//
struct T_VR_MAP
{
	ATTR_VR_ENUM	vrEnumM;
	UINT16			vrUint16M;
	char			*vrStringM_ptr;
};


//>>***************************************************************************

class DATA_TF_CLASS : public ENDIAN_CLASS

//  DESCRIPTION     : Base Data Transfer Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	STORAGE_MODE_ENUM	storageModeM;		// Storage Mode for data transferred
	TS_CODE				tsCodeM;			// Transfer syntax code
	string				transferSyntaxM;	// Transfer syntax

public:
	DATA_TF_CLASS();

	virtual ~DATA_TF_CLASS();

	virtual bool initialiseDecode(bool) = 0;

	virtual bool terminateDecode() = 0;

	virtual bool initialiseEncode() = 0;

	virtual bool terminateEncode() = 0;

	virtual bool rewind(UINT) = 0;

	virtual UINT getOffset() = 0;

	virtual bool setOffset(UINT) = 0;

	virtual UINT getRemainingLength() = 0;

	virtual UINT getLength()
		{ return 0; } //dummy function

	virtual bool isData()
		{ return false; }

	bool isLittleEndian()
		{ return tsCodeM & TS_LITTLE_ENDIAN ? true : false; }

	bool isExplicitVR()
		{ return tsCodeM & TS_EXPLICIT_VR ? true : false; }

	bool isCompressed()
		{ return tsCodeM & TS_COMPRESSED ? true : false; }

	virtual void addItemOffset(string, UINT32) { }

	virtual UINT32 getItemOffset(string)
		{ return 0;}

	void setStorageMode(STORAGE_MODE_ENUM storageMode)
		{ storageModeM = storageMode; }

	STORAGE_MODE_ENUM getStorageMode()
		{ return storageModeM; }

	void setTsCode(TS_CODE, string);

	TS_CODE getTsCode()
		{ return tsCodeM; }

	char *getTransferSyntax()
		{ return (char*) transferSyntaxM.c_str(); }

	UINT16 vrToVr16(ATTR_VR_ENUM);

	ATTR_VR_ENUM vr16ToVr(UINT16);
};

char *stringVr(ATTR_VR_ENUM); 

#endif /* DATA_TF_H */


