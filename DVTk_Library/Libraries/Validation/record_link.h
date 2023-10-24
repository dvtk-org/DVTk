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
//  DESCRIPTION     :   Record link include file for validation.
//*****************************************************************************
#ifndef RECORD_LINK_H
#define RECORD_LINK_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_MESSAGE_CLASS;

//*****************************************************************************
//  Type definitions
//*****************************************************************************

//>>***************************************************************************
class RECORD_LINK_CLASS
{
    public:
		RECORD_LINK_CLASS(UINT32);
        ~RECORD_LINK_CLASS();

        UINT32 GetDownLinkOffset();
        void SetDownLinkOffset(UINT32);

        UINT32 GetHorLinkOffset();
        void SetHorLinkOffset(UINT32);

        UINT32 GetRecordOffset();

        UINT16 GetInUseFlag();
        void SetInUseFlag(UINT16);

        UINT16 GetReferenceCount();
        void IncreaseReferenceCount();

        DICOMDIR_RECORD_TYPE_ENUM GetRecordType();
        void SetRecordType(DICOMDIR_RECORD_TYPE_ENUM);

        LOG_MESSAGE_CLASS *GetMessages();
        bool HasMessages();

        bool CheckUnreferencedRecords();

		void Dump();

    private:
        UINT32 recordOffsetM;
        UINT32 horLinkOffsetM;
        UINT32 downLinkOffsetM;
        UINT16 inUseFlagM;
        UINT16 refCountM;
        DICOMDIR_RECORD_TYPE_ENUM recordTypeM;
        LOG_MESSAGE_CLASS *messagesM_ptr;
};

#endif /* RECORD_LINK_H */
