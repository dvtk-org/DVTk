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
 * contains the Lexer for the extended character set output
 */
#include <stdlib.h>
#include <ctype.h>
#include <string.h>
#include <stdio.h>

#define YY_NEVER_INTERACTIVE 1 

#include "Iglobal.h"	// Global component interface
#include "Ilog.h"		// Log component interface
#include "imagedisplay_parse.cpp.h"

void yyerror(char* msg);
void yywarn(char* msg);

// export variables for logging purposes
LOG_CLASS *imagedisplaylogger_ptr = NULL;

// variables to keep track of current line
static char curr_linebuf[MAXIMUM_LINE_LENGTH];

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
							// reset token position and save the next line
							strcpy(curr_linebuf, yytext+1);
							yyless(1); // give back all but the \n to rescan 
                        }
\\\n					{ /* continuation line */ 
						}

DEFINE						 { return(T_DEFINE); }
ENDDEFINE					 { return(T_ENDDEFINE); } 
SYSTEM						 { return(T_SYSTEM); }
IMAGE-DISPLAY-FORMAT	     { return(T_IMAGEDISPLAYFORMAT); }
ANNOTATION-DISPLAY-FORMAT-ID { return(T_ANNOTATIONDISPLAYFORMATID); }

[0-9]:[0-9]+n           {
                            strcpy(yylval.string, yytext);
                            return(STRING);
						}
(\+|\-)[0-9]+	    |
[0-9]+					{ 
                            yylval.integer = atoi(yytext); 
							return(INTEGER); 
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
						    return(STRING);
						}
.						{ 
                            return(yytext[0]); 
						}
%%

void yyerror(char *errmsg)
{
	if (imagedisplaylogger_ptr)
	{
		imagedisplaylogger_ptr->text(LOG_ERROR, 1, "lineno: %d %s: ", yylineno, errmsg);
		imagedisplaylogger_ptr->text(LOG_NONE, 1, "Current line: ");
		imagedisplaylogger_ptr->text(LOG_NONE, 1, "%-50s...", curr_linebuf);
	}
}

void yywarn(char *errmsg)
{
	if (imagedisplaylogger_ptr)
	{
		imagedisplaylogger_ptr->text(LOG_WARNING, 1, "lineno: %d %s: ", yylineno, errmsg);
		imagedisplaylogger_ptr->text(LOG_NONE, 1, "Current line: ");
		imagedisplaylogger_ptr->text(LOG_NONE, 1, "%-50s...", curr_linebuf);
	}
}
