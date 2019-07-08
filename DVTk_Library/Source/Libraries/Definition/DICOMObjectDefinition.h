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

#ifndef DICOMOBJECTDEF_H
#define DICOMOBJECTDEF_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_ATTRIBUTE_CLASS;
class DEF_MODULE_CLASS;


//>>***************************************************************************

class DEF_DICOM_OBJECT_CLASS

//  DESCRIPTION     : Dicom object definition class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_DICOM_OBJECT_CLASS();
	DEF_DICOM_OBJECT_CLASS(const string);
	~DEF_DICOM_OBJECT_CLASS();

	void SetName(const string name) { nameM = name; }

	void SetDimseCmd(DIMSE_CMD_ENUM dimseCmd) { dimseCmdM = dimseCmd; }

	void AddModule(DEF_MODULE_CLASS*);

	string GetName() { return nameM; }

	DIMSE_CMD_ENUM GetDimseCmd() { return dimseCmdM; }
		
	UINT GetNrModules() { return modulesM.size(); }

	DEF_MODULE_CLASS* GetModule(UINT);
    DEF_MODULE_CLASS* GetModule(UINT16, UINT16);

    DEF_ATTRIBUTE_CLASS* GetAttribute(UINT16, UINT16);
    DEF_ATTRIBUTE_CLASS* GetAttribute(string, UINT16, UINT16);

private:
	string nameM;		
	DIMSE_CMD_ENUM dimseCmdM;
    vector<DEF_MODULE_CLASS*> modulesM;
};

#endif /* DICOMOBJECTDEF_H */

