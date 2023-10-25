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

#include "value_dt.h"

//>>===========================================================================

VALUE_DT_CLASS::VALUE_DT_CLASS()

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

VALUE_DT_CLASS::~VALUE_DT_CLASS()

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

bool VALUE_DT_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_DT)
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

DVT_STATUS VALUE_DT_CLASS::Check (UINT32 flags,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : Check DT format.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DVT_STATUS      result = MSG_OK;
    int             length = valueM.length();
	int				maxLength;
    int             index;
    int             frac_index = 0;
    unsigned int    offset_index = 0;
    string          syntax = "";
    int             second;
    int             minute;
    int             hour;
    int             day;
    int             month;
    int             year;
    char            message[256];

    if (length == 0)
        return (MSG_OK);

	if (flags & ATTR_FLAG_RANGES_ALLOWED)
    {
		maxLength = DT_QR_LENGTH;
	}
	else 
    {
		maxLength = DT_LENGTH;
	}
	
	if (length > maxLength)
	{
		sprintf (message, "value length %i exceeds maximum length %i - truncated for value(%s)",
				 length, maxLength, valueM.c_str());
		messages->AddMessage (VAL_RULE_D_DT_1, message);
		length = maxLength;
		result= MSG_ERROR;
	}	

    if (length > 0)
    {
        for (index=0 ; index<length ; index++)
        {
            if ((valueM[index] >= '0') && (valueM[index] <= '9'))
            {
                syntax += 'n';
            }
            else
            {
                switch (valueM[index])
                {
                case '.':
					if (!(flags & ATTR_FLAG_RANGES_ALLOWED))
					{
						if (frac_index != 0) // twice
						{
							sprintf (message, "more then one '.' Character found at offset %d",
									index);
							messages->AddMessage (VAL_RULE_D_DT_2, message);
    						return MSG_ERROR;
						}
					}
                    syntax += '.';
                    frac_index = index + 1;
                    break;
                case '+':   // Fall through
                case '-':
					if (!(flags & ATTR_FLAG_RANGES_ALLOWED))
					{
						if (offset_index != 0) // twice
						{
							sprintf (message, "more then one '+' or '-' Character found at offset %d",
									index);
							messages->AddMessage (VAL_RULE_D_DT_2, message);
    						return MSG_ERROR;
						}
					}
                    syntax += 'S';
                    offset_index = index + 1;
                    break;
                case ' ':
                    if ( ((index + 1) % 2 != 0) ||
                         ((valueM[index+1] != NULLCHAR) && 
                          ((index + 1) < maxLength)
                         )
                       )
                    {
                        sprintf (message, "unexpected SPACE character at offset %d",
                               index+1);
                        messages->AddMessage (VAL_RULE_D_DT_2, message);
    				    return MSG_ERROR;
                    }
                    break;
			    case 0x00:
                    sprintf (message, "unexpected character [NUL]=0x00 at offset %d",
                            index+1);
                    messages->AddMessage (VAL_RULE_D_DT_2, message);
				    return MSG_ERROR;
			    case 0x0a: // Fall through
			    case 0x0d:
                    sprintf (message, "unexpected character 0x%02X at offset %d",
                            (int)(valueM[index]), index+1);
                    messages->AddMessage (VAL_RULE_D_DT_2, message);
				    return MSG_ERROR;
                default:
                    sprintf (message, "unexpected character %c=0x%02X at offset %d",
                            valueM[index], valueM[index], index+1);
                    messages->AddMessage (VAL_RULE_D_DT_2, message);
                    return MSG_ERROR;
                }
            }
        }

        // Deal with offset first. Note that an offset can not be at the
        // beginning of a string.
        if (offset_index != 0)
        {
            if (syntax.find("nnnn", offset_index) != offset_index)
            {
                sprintf (message, "invalid optional offset suffix");
                messages->AddMessage (VAL_RULE_D_DT_2, message);
				return MSG_ERROR;
            }

            hour = GetNumeric (&valueM[offset_index], 2);
            minute = GetNumeric (&valueM[offset_index + 2], 2);

            if (!(IsTimeValid (hour, minute, 0)))
            {
                sprintf (message, "invalid time value specified in offset suffix");
                messages->AddMessage (VAL_RULE_D_DT_2, message);
				return MSG_ERROR;
            }

            // chop offset off syntax string
            index = offset_index - 1;
            syntax[index] = NULLCHAR;
            hour = 0;
            minute = 0;
        }

        // Compensate the index counter used in the for-loop.
        index--;

        // Deal with frac.
		if (!(flags & ATTR_FLAG_RANGES_ALLOWED))
		{
			if ((syntax.find ("nnnnnnnnnnnnnn.n") == 0) && (index <= 21))
			{
				if (! IsNumeric (&valueM[frac_index], index-frac_index))
				{
					sprintf (message, "invalid fractional part specified");
					messages->AddMessage (VAL_RULE_D_DT_2, message);
					return MSG_ERROR;
				}
			}
			else
			{
				if (frac_index > 0)
				{
					sprintf (message,
							 "fraction '.' Character not expected at offset %d",
							 frac_index);
					messages->AddMessage (VAL_RULE_D_DT_2, message);
					return MSG_ERROR;
				}
			}
		}

        // Deal with main date/time stem - various formats
        index = 0;
        while (syntax[index] == 'n')
        {
            index++;
        }

        second = 0;
        minute = 0;
        hour = 0;
        day = 1;
        month = 1;
        year = 1900;

        switch (index)
        {
        case 14:
            second = GetNumeric(&valueM[12], 2); // Fall through
        case 12:
            minute = GetNumeric(&valueM[10], 2); // Fall through
        case 10:
            hour =   GetNumeric(&valueM[8], 2);  // Fall through
        case 8:
            day =    GetNumeric(&valueM[6], 2);  // Fall through
        case 6:
            month =  GetNumeric(&valueM[4], 2);  // Fall through
        case 4:
            year =   GetNumeric(valueM, 4);
            break;
        default:
            sprintf (message, "unexpected components - expected YYYY[MM[DD[HH[MM[SS[.FRAC]]]]]][OFFSET]");
            messages->AddMessage (VAL_RULE_D_DT_2, message);
			return MSG_ERROR;
        }

        if (IsDateValid(year, month, day) == false)
        {
            sprintf (message, "invalid date");
            messages->AddMessage (VAL_RULE_D_DT_3, message);
			return MSG_ERROR;
        }
        if (IsTimeValid(hour, minute, second) == false)
        {
            sprintf (message, "invalid time");
            messages->AddMessage (VAL_RULE_D_DT_3, message);
			return MSG_ERROR;
        }
    }

    return (result);
}

//>>===========================================================================

DVT_STATUS VALUE_DT_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string          ref;
    DVT_STATUS         rval;

    if (ref_value == NULL) return MSG_OK;

    if (ref_value->GetVRType() != ATTR_VR_DT)
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
    rval = CompareStringValues (ref, true, false);

    ref.erase();
    return (rval);
}

//>>===========================================================================

DVT_STATUS VALUE_DT_CLASS::Compare(string value)

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
    rval = CompareStringValues (value, true, false);

    return (rval);
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_DT_CLASS::GetVRType(void)

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_DT);
}

//>>===========================================================================

string VALUE_DT_CLASS::GetStripped(void)

//  DESCRIPTION     : Get stripped string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string  stripped;
    StringStrip (valueM, valueM.length(), true, false, stripped);

    return stripped;
}
