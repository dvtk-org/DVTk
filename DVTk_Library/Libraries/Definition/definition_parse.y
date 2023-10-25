%{
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
%}


%union {
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
}


%token					T_SYSTEM 
%token 					T_DEFINE T_ENDDEFINE 
%token 					T_METASOPCLASS T_SOPCLASS
%token 					T_MODULE	
%token      			T_ITEM T_MACRO
%token					T_INCLUDEITEM T_INCLUDEMACRO
%token 					T_SQ		
%token					T_AND T_OR T_NOT T_PRESENT T_VALUE T_EMPTY T_TRUE T_FALSE
%token					T_EQUAL T_LESS T_GREATER T_LESS_OR_EQUAL T_GREATER_OR_EQUAL
%token					T_WEAK T_WARN
%token <string>			STRING 
%token <integer>		INTEGER 
%token <hex>			HEX
%token <commandField>	COMMANDFIELD 
%token <type>           TYPE 
%token <usage>			USAGE 
%token <valueType>		VALUETYPE 
%token <vr>				VR

%type <string>			SystemName SystemVersion 
%type <string>			AEName AEVersion
%type <string>			MetaSopClassUID MetaSopClassName
%type <string>			SopClassUID SopClassName MacroName
%type <hex>				AttributeTag
%type <string>			AttributeVM
%type <string>			AttributeIM
%type <node_ptr>		CondExpr PrimaryCond SecondaryCond Expr SimpleExpr 
%type <unary_node_ptr>	AttributeTagAddress AttributeTagUpPath AttributeTagRootPath AttributeTagDownPath AttributeTagPath AttributeTagHere AttributeTagUp AttributeTagRoot AttributeTagDown
%type <binary_node_ptr> Operator
%type <integer>			ValueNr
%type <string>			Value CondValue ValuesList
%type <valueType>       ValueType FirstValuesDef

%%
DefinitionGrammar	: DefinitionComponents
	{
		ResolveMacroReferences();  
		ResetVariables();
	} 
	;

DefinitionComponents	: Definition
	| DefinitionComponents Definition
	;


Definition	: Define
	| error
	{
		definitionwarn("Skipping to next define");
		definitionParseOnly = true;
		YYABORT;
	}
	;

Define	: BeginDefine Definitions EndDefine  
	;

BeginDefine	: T_DEFINE	   
	;

EndDefine	: T_ENDDEFINE  
	{
		// reset the skip definition parameter to allow more than
		// 1 sopclass definition in 1 file
		skipDefinition = false;	
	}
	;

Definitions	: SystemDef
	| MetaSopClassDef
	| CommandDef
	| CommandIodDef
	| ItemDef
	| MacroDef
	;

SystemDef	: T_SYSTEM SystemDefinition AEDefinition	
	;

SystemDefinition	: SystemName SystemVersion
	{
		if (!definitionParseOnly)
		{
			defFileContent_ptr->SetSystem(lsystemname, lsystemversion);
		}
	}
	;

SystemName	: STRING    
	{ 
		strcpy(lsystemname, yyval.string);
	}
	;

SystemVersion	: STRING    
	{ 
		strcpy(lsystemversion, yyval.string);
	}
	;


AEDefinition	: AEName AEVersion 
	{
		if (!definitionParseOnly)
		{
			defFileContent_ptr->SetAE(lAEname, lAEversion);
		}
	}
	;

AEName	: STRING    
	{ 
		strcpy(lAEname, yyval.string);
	}
	;

AEVersion	: STRING    
	{ 
		strcpy(lAEversion, yyval.string);
	}
	;


MetaSopClassDef	: T_METASOPCLASS BeginMetaSopClassDef SopClassDefList
	{
		if (!definitionParseOnly)
		{
			defFileContent_ptr->SetMetaSop(currentMetaSopClass_ptr);
			definitionfileempty = false;
		}
	}
	;

BeginMetaSopClassDef: MetaSopClassUID MetaSopClassName
	{
		if (!definitionParseOnly)
		{
			currentMetaSopClass_ptr = new DEF_METASOPCLASS_CLASS(lMetaSOPClassUID, lMetaSOPClassName);
		}
	}
	;

MetaSopClassUID	: STRING
	{ 
		strcpy(lMetaSOPClassUID, yyval.string);
	}
	;

MetaSopClassName	: STRING
	{ 
		strcpy(lMetaSOPClassName, yyval.string);
	}
	;

SopClassDefList	: SopClassDef
	{
		if (!definitionParseOnly)
		{
			currentMetaSopClass_ptr->AddSopClass(lSOPClassUID, lSOPClassName);
		}
	}	
	| SopClassDefList SopClassDef
	{
		if (!definitionParseOnly)
		{
			currentMetaSopClass_ptr->AddSopClass(lSOPClassUID, lSOPClassName);
		}
	}
	;

SopClassDef	: T_SOPCLASS SopClassUID SopClassName 
	;

SopClassUID	: STRING	
	{ 
		strcpy(lSOPClassUID, yyval.string);	  
	}
	;

SopClassName	: STRING	
	{ 
		strcpy(lSOPClassName, yyval.string);	  
	}
	;

CommandDef	: DimseCmdDef AttributeDefList
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentCommand_ptr->AddModule(currentModule_ptr);
			defFileContent_ptr->AddCommand(currentCommand_ptr);
			definitionfileempty = false;
		}
	}
	;

DimseCmdDef	: DimseCmd
	;

DimseCmd	: COMMANDFIELD
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			// assume we have a command definition
			currentCommand_ptr = new DEF_COMMAND_CLASS();
			currentCommand_ptr->SetDimseCmd(yyval.commandField);
			currentDICOMObject_ptr = currentCommand_ptr;

			lDimseCmd = yyval.commandField;

			// create module for command attributes.
			currentModule_ptr = new DEF_MODULE_CLASS();
			currentModule_ptr->SetUsage(MOD_USAGE_M);
			currentModule_ptr->SetName(mapCommandName(lDimseCmd));                      
			currentAttributeGroup_ptr = currentModule_ptr;
 		}
	}
	;

CommandIodDef : DimseCmdDef IodName IodContentDef
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			definitionfileempty = false;
		}
	}
	;

IodName	: STRING 
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

			strcpy(lIodName, yyval.string);
			currentDataset_ptr = new DEF_DATASET_CLASS(lIodName);
			currentDataset_ptr->SetDimseCmd(lDimseCmd);
			currentDICOMObject_ptr = currentDataset_ptr;
		}
	}
	;

IodContentDef : SopClassDefinition ModuleDefList
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			// add the dataset to the SOPClass Definition
            currentSopClass_ptr->AddDataset(currentDataset_ptr);
		}
	}
    | SopClassDefinition
	{
		// the dataset is empty
	}
	;

SopClassDefinition	: SopClassDef
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
	}
	;

ModuleDefList	: ModuleDef
	| ModuleDefList ModuleDef
	;

ModuleDef	: ModuleNameDef AttributeDefList
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentDICOMObject_ptr->AddModule(currentModule_ptr);
		}
	}
	| ModuleNameDef
	{
		// the module is empty - ignore it
	}
	;

ModuleNameDef	: T_MODULE ModuleName ModuleUsage
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
	}
	| T_MODULE
	{
		// the module name and usage not present - ignore it
	}
	;

ModuleName	: STRING 
	{
		strcpy(lModuleName, yyval.string);
	}
	;


ModuleUsage	: ModUsage ModuleCondition
	;

ModUsage	: USAGE
	{
		lModuleUsage = yyval.usage;					      
	}
	;

ModuleCondition	: Condition
	;

AttributeDefList	: AttributeDef
	| AttributeDefList AttributeDef
	;

AttributeDef	: '(' AttributeTagDef ',' AttributeType ',' AttributeValueDef ')' AttributeName AttributeCondition
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttributeGroup_ptr->AddAttribute(currentAttribute_ptr);
		}
	}
	| MacroReference 
	;


AttributeTagDef	: AttributeTag	
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr = new DEF_ATTRIBUTE_CLASS(lAttrGroup, lAttrElement);
		}
	}
	;

AttributeTag	: HEX           
	{ 
		lAttrGroup   = (UINT16) (definitionlval.hex >> 16);
		lAttrElement = (UINT16) (yyval.hex & 0x0000FFFF);
	}
	;

AttributeType	: INTEGER       
	{
		if (!definitionParseOnly && !skipDefinition)
		{ 
			switch(yyval.integer)
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
	}
	| TYPE			                      
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetType(yyval.type);
		}
	}
	;

AttributeName	: STRING        
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetName(yyval.string);
		}
	}
	;

AttributeCondition	: Condition
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
	}
	;

AttributeValueDef	: SequenceValueDef
	| OtherValueDef
	;

SequenceValueDef	: SequenceIntro ItemAttributeDefList
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
	}
	| SequenceIntro
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
	}
	;

SequenceIntro	: T_SQ ',' AttributeVM ','
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
	}
	// Reuse AttributeVM as number of items
	| T_SQ ',' AttributeVM ',' AttributeIM ','
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
	}
	;

OtherValueDef	: AttributeVR ',' AttributeVM ValuesDef
	;

ItemAttributeDefList: ItemAttributeDef
	| ItemAttributeDefList ItemAttributeDef
	;


ItemAttributeDef	: T_GREATER AttributeDef
	;

AttributeVM	: INTEGER
	{
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) $1); 
			currentAttribute_ptr->SetVmMax((UINT) $1); 
		}
	}
	| INTEGER ':' INTEGER        
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) $1); 
			currentAttribute_ptr->SetVmMax((UINT) $3); 
		}
	}
	| INTEGER ':' 'n'            
	{  
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) $1); 
			currentAttribute_ptr->SetVmMax((UINT) MAXVM); 
		}
	}
	| STRING                     
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			CreateVMFromString(yyval.string, currentAttribute_ptr);
		}
	} 
	| 'n'                        
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetVmMin((UINT) 0); 
			currentAttribute_ptr->SetVmMax((UINT) MAXVM); 
		}
	} 
	;

AttributeIM	: INTEGER
	{
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) $1); 
			currentAttribute_ptr->SetImMax((UINT) $1); 
		}
	}
	| INTEGER ':' INTEGER        
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) $1); 
			currentAttribute_ptr->SetImMax((UINT) $3); 
		}
	}
	| INTEGER ':' 'n'            
	{  
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) $1); 
			currentAttribute_ptr->SetImMax((UINT) MAXVM); 
		}
	}
	| 'n'                        
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{ 
			currentAttribute_ptr->SetImMin((UINT) 0); 
			currentAttribute_ptr->SetImMax((UINT) MAXVM); 
		}
	} 
	;

AttributeVR	: VR 
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			lAttrVr = yyval.vr;
			currentAttribute_ptr->SetVR(lAttrVr);
			}
	}
	| VR '/' VR
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			lAttrVr = $1;
			currentAttribute_ptr->SetVR(lAttrVr);
			currentAttribute_ptr->SetSecondVR($3);
		}
	}
	;

ValuesDef	: 
	{
		// empty
	}
	| ',' FirstValuesDef 
	{
		// reset the list index for the next attribute values
		currentValueListIndex = 0;
	}
	| ',' FirstValuesDef ',' OtherValuesDefList
	{
		// reset the list index for the next attribute values
		currentValueListIndex = 0;
	}
	;
	
FirstValuesDef	: ValueType ',' ValuesList
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
	}
	;

OtherValuesDefList	:  OtherValuesDef
	| OtherValuesDefList ',' OtherValuesDef
	;

OtherValuesDef	: ValueType ',' ValuesList
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttribute_ptr->SetValueType(currentValueType, currentValueListIndex);
			currentValueListIndex++;
		}
	}
	| ValuesList
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			// a ValuesList with no specified type gets the type
			// of the first ValuesList specified
			// add value to attribute - indexed by value list index
			currentAttribute_ptr->SetValueType(lFirstValueType, currentValueListIndex);
			currentValueListIndex++;
		}
	}
	;

ValueType	: VALUETYPE
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentValueType = yyval.valueType;
		}
	}
	;

ValuesList	: Value
	{
		if (!definitionParseOnly && !skipDefinition)
		{
    		currentAttribute_ptr->AddValue(currentValue_ptr, currentValueListIndex);
		}
	} 
	| ValuesList '|' Value
	{
		if (!definitionParseOnly && !skipDefinition)
		{                         
    		currentAttribute_ptr->AddValue(currentValue_ptr, currentValueListIndex);
		}
	}
	;

Value	: STRING   
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			currentValue_ptr = CreateValueFromString(yyval.string, lAttrVr);
		}
	}
	| HEX      
	{  
		if (!definitionParseOnly && !skipDefinition)
		{					   
			currentValue_ptr = CreateValueFromHex(yyval.hex, lAttrVr);
		}
	}
	| INTEGER  
	{ 
		if (!definitionParseOnly && !skipDefinition)
		{
			currentValue_ptr = CreateValueFromInt(yyval.integer, lAttrVr);
		}
	}
	;

ItemDef	: T_ITEM ItemName AttributeDefList
	;

ItemName	: STRING
	{
	}
	;

MacroDef	: MacroBegin AttributeDefList
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
	} 
	| MacroBegin
	{
		// Macro is empty - ignore it
	}
	;

MacroBegin	: T_MACRO MacroName
	{
		if (!definitionParseOnly)
		{
			currentMacro_ptr = new DEF_MACRO_CLASS($2);
    		currentAttributeGroup_ptr = currentMacro_ptr;
		}
	}
	| T_MACRO
	{
		// Macro name is not present - ignore it
		if (!definitionParseOnly)
		{
			currentMacro_ptr = new DEF_MACRO_CLASS("NO MACRO NAME - IGNORED");
    		currentAttributeGroup_ptr = currentMacro_ptr;
		}
	}
    ;

MacroName	: STRING	
	;

MacroReference	: T_INCLUDEMACRO MacroName MacroCondition
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			currentAttributeGroup_ptr->AddMacroReference($2, currentCondition_ptr, lTextualCondition);
			groups_with_refs.push_back(currentAttributeGroup_ptr);

			// reset condition pointer
			currentCondition_ptr = NULL;
			
			// reset textual condition
			lTextualCondition.erase();	
		}
	}
	;

MacroCondition	: Condition
	;


Condition	:
	| ':' STRING
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			lTextualCondition = $2;
		
			// reset condition pointer
			currentCondition_ptr = NULL;	
		}
	}
	| ':' CondExpr 
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			// create new condition
			currentCondition_ptr = new CONDITION_CLASS();

			// add the primary root node to the condition
			condition_node_ptr = $2;
			currentCondition_ptr->SetPrimaryNode(condition_node_ptr);
			
			// reset textual condition
			lTextualCondition.erase();
		}
	} 
	| ':' T_WEAK '(' PrimaryCond ',' SecondaryCond ')' 
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			// create new condition
			currentCondition_ptr = new CONDITION_CLASS();

			// add the primary root node to the condition
			condition_node_ptr = $4;
			currentCondition_ptr->SetPrimaryNode(condition_node_ptr);

			// add the secondary root node to the condition
			condition_node_ptr = $6;
			currentCondition_ptr->SetSecondaryNode(condition_node_ptr);
			
			// reset textual condition
			lTextualCondition.erase();			
		}
	}
	;

PrimaryCond	: CondExpr
	{
		$$ = $1;
	}
	;

SecondaryCond	: CondExpr
	{
		$$ = $1;
	}
	;

CondExpr	: Expr
	{
		$$ = $1;
	}
	| CondExpr T_OR Expr
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_OR_NODE_CLASS($1, $3);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;

Expr	: SimpleExpr
	{
		$$ = $1;
	}
	| Expr T_AND SimpleExpr
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_AND_NODE_CLASS($1, $3);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;

SimpleExpr	: T_PRESENT AttributeTagAddress
	{							
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_PRESENT_NODE_CLASS($2);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	| T_EMPTY AttributeTagAddress
	{
		if (!definitionParseOnly && !skipDefinition)
		{
//			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);  
//			node_ptr->SetLogger(definitionfilelogger_ptr);    

//			$$ = new CONDITION_EMPTY_NODE_CLASS(node_ptr);
			$$ = new CONDITION_EMPTY_NODE_CLASS($2);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	| T_VALUE AttributeTagAddress ValueNr Operator CondValue
	{
		if (!definitionParseOnly && !skipDefinition)
		{
//			node_left_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
//			node_left_ptr->SetLogger(definitionfilelogger_ptr);

			node_right_ptr = new CONDITION_LEAF_VALUE_NR_CLASS((UINT16)$3);
			node_right_ptr->SetLogger(definitionfilelogger_ptr);

//			node_left_ptr = new CONDITION_VALUE_NODE_CLASS(node_left_ptr, node_right_ptr);
			node_left_ptr = new CONDITION_VALUE_NODE_CLASS($2, node_right_ptr);
			node_left_ptr->SetLogger(definitionfilelogger_ptr);

			node_right_ptr = new CONDITION_LEAF_CONST_CLASS($5);
			node_right_ptr->SetLogger(definitionfilelogger_ptr);

			$4->SetLeft(node_left_ptr);
			$4->SetRight(node_right_ptr);

			$$ = $4;
		}
	}
	| T_NOT SimpleExpr
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_NOT_NODE_CLASS($2);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	| '(' CondExpr ')'
	{
		$$ = $2;
	}
	| T_WARN '(' CondExpr ')'
	{
		$$ = $3;
		$$->SetConditionType(CONDITION_TYPE_WARNING);
	}	
	| T_TRUE
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_LEAF_TRUE_CLASS(true);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	| T_FALSE
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_LEAF_TRUE_CLASS(false);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;

ValueNr	: 
	{ 
		$$ = APPLY_TO_ANY_VALUE;
	}
	| INTEGER    
	{
		$$ = $1;
	}
	;

CondValue	: STRING
	;

Operator	: T_EQUAL		
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_EQ_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	| T_LESS				
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_LESS_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	| T_GREATER           
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_GREATER_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	| T_LESS_OR_EQUAL     
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_LESS_EQ_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
        }
	}
	| T_GREATER_OR_EQUAL  
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_GREATER_EQ_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;
	
AttributeTagAddress	: AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);			
			$$ = new CONDITION_NAVIGATION_HERE_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
			$$->SetNode(node_ptr);
		}
	}
	| AttributeTagPath
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = $1;
		}
	}
	;
	
AttributeTagPath	: AttributeTagHere AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);
			$1->SetNode(node_ptr);
			$$ = $1;
		}
	}
	| AttributeTagUpPath AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			$$ = $1;
		}
	}
	| AttributeTagRootPath AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			$$ = $1;
		}
	}
	| AttributeTagDownPath AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			$$ = $1;
		}
	}
	| AttributeTagHere AttributeTagDownPath AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $2;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			$1->SetNode($2);
			$$ = $1;
		}
	}
	| AttributeTagUpPath AttributeTagDownPath AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode($2);

			lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $2;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			$$ = $1;
		}
	}
	| AttributeTagRootPath AttributeTagDownPath AttributeTag
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			node_ptr = new CONDITION_NAVIGATION_TAG_CLASS(lAttrGroup, lAttrElement);   
			node_ptr->SetLogger(definitionfilelogger_ptr);

			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode($2);

			lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $2;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode(node_ptr);
			$$ = $1;
		}
	}
	;
	
AttributeTagHere	: '.' '/'
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_NAVIGATION_HERE_NODE_CLASS(true);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;
	
AttributeTagUpPath	: AttributeTagUp
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = $1;
		}
	}
	| AttributeTagUpPath AttributeTagUp 
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode($2);
			$$ = $1;
		}
	}
	;
	
AttributeTagUp	: '.' '.' '/'
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_NAVIGATION_UP_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;

AttributeTagRootPath	: AttributeTagRoot
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = $1;
		}
	}
	| AttributeTagRootPath AttributeTagRoot 
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode($2);
			$$ = $1;
		}
	}
	;

AttributeTagRoot	: '/'
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_NAVIGATION_ROOT_NODE_CLASS();
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;
	
AttributeTagDownPath	: AttributeTagDown
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = $1;
		}
	}
	| AttributeTagDownPath AttributeTagDown 
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			CONDITION_UNARY_NODE_CLASS* lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) $1;
			while(lNode_ptr->GetNode())
			{
				lNode_ptr = (CONDITION_UNARY_NODE_CLASS*) lNode_ptr->GetNode();
			}
			lNode_ptr->SetNode($2);
			$$ = $1;
		}
	}
	;
	
AttributeTagDown	: AttributeTag '/'
	{
		if (!definitionParseOnly && !skipDefinition)
		{
			$$ = new CONDITION_NAVIGATION_DOWN_NODE_CLASS(lAttrGroup, lAttrElement);
			$$->SetLogger(definitionfilelogger_ptr);
		}
	}
	;
%%


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