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
//  DESCRIPTION     :	Printer emulation classes.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "print.h"
#include "Idefinition.h"	// Definition component interface
#include "Isession.h"		// Session component interface
#include "image_display_file.h"

//>>===========================================================================

BASE_SOP_CLASS::~BASE_SOP_CLASS()

//  DESCRIPTION     : Base class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// empty virtual destructor
}

//>>===========================================================================

UINT16 BASE_SOP_CLASS::set(DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Perform a SET operation - equates to an object merge.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// first set ?
	if (!datasetM_ptr)
	{
		// store incoming dataset
		datasetM_ptr = dataset_ptr;
	}
	else 
	{
		// need to merge datasets
		datasetM_ptr->merge(dataset_ptr);
		delete dataset_ptr;
	}

	// return successful completion
	return DCM_STATUS_SUCCESS;
}


//>>===========================================================================

IMAGE_BOX_CLASS::IMAGE_BOX_CLASS(string sopClassUid, string sopInstanceUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sopClassUidM = sopClassUid;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = NULL;
	loggerM_ptr = NULL;
}

//>>===========================================================================

IMAGE_BOX_CLASS::~IMAGE_BOX_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (datasetM_ptr)
	{
		delete datasetM_ptr;
	}
}

//>>===========================================================================

UINT16 IMAGE_BOX_CLASS::set(DCM_DATASET_CLASS *dataset_ptr) 

//  DESCRIPTION     : Handle SET operation.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_ATTRIBUTE_CLASS	*attribute_ptr;

	if (getSopClassUid() == GRAY_IMAGE_BOX_SOP_CLASS_UID) 
	{
		// Grayscale Image Box SOP Class
		attribute_ptr = dataset_ptr->GetAttributeByTag(TAG_BASIC_GRAYSCALE_IMAGE_SEQUENCE);

		// check for deletion
		if ((attribute_ptr) &&
			(attribute_ptr->GetNrValues() == 0)) 
		{
			// zero-length SQ indicates deletion
			if (datasetM_ptr) 
			{
				delete datasetM_ptr;
				datasetM_ptr = NULL;

				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_INFO, 1, "Deleted Grayscale Image Box SOP Class with Instance UID: %s", (char*) sopInstanceUidM.c_str());
				}
			}
			delete dataset_ptr;
		}
		else 
		{
			// maybe need to merge the datasets
			if (datasetM_ptr) 
			{
				datasetM_ptr->merge(dataset_ptr);
				delete dataset_ptr;
			}
			else 
			{
				datasetM_ptr = dataset_ptr;
			}
		}
	}
	else if (getSopClassUid() == COLOR_IMAGE_BOX_SOP_CLASS_UID)
	{
		// Color Image Box SOP Class
		attribute_ptr = dataset_ptr->GetAttributeByTag(TAG_BASIC_COLOR_IMAGE_SEQUENCE);

		// check for deletion
		if ((attribute_ptr) &&
			(attribute_ptr->GetNrValues() == 0))
		{
			// zero-length SQ indicates deletion
			if (datasetM_ptr) 
			{
				delete datasetM_ptr;
				datasetM_ptr = NULL;

				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_INFO, 1, "Deleted Color Image Box SOP Class with Instance UID: %s", (char*) sopInstanceUidM.c_str());
				}
			}
			delete dataset_ptr;
		}
		else 
		{
			// maybe need to merge the datasets
			if (datasetM_ptr) 
			{
				datasetM_ptr->merge(dataset_ptr);
				delete dataset_ptr;
			}
			else 
			{
				datasetM_ptr = dataset_ptr;
			}
		}
	}
	else 
	{
		// Referenced Image Box SOP Class UID
		attribute_ptr = dataset_ptr->GetAttributeByTag(TAG_REFERENCED_IMAGE_SEQUENCE);

		// check for deletion
		if ((attribute_ptr) &&
			(attribute_ptr->GetNrValues() == 0))
		{
			// zero-length SQ indicate deletion
			if (datasetM_ptr)
			{
				delete datasetM_ptr;
				datasetM_ptr = NULL;

				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_INFO, 1, "Deleted Referenced Image Box SOP Class with Instance UID: %s", (char*) sopInstanceUidM.c_str());
				}
			}
			delete dataset_ptr;
		}
		else 
		{
			// maybe need to merge the datasets
			if (datasetM_ptr)
			{
				datasetM_ptr->merge(dataset_ptr);
				delete dataset_ptr;
			}
			else
			{
				datasetM_ptr = dataset_ptr;
			}
		}
	}

	// return successful completion
	return DCM_STATUS_SUCCESS;
}


//>>===========================================================================

IMAGE_OVERLAY_CLASS::IMAGE_OVERLAY_CLASS(string sopInstanceUid, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sopClassUidM = IMAGE_OVERLAY_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = dataset_ptr;
	loggerM_ptr = NULL;
}

//>>===========================================================================

IMAGE_OVERLAY_CLASS::~IMAGE_OVERLAY_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (datasetM_ptr) 
	{
		delete datasetM_ptr;
	}
}


//>>===========================================================================

ANNOTATION_BOX_CLASS::ANNOTATION_BOX_CLASS(string sopInstanceUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sopClassUidM = ANNOTATION_BOX_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = NULL;
	loggerM_ptr = NULL;
}

//>>===========================================================================

ANNOTATION_BOX_CLASS::~ANNOTATION_BOX_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (datasetM_ptr) 
	{
		delete datasetM_ptr;
	}
}


//>>===========================================================================

PRESENTATION_LUT_CLASS::PRESENTATION_LUT_CLASS(string sopInstanceUid, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sopClassUidM = PRESENTATION_LUT_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = dataset_ptr;
	loggerM_ptr = NULL;
}

//>>===========================================================================

PRESENTATION_LUT_CLASS::~PRESENTATION_LUT_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (datasetM_ptr)
	{
		delete datasetM_ptr;
	}
}


//>>===========================================================================

VOI_LUT_BOX_CLASS::VOI_LUT_BOX_CLASS(string sopInstanceUid, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sopClassUidM = VOI_LUT_BOX_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = dataset_ptr;
	loggerM_ptr = NULL;
}

//>>===========================================================================

VOI_LUT_BOX_CLASS::~VOI_LUT_BOX_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (datasetM_ptr)
	{
		delete datasetM_ptr;
	}
}


//>>===========================================================================

BASIC_FILM_BOX_CLASS::BASIC_FILM_BOX_CLASS(EMULATOR_SESSION_CLASS *session_ptr, LOG_CLASS *logger_ptr, int filmNumber, string sopInstanceUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sessionM_ptr = session_ptr;
	filmNumberM = filmNumber;
	imageNumberM = 1;
	sopClassUidM = FILM_BOX_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = NULL;
	loggerM_ptr = logger_ptr;
}

//>>===========================================================================

BASIC_FILM_BOX_CLASS::BASIC_FILM_BOX_CLASS(EMULATOR_SESSION_CLASS *session_ptr, LOG_CLASS *logger_ptr, int filmNumber, string sopInstanceUid, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sessionM_ptr = session_ptr;
	filmNumberM = filmNumber;
	imageNumberM = 1;
	sopClassUidM = FILM_BOX_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = dataset_ptr;
	loggerM_ptr = logger_ptr;
}

//>>===========================================================================

BASIC_FILM_BOX_CLASS::~BASIC_FILM_BOX_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (datasetM_ptr) 
	{
		delete datasetM_ptr;
	}

	while (imageBoxM.getSize())
	{
		delete imageBoxM[0];
		imageBoxM.removeAt(0);
	}

	while (annotationBoxM.getSize()) 
	{
		delete annotationBoxM[0];
		annotationBoxM.removeAt(0);
	}
}

//>>===========================================================================

int BASIC_FILM_BOX_CLASS::isImageBox(string sopInstanceUid)

//  DESCRIPTION     : Check if instance uid is an Image Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < imageBoxM.getSize(); i++) 
	{
		if (sopInstanceUid == imageBoxM[i]->getSopInstanceUid()) 
		{
			return i;
		}
	}

	return -1;
}
		
//>>===========================================================================

bool BASIC_FILM_BOX_CLASS::removeImageBox(UINT index)

//  DESCRIPTION     : Remove indexed Image Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < imageBoxM.getSize()) 
	{
		delete imageBoxM[index];
		imageBoxM.removeAt(index);
		return true;
	}

	return false;
}

//>>===========================================================================

IMAGE_BOX_CLASS *BASIC_FILM_BOX_CLASS::getImageBox(UINT index)

//  DESCRIPTION     : Get indexed Image Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < imageBoxM.getSize()) 
	{
		return imageBoxM[index];
	}

	return NULL;
}

//>>===========================================================================

int BASIC_FILM_BOX_CLASS::isAnnotationBox(string sopInstanceUid)

//  DESCRIPTION     : Check if instance uid an Annotation Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < annotationBoxM.getSize(); i++) 
	{
		if (sopInstanceUid == annotationBoxM[i]->getSopInstanceUid()) 
		{
			return i;
		}
	}

	return -1;
}

//>>===========================================================================

bool BASIC_FILM_BOX_CLASS::removeAnnotationBox(UINT index)

//  DESCRIPTION     : Remove indexed Annotation Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < annotationBoxM.getSize()) 
	{
		delete annotationBoxM[index];
		annotationBoxM.removeAt(index);
		return true;
	}

	return false;
}

//>>===========================================================================

ANNOTATION_BOX_CLASS *BASIC_FILM_BOX_CLASS::getAnnotationBox(UINT index)

//  DESCRIPTION     : Get indexed Annotation Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < annotationBoxM.getSize()) 
	{
		return annotationBoxM[index];
	}

	return NULL;
}

//>>===========================================================================

void BASIC_FILM_BOX_CLASS::makeImageSopInstanceUid()

//  DESCRIPTION     : Generate an Image Box sop instance uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char buffer[UI_LENGTH + 1];

	createUID(buffer, (char*) sessionM_ptr->getImplementationClassUid());
	imageSopInstanceUidM = buffer;
}

//>>===========================================================================

UINT16 BASIC_FILM_BOX_CLASS::create(char *imageBoxSopClassUid_ptr, DCM_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Film Box creation activities.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_DATASET_CLASS *dataset_ptr = NULL;

	*dataset_ptr_ptr = NULL;

	if (datasetM_ptr == NULL) return DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;

	BYTE *imageDisplayFormat_ptr = new BYTE [ST_LENGTH + 1];

	// get the Image log Format
	if (!datasetM_ptr->getSTValue(TAG_IMAGE_DISPLAY_FORMAT, imageDisplayFormat_ptr, ST_LENGTH))
	{
		return DCM_STATUS_NO_SUCH_ATTRIBUTE;
	}

	// strip off any trailing space before the lookup
	UINT length = byteStrLen(imageDisplayFormat_ptr);
	while ((length > 0) &&
		(imageDisplayFormat_ptr[length - 1] == SPACECHAR))
	{
		// remove the trailing space
		imageDisplayFormat_ptr[length - 1] = NULLCHAR;
		length--;
	}

	// try and get number of images from definition
	int noImages = MYPRINTER->getImageDisplayFormat((char*) imageDisplayFormat_ptr);
	if (noImages == 0) 
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_WARNING, 1, "Image Display Format (2010,0010): \"%s\" unknown to Print SCP. Don't know how many image boxes to create - add \"%s\" definition to ImageDisplayFormat.dat file in bin directory", imageDisplayFormat_ptr, imageDisplayFormat_ptr);
		}
	}

	delete imageDisplayFormat_ptr;

	if (noImages == 0) 
	{
		return DCM_STATUS_INVALID_ATTRIBUTE_VALUE;
	}

	// generate a SQ value
	DCM_VALUE_SQ_CLASS *sqValue_ptr = NULL;
	DCM_VALUE_SQ_CLASS *lSqValue_ptr = NULL;

	// now set up image boxes for the number of images defined
	for (int i = 0; i < noImages; i++) 
	{
		// make sure that we have a dataset and referenced sequence
		if (i == 0) 
		{
			dataset_ptr = new DCM_DATASET_CLASS();
			
			// set up the command id and iod name fields
			dataset_ptr->setCommandId(DIMSE_CMD_NCREATE_RSP);
			dataset_ptr->setIodName("Basic Film Box");

			DCM_ATTRIBUTE_CLASS *refSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_IMAGE_BOX_SEQUENCE, ATTR_VR_SQ);
			dataset_ptr->addAttribute(refSq_ptr);
			sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
			refSq_ptr->AddValue(sqValue_ptr);

			// make local copy too
			DCM_ATTRIBUTE_CLASS *lRefSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_IMAGE_BOX_SEQUENCE, ATTR_VR_SQ);
			datasetM_ptr->addAttribute(lRefSq_ptr);
			lSqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
			lRefSq_ptr->AddValue(lSqValue_ptr);
		}

		makeImageSopInstanceUid();
		IMAGE_BOX_CLASS *imageBox_ptr = new IMAGE_BOX_CLASS(imageBoxSopClassUid_ptr, imageSopInstanceUidM);

		// generate an item
		DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();
		DCM_ITEM_CLASS *lItem_ptr = new DCM_ITEM_CLASS();

		// add SOP Class UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, imageBox_ptr->getSopClassUid());

		// make local copy of SOP Class UID
		lItem_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, imageBox_ptr->getSopClassUid());

		// add SOP Instance UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, imageBox_ptr->getSopInstanceUid());

		// make local copy of SOP Instance UID
		lItem_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, imageBox_ptr->getSopInstanceUid());

		// add the item to the dataset
		sqValue_ptr->Set(item_ptr);

		// add the local item to the local dataset
		lSqValue_ptr->Set(lItem_ptr);

		// store local image box copy
		addImageBox(imageBox_ptr);
	}

	BYTE *annotationDisplayFormatId_ptr = new BYTE [CS_LENGTH + 1];

	// now check if an annotation has been supplied
	if (datasetM_ptr->getCSValue(TAG_ANNOTATION_DISPLAY_FORMAT_ID, annotationDisplayFormatId_ptr, CS_LENGTH))
	{
		// strip off any trailing space before the lookup
		UINT length = byteStrLen(annotationDisplayFormatId_ptr);
		while ((length > 0) &&
			(annotationDisplayFormatId_ptr[length - 1] == SPACECHAR))
		{
			// remove the trailing space
			annotationDisplayFormatId_ptr[length - 1] = NULLCHAR;
			length--;
		}

		// try and get number of images from definition
		int noBoxes = MYPRINTER->getAnnotationDisplayFormatId((char*) annotationDisplayFormatId_ptr);

		if (noBoxes == 0)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_WARNING, 1, "Annotation Display Format ID (2010,0030): \"%s\" unknown to Print SCP. Don't know how many annotation boxes to create - add \"%s\" definition to ImageDisplayFormat.dat file in bin directory", annotationDisplayFormatId_ptr, annotationDisplayFormatId_ptr);
			}
		}

		for (int i = 0; i < noBoxes; i++) 
		{
			// make sure that we have a dataset and referenced sequence
			if (i == 0) 
			{
				if (dataset_ptr == NULL) 
				{
					// should never be the case as there should always be an Image Display
					// Format defined
					dataset_ptr = new DCM_DATASET_CLASS();
				}

				DCM_ATTRIBUTE_CLASS *refSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_BASIC_ANNOTATION_BOX_SEQUENCE, ATTR_VR_SQ);
				dataset_ptr->addAttribute(refSq_ptr);
				sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
				refSq_ptr->AddValue(sqValue_ptr);

				// make local copy too
				DCM_ATTRIBUTE_CLASS *lRefSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_BASIC_ANNOTATION_BOX_SEQUENCE, ATTR_VR_SQ);
				datasetM_ptr->addAttribute(lRefSq_ptr);
				lSqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
				lRefSq_ptr->AddValue(lSqValue_ptr);
			}

			makeImageSopInstanceUid();
			ANNOTATION_BOX_CLASS *annotationBox_ptr = new ANNOTATION_BOX_CLASS(imageSopInstanceUidM);

			// generate an item
			DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();
			DCM_ITEM_CLASS *lItem_ptr = new DCM_ITEM_CLASS();

			// add SOP Class UID
			item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, annotationBox_ptr->getSopClassUid());

			// make local copy of SOP Class UID
			lItem_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, annotationBox_ptr->getSopClassUid());

			// add SOP Instance UID
			item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, annotationBox_ptr->getSopInstanceUid());

			// make local copy of SOP Instance UID
			lItem_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, annotationBox_ptr->getSopInstanceUid());

			// add the item to the dataset
			sqValue_ptr->Set(item_ptr);

			// add the local item to the local dataset
			lSqValue_ptr->Set(lItem_ptr);

			addAnnotationBox(annotationBox_ptr);
		}
	}

	delete annotationDisplayFormatId_ptr;

	// sort the local attributes - we may have added Referenced Image Box and Annotation Box Sequences
	datasetM_ptr->SortAttributes();

	*dataset_ptr_ptr = dataset_ptr;

	// return success
	return DCM_STATUS_SUCCESS;
}

//>>===========================================================================

UINT16 BASIC_FILM_BOX_CLASS::action(BASIC_FILM_SESSION_CLASS*, int,  bool queueJob, DCM_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Action the Film Box - print it.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	*dataset_ptr_ptr = NULL;

	bool emptyPage = true;

	for (int i = 0; i < noImageBoxes(); i++)
	{
		IMAGE_BOX_CLASS *imageBox_ptr = getImageBox(i);
		if (imageBox_ptr->get())
		{
			// check for instantiated Image Box
			emptyPage = false;
		}
	}

	if (emptyPage) 
	{
		return DCM_STATUS_PRINT_FILM_BOX_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES;
	}

	if (queueJob) 
	{

		PRINT_JOB_CLASS	*printJob_ptr = new PRINT_JOB_CLASS(sessionM_ptr, loggerM_ptr);

		// finally store the Print Job
		MYPRINTQUEUE->addPrintJob(printJob_ptr);

		DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

		// identify the dataset
		dataset_ptr->setIodName(PRINT_JOB_IOD_NAME);
		dataset_ptr->setCommandId(DIMSE_CMD_NACTION_RSP);

		DCM_ATTRIBUTE_CLASS *refSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_PRINT_JOB_SEQUENCE, ATTR_VR_SQ);
		dataset_ptr->addAttribute(refSq_ptr);

		// generate an item
		DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

		// add SOP Class UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, printJob_ptr->getSopClassUid());

		// add SOP Instance UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, printJob_ptr->getSopInstanceUid());

		// add the item to the dataset
		DCM_VALUE_SQ_CLASS	*sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
		sqValue_ptr->Set(item_ptr);
		refSq_ptr->AddValue(sqValue_ptr);

		*dataset_ptr_ptr = dataset_ptr;
	}

	// return success
	return DCM_STATUS_SUCCESS;
}


//>>===========================================================================

BASIC_FILM_SESSION_CLASS::BASIC_FILM_SESSION_CLASS(EMULATOR_SESSION_CLASS *session_ptr, LOG_CLASS *logger_ptr, string sopInstanceUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// contructor activities
	sessionM_ptr = session_ptr;
	sopClassUidM = FILM_SESSION_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = NULL;
	loggerM_ptr = logger_ptr;
}

//>>===========================================================================

BASIC_FILM_SESSION_CLASS::BASIC_FILM_SESSION_CLASS(EMULATOR_SESSION_CLASS *session_ptr, LOG_CLASS *logger_ptr, string sopInstanceUid, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	sessionM_ptr = session_ptr;
	sopClassUidM = FILM_SESSION_SOP_CLASS_UID;
	sopInstanceUidM = sopInstanceUid;
	datasetM_ptr = dataset_ptr;
	loggerM_ptr = logger_ptr;
}

//>>===========================================================================

BASIC_FILM_SESSION_CLASS::~BASIC_FILM_SESSION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (datasetM_ptr) 
	{
		delete datasetM_ptr;
	}

	while (filmBoxM.getSize()) 
	{
		delete filmBoxM[0];
		filmBoxM.removeAt(0);
	}

	while (overlayM.getSize()) 
	{
		delete overlayM[0];
		overlayM.removeAt(0);
	}

	while (voiLutM.getSize())
	{
		delete voiLutM[0];
		voiLutM.removeAt(0);
	}
}

//>>===========================================================================

int BASIC_FILM_SESSION_CLASS::isFilmBox(string sopInstanceUid)

//  DESCRIPTION     : Check if instance uid is a Film Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < filmBoxM.getSize(); i++) 
	{
		if (sopInstanceUid == filmBoxM[i]->getSopInstanceUid())
		{
			return i;
		}
	}

	return -1;
}

//>>===========================================================================

bool BASIC_FILM_SESSION_CLASS::removeFilmBox(UINT index)

//  DESCRIPTION     : Remove indexed Film Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < filmBoxM.getSize()) 
	{
		delete filmBoxM[index];
		filmBoxM.removeAt(index);
		return true;
	}

	return false;
}

//>>===========================================================================

BASIC_FILM_BOX_CLASS *BASIC_FILM_SESSION_CLASS::getFilmBox(UINT index)

//  DESCRIPTION     : Get indexed Film Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < filmBoxM.getSize()) 
	{
		return filmBoxM[index];
	}

	return NULL;
}

//>>===========================================================================

int BASIC_FILM_SESSION_CLASS::isImageOverlay(string sopInstanceUid)

//  DESCRIPTION     : Check if instance uid is an Image Overlay.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < overlayM.getSize(); i++) 
	{
		if (sopInstanceUid == overlayM[i]->getSopInstanceUid())
		{
			return i;
		}
	}

	return -1;
}

//>>===========================================================================

bool BASIC_FILM_SESSION_CLASS::removeImageOverlay(UINT index)

//  DESCRIPTION     : Remove indexed Image Overlay.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < overlayM.getSize()) 
	{
		delete overlayM[index];
		overlayM.removeAt(index);
		return true;
	}

	return false;
}

//>>===========================================================================

IMAGE_OVERLAY_CLASS *BASIC_FILM_SESSION_CLASS::getImageOverlay(UINT index)

//  DESCRIPTION     : Get indexed Image Overlay.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < overlayM.getSize()) 
	{
		return overlayM[index];
	}

	return NULL;
}

//>>===========================================================================

int BASIC_FILM_SESSION_CLASS::isVoiLutBox(string sopInstanceUid)

//  DESCRIPTION     : Check if instance uid is a VOI LUT Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < voiLutM.getSize(); i++) 
	{
		if (sopInstanceUid == voiLutM[i]->getSopInstanceUid())
		{
			return i;
		}
	}

	return -1;
}

//>>===========================================================================

bool BASIC_FILM_SESSION_CLASS::removeVoiLutBox(UINT index)

//  DESCRIPTION     : Remove indexed VOI LUT Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < voiLutM.getSize()) 
	{
		delete voiLutM[index];
		voiLutM.removeAt(index);
		return true;
	}

	return false;
}

//>>===========================================================================

VOI_LUT_BOX_CLASS *BASIC_FILM_SESSION_CLASS::getVoiLutBox(UINT index)

//  DESCRIPTION     : Get indexed VOI LUT Box.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < voiLutM.getSize()) 
	{
		return voiLutM[index];
	}

	return NULL;
}

//>>===========================================================================

UINT16 BASIC_FILM_SESSION_CLASS::action(bool queueJob, DCM_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Action the Film Session - print it.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	*dataset_ptr_ptr = NULL;

	if (noFilmBoxes() == 0)
	{
		return DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_FILM_BOX_SOP_INSTANCES;
	}

	bool emptyPage = false;

	for (int i = 0; i < noFilmBoxes(); i++)
	{
		BASIC_FILM_BOX_CLASS *filmBox_ptr = getFilmBox(i);

		int noImageBoxes = 0;
		for (int j = 0; j < filmBox_ptr->noImageBoxes(); j++) 
		{
			IMAGE_BOX_CLASS *imageBox_ptr = filmBox_ptr->getImageBox(j);
			if (imageBox_ptr->get())
			{
				// check for instantiated Image Box
				noImageBoxes++;
			}
		}
		if (noImageBoxes == 0) 
		{
			emptyPage = true;
		}
	}
	if (emptyPage) 
	{
		return DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES;
	}

	if (queueJob) 
	{

		PRINT_JOB_CLASS	*printJob_ptr = new PRINT_JOB_CLASS(sessionM_ptr, loggerM_ptr);

		// finally store the Print Job
		MYPRINTQUEUE->addPrintJob(printJob_ptr);

		// allocate dataset for the print job
		DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

		// identify the dataset
		dataset_ptr->setIodName(PRINT_JOB_IOD_NAME);
		dataset_ptr->setCommandId(DIMSE_CMD_NACTION_RSP);

		DCM_ATTRIBUTE_CLASS *refSq_ptr = new DCM_ATTRIBUTE_CLASS(TAG_REFERENCED_PRINT_JOB_SEQUENCE, ATTR_VR_SQ);
		dataset_ptr->addAttribute(refSq_ptr);

		// generate an item
		DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();

		// add SOP Class UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_CLASS_UID, printJob_ptr->getSopClassUid());

		// add SOP Instance UID
		item_ptr->setUIValue(TAG_REFERENCED_SOP_INSTANCE_UID, printJob_ptr->getSopInstanceUid());

		// add the item to the dataset
		DCM_VALUE_SQ_CLASS	*sqValue_ptr = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);
		sqValue_ptr->Set(item_ptr);
		refSq_ptr->AddValue(sqValue_ptr);

		*dataset_ptr_ptr = dataset_ptr;
	}

	// return success
	return DCM_STATUS_SUCCESS;
}


//>>===========================================================================

PRINT_JOB_CLASS::PRINT_JOB_CLASS(EMULATOR_SESSION_CLASS *session_ptr, LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char buffer[32];

	// constructor activities
	sessionM_ptr = session_ptr;
	sopClassUidM = PRINT_JOB_SOP_CLASS_UID;
	makeSopInstanceUid();
	loggerM_ptr = logger_ptr;

	// set up the Print Job attributes
	datasetM_ptr = new DCM_DATASET_CLASS();

	// Execution Status
	datasetM_ptr->setCSValue(TAG_EXECUTION_STATUS, "DONE");

	// Creation Date - TODAY
	strcpy(buffer, "TODAY");
	mapDate(buffer);
	datasetM_ptr->setDAValue(TAG_CREATION_DATE, buffer);

	// Creation Time - NOW
	strcpy(buffer, "NOW");
	mapTime(buffer);
	datasetM_ptr->setTMValue(TAG_CREATION_TIME, buffer);

	// Printer Name
	datasetM_ptr->setLOValue(TAG_PRINTER_NAME, MYPRINTER->getName());
}

//>>===========================================================================

PRINT_JOB_CLASS::~PRINT_JOB_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (datasetM_ptr) 
	{
		delete datasetM_ptr;
	}
}

//>>===========================================================================

void PRINT_JOB_CLASS::makeSopInstanceUid()

//  DESCRIPTION     : Make a unique Print Job instance uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char	buffer[UI_LENGTH + 1];
	sprintf(buffer, "%s.%d.%d.%d", sessionM_ptr->getImplementationClassUid(), (int) N_ACTION_RQ, 123456, MYPRINTQUEUE->nextPrintJobNo());
	sopInstanceUidM = buffer;
}

//>>===========================================================================
//
// initialise PRINT_QUEUE_CLASS static pointer
//<<===========================================================================
PRINT_QUEUE_CLASS *PRINT_QUEUE_CLASS::instanceM_ptr = NULL;

//>>===========================================================================

void PRINT_QUEUE_CLASS::initialise()

//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	printJobNumberM = 1;
}

//>>===========================================================================

PRINT_QUEUE_CLASS::PRINT_QUEUE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	initialise();
}

//>>===========================================================================

PRINT_QUEUE_CLASS *PRINT_QUEUE_CLASS::instance()

//  DESCRIPTION     : Get Print Queue instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) 
	{
		instanceM_ptr = new PRINT_QUEUE_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

void PRINT_QUEUE_CLASS::cleanup()

//  DESCRIPTION     : Cleanup instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	while (printJobM.getSize()) 
	{
		delete printJobM[0];
		printJobM.removeAt(0);
	}

	initialise();
}

//>>===========================================================================

int PRINT_QUEUE_CLASS::isPrintJob(string sopInstanceUid)

//  DESCRIPTION     : Check if instance uid is a Print Job.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < printJobM.getSize(); i++) 
	{
		if (sopInstanceUid == printJobM[i]->getSopInstanceUid())
		{
			return i;
		}
	}

	return -1;
}

//>>===========================================================================

bool PRINT_QUEUE_CLASS::removePrintJob(UINT index)

//  DESCRIPTION     : Remove indexed Print Job.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < printJobM.getSize()) 
	{
		delete printJobM[index];
		printJobM.removeAt(index);
		return true;
	}

	return false;
}

//>>===========================================================================

PRINT_JOB_CLASS *PRINT_QUEUE_CLASS::getPrintJob(UINT index)

//  DESCRIPTION     : Get indexed Print Job.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < printJobM.getSize()) 
	{
		return printJobM[index];
	}

	return NULL;
}

//>>===========================================================================

UINT16 PRINT_QUEUE_CLASS::get(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Get Print Queue.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string	sopInstanceUid;

	*dataset_ptr_ptr = NULL;

	// check that we know the Print Job
	if (!command_ptr->getUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUid))
	{
		// Missing Attribute
		return DCM_STATUS_MISSING_ATTRIBUTE;
	}

	int i = isPrintJob(sopInstanceUid);
	if (i == -1)
	{
		// No Such Object Instance
		return DCM_STATUS_NO_SUCH_OBJECT_INSTANCE;
	}

	PRINT_JOB_CLASS *printJob_ptr = getPrintJob(i);
	if (printJob_ptr == NULL)
	{
		// Processing Failure
		return DCM_STATUS_PROCESSING_FAILURE;
	}

	DCM_DATASET_CLASS *printJobDataset_ptr = printJob_ptr->get();
	if (printJobDataset_ptr == NULL) 
	{
		// Processing Failure
		return DCM_STATUS_PROCESSING_FAILURE;
	}

	DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

	// set up the command id and iod name fields
	dataset_ptr->setCommandId(DIMSE_CMD_NGET_RSP);
	dataset_ptr->setIodName("Print Job");

	*dataset_ptr_ptr = dataset_ptr;
		
	DCM_ATTRIBUTE_CLASS *attribute_ptr;
	UINT index = 0;
	UINT32 tag;

	// check if all attributes should be returned
	if (command_ptr->GetAttributeByTag(TAG_ATTRIBUTE_IDENTIFIER_LIST) == NULL) 
	{
		// simulate a zero-length
		attribute_ptr = new DCM_ATTRIBUTE_CLASS(TAG_ATTRIBUTE_IDENTIFIER_LIST, ATTR_VR_AT);
		command_ptr->addAttribute(attribute_ptr);
	}

	// check that we have not been given a zero-length
	attribute_ptr = command_ptr->GetAttributeByTag(TAG_ATTRIBUTE_IDENTIFIER_LIST); 
	if (attribute_ptr->GetNrValues() == 0)
	{
		// if so return all attributes
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_EXECUTION_STATUS);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_CREATION_DATE);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_CREATION_TIME);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_PRINTER_NAME);
	}

	// generate return values
	while (command_ptr->getATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, index++, &tag)) 
	{
		switch (tag) 
		{
		case TAG_EXECUTION_STATUS: // Execution Status
			{
				char executionStatus[CS_LENGTH + 1];
				if (printJobDataset_ptr->getCSValue(TAG_EXECUTION_STATUS, (BYTE*) executionStatus, CS_LENGTH)) 
				{
					dataset_ptr->setCSValue(TAG_EXECUTION_STATUS, executionStatus);
				}
			}
			break;
		case TAG_CREATION_DATE: // Creation Date
			{
				char creationDate[DA_LENGTH + 1];
				if (printJobDataset_ptr->getDAValue(TAG_CREATION_DATE, (BYTE*) creationDate, DA_LENGTH))
				{
					dataset_ptr->setDAValue(TAG_CREATION_DATE, creationDate);
				}
			}
			break;
		case TAG_CREATION_TIME: // Creation Time
			{
				char creationTime[TM_LENGTH + 1];
				if (printJobDataset_ptr->getTMValue(TAG_CREATION_TIME, (BYTE*) creationTime, TM_LENGTH))
				{
					dataset_ptr->setTMValue(TAG_CREATION_TIME, creationTime);
				}
			}
			break;
		case TAG_PRINTER_NAME: // Printer Name
			{
				dataset_ptr->setLOValue(TAG_PRINTER_NAME, MYPRINTER->getName());
			}
			break;
		default:
			break;
		}
	}

	// return success
	return DCM_STATUS_SUCCESS;
}


//>>===========================================================================

FORMAT_DESC_CLASS::FORMAT_DESC_CLASS(char *format_ptr, int noBoxes)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	formatM_ptr = new char [ strlen(format_ptr) + 1 ];
	strcpy(formatM_ptr, format_ptr);

	noBoxesM = noBoxes;
}

//>>===========================================================================

FORMAT_DESC_CLASS::~FORMAT_DESC_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (formatM_ptr) 
	{
		delete formatM_ptr;
	}
}


//<<===========================================================================
//
// initialise PRINTER_CLASS static pointer
//>>===========================================================================
PRINTER_CLASS *PRINTER_CLASS::instanceM_ptr = NULL;

//>>===========================================================================

void PRINTER_CLASS::initialise()

//  DESCRIPTION     : Initialise instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise instance
    imageDisplayFormatDataLoadedM = false;
	strcpy(statusM, "NORMAL");
	strcpy(statusInfoM, "");

	while (imageDisplayFormatM.getSize())
	{
		delete imageDisplayFormatM[0];
		imageDisplayFormatM.removeAt(0);
	}

	while (annotationDisplayFormatIdM.getSize())
	{
		delete annotationDisplayFormatIdM[0];
		annotationDisplayFormatIdM.removeAt(0);
	}

	removeAllPresentationLuts();
}

//>>===========================================================================

PRINTER_CLASS::PRINTER_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	initialise();
}

//>>===========================================================================

PRINTER_CLASS *PRINTER_CLASS::instance()

//  DESCRIPTION     : Get Printer instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) 
	{
		instanceM_ptr = new PRINTER_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

void PRINTER_CLASS::cleanup()

//  DESCRIPTION     : Cleanup Printer instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// cleanup instance
	initialise();
}

//>>===========================================================================

UINT16 PRINTER_CLASS::get(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Get Printer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

	// set up the command id and iod name fields
	dataset_ptr->setCommandId(DIMSE_CMD_NGET_RSP);
	dataset_ptr->setIodName("Printer");

	*dataset_ptr_ptr = dataset_ptr;
		
	DCM_ATTRIBUTE_CLASS *attribute_ptr;
	UINT index = 0;
	UINT32 tag;

	// check if all attributes should be returned
	if (command_ptr->GetAttributeByTag(TAG_ATTRIBUTE_IDENTIFIER_LIST) == NULL) 
	{
		// simulate a zero-length
		attribute_ptr = new DCM_ATTRIBUTE_CLASS(TAG_ATTRIBUTE_IDENTIFIER_LIST, ATTR_VR_AT);
		command_ptr->addAttribute(attribute_ptr);
	}

	// check that we have not been given a zero-length
	attribute_ptr = command_ptr->GetAttributeByTag(TAG_ATTRIBUTE_IDENTIFIER_LIST);
	if (attribute_ptr->GetNrValues() == 0) 
	{
		// if so return all attributes
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_PRINTER_STATUS);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_PRINTER_STATUS_INFO);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_PRINTER_NAME);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_MANUFACTURER);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_MANUFACTURERS_MODEL_NAME);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_DEVICE_SERIAL_NUMBER);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_SOFTWARE_VERSIONS);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_DATE_OF_LAST_CALIBRATION);
		command_ptr->addATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, TAG_TIME_OF_LAST_CALIBRATION);
	}

	// generate return values
	while (command_ptr->getATValue(TAG_ATTRIBUTE_IDENTIFIER_LIST, index++, &tag))
	{
		switch (tag)
		{
		case TAG_PRINTER_STATUS: // Printer Status
			{
				dataset_ptr->setCSValue(TAG_PRINTER_STATUS, getStatus());
			}
			break;
		case TAG_PRINTER_STATUS_INFO: // Printer Status Info
			{
				if (strcmp(getStatus(), "NORMAL") != 0) 
				{
					dataset_ptr->setCSValue(TAG_PRINTER_STATUS_INFO, getStatusInfo());
				}
			}
			break;
		case TAG_PRINTER_NAME: // Printer Name
			{
				dataset_ptr->setLOValue(TAG_PRINTER_NAME, getName());
			}
			break;
		case TAG_MANUFACTURER: // Manufacturer
			{
				dataset_ptr->setLOValue(TAG_MANUFACTURER, getManufacturer());
			}
			break;
		case TAG_MANUFACTURERS_MODEL_NAME: // Manufacturer Model Name
			{
				dataset_ptr->setLOValue(TAG_MANUFACTURERS_MODEL_NAME, getModelName());
			}
			break;
		case TAG_DEVICE_SERIAL_NUMBER: // Device Serial Number
			{
				dataset_ptr->setLOValue(TAG_DEVICE_SERIAL_NUMBER, getSerialNumber());
			}
			break;
		case TAG_SOFTWARE_VERSIONS: // Software Versions
			{
				dataset_ptr->setLOValue(TAG_SOFTWARE_VERSIONS, getSoftwareVersion());
			}
			break;
		case TAG_DATE_OF_LAST_CALIBRATION: // Date Last Calibration
			{
				dataset_ptr->setDAValue(TAG_DATE_OF_LAST_CALIBRATION, getCalibrationDate());
			}
			break;
		case TAG_TIME_OF_LAST_CALIBRATION: // Time Last Calibration
			{
				dataset_ptr->setTMValue(TAG_TIME_OF_LAST_CALIBRATION, getCalibrationTime());
			}
			break;
		default:
			break;
		}
	}

	// return success
	return DCM_STATUS_SUCCESS;
}

//>>===========================================================================

void PRINTER_CLASS::setStatus(char *status)

//  DESCRIPTION     : Set status.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	strncpy(statusM, status, CS_LENGTH);
	statusM[CS_LENGTH] = '\0';
}

//>>===========================================================================

void PRINTER_CLASS::setStatusInfo(char *statusInfo)

//  DESCRIPTION     : Set status info.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	strncpy(statusInfoM, statusInfo, CS_LENGTH);
	statusInfoM[CS_LENGTH] = '\0';
}

//>>===========================================================================

UINT PRINTER_CLASS::getNoStatusInfoDTs()

//  DESCRIPTION     : Get the number of Status Info defined terms.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// return the number of printer status defined terms
	return DEFINITION->GetNrDefinedTerms(DIMSE_CMD_NGET_RSP, "Printer", TAG_PRINTER_STATUS_INFO);
}

//>>===========================================================================

string PRINTER_CLASS::getStatusInfoDT(UINT index)

//  DESCRIPTION     : Get the indexed Status Info defined term.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string statusInfoDT = "";

	// try to get the indexed base value
	BASE_VALUE_CLASS *value_ptr = DEFINITION->GetDefinedTerm(DIMSE_CMD_NGET_RSP, "Printer", TAG_PRINTER_STATUS_INFO, index);

	// check a value has been returned
	if (value_ptr)
	{
		// get the value
		value_ptr->Get(statusInfoDT);
	}

	// return value
	return statusInfoDT;
}

//>>===========================================================================

bool PRINTER_CLASS::loadImageDisplayFormats(string filename)

//  DESCRIPTION     : Load the Image DisplayFormats from file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // check if the image display format data has already been loaded
    if (!imageDisplayFormatDataLoadedM)
    {
       	IMAGE_DISPLAY_FILE_CLASS image_display_file(filename);

        // parse the image display format file
	    imageDisplayFormatDataLoadedM = image_display_file.execute();
    }

    return imageDisplayFormatDataLoadedM;
}

//>>===========================================================================

void PRINTER_CLASS::addImageDisplayFormat(char *idf_ptr, int noBoxes)

//  DESCRIPTION     : Add an entry to the Image Display Format list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	FORMAT_DESC_CLASS *formatDesc_ptr = new FORMAT_DESC_CLASS(idf_ptr, noBoxes);
	imageDisplayFormatM.add(formatDesc_ptr);
}
		
//>>===========================================================================

void PRINTER_CLASS::addAnnotationDisplayFormatId(char *adfi_ptr, int noBoxes)

//  DESCRIPTION     : Add an entry to the Annotation Display Format Id list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	FORMAT_DESC_CLASS *formatDesc_ptr = new FORMAT_DESC_CLASS(adfi_ptr, noBoxes);
	annotationDisplayFormatIdM.add(formatDesc_ptr);
}

//>>===========================================================================

int PRINTER_CLASS::addNumbers(char *numbers_ptr)

//  DESCRIPTION     : Add the numbers in the given list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *chr_ptr = numbers_ptr;
	int num = 0;
	int total = 0;
	while (*chr_ptr != '\0')
	{
		if (isdigit(*chr_ptr)) 
		{
			num = (num * 10) + *chr_ptr - '0';
		}
		else if (*chr_ptr == ',') 
		{
			total += num;
			num = 0;
		}
		else 
		{
			return 0;
		}
		chr_ptr++;
	}

	total += num;

	// return the total
	return total;
}

//>>===========================================================================

int PRINTER_CLASS::multiplyNumbers(char *numbers_ptr)

//  DESCRIPTION     : Multiple the numbers in the given list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *chr_ptr = numbers_ptr;
	int col = 0;
	int row = 0;
	bool gotCol = false;

	while (*chr_ptr != '\0') 
	{
		if (isdigit(*chr_ptr))
		{
			if (!gotCol) 
			{
				col = (col * 10) + *chr_ptr - '0';
			}
			else 
			{
				row = (row * 10) + *chr_ptr - '0';
			}
		}
		else if (*chr_ptr == ',') 
		{
			if (gotCol) 
			{
				return 0;
			}

			gotCol = true;
		}
		else 
		{
			return 0;
		}
		chr_ptr++;
	}

	// return rows * cols
	return row * col;
}

//>>===========================================================================

int PRINTER_CLASS::getNoImages(char *imageDisplayFormat_ptr)

//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int noImages = 0;

	if (strncmp(imageDisplayFormat_ptr, "ROW\\", 4) == 0)
	{
		if (strlen(&imageDisplayFormat_ptr[4]) != 0)
		{
			noImages = addNumbers(&imageDisplayFormat_ptr[4]);
		}
	}
	else if (strncmp(imageDisplayFormat_ptr, "COL\\", 4) == 0)
	{
		if (strlen(&imageDisplayFormat_ptr[4]) != 0)
		{
			noImages = addNumbers(&imageDisplayFormat_ptr[4]);
		}
	}
	else if (strncmp(imageDisplayFormat_ptr, "STANDARD\\", 9) == 0)
	{
		if (strlen(&imageDisplayFormat_ptr[9]) >= 3)
		{
			noImages = multiplyNumbers(&imageDisplayFormat_ptr[9]);
		}
	}

	return noImages;
}
		
//>>===========================================================================

int PRINTER_CLASS::getImageDisplayFormat(char *idf_ptr)

//  DESCRIPTION     : Determine number of image boxes defined by the Image
//					: Display Format.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for custom layouts first
	for (UINT i = 0; i < imageDisplayFormatM.getSize(); i++)
	{
		FORMAT_DESC_CLASS *formatDesc_ptr = imageDisplayFormatM[i];

		if (formatDesc_ptr) 
		{
			if (strcmp(formatDesc_ptr->getFormat(), idf_ptr) == 0)
			{
				return formatDesc_ptr->getNoBoxes();
			}
		}
	}

	// try to compute number from IDF string
	return getNoImages(idf_ptr);
}

//>>===========================================================================

int	PRINTER_CLASS::getAnnotationDisplayFormatId(char *adfi_ptr)

//  DESCRIPTION     : Determine number of annotation boxes defined by the 
//					: Annotation Display Format Id.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < annotationDisplayFormatIdM.getSize(); i++)
	{
		FORMAT_DESC_CLASS *formatDesc_ptr = annotationDisplayFormatIdM[i];
		
		if (formatDesc_ptr)
		{
			if (strcmp(formatDesc_ptr->getFormat(), adfi_ptr) == 0)
			{
				return formatDesc_ptr->getNoBoxes();
			}
		}
	}

	// no match found - return zero
	return 0;
}

//>>===========================================================================

int PRINTER_CLASS::isPresentationLut(string sopInstanceUid)

//  DESCRIPTION     : Check if instance uid is a Presentation Lut.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	for (UINT i = 0; i < presentationLutM.getSize(); i++)
	{
		if (sopInstanceUid == presentationLutM[i]->getSopInstanceUid())
		{
			return i;
		}
	}

	return -1;
}

//>>===========================================================================

bool PRINTER_CLASS::removePresentationLut(UINT index)

//  DESCRIPTION     : Remove indexed Presentation Lut.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (index < presentationLutM.getSize()) 
	{
		delete presentationLutM[index];
		presentationLutM.removeAt(index);
		return true;
	}

	return false;
}

//>>===========================================================================

void PRINTER_CLASS::removeAllPresentationLuts()

//  DESCRIPTION     : Remove all Presentation Luts.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	while (presentationLutM.getSize()) 
	{
		delete presentationLutM[0];
		presentationLutM.removeAt(0);
	}
}
