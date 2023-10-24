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

#ifndef VALUE_DA_H
#define VALUE_DA_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <string>

#include "Iglobal.h"      // Global component interface
#include "base_string.h"

//*****************************************************************************
//  VALUE_DA_CLASS definition
//*****************************************************************************
class VALUE_DA_CLASS : public BASE_STRING_CLASS
{
public:
                            VALUE_DA_CLASS();
    virtual                ~VALUE_DA_CLASS();
            bool            operator = (BASE_VALUE_CLASS &value);
            ATTR_VR_ENUM    GetVRType (void);
            DVT_STATUS      Compare (LOG_MESSAGE_CLASS *messages,
                                     BASE_VALUE_CLASS * ref_value);
            DVT_STATUS      Compare (string);
            DVT_STATUS      Check (UINT32 flags,
                                   BASE_VALUE_CLASS **,
                                   LOG_MESSAGE_CLASS *messages,
                                   SPECIFIC_CHARACTER_SET_CLASS *specific_character_set = NULL);

private:
            string          GetStripped (void);

            void            ConvertOldDate (string date, string& conv_date);
            //DVT_STATUS      IsDate (string date, int start_index, LOG_MESSAGE_CLASS * messages);
            DVT_STATUS      IsDate (string date, bool, LOG_MESSAGE_CLASS * messages);
};

#endif /* VALUE_DA_H */
