// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright © 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>

#ifdef _WINDOWS
#pragma warning (disable : 4786)
#endif

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "scriptfile.h"
#include "Isession.h"			// Test Session component interface
#include <stdio.h>
#include <io.h>		// for _access()

//*****************************************************************************
//  EXTERNAL REFERENCES
//*****************************************************************************
extern char				scriptCurrentFilename[_MAX_PATH];
extern FILE*			scriptin;
extern int				scriptlineno;
extern long				scriptCurrentFileOffset;
extern long				scriptCurrentLineNo;
extern int				scriptparse(void);
extern void				scriptrestart(FILE*);
SCRIPT_SESSION_CLASS*   scriptSession_ptr;
extern bool             scriptIsNativeVts;
extern bool             scriptParseOnly;
	
// workaround - 2nd parser instance
extern char				script1CurrentFilename[_MAX_PATH];
extern FILE*			script1in;
extern int				script1lineno;
extern long				script1CurrentFileOffset;
extern long				script1CurrentLineNo;
extern int				script1parse(void);
extern void				script1restart(FILE*);
SCRIPT_SESSION_CLASS*   script1Session_ptr;
extern bool             script1IsNativeVts;
extern bool             script1ParseOnly;

// workaround - 3rd parser instance
extern char				script2CurrentFilename[_MAX_PATH];
extern FILE*			script2in;
extern int				script2lineno;
extern long				script2CurrentFileOffset;
extern long				script2CurrentLineNo;
extern int				script2parse(void);
extern void				script2restart(FILE*);
SCRIPT_SESSION_CLASS*   script2Session_ptr;
extern bool             script2IsNativeVts;
extern bool             script2ParseOnly;

// workaround - 4th parser instance
extern char				script3CurrentFilename[_MAX_PATH];
extern FILE*			script3in;
extern int				script3lineno;
extern long				script3CurrentFileOffset;
extern long				script3CurrentLineNo;
extern int				script3parse(void);
extern void				script3restart(FILE*);
SCRIPT_SESSION_CLASS*	script3Session_ptr;
extern bool             script3IsNativeVts;
extern bool             script3ParseOnly;

// workaround - 5th parser instance
extern char				script4CurrentFilename[_MAX_PATH];
extern FILE*			script4in;
extern int				script4lineno;
extern long				script4CurrentFileOffset;
extern long				script4CurrentLineNo;
extern int				script4parse(void);
extern void				script4restart(FILE*);
SCRIPT_SESSION_CLASS*	script4Session_ptr;
extern bool             script4IsNativeVts;
extern bool             script4ParseOnly;


//>>===========================================================================
//
// initialise PARSER_INSTANCE_CLASS static pointer
//<<===========================================================================
PARSER_INSTANCE_CLASS *PARSER_INSTANCE_CLASS::instanceM_ptr = NULL;

//>>===========================================================================

void PARSER_INSTANCE_CLASS::initialise()

//  DESCRIPTION     : Initialise the parser instances.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise the free parsers
	for (int i = 0; i < MAX_PARSERS; i++)
	{
		freeM[i] = true;
	}
}

//>>===========================================================================

PARSER_INSTANCE_CLASS::PARSER_INSTANCE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	initialise();
}

//>>===========================================================================

PARSER_INSTANCE_CLASS *PARSER_INSTANCE_CLASS::instance()

//  DESCRIPTION     : Get Parser instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// is this the first time ?
	if (instanceM_ptr == NULL) 
	{
		instanceM_ptr = new PARSER_INSTANCE_CLASS();
	}

	return instanceM_ptr;
}

//>>===========================================================================

void PARSER_INSTANCE_CLASS::cleanup()

//  DESCRIPTION     : Cleanup instance.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// re-intialise
	initialise();
}

//>>===========================================================================

int PARSER_INSTANCE_CLASS::allocateParser()

//  DESCRIPTION     : Allocate the first free parser.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int index = -1;

	// allocate the first free parser
	for (int i = 0; i < MAX_PARSERS; i++)
	{
		if (freeM[i]) 
		{
			// allocate the parser
			freeM[i] = false;
			index = i;
			break;
		}
	}

	// return the parser index - maybe -1
	return index;
}

//>>===========================================================================

void PARSER_INSTANCE_CLASS::freeParser(int index)

//  DESCRIPTION     : Free up the indexed parser.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// free up the given parser
	if (index < MAX_PARSERS)
	{
		freeM[index] = true;
	}
}

//>>===========================================================================

bool PARSER_INSTANCE_CLASS::isParserAvailable()

//  DESCRIPTION     : Check if a parser is available.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check for parser availability
	for (int i = 0; i < MAX_PARSERS; i++)
	{
		if (freeM[i]) 
		{
			// at least one free
			return true;
		}
	}

	// none left
	return false;
}


//>>===========================================================================

DICOM_SCRIPT_CLASS::DICOM_SCRIPT_CLASS(SCRIPT_SESSION_CLASS *session_ptr, string filename)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// initialise class members
	// - allocate a parser from the pool
	parserInstanceM = MYPARSER->allocateParser();

	// save the session pointer
	sessionM_ptr = session_ptr;

	switch (parserInstanceM)
	{
		case 0:
			::scriptSession_ptr = sessionM_ptr;
			strcpy(scriptCurrentFilename, filename.c_str());
			scriptIsNativeVts = false;
		break;
		case 1:
			::script1Session_ptr = sessionM_ptr;
			strcpy(script1CurrentFilename, filename.c_str());
			script1IsNativeVts = false;
		break;
		case 2:
			::script2Session_ptr = sessionM_ptr;
			strcpy(script2CurrentFilename, filename.c_str());
			script2IsNativeVts = false;
		break;
		case 3:
			::script3Session_ptr = sessionM_ptr;
			strcpy(script3CurrentFilename, filename.c_str());
			script3IsNativeVts = false;
		break;
		case 4:
			::script4Session_ptr = sessionM_ptr;
			strcpy(script4CurrentFilename, filename.c_str());
			script4IsNativeVts = false;
		break;
		default:
		break;
	}

	// save the logger
	loggerM_ptr = session_ptr->getLogger();

	// save the script file pathname
	pathnameM = sessionM_ptr->getDicomScriptRoot();

	// save the dicom script name
	filenameM = filename;

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "DICOMScript: %s", filenameM.c_str());
	}

	onlyParseFileM = false;
	inSuperScriptM = false;

	fdM_ptr = NULL;
}

//>>===========================================================================

DICOM_SCRIPT_CLASS::~DICOM_SCRIPT_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// close open DICOM Script
	close();

	// free the parser
	MYPARSER->freeParser(parserInstanceM);
}

//>>===========================================================================

bool DICOM_SCRIPT_CLASS::execute()

//  DESCRIPTION     : Execute script file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	int error = 0;

	// check if file exists
	if (!open()) return false;

	if ((!inSuperScriptM) &&
		(loggerM_ptr)) 
	{
		if (onlyParseFileM)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Parsing DICOMScript: %s", filenameM.c_str());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT_NAME, 1, "%s", filenameM.c_str());
			loggerM_ptr->text(LOG_INFO, 1, "Executing DICOMScript: %s", filenameM.c_str());
		}
	}

	UINT32 oldlogmask = LOG_NONE;
	switch (parserInstanceM)
	{
		case 0:
			// set parser to read a DICOMScript
			scriptlineno = 1;
			scriptCurrentFileOffset = 0;
			scriptCurrentLineNo = 0;
			scriptin = fdM_ptr;

			// (re)start the lexer
			scriptrestart(scriptin);	

			// call YACC parser
			// First parse only to check for syntax errors
			scriptParseOnly = true;
			
			//set logmask such that only errors are displayed
			if (loggerM_ptr)
			{
				oldlogmask = loggerM_ptr->getLogMask();
				loggerM_ptr->setLogMask(LOG_ERROR);
			}
			error = scriptparse();

			//reset logmask
			if (loggerM_ptr)
			{
				loggerM_ptr->setLogMask(oldlogmask);
			}

			// If no errors were encountered execute script
			if ( !onlyParseFileM && !error )
			{
				// rewind file & restart the lexer
                rewind(scriptin);
				scriptrestart(scriptin);	
				scriptParseOnly = false;
                error = scriptparse();
			}
		break;
		case 1:
			script1lineno = 1;
			script1CurrentFileOffset = 0;
			script1CurrentLineNo = 0;
			script1in = fdM_ptr;
			script1restart(script1in);
			
			// First parse only to check for syntax errors
			script1ParseOnly = true;
			
			//set logmask such that only errors are displayed
			if (loggerM_ptr)
			{
				oldlogmask = loggerM_ptr->getLogMask();
				loggerM_ptr->setLogMask(LOG_ERROR);
			}
			error = script1parse();

			//reset logmask
			if (loggerM_ptr)
			{
				loggerM_ptr->setLogMask(oldlogmask);
			}

			// If no errors were encountered execute script
			if ( !onlyParseFileM && !error )
			{
				// rewind file & restart the lexer
                rewind(script1in);
				script1restart(script1in);	
				script1ParseOnly = false;
                error = script1parse();
			}
		break;
		case 2:
			script2lineno = 1;
			script2CurrentFileOffset = 0;
			script2CurrentLineNo = 0;
			script2in = fdM_ptr;
			script2restart(script2in);
			// First parse only to check for syntax errors
			script2ParseOnly = true;
			
			//set logmask such that only errors are displayed
			if (loggerM_ptr)
			{
				oldlogmask = loggerM_ptr->getLogMask();
				loggerM_ptr->setLogMask(LOG_ERROR);
			}
			error = script2parse();

			//reset logmask
			if (loggerM_ptr)
			{
				loggerM_ptr->setLogMask(oldlogmask);
			}

			// If no errors were encountered execute script
			if ( !onlyParseFileM && !error )
			{
				// rewind file & restart the lexer
                rewind(script2in);
				script2restart(script2in);	
				script2ParseOnly = false;
                error = script2parse();
			}
		break;
		case 3:
			script3lineno = 1;
			script3CurrentFileOffset = 0;
			script3CurrentLineNo = 0;
			script3in = fdM_ptr;
			script3restart(script3in);
			// First parse only to check for syntax errors
			script3ParseOnly = true;
			
			//set logmask such that only errors are displayed
			if (loggerM_ptr)
			{
				oldlogmask = loggerM_ptr->getLogMask();
				loggerM_ptr->setLogMask(LOG_ERROR);
			}
			error = script3parse();

			//reset logmask
			if (loggerM_ptr)
			{
				loggerM_ptr->setLogMask(oldlogmask);
			}

			// If no errors were encountered execute script
			if ( !onlyParseFileM && !error )
			{
				// rewind file & restart the lexer
                rewind(script3in);
				script3restart(script3in);	
				script3ParseOnly = false;
                error = script3parse();
			}
		break;
		case 4:
			script4lineno = 1;
			script4CurrentFileOffset = 0;
			script4CurrentLineNo = 0;
			script4in = fdM_ptr;
			script4restart(script4in);
			// First parse only to check for syntax errors
			script4ParseOnly = true;
			
			//set logmask such that only errors are displayed
			if (loggerM_ptr)
			{
				oldlogmask = loggerM_ptr->getLogMask();
				loggerM_ptr->setLogMask(LOG_ERROR);
			}
			error = script4parse();

			//reset logmask
			if (loggerM_ptr)
			{
				loggerM_ptr->setLogMask(oldlogmask);
			}

			// If no errors were encountered execute script
			if ( !onlyParseFileM && !error )
			{
				// rewind file & restart the lexer
                rewind(script4in);
				script4restart(script4in);	
				script4ParseOnly = false;
                error = script4parse();
			}
		break;
		default:
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "%s No parser available for executing script %s", VAL_PREFIX_FAILED, filenameM.c_str());
		}
		break;
	}

	// close the file
	close();

	// try logging the relationship analysis
	if (!inSuperScriptM)
	{
		sessionM_ptr->logRelationship();
	}

    return (error) ? false : true;
}


//>>===========================================================================

bool DICOM_SCRIPT_CLASS::parse()

//  DESCRIPTION     : Parse script file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool result = false;
	onlyParseFileM = true;

	result = execute();

	return result;
}

//>>===========================================================================

DICOM_SUPER_SCRIPT_CLASS::DICOM_SUPER_SCRIPT_CLASS(SCRIPT_SESSION_CLASS *session_ptr, string filename)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// initialise class members
	sessionM_ptr = session_ptr;
	loggerM_ptr = session_ptr->getLogger();

	// save dicom script pathname
	pathnameM = sessionM_ptr->getDicomScriptRoot();

	// save dicom script name
	filenameM = filename;

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "DICOMSuperScript: %s", filenameM.c_str());
	}

	onlyParseFileM = false;

	fdM_ptr = NULL;
}

//>>===========================================================================

DICOM_SUPER_SCRIPT_CLASS::~DICOM_SUPER_SCRIPT_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// close open DICOM Super Script
	close();
}

//>>===========================================================================

bool DICOM_SUPER_SCRIPT_CLASS::execute()

//  DESCRIPTION     : Execute super script file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	static const int kReadMode = 4;	// read permission mode for _access()

	bool result = true;
	char line[MAXIMUM_LINE_LENGTH];

	// open super script file
	if (!open()) return false;

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT_NAME, 1, "%s", filenameM.c_str());
	}

	// read script filenames
	while (fgets(line, MAXIMUM_LINE_LENGTH, fdM_ptr) != NULL) 
	{
		// clean up the line
		int length = cleanUpLine(line);

		// check if anything remains
		if (length > 0) 
		{
			// allow comments
			if (line[0] == '#') 
			{
				if (strncmp(line, "##", 2) == 0) 
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_NONE, 1, "%s", line);
					}
				}
				continue;
			}

			// allow "do loop" - DO xxx filename
			// need to improve parsing of whitespace
			int	count;
			int	index;

			if (strncmp(line, "DO", 2) == 0) 
			{
				// get to loop count
				index = 2;
				while ((index < length) &&
					(line[index] == SPACECHAR))
				{
					index++;
				}

				// now we should be at the loop count
				char charCount[8];
				int i = 0;
				while ((i < 8) && 
					(line[index] != SPACECHAR))
				{
					if (!isdigit(line[index]))
					{
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_ERROR, 1, "Syntax error around loop count (xxx) in \"DO xxx DicomScript\"");
						}
						return false;
					}
					charCount[i++] = line[index++];
				}

				charCount[i] = NULLCHAR;
				count = atoi(charCount);

				// move to script name
				while ((index < length) &&
					(line[index] == SPACECHAR))
				{
					index++;
				}
			}
			else 
			{
				index = 0;
				count = 1;
			}

			for (int j = 0; j < count; j++) 
			{
				// instantiate a DICOM Script

				string dicomScriptname(&line[index]);
				string scriptFilename = pathnameM + dicomScriptname;

				// check the existence of the script file name and also for invalid script file
				if( _access(scriptFilename.c_str(), kReadMode) == -1 )
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_ERROR, 1, "Invalid DICOMScript: %s, the DICOMScript doesn't exist", dicomScriptname.c_str());
						return false;
					}
				}

				if (loggerM_ptr) 
				{
					if (onlyParseFileM)
					{
						loggerM_ptr->text(LOG_INFO, 1, "Parsing DICOMScript: %s  for %d of %d times", dicomScriptname.c_str(), j + 1, count);
					}
					else
					{
						loggerM_ptr->text(LOG_INFO, 1, "Executing DICOMScript: %s  for %d of %d times", dicomScriptname.c_str(), j + 1, count);
					}
				}

				// execute the contents
				DICOM_SCRIPT_CLASS dicomScript(sessionM_ptr, dicomScriptname);
				dicomScript.setInSuperScript(true);
				if (!dicomScript.execute()) 
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_INFO, 1, "Failed to execute iteration %d of DICOMScript: %s", j + 1, dicomScriptname.c_str());
					}


					// check whether or not we should continue after the error
					if (sessionM_ptr->getContinueOnError())
					{
						// record error - but continue
						result = false;
					}
					else
					{
						return false;
					}
				}
			}
		}
	}

	// try logging the relationship analysis
	sessionM_ptr->logRelationship();

	return result;
}

//>>===========================================================================

bool DICOM_SUPER_SCRIPT_CLASS::parse()

//  DESCRIPTION     : Parse super script file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool result = false;
	onlyParseFileM = true;

	result = execute();

	return result;
}

//>>===========================================================================

int DICOM_SUPER_SCRIPT_CLASS::cleanUpLine(char *buffer)

//  DESCRIPTION     : Remove any unwanted characters from the line of the SuperScript.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	char line[MAXIMUM_LINE_LENGTH];

	// copy the line locally
	strcpy(line, buffer);

	// get the length of the line
	int length = strlen(line);

	// remove the CR[LF] in the line read
	if ((length > 0)
	 && (line[length-1] == LINEFEED))
		length--;

	if ((length > 0)
	 && (line[length-1] == CARRIAGERETURN))
		length--;

	//  this is intentional!!
	if ((length > 0)
	 && (line[length-1] == CARRIAGERETURN))
		length--;

	// convert tab to space
	for (int i = 0; i < length; i++)
	{
		if (line[i] == HORIZTAB)
		{
			line[i] = SPACECHAR;
		}
	}

	// remove any trailing spaces
	while ((length > 0) &&
		(line[length-1] == SPACECHAR))
	{
		length--;
	}

	// terminate the line
	line[length] = '\0';

	// check for leading spaces
	int index = 0;
	while ((index < length) && 
		(line[index] == SPACECHAR))
	{
		index++;
	}

	// finally copy the line back to the buffer
	strcpy(buffer, &line[index]);

	// return new length
	return strlen(buffer);
}
