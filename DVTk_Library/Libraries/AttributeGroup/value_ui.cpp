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

#include "value_ui.h"


//>>===========================================================================

VALUE_UI_CLASS::VALUE_UI_CLASS()

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

VALUE_UI_CLASS::~VALUE_UI_CLASS()

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

bool VALUE_UI_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_UI)
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

DVT_STATUS VALUE_UI_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : Check UI format.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DVT_STATUS  result = MSG_OK;
    int         index;
    int         length = valueM.length();
    bool        first_num = true;
    char        first_digit = 0;
    char        message[256];

    if (length == 0)
        return (MSG_OK);

    // check max length
    if (length > UI_LENGTH)
    {
        sprintf (message, "value length %i exceeds maximum length %i - truncated",
                length, UI_LENGTH);
        messages->AddMessage (VAL_RULE_D_UI_1, message);
        length = UI_LENGTH;
        result = MSG_ERROR;
    }

    // Check if the value starts with a digit.
    if ((valueM[0] < '0') || (valueM[0] > '9'))
    {
        sprintf (message, "value should start with digit(s)");
        messages->AddMessage (VAL_RULE_D_UI_2, message);
        return (MSG_ERROR);
    }

    for (index=0 ; index<length ; index++)
    {
        if ((valueM[index] >= '0') && (valueM[index] <= '9'))
        {
            if (first_num == true)
            {
                // This is the first digit of a new component.
                first_digit = valueM[index];
                first_num = false;
            }

            // Check if the first digit of the component is zero.
            if (first_digit == '0')
            {
				// check for case .0.
				if(((index +1) < length) && (valueM[index +1] != '.'))
				{
					sprintf (message, "no leading zeros allowed in value at offset %d",
						index+1);
					messages->AddMessage (VAL_RULE_D_UI_6, message);
					return (MSG_ERROR);
				}
            }
        }
        else
        {
            if (valueM[index] == '.')
            {
                // Check if we were expecting a number instead of a dot.
                if (first_num == true)
                {
                    sprintf (message, "digits expected between periods at offset %d",
						index+1);
                    messages->AddMessage (VAL_RULE_D_UI_4, message);
                }

                // The next character is expected to be a number.
                first_num = true;
            }
            else
            {
                sprintf (message, "unexpected character %c=0x%02X at offset %d",
					valueM[index], valueM[index], index+1);
                messages->AddMessage (VAL_RULE_D_UI_5, message);
                return (MSG_ERROR);
            }
        }
    }

    // Check if we were still expecting a digit.
    if (first_num == true)
    {
        sprintf (message, "value should end with digit(s)");
        messages->AddMessage (VAL_RULE_D_UI_3, message);
        result = MSG_ERROR;
    }

    return (result);
}

//>>===========================================================================

DVT_STATUS VALUE_UI_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string          ref;
    DVT_STATUS         rval = MSG_OK;

    if (ref_value == NULL) return MSG_OK;

    if (ref_value->GetVRType() != ATTR_VR_UI)
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
    rval = CompareStringValues (ref, true, true);

    ref.erase();
    return (rval);
}

//>>===========================================================================

DVT_STATUS VALUE_UI_CLASS::Compare(string value)

//  DESCRIPTION     : Compare value to string.
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
    rval = CompareStringValues (value, true, true);

    return (rval);
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_UI_CLASS::GetVRType(void)

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_UI);
}

//>>===========================================================================

string VALUE_UI_CLASS::GetStripped(void)

//  DESCRIPTION     : Get stripped string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string  stripped;
    StringStrip (valueM, valueM.length(), true, true, stripped);

    return stripped;
}
