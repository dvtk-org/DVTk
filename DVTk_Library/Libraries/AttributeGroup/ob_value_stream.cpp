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
#include "../validation/valrules.h"


//>>===========================================================================

OB_VALUE_STREAM_CLASS::OB_VALUE_STREAM_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	Initialise();
	bits_allocatedM = 8;
    file_vrM = GetVR();

    no_basic_offset_table_entriesM = 0;
	basic_offset_tableM_ptr = NULL;
}

//>>===========================================================================

OB_VALUE_STREAM_CLASS::~OB_VALUE_STREAM_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (basic_offset_tableM_ptr) 
	{
		delete [] basic_offset_tableM_ptr;
		no_basic_offset_table_entriesM = 0;
	}

	data_fragmentM.clear();
}

//>>===========================================================================

bool OB_VALUE_STREAM_CLASS::SetFilename(string filename)

//  DESCRIPTION     : Set filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the file contents description - from the filename - ADVT requirement
	if ((strstr(filename.c_str(), "B08_")) ||
		(strstr(filename.c_str(), "B16_")))
	{
		// OB, 8 or 16 bit data, endian unimportant
		fs_codeM = TS_LITTLE_ENDIAN;
	}
	else if ((strstr(filename.c_str(), "B08C")) ||
		(strstr(filename.c_str(), "B16C")))
	{
		// OB, 8 or 16 bit compressed data, endian unimportant
		fs_codeM = TS_LITTLE_ENDIAN | TS_COMPRESSED;
	}
	else if (strstr(filename.c_str(), "W08B")) 
	{
		// OW, 8 bit data, big endian
		fs_codeM = TS_BIG_ENDIAN;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Using original OW 8-bit big-endian data as OB 8-bit data - may produce byte swap problems");
		}
	}
	else if (strstr(filename.c_str(), "W08L")) 
	{
		// OW, 8 bit data, little endian
		fs_codeM = TS_LITTLE_ENDIAN;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Using original OW 8-bit little-endian data as OB 8-bit data - may produce byte swap problems");
		}
	}
	else if (strstr(filename.c_str(), "W16B")) 
	{
		// OW, 16 bit data, big endian
		fs_codeM = TS_BIG_ENDIAN;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Using original OW 16-bit big-endian data as OB 8-bit data - may produce byte swap problems");
		}
	}
	else if (strstr(filename.c_str(), "W16L")) 
	{
		// OW, 16 bit data, little endian
		fs_codeM = TS_LITTLE_ENDIAN;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Using original OW 16-bit little-endian data as OB 8-bit data - may produce byte swap problems");
		}
	}
	else 
	{
		// default to OB, 8 bit data, endian unimportant
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

ATTR_VR_ENUM OB_VALUE_STREAM_CLASS::GetVR()

//  DESCRIPTION     : Return class VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return class VR
	return ATTR_VR_OB;
}

//>>===========================================================================

bool OB_VALUE_STREAM_CLASS::IsByteSwapRequired()

//  DESCRIPTION     : Check if byte swap is required in Other Byte Data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// byte swap never required
	return false;
}

//>>===========================================================================

UINT OB_VALUE_STREAM_CLASS::NoBasicOffsetTableEntries()

//  DESCRIPTION     : Return number of basic offset table entries.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return the number of basic offset table entries
	return no_basic_offset_table_entriesM;
}

//>>===========================================================================

bool OB_VALUE_STREAM_CLASS::GetBasicOffsetTableEntry(UINT index, UINT32& offset)

//  DESCRIPTION     : Return indexed basic offset table entry.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check that the BOT is defined and large enough
	if ((basic_offset_tableM_ptr) &&
		(index < no_basic_offset_table_entriesM))
	{
		// return the offset
		offset = basic_offset_tableM_ptr[index];
		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================

UINT OB_VALUE_STREAM_CLASS::NoDataFragments()

//  DESCRIPTION     : Return number of data fragments.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return the number of data fragments
	return data_fragmentM.size();
}

//>>===========================================================================

bool OB_VALUE_STREAM_CLASS::GetDataFragment(UINT index, UINT32& offset, UINT32& length)

//  DESCRIPTION     : Return indexed data fragment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check valid index
	if (index < data_fragmentM.size())
	{
		// return the pixel data fragment details
		offset = data_fragmentM[index].GetOffset();
		length = data_fragmentM[index].GetLength();
		result = true;
	}

	// result result
	return result;
}

//>>===========================================================================

void OB_VALUE_STREAM_CLASS::AllocateBasicOffsetTable(UINT no_values)

//  DESCRIPTION     : Allocate a basic offset table with the given number of values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// allocate space to store the BOT values
	if (basic_offset_tableM_ptr) 
	{
		delete [] basic_offset_tableM_ptr;
	}
	basic_offset_tableM_ptr = new UINT32[no_values];

	if (basic_offset_tableM_ptr)
	{
		no_basic_offset_table_entriesM = no_values;
	}
	else
	{
		no_basic_offset_table_entriesM = 0;
	}
}

//>>===========================================================================

void OB_VALUE_STREAM_CLASS::AddBasicOffsetTableValue(UINT index, UINT32 offset)

//  DESCRIPTION     : Add the indexed basic offset table entry.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check that the BOT is defined and large enough
	if ((basic_offset_tableM_ptr) &&
		(index < no_basic_offset_table_entriesM))
	{
		// save the offset
		basic_offset_tableM_ptr[index] = offset;
	}
}

//>>===========================================================================

void OB_VALUE_STREAM_CLASS::AddEncodedDataFragment(UINT32 file_offset, UINT32 offset, UINT32 length)

//  DESCRIPTION     : Add the data fragment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// save the pixel data fragment details
	DATA_FRAGMENT_CLASS data_fragment(file_offset, offset, length);
	data_fragmentM.push_back(data_fragment);
}

//>>===========================================================================

DVT_STATUS OB_VALUE_STREAM_CLASS::Compare(LOG_MESSAGE_CLASS *message_ptr, OB_VALUE_STREAM_CLASS &refStream)

//  DESCRIPTION     : Compare this against the reference OB value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    // read the file headers
    bool resultSrc = ReadFileHeader();
    bool resultRef = refStream.ReadFileHeader();
    if ((!resultSrc) || (!resultRef)) return MSG_ERROR;

	// display the headers for debugging
	DisplayHeaderDetail();
	refStream.DisplayHeaderDetail();

   // check that the OB data is 8 bit allocated
   if (GetBitsAllocated() != 8)
   {
        sprintf (message, "Source OB File data - Bits Allocated not 8 but %d", GetBitsAllocated());
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_20, message);
        return MSG_INCOMPATIBLE;
   }
   if (refStream.GetBitsAllocated() != 8)
    {
        sprintf (message, "Reference OB File data - Bits Allocated not 8 but %d", refStream.GetBitsAllocated());
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_20, message);
        return MSG_INCOMPATIBLE;
    }

    // check if the OB data is compressed
    DVT_STATUS status = MSG_ERROR;
    if ((fs_codeM & TS_COMPRESSED) &&
        (refStream.GetFileTSCode() & TS_COMPRESSED))
    {
        status = CompareCompressed(message_ptr, refStream);
    }
    else if ((fs_codeM & TS_COMPRESSED) ||
        (refStream.GetFileTSCode() & TS_COMPRESSED))
    {
        // can't compare compressed with non-compressed
        sprintf (message, "Can't compare OB Compressed and Uncompressed File data");
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_12, message);
        status = MSG_INCOMPATIBLE;
    }
    else
    {
        // compare plain OB data
        // check the length
        bool srcOk = false;
        bool refOk = false;
        if (GetLength(&srcOk) != refStream.GetLength(&refOk)) return MSG_NOT_EQUAL;
        if ((srcOk == false) || (refOk == false)) return MSG_NOT_EQUAL;

        // now check the file contents
        UINT32 lengthToCompare = (UINT32)lengthM;

        // read the data in blocks and compare
        UINT32	blockLength = MAX_OV_BLOCK_LENGTH;
        BYTE *srcData = new BYTE [blockLength];
        BYTE *refData = new BYTE [blockLength];

        // compare the data
        status = MSG_EQUAL;
        while ((lengthToCompare > 0) &&
            (status == MSG_EQUAL))
        {
	        // get the next block length
	        if (lengthToCompare < blockLength) blockLength = lengthToCompare;

		    // read data into buffer
		    UINT32 srcLengthRead = readBinary(srcData, blockLength);
		    if (srcLengthRead == blockLength)
		    {
                UINT32 refLengthRead = refStream.readBinary(refData, blockLength);
		        if (refLengthRead == blockLength)
		        {
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
    }

    // return status
    return status;
}

//>>===========================================================================

DVT_STATUS OB_VALUE_STREAM_CLASS::CompareCompressed(LOG_MESSAGE_CLASS *message_ptr, OB_VALUE_STREAM_CLASS &refStream)

//  DESCRIPTION     : Compare this against the reference OB value - both are compressed.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    // read the BOT (Basic Offset Tables
    bool resultSrc = ReadBOTAndDataFragments();
    bool resultRef = refStream.ReadBOTAndDataFragments();
    if ((!resultSrc) || (!resultRef)) return MSG_ERROR;

    // check that the number of BOT entries is the same
    if (NoBasicOffsetTableEntries() != refStream.NoBasicOffsetTableEntries()) 
    {
        sprintf (message, "Basic Offset Table - difference in number of entries: Source: %d, Reference: %d", NoBasicOffsetTableEntries(), refStream.NoBasicOffsetTableEntries());
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_13, message);
        return MSG_NOT_EQUAL;
    }

    // check that the number of data fragment is the same
    if (NoDataFragments() != refStream.NoDataFragments())
    {
        sprintf (message, "Data Fragments - difference in number of fragments: Source: %d, Reference: %d", NoDataFragments(), refStream.NoDataFragments());
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_14, message);
        return MSG_NOT_EQUAL;
    }

    // now compare the BOT entries - offset by offset
    DVT_STATUS status = MSG_OK;
    for (UINT i = 0; i < NoBasicOffsetTableEntries(); i++)
    {
        UINT32 srcOffset;
        GetBasicOffsetTableEntry(i, srcOffset);

        UINT32 refOffset;
        refStream.GetBasicOffsetTableEntry(i, refOffset);
        
        // compare the offsets
        if (srcOffset != refOffset)
        {
            sprintf (message, "Basic Offset Table - Offsets[%d] different: Source: %d, Reference: %d", i+1, srcOffset, refOffset);
            if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_15, message);
            status = MSG_NOT_EQUAL;
        }
    }
    if (status != MSG_OK) return status;

    // allocate comparison buffers
    BYTE *srcData = new BYTE [MAX_OV_BLOCK_LENGTH];
    BYTE *refData = new BYTE [MAX_OV_BLOCK_LENGTH];

    // now compare the data fragment entries
   /* for (i = 0; i < NoDataFragments(); i++)  MIGRATION_IN_PROGRESS*/  for (UINT i = 0; i < NoDataFragments(); i++)  
    {
        UINT32 srcOffset;
        UINT32 srcLength;
        GetDataFragment(i, srcOffset, srcLength);

        UINT32 refOffset;
        UINT32 refLength;
        refStream.GetDataFragment(i, refOffset, refLength);

        // compare the data fragment offsets
        if (srcOffset != refOffset) 
        {
            sprintf (message, "Data Fragments - Fragment Offsets[%d] different: Source: %d, Reference: %d", i+1, srcOffset, refOffset);
            if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_16, message);
            delete srcData;
            delete refData;
            return MSG_NOT_EQUAL;
        }

        // compare the data fragment lengths
        if (srcLength != refLength) 
        {
            sprintf (message, "Data Fragments - Fragment Lengths[%d] different: Source: %d, Reference: %d", i+1, srcLength, refLength);
            if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_17, message);
            delete srcData;
            delete refData;
            return MSG_NOT_EQUAL;
        }

        // finally we know that the data fragment offsets and lengths are the same
        // - now check the data fragment contents
        UINT32	blockLength = MAX_OV_BLOCK_LENGTH;
        UINT32 lengthToCompare = srcLength;
        UINT32 fragmentOffset = 0;

        status = MSG_EQUAL;
        while ((lengthToCompare > 0) &&
            (status == MSG_EQUAL))
        {
	        // get the next block length
	        if (lengthToCompare < blockLength) blockLength = lengthToCompare;

            UINT32 srcLengthRead = ReadDataFragment(i, srcData, fragmentOffset, blockLength);
            if (srcLengthRead == blockLength)
            {
                UINT32 refLengthRead = refStream.ReadDataFragment(i, refData, fragmentOffset, blockLength); 
		        if (refLengthRead == blockLength)
		        {
                    // now compare the data
                    if (!byteCompare(srcData, refData, blockLength))
                    {
                        status = MSG_NOT_EQUAL;
                    }

                    // move to next block
                    lengthToCompare -= blockLength;
                    fragmentOffset += blockLength;
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
    }
    
    // cleanup
    delete srcData;
    delete refData;

    // return status
    return status;
}

