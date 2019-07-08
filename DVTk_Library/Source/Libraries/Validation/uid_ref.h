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

#ifndef UID_REF_H
#define UID_REF_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_MESSAGE_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
struct ENTITY_NAME_STRUCT
{
    UINT32 tag;
    string name;
    RECORD_IOD_TYPE_ENUM iodType;
};
static const string iodTypes[] =
{
    "IMAGE",
    "PATIENT",
    "PRIVATE",
    "SERIES",
    "STUDY",
    "STUDY COMPONENT",
    "VISIT"
};

typedef vector<ENTITY_NAME_STRUCT*> ENTITY_NAME_VECTOR;

class UID_REF_CLASS
{
    public:
		UID_REF_CLASS();
        virtual ~UID_REF_CLASS();

        string GetUid();
        void SetUid(string);

        void AddDefining(ENTITY_NAME_STRUCT*);
        void AddReferring(ENTITY_NAME_STRUCT*);

        void Check(LOG_MESSAGE_CLASS*);

    private:
        string uidM;
        ENTITY_NAME_VECTOR definingM;
        ENTITY_NAME_VECTOR referringM;
};

#endif /* UID_REF_H */
