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

#ifndef PATIENT_DATA_H
#define PATIENT_DATA_H


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class STUDY_DATA_CLASS;
class LOG_CLASS;


//>>***************************************************************************

class PATIENT_DATA_CLASS

//  DESCRIPTION     : Patient Data Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string patientNameM;
	string patientIdM;
	ARRAY<STUDY_DATA_CLASS*> studyDataM;

public:
	PATIENT_DATA_CLASS(string, string);

	~PATIENT_DATA_CLASS();

	string getPatientName() 
		{ return patientNameM; }

	string getPatientId() 
		{ return patientIdM; }

	UINT noStudies()
		{ return studyDataM.getSize(); }

	STUDY_DATA_CLASS *getStudyData(UINT i) 
		{ return studyDataM[i]; }

	void addStudyData(STUDY_DATA_CLASS *studyData_ptr)
		{ studyDataM.add(studyData_ptr); }

	STUDY_DATA_CLASS *search(string);

	void log(LOG_CLASS*);
};

#endif /* PATIENT_DATA_H */
