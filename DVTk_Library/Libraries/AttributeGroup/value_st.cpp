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

#include "value_st.h"
#include "ext_char_set_definition.h"


//>>===========================================================================

VALUE_ST_CLASS::VALUE_ST_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	sizeM = 0;
    valueM = NULL;
}

//>>===========================================================================

VALUE_ST_CLASS::~VALUE_ST_CLASS()

//  DESCRIPTION     : Default destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (valueM)
    {
        free(valueM);
    }
}

//>>===========================================================================

DVT_STATUS VALUE_ST_CLASS::Set(string value)

//  DESCRIPTION     : Set value from string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // First free the contents of the instance of this class. If there is
    // no content, the function will do nothing.
    free (valueM);

    sizeM = value.length();
    valueM = (unsigned char *)malloc ((sizeof (unsigned char)) * (sizeM + 1));
    memcpy (valueM, value.c_str(), sizeM);

    // null terminate the buffer - required if this is cast to a string in Get()
    valueM[sizeM] = NULLCHAR;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_ST_CLASS::Set (unsigned char *value, UINT32 size)

//  DESCRIPTION     : Set value to size.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    sizeM = size;
    valueM = (unsigned char *)malloc ((sizeof (unsigned char)) * (sizeM + 1));
    memcpy (valueM, value, sizeM);

    // null terminate the buffer - required if this is cast to a string in Get()
    valueM[sizeM] = NULLCHAR;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_ST_CLASS::Get (unsigned char **value, UINT32 &size)

//  DESCRIPTION     : Get value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (valueM == NULL)
    {
        return (MSG_NOT_SET);
    }
    else
    {
        *value = valueM;
        size = sizeM;

        return (MSG_OK);
    }
}

//>>===========================================================================

DVT_STATUS VALUE_ST_CLASS::Get(string &value, bool)

//  DESCRIPTION     : Get the value as a string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // buffer is null terminated in Set()
    value = (char*)valueM;

    return MSG_OK;
}

//>>===========================================================================

bool VALUE_ST_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_ST)
    {
        unsigned char   * tmp_val;

        value.Get (&tmp_val, sizeM);
        valueM = (unsigned char *)malloc ((sizeof (unsigned char)) * (sizeM + 1));
        memcpy (valueM, tmp_val, sizeM);

        // null terminate the buffer - required if this is cast to a string in Get()
        valueM[sizeM] = NULLCHAR;

        return (true);
    }
    else
    {
        return (false);
    }
}

//>>===========================================================================

DVT_STATUS VALUE_ST_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *specific_character_set)

//  DESCRIPTION     : Check ST format.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DVT_STATUS  result = MSG_OK;
    char        message[256];

    if (sizeM == 0)
        return (MSG_OK);

    // check max length
    if (sizeM > ST_LENGTH)
    {
        sprintf (message, "value length %i exceeds maximum length %i",
                 sizeM, SH_LENGTH);
        messages->AddMessage (VAL_RULE_D_ST_1, message);
        result = MSG_ERROR;
    }

    // check the VR of the value
    DVT_STATUS result1 = EXTCHARACTERSET->IsValidExtendedChar(valueM, sizeM, ATTR_VR_ST, ST_CHAR_LENGTH, messages, specific_character_set);
    if (result == MSG_OK) result = result1;

    return (result);
}

//>>===========================================================================

DVT_STATUS VALUE_ST_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    unsigned char   * ref_val;
    UINT32            ref_len;

    if (ref_value == NULL) return MSG_OK;

    if (ref_value->GetVRType() != ATTR_VR_ST)
    {
        return (MSG_INCOMPATIBLE);
    }

    DVT_STATUS rval = ref_value->Get(&ref_val, ref_len);
    if (rval != MSG_OK)
    {
        return (rval);
    }

    // Check for zero length
    if ((sizeM == 0) && 
        (ref_len == 0))
    {
        // values considered the same
        return (MSG_EQUAL);
    }

    // check if lengths vary by 1 - can be the SPACE padding added during the encoding to produce an even length
    UINT32 length_to_compare = ref_len;
    if ((ref_len == sizeM + 1) &&
        !(ref_len & 0x1))
    {
        // check if the last ref value is SPACE
        if (ref_val[ref_len-1] == SPACECHAR)
        {
            length_to_compare = sizeM;
        }
        else
        {
            return (MSG_NOT_EQUAL);
        }
    }
    else if ((ref_len + 1 == sizeM) &&
        !(sizeM & 0x01))
    {
        // check if the last this value is SPACE
        if (valueM[sizeM-1] == SPACECHAR)
        {
            length_to_compare = ref_len;
        }
        else
        {
            return (MSG_NOT_EQUAL);
        }
    }
    else if (ref_len != sizeM)
    {
        return (MSG_NOT_SAME_LEN);
    }

    // We know we have data, and we have the reference value. Now compare these.
    if (memcmp (valueM, ref_val, length_to_compare) == 0)
    {
        rval = MSG_EQUAL;
    }
    else
    {
        rval = MSG_NOT_EQUAL;
    }

    return (rval);
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_ST_CLASS::GetVRType(void)

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_ST);
}

//>>===========================================================================

UINT32 VALUE_ST_CLASS::GetLength (void)

//  DESCRIPTION     : Get length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return sizeM;
}
