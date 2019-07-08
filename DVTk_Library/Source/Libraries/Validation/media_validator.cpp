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
#include "media_validator.h"
#include "val_attribute.h"
#include "val_attribute_group.h"
#include "val_object_results.h"
#include "val_value_sq.h"
#include "record_link.h"
#include "record_results.h"
#include "record_types.h"
#include "record_uid.h"
#include "uid_defining.h"
#include "uid_referring.h"
#include "ValidationEngine.h"

#include "Idefinition.h"        // Definition component interface
#include "IAttributeGroup.h"    // Dicom component interface
#include "Iglobal.h"            // Global component interface
#include "Ilog.h"               // Logger component interface
#include "Imedia.h"             // Media component interface

//>>===========================================================================

MEDIA_VALIDATOR_CLASS::MEDIA_VALIDATOR_CLASS()

//  DESCRIPTION     : Class constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_OBJECT;
    flagsM = 0;
    recordLinksM.clear();
    offsetFirstDirRecordM = DICOMDIR_ILLEGAL_OFFSET;
    offsetLastDirRecordM = DICOMDIR_ILLEGAL_OFFSET;
    directoryM = "";
    suffixM = "";
    fileM = "";
    uidLinksM_ptr = new RECORD_UID_CLASS();
}

//>>===========================================================================

MEDIA_VALIDATOR_CLASS::MEDIA_VALIDATOR_CLASS(string filename)

//  DESCRIPTION     : Class constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_OBJECT;
    flagsM = 0;
    recordLinksM.clear();
    offsetFirstDirRecordM = DICOMDIR_ILLEGAL_OFFSET;
    offsetLastDirRecordM = DICOMDIR_ILLEGAL_OFFSET;
    ExtractFilename(filename);
    uidLinksM_ptr = new RECORD_UID_CLASS();
}

//>>===========================================================================

MEDIA_VALIDATOR_CLASS::~MEDIA_VALIDATOR_CLASS()

//  DESCRIPTION     : Class destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    delete objectResultsM_ptr;
    recordLinksM.clear();
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::CleanResults()

//  DESCRIPTION     : Keeps the object results, but removes all modules within
//                    that object. (An object contains exactly 1 module which
//                    represents a dicomdir record).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    objectResultsM_ptr->CleanUp();
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::CopyObjectResults(VAL_OBJECT_RESULTS_CLASS **valObjectResults_ptr_ptr)

//  DESCRIPTION     : Copy the object results and lose owner ship of the object results.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    *valObjectResults_ptr_ptr = objectResultsM_ptr;

    // Since the caller is now the owner of the object results, we don't want
    // the destructor to cleanup the results.
    objectResultsM_ptr = NULL;
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::Serialize(BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Serialize the validation results.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // serialize the object results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(objectResultsM_ptr, &recordLinksM, flagsM);
    }
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::CreateFMIResultsFromDef(FILE_DATASET_CLASS *fileDataset_ptr,
                                           AE_SESSION_CLASS *aeSession_ptr)

//  DESCRIPTION     : Create the definition File Meta Information results structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool status = true;
    char message[256];

	defDatasetM_ptr = NULL;

    // Get the file meta information from the file dataset
    MEDIA_FILE_HEADER_CLASS *fileMetaInfo_ptr = fileDataset_ptr->getFileMetaInformation();
    if (fileMetaInfo_ptr == NULL)
    {
        // There's no File Meta Information
        sprintf(message,
            "The file \"%s\" does not have (valid) File Meta Information",
            fileDataset_ptr->getFilename());
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_8, message);

        status = false;
    }
    else
    {
        string aeName = aeSession_ptr->GetName();
        string aeVersion = aeSession_ptr->GetVersion();

        // Validate the file meta elements
        DEF_SOPCLASS_CLASS *defSopClass_ptr = NULL;
        DVT_STATUS result = DEFINITION->GetSopClassByUid(FILE_META_SOP_CLASS_UID, aeSession_ptr, &defSopClass_ptr);
		switch(result)
		{
		case MSG_DEFINITION_NOT_FOUND:
			// sop_class is returned NULL here - nothing further to do
			break;
		case MSG_DEFAULT_DEFINITION_FOUND:
            {
                // not using the requested definition AE - should inform the user
                // sop_class contains the default definition
                aeName = DEFAULT_AE_NAME;
                aeVersion = DEFAULT_AE_VERSION;

                string stringMessage;
                sprintf(message,
                    "Could not find System AE Name \"%s\" - AE Version \"%s\" Dataset definition for SOP: \"%s\", Dimse: %s.",
                    aeSession_ptr->GetName().c_str(),
                    aeSession_ptr->GetVersion().c_str(),
                    FILE_META_SOP_CLASS_NAME,
                    mapCommandName(DIMSE_CMD_CSTORE_RQ));
                stringMessage.append(message);
                sprintf(message,
                    " Using Default AE Name \"%s\" - AE Version \"%s\"",
                    DEFAULT_AE_NAME, 
                    DEFAULT_AE_VERSION);
                stringMessage.append(message);
                objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_DEFINITION_6, stringMessage);
            }
			break;
		case MSG_OK:
			// found the requested definition
			// sop_class contains the requested definition
			break;
		default:
			break;
		}        

        if (defSopClass_ptr != NULL)
        {
            // Get the Dataset for the SOP - Dimse combination
            defDatasetM_ptr = defSopClass_ptr->GetDataset(DIMSE_CMD_CSTORE_RQ);
        }

        if (defDatasetM_ptr != NULL)
        {
            objectResultsM_ptr->SetName(defDatasetM_ptr->GetName());

            string stringMessage;
            sprintf(message,
                "Selected Dataset definition: \"%s\".",
                defDatasetM_ptr->GetName().c_str());
            stringMessage.append(message);
            sprintf(message,
                " Using AE Name \"%s\" - AE Version \"%s\"",
                aeName.c_str(),
                aeVersion.c_str());
            stringMessage.append(message);
            objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_SOP_1, stringMessage);

            // Create FMI result
            CreateModuleResultsFromDef(defDatasetM_ptr, fileMetaInfo_ptr);

            status = true;
        }
        else
        {
            string stringMessage;
            sprintf(message, 
                "Could not find Dataset definition for SOP: %s, Dimse: %s.",
                FILE_META_SOP_CLASS_NAME, 
                mapCommandName(DIMSE_CMD_CSTORE_RQ));
            stringMessage.append(message);
            sprintf(message, 
                " Using AE Name \"%s\" - AE Version \"%s\"",
                aeName.c_str(),
                aeVersion.c_str());
            stringMessage.append(message);
            objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_SOP_2, stringMessage);
            status = false;
        }
    }

    return status;
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::CreateMediaResultsFromDef(FILE_DATASET_CLASS *fileDataset_ptr,
                                             AE_SESSION_CLASS *aeSession_ptr)

//  DESCRIPTION     : Create the definition media results structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool status = true;
    char message[256];

	defDatasetM_ptr = NULL;

    // Get the file meta information from the file dataset
    MEDIA_FILE_HEADER_CLASS *fileMetaInfo_ptr = fileDataset_ptr->getFileMetaInformation();

    string uid = "";
    if (fileMetaInfo_ptr != NULL)
    {
        fileMetaInfo_ptr->getUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, uid);
    }

    if (uid.length() == 0)
    {
        // The UID has not been found in the File Meta Information.
        // Try to get it from the dataset.
        fileDataset_ptr->getDataset()->getUIValue(TAG_SOP_CLASS_UID, uid);
    }

    string aeName = aeSession_ptr->GetName();
    string aeVersion = aeSession_ptr->GetVersion();

    DEF_SOPCLASS_CLASS *defSopClass_ptr = NULL;
    if (uid.length() != 0)
    {
        // use UID to search for definition
        DVT_STATUS result = DEFINITION->GetSopClassByUid(uid, aeSession_ptr, &defSopClass_ptr);
		switch(result)
		{
		case MSG_DEFINITION_NOT_FOUND:
			// sop_class is returned NULL here - nothing further to do
			break;
		case MSG_DEFAULT_DEFINITION_FOUND:
			// not using the requested definition AE - should inform the user
			// sop_class contains the default definition
            {
                aeName = DEFAULT_AE_NAME;
                aeVersion = DEFAULT_AE_VERSION;

                string stringMessage;
                sprintf(message,
                    "Could not find System AE Name \"%s\" - AE Version \"%s\" Dataset definition for SOP: \"%s\", Dimse: %s.",
                    aeSession_ptr->GetName().c_str(),
                    aeSession_ptr->GetVersion().c_str(),
                    uid.c_str(),
                    mapCommandName(DIMSE_CMD_CSTORE_RQ));
                stringMessage.append(message);
                sprintf(message,
                    " Using Default AE Name \"%s\" - AE Version \"%s\"",
                    DEFAULT_AE_NAME, 
                    DEFAULT_AE_VERSION);
                stringMessage.append(message);
                objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_DEFINITION_6, stringMessage);
            }
			break;
		case MSG_OK:
			// found the requested definition
			// sop_class contains the requested definition
			break;
		default:
			break;
		}        
    }

    if (defSopClass_ptr != NULL)
    {
        // Get the Dataset for the SOP - Dimse combination
        defDatasetM_ptr = defSopClass_ptr->GetDataset(DIMSE_CMD_CSTORE_RQ);
    }

    if (defDatasetM_ptr != NULL)
    {
        objectResultsM_ptr->SetName(defDatasetM_ptr->GetName());

        string stringMessage;
        sprintf(message,
            "Selected Dataset definition: \"%s\".",
            defDatasetM_ptr->GetName().c_str());
        stringMessage.append(message);
        sprintf(message,
            " Using AE Name \"%s\" - AE Version \"%s\"",
            aeName.c_str(),
            aeVersion.c_str());
        stringMessage.append(message);
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_SOP_1, stringMessage);

		// set the dataset co-object as the file meta information
		objectResultsM_ptr->SetFmi(fileMetaInfo_ptr);

        // Create Dataset Results
        CreateModuleResultsFromDef(defDatasetM_ptr, fileDataset_ptr->getDataset());

        status = true;
    }
    else
    {
        string stringMessage;
        if (uid.length())
        {
            sprintf(message,
                "Could not find Dataset definition for SOP UID: \"%s\", Dimse: %s",
                uid.c_str(), 
                mapCommandName(DIMSE_CMD_CSTORE_RQ));
            stringMessage.append(message);
        }
        else
        {
            sprintf(message,
                "Could not find Dataset definition for SOP UID: \"UNKNOWN\", Dimse: %s.",
                mapCommandName(DIMSE_CMD_CSTORE_RQ));
            stringMessage.append(message);
        }
        sprintf(message,
            " Using Definitions with AE Name \"%s\" - AE Version \"%s\"",
            aeName.c_str(),
            aeVersion.c_str());
        stringMessage.append(message);
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_SOP_2, stringMessage);
        status = false;
    }

    return status;
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::CreateDicomDirResultsFromDef(DCM_DIR_DATASET_CLASS *dicomdirDataset_ptr,
                                                MEDIA_FILE_HEADER_CLASS *fileMetaInfo_ptr,
                                                AE_SESSION_CLASS *aeSession_ptr,
                                                DEF_ATTRIBUTE_GROUP_CLASS **record_ptr_ptr)

//  DESCRIPTION     : Create the definition DicomDir results structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool status = true;
    char message[256];

	defDatasetM_ptr = NULL;

    string uid = "";
    if (fileMetaInfo_ptr != NULL)
    {
        fileMetaInfo_ptr->getUIValue(TAG_MEDIA_STORAGE_SOP_CLASS_UID, uid);
    }

    if (uid == "")
    {
        // The UID has not been found in the File Meta Information.
        // Try to get it from the dataset.
        dicomdirDataset_ptr->getUIValue(TAG_SOP_CLASS_UID, uid);
    }

    string aeName = aeSession_ptr->GetName();
    string aeVersion = aeSession_ptr->GetVersion();

    DEF_SOPCLASS_CLASS *defSopClass_ptr = NULL;

    if (uid != "")
    {
        DVT_STATUS result = DEFINITION->GetSopClassByUid(uid, aeSession_ptr, &defSopClass_ptr);
		switch(result)
		{
		case MSG_DEFINITION_NOT_FOUND:
			// sop_class is returned NULL here - nothing further to do
			break;
		case MSG_DEFAULT_DEFINITION_FOUND:
			// not using the requested definition AE - should inform the user
			// sop_class contains the default definition
            {
                aeName = DEFAULT_AE_NAME;
                aeVersion = DEFAULT_AE_VERSION;

                string stringMessage;
                sprintf(message,
                    "Could not find System AE Name \"%s\" - AE Version \"%s\" Dataset definition for SOP: \"%s\", Dimse: %s.",
                    aeSession_ptr->GetName().c_str(),
                    aeSession_ptr->GetVersion().c_str(),
                    FILE_META_SOP_CLASS_NAME,
                    mapCommandName(DIMSE_CMD_CSTORE_RQ));
                stringMessage.append(message);
                sprintf(message,
                    " Using Default AE Name \"%s\" - AE Version \"%s\"",
                    DEFAULT_AE_NAME, 
                    DEFAULT_AE_VERSION);
                stringMessage.append(message);
                objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_DEFINITION_6, stringMessage);
            }
			break;
		case MSG_OK:
			// found the requested definition
			// sop_class contains the requested definition
			break;
		default:
			break;
		}        
    }

    if (defSopClass_ptr != NULL)
    {
        // Get the Dataset for the SOP - Dimse combination.
        defDatasetM_ptr = defSopClass_ptr->GetDataset(DIMSE_CMD_CSTORE_RQ);
    }

    if (defDatasetM_ptr != NULL)
    {
        objectResultsM_ptr->SetName(defDatasetM_ptr->GetName());
		objectResultsM_ptr->SetDICOMDIRName(fileM);

        string stringMessage;
        sprintf(message,
            "Selected Dataset definition: \"%s\".",
            defDatasetM_ptr->GetName().c_str());
        stringMessage.append(message);
        sprintf(message,
            " Using AE Name \"%s\" - AE Version \"%s\"",
            aeName.c_str(),
            aeVersion.c_str());
        stringMessage.append(message);
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_SOP_1, stringMessage);

        // Create FMI Results
        CreateModuleResultsFromDef(defDatasetM_ptr, fileMetaInfo_ptr);

        // Set the record definition
        DEF_ATTRIBUTE_CLASS *defAttr_ptr = defDatasetM_ptr->GetAttribute((TAG_DIRECTORY_RECORD_SEQUENCE & 0xFFFF0000) >> 16, TAG_DIRECTORY_RECORD_SEQUENCE & 0x0000FFFF);
        if (defAttr_ptr)
        {
#ifdef NDEBUG
            VALUE_SQ_CLASS *sqValue_ptr = static_cast<VALUE_SQ_CLASS*>(defAttr_ptr->GetValue());
#else
            VALUE_SQ_CLASS *sqValue_ptr = dynamic_cast<VALUE_SQ_CLASS*>(defAttr_ptr->GetValue());
#endif
            ATTRIBUTE_GROUP_CLASS *item_ptr = NULL;
            sqValue_ptr->Get(&item_ptr);
#ifdef NDEBUG
            *record_ptr_ptr = static_cast<DEF_ATTRIBUTE_GROUP_CLASS*>(item_ptr);
#else
            *record_ptr_ptr = dynamic_cast<DEF_ATTRIBUTE_GROUP_CLASS*>(item_ptr);
#endif
            status = true;
        }
        else
            status = false;
    }
    else
    {
        string stringMessage;
        sprintf(message,
            "Could not find Dataset definition for SOP: \"%s\", Dimse: %s.",
            FILE_META_SOP_CLASS_NAME, 
            mapCommandName(DIMSE_CMD_CSTORE_RQ));
        stringMessage.append(message);
        sprintf(message,
            " Using AE Name \"%s\" - AE Version \"%s\"",
            aeName.c_str(),
            aeVersion.c_str()
            );
        stringMessage.append(message);
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_SOP_2, stringMessage);
        status = false;
    }

    return status;
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::Validate(DCM_DIR_DATASET_CLASS *dicomdirDataset_ptr,
                                DCM_DATASET_CLASS *refDataset_ptr,
                                MEDIA_FILE_HEADER_CLASS *fileMetaInfo_ptr,
                                VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                AE_SESSION_CLASS *aeSession_ptr,
                                DEF_ATTRIBUTE_GROUP_CLASS **record_ptr_ptr)

//  DESCRIPTION     : Validates a DicomDir dataset object
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Create a results object
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    if (objectResultsM_ptr == NULL)
    {
        return false;
    }

    resultsTypeM = RESULTS_DICOMDIR;

    // Update the results structure with the dicomdir information
    bool status = CreateDicomDirResultsFromDef(dicomdirDataset_ptr, fileMetaInfo_ptr, aeSession_ptr, record_ptr_ptr);
    if (status)
    {
        SetModuleResultsFromDcm(dicomdirDataset_ptr, true, false);
    }

    if (refDataset_ptr)
    {
        SetModuleResultsFromDcm(refDataset_ptr, false, false);
    }

    ValidateResults(validationFlag);

    // Store persistent record link information. This is needed for later.
    DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = dicomdirDataset_ptr->GetAttributeByTag(TAG_OFFSET_OF_THE_FIRST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY);
    if (dcmAttr_ptr)
    {
        dcmAttr_ptr->GetValue()->Get(offsetFirstDirRecordM);
    }

    dcmAttr_ptr = dicomdirDataset_ptr->GetAttributeByTag(TAG_OFFSET_OF_THE_LAST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY);
    if (dcmAttr_ptr)
    {
        dcmAttr_ptr->GetValue()->Get(offsetLastDirRecordM);
    }

    return status;
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::ValidateRecord(DCM_ITEM_CLASS *record_ptr,
                                      DCM_DATASET_CLASS *refRecord_ptr,
                                      DEF_ATTRIBUTE_GROUP_CLASS *recordDef_ptr,
                                      VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                      AE_SESSION_CLASS *aeSession_ptr,
                                      LOG_CLASS *logger_ptr,
                                      BASE_SERIALIZER *serializer_ptr,
                                      UINT32 recordIndex,
                                      STORAGE_MODE_ENUM storageMode,
									  bool validateReferencedFile,
									  bool dumpAttributesOfRefFiles)

//  DESCRIPTION     : Validates a dicomdir record object
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool status = true;

    // Add the record result as a module - will be deleted when objectResultsM_ptr is cleaned up.
    RECORD_RESULTS_CLASS *recordResult_ptr = new RECORD_RESULTS_CLASS(recordIndex);
    objectResultsM_ptr->AddModuleResults(recordResult_ptr);
    recordResult_ptr->SetParentObject(objectResultsM_ptr);

    // Add a persistent record link structure. This structure will contain
    // information needed for validation references between records. Each of
    // these structs is meant to keep as little information as possible to keep
    // memory consumption low.
    RECORD_LINK_CLASS *recordLink_ptr = new RECORD_LINK_CLASS(record_ptr->getOffset());
    recordLinksM.push_back(recordLink_ptr);

    if (recordDef_ptr != NULL)
    {
        // Only build the definition attribute object when the definition file
        // is loaded.
        CreateAttributeGroupResultsFromDef(recordDef_ptr, record_ptr, recordResult_ptr, false, false);
    }

	// Check if the attributes in the record are sorted.
    if (!record_ptr->IsSorted())
    {
        recordResult_ptr->GetMessages()->AddMessage(VAL_RULE_GENERAL_1, "Attributes are not sorted in ascending tag order");
    }

	// Update the Specific Character Set - required for extended character set validation
    UpdateSpecificCharacterSet(record_ptr);

    // We could've used the normal SetModuleResultsFromDcm() method here, but
    // because we need additional information, we've implemented a special
    // function which builds the module results, but also gathers additional
    // information.
    SetRecordResultsFromDcm(record_ptr, recordResult_ptr, recordLink_ptr);

    if (refRecord_ptr)
    {
        SetAttributeGroupResultsFromDcm(refRecord_ptr, recordResult_ptr, false);
    }

    recordResult_ptr->BuildPersistentInfo(uidLinksM_ptr);

    recordResult_ptr->Validate(directoryM,validateReferencedFile);

    // Perform normal object validation on the record.
    ValidateResults(validationFlag);

    // Validate the reference file - if applicable.
    if ((validateReferencedFile) &&
		(recordResult_ptr->GetRefFileName() != ""))
    {
        status = recordResult_ptr->ValidateReferenceFile(uidLinksM_ptr, 
                validationFlag, 
                aeSession_ptr, 
                logger_ptr, 
                serializer_ptr, 
                storageMode,
				dumpAttributesOfRefFiles);		
    }

    // Serialize the record and, if present, the record's reference file.
    serializer_ptr->SerializeValidate(recordResult_ptr, flagsM);

    return status;
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::Validate(FILE_DATASET_CLASS *fileDataset_ptr,
                                DCM_DATASET_CLASS *refDataset_ptr,
                                RECORD_UID_CLASS*,
                                VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                BASE_SERIALIZER *serializer_ptr,
                                AE_SESSION_CLASS *aeSession_ptr)

//  DESCRIPTION     : Validates a media dataset object
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The reason the build results and validation is done in
//                    two steps is because the first part of the media file is
//                    always the same (or should be). The second part is by
//                    definition NOT the same. That part could be a DICOMDIR or
//                    a normal DICOM object. For a DICOMDIR, special rules
//                    apply.
//<<===========================================================================
{
    // Extract relevant filename information required for the validator.
    ExtractFilename(fileDataset_ptr->getFilename());

    // Create a results structure for File Meta Information.
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_OBJECT;

    // Create the File Meta Information results structure.
    bool status = CreateFMIResultsFromDef(fileDataset_ptr, aeSession_ptr);
    if (status)
    {
		// Update the results structure with the source values.
        SetModuleResultsFromDcm(fileDataset_ptr->getFileMetaInformation(), true, false);

		// validate the File Meta Information.
		ValidateResults(validationFlag);
    }

	DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = NULL;
	BASE_VALUE_CLASS *uidInFMI_ptr = NULL;
    BASE_VALUE_CLASS *uidInDataset_ptr = NULL;
    char message[128];
	
	// Retrieve the Media Storage SOP Class UID
	VAL_ATTRIBUTE_CLASS *uidInFMIAttr_ptr = objectResultsM_ptr->GetAttributeResults(TAG_MEDIA_STORAGE_SOP_CLASS_UID);
    if (uidInFMIAttr_ptr)
    {
        dcmAttr_ptr = uidInFMIAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInFMI_ptr = dcmAttr_ptr->GetValue();
        }
    }

    // serialise the validation results
    Serialize(serializer_ptr);

    // Clean up the File Meta Information results structures.
    delete objectResultsM_ptr;

	// Create a new results structure for the Media Dataset.
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_OBJECT;

	// Update the results structure with the rest of the media file
    status = CreateMediaResultsFromDef(fileDataset_ptr, aeSession_ptr);
    if (status)
    {
		// Update the results structure with the source values.
        SetModuleResultsFromDcm(fileDataset_ptr->getDataset(), true, false);

		// Update the results structure with the reference values.
        if (refDataset_ptr != NULL)
        {
            SetModuleResultsFromDcm(refDataset_ptr, false, false);
        }

        ValidateResults(validationFlag);
    }

	// Retrieve the SOP Class UID
	VAL_ATTRIBUTE_CLASS *uidInDatasetAttr_ptr = objectResultsM_ptr->GetAttributeResults(TAG_SOP_CLASS_UID);
    if (uidInDatasetAttr_ptr)
    {
        dcmAttr_ptr = uidInDatasetAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInDataset_ptr = dcmAttr_ptr->GetValue();
        }
    }

	// Compare SOP class uid of the file meta information with the one in the dataset.
    if ((uidInFMI_ptr) &&
        (uidInDataset_ptr))
    {
        if (uidInFMI_ptr->Compare(NULL, uidInDataset_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "UID mismatch with Media Storage SOP Class UID (%04X,%04X) in file meta header", 
                GroupFromTag(TAG_MEDIA_STORAGE_SOP_CLASS_UID),
				ElementFromTag(TAG_MEDIA_STORAGE_SOP_CLASS_UID));
            uidInDatasetAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }

    // serialise the results
    Serialize(serializer_ptr);

    // Clean up the DICOMDIR results structures.
    delete objectResultsM_ptr;
    objectResultsM_ptr = NULL;

    return status;
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::ValidateRecordReferences()

//  DESCRIPTION     : Validate the record references.
//                    This means validating the:
//                     - directory record chain,
//                     - record references
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool status = CheckDirectoryRecordLinks();

    // Check if any of the records are unreferenced.
    for (UINT32 i = 0; i < recordLinksM.size(); i++)
    {
        if (!recordLinksM[i]->CheckUnreferencedRecords())
        {
            status = false;
        }
    }
    return status;
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::SetRecordResultsFromDcm(DCM_ATTRIBUTE_GROUP_CLASS *record_ptr,
                                           VAL_ATTRIBUTE_GROUP_CLASS *valAttrGroup_ptr,
                                           RECORD_LINK_CLASS *recordLink_ptr)

//  DESCRIPTION     : Build the record results structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    valAttrGroup_ptr->SetDcmAttributeGroup(record_ptr);

	for (int i = 0; i < record_ptr->GetNrAttributes(); i++)
    {
        DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = record_ptr->GetAttribute(i);
        VAL_ATTRIBUTE_CLASS *valAttr_ptr = valAttrGroup_ptr->GetAttribute(dcmAttr_ptr->GetMappedGroup(), 
																		dcmAttr_ptr->GetMappedElement());

        if (valAttr_ptr == NULL)
        {
            // The attribute is not known in the object results structure
            // Add a new validation attribute to the results structure.
            valAttr_ptr = new VAL_ATTRIBUTE_CLASS();
            valAttr_ptr->SetParent(valAttrGroup_ptr);
            valAttrGroup_ptr->AddValAttribute(valAttr_ptr);
        }

        valAttr_ptr->SetDcmAttribute(dcmAttr_ptr);

        if (dcmAttr_ptr->GetVR() == ATTR_VR_SQ)
        {
            SetSQResultsFromDcm(dcmAttr_ptr, valAttr_ptr, true);
        }
        else
        {
            SetValueResultsFromDcm(dcmAttr_ptr, valAttr_ptr, true);
        }

        // Store record information that is needed for later
        UINT32 tag = (dcmAttr_ptr->GetGroup () << 16) + dcmAttr_ptr->GetElement ();
        switch(tag)
        {
        case TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD:
            {
                UINT32 offset;
                dcmAttr_ptr->GetValue()->Get(offset);
                recordLink_ptr->SetHorLinkOffset(offset);
            }
            break;
        case TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY:
            {
                UINT32 offset;
                dcmAttr_ptr->GetValue()->Get(offset);
                recordLink_ptr->SetDownLinkOffset(offset);
            }
            break;
        case TAG_RECORD_IN_USE_FLAG:
            {
                UINT16 inUseFlag;
                dcmAttr_ptr->GetValue()->Get(inUseFlag);
                recordLink_ptr->SetInUseFlag(inUseFlag);
            }
            break;
        case TAG_DIRECTORY_RECORD_TYPE:
            recordLink_ptr->SetRecordType (RECORD_TYPES->GetRecordTypeIndexWithRecordName(dcmAttr_ptr->GetValue()));
            break;
        default:
            break;
        }
    }
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::ValidateRefFile(FILE_DATASET_CLASS *fileDataset_ptr,
                                        RECORD_RESULTS_CLASS *record_ptr,
                                        DCM_DATASET_CLASS *refDataset_ptr,
                                        VALIDATION_CONTROL_FLAG_ENUM validationFlag,
                                        AE_SESSION_CLASS *aeSession_ptr)

//  DESCRIPTION     : Validates a media dataset object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Create the default results object.
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_OBJECT;

    // Create the File Meta Information results structure.
    bool fmiStatus = CreateFMIResultsFromDef(fileDataset_ptr, aeSession_ptr);
    if (fmiStatus)
    {
        SetModuleResultsFromDcm(fileDataset_ptr->getFileMetaInformation(), true, false);
    }
	
    // Update the results structure with the rest of the media file.
    bool datasetStatus = CreateMediaResultsFromDef(fileDataset_ptr, aeSession_ptr);
    if (datasetStatus)
    {
        SetModuleResultsFromDcm(fileDataset_ptr->getDataset(), true, false);
    }

    // Validate the actual results structure (if available). Update the
    // results structure with the reference values.
    if ((datasetStatus) || 
        (fmiStatus))
    {
        if (refDataset_ptr != NULL)
        {
            SetModuleResultsFromDcm(refDataset_ptr, false, false);
        }

        string fileName = directoryM;
        if (directoryM[directoryM.length()-1] != '\\') fileName += "\\";
        fileName += fileM;

        // add the file suffix - if defined
        if (suffixM.length())
        {
            fileName += '.';
            fileName += suffixM;
        }
        ValidateResults(validationFlag);
    }

    CompareRecordUidsWithFile(record_ptr);
    CompareRecordAttributesWithFile(record_ptr);

    return datasetStatus;
}

//>>===========================================================================

RECORD_LINK_CLASS *MEDIA_VALIDATOR_CLASS::GetRecordLinkStructByOffset(UINT32 offset)

//  DESCRIPTION     : Get the record link by offset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (UINT i = 0; i < recordLinksM.size(); i++)
    {
        if (recordLinksM[i]->GetRecordOffset() == offset)
            return recordLinksM[i];
    }

    return NULL;
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::CheckDirectoryRecordLinks()

//  DESCRIPTION     : Check the directory record links.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (offsetLastDirRecordM != DICOMDIR_ILLEGAL_OFFSET)
    {
        RECORD_LINK_CLASS *recordLink_ptr = GetRecordLinkStructByOffset(offsetLastDirRecordM);
        if (recordLink_ptr)
        {
            // Is the record allowed as a child of root?
            // The root record type is defined as the 1st in the list of record types.
            UINT childRecordTypeIndex = RECORD_TYPES->GetRecordTypeIndex(recordLink_ptr->GetRecordType());
            if (!RECORD_TYPES->IsChildAllowedInParent(0, childRecordTypeIndex))
            {
                recordLink_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7,
                    "This record - which is on the highest level - is of an illegal type: " +
                    RECORD_TYPES->GetRecordTypeNameOfRecordTypeWithIdx(childRecordTypeIndex));
                // Even with such an error, we can continue with validation of
                // the DICOMDIR object.
            }

            // Check if the last directory record is really the last in the
            // DICOMDIR. In other words, the horizontal link to another record
            // must be 0!
            if (recordLink_ptr->GetHorLinkOffset() != 0)
            {
                recordLink_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7,
                    "This record is flagged as being the last. That means the offset for the next record must be 0.");
            }
        }
        else
        {
            char message[128];
            sprintf(message,
                "Could not find last directory record at offset: %ld",
                offsetLastDirRecordM);
            objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7, message);
        }
    }

    // Check the complete directory record chain. The first record must
    // be a valid record, if that's the case the entire chain can be
    // checked.
    if (offsetFirstDirRecordM != DICOMDIR_ILLEGAL_OFFSET)
    {
        RECORD_LINK_CLASS *recordLink_ptr = GetRecordLinkStructByOffset(offsetFirstDirRecordM);
        if (recordLink_ptr)
        {
            CheckDirChain(recordLink_ptr, 0);
            return true;
        }
        else
        {
            char message[128];
            sprintf(message,
                "Could not find first directory record at offset: %ld",
                offsetFirstDirRecordM);
            objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7, message);
            return false;
        }
    }
    return true;
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::CompareRecordUidsWithFile(RECORD_RESULTS_CLASS *record_ptr)

//  DESCRIPTION     : Compare the record UIDs with those in the referenced file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = NULL;
    BASE_VALUE_CLASS *uidInRecord_ptr = NULL;
    BASE_VALUE_CLASS *uidInFile_ptr = NULL;
    BASE_VALUE_CLASS *uidInDataset_ptr = NULL;
    char message[128];

    // Check the SOP class UIDs
    // Try to get all necessary attributes and their values.
    VAL_ATTRIBUTE_CLASS *uidInRecordAttr_ptr = record_ptr->GetAttributeByTag(TAG_REFERENCED_SOP_CLASS_UID_IN_FILE);
    if (uidInRecordAttr_ptr)
    {
        dcmAttr_ptr = uidInRecordAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInRecord_ptr = dcmAttr_ptr->GetValue();
        }
    }

    VAL_ATTRIBUTE_CLASS *uidInFileAttr_ptr = objectResultsM_ptr->GetAttributeResults(TAG_MEDIA_STORAGE_SOP_CLASS_UID);
    if (uidInFileAttr_ptr)
    {
        dcmAttr_ptr = uidInFileAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInFile_ptr = dcmAttr_ptr->GetValue();
        }
    }

    VAL_ATTRIBUTE_CLASS *uidInDatasetAttr_ptr = objectResultsM_ptr->GetAttributeResults(TAG_SOP_CLASS_UID);
    if (uidInDatasetAttr_ptr)
    {
        dcmAttr_ptr = uidInDatasetAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInDataset_ptr = dcmAttr_ptr->GetValue();
        }
    }

    // Compare SOP class uid of the record with the one in the file.
    if ((uidInRecord_ptr) && 
        (uidInDataset_ptr))
    {
        if (uidInRecord_ptr->Compare(NULL, uidInDataset_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referenced file IOD", 
                GroupFromTag(TAG_SOP_CLASS_UID),
				ElementFromTag(TAG_SOP_CLASS_UID));
            uidInRecordAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);

            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring record", 
                GroupFromTag(TAG_REFERENCED_SOP_CLASS_UID_IN_FILE),
				ElementFromTag(TAG_REFERENCED_SOP_CLASS_UID_IN_FILE));
            uidInDatasetAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }

    // Compare SOP class uid of the file meta information with the one in the file.
    if ((uidInFile_ptr) &&
        (uidInDataset_ptr))
    {
        if (uidInFile_ptr->Compare(NULL, uidInDataset_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referenced file IOD", 
                GroupFromTag(TAG_SOP_CLASS_UID),
				ElementFromTag(TAG_SOP_CLASS_UID));
            uidInFileAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);

            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring file meta header", 
                GroupFromTag(TAG_MEDIA_STORAGE_SOP_CLASS_UID),
				ElementFromTag(TAG_MEDIA_STORAGE_SOP_CLASS_UID));
            uidInDatasetAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }

    // Compare SOP class uid of the file meta information with the one in the record.
    if ((uidInFile_ptr) &&
        (uidInRecord_ptr))
    {
        if (uidInFile_ptr->Compare(NULL, uidInRecord_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring record", 
                GroupFromTag(TAG_REFERENCED_SOP_CLASS_UID_IN_FILE),
				ElementFromTag(TAG_REFERENCED_SOP_CLASS_UID_IN_FILE));
            uidInFileAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);

            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring file meta header", 
                GroupFromTag(TAG_MEDIA_STORAGE_SOP_CLASS_UID),
				ElementFromTag(TAG_MEDIA_STORAGE_SOP_CLASS_UID));
            uidInRecordAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }

    // Check the SOP instance UIDs
    uidInRecord_ptr = NULL;
    uidInFile_ptr = NULL;
    uidInDataset_ptr = NULL;

    // Try to get all necessary attributes and their values.
    uidInRecordAttr_ptr = record_ptr->GetAttributeByTag(TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE);
    if (uidInRecordAttr_ptr)
    {
        dcmAttr_ptr = uidInRecordAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInRecord_ptr = dcmAttr_ptr->GetValue();
        }
    }

    uidInFileAttr_ptr = objectResultsM_ptr->GetAttributeResults(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID);
    if (uidInFileAttr_ptr)
    {
        dcmAttr_ptr = uidInFileAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInFile_ptr = dcmAttr_ptr->GetValue();
        }
    }

    uidInDatasetAttr_ptr = objectResultsM_ptr->GetAttributeResults(TAG_SOP_INSTANCE_UID);
    if (uidInDatasetAttr_ptr)
    {
        dcmAttr_ptr = uidInDatasetAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInDataset_ptr = dcmAttr_ptr->GetValue();
        }
    }

    // Compare SOP instance uid of the record with the one in the file.
    if ((uidInRecord_ptr) &&
        (uidInDataset_ptr))
    {
        if (uidInRecord_ptr->Compare(NULL, uidInDataset_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referenced file IOD", 
                GroupFromTag(TAG_SOP_INSTANCE_UID),
				ElementFromTag(TAG_SOP_INSTANCE_UID));
            uidInRecordAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);

            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring record", 
                GroupFromTag(TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE),
				ElementFromTag(TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE));
            uidInDatasetAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }

    // Compare SOP instance uid of the file meta information with the one in the file.
    if ((uidInFile_ptr) &&
        (uidInDataset_ptr))
    {
        if (uidInFile_ptr->Compare(NULL, uidInDataset_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referenced file IOD", 
                GroupFromTag(TAG_SOP_INSTANCE_UID),
				ElementFromTag(TAG_SOP_INSTANCE_UID));
            uidInFileAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);

            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring file meta header", 
                GroupFromTag(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID),
				ElementFromTag(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID));
            uidInDatasetAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }

    // Compare SOP instance uid of the file meta information with the one in the record.
    if ((uidInFile_ptr) &&
        (uidInRecord_ptr))
    {
        if (uidInFile_ptr->Compare(NULL, uidInRecord_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring record",
                GroupFromTag(TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE),
				ElementFromTag(TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE));
            uidInFileAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);

            sprintf(message, 
                "UID mismatch with tag (%04X,%04X) in referring file meta header", 
                GroupFromTag(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID),
				ElementFromTag(TAG_MEDIA_STORAGE_SOP_INSTANCE_UID));
            uidInRecordAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }

    // Check the Transfer Syntax UIDs between the record and file meta header.
    // Try to get all necessary attributes and their values.
    uidInRecordAttr_ptr = record_ptr->GetAttributeByTag(TAG_REFERENCED_TRANSFER_SYNTAX_UID_IN_FILE);
    if (uidInRecordAttr_ptr)
    {
        dcmAttr_ptr = uidInRecordAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInRecord_ptr = dcmAttr_ptr->GetValue();
        }
    }

    uidInFileAttr_ptr = objectResultsM_ptr->GetAttributeResults(TAG_TRANSFER_SYNTAX_UID);
    if (uidInFileAttr_ptr)
    {
        dcmAttr_ptr = uidInFileAttr_ptr->GetDcmAttribute();
        if (dcmAttr_ptr)
        {
            uidInFile_ptr = dcmAttr_ptr->GetValue();
        }
    }

    // Compare transfer syntax uid of the file meta information with the one in the record.
    if ((uidInFile_ptr) && 
        (uidInRecord_ptr))
    {
        if (uidInFile_ptr->Compare(NULL, uidInRecord_ptr) != MSG_EQUAL)
        {
            sprintf(message, 
                "Transfer syntax mismatch with tag (%04X,%04X) in referring record",
                GroupFromTag(TAG_REFERENCED_TRANSFER_SYNTAX_UID_IN_FILE),
				ElementFromTag(TAG_REFERENCED_TRANSFER_SYNTAX_UID_IN_FILE));
            uidInFileAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);

            sprintf(message, 
                "Transfer syntax mismatch with tag (%04X,%04X) in referring file meta header", 
                GroupFromTag(TAG_TRANSFER_SYNTAX_UID),
				ElementFromTag(TAG_TRANSFER_SYNTAX_UID));
            uidInRecordAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
        }
    }
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::CompareRecordAttributesWithFile(RECORD_RESULTS_CLASS *recordResults_ptr)

//  DESCRIPTION     : Compare the record attributes with those in the referenced file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{    
    char message[256];

    int index = 0;
    VAL_ATTRIBUTE_CLASS *valRecordAttr_ptr = recordResults_ptr->GetAttribute(index);
    while (valRecordAttr_ptr != NULL)
    {
        DCM_ATTRIBUTE_CLASS *dcmRecordAttr_ptr = valRecordAttr_ptr->GetDcmAttribute();
        if (dcmRecordAttr_ptr != NULL)
        {
            UINT16 group = dcmRecordAttr_ptr->GetMappedGroup();
            UINT16 element = dcmRecordAttr_ptr->GetMappedElement();
            UINT32 tag = ((UINT32) (group) << 16) + ((UINT32)element);
            ATTR_VR_ENUM vr = dcmRecordAttr_ptr->GetVR();
            int recordVm = dcmRecordAttr_ptr->GetNrValues();

            // Skip attributes with a group < 0x0008 or a specific tag.
            if ((group > 0x0008) && 
                (!SkipAttribute(tag)))
            {
                VAL_ATTRIBUTE_CLASS *valFileAttr_ptr = objectResultsM_ptr->GetAttributeResults(group, element);
                if (valFileAttr_ptr == NULL)
                {
                    // Make special checks if the attribute is allowed to
                    // be absent in the file.
                    if ( !( // Private recognition strings allowed to be absent.
                           (vr == ATTR_VR_LO) &&
                           ((tag & 0x10000) != 0) &&
                           ((tag & 0xffff) >= 0x10) &&
                           ((tag & 0xffff) <= 0xff)
                          ) &&
                         !( // Group lengths allowed to be absent.
                           (vr == ATTR_VR_UL) &&
                           ((tag & 0xffff) == 0)
                          )
                       )
                    {
			            sprintf(message,
							"An attribute with tag (%04X,%04X) has not been found in the referred file",
			                GroupFromTag(tag),
							ElementFromTag(tag));
                        valRecordAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_5, message);
                    }
                }
                else
                {
                    int fileVm = valFileAttr_ptr->GetNrValues();
                    // Compare both attributes.

                    // We assume that zero length record tags always match
                    // the referenced values.
                    if (recordVm > 0)
                    {
                        if (recordVm != fileVm)
                        {
                            // Value multiplicity inconsistency, log in both record and referring file.
                            sprintf(message,
                                "Value multiplicity mismatch %ld != %ld of attribute (%04X,%04X) in referenced file",
                                recordVm, 
                                fileVm, 
				                GroupFromTag(tag),
								ElementFromTag(tag));
                            valRecordAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_5, message);
                            sprintf(message,
                                "Value multiplicity mismatch %ld != %ld of attribute (%04X,%04X) in referring record",
                                fileVm, 
                                recordVm, 
				                GroupFromTag(tag),
								ElementFromTag(tag));
                            valFileAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_5, message);
                        }
                        else
                        {
                            DVT_STATUS status = valRecordAttr_ptr->GetDcmAttribute()->Compare(valFileAttr_ptr->GetMessages(), valFileAttr_ptr->GetDcmAttribute());
                            if ((status != MSG_EQUAL) &&
                                (status != MSG_OK))
                            {
                                sprintf(message,
                                    "Value mismatch for attribute (%04X,%04X) with corresponding tag in referring record",
						            GroupFromTag(tag),
									ElementFromTag(tag));
                                valFileAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_4, message);
                            }
                        }
                    }
                }
            }
        }
        index++;
        valRecordAttr_ptr = recordResults_ptr->GetAttribute(index);
    }
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::CheckDirChain(RECORD_LINK_CLASS *recordLink_ptr,
                                      UINT parentRecordTypeIndex)

//  DESCRIPTION     : Check the directory chain.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // This record occurs in the directory chain, so obviously it is referenced.
    recordLink_ptr->IncreaseReferenceCount();

//	recordLink_ptr->Dump();

    // If the record has been referenced multiple times, return immediately.
    // If we wouldn't return, this would result in an endless loop.
    // The logger will display this as an error.
    if (recordLink_ptr->GetReferenceCount() > 1)
    {
        return;
    }

    // Get the record index in the list of Record Types.
    UINT recordIndex = RECORD_TYPES->GetRecordTypeIndex(recordLink_ptr->GetRecordType());

    // We only need to validate the directory chain of the given record, if
    // the record is 'in use'. This only applies to the downlink records, the
    // horlink records need to be validated in all cases.
    if (recordLink_ptr->GetInUseFlag())
    {
        if (!RECORD_TYPES->IsChildAllowedInParent(parentRecordTypeIndex, recordIndex))
        {
            recordLink_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7,
                "This record can not be a child of a record with type " +
                RECORD_TYPES->GetRecordTypeNameOfRecordTypeWithIdx(parentRecordTypeIndex));
        }

        CheckDownLink(recordLink_ptr, recordIndex);
    }

    CheckHorLink(recordLink_ptr, parentRecordTypeIndex);
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::CheckDownLink(RECORD_LINK_CLASS *recordLink_ptr,
                                      UINT parentRecordTypeIndex)

//  DESCRIPTION     : Check the vertical (down) record links.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    UINT32 offset = recordLink_ptr->GetDownLinkOffset();

    // If there is a reference to a record via the 'downlink', validate
    // that record.
    if ((offset != DICOMDIR_ILLEGAL_OFFSET) &&
        (offset != 0))
    {
        RECORD_LINK_CLASS *childRecord_ptr = GetRecordLinkStructByOffset(offset);
        if (childRecord_ptr)
        {
            CheckDirChain(childRecord_ptr, parentRecordTypeIndex);
        }
        else
        {
            recordLink_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7,
                "Downlink record offset value points to a non-existing record.");
        }
    }
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::CheckHorLink(RECORD_LINK_CLASS *recordLink_ptr,
                                     UINT parentRecordTypeIndex)

//  DESCRIPTION     : Check the horizontal record links.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    UINT32 offset = recordLink_ptr->GetHorLinkOffset();

    while ((offset != DICOMDIR_ILLEGAL_OFFSET) &&
        (offset != 0))
    {
        RECORD_LINK_CLASS *peerRecord_ptr = GetRecordLinkStructByOffset(offset);
        if (peerRecord_ptr)
        {
		    // This record occurs in the directory chain, so obviously it is referenced.
			peerRecord_ptr->IncreaseReferenceCount();

//			peerRecord_ptr->Dump();

			// If the record is being referenced for the first time - validate it
			// with respect to its parent.
			if (peerRecord_ptr->GetReferenceCount() == 1)
			{
				// Get the record index in the list of Record Types.
				UINT recordIndex = RECORD_TYPES->GetRecordTypeIndex(peerRecord_ptr->GetRecordType());

				// We only need to validate the directory chain of the given record, if
				// the record is 'in use'. This only applies to the downlink records, the
				// horlink records need to be validated in all cases.
				if (peerRecord_ptr->GetInUseFlag())
				{
					if (!RECORD_TYPES->IsChildAllowedInParent(parentRecordTypeIndex, recordIndex))
					{
						peerRecord_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7,
							"This record can not be a child of a record with type " +
							RECORD_TYPES->GetRecordTypeNameOfRecordTypeWithIdx(parentRecordTypeIndex));
					}

					CheckDownLink(peerRecord_ptr, recordIndex);
				}
			}

			// Move to next peer record
			offset = peerRecord_ptr->GetHorLinkOffset();
        }
        else
        {
            recordLink_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7,
                "Next record offset value points to a non-existing record");

			// break out of loop
			offset = 0;
        }
    }
}

//>>===========================================================================

void MEDIA_VALIDATOR_CLASS::ExtractFilename(string filename)

//  DESCRIPTION     : This function parses a given filename and extracts the
//                    directory, filename (without extention) and file
//                    extention.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int suffixPos = 0;

    // Search the filename for the last occurance of a dot '.'
    // If a '/' or a '\' is found before a '.' has been found, the filename
    // contains no suffix.
    for (int i = filename.length() - 1; i >= 0; i--)
    {
        if (filename[i] == '.')
        {
            // Extract the file extention.
            suffixM = filename.substr(i+1, filename.length());
            suffixPos = i-1;
            i = -1;
        }
        else if ((filename[i] == '/') || 
            (filename[i] == '\\'))
        {
            // No '.' character found in the filename.
            i = -1;
        }
    }

    // Find the last '\' or '/', if any
    int prefixPos = filename.length() - 1;
    while ((prefixPos >= 0) && 
        ((filename[prefixPos] != '/') && 
         (filename[prefixPos] != '\\')))
    {
        prefixPos--;
    }

    // Extract the directory name - if applicable
    if (prefixPos > 0)
    {
        directoryM = filename.substr(0, prefixPos);
        prefixPos++;
    }
    else
    {
        directoryM = "";
    }

    // Extract the filename without file extention.
    int length = filename.length();
    fileM = filename.substr(prefixPos, length - prefixPos - (length - suffixPos) + 1);
}

//>>===========================================================================

bool MEDIA_VALIDATOR_CLASS::SkipAttribute(UINT32 tag)

//  DESCRIPTION     : Determine if the attribute should be skipped based on it's Tag value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool skip = true;

    switch(tag)
    {
    case TAG_STUDY_DESCRIPTION:         // Only in study records, not in study files.
    case TAG_ICON_IMAGE:                // Only in image records, not in image files.
    case TAG_CD_MEDICAL_FILE_LOCATION:
    case TAG_CD_MEDICAL_FILE_SIZE:
        skip = true;
        break;
    default:
        skip = false;
        break;
    }
    return skip;
}
