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

#ifndef DCM_ATTRIBUTE_H
#define DCM_ATTRIBUTE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global component interface
#include "Ilog.h"				// Log component interface
#include "Iutility.h"			// Utility component interface
#include "IAttributeGroup.h"	// Attribute Group interface


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_VALUE_SQ_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;
class PRIVATE_ATTRIBUTE_HANDLER_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define MAX_LOGGED_VALUES 34


//>>***************************************************************************

class DCM_ATTRIBUTE_CLASS : public ATTRIBUTE_CLASS

//  DESCRIPTION     : Attribute class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT16							mappedGroupM;	
	UINT16 							mappedElementM;
	UINT32							receivedLengthM;
	TRANSFER_ATTR_VR_ENUM			transferVrM;
	int								nestingDepthM;
	bool							definedLengthM;
	bool							unVrDefinitionLookUpM;
	bool							ensureEvenAttributeValueLengthM;
	LOG_CLASS						*loggerM_ptr;
	DCM_ATTRIBUTE_GROUP_CLASS		*parentM_ptr;
	PRIVATE_ATTRIBUTE_HANDLER_CLASS	*pahM_ptr;

	bool encodeValue(DATA_TF_CLASS&, UINT32);
	
	bool decodeValue(DATA_TF_CLASS&, UINT32*);
	
	bool findBackslash(BYTE*, UINT, UINT, UINT*);

public:
	DCM_ATTRIBUTE_CLASS();
	DCM_ATTRIBUTE_CLASS(UINT16, UINT16);
	DCM_ATTRIBUTE_CLASS(UINT16, UINT16, ATTR_VR_ENUM);
	DCM_ATTRIBUTE_CLASS(UINT32, ATTR_VR_ENUM);

	~DCM_ATTRIBUTE_CLASS();

	void SetMappedGroup(UINT16 group)
		{ mappedGroupM = group; }

	void SetMappedElement(UINT16 element)
		{ mappedElementM = element; }

	void setDefineGroupLengths(bool);

	void setTransferVR(TRANSFER_ATTR_VR_ENUM);

	void setNestingDepth(int);

	void setDefinedLength(bool);

	void setUnVrDefinitionLookUp(bool flag)
		{ unVrDefinitionLookUpM = flag; }

	void setEnsureEvenAttributeValueLength(bool flag);

	bool replaceValue(int index, BASE_VALUE_CLASS *value_ptr);

	UINT16 GetMappedGroup() 
		{ return mappedGroupM; }

	UINT16 GetMappedElement() 
		{ return mappedElementM; }

	TRANSFER_ATTR_VR_ENUM getTransferVR()
		{ return transferVrM; }

	bool getDefinedLength()
		{ return definedLengthM; }

	bool getUnVrDefinitionLookUp()
		{ return unVrDefinitionLookUpM; }

	bool getEnsureEvenAttributeValueLength()
		{ return ensureEvenAttributeValueLengthM; }

	void addSqValue(DCM_VALUE_SQ_CLASS*);

	bool encode(DATA_TF_CLASS&);

	bool decode(DATA_TF_CLASS&, UINT16, UINT16, UINT32*);
		
	UINT32 getPaddedLength();

	UINT32 getReceivedLength()
		{ return receivedLengthM; }

    bool operator > (ATTRIBUTE_CLASS&);

	bool operator = (DCM_ATTRIBUTE_CLASS&);

	void setLogger(LOG_CLASS *logger_ptr);

	LOG_CLASS* getLogger()
		{ return loggerM_ptr; }

	void setParent(DCM_ATTRIBUTE_GROUP_CLASS *parent_ptr)
		{ parentM_ptr = parent_ptr; }

	DCM_ATTRIBUTE_GROUP_CLASS* getParent()
		{ return parentM_ptr; }

	void setPAH(PRIVATE_ATTRIBUTE_HANDLER_CLASS *pah_ptr)
		{ pahM_ptr = pah_ptr; }
};


#endif /* DCM_ATTRIBUTE_H */
