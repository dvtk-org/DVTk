
/*  A Bison parser, made from imagedisplay_parse.y with Bison version GNU Bison version 1.24
  */

#define YYBISON 1  /* Identify Bison output.  */

#define	T_SYSTEM	258
#define	T_DEFINE	259
#define	T_ENDDEFINE	260
#define	T_IMAGEDISPLAYFORMAT	261
#define	T_ANNOTATIONDISPLAYFORMATID	262
#define	STRING	263
#define	INTEGER	264

#line 1 "imagedisplay_parse.y"

// Part of Dvtk Libraries - Internal Native Library Code
// Copyright � 2001-2006
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


#line 23 "imagedisplay_parse.y"
typedef union {
	int			integer;
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



#define	YYFINAL		27
#define	YYFLAG		-32768
#define	YYNTBASE	10

#define YYTRANSLATE(x) ((unsigned)(x) <= 264 ? yytranslate[x] : 21)

static const char yytranslate[] = {     0,
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
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     2,     2,     2,     2,     2,
     2,     2,     2,     2,     2,     1,     2,     3,     4,     5,
     6,     7,     8,     9
};

#if YYDEBUG != 0
static const short yyprhs[] = {     0,
     0,     2,     4,     7,     9,    11,    13,    17,    19,    21,
    25,    28,    31,    35
};

static const short yyrhs[] = {    11,
     0,    12,     0,    11,    12,     0,    13,     0,    19,     0,
    20,     0,    14,    16,    15,     0,     4,     0,     5,     0,
     3,    17,    18,     0,     8,     8,     0,     8,     8,     0,
     6,     8,     9,     0,     7,     8,     9,     0
};

#endif

#if YYDEBUG != 0
static const short yyrline[] = { 0,
    35,    38,    39,    42,    43,    44,    47,    50,    53,    56,
    59,    64,    69,    75
};

static const char * const yytname[] = {   "$","error","$undefined.","T_SYSTEM",
"T_DEFINE","T_ENDDEFINE","T_IMAGEDISPLAYFORMAT","T_ANNOTATIONDISPLAYFORMATID",
"STRING","INTEGER","DefinitionGrammar","DefinitionComponents","DefinitionChoice",
"Definition","BeginDefine","EndDefine","SystemDef","SystemDefinition","AEDefinition",
"ImageDisplayFormatDef","AnnotationDisplayFormatIDDef",""
};
#endif

static const short imagedisplayr1[] = {     0,
    10,    11,    11,    12,    12,    12,    13,    14,    15,    16,
    17,    18,    19,    20
};

static const short imagedisplayr2[] = {     0,
     1,     1,     2,     1,     1,     1,     3,     1,     1,     3,
     2,     2,     3,     3
};

static const short imagedisplaydefact[] = {     0,
     8,     0,     0,     1,     2,     4,     0,     5,     6,     0,
     0,     3,     0,     0,    13,    14,     0,     0,     9,     7,
    11,     0,    10,    12,     0,     0,     0
};

static const short imagedisplaydefgoto[] = {    25,
     4,     5,     6,     7,    20,    14,    18,    23,     8,     9
};

static const short imagedisplaypact[] = {    -4,
-32768,    -7,    -3,    -4,-32768,-32768,     1,-32768,-32768,    -2,
    -1,-32768,     2,     4,-32768,-32768,     3,     5,-32768,-32768,
-32768,     6,-32768,-32768,    12,    15,-32768
};

static const short imagedisplaypgoto[] = {-32768,
-32768,    13,-32768,-32768,-32768,-32768,-32768,-32768,-32768,-32768
};


#define	YYLAST		17


static const short yytable[] = {     1,
    10,     2,     3,    13,    11,     0,    15,    16,    19,    17,
    21,    26,    22,    24,    27,     0,    12
};

static const short yycheck[] = {     4,
     8,     6,     7,     3,     8,    -1,     9,     9,     5,     8,
     8,     0,     8,     8,     0,    -1,     4
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

#define imagedisplayerrok		(yyerrstatus = 0)
#define imagedisplayclearin	(imagedisplaychar = YYEMPTY)
#define YYEMPTY		-2
#define YYEOF		0
#define YYACCEPT	return(0)
#define YYABORT 	return(1)
#define YYERROR		goto imagedisplayerrlab1
/* Like YYERROR except do call imagedisplayerror.
   This remains here temporarily to ease the
   transition to the new meaning of YYERROR, for GCC.
   Once GCC version 2 has supplanted version 1, this can go.  */
#define YYFAIL		goto imagedisplayerrlab
#define YYRECOVERING()  (!!yyerrstatus)
#define YYBACKUP(token, value) \
do								\
  if (imagedisplaychar == YYEMPTY && yylen == 1)				\
    { imagedisplaychar = (token), imagedisplaylval = (value);			\
      imagedisplaychar1 = YYTRANSLATE (imagedisplaychar);				\
      YYPOPSTACK;						\
      goto imagedisplaybackup;						\
    }								\
  else								\
    { imagedisplayerror ("syntax error: cannot back up"); YYERROR; }	\
while (0)

#define YYTERROR	1
#define YYERRCODE	256

#ifndef YYPURE
#define YYLEX		imagedisplaylex()
#endif

#ifdef YYPURE
#ifdef YYLSP_NEEDED
#ifdef YYLEX_PARAM
#define YYLEX		imagedisplaylex(&imagedisplaylval, &yylloc, YYLEX_PARAM)
#else
#define YYLEX		imagedisplaylex(&imagedisplaylval, &yylloc)
#endif
#else /* not YYLSP_NEEDED */
#ifdef YYLEX_PARAM
#define YYLEX		imagedisplaylex(&imagedisplaylval, YYLEX_PARAM)
#else
#define YYLEX		imagedisplaylex(&imagedisplaylval)
#endif
#endif /* not YYLSP_NEEDED */
#endif

/* If nonreentrant, generate the variables here */

#ifndef YYPURE

int	imagedisplaychar;			/*  the lookahead symbol		*/
YYSTYPE	imagedisplaylval;			/*  the semantic value of the		*/
				/*  lookahead symbol			*/

#ifdef YYLSP_NEEDED
YYLTYPE yylloc;			/*  location data for the lookahead	*/
				/*  symbol				*/
#endif

int imagedisplaynerrs;			/*  number of parse errors so far       */
#endif  /* not YYPURE */

#if YYDEBUG != 0
int imagedisplaydebug;			/*  nonzero means print parse trace	*/
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
int imagedisplayparse (void);
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
   into imagedisplayparse.  The argument should have type void *.
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
imagedisplayparse(YYPARSE_PARAM)
     YYPARSE_PARAM_DECL
{
  register int imagedisplaystate;
  register int yyn;
  register short *imagedisplayssp;
  register YYSTYPE *imagedisplayvsp;
  int yyerrstatus;	/*  number of tokens to shift before error messages enabled */
  int imagedisplaychar1 = 0;		/*  lookahead token as an internal (translated) token number */

  short	imagedisplayssa[YYINITDEPTH];	/*  the state stack			*/
  YYSTYPE imagedisplayvsa[YYINITDEPTH];	/*  the semantic value stack		*/

  short *imagedisplayss = imagedisplayssa;		/*  refer to the stacks thru separate pointers */
  YYSTYPE *imagedisplayvs = imagedisplayvsa;	/*  to allow yyoverflow to reallocate them elsewhere */

#ifdef YYLSP_NEEDED
  YYLTYPE yylsa[YYINITDEPTH];	/*  the location stack			*/
  YYLTYPE *yyls = yylsa;
  YYLTYPE *imagedisplaylsp;

#define YYPOPSTACK   (imagedisplayvsp--, imagedisplayssp--, imagedisplaylsp--)
#else
#define YYPOPSTACK   (imagedisplayvsp--, imagedisplayssp--)
#endif

  int imagedisplaystacksize = YYINITDEPTH;

#ifdef YYPURE
  int imagedisplaychar;
  YYSTYPE imagedisplaylval;
  int imagedisplaynerrs;
#ifdef YYLSP_NEEDED
  YYLTYPE yylloc;
#endif
#endif

  YYSTYPE imagedisplayval;		/*  the variable used to return		*/
				/*  semantic values from the action	*/
				/*  routines				*/

  int yylen;

#if YYDEBUG != 0
  if (imagedisplaydebug)
    fprintf(stderr, "Starting parse\n");
#endif

  imagedisplaystate = 0;
  yyerrstatus = 0;
  imagedisplaynerrs = 0;
  imagedisplaychar = YYEMPTY;		/* Cause a token to be read.  */

  /* Initialize stack pointers.
     Waste one element of value and location stack
     so that they stay on the same level as the state stack.
     The wasted elements are never initialized.  */

  imagedisplayssp = imagedisplayss - 1;
  imagedisplayvsp = imagedisplayvs;
#ifdef YYLSP_NEEDED
  imagedisplaylsp = yyls;
#endif

/* Push a new state, which is found in  imagedisplaystate  .  */
/* In all cases, when you get here, the value and location stacks
   have just been pushed. so pushing a state here evens the stacks.  */
yynewstate:

  *++imagedisplayssp = imagedisplaystate;

  if (imagedisplayssp >= imagedisplayss + imagedisplaystacksize - 1)
    {
      /* Give user a chance to reallocate the stack */
      /* Use copies of these so that the &'s don't force the real ones into memory. */
      YYSTYPE *imagedisplayvs1 = imagedisplayvs;
      short *imagedisplayss1 = imagedisplayss;
#ifdef YYLSP_NEEDED
      YYLTYPE *yyls1 = yyls;
#endif

      /* Get the current used size of the three stacks, in elements.  */
      int size = imagedisplayssp - imagedisplayss + 1;

#ifdef yyoverflow
      /* Each stack pointer address is followed by the size of
	 the data in use in that stack, in bytes.  */
#ifdef YYLSP_NEEDED
      /* This used to be a conditional around just the two extra args,
	 but that might be undefined if yyoverflow is a macro.  */
      yyoverflow("parser stack overflow",
		 &imagedisplayss1, size * sizeof (*imagedisplayssp),
		 &imagedisplayvs1, size * sizeof (*imagedisplayvsp),
		 &yyls1, size * sizeof (*imagedisplaylsp),
		 &imagedisplaystacksize);
#else
      yyoverflow("parser stack overflow",
		 &imagedisplayss1, size * sizeof (*imagedisplayssp),
		 &imagedisplayvs1, size * sizeof (*imagedisplayvsp),
		 &imagedisplaystacksize);
#endif

      imagedisplayss = imagedisplayss1; imagedisplayvs = imagedisplayvs1;
#ifdef YYLSP_NEEDED
      yyls = yyls1;
#endif
#else /* no yyoverflow */
      /* Extend the stack our own way.  */
      if (imagedisplaystacksize >= YYMAXDEPTH)
	{
	  imagedisplayerror("parser stack overflow");
	  return 2;
	}
      imagedisplaystacksize *= 2;
      if (imagedisplaystacksize > YYMAXDEPTH)
	imagedisplaystacksize = YYMAXDEPTH;
      imagedisplayss = (short *) alloca (imagedisplaystacksize * sizeof (*imagedisplayssp));
      __yy_memcpy ((char *)imagedisplayss1, (char *)imagedisplayss, size * sizeof (*imagedisplayssp));
      imagedisplayvs = (YYSTYPE *) alloca (imagedisplaystacksize * sizeof (*imagedisplayvsp));
      __yy_memcpy ((char *)imagedisplayvs1, (char *)imagedisplayvs, size * sizeof (*imagedisplayvsp));
#ifdef YYLSP_NEEDED
      yyls = (YYLTYPE *) alloca (imagedisplaystacksize * sizeof (*imagedisplaylsp));
      __yy_memcpy ((char *)yyls1, (char *)yyls, size * sizeof (*imagedisplaylsp));
#endif
#endif /* no yyoverflow */

      imagedisplayssp = imagedisplayss + size - 1;
      imagedisplayvsp = imagedisplayvs + size - 1;
#ifdef YYLSP_NEEDED
      imagedisplaylsp = yyls + size - 1;
#endif

#if YYDEBUG != 0
      if (imagedisplaydebug)
	fprintf(stderr, "Stack size increased to %d\n", imagedisplaystacksize);
#endif

      if (imagedisplayssp >= imagedisplayss + imagedisplaystacksize - 1)
	YYABORT;
    }

#if YYDEBUG != 0
  if (imagedisplaydebug)
    fprintf(stderr, "Entering state %d\n", imagedisplaystate);
#endif

  goto imagedisplaybackup;
 imagedisplaybackup:

/* Do appropriate processing given the current state.  */
/* Read a lookahead token if we need one and don't already have one.  */
/* yyresume: */

  /* First try to decide what to do without reference to lookahead token.  */

  yyn = imagedisplaypact[imagedisplaystate];
  if (yyn == YYFLAG)
    goto imagedisplaydefault;

  /* Not known => get a lookahead token if don't already have one.  */

  /* imagedisplaychar is either YYEMPTY or YYEOF
     or a valid token in external form.  */

  if (imagedisplaychar == YYEMPTY)
    {
#if YYDEBUG != 0
      if (imagedisplaydebug)
	fprintf(stderr, "Reading a token: ");
#endif
      imagedisplaychar = YYLEX;
    }

  /* Convert token to internal form (in imagedisplaychar1) for indexing tables with */

  if (imagedisplaychar <= 0)		/* This means end of input. */
    {
      imagedisplaychar1 = 0;
      imagedisplaychar = YYEOF;		/* Don't call YYLEX any more */

#if YYDEBUG != 0
      if (imagedisplaydebug)
	fprintf(stderr, "Now at end of input.\n");
#endif
    }
  else
    {
      imagedisplaychar1 = YYTRANSLATE(imagedisplaychar);

#if YYDEBUG != 0
      if (imagedisplaydebug)
	{
	  fprintf (stderr, "Next token is %d (%s", imagedisplaychar, yytname[imagedisplaychar1]);
	  /* Give the individual parser a way to print the precise meaning
	     of a token, for further debugging info.  */
#ifdef YYPRINT
	  YYPRINT (stderr, imagedisplaychar, imagedisplaylval);
#endif
	  fprintf (stderr, ")\n");
	}
#endif
    }

  yyn += imagedisplaychar1;
  if (yyn < 0 || yyn > YYLAST || yycheck[yyn] != imagedisplaychar1)
    goto imagedisplaydefault;

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
	goto imagedisplayerrlab;
      yyn = -yyn;
      goto yyreduce;
    }
  else if (yyn == 0)
    goto imagedisplayerrlab;

  if (yyn == YYFINAL)
    YYACCEPT;

  /* Shift the lookahead token.  */

#if YYDEBUG != 0
  if (imagedisplaydebug)
    fprintf(stderr, "Shifting token %d (%s), ", imagedisplaychar, yytname[imagedisplaychar1]);
#endif

  /* Discard the token being shifted unless it is eof.  */
  if (imagedisplaychar != YYEOF)
    imagedisplaychar = YYEMPTY;

  *++imagedisplayvsp = imagedisplaylval;
#ifdef YYLSP_NEEDED
  *++imagedisplaylsp = yylloc;
#endif

  /* count tokens shifted since error; after three, turn off error status.  */
  if (yyerrstatus) yyerrstatus--;

  imagedisplaystate = yyn;
  goto yynewstate;

/* Do the default action for the current state.  */
imagedisplaydefault:

  yyn = imagedisplaydefact[imagedisplaystate];
  if (yyn == 0)
    goto imagedisplayerrlab;

/* Do a reduction.  yyn is the number of a rule to reduce with.  */
yyreduce:
  yylen = imagedisplayr2[yyn];
  if (yylen > 0)
    imagedisplayval = imagedisplayvsp[1-yylen]; /* implement default value of the action */

#if YYDEBUG != 0
  if (imagedisplaydebug)
    {
      int i;

      fprintf (stderr, "Reducing via rule %d (line %d), ",
	       yyn, yyrline[yyn]);

      /* Print the symbols being reduced, and their result.  */
      for (i = yyprhs[yyn]; yyrhs[i] > 0; i++)
	fprintf (stderr, "%s ", yytname[yyrhs[i]]);
      fprintf (stderr, " -> %s\n", yytname[imagedisplayr1[yyn]]);
    }
#endif


  switch (yyn) {

case 11:
#line 60 "imagedisplay_parse.y"
{
	;
    break;}
case 12:
#line 65 "imagedisplay_parse.y"
{
	;
    break;}
case 13:
#line 70 "imagedisplay_parse.y"
{
		MYPRINTER->addImageDisplayFormat(imagedisplayvsp[-1].string, imagedisplayvsp[0].integer);
	;
    break;}
case 14:
#line 76 "imagedisplay_parse.y"
{
		MYPRINTER->addAnnotationDisplayFormatId(imagedisplayvsp[-1].string, imagedisplayvsp[0].integer);
	;
    break;}
}
   /* the action file gets copied in in place of this dollarsign */
#line 487 "bison.simple"

  imagedisplayvsp -= yylen;
  imagedisplayssp -= yylen;
#ifdef YYLSP_NEEDED
  imagedisplaylsp -= yylen;
#endif

#if YYDEBUG != 0
  if (imagedisplaydebug)
    {
      short *ssp1 = imagedisplayss - 1;
      fprintf (stderr, "state stack now");
      while (ssp1 != imagedisplayssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

  *++imagedisplayvsp = imagedisplayval;

#ifdef YYLSP_NEEDED
  imagedisplaylsp++;
  if (yylen == 0)
    {
      imagedisplaylsp->first_line = yylloc.first_line;
      imagedisplaylsp->first_column = yylloc.first_column;
      imagedisplaylsp->last_line = (imagedisplaylsp-1)->last_line;
      imagedisplaylsp->last_column = (imagedisplaylsp-1)->last_column;
      imagedisplaylsp->text = 0;
    }
  else
    {
      imagedisplaylsp->last_line = (imagedisplaylsp+yylen-1)->last_line;
      imagedisplaylsp->last_column = (imagedisplaylsp+yylen-1)->last_column;
    }
#endif

  /* Now "shift" the result of the reduction.
     Determine what state that goes to,
     based on the state we popped back to
     and the rule number reduced by.  */

  yyn = imagedisplayr1[yyn];

  imagedisplaystate = imagedisplaypgoto[yyn - YYNTBASE] + *imagedisplayssp;
  if (imagedisplaystate >= 0 && imagedisplaystate <= YYLAST && yycheck[imagedisplaystate] == *imagedisplayssp)
    imagedisplaystate = yytable[imagedisplaystate];
  else
    imagedisplaystate = imagedisplaydefgoto[yyn - YYNTBASE];

  goto yynewstate;

imagedisplayerrlab:   /* here on detecting error */

  if (! yyerrstatus)
    /* If not already recovering from an error, report this error.  */
    {
      ++imagedisplaynerrs;

#ifdef YYERROR_VERBOSE
      yyn = imagedisplaypact[imagedisplaystate];

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
	      imagedisplayerror(msg);
	      free(msg);
	    }
	  else
	    imagedisplayerror ("parse error; also virtual memory exceeded");
	}
      else
#endif /* YYERROR_VERBOSE */
	imagedisplayerror("parse error");
    }

  goto imagedisplayerrlab1;
imagedisplayerrlab1:   /* here on error raised explicitly by an action */

  if (yyerrstatus == 3)
    {
      /* if just tried and failed to reuse lookahead token after an error, discard it.  */

      /* return failure if at end of input */
      if (imagedisplaychar == YYEOF)
	YYABORT;

#if YYDEBUG != 0
      if (imagedisplaydebug)
	fprintf(stderr, "Discarding token %d (%s).\n", imagedisplaychar, yytname[imagedisplaychar1]);
#endif

      imagedisplaychar = YYEMPTY;
    }

  /* Else will try to reuse lookahead token
     after shifting the error token.  */

  yyerrstatus = 3;		/* Each real token shifted decrements this */

  goto yyerrhandle;

yyerrdefault:  /* current state does not do anything special for the error token. */

#if 0
  /* This is wrong; only states that explicitly want error tokens
     should shift them.  */
  yyn = imagedisplaydefact[imagedisplaystate];  /* If its default is to accept any token, ok.  Otherwise pop it.*/
  if (yyn) goto imagedisplaydefault;
#endif

yyerrpop:   /* pop the current state because it cannot handle the error token */

  if (imagedisplayssp == imagedisplayss) YYABORT;
  imagedisplayvsp--;
  imagedisplaystate = *--imagedisplayssp;
#ifdef YYLSP_NEEDED
  imagedisplaylsp--;
#endif

#if YYDEBUG != 0
  if (imagedisplaydebug)
    {
      short *ssp1 = imagedisplayss - 1;
      fprintf (stderr, "Error: state stack now");
      while (ssp1 != imagedisplayssp)
	fprintf (stderr, " %d", *++ssp1);
      fprintf (stderr, "\n");
    }
#endif

yyerrhandle:

  yyn = imagedisplaypact[imagedisplaystate];
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
  if (imagedisplaydebug)
    fprintf(stderr, "Shifting error token, ");
#endif

  *++imagedisplayvsp = imagedisplaylval;
#ifdef YYLSP_NEEDED
  *++imagedisplaylsp = yylloc;
#endif

  imagedisplaystate = yyn;
  goto yynewstate;
}
#line 80 "imagedisplay_parse.y"

