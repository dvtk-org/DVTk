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
#include "patient_list.h"
#include "patient_data.h"
#include "study_data.h"
#include "series_data.h"
#include "sop_instance_data.h"
#include "Ilog.h"					// Log component interface
#include "Idicom.h"					// Dicom component interface


//>>===========================================================================

PATIENT_LIST_CLASS::PATIENT_LIST_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
}
	
//>>===========================================================================

PATIENT_LIST_CLASS::~PATIENT_LIST_CLASS()

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

//>>===========================================================================

UINT PATIENT_LIST_CLASS::noPatients (void)

//  DESCRIPTION     : Return the number of patients in the relationship
//                    component.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return patientDataM.getSize();
}

//>>===========================================================================

PATIENT_DATA_CLASS * PATIENT_LIST_CLASS::getPatientData (UINT patient_index)

//  DESCRIPTION     : Return the patient information of the patient identified
//                    by patient_index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return patientDataM[patient_index];
}

//>>===========================================================================

void PATIENT_LIST_CLASS::addPatientData (PATIENT_DATA_CLASS * patData_ptr)

//  DESCRIPTION     : Add the given patient information to the relationship
//                    component.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	patientDataM.add(patData_ptr);
}

//>>===========================================================================

void PATIENT_LIST_CLASS::cleanup()

//  DESCRIPTION     : Cleanup the patient list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// clean up any patient data
	while (patientDataM.getSize())
	{
		delete patientDataM[0];
		patientDataM.removeAt(0);
	}
}

//>>===========================================================================

PATIENT_DATA_CLASS *PATIENT_LIST_CLASS::search(string id, string name, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Search the Patient List for Patient Data with an id (and
//					: optional name) matching that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// search patient data
	for (UINT i = 0; i < patientDataM.getSize(); i++)
	{
		PATIENT_DATA_CLASS *patientData_ptr = patientDataM[i];

		// check for match
		if ((patientData_ptr != NULL) &&
			(id == patientData_ptr->getPatientId()))
		{
			// check if name defined
			if ((name.length() != 0) &&
				(patientData_ptr->getPatientName().length() != 0) &&
				(name != patientData_ptr->getPatientName()))
			{
				// patient ids match - but names do not
				// - issue warning
				if (logger_ptr)
				{
					logger_ptr->text(LOG_WARNING, 1,"(0010,0010) Patient Name mis-match during Object Relationship Analysis: \"%s\" and \"%s\"", name.c_str(), patientData_ptr->getPatientName().c_str());
				}
			}

			// match on Patient Id
			return patientData_ptr;
		}
	}

	// no match found
	return NULL;
}

//>>===========================================================================

void PATIENT_LIST_CLASS::analyseStorageDataset(DCM_DATASET_CLASS *dataset_ptr,
											   LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Analyse the dataset of a C-STORE-RQ image. The 
//					: identification information will be used to check whether
//					: any relationship exists between this and previous/future
//					: objects.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	PATIENT_DATA_CLASS *patientData_ptr;
	STUDY_DATA_CLASS *studyData_ptr;
	SERIES_DATA_CLASS *seriesData_ptr;
	SOP_INSTANCE_DATA_CLASS *sopInstanceData_ptr;

	// check that the appriopriate attributes are available
	// Patient ID
	string patientId;
	dataset_ptr->getLOValue(TAG_PATIENT_ID, patientId);
	if (patientId.length() == 0)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_IMAGE_RELATION, 1, "(0010,0020) Patient ID not available for Object Relationship Analysis");
		}
		return;
	}

	// Patient Name
	string patientName;
	dataset_ptr->getPNValue(TAG_PATIENTS_NAME, patientName);
	if (patientName.length() == 0)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_IMAGE_RELATION, 1, "(0010,0010) Patient Name not available for Object Relationship Analysis");
		}
	}

	// Study Instance UID
	string studyInstanceUid;
	dataset_ptr->getUIValue(TAG_STUDY_INSTANCE_UID, studyInstanceUid);
	if (studyInstanceUid.length() == 0)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_IMAGE_RELATION, 1, "(0020,000D) Study Instance UID not available for Object Relationship Analysis");
		}
		return;
	}

	// Series Instance UID
	string seriesInstanceUid;
	dataset_ptr->getUIValue(TAG_SERIES_INSTANCE_UID, seriesInstanceUid);
	if (seriesInstanceUid.length() == 0)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_IMAGE_RELATION, 1, "(0020,000E) Series Instance UID not available for Object Relationship Analysis");
		}
		return;
	}

	// Object SOP Instance UID
	string sopInstanceUid;
	dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, sopInstanceUid);
	if (sopInstanceUid.length() == 0)
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_IMAGE_RELATION, 1, "(0008,0018) SOP (Image) Instance UID not available for Object Relationship Analysis");
		}
		return;
	}
	
	// Optional Frame Of Reference UID
	string frameOfReferenceUid;
	dataset_ptr->getUIValue(TAG_FRAME_OF_REFERENCE_UID, frameOfReferenceUid);

	// Optional Image Type Value 3
	string imageTypeValue3;
	dataset_ptr->getCSValue(TAG_IMAGE_TYPE, imageTypeValue3, 2);

	// Optional Referenced Image Sequence
	string referencedImageUid;
	DCM_ATTRIBUTE_CLASS *attribute_ptr = dataset_ptr->GetAttributeByTag(TAG_REFERENCED_IMAGE_SEQUENCE);
	if ((attribute_ptr != NULL) &&
		(attribute_ptr->GetVR() == ATTR_VR_SQ) &&
		(attribute_ptr->GetNrValues() != 0)) 
	{
		DCM_VALUE_SQ_CLASS *sq_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(attribute_ptr->GetValue(0));

		if ((sq_ptr != NULL) &&
			(sq_ptr->GetNrItems() != 0))
		{
			// now try and get the Referenced SOP Instance UID
			DCM_ITEM_CLASS *item_ptr = sq_ptr->getItem(0);
			
			if (item_ptr != NULL)
			{
				item_ptr->getUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, referencedImageUid);
			}
		}
	}

	// now check if a matching Patient has already been set up
	if ((patientData_ptr = search(patientId, patientName, logger_ptr)) == NULL)
	{
		// create SOP Instance Data
		sopInstanceData_ptr = new SOP_INSTANCE_DATA_CLASS(sopInstanceUid, frameOfReferenceUid, imageTypeValue3, referencedImageUid);

		// create Series Data
		seriesData_ptr = new SERIES_DATA_CLASS(seriesInstanceUid);
		seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
	
		// create Study Data
		studyData_ptr = new STUDY_DATA_CLASS(studyInstanceUid);
		studyData_ptr->addSeriesData(seriesData_ptr);

		// create Patient Data
		patientData_ptr = new PATIENT_DATA_CLASS(patientId, patientName);
		patientData_ptr->addStudyData(studyData_ptr);

		// add Patient Data
		addPatientData(patientData_ptr);
	}
	else 
	{
		// patient already exists - check for study
		if ((studyData_ptr = patientData_ptr->search(studyInstanceUid)) == NULL) 
		{
			// create SOP Instance Data
			sopInstanceData_ptr = new SOP_INSTANCE_DATA_CLASS(sopInstanceUid, frameOfReferenceUid, imageTypeValue3, referencedImageUid);

			// create Series Data
			seriesData_ptr = new SERIES_DATA_CLASS(seriesInstanceUid);
			seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
	
			// create Study Data
			studyData_ptr = new STUDY_DATA_CLASS(studyInstanceUid);
			studyData_ptr->addSeriesData(seriesData_ptr);
		
			// add Study Data
			patientData_ptr->addStudyData(studyData_ptr);
		}
		else 
		{
			// study already exists - check for series
			if ((seriesData_ptr = studyData_ptr->search(seriesInstanceUid)) == NULL)
			{
				// create SOP Instance Data
				sopInstanceData_ptr = new SOP_INSTANCE_DATA_CLASS(sopInstanceUid, frameOfReferenceUid, imageTypeValue3, referencedImageUid);

				// create Series Data
				seriesData_ptr = new SERIES_DATA_CLASS(seriesInstanceUid);
				seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
	
				// add Series Data
				studyData_ptr->addSeriesData(seriesData_ptr);
			}
			else 
			{
				// series already exists - check for sop instance
				if ((sopInstanceData_ptr = seriesData_ptr->search(sopInstanceUid)) == NULL)
				{
					// create SOP Instance Data
					sopInstanceData_ptr = new SOP_INSTANCE_DATA_CLASS(sopInstanceUid, frameOfReferenceUid, imageTypeValue3, referencedImageUid);

					// add SOP Instance Data
					seriesData_ptr->addSopInstanceData(sopInstanceData_ptr);
				}
				else 
				{
					// sop instance exists - we've got the same one again!!
					sopInstanceData_ptr->incrementCount();
				}
			}
		}
	}
}

//>>===========================================================================

void PATIENT_LIST_CLASS::log(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Log the Patient List.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid logger
	if (logger_ptr == NULL) return;

	// save old log level - and set new
	UINT32 oldLogLevel = logger_ptr->logLevel(LOG_IMAGE_RELATION);

	if (patientDataM.getSize() > 0) 
	{
		// display patient data
		logger_ptr->text(2, "Object(Image) Relationship Analysis...");

		// patient data
		for (UINT i = 0; i < patientDataM.getSize(); i++)
		{
			PATIENT_DATA_CLASS *patientData_ptr = patientDataM[i];

			// dump the patient data
			if (patientData_ptr != NULL)
			{
				logger_ptr->text(LOG_NONE, 1, "PATIENT %d of %d", i + 1, patientDataM.getSize());
				patientData_ptr->log(logger_ptr);
			}
		}
	}
	
	// restore original log level
	logger_ptr->logLevel(oldLogLevel);
}
