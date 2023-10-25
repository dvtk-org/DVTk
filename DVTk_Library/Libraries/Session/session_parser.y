%{
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

%}

%start Language

%token T_SESSION T_ENDSESSION T_SESSION_FILE_VERSION
%token T_SESSION_TYPE T_SESSION_TITLE T_SESSION_ID T_MANUFACT T_MODEL T_SOFTWARE T_TESTED T_DATE
%token T_APPL_ENTITY_NAME T_APPL_ENTITY_VERSION
%token T_PRODUCT_ROLE_ACCEPTOR T_PRODUCT_ROLE_REQUESTOR
%token T_CALLED_AE T_CALLING_AE T_MAX_LEN T_IMPL_CLASS T_IMPL_VER
%token T_REMOTE_HOST_NAME T_PORT T_REMOTE_PORT T_LOCAL_PORT
%token T_SOCKET_TIMEOUT T_USE_SECURE_SOCKETS T_TLS_VERSION T_MAX_TLS_VERSION T_MIN_TLS_VERSION T_CHECK_REMOTE_CERTIFICATE 
%token T_CIPHER_LIST T_CACHE_TLS_SESSIONS T_TLS_CACHE_TIMEOUT T_DELAY T_CREDENTIALS_FILENAME T_CERTIFICATE_FILENAME 
%token T_VAL_RES T_DIMSE_MSG T_ACSE_MSG T_PDU_DUMP T_LABEL_DUMP T_IMAGE_SAVE T_STORAGE_MODE
%token T_DEF_SQ_LENGTH T_ADD_GROUP_LENGTH T_SUPPORTED_TRANSFER_SYNTAX T_FORMAT T_STRICT T_DATA_DIRECTORY T_UN_VR_DEF_LOOKUP
%token T_AUTO_TYPE T_AUTO_CREATE T_DEFINITION_DIRECTORY T_DEFINITION_ROOT T_DEFINITION T_DICOMSCRIPT_ROOT T_DICOMSCRIPT
%token T_LOG_ERROR T_LOG_WARNING T_LOG_INFO T_LOG_RELATION T_LOG_DEBUG T_LOG_ACSE T_LOG_DICOM T_LOG_SCP_THREAD T_DULP_STATE
%token T_CONTINUE T_DICOM_VAL_RES T_ACSE_VAL_RES T_RESULTS_ROOT T_DESCRIPTION_DIRECTORY T_APPEND_TO_FILE T_INCLUDE_PATH
%token TRUE_OR_FALSE INTEGER STRING LOG_LEVEL SESSION_TYPE STORAGE_MODE SUT_ROLE T_ENSURE_EVEN_ATTRIBUTE_VALUE_LENGTH
%token T_SUT_AE T_SUT_MAX_LEN T_SUT_IMPL_CLASS T_SUT_IMPL_VER T_SUT_HOSTNAME T_SUT_PORT T_SUT_ROLE
%token T_DVT_AE T_DVT_MAX_LEN T_DVT_IMPL_CLASS T_DVT_IMPL_VER T_DVT_PORT T_DVT_SOCKET_TIMEOUT
%token T_DETAILED_RESULTS T_SUMMARY_RESULTS T_INCLUDE_TYPE3_NOTPRESENT T_DUMP_ATTR_REFFILE T_PRIVATE_MAPPING

%union {
	bool				trueOrFalse;
	int					integer;
	char				string[MAX_STRING_LEN];
	SESSION_TYPE_ENUM	sessionType;
	STORAGE_MODE_ENUM	storageMode;
	UP_ENUM				sutRole;
}

%type <trueOrFalse> TRUE_OR_FALSE
%type <integer> INTEGER
%type <string> STRING
%type <sessionType> SESSION_TYPE
%type <storageMode> STORAGE_MODE
%type <sutRole> SUT_ROLE

%%

Language	: /* empty */
	| LanguageGrammar
	;

LanguageGrammar	: LanguageComponents
	| LanguageGrammar LanguageComponents
	;

LanguageComponents	: Session
	;

Session	: BeginSession SessionContentList EndSession
	;

BeginSession	: T_SESSION
	;

EndSession	: T_ENDSESSION
	;

SessionContentList	: SessionContent
	| SessionContentList SessionContent
	;

SessionContent	: T_SESSION_FILE_VERSION INTEGER
	{
		session_ptr->sessionFileVersion($2);
	}
	| T_SESSION_TYPE SESSION_TYPE
	{
		session_ptr->setSessionType($2);
	}
	| T_SESSION_TITLE STRING
	{
		session_ptr->setSessionTitle($2);
	}
	| T_SESSION_ID INTEGER
	{
		session_ptr->setSessionId($2);
	}
	| T_MANUFACT STRING
	{
		session_ptr->setManufacturer($2);
	}
	| T_MODEL STRING
	{
		session_ptr->setModelName($2);
	}
	| T_SOFTWARE STRING
	{
		session_ptr->setSoftwareVersions($2);
	}
	| T_APPL_ENTITY_NAME STRING
	{
		session_ptr->setApplicationEntityName($2);
	}
	| T_APPL_ENTITY_VERSION STRING
	{
		session_ptr->setApplicationEntityVersion($2);
	}
	| T_TESTED STRING
	{
		session_ptr->setTestedBy($2);
	}
	| T_DATE STRING
	{
		session_ptr->setDate($2);
	}
	| T_PRODUCT_ROLE_ACCEPTOR TRUE_OR_FALSE
	{
		session_ptr->setProductRoleIsAcceptor($2);
	}
	| T_PRODUCT_ROLE_REQUESTOR TRUE_OR_FALSE
	{
		session_ptr->setProductRoleIsRequestor($2);
	}
	| T_CALLED_AE STRING
	{
		session_ptr->setCalledAeTitle($2);
	}
	| T_CALLING_AE STRING
	{
		session_ptr->setCallingAeTitle($2);
	} 
    | T_MAX_LEN INTEGER
	{
		session_ptr->setMaximumLengthReceived($2);
	}
	| T_IMPL_CLASS STRING
	{
		session_ptr->setImplementationClassUid($2);
	}
	| T_IMPL_VER STRING
	{
		session_ptr->setImplementationVersionName($2);
	}
	| T_REMOTE_HOST_NAME STRING
	{
		session_ptr->setRemoteHostname($2);
	}
	| T_PORT INTEGER
	{
		// remain compatible with ADVT2.0
		session_ptr->setRemoteConnectPort((UINT16)$2);
		session_ptr->setLocalListenPort((UINT16)$2);
	}
	| T_REMOTE_PORT INTEGER
	{
		session_ptr->setRemoteConnectPort((UINT16)$2);
	}
	| T_LOCAL_PORT INTEGER
	{
		session_ptr->setLocalListenPort((UINT16)$2);
	}
	| T_SOCKET_TIMEOUT INTEGER
	{
		session_ptr->setSocketTimeout($2);
	}
	| T_SUT_AE STRING
	{
		session_ptr->setSutAeTitle($2);
	}
	| T_SUT_MAX_LEN INTEGER
	{
		session_ptr->setSutMaximumLengthReceived($2);
	}
	| T_SUT_IMPL_CLASS STRING
	{
		session_ptr->setSutImplementationClassUid($2);
	}
	| T_SUT_IMPL_VER STRING
	{
		session_ptr->setSutImplementationVersionName($2);
	}
	| T_SUT_HOSTNAME STRING
	{
		session_ptr->setSutHostname($2);
	}
	| T_SUT_PORT INTEGER
	{
		session_ptr->setSutPort((UINT16)$2);
	}
	| T_SUT_ROLE SUT_ROLE
	{
		session_ptr->setSutRole($2);
	}
	| T_DVT_AE STRING
	{
		session_ptr->setDvtAeTitle($2);
	}
	| T_DVT_MAX_LEN INTEGER
	{
		session_ptr->setDvtMaximumLengthReceived($2);
	}
	| T_DVT_IMPL_CLASS STRING
	{
		session_ptr->setDvtImplementationClassUid($2);
	}
	| T_DVT_IMPL_VER STRING
	{
		session_ptr->setDvtImplementationVersionName($2);
	}
	| T_DVT_PORT INTEGER
	{
		session_ptr->setDvtPort((UINT16)$2);
	}
	|T_DVT_SOCKET_TIMEOUT INTEGER
	{
		session_ptr->setDvtSocketTimeout($2);
	}	
	| T_USE_SECURE_SOCKETS TRUE_OR_FALSE
	{
		session_ptr->setUseSecureSockets($2);
	}
	| T_MAX_TLS_VERSION STRING
	{
		session_ptr->setMaxTlsVersion($2);
	}
	| T_MIN_TLS_VERSION STRING
	{
		session_ptr->setMinTlsVersion($2);
	}
	| T_CHECK_REMOTE_CERTIFICATE TRUE_OR_FALSE
	{
		session_ptr->setCheckRemoteCertificate($2);
	}
	| T_CIPHER_LIST STRING
	{
		session_ptr->setCipherList($2);
	}
	| T_CACHE_TLS_SESSIONS TRUE_OR_FALSE
	{
		session_ptr->setCacheTlsSessions($2);
	}
	| T_TLS_CACHE_TIMEOUT INTEGER
	{
		session_ptr->setTlsCacheTimeout($2);
	}
	| T_DELAY INTEGER
	{
		session_ptr->setDelayForStorageCommitment($2);
	}
	| T_CREDENTIALS_FILENAME STRING
	{
		session_ptr->setCredentialsFilename($2);
	}
	| T_CERTIFICATE_FILENAME STRING
	{
		session_ptr->setCertificateFilename($2);
	}
	| T_VAL_RES TRUE_OR_FALSE
	{
		// deprecated
	}
	| T_DIMSE_MSG TRUE_OR_FALSE
	{
		// deprecated
	}
	| T_ACSE_MSG TRUE_OR_FALSE
	{
		// deprecated
	}
	| T_PDU_DUMP TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_PDU_BYTES);
	}
	| T_LABEL_DUMP TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_LABEL);
	}
	| T_IMAGE_SAVE TRUE_OR_FALSE
	{
		if ($2) 
		{
			session_ptr->setStorageMode(SM_AS_MEDIA);
		}
		else 
		{
			session_ptr->setStorageMode(SM_NO_STORAGE);
		}
	}
	| T_DEF_SQ_LENGTH TRUE_OR_FALSE
	{
		session_ptr->setDefineSqLength($2);
	}
	| T_ADD_GROUP_LENGTH TRUE_OR_FALSE
	{
		session_ptr->setAddGroupLength($2);
	}
	| T_CONTINUE TRUE_OR_FALSE
	{
		session_ptr->setContinueOnError($2);
	}
	| T_SUPPORTED_TRANSFER_SYNTAX STRING
	{
		session_ptr->addSupportedTransferSyntax($2);
	}
	| T_FORMAT TRUE_OR_FALSE
	{
		if (session_ptr->getStorageMode() != SM_NO_STORAGE) 
		{
			if ($2) 
			{
				session_ptr->setStorageMode(SM_AS_MEDIA);
			}
			else 
			{
				session_ptr->setStorageMode(SM_AS_DATASET);
			}
		}
	}
	| T_STORAGE_MODE STORAGE_MODE
	{
		session_ptr->setStorageMode($2);
	}
	| T_STRICT TRUE_OR_FALSE
	{
		session_ptr->setStrictValidation($2);
	}
	| T_DETAILED_RESULTS TRUE_OR_FALSE
	{
		session_ptr->setDetailedValidationResults($2);
	}
	| T_SUMMARY_RESULTS TRUE_OR_FALSE
	{
		session_ptr->setSummaryValidationResults($2);
	}
	| T_INCLUDE_TYPE3_NOTPRESENT TRUE_OR_FALSE
	{
		session_ptr->setIncludeType3NotPresentInResults($2);
	}
	| T_DUMP_ATTR_REFFILE TRUE_OR_FALSE
	{
		session_ptr->setDumpAttributesOfRefFiles($2);
	}
	| T_PRIVATE_MAPPING TRUE_OR_FALSE
	{
		BASE_SESSION_CLASS::setUsePrivateAttributeMapping($2);
	}
	| T_AUTO_TYPE TRUE_OR_FALSE
	{
		session_ptr->setAutoType2Attributes($2);
	}
	| T_AUTO_CREATE TRUE_OR_FALSE
	{
		session_ptr->setAutoCreateDirectory($2);
	}
	| T_ENSURE_EVEN_ATTRIBUTE_VALUE_LENGTH TRUE_OR_FALSE
	{
		session_ptr->setEnsureEvenAttributeValueLength($2);
	}
	| T_LOG_ERROR TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_ERROR);
	} 
	| T_LOG_WARNING TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_WARNING);
	}
	| T_LOG_INFO TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_INFO);
	}
	| T_LOG_RELATION TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_IMAGE_RELATION);
	}
	| T_LOG_DEBUG TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_DEBUG);
	}
	| T_LOG_DICOM LOG_LEVEL
	{
		// deprecated
	}
	| T_DICOM_VAL_RES LOG_LEVEL
	{
		// deprecated
	}
	| T_LOG_ACSE LOG_LEVEL
	{
		// deprecated
	}
	| T_LOG_SCP_THREAD TRUE_OR_FALSE
	{
		session_ptr->setLogScpThread($2);
	}
	| T_DULP_STATE TRUE_OR_FALSE
	{
		session_ptr->setLogLevel($2, LOG_DULP_FSM);
	}
	| T_ACSE_VAL_RES LOG_LEVEL
	{
		// deprecated
	}
	| T_DEFINITION_DIRECTORY STRING
	{
		session_ptr->addDefinitionDirectory($2);
	}
	| T_DATA_DIRECTORY STRING
	{
		session_ptr->setDataDirectory($2);
	}
	| T_UN_VR_DEF_LOOKUP TRUE_OR_FALSE
	{
		session_ptr->setUnVrDefinitionLookUp($2);	
	}
	| T_DEFINITION_ROOT STRING
	{
		// no longer used in DVT V2.x
		// maintained for backwards compatiblity with DVT V1.x
		session_ptr->setDefinitionFileRoot($2);
	}
	| T_DEFINITION STRING
	{
		string definitionFile((char*) $2);
		bool exists = false;
		for (int index = 0; index < session_ptr->noDefinitionFiles(); index++)
		{
		    const char* pDefFileName = session_ptr->getDefinitionFilename(index);
		    if (strcmp(pDefFileName, $2) == 0)
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
	}
	| T_DICOMSCRIPT_ROOT STRING
	{
		session_ptr->setDicomScriptRoot($2);
	}
	| T_DICOMSCRIPT STRING
	{
		// only store this information in a Scripting session
		if (session_ptr->getRuntimeSessionType() == ST_SCRIPT)
		{
			// should check that file extension is OK
			string dicomScript((char*) $2);
			DICOM_SCRIPT_CLASS *dicomScript_ptr = new DICOM_SCRIPT_CLASS(reinterpret_cast<SCRIPT_SESSION_CLASS*>(session_ptr), dicomScript);
			session_ptr->addDicomScript(dicomScript_ptr);
		}
	}
	| T_RESULTS_ROOT STRING
	{
		session_ptr->setResultsRoot($2);
	}
	| T_DESCRIPTION_DIRECTORY STRING
	{
		session_ptr->setDescriptionDirectory($2);
	}
	| T_APPEND_TO_FILE  TRUE_OR_FALSE
	{
		session_ptr->setAppendToResultsFile($2);
	}
	| T_INCLUDE_PATH STRING
	{
		// deprecated
	}
	;
%%


