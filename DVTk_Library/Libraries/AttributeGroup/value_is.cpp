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

#include "value_is.h"

//>>===========================================================================

VALUE_IS_CLASS::VALUE_IS_CLASS()

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

VALUE_IS_CLASS::~VALUE_IS_CLASS()

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

bool VALUE_IS_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_IS)
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

DVT_STATUS VALUE_IS_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : Check IS format.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int         length  = valueM.length();
    DVT_STATUS  result = MSG_OK;
    int         index;
    bool        integer_part=false;
	bool        isValueNegative=false;
    long        digit;
    long        val = 0;
    char        message[256];

    if (length == 0)
        return (MSG_OK);

    // check length
    if (length > IS_LENGTH)
    {
        sprintf (message, "value length %i exceeds maximum length %i - truncated",
                length, IS_LENGTH);
        messages->AddMessage (VAL_RULE_D_IS_1, message);
        length = IS_LENGTH;
        result = MSG_ERROR;
    }

    if (length > 0)
    {
        index = 0;
        // Skip optional leading spaces.
        while (valueM[index] == SPACECHAR)
        {
            index++;
        }

        // Optional sign.
        if ((valueM[index] == '+') || (valueM[index] == '-'))
        {
			if (valueM[index] == '-')
			{
				isValueNegative = true;
			}
            index++;
        }		

        while ((valueM[index] >= '0') && (valueM[index] <= '9'))
        {
            integer_part = true;
            digit = valueM[index] - '0';

            if (val <= 214748364)
            {
				if(val == 214748364)
				{
					if(((isValueNegative) && (digit <= 8)) || ((!isValueNegative) && (digit <= 7)))
						val = 10 * val + digit;
					else
					{
						sprintf (message, "\"%s\" out of specified range -(2^31)..(2^31 - 1)",
								 valueM.c_str());
						messages->AddMessage (VAL_RULE_D_IS_3, message);
						return (MSG_ERROR);
					}
				}
				else
					val = 10 * val + digit;
            }
            else
            {
                sprintf (message, "\"%s\" out of specified range -(2^31)..(2^31 - 1)",
                         valueM.c_str());
                messages->AddMessage (VAL_RULE_D_IS_3, message);
                return (MSG_ERROR);
            }
            index++;
        }

        // Skip optional trailing spaces.
        while (valueM[index] == SPACECHAR)
        {
            index++;
        }

        // Check end of string.
        if (valueM[index] != NULLCHAR)
        {
            sprintf (message, "unexpected character %c=0x%02X at offset %d",
                    valueM[index], valueM[index], index+1);
            messages->AddMessage (VAL_RULE_D_IS_2, message);
            result = MSG_ERROR;
        }

        // Check for integer part.
        if (integer_part == false)
        {
            sprintf (message, "no integer digits found");
            messages->AddMessage (VAL_RULE_D_IS_2, message);
            result = MSG_ERROR;
        }
    }

    return result;
}

//>>===========================================================================

DVT_STATUS VALUE_IS_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string          ref;
    DVT_STATUS      rval = MSG_OK;

    if (ref_value == NULL) return MSG_OK;

    if (ref_value->GetVRType() != ATTR_VR_IS)
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

DVT_STATUS VALUE_IS_CLASS::Compare(string value)

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
    rval = CompareStringValues (value, false, false);

    return (rval);
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_IS_CLASS::GetVRType(void)

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_IS);
}

//>>===========================================================================

DVT_STATUS VALUE_IS_CLASS::Get(INT32 & value)

//  DESCRIPTION     : Get value as INT32.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    value = (INT32) atoi (valueM.c_str());
    return (MSG_OK);
}

//>>===========================================================================

string VALUE_IS_CLASS::GetStripped(void)

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

//>>===========================================================================

DVT_STATUS VALUE_IS_CLASS::CompareStringValues(string ref_value,
                                               bool lead_spc,
                                               bool trail_spc)

//  DESCRIPTION     : Compare the value of the class with the given string.
//                    if leading spaces or trailing spaces are significant, set
//                    the corresponding argument to true.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string      src_value_part;
    string      ref_value_part;
    int         ref_len;
    int         src_len;
    int         max_len;

    src_len = valueM.length();
    ref_len = ref_value.length();


    max_len = src_len;
    if (ref_len > max_len)
    {
        max_len = ref_len;
    }

    // get significant reference string content
    ref_len = StringStrip (ref_value, max_len, lead_spc, trail_spc,
                           ref_value_part);

    // get significant received string content
    src_len = StringStrip (valueM, max_len, lead_spc, trail_spc,
                           src_value_part);

	int src_int_value = atoi(src_value_part.c_str());
	int ref_int_value = atoi(ref_value_part.c_str());

    if (src_int_value == ref_int_value)
    {
        return (MSG_EQUAL);
    }

    if (src_int_value < ref_int_value)
    {
        return (MSG_SMALLER);
    }

    return (MSG_GREATER);
}