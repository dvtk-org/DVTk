// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
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

#ifndef VALUE_OW_H
#define VALUE_OW_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <string>

#include "Iglobal.h"      // Global component interface
#include "other_value.h"

//*****************************************************************************
//  VALUE_OW_CLASS definition
//*****************************************************************************
class VALUE_OW_CLASS : public OTHER_VALUE_CLASS
{
public:
                            VALUE_OW_CLASS();
    virtual                ~VALUE_OW_CLASS();
            bool            operator = (BASE_VALUE_CLASS &value);
            UINT32          GetLength (void);
            ATTR_VR_ENUM    GetVRType (void);
            DVT_STATUS      Compare (LOG_MESSAGE_CLASS *messages,
                                     BASE_VALUE_CLASS * ref_value);
            DVT_STATUS      Check (UINT32 flags,
                                   BASE_VALUE_CLASS **value_list,
                                   LOG_MESSAGE_CLASS *messages,
                                   SPECIFIC_CHARACTER_SET_CLASS *specific_character_set = NULL);
};

#endif /* VALUE_OW_H */
