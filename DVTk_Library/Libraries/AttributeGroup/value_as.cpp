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

#include "value_as.h"

//>>===========================================================================

VALUE_AS_CLASS::VALUE_AS_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    valueM.erase();
}

//>>===========================================================================

VALUE_AS_CLASS::~VALUE_AS_CLASS()

//  DESCRIPTION     : Default destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    valueM.erase();
}

//>>===========================================================================

bool VALUE_AS_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_AS)
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

DVT_STATUS VALUE_AS_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : This function checks if the value set to the instance of the
//					: AS class is checked for validity.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Format:
//					: A string of characters with one of the following:
//					: "nnnD"     nnn days
//					: "nnnW"     nnn weeks
//					: "nnnM"     nnn months
//					: "nnnY"     nnn years
//					:
//					: where nnn is in the range "000" .. "999".
//<<===========================================================================
{
    UINT        index;
    UINT        length  = valueM.length();
    DVT_STATUS  result = MSG_OK;
    char        message[256];

    if (length == 0)
        return (MSG_OK);

    // check length
    if (length != AS_LENGTH)
    {
        sprintf (message, "value length %i not equal to fixed length %i",
                 length, AS_LENGTH);
        messages->AddMessage (VAL_RULE_D_AS_1, message);
        result = MSG_ERROR;
    }

    if (length > 0)
    {
        // check for nnn
        for (index = 0; index < length - 1; index++)
        {
            if ((valueM[index] < '0') || (valueM[index] > '9'))
            {
                sprintf (message, "unexpected character %c=0x%02X at offset %d - Digit expected",
                        (int)valueM[index], (int)valueM[index], index+1);
                messages->AddMessage (VAL_RULE_D_AS_2, message);
                result = MSG_ERROR;
                break;
            }
        }

        // check for 'D', 'W', 'M' or 'Y' in last character
        if ((valueM[length - 1] != 'D') &&
            (valueM[length - 1] != 'W') &&
            (valueM[length - 1] != 'M') &&
            (valueM[length - 1] != 'Y'))
        {
            sprintf (message, "unexpected last character %c=0x%02X at offset %d - D,W,M or Y expected",
                    (int)valueM[length - 1], (int)valueM[length - 1], length);
            messages->AddMessage (VAL_RULE_D_AS_3, message);
            result = MSG_ERROR;
        }
    }

    return result;
}

//>>===========================================================================

DVT_STATUS VALUE_AS_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string          ref;
    DVT_STATUS      rval;

    if (ref_value == NULL) return MSG_OK;

    if (ref_value->GetVRType() != ATTR_VR_AS)
    {
        return (MSG_INCOMPATIBLE);
    }

    rval = ref_value->Get(ref);

    if (rval != MSG_OK)
    {
        ref.erase();
        return (rval);
    }

    // Check for zero length
    if ((valueM.length() == 0) && (ref.length() == 0))
    {
        // values considered the same
        return (MSG_EQUAL);
    }

    // We know we have data, and we have the reference value. Now compare these.
    rval = CompareStringValues (ref, false, false);

    ref.erase();
    return (rval);
}

//>>===========================================================================

DVT_STATUS VALUE_AS_CLASS::Compare(string value)

//  DESCRIPTION     : Compare to string value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DVT_STATUS         rval;

    // Check for zero length
    if ((valueM.length() == 0) && (value.length() == 0))
    {
        // values considered the same
        return (MSG_EQUAL);
    }

    // We know we have data, and we have the reference value. Now compare these.
    rval = CompareStringValues (value, false, false);

    return (rval);
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_AS_CLASS::GetVRType()

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_AS);
}

//>>===========================================================================

string VALUE_AS_CLASS::GetStripped(void)

//  DESCRIPTION     : Get stripped string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string  stripped;
    StringStrip (valueM, valueM.length(), false, false, stripped);

    return stripped;
}
