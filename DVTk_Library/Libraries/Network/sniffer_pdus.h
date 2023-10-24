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
//  DESCRIPTION     :	Sniffer PDUs class.
//*****************************************************************************
#ifndef SNIFFER_PDUS_H
#define SNIFFER_PDUS_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface

#include "accepted.h"       // Accepted Presentation Contexts
#include "file_pdu.h"		// File PDU Class
#include "network_tf.h"     // Network Data Transfer Class


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class RECEIVE_MESSAGE_UNION_CLASS;
class AE_SESSION_CLASS;


//>>***************************************************************************

class SNIFFER_PDUS_CLASS

//  DESCRIPTION     : Class handling the PDUS sniffed (into files) from the network.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	FILE_STREAM_CLASS	*fileStreamM_ptr;
	NETWORK_TF_CLASS	networkTransferM;
	ACCEPTED_PC_CLASS	acceptedPCM;
	string				sopClassUidM;
	string				sopInstanceUidM;
	bool				unVrDefinitionLookUpM;
	bool				ensureEvenAttributeValueLengthM;

	LOG_CLASS			*loggerM_ptr;
    BASE_SERIALIZER     *serializerM_ptr;

private:
	PDU_CLASS *getPdu();

	bool getCommandDataset(DCM_COMMAND_CLASS **, DCM_DATASET_CLASS **);

	bool getCommand(DCM_COMMAND_CLASS **);

	bool getDataset(DCM_DATASET_CLASS **);

public:
	SNIFFER_PDUS_CLASS();

	~SNIFFER_PDUS_CLASS();

	void addFileToStream(string filename);

	void removeFileStream();

	bool getMessage(RECEIVE_MESSAGE_UNION_CLASS**);

	void setStorageMode(STORAGE_MODE_ENUM storageMode)
		{ networkTransferM.setStorageMode(storageMode); }

	void setUnVrDefinitionLookUp(bool flag)
	{
		unVrDefinitionLookUpM = flag;
	}

	bool getUnVrDefinitionLookUp()
	{
		return unVrDefinitionLookUpM;
	}

	void setEnsureEvenAttributeValueLength(bool flag)
	{
		ensureEvenAttributeValueLengthM = flag;
	}

	bool getEnsureEvenAttributeValueLengthM()
	{
		return ensureEvenAttributeValueLengthM;
	}

	void setLogger(LOG_CLASS *logger_ptr)
		{ loggerM_ptr = logger_ptr;
		  acceptedPCM.setLogger(logger_ptr);
		}

	LOG_CLASS *getLogger() { return loggerM_ptr; }

	void setSerializer(BASE_SERIALIZER *serializer_ptr)
		{ serializerM_ptr = serializer_ptr; }
};

#endif /* SNIFFER_PDUS_H */


