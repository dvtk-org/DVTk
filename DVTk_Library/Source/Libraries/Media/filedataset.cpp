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
//  DESCRIPTION     :	File based DICOM Dataset class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "filedataset.h"
#include "filehead.h"
#include "media.h"
#include "Idefinition.h"		// Definition component interface
#include "Inetwork.h"			// Network component interface

//
// Attribute group map
//
static UINT16	TAttributeGroupMap[] =
{
	0x0004,
	0x0008,
	0x0010,
	0x0018,
	0x0020,
	0x0028,
	0x0032,
	0x0038,
	0x0040,
	0x0050,
	0x0054,
	0x0060,
	0x0088,
	0x2000,
	0x2010,
	0x2020,	// Note: Same value in Big and Little Endian!
	0x2030,
	0x2040,
	0x2050,
	0x2100,
	0x2110,
	0x2120,
	0x2130,
	0x3002,
	0x3004,
	0x3006,
	0x3008,
	0x300A,
	0x300C,
	0x300E,
	0x4000,
	0x4008,
	0x5000,	// Note: Repeating Group - 50xx (not supported)
	0x6000,	// Note: Repeating Group - 60xx (not supported)
	0x7FE0
};

//>>===========================================================================

bool isValidGroup(UINT16 group)

//  DESCRIPTION     : Function to check if the given Group Number is either
//					: private or matches the known ones - this is used to try
//					: to help determine the endianess of the attribute Tag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool validGroup = false;

	// Check for Odd group
	if ((group & 0x0001) != 0)
	{
		// Odd group - private - so seen as valid
		validGroup = true;
	}
	else
	{
		// Even group - see if one of those known
		for (int i = 0; i < (sizeof(TAttributeGroupMap)/sizeof(UINT16)); i++)
		{
			if (TAttributeGroupMap[i] == group)
			{
				validGroup = true;
				break;
			}
		}
	}
	
	return validGroup;
}

//>>===========================================================================

FILE_DATASET_CLASS::FILE_DATASET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	filenameM = "";
	fileContentTypeM = MFC_UNKNOWN;
	sopClassUidM = ""; 
	sopInstanceUidM = "";
	transferSyntaxM = "";
	storageModeM = SM_NO_STORAGE;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	addGroupLengthM = false;
    fileTfM_ptr = NULL;
	fmiM_ptr = NULL;
	datasetM_ptr = NULL;
    dicomdirDatasetM_ptr = NULL;
	loggerM_ptr = NULL;
}

//>>===========================================================================

FILE_DATASET_CLASS::FILE_DATASET_CLASS(string filename)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	filenameM = filename;
	fileContentTypeM = MFC_UNKNOWN;
	sopClassUidM = ""; 
	sopInstanceUidM = "";
	transferSyntaxM = "";
	storageModeM = SM_NO_STORAGE;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	addGroupLengthM = false;
    fileTfM_ptr = NULL;
	fmiM_ptr = NULL;
	datasetM_ptr = NULL;
    dicomdirDatasetM_ptr = NULL;
	loggerM_ptr = NULL;
}

//>>===========================================================================

FILE_DATASET_CLASS::FILE_DATASET_CLASS(string filename,
										MEDIA_FILE_CONTENT_TYPE_ENUM fileContentType, 
										string sopClassUid, 
										string sopInstanceUid, 
										string transferSyntaxUid)
//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	filenameM = filename;
	fileContentTypeM = fileContentType;
	sopClassUidM = sopClassUid; 
	sopInstanceUidM = sopInstanceUid; 
	transferSyntaxM = transferSyntaxUid;
	storageModeM = SM_NO_STORAGE;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	addGroupLengthM = false;
    fileTfM_ptr = NULL;
	fmiM_ptr = NULL;
	datasetM_ptr = NULL;
    dicomdirDatasetM_ptr = NULL;
	loggerM_ptr = NULL;
}

//>>===========================================================================

FILE_DATASET_CLASS::~FILE_DATASET_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// destructor activities
    if (fileTfM_ptr)
    {
        delete fileTfM_ptr;
    }
	if (fmiM_ptr)
	{
		delete fmiM_ptr;
	}
	if (datasetM_ptr)
	{
		delete datasetM_ptr;
	}
    if (dicomdirDatasetM_ptr)
	{
		delete dicomdirDatasetM_ptr;
	}
}

//>>===========================================================================

void FILE_DATASET_CLASS::setFilename(string filename)

//  DESCRIPTION     : Set the media filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    filenameM = filename; 
}

//>>===========================================================================

const char* FILE_DATASET_CLASS::getFilename()

//  DESCRIPTION     : Get the media filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    return filenameM.c_str(); 
}

//>>===========================================================================

void FILE_DATASET_CLASS::setTransferSyntax(string transferSyntax)

//  DESCRIPTION     : Set the media filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    transferSyntaxM = transferSyntax; 
}

//>>===========================================================================

string FILE_DATASET_CLASS::getTransferSyntax()

//  DESCRIPTION     : Get the media transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    return transferSyntaxM;
}

//>>===========================================================================

void FILE_DATASET_CLASS::setFileMetaInformation(MEDIA_FILE_HEADER_CLASS *fmi_ptr)

//  DESCRIPTION     : Set the File Meta Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    fmiM_ptr = fmi_ptr;
}

//>>===========================================================================

MEDIA_FILE_HEADER_CLASS* FILE_DATASET_CLASS::getFileMetaInformation()

//  DESCRIPTION     : Get the File Meta Information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    return fmiM_ptr; 
}

//>>===========================================================================

void FILE_DATASET_CLASS::setDataset(DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Set the Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    datasetM_ptr = dataset_ptr;
}

//>>===========================================================================

DCM_DATASET_CLASS* FILE_DATASET_CLASS::getDataset()

//  DESCRIPTION     : Get the Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    return datasetM_ptr; 
}

//>>===========================================================================

DCM_DIR_DATASET_CLASS* FILE_DATASET_CLASS::getDicomdirDataset()

//  DESCRIPTION     : Get the DICOMDIR read from the media file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    return dicomdirDatasetM_ptr; 
}

//>>===========================================================================

DCM_ITEM_CLASS* FILE_DATASET_CLASS::getNextDicomdirRecord()

//  DESCRIPTION     : Get the next DICOMDIR Record read from the media file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    DCM_ITEM_CLASS *item_ptr = NULL;

    // check if we have read the dicom dir
    if (dicomdirDatasetM_ptr)
    {
        // decode the next directory record
        item_ptr = dicomdirDatasetM_ptr->getNextDirRecord(*fileTfM_ptr);
    }

    // return the next directory record - returns NULL after all records have been read
    return item_ptr;
}

//>>===========================================================================

void FILE_DATASET_CLASS::clearFileMetaInformationPtr()

//  DESCRIPTION     : Clear the File Meta Information pointer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    fmiM_ptr = NULL; 
}

//>>===========================================================================

void FILE_DATASET_CLASS::clearDatasetPtr()

//  DESCRIPTION     : Clear the Dataset pointer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    datasetM_ptr = NULL; 
}

//>>===========================================================================

bool FILE_DATASET_CLASS::isFileMetaInformation(FILE_TF_CLASS *fileTf_ptr, string &transferSyntax)

//  DESCRIPTION     : Check if file starts with the meta information. If so read it and
//					  leave the file offset at the beginning of the actual dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	BYTE buffer[FMI_PREAMBLE_LENGTH + FMI_PREFIX_LENGTH];

	// check file is open
	if (!fileTf_ptr->isOpen())
	{
		// can't determine anything
		return false;
	}

	// set the storage mode
	fileTf_ptr->setStorageMode(storageModeM);

	// read the first few bytes of data from the head of the file - we are looking for the 'DICM' prefix
	if (fileTf_ptr->readBinary(buffer, FMI_PREAMBLE_LENGTH + FMI_PREFIX_LENGTH) == FMI_PREAMBLE_LENGTH + FMI_PREFIX_LENGTH)
	{
		// check for the DICM prefix
		if (!byteCompare(&buffer[FMI_PREAMBLE_LENGTH], (BYTE*) FMI_PREFIX_VALUE, FMI_PREFIX_LENGTH))
		{
			// rewind the file buffer
			fileTf_ptr->rewind(0);

			// can't find the preamble
			return false;
		}
	}

	// read the meta file information
	MEDIA_FILE_HEADER_CLASS fileMetaInformation;

	// cascade the logger
	fileMetaInformation.setLogger(loggerM_ptr);

	// set the UN VR definition look-up flag
	fileMetaInformation.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	// set the EnsureEvenAttributeValueLength flag
	fileMetaInformation.setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

	// set the transfer syntax code for reading the FMI
	fileTf_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);

	// decode the FMI
	if (!fileMetaInformation.decode(*fileTf_ptr, GROUP_FOUR))
	{
		// rewind the file buffer
		fileTf_ptr->rewind(0);

		// failed to decode the FMI
		return false;
	}

	// get the transfer syntax of the stored dataset
	if (!fileMetaInformation.getUIValue(TAG_TRANSFER_SYNTAX_UID, transferSyntax))
	{
		// rewind the file buffer
		fileTf_ptr->rewind(0);

		// failed to find the transfer syntax
		return false;
	}

	// set the transfer syntax - mainly for logging purposes
	fileMetaInformation.setTransferSyntaxUid(transferSyntax);

	// we have the detail we need to decode the stored dataset
	return true;
}

//>>===========================================================================

MEDIA_FILE_HEADER_CLASS *FILE_DATASET_CLASS::readFileMetaInformation(FILE_TF_CLASS* fileTf_ptr)

//  DESCRIPTION     : Check if file starts with the meta information. If so read it and
//					  leave the file offset at the beginning of the actual dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	MEDIA_FILE_HEADER_CLASS* fmi_ptr = new MEDIA_FILE_HEADER_CLASS();

    // set the fioename
    fmi_ptr->setFilename(filenameM);

	// cascade the logger
	fmi_ptr->setLogger(loggerM_ptr);

	// set the storage mode
	fileTf_ptr->setStorageMode(storageModeM);

	// read the meta information
	if (fmi_ptr->read(fileTf_ptr))
	{
		return fmi_ptr;
	}
	else
	{
        delete fmi_ptr;
	}
	
	return NULL;
}

//>>===========================================================================

bool FILE_DATASET_CLASS::deduceTransferSyntax(FILE_TF_CLASS *fileTf_ptr, string &transferSyntax)

//  DESCRIPTION     : Try to deduce the transfer syntax by interpretating the first
//					  data element found.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	BYTE buffer[8];
	TS_CODE tsCode;

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Trying to deduce Transfer Syntax directly from first few bytes of stored Dataset...");
	}

	// check file is open
	if (!fileTf_ptr->isOpen())
	{ 
		// can't determine anything
		return false;
	}
			
	// rewind the file buffer
	fileTf_ptr->rewind(0);

	// read the first few bytes of data from the beginning of the file
	if (fileTf_ptr->readBinary(buffer, 8) != 8)
	{
		// rewind the file buffer
		fileTf_ptr->rewind(0);

		// can't find the preamble
		return false;
	}

	// rewind the file buffer
	fileTf_ptr->rewind(0);

	// check if we can make any sense of the first few bytes - they should represent the tag [VR] and length
	// - get big endian values
	UINT16 bigGroup = (buffer[0] * 256) + buffer[1];
	UINT16 bigElement = (buffer[2] * 256) + buffer[3];

	// - get little endian values
	UINT16 littleGroup = (buffer[1] * 256) + buffer[0];
	UINT16 littleElement = (buffer[3] * 256) + buffer[2];

	// debug log the values read
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Big endian tag read (%04X,%04X)", bigGroup, bigElement);
		loggerM_ptr->text(LOG_DEBUG, 1, "Little endian tag read (%04X,%04X)", littleGroup, littleElement);
	}

	// try to get a known VR using the endian representations
	ATTR_VR_ENUM vrBig = DEFINITION->GetAttributeVr(bigGroup, bigElement);
	ATTR_VR_ENUM vrLittle = DEFINITION->GetAttributeVr(littleGroup, littleElement);

	// check if we have big endian
	if ((vrBig != ATTR_VR_UN) &&
		(vrLittle == ATTR_VR_UN))
	{
		// byte ordering is big endian
		tsCode = TS_BIG_ENDIAN;
	}
	else if ((vrBig == ATTR_VR_UN) &&
		(vrLittle != ATTR_VR_UN))
	{
		// byte ordering is little endian
		tsCode = TS_LITTLE_ENDIAN;
	}
	else if (isValidGroup(littleGroup) && 
		(littleElement == LENGTH_ELEMENT))
	{
		// special check for a Group Length - not available in definition files
		// byte ordering is little endian
		tsCode = TS_LITTLE_ENDIAN;
		vrLittle = ATTR_VR_UL;
	}
	else if (isValidGroup(bigGroup) && 
		(bigElement == LENGTH_ELEMENT))
	{
		// special check for a Group Length - not available in definition files
		// byte ordering is big endian
		tsCode = TS_BIG_ENDIAN;
		vrBig = ATTR_VR_UL;
	}	
	else if (((bigGroup & 0x0001) != 0) &&
			(bigElement >= 0x0010) &&
			(bigElement <= 0x00FF))
	{
		// special check for a Private Recognition Code - not available in definition files
		// byte ordering is big endian
		tsCode = TS_BIG_ENDIAN;
		vrBig = ATTR_VR_LO;
	}
	else if (((littleGroup & 0x0001) != 0) &&
			(littleElement >= 0x0010) &&
			(littleElement <= 0x00FF))
	{
		// special check for a Private Recognition Code - not available in definition files
		// byte ordering is little endian
		tsCode = TS_LITTLE_ENDIAN;
		vrLittle = ATTR_VR_LO;
	}
	else
	{
		// cannot determine endianess from VR - must stop
		return false;
	}

	// get VR from definition
	ATTR_VR_ENUM vr = vrLittle;
	if (tsCode == TS_BIG_ENDIAN)
	{
		vr = vrBig;
	}

	// now try to determine if we have an implicit or explicit VR
	// - get the VR of the attribute in 16 bit format
	UINT16 vr16 = (buffer[4] * 256) + buffer[5];

	// check if VR's match
	if (fileTf_ptr->vrToVr16(vr) == vr16)
	{
		// VR is explicit
		tsCode |= TS_EXPLICIT_VR;
	}
	else
	{
		// VR is implicit
		tsCode |= TS_IMPLICIT_VR;
	}

	switch(tsCode)
	{
	case TS_IMPLICIT_VR | TS_LITTLE_ENDIAN:
		transferSyntax = IMPLICIT_VR_LITTLE_ENDIAN;
			
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Detected Implicit VR Little Endian Transfer Syntax for File Dataset");
		}
		break;
	case TS_EXPLICIT_VR | TS_LITTLE_ENDIAN:
		transferSyntax = EXPLICIT_VR_LITTLE_ENDIAN;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Detected Explicit VR Little Endian Transfer Syntax for File Dataset");
		}
		break;
	case TS_EXPLICIT_VR | TS_BIG_ENDIAN:
		transferSyntax = EXPLICIT_VR_BIG_ENDIAN;

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Detected Explicit VR Big Endian Transfer Syntax for File Dataset");
		}
		break;
	default:
		break;
	}

	// return result
	return true;
}

//>>===========================================================================

bool FILE_DATASET_CLASS::read(DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Method to stream the DICOM object from the file using the defined
//					  transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	string transferSyntax = EXPLICIT_VR_LITTLE_ENDIAN;

	// check for valid dataset to read
	if (!dataset_ptr) return false;

	// set up the file transfer
	FILE_TF_CLASS *fileTf_ptr = new FILE_TF_CLASS(filenameM, "rb");

	// set the storage mode
	fileTf_ptr->setStorageMode(storageModeM);

	// cascade the logger
	fileTf_ptr->setLogger(loggerM_ptr);

	// check the file is open
	if (!fileTf_ptr->isOpen()) return false;

	// check if we have the File Meta Information first
	if (!isFileMetaInformation(fileTf_ptr, transferSyntax))
	{
		// try to deduce the transfer syntax from the file contents
		if (!deduceTransferSyntax(fileTf_ptr, transferSyntax))
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Can't deduce Transfer Syntax by reading Dataset from \"%s\". Will assume Explicit VR Little Endian...", filenameM.c_str());
			}
		}
	}

	// now get the tsCode from the transfer syntax
	TS_CODE tsCode = transferSyntaxUidToCode(transferSyntax);

	// set the required transfer syntax
	fileTf_ptr->setTsCode(tsCode, transferSyntax);
    transferSyntaxM = transferSyntax;

	// decode the dataset
    if (dataset_ptr->getLogger() == NULL)
    {
        // use this logger if none already set
        dataset_ptr->setLogger(loggerM_ptr);
    }

	// set the UN VR definition look-up flag
	dataset_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	// set the EnsureEvenAttributeValueLength flag
	dataset_ptr->setEnsureEvenAttributeValueLength(	ensureEvenAttributeValueLengthM);

	bool result = dataset_ptr->decode(*fileTf_ptr);

	// clean up the file transfer
	delete fileTf_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool FILE_DATASET_CLASS::read()

//  DESCRIPTION     : Method to stream the DICOM object from the file using the defined
//					  transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    string mediaStorageSopClassUid;
	string transferSyntax = EXPLICIT_VR_LITTLE_ENDIAN;
	TS_CODE tsCode = transferSyntaxUidToCode(transferSyntax);

	// set up the file transfer
	fileTfM_ptr = new FILE_TF_CLASS(filenameM, "rb");

	// set the storage mode
	fileTfM_ptr->setStorageMode(storageModeM);

	// cascade the logger
	fileTfM_ptr->setLogger(loggerM_ptr);

	// check the file is open
	if (!fileTfM_ptr->isOpen()) return false;

	// check if the file only contains a command or dataset
	if ((fileContentTypeM == MFC_COMMANDSET) ||
		(fileContentTypeM == MFC_DATASET))
	{
		// use the transfer syntax given in the class constructor
		transferSyntax = transferSyntaxM;

		// set the required transfer syntax
		fileTfM_ptr->setTsCode(tsCode, transferSyntax);
	}
	else
	{
		// set the required transfer syntax
		fileTfM_ptr->setTsCode(tsCode, transferSyntax);

		// check if we have the File Meta Information first
		fmiM_ptr = readFileMetaInformation(fileTfM_ptr);
		if (fmiM_ptr == NULL)
		{
			// try to deduce the transfer syntax from the file contents
			if (!deduceTransferSyntax(fileTfM_ptr, transferSyntax))
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Can't deduce Transfer Syntax by reading Dataset from \"%s\". Will assume Explicit VR Little Endian...", filenameM.c_str());
				}
			}
		}
		else
		{
			// extract the sop class uid from the fmi
			// - if the Media Storage SOP Class is not present - we can't tell if the media file is a DICOMDIR
			// - the wrong validation will be done if so - this will be picked up in the validation results.
			(void) fmiM_ptr->getUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, mediaStorageSopClassUid);

			// extract transfer syntax for dataset from fmi
			if (!fmiM_ptr->getUIValue(TAG_TRANSFER_SYNTAX_UID, transferSyntax))
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to retrieve Transfer Syntax from File Meta Information: \"%s\"", filenameM.c_str());
				return false;
			}
		}
	}

	// now get the tsCode from the transfer syntax
	tsCode = transferSyntaxUidToCode(transferSyntax);

	// set the required transfer syntax
	fileTfM_ptr->setTsCode(tsCode, transferSyntax);
    transferSyntaxM = transferSyntax;

    bool result = false;
    // check if we have a DICOMDIR stored
    if (mediaStorageSopClassUid == MEDIA_STORAGE_DIRECTORY_SOP_CLASS_UID)
    {
        // create dicom dir dataset object and decode the stored dataset
	    dicomdirDatasetM_ptr = new DCM_DIR_DATASET_CLASS();
	    dicomdirDatasetM_ptr->setLogger(loggerM_ptr);

		// set the UN VR definition look-up flag
		dicomdirDatasetM_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

		// set the EnsureEvenAttributeValueLength flag
		dicomdirDatasetM_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

        // decode up to the first directory record sequence
	    result = dicomdirDatasetM_ptr->decodeToFirstRecord(*fileTfM_ptr);
        if ((result == false) &&
            (loggerM_ptr))
	    {
		    loggerM_ptr->text(LOG_ERROR, 1, "Failed to decode the DICOMDIR in \"%s\"", filenameM.c_str()); 
        }
    }
    else
    {
  	    // create dataset object and decode the stored dataset
	    datasetM_ptr = new DCM_DATASET_CLASS();
	    datasetM_ptr->setLogger(loggerM_ptr);

		// set the UN VR definition look-up flag
		datasetM_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

		// set the EnsureEvenAttributeValueLength flag
		datasetM_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

        // decode the whole dataset
	    result = datasetM_ptr->decode(*fileTfM_ptr);
        if ((result == false) &&
            (loggerM_ptr))
	    {
		    loggerM_ptr->text(LOG_ERROR, 1, "Failed to decode the Dataset in \"%s\"", filenameM.c_str()); 
        }

	    // clean up the file transfer
	    delete fileTfM_ptr;
        fileTfM_ptr = NULL;
    }

	// return result
	return result;
}

//>>===========================================================================

bool FILE_DATASET_CLASS::write(DCM_DATASET_CLASS *dataset_ptr, bool autoCreateDirectory)

//  DESCRIPTION     : Method to stream the DICOM object to file using the defined
//					  transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = NULL;

	// check for valid dataset to write
	if (dataset_ptr == NULL) return false;

	// check the an absolute pathname has been given
	if (!isAbsolutePath(filenameM))
	{
		// see if a results root is defined
		if (loggerM_ptr)
		{
			// see if the path has been defined
			string pathname = loggerM_ptr->getStorageRoot();
			string filename = filenameM;

			if (pathname.length())
			{
				// set up filename by including path
                filenameM = pathname;
                if (pathname[pathname.length()-1] != '\\') filenameM += "\\";
                filenameM += filename;
			}
		}
	}

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "About to append Dataset to \"%s\"", filenameM.c_str()); 
	}

	if (autoCreateDirectory) //autocreate stuff should be here
	{
		createDirectory(filenameM);
	}

	// set up the file transfer
	FILE_TF_CLASS *fileTf_ptr = new FILE_TF_CLASS(filenameM, "ab");

	// cascade the logger
	fileTf_ptr->setLogger(dataset_ptr->getLogger());

	// check the file is open
	if (!fileTf_ptr->isOpen()) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't append Dataset to \"%s\"", filenameM.c_str());
			loggerM_ptr->text(LOG_NONE, 1, "Check directory path exists.");
		}

		delete fileTf_ptr;
		return false;
	}

	// to check if the dataset is the FMI - see if the attribute tags are from Group 0002
	if (dataset_ptr->containsAttributesFromGroup(GROUP_TWO))
	{
		// force the Group 0002 length
		dataset_ptr->setDefineGroupLengths(true);

		// set the required transfer syntax
		fileTf_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);
	}
	else
	{
		// try to retrive the file head in order to get the transfer syntax to use when
		// encoding this dataset
		if ((wid_ptr = WAREHOUSE->retrieve("", WID_FILEHEAD)) == NULL)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Can't find %s in Data Warehouse in order to get Transfer Syntax for Dataset", WIDName(WID_FILEHEAD));
				loggerM_ptr->text(LOG_DEBUG, 1, " - Going to set Transfer Syntax to Explicit VR Little Endian as default for Media");
			}

			// do our best - set the transfer syntax as dicom default for media
			fileTf_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);
		}
		else 
		{
			FILEHEAD_CLASS *filehead_ptr = static_cast<FILEHEAD_CLASS*>(wid_ptr);

			// set the required transfer syntax
			fileTf_ptr->setTsCode(transferSyntaxUidToCode(filehead_ptr->getTransferSyntaxUid()), (char*) filehead_ptr->getTransferSyntaxUid().get());
		}
	}

	// remove any Dataset Trailing Padding
    dataset_ptr->removeTrailingPadding();
		
	// encode the dataset
	bool result = dataset_ptr->encode(*fileTf_ptr);

	// clean up the file transfer
	delete fileTf_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool FILE_DATASET_CLASS::writeDataset(bool autoCreateDirectory)

//  DESCRIPTION     : Method to stream the DICOM object to file using the supplied
//					  transfer syntax.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// check for valid dataset to write
	DCM_DATASET_CLASS *dataset_ptr = datasetM_ptr;
	if (dataset_ptr == NULL)
    {
        return false;
    }

	// check the an absolute pathname has been given
	if (!isAbsolutePath(filenameM))
	{
		// see if a results root is defined
		if (loggerM_ptr)
		{
			// see if the path has been defined
			string pathname = loggerM_ptr->getStorageRoot();
			string filename = filenameM;

			if (pathname.length())
			{
				// set up filename by including path
                filenameM = pathname;
                if (pathname[pathname.length()-1] != '\\') filenameM += "\\";
                filenameM += filename;
			}
		}
	}

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "About to append Dataset to \"%s\"", filenameM.c_str()); 
	}

	if (autoCreateDirectory) //autocreate stuff should be here
	{
		createDirectory(filenameM);
	}

	// set up the file transfer
	FILE_TF_CLASS *fileTf_ptr = new FILE_TF_CLASS(filenameM, "wb");

	// cascade the logger
	fileTf_ptr->setLogger(dataset_ptr->getLogger());

	// check the file is open
	if (!fileTf_ptr->isOpen()) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't append Dataset to \"%s\"", filenameM.c_str());
			loggerM_ptr->text(LOG_NONE, 1, "Check directory path exists.");
		}

		delete fileTf_ptr;
		return false;
	}

	// to check if the dataset is the FMI - see if the attribute tags are from Group 0002
	if (dataset_ptr->containsAttributesFromGroup(GROUP_TWO))
	{
		// force the Group 0002 length
		dataset_ptr->setDefineGroupLengths(true);

		// set the required transfer syntax
		fileTf_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);
	}
	else
	{
		// try to retrive the file head in order to get the transfer syntax to use when
		// encoding this dataset
		if (transferSyntaxM == "")
		{
			// do our best - set the transfer syntax as dicom default for media
			fileTf_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);
		}
		else 
		{
			// now get the tsCode from the transfer syntax
			TS_CODE tsCode = transferSyntaxUidToCode(transferSyntaxM);

			// set the required transfer syntax
			fileTf_ptr->setTsCode(tsCode, transferSyntaxM);
		}
	}

	// remove any Dataset Trailing Padding
    dataset_ptr->removeTrailingPadding();
		
	// encode the dataset
	bool result = dataset_ptr->encode(*fileTf_ptr);

	// clean up the file transfer
	delete fileTf_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool FILE_DATASET_CLASS::write(bool autoCreateDirectory)

//  DESCRIPTION     : Method to write the FMI and Dataset to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
   	// check for valid FMI & Dataset to write
    MEDIA_FILE_HEADER_CLASS *fmi_ptr = fmiM_ptr;
    DCM_DATASET_CLASS *dataset_ptr = datasetM_ptr;
	if ((fmi_ptr == NULL) ||
        (dataset_ptr == NULL))
    {
        return false;
    }

    if (autoCreateDirectory) //autocreate stuff should be here
	{
		createDirectory(filenameM);
	}

	//Set group length property
	if(getAddGroupLength())
		dataset_ptr->setDefineGroupLengths(true);

    // set up the file transfer
	FILE_TF_CLASS *fileTf_ptr = new FILE_TF_CLASS(filenameM, "wb");

	// cascade the logger
	fileTf_ptr->setLogger(loggerM_ptr);

	// check the file is open
	if (!fileTf_ptr->isOpen()) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't write Media Dataset to \"%s\"", filenameM.c_str());
		}

		delete fileTf_ptr;
		return false;
	}
    string transferSyntaxUid;
    if (!fmi_ptr->getUIValue(TAG_TRANSFER_SYNTAX_UID, transferSyntaxUid))
    {
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't get Transfer Syntax UID from FMI to write \"%s\"", filenameM.c_str());
		}

		delete fileTf_ptr;
		return false;
    }

    // write the FMI to file
	// set the required transfer syntax
	fileTf_ptr->setTsCode(TS_EXPLICIT_VR | TS_LITTLE_ENDIAN, EXPLICIT_VR_LITTLE_ENDIAN);
    if (!fmi_ptr->write(fileTf_ptr))
    {
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't encode the FMI to write \"%s\"", filenameM.c_str());
		}

		delete fileTf_ptr;
		return false;
    }

	// set the required transfer syntax for the dataset
    fileTf_ptr->setTsCode(transferSyntaxUidToCode(transferSyntaxUid), transferSyntaxUid.c_str());

	// encode the dataset
	bool result = dataset_ptr->encode(*fileTf_ptr);

	// clean up the file transfer
	delete fileTf_ptr;

	// return result
	return result;
}

//>>===========================================================================

void FILE_DATASET_CLASS::setStorageMode(STORAGE_MODE_ENUM storageMode) 

//  DESCRIPTION     : Set the Storage Mode.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    storageModeM = storageMode; 
}

//>>===========================================================================

void FILE_DATASET_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the Logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{ 
    loggerM_ptr = logger_ptr; 
}
