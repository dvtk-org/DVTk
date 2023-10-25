%{
// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2006
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

//*****************************************************************************
//  DESCRIPTION     :	Script Lexer
//*****************************************************************************

#ifdef _WINDOWS
#pragma warning (disable : 4102)
#pragma warning (disable : 4273)
#pragma warning (disable : 4786)
#endif

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <io.h>
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface
#include "Iwarehouse.h"		// Warehouse component interface
#include "Idicom.h"			// Dicom component interface
#include "Inetwork.h"		// Network component interface
#include "Imedia.h"			// Media File component interface
#include "Isession.h"		// Test Session component interface

#include "script_parser.cpp.h"

extern void scripterror(char*);

extern FILE					*scriptin;
extern int					scriptlineno;
extern char					scriptCurrentFilename[_MAX_PATH];
extern long					scriptCurrentFileOffset;
extern long					scriptCurrentLineNo;
extern SCRIPT_SESSION_CLASS	*scriptSession_ptr;
extern bool                 scriptIsNativeVts;
extern bool                 scriptParseOnly;

bool IsHex(char, int *);

static char *pEnd;

%}

%option noyywrap

%e 1000			// max number of nodes
%p 3000			// max number of positions
%n 500			// max number of states
%k 100			// max number of packed char classes
%a 1000 		// max number of transitions
%o 1000 		// max number of output slots

%%
[\200-\377]		{ /* skip charcters from 80H - FFH */ 
				}
^[ \t]*##.*\n	{ 
					// copy script comment lines - strip off leading ##
					int length = scriptleng;
			 
					// check to see how line is terminated
					// - and remove termination
					if ((scripttext[scriptleng-1] == LINEFEED)
					 && (scripttext[scriptleng-2] == CARRIAGERETURN))
					    length -= 2;
					else
						length--;
				
					scriptlval.string_ptr = (char*) malloc(length+1);

					memcpy(scriptlval.string_ptr, &scripttext[0], length);
					scriptlval.string_ptr[length] = NULLCHAR;
					if (scriptSession_ptr)
					{
						LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
						if (logger_ptr)
						{
							logger_ptr->text(LOG_NONE, 1, "%s", scriptlval.string_ptr);
						}
					}
					
					free(scriptlval.string_ptr);
					scriptlval.string_ptr = NULL;
				}

^#[ ][0-9]+.*\n	 {	/* extract file and line info from cpp comment lines: # Number ["file"] */
					char* iS = NULL;
					char* nS = NULL;
					iS = strstr(scripttext, "# ");
					if (iS != NULL)
					{
						nS = strstr(iS, " \"");
						if (nS != NULL)
						{
							*nS++ = '\0';
							strcpy(scriptCurrentFilename, nS);
						}
						scriptCurrentFileOffset = atoi(iS+2); 
						scriptCurrentLineNo = scriptlineno;
					}
				}
^[ \t]*#.*\n	{ 
                  char* test;
				  test = 0;
                  /* skip other comments */ 
				}
#.*\n			{ 
                  char* test;
				  test = 0;
                  /* skip other comments */ 
				}
^[ \t]*\n		{ /* skip empty lines */ }
[ \t\n\r]+		{ /* skip white space */ }
\\\n			{ /* continuation line */ }
=				{ /* skip equal sign */ }
;				{ /* skip semi-colon */ }
\{				{ return T_OPEN_BRACKET; }
\(				{ return T_OPEN_BRACKET; }
\}				{ return T_CLOSE_BRACKET; }
\)				{ return T_CLOSE_BRACKET; }
AE				{ scriptlval.vr = ATTR_VR_AE; return VR; }
AS				{ scriptlval.vr = ATTR_VR_AS; return VR; }
AT				{ scriptlval.vr = ATTR_VR_AT; return VR; }
CS				{ scriptlval.vr = ATTR_VR_CS; return VR; }
DA				{ scriptlval.vr = ATTR_VR_DA; return VR; }
DS				{ scriptlval.vr = ATTR_VR_DS; return VR; }
DT				{ scriptlval.vr = ATTR_VR_DT; return VR; }
FD				{ scriptlval.vr = ATTR_VR_FD; return VR; }
FL				{ scriptlval.vr = ATTR_VR_FL; return VR; }
IS				{ scriptlval.vr = ATTR_VR_IS; return VR; }
LO				{ scriptlval.vr = ATTR_VR_LO; return VR; }
LT				{ scriptlval.vr = ATTR_VR_LT; return VR; }
OB				{ scriptlval.vr = ATTR_VR_OB; return VR; }
OF				{ scriptlval.vr = ATTR_VR_OF; return VR; }
OV				{ scriptlval.vr = ATTR_VR_OV; return VR; }
OW				{ scriptlval.vr = ATTR_VR_OW; return VR; }
PN				{ scriptlval.vr = ATTR_VR_PN; return VR; }
SH				{ scriptlval.vr = ATTR_VR_SH; return VR; }
SL				{ scriptlval.vr = ATTR_VR_SL; return VR; }
SQ				{ scriptlval.vr = ATTR_VR_SQ; return T_SQ; }
SS				{ scriptlval.vr = ATTR_VR_SS; return VR; }
ST				{ scriptlval.vr = ATTR_VR_ST; return VR; }
TM				{ scriptlval.vr = ATTR_VR_TM; return VR; }
UI				{ scriptlval.vr = ATTR_VR_UI; return VR; }
UL				{ scriptlval.vr = ATTR_VR_UL; return VR; }
UN				{ scriptlval.vr = ATTR_VR_UN; return VR; }
UR				{ scriptlval.vr = ATTR_VR_UR; return VR; }
US				{ scriptlval.vr = ATTR_VR_US; return VR; }
UT				{ scriptlval.vr = ATTR_VR_UT; return VR; }

SEND			{ return T_SEND; }

RESET			{ return T_RESET; }
CREATE			{ return T_CREATE; }
SET				{ return T_SET; }
COMPARE 		{ return T_COMPARE; }
COMPARE_NOT		{ return T_COMPARE_NOT; }
COPY			{ return T_COPY; }
DELETE			{ return T_DELETE; }

READ			{ return T_READ; }
WRITE			{ return T_WRITE; }

SYSTEM			{ return T_SYSTEM; }
TIME			{ return T_TIME; }
VERBOSE			{ return T_VERBOSE; }
DISPLAY			{ return T_DISPLAY; }
ECHO			{ return T_ECHO; }
ROLE			{ return T_ROLE; }
CONFIRM			{ return T_CONFIRM; }
DELAY			{ return T_DELAY; }

APPLICATION-ENTITY	{ return T_APPL_ENTITY; }
ADD-GROUP-LENGTH	{ return T_ADD_GROUP_LENGTH; }
DEFINE-SQ-LENGTH	{ return T_DEF_SQ_LENGTH; }
POPULATE			{ return T_POPULATE; }
STRICT-VALIDATION	{ return T_STRICT; }
VERBOSE				{ return T_VERBOSE; }
VALIDATION			{ return T_VALIDATION; }
ENABLED					{ scriptlval.validationFlag = ALL; return VALIDATIONFLAG; }
ENABLED-USE-DEF-ONLY	{ scriptlval.validationFlag = USE_DEFINITION; return VALIDATIONFLAG; }
ENABLED-USE-VR-ONLY		{ scriptlval.validationFlag = USE_VR; return VALIDATIONFLAG; }
ENABLED-USE-REF-ONLY	{ scriptlval.validationFlag = USE_REFERENCE; return VALIDATIONFLAG; }
ENABLED-USE-DEF-AND-VR	{ scriptlval.validationFlag = (VALIDATION_CONTROL_FLAG_ENUM)(USE_DEFINITION | USE_VR); return VALIDATIONFLAG; }
ENABLED-USE-DEF-AND-REF	{ scriptlval.validationFlag = (VALIDATION_CONTROL_FLAG_ENUM)(USE_DEFINITION | USE_REFERENCE); return VALIDATIONFLAG; }
ENABLED-USE-VR-AND-REF	{ scriptlval.validationFlag = (VALIDATION_CONTROL_FLAG_ENUM)(USE_VR | USE_REFERENCE); return VALIDATIONFLAG; }
DISABLED				{ scriptlval.validationFlag = NONE; return VALIDATIONFLAG; }

LANGUAGE        { return T_LANGUAGE; }
                  /* 
				   * The LANGUAGE token  is introduced to enable backward compatibility with
				   * VTS and ADVT
				   * Currently the ADVT language is fully supported by the parser
				   * To avoid conflicts and to have a more consistent language for DVT
				   * this language section has been introduced. Here some mappings between VTS 
				   * language tokens and DVT language tokens are provided.
				   */

VALIDATE        {
                   /* 
				    * The VTS VALIDATE command is mapped on the DVT RECEIVE Command
					*/
					if (scriptIsNativeVts)
					{
					    return T_RECEIVE;
					}
					else
					{
                        return T_VALIDATE;
					}
                 } 
VALDIMSE         {
                   /* 
				    * The VTS VALDIMSE command is mapped on the DVT VALIDATE Command
					*/
					if (scriptIsNativeVts)
					{
					    return T_VALIDATE;
					}
					else
					{
					    /* VALDIMSE is not part of the DVT language anymore */
						scripterror("\"VALDIMSE\" is not a DVT script command. Use \"VALIDATE\" ");
					}
                 }

RECEIVE          {
                   /* 
				    * The VTS RECEIVE command is mapped on the DVT IMPORT Command
					*/
					if (scriptIsNativeVts)
					{
					    return T_IMPORT;
					}
					else
					{
					    return T_RECEIVE;
					}
                 }

EXECUTE          {
                   /* 
				    * The VTS EXECUTE command is mapped on the DVT EXPORT Command
					*/
					if (scriptIsNativeVts)
					{
					    return T_EXPORT;
					}
					else
					{
					    /* EXECUTE is not part of the DVT language anymore */
						scripterror("\"EXECUTE\" is not a DVT script command. Use \"EXPORT\" ");
					}
                 }

IMPORT           {
                   return T_IMPORT;
                 }

EXPORT           {
                   return T_EXPORT;
                 }

C_ECHO_RQ		|
C-ECHO-RQ		{ scriptlval.commandField = DIMSE_CMD_CECHO_RQ; return COMMANDFIELD; }
C_ECHO_RSP		|
C-ECHO-RSP		{ scriptlval.commandField = DIMSE_CMD_CECHO_RSP; return COMMANDFIELD; }
C_FIND_RQ		|
C-FIND-RQ		{ scriptlval.commandField = DIMSE_CMD_CFIND_RQ; return COMMANDFIELD; }
C_FIND_RSP		|
C-FIND-RSP		{ scriptlval.commandField = DIMSE_CMD_CFIND_RSP; return COMMANDFIELD; }
C_GET_RQ		|
C-GET-RQ		{ scriptlval.commandField = DIMSE_CMD_CGET_RQ; return COMMANDFIELD; }
C_GET_RSP		|
C-GET-RSP		{ scriptlval.commandField = DIMSE_CMD_CGET_RSP; return COMMANDFIELD; }
C_MOVE_RQ		|
C-MOVE-RQ		{ scriptlval.commandField = DIMSE_CMD_CMOVE_RQ; return COMMANDFIELD; }
C_MOVE_RSP		|
C-MOVE-RSP		{ scriptlval.commandField = DIMSE_CMD_CMOVE_RSP; return COMMANDFIELD; }
C_STORE_RQ		|
C-STORE-RQ		{ scriptlval.commandField = DIMSE_CMD_CSTORE_RQ; return COMMANDFIELD; }
C_STORE_RSP		|
C-STORE-RSP		{ scriptlval.commandField = DIMSE_CMD_CSTORE_RSP; return COMMANDFIELD; }
C_CANCEL_RQ		|
C-CANCEL-RQ		{ scriptlval.commandField = DIMSE_CMD_CCANCEL_RQ; return COMMANDFIELD; }
N_ACTION_RQ		|
N-ACTION-RQ		{ scriptlval.commandField = DIMSE_CMD_NACTION_RQ; return COMMANDFIELD; }
N_ACTION_RSP	|
N-ACTION-RSP	{ scriptlval.commandField = DIMSE_CMD_NACTION_RSP; return COMMANDFIELD; }
N_CREATE_RQ		|
N-CREATE-RQ		{ scriptlval.commandField = DIMSE_CMD_NCREATE_RQ; return COMMANDFIELD; }
N_CREATE_RSP	|
N-CREATE-RSP	{ scriptlval.commandField = DIMSE_CMD_NCREATE_RSP; return COMMANDFIELD; }
N_DELETE_RQ		|
N-DELETE-RQ		{ scriptlval.commandField = DIMSE_CMD_NDELETE_RQ; return COMMANDFIELD; }
N_DELETE_RSP	|
N-DELETE-RSP	{ scriptlval.commandField = DIMSE_CMD_NDELETE_RSP; return COMMANDFIELD; }
N_EVENT_REPORT_RQ	|
N-EVENT-REPORT-RQ	{ scriptlval.commandField = DIMSE_CMD_NEVENTREPORT_RQ; return COMMANDFIELD; }
N_EVENT_REPORT_RSP	|
N-EVENT-REPORT-RSP	{ scriptlval.commandField = DIMSE_CMD_NEVENTREPORT_RSP; return COMMANDFIELD; }
N_GET_RQ		|
N-GET-RQ		{ scriptlval.commandField = DIMSE_CMD_NGET_RQ; return COMMANDFIELD; }
N_GET_RSP		|
N-GET-RSP		{ scriptlval.commandField = DIMSE_CMD_NGET_RSP; return COMMANDFIELD; }
N_SET_RQ		|
N-SET-RQ		{ scriptlval.commandField = DIMSE_CMD_NSET_RQ; return COMMANDFIELD; }
N_SET_RSP		|
N-SET-RSP		{ scriptlval.commandField = DIMSE_CMD_NSET_RSP; return COMMANDFIELD; }

A_ASSOCIATE_RQ	|
ASSOCIATE-RQ	{ return T_ASSOCIATE_RQ; }
A_ASSOCIATE_AC	|
ASSOCIATE-AC	{ return T_ASSOCIATE_AC; }
A_ASSOCIATE_RJ	|
ASSOCIATE-RJ	{ return T_ASSOCIATE_RJ; }
A_RELEASE_RQ	|
RELEASE-RQ		{ return T_RELEASE_RQ; }
A_RELEASE_RP	|
RELEASE-RP		{ return T_RELEASE_RP; }
A_ABORT			|
ABORT-RQ		{ return T_ABORT_RQ; }

ALL								{ return T_ALL; }
WAREHOUSE						{ return T_WAREHOUSE; }
ASSOCIATION						{ return T_ASSOCIATION; }
RELATION						{ return T_RELATION; }
SCRIPT-EXECUTION-CONTEXT		{ return T_EXECUTION_CONTEXT; }

PROTOCOL_VERSION	|
PROTOCOL-VERSION				{ return T_PROT_VER; }
CALLED_AE_TITLE		|
CALLED-AE-TITLE					{ return T_CALLED_AE; }
CALLING_AE_TITLE	|
CALLING-AE-TITLE				{ return T_CALLING_AE; }
APPLICATION_CONTEXT_NAME	|
APPLICATION-CONTEXT-NAME		{ return T_APPL_CTX; }
PRESENTATION_CONTEXT	|
PRESENTATION-CONTEXT	|
PRESENTATION-CONTEXT-ITEMS		{ return T_PRES_CTX; }
MAXIMUM_LENGTH_RECEIVED	|
MAXIMUM-LENGTH-RECEIVED			{ return T_MAX_LEN; }
IMPLEMENTATION_CLASS_UID	|
IMPLEMENTATION-CLASS-UID		{ return T_IMPL_CLASS; }
IMPLEMENTATION_VERSION_NAME	|
IMPLEMENTATION-VERSION-NAME		{ return T_IMPL_VER; }
SOP-CLASS-EXTENDED-NEGOTIATION  { return T_SOP_EXTEND_NEG; }
SCPSCU-ROLE-SELECTION			{ return T_SCPSCU_ROLE; }
ASYNCHRONOUS-OPERATIONS-WINDOW  { return T_ASYNC_WINDOW; }
USER-IDENTITY-NEGOTIATION		{ return T_USER_ID_NEG; }
RESULT							{ return T_RESULT; }
SOURCE							{ return T_SOURCE; }
REASON_DIAGNOSTIC	|
REASON							{ return T_REASON; }

FILE_HEAD	|
FILE-HEAD						{ return T_FILEHEAD; }
FILE_TAIL	|
FILE-TAIL						{ return T_FILETAIL; }

PREAMBLE					{ return T_FILE_PREAMBLE; }
PREFIX						{ return T_DICOM_PREFIX; }
TRANSFER_SYNTAX	|
TRANSFER-SYNTAX				{ return T_TRANSFER_SYNTAX; }
SECTOR_SIZE	|
SECTOR-SIZE					{ return T_SECTOR_SIZE; }
DATASET_TRAILING_PADDING	|
DATASET-TRAILING-PADDING	{ return T_DATASET_TRAILING_PADDING; }
PADDING_VALUE	|
PADDING-VALUE				{ return T_PADDING_VALUE; }

AUTOSET			{ return T_AUTOSET; }
DEFINED_LENGTH	|
DEFINED-LENGTH	{ return T_DEFINED_LENGTH; }

NO			{ return T_NO; }
YES			{ return T_YES; }

OFF			{ return T_OFF; }
ON			{ return T_ON; }

AND			{ return T_AND; }
OR			{ return T_OR; }

PATIENT		{ scriptlval.iomLevel = IOM_PATIENT; return IOMLEVEL; }
STUDY		{ scriptlval.iomLevel = IOM_STUDY; return IOMLEVEL; }
SERIES		{ scriptlval.iomLevel = IOM_SERIES; return IOMLEVEL; }
IMAGE		{ scriptlval.iomLevel = IOM_IMAGE; return IOMLEVEL; }

SCP	|
ACCEPTOR	{ scriptlval.userProvider = UP_ACCEPTOR; return USERPROVIDER; }
SCU	|
REQUESTOR	{ scriptlval.userProvider = UP_REQUESTOR; return USERPROVIDER; }

0(x|X)[0-9a-f]+	|
0(x|X)[0-9A-F]+	{ scriptlval.hex = strtoul(scripttext, &pEnd, 16); return HEXADECIMAL; }

(\+|\-)[0-9]+	|
[0-9]+			{ scriptlval.integer = atoi(scripttext); return INTEGER; }

[A-Z_][0-9A-Z_]+	{ strncpy(scriptlval.identifier, scripttext, sizeof(scriptlval.identifier)); return IDENTIFIER; }

\"[^\"\\\n]*\\\n	{ for (scriptleng = scriptleng-2; scriptleng >= 0; scriptleng--)
				    /* concatenate line */
				    unput(scripttext[scriptleng]);
					}

\"[^\"\n]*\"	{ int i = 1, j = 0;

				scriptlval.string_ptr = (char*) malloc(scriptleng+1);
				if (scriptlval.string_ptr == NULL)
				{
					char message[128];
					sprintf(message, "STRING of %d chars too long for parser - string truncated!",
						scriptleng-2);
					scripterror(message);
					
					scriptlval.string_ptr = (char*) malloc(MAX_STRING_LEN);

					scriptleng = MAX_STRING_LEN - 1;
				}

				while (i < (scriptleng-1)) 
				{
					scriptlval.string_ptr[j++] = scripttext[i++];
				}

				scriptlval.string_ptr[j] = NULLCHAR;
				return STRING;
				}

.			{ return scripttext[0]; }
%%

//>>===========================================================================

void scripterror(char *errmsg)

//  DESCRIPTION     : YACC/LEX error function called when parsing of the 
//                    DICOMScript fails.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
    LOG_CLASS* logger_ptr = scriptSession_ptr->getLogger();

	logger_ptr->text(LOG_ERROR, 1,
	                 "%s - parsing Test Script; line %ld of %s", VAL_PREFIX_FAILED,
					  scriptCurrentFileOffset + (scriptlineno - scriptCurrentLineNo), 
					  scriptCurrentFilename);
	logger_ptr->text( LOG_ERROR, 1, "%s - around token : \"%s\"", errmsg, scripttext);

}


