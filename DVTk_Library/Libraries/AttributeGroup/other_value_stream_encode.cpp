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
#include "other_value_stream.h"


//>>===========================================================================

bool OTHER_VALUE_STREAM_CLASS::StreamTo(DATA_TF_CLASS& data_transfer)

//  DESCRIPTION     : Stream to the data transfer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	// set the memory transfer syntax from the data transfer
	UID_CLASS transfer_syntax;
	transfer_syntax.set(data_transfer.getTransferSyntax());
	SetMemoryTransferSyntax(transfer_syntax);

	// set up the stream length
	GetLength(&result);
    if (result == false) return result;

	// check data source
	switch(data_sourceM) 
	{
	case INFILE: 
		{
		// read DVT header
		if (!ReadFileHeader()) return false;

		// check if byte swap is required
		bool byte_swap = IsByteSwapRequired();

		// check for compressed / non-compressed mismatch
		if ((ms_codeM & TS_COMPRESSED) != (fs_codeM & TS_COMPRESSED))
		{
			if (loggerM_ptr)
			{
				if ((ms_codeM & TS_COMPRESSED) != 0)
				{
// Need to set the correct compression setting in the data sent before uncommenting the following code
//					loggerM_ptr->text(LOG_WARNING, 1, "Sending non-compressed data with a compressed transfer syntax");
				}
				else
				{
//					loggerM_ptr->text(LOG_WARNING, 1, "Sending compressed data with a non-compressed transfer syntax");
				}
			}
		}

		// work out block sizes to export
		UINT32	block_length = MAX_OV_BLOCK_LENGTH;
		UINT32	total_length = (UINT32)lengthM;

		// allocate the Data buffer here
		if (total_length < block_length) block_length = total_length;
		BYTE *data_ptr = new BYTE [block_length + 1];

		// consume the data in blocks
		while (total_length > 0) 
		{
			if (total_length < MAX_OV_BLOCK_LENGTH)
			{
				block_length = total_length;
			}
			else
			{
				block_length = MAX_OV_BLOCK_LENGTH;
			}

			// read the data from file
			if (readBinary(data_ptr, block_length) != (INT) block_length) 
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Reading %d Other data from %s", block_length, filenameM.c_str());
				}
				total_length = 0;
			}
			else 
			{
				total_length -= block_length;

				// check for odd length - last fragment!
				if (block_length & 0x1) 
				{
					data_ptr[block_length] = 0x00;

					// swap the data bytes if required 
					if (byte_swap) 
					{
						SwapBytes(data_ptr, block_length + 1);
					}

					// write data into buffer
					data_transfer.writeBinary(data_ptr, block_length + 1);
				}
				else 
				{
					// swap the data bytes if required 
					if (byte_swap) 
					{
						SwapBytes(data_ptr, block_length);
					}

					// write data into buffer
					data_transfer.writeBinary(data_ptr, block_length);
				}
			}
		}

		// cleanup
		delete data_ptr;

		// return result
		result = true;
		}
		break;

	case GENERATE: 
		// generate a pattern
		result = StreamPatternTo(data_transfer);
		break;
	}

	// return result
	return result;
}

//>>===========================================================================

bool OTHER_VALUE_STREAM_CLASS::StreamFrom(DATA_TF_CLASS& data_transfer)

//  DESCRIPTION     : Stream from the data transfer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	bool compressed = false;

	// set the file and memory transfer syntaxes from the data transfer
	UID_CLASS transfer_syntax;
	transfer_syntax.set(data_transfer.getTransferSyntax());
	SetFileTransferSyntax(transfer_syntax);
	SetMemoryTransferSyntax(transfer_syntax);

	// check if we have an undefined length
	if (lengthM == UNDEFINED_LENGTH)
	{
		// check if we have a compressed transfer syntax
		if (data_transfer.isCompressed() == false)
		{
			// force the transfer syntax to compressed - it may be that we have had to try to deduce the
			// transfer syntax from a dataset and have not been able to establish if the pixel data is
			// compressed or not.
			TS_CODE ts_code = data_transfer.getTsCode();
			ts_code |=  TS_COMPRESSED;
			data_transfer.setTsCode(ts_code, data_transfer.getTransferSyntax());

			ms_codeM |=  TS_COMPRESSED;

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_INFO, 1, "Transfer Syntax provided did not indicate compressed Other Data. UNDEFINED length found - assuming Other Data is compressed.");
			}
		}
	}

	// check for compressed other data
	if ((ms_codeM & TS_COMPRESSED) &&
		(lengthM == UNDEFINED_LENGTH))
	{
		// the actual length is computed as we parse the compressed data fragments
		lengthM = 0;
		compressed = true;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Decoding compressed Other data");
		}
	}

	// check if we are going to save the Other data
	if ((data_transfer.getStorageMode() == SM_NO_STORAGE) ||
		(data_transfer.getStorageMode() == SM_AS_MEDIA_ONLY))
	{
		// just a name for display purposes
		filenameM = DATA_NOT_STORED;
	}
	else
	{
		// generate a filename for the Other data
		data_sourceM = INFILE;
		GenerateFilename();

		// open the file for writing
		if (!FILE_TF_CLASS::open(filenameM, "wb"))
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Other data decode failure. Cannot open file for storing data: %s", filenameM.c_str());
			}
			return false;
		}

		// write DVT header
		if (!WriteFileHeader()) return false;
	}

	// check if we have uncompressed data
	if (!compressed)
	{
		// stream the data in directly
		result = StreamInData(data_transfer, (UINT32)lengthM);
	}
	else
	{
		// pixel data is compressed - need to import fragments
		UINT32 basic_offset_table_length = 0;
		UINT32 fragment_offset = 0;
		UINT32 sequence_delimiter_length = 0;
		UINT16 group = 0;
		UINT16 element = 0;
		UINT32 length = 0;
		int count = 0;
        bool looping = true;
		while (looping)
		{
			// import a tag and length
			if (!(data_transfer >> group))
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Item Tag (Group) in compressed Other data");
					return false;
				}
			}

			if (!(data_transfer >> element))
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Item Tag (Element) in compressed Other data");
					return false;
				}
			}

			if (!(data_transfer >> length))
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Item Length in compressed Other data");
					return false;
				}
			}

			// length should always be even
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Item Tag (%04X,%04X) in compressed Other data has length of: %08X", group, element, length);
			}

			// increment count
			count++;

			// check for an encoded fragment
			if ((group == ITEM_GROUP) &&
				(element == ITEM_ELEMENT))
			{
				// check if Other data should be stored
				if (isOpen())
				{
					(*this) << group;
					(*this) << element;
					(*this) << length;
				}

				// check for Basic Offset Table
				if (count == 1)
				{
					UINT32 offset_value;

					// got the Basic Offset Table
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Basic Offset Table item imported in compressed Other data with length: %d", length);
						if ((length) &&
							(length % sizeof(offset_value)))
						{
							loggerM_ptr->text(LOG_ERROR, 1, "Basic Offset Table item length %d is not a multiple of 4", length);
						}
					}

					// save the total length of the Basic Offset Table
					basic_offset_table_length = sizeof(group) + sizeof(element) + sizeof(length) + length;

					// loop importing Basic Offset Table contents
					while (length / sizeof(offset_value))
					{
						// import an offset value
						if (!(data_transfer >> offset_value))
						{
							if (loggerM_ptr)
							{
								loggerM_ptr->text(LOG_ERROR, 1, "Failed to import Basic Offset Table value in compressed Other data");
								return false;
							}
						}

						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_DEBUG, 1, "Importing Basic Offset Table value of %d", offset_value);
						}

						// check if Other data should be stored
						if (isOpen())
						{
							(*this) << offset_value;
						}

						// decrement the length
						length -= sizeof(offset_value);
					}
				}
				else
				{
					// stream the Other data in
					if (!StreamInData(data_transfer, length))
					{
						return false;
					}

					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Fragment of encoded pixel data imported with Offset: %d and Length: %d", fragment_offset, length);
					}

					// increment the fragment offset
					fragment_offset += (sizeof(group) + sizeof(element) + sizeof(length) + length);
				}
			}
			else if ((group == ITEM_GROUP) &&
				(element == SQ_DELIMITER))
			{
				// check if compressed Other data sequence delimiter should be saved
				if (isOpen())
				{
					(*this) << group;
					(*this) << element;
					(*this) << length;
				}

				// set the sequence delimiter length
				sequence_delimiter_length = sizeof(group) + sizeof(element) + sizeof(length);

				// we have hit the end of the fragments
				// - length should be zero
				if (length)
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Compressed Other data sequence delimiter (FFFE,E0DD) should have zero-length - imported length of: %d", length);
					}
					result = false;
				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Found the compressed Other data sequence delimiter (FFFE,E0DD) with zero-length");
					}
					result = true;
				}

				// we are done
				break;
			}
			else
			{
				// syntax error - don't know what we have imported
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Invalid Tag (%04X,%04X) with length %08X detected while importing compressed Other data - expected Basic Offset Table or Encoded Data Fragment", group, element, length);
				}

				// can't do anymore
				result = false;
				break;
			}
		}

		// save the actual pixel data length
		lengthM = basic_offset_table_length + fragment_offset + sequence_delimiter_length;

		// check if Other data should be stored
		if (isOpen())
		{
			// write the actual pixel data length to the file
			UINT offset = getOffset();
			setOffset(length_offsetM);

			// write the actual length
			(*this) << lengthM;

			// reset the file offset
			setOffset(offset);

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Import compressed Other data length set in Other data file context: %d", lengthM);
			}
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool OTHER_VALUE_STREAM_CLASS::StreamInData(DATA_TF_CLASS& data_transfer, UINT32 length)

//  DESCRIPTION     : Stream the Other data in.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32	block_length = MAX_OV_BLOCK_LENGTH;

	// allocate the Data buffer here
	if (length < block_length) block_length = length;
	BYTE *data_ptr = new BYTE [block_length];

	// consume the data in blocks
	while (length > 0) 
	{
		if (length < MAX_OV_BLOCK_LENGTH)
		{
			block_length = length;
		}
		else
		{
			block_length = MAX_OV_BLOCK_LENGTH;
		}

		// read data into buffer
		UINT32 length_read = data_transfer.readBinary(data_ptr, block_length);
		if (length_read != block_length)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to read %d Other data from input stream - only got %d", block_length, length_read);
			}

			// cleanup
			delete data_ptr;

			// return result
			return false;
		}

		// check if Other data should be stored
		if (isOpen())
		{
			// write the data to file
			if (!writeBinary(data_ptr, block_length)) 
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to write %d Other data to %s", block_length, filenameM.c_str());
				}

				// cleanup
				delete data_ptr;

				// return result
				return false;
			}
			else
			{
				length -= block_length;
			}
		}
		else
		{
			// move to next block
			length -= block_length;
		}
	}

	// cleanup
	delete data_ptr;

	// return result
	return true;
}

//>>===========================================================================

bool OTHER_VALUE_STREAM_CLASS::ReadFileHeader()

//  DESCRIPTION     : Read the file header.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// check if the data is not stored
	if (filenameM == DATA_NOT_STORED) return false;

    // open file
	if (!FILE_TF_CLASS::open(filenameM, "rb"))
	{
    	if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Other data encode failure. Cannot open file for reading data: %s", filenameM.c_str());
		}
        return false;
	}

	// check if the data file has a header
	// - assume big endian file content
	setEndian(BIG_ENDIAN);
		
	// read the magic number = 42
	UINT16 endian;
	(*this) >> endian;
	bool header_included = false;

	TS_CODE fs_code = TS_LITTLE_ENDIAN;

	// big endian format
	if (endian == 0x002A)
	{
		// indicate that the file is in big endian
		fs_code = TS_BIG_ENDIAN;
		header_included = true;
	}
	else if (endian == 0x2A00)
	{
		// little endian - read as big endian
		// - indicate that the file is in little endian
		fs_code = TS_LITTLE_ENDIAN;
		setEndian(LITTLE_ENDIAN);
		header_included = true;
	}

	// if a header is present read the remainder
	UINT16 header_version = 0;
	if (header_included)
	{
		// parse the header
		UINT16 tag;
		UINT32 length;
		UINT16 data_vr = 0;
		UINT16 data_bits = 0;
		BYTE   transfer_syntax[UI_LENGTH + 1];
		UINT32 data_size = 0;

		// save file transfer syntax code
		// - this will be over written from header version 3
		fs_codeM = fs_code;

		do 
		{
			// read the tag and length
			(*this) >> tag;
			(*this) >> length;

			switch(tag)
			{
			case VERSION_TAG:
				// check length and value
				if (length == sizeof(header_version))
				{
					// read version tag
					(*this) >> header_version;
					if ((header_version != OV_HEADER_VERSION1) &&
						(header_version != OV_HEADER_VERSION2) &&
						(header_version != OV_HEADER_VERSION3))
					{
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header version %d not supported - expected %d (VTS) or %d, %d (DVT)", header_version, OV_HEADER_VERSION1, OV_HEADER_VERSION2, OV_HEADER_VERSION3);
						}

						result = false;
					}
				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header version length of %d not supported - expected %d", length, sizeof(header_version));
					}

					result = false;
				}
				break;

			case OV_VR_TAG:
				// version 2 tag
				// check length
				if ((header_version > OV_HEADER_VERSION1) &&
					(length == sizeof(data_vr)))
				{
					// read data vr tag
					(*this) >> data_vr;

                    switch(data_vr)
                	{
                    case OV_VR_OB: file_vrM = ATTR_VR_OB; break;
                    case OV_VR_OF: file_vrM = ATTR_VR_OF; break;
                    case OV_VR_OW: file_vrM = ATTR_VR_OW; break;
	                default: file_vrM = GetVR(); break;
	                }

				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header version %d - image VR length of %d not supported - expected %d", header_version, length, sizeof(data_vr));
					}
	
					result = false;
				}
				break;

			case OV_BITS_ALLOCATED_TAG:
				// version 2 tag
				// check length
				if ((header_version > OV_HEADER_VERSION1) &&
					(length == sizeof(data_bits)))
				{
					// read data bits tag
					(*this) >> data_bits;
					bits_allocatedM = data_bits;
				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header version %d - bits allocated length of %d not supported - expected %d", header_version, length, sizeof(data_bits));
					}

					result = false;
				}
				break;

			case OV_TRANSFER_SYNTAX_TAG:
				// version 3 tag
				// check length
				if ((header_version > OV_HEADER_VERSION2) &&
					(length == UI_LENGTH + 1))
				{
					// read transfer syntax tag
					if (readBinary(transfer_syntax, UI_LENGTH + 1) == UI_LENGTH + 1)
					{
						UID_CLASS file_transfer_syntax;
						file_transfer_syntax.set(transfer_syntax, UI_LENGTH + 1);
						SetFileTransferSyntax(file_transfer_syntax);
					}
					else
					{
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header version %d - failed to read file transfer syntax", header_version);
						}

						result = false;
					}
				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header version %d - transfer syntax length of %d not supported - expected %d", header_version, length, UI_LENGTH);
					}

					result = false;
				}
				break;

			case OV_SIZE_TAG:
				// version 1 tag
				// check length
				if (length == sizeof(data_size))
				{
					// save value as data length
					(*this) >> data_size;
					lengthM = data_size;

					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Export other data length set from Other Data file context: %d", lengthM);
					}
				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header image size length of %d not supported - expected %d", length, sizeof(data_size));
					}

					result = false;
				}
				break;

			default:
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Other Data file - header tag of %X not supported in version %d", tag, OV_HEADER_VERSION3);
				}

				result = false;
				break;
			}
		} 
		while (tag != OV_SIZE_TAG);

		// check if the image vr and image bits are defined
		if ((header_version > OV_HEADER_VERSION1) &&
			(data_vr) &&
			(data_bits))
		{
			// overrule any settings made based on the filename
			// - file may have been copied
			switch(GetVR())
			{
			case ATTR_VR_OB:
				if ((data_vr != OV_VR_OB) ||
					((data_bits != 8) &&
					 (data_bits != 16)))
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_WARNING, 1, "OB Data file - invalid format - being used but may produce unexpected results");
					}
				}
				break;
			case ATTR_VR_OF:
				if ((data_vr != OV_VR_OF) ||
					(data_bits != 32))
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_WARNING, 1, "OF Data file - invalid format - being used but may produce unexpected results");
					}
				}
				break;
			case ATTR_VR_OW:
				if (((data_vr != OV_VR_OB) &&
					(data_vr != OV_VR_OW)) ||
					((data_bits != 8) &&
					(data_bits != 16)))
				{
					if (loggerM_ptr)
					{
						//loggerM_ptr->text(LOG_WARNING, 1, "OW Data file - invalid format - being used but may produce unexpected results");
					}
				}
				break;
			default: break;
			}
		}

		// save the file read position
		file_offsetM = getOffset();
	}
	else
	{
		// just get the length of the header
		lengthM = getLength();

		// save the file read position
		file_offsetM = 0;
		setOffset(file_offsetM);		
	}

	/*
    // display DEBUG information about the PIX file
    if (loggerM_ptr)
    {
        loggerM_ptr->text(LOG_DEBUG, 1, "Other data PIX file \"%s\" - description:", filenameM.c_str());
	    if (header_included)
        {
            loggerM_ptr->text(LOG_DEBUG, 1, "File contains a DVT header - version: %d", header_version);
        }
        else
        {
			loggerM_ptr->text(LOG_DEBUG, 1, "File does not contain a DVT header - following parameters may not be correct");
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
        loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax UID: \"%s\"", (char*)file_transfer_syntaxM.get());
        switch(file_vrM)
        {
        case ATTR_VR_OB: loggerM_ptr->text(LOG_DEBUG, 1, "File VR: OB"); break;
        case ATTR_VR_OF: loggerM_ptr->text(LOG_DEBUG, 1, "File VR: OF"); break;
        case ATTR_VR_OW: loggerM_ptr->text(LOG_DEBUG, 1, "File VR: OW"); break;
        }
        loggerM_ptr->text(LOG_DEBUG, 1, "File Bits Allocated: %d", bits_allocatedM);
        loggerM_ptr->text(LOG_DEBUG, 1, "File Pixel Data Length: %d", lengthM);
        loggerM_ptr->text(LOG_DEBUG, 1, "File Pixel Data Offset: %d", file_offsetM);
    }
*/
	// return result
	return result;
}

//>>===========================================================================

void OTHER_VALUE_STREAM_CLASS::DisplayHeaderDetail()

//  DESCRIPTION     : Display the PIX file header detail.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // display DEBUG information about the PIX file
    if (loggerM_ptr)
    {
        loggerM_ptr->text(LOG_DEBUG, 1, "Other data PIX file \"%s\" - description:", filenameM.c_str());

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
        loggerM_ptr->text(LOG_DEBUG, 1, "File Transfer Syntax UID: \"%s\"", (char*)file_transfer_syntaxM.get());
        switch(file_vrM)
        {
        case ATTR_VR_OB: loggerM_ptr->text(LOG_DEBUG, 1, "File VR: OB"); break;
        case ATTR_VR_OF: loggerM_ptr->text(LOG_DEBUG, 1, "File VR: OF"); break;
        case ATTR_VR_OW: loggerM_ptr->text(LOG_DEBUG, 1, "File VR: OW"); break;
        }
        loggerM_ptr->text(LOG_DEBUG, 1, "File Bits Allocated: %d", bits_allocatedM);
        loggerM_ptr->text(LOG_DEBUG, 1, "File Pixel Data Length: %d", lengthM);
        loggerM_ptr->text(LOG_DEBUG, 1, "File Pixel Data Offset: %d", file_offsetM);
    }
}

//>>===========================================================================

bool OTHER_VALUE_STREAM_CLASS::WriteFileHeader()

//  DESCRIPTION     : Write the file header.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to open the file
	if (!isOpen()) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Other Value data - Cannot open file for writing data: %s", filenameM.c_str());
		}
		return false;
	}

	// set the file endian
	setEndian((fs_codeM & TS_BIG_ENDIAN) ? BIG_ENDIAN : LITTLE_ENDIAN);
		
	// write the magic number = 42
	UINT16 endian = 0x002A;
	(*this) << endian;

	// write the version tag
	UINT16 tag = VERSION_TAG;
	UINT16 value16 = OV_HEADER_VERSION3;
	UINT32 length = sizeof(value16);
	(*this) << tag;
	(*this) << length;
	(*this) << value16;

	// write the data vr
	tag = OV_VR_TAG;
	switch(GetVR())
	{
	case ATTR_VR_OB: value16 = OV_VR_OB; break;
	case ATTR_VR_OF: value16 = OV_VR_OF; break;
	case ATTR_VR_OW: value16 = OV_VR_OW; break;
	default: value16 = 0; break;
	}
	length = sizeof(value16);
	(*this) << tag;
	(*this) << length;
	(*this) << value16;

	// write the data bits tag
	tag = OV_BITS_ALLOCATED_TAG;
	value16 = (INT16) bits_allocatedM;
	length = sizeof(value16);
	(*this) << tag;
	(*this) << length;
	(*this) << value16;

	// write the transfer syntax tag
	tag = OV_TRANSFER_SYNTAX_TAG;
	BYTE transfer_syntax[UI_LENGTH + 1];
	byteZero(transfer_syntax, UI_LENGTH + 1);
	byteCopy(transfer_syntax, file_transfer_syntaxM.get(), file_transfer_syntaxM.getLength());
	length = UI_LENGTH + 1;
	(*this) << tag;
	(*this) << length;
	writeBinary(transfer_syntax, UI_LENGTH + 1);

	// write the length tag
	tag = OV_SIZE_TAG;
	UINT32 value32 = (UINT32)lengthM;
	length = sizeof(value32);
	(*this) << tag;
	(*this) << length;
			
	// save the length offset - we may need to re-write the value later
	length_offsetM = getOffset();
	(*this) << value32;

	// return result
	return true;
}
