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

#ifndef ABORT_RQ_H
#define ABORT_RQ_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"        // Global component interface
#include "Iwarehouse.h"     // Warehouse component interface
#include "pdu_items.h"      // PDU Items


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class PDU_CLASS;


//>>***************************************************************************

class ABORT_RQ_CLASS : public PDU_ITEM_CLASS, public BASE_WAREHOUSE_ITEM_DATA_CLASS

//  DESCRIPTION     : Abort Request Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	UINT32	lengthM;
	BYTE	reserved1M;
	BYTE	reserved2M;
	BYTE	sourceM;
	BYTE	reasonM;

public:
	ABORT_RQ_CLASS();
	ABORT_RQ_CLASS(BYTE, BYTE);

	~ABORT_RQ_CLASS();		

	void setSource(BYTE source)
		{ sourceM = source; }
		
	void setReason(BYTE reason)
		{ reasonM = reason; }

	BYTE getSource()
		{ return sourceM; }
		
	BYTE getReason()
		{ return reasonM; }

	bool encode(PDU_CLASS&);

	bool decode(PDU_CLASS&);

	UINT32 getLength();

	bool updateWid(BASE_WAREHOUSE_ITEM_DATA_CLASS*);
};

#endif /* ABORT_RQ_H */