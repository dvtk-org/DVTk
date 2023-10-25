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

#ifndef SPECIFIC_CHARACTER_SET_H
#define SPECIFIC_CHARACTER_SET_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"


//>>***************************************************************************

class SPECIFIC_CHARACTER_SET_CLASS

//  DESCRIPTION     : Specific Character Set description used during extended character
//                  : set validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
	public:
		SPECIFIC_CHARACTER_SET_CLASS();
        ~SPECIFIC_CHARACTER_SET_CLASS();

		void   ClearCharacterSets();

		void   AddCharacterSet(UINT val_nr, const string& char_set);

		int    GetNrCharacterSets();

		string GetCharacterSetName(UINT nr);

		bool   IsValidCharacterSet(const string& def_term);
	
	private:
		typedef struct
		{
			UINT   value_nrM;
			string character_setM;
		} CHARACTER_SET_STRUCT;

		vector<CHARACTER_SET_STRUCT> character_setsM; // list of defined terms of available
		                                              // character sets
		                                              // the contents are determined by the
		                                              // specific character set attribute
};

#endif /* SPECIFIC_CHARACTER_SET_H */
