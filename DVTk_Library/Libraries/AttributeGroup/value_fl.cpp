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

#include "value_fl.h"

//>>===========================================================================

VALUE_FL_CLASS::VALUE_FL_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

VALUE_FL_CLASS::~VALUE_FL_CLASS()

//  DESCRIPTION     : Default destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

bool VALUE_FL_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_FL)
    {
        value.Get(valueM);
        return (true);
    }
    else
    {
        return (false);
    }
}

//>>===========================================================================

DVT_STATUS VALUE_FL_CLASS::Get(float &value)

//  DESCRIPTION     : Get value as float.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    value = valueM;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_FL_CLASS::Get(string &value, bool)

//  DESCRIPTION     : Get value as string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    char    str_value[128];

    sprintf (str_value, "%f", valueM);
    value = str_value;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_FL_CLASS::Set(float value)

//  DESCRIPTION     : Set value from float.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    valueM = value;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_FL_CLASS::Set(string value)

//  DESCRIPTION     : Set value from string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    sscanf (value.c_str(), "%f", &valueM);
    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_FL_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : Check FL format.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_FL_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    float       ref_fl;

    if (ref_value == NULL) return MSG_OK;

    if (ref_value->GetVRType() != ATTR_VR_FL)
    {
        return (MSG_INCOMPATIBLE);
    }

    ref_value->Get(ref_fl);

    if (valueM == ref_fl)
    {
        return (MSG_EQUAL);
    }
    if (valueM > ref_fl)
    {
        return (MSG_GREATER);
    }
    if (valueM < ref_fl)
    {
        return (MSG_SMALLER);
    }

    // This line should never be executed.
    return (MSG_NOT_EQUAL);
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_FL_CLASS::GetVRType()

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_FL);
}

//>>===========================================================================

UINT32 VALUE_FL_CLASS::GetLength (void)

//  DESCRIPTION     : Get length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return sizeof(valueM);
}