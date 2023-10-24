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
//  DESCRIPTION     : This file contains the implementation for the
//                    attribute_group class. A single instance of the class has
//                    all the necessary knowledge of a single attribute group.
//*****************************************************************************


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <algorithm>

#include "attribute_group.h"
#include "attribute.h"
#include "value_sq.h"

bool Smaller (ATTRIBUTE_CLASS * attr1, ATTRIBUTE_CLASS * attr2);

//>>===========================================================================

ATTRIBUTE_GROUP_CLASS::ATTRIBUTE_GROUP_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    attributesM.clear();
    nameM = "";
}

//>>===========================================================================

ATTRIBUTE_GROUP_CLASS::~ATTRIBUTE_GROUP_CLASS()

//  DESCRIPTION     : Default destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	DeleteAttributes();
}

//>>===========================================================================

void ATTRIBUTE_GROUP_CLASS::DeleteAttributes()

//  DESCRIPTION     : Delete all attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    UINT    size = attributesM.size();

    if (size > 0)
    {
        // Loop through all value lists in the attribute group to delete them.
        for (UINT index=0 ; index<size-1 ; index++)
        {
            delete (attributesM[index]);
            attributesM[index] = NULL;
        }

        // Clean up the list that contained references to all value lists.
        attributesM.clear();
    }
}

//>>===========================================================================

string ATTRIBUTE_GROUP_CLASS::GetName()

//  DESCRIPTION     : Return the name of the attribute group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (nameM);
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::SetName(string name)

//  DESCRIPTION     : Set the name of the attribute group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : No check is done on the validity of the argument name.
//<<===========================================================================
{
    nameM = name;

    return (MSG_OK);
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::AddAttribute(ATTRIBUTE_CLASS *attribute)

//  DESCRIPTION     : Add an attribute to the attribute group. Returns MSG_OK
//                    if successful.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : If the attribute is a NULL pointer, MSG_INVALID_PTR will
//                    be returned.
//  NOTES           :
//<<===========================================================================
{
    if (attribute == NULL)
    {
        return (MSG_INVALID_PTR);
    }

    attributesM.push_back(attribute);

    return (MSG_OK);
}

//>>===========================================================================

ATTRIBUTE_CLASS * ATTRIBUTE_GROUP_CLASS::GetAttribute(int index)

//  DESCRIPTION     : Return the attribute at index specified by the argument
//                    index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns NULL if index is larger than the number of
//                    attributes in the attribute group.
//  NOTES           :
//<<===========================================================================
{
	assert (index >= 0);

    if (attributesM.size() <= (unsigned int) index)
    {
        return (NULL);
    }

    return (attributesM[index]);
}

//>>===========================================================================

ATTRIBUTE_CLASS * ATTRIBUTE_GROUP_CLASS::GetAttribute(unsigned short group,
                                                      unsigned short element,
													  bool parentOnly)

//  DESCRIPTION     : Return the attribute specified by group and element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns NULL if the requested attribute is not in the
//                    attribute group.
//  NOTES           :
//<<===========================================================================
{
    for (UINT index=0; index < attributesM.size() ; index++)
    {
		ATTRIBUTE_CLASS *attribute_ptr = attributesM[index];
        if ((attribute_ptr->GetGroup() == group) &&
            (attribute_ptr->GetElement() == element))
        {
            return attribute_ptr;
        }
		else if ((attribute_ptr->GetVR() == ATTR_VR_SQ) &&
			(parentOnly == false))
		{
            // Search in the sequence for a match
            for (int j = 0; j < attribute_ptr->GetNrValues(); j++)
            {
                VALUE_SQ_CLASS *valueSq_ptr = static_cast<VALUE_SQ_CLASS*>(attribute_ptr->GetValue(j));
                if (valueSq_ptr)
                {
                    // Try to find the attribute inside the first item of the sequence
                    ATTRIBUTE_CLASS *lAttribute_ptr = valueSq_ptr->GetAttribute(group, element);
					if (lAttribute_ptr) return lAttribute_ptr;
                }
            }
		}
    }

    return NULL;
}

//>>===========================================================================

ATTRIBUTE_CLASS * ATTRIBUTE_GROUP_CLASS::GetMappedAttribute(unsigned short group,
                                                      unsigned short element,
													  bool parentOnly)

//  DESCRIPTION     : Return the mapped attribute specified by group and element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns NULL if the requested attribute is not in the
//                    attribute group.
//  NOTES           : parentOnly set true - means only search at this level -
//					: do not recurse into sequences.
//<<===========================================================================
{
    for (UINT index=0; index < attributesM.size() ; index++)
    {
		ATTRIBUTE_CLASS *attribute_ptr = attributesM[index];
        if ((attribute_ptr->GetMappedGroup() == group) &&
            (attribute_ptr->GetMappedElement() == element))
        {
            return attribute_ptr;
        }
		else if ((attribute_ptr->GetVR() == ATTR_VR_SQ) &&
			(parentOnly == false))
		{
            // Search in the sequence for a match
            for (int j = 0; j < attribute_ptr->GetNrValues(); j++)
            {
                VALUE_SQ_CLASS *valueSq_ptr = static_cast<VALUE_SQ_CLASS*>(attribute_ptr->GetValue(j));
                if (valueSq_ptr)
                {
                    // Try to find the attribute inside the first item of the sequence
                    ATTRIBUTE_CLASS *lAttribute_ptr = valueSq_ptr->GetMappedAttribute(group, element);
					if (lAttribute_ptr) return lAttribute_ptr;
                }
            }
		}
    }

    return NULL;
}

//>>===========================================================================

ATTRIBUTE_CLASS * ATTRIBUTE_GROUP_CLASS::GetAttributeByTag(unsigned int tag)

//  DESCRIPTION     : Return the attribute by the specified tag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns NULL if the requested attribute is not in the
//                    attribute group.
//  NOTES           :
//<<===========================================================================
{
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

    return (GetAttribute (group, element));
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::DeleteAttribute(int index)

//  DESCRIPTION     : Delete the attribute at the specified index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_OUT_OF_BOUNDS if the index is larger than the
//                    number of attributes in the attribute group.
//  NOTES           :
//<<===========================================================================
{
	assert (index >= 0);

    if (attributesM.size() <= (unsigned int) index)
    {
        return (MSG_OUT_OF_BOUNDS);
    }

    delete (attributesM[index]);
    attributesM.erase(attributesM.begin() + index - 1);

    return (MSG_OK);

}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::DeleteAttribute(unsigned short group,
                                               unsigned short element)

//  DESCRIPTION     : Delete the attribute specified by group and element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_NOT_IN_LIST if the specified attribute is not
//                    in the attribute group.
//  NOTES           :
//<<===========================================================================
{
    ATTRIBUTE_VECTOR::iterator    attr_it;

    attr_it = attributesM.begin();

    while (attr_it != attributesM.end())
    {
        if (((*attr_it)->GetGroup() == group) &&
            ((*attr_it)->GetElement() == element))
        {
            delete (*attr_it);
            attributesM.erase(attr_it);
            return (MSG_OK);
        }

        // move to next attribute
        attr_it++;
    }

    return (MSG_NOT_IN_LIST);
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::DeleteMappedAttribute(unsigned short group,
                                               unsigned short element)

//  DESCRIPTION     : Delete the mapped attribute specified by group and element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_NOT_IN_LIST if the specified attribute is not
//                    in the attribute group.
//  NOTES           :
//<<===========================================================================
{
    ATTRIBUTE_VECTOR::iterator    attr_it;

    attr_it = attributesM.begin();

    while (attr_it != attributesM.end())
    {
        if (((*attr_it)->GetMappedGroup() == group) &&
            ((*attr_it)->GetMappedElement() == element))
        {
            delete (*attr_it);
            attributesM.erase(attr_it);
            return (MSG_OK);
        }

        // move to next attribute
        attr_it++;
    }

    return (MSG_NOT_IN_LIST);
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::DeleteAttributeIndex(unsigned short group,
                                                    unsigned short element)

//  DESCRIPTION     : Removes the attribute specified by group and element from
//                    the attribute group. The attribute itself is not deleted.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_NOT_IN_LIST if the specified attribute is not
//                    in the attribute group.
//  NOTES           :
//<<===========================================================================
{
    ATTRIBUTE_VECTOR::iterator    attr_it;

    attr_it = attributesM.begin();

    while (attr_it != attributesM.end())
    {
        if (((*attr_it)->GetGroup() == group) &&
            ((*attr_it)->GetElement() == element))
        {
            attributesM.erase(attr_it);
            return (MSG_OK);
        }

        // move to next attribute
        attr_it++;
    }

    return (MSG_NOT_IN_LIST);
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::DeleteMappedAttributeIndex(unsigned short group,
                                                    unsigned short element)

//  DESCRIPTION     : Removes the mapped attribute specified by group and element from
//                    the attribute group. The attribute itself is not deleted.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_NOT_IN_LIST if the specified attribute is not
//                    in the attribute group.
//  NOTES           :
//<<===========================================================================
{
    ATTRIBUTE_VECTOR::iterator    attr_it;

    attr_it = attributesM.begin();

    while (attr_it != attributesM.end())
    {
        if (((*attr_it)->GetMappedGroup() == group) &&
            ((*attr_it)->GetMappedElement() == element))
        {
            attributesM.erase(attr_it);
            return (MSG_OK);
        }

        // move to next attribute
        attr_it++;
    }

    return (MSG_NOT_IN_LIST);
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::DeleteAttributeIndex(int index)

//  DESCRIPTION     : Delete the attribute at the specified index from the
//                    attribute group. The attribute itself is not deleted.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_OUT_OF_BOUNDS if the index is larger than the
//                    number of attributes in the attribute group.
//  NOTES           :
//<<===========================================================================
{
    if ((index < 0) ||
        ((unsigned int)index >= attributesM.size()))
    {
        return (MSG_OUT_OF_BOUNDS);
    }

    attributesM.erase(attributesM.begin() + index);

    return (MSG_OK);
}

//>>===========================================================================

int ATTRIBUTE_GROUP_CLASS::GetNrAttributes()

//  DESCRIPTION     : Returns the number of attributes in the attribute group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (attributesM.size());
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::SortAttributes(void)

//  DESCRIPTION     : Sorts the attributes in the attribute group in an
//                    ascending order.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // sort attributes at this level
    sort(attributesM.begin(), attributesM.end(), Smaller);

    // check if any of the attributes are sequences - need to sort the items too
    for (UINT i = 0; i < attributesM.size(); i++)
    {
        ATTRIBUTE_CLASS *attribute_ptr = attributesM[i];
        if (attribute_ptr == NULL) continue;

        if (attribute_ptr->GetVR() == ATTR_VR_SQ)
        {
            for (int j = 0; j < attribute_ptr->GetNrValues(); j++)
            {
                VALUE_SQ_CLASS *valueSq_ptr = static_cast<VALUE_SQ_CLASS*>(attribute_ptr->GetValue(j));
                if (valueSq_ptr)
                {
                    // sort the items
                    (void) valueSq_ptr->SortAttributes();
                }
            }
        }
    }

    return MSG_OK;
}

//>>===========================================================================

bool ATTRIBUTE_GROUP_CLASS::IsSorted(void)

//  DESCRIPTION     : Returns true when the attributes in the attribute group
//                    are sorted in an ascending order. Otherwise fals is
//                    returned.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool   sorted = true;
    int    index = 0;
    int    nr_attributes = attributesM.size();

    // check attributes at this level
    while ((index<nr_attributes-1) && 
		(sorted==true))
    {
        if (*(attributesM[index]) > *(attributesM[index+1]))
        {
            sorted = false;
        }
        index++;
    }

    // now check if there are any sequences
    for (UINT i = 0; i < attributesM.size() && sorted; i++)
    {
        ATTRIBUTE_CLASS *attribute_ptr = attributesM[i];
        if (attribute_ptr == NULL) continue;

        if (attribute_ptr->GetVR() == ATTR_VR_SQ)
        {
            for (int j = 0; j < attribute_ptr->GetNrValues() && sorted; j++)
            {
                VALUE_SQ_CLASS *valueSq_ptr = static_cast<VALUE_SQ_CLASS*>(attribute_ptr->GetValue(j));
                if (valueSq_ptr)
                {
                    // check if items are sorted
                    sorted = valueSq_ptr->IsSorted();
                }
            }
        }
    }

    return sorted;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_GROUP_CLASS::Check (UINT32 flags,
                                         ATTRIBUTE_GROUP_CLASS *,
                                         LOG_MESSAGE_CLASS *messages,
                                         SPECIFIC_CHARACTER_SET_CLASS *specific_character_set)

//  DESCRIPTION     : Validate the entire attribute group. All attributes will
//                    be checked for validity. Returns MSG_OK if everything is
//                    ok, otherwise MSG_ERROR will be returned.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DVT_STATUS     ag_rval = MSG_OK;
    DVT_STATUS     rval;

    for (int index=0 ; index<GetNrAttributes() ; index++)
    {
        // TODO: pass along the correct reference value, if applicable.
        rval = attributesM[index]->Check (flags, NULL, messages, specific_character_set);
        if ((rval != MSG_OK) && (rval != MSG_EQUAL))
        {
            ag_rval = MSG_ERROR;
        }
    }
    return (ag_rval);
}

//>>===========================================================================

bool Smaller (ATTRIBUTE_CLASS * attr1, ATTRIBUTE_CLASS * attr2)

//  DESCRIPTION     : This is a helper function needed for the sort function in
//                    SortAttributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return (*attr1 < *attr2);
}
