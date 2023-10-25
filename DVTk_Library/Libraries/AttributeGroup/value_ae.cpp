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

#include "value_ae.h"

//>>===========================================================================

VALUE_AE_CLASS::VALUE_AE_CLASS()

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

VALUE_AE_CLASS::~VALUE_AE_CLASS()

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

bool VALUE_AE_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_AE)
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

DVT_STATUS VALUE_AE_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : This function checks if the value set to the instance of the
//					: AE class is checked for validity.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Format:
//					: A string of characters with leading and trailing spaces
//					: (20H) being non-significant. The value made of 16
//					: spaces, meaning "no application name specified", shall
//					: not be used.
//					: Default Character Repertoire excluding control
//					: characters ESC, LF, FF, and CR.
//<<===========================================================================
{
    unsigned int    index;
    unsigned int    length = valueM.length();
    DVT_STATUS      result  = MSG_OK;
    unsigned int    nr_spaces;
    char            message[256];

    if (length == 0)
        return (MSG_OK);

    if (length > AE_LENGTH)
    {
        sprintf (message, "value length %i exceeds maximum length %i - truncated",
                length, AE_LENGTH);
        messages->AddMessage (VAL_RULE_D_AE_1, message);
        length = AE_LENGTH;
        result = MSG_ERROR;
    }

    if (length)
    {
        // check for ESC, LF, FF or CR
        for (index = 0; index < length; index++)
        {
            if ((valueM[index] == LINEFEED) ||
                (valueM[index] == FORMFEED) ||
                (valueM[index] == CARRIAGERETURN) ||
                (valueM[index] == ESCAPE))
            {
                sprintf (message, "unexpected control character 0x%02X at offset %d",
                        (int) valueM[index], index+1);
                messages->AddMessage (VAL_RULE_D_AE_2, message);
                result = MSG_ERROR;
            }
        }

        // Only check if we still don't have any errors
        if (result == MSG_OK)
        {
            nr_spaces = 0;
            // check for 16 SPACE
            for (index = 0; index < length; index++)
            {
                if (valueM[index] == SPACECHAR)
                {
                    nr_spaces++;
                }
            }

            if (nr_spaces == length)
            {
                sprintf (message, "contains only SPACE Characters");
                messages->AddMessage (VAL_RULE_D_AE_3, message);
                result = MSG_ERROR;
            }
        }
    }

    return (result);
}

//>>===========================================================================

DVT_STATUS VALUE_AE_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string          ref;
    DVT_STATUS         rval;

    if ((ref_value != NULL) &&
        (ref_value->GetVRType() != ATTR_VR_AE))
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

DVT_STATUS VALUE_AE_CLASS::Compare(string value)

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

ATTR_VR_ENUM VALUE_AE_CLASS::GetVRType()

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_AE);
}

//>>===========================================================================

string VALUE_AE_CLASS::GetStripped(void)

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
