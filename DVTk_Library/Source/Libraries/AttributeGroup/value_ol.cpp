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

#include "value_ob.h"
#include "value_ol.h"
#include "ob_value_stream.h"
#include "ol_value_stream.h"

//>>===========================================================================

VALUE_OL_CLASS::VALUE_OL_CLASS()

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

VALUE_OL_CLASS::~VALUE_OL_CLASS()

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

bool VALUE_OL_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equals operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (value.GetVRType() == ATTR_VR_OL)
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

        value.Get(&parentGroupM_ptr);

        return (true);
    }
    else
    {
        return (false);
    }
}

//>>===========================================================================

UINT32 VALUE_OL_CLASS::GetLength (void)

//  DESCRIPTION     : Return the length of the data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (lengthOkM == false) return 0;

    OL_VALUE_STREAM_CLASS stream;
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

ATTR_VR_ENUM VALUE_OL_CLASS::GetVRType (void)

//  DESCRIPTION     : Get data VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return ATTR_VR_OL;
}

//>>===========================================================================

DVT_STATUS VALUE_OL_CLASS::Compare (LOG_MESSAGE_CLASS *message_ptr, BASE_VALUE_CLASS *refValue)

//  DESCRIPTION     : Compare this against the reference value - reference could be OB or OW.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    DVT_STATUS status = MSG_ERROR;

    string refFilename;
    bool dataInFiles = false;

    if (refValue == NULL) return MSG_OK;

    // VR's must be either OB or OW
    if ((refValue->GetVRType() != ATTR_VR_OB) &&
        (refValue->GetVRType() != ATTR_VR_OL))
    {
        sprintf (message, "Incompatible Reference VR for OL data comparison");
        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_3, message);
        return MSG_INCOMPATIBLE;
    }

    // check if reference data is OB
    if (refValue->GetVRType() == ATTR_VR_OB)
    {
        // reference data is OB
        VALUE_OB_CLASS *refObValue = static_cast<VALUE_OB_CLASS*>(refValue);

        // check if we are comparing the same thing
        // - both in file
        if ((filenameM.length()) &&
            (refObValue->filenameM.length()))
        {
            if (filenameM.find(DATA_NOT_STORED) != string::npos)
            {
                // source data has not been stored
                sprintf (message, "Source OW data has not been stored - can't compare to reference");
                if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_6, message);
                status = MSG_INCOMPATIBLE;
            }
            else
            {
                // check if the filenames are the same - you never know...
                if (filenameM == refObValue->filenameM)
                {
                    status = MSG_EQUAL;
                }
                else
                {
                    // need to compare files
                    refFilename = refObValue->filenameM;
                    if (refFilename.find(DATA_NOT_STORED) != string::npos)
                    {
                        // reference data has not been stored
                        sprintf (message, "Referenced OB data has not been stored - can't compare with source");
                        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_6, message);
                        status = MSG_INCOMPATIBLE;
                    }
                    else
                    {
                        dataInFiles = true;
                    }
                }
            }
        }
 
        else if ((filenameM.length()) ||
            (refObValue->filenameM.length()))
        {
            // one is in file the other a pattern
            // - can't compare
            sprintf (message, "Can't compare In-File and Pattern OL data");
            if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_6, message);
            status = MSG_INCOMPATIBLE;
        }
        else
        {
            // both patterns
            // - can compare
            if ((rowsM == refObValue->rowsM) &&
                (columnsM == refObValue->columnsM) &&
                (start_valueM == refObValue->start_valueM) &&
                (rows_incrementM == refObValue->rows_incrementM) &&
                (columns_incrementM == refObValue->columns_incrementM) &&
                (rows_sameM == refObValue->rows_sameM) &&
                (columns_sameM == refObValue->columns_sameM))
            {
                status = MSG_EQUAL;
            }
            else
            {
                sprintf (message, "Source OW and Reference OB Pattern data different");
                if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_9, message);
                status = MSG_NOT_EQUAL;
            }
        }
    }
    else
    {
        // reference data is OL
        VALUE_OL_CLASS *refOlValue = static_cast<VALUE_OL_CLASS*>(refValue);

        // check if we are comparing the same thing
        // - both in file
        if ((filenameM.length()) &&
            (refOlValue->filenameM.length()))
        {
            if (filenameM.find(DATA_NOT_STORED) != string::npos)
            {
                // source data has not been stored
                sprintf (message, "Source OL data has not been stored - can't compare to reference");
                if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_6, message);
                status = MSG_INCOMPATIBLE;
            }
            else
            {
                // check if the filenames are the same - you never know...
                if (filenameM == refOlValue->filenameM)
                {
                    status = MSG_EQUAL;
                }
                else
                {
                    // need to compare files
                    refFilename = refOlValue->filenameM;
                    if (refFilename.find(DATA_NOT_STORED) != string::npos)
                    {
                        // reference data has not been stored
                        sprintf (message, "Referenced OL data has not been stored - can't compare to source");
                        if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_6, message);
                        status = MSG_INCOMPATIBLE;
                    }
                    else
                    {
                        dataInFiles = true;
                    }
                }
            }
        }
        else if ((filenameM.length()) ||
            (refOlValue->filenameM.length()))
        {
            // one is in file the other a pattern
            // - can't compare
            sprintf (message, "Can't compare In-File and Pattern OL data");
            if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_6, message);
            status = MSG_INCOMPATIBLE;
        }
        else
        {
            // both patterns
            // - can compare
            if ((rowsM == refOlValue->rowsM) &&
                (columnsM == refOlValue->columnsM) &&
                (start_valueM == refOlValue->start_valueM) &&
                (rows_incrementM == refOlValue->rows_incrementM) &&
                (columns_incrementM == refOlValue->columns_incrementM) &&
                (rows_sameM == refOlValue->rows_sameM) &&
                (columns_sameM == refOlValue->columns_sameM))
            {
                status = MSG_EQUAL;
            }
            else
            {
                sprintf (message, "Source OL and Reference OL Pattern data different");
                if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_9, message);
                status = MSG_NOT_EQUAL;
            }
        }
    }

    // check if the data to be compared is in files
    // - it is not necessarily the case that the data in the files has the same VR as thisSrc and thisRef.
    // Therefore we need to read the file headers to determine the actual VR of the data in the files before
    // calling the appropriate comparsion function.
    if (dataInFiles)
    {
        // read the source file
        OL_VALUE_STREAM_CLASS srcOwStream;
        srcOwStream.SetFilename(filenameM);
        srcOwStream.SetLogger(loggerM_ptr);
        ATTR_VR_ENUM srcFileVr = srcOwStream.GetFileVR();

        // read the refence file
        OL_VALUE_STREAM_CLASS refOwStream;
        refOwStream.SetFilename(refFilename);
        refOwStream.SetLogger(loggerM_ptr);
        ATTR_VR_ENUM refFileVr = refOwStream.GetFileVR();

        if ((srcFileVr == ATTR_VR_OL) &&
            (refFileVr == ATTR_VR_OL))
        {
            // we are certain that we are comparing OW with OW
            status = srcOwStream.Compare(message_ptr, refOwStream);
        }
        else if ((srcFileVr == ATTR_VR_OL) &&
            (refFileVr == ATTR_VR_OB))
        {
            // our reference is actually OB - so we need a reference OB stream
            OL_VALUE_STREAM_CLASS refObStream;
            refObStream.SetFilename(refFilename);
            refObStream.SetLogger(loggerM_ptr);

            // compare OW and OB using the OW Compare overload
            status = srcOwStream.Compare(message_ptr, refObStream);
        }
        else if ((srcFileVr == ATTR_VR_OB) &&
            (refFileVr == ATTR_VR_OL))
        {
            // our source is actually OB - so we need a reference OW stream
            OB_VALUE_STREAM_CLASS srcObStream;
            srcObStream.SetFilename(filenameM);
            srcObStream.SetLogger(loggerM_ptr);

            // compare the OB and OW using the OW Compare overload
            status = refOwStream.Compare(message_ptr, srcObStream);
        }
        else
        {
            // any other combination is not allowed
            sprintf (message, "Both Source and Reference File data is OB - expected OB or OL");
            if (message_ptr) message_ptr->AddMessage(VAL_RULE_D_OTHER_11, message);
            status = MSG_INCOMPATIBLE;
        }
    }

    // return the status
    return status;
}

//>>===========================================================================

DVT_STATUS VALUE_OL_CLASS::Check (UINT32,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *,
                                  SPECIFIC_CHARACTER_SET_CLASS *)

//  DESCRIPTION     : Check data VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return MSG_OK;
}
