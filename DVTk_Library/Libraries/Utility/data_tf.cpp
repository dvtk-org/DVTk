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
#include "data_tf.h"


//*****************************************************************************
//  STATIC DECLARATIONS
//*****************************************************************************

static T_VR_MAP TVRMap[] = {
	{ATTR_VR_AE,	UINT16_VR_AE,	"AE"},
	{ATTR_VR_AS,	UINT16_VR_AS,	"AS"},
	{ATTR_VR_AT,	UINT16_VR_AT,	"AT"},
	{ATTR_VR_CS,	UINT16_VR_CS,	"CS"},
	{ATTR_VR_DA,	UINT16_VR_DA,	"DA"},
	{ATTR_VR_DS,	UINT16_VR_DS,	"DS"},
	{ATTR_VR_DT,	UINT16_VR_DT,	"DT"},
	{ATTR_VR_FL,	UINT16_VR_FL,	"FL"},
	{ATTR_VR_FD,	UINT16_VR_FD,	"FD"},
	{ATTR_VR_IS,	UINT16_VR_IS,	"IS"},
	{ATTR_VR_LO,	UINT16_VR_LO,	"LO"},
	{ATTR_VR_LT,	UINT16_VR_LT,	"LT"},
	{ATTR_VR_OB,	UINT16_VR_OB,	"OB"},
	{ATTR_VR_OF,	UINT16_VR_OF,	"OF"},
	{ATTR_VR_OW,	UINT16_VR_OW,	"OW"},
	{ATTR_VR_OL,	UINT16_VR_OL,	"OL"},
	{ATTR_VR_OD,	UINT16_VR_OD,	"OD"},
	{ATTR_VR_OV,	UINT16_VR_OV,	"OV"},
	{ATTR_VR_PN,	UINT16_VR_PN,	"PN"},
	{ATTR_VR_SH,	UINT16_VR_SH,	"SH"},
	{ATTR_VR_SL,	UINT16_VR_SL,	"SL"},
	{ATTR_VR_SQ,	UINT16_VR_SQ,	"SQ"},
	{ATTR_VR_SS,	UINT16_VR_SS,	"SS"},
	{ATTR_VR_ST,	UINT16_VR_ST,	"ST"},
	{ATTR_VR_SV,	UINT16_VR_SV,	"SV"},
	{ATTR_VR_TM,	UINT16_VR_TM,	"TM"},
	{ATTR_VR_UI,	UINT16_VR_UI,	"UI"},
	{ATTR_VR_UL,	UINT16_VR_UL,	"UL"},
	{ATTR_VR_US,	UINT16_VR_US,	"US"},
	{ATTR_VR_UT,	UINT16_VR_UT,	"UT"},
	{ATTR_VR_UR,	UINT16_VR_UR,	"UR"},
	{ATTR_VR_UV,	UINT16_VR_UV,	"UV"},
	{ATTR_VR_UC,	UINT16_VR_UC,	"UC"},
	{ATTR_VR_QQ,	UINT16_VR_QQ,	"??"},	// Older (Merge) Unknown VR
	{ATTR_VR_UN,	UINT16_VR_UN,	"UN"},
	{ATTR_VR_DOESNOTEXIST,	UINT16_VR_DOESNOTEXIST,	"?."}	// used as sentinal
};


//>>===========================================================================

DATA_TF_CLASS::DATA_TF_CLASS()


//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// constructor activities
	storageModeM = SM_NO_STORAGE;
	tsCodeM = TS_IMPLICIT_VR | TS_LITTLE_ENDIAN;
	transferSyntaxM = IMPLICIT_VR_LITTLE_ENDIAN;

	// set the underlying endian-ness for the transfer
	setEndian(LITTLE_ENDIAN);
}

//>>===========================================================================

DATA_TF_CLASS::~DATA_TF_CLASS()

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

void DATA_TF_CLASS::setTsCode(TS_CODE tsCode, string transferSyntax)

//  DESCRIPTION     : Set the transfer syntax code for the import/export methods.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// set transfer syntax code
	tsCodeM = tsCode;

	// set the transfer syntax
	transferSyntaxM = transferSyntax;

	// set the underlying endian-ness for the transfer
	if (tsCodeM & TS_LITTLE_ENDIAN)
	{
		setEndian(LITTLE_ENDIAN);
	}
	else
	{
		setEndian(BIG_ENDIAN);
	}
}


//>>===========================================================================

UINT16 DATA_TF_CLASS::vrToVr16(ATTR_VR_ENUM vrEnum)

//  DESCRIPTION     : Method to map the enumerated VR value into a UINT16
//					  presentation suitable for encoding.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	int	i = 0;

	// search for match
	while ((TVRMap[i].vrEnumM != vrEnum)
	  && (TVRMap[i].vrEnumM != ATTR_VR_DOESNOTEXIST))
		i++;

	// return match
	return TVRMap[i].vrUint16M;
}

//>>===========================================================================

ATTR_VR_ENUM DATA_TF_CLASS::vr16ToVr(UINT16 vrUint16)

//  DESCRIPTION     : Method to map the UINT16 VR into an enumerated VR value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	int	i = 0;

	// search for match
	while ((TVRMap[i].vrUint16M != vrUint16)
	  && (TVRMap[i].vrUint16M != UINT16_VR_DOESNOTEXIST))
		i++;

	// return match
	return TVRMap[i].vrEnumM;
}

//>>===========================================================================

char *stringVr(ATTR_VR_ENUM vrEnum)

//  DESCRIPTION     : Method to map the enumerated VR to a string equivalent.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	int		i = 0;

	// search for match
	while ((TVRMap[i].vrEnumM != vrEnum)
	  && (TVRMap[i].vrEnumM != ATTR_VR_DOESNOTEXIST))
		i++;

	// return match
	return TVRMap[i].vrStringM_ptr;
}
