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

#ifndef ATTRIBUTE_DEFINITION_H
#define ATTRIBUTE_DEFINITION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global component interface
#include "IAttributeGroup.h"	// Attribute Group interface
#include "Icondition.h"			// Condition interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class CONDITION_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;


//>>***************************************************************************

class DEF_ATTRIBUTE_CLASS : public ATTRIBUTE_CLASS

//  DESCRIPTION     : Attribute definition class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
public:
	DEF_ATTRIBUTE_CLASS();
		
	DEF_ATTRIBUTE_CLASS(UINT16, UINT16);

	DEF_ATTRIBUTE_CLASS(UINT16, UINT16, ATTR_VR_ENUM);
		
	~DEF_ATTRIBUTE_CLASS();
	
	void SetSecondVR(ATTR_VR_ENUM vr) { secondVrM = vr; }

	void SetVmMin(UINT min) { vmM.ATTR_VM_MIN = min; }

	void SetVmMax(UINT max) { vmM.ATTR_VM_MAX = max; }

	void SetVmRestriction(ATTR_VM_RESTRICT_ENUM restriction) { vmM.ATTR_VM_RESTRICTION = restriction; }
	
	void SetName(const string name);

	void SetCondition(CONDITION_CLASS *cond_ptr) { conditionM_ptr = cond_ptr; }

	void SetTextualCondition(const string message) { textual_conditionM = message; }

	UINT32 GetTag() { return ((((UINT32)GetGroup()) << 16) + ((UINT32)GetElement())); }

	ATTR_VR_ENUM GetSecondVR() { return secondVrM; }

	UINT GetVmMin() { return vmM.ATTR_VM_MIN; }

	UINT GetVmMax() { return vmM.ATTR_VM_MAX; }

    ATTR_VM_RESTRICT_ENUM GetVmRestriction() { return vmM.ATTR_VM_RESTRICTION; }

	string GetName()    { return nameM; }

	string GetXMLName() { return xml_nameM; }

	CONDITION_CLASS* GetCondition() { return conditionM_ptr; }

	string GetConditionResultMessage();

	string GetTextualCondition() { return textual_conditionM; }

	bool operator = (DEF_ATTRIBUTE_CLASS&);

	void SetImMin(UINT min) { imM.ATTR_IM_MIN = min; }

	void SetImMax(UINT max) { imM.ATTR_IM_MAX = max; }

	UINT GetImMin() { return imM.ATTR_IM_MIN; }

	UINT GetImMax() { return imM.ATTR_IM_MAX; }

private:
	ATTR_VR_ENUM secondVrM; //some attributes can have a second VR
	ATTR_VM_STRUCT vmM;
	ATTR_IM_STRUCT imM;
	string nameM;
	string xml_nameM;
	CONDITION_CLASS *conditionM_ptr;
	string textual_conditionM;
};


#endif /* ATTRIBUTE_DEFINITION_H */
