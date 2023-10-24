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
//  DESCRIPTION     :   Contains valrules code-name mapping
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "valrules.h"


//*****************************************************************************
//  INTERNAL DEFINITIONS
//*****************************************************************************

//
// DIMSE command map
//
static T_VAL_RULE_CODE_STRING_STRUCT TValRuleMap[] =
{
	// General rules
	{VAL_RULE_GENERAL_1, "GENERAL_1"},
	// Rules regarding value representation
	{VAL_RULE_D_VR_1, "D_VR_1"},
	{VAL_RULE_D_VR_2, "D_VR_2"},
	{VAL_RULE_D_VR_3, "D_VR_3"},
	// Rules regarding value multiplicity
	{VAL_RULE_D_VM_1, "D_VM_1"},
	{VAL_RULE_D_VM_2, "D_VM_2"},
	{VAL_RULE_D_VM_3, "D_VM_3"},
	{VAL_RULE_D_VM_4, "D_VM_4"},
	{VAL_RULE_D_VM_5, "D_VM_5"},
    // Rules regarding value encoding
	// VR AE
	{VAL_RULE_D_AE_1, "D_AE_1"},
	{VAL_RULE_D_AE_2, "D_AE_2"},
	{VAL_RULE_D_AE_3, "D_AE_3"},
	// VR AS
	{VAL_RULE_D_AS_1, "D_AS_1"},
	{VAL_RULE_D_AS_2, "D_AS_2"},
	{VAL_RULE_D_AS_3, "D_AS_3"},
	// VR AT
	{VAL_RULE_D_AT_1, "D_AT_1"},
	{VAL_RULE_D_AT_2, "D_AT_2"},
	{VAL_RULE_D_AT_3, "D_AT_3"},
	// VR CS
	{VAL_RULE_D_CS_1, "D_CS_1"},
	{VAL_RULE_D_CS_2, "D_CS_2"},
	{VAL_RULE_D_CS_3, "D_CS_3"},
	// VR DA
	{VAL_RULE_D_DA_1 , "D_DA_1"},
	{VAL_RULE_D_DA_2 , "D_DA_2"},
	{VAL_RULE_D_DA_3 , "D_DA_3"},
	{VAL_RULE_D_DA_4 , "D_DA_4"},
	{VAL_RULE_D_DA_5 , "D_DA_5"},
	{VAL_RULE_D_DA_6 , "D_DA_6"},
	// VR DS
	{VAL_RULE_D_DS_1 , "D_DS_1"},
	{VAL_RULE_D_DS_2 , "D_DS_2"},
	{VAL_RULE_D_DS_3 , "D_DS_3"},
	{VAL_RULE_D_DS_4 , "D_DS_4"},
	// VR DT
	{VAL_RULE_D_DT_1 , "D_DT_1"},
	{VAL_RULE_D_DT_2 , "D_DT_2"},
	{VAL_RULE_D_DT_3 , "D_DT_3"},
	{VAL_RULE_D_DT_4 , "D_DT_4"},
	{VAL_RULE_D_DT_5 , "D_DT_5"},
	{VAL_RULE_D_DT_6 , "D_DT_6"},
	// VR FL
	{VAL_RULE_D_FL_1 , "D_FL_1"},
	{VAL_RULE_D_FL_2 , "D_FL_2"},
	{VAL_RULE_D_FL_3 , "D_FL_3"},
	// VR FD
	{VAL_RULE_D_FD_1 , "D_FD_1"},
	{VAL_RULE_D_FD_2 , "D_FD_2"},
	{VAL_RULE_D_FD_3 , "D_FD_3"},
	// VR IS
	{VAL_RULE_D_IS_1 , "D_IS_1"},
	{VAL_RULE_D_IS_2 , "D_IS_2"},
	{VAL_RULE_D_IS_3 , "D_IS_3"},
	// VR LO
	{VAL_RULE_D_LO_1 , "D_LO_1"},
	{VAL_RULE_D_LO_2 , "D_LO_2"},
	{VAL_RULE_D_LO_3 , "D_LO_3"},
	// VR LT
	{VAL_RULE_D_LT_1 , "D_LT_1"},
	{VAL_RULE_D_LT_2 , "D_LT_2"},
	{VAL_RULE_D_LT_3 , "D_LT_3"},
	// VR OB/OF/OW
	{VAL_RULE_D_OTHER_1  , "D_OTHER_1"},
	{VAL_RULE_D_OTHER_2  , "D_OTHER_2"},
	{VAL_RULE_D_OTHER_3  , "D_OTHER_3"},
	{VAL_RULE_D_OTHER_4  , "D_OTHER_4"},
	{VAL_RULE_D_OTHER_5  , "D_OTHER_5"},
	{VAL_RULE_D_OTHER_6  , "D_OTHER_6"},
	{VAL_RULE_D_OTHER_7  , "D_OTHER_7"},
	{VAL_RULE_D_OTHER_8  , "D_OTHER_8"},
	{VAL_RULE_D_OTHER_9  , "D_OTHER_9"},
	{VAL_RULE_D_OTHER_10 , "D_OTHER_10"},
	{VAL_RULE_D_OTHER_11 , "D_OTHER_11"},
	{VAL_RULE_D_OTHER_12 , "D_OTHER_12"},
	{VAL_RULE_D_OTHER_13 , "D_OTHER_13"},
	{VAL_RULE_D_OTHER_14 , "D_OTHER_14"},
	{VAL_RULE_D_OTHER_15 , "D_OTHER_15"},
	{VAL_RULE_D_OTHER_16 , "D_OTHER_16"},
	{VAL_RULE_D_OTHER_17 , "D_OTHER_17"},
	{VAL_RULE_D_OTHER_18 , "D_OTHER_18"},
	{VAL_RULE_D_OTHER_19 , "D_OTHER_19"},
	{VAL_RULE_D_OTHER_20 , "D_OTHER_20"},
	// VR PN
	{VAL_RULE_D_PN_1, "D_PN_1"},
	{VAL_RULE_D_PN_2, "D_PN_2"},    
	{VAL_RULE_D_PN_3, "D_PN_3"},    
	// VR SH
	{VAL_RULE_D_SH_1, "D_SH_1"},    
	{VAL_RULE_D_SH_2, "D_SH_2"},    
	{VAL_RULE_D_SH_3, "D_SH_3"},    
	// VR SL
	{VAL_RULE_D_SL_1, "D_SL_1"},    
	{VAL_RULE_D_SL_2, "D_SL_2"},    
	{VAL_RULE_D_SL_3, "D_SL_3"},    
	// VR SS
	{VAL_RULE_D_SS_1, "D_SS_1"},    
	{VAL_RULE_D_SS_2, "D_SS_2"},    
	{VAL_RULE_D_SS_3, "D_SS_3"},    
	// VR ST
	{VAL_RULE_D_ST_1, "D_ST_1"},    
	{VAL_RULE_D_ST_2, "D_ST_2"},    
	{VAL_RULE_D_ST_3, "D_ST_3"},    
	// VR TM
	{VAL_RULE_D_TM_1, "D_TM_1"},    
	{VAL_RULE_D_TM_2, "D_TM_2"},    
	{VAL_RULE_D_TM_3, "D_TM_3"},    
	{VAL_RULE_D_TM_4, "D_TM_4"},    
	{VAL_RULE_D_TM_5, "D_TM_5"},    
	{VAL_RULE_D_TM_6, "D_TM_6"},    
	// VR UI
	{VAL_RULE_D_UI_1, "D_UI_1"},    
	{VAL_RULE_D_UI_2, "D_UI_2"},    
	{VAL_RULE_D_UI_3, "D_UI_3"},    
	{VAL_RULE_D_UI_4, "D_UI_4"},    
	{VAL_RULE_D_UI_5, "D_UI_5"},    
	{VAL_RULE_D_UI_6, "D_UI_6"},    
	// VR UL
	{VAL_RULE_D_UL_1, "D_UL_1"},    
	{VAL_RULE_D_UL_2, "D_UL_2"},    
	{VAL_RULE_D_UL_3, "D_UL_3"},    
	// VR UN
	{VAL_RULE_D_UN_1, "D_UN_1"},  
	{VAL_RULE_D_UN_2, "D_UN_2"},  
	{VAL_RULE_D_UN_3, "D_UN_3"},  
	// VR US
	{VAL_RULE_D_US_1, "D_US_1"},  
	{VAL_RULE_D_US_2, "D_US_2"},  
	{VAL_RULE_D_US_3, "D_US_3"},  
	// VR UT
	{VAL_RULE_D_UT_1, "D_UT_1"},  
	{VAL_RULE_D_UT_2, "D_UT_2"},  
	{VAL_RULE_D_UT_3, "D_UT_3"},  
	// VR Extended character set rules
	{VAL_RULE_D_EXT_1, "D_EXT_1"},  
	{VAL_RULE_D_EXT_2, "D_EXT_2"},  
	{VAL_RULE_D_EXT_3, "D_EXT_3"},  
	{VAL_RULE_D_EXT_4, "D_EXT_4"},  
	{VAL_RULE_D_EXT_5, "D_EXT_5"},  
	{VAL_RULE_D_EXT_6, "D_EXT_6"},  
	{VAL_RULE_D_EXT_7, "D_EXT_7"},  
	{VAL_RULE_D_EXT_8, "D_EXT_8"},  
	{VAL_RULE_D_EXT_9, "D_EXT_9"},  
	{VAL_RULE_D_EXT_10, "D_EXT_10"}, //string too long  
	// Length odd - should be even
	{VAL_RULE_D_ODDLEN_1, "D_ODDLEN_1"},
	// Rules regarding value range
	{VAL_RULE_D_RANGE_1, "D_RANGE_1"},    
	// Rules regarding attribute presence
	{VAL_RULE_D_PRESENCE_1, "D_PRESENCE_1"}, 
	{VAL_RULE_D_PRESENCE_2, "D_PRESENCE_2"}, 
	{VAL_RULE_D_PRESENCE_3, "D_PRESENCE_3"}, 
	{VAL_RULE_D_PRESENCE_4, "D_PRESENCE_4"},
	{VAL_RULE_D_PRESENCE_5, "D_PRESENCE_5"},
	{VAL_RULE_D_PRESENCE_6, "D_PRESENCE_6"},
	{VAL_RULE_D_PRESENCE_7, "D_PRESENCE_7"},
	{VAL_RULE_D_PRESENCE_8, "D_PRESENCE_8"},
	{VAL_RULE_D_PRESENCE_9, "D_PRESENCE_9"},
	{VAL_RULE_D_PRESENCE_10, "D_PRESENCE_10"},
	{VAL_RULE_D_PRESENCE_11, "D_PRESENCE_11"},
	{VAL_RULE_D_PRESENCE_12, "D_PRESENCE_12"},
	{VAL_RULE_D_PRESENCE_13, "D_PRESENCE_13"},
 	{VAL_RULE_D_PRESENCE_14, "D_PRESENCE_14"},
   // Rules regarding acse parameters
	{VAL_RULE_D_PARAM_1, "D_PARAM_1"},
	{VAL_RULE_D_PARAM_2, "D_PARAM_2"},
	{VAL_RULE_D_PARAM_3, "D_PARAM_3"},
	{VAL_RULE_D_PARAM_4, "D_PARAM_4"},
	{VAL_RULE_D_PARAM_5, "D_PARAM_5"},
	{VAL_RULE_D_PARAM_6, "D_PARAM_6"},
	{VAL_RULE_D_PARAM_7, "D_PARAM_7"},
	// Rules regarding private attributes
	{VAL_RULE_PRIVATE_1, "VAL_RULE_PRIVATE_1"},
	{VAL_RULE_PRIVATE_2, "VAL_RULE_PRIVATE_2"},
	// Rules regarding reference object
	//
	// Codes have prefix R_
	//
	// Rules regarding value range
	{VAL_RULE_R_VALUE_1, "R_VALUE_1"},
   	{VAL_RULE_R_VALUE_2, "R_VALUE_2"},    

	// Rules regarding value representation
	{VAL_RULE_R_VR_1, "R_VR_1"},       
	{VAL_RULE_R_VR_2, "R_VR_2"},       
	// Rules regarding value multiplicity
	{VAL_RULE_R_VM_1, "R_VM_1"},       
	// Rules regarding attribute presence
	{VAL_RULE_R_PRESENCE_1, "R_PRESENCE_1"}, 
	// Rules regarding ACSE reference presence
	{VAL_RULE_R_PARAM_1, "R_PARAM_1"},
	{VAL_RULE_R_PARAM_2, "R_PARAM_2"},
    {VAL_RULE_R_PARAM_3, "R_PARAM_3"},

	// Additional validation rules 
	//
	// Codes have prefix A_
	//
	{VAL_RULE_A_MEDIA_1, "A_MEDIA_1"}, 
	{VAL_RULE_A_MEDIA_2, "A_MEDIA_2"}, 
	{VAL_RULE_A_MEDIA_3, "A_MEDIA_3"}, 
	{VAL_RULE_A_MEDIA_4, "A_MEDIA_4"}, 
	{VAL_RULE_A_MEDIA_5, "A_MEDIA_5"}, 
	{VAL_RULE_A_MEDIA_6, "A_MEDIA_6"}, 
	{VAL_RULE_A_MEDIA_7, "A_MEDIA_7"},
	{VAL_RULE_A_MEDIA_8, "A_MEDIA_8"},
	// Sentinel
	{VAL_RULE_UNKNOWN, "No Rule Translation"}
};


//>>===========================================================================

const char* mapValRule(const UINT32 code)

//  DESCRIPTION     : Function to map the validation rule code into a string
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int i = 0;
	
	while ((TValRuleMap[i].code != code) &&
	       (TValRuleMap[i].code != VAL_RULE_UNKNOWN))
	{
		i++;
	}

	// resturn matching code string
	return TValRuleMap[i].codeString_ptr;
}
