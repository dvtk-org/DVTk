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
#include "stored_sop_list.h"
#include "stored_sop_instance.h"
#include "Ilog.h"				// Log component interface
#include "Idicom.h"				// Dicom component interface


//>>***************************************************************************

STORED_SOP_LIST_CLASS::STORED_SOP_LIST_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}

//>>***************************************************************************

STORED_SOP_LIST_CLASS::~STORED_SOP_LIST_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	cleanup();
}

//>>***************************************************************************

void STORED_SOP_LIST_CLASS::cleanup()

//  DESCRIPTION     : Cleanup the stored sop list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	while (sopInstanceM.getSize())
	{
		delete sopInstanceM[0];
		sopInstanceM.removeAt(0);
	}
}

//>>***************************************************************************

STORED_SOP_INSTANCE_CLASS *STORED_SOP_LIST_CLASS::search(string sopClassUid,
														 string sopInstanceUid,
														 LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Search the stored list for a match - check class and instance
//					: combinations.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	STORED_SOP_INSTANCE_CLASS *storedSopInstance_ptr = NULL;

	// seach list of stored sop instances for a match
	for (UINT i = 0; i < sopInstanceM.getSize(); i++)
	{
		if ((sopInstanceM[i]->getSopClassUid() == sopClassUid) &&
			(sopInstanceM[i]->getSopInstanceUid() == sopInstanceUid))
		{
			// matching entry found - return it
			storedSopInstance_ptr = sopInstanceM[i];
			break;
		}
		else
		{
			if (sopInstanceM[i]->getSopInstanceUid() == sopInstanceUid)
			{
				// instance uid has already been used in a different sop class
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1,
									 "(0008,0018) SOP Instance UID %s is used in two different SOP Classes - %s and %s",
									 sopInstanceUid.c_str(),
									 sopInstanceM[i]->getSopClassUid().c_str(),
									 sopClassUid.c_str());
				}
			}
		}
	}

	// return sop instance
	return storedSopInstance_ptr;
}

//>>***************************************************************************

UINT16 STORED_SOP_LIST_CLASS::analyseStorageDataset(DCM_DATASET_CLASS *dataset_ptr,
													string&			   msg,
													LOG_CLASS		  *logger_ptr,
													bool			   isAccept)

//  DESCRIPTION     : Analyse the Storage Dataset for SOP Class UID and SOP
//                    Instance UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	STORED_SOP_INSTANCE_CLASS *storedSopInstance_ptr;
	string sopClassUid;
	string sopInstanceUid;

	// check that the appriopriate attributes are available
	if (!dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, sopClassUid))
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_WARNING, 1, "(0008,0016) SOP (Image) Class UID not available for Object Relationship Analysis");
		}
		return DCM_STATUS_MISSING_ATTRIBUTE;
	}

	if (!dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, sopInstanceUid))
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_WARNING, 1, "(0008,0018) SOP (Image) Instance UID not available for Object Relationship Analysis");
		}
		return DCM_STATUS_MISSING_ATTRIBUTE;
	}
	
	// check if the sop instance has already been seen
	storedSopInstance_ptr = search(sopClassUid, sopInstanceUid, logger_ptr);
	if (storedSopInstance_ptr == NULL)
	{
		// store new SOP instance uid
		storedSopInstance_ptr = new STORED_SOP_INSTANCE_CLASS(sopClassUid, sopInstanceUid);
		sopInstanceM.add(storedSopInstance_ptr);
		return DCM_STATUS_SUCCESS;
	}
	else
	{
		if(!isAccept)
		{
			if (logger_ptr)
			{
				logger_ptr->text(LOG_ERROR, 1,
								"Duplicate - This SOP Instance UID found for SOP Class UID %s and SOP Instance UID %s was already stored.",
								storedSopInstance_ptr->getSopClassUid().c_str(),
								storedSopInstance_ptr->getSopInstanceUid().c_str());
				
				char	buffer[1024];
				sprintf(buffer, "Error: Duplicate - This SOP Instance UID found for SOP Class UID %s and SOP Instance UID %s was already stored.",
								storedSopInstance_ptr->getSopClassUid().c_str(),
								storedSopInstance_ptr->getSopInstanceUid().c_str());
				msg = buffer;
			}

			// sop instance appears more than once
			storedSopInstance_ptr->incrementCount();
			return DCM_STATUS_DUPLICATE_SOPINSTANCE;
		}
		else
		{
			// sop instance appears more than once
			storedSopInstance_ptr->incrementCount();
			sopInstanceM.add(storedSopInstance_ptr);
			return DCM_STATUS_SUCCESS;
		}
	}
}

//>>***************************************************************************

void STORED_SOP_LIST_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log the stored sop list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (logger_ptr == NULL) return;

	if (sopInstanceM.getSize() > 0) 
	{
		logger_ptr->text(LOG_INFO, 1, "Stored SOP Instance List");

		// log stored instances
		for (UINT i = 0; i < sopInstanceM.getSize(); i++) 
		{
			logger_ptr->text(LOG_INFO, 1, "\tSTORED SOP INSTANCE %d of %d", i + 1, sopInstanceM.getSize());
			sopInstanceM[i]->log(logger_ptr);
		}
	}
}

