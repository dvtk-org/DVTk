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

#ifndef VALUE_SQ_H
#define VALUE_SQ_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <string>

#include "Iglobal.h"      // Global component interface
#include "base_value.h"


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ATTRIBUTE_GROUP_CLASS;
class ATTRIBUTE_CLASS;

//*****************************************************************************
//  Type definitions
//*****************************************************************************
typedef vector<ATTRIBUTE_GROUP_CLASS *>    ITEM_LIST_VECTOR;


//*****************************************************************************
//  VALUE_SQ_CLASS definition
//*****************************************************************************
class VALUE_SQ_CLASS : public BASE_VALUE_CLASS
{
public:
                            VALUE_SQ_CLASS();
    virtual                ~VALUE_SQ_CLASS();
            bool            operator = (BASE_VALUE_CLASS &value);
            DVT_STATUS      Get (ATTRIBUTE_GROUP_CLASS **item, int index=0);
            DVT_STATUS      Set (ATTRIBUTE_GROUP_CLASS * item);
            void            SetLength(UINT32 length);

            DVT_STATUS      Compare (LOG_MESSAGE_CLASS *messages,
                                     BASE_VALUE_CLASS * ref_value);
            DVT_STATUS      Check (UINT32 flags,
                                   BASE_VALUE_CLASS **value_list,
                                   LOG_MESSAGE_CLASS *messages,
                                   SPECIFIC_CHARACTER_SET_CLASS *specific_character_set = NULL);
            ATTR_VR_ENUM    GetVRType (void);
            int             GetNrItems (void);
            UINT32          GetLength (void);

            DVT_STATUS      DeleteItemReferences(void);

            DVT_STATUS      DeleteItems(void);

			ATTRIBUTE_CLASS *GetAttribute(unsigned short  group, unsigned short element);

			ATTRIBUTE_CLASS *GetMappedAttribute(unsigned short  group, unsigned short element);

            bool            IsSorted (void);
            DVT_STATUS      SortAttributes(void);

private:
    UINT32                  lengthM;
    ITEM_LIST_VECTOR        itemsM;
};

#endif /* VALUE_SQ_H */
