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

#ifndef BASE_CONFIRMER_H
#define BASE_CONFIRMER_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"				// global component interface

//*****************************************************************************
//  FORWARD DECLARATIONS
//*****************************************************************************

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************

//>>***************************************************************************
//<<abstract>>
class BASE_CONFIRMER

//  DESCRIPTION     : BASE_CONFIRMER class.
//  INVARIANT       :
//  NOTES           : Derived classes should implement the defined methods.
//<<***************************************************************************
{
public:
    virtual void ConfirmInteraction() = 0;
public:
    ~BASE_CONFIRMER();
};

#endif /* BASE_CONFIRMER_H */

