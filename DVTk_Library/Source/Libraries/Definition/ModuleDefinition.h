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

#ifndef DEF_MODULE_H
#define DEF_MODULE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "AttributeGroupDefinition.h"


//>>***************************************************************************

class DEF_MODULE_CLASS : public DEF_ATTRIBUTE_GROUP_CLASS

//  DESCRIPTION     : Module definition class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_MODULE_CLASS();
	DEF_MODULE_CLASS(const string);
	DEF_MODULE_CLASS(const string, const MOD_USAGE_ENUM);

	~DEF_MODULE_CLASS();
};

#endif /* DEF_MODULE_H */
