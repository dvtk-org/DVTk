// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
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

#ifndef ACSE_AB_SOURCE_H
#define ACSE_AB_SOURCE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "ACSE_parameter.h"

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//>>***************************************************************************

class ACSE_AB_SOURCE_CLASS : public ACSE_PARAMETER_CLASS

//  DESCRIPTION     : Abort Request Source class for ACSE parameter validation.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	ACSE_AB_SOURCE_CLASS();
	~ACSE_AB_SOURCE_CLASS();
	
protected:
	bool checkSyntax();
	
	bool checkRange();
	
	bool checkReference(string);
};

#endif /* ACSE_AB_SOURCE_H */
