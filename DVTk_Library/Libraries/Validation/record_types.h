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

#ifndef RECORD_TYPES_H
#define RECORD_TYPES_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_VALUE_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
//this struct defines record types and their allowed relations
//as defined in DICOM part 3, paragraph F.4
typedef struct
{
    DICOMDIR_RECORD_TYPE_ENUM recordType;
    string recordTypeName;				// eg "PATIENT"
    string lowerList;					// list of record types allowed as lower level
    UINT32 recordIdentifyingTag1;
    UINT32 recordIdentifyingTag2;
    bool isRetired;
} RECORD_TYPE_STRUCT;

#define RECORD_TYPES RECORD_TYPES_CLASS::instance()

typedef vector<RECORD_TYPE_STRUCT*> RECORD_TYPE_VECTOR;

//>>***************************************************************************

class RECORD_TYPES_CLASS

//  DESCRIPTION     : Record Types Class
//  NOTES           :
//<<***************************************************************************
{
    public:
        static RECORD_TYPES_CLASS *instance();
        DICOMDIR_RECORD_TYPE_ENUM GetRecordTypeIndexWithRecordName(BASE_VALUE_CLASS*);

        UINT GetRecordTypeIndex(DICOMDIR_RECORD_TYPE_ENUM);

        string GetLowerListOfRecordTypeWithIdx(UINT);
        string GetRecordTypeNameOfRecordTypeWithIdx(UINT);

        UINT32 GetIdentifyingTag1(UINT);
        UINT32 GetIdentifyingTag2(UINT);

        bool IsRecordRetired(UINT);

        bool IsChildAllowedInParent(UINT, UINT);

    protected:
        RECORD_TYPES_CLASS();
        virtual ~RECORD_TYPES_CLASS();

    private:
        static RECORD_TYPES_CLASS *instanceM_ptr;
        RECORD_TYPE_VECTOR recordTypesM;

        void FillRecordTypes();

        RECORD_TYPE_STRUCT *CreateRecordType(DICOMDIR_RECORD_TYPE_ENUM, string, string, UINT32, UINT32, bool);
};

#endif /* RECORD_TYPES_H */
