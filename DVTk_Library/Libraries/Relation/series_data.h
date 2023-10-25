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

#ifndef SERIES_DATA_H
#define SERIES_DATA_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class SOP_INSTANCE_DATA_CLASS;
class LOG_CLASS;


//>>***************************************************************************

class SERIES_DATA_CLASS

//  DESCRIPTION     : Series Data Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string instanceUidM;
	ARRAY<SOP_INSTANCE_DATA_CLASS*>	sopInstanceDataM;

  public:
	SERIES_DATA_CLASS(string);
		
	~SERIES_DATA_CLASS();
		
	string getInstanceUid() 
		{ return instanceUidM; }

	UINT noSopInstances() 
		{ return sopInstanceDataM.getSize(); }

	SOP_INSTANCE_DATA_CLASS *getSopInstanceData(UINT i) 
		{ return sopInstanceDataM[i]; }

	void addSopInstanceData(SOP_INSTANCE_DATA_CLASS *sopInstanceData_ptr)
		{ sopInstanceDataM.add(sopInstanceData_ptr); }

	SOP_INSTANCE_DATA_CLASS *search(string);

	void log(LOG_CLASS*);
};

#endif /* SERIES_DATA_H */
