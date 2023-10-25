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

#include "value_ds.h"
#include <math.h>

//>>===========================================================================

VALUE_DS_CLASS::VALUE_DS_CLASS()

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

VALUE_DS_CLASS::~VALUE_DS_CLASS()

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

bool VALUE_DS_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_DS)
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

DVT_STATUS VALUE_DS_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : This function checks if the value set to the instance of the
//					: DS class is checked for validity.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Format:
//					: A string of characters with leading and trailing spaces
//					: (20H) being non-significant.
//					: spaces, meaning "no application name specified", shall
//					: not be used.
//					: Uppercase characters, "0"-"9", SPACE and UNDERSCORE of
//					: default character repertoire.
//<<===========================================================================
{
    UINT        index;
    UINT        length  = valueM.length();
    DVT_STATUS  result = MSG_OK;
    bool        integer_part = false;
    bool        fractional_part = false;
    bool        exponent_part = false;
    char        message[256];

    if (length == 0)
        return (MSG_OK);

    // check length
    if (length > DS_LENGTH)
    {
        sprintf (message, "value length %i exceeds maximum length %i - truncated",
                length, DS_LENGTH);
        messages->AddMessage (VAL_RULE_D_DS_1, message);
        length = DS_LENGTH;
        result = MSG_ERROR;
    }

    if (length > 0)
    {
        index = 0;

        // Skip all leading spaces
        while ((valueM[index] == SPACECHAR) && (index < length))
        {
            index++;
        }

        // Parse basic real constant
        if ((valueM[index] == '+') || (valueM[index] == '-'))
        {
            index++;
        }
        while ((valueM[index] >= '0') && (valueM[index] <= '9') &&
               (index < length))
        {
            index++;
            integer_part = true;
        }
        if (valueM[index] == '.')
        {
            index++;
        }
        while ((valueM[index] >= '0') && (valueM[index] <= '9') &&
               (index < length))
        {
            index++;
            fractional_part = true;
        }
        if (!integer_part && !fractional_part)
        {
            sprintf (message, "string does not start with a valid expression");
            messages->AddMessage (VAL_RULE_D_DS_2, message);
            return (MSG_ERROR);
        }

        // Parse real exponent
        if ((valueM[index] == 'E') || (valueM[index] == 'e'))
        {
            index++;
            if ((valueM[index] == '+') || (valueM[index] == '-'))
            {
                index++;
            }
            while ((valueM[index] >= '0') && (valueM[index] <= '9') &&
                   (index < length))
            {
                index++;
                exponent_part = true;
            }
            if (!exponent_part)
            {
                sprintf (message, "invalid exponent specified");
                messages->AddMessage (VAL_RULE_D_DS_3, message);
                return (MSG_ERROR);
            }
        }

        // Skip trailing spaces
        while ((valueM[index] == SPACECHAR) && (index < length))
        {
            index++;
        }

        // Check end of string
        if (valueM[index] != NULLCHAR)
        {
            sprintf (message, "unexpected character %c=0x%02X at offset %d",
                    valueM[index], valueM[index], index+1);
            messages->AddMessage (VAL_RULE_D_DS_4, message);
            result = MSG_ERROR;
        }
    }
    else
    {
        // Length = 0. So this value is considered to be not decimal.
        result = MSG_ERROR;
    }

    return result;
}

//>>===========================================================================

DVT_STATUS VALUE_DS_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string      ref_ds;
    double      src_val;
    double      ref_val;
    double      epsilon;
    DVT_STATUS     result;

    if (ref_value == NULL) return MSG_OK;

    if (ref_value->GetVRType() != ATTR_VR_DS)
    {
        return (MSG_INCOMPATIBLE);
    }

    ref_value->Get(ref_ds);

    src_val = atof (valueM.c_str());
    ref_val = atof (ref_ds.c_str());
    epsilon = GetEpsilon ();

    // check if the difference is within the allowed tolerance
    if (fabs(src_val - ref_val) < epsilon)
    {
        // values are deemed to be equal
        result = MSG_EQUAL;
    }
    else
    {
        if (src_val > ref_val)
        {
            // reference is greater than received value
            result = MSG_GREATER;
        }
        else
        {
            // reference is less than received value
            result = MSG_SMALLER;
        }
    }

    return (result);
}

//>>===========================================================================

DVT_STATUS VALUE_DS_CLASS::Compare(string value)

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

ATTR_VR_ENUM VALUE_DS_CLASS::GetVRType(void)

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_DS);
}


//>>===========================================================================

double VALUE_DS_CLASS::GetEpsilon(void)

//  DESCRIPTION     : Determines the allowed error from the input string
//	                : using the following algorithm:					
//	                : - if DS contains no fraction then epsilon = 0.5 * 10^exp	
//	                : - if DS contains a fraction then 0.5 (order of frac) * 10^exp	
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function assumes that the DS represents a normal 	
//		              float: -number[.frac][e-exp]				
//<<===========================================================================
{
    UINT      index = 0;
    int       e = 1;
    double    epsilon = 0.5;

    // The number of digits in the fraction determine the precision.
    // First find the fraction part.
    while ((valueM[index] != '.') && (index < valueM.length()))
    {
        index++;
    }
    if (index < valueM.length())
    {
        // Fraction part available.
        while ((valueM[index] >= '0') && (valueM[index] <= '9') &&
               (index < valueM.length()))
        {
            index++;
            epsilon /= 10;
        }
    }

    // Determine if the exponent is given.
    index = 0;
    while ((valueM[index] != 'e') && (valueM[index] != 'E') &&
           (index < valueM.length()))
    {
        index++;
    }
    if (index < valueM.length())
    {
        index++;

		// Check sign of exponent
        if ((valueM[index] == '-') || (valueM[index] == '+'))
        {
            if (valueM[index] == '-')
            {
                e = -e;
            }
            index++;
        }

        // Determine exponent value
        while ((valueM[index] >= '0') && (valueM[index] <= '9') &&
               (index < valueM.length()))
        {
            e = e * 10 + (valueM[index] - '0');
            index++;
        }
    }
    return epsilon;
}

//>>===========================================================================

string VALUE_DS_CLASS::GetStripped(void)

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
