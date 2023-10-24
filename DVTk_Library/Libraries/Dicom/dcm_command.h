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

#ifndef DCM_COMMAND_H
#define DCM_COMMAND_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_attribute_group.h"

//>>***************************************************************************

class DCM_COMMAND_CLASS : public DCM_ATTRIBUTE_GROUP_CLASS

    //  DESCRIPTION     : DICOM command class.
    //  INVARIANT       :
    //  NOTES           :
    //<<***************************************************************************
{
private:
    static bool isRequest(DIMSE_CMD_ENUM);
    DIMSE_CMD_ENUM	commandIdM;
    bool			isRequestM;	// request = true & response = false
public:
    DCM_COMMAND_CLASS();
    DCM_COMMAND_CLASS(DIMSE_CMD_ENUM);
    DCM_COMMAND_CLASS(DIMSE_CMD_ENUM, string);
    ~DCM_COMMAND_CLASS();
    DIMSE_CMD_ENUM getCommandId();
    void setCommandId(DIMSE_CMD_ENUM commandId);
    bool getIsRequest();
    void setIsRequest(bool flag);
    bool encode(DATA_TF_CLASS&);
    bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);
	DCM_COMMAND_CLASS *cloneAttributes();
};

#endif /* DCM_COMMAND_H */
