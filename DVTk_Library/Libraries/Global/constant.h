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
//  DESCRIPTION     :	Global DICOM Constants and Defines.
//
//					:	This software is based on the UCDavis DICOM Transport
//					:	Library (see #include "ucdavis.h"). The software has been
//					:	modified for use in the DICOM Validation Tool.
//*****************************************************************************
#ifndef CONSTANT_H
#define CONSTANT_H

//*****************************************************************************
//  DVT VERSION -
//  Set values of IMPLEMENTATION_CLASS_UID and IMPLEMENTATION_VERSION_NAME to 
//  indicate the current DVT version.
//
// Implementation Class UID
// As mentioned on DVTk.org
#define IMPLEMENTATION_CLASS_UID				"1.2.826.0.1.3680043.2.1545.1"
//
// Implementation Version Name
// Encoded dvt<version_major>.<version_minor>
#define IMPLEMENTATION_VERSION_NAME				"dvt2.4.0"
//
//*****************************************************************************


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#pragma warning (disable: 4786)

#include "ucdavis.h"
#include "dicomtags.h"

#ifndef _WINDOWS
#define 	FILENAME_LENGTH        MAXPATHLEN
#define     PATH_SEP  '/'
#else
#define		FILENAME_LENGTH		(_MAX_PATH+1)
#define     PATH_SEP  '\\'
#endif

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define MAX_STRING_LEN		1024
#define MAX_ID_LEN			256

//*****************************************************************************
//
// Return status values
//
//*****************************************************************************
enum DVT_STATUS
{
   MSG_OK,
   MSG_ERROR,
   MSG_SMALLER,
   MSG_GREATER,
   MSG_EQUAL,
   MSG_NOT_EQUAL,
   MSG_NOT_IN_LIST,
   MSG_NOT_SET,
   MSG_NO_VALUE,
   MSG_INCOMPATIBLE,
   MSG_OUT_OF_BOUNDS,
   MSG_MEMORY_ERROR,
   MSG_INVALID_PTR,
   MSG_NOT_SAME_LEN,
   MSG_FILE_NOT_EXIST,
   MSG_LIB_NOT_EXIST,
   MSG_INVALID_PASSWORD,
   MSG_DEFINITION_NOT_FOUND,
   MSG_DEFAULT_DEFINITION_FOUND,
   MSG_PERMISSION_NOT_SUFFICIENT
};


//*****************************************************************************
//
// Filename extensions
//
//*****************************************************************************
#define DOT_DEF	".def"
#define DOT_DS	".ds"
#define DOT_DSS	".dss"
#define DOT_SES	".ses"


//*****************************************************************************
//
// Validation result prefixes
//
//*****************************************************************************
#define VAL_PREFIX_ERROR		"ERR"
#define VAL_PREFIX_WARNING		"WRN"
#define VAL_PREFIX_INFO			"INF"
#define VAL_PREFIX_RELATION		"RELATION"
#define VAL_PREFIX_LABEL		"LABEL:"
#define VAL_PREFIX_DEBUG		"DEBUG:"
#define VAL_PREFIX_PASSED		"PASSED:"
#define VAL_PREFIX_FAILED		"FAILED:"

//*****************************************************************************
//
// Association Control Service Element
//
//*****************************************************************************
//
// Socket Types
//
#define HOSTNAME							"localhost"
#define DICOM_PORT                          104		// default DICOM port
#define	SOCKET_TIMEOUT						90		// 90 seconds
#define SOCKET_BACKLOG						5


//
// ACSE PDU
//
#define COMMAND_PDV							0x01
#define DATASET_PDV							0x00
#define LAST_FRAGMENT						0x02

#define	IsThisaCmdMsg(ccc)					(ccc & COMMAND_PDV)
#define	IsThisaDataMsg(ccc)					(!(ccc & COMMAND_PDV))
#define	IsThisTheLastFragment(ccc)			(ccc & LAST_FRAGMENT)

// PDU Types
#define	PDU_ASSOCIATE_RQ					0x01
#define	PDU_ASSOCIATE_AC					0x02
#define	PDU_ASSOCIATE_RJ					0x03
#define	PDU_PDATA							0x04
#define	PDU_RELEASE_RQ						0x05
#define	PDU_RELEASE_RP						0x06
#define	PDU_ABORT_RQ						0x07
#define PDU_UNKNOWN							0xFF

// PDU Item Types
#define	ITEM_APPLICATION_CONTEXT_NAME			0x10
#define	ITEM_PRESENTATION_CONTEXT_RQ			0x20
#define	ITEM_PRESENTATION_CONTEXT_AC			0x21
#define	ITEM_ABSTRACT_SYNTAX_NAME				0x30
#define	ITEM_TRANSFER_SYNTAX_NAME				0x40
#define	ITEM_USER_INFORMATION					0x50
#define ITEM_UI_MAXIMUM_LENGTH					0x51
#define ITEM_UI_IMPLEMENTATION_CLASS_UID		0x52
#define ITEM_UI_ASYNCH_OPERATIONS_WINDOW		0x53
#define ITEM_UI_SCPSCU_ROLE_SELECTION			0x54
#define ITEM_UI_IMPLEMENTATION_VERSION_NAME		0x55
#define ITEM_UI_SOP_CLASS_EXTENDED_NEGOTIATION	0x56
#define ITEM_UI_USER_IDENTITY_NEGOTIATION		0x58

// PDU Status Codes
// Associate Accept PDU - Presentation Context - Result/Reason
#define ACCEPTANCE								0
#define USER_REJECTION							1
#define NO_REASON								2	// Provider Rejection
#define ABSTRACT_SYNTAX_NOT_SUPPORTED			3	// Provider Rejection
#define TRANSFER_SYNTAXES_NOT_SUPPORTED			4	// Provider Rejection

// Associate Reject PDU - Result
#define REJECTED_PERMANENT						1
#define REJECTED_TRANSIENT						2
#define UNDEFINED_REJECT_RESULT					0xFF

// Associate Reject PDU - Source
#define	DICOM_UL_SERVICE_USER					1
#define	DICOM_UL_SERVICE_PROVIDER_ACSE			2
#define	DICOM_UL_SERVICE_PROVIDER_PRESENTATION	3
#define UNDEFINED_REJECT_SOURCE					0xFF

// Associate Reject PDU - Reason/Diagnostic
// DICOM UL Service User
#define NO_REASON_GIVEN							1
#define APPLICATION_CONTEXT_NAME_NOT_SUPPORTED	2
#define CALLING_AE_TITLE_NOT_RECOGNISED			3
#define CALLED_AE_TITLE_NOT_RECOGNISED			7
// DICOM UL Service Provider ACSE
#define PROTOCOL_VERSION_NOT_SUPPORTED			2
// DICOM UL Service Provider Presentation
#define TEMPORARY_CONGESTION					1
#define LOCAL_LIMIT_EXCEEDED					2
#define UNDEFINED_REJECT_REASON					0xFF

// Abort PDU - Source
#define DICOM_UL_SERVICE_USER_INITIATED			0
#define DICOM_UL_SERVICE_PROVIDER_INITIATED		2
#define UNDEFINED_ABORT_SOURCE					0xFF

// Abort PDU - Reason/Diagnostic
#define REASON_NOT_SPECIFIED					0
#define UNRECOGNIZED_PDU						1
#define UNEXPECTED_PDU							2
#define UNRECOGNIZED_PDU_PARAMETER				4
#define UNEXPECTED_PDU_PARAMETER				5
#define INVALID_PDU_PARAMETER_VALUE				6
#define UNDEFINED_ABORT_REASON					0xFF

// User Information - User Identity Negotiation - User Identity Type
#define UIN_UIT_USERNAME						1
#define UIN_UIT_USERNAME_PASSCODE				2
#define UIN_UIT_KERBEROS						3

// User Information - User Identity Negotiation - Positive Reponse Requested
#define UIN_PRR_NO_RESPONSE						0
#define UIN_PRR_POSITIVE_RESPONSE				1


// PDU log length
#define PDU_LOGLENGTH							1024

// Manufacturer
#define MANUFACTURER							"DVT"

// Model Name
#define MODEL_NAME								"DVT"

// Tested By
#define TESTED_BY								"DVT"

// Application Entity Name
#define APPLICATION_ENTITY_NAME					"DICOM"

// Application Entity Version
#define APPLICATION_ENTITY_VERSION				"3.0"

// Date
#define DATE									"20060220"

// Protocol Version
#define PROTOCOL_VERSION						0x0001
#define UNDEFINED_PROTOCOL_VERSION				0xFFFF

// Called AE Title
#define CALLED_AE_TITLE							"DVT_AE"

// Calling AE Title
#define CALLING_AE_TITLE						"DVT_AE"

// Application Context Name
#define APPLICATION_CONTEXT_NAME				"1.2.840.10008.3.1.1.1"

// Maximum Length Received
#define MAXIMUM_LENGTH_RECEIVED					16384
#define MINIMUM_LENGTH_RECEIVED					512
#define INFINITE_MAXIMUM_LENGTH_RECEIVED		1048576
#define UNDEFINED_MAXIMUM_LENGTH_RECEIVED		0xFFFFFFFF
#define REASONABLE_MAXIMUM_LENGTH               0x100000000  // 4GB for now...
#define UNREASONABLE_PDU_LOG_LENGTH             0x80

// SCP & SCU Role Selection
#define SCP_ROLE_SELECT						0
#define	SCU_ROLE_SELECT						0

// Asynchronous Operations Window
#define OPERATIONS_INVOKED					1
#define OPERATIONS_PERFORMED				1

//
//
// Transfer Syntaxes
//
#define IMPLICIT_VR_LITTLE_ENDIAN						"1.2.840.10008.1.2"
#define EXPLICIT_VR_LITTLE_ENDIAN						"1.2.840.10008.1.2.1"
#define EXPLICIT_VR_BIG_ENDIAN							"1.2.840.10008.1.2.2"

#define JPEG_BASELINE_PROCESS1							"1.2.840.10008.1.2.4.50"
#define JPEG_EXTENDED_PROCESS2AND4						"1.2.840.10008.1.2.4.51"
#define JPEG_EXTENDED_PROCESS3AND5						"1.2.840.10008.1.2.4.52"
#define JPEG_SPECTRAL_PROCESS6AND8						"1.2.840.10008.1.2.4.53"
#define JPEG_SPECTRAL_PROCESS7AND9						"1.2.840.10008.1.2.4.54"
#define JPEG_FULL_PROCESS10AND12						"1.2.840.10008.1.2.4.55"
#define JPEG_FULL_PROCESS11AND13						"1.2.840.10008.1.2.4.56"
#define JPEG_LOSSLESS14									"1.2.840.10008.1.2.4.57"
#define JPEG_LOSSLESS15									"1.2.840.10008.1.2.4.58"
#define JPEG_EXTENDED_PROCESS16AND18					"1.2.840.10008.1.2.4.59"
#define JPEG_EXTENDED_PROCESS17AND19					"1.2.840.10008.1.2.4.60"
#define JPEG_SPECTRAL_PROCESS20AND22					"1.2.840.10008.1.2.4.61"
#define JPEG_SPECTRAL_PROCESS21AND23					"1.2.840.10008.1.2.4.62"
#define JPEG_FULL_PROCESS24AND26						"1.2.840.10008.1.2.4.63"
#define JPEG_FULL_PROCESS25AND27						"1.2.840.10008.1.2.4.64"
#define JPEG_LOSSLESS28									"1.2.840.10008.1.2.4.65"
#define JPEG_LOSSLESS29									"1.2.840.10008.1.2.4.66"
#define JPEG_LOSSLESS_FIRST_ORDER14						"1.2.840.10008.1.2.4.70"
#define JPEG_LS_LOSSLESS								"1.2.840.10008.1.2.4.80"
#define JPEG_LS_LOSSY									"1.2.840.10008.1.2.4.81"
#define JPEG_2000_IC_LOSSLESS_ONLY						"1.2.840.10008.1.2.4.90"
#define JPEG_2000_IC									"1.2.840.10008.1.2.4.91"
#define JPEG_2000_MULTICOMPONENT_LOSSLESS2				"1.2.840.10008.1.2.4.92"
#define JPEG_2000_MULTICOMPONENT2						"1.2.840.10008.1.2.4.93"
#define JPIP_REFERENCED									"1.2.840.10008.1.2.4.94"
#define JPIP_REFERENCED_DEFLATE							"1.2.840.10008.1.2.4.95"
#define DEFLATED_EXPLICIT_LITTLE_ENDIAN					"1.2.840.10008.1.2.1.99"
#define MPEG2_MAIN_PROFILE_LEVEL						"1.2.840.10008.1.2.4.100"
#define MPEG2_HIGH_PROFILE_LEVEL						"1.2.840.10008.1.2.4.101"
#define MPEG4_AVC_H_264_High_Profile41					"1.2.840.10008.1.2.4.102"
#define MPEG4_AVC_H_264_BD_compatible_High_Profile41	"1.2.840.10008.1.2.4.103"
#define MPEG4_AVC_H_264_High_Profile42For2D_Video		"1.2.840.10008.1.2.4.104"
#define MPEG4_AVC_H_264_High_Profile42For3D_Video		"1.2.840.10008.1.2.4.105"
#define MPEG4_AVC_H_264_Stereo_High_Profile42			"1.2.840.10008.1.2.4.106"
#define HEVC_H265_Main_Profile51						"1.2.840.10008.1.2.4.107"
#define HEVC_H265_Main_10_Profile51						"1.2.840.10008.1.2.4.108"
#define RFC_2557_MIME_ENCAPSULATION						"1.2.840.10008.1.2.6.1"
#define XML_Encoding									"1.2.840.10008.1.2.6.2"
#define RLE_LOSSLESS									"1.2.840.10008.1.2.5"


//
// Value Representations
//
#define UINT16_VR_AE	'AE'	// Application Entity
#define UINT16_VR_AS	'AS'	// Age String
#define UINT16_VR_AT	'AT'	// Attribute Tag
#define UINT16_VR_CS	'CS'	// Code String
#define UINT16_VR_DA	'DA'	// Date
#define UINT16_VR_DS	'DS'	// Decimal String
#define UINT16_VR_DT	'DT'	// Date Time
#define UINT16_VR_FL	'FL'	// Floating Point Single
#define UINT16_VR_FD	'FD'	// Floating Point Double
#define UINT16_VR_IS	'IS'	// Integer String
#define UINT16_VR_LO	'LO'	// Long String
#define UINT16_VR_LT	'LT'	// Long Text
#define UINT16_VR_OB	'OB'	// Other Byte String
#define UINT16_VR_OF	'OF'	// Other Floating Point String
#define UINT16_VR_OW	'OW'	// Other Word String
#define UINT16_VR_OL	'OL'	// Other Long String
#define UINT16_VR_OD	'OD'	// Other Double String
#define UINT16_VR_OV	'OV'	// Other Very Long String
#define UINT16_VR_PN	'PN'	// Person Name
#define UINT16_VR_SH	'SH'	// Short String
#define UINT16_VR_SL	'SL'	// Signed Long
#define UINT16_VR_SQ	'SQ'	// Sequence of Items
#define UINT16_VR_SS	'SS'	// Signed Short
#define UINT16_VR_ST	'ST'	// Short Text
#define UINT16_VR_SV	'SV'	// Signed Very Long String
#define UINT16_VR_TM	'TM'	// Time
#define UINT16_VR_UI	'UI'	// Unique Identifier
#define UINT16_VR_UL	'UL'	// Unsigned Long
#define UINT16_VR_UR	'UR'	// Universal Resource Locator
#define UINT16_VR_UC	'UC'	// Unlimited Characters
#define UINT16_VR_UN	'UN'	// Unknown Value Representation
#define UINT16_VR_US	'US'	// Unsigned Short
#define UINT16_VR_UT	'UT'	// Unlimited Text
#define UINT16_VR_UV	'UV'	// Unsigned Very Long String
#define UINT16_VR_QQ	0x3F3F	// = '??' - Older (Merge) Unknown Value Representation
#define UINT16_VR_DOESNOTEXIST	0x3F2E	// = '?.' - DVT VR DOESNOTEXIST Value Representation

//
// Value Representation sizes
//
#define UNDEFINED_LENGTH	0xFFFFFFFF		// 2**32-1
#define MAXIMUM_LENGTH		0xFFFFFFFE		// 2**32-2

#define AE_LENGTH	16
#define AS_LENGTH	4
#define AT_LENGTH	4
#define CS_LENGTH	16
#define DA_LENGTH	8		
#define DS_LENGTH	16
#define DT_LENGTH	26		
#define FL_LENGTH	4
#define FD_LENGTH	8
#define IS_LENGTH	12
#define LO_LENGTH	256     // 64 chars + esc sequences = 4*64 max
#define LT_LENGTH	40960   // 10240 chars + esc sequences = 4*10240 max
#define OB_LENGTH	UNDEFINED_LENGTH
#define OF_LENGTH	UNDEFINED_LENGTH
#define OW_LENGTH	UNDEFINED_LENGTH
#define OD_LENGTH	UNDEFINED_LENGTH
#define OL_LENGTH	UNDEFINED_LENGTH
#define OV_LENGTH	UNDEFINED_LENGTH
#define PN_LENGTH	1024    // 3 component groups of 64 displayed chars, 2nd and 3rd with esc sequences - let length be 1024 (can't define real maximum)
#define SH_LENGTH	64      // 16 chars + esc sequences = 4*16 max
#define SL_LENGTH	4
#define SQ_LENGTH	UNDEFINED_LENGTH
#define SS_LENGTH	2
#define ST_LENGTH	4096    // 1024 chars + esc sequences = 4*1024 max
#define SV_LENGTH	8    // 1024 chars + esc sequences = 4*1024 max
#define TM_LENGTH	16      
#define UI_LENGTH	64
#define UL_LENGTH	4
#define UN_LENGTH	UNDEFINED_LENGTH
#define US_LENGTH	2
#define UT_LENGTH	MAXIMUM_LENGTH
#define UR_LENGTH	MAXIMUM_LENGTH
#define UV_LENGTH	8
#define UC_LENGTH	MAXIMUM_LENGTH

#define LO_CHAR_LENGTH	64
#define LT_CHAR_LENGTH	10240
#define PN_CHAR_LENGTH	64 // per component group
#define SH_CHAR_LENGTH	16
#define ST_CHAR_LENGTH	1024

#define DA_QR_LENGTH	18		// 2 X 8 + hyphen - special for Q/R
#define TM_QR_LENGTH	28		// special for Q/R See PS 3.5 2008 Pg 30
#define DT_QR_LENGTH	54		// 2 X 26 + hyphen - special for Q/R

//
// Value Representation enumerates
//
enum ATTR_VR_ENUM
{
	ATTR_VR_AE,
	ATTR_VR_AS,
	ATTR_VR_AT,
	ATTR_VR_CS,
	ATTR_VR_DA,
	ATTR_VR_DS,
	ATTR_VR_DT,
	ATTR_VR_FL,
	ATTR_VR_FD,
    ATTR_VR_IS,
    ATTR_VR_LO,
	ATTR_VR_LT,
    ATTR_VR_OB,
	ATTR_VR_OF,
	ATTR_VR_OW,
	ATTR_VR_OL,
	ATTR_VR_OD,
	ATTR_VR_OV,
    ATTR_VR_PN,
    ATTR_VR_SH,
	ATTR_VR_SL,
	ATTR_VR_SQ,
	ATTR_VR_SS,
	ATTR_VR_ST,
	ATTR_VR_SV,
    ATTR_VR_TM,
    ATTR_VR_UI,
	ATTR_VR_UL,
	ATTR_VR_UN,
	ATTR_VR_US,
	ATTR_VR_UT,
	ATTR_VR_QQ,
	ATTR_VR_UR,
	ATTR_VR_UC,
	ATTR_VR_UV,
	ATTR_VR_DOESNOTEXIST
};


//
// Transfer Value Representation enumerates
// - details how attribute VR was actually transferred
//
enum TRANSFER_ATTR_VR_ENUM
{
	TRANSFER_ATTR_VR_IMPLICIT,
	TRANSFER_ATTR_VR_EXPLICIT,
	TRANSFER_ATTR_VR_UNKNOWN,
	TRANSFER_ATTR_VR_QUESTION
};


//
// Query / Retrieve Information Object Model enumerates
//
enum IOM_LEVEL_ENUM
{
	IOM_PATIENT,
	IOM_STUDY,
	IOM_SERIES,
	IOM_IMAGE
};


//
// Validation enumerates
//
enum VALIDATION_CONTROL_FLAG_ENUM
{
	NONE = 0,
	USE_DEFINITION = 1<<0,
	USE_VR = 1<<1,
	USE_REFERENCE = 1<<2,
	ALL = (USE_DEFINITION|USE_VR|USE_REFERENCE)
};


//
// ACSE User / Provider enumerates
//
enum UP_ENUM
{
	UP_ACCEPTOR = 0x0001,
	UP_REQUESTOR = 0x0002,
    UP_ACCEPTOR_REQUESTOR = UP_ACCEPTOR | UP_REQUESTOR
};


//
// define some special characters encountered in the VR
//
#define NULLCHAR		0x00
#define HORIZTAB		0x09
#define LINEFEED		0x0A
#define FORMFEED		0x0C
#define CARRIAGERETURN	0x0D
#define ESCAPE			0x1B
#define SPACECHAR		0x20
#define START_GL_CHAR_SET	0x20
#define OPENBRACKET		0x28
#define CLOSEBRACKET	0x29
#define WILDCARDALL		0x2A
#define HYPHEN			0x2D
#define PERIOD			0x2E
#define SLASH			0x2F
#define COLON			0x3A
#define EQUALCHAR		0x3D
#define WILDCARDSINGLE	0x3F
#define BACKSLASH		0x5C
#define CARET			0x5E
#define UNDERSCORE		0x5F
#define TILDE			0x7E
#define END_GL_CHAR_SET	0x7E
#define END_G0_CHAR_SET 0x7F
#define DELETE_CHAR		0x7F


//
// DIMSE Attribute Values
//
#define DATA_SET			(UINT16) 0x0000
#define NO_DATA_SET			(UINT16) 0x0101

#define LOW_PRIORITY		(UINT16) 0x0002
#define MEDIUM_PRIORITY		(UINT16) 0x0000
#define HIGH_PRIORITY		(UINT16) 0x0001


//
// DIMSE Group, Element, Lengths
#define COMMAND_GROUP							0x0000
#define LENGTH_ELEMENT							0x0000
#define AFFECTED_SOP_CLASS_UID					0x0002
#define REQUESTED_SOP_CLASS_UID					0x0003
#define AFFECTED_SOP_INSTANCE_UID				0x1000
#define REQUESTED_SOP_INSTANCE_UID				0x1001

#define ITEM_GROUP								0xFFFE
#define ITEM_ELEMENT							0xE000
#define ITEM_DELIMITER							0xE00D
#define SQ_DELIMITER							0xE0DD

#define GROUP_TWO								0x0002

#define GROUP_FOUR								0x0004
#define DIRECTORY_RECORD_SEQUENCE               0x1220

#define GROUP_EIGHT								0x0008
#define SPECIFIC_CHARACTER_SET					0x0005

#define GROUP_TWENTY_EIGHT						0x0028
#define SAMPLES_PER_PIXEL						0x0002
#define PLANAR_CONFIGURATION					0x0006
#define BITS_ALLOCATED							0x0100
#define PIXEL_REPRESENTATION					0x0103
#define SMALLEST_IMAGE_PIXEL_VALUE				0x0106
#define LARGEST_IMAGE_PIXEL_VALUE				0x0107
#define SMALLEST_PIXEL_VALUE_IN_SERIES			0x0108
#define LARGEST_PIXEL_VALUE_IN_SERIES			0x0109
#define SMALLEST_IMAGE_PIXEL_VALUE_IN_PLANE		0x0110
#define LARGEST_IMAGE_PIXEL_VALUE_IN_PLANE		0x0111
#define PIXEL_PADDING_VALUE						0x0120

#define REPEATING_GROUP_5000                    0x5000
#define REPEATING_GROUP_6000                    0x6000
#define REPEATING_GROUP_MASK                    0xFF01

#define PIXEL_GROUP								0x7FE0
#define PIXEL_DATA								0x0010

#define FRAME_INTERLEAVE		0x0001

#define DATASET_TRAILING_PADDING_GROUP		    0xFFFC
#define DATASET_TRAILING_PADDING_ELEMENT	    0xFFFC

#define LAST_GROUP				                0xFFFF
#define LAST_ELEMENT                            0xFFFF
#define TAG_UNDEFINED			                0xFFFFFFFF
#define TAG_UNDEFINED_GROUP                     0xFFFF
#define TAG_UNDEFINED_ELEMENT                   0xFFFF

#define	GroupFromTag(tag)						((tag >> 16) & 0x0000FFFF)
#define	ElementFromTag(tag)						(tag & 0x0000FFFF)

//
// DIMSE Messages
//
// Composite
#define C_ECHO_RQ			0x0030
#define C_ECHO_RSP			0x8030
#define C_FIND_RQ			0x0020
#define C_FIND_RSP			0x8020
#define C_GET_RQ			0x0010
#define C_GET_RSP			0x8010
#define C_MOVE_RQ			0x0021
#define C_MOVE_RSP			0x8021
#define C_STORE_RQ			0x0001
#define C_STORE_RSP			0x8001

#define C_CANCEL_RQ			0x0FFF

// Normalized
#define N_ACTION_RQ			0x0130
#define N_ACTION_RSP		0x8130
#define N_CREATE_RQ			0x0140
#define N_CREATE_RSP		0x8140
#define N_DELETE_RQ			0x0150
#define N_DELETE_RSP		0x8150
#define N_EVENT_REPORT_RQ	0x0100
#define N_EVENT_REPORT_RSP	0x8100
#define N_GET_RQ			0x0110
#define N_GET_RSP			0x8110
#define N_SET_RQ			0x0120
#define N_SET_RSP			0x8120

#define CMD_UNKNOWN			0xFFFF

//
// DIMSE Command enumerates
//
enum DIMSE_CMD_ENUM
{
    DIMSE_CMD_CECHO_RQ, DIMSE_CMD_CECHO_RSP,
    DIMSE_CMD_CFIND_RQ, DIMSE_CMD_CFIND_RSP,
    DIMSE_CMD_CGET_RQ, DIMSE_CMD_CGET_RSP,
    DIMSE_CMD_CMOVE_RQ, DIMSE_CMD_CMOVE_RSP,
    DIMSE_CMD_CSTORE_RQ, DIMSE_CMD_CSTORE_RSP,
    DIMSE_CMD_CCANCEL_RQ,
    DIMSE_CMD_NACTION_RQ, DIMSE_CMD_NACTION_RSP,
    DIMSE_CMD_NCREATE_RQ, DIMSE_CMD_NCREATE_RSP,
    DIMSE_CMD_NDELETE_RQ, DIMSE_CMD_NDELETE_RSP,
    DIMSE_CMD_NEVENTREPORT_RQ, DIMSE_CMD_NEVENTREPORT_RSP,
    DIMSE_CMD_NGET_RQ, DIMSE_CMD_NGET_RSP,
    DIMSE_CMD_NSET_RQ, DIMSE_CMD_NSET_RSP,
    DIMSE_CMD_UNKNOWN
};

//
// DICOM status codes
//
// General SOP status codes
#define DCM_STATUS_SUCCESS						0x0000
#define DCM_STATUS_NO_SUCH_ATTRIBUTE			0x0105
#define DCM_STATUS_INVALID_ATTRIBUTE_VALUE		0x0106
#define DCM_STATUS_ATTRIBUTE_LIST_ERROR			0x0107
#define DCM_STATUS_PROCESSING_FAILURE			0x0110
#define DCM_STATUS_DUPLICATE_SOPINSTANCE		0x0111
#define DCM_STATUS_NO_SUCH_OBJECT_INSTANCE		0x0112
#define DCM_STATUS_INVALID_ARGUMENT_TYPE		0x0115
#define DCM_STATUS_ATTRIBUTE_VALUE_OUT_OF_RANGE	0x0116
#define DCM_STATUS_INVALID_OBJECT_INSTANCE		0x0117
#define DCM_STATUS_CLASS_INSTANCE_CONFLICT		0x0119
#define DCM_STATUS_MISSING_ATTRIBUTE			0x0120
#define DCM_STATUS_SOP_CLASS_NOT_SUPPORTED		0x0122
#define DCM_STATUS_DUPLICATE_INVOCATION			0x0210
#define DCM_STATUS_UNRECOGNIZED_OPERATION		0x0211
#define DCM_STATUS_RESOURCE_LIMITATION			0x0213
#define DCM_STATUS_PENDING						0xFF00

// Query / Retrieve SOP status codes
#define DCM_STATUS_QUERY_FAILED			0xC001
#define DCM_STATUS_RETRIEVE_FAILED1		0xC001
#define DCM_STATUS_RETRIEVE_FAILED2		0xC002
#define DCM_STATUS_RETRIEVE_FAILED3		0xC003
#define DCM_STATUS_RETRIEVE_FAILED4		0xC004
#define DCM_STATUS_RETRIEVE_FAILED5		0xC005

// Print SOP status codes
#define DCM_STATUS_PRINT_MEMORY_ALLOCATION_NOT_SUPPORTED												0xB600
#define DCM_STATUS_PRINT_FILM_SESSION_PRINTING_IS_NOT_SUPPORTED											0xB601
#define DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES	0xB602
#define DCM_STATUS_PRINT_FILM_BOX_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_IMAGE_BOX_SOP_INSTANCES		0xB603
#define DCM_STATUS_PRINT_IMAGE_SIZE_IS_LARGER_THAN_IMAGE_BOX_SIZE										0xB604
#define DCM_STATUS_PRINT_REQUESTED_DENSITY_OUTSIDE_PRINTERS_OPERATING_RANGE								0xB605
#define DCM_STATUS_PRINT_FILM_SESSION_SOP_INSTANCE_HIERARCHY_DOES_NOT_CONTAIN_FILM_BOX_SOP_INSTANCES	0xC600
#define DCM_STATUS_PRINT_CREATION_OF_PRINT_JOB_INSTANCE_FAILED_IN_FILM_SESSION							0xC601
#define DCM_STATUS_PRINT_CREATION_OF_PRINT_JOB_INSTANCE_FAILED_IN_FILM_BOX								0xC602
#define DCM_STATUS_PRINT_IMAGE_CONTAINS_MORE_PIXELS_THAN_THE_PRINTER_CAN_PRINT_IN_THE_IMAGE_BOX			0xC603
#define DCM_STATUS_PRINT_IMAGE_POSITION_COLLISION														0xC604
#define DCM_STATUS_PRINT_INSUFFICIENT_MEMORY_IN_PRINTER_TO_STORE_IMAGES									0xC605


//
// IOD - SOP Class Names & UIDs
//
#define UNDEFINED_MAPPING						"UNDEFINED"

#define ABSTRACT_STORAGE_SOP_CLASS_NAME			"Abstract Storage SOP Class"
#define ABSTRACT_STORAGE_IOD_NAME				"Abstract Storage IOD"

#define ABSTRACT_META_SOP_CLASS_NAME			"Abstract Meta SOP Class"
#define ABSTRACT_IMAGE_BOX_SOP_CLASS_NAME		"Abstract Image Box SOP Class"
#define ABSTRACT_IMAGE_BOX_NAME					"Abstract Image Box"
#define ABSTRACT_IMAGE_BOX_SOP_CLASS_UID_STEM	"1.2.840.10008.5.1.1.4"

#define VERIFICATION_SOP_CLASS_NAME				"Verification SOP Class"
#define VTS_VERIFICATION_SOP_CLASS_NAME			"Verification"
#define VERIFICATION_SOP_CLASS_UID				"1.2.840.10008.1.1"

#define FILE_META_SOP_CLASS_NAME 		        "File Meta Information SOP Class"
#define FILE_META_SOP_CLASS_UID			        "PSEUDOFILEMETASOPUID"
#define MEDIA_STORAGE_DIRECTORY_SOP_CLASS_UID	"1.2.840.10008.1.3.10"

#define BASIC_GRAY_PRINT_META					"1.2.840.10008.5.1.1.9"
#define REFERENCED_GRAY_PRINT_META				"1.2.840.10008.5.1.1.9.1"
#define BASIC_COLOR_PRINT_META					"1.2.840.10008.5.1.1.18"
#define REFERENCED_COLOR_PRINT_META				"1.2.840.10008.5.1.1.18.1"

#define FILM_SESSION_SOP_CLASS_UID				"1.2.840.10008.5.1.1.1"
#define FILM_BOX_SOP_CLASS_UID					"1.2.840.10008.5.1.1.2"
#define GRAY_IMAGE_BOX_SOP_CLASS_UID			"1.2.840.10008.5.1.1.4"
#define COLOR_IMAGE_BOX_SOP_CLASS_UID			"1.2.840.10008.5.1.1.4.1"
#define REFERENCED_IMAGE_BOX_SOP_CLASS_UID		"1.2.840.10008.5.1.1.4.2"
#define PRINTER_SOP_CLASS_UID					"1.2.840.10008.5.1.1.16"
#define PRINTER_SOP_INSTANCE_UID				"1.2.840.10008.5.1.1.17"
#define PRINT_JOB_SOP_CLASS_UID					"1.2.840.10008.5.1.1.14"
#define ANNOTATION_BOX_SOP_CLASS_UID			"1.2.840.10008.5.1.1.15"
#define IMAGE_OVERLAY_SOP_CLASS_UID				"1.2.840.10008.5.1.1.24"
#define VOI_LUT_BOX_SOP_CLASS_UID				"1.2.840.10008.5.1.1.22"
#define PRESENTATION_LUT_SOP_CLASS_UID			"1.2.840.10008.5.1.1.23"

#define PRINT_JOB_IOD_NAME						"Print Job"

// Waveform SOP Classes
#define Waveform_Storage_SOP_CLASS_UID       "1.2.840.10008.5.1.4.1.1.9.1.1"
#define General_ECG_Waveform_Storage_SOP_CLASS_UID       "1.2.840.10008.5.1.4.1.1.9.1.2"
#define Ambulatory_ECG_Waveform_Storage_SOP_CLASS_UID    "1.2.840.10008.5.1.4.1.1.9.1.3"
#define Basic_Cardiac_ECG_Waveform_Storage_SOP_CLASS_UID "1.2.840.10008.5.1.4.1.1.9.3.1"
#define Basic_Voice_Audio_Waveform_Storage_SOP_CLASS_UID "1.2.840.10008.5.1.4.1.1.9.4.1"
#define Hemodynamic_Waveform_Storage_SOP_CLASS_UID       "1.2.840.10008.5.1.4.1.1.9.2.1"

// Query/Retrieve SOP Classes
#define PATIENT_ROOT_QR_FIND_SOP_CLASS_UID       "1.2.840.10008.5.1.4.1.2.1.1"
#define PATIENT_ROOT_QR_MOVE_SOP_CLASS_UID       "1.2.840.10008.5.1.4.1.2.1.2"
#define PATIENT_ROOT_QR_GET_SOP_CLASS_UID        "1.2.840.10008.5.1.4.1.2.1.3"

#define STUDY_ROOT_QR_FIND_SOP_CLASS_UID         "1.2.840.10008.5.1.4.1.2.2.1"
#define STUDY_ROOT_QR_MOVE_SOP_CLASS_UID         "1.2.840.10008.5.1.4.1.2.2.2"
#define STUDY_ROOT_QR_GET_SOP_CLASS_UID          "1.2.840.10008.5.1.4.1.2.2.3"

#define PATIENT_STUDY_ONLY_QR_FIND_SOP_CLASS_UID "1.2.840.10008.5.1.4.1.2.3.1"
#define PATIENT_STUDY_ONLY_QR_MOVE_SOP_CLASS_UID "1.2.840.10008.5.1.4.1.2.3.2"
#define PATIENT_STUDY_ONLY_QR_GET_SOP_CLASS_UID  "1.2.840.10008.5.1.4.1.2.3.3"

// Worklist Management SOP Class
#define MODALITY_WORKLIST_FIND_SOP_CLASS         "1.2.840.10008.5.1.4.31"
#define GENERAL_PURPOSE_WORKLIST_SOP_CLASS       "1.2.840.10008.5.1.4.32.1"

#define UNIFIED_PROCEDURE_STEP_PUSH_SOP_CLASS_UID "1.2.840.10008.5.1.4.34.4.1"
#define UNIFIED_PROCEDURE_STEP_WATCH_SOP_CLASS_UID  "1.2.840.10008.5.1.4.34.4.2"
#define UNIFIED_PROCEDURE_STEP_PULL_SOP_CLASS_UID "1.2.840.10008.5.1.4.34.4.3"
#define UNIFIED_PROCEDURE_STEP_EVENT_SOP_CLASS_UID  "1.2.840.10008.5.1.4.34.4.4"

// Storage Commitment Push Model SOP Class
#define STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID		"1.2.840.10008.1.20.1"
#define STORAGE_COMMITMENT_PUSH_MODEL_SOP_INSTANCE_UID	"1.2.840.10008.1.20.1.1"

// Modality Performed Procedure Step
#define MODALITY_PERFORMED_PROCEDURE_STEP_SOP_CLASS_UID	"1.2.840.10008.3.1.2.3.3"

//
// Media File Content Types
//
enum MEDIA_FILE_CONTENT_TYPE_ENUM
{
    MFC_MEDIAFILE,			// DICOM Media File
    MFC_COMMANDSET,			// DIMSE CommandSet
    MFC_DATASET,			// DICOM DataSet
	MFC_UNKNOWN				// Not sure what is in the file
};

//
// SCP Emulator Type
//
enum SCP_EMULATOR_ENUM
{
	SCP_EMULATOR_STORAGE,
	SCP_EMULATOR_STORAGE_COMMIT,
	SCP_EMULATOR_PRINT,
	SCP_EMULATOR_MPPS,
	SCP_EMULATOR_WORKLIST,
	SCP_EMULATOR_QUERY_RETRIEVE,
	SCP_EMULATOR_UNKNOWN
};

//
// SCU Emulator Type
//
enum SCU_EMULATOR_ENUM
{
	SCU_EMULATOR_STORAGE,
	SCU_EMULATOR_UNKNOWN
};

//
// Storage Mode for received datasets
//
enum STORAGE_MODE_ENUM
{
	SM_AS_MEDIA,				// DCM (Part 10) files and PIX files stored.
	SM_AS_MEDIA_ONLY,			// DCM (Part 10) files only stored.
	SM_AS_DATASET,				// Dataset media files and PIX files stored.
	SM_TEMPORARY_PIXEL_ONLY,	// Only PIX files stored - internal setting.
	SM_NO_STORAGE				// No files stored.
};

//
// storage file extensions
//
enum STORAGE_FILE_EXTENSION_ENUM
{
	SFE_DOT_DCM,
	SFE_DOT_RAW,
	SFE_DOT_PIX,
	SFE_DOT_RES
};

//
// "filename" used when data not stored
//
#define DATA_NOT_STORED	"DATA NOT STORED"


//
// Other Data Source
//
enum OTHER_DATA_SOURCE_ENUM
{
	DS_GENERATE,
	DS_INFILE
};

//
// Module Usage enumerates
//
enum MOD_USAGE_ENUM
{
	MOD_USAGE_M,
	MOD_USAGE_U,
	MOD_USAGE_C
};

//
// Attribute Type enumerates
//
enum ATTR_TYPE_ENUM
{
	ATTR_TYPE_1,
	ATTR_TYPE_1C,
	ATTR_TYPE_2,
	ATTR_TYPE_2C,
	ATTR_TYPE_3,
	ATTR_TYPE_3C,
	ATTR_TYPE_3R
};

//
// Attribute Value enumerates
//
enum ATTR_VAL_TYPE_ENUM
{
    ATTR_VAL_TYPE_ENUMERATED,
    ATTR_VAL_TYPE_DEFINED,
    ATTR_VAL_TYPE_ENUMERATED_LIST,
    ATTR_VAL_TYPE_DEFINED_LIST,
    ATTR_VAL_TYPE_NOVALUE
};

//
// Definition Type enumerates
//
enum DEF_TYPE_ENUM
{
	DEF_TYPE_DIMSE_ONLY,
	DEF_TYPE_DIMSE_IOD
};

//
// Attribute VM structure
//
enum ATTR_VM_RESTRICT_ENUM
{
	ATTR_VM_RESTRICT_NONE,
	ATTR_VM_RESTRICT_EVEN,
	ATTR_VM_RESTRICT_TRIPLE
};

struct ATTR_VM_STRUCT
{
    ATTR_VM_RESTRICT_ENUM ATTR_VM_RESTRICTION;
	UINT				  ATTR_VM_MIN;
	UINT	              ATTR_VM_MAX;
};

struct ATTR_IM_STRUCT
{
	UINT				  ATTR_IM_MIN;
	UINT	              ATTR_IM_MAX;
};

//define large number as maximum value multiplicity
#define MAXVM		0x10000


//
// Type of Results Object created in the validation component.
//
enum RESULTS_TYPE
{
   RESULTS_OBJECT,
   RESULTS_DICOMDIR,
   RESULTS_QR
};


//
// define some bits to describe the actual transfer syntax for
// encoding / decoding
//
#define TS_IMPLICIT_VR			0x01
#define TS_EXPLICIT_VR			0x02
#define TS_BIG_ENDIAN			0x04
#define TS_LITTLE_ENDIAN		0x08
#define TS_COMPRESSED			0x10

typedef BYTE			TS_CODE;


//
// define mapping structures
//
#define MAX_TRN_STX_NAMES	42
#define MAX_APP_CTX_NAMES	1

#define BYTE_SENTINAL	0xFF
#define CHAR_SENTINAL	""

struct T_TS_MAP
{
	char	*uid;
	char	*text;
	TS_CODE	code;
};

struct T_BYTE_TEXT_MAP
{
	BYTE	code;
	char	*text;
};

struct T_BYTE_BYTE_TEXT_MAP
{
	BYTE	code1;
	BYTE	code2;
	char	*text;
};

struct T_CHAR_TEXT_MAP
{
	char	*code;
	char	*text;
};

//required for definition lexer/parser
#define NAME_STRING_LENGTH	1024
typedef char NAME_STRING[NAME_STRING_LENGTH];

enum CMP_RESULT_ENUM
{
	CMP_RESULT_LESS,
    CMP_RESULT_EQUAL,
	CMP_RESULT_GREATER
};

const UINT32 ATTR_FLAG_NONE					= 0;
const UINT32 ATTR_FLAG_RANGES_ALLOWED       = 1 << 0;
const UINT32 ATTR_FLAG_UI_LIST_ALLOWED      = 1 << 1;
const UINT32 ATTR_FLAG_ADDITIONAL_ATTRIBUTE = 1 << 2;
const UINT32 ATTR_FLAG_DO_NOT_INCLUDE_TYPE3 = 1 << 3;
const UINT32 ATTR_FLAG_STRICT_VALIDATION    = 1 << 4;

//
// DICOMDIR Record types enumeration
//
typedef enum
{
    DICOMDIR_RECORD_TYPE_ROOT,               //special
    DICOMDIR_RECORD_TYPE_PATIENT,
    DICOMDIR_RECORD_TYPE_STUDY,
    DICOMDIR_RECORD_TYPE_SERIES,
    DICOMDIR_RECORD_TYPE_IMAGE,
	DICOMDIR_RECORD_TYPE_OVERLAY,
	DICOMDIR_RECORD_TYPE_MODALITY_LUT,
	DICOMDIR_RECORD_TYPE_VOI_LUT,
	DICOMDIR_RECORD_TYPE_CURVE,
	DICOMDIR_RECORD_TYPE_TOPIC,
	DICOMDIR_RECORD_TYPE_VISIT,
	DICOMDIR_RECORD_TYPE_RESULTS,
	DICOMDIR_RECORD_TYPE_INTERPRETATION,
    DICOMDIR_RECORD_TYPE_STUDY_COMPONENT,
	DICOMDIR_RECORD_TYPE_PRINT_QUEUE,
	DICOMDIR_RECORD_TYPE_FILM_SESSION,
    DICOMDIR_RECORD_TYPE_FILM_BOX,
	DICOMDIR_RECORD_TYPE_IMAGE_BOX,
	DICOMDIR_RECORD_TYPE_REGISTRATION,
	DICOMDIR_RECORD_TYPE_RAW_DATA,
    DICOMDIR_RECORD_TYPE_ENCAP_DOC,
	DICOMDIR_RECORD_TYPE_SPECTROSCOPY,
	DICOMDIR_RECORD_TYPE_FIDUCIAL,
	DICOMDIR_RECORD_TYPE_STORED_PRINT,
	DICOMDIR_RECORD_TYPE_RT_DOSE,
	DICOMDIR_RECORD_TYPE_RT_PLAN,
	DICOMDIR_RECORD_TYPE_RT_STRUCTURE_SET,
	DICOMDIR_RECORD_TYPE_RT_TREAT_RECORD,
	DICOMDIR_RECORD_TYPE_PRESENTATION,
	DICOMDIR_RECORD_TYPE_SR_DOCUMENT,
	DICOMDIR_RECORD_TYPE_KEY_OBJECT_DOC,
	DICOMDIR_RECORD_TYPE_WAVEFORM,
    DICOMDIR_RECORD_TYPE_PRIVATE,
	DICOMDIR_RECORD_TYPE_MRDR,
	DICOMDIR_RECORD_TYPE_HANGING_PROTOCOL,
	DICOMDIR_RECORD_TYPE_HL7_STRUC_DOC,
    DICOMDIR_RECORD_TYPE_STEREOMETRIC,
	DICOMDIR_RECORD_TYPE_VALUE_MAP,
	DICOMDIR_RECORD_TYPE_UNKNOWN
} DICOMDIR_RECORD_TYPE_ENUM;

const UINT32 DICOMDIR_ILLEGAL_OFFSET = 0xFFFFFFFF;

const UINT  MEDIA_NR_OF_FILE_PARTS      = 8;    /* Nr of parts in referenced file id's. */
const UINT  MEDIA_NR_CHARS_OF_FILEPART  = 8;    /* Nr of characters in each filename part. */

typedef enum
{
    RECORD_IOD_TYPE_IMAGE = 0, /* used as array index!! */
    RECORD_IOD_TYPE_PATIENT,
    RECORD_IOD_TYPE_PRIVATE,
    RECORD_IOD_TYPE_SERIES,
    RECORD_IOD_TYPE_STUDY,
    RECORD_IOD_TYPE_STUDY_COMPONENT,
    RECORD_IOD_TYPE_VISIT
} RECORD_IOD_TYPE_ENUM;


//
// CONDITION RESULT ENUM
//
enum CONDITION_RESULT_ENUM
{
	CONDITION_FALSE,
	CONDITION_TRUE,
	CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION,
	CONDITION_UNIMPORTANT
};

#endif /* CONSTANT_H */


