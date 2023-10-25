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
//  DESCRIPTION     :   Base value results include file for validation.
//*****************************************************************************
#ifndef VAL_BASE_VALUE_H
#define VAL_BASE_VALUE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class VALUE_LIST_CLASS;
class BASE_VALUE_CLASS;
class LOG_MESSAGE_CLASS;
class VAL_BASE_VALUE_CLASS;
class VAL_ATTRIBUTE_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;

//>>***************************************************************************
class VAL_BASE_VALUE_CLASS
//  DESCRIPTION     :
//  NOTES           :
//<<***************************************************************************
{
    public:
        VAL_BASE_VALUE_CLASS();
        virtual ~VAL_BASE_VALUE_CLASS();

        void SetParent(VAL_ATTRIBUTE_CLASS*);

        void SetDefValueList(VALUE_LIST_CLASS*);
        void SetRefValue(BASE_VALUE_CLASS*);
        void SetDcmValue(BASE_VALUE_CLASS*);

        VALUE_LIST_CLASS *GetDefValueList();
        BASE_VALUE_CLASS *GetRefValue();
        BASE_VALUE_CLASS *GetDcmValue();

        virtual DVT_STATUS Check(UINT32, SPECIFIC_CHARACTER_SET_CLASS*);
        DVT_STATUS CompareRef();

        LOG_MESSAGE_CLASS *GetMessages();
        bool HasMessages();

    private:
        VAL_ATTRIBUTE_CLASS *parentM_ptr;
        VALUE_LIST_CLASS *defValueM_ptr;
        BASE_VALUE_CLASS *refValueM_ptr;
        BASE_VALUE_CLASS *dcmValueM_ptr;
        LOG_MESSAGE_CLASS *messagesM_ptr;
};

#endif /* VAL_BASE_VALUE_H */