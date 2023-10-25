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

#ifndef ATTRIBUTE_REGISTER_H
#define ATTRIBUTE_REGISTER_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_ATTRIBUTE_CLASS;


//>>***************************************************************************

class DEF_ATTRIBUTE_REGISTER_CLASS

//  DESCRIPTION     : Attribute register class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_ATTRIBUTE_REGISTER_CLASS(DEF_ATTRIBUTE_CLASS*);

	~DEF_ATTRIBUTE_REGISTER_CLASS();

	DEF_ATTRIBUTE_CLASS *GetReferenceAttribute();

	void IncrementReferenceCount();

	void DecrementReferenceCount();

	int GetReferenceCount();

private:
	DEF_ATTRIBUTE_CLASS	referenceAttributeM;
	int	referenceCountM;
};


#endif /* ATTRIBUTE_REGISTER_H */
