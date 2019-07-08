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
//  DESCRIPTION     :	Contains general definition for the validation component.
//*****************************************************************************
#ifndef VALDEFINITIONS_H
#define VALDEFINITIONS_H

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ATTRIBUTE_RESULTS_CLASS;
class BASE_VALUE_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
const UINT VAL_OPTIONS_INITIAL            = 0x0000;
const UINT VAL_OPTIONS_STRICT             = 0x0001;
const UINT VAL_OPTIONS_EXT_REF_OBJECTS    = 0x0002;
const UINT VAL_OPTIONS_USE_REQUEST_OBJECT = 0x0004;


struct CODE_MESSAGE_STRUCT
{
    UINT32 index;
	UINT32 code;
	string message;
};

#define MAX_ERROR_LENGTH   512

#define MAX_PC_ID          256
#define MAX_TAG            0xFFFFFFFF
#define MAX_GROUP          0xFFFF
#define MAX_ELEMENT        0xFFFF

static const char	LFChar 		   = LINEFEED;
static const char	FFChar 		   = FORMFEED;
static const char	CRChar 		   = CARRIAGERETURN;
static const char	ESCChar 	   = ESCAPE;
static const char	DELChar 	   = END_G0_CHAR_SET;
static const char	UnderscoreChar = UNDERSCORE;
static const char	HyphenChar	   = HYPHEN;
static const char	PeriodChar	   = PERIOD;
static const char	WildCardAll	   = WILDCARDALL;
static const char	WildCardSingle = WILDCARDSINGLE;
static const char 	NullChar       = NULLCHAR;
static const char 	SpaceChar      = SPACECHAR;
static const char 	BackslashChar  = BACKSLASH;
static const char   CaretChar	   = CARET;
static const char   EqualChar      = EQUALCHAR;

static const int 	DaysInMonth[13] = { 0,	/* dummy */
			/* J   F   M   A   M   J   J   A   S   O   N   D */
			  31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }; 


//*****************************************************************************
//  VALIDATION UTILITY FUNCTIONS
//*****************************************************************************

bool StringValuesEqual(const char*, const char*, int, bool, bool);

#endif /* VALDEFINITIONS_H */
