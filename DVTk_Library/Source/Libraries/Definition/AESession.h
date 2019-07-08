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

#ifndef SESSION_AE_H
#define SESSION_AE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//>>***************************************************************************

class AE_SESSION_CLASS

//  DESCRIPTION     : Application Entity Session Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	AE_SESSION_CLASS();
	AE_SESSION_CLASS(const string, const string);
	~AE_SESSION_CLASS();

	void SetName(const string name) { nameM = name; }
	void SetVersion(const string version) { versionM = version; }

	string GetName() { return nameM; }
	string GetVersion() { return versionM; }

private:
	string nameM;
	string versionM;
};

#endif /* SESSION_AE_H */

