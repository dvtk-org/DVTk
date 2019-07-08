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
//  DESCRIPTION     :   Contains all classes for DICOM definitions
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "definition.h"

#include "AEDefinition.h"
#include "AESession.h"
#include "AttributeDefinition.h"
#include "AttributeRegister.h"
#include "CommandDefinition.h"
#include "DatasetDefinition.h"
#include "definitionfile.h"
#include "ItemDefinition.h"
#include "MacroDefinition.h"
#include "MetaSopClassDefinition.h"
#include "ModuleDefinition.h"
#include "SopClassDefinition.h"
#include "SystemDefinition.h"
#include "DefFileContent.h"
#include "DefinitionDetails.h"
#include "Idicom.h"


#define DIM_OF(a) (sizeof(a) / sizeof(a[0]))

static T_ADVT2DVT_NAME_MAP TAdvt2DvtSopMap[] =
{ 
{"Storage Commitment Push SOP Class",						"Storage Commitment Push Model SOP Class"},
{"Storage Commitment Pull SOP Class",						"Storage Commitment Pull Model SOP Class"},
{"Media Storage Directory Storage SOP Class",				"Media Storage Directory SOP Class"},
{"Modality PPS SOP Class",									"Modality Performed Procedure Step SOP Class"},
{"Modality Retrieve PPS SOP Class",							"Modality Performed Procedure Step Retrieve SOP Class"},
{"Modality PPS Notification SOP Class",						"Modality Performed Procedure Step Notification SOP Class"},
{"CR Image Storage SOP Class",								"Computed Radiography Image Storage SOP Class"},
{"DX Image Storage SOP Class - For Presentation",			"Digital X-Ray Image Storage - For Pres. SOP"},
{"DX Image Storage SOP Class - For Processing",				"Digital X-Ray Image Storage - For Proc. SOP"},
{"MG Image Storage SOP Class - For Presentation",			"Digital Mammography X-Ray Image Storage - Pres. SOP"},
{"MG Image Storage SOP Class - For Processing",				"Digital Mammography X-Ray Image Storage - Proc. SOP"},
{"IO Image Storage SOP Class - For Presentation",			"Digital Intra-oral X-Ray Image Storage - Pres. SOP"},
{"IO Image Storage SOP Class - For Processing",				"Digital Intra-oral X-Ray Image Storage - Proc. SOP"},
{"Grayscale Softcopy Presentation State Storage SOP Class", "Softcopy Presentation State Storage SOP Class"},
{"XA Image Storage SOP Class",								"X-Ray Angiographic Image Storage SOP Class"},
{"XRF Image Storage SOP Class",								"X-Ray Radiofluoroscopic Image Storage SOP Class"},
{"XA Bi-Plane Image Storage SOP Class",						"X-Ray Angiographic Bi-Plane Image Storage SOP Class (Retired)"},
{"PET Image Storage SOP Class",								"Positron Emission Tomography Image Storage SOP Class"},
{"NM Image Storage SOP Class",								"Nuclear Medicine Image Storage SOP Class"},
{"SC Image Storage SOP Class",								"Secondary Capture Image Storage SOP Class"},
{"VL Storage SOP Class",									"VL Endoscopic Image Storage SOP Class"},
{"VL Microscopic Storage SOP Class",						"VL Microscopic Image Storage SOP Class"},
{"VL Slide-Coordinates Microscopic Storage SOP Class",		"VL Slide-Coordinates Microscopic Image Storage SOP Class"},
{"Waveform ECG Storage SOP Class",							"12-Lead ECG Waveform Storage SOP Class"},
{"Waveform Audio Storage SOP Class",						"General ECG Waveform Storage SOP Class"},
{"Patient Root QR FIND SOP Class",							"Patient Root Query/Retrieve Information Model - FIND SOP Class"},
{"Patient Root QR MOVE SOP Class",							"Patient Root Query/Retrieve Information Model - MOVE SOP Class"},
{"Patient Root QR GET SOP Class",						    "Patient Root Query/Retrieve Information Model - GET SOP Class"},
{"Study Root QR FIND SOP Class",							"Study Root Query/Retrieve Information Model - FIND SOP Class"},
{"Study Root QR MOVE SOP Class",							"Study Root Query/Retrieve Information Model - MOVE SOP Class"},
{"Study Root QR GET SOP Class",								"Study Root Query/Retrieve Information Model - GET SOP Class"},
{"Patient Study Only QR FIND SOP Class",					"Patient/Study Only Query/Retrieve Info. Model - FIND SOP Class"},
{"Patient Study Only QR MOVE SOP Class",					"Patient/Study Only Query/Retrieve Info. Model - MOVE SOP Class"},
{"Patient Study Only QR GET SOP Class"	,					"Patient/Study Only Query/Retrieve Info. Model - GET SOP Class"},
{"Modality Worklist FIND SOP Class",						"Modality Worklist Information Model - FIND SOP Class"},
{"DICOM File Meta Information",								"File Meta Information SOP Class"}
};


static T_ADVT2DVT_NAME_MAP TAdvt2DvtIodMap[] =
{ 
{"Storage Commitment Push",										"Commitment Push"},  
{"Storage Commitment Push - SUCCESS",							"Commitment Push"},  
{"Storage Commitment Push - FAILURE",							"Commitment Push"},  
{"Storage Commitment Pull",										"Commitment Pull"},  
{"Storage Commitment Pull - SUCCESS",							"Commitment Pull"},  
{"Storage Commitment Pull - FAILURE",							"Commitment Pull"},  
{"Basic Directory IOD",											"Media Directory"},  
{"Detached Patient - CREATED",									"Detached Patient"}, 
{"Detached Patient - DELETED",									"Detached Patient"}, 
{"Detached Patient - UPDATED",									"Detached Patient"}, 
{"Detached Visit - CREATED",									"Detached Visit"}, 
{"Detached Visit - SCHEDULED",									"Detached Visit"}, 
{"Detached Visit - PATIENT ADMITTED",							"Detached Visit"}, 
{"Detached Visit - PATIENT TRANSFERRED",						"Detached Visit"}, 
{"Detached Visit - PATIENT DISCHARGED",							"Detached Visit"}, 
{"Detached Visit - DELETED",									"Detached Visit"}, 
{"Detached Visit - UPDATED",									"Detached Visit"}, 
{"Detached Study - CREATED",									"Detached Study"}, 
{"Detached Study - SCHEDULED",									"Detached Study"}, 
{"Detached Study - PATIENT ARRIVED",							"Detached Study"}, 
{"Detached Study - STARTED",									"Detached Study"}, 
{"Detached Study - COMPLETED",									"Detached Study"}, 
{"Detached Study - VERIFIED",									"Detached Study"}, 
{"Detached Study - READ",										"Detached Study"}, 
{"Detached Study - UPDATED",									"Detached Study"}, 
{"Modality PPS",												"Modality Performed Procedure Step"}, 
{"Modality Retrieve PPS",										"Modality Performed Procedure Step Retrieve"}, 
{"Detached Results - CREATED",									"Detached Results"}, 
{"Detached Results - DELETED",									"Detached Results"}, 
{"Detached Results - UPDATED",									"Detached Results"}, 
{"Detached Interpretation - CREATED",							"Detached Interpretation"}, 
{"Detached Interpretation - RECORDED",							"Detached Interpretation"}, 
{"Detached Interpretation - TRANSCRIBED",						"Detached Interpretation"}, 
{"Detached Interpretation - APPROVED",							"Detached Interpretation"}, 
{"Detached Interpretation - UPDATED",							"Detached Interpretation"}, 
{"CR Image",													"Computed Radiography Image"}, 
{"DX Image - For Presentation",									"Digital X-Ray Image - For Pres."}, 
{"DX Image - For Processing",									"Digital X-Ray Image - For Proc."}, 
{"MG Image - For Presentation",									"Digital Mammography X-Ray Image - Pres."}, 
{"MG Image - For Processing",									"Digital Mammography X-Ray Image - Proc."}, 
{"IO Image - For Presentation",									"Digital Intra-oral X-Ray Image - Pres."}, 
{"IO Image - For Processing",									"Digital Intra-oral X-Ray Image - Proc."}, 
{"Grayscale Softcopy Presentation State Storage",				"Softcopy Presentation State"}, 
{"XA Image",													"X-Ray Angiographic Image"}, 
{"XRF Image",													"X-Ray Radiofluoroscopic Image"}, 
{"XA Bi-Plane Image",											"X-Ray Angiographic Bi-Plane Image (Retired)"}, 
{"PET Image",													"Positron Emission Tomography Image"}, 
{"NM Image",													"Nuclear Medicine Image"}, 
{"US Multi-frame Image",										"Ultrasound Multi-frame Image"}, 
{"RT Beams",													"RT Beams Treatment Record"}, 
{"US Image",													"Ultrasound Image"}, 
{"SC Image",													"Secondary Capture Image"}, 
{"VL Image",													"VL Endoscopic Image"}, 
{"VL Slide Microscopic Image",									"VL Slide-Coordinates Microscopic Image"}, 
{"VL Photo Image",												"VL Photographic Image"}, 
{"Wave ECG Image",												"12-Lead ECG Waveform"}, 
{"Wave Audio Image",											"General ECG Waveform"}, 
{"Patient Root QR FIND - PATIENT",								"Patient Root Query/Retrieve - FIND"}, 
{"Patient Root QR FIND - STUDY",								"Patient Root Query/Retrieve - FIND"}, 
{"Patient Root QR FIND - SERIES",								"Patient Root Query/Retrieve - FIND"}, 
{"Patient Root QR FIND - IMAGE",								"Patient Root Query/Retrieve - FIND"}, 
{"Patient Root QR MOVE - PATIENT",								"Patient Root Query/Retrieve - MOVE"}, 
{"Patient Root QR MOVE - STUDY",								"Patient Root Query/Retrieve - MOVE"}, 
{"Patient Root QR MOVE - SERIES",								"Patient Root Query/Retrieve - MOVE"}, 
{"Patient Root QR MOVE - IMAGE",								"Patient Root Query/Retrieve - MOVE"}, 
{"Patient Root QR GET - PATIENT",								"Patient Root Query/Retrieve - GET"}, 
{"Patient Root QR GET - STUDY",									"Patient Root Query/Retrieve - GET"}, 
{"Patient Root QR GET - SERIES",								"Patient Root Query/Retrieve - GET"}, 
{"Patient Root QR GET - IMAGE",									"Patient Root Query/Retrieve - GET"}, 
{"Study Root QR FIND - STUDY",									"Study Root Query/Retrieve - FIND"}, 
{"Study Root QR FIND - SERIES",									"Study Root Query/Retrieve - FIND"}, 
{"Study Root QR FIND - IMAGE",									"Study Root Query/Retrieve - FIND"}, 
{"Study Root QR MOVE - STUDY",									"Study Root Query/Retrieve - MOVE"}, 
{"Study Root QR MOVE - SERIES",									"Study Root Query/Retrieve - MOVE"}, 
{"Study Root QR MOVE - IMAGE",									"Study Root Query/Retrieve - MOVE"}, 
{"Study Root QR GET - STUDY",									"Study Root Query/Retrieve - GET"}, 
{"Study Root QR GET - SERIES",									"Study Root Query/Retrieve - GET"}, 
{"Study Root QR GET - IMAGE",									"Study Root Query/Retrieve - GET"}, 
{"Patient Study Only QR FIND - PATIENT",						"Patient/Study Only Query/Retrieve - FIND"}, 
{"Patient Study Only QR FIND - STUDY",							"Patient/Study Only Query/Retrieve - FIND"}, 
{"Patient Study Only QR MOVE - PATIENT",						"Patient/Study Only Query/Retrieve - MOVE"}, 
{"Patient Study Only QR MOVE - STUDY",							"Patient/Study Only Query/Retrieve - MOVE"}, 
{"Patient Study Only QR GET - PATIENT",							"Patient/Study Only Query/Retrieve - GET"}, 
{"Patient Study Only QR GET - STUDY",							"Patient/Study Only Query/Retrieve - GET"}, 
{"Modality Worklist FIND",										"Modality Worklist - FIND"},
{"Print Job - PENDING",											"Print Job"},
{"Print Job - PRINTING",										"Print Job"},
{"Print Job - DONE",											"Print Job"},
{"Print Job - FAILURE",											"Print Job"},
{"Printer - WARNING",											"Printer"},
{"Printer - FAILURE",											"Printer"},
{"DICOM File Meta Information",									"File Meta"} 
};


// initialise static pointer
DEF_DEFINITION_CLASS* DEF_DEFINITION_CLASS::instanceM_ptr = NULL;


//>>===========================================================================

DEF_DEFINITION_CLASS::DEF_DEFINITION_CLASS()

//  DESCRIPTION     : Class Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise class elements
	Initialise(); 
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::Initialise()

//  DESCRIPTION     : Method to initialise the Definition Class
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// create default system and AE
    DEF_SYSTEM_CLASS* system_ptr = new DEF_SYSTEM_CLASS(DEFAULT_SYSTEM_NAME, DEFAULT_SYSTEM_VERSION);
	DEF_AE_CLASS* ae_ptr = new DEF_AE_CLASS(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
	system_ptr->AddAE(ae_ptr);
	AddSystem(system_ptr);
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::Cleanup()

//  DESCRIPTION     : Cleanup the Definition Class
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup the data
	// system data
	for (UINT i = 0; i < systemsM.size(); i++)
	{
		delete systemsM[i];
	}
	systemsM.clear();

	def_filesM.clear();

	// SOP register
	sop_registerM.clear();

	// IOD register
	iod_registerM.clear();

	// reconition code table
	recognition_code_registerM.clear();

	// command register
	commandsM.clear();

	// attribute register
	attribute_registerM.clear();
}

//>>===========================================================================

DEF_DEFINITION_CLASS* DEF_DEFINITION_CLASS::Instance()

//  DESCRIPTION     : Singleton Instance - return pointer to single class
//                    instance
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) 
	{
		instanceM_ptr = new DEF_DEFINITION_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::Install(const string filename, DEF_FILE_CONTENT_CLASS *fileContent_ptr)

//  DESCRIPTION     : Install the defintition from the file content.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (fileContent_ptr == NULL) return false;

	// run through the file content installing it
	// check if System is defined
	DEF_SYSTEM_CLASS *system_ptr = GetSystem(fileContent_ptr->GetSystemName(), fileContent_ptr->GetSystemVersion());
	if (system_ptr == NULL)
	{
		// add this System
		system_ptr = new DEF_SYSTEM_CLASS(fileContent_ptr->GetSystemName(), fileContent_ptr->GetSystemVersion());
		AddSystem(system_ptr);
	}

	// check if AE is defined
	DEF_AE_CLASS *ae_ptr = GetAe(fileContent_ptr->GetAEName(), fileContent_ptr->GetAEVersion());
	if (ae_ptr == NULL)
	{
		// add this AE
		ae_ptr = new DEF_AE_CLASS(fileContent_ptr->GetAEName(), fileContent_ptr->GetAEVersion());
		system_ptr->AddAE(ae_ptr);
	}

	// install the instance
	bool success = ae_ptr->AddInstance(filename, fileContent_ptr);

	return success;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::Uninstall(const string filename)

//  DESCRIPTION     : Uninstall the definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool success = false;

	// find the AE containing an instance with this filename
	DEF_AE_CLASS *ae_ptr = GetAe(filename);
	if (ae_ptr != NULL)
	{
		// uninstall this instance
		success = ae_ptr->RemoveInstance(filename);
	}

	return success;
}
//>>===========================================================================

void DEF_DEFINITION_CLASS::RegisterSop(const string uid, const string name)

//  DESCRIPTION     : Registers sop uid and name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    sop_registerM[uid] = name;
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::RegisterIod(const string uid, const string name)

//  DESCRIPTION     : Registers a sop uid and the corresponding iod
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function should be obsolete. It is only required for 
//                    backward compatibility with VTS scripts. In some scripts 
//                    of the VTS testspecs for setting an UID the alias is used
//                    As aliases are no longer sop classes but IOD names we
//                    may need to lookup the corresponding uid via this IOD name
//<<===========================================================================
{
    iod_registerM[uid] = name;
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::RegisterCommand(DEF_COMMAND_CLASS* cmd_ptr)

//  DESCRIPTION     : Register a command
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (cmd_ptr == NULL) return;

	// register the attributes in the command
	for (UINT i = 0; i < cmd_ptr->GetNrModules(); i++)
	{
		DEF_MODULE_CLASS *module_ptr = cmd_ptr->GetModule(i);
		if (module_ptr)
		{
			for (int j = 0; j < module_ptr->GetNrAttributes(); j++)
			{
				// register the attribute
				RegisterAttribute(module_ptr->GetAttribute(j));
			}
		}
	}

	// save command itself
	commandsM.push_back(cmd_ptr);
}

//>>===========================================================================

void  DEF_DEFINITION_CLASS::RegisterAttribute(DEF_ATTRIBUTE_CLASS* attr_ptr)

//  DESCRIPTION     : Registers attribute by tag
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool found = false;

	if (attr_ptr == NULL) return;

	// check whether the attribute already has been registered
	for (UINT i = 0; i < attribute_registerM.size(); i++)
	{
		DEF_ATTRIBUTE_CLASS *def_attr_ptr = attribute_registerM[i]->GetReferenceAttribute();
		if ((def_attr_ptr->GetGroup() == attr_ptr->GetGroup()) && 
			(def_attr_ptr->GetElement() == attr_ptr->GetElement()))
		{
			// attribute has already been registered
			// increment reference
			attribute_registerM[i]->IncrementReferenceCount();
			found = true;
			break;
		}
    }

	// if not found add to register
	if (!found)
	{
		DEF_ATTRIBUTE_REGISTER_CLASS *attr_reg_ptr = new DEF_ATTRIBUTE_REGISTER_CLASS(attr_ptr);
        attribute_registerM.push_back(attr_reg_ptr);
	}
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::RegisterRecognitionCode(DEF_ATTRIBUTE_CLASS* attr_ptr)

//  DESCRIPTION     : Registers Private Recognition Codes
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// assume the recognition code is the first value
	if (attr_ptr->GetNrValues() > 0)
	{
		BASE_VALUE_CLASS *value_ptr = attr_ptr->GetValue(0);
		if (value_ptr)
		{
			string value;
			if (value_ptr->Get(value) == MSG_OK)
			{
				RECOGNITION_CODE_STRUCT rec_code_pair;

				rec_code_pair.code = value;
				rec_code_pair.tag = attr_ptr->GetTag();
				recognition_code_registerM.push_back(rec_code_pair);
			}
		}
	}
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::UnregisterCommand(DEF_COMMAND_CLASS* cmd_ptr)

//  DESCRIPTION     : Unregister a command
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through the register - use the address to unregister
	DEF_COMMAND_REGISTER::iterator it = commandsM.begin();

	for (UINT i = 0; i < commandsM.size(); i++, it++)
	{
		if (commandsM[i] == cmd_ptr)
		{
			// unregister the attributes in the command
			for (UINT i = 0; i < cmd_ptr->GetNrModules(); i++)
			{
				DEF_MODULE_CLASS *module_ptr = cmd_ptr->GetModule(i);
				if (module_ptr)
				{
					for (int j = 0; j < module_ptr->GetNrAttributes(); j++)
					{
						DEF_ATTRIBUTE_CLASS *attribute_ptr = module_ptr->GetAttribute(j);
						if (attribute_ptr)
						{
							// unregister the attribute
							UnregisterAttribute(attribute_ptr->GetGroup(), attribute_ptr->GetElement());
						}
					}
				}
			}

			// remove the entry
			commandsM.erase(it);
			break;
		}
    }
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::UnregisterAttribute(const UINT group, const UINT element)

//  DESCRIPTION     : Unregisters an attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_ATTRIBUTE_REGISTER::iterator it = attribute_registerM.begin();

	for (UINT i = 0; i < attribute_registerM.size(); i++, it++)
	{
		DEF_ATTRIBUTE_REGISTER_CLASS *register_ptr = attribute_registerM[i];
		if ((register_ptr) &&
			(register_ptr->GetReferenceAttribute()->GetGroup() == group) && 
			(register_ptr->GetReferenceAttribute()->GetElement() == element))
		{
            // decrement the reference count
			register_ptr->DecrementReferenceCount();

			// check if register entry should be removed
			if (register_ptr->GetReferenceCount() == 0)
			{
				// delete the data
				delete register_ptr;

				// remove the entry
				attribute_registerM.erase(it);
			}
			break;
		}
    }
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::AddSystem(DEF_SYSTEM_CLASS* system_def_ptr)

//  DESCRIPTION     : Adds a system to the definition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	systemsM.push_back(system_def_ptr);
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::SetDefinitionFileRoot(const string path) 

//  DESCRIPTION     : Set Definition File Root.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	definition_file_rootM = path; 
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::SetLastSopUidInstalled(const string uid) 

//  DESCRIPTION     : Set last SOP UID installed.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	last_registered_sopM = uid; 
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::SetLastSopUidRemoved(const string uid) 

//  DESCRIPTION     : Set last SOP UID removed.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	last_removed_sopM = uid; 
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetDefinitionFileRoot(void) 

//  DESCRIPTION     : Get Definition File Root.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return definition_file_rootM; 
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::GetDetails(const string filename, DEF_DETAILS_CLASS& details)

//  DESCRIPTION     : Get the definition details.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// try to get the AE defined in the given file
	DEF_AE_CLASS* ae_ptr = GetAe(filename);
	if (ae_ptr)
	{
		// get the definition details from the AE - out of the cache
		result = ae_ptr->GetDetails(filename, details);
	}
	
	if (!result)
	{
		// try to get the result directly from the definition file
		DEFINITION_FILE_CLASS *definitionFile_ptr = new DEFINITION_FILE_CLASS(filename);
		if (definitionFile_ptr)
		{
			result = definitionFile_ptr->GetDetails(details);
		}
	}

	return result;
}

//>>===========================================================================

DEF_SYSTEM_CLASS* DEF_DEFINITION_CLASS::GetSystem(const string name, const string version)

//  DESCRIPTION     : Returns pointer to the requested system definition
//                    If the system is not defined it returns NULL
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SYSTEM_CLASS* system_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
		if ((systemsM[i]->GetName() == name) && 
			(systemsM[i]->GetVersion() == version))
		{
			system_ptr = systemsM[i];
			break;
		}
	}

    return system_ptr;
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::GetSystemDefinitions(string* names, string* versions)

//  DESCRIPTION     : Fills 2 arrays containing the names and versions of all
//                    loaded systems in the definition files
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	int	system_nr = 0;
	bool found = false;

	UINT i = 0;
	while ((i < systemsM.size()) && 
		(system_nr < MAX_APPLICATION_ENTITY))
	{
		for (UINT j = 0; j < systemsM[i]->GetNrAes(); j++)
		{
			string tmp_name    = systemsM[i]->GetAE(j)->GetName();
			string tmp_version = systemsM[i]->GetAE(j)->GetVersion();

			for (int k = 0; k < system_nr ; k++)
			{
				// each system will only be stored in the array once.
				if ((tmp_name == names[k]) && 
					(tmp_version == versions[k]))
				{
					found = true;
				}
			}

			if (found == false)
			{
				names[system_nr] = tmp_name;
				versions[system_nr] = tmp_version;
				system_nr++;
			}
		}

		i++;
	}
}

//>>===========================================================================
		
UINT DEF_DEFINITION_CLASS::GetNoApplicationEntities()

//  DESCRIPTION     : Return the total number of Application Entity definitions
//					: - run through all loaded Systems.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT no_application_entities = 0;

	// loop through all Systems
	for (UINT i = 0; i < systemsM.size(); i++)
	{
		// add the application entities together
		no_application_entities += systemsM[i]->GetNrAes();
	}

	// return total application entities
	return no_application_entities;
}

//>>===========================================================================
		
string DEF_DEFINITION_CLASS::GetApplicationEntityName(UINT index)

//  DESCRIPTION     : Return the indexed Application Entity Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The index includes all Systems.
//<<===========================================================================
{
	string application_entity_name;

	// find the System that is being indexed
	for (UINT i = 0; i < systemsM.size(); i++)
	{
		// check if the index falls in this System
		if (index >= systemsM[i]->GetNrAes())
		{
			// move to next System
			index -= systemsM[i]->GetNrAes();
		}
		else
		{
			// index falls in this System
			// - get the AE definition
			DEF_AE_CLASS *ae_class_ptr = systemsM[i]->GetAE(index);
			if (ae_class_ptr)
			{
				// set the application entity name
				application_entity_name = ae_class_ptr->GetName();
				break;
			}
		}
	}

	// return application entity name
	return application_entity_name;
}

//>>===========================================================================
		
string DEF_DEFINITION_CLASS::GetApplicationEntityVersion(UINT index)

//  DESCRIPTION     : Return the indexed Application Entity Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The index includes all Systems.
//<<===========================================================================
{
	string application_entity_version;

	// find the System that is being indexed
	for (UINT i = 0; i < systemsM.size(); i++)
	{
		// check if the index falls in this System
		if (index >= systemsM[i]->GetNrAes())
		{
			// move to next System
			index -= systemsM[i]->GetNrAes();
		}
		else
		{
			// index falls in this System
			// - get the AE definition
			DEF_AE_CLASS *ae_class_ptr = systemsM[i]->GetAE(index);
			if (ae_class_ptr)
			{
				// set the application entity version
				application_entity_version = ae_class_ptr->GetVersion();
				break;
			}
		}
	}

	// return application entity version
	return application_entity_version;
}

//>>===========================================================================

DEF_AE_CLASS* DEF_DEFINITION_CLASS::GetAe(const string name, const string version)

//  DESCRIPTION     : Returns pointer to the requested Ae definition
//                    If the AE is not defined in any of the systems NULL is returned
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_AE_CLASS* ae_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        ae_ptr = systemsM[i]->GetAE(name, version);
		if (ae_ptr != NULL)
		{
            break;
		}
	}

    return ae_ptr;
}

//>>===========================================================================

DEF_AE_CLASS* DEF_DEFINITION_CLASS::GetAe(const string filename)

//  DESCRIPTION     : Returns pointer to the requested Ae definition
//                    If the AE is not defined in any of the systems NULL is returned
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_AE_CLASS* ae_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        ae_ptr = systemsM[i]->GetAE(filename);
		if (ae_ptr != NULL)
		{
            break;
		}
	}

    return ae_ptr;
}

//>>===========================================================================
		
int DEF_DEFINITION_CLASS::GetNrMetaSopClasses()

//  DESCRIPTION     : Returns the number of metasop classes in the defalt system
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT32 nr_metasops = 0;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
		if (ae_ptr != NULL)
		{
			nr_metasops = ae_ptr->GetNrMetaSops();
            break;
		}
	}

	return nr_metasops;
}

//>>===========================================================================
		
DEF_METASOPCLASS_CLASS* DEF_DEFINITION_CLASS::GetMetaSopClassByUid(const string uid)

//  DESCRIPTION     : Returns pointer to the requested meta sopclass definition.
//                    If the metasop is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_METASOPCLASS_CLASS* metasop_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
		if (ae_ptr != NULL)
		{
		    metasop_ptr = ae_ptr->GetMetaSopByUid(uid);
            break;
		}
	}

	return metasop_ptr;
}

//>>===========================================================================
		
DEF_METASOPCLASS_CLASS* DEF_DEFINITION_CLASS::GetMetaSopClass(UINT32 nr)

//  DESCRIPTION     : Returns pointer to the indexed meta sopclass definition.
//                    If the metasop is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_METASOPCLASS_CLASS* metasop_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
		if (ae_ptr != NULL)
		{
			metasop_ptr = ae_ptr->GetMetaSop(nr);
            break; 
		}
	}

	return metasop_ptr;
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetMetaSopUid(const string uid)

//  DESCRIPTION     : Return the meta sop class uid that contains the given uid 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string metasop_uid;
	DEF_AE_CLASS* ae_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
		if (ae_ptr != NULL)
		{
            break;
		}
	}

    if (ae_ptr != NULL)
	{
		UINT j = 0;
		while (j < ae_ptr->GetNrMetaSops())
		{
            DEF_METASOPCLASS_CLASS* metasop_ptr = ae_ptr->GetMetaSop(j);
			if (metasop_ptr->HasSopClass(uid))
            {
				metasop_uid = metasop_ptr->GetUid();
				break;
			}
			j++;
		}
    }

	return metasop_uid;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_DEFINITION_CLASS::GetSopClassByName(const string name)

//  DESCRIPTION     : Returns pointer to the requested sopclass definition
//                    If the sopclass is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS* sop_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
		if (ae_ptr != NULL)
		{
		    sop_ptr = ae_ptr->GetSopByName(name);
            break;
		}
	}

	return sop_ptr;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_DEFINITION_CLASS::GetSopClassByName(const string ae_name, 
										const string ae_version, 
										const string name)

//  DESCRIPTION     : Returns pointer to the requested sopclass definition
//                    If the sopclass is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS* sop_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(ae_name, ae_version);
		if (ae_ptr != NULL)
		{
		    sop_ptr = ae_ptr->GetSopByName(name);
            break;
		}
	}

	return sop_ptr;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_DEFINITION_CLASS::GetSopClassByFilename(const string filename)

//  DESCRIPTION     : Returns pointer to the requested sopclass definition
//                    If the sopclass is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS* sop_ptr = NULL;
	bool found = false;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
		for (UINT j = 0; j < systemsM[i]->GetNrAes(); j++)
		{
            DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(j);
			if (ae_ptr != NULL)
			{
			    sop_ptr = ae_ptr->GetSopByFilename(filename);
				if (sop_ptr)
				{
					found = true;
				}
			}
			if (found) break;
		}
		if (found) break;
	}

	return sop_ptr;
}

//>>===========================================================================

DVT_STATUS DEF_DEFINITION_CLASS::GetSopClassByUid(const string uid, AE_SESSION_CLASS *ae_session_ptr, DEF_SOPCLASS_CLASS **sop_ptr_ptr)

//  DESCRIPTION     : Returns pointer to the requested sopclass definition
//                    If the system is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DVT_STATUS status = MSG_DEFINITION_NOT_FOUND;
	DEF_SOPCLASS_CLASS* sop_ptr = NULL;

	// search using the AE details given
	for (UINT i = 0; i < systemsM.size(); i++)
	{
		DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(ae_session_ptr->GetName(), ae_session_ptr->GetVersion());
		if (ae_ptr != NULL)
		{
			sop_ptr = ae_ptr->GetSopByUid(uid);
            if (sop_ptr != NULL)
			    break;
		}
	}

	// if nothing found perhaps we need to search using the default AE details
	if ((sop_ptr == NULL) &&
		((ae_session_ptr->GetName() != DEFAULT_AE_NAME) ||
		(ae_session_ptr->GetVersion() != DEFAULT_AE_VERSION)))
	{
		// search using the default AE details
		for (UINT i = 0; i < systemsM.size(); i++)
		{
			DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
			if (ae_ptr != NULL)
			{
				sop_ptr = ae_ptr->GetSopByUid(uid);
                if (sop_ptr != NULL)
				    break;
			}
		}

		if (sop_ptr)
		{
			status = MSG_DEFAULT_DEFINITION_FOUND;
		}
	}
	else if (sop_ptr)
	{
		status = MSG_OK;
	}

	// return status and sop address
	*sop_ptr_ptr = sop_ptr;
	return status;
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetSopName(const string uid)

//  DESCRIPTION     : Returns SopName for requested SopUid
//  PRECONDITIONS   : The sopclass must be defined
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string name;

	DEF_SOP_REGISTER::iterator it = sop_registerM.find(uid);

    if (it != sop_registerM.end())
	{
		// found
	    name = it->second;
	}

	return name;

}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetSopUid(const string name)

//  DESCRIPTION     : Returns Sop Uid for requested SopClass
//  PRECONDITIONS   : The sopclass must be defined
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    string uid;
	bool found = false;

	for (DEF_SOP_REGISTER::iterator sop_register_it = sop_registerM.begin(); sop_register_it != sop_registerM.end(); sop_register_it++)
	{
        if (sop_register_it->second == name)
		{
            uid = sop_register_it->first;
			found = true;
			break;
		}
	}

	// maybe the specified name was an old advt name, try to map
    if (!found)
	{
		string mapped_name = DEFINITION->MapAdvt2DvtSopName(name.c_str());
		for (DEF_SOP_REGISTER::iterator sop_register_it = sop_registerM.begin(); sop_register_it != sop_registerM.end(); sop_register_it++)
		{
			if (sop_register_it->second == mapped_name)
			{
				uid = sop_register_it->first;
				found = true;
				break;
			}
		}
	}

	// maybe the specified name was an iod name (see registerIod function)
	// try to find a uid for this name
	if (!found)
	{
		for (DEF_IOD_REGISTER::iterator iod_register_it = iod_registerM.begin(); iod_register_it != iod_registerM.end(); iod_register_it++)
		{
			if (iod_register_it->second == name)
			{
				uid = iod_register_it->first;
				break;
			}
		}
	}

	return uid;
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetLastSopUidInstalled() 

//  DESCRIPTION     : Get last SOP UID installed.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return last_registered_sopM; 
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetLastSopUidRemoved() 

//  DESCRIPTION     : Get last SOP UID removed.
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return last_removed_sopM; 
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::AddDefFile(const string filename, 
									  const string sopName, 
									  const string sopUid,
									  const string aeName,
									  const string aeVersion,
									  bool isMetasop,
									  bool installed)

//  DESCRIPTION     : Add Def File with the given filename
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_FILE_STRUCT *defFile_ptr = new DEF_FILE_STRUCT();

	defFile_ptr->defFileName = filename;
    defFile_ptr->sopClassName = sopName;
    defFile_ptr->sopClassUid = sopUid;
    defFile_ptr->aeName = aeName;
    defFile_ptr->aeVersion = aeVersion;
    defFile_ptr->isMetaSop = isMetasop;
	defFile_ptr->isInstalled = installed;

	def_filesM.push_back(defFile_ptr);	
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::RemoveDefFile(const string filename)

//  DESCRIPTION     : Remove Def File with the given filename
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop through all the entries
	DEF_FILES_VECTOR::iterator it = def_filesM.begin();
	for (UINT i = 0; i < def_filesM.size(); i++, it++)
    {
        if (def_filesM[i]->defFileName == filename)
        {
            def_filesM.erase(it);
			break;
        }
    }
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::IsDefFileParsed(const string filename)

//  DESCRIPTION     : Check if this Def File with the given filename
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool found = false;
	
	// loop through all the entries
	for (UINT i = 0; i < def_filesM.size(); i++)
    {
        if (def_filesM[i]->defFileName == filename)
        {
            found = true;
			break;
        }
    }

	return found;
}

//>>===========================================================================

DEF_FILE_STRUCT* DEF_DEFINITION_CLASS::GetDefFileStruct(const string filename)

//  DESCRIPTION     : Check if this Def File with the given filename
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_FILE_STRUCT* defFile_ptr = NULL;

	// loop through all the entries
	for (UINT i = 0; i < def_filesM.size(); i++)
    {
		defFile_ptr = def_filesM[i];
        if (defFile_ptr->defFileName == filename)
        {
			// register this as the last sop class used
			SetLastSopUidInstalled(defFile_ptr->sopClassUid);
            break;
        }
		defFile_ptr = NULL;
    }

	return defFile_ptr;
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetImageBoxSopName(const string uid)

//  DESCRIPTION     : Return the name of the image box iod matching to 
//                    print meta sop class uid
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string sop_name;

	//get metasop definition
	DEF_METASOPCLASS_CLASS* metasop_ptr = GetMetaSopClassByUid(uid);
	if (metasop_ptr != NULL)
	{
        sop_name = metasop_ptr->GetImageBoxSopName();
	}

	return sop_name;
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetImageBoxSopUid(const string uid)

//  DESCRIPTION     : Return the uid of the image box iod matching the 
//                    print meta sop class uid
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string box_uid;

	// get metasop definition
	DEF_METASOPCLASS_CLASS* metasop_ptr = GetMetaSopClassByUid(uid);
	if (metasop_ptr != NULL)
	{
        box_uid = metasop_ptr->GetImageBoxSopUid();
	}

	return box_uid;
}

//>>===========================================================================

DVT_STATUS DEF_DEFINITION_CLASS::GetIodName(DIMSE_CMD_ENUM cmd, const string uid, AE_SESSION_CLASS *ae_session_ptr, string &iodName)

//  DESCRIPTION     : Returns the iod name for the requested dimse - sopuid 
//                    combination
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS* sop_ptr = NULL;

	DVT_STATUS status = GetSopClassByUid(uid, ae_session_ptr, &sop_ptr);
	if (sop_ptr != NULL)
	{
		DEF_DATASET_CLASS* dataset_ptr = sop_ptr->GetDataset(cmd);
		if (dataset_ptr != NULL)
		{
			iodName = dataset_ptr->GetName();
		}
	}

	return status;
}

//>>===========================================================================

DVT_STATUS DEF_DEFINITION_CLASS::GetSopFileName(DIMSE_CMD_ENUM cmd, const string uid, AE_SESSION_CLASS *ae_session_ptr, string &fileName)

//  DESCRIPTION     : Returns the iod name for the requested dimse - sopuid 
//                    combination
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DVT_STATUS status = MSG_DEFINITION_NOT_FOUND;

	// search using the AE details given
	for (UINT i = 0; i < systemsM.size(); i++)
	{
		DEF_AE_CLASS* ae_ptr = systemsM[i]->GetAE(ae_session_ptr->GetName(), ae_session_ptr->GetVersion());
		if (ae_ptr != NULL)
		{
			fileName = ae_ptr->GetSopClassFilenameByUID(uid);
            if (fileName != "")
			{
				status = MSG_DEFAULT_DEFINITION_FOUND;
			    break;
			}
		}
	}	

	return status;
}

//>>===========================================================================

DEF_COMMAND_CLASS* DEF_DEFINITION_CLASS::GetCommand(DIMSE_CMD_ENUM cmd)

//  DESCRIPTION     : Return a pointer to a command definition. Can be NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_COMMAND_CLASS* cmd_ptr = NULL;

	for (UINT i = 0; i < commandsM.size(); i++)
	{
        if (commandsM[i]->GetDimseCmd() == cmd)
		{
			cmd_ptr = commandsM[i];
			break;
		}
    }

	return cmd_ptr;
}

//>>===========================================================================

DEF_DATASET_CLASS* DEF_DEFINITION_CLASS::GetDataset(const string name)

//  DESCRIPTION     : Returns pointer to the requested dataset definition
//                    If the dataset is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS* dataset_ptr = NULL;
	DEF_AE_CLASS* ae_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
		if (ae_ptr != NULL)
		{
			break;
		}
	}

    if (ae_ptr != NULL)
	{
		dataset_ptr = ae_ptr->GetDataset(name);
    }

	return dataset_ptr;
}

//>>===========================================================================

DVT_STATUS DEF_DEFINITION_CLASS::GetDataset(DIMSE_CMD_ENUM cmd, const string name, AE_SESSION_CLASS *ae_session_ptr, DEF_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Returns pointer to the requested dataset definition
//                    If the dataset is not defined it returns NULL
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DVT_STATUS status = MSG_DEFINITION_NOT_FOUND;
	DEF_DATASET_CLASS* dataset_ptr = NULL;

	for (UINT i = 0; i < systemsM.size(); i++)
	{
        DEF_AE_CLASS *ae_ptr = systemsM[i]->GetAE(ae_session_ptr->GetName(), ae_session_ptr->GetVersion());
		if (ae_ptr != NULL)
		{
			dataset_ptr = ae_ptr->GetDataset(cmd, name);
            break; 
		}
	}

	// if nothing found perhaps we need to search using the default AE details
	if ((dataset_ptr == NULL) &&
		(ae_session_ptr->GetName() != DEFAULT_AE_NAME) &&
		(ae_session_ptr->GetVersion() != DEFAULT_AE_NAME))
	{
		for (UINT i = 0; i < systemsM.size(); i++)
		{
			DEF_AE_CLASS *ae_ptr = systemsM[i]->GetAE(DEFAULT_AE_NAME, DEFAULT_AE_VERSION);
			if (ae_ptr != NULL)
			{
				dataset_ptr = ae_ptr->GetDataset(cmd, name);
				break; 
			}
		}

		if (dataset_ptr)
		{
			status = MSG_DEFAULT_DEFINITION_FOUND;
		}
	}
	else if (dataset_ptr)
	{
		status = MSG_OK;
	}

	// return status and dataset address
	*dataset_ptr_ptr = dataset_ptr;
	return status;
}

//>>===========================================================================

DEF_ATTRIBUTE_CLASS* DEF_DEFINITION_CLASS::GetAttribute(const UINT group, const UINT element)

//  DESCRIPTION     : Returns the Attribute definition for the requested attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_ATTRIBUTE_CLASS* def_attr_ptr = NULL;

	for (UINT i = 0; i < attribute_registerM.size(); i++)
	{
		DEF_ATTRIBUTE_CLASS *ldef_attr_ptr = attribute_registerM[i]->GetReferenceAttribute();
		if ((ldef_attr_ptr) &&
			(ldef_attr_ptr->GetGroup() == group) && 
			(ldef_attr_ptr->GetElement() == element))
		{
            // found it
			def_attr_ptr = ldef_attr_ptr;
			break;
		}
    }

	return def_attr_ptr;
}

//>>===========================================================================

ATTR_VR_ENUM DEF_DEFINITION_CLASS::GetAttributeVr(const UINT group, const UINT element)

//  DESCRIPTION     : Returns the Attribute VR for the requested attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ATTR_VR_ENUM vr = ATTR_VR_UN;

	DEF_ATTRIBUTE_CLASS* attr_def_ptr = GetAttribute(group, element);
	if (attr_def_ptr != NULL)
	{
		vr = attr_def_ptr->GetVR();
	}

	return vr;
}

//>>===========================================================================

string DEF_DEFINITION_CLASS::GetAttributeName(const UINT group, const UINT element)

//  DESCRIPTION     : Returns the Attribute Name for the requested attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string name;

	DEF_ATTRIBUTE_CLASS* attr_def_ptr = GetAttribute(group, element);
	if (attr_def_ptr != NULL)
	{
		name = attr_def_ptr->GetName();
	}

	return name;
}

//>>===========================================================================

ATTR_TYPE_ENUM DEF_DEFINITION_CLASS::GetAttributeType(const UINT group, const UINT element)

//  DESCRIPTION     : Returns the Attribute Name for the requested attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ATTR_TYPE_ENUM type = ATTR_TYPE_3;

	DEF_ATTRIBUTE_CLASS* attr_def_ptr = GetAttribute(group, element);
	if (attr_def_ptr != NULL)
	{
		type = attr_def_ptr->GetType();
	}

	return type;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::GetAttributeXMLName(const UINT group, const UINT element, string& xml_name)

//  DESCRIPTION     : Tries to find the XMLtag name given a DICOM tag
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool found = false;

	DEF_ATTRIBUTE_CLASS* attr_def_ptr = GetAttribute(group, element);
	if (attr_def_ptr)
	{
		xml_name = attr_def_ptr->GetXMLName();
		found = true;
	}

	return found;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::GetAttributeTagByXMLName(const string xml_name, 
											   UINT16& group, 
											   UINT16& element)

//  DESCRIPTION     : Tries to find the DICOM attribute based on the xml tag name
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool found = false;

	for (UINT i = 0; i < attribute_registerM.size(); i++)
	{
		DEF_ATTRIBUTE_CLASS *def_attr_ptr = attribute_registerM[i]->GetReferenceAttribute();
		if (def_attr_ptr->GetXMLName() == xml_name)
		{
            group = def_attr_ptr->GetGroup();
			element = def_attr_ptr->GetElement();
			found = true;
			break;
		}
    }

	return found;
}

//>>===========================================================================

UINT32 DEF_DEFINITION_CLASS::GetNrDefinedTerms(DIMSE_CMD_ENUM cmd, const string iod_name, UINT32 tag, UINT32 value_nr)

//  DESCRIPTION     : Gets the nr defined/enumerated values for the given
//                    attribute
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT16 group = ((UINT16) (tag >> 16));
	UINT16 element = ((UINT16) (tag & 0x0000FFFF));
	UINT32 nr_defined_values = 0;
	AE_SESSION_CLASS ae_session;

	DEF_DATASET_CLASS *dataset_ptr = NULL;
	GetDataset(cmd, iod_name, &ae_session, &dataset_ptr);
	if (dataset_ptr != NULL)
	{
		DEF_ATTRIBUTE_CLASS* attr_ptr = dataset_ptr->GetAttribute(group, element);
		if (attr_ptr != NULL)
		{
			nr_defined_values = attr_ptr->GetNrValues(value_nr);
		}
	}

	return nr_defined_values;
}

//>>===========================================================================

BASE_VALUE_CLASS* DEF_DEFINITION_CLASS::GetDefinedTerm(DIMSE_CMD_ENUM cmd, const string iod_name, UINT32 tag, UINT32 i, UINT32 value_nr)

//  DESCRIPTION     : Gets the specified defined/enumerated value
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	UINT16 group = ((UINT16) (tag >> 16));
	UINT16 element = ((UINT16) (tag & 0x0000FFFF));
	BASE_VALUE_CLASS* value_ptr = NULL;
	AE_SESSION_CLASS ae_session;

	DEF_DATASET_CLASS *dataset_ptr = NULL;
	GetDataset(cmd, iod_name, &ae_session, &dataset_ptr);
	if (dataset_ptr != NULL)
	{
		DEF_ATTRIBUTE_CLASS* attr_ptr = dataset_ptr->GetAttribute(group, element);
		if (attr_ptr != NULL)
		{
			value_ptr = attr_ptr->GetValue(i, value_nr);
		}
	}

	return value_ptr;
}

//>>===========================================================================

UINT32 DEF_DEFINITION_CLASS::GetNrRecognitionCodes() 

//  DESCRIPTION     : Get the number of Recognition Codes
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return recognition_code_registerM.size(); 
}

//>>===========================================================================

void DEF_DEFINITION_CLASS::GetRecognitionCode(UINT32 i, UINT32* tag_ptr, string& code)

//  DESCRIPTION     : Returns requested tag - recognition code pair
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	RECOGNITION_CODE_STRUCT rec_code_pair;

	rec_code_pair = recognition_code_registerM[i];
	*tag_ptr  = rec_code_pair.tag;
	code = rec_code_pair.code;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::IsStorageSop(const string uid, AE_SESSION_CLASS* ae_session_ptr)

//  DESCRIPTION     : Return true if the sop class uid given is a storage sop 
//                    class 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return (DetermineSopCategory(uid, ae_session_ptr) == SOP_CATEGORY_STORAGE) ? true : false;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::IsPrintSop(const string uid, AE_SESSION_CLASS* ae_session_ptr)

//  DESCRIPTION     : Return true if the sop class uid given is a print meta 
//                    sop class 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return (DetermineSopCategory(uid, ae_session_ptr) == SOP_CATEGORY_PRINT) ? true : false;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::IsMppsSop(const string uid, AE_SESSION_CLASS* ae_session_ptr)

//  DESCRIPTION     : Return true if the sop class uid given is an mpps sop class 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return (DetermineSopCategory(uid, ae_session_ptr) == SOP_CATEGORY_MPPS) ? true : false;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::IsQueryRetrieveSop(const string uid, AE_SESSION_CLASS* ae_session_ptr)

//  DESCRIPTION     : Return true if the sop class uid given is a query/retrieve sop class 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	if ((DetermineSopCategory(uid, ae_session_ptr) == SOP_CATEGORY_QUERY) ||
		(DetermineSopCategory(uid, ae_session_ptr) == SOP_CATEGORY_RETRIEVE))
	{
		result = true;
	}
	return result;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::IsWorklistSop(const string uid, AE_SESSION_CLASS* ae_session_ptr)

//  DESCRIPTION     : Return true if the sop class uid given is a worklist sop class 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return (DetermineSopCategory(uid, ae_session_ptr) == SOP_CATEGORY_WORKLIST) ? true : false;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::PopulateWithAttributes(DCM_DATASET_CLASS* dataset_ptr, LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Adds type 2 attributes based on the definition to the 
//                    given dataset
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool status = true;
	AE_SESSION_CLASS ae_session;
	string name;

    // try to find dataset definition
	// first check whether we deal with an old ADVT or VTS name we have to map
	if (dataset_ptr->getIodName())
	{
		name = dataset_ptr->getIodName();
	}

	DEF_DATASET_CLASS *def_dataset_ptr = NULL;
	GetDataset(dataset_ptr->getCommandId(), name, &ae_session, &def_dataset_ptr);
	if (def_dataset_ptr != NULL)
	{
		PopulateDataset(dataset_ptr, def_dataset_ptr, logger_ptr);
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "Could not find definition for dataset (IOD) %s", name.c_str());
		}
		status = false;
	}

	return status;
}

//>>===========================================================================

const char* DEF_DEFINITION_CLASS::MapAdvt2DvtSopName(const char* advt_name)

//  DESCRIPTION     : Function to map old advt names to dvt names
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Return input name if not found
//<<===========================================================================
{
	const char* found_name_ptr = advt_name;

	for (int i = 0; i < DIM_OF(TAdvt2DvtSopMap); i++)
	{
		if (strcmp(TAdvt2DvtSopMap[i].advt_name, advt_name) == 0)
		{
			found_name_ptr = TAdvt2DvtSopMap[i].dvt_name;
			break;
		}
	}

	// return matching command name if found
	return found_name_ptr;
}

//>>===========================================================================

const char* DEF_DEFINITION_CLASS::MapAdvt2DvtIodName(const char* advt_name)

//  DESCRIPTION     : Function to map old advt names to dvt names
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Return input name if not found
//<<===========================================================================
{
	const char* found_name_ptr = advt_name;

	for (int i = 0; i < DIM_OF(TAdvt2DvtIodMap); i++)
	{
		if (strcmp(TAdvt2DvtIodMap[i].advt_name, advt_name) == 0)
		{
			found_name_ptr = TAdvt2DvtIodMap[i].dvt_name;
			break;
		}
	}

	// return matching command name if found
	return found_name_ptr;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::PopulateDataset(DCM_DATASET_CLASS* dataset_ptr, 
									  DEF_DATASET_CLASS* def_dataset_ptr,
									  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Adds type 2 attributes based on the definition to the 
//                    given dataset
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop over all modules in the definition object
	for (UINT i = 0; i < def_dataset_ptr->GetNrModules(); i++)
	{
		bool addModule = false;
		DEF_MODULE_CLASS* mod_ptr = def_dataset_ptr->GetModule(i);
		MOD_USAGE_ENUM usage = mod_ptr->GetUsage();

		// find out whether the module has to be included
		if (usage == MOD_USAGE_M)
		{
		    addModule = true;
		}

		if (addModule)
		{
			// loop over all attributes in the module
			for (int j = 0; j < mod_ptr->GetNrAttributes(); j++)
			{
				// get attribute definition
                DEF_ATTRIBUTE_CLASS* def_attr_ptr = mod_ptr->GetAttribute(j);
				UINT16 group = def_attr_ptr->GetGroup();
				UINT16 element = def_attr_ptr->GetElement();
				ATTR_VR_ENUM vr = def_attr_ptr->GetVR();
				ATTR_TYPE_ENUM type = def_attr_ptr->GetType();

				if ((type == ATTR_TYPE_2) ||
//					(type == ATTR_TYPE_2C) ||
					 ((type == ATTR_TYPE_1) && 
					 (vr == ATTR_VR_SQ)))
				{
					if (dataset_ptr->getPopulateWithAttributes())
					{
						// check whether the attribute is already present in the dataset
						DCM_ATTRIBUTE_CLASS* attr_ptr = dataset_ptr->GetAttribute(group, element);
						if ((attr_ptr != NULL) && 
							(attr_ptr->IsPresent())) // attribute is present
						{
							if (vr == ATTR_VR_SQ)
							{
                                PopulateSequence(attr_ptr, def_attr_ptr, logger_ptr);
							}
						}
						else // not present
						{
							// ensure that Type2C get encoded
							if (type == ATTR_TYPE_2C) type = ATTR_TYPE_2;

							// add attribute
							attr_ptr = new DCM_ATTRIBUTE_CLASS(group, element, vr);
							attr_ptr->SetType(type);
							dataset_ptr->addAttribute(attr_ptr);
						}
					}
					else // do not auto populate the dataset
					{
						// we don't populate the complete dataset, hoewever
						// if we're dealing with a sequence attribute
						// and it is already present in the dataset we may 
						// want to auto populate the item(s)
						DCM_ATTRIBUTE_CLASS* attr_ptr = dataset_ptr->GetAttribute(group, element);
						if ((attr_ptr != NULL) && 
							(attr_ptr->IsPresent())) // attribute is present
						{
							if (vr == ATTR_VR_SQ)
							{
                                PopulateSequence(attr_ptr, def_attr_ptr, logger_ptr);
							}
						}
					}
				}
			}

			// loop over all macros in the module, evaluate the condition
			// if the condition is true, add all macro attributes to the dataset 
			// if there is no condition specified for the macro the attributes are added
			/* for (j = 0; j < mod_ptr->GetNrMacros(); j++) MIGRATION_IN_PROGRESS */ for (int j = 0; j < mod_ptr->GetNrMacros(); j++)
			{
				bool addMacro = false;
				CONDITION_CLASS* macro_cond_ptr = mod_ptr->GetMacroCondition(j);
				if (macro_cond_ptr != NULL)
				{
					CONDITION_RESULT_ENUM condition_result = macro_cond_ptr->Evaluate(dataset_ptr, 0, logger_ptr);
					addMacro = (condition_result == CONDITION_TRUE) ? true : false;
				}
				else
				{
                    addMacro = true;
				}

				if (addMacro)
				{
					DEF_MACRO_CLASS* macro_ptr = mod_ptr->GetMacro(j);

					// loop over all attributes in the macro
					for (int k = 0; k < macro_ptr->GetNrAttributes(); k++)
					{
						// get attribute definition
					    DEF_ATTRIBUTE_CLASS* def_attr_ptr = macro_ptr->GetAttribute(k);
						UINT16 group = def_attr_ptr->GetGroup();
						UINT16 element = def_attr_ptr->GetElement();
						ATTR_VR_ENUM vr = def_attr_ptr->GetVR();
						ATTR_TYPE_ENUM type = def_attr_ptr->GetType();

						if ((type == ATTR_TYPE_2) ||
//							(type == ATTR_TYPE_2C) ||
							 ((type == ATTR_TYPE_1) && 
							 (vr == ATTR_VR_SQ)))
						{
							//check whether the attribute is already present in the item
							DCM_ATTRIBUTE_CLASS *attr_ptr = dataset_ptr->GetAttribute(group, element);
							if ((attr_ptr != NULL)  && 
								(attr_ptr->IsPresent()))
							{
								if (vr == ATTR_VR_SQ)
								{
									PopulateSequence(attr_ptr, def_attr_ptr, logger_ptr);
								}
							}
							else // not present
							{
								// ensure that Type2C get encoded
								if (type == ATTR_TYPE_2C) type = ATTR_TYPE_2;

								// add attribute
								attr_ptr = new DCM_ATTRIBUTE_CLASS(group, element, vr);
								attr_ptr->SetType(type);
								dataset_ptr->addAttribute(attr_ptr);
							}
						}
					}
				} // addMacro
			}
		} // addModule
	}

	return true;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::PopulateSequence(DCM_ATTRIBUTE_CLASS* attr_ptr, 
				                       DEF_ATTRIBUTE_CLASS* def_attr_ptr,
				                       LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Populates the items in a sequence if desired
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool status = false;

	// SQ definition object will only have 1 valueslist
	if (def_attr_ptr->GetNrLists() > 1)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Multiple SQ definitions not supported");
		}
		return false;
	}

	if (def_attr_ptr->GetNrValues() > 1)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Multiple Item definitions not supported");
		}
		return false;
	}

	VALUE_SQ_CLASS* def_sq_value_ptr = static_cast<VALUE_SQ_CLASS*>(def_attr_ptr->GetValue(0));
	if (def_sq_value_ptr == NULL) return false;

	DEF_ITEM_CLASS*	def_item_ptr;	
	if (def_sq_value_ptr->Get((ATTRIBUTE_GROUP_CLASS**) &def_item_ptr) != MSG_OK) return false;

	if (attr_ptr->GetNrValues() == 1)
	{
		DCM_VALUE_SQ_CLASS* sq_value_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attr_ptr->GetValue(0));
			
		for (int i = 0; i < sq_value_ptr->GetNrItems(); i++)
		{
			DCM_ITEM_CLASS* item_ptr = sq_value_ptr->getItem(i);
			
			// populate items
		    status = PopulateItem(item_ptr, def_item_ptr, logger_ptr);
		}
	}
	else if (attr_ptr->GetNrValues() != 0)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Internal Error: Number of SQ values should be 1");
		}
	}

	return status;
}

//>>===========================================================================

bool DEF_DEFINITION_CLASS::PopulateItem(DCM_ITEM_CLASS* item_ptr, 
								   DEF_ITEM_CLASS* def_item_ptr, 
								   LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Adds type 2 attributes based on the definition to the 
//                    given item
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// loop over all attributes in the item definition
    for (int i = 0; i < def_item_ptr->GetNrAttributes(); i++)
	{
		// get attribute definition
        DEF_ATTRIBUTE_CLASS* def_attr_ptr = def_item_ptr->GetAttribute(i);
		UINT16 group = def_attr_ptr->GetGroup();
		UINT16 element = def_attr_ptr->GetElement();
		ATTR_VR_ENUM vr = def_attr_ptr->GetVR();
		ATTR_TYPE_ENUM type = def_attr_ptr->GetType();

		if ((type == ATTR_TYPE_2) || 
//			(type == ATTR_TYPE_2C) ||
			 ((type == ATTR_TYPE_1) && 
			 (vr == ATTR_VR_SQ)))
		{
			if (item_ptr->getPopulateWithAttributes())
			{
				// check whether the attribute is already present in the item
				DCM_ATTRIBUTE_CLASS* attr_ptr = item_ptr->GetAttribute(group, element);

				if ((attr_ptr != NULL) && 
					(attr_ptr->IsPresent())) // attribute is present
				{
					if (vr == ATTR_VR_SQ)
					{
                        PopulateSequence(attr_ptr, def_attr_ptr, logger_ptr);
					}
				}
				else // not present
				{
					// ensure that Type2C get encoded
					if (type == ATTR_TYPE_2C) type = ATTR_TYPE_2;

					// add attribute
					attr_ptr = new DCM_ATTRIBUTE_CLASS(group, element, vr);
					attr_ptr->SetType(type);
					item_ptr->addAttribute(attr_ptr);
				}
			}
			else // do not auto populate the item
			{
				// we don't populate the complete item, hoewever
				// if we're dealing with a sequence attribute
				// and it is already present in the item we may 
				// want to auto populate the item(s)
				DCM_ATTRIBUTE_CLASS* attr_ptr = item_ptr->GetAttribute(group, element);

				if ((attr_ptr != NULL) && 
					(attr_ptr->IsPresent())) // attribute is present
				{
					if (vr == ATTR_VR_SQ)
					{
                        PopulateSequence(attr_ptr, def_attr_ptr, logger_ptr);
					}
				}
			}
		}
	}

	// loop over all macros in the item, evaluate the condition
	// if the condition is true, add all macro attributes to the results list
	// if there is no condition specified for the macro the attributes are added
	for (int j = 0; j < def_item_ptr->GetNrMacros(); j++)
	{
		bool addMacro = false;
		CONDITION_CLASS* macro_cond_ptr = def_item_ptr->GetMacroCondition(j);

		if (macro_cond_ptr != NULL)
		{
			CONDITION_RESULT_ENUM condition_result = macro_cond_ptr->Evaluate(item_ptr, 0, logger_ptr);
			addMacro = (condition_result == CONDITION_TRUE) ? true : false;
		}
		else
		{
            addMacro = true;
		}

		if (addMacro)
		{
			DEF_MACRO_CLASS* macro_ptr = def_item_ptr->GetMacro(j);

			// loop over all attributes in the macro
			for (int k = 0; k < macro_ptr->GetNrAttributes(); k++)
			{
				// get attribute definition
				DEF_ATTRIBUTE_CLASS* def_attr_ptr = macro_ptr->GetAttribute(k);
                UINT16 group = def_attr_ptr->GetGroup();
				UINT16 element = def_attr_ptr->GetElement();
				ATTR_VR_ENUM vr = def_attr_ptr->GetVR();
				ATTR_TYPE_ENUM type = def_attr_ptr->GetType();

				if ((type == ATTR_TYPE_2) ||
//					(type == ATTR_TYPE_2C) ||
					((type == ATTR_TYPE_1) && 
					(vr == ATTR_VR_SQ)))
				{
					// check whether the attribute is already present in the item
					DCM_ATTRIBUTE_CLASS* attr_ptr = item_ptr->GetAttribute(group, element);

					if ((attr_ptr != NULL)  && 
						(attr_ptr->IsPresent()))
					{
						if (vr == ATTR_VR_SQ)
						{
							PopulateSequence(attr_ptr, def_attr_ptr, logger_ptr);
						}
					}
					else // not present
					{
						// ensure that Type2C get encoded
						if (type == ATTR_TYPE_2C) type = ATTR_TYPE_2;

						// add attribute
						attr_ptr = new DCM_ATTRIBUTE_CLASS(group, element, vr);
						attr_ptr->SetType(type);
						item_ptr->addAttribute(attr_ptr);
					}
				}
			}
		} // addMacro
	}

	return true;
}

//>>===========================================================================

SOP_CATEGORY_ENUM DEF_DEFINITION_CLASS::DetermineSopCategory(const string uid, AE_SESSION_CLASS* ae_session_ptr)

//  DESCRIPTION     : returns the sop class category
//  PRECONDITIONS   : The requested sop class must be loaded
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Only the Storage, Media, Verification and Print categories
//                    are currently determined
//<<===========================================================================
{
	SOP_CATEGORY_ENUM category = SOP_CATEGORY_UNKNOWN;

	// first check for storage sop class
	DEF_SOPCLASS_CLASS *sop_ptr = NULL; 
	DEFINITION->GetSopClassByUid(uid, ae_session_ptr, &sop_ptr);
	if (sop_ptr != NULL)
	{
        DEF_DATASET_CLASS *dataset_ptr = sop_ptr->GetDataset(DIMSE_CMD_CSTORE_RQ);
		if (dataset_ptr != NULL)
		{
            category = SOP_CATEGORY_STORAGE;
		}
		else
		{
		    if (uid == VERIFICATION_SOP_CLASS_UID)
			{
				category = SOP_CATEGORY_VERIFICATION;
			}
			else if (uid == MEDIA_STORAGE_DIRECTORY_SOP_CLASS_UID)
			{
				category = SOP_CATEGORY_MEDIA;
			}
			else if (uid == PATIENT_ROOT_QR_FIND_SOP_CLASS_UID ||
				uid == STUDY_ROOT_QR_FIND_SOP_CLASS_UID ||
				uid == PATIENT_STUDY_ONLY_QR_FIND_SOP_CLASS_UID)
			{
				category = SOP_CATEGORY_QUERY;
			}
			else if (uid == PATIENT_ROOT_QR_MOVE_SOP_CLASS_UID ||
				uid == PATIENT_ROOT_QR_GET_SOP_CLASS_UID ||
				uid == STUDY_ROOT_QR_MOVE_SOP_CLASS_UID ||
				uid == STUDY_ROOT_QR_GET_SOP_CLASS_UID ||
				uid == PATIENT_STUDY_ONLY_QR_MOVE_SOP_CLASS_UID ||
				uid == PATIENT_STUDY_ONLY_QR_GET_SOP_CLASS_UID)
			{
				category = SOP_CATEGORY_RETRIEVE;
			}
			else if (uid == MODALITY_PERFORMED_PROCEDURE_STEP_SOP_CLASS_UID)
			{
				category = SOP_CATEGORY_MPPS;
			}
			else if (uid == MODALITY_WORKLIST_FIND_SOP_CLASS ||
				uid == GENERAL_PURPOSE_WORKLIST_SOP_CLASS)
			{
				category = SOP_CATEGORY_WORKLIST;
			}
			else if (uid == BASIC_GRAY_PRINT_META ||
					uid == REFERENCED_GRAY_PRINT_META ||
					uid == BASIC_COLOR_PRINT_META ||
					uid == REFERENCED_COLOR_PRINT_META	||
					uid == FILM_SESSION_SOP_CLASS_UID ||
					uid == FILM_BOX_SOP_CLASS_UID ||
					uid == GRAY_IMAGE_BOX_SOP_CLASS_UID ||
					uid == COLOR_IMAGE_BOX_SOP_CLASS_UID ||
					uid == REFERENCED_IMAGE_BOX_SOP_CLASS_UID ||
					uid == PRINTER_SOP_CLASS_UID ||
					uid == PRINTER_SOP_INSTANCE_UID	||
					uid == PRINT_JOB_SOP_CLASS_UID ||
					uid == ANNOTATION_BOX_SOP_CLASS_UID ||
					uid == IMAGE_OVERLAY_SOP_CLASS_UID ||
					uid == VOI_LUT_BOX_SOP_CLASS_UID ||
					uid == PRESENTATION_LUT_SOP_CLASS_UID)			
			{
				category = SOP_CATEGORY_PRINT;
			}
		}
    }
	else // special check for print
	{
		if (uid == BASIC_GRAY_PRINT_META ||
			uid == REFERENCED_GRAY_PRINT_META ||
			uid == BASIC_COLOR_PRINT_META ||
			uid == REFERENCED_COLOR_PRINT_META	||
			uid == FILM_SESSION_SOP_CLASS_UID ||
			uid == FILM_BOX_SOP_CLASS_UID ||
			uid == GRAY_IMAGE_BOX_SOP_CLASS_UID ||
			uid == COLOR_IMAGE_BOX_SOP_CLASS_UID ||
			uid == REFERENCED_IMAGE_BOX_SOP_CLASS_UID ||
			uid == PRINTER_SOP_CLASS_UID ||
			uid == PRINTER_SOP_INSTANCE_UID	||
			uid == PRINT_JOB_SOP_CLASS_UID ||
			uid == ANNOTATION_BOX_SOP_CLASS_UID ||
			uid == IMAGE_OVERLAY_SOP_CLASS_UID ||
			uid == VOI_LUT_BOX_SOP_CLASS_UID ||
			uid == PRESENTATION_LUT_SOP_CLASS_UID)			
			{
				category = SOP_CATEGORY_PRINT;
			}
	}
	
	return category;
}
