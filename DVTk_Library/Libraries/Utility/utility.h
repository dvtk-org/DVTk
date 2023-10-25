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

#ifndef UTILITY_H
#define UTILITY_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global Component Interface

#ifdef _WINDOWS
#include <time.h>
#include <sys/timeb.h>
#else
#include <sys/types.h>
#include <sys/time.h>
#include <sys/timeb.h>
#endif


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

// define structure for the storage of the Image File indices
struct STORAGE_FILE_INDICES
{
	unsigned long   dcmIndex;
	unsigned long   rawIndex;
	unsigned long   pixIndex;
	unsigned long   resIndex;
};

#define STORAGE_FILE_INDEX_NAME		"storage.idx"
#define MAX_STORAGE_FILE_INDEX		99999

// define structure for DIMSE command mapping
struct T_DIMSE_MAP
{
	UINT16			commandField;
	char			*commandName;
	DIMSE_CMD_ENUM	commandId;
};

// define structure for attribute type mapping
struct T_TYPE_MAP
{
	ATTR_TYPE_ENUM	type;
	char*           typeName;
};

// define structure for value type mapping
struct T_VALUE_TYPE_MAP
{
	ATTR_VAL_TYPE_ENUM  type;
	char*               typeName;
};


//*****************************************************************************
//  FUNCTION DECLARATIONS
//*****************************************************************************
extern "C"
{
void byteCopy(BYTE*, BYTE*, UINT32);

bool byteCompare(BYTE*, BYTE*, UINT32);

void byteZero(BYTE*, UINT32);

void byteFill(BYTE*, BYTE, UINT32);

void bytePattern(BYTE*, UINT32);

UINT byteStrLen(BYTE*);

void resetUniq8odd();

BYTE uniq8odd();

bool isFileExtension(string, char*);

unsigned long getNextSfeIndex(string, STORAGE_FILE_EXTENSION_ENUM);

unsigned long getPixIndex(string);

unsigned long getResIndex(string);

void getStorageFilename(string, int, string&, STORAGE_FILE_EXTENSION_ENUM);

bool readPassword(char *buffer_ptr, int bufferLength);

char* mapCommandName(DIMSE_CMD_ENUM);

char* mapTypeName(ATTR_TYPE_ENUM type);

char* mapValueTypeName(ATTR_VAL_TYPE_ENUM type);

UINT16 mapCommandId(DIMSE_CMD_ENUM);

DIMSE_CMD_ENUM mapCommandField(UINT16);

bool isUID(const char*);

void createUID(char*, char*);

void createSS(char*);

void dayOfYear(int*, int*);

void monthDay(int, int, int*, int*);

void replaceDate(char*, int, int, int);

UINT mapDate(char*);

bool isDate(const char*);

void replaceTime(char*, int, int);

UINT mapTime(char*);

char* timeStamp();

bool stringValuesEqual(const BYTE*, const BYTE*, int, bool, bool);

int stringStrip(const BYTE*, int, bool, bool, const BYTE**);

bool decodeEscSeq(BYTE *escSeq_ptr, UINT maxSize, UINT &seqLength, bool &gr, bool &multibyte);

char* convertHex(char* string);

unsigned char* convertStringToByteArray(char* string, UINT32*);

char* convertDoubleBackslash(char* string);

double getEpsilon(char ds[]);

THREAD_TYPE getThreadId();

}

bool containsPath(string filename);

bool isAbsolutePath(string filename);

bool isRelativePath(string filename);

void reducePathname(char*);

string getFiletype(string);

string correctPathnameForOS(string);

string generateFullPath(string filename, string rootDir);

string getCurrentWorkingDirectory();

bool createDirectory(string);

#endif /* UTILITY_H */

