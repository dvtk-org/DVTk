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
//  DESCRIPTION     :	File PDU class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "file_pdu.h"

//>>===========================================================================

FILE_PDU_CLASS::FILE_PDU_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	fileM_ptr = NULL;
}

//>>===========================================================================

FILE_PDU_CLASS::FILE_PDU_CLASS(string filename)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	fileM_ptr = NULL;
	filenameM = filename;
}

//>>===========================================================================

FILE_PDU_CLASS::~FILE_PDU_CLASS()

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

bool FILE_PDU_CLASS::open(string mode)

//  DESCRIPTION     : Open the PDU file in the mode given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// open file
	fileM_ptr = NULL;
		
	// try to open the file
    if (filenameM.length() == 0) assert(false);
	fileM_ptr = fopen(filenameM.c_str(), mode.c_str());

	// return
	return (fileM_ptr) ? true : false;
}

//>>===========================================================================

void FILE_PDU_CLASS::close()

//  DESCRIPTION     : Close the PDU file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// close PDU file
	if (fileM_ptr) 
	{
		fclose(fileM_ptr);
		fileM_ptr = NULL;
	}
}

//>>===========================================================================

bool FILE_PDU_CLASS::write(const BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Write given data to PDU file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// write required data length to file
	return (fwrite((char*) data_ptr, 1, length, fileM_ptr) == length) ? true : false;
}

//>>===========================================================================

INT	FILE_PDU_CLASS::read(BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Read required data from PDU file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// read required data length from file
	size_t numread = fread((char*) data_ptr, 1, length, fileM_ptr);

	// return length read
	INT lengthRead = numread;
	return lengthRead;
}

//>>===========================================================================

UINT FILE_PDU_CLASS::getLength()

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

UINT FILE_PDU_CLASS::getOffset()

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

FILE_STREAM_CLASS::FILE_STREAM_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	logLengthM = PDU_LOGLENGTH;
	loggerM_ptr = NULL;
	filePDUIndexM = 0;
}

//>>===========================================================================

FILE_STREAM_CLASS::~FILE_STREAM_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	// - clean up file pdus
	while (filePduM.getSize())
	{
		filePduM.removeAt(0);
	}
}

//>>===========================================================================

bool FILE_STREAM_CLASS::writeBinary(const BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Write given data to file stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// check for valid data to write
	if (data_ptr == NULL) return false;

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
            serializer_ptr->SerializeBytes((BYTE*)data_ptr, bytesToLog, "File PDU Data Written");
        }
    }

	// write data to file pdu
	bool written = false;

	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_DEBUG, 2, "FILE STREAM - FILE_PDU_CLASS::write(%d)", length);
    }

	// set up the file transfer
	FILE_PDU_CLASS *filePdu_ptr = new FILE_PDU_CLASS();

	// check the file is open
	if (filePdu_ptr->open("ab")) 
	{
		written = filePdu_ptr->write(data_ptr ,length );
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't write Data to \"%s\"", (filePdu_ptr->getFilename()).c_str());
			loggerM_ptr->text(LOG_NONE, 1, "Check directory path exists.");
		}

		delete filePdu_ptr;
		return false;
	}

	if(written)
	{
		filePduM.add(*filePdu_ptr);
		
		// clean up the file transfer
		delete filePdu_ptr;
	}

	return written;
}

//>>===========================================================================

INT	FILE_STREAM_CLASS::readBinary(BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Read required data from file. The data should be read from the
//					: underlying files (in the file stream) as if it was one continuous
//					: stream - that is, same as coming from a socket stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// read required data length from file stream
	UINT lengthRead = 0;

	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_DEBUG, 2, "FILE STREAM - FILE_PDU_CLASS::read(%d)", length);
    }

	while(lengthRead < length)
	{
		if(!filePduM[filePDUIndexM].isOpen())
		{
			filePduM[filePDUIndexM].open("rb");
		}

		INT read_bytes = filePduM[filePDUIndexM].read((data_ptr + lengthRead),(length - lengthRead));

		if (read_bytes > 0)
		{
			// Get the current file offset and PDU file length
			UINT current = filePduM[filePDUIndexM].getOffset();
			UINT fileLength = filePduM[filePDUIndexM].getLength();

			// If current PDU is fully read close the PDU file and move to next file
			if(current == fileLength)
			{
				filePduM[filePDUIndexM].close();
				filePDUIndexM++;
			}

			// read some data
			lengthRead += read_bytes;
			if(lengthRead == length)
			{
				break;
			}
		}
		else
		{
			// File reading error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "File read error(read returned %d)", read_bytes);
			}
			return -1;
		}
	}
	
	// check that the RAW dump is enabled
	if ((loggerM_ptr) &&
        (loggerM_ptr->getLogMask() & LOG_PDU_BYTES))
    {
        // serialize the PDU header
        BASE_SERIALIZER *serializer_ptr = loggerM_ptr->getSerializer();
        if (serializer_ptr)
        {
    		UINT bytesToLog = lengthRead;

    		// check how many bytes to log
	    	if (lengthRead > (INT)logLengthM) 
		    {
			    loggerM_ptr->text(LOG_NONE, 1, "Showing first 0x%X=%d bytes...", logLengthM, logLengthM);
			    bytesToLog = logLengthM;
		    }

            // serialize the data
            serializer_ptr->SerializeBytes(data_ptr, bytesToLog, "File PDU Data Read");
        }
    }

	// return length read
	return lengthRead;
}