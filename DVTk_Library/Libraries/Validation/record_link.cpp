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
#include "record_link.h"
#include "record_types.h"
#include "valrules.h"

#include "Iglobal.h"        // Global component interface
#include "Ilog.h"           // Logger component interface

//>>===========================================================================

RECORD_LINK_CLASS::RECORD_LINK_CLASS(UINT32 recordOffset)

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    downLinkOffsetM = DICOMDIR_ILLEGAL_OFFSET;
    horLinkOffsetM = DICOMDIR_ILLEGAL_OFFSET;
    inUseFlagM = 0;
    refCountM = 0;
    recordOffsetM = recordOffset;
    recordTypeM = DICOMDIR_RECORD_TYPE_UNKNOWN;
    messagesM_ptr = NULL;
}

//>>===========================================================================

RECORD_LINK_CLASS::~RECORD_LINK_CLASS()

//  DESCRIPTION     : Class destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    delete messagesM_ptr;
}

//>>===========================================================================

UINT32 RECORD_LINK_CLASS::GetDownLinkOffset()

//  DESCRIPTION     : Get the down link offset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return downLinkOffsetM;
}

//>>===========================================================================

void RECORD_LINK_CLASS::SetDownLinkOffset(UINT32 offset)

//  DESCRIPTION     : Set the down link offset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    downLinkOffsetM = offset;
}

//>>===========================================================================

UINT32 RECORD_LINK_CLASS::GetHorLinkOffset()

//  DESCRIPTION     : Get the horizontal link.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return horLinkOffsetM;
}

//>>===========================================================================

void RECORD_LINK_CLASS::SetHorLinkOffset(UINT32 offset)

//  DESCRIPTION     : Set the horizontal link.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    horLinkOffsetM = offset;
}

//>>===========================================================================

UINT32 RECORD_LINK_CLASS::GetRecordOffset()

//  DESCRIPTION     : Get the record offset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return recordOffsetM;
}

//>>===========================================================================

UINT16 RECORD_LINK_CLASS::GetInUseFlag()

//  DESCRIPTION     : Get the in-use flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return inUseFlagM;
}

//>>===========================================================================

void RECORD_LINK_CLASS::SetInUseFlag(UINT16 inUseFlag)

//  DESCRIPTION     : Set the in-use flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    inUseFlagM = inUseFlag;
}

//>>===========================================================================

UINT16 RECORD_LINK_CLASS::GetReferenceCount()

//  DESCRIPTION     : Get the reference count.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return refCountM;
}

//>>===========================================================================

void RECORD_LINK_CLASS::IncreaseReferenceCount()

//  DESCRIPTION     : Increment the reference count.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    refCountM++;
}

//>>===========================================================================

DICOMDIR_RECORD_TYPE_ENUM RECORD_LINK_CLASS::GetRecordType()

//  DESCRIPTION     : Get the record type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return recordTypeM;
}

//>>===========================================================================

void RECORD_LINK_CLASS::SetRecordType(DICOMDIR_RECORD_TYPE_ENUM recordType)

//  DESCRIPTION     : Set the record type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    recordTypeM = recordType;
}

//>>===========================================================================

bool RECORD_LINK_CLASS::CheckUnreferencedRecords()

//  DESCRIPTION     : Check the unreferenced records.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool result = true;

    if ((refCountM == 0) &&
        (inUseFlagM == 0))
    {
        if (messagesM_ptr)
		{
			messagesM_ptr->AddMessage(VAL_RULE_A_MEDIA_7, "This record is never referenced.");
		}
        result = false;
    }
    return result;
}

//>>===========================================================================

LOG_MESSAGE_CLASS *RECORD_LINK_CLASS::GetMessages()

//  DESCRIPTION     : Get the log messages.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (messagesM_ptr == NULL)
    {
        messagesM_ptr = new LOG_MESSAGE_CLASS();
    }

    return messagesM_ptr;
}

//>>===========================================================================

bool RECORD_LINK_CLASS::HasMessages()

//  DESCRIPTION     : Check if the record link has messages.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (messagesM_ptr == NULL) ? false : true;
}

void RECORD_LINK_CLASS::Dump()
{
	FILE *fd_ptr = fopen("C:\\dvtlog\\recordLog.txt", "a");
	if (fd_ptr != NULL)
	{
		string name = RECORD_TYPES->GetRecordTypeNameOfRecordTypeWithIdx(RECORD_TYPES->GetRecordTypeIndex(recordTypeM));

		char buffer[1024];
		sprintf(buffer, "Off: 0x%08X - Dwn: 0x%08X- Hor: 0x%08X - Use: 0x%04X - Ref: %d - Typ: %s\n", 
			recordOffsetM,
			downLinkOffsetM,
			horLinkOffsetM,
			inUseFlagM,
			refCountM,
			name.c_str());

		fputs(buffer, fd_ptr);

		fclose(fd_ptr);
	}
}