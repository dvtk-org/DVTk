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

#ifndef ASSOCIATION_H
#define ASSOCIATION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"        // Global component interface
#include "network_tf.h"     // Network Data Transfer
#include "vr.h"             // Value Representation
#include "accepted.h"       // Accepted Presentation Contexts


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ABORT_RQ_CLASS;
class ASSOCIATE_RJ_CLASS;
class RELEASE_RP_CLASS;
class RELEASE_RQ_CLASS;
class UNKNOWN_PDU_CLASS;
class DCM_COMMAND_CLASS;
class DCM_DATASET_CLASS;
class RECEIVE_MESSAGE_UNION_CLASS;
class DULP_STATE_CLASS;
class AE_SESSION_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define DULP_ASSOCIATE_REQUEST_LOCAL			0
#define DULP_TRANSPORT_CONFIRM_LOCAL			1
#define DULP_ASSOCIATE_ACCEPT_PDU_RECEIVED		2
#define DULP_ASSOCIATE_REJECT_PDU_RECEIVED		3
#define DULP_TRANSPORT_INDICATION_LOCAL			4
#define	DULP_ASSOCIATE_REQUEST_PDU_RECEIVED		5
#define DULP_ASSOCIATE_RESPONSE_ACCEPT_LOCAL	6
#define DULP_ASSOCIATE_RESPONSE_REJECT_LOCAL	7
#define DULP_DATA_TRANSFER_REQUEST_LOCAL		8
#define DULP_DATA_TRANSFER_PDU_RECEIVED			9
#define DULP_RELEASE_REQUEST_LOCAL				10
#define DULP_RELEASE_REQUEST_PDU_RECEIVED		11
#define DULP_RELEASE_RESPONSE_PDU_RECEIVED		12
#define DULP_RELEASE_RESPONSE_LOCAL				13
#define DULP_ABORT_REQUEST_LOCAL				14
#define DULP_ABORT_REQUEST_PDU_RECEIVED			15
#define DULP_TRANSPORT_CLOSED_LOCAL				16
#define DULP_ARTIM_TIMER_EXPIRED				17
#define DULP_INVALID_PDU_RECEIVED				18
#define DULP_INVALID_PDU_LOCAL					19


#define AE_1	0
#define AE_2	1
#define AE_3	2
#define AE_4	3
#define AE_5	4
#define AE_6	5
#define AE_7	6
#define AE_8	7
#define DT_1	8
#define DT_2	9
#define AR_1	10
#define AR_2	11
#define AR_3	12
#define AR_4	13
#define AR_5	14
#define AR_6	15
#define AR_7	16
#define AR_8	17
#define AR_9	18
#define AR_10	19
#define AA_1	20
#define AA_2	21
#define AA_3	22
#define AA_4	23
#define AA_5	24
#define AA_6	25
#define AA_7	26
#define AA_8	27
#define TO_1	28

//
// Received message enumerates - used when handling message received over association
//
enum RECEIVE_MSG_ENUM
{
	RECEIVE_MSG_SUCCESSFUL,             // successful receive
	RECEIVE_MSG_FAILURE,                // general receive failure
	RECEIVE_MSG_CONNECTION_CLOSED,      // peer has closed socket connection
    RECEIVE_MSG_NO_CONNECTION,          // no socket connection available
	RECEIVE_MSG_ASSOC_REJECTED,         // peer has rejected the association - returned from DICOM Message receive only
	RECEIVE_MSG_ASSOC_RELEASED,         // peer has released the association - returned from DICOM Message receive only
	RECEIVE_MSG_ASSOC_ABORTED           // peer has aborted the association - returned from DICOM Message receive only
};


//>>***************************************************************************

class ASSOCIATION_CLASS

//  DESCRIPTION     : Association Class.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	BASE_SOCKET_CLASS	*socketM_ptr;
	string				calledAeTitleM;
	string				callingAeTitleM;
	UINT32				maximumLengthReceivedM;
	UINT32				actualMaximumLengthToBeReceivedM;
	UID_CLASS			implementationClassUidM;
	string				implementationVersionNameM;
	UID_CLASS			scpScuRoleUidM;
	BYTE				scpRoleM;
	BYTE				scuRoleM;
	ACCEPTED_PC_CLASS	acceptedPCM;
	SUPPORTED_PC_CLASS	supportedPCM;
	ASSOCIATE_RQ_CLASS	*associateRqM_ptr;
	ASSOCIATE_AC_CLASS	*associateAcM_ptr;
	ASSOCIATE_RJ_CLASS	*associateRjM_ptr;
	RELEASE_RQ_CLASS	*releaseRqM_ptr;
	RELEASE_RP_CLASS	*releaseRpM_ptr;
	ABORT_RQ_CLASS		*abortRqM_ptr;
	UNKNOWN_PDU_CLASS	*unknownPduM_ptr;
	NETWORK_TF_CLASS	networkTransferM;
	DULP_STATE_CLASS	*dulpStateM_ptr;
	UINT				eventNumberM;
	string				sopClassUidM;
	string				sopInstanceUidM;
	UINT16				messageIdM;
	UINT16				messageIdBeingRespondedToM;
    UINT16              commandRqFieldSentM;
    BYTE                pcIdUsedToSendRequestM;
	bool				unVrDefinitionLookUpM;
	bool				ensureEvenAttributeValueLengthM;
	bool				associatedM;
	bool				strictMatchM;
	bool				storeCSTOREObjectsM;
	LOG_CLASS			*loggerM_ptr;
    BASE_SERIALIZER     *serializerM_ptr;
	UINT32              pc_idM;
	clock_t				startClockM;

	friend class DULP_STATE_CLASS;
	void changeState(DULP_STATE_CLASS*);

	void cleanup(bool acceptedToo = true);

	// state machine methods
	bool associateRequestLocal();

	bool transportConfirmLocal(ASSOCIATE_RQ_CLASS*);

	bool associateAcceptPduReceived(PDU_CLASS*);

	bool associateRejectPduReceived(PDU_CLASS*);

	bool transportIndicationLocal();

	bool associateRequestPduReceived(PDU_CLASS*);

	bool associateResponseAcceptLocal(ASSOCIATE_AC_CLASS*);

	bool associateResponseRejectLocal(ASSOCIATE_RJ_CLASS*);

	bool dataTransferRequestLocal();

	bool dataTransferPduReceived(DATA_TF_PDU_CLASS*);

	bool releaseRequestLocal(RELEASE_RQ_CLASS*);

	bool releaseRequestPduReceived(PDU_CLASS*);

	bool releaseResponsePduReceived(PDU_CLASS*);

	bool releaseResponseLocal(RELEASE_RP_CLASS*);

	bool abortRequestLocal(ABORT_RQ_CLASS*);

	bool abortRequestPduReceived(PDU_CLASS*);

	bool transportClosedLocal();

	bool artimTimerExpired();

	bool invalidPduReceived(PDU_CLASS*);

	bool invalidPduLocal(UNKNOWN_PDU_CLASS*);

	bool logUnexpectedPdu();

	RECEIVE_MSG_ENUM receivePdu();

	ASSOCIATE_RQ_CLASS *processReceivedAssociateRqPdu();

	ASSOCIATE_AC_CLASS *processReceivedAssociateAcPdu();

	ASSOCIATE_RJ_CLASS *processReceivedAssociateRjPdu();

	RELEASE_RQ_CLASS *processReceivedReleaseRqPdu();

	RELEASE_RP_CLASS *processReceivedReleaseRpPdu();

	ABORT_RQ_CLASS *processReceivedAbortRqPdu();

	UNKNOWN_PDU_CLASS *processUnknownPdu();

	RECEIVE_MSG_ENUM receiveCommand(DCM_COMMAND_CLASS**, bool, bool);

	RECEIVE_MSG_ENUM receiveDataset(DCM_DATASET_CLASS**, bool, bool, AE_SESSION_CLASS*);

	bool sendCommand(DCM_COMMAND_CLASS*, UINT16 dataSetType = NO_DATA_SET);

	bool sendCommand(DCM_COMMAND_CLASS*, BYTE, UINT16 dataSetType = NO_DATA_SET);

	bool sendDataset(DCM_DATASET_CLASS*);

	void completeCommandOnSend(DCM_COMMAND_CLASS*, UINT16);

	void addRqPresentationContexts(ASSOCIATE_RQ_CLASS*);

	void addAcPresentationContexts(ASSOCIATE_RQ_CLASS*, ASSOCIATE_AC_CLASS*);

	BYTE generatePCID();

public:
	ASSOCIATION_CLASS();

	~ASSOCIATION_CLASS();

	bool createSocket(SOCKET_PARAMETERS& socketParams);

	void setSocket(BASE_SOCKET_CLASS* socket_ptr);

	BASE_SOCKET_CLASS* getSocket() { return socketM_ptr; }

	void setCalledAeTitle(string aeTitle)
		{ calledAeTitleM = aeTitle; }

	void setCallingAeTitle(string aeTitle)
		{ callingAeTitleM = aeTitle; }

	string getCalledAeTitle()
		{ return calledAeTitleM; }
		
	string getCallingAeTitle()
		{ return callingAeTitleM; }

	void setMaximumLengthReceived(UINT32 length)
		{ maximumLengthReceivedM = length; 
		  networkTransferM.setMaxTxLength(length); }

    UINT32 getMaximumLengthReceived()
		{ return maximumLengthReceivedM; }

	void setImplementationClassUid(string uid)
		{ implementationClassUidM.set((char*) uid.c_str()); }

	void setImplementationVersionName(string name)	
		{ implementationVersionNameM = name; }

    string getImplementationVersionName()	
        { return implementationVersionNameM; }

	void setScpScuRoleSelect(char *sopClassUid_ptr, BYTE scpRole, BYTE scuRole)
	{
		scpScuRoleUidM.set(sopClassUid_ptr);
		scpRoleM = scpRole;
		scuRoleM = scuRole;
	}

	void setSupportedPresentationContext(char*, char*);

	bool isSupportedPresentationContext(char*, char*);

	int getAcceptedPresentationContextId(char*, char*);

	void setOnlyStoreCSTOREObjects(bool flag)
		{ storeCSTOREObjectsM = flag; }

	void setStrictMatch(bool flag)
		{ strictMatchM = flag; }

	void setSopClassUid(string sopClassUid)
		{ sopClassUidM = sopClassUid; }

	void setSopInstanceUid(string sopInstanceUid)
		{ sopInstanceUidM = sopInstanceUid; }

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

	BYTE getPresentationContextId(char*, char*);

	BYTE getPresentationContextId(UID_CLASS uid)
		{ return acceptedPCM.getPresentationContextId(uid); }

	bool getCurrentAbstractSyntaxName(UID_CLASS &uid)
		{ return acceptedPCM.getAbstractSyntaxName(networkTransferM.getPresentationContextId(), uid); }

	// high level association primitives
	bool makeAssociation();

	bool releaseAssociation();

	bool abortAssociation();

	RECEIVE_MSG_ENUM waitForAssociation();

	// low level association primitives
	void setRemoteHostname(char*);

    void setRemoteConnectPort(UINT16);

    void setLocalListenPort(UINT16);

    void erase();

	bool connect();

	bool listen();

	bool close();

	void reset();

	bool checkForPendingDataInNetworkInputBuffer();

	bool receive(ASSOCIATE_RQ_CLASS**);
	bool receive(ASSOCIATE_AC_CLASS**);
	bool receive(ASSOCIATE_RJ_CLASS**);
	bool receive(RELEASE_RQ_CLASS**);
	bool receive(RELEASE_RP_CLASS**);
	bool receive(ABORT_RQ_CLASS**);
	bool receive(UNKNOWN_PDU_CLASS**);

	RECEIVE_MSG_ENUM receiveCommandDataset(DCM_COMMAND_CLASS**, DCM_DATASET_CLASS**, AE_SESSION_CLASS*, bool, bool dimseOnly = true);

	RECEIVE_MSG_ENUM receive(RECEIVE_MESSAGE_UNION_CLASS**, AE_SESSION_CLASS*);

	bool send(ASSOCIATE_RQ_CLASS*);
	bool send(ASSOCIATE_AC_CLASS*);
	bool send(ASSOCIATE_RJ_CLASS*);
	bool send(RELEASE_RQ_CLASS*);
	bool send(RELEASE_RP_CLASS*);
	bool send(ABORT_RQ_CLASS*);
	bool send(UNKNOWN_PDU_CLASS*);
	bool send(DCM_COMMAND_CLASS*);
	bool send(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
	bool send(DCM_COMMAND_CLASS*, int);
	bool send(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*, int);

	void setSessionId(int sessionId)
		{ networkTransferM.setSessionId(sessionId); }

	void setStorageMode(STORAGE_MODE_ENUM storageMode)
		{ networkTransferM.setStorageMode(storageMode); }

	void setLogger(LOG_CLASS*);

	LOG_CLASS* getLogger()
		{ return loggerM_ptr; }

    void setSerializer(BASE_SERIALIZER*);
};


#endif /* ASSOCIATION_H */