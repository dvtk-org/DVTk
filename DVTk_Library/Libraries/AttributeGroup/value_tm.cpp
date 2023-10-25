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

#include "value_tm.h"

//>>===========================================================================

VALUE_TM_CLASS::VALUE_TM_CLASS()

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

VALUE_TM_CLASS::~VALUE_TM_CLASS()

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

bool VALUE_TM_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_TM)
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

DVT_STATUS VALUE_TM_CLASS::Check (UINT32 flags,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : Checks TM value encoding.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Format: 
//					: A string of charactersof the format hhmmss.frac, where
//					: hh	= hour					
//					: mm 	= minute				
//					: ss	= second				
//					: frac	= fractional part of a second.		
//					:				
//					: Some elements of this format are optional, so the	
//					: actual format becomes:					
//					: hh[mm[ss[.frac]]]				
//					:				
//					: For backward compatibility with older versions of the	
//					: Standard, the format hh:mm:ss.frac should also be	
//					: supported.						
//					:								
//					: Some elements of this format are optional, so the	
//					: actual format becomes:					
//					: hh[:mm[:ss[.frac]]]				
//<<===========================================================================
{
    int         length = valueM.length();
	int     	maxLength;
    int         index;
    DVT_STATUS  result = MSG_OK;
    bool        frac = false;
    bool        range = false;
    bool        time1_old_format = false;
    bool        time2_old_format = false;
    int         time1_index = -1;
    int         time2_index = -1;
    string      time1;
    string      time2;
    string      conv_time1;
    string      conv_time2;
    string      frac1;
    string      frac2;
    char        message[256];

    if (length == 0)
        return (MSG_OK);

    // check length
	if (flags & ATTR_FLAG_RANGES_ALLOWED)
    {
		maxLength = TM_QR_LENGTH;
	}
	else 
    {
		maxLength = TM_LENGTH;
	}

	if (length > maxLength)
	{
		sprintf (message, "value length %i exceeds maximum length %i - truncated for value(%s)",
				 length, maxLength, valueM.c_str());
		messages->AddMessage (VAL_RULE_D_TM_1, message);
		length = maxLength;
		result= MSG_ERROR;
	}	

    for (index=0 ; index<length ; index++)
    {
        if ((valueM[index] >= '0') && (valueM[index] <= '9'))
        {
            if (range == false)
            {
                // We don't know if we're parsing a range. We do know this is the start time in case of a range.
                if (frac == false)
                {
                    time1 += valueM[index];
                    if (time1_index < 0)
                    {
                        // store the index at which the first time starts
                        time1_index = index;
                    }
                }
                else
                {
                    frac1 += valueM[index];
                }
            }
            else
            {
                // We're parsing a range time. The time to parse now is the end time of the range.
                if (frac == false)
                {
                    time2 += valueM[index];
                    if (time2_index < 0)
                    {
                        // store the index at which the second time starts
                        time2_index = index;
                    }
                }
                else
                {
                    frac2 += valueM[index];
                }
            }
        }
        else
        {
            switch (valueM[index])
            {
            case ':':
                if (frac == false)
                {
                    if (range == false)
                    {
                        if (time1_index < 0)
                        {
                            // ':' is not allowed as a starting character
                            sprintf (message, "colon ':' not allowed at position %d",
                                     index);
                            messages->AddMessage (VAL_RULE_D_TM_7, message);
                            return (MSG_ERROR);
                        }
                        time1 += ':';
                        time1_old_format = true;
                    }
                    else
                    {
                        if (time2_index < 0)
                        {
                            // ':' is not allowed as a starting character
                            sprintf (message, "colon ':' not allowed at position %d",
                                    index);
                            messages->AddMessage (VAL_RULE_D_TM_7, message);
                            return (MSG_ERROR);
                        }
                        time2 += ':';
                        time2_old_format = true;
                    }
                }
                else
                {
                    // ':' is not allowed in a frac
                    sprintf (message, "colon ':' not allowed in fraction at position %d",
                            index);
                    messages->AddMessage (VAL_RULE_D_TM_7, message);
                    return (MSG_ERROR);
                }
                break;
            case '-':
                if (!(flags & ATTR_FLAG_RANGES_ALLOWED))
                {
                    sprintf (message, "time range not allowed");
                    messages->AddMessage (VAL_RULE_D_TM_3, message);
                    return (MSG_ERROR);
                }
                if (range == true)
                {
                    sprintf (message, "more than one '-' Character found at position %d",
                            index);
                    messages->AddMessage (VAL_RULE_D_TM_8, message);
                    return (MSG_ERROR);
                }
                range = true;
                // fractions are allowed now since we are now parsing the second time
                frac = false;
                break;
            case '.':
                if (frac == true)
                {
                    sprintf (message, "more than one '.' Character found at position %d",
                             index);
                    messages->AddMessage (VAL_RULE_D_TM_2, message);
                    return MSG_ERROR;
                }
                if ((valueM[index + 1] < '0') || (valueM[index + 1] > '9'))
                {
                    sprintf (message, "'.' found without an actual fraction at position %d",
                            index);
                    messages->AddMessage (VAL_RULE_D_TM_2, message);
                    return MSG_ERROR;
                }
                frac = true;
                break;
            case ' ':
                if ( (index + 1 < length) ||    // A space char can only occur at the end of the time string.
                    ((index + 1) % 2 != 0))     // A space char may only be used for padding
                {
                    sprintf (message, "unexpected SPACE character at offset %d",
                            index+1);
                    messages->AddMessage (VAL_RULE_D_TM_2, message);
                    return MSG_ERROR;
                }
                break;
			case 0x00:
                sprintf (message, "unexpected character [NUL]=0x00 at offset %d",
                         index+1);
                messages->AddMessage (VAL_RULE_D_TM_2, message);
				return MSG_ERROR;
			case 0x0a: // Fall through.
			case 0x0d:
                sprintf (message, "unexpected character 0x%02X at offset %d",
                         (int)(valueM[index]), index+1);
                messages->AddMessage (VAL_RULE_D_TM_2, message);
				return MSG_ERROR;
            default:
                sprintf (message, "unexpected character %c=0x%02X at offset %d",
                         valueM[index], (int)(valueM[index]), index+1);
                messages->AddMessage (VAL_RULE_D_TM_2, message);
                return MSG_ERROR;
            }
        }
    }

    if ((time1_old_format == true) || 
        (time2_old_format == true))
    {
        sprintf (message, "old style time format - 'HH:MM:SS' should not be used");
        messages->AddMessage (VAL_RULE_D_TM_6, message);
    }

    // convert the times from the old format to the new format. This can be
    // done even if the times are already in the new format.
    ConvertOldTime (time1, conv_time1);
    ConvertOldTime (time2, conv_time2);

    if (IsTime (conv_time1, time1_index, messages) != MSG_OK)
    {
        result = MSG_ERROR;
    }
    else
    {
        if (frac1.length() > 0)
        {
            switch (conv_time1.length())
            {
            case 2:
                sprintf (message,
                         "A fraction is not allowed when only HH is specified - expected [HH[MM[SS[.FRAC]]]]");
                messages->AddMessage (VAL_RULE_D_TM_2, message);
                result = MSG_ERROR;
                break;
            case 4:
                sprintf (message, "A fraction is not allowed when only HHMM is specified - expected [HH[MM[SS[.FRAC]]]]");
                messages->AddMessage (VAL_RULE_D_TM_2, message);
                result = MSG_ERROR;
                break;
            case 6:
                // only when HHMMSS is correct, a fraction is allowed.
                if (IsNumeric (frac1, frac1.length()) == false)
                {
                    sprintf (message, "invalid fraction part");
                    messages->AddMessage (VAL_RULE_D_TM_2, message);
                    result = MSG_ERROR;
                }
                break;
            }
        }
    }

    if (range == true)
    {
		if (conv_time2.length() > 0)
		{
			if (IsTime (conv_time2, time2_index, messages) != MSG_OK)
			{
				result = MSG_ERROR;
			}
			else
			{
				if (frac2.length() > 0)
				{
					switch (conv_time2.length())
					{
					case 2:
						sprintf (message, "A fraction is not allowed when only HH is specified - expected [HH[MM[SS[.FRAC]]]]");
						messages->AddMessage (VAL_RULE_D_TM_2, message);
						result = MSG_ERROR;
						break;
					case 4:
						sprintf (message, "A fraction is not allowed when only HHMM is specified - expected [HH[MM[SS[.FRAC]]]]");
						messages->AddMessage (VAL_RULE_D_TM_2, message);
						result = MSG_ERROR;
						break;
					case 6:
						// only when HHMMSS is correct, a fraction is allowed.
						if (IsNumeric (frac2, frac2.length()) == false)
						{
							sprintf (message, "invalid fraction part");
							messages->AddMessage (VAL_RULE_D_TM_2, message);
							result = MSG_ERROR;
						}
						break;
					}
				}
			}

			if (atoi (conv_time1.c_str()) == atoi (conv_time2.c_str()))
			{
				if (atoi (frac1.c_str()) == atoi (frac2.c_str()))
				{
					sprintf (message, "start time and end time in range are equal");
					messages->AddMessage (VAL_RULE_D_TM_A, message);
					result = MSG_ERROR;
				}
				if (atoi (frac1.c_str()) > atoi (frac2.c_str()))
				{
					sprintf (message, "start time is later than end time in range");
					messages->AddMessage (VAL_RULE_D_TM_A, message);
					result = MSG_ERROR;
				}
			}

			if (atoi (conv_time1.c_str()) > atoi (conv_time2.c_str()))
			{
				// No need to check for frac since we already know that time1 is later than time2
				sprintf (message, "start time is later than end time in range");
				messages->AddMessage (VAL_RULE_D_TM_A, message);
				result = MSG_ERROR;
			}
		}
    }

    return result;
}

//>>===========================================================================

DVT_STATUS VALUE_TM_CLASS::Compare(LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS *ref_value)

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

    if (ref_value->GetVRType() != ATTR_VR_TM)
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

DVT_STATUS VALUE_TM_CLASS::Compare(string value)

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

ATTR_VR_ENUM VALUE_TM_CLASS::GetVRType(void)

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_TM);
}

//>>===========================================================================

void VALUE_TM_CLASS::ConvertOldTime(string time, string& conv_time)

//  DESCRIPTION     : Convert old time format to new.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (UINT index=0 ; index<time.length() ; index++)
    {
        if (time[index] != ':')
        {
            conv_time += time[index];
        }
    }
}

//>>===========================================================================

DVT_STATUS VALUE_TM_CLASS::IsTime (string time, int, LOG_MESSAGE_CLASS * messages)

//  DESCRIPTION     : Check if time format is valid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int     length = time.length();
    int     hour;
    int     minute;
    int     second;
    char    message[256];

    if (length > 6)
    {
        sprintf (message, "HHMMSS may not exceed 6 characters. Length %d found.",
                 length);
        messages->AddMessage (VAL_RULE_D_TM_9, message);
        return (MSG_ERROR);
    }

    hour = 0;
    minute = 0;
    second = 0;

    switch (length)
    {
    case 6:
        second = GetNumeric (&time[4],2); // Fall through.
    case 4:
        minute = GetNumeric (&time[2], 2); // Fall through
    case 2:
        hour = GetNumeric (time, 2);
        break;
	case 0:
		break;
    default:
        sprintf (message, "invalid time format - expected [HH[MM[SS[.FRAC]]]]");
        messages->AddMessage (VAL_RULE_D_TM_2, message);
        return (MSG_ERROR);
    }

    if (IsTimeValid (hour, minute, second) == false)
    {
        sprintf (message, "invalid time");
        messages->AddMessage (VAL_RULE_D_TM_5, message);
        return (MSG_ERROR);
    }
    return (MSG_OK);
}

//>>===========================================================================

string VALUE_TM_CLASS::GetStripped(void)

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
