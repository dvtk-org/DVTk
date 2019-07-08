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

#ifndef ITEM_HANDLE_H
#define ITEM_HANDLE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "dcm_item.h"


//>>***************************************************************************

class SEQUENCE_REF_CLASS

//  DESCRIPTION     : Sequence Reference class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string	iodNameM;
	string	identifierM;
	UINT16	groupM;	
	UINT16 	elementM;
	int		itemNumberM;

public:
	SEQUENCE_REF_CLASS(string, string, UINT16, UINT16, UINT);
	SEQUENCE_REF_CLASS(UINT16, UINT16, UINT);

	~SEQUENCE_REF_CLASS();

	string getIodName()
		{ return iodNameM; }

	string getIdentifier()
		{ return identifierM; }

	UINT16 getGroup()
		{ return groupM; }
	
	UINT16 getElement()
		{ return elementM; }

	int getItemNumber()
		{ return itemNumberM; }
};


//>>***************************************************************************

class ITEM_HANDLE_CLASS : public BASE_WAREHOUSE_ITEM_DATA_CLASS

//  DESCRIPTION     : Item Handle class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string						identifierM;
	string						nameM;
	ARRAY<SEQUENCE_REF_CLASS*>	sequenceRefM;

public:
	ITEM_HANDLE_CLASS();

	~ITEM_HANDLE_CLASS();

	void setIdentifier(string identifier)
		{ identifierM = identifier; }

	string getIdentifier()
		{ return identifierM; }

	void setName(string name)
		{ nameM = name; }

	string getName()
		{ return nameM; }

	void add(SEQUENCE_REF_CLASS *sqReference_ptr)
	{	
		if (sqReference_ptr)
		{
			sequenceRefM.add(sqReference_ptr);
		}
	}

	UINT getNoSequenceRefs()
		{ return sequenceRefM.getSize(); }

	SEQUENCE_REF_CLASS *getSequenceRef(UINT i)
	{
		SEQUENCE_REF_CLASS *sequenceRef_ptr = NULL;
		if (i < sequenceRefM.getSize())
		{
			sequenceRef_ptr = sequenceRefM[i];
		}
		return sequenceRef_ptr;
	}

	DCM_ITEM_CLASS *resolveReference();

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);
};


#endif /* ITEM_HANDLE_H */
