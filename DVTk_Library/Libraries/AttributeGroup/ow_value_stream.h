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

#ifndef OW_VALUE_STREAM_H
#define OW_VALUE_STREAM_H


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "other_value_stream.h"


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
class LOG_MESSAGE_CLASS;
class OB_VALUE_STREAM_CLASS;

//>>***************************************************************************

class OW_VALUE_STREAM_CLASS : public OTHER_VALUE_STREAM_CLASS

//  DESCRIPTION     : OW value stream class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	// private methods

public:
	OW_VALUE_STREAM_CLASS();
	~OW_VALUE_STREAM_CLASS();

	bool SetFilename(string filename);

	ATTR_VR_ENUM GetVR();

	bool IsByteSwapRequired();

	bool StreamPatternTo(DATA_TF_CLASS& data_transfer);

    DVT_STATUS Compare(LOG_MESSAGE_CLASS*, OB_VALUE_STREAM_CLASS&);
    DVT_STATUS Compare(LOG_MESSAGE_CLASS*, OW_VALUE_STREAM_CLASS&);
};


#endif /* OW_VALUE_STREAM_H */
