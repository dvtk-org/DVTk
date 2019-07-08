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

//  Base Test Session class.

#ifndef BASE_SESSION_H
#define BASE_SESSION_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "..\Dicom\private_attribute.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEFINITION_FILE_CLASS;
class DICOM_SCRIPT_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
//
// Session Types
//
enum SESSION_TYPE_ENUM
{
    ST_UNKNOWN,				// old session file - type not defined
    ST_SCRIPT,				// scripting session
    ST_EMULATOR,			// emulator session
    ST_MEDIA,				// media session
	ST_SNIFFER				// sniffer session
};

//
// Current Session File Version 
// An integer that is incremented by 1 each time the structure of one of session files changes
//
#define CURRENT_SESSION_FILE_VERSION	2

//
// DVT V1.x definition files that are no longer handled by the definition library
// and need to be filtered out during the session load
//
#define DVT_V1_CHARACTER_SET_DEFINITION_FILENAME            "CharacterSets.def"
#define DVT_V1_IMAGE_DISPLAY_FORMAT_DEFINITION_FILENAME     "ImageDisplayFormat.def"
//
// DVT V2.x loads the character set and image display format data files when a session is begun
//
#define DVT_V2_CHARACTER_SET_DATA_FILENAME            "CharacterSets.dat"
#define DVT_V2_IMAGE_DISPLAY_FORMAT_DATA_FILENAME     "ImageDisplayFormat.dat"
#define DVT_V2_DEFAULT_COMMANDSET_DEF_DATA_FILENAME   "AllDimseCommands.def"


//>>***************************************************************************

class ABSTRACT_MAP_CLASS

    //  DESCRIPTION     : Abstract Name Mapping Class.
    //  INVARIANT       :
    //  NOTES           :
    //<<***************************************************************************
{
private:
    string	mappedFromM;
    string	mappedToM;
    string  sopClassUidM;

public:
    ABSTRACT_MAP_CLASS();
    ABSTRACT_MAP_CLASS(string, string, string);

    ~ABSTRACT_MAP_CLASS();

    bool isMapped(const string);

    bool isSopClassUid(const string);

    string getMapping()
    {
        return mappedToM;
    }
};

//>>***************************************************************************

class BASE_SESSION_CLASS

    //  DESCRIPTION     : Test Session Class.
    //  INVARIANT       :
    //  NOTES           :
    //<<***************************************************************************
{
protected:
    string							sessionDirectoryM;
    SESSION_TYPE_ENUM				sessionTypeM; // flag set from session file content
    int								sessionFileVersionM;
    SESSION_TYPE_ENUM				runtimeSessionTypeM; // crude run-time session type check
    string							sessionTitleM; 
    bool							isOpenM;
    string							filenameM;
    int								counterM;
    bool							isSessionStoppedM;
	bool							isEmulationAbortedM;
	bool							isAssociatedM;

    // Product Test Session Properties
    int								sessionIdM;
    string							manufacturerM;
    string							modelNameM;
    string							softwareVersionsM;
    string							applicationEntityNameM;
    string							applicationEntityVersionM;
    string							testedByM;
    string							dateM;

    // Test Properties
    bool							strictValidationM;
	bool							detailedValidationResultsM;
	bool							summaryValidationResultsM;
	bool							testLogValidationResultsM;
	bool							includeType3NotPresentInResultsM;
	bool							dumpAttributesOfRefFilesM;
	bool							unVrDefinitionLookUpM;
	bool							ensureEvenAttributeValueLengthM;

    STORAGE_MODE_ENUM				storageModeM;

    // Definitions
    vector<string>                  definitionDirectoryM;
    ARRAY<DEFINITION_FILE_CLASS*>	definitionFileM_ptr;

    // Abstract Mappings
    ARRAY<ABSTRACT_MAP_CLASS>		abstractMapM;

	// Data Directory
	string							dataDirectoryM;
	bool							isDataDirectorySetInSessionM;

    // Results
    string							resultsRootM;
    bool							appendToResultsFileM;

    // Test Script Descriptions
    string                          descriptionDirectoryM;
    string                          systemAppDomainBaseDirectoryM;

    LOG_CLASS						*loggerM_ptr;
    BASE_ACTIVITY_REPORTER          *activityReporterM_ptr;
    BASE_SERIALIZER                 *serializerM_ptr;
    UINT32							logMaskM;

    int								instanceIdM;

    void checkAbstractMappings();

    bool isPathAbsolute(char*);

    void installCharacterSetAndImageDisplayFormatAndDefaultCommandSetDefData();

protected:
    BASE_SESSION_CLASS();

public:
    // pure virtual functions
    virtual ~BASE_SESSION_CLASS() = 0;

    virtual bool serialise(FILE*) = 0;

    // virtual functions
    // - most of the functions (empty ones) are provided for backwards compatibility
    // when parsing older session files where the distinction between session types was 
    // not made
    virtual void cleanup();

    virtual void sessionFileVersion(int);

    //
    // deprecated methods
    //
    virtual void setProductRoleIsAcceptor(bool) { }
    virtual bool getProductRoleIsAcceptor() { return false; }

    virtual void setProductRoleIsRequestor(bool) { }
    virtual bool getProductRoleIsRequestor() { return false; }

    virtual void setCalledAeTitle(char*) { }
    virtual const char *getCalledAeTitle() { return NULL; }

    virtual void setCallingAeTitle(char*) { }
    virtual const char *getCallingAeTitle() { return NULL; }

    virtual void setMaximumLengthReceived(int) { }
    virtual UINT32 getMaximumLengthReceived() { return 0; }

    virtual void setImplementationClassUid(char*) { }
    virtual const char *getImplementationClassUid() { return NULL; }

    virtual void setImplementationVersionName(char*) { }
    virtual const char *getImplementationVersionName() { return NULL; }

    virtual void setRemoteHostname(char*) { }
    virtual const char *getRemoteHostname() { return NULL; }

    virtual void setRemoteConnectPort(UINT16) { }
    virtual int getRemoteConnectPort() { return 0; }

    virtual void setLocalListenPort(UINT16) { }
    virtual int getLocalListenPort() { return 0; }

    virtual void setSocketTimeout(int) { }
    virtual int getSocketTimeout() { return 0; }
    //
    // end of deprecated methods
    //


    // DVT session properties
    virtual void setDvtAeTitle(char*) { }
    virtual const char *getDvtAeTitle() { return NULL; }

    virtual void setDvtMaximumLengthReceived(int) { }
    virtual UINT32 getDvtMaximumLengthReceived() { return 0; }

    virtual void setDvtImplementationClassUid(char*) { }
    virtual const char *getDvtImplementationClassUid() { return NULL; }

    virtual void setDvtImplementationVersionName(char*) { }
    virtual const char *getDvtImplementationVersionName() { return NULL; }

    virtual void setDvtPort(UINT16) { }
    virtual UINT16 getDvtPort() { return 0; }

    virtual void setDvtSocketTimeout(int) { }
    virtual int getDvtSocketTimeout() { return 0; }

    // SUT session properties
    virtual void setSutAeTitle(char*) { }
    virtual const char *getSutAeTitle() { return NULL; }

    virtual void setSutMaximumLengthReceived(int) { }
    virtual UINT32 getSutMaximumLengthReceived() { return 0; }

    virtual void setSutImplementationClassUid(char*) { }
    virtual const char *getSutImplementationClassUid() { return NULL; }

    virtual void setSutImplementationVersionName(char*) { }
    virtual const char *getSutImplementationVersionName() { return NULL; }

    virtual void setSutHostname(char*) { }
    virtual const char *getSutHostname() { return NULL; }

    virtual void setSutPort(UINT16) { }
    virtual UINT16 getSutPort() { return 0; }

    virtual void setSutRole(UP_ENUM) { }
    virtual UP_ENUM getSutRole() { return UP_ACCEPTOR; }


    virtual void setUseSecureSockets(bool) { }

    virtual void setTlsPassword(const char*) { }

    virtual bool isTlsPasswordValid(bool& unencryptedKeyFound)
    { 
        unencryptedKeyFound = false;
        return true; 
    }

    virtual void setTlsVersion(const char*) { }

    virtual void setCheckRemoteCertificate(bool) { }

    virtual void setCipherList(const char*) { }

    virtual void setCacheTlsSessions(bool) { }

    virtual void setTlsCacheTimeout(int) { }

	virtual void setDelayForStorageCommitment(int) { }

	virtual void setAcceptDuplicateImage(bool) { }

	virtual void setStoreCStoreReqOnly(bool) { }

    virtual void setCredentialsFilename(const char*) { }

    virtual void setCertificateFilename(const char*) { }

    virtual void setSocketParametersChanged(bool) { }

    virtual bool getUseSecureSockets() { return false; }

    virtual const char *getTlsPassword() { return NULL; }

    virtual const char *getTlsVersion() { return NULL; }

    virtual bool getCheckRemoteCertificate() { return false; }

    virtual const char *getCipherList() { return NULL; }

    virtual bool getCacheTlsSessions() { return false; }

    virtual int getTlsCacheTimeout() { return 0; }

	virtual int getDelayForStorageCommitment() {return 0; }

	virtual bool getSingleCommitReport() {return false; }

    virtual string getCredentialsFilename() { return NULL; }

    virtual string getCertificateFilename() { return NULL; }

    virtual void setApplicationEntityName(char *applicationEntityName_ptr)
    {
        applicationEntityNameM = applicationEntityName_ptr;
    }

    virtual void setApplicationEntityVersion(char *applicationEntityVersion_ptr)
    {
        applicationEntityVersionM = applicationEntityVersion_ptr;
    }

    virtual void setLogScpThread(bool) { } 

    virtual void setStrictValidation(bool);

	void setSerializerStrictValidation(bool);

    virtual void setAutoType2Attributes(bool) { }

    virtual void setAutoCreateDirectory(bool) { }

    virtual void setDefineSqLength(bool) { }

    virtual void setAddGroupLength(bool) { }

    virtual void setContinueOnError(bool) { }

    virtual void deleteSupportedTransferSyntaxes() { }

    virtual void addSupportedTransferSyntax(char*) { }

    virtual bool getLogScpThread() { return false; }

    virtual bool getStrictValidation() { return false; }

    virtual bool getAutoType2Attributes() { return false; }

    virtual bool getAutoCreateDirectory() { return false; }

    virtual bool getDefineSqLength() { return false; }

    virtual bool getAddGroupLength() { return false; }

    virtual bool getContinueOnError() { return false; }

    virtual int noSupportedTransferSyntaxes() { return 0; }

    virtual const char *getSupportedTransferSyntax(int) { return NULL; }

    virtual void setDicomScriptRoot(char*) { }

    virtual const char *getDicomScriptRoot() { return NULL; }

    virtual void addDicomScript(DICOM_SCRIPT_CLASS*) { }

	virtual void setValidateReferencedFile(bool) { }

	virtual bool getValidateReferencedFile() { return false; }

    virtual bool begin(bool&);

    virtual void end();

    virtual bool executeScript(string) { return false; }

    virtual bool parseScript(string) { return false; }

    virtual void setScpEmulatorType(SCP_EMULATOR_ENUM) { }

	virtual SCP_EMULATOR_ENUM getScpEmulatorType() { return SCP_EMULATOR_UNKNOWN; }

    virtual void setScuEmulatorType(SCU_EMULATOR_ENUM) { }

	virtual SCU_EMULATOR_ENUM getScuEmulatorType() { return SCU_EMULATOR_UNKNOWN; }

    virtual bool emulateSCP() { return false; }

    virtual bool sendStatusEvent() { return false; }

    virtual bool terminateConnection() { return false; }

	virtual bool abortEmulation() { return false; }

	virtual bool abortEmulationFromSCU() { return false; }

    virtual void resetAssociation() { }

    virtual void setScriptDoneCallBack(void (*)(void*), void*) { }

    virtual void resetScriptDoneCallBack(void) { }

    virtual bool beginMediaValidation() { return false; }

    virtual bool validateMediaFile(string) { return false; }

	virtual bool validateMediaFile(string, MEDIA_FILE_CONTENT_TYPE_ENUM, string, string, string) { return false; }

    virtual bool validateMediaFile(string, string, int) { return false; }

    virtual bool endMediaValidation() { return false; }

    virtual void setLogger(LOG_CLASS*);

    virtual void setActivityReporter(BASE_ACTIVITY_REPORTER*);

    virtual void setSerializer(BASE_SERIALIZER*);

    // base functions
    void setInstanceId(DWORD instanceId)
    {
        instanceIdM = instanceId;
    }

    int getInstanceId()
    {
        return instanceIdM;
    }

    void setSessionDirectory(char *sessionDirectory_ptr)
    {
        sessionDirectoryM = sessionDirectory_ptr;
    }

    char *getSessionDirectory()
    {
        return (char*) sessionDirectoryM.c_str();
    }

    void setAppendToResultsFile(bool flag)
    {
        appendToResultsFileM = flag;
    }

    bool getAppendToResultsFile()
    {
        return appendToResultsFileM;
    }

    void resetCounter()
    {
        counterM = 0;
    }

    void incrementCounter()
    {
        counterM++;
    }

    int getCounter()
    {
        return counterM;
    }

    bool reloadDefinitions();

    void removeDefinitions();

    void makeRootAbsolute(string&, char*); 

    void setSessionType(SESSION_TYPE_ENUM sessionType)
    {
        sessionTypeM = sessionType;
    }

    void setSessionTitle(char *sessionTitle_ptr)
    {
        sessionTitleM = sessionTitle_ptr;
    }

    void setSessionId(int sessionId)
    {
        sessionIdM = sessionId;
    }

    void setManufacturer(char *manufacturer_ptr)
    {
        manufacturerM = manufacturer_ptr;
    }

    void setModelName(char *modelName_ptr)
    {
        modelNameM = modelName_ptr;
    }

    void setSoftwareVersions(char *softwareVersions_ptr)
    {
        softwareVersionsM = softwareVersions_ptr;
    }

    void setTestedBy(char *testedBy_ptr)
    {
        testedByM = testedBy_ptr;
    }

    void setDate(char *date_ptr)
    {
        dateM = date_ptr;
    }

	void setDetailedValidationResults(bool flag)
	{
		detailedValidationResultsM = flag;
	}
	
	void setTestLogValidationResults(bool flag)
	{
		testLogValidationResultsM = flag;
	}
	
	void setSummaryValidationResults(bool flag)
	{
		summaryValidationResultsM = flag;
	}

	static void setUsePrivateAttributeMapping(bool flag)
	{
		PRIVATE_ATTRIBUTE_HANDLER_CLASS::usePrivateAttributeMapping = flag;
	}

	void setIncludeType3NotPresentInResults(bool flag)
	{
		includeType3NotPresentInResultsM = flag;
	}

	void setDumpAttributesOfRefFiles(bool flag)
	{
		dumpAttributesOfRefFilesM = flag;
	}

	void setUnVrDefinitionLookUp(bool flag)
	{
		unVrDefinitionLookUpM = flag;
	}

	void setEnsureEvenAttributeValueLength(bool flag)
	{
		ensureEvenAttributeValueLengthM = flag;
	}

	SESSION_TYPE_ENUM getSessionType()
    {
        return sessionTypeM;
    }

    SESSION_TYPE_ENUM getRuntimeSessionType()
    {
        return runtimeSessionTypeM;
    }

    const char *getSessionTitle()
    {
        return sessionTitleM.c_str();
    }

    int getSessionId()
    {
        return sessionIdM;
    }

    const char *getManufacturer()
    {
        return manufacturerM.c_str();
    }

    const char *getModelName()
    {
        return modelNameM.c_str();
    }

    const char *getSoftwareVersions()
    {
        return softwareVersionsM.c_str();
    }

    const char *getApplicationEntityName()
    {
        return applicationEntityNameM.c_str();
    }

    const char *getApplicationEntityVersion()
    {
        return applicationEntityVersionM.c_str();
    }

    const char *getTestedBy()
    {
        return testedByM.c_str();
    }

    const char *getDate()
    {
        return dateM.c_str();
    }
	
	bool getDetailedValidationResults()
	{
		return detailedValidationResultsM;
	}
	
	bool getTestLogValidationResults()
	{
		return testLogValidationResultsM;
	}

	bool getSummaryValidationResults()
	{
		return summaryValidationResultsM;
	}
	
	static bool getUsePrivateAttributeMapping()
	{
		return PRIVATE_ATTRIBUTE_HANDLER_CLASS::usePrivateAttributeMapping;
	}

	bool getIncludeType3NotPresentInResults()
	{
		return includeType3NotPresentInResultsM;
	}

	bool getDumpAttributesOfRefFiles()
	{
		return dumpAttributesOfRefFilesM;
	}

	bool getUnVrDefinitionLookUp()
	{
		return unVrDefinitionLookUpM;
	}

	bool getEnsureEvenAttributeValueLength()
	{
		return ensureEvenAttributeValueLengthM;
	}

	string getFilename (void);

    void setFileName (string fileName);

    virtual void setStorageMode(STORAGE_MODE_ENUM storageMode)
    {
        storageModeM = storageMode;
    }

    STORAGE_MODE_ENUM getStorageMode()
    {
        return storageModeM;
    }

    UINT32 getLogMask() { return logMaskM; }

	void addDefinitionDirectory(string);

    UINT noDefinitionDirectories();

    string getDefinitionDirectory(UINT);

    void removeAllDefinitionDirectories();

    void setDefinitionFileRoot(char*);

    const char *getDefinitionFileRoot();

    void addDefinitionFile(DEFINITION_FILE_CLASS *definitionFile_ptr)
    {

        definitionFileM_ptr.add(definitionFile_ptr);
    }

    UINT noDefinitionFiles()
    {
        return definitionFileM_ptr.getSize();
    }

    DEFINITION_FILE_CLASS *getDefinitionFile(UINT);

    const char *getDefinitionFilename(UINT);

	void setDataDirectory(char*);

	const char *getDataDirectory()
	{
		if(isDataDirectorySetInSessionM)
		{
			return dataDirectoryM.c_str();
		}
		else
		{
			return resultsRootM.c_str();
		}
	}

    void setResultsRoot(char*);

    const char *getResultsRoot()
    {
        return resultsRootM.c_str();
    }

    void setDescriptionDirectory(string);

    string getDescriptionDirectory();

    void setSystemAppDomainBaseDirectory(string);

    string getSystemAppDomainBaseDirectory();

    string getAbsolutePixelPathname(char*); 

    bool isOpen()
    {
        return isOpenM;
    }

    bool load(string, bool&, bool andBeginIt = true);

    bool save();
    bool save(string);

	FILE_DATASET_CLASS* readMedia(string, MEDIA_FILE_CONTENT_TYPE_ENUM, string,	string,	string, bool, bool);
    bool writeMedia(FILE_DATASET_CLASS*);

	FILE_DATASET_CLASS* readDicomdir(string, bool, bool);
    bool writeDicomdir(FILE_DATASET_CLASS*);

    bool loadDefinition(string);
    bool unloadDefinition(string);

    string getSopUid(const string&);

    string getIodName(const string&);

	string getIodNameFromDefinition(DIMSE_CMD_ENUM cmd, const string uid);

	string getFileNameFromSOPUID(DIMSE_CMD_ENUM cmd, const string uid);

    LOG_CLASS *getLogger()
    {
        return loggerM_ptr;
    }

    BASE_SERIALIZER* getSerializer()
    {
        return serializerM_ptr;
    }

    void enableLogger()
    {
        if (loggerM_ptr)
        {
            // check if session logMask is defined
            if (logMaskM)
            {
                // restore session log mask
                loggerM_ptr->setLogMask(logMaskM);
            }
        }
    }

    void disableLogger()
    {
        if (loggerM_ptr)
        {
            // get the current log mask
            UINT logMask = loggerM_ptr->getLogMask();
            if (logMask)
            {
                // if it is defined - save it
                logMaskM = logMask;

                // disable further logging
                loggerM_ptr->setLogMask(0);
            }
        }
    }

    void setLogLevel(bool, UINT32);

    bool isLogLevel(UINT32);

    bool isSessionStopped()
    {
        return isSessionStoppedM;
    }

	bool isEmulationAborted()
    {
        return isEmulationAbortedM;
    }

	void setIsAborted(bool aborted)
    {
		isEmulationAbortedM = aborted;
	}

	bool getIsAssociated()
    {
		return isAssociatedM;
	}

	void setIsAssociated(bool associated)
    {
		isAssociatedM = associated;
	}
};

//>>***************************************************************************

class ABSTRACT_SESSION_CLASS : public BASE_SESSION_CLASS

    //  DESCRIPTION     : Abstract Test Session Class - used purely to determine the
    //					  actual session file type.
    //  INVARIANT       :
    //  NOTES           :
    //<<***************************************************************************
{
public:
    ABSTRACT_SESSION_CLASS();

    ~ABSTRACT_SESSION_CLASS();

    bool serialise(FILE*) { return false; }
};

#endif /* BASE_SESSION_H */
