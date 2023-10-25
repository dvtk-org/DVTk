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
//  DESCRIPTION     :	Network Data Transfer class.
//*****************************************************************************
#ifndef NETWORK_TF_H
#define NETWORK_TF_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"        // Global component interface
#include "data_tf_pdu.h"    // Data TF PDU

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class LOG_CLASS;

//>>***************************************************************************

class NETWORK_TF_CLASS : public DATA_TF_CLASS

//  DESCRIPTION     : Network Data Transfer Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	int sessionIdM;
	BYTE pcIdM;
	bool isCommandContentM;
	UINT dataTransferIndexM;
	UINT dataTransferOffsetM;
	UINT32 maxRxLengthM;
	UINT32 maxTxLengthM;
	ARRAY<DATA_TF_PDU_CLASS*> dataTransferPduM;
	LOG_CLASS* loggerM_ptr;

protected:
	bool getMorePdvDataToRead();

	bool getMorePdvSpaceToWrite();

public:
	NETWORK_TF_CLASS();

	~NETWORK_TF_CLASS();

	void cleanup();

	bool initialiseDecode(bool);

	bool terminateDecode();

	bool initialiseEncode();

	bool terminateEncode();

	UINT getRemainingLength();

	bool rewind(UINT);

	UINT getOffset();

	bool setOffset(UINT);

	bool serialise(string, bool);

	bool isData();

	void setSessionId(int sessionId)
		{ sessionIdM = sessionId; }

	int getSessionId()
		{ return sessionIdM; }

	void setPresentationContextId(BYTE pcId)
		{ pcIdM = pcId; }

	BYTE getPresentationContextId()
		{ return pcIdM; }

	void setIsCommandContent(bool isCommandContent)
		{ isCommandContentM = isCommandContent; }

	bool getIsCommandContent()
		{ return isCommandContentM; }

	void setMaxRxLength(UINT32 length)
		{ if (length > maxRxLengthM) maxRxLengthM = length; }

	UINT32 getMaxRxLength()
		{ return maxRxLengthM; }

	void setMaxTxLength(UINT32 length)
		{ maxTxLengthM = length; }

	UINT32 getMaxTxLength()
		{ return maxTxLengthM; }

	void addDataTransferPdu(DATA_TF_PDU_CLASS *dataTransferPdu_ptr)
		{ dataTransferPduM.add(dataTransferPdu_ptr); }

	UINT noDataTransferPdus()
		{ return dataTransferPduM.getSize(); }

	DATA_TF_PDU_CLASS *getDataTransferPdu(UINT i)
		{ 
			DATA_TF_PDU_CLASS *dataTfPdu_ptr = NULL;
			if (i < dataTransferPduM.getSize())
			{
				dataTfPdu_ptr = dataTransferPduM[i]; 
			}
			return dataTfPdu_ptr;
		}

	bool writeBinary(const BYTE *, UINT);
		
	INT	readBinary(BYTE *, UINT);

	bool isRemainingPdvDataInPdu(bool*);

	bool isTherePduData();

	void setLogger(LOG_CLASS *logger_ptr)
		{ loggerM_ptr = logger_ptr; }
};

#endif /* NETWORK_TF_H */