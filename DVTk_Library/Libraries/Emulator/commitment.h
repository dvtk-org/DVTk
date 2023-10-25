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
//  DESCRIPTION     :	Storage Commitment emulation classes.
//*****************************************************************************
#ifndef COMMITMENT_H
#define COMMITMENT_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Irelationship.h"	// Relation component interface

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_DATASET_CLASS;
class LOG_CLASS;


//>>***************************************************************************

class STORAGE_COMMITMENT_CLASS 

//  DESCRIPTION     : Storage Commitment class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	bool								eventToSendM;
	string								transactionUidM;
	ARRAY<STORED_SOP_INSTANCE_CLASS*>	storedInstanceM;
	ARRAY<STORED_SOP_INSTANCE_CLASS*>	failedInstanceM;
	STORED_SOP_INSTANCE_CLASS			studyComponentM;

	void setTransactionUid(string transactionUid)
		{ transactionUidM = transactionUid; }

	void addStoredInstance(string, string);

	void addFailedInstance(string, string);

	void setStudyComponentUids(string sopClassUid, string sopInstanceUid)
	{ 
		studyComponentM.setSopClassUid(sopClassUid);
		studyComponentM.setSopInstanceUid(sopInstanceUid);
	}

	string getTransactionUid() { return transactionUidM; }

	UINT noStoredInstances() { return storedInstanceM.getSize(); }

	STORED_SOP_INSTANCE_CLASS *getStoredInstance(UINT);

	STORED_SOP_INSTANCE_CLASS *getFailedInstance(UINT);

	string getStudyComponentSopClassUid()
		{ return studyComponentM.getSopClassUid(); }

	string getStudyComponentSopInstanceUid()
		{ return studyComponentM.getSopInstanceUid(); }

public:
	STORAGE_COMMITMENT_CLASS();
	~STORAGE_COMMITMENT_CLASS();

	void setEventToSend(bool eventToSend) { eventToSendM = eventToSend; }

	bool isEventToSend() { return eventToSendM; }

	UINT16 action(DCM_DATASET_CLASS*, LOG_CLASS*);

	UINT16 eventReport(DCM_DATASET_CLASS**, LOG_CLASS*);

	UINT noFailedInstances() { return failedInstanceM.getSize(); }
};

#endif /* COMMITMENT_H */


