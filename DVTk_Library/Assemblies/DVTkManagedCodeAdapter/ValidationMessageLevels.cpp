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

//  Validation Message Level Lookup.
#include "StdAfx.h"
#include ".\ValidationMessageLevels.h"
#include "ActivityReportingAdapter.h"
#using <mscorlib.dll>

namespace Wrappers
{
// TODO : Missing rules VAL_RULE_D_OBOW!!!
    void ValidationMessageInfo::_StaticConstructor()
    {
        pRuleUriStandardRules = gcnew System::Uri("urn:rules-dvtk:standard");
        pRuleUriStrictRules = gcnew System::Uri("urn:rules-dvtk:strict");
        //
        // Create pre-boxed values to add to hashtable
        //
        //System::Object __gc* debug                   = __box(WrappedValidationMessageLevel::Debug);
        //System::Object __gc* dicomObjectRelationship = __box(WrappedValidationMessageLevel::DicomObjectRelationship);
        //System::Object __gc* dulpStateMachine        = __box(WrappedValidationMessageLevel::DulpStateMachine);
        System::Object^ error                   = WrappedValidationMessageLevel::Error;
        System::Object^ information             = WrappedValidationMessageLevel::Information;
        System::Object^ none                    = WrappedValidationMessageLevel::None;
        //System::Object __gc* scripting               = __box(WrappedValidationMessageLevel::Scripting);
        //System::Object __gc* scriptName              = __box(WrappedValidationMessageLevel::ScriptName);
        //System::Object __gc* mediaFilename           = __box(WrappedValidationMessageLevel::MediaFilename);
        //System::Object __gc* wareHouseLabel          = __box(WrappedValidationMessageLevel::WareHouseLabel);
        System::Object^ warning                 = WrappedValidationMessageLevel::Warning;
        System::Object^ conditionText           = WrappedValidationMessageLevel::ConditionText;
        pLevelTable = gcnew System::Collections::Hashtable();
        pLevelTableStandard = gcnew System::Collections::Hashtable();
        pLevelTableStrict = gcnew System::Collections::Hashtable();
        //
        // Fill hash table with values
        //
        //pLevelTable->Add(__box(::VAL_RULE_GENERAL_BASE          ), none);
        //pLevelTable->Add(__box(::VAL_RULE_A_MEDIA_BASE          ), none);
        //pLevelTable->Add(__box(::VAL_RULE_D_VR_BASE             ), none);
        //pLevelTable->Add(__box(::VAL_RULE_D_VM_BASE             ), none);
        //pLevelTable->Add(__box(::VAL_RULE_D_ENCODING_BASE       ), none);
        //pLevelTable->Add(__box(::VAL_RULE_D_RANGE_BASE          ), none);
        //pLevelTable->Add(__box(::VAL_RULE_D_PRESENCE_BASE       ), none);
        //pLevelTable->Add(__box(::VAL_RULE_DEF_COMMAND_BASE      ), none);
        //pLevelTable->Add(__box(::VAL_RULE_DEF_DEFINITION_BASE   ), none);
        //pLevelTable->Add(__box(::VAL_RULE_R_REF_BASE            ), none);
        //pLevelTable->Add(__box(::VAL_RULE_A_QR_BASE             ), none);
        //pLevelTable->Add(__box(::VAL_RULE_A_WLM_BASE            ), none);
        //pLevelTable->Add(__box(::VAL_RULE_A_SR_BASE             ), none);
        //pLevelTable->Add(__box(::VAL_RULE_ACSE_BASE		      ), none);
        //pLevelTable->Add(__box(::VAL_RULE_INTERNAL_ERROR_BASE   ), none);
        //
        // General rules
        //
        pLevelTable->Add(::VAL_RULE_GENERAL_BASE      , none);
        pLevelTable->Add(::VAL_RULE_GENERAL_1         , error);
        //
        // Rules regarding VR compatibility with definition file
        //
        pLevelTable->Add(::VAL_RULE_D_VR_1            , error);
        pLevelTable->Add(::VAL_RULE_D_VR_2            , information);
        pLevelTable->Add(::VAL_RULE_D_VR_3            , error);
        //
        // Rules regarding value multiplicity
        //
        pLevelTable->Add(::VAL_RULE_D_VM_1            , error);
        pLevelTable->Add(::VAL_RULE_D_VM_2            , error);
        pLevelTable->Add(::VAL_RULE_D_VM_3            , error);
        pLevelTable->Add(::VAL_RULE_D_VM_4            , error);
        pLevelTable->Add(::VAL_RULE_D_VM_5            , warning);
        //
        // Rules regarding value encoding
        //
        // VR AE
        //
        pLevelTable->Add(::VAL_RULE_D_AE_1            , error);
        pLevelTable->Add(::VAL_RULE_D_AE_2            , error);
        pLevelTable->Add(::VAL_RULE_D_AE_3            , error);
        //
        // VR AS
        //
        pLevelTable->Add(::VAL_RULE_D_AS_1            , error);
        pLevelTable->Add(::VAL_RULE_D_AS_2            , error);
        pLevelTable->Add(::VAL_RULE_D_AS_3            , error);
        //
        // VR AT
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_AT_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_AT_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_AT_3            ), error);
        //
        // VR CS
        //
        pLevelTable->Add(::VAL_RULE_D_CS_1            , error);
        pLevelTable->Add(::VAL_RULE_D_CS_2            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_CS_3            ), error);
        //
        // VR DA
        //
        pLevelTable->Add(::VAL_RULE_D_DA_1            , error);
        pLevelTable->Add(::VAL_RULE_D_DA_2            , error);
        pLevelTable->Add(::VAL_RULE_D_DA_3            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_DA_4            ), error);
        pLevelTable->Add(::VAL_RULE_D_DA_5            , error);
        pLevelTable->Add(::VAL_RULE_D_DA_6            , warning);
        pLevelTable->Add(::VAL_RULE_D_DA_7            , error);
        pLevelTable->Add(::VAL_RULE_D_DA_8            , warning);
        //
        // VR DS
        //
        pLevelTable->Add(::VAL_RULE_D_DS_1            , error);
        pLevelTable->Add(::VAL_RULE_D_DS_2            , error);
        pLevelTable->Add(::VAL_RULE_D_DS_3            , error);
        pLevelTable->Add(::VAL_RULE_D_DS_4            , error);
        // VR DT
        pLevelTable->Add(::VAL_RULE_D_DT_1            , error);
        pLevelTable->Add(::VAL_RULE_D_DT_2            , error);
        pLevelTable->Add(::VAL_RULE_D_DT_3            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_DT_4            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_DT_5            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_DT_6            ), error);
        //
        // VR FL
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_FL_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_FL_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_FL_3            ), error);
        //
        // VR FD
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_FD_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_FD_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_FD_3            ), error);
        //
        // VR IS
        //
        pLevelTable->Add(::VAL_RULE_D_IS_1            , error);
        pLevelTable->Add(::VAL_RULE_D_IS_2            , error);
        pLevelTable->Add(::VAL_RULE_D_IS_3            , error);
        //
        // VR LO
        //
        pLevelTable->Add(::VAL_RULE_D_LO_1            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_LO_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_LO_3            ), error);
        //
        // VR LT
        //
        pLevelTable->Add(::VAL_RULE_D_LT_1            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_LT_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_LT_3            ), error);
        //
        // VR OB/OF/OW
        //
        pLevelTable->Add(::VAL_RULE_D_OTHER_1         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_2         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_3         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_4         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_5         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_6         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_7         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_8         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_9         , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_10        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_11        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_12        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_13        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_14        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_15        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_16        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_17        , error);
        pLevelTable->Add(::VAL_RULE_D_OTHER_18        , information);
        pLevelTable->Add(::VAL_RULE_D_OTHER_19        , information);
        pLevelTable->Add(::VAL_RULE_D_OTHER_20        , error);
        //
        // VR PN
        //
        pLevelTable->Add(::VAL_RULE_D_PN_1            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_PN_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_PN_3            ), error);
        //
        // VR SH
        //
        pLevelTable->Add(::VAL_RULE_D_SH_1            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_SH_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_SH_3            ), error);
        //
        // VR SL
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_SL_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_SL_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_SL_3            ), error);
        //
        // VR SS
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_SS_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_SS_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_SS_3            ), error);
        //
        // VR ST
        //
        pLevelTable->Add(::VAL_RULE_D_ST_1            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_ST_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_ST_3            ), error);
        //
        // VR TM
        //
        pLevelTable->Add(::VAL_RULE_D_TM_1            , error);
        pLevelTable->Add(::VAL_RULE_D_TM_2            , error);
        pLevelTable->Add(::VAL_RULE_D_TM_3            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_TM_4            ), error);
        pLevelTable->Add(::VAL_RULE_D_TM_5            , error);
        pLevelTable->Add(::VAL_RULE_D_TM_6            , warning);
        pLevelTable->Add(::VAL_RULE_D_TM_7            , error);
        pLevelTable->Add(::VAL_RULE_D_TM_8            , error);
        pLevelTable->Add(::VAL_RULE_D_TM_9            , error);
        pLevelTable->Add(::VAL_RULE_D_TM_A            , warning);
        //
        // VR UI
        //
        pLevelTable->Add(::VAL_RULE_D_UI_1            , error);
        pLevelTable->Add(::VAL_RULE_D_UI_2            , error);
        pLevelTable->Add(::VAL_RULE_D_UI_3            , error);
        pLevelTable->Add(::VAL_RULE_D_UI_4            , error);
        pLevelTable->Add(::VAL_RULE_D_UI_5            , error);
        pLevelTable->Add(::VAL_RULE_D_UI_6            , error);
        //
        // VR UL
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_UL_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_UL_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_UL_3            ), error);
        //
        // VR UN
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_UN_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_UN_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_UN_3            ), error);
        //
        // VR US
        //
        //pLevelTable->Add(__box(::VAL_RULE_D_US_1            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_US_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_US_3            ), error);
        //
        // VR UT
        //
        pLevelTable->Add(::VAL_RULE_D_UT_1            , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_UT_2            ), error);
        //pLevelTable->Add(__box(::VAL_RULE_D_UT_3            ), error);
        //
        // VR Extended character set rules
        //
        pLevelTable->Add(::VAL_RULE_D_EXT_1           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_2           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_3           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_4           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_5           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_6           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_7           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_8           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_9           , error);
        pLevelTable->Add(::VAL_RULE_D_EXT_10          , error);
		//
		// Length odd - should be even
		//
        pLevelTable->Add(::VAL_RULE_D_ODDLEN_1        , error);
        //
        // Rules regarding value range
        //
        pLevelTable->Add(::VAL_RULE_D_RANGE_1         , error);
        pLevelTable->Add(::VAL_RULE_D_RANGE_2         , warning);
        pLevelTable->Add(::VAL_RULE_D_RANGE_3         , error);
        //
        // Rules regarding attribute presence
        //
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_1      , error);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_2      , error);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_3      , error);
        //pLevelTable->Add(__box(::VAL_RULE_D_PRESENCE_4      ), warning);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_5      , conditionText);
        //
        //SPLIT: pLevelTable->Add(__box(::VAL_RULE_D_PRESENCE_6      ), error);
        //
        pLevelTableStandard->Add(::VAL_RULE_D_PRESENCE_6      , information);
        pLevelTableStrict->Add(::VAL_RULE_D_PRESENCE_6      , error);

        pLevelTable->Add(::VAL_RULE_D_PRESENCE_7      , information);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_8      , information);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_9      , warning);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_10     , error);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_11     , warning);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_12     , conditionText);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_13     , error);
        pLevelTable->Add(::VAL_RULE_D_PRESENCE_14     , information);
		//
        // Rules regarding ACSE parameters
        //
        pLevelTable->Add(::VAL_RULE_D_PARAM_1         , error);
        pLevelTable->Add(::VAL_RULE_D_PARAM_2         , error);
        pLevelTable->Add(::VAL_RULE_D_PARAM_3         , error);
        pLevelTable->Add(::VAL_RULE_D_PARAM_4         , error);
        pLevelTable->Add(::VAL_RULE_D_PARAM_5         , warning);
        pLevelTable->Add(::VAL_RULE_D_PARAM_6         , warning);
        pLevelTable->Add(::VAL_RULE_D_PARAM_7         , warning);
		//
		// Rules regarding private attributes
		//
        pLevelTable->Add(::VAL_RULE_PRIVATE_1         , error);
        pLevelTable->Add(::VAL_RULE_PRIVATE_2         , error);
        //
        // Rules regarding checks against definition object
        //
        pLevelTable->Add(::VAL_RULE_DEF_COMMAND_1     , error);
        pLevelTable->Add(::VAL_RULE_DEF_COMMAND_2     , error);
		pLevelTable->Add(::VAL_RULE_DEF_COMMAND_3     , error);
        pLevelTable->Add(::VAL_RULE_DEF_DEFINITION_1  , error);
        pLevelTable->Add(::VAL_RULE_DEF_DEFINITION_2  , error);
        pLevelTable->Add(::VAL_RULE_DEF_DEFINITION_3  , warning);
        pLevelTable->Add(::VAL_RULE_DEF_DEFINITION_4  , information);
        pLevelTable->Add(::VAL_RULE_DEF_DEFINITION_5  , error);
        pLevelTable->Add(::VAL_RULE_DEF_DEFINITION_6  , information);
        pLevelTable->Add(::VAL_RULE_DEF_DEFINITION_7  , information);
		//
		// Rules regarding the pixel data length
		//
		pLevelTable->Add(::VAL_RULE_PIXEL_DATA_1  , error);
		pLevelTable->Add(::VAL_RULE_PIXEL_DATA_2  , error);
		pLevelTable->Add(::VAL_RULE_PIXEL_DATA_3  , error);
		pLevelTable->Add(::VAL_RULE_PIXEL_DATA_4  , error);
		pLevelTable->Add(::VAL_RULE_PIXEL_DATA_5  , error);
		pLevelTable->Add(::VAL_RULE_PIXEL_DATA_6  , error);
        //
        // Rules regarding checks against the reference ob
        //
        // Codes have prefix R_
        //
        // Rules regarding value range
        //
        pLevelTable->Add(::VAL_RULE_R_VALUE_1         , error);
        pLevelTable->Add(::VAL_RULE_R_VALUE_2         , warning);
        //
        // Rules regarding value representation
        //
        pLevelTable->Add(::VAL_RULE_R_VR_1            , error);
        pLevelTable->Add(::VAL_RULE_R_VR_2            , warning);
       //
        // Rules regarding value multiplicity
        //
        pLevelTable->Add(::VAL_RULE_R_VM_1            , error);
        //
        // Rules regarding attribute presence
        // 
        pLevelTable->Add(::VAL_RULE_R_PRESENCE_1      , error);
        //
        // Rules regarding ACSE reference presence
        //
        pLevelTable->Add(::VAL_RULE_R_PARAM_1         , warning);
        pLevelTable->Add(::VAL_RULE_R_PARAM_2         , warning);
        pLevelTable->Add(::VAL_RULE_R_PARAM_3         , warning);

        //
        // Additional validation rules 
        //
        // Codes have prefix A_
        //
        pLevelTable->Add(::VAL_RULE_A_MEDIA_1         , error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_2         , error);
        //pLevelTable->Add(__box(::VAL_RULE_A_MEDIA_3         ), error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_4         , error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_5         , error);
        //pLevelTable->Add(__box(::VAL_RULE_A_MEDIA_6         ), error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_7         , error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_8         , warning);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_9         , error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_A         , information);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_B         , error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_C         , error);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_SOP_1     , information);
        pLevelTable->Add(::VAL_RULE_A_MEDIA_SOP_2     , warning);
        //
        // Validation rules specific for Query/Retrieve
        //
        pLevelTable->Add(::VAL_RULE_A_QR_1            , warning);
        //
        // Messages for internal DVT errors.
        //
        //pLevelTable->Add(__box(::VAL_RULE_INTERNAL_ERROR_1  ), error);
        //pLevelTable->Add(__box(::VAL_RULE_UNKNOWN           ), error);
    }
    
    WrappedValidationMessageLevel ValidationMessageInfo::GetLevel(
        System::UInt32 messageUID,
        System::Uri^ pRulesUri)
    {
        enum RulesSet
        {
            RulesSetStandard,
            RulesSetStrict
        };

        if (pRulesUri == nullptr) throw gcnew System::ArgumentNullException();
        RulesSet rulesSet;
        if (pRulesUri->Equals(ValidationMessageInfo::pRuleUriStandardRules))
        {
            rulesSet = RulesSetStandard;
        }
        else if (pRulesUri->Equals(ValidationMessageInfo::pRuleUriStrictRules))
        {
            rulesSet = RulesSetStrict;
        }
        else
        {
            throw gcnew System::ArgumentException();
        }
        WrappedValidationMessageLevel level;
        System::Object^ oMessageUID = messageUID;
        System::Object^ oLevel = nullptr;
        switch (rulesSet)
        {
        case RulesSetStandard:
            if (pLevelTable->ContainsKey(oMessageUID))
            {
                oLevel = pLevelTable[oMessageUID];
            }
            else if (pLevelTableStandard->ContainsKey(oMessageUID))
            {
                oLevel = pLevelTableStandard[oMessageUID];
            }
            else
            {
                oLevel = nullptr;
            }
            //oLevel = pLevelTable->get_Item(oMessageUID);
            break;
        case RulesSetStrict:
            if (pLevelTable->ContainsKey(oMessageUID))
            {
                oLevel = pLevelTable[oMessageUID];
            }
            else if (pLevelTableStrict->ContainsKey(oMessageUID))
            {
                oLevel = pLevelTableStrict[oMessageUID];
            }
            else
            {
                oLevel = nullptr;
            }
            break;
        default:
            System::Diagnostics::Trace::Assert(false);
            break;
        }
        if (oLevel != nullptr)
        {
            // unbox
            level = *dynamic_cast<WrappedValidationMessageLevel^>(oLevel);
        }
        else
        {
            throw gcnew System::NotImplementedException();
//            level = WrappedValidationMessageLevel::None;
        }
        return level;
    }
}