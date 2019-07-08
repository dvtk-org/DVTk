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

//  Script Test Session class.

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "script_context.h"
#include "script_session.h"
#include "Idefinition.h"		// Definition component interface
#include "Inetwork.h"			// Network component interface
#include "Idicom.h"				// DICOM component interface
#include "Ilog.h"               // Logging component interface
#include "Iscripting.h"			// Scripting component interface
#include "Ivalidation.h"		// Validation component interface


//>>===========================================================================

SCRIPT_SESSION_CLASS::SCRIPT_SESSION_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	// - needs cleaning up properly
    dicomScriptRootM = ".\\";
	ACTIVITY_LOG_CLASS *activityLogger_ptr = new ACTIVITY_LOG_CLASS();
	logMaskM = LOG_ERROR | LOG_WARNING | LOG_INFO;
    setLogger(activityLogger_ptr);
	setActivityReporter(NULL);
	setSerializer(NULL);
	setConfirmer(NULL);

	runtimeSessionTypeM = ST_SCRIPT;
	sessionTypeM = ST_SCRIPT;
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

	unVrDefinitionLookUpM = true;
	ensureEvenAttributeValueLengthM = true;
    setDefinitionFileRoot(".\\");
	dataDirectoryM = ".\\";
	resultsRootM = ".\\";
	appendToResultsFileM = false;
	strictValidationM = false;
	detailedValidationResultsM = true;
	summaryValidationResultsM = true;
	testLogValidationResultsM = false;
	includeType3NotPresentInResultsM = false;
	autoType2AttributesM = true;
	autoCreateDirectoryM = false;
	defineSqLengthM = false;
	addGroupLengthM = false;
	continueOnErrorM = true;
	setStorageMode(SM_AS_MEDIA);
	counterM = 0;
	scriptExecutionContextM_ptr = NULL;
	resetScriptExecutionContext();
	scriptDoneCallBackFunctionM_ptr = NULL;
	scriptDoneCallBackContextM_ptr = NULL;
	instanceIdM = 1;
	isSessionStoppedM = false;
	lastCommandSentM_ptr = NULL;
	lastDatasetSentM_ptr = NULL;
}

//>>===========================================================================

SCRIPT_SESSION_CLASS::~SCRIPT_SESSION_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
	cleanup();
	setLogger(NULL);
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::cleanup()

//  DESCRIPTION     : Free up resources.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// indicate that the session is stopped
	isSessionStoppedM = true;

	// free up member structures
	while (dicomScriptM_ptr.getSize())
	{
		delete dicomScriptM_ptr[0];
		dicomScriptM_ptr.removeAt(0);
	}

	// clean up resources
	while (accPresentationContextM.getSize())
	{
		accPresentationContextM.removeAt(0);
	}

	delete lastCommandSentM_ptr;
	delete lastDatasetSentM_ptr;

	// call the base class cleanup()
	BASE_SESSION_CLASS::cleanup();
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setProductRoleIsAcceptor(bool productRoleIsAcceptor)

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

bool SCRIPT_SESSION_CLASS::getProductRoleIsAcceptor()

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

void SCRIPT_SESSION_CLASS::setProductRoleIsRequestor(bool productRoleIsRequestor)

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

bool SCRIPT_SESSION_CLASS::getProductRoleIsRequestor()

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

void SCRIPT_SESSION_CLASS::setCalledAeTitle(char *calledAeTitle_ptr)

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

const char *SCRIPT_SESSION_CLASS::getCalledAeTitle()

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

void SCRIPT_SESSION_CLASS::setCallingAeTitle(char *callingAeTitle_ptr)

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

const char *SCRIPT_SESSION_CLASS::getCallingAeTitle()

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

void SCRIPT_SESSION_CLASS::setMaximumLengthReceived(int maximumLengthReceived)

//  DESCRIPTION     : Set Maximum Length Received.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    sutMaximumLengthReceivedM = (UINT32) maximumLengthReceived;
	associationM.setMaximumLengthReceived(maximumLengthReceived);
}

//>>===========================================================================

UINT32 SCRIPT_SESSION_CLASS::getMaximumLengthReceived()

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

void SCRIPT_SESSION_CLASS::setImplementationClassUid(char *implementationClassUid_ptr)

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

const char *SCRIPT_SESSION_CLASS::getImplementationClassUid()

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

void SCRIPT_SESSION_CLASS::setImplementationVersionName(char *implementationVersionName_ptr)

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

const char *SCRIPT_SESSION_CLASS::getImplementationVersionName()

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

void SCRIPT_SESSION_CLASS::setRemoteHostname(char *remoteHostname_ptr)

//  DESCRIPTION     : Set Remote Hostname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    socketParametersM.remoteHostnameM = remoteHostname_ptr;
	associationM.setRemoteHostname(remoteHostname_ptr);
}

//>>===========================================================================

const char *SCRIPT_SESSION_CLASS::getRemoteHostname()

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

void SCRIPT_SESSION_CLASS::setRemoteConnectPort(UINT16 remoteConnectPort)

//  DESCRIPTION     : Set Remote Connect Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    socketParametersM.remoteConnectPortM = (UINT16) remoteConnectPort;
	associationM.setRemoteConnectPort(remoteConnectPort);
}

//>>===========================================================================

int SCRIPT_SESSION_CLASS::getRemoteConnectPort()

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

void SCRIPT_SESSION_CLASS::setLocalListenPort(UINT16 localListenPort)

//  DESCRIPTION     : Set Local Listen Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Method deprecated.
//<<===========================================================================
{
    socketParametersM.localListenPortM = (UINT16) localListenPort;
	associationM.setLocalListenPort(localListenPort);
}

//>>===========================================================================

int SCRIPT_SESSION_CLASS::getLocalListenPort()

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

void SCRIPT_SESSION_CLASS::setSocketTimeout(int socketTimeout)

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

int SCRIPT_SESSION_CLASS::getSocketTimeout()

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

void SCRIPT_SESSION_CLASS::setDvtAeTitle(char *dvtAeTitle_ptr)

//  DESCRIPTION     : Set DVT Ae Title.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    // trim the ae title length
    char aeTitle[AE_LENGTH +1];
    strncpy(aeTitle, dvtAeTitle_ptr, AE_LENGTH);
    aeTitle[AE_LENGTH] = 0x00;

    dvtAeTitleM = aeTitle;
}

//>>===========================================================================

const char *SCRIPT_SESSION_CLASS::getDvtAeTitle()

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

void SCRIPT_SESSION_CLASS::setDvtMaximumLengthReceived(int maximumLengthReceived)

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

UINT32 SCRIPT_SESSION_CLASS::getDvtMaximumLengthReceived()

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

void SCRIPT_SESSION_CLASS::setDvtImplementationClassUid(char *implementationClassUid_ptr)

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

const char *SCRIPT_SESSION_CLASS::getDvtImplementationClassUid()

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

void SCRIPT_SESSION_CLASS::setDvtImplementationVersionName(char *implementationVersionName_ptr)

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

const char *SCRIPT_SESSION_CLASS::getDvtImplementationVersionName()

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

void SCRIPT_SESSION_CLASS::setDvtPort(UINT16 port)

//  DESCRIPTION     : Set DVT (Listen) Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : DVT session property.
//<<===========================================================================
{
    socketParametersM.localListenPortM = port;
	associationM.setLocalListenPort(port);
}

//>>===========================================================================

UINT16 SCRIPT_SESSION_CLASS::getDvtPort()

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

void SCRIPT_SESSION_CLASS::setDvtSocketTimeout(int socketTimeout)

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

int SCRIPT_SESSION_CLASS::getDvtSocketTimeout()

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

void SCRIPT_SESSION_CLASS::setSutAeTitle(char *sutAeTitle_ptr)

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

const char *SCRIPT_SESSION_CLASS::getSutAeTitle()

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

void SCRIPT_SESSION_CLASS::setSutMaximumLengthReceived(int maximumLengthReceived)

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

UINT32 SCRIPT_SESSION_CLASS::getSutMaximumLengthReceived()

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

void SCRIPT_SESSION_CLASS::setSutImplementationClassUid(char *implementationClassUid_ptr)

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

const char *SCRIPT_SESSION_CLASS::getSutImplementationClassUid()

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

void SCRIPT_SESSION_CLASS::setSutImplementationVersionName(char *implementationVersionName_ptr)

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

const char *SCRIPT_SESSION_CLASS::getSutImplementationVersionName()

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

void SCRIPT_SESSION_CLASS::setSutHostname(char *hostname_ptr)

//  DESCRIPTION     : Set SUT Hostname.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    socketParametersM.remoteHostnameM = hostname_ptr;
	associationM.setRemoteHostname(hostname_ptr);
}

//>>===========================================================================

const char *SCRIPT_SESSION_CLASS::getSutHostname()

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

void SCRIPT_SESSION_CLASS::setSutPort(UINT16 port)

//  DESCRIPTION     : Set SUT (Connect) Port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : SUT session property.
//<<===========================================================================
{
    socketParametersM.remoteConnectPortM = port;
	associationM.setRemoteConnectPort(port);
}

//>>===========================================================================

UINT16 SCRIPT_SESSION_CLASS::getSutPort()

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

void SCRIPT_SESSION_CLASS::setSutRole(UP_ENUM role)

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

UP_ENUM SCRIPT_SESSION_CLASS::getSutRole()

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

void SCRIPT_SESSION_CLASS::setStrictValidation(bool flag)

//  DESCRIPTION     : Set the Strict Validation flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // call base method
    BASE_SESSION_CLASS::setStrictValidation(flag);

	// update execution context
	if (scriptExecutionContextM_ptr)
	{
		scriptExecutionContextM_ptr->setStrictValidation(flag);
	}
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setApplicationEntityName(char *applicationEntityName_ptr)

//  DESCRIPTION     : Set the Application Entity Name.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    applicationEntityNameM = applicationEntityName_ptr;

    // update execution context
	if (scriptExecutionContextM_ptr)
	{
		scriptExecutionContextM_ptr->setApplicationEntityName(applicationEntityName_ptr);
	}
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setApplicationEntityVersion(char *applicationEntityVersion_ptr)

//  DESCRIPTION     : Set the Application Entity Version.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    applicationEntityVersionM = applicationEntityVersion_ptr;

    // update execution context
	if (scriptExecutionContextM_ptr)
	{
		scriptExecutionContextM_ptr->setApplicationEntityVersion(applicationEntityVersion_ptr);
	}
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setStorageMode(STORAGE_MODE_ENUM storageMode)

//  DESCRIPTION     : Set the Storage Mode.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	BASE_SESSION_CLASS::setStorageMode(storageMode);
	associationM.setStorageMode(storageMode);
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setAutoType2Attributes(bool flag)

//  DESCRIPTION     : Set the Auto Type 2 Attributes flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	autoType2AttributesM = flag;

	// update execution context
	if (scriptExecutionContextM_ptr)
	{
		scriptExecutionContextM_ptr->setAutoType2Attributes(flag);
	}
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setAutoCreateDirectory(bool flag)

//  DESCRIPTION     : Set the Auto Create Directory flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	autoCreateDirectoryM = flag;
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setDefineSqLength(bool flag)

//  DESCRIPTION     : Set the Define SQ Length flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	defineSqLengthM = flag;

	// update execution context
	if (scriptExecutionContextM_ptr)
	{
		scriptExecutionContextM_ptr->setDefineSqLength(flag);
	}
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setAddGroupLength(bool flag)

//  DESCRIPTION     : Set the Add Group Length flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	addGroupLengthM = flag;

	// update execution context
	if (scriptExecutionContextM_ptr)
	{
		scriptExecutionContextM_ptr->setAddGroupLength(flag);
	}
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setContinueOnError(bool flag)

//  DESCRIPTION     : Set the Continue On Error flag.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	continueOnErrorM = flag;
}

//>>===========================================================================

SCRIPT_EXECUTION_CONTEXT_CLASS	*SCRIPT_SESSION_CLASS::getScriptExecutionContext()

//  DESCRIPTION     : Get the Script Execution Context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	return scriptExecutionContextM_ptr;
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::resetScriptExecutionContext()

//  DESCRIPTION     : Reset the Script Execution Context.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// release the current execution context
	if (scriptExecutionContextM_ptr)
	{
		delete scriptExecutionContextM_ptr;
	}

	// reset to the session defaults
	scriptExecutionContextM_ptr = new SCRIPT_EXECUTION_CONTEXT_CLASS(this);
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::updateLabelledValues(DCM_ATTRIBUTE_GROUP_CLASS *dsObject_ptr, DCM_ATTRIBUTE_GROUP_CLASS *rxObject_ptr)

//  DESCRIPTION     : Run through received object and check if any labelled attribute
//					: values should be updated.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check that we have valid objects
	if ((dsObject_ptr == NULL) ||
		(rxObject_ptr == NULL))
	{
		// can't continue
		return true;
	}

	// run through all attributes
	for (int i = 0; i < rxObject_ptr->GetNrAttributes(); i++)
	{
		// get received attribute and corresponding script attribute
		DCM_ATTRIBUTE_CLASS *rxAttribute_ptr = rxObject_ptr->GetAttribute(i);
		DCM_ATTRIBUTE_CLASS *dsAttribute_ptr = dsObject_ptr->GetAttribute(rxAttribute_ptr->GetGroup(), rxAttribute_ptr->GetElement());

		// check that we have a value
		if ((dsAttribute_ptr) &&
			(rxAttribute_ptr->GetNrValues() > 0) &&
			(rxAttribute_ptr->GetNrValues() == dsAttribute_ptr->GetNrValues()))
		{
			// process the attribute value(s) further
			if ((rxAttribute_ptr->GetVR() == ATTR_VR_SQ) &&
				(rxAttribute_ptr->GetNrValues() == 1))
			{
				DCM_VALUE_SQ_CLASS *rxSqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*> (rxAttribute_ptr->GetValue(0));
				DCM_VALUE_SQ_CLASS *dsSqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*> (dsAttribute_ptr->GetValue(0));

				// check that the same number of items are present
				if ((rxSqValue_ptr->GetNrItems() > 0) &&
					(rxSqValue_ptr->GetNrItems() == dsSqValue_ptr->GetNrItems()))
				{
					// for all items check the label updates
					for (int j = 0; j < rxSqValue_ptr->GetNrItems(); j++)
					{
						DCM_ITEM_CLASS *rxItem_ptr = rxSqValue_ptr->getItem(j);
						DCM_ITEM_CLASS *dsItem_ptr = dsSqValue_ptr->getItem(j);

						// recurse and update labelled values
						if (!updateLabelledValues(dsItem_ptr, rxItem_ptr)) return false;
					}
				}
			}
			else
			{
				switch (rxAttribute_ptr->GetVR())
				{
				case ATTR_VR_AE:
				case ATTR_VR_AS:
				case ATTR_VR_CS:
				case ATTR_VR_DA:
				case ATTR_VR_DS:
				case ATTR_VR_DT:
				case ATTR_VR_IS:
				case ATTR_VR_LO:
				case ATTR_VR_LT:
				case ATTR_VR_PN:
				case ATTR_VR_SH:
				case ATTR_VR_TM:
				case ATTR_VR_UI:
				case ATTR_VR_UN:
				case ATTR_VR_UT:
				case ATTR_VR_UC:
				case ATTR_VR_UR:
					{
						// for all items check the label updates
						for (int j = 0; j < rxAttribute_ptr->GetNrValues(); j++)
						{
							BASE_VALUE_CLASS *rxValue_ptr = static_cast<BASE_VALUE_CLASS*> (rxAttribute_ptr->GetValue(j));
							BASE_VALUE_CLASS *dsValue_ptr = static_cast<BASE_VALUE_CLASS*> (dsAttribute_ptr->GetValue(j));

							string dsValue;
							string rxValue;

							if (dsValue_ptr)
							{
								dsValue_ptr->Get(dsValue);
							}

							if (rxValue_ptr)
							{
								rxValue_ptr->Get(rxValue);
							}

							// check for value mapping
							char *mappedValue_ptr = WAREHOUSE->getMappedValue((char*) dsValue.c_str(), loggerM_ptr);

							if (mappedValue_ptr != NULL)
							{
								if (strcmp(mappedValue_ptr, UNDEFINED_MAPPING) == 0)
								{
									// define the value mapping
									WAREHOUSE->addMappedValue((char*) dsValue.c_str(),
													(char*) rxValue.c_str(),
													rxAttribute_ptr->GetGroup(),
													rxAttribute_ptr->GetElement(),
													loggerM_ptr);
									mappedValue_ptr = WAREHOUSE->getMappedValue((char*) dsValue.c_str(), loggerM_ptr);
								}

								if (loggerM_ptr)
								{
									loggerM_ptr->text(LOG_DEBUG, 1, "Updating labelled value \"%s\" to \"%s\" for attribute (%04X,%04X)", (char*) dsValue.c_str(), mappedValue_ptr, rxAttribute_ptr->GetGroup(), rxAttribute_ptr->GetElement());
								}

								// replace the expected value with the mapping
								dsValue_ptr = CreateNewValue(dsAttribute_ptr->GetVR());
								dsValue_ptr->Set((BYTE*) mappedValue_ptr, strlen(mappedValue_ptr));

								(void) dsAttribute_ptr->replaceValue(j, dsValue_ptr);
							}
						}
					}
					break;
				default:
					break;
				}
			}
		}
	}

	// return result
	return true;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::comparePixelData(DCM_DATASET_CLASS *srcObject_ptr, DCM_DATASET_CLASS *refObject_ptr)

//  DESCRIPTION     : Run through source object and compare OB,OW & OF data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	bool ok = false;

	// check that we have valid objects
	if ((srcObject_ptr == NULL) ||
		(refObject_ptr == NULL))
	{
		// can't continue
		return false;
	}

	// run through all attributes
	for (int i = 0; i < srcObject_ptr->GetNrAttributes(); i++)
	{
		// get received attribute and corresponding script attribute
		DCM_ATTRIBUTE_CLASS *srcAttribute_ptr = srcObject_ptr->GetAttribute(i);
		DCM_ATTRIBUTE_CLASS *refAttribute_ptr = refObject_ptr->GetAttribute(srcAttribute_ptr->GetMappedGroup(), srcAttribute_ptr->GetMappedElement());

		if((srcAttribute_ptr->GetVR() == ATTR_VR_OB) ||
			(srcAttribute_ptr->GetVR() == ATTR_VR_OW) ||
			(srcAttribute_ptr->GetVR() == ATTR_VR_OF))
		{
			// check that we have a value
			if ((srcAttribute_ptr) &&
				(srcAttribute_ptr->GetNrValues() == 1) &&
				(refAttribute_ptr->GetNrValues() == 1))
			{
				BASE_VALUE_CLASS *refValue_ptr = static_cast<BASE_VALUE_CLASS*> (refAttribute_ptr->GetValue(0));
				BASE_VALUE_CLASS *srcValue_ptr = static_cast<BASE_VALUE_CLASS*> (srcAttribute_ptr->GetValue(0));

				DVT_STATUS status = srcValue_ptr->Compare(NULL, refValue_ptr);

				if(status == MSG_EQUAL)
				{
					ok = true;
				}			
			}
		}
	}

	// return result
	return ok;
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::logRelationship()

//  DESCRIPTION     : Log the image relationship analysis.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log the object relationship analysis
	if (isLogLevel(LOG_IMAGE_RELATION))
	{
		RELATIONSHIP->logObjectRelationshipAnalysis(loggerM_ptr);
	}
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::scriptDone(void)

//  DESCRIPTION     : Script done - try calling callback.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log label mappings
	WAREHOUSE->logMapping(loggerM_ptr);

	// check if callback defined
	if (scriptDoneCallBackFunctionM_ptr)
	{
		// if enabled - call the callback function to indicate that the
		// script has been handled
		(*scriptDoneCallBackFunctionM_ptr)(scriptDoneCallBackContextM_ptr);
	}
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::serialise(FILE *file_ptr)

//  DESCRIPTION     : Serialise the script session to file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// write the file contents
	fprintf(file_ptr, "SESSION\n\n");
	fprintf(file_ptr, "SESSION-TYPE script\n");
	fprintf(file_ptr, "SESSION-FILE-VERSION %d\n", CURRENT_SESSION_FILE_VERSION);

	fprintf(file_ptr, "\n# SUT Test Session Properties\n");
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
	fprintf(file_ptr, "TLS-VERSION \"%s\"\n", getTlsVersion());
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
	fprintf(file_ptr, "PDU-DUMP %s\n", (isLogLevel(LOG_PDU_BYTES)) ? "true" : "false");
	fprintf(file_ptr, "LABEL-DUMP %s\n", (isLogLevel(LOG_LABEL)) ? "true" : "false");

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
	fprintf(file_ptr, "AUTO-CREATE-DIRECTORY %s\n", (getAutoCreateDirectory()) ? "true" : "false");
	fprintf(file_ptr, "DEFINE-SQ-LENGTH %s\n", (getDefineSqLength()) ? "true" : "false");
	fprintf(file_ptr, "ADD-GROUP-LENGTH %s\n", (getAddGroupLength()) ? "true" : "false");
	fprintf(file_ptr, "CONTINUE-ON-ERROR %s\n", (getContinueOnError()) ? "true" : "false");
	fprintf(file_ptr, "ENSURE-EVEN-ATTRIBUTE-VALUE-LENGTH %s\n", (getEnsureEvenAttributeValueLength()) ? "true" : "false");

	fprintf(file_ptr, "\n# Definitions\n");
	for (UINT i = 0; i < noDefinitionDirectories(); i++)
	{
		fprintf(file_ptr, "DEFINITION-DIRECTORY \"%s\"\n", getDefinitionDirectory(i).c_str());
	}

	for (UINT i = 0; i < noDefinitionFiles(); i++)
	{
		DEFINITION_FILE_CLASS *definitionFile_ptr = getDefinitionFile(i);
		fprintf(file_ptr, "DEFINITION \"%s\"\n", definitionFile_ptr->getFilename());
	}

	fprintf(file_ptr, "\n# DICOMScripts\n");
	fprintf(file_ptr, "DICOMSCRIPT-ROOT \"%s\"\n", getDicomScriptRoot());
	for (UINT i = 0; i < noDicomScripts(); i++)
	{
		DICOM_SCRIPT_CLASS *dicomScript_ptr = getDicomScript(i);
		fprintf(file_ptr, "DICOMSCRIPT \"%s\"\n", dicomScript_ptr->getFilename());
	}

	fprintf(file_ptr, "\n# Results\n");
	fprintf(file_ptr, "RESULTS-ROOT \"%s\"\n", getResultsRoot());
	fprintf(file_ptr, "APPEND-TO-RESULTS-FILE %s\n", (getAppendToResultsFile()) ? "true" : "false");

	fprintf(file_ptr, "\n# Data Directory\n");
	fprintf(file_ptr, "DATA-DIRECTORY \"%s\"\n", getDataDirectory());

	fprintf(file_ptr, "\n# DICOMScript Description Directory\n");
	fprintf(file_ptr, "DESCRIPTION-DIRECTORY \"%s\"\n", getDescriptionDirectory().c_str());

	fprintf(file_ptr, "\nENDSESSION\n");

	// return success
	return true;
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setDicomScriptRoot(char* dicomScriptRoot_ptr)

//  DESCRIPTION     : Set the dicom script root - make it absolute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// save the absolute dicom script root
	makeRootAbsolute(dicomScriptRootM, dicomScriptRoot_ptr);
    //
    // Add backslash at the end of the dicomScriptRootM directory path
    //
    if (dicomScriptRootM[dicomScriptRootM.length()-1] != '\\')
    {
        dicomScriptRootM += "\\";
    }
    //
    // Clip away ".\" from end of absolute path.
    //
    char pathname[_MAX_DIR];
    strcpy(pathname, dicomScriptRootM.c_str());
    reducePathname(pathname);
    dicomScriptRootM = pathname;

	// log the dicom script root
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "DICOMScript root: \"%s\"", dicomScriptRootM.c_str());
	}
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::begin(bool& definitionFileloaded)

//  DESCRIPTION     : Begin the session - load definition files and execute scripts.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    //
    // enable script logging
    //
    if (loggerM_ptr)
    {
        //
        // enable the base level logging
        //
        UINT32 logMask = loggerM_ptr->getLogMask();
        logMask |= (LOG_NONE | LOG_SCRIPT | LOG_SCRIPT_NAME | LOG_MEDIA_FILENAME);
        loggerM_ptr->setLogMask(logMask);
    }
    //
    // call the base class begin()
    //
    if (!BASE_SESSION_CLASS::begin(definitionFileloaded)) return false;
    //
    // reset the script execution context after loading the session file
    // - takes over latest property values
    //
    resetScriptExecutionContext();
    //
    // make sure that we have an absolute pathname defined for the dicom scripts
    //
    if (dicomScriptRootM == ".\\")
    {
        //
        // explicity set the dicom script root
        //
        setDicomScriptRoot(".\\");
    }
    //
    // create the socket class (if it has not been or if it has changed)
    //
    if (!associationM.createSocket(socketParametersM))
    {
        //
        // couldn't create socket class
        //
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_NONE, 1, "%s %s", VAL_PREFIX_FAILED, "Can't create socket class");
        }
        //
        // return
        //
        return false;
    }

	// set the EnsureEvenAttributeValueLength flag
	associationM.setEnsureEvenAttributeValueLength(ensureEvenAttributeValueLengthM);

    //
    // execute any given dicomScripts
    //
    for (UINT i = 0; i < dicomScriptM_ptr.getSize(); i++)
    {
        DICOM_SCRIPT_CLASS	*dicomScript_ptr = dicomScriptM_ptr[i];
        //
        // execute the dicomScript
        //
        if (!dicomScript_ptr->execute())
        {
            //
            // script failure
            //
            if (loggerM_ptr)
            {
                loggerM_ptr->text(LOG_NONE, 1, "%s %s", VAL_PREFIX_FAILED, dicomScript_ptr->getFilename());
            }
            //
            // return - can't continue on script error
            //
            return false;
        }
    }
    //
    // return result
    //
    return true;
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::end()

//  DESCRIPTION     : End session - shut it down gracefully.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// close the association down
	associationM.close();

	// call the base class end()
	BASE_SESSION_CLASS::end();
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::connectOnTcpIp()

//  DESCRIPTION     : Connect On TCP/IP.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    //
    // check if listen socket needs updating due to possible change in parameters
    //
    if (!associationM.createSocket(socketParametersM))
    {
        //
        // couldn't create socket class
        //
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_NONE, 1, "%s %s", VAL_PREFIX_FAILED, "Can't create socket class");
        }
        //
        // return
        //
        return false;
	}

    return associationM.connect();
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::listenOnTcpIp()

//  DESCRIPTION     : Listen On TCP/IP.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    //
    // check if listen socket needs updating due to possible change in parameters
    //
    if (!associationM.createSocket(socketParametersM))
    {
        //
        // couldn't create socket class
        //
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_NONE, 1, "%s %s", VAL_PREFIX_FAILED, "Can't create socket class");
        }
        //
        // return
        //
        return false;
	}

	return associationM.listen();
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::executeScript(string scriptName)

//  DESCRIPTION     : Execute the given script.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Script name may have .ds or .dss extension.
//<<===========================================================================
{
	if (loggerM_ptr)
	{
		// set the storage root as the data directory
		loggerM_ptr->setStorageRoot(getDataDirectory());
	}

	isSessionStoppedM = false;
	bool result = false;

	// create the socket class (if it has not been or if it has changed)
	if (!associationM.createSocket(socketParametersM))
	{
		// couldn't create socket class
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_NONE, 1, "%s %s", VAL_PREFIX_FAILED, "Can't create socket class");
		}

		// return
		return false;
	}

	// check if we have a DICOMScript or DICOMSuperScript
	if (isFileExtension(scriptName, DOT_DS))
	{
		// execute a DICOMScript
		DICOM_SCRIPT_CLASS	dicomScript(this, scriptName);
		result = dicomScript.execute();
	}
	else if (isFileExtension(scriptName, DOT_DSS))
	{
		// execute a DICOMSuperScript
		DICOM_SUPER_SCRIPT_CLASS dicomSuperScript(this, scriptName);
		result = dicomSuperScript.execute();
	}

	// if problems encountered during script execution - it may be necessary to reset the
	// association
	if (!result)
	{
		// reset association
		resetAssociation();
	}

	// indicate that the script is done
	scriptDone();

	// return the execution result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::parseScript(string scriptName)

//  DESCRIPTION     : Parse the given script.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : Script name may have .ds or .dss extension.
//<<===========================================================================
{
	bool result = false;
	if (loggerM_ptr)
	{
		// set the storage root as the data directory
		loggerM_ptr->setStorageRoot(getDataDirectory());
	}

	isSessionStoppedM = false;

	// check if we have a DICOMScript or DICOMSuperScript
	if (isFileExtension(scriptName, DOT_DS))
	{
		// execute a DICOMScript
		DICOM_SCRIPT_CLASS	dicomScript(this, scriptName);
		result = dicomScript.parse();
	}
	else if (isFileExtension(scriptName, DOT_DSS))
	{
		// execute a DICOMSuperScript
		DICOM_SUPER_SCRIPT_CLASS dicomSuperScript(this, scriptName);
		result = dicomSuperScript.parse();
	}

	// indicate that the script is done
	// check if callback defined
	if (scriptDoneCallBackFunctionM_ptr)
	{
		// if enabled - call the callback function to indicate that the
		// script has been handled
		(*scriptDoneCallBackFunctionM_ptr)(scriptDoneCallBackContextM_ptr);
	}

	// return the execution result
	return result;
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::resetAssociation()

//  DESCRIPTION     : Reset the Association DULP state machine.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Resetting Association DULP state machine");
	}

	// reset the DULP state machine
	associationM.reset();
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setScriptDoneCallBack(void (*callBackFunction_ptr)(void*), void *callBackContext_ptr)

//  DESCRIPTION     : Set up the script done callback.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// save the callback function and context
	scriptDoneCallBackFunctionM_ptr = callBackFunction_ptr;
	scriptDoneCallBackContextM_ptr = callBackContext_ptr;
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::resetScriptDoneCallBack(void)

//  DESCRIPTION     : Reset the script done callback.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// reset the callback function and context
	scriptDoneCallBackFunctionM_ptr = NULL;
	scriptDoneCallBackContextM_ptr = NULL;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::terminateConnection()

//  DESCRIPTION     : Terminate the connection (abruptly).
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// indicate that we are stopping the session
	isSessionStoppedM = true;

	// reset the association
	associationM.reset();

	// return result
	return true;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(ASSOCIATE_RQ_CLASS *associateRq_ptr, string identifier)

//  DESCRIPTION     : Send Associate Request during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateRq_ptr == NULL) return false;

	// first make connection to peer
	bool result = connectOnTcpIp();
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't connect over TCP/IP to remote host \"%s\" using port number %d", getRemoteHostname(), getRemoteConnectPort());
		}
		return result;
	}

	// search for associate request in the Data Warehouse
	// - we assume that if the presentation contexts are already set in the associate request
	// we don't need to look for the associate request in the warehouse
	// - some VTS scripts do not delete the the warehouse content on completion and so
	// some content remains that should not be used
	if (associateRq_ptr->noPresentationContexts() == 0)
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), associateRq_ptr->getWidType());
		if (wid_ptr)
		{
			// update associateRq_ptr to matching associate request in Data Warehouse
			associateRq_ptr = static_cast<ASSOCIATE_RQ_CLASS*>(wid_ptr);

			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", WIDName(associateRq_ptr->getWidType()), identifier.c_str(), timeStamp());
			}
		}
		else
		{
			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_INFO, 1, "%s %s - not found in Data Warehouse", WIDName(associateRq_ptr->getWidType()), identifier.c_str());
			}
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", WIDName(associateRq_ptr->getWidType()), timeStamp());
		}
	}

	// update associate request defaults
	updateDefaults(associateRq_ptr);

	// loop through all requested PCs
	for (UINT i = 0; i < associateRq_ptr->noPresentationContexts(); i++)
	{
		// get the presentation context
		PRESENTATION_CONTEXT_RQ_CLASS reqPC = associateRq_ptr->getPresentationContext(i);
		string reqSopUid = (char*) (reqPC.getAbstractSyntaxName().get());
		if(reqSopUid == STORAGE_COMMITMENT_PUSH_MODEL_SOP_CLASS_UID)
		{
			if(sutRoleM == UP_REQUESTOR)
				associateRq_ptr->setScpScuRoleSelect(reqPC.getAbstractSyntaxName(), 1, 0);
			else if(sutRoleM == UP_ACCEPTOR)
				associateRq_ptr->setScpScuRoleSelect(reqPC.getAbstractSyntaxName(), 0, 1);
		}
	}	

	// try sending an associate request
	result = associationM.send(associateRq_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s over TCP/IP to remote host %s on port number %d", WIDName(associateRq_ptr->getWidType()), getRemoteHostname(), getRemoteConnectPort());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(ASSOCIATE_AC_CLASS *associateAc_ptr, string identifier)

//  DESCRIPTION     : Send Associate Accept during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateAc_ptr == NULL) return false;

	// search for associate accept in the Data Warehouse
	// - we assume that if the presentation contexts are already set in the associate accept
	// we don't need to look for the associate accept in the warehouse
	// - some VTS scripts do not delete the the warehouse content on completion and so
	// some content remains that should not be used
	if (associateAc_ptr->noPresentationContexts() == 0)
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), associateAc_ptr->getWidType());
		if (wid_ptr)
		{
			// update associateAc_ptr to matching associate accept in Data Warehouse
			associateAc_ptr = static_cast<ASSOCIATE_AC_CLASS*>(wid_ptr);

			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", WIDName(associateAc_ptr->getWidType()), identifier.c_str(), timeStamp());
			}
		}
		else
		{
			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_INFO, 1, "%s %s - not found in Data Warehouse", WIDName(associateAc_ptr->getWidType()), identifier.c_str());
			}
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", WIDName(associateAc_ptr->getWidType()), timeStamp());
		}
	}
	// update associate accept defaults
	updateDefaults(associateAc_ptr);

	// loop through all accepted PCs
	for (UINT i = 0; i < associateAc_ptr->noPresentationContexts(); i++)
	{
		// get the presentation context
		PRESENTATION_CONTEXT_AC_CLASS accPC = associateAc_ptr->getPresentationContext(i);
		if(accPC.getResultReason() == ACCEPTANCE)
			accPresentationContextM.add(accPC);
	}

	// try sending an associate accept
	bool result = associationM.send(associateAc_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s", WIDName(associateAc_ptr->getWidType()));
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(ASSOCIATE_RJ_CLASS *associateRj_ptr, string identifier)

//  DESCRIPTION     : Send Associate Reject during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateRj_ptr == NULL) return false;

	// search for associate reject in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), associateRj_ptr->getWidType());
	if (wid_ptr)
	{
		// update associateRj_ptr to matching associate reject in Data Warehouse
		associateRj_ptr = static_cast<ASSOCIATE_RJ_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", WIDName(associateRj_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", WIDName(associateRj_ptr->getWidType()), timeStamp());
		}
	}

	// try sending an associate reject
	bool result = associationM.send(associateRj_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s", WIDName(associateRj_ptr->getWidType()));
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(RELEASE_RQ_CLASS *releaseRq_ptr, string identifier)

//  DESCRIPTION     : Send Release Request during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (releaseRq_ptr == NULL) return false;

	// search for release request in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), releaseRq_ptr->getWidType());
	if (wid_ptr)
	{
		// update releaseRq_ptr to matching release request in Data Warehouse
		releaseRq_ptr = static_cast<RELEASE_RQ_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", WIDName(releaseRq_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", WIDName(releaseRq_ptr->getWidType()), timeStamp());
		}
	}

	// try sending a release request
	bool result = associationM.send(releaseRq_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s", WIDName(releaseRq_ptr->getWidType()));
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(RELEASE_RP_CLASS *releaseRp_ptr, string identifier)

//  DESCRIPTION     : Send Release Reesponse during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (releaseRp_ptr == NULL) return false;

	// search for release response in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), releaseRp_ptr->getWidType());
	if (wid_ptr)
	{
		// update releaseRp_ptr to matching release response in Data Warehouse
		releaseRp_ptr = static_cast<RELEASE_RP_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", WIDName(releaseRp_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", WIDName(releaseRp_ptr->getWidType()), timeStamp());
		}
	}

	// try sending a release response
	bool result = associationM.send(releaseRp_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s", WIDName(releaseRp_ptr->getWidType()));
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(ABORT_RQ_CLASS *abortRq_ptr, string identifier)

//  DESCRIPTION     : Send Abort Request during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (abortRq_ptr == NULL) return false;

	// search for abort request in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), abortRq_ptr->getWidType());
	if (wid_ptr)
	{
		// update abortRq_ptr to matching abort request in Data Warehouse
		abortRq_ptr = static_cast<ABORT_RQ_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", WIDName(abortRq_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", WIDName(abortRq_ptr->getWidType()), timeStamp());
		}
	}

	// try sending an abort request
	bool result = associationM.send(abortRq_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s", WIDName(abortRq_ptr->getWidType()));
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(UNKNOWN_PDU_CLASS *unknownPdu_ptr, string identifier)

//  DESCRIPTION     : Send Unknown Pdu during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (unknownPdu_ptr == NULL) return false;

	// search for unknown pdu in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), unknownPdu_ptr->getWidType());
	if (wid_ptr)
	{
		// update unknownPdu_ptr to matching unknown pdu in Data Warehouse
		unknownPdu_ptr = static_cast<UNKNOWN_PDU_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", WIDName(unknownPdu_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", WIDName(unknownPdu_ptr->getWidType()), timeStamp());
		}
	}

	// try sending an unknown pdu
	bool result = associationM.send(unknownPdu_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s over TCP/IP to remote host %s on port number %d", WIDName(unknownPdu_ptr->getWidType()), getRemoteHostname(), getRemoteConnectPort());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(DCM_COMMAND_CLASS *command_ptr)

//  DESCRIPTION     : Send DICOM Command during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (command_ptr == NULL) return false;

	// search for command in the Data Warehouse
	if (command_ptr->getIdentifier())
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(command_ptr->getIdentifier(), command_ptr->getWidType());
		if (wid_ptr)
		{
			// update command_ptr to matching command in Data Warehouse
			command_ptr = static_cast<DCM_COMMAND_CLASS*>(wid_ptr);
		}
	}

	// log action
	if (loggerM_ptr)
	{
		if (command_ptr->getIdentifier())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), timeStamp());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", mapCommandName(command_ptr->getCommandId()), timeStamp());
		}
	}

	// try sending a DICOM Command
	bool result = associationM.send(command_ptr);
	if (result)
	{
		// save the address of this command - being that last command sent
		// - it may be needed when a command is received for validation purposes
		lastCommandSentM_ptr = command_ptr->cloneAttributes();
		lastDatasetSentM_ptr = NULL;
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s", mapCommandName(command_ptr->getCommandId()));
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr)

//  DESCRIPTION     : Send DICOM Command and Dataset during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointers
	if ((command_ptr == NULL) ||
		(dataset_ptr == NULL)) return false;

	// search for command in the Data Warehouse
	if (command_ptr->getIdentifier())
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(command_ptr->getIdentifier(), command_ptr->getWidType());
		if (wid_ptr)
		{
			// update command_ptr to matching command in Data Warehouse
			command_ptr = static_cast<DCM_COMMAND_CLASS*>(wid_ptr);
		}
	}

	// now search for dataset in the Data Warehouse
	if (dataset_ptr->getIdentifier())
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(dataset_ptr->getIdentifier(), WID_DATASET);
		if (wid_ptr)
		{
			// update dataset_ptr to matching dataset in Data Warehouse
			dataset_ptr = static_cast<DCM_DATASET_CLASS*>(wid_ptr);
		}
	}

	// log action
	if (loggerM_ptr)
	{
		if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()) &&
			(dataset_ptr->getIdentifier()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s %s %s (%s)", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName(), dataset_ptr->getIdentifier(), timeStamp());
		}
		else if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s %s (%s)", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName(), timeStamp());
		}
		else if ((dataset_ptr->getIodName()) &&
			(dataset_ptr->getIdentifier()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s %s (%s)", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName(), dataset_ptr->getIdentifier(), timeStamp());
		}
		else if (dataset_ptr->getIodName())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName(), timeStamp());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", mapCommandName(command_ptr->getCommandId()), timeStamp());
		}
	}

	// set up some encoding flags
	bool addGroupLength = addGroupLengthM;
	bool definedLength = defineSqLengthM;
	bool populateWithAttributes = autoType2AttributesM;
	if (scriptExecutionContextM_ptr)
	{
		addGroupLength = scriptExecutionContextM_ptr->getAddGroupLength();
		definedLength = scriptExecutionContextM_ptr->getDefineSqLength();
		populateWithAttributes = scriptExecutionContextM_ptr->getPopulateWithAttributes();
	}
	dataset_ptr->setDefineGroupLengths(addGroupLength);
	dataset_ptr->setDefineSqLengths(definedLength);
	dataset_ptr->setPopulateWithAttributes(populateWithAttributes);

	// try sending a DICOM command and dataset
	bool result = associationM.send(command_ptr, dataset_ptr);
	if (result)
	{
		// save the address of this command/dataset - being that last command/dataset sent
		// - it may be needed when a command/dataset is received for validation purposes
		// - example C-FIND-RSP validation is better if we know what the C-FIND-RQ identifier contained
		lastCommandSentM_ptr = command_ptr->cloneAttributes();
		lastDatasetSentM_ptr = dataset_ptr->cloneAttributes();
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s %s", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(DCM_COMMAND_CLASS *command_ptr, int pcId)

//  DESCRIPTION     : Send DICOM Command during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (command_ptr == NULL) return false;

	// search for command in the Data Warehouse
	if (command_ptr->getIdentifier())
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(command_ptr->getIdentifier(), command_ptr->getWidType());
		if (wid_ptr)
		{
			// update command_ptr to matching command in Data Warehouse
			command_ptr = static_cast<DCM_COMMAND_CLASS*>(wid_ptr);
		}
	}

	// log action
	if (loggerM_ptr)
	{
		if (command_ptr->getIdentifier())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), timeStamp());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", mapCommandName(command_ptr->getCommandId()), timeStamp());
		}
	}

	// try sending a DICOM Command
	bool result = associationM.send(command_ptr, pcId);
	if (result)
	{
		// save the address of this command - being that last command sent
		// - it may be needed when a command is received for validation purposes
		lastCommandSentM_ptr = command_ptr->cloneAttributes();
		lastDatasetSentM_ptr = NULL;
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s", mapCommandName(command_ptr->getCommandId()));
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::send(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr, int pcId)

//  DESCRIPTION     : Send DICOM Command and Dataset during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointers
	if ((command_ptr == NULL) ||
		(dataset_ptr == NULL)) return false;

	// search for command in the Data Warehouse
	if (command_ptr->getIdentifier())
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(command_ptr->getIdentifier(), command_ptr->getWidType());
		if (wid_ptr)
		{
			// update command_ptr to matching command in Data Warehouse
			command_ptr = static_cast<DCM_COMMAND_CLASS*>(wid_ptr);
		}
	}

	// now search for dataset in the Data Warehouse
	if (dataset_ptr->getIdentifier())
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve(dataset_ptr->getIdentifier(), WID_DATASET);
		if (wid_ptr)
		{
			// update dataset_ptr to matching dataset in Data Warehouse
			dataset_ptr = static_cast<DCM_DATASET_CLASS*>(wid_ptr);
		}
	}

	// log action
	if (loggerM_ptr)
	{
		if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()) &&
			(dataset_ptr->getIdentifier()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s %s %s (%s)", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName(), dataset_ptr->getIdentifier(), timeStamp());
		}
		else if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s %s (%s)", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName(), timeStamp());
		}
		else if ((dataset_ptr->getIodName()) &&
			(dataset_ptr->getIdentifier()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s %s (%s)", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName(), dataset_ptr->getIdentifier(), timeStamp());
		}
		else if (dataset_ptr->getIodName())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s %s (%s)", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName(), timeStamp());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "SEND %s (%s)", mapCommandName(command_ptr->getCommandId()), timeStamp());
		}
	}

	// set up some encoding flags
	bool addGroupLength = addGroupLengthM;
	bool definedLength = defineSqLengthM;
	bool populateWithAttributes = autoType2AttributesM;
	if (scriptExecutionContextM_ptr)
	{
		addGroupLength = scriptExecutionContextM_ptr->getAddGroupLength();
		definedLength = scriptExecutionContextM_ptr->getDefineSqLength();
		populateWithAttributes = scriptExecutionContextM_ptr->getPopulateWithAttributes();
	}
	dataset_ptr->setDefineGroupLengths(addGroupLength);
	dataset_ptr->setDefineSqLengths(definedLength);
	dataset_ptr->setPopulateWithAttributes(populateWithAttributes);

	// try sending a DICOM command and dataset
	bool result = associationM.send(command_ptr, dataset_ptr, pcId);
	if (result)
	{
		// save the address of this command/dataset - being that last command/dataset sent
		// - it may be needed when a command/dataset is received for validation purposes
		// - example C-FIND-RSP validation is better if we know what the C-FIND-RQ identifier contained
		lastCommandSentM_ptr = command_ptr->cloneAttributes();
		lastDatasetSentM_ptr = dataset_ptr->cloneAttributes();
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Can't send %s %s", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName());
		}
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(ASSOCIATE_RQ_CLASS *associateRq_ptr, string identifier)

//  DESCRIPTION     : Receive Associate Request during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateRq_ptr == NULL) return false;

	// first listen for connection from peer
	bool result = listenOnTcpIp();
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_INFO, 1, "Stopped listening on TCP/IP port number %d", getLocalListenPort());
		}
		return result;
	}

	// search for associate request in the Data Warehouse
	// - we assume that if the presentation contexts are already set in the associate request
	// we don't need to look for the associate request in the warehouse
	// - some VTS scripts do not delete the the warehouse content on completion and so
	// some content remains that should not be used
	if (associateRq_ptr->noPresentationContexts() == 0)
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), associateRq_ptr->getWidType());
		if (wid_ptr)
		{
			// update associateRq_ptr to matching associate request in Data Warehouse
			associateRq_ptr = static_cast<ASSOCIATE_RQ_CLASS*>(wid_ptr);

			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", WIDName(associateRq_ptr->getWidType()), identifier.c_str(), timeStamp());
			}
		}
		else
		{
			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_INFO, 1, "%s %s - not found in Data Warehouse", WIDName(associateRq_ptr->getWidType()), identifier.c_str());
			}
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", WIDName(associateRq_ptr->getWidType()), timeStamp());
		}
	}

	// clear old accepted PCs if any
	while (accPresentationContextM.getSize())
	{
		accPresentationContextM.removeAt(0);
	}

	// try receiving an associate request
	ASSOCIATE_RQ_CLASS *rxAssociateRq_ptr = NULL;
	result = associationM.receive(&rxAssociateRq_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", WIDName(associateRq_ptr->getWidType()));
		}
		return result;
	}

	// log it
	if (rxAssociateRq_ptr)
	{
        // serialize it
        if (serializerM_ptr)
        {
            serializerM_ptr->SerializeReceive(rxAssociateRq_ptr);
        }

        bool strictValidation = getStrictValidation();
		VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
		if (scriptExecutionContextM_ptr)
		{
			strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
			validationFlag = scriptExecutionContextM_ptr->getValidationFlag();
		}
   	    setSerializerStrictValidation(strictValidation);
		VALIDATION->setStrictValidation(strictValidation);
        result = validate(rxAssociateRq_ptr, associateRq_ptr, validationFlag);

		// receive cleanup
		delete rxAssociateRq_ptr;
	}
	else
	{
		result = false;
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(ASSOCIATE_AC_CLASS *associateAc_ptr, string identifier)

//  DESCRIPTION     : Receive Associate Accept during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateAc_ptr == NULL) return false;

	// search for associate accept in the Data Warehouse
	// - we assume that if the presentation contexts are already set in the associate accept
	// we don't need to look for the associate accept in the warehouse
	// - some VTS scripts do not delete the the warehouse content on completion and so
	// some content remains that should not be used
	if (associateAc_ptr->noPresentationContexts() == 0)
	{
		BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), associateAc_ptr->getWidType());
		if (wid_ptr)
		{
			// update associateAc_ptr to matching associate accept in Data Warehouse
			associateAc_ptr = static_cast<ASSOCIATE_AC_CLASS*>(wid_ptr);

			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", WIDName(associateAc_ptr->getWidType()), identifier.c_str(), timeStamp());
			}
		}
		else
		{
			// log action
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_INFO, 1, "%s %s - not found in Data Warehouse", WIDName(associateAc_ptr->getWidType()), identifier.c_str());
			}
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", WIDName(associateAc_ptr->getWidType()), timeStamp());
		}
	}

	// try receiving an associate accept
	ASSOCIATE_AC_CLASS *rxAssociateAc_ptr = NULL;
	bool result = associationM.receive(&rxAssociateAc_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", WIDName(associateAc_ptr->getWidType()));
		}
		return result;
	}

    if (rxAssociateAc_ptr)
    {
        // serialize it
        if (serializerM_ptr)
        {
            serializerM_ptr->SerializeReceive(rxAssociateAc_ptr);
        }

    	// update any zero presentation context ids in the script object
	    associateAc_ptr->setZeroPresentationContextIds(rxAssociateAc_ptr);

		bool strictValidation = getStrictValidation();
		VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
		if (scriptExecutionContextM_ptr)
		{
			strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
			validationFlag = scriptExecutionContextM_ptr->getValidationFlag();
		}
   	    setSerializerStrictValidation(strictValidation);
		VALIDATION->setStrictValidation(strictValidation);
        result = validate(rxAssociateAc_ptr, associateAc_ptr, validationFlag);

		// receive cleanup
		delete rxAssociateAc_ptr;
    }
    else
    {
        result = false;
    }

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(ASSOCIATE_RJ_CLASS *associateRj_ptr, string identifier)

//  DESCRIPTION     : Receive Associate Reject during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (associateRj_ptr == NULL) return false;

	// search for associate reject in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), associateRj_ptr->getWidType());
	if (wid_ptr)
	{
		// update associateRj_ptr to matching associate reject in Data Warehouse
		associateRj_ptr = static_cast<ASSOCIATE_RJ_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", WIDName(associateRj_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", WIDName(associateRj_ptr->getWidType()), timeStamp());
		}
	}

	// try receiving an associate reject
	ASSOCIATE_RJ_CLASS *rxAssociateRj_ptr;
	bool result = associationM.receive(&rxAssociateRj_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", WIDName(associateRj_ptr->getWidType()));
		}
		return result;
	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeReceive(rxAssociateRj_ptr);
    }

	// now validate the received associate reject
	bool strictValidation = getStrictValidation();
	VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
	if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
		validationFlag = scriptExecutionContextM_ptr->getValidationFlag();
	}
 	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
    result = validate(rxAssociateRj_ptr, associateRj_ptr, validationFlag);

	// receive cleanup
	delete rxAssociateRj_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(RELEASE_RQ_CLASS *releaseRq_ptr, string identifier)

//  DESCRIPTION     : Receive Release Request during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (releaseRq_ptr == NULL) return false;

	// search for release request in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), releaseRq_ptr->getWidType());
	if (wid_ptr)
	{
		// update releaseRq_ptr to matching release request in Data Warehouse
		releaseRq_ptr = static_cast<RELEASE_RQ_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", WIDName(releaseRq_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", WIDName(releaseRq_ptr->getWidType()), timeStamp());
		}
	}

	// try receiving a release request
	RELEASE_RQ_CLASS *rxReleaseRq_ptr;
	bool result = associationM.receive(&rxReleaseRq_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", WIDName(releaseRq_ptr->getWidType()));
		}
		return result;
	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeReceive(rxReleaseRq_ptr);
    }

	// now validate the received release request
	bool strictValidation = getStrictValidation();
	VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
	if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
		validationFlag = scriptExecutionContextM_ptr->getValidationFlag();
	}
 	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
    result = validate(rxReleaseRq_ptr, releaseRq_ptr, validationFlag);

	// receive cleanup
	delete rxReleaseRq_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(RELEASE_RP_CLASS *releaseRp_ptr, string identifier)

//  DESCRIPTION     : Receive Release Response during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (releaseRp_ptr == NULL) return false;

	// search for release response in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), releaseRp_ptr->getWidType());
	if (wid_ptr)
	{
		// update releaseRp_ptr to matching release response in Data Warehouse
		releaseRp_ptr = static_cast<RELEASE_RP_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", WIDName(releaseRp_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", WIDName(releaseRp_ptr->getWidType()), timeStamp());
		}
	}

	// try receiving a release response
	RELEASE_RP_CLASS *rxReleaseRp_ptr;
	bool result = associationM.receive(&rxReleaseRp_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", WIDName(releaseRp_ptr->getWidType()));
		}
		return result;
	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeReceive(rxReleaseRp_ptr);
    }

	// now validate the received release response
	bool strictValidation = getStrictValidation();
	VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
	if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
		validationFlag = scriptExecutionContextM_ptr->getValidationFlag();
	}
   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
    result = validate(rxReleaseRp_ptr, releaseRp_ptr, validationFlag);

	// receive cleanup
	delete rxReleaseRp_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(ABORT_RQ_CLASS *abortRq_ptr, string identifier)

//  DESCRIPTION     : Receive Abort Request during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (abortRq_ptr == NULL) return false;

	// search for abort request in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), abortRq_ptr->getWidType());
	if (wid_ptr)
	{
		// update abortRq_ptr to matching associate abort in Data Warehouse
		abortRq_ptr = static_cast<ABORT_RQ_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", WIDName(abortRq_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", WIDName(abortRq_ptr->getWidType()), timeStamp());
		}
	}

	// try receiving an abort request
	ABORT_RQ_CLASS *rxAbortRq_ptr;
	bool result = associationM.receive(&rxAbortRq_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", WIDName(abortRq_ptr->getWidType()));
		}
		return result;
	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeReceive(rxAbortRq_ptr);
    }

	// now validate the received abort request
	bool strictValidation = getStrictValidation();
	VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
	if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
		validationFlag = scriptExecutionContextM_ptr->getValidationFlag();
	}
   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
    result = validate(rxAbortRq_ptr, abortRq_ptr, validationFlag);

	// receive cleanup
	delete rxAbortRq_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(UNKNOWN_PDU_CLASS *unknownPdu_ptr, string identifier)

//  DESCRIPTION     : Receive Unknown PDU during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check for valid pointer
	if (unknownPdu_ptr == NULL) return false;

	// search for unknown pdu in the Data Warehouse
	BASE_WAREHOUSE_ITEM_DATA_CLASS *wid_ptr = WAREHOUSE->retrieve((char*) identifier.c_str(), unknownPdu_ptr->getWidType());
	if (wid_ptr)
	{
		// update unknownPdu_ptr to matching unknown pdu in Data Warehouse
		unknownPdu_ptr = static_cast<UNKNOWN_PDU_CLASS*>(wid_ptr);

		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", WIDName(unknownPdu_ptr->getWidType()), identifier.c_str(), timeStamp());
		}
	}
	else
	{
		// log action
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", WIDName(unknownPdu_ptr->getWidType()), timeStamp());
		}
	}

	// try receiving an unknown pdu
	UNKNOWN_PDU_CLASS *rxUnknownPdu_ptr;
	bool result = associationM.receive(&rxUnknownPdu_ptr);
	if (!result)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", WIDName(unknownPdu_ptr->getWidType()));
		}
		return result;
	}

	// now validate the received unknown pdu
	bool strictValidation = getStrictValidation();
	VALIDATION_CONTROL_FLAG_ENUM validationFlag = ALL;
	if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
		validationFlag = scriptExecutionContextM_ptr->getValidationFlag();
	}
   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
	result = VALIDATION->validate(rxUnknownPdu_ptr, unknownPdu_ptr, validationFlag, serializerM_ptr);

	// receive cleanup
	delete rxUnknownPdu_ptr;

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(DCM_COMMAND_CLASS** rx_command_ptr, DCM_COMMAND_CLASS* ref_command_ptr)

//  DESCRIPTION     : Receive DICOM Command during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	DCM_COMMAND_CLASS *rxCommand_ptr = NULL;
	DCM_DATASET_CLASS *rxDataset_ptr = NULL;
	bool result = true;

	associationM.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	AE_SESSION_CLASS ae_session;
	ae_session.SetName (this->applicationEntityNameM);
	ae_session.SetVersion (this->applicationEntityVersionM);
    if (getScriptExecutionContext())
    {
        ae_session.SetName(getScriptExecutionContext()->getApplicationEntityName());
        ae_session.SetVersion(getScriptExecutionContext()->getApplicationEntityVersion());
    }

	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", mapCommandName(ref_command_ptr->getCommandId()), timeStamp());
	}

	// try to receive a command (may also get a dataset)
	if (associationM.receiveCommandDataset(&rxCommand_ptr, &rxDataset_ptr, &ae_session, true) != RECEIVE_MSG_SUCCESSFUL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s - as expected in DICOMScript.", mapCommandName(ref_command_ptr->getCommandId()));
		}
		return false;
	}

	// log it && update any labelled values
	if (rxCommand_ptr)
	{
		// check that command received matches command expected
		if (ref_command_ptr->getCommandId() != rxCommand_ptr->getCommandId())
		{
			// there is a mis-match
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Command mis-match - expected %s in DICOMScript but actually received %s", mapCommandName(ref_command_ptr->getCommandId()), mapCommandName(rxCommand_ptr->getCommandId()));
			}
			return false;
		}

		// update any labelled values
		updateLabelledValues(ref_command_ptr, rxCommand_ptr);
	}
	else
	{
		//something went wrong
		return false;
	}

	// check if we got a dataset too - we were not expecting it
	if (rxDataset_ptr)
	{
		// check for strict validation
		if (getStrictValidation())
		{
			// not expecting a dataset - this is an error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Unexpected DICOM Dataset received - DICOMScript does not match Product behaviour.");
			}

			// did not expect a dataset too
			result = false;
		}
		else
		{
			// let's accept the dataset even though we were not expecting it
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_WARNING, 2, "Unexpected DICOM Dataset received - DICOMScript does not match Product behaviour.");
			}
		}

	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeReceive(rxCommand_ptr, rxDataset_ptr);
    }

	if (rxDataset_ptr)
	{
        // - not expecting this
		// delete the dataset
		delete rxDataset_ptr;
	}

	// set return parameter
	*rx_command_ptr = rxCommand_ptr;
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::receive(DCM_COMMAND_CLASS** rx_command_ptr,
								   DCM_DATASET_CLASS** rx_dataset_ptr,
								   DCM_COMMAND_CLASS*  ref_command_ptr,
								   DCM_DATASET_CLASS*  ref_dataset_ptr)

//  DESCRIPTION     : Receive DICOM Command and Dataset during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	DCM_COMMAND_CLASS *rxCommand_ptr = NULL;
	DCM_DATASET_CLASS *rxDataset_ptr = NULL;
	bool result = true;
	char *iodName_ptr = NULL;

	associationM.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	AE_SESSION_CLASS ae_session;
	ae_session.SetName (this->applicationEntityNameM);
	ae_session.SetVersion (this->applicationEntityVersionM);
    if (getScriptExecutionContext())
    {
        ae_session.SetName(getScriptExecutionContext()->getApplicationEntityName());
        ae_session.SetVersion(getScriptExecutionContext()->getApplicationEntityVersion());
    }

	// check if we can get the IOD name from the reference dataset
	if (ref_dataset_ptr)
	{
		// - may still be NULL
		iodName_ptr = (char*) ref_dataset_ptr->getIodName();
	}

	// log action
	if (loggerM_ptr)
	{
		if (iodName_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s %s (%s)", mapCommandName(ref_command_ptr->getCommandId()), iodName_ptr, timeStamp());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "RECEIVE %s (%s)", mapCommandName(ref_command_ptr->getCommandId()), timeStamp());
		}
	}

	// try to receive a command and dataset
	if (associationM.receiveCommandDataset(&rxCommand_ptr, &rxDataset_ptr, &ae_session, true) != RECEIVE_MSG_SUCCESSFUL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to receive %s %s - as expected in DICOMScript.", mapCommandName(ref_command_ptr->getCommandId()), iodName_ptr);
		}

		return false;
	}

	// log it
	if (rxCommand_ptr)
	{
		// check that command received matches command expected
		if (ref_command_ptr->getCommandId() != rxCommand_ptr->getCommandId())
		{
			// there is a mis-match
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Command mis-match - expected %s in DICOMScript but actually received %s", mapCommandName(ref_command_ptr->getCommandId()), mapCommandName(rxCommand_ptr->getCommandId()));
			}
			return false;
		}

		// update any labelled values
		updateLabelledValues(ref_command_ptr, rxCommand_ptr);
	}
	else
	{
		//something went wrong
		return false;
	}

	// check if we got a dataset too - we were expecting one
	// From the script it may be possible that we don't now whether
	// a dataset will be sent. So we check on a received dataset and
	// we check consistency with the dataset type attribute in the command
	// If this is correct we continue
	if (rxDataset_ptr)
	{
		// set name
		rxDataset_ptr->setIodName(iodName_ptr);

		// update any labelled values
		updateLabelledValues(ref_dataset_ptr, rxDataset_ptr);

		// if storage - analysis the relationship
		string msg;
		if (rxCommand_ptr->getCommandId() == DIMSE_CMD_CSTORE_RQ)
		{
			RELATIONSHIP->analyseStorageDataset(rxDataset_ptr, msg, loggerM_ptr);
		}
	}
	else
	{
		// check if data set type attribute is available
		UINT16 status = DCM_STATUS_SUCCESS;
		rxCommand_ptr->getUSValue(TAG_STATUS, &status);

		if (loggerM_ptr)
		{
			if (status != DCM_STATUS_SUCCESS)
			{
				logStatus(loggerM_ptr, status);
			}

			UINT16 datasettype = NO_DATA_SET;
			rxCommand_ptr->getUSValue(TAG_DATA_SET_TYPE, &datasettype);

			// expecting a dataset - this is an error
			if (datasettype == NO_DATA_SET)
			{
				// check for strict validation
				if (getStrictValidation())
				{
					loggerM_ptr->text(LOG_ERROR, 2, "Expected DICOM Dataset - none received from Product");

					// expected a dataset too
					result = false;
				}
				else
				{
					loggerM_ptr->text(LOG_WARNING, 2, "Expected DICOM Dataset - none received from Product");
				}
			}
		}
	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeReceive(rxCommand_ptr, rxDataset_ptr);
    }

	// set return parameters and return result
	*rx_command_ptr = rxCommand_ptr;
	*rx_dataset_ptr = rxDataset_ptr;

	// return result
	return result;
}

//>>===========================================================================

RECEIVE_MSG_ENUM SCRIPT_SESSION_CLASS::receive(RECEIVE_MESSAGE_UNION_CLASS **rx_msg_union_ptr_ptr)

//  DESCRIPTION     : Receive any kind of message during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The union message type will indicate which message has been received.
//<<===========================================================================
{
	*rx_msg_union_ptr_ptr = NULL;

	associationM.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	AE_SESSION_CLASS ae_session;
	ae_session.SetName (this->applicationEntityNameM);
	ae_session.SetVersion (this->applicationEntityVersionM);
    if (getScriptExecutionContext())
    {
        ae_session.SetName(getScriptExecutionContext()->getApplicationEntityName());
        ae_session.SetVersion(getScriptExecutionContext()->getApplicationEntityVersion());
    }

    //
    // Determine whether a socket connection exists.
    // TODO: This chack can be dangerous in that we could end up listening for a connection in the wrong
    // DULP FSM state. Listening for a connection should be done in the FSM but that affects the freedom
    // of command order during script execution - to be further investigated...
    //
    bool socketIsConnected = false;
    if (
        associationM.getSocket() == NULL ||
        !(associationM.getSocket()->isConnected())
        )
    {
        //
        // first listen for connection from peer
        //
        socketIsConnected = listenOnTcpIp();
        if (!socketIsConnected)
        {
            if (loggerM_ptr)
            {
                loggerM_ptr->text(LOG_INFO, 1, "Stopped listening on TCP/IP port number %d", getLocalListenPort());
            }
        }
    }
    else
    {
        socketIsConnected = true;
    }
    RECEIVE_MSG_ENUM status;
    status = ::RECEIVE_MSG_NO_CONNECTION;
    if (socketIsConnected)
    {
        // try to receive any message
        status = associationM.receive(rx_msg_union_ptr_ptr, &ae_session);
    }
    else
    {
        status = ::RECEIVE_MSG_NO_CONNECTION;
    }
	return status;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::getHasPendingDataInNetworkInputBuffer()

//  DESCRIPTION     : Get the of Pending data in Network Input Buffer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : 
//<<===========================================================================
{
	//
    // Determine whether a socket connection exists.
    // TODO: This chack can be dangerous in that we could end up listening for a connection in the wrong
    // DULP FSM state. Listening for a connection should be done in the FSM but that affects the freedom
    // of command order during script execution - to be further investigated...
    //
    bool hasPendingData = false;
    if (
        associationM.getSocket() != NULL ||
        (associationM.getSocket()->isConnected())
        )
    {
		hasPendingData = associationM.checkForPendingDataInNetworkInputBuffer();
	}
	return hasPendingData;
}


//>>===========================================================================

bool SCRIPT_SESSION_CLASS::importCommand(DIMSE_CMD_ENUM cmd, string identifier)

//  DESCRIPTION     : Receive DICOM Command during current session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The DICOM command will be received anonymously
//<<===========================================================================
{
	DCM_COMMAND_CLASS *rxCommand_ptr = NULL;
	DCM_DATASET_CLASS *rxDataset_ptr = NULL;
	bool result = true;

	associationM.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	AE_SESSION_CLASS ae_session;
	ae_session.SetName (this->applicationEntityNameM);
	ae_session.SetVersion (this->applicationEntityVersionM);
    if (getScriptExecutionContext())
    {
        ae_session.SetName(getScriptExecutionContext()->getApplicationEntityName());
        ae_session.SetVersion(getScriptExecutionContext()->getApplicationEntityVersion());
    }

	// log action
	if (loggerM_ptr)
	{
		if (identifier.length())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "IMPORT %s %s (%s)", mapCommandName(cmd), identifier.c_str(), timeStamp());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "IMPORT %s (%s)", mapCommandName(cmd), timeStamp());
		}
	}

	// try to receive a command (may also get a dataset)
	if (associationM.receiveCommandDataset(&rxCommand_ptr, &rxDataset_ptr, &ae_session, true) != RECEIVE_MSG_SUCCESSFUL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to import %s - as expected in DICOMScript", mapCommandName(cmd));
		}
		return false;
	}

	// log it
	if (rxCommand_ptr)
	{
		//copy identifier
		rxCommand_ptr->setIdentifier(identifier);

		//ensure the widtype for the command is correct
		rxCommand_ptr->setWidType(WAREHOUSE->dimse2widtype(cmd));

		// store received command for later validation
		if ((result = WAREHOUSE->store(rxCommand_ptr->getIdentifier(), rxCommand_ptr)) == false)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Storing %s %s in Data Warehouse", WIDName(rxCommand_ptr->getWidType()), identifier.c_str());
			}
		}
	}

	// check if we got a dataset too - we were not expecting it
	if (rxDataset_ptr)
	{
		// check for strict validation
		if (getStrictValidation())
		{
			// not expecting a dataset - this is an error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Unexpected DICOM Dataset received - DICOMScript does not match Product behaviour.");
			}

			// did not expect a dataset too
			result = false;
		}
		else
		{
			// let's accept the dataset even though we were not expecting it
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_WARNING, 2, "Unexpected DICOM Dataset received - DICOMScript does not match Product behaviour.");
			}
		}
	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeImport(rxCommand_ptr, rxDataset_ptr);
    }

    if (rxDataset_ptr)
    {
        // no expecting this
		// delete the dataset
		delete rxDataset_ptr;
    }

	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::importCommandDataset(DIMSE_CMD_ENUM cmd,
												string cmd_identifier,
												string iodname,
												string data_identifier)

//  DESCRIPTION     : Import DICOM Command and dataset during current session
//                    and store it in the warehouse
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	DCM_COMMAND_CLASS *rxCommand_ptr = NULL;
	DCM_DATASET_CLASS *rxDataset_ptr = NULL;
	bool result = true;
	char *iodName_ptr = NULL;

	associationM.setUnVrDefinitionLookUp(unVrDefinitionLookUpM);

	AE_SESSION_CLASS ae_session;
	ae_session.SetName (this->applicationEntityNameM);
	ae_session.SetVersion (this->applicationEntityVersionM);
    if (getScriptExecutionContext())
    {
        ae_session.SetName(getScriptExecutionContext()->getApplicationEntityName());
        ae_session.SetVersion(getScriptExecutionContext()->getApplicationEntityVersion());
    }

	if (iodname.length())
	{
		iodName_ptr = (char*) iodname.c_str();
	}

	// log action
	if (loggerM_ptr)
	{
		if ((cmd_identifier.length()) &&
			(iodName_ptr) &&
			(data_identifier.length()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "IMPORT %s %s %s %s (%s)", mapCommandName(cmd), cmd_identifier.c_str(), iodName_ptr, data_identifier.c_str(), timeStamp());
		}
		else if ((iodName_ptr) &&
			(data_identifier.length()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "IMPORT %s %s %s (%s)", mapCommandName(cmd), iodName_ptr, data_identifier.c_str(), timeStamp());
		}
		else if (iodName_ptr)
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "IMPORT %s %s (%s)", mapCommandName(cmd), iodName_ptr, timeStamp());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "IMPORT %s (%s)", mapCommandName(cmd), timeStamp());
		}
	}

	// try to receive a command and dataset
	if (associationM.receiveCommandDataset(&rxCommand_ptr, &rxDataset_ptr, &ae_session, true) != RECEIVE_MSG_SUCCESSFUL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Failed to import %s %s - as expected in DICOMScript", mapCommandName(cmd), iodName_ptr);
		}
		return false;
	}

	// log it
	if (rxCommand_ptr)
	{
		//copy identifier
		rxCommand_ptr->setIdentifier(cmd_identifier);

		//ensure the widtype for the command is correct
		rxCommand_ptr->setWidType(WAREHOUSE->dimse2widtype(cmd));

		// store received command for later validation
		if ((result = WAREHOUSE->store(cmd_identifier, rxCommand_ptr)) == false)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Storing %s %s in Data Warehouse", WIDName(rxCommand_ptr->getWidType()), cmd_identifier.c_str());
			}
		}
	}

	// check if we got a dataset too - we were expecting one
	if (rxDataset_ptr)
	{
		// copy identifier
		rxDataset_ptr->setIdentifier(data_identifier.c_str());

		// copy iod name
		rxDataset_ptr->setIodName(iodName_ptr);

		//ensure the widtype for the dataset is correct
		rxDataset_ptr->setWidType(WID_DATASET);

		// store received dataset for later validation
		if ((result = WAREHOUSE->store(rxDataset_ptr->getIdentifier(), rxDataset_ptr)) == false)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Storing %s %s in Data Warehouse", WIDName(rxDataset_ptr->getWidType()), rxDataset_ptr->getIdentifier());
			}
		}
	}
	else
	{
		// check if data set type attribute is available
		UINT16 status = DCM_STATUS_SUCCESS;
		rxCommand_ptr->getUSValue(TAG_STATUS, &status);

		if (loggerM_ptr)
		{
			if (status != DCM_STATUS_SUCCESS)
			{
				logStatus(loggerM_ptr, status);
			}

			UINT16 datasettype = NO_DATA_SET;
			rxCommand_ptr->getUSValue(TAG_DATA_SET_TYPE, &datasettype);

			// expecting a dataset - this is an error
			if (datasettype == NO_DATA_SET)
			{
				// check for strict validation
				if (getStrictValidation())
				{
					loggerM_ptr->text(LOG_ERROR, 2, "Expected DICOM Dataset - none received from Product");

					// expected a dataset too
					result = false;
				}
				else
				{
					loggerM_ptr->text(LOG_WARNING, 2, "Expected DICOM Dataset - none received from Product");
				}
			}
		}
	}

    // serialize it
    if (serializerM_ptr)
    {
        serializerM_ptr->SerializeImport(rxCommand_ptr, rxDataset_ptr);
    }

	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(DCM_COMMAND_CLASS *command_ptr, DCM_COMMAND_CLASS *refCommand_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate DICOM Command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The DICOM refCommand may be NULL.
//<<===========================================================================
{
	bool result = false;

	// check for valid pointers
	if (command_ptr == NULL) return false;

	// log action
	if (loggerM_ptr)
	{
		if (command_ptr->getIdentifier())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", mapCommandName(command_ptr->getCommandId()));
		}

		if ((refCommand_ptr) &&
			(refCommand_ptr->GetNrAttributes()))
		{
			if (refCommand_ptr->getIdentifier())
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s %s", mapCommandName(refCommand_ptr->getCommandId()), refCommand_ptr->getIdentifier());
			}
			else
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s", mapCommandName(refCommand_ptr->getCommandId()));
			}
		}
	}

	// validate the command - refCommand_ptr maybe NULL
	bool strictValidation = getStrictValidation();
    if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
	}

	// loop through all accepted PCs
	for (UINT i = 0; i < accPresentationContextM.getSize(); i++)
	{
		if(accPresentationContextM[i].getPresentationContextId() == command_ptr->getEncodePresentationContextId())
		{
			string tranferSyntax = (char*)(accPresentationContextM[i].getTransferSyntaxName().get());
			if(tranferSyntax != IMPLICIT_VR_LITTLE_ENDIAN)
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
			else
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
		}
	}

   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
	VALIDATION->setIncludeType3NotPresentInResults(getIncludeType3NotPresentInResults());
	result = VALIDATION->validate(command_ptr, 
								refCommand_ptr,
								lastCommandSentM_ptr,
								validationFlag, 
								serializerM_ptr);

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(DCM_COMMAND_CLASS *command_ptr, DCM_COMMAND_CLASS *refCommand_ptr, DCM_COMMAND_CLASS *lastCommand_ptr,VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate DICOM Command.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The DICOM refCommand may be NULL.
//<<===========================================================================
{
	bool result = false;

	// check for valid pointers
	if (command_ptr == NULL) return false;

	// log action
	if (loggerM_ptr)
	{
		if (command_ptr->getIdentifier())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", mapCommandName(command_ptr->getCommandId()));
		}

		if ((refCommand_ptr) &&
			(refCommand_ptr->GetNrAttributes()))
		{
			if (refCommand_ptr->getIdentifier())
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s %s", mapCommandName(refCommand_ptr->getCommandId()), refCommand_ptr->getIdentifier());
			}
			else
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s", mapCommandName(refCommand_ptr->getCommandId()));
			}
		}
	}

	// validate the command - refCommand_ptr maybe NULL
	bool strictValidation = getStrictValidation();
    if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
	}

	// loop through all accepted PCs
	for (UINT i = 0; i < accPresentationContextM.getSize(); i++)
	{
		if(accPresentationContextM[i].getPresentationContextId() == command_ptr->getEncodePresentationContextId())
		{
			string tranferSyntax = (char*)(accPresentationContextM[i].getTransferSyntaxName().get());
			if(tranferSyntax != IMPLICIT_VR_LITTLE_ENDIAN)
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
			else
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
		}
	}

   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
	VALIDATION->setIncludeType3NotPresentInResults(getIncludeType3NotPresentInResults());
	result = VALIDATION->validate(command_ptr, 
								refCommand_ptr,
								lastCommand_ptr,
								validationFlag, 
								serializerM_ptr);

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr, DCM_COMMAND_CLASS *refCommand_ptr, DCM_DATASET_CLASS *refDataset_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag, AE_SESSION_CLASS *ae_session_ptr)

//  DESCRIPTION     : Validate DICOM Command and Dataset stored in Data Warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The DICOM refCommand/refDataset may be NULL.
//<<===========================================================================
{
	UINT16			cs_group;
	UINT16			cs_element;
	UINT16			ds_group;
	UINT16			ds_element;
	string			ds_attr_name;
	string			cs_attr_name;

	// check for valid pointers
	if ((command_ptr == NULL) ||
		(dataset_ptr == NULL)) return false;

	// serialize the command & dataset (as display)
    BASE_SERIALIZER *serializer_ptr = getSerializer();
    if (serializer_ptr)
    {
        serializer_ptr->SerializeDisplay(command_ptr,dataset_ptr);
    }

	// log action
	if (loggerM_ptr)
	{
		if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()) &&
			(dataset_ptr->getIdentifier()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName(), dataset_ptr->getIdentifier());
		}
		else if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName());
		}
		else if (dataset_ptr->getIodName())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName());
		}
		else if (command_ptr->getIdentifier())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", mapCommandName(command_ptr->getCommandId()));
		}

		if ((refCommand_ptr) &&
			(refCommand_ptr->GetNrAttributes()))
		{
			if (refCommand_ptr->getIdentifier())
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s %s", mapCommandName(refCommand_ptr->getCommandId()), refCommand_ptr->getIdentifier());
			}
			else
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s", mapCommandName(refCommand_ptr->getCommandId()));
			}
		}
		if ((refDataset_ptr) &&
			(refDataset_ptr->GetNrAttributes()))
		{
			if (refDataset_ptr->getIdentifier())
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Dataset: %s %s", refDataset_ptr->getIodName(), refDataset_ptr->getIdentifier());
			}
			else
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Dataset: %s", refDataset_ptr->getIodName());
			}
		}
	}

	string	uid_ds;
	string	uid_cs;

	if (dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, uid_ds) == true)
	{
		if (command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, uid_cs) == true)
		{
			if ((uid_ds != uid_cs) && (command_ptr->getCommandId() == DIMSE_CMD_CSTORE_RQ))
			{
				cs_group = ((UINT16) (TAG_AFFECTED_SOP_CLASS_UID >> 16));
				cs_element = ((UINT16) (TAG_AFFECTED_SOP_CLASS_UID & 0x0000FFFF));
				ds_group = ((UINT16) (TAG_SOP_CLASS_UID >> 16));
				ds_element = ((UINT16) (TAG_SOP_CLASS_UID & 0x0000FFFF));

				ds_attr_name = DEFINITION->GetAttributeName (ds_group, ds_element);
				cs_attr_name = DEFINITION->GetAttributeName (cs_group, cs_element);
				loggerM_ptr->text(LOG_ERROR, 1,
								  "\"%s\" (%s) is not equal to \"%s\" (%s)",
								  ds_attr_name.c_str(),
								  uid_ds.c_str(),
								  cs_attr_name.c_str(),
								  uid_cs.c_str());
			}
		}
	}

	if (dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, uid_ds) == true)
	{
		if (command_ptr->getUIValue (TAG_AFFECTED_SOP_INSTANCE_UID, uid_cs) == true)
		{
			if ((uid_ds != uid_cs) && (command_ptr->getCommandId() == DIMSE_CMD_CSTORE_RQ))
			{
				cs_group = ((UINT16) (TAG_AFFECTED_SOP_INSTANCE_UID >> 16));
				cs_element = ((UINT16) (TAG_AFFECTED_SOP_INSTANCE_UID & 0x0000FFFF));
				ds_group = ((UINT16) (TAG_SOP_INSTANCE_UID >> 16));
				ds_element = ((UINT16) (TAG_SOP_INSTANCE_UID & 0x0000FFFF));

				ds_attr_name = DEFINITION->GetAttributeName (ds_group, ds_element);
				cs_attr_name = DEFINITION->GetAttributeName (cs_group, cs_element);
				loggerM_ptr->text(LOG_ERROR, 1,
								  "\"%s\" (%s) is not equal to \"%s\" (%s)",
								  ds_attr_name.c_str(),
								  uid_ds.c_str(),
								  cs_attr_name.c_str(),
								  uid_cs.c_str());;
			}
		}
	}

	// validate the command - refCommand_ptr maybe NULL
	bool strictValidation = getStrictValidation();
	if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
	}

	// loop through all accepted PCs
	for (UINT i = 0; i < accPresentationContextM.getSize(); i++)
	{
		if(accPresentationContextM[i].getPresentationContextId() == command_ptr->getEncodePresentationContextId())
		{
			string tranferSyntax = (char*)(accPresentationContextM[i].getTransferSyntaxName().get());
			if(tranferSyntax != IMPLICIT_VR_LITTLE_ENDIAN)
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
			else
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
		}
	}

	// loop through all accepted PCs
	for (UINT i = 0; i < accPresentationContextM.getSize(); i++)
	{
		if(accPresentationContextM[i].getPresentationContextId() == dataset_ptr->getEncodePresentationContextId())
		{
			string tranferSyntax = (char*)(accPresentationContextM[i].getTransferSyntaxName().get());
			if(tranferSyntax != IMPLICIT_VR_LITTLE_ENDIAN)
				dataset_ptr->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
			else
				dataset_ptr->setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
		}
	}

   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
	VALIDATION->setIncludeType3NotPresentInResults(getIncludeType3NotPresentInResults());
	bool result1 = VALIDATION->validate(command_ptr, 
										refCommand_ptr, 
										lastCommandSentM_ptr, 
										validationFlag, 
										serializerM_ptr);

	// validate the dataset - refDataset_ptr maybe NULL
	bool result2 = VALIDATION->validate(command_ptr,
										dataset_ptr, 
										refDataset_ptr,
										lastCommandSentM_ptr,
										lastDatasetSentM_ptr, 
										validationFlag, 
										serializerM_ptr, 
										ae_session_ptr);

	// If any of the previous validations went wrong, the total validation has failed.
	bool result = true;
	if ((!result1) ||
		(!result2))
	{
		result = false;
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(DCM_COMMAND_CLASS *command_ptr, DCM_DATASET_CLASS *dataset_ptr, DCM_COMMAND_CLASS *refCommand_ptr, DCM_DATASET_CLASS *refDataset_ptr, DCM_COMMAND_CLASS *lastCommand_ptr, DCM_DATASET_CLASS *lastDataset_ptr,VALIDATION_CONTROL_FLAG_ENUM validationFlag, AE_SESSION_CLASS *ae_session_ptr)

//  DESCRIPTION     : Validate DICOM Command and Dataset stored in Data Warehouse.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : The DICOM refCommand/refDataset may be NULL.
//<<===========================================================================
{
	UINT16			cs_group;
	UINT16			cs_element;
	UINT16			ds_group;
	UINT16			ds_element;
	string			ds_attr_name;
	string			cs_attr_name;

	// check for valid pointers
	if ((command_ptr == NULL) ||
		(dataset_ptr == NULL)) return false;

	// serialize the command & dataset (as display)
    BASE_SERIALIZER *serializer_ptr = getSerializer();
    if (serializer_ptr)
    {
        serializer_ptr->SerializeDisplay(command_ptr,dataset_ptr);
    }

	// log action
	if (loggerM_ptr)
	{
		if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()) &&
			(dataset_ptr->getIdentifier()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName(), dataset_ptr->getIdentifier());
		}
		else if ((command_ptr->getIdentifier()) &&
			(dataset_ptr->getIodName()))
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier(), dataset_ptr->getIodName());
		}
		else if (dataset_ptr->getIodName())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), dataset_ptr->getIodName());
		}
		else if (command_ptr->getIdentifier())
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s %s", mapCommandName(command_ptr->getCommandId()), command_ptr->getIdentifier());
		}
		else
		{
			loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", mapCommandName(command_ptr->getCommandId()));
		}

		if ((refCommand_ptr) &&
			(refCommand_ptr->GetNrAttributes()))
		{
			if (refCommand_ptr->getIdentifier())
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s %s", mapCommandName(refCommand_ptr->getCommandId()), refCommand_ptr->getIdentifier());
			}
			else
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Command: %s", mapCommandName(refCommand_ptr->getCommandId()));
			}
		}
		if ((refDataset_ptr) &&
			(refDataset_ptr->GetNrAttributes()))
		{
			if (refDataset_ptr->getIdentifier())
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Dataset: %s %s", refDataset_ptr->getIodName(), refDataset_ptr->getIdentifier());
			}
			else
			{
	            loggerM_ptr->text(LOG_NONE, 1, "Against reference Dataset: %s", refDataset_ptr->getIodName());
			}
		}
	}

	string	uid_ds;
	string	uid_cs;

	if (dataset_ptr->getUIValue(TAG_SOP_CLASS_UID, uid_ds) == true)
	{
		if (command_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, uid_cs) == true)
		{
			if ((uid_ds != uid_cs) && (command_ptr->getCommandId() == DIMSE_CMD_CSTORE_RQ))
			{
				cs_group = ((UINT16) (TAG_AFFECTED_SOP_CLASS_UID >> 16));
				cs_element = ((UINT16) (TAG_AFFECTED_SOP_CLASS_UID & 0x0000FFFF));
				ds_group = ((UINT16) (TAG_SOP_CLASS_UID >> 16));
				ds_element = ((UINT16) (TAG_SOP_CLASS_UID & 0x0000FFFF));

				ds_attr_name = DEFINITION->GetAttributeName (ds_group, ds_element);
				cs_attr_name = DEFINITION->GetAttributeName (cs_group, cs_element);
				loggerM_ptr->text(LOG_ERROR, 1,
								  "\"%s\" (%s) is not equal to \"%s\" (%s)",
								  ds_attr_name.c_str(),
								  uid_ds.c_str(),
								  cs_attr_name.c_str(),
								  uid_cs.c_str());
			}
		}
	}

	if (dataset_ptr->getUIValue(TAG_SOP_INSTANCE_UID, uid_ds) == true)
	{
		if (command_ptr->getUIValue (TAG_AFFECTED_SOP_INSTANCE_UID, uid_cs) == true)
		{
			if ((uid_ds != uid_cs) && (command_ptr->getCommandId() == DIMSE_CMD_CSTORE_RQ))
			{
				cs_group = ((UINT16) (TAG_AFFECTED_SOP_INSTANCE_UID >> 16));
				cs_element = ((UINT16) (TAG_AFFECTED_SOP_INSTANCE_UID & 0x0000FFFF));
				ds_group = ((UINT16) (TAG_SOP_INSTANCE_UID >> 16));
				ds_element = ((UINT16) (TAG_SOP_INSTANCE_UID & 0x0000FFFF));

				ds_attr_name = DEFINITION->GetAttributeName (ds_group, ds_element);
				cs_attr_name = DEFINITION->GetAttributeName (cs_group, cs_element);
				loggerM_ptr->text(LOG_ERROR, 1,
								  "\"%s\" (%s) is not equal to \"%s\" (%s)",
								  ds_attr_name.c_str(),
								  uid_ds.c_str(),
								  cs_attr_name.c_str(),
								  uid_cs.c_str());;
			}
		}
	}

	// validate the command - refCommand_ptr maybe NULL
	bool strictValidation = getStrictValidation();
	if (scriptExecutionContextM_ptr)
	{
		strictValidation = scriptExecutionContextM_ptr->getStrictValidation();
	}

	// loop through all accepted PCs
	for (UINT i = 0; i < accPresentationContextM.getSize(); i++)
	{
		if(accPresentationContextM[i].getPresentationContextId() == command_ptr->getEncodePresentationContextId())
		{
			string tranferSyntax = (char*)(accPresentationContextM[i].getTransferSyntaxName().get());
			if(tranferSyntax != IMPLICIT_VR_LITTLE_ENDIAN)
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
			else
				command_ptr->setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
		}
	}

	// loop through all accepted PCs
	for (UINT i = 0; i < accPresentationContextM.getSize(); i++)
	{
		if(accPresentationContextM[i].getPresentationContextId() == dataset_ptr->getEncodePresentationContextId())
		{
			string tranferSyntax = (char*)(accPresentationContextM[i].getTransferSyntaxName().get());
			if(tranferSyntax != IMPLICIT_VR_LITTLE_ENDIAN)
				dataset_ptr->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
			else
				dataset_ptr->setTransferVR(TRANSFER_ATTR_VR_IMPLICIT);
		}
	}

   	setSerializerStrictValidation(strictValidation);
	VALIDATION->setStrictValidation(strictValidation);
	VALIDATION->setIncludeType3NotPresentInResults(getIncludeType3NotPresentInResults());
	bool result1 = VALIDATION->validate(command_ptr, 
										refCommand_ptr, 
										lastCommand_ptr, 
										validationFlag, 
										serializerM_ptr);

	// validate the dataset - refDataset_ptr maybe NULL
	bool result2 = VALIDATION->validate(command_ptr,
										dataset_ptr, 
										refDataset_ptr,
										lastCommand_ptr,
										lastDataset_ptr, 
										validationFlag, 
										serializerM_ptr, 
										ae_session_ptr);

	// If any of the previous validations went wrong, the total validation has failed.
	bool result = true;
	if ((!result1) ||
		(!result2))
	{
		result = false;
	}

	// return result
	return result;
}

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(ASSOCIATE_AC_CLASS *associateAc_ptr, ASSOCIATE_AC_CLASS *refAssociateAc_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Associate Accept - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", WIDName(associateAc_ptr->getWidType()));
	}

    // Validate the Associate Accept - reference object is optional
    ACSE_PROPERTIES_CLASS	acseProperties;

    //
    // This validation is done on the Associate Accept that has been received.
    // We can therefore assume that DVT is the Requester and the SUT the Accepter.
    //
    acseProperties.setCallingAeTitle(this->dvtAeTitleM);
    acseProperties.setCalledAeTitle(this->sutAeTitleM);

    acseProperties.setMaximumLengthReceived(this->sutMaximumLengthReceivedM);
    acseProperties.setImplementationClassUid(this->sutImplementationClassUidM);
    acseProperties.setImplementationVersionName(this->sutImplementationVersionNameM);	

	// now validate the received associate accept
	bool result = VALIDATION->validate(associateAc_ptr, refAssociateAc_ptr, validationFlag, serializerM_ptr, &acseProperties);

    return result;
};

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(ASSOCIATE_RJ_CLASS *associateRj_ptr, ASSOCIATE_RJ_CLASS *refAssociateRj_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Associate Reject - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", WIDName(associateRj_ptr->getWidType()));
	}

    // Validate the Associate Reject - reference object is optional
	bool result = VALIDATION->validate(associateRj_ptr, refAssociateRj_ptr, validationFlag, serializerM_ptr);

    return result;
};

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(ASSOCIATE_RQ_CLASS *associateRq_ptr, ASSOCIATE_RQ_CLASS *refAssociateRq_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Associate Request - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", WIDName(associateRq_ptr->getWidType()));
	}

    // Validate the Associate Request - reference object is optional
	ACSE_PROPERTIES_CLASS	acseProperties;

    //
    // This validation is done on the Associate Request that has been received.
    // We can therefore assume that DVT is the Accepter and the SUT the Requester.
    //
    acseProperties.setCallingAeTitle(this->sutAeTitleM);
    acseProperties.setCalledAeTitle(this->dvtAeTitleM);

	acseProperties.setMaximumLengthReceived(this->sutMaximumLengthReceivedM);
	acseProperties.setImplementationClassUid(this->sutImplementationClassUidM);
	acseProperties.setImplementationVersionName(this->sutImplementationVersionNameM);

	// now validate the received associate request
	bool result = VALIDATION->validate(associateRq_ptr, refAssociateRq_ptr, validationFlag, serializerM_ptr, &acseProperties);

    return result;
};

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(RELEASE_RP_CLASS *releaseRp_ptr, RELEASE_RP_CLASS *refReleaseRp_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Release Response - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", WIDName(releaseRp_ptr->getWidType()));
	}

    // Validate the Release Response - reference object is optional
	bool result = VALIDATION->validate(releaseRp_ptr, refReleaseRp_ptr, validationFlag, serializerM_ptr);

    return result;
};

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(RELEASE_RQ_CLASS *releaseRq_ptr, RELEASE_RQ_CLASS *refReleaseRq_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Release Request - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", WIDName(releaseRq_ptr->getWidType()));
	}

    // Validate the Release Request - reference object is optional
	bool result = VALIDATION->validate(releaseRq_ptr, refReleaseRq_ptr, validationFlag, serializerM_ptr);

    return result;
};

//>>===========================================================================

bool SCRIPT_SESSION_CLASS::validate(ABORT_RQ_CLASS *abortRq_ptr, ABORT_RQ_CLASS *refAbortRq_ptr, VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validate the Abort Request - reference object is optional
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// log action
	if (loggerM_ptr)
	{
		loggerM_ptr->text(LOG_SCRIPT, 2, "VALIDATE %s", WIDName(abortRq_ptr->getWidType()));
	}

    // Validate the Abort Request - reference object is optional
   	bool result = VALIDATION->validate(abortRq_ptr, refAbortRq_ptr, validationFlag, serializerM_ptr);

    return result;
};

//>>===========================================================================

void SCRIPT_SESSION_CLASS::updateDefaults(ASSOCIATE_RQ_CLASS *associateRq_ptr)

//  DESCRIPTION     : Update any undefined parameters in the given associate request
//					: to the test session defaults.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    //
    // This update is done on the Associate Request that is being sent.
    // We can therefore assume that DVT is the Requester and the SUT the Accepter.
    //
	// set defaults
	// - calling ae title
	if (!associateRq_ptr->getCalledAeTitle())
	{
		associateRq_ptr->setCalledAeTitle((char*) getSutAeTitle());
	}

	// - calling ae title
	if (!associateRq_ptr->getCallingAeTitle())
	{
		associateRq_ptr->setCallingAeTitle((char*) getDvtAeTitle());
	}

	// - maximum length received
	if (associateRq_ptr->getMaximumLengthReceived() == UNDEFINED_MAXIMUM_LENGTH_RECEIVED)
	{
		associateRq_ptr->setMaximumLengthReceived(getDvtMaximumLengthReceived());
	}

	// - implementation class uid
	if (!associateRq_ptr->getImplementationClassUid())
	{
		associateRq_ptr->setImplementationClassUid((char*) getDvtImplementationClassUid());
	}

	// - implementation version name
	if ((!associateRq_ptr->getImplementationVersionName()) &&
		(getDvtImplementationVersionName()) &&
		(strlen(getDvtImplementationVersionName()) > 0))
	{
		associateRq_ptr->setImplementationVersionName((char*) getDvtImplementationVersionName());
	}

	// finally update any other default
	associateRq_ptr->updateDefaults();
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::updateDefaults(ASSOCIATE_AC_CLASS *associateAc_ptr)

//  DESCRIPTION     : Update any undefined parameters in the given associate accept
//					: to the test session defaults.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    //
    // This update is done on the Associate Accept that is being sent.
    // We can therefore assume that DVT is the Accepter and the SUT the Requester.
    //
	// set defaults
	// - calling ae title
	if (!associateAc_ptr->getCalledAeTitle())
	{
		associateAc_ptr->setCalledAeTitle((char*) getDvtAeTitle());
	}

	// - calling ae title
	if (!associateAc_ptr->getCallingAeTitle())
	{
		associateAc_ptr->setCallingAeTitle((char*) getSutAeTitle());
	}

	// - maximum length received
	if (associateAc_ptr->getMaximumLengthReceived() == UNDEFINED_MAXIMUM_LENGTH_RECEIVED)
	{
		associateAc_ptr->setMaximumLengthReceived(getDvtMaximumLengthReceived());
	}

	// - implementation class uid
	if (!associateAc_ptr->getImplementationClassUid())
	{
		associateAc_ptr->setImplementationClassUid((char*) getDvtImplementationClassUid());
	}

	// - implementation version name
	if ((!associateAc_ptr->getImplementationVersionName()) &&
		(getDvtImplementationVersionName()) &&
		(strlen(getDvtImplementationVersionName()) > 0))
	{
		associateAc_ptr->setImplementationVersionName((char*) getDvtImplementationVersionName());
	}

	// finally update any other default
	associateAc_ptr->updateDefaults();
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    BASE_SESSION_CLASS::setLogger(logger_ptr);
    associationM.setLogger(logger_ptr);
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setSerializer(BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Set the serializer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    BASE_SESSION_CLASS::setSerializer(serializer_ptr);
    associationM.setSerializer(serializer_ptr);
}

//>>===========================================================================

void SCRIPT_SESSION_CLASS::setConfirmer(BASE_CONFIRMER *confirmer_ptr)

//  DESCRIPTION     : Set up the confirmer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// save the activity reporter
	confirmerM_ptr = confirmer_ptr;
}

//>>===========================================================================

BASE_CONFIRMER *SCRIPT_SESSION_CLASS::getConfirmer()

//  DESCRIPTION     : Get the confirmer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return confirmerM_ptr;
}
