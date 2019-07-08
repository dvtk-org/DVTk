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

#ifndef BASE_STRING_H
#define BASE_STRING_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <string>
#include "Iglobal.h"      // Global component interface
#include "base_value.h"

class BASE_STRING_CLASS : public BASE_VALUE_CLASS  
{
public:
                        BASE_STRING_CLASS();
    virtual            ~BASE_STRING_CLASS();
    virtual DVT_STATUS  Get (string &value, bool stripped=false);
    virtual DVT_STATUS  Get (unsigned char ** value, UINT32 &length);
    virtual DVT_STATUS  Set (string value);
    virtual DVT_STATUS  Set (unsigned char * value, UINT32 length);
    virtual bool        operator = (BASE_STRING_CLASS &value);
    virtual UINT32      GetLength (void);

protected:
    string              valueM;

    virtual string      GetStripped (void) = 0;
            int         StringStrip (string     src_string,
                                     int        max_length,
                                     bool       lead_spc,
                                     bool       trail_spc,
                                     string    &result);

    virtual DVT_STATUS  CompareStringValues (string     ref_value,
                                             bool       lead_spc,
                                             bool       trail_spc);
};

#endif /* BASE_STRING_H */
