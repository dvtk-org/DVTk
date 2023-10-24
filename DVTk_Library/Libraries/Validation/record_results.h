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

#ifndef RECORD_RESULTS_H
#define RECORD_RESULTS_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "val_attribute_group.h"
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class AE_SESSION_CLASS;
class LOG_MESSAGE_CLASS;
class LOG_CLASS;
class RECORD_UID_CLASS;

//>>***************************************************************************

class RECORD_RESULTS_CLASS : public VAL_ATTRIBUTE_GROUP_CLASS

//  DESCRIPTION     : Record Results Class
//  NOTES           :
//<<***************************************************************************
{
    public:
        RECORD_RESULTS_CLASS(UINT32);
        virtual ~RECORD_RESULTS_CLASS();

        void Validate(string, bool);

        bool ValidateReferenceFile(RECORD_UID_CLASS*,
				VALIDATION_CONTROL_FLAG_ENUM,
                AE_SESSION_CLASS*,
                LOG_CLASS*,
                BASE_SERIALIZER*,
                STORAGE_MODE_ENUM,
				bool);

        void BuildPersistentInfo(RECORD_UID_CLASS*);

        DICOMDIR_RECORD_TYPE_ENUM GetRecordType();

        string GetRefFileName();

        UINT32 GetRecordIndex();

        VAL_OBJECT_RESULTS_CLASS *GetRefFileResults();

	private:
        string referenceFileM;
        UINT32 recordIndexM;
        VAL_OBJECT_RESULTS_CLASS *referenceFileResultsM_ptr;
        FILE_DATASET_CLASS *fileDatasetM_ptr;

        string GetAndCheckRefFileName(string);

        bool CheckRetiredRecords();

        bool CheckUnreferencedRecords();

        bool CheckFileExistance(string);

        void ExtractRecordUids(DCM_ATTRIBUTE_GROUP_CLASS*, RECORD_UID_CLASS*);

        bool ExtractFileUids(FILE_DATASET_CLASS*, RECORD_UID_CLASS*);
};

#endif /* RECORD_RESULTS_H */
