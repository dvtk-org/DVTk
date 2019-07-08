
/*  A Bison parser, made from session_parser.y with Bison version GNU Bison version 1.24
  */

#define YYBISON 1  /* Identify Bison output.  */

#define	T_SESSION	258
#define	T_ENDSESSION	259
#define	T_SESSION_FILE_VERSION	260
#define	T_SESSION_TYPE	261
#define	T_SESSION_TITLE	262
#define	T_SESSION_ID	263
#define	T_MANUFACT	264
#define	T_MODEL	265
#define	T_SOFTWARE	266
#define	T_TESTED	267
#define	T_DATE	268
#define	T_APPL_ENTITY_NAME	269
#define	T_APPL_ENTITY_VERSION	270
#define	T_PRODUCT_ROLE_ACCEPTOR	271
#define	T_PRODUCT_ROLE_REQUESTOR	272
#define	T_CALLED_AE	273
#define	T_CALLING_AE	274
#define	T_MAX_LEN	275
#define	T_IMPL_CLASS	276
#define	T_IMPL_VER	277
#define	T_REMOTE_HOST_NAME	278
#define	T_PORT	279
#define	T_REMOTE_PORT	280
#define	T_LOCAL_PORT	281
#define	T_SOCKET_TIMEOUT	282
#define	T_USE_SECURE_SOCKETS	283
#define	T_TLS_VERSION	284
#define	T_CHECK_REMOTE_CERTIFICATE	285
#define	T_CIPHER_LIST	286
#define	T_CACHE_TLS_SESSIONS	287
#define	T_TLS_CACHE_TIMEOUT	288
#define	T_DELAY	289
#define	T_CREDENTIALS_FILENAME	290
#define	T_CERTIFICATE_FILENAME	291
#define	T_VAL_RES	292
#define	T_DIMSE_MSG	293
#define	T_ACSE_MSG	294
#define	T_PDU_DUMP	295
#define	T_LABEL_DUMP	296
#define	T_IMAGE_SAVE	297
#define	T_STORAGE_MODE	298
#define	T_DEF_SQ_LENGTH	299
#define	T_ADD_GROUP_LENGTH	300
#define	T_SUPPORTED_TRANSFER_SYNTAX	301
#define	T_FORMAT	302
#define	T_STRICT	303
#define	T_DATA_DIRECTORY	304
#define	T_UN_VR_DEF_LOOKUP	305
#define	T_AUTO_TYPE	306
#define	T_AUTO_CREATE	307
#define	T_DEFINITION_DIRECTORY	308
#define	T_DEFINITION_ROOT	309
#define	T_DEFINITION	310
#define	T_DICOMSCRIPT_ROOT	311
#define	T_DICOMSCRIPT	312
#define	T_LOG_ERROR	313
#define	T_LOG_WARNING	314
#define	T_LOG_INFO	315
#define	T_LOG_RELATION	316
#define	T_LOG_DEBUG	317
#define	T_LOG_ACSE	318
#define	T_LOG_DICOM	319
#define	T_LOG_SCP_THREAD	320
#define	T_DULP_STATE	321
#define	T_CONTINUE	322
#define	T_DICOM_VAL_RES	323
#define	T_ACSE_VAL_RES	324
#define	T_RESULTS_ROOT	325
#define	T_DESCRIPTION_DIRECTORY	326
#define	T_APPEND_TO_FILE	327
#define	T_INCLUDE_PATH	328
#define	TRUE_OR_FALSE	329
#define	INTEGER	330
#define	STRING	331
#define	LOG_LEVEL	332
#define	SESSION_TYPE	333
#define	STORAGE_MODE	334
#define	SUT_ROLE	335
#define	T_ENSURE_EVEN_ATTRIBUTE_VALUE_LENGTH	336
#define	T_SUT_AE	337
#define	T_SUT_MAX_LEN	338
#define	T_SUT_IMPL_CLASS	339
#define	T_SUT_IMPL_VER	340
#define	T_SUT_HOSTNAME	341
#define	T_SUT_PORT	342
#define	T_SUT_ROLE	343
#define	T_DVT_AE	344
#define	T_DVT_MAX_LEN	345
#define	T_DVT_IMPL_CLASS	346
#define	T_DVT_IMPL_VER	347
#define	T_DVT_PORT	348
#define	T_DVT_SOCKET_TIMEOUT	349
#define	T_DETAILED_RESULTS	350
#define	T_SUMMARY_RESULTS	351
#define	T_INCLUDE_TYPE3_NOTPRESENT	352
#define	T_DUMP_ATTR_REFFILE	353
#define	T_PRIVATE_MAPPING	354

#line 1 "session_parser.y"

// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2006
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

//*****************************************************************************
//  DESCRIPTION     :	Test Session Parser.
//*****************************************************************************

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global component interface
#include "Idefinition.h"
#include "Ilog.h"				// Log component interface
#include "Iscripting.h"
#include "Iutility.h"			// Utility component interface
#include "session.h"			// Base Test Session

extern void				sessionerror(char*);
extern int				sessionlex(void);
extern int				sessionlineno;

extern BASE_SESSION_CLASS	*session_ptr;


#line 48 "session_parser.y"
typedef union {
	bool				trueOrFalse;
	int					integer;
	char				string[MAX_STRING_LEN];
	SESSION_TYPE_ENUM	sessionType;
	STORAGE_MODE_ENUM	storageMode;
	UP_ENUM				sutRole;
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



#define	YYFINAL		190
#define	YYFLAG		-32768
#define	YYNTBASE	100

#define YYTRANSLATE(x) ((unsigned)(x) <= 354 ? yytranslate[x] : 108)

static const char yytranslate[] = {     0,
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
    76,    77,    78,    79,    80,    81,    82,    83,    84,    85,
    86,    87,    88,    89,    90,    91,    92,    93,    94,    95,
    96,    97,    98,    99
};

#if YYDEBUG != 0
static const short yyprhs[] = {     0,
     0,     1,     3,     5,     8,    10,    14,    16,    18,    20,
    23,    26,    29,    32,    35,    38,    41,    44,    47,    50,
    53,    56,    59,    62,    65,    68,    71,    74,    77,    80,
    83,    86,    89,    92,    95,    98,   101,   104,   107,   110,
   113,   116,   119,   122,   125,   128,   131,   134,   137,   140,
   143,   146,   149,   152,   155,   158,   161,   164,   167,   170,
   173,   176,   179,   182,   185,   188,   191,   194,   197,   200,
   203,   206,   209,   212,   215,   218,   221,   224,   227,   230,
   233,   236,   239,   242,   245,   248,   251,   254,   257,   260,
   263,   266,   269,   272,   275,   278,   281,   284
};

static const short yyrhs[] = {    -1,
   101,     0,   102,     0,   101,   102,     0,   103,     0,   104,
   106,   105,     0,     3,     0,     4,     0,   107,     0,   106,
   107,     0,     5,    75,     0,     6,    78,     0,     7,    76,
     0,     8,    75,     0,     9,    76,     0,    10,    76,     0,
    11,    76,     0,    14,    76,     0,    15,    76,     0,    12,
    76,     0,    13,    76,     0,    16,    74,     0,    17,    74,
     0,    18,    76,     0,    19,    76,     0,    20,    75,     0,
    21,    76,     0,    22,    76,     0,    23,    76,     0,    24,
    75,     0,    25,    75,     0,    26,    75,     0,    27,    75,
     0,    82,    76,     0,    83,    75,     0,    84,    76,     0,
    85,    76,     0,    86,    76,     0,    87,    75,     0,    88,
    80,     0,    89,    76,     0,    90,    75,     0,    91,    76,
     0,    92,    76,     0,    93,    75,     0,    94,    75,     0,
    28,    74,     0,    29,    76,     0,    30,    74,     0,    31,
    76,     0,    32,    74,     0,    33,    75,     0,    34,    75,
     0,    35,    76,     0,    36,    76,     0,    37,    74,     0,
    38,    74,     0,    39,    74,     0,    40,    74,     0,    41,
    74,     0,    42,    74,     0,    44,    74,     0,    45,    74,
     0,    67,    74,     0,    46,    76,     0,    47,    74,     0,
    43,    79,     0,    48,    74,     0,    95,    74,     0,    96,
    74,     0,    97,    74,     0,    98,    74,     0,    99,    74,
     0,    51,    74,     0,    52,    74,     0,    81,    74,     0,
    58,    74,     0,    59,    74,     0,    60,    74,     0,    61,
    74,     0,    62,    74,     0,    64,    77,     0,    68,    77,
     0,    63,    77,     0,    65,    74,     0,    66,    74,     0,
    69,    77,     0,    53,    76,     0,    49,    76,     0,    50,
    74,     0,    54,    76,     0,    55,    76,     0,    56,    76,
     0,    57,    76,     0,    70,    76,     0,    71,    76,     0,
    72,    74,     0,    73,    76,     0
};

#endif

#if YYDEBUG != 0
static const short yyrline[] = { 0,
    66,    67,    70,    71,    74,    77,    80,    83,    86,    87,
    90,    94,    98,   102,   106,   110,   114,   118,   122,   126,
   130,   134,   138,   142,   146,   150,   154,   158,   162,   166,
   172,   176,   180,   184,   188,   192,   196,   200,   204,   208,
   212,   216,   220,   224,   228,   232,   236,   240,   244,   248,
   252,   256,   260,   264,   268,   272,   276,   280,   284,   288,
   292,   303,   307,   311,   315,   319,   333,   337,   341,   345,
   349,   353,   357,   361,   365,   369,   373,   377,   381,   385,
   389,   393,   397,   401,   405,   409,   413,   417,   421,   425,
   429,   435,   463,   467,   478,   482,   486,   490
};

static const char * const yytname[] = {   "$","error","$undefined.","T_SESSION",
"T_ENDSESSION","T_SESSION_FILE_VERSION","T_SESSION_TYPE","T_SESSION_TITLE","T_SESSION_ID",
"T_MANUFACT","T_MODEL","T_SOFTWARE","T_TESTED","T_DATE","T_APPL_ENTITY_NAME",
"T_APPL_ENTITY_VERSION","T_PRODUCT_ROLE_ACCEPTOR","T_PRODUCT_ROLE_REQUESTOR",
"T_CALLED_AE","T_CALLING_AE","T_MAX_LEN","T_IMPL_CLASS","T_IMPL_VER","T_REMOTE_HOST_NAME",
"T_PORT","T_REMOTE_PORT","T_LOCAL_PORT","T_SOCKET_TIMEOUT","T_USE_SECURE_SOCKETS",
"T_TLS_VERSION","T_CHECK_REMOTE_CERTIFICATE","T_CIPHER_LIST","T_CACHE_TLS_SESSIONS",
"T_TLS_CACHE_TIMEOUT","T_DELAY","T_CREDENTIALS_FILENAME","T_CERTIFICATE_FILENAME",
"T_VAL_RES","T_DIMSE_MSG","T_ACSE_MSG","T_PDU_DUMP","T_LABEL_DUMP","T_IMAGE_SAVE",
"T_STORAGE_MODE","T_DEF_SQ_LENGTH","T_ADD_GROUP_LENGTH","T_SUPPORTED_TRANSFER_SYNTAX",
"T_FORMAT","T_STRICT","T_DATA_DIRECTORY","T_UN_VR_DEF_LOOKUP","T_AUTO_TYPE",
"T_AUTO_CREATE","T_DEFINITION_DIRECTORY","T_DEFINITION_ROOT","T_DEFINITION",
"T_DICOMSCRIPT_ROOT","T_DICOMSCRIPT","T_LOG_ERROR","T_LOG_WARNING","T_LOG_INFO",
"T_LOG_RELATION","T_LOG_DEBUG","T_LOG_ACSE","T_LOG_DICOM","T_LOG_SCP_THREAD",
"T_DULP_STATE","T_CONTINUE","T_DICOM_VAL_RES","T_ACSE_VAL_RES","T_RESULTS_ROOT",
"T_DESCRIPTION_DIRECTORY","T_APPEND_TO_FILE","T_INCLUDE_PATH","TRUE_OR_FALSE",
"INTEGER","STRING","LOG_LEVEL","SESSION_TYPE","STORAGE_MODE","SUT_ROLE","T_ENSURE_EVEN_ATTRIBUTE_VALUE_LENGTH",
"T_SUT_AE","T_SUT_MAX_LEN","T_SUT_IMPL_CLASS","T_SUT_IMPL_VER","T_SUT_HOSTNAME",
"T_SUT_PORT","T_SUT_ROLE","T_DVT_AE","T_DVT_MAX_LEN","T_DVT_IMPL_CLASS","T_DVT_IMPL_VER",
"T_DVT_PORT","T_DVT_SOCKET_TIMEOUT","T_DETAILED_RESULTS","T_SUMMARY_RESULTS",
"T_INCLUDE_TYPE3_NOTPRESENT","T_DUMP_ATTR_REFFILE","T_PRIVATE_MAPPING","Language",
"LanguageGrammar","LanguageComponents","Session","BeginSession","EndSession",
"SessionContentList","SessionContent",""
};
#endif

static const short sessionr1[] = {     0,
   100,   100,   101,   101,   102,   103,   104,   105,   106,   106,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107,   107,   107,
   107,   107,   107,   107,   107,   107,   107,   107
};

static const short sessionr2[] = {     0,
     0,     1,     1,     2,     1,     3,     1,     1,     1,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2
};

static const short sessiondefact[] = {     1,
     7,     2,     3,     5,     0,     4,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     0,     0,     0,     0,     0,
     0,     0,     0,     0,     0,     9,    11,    12,    13,    14,
    15,    16,    17,    20,    21,    18,    19,    22,    23,    24,
    25,    26,    27,    28,    29,    30,    31,    32,    33,    47,
    48,    49,    50,    51,    52,    53,    54,    55,    56,    57,
    58,    59,    60,    61,    67,    62,    63,    65,    66,    68,
    89,    90,    74,    75,    88,    91,    92,    93,    94,    77,
    78,    79,    80,    81,    84,    82,    85,    86,    64,    83,
    87,    95,    96,    97,    98,    76,    34,    35,    36,    37,
    38,    39,    40,    41,    42,    43,    44,    45,    46,    69,
    70,    71,    72,    73,     8,     6,    10,     0,     0,     0
};

static const short sessiondefgoto[] = {   188,
     2,     3,     4,     5,   186,    95,    96
};

static const short sessionpact[] = {    67,
-32768,    67,-32768,-32768,    91,-32768,    -3,    -7,    -2,     0,
    89,    90,    92,    93,    94,    95,   115,    -1,     2,   116,
   117,   119,   120,   121,   122,   124,   125,   126,   127,   129,
   128,   131,   130,   133,   134,   135,   132,   136,   137,   139,
   140,   141,   142,   143,    88,   144,   145,   146,   147,   149,
   148,   151,   152,   153,   154,   155,   156,   157,   158,   161,
   162,   163,   164,   165,   118,   166,   167,   168,   170,   169,
   171,   173,   174,   177,   176,   179,   178,   172,   180,   181,
   182,   184,   160,   185,   187,   188,   189,   191,   192,   186,
   194,   195,   196,   197,    -4,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,   220,   228,-32768
};

static const short sessionpgoto[] = {-32768,
-32768,   227,-32768,-32768,-32768,-32768,   150
};


#define	YYLAST		271


static const short yytable[] = {   185,
     7,     8,     9,    10,    11,    12,    13,    14,    15,    16,
    17,    18,    19,    20,    21,    22,    23,    24,    25,    26,
    27,    28,    29,    30,    31,    32,    33,    34,    35,    36,
    37,    38,    39,    40,    41,    42,    43,    44,    45,    46,
    47,    48,    49,    50,    51,    52,    53,    54,    55,    56,
    57,    58,    59,    60,    61,    62,    63,    64,    65,    66,
    67,    68,    69,    70,    71,    72,    73,    74,    75,     1,
    98,    97,   108,    99,   100,   109,    76,    77,    78,    79,
    80,    81,    82,    83,    84,    85,    86,    87,    88,    89,
    90,    91,    92,    93,    94,     7,     8,     9,    10,    11,
    12,    13,    14,    15,    16,    17,    18,    19,    20,    21,
    22,    23,    24,    25,    26,    27,    28,    29,    30,    31,
    32,    33,    34,    35,    36,    37,    38,    39,    40,    41,
    42,    43,    44,    45,    46,    47,    48,    49,    50,    51,
    52,    53,    54,    55,    56,    57,    58,    59,    60,    61,
    62,    63,    64,    65,    66,    67,    68,    69,    70,    71,
    72,    73,    74,    75,   101,   102,   135,   103,   104,   105,
   106,    76,    77,    78,    79,    80,    81,    82,    83,    84,
    85,    86,    87,    88,    89,    90,    91,    92,    93,    94,
   107,   110,   111,   112,   155,   113,   114,   115,   116,   117,
   118,   119,   120,   121,   122,   123,   124,   127,   125,   126,
   129,   128,   130,   131,   132,   133,   134,   136,   137,   189,
   139,   138,   140,   141,   142,   143,   144,   190,     6,   145,
   146,   147,   148,   149,   150,   151,   152,   153,   154,   173,
   157,   158,   156,   159,   187,   160,   168,   161,   162,   163,
   164,   165,   166,   167,     0,   169,   170,   171,   172,   180,
   174,   175,     0,   176,   177,   178,   179,   181,   182,   183,
   184
};

static const short yycheck[] = {     4,
     5,     6,     7,     8,     9,    10,    11,    12,    13,    14,
    15,    16,    17,    18,    19,    20,    21,    22,    23,    24,
    25,    26,    27,    28,    29,    30,    31,    32,    33,    34,
    35,    36,    37,    38,    39,    40,    41,    42,    43,    44,
    45,    46,    47,    48,    49,    50,    51,    52,    53,    54,
    55,    56,    57,    58,    59,    60,    61,    62,    63,    64,
    65,    66,    67,    68,    69,    70,    71,    72,    73,     3,
    78,    75,    74,    76,    75,    74,    81,    82,    83,    84,
    85,    86,    87,    88,    89,    90,    91,    92,    93,    94,
    95,    96,    97,    98,    99,     5,     6,     7,     8,     9,
    10,    11,    12,    13,    14,    15,    16,    17,    18,    19,
    20,    21,    22,    23,    24,    25,    26,    27,    28,    29,
    30,    31,    32,    33,    34,    35,    36,    37,    38,    39,
    40,    41,    42,    43,    44,    45,    46,    47,    48,    49,
    50,    51,    52,    53,    54,    55,    56,    57,    58,    59,
    60,    61,    62,    63,    64,    65,    66,    67,    68,    69,
    70,    71,    72,    73,    76,    76,    79,    76,    76,    76,
    76,    81,    82,    83,    84,    85,    86,    87,    88,    89,
    90,    91,    92,    93,    94,    95,    96,    97,    98,    99,
    76,    76,    76,    75,    77,    76,    76,    76,    75,    75,
    75,    75,    74,    76,    74,    76,    74,    76,    75,    75,
    74,    76,    74,    74,    74,    74,    74,    74,    74,     0,
    74,    76,    74,    76,    74,    74,    74,     0,     2,    76,
    76,    76,    76,    76,    74,    74,    74,    74,    74,    80,
    74,    74,    77,    74,    95,    77,    75,    77,    76,    76,
    74,    76,    74,    76,    -1,    76,    76,    76,    75,    74,
    76,    75,    -1,    76,    76,    75,    75,    74,    74,    74,
    74
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

#define sessionerrok		(yyerrstatus = 0)
#define sessionclearin	(sessionchar = YYEMPTY)
#define YYEMPTY		-2
#define YYEOF		0
#define YYACCEPT	return(0)
#define YYABORT 	return(1)
#define YYERROR		goto sessionerrlab1
/* Like YYERROR except do call sessionerror.
   This remains here temporarily to ease the
   transition to the new meaning of YYERROR, for GCC.
   Once GCC version 2 has supplanted version 1, this can go.  */
#define YYFAIL		goto sessionerrlab
#define YYRECOVERING()  (!!yyerrstatus)
#define YYBACKUP(token, value) \
do								\
  if (sessionchar == YYEMPTY && yylen == 1)				\
    { sessionchar = (token), sessionlval = (value);			\
      sessionchar1 = YYTRANSLATE (sessionchar);				\
      YYPOPSTACK;						\
      goto sessionbackup;						\
    }								\
  else								\
    { sessionerror ("syntax error: cannot back up"); YYERROR; }	\
while (0)

#define YYTERROR	1
#define YYERRCODE	256

#ifndef YYPURE
#define YYLEX		sessionlex()
#endif

#ifdef YYPURE
#ifdef YYLSP_NEEDED
#ifdef YYLEX_PARAM
#define YYLEX		sessionlex(&sessionlval, &yylloc, YYLEX_PARAM)
#else
#define YYLEX		sessionlex(&sessionlval, &yylloc)
#endif
#else /* not YYLSP_NEEDED */
#ifdef YYLEX_PARAM
#define YYLEX		sessionlex(&sessionlval, YYLEX_PARAM)
#else
#define YYLEX		sessionlex(&sessionlval)
#endif
#endif /* not YYLSP_NEEDED */
#endif

/* If nonreentrant, generate the variables here */

#ifndef YYPURE

int	sessionchar;			/*  the lookahead symbol		*/
YYSTYPE	sessionlval;			/*  the semantic value of the		*/
				/*  lookahead symbol			*/

#ifdef YYLSP_NEEDED
YYLTYPE yylloc;			/*  location data for the lookahead	*/
				/*  symbol				*/
#endif

int sessionnerrs;			/*  number of parse errors so far       */
#endif  /* not YYPURE */

#if YYDEBUG != 0
int sessiondebug;			/*  nonzero means print parse trace	*/
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
int sessionparse (void);
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
   into sessionparse.  The argument should have type void *.
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
sessionparse(YYPARSE_PARAM)
     YYPARSE_PARAM_DECL
{
  register int sessionstate;
  register int yyn;
  register short *sessionssp;
  register YYSTYPE *sessionvsp;
  int yyerrstatus;	/*  number of tokens to shift before error messages enabled */
  int sessionchar1 = 0;		/*  lookahead token as an internal (translated) token number */

  short	sessionssa[YYINITDEPTH];	/*  the state stack			*/
  YYSTYPE sessionvsa[YYINITDEPTH];	/*  the semantic value stack		*/

  short *sessionss = sessionssa;		/*  refer to the stacks thru separate pointers */
  YYSTYPE *sessionvs = sessionvsa;	/*  to allow yyoverflow to reallocate them elsewhere */

#ifdef YYLSP_NEEDED
  YYLTYPE yylsa[YYINITDEPTH];	/*  the location stack			*/
  YYLTYPE *yyls = yylsa;
  YYLTYPE *sessionlsp;

#define YYPOPSTACK   (sessionvsp--, sessionssp--, sessionlsp--)
#else
#define YYPOPSTACK   (sessionvsp--, sessionssp--)
#endif

  int sessionstacksize = YYINITDEPTH;

#ifdef YYPURE
  int sessionchar;
  YYSTYPE sessionlval;
  int sessionnerrs;
#ifdef YYLSP_NEEDED
  YYLTYPE yylloc;
#endif
#endif

  YYSTYPE sessionval;		/*  the variable used to return		*/
				/*  semantic values from the action	*/
				/*  routines				*/

  int yylen;

#if YYDEBUG != 0
  if (sessiondebug)
    fprintf(stderr, "Starting parse\n");
#endif

  sessionstate = 0;
  yyerrstatus = 0;
  sessionnerrs = 0;
  sessionchar = YYEMPTY;		/* Cause a token to be read.  */

  /* Initialize stack pointers.
     Waste one element of value and location stack
     so that they stay on the same level as the state stack.
     The wasted elements are never initialized.  */

  sessionssp = sessionss - 1;
  sessionvsp = sessionvs;
#ifdef YYLSP_NEEDED
  sessionlsp = yyls;
#endif

/* Push a new state, which is found in  sessionstate  .  */
/* In all cases, when you get here, the value and location stacks
   have just been pushed. so pushing a state here evens the stacks.  */
yynewstate:

  *++sessionssp = sessionstate;

  if (sessionssp >= sessionss + sessionstacksize - 1)
    {
      /* Give user a chance to reallocate the stack */
      /* Use copies of these so that the &'s don't force the real ones into memory. */
      YYSTYPE *sessionvs1 = sessionvs;
      short *sessionss1 = sessionss;
#ifdef YYLSP_NEEDED
      YYLTYPE *yyls1 = yyls;
#endif

      /* Get the current used size of the three stacks, in elements.  */
      int size = sessionssp - sessionss + 1;

#ifdef yyoverflow
      /* Each stack pointer address is followed by the size of
	 the data in use in that stack, in bytes.  */
#ifdef YYLSP_NEEDED
      /* This used to be a conditional around just the two extra args,
	 but that might be undefined if yyoverflow is a macro.  */
      yyoverflow("parser stack overflow",
		 &sessionss1, size * sizeof (*sessionssp),
		 &sessionvs1, size * sizeof (*sessionvsp),
		 &yyls1, size * sizeof (*sessionlsp),
		 &sessionstacksize);
#else
      yyoverflow("parser stack overflow",
		 &sessionss1, size * sizeof (*sessionssp),
		 &sessionvs1, size * sizeof (*sessionvsp),
		 &sessionstacksize);
#endif

      sessionss = sessionss1; sessionvs = sessionvs1;
#ifdef YYLSP_NEEDED
      yyls = yyls1;
#endif
#else /* no yyoverflow */
      /* Extend the stack our own way.  */
      if (sessionstacksize >= YYMAXDEPTH)
	{
	  sessionerror("parser stack overflow");
	  return 2;
	}
      sessionstacksize *= 2;
      if (sessionstacksize > YYMAXDEPTH)
	sessionstacksize = YYMAXDEPTH;
      sessionss = (short *) alloca (sessionstacksize * sizeof (*sessionssp));
      __yy_memcpy ((char *)sessionss1, (char *)sessionss, size * sizeof (*sessionssp));
      sessionvs = (YYSTYPE *) alloca (sessionstacksize * sizeof (*sessionvsp));
      __yy_memcpy ((char *)sessionvs1, (char *)sessionvs, size * sizeof (*sessionvsp));
#ifdef YYLSP_NEEDED
      yyls = (YYLTYPE *) alloca (sessionstacksize * sizeof (*sessionlsp));
      __yy_memcpy ((char *)yyls1, (char *)yyls, size * sizeof (*sessionlsp));
#endif
#endif /* no yyoverflow */

      sessionssp = sessionss + size - 1;
      sessionvsp = sessionvs + size - 1;
#ifdef YYLSP_NEEDED
      sessionlsp = yyls + size - 1;
#endif

#if YYDEBUG != 0
      if (sessiondebug)
	fprintf(stderr, "Stack size increased to %d\n", sessionstacksize);
#endif

      if (sessionssp >= sessionss + sessionstacksize - 1)
	YYABORT;
    }

#if YYDEBUG != 0
  if (sessiondebug)
    fprintf(stderr, "Entering state %d\n", sessionstate);
#endif

  goto sessionbackup;
 sessionbackup:

/* Do appropriate processing given the current state.  */
/* Read a lookahead token if we need one and don't already have one.  */
/* yyresume: */

  /* First try to decide what to do without reference to lookahead token.  */

  yyn = sessionpact[sessionstate];
  if (yyn == YYFLAG)
    goto sessiondefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* sessionchar is either YYEMPTY or YYEOF
     or a valid token in external form.  */

  if (sessionchar == YYEMPTY)
    {
#if YYDEBUG != 0
      if (sessiondebug)
	fprintf(stderr, "Reading a token: ");
#endif
      sessionchar = YYLEX;
    }

  /* Convert token to internal form (in sessionchar1) for indexing tables with */

  if (sessionchar <= 0)		/* This means end of input. */
    {
      sessionchar1 = 0;
      sessionchar = YYEOF;		/* Don't call YYLEX any more */

#if YYDEBUG != 0
      if (sessiondebug)
	fprintf(stderr, "Now at end of input.\n");
#endif
    }
  else
    {
      sessionchar1 = YYTRANSLATE(sessionchar);

#if YYDEBUG != 0
      if (sessiondebug)
	{
	  fprintf (stderr, "Next token is %d (%s", sessionchar, yytname[sessionchar1]);
	  /* Give the individual parser a way to print the precise meaning
	     of a token, for further debugging info.  */
#ifdef YYPRINT
	  YYPRINT (stderr, sessionchar, sessionlval);
#endif
	  fprintf (stderr, ")\n");
	}
#endif
    }

  yyn += sessionchar1;
  if (yyn < 0 || yyn > YYLAST || yycheck[yyn] != sessionchar1)
    goto sessiondefault;

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
	goto sessionerrlab;
      yyn = -yyn;
      goto yyreduce;
    }
  else if (yyn == 0)
    goto sessionerrlab;

  if (yyn == YYFINAL)
    YYACCEPT;

  /* Shift the lookahead token.  */

#if YYDEBUG != 0
  if (sessiondebug)
    fprintf(stderr, "Shifting token %d (%s), ", sessionchar, yytname[sessionchar1]);
#endif

  /* Discard the token being shifted unless it is eof.  */
  if (sessionchar != YYEOF)
    sessionchar = YYEMPTY;

  *++sessionvsp = sessionlval;
#ifdef YYLSP_NEEDED
  *++sessionlsp = yylloc;
#endif

  /* count tokens shifted since error; after three, turn off error status.  */
  if (yyerrstatus) yyerrstatus--;

  sessionstate = yyn;
  goto yynewstate;

/* Do the default action for the current state.  */
sessiondefault:

  yyn = sessiondefact[sessionstate];
  if (yyn == 0)
    goto sessionerrlab;

/* Do a reduction.  yyn is the number of a rule to reduce with.  */
yyreduce:
  yylen = sessionr2[yyn];
  if (yylen > 0)
    sessionval = sessionvsp[1-yylen]; /* implement default value of the action */

#if YYDEBUG != 0
  if (sessiondebug)
    {
      int i;

      fprintf (stderr, "Reducing via rule %d (line %d), ",
	       yyn, yyrline[yyn]);

      /* Print the symbols being reduced, and their result.  */
      for (i = yyprhs[yyn]; yyrhs[i] > 0; i++)
	fprintf (stderr, "%s ", yytname[yyrhs[i]]);
      fprintf (stderr, " -> %s\n", yytname[sessionr1[yyn]]);
    }
#endif


  switch (yyn) {

case 11:
#line 91 "session_parser.y"
{
		session_ptr->sessionFileVersion(sessionvsp[0].integer);
	;
    break;}
case 12:
#line 95 "session_parser.y"
{
		session_ptr->setSessionType(sessionvsp[0].sessionType);
	;
    break;}
case 13:
#line 99 "session_parser.y"
{
		session_ptr->setSessionTitle(sessionvsp[0].string);
	;
    break;}
case 14:
#line 103 "session_parser.y"
{
		session_ptr->setSessionId(sessionvsp[0].integer);
	;
    break;}
case 15:
#line 107 "session_parser.y"
{
		session_ptr->setManufacturer(sessionvsp[0].string);
	;
    break;}
case 16:
#line 111 "session_parser.y"
{
		session_ptr->setModelName(sessionvsp[0].string);
	;
    break;}
case 17:
#line 115 "session_parser.y"
{
		session_ptr->setSoftwareVersions(sessionvsp[0].string);
	;
    break;}
case 18:
#line 119 "session_parser.y"
{
		session_ptr->setApplicationEntityName(sessionvsp[0].string);
	;
    break;}
case 19:
#line 123 "session_parser.y"
{
		session_ptr->setApplicationEntityVersion(sessionvsp[0].string);
	;
    break;}
case 20:
#line 127 "session_parser.y"
{
		session_ptr->setTestedBy(sessionvsp[0].string);
	;
    break;}
case 21:
#line 131 "session_parser.y"
{
		session_ptr->setDate(sessionvsp[0].string);
	;
    break;}
case 22:
#line 135 "session_parser.y"
{
		session_ptr->setProductRoleIsAcceptor(sessionvsp[0].trueOrFalse);
	;
    break;}
case 23:
#line 139 "session_parser.y"
{
		session_ptr->setProductRoleIsRequestor(sessionvsp[0].trueOrFalse);
	;
    break;}
case 24:
#line 143 "session_parser.y"
{
		session_ptr->setCalledAeTitle(sessionvsp[0].string);
	;
    break;}
case 25:
#line 147 "session_parser.y"
{
		session_ptr->setCallingAeTitle(sessionvsp[0].string);
	;
    break;}
case 26:
#line 151 "session_parser.y"
{
		session_ptr->setMaximumLengthReceived(sessionvsp[0].integer);
	;
    break;}
case 27:
#line 155 "session_parser.y"
{
		session_ptr->setImplementationClassUid(sessionvsp[0].string);
	;
    break;}
case 28:
#line 159 "session_parser.y"
{
		session_ptr->setImplementationVersionName(sessionvsp[0].string);
	;
    break;}
case 29:
#line 163 "session_parser.y"
{
		session_ptr->setRemoteHostname(sessionvsp[0].string);
	;
    break;}
case 30:
#line 167 "session_parser.y"
{
		// remain compatible with ADVT2.0
		session_ptr->setRemoteConnectPort((UINT16)sessionvsp[0].integer);
		session_ptr->setLocalListenPort((UINT16)sessionvsp[0].integer);
	;
    break;}
case 31:
#line 173 "session_parser.y"
{
		session_ptr->setRemoteConnectPort((UINT16)sessionvsp[0].integer);
	;
    break;}
case 32:
#line 177 "session_parser.y"
{
		session_ptr->setLocalListenPort((UINT16)sessionvsp[0].integer);
	;
    break;}
case 33:
#line 181 "session_parser.y"
{
		session_ptr->setSocketTimeout(sessionvsp[0].integer);
	;
    break;}
case 34:
#line 185 "session_parser.y"
{
		session_ptr->setSutAeTitle(sessionvsp[0].string);
	;
    break;}
case 35:
#line 189 "session_parser.y"
{
		session_ptr->setSutMaximumLengthReceived(sessionvsp[0].integer);
	;
    break;}
case 36:
#line 193 "session_parser.y"
{
		session_ptr->setSutImplementationClassUid(sessionvsp[0].string);
	;
    break;}
case 37:
#line 197 "session_parser.y"
{
		session_ptr->setSutImplementationVersionName(sessionvsp[0].string);
	;
    break;}
case 38:
#line 201 "session_parser.y"
{
		session_ptr->setSutHostname(sessionvsp[0].string);
	;
    break;}
case 39:
#line 205 "session_parser.y"
{
		session_ptr->setSutPort((UINT16)sessionvsp[0].integer);
	;
    break;}
case 40:
#line 209 "session_parser.y"
{
		session_ptr->setSutRole(sessionvsp[0].sutRole);
	;
    break;}
case 41:
#line 213 "session_parser.y"
{
		session_ptr->setDvtAeTitle(sessionvsp[0].string);
	;
    break;}
case 42:
#line 217 "session_parser.y"
{
		session_ptr->setDvtMaximumLengthReceived(sessionvsp[0].integer);
	;
    break;}
case 43:
#line 221 "session_parser.y"
{
		session_ptr->setDvtImplementationClassUid(sessionvsp[0].string);
	;
    break;}
case 44:
#line 225 "session_parser.y"
{
		session_ptr->setDvtImplementationVersionName(sessionvsp[0].string);
	;
    break;}
case 45:
#line 229 "session_parser.y"
{
		session_ptr->setDvtPort((UINT16)sessionvsp[0].integer);
	;
    break;}
case 46:
#line 233 "session_parser.y"
{
		session_ptr->setDvtSocketTimeout(sessionvsp[0].integer);
	;
    break;}
case 47:
#line 237 "session_parser.y"
{
		session_ptr->setUseSecureSockets(sessionvsp[0].trueOrFalse);
	;
    break;}
case 48:
#line 241 "session_parser.y"
{
		session_ptr->setTlsVersion(sessionvsp[0].string);
	;
    break;}
case 49:
#line 245 "session_parser.y"
{
		session_ptr->setCheckRemoteCertificate(sessionvsp[0].trueOrFalse);
	;
    break;}
case 50:
#line 249 "session_parser.y"
{
		session_ptr->setCipherList(sessionvsp[0].string);
	;
    break;}
case 51:
#line 253 "session_parser.y"
{
		session_ptr->setCacheTlsSessions(sessionvsp[0].trueOrFalse);
	;
    break;}
case 52:
#line 257 "session_parser.y"
{
		session_ptr->setTlsCacheTimeout(sessionvsp[0].integer);
	;
    break;}
case 53:
#line 261 "session_parser.y"
{
		session_ptr->setDelayForStorageCommitment(sessionvsp[0].integer);
	;
    break;}
case 54:
#line 265 "session_parser.y"
{
		session_ptr->setCredentialsFilename(sessionvsp[0].string);
	;
    break;}
case 55:
#line 269 "session_parser.y"
{
		session_ptr->setCertificateFilename(sessionvsp[0].string);
	;
    break;}
case 56:
#line 273 "session_parser.y"
{
		// deprecated
	;
    break;}
case 57:
#line 277 "session_parser.y"
{
		// deprecated
	;
    break;}
case 58:
#line 281 "session_parser.y"
{
		// deprecated
	;
    break;}
case 59:
#line 285 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_PDU_BYTES);
	;
    break;}
case 60:
#line 289 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_LABEL);
	;
    break;}
case 61:
#line 293 "session_parser.y"
{
		if (sessionvsp[0].trueOrFalse) 
		{
			session_ptr->setStorageMode(SM_AS_MEDIA);
		}
		else 
		{
			session_ptr->setStorageMode(SM_NO_STORAGE);
		}
	;
    break;}
case 62:
#line 304 "session_parser.y"
{
		session_ptr->setDefineSqLength(sessionvsp[0].trueOrFalse);
	;
    break;}
case 63:
#line 308 "session_parser.y"
{
		session_ptr->setAddGroupLength(sessionvsp[0].trueOrFalse);
	;
    break;}
case 64:
#line 312 "session_parser.y"
{
		session_ptr->setContinueOnError(sessionvsp[0].trueOrFalse);
	;
    break;}
case 65:
#line 316 "session_parser.y"
{
		session_ptr->addSupportedTransferSyntax(sessionvsp[0].string);
	;
    break;}
case 66:
#line 320 "session_parser.y"
{
		if (session_ptr->getStorageMode() != SM_NO_STORAGE) 
		{
			if (sessionvsp[0].trueOrFalse) 
			{
				session_ptr->setStorageMode(SM_AS_MEDIA);
			}
			else 
			{
				session_ptr->setStorageMode(SM_AS_DATASET);
			}
		}
	;
    break;}
case 67:
#line 334 "session_parser.y"
{
		session_ptr->setStorageMode(sessionvsp[0].storageMode);
	;
    break;}
case 68:
#line 338 "session_parser.y"
{
		session_ptr->setStrictValidation(sessionvsp[0].trueOrFalse);
	;
    break;}
case 69:
#line 342 "session_parser.y"
{
		session_ptr->setDetailedValidationResults(sessionvsp[0].trueOrFalse);
	;
    break;}
case 70:
#line 346 "session_parser.y"
{
		session_ptr->setSummaryValidationResults(sessionvsp[0].trueOrFalse);
	;
    break;}
case 71:
#line 350 "session_parser.y"
{
		session_ptr->setIncludeType3NotPresentInResults(sessionvsp[0].trueOrFalse);
	;
    break;}
case 72:
#line 354 "session_parser.y"
{
		session_ptr->setDumpAttributesOfRefFiles(sessionvsp[0].trueOrFalse);
	;
    break;}
case 73:
#line 358 "session_parser.y"
{
		BASE_SESSION_CLASS::setUsePrivateAttributeMapping(sessionvsp[0].trueOrFalse);
	;
    break;}
case 74:
#line 362 "session_parser.y"
{
		session_ptr->setAutoType2Attributes(sessionvsp[0].trueOrFalse);
	;
    break;}
case 75:
#line 366 "session_parser.y"
{
		session_ptr->setAutoCreateDirectory(sessionvsp[0].trueOrFalse);
	;
    break;}
case 76:
#line 370 "session_parser.y"
{
		session_ptr->setEnsureEvenAttributeValueLength(sessionvsp[0].trueOrFalse);
	;
    break;}
case 77:
#line 374 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_ERROR);
	;
    break;}
case 78:
#line 378 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_WARNING);
	;
    break;}
case 79:
#line 382 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_INFO);
	;
    break;}
case 80:
#line 386 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_IMAGE_RELATION);
	;
    break;}
case 81:
#line 390 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_DEBUG);
	;
    break;}
case 82:
#line 394 "session_parser.y"
{
		// deprecated
	;
    break;}
case 83:
#line 398 "session_parser.y"
{
		// deprecated
	;
    break;}
case 84:
#line 402 "session_parser.y"
{
		// deprecated
	;
    break;}
case 85:
#line 406 "session_parser.y"
{
		session_ptr->setLogScpThread(sessionvsp[0].trueOrFalse);
	;
    break;}
case 86:
#line 410 "session_parser.y"
{
		session_ptr->setLogLevel(sessionvsp[0].trueOrFalse, LOG_DULP_FSM);
	;
    break;}
case 87:
#line 414 "session_parser.y"
{
		// deprecated
	;
    break;}
case 88:
#line 418 "session_parser.y"
{
		session_ptr->addDefinitionDirectory(sessionvsp[0].string);
	;
    break;}
case 89:
#line 422 "session_parser.y"
{
		session_ptr->setDataDirectory(sessionvsp[0].string);
	;
    break;}
case 90:
#line 426 "session_parser.y"
{
		session_ptr->setUnVrDefinitionLookUp(sessionvsp[0].trueOrFalse);	
	;
    break;}
case 91:
#line 430 "session_parser.y"
{
		// no longer used in DVT V2.x
		// maintained for backwards compatiblity with DVT V1.x
		session_ptr->setDefinitionFileRoot(sessionvsp[0].string);
	;
    break;}
case 92:
#line 436 "session_parser.y"
{
		string definitionFile((char*) sessionvsp[0].string);
		bool exists = false;
		for (int index = 0; index < session_ptr->noDefinitionFiles(); index++)
		{
		    const char* pDefFileName = session_ptr->getDefinitionFilename(index);
		    if (strcmp(pDefFileName, sessionvsp[0].string) == 0)
		    {		    
		        exists = true;
		        break;
		    }
		}
		if (!exists)
		{
		    DEFINITION_FILE_CLASS *definitionFile_ptr = new DEFINITION_FILE_CLASS(session_ptr, definitionFile);
		
		    // for backwards compatibility with DVT V1.x we need to check if the
		    // character set or image display format files are being defined in the session
		    // file
		    string pathname = definitionFile_ptr->getAbsoluteFilename();
		    if ((pathname.find(DVT_V1_CHARACTER_SET_DEFINITION_FILENAME) == pathname.npos) &&
			    (pathname.find(DVT_V1_IMAGE_DISPLAY_FORMAT_DEFINITION_FILENAME) == pathname.npos))
		    {
			    session_ptr->addDefinitionFile(definitionFile_ptr);
		    }
		}
	;
    break;}
case 93:
#line 464 "session_parser.y"
{
		session_ptr->setDicomScriptRoot(sessionvsp[0].string);
	;
    break;}
case 94:
#line 468 "session_parser.y"
{
		// only store this information in a Scripting session
		if (session_ptr->getRuntimeSessionType() == ST_SCRIPT)
		{
			// should check that file extension is OK
			string dicomScript((char*) sessionvsp[0].string);
			DICOM_SCRIPT_CLASS *dicomScript_ptr = new DICOM_SCRIPT_CLASS(reinterpret_cast<SCRIPT_SESSION_CLASS*>(session_ptr), dicomScript);
			session_ptr->addDicomScript(dicomScript_ptr);
		}
	;
    break;}
case 95:
#line 479 "session_parser.y"
{
		session_ptr->setResultsRoot(sessionvsp[0].string);
	;
    break;}
case 96:
#line 483 "session_parser.y"
{
		session_ptr->setDescriptionDirectory(sessionvsp[0].string);
	;
    break;}
case 97:
#line 487 "session_parser.y"
{
		session_ptr->setAppendToResultsFile(sessionvsp[0].trueOrFalse);
	;
    break;}
case 98:
#line 491 "session_parser.y"
{
		// deprecated
	;
    break;}
}
   /* the action file gets copied in in place of this dollarsign */
#line 487 "bison.simple"

  sessionvsp -= yylen;
  sessionssp -= yylen;
#ifdef YYLSP_NEEDED
  sessionlsp -= yylen;
#endif

#if YYDEBUG != 0
  if (sessiondebug)
    {
      short *ssp1 = sessionss - 1;
      fprintf (stderr, "state stack now");
      while (ssp1 != sessionssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

  *++sessionvsp = sessionval;

#ifdef YYLSP_NEEDED
  sessionlsp++;
  if (yylen == 0)
    {
      sessionlsp->first_line = yylloc.first_line;
      sessionlsp->first_column = yylloc.first_column;
      sessionlsp->last_line = (sessionlsp-1)->last_line;
      sessionlsp->last_column = (sessionlsp-1)->last_column;
      sessionlsp->text = 0;
    }
  else
    {
      sessionlsp->last_line = (sessionlsp+yylen-1)->last_line;
      sessionlsp->last_column = (sessionlsp+yylen-1)->last_column;
    }
#endif

  /* Now "shift" the result of the reduction.
     Determine what state that goes to,
     based on the state we popped back to
     and the rule number reduced by.  */

  yyn = sessionr1[yyn];

  sessionstate = sessionpgoto[yyn - YYNTBASE] + *sessionssp;
  if (sessionstate >= 0 && sessionstate <= YYLAST && yycheck[sessionstate] == *sessionssp)
    sessionstate = yytable[sessionstate];
  else
    sessionstate = sessiondefgoto[yyn - YYNTBASE];

  goto yynewstate;

sessionerrlab:   /* here on detecting error */

  if (! yyerrstatus)
    /* If not already recovering from an error, report this error.  */
    {
      ++sessionnerrs;

#ifdef YYERROR_VERBOSE
      yyn = sessionpact[sessionstate];

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
	      sessionerror(msg);
	      free(msg);
	    }
	  else
	    sessionerror ("parse error; also virtual memory exceeded");
	}
      else
#endif /* YYERROR_VERBOSE */
	sessionerror("parse error");
    }

  goto sessionerrlab1;
sessionerrlab1:   /* here on error raised explicitly by an action */

  if (yyerrstatus == 3)
    {
      /* if just tried and failed to reuse lookahead token after an error, discard it.  */

      /* return failure if at end of input */
      if (sessionchar == YYEOF)
	YYABORT;

#if YYDEBUG != 0
      if (sessiondebug)
	fprintf(stderr, "Discarding token %d (%s).\n", sessionchar, yytname[sessionchar1]);
#endif

      sessionchar = YYEMPTY;
    }

  /* Else will try to reuse lookahead token
     after shifting the error token.  */

  yyerrstatus = 3;		/* Each real token shifted decrements this */

  goto yyerrhandle;

yyerrdefault:  /* current state does not do anything special for the error token. */

#if 0
  /* This is wrong; only states that explicitly want error tokens
     should shift them.  */
  yyn = sessiondefact[sessionstate];  /* If its default is to accept any token, ok.  Otherwise pop it.*/
  if (yyn) goto sessiondefault;
#endif

yyerrpop:   /* pop the current state because it cannot handle the error token */

  if (sessionssp == sessionss) YYABORT;
  sessionvsp--;
  sessionstate = *--sessionssp;
#ifdef YYLSP_NEEDED
  sessionlsp--;
#endif

#if YYDEBUG != 0
  if (sessiondebug)
    {
      short *ssp1 = sessionss - 1;
      fprintf (stderr, "Error: state stack now");
      while (ssp1 != sessionssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

yyerrhandle:

  yyn = sessionpact[sessionstate];
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
  if (sessiondebug)
    fprintf(stderr, "Shifting error token, ");
#endif

  *++sessionvsp = sessionlval;
#ifdef YYLSP_NEEDED
  *++sessionlsp = yylloc;
#endif

  sessionstate = yyn;
  goto yynewstate;
}
#line 495 "session_parser.y"



