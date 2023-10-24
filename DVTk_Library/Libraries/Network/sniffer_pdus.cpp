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

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "sniffer_pdus.h"	// Sniffer PDUs
#include "message_union.h"	// Message Union
#include "pdu_items.h"      // PDU Items
#include "assoc_rq.h"		// Associate Request
#include "assoc_ac.h"		// Associate Accept
#include "assoc_rj.h"		// Associate Reject
#include "rel_rq.h"			// Release Request
#include "rel_rp.h"			// Release Response
#include "abort_rq.h"		// Abort Request

#include "Idicom.h"			// DICOM component interface
#include "Idefinition.h"	// Definition component interface
#include "Imedia.h"			// Media component interface

//>>===========================================================================

SNIFFER_PDUS_CLASS::SNIFFER_PDUS_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	acceptedPCM.clear();
	fileStreamM_ptr = NULL;
	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
	loggerM_ptr = NULL;
    serializerM_ptr = NULL;
}

//>>===========================================================================

SNIFFER_PDUS_CLASS::~SNIFFER_PDUS_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	if (fileStreamM_ptr)
	{
		delete fileStreamM_ptr;
	}
}

//>>===========================================================================

void SNIFFER_PDUS_CLASS::addFileToStream(string filename)

//  DESCRIPTION     : Add a PDU file to the file stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// Ensure that we have a file stream
	if (fileStreamM_ptr == NULL)
	{
		fileStreamM_ptr = new FILE_STREAM_CLASS();

		// set logger
	    fileStreamM_ptr->setLogger(loggerM_ptr);
	}

	// Add the file to the file stream
	if (loggerM_ptr)
    {
    	loggerM_ptr->text(LOG_INFO, 2, "Adding the %s PDU file into FileStream", filename.c_str());
    }
	fileStreamM_ptr->addFileToStream(filename);
}

//>>===========================================================================

void SNIFFER_PDUS_CLASS::removeFileStream()

//  DESCRIPTION     : Remove the file stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// Remove the file stream
	if (fileStreamM_ptr)
	{
		delete fileStreamM_ptr;
		fileStreamM_ptr = NULL;
		acceptedPCM.clear();
	}
}

//>>===========================================================================

bool SNIFFER_PDUS_CLASS::getMessage(RECEIVE_MESSAGE_UNION_CLASS **receiveMessageUnion_ptr_ptr)

//  DESCRIPTION     : Get any kind of message from the sniffer file stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The message type must be read from the union in order to determine
//					: the actual message available.
//<<===========================================================================
{
	if (fileStreamM_ptr == NULL) return false;

	// instantiate the return union class
	RECEIVE_MESSAGE_UNION_CLASS *receiveMessageUnion_ptr = new RECEIVE_MESSAGE_UNION_CLASS();

	// get a PDU from the file stream
	PDU_CLASS *pdu_ptr = getPdu();
	if (pdu_ptr == NULL) return false;

	bool result = false;
	switch(pdu_ptr->getType())
	{
	    case PDU_ASSOCIATE_RQ:
		{
			ASSOCIATE_RQ_CLASS *associateRq_ptr = new ASSOCIATE_RQ_CLASS();

   			// associate request PDU received
			result = associateRq_ptr->decode(*pdu_ptr);
			receiveMessageUnion_ptr->setAssociateRequest(associateRq_ptr);

			// initialise the accepted Presentation Context list
			if (result)
			{
				acceptedPCM.initialiseAcceptedPCs(associateRq_ptr);
			}

			// serialize it
			if (serializerM_ptr)
			{
				serializerM_ptr->SerializeReceive(associateRq_ptr);
			}					
		}
		break;

	    case PDU_ASSOCIATE_AC:
		{
			ASSOCIATE_AC_CLASS *associateAc_ptr = new ASSOCIATE_AC_CLASS();

			// associate accept PDU received
			result = associateAc_ptr->decode(*pdu_ptr);
			receiveMessageUnion_ptr->setAssociateAccept(associateAc_ptr);

			// update the accepted Presentation Context list
			if (result)
			{
				acceptedPCM.updateAcceptedPCsOnReceive(associateAc_ptr);
			}

			// serialize it
			if (serializerM_ptr)
			{
				serializerM_ptr->SerializeReceive(associateAc_ptr);
			}					
		}
		break;

	    case PDU_ASSOCIATE_RJ:
		{
			ASSOCIATE_RJ_CLASS *associateRj_ptr = new ASSOCIATE_RJ_CLASS();

			// associate reject PDU received
			result = associateRj_ptr->decode(*pdu_ptr);
			receiveMessageUnion_ptr->setAssociateReject(associateRj_ptr);

			// serialize it
			if (serializerM_ptr)
			{
				serializerM_ptr->SerializeReceive(associateRj_ptr);
			}					
		}
		break;

		case PDU_PDATA:
		{
			// got data transfer pdu
			// could be command only
			// or command and dataset
			DCM_COMMAND_CLASS *command_ptr = NULL; 
			DCM_DATASET_CLASS *dataset_ptr = NULL;
			result = getCommandDataset(&command_ptr, &dataset_ptr);
			if (result)
			{
				// received command [and dataset]
				receiveMessageUnion_ptr->setCommandDataset(command_ptr, dataset_ptr);

				// serialize it
				if (serializerM_ptr)
                {
					serializerM_ptr->SerializeReceive(command_ptr, dataset_ptr);
                }
			}
			else
			{
				// decode failure
				receiveMessageUnion_ptr->setIncompleteByteStreamFailure();
			}
		}
		break;

	    case PDU_RELEASE_RQ:
		{
			RELEASE_RQ_CLASS *releaseRq_ptr = new RELEASE_RQ_CLASS();

   			// release request PDU received
			result = releaseRq_ptr->decode(*pdu_ptr);
			receiveMessageUnion_ptr->setReleaseRequest(releaseRq_ptr);

			// serialize it
			if (serializerM_ptr)
			{
				serializerM_ptr->SerializeReceive(releaseRq_ptr);
			}					
		}
		break;

	    case PDU_RELEASE_RP:
		{
			RELEASE_RP_CLASS *releaseRp_ptr = new RELEASE_RP_CLASS();

   			// release response PDU received
			result = releaseRp_ptr->decode(*pdu_ptr);
			receiveMessageUnion_ptr->setReleaseResponse(releaseRp_ptr);

			// serialize it
			if (serializerM_ptr)
			{
				serializerM_ptr->SerializeReceive(releaseRp_ptr);
			}					
		}
		break;

		case PDU_ABORT_RQ:
		{
			ABORT_RQ_CLASS *abortRq_ptr = new ABORT_RQ_CLASS();

   			// abort request PDU received
			result = abortRq_ptr->decode(*pdu_ptr);
			receiveMessageUnion_ptr->setAbortRequest(abortRq_ptr);

			// serialize it
			if (serializerM_ptr)
			{
				serializerM_ptr->SerializeReceive(abortRq_ptr);
			}					
		}
		break;

		default:
			// receive failure
			receiveMessageUnion_ptr->setFailure();
			result = false;
   		break;
	}

	delete pdu_ptr;

	// If all the messages(file PDUs) are read clear off the File PDU stream
	if(fileStreamM_ptr->getNrOfPDUs() == fileStreamM_ptr->getCurrentPDUFileIndex())
	{
		if (loggerM_ptr)
		{
    		loggerM_ptr->text(LOG_INFO, 2, "All the PDUs from the FileStream are read successfully");
		}
		removeFileStream();
	}

	// return the union
	*receiveMessageUnion_ptr_ptr = receiveMessageUnion_ptr;

	// return result
	return result;
}

//>>===========================================================================

PDU_CLASS *SNIFFER_PDUS_CLASS::getPdu()

//  DESCRIPTION     : Get a PDU from the PDU File Stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Any P-DATA-TF PDU are collected here for later processing.
//<<===========================================================================
{
	if (fileStreamM_ptr == NULL) return NULL;

	PDU_CLASS *pdu_ptr = new PDU_CLASS();

	bool morePDUs = false;

	// loop getting at least one PDU
	do
	{
		// reset loop condition
		morePDUs = false;

		// read the PDU Type
		if (!pdu_ptr->readType(fileStreamM_ptr))
        {
			// pdu stream failure
			if (loggerM_ptr)
			{
				// log the details
				loggerM_ptr->text(LOG_ERROR, 1, "Can't read from PDU File Stream");
			}

			delete pdu_ptr;
            return NULL;
        }

        // check for Data Transfer PDU
        if (pdu_ptr->getType() == PDU_PDATA)
        {
	        // allocate a data transfer pdu
	        DATA_TF_PDU_CLASS *dataTfPdu_ptr = new DATA_TF_PDU_CLASS();

	        // set logger
	        dataTfPdu_ptr->setLogger(loggerM_ptr);

	        // read the data transfer pdu
	        if (!dataTfPdu_ptr->readBody(fileStreamM_ptr))
            {
				// pdu stream failure
		        if (loggerM_ptr)
		        {
			        // log the details
			        loggerM_ptr->text(LOG_ERROR, 1, "Can't read from PDU File Stream");
		        }

				delete pdu_ptr;
                return NULL;
            }

			// log the PDU
		    dataTfPdu_ptr->logRaw(loggerM_ptr);

		    // data transfer PDU received
			// - save the data transfer PDU
			networkTransferM.addDataTransferPdu(dataTfPdu_ptr);

			// check if more Data Transfer PDUs expected
		    morePDUs = !dataTfPdu_ptr->isLast();

			// Also check for incomplete byte stream
			// If all the messages(file PDUs) are read then return
			if( morePDUs && (fileStreamM_ptr->getNrOfPDUs() == fileStreamM_ptr->getCurrentPDUFileIndex()))
			{
				morePDUs = false;
				if (loggerM_ptr)
		        {
			        // log the details
			        loggerM_ptr->text(LOG_ERROR, 1, "Incomplete byte Stream");
		        }
			}
	    }
        else
        {
	        // read the pdu
	        if (!pdu_ptr->readBody(fileStreamM_ptr))
            {
				// pdu stream failure
			    if (loggerM_ptr)
			    {
				    // log the details
				    loggerM_ptr->text(LOG_ERROR, 1, "Can't read from PDU File Stream");
			    }

				delete pdu_ptr;
                return NULL;
            }

            // log the PDU
		    pdu_ptr->logRaw(loggerM_ptr);
		}
	} while (morePDUs);

	// return PDU
	return pdu_ptr;
}

//>>===========================================================================

bool SNIFFER_PDUS_CLASS::getCommandDataset(DCM_COMMAND_CLASS **command_ptr_ptr, DCM_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Get DICOM command and dataset.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise return values
	*command_ptr_ptr = NULL;
	*dataset_ptr_ptr = NULL;

	// try to get the DICOM command
	if (getCommand(command_ptr_ptr) == false)
	{
		// failed to decode the DICOM command
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to import DICOM command");
		}
		return false;
	}

	// check if command field attribute is available
	UINT16 commandField;
	if (!(*command_ptr_ptr)->getUSValue(TAG_COMMAND_FIELD, &commandField))
	{
		// no command field attribute present - can't continue
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "No Command Field (0000,0100) attribute value present in DICOM command");

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(*command_ptr_ptr, NULL);
            }
		}
		return false;
	}

	// check if data set type attribute is available
	UINT16 dataSetType = NO_DATA_SET;
	if (!(*command_ptr_ptr)->getUSValue(TAG_DATA_SET_TYPE, &dataSetType))
	{
		// no data set type attribute present - can't continue
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "No Data Set Type (0000,0800) attribute value present in DICOM command");

            // serialize it
            if (serializerM_ptr)
            {
                serializerM_ptr->SerializeReceive(*command_ptr_ptr, NULL);
            }
		}
		return false;
	}

	// check if SOP Class UID attribute is available - try both affected and requested
	// sop class uids - the command may be validated later to ensuer the correct one defined
	if (!(*command_ptr_ptr)->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUidM)) 
	{
		if (!(*command_ptr_ptr)->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUidM))
		{
			// SOP Class UID not available
			sopClassUidM = "";
		}
	}

	// check if SOP Instance UID attribute is available - try both affected and requested
	// sop instance uids - the command may be validated later to ensure the correct one defined
	if (!(*command_ptr_ptr)->getUIValue(TAG_AFFECTED_SOP_INSTANCE_UID, sopInstanceUidM)) 
	{
		if (!(*command_ptr_ptr)->getUIValue(TAG_REQUESTED_SOP_INSTANCE_UID, sopInstanceUidM))
		{
			// SOP Instance UID not available
			sopInstanceUidM = "";
		}
	}

	// check if a dataset is present
	if (dataSetType != NO_DATA_SET)
	{
		// try to decode dataset
		if (getDataset(dataset_ptr_ptr) == false)
		{
			// failed to decode the DICOM dataset
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Failed to import DICOM dataset");
			}
			return false;
		}
	}

	// return result
	return true;
}

//>>===========================================================================

bool SNIFFER_PDUS_CLASS::getCommand(DCM_COMMAND_CLASS **command_ptr_ptr)

//  DESCRIPTION     : Get a command from the PDU File Stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// initialise the data transfer decode
	if (!networkTransferM.initialiseDecode(false)) return false;

    BYTE receivePcId = networkTransferM.getPresentationContextId();

	// log maximum length received
	if (loggerM_ptr)
	{
		// log the P-DATA-TF pdu details received
		loggerM_ptr->text(LOG_INFO, 2, "Maximum length of Command DATA-TF PDU received (with pcId %d) is 0x%X=%d", receivePcId, networkTransferM.getMaxRxLength(), networkTransferM.getMaxRxLength());
	}

	// set the Transfer Syntax Code to use for decode
	UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);
	networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

	// allocate a new command object
	DCM_COMMAND_CLASS *command_ptr = new DCM_COMMAND_CLASS();

	// cascade the logger
	command_ptr->setLogger(loggerM_ptr);

	// set the UN VR definition look-up flag
	command_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	// set the EnsureEvenAttributeValueLength flag
	command_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

	// save the received pcId
	command_ptr->setEncodePresentationContextId((BYTE)receivePcId);

	// decode the command over the association - network transfer
	bool result = command_ptr->decode(networkTransferM);
	if (result)
	{
		// save return address
		*command_ptr_ptr = command_ptr;
	}	

	// terminate the data transfer decode
	networkTransferM.terminateDecode();

	// return result
	return result;
}

//>>===========================================================================

bool SNIFFER_PDUS_CLASS::getDataset(DCM_DATASET_CLASS **dataset_ptr_ptr)

//  DESCRIPTION     : Get a dataset from the PDU File Stream.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// check to see if any PDU data is left from the Command receive
	if (networkTransferM.isTherePduData())
	{
		// initialise the data transfer decode
		if (!networkTransferM.initialiseDecode(true)) return false;

		// check if we already have the last PDV
		bool isLast;
		bool remainingPdvData = networkTransferM.isRemainingPdvDataInPdu(&isLast);
		if (!remainingPdvData) return false;

		// if we don't have the last PDV - we need to read more PDUs
		if (!isLast)
		{
			// we have some remaining data in the current PDU but not
			// all required data
			// - get remaining PDU(s)
			// - on success check that we have the correct PDU
			// get a PDU from the file stream
			PDU_CLASS *pdu_ptr = getPdu();
			if (pdu_ptr == NULL) return false;
			if (pdu_ptr->getType() != PDU_PDATA)
			{
				// failed to read the Dataset P-DATA-TF PDU(s)
				delete pdu_ptr;
				return false;
			}
			delete pdu_ptr;
		}
	}
	else
	{
		// get Dataset P-DATA-TF PDU(s)
		// - on success check that we have the correct PDU
		// get a PDU from the file stream
		PDU_CLASS *pdu_ptr = getPdu();
		if (pdu_ptr == NULL) return false;
		if (pdu_ptr->getType() != PDU_PDATA)
		{
			// failed to read the Dataset P-DATA-TF PDU(s)
			delete pdu_ptr;
			return false;
		}
		delete pdu_ptr;

		// initialise the data transfer decode
		if (!networkTransferM.initialiseDecode(true)) return false;
	}

	// get the current Presentation Context Id
	BYTE pcId = networkTransferM.getPresentationContextId();

	// log maximum length received
	if (loggerM_ptr)
	{
		// log the P-DATA-TF pdu details received
		loggerM_ptr->text(LOG_INFO, 1, "Maximum length of Dataset DATA-TF PDU received (with pcId %d) is 0x%X=%d", pcId, networkTransferM.getMaxRxLength(), networkTransferM.getMaxRxLength());
	}

	// check if Presentation Context is accepted
	bool result = false;
	UID_CLASS transferSyntaxUid(IMPLICIT_VR_LITTLE_ENDIAN);
	if (acceptedPCM.getTransferSyntaxUid(pcId, transferSyntaxUid))
	{
		// set the Transfer Syntax Code to use for decode
		networkTransferM.setTsCode(transferSyntaxUidToCode(transferSyntaxUid), (char*) transferSyntaxUid.get());

		// check if we should serialise the dataset
		if (networkTransferM.getStorageMode() != SM_NO_STORAGE)
		{
			string	filename;
			bool	appendToFile;

			// check storage mode to see if we should include a media header or not
			if ((networkTransferM.getStorageMode() == SM_AS_MEDIA) ||
				(networkTransferM.getStorageMode() == SM_AS_MEDIA_ONLY))
			{
				// set up the media header
				MEDIA_FILE_HEADER_CLASS *mediaHeader_ptr = new MEDIA_FILE_HEADER_CLASS(networkTransferM.getSessionId(), sopClassUidM, sopInstanceUidM, (char*) transferSyntaxUid.get(), loggerM_ptr);

				// write the media header
				if (mediaHeader_ptr->write())
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_DEBUG, 1, "Generating Media Storage File: - %s", mediaHeader_ptr->getFilename());
						loggerM_ptr->text(LOG_MEDIA_FILENAME, 1, "%s", mediaHeader_ptr->getFilename());
					}
				}
				else 
				{
					if (loggerM_ptr)
					{
						loggerM_ptr->text(LOG_INFO, 1, "Failed to generate Media Storage File: - %s", mediaHeader_ptr->getFilename());
					}
				}

				// append the dataset to the header
				appendToFile = true;
				filename = mediaHeader_ptr->getFilename();

				// cleanup
				delete mediaHeader_ptr;
			}
			else
			{
				// generate a filename for the raw storage
				appendToFile = false;
				string storageRoot;
				if (loggerM_ptr)
				{
					// get the storage root
					storageRoot = loggerM_ptr->getStorageRoot();
				}
				getStorageFilename(storageRoot, networkTransferM.getSessionId(), filename, SFE_DOT_RAW);
				if (loggerM_ptr)
				{
					// log filename used for RAW dataset storage
					loggerM_ptr->text(LOG_INFO, 1, "Generating Storage Dataset File: - %s", filename.c_str());
				}
			}

			// serialise the dataset
			if (!networkTransferM.serialise(filename, appendToFile))
			{
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Failed to store Dataset in File: - %s", filename.c_str());
				}

				// return error
				return false;
			}
		}

		// allocate a new dataset object
		DCM_DATASET_CLASS *dataset_ptr = new DCM_DATASET_CLASS();

		// cascade the logger
		dataset_ptr->setLogger(loggerM_ptr);

		// set the UN VR definition look-up flag
		dataset_ptr->setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

		// set the EnsureEvenAttributeValueLength flag
		dataset_ptr->setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

		// save the received pcId
		dataset_ptr->setEncodePresentationContextId((BYTE)pcId);

		// decode the dataset over the association - network transfer
		result = dataset_ptr->decode(networkTransferM);
		if (result)
		{
			// save return address
			*dataset_ptr_ptr = dataset_ptr;
		}
		else
		{
			if (loggerM_ptr)
			{
				// log the details
				loggerM_ptr->text(LOG_ERROR, 1, "Illegal Data");
			}
		}

		// terminate the data transfer decode
		networkTransferM.terminateDecode();
	}
	else
	{
		// error - can't find Presentation Context Id is accepted list
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Cannot find Presentation Context ID of %d in Accepted list", pcId);
		}
	}

	// return result
	return result;
}
