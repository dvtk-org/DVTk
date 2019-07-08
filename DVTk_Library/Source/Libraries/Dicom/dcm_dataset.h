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

#ifndef DCM_DATASET_H
#define DCM_DATASET_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_attribute_group.h"
#include "private_attribute.h"	


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_COMMAND_CLASS;


//>>***************************************************************************

class DCM_DATASET_CLASS : public DCM_ATTRIBUTE_GROUP_CLASS

//  DESCRIPTION     : DICOM dataset class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	DIMSE_CMD_ENUM	commandIdM;
	string			iodNameM;
	PRIVATE_ATTRIBUTE_HANDLER_CLASS	pahM;

	void populateWithAttributes();

public:
	DCM_DATASET_CLASS();
	DCM_DATASET_CLASS(DIMSE_CMD_ENUM, string);
	DCM_DATASET_CLASS(DIMSE_CMD_ENUM, string, string);

	~DCM_DATASET_CLASS();

	void setCommandId(DIMSE_CMD_ENUM commandId)
		{ commandIdM = commandId; }

	void setIodName(char *iodName_ptr)
	{ 
		if (iodName_ptr)
		{
			iodNameM = iodName_ptr; 
		}
	}

	DIMSE_CMD_ENUM getCommandId()
		{ return commandIdM; }

	const char* getIodName()
	{ 
		const char* iodName_ptr = NULL;

		if (iodNameM.length())
		{
			iodName_ptr = iodNameM.c_str(); 
		}
		return iodName_ptr;
	}

	void removeTrailingPadding();

	bool setIdentifierByTag(UINT32);

	void computeItemOffsets(DATA_TF_CLASS&);

	UINT32 computeItemOffsetsForDICOMDIR(string);

	bool encode(DATA_TF_CLASS&);

	bool decode(DATA_TF_CLASS&, UINT16 lastGroup = 0xFFFF);

	void completeCommandOnSend(DIMSE_CMD_ENUM, DCM_COMMAND_CLASS*, UINT16);

	void completeDatasetOnSend(DIMSE_CMD_ENUM, string, DCM_DATASET_CLASS*);

	void completeSQOnSend();

    void initializePrivateAttributeHandler();

    void terminatePrivateAttributeHandler();

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);

	DCM_DATASET_CLASS *cloneAttributes();
};

#endif /* DCM_DATASET_H */

