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

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "commitment.h"
#include "Idicom.h"		// DICOM component interface


//>>===========================================================================

STORAGE_COMMITMENT_CLASS::STORAGE_COMMITMENT_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	eventToSendM = false;
}

//>>===========================================================================

STORAGE_COMMITMENT_CLASS::~STORAGE_COMMITMENT_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	while (storedInstanceM.getSize())
	{
		delete storedInstanceM[0];
		storedInstanceM.removeAt(0);
	}

	while (failedInstanceM.getSize())
	{
		delete failedInstanceM[0];
		failedInstanceM.removeAt(0);
	}
}

//>>===========================================================================

void STORAGE_COMMITMENT_CLASS::addStoredInstance(string sopClassUid, string sopInstanceUid)

//  DESCRIPTION     : Add a stored sop instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// add an entry to the stored list
	STORED_SOP_INSTANCE_CLASS *instance_ptr = new STORED_SOP_INSTANCE_CLASS(sopClassUid, sopInstanceUid);
	storedInstanceM.add(instance_ptr);
}

//>>===========================================================================

void STORAGE_COMMITMENT_CLASS::addFailedInstance(string sopClassUid, string sopInstanceUid)

//  DESCRIPTION     : Add a failed sop instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// add an entry to the failed list
	STORED_SOP_INSTANCE_CLASS *instance_ptr = new STORED_SOP_INSTANCE_CLASS(sopClassUid, sopInstanceUid);
	failedInstanceM.add(instance_ptr);
}

//>>===========================================================================

STORED_SOP_INSTANCE_CLASS *STORAGE_COMMITMENT_CLASS::getStoredInstance(UINT i)

//  DESCRIPTION     : Get the indexed stored sop instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	STORED_SOP_INSTANCE_CLASS *instance_ptr = NULL;
	
	// check for valid index
	if (i < storedInstanceM.getSize())
	{
		// get stored instance
		instance_ptr = storedInstanceM[i];
	}

	// return the stored instance
	return instance_ptr;
}

//>>===========================================================================

STORED_SOP_INSTANCE_CLASS *STORAGE_COMMITMENT_CLASS::getFailedInstance(UINT i)

//  DESCRIPTION     : Get the indexed failed sop instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	STORED_SOP_INSTANCE_CLASS *instance_ptr = NULL;
	
	// check for valid index
	if (i < failedInstanceM.getSize())
	{
		// get failed instance
		instance_ptr = failedInstanceM[i];
	}

	// return the failed instance
	return instance_ptr;
}

//>>===========================================================================

UINT16 STORAGE_COMMITMENT_CLASS::action(DCM_DATASET_CLASS *dataset_ptr, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Build the storage commitment class from the given dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string sopClassUid;
	string sopInstanceUid;

	// check for valid dataset
	if (dataset_ptr == NULL) return DCM_STATUS_PROCESSING_FAILURE;

	// get the Transaction UID
	if (!dataset_ptr->getUIValue(TAG_TRANSACTION_UID, sopInstanceUid))
	{
		// Missing Attribute
		return DCM_STATUS_MISSING_ATTRIBUTE;
	}
	setTransactionUid(sopInstanceUid);

	// get the Reference SOP Sequence
	DCM_ATTRIBUTE_CLASS *attribute_ptr = dataset_ptr->GetAttributeByTag(TAG_REFERENCED_SOP_SEQUENCE);
	if (attribute_ptr == NULL)
	{
		// Missing Attribute
		return DCM_STATUS_MISSING_ATTRIBUTE;
	}
	
	// check that we have a sequence and that it contains items
	if ((attribute_ptr->GetVR() != ATTR_VR_SQ) ||
		(attribute_ptr->GetNrValues() != 1))
	{
		// Treat it as an invalid attribute value
		return DCM_STATUS_INVALID_ATTRIBUTE_VALUE;
	}

	// get the sequence
	DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

	// now handle all the Referenced SOP Items
	for (int i = 0; i < sqValue_ptr->GetNrItems(); i++)
	{
		DCM_ITEM_CLASS *item_ptr = sqValue_ptr->getItem(i);
		if (item_ptr == NULL) return  DCM_STATUS_MISSING_ATTRIBUTE;

		// get the SOP Class UID and SOP Instance UID from the item
		if (!item_ptr->getUIValue(TAG_REFERENCED_SOP_CLASS_UID, sopClassUid))
		{
			// Missing Attribute
			return DCM_STATUS_MISSING_ATTRIBUTE;
		}

		if (!item_ptr->getUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, sopInstanceUid))
		{
			// Missing Attribute
			return DCM_STATUS_MISSING_ATTRIBUTE;
		}

		// check if we have stored the corresponding SOP instance
		if (RELATIONSHIP->commit(sopClassUid, sopInstanceUid, logger_ptr))
		{
			// add to stored sop list
			addStoredInstance(sopClassUid, sopInstanceUid);

			logger_ptr->text(LOG_INFO, 1, "Commitment of SOP Instance UID: %s is successful.", sopInstanceUid.c_str());
		}
		else
		{
			// add to failed sop list
			addFailedInstance(sopClassUid, sopInstanceUid);

			logger_ptr->text(LOG_INFO, 1, "Commitment of SOP Instance UID: %s is failed.", sopInstanceUid.c_str());
		}
	}

	// finally try to get the Reference Study Component Sequence
	attribute_ptr = dataset_ptr->GetAttributeByTag(TAG_REFERENCED_STUDY_COMPONENT_SEQUENCE);
	if (attribute_ptr != NULL)
	{
		// check that we have a sequence and that it contains items
		if ((attribute_ptr->GetVR() != ATTR_VR_SQ) ||
			(attribute_ptr->GetNrValues() != 1))
		{
			// Treat it as an invalid attribute value
			return DCM_STATUS_INVALID_ATTRIBUTE_VALUE;
		}

		// get the sequence
		DCM_VALUE_SQ_CLASS *sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

		// now handle the Referenced SOP Item
		if (sqValue_ptr->GetNrItems() == 1)
		{
			DCM_ITEM_CLASS *item_ptr = sqValue_ptr->getItem(0);
			if (item_ptr == NULL) return  DCM_STATUS_MISSING_ATTRIBUTE;

			// get the SOP Class UID and SOP Instance UID from the item
			if (!item_ptr->getUIValue(TAG_REFERENCED_SOP_CLASS_UID, sopClassUid))
			{
				// Missing Attribute
				return DCM_STATUS_MISSING_ATTRIBUTE;
			}

			if (!item_ptr->getUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, sopInstanceUid))
			{
				// Missing Attribute
				return DCM_STATUS_MISSING_ATTRIBUTE;
			}

			// save the SOP Class and Instance UIDs
			setStudyComponentUids(sopClassUid, sopInstanceUid);
		}
	}

	// indicate that an event should be sent too
	eventToSendM = true;

	// return status
	return DCM_STATUS_SUCCESS;
}

//>>===========================================================================

UINT16 STORAGE_COMMITMENT_CLASS::eventReport(DCM_DATASET_CLASS **dataset_ptr_ptr, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Build an N-EVENT-REPORT-RSP dataset from the storage
//					: commitment class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	*dataset_ptr_ptr = NULL;

	// set up the response dataset
	DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();
			
	// set up the command id and iod name fields
	dataset_ptr->setCommandId(DIMSE_CMD_NEVENTREPORT_RQ);
	dataset_ptr->setIodName("Commitment Push");

	// add the Transaction UID
	dataset_ptr->setUIValue(TAG_TRANSACTION_UID, transactionUidM);

    // generate a SQ value
	DCM_VALUE_SQ_CLASS *sqValue_ptr = NULL;

	// add the stored sop instances
	for (UINT i = 0; i < storedInstanceM.getSize(); i++) 
	{
		// make sure that we have a referenced sequence
		if (i == 0) 
		{
			DCM_ATTRIBUTE_CLASS *refSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_SOP_SEQUENCE, ATTR_VR_SQ);
			dataset_ptr->addAttribute(refSq_ptr);
			sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
			refSq_ptr->AddValue(sqValue_ptr);
		}

		// generate an item
		DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

		// add SOP Class UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, storedInstanceM[i]->getSopClassUid());

		// add SOP Instance UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, storedInstanceM[i]->getSopInstanceUid());

		// add the item to the dataset
		sqValue_ptr->Set(item_ptr);
	}

	// add the failed sop instances
	for (UINT i = 0; i < failedInstanceM.getSize(); i++) 
	{
		// make sure that we have a referenced sequence
		if (i == 0) 
		{
			DCM_ATTRIBUTE_CLASS *refSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_FAILED_SOP_SEQUENCE, ATTR_VR_SQ);
			dataset_ptr->addAttribute(refSq_ptr);
			sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
			refSq_ptr->AddValue(sqValue_ptr);
		}

		// generate an item
		DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

		// add SOP Class UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, failedInstanceM[i]->getSopClassUid());

		// add SOP Instance UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, failedInstanceM[i]->getSopInstanceUid());

		// add Failure Reason
		UINT16 us_value = DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
		item_ptr->setUSValue(TAG_FAILURE_REASON, us_value);

		// add the item to the dataset
		sqValue_ptr->Set(item_ptr);
	}

	// cascade the logger
	dataset_ptr->setLogger(logger_ptr);

	// return the dataset
	*dataset_ptr_ptr = dataset_ptr;

	// return status
	return DCM_STATUS_SUCCESS;
}
