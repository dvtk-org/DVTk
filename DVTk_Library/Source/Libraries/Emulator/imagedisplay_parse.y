%{
// Part of Dvtk Libraries - Internal Native Library Code
// Copyright © 2001-2006
// Philips Medical Systems NL B.V., Agfa-Gevaert N.V.

/*
 * A parser for the imagedisplay files
 */

#include <malloc.h>

#include "Iglobal.h"	// Global component interface
#include "Ilog.h"		// Log component interface
#include "print.h"

// flex / bison error and warning functions
extern void imagedisplayerror(char* msg);
extern void imagedisplaywarn(char* msg);
extern int  imagedisplaylex(void);

%}

%union {
	int			integer;
    NAME_STRING string;
}

%token					T_SYSTEM 
%token 					T_DEFINE T_ENDDEFINE 
%token                  T_IMAGEDISPLAYFORMAT T_ANNOTATIONDISPLAYFORMATID
%token <string>			STRING 
%token <integer>		INTEGER 

%%
DefinitionGrammar	: DefinitionComponents
	;

DefinitionComponents	: DefinitionChoice
	| DefinitionComponents DefinitionChoice
	;

DefinitionChoice	: Definition
	| ImageDisplayFormatDef
	| AnnotationDisplayFormatIDDef
	;

Definition	: BeginDefine SystemDef EndDefine  
	;

BeginDefine	: T_DEFINE	   
	;

EndDefine	: T_ENDDEFINE  
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
	
ImageDisplayFormatDef	: T_IMAGEDISPLAYFORMAT STRING INTEGER
	{
		MYPRINTER->addImageDisplayFormat($2, $3);
	}
	;

AnnotationDisplayFormatIDDef	: T_ANNOTATIONDISPLAYFORMATID STRING INTEGER
	{
		MYPRINTER->addAnnotationDisplayFormatId($2, $3);
	}
	;	
%%
