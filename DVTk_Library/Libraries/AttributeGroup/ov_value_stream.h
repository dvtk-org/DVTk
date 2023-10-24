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

//  DICOM Other Float Attribute Value Stream Class.
#ifndef OV_VALUE_STREAM_H
#define OV_VALUE_STREAM_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "other_value_stream.h"

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
class LOG_MESSAGE_CLASS;


//>>***************************************************************************

class OV_VALUE_STREAM_CLASS : public OTHER_VALUE_STREAM_CLASS

//  DESCRIPTION     : OD value stream class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	// private methods

public:
	OV_VALUE_STREAM_CLASS();
	~OV_VALUE_STREAM_CLASS();

	bool SetFilename(string filename);

	ATTR_VR_ENUM GetVR();

	bool IsByteSwapRequired();

	void SwapBytes(BYTE* buffer_ptr, UINT32 length);

	bool StreamPatternTo(DATA_TF_CLASS& data_transfer);

    DVT_STATUS Compare(LOG_MESSAGE_CLASS*, OV_VALUE_STREAM_CLASS&);
};


#endif /* OV_VALUE_STREAM_H */
