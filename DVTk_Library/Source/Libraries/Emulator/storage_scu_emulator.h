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
//  DESCRIPTION     :	Storage SCU emulator class.
//*****************************************************************************
#ifndef STORAGE_SCU_EMULATOR_H
#define STORAGE_SCU_EMULATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"				// Global component interface
#include "Idicom.h"			// Dicom component interface
#include "Inetwork.h"		// Network component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class EMULATOR_SESSION_CLASS;
class LOG_CLASS;
class DCM_DATASET_CLASS;
class MEDIA_FILE_HEADER_CLASS;


const UINT SCU_STORAGE_EML_OPTION_ASSOC_SINGLE       = 0x0001;
const UINT SCU_STORAGE_EML_OPTION_ASSOC_MULTI        = 0x0002;
const UINT SCU_STORAGE_EML_OPTION_VALIDATE_ON_IMPORT = 0x0004;
const UINT SCU_STORAGE_EML_OPTION_DATA_UNDER_NEWSTUDY= 0x0008;
const UINT SCU_STORAGE_EML_OPTION_REPEAT             = 0x0010;

struct FMI_DATASET_STRUCT
{
	string                   filename;
    string                   transferSyntax;
	MEDIA_FILE_HEADER_CLASS* fmi_ptr;
	DCM_DATASET_CLASS*       dat_ptr;
};

struct REF_INSTANCES_STRUCT
{
	string                   sopClassUid;
    string                   sopInstanceUid;
};


//>>***************************************************************************

class STORAGE_SCU_EMULATOR_CLASS

//  DESCRIPTION     : Storage emulator class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	EMULATOR_SESSION_CLASS		*sessionM_ptr;
	ASSOCIATION_CLASS			associationM;
	LOG_CLASS					*loggerM_ptr;
    BASE_SERIALIZER             *serializerM_ptr;
	bool						autoType2AttributesM;
	bool						defineSqLengthM;
	bool						addGroupLengthM;
	vector<string>              filenamesM;
	UINT                        optionsM;
	UINT                        nr_repetitionsM;
	UINT                        message_idM;
    vector<FMI_DATASET_STRUCT>  filedatasetsM;
	vector<REF_INSTANCES_STRUCT>refSopInstancesM;
	bool						associatedM;

	bool sendFilesInMultipleAssociations(int repetition_index);

    bool sendFilesInSingleAssociation(int repetition_index);

	bool importFile(string filename);

	bool importRefFilesFromDicomdir(FILE_DATASET_CLASS* fileDataset,string filename);

	void modifyDicomObjectWithSeriesUID(DCM_ATTRIBUTE_GROUP_CLASS* fileDataset, string uid, string imsge_uid);

	string extractDirectoryName (string filename);

	bool CheckFileExistance (string filename);

	void cleanup();

public:
	STORAGE_SCU_EMULATOR_CLASS(EMULATOR_SESSION_CLASS*);
	~STORAGE_SCU_EMULATOR_CLASS();

	bool emulate();

	bool verify();

	bool sendNActionReq(int delay);

	bool eventReportStorageCommitment(UINT16, DCM_DATASET_CLASS*);

	void addFile(string filename);

	void removeFile(string filename);

	bool isAssociated() { return associatedM; }

	UINT resetOptions();

	UINT setOption(UINT option); 
	UINT addOption(UINT option); 
	void setNrRepetitions(UINT nr);

	void setLogger(LOG_CLASS*);

	void performStatusLogging(UINT16 status, string sop_uid,string sop_instance_uid);

    void setSerializer(BASE_SERIALIZER*);
};

#endif /* STORAGE_SCU_EMULATOR_H */
