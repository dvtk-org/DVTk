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
#include "dcm_dir_dataset.h"

//>>===========================================================================

DCM_DIR_DATASET_CLASS::DCM_DIR_DATASET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
    sqValueM_ptr = NULL;
}

//>>===========================================================================

DCM_DIR_DATASET_CLASS::~DCM_DIR_DATASET_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
    if (sqValueM_ptr)
    {
        delete sqValueM_ptr;
    }
}

//>>===========================================================================

bool DCM_DIR_DATASET_CLASS::decodeToFirstRecord(DATA_TF_CLASS &dataTransfer)

//  DESCRIPTION     : Decode the DICOM DIR up to the beginning of the first
//                  : Directory Record.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialize the Private Attribute Handler
    initializePrivateAttributeHandler();

    bool lookingForDirectoryRecordSequence = true;

	// decode all attributes
	while ((dataTransfer.isData()) &&
        (lookingForDirectoryRecordSequence))
	{
		DCM_ATTRIBUTE_CLASS *attribute_ptr = new DCM_ATTRIBUTE_CLASS();
		if (attribute_ptr == NULL) return false;

		// cascade the logger
		attribute_ptr->setLogger(loggerM_ptr);

		// cascade the parent
		attribute_ptr->setParent(this);

		// cascade the private attribute handler
		attribute_ptr->setPAH(pahM_ptr);

		// set the UN VR definition look-up flag
		attribute_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

		// set EnsureEvenAttributeValueLength
		attribute_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

        UINT16 lastGroup = GROUP_FOUR;
        UINT16 lastElement = DIRECTORY_RECORD_SEQUENCE;

		// decode attributes one at a time
        UINT32 attributeLength;
		if (attribute_ptr->decode(dataTransfer, lastGroup, lastElement,  &attributeLength))
		{
			// add attribute to object
			addAttribute(attribute_ptr);
		}
		else
		{
			// check if we have reached the Directory Record Sequence
			if (attribute_ptr->GetGroup() < lastGroup)
			{
        		// clean up
		        delete attribute_ptr;

				// return error when reason for stopping is not the last group reached
				return false;
			}

     	    // decode group
    	    UINT16 group;
	        dataTransfer >> group;

            // decode element
	        UINT16 element;
	        dataTransfer >> element;;

            // check that we have reached the Directory Record Sequence
            if ((group != GROUP_FOUR) ||
                (element != DIRECTORY_RECORD_SEQUENCE))
            {
        		// clean up
		        delete attribute_ptr;

                // rewound in decode() method to reread this tag but something has gone wrong
                return false;
            }

            // check on explicit VR
            ATTR_VR_ENUM vr = ATTR_VR_DOESNOTEXIST;
            if (dataTransfer.isExplicitVR()) 
            {
	            BYTE    sl[2];
	            UINT16	length16;

	            // we get the VR given
	            dataTransfer >> sl[0];
	            dataTransfer >> sl[1];
	            UINT16 vr16 = (((UINT16) sl[0]) << 8) + ((UINT16) sl[1]);
	            vr = dataTransfer.vr16ToVr(vr16);

   	            // set transfer vr
	            attribute_ptr->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);

	            // should be a sequence
	            if (vr != ATTR_VR_SQ)
	            {
		            // sequence attribute expected
		            if (loggerM_ptr)
		            {
			            loggerM_ptr->text(LOG_ERROR, 1, "Attribute (%04X,%04X) - should be VR of SQ but %02X%02X found.", group, element, sl[0], sl[1]);
		            }
		            return false;
	            }

	            // special SQ encoding
		        // - decode 16 bit padding
		        dataTransfer >> length16;
	        }
            else
            {
	            // set transfer vr
	            attribute_ptr->setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);

    			// implicit VR
	        	vr = ATTR_VR_SQ;
	        }

	        // decode 32 bit length
            UINT32 length32;
	        dataTransfer >> length32;

            // save the record sequence (as an empty sequence in the dataset
            attribute_ptr->SetVR(vr);
            addAttribute(attribute_ptr);

        	// save the sequence value decoder for the calls to get next items
			sqValueM_ptr = new DCM_VALUE_SQ_CLASS(length32);

			// cascade the logger
			sqValueM_ptr->setLogger(loggerM_ptr);

			// cascade the private attribute handler
			sqValueM_ptr->setPAH(getPAH());

            // found the directory record sequence
            lookingForDirectoryRecordSequence = false;
        }
	}

	// return result
	return true;
}

//>>===========================================================================

DCM_ITEM_CLASS *DCM_DIR_DATASET_CLASS::getNextDirRecord(DATA_TF_CLASS &dataTransfer)

//  DESCRIPTION     : Decode the next Directory Record.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    // use the sequence value as the decoder
    if (sqValueM_ptr == NULL) return NULL;

    // get the next item decoded
    DCM_ITEM_CLASS *item_ptr = sqValueM_ptr->decodeNextItem(dataTransfer);

    // check if the last item has been reached
    if (item_ptr == NULL)
    {
        // terminate the private attribute handler
        terminatePrivateAttributeHandler();
    }

    // return next item decoded - maybe NULL returned
    return item_ptr;
}
