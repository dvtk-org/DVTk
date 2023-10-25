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

#ifndef WAREHOUSE_H
#define WAREHOUSE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define WAREHOUSE	OBJECT_WAREHOUSE_CLASS::instance()

//
// Warehouse Item Data enumerates
//
enum WID_ENUM
{
	WID_ASSOCIATE_RQ, 
	WID_ASSOCIATE_AC, 
	WID_ASSOCIATE_RJ,
	WID_RELEASE_RQ,
	WID_RELEASE_RP, 
	WID_ABORT_RQ,
	WID_UNKNOWN_PDU,
//	WID_COMMAND_RQ,
//	WID_COMMAND_RSP,
	WID_C_ECHO_RQ,
	WID_C_ECHO_RSP,
	WID_C_FIND_RQ,
	WID_C_FIND_RSP,
	WID_C_GET_RQ,
	WID_C_GET_RSP,
	WID_C_MOVE_RQ,
	WID_C_MOVE_RSP,
	WID_C_STORE_RQ,
	WID_C_STORE_RSP,
	WID_C_CANCEL_RQ,
	WID_N_ACTION_RQ,
	WID_N_ACTION_RSP,
	WID_N_CREATE_RQ,
	WID_N_CREATE_RSP,
	WID_N_DELETE_RQ,
	WID_N_DELETE_RSP,
	WID_N_EVENT_REPORT_RQ,
	WID_N_EVENT_REPORT_RSP,
	WID_N_GET_RQ,
	WID_N_GET_RSP,
	WID_N_SET_RQ,
	WID_N_SET_RSP,
	WID_DATASET, 
	WID_ITEM,
	WID_ITEM_HANDLE,
    WID_FILEHEAD,
    WID_META_INFO, 
	WID_FILETAIL,
	WID_UNKNOWN
};

//
// Warehouse Item Data name mapping
//
struct T_WID_MAP
{
	WID_ENUM	id;
	char		*name;
};

char *WIDName(WID_ENUM);

//
// define the NEW: and LABEL: keyword
//
#define NEWKEYWORD		"NEW:"
#define LABELKEYWORD	"LABEL:"


//>>***************************************************************************

class BASE_WAREHOUSE_ITEM_DATA_CLASS

//  DESCRIPTION     : Warehouse Item Data abstract base class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	WID_ENUM	widTypeM;
	LOG_CLASS	*loggerM_ptr;

public:
	virtual ~BASE_WAREHOUSE_ITEM_DATA_CLASS() = 0;

	inline WID_ENUM getWidType() 
		{ return widTypeM; }

	void setWidType(WID_ENUM type);

	virtual bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*) = 0;

	virtual void setLogger(LOG_CLASS *logger_ptr)
		{ loggerM_ptr = logger_ptr; }

	LOG_CLASS* getLogger()
		{ return loggerM_ptr; }
};


//>>***************************************************************************

class WAREHOUSE_ITEM_CLASS

//  DESCRIPTION     : Class describing individual warehouse items - script object.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string							identifierM;
	BASE_WAREHOUSE_ITEM_DATA_CLASS	*dataM_ptr;

public:
	WAREHOUSE_ITEM_CLASS();
	WAREHOUSE_ITEM_CLASS(string, BASE_WAREHOUSE_ITEM_DATA_CLASS*);
	WAREHOUSE_ITEM_CLASS(WAREHOUSE_ITEM_CLASS&);

	~WAREHOUSE_ITEM_CLASS();

	string getIdentifier()
		{ return identifierM; }

	WID_ENUM getType();

	void setLogger(LOG_CLASS *logger_ptr)
	{
		if (dataM_ptr)
		{
			dataM_ptr->setLogger(logger_ptr);
		}
	}

	LOG_CLASS *getLogger()
	{
		LOG_CLASS *logger_ptr = NULL;
		if (dataM_ptr)
		{
			logger_ptr = dataM_ptr->getLogger();
		}

		return logger_ptr;
	}

	BASE_WAREHOUSE_ITEM_DATA_CLASS *getItemData()
		{ return dataM_ptr; }
};

//>>***************************************************************************

class WAREHOUSE_MAPPING_CLASS

//  DESCRIPTION     : Class defining attribute Name / Value mappings.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string	nameM;
	string	valueM;
	UINT16	groupM;
	UINT16	elementM;
	bool	userDefinedM;

public:
	WAREHOUSE_MAPPING_CLASS();
	WAREHOUSE_MAPPING_CLASS(char*, char*, UINT16, UINT16, bool userDefined = true);

	~WAREHOUSE_MAPPING_CLASS();

	char *getName()
		{ return (char*) nameM.c_str(); }

	char *getValue()
		{ return (char*) valueM.c_str(); }

	bool isUserDefined()
		{ return userDefinedM; }
	
	void log(LOG_CLASS*);
};


//>>***************************************************************************

class OBJECT_WAREHOUSE_CLASS

//  DESCRIPTION     : Class used to store the scripting objects.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static OBJECT_WAREHOUSE_CLASS	*instanceM_ptr;	// Singleton
	ARRAY<WAREHOUSE_ITEM_CLASS>		itemM;
	ARRAY<WAREHOUSE_MAPPING_CLASS>	mappingM;
	UINT32							referenceTagM;

	void initSemaphore();
	void postSemaphore();
	void waitSemaphore();
	void termSemaphore();

	bool search(string, WID_ENUM, UINT*);

	void registerTransferSyntaxes();

	void registerApplicationContextNames();

	void registerVerificationSopClass();

public:
	OBJECT_WAREHOUSE_CLASS();

	~OBJECT_WAREHOUSE_CLASS();

	static OBJECT_WAREHOUSE_CLASS *instance();

	// storage functions
	void empty();

	bool store(string, BASE_WAREHOUSE_ITEM_DATA_CLASS*);

	bool remove(string, WID_ENUM);

	bool update(string, BASE_WAREHOUSE_ITEM_DATA_CLASS*);

	BASE_WAREHOUSE_ITEM_DATA_CLASS *retrieve(string, WID_ENUM);

	void setReferenceTag(UINT32 tag)
		{ referenceTagM = tag; }

	UINT32 getReferenceTag()
		{ return referenceTagM; }
		
	// mapping functions
	bool addMappedValue(char*, char*, UINT16, UINT16, LOG_CLASS*, bool userDefined = true);

	void modifyMappedValue(char*, char*, UINT16, UINT16, LOG_CLASS*);

	char *getMappedName(char*, LOG_CLASS*);

	char *refreshMappedName(char*, LOG_CLASS*);

	char *getMappedValue(char*, LOG_CLASS*);

	char *setLabelledValue(char*, UINT16, UINT16, LOG_CLASS*);

	WID_ENUM dimse2widtype(DIMSE_CMD_ENUM dimse_cmd);

   	void serialize(LOG_CLASS*);

	void log(LOG_CLASS*);

	void logMapping(LOG_CLASS*);
};


#endif /* OBJECT_WAREHOUSE_H */