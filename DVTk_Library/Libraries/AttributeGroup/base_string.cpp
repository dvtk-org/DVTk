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

//*****************************************************************************
//  DESCRIPTION     : This file contains the implementation for the
//                    base_string class.
//*****************************************************************************


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "base_string.h"

//>>===========================================================================

BASE_STRING_CLASS::BASE_STRING_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

BASE_STRING_CLASS::~BASE_STRING_CLASS()

//  DESCRIPTION     : Default destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

bool BASE_STRING_CLASS::operator = (BASE_STRING_CLASS &)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (false);
}

//>>===========================================================================

DVT_STATUS BASE_STRING_CLASS::Set(string value)

//  DESCRIPTION     : Set the member value to the value passed in the argument
//                    'value'.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : No exception handling is done.
//<<===========================================================================
{
    valueM = value;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS BASE_STRING_CLASS::Set(unsigned char * value, UINT32 length)

//  DESCRIPTION     : Set the member value to the value passed in the argument
//                    value. The length of the value is passed in 'length'.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	char *buffer_ptr = new char [length + 1];
	strncpy(buffer_ptr, (char*) value, length);
	buffer_ptr[length] = '\0';
	valueM = buffer_ptr;
	delete [] buffer_ptr;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS BASE_STRING_CLASS::Get(string &value, bool stripped)

//  DESCRIPTION     : A reference to the value is passed in 'value'.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (stripped)
	{
        value = GetStripped();
	}
    else
	{
        value = valueM;
	}

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS BASE_STRING_CLASS::Get (unsigned char **value, UINT32 &size)

//  DESCRIPTION     : A reference to the value is passed in 'value'. The length
//                    of the value is set in 'size'. The caller is not allowed
//                    to modify the values directly.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : No exception handling is done.
//<<===========================================================================
{
    if (valueM.length() == 0)
    {
        return (MSG_NOT_SET);
    }
    else
    {
        size = valueM.length();
        *value = (unsigned char *)valueM.c_str();

        return (MSG_OK);
    }
}

//>>===========================================================================

UINT32 BASE_STRING_CLASS::GetLength (void)

//  DESCRIPTION     : Returns the length of the string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return valueM.length();
}

//>>===========================================================================

DVT_STATUS BASE_STRING_CLASS::CompareStringValues(string ref_value,
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
    int         compare;

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

    // significant lengths should be the same
    if (ref_len != src_len)
    {
        return (MSG_NOT_SAME_LEN);
    }

    // compare the significant string parts
    compare = src_value_part.compare(0, ref_len, ref_value_part);

    if (compare == 0)
    {
        return (MSG_EQUAL);
    }

    if (compare < 0)
    {
        return (MSG_SMALLER);
    }

    return (MSG_GREATER);
}

//>>===========================================================================

int BASE_STRING_CLASS::StringStrip(string src_string,
                                   int max_length,
                                   bool lead_spc,
                                   bool trail_spc,
                                   string &result)

//  DESCRIPTION     : Strip the given string from a leading and/or trailing
//                    space. This depends on the setting of the corresponding
//                    argument. The resulting string is created in 'result'.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int     length;
    int     start_idx = 0;
    int     end_idx = 0;

    length = src_string.length();
    if (length > max_length)
    {
        length = max_length;
    }

    if (length > 0)
    {
        // Only when the string actually contains characters, do we need to
        // check for insignificant leading and trailing space characters.
        start_idx = 0;
		result = src_string;

        if (lead_spc == false)
        {
            // All leading spaces are insignificant, find the first non
            // space character in the src_string.
            while (src_string[start_idx] == ' ')
            {
                start_idx++;
            }
        }

        end_idx = src_string.length() - 1;

        if ((trail_spc == false) && (end_idx != 0))
        {
            // All trailing spaces are insignificant, find the last non
            // space character in the src_string.
            while (src_string[end_idx] == ' ')
            {
                end_idx--;

				if(end_idx < 0)
				{
					end_idx = 0;
					break;
				}
            }

			result.erase(end_idx + 1);
        }

		result.erase(0, start_idx);
        length = result.length();
    }
    else
    {
        // The length of the src_string is 0, so the result string must be
        // set to empty.
        result = "";
        length = 0;
    }

    return length;
}
