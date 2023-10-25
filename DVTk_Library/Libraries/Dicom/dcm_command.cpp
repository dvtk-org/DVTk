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
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_command.h"

//>>===========================================================================

DCM_COMMAND_CLASS::DCM_COMMAND_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    DIMSE_CMD_ENUM commandId = DIMSE_CMD_UNKNOWN;
    // constructor activities
    defineGroupLengthsM = true;
    commandIdM = commandId;
    isRequestM = isRequest(commandId);
    widTypeM = WAREHOUSE->dimse2widtype(commandId);
    return;
}

//>>===========================================================================

DCM_COMMAND_CLASS::DCM_COMMAND_CLASS(DIMSE_CMD_ENUM commandId)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // constructor activities
    defineGroupLengthsM = true;
    commandIdM = commandId;
    isRequestM = isRequest(commandId);
    widTypeM = WAREHOUSE->dimse2widtype(commandId);
    return;
}

//>>===========================================================================

DCM_COMMAND_CLASS::DCM_COMMAND_CLASS(DIMSE_CMD_ENUM commandId, string identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // constructor activities
    defineGroupLengthsM = true;
    commandIdM = commandId;
    isRequestM = isRequest(commandId);
    widTypeM = WAREHOUSE->dimse2widtype(commandId);
    // set identifier
    identifierM = identifier;
    return;
}

//>>===========================================================================

DCM_COMMAND_CLASS::~DCM_COMMAND_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // destructor activities
    return;
}

//>>===========================================================================

bool DCM_COMMAND_CLASS::isRequest(DIMSE_CMD_ENUM commandId)

//  DESCRIPTION     : Check if command is a request or response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    bool isRequest;
    // check if we have a request
    switch(commandId)
    {
        // check all known request types
    case DIMSE_CMD_CECHO_RQ:
    case DIMSE_CMD_CFIND_RQ:
    case DIMSE_CMD_CGET_RQ:
    case DIMSE_CMD_CMOVE_RQ:
    case DIMSE_CMD_CSTORE_RQ:
    case DIMSE_CMD_CCANCEL_RQ:
    case DIMSE_CMD_NACTION_RQ:
    case DIMSE_CMD_NCREATE_RQ: 
    case DIMSE_CMD_NDELETE_RQ:
    case DIMSE_CMD_NEVENTREPORT_RQ:
    case DIMSE_CMD_NGET_RQ:
    case DIMSE_CMD_NSET_RQ:
    case DIMSE_CMD_UNKNOWN:
        isRequest = true;
        break;
    default:
        // must be a response
        isRequest = false;
        break;
    }
    // return result
    return isRequest;
}

//>>===========================================================================

bool DCM_COMMAND_CLASS::encode(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : encode DICOM command to dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // check if we need to add the group length attributes
    if (defineGroupLengthsM) 
    {
        // add group length(s) - attributes sorted here
        addGroupLengths();

        // update group length(s)
        setGroupLengths(dataTransfer.getTsCode());
    }
    else
    {
        // sort the DICOM command attributes into ascending order
        SortAttributes();
    }
    // set the transfer VR - used by the logger
    if (dataTransfer.isExplicitVR())
    {
        // set to explicit
        setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
    }
    else
    {
        // set to implicit
        setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
    }
    // encode the command attributes
    return DCM_ATTRIBUTE_GROUP_CLASS::encode(dataTransfer);
}

//>>===========================================================================

bool DCM_COMMAND_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this command with the contents of the command given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    bool result = false;
    //
    // ensure update WID is a command
    //
    if (wid_ptr->getWidType() == widTypeM)
    {
        DCM_COMMAND_CLASS *updateCommand_ptr = static_cast<DCM_COMMAND_CLASS*>(wid_ptr);
        //
        // we can perform the update by merging the update command into this
        //
        merge(updateCommand_ptr);
        //
        // sort attributes after update
        //
        SortAttributes();
        //
        // result is OK
        //
        result = true;
    }
    return result;
}

//>>===========================================================================

DIMSE_CMD_ENUM DCM_COMMAND_CLASS::getCommandId()

//  DESCRIPTION     : Get Command ID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    return commandIdM;
}

//>>===========================================================================

void DCM_COMMAND_CLASS::setCommandId(DIMSE_CMD_ENUM commandId)

//  DESCRIPTION     : Set Command ID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    commandIdM = commandId; 
    return;
}

//>>===========================================================================

bool DCM_COMMAND_CLASS::getIsRequest()

//  DESCRIPTION     : Determine whether the command is a request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    return isRequestM;
}

//>>===========================================================================

void DCM_COMMAND_CLASS::setIsRequest(bool flag)

//  DESCRIPTION     : Set the command to be a request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    isRequestM = flag;
    return;
}

//>>===========================================================================

DCM_COMMAND_CLASS* DCM_COMMAND_CLASS::cloneAttributes()

//  DESCRIPTION     : Clone the Command (but only by copying the underlying attributes).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// Clone this command - by copying attributes only
	DCM_COMMAND_CLASS *command_ptr = new DCM_COMMAND_CLASS();
	command_ptr->DCM_ATTRIBUTE_GROUP_CLASS::cloneAttributes(this);

	return command_ptr;
}