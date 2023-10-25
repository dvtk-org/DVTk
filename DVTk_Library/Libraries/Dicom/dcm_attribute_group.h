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

#ifndef DCM_ATTRIBUTE_GROUP_H
#define DCM_ATTRIBUTE_GROUP_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global component interface
#include "Ilog.h"				// Log component interface
#include "Iutility.h"			// Utility component interface
#include "Iwarehouse.h"			// Warehouse component interface
#include "IAttributeGroup.h"	// Attribute Group interface
#include "dcm_attribute.h"


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_ATTRIBUTE_CLASS;
class DCM_VALUE_SQ_CLASS;


//>>***************************************************************************

class DCM_ATTRIBUTE_GROUP_CLASS : public BASE_WAREHOUSE_ITEM_DATA_CLASS, public ATTRIBUTE_GROUP_CLASS

//  DESCRIPTION     : Generic DICOM object class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	string							identifierM;
	int								nestingDepthM;
	bool							defineGroupLengthsM;
	BYTE							encodePresentationContextIdM;
	bool							populateWithAttributesM;
	bool							validateOnReceiveM;
	bool							unVrDefinitionLookUpM;
	bool							ensureEvenAttributeValueLengthM;
	UINT32							offsetM; // the offset from the beginning of 
											 // the file or the network message
	UINT16							pixelRepresentationM;
	DCM_VALUE_SQ_CLASS				*parentM_ptr;
	PRIVATE_ATTRIBUTE_HANDLER_CLASS	*pahM_ptr;

	DCM_ATTRIBUTE_CLASS* setAttribute(UINT32, ATTR_VR_ENUM);

	void updatePixelData();

public:
	DCM_ATTRIBUTE_GROUP_CLASS();

	virtual ~DCM_ATTRIBUTE_GROUP_CLASS();

	void setGroupLengths(TS_CODE);

	void addGroupLengths();

	virtual UINT32 computeLength(TS_CODE);

	void setNestingDepth(int nestingDepth = 0);

	void setTransferVR(TRANSFER_ATTR_VR_ENUM);

	void setUnVrDefinitionLookUp(bool flag)
	{
		unVrDefinitionLookUpM = flag;
	}

	bool getUnVrDefinitionLookUp()
	{
		return unVrDefinitionLookUpM;
	}

	void setEnsureEvenAttributeValueLength(bool flag);

	bool getEnsureEvenAttributeValueLength()
	{
		return ensureEvenAttributeValueLengthM;
	}

	void setOffset(UINT32 offset)
		{ offsetM = offset; }

	UINT32 getOffset()
		{ return offsetM; }

	void setPixelRepresentation(UINT16 value)
		{ pixelRepresentationM = value; }

	UINT16 getPixelRepresentation()
		{ return pixelRepresentationM; }

	void setParent(DCM_VALUE_SQ_CLASS *parent_ptr)
		{ parentM_ptr = parent_ptr; }

	DCM_VALUE_SQ_CLASS* getParent()
		{ return parentM_ptr; }

	void merge(DCM_ATTRIBUTE_GROUP_CLASS*);

	void cloneAttributes(DCM_ATTRIBUTE_GROUP_CLASS*);

	void addAttribute(DCM_ATTRIBUTE_CLASS*);

	DCM_ATTRIBUTE_CLASS *GetAttribute(UINT16, UINT16);

	DCM_ATTRIBUTE_CLASS *GetMappedAttribute(UINT16, UINT16, bool parentOnly = false);

    DCM_ATTRIBUTE_CLASS *GetAttribute(int);
    
	DCM_ATTRIBUTE_CLASS *GetAttributeByTag(UINT32);

	bool containsAttributesFromGroup(UINT16);

	virtual void computeItemOffsets(DATA_TF_CLASS&);

	void computeItemOffsets(string);

	virtual bool encode(DATA_TF_CLASS&);

	virtual bool decode(DATA_TF_CLASS&, UINT16 lastGroup = LAST_GROUP, UINT16 lastElement = LAST_ELEMENT, UINT32 *length_ptr = NULL);

	void setIdentifier(string identifier)
		{ identifierM = identifier; }

	void setDefineGroupLengths(bool);

	void setDefineSqLengths(bool);

	void setEncodePresentationContextId(BYTE pcId)
		{ encodePresentationContextIdM = pcId; }

	void setPopulateWithAttributes(bool flag)
		{ populateWithAttributesM = flag; }

	void setValidateOnReceive(bool flag)
		{ validateOnReceiveM = flag; }
	
	int getNestingDepth() 
		{ return nestingDepthM; }

	const char* getIdentifier()
		{ 
			const char* identifier_ptr = NULL;

			if (identifierM.length())
			{
				identifier_ptr = identifierM.c_str(); 
			}

			return identifier_ptr;
		}

	bool getDefineGroupLengths()
		{ return defineGroupLengthsM; }

	BYTE getEncodePresentationContextId()
		{ return encodePresentationContextIdM; }

	bool getPopulateWithAttributes()
		{ return populateWithAttributesM; }

	bool getValidateOnReceive()
		{ return validateOnReceiveM; }

	void setValue(UINT32, BYTE*);
	void setAEValue(UINT32, string);
	void addATValue(UINT32, UINT32);
	void setCSValue(UINT32, string);
	void setSHValue(UINT32, string, bool replaceFirst = true);
	void setDAValue(UINT32, string);
	void setISValue(UINT32, string);
	void setLOValue(UINT32, string);
	void setPNValue(UINT32, string);
	void setSQValue(UINT32, DCM_VALUE_SQ_CLASS*);
	void setOBValue(UINT32, UINT, UINT);
	void setOBValue(UINT32, UINT, UINT, BYTE);
	void setSTValue(UINT32, string);
	void setTMValue(UINT32, string);
	void setUIValue(UINT32, string, bool replaceFirst = true);
	void setULValue(UINT32, UINT32, bool replaceFirst = true);
	void setUSValue(UINT32, UINT16, bool replaceFirst = true);

	bool getATValue(UINT32 tag, int index, UINT32 *data_ptr);
	bool getCSValue(UINT32, BYTE*, UINT);
	bool getCSValue(UINT32 tag, string& data, int index = 0);
	bool getSHValue(UINT32 tag, string& data, int index = 0);
	bool getDAValue(UINT32, BYTE*, UINT);
	bool getISValue(UINT32, INT32*);
    bool getLOValue(UINT32, string&);
	bool getPNValue(UINT32, string&);
	bool getSQValue(UINT32, DCM_VALUE_SQ_CLASS**);
	bool getSTValue(UINT32, BYTE*, UINT);
	bool getTMValue(UINT32, BYTE*, UINT);
	bool getUIValue(UINT32, string&);
    bool getULValue(UINT32, UINT32*);
	bool getUSValue(UINT32, UINT16*);

	bool operator == (DCM_ATTRIBUTE_GROUP_CLASS&);

	bool operator != (DCM_ATTRIBUTE_GROUP_CLASS&);

	virtual bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);

	void setLogger(LOG_CLASS*);

	void setPAH(PRIVATE_ATTRIBUTE_HANDLER_CLASS*);

    PRIVATE_ATTRIBUTE_HANDLER_CLASS* getPAH();
};


//*****************************************************************************
//  FUNCTION DECLARATIONS
//*****************************************************************************
extern "C"
{
void logStatus(LOG_CLASS*, UINT16);
}

#endif /* DCM_ATTRIBUTE_GROUP_H */

