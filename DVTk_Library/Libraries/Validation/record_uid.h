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

#ifndef RECORD_UID_H
#define RECORD_UID_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_ATTRIBUTE_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;
class LOG_MESSAGE_CLASS;
class UID_DEFINING_CLASS;
class UID_REFERRING_CLASS;
class UID_REF_CLASS;
struct UID_REFERENCE_STRUCT;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
typedef vector<UID_REF_CLASS *>   UID_REF_VECTOR;

class RECORD_UID_CLASS
{
    public:
        RECORD_UID_CLASS();
        virtual ~RECORD_UID_CLASS();

        bool InstallDefiningUid(DCM_ATTRIBUTE_GROUP_CLASS*, UID_DEFINING_CLASS*);

        bool InstallReferringUid(DCM_ATTRIBUTE_GROUP_CLASS*, UID_REFERRING_CLASS*);

        void Check(LOG_MESSAGE_CLASS*);

    private:
        UID_REF_VECTOR uidRefsM;

        void StoreReferringUid(string, UID_REFERENCE_STRUCT*, string);
};

#endif /* RECORD_UID_H */
