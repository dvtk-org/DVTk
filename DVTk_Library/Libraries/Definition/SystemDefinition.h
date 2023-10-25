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

#ifndef DEF_SYSTEM_H
#define DEF_SYSTEM_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATIONS
//*****************************************************************************
class DEF_AE_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
typedef vector<DEF_AE_CLASS*>	DEF_AE_LIST;


//>>***************************************************************************

class DEF_SYSTEM_CLASS

//  DESCRIPTION     : System Definition Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_SYSTEM_CLASS();  
	DEF_SYSTEM_CLASS(const string, const string);
    ~DEF_SYSTEM_CLASS();
          
	void SetName(const string);

	void SetVersion(const string);

	string GetName();

	string GetVersion();

	void AddAE(DEF_AE_CLASS*);
		
	UINT GetNrAes();

	DEF_AE_CLASS* GetAE(UINT);
    DEF_AE_CLASS* GetAE(const string, const string);
	DEF_AE_CLASS* GetAE(const string);

private:
	string nameM;
	string versionM;
    DEF_AE_LIST aesM;
};


#endif /* SYSTEM_DEFINITION_H */
