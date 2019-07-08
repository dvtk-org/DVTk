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

#ifndef STORED_SOP_INSTANCE_H
#define STORED_SOP_INSTANCE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;


//>>***************************************************************************

class STORED_SOP_INSTANCE_CLASS

//  DESCRIPTION     : Class used to store the sop class/instance detail.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string	sopClassUidM;
	string	sopInstanceUidM;
	UINT	countM;
	bool	committedM;

public:
	STORED_SOP_INSTANCE_CLASS();
	STORED_SOP_INSTANCE_CLASS(string, string);
	
	~STORED_SOP_INSTANCE_CLASS();

	void setSopClassUid(string sopClassUid)
		{ sopClassUidM = sopClassUid; }

	void setSopInstanceUid(string sopInstanceUid)
		{ sopInstanceUidM = sopInstanceUid; }

	void commit() { committedM = true; }

	string getSopClassUid() { return sopClassUidM; }

	string getSopInstanceUid() { return sopInstanceUidM; }

	void incrementCount() 
		{ countM++; }

	UINT getCount() 
		{ return countM; }

	void log(LOG_CLASS*);
};

#endif /* STORED_SOP_INSTANCE_H */
