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
#ifndef SCRIPT_SESSION_H
#define SCRIPT_SESSION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"			// Global component interface
#include "session.h"			// Base Session include
#include "IAttributeGroup.h"	// Attribute Group component interface
#include "Inetwork.h"			// Network component interface
#include "Irelationship.h"		// Relationship component interface

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class SCRIPT_EXECUTION_CONTEXT_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;
class DCM_COMMAND_CLASS;
class DCM_DATASET_CLASS;


//>>***************************************************************************

class SCRIPT_SESSION_CLASS : public BASE_SESSION_CLASS

    //  DESCRIPTION     : Script Test Session Class.
    //  INVARIANT       :
    //  NOTES           :
    //<<***************************************************************************
{
private:
    // DVT ACSE Properties
    string							dvtAeTitleM;
    UINT32							dvtMaximumLengthReceivedM;
    string							dvtImplementationClassUidM;
    string							dvtImplementationVersionNameM;

    // SUT ACSE Properties
    string							sutAeTitleM;
    UINT32							sutMaximumLengthReceivedM;
    string							sutImplementationClassUidM;
    string							sutImplementationVersionNameM;
    UP_ENUM                         sutRoleM;

    // Test Properties
    bool							autoType2AttributesM;
    bool							autoCreateDirectoryM;
    bool							defineSqLengthM;
    bool							addGroupLengthM;
    bool							continueOnErrorM;

    // Test Session Properties - via Association and Socket Parameters
    ASSOCIATION_CLASS				associationM;
    SOCKET_PARAMETERS				socketParametersM;

    // DICOMScripts
    string							dicomScriptRootM;
    ARRAY<DICOM_SCRIPT_CLASS*>		dicomScriptM_ptr;

	// Script Execution Context
	SCRIPT_EXECUTION_CONTEXT_CLASS	*scriptExecutionContextM_ptr;


	// last Command / Dataset Sent
	// - this maybe used during the validation of the next received dataset
	// - example: if DVT sends a C-FIND-RQ then this C-FIND-RQ should be used when
	// validating the returned C-FIND-RSP
	DCM_COMMAND_CLASS				*lastCommandSentM_ptr;
	DCM_DATASET_CLASS				*lastDatasetSentM_ptr;

	ARRAY<PRESENTATION_CONTEXT_AC_CLASS>	accPresentationContextM;

    // Script done callback & context
    void (*scriptDoneCallBackFunctionM_ptr)(void*);
    void *scriptDoneCallBackContextM_ptr;

    void cleanup();

    bool updateLabelledValues(DCM_ATTRIBUTE_GROUP_CLASS*, DCM_ATTRIBUTE_GROUP_CLASS*);

    void scriptDone(void);

    BASE_CONFIRMER                  *confirmerM_ptr;
public:
    SCRIPT_SESSION_CLASS();

    ~SCRIPT_SESSION_CLASS();

    bool serialise(FILE*);

    //
    // deprecated methods
    //
    void setProductRoleIsAcceptor(bool productRoleIsAcceptor);
    bool getProductRoleIsAcceptor();

    void setProductRoleIsRequestor(bool productRoleIsRequestor);
    bool getProductRoleIsRequestor();

    void setCalledAeTitle(char *calledAeTitle_ptr);
    const char *getCalledAeTitle();

    void setCallingAeTitle(char *callingAeTitle_ptr);
    const char *getCallingAeTitle();

    void setMaximumLengthReceived(int maximumLengthReceived);
    UINT32 getMaximumLengthReceived();

    void setImplementationClassUid(char *implementationClassUid_ptr);
    const char *getImplementationClassUid();

    void setImplementationVersionName(char *implementationVersionName_ptr);
    const char *getImplementationVersionName();

    void setRemoteHostname(char *remoteHostname_ptr);
    const char *getRemoteHostname();

    void setRemoteConnectPort(UINT16 remoteConnectPort);
    int getRemoteConnectPort();

    void setLocalListenPort(UINT16 localListenPort);
    int getLocalListenPort();

    void setSocketTimeout(int socketTimeout);
    int getSocketTimeout();
    //
    // end of deprecated methods
    //

    // DVT session properties
    void setDvtAeTitle(char *dvtAeTitle_ptr);
    const char *getDvtAeTitle();

    void setDvtMaximumLengthReceived(int maximumLengthReceived);
    UINT32 getDvtMaximumLengthReceived();

    void setDvtImplementationClassUid(char *implementationClassUid_ptr);
    const char *getDvtImplementationClassUid();

    void setDvtImplementationVersionName(char *implementationVersionName_ptr);
    const char *getDvtImplementationVersionName();

    void setDvtPort(UINT16 port);
    UINT16 getDvtPort();

    void setDvtSocketTimeout(int socketTimeout);
    int getDvtSocketTimeout();

    // SUT session properties
    void setSutAeTitle(char *sutAeTitle_ptr);
    const char *getSutAeTitle();

    void setSutMaximumLengthReceived(int maximumLengthReceived);
    UINT32 getSutMaximumLengthReceived();

    void setSutImplementationClassUid(char *implementationClassUid_ptr);
    const char *getSutImplementationClassUid();

    void setSutImplementationVersionName(char *implementationVersionName_ptr);
    const char *getSutImplementationVersionName();

    void setSutHostname(char *hostname_ptr);
    const char *getSutHostname();

    void setSutPort(UINT16 port);
    UINT16 getSutPort();

    void setSutRole(UP_ENUM role);
    UP_ENUM getSutRole();

    SOCKET_PARAMETERS& getSocketParameters()
    { return socketParametersM; }

    // security properties
    void setUseSecureSockets(bool useSecureSockets)
    {
        socketParametersM.useSecureSocketsM = useSecureSockets;
    }

    void setTlsPassword(const char* password)
    {
        socketParametersM.certificateFilePasswordM = password;
    }

    bool isTlsPasswordValid(bool& unencryptedKeyFound)
    {
        return socketParametersM.isTlsPasswordValid(unencryptedKeyFound);
    }

    void setMaxTlsVersion(const char* maxTlsVersion)
    {
        socketParametersM.maxTlsVersionM = maxTlsVersion;
    }

    void setMinTlsVersion(const char* minTlsVersion)
    {
        socketParametersM.minTlsVersionM = minTlsVersion;
    }

    void setCheckRemoteCertificate(bool checkRemoteCertificate)
    {
        socketParametersM.checkRemoteCertificateM = checkRemoteCertificate;
    }

    void setCipherList(const char* cipherList)
    {
        socketParametersM.cipherListM = cipherList;
    }

    void setCacheTlsSessions(bool cacheTlsSessions)
    {
        socketParametersM.cacheTlsSessionsM = cacheTlsSessions;
    }

    void setTlsCacheTimeout(int tlsCacheTimeout)
    {
        socketParametersM.tlsCacheTimeoutM = tlsCacheTimeout;
    }

    void setCredentialsFilename(const char* credentialsFilename)
    {
        string filename = credentialsFilename;
        socketParametersM.credentialsFilenameM = generateFullPath(filename, sessionDirectoryM); 
    }

    void setCertificateFilename(const char* certificateFilename)
    {
        string filename = certificateFilename;
        socketParametersM.certificateFilenameM = generateFullPath(filename, sessionDirectoryM); 
    }

    void setSocketParametersChanged(bool changed)
    {
        socketParametersM.contentsChangedM = changed;
    }

    bool getUseSecureSockets()
    {
        return socketParametersM.useSecureSocketsM;
    }

    const char *getTlsPassword()
    {
        return socketParametersM.certificateFilePasswordM.c_str();
    }

    const char* getMaxTlsVersion()
    {
        return socketParametersM.maxTlsVersionM.c_str();
    }

    const char* getMinTlsVersion()
    {
        return socketParametersM.minTlsVersionM.c_str();
    }

    bool getCheckRemoteCertificate()
    {
        return socketParametersM.checkRemoteCertificateM;
    }

    const char *getCipherList()
    {
        return socketParametersM.cipherListM.c_str();
    }

    bool getCacheTlsSessions()
    {
        return socketParametersM.cacheTlsSessionsM;
    }

    int getTlsCacheTimeout()
    {
        return socketParametersM.tlsCacheTimeoutM;
    }

    string getCredentialsFilename()
    {
        return generateFullPath(socketParametersM.credentialsFilenameM, sessionDirectoryM);
    }

    string getCertificateFilename()
    {
        return generateFullPath(socketParametersM.certificateFilenameM, sessionDirectoryM);
    }

    void setApplicationEntityName(char*);

    void setApplicationEntityVersion(char*);

    void setStorageMode(STORAGE_MODE_ENUM);
		
	void setStrictValidation(bool);

	void setAutoType2Attributes(bool);

	void setAutoCreateDirectory(bool);

	void setDefineSqLength(bool);

	void setAddGroupLength(bool);

	void setContinueOnError(bool);

    STORAGE_MODE_ENUM getStorageMode()
    {
        return storageModeM;
    }

    bool getStrictValidation()
    {
        return strictValidationM;
    }

    bool getAutoType2Attributes()
    {
        return autoType2AttributesM;
    }

    bool getAutoCreateDirectory()
    {
        return autoCreateDirectoryM;
    }

    bool getDefineSqLength()
    {
        return defineSqLengthM;
    }

    bool getAddGroupLength()
    {
        return addGroupLengthM;
    }

    bool getContinueOnError()
    {
        return continueOnErrorM;
    }

    void setDicomScriptRoot(char*);

	bool getHasPendingDataInNetworkInputBuffer();
    
    const char *getDicomScriptRoot()
    {
        return dicomScriptRootM.c_str();
    }

    void addDicomScript(DICOM_SCRIPT_CLASS *dicomScript_ptr)
    {
        dicomScriptM_ptr.add(dicomScript_ptr);
    }

    UINT noDicomScripts()
    {
        return dicomScriptM_ptr.getSize();
    }

    DICOM_SCRIPT_CLASS *getDicomScript(UINT i)
    {
        DICOM_SCRIPT_CLASS *dicomScript_ptr = NULL;
        if (i < dicomScriptM_ptr.getSize())
        {
            dicomScript_ptr = dicomScriptM_ptr[i]; 
        }
        return dicomScript_ptr;
    }

	SCRIPT_EXECUTION_CONTEXT_CLASS	*getScriptExecutionContext();
	
	void resetScriptExecutionContext();

    bool begin(bool& definitionFileLoaded);

    void end();

    bool executeScript(string);

    bool parseScript(string);

    void setScriptDoneCallBack(void (*)(void*), void*);

    void resetScriptDoneCallBack(void);

    bool terminateConnection();

    void resetAssociation();

    bool connectOnTcpIp();

    bool listenOnTcpIp();

    bool send(ASSOCIATE_RQ_CLASS*, string);
    bool send(ASSOCIATE_AC_CLASS*, string);
    bool send(ASSOCIATE_RJ_CLASS*, string);
    bool send(RELEASE_RQ_CLASS*, string);
    bool send(RELEASE_RP_CLASS*, string);
    bool send(ABORT_RQ_CLASS*, string);
    bool send(UNKNOWN_PDU_CLASS*, string);
    bool send(DCM_COMMAND_CLASS*);
    bool send(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
	bool send(DCM_COMMAND_CLASS*,int);
    bool send(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*,int);

    bool receive(ASSOCIATE_RQ_CLASS*, string);
    bool receive(ASSOCIATE_AC_CLASS*, string);
    bool receive(ASSOCIATE_RJ_CLASS*, string);
    bool receive(RELEASE_RQ_CLASS*, string);
    bool receive(RELEASE_RP_CLASS*, string);
    bool receive(ABORT_RQ_CLASS*, string);
    bool receive(UNKNOWN_PDU_CLASS*, string);
    bool receive(DCM_COMMAND_CLASS**, DCM_COMMAND_CLASS*);
    bool receive(DCM_COMMAND_CLASS**, DCM_DATASET_CLASS**, DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
	RECEIVE_MSG_ENUM receive(RECEIVE_MESSAGE_UNION_CLASS**);

    bool importCommand(DIMSE_CMD_ENUM, string);
    bool importCommandDataset(DIMSE_CMD_ENUM, string, string, string);
	bool comparePixelData(DCM_DATASET_CLASS*, DCM_DATASET_CLASS*);

    bool validate(DCM_COMMAND_CLASS*, DCM_COMMAND_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
	bool validate(DCM_COMMAND_CLASS*, DCM_COMMAND_CLASS*, DCM_COMMAND_CLASS*,VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*, DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*, VALIDATION_CONTROL_FLAG_ENUM, AE_SESSION_CLASS*);
	bool validate(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*, DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*, DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*,VALIDATION_CONTROL_FLAG_ENUM, AE_SESSION_CLASS*);
    bool validate(ASSOCIATE_AC_CLASS*, ASSOCIATE_AC_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(ASSOCIATE_RJ_CLASS*, ASSOCIATE_RJ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(ASSOCIATE_RQ_CLASS*, ASSOCIATE_RQ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(RELEASE_RP_CLASS*, RELEASE_RP_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(RELEASE_RQ_CLASS*, RELEASE_RQ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);
    bool validate(ABORT_RQ_CLASS*, ABORT_RQ_CLASS*, VALIDATION_CONTROL_FLAG_ENUM);

    void updateDefaults(ASSOCIATE_RQ_CLASS*);
    void updateDefaults(ASSOCIATE_AC_CLASS*);

    void logRelationship();

    void setLogger(LOG_CLASS*);

    void setSerializer(BASE_SERIALIZER*);
    void setConfirmer(BASE_CONFIRMER *);
    BASE_CONFIRMER *getConfirmer();
};

#endif /* SCRIPT_SESSION_H */