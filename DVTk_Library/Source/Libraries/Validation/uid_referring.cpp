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
//  DESCRIPTION     :   Referring UID types Singleton class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "uid_referring.h"

//*****************************************************************************
//  INTERNAL DECLARATIONS
//*****************************************************************************

UID_REFERRING_CLASS * UID_REFERRING_CLASS::instancePatientFileM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instancePatientRecordM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceStudyFileM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceStudyRecordM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceVisitFileM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceVisitRecordM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceStudyComponentFileM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceStudyComponentRecordM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceXaPrivateFileM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceXaPrivateRecordM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceImageFileM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceImageRecordM_ptr = NULL;
UID_REFERRING_CLASS * UID_REFERRING_CLASS::instanceUnknownRecordM_ptr = NULL;

//>>===========================================================================

int UID_REFERRING_CLASS::GetNrUids()

//  DESCRIPTION     : Returns the number of UID structs in the requested
//                    uid definition object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return uidReferencesM.size();
}

//>>===========================================================================

UID_REFERENCE_STRUCT *UID_REFERRING_CLASS::GetUidStruct(UINT index)

//  DESCRIPTION     : Returns the UID struct with the requested index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    UID_REFERENCE_STRUCT *uidRef_ptr = NULL;
    if (index < uidReferencesM.size())
    {
        uidRef_ptr = uidReferencesM[index];
    }

    return uidRef_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS::UID_REFERRING_CLASS(vector<UID_REFERENCE_STRUCT*> uids)

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (UINT i = 0; i < uids.size(); i++)
    {
        uidReferencesM.push_back(uids[i]);
    }
}

//>>===========================================================================

UID_REFERRING_CLASS::~UID_REFERRING_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instancePatientFile()

//  DESCRIPTION     : Get patient file instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instancePatientFileM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_SERIES_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_SERIES;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_VISIT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_VISIT;
        uids.push_back(uidRef_ptr);

        instancePatientFileM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instancePatientFileM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instancePatientRecord()

//  DESCRIPTION     : Get patient record instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instancePatientRecordM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PATIENT;
        uids.push_back(uidRef_ptr);

        instancePatientRecordM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instancePatientRecordM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceStudyFile()

//  DESCRIPTION     : Get study file instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceStudyFileM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_SERIES_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_SERIES;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_COMPONENT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY_COMPONENT;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_VISIT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_VISIT;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_PATIENT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PATIENT;
        uids.push_back(uidRef_ptr);

        instanceStudyFileM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceStudyFileM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceStudyRecord()

//  DESCRIPTION     : Get study record instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceStudyRecordM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY;
        uids.push_back(uidRef_ptr);

        instanceStudyRecordM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceStudyRecordM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceVisitFile()

//  DESCRIPTION     : Get visit file instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceVisitFileM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_SERIES;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_PATIENT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PATIENT;
        uids.push_back(uidRef_ptr);

        instanceVisitFileM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceVisitFileM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceVisitRecord()

//  DESCRIPTION     : Get visit record instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceVisitRecordM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_VISIT;
        uids.push_back(uidRef_ptr);

		instanceVisitRecordM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceVisitRecordM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceStudyComponentFile()

//  DESCRIPTION     : Get study component file instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceStudyComponentFileM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_IMAGE_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_IMAGE;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_CD_MEDICAL_REFERENCED_ALTERNATE_IMAGE_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PRIVATE;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_SERIES_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_SERIES;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_SERIES_SEQUENCE;
        uidRef_ptr->uidTag = TAG_SERIES_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_SERIES;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY;
        uids.push_back(uidRef_ptr);

		instanceStudyComponentFileM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceStudyComponentFileM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceStudyComponentRecord()

//  DESCRIPTION     : Get study component record instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceStudyComponentRecordM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY_COMPONENT;
        uids.push_back(uidRef_ptr);

		instanceStudyComponentRecordM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceStudyComponentRecordM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceXaPrivateFile()

//  DESCRIPTION     : Get xa private file instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceXaPrivateFileM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_SERIES_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_SERIES;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_PATIENT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PATIENT;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_COMPONENT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY_COMPONENT;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_SOURCE_IMAGE_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_IMAGE;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_IMAGE_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PRIVATE;
        uids.push_back(uidRef_ptr);

		instanceXaPrivateFileM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceXaPrivateFileM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceXaPrivateRecord()

//  DESCRIPTION     : Get xa private record instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceXaPrivateRecordM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PRIVATE;
        uids.push_back(uidRef_ptr);

		instanceXaPrivateRecordM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceXaPrivateRecordM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceImageFile()

//  DESCRIPTION     : Get image file instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceImageFileM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_SERIES_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_SERIES;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_PATIENT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PATIENT;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_STUDY_COMPONENT_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_STUDY_COMPONENT;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_CD_MEDICAL_REFERENCED_ALTERNATE_IMAGE_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PRIVATE;
        uids.push_back(uidRef_ptr);

        uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_REFERENCED_IMAGE_SEQUENCE;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_IMAGE;
        uids.push_back(uidRef_ptr);

		instanceImageFileM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceImageFileM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceImageRecord()

//  DESCRIPTION     : Get image record instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceImageRecordM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_IMAGE;
        uids.push_back(uidRef_ptr);

		instanceImageRecordM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceImageRecordM_ptr;
}

//>>===========================================================================

UID_REFERRING_CLASS *UID_REFERRING_CLASS::instanceUnknownRecord()

//  DESCRIPTION     : Get unknown record instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceUnknownRecordM_ptr == NULL) 
	{
        vector<UID_REFERENCE_STRUCT*> uids;
        UID_REFERENCE_STRUCT *uidRef_ptr = new UID_REFERENCE_STRUCT();
        uidRef_ptr->sequenceTag = TAG_UNDEFINED;
        uidRef_ptr->uidTag = TAG_REFERENCED_SOP_INSTANCE_UID_IN_FILE;
        uidRef_ptr->uidType = RECORD_IOD_TYPE_PRIVATE;
        uids.push_back(uidRef_ptr);

		instanceUnknownRecordM_ptr = new UID_REFERRING_CLASS(uids);
	}

	return instanceUnknownRecordM_ptr;
}
