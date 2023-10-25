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
//  DESCRIPTION     : This file contains the implementation for the attribute
//                    class. A single instance of the attribute class has all
//                    the necessary knowledge of a single attribute.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "attribute.h"
#include "base_value.h"
#include "value_list.h"
#include <vector>
#include <algorithm>

//>>===========================================================================

ATTRIBUTE_CLASS::ATTRIBUTE_CLASS() : groupM( TAG_UNDEFINED_GROUP ), 
	elementM( TAG_UNDEFINED_ELEMENT ), 
	vr_typeM( ATTR_VR_UN ), 
	attr_typeM( ATTR_TYPE_3 ), 
	presentM ( true )

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

ATTRIBUTE_CLASS::~ATTRIBUTE_CLASS()

//  DESCRIPTION     : Default destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Loop through all value lists in the attribute to delete them.
    // Clean up the list that contained references to all value lists.
    for (UINT i=0 ; i<value_listM.size() ; i++)
    {
        delete (value_listM[i]);
        value_listM[i] = NULL;
    }
    value_listM.clear();
}

//>>===========================================================================

unsigned short ATTRIBUTE_CLASS::GetGroup( void )

//  DESCRIPTION     : Return the group of this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return groupM;
}

//>>===========================================================================

unsigned short ATTRIBUTE_CLASS::GetMappedGroup( void )

//  DESCRIPTION     : Return the mapped group of this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // simply return the Group
    return GetGroup();
}

//>>===========================================================================

unsigned short ATTRIBUTE_CLASS::GetElement( void )

//  DESCRIPTION     : Return the element of this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return elementM;
}

//>>===========================================================================

unsigned short ATTRIBUTE_CLASS::GetMappedElement( void )

//  DESCRIPTION     : Return the mapped element of this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // simply return the Element
    return GetElement();
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::SetGroup( unsigned short group )

//  DESCRIPTION     : Set the group of this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    groupM = group;
    return MSG_OK;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::SetElement( unsigned short element )

//  DESCRIPTION     : Set the element of this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    elementM = (UINT16)element;
    return MSG_OK;
}

//>>===========================================================================

ATTR_TYPE_ENUM ATTRIBUTE_CLASS::GetType( void )

//  DESCRIPTION     : Return the type of the attribute (1, 1c, 2, 2c, 3 or 3c).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return attr_typeM;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::SetType( ATTR_TYPE_ENUM attr_type )

//  DESCRIPTION     : Set the type of the attribute (1, 1c, 2, 2c, 3 or 3c).
//                    The ATTR_TYPE_ENUM is defined in constant.h
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    attr_typeM = attr_type;
    return MSG_OK;
}

//>>===========================================================================

ATTR_VR_ENUM ATTRIBUTE_CLASS::GetVR( void )

//  DESCRIPTION     : Return the VR of the attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return vr_typeM;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::SetVR( ATTR_VR_ENUM vr_type )

//  DESCRIPTION     : Set the VR of the attribute. The ATTR_VR_ENUM is defined
//                    in constant.h
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    vr_typeM = vr_type;
    return MSG_OK;
}

//>>===========================================================================

BASE_VALUE_CLASS* ATTRIBUTE_CLASS::GetValue( int value_index, int list_index )

//  DESCRIPTION     : Return the value of the attribute. The value_index
//                    specifies which value of the value list is requested. The
//                    list_index specifies which value list of the attribute is
//                    requested. Both have a default value of 0 if none
//                    specified.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns NULL if the value_index or list_index are out of
//                    bounds.
//  NOTES           :
//<<===========================================================================
{
    VALUE_LIST_CLASS* value_list;
    assert( list_index >= 0 );
    assert( value_index >= 0 );

	if ( value_listM.size() <= (unsigned int)list_index )
    {
        return NULL;
    }

	value_list = value_listM[list_index];
    return (value_list->GetValue( value_index ));
}

//>>===========================================================================

VALUE_LIST_CLASS* ATTRIBUTE_CLASS::GetValueList( int list_index )

//  DESCRIPTION     : Return the value of the attribute. The value_index
//                    specifies which value of the value list is requested. The
//                    list_index specifies which value list of the attribute is
//                    requested. Both have a default value of 0 if none
//                    specified.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns NULL if the value_index or list_index are out of
//                    bounds.
//  NOTES           :
//<<===========================================================================
{
    assert( list_index >= 0 );

	if ( value_listM.size() <= (unsigned int)list_index )
    {
        return NULL;
    }

	VALUE_LIST_CLASS* ptr = value_listM[list_index];
    return ptr;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::AddValue( BASE_VALUE_CLASS *value, int list_index )

//  DESCRIPTION     : Add a value to the attribute. By default the value will
//                    be added to the first value list. If list_index is
//                    specified and the number of lists is exactly one smaller,
//                    a new value list will be created. If list_index is
//                    smaller, the value will be added to the value list
//                    specified.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_OUT_OF_BOUNDS if list_index larger than the
//                    number of value lists in the attribute.
//  NOTES           :
//<<===========================================================================
{
    VALUE_LIST_CLASS* value_list;
    assert( list_index >= 0 );

	if ( value_listM.size() <= (unsigned int)list_index )
    {
        if ( value_listM.size() == (UINT)list_index )
        {
            value_list = new VALUE_LIST_CLASS;
            value_listM.push_back( value_list );
        }
        else
        {
            return MSG_OUT_OF_BOUNDS;
        }
    }

	value_list = value_listM[list_index];
    DVT_STATUS status = value_list->AddValue( value );

	return status;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::Replace(BASE_VALUE_CLASS* value,
								int value_index,
								int list_index )

//  DESCRIPTION     : Replaces the value at the given index specified in
//                    value_index and list_index with the value passed in
//                    value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_OUT_OF_BOUNDS if any of the index arguments
//                    is too large.
//  NOTES           :
//<<===========================================================================
{
    VALUE_LIST_CLASS* value_list;
    assert( list_index >= 0 );

	if ( value_listM.size()-1 < (unsigned int)list_index )
    {
        return MSG_OUT_OF_BOUNDS;
    }

	value_list = value_listM[list_index];
    DVT_STATUS status = value_list->Replace( value, value_index );

	return status;
}

//>>===========================================================================

int ATTRIBUTE_CLASS::GetNrLists( void )

//  DESCRIPTION     : Returns the number of value lists in the attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    size_t size = value_listM.size();
    return (int)size;
}

//>>===========================================================================

int ATTRIBUTE_CLASS::GetNrValues( int list_index )

//  DESCRIPTION     : Returns the number of values in the specified value list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : If list_index is larger than the number of value lists,
//                    the function will return 0.
//  NOTES           :
//<<===========================================================================
{
    VALUE_LIST_CLASS* value_list;
    assert( list_index >= 0 );

	if ( value_listM.size() <= (unsigned int)list_index )
    {
        return 0;
    }

	value_list = value_listM.at( list_index );
    int nrValues = value_list->GetNrValues();

	return nrValues;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::SetValueType(ATTR_VAL_TYPE_ENUM val_type,
					 int list_index )

//  DESCRIPTION     : Set the type of a value list (Defined or Enumerated).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns MSG_OUT_OF_BOUNDS if the list_index is greater
//                    than the number of value lists in the attribute.
//  NOTES           : When created, ATTR_VAL_TYPE_NOVALUE is the default value.
//<<===========================================================================
{
    VALUE_LIST_CLASS* value_list;
    assert( list_index >= 0 );
    if ( value_listM.size()-1 < (unsigned int)list_index )
    {
        return MSG_OUT_OF_BOUNDS;
    }

    value_list = value_listM[list_index];
    value_list->SetValueType( val_type );

    return MSG_OK;
}

//>>===========================================================================

ATTR_VAL_TYPE_ENUM ATTRIBUTE_CLASS::GetValueType( int list_index )

//  DESCRIPTION     : Get the type of a value list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : Returns ATTR_VAL_TYPE_NOVALUE when list_index is greater
//                    than the number of value lists in the attribute.
//  NOTES           :
//<<===========================================================================
{
    VALUE_LIST_CLASS* value_list;
    assert( list_index >= 0 );

	if ( value_listM.size()-1 < (unsigned int)list_index )
    {
        return ATTR_VAL_TYPE_NOVALUE;
    }

	value_list = value_listM[list_index];
    ATTR_VAL_TYPE_ENUM valueType = value_list->GetValueType();

	return valueType;
}

//>>===========================================================================

void ATTRIBUTE_CLASS::SetPresent(bool flag)

//  DESCRIPTION     : Set the value of the present flag to true or false.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	presentM = flag; 
}

//>>===========================================================================

bool ATTRIBUTE_CLASS::IsPresent()

//  DESCRIPTION     : Return the value of the present flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{ 
	return presentM;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::Check (UINT32 flags,
                        ATTRIBUTE_CLASS *,
                        LOG_MESSAGE_CLASS * messages,
                        SPECIFIC_CHARACTER_SET_CLASS *specific_character_set)

//  DESCRIPTION     : Check the values of the first value list in the attribute
//                    only. Returns MSG_OK if no problems are found. Any other
//                    return value means something is not ok.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    VALUE_LIST_CLASS* value_list;
    DVT_STATUS item_rval = MSG_OK;
    DVT_STATUS rval;

    // Sanity check. If no values available, return.
    if (value_listM.size() == 0) return MSG_OK;

    // We only check the first value list of each attribute.
    value_list = value_listM[0];

    // Loop through all values of that value list.
    for ( int index=0; index < value_list->GetNrValues(); index++ )
    {
        // TODO: pass along the correct reference value, if applicable.
        rval = value_list->GetValue( index )->Check( flags, NULL, messages, specific_character_set );
        if ( (rval != MSG_OK) && 
			(rval != MSG_EQUAL) )
        {
            item_rval = MSG_ERROR;
        }
    }

    return MSG_OK;
}

//>>===========================================================================

bool ATTRIBUTE_CLASS::operator < ( ATTRIBUTE_CLASS& attribute )

//  DESCRIPTION     : Returns true if the left-hand value is smaller than the
//                    right-hand value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (this->groupM < attribute.GetGroup())
    {
        return true;
    }

	if ((this->groupM == attribute.GetGroup()) &&
        (this->elementM < attribute.GetElement()))
    {
        return true;
    }

    return false;
}

//>>===========================================================================

bool ATTRIBUTE_CLASS::operator > ( ATTRIBUTE_CLASS& attribute )

//  DESCRIPTION     : Returns true if the left-hand value is greater than the
//                    right-hand value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (this->groupM > attribute.GetGroup())
    {
        return true;
    }

    if ((this->groupM == attribute.GetGroup()) &&
        (this->elementM > attribute.GetElement()))
    {
        return true;
    }

    return false;
}

//>>===========================================================================

bool ATTRIBUTE_CLASS::operator == ( ATTRIBUTE_CLASS& attribute )

//  DESCRIPTION     : Returns true if the left-hand value is equal to the
//                    right-hand value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if ((this->groupM == attribute.GetGroup()) &&
        (this->elementM == attribute.GetElement()))
    {
        return true;
    }

    return false;
}

//>>===========================================================================

bool ATTRIBUTE_CLASS::operator != ( ATTRIBUTE_CLASS& attribute )

//  DESCRIPTION     : Returns true if the left-hand operator is not equal to
//                    the right-hand value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if ((groupM != attribute.GetGroup()) ||
        (elementM != attribute.GetElement()))
    {
        return true;
    }

    return false;
}

//>>===========================================================================

bool ATTRIBUTE_CLASS::operator = ( ATTRIBUTE_CLASS& attribute )

//  DESCRIPTION     : Assigns the contents of the right-hand value to the
//                    left-hand value by allocating each variable again.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    groupM = attribute.GetGroup();
    elementM = attribute.GetElement();
    attr_typeM = attribute.GetType();
    vr_typeM = attribute.GetVR();

	// Delete all values in all value lists in this attribute.
    for (UINT i=0 ; i<value_listM.size() ; i++)
	{
        delete (value_listM[i]);
	}
    value_listM.clear();

    // Copy all values in all value lists to this attribute.
    assert( attribute.GetNrLists() >= 0 );

    for (int index=0; index < attribute.GetNrLists(); index++)
    {
        VALUE_LIST_CLASS *value_list_ptr = new VALUE_LIST_CLASS;
        value_list_ptr->SetValueType( attribute.GetValueType( index ) );

        for ( int val_index=0; val_index < attribute.GetNrValues( index ); val_index++ )
        {
            BASE_VALUE_CLASS *value_ptr = CreateNewValue( vr_typeM );
            BASE_VALUE_CLASS *src_value_ptr = attribute.GetValue( val_index, index );
            *value_ptr = *src_value_ptr;
            value_list_ptr->AddValue( value_ptr );
        }

        value_listM.push_back( value_list_ptr );
    }

    return true;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::Compare(LOG_MESSAGE_CLASS* messages, 
                                    ATTRIBUTE_CLASS* attribute )

//  DESCRIPTION     : Compares the given attribute content with the content of
//                    this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    char message[256];

    // Check if the attribute tags are the same.
    if (( groupM != attribute->GetGroup()) ||
        (elementM != attribute->GetElement()))
    {
        sprintf (message,
            "Attribute (%04X,%04X) tag is not the same as attribute (%04X,%04X) in reference object",
            groupM,
            elementM,
            attribute->GetGroup(),
            attribute->GetElement()
            );
        if (messages) messages->AddMessage(VAL_RULE_DEF_DEFINITION_6, message);

        return MSG_ERROR;
    }

    // Check if the VR of the attributes are the same.
    if ( vr_typeM != attribute->GetVR() )
    {
        sprintf (message,
            "The VR of attribute (%04X,%04X) is not the same as in reference object",
            groupM,
            elementM
            );
        if (messages) messages->AddMessage(VAL_RULE_DEF_DEFINITION_6, message);

        return MSG_INCOMPATIBLE;
    }

    // For now we don't check if the number of value lists are the same.

    // Check if the number of values in the first list are the same.
    if ( GetNrValues() != attribute->GetNrValues() )
    {
        sprintf (message,
            "The number of values (%d) in the source attribute (%04X,%04X) are not the same as in the reference attribute (%d)",
            GetNrValues(),
            groupM,
            elementM,
            attribute->GetNrValues()
            );
        if (messages) messages->AddMessage(VAL_RULE_DEF_DEFINITION_6, message);

        return MSG_NOT_SAME_LEN;
    }

    DVT_STATUS attr_rval = MSG_OK;
    for ( int index=0; index < GetNrValues(); index++ )
    {
        DVT_STATUS rval;
        BASE_VALUE_CLASS* src_value;
        src_value = GetValue( index );
        rval = src_value->Compare(messages, attribute->GetValue( index ) );
        switch ( rval )
        {
        case MSG_ERROR:
        sprintf (message,
                "An undefined error has occurred with attribute (%04X,%04X) and the reference attribute (%04X,%04X)",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_SMALLER:
        sprintf (message,
                "The content of attribute (%04X,%04X) is smaller than attribute (%04X,%04X)",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_GREATER:
        sprintf (message,
                "The content of attribute (%04X,%04X) is greater than attribute (%04X,%04X)",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_NOT_IN_LIST:
        sprintf (message,
                "An attribute is present in item (%04X,%04X) or in item (%04X,%04X) but not both",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_NOT_SET:
        sprintf (message,
                "Either attribute (%04X,%04X) or attribute (%04X,%04X) is not set",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_INCOMPATIBLE:
            // This shouldn't happen because the vr has already been
            // for compatibility.
            break;
        case MSG_OUT_OF_BOUNDS:
        sprintf (message,
                "Either attribute (%04X,%04X) or attribute (%04X,%04X) is out of bounds",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_MEMORY_ERROR:
        sprintf (message,
                "Either attribute (%04X,%04X) or attribute (%04X,%04X) could not allocate enough memory for the operation",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_INVALID_PTR:
         sprintf (message,
                "Either attribute (%04X,%04X) or attribute (%04X,%04X) is pointing to invalid memory",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_NOT_SAME_LEN:
        sprintf (message,
                "The length of attribute (%04X,%04X) and attribute (%04X,%04X) is not equal",
                groupM,
                elementM,
                attribute->GetGroup(),
                attribute->GetElement()
                );
            break;
        case MSG_EQUAL:     // Fall through.
        case MSG_OK:
            // No problem, so no logging.
            break;
        default:
            // We shouln't get here.
            break;
        }

        if ( (rval != MSG_EQUAL) && (rval != MSG_OK) )
        {
            if (messages) messages->AddMessage(VAL_RULE_DEF_DEFINITION_6, message);
            attr_rval = MSG_ERROR;
        }
    }

    return attr_rval;
}

//>>===========================================================================

DVT_STATUS ATTRIBUTE_CLASS::CompareVRAndValues(ATTRIBUTE_CLASS * attribute)

//  DESCRIPTION     : Compares the given attribute VR and values with this attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Check if the VR of the attributes are the same.
    if ( vr_typeM != attribute->GetVR() )
    {
        return MSG_INCOMPATIBLE;
    }

    // For now we don't check if the number of value lists are the same.

    // Check if the number of values in the first list are the same.
    if ( GetNrValues() != attribute->GetNrValues() )
    {
        return MSG_NOT_SAME_LEN;
    }

    DVT_STATUS attr_rval = MSG_OK;
    LOG_MESSAGE_CLASS* messages = new LOG_MESSAGE_CLASS();
    for ( int index=0; index < GetNrValues(); index++ )
    {
        BASE_VALUE_CLASS* src_value = GetValue(index);
        attr_rval = src_value->Compare(messages, attribute->GetValue(index));
        if (attr_rval != MSG_EQUAL) break;
    }

    // messages are not used for anything here
    delete messages;

    return attr_rval;
}
