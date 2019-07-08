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
#include "definition.h"
#include "AttributeDefinition.h"


//*****************************************************************************
//  INTERNAL DECLARATIONS
//*****************************************************************************
static bool MapDICOMName2XMLName(const string dicom_name, string& xml_name);


//>>===========================================================================

DEF_ATTRIBUTE_CLASS::DEF_ATTRIBUTE_CLASS()

//  DESCRIPTION     : Default Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	secondVrM = ATTR_VR_UN;
	SetVmMin(0);
	SetVmMax(0);
	SetVmRestriction(ATTR_VM_RESTRICT_NONE);
	conditionM_ptr = NULL;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS::DEF_ATTRIBUTE_CLASS(UINT16 group, UINT16 element)

//  DESCRIPTION     : Constructor with Attribute Group and Element
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetGroup(group);
	SetElement(element);
	SetVR(ATTR_VR_UN);
	SetType(ATTR_TYPE_3);
	secondVrM = ATTR_VR_UN;
	SetVmMin(0);
	SetVmMax(0);
	SetVmRestriction(ATTR_VM_RESTRICT_NONE);
	conditionM_ptr = NULL;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS::DEF_ATTRIBUTE_CLASS(UINT16 group, UINT16 element, ATTR_VR_ENUM vr)

//  DESCRIPTION     : Constructor with Attribute Group and Element & VR
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetGroup(group);
	SetElement(element);
	SetVR(vr);
	SetType(ATTR_TYPE_3);
	secondVrM = ATTR_VR_UN;
	SetVmMin(0);
	SetVmMax(0);
	SetVmRestriction(ATTR_VM_RESTRICT_NONE);
	conditionM_ptr = NULL;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS::~DEF_ATTRIBUTE_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (conditionM_ptr)
	{
//		delete conditionM_ptr;
	}
}

//>>===========================================================================

void DEF_ATTRIBUTE_CLASS::SetName(const string Name)

//  DESCRIPTION     : Sets attribute name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	nameM = Name;

	// also set XML name
	MapDICOMName2XMLName(nameM, xml_nameM);
}


//>>===========================================================================

string DEF_ATTRIBUTE_CLASS::GetConditionResultMessage()

//  DESCRIPTION     : Returns condition result message
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	if (conditionM_ptr)
	{
		return conditionM_ptr->GetResultMessage();
	}
	else
	{
		return textual_conditionM;
	}
}

//>>===========================================================================

bool DEF_ATTRIBUTE_CLASS::operator = (DEF_ATTRIBUTE_CLASS& sourceAttribute)

//  DESCRIPTION     : Operator assignment.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// call base class operator to copy base class
	ATTRIBUTE_CLASS::operator=(sourceAttribute);

	// copy other parameters
	secondVrM = sourceAttribute.secondVrM;
	vmM.ATTR_VM_RESTRICTION = sourceAttribute.vmM.ATTR_VM_RESTRICTION;
	vmM.ATTR_VM_MIN = sourceAttribute.vmM.ATTR_VM_MIN;
	vmM.ATTR_VM_MAX = sourceAttribute.vmM.ATTR_VM_MAX;
	nameM = sourceAttribute.nameM;
	xml_nameM = sourceAttribute.xml_nameM;

	// condition needs copying by value - not by reference
	conditionM_ptr = sourceAttribute.conditionM_ptr;
	textual_conditionM = sourceAttribute.textual_conditionM;

	// return copy result
	return true;
}


//>>===========================================================================

static bool MapDICOMName2XMLName(const string dicom_name, string& xml_name)

//  DESCRIPTION     : Maps a DICOM Name to an XML name 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Currently the following transformation is used:
//                    - All characters to lowercase
//                    - spaces are replaced by underscores '_'
//                    - special characters (quotes etc) are removed 
//<<===========================================================================
{
	bool result = true;
	string tmp;
	char current_char = '\0';
	char changed_char = '\0';
	UINT i = 0;

	while (i < dicom_name.length())
	{
        current_char = (char)dicom_name[i];

		// Remove undesired character
		if (int(current_char) < 0)
		{
			i++;
			continue;
		}
		
		// only add 'normal' characters
		if (isalnum(current_char))
		{
	       changed_char = (char) tolower(current_char);
           tmp += changed_char;
		}
		else if (isspace(current_char))
		{
		    changed_char = '_';
			tmp += changed_char;
		}
        i++;
	}

    xml_name = tmp;

    return result;
}

