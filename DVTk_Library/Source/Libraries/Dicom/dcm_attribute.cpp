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
#include "dcm_attribute.h"
#include "dcm_value_sq.h"
#include "dcm_value_ul.h"
#include "private_attribute.h"

#include "Idefinition.h"		// Definition component interface


//>>===========================================================================

DCM_ATTRIBUTE_CLASS::DCM_ATTRIBUTE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetGroup(TAG_UNDEFINED_GROUP);
	SetElement(TAG_UNDEFINED_ELEMENT);
	SetType(ATTR_TYPE_3);
	SetVR(ATTR_VR_UN);
	mappedGroupM = TAG_UNDEFINED_GROUP; 	
	mappedElementM = TAG_UNDEFINED_ELEMENT;
	receivedLengthM = 0;
	transferVrM = TRANSFER_ATTR_VR_IMPLICIT;
	nestingDepthM = 0;
	definedLengthM = false;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	loggerM_ptr = NULL;
	parentM_ptr = NULL;
	pahM_ptr = NULL;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS::DCM_ATTRIBUTE_CLASS(UINT16 group, UINT16 element)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetGroup(group);	
	SetElement(element);
	SetType(ATTR_TYPE_3);
	SetVR(ATTR_VR_UN);
	mappedGroupM = group; 	
	mappedElementM = element;
	receivedLengthM = 0;
	transferVrM = TRANSFER_ATTR_VR_IMPLICIT;
	nestingDepthM = 0;
	definedLengthM = false;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	loggerM_ptr = NULL;
	parentM_ptr = NULL;
	pahM_ptr = NULL;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS::DCM_ATTRIBUTE_CLASS(UINT16 group, UINT16 element, ATTR_VR_ENUM vr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetGroup(group);	
	SetElement(element);
	SetType(ATTR_TYPE_3);
	SetVR(vr);
	mappedGroupM = group; 	
	mappedElementM = element;
	receivedLengthM = 0;
	transferVrM = TRANSFER_ATTR_VR_IMPLICIT;
	nestingDepthM = 0;
	definedLengthM = false;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	loggerM_ptr = NULL;
	parentM_ptr = NULL;
	pahM_ptr = NULL;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS::DCM_ATTRIBUTE_CLASS(UINT32 tag, ATTR_VR_ENUM vr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 group = ((UINT16) (tag >> 16));
	UINT16 element = ((UINT16) (tag & 0x0000FFFF));

	// constructor activities
	SetGroup(group);	
	SetElement(element);
	SetType(ATTR_TYPE_3);
	SetVR(vr);
	mappedGroupM = group; 	
	mappedElementM = element;
	receivedLengthM = 0;
	transferVrM = TRANSFER_ATTR_VR_IMPLICIT;
	nestingDepthM = 0;
	definedLengthM = false;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	loggerM_ptr = NULL;
	parentM_ptr = NULL;
	pahM_ptr = NULL;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS::~DCM_ATTRIBUTE_CLASS()

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

bool DCM_ATTRIBUTE_CLASS::replaceValue(int index, BASE_VALUE_CLASS *value_ptr)

//  DESCRIPTION     : Method to replace the indexed attribute value with
//					  the given value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check index range
	if (index >= GetNrValues()) return false;

	// replace old value with new one
	Replace(value_ptr, index);

	// return result
	return true;
}

//>>===========================================================================

void DCM_ATTRIBUTE_CLASS::setTransferVR(TRANSFER_ATTR_VR_ENUM transferVr)

//  DESCRIPTION     : Set the transfer VR - this parameter is used to indicate
//					  how an attribute should be encoded - implicit, explicit,
//					  unknown or using question marks.
//					  This parameter is also used in logging the attribute VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// only change transfer VR if it is not already UNKNOWN or QUESTION
	if ((transferVrM == TRANSFER_ATTR_VR_UNKNOWN) ||
		(transferVrM == TRANSFER_ATTR_VR_QUESTION))
	{
		// value has been set from script and so should not be changed
		return;
	}

	// store new transfer VR
	transferVrM = transferVr;

	// if this attribute has an SQ VR - recurse through the sequence items
	// - only interested if one SQ value available
	if ((GetVR() == ATTR_VR_SQ) &&
		(GetNrValues() == 1))
	{
		// get SQ value
		DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(GetValue(0));

		// set the transfer VR
		sqValue_ptr->setTransferVR(transferVr);
	}
}

//>>===========================================================================

void  DCM_ATTRIBUTE_CLASS::setDefinedLength(bool flag)

//  DESCRIPTION     : Set the defined length flag. This really only applies to
//					  SQ VR attributes and items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// set the defined length
	definedLengthM = flag;

	// if this attribute has an SQ VR - recurse through the sequence items
	// - only interested if one SQ value available
	if ((GetVR() == ATTR_VR_SQ) &&
		(GetNrValues() == 1))
	{
		// get SQ value
		DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(GetValue(0));

		// set the defined length
		sqValue_ptr->setDefinedLength(flag);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_CLASS::setDefineGroupLengths(bool flag)
{ 
	// if this attribute has an SQ VR - recurse through the sequence items
	// - only interested if one SQ value available
	if ((GetVR() == ATTR_VR_SQ) &&
		(GetNrValues() == 1))
	{
		// get SQ value
		DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(GetValue(0));

		// set the nestingDepth
		sqValue_ptr->setDefineGroupLengths(flag);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_CLASS::setNestingDepth(int nestingDepth)

//  DESCRIPTION     : Set the nesting depth.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// set the nesting depth
	nestingDepthM = nestingDepth;

	// if this attribute has an SQ VR - recurse through the sequence items
	// - only interested if one SQ value available
	if ((GetVR() == ATTR_VR_SQ) &&
		(GetNrValues() == 1))
	{
		// get SQ value
		DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(GetValue(0));

		// set the nestingDepth
		sqValue_ptr->setNestingDepth(nestingDepth);
	}
}

//>>===========================================================================

UINT32 DCM_ATTRIBUTE_CLASS::getPaddedLength()

//  DESCRIPTION     : Compute the padded length of the attribute value(s).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32 paddedLength = 0;
	ATTR_VR_ENUM vr = GetVR();

	// check if attribute is present
	if (!IsPresent()) return 0;

	if (vr == ATTR_VR_SQ)
	{
		// only interested if one SQ value available
		if (GetNrValues() == 1)
		{
			// get SQ value
			DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(GetValue(0));

			if (sqValue_ptr->isDefinedLength())
			{
				// get the real SQ length
				paddedLength = sqValue_ptr->GetLength();
			}
			else
			{
				// SQ length is undefined
				paddedLength = UNDEFINED_LENGTH;
			}
		}
	}
	else 
	{
		// get length of all Attribute Values
		for (int i = 0; i < GetNrValues(); i++)
		{
			paddedLength += GetValue(i)->GetLength();
		}

		switch (vr) 
		{
		case ATTR_VR_AE:
		case ATTR_VR_CS:
		case ATTR_VR_DA:
		case ATTR_VR_DS:
		case ATTR_VR_DT:
		case ATTR_VR_IS:
		case ATTR_VR_LO:
		case ATTR_VR_PN:
		case ATTR_VR_SH:
		case ATTR_VR_TM:
		case ATTR_VR_UI:
		case ATTR_VR_UC:
			// these VRs have the BACKSLASH as a delimiter between values
			// - this contributes to the overall length
			paddedLength += (GetNrValues() - 1);
			break;
		default:
			break;
		}
	}

	// the DICOM Standard states that all attributes must have an even length and
	// when odd will be padded to an even length. However, for testing we want to be
	// able to handle odd length attributes too. So our padding will only be doen if
	// the ensureEvenAttributeValueLengthM boolean is set true
    // - don't do this for SQ as it maybe UNDEFINED length
   	if (vr != ATTR_VR_SQ)
    {
		if (ensureEvenAttributeValueLengthM == true)
		{
    		if (paddedLength & 0x01) paddedLength++;
		}
    }

	// return padded length
	return paddedLength;
}

//>>===========================================================================

void DCM_ATTRIBUTE_CLASS::addSqValue(DCM_VALUE_SQ_CLASS *valueSq_ptr)

//  DESCRIPTION     : Add a sequence value to the attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// store parent
	valueSq_ptr->setParent(this);

	// add sequence value using base class
	ATTRIBUTE_CLASS::AddValue(valueSq_ptr);
}

//>>===========================================================================

bool DCM_ATTRIBUTE_CLASS::operator > (ATTRIBUTE_CLASS& attribute)

//  DESCRIPTION     : Returns true if the left-hand value is greater than the
//                    right-hand value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (GetGroup() > attribute.GetGroup())
    {
        return true;
    }

    if ((GetGroup() == attribute.GetGroup()) &&
        (GetElement() > attribute.GetElement()))
    {
        return true;
    }

    return false;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_CLASS::operator = (DCM_ATTRIBUTE_CLASS& sourceAttribute)

//  DESCRIPTION     : Operator assignment - for assigning this attribute to the same value as the source.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The attribute properties and value(s) will be copied. The tag (group,element) will not be copied.
//<<===========================================================================
{
	// save local group & element
	UINT16 group = GetGroup();
	UINT16 element = GetElement();
	UINT16 mappedGroup = GetMappedGroup();
	UINT16 mappedElement = GetMappedElement();
	receivedLengthM = sourceAttribute.receivedLengthM;

	// copy VR
    ATTR_VR_ENUM vr = sourceAttribute.GetVR();
	SetVR(vr);

	// copy EnsureEvenAttributeValueLength
	ensureEvenAttributeValueLengthM = sourceAttribute.ensureEvenAttributeValueLengthM;

	// copy logger
	loggerM_ptr = sourceAttribute.loggerM_ptr;

	// special checks for some VRs
    switch(vr)
    {
    case ATTR_VR_SQ:
   		// only interested if one SQ value available
		if (sourceAttribute.GetNrValues() == 1)
		{
			// get SQ value
			DCM_VALUE_SQ_CLASS *sourceSqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(sourceAttribute.GetValue(0));
            if (sourceSqValue_ptr)
            {
                DCM_VALUE_SQ_CLASS *destSqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
                *destSqValue_ptr = *sourceSqValue_ptr;
                ATTRIBUTE_CLASS::AddValue(destSqValue_ptr); 
            }
        }
        break;
    case ATTR_VR_UL:
        // copy all UL values
        for (int i = 0; i < sourceAttribute.GetNrValues(); i++)
        {
			// get UL value
			DCM_VALUE_UL_CLASS *sourceUlValue_ptr = static_cast<DCM_VALUE_UL_CLASS*>(sourceAttribute.GetValue(i));
            if (sourceUlValue_ptr)
            {
			    DCM_VALUE_UL_CLASS *destUlValue_ptr = new DCM_VALUE_UL_CLASS();
                *destUlValue_ptr = *sourceUlValue_ptr;
                ATTRIBUTE_CLASS::AddValue(destUlValue_ptr); 
            }
        }
        break;
    default:
        // copy all values
        for (int i = 0; i < sourceAttribute.GetNrValues(); i++)
        {
			// get UL value
			BASE_VALUE_CLASS *sourceValue_ptr = sourceAttribute.GetValue(i);
            if (sourceValue_ptr)
            {
                BASE_VALUE_CLASS *destValue_ptr = CreateNewValue(vr);
                *destValue_ptr = *sourceValue_ptr;
                ATTRIBUTE_CLASS::AddValue(destValue_ptr);
            }
        }
        break;
    }

	// restore local group & element after base class copy
	SetGroup(group);
	SetElement(element);
	SetMappedGroup(mappedGroup);
	SetMappedElement(mappedElement);

	// return copy result
	return true;
}

//>>===========================================================================

void DCM_ATTRIBUTE_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Cascade the logger through the attribute values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// store local logger
	loggerM_ptr = logger_ptr; 
}

//>>===========================================================================

void DCM_ATTRIBUTE_CLASS::setEnsureEvenAttributeValueLength(bool flag)

//  DESCRIPTION     : Cascade the EnsureEvenAttributeValueLength flag through the attribute values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	ensureEvenAttributeValueLengthM = flag;

	// special checks for some VRs
    switch(GetVR())
    {
    case ATTR_VR_SQ:
   		// only interested if one SQ value available
		if (GetNrValues() == 1)
		{
			// get SQ value
			DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(GetValue(0));
            if (sqValue_ptr)
            {
				sqValue_ptr->setEnsureEvenAttributeValueLength(flag);
            }
        }
        break;
    default:
		break;
	}
}

