%option noyywrap

%{
// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2006
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

#ifdef _WINDOWS
#pragma warning (disable : 4102)
#pragma warning (disable : 4273)
#pragma warning (disable : 4786)
#endif

/*
 *	contains the Lexer for the ODD output
 */
#include <stdlib.h>
#include <ctype.h>
#include <string.h>
#include <stdio.h>

#define YY_NEVER_INTERACTIVE 1 

#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Icondition.h"		// Condition component interface
#include "definition_parse.cpp.h"

#define ODD_DIM_OF(a) (sizeof((a))/sizeof((a)[0]))
#define RETURN(a) tokenType = (a); return((a));
#define yylval  definitionlval
#define yyerror definitionerror
#define yywarn  definitionwarn

void yywarn(char *msg);
void yyerror(char* msg);

//Export variables for logging purposes
LOG_CLASS* definitionfilelogger_ptr = 0;
UINT       definitionerrors = 0;
UINT       definitionwarnings = 0;

//Extern variables
extern bool definitionParseOnly;

//Variables to keep track of current line and the 
//position of the token in this line 
static char prev_linebuf[MAXIMUM_LINE_LENGTH];
static char curr_linebuf[MAXIMUM_LINE_LENGTH];

bool IsHex(char, int*);

static int tokenType = '\0';
char*   End_ptr;

typedef struct {
    int tokenType;
	char* tokenName;
} TOKEN_MAP_STRUCT;


static TOKEN_MAP_STRUCT tokenTable[] = {
{ T_SYSTEM, "T_SYSTEM"},
{ T_DEFINE, "T_DEFINE"},
{ T_ENDDEFINE, "T_ENDDEFINE"},
{ T_METASOPCLASS, "T_METASOPCLASS"},
{ T_SOPCLASS, "T_SOPCLASS"},
{ T_MODULE,	"T_MODULE"},
{ T_MACRO, "T_MACRO"},
{ T_INCLUDEMACRO, "T_INCLUDEMACRO"},
{ T_SQ, "T_SQ"},		
{ T_AND, "T_AND"},
{ T_OR,  "T_OR"},
{ T_NOT, "T_NOT"},
{ T_PRESENT, "T_PRESENT"},
{ T_VALUE, "T_VALUE"},
{ T_EMPTY, "T_EMPTY"},
{ T_TRUE, "T_TRUE"},  
{ T_FALSE, "T_FALSE"},  
{ T_EQUAL, "T_EQUAL"},
{ T_LESS, "T_LESS"},
{ T_GREATER, "T_GREATER"},
{ T_LESS_OR_EQUAL, "T_LESS_OR_EQUAL"},
{ T_GREATER_OR_EQUAL, "T_GREATER_OR_EQUAL"},
{ T_WEAK, "T_WEAK"},
{ T_WARN, "T_WARN"},
{ STRING, "STRING"},
{ INTEGER, "INTEGER"},
{ HEX, "HEX"},
{ COMMANDFIELD, "COMMANDFIELD"},
{ TYPE, "TYPE"},
{ USAGE, "USAGE"},
{ VALUETYPE, "VALUETYPE"},
{ VR, "VR"}
};


%}

%array

%%
[\200-\377]				{ /* skip charcters from 80H - FFH */ 
						}
^[ \t]*#.*\n			{ /* skip comment lines */ 
						}
^[ \t]*\n				{ /* skip empty lines */ 
						}
#.*						{ /* skip comment */ }
[ \t\n\r]+				{ /* skip white space */ }
^[ \t]*\n		        { /* skip empty lines */ }

\n.*					{ 
                          // skip newlines  
                          //Reset token position and
						  //save the next line
						  strcpy(prev_linebuf, curr_linebuf);
						  strcpy(curr_linebuf, yytext+1);
						  yyless(1); // give back all but the \n to rescan 
                        }
\\\n					{ /* continuation line */ 
						}

D			{ yylval.valueType = ATTR_VAL_TYPE_DEFINED;     RETURN(VALUETYPE); 	}
E			{ yylval.valueType = ATTR_VAL_TYPE_ENUMERATED;  RETURN(VALUETYPE);  }
DL			{ yylval.valueType = ATTR_VAL_TYPE_DEFINED_LIST;     RETURN(VALUETYPE); }
EL			{ yylval.valueType = ATTR_VAL_TYPE_ENUMERATED_LIST;  RETURN(VALUETYPE); }

C			{ yylval.usage = MOD_USAGE_C; RETURN(USAGE); }
M			{ yylval.usage = MOD_USAGE_M; RETURN(USAGE); }
U			{ yylval.usage = MOD_USAGE_U; RETURN(USAGE); }

1C			{ yylval.type = ATTR_TYPE_1C; RETURN(TYPE); }
2C			{ yylval.type = ATTR_TYPE_2C; RETURN(TYPE); }
3C			{ yylval.type = ATTR_TYPE_3C; RETURN(TYPE); }
3R			{ yylval.type = ATTR_TYPE_3R; RETURN(TYPE); }

AE			{ yylval.vr = ATTR_VR_AE; RETURN(VR); }
AS			{ yylval.vr = ATTR_VR_AS; RETURN(VR); }
AT			{ yylval.vr = ATTR_VR_AT; RETURN(VR); }
CS			{ yylval.vr = ATTR_VR_CS; RETURN(VR); }
DA			{ yylval.vr = ATTR_VR_DA; RETURN(VR); }
DS			{ yylval.vr = ATTR_VR_DS; RETURN(VR); }
DT			{ yylval.vr = ATTR_VR_DT; RETURN(VR); }
FD			{ yylval.vr = ATTR_VR_FD; RETURN(VR); }
FL			{ yylval.vr = ATTR_VR_FL; RETURN(VR); }
IS			{ yylval.vr = ATTR_VR_IS; RETURN(VR); }
LO			{ yylval.vr = ATTR_VR_LO; RETURN(VR); }
LT			{ yylval.vr = ATTR_VR_LT; RETURN(VR); }
OB			{ yylval.vr = ATTR_VR_OB; RETURN(VR); }
OF			{ yylval.vr = ATTR_VR_OF; RETURN(VR); }
OW			{ yylval.vr = ATTR_VR_OW; RETURN(VR); }
OL			{ yylval.vr = ATTR_VR_OL; RETURN(VR); }
OV			{ yylval.vr = ATTR_VR_OV; RETURN(VR); }
OD			{ yylval.vr = ATTR_VR_OD; RETURN(VR); }
PN			{ yylval.vr = ATTR_VR_PN; RETURN(VR); }
SH			{ yylval.vr = ATTR_VR_SH; RETURN(VR); }
SL			{ yylval.vr = ATTR_VR_SL; RETURN(VR); }
SQ			{ yylval.vr = ATTR_VR_SQ; RETURN(T_SQ); }
SS			{ yylval.vr = ATTR_VR_SS; RETURN(VR); }
ST			{ yylval.vr = ATTR_VR_ST; RETURN(VR); }
SV			{ yylval.vr = ATTR_VR_SV; RETURN(VR); }
TM			{ yylval.vr = ATTR_VR_TM; RETURN(VR); }
UI			{ yylval.vr = ATTR_VR_UI; RETURN(VR); }
UL			{ yylval.vr = ATTR_VR_UL; RETURN(VR); }
UN			{ yylval.vr = ATTR_VR_UN; RETURN(VR); }
US			{ yylval.vr = ATTR_VR_US; RETURN(VR); }
UT			{ yylval.vr = ATTR_VR_UT; RETURN(VR); }
UR			{ yylval.vr = ATTR_VR_UR; RETURN(VR); }
UV			{ yylval.vr = ATTR_VR_UV; RETURN(VR); }
UC			{ yylval.vr = ATTR_VR_UC; RETURN(VR); }

C-ECHO-RQ			{ yylval.commandField = DIMSE_CMD_CECHO_RQ;         RETURN(COMMANDFIELD); }
C-ECHO-RSP			{ yylval.commandField = DIMSE_CMD_CECHO_RSP;        RETURN(COMMANDFIELD); }
C-FIND-RQ			{ yylval.commandField = DIMSE_CMD_CFIND_RQ;         RETURN(COMMANDFIELD); }
C-FIND-RSP			{ yylval.commandField = DIMSE_CMD_CFIND_RSP;        RETURN(COMMANDFIELD); }
C-GET-RQ			{ yylval.commandField = DIMSE_CMD_CGET_RQ;          RETURN(COMMANDFIELD); }
C-GET-RSP			{ yylval.commandField = DIMSE_CMD_CGET_RSP;         RETURN(COMMANDFIELD); }
C-MOVE-RQ			{ yylval.commandField = DIMSE_CMD_CMOVE_RQ;         RETURN(COMMANDFIELD); }
C-MOVE-RSP			{ yylval.commandField = DIMSE_CMD_CMOVE_RSP;        RETURN(COMMANDFIELD); }
C-STORE-RQ			{ yylval.commandField = DIMSE_CMD_CSTORE_RQ;        RETURN(COMMANDFIELD); }
C-STORE-RSP			{ yylval.commandField = DIMSE_CMD_CSTORE_RSP;       RETURN(COMMANDFIELD); }
C-CANCEL-RQ			{ yylval.commandField = DIMSE_CMD_CCANCEL_RQ;       RETURN(COMMANDFIELD); }
N-ACTION-RQ			{ yylval.commandField = DIMSE_CMD_NACTION_RQ;       RETURN(COMMANDFIELD); }
N-ACTION-RSP		{ yylval.commandField = DIMSE_CMD_NACTION_RSP;      RETURN(COMMANDFIELD); }
N-CREATE-RQ			{ yylval.commandField = DIMSE_CMD_NCREATE_RQ;       RETURN(COMMANDFIELD); }
N-CREATE-RSP		{ yylval.commandField = DIMSE_CMD_NCREATE_RSP;      RETURN(COMMANDFIELD); }
N-DELETE-RQ			{ yylval.commandField = DIMSE_CMD_NDELETE_RQ;       RETURN(COMMANDFIELD); }
N-DELETE-RSP		{ yylval.commandField = DIMSE_CMD_NDELETE_RSP;      RETURN(COMMANDFIELD); }
N-EVENT-REPORT-RQ	{ yylval.commandField = DIMSE_CMD_NEVENTREPORT_RQ;  RETURN(COMMANDFIELD); }
N-EVENT-REPORT-RSP	{ yylval.commandField = DIMSE_CMD_NEVENTREPORT_RSP; RETURN(COMMANDFIELD); }
N-GET-RQ			{ yylval.commandField = DIMSE_CMD_NGET_RQ;          RETURN(COMMANDFIELD); }
N-GET-RSP			{ yylval.commandField = DIMSE_CMD_NGET_RSP;         RETURN(COMMANDFIELD); }
N-SET-RQ			{ yylval.commandField = DIMSE_CMD_NSET_RQ;          RETURN(COMMANDFIELD); }
N-SET-RSP			{ yylval.commandField = DIMSE_CMD_NSET_RSP;         RETURN(COMMANDFIELD); }


DEFINE						 { RETURN(T_DEFINE); }
ENDDEFINE					 { RETURN(T_ENDDEFINE); } 
SYSTEM						 { RETURN(T_SYSTEM); }
METASOPCLASS				 { RETURN(T_METASOPCLASS); }
SOPCLASS					 { RETURN(T_SOPCLASS); }
MODULE						 { RETURN(T_MODULE); }
ITEM						 { RETURN(T_ITEM); }
INCLUDEITEM					 { RETURN(T_INCLUDEITEM); }
MACRO						 { RETURN(T_MACRO); }
INCLUDEMACRO				 { RETURN(T_INCLUDEMACRO); }
"="							 { RETURN(T_EQUAL); }
"<"							 { RETURN(T_LESS); }
">"							 { RETURN(T_GREATER); }
"<="						 { RETURN(T_LESS_OR_EQUAL);}
">="						 { RETURN(T_GREATER_OR_EQUAL);}
WEAK						 { RETURN(T_WEAK);}
WARN						 { RETURN(T_WARN);}
AND							 { RETURN(T_AND);}
OR							 { RETURN(T_OR);}
NOT							 { RETURN(T_NOT);}
PRESENT						 { RETURN(T_PRESENT);}
EMPTY						 { RETURN(T_EMPTY);}
TRUE						 { RETURN(T_TRUE);}
FALSE						 { RETURN(T_FALSE);}
VALUE						 { RETURN(T_VALUE);}

[0-9]:[0-9]+n           {
                            strcpy(yylval.string, yytext);
                            RETURN(STRING);
						}


0(x|X)[0-9a-f]+		|
0(x|X)[0-9A-F]+			{ 
                            yylval.hex  = strtoul(yytext, &End_ptr, 16);
                            RETURN(HEX); 
						}
(\+|\-)[0-9]+	    |
[0-9]+					{ 
                            yylval.integer = atoi(yytext); 
							RETURN(INTEGER); 
						}
\"[^\"\\\n]*\\\n		{ 
                            for (yyleng = yyleng-2; yyleng >= 0; yyleng--)
							{
						        unput(yytext[yyleng]); /* concatenate line */ 
							}
                        }
\"[^\"\n]*\"			{ 
                            if ((yyleng-2) >= sizeof(yylval.string))
						    {
						        yywarn("STRING too long!!! truncated") ;
			                    yyleng = sizeof(yylval.string) +1 ;
						    }
						    memcpy(yylval.string, &yytext[1], yyleng-2); 
                            yylval.string[yyleng-2] = '\0';
						    RETURN(STRING);
						}
.						{ 
                            RETURN(yytext[0]); 
						}
%%

void yyerror(char *errmsg)
{
	if ( definitionfilelogger_ptr)
	{
		definitionfilelogger_ptr->text(LOG_ERROR, 1, "lineno: %d %s: ",
									   yylineno,
									   errmsg);
		definitionfilelogger_ptr->text(LOG_NONE, 1, "Current line: ");
		definitionfilelogger_ptr->text(LOG_NONE, 1, "%-50s...",curr_linebuf);
	}

    //Switch to parse only mode and try to find more errors
    definitionParseOnly = true;
	definitionerrors++;
	
	if (definitionerrors > 10)
	{
		if ( definitionfilelogger_ptr)
		{
			definitionfilelogger_ptr->text(LOG_ERROR, 1, "Unable to recover from previous errors. Stopping...");
		}
		exit(1);
	}
}

void yyerror(char *errmsg, bool PrintCurrentLine)
{
    char linebuf[MAXIMUM_LINE_LENGTH];
	if (PrintCurrentLine)
	{
        strcpy(linebuf, curr_linebuf);
	}
	else
	{
		strcpy(linebuf, prev_linebuf);
	}

	if ( definitionfilelogger_ptr)
	{
		definitionfilelogger_ptr->text(LOG_ERROR, 1, "lineno: %d %s: ",
									   yylineno,
									   errmsg);
		definitionfilelogger_ptr->text(LOG_NONE, 1, "Current line: ");
		definitionfilelogger_ptr->text(LOG_NONE, 1, "%-50s...",curr_linebuf);
	}

    //Switch to parse only mode and try to find more errors
    definitionParseOnly = true;
	definitionerrors++;
}

void yywarn(char *msg)
{
	if ( definitionfilelogger_ptr)
	{
		definitionfilelogger_ptr->text(LOG_WARNING, 1, "lineno: %d %s: ",
									   yylineno,
									   msg);
		definitionfilelogger_ptr->text(LOG_NONE, 1, "Current line: ");
		definitionfilelogger_ptr->text(LOG_NONE, 1, "%-50s...",curr_linebuf);
	}

	definitionwarnings++;
}

void yywarn(char *msg, bool PrintCurrentLine)
{
    char linebuf[MAXIMUM_LINE_LENGTH];
	if (PrintCurrentLine)
	{
        strcpy(linebuf, curr_linebuf);
	}
	else
	{
		strcpy(linebuf, prev_linebuf);
	}

	if ( definitionfilelogger_ptr)
	{
		definitionfilelogger_ptr->text(LOG_WARNING, 1, "lineno: %d %s: ",
									   yylineno,
									   msg);
		definitionfilelogger_ptr->text(LOG_NONE, 1, "Current line: ");
		definitionfilelogger_ptr->text(LOG_NONE, 1, "%-50s...",curr_linebuf);
	}

	definitionwarnings++;
}

bool IsHex(char digit, int *value)
{
	bool isHex = true;
	
    if (isdigit(digit)) 
	{
		*value = digit - '0';
	}
	else if (('a' <= digit) && (digit <= 'f')) 
	{
		*value = digit - 'W';
	}
	else if (('A' <= digit) && (digit <= 'F')) 
	{
		*value = digit - '7';
	}
	else 
	{
		isHex = false;
	}

	return isHex;
}
