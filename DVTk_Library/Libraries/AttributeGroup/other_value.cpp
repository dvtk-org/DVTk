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
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "other_value.h"
#include "attribute_group.h"
#include "attribute.h"
#include "ob_value_stream.h"
#include "of_value_stream.h"
#include "ow_value_stream.h"


//>>===========================================================================
OTHER_VALUE_CLASS::OTHER_VALUE_CLASS()
//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    lengthOkM = true;
    parentGroupM_ptr = NULL;
    loggerM_ptr = NULL;
    filenameM = "";
	lengthM = 0;
	bitsAllocatedM = 8;
    samplesPerPixelM = 1;
    planarConfigurationM = FRAME_INTERLEAVE;
	indexM = 0; 
    rowsM = UNDEFINED_LENGTH;
    columnsM = UNDEFINED_LENGTH;
    start_valueM = 0;
    rows_incrementM = 1;
    columns_incrementM = 1;
    rows_sameM = 1;
    columns_sameM = 1;
	isCompressedM = false;
	decodedLengthUndefinedM = true; // Initialize to true for backwards compatibility reasons.
}

//>>===========================================================================
OTHER_VALUE_CLASS::~OTHER_VALUE_CLASS()
//  DESCRIPTION     : Default destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================
DVT_STATUS OTHER_VALUE_CLASS::Set (ATTRIBUTE_GROUP_CLASS * parent)
//  DESCRIPTION     : Set the parent attribute group. This is needed for
//                    checking the validity of the OB/OF/OW values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    parentGroupM_ptr = parent;
    return (MSG_OK);
}

//>>===========================================================================
DVT_STATUS OTHER_VALUE_CLASS::Set (UINT32 value)
//  DESCRIPTION     : Set the first data property.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : If the filename has already been set, MSG_ERROR will be
//                    returned.
//  NOTES           :
//<<===========================================================================
{
    if (filenameM.length() != 0)
    {
        return (MSG_ERROR);
    }
    rowsM = value;
	
	// set the columns to the same value as rows so that a square pattern will be generated
    columnsM = value;

	// set the index to indicate that the next property to update via the add method is the
	// columns. NOTE: call to add property is optional
	indexM = 1;

    return (MSG_OK);
}

//>>===========================================================================
DVT_STATUS OTHER_VALUE_CLASS::Set (string filename)
//  DESCRIPTION     : Set the filename where the data is stored.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : If the pixel data properties are already set, MSG_ERROR
//                    will be returned.
//  NOTES           :
//<<===========================================================================
{
    if ((rowsM != UNDEFINED_LENGTH) && (columnsM != UNDEFINED_LENGTH))
    {
        return (MSG_ERROR);
    }
    filenameM = filename;

    return (MSG_OK);
}

//>>===========================================================================
void OTHER_VALUE_CLASS::SetBitsAllocated(UINT16 bitsAllocated)
//  DESCRIPTION     : Set the Bits Allocated for the underlying stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bitsAllocatedM = bitsAllocated;
}

//>>===========================================================================
void OTHER_VALUE_CLASS::SetSamplesPerPixel(UINT16 samplesPerPixel)
//  DESCRIPTION     : Set the Samples Per Pixel for the underlying stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    samplesPerPixelM = samplesPerPixel;
}

//>>===========================================================================
void OTHER_VALUE_CLASS::SetPlanarConfiguration(UINT16 planarConfiguration)
//  DESCRIPTION     :  Set the Planar Configuration for the underlying stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    planarConfigurationM = planarConfiguration;
}

//>>===========================================================================
DVT_STATUS OTHER_VALUE_CLASS::Add (unsigned int value)
//  DESCRIPTION     : Set the next (indexed) data property.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : If the filename has already been set, MSG_ERROR will be
//                    returned.
//  NOTES           :
//<<===========================================================================
{
	DVT_STATUS result = MSG_OK;

    if (filenameM.length() != 0)
    {
        return (MSG_ERROR);
    }

	// set indexed property
	switch(indexM)
	{
	case 0: rowsM = value; break;
	case 1: columnsM = value; break;
	case 2: start_valueM = value; break;
	case 3: rows_incrementM = value; break;
	case 4: columns_incrementM = value; break;
	case 5: rows_sameM = value; break;
	case 6: columns_sameM = value; break;
	default: result = MSG_ERROR; break;
	}

	// increment the index for the next call
	indexM++;

	// return result
	return (result);
}

//>>===========================================================================
DVT_STATUS
OTHER_VALUE_CLASS::Get (ATTRIBUTE_GROUP_CLASS **parent, int)
//  DESCRIPTION     : Returns the parent.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    *parent = parentGroupM_ptr;

    return (MSG_OK);
}

//>>===========================================================================
DVT_STATUS OTHER_VALUE_CLASS::Get (UINT32 index, UINT32 &value)
//  DESCRIPTION     : Returns the indexed pixel data property.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : If the filename has been set, MSG_ERROR is returned. If
//                    the pixel data properties have not been set, MSG_NOT_SET
//                    is returned.
//  NOTES           :
//<<===========================================================================
{
	DVT_STATUS result = MSG_OK;

    if (filenameM.length() > 0)
    {
        return (MSG_ERROR);
    }
    if ((rowsM == UNDEFINED_LENGTH) && (columnsM == UNDEFINED_LENGTH))
    {
        return (MSG_NOT_SET);
    }

	// get indexed property
	switch(index)
	{
	case 0: value = rowsM; break;
	case 1: value = columnsM; break;
	case 2: value = start_valueM; break;
	case 3: value = rows_incrementM; break;
	case 4: value = columns_incrementM; break;
	case 5: value = rows_sameM; break;
	case 6: value = columns_sameM; break;
	default: result = MSG_ERROR; break;
	}

	// return result
	return (result);
}

//>>===========================================================================
DVT_STATUS OTHER_VALUE_CLASS::Get(UINT32 &value)
//  DESCRIPTION     : Get the first value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DVT_STATUS result = MSG_OK;

    if (filenameM.length() > 0)
    {
        return (MSG_ERROR);
    }
    if (rowsM == UNDEFINED_LENGTH)
    {
        return (MSG_NOT_SET);
    }

	// get first value
	value = rowsM;

	// return result
	return (result);
}

//>>===========================================================================
DVT_STATUS OTHER_VALUE_CLASS::Get (string &filename, bool)
//  DESCRIPTION     : return the filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : If the pixel data properties are set, MSG_ERROR is
//                    returned. If the filename is not set, MSG_NOT_SET is
//                    returned.
//  NOTES           :
//<<===========================================================================
{
    if ((rowsM != UNDEFINED_LENGTH) && (columnsM != UNDEFINED_LENGTH))
    {
        return (MSG_ERROR);
    }
    if (filenameM.length() == 0)
    {
        return (MSG_NOT_SET);
    }
    filename = filenameM;

    return (MSG_OK);
}

//>>===========================================================================
UINT16 OTHER_VALUE_CLASS::GetBitsAllocated()
//  DESCRIPTION     : Get the Bits Allocated for the underlying stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return bitsAllocatedM;
}

//>>===========================================================================
UINT16 OTHER_VALUE_CLASS::GetSamplesPerPixel()
//  DESCRIPTION     : Get the Samples Per Pixel for the underlying stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    return samplesPerPixelM;
}

//>>===========================================================================
UINT16 OTHER_VALUE_CLASS::GetPlanarConfiguration()
//  DESCRIPTION     :  Get the Planar Configuration for the underlying stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return planarConfigurationM;
}

//>>===========================================================================
bool OTHER_VALUE_CLASS::IsDataStored()
//  DESCRIPTION     :  Return boolean indicating whether or not the pixel data
//					: is stored.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool dataStored = true;

	if (filenameM.find(DATA_NOT_STORED) != string::npos)
	{
		dataStored = false;
	}

	return dataStored;
}

//>>===========================================================================
void OTHER_VALUE_CLASS::SetLogger(LOG_CLASS *logger_ptr)
//  DESCRIPTION     :  Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    loggerM_ptr = logger_ptr;
}

//>>===========================================================================
LOG_CLASS *OTHER_VALUE_CLASS::GetLogger()
//  DESCRIPTION     :  Get the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return loggerM_ptr;
}

//>>===========================================================================

void OTHER_VALUE_CLASS::SetCompressed(bool flag)

//  DESCRIPTION     : Set the compressed flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	isCompressedM = flag;
}

//>>===========================================================================

bool OTHER_VALUE_CLASS::IsCompressed()

//  DESCRIPTION     : Return the compressed flag value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return isCompressedM;
}

//>>===========================================================================

void OTHER_VALUE_CLASS::SetDecodedLengthUndefined(UINT32 length)

//  DESCRIPTION     : Set Decoded Length Undefined value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the decodedLengthUndefined bool according to the length set
	// - we need to know this for encoding later
	if (length == UNDEFINED_LENGTH)
	{
		decodedLengthUndefinedM = true;
	}
	else
	{
		decodedLengthUndefinedM = false;
	}
}

//>>===========================================================================

bool OTHER_VALUE_CLASS::GetDecodedLengthUndefined()

//  DESCRIPTION     : Return Decoded Length Undefined value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return decodedLengthUndefinedM;
}
