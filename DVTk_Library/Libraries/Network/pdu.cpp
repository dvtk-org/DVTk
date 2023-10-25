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
//  DESCRIPTION     :	Raw PDU class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "pdu.h"
#include "base_io.h"
#include "Ilog.h"			// Log component interface


//>>===========================================================================

PDU_CLASS::PDU_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	setEndian(BIG_ENDIAN);
	maxLengthM = 0;
	offsetM = 0;
	logLengthM = PDU_LOGLENGTH;
	pduTypeM = PDU_UNKNOWN;
	reservedM = 0;
	lengthM = 0;
	bodyM_ptr = NULL;
}		

//>>===========================================================================

PDU_CLASS::~PDU_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// free up resources
	if (bodyM_ptr) {
		delete [] bodyM_ptr;
	}
}
		
//>>===========================================================================

bool PDU_CLASS::allocateBody(UINT32 length)

//  DESCRIPTION     : Set the PDU maximum length and allocate the corresponding space for the PDU body.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// free up resources
	if (bodyM_ptr) {
		delete [] bodyM_ptr;
	}
	maxLengthM = 0;
	lengthM = 0;
	offsetM = 0;

	// allocate storage for the PDU body
	bodyM_ptr = new BYTE [length];
	if (bodyM_ptr)
	{
		maxLengthM = length;
		lengthM = length;
	}

	return (bodyM_ptr) ? true : false;
}

//>>===========================================================================

bool PDU_CLASS::setLength(UINT32 length)

//  DESCRIPTION     : Set the actual PDU length.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check that actual length is within maximum length already allocated
	if (length <= maxLengthM)
	{
		// update the actual length
		lengthM = length;
		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================
	
void PDU_CLASS::setMaxLengthToReceive(UINT32 length)

//  DESCRIPTION     : Set maximum allowed PDU length to be received.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      :
//  NOTES           : The length is the negotiated length during association
//<<===========================================================================
{
    maxLengthToReceiveM = length;
}

//>>===========================================================================
	
bool PDU_CLASS::write(BASE_IO_CLASS *io_ptr)

//  DESCRIPTION     : Write PDU to IO.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (io_ptr == NULL) return false;

	// PDU header is 6 bytes long
    BYTE header[6];

    // write the pdu header
    header[0] = pduTypeM;
    header[1] = reservedM;
    header[2] = (BYTE)((lengthM >> 24) & 0x000000FF);
    header[3] = (BYTE)((lengthM >> 16) & 0x000000FF);
    header[4] = (BYTE)((lengthM >> 8) & 0x000000FF);
    header[5] = (BYTE)(lengthM & 0x000000FF);

	UINT pduLength = lengthM + 6;
	BYTE* pduBuffer = new BYTE [pduLength];

	// copy the header
	byteCopy(pduBuffer, header, 6);

	// copy the body
	byteCopy(&pduBuffer[6], bodyM_ptr, lengthM);

	/*if (!(*io_ptr << pduTypeM)) return false;
	if (!(*io_ptr << reservedM)) return false;
	if (!(*io_ptr << lengthM)) return false;*/

	// write the pdu header
	//if (!(io_ptr->writeBinary(header, 6))) return false;

	// write the pdu 
	return io_ptr->writeBinary(pduBuffer, pduLength);
}

//>>===========================================================================

bool PDU_CLASS::readType(BASE_IO_CLASS *io_ptr)

//  DESCRIPTION     : Read PDU Type from IO.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (io_ptr == NULL) return false;

	// read the item type
	return (*io_ptr >> pduTypeM);
}

//>>===========================================================================

bool PDU_CLASS::readBody(BASE_IO_CLASS *io_ptr)

//  DESCRIPTION     : Read PDU Body from IO.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
    bool maxLenExceeded = false;
    UINT bytesToRead;

	if (io_ptr == NULL) return false;

	// read reserved field and length
	if (!(*io_ptr >> reservedM)) return false;
	if (!(*io_ptr >> lengthM)) return false;

    if (lengthM > REASONABLE_MAXIMUM_LENGTH)
    {
        bodyM_ptr = new BYTE [UNREASONABLE_PDU_LOG_LENGTH];
        bytesToRead = UNREASONABLE_PDU_LOG_LENGTH;
        maxLenExceeded = true;
    }
    else
    {
	    // allocate storage for the PDU body
	    bodyM_ptr = new BYTE [lengthM];
        bytesToRead = lengthM;
    }

	// read PDU body
	if (bodyM_ptr)
	{
		offsetM = 0;
		while (bytesToRead) 
		{
			// read as many bytes from IO as possible
			INT bytesRead = io_ptr->readBinary(&bodyM_ptr[offsetM], bytesToRead);

			if (bytesRead <= 0) 
			{
				// failed to read anything
				break;
			}

			// update counters
			offsetM += bytesRead;
			bytesToRead -= bytesRead;
		}

		// set result
		offsetM = 0;
		result = (bytesToRead == 0) ? true : false;
	}

    if (maxLenExceeded)
    {
        result = false;
    }

	return result;
}

//>>===========================================================================

INT	PDU_CLASS::readBinary(BYTE* data_ptr, UINT length)

//  DESCRIPTION     : Read requested length of data from PDU Body into given buffer.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	INT bytesRead = 0;

	// check that we have a PDU body
	if (bodyM_ptr) 
	{
		// fullfill the read request as far as possible
		UINT32 remainingBytes = lengthM - offsetM;
		bytesRead = (length < remainingBytes) ? length : remainingBytes;

		// copy the data
		byteCopy(data_ptr, &bodyM_ptr[offsetM], bytesRead);

		// update counters
		offsetM += bytesRead;
	}

	// return number of bytes actually read
	return bytesRead;
}

//>>===========================================================================

bool PDU_CLASS::writeBinary(const BYTE* data_ptr, UINT length)

//  DESCRIPTION     : Write requested length of data to PDU Body from given buffer.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check that we have a PDU body
	if (bodyM_ptr) 
	{
		// only write if the remaining PDU body length is large enough
		if (length <= lengthM - offsetM) 
		{
			// copy the data
			byteCopy(&bodyM_ptr[offsetM], (BYTE*) data_ptr, length);

			// update counters
			offsetM += length;
			result = true;
		}
	}

	return result;
}

//>>===========================================================================

void PDU_CLASS::logRaw(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log the Raw PDU data.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check logger is defined
	if (logger_ptr == NULL) return;

	// check that the RAW dump is enabled
	if (logger_ptr->getLogMask() & LOG_PDU_BYTES)
    {
        // serialize the PDU header
        BASE_SERIALIZER *serializer_ptr = logger_ptr->getSerializer();
        if (serializer_ptr)
        {
            // PDU header is 6 bytes long
            BYTE header[6];

            // copy the header
            header[0] = pduTypeM;
            header[1] = reservedM;
            header[2] = (BYTE)((lengthM >> 24) & 0x000000FF);
            header[3] = (BYTE)((lengthM >> 16) & 0x000000FF);
            header[4] = (BYTE)((lengthM >> 8) & 0x000000FF);
            header[5] = (BYTE)(lengthM & 0x000000FF);
            serializer_ptr->SerializeBytes(header, 6, "PDU Header");
        }

    	if (bodyM_ptr) 
	    {
            UINT bytesToLog = lengthM;

		    // check how many bytes to log
            if (lengthM > REASONABLE_MAXIMUM_LENGTH)
            {
			    logger_ptr->text(LOG_NONE, 1, "Showing first 0x%X=%d bytes...", UNREASONABLE_PDU_LOG_LENGTH, UNREASONABLE_PDU_LOG_LENGTH);
                bytesToLog = UNREASONABLE_PDU_LOG_LENGTH;
            }
            else if (lengthM > logLengthM) 
		    {
			        logger_ptr->text(LOG_NONE, 1, "Showing first 0x%X=%d bytes...", logLengthM, logLengthM);
			        bytesToLog = logLengthM;
            }

            // serialize the PDU body
            if (serializer_ptr)
            {
                serializer_ptr->SerializeBytes(bodyM_ptr, bytesToLog, "PDU Body");
            }
        }
	}
}


