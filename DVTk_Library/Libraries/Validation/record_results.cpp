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
//  DESCRIPTION     :   Record results class for validation.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "media_validator.h"
#include "record_results.h"
#include "record_types.h"
#include "record_uid.h"
#include "uid_defining.h"
#include "uid_referring.h"
#include "val_attribute.h"
#include "val_base_value.h"
#include "val_object_results.h"

#include "Iglobal.h"            // Global interface file
#include "IAttributeGroup.h"    // Attribute Group interface file
#include "Idicom.h"             // Dicom component interface file
#include "Ilog.h"               // Log component interface file
#include "Imedia.h"             // Media component interface

//>>===========================================================================

RECORD_RESULTS_CLASS::RECORD_RESULTS_CLASS(UINT32 index)

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    referenceFileM = "";
    recordIndexM = index;
    referenceFileResultsM_ptr = NULL;
    fileDatasetM_ptr = NULL;
}

//>>===========================================================================

RECORD_RESULTS_CLASS::~RECORD_RESULTS_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    delete referenceFileResultsM_ptr;
    delete fileDatasetM_ptr;
}

//>>===========================================================================

void RECORD_RESULTS_CLASS::Validate(string rootDirLocation, bool validateReferencedFile)

//  DESCRIPTION     : Performs validation on the record. We don't do regular
//                    validation of the record (against def, vr and ref).
//                    For that validation, we call the normal validation
//                    function.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Check the filename attribute in the record. Return the filename in
    // a string.
	if(validateReferencedFile)
	{
		referenceFileM = GetAndCheckRefFileName(rootDirLocation);

		// Check if the file exists on the medium.
		if (referenceFileM != "")
		{
			CheckFileExistance(referenceFileM);
		}
	}

    // Check if the record is retired.
    CheckRetiredRecords();
}

//>>===========================================================================

void RECORD_RESULTS_CLASS::BuildPersistentInfo(RECORD_UID_CLASS *uidLinks_ptr)

//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Extract record UIDS from the DICOMDIR record
    ExtractRecordUids(GetDcmAttributeGroup(), uidLinks_ptr);
}

//>>===========================================================================

bool RECORD_RESULTS_CLASS::CheckRetiredRecords()

//  DESCRIPTION     : Checks if the record is retired. If that's the case, a
//                    new validation message is added to the record.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool result = true;

    UINT index = RECORD_TYPES->GetRecordTypeIndex(GetRecordType());
    if (RECORD_TYPES->IsRecordRetired(index))
    {
		GetMessages()->AddMessage(VAL_RULE_A_MEDIA_7,
            "Record type \"" +
            RECORD_TYPES->GetRecordTypeNameOfRecordTypeWithIdx(index) +
            "\" is retired.");
        result = false;
    }
    return result;
}

//>>===========================================================================

DICOMDIR_RECORD_TYPE_ENUM RECORD_RESULTS_CLASS::GetRecordType()

//  DESCRIPTION     : Returns the record type of the DICOMDIR record.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    VAL_ATTRIBUTE_CLASS *valAttr_ptr = GetAttributeByTag(TAG_DIRECTORY_RECORD_TYPE);
    if (valAttr_ptr != NULL)
    {
        DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = valAttr_ptr->GetDcmAttribute ();
        if (dcmAttr_ptr != NULL)
        {
            string recordTypeName;
            DVT_STATUS status = dcmAttr_ptr->GetValue()->Get(recordTypeName);
            if (status == MSG_OK)
            {
                return RECORD_TYPES->GetRecordTypeIndexWithRecordName(dcmAttr_ptr->GetValue());
            }
        }
    }

    return DICOMDIR_RECORD_TYPE_UNKNOWN;
}

//>>===========================================================================

bool RECORD_RESULTS_CLASS::ValidateReferenceFile(RECORD_UID_CLASS *uidLinks_ptr,
		VALIDATION_CONTROL_FLAG_ENUM validationFlag,
        AE_SESSION_CLASS *aeSession_ptr,
        LOG_CLASS *logger_ptr,
        BASE_SERIALIZER* serializer_ptr,
        STORAGE_MODE_ENUM storageMode,
		bool dumpAttributesOfRefFiles)

//  DESCRIPTION     : This function creates a new validator to validate the
//                    reference file associated with this record.
//                    If possible, a reference object is added for additional
//                    validation.
//  PRECONDITIONS   : referenceFileM != ""
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

    assert (referenceFileM != "");

    fileDatasetM_ptr = new FILE_DATASET_CLASS(referenceFileM);

    // Cascade the logger
    fileDatasetM_ptr->setLogger(logger_ptr);

    // Set the storage mode
    fileDatasetM_ptr->setStorageMode(storageMode);

    // Read the dataset from the file.
    if (!fileDatasetM_ptr->read())
    {
        GetMessages()->AddMessage(VAL_RULE_A_MEDIA_9, "Can't read referenced file " + referenceFileM);
        result = false;		
    }
    else
    {
        // Try to get the reference dataset from the warehouse
        // based on the stored reference tag.
        DCM_DATASET_CLASS *refDataset_ptr = NULL;
        DCM_DATASET_CLASS *dataset_ptr = fileDatasetM_ptr->getDataset();

        // Only validate against a reference object when the option is set.
        if (validationFlag & USE_REFERENCE)
        {
            if (dataset_ptr != NULL)
            {
                if (dataset_ptr->setIdentifierByTag(WAREHOUSE->getReferenceTag()))
                {
                    string identifier = dataset_ptr->getIdentifier();
                    BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(identifier, WID_DATASET);
                    if (wid_ptr != NULL)
                    {
                        refDataset_ptr = static_cast<DCM_DATASET_CLASS*>(wid_ptr);
                        GetMessages()->AddMessage(VAL_RULE_A_MEDIA_A,
                            "Reference dataset with identifier \"" +
                            identifier + "\" found in data warehouse");
                    }
                }
            }
        }

        // Validate the reference file.
        MEDIA_VALIDATOR_CLASS *validator_ptr = new MEDIA_VALIDATOR_CLASS(referenceFileM);

        result = validator_ptr->ValidateRefFile(fileDatasetM_ptr, this, refDataset_ptr, validationFlag, aeSession_ptr);
		
        ExtractFileUids(fileDatasetM_ptr, uidLinks_ptr);

        validator_ptr->CopyObjectResults(&referenceFileResultsM_ptr);

        delete validator_ptr;
    }

	//Provide message dump in case of failure
	if((!result) || dumpAttributesOfRefFiles)
	{
		// serialize the fileDataset (as display)
		if (serializer_ptr)
		{
			serializer_ptr->SerializeDisplay(fileDatasetM_ptr);
		}
	}

	return result;
}

//>>===========================================================================

void RECORD_RESULTS_CLASS::ExtractRecordUids(DCM_ATTRIBUTE_GROUP_CLASS *record_ptr,
                                         RECORD_UID_CLASS *uidLinks_ptr)

//  DESCRIPTION     : Extract the record uids.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // For the supported record types, find and remember all
    // defining and referring UIDs.
    switch (GetRecordType())
    {
    case DICOMDIR_RECORD_TYPE_PATIENT:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_PATIENT_RECORD);
        uidLinks_ptr->InstallReferringUid(record_ptr, UID_REFERRING_PATIENT_RECORD);
        break;
    case DICOMDIR_RECORD_TYPE_STUDY:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_STUDY_RECORD);
        uidLinks_ptr->InstallReferringUid(record_ptr, UID_REFERRING_STUDY_RECORD);
        break;
    case DICOMDIR_RECORD_TYPE_SERIES:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_SERIES_RECORD);
        break;
    case DICOMDIR_RECORD_TYPE_IMAGE:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_IMAGE_RECORD);
        uidLinks_ptr->InstallReferringUid(record_ptr, UID_REFERRING_IMAGE_RECORD);
        break;
    case DICOMDIR_RECORD_TYPE_PRIVATE:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_XA_PRIVATE_RECORD);
        uidLinks_ptr->InstallReferringUid(record_ptr, UID_REFERRING_XA_PRIVATE_RECORD);
        break;
    case DICOMDIR_RECORD_TYPE_VISIT:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_VISIT_RECORD);
        uidLinks_ptr->InstallReferringUid(record_ptr, UID_REFERRING_VISIT_RECORD);
        break;
    case DICOMDIR_RECORD_TYPE_STUDY_COMPONENT:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_STUDY_COMPONENT_RECORD);
        uidLinks_ptr->InstallReferringUid(record_ptr, UID_REFERRING_STUDY_COMPONENT_RECORD);
        break;
    case DICOMDIR_RECORD_TYPE_UNKNOWN:
        uidLinks_ptr->InstallDefiningUid(record_ptr, UID_DEFINING_UNKNOWN_RECORD);
        uidLinks_ptr->InstallReferringUid(record_ptr, UID_REFERRING_UNKNOWN_RECORD);
        break;
    default:
        break;
    }
}

//>>===========================================================================

bool RECORD_RESULTS_CLASS::ExtractFileUids(FILE_DATASET_CLASS *fileDataset_ptr,
                                       RECORD_UID_CLASS *uidLinks_ptr)

//  DESCRIPTION     : Extract the file uids.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool status = true;
    MEDIA_FILE_HEADER_CLASS *fileMetaInfo_ptr = fileDataset_ptr->getFileMetaInformation();
    DCM_DATASET_CLASS *dataset_ptr = fileDataset_ptr->getDataset();

    switch (GetRecordType())
    {
    case DICOMDIR_RECORD_TYPE_PATIENT:
        status = uidLinks_ptr->InstallDefiningUid(dataset_ptr, UID_DEFINING_PATIENT_FILE);
        if (!status)
        {
            status = uidLinks_ptr->InstallDefiningUid(fileMetaInfo_ptr, UID_DEFINING_PATIENT_FILE);
        }
        uidLinks_ptr->InstallReferringUid(dataset_ptr, UID_REFERRING_PATIENT_FILE);
        break;
    case DICOMDIR_RECORD_TYPE_STUDY:
        status = uidLinks_ptr->InstallDefiningUid(dataset_ptr, UID_DEFINING_STUDY_FILE);
        if (!status)
        {
            status = uidLinks_ptr->InstallDefiningUid(fileMetaInfo_ptr, UID_DEFINING_STUDY_FILE);
        }
        uidLinks_ptr->InstallReferringUid(dataset_ptr, UID_REFERRING_STUDY_FILE);
        break;
    case DICOMDIR_RECORD_TYPE_IMAGE:
        status = uidLinks_ptr->InstallDefiningUid(dataset_ptr, UID_DEFINING_IMAGE_FILE);
        if (!status)
        {
            status = uidLinks_ptr->InstallDefiningUid(fileMetaInfo_ptr, UID_DEFINING_IMAGE_FILE);
        }
        uidLinks_ptr->InstallReferringUid(dataset_ptr, UID_REFERRING_IMAGE_FILE);
        break;
    case DICOMDIR_RECORD_TYPE_PRIVATE:
        status = uidLinks_ptr->InstallDefiningUid(dataset_ptr, UID_DEFINING_XA_PRIVATE_FILE);
        if (!status)
        {
            status = uidLinks_ptr->InstallDefiningUid(fileMetaInfo_ptr, UID_DEFINING_XA_PRIVATE_FILE);
        }
        uidLinks_ptr->InstallReferringUid(dataset_ptr, UID_REFERRING_XA_PRIVATE_FILE);
        break;
    case DICOMDIR_RECORD_TYPE_VISIT:
        status = uidLinks_ptr->InstallDefiningUid(dataset_ptr, UID_DEFINING_VISIT_FILE);
        if (!status)
        {
            status = uidLinks_ptr->InstallDefiningUid(fileMetaInfo_ptr, UID_DEFINING_VISIT_FILE);
        }
        uidLinks_ptr->InstallReferringUid(dataset_ptr, UID_REFERRING_VISIT_FILE);
        break;
    case DICOMDIR_RECORD_TYPE_STUDY_COMPONENT:
        status = uidLinks_ptr->InstallDefiningUid(dataset_ptr, UID_DEFINING_STUDY_COMPONENT_FILE);
        if (!status)
        {
            status = uidLinks_ptr->InstallDefiningUid(fileMetaInfo_ptr, UID_DEFINING_STUDY_COMPONENT_FILE);
        }
        uidLinks_ptr->InstallReferringUid(dataset_ptr, UID_REFERRING_STUDY_COMPONENT_FILE);
        break;
    default:
        status = false;
        break;
    }
    return status;
}

//>>===========================================================================

string RECORD_RESULTS_CLASS::GetAndCheckRefFileName(string rootDirLocation)

//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string refFilename = "";

    VAL_ATTRIBUTE_CLASS *valAttr_ptr = GetAttributeByTag (TAG_REFERENCED_FILE_ID);
    if (valAttr_ptr == NULL)
    {
        return "";         // Validation attribute not present.
    }

    if (valAttr_ptr->GetDcmAttribute() == NULL)
    {
        return "";         // Attribute not present in the DICOMDIR.
    }

    for (int i = 0; i < valAttr_ptr->GetNrValues(); i++)
    {
        VAL_BASE_VALUE_CLASS *valValue_ptr = valAttr_ptr->GetValue(i);
        if (valValue_ptr != NULL)
        {
            BASE_VALUE_CLASS *dcmValue_ptr = valValue_ptr->GetDcmValue();
            if (dcmValue_ptr != NULL)
            {
                string filename;
                if (dcmValue_ptr->Get(filename, true) == MSG_OK)
                {
                    if (filename.length() > MEDIA_NR_CHARS_OF_FILEPART)
                    {
                        char message[128];

                        sprintf(message,
								"Filename part length of %d characters exceeds maximum length of %d",
                                filename.length(), 
								MEDIA_NR_CHARS_OF_FILEPART);
                        valValue_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_1, message);
                    }

                    if (i > 0)
                    {
                        // This is not the first part of the filename. Add a slash '/'.
                        refFilename += PATH_SEP;
                    }
                    refFilename += filename;
                }
            }
        }
    }

    if (valAttr_ptr->GetNrValues() > MEDIA_NR_OF_FILE_PARTS)
    {
        char message[128];

        sprintf(message, 
				"Reference filename has too many values (%d, max %d)",
                valAttr_ptr->GetNrValues(), 
				MEDIA_NR_OF_FILE_PARTS);
        valAttr_ptr->GetMessages()->AddMessage(VAL_RULE_A_MEDIA_1, message);
    }

    string filename = rootDirLocation;
    if (rootDirLocation[rootDirLocation.length()-1] != '\\')
    {
        filename += "\\";
    }
    filename += refFilename;

    return filename;
}

//>>===========================================================================

bool RECORD_RESULTS_CLASS::CheckFileExistance(string filename)

//  DESCRIPTION     : Check the file existance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Add a special check when the file is not found. For some
//                    record types, it's not an error when the referenced file
//                    is not on the same media.
//<<===========================================================================
{
    if (filename.length() == 0) return false;

    FILE *fd_ptr = fopen(filename.c_str(), "rb");

    if (fd_ptr == NULL)
    {
        GetMessages()->AddMessage(VAL_RULE_A_MEDIA_2,
            "Cannot open referenced file \"" +
            filename + "\"");
        return false;
    }

    fclose (fd_ptr);

    return true;
}

//>>===========================================================================

string RECORD_RESULTS_CLASS::GetRefFileName(void)

//  DESCRIPTION     : Get the filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return referenceFileM;
}

//>>===========================================================================

UINT32 RECORD_RESULTS_CLASS::GetRecordIndex(void)

//  DESCRIPTION     : Get the record index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return recordIndexM;
}

//>>===========================================================================

VAL_OBJECT_RESULTS_CLASS *RECORD_RESULTS_CLASS::GetRefFileResults()

//  DESCRIPTION     : Get the referenced file results.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return referenceFileResultsM_ptr;
}
