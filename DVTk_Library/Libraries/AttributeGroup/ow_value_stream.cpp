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
#include "ob_value_stream.h"
#include "ow_value_stream.h"
#include "../validation/valrules.h"


//>>===========================================================================

OW_VALUE_STREAM_CLASS::OW_VALUE_STREAM_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	Initialise();
	bits_allocatedM = 16;
    file_vrM = GetVR();
}

//>>===========================================================================

OW_VALUE_STREAM_CLASS::~OW_VALUE_STREAM_CLASS()

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

bool OW_VALUE_STREAM_CLASS::SetFilename(string filename)

//  DESCRIPTION     : Set filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the file contents description - from the filename - ADVT requirement
	if (strstr(filename.c_str(), "B08_")) 
	{
		// OB, 8 bit data, endian unimportant
		bits_allocatedM = 8;
		fs_codeM = TS_LITTLE_ENDIAN;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Using original OB 8-bit data as OW 8-bit little-endian data - may produce byte swap problems");
		}
	}
	else if (strstr(filename.c_str(), "B08C")) 
	{
		// OB, 8 bit compressed data, endian unimportant
		bits_allocatedM = 8;
		fs_codeM = TS_LITTLE_ENDIAN | TS_COMPRESSED;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Using original OB 8-bit compressed data as OW 8-bit little-endian data - may produce byte swap problems");
		}
	}
	else if (strstr(filename.c_str(), "W08B")) 
	{
		// OW, 8 bit data, big endian
		bits_allocatedM = 8;
		fs_codeM = TS_BIG_ENDIAN;
	}
	else if (strstr(filename.c_str(), "W08L")) 
	{
		// OW, 8 bit data, little endian
		bits_allocatedM = 8;
		fs_codeM = TS_LITTLE_ENDIAN;
	}
	else if (strstr(filename.c_str(), "W16B"))
	{
		// OW, 16 bit data, big endian
		bits_allocatedM = 16;
		fs_codeM = TS_BIG_ENDIAN;
	}
	else if (strstr(filename.c_str(), "W16L"))
	{
		// OW, 16 bit data, little endian
		bits_allocatedM = 16;
		fs_codeM = TS_LITTLE_ENDIAN;
	}
	else {
		// default to OW, 16 bit data, endian unimportant
		bits_allocatedM = 16;
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

ATTR_VR_ENUM OW_VALUE_STREAM_CLASS::GetVR()

//  DESCRIPTION     : Return class VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return class VR
	return ATTR_VR_OW;
}

//>>===========================================================================

bool OW_VALUE_STREAM_CLASS::IsByteSwapRequired()

//  DESCRIPTION     : Check if byte swap is required in Other Word Data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool byte_swap = false;

	// check criteria for byte swap
	// do not optimise the if..else.. constructions - it should be clearer like this
	if ((bits_allocatedM == 8) && 
		(fs_codeM & TS_LITTLE_ENDIAN) && 
		(ms_codeM & TS_BIG_ENDIAN)) 
	{
		// byte swap
		byte_swap = true;
	}
	else if ((bits_allocatedM == 8) && 
		(fs_codeM & TS_BIG_ENDIAN) && 
		(ms_codeM & TS_LITTLE_ENDIAN)) 
	{
		// byte swap
		byte_swap = true;
	}
	else if ((bits_allocatedM == 16) && 
		(fs_codeM & TS_LITTLE_ENDIAN) && 
		(ms_codeM & TS_BIG_ENDIAN)) 
	{
		// byte swap
		byte_swap = true;
	}
	else if ((bits_allocatedM == 16) && 
		(fs_codeM & TS_BIG_ENDIAN) && 
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
            loggerM_ptr->text(LOG_INFO, 1, "Byte Swap of original OW data needed - details:");
        }
        else
        {
            loggerM_ptr->text(LOG_INFO, 1, "No Byte Swap of original OW data needed - details:");
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

bool OW_VALUE_STREAM_CLASS::StreamPatternTo(DATA_TF_CLASS& data_transfer)

//  DESCRIPTION     : Stream a OW pattern to the data_transfer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT row_start = start_valueM;
	UINT row_inc = rows_incrementM;
	UINT col_inc = columns_incrementM;
	UINT rows_same = rows_sameM;
	UINT columns_same = columns_sameM;
	bool byte_swap = IsByteSwapRequired();

	if (rows_same == 0) 
	{
		rows_same++;
	}
	if (columns_same == 0) 
	{
		columns_same++;
	}

	// get hold of the current bits allocated
	if (bits_allocatedM == 16) 
	{
		UINT16	value;

		// generate a simple test pattern
		const UINT nr_of_OT_in_OW = 2;
		if ((rowsM * columnsM * nr_of_OT_in_OW) != lengthM) 
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "OW data generation failure - rows (%d) * columns (%d) not equal length (%d)", rowsM, columnsM, lengthM);
			}
			return false;
		}

		UINT32 length = columnsM;	// columns = row length (NP)
		UINT16 *data_ptr = new UINT16 [length];

		for (UINT rows = 0; rows < rowsM; rows++) 
		{
			value = (UINT16)row_start;
			for (UINT columns = 0; columns < columnsM; columns++) 
			{
				data_ptr[columns] = value;

				if ((columns + 1) % columns_same == 0)
				{
					if ((value == 0xFFFF) && (col_inc != 0))
					{
						value = 0;
					}
					else
					{
						value = (UINT16)(value + col_inc);
					}
				}
			}

			// swap the data bytes if required 
			if (byte_swap) 
			{
				SwapBytes((BYTE*) data_ptr, length * 2);
			}

			// write data into buffer
			data_transfer.writeBinary((BYTE*) data_ptr, length * 2);

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
	}
	else if (bits_allocatedM == 8) 
	{
		BYTE value;

		// generate a simple test pattern - values have previously been range checked
		if ((rowsM * columnsM) != lengthM) 
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "OW data generation failure - rows (%d) * columns (%d) not equal length (%d)", rowsM, columnsM, lengthM);
			}
			return false;
		}

		UINT32 length = columnsM;	// columns = row length (NP)
		BYTE *data_ptr = new BYTE [length];

		for (UINT rows = 0; rows < rowsM; rows++) 
		{	
			value = (BYTE)row_start;
			for (UINT columns = 0; columns < columnsM; columns++) 
			{
				data_ptr[columns] = value;

				if ((columns + 1) % columns_same == 0)
				{
					if ((value == 0xFF) && (col_inc != 0))
					{
						value = 0;
					}
					else
					{
						value = (BYTE)(value + col_inc);
					}
				}
			}

			// swap the data bytes if required 
			if (byte_swap) 
			{
				SwapBytes(data_ptr, length);
			}

			// write data into buffer
			data_transfer.writeBinary(data_ptr, length);

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

		// check for odd length - last fragment!
		if (lengthM & 0x1) 
		{
			data_ptr[0] = 0x00;

			// write data into buffer
			data_transfer.writeBinary(data_ptr, 1);
		}

		// cleanup
		delete data_ptr;
	}

	// return result
	return true;
}

//>>===========================================================================

DVT_STATUS OW_VALUE_STREAM_CLASS::Compare(LOG_MESSAGE_CLASS *message_ptr, OB_VALUE_STREAM_CLASS &refObStream)

//  DESCRIPTION     :  Compare this against the reference OB value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    // read the file headers
    bool resultSrc = ReadFileHeader();
    bool resultRef = refObStream.ReadFileHeader();
    if ((!resultSrc) || (!resultRef)) return MSG_ERROR;

	// display the headers for debugging
	DisplayHeaderDetail();
	refObStream.DisplayHeaderDetail();

    // check that the OB data is 8 bit allocated
    if (refObStream.GetBitsAllocated() != 8)
    {
        sprintf (message, "Reference OB File data - Bits Allocated not 8 but %d", refObStream.GetBitsAllocated());
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_20, message);
        return MSG_INCOMPATIBLE;
    }

    // check that the OB data is not compressed
    if (refObStream.GetFileTSCode() & TS_COMPRESSED)
    {
        sprintf (message, "Reference OB File data is compressed - can't be compared to Source OW data");
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_20, message);
        return MSG_INCOMPATIBLE;
    }

    // check the length
    bool srcOk = false;
    bool refOk = false;
    if (GetLength(&srcOk) != refObStream.GetLength(&refOk)) return MSG_NOT_EQUAL;
    if ((srcOk == false) || (refOk == false)) return MSG_NOT_EQUAL;

    // now check the file contents - endianess
    // - may need byte swap to be able to compare the data
    bool byteSwap = true;
    if (fs_codeM & TS_LITTLE_ENDIAN)
    {
		if (refObStream.GetFileTSCode() & TS_LITTLE_ENDIAN)
        {
            byteSwap = false;
        }
    }
    else if (fs_codeM & TS_BIG_ENDIAN)
    {
        if (refObStream.GetFileTSCode() & TS_BIG_ENDIAN)
        {
            byteSwap = false;
        }
    }
    if (byteSwap)
    {
        // need to byte swap either source or reference before comparison
        sprintf (message, "Source OW File data being byte swapped before comparison with Reference OB data");
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_19, message);
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
            UINT32 refLengthRead = refObStream.readBinary(refData, blockLength);
		    if (refLengthRead == blockLength)
		    {
                // check for byte swap
                if (byteSwap)
                {
                    // swap OW source data
                    SwapBytes(srcData, blockLength);
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

//>>===========================================================================

DVT_STATUS OW_VALUE_STREAM_CLASS::Compare(LOG_MESSAGE_CLASS *message_ptr, OW_VALUE_STREAM_CLASS &refOwStream)

//  DESCRIPTION     :  Compare this against the reference OW value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    // read the file headers
    bool resultSrc = ReadFileHeader();
    bool resultRef = refOwStream.ReadFileHeader();
    if ((!resultSrc) || (!resultRef)) return MSG_ERROR;

	// display the headers for debugging
	DisplayHeaderDetail();
	refOwStream.DisplayHeaderDetail();

    // check the length
    bool srcOk = false;
    bool refOk = false;
    if (GetLength(&srcOk) != refOwStream.GetLength(&refOk)) return MSG_NOT_EQUAL;
    if ((srcOk == false) || (refOk == false)) return MSG_NOT_EQUAL;

    // now check the file contents - endianess
    // - may need byte swap to be able to compare the data
    bool byteSwap = true;
    if (fs_codeM & TS_LITTLE_ENDIAN)
    {
        if (refOwStream.fs_codeM & TS_LITTLE_ENDIAN)
        {
            byteSwap = false;
        }
    }
    else if (fs_codeM & TS_BIG_ENDIAN)
    {
        if (refOwStream.fs_codeM & TS_BIG_ENDIAN)
        {
            byteSwap = false;
        }
    }
    if (byteSwap)
    {
        // need to byte swap either source or reference before comparison
        sprintf (message, "Source OW File data being byte swapped before comparison with Reference OW data");
        if (message_ptr != NULL) message_ptr->AddMessage(VAL_RULE_D_OTHER_19, message);
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
            UINT32 refLengthRead = refOwStream.readBinary(refData, blockLength);
		    if (refLengthRead == blockLength)
		    {
                // check for byte swap
                if (byteSwap)
                {
                    // doesn't matter whether we swap source or reference data
                    SwapBytes(srcData, blockLength);
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
