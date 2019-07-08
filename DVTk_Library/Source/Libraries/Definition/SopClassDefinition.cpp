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
#include "SopClassDefinition.h"
#include "DatasetDefinition.h"
#include "definition.h"
#include "MacroDefinition.h"
#include "Iutility.h"			// Utility component interface
#include "Iwarehouse.h"			// Warehouse component interface


//>>===========================================================================

DEF_SOPCLASS_CLASS::DEF_SOPCLASS_CLASS()

//  DESCRIPTION     : Default contructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>===========================================================================

DEF_SOPCLASS_CLASS::DEF_SOPCLASS_CLASS(const string UID, const string Name)

//  DESCRIPTION     : Default contructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	uidM  = UID;
	nameM = Name;

	// register the Name / UID mapping
	WAREHOUSE->addMappedValue((char*) Name.c_str(), (char*) UID.c_str(), 0, 0, false);
}

//>>===========================================================================

DEF_SOPCLASS_CLASS::~DEF_SOPCLASS_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	// - delete dataset definitions
    for (UINT i = 0; i < datasetsM.size(); i++)
	{
		delete datasetsM[i];
	}
	datasetsM.clear();

	// - delete macro definitions
    for (UINT i = 0; i < macrosM.size(); i++)
	{
		delete macrosM[i];
	}
	macrosM.clear();
}

//>>===========================================================================

bool DEF_SOPCLASS_CLASS::AddDataset(DEF_DATASET_CLASS* ds_ptr)

//  DESCRIPTION     : Adds a dataset to the SopClass definition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check whether a dataset for this dimse cmd already exists
	DEF_DATASET_CLASS* dataset_ptr = GetDataset(ds_ptr->GetName(), ds_ptr->GetDimseCmd());
	if (dataset_ptr == NULL)
	{
		// this is a new datasetdefinition. Add it to the sop class
		datasetsM.push_back(ds_ptr);
		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================

void DEF_SOPCLASS_CLASS::AddMacro(DEF_MACRO_CLASS* macro_ptr)

//  DESCRIPTION     : Adds a Macro to the SopClass definition
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	macrosM.push_back(macro_ptr);
}

//>>===========================================================================

DEF_DATASET_CLASS* DEF_SOPCLASS_CLASS::GetDataset(DIMSE_CMD_ENUM cmd)

//  DESCRIPTION     : Return a pointer to the dataset for the dimse_cmd 
//                    Can be 0!
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS* dataset_ptr = NULL;

	for (UINT i = 0; i < datasetsM.size(); i++)
	{
        if (datasetsM[i]->GetDimseCmd() == cmd)
		{
			dataset_ptr = datasetsM[i];
			break;
		}
    }

	return dataset_ptr;
}

//>>===========================================================================

DEF_DATASET_CLASS* DEF_SOPCLASS_CLASS::GetDataset(const string name)

//  DESCRIPTION     : Return a pointer to the specified dataset. Can be 0!
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS* dataset_ptr = NULL;

	for (UINT i = 0; i < datasetsM.size(); i++)
	{
        if (datasetsM[i]->GetName() == name)
		{
			dataset_ptr = datasetsM[i];
			break;
		}
    }

	// maybe we're dealing with an old ADVT name, try to map
	if (dataset_ptr == NULL)
	{
		// try to map name
		string mapped_name = DEFINITION->MapAdvt2DvtIodName(name.c_str());

		for (UINT j = 0; j < datasetsM.size(); j++)
		{
			if (datasetsM[j]->GetName() == mapped_name)
			{
				dataset_ptr = datasetsM[j];
				break;
			}
		}
	}

	return dataset_ptr;
}

//>>===========================================================================

DEF_DATASET_CLASS* DEF_SOPCLASS_CLASS::GetDataset(const string name, DIMSE_CMD_ENUM cmd)

//  DESCRIPTION     : Return a pointer to the dataset for the dimse_cmd 
//                    Can be 0!
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DEF_DATASET_CLASS* dataset_ptr = NULL;

	for (UINT i = 0; i < datasetsM.size(); i++)
	{
        if ((datasetsM[i]->GetDimseCmd() == cmd) &&
			(datasetsM[i]->GetName() == name))
		{
			dataset_ptr = datasetsM[i];
			break;
		}
    }

	// maybe we're dealing with an old ADVT name, try to map
	if (dataset_ptr == NULL)
	{
		// try to map name
		string mapped_name = DEFINITION->MapAdvt2DvtIodName(name.c_str());

		for (UINT j = 0; j < datasetsM.size(); j++)
		{
			if ((datasetsM[j]->GetDimseCmd() == cmd) &&
				(datasetsM[j]->GetName() == mapped_name))
			{
				dataset_ptr = datasetsM[j];
				break;
			}
		}
	}

	return dataset_ptr;
}
