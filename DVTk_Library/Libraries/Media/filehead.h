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

#ifndef FILEHEAD_H
#define FILEHEAD_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Idicom.h"			// Dicom component interface
#include "Iwarehouse.h"		// Warehouse component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define FMI_PREAMBLE_LENGTH		128
#define FMI_PREAMBLE_VALUE		0
#define FMI_PREFIX_LENGTH		4
#define FMI_PREFIX_VALUE		"DICM"


//>>***************************************************************************

class FILEHEAD_CLASS : public BASE_WAREHOUSE_ITEM_DATA_CLASS

//  DESCRIPTION     : File Head - includes preamble and prefix.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string			filenameM;
	BYTE			preambleValueM;
	BYTE			preambleM[FMI_PREAMBLE_LENGTH];
	BYTE			prefixM[FMI_PREFIX_LENGTH + 1];
	UID_CLASS		transferSyntaxUidM;
	LOG_CLASS		*loggerM_ptr;

public:
	FILEHEAD_CLASS();

	~FILEHEAD_CLASS();

	void setFilename(string filename)
		{ filenameM = filename; }

	void setPreambleValue(char* preamble_ptr);

	void setPrefix(char*);

	void setTransferSyntaxUid(UID_CLASS &transferSyntaxUid)
		{ transferSyntaxUidM = transferSyntaxUid; }

	const char* getFilename()
		{ return filenameM.c_str(); }

	BYTE getPreambleValue()
		{ return preambleValueM; }

	char* getPrefix()
		{ return (char*) prefixM; }

	UID_CLASS &getTransferSyntaxUid()
		{ return transferSyntaxUidM; }

	bool write(bool autoCreateDirectory);

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);

	void setLogger(LOG_CLASS *logger_ptr)
		{ loggerM_ptr = logger_ptr; }
};

#endif /* FILEHEAD_H */


