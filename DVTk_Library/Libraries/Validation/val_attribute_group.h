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
//  DESCRIPTION     :   Attribute group results include file for validation.
//*****************************************************************************
#ifndef VAL_ATTRIBUTE_GROUP_H
#define VAL_ATTRIBUTE_GROUP_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_ATTRIBUTE_GROUP_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;
class VAL_ATTRIBUTE_CLASS;
class LOG_MESSAGE_CLASS;
class VAL_OBJECT_RESULTS_CLASS;
class VAL_VALUE_SQ_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;

//>>***************************************************************************

class VAL_ATTRIBUTE_GROUP_CLASS

//  DESCRIPTION     : Validation Attribute Group Class
//  NOTES           :
//<<***************************************************************************
{
    public:
        VAL_ATTRIBUTE_GROUP_CLASS();
        virtual ~VAL_ATTRIBUTE_GROUP_CLASS();

		void SetIgnoreThisAttributeGroup(bool);
		bool GetIgnoreThisAttributeGroup();

		bool IsOnlySingleMatchingAttribute();
		void SetSingleMatchingAttribute(UINT32);
		UINT32 GetSingleMatchingAttribute();

        void SetParentObject(VAL_OBJECT_RESULTS_CLASS*);
        void SetParentSQ(VAL_VALUE_SQ_CLASS*);
        VAL_OBJECT_RESULTS_CLASS * GetParentObject();

        void SetDefAttributeGroup(DEF_ATTRIBUTE_GROUP_CLASS*);
        void SetRefAttributeGroup(DCM_ATTRIBUTE_GROUP_CLASS*);
        void SetDcmAttributeGroup(DCM_ATTRIBUTE_GROUP_CLASS*);
        DEF_ATTRIBUTE_GROUP_CLASS *GetDefAttributeGroup();
        DCM_ATTRIBUTE_GROUP_CLASS *GetRefAttributeGroup();
        DCM_ATTRIBUTE_GROUP_CLASS *GetDcmAttributeGroup();

        bool AddValAttribute(VAL_ATTRIBUTE_CLASS*);
        int GetNrAttributes();
        VAL_ATTRIBUTE_CLASS *GetAttribute(int);
        VAL_ATTRIBUTE_CLASS *GetAttribute(UINT16, UINT16);
        VAL_ATTRIBUTE_CLASS *GetAttributeByTag(UINT32);

        VAL_ATTRIBUTE_CLASS *GetDefAttribute(UINT16, UINT16);
        VAL_ATTRIBUTE_CLASS *GetDefAttributeByTag(UINT32);

        DVT_STATUS ValidateAgainstDef(UINT32);
        DVT_STATUS ValidateAgainstRef(UINT32);
        DVT_STATUS ValidateVR(UINT32, SPECIFIC_CHARACTER_SET_CLASS*);

        LOG_MESSAGE_CLASS *GetMessages();
        bool HasMessages();
        
    private:
		bool ignoreThisAttributeGroupM;			// set true if this val_attribute_group should be ignored when producing the final validation
												// results.
		UINT32 tagOfSingleMatchingAttributeM;	// if only one attribute from the dcmAttributeGroupM_ptr is found in the defAttributeGroupM_ptr then
												// the tagOfSingleMatchingAttributeM stores the DICOM Tag of this attribute - this is later used
												// after building the results object to determine if this val_attribute_group should be included
												// in the final validation results.
        VAL_OBJECT_RESULTS_CLASS *parentObjectM_ptr;
        VAL_VALUE_SQ_CLASS *parentSqM_ptr;
        DEF_ATTRIBUTE_GROUP_CLASS *defAttributeGroupM_ptr;
        DCM_ATTRIBUTE_GROUP_CLASS *refAttributeGroupM_ptr;
        DCM_ATTRIBUTE_GROUP_CLASS *dcmAttributeGroupM_ptr;
        LOG_MESSAGE_CLASS *messagesM_ptr;
        vector<VAL_ATTRIBUTE_CLASS*> attributesM;
};

#endif /* VAL_ATTRIBUTE_GROUP_H */
