%{
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

%}

%start Language

%token T_LANGUAGE T_RESET T_ALL T_WAREHOUSE T_ASSOCIATION T_RELATION T_EXECUTION_CONTEXT
%token T_COMPARE T_COMPARE_NOT T_CONFIRM T_COPY T_CREATE T_DELAY T_DELETE
%token T_DISPLAY T_ECHO T_POPULATE T_READ T_RECEIVE T_ROLE T_SEND
%token T_SET T_SYSTEM T_TIME T_VALIDATE T_VERBOSE T_WRITE
%token T_IMPORT T_EXPORT
%token T_VALIDATION T_DEF_SQ_LENGTH T_ADD_GROUP_LENGTH T_STRICT T_APPL_ENTITY
%token T_ASSOCIATE_RQ T_ASSOCIATE_AC T_ASSOCIATE_RJ T_RELEASE_RQ T_RELEASE_RP T_ABORT_RQ
%token T_PROT_VER T_CALLED_AE T_CALLING_AE T_APPL_CTX
%token T_PRES_CTX T_MAX_LEN T_IMPL_CLASS T_IMPL_VER
%token T_SOP_EXTEND_NEG T_SCPSCU_ROLE T_ASYNC_WINDOW T_USER_ID_NEG
%token T_RESULT T_SOURCE T_REASON
%token T_DEFINED_LENGTH T_PRES_CTX T_AUTOSET
%token T_FILEHEAD T_FILETAIL T_FILE_PREAMBLE T_DICOM_PREFIX T_TRANSFER_SYNTAX
%token T_DATASET_TRAILING_PADDING T_SECTOR_SIZE T_PADDING_VALUE
%token T_SQ T_OPEN_BRACKET T_CLOSE_BRACKET
%token T_YES T_NO T_ON T_OFF T_OR T_AND T_OPEN_BRACKET T_CLOSE_BRACKET
%token COMMANDFIELD HEXADECIMAL IDENTIFIER INTEGER VALIDATIONFLAG IOMLEVEL STRING USERPROVIDER VR

%union {
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
}

%type <commandField> COMMANDFIELD
%type <hex> HEXADECIMAL
%type <identifier> IDENTIFIER
%type <integer> INTEGER
%type <iomLevel> IOMLEVEL
%type <string_ptr> STRING
%type <userProvider> USERPROVIDER
%type <vr> VR
%type <validationFlag> VALIDATIONFLAG

%%
Language	: // empty
	| LanguageGrammar
	  {
	      //cleanup
		  clearVTSUidMappings();	
	  }
	;

LanguageGrammar	: LanguageComponents
	{
		if (scriptSession_ptr->isSessionStopped())
		{
			YYACCEPT;
		}
	}
	| LanguageGrammar LanguageComponents
	{
		if (scriptSession_ptr->isSessionStopped())
		{
			YYACCEPT;
		}
	}
	;

LanguageComponents	: LanguageSpecifier
	| ResetCommand
    | CompareCommand
	| ConfirmCommand
	| CopyCommand
	| CreateCommand
	| DelayCommand
	| DeleteCommand
	| DisplayCommand
	| EchoCommand
	| ExportCommand
	| ImportCommand
	| PopulateCommand
	| ReadCommand
	| ReceiveCommand
	| RoleCommand
	| SendCommand
	| SetCommand
	| SystemCommand
	| TimeCommand
	| ValidateCommand
	| VerboseCommand
	| WriteCommand
	| ApplicationEntityFlagCommand
	| ValidationFlagCommand
	| DefineSqLengthFlagCommand
	| AddGroupLengthFlagCommand
	| StrictValidationFlagCommand
	;

LanguageSpecifier: T_LANGUAGE STRING
    {
		if (strcmp($2, "NATIVE_VTS") == 0)
		{
			scriptIsNativeVts = true;
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	;

ResetCommand:	T_RESET T_ALL
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
	}
	| T_RESET T_WAREHOUSE
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
	}
	| T_RESET T_ASSOCIATION
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
	}
	| T_RESET T_RELATION
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
	}
	| T_RESET T_EXECUTION_CONTEXT
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
	}
	;

CompareCommand	: T_COMPARE ObjectRef1 TagRef1 ObjectRef2 TagRef2
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
	}
	| T_COMPARE_NOT ObjectRef1 TagRef1 ObjectRef2 TagRef2
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
	}
	;

ConfirmCommand	: T_CONFIRM
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
	}
	;

CopyCommand	: T_COPY ObjectRef1 TagRef1 ObjectRef2 TagRef2
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
	}
	;

CreateCommand	: T_CREATE CreateList
	;

DelayCommand	: T_DELAY INTEGER
	{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			// delay for given number of seconds
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "DELAY %d seconds", $2);
			}

			for (int sec = 0; sec < $2; sec++)
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
	}
	;

DeleteCommand	: T_DELETE DeleteList
	;

DisplayCommand	: DisplayTagList
	| T_DISPLAY ObjectRef1
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
	}
	| T_DISPLAY T_WAREHOUSE
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
	}
	;

EchoCommand	: T_ECHO STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			if (logger_ptr)
			{
				// display string to user
				logger_ptr->text(LOG_SCRIPT, 1, $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;

	}
	;

ExportCommand : T_EXPORT ExportList
    ;

ExportList    : ExportAcseObject
              | ExportDimseObjects
    ;

ExportAcseObject: AssociateRq
	{
        if (!scriptParseOnly)
		{
		    if (!sendAcse(scriptSession_ptr, associateRq_ptr, identifier)) YYABORT;
        }
	}
	| AssociateAc
	{
        if (!scriptParseOnly)
		{
		    if (!sendAcse(scriptSession_ptr, associateAc_ptr, identifier)) YYABORT;
		}
	}
	| AssociateRj
	{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, associateRj_ptr, identifier)) YYABORT;
		}
	}
	| ReleaseRq
	{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, releaseRq_ptr, identifier)) YYABORT;
		}
	}
	| ReleaseRp
	{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, releaseRp_ptr, identifier)) YYABORT;
		}
	}
	| AbortRq
	{
        if (!scriptParseOnly)
		{	  
			if (!sendAcse(scriptSession_ptr, abortRq_ptr, identifier)) YYABORT;
		}
	}
	;

ExportDimseObjects: DimseCmd CommandIdentifier
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
	}
	| DimseCmd CommandIdentifier IomIod DatasetIdentifier
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
	}
	| DimseCmd CommandIdentifier IomIod DatasetIdentifier T_PRES_CTX PresentationContextId
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
	}
    ;

ImportCommand : T_IMPORT ImportList
    ;

ImportList    : DimseCmd CommandIdentifier 
	{
        if (!scriptParseOnly)
		{	  
			if (!importCommand(scriptSession_ptr, commandField, identifier)) 
			{
				YYABORT;
			}
		}
	}
	| DimseCmd CommandIdentifier IomIod DatasetIdentifier
	{
        if (!scriptParseOnly)
		{	  
			if (!importCommandDataset(scriptSession_ptr, commandField, identifier, iodName, datasetidentifier))
			{
				YYABORT;
			}
		}
	}
    ;

PopulateCommand	: T_POPULATE T_ON
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
	}
	| T_POPULATE T_OFF
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
	}
	;

ReadCommand	: T_READ ReadList
	;

ReceiveCommand	: T_RECEIVE ReceiveList
	;

RoleCommand	: T_ROLE USERPROVIDER
	{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

			// set product role based on new setting for tool
			if ($2 == UP_REQUESTOR)
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
				if ($2 == UP_REQUESTOR)
				{
					logger_ptr->text(LOG_SCRIPT, 2, "DVT ROLE REQUESTOR - PRODUCT ROLE ACCEPTOR");
				}
				else
				{
					logger_ptr->text(LOG_SCRIPT, 2, "DVT ROLE ACCEPTOR - PRODUCT ROLE REQUESTOR");
				}
			}
		}
	}
	;

SendCommand	: T_SEND SendList
	;

SetCommand	: T_SET SetList
	;

SystemCommand	: T_SYSTEM STRING
	{
        if (!scriptParseOnly)
		{	  
			// make system call within session
			if (!systemCall(scriptSession_ptr, $2))
			{
				YYABORT;
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;		
	}
	;

TimeCommand	: T_TIME
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
	}
	;

ValidateCommand	: T_VALIDATE ValidateList
	;

VerboseCommand	: T_VERBOSE T_ON
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
	}
	| T_VERBOSE T_OFF
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
	}
	;

WriteCommand	: T_WRITE WriteList
	;

ApplicationEntityFlagCommand	: T_APPL_ENTITY STRING STRING
	{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				logger_ptr->text(LOG_SCRIPT, 2, "APPLICATION-ENTITY %s %s", $2, $3);
			}

			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setApplicationEntityName($2);
				scriptExecutionContext_ptr->setApplicationEntityVersion($3);
			}
		}
		// free malloced string buffers
		free($2);
		$2 = NULL;
		free($3);
		$3 = NULL;

	}
	;
	
ValidationFlagCommand	: T_VALIDATION VALIDATIONFLAG
	{
        if (!scriptParseOnly)
		{	  
			LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
			if (logger_ptr)
			{
				switch($2)
				{
				case ALL: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED"); break;
				case USE_DEFINITION: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-DEF-ONLY"); break;
				case USE_VR: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-VR-ONLY"); break;
				case USE_REFERENCE: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-REF-ONLY"); break;
				case NONE: logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION DISABLED"); break;
				default: 
					if ($2 == (USE_DEFINITION | USE_VR)) logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-DEF-AND-VR");
					else if ($2 == (USE_DEFINITION | USE_REFERENCE)) logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-DEF-AND-REF");
					else if ($2 == (USE_VR | USE_REFERENCE)) logger_ptr->text(LOG_SCRIPT, 2, "VALIDATION ENABLED-USE-VR-AND-REF");
				break;
				}
			}
			
			SCRIPT_EXECUTION_CONTEXT_CLASS *scriptExecutionContext_ptr = scriptSession_ptr->getScriptExecutionContext();
			if (scriptExecutionContext_ptr)
			{
				scriptExecutionContext_ptr->setValidationFlag($2);
			}
		}
	}
	;
	
DefineSqLengthFlagCommand	: T_DEF_SQ_LENGTH T_ON
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
	}
	| T_DEF_SQ_LENGTH T_OFF
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
	}
	;
	
AddGroupLengthFlagCommand	: T_ADD_GROUP_LENGTH T_ON
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
	}
	| T_ADD_GROUP_LENGTH T_OFF
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
	}
	;
	
StrictValidationFlagCommand	: T_STRICT T_ON
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
	}
	| T_STRICT T_OFF
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
	}
	;

CreateList	: Acse
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
	}
	| Sop
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
	}
	| Dataset /* must be an Item */
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
	}
	| Dataset T_DEFINED_LENGTH /* must be an Item */
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
	}
	| SequenceRef Dataset /* must be an Item */
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
	}
	| Filehead
	{
        if (!scriptParseOnly)
		{	  
			if (fileHead_ptr)
			{
				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileHead_ptr)) YYABORT;
			}
		}
	}
	| Filetail
	{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				if (!storeObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileTail_ptr)) YYABORT;
			}
		}
	}
	;

DeleteList	: Acse
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
	}
	| Command
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
	}
	| Dataset /* could be an Item */
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
	}
	| T_FILEHEAD
	{
        if (!scriptParseOnly)
		{	  
			identifier = "";

			if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), WID_FILEHEAD)) YYABORT;
		}
	}
	| T_FILETAIL
	{
        if (!scriptParseOnly)
		{	  
			identifier = "";

			if (!removeObjectFromWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), WID_FILETAIL)) YYABORT;
		}
	}
	;

DisplayTagList	: T_DISPLAY ObjectRef1 TagRef
	{
        if (!scriptParseOnly)
		{	  
			if (wid1_ptr)
			{
				// display the referenced warehouse object
				displayAttribute(scriptSession_ptr->getLogger(), scriptSession_ptr->getSerializer(), wid1_ptr, group, element);
			}
		}
	}	
	| DisplayTagList TagRef
	{
        if (!scriptParseOnly)
		{	  
			if (wid1_ptr)
			{
				// display the referenced warehouse object
				displayAttribute(scriptSession_ptr->getLogger(), scriptSession_ptr->getSerializer(), wid1_ptr, group, element);
			}
		}
	}
	;

ReadList	: STRING Dataset
	{
        if (!scriptParseOnly)
		{	  
			if (!readFileDataset(scriptSession_ptr, $1, dataset_ptr)) YYABORT;
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
		
	}
	| STRING HEXADECIMAL
	{
		if (!scriptParseOnly)
		{
			if (!readFileDataset(scriptSession_ptr, $1, $2)) YYABORT;
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	;

ReceiveList	: AcseContents /* always validate on receive */
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
	}
	| SopList
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
	}
	;

SendList : AcseContents
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
	}
	| SopContents
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
	}
	| SopContents T_PRES_CTX PresentationContextId
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
	}
	;

PresentationContextId	: T_OPEN_BRACKET HEXADECIMAL T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			presContextId = (BYTE) $2;
		}
	}
	| T_OPEN_BRACKET INTEGER T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			presContextId = (BYTE) $2;
		}
	}
	;

SetList	: AcseContents
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
	}
	| CommandContents
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
	}
	| DatasetContents  /* could be an Item */
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
	}
	| FileheadContents
	{
        if (!scriptParseOnly)
		{	  
			if (fileHead_ptr)
			{
				if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileHead_ptr)) YYABORT;
			}
		}
	}
	| FiletailContents
	{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				if (!updateObjectInWarehouse(scriptSession_ptr->getLogger(), identifier.c_str(), fileTail_ptr)) YYABORT;
			}
		}
	}
	;

ValidateList	: SourceSopRef ReferenceSopList 
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
	}
	;

SourceSopRef : DimseCmd CommandIdentifier
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
	}
	| DimseCmd CommandIdentifier IomIod DatasetIdentifier
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
	}
	;

ReferenceSopList	: //empty
    | ReferenceSopRef
	{
        if (!scriptParseOnly)
		{	  
			addReferenceObjects(scriptSession_ptr,ref_command_ptr, ref_dataset_ptr, scriptSession_ptr->getLogger());
		}
	}
	| ReferenceSopList T_OR ReferenceSopRef
	{
        if (!scriptParseOnly)
		{	  
			setLogicalOperand(OPERAND_OR);
			addReferenceObjects(scriptSession_ptr,ref_command_ptr, ref_dataset_ptr, scriptSession_ptr->getLogger());
		}
	}
	;

ReferenceSopRef	: DimseCmd CommandIdentifier
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
	}
	| DimseCmd CommandIdentifier IomIod DatasetIdentifier
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
	}

	;

SopList	: 
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
	}
	| SopContents
	{
        if (!scriptParseOnly)
		{	  
			addReferenceObjects(scriptSession_ptr, command_ptr, dataset_ptr, scriptSession_ptr->getLogger());
		}
	}
	| SopList T_OR SopContents
	{
        if (!scriptParseOnly)
		{	  
			setLogicalOperand(OPERAND_OR);
			addReferenceObjects(scriptSession_ptr, command_ptr, dataset_ptr, scriptSession_ptr->getLogger());
		}
	}
	| SopList T_AND SopContents
	{
        if (!scriptParseOnly)
		{	  
			setLogicalOperand(OPERAND_AND);
			addReferenceObjects(scriptSession_ptr, command_ptr, dataset_ptr, scriptSession_ptr->getLogger());
		}
	}
	;

WriteList	: STRING T_FILEHEAD
	{
        if (!scriptParseOnly)
		{	  
			if (!writeFileHead(scriptSession_ptr->getLogger(), $1, scriptSession_ptr->getAutoCreateDirectory())) YYABORT;
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;	
	}
	| STRING T_FILETAIL
	{
        if (!scriptParseOnly)
		{	  	
			if (!writeFileTail(scriptSession_ptr->getLogger(), $1, scriptSession_ptr->getAutoCreateDirectory())) YYABORT;
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	| STRING Dataset
	{
        if (!scriptParseOnly)
		{	  
			if (!writeFileDataset(scriptSession_ptr->getLogger(), $1, dataset_ptr,scriptSession_ptr->getAutoCreateDirectory())) YYABORT;
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	;

Acse	: AssociateRq
	| AssociateAc
	| AssociateRj
	| ReleaseRq
	| ReleaseRp
	| AbortRq
	;

AcseContents	: AssociateRqContents
	| AssociateAcContents
	| AssociateRjContents
	| ReleaseRqContents
	| ReleaseRpContents
	| AbortRqContents
	;

AssociateRqContents	: AssociateRq
	| AssociateRq T_OPEN_BRACKET AssociateRqParameterList T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			// store the user information
			associateRq_ptr->setUserInformation(userInformation);
			userInformation.cleanup();
		}
	}
	;

AssociateRqParameterList	: AssociateRqParameter
	| AssociateRqParameterList AssociateRqParameter
	;

AssociateRqParameter	: T_PROT_VER INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set associate request protocol version parameter
			associateRq_ptr->setProtocolVersion((UINT16) $2);
		}
	}
	| T_CALLED_AE STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			if (strlen($2) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Called AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Request", $2, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate request called Ae title parameter
				associateRq_ptr->setCalledAeTitle((char*) $2);
			}			
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;	
	}
	| T_CALLING_AE STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			if (strlen($2) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Calling AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Request", $2, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate request calling Ae title parameter
				associateRq_ptr->setCallingAeTitle((char*) $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_APPL_CTX STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			// set associate request application context parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				associateRq_ptr->setApplicationContextName((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				associateRq_ptr->setApplicationContextName((char*) $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_PRES_CTX AssociateRqPresCtxList
	| T_MAX_LEN INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set user information maximum length received parameter
			userInformation.setMaximumLengthReceived((UINT32) $2);
		}
	}
	| T_IMPL_CLASS STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			// set user information implementation class uid parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				userInformation.setImplementationClassUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				userInformation.setImplementationClassUid((char*) $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_IMPL_VER STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			// set user information implementation version name parameter
			userInformation.setImplementationVersionName((char*) $2);
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_SOP_EXTEND_NEG SopClassExtendedList
	| T_SCPSCU_ROLE ScpScuRoleList
	| T_ASYNC_WINDOW INTEGER INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set user information asynchronous operations window parameter
			userInformation.setAsynchronousOperationWindow((UINT16) $2, (UINT16) $3);
		}
	}
	| T_USER_ID_NEG UserIdentityNegotiation
	;

AssociateRqPresCtxList	: AssociateRqPresCtx
	| AssociateRqPresCtxList AssociateRqPresCtx
	| AssociateRqPresCtxList ',' AssociateRqPresCtx
	;

AssociateRqPresCtx	: RqPresCtx
	| VtsRqPresCtx
	;

RqPresCtx	: T_OPEN_BRACKET STRING ',' RqTransferSyntaxList T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) $2);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", $2);
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
	}
	;

VtsRqPresCtx	: T_OPEN_BRACKET INTEGER ',' STRING ',' INTEGER ',' INTEGER ',' RqTransferSyntaxList T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			convertHex($4);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $4);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) $4);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", $4);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// store the presentation context id
			presRqContext.setPresentationContextId((BYTE) $2);

			// store the abstract syntax name
			presRqContext.setAbstractSyntaxName(abstractSyntaxName);

			// add the presentation context to the associate request
			associateRq_ptr->addPresentationContext(presRqContext);

			// free up the local presentation context
			presRqContext.cleanup();
			presRqContext.setPresentationContextId(0);

			int scuRole = $6;
			int scpRole = $8;

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
		free($4);
		$4 = NULL;
	}
	;

RqTransferSyntaxList	: TransferSyntax
	{
        if (!scriptParseOnly)
		{	  
			// save the latest transfer syntax
			presRqContext.addTransferSyntaxName(transferSyntaxName);
		}
	}
	| RqTransferSyntaxList ',' TransferSyntax
	{
        if (!scriptParseOnly)
		{	  
			// save the latest transfer syntax
			presRqContext.addTransferSyntaxName(transferSyntaxName);
		}
	}
	;

TransferSyntax	: STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($1);
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $1);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				transferSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				transferSyntaxName.setUid((char*) $1);

				// check that we now have a valid transfer syntax
				if (!transferSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", $1);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	;

SopClassExtendedList    : SopClassExtended
	| SopClassExtendedList SopClassExtended
	;

SopClassExtended	: T_OPEN_BRACKET STRING ',' ApplicationInfoList T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				sopClassExtended.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				sopClassExtended.setUid((char*) $2);

				// check that we now have a valid transfer syntax
				if (!sopClassExtended.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", $2);
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
		free($2);
		$2 = NULL;
	}
	;

ApplicationInfoList	: ApplicationInfoByte
	| ApplicationInfoList ',' ApplicationInfoByte
	;

ApplicationInfoByte	: INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// handle application info bytes
			sopClassExtended.addApplicationInformation((BYTE) $1);
		}
	}
	;

ScpScuRoleList	: ScpScuRole
	| ScpScuRoleList ScpScuRole
	;

ScpScuRole	: T_OPEN_BRACKET STRING ',' INTEGER ',' INTEGER T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			UID_CLASS	sopClassUid;

			convertHex($2);
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				sopClassUid.set((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				sopClassUid.set((char*) $2);

				// check that we now have a valid abstract syntax
				if (!sopClassUid.isValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", $2);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// handle scp scu role
			scpScuRoleSelect.setUid(sopClassUid);
			scpScuRoleSelect.setScpRole((BYTE) $4);
			scpScuRoleSelect.setScuRole((BYTE) $6);

			// store scp scu role select
			userInformation.addScpScuRoleSelect(scpScuRoleSelect);
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	;

UserIdentityNegotiation : INTEGER INTEGER STRING
	{
		if (!scriptParseOnly)
		{	  
			// store the user identity information
			userInformation.setUserIdentityNegotiation((BYTE) $1, (BYTE) $2, $3);
		}
			
		// free malloced string buffer
		free($3);
	}
	| INTEGER INTEGER STRING STRING
	{
        if (!scriptParseOnly)
		{
			// store the user identity information
			userInformation.setUserIdentityNegotiation((BYTE) $1, (BYTE) $2, $3, $4);
		}
				
		// free malloced string buffer
		free($3);
		free($4);
	}
	| STRING
	{
        if (!scriptParseOnly)
		{	
			// store the user identity information
			userInformation.setUserIdentityNegotiation($1);
		}
		
		// free malloced string buffer
		free($1);
	}
	;
	
AssociateAcContents	: AssociateAc
	| AssociateAc T_OPEN_BRACKET AssociateAcParameterList T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			// store the user information
			associateAc_ptr->setUserInformation(userInformation);
			userInformation.cleanup();
		}
	}
	;

AssociateAcParameterList	: AssociateAcParameter
	| AssociateAcParameterList AssociateAcParameter
	;

AssociateAcParameter	: T_PROT_VER INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set associate accept protocol version parameter
			associateAc_ptr->setProtocolVersion((UINT16) $2);
		}
	}
	| T_CALLED_AE STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			if (strlen($2) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Called AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Accept", $2, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate accept called Ae title parameter
				associateAc_ptr->setCalledAeTitle((char*) $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_CALLING_AE STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			if (strlen($2) > AE_LENGTH)
			{
				LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();
				if (logger_ptr)
				{
					logger_ptr->text(LOG_ERROR, 1, "Calling AE Title \"%s\" - length exceeds maximum of %d allowed in Associate Accept", $2, AE_LENGTH);
				}
				YYABORT;
			}
			else
			{
				// set associate accept calling Ae title parameter
				associateAc_ptr->setCallingAeTitle((char*) $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_APPL_CTX STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			// set associate accept application context parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				associateAc_ptr->setApplicationContextName((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				associateAc_ptr->setApplicationContextName((char*) $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_PRES_CTX AssociateAcPresCtxList
	| T_MAX_LEN INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set user information maximum length received parameter
			userInformation.setMaximumLengthReceived((UINT32) $2);
		}
	}
	| T_IMPL_CLASS STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			// set user information implementation class uid parameter
			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				userInformation.setImplementationClassUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				userInformation.setImplementationClassUid((char*) $2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_IMPL_VER STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			// set user information implementation version name parameter
			userInformation.setImplementationVersionName((char*) $2);
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	| T_SOP_EXTEND_NEG SopClassExtendedList
	| T_SCPSCU_ROLE ScpScuRoleList
	| T_ASYNC_WINDOW INTEGER INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set user information asynchronous operations window parameter
			userInformation.setAsynchronousOperationWindow((UINT16) $2, (UINT16) $3);
		}
	}
	;

AssociateAcPresCtxList	: AssociateAcPresCtx
	| AssociateAcPresCtxList AssociateAcPresCtx
	| AssociateAcPresCtxList ',' AssociateAcPresCtx
	;

AssociateAcPresCtx	: AcPresCtx
	| VtsAcPresCtx
	;

AcPresCtx	: T_OPEN_BRACKET STRING ',' INTEGER AcTransferSyntax T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $2);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) $2);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", $2);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// store the abstract syntax name
			presAcContext.setAbstractSyntaxName(abstractSyntaxName);

			// store the result
			presAcContext.setResultReason((BYTE) $4);

			// store the transfer syntax name
			presAcContext.setTransferSyntaxName(transferSyntaxName);

			// add the presentation context to the associate accept
			associateAc_ptr->addPresentationContext(presAcContext);

			// free up the local presentation context
			presAcContext.setPresentationContextId(0);
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	;

AcTransferSyntax	: // nothing - not accepted
	{
        if (!scriptParseOnly)
		{	  
			// no transfer syntax name provided
			transferSyntaxName.setUid("");
		}
	}
	| ',' TransferSyntax
	;

VtsAcPresCtx	: T_OPEN_BRACKET INTEGER ',' INTEGER ',' STRING MoreAcPresentationContext T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			convertHex($6);
			ABSTRACT_SYNTAX_NAME_CLASS	abstractSyntaxName;

			// check whether name to uid mapping needed
			string uid = scriptSession_ptr->getSopUid((char*) $6);

			if (uid.length() > 0) 
			{
				// stored mapped uid
				abstractSyntaxName.setUid((char*) uid.c_str());
			}
			else 
			{
				// store given uid
				abstractSyntaxName.setUid((char*) $6);

				// check that we now have a valid abstract syntax
				if (!abstractSyntaxName.isUidValid())
				{
					LOG_CLASS *logger_ptr = scriptSession_ptr->getLogger();

					if (logger_ptr)
					{
						logger_ptr->text(LOG_ERROR, 1, "Cannot map '%s' to a UID", $6);
						logger_ptr->text(LOG_NONE, 1, "Check that the correct Definition File (.def) is loaded");
					}
					YYABORT;
				}
			}

			// store the abstract syntax name
			presAcContext.setAbstractSyntaxName(abstractSyntaxName);

			// store the presentation context id
			presAcContext.setPresentationContextId((BYTE) $2);

			// store the result
			presAcContext.setResultReason((BYTE) $4);

			// store the transfer syntax name
			presAcContext.setTransferSyntaxName(transferSyntaxName);

			// add the presentation context to the associate accept
			associateAc_ptr->addPresentationContext(presAcContext);

			// free up the local presentation context
			presAcContext.setPresentationContextId(0);

			// handle scp scu role
			if (((BYTE) $4 == ACCEPTANCE) &&
				(assocAcScuScpRolesDefined))

			{
				// save abstract syntax name
				scpScuRoleSelect.setUid(abstractSyntaxName.getUid());

				// store scp scu role select
				userInformation.addScpScuRoleSelect(scpScuRoleSelect);
			}
		}
		// free malloced string buffer
		free($6);
		$6 = NULL;
	}
	;

MoreAcPresentationContext :
	{
        if (!scriptParseOnly)
		{	  
			// no transfer syntax name provided
			transferSyntaxName.setUid("");
		}
	}
	| ',' INTEGER ',' INTEGER ',' TransferSyntax
	{
        if (!scriptParseOnly)
		{	  
			assocAcScuScpRolesDefined = false;
			int scuRole = $2;
			int scpRole = $4;

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
	}
	;

AssociateRjContents	: AssociateRj
	| AssociateRj T_OPEN_BRACKET AssociateRjParameterList T_CLOSE_BRACKET
	;

AssociateRjParameterList	: AssociateRjParameter
	| AssociateRjParameterList AssociateRjParameter
	;

AssociateRjParameter	: T_RESULT INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set associate reject result parameter
			associateRj_ptr->setResult((BYTE) $2);
		}
	}
	| T_SOURCE INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set associate reject source parameter
			associateRj_ptr->setSource((BYTE) $2);
		}
	}
	| T_REASON INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set associate reject reason parameter
			associateRj_ptr->setReason((BYTE) $2);
		}
	}
	;

ReleaseRqContents	: ReleaseRq
	;

ReleaseRpContents	: ReleaseRp
	;

AbortRqContents	: AbortRq
	| AbortRq T_OPEN_BRACKET AbortRqParameterList T_CLOSE_BRACKET
	;

AbortRqParameterList	: AbortRqParameter
	| AbortRqParameterList AbortRqParameter
	;

AbortRqParameter	: T_SOURCE INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set abort request source parameter
			abortRq_ptr->setSource((BYTE) $2);
		}
	}
	| T_REASON INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// set abort request reason parameter
			abortRq_ptr->setReason((BYTE) $2);
		}
	}
	;

AssociateRq	: T_ASSOCIATE_RQ
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
	}
	;

AssociateAc	: T_ASSOCIATE_AC
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
	}
	;

AssociateRj	: T_ASSOCIATE_RJ
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
	}
	;

ReleaseRq	: T_RELEASE_RQ
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
	}
	;

ReleaseRp	: T_RELEASE_RP
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
	}
	;

AbortRq	: T_ABORT_RQ
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
	}
	;

Sop	: Command
	| Command Dataset
	;

SopContents	: CommandContents
	| Command DatasetContents
	;

CommandContents	: Command
	| CommandRef T_OPEN_BRACKET T_CLOSE_BRACKET
	| CommandRef T_OPEN_BRACKET AttributeList T_CLOSE_BRACKET
	;

Command	: DimseCmd
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
	}
	| DimseCmd CommandIdentifier
	{
        if (!scriptParseOnly)
		{	  
			// instantiate a new command
			command_ptr = new DCM_COMMAND_CLASS(commandField, identifier);

			// cascade the logger
			command_ptr->setLogger(scriptSession_ptr->getLogger());
			dataset_ptr = NULL;
		}
	}
	;

DimseCmd	: COMMANDFIELD
	{
        if (!scriptParseOnly)
		{	  
			// save command field locally
			commandField = yylval.commandField;
		}
	}
	;

CommandIdentifier	: IDENTIFIER
	{
        if (!scriptParseOnly)
		{	  
			// save identifier locally
			identifier = yylval.identifier;
		}
	}
	;

CommandRef: Command
	;

DatasetContents	: Dataset
	| DatasetRef T_OPEN_BRACKET T_CLOSE_BRACKET
	| DatasetRef T_OPEN_BRACKET AttributeList T_CLOSE_BRACKET
	;

DatasetRef	: Dataset
	;

Dataset	: IomIod 
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
	}
	| IomIod DatasetIdentifier
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
	}
	;

IomIod : IodName
	{

	}
	| IomLevel IodName
	{
	}
	;

IomLevel	: IOMLEVEL
	{
		// this rule introduces a shift/reduce warning in the parser
		// - can't get rid of this due to sytax needed for backwards compatibility
	}
	;

IodName	: STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($1);
			// save iod name locally
			// - check first for iod name mapping
			iodName = scriptSession_ptr->getIodName((char*) $1);
			if (!iodName.length()) 
			{
				// no name mapping - take iod name directly
				iodName = (char*) $1;
			}
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	;

DatasetIdentifier	: IDENTIFIER
	{
        if (!scriptParseOnly)
		{	  
			// save identifier locally
			datasetidentifier = $1;
		}
	}
	| STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($1);
			// save the identifier locally
			datasetidentifier = $1;
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	;

AttributeList	: Attribute
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
	}
	| AttributeList Attribute
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
	}
	;

Attribute	: T_OPEN_BRACKET AttributeIdentification ',' AttributeValue T_CLOSE_BRACKET
	|  T_OPEN_BRACKET TagRef T_CLOSE_BRACKET
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
	}
	;

AttributeIdentification	: AttributeTag
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
	}
	;

AttributeTag	: HEXADECIMAL
	{
        if (!scriptParseOnly)
		{	  
			// store tag locally
			group = (UINT16) (yylval.hex >> 16);
			element = (UINT16) (yylval.hex & 0x0000FFFF);
		}
	}
	;

AttributeValue	: SequenceValue
	| OtherValue
	;

SequenceValue	: SequenceVR ',' ItemList
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
	}
	;

SequenceVR	: T_SQ
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
	}
	| '[' T_SQ ']'
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
	}
	;

ItemList	: ItemByReferenceList
	| ItemByValueList
	;

ItemByReferenceList	: ItemByReference
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
	}
	| ItemByReferenceList ',' ItemByReference
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
	}
	;

ItemByReference	: STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($1);
			// store item - identifier
			item_ptr[nd]->setIdentifier($1);
			item_ptr[nd]->setValueByReference(true);
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	;

ItemByValueList	: ItemByValue
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
	}
	| ItemByValueList ',' ItemByValue
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
	}
	;

ItemByValue	: // nothing - no attributes defined for item
	| ItemAttributeList
	;

ItemAttributeList	: ItemAttribute
	| ItemAttributeList ItemAttribute
	;

ItemAttribute	: '>' Attribute
	{
        if (!scriptParseOnly)
		{	  
			// store item - attribute value
			item_ptr[nd]->AddAttribute(attribute_ptr[nd]);
		}
	}
	;

OtherValue	: OptionalVR Values
        ;

OptionalVR	:
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
	}
	| AttributeVR ','
	{
        if (!scriptParseOnly)
		{	  
			// store attribute vr
			attribute_ptr[nd]->SetVR(vr);
			attribute_ptr[nd]->setTransferVR(transferVr);
			attribute_ptr[nd]->setDefinedLength(definedLength);
		}
	}
	;

AttributeVR	: VR
	{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = yylval.vr;
			transferVr = TRANSFER_ATTR_VR_IMPLICIT;
			definedLength = false;
		}
	}
	| '[' VR ']'
	{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = yylval.vr;
			transferVr = TRANSFER_ATTR_VR_IMPLICIT;
			definedLength = true;
		}
	}
	| T_OPEN_BRACKET VR T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = yylval.vr;
			transferVr = TRANSFER_ATTR_VR_UNKNOWN;
			definedLength = false;
		}
	}
	| T_OPEN_BRACKET '[' VR ']' T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = yylval.vr;
			transferVr = TRANSFER_ATTR_VR_UNKNOWN;
			definedLength = true;
		}
	}
	| T_OPEN_BRACKET VR '?' T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = yylval.vr;
			transferVr = TRANSFER_ATTR_VR_QUESTION;
			definedLength = false;
		}
	}
	| T_OPEN_BRACKET '[' VR ']' '?' T_CLOSE_BRACKET
	{
        if (!scriptParseOnly)
		{	  
			// store vr, transfer vr & defined length locally
			vr = yylval.vr;
			transferVr = TRANSFER_ATTR_VR_QUESTION;
			definedLength = true;
		}
	}
	;

Values	: // nothing - no attribute value
	| VMList
	;

VMList	: Value
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
	}
	| VMList ',' Value
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
	}
	;

Value	: STRING
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
				convertDoubleBackslash($1);
				
				// store string attribute value locally
				value_ptr = stringValue(scriptSession_ptr, $1, vr, group, element);
			}
			else if (vr == ATTR_VR_UN)
			{
				// although we have parsed a string from the DS Script - the UN value
				// should be interpreted as a byte array - this can contain byte values of 0
				// (zero) which would normally be seen as a string terminator bu tin this
				// case must be stored as part of the byte array
				value_ptr = byteArrayValue($1);
			}
			else
			{
				convertHex($1);
				
				// store string attribute value locally
				value_ptr = stringValue(scriptSession_ptr, $1, vr, group, element);
			}
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	| HEXADECIMAL
	{
        if (!scriptParseOnly)
		{	  
			// store hex attribute value locally
			value_ptr = hexValue(scriptSession_ptr, $1, vr, group, element);
		}
	}
	| INTEGER
	{
        if (!scriptParseOnly)
		{	  
			// store integer attribute value locally
			value_ptr = integerValue(scriptSession_ptr, $1, vr, group, element);
		}
	}
	| T_AUTOSET
	{
        if (!scriptParseOnly)
		{	  
			// autoset attribute value
			value_ptr = autoSetValue(scriptSession_ptr, vr, group, element);
		}
	}
	;
TagRef1 : TagRef
	{
        if (!scriptParseOnly)
		{	  
			// copy tag to reference one
			group1 = group;
			element1 = element;
		}
	}
	;

TagRef2 : TagRef
	{
        if (!scriptParseOnly)
		{	  
			// copy tag to reference two
			group2 = group;
			element2 = element;
		}
	}
	;

TagRef: AttributeTag
	{
	}
	| STRING
	{
        if (!scriptParseOnly)
		{	  
			// look up tag by name and get group & element returned
			group = 0;
			element = 0;
		}
		// free malloced string buffer
		free($1);
		$1 = NULL;
	}
	;

ObjectRef1	: Command
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
	}
	| Dataset
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
	}
	;

ObjectRef2	: Command
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
	}
	| Dataset
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
	}
	;

SequenceRef	: T_OPEN_BRACKET Dataset TagRef ItemNumber T_CLOSE_BRACKET
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
	}
	| T_OPEN_BRACKET SequenceRef TagRef ItemNumber T_CLOSE_BRACKET
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
	}
	;

ItemNumber	:
	{
        if (!scriptParseOnly)
		{	  
			// default Item Number is 1 - index is then zero 
			itemNumber = 0;
		}
	}
	| '[' INTEGER ']'
	{
        if (!scriptParseOnly)
		{	  
			// take given Item Number - index is one less
			itemNumber = $2 - 1;
		}
	}
	;

FileheadContents	: Filehead 
	| Filehead  T_OPEN_BRACKET FileheadParameterList T_CLOSE_BRACKET
	;

Filehead	: T_FILEHEAD
	{
        if (!scriptParseOnly)
		{	  
			fileHead_ptr = new FILEHEAD_CLASS();

			// cascade the logger
			fileHead_ptr->setLogger(scriptSession_ptr->getLogger());

			identifier = "";
		}
	}
	;

FileheadParameterList	: FileheadParameter
	| FileheadParameterList FileheadParameter
	;

FileheadParameter	: FilePreamble
	| DicomPrefix
	| FileTransferSyntax
	;

FilePreamble	: T_FILE_PREAMBLE STRING 
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			if (fileHead_ptr)
			{
				fileHead_ptr->setPreambleValue($2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	;

DicomPrefix	: T_DICOM_PREFIX STRING
	{
        if (!scriptParseOnly)
		{	  
			convertHex($2);
			if (fileHead_ptr)
			{
				fileHead_ptr->setPrefix($2);
			}
		}
		// free malloced string buffer
		free($2);
		$2 = NULL;
	}
	;

FileTransferSyntax : T_TRANSFER_SYNTAX TransferSyntax
	{
        if (!scriptParseOnly)
		{	  
			if (fileHead_ptr)
			{
				fileHead_ptr->setTransferSyntaxUid(transferSyntaxName.getUid());
			}
		}
	}
	;

FiletailContents	: Filetail 
	| Filetail T_OPEN_BRACKET FiletailParameterList T_CLOSE_BRACKET
	;

Filetail	: T_FILETAIL
	{
        if (!scriptParseOnly)
		{	  
			fileTail_ptr = new FILETAIL_CLASS();

			// cascade the logger
			fileTail_ptr->setLogger(scriptSession_ptr->getLogger());

			identifier = "";
		}
	}
	;

FiletailParameterList	: FiletailParameter
	| FiletailParameterList FiletailParameter
	;

FiletailParameter	: DatasetTrailingPadding
	| SectorSize
	| PaddingValue
	;

DatasetTrailingPadding	: T_DATASET_TRAILING_PADDING T_YES
	{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setTrailingPadding(true);
			}
		}
	}
	| T_DATASET_TRAILING_PADDING T_NO
	{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setTrailingPadding(false);
			}
		}
	}
	;

SectorSize	: T_SECTOR_SIZE INTEGER
	{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setSectorSize($2);
			}
		}
	}
	;

PaddingValue	: T_PADDING_VALUE INTEGER
	{
        if (!scriptParseOnly)
		{	  
			if (fileTail_ptr)
			{
				fileTail_ptr->setPaddingValue((BYTE)$2);
			}
		}
	}
	;
%%

