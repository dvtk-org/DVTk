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
#include "Iutility.h"                   // Utility library
#include "specific_character_set.h"

//*****************************************************************************
//  INTERNAL DECLARATIONS
//*****************************************************************************

//>>===========================================================================			  

SPECIFIC_CHARACTER_SET_CLASS::SPECIFIC_CHARACTER_SET_CLASS()

//  DESCRIPTION     : Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================			  
{
    // constructor activities
}

//>>===========================================================================			  

SPECIFIC_CHARACTER_SET_CLASS::~SPECIFIC_CHARACTER_SET_CLASS()

//  DESCRIPTION     : Destructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================			  
{
    // destructor activities
    ClearCharacterSets();
}

//>>===========================================================================			  

void SPECIFIC_CHARACTER_SET_CLASS::ClearCharacterSets()

//  DESCRIPTION     : Clears all character sets
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================			  
{
	character_setsM.clear();
}

//>>===========================================================================	

void SPECIFIC_CHARACTER_SET_CLASS::AddCharacterSet(UINT val_nr, const string& char_set)

//  DESCRIPTION     : Adds the defined term for a character set
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================			  
{
	// remove any training spaces from the char_set
	char buffer[64];
	strcpy(buffer, char_set.c_str());
	int length = strlen(buffer);
	while ((length > 0) &&
		(buffer[length-1] == ' '))
	{
		length--;
	}
	buffer[length] = '\0';

	CHARACTER_SET_STRUCT character_set;
	character_set.value_nrM = val_nr;
	character_set.character_setM = buffer;
	character_setsM.push_back(character_set);
}

//>>===========================================================================			  

int SPECIFIC_CHARACTER_SET_CLASS::GetNrCharacterSets()

//  DESCRIPTION     : Returns number of defined terms for character sets
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================			  
{
	return character_setsM.size();
}

//>>===========================================================================

string SPECIFIC_CHARACTER_SET_CLASS::GetCharacterSetName(UINT nr)

//  DESCRIPTION     : Returns number of defined teerms for character sets
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================			  
{
	bool found = false;
	string name;

	vector<CHARACTER_SET_STRUCT>::iterator it;
	it = character_setsM.begin();
	while (it < character_setsM.end() && !found)
	{
		if ((*it).value_nrM == nr)
		{
			name = (*it).character_setM;
			found = true;
		}
		++it;
	}

	return name;
}

//>>===========================================================================			  

bool SPECIFIC_CHARACTER_SET_CLASS::IsValidCharacterSet(const string& def_term)

//  DESCRIPTION     : Checks if the defined term is valid during this validation
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================			  
{
	bool found = false;

	vector<CHARACTER_SET_STRUCT>::iterator it;
	it = character_setsM.begin();
	while ( it < character_setsM.end() && !found)
	{
		if (stringValuesEqual((BYTE*)((*it).character_setM.c_str()), (BYTE*)def_term.c_str(), CS_LENGTH, false, false))
		{
			found = true;
		}
		++it;
	}

	// if first value of defined terms is empty
	// but multi-valued, default OK
	if (!found)
	{
		if ((character_setsM.size() > 1) && (def_term == "ISO 2022 IR 6"))
		{
			found = true;
		}
	}

	return found;
}
