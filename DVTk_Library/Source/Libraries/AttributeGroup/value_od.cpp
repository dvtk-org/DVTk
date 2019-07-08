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

#include "value_od.h"
#include "od_value_stream.h"


//>>===========================================================================

VALUE_OD_CLASS::VALUE_OD_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // constructor activities
}

//>>===========================================================================

VALUE_OD_CLASS::~VALUE_OD_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // destructor activities
}

//>>===========================================================================

bool VALUE_OD_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equals operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_OD)
    {
        if (filenameM.length() > 0)
        {
            value.Get (filenameM);
        }
        else
        {
            value.Get ((UINT32) 0, rowsM);
            value.Get ((UINT32) 1, columnsM);
            value.Get ((UINT32) 2, start_valueM);
            value.Get ((UINT32) 3, rows_incrementM);
            value.Get ((UINT32) 4, columns_incrementM);
            value.Get ((UINT32) 5, rows_sameM);
            value.Get ((UINT32) 6, columns_sameM);
        }

        value.Get (&parentGroupM_ptr);

        return (true);
    }
    else
    {
        return (false);
    }
}

//>>===========================================================================

UINT32 VALUE_OD_CLASS::GetLength (void)

//  DESCRIPTION     : Return the length of the data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (lengthOkM == false) return 0;

    OD_VALUE_STREAM_CLASS stream;
    stream.SetLogger(GetLogger());
    if (filenameM.length())
    {
        stream.SetFilename(filenameM);
    }
    else
    {
        stream.SetPatternValues(rowsM, columnsM,
                                start_valueM,
                                rows_incrementM, columns_incrementM,
                                rows_sameM, columns_sameM);
    }

    // update the stream with the appropriate attribute values
    stream.UpdateData(this->GetBitsAllocated(), this->GetSamplesPerPixel(), this->GetPlanarConfiguration());

    UINT32 length = stream.GetLength(&lengthOkM);

    return length;
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_OD_CLASS::GetVRType (void)

//  DESCRIPTION     : Return the VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return ATTR_VR_OD;
}

//>>===========================================================================

DVT_STATUS VALUE_OD_CLASS::Compare (LOG_MESSAGE_CLASS *message_ptr, BASE_VALUE_CLASS *refValue)

//  DESCRIPTION     : Compare this against the reference OF value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    DVT_STATUS status = MSG_ERROR;

    if (refValue == NULL) return MSG_OK;

    // VR's must be the same
    if (refValue->GetVRType() != ATTR_VR_OD)
    {
        sprintf (message, "Incompatible Reference VR for OD data comparison");
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_2, message);
        return MSG_INCOMPATIBLE;
    }
    VALUE_OD_CLASS *refOdValue = static_cast<VALUE_OD_CLASS*>(refValue);

    // check if we are comparing the same thing
    // - both in file
    if ((filenameM.length()) &&
        (refOdValue->filenameM.length()))
    {
        // check if the filenames are the same - you never know...
        if (filenameM == refOdValue->filenameM)
        {
            status = MSG_EQUAL;
        }
        else
        {
            // set up the source stream
            OD_VALUE_STREAM_CLASS srcStream;
            srcStream.SetFilename(filenameM);
            srcStream.SetLogger(loggerM_ptr);
            
            // check that the source file really does contain OF data
            if (srcStream.GetFileVR() == ATTR_VR_OD)
            {
                // set up the reference stream
                OD_VALUE_STREAM_CLASS refStream;
                refStream.SetFilename(refOdValue->filenameM);
                refStream.SetLogger(loggerM_ptr);
            
                // check that the reference file really does contain OF data
                if (refStream.GetFileVR() == ATTR_VR_OD)
                {
                    // now compare the stream data
                    status = srcStream.Compare(message_ptr, refStream);
                }
            }

        }
    }
    else if ((filenameM.length()) ||
        (refOdValue->filenameM.length()))
    {
        // one is in file the other a pattern
        // - can't compare
        sprintf (message, "Can't compare In-File and Pattern OD data");
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_5, message);
        status = MSG_INCOMPATIBLE;
    }
    else
    {
        // both patterns
        // - can compare
        if ((rowsM == refOdValue->rowsM) &&
            (columnsM == refOdValue->columnsM) &&
            (start_valueM == refOdValue->start_valueM) &&
            (rows_incrementM == refOdValue->rows_incrementM) &&
            (columns_incrementM == refOdValue->columns_incrementM) &&
            (rows_sameM == refOdValue->rows_sameM) &&
            (columns_sameM == refOdValue->columns_sameM))
        {
            status = MSG_EQUAL;
        }
        else
        {
            sprintf (message, "Source and Reference OD Pattern data different");
            if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_8, message);
            status = MSG_NOT_EQUAL;
        }
    }

    // return the status
    return status;
}

//>>===========================================================================

DVT_STATUS VALUE_OD_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : Check the data VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return MSG_OK;
}
