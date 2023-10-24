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

#ifndef FILETAIL_H
#define FILETAIL_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Idicom.h"			// Dicom component interface


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define DSTP_SECTOR_SIZE	512
#define DSTP_PADDING_VALUE	0


//>>***************************************************************************

class FILETAIL_CLASS : public DCM_ATTRIBUTE_GROUP_CLASS

//  DESCRIPTION     : File Tail.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string			filenameM;
	bool			trailingPaddingM;
	UINT			sectorSizeM;
	BYTE			paddingValueM;

public:
	FILETAIL_CLASS();

	~FILETAIL_CLASS();

	void setFilename(string filename)
		{ filenameM = filename; }

	void setTrailingPadding(bool flag)
		{ trailingPaddingM = flag; }

	void setSectorSize(UINT sectorSize)
		{ sectorSizeM = sectorSize; }
	
	void setPaddingValue(BYTE paddingValue)
	{ paddingValueM = paddingValue; }

	const char* getFilename()
		{ return filenameM.c_str(); }

	bool isTrailingPadding()
		{ return trailingPaddingM; }

	UINT getSectorSize()
		{ return sectorSizeM; }

	BYTE getPaddingValue()
		{ return paddingValueM; }

	bool write(bool autoCreateDirectory);

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);
};

#endif /* FILETAIL_H */


