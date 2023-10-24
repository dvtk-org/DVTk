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
//  DESCRIPTION     :   Query/Retrieve validator class.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "qr_validator.h"
#include "val_object_results.h"

#include "Idicom.h"             // Dicom component interface file.

//>>===========================================================================

QR_VALIDATOR_CLASS::QR_VALIDATOR_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

QR_VALIDATOR_CLASS::~QR_VALIDATOR_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

bool QR_VALIDATOR_CLASS::CreateResultsObject()

//  DESCRIPTION     : Validates the data present in the object results
//                    structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    objectResultsM_ptr = new VAL_QR_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_QR;
	return (objectResultsM_ptr == NULL) ? false : true;
}

//>>===========================================================================

void QR_VALIDATOR_CLASS::ValidateResults(VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validates the data present in the object results
//                    structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // validate according to the current validation flag settings
    // - validate against the definition
    if (validationFlag & USE_DEFINITION)
    {
        if ((objectResultsM_ptr->GetCommand()->getCommandId() == DIMSE_CMD_CFIND_RQ) ||
			(objectResultsM_ptr->GetCommand()->getCommandId() == DIMSE_CMD_CMOVE_RQ) ||
			(objectResultsM_ptr->GetCommand()->getCommandId() == DIMSE_CMD_CGET_RQ))
		{
			// List Of UID Matching is allowed for C-FIND-RQ, C-MOVE-RQ and C-GET-RQ
            objectResultsM_ptr->ValidateAgainstDef(ATTR_FLAG_UI_LIST_ALLOWED);
        }
        else
        {
            objectResultsM_ptr->ValidateAgainstDef(ATTR_FLAG_NONE);
        }
    }

    // - validate the vr
    if (validationFlag & USE_VR)
    {
        if ((objectResultsM_ptr->GetCommand()->getCommandId() == DIMSE_CMD_CFIND_RQ) ||
			(objectResultsM_ptr->GetCommand()->getCommandId() == DIMSE_CMD_CMOVE_RQ) ||
			(objectResultsM_ptr->GetCommand()->getCommandId() == DIMSE_CMD_CGET_RQ))
        {
			// Ranges allowed for C-FIND-RQ
            objectResultsM_ptr->ValidateVR(ATTR_FLAG_RANGES_ALLOWED, specificCharacterSetM_ptr);
        }
        else
        {
            objectResultsM_ptr->ValidateVR(ATTR_FLAG_NONE, specificCharacterSetM_ptr);
        }
    }

    // - validate against the reference
    if (validationFlag & USE_REFERENCE)
    {
        objectResultsM_ptr->ValidateAgainstRef(ATTR_FLAG_NONE);
    }
}

//>>===========================================================================

void QR_VALIDATOR_CLASS::SetReqDataSetResultsFromDcm(DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Validates the data present in the object results
//                    structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (int i = 0; i < dataset_ptr->GetNrAttributes(); i++)
    {
        DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = dataset_ptr->GetAttribute(i);
        VAL_QR_ATTRIBUTE_CLASS *valAttr_ptr = static_cast<VAL_QR_ATTRIBUTE_CLASS*>
                    (objectResultsM_ptr->GetAttributeResults(dcmAttr_ptr->GetMappedGroup(), dcmAttr_ptr->GetMappedElement()));
        if (valAttr_ptr == NULL)
        {
            valAttr_ptr = new VAL_QR_ATTRIBUTE_CLASS();
            valAttr_ptr->SetParent(objectResultsM_ptr->GetAdditionalAttributeGroup());
            objectResultsM_ptr->GetAdditionalAttributeGroup()->AddValAttribute(valAttr_ptr);
        }
        valAttr_ptr->SetReqAttribute(dcmAttr_ptr);
    }
}

//>>===========================================================================

VAL_QR_OBJECT_RESULTS_CLASS::VAL_QR_OBJECT_RESULTS_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

VAL_QR_OBJECT_RESULTS_CLASS::~VAL_QR_OBJECT_RESULTS_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

DVT_STATUS VAL_QR_OBJECT_RESULTS_CLASS::ValidateAgainstReq(UINT32)

//  DESCRIPTION     : Validates the attributes from the requested object
//                    against the incoming data stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    VAL_ATTRIBUTE_GROUP_CLASS *valModule_ptr;
    for (int i = 0; i < GetNrModuleResults(); i++)
    {
        valModule_ptr = GetModuleResults(i);
        if (valModule_ptr)
        {
            for (int j = 0; j < valModule_ptr->GetNrAttributes(); j++)
            {
                static_cast<VAL_QR_ATTRIBUTE_CLASS*>(valModule_ptr->GetAttribute(j))->CheckAgainstRequested();
            }
        }
    }

    valModule_ptr = GetAdditionalAttributeGroup();
    if (valModule_ptr)
    {
        for (int i = 0; i < valModule_ptr->GetNrAttributes(); i++)
        {
            static_cast<VAL_QR_ATTRIBUTE_CLASS*>(valModule_ptr->GetAttribute(i))->CheckAgainstRequested();
        }
    }

    return MSG_OK;
}

//>>===========================================================================

VAL_QR_ATTRIBUTE_CLASS::VAL_QR_ATTRIBUTE_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    reqAttributeM_ptr = NULL;
}

//>>===========================================================================

VAL_QR_ATTRIBUTE_CLASS::~VAL_QR_ATTRIBUTE_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
}

//>>===========================================================================

void VAL_QR_ATTRIBUTE_CLASS::SetReqAttribute(DCM_ATTRIBUTE_CLASS *reqAttr_ptr)

//  DESCRIPTION     : Set the requested attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    reqAttributeM_ptr = reqAttr_ptr;
}

//>>===========================================================================

DCM_ATTRIBUTE_CLASS *VAL_QR_ATTRIBUTE_CLASS::GetReqAttribute()

//  DESCRIPTION     : Get the requested attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return reqAttributeM_ptr;
}

//>>===========================================================================

void VAL_QR_ATTRIBUTE_CLASS::CheckAgainstRequested()

//  DESCRIPTION     : Check against the requested attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (GetDcmAttribute() != NULL)
    {
        if (reqAttributeM_ptr == NULL)
        {
            char message[128];

            sprintf(message,
				"Returned attribute (0x%04X%04X) not requested",
                dcmAttributeM_ptr->GetMappedGroup(), 
				dcmAttributeM_ptr->GetMappedElement());
			GetMessages()->AddMessage(VAL_RULE_A_QR_1, message);
        }
    }
}
