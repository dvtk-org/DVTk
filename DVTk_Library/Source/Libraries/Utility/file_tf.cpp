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

//  	File Data Transfer class.

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "file_tf.h"
#include "Ilog.h"		// Log component interface

//>>===========================================================================

ITEM_OFFSET_CLASS::ITEM_OFFSET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	identifierM = "";
	offsetM = 0;
}


//>>===========================================================================

ITEM_OFFSET_CLASS::ITEM_OFFSET_CLASS(string identifier, UINT32 offset)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	identifierM = identifier;
	offsetM = offset;
}


//>>===========================================================================

FILE_TF_CLASS::FILE_TF_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	fileM_ptr = NULL;
	logLengthM = PDU_LOGLENGTH;
	loggerM_ptr = NULL;
}

//>>===========================================================================

FILE_TF_CLASS::FILE_TF_CLASS(string filename, string mode)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	fileM_ptr = NULL;
	if (filename != DATA_NOT_STORED)
	{
        if (filename.length() == 0) assert(false);
		// try to open the file
		fileM_ptr = fopen(filename.c_str(), mode.c_str());
	}
	logLengthM = PDU_LOGLENGTH;
	loggerM_ptr = NULL;
}

//>>===========================================================================

FILE_TF_CLASS::~FILE_TF_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// destructor activities
	close();
}

//>>===========================================================================

UINT32 FILE_TF_CLASS::getItemOffset(string identifier)

//  DESCRIPTION     : Search the array of item offsets for an entry
//					: with the given identifier.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	UINT32 offset = 0;

	// search through list for matching entry
	for (UINT i = 0; i < itemOffsetM.getSize(); i++)
	{
		// check if identifiers match
		if (itemOffsetM[i].getIdentifier() == identifier)
		{
			// match found - get the offset
			offset = itemOffsetM[i].getOffset();
		}
	}

	// return the offset - maybe zero
	return offset;
}

//>>===========================================================================

bool FILE_TF_CLASS::initialiseDecode(bool)

//  DESCRIPTION     : Initialise the data decode.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	return true;
}
//>>===========================================================================

bool FILE_TF_CLASS::terminateDecode()

//  DESCRIPTION     : Terminate the data decode.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return true;
}

//>>===========================================================================

bool FILE_TF_CLASS::initialiseEncode()

//  DESCRIPTION     : Initialise the data encoding.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return true;
}

//>>===========================================================================

bool FILE_TF_CLASS::terminateEncode()

//  DESCRIPTION     : Terminate the data encoding.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return true;
}

//>>===========================================================================

bool FILE_TF_CLASS::open(string filename, string mode)

//  DESCRIPTION     : Open the file transfer file in the mode given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// open file
	if (filename != DATA_NOT_STORED)
	{
		close();	// clear any old file pointers / handles

		// try to open the file
        if (filename.length() == 0) assert(false);
		fileM_ptr = fopen(filename.c_str(), mode.c_str());
	}
	logLengthM = PDU_LOGLENGTH;
	loggerM_ptr = NULL;

	// return
	return (NULL != fileM_ptr);
}

//>>===========================================================================

void FILE_TF_CLASS::close()

//  DESCRIPTION     : Close the file transfer file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// - clean up item offsets
	while (itemOffsetM.getSize())
	{
		itemOffsetM.removeAt(0);
	}

	// close media file
	if (fileM_ptr)
	{
		fclose(fileM_ptr);
		fileM_ptr = NULL;
	}
}

//>>===========================================================================

bool FILE_TF_CLASS::rewind(UINT length)

//  DESCRIPTION     : Rewind the file offset by the given length.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           : length of zero - means reset to beginning of file.
//<<===========================================================================
{
	// check file is open
	if (fileM_ptr) 
	{
		// get new file offset
		UINT current = 0;
		
		// length defined - compute current
		if (length)
		{
			current = ftell(fileM_ptr) - length;
		}

		// set new file offset
		(void) fseek(fileM_ptr, current, SEEK_SET);
	}

	// return result
	return true;
}

//>>===========================================================================

UINT FILE_TF_CLASS::getOffset()

//  DESCRIPTION     : Get the current file offset (position).
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get current file position
	UINT current = 0;

	// check file is open
	if (fileM_ptr) 
	{
		current = ftell(fileM_ptr);
	}

	// return current position
	return current;
}

//>>===========================================================================

bool FILE_TF_CLASS::setOffset(UINT offset)

//  DESCRIPTION     : Set the file offset (position).
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check file is open
	if (fileM_ptr) 
	{
		// set new file offset
		result = (fseek(fileM_ptr, offset, SEEK_SET) == 0) ? true : false;
	}

	// return result
	return result;
}

//>>===========================================================================

bool FILE_TF_CLASS::isData()

//  DESCRIPTION     : Check if there is any data remaining.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// get current file offset
	UINT current = ftell(fileM_ptr);
	UINT length = 0;

	if (!fseek(fileM_ptr, 0, SEEK_END)) 
	{
		// length is total minus current
		length = ftell(fileM_ptr);
		(void) fseek(fileM_ptr, current, SEEK_SET);
	}

	// check if length reached
	if (current == length) return false;

	// fallback - check for end of file
	return (feof(fileM_ptr)) ? false : true;
}

//>>===========================================================================

UINT FILE_TF_CLASS::getLength()

//  DESCRIPTION     : Return the current file length.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT length = 0;

	// check file is open
	if (fileM_ptr) 
	{
		// get current file offset
		UINT current = ftell(fileM_ptr);

		if (!fseek(fileM_ptr, 0, SEEK_END)) 
		{
			// get current file length
			length = ftell(fileM_ptr);
			(void) fseek(fileM_ptr, current, SEEK_SET);
		}
	}

	// return current length
	return length;
}


//>>===========================================================================

UINT FILE_TF_CLASS::getRemainingLength()

//  DESCRIPTION     : Compute remaining data length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	UINT	length = 0;

	// compute the remaining data length
	if (fileM_ptr) 
	{
		UINT current = ftell(fileM_ptr);

		if (!fseek(fileM_ptr, 0, SEEK_END)) 
		{
			// length is total minus current
			length = ftell(fileM_ptr) - current;
			(void) fseek(fileM_ptr, current, SEEK_SET);
		}
	}

	// return length
	return length;
}

//>>===========================================================================

bool FILE_TF_CLASS::writeBinary(const BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Write given data to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// check that the RAW dump is enabled
	if ((loggerM_ptr) &&
        (loggerM_ptr->getLogMask() & LOG_PDU_BYTES))
    {
        // serialize the PDU header
        BASE_SERIALIZER *serializer_ptr = loggerM_ptr->getSerializer();
        if (serializer_ptr)
        {
    		UINT bytesToLog = length;

    		// check how many bytes to log
	    	if (length > logLengthM) 
		    {
			    loggerM_ptr->text(LOG_NONE, 1, "Showing first 0x%X=%d bytes...", logLengthM, logLengthM);
			    bytesToLog = logLengthM;
		    }

            // serialize the data
            serializer_ptr->SerializeBytes((BYTE*)data_ptr, bytesToLog, "Media File Data Written");
        }
    }

	// write required data length to file
	return (fwrite((char*) data_ptr, 1, length, fileM_ptr) == length) ? true : false;
}

//>>===========================================================================

INT	FILE_TF_CLASS::readBinary(BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Read required data from file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// read required data length from file
	size_t numread = fread((char*) data_ptr, 1, length, fileM_ptr);

	// check that the RAW dump is enabled
	if ((loggerM_ptr) &&
        (loggerM_ptr->getLogMask() & LOG_PDU_BYTES))
    {
        // serialize the PDU header
        BASE_SERIALIZER *serializer_ptr = loggerM_ptr->getSerializer();
        if (serializer_ptr)
        {
    		UINT bytesToLog = length;

    		// check how many bytes to log
	    	if (length > logLengthM) 
		    {
			    loggerM_ptr->text(LOG_NONE, 1, "Showing first 0x%X=%d bytes...", logLengthM, logLengthM);
			    bytesToLog = logLengthM;
		    }

            // serialize the data
            serializer_ptr->SerializeBytes(data_ptr, bytesToLog, "Media File Data Read");
        }
    }

	// return length read
	INT lengthRead = numread;
	return lengthRead;
}


