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

#ifndef METASOPCLASSDEF_H
#define METASOPCLASSDEF_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  CONSTANTS AND TYPE DEFS
//*****************************************************************************
typedef map<string, string> METASOPCLASS_MAP;


//>>***************************************************************************

class DEF_METASOPCLASS_CLASS

//  DESCRIPTION     :
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_METASOPCLASS_CLASS(const string, const string);

	~DEF_METASOPCLASS_CLASS();

	void AddSopClass(const string, const string);

	bool HasSopClass(const string);
		
	inline string GetUid() { return uidM; };

	inline string GetName() { return nameM; };

	UINT GetNoSopClasses();
	void GetSopClass(UINT, string&, string&);

    string GetImageBoxSopUid();
	string GetImageBoxSopName();

private:
	string uidM;
	string nameM;

	METASOPCLASS_MAP sopClassesM;
};


#endif /* METASOPCLASSDEF_H */
