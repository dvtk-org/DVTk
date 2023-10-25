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
//  FILENAME        :	DCM_DIR_DATASET.CPP
//  PACKAGE         :	DVT
//  COMPONENT       :	DICOM
//  DESCRIPTION     :	DICOMDIR Dataset Class.
//  COPYRIGHT(c)    :   2004, Philips Electronics N.V.
//                      2004, Agfa Gevaert N.V.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_dir_dataset.h"

//>>===========================================================================

DCM_DIR_DATASET_CLASS::DCM_DIR_DATASET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_DATASET;
	commandIdM = DIMSE_CMD_UNKNOWN;
	iodNameM ="";
	pahM_ptr = &pahM;
}

//>>===========================================================================

DCM_DIR_DATASET_CLASS::~DCM_DIR_DATASET_CLASS()

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

bool DCM_DIR_DATASET_CLASS::decode(DATA_TF_CLASS &dataTransfer, UINT16 lastGroup)

//  DESCRIPTION     : decode the dataset attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// install the Private Attribute Handler
	pahM.setLogger(loggerM_ptr);
	pahM.install();

	// decode the dataset attributes
	bool result = DCM_ATTRIBUTE_GROUP_CLASS::decode(dataTransfer, lastGroup);

////

	bool result = true;
	UINT32 length = 0;

	// decode all attributes
	while (dataTransfer.isData())
	{
		// check if anything left to decode
		if ((length_ptr) &&
			(*length_ptr == length))
		{
			// break from loop
			break;
		}

		DCM_ATTRIBUTE_CLASS *attribute_ptr = new DCM_ATTRIBUTE_CLASS();
		if (attribute_ptr == NULL) return false;

		UINT32 attributeLength = 0;

		// cascade the logger
		attribute_ptr->setLogger(loggerM_ptr);

		// cascade the parent
		attribute_ptr->setParent(this);

		// cascade the private attribute handler
		attribute_ptr->setPAH(pahM_ptr);

		// decode attributes one at a time
		if (attribute_ptr->decode(dataTransfer, lastGroup, &attributeLength))
		{
			// add attribute length to total
			length += attributeLength;

			// add attribute to object
			addAttribute(attribute_ptr);
		}
		else
		{
			// check if we have reached the last group
			if (attribute_ptr->GetGroup() < lastGroup)
			{
				// return error when reason for stopping is not the last group reached
				result = false;
			}

			// clean up
			delete attribute_ptr;
			break;
		} 
	}

	// return actual object length
	if (length_ptr)	*length_ptr = length;

	// return result
	return result;

////

	// remove the private attribute handler
	pahM.remove();

	// return the decode result
	return result;
}
