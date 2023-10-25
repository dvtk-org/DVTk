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

#ifndef DEF_DETAIL_H
#define DEF_DETAIL_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//>>***************************************************************************

class DEF_DETAILS_CLASS

//  DESCRIPTION     : Definition details class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_DETAILS_CLASS();

	~DEF_DETAILS_CLASS();

	void SetAEName(const string);

	void SetAEVersion(const string);

	void SetSOPClassName(const string);

	void SetSOPClassUID(const string);

	void SetIsMetaSOPClass(bool);

	string GetAEName();

	string GetAEVersion();

	string GetSOPClassName();

	string GetSOPClassUID();

	bool GetIsMetaSOPClass();

private:
	string AENameM;
	string AEVersionM;
	string SOPClassNameM;
	string SOPClassUIDM;
	bool   IsMetaSOPClassM;
};		

#endif /* DEF_DETAIL_H */
