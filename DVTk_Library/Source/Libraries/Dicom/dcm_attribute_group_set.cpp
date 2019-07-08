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
//  DESCRIPTION     :	DICOM Attribute Group Class - set/get methods.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_attribute_group.h"
#include "dcm_value_sq.h"

#include "Idefinition.h"		// Definition component interface


//>>===========================================================================

DCM_ATTRIBUTE_CLASS* DCM_ATTRIBUTE_GROUP_CLASS::setAttribute(UINT32 tag, ATTR_VR_ENUM vr)

//  DESCRIPTION     : Ensure that the attribute is present in the group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Return NULL if the given VR does not match the attribute VR.
//<<===========================================================================
{
	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) 
	{
		// if not present
		// - instantiate a new attribute
		attribute_ptr = new DCM_ATTRIBUTE_CLASS(tag, vr);
		attribute_ptr->SetType(ATTR_TYPE_1);

		// set EnsureEvenAttributeValueLength
		attribute_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

		// now add the attribute to the DICOM object
		addAttribute(attribute_ptr);
	}

	// check that the attribute VR matches that given
	if (attribute_ptr->GetVR() != vr)
	{
		attribute_ptr = NULL;
	}

	// return attribute
	return attribute_ptr;
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setValue(UINT32 tag, BYTE *data_ptr)

//  DESCRIPTION     : Set tagged attribute value in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 group = ((UINT16) (tag >> 16));
	UINT16 element = ((UINT16) (tag & 0x0000FFFF));

	ATTR_VR_ENUM vr = DEFINITION->GetAttributeVr(group, element);

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, vr);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(vr);
	value_ptr->Set(data_ptr, byteStrLen(data_ptr));
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::addATValue(UINT32 tag, UINT32 data)

//  DESCRIPTION     : Add tagged AT values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_AT);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_AT);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setAEValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged CS values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_AE);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_AE);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setCSValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged CS values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_CS);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_CS);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setSHValue(UINT32 tag, string data, bool replaceFirst)

//  DESCRIPTION     : Set tagged SH values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_SH);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_SH);
	value_ptr->Set(data);

	// check if we should replace the first value or just add one more
	if (attribute_ptr->GetNrValues() == 1)
	{
		if (replaceFirst)
		{
			// replace first value
			attribute_ptr->replaceValue(0, value_ptr);
		}
	}
	else
	{
		// add value
		attribute_ptr->AddValue(value_ptr);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setDAValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged DA values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_DA);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_DA);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setISValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged IS value in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_IS);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_IS);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setLOValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged LO values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_LO);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_LO);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setPNValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged PN values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_PN);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_PN);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setSQValue(UINT32 tag, DCM_VALUE_SQ_CLASS* sqValue)

//  DESCRIPTION     : Set tagged SQ value in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_SQ);
	if (attribute_ptr == NULL) return;

	// Create new SQ value
	DCM_VALUE_SQ_CLASS* newSeqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);

	newSeqValue_ptr->setDefinedLength(true);

	for (int item_index = 0; item_index < sqValue->GetNrItems(); item_index++)
    {
		// Get the item
		DCM_ITEM_CLASS *item_ptr = sqValue->getItem(item_index);
		newSeqValue_ptr->addItem(item_ptr);
	}
	
	attribute_ptr->addSqValue(newSeqValue_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setOBValue(UINT32 tag, UINT rows, UINT columns)

//  DESCRIPTION     : Set tagged OB values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_OB);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_OB);
	value_ptr->Set(rows);
	value_ptr->Add(columns);

	// check if we should replace the first value or just add one more
	if (attribute_ptr->GetNrValues() == 1)
	{
		// replace first value
		attribute_ptr->replaceValue(0, value_ptr);
	}
	else
	{
		attribute_ptr->AddValue(value_ptr);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setOBValue(UINT32 tag, UINT rows, UINT columns, BYTE value)

//  DESCRIPTION     : Set tagged OB values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_OB);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_OB);

	// set the pattern values
	value_ptr->Set(rows);
	value_ptr->Add(columns);
	value_ptr->Add(value);
	value_ptr->Add(rows);
	value_ptr->Add(0);
	value_ptr->Add(columns);
	value_ptr->Add(0);

	// check if we should replace the first value or just add one more
	if (attribute_ptr->GetNrValues() == 1)
	{
		// replace first value
		attribute_ptr->replaceValue(0, value_ptr);
	}
	else
	{
		attribute_ptr->AddValue(value_ptr);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setTMValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged TM values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_TM);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_TM);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setSTValue(UINT32 tag, string data)

//  DESCRIPTION     : Set tagged ST values in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_ST);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_ST);
	value_ptr->Set(data);
	attribute_ptr->AddValue(value_ptr);
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setUIValue(UINT32 tag, string data, bool replaceFirst)

//  DESCRIPTION     : Set tagged UI value in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_UI);
	if (attribute_ptr == NULL) return;
	
	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_UI);
	value_ptr->Set(data);

	// check if we should replace the first value or just add one more
	if (attribute_ptr->GetNrValues() >= 1)
	{
		if (replaceFirst)
		{
			// replace first value
			attribute_ptr->replaceValue(0, value_ptr);
		}
	}
	else
	{
		// add value
		attribute_ptr->AddValue(value_ptr);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setULValue(UINT32 tag, UINT32 value, bool replaceFirst)

//  DESCRIPTION     : Set tagged US value in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_UL);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_UL);
	value_ptr->Set(value);

	// check if we should replace the first value or just add one more
	if (attribute_ptr->GetNrValues() >= 1)
	{
		if (replaceFirst)
		{
			// replace first value
			attribute_ptr->replaceValue(0, value_ptr);
		}
	}
	else
	{
		// add value
		attribute_ptr->AddValue(value_ptr);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setUSValue(UINT32 tag, UINT16 value, bool replaceFirst)

//  DESCRIPTION     : Set tagged US value in object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = setAttribute(tag, ATTR_VR_US);
	if (attribute_ptr == NULL) return;

	// add the value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_US);
	value_ptr->Set(value);

	// check if we should replace the first value or just add one more
	if (attribute_ptr->GetNrValues() >= 1)
	{
		if (replaceFirst)
		{
			// replace first value
			attribute_ptr->replaceValue(0, value_ptr);
		}
	}
	else
	{
		// add value
		attribute_ptr->AddValue(value_ptr);
	}
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getATValue(UINT32 tag, int index, UINT32 *data_ptr)

//  DESCRIPTION     : Get tagged indexed AT value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_AT) && 
		(index < attribute_ptr->GetNrValues())) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(index);

		// have we really got a value ?
		if (value_ptr)
		{
			UINT32 value;
			value_ptr->Get(value);

			// ok - return attribute value
			*data_ptr = value;
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getCSValue(UINT32 tag, BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Get tagged CS value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_CS) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			if (value_ptr->GetLength() < length) length = (UINT)(value_ptr->GetLength());
			BYTE *l_data_ptr;
			value_ptr->Get(&l_data_ptr, length);
			byteCopy(data_ptr, l_data_ptr, length);
			data_ptr[length] = 0x00;
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getCSValue(UINT32 tag, string& data, int index)

//  DESCRIPTION     : Get tagged CS value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_CS) && 
		(index < attribute_ptr->GetNrValues())) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(index);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			value_ptr->Get(data);
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getSHValue(UINT32 tag, string& data, int index)

//  DESCRIPTION     : Get tagged SH value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_SH) && 
		(index < attribute_ptr->GetNrValues())) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(index);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			value_ptr->Get(data);
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getDAValue(UINT32 tag, BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Get tagged DA value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_DA) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			if (value_ptr->GetLength() < length) length = (UINT)(value_ptr->GetLength());
			BYTE *l_data_ptr;
			value_ptr->Get(&l_data_ptr, length);
			byteCopy(data_ptr, l_data_ptr, length);
			data_ptr[length] = 0x00;
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getISValue(UINT32 tag, INT32 *data_ptr)

//  DESCRIPTION     : Get tagged IS value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_IS) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			INT32 value;
			value_ptr->Get(value);

			// ok - return attribute value
			*data_ptr = value;
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getSTValue(UINT32 tag, BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Get tagged ST value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_ST) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			if (value_ptr->GetLength() < length) length = (UINT)(value_ptr->GetLength());
			BYTE *l_data_ptr;
			value_ptr->Get(&l_data_ptr, length);
			byteCopy(data_ptr, l_data_ptr, length);
			data_ptr[length] = 0x00;
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getSQValue(UINT32 tag, DCM_VALUE_SQ_CLASS** sq_value_ptr_ptr)

//  DESCRIPTION     : Get tagged SQ value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttributeByTag(tag);
	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_SQ) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		DCM_VALUE_SQ_CLASS* sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

		// have we really got a value ?
		if (sqValue_ptr)
		{
			*sq_value_ptr_ptr = sqValue_ptr;
			result = true;
		}
	}

	// return result
	return result;

}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getTMValue(UINT32 tag, BYTE *data_ptr, UINT length)

//  DESCRIPTION     : Get tagged TM value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_TM) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			if (value_ptr->GetLength() < length) length = (UINT)(value_ptr->GetLength());
			BYTE *l_data_ptr;
			value_ptr->Get(&l_data_ptr, length);
			byteCopy(data_ptr, l_data_ptr, length);
			data_ptr[length] = 0x00;
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getLOValue(UINT32 tag, string& data)

//  DESCRIPTION     : Get tagged LO value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_LO) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			value_ptr->Get(data);
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getPNValue(UINT32 tag, string& data)

//  DESCRIPTION     : Get tagged PN value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_PN) && 
		(attribute_ptr->GetNrValues() > 0)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			value_ptr->Get(data);
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getUIValue(UINT32 tag, string& data)

//  DESCRIPTION     : Get tagged UI value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	//Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_UI) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			// ok - return attribute value
			value_ptr->Get(data);
			result = true;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getUSValue(UINT32 tag, UINT16 *data_ptr)

//  DESCRIPTION     : Get tagged US value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    bool result = false;

    //Get group & element
	unsigned short	group = ((unsigned short) (tag >> 16));
	unsigned short	element = ((unsigned short) (tag & 0x0000FFFF));

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(group,element,true);

    if (attribute_ptr == NULL) return false;
    //
    // have we got the correct VR ?
    //
    if (
        (attribute_ptr->GetVR() == ATTR_VR_US) && 
        (attribute_ptr->GetNrValues() == 1)
        )
    {
        BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);
        //
        // have we really got a value ?
        //
        if (value_ptr)
        {
            UINT16 value;
            value_ptr->Get(value);

            // ok - return attribute value
            *data_ptr = value;
            result = true;
        }
    }
    return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::getULValue(UINT32 tag, UINT32 *data_ptr)

//  DESCRIPTION     : Get tagged UL value from object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool	result = false;

	// get the attribute with the given tag
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttributeByTag(tag);
	if (attribute_ptr == NULL) return false;

	// have we got the correct VR ?
	if ((attribute_ptr->GetVR() == ATTR_VR_UL) && 
		(attribute_ptr->GetNrValues() == 1)) 
	{
		BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(0);

		// have we really got a value ?
		if (value_ptr)
		{
			UINT32 value;
			value_ptr->Get(value);

			// ok - return attribute value
			*data_ptr = value;
			result = true;
		}
	}

	// return result
	return result;
}
