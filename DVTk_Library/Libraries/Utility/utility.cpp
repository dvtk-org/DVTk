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
#include "utility.h"
#include <math.h>
#include <direct.h>
#include <io.h>

#ifdef _WINDOWS
#include <conio.h>
#endif



//*****************************************************************************
//  STATIC DECLARATIONS
//*****************************************************************************
// used to generate counters
static UINT32 uniqid = 0;

//
// DIMSE command map
//
static T_DIMSE_MAP	TDimseMap[] =
{
{C_ECHO_RQ,			"C-ECHO-RQ",			DIMSE_CMD_CECHO_RQ},
{C_ECHO_RSP,		"C-ECHO-RSP",			DIMSE_CMD_CECHO_RSP},
{C_FIND_RQ,			"C-FIND-RQ",			DIMSE_CMD_CFIND_RQ},
{C_FIND_RSP,		"C-FIND-RSP",			DIMSE_CMD_CFIND_RSP},
{C_GET_RQ,			"C-GET-RQ",				DIMSE_CMD_CGET_RQ},
{C_GET_RSP,			"C-GET-RSP",			DIMSE_CMD_CGET_RSP},
{C_MOVE_RQ,			"C-MOVE-RQ",			DIMSE_CMD_CMOVE_RQ},
{C_MOVE_RSP,		"C-MOVE-RSP",			DIMSE_CMD_CMOVE_RSP},
{C_STORE_RQ,		"C-STORE-RQ",			DIMSE_CMD_CSTORE_RQ},
{C_STORE_RSP,		"C-STORE-RSP",			DIMSE_CMD_CSTORE_RSP},
{C_CANCEL_RQ,		"C-CANCEL-RQ",			DIMSE_CMD_CCANCEL_RQ},
{N_ACTION_RQ,		"N-ACTION-RQ",			DIMSE_CMD_NACTION_RQ},
{N_ACTION_RSP,		"N-ACTION-RSP",			DIMSE_CMD_NACTION_RSP},
{N_CREATE_RQ,		"N-CREATE-RQ",			DIMSE_CMD_NCREATE_RQ},
{N_CREATE_RSP,		"N-CREATE-RSP",			DIMSE_CMD_NCREATE_RSP},
{N_DELETE_RQ,		"N-DELETE-RQ",			DIMSE_CMD_NDELETE_RQ},
{N_DELETE_RSP,		"N-DELETE-RSP",			DIMSE_CMD_NDELETE_RSP},
{N_EVENT_REPORT_RQ,	"N-EVENT-REPORT-RQ",	DIMSE_CMD_NEVENTREPORT_RQ},
{N_EVENT_REPORT_RSP,"N-EVENT-REPORT-RSP",	DIMSE_CMD_NEVENTREPORT_RSP},
{N_GET_RQ,			"N-GET-RQ",				DIMSE_CMD_NGET_RQ},
{N_GET_RSP,			"N-GET-RSP",			DIMSE_CMD_NGET_RSP},
{N_SET_RQ,			"N-SET-RQ",				DIMSE_CMD_NSET_RQ},
{N_SET_RSP,			"N-SET-RSP",			DIMSE_CMD_NSET_RSP},
{0x0000,			"UNKNOWN",				DIMSE_CMD_UNKNOWN}
};

//
// Attribute type map
//
static T_TYPE_MAP	TTypeMap[] =
{
{ATTR_TYPE_1,  "1"},
{ATTR_TYPE_1C, "1C"},
{ATTR_TYPE_2,  "2"},
{ATTR_TYPE_2C, "2C"},
{ATTR_TYPE_3,  "3"},
{ATTR_TYPE_3C, "3C"},
{ATTR_TYPE_3R, "3R"}
};

//
// Attribute value map
//
static T_VALUE_TYPE_MAP	TValueTypeMap[] =
{
{ATTR_VAL_TYPE_DEFINED,			"defined"},
{ATTR_VAL_TYPE_ENUMERATED,		"enumerated"},
{ATTR_VAL_TYPE_DEFINED_LIST,    "defined (list)"},
{ATTR_VAL_TYPE_ENUMERATED_LIST, "enumerated (list)"},
{ATTR_VAL_TYPE_NOVALUE,			"unknown value type"}
};


//
// Date conversions
//
#define YESTERDAY	"YESTERDAY"
#define TODAY		"TODAY"
#define TOMORROW	"TOMORROW"
#define TIMENOW		"NOW"

static char daytab[2][13] =
{
	{0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31},
	{0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
};


//>>===========================================================================

void byteCopy(BYTE *destination_ptr, BYTE *source_ptr, UINT32 length)

//  DESCRIPTION     : Copy source length bytes to destination.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// use underlying copy function
	memcpy((void *) destination_ptr, (void *) source_ptr, length);
}


//>>===========================================================================

bool byteCompare(BYTE *buffer1_ptr, BYTE *buffer2_ptr, UINT32 length)

//  DESCRIPTION     : Compare length bytes in buffer 1 & 2.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// use underlying compare function
	return (memcmp((void*) buffer1_ptr, (void*) buffer2_ptr, length) == 0) ? true : false;
}


//>>===========================================================================

void byteZero(BYTE *buffer_ptr, UINT32 length)

//  DESCRIPTION     : Initialise length bytes in buffer to zero.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// use underlying set function
	memset((void*) buffer_ptr, 0, length);
}


//>>===========================================================================

void byteFill(BYTE *buffer_ptr, BYTE value, UINT32 length)

//  DESCRIPTION     : Initialise length bytes in buffer to value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// use underlying set function
	memset((void*) buffer_ptr, (int) value, length);
}

//>>===========================================================================

void bytePattern(BYTE *buffer_ptr, UINT32 length)

//  DESCRIPTION     : Fill the given buffer with a pattern of 'a'..'z'. 
//					: Each pattern is terminated with a newline - CRLF.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// fill buffer with pattern
	BYTE value = (BYTE) 'a';

	for (UINT i = 0; i < length; i++)
	{
		// check for wrap around - after 'z' add newline - after newline start with 'a' again
		if (value == ('z' + 1))
		{
			// terminate with newline - CR
			value = (BYTE) CARRIAGERETURN;
			buffer_ptr[i] = value;
		}
		else if (value == CARRIAGERETURN)
		{
			// append LF onto CR
			value = LINEFEED;
			buffer_ptr[i] = value;
		}
		else if (value == LINEFEED)
		{
			// start at the begining
			value = (BYTE) 'a';
			buffer_ptr[i] = value++;
		}
		else
		{
			// save pattern
			buffer_ptr[i] = value++;
		}
	}
}

//>>===========================================================================

UINT byteStrLen(BYTE *buffer_ptr)

//  DESCRIPTION     : Return length of byte string.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return strlen((char*) buffer_ptr);
}

//>>===========================================================================

void resetUniq8odd()

//  DESCRIPTION     : Reset the counter value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// reset the counter value
	uniqid = 0;
}

//>>===========================================================================

BYTE uniq8odd()

//  DESCRIPTION     : Return an odd BYTE value counter.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// generate next odd number
	if (uniqid & 0x01) 
	{
		// make counter even
		uniqid++;
	}

	// make counter odd
	uniqid++;

	return (BYTE) (uniqid & 0x000000ff);
}

//>>===========================================================================

bool isFileExtension(string filename, char *extension_ptr)

//  DESCRIPTION     : Check if filename has given extension.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool extension = false;

	// check filename has given extension
	int extensionStart = filename.find(extension_ptr);
	int extensionLength = strlen(extension_ptr);

	if ((extensionStart) &&
		(extensionStart + extensionLength == (int) filename.length()))
	{
		// filename has the given extension
		extension = true;
	}

	// return the extension boolean
	return extension;
}

//>>===========================================================================

unsigned long getNextSfeIndex(string storageRoot, STORAGE_FILE_EXTENSION_ENUM extension)

//  DESCRIPTION     : Function to return the next Storage File Extension index.
//					  Some simple index persistance is maintained via the
//					  local file contents.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string					filename;
	STORAGE_FILE_INDICES	indices;
	unsigned long			index;

	// check if a storage root has been given
	if (storageRoot.length())
	{
		// use absolute filename
        filename = storageRoot;
        if (filename[filename.length()-1] != '\\') filename += "\\";
        filename += STORAGE_FILE_INDEX_NAME;
//#ifdef _WINDOWS
//		filename = storageRoot + "\\" + STORAGE_FILE_INDEX_NAME;
//#else
//		filename = storageRoot + "/" + STORAGE_FILE_INDEX_NAME;
//#endif
	}
	else
	{
		// just take the filename in the current directory
		filename = STORAGE_FILE_INDEX_NAME;
	}

	FILE *file_ptr = fopen(filename.c_str(), "r"); 
	if (file_ptr)
	{
		fscanf(file_ptr, "%d %d %d %d", &indices.dcmIndex, &indices.rawIndex, &indices.pixIndex, &indices.resIndex);
 		fclose(file_ptr);
	}
	else 
	{
		indices.dcmIndex = 1;
		indices.rawIndex = 1;
		indices.pixIndex = 1;
		indices.resIndex = 1;
	}

	switch(extension)
	{
	case SFE_DOT_DCM:
		if (indices.dcmIndex == MAX_STORAGE_FILE_INDEX)
		{
			indices.dcmIndex = 1;
		}

		index = indices.dcmIndex++;
		break;
	case SFE_DOT_RAW:
		if (indices.rawIndex == MAX_STORAGE_FILE_INDEX)
		{
			indices.rawIndex = 1;
		}

		index = indices.rawIndex++;
		break;
	case SFE_DOT_PIX:
		if (indices.pixIndex == MAX_STORAGE_FILE_INDEX)
		{
			indices.pixIndex = 1;
		}

		index = indices.pixIndex++;
		break;
	case SFE_DOT_RES:
	default:
		if (indices.resIndex == MAX_STORAGE_FILE_INDEX)
		{
			indices.resIndex = 1;
		}

		index = indices.resIndex++;
		break;
	}

    if (filename.length() == 0) assert(false);
	file_ptr = fopen(filename.c_str(), "w");
	if (file_ptr) 
	{
		fprintf(file_ptr, "%d %d %d %d", indices.dcmIndex, indices.rawIndex, indices.pixIndex, indices.resIndex);
		fclose(file_ptr);
	}

	return index;
}

//>>===========================================================================

unsigned long getPixIndex(string storageRoot)

//  DESCRIPTION     : Function to return the next PIX index.
//					  Some simple index persistance is maintained via the
//					  "imgfile.idx" file contents.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	unsigned long pixIndex = 0;
	if (storageRoot.length() == 0)
	{
		pixIndex = uniq8odd();
	}
	else
	{
		pixIndex = getNextSfeIndex(storageRoot, SFE_DOT_PIX);
	}
	return pixIndex;
}

//>>===========================================================================

unsigned long getResIndex(string storageRoot)

//  DESCRIPTION     : Function to return the next RES index.
//					  Some simple index persistance is maintained via the
//					  "imgfile.idx" file contents.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	return getNextSfeIndex(storageRoot, SFE_DOT_RES);
}

//>>===========================================================================

void getStorageFilename(string storageRoot, int sessionId, string &storageFilename, STORAGE_FILE_EXTENSION_ENUM extension)

//  DESCRIPTION     : Function to generate a filename for storing
//					  information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char filename[32];

	// get the next file index
	unsigned long index = getNextSfeIndex(storageRoot, extension);

	// format up the storage filename
	switch(extension) 
	{
	case SFE_DOT_DCM:
		sprintf(filename, "%dI%05d.dcm", sessionId, index);
		break;

	case SFE_DOT_RAW:
		sprintf(filename, "%dI%05d.raw", sessionId, index);
		break;
		
	case SFE_DOT_PIX:
	default:
		sprintf(filename, "%dI%05d.pix", sessionId, index);
		break;
	}

	// check if a storage root has been given
	if (storageRoot.length())
	{
		// return absolute storage filename
		// use absolute filename
        storageFilename = storageRoot;
        if (storageFilename[storageFilename.length()-1] != '\\') storageFilename += "\\";
        storageFilename += filename;
	}
	else
	{
		// return local storage filename
		storageFilename = filename;
	}
}

//>>===========================================================================

bool readPassword(char *buffer_ptr, int bufferLength)

//  DESCRIPTION     : Function to read a password from stdin.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Returns false if EOF, ctrl-C or ctrl-D pressed
//<<===========================================================================
{
	char *ptr = buffer_ptr;
	int chr;
	bool ret = true;

	bool looping = true;
	while (looping)
	{
#ifdef _WINDOWS
		chr = _getch(); // getchar with no echo
#else
		chr = getchar();
should use a non-echo mode here
#endif
		if ((chr == EOF) || (chr == '\n') || (chr == '\r') || (chr == '\f') ||
			(chr == '\03'/*ctrl-C*/) || (chr == '\04'/*ctrl-D*/))
		{
			// a termination character was received - exit
			if ((chr == EOF) || (chr == '\03'/*ctrl-C*/) || (chr == '\04'/*ctrl-D*/))
			{
				// abnormal termination
				ret = false;
			}
			break;
		}

		if (chr == '\b')
		{
			// backspace
			if (ptr == buffer_ptr)
			{
				// at the start of the buffer can't backspace
				putchar('\a'); // sound the bell
			}
			else
			{
				// backup
				ptr--;
				putchar('\b');
				putchar(' ');
				putchar('\b');
			}
		}
		else
		{
			// normal character
			if ((ptr - buffer_ptr + 1) < bufferLength)
			{
				// add to buffer
				*ptr++ = (char) chr;
#ifdef _WINDOWS
				putchar('*');
#endif
			}
			else
			{
				// no room in the buffer
				putchar('\a'); // sound the bell
			}
		}
	}

	// null terminate the buffer
	*ptr = 0;

	return ret;
}

//>>===========================================================================

char* mapCommandName(DIMSE_CMD_ENUM commandId)

//  DESCRIPTION     : Function to map the command id to the corresponding command name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int i = 0;
	
	while ((TDimseMap[i].commandId != commandId)
	  && (TDimseMap[i].commandId != DIMSE_CMD_UNKNOWN))
	{
		i++;
	}

	// return matching command name
	return TDimseMap[i].commandName;
}

//>>===========================================================================

char* mapTypeName(ATTR_TYPE_ENUM type)

//  DESCRIPTION     : Function to map the attribute type tp a meaningfull string
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int i = 0;
	
	while (TTypeMap[i].type != type)
	{
		i++;
	}

	// return matching command name
	return TTypeMap[i].typeName;
}

//>>===========================================================================

char* mapValueTypeName(ATTR_VAL_TYPE_ENUM type)

//  DESCRIPTION     : Function to map the attribute type tp a meaningfull string
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int i = 0;
	
	while (TValueTypeMap[i].type != type)
	{
		i++;
	}

	// return matching command name
	return TValueTypeMap[i].typeName;
}

//>>===========================================================================

UINT16 mapCommandId(DIMSE_CMD_ENUM commandId)

//  DESCRIPTION     : Function to map the command id to the corresponding command field.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int i = 0;
	
	while ((TDimseMap[i].commandId != commandId)
	  && (TDimseMap[i].commandId != DIMSE_CMD_UNKNOWN))
	{
		i++;
	}

	// resturn matching command field
	return TDimseMap[i].commandField;
}

//>>===========================================================================

DIMSE_CMD_ENUM mapCommandField(UINT16 commandField)

//  DESCRIPTION     : Function to map the command field to the corresponding command id.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int i = 0;
	
	while ((TDimseMap[i].commandField != commandField)
	  && (TDimseMap[i].commandField != 0x0000))
	{
		i++;
	}

	// return matching command id
	return TDimseMap[i].commandId;
}

//>>===========================================================================

bool isUID(const char *uid_ptr)

//  DESCRIPTION     : Check that the given string is a valid UID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool lastCharDigit = false;
	bool lastCharPeriod = false;
	bool lastCharZero = false;

	if (!uid_ptr) return false;

	for (UINT i = 0; i < strlen(uid_ptr); i++) 
	{
		// check for digits and period characters
		if ((!isdigit(uid_ptr[i])) &&
			(uid_ptr[i] != PERIOD)) 
		{
			return false;
		}

		// if we have a period - it must be preceeded by a digit
		if (uid_ptr[i] == PERIOD)
		{
			if (!lastCharDigit) return false;
			lastCharPeriod = true;
			lastCharZero = false;
			lastCharDigit = false;
		}
		else 
		{
			// check for leading zero
			// - maybe we should log this
			if ((lastCharPeriod) &&
				(uid_ptr[i] == '0'))
			{
				lastCharZero = true;
			}
			else if (lastCharZero) 
			{
				return false;
			}
			lastCharPeriod = false;
			lastCharDigit = true;
		}
	}

	// may not end with a period
	if (lastCharPeriod)
		return false;

	// seems OK
	return true;
}

//>>===========================================================================

void createUID(char *uid_ptr, char *uidRoot_ptr)

//  DESCRIPTION     : Creates UID by appending the current time to the given root.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    struct timeb 	timeValue;
    ftime(&timeValue);

    if ((uidRoot_ptr == NULL) ||
        (strlen(uidRoot_ptr) == 0))
    {
		char temp_str[MAX_STRING_LEN];
		sprintf(uid_ptr, "%s.%ld.", IMPLEMENTATION_CLASS_UID, timeValue.time);
		sprintf(temp_str, "%u.%u", timeValue.millitm, uniq8odd());
		strcat(uid_ptr, temp_str);
    }
    else
    {
		char temp_str[MAX_STRING_LEN];
	    sprintf(uid_ptr, "%s.%ld.", uidRoot_ptr, timeValue.time);
		sprintf(temp_str, "%u.%u", timeValue.millitm, uniq8odd());
		strcat(uid_ptr, temp_str);
    }
}

//>>===========================================================================

void createSS(char *uid_ptr)

//  DESCRIPTION     : Creates Short string for unique accession number by appending random number generator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
    struct timeb 	timeValue;
    ftime(&timeValue);

    sprintf(uid_ptr, "%d%d",  timeValue.time, timeValue.millitm);    
}

//>>===========================================================================

void dayOfYear(int *year, int *day)

//  DESCRIPTION     : Function to convert today's date into a number of days
//					: from the beginning of the year.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// get date
	time_t t = time(NULL);
	if (t == -1)
	{
		*year = 1900;
		*day = 1;
		return;
	}

	// convert to local time
	tm *time_ptr = localtime(&t);
	if (!time_ptr) 
	{
		*year = 1900;
		*day = 1;
		return;
	}

	// set returned year and day
	*year = 1900 + time_ptr->tm_year;
	*day = time_ptr->tm_mday;

	int leap = (*year % 4 == 0) && (*year % 100 != 0) || (*year % 400 == 0);

	for (int i = 1; i < time_ptr->tm_mon + 1; i++)
	{
		*day += daytab[leap][i];
	}
}

//>>===========================================================================

void monthDay(int year, int yearDay, int *pMonth, int *pDay)

//  DESCRIPTION     : Function to convert the given day number into a month
//					: and day.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check on leap year
	int leap = (year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0);

	// count the days
	int i;
	for (i = 1; yearDay > daytab[leap][i]; i++)
	{
		yearDay -= daytab[leap][i];
	}

	*pMonth = i;
	*pDay = yearDay;
}

//>>===========================================================================

void replaceDate(char *dateString_ptr, int replacePosition, int replaceLength, int dayOffset)

//  DESCRIPTION     : Function to replace the string position with the
//					: appropriate date.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int year, month, day, yearDay;

	// get the day number
	dayOfYear(&year, &yearDay);

	// modify it by the given offset
	yearDay += dayOffset;

	// now convert that back into a calender date
	monthDay(year, yearDay, &month, &day);

	// shift any remaining date characters
	// - need 8 free characters for our date
	int length = strlen(dateString_ptr);
	if (replaceLength < 8) 
	{
		// shift forward
		for (int i = length - 1; i >= replacePosition + replaceLength; i--) 
		{
			dateString_ptr[i + 8 - replaceLength] = dateString_ptr[i];
		}
		dateString_ptr[length + 8 - replaceLength] = '\0';
	}
	else if (replaceLength > 8)
	{
		// shift backward
		for (int i = replacePosition + replaceLength; i < length; i++)
		{
			dateString_ptr[i + 8 - replaceLength] = dateString_ptr[i];
		}
		dateString_ptr[length + 8 - replaceLength] = '\0';
	}

	// finally add our date
	char ldate[16];
	sprintf(ldate, "%04d%02d%02d", year, month, day);
	strncpy(&dateString_ptr[replacePosition], ldate, 8);
}

//>>===========================================================================

UINT mapDate(char *dateString_ptr)

//  DESCRIPTION     : Function to check if date mapping is required.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *pChr;

	// check for mapping from one of the supported date terms
	// - check for YESTERDAY
	if ((pChr = strstr(dateString_ptr, YESTERDAY)) != NULL)
	{
		// replace YESTERDAY with yesterday's date	
		replaceDate(dateString_ptr, (int)(pChr - &dateString_ptr[0]), strlen(YESTERDAY), -1);
	}

	// - could be range YESTERDAY-YESTERDAY
	if ((pChr = strstr(dateString_ptr, YESTERDAY)) != NULL)
	{
		// replace YESTERDAY with yesterday's date	
		replaceDate(dateString_ptr, (int)(pChr - &dateString_ptr[0]), strlen(YESTERDAY), -1);
	}

	// check for TODAY
	if ((pChr = strstr(dateString_ptr, TODAY)) != NULL) 
	{
		// replace TODAY with today's date
		replaceDate(dateString_ptr, (int)(pChr - &dateString_ptr[0]), strlen(TODAY), 0);
	}

	// - could be range TODAY-TODAY
	if ((pChr = strstr(dateString_ptr, TODAY)) != NULL) 
	{
		// replace TODAY with today's date
		replaceDate(dateString_ptr, (int)(pChr - &dateString_ptr[0]), strlen(TODAY), 0);
	}

	// check for TOMORROW
	if ((pChr = strstr(dateString_ptr, TOMORROW)) != NULL)
	{
		// replace TOMORROW with tomorrow's date
		replaceDate(dateString_ptr, (int)(pChr - &dateString_ptr[0]), strlen(TOMORROW), +1);
	}

	// - could be range TOMORROW-TOMORROW
	if ((pChr = strstr(dateString_ptr, TOMORROW)) != NULL)
	{
		// replace TOMORROW with tomorrow's date
		replaceDate(dateString_ptr, (int)(pChr - &dateString_ptr[0]), strlen(TOMORROW), +1);
	}

	// return length of date string
	return strlen(dateString_ptr);
}

//>>===========================================================================

bool isDate(const char* date_ptr)

//  DESCRIPTION     : Function to check whether the date is valid.
//  PRECONDITIONS   : YYYYMMDD format; "99991231" Null-terminated
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Only range check. No calender check.
//<<===========================================================================
{
    // check length
    if (strlen(date_ptr) != 8) return false;
    // check for digit format
    for (UINT i = 0; i < strlen(date_ptr); i++)
    {
        if (!isdigit(date_ptr[i])) return false;
    }
    // Check year format
    //char year_str[4+1]; // Null-terminated
    //strncpy(year_str, date_ptr + 0, 4);
    //year_str[4] = NULL; // Explicitely terminate the string
    //UINT year = atoi(year_str);
    // Check month format
    char month_str[2+1]; // Null-terminated
    strncpy(month_str, date_ptr + 4, 2);
    month_str[2] = NULL; // Explicitely terminate the string
    UINT month = atoi(month_str);
    if (month > 12) return false;
    // Check day format
    char day_str[2+1]; // Null-terminated
    strncpy(day_str, date_ptr + 4 + 2, 2);
    day_str[2] = NULL; // Explicitely terminate the string
    UINT day = atoi(day_str);
    if (day > 31) return false;
    return true;
}

//>>===========================================================================

void replaceTime(char *timeString_ptr, int replacePosition, int replaceLength)

//  DESCRIPTION     : Function to replace the string position with the
//					: appropriate time.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int hour = 0, minute = 0, second = 0;

	// get the time
	time_t t = time(NULL);
	if (t != -1) 
	{
		tm *time_ptr = localtime(&t);
		if (time_ptr)
		{
			hour = time_ptr->tm_hour;
			minute = time_ptr->tm_min;
			second = time_ptr->tm_sec;
		}
	}

	// shift any remaining time characters
	// - need 6 free characters for our time
	int length = strlen(timeString_ptr);
	for (int i = length - 1; i >= replacePosition + replaceLength; i--)
	{
		timeString_ptr[i + 6 - replaceLength] = timeString_ptr[i];
	}
	timeString_ptr[length + 6 - replaceLength] = '\0';

	// finally add our time
	char ltime[8];
	sprintf(ltime, "%02d%02d%02d", hour, minute, second);
	strncpy(&timeString_ptr[replacePosition], ltime, 6);
}

//>>===========================================================================

UINT mapTime(char *timeString_ptr)

//  DESCRIPTION     : Function to check if time mapping is required.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char *pChr;

	// check for TIMENOW mapping
	if ((pChr = strstr(timeString_ptr, TIMENOW)) != NULL)
	{
		// replace TIMENOW with time now
		replaceTime(timeString_ptr, (int)(pChr - &timeString_ptr[0]), strlen(TIMENOW));
	}

	// return length of time string
	return strlen(timeString_ptr);
}

//>>===========================================================================

char* timeStamp()

//  DESCRIPTION     : Returns a string version of the current time.  The value 
//					: is stored in a static buffer maintained by the function
//  PRECONDITIONS   :
//  POSTCONDITIONS  : Static buffer updated
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	static char buffer[30];

	buffer[0] = '\0';

	// get the time
	time_t t = time(NULL);
	if (t != -1) 
	{
		tm *time_ptr = localtime(&t);
		if (time_ptr)
		{
			strftime(buffer, sizeof(buffer), "%X", time_ptr);
		}
	}

	return buffer;
}

//>>===========================================================================

bool stringValuesEqual(const BYTE* refValue, const BYTE* srcValue, int maxLength, bool leadingSpace, bool trailingSpace)

//  DESCRIPTION     : Function to compare two "string" based VR types of the 
//                    (maximum) given length taking into account the 
//                    significance of any leading/trailing spaces.					
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	const BYTE	*refValueP, *srcValueP;
	int		refLength, srcLength;

	// get significant reference string content
	refLength = stringStrip(refValue, maxLength, leadingSpace, trailingSpace, &refValueP);

	// get significant received string content			
	srcLength = stringStrip(srcValue, maxLength, leadingSpace, trailingSpace, &srcValueP);

	// significant lengths should be the same
	if (refLength != srcLength) 
	{
		return false;
	}

	// compare the significant string parts
	if (memcmp(refValueP, srcValueP, refLength) == 0) 
	{
		return true;
	}

	return false;
}

//>>===========================================================================

int stringStrip(const BYTE *string,	int maxLength, bool leadingSpace, bool trailingSpace, const BYTE **stringP)

//  DESCRIPTION     : Function to strip the given string down to the significant 
//				      characters (leading and tailing spaces may be stripped off).	
//					  Return a pointer to the first significant character and the	
//					  stripped string length.						
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	const BYTE *chr;
	int		length = 0;

	//get normal string length					
	chr = string;
	while ((*chr++ != NULLCHAR) && (length < maxLength)) 
	{
		length++;
	}

	//check for insignificant leading spaces		
	chr = string;
	if (leadingSpace == false) 
	{
		while ((*chr == ' ') && (length > 0)) 
		{
			chr++;
			length--;
		}
	}
	*stringP = chr;

	//check for insignificant trailing spaces			
	if ((trailingSpace == false) && (length > 0)) 
	{
		chr = (*stringP) + length - 1;
		while ((*chr == ' ') && (length > 0)) 
		{
			chr--;
			length--;
		}
	}

	return length;
}

//>>===========================================================================

bool decodeEscSeq(BYTE *escSeq_ptr, UINT maxSize, UINT &seqLength, bool &gr, bool &multibyte)

//  DESCRIPTION     : decodes a given escape sequence 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : does not use the character set definitions.  Uses the standard
//					: ISO escape sequence rules.
//					: returns false if the sequence could not be decoded
//<<===========================================================================
{
	char *seq_ptr = (char *)escSeq_ptr;

	if (maxSize < 2)
		return false;

	switch (*seq_ptr)
	{
		case '(':
		case ',':
			multibyte = false;
			gr = false;
			seq_ptr++;
			break;
		
		case ')':
		case '-':
			multibyte = false;
			gr = true;
			seq_ptr++;
			break;
		
		case '$':
			multibyte = true;
			seq_ptr++;
			if (*seq_ptr == '(')
			{
				gr = false;
				seq_ptr++;
			}
			else if (*seq_ptr == ')')
			{
				gr = true;
				seq_ptr++;
			}
			else
			{
				gr = false;
			}
			break;

		default:
			return false;
	}

	if (*seq_ptr == '!')
		seq_ptr++;

	if ((*seq_ptr < START_GL_CHAR_SET) || (*seq_ptr > END_GL_CHAR_SET))
		return false;


	// we have a valid sequence
	seqLength = seq_ptr - (char *)escSeq_ptr + 1;

	return true;
}

//>>===========================================================================

static int GetHexValue(char *hexString, int length)

//  DESCRIPTION     : Convert the given hexString into an integer value.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : A '?' is returned for an invalid hexString.
//<<===========================================================================
{
	int i, digit, value = 0;

	for (i = 0; i < length; i++) 
	{
		if (('A' <= hexString[i]) && 
			(hexString[i] <= 'F')) 
		{
			digit = hexString[i] - '7';
		} 	
		else if (('a' <= hexString[i]) && 
				 (hexString[i] <= 'f')) 
		{
			digit = hexString[i] - 'W';
		}
		else if (('0' <= hexString[i]) && 
				 (hexString[i] <= '9')) 
		{
			digit = hexString[i] - '0';
		}
		else return ((int) '?');

		value = (value * 16) + digit;
	}

	return value;
}

//>>===========================================================================

char* convertHex(char* string)

//  DESCRIPTION     : Convert any hex values (indicated by (XX) or \XX) to integers
//  PRECONDITIONS   :
//  POSTCONDITIONS  : The string is converted in place and returned
//  EXCEPTIONS      : 
//  NOTES           : A '?' is returned for an invalid hexString.
//<<===========================================================================
{
	char* read_ptr = string;
	char* write_ptr = string;

	while (*read_ptr != NULLCHAR) 
	{
		// Check for [XX]
		if ((*read_ptr == OPENBRACKET) &&
			(*(read_ptr + 1) != NULLCHAR) &&
			(*(read_ptr + 2) != NULLCHAR) &&
			(*(read_ptr + 3) == CLOSEBRACKET)) 
		{
			char c = (char)GetHexValue((read_ptr + 1), 2);
			*write_ptr = c;
			read_ptr += 3;
		}
		// Check for \XX
		else if (*read_ptr == '\\') 
		{
			read_ptr++;
			if (*read_ptr == '\\') 
			{
				*write_ptr = *read_ptr;
			}
			else 
			{
				char c = (char)GetHexValue(read_ptr, 2);
				*write_ptr = c;
				read_ptr++;
			}
		}
		else 
		{
			*write_ptr = *read_ptr;
		}
		read_ptr++;
		write_ptr++;
	}

	*write_ptr = NULLCHAR;

	return string;
}

//>>===========================================================================

unsigned char* convertStringToByteArray(char* string, UINT32* length)

//  DESCRIPTION     : convert the incoming string support NULL - return a pointer
//					 to the byte array (unsigned char*) and it's length
//  PRECONDITIONS   :
//  POSTCONDITIONS  : The string is converted in place and returned
//  EXCEPTIONS      : 
//  NOTES           : A '?' is returned for an invalid hexString.
//<<===========================================================================
{
	char* read_ptr = string;
	UINT inLength = strlen(string);
	unsigned char* byteArray = (unsigned char *)malloc ((sizeof (unsigned char)) * (inLength + 1));
	int i=0;

	while (*read_ptr != NULLCHAR) 
	{
		// Check for [XX]
		if ((*read_ptr == OPENBRACKET) &&
			(*(read_ptr + 1) != NULLCHAR) &&
			(*(read_ptr + 2) != NULLCHAR) &&
			(*(read_ptr + 3) == CLOSEBRACKET)) 
		{
			char c = (char)GetHexValue((read_ptr + 1), 2);
			byteArray[i] = c;
			read_ptr += 3;
		}
		// Check for \XX
		else if (*read_ptr == '\\') 
		{
			read_ptr++;
			if (*read_ptr == '\\') 
			{
				byteArray[i] = *read_ptr;
			}
			else 
			{
				char c = (char)GetHexValue(read_ptr, 2);
				byteArray[i] = c;
				read_ptr++;
			}
		}
		else 
		{
			byteArray[i] = *read_ptr;
		}
		read_ptr++;
		i++;
	}

	*length = i;

	return byteArray;
}

//>>===========================================================================

char* convertDoubleBackslash(char* string)

//  DESCRIPTION     : Convert any double backslashes into a single backslash.
//  PRECONDITIONS   :
//  POSTCONDITIONS  : The string is converted in place and returned.
//  EXCEPTIONS      : 
//  NOTES           : This is required for backwards compatibility with ADVT.
//<<===========================================================================
{
	char* read_ptr = string;
	char* write_ptr = string;

	while (*read_ptr != NULLCHAR) 
	{
		// check for backslash
		if (*read_ptr == '\\') 
		{
			// copy single backslash
			*write_ptr = *read_ptr;
			read_ptr++;

			// check for second backslash
			if (*read_ptr == '\\') 
			{
				// skip second backslash
				read_ptr++;
			}
		}
		else 
		{
			// just copy other characters
			*write_ptr = *read_ptr;
			read_ptr++;
		}
		write_ptr++;
	}

	*write_ptr = NULLCHAR;

	return string;
}

//>>===========================================================================

double getEpsilon(char ds[])

//  DESCRIPTION     : Determines the allowed error from the input string
//	                  using the following algorithm:					
//	                  - if DS contains no fraction then epsilon = 0.5 * 10^exp	
//	                  - if DS contains a fraction then 0.5 (order of frac) * 10^exp	
//  PRECONDITIONS   : 
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This function assumes that the DS represents a normal 	
//		              float: -number[.frac][e-exp]				
//<<===========================================================================
{
	char	*fract_ptr;
	char	*exp_ptr;
	double	epsilon = 0.5;


	// the number of digits in the fraction determine the precision	
	fract_ptr = strchr(ds, '.');
	if (fract_ptr != NULL) 
	{
		for (fract_ptr++; isdigit(*fract_ptr); fract_ptr++) 
		{
			epsilon /= 10;
		}
	}

	// determine if the exponent is given				
	exp_ptr = strchr(ds, 'E');
	if (exp_ptr == NULL) 
	{
		exp_ptr = strchr(ds, 'e');
	}

	if (exp_ptr != NULL) 
	{

		int	e = 1;

		// check sign of exponent				
		exp_ptr++;
		if (*exp_ptr == '-') 
		{
			e = -e;
			exp_ptr++;
		}
		else if (*exp_ptr == '+') 
		{
			exp_ptr++;
		}
		else 
		{
			/* void */
		}

		//determine exponent value				
		for (; isdigit(*exp_ptr); exp_ptr++) 
		{
			e = e * 10 + ((*exp_ptr) - '0');
		}

		/* epsilon = epsilon * pow(10, e); MIGRATION_IN_PROGRESS */ epsilon = epsilon * pow(10, (double)e);
	}
	
	return epsilon;
}

//>>===========================================================================

THREAD_TYPE getThreadId()

//  DESCRIPTION     : Returns the current thread ID.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
#if defined(_WINDOWS)
	return GetCurrentThreadId();
#elif defined(_POSIX_THREADS)
	return pthread_self();
#endif
}

//>>===========================================================================

bool containsPath(string filename)

//  DESCRIPTION     : Check if file name contains a directory component - 
//					: relative or absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check if a path separator appears in the filename
	return (filename.find(PATH_SEP) == string::npos) ? false : true;
}

//>>===========================================================================

bool isAbsolutePath(string filename)

//  DESCRIPTION     : Check if file name is an absolute path 
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	bool result = false;

	// check for valid pathname
	if (filename.length() == 0) return false;

#ifdef _WINDOWS
	// check for absolute Windows pathname
	if (( filename[0] == BACKSLASH) ||		// format: \pathname
		((filename[1] == COLON) &&			// format: C:\pathname
		 (filename[2] == BACKSLASH)) ||
		( filename[1] == COLON))			// format: C:
	{
		// pathname is absolute
		result = true;
	}
#else
	// check for absolute Unix pathname
	if ((filename[0] == SLASH) ||			// format: /pathname
		(filename[0] == TILDE))				// format: ~pathname
	{
		// pathname is absolute
		result = true;
	}
#endif

	// return result
	return result;
}

//>>===========================================================================

bool isRelativePath(string filename)

//  DESCRIPTION     : Check if file name contains a relative path
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// relative pathname is not absolute
	return !isAbsolutePath(filename);
}

//>>===========================================================================

void reducePathname(char *pathname_ptr)

//  DESCRIPTION     : Reduce the pathname to its shortest form.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	UINT separator[20];
	UINT i, index = 0;
	char pathname[_MAX_DIR];

	// don't reduce if we start with a dot!
	if (pathname_ptr[0] == '.') 
	{
		for (i = 0; i < strlen(pathname_ptr); i++) 
		{
			if ((pathname_ptr[i] == BACKSLASH) ||
				(pathname_ptr[i] == SLASH)) 
			{
#ifdef _WINDOWS
				pathname_ptr[i] = BACKSLASH;
#else
				pathname_ptr[i] = SLASH;
#endif
			}
		}	
		return;
	}

	strcpy(pathname, pathname_ptr);
	UINT length = strlen(pathname);

	i = 0;
	while (i < length) 
	{
		if ((pathname[i] == BACKSLASH) ||
			(pathname[i] == SLASH)) 
		{
			if ((i + 2 < length) &&
				(pathname[i + 1] == '.') &&
				(pathname[i + 2] == '.')) 
			{
				i += 3;
				// reduce pathname
				if (index) index--;
				strcpy(&pathname[separator[index]], &pathname[i]);
				length = strlen(pathname);
				i = separator[index] - 1;
			}
			else if ((pathname[i + 1] == '.') &&
                     ((pathname[i + 2] == BACKSLASH) || (pathname[i + 2] == SLASH)))
            {
                // The following construction has been found:
                // dir/./dir
                // Remove the ./ part:
                // dir/dir
                strcpy (&(pathname[i]), &(pathname[i+2]));
            }
            else
			{
				separator[index++] = i;
				if (index == 20) return;
#ifdef _WINDOWS
				pathname[i] = BACKSLASH;
#else
				pathname[i] = SLASH;
#endif
			}
		}
		i++;
	}

	strcpy(pathname_ptr, pathname);
}

//>>===========================================================================

string getFiletype(string fullname)

//  DESCRIPTION     : Returns the file type for the given path
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string filetype;

	// search from the back of the string
	string::size_type pos = fullname.rfind('.');

	// position indicates the last '.' in the fullname
	if (pos != string::npos)
	{
		filetype = fullname.substr(pos + 1);
	}
	else
	{
		filetype = "";
	}

	return filetype;
}

//>>===========================================================================

string correctPathnameForOS(string pathname)

//  DESCRIPTION     : Corrects the pathname format for the native operating system
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char buffer[_MAX_DIR];

	// copy original pathname
	strcpy(buffer, pathname.c_str());

	// set directory delimiters
	for (unsigned int i = 0; i < strlen(buffer); i++) 
	{
		if ((buffer[i] == BACKSLASH) ||
			(buffer[i] == SLASH)) 
		{
#ifdef _WINDOWS
			buffer[i] = BACKSLASH;
#else
			buffer[i] = SLASH;
#endif
		}	
	}

	// set up return pathname
	string correctedPathname = buffer;

	// and return it
	return correctedPathname;
}


//>>===========================================================================

string generateFullPath(string filename, string rootDir)

//  DESCRIPTION     : Returns a full path for the given filename.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string correctedFilename = correctPathnameForOS(filename);

	if (isAbsolutePath(correctedFilename))
	{
		// already have the full path, return it
		return correctedFilename;
	}
	else if (rootDir.length() == 0)
	{
		// don't have a root dir, just return what we have
		return correctedFilename;
	}
	else
	{
		string correctedRootDir = correctPathnameForOS(rootDir);

		// add the root dir to the beginning of the filename to make the full path
        string fullpath = correctedRootDir;
        if (correctedRootDir[correctedRootDir.length()-1] != PATH_SEP)
            fullpath += PATH_SEP;
        fullpath += correctedFilename;

		char fullpathBuffer[FILENAME_LENGTH];
		strcpy(fullpathBuffer, fullpath.c_str());

		reducePathname(fullpathBuffer);

		fullpath = fullpathBuffer;

		return fullpath;
	}
}


//>>===========================================================================

string getCurrentWorkingDirectory()

//  DESCRIPTION     : Get the current working directory from the OS.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	string cwd;
	char buffer[_MAX_PATH];

   // get the current working directory
   if (_getcwd(buffer, _MAX_PATH))
   {
	   // save the directory
	   cwd = buffer;
   }

   // return the directory
   return cwd;
}

//>>===========================================================================

bool createDirectory(string fullname)

//  DESCRIPTION     : Create the directory
//  PRECONDITIONS   :
//  POSTCONDITIONS  : All the needed paths for the directory will be created
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (fullname.find(PATH_SEP) != string::npos)
	{
		// save the current directory
		string cwd=getCurrentWorkingDirectory();

#ifdef _WINDOWS
		_chdir("\\");
#else
	    need to add Unix code here;  //to do
#endif

		string pathname = fullname;
		char drive = pathname.at(0);
		if ((toupper(drive) >='A') && (toupper(drive) <='Z'))	
		{
#ifdef _WINDOWS
			if(_chdrive(toupper(drive)-'A'+1)!=0)
			{
				_chdir(cwd.c_str());
				return false;
			}
			_chdir("\\");
#else
			need to add Unix code here;
#endif
		}
		string::size_type pos2=0;
		// search from the back of the string
		string::size_type pos = fullname.rfind(PATH_SEP);

		// position indicates the start of the filename in the full path
		if (pos != string::npos)
		{
			pathname = fullname.erase(pos); //remove file name from path
			if (pathname.at(pos-1)==PATH_SEP)
			{
				pathname=fullname.erase(pos-1);
			}
		}

		//look for network path and then the first directory
		pos = pathname.find(PATH_SEP);
		if ((pathname.at(0) ==PATH_SEP) && (pathname.at(1)==PATH_SEP))//check for network path
		{
			pos= 0;
			pos2= pathname.find_first_of(PATH_SEP, 2);
			pos2= pathname.find_first_of(PATH_SEP, pos2+1);
			int len =pos2-pos;
			string  dir = pathname.substr(pos,len);
			pos=pos2;
			if (dir.length()!=0)
			{
#ifdef _WINDOWS
				{
					if (_chdir(dir.c_str()))
					{
						_mkdir(dir.c_str());
						if(_chdir(dir.c_str())!=0)
						{
							_chdir(cwd.c_str());
							return false;
						}
					}
				}
#else
				need to add Unix code here;
#endif
			}
		}
		
		//parse through string looking for directory names
		while (pos2!=string::npos)
		{
			pos2= pathname.find_first_of(PATH_SEP, pos+1);
			int len =pos2-pos-1;
			string  dir = pathname.substr(pos+1,len);
			pos=pos2;
			if (dir.length()!=0)
			{
#ifdef _WINDOWS
				{
					if (_chdir(dir.c_str()))
					{
						_mkdir(dir.c_str());
						if(_chdir(dir.c_str())!=0)
						{
							_chdir(cwd.c_str());
							return false;
						}
					}
				}
#else
				need to add Unix code here;
#endif
			}
		}

		// restore the current directory
#ifdef _WINDOWS
		_chdir(cwd.c_str());
#else
		need to add Unix code here;
#endif
	}

	return true;
}
