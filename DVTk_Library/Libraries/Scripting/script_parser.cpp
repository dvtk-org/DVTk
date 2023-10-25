
/*  A Bison parser, made from script_parser.y with Bison version GNU Bison version 1.24
  */

#define YYBISON 1  /* Identify Bison output.  */

#define	T_LANGUAGE	258
#define	T_RESET	259
#define	T_ALL	260
#define	T_WAREHOUSE	261
#define	T_ASSOCIATION	262
#define	T_RELATION	263
#define	T_EXECUTION_CONTEXT	264
#define	T_COMPARE	265
#define	T_COMPARE_NOT	266
#define	T_CONFIRM	267
#define	T_COPY	268
#define	T_CREATE	269
#define	T_DELAY	270
#define	T_DELETE	271
#define	T_DISPLAY	272
#define	T_ECHO	273
#define	T_POPULATE	274
#define	T_READ	275
#define	T_RECEIVE	276
#define	T_ROLE	277
#define	T_SEND	278
#define	T_SET	279
#define	T_SYSTEM	280
#define	T_TIME	281
#define	T_VALIDATE	282
#define	T_VERBOSE	283
#define	T_WRITE	284
#define	T_IMPORT	285
#define	T_EXPORT	286
#define	T_VALIDATION	287
#define	T_DEF_SQ_LENGTH	288
#define	T_ADD_GROUP_LENGTH	289
#define	T_STRICT	290
#define	T_APPL_ENTITY	291
#define	T_ASSOCIATE_RQ	292
#define	T_ASSOCIATE_AC	293
#define	T_ASSOCIATE_RJ	294
#define	T_RELEASE_RQ	295
#define	T_RELEASE_RP	296
#define	T_ABORT_RQ	297
#define	T_PROT_VER	298
#define	T_CALLED_AE	299
#define	T_CALLING_AE	300
#define	T_APPL_CTX	301
#define	T_PRES_CTX	302
#define	T_MAX_LEN	303
#define	T_IMPL_CLASS	304
#define	T_IMPL_VER	305
#define	T_SOP_EXTEND_NEG	306
#define	T_SCPSCU_ROLE	307
#define	T_ASYNC_WINDOW	308
#define	T_USER_ID_NEG	309
#define	T_RESULT	310
#define	T_SOURCE	311
#define	T_REASON	312
#define	T_DEFINED_LENGTH	313
#define	T_AUTOSET	314
#define	T_FILEHEAD	315
#define	T_FILETAIL	316
#define	T_FILE_PREAMBLE	317
#define	T_DICOM_PREFIX	318
#define	T_TRANSFER_SYNTAX	319
#define	T_DATASET_TRAILING_PADDING	320
#define	T_SECTOR_SIZE	321
#define	T_PADDING_VALUE	322
#define	T_SQ	323
#define	T_OPEN_BRACKET	324
#define	T_CLOSE_BRACKET	325
#define	T_YES	326
#define	T_NO	327
#define	T_ON	328
#define	T_OFF	329
#define	T_OR	330
#define	T_AND	331
#define	COMMANDFIELD	332
#define	HEXADECIMAL	333
#define	IDENTIFIER	334
#define	INTEGER	335
#define	VALIDATIONFLAG	336
#define	IOMLEVEL	337
#define	STRING	338
#define	USERPROVIDER	339
#define	VR	340

#line 1 "script_parser.y"

// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2006
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

//*****************************************************************************
//  DESCRIPTION     :	Script Parser
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Logger component interface
#include "Idefinition.h"	// Definition component interface
#include "Idicom.h"			// Dicom component interface
#include "Imedia.h"			// Media File component interface
#include "Isession.h"		// Test Session component interface

#ifdef _WINDOWS
#include <time.h>
#else
#include <unistd.h>
#include <sys/time.h>
#endif

#define	UNDEFINED_SCU_ROLE	256	// undefined SCU role - from VTS
#define	UNDEFINED_SCP_ROLE	256	// undefined SCP role - from VTS

#define MAX_ND	100				// maximum nesting depth

extern void scripterror(char*);
extern int scriptlex(void);
extern int scriptlineno;

extern SCRIPT_SESSION_CLASS	*scriptSession_ptr;

typedef enum
{
  OPERAND_OR,
  OPERAND_AND
} OPERAND_ENUM;


bool    scriptIsNativeVts = false;
char	scriptCurrentFilename[_MAX_PATH] = {" (not defined) "};
long	scriptCurrentFileOffset = 0;
long	scriptCurrentLineNo = 0;
bool    scriptParseOnly = false;

//*****************************************************************************
//  LOCAL DEFINITIONS
//*****************************************************************************
// local variables - unfortunately these structures are needed for YACC / LEX
static DIMSE_CMD_ENUM		commandField;
static DCM_VALUE_SQ_CLASS	*sq_ptr[MAX_ND];
static DCM_ITEM_CLASS		*item_ptr[MAX_ND];
static DCM_ATTRIBUTE_CLASS	*attribute_ptr[MAX_ND];
static BASE_VALUE_CLASS		*value_ptr = NULL;

static string					identifier;
static string					datasetidentifier;
static string					iodName;
static UINT16					group, group1, group2;
static UINT16					element, element1, element2;
static ATTR_VR_ENUM				vr;
static bool						definedLength;
static TRANSFER_ATTR_VR_ENUM	transferVr;
static bool						assocAcScuScpRolesDefined = false;
static UINT						itemNumber = 0;

static PRESENTATION_CONTEXT_RQ_CLASS	presRqContext;
static PRESENTATION_CONTEXT_AC_CLASS	presAcContext;
static BYTE								presContextId = 0;
static TRANSFER_SYNTAX_NAME_CLASS		transferSyntaxName;
static USER_INFORMATION_CLASS			userInformation;
static SOP_CLASS_EXTENDED_CLASS			sopClassExtended;
static SCP_SCU_ROLE_SELECT_CLASS		scpScuRoleSelect;


static DCM_COMMAND_CLASS				*command_ptr = NULL;
static DCM_COMMAND_CLASS				*ref_command_ptr = NULL;
static DCM_DATASET_CLASS				*dataset_ptr = NULL;
static DCM_DATASET_CLASS				*ref_dataset_ptr = NULL;
static ITEM_HANDLE_CLASS				*item_handle_ptr = NULL;
static BASE_WAREHOUSE_ITEM_DATA_CLASS	*wid1_ptr = NULL, *wid2_ptr = NULL;
static FILEHEAD_CLASS					*fileHead_ptr = NULL;
static FILETAIL_CLASS					*fileTail_ptr = NULL;

static BYTE					acseType;
static ASSOCIATE_RQ_CLASS	*associateRq_ptr = NULL;
static ASSOCIATE_AC_CLASS	*associateAc_ptr = NULL;
static ASSOCIATE_RJ_CLASS	*associateRj_ptr = NULL;
static RELEASE_RQ_CLASS		*releaseRq_ptr = NULL;
static RELEASE_RP_CLASS		*releaseRp_ptr = NULL;
static ABORT_RQ_CLASS		*abortRq_ptr = NULL;
static UNKNOWN_PDU_CLASS	*unknownPdu_ptr = NULL;

static int nd = 0; // nesting depth

extern bool compareDatasetValueWithWarehouse(LOG_CLASS*, const char*, DCM_DATASET_CLASS*);
extern bool storeObjectInWarehouse(LOG_CLASS*, const char*, BASE_WAREHOUSE_ITEM_DATA_CLASS*);
extern bool updateObjectInWarehouse(LOG_CLASS*, const char*, BASE_WAREHOUSE_ITEM_DATA_CLASS*);
extern bool removeObjectFromWarehouse(LOG_CLASS*, const char*, WID_ENUM);
extern BASE_WAREHOUSE_ITEM_DATA_CLASS *retrieveFromWarehouse(LOG_CLASS*, const char*, WID_ENUM);

extern bool displayAttribute(LOG_CLASS*, BASE_SERIALIZER*, BASE_WAREHOUSE_ITEM_DATA_CLASS*, UINT16, UINT16);
extern bool compareAttributes(LOG_CLASS*, BASE_WAREHOUSE_ITEM_DATA_CLASS*, UINT16, UINT16, BASE_WAREHOUSE_ITEM_DATA_CLASS*, UINT16, UINT16);
extern bool copyAttribute(LOG_CLASS*, BASE_WAREHOUSE_ITEM_DATA_CLASS*, UINT16, UINT16, BASE_WAREHOUSE_ITEM_DATA_CLASS*, UINT16, UINT16);

extern bool readFileDataset(SCRIPT_SESSION_CLASS *session_ptr, string, DCM_DATASET_CLASS*);
extern bool readFileDataset(SCRIPT_SESSION_CLASS *session_ptr, string, UINT32);

extern bool writeFileHead(LOG_CLASS*, string, bool);
extern bool writeFileTail(LOG_CLASS*, string, bool);
extern bool writeFileDataset(LOG_CLASS*, string, DCM_DATASET_CLASS*, bool);

extern bool receiveAcse(SCRIPT_SESSION_CLASS*, ASSOCIATE_RQ_CLASS*, string);
extern bool receiveAcse(SCRIPT_SESSION_CLASS*, ASSOCIATE_AC_CLASS*, string);
extern bool receiveAcse(SCRIPT_SESSION_CLASS*, ASSOCIATE_RJ_CLASS*, string);
extern bool receiveAcse(SCRIPT_SESSION_CLASS*, RELEASE_RQ_CLASS*, string);
extern bool receiveAcse(SCRIPT_SESSION_CLASS*, RELEASE_RP_CLASS*, string);
extern bool receiveAcse(SCRIPT_SESSION_CLASS*, ABORT_RQ_CLASS*, string);
extern bool receiveAcse(SCRIPT_SESSION_CLASS*, UNKNOWN_PDU_CLASS*, string);
extern bool receiveSop(SCRIPT_SESSION_CLASS*,  DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);

extern bool importCommand(SCRIPT_SESSION_CLASS*, DIMSE_CMD_ENUM, string);
extern bool importCommandDataset(SCRIPT_SESSION_CLASS*, DIMSE_CMD_ENUM, string, string, string);

extern bool sendAcse(SCRIPT_SESSION_CLASS*, ASSOCIATE_RQ_CLASS*, string);
extern bool sendAcse(SCRIPT_SESSION_CLASS*, ASSOCIATE_AC_CLASS*, string);
extern bool sendAcse(SCRIPT_SESSION_CLASS*, ASSOCIATE_RJ_CLASS*, string);
extern bool sendAcse(SCRIPT_SESSION_CLASS*, RELEASE_RQ_CLASS*, string);
extern bool sendAcse(SCRIPT_SESSION_CLASS*, RELEASE_RP_CLASS*, string);
extern bool sendAcse(SCRIPT_SESSION_CLASS*, ABORT_RQ_CLASS*, string);
extern bool sendAcse(SCRIPT_SESSION_CLASS*, UNKNOWN_PDU_CLASS*, string);
extern bool sendSop(SCRIPT_SESSION_CLASS*, DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);

extern bool validateSopAgainstList(SCRIPT_SESSION_CLASS*, DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
extern void setLogicalOperand(OPERAND_ENUM);
extern void addReferenceObjects(SCRIPT_SESSION_CLASS*, DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*, LOG_CLASS*);
extern void clearValidationObjects(SCRIPT_SESSION_CLASS*);

extern bool systemCall(SCRIPT_SESSION_CLASS*, char*);

extern BASE_VALUE_CLASS *stringValue(SCRIPT_SESSION_CLASS*, char*, ATTR_VR_ENUM, UINT16, UINT16);
extern BASE_VALUE_CLASS *byteArrayValue(char*);
extern BASE_VALUE_CLASS *hexValue(SCRIPT_SESSION_CLASS*, unsigned long, ATTR_VR_ENUM, UINT16, UINT16);
extern BASE_VALUE_CLASS *integerValue(SCRIPT_SESSION_CLASS*, int, ATTR_VR_ENUM, UINT16, UINT16);
extern BASE_VALUE_CLASS *autoSetValue(SCRIPT_SESSION_CLASS*, ATTR_VR_ENUM, UINT16, UINT16);

//The following may only be called for native VTS scripts!!!
extern void resolveVTSUidMappings(DCM_ATTRIBUTE_GROUP_CLASS*);
extern void clearVTSUidMappings(void);


#line 178 "script_parser.y"
typedef union {
	DIMSE_CMD_ENUM	commandField;
	unsigned long   hex;
	char			identifier[MAX_ID_LEN];
	int				integer;
	IOM_LEVEL_ENUM	iomLevel;
	char			*string_ptr;
//	char			string[MAX_STRING_LEN];
	UP_ENUM			userProvider;
	ATTR_VR_ENUM	vr;
	VALIDATION_CONTROL_FLAG_ENUM validationFlag;
} YYSTYPE;

#ifndef YYLTYPE
typedef
  struct yyltype
    {
      int timestamp;
      int first_line;
      int first_column;
      int last_line;
      int last_column;
      char *text;
   }
  yyltype;

#define YYLTYPE yyltype
#endif

#include <stdio.h>

#ifndef __cplusplus
#ifndef __STDC__
#define const
#endif
#endif



#define	YYFINAL		507
#define	YYFLAG		-32768
#define	YYNTBASE	91

#define YYTRANSLATE(x) ((unsigned)(x) <= 340 ? yytranslate[x] : 236)

static const char yytranslate[] = {     0,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,    86,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,    89,    90,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
    87,     2,    88,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     1,     2,     3,     4,     5,
     6,     7,     8,     9,    10,    11,    12,    13,    14,    15,
    16,    17,    18,    19,    20,    21,    22,    23,    24,    25,
    26,    27,    28,    29,    30,    31,    32,    33,    34,    35,
    36,    37,    38,    39,    40,    41,    42,    43,    44,    45,
    46,    47,    48,    49,    50,    51,    52,    53,    54,    55,
    56,    57,    58,    59,    60,    61,    62,    63,    64,    65,
    66,    67,    68,    69,    70,    71,    72,    73,    74,    75,
    76,    77,    78,    79,    80,    81,    82,    83,    84,    85
};

#if YYDEBUG != 0
static const short yyprhs[] = {     0,
     0,     1,     3,     5,     8,    10,    12,    14,    16,    18,
    20,    22,    24,    26,    28,    30,    32,    34,    36,    38,
    40,    42,    44,    46,    48,    50,    52,    54,    56,    58,
    60,    62,    64,    67,    70,    73,    76,    79,    82,    88,
    94,    96,   102,   105,   108,   111,   113,   116,   119,   122,
   125,   127,   129,   131,   133,   135,   137,   139,   141,   144,
   149,   156,   159,   162,   167,   170,   173,   176,   179,   182,
   185,   188,   191,   193,   196,   199,   202,   205,   209,   212,
   215,   218,   221,   224,   227,   230,   232,   234,   236,   239,
   242,   244,   246,   248,   250,   252,   254,   256,   260,   263,
   266,   269,   271,   273,   275,   277,   281,   285,   289,   291,
   293,   295,   297,   299,   302,   305,   310,   311,   313,   317,
   320,   325,   326,   328,   332,   336,   339,   342,   345,   347,
   349,   351,   353,   355,   357,   359,   361,   363,   365,   367,
   369,   371,   376,   378,   381,   384,   387,   390,   393,   396,
   399,   402,   405,   408,   411,   415,   418,   420,   423,   427,
   429,   431,   437,   449,   451,   455,   457,   459,   462,   468,
   470,   474,   476,   478,   481,   489,   493,   498,   500,   502,
   507,   509,   512,   515,   518,   521,   524,   527,   530,   533,
   536,   539,   542,   546,   548,   551,   555,   557,   559,   566,
   567,   570,   579,   580,   587,   589,   594,   596,   599,   602,
   605,   608,   610,   612,   614,   619,   621,   624,   627,   630,
   632,   634,   636,   638,   640,   642,   644,   647,   649,   652,
   654,   658,   663,   665,   668,   670,   672,   674,   676,   680,
   685,   687,   689,   692,   694,   697,   699,   701,   703,   705,
   707,   710,   716,   720,   722,   724,   726,   728,   732,   734,
   738,   740,   742,   744,   748,   750,   752,   756,   757,   759,
   761,   764,   767,   770,   771,   774,   776,   780,   784,   790,
   795,   802,   803,   805,   807,   811,   813,   815,   817,   819,
   821,   823,   825,   827,   829,   831,   833,   835,   841,   847,
   848,   852,   854,   859,   861,   863,   866,   868,   870,   872,
   875,   878,   881,   883,   888,   890,   892,   895,   897,   899,
   901,   904,   907,   910
};

static const short yyrhs[] = {    -1,
    92,     0,    93,     0,    92,    93,     0,    94,     0,    95,
     0,    96,     0,    97,     0,    98,     0,    99,     0,   100,
     0,   101,     0,   102,     0,   103,     0,   104,     0,   108,
     0,   110,     0,   111,     0,   112,     0,   113,     0,   114,
     0,   115,     0,   116,     0,   117,     0,   118,     0,   119,
     0,   120,     0,   121,     0,   122,     0,   123,     0,   124,
     0,   125,     0,     3,    83,     0,     4,     5,     0,     4,
     6,     0,     4,     7,     0,     4,     8,     0,     4,     9,
     0,    10,   218,   215,   219,   216,     0,    11,   218,   215,
   219,   216,     0,    12,     0,    13,   218,   215,   219,   216,
     0,    14,   126,     0,    15,    80,     0,    16,   127,     0,
   128,     0,    17,   218,     0,    17,     6,     0,    18,    83,
     0,    31,   105,     0,   106,     0,   107,     0,   175,     0,
   176,     0,   177,     0,   178,     0,   179,     0,   180,     0,
   185,   186,     0,   185,   186,   191,   194,     0,   185,   186,
   191,   194,    47,   132,     0,    30,   109,     0,   185,   186,
     0,   185,   186,   191,   194,     0,    19,    73,     0,    19,
    74,     0,    20,   129,     0,    21,   130,     0,    22,    84,
     0,    23,   131,     0,    24,   133,     0,    25,    83,     0,
    26,     0,    27,   134,     0,    28,    73,     0,    28,    74,
     0,    29,   139,     0,    36,    83,    83,     0,    32,    81,
     0,    33,    73,     0,    33,    74,     0,    34,    73,     0,
    34,    74,     0,    35,    73,     0,    35,    74,     0,   140,
     0,   181,     0,   190,     0,   190,    58,     0,   220,   190,
     0,   223,     0,   230,     0,   140,     0,   184,     0,   190,
     0,    60,     0,    61,     0,    17,   218,   217,     0,   128,
   217,     0,    83,   190,     0,    83,    78,     0,   141,     0,
   138,     0,   141,     0,   182,     0,   182,    47,   132,     0,
    69,    78,    70,     0,    69,    80,    70,     0,   141,     0,
   183,     0,   188,     0,   222,     0,   229,     0,   135,   136,
     0,   185,   186,     0,   185,   186,   191,   194,     0,     0,
   137,     0,   136,    75,   137,     0,   185,   186,     0,   185,
   186,   191,   194,     0,     0,   182,     0,   138,    75,   182,
     0,   138,    76,   182,     0,    83,    60,     0,    83,    61,
     0,    83,   190,     0,   175,     0,   176,     0,   177,     0,
   178,     0,   179,     0,   180,     0,   142,     0,   158,     0,
   167,     0,   170,     0,   171,     0,   172,     0,   175,     0,
   175,    69,   143,    70,     0,   144,     0,   143,   144,     0,
    43,    80,     0,    44,    83,     0,    45,    83,     0,    46,
    83,     0,    47,   145,     0,    48,    80,     0,    49,    83,
     0,    50,    83,     0,    51,   151,     0,    52,   155,     0,
    53,    80,    80,     0,    54,   157,     0,   146,     0,   145,
   146,     0,   145,    86,   146,     0,   147,     0,   148,     0,
    69,    83,    86,   149,    70,     0,    69,    80,    86,    83,
    86,    80,    86,    80,    86,   149,    70,     0,   150,     0,
   149,    86,   150,     0,    83,     0,   152,     0,   151,   152,
     0,    69,    83,    86,   153,    70,     0,   154,     0,   153,
    86,   154,     0,    80,     0,   156,     0,   155,   156,     0,
    69,    83,    86,    80,    86,    80,    70,     0,    80,    80,
    83,     0,    80,    80,    83,    83,     0,    83,     0,   176,
     0,   176,    69,   159,    70,     0,   160,     0,   159,   160,
     0,    43,    80,     0,    44,    83,     0,    45,    83,     0,
    46,    83,     0,    47,   161,     0,    48,    80,     0,    49,
    83,     0,    50,    83,     0,    51,   151,     0,    52,   155,
     0,    53,    80,    80,     0,   162,     0,   161,   162,     0,
   161,    86,   162,     0,   163,     0,   165,     0,    69,    83,
    86,    80,   164,    70,     0,     0,    86,   150,     0,    69,
    80,    86,    80,    86,    83,   166,    70,     0,     0,    86,
    80,    86,    80,    86,   150,     0,   177,     0,   177,    69,
   168,    70,     0,   169,     0,   168,   169,     0,    55,    80,
     0,    56,    80,     0,    57,    80,     0,   178,     0,   179,
     0,   180,     0,   180,    69,   173,    70,     0,   174,     0,
   173,   174,     0,    56,    80,     0,    57,    80,     0,    37,
     0,    38,     0,    39,     0,    40,     0,    41,     0,    42,
     0,   184,     0,   184,   190,     0,   183,     0,   184,   188,
     0,   184,     0,   187,    69,    70,     0,   187,    69,   195,
    70,     0,   185,     0,   185,   186,     0,    77,     0,    79,
     0,   184,     0,   190,     0,   189,    69,    70,     0,   189,
    69,   195,    70,     0,   190,     0,   191,     0,   191,   194,
     0,   193,     0,   192,   193,     0,    82,     0,    83,     0,
    79,     0,    83,     0,   196,     0,   195,   196,     0,    69,
   197,    86,   199,    70,     0,    69,   217,    70,     0,   198,
     0,    78,     0,   200,     0,   209,     0,   201,    86,   202,
     0,    68,     0,    87,    68,    88,     0,   203,     0,   205,
     0,   204,     0,   203,    86,   204,     0,    83,     0,   206,
     0,   205,    86,   206,     0,     0,   207,     0,   208,     0,
   207,   208,     0,    89,   196,     0,   210,   212,     0,     0,
   211,    86,     0,    85,     0,    87,    85,    88,     0,    69,
    85,    70,     0,    69,    87,    85,    88,    70,     0,    69,
    85,    90,    70,     0,    69,    87,    85,    88,    90,    70,
     0,     0,   213,     0,   214,     0,   213,    86,   214,     0,
    83,     0,    78,     0,    80,     0,    59,     0,   217,     0,
   217,     0,   198,     0,    83,     0,   184,     0,   190,     0,
   184,     0,   190,     0,    69,   190,   217,   221,    70,     0,
    69,   220,   217,   221,    70,     0,     0,    87,    80,    88,
     0,   223,     0,   223,    69,   224,    70,     0,    60,     0,
   225,     0,   224,   225,     0,   226,     0,   227,     0,   228,
     0,    62,    83,     0,    63,    83,     0,    64,   150,     0,
   230,     0,   230,    69,   231,    70,     0,    61,     0,   232,
     0,   231,   232,     0,   233,     0,   234,     0,   235,     0,
    65,    71,     0,    65,    72,     0,    66,    80,     0,    67,
    80,     0
};

#endif

#if YYDEBUG != 0
static const short yyrline[] = { 0,
   202,   203,   210,   217,   226,   227,   228,   229,   230,   231,
   232,   233,   234,   235,   236,   237,   238,   239,   240,   241,
   242,   243,   244,   245,   246,   247,   248,   249,   250,   251,
   252,   253,   256,   268,   294,   307,   321,   338,   355,   384,
   415,   433,   448,   451,   479,   482,   483,   559,   574,   594,
   597,   598,   601,   608,   615,   622,   629,   636,   645,   680,
   728,   782,   785,   795,   807,   824,   843,   846,   849,   886,
   889,   892,   908,   937,   940,   972,  1006,  1009,  1035,  1066,
  1083,  1102,  1119,  1138,  1155,  1174,  1213,  1263,  1289,  1310,
  1347,  1357,  1369,  1418,  1437,  1461,  1470,  1481,  1492,  1505,
  1516,  1528,  1615,  1634,  1714,  1730,  1761,  1768,  1777,  1816,
  1843,  1887,  1897,  1909,  1933,  1948,  1985,  1986,  1993,  2003,
  2018,  2056,  2074,  2081,  2089,  2099,  2109,  2119,  2131,  2132,
  2133,  2134,  2135,  2136,  2139,  2140,  2141,  2142,  2143,  2144,
  2147,  2148,  2159,  2160,  2163,  2171,  2195,  2219,  2243,  2244,
  2252,  2276,  2288,  2289,  2290,  2298,  2301,  2302,  2303,  2306,
  2307,  2310,  2357,  2426,  2434,  2444,  2482,  2483,  2486,  2528,
  2529,  2532,  2542,  2543,  2546,  2594,  2605,  2617,  2630,  2631,
  2642,  2643,  2646,  2654,  2678,  2702,  2726,  2727,  2735,  2759,
  2771,  2772,  2773,  2783,  2784,  2785,  2788,  2789,  2792,  2847,
  2855,  2858,  2928,  2936,  2957,  2958,  2961,  2962,  2965,  2973,
  2981,  2991,  2994,  2997,  2998,  3001,  3002,  3005,  3013,  3023,
  3048,  3073,  3093,  3113,  3134,  3154,  3155,  3158,  3159,  3162,
  3163,  3164,  3167,  3180,  3194,  3204,  3214,  3217,  3218,  3219,
  3222,  3225,  3251,  3274,  3278,  3283,  3290,  3310,  3318,  3332,
  3350,  3370,  3371,  3395,  3415,  3426,  3427,  3430,  3454,  3490,
  3528,  3529,  3532,  3575,  3619,  3634,  3672,  3712,  3713,  3716,
  3717,  3720,  3730,  3733,  3766,  3778,  3788,  3798,  3808,  3818,
  3828,  3840,  3841,  3844,  3871,  3900,  3938,  3946,  3954,  3963,
  3974,  3985,  3988,  4002,  4027,  4066,  4091,  4130,  4149,  4166,
  4174,  4184,  4185,  4188,  4202,  4203,  4206,  4207,  4208,  4211,
  4227,  4243,  4255,  4256,  4259,  4273,  4274,  4277,  4278,  4279,
  4282,  4292,  4304,  4316
};

static const char * const yytname[] = {   "$","error","$undefined.","T_LANGUAGE",
"T_RESET","T_ALL","T_WAREHOUSE","T_ASSOCIATION","T_RELATION","T_EXECUTION_CONTEXT",
"T_COMPARE","T_COMPARE_NOT","T_CONFIRM","T_COPY","T_CREATE","T_DELAY","T_DELETE",
"T_DISPLAY","T_ECHO","T_POPULATE","T_READ","T_RECEIVE","T_ROLE","T_SEND","T_SET",
"T_SYSTEM","T_TIME","T_VALIDATE","T_VERBOSE","T_WRITE","T_IMPORT","T_EXPORT",
"T_VALIDATION","T_DEF_SQ_LENGTH","T_ADD_GROUP_LENGTH","T_STRICT","T_APPL_ENTITY",
"T_ASSOCIATE_RQ","T_ASSOCIATE_AC","T_ASSOCIATE_RJ","T_RELEASE_RQ","T_RELEASE_RP",
"T_ABORT_RQ","T_PROT_VER","T_CALLED_AE","T_CALLING_AE","T_APPL_CTX","T_PRES_CTX",
"T_MAX_LEN","T_IMPL_CLASS","T_IMPL_VER","T_SOP_EXTEND_NEG","T_SCPSCU_ROLE","T_ASYNC_WINDOW",
"T_USER_ID_NEG","T_RESULT","T_SOURCE","T_REASON","T_DEFINED_LENGTH","T_AUTOSET",
"T_FILEHEAD","T_FILETAIL","T_FILE_PREAMBLE","T_DICOM_PREFIX","T_TRANSFER_SYNTAX",
"T_DATASET_TRAILING_PADDING","T_SECTOR_SIZE","T_PADDING_VALUE","T_SQ","T_OPEN_BRACKET",
"T_CLOSE_BRACKET","T_YES","T_NO","T_ON","T_OFF","T_OR","T_AND","COMMANDFIELD",
"HEXADECIMAL","IDENTIFIER","INTEGER","VALIDATIONFLAG","IOMLEVEL","STRING","USERPROVIDER",
"VR","','","'['","']'","'>'","'?'","Language","LanguageGrammar","LanguageComponents",
"LanguageSpecifier","ResetCommand","CompareCommand","ConfirmCommand","CopyCommand",
"CreateCommand","DelayCommand","DeleteCommand","DisplayCommand","EchoCommand",
"ExportCommand","ExportList","ExportAcseObject","ExportDimseObjects","ImportCommand",
"ImportList","PopulateCommand","ReadCommand","ReceiveCommand","RoleCommand",
"SendCommand","SetCommand","SystemCommand","TimeCommand","ValidateCommand","VerboseCommand",
"WriteCommand","ApplicationEntityFlagCommand","ValidationFlagCommand","DefineSqLengthFlagCommand",
"AddGroupLengthFlagCommand","StrictValidationFlagCommand","CreateList","DeleteList",
"DisplayTagList","ReadList","ReceiveList","SendList","PresentationContextId",
"SetList","ValidateList","SourceSopRef","ReferenceSopList","ReferenceSopRef",
"SopList","WriteList","Acse","AcseContents","AssociateRqContents","AssociateRqParameterList",
"AssociateRqParameter","AssociateRqPresCtxList","AssociateRqPresCtx","RqPresCtx",
"VtsRqPresCtx","RqTransferSyntaxList","TransferSyntax","SopClassExtendedList",
"SopClassExtended","ApplicationInfoList","ApplicationInfoByte","ScpScuRoleList",
"ScpScuRole","UserIdentityNegotiation","AssociateAcContents","AssociateAcParameterList",
"AssociateAcParameter","AssociateAcPresCtxList","AssociateAcPresCtx","AcPresCtx",
"AcTransferSyntax","VtsAcPresCtx","MoreAcPresentationContext","AssociateRjContents",
"AssociateRjParameterList","AssociateRjParameter","ReleaseRqContents","ReleaseRpContents",
"AbortRqContents","AbortRqParameterList","AbortRqParameter","AssociateRq","AssociateAc",
"AssociateRj","ReleaseRq","ReleaseRp","AbortRq","Sop","SopContents","CommandContents",
"Command","DimseCmd","CommandIdentifier","CommandRef","DatasetContents","DatasetRef",
"Dataset","IomIod","IomLevel","IodName","DatasetIdentifier","AttributeList",
"Attribute","AttributeIdentification","AttributeTag","AttributeValue","SequenceValue",
"SequenceVR","ItemList","ItemByReferenceList","ItemByReference","ItemByValueList",
"ItemByValue","ItemAttributeList","ItemAttribute","OtherValue","OptionalVR",
"AttributeVR","Values","VMList","Value","TagRef1","TagRef2","TagRef","ObjectRef1",
"ObjectRef2","SequenceRef","ItemNumber","FileheadContents","Filehead","FileheadParameterList",
"FileheadParameter","FilePreamble","DicomPrefix","FileTransferSyntax","FiletailContents",
"Filetail","FiletailParameterList","FiletailParameter","DatasetTrailingPadding",
"SectorSize","PaddingValue",""
};
#endif

static const short scriptr1[] = {     0,
    91,    91,    92,    92,    93,    93,    93,    93,    93,    93,
    93,    93,    93,    93,    93,    93,    93,    93,    93,    93,
    93,    93,    93,    93,    93,    93,    93,    93,    93,    93,
    93,    93,    94,    95,    95,    95,    95,    95,    96,    96,
    97,    98,    99,   100,   101,   102,   102,   102,   103,   104,
   105,   105,   106,   106,   106,   106,   106,   106,   107,   107,
   107,   108,   109,   109,   110,   110,   111,   112,   113,   114,
   115,   116,   117,   118,   119,   119,   120,   121,   122,   123,
   123,   124,   124,   125,   125,   126,   126,   126,   126,   126,
   126,   126,   127,   127,   127,   127,   127,   128,   128,   129,
   129,   130,   130,   131,   131,   131,   132,   132,   133,   133,
   133,   133,   133,   134,   135,   135,   136,   136,   136,   137,
   137,   138,   138,   138,   138,   139,   139,   139,   140,   140,
   140,   140,   140,   140,   141,   141,   141,   141,   141,   141,
   142,   142,   143,   143,   144,   144,   144,   144,   144,   144,
   144,   144,   144,   144,   144,   144,   145,   145,   145,   146,
   146,   147,   148,   149,   149,   150,   151,   151,   152,   153,
   153,   154,   155,   155,   156,   157,   157,   157,   158,   158,
   159,   159,   160,   160,   160,   160,   160,   160,   160,   160,
   160,   160,   160,   161,   161,   161,   162,   162,   163,   164,
   164,   165,   166,   166,   167,   167,   168,   168,   169,   169,
   169,   170,   171,   172,   172,   173,   173,   174,   174,   175,
   176,   177,   178,   179,   180,   181,   181,   182,   182,   183,
   183,   183,   184,   184,   185,   186,   187,   188,   188,   188,
   189,   190,   190,   191,   191,   192,   193,   194,   194,   195,
   195,   196,   196,   197,   198,   199,   199,   200,   201,   201,
   202,   202,   203,   203,   204,   205,   205,   206,   206,   207,
   207,   208,   209,   210,   210,   211,   211,   211,   211,   211,
   211,   212,   212,   213,   213,   214,   214,   214,   214,   215,
   216,   217,   217,   218,   218,   219,   219,   220,   220,   221,
   221,   222,   222,   223,   224,   224,   225,   225,   225,   226,
   227,   228,   229,   229,   230,   231,   231,   232,   232,   232,
   233,   233,   234,   235
};

static const short scriptr2[] = {     0,
     0,     1,     1,     2,     1,     1,     1,     1,     1,     1,
     1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
     1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
     1,     1,     2,     2,     2,     2,     2,     2,     5,     5,
     1,     5,     2,     2,     2,     1,     2,     2,     2,     2,
     1,     1,     1,     1,     1,     1,     1,     1,     2,     4,
     6,     2,     2,     4,     2,     2,     2,     2,     2,     2,
     2,     2,     1,     2,     2,     2,     2,     3,     2,     2,
     2,     2,     2,     2,     2,     1,     1,     1,     2,     2,
     1,     1,     1,     1,     1,     1,     1,     3,     2,     2,
     2,     1,     1,     1,     1,     3,     3,     3,     1,     1,
     1,     1,     1,     2,     2,     4,     0,     1,     3,     2,
     4,     0,     1,     3,     3,     2,     2,     2,     1,     1,
     1,     1,     1,     1,     1,     1,     1,     1,     1,     1,
     1,     4,     1,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     3,     2,     1,     2,     3,     1,
     1,     5,    11,     1,     3,     1,     1,     2,     5,     1,
     3,     1,     1,     2,     7,     3,     4,     1,     1,     4,
     1,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     3,     1,     2,     3,     1,     1,     6,     0,
     2,     8,     0,     6,     1,     4,     1,     2,     2,     2,
     2,     1,     1,     1,     4,     1,     2,     2,     2,     1,
     1,     1,     1,     1,     1,     1,     2,     1,     2,     1,
     3,     4,     1,     2,     1,     1,     1,     1,     3,     4,
     1,     1,     2,     1,     2,     1,     1,     1,     1,     1,
     2,     5,     3,     1,     1,     1,     1,     3,     1,     3,
     1,     1,     1,     3,     1,     1,     3,     0,     1,     1,
     2,     2,     2,     0,     2,     1,     3,     3,     5,     4,
     6,     0,     1,     1,     3,     1,     1,     1,     1,     1,
     1,     1,     1,     1,     1,     1,     1,     5,     5,     0,
     3,     1,     4,     1,     1,     2,     1,     1,     1,     2,
     2,     2,     1,     4,     1,     1,     2,     1,     1,     1,
     2,     2,     2,     2
};

static const short scriptdefact[] = {     1,
     0,     0,     0,     0,    41,     0,     0,     0,     0,     0,
     0,     0,     0,   122,     0,     0,     0,     0,    73,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     2,
     3,     5,     6,     7,     8,     9,    10,    11,    12,    13,
    14,    15,    16,    17,    18,    19,    20,    21,    22,    23,
    24,    25,    26,    27,    28,    29,    30,    31,    32,    46,
    33,    34,    35,    36,    37,    38,   235,   246,   247,   294,
   233,   295,   242,     0,   244,     0,     0,     0,   220,   221,
   222,   223,   224,   225,   304,   315,     0,    43,    86,   129,
   130,   131,   132,   133,   134,    87,   226,    88,     0,    91,
    92,    44,    96,    97,    45,    93,    94,    95,    48,    47,
    49,    65,    66,     0,    67,    68,   103,   102,   135,   136,
   137,   138,   139,   140,   141,   179,   205,   212,   213,   214,
   123,   228,   230,     0,    69,    70,   104,   105,    71,   109,
   110,   230,   111,     0,   238,   112,   302,   113,   313,    72,
    74,   117,     0,    75,    76,     0,    77,    62,     0,    50,
    51,    52,    53,    54,    55,    56,    57,    58,     0,    79,
    80,    81,    82,    83,    84,    85,     0,     4,   255,   293,
   292,    99,   236,   234,   248,   249,   243,   245,     0,   290,
     0,     0,     0,     0,   227,    89,    90,    98,   101,   100,
     0,     0,     0,     0,     0,     0,   229,     0,     0,     0,
     0,     0,   114,   118,     0,   115,   126,   127,   128,    63,
    59,    78,   296,   297,     0,     0,     0,   300,   300,   124,
   125,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,   143,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,   181,     0,     0,
     0,     0,   207,     0,     0,     0,   216,     0,   231,     0,
   250,     0,   106,   239,     0,     0,     0,     0,     0,   305,
   307,   308,   309,     0,     0,     0,     0,   316,   318,   319,
   320,     0,   120,     0,     0,     0,    39,   291,    40,    42,
     0,     0,     0,   145,   146,   147,   148,     0,   149,   157,
   160,   161,   150,   151,   152,     0,   153,   167,     0,   154,
   173,     0,     0,   178,   156,   142,   144,   183,   184,   185,
   186,     0,   187,   194,   197,   198,   188,   189,   190,   191,
   192,     0,   180,   182,   209,   210,   211,   206,   208,   218,
   219,   215,   217,     0,   254,     0,   232,   251,     0,     0,
   240,   310,   311,   166,   312,   303,   306,   321,   322,   323,
   324,   314,   317,   119,     0,   116,    64,    60,     0,   298,
   299,     0,     0,     0,   158,     0,   168,     0,   174,   155,
     0,     0,     0,     0,   195,   193,   274,   253,   107,   108,
   121,     0,   301,     0,     0,   159,     0,     0,   176,     0,
     0,   196,   259,     0,   276,     0,     0,   256,     0,   257,
   282,     0,    61,     0,     0,   164,   172,     0,   170,     0,
   177,     0,   200,     0,     0,     0,     0,   252,   268,   289,
   287,   288,   286,   273,   283,   284,   275,     0,   162,     0,
   169,     0,     0,     0,     0,     0,   278,     0,     0,   260,
   277,   265,     0,   258,   261,   263,   262,   266,   269,   270,
     0,     0,   165,   171,     0,   203,   201,   199,   280,     0,
   272,     0,   268,   271,   285,     0,   175,     0,     0,   279,
     0,   264,   267,     0,     0,   202,   281,     0,     0,     0,
     0,   163,     0,   204,     0,     0,     0
};

static const short scriptdefgoto[] = {   505,
    30,    31,    32,    33,    34,    35,    36,    37,    38,    39,
    40,    41,    42,   160,   161,   162,    43,   158,    44,    45,
    46,    47,    48,    49,    50,    51,    52,    53,    54,    55,
    56,    57,    58,    59,    88,   105,    60,   115,   116,   136,
   273,   139,   151,   152,   213,   214,   117,   157,    89,   118,
   119,   244,   245,   309,   310,   311,   312,   425,   426,   317,
   318,   428,   429,   320,   321,   325,   120,   257,   258,   333,
   334,   335,   456,   336,   489,   121,   262,   263,   122,   123,
   124,   266,   267,   125,   126,   127,   128,   129,   130,    96,
   131,   132,    70,    71,   184,   134,   143,   144,    72,    73,
    74,    75,   187,   270,   271,   354,   181,   417,   418,   419,
   464,   465,   466,   467,   468,   469,   470,   420,   421,   422,
   444,   445,   446,   189,   297,   190,    76,   225,    99,   302,
   146,   100,   279,   280,   281,   282,   283,   148,   101,   287,
   288,   289,   290,   291
};

static const short scriptpact[] = {   217,
   -50,   208,   -48,   -48,-32768,   -48,     3,   -11,    66,    10,
     6,    36,    81,    18,   109,    18,    77,   103,-32768,   127,
    47,   128,   127,    18,   144,   124,   145,   203,   187,   217,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,   -13,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
   195,-32768,    62,   197,-32768,   -13,   -13,   -13,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,    83,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,   196,   271,   196,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,   -13,
-32768,-32768,-32768,   -46,-32768,-32768,   230,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,   272,   273,   274,-32768,-32768,   275,
-32768,-32768,   102,   276,-32768,-32768,-32768,   293,-32768,-32768,
-32768,   277,-32768,   278,   279,-32768,   280,-32768,   281,-32768,
-32768,   127,   195,-32768,-32768,    -7,-32768,-32768,   195,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,   195,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,   268,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,   -48,-32768,
   -48,   -48,   -13,   -13,-32768,-32768,-32768,-32768,-32768,-32768,
   127,   127,   250,   267,   125,   251,-32768,   252,   283,   254,
   160,   201,   282,-32768,   195,   196,-32768,-32768,-32768,   196,
   196,-32768,-32768,-32768,   -13,   -13,   -13,   266,   266,-32768,
-32768,   284,   285,   286,   287,   289,   291,   290,   292,   294,
   296,   297,    19,   211,-32768,   298,   299,   300,   301,   303,
   305,   304,   306,   294,   296,   308,   239,-32768,   310,   311,
   312,   106,-32768,   313,   314,   118,-32768,   -13,-32768,   256,
-32768,   191,-32768,-32768,   258,   315,   317,   318,   132,-32768,
-32768,-32768,-32768,   259,   319,   322,   142,-32768,-32768,-32768,
-32768,   127,   196,    62,    62,    62,-32768,-32768,-32768,-32768,
   323,   309,   325,-32768,-32768,-32768,-32768,    50,     5,-32768,
-32768,-32768,-32768,-32768,-32768,   321,   294,-32768,   324,   296,
-32768,   326,   328,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,    73,    65,-32768,-32768,-32768,-32768,-32768,-32768,   294,
   296,   329,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,   269,   327,   335,-32768,-32768,   340,   341,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,    62,-32768,-32768,   307,   288,-32768,
-32768,   270,   295,   289,-32768,   330,-32768,   331,-32768,-32768,
   332,   333,   334,   303,-32768,-32768,    13,-32768,-32768,-32768,
-32768,   283,-32768,   338,   318,-32768,   342,   343,   344,   345,
   346,-32768,-32768,   188,-32768,    -6,   348,-32768,   347,-32768,
   -32,   349,-32768,   350,    -9,-32768,-32768,    26,-32768,   351,
-32768,   352,   353,   -40,   339,   354,   355,-32768,    61,-32768,
-32768,-32768,-32768,-32768,   358,-32768,-32768,   360,-32768,   318,
-32768,   342,   361,   362,   318,   359,-32768,   364,   363,-32768,
-32768,-32768,   377,-32768,   366,-32768,   367,-32768,   365,-32768,
   -32,   369,-32768,-32768,   378,   370,-32768,-32768,-32768,   -23,
-32768,   374,   365,-32768,-32768,   379,-32768,   380,   388,-32768,
   391,-32768,-32768,   376,   381,-32768,-32768,   318,   383,    72,
   382,-32768,   318,-32768,   386,   396,-32768
};

static const short scriptpgoto[] = {-32768,
-32768,   336,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
   -43,-32768,-32768,-32768,-32768,    68,-32768,-32768,   371,   316,
-32768,-32768,   117,-32768,  -290,-32768,-32768,  -136,  -277,   113,
  -291,-32768,   -78,   157,  -302,-32768,-32768,-32768,   156,-32768,
  -316,-32768,-32768,-32768,-32768,-32768,-32768,   152,-32768,-32768,
-32768,-32768,   162,    59,    64,   115,   116,   122,   148,-32768,
   -10,   413,    -2,   -20,  -131,-32768,   337,-32768,    14,  -196,
-32768,   357,  -285,   222,  -262,-32768,   179,-32768,-32768,-32768,
-32768,-32768,   -33,-32768,   -19,-32768,    -4,-32768,-32768,-32768,
-32768,-32768,   -21,   257,   110,   -58,   173,   147,   384,   237,
-32768,   452,-32768,   193,-32768,-32768,-32768,-32768,   456,-32768,
   189,-32768,-32768,-32768
};


#define	YYLAST		476


static const short yytable[] = {   153,
   365,   182,   159,   169,    97,   138,   107,   358,   376,   377,
   378,   133,   358,   133,   142,   109,   395,   389,   385,   294,
    98,   216,   108,   295,   296,   387,   440,   220,    67,   457,
   145,   199,    61,    68,    69,    68,    69,   221,   389,    79,
    80,    81,    82,    83,    84,   441,   490,   442,   387,   458,
   443,   198,   217,   218,    79,    80,    81,    82,    83,    84,
   449,   436,    85,    86,   179,    90,   491,    90,   102,   180,
    91,    87,    91,   308,    68,    69,   450,   412,   437,    67,
   413,   414,   163,   293,    68,    69,    67,   164,   111,   401,
   384,    68,    69,   406,    67,   451,   375,   415,   323,   416,
   193,   324,    79,    80,    81,    82,    83,    84,   112,   113,
   195,   452,   197,    79,    80,    81,    82,    83,    84,   154,
   155,    92,    93,    92,    93,   103,   104,   200,    94,   382,
    94,   215,   383,   332,   228,   229,    85,    86,   165,   166,
   185,   502,    67,   462,   186,   167,   145,    68,    69,   463,
   394,    87,   392,    67,    95,   393,    95,   450,    68,    69,
   259,   260,   261,   114,    68,    69,   298,   298,   298,   219,
  -237,   168,   473,   264,   265,   348,    77,   477,    78,   259,
   260,   261,   110,    68,    69,   150,   223,   352,   223,   223,
   230,   231,   135,   276,   277,   278,   171,   172,   133,   133,
   481,   366,   224,    67,   224,   224,   284,   285,   286,   356,
   156,   372,    62,    63,    64,    65,    66,   173,   174,     1,
     2,   276,   277,   278,   170,   504,     3,     4,     5,     6,
     7,     8,     9,    10,    11,    12,    13,    14,    15,    16,
    17,    18,    19,    20,    21,    22,    23,    24,    25,    26,
    27,    28,    29,   232,   233,   234,   235,   236,   237,   238,
   239,   240,   241,   242,   243,   284,   285,   286,   359,   177,
   360,   215,   434,   183,   435,   175,   176,    68,    69,    69,
   326,   246,   247,   248,   249,   250,   251,   252,   253,   254,
   255,   256,   232,   233,   234,   235,   236,   237,   238,   239,
   240,   241,   242,   243,   201,   202,   264,   265,   343,   246,
   247,   248,   249,   250,   251,   252,   253,   254,   255,   256,
   268,   269,   268,   274,   268,   357,   268,   361,   196,   368,
   369,   137,   140,   191,   192,   299,   300,   226,   227,   209,
   203,   204,   205,   206,   208,  -237,   210,  -241,   211,   212,
   222,   272,   301,   402,   397,   404,   292,   308,   423,   374,
   327,   500,   316,   304,   319,   178,   340,   305,   306,   307,
   313,   332,   314,   474,   315,   403,   322,   328,   380,   106,
   405,   329,   330,   331,   337,   506,   338,   342,   339,   345,
   346,   347,   350,   351,   381,   507,  -292,   362,   370,   363,
   364,   371,   379,   386,   398,   390,   388,   391,   396,   399,
   400,   341,   344,   349,   409,   407,   408,   438,   410,   411,
   424,   427,   430,   459,   432,   433,   431,   353,   478,   141,
   188,   275,   439,   479,   447,   448,   453,   454,   455,   472,
   475,   460,   461,   471,   476,   268,   355,   487,   492,   485,
   480,   482,   483,   463,   486,   488,   462,   496,   494,   495,
   497,   498,   501,   493,   484,   303,   499,   503,   147,   207,
   194,   367,   149,     0,     0,   373
};

static const short yycheck[] = {    20,
   278,    60,    23,    24,     7,    16,     9,   270,   294,   295,
   296,    14,   275,    16,    17,     6,   333,   320,   309,   216,
     7,   153,     9,   220,   221,   317,    59,   159,    77,    70,
    17,    78,    83,    82,    83,    82,    83,   169,   341,    37,
    38,    39,    40,    41,    42,    78,    70,    80,   340,    90,
    83,   110,    60,    61,    37,    38,    39,    40,    41,    42,
    70,    68,    60,    61,    78,     7,    90,     9,    80,    83,
     7,    69,     9,    69,    82,    83,    86,   394,    85,    77,
    68,    69,    24,   215,    82,    83,    77,    24,    83,   375,
    86,    82,    83,   384,    77,    70,   293,    85,    80,    87,
    87,    83,    37,    38,    39,    40,    41,    42,    73,    74,
    97,    86,    99,    37,    38,    39,    40,    41,    42,    73,
    74,     7,     7,     9,     9,    60,    61,   114,     7,    80,
     9,   152,    83,    69,   193,   194,    60,    61,    24,    24,
    79,    70,    77,    83,    83,    24,   133,    82,    83,    89,
    86,    69,    80,    77,     7,    83,     9,    86,    82,    83,
    55,    56,    57,    83,    82,    83,   225,   226,   227,   156,
    69,    24,   450,    56,    57,    70,     4,   455,     6,    55,
    56,    57,    10,    82,    83,    83,   189,    70,   191,   192,
   201,   202,    84,    62,    63,    64,    73,    74,   201,   202,
   463,    70,   189,    77,   191,   192,    65,    66,    67,   268,
    83,    70,     5,     6,     7,     8,     9,    73,    74,     3,
     4,    62,    63,    64,    81,   503,    10,    11,    12,    13,
    14,    15,    16,    17,    18,    19,    20,    21,    22,    23,
    24,    25,    26,    27,    28,    29,    30,    31,    32,    33,
    34,    35,    36,    43,    44,    45,    46,    47,    48,    49,
    50,    51,    52,    53,    54,    65,    66,    67,    78,    83,
    80,   292,    85,    79,    87,    73,    74,    82,    83,    83,
    70,    43,    44,    45,    46,    47,    48,    49,    50,    51,
    52,    53,    43,    44,    45,    46,    47,    48,    49,    50,
    51,    52,    53,    54,    75,    76,    56,    57,    70,    43,
    44,    45,    46,    47,    48,    49,    50,    51,    52,    53,
    69,    70,    69,    70,    69,    70,    69,    70,    58,    71,
    72,    16,    17,    77,    78,   226,   227,   191,   192,    47,
    69,    69,    69,    69,    69,    69,    69,    69,    69,    69,
    83,    69,    87,    47,    86,    86,    75,    69,   402,   292,
   244,   498,    69,    80,    69,    30,   254,    83,    83,    83,
    80,    69,    83,   452,    83,    88,    80,    80,    70,     9,
    86,    83,    83,    83,    80,     0,    83,    80,    83,    80,
    80,    80,    80,    80,    70,     0,    70,    83,    80,    83,
    83,    80,    80,    83,    70,    80,    83,    80,    80,    70,
    70,   255,   257,   262,    83,    86,    86,    70,    86,    86,
    83,    80,    80,    85,    80,    80,    83,   266,    70,    17,
    74,   210,    86,    70,    86,    86,    86,    86,    86,    80,
    80,    88,    88,    86,    83,    69,   268,    70,   482,   471,
    88,    86,    86,    89,    86,    86,    83,    70,    80,    80,
    70,    86,    80,   483,   469,   229,    86,    86,    17,   133,
    87,   279,    17,    -1,    -1,   287
};
/* -*-C-*-  Note some compilers choke on comments on `#line' lines.  */
#line 3 "bison.simple"

/* Skeleton output parser for bison,
   Copyright (C) 1984, 1989, 1990 Free Software Foundation, Inc.

   This program is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2, or (at your option)
   any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.  */

/* As a special exception, when this file is copied by Bison into a
   Bison output file, you may use that output file without restriction.
   This special exception was added by the Free Software Foundation
   in version 1.24 of Bison.  */

#ifndef alloca
#ifdef __GNUC__
#define alloca __builtin_alloca
#else /* not GNU C.  */
#if (!defined (__STDC__) && defined (sparc)) || defined (__sparc__) || defined (__sparc) || defined (__sgi)
#include <alloca.h>
#else /* not sparc */
#if defined (MSDOS) && !defined (__TURBOC__)
#include <malloc.h>
#else /* not MSDOS, or __TURBOC__ */
#if defined(_AIX)
#include <malloc.h>
 #pragma alloca
#else /* not MSDOS, __TURBOC__, or _AIX */
#ifdef __hpux
#ifdef __cplusplus
extern "C" {
void *alloca (unsigned int);
};
#else /* not __cplusplus */
void *alloca ();
#endif /* not __cplusplus */
#endif /* __hpux */
#endif /* not _AIX */
#endif /* not MSDOS, or __TURBOC__ */
#endif /* not sparc.  */
#endif /* not GNU C.  */
#endif /* alloca not defined.  */

/* This is the parser code that is written into each bison parser
  when the %semantic_parser declaration is not specified in the grammar.
  It was written by Richard Stallman by simplifying the hairy parser
  used when %semantic_parser is specified.  */

/* Note: there must be only one dollar sign in this file.
   It is replaced by the list of actions, each action
   as one case of the switch.  */

#define scripterrok		(yyerrstatus = 0)
#define scriptclearin	(scriptchar = YYEMPTY)
#define YYEMPTY		-2
#define YYEOF		0
#define YYACCEPT	return(0)
#define YYABORT 	return(1)
#define YYERROR		goto scripterrlab1
/* Like YYERROR except do call scripterror.
   This remains here temporarily to ease the
   transition to the new meaning of YYERROR, for GCC.
   Once GCC version 2 has supplanted version 1, this can go.  */
#define YYFAIL		goto scripterrlab
#define YYRECOVERING()  (!!yyerrstatus)
#define YYBACKUP(token, value) \
do								\
  if (scriptchar == YYEMPTY && yylen == 1)				\
    { scriptchar = (token), scriptlval = (value);			\
      scriptchar1 = YYTRANSLATE (scriptchar);				\
      YYPOPSTACK;						\
      goto scriptbackup;						\
    }								\
  else								\
    { scripterror ("syntax error: cannot back up"); YYERROR; }	\
while (0)

#define YYTERROR	1
#define YYERRCODE	256

#ifndef YYPURE
#define YYLEX		scriptlex()
#endif

#ifdef YYPURE
#ifdef YYLSP_NEEDED
#ifdef YYLEX_PARAM
#define YYLEX		scriptlex(&scriptlval, &yylloc, YYLEX_PARAM)
#else
#define YYLEX		scriptlex(&scriptlval, &yylloc)
#endif
#else /* not YYLSP_NEEDED */
#ifdef YYLEX_PARAM
#define YYLEX		scriptlex(&scriptlval, YYLEX_PARAM)
#else
#define YYLEX		scriptlex(&scriptlval)
#endif
#endif /* not YYLSP_NEEDED */
#endif

/* If nonreentrant, generate the variables here */

#ifndef YYPURE

int	scriptchar;			/*  the lookahead symbol		*/
YYSTYPE	scriptlval;			/*  the semantic value of the		*/
				/*  lookahead symbol			*/

#ifdef YYLSP_NEEDED
YYLTYPE yylloc;			/*  location data for the lookahead	*/
				/*  symbol				*/
#endif

int scriptnerrs;			/*  number of parse errors so far       */
#endif  /* not YYPURE */

#if YYDEBUG != 0
int scriptdebug;			/*  nonzero means print parse trace	*/
/* Since this is uninitialized, it does not stop multiple parsers
   from coexisting.  */
#endif

/*  YYINITDEPTH indicates the initial size of the parser's stacks	*/

#ifndef	YYINITDEPTH
#define YYINITDEPTH 200
#endif

/*  YYMAXDEPTH is the maximum size the stacks can grow to
    (effective only if the built-in stack extension method is used).  */

#if YYMAXDEPTH == 0
#undef YYMAXDEPTH
#endif

#ifndef YYMAXDEPTH
#define YYMAXDEPTH 10000
#endif

/* Prevent warning if -Wstrict-prototypes.  */
#ifdef __GNUC__
int scriptparse (void);
#endif

#if __GNUC__ > 1		/* GNU C and GNU C++ define this.  */
#define __yy_memcpy(FROM,TO,COUNT)	__builtin_memcpy(TO,FROM,COUNT)
#else				/* not GNU C or C++ */
#ifndef __cplusplus

/* This is the most reliable way to avoid incompatibilities
   in available built-in functions on various systems.  */
static void
__yy_memcpy (from, to, count)
     char *from;
     char *to;
     int count;
{
  register char *f = from;
  register char *t = to;
  register int i = count;

  while (i-- > 0)
    *t++ = *f++;
}

#else /* __cplusplus */

/* This is the most reliable way to avoid incompatibilities
   in available built-in functions on various systems.  */
static void
__yy_memcpy (char *from, char *to, int count)
{
  register char *f = from;
  register char *t = to;
  register int i = count;

  while (i-- > 0)
    *t++ = *f++;
}

#endif
#endif

#line 192 "bison.simple"

/* The user can define YYPARSE_PARAM as the name of an argument to be passed
   into scriptparse.  The argument should have type void *.
   It should actually point to an object.
   Grammar actions can access the variable by casting it
   to the proper pointer type.  */

#ifdef YYPARSE_PARAM
#define YYPARSE_PARAM_DECL void *YYPARSE_PARAM;
#else
#define YYPARSE_PARAM
#define YYPARSE_PARAM_DECL
#endif

int
scriptparse(YYPARSE_PARAM)
     YYPARSE_PARAM_DECL
{
  register int scriptstate;
  register int yyn;
  register short *scriptssp;
  register YYSTYPE *scriptvsp;
  int yyerrstatus;	/*  number of tokens to shift before error messages enabled */
  int scriptchar1 = 0;		/*  lookahead token as an internal (translated) token number */

  short	scriptssa[YYINITDEPTH];	/*  the state stack			*/
  YYSTYPE scriptvsa[YYINITDEPTH];	/*  the semantic value stack		*/

  short *scriptss = scriptssa;		/*  refer to the stacks thru separate pointers */
  YYSTYPE *scriptvs = scriptvsa;	/*  to allow yyoverflow to reallocate them elsewhere */

#ifdef YYLSP_NEEDED
  YYLTYPE yylsa[YYINITDEPTH];	/*  the location stack			*/
  YYLTYPE *yyls = yylsa;
  YYLTYPE *scriptlsp;

#define YYPOPSTACK   (scriptvsp--, scriptssp--, scriptlsp--)
#else
#define YYPOPSTACK   (scriptvsp--, scriptssp--)
#endif

  int scriptstacksize = YYINITDEPTH;

#ifdef YYPURE
  int scriptchar;
  YYSTYPE scriptlval;
  int scriptnerrs;
#ifdef YYLSP_NEEDED
  YYLTYPE yylloc;
#endif
#endif

  YYSTYPE scriptval;		/*  the variable used to return		*/
				/*  semantic values from the action	*/
				/*  routines				*/

  int yylen;

#if YYDEBUG != 0
  if (scriptdebug)
    fprintf(stderr, "Starting parse\n");
#endif

  scriptstate = 0;
  yyerrstatus = 0;
  scriptnerrs = 0;
  scriptchar = YYEMPTY;		/* Cause a token to be read.  */

  /* Initialize stack pointers.
     Waste one element of value and location stack
     so that they stay on the same level as the state stack.
     The wasted elements are never initialized.  */

  scriptssp = scriptss - 1;
  scriptvsp = scriptvs;
#ifdef YYLSP_NEEDED
  scriptlsp = yyls;
#endif

/* Push a new state, which is found in  scriptstate  .  */
/* In all cases, when you get here, the value and location stacks
   have just been pushed. so pushing a state here evens the stacks.  */
yynewstate:

  *++scriptssp = scriptstate;

  if (scriptssp >= scriptss + scriptstacksize - 1)
    {
      /* Give user a chance to reallocate the stack */
      /* Use copies of these so that the &'s don't force the real ones into memory. */
      YYSTYPE *scriptvs1 = scriptvs;
      short *scriptss1 = scriptss;
#ifdef YYLSP_NEEDED
      YYLTYPE *yyls1 = yyls;
#endif

      /* Get the current used size of the three stacks, in elements.  */
      int size = scriptssp - scriptss + 1;

#ifdef yyoverflow
      /* Each stack pointer address is followed by the size of
	 the data in use in that stack, in bytes.  */
#ifdef YYLSP_NEEDED
      /* This used to be a conditional around just the two extra args,
	 but that might be undefined if yyoverflow is a macro.  */
      yyoverflow("parser stack overflow",
		 &scriptss1, size * sizeof (*scriptssp),
		 &scriptvs1, size * sizeof (*scriptvsp),
		 &yyls1, size * sizeof (*scriptlsp),
		 &scriptstacksize);
#else
      yyoverflow("parser stack overflow",
		 &scriptss1, size * sizeof (*scriptssp),
		 &scriptvs1, size * sizeof (*scriptvsp),
		 &scriptstacksize);
#endif

      scriptss = scriptss1; scriptvs = scriptvs1;
#ifdef YYLSP_NEEDED
      yyls = yyls1;
#endif
#else /* no yyoverflow */
      /* Extend the stack our own way.  */
      if (scriptstacksize >= YYMAXDEPTH)
	{
	  scripterror("parser stack overflow");
	  return 2;
	}
      scriptstacksize *= 2;
      if (scriptstacksize > YYMAXDEPTH)
	scriptstacksize = YYMAXDEPTH;
      scriptss = (short *) alloca (scriptstacksize * sizeof (*scriptssp));
      __yy_memcpy ((char *)scriptss1, (char *)scriptss, size * sizeof (*scriptssp));
      scriptvs = (YYSTYPE *) alloca (scriptstacksize * sizeof (*scriptvsp));
      __yy_memcpy ((char *)scriptvs1, (char *)scriptvs, size * sizeof (*scriptvsp));
#ifdef YYLSP_NEEDED
      yyls = (YYLTYPE *) alloca (scriptstacksize * sizeof (*scriptlsp));
      __yy_memcpy ((char *)yyls1, (char *)yyls, size * sizeof (*scriptlsp));
#endif
#endif /* no yyoverflow */

      scriptssp = scriptss + size - 1;
      scriptvsp = scriptvs + size - 1;
#ifdef YYLSP_NEEDED
      scriptlsp = yyls + size - 1;
#endif

#if YYDEBUG != 0
      if (scriptdebug)
	fprintf(stderr, "Stack size increased to %d\n", scriptstacksize);
#endif

      if (scriptssp >= scriptss + scriptstacksize - 1)
	YYABORT;
    }

#if YYDEBUG != 0
  if (scriptdebug)
    fprintf(stderr, "Entering state %d\n", scriptstate);
#endif

  goto scriptbackup;
 scriptbackup:

/* Do appropriate processing given the current state.  */
/* Read a lookahead token if we need one and don't already have one.  */
/* yyresume: */

  /* First try to decide what to do without reference to lookahead token.  */

  yyn = scriptpact[scriptstate];
  if (yyn == YYFLAG)
    goto scriptdefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* scriptchar is either YYEMPTY or YYEOF
     or a valid token in external form.  */

  if (scriptchar == YYEMPTY)
    {
#if YYDEBUG != 0
      if (scriptdebug)
	fprintf(stderr, "Reading a token: ");
#endif
      scriptchar = YYLEX;
    }

  /* Convert token to internal form (in scriptchar1) for indexing tables with */

  if (scriptchar <= 0)		/* This means end of input. */
    {
      scriptchar1 = 0;
      scriptchar = YYEOF;		/* Don't call YYLEX any more */

#if YYDEBUG != 0
      if (scriptdebug)
	fprintf(stderr, "Now at end of input.\n");
#endif
    }
  else
    {
      scriptchar1 = YYTRANSLATE(scriptchar);

#if YYDEBUG != 0
      if (scriptdebug)
	{
	  fprintf (stderr, "Next token is %d (%s", scriptchar, yytname[scriptchar1]);
	  /* Give the individual parser a way to print the precise meaning
	     of a token, for further debugging info.  */
#ifdef YYPRINT
	  YYPRINT (stderr, scriptchar, scriptlval);
#endif
	  fprintf (stderr, ")\n");
	}
#endif
    }

  yyn += scriptchar1;
  if (yyn < 0 || yyn > YYLAST || yycheck[yyn] != scriptchar1)
    goto scriptdefault;

  yyn = yytable[yyn];

  /* yyn is what to do for this token type in this state.
     Negative => reduce, -yyn is rule number.
     Positive => shift, yyn is new state.
       New state is final state => don't bother to shift,
       just return success.
     0, or most negative number => error.  */

  if (yyn < 0)
    {
      if (yyn == YYFLAG)
	goto scripterrlab;
      yyn = -yyn;
      goto yyreduce;
    }
  else if (yyn == 0)
    goto scripterrlab;

  if (yyn == YYFINAL)
    YYACCEPT;

  /* Shift the lookahead token.  */

#if YYDEBUG != 0
  if (scriptdebug)
    fprintf(stderr, "Shifting token %d (%s), ", scriptchar, yytname[scriptchar1]);
#endif

  /* Discard the token being shifted unless it is eof.  */
  if (scriptchar != YYEOF)
    scriptchar = YYEMPTY;

  *++scriptvsp = scriptlval;
#ifdef YYLSP_NEEDED
  *++scriptlsp = yylloc;
#endif

  /* count tokens shifted since error; after three, turn off error status.  */
  if (yyerrstatus) yyerrstatus--;

  scriptstate = yyn;
  goto yynewstate;

/* Do the default action for the current state.  */
scriptdefault:

  yyn = scriptdefact[scriptstate];
  if (yyn == 0)
    goto scripterrlab;

/* Do a reduction.  yyn is the number of a rule to reduce with.  */
yyreduce:
  yylen = scriptr2[yyn];
  if (yylen > 0)
    scriptval = scriptvsp[1-yylen]; /* implement default value of the action */

#if YYDEBUG != 0
  if (scriptdebug)
    {
      int i;

      fprintf (stderr, "Reducing via rule %d (line %d), ",
	       yyn, yyrline[yyn]);

      /* Print the symbols being reduced, and their result.  */
      for (i = yyprhs[yyn]; yyrhs[i] > 0; i++)
	fprintf (stderr, "%s ", yytname[yyrhs[i]]);
      fprintf (stderr, " -> %s\n", yytname[scriptr1[yyn]]);
    }
#endif


  switch (yyn) {

case 2:
#line 204 "script_parser.y"
{
	      //cleanup
		  clearVTSUidMappings();	
	  ;
    break;}
case 3:
#line 211 "script_parser.y"
{
		if (scriptSession_ptr->isSessionStopped())
		{
			YYACCEPT;
		}
	;
    break;}
case 4:
#line 218 "script_parser.y"
{
		if (scriptSession_ptr->isSessionStopped())
		{
			YYACCEPT;
		}
	;
    break;}
case 33:
#line 257 "script_parser.y"
{
		if (strcmp(scriptvsp[0].string_ptr, "NATIVE_VTS") == 0)
		{
			scriptIsNativeVts = true;
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 34:
#line 269 "script_parser.y"
{
		// data warehouse should be emptied
		// and association reset
		// - backwards compatibility with VTS
		LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

		// log action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "RESET ALL");
			logger_ptr->text(LOG_DEBUG, 1, "Warehouse content has been deleted.");
			logger_ptr->text(LOG_DEBUG, 1, "Association has been reset.");
			logger_ptr->text(LOG_DEBUG, 1, "Object(Image) Relationship has been reset.");
			logger_ptr->text(LOG_DEBUG, 1, "Script Execution Context has been reset.");
		}
		WAREHOUSE->empty();
		scriptSession_ptr->resetAssociation();

		// cleanup any outstanding relationships
		// - from previous emulations / script executions
		RELATIONSHIP->cleanup();

		// reset the script execution context
		scriptSession_ptr->resetScriptExecutionContext();
	;
    break;}
case 35:
#line 295 "script_parser.y"
{
		// data warehouse should be emptied
		LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

		// log action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "RESET WAREHOUSE");
			logger_ptr->text(LOG_DEBUG, 1, "Warehouse content has been deleted.");
		}
		WAREHOUSE->empty();
	;
    break;}
case 36:
#line 308 "script_parser.y"
{
		// association reset
		// - backwards compatibility with VTS
		LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

		// log action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "RESET ASSOCIATION");
			logger_ptr->text(LOG_DEBUG, 1, "Association has been reset.");
		}
		scriptSession_ptr->resetAssociation();
	;
    break;}
case 37:
#line 322 "script_parser.y"
{
		// relation reset
		// - backwards compatibility with VTS
		LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

		// log action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "RESET RELATION");
			logger_ptr->text(LOG_DEBUG, 1, "Object(Image) Relationship has been reset.");
		}

		// cleanup any outstanding relationships
		// - from previous emulations / script executions
		RELATIONSHIP->cleanup();
	;
    break;}
case 38:
#line 339 "script_parser.y"
{
		// script execution context reset
		LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

		// log action
		if (logger_ptr)
		{
			logger_ptr->text(LOG_SCRIPT, 2, "RESET SCRIPT-EXECUTION-CONTEXT");
			logger_ptr->text(LOG_DEBUG, 1, "Script Execution Context has been reset.");
		}
		
		// reset the script execution context
		scriptSession_ptr->resetScriptExecutionContext();
	;
    break;}
case 39:
#line 356 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if ((wid1_ptr) && 
				(wid2_ptr))
			{
				// compare the attribute in object 1 with that in object 2 - expect that they are the same
				if (!compareAttributes(logger_ptr, wid1_ptr, group1, element1, wid2_ptr, group2, element2))
				{
					// attributes are different
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Compared attributes should be equal but are not: (%04X,%04X) != (%04X,%04X)", group1, element1, group2, element2);
					}
				}
				else
				{
					// attributes are the same
					if (logger_ptr)
					{
						logger_ptr->text(LOG_INFO, 1, "Compared attributes are equal: (%04X,%04X) == (%04X,%04X)", group1, element1, group2, element2);
					}
				}
			}
		}
	;
    break;}
case 40:
#line 385 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if ((wid1_ptr) &&
				(wid2_ptr))
			{
				// compare the attribute in object 1 with that in object 2 - expect that they are different
				if (compareAttributes(logger_ptr, wid1_ptr, group1, element1, wid2_ptr, group2, element2))
				{
					// attributes are different
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Compared attributes should not be equal but are: (%04X,%04X) == (%04X,%04X)", group1, element1, group2, element2);
					}
				}
				else
				{
					// attributes are the same
					if (logger_ptr)
					{
						logger_ptr->text(LOG_INFO, 1, "Compared attributes are not equal: (%04X,%04X) != (%04X,%04X)", group1, element1, group2, element2);
					}
				}
			}
		}
	;
    break;}
case 41:
#line 416 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			BASE_CONFIRMER *confirmer_ptr = scriptSession_ptr->getConfirmer();
			
			// ask user to confirm action through interaction with logger and confirmer
			if ((logger_ptr) &&
				(confirmer_ptr))
			{
				logger_ptr->text(LOG_SCRIPT, 2, "CONFIRM");
				confirmer_ptr->ConfirmInteraction();
			}
		}
	;
    break;}
case 42:
#line 434 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if ((wid1_ptr) &&
				(wid2_ptr))
			{
				// copy the attribute in object 1 to that in object 2
				// - don't stop on returned error
				copyAttribute(scriptSession_ptr->getLogger(), wid1_ptr, group1, element1, wid2_ptr, group2, element2);
			}
		}
	;
    break;}
case 44:
#line 452 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			// delay for given number of seconds
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "DELAY %d seconds", scriptvsp[0].integer);
			}

			for (int sec = 0; sec < scriptvsp[0].integer; sec++)
			{
				if (scriptSession_ptr->isSessionStopped())
				{
					YYACCEPT;
				}
#ifdef _WINDOWS
				Sleep(1000);
#else
				sleep(1);
#endif
			}
		}
	;
    break;}
case 47:
#line 484 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  		
			if (wid1_ptr)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

				if (logger_ptr)
				{
					logger_ptr->text(LOG_SCRIPT, 2, "DISPLAY object");
				}

				// display the referenced warehouse object
				wid1_ptr->setLogger(scriptSession_ptr->getLogger());

				BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
				if (serializer_ptr)
				{
					switch(wid1_ptr->getWidType())
					{
					case WID_C_ECHO_RQ:
					case WID_C_ECHO_RSP:
					case WID_C_FIND_RQ:
					case WID_C_FIND_RSP:
					case WID_C_GET_RQ:
					case WID_C_GET_RSP:
					case WID_C_MOVE_RQ:
					case WID_C_MOVE_RSP:
					case WID_C_STORE_RQ:
					case WID_C_STORE_RSP:
					case WID_C_CANCEL_RQ:
					case WID_N_ACTION_RQ:
					case WID_N_ACTION_RSP:
					case WID_N_CREATE_RQ:
					case WID_N_CREATE_RSP:
					case WID_N_DELETE_RQ:
					case WID_N_DELETE_RSP:
					case WID_N_EVENT_REPORT_RQ:
					case WID_N_EVENT_REPORT_RSP:
					case WID_N_GET_RQ:
					case WID_N_GET_RSP:
					case WID_N_SET_RQ:
					case WID_N_SET_RSP:
						{
							// retrieve the command from the warehouse
							DCM_COMMAND_CLASS *command_ptr = static_cast<DCM_COMMAND_CLASS*>(wid1_ptr);
					
							// serialize it
							serializer_ptr->SerializeDisplay(command_ptr, NULL);
						}
						break;
					case WID_DATASET:
						{
							// retrieve the dataset from the warehouse
							DCM_DATASET_CLASS *dataset_ptr = static_cast<DCM_DATASET_CLASS*>(wid1_ptr);
					
							// serialize it
							serializer_ptr->SerializeDisplay(dataset_ptr);
						}
						break;
				    case WID_ITEM:
						{
						 	// retrieve the item from the warehouse
						    DCM_ITEM_CLASS *item_ptr = static_cast<DCM_ITEM_CLASS*>(wid1_ptr);
	
						    // serialize it
//						    serializer_ptr->SerializeDisplay(item_ptr);
						}
					break;	
					default: break;
					}
				}
			}
		}
	;
    break;}
case 48:
#line 560 "script_parser.y"
{
		if (!scriptParseOnly)
		{
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "DISPLAY WAREHOUSE");
			}
			WAREHOUSE->serialize(scriptSession_ptr->getLogger());
		}
	;
    break;}
case 49:
#line 575 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if (logger_ptr)
			{
				// display string to user
				logger_ptr->text(LOG_SCRIPT, 1, scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;

	;
    break;}
case 53:
#line 602 "script_parser.y"
{
        if (!scriptParseOnly)
		{
		    if (!sendAcse(scriptSession_ptr, associateRq_ptr, identifier)) YYABORT;
        }
	;
    break;}
case 54:
#line 609 "script_parser.y"
{
        if (!scriptParseOnly)
		{
		    if (!sendAcse(scriptSession_ptr, associateAc_ptr, identifier)) YYABORT;
		}
	;
    break;}
case 55:
#line 616 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, associateRj_ptr, identifier)) YYABORT;
		}
	;
    break;}
case 56:
#line 623 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, releaseRq_ptr, identifier)) YYABORT;
		}
	;
    break;}
case 57:
#line 630 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, releaseRp_ptr, identifier)) YYABORT;
		}
	;
    break;}
case 58:
#line 637 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, abortRq_ptr, identifier)) YYABORT;
		}
	;
    break;}
case 59:
#line 646 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			/* Try to find the command object in the warehouse and send it */
			LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();
			DCM_COMMAND_CLASS* cmd_ptr = 0;
			cmd_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		identifier.c_str(),
																		WAREHOUSE->dimse2widtype(commandField)));
			if (cmd_ptr)
			{
				if (scriptIsNativeVts)
				{
					//Check for any VTS style uid mappings and try to resolve them
					resolveVTSUidMappings(cmd_ptr);
				}
				
				if (!sendSop(scriptSession_ptr, cmd_ptr, 0)) 
				{
					YYABORT;
				}


			}
			else
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 2, "Can't send command object %s, because it has not been created!",
					identifier.c_str());
				}
			}
		}
	;
    break;}
case 60:
#line 681 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			/* Try to find the command and dataset objects in the warehouse and send them */
			LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();
			DCM_COMMAND_CLASS* cmd_ptr = 0;
			cmd_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		identifier.c_str(),
																		WAREHOUSE->dimse2widtype(commandField)));
			DCM_DATASET_CLASS* data_ptr = 0;
			data_ptr = static_cast<DCM_DATASET_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		 datasetidentifier.c_str(),
																		 WID_DATASET));
			if (!cmd_ptr)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 2, "Can't send command object %s, because it has not been created!",
					identifier.c_str());
				}
			}
			if (!data_ptr)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 2, "Can't send dataset object %s, because it has not been created!",
					datasetidentifier.c_str());
				}
			}

			if (cmd_ptr && data_ptr)
			{
			    if (scriptIsNativeVts)
			    {
			        //Check for any VTS style uid mappings and try to resolve them
					//before sending the object
				    resolveVTSUidMappings(cmd_ptr);
					resolveVTSUidMappings(data_ptr);
			    }

				if (!sendSop(scriptSession_ptr, cmd_ptr, data_ptr)) 
				{
					YYABORT;
				}
			}
		}
	;
    break;}
case 61:
#line 729 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			/* Try to find the command and dataset objects in the warehouse and send them */
			LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();
			DCM_COMMAND_CLASS* cmd_ptr = 0;
			cmd_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		identifier.c_str(),
																		WAREHOUSE->dimse2widtype(commandField)));
			DCM_DATASET_CLASS* data_ptr = 0;
			data_ptr = static_cast<DCM_DATASET_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		 datasetidentifier.c_str(),
																		 WID_DATASET));
			if (!cmd_ptr)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 2, "Can't send command object %s, because it has not been created!",
					identifier.c_str());
				}
			}
			if (!data_ptr)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 2, "Can't send dataset object %s, because it has not been created!",
					datasetidentifier.c_str());
				}
			}

			if (cmd_ptr && data_ptr)
			{
			    cmd_ptr->setEncodePresentationContextId(presContextId);
				data_ptr->setEncodePresentationContextId(presContextId);
				presContextId = 0;

			    if (scriptIsNativeVts)
			    {
			        //Check for any VTS style uid mappings and try to resolve them
					//before sending the object
				    resolveVTSUidMappings(cmd_ptr);
					resolveVTSUidMappings(data_ptr);
			    }

				if (!sendSop(scriptSession_ptr, cmd_ptr, data_ptr)) 
				{
					YYABORT;
				}
			}
		}
	;
    break;}
case 63:
#line 786 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!importCommand(scriptSession_ptr, commandField, identifier)) 
			{
				YYABORT;
			}
		}
	;
    break;}
case 64:
#line 796 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!importCommandDataset(scriptSession_ptr, commandField, identifier, iodName, datasetidentifier))
			{
				YYABORT;
			}
		}
	;
    break;}
case 65:
#line 808 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "POPULATE ON");
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setPopulateWithAttributes(true);
			}			
		}
	;
    break;}
case 66:
#line 825 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "POPULATE OFF");
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setPopulateWithAttributes(false);
			}
		}
	;
    break;}
case 69:
#line 850 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			// set product role based on new setting for tool
			if (scriptvsp[0].userProvider == UP_REQUESTOR)
			{
				// tool role is now Requestor
				// - product is therefore Acceptor
				scriptSession_ptr->setProductRoleIsRequestor(false);
				scriptSession_ptr->setProductRoleIsAcceptor(true);
			}
			else
			{
				// tool role is now Acceptor
				// - product is therefore Requestor
				scriptSession_ptr->setProductRoleIsRequestor(true);
				scriptSession_ptr->setProductRoleIsAcceptor(false);
			}

			if (logger_ptr)
			{
				if (scriptvsp[0].userProvider == UP_REQUESTOR)
				{
					logger_ptr->text(LOG_SCRIPT, 2, "DVT ROLE REQUESTOR - PRODUCT ROLE ACCEPTOR");
				}
				else
				{
					logger_ptr->text(LOG_SCRIPT, 2, "DVT ROLE ACCEPTOR - PRODUCT ROLE REQUESTOR");
				}
			}
		}
	;
    break;}
case 72:
#line 893 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// make system call within session
			if (!systemCall(scriptSession_ptr, scriptvsp[0].string_ptr))
			{
				YYABORT;
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;		
	;
    break;}
case 73:
#line 909 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			static time_t tprv=0;
			time_t tnew;

			// get the time from the system
			time(&tnew);
			if (logger_ptr)
			{
				// on first call just display time now
				if (tprv == 0)
				{
					logger_ptr->text(LOG_SCRIPT, 2, "TIME %s", ctime(&tnew));
				}
				else
				{
					// on further calls display time now and difference since last call
					logger_ptr->text(LOG_SCRIPT, 2, "TIME %s (+%ld sec.)", ctime(&tnew), tnew-tprv);
				}
			}
			tprv = tnew;
		}
	;
    break;}
case 75:
#line 941 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_INFO, 2, "VERBOSE ON");
			}

			// ignore command in debug mode
			if (scriptSession_ptr->isLogLevel(LOG_DEBUG))
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_DEBUG, 1, "Ignoring VERBOSE ON");
				}
			}
			else
			{
				// enable the logger
//				scriptSession_ptr->enableLogger();

				// resume the serializer
//				BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
//				if (serializer_ptr)
//				{
//					serializer_ptr->Resume();
//				}
			}			
		}
	;
    break;}
case 76:
#line 973 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_INFO, 2, "VERBOSE OFF");
			}

			// ignore command in debug mode
			if (scriptSession_ptr->isLogLevel(LOG_DEBUG))
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_DEBUG, 1, "Ignoring VERBOSE OFF");
				}
			}
			else
			{
				// disable the logger
//				scriptSession_ptr->disableLogger();
			
				// pause the serializer
//				BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
//				if (serializer_ptr)
//				{
//					serializer_ptr->Pause();
//				}
			}
		}
	;
    break;}
case 78:
#line 1010 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "APPLICATION-ENTITY %s %s", scriptvsp[-1].string_ptr, scriptvsp[0].string_ptr);
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setApplicationEntityName(scriptvsp[-1].string_ptr);
				scriptExecutionContext_ptr->setApplicationEntityVersion(scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffers
		free(scriptvsp[-1].string_ptr);
		scriptvsp[-1].string_ptr = NULL;
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;

	;
    break;}
case 79:
#line 1036 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				switch(scriptvsp[0].validationFlag)
				{
				case ALL: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED"); break;
				case USE_DEFINITION: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-DEF-ONLY"); break;
				case USE_VR: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-VR-ONLY"); break;
				case USE_REFERENCE: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-REF-ONLY"); break;
				case NONE: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION DISABLED"); break;
				default: 
					if (scriptvsp[0].validationFlag == (USE_DEFINITION | USE_VR)) logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-DEF-AND-VR");
					else if (scriptvsp[0].validationFlag == (USE_DEFINITION | USE_REFERENCE)) logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-DEF-AND-REF");
					else if (scriptvsp[0].validationFlag == (USE_VR | USE_REFERENCE)) logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-VR-AND-REF");
				break;
				}
			}
			
			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setValidationFlag(scriptvsp[0].validationFlag);
			}
		}
	;
    break;}
case 80:
#line 1067 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "DEFINE-SQ-LENGTH ON");
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setDefineSqLength(true);
			}
		}
	;
    break;}
case 81:
#line 1084 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "DEFINE-SQ-LENGTH OFF");
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setDefineSqLength(false);
			}
		}
	;
    break;}
case 82:
#line 1103 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "ADD-GROUP-LENGTH ON");
			}
			
			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setAddGroupLength(true);
			}
		}
	;
    break;}
case 83:
#line 1120 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "ADD-GROUP-LENGTH OFF");
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setAddGroupLength(false);
			}
		}
	;
    break;}
case 84:
#line 1139 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "STRICT-VALIDATION ON");
			}
			
			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setStrictValidation(true);
			}
		}
	;
    break;}
case 85:
#line 1156 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "STRICT-VALIDATION OFF");
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setStrictValidation(false);
			}
		}
	;
    break;}
case 86:
#line 1175 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			BASE_WAREHOUSE_ITEM_DATA_CLASS	*wid_ptr = NULL;

			// generate the warehouse item data type
			switch(acseType)
			{
			case PDU_ASSOCIATE_RQ:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(associateRq_ptr);
				break;
			case PDU_ASSOCIATE_AC:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(associateAc_ptr);
				break;
			case PDU_ASSOCIATE_RJ:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(associateRj_ptr);
				break;
			case PDU_RELEASE_RQ:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(releaseRq_ptr);
				break;
			case PDU_RELEASE_RP:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(releaseRp_ptr);
				break;
			case PDU_ABORT_RQ:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(abortRq_ptr);
				break;
			case PDU_UNKNOWN:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(unknownPdu_ptr);
				break;
			default: break;
			}
			
			if (wid_ptr)
			{
				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), wid_ptr)) YYABORT;
			}
		}
	;
    break;}
case 87:
#line 1214 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// serialize
			BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
			if (serializer_ptr)
			{
				if ((command_ptr) && 
					(dataset_ptr))
				{
					// serialize the command and dataset create
					serializer_ptr->SerializeDSCreate(command_ptr->getIdentifier(), command_ptr, dataset_ptr->getIdentifier(), dataset_ptr);				
				}
				else if (command_ptr)
				{
					// serialize the command create
					serializer_ptr->SerializeDSCreate(command_ptr->getIdentifier(), command_ptr);
				}
			}

			if (command_ptr)
			{
				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), command_ptr->getIdentifier(), command_ptr)) YYABORT;
			}

			if (dataset_ptr) 
			{
				if (!command_ptr)
				{
					compareDatasetValueWithWarehouse(scriptSession_ptr->getLogger(), (char*)(iodName).c_str(), dataset_ptr);
				}

				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), dataset_ptr))
				{
					YYABORT;
				}
				else
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if ((dataset_ptr->getIdentifier() == NULL) &&
						(logger_ptr))
					{
						logger_ptr->text(LOG_WARNING, 1, "Unidentified Dataset added to Warehouse - beware!");
					}
				}
			}			
		}
	;
    break;}
case 88:
#line 1264 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (dataset_ptr)
			{
				// morph the dataset into an item
				DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();
				item_ptr->morph(dataset_ptr);

				// set defined length according to session setting
				bool definedLength = scriptSession_ptr->getDefineSqLength();
				if (scriptSession_ptr->getScriptExecutionContext())
				{
					definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
				}
				item_ptr->setDefinedLength(definedLength);

				// clean up - side effect of parser
				delete dataset_ptr;
				dataset_ptr = NULL;

				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), item_ptr->getIdentifier(), item_ptr)) YYABORT;
			}
		}
	;
    break;}
case 89:
#line 1290 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (dataset_ptr)
			{
				// morph the dataset into an item
				DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();
				item_ptr->morph(dataset_ptr);

				// indicate that the item should be encoded with a defined length
				item_ptr->setDefinedLength(true);

				// clean up - side effect of parser
				delete dataset_ptr;
				dataset_ptr = NULL;

				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), item_ptr->getIdentifier(), item_ptr)) YYABORT;
			}
		}
	;
    break;}
case 90:
#line 1311 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (dataset_ptr)
			{
				// set item handle name
				if (item_handle_ptr)
				{
					// set the item handle name and identifier
					item_handle_ptr->setName(dataset_ptr->getIodName());
					item_handle_ptr->setIdentifier(dataset_ptr->getIdentifier());

					// cascade the logger
					item_handle_ptr->setLogger(scriptSession_ptr->getLogger());

					// use the item handle
					if (item_handle_ptr->resolveReference() != NULL)
					{
						if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), item_handle_ptr->getIdentifier().c_str(), item_handle_ptr)) YYABORT;
					}
					else
					{
						// delete the unresolved item handle
						delete item_handle_ptr;
					}

					// clean up - side effect of parser
					item_handle_ptr = NULL;
				}

				// delete the dataset
				delete dataset_ptr;
				dataset_ptr = NULL;
			}
		}
	;
    break;}
case 91:
#line 1348 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileHead_ptr)
			{
				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileHead_ptr)) YYABORT;
			}
		}
	;
    break;}
case 92:
#line 1358 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileTail_ptr)) YYABORT;
			}
		}
	;
    break;}
case 93:
#line 1370 "script_parser.y"
{
        if (!scriptParseOnly)
		{
			WID_ENUM wid = WID_UNKNOWN;

			// generate the warehouse item data type
			switch(acseType)
			{
			case PDU_ASSOCIATE_RQ:
				wid = WID_ASSOCIATE_RQ;
				delete associateRq_ptr;
				associateRq_ptr = NULL; 
				break;
			case PDU_ASSOCIATE_AC: 
				wid = WID_ASSOCIATE_AC; 
				delete associateAc_ptr;
				associateAc_ptr = NULL; 
				break;
			case PDU_ASSOCIATE_RJ: 
				wid = WID_ASSOCIATE_RJ; 
				delete associateRj_ptr;
				associateRj_ptr = NULL; 
				break;
			case PDU_RELEASE_RQ: 
				wid = WID_RELEASE_RQ; 
				delete releaseRq_ptr;
				releaseRq_ptr = NULL; 
				break;
			case PDU_RELEASE_RP: 
				wid = WID_RELEASE_RP; 
				delete releaseRp_ptr;
				releaseRp_ptr = NULL; 
				break;
			case PDU_ABORT_RQ: 
				wid = WID_ABORT_RQ; 
				delete abortRq_ptr;
				abortRq_ptr = NULL; 
				break;
			case PDU_UNKNOWN: 
				wid = WID_UNKNOWN_PDU; 
				delete unknownPdu_ptr;
				unknownPdu_ptr = NULL; 
				break;
			default: break;
			}
			if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), wid)) YYABORT;
		}
	;
    break;}
case 94:
#line 1419 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// serialize
			BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
			if (serializer_ptr)
			{
				// serialize the command delete
				serializer_ptr->SerializeDSDeleteCommandSet(command_ptr->getIdentifier(), command_ptr);				
			}

			if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), command_ptr->getWidType())) YYABORT;
			
			// clean up - side effect of parser
			delete command_ptr;
			command_ptr = NULL; 
		}
	;
    break;}
case 95:
#line 1438 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// serialize
			BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
			if (serializer_ptr)
			{
				// serialize the dataset delete
				serializer_ptr->SerializeDSDeleteDataSet(dataset_ptr->getIdentifier(), dataset_ptr);				
			}
			
			// first try deleting as a dataset - leave explicit WID_DATASET in call
			if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), datasetidentifier.c_str(), WID_DATASET)) 
			{
				// then try deleting as an item - leave explicit WID_ITEM in call
				if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), datasetidentifier.c_str(), WID_ITEM)) YYABORT;
			}

			// clean up - side effect of parser
			delete dataset_ptr;
			dataset_ptr = NULL; 
		}
	;
    break;}
case 96:
#line 1462 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			identifier = "";

			if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), WID_FILEHEAD)) YYABORT;
		}
	;
    break;}
case 97:
#line 1471 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			identifier = "";

			if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), WID_FILETAIL)) YYABORT;
		}
	;
    break;}
case 98:
#line 1482 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (wid1_ptr)
			{
				// display the referenced warehouse object
				displayAttribute(scriptSession_ptr->getLogger(), scriptSession_ptr->getSerializer(), wid1_ptr, group, element);
			}
		}
	;
    break;}
case 99:
#line 1493 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (wid1_ptr)
			{
				// display the referenced warehouse object
				displayAttribute(scriptSession_ptr->getLogger(), scriptSession_ptr->getSerializer(), wid1_ptr, group, element);
			}
		}
	;
    break;}
case 100:
#line 1506 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!readFileDataset(scriptSession_ptr, scriptvsp[-1].string_ptr, dataset_ptr)) YYABORT;
		}
		// free malloced string buffer
		free(scriptvsp[-1].string_ptr);
		scriptvsp[-1].string_ptr = NULL;
		
	;
    break;}
case 101:
#line 1517 "script_parser.y"
{
		if (!scriptParseOnly)
		{
			if (!readFileDataset(scriptSession_ptr, scriptvsp[-1].string_ptr, scriptvsp[0].hex)) YYABORT;
		}
		// free malloced string buffer
		free(scriptvsp[-1].string_ptr);
		scriptvsp[-1].string_ptr = NULL;
	;
    break;}
case 102:
#line 1529 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			switch(acseType)
			{
			case PDU_ASSOCIATE_RQ:
				if (!receiveAcse(scriptSession_ptr, associateRq_ptr, identifier)) 
				{
					clearValidationObjects(scriptSession_ptr);
					YYABORT;
				}
				
				// parser cleanup
				delete associateRq_ptr;
				associateRq_ptr = NULL; 
				break;
			case PDU_ASSOCIATE_AC:
				if (!receiveAcse(scriptSession_ptr, associateAc_ptr, identifier))
				{
					clearValidationObjects(scriptSession_ptr);
					YYABORT;
				}
				
				// parser cleanup
				delete associateAc_ptr;
				associateAc_ptr = NULL; 				
				break;
			case PDU_ASSOCIATE_RJ:
				if (!receiveAcse(scriptSession_ptr, associateRj_ptr, identifier))
				{
					clearValidationObjects(scriptSession_ptr);
					YYABORT;
				}
				
				// parser cleanup
				delete associateRj_ptr;
				associateRj_ptr = NULL; 
				break;
			case PDU_RELEASE_RQ:
				if (!receiveAcse(scriptSession_ptr, releaseRq_ptr, identifier))
				{
					clearValidationObjects(scriptSession_ptr);
					YYABORT;
				}
				
				// parser cleanup
				delete releaseRq_ptr;
				releaseRq_ptr = NULL; 
				break;
			case PDU_RELEASE_RP:
				if (!receiveAcse(scriptSession_ptr, releaseRp_ptr, identifier))
				{
					clearValidationObjects(scriptSession_ptr);
					YYABORT;
				}
				
				// parser cleanup
				delete releaseRp_ptr;
				releaseRp_ptr = NULL; 
				break;
			case PDU_ABORT_RQ:
				if (!receiveAcse(scriptSession_ptr, abortRq_ptr, identifier))
				{
					clearValidationObjects(scriptSession_ptr);
					YYABORT;
				}
				
				// parser cleanup
				delete abortRq_ptr;
				abortRq_ptr = NULL; 
				break;
			case PDU_UNKNOWN:
				if (!receiveAcse(scriptSession_ptr, unknownPdu_ptr, identifier))
				{
					clearValidationObjects(scriptSession_ptr);
					YYABORT;
				}
				
				// parser cleanup
				delete unknownPdu_ptr;
				unknownPdu_ptr = NULL; 
				break;
			default: break;
			}
		}
	;
    break;}
case 103:
#line 1616 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!receiveSop(scriptSession_ptr, command_ptr, dataset_ptr)) 
			{
				clearValidationObjects(scriptSession_ptr);
				YYABORT;
			}
			
			// parser cleanup
			delete command_ptr;
			command_ptr = NULL; 
			delete dataset_ptr;
			dataset_ptr = NULL; 
		}
	;
    break;}
case 104:
#line 1635 "script_parser.y"
{
        if (!scriptParseOnly)
		{
			switch(acseType)
			{
			case PDU_ASSOCIATE_RQ:
				if (!sendAcse(scriptSession_ptr, associateRq_ptr, identifier))
				{ 
					YYABORT;
				}

				// parser cleanup
				delete associateRq_ptr;
				associateRq_ptr = NULL; 
				break;
			case PDU_ASSOCIATE_AC:
				if (!sendAcse(scriptSession_ptr, associateAc_ptr, identifier))
				{ 
					YYABORT;
				}

				// parser cleanup
				delete associateAc_ptr;
				associateAc_ptr = NULL; 
				break;
			case PDU_ASSOCIATE_RJ:
				if (!sendAcse(scriptSession_ptr, associateRj_ptr, identifier))
				{ 
					YYABORT;
				}

				// parser cleanup
				delete associateRj_ptr;
				associateRj_ptr = NULL; 
				break;
			case PDU_RELEASE_RQ:
				if (!sendAcse(scriptSession_ptr, releaseRq_ptr, identifier))
				{ 
					YYABORT;
				}

				// parser cleanup
				delete releaseRq_ptr;
				releaseRq_ptr = NULL; 
				break;
			case PDU_RELEASE_RP:
				if (!sendAcse(scriptSession_ptr, releaseRp_ptr, identifier))
				{ 
					YYABORT;
				}

				// parser cleanup
				delete releaseRp_ptr;
				releaseRp_ptr = NULL; 
				break;
			case PDU_ABORT_RQ:
				if (!sendAcse(scriptSession_ptr, abortRq_ptr, identifier))
				{ 
					YYABORT;
				}

				// parser cleanup
				delete abortRq_ptr;
				abortRq_ptr = NULL; 
				break;
			case PDU_UNKNOWN:
				if (!sendAcse(scriptSession_ptr, unknownPdu_ptr, identifier))
				{ 
					YYABORT;
				}

				// parser cleanup
				delete unknownPdu_ptr;
				unknownPdu_ptr = NULL; 
				break;
			default: break;
			}
		}
	;
    break;}
case 105:
#line 1715 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!sendSop(scriptSession_ptr, command_ptr, dataset_ptr)) 
			{
				YYABORT;
			}

			// parser cleanup
			delete command_ptr;
			command_ptr = NULL; 
			delete dataset_ptr;
			dataset_ptr = NULL; 
		}
	;
    break;}
case 106:
#line 1731 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// send command and dataset using the given presentation context id
			if (command_ptr)
			{
				command_ptr->setEncodePresentationContextId(presContextId);
			}

			if (dataset_ptr)
			{
				dataset_ptr->setEncodePresentationContextId(presContextId);
			}

			presContextId = 0;

			if (!sendSop(scriptSession_ptr, command_ptr, dataset_ptr))
			{
				YYABORT;
			}

			// parser cleanup
			delete command_ptr;
			command_ptr = NULL; 
			delete dataset_ptr;
			dataset_ptr = NULL; 
		}
	;
    break;}
case 107:
#line 1762 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			presContextId = (BYTE) scriptvsp[-1].hex;
		}
	;
    break;}
case 108:
#line 1769 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			presContextId = (BYTE) scriptvsp[-1].integer;
		}
	;
    break;}
case 109:
#line 1778 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			BASE_WAREHOUSE_ITEM_DATA_CLASS	*wid_ptr = NULL;

			// generate the warehouse item data type
			switch(acseType)
			{
			case PDU_ASSOCIATE_RQ:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(associateRq_ptr);
				break;
			case PDU_ASSOCIATE_AC:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(associateAc_ptr);
				break;
			case PDU_ASSOCIATE_RJ: 
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(associateRj_ptr);
				break;
			case PDU_RELEASE_RQ:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(releaseRq_ptr);
				break;
			case PDU_RELEASE_RP:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(releaseRp_ptr);
				break;
			case PDU_ABORT_RQ:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(abortRq_ptr);
				break;
			case PDU_UNKNOWN:
				wid_ptr = static_cast<BASE_WAREHOUSE_ITEM_DATA_CLASS*>(unknownPdu_ptr);
				break;
			default: break;
			}
			
			if (wid_ptr)
			{
				if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), wid_ptr)) YYABORT;
			}
		}
	;
    break;}
case 110:
#line 1817 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (command_ptr)
			{
				// serialize
				BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
				if (serializer_ptr)
				{
					// serialize the command set
					serializer_ptr->SerializeDSSetCommandSet(command_ptr->getIdentifier(), command_ptr);
				}
				
				if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), command_ptr->getIdentifier(), command_ptr))
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "SET - Can't find COMMAND with id: %s in Data Warehouse", command_ptr->getIdentifier());
					}
				 
					YYABORT;
				}
			}
		}
	;
    break;}
case 111:
#line 1844 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (dataset_ptr) 
			{
				// serialize
				BASE_SERIALIZER *serializer_ptr = scriptSession_ptr->getSerializer();
				if (serializer_ptr)
				{
					// serialize the dataset set
					serializer_ptr->SerializeDSSetDataSet(dataset_ptr->getIdentifier(), dataset_ptr);
				}

				// first try updating as a dataset
				if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), dataset_ptr))
				{
					// morph the dataset into an item
					DCM_ITEM_CLASS *item_ptr = new DCM_ITEM_CLASS();
					item_ptr->morph(dataset_ptr);

					// set defined length according to session setting
					bool definedLength = scriptSession_ptr->getDefineSqLength();
					if (scriptSession_ptr->getScriptExecutionContext())
					{
						definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
					}
					item_ptr->setDefinedLength(definedLength);

					// then try updating as an item
					if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), item_ptr->getIdentifier(), item_ptr)) 
					{
						LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
						if (logger_ptr)
						{
							logger_ptr->text(LOG_ERROR, 1, "SET - Can't find DATASET/ITEM with id: %s in Data Warehouse", item_ptr->getIdentifier());
						}

						YYABORT;
					}
				}
			}
		}
	;
    break;}
case 112:
#line 1888 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileHead_ptr)
			{
				if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileHead_ptr)) YYABORT;
			}
		}
	;
    break;}
case 113:
#line 1898 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileTail_ptr)) YYABORT;
			}
		}
	;
    break;}
case 114:
#line 1910 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// try to validate the Sop Class
			if (!validateSopAgainstList(scriptSession_ptr, command_ptr, dataset_ptr)) 
			{
				//cleanup, parser side effect
				command_ptr = 0;
				dataset_ptr = 0;
				clearValidationObjects(scriptSession_ptr);
				YYABORT;
			}
			else
			{
				//cleanup, parser side effect
				command_ptr = 0;
				dataset_ptr = 0;
				clearValidationObjects(scriptSession_ptr);
			}
		}
	;
    break;}
case 115:
#line 1934 "script_parser.y"
{		
        if (!scriptParseOnly)
		{	  
		   /* Try to find the command object in the warehouse */
		   LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();

		   //re-initialize object pointers
		   command_ptr = 0;
		   dataset_ptr = 0;
		   command_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		   identifier.c_str(),
																		   WAREHOUSE->dimse2widtype(commandField)));
		}
	;
    break;}
case 116:
#line 1949 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			/* Try to find the objects in the warehouse */
			LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();

			//re-initialize object pointers
			command_ptr = 0;
			dataset_ptr = 0;
			command_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		   identifier.c_str(),
																		   WAREHOUSE->dimse2widtype(commandField)));

			dataset_ptr = static_cast<DCM_DATASET_CLASS*>(retrieveFromWarehouse(logger_ptr,
																		   datasetidentifier.c_str(),
																		   WID_DATASET));
			if (!command_ptr)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 2, "Can't validate command object %s, because it has not been created!",
					identifier.c_str());
				}
			}
			if (!dataset_ptr)
			{
			   if (logger_ptr)
			   {	
					logger_ptr->text(LOG_ERROR, 2, "Can't validate dataset object %s, because it has not been created!",
					datasetidentifier.c_str());
				}
			}
		}
	;
    break;}
case 118:
#line 1987 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			addReferenceObjects(scriptSession_ptr,ref_command_ptr, ref_dataset_ptr, scriptSession_ptr->getLogger());
		}
	;
    break;}
case 119:
#line 1994 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			setLogicalOperand(OPERAND_OR);
			addReferenceObjects(scriptSession_ptr,ref_command_ptr, ref_dataset_ptr, scriptSession_ptr->getLogger());
		}
	;
    break;}
case 120:
#line 2004 "script_parser.y"
{		
        if (!scriptParseOnly)
		{	  
			/* Try to find the command object in the warehouse */
			LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();

			//re-initialize object pointers
			ref_command_ptr = 0;
			ref_dataset_ptr = 0;
			ref_command_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(logger_ptr,
																			  identifier.c_str(),
																			  WAREHOUSE->dimse2widtype(commandField)));
		}
	;
    break;}
case 121:
#line 2019 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			/* Try to find the objects in the warehouse */
			LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();

			//re-initialize object pointers
			ref_command_ptr = 0;
			ref_dataset_ptr = 0;
			ref_command_ptr = static_cast<DCM_COMMAND_CLASS*>(retrieveFromWarehouse(logger_ptr,
																			  identifier.c_str(),
																			  WAREHOUSE->dimse2widtype(commandField)));

			ref_dataset_ptr = static_cast<DCM_DATASET_CLASS*>(retrieveFromWarehouse(logger_ptr,
																			  datasetidentifier.c_str(),
																			  WID_DATASET));
			if (!ref_command_ptr)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 2, "Can't use reference command object %s, because it has not been created!",
				    identifier.c_str());
			    }
			}
			if (!ref_dataset_ptr)
			{
				if (logger_ptr)
			    {	
				    logger_ptr->text(LOG_ERROR, 2, "Can't use reference dataset object %s, because it has not been created!",
				    datasetidentifier.c_str());
			    }
			}
		}
	;
    break;}
case 122:
#line 2057 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// ensure that no command or dataset is defined
			if (command_ptr)
			{
				delete command_ptr;
				command_ptr = NULL;
			}

			if (dataset_ptr)
			{
				delete dataset_ptr;
				dataset_ptr = NULL;
			}
		}
	;
    break;}
case 123:
#line 2075 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			addReferenceObjects(scriptSession_ptr, command_ptr, dataset_ptr, scriptSession_ptr->getLogger());
		}
	;
    break;}
case 124:
#line 2082 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			setLogicalOperand(OPERAND_OR);
			addReferenceObjects(scriptSession_ptr, command_ptr, dataset_ptr, scriptSession_ptr->getLogger());
		}
	;
    break;}
case 125:
#line 2090 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			setLogicalOperand(OPERAND_AND);
			addReferenceObjects(scriptSession_ptr, command_ptr, dataset_ptr, scriptSession_ptr->getLogger());
		}
	;
    break;}
case 126:
#line 2100 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!writeFileHead(scriptSession_ptr->getLogger(), scriptvsp[-1].string_ptr, scriptSession_ptr->getAutoCreateDirectory())) YYABORT;
		}
		// free malloced string buffer
		free(scriptvsp[-1].string_ptr);
		scriptvsp[-1].string_ptr = NULL;	
	;
    break;}
case 127:
#line 2110 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  	
			if (!writeFileTail(scriptSession_ptr->getLogger(), scriptvsp[-1].string_ptr, scriptSession_ptr->getAutoCreateDirectory())) YYABORT;
		}
		// free malloced string buffer
		free(scriptvsp[-1].string_ptr);
		scriptvsp[-1].string_ptr = NULL;
	;
    break;}
case 128:
#line 2120 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (!writeFileDataset(scriptSession_ptr->getLogger(), scriptvsp[-1].string_ptr, dataset_ptr,scriptSession_ptr->getAutoCreateDirectory())) YYABORT;
		}
		// free malloced string buffer
		free(scriptvsp[-1].string_ptr);
		scriptvsp[-1].string_ptr = NULL;
	;
    break;}
case 142:
#line 2149 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store the user information
			associateRq_ptr->setUserInformation(userInformation);
			userInformation.cleanup();
		}
	;
    break;}
case 145:
#line 2164 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set associate request protocol version parameter
			associateRq_ptr->setProtocolVersion((UINT16) scriptvsp[0].integer);
		}
	;
    break;}
case 146:
#line 2172 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			if (strlen(scriptvsp[0].string_ptr) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Called AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Request", scriptvsp[0].string_ptr, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate request called Ae title parameter
				associateRq_ptr->setCalledAeTitle((char*) scriptvsp[0].string_ptr);
			}			
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;	
	;
    break;}
case 147:
#line 2196 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			if (strlen(scriptvsp[0].string_ptr) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Calling AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Request", scriptvsp[0].string_ptr, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate request calling Ae title parameter
				associateRq_ptr->setCallingAeTitle((char*) scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 148:
#line 2220 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// set associate request application context parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[0].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				associateRq_ptr->setApplicationContextName((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				associateRq_ptr->setApplicationContextName((char*) scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 150:
#line 2245 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set user information maximum length received parameter
			userInformation.setMaximumLengthReceived((UINT32) scriptvsp[0].integer);
		}
	;
    break;}
case 151:
#line 2253 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// set user information implementation class uid parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[0].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				userInformation.setImplementationClassUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				userInformation.setImplementationClassUid((char*) scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 152:
#line 2277 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// set user information implementation version name parameter
			userInformation.setImplementationVersionName((char*) scriptvsp[0].string_ptr);
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 155:
#line 2291 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set user information asynchronous operations window parameter
			userInformation.setAsynchronousOperationWindow((UINT16) scriptvsp[-1].integer, (UINT16) scriptvsp[0].integer);
		}
	;
    break;}
case 162:
#line 2311 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[-3].string_ptr);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[-3].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) scriptvsp[-3].string_ptr);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", scriptvsp[-3].string_ptr);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// store the abstract syntax name
			presRqContext.setAbstractSyntaxName(abstractSyntaxName);

			// add the presentation context to the associate request
			associateRq_ptr->addPresentationContext(presRqContext);

			// free up the local presentation context
			presRqContext.cleanup();
			presRqContext.setPresentationContextId(0);
		}
	;
    break;}
case 163:
#line 2358 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[-7].string_ptr);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[-7].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) scriptvsp[-7].string_ptr);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", scriptvsp[-7].string_ptr);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// store the presentation context id
			presRqContext.setPresentationContextId((BYTE) scriptvsp[-9].integer);

			// store the abstract syntax name
			presRqContext.setAbstractSyntaxName(abstractSyntaxName);

			// add the presentation context to the associate request
			associateRq_ptr->addPresentationContext(presRqContext);

			// free up the local presentation context
			presRqContext.cleanup();
			presRqContext.setPresentationContextId(0);

			int scuRole = scriptvsp[-5].integer;
			int scpRole = scriptvsp[-3].integer;

			// check if roles explicitly defined by scripts
			if ((scuRole != UNDEFINED_SCU_ROLE) &&
				(scpRole != UNDEFINED_SCP_ROLE))
			{
				// handle scp scu role
				scpScuRoleSelect.setUid(abstractSyntaxName.getUid());
				scpScuRoleSelect.setScuRole((BYTE) scuRole);
				scpScuRoleSelect.setScpRole((BYTE) scpRole);

				// store scp scu role select
				userInformation.addScpScuRoleSelect(scpScuRoleSelect);
			}
		}
		// free malloced string buffer
		free(scriptvsp[-7].string_ptr);
		scriptvsp[-7].string_ptr = NULL;
	;
    break;}
case 164:
#line 2427 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// save the latest transfer syntax
			presRqContext.addTransferSyntaxName(transferSyntaxName);
		}
	;
    break;}
case 165:
#line 2435 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// save the latest transfer syntax
			presRqContext.addTransferSyntaxName(transferSyntaxName);
		}
	;
    break;}
case 166:
#line 2445 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[0].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				transferSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				transferSyntaxName.setUid((char*) scriptvsp[0].string_ptr);

				// check that we now have a valid transfer syntax
				if (!transferSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", scriptvsp[0].string_ptr);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 169:
#line 2487 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[-3].string_ptr);
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[-3].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				sopClassExtended.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				sopClassExtended.setUid((char*) scriptvsp[-3].string_ptr);

				// check that we now have a valid transfer syntax
				if (!sopClassExtended.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", scriptvsp[-3].string_ptr);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// save the sop class extended information
			userInformation.addSopClassExtended(sopClassExtended);
			sopClassExtended.cleanup();
		}
		// free malloced string buffer
		free(scriptvsp[-3].string_ptr);
		scriptvsp[-3].string_ptr = NULL;
	;
    break;}
case 172:
#line 2533 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// handle application info bytes
			sopClassExtended.addApplicationInformation((BYTE) scriptvsp[0].integer);
		}
	;
    break;}
case 175:
#line 2547 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			UID_CLASS	sopClassUid;

			convertHex(scriptvsp[-5].string_ptr);
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[-5].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				sopClassUid.set((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				sopClassUid.set((char*) scriptvsp[-5].string_ptr);

				// check that we now have a valid abstract syntax
				if (!sopClassUid.isValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", scriptvsp[-5].string_ptr);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// handle scp scu role
			scpScuRoleSelect.setUid(sopClassUid);
			scpScuRoleSelect.setScpRole((BYTE) scriptvsp[-3].integer);
			scpScuRoleSelect.setScuRole((BYTE) scriptvsp[-1].integer);

			// store scp scu role select
			userInformation.addScpScuRoleSelect(scpScuRoleSelect);
		}
		// free malloced string buffer
		free(scriptvsp[-5].string_ptr);
		scriptvsp[-5].string_ptr = NULL;
	;
    break;}
case 176:
#line 2595 "script_parser.y"
{
		if (!scriptParseOnly)
		{	  
			// store the user identity information
			userInformation.setUserIdentityNegotiation((BYTE) scriptvsp[-2].integer, (BYTE) scriptvsp[-1].integer, scriptvsp[0].string_ptr);
		}
			
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
	;
    break;}
case 177:
#line 2606 "script_parser.y"
{
        if (!scriptParseOnly)
		{
			// store the user identity information
			userInformation.setUserIdentityNegotiation((BYTE) scriptvsp[-3].integer, (BYTE) scriptvsp[-2].integer, scriptvsp[-1].string_ptr, scriptvsp[0].string_ptr);
		}
				
		// free malloced string buffer
		free(scriptvsp[-1].string_ptr);
		free(scriptvsp[0].string_ptr);
	;
    break;}
case 178:
#line 2618 "script_parser.y"
{
        if (!scriptParseOnly)
		{	
			// store the user identity information
			userInformation.setUserIdentityNegotiation(scriptvsp[0].string_ptr);
		}
		
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
	;
    break;}
case 180:
#line 2632 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store the user information
			associateAc_ptr->setUserInformation(userInformation);
			userInformation.cleanup();
		}
	;
    break;}
case 183:
#line 2647 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set associate accept protocol version parameter
			associateAc_ptr->setProtocolVersion((UINT16) scriptvsp[0].integer);
		}
	;
    break;}
case 184:
#line 2655 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			if (strlen(scriptvsp[0].string_ptr) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Called AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Accept", scriptvsp[0].string_ptr, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate accept called Ae title parameter
				associateAc_ptr->setCalledAeTitle((char*) scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 185:
#line 2679 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			if (strlen(scriptvsp[0].string_ptr) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Calling AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Accept", scriptvsp[0].string_ptr, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate accept calling Ae title parameter
				associateAc_ptr->setCallingAeTitle((char*) scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 186:
#line 2703 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// set associate accept application context parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[0].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				associateAc_ptr->setApplicationContextName((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				associateAc_ptr->setApplicationContextName((char*) scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 188:
#line 2728 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set user information maximum length received parameter
			userInformation.setMaximumLengthReceived((UINT32) scriptvsp[0].integer);
		}
	;
    break;}
case 189:
#line 2736 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// set user information implementation class uid parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[0].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				userInformation.setImplementationClassUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				userInformation.setImplementationClassUid((char*) scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 190:
#line 2760 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// set user information implementation version name parameter
			userInformation.setImplementationVersionName((char*) scriptvsp[0].string_ptr);
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 193:
#line 2774 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set user information asynchronous operations window parameter
			userInformation.setAsynchronousOperationWindow((UINT16) scriptvsp[-1].integer, (UINT16) scriptvsp[0].integer);
		}
	;
    break;}
case 199:
#line 2793 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[-4].string_ptr);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[-4].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) scriptvsp[-4].string_ptr);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", scriptvsp[-4].string_ptr);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// store the abstract syntax name
			presAcContext.setAbstractSyntaxName(abstractSyntaxName);

			// store the result
			presAcContext.setResultReason((BYTE) scriptvsp[-2].integer);

			// store the transfer syntax name
			presAcContext.setTransferSyntaxName(transferSyntaxName);

			// add the presentation context to the associate accept
			associateAc_ptr->addPresentationContext(presAcContext);

			// free up the local presentation context
			presAcContext.setPresentationContextId(0);
		}
		// free malloced string buffer
		free(scriptvsp[-4].string_ptr);
		scriptvsp[-4].string_ptr = NULL;
	;
    break;}
case 200:
#line 2848 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// no transfer syntax name provided
			transferSyntaxName.setUid("");
		}
	;
    break;}
case 202:
#line 2859 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[-2].string_ptr);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) scriptvsp[-2].string_ptr);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) scriptvsp[-2].string_ptr);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", scriptvsp[-2].string_ptr);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// store the abstract syntax name
			presAcContext.setAbstractSyntaxName(abstractSyntaxName);

			// store the presentation context id
			presAcContext.setPresentationContextId((BYTE) scriptvsp[-6].integer);

			// store the result
			presAcContext.setResultReason((BYTE) scriptvsp[-4].integer);

			// store the transfer syntax name
			presAcContext.setTransferSyntaxName(transferSyntaxName);

			// add the presentation context to the associate accept
			associateAc_ptr->addPresentationContext(presAcContext);

			// free up the local presentation context
			presAcContext.setPresentationContextId(0);

			// handle scp scu role
			if (((BYTE) scriptvsp[-4].integer == ACCEPTANCE) &&
				(assocAcScuScpRolesDefined))

			{
				// save abstract syntax name
				scpScuRoleSelect.setUid(abstractSyntaxName.getUid());

				// store scp scu role select
				userInformation.addScpScuRoleSelect(scpScuRoleSelect);
			}
		}
		// free malloced string buffer
		free(scriptvsp[-2].string_ptr);
		scriptvsp[-2].string_ptr = NULL;
	;
    break;}
case 203:
#line 2929 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// no transfer syntax name provided
			transferSyntaxName.setUid("");
		}
	;
    break;}
case 204:
#line 2937 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			assocAcScuScpRolesDefined = false;
			int scuRole = scriptvsp[-4].integer;
			int scpRole = scriptvsp[-2].integer;

			// check if roles explicitly defined by scripts
			if ((scuRole != UNDEFINED_SCU_ROLE) &&
				(scpRole != UNDEFINED_SCP_ROLE))
			{
				// handle scp scu role
				scpScuRoleSelect.setScuRole((BYTE) scuRole);
				scpScuRoleSelect.setScpRole((BYTE) scpRole);
				assocAcScuScpRolesDefined = true;
			}
		}
	;
    break;}
case 209:
#line 2966 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set associate reject result parameter
			associateRj_ptr->setResult((BYTE) scriptvsp[0].integer);
		}
	;
    break;}
case 210:
#line 2974 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set associate reject source parameter
			associateRj_ptr->setSource((BYTE) scriptvsp[0].integer);
		}
	;
    break;}
case 211:
#line 2982 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set associate reject reason parameter
			associateRj_ptr->setReason((BYTE) scriptvsp[0].integer);
		}
	;
    break;}
case 218:
#line 3006 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set abort request source parameter
			abortRq_ptr->setSource((BYTE) scriptvsp[0].integer);
		}
	;
    break;}
case 219:
#line 3014 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set abort request reason parameter
			abortRq_ptr->setReason((BYTE) scriptvsp[0].integer);
		}
	;
    break;}
case 220:
#line 3024 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set up default user information details
			userInformation.setMaximumLengthReceived(UNDEFINED_MAXIMUM_LENGTH_RECEIVED);
			userInformation.setImplementationClassUid((char*) "");
			userInformation.setImplementationVersionName((char*) "");

			// set up test session associate request details
			associateRq_ptr = new ASSOCIATE_RQ_CLASS();

			// cascade the logger
			associateRq_ptr->setLogger(scriptSession_ptr->getLogger());

			acseType = PDU_ASSOCIATE_RQ;

			// set up identifier
			char buffer[32];
			sprintf(buffer, "ID%d", scriptSession_ptr->getInstanceId());
			identifier = buffer;
		}
	;
    break;}
case 221:
#line 3049 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set up default user information details
			userInformation.setMaximumLengthReceived(UNDEFINED_MAXIMUM_LENGTH_RECEIVED);
			userInformation.setImplementationClassUid((char*) "");
			userInformation.setImplementationVersionName((char*) "");

			// set up test session associate accept details
			associateAc_ptr = new ASSOCIATE_AC_CLASS();

			// cascade the logger
			associateAc_ptr->setLogger(scriptSession_ptr->getLogger());

			acseType = PDU_ASSOCIATE_AC;

			// set up identifier
			char buffer[32];
			sprintf(buffer, "ID%d", scriptSession_ptr->getInstanceId());
			identifier = buffer;
		}
	;
    break;}
case 222:
#line 3074 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set up default associate reject details
			associateRj_ptr = new ASSOCIATE_RJ_CLASS();

			// cascade the logger
			associateRj_ptr->setLogger(scriptSession_ptr->getLogger());

			acseType = PDU_ASSOCIATE_RJ;

			// set up identifier
			char buffer[32];
			sprintf(buffer, "ID%d", scriptSession_ptr->getInstanceId());
			identifier = buffer;
		}
	;
    break;}
case 223:
#line 3094 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set up default release request details
			releaseRq_ptr = new RELEASE_RQ_CLASS();

			// cascade the logger
			releaseRq_ptr->setLogger(scriptSession_ptr->getLogger());

			acseType = PDU_RELEASE_RQ;

			// set up identifier
			char buffer[32];
			sprintf(buffer, "ID%d", scriptSession_ptr->getInstanceId());
			identifier = buffer;
		}
	;
    break;}
case 224:
#line 3114 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set up default release response details
			releaseRp_ptr = new RELEASE_RP_CLASS();

			// cascade the logger
			releaseRp_ptr->setLogger(scriptSession_ptr->getLogger());

			acseType = PDU_RELEASE_RP;

			// set up identifier
			char buffer[32];
			sprintf(buffer, "ID%d", scriptSession_ptr->getInstanceId());
			identifier = buffer;

		}
	;
    break;}
case 225:
#line 3135 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// set up default abort request details
			abortRq_ptr = new ABORT_RQ_CLASS();

			// cascade the logger
			abortRq_ptr->setLogger(scriptSession_ptr->getLogger());

			acseType = PDU_ABORT_RQ;

			// set up identifier
			char buffer[32];
			sprintf(buffer, "ID%d", scriptSession_ptr->getInstanceId());
			identifier = buffer;
		}
	;
    break;}
case 233:
#line 3168 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// instantiate a new command
			command_ptr = new DCM_COMMAND_CLASS(commandField);

			// cascade the logger
			command_ptr->setLogger(scriptSession_ptr->getLogger());
			dataset_ptr = NULL;
			identifier = "";
		}
	;
    break;}
case 234:
#line 3181 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// instantiate a new command
			command_ptr = new DCM_COMMAND_CLASS(commandField, identifier);

			// cascade the logger
			command_ptr->setLogger(scriptSession_ptr->getLogger());
			dataset_ptr = NULL;
		}
	;
    break;}
case 235:
#line 3195 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// save command field locally
			commandField = scriptlval.commandField;
		}
	;
    break;}
case 236:
#line 3205 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// save identifier locally
			identifier = scriptlval.identifier;
		}
	;
    break;}
case 242:
#line 3226 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// instantiate a new dataset
			dataset_ptr = new DCM_DATASET_CLASS(commandField, iodName);

			// cascade the logger
			dataset_ptr->setLogger(scriptSession_ptr->getLogger());
			
			bool addGroupLength = scriptSession_ptr->getAddGroupLength();
			bool populateWithAttributes = scriptSession_ptr->getAutoType2Attributes();
			bool definedLength = scriptSession_ptr->getDefineSqLength();
			if (scriptSession_ptr->getScriptExecutionContext())
			{
				addGroupLength = scriptSession_ptr->getScriptExecutionContext()->getAddGroupLength();
				populateWithAttributes = scriptSession_ptr->getScriptExecutionContext()->getPopulateWithAttributes();
				definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
			}
			dataset_ptr->setDefineGroupLengths(addGroupLength);
			dataset_ptr->setPopulateWithAttributes(populateWithAttributes);
			dataset_ptr->setDefineSqLengths(definedLength);

			identifier = "";
		}
	;
    break;}
case 243:
#line 3252 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// instantiate a new dataset
			dataset_ptr = new DCM_DATASET_CLASS(commandField, iodName, datasetidentifier);

			bool addGroupLength = scriptSession_ptr->getAddGroupLength();
			bool populateWithAttributes = scriptSession_ptr->getAutoType2Attributes();
			bool definedLength = scriptSession_ptr->getDefineSqLength();
			if (scriptSession_ptr->getScriptExecutionContext())
			{
				addGroupLength = scriptSession_ptr->getScriptExecutionContext()->getAddGroupLength();
				populateWithAttributes = scriptSession_ptr->getScriptExecutionContext()->getPopulateWithAttributes();
				definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
			}
			dataset_ptr->setDefineGroupLengths(addGroupLength);
			dataset_ptr->setPopulateWithAttributes(populateWithAttributes);
			dataset_ptr->setDefineSqLengths(definedLength);
		}
	;
    break;}
case 244:
#line 3275 "script_parser.y"
{

	;
    break;}
case 245:
#line 3279 "script_parser.y"
{
	;
    break;}
case 246:
#line 3284 "script_parser.y"
{
		// this rule introduces a shift/reduce warning in the parser
		// - can't get rid of this due to sytax needed for backwards compatibility
	;
    break;}
case 247:
#line 3291 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// save iod name locally
			// - check first for iod name mapping
			iodName = scriptSession_ptr->getIodName((char*) scriptvsp[0].string_ptr);
			if (!iodName.length()) 
			{
				// no name mapping - take iod name directly
				iodName = (char*) scriptvsp[0].string_ptr;
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 248:
#line 3311 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// save identifier locally
			datasetidentifier = scriptvsp[0].identifier;
		}
	;
    break;}
case 249:
#line 3319 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// save the identifier locally
			datasetidentifier = scriptvsp[0].string_ptr;
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 250:
#line 3333 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// add attribute to local command or dataset
			if ((group >= GROUP_TWO) &&
				(dataset_ptr))
			{
				// save attribute in dataset
				dataset_ptr->addAttribute(attribute_ptr[nd]);
			}
			else
			{
				// save attribute in command
				command_ptr->addAttribute(attribute_ptr[nd]);
			}
		}
	;
    break;}
case 251:
#line 3351 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// add attribute to local command or dataset
			if ((group >= GROUP_TWO) &&
				(dataset_ptr))
			{
				// save attribute in dataset
				dataset_ptr->addAttribute(attribute_ptr[nd]);
			}
			else
			{
				// save attribute in command
				command_ptr->addAttribute(attribute_ptr[nd]);
			}
		}
	;
    break;}
case 253:
#line 3372 "script_parser.y"
{
		// attribute should be seen as deleted from the Command/Dataset
        if (!scriptParseOnly)
		{	  
			// attribute deletion
			// - instantiate a new attribute
			attribute_ptr[nd] = new DCM_ATTRIBUTE_CLASS(group, element);
			
			// mark attribute as deleted - not present
			attribute_ptr[nd]->SetPresent(false);
			
			// look up vr based on tag - need to look-up the VR in the definitions
			vr = DEFINITION->GetAttributeVr(group, element);

			// store attribute vr
			attribute_ptr[nd]->SetVR(vr);

			// cascade the logger
			attribute_ptr[nd]->setLogger(scriptSession_ptr->getLogger());
		}
	;
    break;}
case 254:
#line 3396 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// instantiate a new attribute
			attribute_ptr[nd] = new DCM_ATTRIBUTE_CLASS(group, element);

			// cascade the logger
			attribute_ptr[nd]->setLogger(scriptSession_ptr->getLogger());

			bool definedLength = scriptSession_ptr->getDefineSqLength();
			if (scriptSession_ptr->getScriptExecutionContext())
			{
				definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
			}
			attribute_ptr[nd]->setDefinedLength(definedLength);
		}
	;
    break;}
case 255:
#line 3416 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store tag locally
			group = (UINT16) (scriptlval.hex >> 16);
			element = (UINT16) (scriptlval.hex & 0x0000FFFF);
		}
	;
    break;}
case 258:
#line 3431 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (nd == 0)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "DICOMScript SQ nesting corrupted");
				}
				YYABORT;
			}

			// clean up last (unused) item
			delete item_ptr[nd];

			// decrement nesting depth
			nd--;
		}
	;
    break;}
case 259:
#line 3455 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			// store vr locally
			attribute_ptr[nd]->SetVR(ATTR_VR_SQ);
			attribute_ptr[nd]->setDefinedLength(false);
			sq_ptr[nd] = NULL;

			// increment nesting depth
			nd++;
			if (nd == MAX_ND)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "DICOMScript SQ nesting too deep - reached: %d", MAX_ND);
				}
				YYABORT;
			}

			// instantiate the first item
			item_ptr[nd] = new DCM_ITEM_CLASS();

			// cascade the logger
			item_ptr[nd]->setLogger(logger_ptr);
			
			bool definedLength = scriptSession_ptr->getDefineSqLength();
			if (scriptSession_ptr->getScriptExecutionContext())
			{
				definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
			}
			item_ptr[nd]->setDefinedLength(definedLength);
		}
	;
    break;}
case 260:
#line 3491 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			// store vr locally
			attribute_ptr[nd]->SetVR(ATTR_VR_SQ);
			attribute_ptr[nd]->setDefinedLength(true);
			sq_ptr[nd] = NULL;

			// increment nesting depth
			nd++;
			if (nd == MAX_ND)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "DICOMScript SQ nesting too deep - reached: %d", MAX_ND);
				}
				YYABORT;
			}

			// instantiate the first item
			item_ptr[nd] = new DCM_ITEM_CLASS();

			// cascade the logger
			item_ptr[nd]->setLogger(logger_ptr);
			
			bool definedLength = scriptSession_ptr->getDefineSqLength();
			if (scriptSession_ptr->getScriptExecutionContext())
			{
				definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
			}
			item_ptr[nd]->setDefinedLength(definedLength);
		}
	;
    break;}
case 263:
#line 3533 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// link item to parent sq attribute
			// NOTE: Minus 1 here!
			if (sq_ptr[nd - 1] == NULL) 
			{
				sq_ptr[nd - 1] = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);

				// cascade the logger
				sq_ptr[nd - 1]->setLogger(scriptSession_ptr->getLogger());
				
				bool definedLength = scriptSession_ptr->getDefineSqLength();
				if (scriptSession_ptr->getScriptExecutionContext())
				{
					definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
				}
				sq_ptr[nd - 1]->setDefinedLength(definedLength);

				attribute_ptr[nd -1]->addSqValue(sq_ptr[nd -1]);
			}

			// check that an item has been defined - could be a zero length SQ
			if (item_ptr[nd]->getIdentifier())
			{
				sq_ptr[nd - 1]->addItem(item_ptr[nd]);

				// instantiate the next item
				item_ptr[nd] = new DCM_ITEM_CLASS();

				// cascade the logger
				item_ptr[nd]->setLogger(scriptSession_ptr->getLogger());
				
				bool definedLength = scriptSession_ptr->getDefineSqLength();
				if (scriptSession_ptr->getScriptExecutionContext())
				{
					definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
				}
				item_ptr[nd]->setDefinedLength(definedLength);
			}
		}
	;
    break;}
case 264:
#line 3576 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// link item to parent sq attribute
			// NOTE: Minus 1 here!
			if (sq_ptr[nd - 1] == NULL) 
			{
				sq_ptr[nd - 1] = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);

				// cascade the logger
				sq_ptr[nd - 1]->setLogger(scriptSession_ptr->getLogger());
				
				bool definedLength = scriptSession_ptr->getDefineSqLength();
				if (scriptSession_ptr->getScriptExecutionContext())
				{
					definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
				}
				sq_ptr[nd - 1]->setDefinedLength(definedLength);

				attribute_ptr[nd -1]->addSqValue(sq_ptr[nd -1]);
			}
			// check that an item has been defined - could be a zero length SQ
			if (item_ptr[nd]->getIdentifier())
			{
				sq_ptr[nd - 1]->addItem(item_ptr[nd]);

				// instantiate the next item
				item_ptr[nd] = new DCM_ITEM_CLASS();

				// cascade the logger
				item_ptr[nd]->setLogger(scriptSession_ptr->getLogger());
				
				bool definedLength = scriptSession_ptr->getDefineSqLength();
				if (scriptSession_ptr->getScriptExecutionContext())
				{
					definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
				}
				item_ptr[nd]->setDefinedLength(definedLength);
			}
		}
	;
    break;}
case 265:
#line 3620 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			// store item - identifier
			item_ptr[nd]->setIdentifier(scriptvsp[0].string_ptr);
			item_ptr[nd]->setValueByReference(true);
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 266:
#line 3635 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// link item to parent sq attribute
			// NOTE: Minus 1 here!
			if (sq_ptr[nd - 1] == NULL) 
			{
				sq_ptr[nd - 1] = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);

				// cascade the logger
				sq_ptr[nd - 1]->setLogger(scriptSession_ptr->getLogger());
				
				bool definedLength = scriptSession_ptr->getDefineSqLength();
				if (scriptSession_ptr->getScriptExecutionContext())
				{
					definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
				}
				sq_ptr[nd - 1]->setDefinedLength(definedLength);

				attribute_ptr[nd -1]->addSqValue(sq_ptr[nd -1]);
			}
			sq_ptr[nd - 1]->addItem(item_ptr[nd]);

			// instantiate the next item
			item_ptr[nd] = new DCM_ITEM_CLASS();

			// cascade the logger
			item_ptr[nd]->setLogger(scriptSession_ptr->getLogger());
			
			bool definedLength = scriptSession_ptr->getDefineSqLength();
			if (scriptSession_ptr->getScriptExecutionContext())
			{
				definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
			}
			item_ptr[nd]->setDefinedLength(definedLength);
		}
	;
    break;}
case 267:
#line 3673 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// link item to parent sq attribute
			// NOTE: Minus 1 here!
			if (sq_ptr[nd - 1] == NULL) 
			{
				sq_ptr[nd - 1] = new DCM_VALUE_SQ_CLASS(UNDEFINED_LENGTH);

				// cascade the logger
				sq_ptr[nd - 1]->setLogger(scriptSession_ptr->getLogger());
				
				bool definedLength = scriptSession_ptr->getDefineSqLength();
				if (scriptSession_ptr->getScriptExecutionContext())
				{
					definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
				}
				sq_ptr[nd - 1]->setDefinedLength(definedLength);

				attribute_ptr[nd -1]->addSqValue(sq_ptr[nd -1]);
			}
			sq_ptr[nd - 1]->addItem(item_ptr[nd]);

			// instantiate the next item
			item_ptr[nd] = new DCM_ITEM_CLASS();

			// cascade the logger
			item_ptr[nd]->setLogger(scriptSession_ptr->getLogger());
			
			bool definedLength = scriptSession_ptr->getDefineSqLength();
			if (scriptSession_ptr->getScriptExecutionContext())
			{
				definedLength = scriptSession_ptr->getScriptExecutionContext()->getDefineSqLength();
			}
			item_ptr[nd]->setDefinedLength(definedLength);
		}
	;
    break;}
case 272:
#line 3721 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store item - attribute value
			item_ptr[nd]->AddAttribute(attribute_ptr[nd]);
		}
	;
    break;}
case 274:
#line 3734 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			// look up vr based on tag - need to look-up the VR in the definitions
			vr = DEFINITION->GetAttributeVr(group, element);

			if (vr == ATTR_VR_SQ)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Attribute VR of SQ must be specified explicity");
					logger_ptr->text(LOG_NONE, 1, "as (TAG, SQ, ...) - Script line no: %d", scriptlineno);
				}
				YYABORT;
			}
			else if (vr == ATTR_VR_UN)
			{
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Attribute (%04X,%04X) unknown in definitions - VR must be specified explicity", group, element);
					logger_ptr->text(LOG_NONE, 1, "- on Script line no: %d", scriptlineno);
				}
				YYABORT;
			}

			// store attribute vr
			attribute_ptr[nd]->SetVR(vr);
			attribute_ptr[nd]->setDefinedLength(false);
		}
	;
    break;}
case 275:
#line 3767 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store attribute vr
			attribute_ptr[nd]->SetVR(vr);
			attribute_ptr[nd]->setTransferVR(transferVr);
			attribute_ptr[nd]->setDefinedLength(definedLength);
		}
	;
    break;}
case 276:
#line 3779 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = scriptlval.vr;
			transferVr = TRANSFER_ATTR_VR_IMPLICIT;
			definedLength = false;
		}
	;
    break;}
case 277:
#line 3789 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = scriptlval.vr;
			transferVr = TRANSFER_ATTR_VR_IMPLICIT;
			definedLength = true;
		}
	;
    break;}
case 278:
#line 3799 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = scriptlval.vr;
			transferVr = TRANSFER_ATTR_VR_UNKNOWN;
			definedLength = false;
		}
	;
    break;}
case 279:
#line 3809 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = scriptlval.vr;
			transferVr = TRANSFER_ATTR_VR_UNKNOWN;
			definedLength = true;
		}
	;
    break;}
case 280:
#line 3819 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = scriptlval.vr;
			transferVr = TRANSFER_ATTR_VR_QUESTION;
			definedLength = false;
		}
	;
    break;}
case 281:
#line 3829 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = scriptlval.vr;
			transferVr = TRANSFER_ATTR_VR_QUESTION;
			definedLength = true;
		}
	;
    break;}
case 284:
#line 3845 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store attribute value
			if (value_ptr != NULL)
			{
				// check if the attribute is Other Data and has values
				if (((attribute_ptr[nd]->GetVR() == ATTR_VR_OB) ||
					(attribute_ptr[nd]->GetVR() == ATTR_VR_OF) ||
					(attribute_ptr[nd]->GetVR() == ATTR_VR_OW)) &&
					(attribute_ptr[nd]->GetNrValues() > 0))
				{
					// get next data value and add to existing values
					UINT32 data;
					value_ptr->Get(data);
					BASE_VALUE_CLASS *destValue_ptr = attribute_ptr[nd]->GetValue(0);
					destValue_ptr->Add(data);
				}
				else
				{
					// add new attribute value
					attribute_ptr[nd]->AddValue(value_ptr);
				}
			}
		}
	;
    break;}
case 285:
#line 3872 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store attribute value
			if (value_ptr != NULL)
			{
				// check if the attribute is Other Data and has values
				if (((attribute_ptr[nd]->GetVR() == ATTR_VR_OB) ||
					(attribute_ptr[nd]->GetVR() == ATTR_VR_OF) ||
					(attribute_ptr[nd]->GetVR() == ATTR_VR_OW)) &&
					(attribute_ptr[nd]->GetNrValues() > 0))
				{
					// get next data value and add to existing values
					UINT32 data;
					value_ptr->Get(data);
					BASE_VALUE_CLASS *destValue_ptr = attribute_ptr[nd]->GetValue(0);
					destValue_ptr->Add(data);
				}
				else
				{
					// add new attribute value
					attribute_ptr[nd]->AddValue(value_ptr);
				}
			}
		}
	;
    break;}
case 286:
#line 3901 "script_parser.y"
{
        if (!scriptParseOnly)
		{
			// special check for the Image Display Format ID / Pixel Data / FMI version attribute
			// - these need to be handled differently for the ADVT scripts
			if (((group == 0x2010) && 
				(element == 0x0010)) ||
				((group == 0x7FE0) &&
				(element == 0x0010)) ||
				((group == 0x0002) &&
				(element == 0x0001)))
			{
				convertDoubleBackslash(scriptvsp[0].string_ptr);
				
				// store string attribute value locally
				value_ptr = stringValue(scriptSession_ptr, scriptvsp[0].string_ptr, vr, group, element);
			}
			else if (vr == ATTR_VR_UN)
			{
				// although we have parsed a string from the DS Script - the UN value
				// should be interpreted as a byte array - this can contain byte values of 0
				// (zero) which would normally be seen as a string terminator bu tin this
				// case must be stored as part of the byte array
				value_ptr = byteArrayValue(scriptvsp[0].string_ptr);
			}
			else
			{
				convertHex(scriptvsp[0].string_ptr);
				
				// store string attribute value locally
				value_ptr = stringValue(scriptSession_ptr, scriptvsp[0].string_ptr, vr, group, element);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 287:
#line 3939 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store hex attribute value locally
			value_ptr = hexValue(scriptSession_ptr, scriptvsp[0].hex, vr, group, element);
		}
	;
    break;}
case 288:
#line 3947 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// store integer attribute value locally
			value_ptr = integerValue(scriptSession_ptr, scriptvsp[0].integer, vr, group, element);
		}
	;
    break;}
case 289:
#line 3955 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// autoset attribute value
			value_ptr = autoSetValue(scriptSession_ptr, vr, group, element);
		}
	;
    break;}
case 290:
#line 3964 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// copy tag to reference one
			group1 = group;
			element1 = element;
		}
	;
    break;}
case 291:
#line 3975 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// copy tag to reference two
			group2 = group;
			element2 = element;
		}
	;
    break;}
case 292:
#line 3986 "script_parser.y"
{
	;
    break;}
case 293:
#line 3989 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// look up tag by name and get group & element returned
			group = 0;
			element = 0;
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 294:
#line 4003 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if (command_ptr)
			{
				// get reference to this command
				wid1_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), command_ptr->getIdentifier(), command_ptr->getWidType());

				if (wid1_ptr == NULL)
				{
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Can't find Command %s in Data Warehouse", command_ptr->getIdentifier());
					}
				}

				// clean up - side effect of parser
				delete command_ptr;
				command_ptr = NULL;
			}
		}
	;
    break;}
case 295:
#line 4028 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if (dataset_ptr)
			{
				// get reference to this dataset
				wid1_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), dataset_ptr->getWidType());

				//if we did not find the datset object try for an item
				if (wid1_ptr == NULL)
				{
					wid1_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), WID_ITEM);
				}

				// if we did not find the dataset object or item - try for an item handle
				if (wid1_ptr == NULL)
				{
					wid1_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), WID_ITEM_HANDLE);
				}

				if (wid1_ptr == NULL)
				{
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Can't find Dataset/Item %s in Data Warehouse", dataset_ptr->getIdentifier());
					}
				}

				// clean up - side effect of parser
				delete dataset_ptr;
				dataset_ptr = NULL;
			}
		}
	;
    break;}
case 296:
#line 4067 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if (command_ptr)
			{
				// get reference to this command
				wid2_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), command_ptr->getIdentifier(), command_ptr->getWidType());

				if (wid2_ptr == NULL)
				{
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Can't find Command %s in Data Warehouse", command_ptr->getIdentifier());
					}
				}

				// clean up - side effect of parser
				delete command_ptr;
				command_ptr = NULL;
			}
		}
	;
    break;}
case 297:
#line 4092 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if (dataset_ptr)
			{
				// get reference to this dataset
				wid2_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), dataset_ptr->getWidType());

				// if we did not find the datset object try for an item
				if (wid2_ptr == NULL)
				{
					wid2_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), WID_ITEM);
				}

				// if we did not find the dataset object or item - try for an item handle
				if (wid2_ptr == NULL)
				{
					wid2_ptr = retrieveFromWarehouse(scriptSession_ptr->getLogger(), dataset_ptr->getIdentifier(), WID_ITEM_HANDLE);
				}

				if (wid2_ptr == NULL)
				{
					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Can't find Dataset/Item %s in Data Warehouse", dataset_ptr->getIdentifier());
					}
				}

				// clean up - side effect of parser
				delete dataset_ptr;
				dataset_ptr = NULL;
			}
		}
	;
    break;}
case 298:
#line 4131 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// ensure that we have an item handle
			if (item_handle_ptr == NULL)
			{
				item_handle_ptr = new ITEM_HANDLE_CLASS();
			}

			// add the sequence reference to the item handle
			SEQUENCE_REF_CLASS *sq_ref_ptr = new SEQUENCE_REF_CLASS(dataset_ptr->getIodName(), dataset_ptr->getIdentifier(), group, element, itemNumber);
			item_handle_ptr->add(sq_ref_ptr);

			// delete the dataset
			delete dataset_ptr;
			dataset_ptr = NULL;
		}
	;
    break;}
case 299:
#line 4150 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// ensure that we have an item handle
			if (item_handle_ptr == NULL)
			{
				item_handle_ptr = new ITEM_HANDLE_CLASS();
			}

			// add the sequence reference to the item handle
			SEQUENCE_REF_CLASS *sq_ref_ptr = new SEQUENCE_REF_CLASS(group, element, itemNumber);
			item_handle_ptr->add(sq_ref_ptr);
		}
	;
    break;}
case 300:
#line 4167 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// default Item Number is 1 - index is then zero 
			itemNumber = 0;
		}
	;
    break;}
case 301:
#line 4175 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			// take given Item Number - index is one less
			itemNumber = scriptvsp[-1].integer - 1;
		}
	;
    break;}
case 304:
#line 4189 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			fileHead_ptr = new FILEHEAD_CLASS();

			// cascade the logger
			fileHead_ptr->setLogger(scriptSession_ptr->getLogger());

			identifier = "";
		}
	;
    break;}
case 310:
#line 4212 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			if (fileHead_ptr)
			{
				fileHead_ptr->setPreambleValue(scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 311:
#line 4228 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			convertHex(scriptvsp[0].string_ptr);
			if (fileHead_ptr)
			{
				fileHead_ptr->setPrefix(scriptvsp[0].string_ptr);
			}
		}
		// free malloced string buffer
		free(scriptvsp[0].string_ptr);
		scriptvsp[0].string_ptr = NULL;
	;
    break;}
case 312:
#line 4244 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileHead_ptr)
			{
				fileHead_ptr->setTransferSyntaxUid(transferSyntaxName.getUid());
			}
		}
	;
    break;}
case 315:
#line 4260 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			fileTail_ptr = new FILETAIL_CLASS();

			// cascade the logger
			fileTail_ptr->setLogger(scriptSession_ptr->getLogger());

			identifier = "";
		}
	;
    break;}
case 321:
#line 4283 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setTrailingPadding(true);
			}
		}
	;
    break;}
case 322:
#line 4293 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setTrailingPadding(false);
			}
		}
	;
    break;}
case 323:
#line 4305 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setSectorSize(scriptvsp[0].integer);
			}
		}
	;
    break;}
case 324:
#line 4317 "script_parser.y"
{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setPaddingValue((BYTE)scriptvsp[0].integer);
			}
		}
	;
    break;}
}
   /* the action file gets copied in in place of this dollarsign */
#line 487 "bison.simple"

  scriptvsp -= yylen;
  scriptssp -= yylen;
#ifdef YYLSP_NEEDED
  scriptlsp -= yylen;
#endif

#if YYDEBUG != 0
  if (scriptdebug)
    {
      short *ssp1 = scriptss - 1;
      fprintf (stderr, "state stack now");
      while (ssp1 != scriptssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

  *++scriptvsp = scriptval;

#ifdef YYLSP_NEEDED
  scriptlsp++;
  if (yylen == 0)
    {
      scriptlsp->first_line = yylloc.first_line;
      scriptlsp->first_column = yylloc.first_column;
      scriptlsp->last_line = (scriptlsp-1)->last_line;
      scriptlsp->last_column = (scriptlsp-1)->last_column;
      scriptlsp->text = 0;
    }
  else
    {
      scriptlsp->last_line = (scriptlsp+yylen-1)->last_line;
      scriptlsp->last_column = (scriptlsp+yylen-1)->last_column;
    }
#endif

  /* Now "shift" the result of the reduction.
     Determine what state that goes to,
     based on the state we popped back to
     and the rule number reduced by.  */

  yyn = scriptr1[yyn];

  scriptstate = scriptpgoto[yyn - YYNTBASE] + *scriptssp;
  if (scriptstate >= 0 && scriptstate <= YYLAST && yycheck[scriptstate] == *scriptssp)
    scriptstate = yytable[scriptstate];
  else
    scriptstate = scriptdefgoto[yyn - YYNTBASE];

  goto yynewstate;

scripterrlab:   /* here on detecting error */

  if (! yyerrstatus)
    /* If not already recovering from an error, report this error.  */
    {
      ++scriptnerrs;

#ifdef YYERROR_VERBOSE
      yyn = scriptpact[scriptstate];

      if (yyn > YYFLAG && yyn < YYLAST)
	{
	  int size = 0;
	  char *msg;
	  int x, count;

	  count = 0;
	  /* Start X at -yyn if nec to avoid negative indexes in yycheck.  */
	  for (x = (yyn < 0 ? -yyn : 0);
	       x < (sizeof(yytname) / sizeof(char *)); x++)
	    if (yycheck[x + yyn] == x)
	      size += strlen(yytname[x]) + 15, count++;
	  msg = (char *) malloc(size + 15);
	  if (msg != 0)
	    {
	      strcpy(msg, "parse error");

	      if (count < 5)
		{
		  count = 0;
		  for (x = (yyn < 0 ? -yyn : 0);
		       x < (sizeof(yytname) / sizeof(char *)); x++)
		    if (yycheck[x + yyn] == x)
		      {
			strcat(msg, count == 0 ? ", expecting `" : " or `");
			strcat(msg, yytname[x]);
			strcat(msg, "'");
			count++;
		      }
		}
	      scripterror(msg);
	      free(msg);
	    }
	  else
	    scripterror ("parse error; also virtual memory exceeded");
	}
      else
#endif /* YYERROR_VERBOSE */
	scripterror("parse error");
    }

  goto scripterrlab1;
scripterrlab1:   /* here on error raised explicitly by an action */

  if (yyerrstatus == 3)
    {
      /* if just tried and failed to reuse lookahead token after an error, discard it.  */

      /* return failure if at end of input */
      if (scriptchar == YYEOF)
	YYABORT;

#if YYDEBUG != 0
      if (scriptdebug)
	fprintf(stderr, "Discarding token %d (%s).\n", scriptchar, yytname[scriptchar1]);
#endif

      scriptchar = YYEMPTY;
    }

  /* Else will try to reuse lookahead token
     after shifting the error token.  */

  yyerrstatus = 3;		/* Each real token shifted decrements this */

  goto yyerrhandle;

yyerrdefault:  /* current state does not do anything special for the error token. */

#if 0
  /* This is wrong; only states that explicitly want error tokens
     should shift them.  */
  yyn = scriptdefact[scriptstate];  /* If its default is to accept any token, ok.  Otherwise pop it.*/
  if (yyn) goto scriptdefault;
#endif

yyerrpop:   /* pop the current state because it cannot handle the error token */

  if (scriptssp == scriptss) YYABORT;
  scriptvsp--;
  scriptstate = *--scriptssp;
#ifdef YYLSP_NEEDED
  scriptlsp--;
#endif

#if YYDEBUG != 0
  if (scriptdebug)
    {
      short *ssp1 = scriptss - 1;
      fprintf (stderr, "Error: state stack now");
      while (ssp1 != scriptssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

yyerrhandle:

  yyn = scriptpact[scriptstate];
  if (yyn == YYFLAG)
    goto yyerrdefault;

  yyn += YYTERROR;
  if (yyn < 0 || yyn > YYLAST || yycheck[yyn] != YYTERROR)
    goto yyerrdefault;

  yyn = yytable[yyn];
  if (yyn < 0)
    {
      if (yyn == YYFLAG)
	goto yyerrpop;
      yyn = -yyn;
      goto yyreduce;
    }
  else if (yyn == 0)
    goto yyerrpop;

  if (yyn == YYFINAL)
    YYACCEPT;

#if YYDEBUG != 0
  if (scriptdebug)
    fprintf(stderr, "Shifting error token, ");
#endif

  *++scriptvsp = scriptlval;
#ifdef YYLSP_NEEDED
  *++scriptlsp = yylloc;
#endif

  scriptstate = yyn;
  goto yynewstate;
}
#line 4327 "script_parser.y"


