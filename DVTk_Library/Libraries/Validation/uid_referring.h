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

#ifndef UID_REFERRING_H
#define UID_REFERRING_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
typedef struct UID_REFERENCE_STRUCT
{
    UINT32 sequenceTag;
    UINT32 uidTag;
    RECORD_IOD_TYPE_ENUM uidType;
} UID_REFERENCE_STRUCT;

#define UID_REFERRING_PATIENT_FILE UID_REFERRING_CLASS::instancePatientFile()
#define UID_REFERRING_PATIENT_RECORD UID_REFERRING_CLASS::instancePatientRecord()
#define UID_REFERRING_STUDY_FILE UID_REFERRING_CLASS::instanceStudyFile()
#define UID_REFERRING_STUDY_RECORD UID_REFERRING_CLASS::instanceStudyRecord()
#define UID_REFERRING_VISIT_FILE UID_REFERRING_CLASS::instanceVisitFile()
#define UID_REFERRING_VISIT_RECORD UID_REFERRING_CLASS::instanceVisitRecord()
#define UID_REFERRING_STUDY_COMPONENT_FILE UID_REFERRING_CLASS::instanceStudyComponentFile()
#define UID_REFERRING_STUDY_COMPONENT_RECORD UID_REFERRING_CLASS::instanceStudyComponentRecord()
#define UID_REFERRING_XA_PRIVATE_FILE UID_REFERRING_CLASS::instanceXaPrivateFile()
#define UID_REFERRING_XA_PRIVATE_RECORD UID_REFERRING_CLASS::instanceXaPrivateRecord()
#define UID_REFERRING_IMAGE_FILE UID_REFERRING_CLASS::instanceImageFile()
#define UID_REFERRING_IMAGE_RECORD UID_REFERRING_CLASS::instanceImageRecord()
#define UID_REFERRING_UNKNOWN_RECORD UID_REFERRING_CLASS::instanceUnknownRecord()

typedef vector<UID_REFERENCE_STRUCT*> UID_REFERENCE_VECTOR;

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_VALUE_CLASS;

class UID_REFERRING_CLASS
{
    public:
        static UID_REFERRING_CLASS *instancePatientFile();
        static UID_REFERRING_CLASS *instancePatientRecord();
        static UID_REFERRING_CLASS *instanceStudyFile();
        static UID_REFERRING_CLASS *instanceStudyRecord();
        static UID_REFERRING_CLASS *instanceVisitFile();
        static UID_REFERRING_CLASS *instanceVisitRecord();
        static UID_REFERRING_CLASS *instanceStudyComponentFile();
        static UID_REFERRING_CLASS *instanceStudyComponentRecord();
        static UID_REFERRING_CLASS *instanceXaPrivateFile();
        static UID_REFERRING_CLASS *instanceXaPrivateRecord();
        static UID_REFERRING_CLASS *instanceImageFile();
        static UID_REFERRING_CLASS *instanceImageRecord();
        static UID_REFERRING_CLASS *instanceUnknownRecord();

        int GetNrUids();
        UID_REFERENCE_STRUCT *GetUidStruct(UINT);

    protected:
        UID_REFERRING_CLASS(vector<UID_REFERENCE_STRUCT*>);
        virtual ~UID_REFERRING_CLASS();

    private:
        UID_REFERENCE_VECTOR uidReferencesM;

        static UID_REFERRING_CLASS *instancePatientFileM_ptr;
        static UID_REFERRING_CLASS *instancePatientRecordM_ptr;
        static UID_REFERRING_CLASS *instanceStudyFileM_ptr;
        static UID_REFERRING_CLASS *instanceStudyRecordM_ptr;
        static UID_REFERRING_CLASS *instanceVisitFileM_ptr;
        static UID_REFERRING_CLASS *instanceVisitRecordM_ptr;
        static UID_REFERRING_CLASS *instanceStudyComponentFileM_ptr;
        static UID_REFERRING_CLASS *instanceStudyComponentRecordM_ptr;
        static UID_REFERRING_CLASS *instanceXaPrivateFileM_ptr;
        static UID_REFERRING_CLASS *instanceXaPrivateRecordM_ptr;
        static UID_REFERRING_CLASS *instanceImageFileM_ptr;
        static UID_REFERRING_CLASS *instanceImageRecordM_ptr;
        static UID_REFERRING_CLASS *instanceUnknownRecordM_ptr;
};

#endif /* UID_REFERRING_H */
