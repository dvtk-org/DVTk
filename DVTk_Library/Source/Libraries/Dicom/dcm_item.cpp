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
#include "dcm_item.h"
#include "dcm_dataset.h"


//>>===========================================================================

DCM_ITEM_CLASS::DCM_ITEM_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_ITEM;
	valueByReferenceM = false;
	definedLengthM = false;
	lengthM = 0; // From PR 970 - this represents the actual length of the item.
	introducerGroupM = ITEM_GROUP;
	introducerElementM = ITEM_ELEMENT;
	introducerLengthM = UNDEFINED_LENGTH; // From PR 970 - constant value should not be changed anywhere in the code.
	delimiterGroupM = ITEM_GROUP;
	delimiterElementM = ITEM_DELIMITER;
	delimiterLengthM = 0;
	offsetM = 0;
}

//>>===========================================================================

DCM_ITEM_CLASS::~DCM_ITEM_CLASS()

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

UINT32 DCM_ITEM_CLASS::computeLength(TS_CODE tsCode)

//  DESCRIPTION     : Method to compute the length of the item.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This method maybe called recursively.
//<<===========================================================================
{
	// count the length of the Item Introducer
	lengthM = 0;

	// compute the length of the item attributes
	lengthM += DCM_ATTRIBUTE_GROUP_CLASS::computeLength(tsCode);

	// count the length of the Item Delimitier
	if (!definedLengthM) 
	{
		lengthM += (sizeof(delimiterGroupM) + sizeof(delimiterElementM) + sizeof(delimiterLengthM));
	}

	// return computed length
	return lengthM + (sizeof(introducerGroupM) + sizeof(introducerElementM) + sizeof(introducerLengthM));
}

//>>===========================================================================

void DCM_ITEM_CLASS::computeItemOffsets(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : Compute any item offsets - for DICOMDIR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// count the length of the Item Introducer
	offsetM += (sizeof(introducerGroupM) + sizeof(introducerElementM) + sizeof(introducerLengthM));

	// call base method
	DCM_ATTRIBUTE_GROUP_CLASS::computeItemOffsets(dataTransfer);

	// count the length of the Item Delimitier
	if (!definedLengthM) 
	{
		offsetM += (sizeof(delimiterGroupM) + sizeof(delimiterElementM) + sizeof(delimiterLengthM));
	}
}

//>>===========================================================================

void DCM_ITEM_CLASS::computeItemOffsets(string tsStr)

//  DESCRIPTION     : Compute any item offsets - for DICOMDIR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// count the length of the Item Introducer
	offsetM += (sizeof(introducerGroupM) + sizeof(introducerElementM) + sizeof(introducerLengthM));

	// call base method
	DCM_ATTRIBUTE_GROUP_CLASS::computeItemOffsets(tsStr);

	// count the length of the Item Delimitier
	if (!definedLengthM) 
	{
		offsetM += (sizeof(delimiterGroupM) + sizeof(delimiterElementM) + sizeof(delimiterLengthM));
	}
}

//>>===========================================================================

bool DCM_ITEM_CLASS::encode(DATA_TF_CLASS &dataTransfer)

//  DESCRIPTION     : encode the item attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    UINT32 itemLength = 0;

	// set up the defined length field
	//definedLengthM = (lengthM == UNDEFINED_LENGTH) ? false : true;

	// encode item introducer
	dataTransfer << introducerGroupM;
	dataTransfer << introducerElementM;
	if (definedLengthM)
	{
		// use the actual item length
		itemLength = lengthM;
	}
	else
	{
		// use the introducer item length
		itemLength = introducerLengthM;
	}
    dataTransfer << itemLength;

    if (loggerM_ptr)
	{
	    loggerM_ptr->text(LOG_DEBUG, 1, "Encode - Item Introducer (%04X,%04X), length is %08X", introducerGroupM, introducerElementM, itemLength);
	}

	// encode the item attributes
	bool result = DCM_ATTRIBUTE_GROUP_CLASS::encode(dataTransfer);

	// check if item should be delimited
	if (!definedLengthM)
	{
		if (loggerM_ptr)
		{
		    loggerM_ptr->text(LOG_DEBUG, 1, "Encode - Item Delimiter (%04X,%04X), length is %08X", delimiterGroupM, delimiterElementM, delimiterLengthM);
		}

		// encode the delimiter
		dataTransfer << delimiterGroupM;
		dataTransfer <<	delimiterElementM;
		dataTransfer << delimiterLengthM;
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ITEM_CLASS::decode(DATA_TF_CLASS &dataTransfer, UINT16 lastGroup, UINT16 lastElement, UINT32 *length_ptr)

//  DESCRIPTION     : decode the item attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	//set offset for this item;
	//will be used for media purposes
	offsetM = dataTransfer.getLength() - dataTransfer.getRemainingLength();

	// decode item introducer
	dataTransfer >> introducerGroupM;
	if (length_ptr) *length_ptr += sizeof(introducerGroupM);

	dataTransfer >> introducerElementM;
	if (length_ptr) *length_ptr += sizeof(introducerElementM);

	// check if we have reached the sequence delimiter
	if (introducerElementM == SQ_DELIMITER)
	{
		// rewind the dataTransfer by the length of data read
		dataTransfer.rewind(sizeof(introducerGroupM) + sizeof(introducerElementM));
		return false;
	}

	// decode the received length into the actual item length
    dataTransfer >> lengthM;
	if (length_ptr) *length_ptr += sizeof(lengthM);

	// check if we have reached the dataset trailing padding
	if ((introducerGroupM == DATASET_TRAILING_PADDING_GROUP) &&
		(introducerElementM == DATASET_TRAILING_PADDING_ELEMENT))
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Decode - Dataset Trailing Padding (%04X,%04X), length is %08X", introducerGroupM, introducerElementM, lengthM);
		}

		// rewind the dataTransfer by the length of data read
		dataTransfer.rewind(sizeof(introducerGroupM) + sizeof(introducerElementM) + sizeof(lengthM));

		// just return - don't do anything with dataset trailing padding
		return false;
	}

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Decode - Item Introducer (%04X,%04X), length is %08X", introducerGroupM, introducerElementM, lengthM);
	}

	if (introducerGroupM != ITEM_GROUP)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Item Introducer Group. Should be: %04X, is: %04X", ITEM_GROUP, introducerGroupM);
		}
	}
	if (introducerElementM != ITEM_ELEMENT)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Item Introducer Element. Should be: %04X, is: %04X", ITEM_ELEMENT, introducerElementM);
		}
	}

	// set up the defined length field - based on the value of the length received - which maybe UNDEFINED or the actual item length
	definedLengthM = (lengthM == UNDEFINED_LENGTH) ? false : true;
	UINT32 itemLength = lengthM;

	// push a new recognition code table for private attribute handling in the item
	if (pahM_ptr)
	{
		pahM_ptr->pushRecognitionCodeTable();
	}

	// decode the item attributes
	bool result = DCM_ATTRIBUTE_GROUP_CLASS::decode(dataTransfer, lastGroup, lastElement, &itemLength);

	// update the message length
	if (length_ptr) *length_ptr += itemLength;

	// now pop the new recognition code table
	if (pahM_ptr)
	{
		pahM_ptr->popRecognitionCodeTable();
	}

	// check if item should be delimited
	if ((result) &&
		(!definedLengthM))
	{
		// decode the delimiter
		dataTransfer >> delimiterGroupM;
		if (length_ptr) *length_ptr += sizeof(delimiterGroupM);
	
		dataTransfer >>	delimiterElementM;
		if (length_ptr) *length_ptr += sizeof(delimiterElementM);

		dataTransfer >> delimiterLengthM;
		if (length_ptr) *length_ptr += sizeof(delimiterLengthM);

		if (loggerM_ptr)
		{
		    loggerM_ptr->text(LOG_DEBUG, 1, "Decode - Item Delimiter (%04X,%04X), length is %08X", delimiterGroupM, delimiterElementM, delimiterLengthM);
		}

		if (delimiterGroupM != ITEM_GROUP)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Item Delimiter Group. Should be: %04X, is: %04X", ITEM_GROUP, delimiterGroupM);
			}
		}
		if (delimiterElementM != ITEM_DELIMITER)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Item Delimiter Element. Should be: %04X, is: %04X", ITEM_DELIMITER, delimiterElementM);
			}
		}
		if (delimiterLengthM != 0)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Item Delimiter Length. Should be: %08X, is: %08X", 0, delimiterLengthM);
			}
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ITEM_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this item with the contents of the item given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// ensure update WID is an item
	if (wid_ptr->getWidType() == widTypeM)
	{
		DCM_ITEM_CLASS *updateItem_ptr = static_cast<DCM_ITEM_CLASS*>(wid_ptr);

		// we can perform the update by merging the update item into this
		merge(updateItem_ptr);

		// result is OK
		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ITEM_CLASS::morph(DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Morph the dataset into this.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is needed to convert a dataset into an item when
//					  parsing the DICOMScripts. We can't tell the difference between
//					  the two until late in the parsing - so always use a dataset and
//					  morph it into an item when necessary.
//<<===========================================================================
{
	// copy identifier
	setIdentifier(dataset_ptr->getIdentifier());

	// merge the dataset attributes into this
	merge(dataset_ptr);

	// copy logger
	setLogger(dataset_ptr->getLogger());

	// copy other flags
	setPopulateWithAttributes(dataset_ptr->getPopulateWithAttributes());

	// return result
	return true;
}

//>>===========================================================================

bool DCM_ITEM_CLASS::operator = (DCM_ITEM_CLASS& sourceItem)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // copy all members
    valueByReferenceM = sourceItem.valueByReferenceM;
    definedLengthM = sourceItem.definedLengthM;
    lengthM = sourceItem.lengthM;
    introducerGroupM = sourceItem.introducerGroupM;
    introducerElementM = sourceItem.introducerElementM;
    delimiterGroupM = sourceItem.delimiterGroupM;
    delimiterElementM = sourceItem.delimiterElementM;
    delimiterLengthM = sourceItem.delimiterLengthM;

	for (int i = 0; i < sourceItem.GetNrAttributes(); i++)
	{
		// get individual attributes
		DCM_ATTRIBUTE_CLASS *sourceAttribute_ptr = sourceItem.GetAttribute(i);
        if (sourceAttribute_ptr)
        {
            DCM_ATTRIBUTE_CLASS *destAttribute_ptr = new DCM_ATTRIBUTE_CLASS(sourceAttribute_ptr->GetGroup(), sourceAttribute_ptr->GetElement());
            *destAttribute_ptr = *sourceAttribute_ptr;

            // add Attribute to item
			addAttribute(destAttribute_ptr);
        }
    }

    return true;
}

//>>===========================================================================

void DCM_ITEM_CLASS::clone(DCM_ITEM_CLASS *copyItem)

//  DESCRIPTION     : Clone this item.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// Delete all existing attributes
	DeleteAttributes();

    // Copy all members
    valueByReferenceM = copyItem->valueByReferenceM;
    definedLengthM = copyItem->definedLengthM;
    lengthM = copyItem->lengthM;
    introducerGroupM = copyItem->introducerGroupM;
    introducerElementM = copyItem->introducerElementM;
    delimiterGroupM = copyItem->delimiterGroupM;
    delimiterElementM = copyItem->delimiterElementM;
    delimiterLengthM = copyItem->delimiterLengthM;

	for (int i = 0; i < copyItem->GetNrAttributes(); i++)
	{
		// get individual attributes
		DCM_ATTRIBUTE_CLASS *sourceAttribute_ptr = copyItem->GetAttribute(i);
        if (sourceAttribute_ptr)
        {
            DCM_ATTRIBUTE_CLASS *destAttribute_ptr = new DCM_ATTRIBUTE_CLASS(sourceAttribute_ptr->GetGroup(), sourceAttribute_ptr->GetElement());
            *destAttribute_ptr = *sourceAttribute_ptr;

            // add Attribute to item
			addAttribute(destAttribute_ptr);
        }
    }
}
