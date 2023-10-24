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

//  Emulator Test Session class.

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#ifdef _WINDOWS
#include "..\global\stdafx.h"
#else
#include <thread.h>
thread_t btid;
#endif

#include "emulator_session.h"
#include "Idefinition.h"
#include "Iemulator.h"			// Emulator component interface
#include "Irelationship.h"
#include "Ivalidation.h"


//*****************************************************************************
//  FORWARD DECLARATIONS
//*****************************************************************************
#ifdef _WINDOWS
UINT ScpEmulatorThread(void*);
#else
void *ScpEmulatorThread(void*);
#endif

//>>===========================================================================

EMULATOR_SESSION_CLASS::EMULATOR_SESSION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// - needs cleaning up properly
    ACTIVITY_LOG_CLASS *activityLogger_ptr = new ACTIVITY_LOG_CLASS();
	logMaskM = LOG_ERROR | LOG_WARNING;
    serverSocketM_ptr = NULL;
	scuEmulatorM_ptr = NULL;
	setLogger(activityLogger_ptr);
	setActivityReporter(NULL);
	setSerializer(NULL);

	runtimeSessionTypeM = ST_EMULATOR;
	sessionTypeM = ST_EMULATOR;
	sessionFileVersionM = 0;
	sessionTitleM = "";
	isOpenM = false;
	filenameM = "";
	setSessionId(1);
	manufacturerM = MANUFACTURER;
	modelNameM = MODEL_NAME;
	softwareVersionsM = IMPLEMENTATION_VERSION_NAME;
	applicationEntityNameM = APPLICATION_ENTITY_NAME;
	applicationEntityVersionM = APPLICATION_ENTITY_VERSION;
	testedByM = TESTED_BY;
	dateM = DATE;

    sutRoleM = UP_ACCEPTOR;
	dvtAeTitleM = CALLING_AE_TITLE;
    sutAeTitleM = CALLED_AE_TITLE;
	dvtMaximumLengthReceivedM = MAXIMUM_LENGTH_RECEIVED;
	dvtImplementationClassUidM = IMPLEMENTATION_CLASS_UID;
	dvtImplementationVersionNameM = IMPLEMENTATION_VERSION_NAME;

    sutMaximumLengthReceivedM = 0;
	sutImplementationClassUidM = "";
	sutImplementationVersionNameM = "";

	delayForStorageCommitmentM = 5;

	logScpThreadM = true;
	strictValidationM = false;
	detailedValidationResultsM = true;
	summaryValidationResultsM = true;
	testLogValidationResultsM = false;
	includeType3NotPresentInResultsM = false;
	autoType2AttributesM = false;
	defineSqLengthM = false;
	addGroupLengthM = false;

	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
    setDefinitionFileRoot(".\\");
	dataDirectoryM = ".\\";
	resultsRootM = ".\\";
	appendToResultsFileM = false;
	storageModeM = SM_NO_STORAGE;
	counterM = 0;
	instanceIdM = 1;
	isSessionStoppedM = false;
	isEmulationAbortedM = false;
	isAssociatedM = false;
	scpEmulatorTypeM = SCP_EMULATOR_UNKNOWN;
	scuEmulatorTypeM = SCU_EMULATOR_UNKNOWN;

	storeCStoreReqOnlyM = false;
	acceptDuplicateImageM = true;
}

//>>===========================================================================

EMULATOR_SESSION_CLASS::~EMULATOR_SESSION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	//Remove all supported SOPs and Transfer syntaxes
	while (supportedTransferSyntaxM.getSize())
	{
		supportedTransferSyntaxM.removeAt(0);
	}
	
	cleanup();
	setLogger(NULL);
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::cleanup()

//  DESCRIPTION     : Free up resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// indicate that the session is stopped
	isSessionStoppedM = true;

	// release emulator threads
	while (scpEmulatorThreadM.getSize())
	{
		// terminate the emulation
		scpEmulatorThreadM[0]->terminate();
		scpEmulatorThreadM.removeAt(0);
	}

	if(scuEmulatorM_ptr != NULL)
		delete scuEmulatorM_ptr;

	// call the base class cleanup()
	BASE_SESSION_CLASS::cleanup();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setProductRoleIsAcceptor(bool productRoleIsAcceptor)

//  DESCRIPTION     : Set Product Role Is Acceptor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    sutRoleM = (productRoleIsAcceptor) ? UP_ACCEPTOR : UP_REQUESTOR;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::getProductRoleIsAcceptor()

//  DESCRIPTION     : Get Product Role Is Acceptor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return (sutRoleM == UP_ACCEPTOR) ? true : false;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setProductRoleIsRequestor(bool productRoleIsRequestor)

//  DESCRIPTION     : Set Product Role Is Requestor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    sutRoleM = (productRoleIsRequestor) ? UP_REQUESTOR : UP_ACCEPTOR;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::getProductRoleIsRequestor()

//  DESCRIPTION     : Get Product Role Is Requestor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return (sutRoleM == UP_REQUESTOR) ? true : false;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setCalledAeTitle(char *calledAeTitle_ptr)

//  DESCRIPTION     : Set Called Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    // trim the ae title length
    char aeTitle[AE_LENGTH +1];
    strncpy(aeTitle, calledAeTitle_ptr, AE_LENGTH);
    aeTitle[AE_LENGTH] = 0x00;

    switch(sutRoleM)
    {
        case UP_ACCEPTOR:
            sutAeTitleM = aeTitle;
            break;
        case UP_REQUESTOR:
        default:
            dvtAeTitleM = aeTitle;
            break;
    }
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getCalledAeTitle()

//  DESCRIPTION     : Get Called Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    const char *calledAeTitle_ptr = NULL;
    switch(sutRoleM)
    {
        case UP_ACCEPTOR:
            calledAeTitle_ptr = sutAeTitleM.c_str();
            break;
        case UP_REQUESTOR:
        default:
            calledAeTitle_ptr = dvtAeTitleM.c_str();
            break;
    }
    return calledAeTitle_ptr;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setCallingAeTitle(char *callingAeTitle_ptr)

//  DESCRIPTION     : Set Calling Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    // trim the ae title length
    char aeTitle[AE_LENGTH +1];
    strncpy(aeTitle, callingAeTitle_ptr, AE_LENGTH);
    aeTitle[AE_LENGTH] = 0x00;

    switch(sutRoleM)
    {
        case UP_ACCEPTOR:
            dvtAeTitleM = aeTitle;
            break;
        case UP_REQUESTOR:
        default:
            sutAeTitleM = aeTitle;
            break;
    }
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getCallingAeTitle()

//  DESCRIPTION     : Get Calling Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    const char *callingAeTitle_ptr = NULL;
    switch(sutRoleM)
    {
        case UP_ACCEPTOR:
            callingAeTitle_ptr = dvtAeTitleM.c_str();
            break;
        case UP_REQUESTOR:
        default:
            callingAeTitle_ptr = sutAeTitleM.c_str();
            break;
    }
    return callingAeTitle_ptr;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setMaximumLengthReceived(int maximumLengthReceived)

//  DESCRIPTION     : Set Maximum Length Received.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    sutMaximumLengthReceivedM = (UINT32) maximumLengthReceived;
}

//>>===========================================================================

UINT32 EMULATOR_SESSION_CLASS::getMaximumLengthReceived()

//  DESCRIPTION     : Get Maximum Length Received.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return sutMaximumLengthReceivedM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setImplementationClassUid(char *implementationClassUid_ptr)

//  DESCRIPTION     : Set Implementation Class Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    // trim the uid length
    char uid[UI_LENGTH +1];
    strncpy(uid, implementationClassUid_ptr, UI_LENGTH);
    uid[UI_LENGTH] = 0x00;

    sutImplementationClassUidM = uid;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getImplementationClassUid()

//  DESCRIPTION     : Get Implementation Class Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return sutImplementationClassUidM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setImplementationVersionName(char *implementationVersionName_ptr)

//  DESCRIPTION     : Set Implementation Version Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    // trim the name length
    char name[AE_LENGTH +1];
    strncpy(name, implementationVersionName_ptr, AE_LENGTH);
    name[AE_LENGTH] = 0x00;

    sutImplementationVersionNameM = name;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getImplementationVersionName()

//  DESCRIPTION     : Get Implementation Version Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return sutImplementationVersionNameM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setRemoteHostname(char *remoteHostname_ptr)

//  DESCRIPTION     : Set Remote Hostname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    socketParametersM.remoteHostnameM = remoteHostname_ptr;

	if (serverSocketM_ptr)
	{
		serverSocketM_ptr->setRemoteHostname(remoteHostname_ptr);
	}
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getRemoteHostname()

//  DESCRIPTION     : Get Remote Hostname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return socketParametersM.remoteHostnameM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setRemoteConnectPort(UINT16 remoteConnectPort)

//  DESCRIPTION     : Set Remote Connect Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    socketParametersM.remoteConnectPortM = (UINT16) remoteConnectPort;

	if (serverSocketM_ptr)
	{
		serverSocketM_ptr->setRemoteConnectPort(remoteConnectPort);
	}
}

//>>===========================================================================

int EMULATOR_SESSION_CLASS::getRemoteConnectPort()

//  DESCRIPTION     : Get Remote Connect Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return (int) socketParametersM.remoteConnectPortM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setLocalListenPort(UINT16 localListenPort)

//  DESCRIPTION     : Set Local Listen Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    socketParametersM.localListenPortM = (UINT16) localListenPort;

	if (serverSocketM_ptr)
	{
		serverSocketM_ptr->setLocalListenPort(localListenPort);
	}
}

//>>===========================================================================

int EMULATOR_SESSION_CLASS::getLocalListenPort()

//  DESCRIPTION     : Get Local Listen Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return (int) socketParametersM.localListenPortM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSocketTimeout(int socketTimeout)

//  DESCRIPTION     : Set Socket Timeout.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    socketParametersM.socketTimeoutM = socketTimeout;
}

//>>===========================================================================

int EMULATOR_SESSION_CLASS::getSocketTimeout()

//  DESCRIPTION     : Get Socket Timeout.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    return socketParametersM.socketTimeoutM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setDvtAeTitle(char *dvtAeTitle_ptr)

//  DESCRIPTION     : Set DVT Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    // trim the aeTitle length
    char aeTitle[AE_LENGTH +1];
    strncpy(aeTitle, dvtAeTitle_ptr, AE_LENGTH);
    aeTitle[AE_LENGTH] = 0x00;

    dvtAeTitleM = aeTitle;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getDvtAeTitle()

//  DESCRIPTION     : Get DVT Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    return dvtAeTitleM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setDvtMaximumLengthReceived(int maximumLengthReceived)

//  DESCRIPTION     : Set DVT Maximum Length Received.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    dvtMaximumLengthReceivedM = maximumLengthReceived;
}

//>>===========================================================================

UINT32 EMULATOR_SESSION_CLASS::getDvtMaximumLengthReceived()

//  DESCRIPTION     : Get DVT Maximum Length Received.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    return dvtMaximumLengthReceivedM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setDvtImplementationClassUid(char *implementationClassUid_ptr)

//  DESCRIPTION     : Set DVT Implementation Class Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    // trim the uid length
    char uid[UI_LENGTH +1];
    strncpy(uid, implementationClassUid_ptr, UI_LENGTH);
    uid[UI_LENGTH] = 0x00;

    dvtImplementationClassUidM = uid;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getDvtImplementationClassUid()

//  DESCRIPTION     : Get DVT Implementation Class Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    return dvtImplementationClassUidM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setDvtImplementationVersionName(char *implementationVersionName_ptr)

//  DESCRIPTION     : Set DVT Implementation Version Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    // trim the name length
    char name[AE_LENGTH +1];
    strncpy(name, implementationVersionName_ptr, AE_LENGTH);
    name[AE_LENGTH] = 0x00;

    dvtImplementationVersionNameM = name;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getDvtImplementationVersionName()

//  DESCRIPTION     : Get DVT Implementation Version Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    return dvtImplementationVersionNameM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setDvtPort(UINT16 port)

//  DESCRIPTION     : Set DVT (Listen) Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    socketParametersM.localListenPortM = port;
	
	if (serverSocketM_ptr)
	{
		serverSocketM_ptr->setLocalListenPort(port);
	}
}

//>>===========================================================================

UINT16 EMULATOR_SESSION_CLASS::getDvtPort()

//  DESCRIPTION     : Get DVT (Listen) Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    return socketParametersM.localListenPortM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setDvtSocketTimeout(int socketTimeout)

//  DESCRIPTION     : Set DVT Socket Timeout.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    socketParametersM.socketTimeoutM = socketTimeout;
}

//>>===========================================================================

int EMULATOR_SESSION_CLASS::getDvtSocketTimeout()

//  DESCRIPTION     : Get DVT Socket Timeout.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    return socketParametersM.socketTimeoutM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSutAeTitle(char *sutAeTitle_ptr)

//  DESCRIPTION     : Set SUT Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    // trim the aeTitle length
    char aeTitle[AE_LENGTH +1];
    strncpy(aeTitle, sutAeTitle_ptr, AE_LENGTH);
    aeTitle[AE_LENGTH] = 0x00;

    sutAeTitleM = aeTitle;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getSutAeTitle()

//  DESCRIPTION     : Get SUT Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    return sutAeTitleM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSutMaximumLengthReceived(int maximumLengthReceived)

//  DESCRIPTION     : Set SUT Maximum Length Received.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    sutMaximumLengthReceivedM = maximumLengthReceived;
}

//>>===========================================================================

UINT32 EMULATOR_SESSION_CLASS::getSutMaximumLengthReceived()

//  DESCRIPTION     : Get SUT Maximum Length Received.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    return sutMaximumLengthReceivedM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSutImplementationClassUid(char *implementationClassUid_ptr)

//  DESCRIPTION     : Set SUT Implementation Class Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    // trim the uid length
    char uid[UI_LENGTH +1];
    strncpy(uid, implementationClassUid_ptr, UI_LENGTH);
    uid[UI_LENGTH] = 0x00;

    sutImplementationClassUidM = uid;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getSutImplementationClassUid()

//  DESCRIPTION     : Get SUT Implementation Class Uid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    return sutImplementationClassUidM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSutImplementationVersionName(char *implementationVersionName_ptr)

//  DESCRIPTION     : Set SUT Implementation Version Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    // trim the name length
    char name[AE_LENGTH +1];
    strncpy(name, implementationVersionName_ptr, AE_LENGTH);
    name[AE_LENGTH] = 0x00;

    sutImplementationVersionNameM = name;
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getSutImplementationVersionName()

//  DESCRIPTION     : Get SUT Implementation Version Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    return sutImplementationVersionNameM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSutHostname(char *hostname_ptr)

//  DESCRIPTION     : Set SUT Hostname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    socketParametersM.remoteHostnameM = hostname_ptr;

	if (serverSocketM_ptr)
	{
		serverSocketM_ptr->setRemoteHostname(hostname_ptr);
	}
}

//>>===========================================================================

const char *EMULATOR_SESSION_CLASS::getSutHostname()

//  DESCRIPTION     : Get SUT Hostname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    return socketParametersM.remoteHostnameM.c_str();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSutPort(UINT16 port)

//  DESCRIPTION     : Set SUT (Connect) Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    socketParametersM.remoteConnectPortM = port;

	if (serverSocketM_ptr)
	{
		serverSocketM_ptr->setRemoteConnectPort(port);
	}
}

//>>===========================================================================

UINT16 EMULATOR_SESSION_CLASS::getSutPort()

//  DESCRIPTION     : Get SUT (Connect) Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    return socketParametersM.remoteConnectPortM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setSutRole(UP_ENUM role)

//  DESCRIPTION     : Set SUT Role.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    sutRoleM = role;
}

//>>===========================================================================

UP_ENUM EMULATOR_SESSION_CLASS::getSutRole()

//  DESCRIPTION     : Get SUT Role.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    return sutRoleM;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::serialise(FILE *file_ptr)

//  DESCRIPTION     : Serialise the script session to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// write the file contents
	fprintf(file_ptr, "SESSION\n\n");
	fprintf(file_ptr, "SESSION-TYPE emulator\n");
	fprintf(file_ptr, "SESSION-FILE-VERSION %d\n", CURRENT_SESSION_FILE_VERSION);

	fprintf(file_ptr, "\n# Product Test Session Properties\n");
	fprintf(file_ptr, "SESSION-TITLE \"%s\"\n", getSessionTitle());
	fprintf(file_ptr, "SESSION-ID %03d\n", getSessionId());
	fprintf(file_ptr, "MANUFACTURER \"%s\"\n", getManufacturer());
	fprintf(file_ptr, "MODEL-NAME \"%s\"\n", getModelName());
	fprintf(file_ptr, "SOFTWARE-VERSIONS \"%s\"\n", getSoftwareVersions());
	fprintf(file_ptr, "APPLICATION-ENTITY-NAME \"%s\"\n", getApplicationEntityName());
	fprintf(file_ptr, "APPLICATION-ENTITY-VERSION \"%s\"\n", getApplicationEntityVersion());
	fprintf(file_ptr, "TESTED-BY \"%s\"\n", getTestedBy());
	fprintf(file_ptr, "DATE \"%s\"\n", getDate());

    fprintf(file_ptr, "\n# SUT ACSE Properties\n");
	fprintf(file_ptr, "SUT-ROLE ");
	switch(getSutRole())
    {
    case UP_ACCEPTOR_REQUESTOR:
        fprintf(file_ptr, "acceptorAndRequestor\n");
        break;
    case UP_ACCEPTOR:
        fprintf(file_ptr, "acceptor\n");
        break;
    case UP_REQUESTOR:
        fprintf(file_ptr, "requestor\n");
        break;
    default:
        break;
    }
	fprintf(file_ptr, "SUT-AE-TITLE \"%s\"\n", getSutAeTitle());
	fprintf(file_ptr, "SUT-MAXIMUM-LENGTH-RECEIVED %d\n", getSutMaximumLengthReceived());
	fprintf(file_ptr, "SUT-IMPLEMENTATION-CLASS-UID \"%s\"\n", getSutImplementationClassUid());
	fprintf(file_ptr, "SUT-IMPLEMENTATION-VERSION-NAME ");
	if (getSutImplementationVersionName())
		fprintf(file_ptr, "\"%s\"\n", getSutImplementationVersionName());
	else
		fprintf(file_ptr, "\"\"\n");

	fprintf(file_ptr, "\n# DVT ACSE Properties\n");
	fprintf(file_ptr, "DVT-AE-TITLE \"%s\"\n", getDvtAeTitle());
	fprintf(file_ptr, "DVT-MAXIMUM-LENGTH-RECEIVED %d\n", getDvtMaximumLengthReceived());
	fprintf(file_ptr, "DVT-IMPLEMENTATION-CLASS-UID \"%s\"\n", getDvtImplementationClassUid());
	fprintf(file_ptr, "DVT-IMPLEMENTATION-VERSION-NAME ");
	if (getDvtImplementationVersionName())
		fprintf(file_ptr, "\"%s\"\n", getDvtImplementationVersionName());
	else
		fprintf(file_ptr, "\"\"\n");

	fprintf(file_ptr, "\n# Socket Properties\n");
	fprintf(file_ptr, "SUT-HOSTNAME \"%s\"\n", getSutHostname());
	fprintf(file_ptr, "SUT-PORT %d\n", getSutPort());
	fprintf(file_ptr, "DVT-PORT %d\n", getDvtPort());
	fprintf(file_ptr, "DVT-SOCKET-TIMEOUT %d\n", getDvtSocketTimeout());
	fprintf(file_ptr, "USE-SECURE-SOCKETS %s\n", (getUseSecureSockets()) ? "true" : "false");
	fprintf(file_ptr, "MAX-TLS-VERSION \"%s\"\n", getMaxTlsVersion());
	fprintf(file_ptr, "MIN-TLS-VERSION \"%s\"\n", getMinTlsVersion());
	fprintf(file_ptr, "CHECK-REMOTE-CERTIFICATE %s\n", (getCheckRemoteCertificate()) ? "true" : "false");
	fprintf(file_ptr, "CIPHER-LIST \"%s\"\n", getCipherList());
	fprintf(file_ptr, "CACHE-TLS-SESSIONS %s\n", (getCacheTlsSessions()) ? "true" : "false");
	fprintf(file_ptr, "TLS-CACHE-TIMEOUT %d\n", getTlsCacheTimeout());
	fprintf(file_ptr, "CREDENTIALS-FILENAME \"%s\"\n", getCredentialsFilename().c_str());
	fprintf(file_ptr, "CERTIFICATE-FILENAME \"%s\"\n", getCertificateFilename().c_str());

	fprintf(file_ptr, "\n# Test Session Properties\n");
	fprintf(file_ptr, "LOG-ERROR %s\n", (isLogLevel(LOG_ERROR)) ? "true" : "false");
	fprintf(file_ptr, "LOG-WARNING %s\n", (isLogLevel(LOG_WARNING)) ? "true" : "false");
	fprintf(file_ptr, "LOG-INFO %s\n", (isLogLevel(LOG_INFO)) ? "true" : "false");
	fprintf(file_ptr, "LOG-RELATION %s\n", (isLogLevel(LOG_IMAGE_RELATION)) ? "true" : "false");
	fprintf(file_ptr, "LOG-DEBUG %s\n", (isLogLevel(LOG_DEBUG)) ? "true" : "false");

	fprintf(file_ptr, "LOG-DULP-STATE %s\n", (isLogLevel(LOG_DULP_FSM)) ? "true" : "false");
	fprintf(file_ptr, "LOG-SCP-THREAD %s\n", (getLogScpThread()) ? "true" : "false");
	fprintf(file_ptr, "PDU-DUMP %s\n", (isLogLevel(LOG_PDU_BYTES)) ? "true" : "false");

	fprintf(file_ptr, "STORAGE-MODE ");
	switch(getStorageMode())
	{
	case SM_AS_MEDIA:
		fprintf(file_ptr, "as-media\n");
		break;
	case SM_AS_MEDIA_ONLY:
		fprintf(file_ptr, "as-media-only\n");
		break;
	case SM_AS_DATASET:
		fprintf(file_ptr, "as-dataset\n");
		break;
	case SM_NO_STORAGE:
	default:
		fprintf(file_ptr, "no-storage\n");
		break;
	}

	fprintf(file_ptr, "STRICT-VALIDATION %s\n", (getStrictValidation()) ? "true" : "false");
	fprintf(file_ptr, "DETAILED-VALIDATION-RESULTS %s\n", (getDetailedValidationResults()) ? "true" : "false");
	fprintf(file_ptr, "SUMMARY-VALIDATION-RESULTS %s\n", (getSummaryValidationResults()) ? "true" : "false");
	fprintf(file_ptr, "INCLUDE-TYPE-3-NOTPRESENT-INRESULTS %s\n", (getIncludeType3NotPresentInResults()) ? "true" : "false");
	fprintf(file_ptr, "AUTO-TYPE-2-ATTRIBUTES %s\n", (getAutoType2Attributes()) ? "true" : "false");
	fprintf(file_ptr, "DEFINE-SQ-LENGTH %s\n", (getDefineSqLength()) ? "true" : "false");
	fprintf(file_ptr, "ADD-GROUP-LENGTH %s\n", (getAddGroupLength()) ? "true" : "false");
	fprintf(file_ptr, "ENSURE-EVEN-ATTRIBUTE-VALUE-LENGTH %s\n", (getEnsureEvenAttributeValueLength()) ? "true" : "false");

	fprintf(file_ptr, "\n# Supported Transfer Syntaxes\n");
	for (int i = 0; i < noSupportedTransferSyntaxes(); i++)
	{
		fprintf(file_ptr, "SUPPORTED-TRANSFER-SYNTAX \"%s\"\n", getSupportedTransferSyntax(i));
	}

	fprintf(file_ptr, "\n# Configurable Delay between N-Action and N-Event Command\n");
	fprintf(file_ptr, "DELAY %d\n", getDelayForStorageCommitment());

	fprintf(file_ptr, "\n# Definitions\n");
	for (UINT i = 0; i < noDefinitionDirectories(); i++)
	{
		fprintf(file_ptr, "DEFINITION-DIRECTORY \"%s\"\n", getDefinitionDirectory(i).c_str());
	}

	for (unsigned int j = 0; j < noDefinitionFiles(); j++)
	{
		DEFINITION_FILE_CLASS *definitionFile_ptr = getDefinitionFile(j);
		fprintf(file_ptr, "DEFINITION \"%s\"\n", definitionFile_ptr->getFilename());
	}

	fprintf(file_ptr, "\n# Results\n");
	fprintf(file_ptr, "RESULTS-ROOT \"%s\"\n", getResultsRoot());
	fprintf(file_ptr, "APPEND-TO-RESULTS-FILE %s\n", (getAppendToResultsFile()) ? "true" : "false");

	fprintf(file_ptr, "\n# Data Directory\n");
	fprintf(file_ptr, "DATA-DIRECTORY \"%s\"\n", getDataDirectory());

	/*fprintf(file_ptr, "\n# DICOMScript Description Directory\n");
	fprintf(file_ptr, "DESCRIPTION-DIRECTORY \"%s\"\n", getDescriptionDirectory().c_str());*/

	fprintf(file_ptr, "\nENDSESSION\n");

	// return success
	return true;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setStrictValidation(bool flag)

//  DESCRIPTION     : Set the Strict Validation flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // call base method
    BASE_SESSION_CLASS::setStrictValidation(flag);

    VALIDATION->setStrictValidation(flag);
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setScpEmulatorType(SCP_EMULATOR_ENUM scpEmulatorType)

//  DESCRIPTION     : Set the Emulator Type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{ 
	scpEmulatorTypeM = scpEmulatorType; 
}
	
//>>===========================================================================

SCP_EMULATOR_ENUM EMULATOR_SESSION_CLASS::getScpEmulatorType()

//  DESCRIPTION     : Get the Emulator Type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return scpEmulatorTypeM;
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setScuEmulatorType(SCU_EMULATOR_ENUM scuEmulatorType)

//  DESCRIPTION     : Set the Emulator Type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{ 
	scuEmulatorTypeM = scuEmulatorType; 
}
	
//>>===========================================================================

SCU_EMULATOR_ENUM EMULATOR_SESSION_CLASS::getScuEmulatorType()

//  DESCRIPTION     : Get the Emulator Type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return scuEmulatorTypeM;
}

//>>===========================================================================

bool  EMULATOR_SESSION_CLASS::emulateSCP()

//  DESCRIPTION     : Emulate an SCP.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Multiple instances are supported by spawning a new thread
//					: to handle each connection.
//<<===========================================================================
{
	// cleanup any outstanding relationships
	// - from previous emulations
	RELATIONSHIP->cleanup();

	// Set up the supported SOP Classes
	switch(scpEmulatorTypeM)
	{
	case SCP_EMULATOR_STORAGE:
		// storage emulator SOP Classes
		STORAGE_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(this);
		break;
	case SCP_EMULATOR_STORAGE_COMMIT:
		// storage emulator SOP Classes
		COMMIT_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(this);
		break;
	case SCP_EMULATOR_PRINT:
		// print emulator SOP Classes
		PRINT_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(this);
		break;
	case SCP_EMULATOR_MPPS:
		// mpps emulator SOP Classes
		MPPS_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(this);
		break;
	case SCP_EMULATOR_WORKLIST:
		// worklist emulator SOP Classes
		WORKLIST_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(this);
		break;
	case SCP_EMULATOR_QUERY_RETRIEVE:
		// query/retrieve emulator SOP Classes
		QUERY_RETRIEVE_SCP_EMULATOR_CLASS::addSupportedPresentationContexts(this);
		break;
	default:
		break;
	}

	// create the server socket class (if it has not been or if it has changed)
	if (serverSocketM_ptr)
	{
		if (serverSocketM_ptr->socketParametersChanged(socketParametersM))
		{
			// parameters have changed, need to delete the old socket and recreate a new one
			delete serverSocketM_ptr;
			serverSocketM_ptr = BASE_SOCKET_CLASS::createSocket(socketParametersM, loggerM_ptr);
		}
		else
		{
			// set this thread as the owner of the socket
			serverSocketM_ptr->setOwnerThread(getThreadId());
		}
	}
	else
	{
		serverSocketM_ptr = BASE_SOCKET_CLASS::createSocket(socketParametersM, loggerM_ptr);
	}
	if (serverSocketM_ptr == NULL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 2, "Unable to create socket class");
		}
		return false;
	}

	// the session is not stopped/aborted
	isSessionStoppedM = false;
	isEmulationAbortedM = false;

	if (loggerM_ptr)
	{
        // set the storage root as the data directory
        loggerM_ptr->setStorageRoot(getDataDirectory());

		if (serverSocketM_ptr->isSecure())
		{
			loggerM_ptr->text(LOG_INFO, 2, "TCP/IP - Listening for connection using secure port number %d ...", getLocalListenPort());
		}
		else
		{
			loggerM_ptr->text(LOG_INFO, 2, "TCP/IP - Listening for connection using unsecure port number %d ...", getLocalListenPort());
		}
	}

	// listen for a connection
	if (!serverSocketM_ptr->listen())
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "No longer listening for connections on TCP/IP port: %d", getLocalListenPort());
		}
		return false;
	}

	// loop forever accepting connections and dispatching them to SCP emulator threads
	for (;;)
	{
		BASE_SOCKET_CLASS* acceptedSocket_ptr;

		if (isSessionStoppedM)
		{
			// The session is stopping, return instead of blocking in the accept
			return false;
		}

		// accept connection from peer
		if (!serverSocketM_ptr->accept(&acceptedSocket_ptr))
		{
			// close the socket in case it is not already closed
			serverSocketM_ptr->close();

			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_INFO, 1, "No longer accepting connections on TCP/IP port: %d", getLocalListenPort());
			}
			return false;
		}

		if (isSessionStoppedM)
		{
			// The session is stopping, return instead of creating a new child thread
			return false;
		}

		// spawn an emulator thread
		BASE_SCP_EMULATOR_CLASS *emulator_ptr = NULL;
		switch(scpEmulatorTypeM)
		{
		case SCP_EMULATOR_STORAGE:
			// instantiate a storage emulator
			emulator_ptr = new STORAGE_SCP_EMULATOR_CLASS(this, acceptedSocket_ptr, logScpThreadM);
			break;
		case SCP_EMULATOR_STORAGE_COMMIT:
			// instantiate a storage commit emulator
			emulator_ptr = new COMMIT_SCP_EMULATOR_CLASS(this, acceptedSocket_ptr, logScpThreadM);
			break;
		case SCP_EMULATOR_PRINT:
			// instantiate a print emulator
			emulator_ptr = new PRINT_SCP_EMULATOR_CLASS(this, acceptedSocket_ptr, logScpThreadM);
			break;
		case SCP_EMULATOR_MPPS:
			// instantiate a mpps emulator
			emulator_ptr = new MPPS_SCP_EMULATOR_CLASS(this, acceptedSocket_ptr, logScpThreadM);
			break;
		case SCP_EMULATOR_WORKLIST:
			// instantiate a worklist emulator
			emulator_ptr = new WORKLIST_SCP_EMULATOR_CLASS(this, acceptedSocket_ptr, logScpThreadM);
			break;
		case SCP_EMULATOR_QUERY_RETRIEVE:
			// instantiate a query/retrieve emulator
			emulator_ptr = new QUERY_RETRIEVE_SCP_EMULATOR_CLASS(this, acceptedSocket_ptr, logScpThreadM);
			break;
		default:
			break;
		}
		if (!emulator_ptr)
		{
			return false;
		}

		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Spawning thread to handle new Association");
		}

		// spawn emulator as new thread
		// - it will register itself with this once it has started
#ifdef _WINDOWS
		AfxBeginThread(ScpEmulatorThread, emulator_ptr);
#else
		thr_create(NULL, 0, ScpEmulatorThread, (void*) emulator_ptr, 0, &btid);
#endif
	}

	// never reached
	return true;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::sendStatusEvent()

//  DESCRIPTION     : Send a status event through the emulator.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// loop through current emulator threads
	for (UINT i = 0; i < scpEmulatorThreadM.getSize(); i++)
	{
		// send event to each
		scpEmulatorThreadM[i]->sendStatusEvent();
	}

	// return success
	return true;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::terminateConnection()

//  DESCRIPTION     : Terminate the emulation connection.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// indicate that the session is stopping
	isSessionStoppedM = true;

	// loop through current emulator threads
	for (UINT i = 0; i < scpEmulatorThreadM.getSize(); i++)
	{
		// terminate each
		scpEmulatorThreadM[i]->terminate();
	}

	// close the socket server
	if (serverSocketM_ptr)
	{
		serverSocketM_ptr->close();
	}

	// return success
	return true;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::abortEmulation()

//  DESCRIPTION     : Abort the emulation by DICOM way i.e. sending Abort Rq from SCP.
//  PRECONDITIONS   : Association must be there.
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// indicate that the emulation is stopping by DICOM way
	bool ok = false;

	// loop through current emulator threads
	for (UINT i = 0; i < scpEmulatorThreadM.getSize(); i++)
	{
		// Abort each
		if(scpEmulatorThreadM[i]->isAssociated())
		{
			isEmulationAbortedM = true;
			ok  = true;
		}
	}

	// return success
	return ok;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::abortEmulationFromSCU()

//  DESCRIPTION     : Abort the emulation by DICOM way i.e. sending Abort Rq from SCU.
//  PRECONDITIONS   : Association must be there.
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// indicate that the emulation is stopping by DICOM way
	bool ok = false;

	// loop through current emulator threads
	if(scuEmulatorM_ptr != NULL)
	{
		// Abort each
		if(scuEmulatorM_ptr->isAssociated())
		{
			isEmulationAbortedM = true;
			ok  = true;
		}
	}

	// return success
	return ok;
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::emulateStorageSCU(vector<string>* filenames, UINT options, UINT nr_repetitions)

//  DESCRIPTION     : Emulates storage SCU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// the session is not stopped
	isSessionStoppedM = false;

	//// Check for any previous emulator instance
	//if(scuEmulatorM_ptr != NULL)
	//{
	//	delete scuEmulatorM_ptr;
	//	scuEmulatorM_ptr = NULL;
	//}

	// instantiate a storage emulator
	scuEmulatorM_ptr = new STORAGE_SCU_EMULATOR_CLASS(this);

	// set options
	scuEmulatorM_ptr->setOption(options);
	scuEmulatorM_ptr->setNrRepetitions(nr_repetitions);

	vector<string>::iterator it;

	// add files to emulator
	for (it = filenames->begin(); it < filenames->end(); ++it)
	{
		scuEmulatorM_ptr->addFile(*it);
	}

	// emulate a storage SCU
	return scuEmulatorM_ptr->emulate();
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::emulateStorageCommitSCU(int delay)

//  DESCRIPTION     : Emulates storage SCU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// the session is not stopped
	isSessionStoppedM = false;

	// instantiate a storage emulator
	//STORAGE_SCU_EMULATOR_CLASS* emulator_ptr = new STORAGE_SCU_EMULATOR_CLASS(this);

	// emulate a storage SCU
	return scuEmulatorM_ptr->sendNActionReq(delay);
}

//>>===========================================================================

bool EMULATOR_SESSION_CLASS::emulateVerificationSCU()

//  DESCRIPTION     : Emulates verification SCU.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// the session is not stopped
	isSessionStoppedM = false;

	// instantiate a storage emulator
	STORAGE_SCU_EMULATOR_CLASS* emulator_ptr = new STORAGE_SCU_EMULATOR_CLASS(this);

	// emulate a storage SCP
	return emulator_ptr->verify();
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::registerEmulateSCPThread(BASE_SCP_EMULATOR_CLASS *emulator_ptr)

//  DESCRIPTION     : Register the emulator thread.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// add the thead to the list
	scpEmulatorThreadM.add(emulator_ptr);

	// set the owner thread ID of the socket to this thread
	emulator_ptr->setSocketOwnerThreadId(getThreadId());

}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::unRegisterEmulateSCPThead(BASE_SCP_EMULATOR_CLASS *emulator_ptr)

//  DESCRIPTION     : Unregister the emulator thread.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// loop through current emulator threads
	for (UINT i = 0; i < scpEmulatorThreadM.getSize(); i++)
	{
		// remove matching entry
		if (scpEmulatorThreadM[i] == emulator_ptr)
		{
			scpEmulatorThreadM.removeAt(i);
			break;
		}
	}
}

//>>===========================================================================

void EMULATOR_SESSION_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// set logger in base class
	BASE_SESSION_CLASS::setLogger(logger_ptr);

	if (serverSocketM_ptr != NULL)
	{
		serverSocketM_ptr->setLogger(logger_ptr);
	}
}

//>>===========================================================================

#ifdef _WINDOWS
UINT ScpEmulatorThread(void *param_ptr)
#else
void *ScpEmulatorThread(void *param_ptr)
#endif

//  DESCRIPTION     : Function to act as an SCP emulator thread. This is started
//					: when a thread is spawned to handle a new SCP Association.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	BASE_SCP_EMULATOR_CLASS *emulator_ptr = (BASE_SCP_EMULATOR_CLASS*) param_ptr;
	EMULATOR_SESSION_CLASS *session_ptr = emulator_ptr->getSession();

	// register the scp thread
	session_ptr->registerEmulateSCPThread(emulator_ptr);

	// emulate the SCP
	emulator_ptr->emulateScp();

	// unregister this emulator thread
	session_ptr->unRegisterEmulateSCPThead(emulator_ptr);

	LOG_CLASS *logger_ptr = session_ptr->getLogger();
	if ((!session_ptr->isSessionStopped()) && 
		(logger_ptr))
	{
		if (session_ptr->getUseSecureSockets())
		{
			logger_ptr->text(LOG_INFO, 2, "TCP/IP - Listening for connection using secure port number %d ...", session_ptr->getLocalListenPort());
		}
		else
		{
			logger_ptr->text(LOG_INFO, 2, "TCP/IP - Listening for connection using unsecure port number %d ...", session_ptr->getLocalListenPort());
		}
	}

	// cleanup here
	delete emulator_ptr;

	return 0;
}

