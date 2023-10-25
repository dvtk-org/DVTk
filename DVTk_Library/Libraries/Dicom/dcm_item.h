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

#ifndef DCM_ITEM_H
#define DCM_ITEM_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_attribute_group.h"
#include "private_attribute.h"	


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DCM_DATASET_CLASS;


//>>***************************************************************************

class DCM_ITEM_CLASS : public DCM_ATTRIBUTE_GROUP_CLASS

//  DESCRIPTION     : DICOM item class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	bool				valueByReferenceM;
	bool				definedLengthM;
	UINT32				lengthM;
	UINT16				introducerGroupM;
	UINT16				introducerElementM;
	UINT32				introducerLengthM;
	UINT16				delimiterGroupM;
	UINT16				delimiterElementM;
	UINT32				delimiterLengthM;

public:
	DCM_ITEM_CLASS();

	~DCM_ITEM_CLASS();

	void setDefinedLength(bool flag)
		{ definedLengthM = flag;
		  setDefineSqLengths(flag); }

	void setIntroducerGroup(UINT16 group)
		{ introducerGroupM = group; }

	void setIntroducerElement(UINT16 element)
		{ introducerElementM = element; }

	void setDelimiterGroup(UINT16 group)
		{ delimiterGroupM = group; }

	void setDelimiterElement(UINT16 element)
		{ delimiterElementM = element; }

	void setValueByReference(bool flag)
		{ valueByReferenceM = flag; }

	bool isDefinedLength()
		{ return definedLengthM; }

	UINT16 getIntroducerGroup()
		{ return introducerGroupM; }

	UINT16 getIntroducerElement()
		{ return introducerElementM; }

	UINT32 getIntroducerLength()
		{ return introducerLengthM; }

	UINT16 getDelimiterGroup()
		{ return delimiterGroupM; }

	UINT16 getDelimiterElement()
		{ return delimiterElementM; }

	UINT32 getDelimiterLength()
		{ return delimiterLengthM; }

	bool getValueByReference()
		{ return valueByReferenceM; }

	UINT32 computeLength(TS_CODE);

	void computeItemOffsets(DATA_TF_CLASS&);

	void computeItemOffsets(string);

	bool encode(DATA_TF_CLASS&);

	bool decode(DATA_TF_CLASS&, UINT16 lastGroup = ITEM_GROUP, UINT16 lastElement = LAST_ELEMENT, UINT32 *length_ptr = NULL);

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);

	bool morph(DCM_DATASET_CLASS*);

    bool operator = (DCM_ITEM_CLASS&);

	void clone(DCM_ITEM_CLASS*);
};

#endif /* DCM_ITEM_H */

