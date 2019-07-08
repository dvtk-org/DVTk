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

#ifndef RELATION_H
#define RELATION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "patient_list.h"
#include "stored_sop_list.h"


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_DATASET_CLASS;
class LOG_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define RELATIONSHIP	RELATIONSHIP_CLASS::instance()

//>>***************************************************************************

class RELATIONSHIP_CLASS

//  DESCRIPTION     : Class used to store the relationships.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	static RELATIONSHIP_CLASS	*instanceM_ptr;	// Singleton
	PATIENT_LIST_CLASS			patientListM;
	STORED_SOP_LIST_CLASS		storedSopListM;

	RELATIONSHIP_CLASS();

	~RELATIONSHIP_CLASS();

	void		initSemaphore					(void);
	void		postSemaphore					(void);
	void		waitSemaphore					(void);
	void		termSemaphore					(void);

public:

	static RELATIONSHIP_CLASS *	instance		(void);

	void		cleanup							(void);

	UINT16		analyseStorageDataset			(DCM_DATASET_CLASS*, 
												 string&, 
												 LOG_CLASS*,
												 bool isAccept = false);

	bool		commit							(string		  sopClassUid,
												 string		  sopInstanceUid,
												 LOG_CLASS	* logger_ptr);

	void		logObjectRelationshipAnalysis	(LOG_CLASS	* logger_ptr);
};

#endif /* RELATION_H */
