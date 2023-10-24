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

#ifndef VALUE_LIST_H
#define VALUE_LIST_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"         // Log component interface
#include <string>
#include <vector>


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_VALUE_CLASS;

//*****************************************************************************
//  Type definitions
//*****************************************************************************
typedef vector<BASE_VALUE_CLASS *>  BASE_VALUE_VECTOR;


class VALUE_LIST_CLASS  
{
public:
                                VALUE_LIST_CLASS();
    virtual                    ~VALUE_LIST_CLASS();
	        BASE_VALUE_CLASS*   GetValue (int index);
            int                 GetNrValues (void);
	        DVT_STATUS          AddValue (BASE_VALUE_CLASS * value);
	        void                SetValueType (ATTR_VAL_TYPE_ENUM val_type);
	        ATTR_VAL_TYPE_ENUM  GetValueType (void);
            DVT_STATUS          Replace (BASE_VALUE_CLASS * value,
                                         int index=0);
            bool                HasValue (BASE_VALUE_CLASS * value);

private:
	ATTR_VAL_TYPE_ENUM          value_typeM;
    BASE_VALUE_VECTOR           valuesM;
};

#endif /* VALUE_LIST_H */
