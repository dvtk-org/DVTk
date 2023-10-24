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
//  DESCRIPTION     :   Attribute results include file for validation.
//*****************************************************************************
#ifndef VAL_ATTRIBUTE_H
#define VAL_ATTRIBUTE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_VALUE_CLASS;
class VALUE_LIST_CLASS;
class DEF_ATTRIBUTE_CLASS;
class DCM_ATTRIBUTE_CLASS;
class VAL_BASE_VALUE_CLASS;
class LOG_MESSAGE_CLASS;
class VAL_ATTRIBUTE_GROUP_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;


//>>***************************************************************************

class VAL_ATTRIBUTE_CLASS

//  DESCRIPTION     : Validation Attribute Class
//  NOTES           :
//<<***************************************************************************
{
    public:
	    VAL_ATTRIBUTE_CLASS();
	    virtual ~VAL_ATTRIBUTE_CLASS();

        void SetParent(VAL_ATTRIBUTE_GROUP_CLASS*);
        VAL_ATTRIBUTE_GROUP_CLASS *GetParent();

        void SetDefAttribute(DEF_ATTRIBUTE_CLASS*, bool);
        void SetRefAttribute(DCM_ATTRIBUTE_CLASS*);
        void SetDcmAttribute(DCM_ATTRIBUTE_CLASS*);
        DEF_ATTRIBUTE_CLASS *GetDefAttribute();
        DCM_ATTRIBUTE_CLASS *GetRefAttribute();
        DCM_ATTRIBUTE_CLASS *GetDcmAttribute();

        bool AddValValue(VAL_BASE_VALUE_CLASS*);
        int GetNrValues();
        VAL_BASE_VALUE_CLASS *GetValue(int valueIndex = 0);

        LOG_MESSAGE_CLASS *GetMessages();
        bool HasMessages();

        DVT_STATUS ValidateAgainstDef(UINT32);
        DVT_STATUS ValidateAgainstRef(UINT32);
        DVT_STATUS ValidateVR(UINT32, SPECIFIC_CHARACTER_SET_CLASS*);

        void SetUseConditionalTextDuringValidation(bool);
        bool GetUseConditionalTextDuringValidation();

		void SetDefAttributeType(ATTR_TYPE_ENUM);
		ATTR_TYPE_ENUM GetDefAttributeType();
		void SetDefAttributeVr(ATTR_VR_ENUM);
		ATTR_VR_ENUM GetDefAttributeVr();

    protected:
        VAL_ATTRIBUTE_GROUP_CLASS *parentM_ptr;
        DEF_ATTRIBUTE_CLASS *defAttributeM_ptr;
        DCM_ATTRIBUTE_CLASS *refAttributeM_ptr;
        DCM_ATTRIBUTE_CLASS *dcmAttributeM_ptr;
		ATTR_TYPE_ENUM defAttributeTypeM;			// copied from defAttributeM_ptr - modified during validation
		ATTR_VR_ENUM defAttributeVrM;				// copied from defAttributeM_ptr - modified during validation 
        LOG_MESSAGE_CLASS *messagesM_ptr;
        vector<VAL_BASE_VALUE_CLASS*> valuesM;
        bool useConditionalTextDuringValidationM;

        DVT_STATUS CheckDefPresence(UINT32);
        DVT_STATUS CheckDefVRCompatibility(UINT32);
        DVT_STATUS CheckRefPresence(UINT32);
        DVT_STATUS CheckAgainstDTAndEV(UINT32);
        DVT_STATUS CheckDefValueMultiplicity(UINT32);
        DVT_STATUS CheckVRCompatibility(UINT32);
        DVT_STATUS CheckRefVM(UINT32);
        DVT_STATUS CheckRefContent(UINT32);
		DVT_STATUS CheckPixelDataLength();
		DVT_STATUS CheckPrivateAttribute();
		bool IsDataStored(DCM_ATTRIBUTE_CLASS*);
};

#endif /* VAL_ATTRIBUTE_H */