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
//  DESCRIPTION     :	Base SCP emulator class.
//*****************************************************************************
#ifndef EMULATOR_H
#define EMULATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Idicom.h"			// Dicom component interface
#include "Inetwork.h"		// Network component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class EMULATOR_SESSION_CLASS;
class STORAGE_COMMITMENT_CLASS;
class LOG_CLASS;

//>>***************************************************************************

class BASE_SCP_EMULATOR_CLASS 

//  DESCRIPTION     : Abstract base SCP emulator class for various SOP class SCP emulations
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	EMULATOR_SESSION_CLASS		*sessionM_ptr;
	int							connectedSocketIdM;
	ASSOCIATION_CLASS			associationM;
	string						sopClassUidM;
	string						sopInstanceUidM;
	LOG_CLASS					*loggerM_ptr;
    BASE_SERIALIZER             *serializerM_ptr;
	bool						autoType2AttributesM;
	bool						defineSqLengthM;
	bool						addGroupLengthM;
	bool						associatedM;
	

	void setup(EMULATOR_SESSION_CLASS*, BASE_SOCKET_CLASS*, bool);

	void teardown();
	
	virtual bool processCommandDataset(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*) = 0;

	bool sendResponse(DIMSE_CMD_ENUM, BYTE, UINT16 status = DCM_STATUS_SUCCESS, DCM_DATASET_CLASS *dataset_ptr = NULL);

	virtual void completeLogging() = 0;

public:
	bool						isAsync;
	
	virtual ~BASE_SCP_EMULATOR_CLASS() = 0;

	bool emulateScp();

	virtual STORAGE_COMMITMENT_CLASS* getStorageCommitObject()
	{
		return NULL;
	}

	virtual bool postProcess()
		{ return true; }

	virtual bool sendEventReport()
		{ return true; }

	virtual bool terminate();

	virtual bool sendStatusEvent()
		{ return true; }

	
	EMULATOR_SESSION_CLASS *getSession() { return sessionM_ptr; }

	LOG_CLASS *getLogger() { return loggerM_ptr; }

	bool isAssociated() { return associatedM; }

	void setLogger(LOG_CLASS*);

    void setSerializer(BASE_SERIALIZER*);

	bool setSocketOwnerThreadId(THREAD_TYPE tid);
};

#endif /* EMULATOR_H */


