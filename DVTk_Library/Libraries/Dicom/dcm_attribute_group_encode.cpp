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
//  DESCRIPTION     :	DICOM Attribute Group Class - encode & decode methods.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_attribute_group.h"
#include "dcm_value_sq.h"

#include "Idefinition.h"		// Definition component interface


//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::encode(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : encode object to dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;

	// check if we have pixel data that may need updating with regard to color pixel
	// pattern generation
	updatePixelData();

	// encode all attributes
	for (int i = 0; i < GetNrAttributes(); i++)
	{
		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttribute(i);
		if (attribute_ptr == NULL) return false;

		// encode attributes one at a time
		if (!attribute_ptr->encode(dataTransfer))
		{
			result = false;
			break;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::decode(DATA_TF_CLASS& dataTransfer, UINT16 lastGroup, UINT16 lastElement, UINT32 *length_ptr)

//  DESCRIPTION     : decode object from dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = true;
	UINT32 length = 0;

	UINT32 remainingGroupTwoLength = UNDEFINED_LENGTH;

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

		// set the UN VR definition look-up flag
		attribute_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

		// set EnsureEvenAttributeValueLength
		attribute_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);
		
		// decode attributes one at a time
		if (attribute_ptr->decode(dataTransfer, lastGroup, lastElement, &attributeLength))
		{
			// add attribute length to total
			length += attributeLength;

			// add attribute to object
			addAttribute(attribute_ptr);

			// check if we have a group 0002 length attribute
			// - group 0002 is present at the beginning of a DCM file
			// - we need to stop decoding the media file header at the
			// end of group 0002. A second call is used to decode higher
			// groups.
			if ((attribute_ptr->GetGroup() == GROUP_TWO) &&
				(attribute_ptr->GetElement() == LENGTH_ELEMENT))
			{
				// get the group 0002 length value
				BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue();
				value_ptr->Get(remainingGroupTwoLength);
			}
			else if (attribute_ptr->GetGroup() == GROUP_TWO)
			{
				// decrement the group 0002 length
				remainingGroupTwoLength -= attributeLength;

				// if we have reached the end of the given group 0002 length - return
				if (remainingGroupTwoLength == 0) break;
			}
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
}
