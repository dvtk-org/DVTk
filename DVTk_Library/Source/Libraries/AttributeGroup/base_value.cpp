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
//                    base_value class.
//*****************************************************************************


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "base_value.h"
#include "value_ae.h"
#include "value_as.h"
#include "value_at.h"
#include "value_cs.h"
#include "value_da.h"
#include "value_ds.h"
#include "value_dt.h"
#include "value_fd.h"
#include "value_fl.h"
#include "value_is.h"
#include "value_lo.h"
#include "value_lt.h"
#include "value_ob.h"
#include "value_of.h"
#include "value_ow.h"
#include "value_pn.h"
#include "value_sh.h"
#include "value_sl.h"
#include "value_sq.h"
#include "value_ss.h"
#include "value_st.h"
#include "value_tm.h"
#include "value_ui.h"
#include "value_ul.h"
#include "value_un.h"
#include "value_us.h"
#include "value_ut.h"
#include "value_uc.h"
#include "value_ur.h"
#include "value_od.h"
#include "value_ol.h"
#include "value_ov.h"

//>>===========================================================================

BASE_VALUE_CLASS * CreateNewValue (ATTR_VR_ENUM vr)

//  DESCRIPTION     : This is a global function that creates a new value class
//					: based on the vr passed to this function.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    switch (vr)
    {
    case ATTR_VR_AE:    return (new (VALUE_AE_CLASS));
    case ATTR_VR_AS:    return (new (VALUE_AS_CLASS));
    case ATTR_VR_AT:    return (new (VALUE_AT_CLASS));
    case ATTR_VR_CS:    return (new (VALUE_CS_CLASS));
    case ATTR_VR_DA:    return (new (VALUE_DA_CLASS));
    case ATTR_VR_DS:    return (new (VALUE_DS_CLASS));
    case ATTR_VR_DT:    return (new (VALUE_DT_CLASS));
    case ATTR_VR_FD:    return (new (VALUE_FD_CLASS));
    case ATTR_VR_FL:    return (new (VALUE_FL_CLASS));
    case ATTR_VR_IS:    return (new (VALUE_IS_CLASS));
    case ATTR_VR_LO:    return (new (VALUE_LO_CLASS));
    case ATTR_VR_LT:    return (new (VALUE_LT_CLASS));
    case ATTR_VR_OB:    return (new (VALUE_OB_CLASS));
    case ATTR_VR_OF:    return (new (VALUE_OF_CLASS));
    case ATTR_VR_OW:    return (new (VALUE_OW_CLASS));
	case ATTR_VR_OD:    return (new (VALUE_OD_CLASS));
	case ATTR_VR_OL:    return (new (VALUE_OL_CLASS));
	case ATTR_VR_OV:    return (new (VALUE_OV_CLASS));
    case ATTR_VR_PN:    return (new (VALUE_PN_CLASS));
    case ATTR_VR_SH:    return (new (VALUE_SH_CLASS));
    case ATTR_VR_SL:    return (new (VALUE_SL_CLASS));
    case ATTR_VR_SQ:    return (new (VALUE_SQ_CLASS));
    case ATTR_VR_SS:    return (new (VALUE_SS_CLASS));
    case ATTR_VR_ST:    return (new (VALUE_ST_CLASS));
    case ATTR_VR_TM:    return (new (VALUE_TM_CLASS));
    case ATTR_VR_UI:    return (new (VALUE_UI_CLASS));
    case ATTR_VR_UL:    return (new (VALUE_UL_CLASS));
    case ATTR_VR_UN:    return (new (VALUE_UN_CLASS));
    case ATTR_VR_US:    return (new (VALUE_US_CLASS));
    case ATTR_VR_UT:    return (new (VALUE_UT_CLASS));
	case ATTR_VR_UC:    return (new (VALUE_UC_CLASS));
	case ATTR_VR_UR:    return (new (VALUE_UR_CLASS));
    default:
                        // The vr passed to this function is unknown
                        // should we display some kind of message here?
                        return (NULL);
    }
}

//>>===========================================================================

BASE_VALUE_CLASS::~BASE_VALUE_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

UINT32 BASE_VALUE_CLASS::GetMaximumVrLength()

//  DESCRIPTION     : Return the maximum length allowed for this VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    switch (GetVRType())
    {
    case ATTR_VR_AE:    return AE_LENGTH;
    case ATTR_VR_AS:    return AS_LENGTH;
    case ATTR_VR_AT:    return AT_LENGTH;
    case ATTR_VR_CS:    return CS_LENGTH;
    case ATTR_VR_DA:    return DA_QR_LENGTH;
    case ATTR_VR_DS:    return DS_LENGTH;
    case ATTR_VR_DT:    return DT_QR_LENGTH;
    case ATTR_VR_FD:    return FD_LENGTH;
    case ATTR_VR_FL:    return FL_LENGTH;
    case ATTR_VR_IS:    return IS_LENGTH;
    case ATTR_VR_LO:    return LO_LENGTH;
    case ATTR_VR_LT:    return LT_LENGTH;
    case ATTR_VR_OB:    return OB_LENGTH;
    case ATTR_VR_OF:    return OF_LENGTH;
	case ATTR_VR_OD:    return OD_LENGTH;
	case ATTR_VR_OL:    return OL_LENGTH;
    case ATTR_VR_OW:    return OW_LENGTH;
    case ATTR_VR_OV:    return OV_LENGTH;
    case ATTR_VR_PN:    return PN_LENGTH;
    case ATTR_VR_SH:    return SH_LENGTH;
    case ATTR_VR_SL:    return SL_LENGTH;
    case ATTR_VR_SQ:    return SQ_LENGTH;
    case ATTR_VR_SS:    return SS_LENGTH;
    case ATTR_VR_ST:    return ST_LENGTH;
    case ATTR_VR_TM:    return TM_QR_LENGTH;
    case ATTR_VR_UI:    return UI_LENGTH;
    case ATTR_VR_UL:    return UL_LENGTH;
    case ATTR_VR_UN:    return UN_LENGTH;
    case ATTR_VR_US:    return US_LENGTH;
    case ATTR_VR_UT:    return UT_LENGTH;
	case ATTR_VR_UR:    return UR_LENGTH;
	case ATTR_VR_UC:    return UC_LENGTH;
    default: return 0;
    }
}

//>>===========================================================================

bool BASE_VALUE_CLASS::operator = (BASE_VALUE_CLASS &)

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

DVT_STATUS BASE_VALUE_CLASS::Set(string)

//  DESCRIPTION     : Set value from string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(float)

//  DESCRIPTION     : Set value from float.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(double)

//  DESCRIPTION     : Set value from double.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(unsigned char *, UINT32)

//  DESCRIPTION     : Set value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(UINT32)

//  DESCRIPTION     : Set value from UINT32.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(UINT16)

//  DESCRIPTION     : Set value from UINT16.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(INT32)

//  DESCRIPTION     : Set value from INT32.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(INT16)

//  DESCRIPTION     : Set value from INT16.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Set(ATTRIBUTE_GROUP_CLASS *)

//  DESCRIPTION     : Set attribute group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Add (UINT32)

//  DESCRIPTION     : Add UINT32 value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_INCOMPATIBLE);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(string &, bool)

//  DESCRIPTION     : Get value to string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(float &)

//  DESCRIPTION     : Get value to float.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(double &)

//  DESCRIPTION     : Get value to double.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(unsigned char **, UINT32 &)

//  DESCRIPTION     : Get value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(UINT32 &)

//  DESCRIPTION     : Get value to UINT32.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(UINT16 &)

//  DESCRIPTION     : Get value to UINT16.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(INT32 &)

//  DESCRIPTION     : Get value to INT32.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(INT16 &)

//  DESCRIPTION     : Get value to INT16.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get(ATTRIBUTE_GROUP_CLASS **, int)

//  DESCRIPTION     : Get attribute group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Get (UINT32, UINT32 &)

//  DESCRIPTION     : Get indexed value to UINT32.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_NOT_SET);
}

//>>===========================================================================

DVT_STATUS BASE_VALUE_CLASS::Compare(string)

//  DESCRIPTION     : Compare value to string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (MSG_ERROR);
}

//>>===========================================================================

int BASE_VALUE_CLASS::GetNrItems (void)

//  DESCRIPTION     : Get number of items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (0);
}

//>>===========================================================================

int BASE_VALUE_CLASS::GetNumeric(string value, int nr_digits)

//  DESCRIPTION     : Get numeric value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int int_value = 0;
    unsigned int index;
    unsigned int length;

	assert (nr_digits >= 0);

	length = (unsigned int) nr_digits;
    if (length > value.length())
    {
        length = value.length();
    }

    // No checks are made if the characters in the string are actually
    // numeric.
    for (index = 0; index < length; index++)
    {
        int_value = (int_value * 10) + value[index] - '0';
    }

    return (int_value);
}

//>>===========================================================================

bool BASE_VALUE_CLASS::IsNumeric(string num_value, int nr_digits)

//  DESCRIPTION     : Check is value is numeric.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int     index;

	assert (nr_digits >= 0);
    if (num_value.length() < (unsigned int) nr_digits)
    {
        return (false);
    }

    for (index=0 ; index<nr_digits ; index++)
    {
        if (!isdigit (num_value[index]))
        {
            return (false);
        }
    }
    return (true);
}

//>>===========================================================================

bool BASE_VALUE_CLASS::IsTimeValid(int hour, int minute, int second)

//  DESCRIPTION     : Check time is valid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool timeValid = false;

    // Values can only be positive.
    if ( (hour <= 23)   &&
         (minute <= 59) &&
         (second <= 59)
        )
    {
        timeValid = true;
    }

    return timeValid;
}

//>>===========================================================================

bool BASE_VALUE_CLASS::IsDateValid(int year, int month, int day)

//  DESCRIPTION     : Check date is valid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool    date_valid = false;
    int     last_day;

    if ((0 <= year) && 
		(year <= 9999))
    {
        if ((1 <= month) && 
			(month <= 12))
        {
            last_day = DaysInMonth(month);

            // check for leap year - every 4 years, except
            // for centuries not a multiple of 400
            if ((((year % 4 == 0) && 
				(year % 100 != 0)) || 
				(year % 400 == 0)) && 
				(month == 2))
            {
                // in a leapyear February has 29 days
                last_day = 29;
            }

            if ((1 <= day) && 
				(day <= last_day))
            {
                date_valid = true;
            }
        }
    }

    return date_valid;
}

//>>===========================================================================

int BASE_VALUE_CLASS::DaysInMonth(int month)

//  DESCRIPTION     : Get number of days in the given month.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    int days = 0;

    switch (month)
    {
    case 1:     days = 31;  break;  // January
    case 2:     days = 28;  break;  // February
    case 3:     days = 31;  break;  // March
    case 4:     days = 30;  break;  // April
    case 5:     days = 31;  break;  // May
    case 6:     days = 30;  break;  // June
    case 7:     days = 31;  break;  // July
    case 8:     days = 31;  break;  // August
    case 9:     days = 30;  break;  // September
    case 10:    days = 31;  break;  // October
    case 11:    days = 30;  break;  // November
    case 12:    days = 31;  break;  // December
    }

    return (days);
}
