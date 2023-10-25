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

#ifndef DEFINITION_H
#define DEFINITION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_DETAILS_CLASS;
class DEF_FILE_CONTENT_CLASS;
class AE_SESSION_CLASS;
class DCM_ATTRIBUTE_CLASS;
class DCM_ITEM_CLASS;
class DCM_DATASET_CLASS;
class DEF_AE_CLASS;
class DEF_ATTRIBUTE_REGISTER_CLASS;
class DEF_ATTRIBUTE_CLASS;
class BASE_VALUE_CLASS;
class DEF_COMMAND_CLASS;
class DEF_DATASET_CLASS;
class DEF_ITEM_CLASS;
class DEF_METASOPCLASS_CLASS;
class DEF_SOPCLASS_CLASS;
class DEF_SYSTEM_CLASS;
class LOG_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define DEFINITION	DEF_DEFINITION_CLASS::Instance()

#define DEFAULT_SYSTEM_NAME     "DICOM"
#define DEFAULT_SYSTEM_VERSION  "3.0"
#define DEFAULT_AE_NAME         "DICOM"
#define DEFAULT_AE_VERSION      "3.0"

// number of different application entities allowed
#define MAX_APPLICATION_ENTITY	256

// define structure for SOP Class name mapping
struct T_ADVT2DVT_NAME_MAP
{
	char* advt_name;
	char* dvt_name;
};

// define structure for recognition code mapping
struct RECOGNITION_CODE_STRUCT
{
	UINT32 tag;
	string code;
};		

// SOP category enumeration
enum SOP_CATEGORY_ENUM
{
	SOP_CATEGORY_UNKNOWN,
	SOP_CATEGORY_STORAGE,
	SOP_CATEGORY_PRINT,
	SOP_CATEGORY_QUERY,
	SOP_CATEGORY_RETRIEVE,
	SOP_CATEGORY_MEDIA,
	SOP_CATEGORY_MPPS,
	SOP_CATEGORY_COMMIT,
	SOP_CATEGORY_WORKLIST,
	SOP_CATEGORY_VERIFICATION
};

typedef struct
{
	string defFileName;
    string sopClassName;				
    string sopClassUid;					
    string aeName;
    string aeVersion;
	bool isMetaSop;
	bool isInstalled;
} DEF_FILE_STRUCT;

typedef vector<DEF_FILE_STRUCT*> DEF_FILES_VECTOR;

// define register types
typedef vector<DEF_SYSTEM_CLASS*> DEF_SYSTEM_REGISTER;

typedef vector<RECOGNITION_CODE_STRUCT> DEF_RECOGNITION_CODE_REGISTER;

typedef vector<DEF_COMMAND_CLASS*> DEF_COMMAND_REGISTER;

typedef vector<DEF_ATTRIBUTE_REGISTER_CLASS*> DEF_ATTRIBUTE_REGISTER;

typedef map<string,string> DEF_SOP_REGISTER;

typedef map<string,string> DEF_IOD_REGISTER;

//typedef map<string,string> DEF_FILES_MAP;


//>>***************************************************************************

class DEF_DEFINITION_CLASS

//  DESCRIPTION     : Provides 1 instance (Singleton) for definition storage.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	static DEF_DEFINITION_CLASS* Instance();

	void Cleanup();

	bool Install(const string, DEF_FILE_CONTENT_CLASS*);

	bool Uninstall(const string);

	void RegisterSop(const string, const string);

	void RegisterIod(const string, const string);

	void RegisterCommand(DEF_COMMAND_CLASS*);

	void RegisterAttribute(DEF_ATTRIBUTE_CLASS*);

	void RegisterRecognitionCode(DEF_ATTRIBUTE_CLASS*);

	void UnregisterCommand(DEF_COMMAND_CLASS*);

	void UnregisterAttribute(const UINT, const UINT);

	void AddSystem(DEF_SYSTEM_CLASS*);

	void SetDefinitionFileRoot(const string);

	void SetLastSopUidInstalled(const string);

	void SetLastSopUidRemoved(const string);

	string GetDefinitionFileRoot(void);

	bool GetDetails(const string, DEF_DETAILS_CLASS&);

	DEF_SYSTEM_CLASS* GetSystem(const string, const string);

	void GetSystemDefinitions(string*, string*);

	UINT GetNoApplicationEntities();

	string GetApplicationEntityName(UINT);

	string GetApplicationEntityVersion(UINT);

	DEF_AE_CLASS* GetAe(const string, const string);
	DEF_AE_CLASS* GetAe(const string);

	int GetNrMetaSopClasses();

	DEF_METASOPCLASS_CLASS* GetMetaSopClassByUid(const string);

	DEF_METASOPCLASS_CLASS* GetMetaSopClass(UINT32);

	string GetMetaSopUid(const string);

	DEF_SOPCLASS_CLASS* GetSopClassByName(const string);

	DEF_SOPCLASS_CLASS* GetSopClassByName(const string, const string, const string);
		
	DEF_SOPCLASS_CLASS* GetSopClassByFilename(const string);

	DVT_STATUS GetSopClassByUid(const string, AE_SESSION_CLASS*, DEF_SOPCLASS_CLASS**);

	string GetSopName(const string);
 
	string GetSopUid(const string);

	string GetLastSopUidInstalled();

	string GetLastSopUidRemoved();

	string GetImageBoxSopName(const string);

	string GetImageBoxSopUid(const string);

	DVT_STATUS GetIodName(DIMSE_CMD_ENUM, const string, AE_SESSION_CLASS*, string&);

	DVT_STATUS GetSopFileName(DIMSE_CMD_ENUM, const string, AE_SESSION_CLASS*, string&);
      
	DEF_COMMAND_CLASS* GetCommand(DIMSE_CMD_ENUM);

	DEF_DATASET_CLASS* GetDataset(const string);
	DVT_STATUS GetDataset(DIMSE_CMD_ENUM, const string, AE_SESSION_CLASS*, DEF_DATASET_CLASS**);

	DEF_ATTRIBUTE_CLASS* GetAttribute(const UINT, const UINT);

    ATTR_VR_ENUM GetAttributeVr(const UINT, const UINT);

    string GetAttributeName(const UINT, const UINT);

	ATTR_TYPE_ENUM GetAttributeType(const UINT group, const UINT element);

    bool GetAttributeXMLName(const UINT, const UINT, string&);

	bool GetAttributeTagByXMLName(const string, UINT16&, UINT16&);

	UINT32 GetNrDefinedTerms(DIMSE_CMD_ENUM, const string, UINT32, UINT32 value_nr = 0);

	BASE_VALUE_CLASS* GetDefinedTerm(DIMSE_CMD_ENUM, const string, UINT32, UINT32, UINT32 value_nr = 0);

	UINT32 GetNrRecognitionCodes();

	void GetRecognitionCode(UINT32, UINT32*, string&);

	bool IsStorageSop(const string, AE_SESSION_CLASS*);

	bool IsPrintSop(const string, AE_SESSION_CLASS*);

	bool IsMppsSop(const string, AE_SESSION_CLASS*);

	bool IsQueryRetrieveSop(const string, AE_SESSION_CLASS*);

	bool IsWorklistSop(const string, AE_SESSION_CLASS*);

	bool PopulateWithAttributes(DCM_DATASET_CLASS*, LOG_CLASS*);

	const char* MapAdvt2DvtSopName(const char*);

	const char* MapAdvt2DvtIodName(const char*);

	void AddDefFile(const string,const string,const string,const string,const string,bool,bool);

	void RemoveDefFile(const string);

	bool IsDefFileParsed(const string);

	DEF_FILE_STRUCT* GetDefFileStruct(const string);

protected:
	DEF_DEFINITION_CLASS();

	void Initialise();

private:
	static DEF_DEFINITION_CLASS* instanceM_ptr; // Singleton
	DEF_SYSTEM_REGISTER systemsM;
	DEF_SYSTEM_CLASS* current_systemM_ptr;
	DEF_AE_CLASS* current_aeM_ptr;
	DEF_SOPCLASS_CLASS* current_sopM_ptr;
	string definition_file_rootM;
	DEF_SOP_REGISTER sop_registerM;
	DEF_FILES_VECTOR def_filesM;
	string last_registered_sopM;
	string last_removed_sopM;
	DEF_IOD_REGISTER iod_registerM;
	DEF_RECOGNITION_CODE_REGISTER recognition_code_registerM;
	DEF_COMMAND_REGISTER commandsM;
	DEF_ATTRIBUTE_REGISTER attribute_registerM;

	bool PopulateDataset(DCM_DATASET_CLASS*, DEF_DATASET_CLASS*, LOG_CLASS*);

    bool PopulateSequence(DCM_ATTRIBUTE_CLASS*, DEF_ATTRIBUTE_CLASS*, LOG_CLASS*);

	bool PopulateItem(DCM_ITEM_CLASS*, DEF_ITEM_CLASS*, LOG_CLASS*);

    SOP_CATEGORY_ENUM DetermineSopCategory(const string, AE_SESSION_CLASS*);
};

#endif /* DEFINITION_H */
