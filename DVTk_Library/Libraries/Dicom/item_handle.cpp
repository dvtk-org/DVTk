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
#include "item_handle.h"
#include "dcm_value_sq.h"


//>>===========================================================================

SEQUENCE_REF_CLASS::SEQUENCE_REF_CLASS(string iodName, string identifier, UINT16 group, UINT16 element, UINT itemNumber)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	iodNameM = iodName;
	identifierM = identifier;
	groupM = group;
	elementM = element;
	itemNumberM = itemNumber;
}

//>>===========================================================================

SEQUENCE_REF_CLASS::SEQUENCE_REF_CLASS(UINT16 group, UINT16 element, UINT itemNumber)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	iodNameM = "";
	identifierM = "";
	groupM = group;
	elementM = element;
	itemNumberM = itemNumber;
}

//>>===========================================================================

SEQUENCE_REF_CLASS::~SEQUENCE_REF_CLASS()

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

ITEM_HANDLE_CLASS::ITEM_HANDLE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ITEM_HANDLE;
	nameM = "";
	identifierM = "";
}

//>>===========================================================================

ITEM_HANDLE_CLASS::~ITEM_HANDLE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	while (sequenceRefM.getSize())
	{
		delete sequenceRefM[0];
		sequenceRefM.removeAt(0);
	}
}

//>>===========================================================================

DCM_ITEM_CLASS *ITEM_HANDLE_CLASS::resolveReference()

//  DESCRIPTION     : Try to resolve the item reference using the Data
//					: Warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_ITEM_CLASS *item_ptr = NULL;

	// check that we have at least one reference
	if (sequenceRefM.getSize() == 0) return NULL;

	// get the reference
	SEQUENCE_REF_CLASS *sqReference_ptr = sequenceRefM[0];
	if (sqReference_ptr == NULL) return NULL;

	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Trying to resolve Item Reference with Identifier: %s - Tag: (%04X,%04X) - Item: [%d]", sqReference_ptr->getIdentifier().c_str(), sqReference_ptr->getGroup(), sqReference_ptr->getElement(), sqReference_ptr->getItemNumber() + 1);
	}

	// get the identifier and search the Data Warehouse
	// - try as Dataset first
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(sqReference_ptr->getIdentifier(), WID_DATASET);
	if (wid_ptr == NULL)
	{
		// - now try as Item
		wid_ptr = WAREHOUSE->retrieve(sqReference_ptr->getIdentifier(), WID_ITEM);
	}
	if (wid_ptr == NULL)
	{
		// - now try as Item Handle
		wid_ptr = WAREHOUSE->retrieve(sqReference_ptr->getIdentifier(), WID_ITEM_HANDLE);
		if (wid_ptr)
		{
			// recurse to get the actual referenced item
			ITEM_HANDLE_CLASS *itemHandle_ptr = static_cast<ITEM_HANDLE_CLASS*>(wid_ptr);
			item_ptr = itemHandle_ptr->resolveReference();
			if (item_ptr == NULL) return NULL;
		}
	}

	if (wid_ptr == NULL)
	{
		// can't find the item to resolve
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't resolve Item Reference with Identifier: %s - Tag: (%04X,%04X) - Item: [%d]", sqReference_ptr->getIdentifier().c_str(), sqReference_ptr->getGroup(), sqReference_ptr->getElement(), sqReference_ptr->getItemNumber() + 1);
		}
		return NULL;
	}

	DCM_ATTRIBUTE_GROUP_CLASS *dicomObject_ptr = NULL;

	// start at the beginning of the reference
	for (UINT i = 0; i < sequenceRefM.getSize(); i++)
	{
		// get the sequence reference
		sqReference_ptr = sequenceRefM[i];

		// on first loop get dicom object from wid
		if (i == 0)
		{
			// check if an item was return from the item handle search
			if (item_ptr)
			{
				// get the item from the item handle
				dicomObject_ptr = static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(item_ptr);
			}
			else
			{
				// get the dataset / item class
				dicomObject_ptr =  static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(wid_ptr);
			}
		}
		else
		{
			// try to get dicom object from last item
			dicomObject_ptr =  static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(item_ptr);
		}
		if (dicomObject_ptr == NULL) return NULL;

		// now try to get the sequence attribute from the dataset/item
		DCM_ATTRIBUTE_CLASS *attribute_ptr = dicomObject_ptr->GetMappedAttribute(sqReference_ptr->getGroup(), sqReference_ptr->getElement());
		if (attribute_ptr == NULL)
		{
			// can't find the tag within the dataset/item
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Can't find SQ Attribute with Tag: (%04X,%04X) (while trying to resolve Item Reference with Identifier: %s)", sqReference_ptr->getGroup(), sqReference_ptr->getElement(), sqReference_ptr->getIdentifier().c_str());
			}
			return NULL;
		}

		// check that the tag is a sequence
		if (attribute_ptr->GetVR() != ATTR_VR_SQ)
		{
			// can't find the tag within the dataset/item
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Attribute with Tag: (%04X,%04X) is not an SQ (while trying to resolve Item Reference with Identifier: %s)", sqReference_ptr->getGroup(), sqReference_ptr->getElement(), sqReference_ptr->getIdentifier().c_str());
			}
			return NULL;
		}
	
		DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));
		if (sqValue_ptr == NULL) return NULL;

		// check that the referenced item is in the sequence
		if ((sqValue_ptr->GetNrItems() == 0) ||
			(sqValue_ptr->GetNrItems() <= sqReference_ptr->getItemNumber()))
		{
			// item index too large for sequence
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Item Number: [%d] - too large for Attribute with Tag: (%04X,%04X) containing only %d items", sqReference_ptr->getItemNumber() + 1, sqReference_ptr->getGroup(), sqReference_ptr->getElement(), sqValue_ptr->GetNrItems());
			}
			return NULL;
		}
	
		// finally get the referenced item
		item_ptr = sqValue_ptr->getItem(sqReference_ptr->getItemNumber());

		// set the item identifier - for logging purposes
		if (item_ptr)
		{
			item_ptr->setIdentifier(identifierM);
		}
	}

	// return resolved item - maybe NULL
	return item_ptr;
}

//>>===========================================================================

bool ITEM_HANDLE_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*)

//  DESCRIPTION     : Log the Item Handle.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// not required for Item Handle
	return true;
}
