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
#include "DICOMObjectDefinition.h"
#include "ModuleDefinition.h"
#include "AttributeDefinition.h"


//>>===========================================================================

DEF_DICOM_OBJECT_CLASS::DEF_DICOM_OBJECT_CLASS()

//  DESCRIPTION     : Default constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

DEF_DICOM_OBJECT_CLASS::DEF_DICOM_OBJECT_CLASS(const string name)

//  DESCRIPTION     : Constructor with name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	nameM = name;
}

//>>===========================================================================

DEF_DICOM_OBJECT_CLASS::~DEF_DICOM_OBJECT_CLASS()

//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	for (UINT i = 0; i < modulesM.size(); i++)
	{
		delete modulesM[i];
	}
	modulesM.clear();
}

//>>===========================================================================

void DEF_DICOM_OBJECT_CLASS::AddModule(DEF_MODULE_CLASS* mod_ptr)

//  DESCRIPTION     : Adds module to DICOM Object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	modulesM.push_back(mod_ptr);
}

//>>===========================================================================

DEF_MODULE_CLASS* DEF_DICOM_OBJECT_CLASS::GetModule(UINT index)

//  DESCRIPTION     : Returns indexed module.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_MODULE_CLASS* module_ptr = NULL;

	// check index in range
	if (index < modulesM.size())
	{
		module_ptr = modulesM[index];
	}

	// return module pointer
	return module_ptr;
}

//>>===========================================================================

DEF_MODULE_CLASS* DEF_DICOM_OBJECT_CLASS::GetModule(UINT16 group, UINT16 element)

//  DESCRIPTION     : Searches for a module containing the requested attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_MODULE_CLASS* module_ptr = NULL;

	// seach through all modules
	for (UINT i = 0; i < modulesM.size(); i++)
	{
		module_ptr = modulesM[i];
		if (module_ptr == NULL) continue;

		// check if attribute is in this module
		if (module_ptr->GetAttribute(group, element)) break;
	}

    // return module pointer
	return module_ptr;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS* DEF_DICOM_OBJECT_CLASS::GetAttribute(UINT16 group, UINT16 element)

//  DESCRIPTION     : Searches for an attribute in the object definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_ATTRIBUTE_CLASS* attribute_ptr = NULL;

	// seach through all modules
	for (UINT i = 0; i < modulesM.size(); i++)
	{
		DEF_MODULE_CLASS *module_ptr = modulesM[i];
		if (module_ptr == NULL) continue;

		attribute_ptr = module_ptr->GetAttribute(group, element);
		if (attribute_ptr) break;
	}
    
	// return attribute pointer
	return attribute_ptr;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS* DEF_DICOM_OBJECT_CLASS::GetAttribute(string moduleName, UINT16 group, UINT16 element)

//  DESCRIPTION     : Searches for an attribute in the object definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_ATTRIBUTE_CLASS *attribute_ptr = NULL;

	// seach through all modules
	for (UINT i = 0; i < modulesM.size(); i++)
	{
		DEF_MODULE_CLASS *module_ptr = modulesM[i];
		if (module_ptr == NULL) continue;

		// check if module name matches
		if (module_ptr->GetName() == moduleName)
		{
			attribute_ptr = module_ptr->GetAttribute(group, element);
			if (attribute_ptr) break;
		}
	}

	// return attribute pointer
	return attribute_ptr;
}

void DEF_DICOM_OBJECT_CLASS::DetectDuplicate()

//  DESCRIPTION     : Detect duplicated attribute of conditional module.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < GetNrModules(); i++) 
	{
		DEF_MODULE_CLASS* module_ptr = GetModule(i);
		if ((module_ptr->GetUsage() == MOD_USAGE_C) || (module_ptr->GetUsage() == MOD_USAGE_U) ) 
		{
			for (UINT j = 0; j < module_ptr->GetNrAttributes(); j++) 
			{
				DEF_ATTRIBUTE_CLASS* attribute_ptr = module_ptr->GetAttribute(j);
				for (UINT k = 0; k < GetNrModules(); k++) 
				{
					if (k != i)
					{
						if (GetModule(k)->GetAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement()))
						{
							attribute_ptr->SetDuplicate(true);
						}
					}
				}
			}
		}
	}
}