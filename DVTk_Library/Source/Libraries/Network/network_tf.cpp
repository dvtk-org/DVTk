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
//  DESCRIPTION     :	Network Data Transfer class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "network_tf.h"
#include "Ilog.h"				// Log interface component


//>>===========================================================================

NETWORK_TF_CLASS::NETWORK_TF_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sessionIdM = 1;
	pcIdM = 0;
	isCommandContentM = false;
	setEndian(LITTLE_ENDIAN);
	dataTransferIndexM = 0;
	dataTransferOffsetM = 0;
	maxRxLengthM = 0;
	maxTxLengthM = MAXIMUM_LENGTH_RECEIVED;
	loggerM_ptr = NULL;
}		

//>>===========================================================================

NETWORK_TF_CLASS::~NETWORK_TF_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
	cleanup();
}

//>>===========================================================================

void NETWORK_TF_CLASS::cleanup()

//  DESCRIPTION     : Cleanup resources.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
	while (dataTransferPduM.getSize())
	{
		delete dataTransferPduM[0];
		dataTransferPduM.removeAt(0);
	}

	// reset counters
	dataTransferIndexM = 0;
	dataTransferOffsetM = 0;

}

//>>===========================================================================

bool NETWORK_TF_CLASS::initialiseDecode(bool lookingForDataset)

//  DESCRIPTION     : Initialise decode from data transfer PDUs.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// reset counters
	dataTransferIndexM = 0;
	dataTransferOffsetM = 0;
	maxRxLengthM = 0;

	// move to first PDV in first PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
	if (dataTfPdu_ptr == NULL) return false;

	// set max received length to this value if larger than
	// any earlier value
	setMaxRxLength(dataTfPdu_ptr->getLength());

	// move to the first PDV in the next PDU
	dataTfPdu_ptr->moveToFirstPdv(loggerM_ptr);

	// get presentation context id and command/dataset flag
	pcIdM = dataTfPdu_ptr->getCurrentPresentationContextId();
	isCommandContentM = IsThisaCmdMsg(dataTfPdu_ptr->getCurrentMessageControlHeader());

	// check if we first have a command PDV - whilst we are looking for a dataset PDV
	if ((isCommandContentM) &&
		(lookingForDataset))
	{
		// move to next PDV - until we hit the first data
		while (isCommandContentM)
		{
			// move to the next PDV
			if (dataTfPdu_ptr->moveToNextPdv(loggerM_ptr))
			{
				// get presentation context id and command/dataset flag
				pcIdM = dataTfPdu_ptr->getCurrentPresentationContextId();
				isCommandContentM = IsThisaCmdMsg(dataTfPdu_ptr->getCurrentMessageControlHeader());
			}
			else
			{
				// something wrong we should hit the dataset PDV before the end of the PDU
				return false;
			}
		}
	}

	// return result
	return true;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::terminateDecode()

//  DESCRIPTION     : Terminate decode from data transfer PDUs.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the current PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
	if (dataTfPdu_ptr == NULL) return false;

	// check if there is some un-read PDV data in this PDU
	if (dataTfPdu_ptr->isThereMorePdvData())
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_WARNING, 1, "Command and Dataset PDV's found in single P-DATA-TF PDU");
		}
	}
	else
	{
		// all done
		// - clean up resources
		cleanup();
	}

	// return result
	return true;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::initialiseEncode()

//  DESCRIPTION     : Initialise encoding to data transfer PDUs.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// clean up resources
	cleanup();

	// allocate the first PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = new DATA_TF_PDU_CLASS(maxTxLengthM);
	addDataTransferPdu(dataTfPdu_ptr);

	// reset counters
	dataTransferIndexM = noDataTransferPdus() - 1;
	dataTransferOffsetM = 0;

	// return result
	return true;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::terminateEncode()

//  DESCRIPTION     : Terminate encoding to data transfer PDUs.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the current PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
	if (dataTfPdu_ptr == NULL) return false;

	// set the message control header - this is the last fragment
	BYTE messageControlHeader = DATASET_PDV | LAST_FRAGMENT;
	if (isCommandContentM)
	{
		messageControlHeader = COMMAND_PDV | LAST_FRAGMENT;
	}

	// update the content of the PDV header
	dataTfPdu_ptr->updateFirstPdv(dataTransferOffsetM, pcIdM, messageControlHeader);

	// update the actual PDU length now too - includes PDV header and data
	dataTfPdu_ptr->setLength(dataTfPdu_ptr->getCurrentPdvLength() + sizeof(UINT32) + sizeof(BYTE) + sizeof(BYTE));

	// return result
	return true;
}


//>>===========================================================================

UINT NETWORK_TF_CLASS::getRemainingLength()

//  DESCRIPTION     : Dummy function.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    return 0;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::rewind(UINT length)

//  DESCRIPTION     : Rewind the data transfer index by the length given.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if we can rewind in the current PDV
	if (dataTransferOffsetM >= length)
	{
		dataTransferOffsetM -= length;
	}
	else
	{
		// NOTE: The current implementation will work if we need to rewind to the
		// previous PDU - if we should just rewind to the previous PDV in the current
		// PDU then we will need to revisit this code again.

		// update length based on what is currently in this PDV
		length -= dataTransferOffsetM;

		// need to go back to the previous PDU
		if (dataTransferIndexM == 0) return false;
		dataTransferIndexM--;

		// get the current PDU
		DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
		if (dataTfPdu_ptr == NULL) return false;
	
		// now move to the last PDV in this PDU
		dataTfPdu_ptr->moveToLastPdv();

		// update the offset back into the PDV
		dataTransferOffsetM = dataTfPdu_ptr->getCurrentPdvLength() - length;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "NETWORK_TF_CLASS::rewind() - trying to rewind PDVs inside PDUs.");
		}
	}

	// return result
	return true;
}

//>>===========================================================================

UINT NETWORK_TF_CLASS::getOffset()

//  DESCRIPTION     : Return the current dataTransfer offset.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return the current offset
	// this is OK for now - make sure index does not go negative - this should
	// be improved to allow wrap back to previous PDU/PDV
	return dataTransferOffsetM;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::setOffset(UINT offset)

//  DESCRIPTION     : Set the current dataTransfer offset.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set the current offset
	// this is OK for now - make sure index does not go negative - this should
	// be improved to allow wrap back to previous PDU/PDV
	dataTransferOffsetM = offset;

	// return result
	return true;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::serialise(string filename, bool openToAppend)

//  DESCRIPTION     : Serialise the data transfer PDVs to the given file.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	FILE *file_ptr;

    if (filename.length() == 0) return false;

	// check how the file should be opened
	if (openToAppend)
	{
		// open storage file in append write mode
		file_ptr = fopen(filename.c_str(), "ab");
	}
	else
	{
		// open storage file in normal write mode
		file_ptr = fopen(filename.c_str(), "wb");
	}
	if (file_ptr == NULL) return false;

	// loop through all PDU / PDVs serialising data
	if (!initialiseDecode(true))
	{
		// close the storage file
		fclose(file_ptr);

		return false;
	}

	// while data to serialize
	bool serialising = true;
	while (serialising)
	{		
		// serialise the current PDV
		DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
		if (dataTfPdu_ptr)
		{
			fwrite(dataTfPdu_ptr->getCurrentPdvData(), 1, dataTfPdu_ptr->getCurrentPdvLength(), file_ptr);

			// try to get more PDV data
			serialising = getMorePdvDataToRead(); 
		}
		else
		{
			serialising = false;
		}
	}

	// close the storage file
	fclose(file_ptr);

	// re-initialise the decoding
	return initialiseDecode(true);
}

//>>===========================================================================

bool NETWORK_TF_CLASS::isData()

//  DESCRIPTION     : Check if there is still data in data transfer PDUs.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool isData = true;

	// check if this is the last but one PDU as the last one may only contain an "empty" PDV
	if (dataTransferIndexM == noDataTransferPdus() - 2)
	{
		// get this PDU
		DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
		if (dataTfPdu_ptr == NULL) return false;

		// see if any data remains in this PDV
		UINT32 remainingPdvLength = dataTfPdu_ptr->getCurrentPdvLength() - dataTransferOffsetM;

		// when there is no remaning PDV data and this is the last PDV - then we are done
		if (remainingPdvLength == 0)
		{
			// get last PDU
			DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM + 1);
			if (dataTfPdu_ptr == NULL) return false;

			// this is the special case when the last PDV only contains the presentation context id and mch
			// - that is there is no data present
			if ((dataTfPdu_ptr->getCurrentPdvLength() == 0) &&
			(IsThisTheLastFragment(dataTfPdu_ptr->getCurrentMessageControlHeader())))
			{
				isData = false;
			}
		}
	}
	// check if we are processing the last PDU
	else if (dataTransferIndexM == noDataTransferPdus() - 1)
	{
		// get last PDU
		DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
		if (dataTfPdu_ptr == NULL) return false;

		// get maximum length remaining in current PDV
		UINT32 remainingPdvLength = dataTfPdu_ptr->getCurrentPdvLength() - dataTransferOffsetM;

		// when there is no remaning PDV data and this is the last PDV - then we are done
		if ((remainingPdvLength == 0) &&
			(IsThisTheLastFragment(dataTfPdu_ptr->getCurrentMessageControlHeader())))
		{
			isData = false;
		}
	}

	// return result
	return isData;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::writeBinary(const BYTE *buffer_ptr, UINT length)

//  DESCRIPTION     : Write data to underlying Data Transfer PDUs.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT totalLength = length;
	UINT lengthWritten = 0;

	// loop reading required amount of data - if possible
	while (lengthWritten < totalLength)
	{
		// get the current PDU
		DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
		if (dataTfPdu_ptr == NULL) return false;

		// get maximum length remaining in current PDV
		UINT32 remainingPdvLength = dataTfPdu_ptr->getCurrentPdvLength() - dataTransferOffsetM;

		// check if sufficient space in the current PDV to satisfy the write request
		if (length <= remainingPdvLength)
		{
			// sufficient data - copy amount requested
			byteCopy(dataTfPdu_ptr->getCurrentPdvData() + dataTransferOffsetM, (BYTE*) buffer_ptr + lengthWritten, length);

			// update offset by amount requested
			dataTransferOffsetM += length;

			// write complete
			return true;
		}
		else
		{
			// write remaining PDV data
			byteCopy(dataTfPdu_ptr->getCurrentPdvData() + dataTransferOffsetM, (BYTE*) buffer_ptr + lengthWritten, remainingPdvLength);

			// update offset by amount requested
			dataTransferOffsetM += remainingPdvLength;

			// update lengths
			lengthWritten += remainingPdvLength;
			length -= remainingPdvLength;

			// try to get more PDV space to write
			if (!getMorePdvSpaceToWrite()) 
			{
				// failed to get more space - return failure
				break;
			}
		}
	}

	// return failure
	return false;
}
		
//>>===========================================================================

INT	NETWORK_TF_CLASS::readBinary(BYTE *buffer_ptr, UINT length)

//  DESCRIPTION     : Read data from underlying Data Transfer PDUs.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT totalLength = length;
	UINT lengthRead = 0;

	// loop reading required amount of data - if possible
	while (lengthRead < totalLength)
	{
		// get the current PDU
		DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
		if (dataTfPdu_ptr == NULL) return -1;

		// get maximum length remaining in current PDV
		UINT32 remainingPdvLength = dataTfPdu_ptr->getCurrentPdvLength() - dataTransferOffsetM;

		// check if sufficient data in the current PDV to satisfy the read request
		if (length <= remainingPdvLength)
		{
			// sufficient data - copy amount requested
			byteCopy(buffer_ptr + lengthRead, dataTfPdu_ptr->getCurrentPdvData() + dataTransferOffsetM, length);

			// update offset by amount requested
			dataTransferOffsetM += length;

			// return length read
			lengthRead += length;
		}
		else
		{
			// read remaining PDV data
			byteCopy(buffer_ptr + lengthRead, dataTfPdu_ptr->getCurrentPdvData() + dataTransferOffsetM, remainingPdvLength);

			// update offset by amount requested
			dataTransferOffsetM += remainingPdvLength;

			// update lengths
			lengthRead += remainingPdvLength;
			length -= remainingPdvLength;

			// try to get more PDV data to read
			if (!getMorePdvDataToRead()) 
			{
				// failed to get more data - return the length read so far
				break;
			}
		}
	}

	// return length read
	return lengthRead;
}
		
//>>===========================================================================

bool NETWORK_TF_CLASS::getMorePdvDataToRead()

//  DESCRIPTION     : Move read indexes to next PDU / PDV containing data.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	
	// get the current PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
	if (dataTfPdu_ptr == NULL) return false;

	// try moving to the next PDV
	if (dataTfPdu_ptr->moveToNextPdv(loggerM_ptr))
	{
		// reset offset to index begining of next PDV
		dataTransferOffsetM = 0;
	}
	else
	{
		// current PDU exhausted - move to next PDU
		if (dataTransferIndexM < noDataTransferPdus() - 1)
		{
			// move to next PDU
			dataTransferIndexM++;

			// get address of the PDU
			dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);

			// set max received length to this value if larger than
			// any earlier value
			setMaxRxLength(dataTfPdu_ptr->getLength());

			// move to the first PDV in the next PDU
			dataTfPdu_ptr->moveToFirstPdv(loggerM_ptr);

			// reset offset to index begining of next PDV
			dataTransferOffsetM = 0;
		}
		else
		{
			// read request beyond the amount of PDU data available
			result = false;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::getMorePdvSpaceToWrite()

//  DESCRIPTION     : Set the current PDU length, etc and move write indexes to next 
//					  PDU / PDV containing data.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{	
	// get the current PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
	if (dataTfPdu_ptr == NULL) return false;

	// set the message control header - this is not the last fragment
	BYTE messageControlHeader = DATASET_PDV;
	if (isCommandContentM)
	{
		messageControlHeader = COMMAND_PDV;
	}

	// update the content of the PDV header
	dataTfPdu_ptr->updateFirstPdv(dataTransferOffsetM, pcIdM, messageControlHeader);

	// update the actual PDU length now too - includes PDV header and data
	dataTfPdu_ptr->setLength(dataTfPdu_ptr->getCurrentPdvLength() + sizeof(UINT32) + sizeof(BYTE) + sizeof(BYTE));

	// allocate the next PDU
	dataTfPdu_ptr = new DATA_TF_PDU_CLASS(maxTxLengthM);
	addDataTransferPdu(dataTfPdu_ptr);

	// reset counters
	dataTransferIndexM = noDataTransferPdus() - 1;
	dataTransferOffsetM = 0;

	// return result
	return true;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::isRemainingPdvDataInPdu(bool *isLast)

//  DESCRIPTION     : Check if there is still some PDV data in the PDU and
//					  return an indication if it is the last PDV or not.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool remainingPdvData = false;
	*isLast = false;

	// get the current PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);

	if (dataTfPdu_ptr)
	{
		// get maximum length remaining in current PDV
		if (dataTfPdu_ptr->getCurrentPdvLength() - dataTransferOffsetM)
		{
			remainingPdvData = true;
		}

		// get last indicator - directly from current PDV mch
		if (IsThisTheLastFragment(dataTfPdu_ptr->getCurrentMessageControlHeader()))
		{
			*isLast = true;
		}
	}

	// return remaining pdv data
	return remainingPdvData;
}

//>>===========================================================================

bool NETWORK_TF_CLASS::isTherePduData()

//  DESCRIPTION     : Check to see if any PDU data is avaiable.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  : 
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// move to next PDU
	DATA_TF_PDU_CLASS *dataTfPdu_ptr = getDataTransferPdu(dataTransferIndexM);
	if (dataTfPdu_ptr == NULL) return false;

	// pdu data available
	return true;
}