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
//  DESCRIPTION     :	Query/Retrieve SCP emulator class.
//*****************************************************************************
#ifndef QR_SCP_EMULATOR_H
#define QR_SCP_EMULATOR_H

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


//>>***************************************************************************

class QUERY_RETRIEVE_SCP_EMULATOR_CLASS : public BASE_SCP_EMULATOR_CLASS 

//  DESCRIPTION     : Query/Retrieve emulator class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	bool processFind(BYTE, DCM_DATASET_CLASS*);

	bool processMove(BYTE, DCM_DATASET_CLASS*);

	bool processGet(BYTE, DCM_DATASET_CLASS*);

protected:
	bool processCommandDataset(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);

	void completeLogging();

public:
	QUERY_RETRIEVE_SCP_EMULATOR_CLASS(EMULATOR_SESSION_CLASS*, BASE_SOCKET_CLASS*, bool);
	~QUERY_RETRIEVE_SCP_EMULATOR_CLASS();

	static bool addSupportedPresentationContexts(EMULATOR_SESSION_CLASS*);
};

#endif /* QR_SCP_EMULATOR_H */
