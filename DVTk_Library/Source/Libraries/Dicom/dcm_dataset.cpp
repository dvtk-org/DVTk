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
#include "dcm_dataset.h"

#include "Idefinition.h"		// Definition component interface


//>>===========================================================================

DCM_DATASET_CLASS::DCM_DATASET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_DATASET;
	commandIdM = DIMSE_CMD_UNKNOWN;
	iodNameM ="";
    setPAH(&pahM);
}

//>>===========================================================================

DCM_DATASET_CLASS::DCM_DATASET_CLASS(DIMSE_CMD_ENUM commandId, string iodName)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_DATASET;
	commandIdM = commandId;
	iodNameM = iodName;
	setPAH(&pahM);
}

//>>===========================================================================

DCM_DATASET_CLASS::DCM_DATASET_CLASS(DIMSE_CMD_ENUM commandId, string iodName, string identifier)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	widTypeM = WID_DATASET;
	commandIdM = commandId;
	iodNameM = iodName;
	identifierM = identifier;
	setPAH(&pahM);
}

//>>===========================================================================

DCM_DATASET_CLASS::~DCM_DATASET_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

void DCM_DATASET_CLASS::removeTrailingPadding()

//  DESCRIPTION     : Flag any the Dataset Trailing Padding attribute as being
//					: deleted from the dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// try to get the Dataset Trailing Padding
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttributeByTag(TAG_DATASET_TRAILING_PADDING);

	// check if we found the Dataset Trailing Padding
	if (attribute_ptr)
	{
		// flag attribute as being deleted
		attribute_ptr->SetPresent(false);
	}
}

//>>===========================================================================

bool DCM_DATASET_CLASS::encode(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : encode DICOM dataset to dataTransfer stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if the Dataset should be completed (populated) by
	// comparison with the definition
	if (populateWithAttributesM) 
	{
		// add the type 1 & 2 attributes that are needed to satisfy the definition
		populateWithAttributes();
	}

	// check if we need to add the group length attributes
	if (defineGroupLengthsM) 
	{
		// add group length(s) - attributes sorted here
		addGroupLengths();
	}
	else
	{
		// sort the DICOM dataset attributes into ascending order
		SortAttributes();
	}

	// compute the dataset length
	computeLength(dataTransfer.getTsCode());

	// update the group lengths now that the sequence lengths should be known
	if (defineGroupLengthsM)
	{
		setGroupLengths(dataTransfer.getTsCode());
	}

	// set the transfer VR - used by the logger
	if (dataTransfer.isExplicitVR())
	{
		// set to explicit
		setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
	}
	else
	{
		// set to implicit
		setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
	}

	// compute any item offsets
	computeItemOffsets(dataTransfer);

	// encode the dataset attributes
	return DCM_ATTRIBUTE_GROUP_CLASS::encode(dataTransfer);
}

//>>===========================================================================

void DCM_DATASET_CLASS::populateWithAttributes()

//  DESCRIPTION     : Method to ensure that the dataset sent is
//					  complete - at least zero length Attributes sent for all
//					  Type 1 & Type 2.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Zero length Mandatory Attributes are a protocol
//					  violation - but at least something is sent!
//<<===========================================================================
{
	// let the Definition component do this for us
	if (DEFINITION->PopulateWithAttributes(this, loggerM_ptr))
    {
    	if (loggerM_ptr)
	    {
		    if (getIdentifier())
		    {
			    loggerM_ptr->text(LOG_INFO, 1, "Automatic Type 2 Attribute population from Definition applied to: %s %s", getIdentifier(), getIodName());
		    }
		    else
		    {
			    loggerM_ptr->text(LOG_INFO, 1, "Automatic Type 2 Attribute population from Definition applied to: %s", getIodName());
		    }
	    }
    }
    else
	{
		if (loggerM_ptr)
		{
			if (getIdentifier())
			{
				loggerM_ptr->text(LOG_INFO, 1, "Automatic Type 2 Attribute population from Definition not done for: %s %s", getIdentifier(), getIodName());
			}
			else
			{
				loggerM_ptr->text(LOG_INFO, 1, "Automatic Type 2 Attribute population from Definition not done for: %s", getIodName());
			}
		}
	}
}

//>>===========================================================================

bool DCM_DATASET_CLASS::setIdentifierByTag(UINT32 tag)

//  DESCRIPTION     : Set the dataset identifier to the value of the attribute
//					: given by the tag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check to see if an attribute with the given tag can be found in the dataset
	// - the identifier may only be a single value
	DCM_ATTRIBUTE_CLASS *attribute_ptr = GetAttributeByTag(tag);
	if ((attribute_ptr) &&
		(attribute_ptr->GetNrValues() == 1))
	{
		// switch on the attribute vr
		switch(attribute_ptr->GetVR())
		{
		case ATTR_VR_UI:
			{
				string uid;
				getUIValue(tag, uid);

				// set the uid into the identifier
				setIdentifier(uid);

				result = true;
			}
			break;
		default:
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Dataset can't be identified by attribute with tag 0x%08X - this VR not yet supported for identifiers", tag); 
			}
			break;
		}
	}

	// return result
	return result;
}

//>>===========================================================================

void DCM_DATASET_CLASS::computeItemOffsets(DATA_TF_CLASS& dataTransfer)

//  DESCRIPTION     : Compute any item offsets - for DICOMDIR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise the offset
	offsetM = dataTransfer.getLength();

	// call base method
	DCM_ATTRIBUTE_GROUP_CLASS::computeItemOffsets(dataTransfer);
}

//>>===========================================================================

UINT32 DCM_DATASET_CLASS::computeItemOffsetsForDICOMDIR(string transferSyntax)

//  DESCRIPTION     : Compute any item offsets - for DICOMDIR.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// call base method
	DCM_ATTRIBUTE_GROUP_CLASS::computeItemOffsets(transferSyntax);

	return getOffset();
}

//>>===========================================================================

bool DCM_DATASET_CLASS::decode(DATA_TF_CLASS &dataTransfer, UINT16 lastGroup)

//  DESCRIPTION     : Decode the dataset attributes.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialize the Private Attribute Handler
    initializePrivateAttributeHandler();

	// decode the dataset attributes
	bool result = DCM_ATTRIBUTE_GROUP_CLASS::decode(dataTransfer, lastGroup);

	// terminate the private attribute handler
    terminatePrivateAttributeHandler();

	// return the decode result
	return result;
}

//>>===========================================================================

void DCM_DATASET_CLASS::initializePrivateAttributeHandler()

//  DESCRIPTION     : Initialize the Private Attribute Handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // initialize the Private Attribute Handler
	pahM.setLogger(loggerM_ptr);
	pahM.install();
}

//>>===========================================================================

void DCM_DATASET_CLASS::terminatePrivateAttributeHandler()

//  DESCRIPTION     : Terminate the Private Attribute Handler.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // remove the Private Attribute Handler
    pahM.remove();
}

//>>===========================================================================

bool DCM_DATASET_CLASS::updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr)

//  DESCRIPTION     : Update this dataset with the contents of the dataset given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// ensure update WID is a dataset
	if (wid_ptr->getWidType() == widTypeM)
	{
		DCM_DATASET_CLASS *updateDataset_ptr = static_cast<DCM_DATASET_CLASS*>(wid_ptr);

		LOG_CLASS *logger_ptr = getLogger();
		if (logger_ptr == NULL)
		{
			logger_ptr = updateDataset_ptr->getLogger();
			setLogger(logger_ptr);
		}
		else
		{
			updateDataset_ptr->setLogger(logger_ptr);
		}

		// we can perform the update by merging the update dataset into this

		merge(updateDataset_ptr);

		// sort attributes after update
		SortAttributes();

		// result is OK
		result = true;
	}

	// return result
	return result;
}

//>>===========================================================================

DCM_DATASET_CLASS* DCM_DATASET_CLASS::cloneAttributes()

//  DESCRIPTION     : Clone the Dataset (but only by copying the underlying attributes).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// Clone this dataset - by copying attributes only
	DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();
	dataset_ptr->DCM_ATTRIBUTE_GROUP_CLASS::cloneAttributes(this);

	return dataset_ptr;
}