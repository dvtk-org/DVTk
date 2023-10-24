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

//*****************************************************************************
//  FILENAME        : ConditionDefinition.h
//  PACKAGE         : DVT
//  COMPONENT       : DEFINITION
//  DESCRIPTION     : Condition Definition Class
//  COPYRIGHT(c)    : 2000, Philips Electronics N.V.
//                    2000, Agfa Gevaert N.V.
//*****************************************************************************
#ifndef CONDITION_DEFINITION_H
#define CONDITION_DEFINITION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;
class DEF_COND_NODE_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;


//>>***************************************************************************

class DEF_CONDITION_CLASS

//  DESCRIPTION     : Defines Conditions for the DICOM definition files.
//                    Conditions can be applied for example for Attributes, 
//                    Modules, Macro's and Items
//  INVARIANT       :
//  NOTES           : 
//                    
//<<***************************************************************************
{
    private:
        DEF_COND_NODE_CLASS* nodeM_ptr;

	public:
		DEF_CONDITION_CLASS();
		~DEF_CONDITION_CLASS();

		void SetNode(DEF_COND_NODE_CLASS* node_ptr) { nodeM_ptr = node_ptr; }

		string GetResultMessage();

		bool Evaluate(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*, LOG_CLASS*);

		void Log(LOG_CLASS*);
};


#endif /* CONDITION_DEFINITION_H */

