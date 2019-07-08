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

//  Base Test Session class.

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <direct.h>
#include <stdlib.h>
#include <stdio.h>

#include "session.h"
#include "sessionfile.h"	    // Session File include
#include "IAttributeGroup.h"    // Attribute Group component interface
#include "Idefinition.h"	    // Definition component interface
#include "Iwarehouse.h"		    // Warehouse component interface
#include "Iemulator.h"          // Emulator component interface
#include "Imedia.h"				// Media component interface

#ifdef _DEBUG
//#define _DEBUG_SESSION
#endif


//>>===========================================================================

ABSTRACT_MAP_CLASS::ABSTRACT_MAP_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	mappedFromM = "";
	mappedToM = "";
	sopClassUidM = "";
}

//>>===========================================================================

ABSTRACT_MAP_CLASS::ABSTRACT_MAP_CLASS(string mappedFrom, string mappedTo, string sopClassUid)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
#ifdef _DEBUG_SESSION
printf("\nDEBUG: ABSTRACT_MAP_CLASS::ABSTRACT_MAP_CLASS(mappedFrom:= %s mappedTo:= %s)", mappedFrom.c_str(), mappedTo.c_str());
#endif

	// constructor activities
	mappedFromM = mappedFrom;
	mappedToM = mappedTo;
	sopClassUidM = sopClassUid;
}

//>>===========================================================================

ABSTRACT_MAP_CLASS::~ABSTRACT_MAP_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}

//>>===========================================================================

bool ABSTRACT_MAP_CLASS::isMapped(const string mappedFrom)

//  DESCRIPTION     : Check if the string from matches that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check if the mapped from values match
	return (mappedFromM == mappedFrom) ? true : false;
}

//>>===========================================================================

bool ABSTRACT_MAP_CLASS::isSopClassUid(const string sopClassUid)

//  DESCRIPTION     : Check if the sopClassUid matches that given.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check if the sopClassUid values match
	return (sopClassUidM == sopClassUid) ? true : false;
}

//>>===========================================================================

BASE_SESSION_CLASS::BASE_SESSION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
    sessionDirectoryM = getCurrentWorkingDirectory();
    storageModeM = ::SM_NO_STORAGE;
	isDataDirectorySetInSessionM = false;	
}


//>>===========================================================================

BASE_SESSION_CLASS::~BASE_SESSION_CLASS()


//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// empty virtual destructor
}

//>>===========================================================================

void BASE_SESSION_CLASS::cleanup()

//  DESCRIPTION     : Free up resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// end the session
	if (isOpenM) end();

    // remove the definitions
	removeDefinitions();

	// remove the Definition Directories
    definitionDirectoryM.clear();
}

//>>===========================================================================

void BASE_SESSION_CLASS::setStrictValidation(bool flag)

//  DESCRIPTION     : Set the Strict Validation flag including the serializer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    strictValidationM = flag;
    if (serializerM_ptr)
    {
        serializerM_ptr->set_StrictValidationLogging(flag);
    }
}

//>>===========================================================================

void BASE_SESSION_CLASS::setSerializerStrictValidation(bool flag)

//  DESCRIPTION     : Set the Serializer Strict Validation flag only.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// update the serializer only
	if (serializerM_ptr)
	{
		serializerM_ptr->set_StrictValidationLogging(flag);
	}
}

//>>===========================================================================

void BASE_SESSION_CLASS::sessionFileVersion(int version)

//  DESCRIPTION     : Handle the version of the session file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	sessionFileVersionM = version;

	if (version > CURRENT_SESSION_FILE_VERSION)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_WARNING, 1, "Session file was created with a newer version of DVT");
			loggerM_ptr->text(LOG_WARNING, 1, "Incompatibilites or parse errors may result");
		}
	}
}

//>>===========================================================================

bool BASE_SESSION_CLASS::reloadDefinitions()

//  DESCRIPTION     : Unload the current definition file set and reset the
//					: abstract mappings.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool definitionFileLoaded = true;

	// unload definition files
	for (UINT i = 0; i < definitionFileM_ptr.getSize(); i++)
	{
		// unload the definition file
		if (definitionFileM_ptr[i]->IsLoaded())
		{
			definitionFileM_ptr[i]->Unload();
		}
	}

	while (abstractMapM.getSize())
	{
		abstractMapM.removeAt(0);
	}

	// begin the session again
	return begin(definitionFileLoaded);
}

//>>===========================================================================

void BASE_SESSION_CLASS::removeDefinitions()

//  DESCRIPTION     : Remove the Definition Files.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// free up member structures
	while (definitionFileM_ptr.getSize())
	{
		// unload the definition file
		if (definitionFileM_ptr[0]->IsLoaded())
		{
			definitionFileM_ptr[0]->Unload();
		}

		// free local storage
		delete definitionFileM_ptr[0];
		definitionFileM_ptr.removeAt(0);
	}

	while (abstractMapM.getSize())
	{
		abstractMapM.removeAt(0);
	}
}

//>>===========================================================================

void BASE_SESSION_CLASS::checkAbstractMappings()

//  DESCRIPTION     : Check if the abstract mappings need to be set up.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	AE_SESSION_CLASS ae_session;

	ae_session.SetName (this->getApplicationEntityName());
	ae_session.SetVersion (this->getApplicationEntityVersion());

	// set up the abstract mappings - get the sop of the last definition file loaded
	string lastSopUidInstalled = DEFINITION->GetLastSopUidInstalled();
	if (!lastSopUidInstalled.length()) return;


	// check for storage or print sop classes
	if (DEFINITION->IsStorageSop(lastSopUidInstalled, &ae_session))
	{
		// last definition file was a storage one
		string abstractSopName = ABSTRACT_STORAGE_SOP_CLASS_NAME;
		bool mapped = false;

		// check if the abstract sop class uid has aleady been defined
		for (UINT i = 0; i < abstractMapM.getSize(); i++)
		{
			if (abstractMapM[i].isMapped(abstractSopName))
			{
				// we already have a mapping
				mapped = true;
				break;
			}
		}

		// check if a mapping has been found
		if (!mapped)
		{
			// set up the new abstract uid mapping
			ABSTRACT_MAP_CLASS abstractSopUidMap(abstractSopName, lastSopUidInstalled, lastSopUidInstalled);
			abstractMapM.add(abstractSopUidMap);
			string uidName = DEFINITION->GetSopName(lastSopUidInstalled);

			// also define the abstract iod name mapping
			string abstractIodName = ABSTRACT_STORAGE_IOD_NAME;
			string iodName;
			DEFINITION->GetIodName(DIMSE_CMD_CSTORE_RQ, lastSopUidInstalled, &ae_session, iodName);
			ABSTRACT_MAP_CLASS abstractIodNameMap(abstractIodName, iodName, lastSopUidInstalled);
			abstractMapM.add(abstractIodNameMap);

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Setting abstract Storage mapping \"%s\" to \"%s\" (%s)", abstractSopName.c_str(), uidName.c_str(), lastSopUidInstalled.c_str());
				loggerM_ptr->text(LOG_DEBUG, 1, "Abstract Iod Name for \"%s\" is \"%s\"", abstractIodName.c_str(), iodName.c_str());
			}
		}
	}
	else if (DEFINITION->IsPrintSop(lastSopUidInstalled, &ae_session))
	{
		// try to get containing meta sop class from sop class
		string metaSopUid = DEFINITION->GetMetaSopUid(lastSopUidInstalled);

		if (metaSopUid.length())
		{
			// last definition file was a print one
			string abstractSopName = ABSTRACT_META_SOP_CLASS_NAME;
			bool mapped = false;

			// check if the abstract meta sop class uid has aleady been defined
			for (UINT i = 0; i < abstractMapM.getSize(); i++)
			{
				if (abstractMapM[i].isMapped(abstractSopName))
				{
					// we already have a mapping
					mapped = true;
					break;
				}
			}

			// check if a mapping has been found
			if (!mapped)
			{
				// set up the new abstract uid mapping
				ABSTRACT_MAP_CLASS abstractSopUidMap(abstractSopName, metaSopUid, metaSopUid);
				abstractMapM.add(abstractSopUidMap);
				string uidName = DEFINITION->GetSopName(metaSopUid);

				// define the abstract image box name mapping
				string abstractImageBoxSopUid = ABSTRACT_IMAGE_BOX_SOP_CLASS_NAME;
				string imageBoxSopUid = DEFINITION->GetImageBoxSopUid(metaSopUid);

				ABSTRACT_MAP_CLASS abstractImageBoxSopUidMap(abstractImageBoxSopUid, imageBoxSopUid, metaSopUid);
				abstractMapM.add(abstractImageBoxSopUidMap);

				// also define the abstract image box iod name mapping
				string abstractImageBoxIodName = ABSTRACT_IMAGE_BOX_NAME;
				string imageBoxSopUidName = DEFINITION->GetImageBoxSopName(metaSopUid);
				string imageBoxIodName;
				DEFINITION->GetIodName(DIMSE_CMD_NSET_RQ, imageBoxSopUid, &ae_session, imageBoxIodName);

				ABSTRACT_MAP_CLASS abstractImageBoxNameMap(abstractImageBoxIodName, imageBoxIodName, metaSopUid);
				abstractMapM.add(abstractImageBoxNameMap);

				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_DEBUG, 1, "Setting abstract Print mapping \"%s\" to \"%s\" (%s)", abstractSopName.c_str(), uidName.c_str(), metaSopUid.c_str());
					loggerM_ptr->text(LOG_DEBUG, 1, "Abstract Image Box Uid for \"%s\" is \"%s\" (%s)", abstractImageBoxSopUid.c_str(), imageBoxSopUidName.c_str(), imageBoxSopUid.c_str());
					loggerM_ptr->text(LOG_DEBUG, 1, "Abstract Image Box Name for \"%s\" is \"%s\"", abstractImageBoxIodName.c_str(), imageBoxIodName.c_str());
				}
			}
		}
		else
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Cannot find Meta SOP Class UID for SOP Class UID: %s", lastSopUidInstalled.c_str());
			}
		}
	}
}

//>>===========================================================================

bool BASE_SESSION_CLASS::isPathAbsolute(char *root_ptr)

//  DESCRIPTION     : Check if the root is absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// call utility function
	return isAbsolutePath(root_ptr);
}

//>>===========================================================================

void BASE_SESSION_CLASS::makeRootAbsolute(string &root, char *root_ptr)

//  DESCRIPTION     : Take the root pointer and turn it into an absolute root.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// copy the session directory
	char buffer[_MAX_PATH];
	strcpy(buffer, (char*) sessionDirectoryM.c_str());
    if (sessionDirectoryM[sessionDirectoryM.length()-1] != '\\')
        strcat(buffer, "\\");

   // check for current directory
   if (strcmp(root_ptr, ".\\") == 0)
   {
	   	// return session directory
		root = sessionDirectoryM;
		return;
   }

	// check if the root is already absolute
	if (isPathAbsolute(root_ptr))
	{
		// pathname is absolute
		strcpy(buffer, root_ptr);
	}
	else
	{
		// add current working directory
		strcat(buffer, root_ptr);
	}

	// reduce pathname to its shortest form
	reducePathname(buffer);

	// copy store result
	root = buffer;
}


//>>===========================================================================

void BASE_SESSION_CLASS::setLogLevel(bool flag, UINT32 logLevel)

//  DESCRIPTION     : Set/reset the given log level according to the flag value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	UINT32 logMask;

	// get the current log mask
	if (loggerM_ptr)
	{
		logMask = loggerM_ptr->getLogMask();
	}
	else
	{
		logMask = logMaskM;
	}

	if (flag)
	{
		// enable the logging
		logMask |= logLevel;
	}
	else
	{
		// disable the logging
		UINT32 notLogLevel = ~logLevel;
		logMask &= notLogLevel;
	}

	// set the new log mask
	if (loggerM_ptr)
	{
		loggerM_ptr->setLogMask(logMask);
	}
	else
	{
		logMaskM = logMask;
	}
}

//>>===========================================================================

bool BASE_SESSION_CLASS::isLogLevel(UINT32 logLevel)

//  DESCRIPTION     : Check if the given log level is enabled.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool flag = false;

	// check if logger available
	if (loggerM_ptr)
	{
		// set the verbose flag
		UINT32 logMask = loggerM_ptr->getLogMask();
		logMask &= logLevel;
		flag = (logMask) ? true : false;
	}

	// return verbose flag
	return flag;
}

//>>===========================================================================

void BASE_SESSION_CLASS::setDefinitionFileRoot(char* definitionFileRoot_ptr)

//  DESCRIPTION     : Set the definition file root - make it absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The definition file root is now defined as the first entry
//                  : in the definitionDirectory vector.
//<<===========================================================================
{
	// save the absolute definition file root
    string definitionFileRoot;

	makeRootAbsolute(definitionFileRoot, definitionFileRoot_ptr);
    //
    // Add backslash at the end of the definition file root path
    //
    if (definitionFileRoot[definitionFileRoot.length()-1] != '\\')
    {
        definitionFileRoot += "\\";
    }
    //
    // Clip away ".\" from end of absolute path.
    //
    char pathname[_MAX_DIR];
    strcpy(pathname, definitionFileRoot.c_str());
    reducePathname(pathname);
    definitionFileRoot = pathname;

	// log the definition file root
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Definition File root: \"%s\"", definitionFileRoot.c_str());
	}

    // root must always be the first entry - no matter what!
    if (definitionDirectoryM.size() > 0)
    {
        // update the first entry
        definitionDirectoryM[0] = definitionFileRoot;
    }
    else
    {
        // add the first entry
        definitionDirectoryM.push_back(definitionFileRoot);
    }
}

//>>===========================================================================

const char *BASE_SESSION_CLASS::getDefinitionFileRoot()

//  DESCRIPTION     : Set the definition file root - make it absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    const char *definitionFileRoot = NULL;

    // ensure that the entry is defined
    if (definitionDirectoryM.size() > 0)
    {
        // definition file root is always the first entry
        definitionFileRoot = definitionDirectoryM[0].c_str();
    }

    return definitionFileRoot;
}

//>>===========================================================================

DEFINITION_FILE_CLASS * BASE_SESSION_CLASS::getDefinitionFile(UINT i)

//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	DEFINITION_FILE_CLASS *definitionFile_ptr = NULL;

	if (i < definitionFileM_ptr.getSize())
	{
		definitionFile_ptr = definitionFileM_ptr[i];
	}

	return definitionFile_ptr;
}

//>>===========================================================================

const char * BASE_SESSION_CLASS::getDefinitionFilename(UINT i)

//  DESCRIPTION     :
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	const char *filename_ptr = NULL;

	if (i < definitionFileM_ptr.getSize())
	{
		filename_ptr = definitionFileM_ptr[i]->getFilename();
	}

	return filename_ptr;
}


//>>===========================================================================

void BASE_SESSION_CLASS::setDataDirectory(char* dataDirectory_ptr)

//  DESCRIPTION     : Set the data directory - make it absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// Data directory is set in the session file
	isDataDirectorySetInSessionM = true;

	// save the absolute data directory
	makeRootAbsolute(dataDirectoryM, dataDirectory_ptr);
    //
    // Add backslash at the end of the dataDirectoryM directory path
    //
    if (dataDirectoryM[dataDirectoryM.length()-1] != '\\')
    {
        dataDirectoryM += "\\";
    }
    //
    // Clip away ".\" from end of absolute path.
    //
    char pathname[_MAX_DIR];
    strcpy(pathname, dataDirectoryM.c_str());
    reducePathname(pathname);
    dataDirectoryM = pathname;

	// log the data directory
	if (loggerM_ptr)
	{
		loggerM_ptr->setStorageRoot(dataDirectoryM);
		loggerM_ptr->text(LOG_DEBUG, 1, "Data Directory: \"%s\"", dataDirectoryM.c_str());
	}
}

//>>===========================================================================

void BASE_SESSION_CLASS::setResultsRoot(char* resultsRoot_ptr)

//  DESCRIPTION     : Set the results file root - make it absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// save the absolute results file root
	makeRootAbsolute(resultsRootM, resultsRoot_ptr);
    //
    // Add backslash at the end of the resultsRootM directory path
    //
    if (resultsRootM[resultsRootM.length()-1] != '\\')
    {
        resultsRootM += "\\";
    }
    //
    // Clip away ".\" from end of absolute path.
    //
    char pathname[_MAX_DIR];
    strcpy(pathname, resultsRootM.c_str());
    reducePathname(pathname);
    resultsRootM = pathname;

	// log the results root
	if (loggerM_ptr)
	{
		loggerM_ptr->setResultsRoot(resultsRootM);
		loggerM_ptr->text(LOG_DEBUG, 1, "Results root: \"%s\"", resultsRootM.c_str());
	}
}

//>>===========================================================================

string BASE_SESSION_CLASS::getAbsolutePixelPathname(char *filename_ptr)

//  DESCRIPTION     : Get the absolute pixel pathname by checking the incoming
//					  filename and combining it with the script root where necessary.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	char buffer[_MAX_PATH];
	string pathname = "";

	// check if the filename is already absolute
	if (isPathAbsolute(filename_ptr))
	{
		// filename is absolute
		pathname = filename_ptr;
	}
	else
	{
		// add the script root
		strcpy(buffer, getDicomScriptRoot());

//		// add the filename
//#ifdef _WINDOWS
//		strcat(buffer, "\\");
//#else
//		strcat(buffer, "/");
//#endif
		strcat(buffer, filename_ptr);

		// reduce pathname to its shortest form
		reducePathname(buffer);

		// filename is now absolute
		pathname = buffer;
	}

	// log the pixel filename
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Pixel filename: \"%s\"", pathname.c_str());
	}

	// return absolute pixel file pathname
	return pathname;
}

//>>===========================================================================

bool BASE_SESSION_CLASS::load(string sessionFilename, bool& definitionFileLoaded, bool andBeginIt)

//  DESCRIPTION     : Load the session file parameters.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	definitionFileLoaded = true;

	// enable some logging
	if (loggerM_ptr)
	{
		// enable the base level logging
		UINT32 logMask = loggerM_ptr->getLogMask();
		logMask |= LOG_NONE;
		loggerM_ptr->setLogMask(logMask);
	}

    // Check if the given session filename contains a full path.
    // If so, get the session directory from the filename.
    unsigned int slashPosition = sessionFilename.find_last_of ('\\');
    if (slashPosition != sessionFilename.npos)
    {
        sessionDirectoryM = sessionFilename.substr (0, slashPosition + 1);
    }

	// cleanup previous session
	cleanup();

	// save session file name
	filenameM = sessionFilename;
	SESSION_FILE_CLASS	sessionFile(this, sessionFilename);

	// load the session file
	if (!sessionFile.execute())
	{
		// failed to load session file
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_NONE, 2, "%s %s", VAL_PREFIX_FAILED, sessionFile.getFilename());
		}

		// can't continue if session file load fails
		return false;
	}

	bool result = true;

	// check whether or not to begin the session after a successful load
	if (andBeginIt)
	{
		// begin the session
		result = begin(definitionFileLoaded);
	}

	// return result
	return result;
}

//>>===========================================================================

bool BASE_SESSION_CLASS::begin(bool& definitionFileLoaded)

//  DESCRIPTION     : Begin the session - load definition files.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	definitionFileLoaded = true;

	// enable some logging
	if (loggerM_ptr)
	{
		// enable the base level logging
		UINT32 logMask = loggerM_ptr->getLogMask();
		logMask |= LOG_NONE;
		loggerM_ptr->setLogMask(logMask);
	}

    // install the Character Set and Image Display Format data files
    installCharacterSetAndImageDisplayFormatAndDefaultCommandSetDefData();

	// make sure that we have absolute pathnames defined for the definition files and results
    string definitionFileRoot = ".\\";
    if (noDefinitionDirectories() > 0)
    {
            definitionFileRoot = getDefinitionDirectory(0);
    }
	if (definitionFileRoot == ".\\")
	{
		// explicity set the definition file root
		setDefinitionFileRoot(".\\");
	}

	if (resultsRootM == ".\\")
	{
		// explicity set the results root
		setResultsRoot(".\\");
	}

	// load any definition files
	string blank;
	DEFINITION->SetLastSopUidInstalled(blank);
	for (UINT i = 0; i < definitionFileM_ptr.getSize(); i++)
	{
		DEFINITION_FILE_CLASS *definitionFile_ptr = definitionFileM_ptr[i];

		//set logger for definition file errors
		definitionFile_ptr->SetLogger(loggerM_ptr);

		// load defintion file
		if (!definitionFile_ptr->Load())
		{
			// failed to load definition file
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_NONE, 2, "%s Could not load %s", VAL_PREFIX_FAILED, definitionFile_ptr->getFilename());
			}
	
			// remove definition file from list on failure to load
			delete definitionFileM_ptr[i];
			definitionFileM_ptr[i] = NULL;

			definitionFileLoaded = false;
		}
        else
        {
    		// check if the abstract mapping need setting up
	    	checkAbstractMappings();
        }
	}

	// check if any definition files failed to load
loop1:
	if (definitionFileLoaded == false)
	{
		for (UINT i = 0; i < definitionFileM_ptr.getSize(); i++)
		{
			if (definitionFileM_ptr[i] == NULL)
			{
				definitionFileM_ptr.removeAt(i);
				goto loop1;
			}
		}
	}

	// session is now open for business
	isOpenM = true;

	// session is not stopped
	isSessionStoppedM = false;

	// return result
	return true;
}

//>>===========================================================================
string BASE_SESSION_CLASS::getFilename(void)

//  DESCRIPTION     : Get destination file name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return filenameM;
}

//>>===========================================================================

void BASE_SESSION_CLASS::setFileName(string fileName)

//  DESCRIPTION     : Set destination file name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    filenameM = fileName;
}

//>>===========================================================================

bool BASE_SESSION_CLASS::save()

//  DESCRIPTION     : Save the session parameters to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	SESSION_FILE_CLASS	sessionFile(this, filenameM);

	// save the session file
	return sessionFile.save();
}

//>>===========================================================================

bool BASE_SESSION_CLASS::save(string sessionFilename)

//  DESCRIPTION     : Save the session parameters to the given file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// save new file name
	filenameM = sessionFilename;

	// save the session
	return save();
}

//>>===========================================================================

void BASE_SESSION_CLASS::end()

//  DESCRIPTION     : End session - shut it down gracefully.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// session no longer open
	isOpenM = false;
}

//>>===========================================================================

void BASE_SESSION_CLASS::addDefinitionDirectory(string definitionDirectory)

//  DESCRIPTION     : Add a Definition Directory to the Session list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	string lDefinitionDirectory;
	char buffer[MAX_STRING_LEN];
	char * environmentVar = NULL;
	const char * remPath = NULL;

	// check for environment valriable set 	
	if(  strncmp(definitionDirectory.c_str(),"%",1) == 0)
	{		
		strcpy(buffer,definitionDirectory.c_str());
		environmentVar = strtok(buffer,"%");

		lDefinitionDirectory = getenv(environmentVar); 

		remPath = strrchr(definitionDirectory.c_str(),'%');
		strcpy(buffer,remPath);
		remPath = strtok(buffer,"%");

		lDefinitionDirectory = 	lDefinitionDirectory+ remPath;
	}

	// check if the filename is already absolute
	else if (isPathAbsolute((char*) definitionDirectory.c_str()))
	{
		lDefinitionDirectory = definitionDirectory;
	}
	else
	{
		// start from session directory
		lDefinitionDirectory = sessionDirectoryM;

		// save the absolute results file root
		makeRootAbsolute(lDefinitionDirectory, (char*) definitionDirectory.c_str());
	}

    //
    // Add backslash at the end of the lDefinitionDirectory directory path
    //
    if (lDefinitionDirectory[lDefinitionDirectory.length()-1] != '\\')
    {
        lDefinitionDirectory += "\\";
    }

	// log the definition directory
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Definition directory: \"%s\"", lDefinitionDirectory.c_str());
	}


    // add the directory to the list
    definitionDirectoryM.push_back(lDefinitionDirectory);
}

//>>===========================================================================

UINT BASE_SESSION_CLASS::noDefinitionDirectories()

//  DESCRIPTION     : Get the number of Definition Directories currently defined in the Session list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // return number of Definition Directories
    return definitionDirectoryM.size();
}

//>>===========================================================================

string BASE_SESSION_CLASS::getDefinitionDirectory(UINT index)

//  DESCRIPTION     : Get the indexed Definition Directory from Session list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    string definitionDirectory;

    // return the indexed Definition Directory
    if (index < definitionDirectoryM.size())
    {
        definitionDirectory = definitionDirectoryM[index];
    }

    return definitionDirectory;
}

//>>===========================================================================

void BASE_SESSION_CLASS::removeAllDefinitionDirectories()

//  DESCRIPTION     : Remove Definition Directory from Session list.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    definitionDirectoryM.clear();
}

//>>===========================================================================

FILE_DATASET_CLASS* BASE_SESSION_CLASS::readMedia(string fileName, 
											MEDIA_FILE_CONTENT_TYPE_ENUM fileContentType, 
											string sopClassUid, 
											string sopInstanceUid, 
											string transferSyntaxUid,
											bool useSessionStorageMode, 
											bool logIt)

//  DESCRIPTION     : Read the media file to in memory FILE_DATASET_CLASS object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  : Returns NULL on failure to load media.
//  EXCEPTIONS      :
//  NOTES           : Caller becomes owner
//<<===========================================================================
{
    //create file object
    FILE_DATASET_CLASS* fileDataset_ptr = new FILE_DATASET_CLASS(fileName);

	fileDataset_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);
	fileDataset_ptr->setEnsureEvenAttributeValueLengthM(ensureEvenAttributeValueLengthM);

	// set up the storage mode
	if (useSessionStorageMode)
	{
		// set storage mode to the value defined in the session
		fileDataset_ptr->setStorageMode(storageModeM);

		// also cascade logger - independent of logIt setting
		// - logger needed to define storage root associated with the storage mode
        fileDataset_ptr->setLogger(loggerM_ptr);
	}

    // cascade the logger
    if (logIt)
    {
        fileDataset_ptr->setLogger(loggerM_ptr);
    }

    // read the dataset from the file
    bool result = fileDataset_ptr->read();
    if (!result)
    {
        if ((logIt) &&
            (loggerM_ptr))
        {
            loggerM_ptr->text(LOG_ERROR, 1, "Can't read file: %s", fileName.c_str());
        }

        delete fileDataset_ptr;
        fileDataset_ptr = NULL;
    }

    // return the dataset
    return fileDataset_ptr;
}

//>>===========================================================================

bool BASE_SESSION_CLASS::writeMedia(FILE_DATASET_CLASS* fileDataset_ptr)

//  DESCRIPTION     : Write the in memory FILE_DATASET_CLASS object to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (fileDataset_ptr == NULL) return false;

	// Set Group length setting
	if (getAddGroupLength())
	{
		fileDataset_ptr->setAddGroupLength(true);
	}

    string fileName = fileDataset_ptr->getFilename();
    if (fileName.length() == 0)
    {
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_ERROR, 1, "Filename not defined for writing media");
        }
        return false;
    }

    if (loggerM_ptr)
    {
        loggerM_ptr->text(LOG_SCRIPT, 1, "Writing media file: \"%s\"", fileName.c_str());
    }

    // cascade the logger
    fileDataset_ptr->setLogger(loggerM_ptr);
	fileDataset_ptr->setEnsureEvenAttributeValueLengthM(ensureEvenAttributeValueLengthM);

    // write the dataset to file (including the FMI)
    bool result = fileDataset_ptr->write();
    if (!result)
    {
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_ERROR, 1, "Can't write file: %s", fileName.c_str());
        }
    }

    // return result
    return result;
}

//>>===========================================================================

FILE_DATASET_CLASS* BASE_SESSION_CLASS::readDicomdir(string fileName, 
											bool useSessionStorageMode, 
											bool logIt)

//  DESCRIPTION     : Read the media file to in memory FILE_DATASET_CLASS object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  : Returns NULL on failure to load media.
//  EXCEPTIONS      :
//  NOTES           : Caller becomes owner
//<<===========================================================================
{
    //create file object
    FILE_DATASET_CLASS* fileDataset_ptr = new FILE_DATASET_CLASS(fileName);

	fileDataset_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);
	fileDataset_ptr->setEnsureEvenAttributeValueLengthM(ensureEvenAttributeValueLengthM);

	// set up the storage mode
	if (useSessionStorageMode)
	{
		// set storage mode to the value defined in the session
		fileDataset_ptr->setStorageMode(storageModeM);

		// also cascade logger - independent of logIt setting
		// - logger needed to define storage root associated with the storage mode
        fileDataset_ptr->setLogger(loggerM_ptr);
	}

    // cascade the logger
    if (logIt)
    {
        fileDataset_ptr->setLogger(loggerM_ptr);
    }

    // read the dataset from the file
    bool result = fileDataset_ptr->read();
    if (!result)
    {
        if ((logIt) &&
            (loggerM_ptr))
        {
            loggerM_ptr->text(LOG_ERROR, 1, "Can't read file: %s", fileName.c_str());
        }

        delete fileDataset_ptr;
        fileDataset_ptr = NULL;
    }

    // return the dataset
    return fileDataset_ptr;
}

//>>===========================================================================

bool BASE_SESSION_CLASS::writeDicomdir(FILE_DATASET_CLASS* fileDataset_ptr)

//  DESCRIPTION     : Write the in memory FILE_DATASET_CLASS object to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (fileDataset_ptr == NULL) return false;

	// Set Group length setting
	if (getAddGroupLength())
	{
		fileDataset_ptr->setAddGroupLength(true);
	}

    string fileName = fileDataset_ptr->getFilename();
    if (fileName.length() == 0)
    {
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_ERROR, 1, "Filename not defined for writing media");
        }
        return false;
    }

    if (loggerM_ptr)
    {
        loggerM_ptr->text(LOG_SCRIPT, 1, "Writing media file: \"%s\"", fileName.c_str());
    }

    // cascade the logger
    fileDataset_ptr->setLogger(loggerM_ptr);
	fileDataset_ptr->setEnsureEvenAttributeValueLengthM(ensureEvenAttributeValueLengthM);

    // write the dataset to file (including the FMI)
    bool result = fileDataset_ptr->write();
    if (!result)
    {
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_ERROR, 1, "Can't write file: %s", fileName.c_str());
        }
    }

    // return result
    return result;
}

//>>===========================================================================

bool BASE_SESSION_CLASS::loadDefinition(string definitionFileName)

//  DESCRIPTION     : Load the given definition file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

    bool loaded = false;
    UINT nrOfDefinitionFiles = noDefinitionFiles();
    for (UINT index = 0; index < nrOfDefinitionFiles; index++)
    {
        DEFINITION_FILE_CLASS* pDefFile = getDefinitionFile(index);
		const char* defFileName = pDefFile->getFilename();
		if(!isPathAbsolute((char*)defFileName))
		{
			defFileName = pDefFile->getAbsoluteFilename();
		}
		if (_stricmp(defFileName, definitionFileName.c_str()) == 0)
        //if (strcmp(defFileName, definitionFileName.c_str()) == 0) //File comparision should be in case in-sensitive
        {
            loaded = true;
            result = true;
            break;
        }
    }
    // Skip loading if already loaded.
    if (loaded) return result;

	// allocate a new defintion file
	DEFINITION_FILE_CLASS	*definitionFile_ptr = new DEFINITION_FILE_CLASS(this, definitionFileName);

	if (definitionFile_ptr)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_NONE, 1, "Loading Definition File: -");
			loggerM_ptr->text(LOG_NONE, 1, "\t%s", definitionFile_ptr->getFilename());
		}

		// load defintion file
		result = definitionFile_ptr->Load();

		// on success add file to session list
		if (result)
		{
			addDefinitionFile(definitionFile_ptr);

			// check if the abstract mapping need setting up
			checkAbstractMappings();
		}
		else
		{
			// failed to load definition file
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_NONE, 1, "%s %s", VAL_PREFIX_FAILED, definitionFile_ptr->getFilename());
			}

			delete definitionFile_ptr;
		}
	}

	// return the load result
	return result;
}

//>>===========================================================================

bool BASE_SESSION_CLASS::unloadDefinition(string definitionFileName)

//  DESCRIPTION     : Unload the given definition file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	DEFINITION_FILE_CLASS* definitionFile_ptr = 0;
	UINT i = 0;
	bool found = false;
	AE_SESSION_CLASS ae_session;

	ae_session.SetName (this->getApplicationEntityName ());
	ae_session.SetVersion (this->getApplicationEntityVersion ());

	while ( i < definitionFileM_ptr.getSize() && !found)
	{
		definitionFile_ptr = definitionFileM_ptr[i];
		const char* defFileName = definitionFile_ptr->getFilename();
		if(!isPathAbsolute((char*)defFileName))
		{
			defFileName = definitionFile_ptr->getAbsoluteFilename();
		}
		if (_stricmp(defFileName, definitionFileName.c_str()) == 0)
		//if (strcmp(defFileName, definitionFileName.c_str()) == 0)//File comparision should be in case in-sensitive
		{
			// We have found the definition file
			// Remove it and pass the filename to the definition component for removal
			string sopClassUid;
			if (definitionFile_ptr->IsLoaded())
			{
				definitionFile_ptr->Unload();
				sopClassUid = DEFINITION->GetLastSopUidRemoved();
			}
			definitionFileM_ptr.removeAt(i);


			if (sopClassUid.length())
			{
				// check if we are dealing with a print sop class
				if (DEFINITION->IsPrintSop(sopClassUid, &ae_session))
				{
					// try to get containing meta sop class from sop class
					string metaSopUid = DEFINITION->GetMetaSopUid(sopClassUid);
					if (metaSopUid.length())
					{
						// meta sop class uid is the key for removal
						sopClassUid = metaSopUid;
					}
				}
beginLoop:
				// now check if the abstract mappings should be updated
				for (UINT i = 0; i < abstractMapM.getSize(); i++)
				{
					if (abstractMapM[i].isSopClassUid(sopClassUid))
					{
						// we need to remove this mapping
						if (loggerM_ptr)
						{
							loggerM_ptr->text(LOG_DEBUG, 1, "Removed abstract mapping for %s", sopClassUid.c_str());
						}
						abstractMapM.removeAt(i);

						// this is done on purpose as abstractMapM.getSize() is affected by the removal
						// of items
						goto beginLoop;
					}
				}
			}

			found = true;
		}

		++i;
	}

	// return the unload result
	return found;
}

//>>===========================================================================

void BASE_SESSION_CLASS::setDescriptionDirectory(string descriptionDirectory)

//  DESCRIPTION     : Set the Description Directory - directory where the test script descriptions are stored.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check if the filename is already absolute
	if (isPathAbsolute((char*) descriptionDirectory.c_str()))
	{
		descriptionDirectoryM = descriptionDirectory;
	}
	else
	{
		// start from session directory
		descriptionDirectoryM = sessionDirectoryM;

		// save the absolute results file root
		makeRootAbsolute(descriptionDirectoryM, (char*) descriptionDirectory.c_str());
	}

    //
    // Add backslash at the end of the descriptionDirectory directory path
    //
    if (descriptionDirectoryM[descriptionDirectoryM.length()-1] != '\\')
    {
        descriptionDirectoryM += "\\";
    }

	// log the description directory
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Description directory: \"%s\"", descriptionDirectoryM.c_str());
	}
}

//>>===========================================================================

string BASE_SESSION_CLASS::getSystemAppDomainBaseDirectory()

//  DESCRIPTION     : Get the System::AppDomain::BaseDirectory
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return systemAppDomainBaseDirectoryM;
}

//>>===========================================================================

void BASE_SESSION_CLASS::setSystemAppDomainBaseDirectory(string systemAppDomainBaseDirectory)

//  DESCRIPTION     : Set the System::AppDomain::BaseDirectory
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    systemAppDomainBaseDirectoryM = systemAppDomainBaseDirectory;
}

//>>===========================================================================

string BASE_SESSION_CLASS::getDescriptionDirectory()

//  DESCRIPTION     : Get the Description Directory - directory where the test script descriptions are stored.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return descriptionDirectoryM;
}

//>>===========================================================================

string BASE_SESSION_CLASS::getSopUid(const string &sopName)

//  DESCRIPTION     : Search for Uid matching given Sop name and return it.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	string mappedSopName = sopName;
	string mappedSopUid;

	// check if the given sop Name has been mapped
	for (UINT i = 0; i < abstractMapM.getSize(); i++)
	{
		if (abstractMapM[i].isMapped(sopName))
		{
			// we already have a mapping
			mappedSopName = abstractMapM[i].getMapping();

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Mapping abstract Uid %s to %s", sopName.c_str(), mappedSopName.c_str());
			}

			break;
		}
	}

	// first try getting mapping from Data Warehouse
	char *mappedValue_ptr = WAREHOUSE->getMappedValue((char*) mappedSopName.c_str(), loggerM_ptr);
	if (mappedValue_ptr)
	{
		// save mapped value
		mappedSopUid = mappedValue_ptr;
	}
	else
	{
		// now try from Definitions
		mappedSopUid = DEFINITION->GetSopUid(mappedSopName);
		if (!mappedSopUid.length())
		{
			// no mapping found - return original name
			mappedSopUid = mappedSopName;
		}
	}

#ifdef _DEBUG_SESSION
printf("\nDEBUG: BASE_SESSION_CLASS::getSopUid(sopName:= %s) -> mappedSopUid:= %s", sopName.c_str(), mappedSopUid.c_str());
#endif

	return mappedSopUid;
}

//>>===========================================================================

string BASE_SESSION_CLASS::getIodName(const string &iodName)

//  DESCRIPTION     : Search for a mapping for the given Iod name and return it.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	string mappedIodName = "";

	// check if the given iod name has been mapped
	for (UINT i = 0; i < abstractMapM.getSize(); i++)
	{
		if (abstractMapM[i].isMapped(iodName))
		{
			// we already have a mapping
			mappedIodName = abstractMapM[i].getMapping();

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_DEBUG, 1, "Mapping abstract Iod name %s to %s", iodName.c_str(), mappedIodName.c_str());
			}

			break;
		}
	}

#ifdef _DEBUG_SESSION
if (mappedIodName.length()) printf("\nDEBUG: BASE_SESSION_CLASS::getIodName(iodName:= %s) -> mappedIodName:= %s", iodName.c_str(), mappedIodName.c_str());
#endif

	// return iod name
	return mappedIodName;
}

//>>===========================================================================

string BASE_SESSION_CLASS::getIodNameFromDefinition(DIMSE_CMD_ENUM cmd, string uid)

//  DESCRIPTION     : Get Iod Name from the loaded definitions using the DIMSE command
//					: and SOP Class UID as the the search keys.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Use the current session settings for the AE_SESSION_CLASS.
//<<===========================================================================
{
	string iodName = "";
	AE_SESSION_CLASS ae_session;

	ae_session.SetName (this->getApplicationEntityName());
	ae_session.SetVersion (this->getApplicationEntityVersion());

	// Try to get the IOD Name from the loaded definitions
	DEFINITION->GetIodName(cmd, uid, &ae_session, iodName);

	return iodName;
}

//>>===========================================================================

string BASE_SESSION_CLASS::getFileNameFromSOPUID(DIMSE_CMD_ENUM cmd, string uid)

//  DESCRIPTION     : Get FIle Name from the loaded definitions using the DIMSE command
//					: and SOP Class UID as the the search keys.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Use the current session settings for the AE_SESSION_CLASS.
//<<===========================================================================
{
	string fileName = "";
	AE_SESSION_CLASS ae_session;

	ae_session.SetName (this->getApplicationEntityName());
	ae_session.SetVersion (this->getApplicationEntityVersion());

	// Try to get the File Name from the loaded definitions
	DEFINITION->GetSopFileName(cmd, uid, &ae_session, fileName);

	return fileName;
}

//>>===========================================================================

void BASE_SESSION_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set up the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	loggerM_ptr = logger_ptr;
	if (loggerM_ptr)
	{
		loggerM_ptr->setResultsRoot(resultsRootM);
		logMaskM = loggerM_ptr->getLogMask();

		// link in the activity reporter
		loggerM_ptr->setActivityReporter(activityReporterM_ptr);

        // link in the serializer
        loggerM_ptr->setSerializer(serializerM_ptr);
	}
}

//>>===========================================================================

void BASE_SESSION_CLASS::setActivityReporter(BASE_ACTIVITY_REPORTER *activityReporter_ptr)

//  DESCRIPTION     : Set up the activity reporter.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// save the activity reporter
	activityReporterM_ptr = activityReporter_ptr;

	// link it to the logger
	if (loggerM_ptr)
	{
		// link in the activity reporter
		loggerM_ptr->setActivityReporter(activityReporterM_ptr);
	}
}

//>>===========================================================================

void BASE_SESSION_CLASS::setSerializer(BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Set up the serializer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	serializerM_ptr = serializer_ptr;

    // link it to the logger
    if (loggerM_ptr)
    {
        // link in the serializer
        loggerM_ptr->setSerializer(serializer_ptr);
    }
}

//>>===========================================================================

void BASE_SESSION_CLASS::installCharacterSetAndImageDisplayFormatAndDefaultCommandSetDefData()

//  DESCRIPTION     : Install the Character Set and Image Display Format Data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // the character set and image display format data files are stored in the
    // System Application Domain Base Directory
	if (systemAppDomainBaseDirectoryM.length())
    {
		//
		// Add backslash at the end of the System Application Domain Base directory path
		//
		if (systemAppDomainBaseDirectoryM[systemAppDomainBaseDirectoryM.length()-1] != '\\')
		{
			systemAppDomainBaseDirectoryM += "\\";
		}

        // install the character set data
        string characterSetFilename = systemAppDomainBaseDirectoryM + DVT_V2_CHARACTER_SET_DATA_FILENAME;
        EXTCHARACTERSET->LoadCharacterSets(characterSetFilename);

        string imageDisplayFormatFilename = systemAppDomainBaseDirectoryM + DVT_V2_IMAGE_DISPLAY_FORMAT_DATA_FILENAME;
        MYPRINTER->loadImageDisplayFormats(imageDisplayFormatFilename);

		//Load default command def file
		string defFilename = systemAppDomainBaseDirectoryM + DVT_V2_DEFAULT_COMMANDSET_DEF_DATA_FILENAME;
		loadDefinition(defFilename);
    }
}

//>>===========================================================================

ABSTRACT_SESSION_CLASS::ABSTRACT_SESSION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	runtimeSessionTypeM = ST_UNKNOWN;
	sessionTypeM = ST_SCRIPT; // assume old Session Files are Script Sessions
	sessionFileVersionM = 0;
	sessionTitleM = "";
	isOpenM = false;
	filenameM = "";
	setSessionId(1);
	manufacturerM = "";
	modelNameM = "";
	softwareVersionsM = "";
	applicationEntityNameM = "";
	applicationEntityVersionM = "";
	testedByM = "";
	dateM = "";
	resultsRootM = "";
	counterM = 0;
	logMaskM = LOG_ERROR | LOG_WARNING | LOG_INFO;
	setLogger(NULL);
	setActivityReporter(NULL);
	setSerializer(NULL);
	isSessionStoppedM = false;
}

//>>===========================================================================

ABSTRACT_SESSION_CLASS::~ABSTRACT_SESSION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	// - call the base class cleanup()
	BASE_SESSION_CLASS::cleanup();
}


