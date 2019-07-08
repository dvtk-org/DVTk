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

#ifndef DCM_VALUE_UL_H
#define DCM_VALUE_UL_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global component interface
#include "Ilog.h"				// Log component interface
#include "Iutility.h"			// Utility component interface
#include "Iwarehouse.h"			// Warehouse component interface
#include "IAttributeGroup.h"	// Attribute component interface


//>>***************************************************************************
class DCM_VALUE_UL_CLASS : public VALUE_UL_CLASS

//  DESCRIPTION     : DCM UL value class.
//  INVARIANT       :
//  NOTES           : Identifier is used to compute Offsets in Media.
//<<***************************************************************************
{
private:
	string	identifierM;

public:
	DCM_VALUE_UL_CLASS();

	~DCM_VALUE_UL_CLASS();

	void setIdentifier(string identifier) { identifierM = identifier; }

	string getIdentifier() { return identifierM; }

    bool operator = (DCM_VALUE_UL_CLASS&);
};

#endif /* DCM_VALUE_UL_H */
