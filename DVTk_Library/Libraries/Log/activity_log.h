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

#ifndef ACTIVITY_LOG_H
#define ACTIVITY_LOG_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "base_log.h"


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************


//>>***************************************************************************

class ACTIVITY_LOG_CLASS : public LOG_CLASS

//  DESCRIPTION     : Base Line Log Display Class.
//  INVARIANT       :
//  NOTES           : Display methods.
//<<***************************************************************************
{
public:
	ACTIVITY_LOG_CLASS();
	~ACTIVITY_LOG_CLASS();

	void displayText();
};

#endif /* ACTIVITY_LOG_LOG_H */
