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
#include "val_object_results.h"
#include "val_attribute_group.h"
#include "val_attribute.h"

#include "Iglobal.h"        // Global interface file
#include "Idicom.h"         // Dicom component interface file
#include "Ilog.h"           // Log component interface file

//>>===========================================================================

VAL_OBJECT_RESULTS_CLASS::VAL_OBJECT_RESULTS_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    messagesM_ptr = NULL;
    additionalAttributesM_ptr = new VAL_ATTRIBUTE_GROUP_CLASS();
    hasReferenceObjectM = false;
    commandM_ptr = NULL;
	fileMetaInfoM_ptr = NULL;
}

//>>===========================================================================

VAL_OBJECT_RESULTS_CLASS::~VAL_OBJECT_RESULTS_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    delete messagesM_ptr;

    for (UINT i = 0; i < modulesM.size(); i++)
    {
        delete modulesM[i];
    }
    modulesM.clear();

    delete additionalAttributesM_ptr;
}

//>>===========================================================================

void VAL_OBJECT_RESULTS_CLASS::CleanUp()

//  DESCRIPTION     : Initialize the object results to the original state.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    delete messagesM_ptr;
    messagesM_ptr = NULL;

    for (UINT i = 0; i < modulesM.size(); i++)
    {
        delete modulesM[i];
    }
    modulesM.clear();

    delete additionalAttributesM_ptr;
    additionalAttributesM_ptr = new VAL_ATTRIBUTE_GROUP_CLASS();

    hasReferenceObjectM = false;
    commandM_ptr = NULL;
	fileMetaInfoM_ptr = NULL;
}

//>>===========================================================================

DVT_STATUS VAL_OBJECT_RESULTS_CLASS::AddModuleResults(VAL_ATTRIBUTE_GROUP_CLASS *valModule_ptr)

//  DESCRIPTION     : Add module results to this object results.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    modulesM.push_back(valModule_ptr);

    return MSG_OK;
}

//>>===========================================================================

int VAL_OBJECT_RESULTS_CLASS::GetNrModuleResults()

//  DESCRIPTION     : Get the number of module results in this object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return modulesM.size();
}

//>>===========================================================================

VAL_ATTRIBUTE_GROUP_CLASS *VAL_OBJECT_RESULTS_CLASS::GetModuleResults(int index)

//  DESCRIPTION     : Get the index module results from this object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	assert (index >= 0);

    VAL_ATTRIBUTE_GROUP_CLASS *valModule_ptr = NULL;
    if (modulesM.size() > (unsigned int) index)
    {
        valModule_ptr = modulesM[index];
    }
    return valModule_ptr;
}

//>>===========================================================================

VAL_ATTRIBUTE_CLASS *VAL_OBJECT_RESULTS_CLASS::GetAttributeResults(UINT16 group, UINT16 element)

//  DESCRIPTION     : This function returns the validation attribute with the
//                    group and element specified. If the requested attribute
//                    is not found, NULL will be returned.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (UINT i = 0; i < modulesM.size(); i++)
    {
        VAL_ATTRIBUTE_CLASS *valAttr_ptr = modulesM[i]->GetAttribute(group, element);
        if (valAttr_ptr != NULL)
        {
            return valAttr_ptr;
        }
    }

    // The attribute is not found in the defined module results, maybe the
    // attribute is defined in the additional attribute group.
    return additionalAttributesM_ptr->GetAttribute(group, element);
}

//>>===========================================================================

VAL_ATTRIBUTE_CLASS *VAL_OBJECT_RESULTS_CLASS::GetAttributeResults(UINT32 tag)

//  DESCRIPTION     : This function returns the validation attribute with the
//                    group and element specified. If the requested attribute
//                    is not found, NULL will be returned.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return GetAttributeResults((UINT16)((tag & 0xFFFF0000) >> 16), (UINT16)(tag & 0x0000FFFF));
}

//>>===========================================================================

string VAL_OBJECT_RESULTS_CLASS::GetSpecificCharacterSetValues()

//  DESCRIPTION     : This function returns the concatenated values of the
//					: Specific Character Set attribute (0008,0005) if present.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	string specificCharacterSetValues = "";

	VAL_ATTRIBUTE_CLASS* valAttribute_ptr = this->GetAttributeResults(0x00080005);
	if (valAttribute_ptr != NULL)
	{
		DCM_ATTRIBUTE_CLASS* dcmAttribute_ptr = valAttribute_ptr->GetDcmAttribute();
		if ((dcmAttribute_ptr != NULL) &&
			(dcmAttribute_ptr->GetVR() == ATTR_VR_CS))
		{
			for (int i = 0; i < dcmAttribute_ptr->GetNrValues(); i++)
			{
				BASE_VALUE_CLASS *value_ptr = (BASE_VALUE_CLASS*)dcmAttribute_ptr->GetValue(i);
				string csValue;
				if (value_ptr->Get(csValue) == MSG_OK)
				{
					specificCharacterSetValues += csValue;
					if ((i + 1) < dcmAttribute_ptr->GetNrValues())
					{
						specificCharacterSetValues += "\\";
					}
				}
			}
		}
	}

	return specificCharacterSetValues;
}

//>>===========================================================================

bool VAL_OBJECT_RESULTS_CLASS::GetListOfAttributeResults(UINT16 group,
                                                     UINT16 element,
                                                     vector <VAL_ATTRIBUTE_CLASS*> *valAttrList_ptr)

//  DESCRIPTION     : This function returns a list of validation attributes
//                    with the specified group and element. If the requested
//                    attribute is not found, 'false' will be the return value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    bool valAttrFound = false;
    VAL_ATTRIBUTE_CLASS *valAttr_ptr;

    for (UINT i = 0; i < modulesM.size(); i++)
    {
        valAttr_ptr = modulesM[i]->GetAttribute(group, element);
        if (valAttr_ptr != NULL)
        {
            valAttrList_ptr->push_back(valAttr_ptr);
            valAttrFound = true;
        }
    }

    valAttr_ptr = additionalAttributesM_ptr->GetAttribute(group, element);
    if (valAttr_ptr != NULL)
    {
        valAttrList_ptr->push_back(valAttr_ptr);
        valAttrFound = true;
    }

    return valAttrFound;
}

//>>===========================================================================

VAL_ATTRIBUTE_GROUP_CLASS *VAL_OBJECT_RESULTS_CLASS::GetAGWithAttributeInGroup(UINT16 group)

//  DESCRIPTION     : This function returns true if the attribute group has an
//                    attribute in the requested group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    UINT i = 0;
    while (i < modulesM.size())
    {
        int j = 0;
        while (j < modulesM[i]->GetNrAttributes())
        {
            DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = modulesM[i]->GetAttribute(j)->GetDcmAttribute();
            if (dcmAttr_ptr != NULL)
            {
                if (dcmAttr_ptr->GetMappedGroup() == group)
                {
                    return modulesM[i];
                }
            }
            j++;
        }
        i++;
    }

    // The attribute is not found in the defined module results, maybe the
    // attribute is defined in the additional attribute group.
    int k = 0;
    while (k < additionalAttributesM_ptr->GetNrAttributes())
    {
        DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = additionalAttributesM_ptr->GetAttribute(k)->GetDcmAttribute();
        if (dcmAttr_ptr != NULL)
        {
            if (dcmAttr_ptr->GetMappedGroup() == group)
            {
                return additionalAttributesM_ptr;
            }
        }
        k++;
    }

    return NULL;
}

//>>===========================================================================

VAL_ATTRIBUTE_GROUP_CLASS *VAL_OBJECT_RESULTS_CLASS::GetAdditionalAttributeGroup()

//  DESCRIPTION     : Get the additional attribute group.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return additionalAttributesM_ptr;
}

//>>===========================================================================

DVT_STATUS VAL_OBJECT_RESULTS_CLASS::ValidateAgainstDef(UINT32 flags)

//  DESCRIPTION     : Validate the object against the definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Validate standard modules against the definition
    for (UINT i = 0; i < modulesM.size(); i++)
    {
		VAL_ATTRIBUTE_GROUP_CLASS *module_ptr = modulesM[i];
		if (module_ptr->GetIgnoreThisAttributeGroup() == false)
		{
			module_ptr->ValidateAgainstDef(flags);
		}
    }

    // Validate any additional attributes against the definition
    // - flag that additional attributes are allowed during further
    // validation
    if (additionalAttributesM_ptr)
    {
        additionalAttributesM_ptr->ValidateAgainstDef(flags | ATTR_FLAG_ADDITIONAL_ATTRIBUTE);
    }

    return MSG_OK;
}

//>>===========================================================================

DVT_STATUS VAL_OBJECT_RESULTS_CLASS::ValidateAgainstRef(UINT32 flags)

//  DESCRIPTION     : Validate the object against the reference.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (hasReferenceObjectM)
    {
        // Validate standard modules against the reference
        for (UINT i = 0; i < modulesM.size(); i++)
        {
			VAL_ATTRIBUTE_GROUP_CLASS *module_ptr = modulesM[i];
			if (module_ptr->GetIgnoreThisAttributeGroup() == false)
			{
				module_ptr->ValidateAgainstRef(flags);
			}
        }

        // Validate any additional attributes against the reference
        if (additionalAttributesM_ptr)
        {
            additionalAttributesM_ptr->ValidateAgainstRef(flags | ATTR_FLAG_ADDITIONAL_ATTRIBUTE);
        }
    }

    return MSG_OK;
}

//>>===========================================================================

DVT_STATUS VAL_OBJECT_RESULTS_CLASS::ValidateVR(UINT32 flags, SPECIFIC_CHARACTER_SET_CLASS *specificCharacterSet_ptr)

//  DESCRIPTION     : Validate the VR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // Validate the standard modules against the VR
    for (UINT i = 0; i < modulesM.size(); i++)
    {
		VAL_ATTRIBUTE_GROUP_CLASS *module_ptr = modulesM[i];
		if (module_ptr->GetIgnoreThisAttributeGroup() == false)
		{
			module_ptr->ValidateVR(flags, specificCharacterSet_ptr);
		}
    }

    // Validate any additional attributes against the VR
    if (additionalAttributesM_ptr)
    {
        additionalAttributesM_ptr->ValidateVR(flags | ATTR_FLAG_ADDITIONAL_ATTRIBUTE, specificCharacterSet_ptr);
    }

    return MSG_OK;
}

//>>===========================================================================

void VAL_OBJECT_RESULTS_CLASS::SetName(string name)

//  DESCRIPTION     : Set the object name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    nameM = name;
}

//>>===========================================================================

string VAL_OBJECT_RESULTS_CLASS::GetName()

//  DESCRIPTION     : Get the object name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return nameM;
}

//>>===========================================================================

void VAL_OBJECT_RESULTS_CLASS::SetDICOMDIRName(string name)

//  DESCRIPTION     : Set the object name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    dicomdirNameM = name;
}

//>>===========================================================================

string VAL_OBJECT_RESULTS_CLASS::GetDICOMDIRName()

//  DESCRIPTION     : Get the object name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return dicomdirNameM;
}

//>>===========================================================================

void VAL_OBJECT_RESULTS_CLASS::SetCommand(DCM_COMMAND_CLASS *command_ptr)

//  DESCRIPTION     : Set the object command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    commandM_ptr = command_ptr;
}

//>>===========================================================================

DCM_COMMAND_CLASS *VAL_OBJECT_RESULTS_CLASS::GetCommand()

//  DESCRIPTION     : Get the object command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return commandM_ptr;
}

//>>===========================================================================

void VAL_OBJECT_RESULTS_CLASS::SetFmi(DCM_ATTRIBUTE_GROUP_CLASS *fmi_ptr)

//  DESCRIPTION     : Set the object fmi.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    fileMetaInfoM_ptr = fmi_ptr;
}

//>>===========================================================================

DCM_ATTRIBUTE_GROUP_CLASS *VAL_OBJECT_RESULTS_CLASS::GetFmi()

//  DESCRIPTION     : Get the object fmi.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return fileMetaInfoM_ptr;
}

//>>===========================================================================

LOG_MESSAGE_CLASS *VAL_OBJECT_RESULTS_CLASS::GetMessages()

//  DESCRIPTION     : Get the log messages.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (messagesM_ptr == NULL)
    {
        messagesM_ptr = new LOG_MESSAGE_CLASS();
    }
    return messagesM_ptr;
}

//>>===========================================================================

bool VAL_OBJECT_RESULTS_CLASS::HasMessages()

//  DESCRIPTION     : Check if the object has log messages.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return (messagesM_ptr == NULL) ? false : true;
}

//>>===========================================================================

void VAL_OBJECT_RESULTS_CLASS::HasReferenceObject(bool hasRefObject)

//  DESCRIPTION     : Set the has reference flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    hasReferenceObjectM = hasRefObject;
}
