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
//  DESCRIPTION     :	Storage SCP emulator class.
//*****************************************************************************
#ifndef STORAGE_SCP_EMULATOR_H
#define STORAGE_SCP_EMULATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"				// Global component interface
#include "emulator.h"				// Base emulator class

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_COMMAND_CLASS;
class DCM_DATASET_CLASS;
class MEDIA_FILE_HEADER_CLASS;
class STORAGE_COMMITMENT_CLASS;


//>>***************************************************************************

class STORAGE_SCP_EMULATOR_CLASS : public BASE_SCP_EMULATOR_CLASS 

//  DESCRIPTION     : Storage emulator class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	STORAGE_COMMITMENT_CLASS *storageCommitmentM_ptr;

	bool processStore(BYTE, DCM_DATASET_CLASS*);

	bool processAction(BYTE, DCM_DATASET_CLASS*);

protected:
	STORAGE_COMMITMENT_CLASS * getStorageCommitObject()
	{
		return storageCommitmentM_ptr;
	}

	bool processCommandDataset(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
	
	bool postProcess();

	bool sendEventReport();

	void completeLogging();

public:
	STORAGE_SCP_EMULATOR_CLASS(EMULATOR_SESSION_CLASS*, BASE_SOCKET_CLASS*, bool);
	~STORAGE_SCP_EMULATOR_CLASS();

	static bool addSupportedPresentationContexts(EMULATOR_SESSION_CLASS*);
};

#endif /* STORAGE_SCP_EMULATOR_H */
