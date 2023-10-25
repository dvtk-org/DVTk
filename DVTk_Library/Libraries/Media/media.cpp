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
//  DESCRIPTION     :	File Meta Information class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "media.h"
#include "Inetwork.h"			// Network component interface


//>>===========================================================================

MEDIA_FILE_HEADER_CLASS::MEDIA_FILE_HEADER_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	defineGroupLengthsM = true;

	// set up file preamble and prefix
	preambleValueM = FMI_PREAMBLE_VALUE;
	byteFill(preambleM, preambleValueM, FMI_PREAMBLE_LENGTH);
	byteZero(prefixM, sizeof(prefixM));
	byteCopy(prefixM, (BYTE*) FMI_PREFIX_VALUE, FMI_PREFIX_LENGTH);
	transferSyntaxUidM = EXPLICIT_VR_LITTLE_ENDIAN;
	filenameM = "";
	fileTfM_ptr = NULL;
}


//>>===========================================================================

MEDIA_FILE_HEADER_CLASS::MEDIA_FILE_HEADER_CLASS(int sessionId, string sopClassUid, string sopInstanceUid, string transferSyntaxUid, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	defineGroupLengthsM = true;

	// set up file preamble and prefix
	preambleValueM = FMI_PREAMBLE_VALUE;
	byteFill(preambleM, preambleValueM, FMI_PREAMBLE_LENGTH);
	byteZero(prefixM, sizeof(prefixM));
	byteCopy(prefixM, (BYTE*) FMI_PREFIX_VALUE, FMI_PREFIX_LENGTH);
	transferSyntaxUidM = transferSyntaxUid;
	loggerM_ptr = logger_ptr;

	string storageRoot;
	if (loggerM_ptr)
	{
		// get the storage root
		storageRoot = loggerM_ptr->getStorageRoot();
	}

	// get a filename for the media file
	getStorageFilename(storageRoot, sessionId, filenameM, SFE_DOT_DCM);

	fileTfM_ptr = new FILE_TF_CLASS(filenameM, "wb");
	fileTfM_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);
	fileTfM_ptr->setLogger(loggerM_ptr);

	// save the media header parameters
	(void) setOBValue(TAG_FILE_META_INFORMATION_VERSION, 0x0001, 0x0002);
	(void) setUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, sopClassUid);
	(void) setUIValue(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID, sopInstanceUid);
	(void) setUIValue(TAG_TRANSFER_SYNTAX_UID, transferSyntaxUid);

	// set  implementation class uid
	(void) setUIValue(TAG_IMPLEMENTATION_CLASS_UID, IMPLEMENTATION_CLASS_UID);
}


//>>===========================================================================

MEDIA_FILE_HEADER_CLASS::MEDIA_FILE_HEADER_CLASS(string filename,  bool openForReading)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// set up file preamble and prefix
	preambleValueM = FMI_PREAMBLE_VALUE;
	transferSyntaxUidM = EXPLICIT_VR_LITTLE_ENDIAN;

	// save the media storage filename
	filenameM = filename;

	// open the file for reading
    if (openForReading)
    {
        // open the file for reading
    	fileTfM_ptr = new FILE_TF_CLASS(filenameM, "rb");
    }
    else
    {
        // open the file for writing
    	fileTfM_ptr = new FILE_TF_CLASS(filenameM, "wb");
    }
	fileTfM_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);
	fileTfM_ptr->setLogger(loggerM_ptr);
}


//>>===========================================================================

MEDIA_FILE_HEADER_CLASS::~MEDIA_FILE_HEADER_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// delete the file transfer class
	if (fileTfM_ptr)
	{
		delete fileTfM_ptr;
	}

	fileTfM_ptr = NULL;
}


//>>===========================================================================

void MEDIA_FILE_HEADER_CLASS::setFilename(string filename)

//  DESCRIPTION     : Method to set the filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
     filenameM = filename;
}


//>>===========================================================================

void MEDIA_FILE_HEADER_CLASS::setFileTFpointer(FILE_TF_CLASS* fileTf_ptr)

//  DESCRIPTION     : Method to set the file handle.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
     fileTfM_ptr = fileTf_ptr;
}


//>>===========================================================================

void MEDIA_FILE_HEADER_CLASS::setPreambleValue(BYTE value)

//  DESCRIPTION     : Method to set the FMI Preamble.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// set up the preamble
	preambleValueM = value;
	byteFill(preambleM, preambleValueM, FMI_PREAMBLE_LENGTH);
}

//>>===========================================================================

const BYTE* MEDIA_FILE_HEADER_CLASS::getPreamble()

//  DESCRIPTION     : Method to get the FMI Preamble.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return preambleM;
}

//>>===========================================================================

void MEDIA_FILE_HEADER_CLASS::setPrefix(char *prefix_ptr)

//  DESCRIPTION     : Method to set the FMI Dicom Prefix.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// set up the Dicom prefix
	byteZero(prefixM, sizeof(prefixM));
	UINT length = byteStrLen((BYTE*) prefix_ptr);
	length = (length > FMI_PREFIX_LENGTH) ? FMI_PREFIX_LENGTH : length;
	byteCopy(prefixM, (BYTE*) prefix_ptr, length);
}

//>>===========================================================================

const BYTE* MEDIA_FILE_HEADER_CLASS::getPrefix()

//  DESCRIPTION     : Method to get the FMI Dicom Prefix.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return prefixM;
}

//>>===========================================================================

void MEDIA_FILE_HEADER_CLASS::setTransferSyntaxUid(string transferSyntaxUid)

//  DESCRIPTION     : Method to set the FMI Transfer Syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// set up the transfer syntax uid
	transferSyntaxUidM = transferSyntaxUid;
}

//>>===========================================================================

const char* MEDIA_FILE_HEADER_CLASS::getTransferSyntaxUid()

//  DESCRIPTION     : Method to get the FMI Transfer Syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return transferSyntaxUidM.c_str();
}

//>>===========================================================================

const char* MEDIA_FILE_HEADER_CLASS::getFilename()

//  DESCRIPTION     : Method to get the Filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{ 
    return filenameM.c_str(); 
}

//>>===========================================================================

DCM_ATTRIBUTE_GROUP_CLASS *MEDIA_FILE_HEADER_CLASS::read()

//  DESCRIPTION     : Method to stream the media file header and dataset
//					  from the given file buffer. The file preamble and DICOM
//					  prefix are read too.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check the file is open
	if (!fileTfM_ptr->isOpen()) return NULL;

	// read file preamble and DICOM prefix from file
	INT count;
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Reading %d byte FMI Preamble", FMI_PREAMBLE_LENGTH);
	}

	if ((count = fileTfM_ptr->readBinary(preambleM, FMI_PREAMBLE_LENGTH)) != FMI_PREAMBLE_LENGTH)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Tried to read FMI Preamble of %d bytes from file: %s - could only read %d bytes", FMI_PREAMBLE_LENGTH, filenameM.c_str(), count);
		}
		return NULL;
	}

	if (loggerM_ptr)
	{
        loggerM_ptr->text(LOG_DEBUG, 1, "Reading %d byte FMI Prefix. Expect to find \"DICM\"", FMI_PREFIX_LENGTH);
	}

	if ((count = fileTfM_ptr->readBinary(prefixM, FMI_PREFIX_LENGTH)) != FMI_PREFIX_LENGTH)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Tried to read FMI Prefix of %d bytes from file: %s - could only read %d bytes", FMI_PREFIX_LENGTH, filenameM.c_str(), count);
		}
		return NULL;
	}

	// check that we have a media file
	if (!byteCompare(prefixM, (BYTE*) FMI_PREFIX_VALUE, FMI_PREFIX_LENGTH))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_WARNING, 1, "File: \"%s\" is not a valid DICOM Media Storage File. No \"DICM\" FMI Prefix at offset 128.", filenameM.c_str());
		}
		return NULL;
	}

	// decode file meta information
	if (!decode(*fileTfM_ptr, GROUP_FOUR))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Cannot decode File Meta Information in: %s", filenameM.c_str());
		}
		return NULL;
	}

	// get the transfer syntax from the FMI
	string transferSyntaxUid = IMPLICIT_VR_LITTLE_ENDIAN;
	if (!getUIValue(TAG_TRANSFER_SYNTAX_UID, transferSyntaxUid))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Cannot get Transfer Syntax UID (0002,0010) from File Meta Information in: %s", filenameM.c_str());
		}
		return NULL;
	}

	// set the transfer syntax code to use for decode
	fileTfM_ptr->setTsCode(transferSyntaxUidToCode(transferSyntaxUid), transferSyntaxUid);

	// instantiate a new dataset object
	DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

	// set the UN VR definition look-up flag
	dataset_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	// set the EnsureEvenAttributeValueLength flag
	dataset_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

	// decode the dataset
	if (!dataset_ptr->decode(*fileTfM_ptr))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Cannot decode Dataset in: %s", filenameM.c_str());
		}
		delete dataset_ptr;
		return NULL;
	}

	// return dataset
	return dataset_ptr;
}


//>>===========================================================================

bool MEDIA_FILE_HEADER_CLASS::read(FILE_TF_CLASS* fileTf_ptr)

//  DESCRIPTION     : Method to stream the media file header from the given file
//                    buffer. This includes the file preamble, the DICOM
//					  prefix and the file meta elements.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check the file is open
	if (!fileTf_ptr->isOpen()) return false;

	// read file preamble and DICOM prefix from file
	INT count;
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Reading %d byte FMI Preamble", FMI_PREAMBLE_LENGTH);
	}

	if ((count = fileTf_ptr->readBinary(preambleM, FMI_PREAMBLE_LENGTH)) != FMI_PREAMBLE_LENGTH)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Tried to read FMI Preamble of %d bytes from file: %s - could only read %d bytes", FMI_PREAMBLE_LENGTH, filenameM.c_str(), count);
		}
		return false;
	}

	if (loggerM_ptr)
	{
        loggerM_ptr->text(LOG_DEBUG, 1, "Reading %d byte FMI Prefix. Expect to find \"DICM\"", FMI_PREFIX_LENGTH);
	}

	if ((count = fileTf_ptr->readBinary(prefixM, FMI_PREFIX_LENGTH)) != FMI_PREFIX_LENGTH)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Tried to read FMI Prefix of %d bytes from file: %s - could only read %d bytes", FMI_PREFIX_LENGTH, filenameM.c_str(), count);
		}
		return false;
	}

	// check that we have a media file
	if (!byteCompare(prefixM, (BYTE*) FMI_PREFIX_VALUE, FMI_PREFIX_LENGTH))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_WARNING, 1, "File: \"%s\" is not a valid DICOM Media Storage File. No \"DICM\" FMI Prefix at offset 128.", filenameM.c_str());
		}
		return false;
	}

	// decode file meta information
	if (!decode(*fileTf_ptr, GROUP_FOUR))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Cannot decode File Meta Information in: %s", filenameM.c_str());
		}
		return false;
	}

	// get the transfer syntax from the FMI
	string transferSyntaxUid = IMPLICIT_VR_LITTLE_ENDIAN;
	if (!getUIValue(TAG_TRANSFER_SYNTAX_UID, transferSyntaxUid))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Cannot get Transfer Syntax UID (0002,0010) from File Meta Information in: %s", filenameM.c_str());
		}
	}
	else
	{
		setTransferSyntaxUid(transferSyntaxUid);
	}

	return true;
}


//>>===========================================================================

bool MEDIA_FILE_HEADER_CLASS::encode(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : Encode media header to dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check if we need to add the group length attributes
	if (defineGroupLengthsM)
	{
		// add group length(s) - attributes sorted here
		addGroupLengths();

		// update group length(s)
		setGroupLengths(dataTransfer.getTsCode());
	}
	else
	{
		// sort the media header attributes into ascending order
		SortAttributes();
	}

	// encode the media header attributes
	return DCM_ATTRIBUTE_GROUP_CLASS::encode(dataTransfer);
}


//>>===========================================================================

bool MEDIA_FILE_HEADER_CLASS::decode(DATA_TF_CLASS& dataTransfer, UINT16 lastGroup)

//  DESCRIPTION     : Decode media header from dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// decode the media header attributes
	return DCM_ATTRIBUTE_GROUP_CLASS::decode(dataTransfer, lastGroup);
}


//>>===========================================================================

bool MEDIA_FILE_HEADER_CLASS::write()

//  DESCRIPTION     : Method to stream the media file preamble, DICOM prefix and
//					  file meta information into the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check the file is open
	if (!fileTfM_ptr->isOpen()) return false;

	// write file preamble and DICOM prefix to file
	(void) fileTfM_ptr->writeBinary(preambleM, FMI_PREAMBLE_LENGTH);
	(void) fileTfM_ptr->writeBinary(prefixM, FMI_PREFIX_LENGTH);

	// encode file meta information
	return encode(*fileTfM_ptr);
}

//>>===========================================================================

bool MEDIA_FILE_HEADER_CLASS::write(FILE_TF_CLASS* fileTf_ptr)

//  DESCRIPTION     : Method to stream the media file preamble, DICOM prefix and
//					  file meta information into the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check the file is open
	if (!fileTf_ptr->isOpen()) return false;

	// write file preamble and DICOM prefix to file
	(void) fileTf_ptr->writeBinary(preambleM, FMI_PREAMBLE_LENGTH);
	(void) fileTf_ptr->writeBinary(prefixM, FMI_PREFIX_LENGTH);

	// encode file meta information
	return encode(*fileTf_ptr);
}

//>>===========================================================================

bool MEDIA_FILE_HEADER_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*)

//  DESCRIPTION     : Update this media header with the contents of the media header given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// return result
	return result;
}
