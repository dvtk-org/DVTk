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

#ifndef SOP_INSTANCE_DATA_H
#define SOP_INSTANCE_DATA_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;


//>>***************************************************************************

class SOP_INSTANCE_DATA_CLASS

//  DESCRIPTION     : SOP Instance Data Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string instanceUidM;
	string frameOfReferenceUidM;
	string imageTypeValue3M;
	string refImageInstanceUidM;
	UINT countM;

public:
	SOP_INSTANCE_DATA_CLASS(string, string, string, string);

	~SOP_INSTANCE_DATA_CLASS();

	void incrementCount() 
		{ countM++; }

	string getInstanceUid() 
		{ return instanceUidM; }

	UINT getCount() 
		{ return countM; }

	void log(LOG_CLASS*);
};

#endif /* SOP_INSTANCE_DATA_H */
