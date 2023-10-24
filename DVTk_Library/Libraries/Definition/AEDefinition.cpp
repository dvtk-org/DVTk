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
#include "Ilog.h"				// Logger Interface
#include "DefinitionDetails.h"
#include "DefFileContent.h"
#include "DefinitionInstance.h"
#include "AEDefinition.h"
#include "SopClassDefinition.h"
#include "MetaSopClassDefinition.h"
#include "definition.h"


//>>===========================================================================

DEF_AE_CLASS::DEF_AE_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

DEF_AE_CLASS::DEF_AE_CLASS(const string name, const string version)

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
    nameM = name;
	versionM = version;
}

//>>===========================================================================

DEF_AE_CLASS::~DEF_AE_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // destructor activities
	// - clean up instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		delete instanceM[i];
	}
	instanceM.clear();
}

//>>===========================================================================

void DEF_AE_CLASS::SetName(const string name)

//  DESCRIPTION     : Set the AE Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	nameM = name;
}

//>>===========================================================================

void DEF_AE_CLASS::SetVersion(const string version)

//  DESCRIPTION     : Set the AE Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	versionM = version;
}

//>>===========================================================================

string DEF_AE_CLASS::GetName()

//  DESCRIPTION     : Get the AE Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{ 
	return nameM;
}

//>>===========================================================================

string DEF_AE_CLASS::GetVersion()

//  DESCRIPTION     : Get the AE Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return versionM;
}

//>>===========================================================================

bool DEF_AE_CLASS::AddInstance(const string filename, DEF_FILE_CONTENT_CLASS *fileContent_ptr)

//  DESCRIPTION     : Add an instance
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool installed = false;

	// add an instance - first check if the instance is already defined
	// - if so increment the reference count
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// instance already installed if the filenames are the same
		if (instanceM[i]->GetFilename() == filename)
		{
			// increment the reference index
			instanceM[i]->IncrementReferenceIndex();

			// register this as the last sop class used
			DEFINITION->SetLastSopUidInstalled(fileContent_ptr->GetSOPClassUID());
			
			// delete the new instance - part of cleanup
			delete fileContent_ptr;

			// set installed flag
			installed = true;
			break;
		}
	}

	// check if installed
	if (!installed)
	{
		// create a new instance
		DEF_INSTANCE_CLASS *instance_ptr = new DEF_INSTANCE_CLASS(filename, fileContent_ptr);

		// register the file content
		fileContent_ptr->Register();

		// add the new instance
		instanceM.push_back(instance_ptr);

		// indicate that the definition is now installed
		installed = true;
	}

	return installed;
}

//>>===========================================================================

bool DEF_AE_CLASS::RemoveInstance(const string filename)

//  DESCRIPTION     : Remove an instance
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_INSTANCE_VECTOR::iterator it = instanceM.begin();

	// remove an instance - but only completely remove it when the reference index is zero
	for (UINT i = 0; i < instanceM.size(); i++, it++)
	{
		// instance installed if the filenames are the same
		if (instanceM[i]->GetFilename() == filename)
		{
			// decrement the reference index
			instanceM[i]->DecrementReferenceIndex();

			// if the reference index is now zero - remove the instance completely
			if (instanceM[i]->GetReferenceIndex() == 0)
			{
				// clean up
				DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
				if (fileContent_ptr)
				{
					// unregister the file content
					fileContent_ptr->Unregister();
				}
				delete instanceM[i];
				instanceM[i] = NULL;
				instanceM.erase(it);
				break;
			}
		}
	}

	return true;
}

//>>===========================================================================

bool DEF_AE_CLASS::ContainsInstance(const string filename)

//  DESCRIPTION     : Check if this AE contains a definition instance with the
//					: given filename
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool found = false;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// instance installed if the filenames are the same
		if (instanceM[i]->GetFilename() == filename)
		{
			// instance found
			found = true;
			break;
		}
	}

	return found;
}

//>>===========================================================================

bool DEF_AE_CLASS::GetDetails(const string filename, DEF_DETAILS_CLASS& details)

//  DESCRIPTION     : Try to get the definition detail in the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the required instance
		if (instanceM[i]->GetFilename() == filename)
		{
			// get the file contents
			DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
			if (fileContent_ptr)
			{
				// get the detail
				details.SetAEName(fileContent_ptr->GetAEName());
				details.SetAEVersion(fileContent_ptr->GetAEVersion());
				details.SetSOPClassName(fileContent_ptr->GetSOPClassName());
				details.SetSOPClassUID(fileContent_ptr->GetSOPClassUID());
				details.SetIsMetaSOPClass(fileContent_ptr->IsMetaSOPClass());
	
				result = true;
			}
			break;
		}
	}

	return result;
}

//>>===========================================================================

UINT DEF_AE_CLASS::GetNrMetaSops() 

//  DESCRIPTION     : Get the number of Meta SOP Classes installed.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT nrMetaSops = 0;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			if (fileContent_ptr->IsMetaSOPClass())
			{
				nrMetaSops++;
			}
		}
	}

	return nrMetaSops;
}

//>>===========================================================================

DEF_METASOPCLASS_CLASS* DEF_AE_CLASS::GetMetaSop(UINT index) 

//  DESCRIPTION     : Get the indexed Meta SOP Class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_METASOPCLASS_CLASS* metaSopClass_ptr = NULL;
	UINT nrMetaSops = 0;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			if (fileContent_ptr->IsMetaSOPClass())
			{
				// check if the index matches
				if (index == nrMetaSops)
				{
					metaSopClass_ptr = fileContent_ptr->GetMetaSop();
					break;
				}
				
				nrMetaSops++;
			}
		}
	}

	return metaSopClass_ptr;
}

//>>===========================================================================

DEF_METASOPCLASS_CLASS* DEF_AE_CLASS::GetMetaSopByUid(const string uid)

//  DESCRIPTION     : Get the Meta SOP Class definition by UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_METASOPCLASS_CLASS *metaSopClass_ptr = NULL;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			// try to get the Meta SOP Class
			metaSopClass_ptr = fileContent_ptr->GetMetaSopByUid(uid);
			break;
		}
	}

	return metaSopClass_ptr;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_AE_CLASS::GetSopByUid(const string uid)

//  DESCRIPTION     : Get SOP Class definition by UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS *sopClass_ptr = NULL;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			// try to get the SOP Class
			sopClass_ptr = fileContent_ptr->GetSopByUid(uid);
            if (sopClass_ptr != NULL)
			    break;
		}
	}

	return sopClass_ptr;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_AE_CLASS::GetSopByFilename(const string filename)

//  DESCRIPTION     : Get SOP Class definition by Filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS *sopClass_ptr = NULL;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// instance installed if the filenames are the same
		if (instanceM[i]->GetFilename() == filename)
		{
			// get the file contents
			DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
			if (fileContent_ptr)
			{
				// try to get the SOP Class
				sopClass_ptr = fileContent_ptr->GetFirstSop();
			}

			// instance found
			break;
		}
	}

	return sopClass_ptr;
}

//>>===========================================================================

string DEF_AE_CLASS::GetSopClassFilenameByUID(const string uid)

//  DESCRIPTION     : Get SOP Class file name by SOP class UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string fileName = "";

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// instance installed if the uid is the same
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			if (fileContent_ptr->GetSOPClassUID() == uid)
			{
				fileName = instanceM[i]->GetFilename();

				// instance found
				break;
			}
		}		
	}

	return fileName;
}

//>>===========================================================================

DEF_SOPCLASS_CLASS* DEF_AE_CLASS::GetSopByName(const string name)

//  DESCRIPTION     : Get SOP Class definition by Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_SOPCLASS_CLASS *sopClass_ptr = NULL;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			// try to get the SOP Class
			sopClass_ptr = fileContent_ptr->GetSopByName(name);
			break;
		}
	}

	return sopClass_ptr;
}
	
//>>===========================================================================

DEF_DATASET_CLASS *DEF_AE_CLASS::GetDataset(const string name)

//  DESCRIPTION     : Get Dataset definition by IOD Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS *dataset_ptr = NULL;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			// try to get the dataset
			dataset_ptr = fileContent_ptr->GetDataset(name);
			if (dataset_ptr)
			{
				break;
			}
		}
	}

	return dataset_ptr;
}

//>>===========================================================================

DEF_DATASET_CLASS *DEF_AE_CLASS::GetDataset(DIMSE_CMD_ENUM cmd, const string name)

//  DESCRIPTION     : Get Dataset definition by DIMSE Command and IOD Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS *dataset_ptr = NULL;

	// loop through all instances
	for (UINT i = 0; i < instanceM.size(); i++)
	{
		// get the file contents
		DEF_FILE_CONTENT_CLASS *fileContent_ptr = instanceM[i]->GetFileContent();
		if (fileContent_ptr)
		{
			// try to get the dataset
			dataset_ptr = fileContent_ptr->GetDataset(cmd, name);
			if (dataset_ptr)
			{
				break;
			}
		}
	}

	return dataset_ptr;
}
