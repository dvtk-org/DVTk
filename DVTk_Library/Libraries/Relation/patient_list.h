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

#ifndef PATIENT_LIST_H
#define PATIENT_LIST_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_DATASET_CLASS;
class LOG_CLASS;
class PATIENT_DATA_CLASS;


//>>***************************************************************************

class PATIENT_LIST_CLASS

//  DESCRIPTION     : Patient List Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	ARRAY<PATIENT_DATA_CLASS*>	patientDataM;

	UINT noPatients(void);

	PATIENT_DATA_CLASS *getPatientData(UINT i);

	void addPatientData(PATIENT_DATA_CLASS *patData_ptr);

	PATIENT_DATA_CLASS *search(string id, string name, LOG_CLASS *logger_ptr);

public:
	PATIENT_LIST_CLASS();

	~PATIENT_LIST_CLASS();

	void cleanup(void);

	void analyseStorageDataset(DCM_DATASET_CLASS *dataset_ptr, LOG_CLASS *logger_ptr);

	void log(LOG_CLASS *logger_ptr);
};

#endif /* PATIENT_LIST_H */
