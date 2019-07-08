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

//>>===========================================================================

bool OB_VALUE_STREAM_CLASS::StreamPatternTo(DATA_TF_CLASS& data_transfer)

//  DESCRIPTION     : Stream a OB pattern to the data_transfer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BYTE value;
	UINT row_start = start_valueM;
	UINT row_inc = rows_incrementM;
	UINT col_inc = columns_incrementM;
	UINT rows_same = rows_sameM;
	UINT columns_same = columns_sameM;
	bool byte_swap = IsByteSwapRequired();

	// generate a simple test pattern - values have previously been range checked
	if ((rowsM * columnsM) != lengthM) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "ERROR: OB data generation failure - rows (%d) * columns (%d) not equal length (%d)", rowsM, columnsM, lengthM);
		}
		return false;
	}

	UINT32 length = columnsM;	// columns = row length (NP)
	BYTE *data_ptr = new BYTE [length];

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
		value = (BYTE)row_start;
		for (UINT columns = 0; columns < columnsM; columns++) 
		{
			data_ptr[columns] = value;

			if ((columns + 1) % columns_same == 0 )
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
			if ((row_start == 0xFF) && (row_inc !=0))
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

	// return result
	return true;
}

//>>===========================================================================

bool OB_VALUE_STREAM_CLASS::ReadBOTAndDataFragments()

//  DESCRIPTION     : Read the optional BOT and Data Fragments.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// BOT and data fragments only present in a compressed OB file
	if (!(fs_codeM & TS_COMPRESSED)) return true;

	// save the current file position
	UINT old_file_offset = getOffset();

	// pixel data is compressed - need to import fragments
	UINT32 basic_offset_table_length = 0;
	UINT32 fragment_offset = 0;
	UINT16 group = 0;
	UINT16 element = 0;
	UINT32 length = 0;
	int count = 0;
    bool looping = true;
	while (looping)
	{
		// import a tag and length
		if (!((*this) >> group))
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Item Tag (Group) in compressed OB data");
				return false;
			}
		}

		if (!((*this) >> element))
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Item Tag (Element) in compressed OB data");
				return false;
			}
		}

		if (!((*this) >> length))
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Item Length in compressed OB data");
				return false;
			}
		}

		// length should always be even
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Item Tag (%04X,%04X) in compressed OB data has length of: %08X", group, element, length);
		}

		// increment count
		count++;

		// check for an encoded fragment
		if ((group == ITEM_GROUP) &&
			(element == ITEM_ELEMENT))
		{
			// check for Basic Offset Table
			if (count == 1)
			{
				UINT32 offset_value;

				// got the Basic Offset Table
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Basic Offset Table item imported in compressed OB data with length: %d", length);
					if ((length) &&
						(length % sizeof(offset_value)))
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Basic Offset Table item length %d is not a multiple of 4", length);
					}
				}

				// allocate space for the Basic Offset Table to store the values
				AllocateBasicOffsetTable(length / sizeof(offset_value));
				UINT32 index = 0;

				// save the total length of the Basic Offset Table
				basic_offset_table_length = sizeof(group) + sizeof(element) + sizeof(length) + length;

				// loop importing Basic Offset Table contents
				while (length / sizeof(offset_value))
				{
					// import an offset value
					if (!((*this) >> offset_value))
					{
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Basic Offset Table value in compressed OB data");
							return false;
						}
					}

					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Importing Basic Offset Table value of %d", offset_value);
					}

					// save the offset value details
					AddBasicOffsetTableValue(index++, offset_value);

					// decrement the length
					length -= sizeof(offset_value);
				}
			}
			else
			{
				// skip over the compressed OB data fragment
				UINT file_offset = getOffset();

				// save the encoded pixel data fragment details
				AddEncodedDataFragment((file_offset - sizeof(group) - sizeof(element) - sizeof(length)), fragment_offset, length);

				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Fragment of compressed OB data imported with Offset: %d and Length: %d", fragment_offset, length);
				}

				// increment the fragment offset
				fragment_offset += (sizeof(group) + sizeof(element) + sizeof(length) + length);

				// skip over the compressed OB data fragment
				setOffset(file_offset + length);
			}
		}
		else if ((group == ITEM_GROUP) &&
				(element == SQ_DELIMITER))
		{
			// we have hit the end of the fragments
			// - length should be zero
			if (length)
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Compressed OB data sequence delimiter (FFFE,E0DD) should have zero-length - imported length of: %d", length);
				}

				return false;
			}
			else
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Found the compressed OB data sequence delimiter (FFFE,E0DD) with zero-length");
				}
			}

			// we are done
			break;
		}
		else
		{
			// syntax error - don't know what we have imported
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Invalid Tag (%04X,%04X) with length %08X detected while importing compressed OB data - expected Basic Offset Table or Encoded Data Fragment", group, element, length);
			}

			// can't do anymore
			return false;
		}
	}

	// restore the file position
	setOffset(old_file_offset);

	// return result
	return true;
}

//>>===========================================================================

UINT32 OB_VALUE_STREAM_CLASS::ReadDataFragment(UINT index, BYTE *buffer_ptr, UINT32 offset, UINT32 length)

//  DESCRIPTION     : Read the indexed data fragment into the buffer provided from the offset given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    UINT32 dfOffset = 0;
    UINT32 dfLength = 0;

    // Get the indexed data fragment
    if (!GetDataFragment(index, dfOffset, dfLength)) return 0;

    // save current offset
	UINT oldFileOffset = getOffset();

    // set the new offset - take data fragment offset plus the given offset
    setOffset(dfOffset + offset);

    // check maximum length
    if (dfLength < length)
    {
        length = dfLength;
    }

    // read from the data fragment into the buffer
    UINT32 bytesRead = readBinary(buffer_ptr, length);

    // reset offset
    setOffset(oldFileOffset);

	// return number of bytes read
	return bytesRead;
}
