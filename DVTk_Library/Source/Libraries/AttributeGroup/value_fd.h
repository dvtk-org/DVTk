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

#ifndef VALUE_FD_H
#define VALUE_FD_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <string>

#include "Iglobal.h"      // Global component interface
#include "base_value.h"

//*****************************************************************************
//  VALUE_FD_CLASS definition
//*****************************************************************************
class VALUE_FD_CLASS : public BASE_VALUE_CLASS
{
public:
                            VALUE_FD_CLASS();
    virtual                ~VALUE_FD_CLASS();
            bool            operator = (BASE_VALUE_CLASS &value);
            ATTR_VR_ENUM    GetVRType (void);
            DVT_STATUS      Compare (LOG_MESSAGE_CLASS *messages,
                                     BASE_VALUE_CLASS * ref_value);
            DVT_STATUS      Check (UINT32 flags,
                                   BASE_VALUE_CLASS **value_list,
                                   LOG_MESSAGE_CLASS *messages,
                                   SPECIFIC_CHARACTER_SET_CLASS *specific_character_set = NULL);
            DVT_STATUS      Set (string value);
            DVT_STATUS      Set (double value);
            DVT_STATUS      Get (string &value, bool);
            DVT_STATUS      Get (double &value);
            UINT32          GetLength (void);

private:
    double                  valueM;
};

#endif /* VALUE_FD_H */
