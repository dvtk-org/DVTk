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
#include "Iglobal.h"			// Global component interface
#include "Ilog.h"				// Log component interface
#include "IAttributeGroup.h"	// Attribute Group component interface
#include "Icondition.h"	        // Condition component interface
#include "DefFileContent.h"
#include "AEDefinition.h"
#include "AttributeDefinition.h"
#include "AttributeGroupDefinition.h"
#include "CommandDefinition.h"
#include "DatasetDefinition.h"
#include "DICOMObjectDefinition.h"
#include "ItemDefinition.h"
#include "MacroDefinition.h"
#include "MetaSopClassDefinition.h"
#include "ModuleDefinition.h"
#include "SopClassDefinition.h"
#include "SystemDefinition.h"
#include "definition.h"


//>>===========================================================================

DEF_FILE_CONTENT_CLASS::DEF_FILE_CONTENT_CLASS()

//  DESCRIPTION     : Class Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	metaSopClassM_ptr = NULL;
}

//>>===========================================================================

DEF_FILE_CONTENT_CLASS::~DEF_FILE_CONTENT_CLASS()

//  DESCRIPTION     : Class Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (metaSopClassM_ptr)
	{
		delete metaSopClassM_ptr;
	}
	
	for (UINT i = 0; i < sopClassM.size(); i++)
	{
		if (sopClassM[i]) delete sopClassM[i];
	}
	sopClassM.clear();

	commandM.clear();
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::SetSystem(const string systemName, const string systemVersion)

//  DESCRIPTION     : Set the system name and version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set name and version
	systemNameM = systemName;
	systemVersionM = systemVersion;
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::SetAE(const string aeName, const string aeVersion)

//  DESCRIPTION     : Set the ae name and version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set name and version
	aeNameM = aeName;
	aeVersionM = aeVersion;
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::SetMetaSop(DEF_METASOPCLASS_CLASS *metaSopClass_ptr) 

//  DESCRIPTION     : Set the Meta SOP Class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	metaSopClassM_ptr = metaSopClass_ptr; 
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::AddSopClass(DEF_SOPCLASS_CLASS *sopClass_ptr) 

//  DESCRIPTION     : Add a SOP Class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	sopClassM.push_back(sopClass_ptr); 
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::AddCommand(DEF_COMMAND_CLASS *command_ptr) 

//  DESCRIPTION     : Add a Command definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	commandM.push_back(command_ptr); 
}

//>>===========================================================================

bool DEF_FILE_CONTENT_CLASS::Register()

//  DESCRIPTION     : Register the file content into the definition library and
//					: make it available for consultation.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// register the MetaSopClass
	if (metaSopClassM_ptr)
	{
		// register the meta sop class
		DEFINITION->RegisterSop(metaSopClassM_ptr->GetUid(), metaSopClassM_ptr->GetName());

		// register all the included sop classes
		for (UINT j = 0; j < metaSopClassM_ptr->GetNoSopClasses(); j++)
		{
			// register the sop classes
			string sopClassUid;
			string sopClassName;
			metaSopClassM_ptr->GetSopClass(j, sopClassUid, sopClassName);
			DEFINITION->RegisterSop(sopClassUid, sopClassName);
		}
	}

	// loop registering all the SopClasses
	for (UINT i = 0 ; i < sopClassM.size(); i++)
	{
		DEF_SOPCLASS_CLASS *sopClass_ptr = sopClassM[i];
		if (sopClass_ptr == NULL) continue;

		// register the sop class
		DEFINITION->RegisterSop(sopClass_ptr->GetUid(), sopClass_ptr->GetName());

		// register the IOD and attributes
		RegisterIod(sopClass_ptr, true);

		// register this as the last sop class used
        DEFINITION->SetLastSopUidInstalled(sopClass_ptr->GetUid());
	}

	// loop registering all the Commands
	for (UINT i = 0; i < commandM.size(); i++)
	{
		DEFINITION->RegisterCommand(commandM[i]);
	}

	// return result
	return true;
}

//>>===========================================================================

bool DEF_FILE_CONTENT_CLASS::Unregister()

//  DESCRIPTION     : Unregister the file content.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop unregistering all the SopClasses
	for (UINT i = 0 ; i < sopClassM.size(); i++)
	{
		DEF_SOPCLASS_CLASS *sopClass_ptr = sopClassM[i];
		if (sopClass_ptr == NULL) continue;

		// unregister the IOD and attributes
		RegisterIod(sopClass_ptr, false);

		// register this as the last sop class removed
		DEFINITION->SetLastSopUidRemoved(sopClass_ptr->GetUid());
	}

	// loop unregistering all the Commands
	for (UINT i = 0; i < commandM.size(); i++)
	{
		DEFINITION->UnregisterCommand(commandM[i]);
	}

	return true;
}

//>>===========================================================================

string DEF_FILE_CONTENT_CLASS::GetSystemName()

//  DESCRIPTION     : Get System Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return systemNameM;
}

//>>===========================================================================

string DEF_FILE_CONTENT_CLASS::GetSystemVersion()

//  DESCRIPTION     : Get System Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return systemVersionM;
}

//>>===========================================================================

string DEF_FILE_CONTENT_CLASS::GetAEName()

//  DESCRIPTION     : Get AE Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return aeNameM;
}

//>>===========================================================================

string DEF_FILE_CONTENT_CLASS::GetAEVersion()

//  DESCRIPTION     : Get AE Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return aeVersionM;
}

//>>===========================================================================

string DEF_FILE_CONTENT_CLASS::GetSOPClassName()

//  DESCRIPTION     : Get SOP Class Name - Meta SOP Class will overrule
//					: any underlying SOP Class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string name;

	// check if a Meta SOP Class is defined
	if (metaSopClassM_ptr)
	{
		name = metaSopClassM_ptr->GetName();
	}
	else if (sopClassM.size())
	{
		// try first SOP class
		name = sopClassM[0]->GetName();
	}

	return name;
}

//>>===========================================================================

string DEF_FILE_CONTENT_CLASS::GetSOPClassUID()

//  DESCRIPTION     : Get SOP Class UID - Meta SOP Class will overrule
//					: any underlying SOP Class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string uid;

	// check if a Meta SOP Class is defined
	if (metaSopClassM_ptr)
	{
		uid = metaSopClassM_ptr->GetUid();
	}
	else if (sopClassM[0])
	{
		// try first SOP class
		uid = sopClassM[0]->GetUid();
	}

	return uid;
}

//>>===========================================================================

bool DEF_FILE_CONTENT_CLASS::IsMetaSOPClass()

//  DESCRIPTION     : Indicate if Meta SOP Class or not.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool metaSOPClass = (metaSopClassM_ptr == NULL) ? false : true;
	return metaSOPClass;
}

//>>===========================================================================

DEF_METASOPCLASS_CLASS* DEF_FILE_CONTENT_CLASS::GetMetaSop()

//  DESCRIPTION     : Get the Meta SOP Class definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return metaSopClassM_ptr;
}

//>>===========================================================================

DEF_METASOPCLASS_CLASS* DEF_FILE_CONTENT_CLASS::GetMetaSopByUid(const string uid)

//  DESCRIPTION     : Get the Meta SOP Class definition by UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_METASOPCLASS_CLASS *metaSopClass_ptr = NULL;

	// check for matching UID
	if ((metaSopClassM_ptr) &&
		(metaSopClassM_ptr->GetUid() == uid))
	{
		metaSopClass_ptr = metaSopClassM_ptr;
	}

	return metaSopClass_ptr;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_FILE_CONTENT_CLASS::GetSopByUid(const string uid)

//  DESCRIPTION     : Get SOP Class definition by UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS *sopClass_ptr = NULL;

	for (UINT i = 0; i < sopClassM.size(); i++)
	{
		if (sopClassM[i]->GetUid() == uid)
		{
			sopClass_ptr = sopClassM[i];
			break;
		}
	}

	return sopClass_ptr;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_FILE_CONTENT_CLASS::GetFirstSop()

//  DESCRIPTION     : Get the first SOP Class definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS *sopClass_ptr = NULL;

	if (sopClassM.size())
	{
		// return first SOP Class
		sopClass_ptr = sopClassM[0];
	}

	return sopClass_ptr;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_FILE_CONTENT_CLASS::GetSopByName(const string name)

//  DESCRIPTION     : Get SOP Class definition by Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS *sopClass_ptr = NULL;

	for (UINT i = 0; i < sopClassM.size(); i++)
	{
		if (sopClassM[i]->GetName() == name)
		{
			sopClass_ptr = sopClassM[i];
			break;
		}
	}

	// we may need to map the name before searching
	if (sopClass_ptr == NULL)
	{
		string mappedName = DEFINITION->MapAdvt2DvtSopName(name.c_str());
	
		for (UINT i = 0; i < sopClassM.size(); i++)
		{
			if (sopClassM[i]->GetName() == mappedName)
			{
				sopClass_ptr = sopClassM[i];
				break;
			}
		}
	}

	return sopClass_ptr;
}
	
//>>===========================================================================

DEF_DATASET_CLASS* DEF_FILE_CONTENT_CLASS::GetDataset(const string name)

//  DESCRIPTION     : Get Dataset definition by IOD Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS *dataset_ptr = NULL;

	for (UINT i = 0; i < sopClassM.size(); i++)
	{
		dataset_ptr = sopClassM[i]->GetDataset(name);
		if (dataset_ptr)
		{
			break;
		}
	}

	return dataset_ptr;
}

//>>===========================================================================

DEF_DATASET_CLASS* DEF_FILE_CONTENT_CLASS::GetDataset(DIMSE_CMD_ENUM cmd, const string name)

//  DESCRIPTION     : Get Dataset definition by DIMSE Command and IOD Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS *dataset_ptr = NULL;

	for (UINT i = 0; i < sopClassM.size(); i++)
	{
		dataset_ptr = sopClassM[i]->GetDataset(name, cmd);
		if (dataset_ptr)
		{
			break;
		}
	}

	return dataset_ptr;
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::RegisterIod(DEF_SOPCLASS_CLASS *sopClass_ptr, bool registering)

//  DESCRIPTION     : Register the Information Object Definition and Attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < sopClass_ptr->GetNrDatasets(); i++)
	{
		// register all the IODs
		DEF_DATASET_CLASS *dataset_ptr = sopClass_ptr->GetDataset(i);
		if (dataset_ptr == NULL) continue;
		DEFINITION->RegisterIod(sopClass_ptr->GetUid(), dataset_ptr->GetName());

		// register all attributes
		// - loop through modules
		for (UINT j = 0; j < dataset_ptr->GetNrModules(); j++)
		{
			DEF_MODULE_CLASS *module_ptr = dataset_ptr->GetModule(j);
			if (module_ptr == NULL) continue;

			// check if the module has already been (un)registered
			if (module_ptr->GetContentRegistered() != registering)
			{
				for (int k = 0; k < module_ptr->GetNrAttributes(); k++)
				{
					DEF_ATTRIBUTE_CLASS *attribute_ptr = module_ptr->GetAttribute(k);
					if (attribute_ptr == NULL) continue;

					// register the attribute
					if (registering)
					{
						// registering
						DEFINITION->RegisterAttribute(attribute_ptr);
						if (IsPrivateAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement()))
						{
							DEFINITION->RegisterRecognitionCode(attribute_ptr);
						}
					}
					else
					{
						// unregistering
						DEFINITION->UnregisterAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement());
					}

					// check if the attribute is a sequence
					if (attribute_ptr->GetVR() == ATTR_VR_SQ)
					{
						// register attributes in sequence
						RegisterAttributesInSequence(attribute_ptr, registering);
					}
				}
			}

			// set module content registration flag
			module_ptr->SetContentRegistered(registering);

			// register the attributes in all the macros in the module
			for (int k = 0; k < module_ptr->GetNrMacros(); k++)
			{
				DEF_MACRO_CLASS *macro_ptr = module_ptr->GetMacro(k);
				if (macro_ptr == NULL) continue;
				
				// check if the macro has already been (un)registered
				if (macro_ptr->GetContentRegistered() != registering)
				{
					// loop over all attributes in the macro
					for (int l = 0; l < macro_ptr->GetNrAttributes(); l++)
					{
						// get attribute definition
						DEF_ATTRIBUTE_CLASS *attribute_ptr = macro_ptr->GetAttribute(l);
						if (attribute_ptr == NULL) continue;

						// register the attribute
						if (registering)
						{
							// registering
							DEFINITION->RegisterAttribute(attribute_ptr);
							if (IsPrivateAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement()))
							{
								DEFINITION->RegisterRecognitionCode(attribute_ptr);
							}
						}
						else
						{
							// unregistering
							DEFINITION->UnregisterAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement());
						}

						// check if the attribute is a sequence
						if (attribute_ptr->GetVR() == ATTR_VR_SQ)
						{
							// register attributes in sequence
							RegisterAttributesInSequence(attribute_ptr, registering);
						}
					}
				}

				// set macro content registration flag
				macro_ptr->SetContentRegistered(registering);

				// register any child macros
				RegisterMacroInMacro(macro_ptr, registering);
			}
		}
	}
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::RegisterMacroInMacro(DEF_MACRO_CLASS *parentMacro_ptr, bool registering)

//  DESCRIPTION     : Register the Attributes in nested macros.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// register the attributes in all the macros in the module
	for (int i = 0;  i < parentMacro_ptr->GetNrMacros(); i++)
	{
		DEF_MACRO_CLASS *macro_ptr = parentMacro_ptr->GetMacro(i);
		if (macro_ptr == NULL) continue;
				
		// check if the macro has already been (un)registered
		if (macro_ptr->GetContentRegistered() != registering)
		{
			// loop over all attributes in the macro
			for (int j = 0; j < macro_ptr->GetNrAttributes(); j++)
			{
				// get attribute definition
				DEF_ATTRIBUTE_CLASS *attribute_ptr = macro_ptr->GetAttribute(j);
				if (attribute_ptr == NULL) continue;

				// register the attribute
				if (registering)
				{
					// registering
					DEFINITION->RegisterAttribute(attribute_ptr);
					if (IsPrivateAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement()))
					{
						DEFINITION->RegisterRecognitionCode(attribute_ptr);
					}
				}
				else
				{
					// unregistering
					DEFINITION->UnregisterAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement());
				}

				// check if the attribute is a sequence
				if (attribute_ptr->GetVR() == ATTR_VR_SQ)
				{
					// register attributes in sequence
					RegisterAttributesInSequence(attribute_ptr, registering);
				}
			}
		}

		// set macro content registration flag
		macro_ptr->SetContentRegistered(registering);

		// register any child macros
		RegisterMacroInMacro(macro_ptr, registering);
	}
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::RegisterAttributesInSequence(DEF_ATTRIBUTE_CLASS *attribute_ptr, bool registering)

//  DESCRIPTION     : Register the Attributes in a Sequence.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (attribute_ptr == NULL) return;

	// get the Sequence value	
	VALUE_SQ_CLASS* sqValue_ptr = static_cast<VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));
	if (sqValue_ptr == NULL) return;

	// handle all Items
	for (int i = 0; i < sqValue_ptr->GetNrItems(); i++)
	{
		DEF_ITEM_CLASS *item_ptr = NULL;
		DVT_STATUS status = sqValue_ptr->Get((ATTRIBUTE_GROUP_CLASS**)&item_ptr, i);
		if ((status != MSG_OK) || (item_ptr == NULL)) continue;
			
		// register the item attributes
		if (item_ptr->GetContentRegistered() != registering)
		{
			RegisterAttributesInItem(item_ptr, registering);
		}
	}
}

//>>===========================================================================

void DEF_FILE_CONTENT_CLASS::RegisterAttributesInItem(DEF_ITEM_CLASS *item_ptr, bool registering)

//  DESCRIPTION     : Register the Attributes in an Item
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (item_ptr == NULL) return;

	// loop over all attributes in the item definition
	for (int i = 0; i < item_ptr->GetNrAttributes(); i++)
	{
		DEF_ATTRIBUTE_CLASS *attribute_ptr = item_ptr->GetAttribute(i);
		if (attribute_ptr == NULL) continue;

		// register the attribute
		if (registering)
		{
			// registering
			DEFINITION->RegisterAttribute(attribute_ptr);
			if (IsPrivateAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement()))
			{
				DEFINITION->RegisterRecognitionCode(attribute_ptr);
			}
		}
		else
		{
			// unregistering
			DEFINITION->UnregisterAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement());
		}

		// check if the attribute is a sequence
		if (attribute_ptr->GetVR() == ATTR_VR_SQ)
		{
			// register attributes in sequence
			RegisterAttributesInSequence(attribute_ptr, registering);
		}
	}

	// set item content registration flag
	item_ptr->SetContentRegistered(registering);

	// register the attributes in all the macros in the item
	for (int i = 0; i < item_ptr->GetNrMacros(); i++)
	{
		DEF_MACRO_CLASS *macro_ptr = item_ptr->GetMacro(i);
		if (macro_ptr == NULL) continue;

		// check if the macro has already been (un)registered
		if (macro_ptr->GetContentRegistered() != registering)
		{
			// loop over all attributes in the macro
			for (int j = 0; j < macro_ptr->GetNrAttributes(); j++)
			{
				// get attribute definition
				DEF_ATTRIBUTE_CLASS *attribute_ptr = macro_ptr->GetAttribute(j);
				if (attribute_ptr == NULL) continue;

				// register the attribute
				if (registering)
				{
					// registering
					DEFINITION->RegisterAttribute(attribute_ptr);
					if (IsPrivateAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement()))
					{
						DEFINITION->RegisterRecognitionCode(attribute_ptr);
					}
				}
				else
				{
					// unregistering
					DEFINITION->UnregisterAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement());
				}

				// check if the attribute is a sequence
				if (attribute_ptr->GetVR() == ATTR_VR_SQ)
				{
					// register attributes in sequence
					RegisterAttributesInSequence(attribute_ptr, registering);
				}
			}
		}

		// set macro content registration flag
		macro_ptr->SetContentRegistered(registering);

		// register any child macros
		RegisterMacroInMacro(macro_ptr, registering);
	}
}

//>>===========================================================================

bool DEF_FILE_CONTENT_CLASS::IsPrivateAttribute(UINT16 group, UINT16 element)

//  DESCRIPTION     : Test if the given Tag is a Private one.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    bool result = false;

    // check for private attributes
	if (((group & 0x0001) != 0) && 
		((element >= 0x0010 && element <= 0x00FF)))
	{
	    result = true;
	}

	return result;
}
