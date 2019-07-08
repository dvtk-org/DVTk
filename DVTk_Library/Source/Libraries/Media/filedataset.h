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
#ifndef FILEDATASET_H
#define FILEDATASET_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_DATASET_CLASS;
class DCM_DIR_DATASET_CLASS;
class FILE_TF_CLASS;
class LOG_CLASS;
class MEDIA_FILE_HEADER_CLASS;
class DCM_ITEM_CLASS;


//>>***************************************************************************

class FILE_DATASET_CLASS

//  DESCRIPTION     : File based DICOM Dataset
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string							filenameM;
    string							transferSyntaxM;
	MEDIA_FILE_CONTENT_TYPE_ENUM	fileContentTypeM;
	string							sopClassUidM; 
	string							sopInstanceUidM; 
	STORAGE_MODE_ENUM				storageModeM;
	bool							unVrDefinitionLookUpM;
	bool							ensureEvenAttributeValueLengthM;
	bool							addGroupLengthM;
    FILE_TF_CLASS					*fileTfM_ptr;
	MEDIA_FILE_HEADER_CLASS			*fmiM_ptr;
	DCM_DATASET_CLASS				*datasetM_ptr;
    DCM_DIR_DATASET_CLASS			*dicomdirDatasetM_ptr;
	LOG_CLASS						*loggerM_ptr;

	bool isFileMetaInformation(FILE_TF_CLASS*, string&);

    MEDIA_FILE_HEADER_CLASS* readFileMetaInformation(FILE_TF_CLASS*);

	bool deduceTransferSyntax(FILE_TF_CLASS*, string&);

public:
	FILE_DATASET_CLASS();
	FILE_DATASET_CLASS(string);
	FILE_DATASET_CLASS(string, MEDIA_FILE_CONTENT_TYPE_ENUM, string, string, string);

	~FILE_DATASET_CLASS();

	void setFilename(string);

	const char* getFilename();

	void setTransferSyntax(string);

    string getTransferSyntax();

    void setFileMetaInformation(MEDIA_FILE_HEADER_CLASS*);

	MEDIA_FILE_HEADER_CLASS* getFileMetaInformation();

	void setUnVrDefinitionLookUp(bool flag)
	{
		unVrDefinitionLookUpM = flag;
	}

	bool getUnVrDefinitionLookUp()
	{
		return unVrDefinitionLookUpM;
	}

	void setEnsureEvenAttributeValueLengthM(bool flag)
	{
		ensureEvenAttributeValueLengthM = flag;
	}

	bool getEnsureEvenAttributeValueLengthM()
	{
		return ensureEvenAttributeValueLengthM;
	}

	void setAddGroupLength(bool flag)
	{
		addGroupLengthM = flag;
	}

	bool getAddGroupLength()
	{
		return addGroupLengthM;
	}

    void setDataset(DCM_DATASET_CLASS*);

	DCM_DATASET_CLASS* getDataset();

    DCM_DIR_DATASET_CLASS* getDicomdirDataset();

    DCM_ITEM_CLASS* getNextDicomdirRecord();

	void clearFileMetaInformationPtr();

	void clearDatasetPtr();

	bool read(DCM_DATASET_CLASS*);

	bool read();

	bool write(DCM_DATASET_CLASS*, bool autoCreateDirectory = true);

	bool writeDataset(bool autoCreateDirectory = true);

    bool write(bool autoCreateDirectory = true);

	void setStorageMode(STORAGE_MODE_ENUM);

	void setLogger(LOG_CLASS*);
};

#endif /* FILEDATASET_H */
