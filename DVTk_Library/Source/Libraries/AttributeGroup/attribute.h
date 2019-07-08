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

#ifndef ATTRIBUTE_H
#define ATTRIBUTE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"      // Global component interface
#include "Ilog.h"         // Log component interface
#include <string>
#include <vector>


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_VALUE_CLASS;
class VALUE_LIST_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;


//*****************************************************************************
//  Type definitions
//*****************************************************************************
typedef vector<VALUE_LIST_CLASS *>    VALUE_LIST_VECTOR;

class ATTRIBUTE_CLASS
{
public:
                        ATTRIBUTE_CLASS();
    virtual            ~ATTRIBUTE_CLASS();

    UINT16              GetElement (void);
    virtual UINT16      GetMappedElement();
    DVT_STATUS          SetElement (UINT16 element);

    UINT16              GetGroup (void);
    virtual UINT16      GetMappedGroup();
    DVT_STATUS          SetGroup (UINT16 group);

    ATTR_VR_ENUM        GetVR (void);
    DVT_STATUS          SetVR (ATTR_VR_ENUM vr_type);

    ATTR_TYPE_ENUM      GetType (void);
    DVT_STATUS          SetType (ATTR_TYPE_ENUM attr_type);

    BASE_VALUE_CLASS  * GetValue (int value_index=0, int list_index=0);
    VALUE_LIST_CLASS  * GetValueList (int list_index=0);

    DVT_STATUS          AddValue (BASE_VALUE_CLASS * value, int list_index=0);

    DVT_STATUS          Replace (BASE_VALUE_CLASS * value,
                                 int value_index=0, int list_index=0);

    int                 GetNrLists (void);
    int                 GetNrValues (int list_index=0);

    DVT_STATUS          SetValueType (ATTR_VAL_TYPE_ENUM val_type, int list_index=0);
    ATTR_VAL_TYPE_ENUM  GetValueType (int list_index=0);

	void				SetPresent(bool flag);
	bool				IsPresent();

    DVT_STATUS          Check (UINT32 flags,
                               ATTRIBUTE_CLASS * ref_attributes,
                               LOG_MESSAGE_CLASS *messages,
                               SPECIFIC_CHARACTER_SET_CLASS *specific_character_set = NULL);

    virtual bool        operator < (ATTRIBUTE_CLASS &attribute);
    virtual bool        operator > (ATTRIBUTE_CLASS &attribute);
    bool                operator == (ATTRIBUTE_CLASS &attribute);
    bool                operator != (ATTRIBUTE_CLASS &attribute);
    bool                operator = (ATTRIBUTE_CLASS &attribute);

    DVT_STATUS          Compare ( LOG_MESSAGE_CLASS *messages,
                                  ATTRIBUTE_CLASS * attribute);

    DVT_STATUS          CompareVRAndValues(ATTRIBUTE_CLASS * attribute);

private:
    VALUE_LIST_VECTOR   value_listM;
    UINT16              groupM;
    UINT16              elementM;
    ATTR_VR_ENUM        vr_typeM;
    ATTR_TYPE_ENUM      attr_typeM;
	bool				presentM;
};

#endif /* ATTRIBUTE_H */
