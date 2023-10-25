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
#include "filetail.h"
#include "filehead.h"
#include "Inetwork.h"			// Network component interface


//>>===========================================================================

FILETAIL_CLASS::FILETAIL_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_FILETAIL;
	identifierM = "";

	// set up trailing padding
	trailingPaddingM = false;
	sectorSizeM = DSTP_SECTOR_SIZE;
	paddingValueM = DSTP_PADDING_VALUE;
	loggerM_ptr = NULL;
}

//>>===========================================================================

FILETAIL_CLASS::~FILETAIL_CLASS()

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

bool FILETAIL_CLASS::write(bool autoCreateDirectory)

//  DESCRIPTION     : Method to stream the dataset trailing padding into the
//					  given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = NULL;

	// check if we should write the trailing padding
	if (!trailingPaddingM) return true;

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
		loggerM_ptr->text(LOG_DEBUG, 1, "About to append FILE_TAIL to \"%s\"", filenameM.c_str()); 
	}

	if (autoCreateDirectory) //autocreate stuff should be here
	{
		createDirectory(filenameM);
	}

	// set up the file transfer - append dataset trailing padding
	FILE_TF_CLASS *fileTf_ptr = new FILE_TF_CLASS(filenameM, "ab");

	// cascade the logger
	fileTf_ptr->setLogger(loggerM_ptr);

	// try to retrive the file head in order to get the transfer syntax to use when
	// encoding this trailing padding
	if ((wid_ptr = WAREHOUSE->retrieve("", WID_FILEHEAD)) == NULL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Can't find %s in Data Warehouse", WIDName(WID_FILEHEAD));
		}
		return false;
	}
	FILEHEAD_CLASS *filehead_ptr = static_cast<FILEHEAD_CLASS*>(wid_ptr);

	// set the required transfer syntax
	fileTf_ptr->setTsCode(transferSyntaxUidToCode(filehead_ptr->getTransferSyntaxUid()), (char*) filehead_ptr->getTransferSyntaxUid().get());

	// check the file is open
	if (!fileTf_ptr->isOpen()) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't append FILE_TAIL to \"%s\"", filenameM.c_str());
			loggerM_ptr->text(LOG_NONE, 1, "Check directory path exists.");
		}

		delete fileTf_ptr;
		return false;
	}

	// compute the length of the trailing padding
	// - need to pad file length to multiple of sector size
	INT length = fileTf_ptr->getLength();
	length = ((length / sectorSizeM) * sectorSizeM) - length + sectorSizeM;
	bool result = true;

	// don't add padding if the file length is a multiple of the sectorSize
	if (length != (INT) sectorSizeM)
	{
		// - modify length by dataset trailing padding tag (4), [vr (2), padding (2)] & length (4)
		if (fileTf_ptr->getTsCode() & TS_EXPLICIT_VR)
		{
			// check if the remaining sector length can include at least the tag, etc
			if (length < 12) length += sectorSizeM;

			// subtract length required for tag, etc
			length -= 12;
		}
		else
		{
			// check if the remaining sector length can include at least the tag, etc
			if (length < 8) length += sectorSizeM;

			// subtract length required for tag, etc
			length -= 8;
		}

		// add the attribute with value
		(void) setOBValue(TAG_DATASET_TRAILING_PADDING, 1, length, paddingValueM);

		// encode the media header attributes
		result = DCM_ATTRIBUTE_GROUP_CLASS::encode(*fileTf_ptr);
	}

	// clean up the file transfer
	delete fileTf_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool FILETAIL_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this file tail with the contents of the file tail given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	FILETAIL_CLASS *updateFiletail_ptr = static_cast<FILETAIL_CLASS*>(wid_ptr);

	// copy all fields - assume update of all values
	trailingPaddingM = updateFiletail_ptr->trailingPaddingM;
	sectorSizeM = updateFiletail_ptr->sectorSizeM;
	paddingValueM = updateFiletail_ptr->paddingValueM;

	// return result
	return result;
}
