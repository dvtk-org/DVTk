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

#ifndef STUDY_DATA_H
#define STUDY_DATA_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;
class SERIES_DATA_CLASS;


//>>***************************************************************************

class STUDY_DATA_CLASS

//  DESCRIPTION     : Study Data Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string instanceUidM;
	ARRAY<SERIES_DATA_CLASS*> seriesDataM;

public:
	STUDY_DATA_CLASS(string);

	~STUDY_DATA_CLASS();

	string getInstanceUid() 
		{ return instanceUidM; }

	UINT noSeries()
		{ return seriesDataM.getSize(); }

	SERIES_DATA_CLASS *getSeriesData(UINT i)
		{ return seriesDataM[i]; }

	void addSeriesData(SERIES_DATA_CLASS *seriesData_ptr)
		{ seriesDataM.add(seriesData_ptr); }

	SERIES_DATA_CLASS *search(string);

	void log(LOG_CLASS*);
};

#endif /* STUDY_DATA_H */
