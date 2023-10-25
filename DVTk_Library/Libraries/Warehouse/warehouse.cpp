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

//  Warehouse storage for Objects.

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#ifdef _WINDOWS
//#include "..\global\stdafx.h"    //MIGRATION_IN_PROGRESS
#include <time.h>
#else
#include <unistd.h>
#include <sys/time.h>
#include <semaphore.h>
#endif

#include "warehouse.h"
#include "Idefinition.h"	// Definition component interface
#include "Ilog.h"			// Log component interface
#include "Idicom.h"         // DICOM component interface


extern T_TS_MAP			TTrnStx[MAX_TRN_STX_NAMES];
extern T_CHAR_TEXT_MAP	TAbsStx[MAX_APP_CTX_NAMES];


//*****************************************************************************
//  STATIC DECLARATIONS
//*****************************************************************************
static T_WID_MAP TWIDMap[] = {
{WID_ASSOCIATE_RQ,	"ASSOCIATE-RQ"},
{WID_ASSOCIATE_AC,	"ASSOCIATE-AC"},
{WID_ASSOCIATE_RJ,	"ASSOCIATE-RJ"},
{WID_RELEASE_RQ,	"RELEASE-RQ"},
{WID_RELEASE_RP,	"RELEASE-RP"},
{WID_ABORT_RQ,		"ABORT-RQ"},
{WID_UNKNOWN_PDU,	"UNKNOWN PDU"},
{WID_C_ECHO_RQ,		"C-ECHO-RQ"},
{WID_C_ECHO_RSP,	"C-ECHO-RSP"},
{WID_C_FIND_RQ,		"C-FIND-RQ"},
{WID_C_FIND_RSP,	"C-FIND-RSP"},
{WID_C_GET_RQ,		"C-GET-RQ"},
{WID_C_GET_RSP,		"C-GET-RSP"},
{WID_C_MOVE_RQ,		"C-MOVE-RQ"},
{WID_C_MOVE_RSP,	"C-MOVE-RSP"},
{WID_C_STORE_RQ,	"C-STORE-RQ"},
{WID_C_STORE_RSP,	"C-STORE-RSP"},
{WID_C_CANCEL_RQ,	"C-CANCEL-RQ"},
{WID_N_ACTION_RQ,	"N-ACTION-RQ"},
{WID_N_ACTION_RSP,	"N-ACTION-RSP"},
{WID_N_CREATE_RQ,	"N-CREATE-RQ"},
{WID_N_CREATE_RSP,	"N-CREATE-RSP"},
{WID_N_DELETE_RQ,	"N-DELETE-RQ"},
{WID_N_DELETE_RSP,	"N-DELETE-RSP"},
{WID_N_EVENT_REPORT_RQ,	 "N-EVENT-REPORT-RQ"},
{WID_N_EVENT_REPORT_RSP, "N-EVENT_REPORT-RSP"},
{WID_N_GET_RQ,		"N-GET-RQ"},
{WID_N_GET_RSP,		"N-GET-RSP"},
{WID_N_SET_RQ,		"N-SET-RQ"},
{WID_N_SET_RSP,		"N-SET-RSP"},
{WID_DATASET,		"DATASET"},
{WID_ITEM,			"ITEM"},
{WID_ITEM_HANDLE,	"ITEM HANDLE"},
{WID_FILEHEAD,		"FILE-HEAD"},
{WID_META_INFO,		"META FILE INFORMATION"},
{WID_FILETAIL,		"FILE-TAIL"},
{WID_UNKNOWN,		"UNKNOWN OBJECT"}
};


//>>===========================================================================

char *WIDName(WID_ENUM id)

//  DESCRIPTION     : Warehouse Item Data - name mapping.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int		index = 0;

	while ((TWIDMap[index].id != id)
	  && (TWIDMap[index].id != WID_UNKNOWN))
		index++;

	return TWIDMap[index].name;
}


//*****************************************************************************
// initialise static pointers
//*****************************************************************************
OBJECT_WAREHOUSE_CLASS *OBJECT_WAREHOUSE_CLASS::instanceM_ptr = NULL;


//>>===========================================================================

BASE_WAREHOUSE_ITEM_DATA_CLASS::~BASE_WAREHOUSE_ITEM_DATA_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// empty virtual destructor
}

//>>===========================================================================

void 
BASE_WAREHOUSE_ITEM_DATA_CLASS::setWidType(WID_ENUM type)

//  DESCRIPTION     : Sets WID type
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	widTypeM = type;
}

//>>===========================================================================

WAREHOUSE_ITEM_CLASS::WAREHOUSE_ITEM_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	identifierM = "";
	dataM_ptr = NULL;
}

//>>===========================================================================

WAREHOUSE_ITEM_CLASS::WAREHOUSE_ITEM_CLASS(string identifier, BASE_WAREHOUSE_ITEM_DATA_CLASS *data_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	identifierM = identifier;
	dataM_ptr = data_ptr;
}

//>>===========================================================================

WAREHOUSE_ITEM_CLASS::WAREHOUSE_ITEM_CLASS(WAREHOUSE_ITEM_CLASS& item)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	(*this) = item;
}

//>>===========================================================================

WAREHOUSE_ITEM_CLASS::~WAREHOUSE_ITEM_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (dataM_ptr) 
	{
//		delete dataM_ptr;
	}
}

//>>===========================================================================

WID_ENUM WAREHOUSE_ITEM_CLASS::getType()

//  DESCRIPTION     : Return the type of data stored in the item.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	WID_ENUM type = WID_UNKNOWN;

	// check if data actually stored.
	if (dataM_ptr)
	{
		// get type of data stored
		type = dataM_ptr->getWidType(); 
	}

	// return type of data stored - maybe unknown
	return type;
}


//>>===========================================================================

WAREHOUSE_MAPPING_CLASS::WAREHOUSE_MAPPING_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	nameM = "";
	valueM = "";
	groupM = 0;
	elementM = 0;
	userDefinedM = false;
}

//>>===========================================================================

WAREHOUSE_MAPPING_CLASS::WAREHOUSE_MAPPING_CLASS(char *name_ptr, char *value_ptr, UINT16 group, UINT16 element, bool userDefined)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	nameM = name_ptr;
	valueM = value_ptr;
	groupM = group;
	elementM = element;
	userDefinedM = userDefined;
}

//>>===========================================================================

WAREHOUSE_MAPPING_CLASS::~WAREHOUSE_MAPPING_CLASS()

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

void WAREHOUSE_MAPPING_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log the Name / Value mapping.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if logger available
	if ((logger_ptr == NULL) ||
		(!userDefinedM)) return;

	// save old log level - and set new
	UINT32 oldLogLevel = logger_ptr->logLevel(LOG_LABEL);

	// log the mapping
	if ((nameM.length()) &&
		(valueM.length()))
	{
		if ((groupM == 0) &&
			(elementM == 0))
		{
			if (valueM == UNDEFINED_MAPPING)
			{
				logger_ptr->text(1, "\"%s\" : \"not defined\"", nameM.c_str());
			}
			else
			{
				logger_ptr->text(1, "\"%s\" : \"%s\"", nameM.c_str(), valueM.c_str());
			}
		}
		else
		{
			string attributeName = DEFINITION->GetAttributeName(groupM, elementM);

			if (valueM == UNDEFINED_MAPPING)
			{
				logger_ptr->text(1, "\"%s\" : \"not defined\" : (%04X,%04X) : \"%s\"", nameM.c_str(), groupM, elementM, attributeName.c_str());
			}
			else
			{
				logger_ptr->text(1, "\"%s\" : \"%s\" : (%04X,%04X) : \"%s\"", nameM.c_str(), valueM.c_str(), groupM, elementM, attributeName.c_str());
			}
		}
	}

	// restore original log level
	logger_ptr->logLevel(oldLogLevel);
}


//>>===========================================================================

OBJECT_WAREHOUSE_CLASS::OBJECT_WAREHOUSE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	registerTransferSyntaxes();
	registerApplicationContextNames();
	registerVerificationSopClass();

	// initialise the access semaphore
	initSemaphore();

	// set the reference tag
	referenceTagM = TAG_UNDEFINED;
}

//>>===========================================================================

OBJECT_WAREHOUSE_CLASS::~OBJECT_WAREHOUSE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	while (itemM.getSize())
	{
		// remove warehouse item
		itemM.removeAt(0);
	}
	while (mappingM.getSize())
	{
		// remove mapping item
		mappingM.removeAt(0);
	}

	// terminate the access semaphore
	termSemaphore();
}

//>>===========================================================================

OBJECT_WAREHOUSE_CLASS *OBJECT_WAREHOUSE_CLASS::instance()

//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new OBJECT_WAREHOUSE_CLASS();
	}

	return instanceM_ptr;
}

//
// set up mutual exclusion
//
#ifdef _WINDOWS

class CCriticalSection
{
public:
	CCriticalSection()
	{
		mInitialized = false;
	}
	~CCriticalSection()
	{
	}
	void Lock()
	{
		if( !mInitialized )
		{
			InitializeCriticalSection(&mSection);
			mInitialized = true;
		}
		EnterCriticalSection(&mSection);
	}
	void Unlock()
	{
		LeaveCriticalSection(&mSection);
	}
private:
	bool mInitialized;
	CRITICAL_SECTION mSection;
};

CCriticalSection	WarehouseAccess;

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::initSemaphore()

//  DESCRIPTION     : Initialize the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// do nothing
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::postSemaphore()

//  DESCRIPTION     : Post the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// post the semaphore
	WarehouseAccess.Unlock();
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::waitSemaphore()

//  DESCRIPTION     : Wait for the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for the semaphore
	WarehouseAccess.Lock();
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::termSemaphore()

//  DESCRIPTION     : Terminate the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// do nothing
}

#else
sem_t	WarehouseSemaphoreId;

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::initSemaphore()

//  DESCRIPTION     : Initialize the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise the semaphore
	if (sem_init(&WarehouseSemaphoreId, 0, 1) != 0) {
		printf("\nFailed to create a semaphore for database locking - exiting...");
		exit(0);
	}
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::postSemaphore()

//  DESCRIPTION     : Post the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// post the semaphore
	if (sem_post(&WarehouseSemaphoreId) != 0) {
		printf("\nFailed to post to semaphore for database locking - exiting...");
		exit(0);
	}
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::waitSemaphore()

//  DESCRIPTION     : Wait for the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for the semaphore
	if (sem_wait(&WarehouseSemaphoreId) != 0) {
		printf("\nFailed to wait on semaphore for database locking - exiting...");
		exit(0);	
	}
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::termSemaphore()

//  DESCRIPTION     : Terminate the warehouse access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destroy the semaphore
	if (sem_destroy(&WarehouseSemaphoreId) != 0) {
		printf("\nFailed to destroy the semaphore for database locking - exiting...");
		exit(0);	
	}
}

#endif

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::empty()

//  DESCRIPTION     : Empty the warehouse of all stored items.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for access to warehouse
	waitSemaphore();

	// destructor activities
	while (itemM.getSize())
	{
		// remove warehouse item
		itemM.removeAt(0);
	}

	// reset the reference tag
	referenceTagM = TAG_UNDEFINED;

	// release access to warehouse
	postSemaphore();
}

//>>===========================================================================

bool OBJECT_WAREHOUSE_CLASS::search(string identifier, WID_ENUM type, UINT *index_ptr)

//  DESCRIPTION     : Search the warehouse for the identified item.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	*index_ptr = 0;

	// search warehouse for identified item
	// - perform simple sequential search
	for (UINT i = 0; i < itemM.getSize(); i++)
	{
		// must match identifier and type
		if ((itemM[i].getIdentifier() == identifier) &&
			(itemM[i].getType() == type))
		{
			// return index to matching item
			*index_ptr = i;
			result = true;
			break;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool OBJECT_WAREHOUSE_CLASS::store(string identifier, BASE_WAREHOUSE_ITEM_DATA_CLASS* data_ptr)

//  DESCRIPTION     : Store given data in the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT index;

	// check for valid data
	if (data_ptr == NULL) return false;

	// wait for access to warehouse
	waitSemaphore();

	// first check if the same identifier has already been used
	if (search(identifier, data_ptr->getWidType(), &index))
	{
		// remove existing item - the new data will simply replace the old
		itemM.removeAt(index);
	}

	// store the new data
	WAREHOUSE_ITEM_CLASS newData(identifier, data_ptr);
	itemM.add(newData);

	// release access to warehouse
	postSemaphore();

	// return result
	return true;
}

//>>===========================================================================

bool OBJECT_WAREHOUSE_CLASS::remove(string identifier, WID_ENUM type)

//  DESCRIPTION     : Remove data from the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	UINT index;

	// wait for access to warehouse
	waitSemaphore();

	// search for item to remove
	if (search(identifier, type, &index))
	{
		// remove item
		BASE_WAREHOUSE_ITEM_DATA_CLASS *item_ptr = itemM[index].getItemData();
		delete item_ptr;
		itemM.removeAt(index);
		result = true;
	}

	// release access to warehouse
	postSemaphore();

	// return result
	return result;
}

//>>===========================================================================

bool OBJECT_WAREHOUSE_CLASS::update(string identifier, BASE_WAREHOUSE_ITEM_DATA_CLASS *updateData_ptr)

//  DESCRIPTION     : Update the data in the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;
	UINT index;

	// check for valid data
	if (updateData_ptr == NULL) return false;

	// wait for access to warehouse
	waitSemaphore();

	// find the item to update
	if (search(identifier, updateData_ptr->getWidType(), &index))
	{
		// update the matching item
		BASE_WAREHOUSE_ITEM_DATA_CLASS *item_ptr = itemM[index].getItemData();
		result = item_ptr->updateWid(updateData_ptr);
	}

	// release access to warehouse
	postSemaphore();

	// return result
	return result;
}

//>>===========================================================================

BASE_WAREHOUSE_ITEM_DATA_CLASS *OBJECT_WAREHOUSE_CLASS::retrieve(string identifier, WID_ENUM wid)

//  DESCRIPTION     : Retrieve the data from the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = NULL;
	UINT index;

	// find the item to retrieve
	if (search(identifier, wid, &index))
	{
		// update the matching item
		wid_ptr = itemM[index].getItemData();
	}

	// return result
	return wid_ptr;
}


//>>===========================================================================

bool OBJECT_WAREHOUSE_CLASS::addMappedValue(char *name_ptr, char *value_ptr, UINT16 group, UINT16 element, LOG_CLASS *logger_ptr, bool userDefined)

//  DESCRIPTION     : Method to add a Name / Value mapping to the mapping list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 1, "OBJECT_WAREHOUSE_CLASS::addMappedValue(name_ptr:= %s value_ptr:= %s)", name_ptr, value_ptr);
	}

	char *existingValue_ptr;

	// check if Value has already been defined
	if ((existingValue_ptr = getMappedValue(name_ptr, logger_ptr)) != NULL)
	{
		if (strcmp(existingValue_ptr, value_ptr) != 0)
		{
			// Name previously given to different Value
			// - modify the Name / Value mapping
			modifyMappedValue(name_ptr, value_ptr, group, element, logger_ptr);
		}
	}
	else
	{
		// instaniate a new Name / Value mapping
		WAREHOUSE_MAPPING_CLASS mapping(name_ptr, value_ptr, group, element, userDefined);

		// add Name / Value mapping
		mappingM.add(mapping);
	}

	// return result
	return true;
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::modifyMappedValue(char *name_ptr, char *value_ptr, UINT16 group, UINT16 element, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Method to modify the Name / Value. The Name must already
//					: exist in the mapping table.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr)
	{
		logger_ptr->text(LOG_DEBUG, 1, "OBJECT_WAREHOUSE_CLASS::modifyMappedValue(name_ptr:= %s value_ptr:= %s)", name_ptr, value_ptr);
	}

	// simple sequential search
	for (UINT i = 0; i < mappingM.getSize(); i++)
	{
		// got match ?
		if (strcmp(mappingM[i].getName(), name_ptr) == 0)
		{
			// remove existing mapping
			mappingM.removeAt(i);

			// replace mapping with new values
			addMappedValue(name_ptr, value_ptr, group, element, logger_ptr);
			break;
		}
	}
}

//>>===========================================================================

char *OBJECT_WAREHOUSE_CLASS::getMappedName(char *value_ptr, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Method to get the Name matching the given Value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT	i = 0;

	// simple sequential search
	while (i < mappingM.getSize())
	{
		// got match ?
		if (strcmp(mappingM[i].getValue(), value_ptr) == 0)
		{
			if (logger_ptr)
			{
				logger_ptr->text(LOG_DEBUG, 1, "OBJECT_WAREHOUSE_CLASS::getMappedName(value_ptr:= %s) -> mappedName:= %s", value_ptr, mappingM[i].getName());
			}

			// got it
			return mappingM[i].getName();
		}

		// move to next entry
		i++;
	}

	// no match found - we don't know the Name
	return NULL;
}

//>>===========================================================================

char *OBJECT_WAREHOUSE_CLASS::refreshMappedName(char *name_ptr, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Method to get the Mapped Name or Value matching the given Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : If a value is defined - return the value - not the name.
//<<===========================================================================
{
	UINT	i = 0;

	// simple sequential search
	while (i < mappingM.getSize())
	{
		// got match ?
		if (strcmp(mappingM[i].getName(), name_ptr) == 0)
		{
			// check if a value has been defined
			if ((strlen(mappingM[i].getValue())) &&
				(strcmp(mappingM[i].getValue(), UNDEFINED_MAPPING) != 0))
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_DEBUG, 1, "OBJECT_WAREHOUSE_CLASS::refreshMappedName(name_ptr:= %s) -> mappedValue:= %s", name_ptr, mappingM[i].getValue());
				}

				// got value
				return mappingM[i].getValue();
			}
			else
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_DEBUG, 1, "OBJECT_WAREHOUSE_CLASS::refreshMappedName(name_ptr:= %s) -> mappedName:= %s", name_ptr, mappingM[i].getName());
				}

				// got name
				return mappingM[i].getName();
			}
		}

		// move to next entry
		i++;
	}

	// no match found - we don't know the Name
	return NULL;
}

//>>===========================================================================

char *OBJECT_WAREHOUSE_CLASS::getMappedValue(char *name_ptr, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Method to get the Value matching the given Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT	i = 0;

	// check for valid pointer
	if (name_ptr == NULL) return NULL;

	// simple sequential search
	while (i < mappingM.getSize()) 
	{
		// got match ?
		if (strcmp(mappingM[i].getName(), name_ptr) == 0)
		{
			if (logger_ptr)
			{
				logger_ptr->text(LOG_DEBUG, 1, "OBJECT_WAREHOUSE_CLASS::getMappedValue(name_ptr:= %s) -> mappedValue:= %s", name_ptr, mappingM[i].getValue());
			}

			// got it
			return mappingM[i].getValue();
		}

		// move to next entry
		i++;
	}

	// no match found - we don't know the Value
	return NULL;
}

//>>===========================================================================

char *OBJECT_WAREHOUSE_CLASS::setLabelledValue(char *name_ptr, UINT16 group, UINT16 element, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Method to checked if we are dealing with a labelled
//					: value. A labelled value is used to initialise the mapping
//					: of a Name to a Value. Each time a labelled Name is given,
//					: the Name / Value mapping must be reset so that a new Value
//					: can be mapped to the Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char	*mapping_ptr = NULL;
	UINT	length = strlen(NEWKEYWORD);
	char	value[MAX_STRING_LEN];

	// check for "NEW:" mapping
	if ((strlen(name_ptr) > length)
		&& (strncmp(name_ptr, NEWKEYWORD, length) == 0))
	{
		// get the VR from the definitions
		if (DEFINITION->GetAttributeVr(group, element) == ATTR_VR_UI)
		{
			// create a new UID
			createUID(value, IMPLEMENTATION_CLASS_UID);
		}
		else
		{
			// generate a new Value - using the current time
			struct tm	*currentTime_ptr;
			time_t		clock;

			// get time is seconds and convert to struct tm
			time(&clock);
			currentTime_ptr = localtime(&clock);
			sprintf(value, "1%02d%02d%02d%02d", currentTime_ptr->tm_hour, currentTime_ptr->tm_min, currentTime_ptr->tm_sec, uniq8odd());
		}

		// add the mapping to the list
		mapping_ptr = name_ptr + length;
		addMappedValue(mapping_ptr, value, group, element, logger_ptr);

		// return new Name / Value mapping
		mapping_ptr = getMappedValue(mapping_ptr, logger_ptr);
	}
	else
	{
		length = strlen(LABELKEYWORD);

		// check for a "LABEL:" mapping
		if ((strlen(name_ptr) > length)
			&& (strncmp(name_ptr, LABELKEYWORD, length) == 0))
		{
			char name[MAX_STRING_LEN];

			// we've got a labelled name
			mapping_ptr = name_ptr + length;

			// copy the label name [and value]
			UINT i = 0;
			UINT index = 0;
			strcpy(value, UNDEFINED_MAPPING);
			while (*mapping_ptr != '\0')
			{
				if (*mapping_ptr == COLON)
				{
					if (index == 0)
					{
						// occurence of first colon indicates transition from
						// name to value
						name[i] = '\0';
						i = 0;
						index++;
						mapping_ptr++;
						continue;
					}
				}
				switch (index) 
				{
				case 0:
					name[i++] = *mapping_ptr;
					break;
				case 1:
					value[i++] = *mapping_ptr;
					break;
				}

				mapping_ptr++;
			}

			// terminate last copied string and setup return pointer
			if (index == 0)
			{
				name[i] = '\0';
				mapping_ptr = name;
			}
			else 
			{
				value[i] = '\0';
				mapping_ptr = value;
			}

			// let's check if we need to remove an existing Name / Value mapping for this label
			i = 0;
			while (i < mappingM.getSize())
			{
				// got match ?
				if (strcmp(mappingM[i].getName(), name) == 0)
				{
					// got an old mapping - remove it
					mappingM.removeAt(i);
				}

				// move to next entry
				i++;
			}

			// save label mapping - but first see if the value specified is already a label that
			// should be mapped
			char *existingMapping_ptr = getMappedValue(value, logger_ptr);
			if (existingMapping_ptr != NULL) 
			{
				addMappedValue(name, existingMapping_ptr, group, element, logger_ptr);
				mapping_ptr = existingMapping_ptr;
			}
			else 
			{
				addMappedValue(name, value, group, element, logger_ptr);
				mapping_ptr = refreshMappedName(name, logger_ptr);
			}
		}
		else 
		{
			// just check for normal (unlabelled) Name / Value mapping
			mapping_ptr = getMappedValue(name_ptr, logger_ptr);
		}
	}

	if ((mapping_ptr) &&
		(logger_ptr))
	{
		logger_ptr->text(LOG_DEBUG, 1, "OBJECT_WAREHOUSE_CLASS::setLabelledValue(name_ptr:= %s) -> mapping_ptr:= %s", name_ptr, mapping_ptr);
	}

	// return mapped label
	return mapping_ptr;
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::registerTransferSyntaxes()

//  DESCRIPTION     : Method to register all the known Transfer Syntaxes
//					: for use by the Scripting component.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// register all the known Transfer Syntax mappings
	for (int i = 0; i < MAX_TRN_STX_NAMES; i++)
	{
		addMappedValue(TTrnStx[i].text, TTrnStx[i].uid, 0, 0, NULL, false);
	}
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::registerApplicationContextNames()

//  DESCRIPTION     : Method to register all the known Application Context Names
//					: for use by the Scripting component.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// register all the known Application Context Name mappings
	for (int i = 0; i < MAX_APP_CTX_NAMES; i++) 
	{
		addMappedValue(TAbsStx[i].text, TAbsStx[i].code, 0, 0, NULL, false);
	}
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::registerVerificationSopClass()

//  DESCRIPTION     : Method to register the Verification SOP Class mappings
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// register the Verification SOP Class mappings
	addMappedValue(VERIFICATION_SOP_CLASS_NAME, VERIFICATION_SOP_CLASS_UID, 0, 0, NULL, false);
	addMappedValue(VTS_VERIFICATION_SOP_CLASS_NAME, VERIFICATION_SOP_CLASS_UID, 0, 0, NULL, false);
}

//>>===========================================================================

WID_ENUM OBJECT_WAREHOUSE_CLASS::dimse2widtype(DIMSE_CMD_ENUM dimse_cmd)

//  DESCRIPTION     : Maps dimse commands to a WID Type
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	WID_ENUM widtype = WID_UNKNOWN;

	switch (dimse_cmd)
	{
		case DIMSE_CMD_CECHO_RQ:
			widtype = WID_C_ECHO_RQ;
			break;
		case DIMSE_CMD_CECHO_RSP:
			widtype = WID_C_ECHO_RSP;
			break;
		case DIMSE_CMD_CFIND_RQ:
			widtype = WID_C_FIND_RQ;
			break;
		case DIMSE_CMD_CFIND_RSP:
			widtype = WID_C_FIND_RSP;
			break;
		case DIMSE_CMD_CGET_RQ:
			widtype = WID_C_GET_RQ;
		break;
		case DIMSE_CMD_CGET_RSP:
			widtype = WID_C_GET_RSP;
		break;
		case DIMSE_CMD_CMOVE_RQ:
			widtype = WID_C_MOVE_RQ;
		break;
		case DIMSE_CMD_CMOVE_RSP:
			widtype = WID_C_MOVE_RSP;
		break;
		case DIMSE_CMD_CSTORE_RQ:
			widtype = WID_C_STORE_RQ;
		break;
		case DIMSE_CMD_CSTORE_RSP:
			widtype = WID_C_STORE_RSP;
		break;
		case DIMSE_CMD_CCANCEL_RQ:
			widtype = WID_C_CANCEL_RQ;
		break;
		case DIMSE_CMD_NACTION_RQ:
			widtype = WID_N_ACTION_RQ;
		break;
		case DIMSE_CMD_NACTION_RSP:
			widtype = WID_N_ACTION_RSP;
		break;
		case DIMSE_CMD_NCREATE_RQ:
			widtype = WID_N_CREATE_RQ;
		break;
		case DIMSE_CMD_NCREATE_RSP:
			widtype = WID_N_CREATE_RSP;
		break;
		case DIMSE_CMD_NDELETE_RQ:
			widtype = WID_N_DELETE_RQ;
		break;
		case DIMSE_CMD_NDELETE_RSP:
			widtype = WID_N_DELETE_RSP;
		break;
		case DIMSE_CMD_NEVENTREPORT_RQ:
			widtype = WID_N_EVENT_REPORT_RQ;
		break;
		case DIMSE_CMD_NEVENTREPORT_RSP:
			widtype = WID_N_EVENT_REPORT_RSP;
		break;
		case DIMSE_CMD_NGET_RQ:
			widtype = WID_N_GET_RQ;
		break;
		case DIMSE_CMD_NGET_RSP:
			widtype = WID_N_GET_RSP;
		break;
		case DIMSE_CMD_NSET_RQ:
			widtype = WID_N_SET_RQ;
		break;
		case DIMSE_CMD_NSET_RSP:
			widtype = WID_N_SET_RSP;
		break;
		default:
		    widtype = WID_UNKNOWN;
		break;
	}

    return widtype;
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::serialize(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Serialize the current contents of the warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if logger available
	if (logger_ptr == NULL) return;

    // get the serializer
    BASE_SERIALIZER *serializer_ptr = logger_ptr->getSerializer();
    if (serializer_ptr == NULL) return;

	// save old log level - and set new
	UINT32 oldLogLevel = logger_ptr->logLevel(LOG_INFO);

	// log the warehouse content
	logger_ptr->text(1, "Data Warehouse contains %d objects", itemM.getSize());

	for (UINT i = 0; i < itemM.getSize(); i++)
	{
		logger_ptr->text(1, "Item %d : Identifier %s of type %s", i+1, itemM[i].getIdentifier().c_str(), WIDName(itemM[i].getType()));

		// save the original logger
       	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = itemM[i].getItemData();
        if (wid_ptr == NULL) continue;

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
		    {
			    // retrieve the command from the warehouse
			    DCM_COMMAND_CLASS *command_ptr = static_cast<DCM_COMMAND_CLASS*>(wid_ptr);
	
			    // serialize it
			    serializer_ptr->SerializeDisplay(command_ptr, NULL);
		    }
		    break;
	    case WID_DATASET:
		    {
		    	// retrieve the dataset from the warehouse
			    DCM_DATASET_CLASS *dataset_ptr = static_cast<DCM_DATASET_CLASS*>(wid_ptr);
	
			    // serialize it
			    serializer_ptr->SerializeDisplay(dataset_ptr);
		    }
		    break;
	    case WID_ITEM:
		    {
		    	// retrieve the item from the warehouse
			    DCM_ITEM_CLASS *item_ptr = static_cast<DCM_ITEM_CLASS*>(wid_ptr);
	
			    // serialize it
//			    serializer_ptr->SerializeDisplay(item_ptr);
		    }
		    break;

	    default: break;
	    }
	}

	// restore original log level
	logger_ptr->logLevel(oldLogLevel);	
}

//>>===========================================================================

void OBJECT_WAREHOUSE_CLASS::logMapping(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log the current contents of the warehouse mappings.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if logger available
	if ((logger_ptr == NULL) ||
		(mappingM.getSize() == 0)) 
	{
		return;
	}

	// log the warehouse mapping content
	for (UINT i = 0; i < mappingM.getSize(); i++)
	{
		mappingM[i].log(logger_ptr);
	}
}