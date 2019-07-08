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
#include "IAttributeGroup.h"	// Attribute Group component interface
#include "Idefinition.h"		// Definition component interface
#include "Idicom.h"				// Dicom component interface
#include "Imedia.h"				// Media File component interface
#include "Isession.h"			// Test Session component interface
#include "Ivalidation.h"		// Validation component interface

#include <direct.h>
#include <io.h>

#ifdef _WINDOWS
#include <time.h>
#else
#include <unistd.h>
#include <sys/time.h>
#endif

/* RBTD */
#define MAX_TAG            0xFFFFFFFF
#define MAX_GROUP          0xFFFF
#define MAX_ELEMENT        0xFFFF

struct OBJECT_PAIR_STRUCT
{
	SCRIPT_SESSION_CLASS*	session_ptr;    //store objects per session
    DCM_COMMAND_CLASS*		command_ptr;
	DCM_DATASET_CLASS*		dataset_ptr;
};

static vector<OBJECT_PAIR_STRUCT> ref_objects;

static map<SCRIPT_SESSION_CLASS*, bool> datasetexpected;

enum OPERAND_ENUM
{
  OPERAND_OR,
  OPERAND_AND
};

static OPERAND_ENUM operand = OPERAND_OR;

bool validateSopAgainstList(SCRIPT_SESSION_CLASS *session_ptr, DCM_COMMAND_CLASS* command_ptr, DCM_DATASET_CLASS* dataset_ptr);

struct VTS_UID_MAPPING_STRUCT
{
    UINT32       tag;
	string       label;
	string       value;
};

static vector<VTS_UID_MAPPING_STRUCT> vts_uid_mappings;

bool registerVTSUidMappings(DCM_ATTRIBUTE_GROUP_CLASS* src_object_ptr,
					        DCM_ATTRIBUTE_GROUP_CLASS* ref_object_ptr,
					        LOG_CLASS*          logger_ptr);
bool checkVTSUidMappingsInSequence(DCM_ATTRIBUTE_CLASS* src_attr_ptr, 
							       DCM_ATTRIBUTE_CLASS* ref_attr_ptr,
							       LOG_CLASS*       logger_ptr);

void registerVTSUidMapping(UINT32 tag, string label, string value, LOG_CLASS* logger_ptr);
void resolveVTSUidMappings(DCM_ATTRIBUTE_GROUP_CLASS* object_ptr);

static void replaceVTSUidLabels(DCM_ATTRIBUTE_GROUP_CLASS* object_ptr, string label, string value);
static bool findUidAttribute(UINT32 tag, DCM_ATTRIBUTE_GROUP_CLASS* object_ptr, string& uid);

//>>===========================================================================

bool compareDatasetValueWithWarehouse (LOG_CLASS		* logger_ptr,
									   const char		* identifier_ptr,
									   DCM_DATASET_CLASS	* dataset_ptr)

//  DESCRIPTION     : Function to compare a dataset value with the value
//                    stored in the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool result;
	AE_SESSION_CLASS ae_session;

	DEF_DATASET_CLASS *def_dataset_ptr = NULL;
	DEFINITION->GetDataset(dataset_ptr->getCommandId(), identifier_ptr, &ae_session, &def_dataset_ptr);
	if (def_dataset_ptr == NULL)
	{
		result = false;

		// log the action
		if (logger_ptr != NULL)
		{
			logger_ptr->text(LOG_WARNING,
							 1,
							 "Dataset \"%s\" with value \"%s\" not found in warehouse",
							 dataset_ptr->getIdentifier(),
							 identifier_ptr);
		}
	}
	else
	{
		result = true;
	}

	return result;
}

//>>===========================================================================

bool storeObjectInWarehouse(LOG_CLASS *logger_ptr, const char *identifier_ptr, BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Function to handle the object storage in the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string identifier;

	if (identifier_ptr)
	{
		identifier = identifier_ptr;
	}

	// try to store the object in the warehouse
	wid_ptr->setLogger(logger_ptr);
	bool result = WAREHOUSE->store(identifier, wid_ptr);
	if (result)
	{
		// log the action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "CREATE %s %s (in Data Warehouse)", WIDName(wid_ptr->getWidType()), identifier.c_str());
		}
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "Can't create %s %s in Data Warehouse", WIDName(wid_ptr->getWidType()), identifier.c_str());
		}
	}
	wid_ptr->setLogger(NULL);

	// return result
	return result;
}

//>>===========================================================================

bool updateObjectInWarehouse(LOG_CLASS *logger_ptr, const char *identifier_ptr, BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Function to handle the stored object update in the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string identifier;

	if (identifier_ptr)
	{
		identifier = identifier_ptr;
	}

	// try to update the stored object in the warehouse
	wid_ptr->setLogger(logger_ptr);
	bool result = WAREHOUSE->update(identifier, wid_ptr);
	if (result)
	{
		// log the action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "SET %s %s (stored in Data Warehouse)", WIDName(wid_ptr->getWidType()), identifier.c_str());
		}
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "Can't find %s %s stored in Data Warehouse for update - check that object has been created.", WIDName(wid_ptr->getWidType()), identifier.c_str());
		}
	}
	wid_ptr->setLogger(NULL);

	// return result
	return result;
}

//>>===========================================================================

bool removeObjectFromWarehouse(LOG_CLASS *logger_ptr, const char *identifier_ptr, WID_ENUM wid)

//  DESCRIPTION     : Function to handle the object removal from the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string identifier;

	if (identifier_ptr)
	{
		identifier = identifier_ptr;
	}

	// try to remove the object from the warehouse
	bool result = WAREHOUSE->remove(identifier, wid);
	if (result)
	{
		// log the action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "DELETE %s %s (from Data Warehouse)", WIDName(wid), identifier.c_str());
		}
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "Can't delete %s %s from Data Warehouse", WIDName(wid), identifier.c_str());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

BASE_WAREHOUSE_ITEM_DATA_CLASS *retrieveFromWarehouse(LOG_CLASS *logger_ptr, const char *identifier_ptr, WID_ENUM wid)

//  DESCRIPTION     : Function to retrieve the the requested object from the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = NULL;
	string identifier;

	if (identifier_ptr)
	{
		identifier = identifier_ptr;
	}

	// log the action
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 1, "Searching for %s %s in Data Warehouse", WIDName(wid), identifier.c_str());
	}

	// try to update the stored object in the warehouse
	if ((wid_ptr = WAREHOUSE->retrieve(identifier, wid)) == NULL)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "Can't find %s %s in Data Warehouse", WIDName(wid), identifier.c_str());
		}
	}

	// return result
	return wid_ptr;

}

//>>===========================================================================

bool displayAttribute(LOG_CLASS *logger_ptr, BASE_SERIALIZER *serializer_ptr, BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr, UINT16 group, UINT16 element)

//  DESCRIPTION     : Function to display the given attribute in the object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check that we have a valid WID
	if (wid_ptr == NULL) return false;

	// get the attribute to display
	DCM_ATTRIBUTE_GROUP_CLASS *object_ptr = NULL;
	DCM_ATTRIBUTE_CLASS	*attribute_ptr = NULL;
	switch(wid_ptr->getWidType())
	{
	case WID_C_ECHO_RQ:
	case WID_C_ECHO_RSP:
	case WID_C_FIND_RQ:
	case WID_C_FIND_RSP:
	case WID_C_GET_RQ:
	case WID_C_GET_RSP:
	case WID_C_MOVE_RQ:
	case WID_C_MOVE_RSP:
	case WID_C_STORE_RQ:
	case WID_C_STORE_RSP:
	case WID_C_CANCEL_RQ:
	case WID_N_ACTION_RQ:
	case WID_N_ACTION_RSP:
	case WID_N_CREATE_RQ:
	case WID_N_CREATE_RSP:
	case WID_N_DELETE_RQ:
	case WID_N_DELETE_RSP:
	case WID_N_EVENT_REPORT_RQ:
	case WID_N_EVENT_REPORT_RSP:
	case WID_N_GET_RQ:
	case WID_N_GET_RSP:
	case WID_N_SET_RQ:
	case WID_N_SET_RSP:
	case WID_DATASET:
	case WID_ITEM:
			// retrieve the attribute from the dicom object
			object_ptr = static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(wid_ptr);
			if (object_ptr)
			{
				attribute_ptr = object_ptr->GetMappedAttribute(group, element);
			}
		break;

	case WID_ITEM_HANDLE:
			{
				// retrieve the dicom object (item) from the item handle
				ITEM_HANDLE_CLASS *itemHandle_ptr = static_cast<ITEM_HANDLE_CLASS*>(wid_ptr);
				object_ptr = (DCM_ATTRIBUTE_GROUP_CLASS*) itemHandle_ptr->resolveReference();
				if (object_ptr)
				{
					// retrieve the attribute from the dicom object
					attribute_ptr = object_ptr->GetMappedAttribute(group, element);
				}
			}
		break;

	default:
		break;
	}

	// check if we have found the attribute
	if ((logger_ptr) &&
		(object_ptr) &&
		(attribute_ptr))
	{
		// log action
		logger_ptr->text(LOG_SCRIPT, 2, "DISPLAY attribute (%04X,%04X) from %s %s", group, element, WIDName(wid_ptr->getWidType()), object_ptr->getIdentifier());

        // serialize it
        if (serializer_ptr)
        {
            serializer_ptr->SerializeDisplay(attribute_ptr);
        }
	}

	// return result
	return true;
}

//>>===========================================================================

bool compareAttributes(LOG_CLASS *logger_ptr, BASE_WAREHOUSE_ITEM_DATA_CLASS *wid1_ptr, UINT16 group1, UINT16 element1, BASE_WAREHOUSE_ITEM_DATA_CLASS *wid2_ptr, UINT16 group2, UINT16 element2)

//  DESCRIPTION     : Function to compare two attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool equal = false;

	// check that we have a valid WID
	if ((wid1_ptr == NULL) ||
		(wid2_ptr == NULL)) return false;

	// get the first attribute to compare
	DCM_ATTRIBUTE_GROUP_CLASS *object1_ptr = NULL;
	DCM_ATTRIBUTE_CLASS	*attribute1_ptr = NULL;
	switch(wid1_ptr->getWidType())
	{
	case WID_C_ECHO_RQ:
	case WID_C_ECHO_RSP:
	case WID_C_FIND_RQ:
	case WID_C_FIND_RSP:
	case WID_C_GET_RQ:
	case WID_C_GET_RSP:
	case WID_C_MOVE_RQ:
	case WID_C_MOVE_RSP:
	case WID_C_STORE_RQ:
	case WID_C_STORE_RSP:
	case WID_C_CANCEL_RQ:
	case WID_N_ACTION_RQ:
	case WID_N_ACTION_RSP:
	case WID_N_CREATE_RQ:
	case WID_N_CREATE_RSP:
	case WID_N_DELETE_RQ:
	case WID_N_DELETE_RSP:
	case WID_N_EVENT_REPORT_RQ:
	case WID_N_EVENT_REPORT_RSP:
	case WID_N_GET_RQ:
	case WID_N_GET_RSP:
	case WID_N_SET_RQ:
	case WID_N_SET_RSP:
	case WID_DATASET:
	case WID_ITEM:
			// retrieve the attribute from the dicom object
			object1_ptr = static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(wid1_ptr);
			if (object1_ptr)
			{
				attribute1_ptr = object1_ptr->GetMappedAttribute(group1, element1);
			}
		break;
	case WID_ITEM_HANDLE:
			{
				// retrieve the dicom object (item) from the item handle
				ITEM_HANDLE_CLASS *itemHandle_ptr = static_cast<ITEM_HANDLE_CLASS*>(wid1_ptr);
				object1_ptr = (DCM_ATTRIBUTE_GROUP_CLASS*) itemHandle_ptr->resolveReference();
				if (object1_ptr)
				{
					// retrieve the attribute from the dicom object
					attribute1_ptr = object1_ptr->GetMappedAttribute(group1, element1);
				}
			}
		break;

	default:
		break;
	}

	// get the second attribute to compare
	DCM_ATTRIBUTE_GROUP_CLASS *object2_ptr = NULL;
	DCM_ATTRIBUTE_CLASS	*attribute2_ptr = NULL;
	switch(wid2_ptr->getWidType())
	{
	case WID_C_ECHO_RQ:
	case WID_C_ECHO_RSP:
	case WID_C_FIND_RQ:
	case WID_C_FIND_RSP:
	case WID_C_GET_RQ:
	case WID_C_GET_RSP:
	case WID_C_MOVE_RQ:
	case WID_C_MOVE_RSP:
	case WID_C_STORE_RQ:
	case WID_C_STORE_RSP:
	case WID_C_CANCEL_RQ:
	case WID_N_ACTION_RQ:
	case WID_N_ACTION_RSP:
	case WID_N_CREATE_RQ:
	case WID_N_CREATE_RSP:
	case WID_N_DELETE_RQ:
	case WID_N_DELETE_RSP:
	case WID_N_EVENT_REPORT_RQ:
	case WID_N_EVENT_REPORT_RSP:
	case WID_N_GET_RQ:
	case WID_N_GET_RSP:
	case WID_N_SET_RQ:
	case WID_N_SET_RSP:
	case WID_DATASET:
	case WID_ITEM:
			// retrieve the attribute from the dicom object
			object2_ptr = static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(wid2_ptr);
			if (object2_ptr)
			{
				attribute2_ptr = object2_ptr->GetMappedAttribute(group2, element2);
			}
		break;

	case WID_ITEM_HANDLE:
			{
				// retrieve the dicom object (item) from the item handle
				ITEM_HANDLE_CLASS *itemHandle_ptr = static_cast<ITEM_HANDLE_CLASS*>(wid2_ptr);
				object2_ptr = (DCM_ATTRIBUTE_GROUP_CLASS*) itemHandle_ptr->resolveReference();
				if (object2_ptr)
				{
					// retrieve the attribute from the dicom object
					attribute2_ptr = object2_ptr->GetMappedAttribute(group2, element2);
				}
			}
		break;

	default:
		break;
	}

	// log the action
	if ((logger_ptr) &&
		(object1_ptr) &&
		(object2_ptr))
	{
		logger_ptr->text(LOG_SCRIPT, 2, "COMPARE attribute (%04X,%04X) from %s %s with attribute (%04X,%04X) from %s %s", group1, element1, WIDName(wid1_ptr->getWidType()), object1_ptr->getIdentifier(), group2, element2, WIDName(wid2_ptr->getWidType()), object2_ptr->getIdentifier());
	}

	// check if the attributes have been found
	if ((attribute1_ptr == NULL) || 
		(!attribute1_ptr->IsPresent()))
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Attribute (%04X,%04X) from %s %s not present for comparison", group1, element1, WIDName(wid1_ptr->getWidType()), object1_ptr->getIdentifier());
		}
	}
	else if ((attribute2_ptr == NULL) ||
		(!attribute2_ptr->IsPresent()))
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Attribute (%04X,%04X) from %s %s not present for comparison", group2, element2, WIDName(wid2_ptr->getWidType()), object2_ptr->getIdentifier());
		}
	}
	else
	{
		if (attribute1_ptr->CompareVRAndValues(attribute2_ptr) == MSG_EQUAL)
		{
			// attributes are equal
			equal = true;
		}
	}

	// return result
	return equal;
}

//>>===========================================================================

bool copyAttribute(LOG_CLASS *logger_ptr, BASE_WAREHOUSE_ITEM_DATA_CLASS *destWid_ptr, UINT16 destGroup, UINT16 destElement, BASE_WAREHOUSE_ITEM_DATA_CLASS *sourceWid_ptr, UINT16 sourceGroup, UINT16 sourceElement)

//  DESCRIPTION     : Function to copy one attribute to another from source to destination DICOM objects.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check that we have valid WIDs
	if ((sourceWid_ptr == NULL) ||
		(destWid_ptr == NULL)) return false;

	// get the source attribute to copy
	DCM_ATTRIBUTE_GROUP_CLASS *sourceObject_ptr = NULL;
	DCM_ATTRIBUTE_CLASS	*sourceAttribute_ptr = NULL;
	switch(sourceWid_ptr->getWidType())
	{
	case WID_C_ECHO_RQ:
	case WID_C_ECHO_RSP:
	case WID_C_FIND_RQ:
	case WID_C_FIND_RSP:
	case WID_C_GET_RQ:
	case WID_C_GET_RSP:
	case WID_C_MOVE_RQ:
	case WID_C_MOVE_RSP:
	case WID_C_STORE_RQ:
	case WID_C_STORE_RSP:
	case WID_C_CANCEL_RQ:
	case WID_N_ACTION_RQ:
	case WID_N_ACTION_RSP:
	case WID_N_CREATE_RQ:
	case WID_N_CREATE_RSP:
	case WID_N_DELETE_RQ:
	case WID_N_DELETE_RSP:
	case WID_N_EVENT_REPORT_RQ:
	case WID_N_EVENT_REPORT_RSP:
	case WID_N_GET_RQ:
	case WID_N_GET_RSP:
	case WID_N_SET_RQ:
	case WID_N_SET_RSP:
	case WID_DATASET:
	case WID_ITEM:
			// retrieve the attribute from the dicom object
			sourceObject_ptr = static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(sourceWid_ptr);
			if (sourceObject_ptr)
			{
				sourceAttribute_ptr = sourceObject_ptr->GetMappedAttribute(sourceGroup, sourceElement);
			}
		break;

	case WID_ITEM_HANDLE:
			{
				// retrieve the dicom object (item) from the item handle
				ITEM_HANDLE_CLASS *itemHandle_ptr = static_cast<ITEM_HANDLE_CLASS*>(sourceWid_ptr);
				sourceObject_ptr = (DCM_ATTRIBUTE_GROUP_CLASS*) itemHandle_ptr->resolveReference();
				if (sourceObject_ptr)
				{
					// retrieve the attribute from the dicom object
					sourceAttribute_ptr = sourceObject_ptr->GetMappedAttribute(sourceGroup, sourceElement);
				}
			}
		break;

	default:
		break;
	}

	// get the destination object to copy into
	DCM_ATTRIBUTE_GROUP_CLASS *destObject_ptr = NULL;
	switch(destWid_ptr->getWidType())
	{
	case WID_C_ECHO_RQ:
	case WID_C_ECHO_RSP:
	case WID_C_FIND_RQ:
	case WID_C_FIND_RSP:
	case WID_C_GET_RQ:
	case WID_C_GET_RSP:
	case WID_C_MOVE_RQ:
	case WID_C_MOVE_RSP:
	case WID_C_STORE_RQ:
	case WID_C_STORE_RSP:
	case WID_C_CANCEL_RQ:
	case WID_N_ACTION_RQ:
	case WID_N_ACTION_RSP:
	case WID_N_CREATE_RQ:
	case WID_N_CREATE_RSP:
	case WID_N_DELETE_RQ:
	case WID_N_DELETE_RSP:
	case WID_N_EVENT_REPORT_RQ:
	case WID_N_EVENT_REPORT_RSP:
	case WID_N_GET_RQ:
	case WID_N_GET_RSP:
	case WID_N_SET_RQ:
	case WID_N_SET_RSP:
	case WID_DATASET:
	case WID_ITEM:
			// retrieve the destination object
			destObject_ptr = static_cast<DCM_ATTRIBUTE_GROUP_CLASS*>(destWid_ptr);
		break;

	case WID_ITEM_HANDLE:
			{
				// retrieve the dicom object (item) from the item handle
				ITEM_HANDLE_CLASS *itemHandle_ptr = static_cast<ITEM_HANDLE_CLASS*>(destWid_ptr);
				destObject_ptr = (DCM_ATTRIBUTE_GROUP_CLASS*) itemHandle_ptr->resolveReference();
			}
		break;

	default:
		break;
	}

	// check that we have valid objects
	if ((sourceObject_ptr == NULL) ||
		(destObject_ptr == NULL)) return false;

	// log the action
	if (logger_ptr)
	{
		logger_ptr->text(LOG_SCRIPT, 2, "COPY attribute (%04X,%04X) from %s %s to attribute (%04X,%04X) in %s %s", sourceGroup, sourceElement, WIDName(sourceWid_ptr->getWidType()), sourceObject_ptr->getIdentifier(), destGroup, destElement, WIDName(destWid_ptr->getWidType()), destObject_ptr->getIdentifier());
	}

	if ((sourceAttribute_ptr == NULL) ||
		(!sourceAttribute_ptr->IsPresent()))
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Attribute (%04X,%04X) from %s %s not present for copy", sourceGroup, sourceElement, WIDName(sourceWid_ptr->getWidType()), sourceObject_ptr->getIdentifier());
		}
		return false;
	}

	// check if we are copying the same attribute within the same DICOM object
	if ((sourceObject_ptr == destObject_ptr) &&
		(sourceGroup == destGroup) &&
		(sourceElement == destElement)) return true;

	// remove any existing attribute from destination object
	destObject_ptr->DeleteMappedAttribute(destGroup, destElement);

	// allocate the destination attribute
	DCM_ATTRIBUTE_CLASS	*destAttribute_ptr = new DCM_ATTRIBUTE_CLASS(destGroup, destElement);
	if (!destAttribute_ptr) return false;

	// copy the source attribute value to the destination
	*destAttribute_ptr = *sourceAttribute_ptr;

	// add the destination attribute to the destination object
	destObject_ptr->addAttribute(destAttribute_ptr);

	// set the nesting depth for logging purposes
	destObject_ptr->setNestingDepth();

	// sort the attributes too
	destObject_ptr->SortAttributes();

	// return result
	return true;
}

//>>===========================================================================

bool readFileDataset(SCRIPT_SESSION_CLASS *session_ptr, string filename_in, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Function to read the dataset from the given file
//					: into the warehouse using the given identifier.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS*  logger_ptr = session_ptr->getLogger();
	string filename = filename_in;
	string identifier;
	
	if (dataset_ptr->getIdentifier())
	{
		identifier = dataset_ptr->getIdentifier();
	}

	// log the action
	if (logger_ptr)
	{
		logger_ptr->text(LOG_SCRIPT, 2, "READ %s %s from %s (into Data Warehouse)", WIDName(dataset_ptr->getWidType()), identifier.c_str(), filename.c_str());
	}

	// see if a path has been defined
	// if there is no path assume the file is in the script or session directory
	if (!isAbsolutePath(filename))
	{
		//check to see if exists in script directory and, if not, then in the session directory
		string pathname;

		pathname = session_ptr->getDicomScriptRoot() + filename;

		if (_access(pathname.c_str(),0) != 0)
		{	
			pathname = session_ptr->getSessionDirectory() + filename;
		}	

		// copy filename
		filename = pathname;
	}

	// set up the file read
	FILE_DATASET_CLASS	fileDataset(filename);

	// cascade the logger
	fileDataset.setLogger(logger_ptr);

    // save the pixel data only - for comparison purposes with dataset from emulator
    fileDataset.setStorageMode(SM_TEMPORARY_PIXEL_ONLY);

	// read the dataset from the file
	bool result = fileDataset.read(dataset_ptr);
	if (!result)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Can't read %s %s from file %s into Data Warehouse", 
                WIDName(dataset_ptr->getWidType()), 
                identifier.c_str(), 
                filename.c_str());
		}
	}
	else
	{
		// set the iod name to the filename for now
        if (dataset_ptr->getIodName() == NULL)
        {
    		dataset_ptr->setIodName((char*) filename_in.c_str());
        }

		// try to store the object in the warehouse
		result = WAREHOUSE->store(identifier, dataset_ptr);
		if (!result)
		{
			if (logger_ptr)
			{
				logger_ptr->text(LOG_ERROR, 1, "Can't store %s %s in Data Warehouse", WIDName(dataset_ptr->getWidType()), identifier.c_str());
			}
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool readFileDataset(SCRIPT_SESSION_CLASS *session_ptr, string filename_in, UINT32 tag)

//  DESCRIPTION     : Function to read the dataset from the given file and
//					: store it in the warehouse with the given tag value as the
//					: identifier.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS*  logger_ptr = session_ptr->getLogger();
	string filename = filename_in;

	// see if a path has been defined
	// if there is no path assume the file is in the script or session directory
	if (!isAbsolutePath(filename))
	{
		//check to see if exists in script directory and, if not, then in the session directory
		string pathname;

		pathname = session_ptr->getDicomScriptRoot() + filename;

		if (_access(pathname.c_str(),0) != 0)
		{	
			pathname = session_ptr->getSessionDirectory() + filename;
		}	

		// copy filename
		filename = pathname;
	}

	// set up the file read
	FILE_DATASET_CLASS	fileDataset(filename);

	// cascade the logger
	fileDataset.setLogger(logger_ptr);

    // save the pixel data only - for comparison purposes with dataset from emulator
    fileDataset.setStorageMode(SM_TEMPORARY_PIXEL_ONLY);

	// read the dataset from the file
	DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();
	bool result = fileDataset.read(dataset_ptr);
	if (!result)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Can't read %s using identifier tag 0x%08X from %s into Data Warehouse", WIDName(dataset_ptr->getWidType()), tag, filename_in.c_str());
		}
	}
	else
	{
		// set the identifying tag value from within the dataset
		if (dataset_ptr->setIdentifierByTag(tag))
		{
			// try to store the object in the warehouse
			result = WAREHOUSE->store(dataset_ptr->getIdentifier(), dataset_ptr);
			if (result)
			{
				// set reference tag
				WAREHOUSE->setReferenceTag(tag);

				// log the action
				if (logger_ptr)
				{
					logger_ptr->text(LOG_SCRIPT, 2, "READ %s using identifier %s (tag 0x%08X) from %s (into Data Warehouse)", WIDName(dataset_ptr->getWidType()), dataset_ptr->getIdentifier(), tag, filename_in.c_str());
				}
			}
			else
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Can't store %s %s in Data Warehouse", WIDName(dataset_ptr->getWidType()), dataset_ptr->getIdentifier());
				}
			}
		}
		else
		{
			// can't get required identifier from dataset
			if (logger_ptr)
			{
				logger_ptr->text(LOG_ERROR, 1, "Can't find identifier tag 0x%08X in %s - read from %s", tag, WIDName(dataset_ptr->getWidType()), filename_in.c_str());
			}
			result = false;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool writeFileHead(LOG_CLASS *logger_ptr, string filename, bool autoCreateDirectory)

//  DESCRIPTION     : Function to write the file head to the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// try to retrive the file head from the warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve("", WID_FILEHEAD);
	if (wid_ptr)
	{
		// log the action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "WRITE %s (from Data Warehouse) to %s", WIDName(WID_FILEHEAD), filename.c_str());
		}

		FILEHEAD_CLASS *filehead_ptr = static_cast<FILEHEAD_CLASS*>(wid_ptr);

		// set up the write file
		filehead_ptr->setFilename(correctPathnameForOS(filename));

		// write the file head to the file
		result = filehead_ptr->write(autoCreateDirectory);
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "Can't write %s from Data Warehouse to %s", WIDName(WID_FILEHEAD), filename.c_str());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool writeFileTail(LOG_CLASS *logger_ptr, string filename, bool autoCreateDirectory)

//  DESCRIPTION     : Function to write the file tail to the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// try to retrive the file tail from the warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve("", WID_FILETAIL);
	if (wid_ptr)
	{
		// log the action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "WRITE %s (from Data Warehouse) to %s", WIDName(WID_FILETAIL), filename.c_str());
		}

		FILETAIL_CLASS *filetail_ptr = static_cast<FILETAIL_CLASS*>(wid_ptr);

		// set up the write file
		filetail_ptr->setFilename(correctPathnameForOS(filename));

		// write the file tail to the file
		result = filetail_ptr->write(autoCreateDirectory);
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_DEBUG, 1, "Can't write %s from Data Warehouse to %s", WIDName(WID_FILETAIL), filename.c_str());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool writeFileDataset(LOG_CLASS *logger_ptr, string filename, DCM_DATASET_CLASS *dataset_ptr, bool autoCreateDirectory)

//  DESCRIPTION     : Function to write the dataset to the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	string identifier;

	if (dataset_ptr->getIdentifier())
	{
		identifier = dataset_ptr->getIdentifier();
	}

	// try to retrive the file head from the warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(identifier, dataset_ptr->getWidType());
	if (wid_ptr)
	{
		// log the action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "WRITE %s %s (from Data Warehouse) to %s", WIDName(wid_ptr->getWidType()), identifier.c_str(), filename.c_str());
		}

		DCM_DATASET_CLASS *writeDataset_ptr = static_cast<DCM_DATASET_CLASS*>(wid_ptr);
	
		// set up the write file
		FILE_DATASET_CLASS	fileDataset(correctPathnameForOS(filename));

		// cascade the logger
		fileDataset.setLogger(logger_ptr);

		// write the dataset to the file
		result = fileDataset.write(writeDataset_ptr, autoCreateDirectory);
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Can't write %s from Data Warehouse to %s", identifier.c_str(), filename.c_str());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool receiveAcse(SCRIPT_SESSION_CLASS *session_ptr, ASSOCIATE_RQ_CLASS *associateRq_ptr, string identifier)

//  DESCRIPTION     : Function to receive an Associate Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to receive an associate request
	return session_ptr->receive(associateRq_ptr, identifier);
}

//>>===========================================================================

bool receiveAcse(SCRIPT_SESSION_CLASS *session_ptr, ASSOCIATE_AC_CLASS *associateAc_ptr, string identifier)

//  DESCRIPTION     : Function to receive an Associate Accept.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to receive an associate accept
	return session_ptr->receive(associateAc_ptr, identifier);
}

//>>===========================================================================

bool receiveAcse(SCRIPT_SESSION_CLASS *session_ptr, ASSOCIATE_RJ_CLASS *associateRj_ptr, string identifier)

//  DESCRIPTION     : Function to receive an Associate Reject.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to receive an associate reject
	return session_ptr->receive(associateRj_ptr, identifier);
}

//>>===========================================================================

bool receiveAcse(SCRIPT_SESSION_CLASS *session_ptr, RELEASE_RQ_CLASS *releaseRq_ptr, string identifier)

//  DESCRIPTION     : Function to receive a Release Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to receive a release request
	return session_ptr->receive(releaseRq_ptr, identifier);
}

//>>===========================================================================

bool receiveAcse(SCRIPT_SESSION_CLASS *session_ptr, RELEASE_RP_CLASS *releaseRp_ptr, string identifier)

//  DESCRIPTION     : Function to receive a Release Response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to receive a release response
	return session_ptr->receive(releaseRp_ptr, identifier);
}

//>>===========================================================================

bool receiveAcse(SCRIPT_SESSION_CLASS *session_ptr, ABORT_RQ_CLASS *abortRq_ptr, string identifier)

//  DESCRIPTION     : Function to receive an Abort Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to receive an abort request
	return session_ptr->receive(abortRq_ptr, identifier);
}

//>>===========================================================================

bool receiveAcse(SCRIPT_SESSION_CLASS *session_ptr, UNKNOWN_PDU_CLASS *unknownPdu_ptr, string identifier)

//  DESCRIPTION     : Function to receive an Unknown Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to receive an unknown pdu
	return session_ptr->receive(unknownPdu_ptr, identifier);
}

//>>===========================================================================

bool receiveSop(SCRIPT_SESSION_CLASS *session_ptr, 
				DCM_COMMAND_CLASS*       ref_command_ptr,
				DCM_DATASET_CLASS*       ref_dataset_ptr)

//  DESCRIPTION     : Function to receive a DICOM message and validate it
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool result = false;
	DCM_COMMAND_CLASS* rxCommand_ptr = NULL;
	DCM_DATASET_CLASS* rxDataset_ptr = NULL;

	// receive the command and dataset
	if (datasetexpected[session_ptr] == true )
    {
	    result = session_ptr->receive(&rxCommand_ptr, &rxDataset_ptr, ref_command_ptr, ref_dataset_ptr);
    }
	else
	{
        result = session_ptr->receive(&rxCommand_ptr, ref_command_ptr);
	}

	// validate the command and dataset
	if (result)
	{
		result = validateSopAgainstList(session_ptr, rxCommand_ptr, rxDataset_ptr);
	}

	// receive cleanup
	delete rxCommand_ptr;
	delete rxDataset_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool importCommand(SCRIPT_SESSION_CLASS* session_ptr, 
				   DIMSE_CMD_ENUM        cmd, 
				   string                cmd_identifier)

//  DESCRIPTION     : Function to receive a DICOM message and validate it
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// import command only
	return session_ptr->importCommand(cmd, cmd_identifier);
}

//>>===========================================================================

bool importCommandDataset(SCRIPT_SESSION_CLASS* session_ptr, 
						  DIMSE_CMD_ENUM        cmd, 
						  string                cmd_identifier,
						  string                iodname,
						  string                data_identifier)

//  DESCRIPTION     : Function to receive a DICOM message and validate it
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// import command and dataset
	return session_ptr->importCommandDataset(cmd, cmd_identifier, iodname, data_identifier);
}

//>>===========================================================================

bool sendAcse(SCRIPT_SESSION_CLASS *session_ptr, ASSOCIATE_RQ_CLASS *associateRq_ptr, string identifier)

//  DESCRIPTION     : Function to send an Associate Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to send an associate request
	return session_ptr->send(associateRq_ptr, identifier);
}

//>>===========================================================================

bool sendAcse(SCRIPT_SESSION_CLASS *session_ptr, ASSOCIATE_AC_CLASS *associateAc_ptr, string identifier)

//  DESCRIPTION     : Function to send an Associate Accept.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to send an associate accept
	return session_ptr->send(associateAc_ptr, identifier);
}

//>>===========================================================================

bool sendAcse(SCRIPT_SESSION_CLASS *session_ptr, ASSOCIATE_RJ_CLASS *associateRj_ptr, string identifier)

//  DESCRIPTION     : Function to send an Associate Reject.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to send an associate reject
	return session_ptr->send(associateRj_ptr, identifier);
}

//>>===========================================================================

bool sendAcse(SCRIPT_SESSION_CLASS *session_ptr, RELEASE_RQ_CLASS *releaseRq_ptr, string identifier)

//  DESCRIPTION     : Function to send a Release Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to send a release request
	return session_ptr->send(releaseRq_ptr, identifier);
}

//>>===========================================================================

bool sendAcse(SCRIPT_SESSION_CLASS *session_ptr, RELEASE_RP_CLASS *releaseRp_ptr, string identifier)

//  DESCRIPTION     : Function to send a Release Response.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to send a release response
	return session_ptr->send(releaseRp_ptr, identifier);
}

//>>===========================================================================

bool sendAcse(SCRIPT_SESSION_CLASS *session_ptr, ABORT_RQ_CLASS *abortRq_ptr, string identifier)

//  DESCRIPTION     : Function to send an Abort Request.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to send an abort request
	return session_ptr->send(abortRq_ptr, identifier);
}

//>>===========================================================================

bool sendAcse(SCRIPT_SESSION_CLASS *session_ptr, UNKNOWN_PDU_CLASS *unknownPdu_ptr, string identifier)

//  DESCRIPTION     : Function to send an Unknown Pdu.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// try to send an unknown pdu
	return session_ptr->send(unknownPdu_ptr, identifier);
}

//>>===========================================================================

bool sendSop(SCRIPT_SESSION_CLASS *session_ptr, DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Function to send a DICOM message.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check if both command and dataset are defined
	if ((command_ptr) && 
		(dataset_ptr))
	{
		// remove any Dataset Trailing Padding
		dataset_ptr->removeTrailingPadding();

		// try to send command and dataset
		result = session_ptr->send(command_ptr, dataset_ptr);
	}
	else if (command_ptr)
	{
		// try to send command only
		result = session_ptr->send(command_ptr);
	}

	// return result
	return result;
}

//>>===========================================================================

//>>===========================================================================

void setLogicalOperand(OPERAND_ENUM op)

//  DESCRIPTION     : Function to set the logical operand for the validation list
//                    0 : OR (default)
//                    1 : AND
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    operand = op;
}

//>>===========================================================================

void addReferenceObjects(SCRIPT_SESSION_CLASS *session_ptr, DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr, LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Function to add a pair of reference objects to the
//                    list of reference objects which is used to validate an object
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	OBJECT_PAIR_STRUCT ref_objs;

	ref_objs.session_ptr = session_ptr;
	ref_objs.command_ptr = command_ptr;
	ref_objs.dataset_ptr = dataset_ptr;

	if (command_ptr)
	{
		if (command_ptr->getIdentifier())
		{
	        logger_ptr->text(LOG_DEBUG, 1, "Adding reference object(s): %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier());
		}
		else
		{
	        logger_ptr->text(LOG_DEBUG, 1, "Adding reference object(s): %s", mapCommandName(command_ptr->getCommandId()));
		}
	}
	if (dataset_ptr)
	{
		if (dataset_ptr->getIdentifier())
		{
			logger_ptr->text(LOG_DEBUG, 1, " and \"%s\" \"%s\"", dataset_ptr->getIodName(), dataset_ptr->getIdentifier());
		}
		else
		{
	        logger_ptr->text(LOG_DEBUG, 1, " and \"%s\"", dataset_ptr->getIodName());
		}
		datasetexpected[session_ptr] = true;
	}

	ref_objects.push_back(ref_objs);
}

//>>===========================================================================

void clearValidationObjects(SCRIPT_SESSION_CLASS* session_ptr)

//  DESCRIPTION     : Clears the source and reference objects 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    vector<OBJECT_PAIR_STRUCT>::iterator ref_objects_it;

label1:

	for (ref_objects_it = ref_objects.begin(); ref_objects_it < ref_objects.end(); ref_objects_it++)
	{
		if (ref_objects_it->session_ptr == session_ptr)
		{
			ref_objects.erase(ref_objects_it);
			datasetexpected[session_ptr] = false;
			goto label1;
		}
	}
}


//>>===========================================================================

bool validateSopAgainstList(SCRIPT_SESSION_CLASS *session_ptr, DCM_COMMAND_CLASS* command_ptr, DCM_DATASET_CLASS* dataset_ptr)

//  DESCRIPTION     : Function to validate the given command and dataset initially against the
//					  corresponding definitions and additionally against the reference command
//					  and dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	LOG_CLASS *logger_ptr = session_ptr->getLogger();
	DIMSE_CMD_ENUM cmd;
	AE_SESSION_CLASS ae_session;
    DCM_COMMAND_CLASS* ref_command_ptr = NULL;
	DCM_DATASET_CLASS* ref_dataset_ptr = NULL;   

	// set ae session
    // and the validation control flag
	ae_session.SetName(session_ptr->getApplicationEntityName());
	ae_session.SetVersion(session_ptr->getApplicationEntityVersion());
   	VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
    if (session_ptr->getScriptExecutionContext())
    {
        ae_session.SetName(session_ptr->getScriptExecutionContext()->getApplicationEntityName());
        ae_session.SetVersion(session_ptr->getScriptExecutionContext()->getApplicationEntityVersion());
		validationFlag = session_ptr->getScriptExecutionContext()->getValidationFlag();
    }

   	// we need at least a command 
	// - dataset may be null
	if (command_ptr)
	{
        // validate against definition only       
		if (ref_objects.size() == 0)
		{
			// check if a dataset is available
			if (dataset_ptr)
			{
				// validate command and dataset
				result = session_ptr->validate(command_ptr, dataset_ptr, NULL, NULL, validationFlag, &ae_session);
			}
			else
			{
				// validate command only
				result = session_ptr->validate(command_ptr, NULL, validationFlag);
			}
		}
		else
		{
            // validate against reference objects
            for (UINT counter=0 ; counter<ref_objects.size(); counter++)
            {
                if (ref_objects[counter].session_ptr == session_ptr)
				{
					// re-initialize variables
					ref_command_ptr = NULL;
					ref_dataset_ptr = NULL;
					if (ref_objects[counter].command_ptr != NULL)
					{
						cmd = ref_objects[counter].command_ptr->getCommandId();
					
						// for VTS we need to retrieve the object from the warehouse
						ref_command_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(session_ptr->getLogger(),
																						ref_objects[counter].command_ptr->getIdentifier(),
																						WAREHOUSE->dimse2widtype(cmd)));

						// we found the object in the warehouse
						if (ref_command_ptr != NULL)
						{
							// register any VTS style uid mappings
							registerVTSUidMappings(command_ptr, ref_command_ptr, session_ptr->getLogger());
						}
						else
						{
							// the object was set in-line (ADVT style)
							ref_command_ptr = ref_objects[counter].command_ptr;
						}
					}

					if (ref_objects[counter].dataset_ptr != NULL)
					{
						ref_dataset_ptr = static_cast<DCM_DATASET_CLASS*>(retrieveFromWarehouse(session_ptr->getLogger(),
																						ref_objects[counter].dataset_ptr->getIdentifier(),
																						WID_DATASET));
						
						// we found the object in the warehouse
						if (ref_dataset_ptr != NULL)
						{
							// register any VTS style uid mappings
							registerVTSUidMappings(dataset_ptr, ref_dataset_ptr, session_ptr->getLogger());
						}
						else
						{
							// the object was set in-line (ADVT style)
							ref_dataset_ptr = ref_objects[counter].dataset_ptr;
						}
					}

					if (logger_ptr)
					{
						// check if a dataset is received but not referenced
						if ((dataset_ptr) &&
							(!ref_dataset_ptr))
						{
							logger_ptr->text(LOG_DEBUG, 1, "Dataset received but no reference in DICOMScript for validation");
						}
						else if ((!dataset_ptr) &&
							(ref_dataset_ptr))
						{
							logger_ptr->text(LOG_DEBUG, 1, "No dataset received for validation against reference in DICOMScript");
							result = true;
						}
					}

					// check that there are attributes defined in the reference objects
					// - if not set them NULL - validation algorithm expects NULL
					if ((ref_command_ptr) &&
						(ref_command_ptr->GetNrAttributes() == 0))
					{
						// force it NULL for validation
						ref_command_ptr = NULL;
					}
					if ((ref_dataset_ptr) &&
						(ref_dataset_ptr->GetNrAttributes() == 0))
					{
						// force it NULL for validation
						ref_dataset_ptr = NULL;
					}

					if (operand == OPERAND_OR)
					{
						// check for command and dataset
						if ((command_ptr) &&
							(dataset_ptr))
						{
							// validate command and dataset
							result |= session_ptr->validate(command_ptr, dataset_ptr, 
														ref_command_ptr, ref_dataset_ptr, validationFlag, &ae_session);
						}
						else if (command_ptr)
						{
							// validate command only
							result |= session_ptr->validate(command_ptr, ref_command_ptr, validationFlag);
						}
					}
					else if (operand == OPERAND_AND)
					{
						// check for command and dataset
						if ((command_ptr) &&
							(dataset_ptr))
						{
							// validate command and dataset
							result &= session_ptr->validate(command_ptr, dataset_ptr, 
														ref_command_ptr, ref_dataset_ptr, validationFlag, &ae_session);
						}
						else if (command_ptr)
						{
							// validate command only
							result &= session_ptr->validate(command_ptr, ref_command_ptr, validationFlag);
						}
					}
				}
			}
		}
	}

	// clear list with reference objects for this session
	clearValidationObjects(session_ptr);

	// return result
	return result;
}

//>>===========================================================================

bool systemCall(SCRIPT_SESSION_CLASS *session_ptr, char *systemCall)

//  DESCRIPTION     : Function to the SYSTEM command by making a system call to the OS.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Call must be made from session directory.
//<<===========================================================================
{
	bool result = true;

	LOG_CLASS *logger_ptr = session_ptr->getLogger();

	if (logger_ptr)
	{
		logger_ptr->text(LOG_SCRIPT, 2, "SYSTEM call \"%s\"", systemCall);
	}

	// save current working directory
	string cwd = getCurrentWorkingDirectory();

	// change to the session directory
	_chdir(session_ptr->getDicomScriptRoot());

	// log directory used with system call
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 1, "SYSTEM call made in directory: \"%s\"", session_ptr->getDicomScriptRoot());
	}

	// make system call with string (command) provided
	if (system(systemCall) == -1)
	{
		// system call failed
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "SYSTEM call failed: \"%s\"", systemCall);
		}
		result = false;
	}

	// restore current working directory
	_chdir(cwd.c_str());

	// return result
	return result;
}

//>>===========================================================================

BASE_VALUE_CLASS *stringValue(SCRIPT_SESSION_CLASS *session_ptr, char *stringValue_ptr, ATTR_VR_ENUM vr, UINT16 group, UINT16 element)

//  DESCRIPTION     : Function to handle Attribute "String" type values.
//					: The appropriate Attribute VR Type Value is
//					: instantiated with the given string value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS *logger_ptr = session_ptr->getLogger();
	BASE_VALUE_CLASS *value_ptr = NULL;
	UINT length = strlen(stringValue_ptr);

	switch (vr) 
	{
	case ATTR_VR_AE: 
	{
		string data = stringValue_ptr;

		// check for label value mapping
		char *mappedValue_ptr = WAREHOUSE->setLabelledValue(stringValue_ptr, group, element, logger_ptr);
		if (mappedValue_ptr)
		{
			// set the data up as the mapped value
			data = mappedValue_ptr;
		}

		// now check for VR keywords
		if (data == "CALLEDAETITLE")
		{
			// set the value to the called ae title
			data = session_ptr->getCalledAeTitle();
		}
		else if (data == "CALLINGAETITLE")
		{
			// set the value to the calling ae title
			data = session_ptr->getCallingAeTitle();
		}

		// allocate a new vr value
		value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
	}
	break;

	case ATTR_VR_AS:
	case ATTR_VR_CS:
	case ATTR_VR_DS:
	case ATTR_VR_IS:
	case ATTR_VR_LO:
	case ATTR_VR_LT:
	case ATTR_VR_PN:
	case ATTR_VR_SH:
	case ATTR_VR_ST:
	case ATTR_VR_UI:
    case ATTR_VR_UN:
	case ATTR_VR_UT:
	case ATTR_VR_UR:
	case ATTR_VR_UC:
	{
		string data = stringValue_ptr;

		// check for label value mapping
		char *mappedValue_ptr = WAREHOUSE->setLabelledValue(stringValue_ptr, group, element, logger_ptr);
		if (mappedValue_ptr)
		{
			// mapping found - take mapped value
			data = mappedValue_ptr;
		}
		else if (vr == ATTR_VR_UI)
		{
			// special case for UI
			// try getting an abstract mapping
			string uid = session_ptr->getSopUid(data);
			if (uid.length())
			{
				// take abstract mapping found
				data = uid;
			}
		}

		// allocate a new vr value
		value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
	}
	break;

	case ATTR_VR_DA: 
	{
		char tempDate[64];
		strcpy(tempDate, stringValue_ptr);

	   	// check for date mapping
		length = mapDate(tempDate);
		string data = tempDate;

		// check for label value mapping
		char *mappedValue_ptr = WAREHOUSE->setLabelledValue(tempDate, group, element, logger_ptr);
		if (mappedValue_ptr)
		{
			// mapping found - take mapped value
			data = mappedValue_ptr;
		}

		// allocate a new vr value
		value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
	}
	break;

	case ATTR_VR_DT:
	{
		char tempDateTime[64];
		strcpy(tempDateTime, stringValue_ptr);

		// check for date & time mapping
	    length = mapDate(tempDateTime);
	    length = mapTime(tempDateTime);
    	string data = tempDateTime;

		// check for label value mapping
	    char *mappedValue_ptr = WAREHOUSE->setLabelledValue(tempDateTime, group, element, logger_ptr);
	    if (mappedValue_ptr)
	    {
	    	// mapping found - take mapped value
    		data = mappedValue_ptr;
    	}

    	// allocate a new vr value
    	value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
	}
	break;

	case ATTR_VR_FD: 
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			double data = atof(stringValue_ptr);

			// allocate a new vr value
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
	}
	break;

	case ATTR_VR_FL: 
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			float data = (float) atof(stringValue_ptr);

			// allocate a new vr value
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
	}
	break;

	case ATTR_VR_TM: 
	{
		char tempTime[64];
		strcpy(tempTime, stringValue_ptr);

	    // check for time mapping
	    length = mapTime(tempTime);
    	string data = tempTime;

	    // check for label value mapping
	    char *mappedValue_ptr = WAREHOUSE->setLabelledValue(tempTime, group, element, logger_ptr);
	    if (mappedValue_ptr)
	    {
	    	// mapping found - take mapped value
    		data = mappedValue_ptr;
    	}

    	// allocate a new vr value
    	value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
	}
	break;

	case ATTR_VR_OB:
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			// string is a filename - convert it into an absolute pathname
			string data = session_ptr->getAbsolutePixelPathname(stringValue_ptr);

			// allocate a new vr value
			VALUE_OB_CLASS *ob_value_ptr = (VALUE_OB_CLASS*)CreateNewValue(vr);
            ob_value_ptr->SetLogger(logger_ptr);
			ob_value_ptr->Set(data);
            value_ptr = ob_value_ptr;
		}
	}
	break;

	case ATTR_VR_OF:
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			// string is a filename - convert it into an absolute pathname
			string data = session_ptr->getAbsolutePixelPathname(stringValue_ptr);

			// allocate a new vr value
			VALUE_OF_CLASS *of_value_ptr = (VALUE_OF_CLASS*)CreateNewValue(vr);
            of_value_ptr->SetLogger(logger_ptr);
			of_value_ptr->Set(data);
            value_ptr = of_value_ptr;
		}
	}
	break;

	case ATTR_VR_OD:
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			// string is a filename - convert it into an absolute pathname
			string data = session_ptr->getAbsolutePixelPathname(stringValue_ptr);

			// allocate a new vr value
			VALUE_OD_CLASS *od_value_ptr = (VALUE_OD_CLASS*)CreateNewValue(vr);
            od_value_ptr->SetLogger(logger_ptr);
			od_value_ptr->Set(data);
            value_ptr = od_value_ptr;
		}
	}
	break;

	case ATTR_VR_OW:
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			// string is a filename - convert it into an absolute pathname
			string data = session_ptr->getAbsolutePixelPathname(stringValue_ptr);

			// allocate a new vr value
			VALUE_OW_CLASS *ow_value_ptr = (VALUE_OW_CLASS*)CreateNewValue(vr);
            ow_value_ptr->SetLogger(logger_ptr);
			ow_value_ptr->Set(data);
            value_ptr = ow_value_ptr;
		}
	}
	break;

	case ATTR_VR_OL:
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			// string is a filename - convert it into an absolute pathname
			string data = session_ptr->getAbsolutePixelPathname(stringValue_ptr);

			// allocate a new vr value
			VALUE_OL_CLASS *ol_value_ptr = (VALUE_OL_CLASS*)CreateNewValue(vr);
            ol_value_ptr->SetLogger(logger_ptr);
			ol_value_ptr->Set(data);
            value_ptr = ol_value_ptr;
		}
	}
	break;
	
	case ATTR_VR_OV:
	{
		// check if value defined - otherwise just treat as zero-length
		if (length)
		{
			// string is a filename - convert it into an absolute pathname
			string data = session_ptr->getAbsolutePixelPathname(stringValue_ptr);

			// allocate a new vr value
			VALUE_OV_CLASS *ov_value_ptr = (VALUE_OV_CLASS*)CreateNewValue(vr);
            ov_value_ptr->SetLogger(logger_ptr);
			ov_value_ptr->Set(data);
            value_ptr = ov_value_ptr;
		}
	}
	break;

	case ATTR_VR_AT:
		// check if value defined - otherwise just treat as zero-length
		if ((length) &&
			(logger_ptr))
		{
			logger_ptr->text(LOG_ERROR, 1, "AT Value in DICOMScript should be expressed in HEX format for attribute (%04X,%04X)", group, element);
			logger_ptr->text(LOG_NONE, 1, "Value not used - zero length taken");
		}
	break;

	case ATTR_VR_SL:
		{
			// check if value defined - otherwise just treat as zero-length
			if (length)
			{
				// VTS support - will not be documented
				// - need to convert string to SL value
				INT32 data = (INT32) atoi(stringValue_ptr);

				// allocate a new vr value
				value_ptr = CreateNewValue(vr);
				value_ptr->Set(data);
			}
		}
	break;

	case ATTR_VR_SS:
		{
			// check if value defined - otherwise just treat as zero-length
			if (length)
			{
				// VTS support - will not be documented
				// - need to convert string to SS value
				INT16 data = (INT16) atoi(stringValue_ptr);

				// allocate a new vr value
				value_ptr = CreateNewValue(vr);
				value_ptr->Set(data);
			}
		}
	break;

	case ATTR_VR_UL:
		{
			UINT32 tag = (group << 16) + element;

			// check if the tag is one of the directory record tags
			switch(tag) {
			case TAG_OFFSET_OF_THE_FIRST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY:
			case TAG_OFFSET_OF_THE_LAST_DIRECTORY_RECORD_OF_THE_ROOT_DIRECTORY_ENTITY:
			case TAG_OFFSET_OF_THE_NEXT_DIRECTORY_RECORD:
			case TAG_OFFSET_OF_REFERENCED_LOWER_LEVEL_DIRECTORY_ENTITY:
			case TAG_MRDR_DIRECTORY_RECORD_OFFSET:
				{
					// instantiate a value - used later to index the actual computed value
					DCM_VALUE_UL_CLASS *valueUL_ptr = new DCM_VALUE_UL_CLASS();
					valueUL_ptr->setIdentifier(stringValue_ptr);
					value_ptr = valueUL_ptr;
				}
				break;

			default:
				{
					// check if value defined - otherwise just treat as zero-length
					if (length)
					{
						// VTS support - will not be documented
						// - need to convert string to UL value
						UINT32 data = (UINT32) atoi(stringValue_ptr);
						DCM_VALUE_UL_CLASS *valueUL_ptr = new DCM_VALUE_UL_CLASS();
						valueUL_ptr->Set(data);
						value_ptr = valueUL_ptr;
					}
				}
				break;
			}
		}
	break;

	case ATTR_VR_US:
		{
			// check if value defined - otherwise just treat as zero-length
			if (length)
			{
				// VTS support - will not be documented
				// - need to convert string to US value
				UINT16 data = (UINT16) atoi(stringValue_ptr);

				// allocate a new vr value
				value_ptr = CreateNewValue(vr);
				value_ptr->Set(data);
			}
		}
	break;

	default:
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR,1, "Unknown VR in DICOMScript for attribute (%04X,%04X)", group, element);
			logger_ptr->text(LOG_NONE, 1, "Value not used - zero length taken");
		}
	break;
	}

	// return the value
	return value_ptr;
}

//>>===========================================================================

BASE_VALUE_CLASS *byteArrayValue(char *stringValue_ptr)

//  DESCRIPTION     : Function to handle Attribute "ByteArray" type values. Only applicable to the VR or UN.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// convert the incoming string in a way similar to the convertHex but support NULL -
	// return a pointer to the byte array (unsigned char*) and it's length
	UINT32 length = 0;
	unsigned char *byteArray_ptr = convertStringToByteArray(stringValue_ptr, &length);
/*

DO SOMETHING LIKE utility convertHex() below as convertStringToByteArray() but support NULL in array and get correct length

//>>===========================================================================

char* convertHex(char* string)

//  DESCRIPTION     : Convert any hex values (indicated by (XX) or \XX) to integers
//  PRECONDITIONS   :
//  POSTCONDITIONS  : The string is converted in place and returned
//  EXCEPTIONS      : 
//  NOTES           : A '?' is returned for an invalid hexString.
//<<===========================================================================
{
	char* read_ptr = string;
	char* write_ptr = string;

	while (*read_ptr != NULLCHAR) 
	{
		// Check for [XX]
		if ((*read_ptr == OPENBRACKET) &&
			(*(read_ptr + 1) != NULLCHAR) &&
			(*(read_ptr + 2) != NULLCHAR) &&
			(*(read_ptr + 3) == CLOSEBRACKET)) 
		{
			*write_ptr = (char) GetHexValue((read_ptr + 1), 2);
			read_ptr += 3;
		}
		// Check for \XX
		else if (*read_ptr == '\\') 
		{
			read_ptr++;
			if (*read_ptr == '\\') 
			{
				*write_ptr = *read_ptr;
			}
			else 
			{
				*write_ptr = (char) GetHexValue(read_ptr, 2);
				read_ptr++;
			}
		}
		else 
		{
			*write_ptr = *read_ptr;
		}
		read_ptr++;
		write_ptr++;
	}

	*write_ptr = NULLCHAR;

	return string;
}
*/
	// allocate a new vr value
	BASE_VALUE_CLASS *value_ptr = CreateNewValue(ATTR_VR_UN);
	value_ptr->Set(byteArray_ptr, length);

	// return the value
	return value_ptr;
}

//>>===========================================================================

BASE_VALUE_CLASS *hexValue(SCRIPT_SESSION_CLASS *session_ptr, unsigned long hexValue, ATTR_VR_ENUM vr, UINT16 group, UINT16 element)

//  DESCRIPTION     : Function to handle Attribute "Hex" type values.
//					: The appropriate Attribute VR Type Value is
//					: instantiated with the given hexadecimal value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS *logger_ptr = session_ptr->getLogger();
	BASE_VALUE_CLASS *value_ptr = NULL;

	switch (vr)
	{
	case ATTR_VR_AT:
		value_ptr = CreateNewValue(ATTR_VR_AT);
		value_ptr->Set((UINT32) hexValue);
		break;

	case ATTR_VR_US:
		value_ptr = CreateNewValue(ATTR_VR_US);
		value_ptr->Set((UINT16) hexValue);
		break;

	case ATTR_VR_UL:
		{
			DCM_VALUE_UL_CLASS *valueUL_ptr = new DCM_VALUE_UL_CLASS();
			valueUL_ptr->Set((UINT32) hexValue);
			value_ptr = valueUL_ptr;
		}
		break;

	case ATTR_VR_OB:
		value_ptr = CreateNewValue(ATTR_VR_OB);
		value_ptr->Set((UINT32) hexValue);
		break;

	case ATTR_VR_OF:
		value_ptr = CreateNewValue(ATTR_VR_OF);
		value_ptr->Set((UINT32) hexValue);
		break;

	case ATTR_VR_OW:
		value_ptr = CreateNewValue(ATTR_VR_OW);
		value_ptr->Set((UINT32) hexValue);
		break;

	case ATTR_VR_SL:
	case ATTR_VR_SS:
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "SL/SS Value in DICOMScript should be expressed in INTEGER format for attribute (%04X,%04X)", group, element);
			logger_ptr->text(LOG_NONE, 1, "Value not used - zero length taken");
		}
		break;

	default:
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Value in DICOMScript should be expressed in STRING format for attribute (%04X,%04X)", group, element);
			logger_ptr->text(LOG_NONE, 1, "Value not used - zero length taken");
		}
		break;
	}

	// return the value
	return value_ptr;
}

//>>===========================================================================

BASE_VALUE_CLASS *integerValue(SCRIPT_SESSION_CLASS *session_ptr, int intValue, ATTR_VR_ENUM vr, UINT16 group, UINT16 element)

//  DESCRIPTION     : Function to handle Attribute "Integer" type values.
//					: The appropriate Attribute VR Type Value is
//					: instantiated with the given integer value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS *logger_ptr = session_ptr->getLogger();
	BASE_VALUE_CLASS *value_ptr = NULL;

	switch (vr) 
	{
	case ATTR_VR_DS:
		{
			// VTS support - will not be documented
			// - need to convert integer to DS string value
			char buffer[DS_LENGTH];
			sprintf(buffer, "%d", intValue);

			value_ptr = CreateNewValue(vr);
			value_ptr->Set((BYTE*) buffer, strlen(buffer));
		}
		break;
	case ATTR_VR_IS:
		{
			// VTS support - will not be documented
			// - need to convert integer to IS string value
			char buffer[IS_LENGTH];
			sprintf(buffer, "%d", intValue);

			value_ptr = CreateNewValue(vr);
			value_ptr->Set((BYTE*) buffer, strlen(buffer));
		}
		break;
	case ATTR_VR_SS:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((INT16) intValue);
		break;

	case ATTR_VR_SL:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((INT32) intValue);
		break;

	case ATTR_VR_US:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((UINT16) intValue);
		break;

	case ATTR_VR_UL:
		{
			DCM_VALUE_UL_CLASS *valueUL_ptr = new DCM_VALUE_UL_CLASS();
			valueUL_ptr->Set((UINT32) intValue);
			value_ptr = valueUL_ptr;
		}
		break;

	case ATTR_VR_OB:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((UINT32) intValue);
		break;

	case ATTR_VR_OF:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((UINT32) intValue);
		break;

	case ATTR_VR_OW:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((UINT32) intValue);
		break;

	case ATTR_VR_OD:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((UINT32) intValue);
		break;

	case ATTR_VR_OL:
	case ATTR_VR_OV:
		value_ptr = CreateNewValue(vr);
		value_ptr->Set((UINT32) intValue);
		break;

	case ATTR_VR_LT:
	case ATTR_VR_ST:
	case ATTR_VR_UT:
	case ATTR_VR_UC:
	case ATTR_VR_UR:
		{
			// allocate a buffer of the given length and fill with a text pattern
			BYTE *patternValue_ptr = new BYTE [intValue];

			if (patternValue_ptr)
			{
				// generate a pattern
				bytePattern(patternValue_ptr, intValue);

				value_ptr = CreateNewValue(vr);
				value_ptr->Set(patternValue_ptr, intValue);
			}
		}
		break;

	default:
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Value in DICOMScript should be expressed in STRING format for attribute (%04X,%04X)", group, element);
			logger_ptr->text(LOG_NONE, 1, "Value not used - zero length taken");
		}
		break;
	}

	// return the value
	return value_ptr;
}

//>>===========================================================================

BASE_VALUE_CLASS *autoSetValue(SCRIPT_SESSION_CLASS *session_ptr, ATTR_VR_ENUM vr, UINT16 group, UINT16 element)

//  DESCRIPTION     : Function to handle Attribute autoset values.
//					: The appropriate Attribute VR Type Value is
//					: instantiated and given an automatic value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	LOG_CLASS *logger_ptr = session_ptr->getLogger();
	BASE_VALUE_CLASS *value_ptr = NULL;

	// get the current date/time
	struct tm *currentTime_ptr;
	time_t clock;
	char data[MAX_STRING_LEN];

	// get time is seconds and convert to struct tm
	time(&clock);
	currentTime_ptr = localtime(&clock);

	switch (vr) 
	{
	case ATTR_VR_DA:
		// format up to YYYYMMDD
		sprintf(data, "%04d%02d%02d", currentTime_ptr->tm_year+1900, currentTime_ptr->tm_mon+1, currentTime_ptr->tm_mday);
		value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
		break;

	case ATTR_VR_TM:
		// format up to HHMMSS
		sprintf(data, "%02d%02d%02d", currentTime_ptr->tm_hour, currentTime_ptr->tm_min, currentTime_ptr->tm_sec);
		value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
		break;

	case ATTR_VR_DT:
		// format up to YYYYMMDDHHMMSS.000000
		sprintf(data, "%04d%02d%02d%02d%02d%02d.000000", currentTime_ptr->tm_year+1900, currentTime_ptr->tm_mon+1, currentTime_ptr->tm_mday, currentTime_ptr->tm_hour, currentTime_ptr->tm_min, currentTime_ptr->tm_sec);
		value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
		break;

	case ATTR_VR_UI:
		// generate a UID
   		createUID(data, (char*) session_ptr->getImplementationClassUid());
		value_ptr = CreateNewValue(vr);
		value_ptr->Set(data);
		break;

	default:
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Value in DICOMScript cannot be AUTOSET for attribute (%04X,%04X)", group, element);
			logger_ptr->text(LOG_NONE, 1, "Value not used - zero length taken");
		}
		break;
	}

	// return the value
	return value_ptr;
}


//>>===========================================================================

void registerVTSUidMapping(UINT32     tag,
					  string     label, 
					  string     value,
					  LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Function to handle VTS style automatic uid referencing
//                    Checks if the value is a vts style label and adds 
//                    defining entry in the uid mapping table.
//                    If an entry with the given label already exists
//                    it is assumed that the existing entry is the defining
//                    entry and the next is a referring entry. It will not be 
//                    stored again but used later when resolving the references
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is only intended for backward compatibility
//<<===========================================================================
{
	//check is an entry for this label && tag already exists
	//A label may occur for different attributes so we take into account
	//the attribute tag.
	bool found = false;
	vector<VTS_UID_MAPPING_STRUCT>::iterator it;
	it = vts_uid_mappings.begin();
	while (it < vts_uid_mappings.end() && !found)
	{
		if ( it->label == label )
		{
			found = true;
			if ( it->tag != tag)
			{
				logger_ptr->text(LOG_WARNING, 1, "Label %s, (%08X) already defined for tag %08X. Skipped",
					                              label.c_str(), tag, it->tag);
			}
		}
		++it;
	}

	if (!found)
	{
		//If not, assume it is a mapping
		VTS_UID_MAPPING_STRUCT mapping;
		mapping.tag   = tag;	
		mapping.label = label;
		mapping.value = value;

		vts_uid_mappings.push_back(mapping);
	}

}

//>>===========================================================================

void resolveVTSUidMappings(DCM_ATTRIBUTE_GROUP_CLASS* src_object_ptr)

//  DESCRIPTION     : Function to handle VTS style automatic uid referencing
//                    It tries to resolve UID <-> label mappings in the objects 
//                    to be send
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is only intended for backward compatibility
//<<===========================================================================
{
	vector<VTS_UID_MAPPING_STRUCT>::iterator it;

	for (it = vts_uid_mappings.begin(); it < vts_uid_mappings.end(); ++it)
	{
		if (it->value != "UNDEFINED")
		{
			//the mapping is already resolved, copy value
			//to atttributes in object if the labels are equal
			replaceVTSUidLabels(src_object_ptr, it->label, it->value);
		}
	}
}

//>>===========================================================================

bool registerVTSUidMappings(DCM_ATTRIBUTE_GROUP_CLASS* src_object_ptr,
					   DCM_ATTRIBUTE_GROUP_CLASS* ref_object_ptr,
					   LOG_CLASS*          logger_ptr)

//  DESCRIPTION     : Function to handle VTS style automatic uid referencing
//                    It loops over the received object and the reference
//                    object. If it finds any VTS Style uid references
//                    (labelled with a string value) it is registered
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is only intended for backward compatibility
//<<===========================================================================
{
	bool status = true;

    UINT                     nr_src_attrs = 0;
	UINT                     nr_ref_attrs = 0;
	DCM_ATTRIBUTE_CLASS*     src_attr_ptr = NULL;
	DCM_ATTRIBUTE_CLASS*     ref_attr_ptr = NULL;
    UINT16                   src_group = MAX_GROUP;
    UINT16                   src_element = MAX_ELEMENT;	
    UINT16                   ref_group = MAX_GROUP;
    UINT16                   ref_element = MAX_ELEMENT;	
	UINT32                   srctag = MAX_TAG;
	UINT32                   reftag = MAX_TAG;
	UINT                     srcVM;
	UINT                     refVM;
	ATTR_VR_ENUM             srcVR;
	ATTR_VR_ENUM             refVR;
	UINT                     i = 0; //counter
	UINT                     j = 0; //counter
	bool                     src_attrs = false;
	bool                     ref_attrs = false;
	BASE_VALUE_CLASS*        src_value_ptr = NULL;
	BASE_VALUE_CLASS*        ref_value_ptr = NULL;

	// check for valid objects
	if ((src_object_ptr == NULL) ||
		(ref_object_ptr == NULL))
	{
		return true;
	}

	//get first source attribute
	nr_src_attrs = src_object_ptr->GetNrAttributes();
	if (i < nr_src_attrs )
	{
	    src_attr_ptr = src_object_ptr->GetAttribute(i);
		src_attrs = true;
    }

	//get first reference attribute
	nr_ref_attrs = ref_object_ptr->GetNrAttributes();
	if (j < nr_ref_attrs )
	{
		ref_attr_ptr = ref_object_ptr->GetAttribute(j);
		ref_attrs = true;
	}

	//loop over src and reference objects
	while (src_attrs || ref_attrs) //while we have attributes
	{
		if (src_attrs)
		{
		    src_group   = src_attr_ptr->GetGroup();
		    src_element = src_attr_ptr->GetElement();
		    srctag = (((UINT32)(src_group))<<16) + (UINT32)src_element;
        }

		if (ref_attrs)
		{
			ref_group   = ref_attr_ptr->GetGroup();
			ref_element = ref_attr_ptr->GetElement();
			reftag  = (((UINT32)(ref_group))<<16) + (UINT32)ref_element;
		}

		//Only check attributes with equal tags
		if (srctag == reftag)
		{
			//compare VR
			srcVR = src_attr_ptr->GetVR();
			refVR = ref_attr_ptr->GetVR();

			//Only check for uid references if VR is equal
			if (srcVR == refVR)
			{
				//For UI attributes check for labels
				if (srcVR == ATTR_VR_UI)
				{
					//compare VM
					//Only register if the the number of values is equal
					srcVM = src_attr_ptr->GetNrValues();
					refVM = ref_attr_ptr->GetNrValues();

					if ( (srcVM == refVM) && (refVM > 0) )
					{  
						for ( UINT val_idx = 0; val_idx < srcVM; ++val_idx)
						{
							src_value_ptr = src_attr_ptr->GetValue(val_idx);
							ref_value_ptr = ref_attr_ptr->GetValue(val_idx);
							string src_value;
							string ref_value;

							src_value_ptr->Get(src_value);
							ref_value_ptr->Get(ref_value);
							if ((src_value.length()) &&
								(ref_value.length()))
							{
								//If the referece value is not a valid UID it probably is a label
								//so we register it
								if (!isUID((char*)(ref_value.c_str())))
								{
									registerVTSUidMapping(srctag, (char*)(ref_value.c_str()), (char*)(src_value.c_str()), logger_ptr);

									//replace label in reference object with actual value
									ref_object_ptr->setUIValue(srctag, src_value);
								}
							}
						}
					}
				}
				else if (srcVR == ATTR_VR_SQ)
				{
					//compare values
					status = checkVTSUidMappingsInSequence(src_attr_ptr, ref_attr_ptr, logger_ptr);
				}
			}

			//get next source attribute
			++i;
			if (i < nr_src_attrs)
			{
				src_attr_ptr = src_object_ptr->GetAttribute(i);
				src_attrs = true;
			}
			else
			{
                src_attrs = false;
				srctag = MAX_TAG;
			}

			//get next reference attribute
			++j;
			if (j < nr_ref_attrs)
			{
				ref_attr_ptr = ref_object_ptr->GetAttribute(j);
				ref_attrs = true;
			}
			else
			{
                ref_attrs = false;
				reftag = MAX_TAG;
			}
		}
		else if (srctag > reftag) //Reference object contains more attributes
		{
			//get next reference attribute
			++j;
			if (j < nr_ref_attrs)
			{
				ref_attr_ptr = ref_object_ptr->GetAttribute(j);
				ref_attrs = true;
			}
			else
			{
                ref_attrs = false;
				reftag = MAX_TAG;
			}
		}
		else if (srctag < reftag) //Source object contains more attributes
		{
			//get next source attribute
			++i;
			if (i < nr_src_attrs)
			{
				src_attr_ptr = src_object_ptr->GetAttribute(i);
				src_attrs = true;
			}
			else
			{
                src_attrs = false;
				srctag = MAX_TAG;
			}
		}
	} //end of while

	return status;
}

//>>===========================================================================

bool checkVTSUidMappingsInSequence(DCM_ATTRIBUTE_CLASS* src_attr_ptr, 
							  DCM_ATTRIBUTE_CLASS* ref_attr_ptr,
							  LOG_CLASS*       logger_ptr)

//  DESCRIPTION     : Function to handle VTS style automatic uid referencing
//                    in sequence attributes
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is only intended for backward compatibility
//<<===========================================================================
{
	bool				status = true;
	UINT				nr_src_sq_values = 0;
	UINT				nr_src_items = 0;
    DCM_VALUE_SQ_CLASS*	src_sq_value_ptr = NULL;
	DCM_ITEM_CLASS*		src_item_ptr = NULL;
	UINT				nr_ref_sq_values = 0;
	UINT				nr_ref_items = 0;
    DCM_VALUE_SQ_CLASS*	ref_sq_value_ptr = NULL;
	DCM_ITEM_CLASS*     ref_item_ptr = NULL;


	//compare items
	nr_src_sq_values = src_attr_ptr->GetNrValues();
	nr_ref_sq_values = ref_attr_ptr->GetNrValues();
	if ((nr_src_sq_values == 1) && (nr_ref_sq_values == 1))
	{
		src_sq_value_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(src_attr_ptr->GetValue(0));
		ref_sq_value_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(ref_attr_ptr->GetValue(0));
		
		//Do some basic checks before validating the items
		nr_src_items = src_sq_value_ptr->GetNrItems();
		nr_ref_items = ref_sq_value_ptr->GetNrItems();
		if (nr_src_items != nr_ref_items)
		{
			status = false;
		}

		if (status)
		{
			for (UINT i = 0; i < nr_src_items; i++)
			{
				//for each item search for mappings
				src_item_ptr = src_sq_value_ptr->getItem(i);
				ref_item_ptr = ref_sq_value_ptr->getItem(i);

				registerVTSUidMappings(src_item_ptr, 
									   ref_item_ptr, 
									   logger_ptr);
			}
		}
	}
	else if (nr_src_sq_values != nr_ref_sq_values)
	{
		status = false;
	}

	return status;
}

//>>===========================================================================

static void replaceVTSUidLabels(DCM_ATTRIBUTE_GROUP_CLASS* object_ptr, string label, string value)

//  DESCRIPTION     : Function to handle VTS style automatic uid referencing
//                    It replaces (recursively) the labels by the given value 
//                    for the attributes having this label
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is only intended for backward compatibility
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS* attr_ptr = NULL;
	ATTR_VR_ENUM vr = ATTR_VR_UN;

	UINT nr_attrs = object_ptr->GetNrAttributes();
	for (UINT attr_idx = 0; attr_idx < nr_attrs; ++attr_idx)
	{
		attr_ptr = object_ptr->GetAttribute(attr_idx);
		UINT16 group   = attr_ptr->GetGroup();
		UINT16 element = attr_ptr->GetElement();
		UINT32 tag = ( ( (UINT32)group ) << 16) + ( (UINT32)element);

		vr = attr_ptr->GetVR();		
		if (vr == ATTR_VR_UI)
		{
			string uid;
			object_ptr->getUIValue(tag, uid);

			//check if the uid value value is equal to the label
			if (label == uid)
			{
				//copy mapped value to attribute
				object_ptr->setUIValue(tag, value);
			}
		}
		else if (vr == ATTR_VR_SQ)
		{
			//recurse for sequence attributes
			if (attr_ptr->GetNrValues() == 1)
			{
				// get SQ value
				DCM_VALUE_SQ_CLASS* sq_value_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attr_ptr->GetValue(0));
				DCM_ITEM_CLASS*     item_ptr = NULL;

				UINT nr_items = sq_value_ptr->GetNrItems();
				for (UINT item_idx = 0; item_idx < nr_items; ++item_idx)
				{
					item_ptr = sq_value_ptr->getItem(item_idx);

					replaceVTSUidLabels(item_ptr, label, value);
				}
			}
		}
	}
}

//>>===========================================================================

static bool findUidAttribute(UINT32 ui_tag, DCM_ATTRIBUTE_GROUP_CLASS* object_ptr, string& uid)

//  DESCRIPTION     : Function to handle VTS style automatic uid referencing
//                    It searches the given object (recursively) for the attribute 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is only intended for backward compatibility
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS* attr_ptr = NULL;
	ATTR_VR_ENUM vr = ATTR_VR_UN;
	bool found = false;

	int attr_idx = 0;
	while ((attr_idx < object_ptr->GetNrAttributes()) && !found)
	{
		attr_ptr = object_ptr->GetAttribute(attr_idx);
		UINT16 group   = attr_ptr->GetGroup();
		UINT16 element = attr_ptr->GetElement();
		UINT32 tag = ( ( (UINT32)group ) << 16) + ( (UINT32)element);
		vr = attr_ptr->GetVR();

		if (vr == ATTR_VR_UI)
		{
			if (tag == ui_tag)
			{
				found = true;
				//assume we have only 1 value. Risky!
				BASE_VALUE_CLASS* value_ptr = attr_ptr->GetValue(0);
				value_ptr->Get(uid);
			}
		}
		else if (vr == ATTR_VR_SQ)
		{
			//recurse for sequence attributes
			if (attr_ptr->GetNrValues() == 1)
			{
				// get SQ value
				DCM_VALUE_SQ_CLASS* sq_value_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attr_ptr->GetValue(0));
				DCM_ITEM_CLASS*     item_ptr = NULL;

				int item_idx = 0;
				while ( item_idx < sq_value_ptr->GetNrItems() && !found )
				{
					item_ptr = sq_value_ptr->getItem(item_idx);

					found = findUidAttribute(tag, item_ptr, uid);

					++item_idx;
				}
			}
		}
		
		++attr_idx;
	}

	return found;
}

//>>===========================================================================

void clearVTSUidMappings(void)

//  DESCRIPTION     : Function to handle VTS style automatic uid referencing
//                    Clears all established UID <-> label mappings 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function is only intended for backward compatibility
//<<===========================================================================
{
	vts_uid_mappings.clear();
}
