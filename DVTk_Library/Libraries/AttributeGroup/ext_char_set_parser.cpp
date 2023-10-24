
/*  A Bison parser, made from ext_char_set_parser.y with Bison version GNU Bison version 1.24
  */

#define YYBISON 1  /* Identify Bison output.  */

#define	T_DEFINE	258
#define	T_ENDDEFINE	259
#define	T_SYSTEM	260
#define	T_CHARACTER_SET	261
#define	T_CODE_ELEMENT	262
#define	T_CODE_EXTENSIONS	263
#define	T_NO_CODE_EXTENSIONS	264
#define	T_ESC	265
#define	STRING	266

#line 1 "ext_char_set_parser.y"

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

#line 29 "ext_char_set_parser.y"
typedef union {
    NAME_STRING string;
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



#define	YYFINAL		56
#define	YYFLAG		-32768
#define	YYNTBASE	15

#define YYTRANSLATE(x) ((unsigned)(x) <= 266 ? yytranslate[x] : 37)

static const char yytranslate[] = {     0,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,    12,
    14,     2,     2,    13,     2,     2,     2,     2,     2,     2,
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
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     1,     2,     3,     4,     5,
     6,     7,     8,     9,    10,    11
};

#if YYDEBUG != 0
static const short yyprhs[] = {     0,
     0,     2,     4,     7,    11,    13,    15,    17,    19,    23,
    26,    29,    33,    36,    39,    41,    43,    46,    66,    68,
    70,    72,    74,    76,    78,    80
};

static const short yyrhs[] = {    16,
     0,    17,     0,    16,    17,     0,    18,    20,    19,     0,
     3,     0,     4,     0,    21,     0,    24,     0,     5,    22,
    23,     0,    11,    11,     0,    11,    11,     0,     6,    25,
    26,     0,    11,     9,     0,    11,     8,     0,    27,     0,
    28,     0,    27,    28,     0,     7,    12,    29,    13,    30,
    13,    31,    13,    10,    32,    13,    33,    13,    34,    13,
    35,    13,    36,    14,     0,    11,     0,    11,     0,    11,
     0,    11,     0,    11,     0,    11,     0,    11,     0,    11,
     0
};

#endif

#if YYDEBUG != 0
static const short yyrline[] = { 0,
    45,    48,    49,    52,    55,    58,    61,    62,    65,    68,
    73,    78,    85,    89,    95,    98,   106,   117,   142,   145,
   148,   151,   154,   157,   160,   163
};

static const char * const yytname[] = {   "$","error","$undefined.","T_DEFINE",
"T_ENDDEFINE","T_SYSTEM","T_CHARACTER_SET","T_CODE_ELEMENT","T_CODE_EXTENSIONS",
"T_NO_CODE_EXTENSIONS","T_ESC","STRING","'('","','","')'","DefinitionGrammar",
"DefinitionComponents","Definition","BeginDefine","EndDefine","DefinitionChoice",
"SystemDef","SystemDefinition","AEDefinition","CharacterSet","CSDescription",
"CSDefinition","CodeElementList","CodeElement","CEBytes","CEDefinedTerm","CEStandardForCodeExt",
"CEEscSequence","CEISORegNr","CENrChars","CECodeElement","CECharSet",""
};
#endif

static const short extcharsetr1[] = {     0,
    15,    16,    16,    17,    18,    19,    20,    20,    21,    22,
    23,    24,    25,    25,    26,    27,    27,    28,    29,    30,
    31,    32,    33,    34,    35,    36
};

static const short extcharsetr2[] = {     0,
     1,     1,     2,     3,     1,     1,     1,     1,     3,     2,
     2,     3,     2,     2,     1,     1,     2,    19,     1,     1,
     1,     1,     1,     1,     1,     1
};

static const short extcharsetdefact[] = {     0,
     5,     1,     2,     0,     3,     0,     0,     0,     7,     8,
     0,     0,     0,     0,     6,     4,    10,     0,     9,    14,
    13,     0,    12,    15,    16,    11,     0,    17,    19,     0,
     0,    20,     0,     0,    21,     0,     0,     0,    22,     0,
     0,    23,     0,     0,    24,     0,     0,    25,     0,     0,
    26,     0,    18,     0,     0,     0
};

static const short extcharsetdefgoto[] = {    54,
     2,     3,     4,    16,     8,     9,    12,    19,    10,    14,
    23,    24,    25,    30,    33,    36,    40,    43,    46,    49,
    52
};

static const short extcharsetpact[] = {     1,
-32768,     1,-32768,    -5,-32768,    -4,    -3,     2,-32768,-32768,
    -2,    -1,    -6,     4,-32768,-32768,-32768,     3,-32768,-32768,
-32768,    -7,-32768,     4,-32768,-32768,     5,-32768,-32768,     0,
     6,-32768,     7,     8,-32768,     9,    11,    12,-32768,    13,
    14,-32768,    15,    16,-32768,    17,    18,-32768,    19,    20,
-32768,    10,-32768,    33,    34,-32768
};

static const short extcharsetpgoto[] = {-32768,
-32768,    35,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768,-32768,   -12,-32768,-32768,-32768,-32768,-32768,-32768,-32768,
-32768
};


#define	YYLAST		37


static const short yytable[] = {     6,
     7,    20,    21,     1,    27,    15,    11,    13,    17,    18,
    22,    28,    31,    26,     0,    29,    32,     0,    35,    34,
    38,    37,    39,    53,    42,    41,    45,    44,    48,    47,
    51,    50,    55,    56,     0,     0,     5
};

static const short yycheck[] = {     5,
     6,     8,     9,     3,    12,     4,    11,    11,    11,    11,
     7,    24,    13,    11,    -1,    11,    11,    -1,    11,    13,
    10,    13,    11,    14,    11,    13,    11,    13,    11,    13,
    11,    13,     0,     0,    -1,    -1,     2
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

#define extcharseterrok		(yyerrstatus = 0)
#define extcharsetclearin	(extcharsetchar = YYEMPTY)
#define YYEMPTY		-2
#define YYEOF		0
#define YYACCEPT	return(0)
#define YYABORT 	return(1)
#define YYERROR		goto extcharseterrlab1
/* Like YYERROR except do call extcharseterror.
   This remains here temporarily to ease the
   transition to the new meaning of YYERROR, for GCC.
   Once GCC version 2 has supplanted version 1, this can go.  */
#define YYFAIL		goto extcharseterrlab
#define YYRECOVERING()  (!!yyerrstatus)
#define YYBACKUP(token, value) \
do								\
  if (extcharsetchar == YYEMPTY && yylen == 1)				\
    { extcharsetchar = (token), extcharsetlval = (value);			\
      extcharsetchar1 = YYTRANSLATE (extcharsetchar);				\
      YYPOPSTACK;						\
      goto extcharsetbackup;						\
    }								\
  else								\
    { extcharseterror ("syntax error: cannot back up"); YYERROR; }	\
while (0)

#define YYTERROR	1
#define YYERRCODE	256

#ifndef YYPURE
#define YYLEX		extcharsetlex()
#endif

#ifdef YYPURE
#ifdef YYLSP_NEEDED
#ifdef YYLEX_PARAM
#define YYLEX		extcharsetlex(&extcharsetlval, &yylloc, YYLEX_PARAM)
#else
#define YYLEX		extcharsetlex(&extcharsetlval, &yylloc)
#endif
#else /* not YYLSP_NEEDED */
#ifdef YYLEX_PARAM
#define YYLEX		extcharsetlex(&extcharsetlval, YYLEX_PARAM)
#else
#define YYLEX		extcharsetlex(&extcharsetlval)
#endif
#endif /* not YYLSP_NEEDED */
#endif

/* If nonreentrant, generate the variables here */

#ifndef YYPURE

int	extcharsetchar;			/*  the lookahead symbol		*/
YYSTYPE	extcharsetlval;			/*  the semantic value of the		*/
				/*  lookahead symbol			*/

#ifdef YYLSP_NEEDED
YYLTYPE yylloc;			/*  location data for the lookahead	*/
				/*  symbol				*/
#endif

int extcharsetnerrs;			/*  number of parse errors so far       */
#endif  /* not YYPURE */

#if YYDEBUG != 0
int extcharsetdebug;			/*  nonzero means print parse trace	*/
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
int extcharsetparse (void);
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
   into extcharsetparse.  The argument should have type void *.
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
extcharsetparse(YYPARSE_PARAM)
     YYPARSE_PARAM_DECL
{
  register int extcharsetstate;
  register int yyn;
  register short *extcharsetssp;
  register YYSTYPE *extcharsetvsp;
  int yyerrstatus;	/*  number of tokens to shift before error messages enabled */
  int extcharsetchar1 = 0;		/*  lookahead token as an internal (translated) token number */

  short	extcharsetssa[YYINITDEPTH];	/*  the state stack			*/
  YYSTYPE extcharsetvsa[YYINITDEPTH];	/*  the semantic value stack		*/

  short *extcharsetss = extcharsetssa;		/*  refer to the stacks thru separate pointers */
  YYSTYPE *extcharsetvs = extcharsetvsa;	/*  to allow yyoverflow to reallocate them elsewhere */

#ifdef YYLSP_NEEDED
  YYLTYPE yylsa[YYINITDEPTH];	/*  the location stack			*/
  YYLTYPE *yyls = yylsa;
  YYLTYPE *extcharsetlsp;

#define YYPOPSTACK   (extcharsetvsp--, extcharsetssp--, extcharsetlsp--)
#else
#define YYPOPSTACK   (extcharsetvsp--, extcharsetssp--)
#endif

  int extcharsetstacksize = YYINITDEPTH;

#ifdef YYPURE
  int extcharsetchar;
  YYSTYPE extcharsetlval;
  int extcharsetnerrs;
#ifdef YYLSP_NEEDED
  YYLTYPE yylloc;
#endif
#endif

  YYSTYPE extcharsetval;		/*  the variable used to return		*/
				/*  semantic values from the action	*/
				/*  routines				*/

  int yylen;

#if YYDEBUG != 0
  if (extcharsetdebug)
    fprintf(stderr, "Starting parse\n");
#endif

  extcharsetstate = 0;
  yyerrstatus = 0;
  extcharsetnerrs = 0;
  extcharsetchar = YYEMPTY;		/* Cause a token to be read.  */

  /* Initialize stack pointers.
     Waste one element of value and location stack
     so that they stay on the same level as the state stack.
     The wasted elements are never initialized.  */

  extcharsetssp = extcharsetss - 1;
  extcharsetvsp = extcharsetvs;
#ifdef YYLSP_NEEDED
  extcharsetlsp = yyls;
#endif

/* Push a new state, which is found in  extcharsetstate  .  */
/* In all cases, when you get here, the value and location stacks
   have just been pushed. so pushing a state here evens the stacks.  */
yynewstate:

  *++extcharsetssp = extcharsetstate;

  if (extcharsetssp >= extcharsetss + extcharsetstacksize - 1)
    {
      /* Give user a chance to reallocate the stack */
      /* Use copies of these so that the &'s don't force the real ones into memory. */
      YYSTYPE *extcharsetvs1 = extcharsetvs;
      short *extcharsetss1 = extcharsetss;
#ifdef YYLSP_NEEDED
      YYLTYPE *yyls1 = yyls;
#endif

      /* Get the current used size of the three stacks, in elements.  */
      int size = extcharsetssp - extcharsetss + 1;

#ifdef yyoverflow
      /* Each stack pointer address is followed by the size of
	 the data in use in that stack, in bytes.  */
#ifdef YYLSP_NEEDED
      /* This used to be a conditional around just the two extra args,
	 but that might be undefined if yyoverflow is a macro.  */
      yyoverflow("parser stack overflow",
		 &extcharsetss1, size * sizeof (*extcharsetssp),
		 &extcharsetvs1, size * sizeof (*extcharsetvsp),
		 &yyls1, size * sizeof (*extcharsetlsp),
		 &extcharsetstacksize);
#else
      yyoverflow("parser stack overflow",
		 &extcharsetss1, size * sizeof (*extcharsetssp),
		 &extcharsetvs1, size * sizeof (*extcharsetvsp),
		 &extcharsetstacksize);
#endif

      extcharsetss = extcharsetss1; extcharsetvs = extcharsetvs1;
#ifdef YYLSP_NEEDED
      yyls = yyls1;
#endif
#else /* no yyoverflow */
      /* Extend the stack our own way.  */
      if (extcharsetstacksize >= YYMAXDEPTH)
	{
	  extcharseterror("parser stack overflow");
	  return 2;
	}
      extcharsetstacksize *= 2;
      if (extcharsetstacksize > YYMAXDEPTH)
	extcharsetstacksize = YYMAXDEPTH;
      extcharsetss = (short *) alloca (extcharsetstacksize * sizeof (*extcharsetssp));
      __yy_memcpy ((char *)extcharsetss1, (char *)extcharsetss, size * sizeof (*extcharsetssp));
      extcharsetvs = (YYSTYPE *) alloca (extcharsetstacksize * sizeof (*extcharsetvsp));
      __yy_memcpy ((char *)extcharsetvs1, (char *)extcharsetvs, size * sizeof (*extcharsetvsp));
#ifdef YYLSP_NEEDED
      yyls = (YYLTYPE *) alloca (extcharsetstacksize * sizeof (*extcharsetlsp));
      __yy_memcpy ((char *)yyls1, (char *)yyls, size * sizeof (*extcharsetlsp));
#endif
#endif /* no yyoverflow */

      extcharsetssp = extcharsetss + size - 1;
      extcharsetvsp = extcharsetvs + size - 1;
#ifdef YYLSP_NEEDED
      extcharsetlsp = yyls + size - 1;
#endif

#if YYDEBUG != 0
      if (extcharsetdebug)
	fprintf(stderr, "Stack size increased to %d\n", extcharsetstacksize);
#endif

      if (extcharsetssp >= extcharsetss + extcharsetstacksize - 1)
	YYABORT;
    }

#if YYDEBUG != 0
  if (extcharsetdebug)
    fprintf(stderr, "Entering state %d\n", extcharsetstate);
#endif

  goto extcharsetbackup;
 extcharsetbackup:

/* Do appropriate processing given the current state.  */
/* Read a lookahead token if we need one and don't already have one.  */
/* yyresume: */

  /* First try to decide what to do without reference to lookahead token.  */

  yyn = extcharsetpact[extcharsetstate];
  if (yyn == YYFLAG)
    goto extcharsetdefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* extcharsetchar is either YYEMPTY or YYEOF
     or a valid token in external form.  */

  if (extcharsetchar == YYEMPTY)
    {
#if YYDEBUG != 0
      if (extcharsetdebug)
	fprintf(stderr, "Reading a token: ");
#endif
      extcharsetchar = YYLEX;
    }

  /* Convert token to internal form (in extcharsetchar1) for indexing tables with */

  if (extcharsetchar <= 0)		/* This means end of input. */
    {
      extcharsetchar1 = 0;
      extcharsetchar = YYEOF;		/* Don't call YYLEX any more */

#if YYDEBUG != 0
      if (extcharsetdebug)
	fprintf(stderr, "Now at end of input.\n");
#endif
    }
  else
    {
      extcharsetchar1 = YYTRANSLATE(extcharsetchar);

#if YYDEBUG != 0
      if (extcharsetdebug)
	{
	  fprintf (stderr, "Next token is %d (%s", extcharsetchar, yytname[extcharsetchar1]);
	  /* Give the individual parser a way to print the precise meaning
	     of a token, for further debugging info.  */
#ifdef YYPRINT
	  YYPRINT (stderr, extcharsetchar, extcharsetlval);
#endif
	  fprintf (stderr, ")\n");
	}
#endif
    }

  yyn += extcharsetchar1;
  if (yyn < 0 || yyn > YYLAST || yycheck[yyn] != extcharsetchar1)
    goto extcharsetdefault;

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
	goto extcharseterrlab;
      yyn = -yyn;
      goto yyreduce;
    }
  else if (yyn == 0)
    goto extcharseterrlab;

  if (yyn == YYFINAL)
    YYACCEPT;

  /* Shift the lookahead token.  */

#if YYDEBUG != 0
  if (extcharsetdebug)
    fprintf(stderr, "Shifting token %d (%s), ", extcharsetchar, yytname[extcharsetchar1]);
#endif

  /* Discard the token being shifted unless it is eof.  */
  if (extcharsetchar != YYEOF)
    extcharsetchar = YYEMPTY;

  *++extcharsetvsp = extcharsetlval;
#ifdef YYLSP_NEEDED
  *++extcharsetlsp = yylloc;
#endif

  /* count tokens shifted since error; after three, turn off error status.  */
  if (yyerrstatus) yyerrstatus--;

  extcharsetstate = yyn;
  goto yynewstate;

/* Do the default action for the current state.  */
extcharsetdefault:

  yyn = extcharsetdefact[extcharsetstate];
  if (yyn == 0)
    goto extcharseterrlab;

/* Do a reduction.  yyn is the number of a rule to reduce with.  */
yyreduce:
  yylen = extcharsetr2[yyn];
  if (yylen > 0)
    extcharsetval = extcharsetvsp[1-yylen]; /* implement default value of the action */

#if YYDEBUG != 0
  if (extcharsetdebug)
    {
      int i;

      fprintf (stderr, "Reducing via rule %d (line %d), ",
	       yyn, yyrline[yyn]);

      /* Print the symbols being reduced, and their result.  */
      for (i = yyprhs[yyn]; yyrhs[i] > 0; i++)
	fprintf (stderr, "%s ", yytname[yyrhs[i]]);
      fprintf (stderr, " -> %s\n", yytname[extcharsetr1[yyn]]);
    }
#endif


  switch (yyn) {

case 10:
#line 69 "ext_char_set_parser.y"
{
	;
    break;}
case 11:
#line 74 "ext_char_set_parser.y"
{
	;
    break;}
case 12:
#line 79 "ext_char_set_parser.y"
{
	EXTCHARACTERSET->AddCharacterSet(current_character_set_ptr);
	current_character_set_ptr = NULL;
	;
    break;}
case 13:
#line 86 "ext_char_set_parser.y"
{  
	current_character_set_ptr = new CHARACTER_SET_CLASS(extcharsetvsp[-1].string, false);
	;
    break;}
case 14:
#line 90 "ext_char_set_parser.y"
{  
	current_character_set_ptr = new CHARACTER_SET_CLASS(extcharsetvsp[-1].string, true);
	;
    break;}
case 16:
#line 99 "ext_char_set_parser.y"
{
	if (current_character_set_ptr)
	{
		current_character_set_ptr->AddCodeElement(current_code_element_ptr);
	}
	current_code_element_ptr = NULL;
	;
    break;}
case 17:
#line 107 "ext_char_set_parser.y"
{
	if (current_character_set_ptr)
	{
		current_character_set_ptr->AddCodeElement(current_code_element_ptr);
	}
	current_code_element_ptr = NULL;
	;
    break;}
case 18:
#line 118 "ext_char_set_parser.y"
{
	// create new code element
	current_code_element_ptr = new CODE_ELEMENT_CLASS(extcharsetvsp[-14].string);				

	// and set parameters	
	if (strcmp(extcharsetvsp[-16].string, "MULTI") == 0)
	{
		current_code_element_ptr->SetMultiByte(true);
	}
	else 
	{
		current_code_element_ptr->SetMultiByte(false);
	}

	current_code_element_ptr->SetDefinedTerm(extcharsetvsp[-14].string);
	current_code_element_ptr->SetStdForCodeExt(extcharsetvsp[-12].string);
	current_code_element_ptr->SetEscSequenceCRFormat(extcharsetvsp[-9].string);
	current_code_element_ptr->SetISORegNr(extcharsetvsp[-7].string);
	current_code_element_ptr->SetNrChars(extcharsetvsp[-5].string);
	current_code_element_ptr->SetCodeElementName(extcharsetvsp[-3].string);
	current_code_element_ptr->SetCharacterSet(extcharsetvsp[-1].string);
	;
    break;}
}
   /* the action file gets copied in in place of this dollarsign */
#line 487 "bison.simple"

  extcharsetvsp -= yylen;
  extcharsetssp -= yylen;
#ifdef YYLSP_NEEDED
  extcharsetlsp -= yylen;
#endif

#if YYDEBUG != 0
  if (extcharsetdebug)
    {
      short *ssp1 = extcharsetss - 1;
      fprintf (stderr, "state stack now");
      while (ssp1 != extcharsetssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

  *++extcharsetvsp = extcharsetval;

#ifdef YYLSP_NEEDED
  extcharsetlsp++;
  if (yylen == 0)
    {
      extcharsetlsp->first_line = yylloc.first_line;
      extcharsetlsp->first_column = yylloc.first_column;
      extcharsetlsp->last_line = (extcharsetlsp-1)->last_line;
      extcharsetlsp->last_column = (extcharsetlsp-1)->last_column;
      extcharsetlsp->text = 0;
    }
  else
    {
      extcharsetlsp->last_line = (extcharsetlsp+yylen-1)->last_line;
      extcharsetlsp->last_column = (extcharsetlsp+yylen-1)->last_column;
    }
#endif

  /* Now "shift" the result of the reduction.
     Determine what state that goes to,
     based on the state we popped back to
     and the rule number reduced by.  */

  yyn = extcharsetr1[yyn];

  extcharsetstate = extcharsetpgoto[yyn - YYNTBASE] + *extcharsetssp;
  if (extcharsetstate >= 0 && extcharsetstate <= YYLAST && yycheck[extcharsetstate] == *extcharsetssp)
    extcharsetstate = yytable[extcharsetstate];
  else
    extcharsetstate = extcharsetdefgoto[yyn - YYNTBASE];

  goto yynewstate;

extcharseterrlab:   /* here on detecting error */

  if (! yyerrstatus)
    /* If not already recovering from an error, report this error.  */
    {
      ++extcharsetnerrs;

#ifdef YYERROR_VERBOSE
      yyn = extcharsetpact[extcharsetstate];

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
	      extcharseterror(msg);
	      free(msg);
	    }
	  else
	    extcharseterror ("parse error; also virtual memory exceeded");
	}
      else
#endif /* YYERROR_VERBOSE */
	extcharseterror("parse error");
    }

  goto extcharseterrlab1;
extcharseterrlab1:   /* here on error raised explicitly by an action */

  if (yyerrstatus == 3)
    {
      /* if just tried and failed to reuse lookahead token after an error, discard it.  */

      /* return failure if at end of input */
      if (extcharsetchar == YYEOF)
	YYABORT;

#if YYDEBUG != 0
      if (extcharsetdebug)
	fprintf(stderr, "Discarding token %d (%s).\n", extcharsetchar, yytname[extcharsetchar1]);
#endif

      extcharsetchar = YYEMPTY;
    }

  /* Else will try to reuse lookahead token
     after shifting the error token.  */

  yyerrstatus = 3;		/* Each real token shifted decrements this */

  goto yyerrhandle;

yyerrdefault:  /* current state does not do anything special for the error token. */

#if 0
  /* This is wrong; only states that explicitly want error tokens
     should shift them.  */
  yyn = extcharsetdefact[extcharsetstate];  /* If its default is to accept any token, ok.  Otherwise pop it.*/
  if (yyn) goto extcharsetdefault;
#endif

yyerrpop:   /* pop the current state because it cannot handle the error token */

  if (extcharsetssp == extcharsetss) YYABORT;
  extcharsetvsp--;
  extcharsetstate = *--extcharsetssp;
#ifdef YYLSP_NEEDED
  extcharsetlsp--;
#endif

#if YYDEBUG != 0
  if (extcharsetdebug)
    {
      short *ssp1 = extcharsetss - 1;
      fprintf (stderr, "Error: state stack now");
      while (ssp1 != extcharsetssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

yyerrhandle:

  yyn = extcharsetpact[extcharsetstate];
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
  if (extcharsetdebug)
    fprintf(stderr, "Shifting error token, ");
#endif

  *++extcharsetvsp = extcharsetlval;
#ifdef YYLSP_NEEDED
  *++extcharsetlsp = yylloc;
#endif

  extcharsetstate = yyn;
  goto yynewstate;
}
#line 165 "ext_char_set_parser.y"

