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
#include "ov_value_stream.h"
#include "../validation/valrules.h"


//>>===========================================================================

OV_VALUE_STREAM_CLASS::OV_VALUE_STREAM_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	Initialise();
	bits_allocatedM = 32;
    file_vrM = GetVR();
}

//>>===========================================================================

OV_VALUE_STREAM_CLASS::~OV_VALUE_STREAM_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

bool OV_VALUE_STREAM_CLASS::SetFilename(string filename)

//  DESCRIPTION     : Set filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the file contents description - from the filename - ADVT requirement
	if (strstr(filename.c_str(), "VL64B")) 
	{
		// OV, 64 bit data, big endian
		fs_codeM = TS_BIG_ENDIAN;
	}
	else if (strstr(filename.c_str(), "VL64L")) 
	{
		// OV, 64 bit data, little endian
		fs_codeM = TS_LITTLE_ENDIAN;
	}
	else {
		// default to OV, 32 bit data, endian unimportant
#ifdef _WINDOWS
		fs_codeM = TS_LITTLE_ENDIAN;
#else
		fs_codeM = TS_BIG_ENDIAN;
#endif
	}

	// save the filename
	return OTHER_VALUE_STREAM_CLASS::SetFilename(filename);
}

//>>===========================================================================

ATTR_VR_ENUM OV_VALUE_STREAM_CLASS::GetVR()

//  DESCRIPTION     : Return class VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return class VR
	return ATTR_VR_OV;
}

//>>===========================================================================

bool OV_VALUE_STREAM_CLASS::IsByteSwapRequired()

//  DESCRIPTION     : Check if byte swap is required in Other Very Long Data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool byte_swap = false;

	// check criteria for byte swap
	if ((fs_codeM & TS_LITTLE_ENDIAN) && 
		(ms_codeM & TS_BIG_ENDIAN)) 
	{
		// byte swap
		byte_swap = true;
	}
	else if ((fs_codeM & TS_BIG_ENDIAN) && 
		(ms_codeM & TS_LITTLE_ENDIAN)) 
	{
		// byte swap
		byte_swap = true;
	}

	// log debug result to user
	if (loggerM_ptr)
	{
	    if (byte_swap) 
	    {
            loggerM_ptr->text(LOG_INFO, 1, "Byte Swap of original OV data needed - details:");
        }
        else
        {
            loggerM_ptr->text(LOG_INFO, 1, "No Byte Swap of original OV data needed - details:");
        }
        switch(fs_codeM)
        {
        case TS_LITTLE_ENDIAN: loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax: Little Endian"); break;
        case TS_BIG_ENDIAN: loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax: Big Endian"); break;
        case TS_LITTLE_ENDIAN | TS_IMPLICIT_VR: loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax: Implicit VR Little Endian"); break;
        case TS_LITTLE_ENDIAN | TS_EXPLICIT_VR: loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax: Explicit VR Little Endian"); break;
        case TS_LITTLE_ENDIAN | TS_EXPLICIT_VR | TS_COMPRESSED: loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax: Explicit VR Little Endian - Compressed"); break;
        case TS_BIG_ENDIAN | TS_EXPLICIT_VR: loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax: Explicit VR Big Endian"); break;
        default: loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax: %X = Unknown", fs_codeM); break;
        }
        switch(ms_codeM)
        {
        case TS_LITTLE_ENDIAN: loggerM_ptr->text(LOG_DEBUG, 1, "Memory Transfer Syntax: Little Endian"); break;
        case TS_BIG_ENDIAN: loggerM_ptr->text(LOG_DEBUG, 1, "Memory Transfer Syntax: Big Endian"); break;
        case TS_LITTLE_ENDIAN | TS_IMPLICIT_VR: loggerM_ptr->text(LOG_DEBUG, 1, "Memory Transfer Syntax: Implicit VR Little Endian"); break;
        case TS_LITTLE_ENDIAN | TS_EXPLICIT_VR: loggerM_ptr->text(LOG_DEBUG, 1, "Memory Transfer Syntax: Explicit VR Little Endian"); break;
        case TS_LITTLE_ENDIAN | TS_EXPLICIT_VR | TS_COMPRESSED: loggerM_ptr->text(LOG_DEBUG, 1, "Memory Transfer Syntax: Explicit VR Little Endian - Compressed"); break;
        case TS_BIG_ENDIAN | TS_EXPLICIT_VR: loggerM_ptr->text(LOG_DEBUG, 1, "MemoryTransfer Syntax: Explicit VR Big Endian"); break;
        default: loggerM_ptr->text(LOG_DEBUG, 1, "Memory Transfer Syntax: %X = Unknown", ms_codeM); break;
        }
        loggerM_ptr->text(LOG_DEBUG, 1, "File Bits Allocated: %d", bits_allocatedM);
	}

	return byte_swap;
}

//>>===========================================================================

void OV_VALUE_STREAM_CLASS::SwapBytes(BYTE* buffer_ptr, UINT32 length)

//  DESCRIPTION     : Perform byte swap in 32-bit data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// byte swap data
	UINT32 i = 0;

	while (i < length) 
	{
		BYTE temp1 = buffer_ptr[i];
		BYTE temp2 = buffer_ptr[i+1];
		BYTE temp3 = buffer_ptr[i+2];
		buffer_ptr[i] = buffer_ptr[i+3];
		buffer_ptr[i+1] = temp3;
		buffer_ptr[i+2] = temp2;
		buffer_ptr[i+3] = temp1;
		i += 4;
	}
}

//>>===========================================================================

bool OV_VALUE_STREAM_CLASS::StreamPatternTo(DATA_TF_CLASS& data_transfer)

//  DESCRIPTION     : Stream a OV pattern to the data_transfer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32 value;
	UINT row_start = start_valueM;
	UINT row_inc = rows_incrementM;
	UINT col_inc = columns_incrementM;
	UINT rows_same = rows_sameM;
	UINT columns_same = columns_sameM;
	bool byte_swap = IsByteSwapRequired();
	const UINT nr_of_OT_in_OF = 4;

	// generate a simple test pattern
	if ((rowsM * columnsM * nr_of_OT_in_OF) != lengthM) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "OF data generation failure - rows (%d) * columns (%d) not equal length (%d)", rowsM, columnsM, lengthM);
		}
		return false;
	}

	UINT32 length = columnsM;	// columns = row length (NP)
	UINT32 *data_ptr = new UINT32[length];

	if (rows_same == 0) 
	{
		rows_same++;
	}
	if (columns_same == 0) 
	{
		columns_same++;
	}

	for (UINT rows = 0; rows < rowsM; rows++) 
	{
		value = (UINT32)row_start;
		for (UINT columns = 0; columns < columnsM; columns++) 
		{
			data_ptr[columns] = value;

			if ((columns + 1) % columns_same == 0 )
			{
				if ((value == 0xFFFF) && (col_inc != 0))
				{
					value = 0;
				}
				else
				{
					value += (UINT32)col_inc;
				}
			}
		}

		// swap the data bytes if required 
		if (byte_swap) 
		{
			SwapBytes((BYTE*) data_ptr, length * 4);
		}

		// write data into buffer
		data_transfer.writeBinary((BYTE*) data_ptr, length * 4);

		if ((rows + 1) % rows_same == 0)
		{
			if ((row_start == 0xFFFF) && (row_inc !=0))
			{
				row_start = 0;
			}
			else
			{
				row_start += row_inc;
			}
		}
	}

	// cleanup
	delete data_ptr;

	// return result
	return true;
}

//>>===========================================================================

DVT_STATUS OV_VALUE_STREAM_CLASS::Compare(LOG_MESSAGE_CLASS *message_ptr, OV_VALUE_STREAM_CLASS &refOfStream)

//  DESCRIPTION     : Compare two OF file streams.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    // read the file headers
    bool resultSrc = ReadFileHeader();
    bool resultRef = refOfStream.ReadFileHeader();
    if ((!resultSrc) || (!resultRef)) return MSG_ERROR;

	// display the headers for debugging
	DisplayHeaderDetail();
	refOfStream.DisplayHeaderDetail();

    // check the length
    bool srcOk = false;
    bool refOk = false;
    if (GetLength(&srcOk) != refOfStream.GetLength(&refOk)) return MSG_NOT_EQUAL;
    if ((srcOk == false) || (refOk == false)) return MSG_NOT_EQUAL;

    // now check the file contents - endianess
    // - may need byte swap to be able to compare the data
    bool byteSwap = true;
    if (fs_codeM & TS_LITTLE_ENDIAN)
    {
        if (refOfStream.fs_codeM & TS_LITTLE_ENDIAN)
        {
            byteSwap = false;
        }
    }
    else if (fs_codeM & TS_BIG_ENDIAN)
    {
        if (refOfStream.fs_codeM & TS_BIG_ENDIAN)
        {
            byteSwap = false;
        }
    }
    if (byteSwap)
    {
        // need to byte swap either source or reference before comparison
        sprintf (message, "Source OV File data being byte swapped before comparison with Reference OV data");
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_18, message);
    }

    UINT32 lengthToCompare = (UINT32)lengthM;

    // read the data in blocks and compare
    UINT32	blockLength = MAX_OV_BLOCK_LENGTH;
    BYTE *srcData = new BYTE [blockLength];
    BYTE *refData = new BYTE [blockLength];

    // compare the data
    DVT_STATUS status = MSG_EQUAL;
    while ((lengthToCompare > 0) &&
        (status == MSG_EQUAL))
    {
	    // get the next block length
	    if (lengthToCompare < blockLength) blockLength = lengthToCompare;

		// read data into buffer
		UINT32 srcLengthRead = readBinary(srcData, blockLength);
		if (srcLengthRead == blockLength)
		{
            UINT32 refLengthRead = refOfStream.readBinary(refData, blockLength);
		    if (refLengthRead == blockLength)
		    {
                // check for byte swap
                if (byteSwap)
                {
                    SwapBytes(refData, blockLength);
                }

                // now compare the data
                if (!byteCompare(srcData, refData, blockLength))
                {
                    status = MSG_NOT_EQUAL;
                }

                // move to next block
                lengthToCompare -= blockLength;
            }
            else
            {
                status = MSG_ERROR;
            }
        }
        else
        {
            status = MSG_ERROR;
        }
	}

	// cleanup
	delete srcData;
    delete refData;

    // return status
    return status;
}
