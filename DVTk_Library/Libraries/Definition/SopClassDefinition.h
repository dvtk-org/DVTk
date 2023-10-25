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

#ifndef DEF_SOPCLASS_H
#define DEF_SOPCLASS_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_DATASET_CLASS;
class DEF_MACRO_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
typedef vector<DEF_DATASET_CLASS*>	DEF_DATASET_LIST;
typedef vector<DEF_MACRO_CLASS*>	DEF_MACRO_LIST;


//>>***************************************************************************

class DEF_SOPCLASS_CLASS

//  DESCRIPTION     : SOP Class Definition Class
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_SOPCLASS_CLASS();

	DEF_SOPCLASS_CLASS(const string, const string);

	~DEF_SOPCLASS_CLASS();

	string GetUid() { return uidM; }

	string GetName() { return nameM; }

	bool AddDataset(DEF_DATASET_CLASS*);

	void AddMacro(DEF_MACRO_CLASS*);

	UINT GetNrDatasets() { return datasetsM.size(); }

	DEF_DATASET_CLASS* GetDataset(UINT i) { return datasetsM[i]; }
	DEF_DATASET_CLASS* GetDataset(DIMSE_CMD_ENUM);
	DEF_DATASET_CLASS* GetDataset(const string);
	DEF_DATASET_CLASS* GetDataset(const string, DIMSE_CMD_ENUM);

private:
	string uidM;
	string nameM;
	DEF_DATASET_LIST datasetsM;
	DEF_MACRO_LIST macrosM;
};

#endif /* DEF_SOPCLASS_H */

