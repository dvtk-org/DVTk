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
#include "dcm_value_sq.h"


//>>===========================================================================

DCM_VALUE_SQ_CLASS::DCM_VALUE_SQ_CLASS(UINT32 length)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetLength(length);
	nestingDepthM = 0;
	definedLengthM = (length == UNDEFINED_LENGTH) ? false : true;
	delimiterGroupM = ITEM_GROUP;
	delimiterElementM = SQ_DELIMITER;
	delimiterLengthM = 0;
	parentM_ptr = NULL;
	pahM_ptr = NULL;
	loggerM_ptr = NULL;
}

//>>===========================================================================

DCM_VALUE_SQ_CLASS::~DCM_VALUE_SQ_CLASS()

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

void DCM_VALUE_SQ_CLASS::addItem(DCM_ITEM_CLASS *item_ptr)

//  DESCRIPTION     : Add an item to the sequence value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// save parent
	item_ptr->setParent(this);

	// add the item using the base class
	VALUE_SQ_CLASS::Set(item_ptr);
}

//>>===========================================================================

DCM_ITEM_CLASS* DCM_VALUE_SQ_CLASS::getItem(UINT i) 

//  DESCRIPTION     : Return the address of the item with the given index.
//					: If the item is stored by reference, the reference is
//					: used to return the referenced item directly via a search in
//					: the data warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// get item
	ATTRIBUTE_GROUP_CLASS *ag_ptr;
	if (Get(&ag_ptr, i) != MSG_OK) return NULL;

	DCM_ITEM_CLASS *item_ptr = static_cast<DCM_ITEM_CLASS*>(ag_ptr);
	if (!item_ptr) return NULL;
	
	// check if item known by reference or value
	if (item_ptr->getValueByReference())
	{
		// by reference - need to look up the item in the data warehouse
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(item_ptr->getIdentifier(), WID_ITEM);
		if (!wid_ptr) 
		{
			// can't find reference - we will log this as an error elsewhere
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Can't encode Item: %s - not found in Data Warehouse", item_ptr->getIdentifier());
			}

			return NULL;
		}

		// replace reference by actual item
		item_ptr = static_cast<DCM_ITEM_CLASS*>(wid_ptr);
	}

	// return item address
	return item_ptr;
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::setNestingDepth(int nestingDepth)

//  DESCRIPTION     : Set the nesting depth for all items in the sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// save the nesting depth
	nestingDepthM = nestingDepth;

	// loop through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;
		
		// set the nesting dept - one deeper
		item_ptr->setNestingDepth(nestingDepth + 1);
	}
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::setTransferVR(TRANSFER_ATTR_VR_ENUM transferVr)

//  DESCRIPTION     : Method to set the transfer VR for all items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// run through all items
	for (int i = 0; i < GetNrItems(); i++) 
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		// set the transfer VR
		item_ptr->setTransferVR(transferVr);
	}
}

//>>===========================================================================

void  DCM_VALUE_SQ_CLASS::setDefineGroupLengths(bool flag)

//  DESCRIPTION     : Method to set the group lengths flag for all items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// run through all items
	for (int i = 0; i < GetNrItems(); i++) 
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		// set the transfer VR
		item_ptr->setDefineGroupLengths(flag);
	}
}

//>>===========================================================================

void  DCM_VALUE_SQ_CLASS::setDefinedLength(bool flag)

//  DESCRIPTION     : Method to set the defined length flag for all items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// set defined length
	definedLengthM = flag;

	// run through all items
	for (int i = 0; i < GetNrItems(); i++) 
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		// set the transfer VR
		item_ptr->setDefinedLength(flag);
	}
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::setGroupLengths(TS_CODE tsCode)

//  DESCRIPTION     : Set the group length attribute values for items in the sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;
		
		// set the item attribute group lengths
		item_ptr->setGroupLengths(tsCode);
	}
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::addGroupLengths()

//  DESCRIPTION     : Add the group length attribute values for the items in the sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		// add the item attribute group lengths
		item_ptr->addGroupLengths();
	}
}

//>>===========================================================================

UINT32 DCM_VALUE_SQ_CLASS::computeLength(TS_CODE tsCode)

//  DESCRIPTION     : Method to compute the length of the SQ value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This method maybe called recursively.
//<<===========================================================================
{
	// initialise the length
	UINT32 length = 0;

	// loop through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		item_ptr->setDefinedLength(definedLengthM);
		
		// compute the item lengths
		length += item_ptr->computeLength(tsCode);
	}

	// count the length of the SQ Delimitier
	if (!definedLengthM) 
	{
		length += (sizeof(delimiterGroupM) + sizeof(delimiterElementM) + sizeof(delimiterLengthM));
	}

	// store the computed length
	SetLength(length);

	// return computed length
	return length;
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::computeItemOffsets(DATA_TF_CLASS &dataTransfer, UINT32 *offset_ptr)

//  DESCRIPTION     : Compute any item offsets.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32 offset = *offset_ptr;

	// loop through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		// check if this item offset should be registered
		// - it must be identified
		if (item_ptr->getIdentifier())
		{
			// save the item offset in the data transfer class
			dataTransfer.addItemOffset(item_ptr->getIdentifier(), offset);
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Setting Item Offset for  \"%s\" to 0x%08X=%d", item_ptr->getIdentifier(), offset, offset);
			}
		}

		// compute the offsets within the item
		item_ptr->setOffset(offset);
		item_ptr->computeItemOffsets(dataTransfer);
		offset = item_ptr->getOffset();
	}

	// check if the sequence should be delimited
	if (!definedLengthM)
	{
		// include delimiter
		offset += (sizeof(delimiterGroupM) + sizeof(delimiterElementM) + sizeof(delimiterLengthM));
	}

	// return updated offset
	*offset_ptr = offset;
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::computeItemOffsets(string tsStr, UINT32 *offset_ptr)

//  DESCRIPTION     : Compute any item offsets.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32 offset = *offset_ptr;

	// loop through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		// compute the offsets within the item
		item_ptr->setOffset(offset);
		item_ptr->computeItemOffsets(tsStr);
		offset = item_ptr->getOffset();
	}

	// check if the sequence should be delimited
	if (!definedLengthM)
	{
		// include delimiter
		offset += (sizeof(delimiterGroupM) + sizeof(delimiterElementM) + sizeof(delimiterLengthM));
	}

	// return updated offset
	*offset_ptr = offset;
}


//>>===========================================================================

bool DCM_VALUE_SQ_CLASS::encode(DATA_TF_CLASS &dataTransfer)

//  DESCRIPTION     : Encode the items within the sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// loop through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get next item
		DCM_ITEM_CLASS *item_ptr = getItem(i);
		if (!item_ptr) continue;

		// encode the items attributes
		result = item_ptr->encode(dataTransfer);
		if (!result) break;
	}

	// check if the sequence should be delimited
	if (!definedLengthM)
	{
		if (loggerM_ptr)
		{
		    loggerM_ptr->text(LOG_DEBUG, 1, "Encode - SQ Delimiter (%04X,%04X), length is %08X", delimiterGroupM, delimiterElementM, delimiterLengthM);
		}

		// encode delimiter
		dataTransfer << delimiterGroupM;
		dataTransfer << delimiterElementM;
		dataTransfer << delimiterLengthM;
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_VALUE_SQ_CLASS::decode(DATA_TF_CLASS &dataTransfer)

//  DESCRIPTION     : Decode the items within the sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	UINT32 length = 0;
    bool mixedEncodingErrorReported = false;

	// decode all attributes
	while ((dataTransfer.isData())
		&& (length != GetLength()))
	{
		DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();
		if (item_ptr == NULL) return false;
		UINT32 itemLength = 0;

		// cascade the logger
		item_ptr->setLogger(loggerM_ptr);

		// cascade the private attribute handler
		item_ptr->setPAH(pahM_ptr);

		// decode items one at a time
		if (item_ptr->decode(dataTransfer, ITEM_GROUP, LAST_ELEMENT, &itemLength))
		{
            // check for mixture of definded SQ length but undefined item length
            if ((definedLengthM) &&
                (item_ptr->isDefinedLength() == false))
            {
                UINT16 group = TAG_UNDEFINED_GROUP;
                UINT16 element = TAG_UNDEFINED_ELEMENT;
                if (parentM_ptr)
                {
                    group = parentM_ptr->GetGroup();
                    element = parentM_ptr->GetElement();
                }

                // report error only once per sequence
                if ((mixedEncodingErrorReported == false) &&
                    (loggerM_ptr))
                {
                    loggerM_ptr->text(LOG_ERROR, 1, "Attribute (%04X,%04X) - mixed defined SQ length with undefined Item length - not recommended in DICOM", group, element);
                }
                mixedEncodingErrorReported = true;
            }

			// add item length to sequence
			length += itemLength;

			// add item to sequence object
			addItem(item_ptr);
		}
		else
		{
			// check if we have reached the SQ delimiter
			if (item_ptr->getIntroducerElement() != SQ_DELIMITER)
			{
				// return error when reason for stopping is not the SQ delimiter reached
				result = false;
			}

			// clean up
			delete item_ptr;
			break;
		} 
	}

	// check if the sequence should be delimited
	if ((result) &&
		(!definedLengthM))
	{
		// decode delimiter
		dataTransfer >> delimiterGroupM;
		length += sizeof(delimiterGroupM);

		dataTransfer >> delimiterElementM;
		length += sizeof(delimiterElementM);

		dataTransfer >> delimiterLengthM;
		length += sizeof(delimiterLengthM);

		if (loggerM_ptr)
		{
		    loggerM_ptr->text(LOG_DEBUG, 1, "Decode - SQ Delimiter (%04X,%04X), length is %08X", delimiterGroupM, delimiterElementM, delimiterLengthM);
		}

		if (delimiterGroupM != ITEM_GROUP)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "SQ Delimiter Group. Should be: %04X, is: %04X", ITEM_GROUP, delimiterGroupM);
			}
		}
		if (delimiterElementM != SQ_DELIMITER)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "SQ Delimiter Element. Should be: %04X, is: %04X", SQ_DELIMITER, delimiterElementM);
			}
		}
		if (delimiterLengthM != 0)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "SQ Delimiter Length. Should be: %08X, is: %08X", 0, delimiterLengthM);
			}
		}
	}

	// set the sequence length to be the actual length decoded
	SetLength(length);

	// return result
	return result;
}

//>>===========================================================================

DCM_ITEM_CLASS* DCM_VALUE_SQ_CLASS::decodeNextItem(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : Decode the next Item in the Data Transfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    DCM_ITEM_CLASS *item_ptr = NULL;

	// check if some data remaining in the Data Transfer stream
	if (dataTransfer.isData())
	{
		item_ptr = new DCM_ITEM_CLASS();
		if (item_ptr == NULL) return NULL;

		// cascade the logger
		item_ptr->setLogger(loggerM_ptr);

		// cascade the private attribute handler
		item_ptr->setPAH(pahM_ptr);

		// decode items one at a time
        UINT32 itemLength;
		if (!item_ptr->decode(dataTransfer, ITEM_GROUP, LAST_ELEMENT, &itemLength))
		{
			// check if we have reached the SQ delimiter
			if (item_ptr->getIntroducerElement() == SQ_DELIMITER)
			{
        		// decode delimiter
		        dataTransfer >> delimiterGroupM;
        		dataTransfer >> delimiterElementM;
		        dataTransfer >> delimiterLengthM;
			}

			// clean up - end of sequence only - not an item
			delete item_ptr;
            item_ptr = NULL;
		} 
	}

	// return item_ptr
	return item_ptr;
}

//>>===========================================================================

bool DCM_VALUE_SQ_CLASS::operator == (BASE_VALUE_CLASS *baseValue_ptr)

//  DESCRIPTION     : Equality operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_VALUE_SQ_CLASS *that_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(baseValue_ptr);
	bool equal = false;

	// number of items must be the same
	if (GetNrItems() == that_ptr->GetNrItems())
	{
		// set result true - could be zero-length items
		equal = true;

		// check items - the order is assumed to be the same in both sequences
		for (int i = 0; i < GetNrItems(); i++)
		{
			// get individual items
			ATTRIBUTE_GROUP_CLASS *ag_ptr;

			Get(&ag_ptr, i);
			DCM_ITEM_CLASS *thisItem_ptr = static_cast<DCM_ITEM_CLASS*>(ag_ptr);

			that_ptr->Get(&ag_ptr, i);
			DCM_ITEM_CLASS *thatItem_ptr = static_cast<DCM_ITEM_CLASS*>(ag_ptr);

			// check each item
			if (*thisItem_ptr != *thatItem_ptr)
			{
				// items not equal
				equal = false;
				break;
			}
		}
	}

	// return equality result
	return equal;
}

//>>===========================================================================
	
bool DCM_VALUE_SQ_CLASS::operator != (BASE_VALUE_CLASS *baseValue_ptr)

//  DESCRIPTION     : Non-equality operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return not equal result
	return (*this == baseValue_ptr) ? false : true;
}

//>>===========================================================================

bool DCM_VALUE_SQ_CLASS::operator = (DCM_VALUE_SQ_CLASS& source)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // copy all members
    nestingDepthM = source.nestingDepthM;
    definedLengthM = source.definedLengthM;
    delimiterGroupM = source.delimiterGroupM;
    delimiterElementM = source.delimiterElementM;
    delimiterLengthM = source.delimiterLengthM;

	// copy all items
    for (int i = 0; i < source.GetNrItems(); i++)
    {
		// get individual items
		ATTRIBUTE_GROUP_CLASS *ag_ptr;

	    source.Get(&ag_ptr, i);
		DCM_ITEM_CLASS *sourceItem_ptr = static_cast<DCM_ITEM_CLASS*>(ag_ptr);
        if (sourceItem_ptr)
        {
    		DCM_ITEM_CLASS *destItem_ptr = new DCM_ITEM_CLASS();

            // copy the item contents
            *destItem_ptr = *sourceItem_ptr;

            // add item to this object
		    addItem(destItem_ptr);
        }
	}

	// return equal result
	return true;
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Cascade logger through items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// store logger locally
	loggerM_ptr = logger_ptr;

	// cascade logger through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get item
		ATTRIBUTE_GROUP_CLASS *ag_ptr;
		if (Get(&ag_ptr, i) != MSG_OK) return;

		DCM_ITEM_CLASS *item_ptr = static_cast<DCM_ITEM_CLASS*>(ag_ptr);
		if (!item_ptr) continue;

		// store the logger
		item_ptr->setLogger(logger_ptr);
	}
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::setEnsureEvenAttributeValueLength(bool flag)

//  DESCRIPTION     : Cascade EnsureEvenAttributeValueLength flag through items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// cascade logger through all items
	for (int i = 0; i < GetNrItems(); i++)
	{
		// get item
		ATTRIBUTE_GROUP_CLASS *ag_ptr;
		if (Get(&ag_ptr, i) != MSG_OK) return;

		DCM_ITEM_CLASS *item_ptr = static_cast<DCM_ITEM_CLASS*>(ag_ptr);
		if (!item_ptr) continue;

		// store the EnsureEvenAttributeValueLength flag
		item_ptr->setEnsureEvenAttributeValueLength(flag);
	}
}

//>>===========================================================================

void DCM_VALUE_SQ_CLASS::merge(DCM_ATTRIBUTE_CLASS *mergeAttribute_ptr)

//  DESCRIPTION     : Merge the given sequence into this.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The merge attributes can overwrite "this" attributes
//<<===========================================================================
{
	// sanity check on parameters
	if (mergeAttribute_ptr == NULL) return;
	
	// get SQ value from merge attribute
	DCM_VALUE_SQ_CLASS *mergeSqValue_ptr = (DCM_VALUE_SQ_CLASS*) mergeAttribute_ptr->GetValue(0);
	
	if (mergeSqValue_ptr == NULL) return;

	// check if the merge sequence is empty - if so remove all items in this
	if (mergeSqValue_ptr->GetNrItems() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Source SQ is empty - will make Destination SQ empty too - no Items.");
		}
		DeleteItems();
		return;
	}

	// check if items are present in destination
	if (GetNrItems() == 0)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Destination SQ is empty - adding items from merge");
		}

		// copy items from merge sequence to destination
		for (int i = 0; i < mergeSqValue_ptr->GetNrItems(); i++)
		{
			DCM_ITEM_CLASS *item_ptr = mergeSqValue_ptr->getItem(i);

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Adding item %d into destination item", i + 1);
			}

			// add merge item to destination
			addItem(item_ptr);
		}
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Destination SQ is not empty");
		}

		// merge destination item with source item
		for (int i = 0; i < GetNrItems(); i++)
		{
			DCM_ITEM_CLASS *item_ptr = getItem(i);

			// check if source item exists for merge
			if (i < mergeSqValue_ptr->GetNrItems())
			{
				DCM_ITEM_CLASS *mergeItem_ptr = mergeSqValue_ptr->getItem(i);

				// merge source to destination
				if (mergeItem_ptr->GetNrAttributes() == 0)
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Source Item is empty - will make Destination Item %d empty too - no Attributes.", i + 1);
					}
					item_ptr->clone(mergeItem_ptr);
					delete mergeItem_ptr;
				}
				else
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Merging Source Item %d into Destination Item", i + 1);
					}
					item_ptr->merge(mergeItem_ptr);
				}

			}
		}

		// check if more items are in source than in destination
		for (int j = GetNrItems(); j < mergeSqValue_ptr->GetNrItems(); j++)
		{
			// copy remaining items from source to destination
			DCM_ITEM_CLASS *item_ptr = mergeSqValue_ptr->getItem(j);
			
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Adding item %d into destination item", j + 1);
			}

			// add merge item to destination
			addItem(item_ptr);
		}
	}

	// remove item references from merge sequence
	mergeSqValue_ptr->DeleteItemReferences();
}
