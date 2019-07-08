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
#include "relation.h"
#include "stored_sop_instance.h"
#include "Ilog.h"					// Log component interface

//*****************************************************************************
// initialise static pointers
//*****************************************************************************
RELATIONSHIP_CLASS *RELATIONSHIP_CLASS::instanceM_ptr = NULL;


//>>===========================================================================
RELATIONSHIP_CLASS::RELATIONSHIP_CLASS()
//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// - initialise the access semaphore
	initSemaphore();
}

//>>===========================================================================
RELATIONSHIP_CLASS::~RELATIONSHIP_CLASS()
//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	cleanup();

	// terminate the access semaphore
	termSemaphore();
}

//>>===========================================================================
RELATIONSHIP_CLASS *RELATIONSHIP_CLASS::instance()
//  DESCRIPTION     : Singleton instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) {
		instanceM_ptr = new RELATIONSHIP_CLASS();
	}

	return instanceM_ptr;
}

//
// set up mutual exclusion
//
#ifdef _WINDOWS
//CCriticalSection	RelationshipAccess;

//>>===========================================================================
void RELATIONSHIP_CLASS::initSemaphore()
//  DESCRIPTION     : Initialize the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// do nothing
}

//>>===========================================================================
void RELATIONSHIP_CLASS::postSemaphore()
//  DESCRIPTION     : Post the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// post the semaphore
	//RelationshipAccess.Unlock();
}

//>>===========================================================================
void RELATIONSHIP_CLASS::waitSemaphore()
//  DESCRIPTION     : Wait for the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for the semaphore
	//RelationshipAccess.Lock();
}

//>>===========================================================================
void RELATIONSHIP_CLASS::termSemaphore()
//  DESCRIPTION     : Terminate the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// do nothing
}

#else
sem_t	RelationshipSemaphoreId;

//>>===========================================================================
void RELATIONSHIP_CLASS::initSemaphore()
//  DESCRIPTION     : Initialize the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise the semaphore
	if (sem_init(&RelationshipSemaphoreId, 0, 1) != 0) {
		printf("\nFailed to create a semaphore for database locking - exiting...");
		exit(0);
	}
}

//>>===========================================================================
void RELATIONSHIP_CLASS::postSemaphore()
//  DESCRIPTION     : Post the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// post the semaphore
	if (sem_post(&RelationshipSemaphoreId) != 0) {
		printf("\nFailed to post to semaphore for database locking - exiting...");
		exit(0);
	}
}

//>>===========================================================================
void RELATIONSHIP_CLASS::waitSemaphore()
//  DESCRIPTION     : Wait for the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for the semaphore
	if (sem_wait(&RelationshipSemaphoreId) != 0) {
		printf("\nFailed to wait on semaphore for database locking - exiting...");
		exit(0);	
	}
}

//>>===========================================================================
void RELATIONSHIP_CLASS::termSemaphore()
//  DESCRIPTION     : Terminate the relationship access semaphore.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destroy the semaphore
	if (sem_destroy(&RelationshipSemaphoreId) != 0) {
		printf("\nFailed to destroy the semaphore for database locking - exiting...");
		exit(0);	
	}
}

#endif

//>>===========================================================================
void RELATIONSHIP_CLASS::cleanup()
//  DESCRIPTION     : Cleanup the relationship data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for access to warehouse
	waitSemaphore();

	// cleanup the patient list - instantiate it again
	patientListM.cleanup();

	// cleanup the stored sop list - instantiate it again
	storedSopListM.cleanup();

	// release access to warehouse
	postSemaphore();
}

//>>===========================================================================
UINT16 RELATIONSHIP_CLASS::analyseStorageDataset(DCM_DATASET_CLASS *dataset_ptr, 
												 string& msg, 
												 LOG_CLASS *logger_ptr,
												 bool isAcceptDuplicateImage)
//  DESCRIPTION     : Analyse the Storage Dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for access to warehouse
	waitSemaphore();

	UINT16 status = DCM_STATUS_PROCESSING_FAILURE;

	// analyse the Storage Dataset for the patient relationship
	patientListM.analyseStorageDataset(dataset_ptr, logger_ptr);

	// analyse the Storage Dataset for the stored sop
	status = storedSopListM.analyseStorageDataset(dataset_ptr, msg, logger_ptr, isAcceptDuplicateImage);

	// release access to warehouse
	postSemaphore();

	return status;
}

//>>===========================================================================
bool RELATIONSHIP_CLASS::commit(string		  sopClassUid,
								string		  sopInstanceUid,
								LOG_CLASS	* logger_ptr)
//  DESCRIPTION     : Commit the storage if the sop instance can be found.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	STORED_SOP_INSTANCE_CLASS*	  storedSopInstance_ptr;
	bool						  result = false;

	// wait for access to warehouse
	waitSemaphore();

	// check if the sop is stored
	storedSopInstance_ptr = storedSopListM.search(sopClassUid,
												  sopInstanceUid,
												  logger_ptr);
	if (storedSopInstance_ptr)
	{
		// commit the storage
		storedSopInstance_ptr->commit();

		// return success
		result = true;
	}

	// release access to warehouse
	postSemaphore();

	// return result
	return result;
}

//>>===========================================================================
void RELATIONSHIP_CLASS::logObjectRelationshipAnalysis(LOG_CLASS *logger_ptr)
//  DESCRIPTION     : Log the Object Relationship Analysis.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// wait for access to warehouse
	waitSemaphore();

	// log the patient list
	patientListM.log(logger_ptr);

	// log the stored sop list
	storedSopListM.log(logger_ptr);

	// release access to warehouse
	postSemaphore();
}
