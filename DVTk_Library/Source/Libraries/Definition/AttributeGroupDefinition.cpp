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
#include "AttributeGroupDefinition.h"
#include "AttributeDefinition.h"
#include "MacroDefinition.h"

#include "Idicom.h"				// DICOM component interface


//>>===========================================================================

DEF_ATTRIBUTE_GROUP_CLASS::DEF_ATTRIBUTE_GROUP_CLASS()

//  DESCRIPTION     : Default constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	usageM = MOD_USAGE_M;
	contentRegisteredM = false;
	conditionM_ptr = NULL;
}

//>>===========================================================================

DEF_ATTRIBUTE_GROUP_CLASS::DEF_ATTRIBUTE_GROUP_CLASS(const string name)

//  DESCRIPTION     : Constructor with name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetName(name);
	usageM = MOD_USAGE_M;
	contentRegisteredM = false;
	conditionM_ptr = NULL;
}

//>>===========================================================================

DEF_ATTRIBUTE_GROUP_CLASS::DEF_ATTRIBUTE_GROUP_CLASS(const string name, const MOD_USAGE_ENUM usage)

//  DESCRIPTION     : Constructor with name and usage
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	SetName(name);
	usageM = usage;
	contentRegisteredM = false;
	conditionM_ptr = NULL;
}

//>>===========================================================================

DEF_ATTRIBUTE_GROUP_CLASS::~DEF_ATTRIBUTE_GROUP_CLASS()

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
		delete conditionM_ptr;
	}

	// - don't delete the macro references here - the actual macros are cleaned up elsewhere
	macro_refsM.clear();
}

//>>===========================================================================

void DEF_ATTRIBUTE_GROUP_CLASS::SetUsage(const MOD_USAGE_ENUM usage)

//  DESCRIPTION     : Set Usage.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	usageM = usage; 
}

//>>===========================================================================

void DEF_ATTRIBUTE_GROUP_CLASS::SetContentRegistered(const bool flag)

//  DESCRIPTION     : Set Content Registered.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	contentRegisteredM = flag;
}

//>>===========================================================================

void DEF_ATTRIBUTE_GROUP_CLASS::SetCondition(CONDITION_CLASS* cond_ptr) 

//  DESCRIPTION     : Set Condition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	conditionM_ptr = cond_ptr; 
}

//>>===========================================================================

void DEF_ATTRIBUTE_GROUP_CLASS::SetTextualCondition(const string message) 

//  DESCRIPTION     : Set Textual Condition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	textual_conditionM = message; 
}
		
//>>===========================================================================

MOD_USAGE_ENUM DEF_ATTRIBUTE_GROUP_CLASS::GetUsage() 

//  DESCRIPTION     : Get Usage.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return usageM; 
}

//>>===========================================================================

bool DEF_ATTRIBUTE_GROUP_CLASS::GetContentRegistered()

//  DESCRIPTION     : Get Content Registered.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return contentRegisteredM;
}

//>>===========================================================================

CONDITION_CLASS* DEF_ATTRIBUTE_GROUP_CLASS::GetCondition() 

//  DESCRIPTION     : Get Condition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return conditionM_ptr; 
}

//>>===========================================================================

bool DEF_ATTRIBUTE_GROUP_CLASS::EvaluateAddUserOptionalModule(ATTRIBUTE_GROUP_CLASS *dcmAttributeGroup_ptr, UINT32 *singleMatchingAttribute_ptr)

//  DESCRIPTION     : Return true when the actual data contains at least one 
//					: attribute in this module. If exactly one attribute is
//					: found, return it's DICOM tag in the singleMatchingAttribute_ptr
//					: parameter.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	*singleMatchingAttribute_ptr = 0;
	int	matchingCount = 0;
    int i = 0;
    while (i < (int)this->attributesM.size())
    {
        UINT16 group = attributesM[i]->GetGroup();
		UINT16 element = attributesM[i]->GetElement();
		ATTR_TYPE_ENUM type = attributesM[i]->GetType();

        // check if attribute type not 3C (dvt extension)
        // and not a private recognition code
		if ((type < ATTR_TYPE_3C) &&
            !(((group & 0x0001) != 0) && (element > 0x00FF)))
        { 
            // see if attribute is present in the DICOM attribute group - search parent level only
            if (dcmAttributeGroup_ptr->GetAttribute(group, element, true))
            {
				// found another attribute from the attribute group that is in this module.
				matchingCount++;

				if (matchingCount == 1)
				{
					// save the value of the matching DICOM attribute tag
					*singleMatchingAttribute_ptr = (group << 16) + element;
				}
            }
        }
        i++;
    }

    if (matchingCount == 0)
	{
        textual_conditionM = "Module did not match default condition (at least one attribute of module is present)";
	}
	else
	{
		textual_conditionM = "Module matches default condition (at least one attribute of module is present)";

		if (matchingCount > 1)
		{
			// reset the tag of the single matching attribute - as there is more than 1 matching attribute
			*singleMatchingAttribute_ptr = 0;
		}
	}

	// return false if no matches found
	return (matchingCount == 0 ? false : true); 
}

//>>===========================================================================

string DEF_ATTRIBUTE_GROUP_CLASS::GetTextualCondition() 

//  DESCRIPTION     : Get Textual Condition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return textual_conditionM; 
}

//>>===========================================================================

string DEF_ATTRIBUTE_GROUP_CLASS::GetConditionResultMessage()

//  DESCRIPTION     : Get Condition Result Message.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	// get the result message
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

void DEF_ATTRIBUTE_GROUP_CLASS::AddMacroReference(const string name, CONDITION_CLASS* cond_ptr, const string textualCondition)

//  DESCRIPTION     : Adds a macro reference to the group
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	MACRO_REF_STRUCT macro_ref;
	macro_ref.name = name;
	macro_ref.macro_ptr = NULL;
	macro_ref.cond_ptr = cond_ptr;
    macro_ref.textualCondition = textualCondition;
    macro_refsM.push_back(macro_ref);
}

//>>===========================================================================

void DEF_ATTRIBUTE_GROUP_CLASS::ResolveMacroReference(const string name, DEF_MACRO_CLASS* macro_ptr)

//  DESCRIPTION     : Resolves a macro reference 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through all macro references
	for (UINT i = 0; i < macro_refsM.size(); i++)
	{
		if (macro_refsM[i].name == name)
		{
			macro_refsM[i].macro_ptr = macro_ptr;
		}
	}
}

//>>===========================================================================

bool DEF_ATTRIBUTE_GROUP_CLASS::CheckMacroReferences()

//  DESCRIPTION     : Checks whether all macro references have been resolved
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool resolved = true;

	// check that all macro references have been resolved
	for (UINT i = 0; i < macro_refsM.size(); i++)
	{
		if (macro_refsM[i].macro_ptr == NULL)
		{
			resolved = false;
			break;
		}
	}

	// return resolved
	return resolved;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS* DEF_ATTRIBUTE_GROUP_CLASS::GetAttribute(UINT index)

//  DESCRIPTION     : Return pointer to indexed attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return static_cast<DEF_ATTRIBUTE_CLASS*>(ATTRIBUTE_GROUP_CLASS::GetAttribute(index));
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS* DEF_ATTRIBUTE_GROUP_CLASS::GetAttribute(UINT group, UINT element)

//  DESCRIPTION     : Return pointer to attribute with given tag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    DEF_ATTRIBUTE_CLASS* attribute_ptr = static_cast<DEF_ATTRIBUTE_CLASS*>(ATTRIBUTE_GROUP_CLASS::GetAttribute((UINT16)group, (UINT16)element));

	// if not found search through macros
	// the first occurence is returned
	if (attribute_ptr == NULL)
	{
		for (UINT i = 0; i < macro_refsM.size(); i++)
		{
			if (macro_refsM[i].macro_ptr)
			{
				attribute_ptr = macro_refsM[i].macro_ptr->GetAttribute(group, element);
				if (attribute_ptr) break;
			}
		}
	}

	// return the attribute pointer
	return attribute_ptr;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS* DEF_ATTRIBUTE_GROUP_CLASS::GetAttribute(UINT group, UINT element, DEF_MACRO_CLASS* macro_ptr)

//  DESCRIPTION     : Return pointer to attribute.
//                    If the specified macro_ptr is not null, this function searches 
//                    for the attribute in the specified macro.    
//                    This is neccessary as it is possible that attribute definitions
//                    are different in different macro's.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    DEF_ATTRIBUTE_CLASS* attribute_ptr = NULL;

	// get the attribute from the specified macro 
	if (macro_ptr)
	{
		// get attribute
        attribute_ptr = macro_ptr->GetAttribute(group, element);
	}

	//try to find the attribute definiton elsewhere in this attribute group
    if (attribute_ptr == NULL)
	{
        attribute_ptr = GetAttribute(group, element);     
	}

	// return attribute pointer
	return attribute_ptr;
}

//>>===========================================================================

DEF_MACRO_CLASS* DEF_ATTRIBUTE_GROUP_CLASS::GetMacro(UINT index) 

//  DESCRIPTION     : Returns the indexed macro pointer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{ 
	DEF_MACRO_CLASS *macro_ptr = NULL;

	if (index < macro_refsM.size())
	{
		macro_ptr = macro_refsM[index].macro_ptr;
	}

	// return the macro pointer
	return macro_ptr;
}

//>>===========================================================================

CONDITION_CLASS* DEF_ATTRIBUTE_GROUP_CLASS::GetMacroCondition(UINT index) 

//  DESCRIPTION     : Returns the indexed condition pointer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	CONDITION_CLASS *condition_ptr = NULL;

	if (index < macro_refsM.size())
	{
		condition_ptr = macro_refsM[index].cond_ptr;
	}

	// return the condition pointer
	return condition_ptr;
}

//>>===========================================================================

string DEF_ATTRIBUTE_GROUP_CLASS::GetMacroTextualCondition(UINT index) 

//  DESCRIPTION     : Returns the indexed textual condition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================		
{
	string textualCondition;

	if (index < macro_refsM.size())
	{
        textualCondition = macro_refsM[index].textualCondition;
	}

	// return the textual condition
	return textualCondition;
}
