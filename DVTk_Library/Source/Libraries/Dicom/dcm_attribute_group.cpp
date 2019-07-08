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
#include "dcm_attribute_group.h"
#include "dcm_value_ul.h"
#include "dcm_value_sq.h"

#include "Idefinition.h"		// Definition component interface


//>>===========================================================================

DCM_ATTRIBUTE_GROUP_CLASS::DCM_ATTRIBUTE_GROUP_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_UNKNOWN;
	identifierM = "";
	nestingDepthM = 0;
	defineGroupLengthsM = false;
	encodePresentationContextIdM = 0;
	populateWithAttributesM = true;
	validateOnReceiveM = false;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	offsetM = 0;
	pixelRepresentationM = 0;
	parentM_ptr = NULL;
	loggerM_ptr = NULL;
	pahM_ptr = NULL;
}

//>>===========================================================================

DCM_ATTRIBUTE_GROUP_CLASS::~DCM_ATTRIBUTE_GROUP_CLASS()

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

void DCM_ATTRIBUTE_GROUP_CLASS::setGroupLengths(TS_CODE tsCode)

//  DESCRIPTION     : Method to compute the value of the Group Length attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// run through all attributes
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		// get hold of the Group Length attribute
		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttribute(i);

		if (attribute_ptr->GetElement() == LENGTH_ELEMENT)
		{
			UINT32 length = 0;

			for (int j = 0; j < GetNrAttributes(); j++) 
			{
				DCM_ATTRIBUTE_CLASS *tempAttribute_ptr = GetAttribute(j);

				// only include attribute if it is set to present - i.e. not deleted
				if (tempAttribute_ptr->IsPresent() == true)
				{
					// attribute belongs to Group in question
					if (tempAttribute_ptr->GetGroup() == attribute_ptr->GetGroup())
					{
						// do not include the Group Length Attribute itself
						if (tempAttribute_ptr->GetElement() != LENGTH_ELEMENT)
						{
							length += (sizeof(UINT16) + sizeof(UINT16)
									+ sizeof(UINT32) + tempAttribute_ptr->getPaddedLength());

							// NOTE: add length of extra padding field for explicit encoding
							// of OB, OF, OW, SQ, UN & UT
							if ((tsCode & TS_EXPLICIT_VR)
							  && ((tempAttribute_ptr->GetVR() == ATTR_VR_OB) 
							  || (tempAttribute_ptr->GetVR() == ATTR_VR_OF)
							  || (tempAttribute_ptr->GetVR() == ATTR_VR_OW)
							  || (tempAttribute_ptr->GetVR() == ATTR_VR_SQ)
							  || (tempAttribute_ptr->GetVR() == ATTR_VR_UN)
							  || (tempAttribute_ptr->GetVR() == ATTR_VR_UT)))
							{
								length += sizeof(UINT32);
							}
						}
					}
				}
			}

			// set the Group Length value
			DCM_VALUE_UL_CLASS *value_ptr = new DCM_VALUE_UL_CLASS();
			value_ptr->Set(length);

			// add Value to attribute
			attribute_ptr->replaceValue(0, value_ptr);
		}
		else
		{
			// recurse through the sequence items
			if (attribute_ptr->GetVR() == ATTR_VR_SQ)
			{
				// only interested if one SQ value available
				if (attribute_ptr->GetNrValues() == 1)
				{
					// get SQ value
					DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

					// set the group lengths
					sqValue_ptr->setGroupLengths(tsCode);
				}
			}
		}
	}
}


//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::addGroupLengths()

//  DESCRIPTION     : Method to add the Group Length attributes to the
//					  DICOM object. They are initially filled in with a zero
//					  length. The actual lengths need to be computed when
//					  when the sequence lengths are known.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32	thisGroup = 0xFFFF;

	// sort attributes before adding group lengths
	SortAttributes();

	// run through all Attributes - they may not have been sorted yet
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		DCM_ATTRIBUTE_CLASS	*attribute_ptr = GetAttribute(i);

		if (attribute_ptr->GetGroup() != thisGroup) 
		{
			// we are in a new group and must check to see if a group length should be added
			if (attribute_ptr->GetElement() != LENGTH_ELEMENT) 
			{
				// set the Group Length value
				UINT32	length = 0;
				DCM_VALUE_UL_CLASS *value_ptr = new DCM_VALUE_UL_CLASS();
				value_ptr->Set(length);

				// allocate the Group Length attribute
				DCM_ATTRIBUTE_CLASS *groupLengthAttribute_ptr = new DCM_ATTRIBUTE_CLASS(attribute_ptr->GetGroup(), LENGTH_ELEMENT);
				groupLengthAttribute_ptr->SetVR(ATTR_VR_UL);

				// make Group Length mandatory to ensure it is encoded
				groupLengthAttribute_ptr->SetType(ATTR_TYPE_1);

				// set EnsureEvenAttributeValueLength
				groupLengthAttribute_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

				// add Value to Attribute
				groupLengthAttribute_ptr->AddValue(value_ptr);

				// add Attribute to Object
				addAttribute(groupLengthAttribute_ptr);
			}
			else
			{
				// check if a value has been defined
				if (attribute_ptr->GetNrValues() == 0)
				{
					// set the Group Length value
					UINT32	length = 0;
					DCM_VALUE_UL_CLASS *value_ptr = new DCM_VALUE_UL_CLASS;
					value_ptr->Set(length);

					// make Group Length mandatory to ensure it is encoded
					attribute_ptr->SetType(ATTR_TYPE_1);

					// add Value to Attribute
					attribute_ptr->AddValue(value_ptr);
				}
			}

			thisGroup = attribute_ptr->GetGroup();
		}

		// recurse through the sequence items
		if (attribute_ptr->GetVR() == ATTR_VR_SQ)
		{
			// only interested if one SQ value available
			if (attribute_ptr->GetNrValues() == 1)
			{
				// get SQ value
				DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

				// set the group lengths
				sqValue_ptr->addGroupLengths();
			}
		}
	}

	// having added the Group Length - we must sort the object again
	SortAttributes();
}

//>>===========================================================================

UINT32 DCM_ATTRIBUTE_GROUP_CLASS::computeLength(TS_CODE tsCode)

//  DESCRIPTION     : Method to compute the length of the object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This method maybe called recursively.
//<<===========================================================================
{
	// initialise the length
	UINT32 totalLength = 0;

	// loop computing the total length of the DICOM object
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		// get the Attribute Data
		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttribute(i);

		// Only include length - if attribute is set present - that is, it is not flagged for deletion.
		if (attribute_ptr->IsPresent() == true)
		{
			ATTR_VR_ENUM vr = attribute_ptr->GetVR();
			ATTR_TYPE_ENUM type = attribute_ptr->GetType();
			UINT32 length = attribute_ptr->getPaddedLength();

			if ((vr != ATTR_VR_SQ) &&
				(length == 0) && 
				((type == ATTR_TYPE_1C) ||
				(type == ATTR_TYPE_2C)))
			{
				// do not encode zero length Type1C, Type2C
				continue;
			}

			// count length of the group & element = 4 bytes
			totalLength += 4;

			// count the [VR] length
			if (tsCode & TS_IMPLICIT_VR)
			{
				// implicit VR - 32 bit length used = 4 bytes
				totalLength += 4;
			}
			else 
			{
				// explicit VR = 2 bytes
				totalLength += 2;
			
				// check for special OB, OF, OW, OL, OD, SQ, UN, UR, UC& UT encoding
			if ((vr == ATTR_VR_OB) || 
				(vr == ATTR_VR_OF) || 
				(vr == ATTR_VR_OW) || 
				(vr == ATTR_VR_OL) || 
				(vr == ATTR_VR_OV) || 
				(vr == ATTR_VR_OD) || 
				(vr == ATTR_VR_SQ) || 
				(vr == ATTR_VR_UN) || 
				(vr == ATTR_VR_UR) ||
				(vr == ATTR_VR_UC) ||
				(vr == ATTR_VR_UT)) 
				{
					// 16 bit padding & 32 bit length used = 6 bytes
					totalLength += 6;
				}
				else 
				{
					// 16 bit length used = 4 bytes
					totalLength += 2;
				}
			}

			// handle the SQs separately
			if (vr == ATTR_VR_SQ)
			{
				// only interested if one SQ value available
				if (attribute_ptr->GetNrValues() == 1)
				{
					// get SQ value
					DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

					// get the SQ length
					totalLength += sqValue_ptr->computeLength(tsCode);
				}
			}
			else 
			{
				// count standard attribute length
				totalLength += length;
			}
		}
	}

	// return computed length
	return totalLength;
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setNestingDepth(int nestingDepth)

//  DESCRIPTION     : Method to set the nesting depth for all SQ items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// save nesting depth
	nestingDepthM = nestingDepth;

	// run through all Attributes
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		DCM_ATTRIBUTE_CLASS	*attribute_ptr = GetAttribute(i);

		// set the nesting depth
		attribute_ptr->setNestingDepth(nestingDepth);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setTransferVR(TRANSFER_ATTR_VR_ENUM transferVr)

//  DESCRIPTION     : Method to set the transfer VR for all attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This is used in order to set up the correct VR for
//					  logging purposes on send.
//<<===========================================================================
{
	// run through all Attributes
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		DCM_ATTRIBUTE_CLASS	*attribute_ptr = GetAttribute(i);

		// set the transfer VR
		attribute_ptr->setTransferVR(transferVr);
	}
}


//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setDefineGroupLengths(bool flag)

//  DESCRIPTION     : Method to set the group length flag for all attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// set local flag
	defineGroupLengthsM = flag;

	// run through all Attributes
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		DCM_ATTRIBUTE_CLASS	*attribute_ptr = GetAttribute(i);

		// set the defined lengths flag
		attribute_ptr->setDefineGroupLengths(flag);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setDefineSqLengths(bool flag)

//  DESCRIPTION     : Method to set the defined length flag for all attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// run through all Attributes
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		DCM_ATTRIBUTE_CLASS	*attribute_ptr = GetAttribute(i);

		// set the defined lengths flag
		attribute_ptr->setDefinedLength(flag);
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::merge(DCM_ATTRIBUTE_GROUP_CLASS *mergeDicomObject_ptr)

//  DESCRIPTION     : Merge the given dicom object into "this".
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The merge attributes can overwrite "this" attributes.
//<<===========================================================================
{
	// check if we are trying to merge this into this
	if (mergeDicomObject_ptr == this)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Trying to merge into same object - doing nothing");
		}

		// don't do anything
		return;
	}

	// merge given object into this
	// clean up any attribute
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Merge attribute count: %d", mergeDicomObject_ptr->GetNrAttributes());
	}

	while (mergeDicomObject_ptr->GetNrAttributes()) 
	{
		// get next merge attribute and remove it from source
		DCM_ATTRIBUTE_CLASS *mergeAttribute_ptr = mergeDicomObject_ptr->GetAttribute(0);
		mergeDicomObject_ptr->DeleteAttributeIndex(0);

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Merge attribute is (%04X,%04X)", mergeAttribute_ptr->GetGroup(), mergeAttribute_ptr->GetElement());
		}

		if(!mergeAttribute_ptr->IsPresent())
		{
			DeleteAttribute(mergeAttribute_ptr->GetMappedGroup(), mergeAttribute_ptr->GetMappedElement());
			continue;
		}

		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetMappedAttribute(mergeAttribute_ptr->GetMappedGroup(), mergeAttribute_ptr->GetMappedElement(), true);

		if (attribute_ptr != NULL)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "GetMappedAttribute returns (%04X,%04X)", attribute_ptr->GetGroup(), attribute_ptr->GetElement());
			}

			// check VR of the merge attribute for SQ
			if (mergeAttribute_ptr->GetVR() == ATTR_VR_SQ)
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Merge attribute (%04X,%04X) is SQ", mergeAttribute_ptr->GetGroup(), mergeAttribute_ptr->GetElement());
				}

				// check VR of matching attribute in destination
				if (attribute_ptr->GetVR() != ATTR_VR_SQ)
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Destination attribute (%04X,%04X) is not SQ", attribute_ptr->GetGroup(), attribute_ptr->GetElement());
						loggerM_ptr->text(LOG_DEBUG, 1, "Replacing destination attribute by merge SQ attribute - (%04X,%04X)", mergeAttribute_ptr->GetGroup(), mergeAttribute_ptr->GetElement());
					}

					// source to destination VRs are different
					// overwrite destination with source
					DeleteMappedAttributeIndex(mergeAttribute_ptr->GetMappedGroup(), mergeAttribute_ptr->GetMappedElement());

					// copy merge attribute to destination
					addAttribute(mergeAttribute_ptr);
				}
				else // destination attribute is a SQ
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Destination attribute (%04X,%04X) is SQ", attribute_ptr->GetGroup(), attribute_ptr->GetElement());
					}

					// check if destination SQ is empty
					if (attribute_ptr->GetNrValues() == 0)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Destination attribute has no items");

						// check if there are items in the source
						if (mergeAttribute_ptr->GetNrValues() != 0)
						{
							// allocate new SQ value for destination
							DCM_VALUE_SQ_CLASS *sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
		
							// add SQ value to destination
							attribute_ptr->addSqValue(sqValue_ptr);

							// copy items from source to destination
							sqValue_ptr->merge(mergeAttribute_ptr);
						}
					}
					else if (attribute_ptr->GetNrValues() == 1)
					{
						// sequence contains items
						DCM_VALUE_SQ_CLASS *sqValue_ptr = (DCM_VALUE_SQ_CLASS*) attribute_ptr->GetValue(0);
						
						// copy items from source to destination
						sqValue_ptr->merge(mergeAttribute_ptr);
					}
				}
			}
			else
			{
				// delete original attribute
				DeleteMappedAttributeIndex(mergeAttribute_ptr->GetMappedGroup(), mergeAttribute_ptr->GetMappedElement());

				// copy merge attribute from source to destination
				addAttribute(mergeAttribute_ptr);
				
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Replacing destination attribute by merge attribute - (%04X,%04X)", mergeAttribute_ptr->GetGroup(), mergeAttribute_ptr->GetElement());
				}
			}
		}
		else
		{
			// no matching attribute found in destination
			// simply copy merge attribute from source is destination
			addAttribute(mergeAttribute_ptr);
			
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Adding attribute for first time - (%04X,%04X)", mergeAttribute_ptr->GetGroup(), mergeAttribute_ptr->GetElement());
			}
		}
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::cloneAttributes(DCM_ATTRIBUTE_GROUP_CLASS *cloneDicomObject_ptr)

//  DESCRIPTION     : Clone the attribute group class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (int i = 0; i < cloneDicomObject_ptr->GetNrAttributes(); i++)
	{
		DCM_ATTRIBUTE_CLASS *cloneAttr_ptr = cloneDicomObject_ptr->GetAttribute(i);
		if (cloneAttr_ptr)
		{
			DCM_ATTRIBUTE_CLASS *attr_ptr = new DCM_ATTRIBUTE_CLASS(cloneAttr_ptr->GetMappedGroup(), cloneAttr_ptr->GetMappedElement());
			*attr_ptr = *cloneAttr_ptr;

			addAttribute(attr_ptr);
		}
	}
}


//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::addAttribute(DCM_ATTRIBUTE_CLASS *attribute_ptr)

//  DESCRIPTION     : Add attribute to DICOM object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// define logger for base value
	attribute_ptr->setLogger(loggerM_ptr);

	// set the private attribute handler
	attribute_ptr->setPAH(pahM_ptr);

	// cascade the parent
	attribute_ptr->setParent(this);

	// add attribute to object
	ATTRIBUTE_GROUP_CLASS::AddAttribute(attribute_ptr);
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *DCM_ATTRIBUTE_GROUP_CLASS::GetAttribute(UINT16 group, UINT16 element)

//  DESCRIPTION     : Get the attribute identified by the given group & element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// call the base class
	return static_cast<DCM_ATTRIBUTE_CLASS*>(ATTRIBUTE_GROUP_CLASS::GetAttribute(group, element));
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *DCM_ATTRIBUTE_GROUP_CLASS::GetMappedAttribute(UINT16 group, UINT16 element, bool parentOnly)

//  DESCRIPTION     : Get the mapped attribute identified by the given group & element.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// call the base class
	return static_cast<DCM_ATTRIBUTE_CLASS*>(ATTRIBUTE_GROUP_CLASS::GetMappedAttribute(group, element, parentOnly));
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *DCM_ATTRIBUTE_GROUP_CLASS::GetAttribute(int index)

//  DESCRIPTION     : Get the indexed attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// call the base class
	return static_cast<DCM_ATTRIBUTE_CLASS*>(ATTRIBUTE_GROUP_CLASS::GetAttribute(index));
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *DCM_ATTRIBUTE_GROUP_CLASS::GetAttributeByTag(UINT32 tag)

//  DESCRIPTION     : Get the attribute identified by the given tag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// call the base class
	return static_cast<DCM_ATTRIBUTE_CLASS*>(ATTRIBUTE_GROUP_CLASS::GetAttributeByTag(tag));
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::containsAttributesFromGroup(UINT16 group)

//  DESCRIPTION     : Check if the object contains any attributes from the given group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// simple search for the required attribute
	for (int i = 0; i < GetNrAttributes(); i++)
	{
		if (!GetAttribute(i)) return false;

		// is group present ?
		if (GetAttribute(i)->GetGroup() == group)
		{
			// we have an attribute from this group present
			return true;
		}
	}

	// no attributes matching the given group found
	return false;
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::computeItemOffsets(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : Compute any item offsets - for DICOMDIR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop computing the offsets for each attribute
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		// get the Attribute Data
		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttribute(i);

		ATTR_VR_ENUM vr = attribute_ptr->GetVR();
		ATTR_TYPE_ENUM type = attribute_ptr->GetType();
		UINT32 length = attribute_ptr->getPaddedLength();

		if ((vr != ATTR_VR_SQ) &&
			(length == 0) && 
			((type == ATTR_TYPE_1C) ||
			(type == ATTR_TYPE_2C)))
		{
			// do not encode zero length Type1C, Type2C
			continue;
		}

		// count length of the group & element = 4 bytes
		offsetM += 4;

		// count the [VR] length
		if (dataTransfer.getTsCode() & TS_IMPLICIT_VR)
		{
			// implicit VR - 32 bit length used = 4 bytes
			offsetM += 4;
		}
		else 
		{
			// explicit VR = 2 bytes
			offsetM += 2;
			
			// check for special OB, OF, OW, OL, OD, SQ, UN, UR, UC& UT encoding
			if ((vr == ATTR_VR_OB) || 
				(vr == ATTR_VR_OF) || 
				(vr == ATTR_VR_OW) || 
				(vr == ATTR_VR_OL) || 
				(vr == ATTR_VR_OV) || 
				(vr == ATTR_VR_OD) || 
				(vr == ATTR_VR_SQ) || 
				(vr == ATTR_VR_UN) || 
				(vr == ATTR_VR_UR) ||
				(vr == ATTR_VR_UC) ||
				(vr == ATTR_VR_UT)) 
			{
				// 16 bit padding & 32 bit length used = 6 bytes
				offsetM += 6;
			}
			else 
			{
				// 16 bit length used = 4 bytes
				offsetM += 2;
			}
		}

		// handle the SQs separately
		if (vr == ATTR_VR_SQ)
		{
			// only interested if one SQ value available
			if (attribute_ptr->GetNrValues() == 1)
			{
				// get SQ value
				DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

				// run through items in sequence
				sqValue_ptr->computeItemOffsets(dataTransfer, &offsetM);
			}
		}
		else 
		{
			// count standard attribute length
			offsetM += length;
		}
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::computeItemOffsets(string transferSyntax)

//  DESCRIPTION     : Compute any item offsets - for DICOMDIR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop computing the offsets for each attribute
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		// get the Attribute Data
		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttribute(i);

		ATTR_VR_ENUM vr = attribute_ptr->GetVR();
		ATTR_TYPE_ENUM type = attribute_ptr->GetType();
		UINT32 length = attribute_ptr->getPaddedLength();

		if ((vr != ATTR_VR_SQ) &&
			(length == 0) && 
			((type == ATTR_TYPE_1C) ||
			(type == ATTR_TYPE_2C)))
		{
			// do not encode zero length Type1C, Type2C
			continue;
		}

		// count length of the group & element = 4 bytes
		offsetM += 4;

		// count the [VR] length
		if (transferSyntaxUidToCode(transferSyntax) & TS_IMPLICIT_VR)
		{
			// implicit VR - 32 bit length used = 4 bytes
			offsetM += 4;
		}
		else 
		{
			// explicit VR = 2 bytes
			offsetM += 2;
			
			// check for special OB, OF, OW, OL, OD, SQ, UN, UR, UC& UT encoding
			if ((vr == ATTR_VR_OB) || 
				(vr == ATTR_VR_OF) || 
				(vr == ATTR_VR_OW) || 
				(vr == ATTR_VR_OL) || 
				(vr == ATTR_VR_OV) || 
				(vr == ATTR_VR_OD) || 
				(vr == ATTR_VR_SQ) || 
				(vr == ATTR_VR_UN) || 
				(vr == ATTR_VR_UR) ||
				(vr == ATTR_VR_UC) ||
				(vr == ATTR_VR_UT)) 
			{
				// 16 bit padding & 32 bit length used = 6 bytes
				offsetM += 6;
			}
			else 
			{
				// 16 bit length used = 4 bytes
				offsetM += 2;
			}
		}

		// handle the SQs separately
		if (vr == ATTR_VR_SQ)
		{
			// only interested if one SQ value available
			if (attribute_ptr->GetNrValues() == 1)
			{
				// get SQ value
				DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

				// run through items in sequence
				sqValue_ptr->computeItemOffsets(transferSyntax, &offsetM);
			}
		}
		else 
		{
			// count standard attribute length
			offsetM += length;
		}
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::updatePixelData()

//  DESCRIPTION     : Check for Pixel Data and update the lengths based on
//					: the image being a color.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS *bitsAllocated_ptr = GetAttributeByTag(TAG_BITS_ALLOCATED);
	DCM_ATTRIBUTE_CLASS *samplesPerPixel_ptr = GetAttributeByTag(TAG_SAMPLES_PER_PIXEL);
	DCM_ATTRIBUTE_CLASS *planarConfiguration_ptr = GetAttributeByTag(TAG_PLANAR_CONFIGURATION);
	DCM_ATTRIBUTE_CLASS *pixelData_ptr = GetAttributeByTag(TAG_PIXEL_DATA);
	UINT16 bitsAllocated = 8;
    UINT16 samplesPerPixel = 1;
	UINT16 planarConfiguration = FRAME_INTERLEAVE;
	BASE_VALUE_CLASS *value_ptr;

	// try and get the bits allocated
	if ((bitsAllocated_ptr) &&
		(bitsAllocated_ptr->GetVR() == ATTR_VR_US) &&
		(bitsAllocated_ptr->GetNrValues())) 
	{
		value_ptr = bitsAllocated_ptr->GetValue(0);
		if (value_ptr)
        {	
    		value_ptr->Get(bitsAllocated);
        }
	}

	// try and get the planar configuration
	if ((planarConfiguration_ptr) &&
		(planarConfiguration_ptr->GetVR() == ATTR_VR_US) &&
		(planarConfiguration_ptr->GetNrValues())) 
	{
		value_ptr = planarConfiguration_ptr->GetValue(0);
		if (value_ptr)
        {
		    value_ptr->Get(planarConfiguration);
        }
	}

	// try and get the samples per pixel
	if ((samplesPerPixel_ptr) && 
		(samplesPerPixel_ptr->GetVR() == ATTR_VR_US) && 
		(samplesPerPixel_ptr->GetNrValues())) 
	{
		value_ptr = samplesPerPixel_ptr->GetValue(0);
		if (value_ptr)
        {
		    value_ptr->Get(samplesPerPixel);
        }
    }

	if ((pixelData_ptr) &&
        ((pixelData_ptr->GetVR() == ATTR_VR_OB) || 
		 (pixelData_ptr->GetVR() == ATTR_VR_OF) ||
		 (pixelData_ptr->GetVR() == ATTR_VR_OW)) && 
		(pixelData_ptr->GetNrValues())) 
	{
	    OTHER_VALUE_CLASS *otherValue_ptr = static_cast<OTHER_VALUE_CLASS*>(pixelData_ptr->GetValue(0));
		if (otherValue_ptr == NULL) return; // should not happen
		
		// update the other data for the Bits Allocated, Samples Per Pixel and Planar Configuration
        otherValue_ptr->SetBitsAllocated(bitsAllocated);
        otherValue_ptr->SetSamplesPerPixel(samplesPerPixel);
        otherValue_ptr->SetPlanarConfiguration(planarConfiguration);
	}
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*)

//  DESCRIPTION     : Update this object with the contents of the object given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return result
	return false;
}

//>>===========================================================================

bool DCM_ATTRIBUTE_GROUP_CLASS::operator == (DCM_ATTRIBUTE_GROUP_CLASS &that)

//  DESCRIPTION     : Equality operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool equal = false;

	// lengths must be the same
	if (GetNrAttributes() == that.GetNrAttributes())
	{
		// set result true - could be zero attributes
		equal = true;

		// check attributes - the order is assumed to be the same in both objects
		for (int i = 0; i < GetNrAttributes(); i++)
		{
			// get individual attributes
			DCM_ATTRIBUTE_CLASS *thisAttribute_ptr = GetAttribute(i);
			DCM_ATTRIBUTE_CLASS *thatAttribute_ptr = that.GetAttribute(i);

			// check each attribute - tag (group/element) and contents
			if ((thisAttribute_ptr->GetGroup() != thatAttribute_ptr->GetGroup()) ||
				(thisAttribute_ptr->GetElement() != thatAttribute_ptr->GetElement()) ||
				(*thisAttribute_ptr != *thatAttribute_ptr))
			{
				// attributes not equal
				equal = false;
				break;
			}
		}
	}

	// return equality result
	return equal;
}

//>>===========================================================================
	
bool DCM_ATTRIBUTE_GROUP_CLASS::operator != (DCM_ATTRIBUTE_GROUP_CLASS &that)

//  DESCRIPTION     : Non-equality operator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return not equal result
	return (*this == that) ? false : true;
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setPAH(PRIVATE_ATTRIBUTE_HANDLER_CLASS *pah_ptr)

//  DESCRIPTION     : Set the private attribute handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	// set the private attribute handler
	pahM_ptr = pah_ptr;
}

//>>===========================================================================

PRIVATE_ATTRIBUTE_HANDLER_CLASS* DCM_ATTRIBUTE_GROUP_CLASS::getPAH()

//  DESCRIPTION     : Get the private attribute handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // get the private attribute handler
    return pahM_ptr;
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the logger - cascade it through all the attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	loggerM_ptr = logger_ptr;

	// cascade logger through the attributes
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		// set logger
		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttribute(i);
		if (attribute_ptr)
		{
			attribute_ptr->setLogger(loggerM_ptr);
		}
	}
}

//>>===========================================================================

void DCM_ATTRIBUTE_GROUP_CLASS::setEnsureEvenAttributeValueLength(bool flag)

//  DESCRIPTION     : Set the EnsureEvenAttributeValueLength flag - cascade it through all the attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ensureEvenAttributeValueLengthM = flag;

	// cascade EnsureEvenAttributeValueLength flag through the attributes
	for (int i = 0; i < GetNrAttributes(); i++) 
	{
		// set EnsureEvenAttributeValueLength flag
		DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttribute(i);
		if (attribute_ptr)
		{
			attribute_ptr->setEnsureEvenAttributeValueLength(flag);
		}
	}
}


//>>===========================================================================

void logStatus(LOG_CLASS *logger_ptr, UINT16 status)

//  DESCRIPTION     : Log the DICOM textual representation of the status code.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	UINT32 logLevel = LOG_NONE;

	// check if logger defined
	if (!logger_ptr) return;

	if (status != DCM_STATUS_SUCCESS) 
	{
		switch (status) 
		{
		case DCM_STATUS_ATTRIBUTE_LIST_ERROR:
		case DCM_STATUS_ATTRIBUTE_VALUE_OUT_OF_RANGE:
		case DCM_STATUS_PRINT_MEMORY_ALLOCATION_NOT_SUPPORTED:
		case DCM_STATUS_PRINT_FILM_SESSION_PRINTING_IS_NOT_SUPPORTED:
		case DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES:
		case DCM_STATUS_PRINT_FILM_BOX_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES:
		case DCM_STATUS_PRINT_IMAGE_SIZE_IS_LARGER_THAN_IMAGE_BOX_SIZE:
		case DCM_STATUS_PRINT_REQUESTED_DENSITY_OUTSIDE_PRINTERS_OPERATING_RANGE:
			logLevel = LOG_WARNING;
			logger_ptr->text(logLevel, 1, "Device returned non-zero Warning Status: %04X", status);
			switch (status) 
			{
			case DCM_STATUS_ATTRIBUTE_LIST_ERROR:
				logger_ptr->text(LOG_NONE, 0, " = ATTRIBUTE LIST ERROR."); 
				break;
			case DCM_STATUS_ATTRIBUTE_VALUE_OUT_OF_RANGE:
				logger_ptr->text(LOG_NONE, 0, " = ATTRIBUTE VALUE OUT OF RANGE.");
				break;
			case DCM_STATUS_PRINT_MEMORY_ALLOCATION_NOT_SUPPORTED:
				logger_ptr->text(LOG_NONE, 0, " = MEMORY ALLOCATION NOT SUPPORTED."); 
				break;
			case DCM_STATUS_PRINT_FILM_SESSION_PRINTING_IS_NOT_SUPPORTED:
				logger_ptr->text(LOG_NONE, 0, " = FILM SESSION PRINTING (COLLATION) IS NOT SUPPORTED.");
				break;
			case DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES:
				logger_ptr->text(LOG_NONE, 0, " = FILM SESSION SOP INSTANCE HIERARCHY DOES NOT CONTAIN IMAGE BOX SOP INSTANCES.");
				break;
			case DCM_STATUS_PRINT_FILM_BOX_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES:
				logger_ptr->text(LOG_NONE, 0, " = FILM BOX SOP INSTANCE HIERARCHY DOES NOT CONTAIN IMAGE BOX SOP INSTANCES."); 
				break;
			case DCM_STATUS_PRINT_IMAGE_SIZE_IS_LARGER_THAN_IMAGE_BOX_SIZE:
				logger_ptr->text(LOG_NONE, 0, " = IMAGE SIZE IS LARGER THAN IMAGE BOX SIZE, THE IMAGE HAS BEEN DEMAGNIFIED.");
				break;
			case DCM_STATUS_PRINT_REQUESTED_DENSITY_OUTSIDE_PRINTERS_OPERATING_RANGE:
				logger_ptr->text(LOG_NONE, 0, " = REQUESTED MIN. OR MAX. DENSITY OUTSIDE PRINTERS OPERATING RANGE.");
				break;
			default: break;
			}
			break;
		case DCM_STATUS_NO_SUCH_ATTRIBUTE:
		case DCM_STATUS_INVALID_ATTRIBUTE_VALUE:
		case DCM_STATUS_PROCESSING_FAILURE:
		case DCM_STATUS_NO_SUCH_OBJECT_INSTANCE:
		case DCM_STATUS_INVALID_ARGUMENT_TYPE:
		case DCM_STATUS_INVALID_OBJECT_INSTANCE:
		case DCM_STATUS_CLASS_INSTANCE_CONFLICT:
		case DCM_STATUS_MISSING_ATTRIBUTE:
		case DCM_STATUS_SOP_CLASS_NOT_SUPPORTED:
		case DCM_STATUS_DUPLICATE_INVOCATION:
		case DCM_STATUS_UNRECOGNIZED_OPERATION:
		case DCM_STATUS_RESOURCE_LIMITATION:
		case DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_FILM_BOX_SOP_INSTANCES:
		case DCM_STATUS_PRINT_CREATION_OF_PRINT_JOB_INSTANCE_FAILED_IN_FILM_SESSION:
		case DCM_STATUS_PRINT_CREATION_OF_PRINT_JOB_INSTANCE_FAILED_IN_FILM_BOX:
		case DCM_STATUS_PRINT_IMAGE_CONTAINS_MORE_PIXELS_THAN_THE_PRINTER_CAN_PRINT_IN_THE_IMAGE_BOX:
		case DCM_STATUS_PRINT_IMAGE_POSITION_COLLISION:
		case DCM_STATUS_PRINT_INSUFFICIENT_MEMORY_IN_PRINTER_TO_STORE_IMAGES:
			logLevel = LOG_ERROR;
			logger_ptr->text(logLevel, 1, "Device returned non-zero Error Status: %04X", status);
			switch (status) 
			{
			case DCM_STATUS_NO_SUCH_ATTRIBUTE: 
				logger_ptr->text(LOG_NONE, 0, " = NO SUCH ATTRIBUTE."); 
				break;
			case DCM_STATUS_INVALID_ATTRIBUTE_VALUE:
				logger_ptr->text(LOG_NONE, 0, " = INVALID ATTRIBUTE VALUE.");
				break;
			case DCM_STATUS_PROCESSING_FAILURE:
				logger_ptr->text(LOG_NONE, 0, " = PROCESSING FAILURE.");
				break;
			case DCM_STATUS_NO_SUCH_OBJECT_INSTANCE:
				logger_ptr->text(LOG_NONE, 0, " = NO SUCH OBJECT INSTANCE.");
				break;
			case DCM_STATUS_INVALID_ARGUMENT_TYPE:
				logger_ptr->text(LOG_NONE, 0, " = INVALID ARGUMENT TYPE."); 
				break;
			case DCM_STATUS_INVALID_OBJECT_INSTANCE:
				logger_ptr->text(LOG_NONE, 0, " = INVALID OBJECT INSTANCE.");
				break;
			case DCM_STATUS_CLASS_INSTANCE_CONFLICT:
				logger_ptr->text(LOG_NONE, 0, " = CLASS-INSTANCE CONFLICT.");
				break;
			case DCM_STATUS_MISSING_ATTRIBUTE:
				logger_ptr->text(LOG_NONE, 0, " = MISSING ATTRIBUTE.");
				break;
			case DCM_STATUS_SOP_CLASS_NOT_SUPPORTED:
				logger_ptr->text(LOG_NONE, 0, " = SOP CLASS NOT SUPPORTED.");
				break;
			case DCM_STATUS_DUPLICATE_INVOCATION:
				logger_ptr->text(LOG_NONE, 0, " = DUPLICATE INVOCATION."); 
				break;
			case DCM_STATUS_UNRECOGNIZED_OPERATION:
				logger_ptr->text(LOG_NONE, 0, " = UNRECOGNIZED OPERATION."); 
				break;
			case DCM_STATUS_RESOURCE_LIMITATION:
				logger_ptr->text(LOG_NONE, 0, " = RESOURCE LIMITATION.");
				break;
			case DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_FILM_BOX_SOP_INSTANCES:
				logger_ptr->text(LOG_NONE, 0, " = FILM SESSION SOP INSTANCE HIERARCHY DOES NOT CONTAIN FILM BOX SOP INSTANCES.");
				break;
			case DCM_STATUS_PRINT_CREATION_OF_PRINT_JOB_INSTANCE_FAILED_IN_FILM_SESSION:
				logger_ptr->text(LOG_NONE, 0, " = CREATION OF PRINT JOB INSTANCE FAILED IN FILM SESSION."); 
				break;
			case DCM_STATUS_PRINT_CREATION_OF_PRINT_JOB_INSTANCE_FAILED_IN_FILM_BOX:
				logger_ptr->text(LOG_NONE, 0, " = CREATION OF PRINT JOB INSTANCE FAILED IN FILM BOX."); 
				break;
			case DCM_STATUS_PRINT_IMAGE_CONTAINS_MORE_PIXELS_THAN_THE_PRINTER_CAN_PRINT_IN_THE_IMAGE_BOX:
				logger_ptr->text(LOG_NONE, 0, " = IMAGE CONTAINS MORE PIXELS THAN THE PRINTER CAN PRINT IN THE IMAGE BOX. PRINTER CANNOT DEMAGNIFY IMAGES."); 
				break;
			case DCM_STATUS_PRINT_IMAGE_POSITION_COLLISION:
				logger_ptr->text(LOG_NONE, 0, " = Retired IMAGE POSITION COLLISION."); 
				break;
			case DCM_STATUS_PRINT_INSUFFICIENT_MEMORY_IN_PRINTER_TO_STORE_IMAGES:
				logger_ptr->text(LOG_NONE, 0, " = INSUFFICIENT MEMORY IN PRINTER TO STORE IMAGES."); 
				break;
			default: break;
			}
			break;
		default: break;
		}
	}
}
