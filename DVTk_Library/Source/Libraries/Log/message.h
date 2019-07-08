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
//  DESCRIPTION     :   Class for storing all messages of a single object.
//*****************************************************************************
#ifndef MESSAGE_H
#define MESSAGE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"          // global component interface


//*****************************************************************************
//  Type definitions
//*****************************************************************************
typedef vector<UINT32>    INDEX_VECTOR;
typedef vector<string>    STRING_VECTOR;
typedef vector<UINT32>    ID_VECTOR;

UINT32 GetNextMessageIndex();

#undef GetMessage    /*MIGRATION_IN_PROGRESS*/
//>>***************************************************************************
class LOG_MESSAGE_CLASS
//  DESCRIPTION     :
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
    public:
                            LOG_MESSAGE_CLASS();
        virtual             ~LOG_MESSAGE_CLASS();

        void        AddMessage      (UINT32 id, string message);
        int         GetNrMessages   (void);
        UINT32      GetIndex        (int i);
        UINT32      GetMessageId    (int i);
        string      GetMessage      (int i);   // MIGRATION_IN_PROGRESS 
        bool        operator = (LOG_MESSAGE_CLASS &messages);
    protected:
    private:
        void        AddMessage      (UINT32 index, UINT32 id, string message);

        INDEX_VECTOR        indexesM;
        STRING_VECTOR       messagesM;
        ID_VECTOR           idsM;
};

#endif /* MESSAGE_H */
