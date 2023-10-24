%{
// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2006
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

//*****************************************************************************
//  DESCRIPTION     :	Session File Lexer.
//*****************************************************************************

#ifdef _WINDOWS
#pragma warning (disable : 4102)
#pragma warning (disable : 4273)
#pragma warning (disable : 4786)
#endif

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
// Added this for DVT
// # define YYLMAX 1024
// - don't forget it
#include <io.h>
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface
#include "session.h"		// Test Session
#include "session_parser.cpp.h"

extern void scripterror(char *);

extern void		sessionerror(char*);
//extern string	sessionfile;
extern FILE		*sessionin;
extern int		scriptlineno;

extern BASE_SESSION_CLASS	*session_ptr;

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
				int length = yyleng;
			 
				// check to see how line is terminated
				// - and remove termination
				if ((sessiontext[yyleng-1] == LINEFEED)
				 && (sessiontext[yyleng-2] == CARRIAGERETURN))
				    length -= 2;
				else
					length--;

				memcpy(sessionlval.string, &sessiontext[0], length);
				sessionlval.string[length] = '\0';

				LOG_CLASS *logger_ptr = session_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_SCRIPT, 1, "%s", sessionlval.string);
				}
				}
^[ \t]*#.*\n	{ /* skip other comments */ }
^[ \t]*\n		{ /* skip empty lines */ }
#.*				{ /* skip comment */ }
[ \t\n\r]+		{ /* skip white space */ }
\\\n			{ /* continuation line */ }

SESSION				{ return T_SESSION; }
ENDSESSION			{ return T_ENDSESSION; }
SESSION-FILE-VERSION	{ return T_SESSION_FILE_VERSION; }
SESSION-TYPE		{ return T_SESSION_TYPE; }
SESSION-TITLE		{ return T_SESSION_TITLE; }
SESSION-ID			{ return T_SESSION_ID; }
MANUFACTURER		{ return T_MANUFACT; }
MODEL-NAME			{ return T_MODEL; }
APPLICATION-ENTITY-NAME	{ return T_APPL_ENTITY_NAME; }
APPLICATION-ENTITY-VERSION { return T_APPL_ENTITY_VERSION; }
SOFTWARE-VERSIONS	{ return T_SOFTWARE; }
TESTED-BY			{ return T_TESTED; }
DATE				{ return T_DATE; }
PRODUCT-ROLE-IS-SCP	{ return T_PRODUCT_ROLE_ACCEPTOR; }
PRODUCT-ROLE-IS-ACCEPTOR	{ return T_PRODUCT_ROLE_ACCEPTOR; }
PRODUCT-ROLE-IS-SCU	{ return T_PRODUCT_ROLE_REQUESTOR; }
PRODUCT-ROLE-IS-REQUESTOR	{ return T_PRODUCT_ROLE_REQUESTOR; }
CALLED-AE-TITLE		{ return T_CALLED_AE ; }
CALLING-AE-TITLE	{ return T_CALLING_AE; }
MAXIMUM-LENGTH-RECEIVED		{ return T_MAX_LEN; }
IMPLEMENTATION-CLASS-UID	{ return T_IMPL_CLASS; }
IMPLEMENTATION-VERSION-NAME	{ return T_IMPL_VER; }

SUT-AE-TITLE					{ return T_SUT_AE ; }
SUT-MAXIMUM-LENGTH-RECEIVED		{ return T_SUT_MAX_LEN; }
SUT-IMPLEMENTATION-CLASS-UID	{ return T_SUT_IMPL_CLASS; }
SUT-IMPLEMENTATION-VERSION-NAME	{ return T_SUT_IMPL_VER; }
SUT-HOSTNAME					{ return T_SUT_HOSTNAME; }
SUT-PORT						{ return T_SUT_PORT; }
SUT-ROLE						{ return T_SUT_ROLE; }

DVT-AE-TITLE					{ return T_DVT_AE ; }
DVT-MAXIMUM-LENGTH-RECEIVED		{ return T_DVT_MAX_LEN; }
DVT-IMPLEMENTATION-CLASS-UID	{ return T_DVT_IMPL_CLASS; }
DVT-IMPLEMENTATION-VERSION-NAME	{ return T_DVT_IMPL_VER; }
DVT-PORT						{ return T_DVT_PORT; }
DVT-SOCKET-TIMEOUT				{ return T_DVT_SOCKET_TIMEOUT; }

SCP-HOST-NAME		{ return T_REMOTE_HOST_NAME; }
REMOTE-HOST-NAME	{ return T_REMOTE_HOST_NAME; }
TCP-IP-PORT			{ return T_PORT; }
REMOTE-CONNECT-PORT	{ return T_REMOTE_PORT; }
LOCAL-LISTEN-PORT	{ return T_LOCAL_PORT; }
SOCKET-TIMEOUT			{ return T_SOCKET_TIMEOUT; }
USE-SECURE-SOCKETS		{ return T_USE_SECURE_SOCKETS; }
TLS-VERSION				{ return T_TLS_VERSION; }
MAX-TLS-VERSION				{ return T_MAX_TLS_VERSION; }
MIN-TLS-VERSION				{ return T_MIN_TLS_VERSION; }
CHECK-REMOTE-CERTIFICATE	{ return T_CHECK_REMOTE_CERTIFICATE; }
CIPHER-LIST				{ return T_CIPHER_LIST; }
CACHE-TLS-SESSIONS		{ return T_CACHE_TLS_SESSIONS; }
TLS-CACHE-TIMEOUT		{ return T_TLS_CACHE_TIMEOUT; }
DELAY					{ return T_DELAY; }
CREDENTIALS-FILENAME	{ return T_CREDENTIALS_FILENAME; }
CERTIFICATE-FILENAME	{ return T_CERTIFICATE_FILENAME; }

VALIDATION-RESULTS	{ return T_VAL_RES; }
DIMSE-MESSAGES		{ return T_DIMSE_MSG; }
ACSE-MESSAGES		{ return T_ACSE_MSG; }
PDU-DUMP			{ return T_PDU_DUMP; }
LABEL-DUMP			{ return T_LABEL_DUMP; }
SAVE-IMAGE-DATA		{ return T_IMAGE_SAVE; }
DEFINE-SQ-LENGTH	{ return T_DEF_SQ_LENGTH; }
ADD-GROUP-LENGTH	{ return T_ADD_GROUP_LENGTH; }
CONTINUE-ON-ERROR	{ return T_CONTINUE; }
SCP-EMULATOR-TS		{ return T_SUPPORTED_TRANSFER_SYNTAX; }
SUPPORTED-TRANSFER-SYNTAX	{ return T_SUPPORTED_TRANSFER_SYNTAX; }
FORMAT-IS-MEDIA		{ return T_FORMAT; }
STRICT-VALIDATION	{ return T_STRICT; }
DETAILED-VALIDATION-RESULTS	{ return T_DETAILED_RESULTS; }
SUMMARY-VALIDATION-RESULTS	{ return T_SUMMARY_RESULTS; }
INCLUDE-TYPE-3-NOTPRESENT-INRESULTS	{ return T_INCLUDE_TYPE3_NOTPRESENT; }
DUMP-ATTRIBUTES-REF-FILES	{ return T_DUMP_ATTR_REFFILE; }
USE-PRIVATE-MAPPING	{ return T_PRIVATE_MAPPING; }
AUTO-TYPE-2-ATTRIBUTES	{ return T_AUTO_TYPE; }
AUTO-CREATE-DIRECTORY	{ return T_AUTO_CREATE; }
ENSURE-EVEN-ATTRIBUTE-VALUE-LENGTH	{ return T_ENSURE_EVEN_ATTRIBUTE_VALUE_LENGTH; }
LOG-ERROR			{ return T_LOG_ERROR; } 
LOG-WARNING			{ return T_LOG_WARNING; } 
LOG-INFO			{ return T_LOG_INFO; }
LOG-RELATION		{ return T_LOG_RELATION; } 
LOG-DEBUG			{ return T_LOG_DEBUG; } 
LOG-DICOM-MESSAGES	{ return T_LOG_DICOM; } 
LOG-DICOM-VALIDATION-RESULTS { return T_DICOM_VAL_RES; }
LOG-ACSE-MESSAGES	{ return T_LOG_ACSE; } 
LOG-ACSE-VALIDATION-RESULTS { return T_ACSE_VAL_RES; }
LOG-DULP-STATE		{ return T_DULP_STATE; }
LOG-SCP-THREAD		{ return T_LOG_SCP_THREAD; }
STORAGE-MODE		{ return T_STORAGE_MODE; }
DATA-DIRECTORY		{ return T_DATA_DIRECTORY; }
UN-VR-DEF-LOOKUP	{ return T_UN_VR_DEF_LOOKUP; }
DEFINITION-DIRECTORY	{ return T_DEFINITION_DIRECTORY; }
DEFINITION-ROOT		{ return T_DEFINITION_ROOT; }
DEFINITION			{ return T_DEFINITION; }
DICOMSCRIPT-ROOT	{ return T_DICOMSCRIPT_ROOT; }
DICOMSCRIPT			{ return T_DICOMSCRIPT; }
RESULTS-ROOT		{ return T_RESULTS_ROOT; }
DESCRIPTION-DIRECTORY	{ return T_DESCRIPTION_DIRECTORY; }
APPEND-TO-RESULTS-FILE { return T_APPEND_TO_FILE; }
INCLUDE-PATH		{ return T_INCLUDE_PATH; }

true			{ sessionlval.trueOrFalse = true; return TRUE_OR_FALSE; }
false			{ sessionlval.trueOrFalse = false; return TRUE_OR_FALSE; }
none			{ return LOG_LEVEL; }
raw				{ return LOG_LEVEL; }
standard		{ return LOG_LEVEL; }
short			{ return LOG_LEVEL; }
long			{ return LOG_LEVEL; }
tabular			{ return LOG_LEVEL; }
script			{ sessionlval.sessionType = ST_SCRIPT; return SESSION_TYPE; }
emulator		{ sessionlval.sessionType = ST_EMULATOR; return SESSION_TYPE; }
media			{ sessionlval.sessionType = ST_MEDIA; return SESSION_TYPE; }
sniffer			{ sessionlval.sessionType = ST_SNIFFER; return SESSION_TYPE; }
no-storage		{ sessionlval.storageMode = SM_NO_STORAGE; return STORAGE_MODE; }
as-dataset		{ sessionlval.storageMode = SM_AS_DATASET; return STORAGE_MODE; }
as-media		{ sessionlval.storageMode = SM_AS_MEDIA; return STORAGE_MODE; }
as-media-only	{ sessionlval.storageMode = SM_AS_MEDIA_ONLY; return STORAGE_MODE; }
acceptorAndRequestor	{ sessionlval.sutRole = UP_ACCEPTOR_REQUESTOR; return SUT_ROLE; }
acceptor		{ sessionlval.sutRole = UP_ACCEPTOR; return SUT_ROLE; }
requestor		{ sessionlval.sutRole = UP_REQUESTOR; return SUT_ROLE; }

(\+|\-)[0-9]+	|
[0-9]+			{ sessionlval.integer = atoi(sessiontext); return INTEGER; }

\"[^\"\\\n]*\\\n	{ for (yyleng = yyleng-2; yyleng >= 0; yyleng--)
				    /* concatenate line */
				    unput(sessiontext[yyleng]);
					}

\"[^\"\n]*\"	{ int i = 1, j = 0;

				memset(sessionlval.string, 0, sizeof(sessionlval.string));
				if ((yyleng-2) >= sizeof(sessionlval.string)) 
				{
					sessionerror("STRING too long - truncated!");
					yyleng = sizeof(sessionlval.string) + 1;
				}

				while (i < (yyleng-1)) 
				{
					sessionlval.string[j++] = sessiontext[i++];
				}

				sessionlval.string[j] = NULLCHAR;
				return STRING;
				}

.			{ return sessiontext[0]; }
%%

void sessionerror(char *yymsg)
{
    printf("\n%s %s during parse of s on line no: %d (near: \"%s\")",
			  VAL_PREFIX_ERROR, 
			  yymsg,
//			  sessionfile.c_str(),
			  scriptlineno,
			  sessiontext);
}

