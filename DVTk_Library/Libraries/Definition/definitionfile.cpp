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

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "definitionfile.h"
#include "deffilecontent.h"
#include "definitiondetails.h"
#include "definition.h"
#include "Isession.h"			// Session component interface


extern FILE*      definitionin;
extern string     definitionfilename;
extern bool		  definitionfileempty;
extern bool       definitionnewfile;
extern int	      definitionlineno;
extern void		  definitionrestart(FILE*);
extern int	      definitionparse(void);
extern UINT       definitionerrors;
extern UINT       definitionwarnings;
extern LOG_CLASS* definitionfilelogger_ptr;

extern void       resetDefinitionParser();

DEF_FILE_CONTENT_CLASS* defFileContent_ptr = NULL;


//>>===========================================================================

DEFINITION_FILE_CLASS::DEFINITION_FILE_CLASS(const string filename) 

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// initialise class members
	fileContentM_ptr = NULL;
	loadedM = false;
	sessionM_ptr = NULL;
	loggerM_ptr = NULL;
	pathnameM = "";
	filenameM = filename;
	onlyParseFileM = false;
	fdM_ptr = NULL;
}


//>>===========================================================================

DEFINITION_FILE_CLASS::DEFINITION_FILE_CLASS(BASE_SESSION_CLASS *session_ptr, const string filename) 

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	const char * remPath = NULL;
	char buffer[MAX_STRING_LEN];
	char * environmentVar = NULL;

	// initialise class members
	fileContentM_ptr = NULL;
	loadedM = false;
	sessionM_ptr = session_ptr;
	loggerM_ptr = session_ptr->getLogger();

	// save the defintion file pathname
	pathnameM = sessionM_ptr->getDefinitionFileRoot();

	// save filename
	filenameM = filename;

	if( strncmp(filename.c_str(),"%",1) == 0)
	{	
		strcpy(buffer ,filename.c_str());
		environmentVar = strtok(buffer,"%");
		filenameM = getenv(environmentVar); 

		remPath = strrchr(filename.c_str(),'%');
		strcpy(buffer,remPath);
		remPath = strtok(buffer,"%");		
		filenameM = filenameM+remPath;
	}

	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Definition File: %s", filenameM.c_str());
	}

	onlyParseFileM = false;
	fdM_ptr = NULL;
}

//>>===========================================================================

DEFINITION_FILE_CLASS::~DEFINITION_FILE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// destructor activities
	if (fileContentM_ptr)
	{
		delete fileContentM_ptr;
	}
}

//>>===========================================================================

bool DEFINITION_FILE_CLASS::execute()

//  DESCRIPTION     : Execute (parse) the definition file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool success = false;


    // Create a semaphore with initial and max. counts of 1.
    LONG cMax = 1;
    HANDLE hSemaphore = CreateSemaphore( 
        NULL,   // no security attributes
        cMax,   // initial count
        cMax,   // maximum count
        "DefinitionFile");  // unnamed semaphore
    if (hSemaphore == NULL) 
    {
        return false;
    }

    // Try to enter the semaphore gate.
    DWORD dwWaitResult = WaitForSingleObject( 
        hSemaphore,   // handle to semaphore
        10L);         // ten-second time-out interval
    if (dwWaitResult == WAIT_TIMEOUT)
    {
        return false;
    }

	// file must be open to be parsed
	if (open()) 
	{
		// set parser to read a Definition File
		definitionlineno = 1;
		definitionin = fdM_ptr;
		definitionfileempty = true;
		definitionnewfile = true;
		definitionfilename = filenameM;
		definitionfilelogger_ptr = loggerM_ptr;
		definitionrestart(definitionin);
		resetDefinitionParser();

		//initialize errors and warnings
		definitionerrors = 0;
		definitionwarnings = 0;

		// allocate the file contents class
		fileContentM_ptr = new DEF_FILE_CONTENT_CLASS();

		// copy address to static
		defFileContent_ptr = fileContentM_ptr;

		//call definition parser
		(void) definitionparse();

		//Get error statistics from definition parser
        setNrErrors(definitionerrors);
		setNrWarnings(definitionwarnings);

		// reset the static
		defFileContent_ptr = NULL;

		// close the file
	    close();

		// set status
		success = (nr_errorsM == 0 ? true : false);

		// check if definition file empty
		if (definitionfileempty)
		{
			success = false;
		}
	}

    // Increment the count of the semaphore.
    if (!ReleaseSemaphore( 
        hSemaphore,  // handle to semaphore
        1,           // increase count by one
        NULL))       // not interested in previous count
    {
        // Deal with the error.
    }

	return success;
}

//>>===========================================================================

bool DEFINITION_FILE_CLASS::execute(int& lineNumber)

//  DESCRIPTION     : Execute (parse) the definition file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool success = false;

    // Create a semaphore with initial and max. counts of 1.
    LONG cMax = 1;
    HANDLE hSemaphore = CreateSemaphore( 
        NULL,   // no security attributes
        cMax,   // initial count
        cMax,   // maximum count
        "DefinitionFile");  // unnamed semaphore
    if (hSemaphore == NULL) 
    {
        return false;
    }

    // Try to enter the semaphore gate.
    DWORD dwWaitResult = WaitForSingleObject( 
        hSemaphore,   // handle to semaphore
        10L);         // ten-second time-out interval
    if (dwWaitResult == WAIT_TIMEOUT)
    {
        return false;
    }

	// file must be open to be parsed
	if (open()) 
	{
		// set parser to read a Definition File
		definitionlineno = 1;
		definitionin = fdM_ptr;
		definitionfileempty = true;
		definitionnewfile = true;
		definitionfilename = filenameM;
		definitionfilelogger_ptr = loggerM_ptr;
		definitionrestart(definitionin);
		resetDefinitionParser();

		//initialize errors and warnings
		definitionerrors = 0;
		definitionwarnings = 0;

		// allocate the file contents class
		fileContentM_ptr = new DEF_FILE_CONTENT_CLASS();

		// copy address to static
		defFileContent_ptr = fileContentM_ptr;

		//call definition parser
		(void) definitionparse();

		lineNumber = definitionlineno;

		//Get error statistics from definition parser
        setNrErrors(definitionerrors);
		setNrWarnings(definitionwarnings);

		// reset the static
		defFileContent_ptr = NULL;

		// close the file
	    close();

		// set status
		success = (nr_errorsM == 0 ? true : false);
	}

    // Increment the count of the semaphore.
    if (!ReleaseSemaphore( 
        hSemaphore,  // handle to semaphore
        1,           // increase count by one
        NULL))       // not interested in previous count
    {
        // Deal with the error.
    }

	// check if definition file empty
	if ((definitionfileempty) && (definitionlineno == 0))
	{
		success = false;

		char* errorText = new char[100];

		sprintf(errorText, "Definition file is empty.");
		throw errorText;
	}

	return success;
}

//>>===========================================================================

bool DEFINITION_FILE_CLASS::Load()

//  DESCRIPTION     : Load the defintion file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_INFO, 1, "Reading definition file: %s", filenameM.c_str());
	}

	bool parsed = false;

	// Add an Def File - first check if the Def File is already installed
	// Parse only new def files
	//parsed = DEFINITION->IsDefFileParsed(filenameM);
	
	if (filenameM != "")
	{
		string defFile = filenameM;
		if(!isAbsolutePath(defFile))
		{
			defFile = getAbsoluteFilename();
		}

		DEF_FILE_STRUCT* defFile_ptr = DEFINITION->GetDefFileStruct(defFile);
		if(defFile_ptr != NULL )
		{
			parsed = defFile_ptr->isInstalled;
		}

		if(!parsed)
		{
			// parse the file
			parsed = execute();

			if (parsed)
			{
				if((defFile_ptr != NULL ) && (!(defFile_ptr->isInstalled)))
				{
					DEFINITION->RemoveDefFile(defFile);
					DEFINITION->AddDefFile(defFile, defFile_ptr->sopClassName,
													defFile_ptr->sopClassUid,
													defFile_ptr->aeName,
													defFile_ptr->aeVersion,
													defFile_ptr->isMetaSop,
													true);
				}
				else
				{
					DEFINITION->AddDefFile(defFile, fileContentM_ptr->GetSOPClassName(),
													fileContentM_ptr->GetSOPClassUID(),
													fileContentM_ptr->GetAEName(),
													fileContentM_ptr->GetAEVersion(),
													fileContentM_ptr->IsMetaSOPClass(),
													true);
				}

				// install the definition file contents
				parsed = DEFINITION->Install(defFile, fileContentM_ptr);							
			}

			if (parsed)
			{
				// on successful load the pointer is taken over by the DEFINITION singleton
				// - set the file content to NULL
    			fileContentM_ptr = NULL;
			}
			else
			{
				// cleanup
				delete fileContentM_ptr;
				fileContentM_ptr = NULL;
			}

			// check for successful load
			if (nr_errorsM == 0)
			{
				// indicate that the defintion file is now loaded
				loadedM = true;
			}
			else
			{
				// not loaded successfully
				loadedM = false;
				parsed = false;
			}
		}
	}	

	return parsed;
}

//>>===========================================================================

bool DEFINITION_FILE_CLASS::Unload()

//  DESCRIPTION     : Unload the defintion file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	// uninstall the definition file contents
	string defFile = filenameM;
	if(!isAbsolutePath(defFile))
	{
		defFile = getAbsoluteFilename();
	}

	bool success = DEFINITION->Uninstall(defFile);
	if (success)
	{
		DEFINITION->RemoveDefFile(defFile);

		// set flag to indicate that the file is no longer loaded
		loadedM = false;

		// reset the definition file root
		// - this maybe the reason why the file was unloaded
		if (sessionM_ptr)
		{
			// save the defintion file pathname
			pathnameM = sessionM_ptr->getDefinitionFileRoot();
		}
		else
		{
			pathnameM = "";
		}
	}

	return success;
}

//>>===========================================================================

bool DEFINITION_FILE_CLASS::GetDetails(DEF_DETAILS_CLASS& details)

//  DESCRIPTION     : Get the defintion file details.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool parsed = false;
	
	string sopClassName;
	string sopClassUid;
	string aeName;
	string aeVersion;
	bool isMetaSop = false;

	// Add an Def File - first check if the Def File is already installed
	// Parse only new def files
	string defFile = filenameM;
	if(!isAbsolutePath(defFile))
	{
		defFile = getAbsoluteFilename();
	}

	parsed = DEFINITION->IsDefFileParsed(defFile);
	if (defFile != "")
	{
		if(!parsed)
		{
			int lineNumber = 0;

			// try to execute the file to load the details locally
			parsed = execute(lineNumber);

			if (parsed)
			{
				DEFINITION->AddDefFile(defFile, fileContentM_ptr->GetSOPClassName(),
												  fileContentM_ptr->GetSOPClassUID(),
												  fileContentM_ptr->GetAEName(),
												  fileContentM_ptr->GetAEVersion(),
												  fileContentM_ptr->IsMetaSOPClass(),
												  false);

				sopClassName = fileContentM_ptr->GetSOPClassName();
				sopClassUid = fileContentM_ptr->GetSOPClassUID();
				aeName = fileContentM_ptr->GetAEName();
				aeVersion = fileContentM_ptr->GetAEVersion();
				isMetaSop = fileContentM_ptr->IsMetaSOPClass();

				// delete the file content
				delete fileContentM_ptr;
				fileContentM_ptr = NULL;
			}
			else
			{
				// delete the file content
				delete fileContentM_ptr;
				fileContentM_ptr = NULL;

				char* errorText = new char[100];

				sprintf(errorText, "Parse error in line %i.", lineNumber);
				throw errorText;
			}
		}
		else
		{
			DEF_FILE_STRUCT* defFile_ptr = DEFINITION->GetDefFileStruct(defFile);
			if(defFile_ptr != NULL )
			{
				sopClassName = defFile_ptr->sopClassName;
				sopClassUid = defFile_ptr->sopClassUid;
				aeName = defFile_ptr->aeName;
				aeVersion = defFile_ptr->aeVersion;
				isMetaSop = defFile_ptr->isMetaSop;
			}
		}	
	
		// get the detail locally
		try
		{
			details.SetAEName(aeName);
		}
		catch(...)
		{
			throw "Error determining Application Entity name.";
		}

		try
		{
			details.SetAEVersion(aeVersion);
		}
		catch(...)
		{
			throw "Error determining Application Entity version.";
		}

		try
		{
			details.SetSOPClassName(sopClassName);
		}
		catch(...)
		{
			throw "Error determining SOP Class name.";
		}

		try
		{
			details.SetSOPClassUID(sopClassUid);
		}
		catch(...)
		{
			throw "Error determining SOP Class UID.";
		}

		try
		{
			details.SetIsMetaSOPClass(isMetaSop);
		}
		catch(...)
		{
			throw "Error determining if this is a meta SOP Class.";
		}		
	}

	return parsed;
}
