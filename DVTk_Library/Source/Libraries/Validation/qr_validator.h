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
//  DESCRIPTION     :   Query Retrieve Validator include file.
//*****************************************************************************
#ifndef QR_VALIDATOR_H
#define QR_VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"
#include "validator.h"
#include "val_object_results.h"     /* Remove when VAL_QR_OBJECT_RESULTS_CLASS is moved to a seperate file. */
#include "val_attribute_group.h"    /* Remove when VAL_QR_ATTRIBUTE_GROUP_CLASS is moved to a seperate file. */
#include "val_attribute.h"          /* Remove when VAL_QR_ATTRIBUTE_CLASS is moved to a seperate file. */

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_DATASET_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//>>***************************************************************************

class QR_VALIDATOR_CLASS : public VALIDATOR_CLASS

//  DESCRIPTION     : QR Validator Class
//  NOTES           :
//<<***************************************************************************
{
    public:
		QR_VALIDATOR_CLASS();
        virtual ~QR_VALIDATOR_CLASS();

        void ValidateResults(VALIDATION_CONTROL_FLAG_ENUM);

        bool CreateResultsObject();

        void SetReqDataSetResultsFromDcm(DCM_DATASET_CLASS*);
};

//>>***************************************************************************

class VAL_QR_OBJECT_RESULTS_CLASS : public VAL_OBJECT_RESULTS_CLASS

//  DESCRIPTION     : QR Object Validation Results Class
//  NOTES           :
//<<***************************************************************************
{
    public:
		VAL_QR_OBJECT_RESULTS_CLASS();
        virtual ~VAL_QR_OBJECT_RESULTS_CLASS();

        DVT_STATUS ValidateAgainstReq(UINT32);
};

//>>***************************************************************************

class VAL_QR_ATTRIBUTE_CLASS : public VAL_ATTRIBUTE_CLASS

//  DESCRIPTION     : QR Attribute Validation Results Class
//  NOTES           :
//<<***************************************************************************
{
    public:
        VAL_QR_ATTRIBUTE_CLASS();
        virtual ~VAL_QR_ATTRIBUTE_CLASS();

        void SetReqAttribute(DCM_ATTRIBUTE_CLASS*);

        DCM_ATTRIBUTE_CLASS *GetReqAttribute();

        void CheckAgainstRequested();

    private:
        DCM_ATTRIBUTE_CLASS *reqAttributeM_ptr;
};

#endif /* QR_VALIDATOR_H */
