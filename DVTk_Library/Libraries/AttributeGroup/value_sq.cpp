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

#include "value_sq.h"
#include "attribute_group.h"
#include "attribute.h"
#include "value_list.h"

//>>===========================================================================

VALUE_SQ_CLASS::VALUE_SQ_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

VALUE_SQ_CLASS::~VALUE_SQ_CLASS()

//  DESCRIPTION     : Default destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// Clean up all the items
	DeleteItems();
}

//>>===========================================================================

bool VALUE_SQ_CLASS::operator = (BASE_VALUE_CLASS &value)

//  DESCRIPTION     : Equal operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool result = false;

    if (value.GetVRType() == ATTR_VR_SQ)
    {
        for (int item_index = 0; item_index < value.GetNrItems(); item_index++)
        {
            ATTRIBUTE_GROUP_CLASS *item;
            value.Get(&item, item_index);

            ATTRIBUTE_GROUP_CLASS *new_item = new ATTRIBUTE_GROUP_CLASS();

            for (int index = 0; index < item->GetNrAttributes(); index++)
            {
                ATTRIBUTE_CLASS *new_attribute = new ATTRIBUTE_CLASS();
                ATTRIBUTE_CLASS *attribute = item->GetAttribute(index);

                // copy the attribute contents
                *new_attribute = *attribute;

                // add the new attribute to the item
                new_item->AddAttribute(new_attribute);
            }

            // add the new item to the sequence
            itemsM.push_back(new_item);
        }

        result = true;
    }

    return result;
}

//>>===========================================================================

DVT_STATUS VALUE_SQ_CLASS::Get (ATTRIBUTE_GROUP_CLASS **item, int index)

//  DESCRIPTION     : Get indexed item.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	assert (index >= 0);
    *item = NULL;

    if ((unsigned int) index >= itemsM.size())
    {
        return (MSG_NOT_SET);
    }

    *item = itemsM[index];

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_SQ_CLASS::Set(ATTRIBUTE_GROUP_CLASS *item)

//  DESCRIPTION     : Add item to sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    itemsM.push_back(item);

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_SQ_CLASS::Compare (LOG_MESSAGE_CLASS*, BASE_VALUE_CLASS * ref_value)

//  DESCRIPTION     : Compare values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if ((ref_value != NULL) &&
        (ref_value->GetVRType() != ATTR_VR_SQ))
    {
        return MSG_INCOMPATIBLE;
	}
    else
	{
        return MSG_OK;
	}
}

//>>===========================================================================

DVT_STATUS VALUE_SQ_CLASS::Check (UINT32 flags,
                                  BASE_VALUE_CLASS **,
                                  LOG_MESSAGE_CLASS *messages,
                                  SPECIFIC_CHARACTER_SET_CLASS *specific_character_set)

//  DESCRIPTION     : Check SQ format.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DVT_STATUS     item_rval = MSG_OK;
    DVT_STATUS     rval;

    for (UINT index=0 ; index<itemsM.size() ; index++)
    {
        // TODO: Pass along the correct reference value, if applicable.
        rval = itemsM[index]->Check (flags, NULL, messages, specific_character_set);
        if ((rval != MSG_OK) && 
			(rval != MSG_EQUAL))
        {
            item_rval = MSG_ERROR;
        }
    }

    return (item_rval);
}

//>>===========================================================================

ATTRIBUTE_CLASS *VALUE_SQ_CLASS::GetAttribute(unsigned short  group, unsigned short element)

//  DESCRIPTION     : Try to find the given tag in the items of the sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	ATTRIBUTE_CLASS *attribute_ptr = NULL;

    for (UINT index=0 ; index<itemsM.size() ; index++)
    {
		attribute_ptr = itemsM[index]->GetAttribute(group, element);
		if (attribute_ptr != NULL) break;
	}

	return attribute_ptr;
}

//>>===========================================================================

ATTRIBUTE_CLASS *VALUE_SQ_CLASS::GetMappedAttribute(unsigned short  group, unsigned short element)

//  DESCRIPTION     : Try to find the given mapped tag in the items of the sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	ATTRIBUTE_CLASS *attribute_ptr = NULL;

    for (UINT index=0 ; index<itemsM.size() ; index++)
    {
		attribute_ptr = itemsM[index]->GetMappedAttribute(group, element);
		if (attribute_ptr != NULL) break;
	}

	return attribute_ptr;
}

//>>===========================================================================

bool VALUE_SQ_CLASS::IsSorted(void)

//  DESCRIPTION     : Check if attributes in items are sorted.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool sorted = true;
    for (UINT index=0 ; index<itemsM.size() ; index++)
    {
        if (!itemsM[index]->IsSorted())
        {
            sorted = false;
            break;
        }
    }

    return sorted;
}

//>>===========================================================================

DVT_STATUS VALUE_SQ_CLASS::SortAttributes(void)

//  DESCRIPTION     : Sort attributes in items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (UINT index=0 ; index<itemsM.size() ; index++)
    {
        itemsM[index]->SortAttributes();
    }

    return MSG_OK;
}

//>>===========================================================================

ATTR_VR_ENUM VALUE_SQ_CLASS::GetVRType (void)

//  DESCRIPTION     : Get VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (ATTR_VR_SQ);
}

//>>===========================================================================

int VALUE_SQ_CLASS::GetNrItems (void)

//  DESCRIPTION     : Get number of items in sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return ( itemsM.size () );
}

//>>===========================================================================

DVT_STATUS VALUE_SQ_CLASS::DeleteItemReferences(void)

//  DESCRIPTION     : Delete all item references in sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // delete all the item references without freeing up the items
    itemsM.clear();

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS VALUE_SQ_CLASS::DeleteItems(void)

//  DESCRIPTION     : Delete all items in sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // delete all the items
    for (UINT index=0 ; index<itemsM.size() ; index++)
    {
        delete (itemsM[index]);
        itemsM[index] = NULL;
    }
    itemsM.clear();

    return (MSG_OK);
}

//>>===========================================================================

void VALUE_SQ_CLASS::SetLength(UINT32 length)

//  DESCRIPTION     : Set length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    lengthM = length;
}


//>>===========================================================================

UINT32 VALUE_SQ_CLASS::GetLength (void)

//  DESCRIPTION     : Get length.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return lengthM;
}
