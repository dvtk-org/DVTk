%{
// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2005
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

//*****************************************************************************
//  DESCRIPTION     :	Extended Character Set File Parser.
//*****************************************************************************

#include <malloc.h>

#include "Iglobal.h"	// Global component interface
#include "Ilog.h"		// Log component interface
#include "ext_char_set_definition.h"


// flex / bison error and warning functions
extern void extcharseterror(char* msg);
extern void extcharsetwarn(char* msg);
extern int  extcharsetlex(void);

// local variables - unfortunately this structure is needed for YACC/LEX
// they are used as temporary variables
static CHARACTER_SET_CLASS* current_character_set_ptr = NULL;
static CODE_ELEMENT_CLASS* current_code_element_ptr = NULL;
%}


%union {
    NAME_STRING string;
}


%token T_DEFINE T_ENDDEFINE T_SYSTEM
%token T_CHARACTER_SET T_CODE_ELEMENT
%token T_CODE_EXTENSIONS T_NO_CODE_EXTENSIONS T_ESC
%token <string>	STRING 

%type <string> CSDescription
%type <string> CodeElement
%type <string> CEDefinedTerm CEStandardForCodeExt CEEscSequence CEISORegNr CECodeElement CECharSet
%type <string> CENrChars CEBytes

%%
DefinitionGrammar	: DefinitionComponents
	;

DefinitionComponents	: Definition
	| DefinitionComponents Definition
	;

Definition	: BeginDefine DefinitionChoice EndDefine  
	;

BeginDefine	: T_DEFINE	   
	;

EndDefine	: T_ENDDEFINE  
	;

DefinitionChoice	: SystemDef
	| CharacterSet
	;

SystemDef	: T_SYSTEM SystemDefinition AEDefinition	
	;

SystemDefinition	: STRING STRING
	{
	}
	;

AEDefinition	: STRING STRING
	{
	}
	;

CharacterSet	: T_CHARACTER_SET CSDescription CSDefinition
	{
	EXTCHARACTERSET->AddCharacterSet(current_character_set_ptr);
	current_character_set_ptr = NULL;
	}
	;

CSDescription	: STRING T_NO_CODE_EXTENSIONS
	{  
	current_character_set_ptr = new CHARACTER_SET_CLASS($1, false);
	}
	| STRING T_CODE_EXTENSIONS
	{  
	current_character_set_ptr = new CHARACTER_SET_CLASS($1, true);
	}
	;

CSDefinition : CodeElementList
	;

CodeElementList	: CodeElement
	{
	if (current_character_set_ptr)
	{
		current_character_set_ptr->AddCodeElement(current_code_element_ptr);
	}
	current_code_element_ptr = NULL;
	}
	| CodeElementList CodeElement
	{
	if (current_character_set_ptr)
	{
		current_character_set_ptr->AddCodeElement(current_code_element_ptr);
	}
	current_code_element_ptr = NULL;
	}
	;


CodeElement	: T_CODE_ELEMENT '(' CEBytes ',' CEDefinedTerm ',' CEStandardForCodeExt ',' T_ESC CEEscSequence ',' CEISORegNr ',' CENrChars ',' CECodeElement ',' CECharSet ')' 
	{
	// create new code element
	current_code_element_ptr = new CODE_ELEMENT_CLASS($5);				

	// and set parameters	
	if (strcmp($3, "MULTI") == 0)
	{
		current_code_element_ptr->SetMultiByte(true);
	}
	else 
	{
		current_code_element_ptr->SetMultiByte(false);
	}

	current_code_element_ptr->SetDefinedTerm($5);
	current_code_element_ptr->SetStdForCodeExt($7);
	current_code_element_ptr->SetEscSequenceCRFormat($10);
	current_code_element_ptr->SetISORegNr($12);
	current_code_element_ptr->SetNrChars($14);
	current_code_element_ptr->SetCodeElementName($16);
	current_code_element_ptr->SetCharacterSet($18);
	}
	;

CEBytes	: STRING
	;

CEDefinedTerm	: STRING
	;
	
CEStandardForCodeExt	: STRING
	; 
	
CEEscSequence	: STRING
	;
	
CEISORegNr	: STRING
	;
	
CENrChars	: STRING
	;
	
CECodeElement	: STRING
	;
	
CECharSet	: STRING
	;
%%
