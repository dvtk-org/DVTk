
/*  A Bison parser, made from definition_parse.y with Bison version GNU Bison version 1.24
  */

#define YYBISON 1  /* Identify Bison output.  */

#define	T_SYSTEM	258
#define	T_DEFINE	259
#define	T_ENDDEFINE	260
#define	T_METASOPCLASS	261
#define	T_SOPCLASS	262
#define	T_MODULE	263
#define	T_ITEM	264
#define	T_MACRO	265
#define	T_INCLUDEITEM	266
#define	T_INCLUDEMACRO	267
#define	T_SQ	268
#define	T_AND	269
#define	T_OR	270
#define	T_NOT	271
#define	T_PRESENT	272
#define	T_VALUE	273
#define	T_EMPTY	274
#define	T_TRUE	275
#define	T_FALSE	276
#define	T_EQUAL	277
#define	T_LESS	278
#define	T_GREATER	279
#define	T_LESS_OR_EQUAL	280
#define	T_GREATER_OR_EQUAL	281
#define	T_WEAK	282
#define	T_WARN	283
#define	STRING	284
#define	INTEGER	285
#define	HEX	286
#define	COMMANDFIELD	287
#define	TYPE	288
#define	USAGE	289
#define	VALUETYPE	290
#define	VR	291

#line 1 "definition_parse.y"

// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2006
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

/*
 * A parser for the definition files
 */

#include <malloc.h>

#include "Iglobal.h"			// Global component interface
#include "Ilog.h"				// Log component interface
#include "IAttributeGroup.h"	// Attribute Group component interface
#include "Icondition.h"	        // Condition component interface
#include "AEDefinition.h"
#include "AttributeDefinition.h"
#include "AttributeGroupDefinition.h"
#include "CommandDefinition.h"
#include "DatasetDefinition.h"
#include "DICOMObjectDefinition.h"
#include "ItemDefinition.h"
#include "MacroDefinition.h"
#include "MetaSopClassDefinition.h"
#include "ModuleDefinition.h"
#include "SopClassDefinition.h"
#include "SystemDefinition.h"
#include "deffilecontent.h"

// flex/bison error and warning functions
extern void definitionerror(char*);
extern void definitionwarn(char*);
extern void definitionerror(char*, bool);
extern void definitionwarn(char*, bool);
extern int  definitionlex(void);
extern LOG_CLASS* definitionfilelogger_ptr;
extern DEF_FILE_CONTENT_CLASS* defFileContent_ptr;

void resetDefinitionParser();

string definitionfilename;
bool   definitionnewfile = false;
bool   definitionfileempty = false;

static BASE_VALUE_CLASS* CreateValueFromString(NAME_STRING, ATTR_VR_ENUM);
static BASE_VALUE_CLASS* CreateValueFromInt(int, ATTR_VR_ENUM);
static BASE_VALUE_CLASS* CreateValueFromHex(unsigned long, ATTR_VR_ENUM);
static void CreateVMFromString(NAME_STRING, DEF_ATTRIBUTE_CLASS*);
static void ResetVariables();
static void ResolveMacroReferences();


// local variables - unfortunately this structure is needed for YACC/LEX
// they are used as temporary variables
static DEF_METASOPCLASS_CLASS*		currentMetaSopClass_ptr = NULL;
static DEF_SOPCLASS_CLASS*			currentSopClass_ptr = NULL;
static DEF_DICOM_OBJECT_CLASS*		currentDICOMObject_ptr = NULL;
static DEF_COMMAND_CLASS*			currentCommand_ptr = NULL;
static DEF_DATASET_CLASS*			currentDataset_ptr = NULL;
static DEF_ATTRIBUTE_GROUP_CLASS*	currentAttributeGroup_ptr = NULL;
static DEF_MODULE_CLASS*			currentModule_ptr = NULL;
static DEF_ITEM_CLASS*              currentItem_ptr = NULL;
static DEF_MACRO_CLASS*             currentMacro_ptr = NULL;
static DEF_ATTRIBUTE_CLASS*         currentAttribute_ptr = NULL;
static int							currentValueListIndex = 0;
static BASE_VALUE_CLASS*			currentValue_ptr = NULL;
static CONDITION_CLASS*             currentCondition_ptr = NULL;
static CONDITION_NODE_CLASS*        condition_node_ptr = NULL;
static CONDITION_NODE_CLASS*        node_left_ptr = NULL;
static CONDITION_NODE_CLASS*        node_right_ptr = NULL;
static CONDITION_NODE_CLASS*        node_ptr = NULL;
static string                       lTextualCondition;
static NAME_STRING					lsystemname;
static NAME_STRING			        lsystemversion;
static NAME_STRING					lAEname;
static NAME_STRING					lAEversion;
static NAME_STRING					lMetaSOPClassUID;
static NAME_STRING					lMetaSOPClassName;
static NAME_STRING					lSOPClassUID;
static NAME_STRING					lSOPClassName;
static NAME_STRING					lIodName;
static NAME_STRING					lModuleName;
static MOD_USAGE_ENUM				lModuleUsage;
static UINT16						lAttrGroup;
static UINT16						lAttrElement;
static ATTR_VR_ENUM					lAttrVr;
static DIMSE_CMD_ENUM				lDimseCmd;
static ATTR_VAL_TYPE_ENUM			currentValueType;
static ATTR_VAL_TYPE_ENUM			lFirstValueType;


// switch which in case errors occur prevents
// that attributes and the like are added to non-existing
// objects. Parsing can continue.
bool definitionParseOnly = false;
bool skipDefinition      = false;

// keep track of nesting levels
static stack<DEF_ATTRIBUTE_GROUP_CLASS*> groups;
static stack<DEF_ATTRIBUTE_CLASS*> attributes;

// remember which groups have macro references
static vector<DEF_ATTRIBUTE_GROUP_CLASS*> groups_with_refs;
static vector<DEF_MACRO_CLASS*> macros;

#line 108 "definition_parse.y"
typedef union {
	bool						boolean;
	DIMSE_CMD_ENUM				commandField;
    unsigned long				hex;
	int							integer;
    NAME_STRING  				string;
	ATTR_TYPE_ENUM				type;
	MOD_USAGE_ENUM				usage;
	ATTR_VAL_TYPE_ENUM			valueType;
	ATTR_VR_ENUM				vr;
	CONDITION_NODE_CLASS*		node_ptr;
	CONDITION_UNARY_NODE_CLASS*	unary_node_ptr;
	CONDITION_BINARY_NODE_CLASS* binary_node_ptr;
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



#define	YYFINAL		223
#define	YYFLAG		-32768
#define	YYNTBASE	45

#define YYTRANSLATE(x) ((unsigned)(x) <= 291 ? yytranslate[x] : 129)

static const char yytranslate[] = {     0,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,    37,
    39,     2,     2,    38,     2,    44,    42,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,    40,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,    41,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,    43,     2,     2,     2,     2,     2,     2,
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
    36
};

#if YYDEBUG != 0
static const short yyprhs[] = {     0,
     0,     2,     4,     7,     9,    11,    15,    17,    19,    21,
    23,    25,    27,    29,    31,    35,    38,    40,    42,    45,
    47,    49,    53,    56,    58,    60,    62,    65,    69,    71,
    73,    76,    78,    80,    84,    86,    89,    91,    93,    95,
    98,   101,   103,   107,   109,   111,   114,   116,   118,   120,
   123,   133,   135,   137,   139,   141,   143,   145,   147,   149,
   151,   154,   156,   161,   168,   173,   175,   178,   181,   183,
   187,   191,   193,   195,   197,   201,   205,   207,   209,   213,
   214,   217,   222,   226,   228,   232,   236,   238,   240,   242,
   246,   248,   250,   252,   256,   258,   261,   263,   266,   268,
   270,   274,   276,   277,   280,   283,   291,   293,   295,   297,
   301,   303,   307,   310,   313,   319,   322,   326,   331,   333,
   335,   336,   338,   340,   342,   344,   346,   348,   350,   352,
   354,   357,   360,   363,   366,   370,   374,   378,   381,   383,
   386,   390,   392,   395,   397,   399,   402
};

static const short yyrhs[] = {    46,
     0,    47,     0,    46,    47,     0,    48,     0,     1,     0,
    49,    51,    50,     0,     4,     0,     5,     0,    52,     0,
    59,     0,    67,     0,    70,     0,   104,     0,   106,     0,
     3,    53,    56,     0,    54,    55,     0,    29,     0,    29,
     0,    57,    58,     0,    29,     0,    29,     0,     6,    60,
    63,     0,    61,    62,     0,    29,     0,    29,     0,    64,
     0,    63,    64,     0,     7,    65,    66,     0,    29,     0,
    29,     0,    68,    81,     0,    69,     0,    32,     0,    68,
    71,    72,     0,    29,     0,    73,    74,     0,    73,     0,
    64,     0,    75,     0,    74,    75,     0,    76,    81,     0,
    76,     0,     8,    77,    78,     0,     8,     0,    29,     0,
    79,    80,     0,    34,     0,   111,     0,    82,     0,    81,
    82,     0,    37,    83,    38,    85,    38,    88,    39,    86,
    87,     0,   109,     0,    84,     0,    31,     0,    30,     0,
    33,     0,    29,     0,   111,     0,    89,     0,    91,     0,
    90,    92,     0,    90,     0,    13,    38,    94,    38,     0,
    13,    38,    94,    38,    95,    38,     0,    96,    38,    94,
    97,     0,    93,     0,    92,    93,     0,    24,    82,     0,
    30,     0,    30,    40,    30,     0,    30,    40,    41,     0,
    29,     0,    41,     0,    30,     0,    30,    40,    30,     0,
    30,    40,    41,     0,    41,     0,    36,     0,    36,    42,
    36,     0,     0,    38,    98,     0,    38,    98,    38,    99,
     0,   101,    38,   102,     0,   100,     0,    99,    38,   100,
     0,   101,    38,   102,     0,   102,     0,    35,     0,   103,
     0,   102,    43,   103,     0,    29,     0,    31,     0,    30,
     0,     9,   105,    81,     0,    29,     0,   107,    81,     0,
   107,     0,    10,   108,     0,    10,     0,    29,     0,    12,
   108,   110,     0,   111,     0,     0,    40,    29,     0,    40,
   114,     0,    40,    27,    37,   112,    38,   113,    39,     0,
   114,     0,   114,     0,   115,     0,   114,    15,   115,     0,
   116,     0,   115,    14,   116,     0,    17,   120,     0,    19,
   120,     0,    18,   120,   117,   119,   118,     0,    16,   116,
     0,    37,   114,    39,     0,    28,    37,   114,    39,     0,
    20,     0,    21,     0,     0,    30,     0,    29,     0,    22,
     0,    23,     0,    24,     0,    25,     0,    26,     0,    84,
     0,   121,     0,   122,    84,     0,   123,    84,     0,   125,
    84,     0,   127,    84,     0,   122,   127,    84,     0,   123,
   127,    84,     0,   125,   127,    84,     0,    44,    42,     0,
   124,     0,   123,   124,     0,    44,    44,    42,     0,   126,
     0,   125,   126,     0,    42,     0,   128,     0,   127,   128,
     0,    84,    42,     0
};

#endif

#if YYDEBUG != 0
static const short yyrline[] = { 0,
   158,   165,   166,   170,   171,   179,   182,   185,   193,   194,
   195,   196,   197,   198,   201,   204,   213,   219,   226,   235,
   241,   248,   258,   267,   273,   279,   286,   295,   298,   304,
   310,   321,   324,   344,   353,   378,   386,   392,   414,   415,
   418,   425,   431,   445,   451,   458,   461,   467,   470,   471,
   474,   481,   485,   494,   501,   521,   530,   539,   564,   565,
   568,   581,   597,   623,   650,   653,   654,   658,   661,   669,
   677,   685,   692,   702,   710,   718,   726,   736,   744,   755,
   759,   764,   771,   787,   788,   791,   799,   812,   821,   828,
   837,   844,   851,   860,   863,   868,   881,   887,   895,   906,
   909,   925,   929,   930,   940,   955,   976,   982,   988,   992,
  1002,  1006,  1016,  1024,  1036,  1059,  1067,  1071,  1076,  1084,
  1094,  1098,  1104,  1107,  1115,  1123,  1131,  1139,  1149,  1160,
  1169,  1179,  1195,  1211,  1227,  1244,  1267,  1292,  1302,  1309,
  1324,  1334,  1341,  1356,  1366,  1373,  1388
};

static const char * const yytname[] = {   "$","error","$undefined.","T_SYSTEM",
"T_DEFINE","T_ENDDEFINE","T_METASOPCLASS","T_SOPCLASS","T_MODULE","T_ITEM","T_MACRO",
"T_INCLUDEITEM","T_INCLUDEMACRO","T_SQ","T_AND","T_OR","T_NOT","T_PRESENT","T_VALUE",
"T_EMPTY","T_TRUE","T_FALSE","T_EQUAL","T_LESS","T_GREATER","T_LESS_OR_EQUAL",
"T_GREATER_OR_EQUAL","T_WEAK","T_WARN","STRING","INTEGER","HEX","COMMANDFIELD",
"TYPE","USAGE","VALUETYPE","VR","'('","','","')'","':'","'n'","'/'","'|'","'.'",
"DefinitionGrammar","DefinitionComponents","Definition","Define","BeginDefine",
"EndDefine","Definitions","SystemDef","SystemDefinition","SystemName","SystemVersion",
"AEDefinition","AEName","AEVersion","MetaSopClassDef","BeginMetaSopClassDef",
"MetaSopClassUID","MetaSopClassName","SopClassDefList","SopClassDef","SopClassUID",
"SopClassName","CommandDef","DimseCmdDef","DimseCmd","CommandIodDef","IodName",
"IodContentDef","SopClassDefinition","ModuleDefList","ModuleDef","ModuleNameDef",
"ModuleName","ModuleUsage","ModUsage","ModuleCondition","AttributeDefList","AttributeDef",
"AttributeTagDef","AttributeTag","AttributeType","AttributeName","AttributeCondition",
"AttributeValueDef","SequenceValueDef","SequenceIntro","OtherValueDef","ItemAttributeDefList",
"ItemAttributeDef","AttributeVM","AttributeIM","AttributeVR","ValuesDef","FirstValuesDef",
"OtherValuesDefList","OtherValuesDef","ValueType","ValuesList","Value","ItemDef",
"ItemName","MacroDef","MacroBegin","MacroName","MacroReference","MacroCondition",
"Condition","PrimaryCond","SecondaryCond","CondExpr","Expr","SimpleExpr","ValueNr",
"CondValue","Operator","AttributeTagAddress","AttributeTagPath","AttributeTagHere",
"AttributeTagUpPath","AttributeTagUp","AttributeTagRootPath","AttributeTagRoot",
"AttributeTagDownPath","AttributeTagDown",""
};
#endif

static const short definitionr1[] = {     0,
    45,    46,    46,    47,    47,    48,    49,    50,    51,    51,
    51,    51,    51,    51,    52,    53,    54,    55,    56,    57,
    58,    59,    60,    61,    62,    63,    63,    64,    65,    66,
    67,    68,    69,    70,    71,    72,    72,    73,    74,    74,
    75,    75,    76,    76,    77,    78,    79,    80,    81,    81,
    82,    82,    83,    84,    85,    85,    86,    87,    88,    88,
    89,    89,    90,    90,    91,    92,    92,    93,    94,    94,
    94,    94,    94,    95,    95,    95,    95,    96,    96,    97,
    97,    97,    98,    99,    99,   100,   100,   101,   102,   102,
   103,   103,   103,   104,   105,   106,   106,   107,   107,   108,
   109,   110,   111,   111,   111,   111,   112,   113,   114,   114,
   115,   115,   116,   116,   116,   116,   116,   116,   116,   116,
   117,   117,   118,   119,   119,   119,   119,   119,   120,   120,
   121,   121,   121,   121,   121,   121,   121,   122,   123,   123,
   124,   125,   125,   126,   127,   127,   128
};

static const short definitionr2[] = {     0,
     1,     1,     2,     1,     1,     3,     1,     1,     1,     1,
     1,     1,     1,     1,     3,     2,     1,     1,     2,     1,
     1,     3,     2,     1,     1,     1,     2,     3,     1,     1,
     2,     1,     1,     3,     1,     2,     1,     1,     1,     2,
     2,     1,     3,     1,     1,     2,     1,     1,     1,     2,
     9,     1,     1,     1,     1,     1,     1,     1,     1,     1,
     2,     1,     4,     6,     4,     1,     2,     2,     1,     3,
     3,     1,     1,     1,     3,     3,     1,     1,     3,     0,
     2,     4,     3,     1,     3,     3,     1,     1,     1,     3,
     1,     1,     1,     3,     1,     2,     1,     2,     1,     1,
     3,     1,     0,     2,     2,     7,     1,     1,     1,     3,
     1,     3,     2,     2,     5,     2,     3,     4,     1,     1,
     0,     1,     1,     1,     1,     1,     1,     1,     1,     1,
     2,     2,     2,     2,     3,     3,     3,     2,     1,     2,
     3,     1,     2,     1,     1,     2,     2
};

static const short definitiondefact[] = {     0,
     5,     7,     0,     2,     4,     0,     3,     0,     0,     0,
    99,    33,     0,     9,    10,    11,     0,    32,    12,    13,
    14,    97,    17,     0,     0,    24,     0,     0,    95,     0,
   100,    98,     8,     6,     0,    35,     0,     0,    31,    49,
    52,    96,    20,    15,     0,    18,    16,     0,    22,    26,
    25,    23,    94,   103,    54,     0,    53,    38,    34,    37,
    50,    21,    19,    29,     0,    27,     0,   101,   102,     0,
    44,    36,    39,    42,    30,    28,     0,     0,     0,     0,
   119,   120,     0,     0,   104,     0,   105,   109,   111,    55,
    56,     0,    45,     0,    40,    41,   116,   144,     0,   129,
   113,   130,     0,     0,   139,     0,   142,     0,   145,   121,
   114,     0,     0,     0,     0,     0,     0,    47,    43,   103,
   138,     0,   147,   131,     0,     0,   132,   140,     0,   133,
   143,     0,   134,   146,   122,     0,     0,   107,     0,   117,
   110,   112,     0,    78,     0,    59,    62,    60,     0,    46,
    48,   141,   135,   136,   137,   124,   125,   126,   127,   128,
     0,     0,   118,     0,     0,     0,     0,    61,    66,     0,
   123,   115,     0,   108,    72,    69,    73,     0,    79,    57,
   103,    68,    67,    80,   106,     0,    63,    51,    58,     0,
    65,    70,    71,    74,    77,     0,    88,    81,     0,     0,
    64,     0,     0,    75,    76,    91,    93,    92,    82,    84,
     0,    87,    89,    83,     0,     0,     0,    85,    86,    90,
     0,     0,     0
};

static const short definitiondefgoto[] = {   221,
     3,     4,     5,     6,    34,    13,    14,    24,    25,    47,
    44,    45,    63,    15,    27,    28,    52,    49,    50,    65,
    76,    16,    17,    18,    19,    38,    59,    60,    72,    73,
    74,    94,   119,   120,   150,    39,    40,    56,   100,    92,
   181,   188,   145,   146,   147,   148,   168,   169,   178,   196,
   149,   191,   198,   209,   210,   211,   212,   213,    20,    30,
    21,    22,    32,    41,    68,    69,   137,   173,    87,    88,
    89,   136,   172,   161,   101,   102,   103,   104,   105,   106,
   107,   108,   109
};

static const short definitionpact[] = {    91,
-32768,-32768,   101,-32768,-32768,     3,-32768,   -11,    19,    31,
    40,-32768,    25,-32768,-32768,-32768,    -4,-32768,-32768,-32768,
-32768,    -1,-32768,    65,    75,-32768,    57,    87,-32768,    -1,
-32768,-32768,-32768,-32768,    40,-32768,    42,    57,    -1,-32768,
-32768,    -1,-32768,-32768,    88,-32768,-32768,    89,    57,-32768,
-32768,-32768,    -1,    79,-32768,    83,-32768,-32768,-32768,   112,
-32768,-32768,-32768,-32768,    93,-32768,    34,-32768,-32768,    76,
    94,   112,-32768,    -1,-32768,-32768,    63,    -5,    -5,    -5,
-32768,-32768,    90,    92,-32768,    63,   109,   111,-32768,-32768,
-32768,    95,-32768,    96,-32768,    -1,-32768,-32768,    46,    84,
-32768,-32768,    42,    12,-32768,     7,-32768,    42,-32768,   102,
-32768,    63,    63,     1,    63,    63,     6,-32768,-32768,    79,
-32768,    97,-32768,    84,    42,    98,    84,-32768,    42,    84,
-32768,    42,    84,-32768,-32768,    -2,    99,   109,     2,-32768,
   111,-32768,   100,   103,   104,-32768,   107,-32768,   106,-32768,
-32768,-32768,    84,    84,    84,-32768,-32768,-32768,-32768,-32768,
   105,    63,-32768,    17,   110,   118,    -1,   107,-32768,    17,
-32768,-32768,   113,   109,-32768,   108,-32768,   115,-32768,-32768,
    79,-32768,-32768,   116,-32768,    36,    44,-32768,-32768,   114,
-32768,-32768,-32768,   117,-32768,   120,-32768,   121,   122,    45,
-32768,    68,    82,-32768,-32768,-32768,-32768,-32768,   123,-32768,
   124,   125,-32768,   125,    68,    82,    82,-32768,   125,-32768,
   135,   136,-32768
};

static const short definitionpgoto[] = {-32768,
-32768,   137,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,    38,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,    69,
-32768,-32768,-32768,-32768,-32768,   -15,   -39,-32768,   -36,-32768,
-32768,-32768,-32768,-32768,-32768,-32768,-32768,   -18,   -19,-32768,
-32768,-32768,-32768,-32768,   -60,   -34,  -171,   -54,-32768,-32768,
-32768,-32768,   129,-32768,-32768,  -116,-32768,-32768,   -84,    50,
   -72,-32768,-32768,-32768,    35,-32768,-32768,-32768,    62,-32768,
    61,     4,   -98
};


#define	YYLAST		168


static const short yytable[] = {    61,
    57,   114,    61,   151,    97,     8,    42,    35,     9,   134,
    35,    10,    11,    61,    53,   115,   115,    23,   143,   156,
   157,   158,   159,   160,    36,    55,   134,   138,   139,    33,
   134,   214,    37,   134,    12,    37,    98,    55,    99,   140,
   163,   144,    55,   142,   219,   175,   176,    26,    98,    77,
    78,    79,    80,    81,    82,   126,    61,   177,    96,    29,
    83,    84,    85,    48,   189,   192,   124,   127,    31,   130,
    86,   133,    55,   194,   204,    58,   193,   174,    77,    78,
    79,    80,    81,    82,   195,   205,    66,   121,   153,   122,
    84,     1,   154,    43,     2,   155,   206,   207,   208,    86,
    -1,     1,   197,    46,     2,    90,   125,   129,    91,   132,
   206,   207,   208,   110,   111,    51,    62,    64,    67,    71,
    70,    75,    93,   115,   116,   123,   112,   182,   113,   118,
   167,   135,   117,   171,   222,   223,   162,   164,   152,     7,
    95,   122,   166,   170,   165,   179,   180,   186,   197,   183,
   184,   185,   187,   190,   218,   199,   200,   201,   202,   203,
   215,   216,   220,    54,   141,   128,   131,   217
};

static const short yycheck[] = {    39,
    37,    86,    42,   120,    77,     3,    22,    12,     6,   108,
    12,     9,    10,    53,    30,    15,    15,    29,    13,    22,
    23,    24,    25,    26,    29,    31,   125,   112,   113,     5,
   129,   203,    37,   132,    32,    37,    42,    31,    44,    39,
    39,    36,    31,   116,   216,    29,    30,    29,    42,    16,
    17,    18,    19,    20,    21,    44,    96,    41,    74,    29,
    27,    28,    29,     7,   181,    30,   103,   104,    29,   106,
    37,   108,    31,    30,    30,    38,    41,   162,    16,    17,
    18,    19,    20,    21,    41,    41,    49,    42,   125,    44,
    28,     1,   129,    29,     4,   132,    29,    30,    31,    37,
     0,     1,    35,    29,     4,    30,   103,   104,    33,   106,
    29,    30,    31,    79,    80,    29,    29,    29,    40,     8,
    38,    29,    29,    15,    14,    42,    37,   167,    37,    34,
    24,    30,    38,    29,     0,     0,    38,    38,    42,     3,
    72,    44,    39,    38,    42,    36,    29,    40,    35,   168,
   170,    39,    38,    38,   215,   190,    40,    38,    38,    38,
    38,    38,   217,    35,   115,   104,   106,    43
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

#define definitionerrok		(yyerrstatus = 0)
#define definitionclearin	(definitionchar = YYEMPTY)
#define YYEMPTY		-2
#define YYEOF		0
#define YYACCEPT	return(0)
#define YYABORT 	return(1)
#define YYERROR		goto definitionerrlab1
/* Like YYERROR except do call definitionerror.
   This remains here temporarily to ease the
   transition to the new meaning of YYERROR, for GCC.
   Once GCC version 2 has supplanted version 1, this can go.  */
#define YYFAIL		goto definitionerrlab
#define YYRECOVERING()  (!!yyerrstatus)
#define YYBACKUP(token, value) \
do								\
  if (definitionchar == YYEMPTY && yylen == 1)				\
    { definitionchar = (token), definitionlval = (value);			\
      definitionchar1 = YYTRANSLATE (definitionchar);				\
      YYPOPSTACK;						\
      goto definitionbackup;						\
    }								\
  else								\
    { definitionerror ("syntax error: cannot back up"); YYERROR; }	\
while (0)

#define YYTERROR	1
#define YYERRCODE	256

#ifndef YYPURE
#define YYLEX		definitionlex()
#endif

#ifdef YYPURE
#ifdef YYLSP_NEEDED
#ifdef YYLEX_PARAM
#define YYLEX		definitionlex(&definitionlval, &yylloc, YYLEX_PARAM)
#else
#define YYLEX		definitionlex(&definitionlval, &yylloc)
#endif
#else /* not YYLSP_NEEDED */
#ifdef YYLEX_PARAM
#define YYLEX		definitionlex(&definitionlval, YYLEX_PARAM)
#else
#define YYLEX		definitionlex(&definitionlval)
#endif
#endif /* not YYLSP_NEEDED */
#endif

/* If nonreentrant, generate the variables here */

#ifndef YYPURE

int	definitionchar;			/*  the lookahead symbol		*/
YYSTYPE	definitionlval;			/*  the semantic value of the		*/
				/*  lookahead symbol			*/

#ifdef YYLSP_NEEDED
YYLTYPE yylloc;			/*  location data for the lookahead	*/
				/*  symbol				*/
#endif

int definitionnerrs;			/*  number of parse errors so far       */
#endif  /* not YYPURE */

#if YYDEBUG != 0
int definitiondebug;			/*  nonzero means print parse trace	*/
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
int definitionparse (void);
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
   into definitionparse.  The argument should have type void *.
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
definitionparse(YYPARSE_PARAM)
     YYPARSE_PARAM_DECL
{
  register int definitionstate;
  register int yyn;
  register short *definitionssp;
  register YYSTYPE *definitionvsp;
  int yyerrstatus;	/*  number of tokens to shift before error messages enabled */
  int definitionchar1 = 0;		/*  lookahead token as an internal (translated) token number */

  short	definitionssa[YYINITDEPTH];	/*  the state stack			*/
  YYSTYPE definitionvsa[YYINITDEPTH];	/*  the semantic value stack		*/

  short *definitionss = definitionssa;		/*  refer to the stacks thru separate pointers */
  YYSTYPE *definitionvs = definitionvsa;	/*  to allow yyoverflow to reallocate them elsewhere */

#ifdef YYLSP_NEEDED
  YYLTYPE yylsa[YYINITDEPTH];	/*  the location stack			*/
  YYLTYPE *yyls = yylsa;
  YYLTYPE *definitionlsp;

#define YYPOPSTACK   (definitionvsp--, definitionssp--, definitionlsp--)
#else
#define YYPOPSTACK   (definitionvsp--, definitionssp--)
#endif

  int definitionstacksize = YYINITDEPTH;

#ifdef YYPURE
  int definitionchar;
  YYSTYPE definitionlval;
  int definitionnerrs;
#ifdef YYLSP_NEEDED
  YYLTYPE yylloc;
#endif
#endif

  YYSTYPE definitionval;		/*  the variable used to return		*/
				/*  semantic values from the action	*/
				/*  routines				*/

  int yylen;

#if YYDEBUG != 0
  if (definitiondebug)
    fprintf(stderr, "Starting parse\n");
#endif

  definitionstate = 0;
  yyerrstatus = 0;
  definitionnerrs = 0;
  definitionchar = YYEMPTY;		/* Cause a token to be read.  */

  /* Initialize stack pointers.
     Waste one element of value and location stack
     so that they stay on the same level as the state stack.
     The wasted elements are never initialized.  */

  definitionssp = definitionss - 1;
  definitionvsp = definitionvs;
#ifdef YYLSP_NEEDED
  definitionlsp = yyls;
#endif

/* Push a new state, which is found in  definitionstate  .  */
/* In all cases, when you get here, the value and location stacks
   have just been pushed. so pushing a state here evens the stacks.  */
yynewstate:

  *++definitionssp = definitionstate;

  if (definitionssp >= definitionss + definitionstacksize - 1)
    {
      /* Give user a chance to reallocate the stack */
      /* Use copies of these so that the &'s don't force the real ones into memory. */
      YYSTYPE *definitionvs1 = definitionvs;
      short *definitionss1 = definitionss;
#ifdef YYLSP_NEEDED
      YYLTYPE *yyls1 = yyls;
#endif

      /* Get the current used size of the three stacks, in elements.  */
      int size = definitionssp - definitionss + 1;

#ifdef yyoverflow
      /* Each stack pointer address is followed by the size of
	 the data in use in that stack, in bytes.  */
#ifdef YYLSP_NEEDED
      /* This used to be a conditional around just the two extra args,
	 but that might be undefined if yyoverflow is a macro.  */
      yyoverflow("parser stack overflow",
		 &definitionss1, size * sizeof (*definitionssp),
		 &definitionvs1, size * sizeof (*definitionvsp),
		 &yyls1, size * sizeof (*definitionlsp),
		 &definitionstacksize);
#else
      yyoverflow("parser stack overflow",
		 &definitionss1, size * sizeof (*definitionssp),
		 &definitionvs1, size * sizeof (*definitionvsp),
		 &definitionstacksize);
#endif

      definitionss = definitionss1; definitionvs = definitionvs1;
#ifdef YYLSP_NEEDED
      yyls = yyls1;
#endif
#else /* no yyoverflow */
      /* Extend the stack our own way.  */
      if (definitionstacksize >= YYMAXDEPTH)
	{
	  definitionerror("parser stack overflow");
	  return 2;
	}
      definitionstacksize *= 2;
      if (definitionstacksize > YYMAXDEPTH)
	definitionstacksize = YYMAXDEPTH;
      definitionss = (short *) alloca (definitionstacksize * sizeof (*definitionssp));
      __yy_memcpy ((char *)definitionss1, (char *)definitionss, size * sizeof (*definitionssp));
      definitionvs = (YYSTYPE *) alloca (definitionstacksize * sizeof (*definitionvsp));
      __yy_memcpy ((char *)definitionvs1, (char *)definitionvs, size * sizeof (*definitionvsp));
#ifdef YYLSP_NEEDED
      yyls = (YYLTYPE *) alloca (definitionstacksize * sizeof (*definitionlsp));
      __yy_memcpy ((char *)yyls1, (char *)yyls, size * sizeof (*definitionlsp));
#endif
#endif /* no yyoverflow */

      definitionssp = definitionss + size - 1;
      definitionvsp = definitionvs + size - 1;
#ifdef YYLSP_NEEDED
      definitionlsp = yyls + size - 1;
#endif

#if YYDEBUG != 0
      if (definitiondebug)
	fprintf(stderr, "Stack size increased to %d\n", definitionstacksize);
#endif

      if (definitionssp >= definitionss + definitionstacksize - 1)
	YYABORT;
    }

#if YYDEBUG != 0
  if (definitiondebug)
    fprintf(stderr, "Entering state %d\n", definitionstate);
#endif

  goto definitionbackup;
 definitionbackup:

/* Do appropriate processing given the current state.  */
/* Read a lookahead token if we need one and don't already have one.  */
/* yyresume: */

  /* First try to decide what to do without reference to lookahead token.  */

  yyn = definitionpact[definitionstate];
  if (yyn == YYFLAG)
    goto definitiondefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* definitionchar is either YYEMPTY or YYEOF
     or a valid token in external form.  */

  if (definitionchar == YYEMPTY)
    {
#if YYDEBUG != 0
      if (definitiondebug)
	fprintf(stderr, "Reading a token: ");
#endif
      definitionchar = YYLEX;
    }

  /* Convert token to internal form (in definitionchar1) for indexing tables with */

  if (definitionchar <= 0)		/* This means end of input. */
    {
      definitionchar1 = 0;
      definitionchar = YYEOF;		/* Don't call YYLEX any more */

#if YYDEBUG != 0
      if (definitiondebug)
	fprintf(stderr, "Now at end of input.\n");
#endif
    }
  else
    {
      definitionchar1 = YYTRANSLATE(definitionchar);

#if YYDEBUG != 0
      if (definitiondebug)
	{
	  fprintf (stderr, "Next token is %d (%s", definitionchar, yytname[definitionchar1]);
	  /* Give the individual parser a way to print the precise meaning
	     of a token, for further debugging info.  */
#ifdef YYPRINT
	  YYPRINT (stderr, definitionchar, definitionlval);
#endif
	  fprintf (stderr, ")\n");
	}
#endif
    }

  yyn += definitionchar1;
  if (yyn < 0 || yyn > YYLAST || yycheck[yyn] != definitionchar1)
    goto definitiondefault;

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
	goto definitionerrlab;
      yyn = -yyn;
      goto yyreduce;
    }
  else if (yyn == 0)
    goto definitionerrlab;

  if (yyn == YYFINAL)
    YYACCEPT;

  /* Shift the lookahead token.  */

#if YYDEBUG != 0
  if (definitiondebug)
    fprintf(stderr, "Shifting token %d (%s), ", definitionchar, yytname[definitionchar1]);
#endif

  /* Discard the token being shifted unless it is eof.  */
  if (definitionchar != YYEOF)
    definitionchar = YYEMPTY;

  *++definitionvsp = definitionlval;
#ifdef YYLSP_NEEDED
  *++definitionlsp = yylloc;
#endif

  /* count tokens shifted since error; after three, turn off error status.  */
  if (yyerrstatus) yyerrstatus--;

  definitionstate = yyn;
  goto yynewstate;

/* Do the default action for the current state.  */
definitiondefault:

  yyn = definitiondefact[definitionstate];
  if (yyn == 0)
    goto definitionerrlab;

/* Do a reduction.  yyn is the number of a rule to reduce with.  */
yyreduce:
  yylen = definitionr2[yyn];
  if (yylen > 0)
    definitionval = definitionvsp[1-yylen]; /* implement default value of the action */

#if YYDEBUG != 0
  if (definitiondebug)
    {
      int i;

      fprintf (stderr, "Reducing via rule %d (line %d), ",
	       yyn, yyrline[yyn]);

      /* Print the symbols being reduced, and their result.  */
      for (i = yyprhs[yyn]; yyrhs[i] > 0; i++)
	fprintf (stderr, "%s ", yytname[yyrhs[i]]);
      fprintf (stderr, " -> %s\n", yytname[definitionr1[yyn]]);
    }
#endif


  switch (yyn) {

case 1:
#line 159 "definition_parse.y"
{
		ResolveMacroReferences();  
		ResetVariables();
	;
    break;}
case 5:
#line 172 "definition_parse.y"
{
		definitionwarn("Skipping to next define");
		definitionParseOnly = true;
		YYABORT;
	;
    break;}
case 8:
#line 186 "definition_parse.y"
{
		// reset the skip definition parameter to allow more than
		// 1 sopclass definition in 1 file
		skipDefinition = false;	
	;
    break;}
case 16:
#line 205 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			defFileContent_ptr->SetSystem(lsystemname, lsystemversion);
		}
	;
    break;}
case 17:
#line 214 "definition_parse.y"
{ 
		strcpy(lsystemname, definitionval.string);
	;
    break;}
case 18:
#line 220 "definition_parse.y"
{ 
		strcpy(lsystemversion, definitionval.string);
	;
    break;}
case 19:
#line 227 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			defFileContent_ptr->SetAE(lAEname, lAEversion);
		}
	;
    break;}
case 20:
#line 236 "definition_parse.y"
{ 
		strcpy(lAEname, definitionval.string);
	;
    break;}
case 21:
#line 242 "definition_parse.y"
{ 
		strcpy(lAEversion, definitionval.string);
	;
    break;}
case 22:
#line 249 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			defFileContent_ptr->SetMetaSop(currentMetaSopClass_ptr);
			definitionfileempty = false;
		}
	;
    break;}
case 23:
#line 259 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			currentMetaSopClass_ptr = new DEF_METASOPCLASS_CLASS(lMetaSOPClassUID, lMetaSOPClassName);
		}
	;
    break;}
case 24:
#line 268 "definition_parse.y"
{ 
		strcpy(lMetaSOPClassUID, definitionval.string);
	;
    break;}
case 25:
#line 274 "definition_parse.y"
{ 
		strcpy(lMetaSOPClassName, definitionval.string);
	;
    break;}
case 26:
#line 280 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			currentMetaSopClass_ptr->AddSopClass(lSOPClassUID, lSOPClassName);
		}
	;
    break;}
case 27:
#line 287 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			currentMetaSopClass_ptr->AddSopClass(lSOPClassUID, lSOPClassName);
		}
	;
    break;}
case 29:
#line 299 "definition_parse.y"
{ 
		strcpy(lSOPClassUID, definitionval.string);	  
	;
    break;}
case 30:
#line 305 "definition_parse.y"
{ 
		strcpy(lSOPClassName, definitionval.string);	  
	;
    break;}
case 31:
#line 311 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentCommand_ptr->AddModule(currentModule_ptr);
			defFileContent_ptr->AddCommand(currentCommand_ptr);
			definitionfileempty = false;
		}
	;
    break;}
case 33:
#line 325 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// assume we have a command definition
			currentCommand_ptr = new DEF_COMMAND_CLASS();
			currentCommand_ptr->SetDimseCmd(definitionval.commandField);
			currentDICOMObject_ptr = currentCommand_ptr;

			lDimseCmd = definitionval.commandField;

			// create module for command attributes.
			currentModule_ptr = new DEF_MODULE_CLASS();
			currentModule_ptr->SetUsage(MOD_USAGE_M);
			currentModule_ptr->SetName(mapCommandName(lDimseCmd));                      
			currentAttributeGroup_ptr = currentModule_ptr;
 		}
	;
    break;}
case 34:
#line 345 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionfileempty = false;
		}
	;
    break;}
case 35:
#line 354 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// we're dealing with a dataset
			// delete previously created command & module
			if (currentCommand_ptr)
			{
				delete currentCommand_ptr;
				currentCommand_ptr = NULL;
			}
			if (currentModule_ptr)
			{
				delete currentModule_ptr;
				currentModule_ptr = NULL;
			}

			strcpy(lIodName, definitionval.string);
			currentDataset_ptr = new DEF_DATASET_CLASS(lIodName);
			currentDataset_ptr->SetDimseCmd(lDimseCmd);
			currentDICOMObject_ptr = currentDataset_ptr;
		}
	;
    break;}
case 36:
#line 379 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// add the dataset to the SOPClass Definition
            currentSopClass_ptr->AddDataset(currentDataset_ptr);
		}
	;
    break;}
case 37:
#line 387 "definition_parse.y"
{
		// the dataset is empty
	;
    break;}
case 38:
#line 393 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			if ((currentSopClass_ptr != NULL) &&
			    (currentSopClass_ptr->GetUid() == lSOPClassUID) &&
			    (currentSopClass_ptr->GetName() == lSOPClassName))
			{
				// Do not add a new SOP CLASS to the File Content as
				// the existing one has the same UID and Name.
				// We are probably adding another Command / Dataset
				// for the same SOP Class.
			}
			else
			{
				currentSopClass_ptr = new DEF_SOPCLASS_CLASS(lSOPClassUID, lSOPClassName);
				defFileContent_ptr->AddSopClass(currentSopClass_ptr);
			}
		}
	;
    break;}
case 41:
#line 419 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentDICOMObject_ptr->AddModule(currentModule_ptr);
		}
	;
    break;}
case 42:
#line 426 "definition_parse.y"
{
		// the module is empty - ignore it
	;
    break;}
case 43:
#line 432 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentModule_ptr = new DEF_MODULE_CLASS(lModuleName, lModuleUsage);
			currentModule_ptr->SetCondition(currentCondition_ptr);
			currentModule_ptr->SetTextualCondition(lTextualCondition);

			// reset condition
			currentCondition_ptr = NULL;
			lTextualCondition.erase();	
			currentAttributeGroup_ptr = currentModule_ptr;
		}
	;
    break;}
case 44:
#line 446 "definition_parse.y"
{
		// the module name and usage not present - ignore it
	;
    break;}
case 45:
#line 452 "definition_parse.y"
{
		strcpy(lModuleName, definitionval.string);
	;
    break;}
case 47:
#line 462 "definition_parse.y"
{
		lModuleUsage = definitionval.usage;					      
	;
    break;}
case 51:
#line 475 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttributeGroup_ptr->AddAttribute(currentAttribute_ptr);
		}
	;
    break;}
case 53:
#line 486 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr = new DEF_ATTRIBUTE_CLASS(lAttrGroup, lAttrElement);
		}
	;
    break;}
case 54:
#line 495 "definition_parse.y"
{ 
		lAttrGroup   = (UINT16) (definitionlval.hex >> 16);
		lAttrElement = (UINT16) (definitionval.hex & 0x0000FFFF);
	;
    break;}
case 55:
#line 502 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{ 
			switch(definitionval.integer)
			{
			case 1:
				currentAttribute_ptr->SetType(ATTR_TYPE_1);
				break;
			case 2: 
				currentAttribute_ptr->SetType(ATTR_TYPE_2);
				break;
			case 3: 
				currentAttribute_ptr->SetType(ATTR_TYPE_3);
				break;
			default: 
				break;
			}
		}
	;
    break;}
case 56:
#line 522 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetType(definitionval.type);
		}
	;
    break;}
case 57:
#line 531 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetName(definitionval.string);
		}
	;
    break;}
case 58:
#line 540 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			if (currentCondition_ptr != NULL)
			{
				currentAttribute_ptr->SetCondition(currentCondition_ptr);
								
				// reset condition pointer
				currentCondition_ptr = NULL;
			}
			else
			{
				if (!lTextualCondition.empty())
				{
					currentAttribute_ptr->SetTextualCondition(lTextualCondition);

					// reset textual condition
					lTextualCondition.erase();
				}
			}
		}
	;
    break;}
case 61:
#line 569 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// restore attribute group and remove from stack
			currentAttributeGroup_ptr = groups.top();
			groups.pop();

			// restore current attribute (the sequence attribute)
			currentAttribute_ptr = attributes.top();
			attributes.pop();          
		}
	;
    break;}
case 62:
#line 582 "definition_parse.y"
{
		// we were dealing with an empty item
		if (!definitionParseOnly && !skipDefinition)
		{
			// restore attribute group and remove from stack
			currentAttributeGroup_ptr = groups.top();
			groups.pop();

			// restore current attribute (the sequence attribute)
			currentAttribute_ptr = attributes.top();
			attributes.pop();          
		}
	;
    break;}
case 63:
#line 598 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetVR(ATTR_VR_SQ);

			currentItem_ptr = new DEF_ITEM_CLASS();

			// create new value and value list, add item
			currentValue_ptr = CreateNewValue(ATTR_VR_SQ);
			currentValue_ptr->Set(currentItem_ptr);

			// add value to attribute - indexed by value list index
    		currentAttribute_ptr->AddValue(currentValue_ptr, currentValueListIndex);
			currentAttribute_ptr->SetValueType(ATTR_VAL_TYPE_NOVALUE, currentValueListIndex);

			// the item is the new current attribute group. 
			// store the old one
			groups.push(currentAttributeGroup_ptr);
			currentAttributeGroup_ptr = currentItem_ptr;

			// store currentAttribute_ptr
			attributes.push(currentAttribute_ptr);
		}
	;
    break;}
case 64:
#line 624 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetVR(ATTR_VR_SQ);

			currentItem_ptr = new DEF_ITEM_CLASS();

			// create new value and value list, add item
			currentValue_ptr = CreateNewValue(ATTR_VR_SQ);
			currentValue_ptr->Set(currentItem_ptr);

			// add value to attribute - indexed by value list index
    		currentAttribute_ptr->AddValue(currentValue_ptr, currentValueListIndex);
			currentAttribute_ptr->SetValueType(ATTR_VAL_TYPE_NOVALUE, currentValueListIndex);

			// the item is the new current attribute group. 
			// store the old one
			groups.push(currentAttributeGroup_ptr);
			currentAttributeGroup_ptr = currentItem_ptr;

			// store currentAttribute_ptr
			attributes.push(currentAttribute_ptr);
		}
	;
    break;}
case 69:
#line 662 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) definitionvsp[0].integer); 
			currentAttribute_ptr->SetVmMax((UINT) definitionvsp[0].integer); 
		}
	;
    break;}
case 70:
#line 670 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) definitionvsp[-2].integer); 
			currentAttribute_ptr->SetVmMax((UINT) definitionvsp[0].integer); 
		}
	;
    break;}
case 71:
#line 678 "definition_parse.y"
{  
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) definitionvsp[-2].integer); 
			currentAttribute_ptr->SetVmMax((UINT) MAXVM); 
		}
	;
    break;}
case 72:
#line 686 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			CreateVMFromString(definitionval.string, currentAttribute_ptr);
		}
	;
    break;}
case 73:
#line 693 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) 0); 
			currentAttribute_ptr->SetVmMax((UINT) MAXVM); 
		}
	;
    break;}
case 74:
#line 703 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) definitionvsp[0].integer); 
			currentAttribute_ptr->SetImMax((UINT) definitionvsp[0].integer); 
		}
	;
    break;}
case 75:
#line 711 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) definitionvsp[-2].integer); 
			currentAttribute_ptr->SetImMax((UINT) definitionvsp[0].integer); 
		}
	;
    break;}
case 76:
#line 719 "definition_parse.y"
{  
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) definitionvsp[-2].integer); 
			currentAttribute_ptr->SetImMax((UINT) MAXVM); 
		}
	;
    break;}
case 77:
#line 727 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) 0); 
			currentAttribute_ptr->SetImMax((UINT) MAXVM); 
		}
	;
    break;}
case 78:
#line 737 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			lAttrVr = definitionval.vr;
			currentAttribute_ptr->SetVR(lAttrVr);
			}
	;
    break;}
case 79:
#line 745 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			lAttrVr = definitionvsp[-2].vr;
			currentAttribute_ptr->SetVR(lAttrVr);
			currentAttribute_ptr->SetSecondVR(definitionvsp[0].vr);
		}
	;
    break;}
case 80:
#line 756 "definition_parse.y"
{
		// empty
	;
    break;}
case 81:
#line 760 "definition_parse.y"
{
		// reset the list index for the next attribute values
		currentValueListIndex = 0;
	;
    break;}
case 82:
#line 765 "definition_parse.y"
{
		// reset the list index for the next attribute values
		currentValueListIndex = 0;
	;
    break;}
case 83:
#line 772 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// remember type of the first value list
			// this is the default type for other
			// value lists
			// add value to attribute - indexed by value list index
			currentAttribute_ptr->SetValueType(currentValueType, currentValueListIndex);
			currentValueListIndex++;

			lFirstValueType = currentValueType;
		}
	;
    break;}
case 86:
#line 792 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetValueType(currentValueType, currentValueListIndex);
			currentValueListIndex++;
		}
	;
    break;}
case 87:
#line 800 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// a ValuesList with no specified type gets the type
			// of the first ValuesList specified
			// add value to attribute - indexed by value list index
			currentAttribute_ptr->SetValueType(lFirstValueType, currentValueListIndex);
			currentValueListIndex++;
		}
	;
    break;}
case 88:
#line 813 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentValueType = definitionval.valueType;
		}
	;
    break;}
case 89:
#line 822 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
    		currentAttribute_ptr->AddValue(currentValue_ptr, currentValueListIndex);
		}
	;
    break;}
case 90:
#line 829 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{                         
    		currentAttribute_ptr->AddValue(currentValue_ptr, currentValueListIndex);
		}
	;
    break;}
case 91:
#line 838 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			currentValue_ptr = CreateValueFromString(definitionval.string, lAttrVr);
		}
	;
    break;}
case 92:
#line 845 "definition_parse.y"
{  
		if (!definitionParseOnly && !skipDefinition)
		{					   
			currentValue_ptr = CreateValueFromHex(definitionval.hex, lAttrVr);
		}
	;
    break;}
case 93:
#line 852 "definition_parse.y"
{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			currentValue_ptr = CreateValueFromInt(definitionval.integer, lAttrVr);
		}
	;
    break;}
case 95:
#line 864 "definition_parse.y"
{
	;
    break;}
case 96:
#line 869 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			if (currentSopClass_ptr)
			{
				currentSopClass_ptr->AddMacro(currentMacro_ptr);
			}

			// store macro pointers to be able to resolve references
			macros.push_back(currentMacro_ptr);
		}
	;
    break;}
case 97:
#line 882 "definition_parse.y"
{
		// Macro is empty - ignore it
	;
    break;}
case 98:
#line 888 "definition_parse.y"
{
		if (!definitionParseOnly)
		{
			currentMacro_ptr = new DEF_MACRO_CLASS(definitionvsp[0].string);
    		currentAttributeGroup_ptr = currentMacro_ptr;
		}
	;
    break;}
case 99:
#line 896 "definition_parse.y"
{
		// Macro name is not present - ignore it
		if (!definitionParseOnly)
		{
			currentMacro_ptr = new DEF_MACRO_CLASS("NO MACRO NAME - IGNORED");
    		currentAttributeGroup_ptr = currentMacro_ptr;
		}
	;
    break;}
case 101:
#line 910 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttributeGroup_ptr->AddMacroReference(definitionvsp[-1].string, currentCondition_ptr, lTextualCondition);
			groups_with_refs.push_back(currentAttributeGroup_ptr);

			// reset condition pointer
			currentCondition_ptr = NULL;
			
			// reset textual condition
			lTextualCondition.erase();	
		}
	;
    break;}
case 104:
#line 931 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			lTextualCondition = definitionvsp[0].string;
		
			// reset condition pointer
			currentCondition_ptr = NULL;	
		}
	;
    break;}
case 105:
#line 941 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// create new condition
			currentCondition_ptr = new CONDITION_CLASS();

			// add the primary root node to the condition
			condition_node_ptr = definitionvsp[0].node_ptr;
			currentCondition_ptr->SetPrimaryNode(condition_node_ptr);
			
			// reset textual condition
			lTextualCondition.erase();
		}
	;
    break;}
case 106:
#line 956 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			// create new condition
			currentCondition_ptr = new CONDITION_CLASS();

			// add the primary root node to the condition
			condition_node_ptr = definitionvsp[-3].node_ptr;
			currentCondition_ptr->SetPrimaryNode(condition_node_ptr);

			// add the secondary root node to the condition
			condition_node_ptr = definitionvsp[-1].node_ptr;
			currentCondition_ptr->SetSecondaryNode(condition_node_ptr);
			
			// reset textual condition
			lTextualCondition.erase();			
		}
	;
    break;}
case 107:
#line 977 "definition_parse.y"
{
		definitionval.node_ptr = definitionvsp[0].node_ptr;
	;
    break;}
case 108:
#line 983 "definition_parse.y"
{
		definitionval.node_ptr = definitionvsp[0].node_ptr;
	;
    break;}
case 109:
#line 989 "definition_parse.y"
{
		definitionval.node_ptr = definitionvsp[0].node_ptr;
	;
    break;}
case 110:
#line 993 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.node_ptr = new CONDITION_OR_NODE_CLASS(definitionvsp[-2].node_ptr, definitionvsp[0].node_ptr);
			definitionval.node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 111:
#line 1003 "definition_parse.y"
{
		definitionval.node_ptr = definitionvsp[0].node_ptr;
	;
    break;}
case 112:
#line 1007 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.node_ptr = new CONDITION_AND_NODE_CLASS(definitionvsp[-2].node_ptr, definitionvsp[0].node_ptr);
			definitionval.node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 113:
#line 1017 "definition_parse.y"
{							
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.node_ptr = new CONDITION_PRESENT_NODE_CLASS(definitionvsp[0].unary_node_ptr);
			definitionval.node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 114:
#line 1025 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
//			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);  
//			node_ptr->SetLogger(definitionfilelogger_ptr);    

//			$$ = new CONDITION_EMPTY_NODE_CLASS(node_ptr);
			definitionval.node_ptr = new CONDITION_EMPTY_NODE_CLASS(definitionvsp[0].unary_node_ptr);
			definitionval.node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 115:
#line 1037 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
//			node_left_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
//			node_left_ptr->SetLogger(definitionfilelogger_ptr);

			node_right_ptr = new CONDITION_LEAF_VALUE_NR_CLASS((UINT16)definitionvsp[-2].integer);
			node_right_ptr->SetLogger(definitionfilelogger_ptr);

//			node_left_ptr = new CONDITION_VALUE_NODE_CLASS(node_left_ptr, node_right_ptr);
			node_left_ptr = new CONDITION_VALUE_NODE_CLASS(definitionvsp[-3].unary_node_ptr, node_right_ptr);
			node_left_ptr->SetLogger(definitionfilelogger_ptr);

			node_right_ptr = new CONDITION_LEAF_CONST_CLASS(definitionvsp[0].string);
			node_right_ptr->SetLogger(definitionfilelogger_ptr);

			definitionvsp[-1].binary_node_ptr->SetLeft(node_left_ptr);
			definitionvsp[-1].binary_node_ptr->SetRight(node_right_ptr);

			definitionval.node_ptr = definitionvsp[-1].binary_node_ptr;
		}
	;
    break;}
case 116:
#line 1060 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.node_ptr = new CONDITION_NOT_NODE_CLASS(definitionvsp[0].node_ptr);
			definitionval.node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 117:
#line 1068 "definition_parse.y"
{
		definitionval.node_ptr = definitionvsp[-1].node_ptr;
	;
    break;}
case 118:
#line 1072 "definition_parse.y"
{
		definitionval.node_ptr = definitionvsp[-1].node_ptr;
		definitionval.node_ptr->SetConditionType(CONDITION_TYPE_WARNING);
	;
    break;}
case 119:
#line 1077 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.node_ptr = new CONDITION_LEAF_TRUE_CLASS(true);
			definitionval.node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 120:
#line 1085 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.node_ptr = new CONDITION_LEAF_TRUE_CLASS(false);
			definitionval.node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 121:
#line 1095 "definition_parse.y"
{ 
		definitionval.integer = APPLY_TO_ANY_VALUE;
	;
    break;}
case 122:
#line 1099 "definition_parse.y"
{
		definitionval.integer = definitionvsp[0].integer;
	;
    break;}
case 124:
#line 1108 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.binary_node_ptr = new CONDITION_EQ_NODE_CLASS();
			definitionval.binary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 125:
#line 1116 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.binary_node_ptr = new CONDITION_LESS_NODE_CLASS();
			definitionval.binary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 126:
#line 1124 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.binary_node_ptr = new CONDITION_GREATER_NODE_CLASS();
			definitionval.binary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 127:
#line 1132 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.binary_node_ptr = new CONDITION_LESS_EQ_NODE_CLASS();
			definitionval.binary_node_ptr->SetLogger(definitionfilelogger_ptr);
        }
	;
    break;}
case 128:
#line 1140 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.binary_node_ptr = new CONDITION_GREATER_EQ_NODE_CLASS();
			definitionval.binary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 129:
#line 1150 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);			
			definitionval.unary_node_ptr = new CONDITION_NAVIGATION_HERE_NODE_CLASS();
			definitionval.unary_node_ptr->SetLogger(definitionfilelogger_ptr);
			definitionval.unary_node_ptr->SetNode(node_ptr);
		}
	;
    break;}
case 130:
#line 1161 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = definitionvsp[0].unary_node_ptr;
		}
	;
    break;}
case 131:
#line 1170 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);
			definitionvsp[-1].unary_node_ptr->SetNode(node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-1].unary_node_ptr;
		}
	;
    break;}
case 132:
#line 1180 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-1].unary_node_ptr;
		}
	;
    break;}
case 133:
#line 1196 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-1].unary_node_ptr;
		}
	;
    break;}
case 134:
#line 1212 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-1].unary_node_ptr;
		}
	;
    break;}
case 135:
#line 1228 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			definitionvsp[-2].unary_node_ptr->SetNode(definitionvsp[-1].unary_node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-2].unary_node_ptr;
		}
	;
    break;}
case 136:
#line 1245 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-2].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(definitionvsp[-1].unary_node_ptr);

			lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-2].unary_node_ptr;
		}
	;
    break;}
case 137:
#line 1268 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-2].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(definitionvsp[-1].unary_node_ptr);

			lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-2].unary_node_ptr;
		}
	;
    break;}
case 138:
#line 1293 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = new CONDITION_NAVIGATION_HERE_NODE_CLASS(true);
			definitionval.unary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 139:
#line 1303 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = definitionvsp[0].unary_node_ptr;
		}
	;
    break;}
case 140:
#line 1310 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(definitionvsp[0].unary_node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-1].unary_node_ptr;
		}
	;
    break;}
case 141:
#line 1325 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = new CONDITION_NAVIGATION_UP_NODE_CLASS();
			definitionval.unary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 142:
#line 1335 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = definitionvsp[0].unary_node_ptr;
		}
	;
    break;}
case 143:
#line 1342 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(definitionvsp[0].unary_node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-1].unary_node_ptr;
		}
	;
    break;}
case 144:
#line 1357 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = new CONDITION_NAVIGATION_ROOT_NODE_CLASS();
			definitionval.unary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
case 145:
#line 1367 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = definitionvsp[0].unary_node_ptr;
		}
	;
    break;}
case 146:
#line 1374 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) definitionvsp[-1].unary_node_ptr;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(definitionvsp[0].unary_node_ptr);
			definitionval.unary_node_ptr = definitionvsp[-1].unary_node_ptr;
		}
	;
    break;}
case 147:
#line 1389 "definition_parse.y"
{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionval.unary_node_ptr = new CONDITION_NAVIGATION_DOWN_NODE_CLASS(lAttrGroup, lAttrElement);
			definitionval.unary_node_ptr->SetLogger(definitionfilelogger_ptr);
		}
	;
    break;}
}
   /* the action file gets copied in in place of this dollarsign */
#line 487 "bison.simple"

  definitionvsp -= yylen;
  definitionssp -= yylen;
#ifdef YYLSP_NEEDED
  definitionlsp -= yylen;
#endif

#if YYDEBUG != 0
  if (definitiondebug)
    {
      short *ssp1 = definitionss - 1;
      fprintf (stderr, "state stack now");
      while (ssp1 != definitionssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

  *++definitionvsp = definitionval;

#ifdef YYLSP_NEEDED
  definitionlsp++;
  if (yylen == 0)
    {
      definitionlsp->first_line = yylloc.first_line;
      definitionlsp->first_column = yylloc.first_column;
      definitionlsp->last_line = (definitionlsp-1)->last_line;
      definitionlsp->last_column = (definitionlsp-1)->last_column;
      definitionlsp->text = 0;
    }
  else
    {
      definitionlsp->last_line = (definitionlsp+yylen-1)->last_line;
      definitionlsp->last_column = (definitionlsp+yylen-1)->last_column;
    }
#endif

  /* Now "shift" the result of the reduction.
     Determine what state that goes to,
     based on the state we popped back to
     and the rule number reduced by.  */

  yyn = definitionr1[yyn];

  definitionstate = definitionpgoto[yyn - YYNTBASE] + *definitionssp;
  if (definitionstate >= 0 && definitionstate <= YYLAST && yycheck[definitionstate] == *definitionssp)
    definitionstate = yytable[definitionstate];
  else
    definitionstate = definitiondefgoto[yyn - YYNTBASE];

  goto yynewstate;

definitionerrlab:   /* here on detecting error */

  if (! yyerrstatus)
    /* If not already recovering from an error, report this error.  */
    {
      ++definitionnerrs;

#ifdef YYERROR_VERBOSE
      yyn = definitionpact[definitionstate];

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
	      definitionerror(msg);
	      free(msg);
	    }
	  else
	    definitionerror ("parse error; also virtual memory exceeded");
	}
      else
#endif /* YYERROR_VERBOSE */
	definitionerror("parse error");
    }

  goto definitionerrlab1;
definitionerrlab1:   /* here on error raised explicitly by an action */

  if (yyerrstatus == 3)
    {
      /* if just tried and failed to reuse lookahead token after an error, discard it.  */

      /* return failure if at end of input */
      if (definitionchar == YYEOF)
	YYABORT;

#if YYDEBUG != 0
      if (definitiondebug)
	fprintf(stderr, "Discarding token %d (%s).\n", definitionchar, yytname[definitionchar1]);
#endif

      definitionchar = YYEMPTY;
    }

  /* Else will try to reuse lookahead token
     after shifting the error token.  */

  yyerrstatus = 3;		/* Each real token shifted decrements this */

  goto yyerrhandle;

yyerrdefault:  /* current state does not do anything special for the error token. */

#if 0
  /* This is wrong; only states that explicitly want error tokens
     should shift them.  */
  yyn = definitiondefact[definitionstate];  /* If its default is to accept any token, ok.  Otherwise pop it.*/
  if (yyn) goto definitiondefault;
#endif

yyerrpop:   /* pop the current state because it cannot handle the error token */

  if (definitionssp == definitionss) YYABORT;
  definitionvsp--;
  definitionstate = *--definitionssp;
#ifdef YYLSP_NEEDED
  definitionlsp--;
#endif

#if YYDEBUG != 0
  if (definitiondebug)
    {
      short *ssp1 = definitionss - 1;
      fprintf (stderr, "Error: state stack now");
      while (ssp1 != definitionssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

yyerrhandle:

  yyn = definitionpact[definitionstate];
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
  if (definitiondebug)
    fprintf(stderr, "Shifting error token, ");
#endif

  *++definitionvsp = definitionlval;
#ifdef YYLSP_NEEDED
  *++definitionlsp = yylloc;
#endif

  definitionstate = yyn;
  goto yynewstate;
}
#line 1397 "definition_parse.y"



/*****************************************************************************/
/*
/* Private functions
/*
/*****************************************************************************/

static BASE_VALUE_CLASS* CreateValueFromString(NAME_STRING string_val, ATTR_VR_ENUM vr)
{
	BASE_VALUE_CLASS* value_ptr = NULL;

    switch (vr)
	{
         case ATTR_VR_AE:
         case ATTR_VR_AS:
         case ATTR_VR_CS:
         case ATTR_VR_DA:
         case ATTR_VR_DS:
         case ATTR_VR_DT:
         case ATTR_VR_IS:
         case ATTR_VR_LO:
         case ATTR_VR_LT:
		 case ATTR_VR_OB:
		 case ATTR_VR_OF:
		 case ATTR_VR_OW:
		 case ATTR_VR_OL:
		 case ATTR_VR_OD:
		 case ATTR_VR_OV:
         case ATTR_VR_PN:
         case ATTR_VR_SH:
         case ATTR_VR_ST:
         case ATTR_VR_TM:
         case ATTR_VR_UI:
		 case ATTR_VR_UN:
		 case ATTR_VR_UR:
		 case ATTR_VR_UC:
         case ATTR_VR_UT:
 			 {
				string data = string_val;
				value_ptr = CreateNewValue(vr);
				value_ptr->Set(data);
			 }
			 break;

         case ATTR_VR_AT:
	        definitionwarn("Attribute AT Value should be expressed in HEX format");
	        definitionwarn("Value not used - zero length taken");
			break;
         case ATTR_VR_FL:
			{
				float data = (float) atof(string_val);
				value_ptr = CreateNewValue(vr);
				value_ptr->Set(data);
			}
			break;
         case ATTR_VR_FD:
			{
				double data = atof(string_val);
				value_ptr = CreateNewValue(vr);
				value_ptr->Set(data);
			}
			break;
         case ATTR_VR_SL:
	 		definitionwarn("Attribute SL Value should be expressed in INTEGER format");
			definitionwarn("Value not used - zero length taken");
			break;
         case ATTR_VR_SS:
	 		definitionwarn("Attribute SS Value should be expressed in INTEGER format");
			definitionwarn("Value not used - zero length taken");
			break;
         case ATTR_VR_SV:
	 		definitionwarn("Attribute SV Value should be expressed in INTEGER format");
			definitionwarn("Value not used - zero length taken");
			break;
         case ATTR_VR_UL:
			definitionwarn("Attribute UL Value should be expressed in INTEGER or HEX format");
		    definitionwarn("Value not used - zero length taken");
			break;
         case ATTR_VR_US:
			definitionwarn("Attribute US Value should be expressed in INTEGER or HEX format");
		    definitionwarn("Value not used - zero length taken");
			break;
         case ATTR_VR_UV:
			definitionwarn("Attribute UV Value should be expressed in INTEGER or HEX format");
		    definitionwarn("Value not used - zero length taken");
			break;
         default:
			definitionwarn("Attribute Value not expected");
		    definitionwarn("Value not used - zero length taken");
		    break;
	}

	return value_ptr;
}

static BASE_VALUE_CLASS* CreateValueFromHex(unsigned long hexValue, ATTR_VR_ENUM vr)
{
	BASE_VALUE_CLASS* value_ptr = NULL;

	switch (vr) 
	{
	case ATTR_VR_AT:
		{
			UINT32 data = (UINT32) hexValue;
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
		break;
	case ATTR_VR_US:
		{
			UINT16 data = (UINT16) hexValue;
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
		break;
	case ATTR_VR_UL:
		{
			UINT32 data = (UINT32) hexValue;
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
		break;
	case ATTR_VR_SL:
	case ATTR_VR_SS:
		definitionwarn("Attribute SL/SS Value should be expressed in INTEGER format", false);
		definitionwarn("Value not used - zero length taken", false);
		break;
	default:
		definitionwarn("Attribute Value should be expressed in STRING format", false);
		definitionwarn("Value not used - zero length taken", false);
		break;
	}

	return value_ptr;
}

static BASE_VALUE_CLASS* CreateValueFromInt(int intValue, ATTR_VR_ENUM vr)
{
	BASE_VALUE_CLASS* value_ptr = NULL;

	switch (vr) {
	case ATTR_VR_SS:
		{
			INT16 data = (INT16) intValue;
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
		break;
	case ATTR_VR_SL:
		{
			INT32 data = (INT32) intValue;
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
		break;
	case ATTR_VR_US:
		{
			UINT16 data = (UINT16) intValue;
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
		break;
	case ATTR_VR_UL:
		{
			UINT32 data = (UINT32) intValue;
			value_ptr = CreateNewValue(vr);
			value_ptr->Set(data);
		}
		break;
	default:
		definitionwarn("Attribute Value should be expressed in STRING format", false);
		definitionwarn("Value not used - zero length taken", false);
		break;
	}

	return value_ptr;
}

static void CreateVMFromString(NAME_STRING vmstring, DEF_ATTRIBUTE_CLASS* attr_ptr)
{
    string            vm = vmstring;
	string            upper_vm = "0";
	string            lower_vm = "0";
    string::size_type pos = 0;
	
	// check presence of ':'
	pos = vm.find(":");
	if (pos != string::npos)
	{
	    lower_vm = vm.substr(0, pos); 
        upper_vm = vm.substr(pos+1);

		// only allow single integers in lower vm
		if ((pos = lower_vm.find_first_of("0123456789")) == string::npos ||
		      lower_vm.length() == 0 || 
			  lower_vm.length() > 1 )
	    {
            definitionwarn("Illegal Attribute VM, Only single integers for lower VM allowed", false);   
		}
		else
		{
            attr_ptr->SetVmMin( atoi(lower_vm.c_str()) );
		}

		// check whether upper vm is symmetric with lower vm
		if (upper_vm.substr(0,1) != lower_vm)
		{
            definitionwarn("Illegal Attribute VM, lower vm and upper vm should be symmetric", false);   
		}
		//only allow 1n, 2n, 3n in upper vm
		if (upper_vm == "1n")
		{
		    attr_ptr->SetVmMax((UINT) MAXVM);
         	attr_ptr->SetVmRestriction(ATTR_VM_RESTRICT_NONE);	
		}
		else if (upper_vm == "2n")
	    {
		    attr_ptr->SetVmMax((UINT) MAXVM);
			attr_ptr->SetVmRestriction(ATTR_VM_RESTRICT_EVEN);	
		}
		else if (upper_vm == "3n")
		{
		    attr_ptr->SetVmMax((UINT) MAXVM);
			attr_ptr->SetVmRestriction(ATTR_VM_RESTRICT_TRIPLE);
		}
		else
		{
            definitionwarn("Illegal Attribute VM, only 1n, 2n or 3n allowed for upper vm", false);   
		}
	}
	else
	{
        definitionwarn("Illegal Attribute VM, should contain ':' ", false);   
	}
}

static void ResolveMacroReferences()
{
	for (UINT i = 0; i < groups_with_refs.size(); i++)
	{
       for (UINT j = 0; j < macros.size(); j++)
	   {
	       string name = macros[j]->GetName();
           groups_with_refs[i]->ResolveMacroReference(name, macros[j]);        
	   }
	}   
}

static void ResetVariables()
{
	// Reset static variables for next definition file parsing
	currentMetaSopClass_ptr = NULL;
	currentSopClass_ptr = NULL;
	currentDICOMObject_ptr = NULL;
	currentCommand_ptr = NULL;
	currentDataset_ptr = NULL;
	currentAttributeGroup_ptr = NULL;
	currentModule_ptr = NULL;
	currentItem_ptr = NULL;
	currentMacro_ptr = NULL;
	currentAttribute_ptr = NULL;
	currentValueListIndex = 0;
	currentValue_ptr = NULL;
	currentCondition_ptr = NULL;
	condition_node_ptr = NULL;
	node_left_ptr = NULL;
	node_right_ptr = NULL;

    lsystemname[0] = '\0';
    lsystemversion[0] = '\0';
    lAEname[0] = '\0';
    lAEversion[0] = '\0';
    lMetaSOPClassUID[0] = '\0';
    lMetaSOPClassName[0] = '\0';
    lSOPClassUID[0] = '\0';
    lSOPClassName[0] = '\0';
    lIodName[0] = '\0';
    lModuleName[0] = '\0';

	// clear stacks and references
    for (UINT i = 0; i < groups.size(); i++)
	{
		groups.pop();
	}

    for (UINT i = 0; i < attributes.size(); i++)
	{
		attributes.pop();
	}

    groups_with_refs.clear();
    macros.clear();

    definitionfilename.empty();
	definitionnewfile = false;

	definitionParseOnly = false;
	skipDefinition = false;
}

void resetDefinitionParser()
{
	ResetVariables();
}