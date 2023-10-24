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

#ifndef MEDIA_VALIDATOR_H
#define MEDIA_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"
#include "validator.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class AE_SESSION_CLASS;
class DCM_DIR_DATASET_CLASS;
class DCM_ITEM_CLASS;
class FILE_DATASET_CLASS;
class LOG_MESSAGE_CLASS;
class LOG_CLASS;
class MEDIA_FILE_HEADER_CLASS;
class RECORD_LINK_CLASS;
class RECORD_RESULTS_CLASS;
class RECORD_UID_CLASS;
class BASE_SERIALIZER;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
typedef vector<RECORD_LINK_CLASS*> RECORD_LINK_VECTOR;

//>>***************************************************************************

class MEDIA_VALIDATOR_CLASS : public VALIDATOR_CLASS

//  DESCRIPTION     : Media Validator Class
//  NOTES           :
//<<***************************************************************************
{
    public:
        MEDIA_VALIDATOR_CLASS();
        MEDIA_VALIDATOR_CLASS(string);
        virtual ~MEDIA_VALIDATOR_CLASS();

        bool CreateFMIResultsFromDef(FILE_DATASET_CLASS*, AE_SESSION_CLASS*);

        bool CreateMediaResultsFromDef(FILE_DATASET_CLASS*, AE_SESSION_CLASS*);

        bool Validate(FILE_DATASET_CLASS*,
				DCM_DATASET_CLASS*,
                RECORD_UID_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*,
                AE_SESSION_CLASS*);

        bool Validate(DCM_DIR_DATASET_CLASS*,
                DCM_DATASET_CLASS*,
                MEDIA_FILE_HEADER_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                AE_SESSION_CLASS*,
                DEF_ATTRIBUTE_GROUP_CLASS**);

        bool ValidateRecord(DCM_ITEM_CLASS*,
                DCM_DATASET_CLASS*,
                DEF_ATTRIBUTE_GROUP_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                AE_SESSION_CLASS*,
                LOG_CLASS*,
                BASE_SERIALIZER*,
                UINT32,
                STORAGE_MODE_ENUM,
				bool,
				bool);

        bool ValidateRefFile(FILE_DATASET_CLASS*,
                RECORD_RESULTS_CLASS*,
                DCM_DATASET_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                AE_SESSION_CLASS*);

        bool ValidateRecordReferences();

        void CleanResults();

        void CopyObjectResults(VAL_OBJECT_RESULTS_CLASS**);

        void Serialize(BASE_SERIALIZER*);

    private:
        RECORD_LINK_VECTOR  recordLinksM;
        UINT32 offsetLastDirRecordM;
        UINT32 offsetFirstDirRecordM;
        RECORD_UID_CLASS *uidLinksM_ptr;
        string directoryM;
        string fileM;
        string suffixM;

        bool CreateDicomDirResultsFromDef(DCM_DIR_DATASET_CLASS*,
				MEDIA_FILE_HEADER_CLASS*,
                AE_SESSION_CLASS*,
                DEF_ATTRIBUTE_GROUP_CLASS **);

        void SetRecordResultsFromDcm(DCM_ATTRIBUTE_GROUP_CLASS*,
				VAL_ATTRIBUTE_GROUP_CLASS*,
                RECORD_LINK_CLASS*);

        bool CheckDirectoryRecordLinks();
        
		RECORD_LINK_CLASS *GetLastDirectoryRecord();
        
		RECORD_LINK_CLASS *GetFirstDirectoryRecord();
        
		RECORD_LINK_CLASS *GetRecordLinkStructByOffset(UINT32);
        
		void CheckDirChain(RECORD_LINK_CLASS*, UINT);

        void CheckDownLink(RECORD_LINK_CLASS*, UINT);

        void CheckHorLink(RECORD_LINK_CLASS*, UINT);

        void ExtractFilename(string);

        void CompareRecordUidsWithFile(RECORD_RESULTS_CLASS*);

        void CompareRecordAttributesWithFile(RECORD_RESULTS_CLASS*);

        bool SkipAttribute(UINT32);
};

#endif /* MEDIA_VALIDATOR_H */
