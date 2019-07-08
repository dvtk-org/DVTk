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

#include "StdAfx.h"
#include ".\mbasesession.h"
#include "MDIMSEConvertors.h"
#include "MMediaConvertors.h"
#include < vcclr.h >
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    void MBaseSession::_StaticConstructor()
    {
        MBaseSession::RuleUriStandardRules = new System::Uri("urn:rules-dvtk:standard");
        MBaseSession::RuleUriStrictRules = new System::Uri("urn:rules-dvtk:strict");
    }

    MBaseSession::MBaseSession(void)
    {
        m_pActivityReportingAdapter = NULL;
        m_pSerializationAdapter = NULL;
        m_pCountingAdapter = NULL;
    }

    void MBaseSession::Initialize(void)
    {
        System::String __gc* pSystemAppDomainBaseDirectory =
            System::AppDomain::CurrentDomain->BaseDirectory;
        char* pAnsiString = 
            (char*)(void*)Marshal::StringToHGlobalAnsi(pSystemAppDomainBaseDirectory);
        m_pBASE_SESSION->setSystemAppDomainBaseDirectory(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }

    MBaseSession::~MBaseSession(void)
    {
        if (m_pActivityReportingAdapter != NULL) delete m_pActivityReportingAdapter;
        if (m_pSerializationAdapter != NULL) delete m_pSerializationAdapter;
        if (m_pCountingAdapter != NULL) delete m_pCountingAdapter;
    }

    void MBaseSession::set_SessionFileVersion(System::UInt16 value)
    {
        return m_pBASE_SESSION->sessionFileVersion(value);
    }

    void MBaseSession::InitTopSerializationAndCountingAndActivityReportingTargets(
			ISerializationTarget __gc* serializationTarget,
			ICountingTarget __gc* countingTarget,
			IActivityReportingTarget __gc* activityReportingTarget)
    {
		//
		// Preconditions: Method arguments.
		//
        if (serializationTarget == NULL) throw new System::ArgumentNullException();
        if (countingTarget == NULL) throw new System::ArgumentNullException();
        if (activityReportingTarget == NULL) throw new System::ArgumentNullException();
		//
		// Clean-up existing object-references.
		//
		if (this->m_pCountingAdapter != NULL) delete this->m_pCountingAdapter;
		if (this->m_pSerializationAdapter != NULL) delete this->m_pSerializationAdapter;
		if (this->m_pActivityReportingAdapter != NULL) delete this->m_pActivityReportingAdapter;
		//
		// Create new objects.
		//
        this->m_pCountingAdapter = new CountingAdapter(countingTarget);
		System::Uri __gc* pRules = Wrappers::MBaseSession::RuleUriStrictRules;
        this->m_pSerializationAdapter = new SerializationAdapter(serializationTarget, countingTarget, pRules);
        this->m_pBASE_SESSION->setSerializer(this->m_pSerializationAdapter);
		this->m_pActivityReportingAdapter = new ActivityReportingAdapter(activityReportingTarget, countingTarget);
        this->m_pBASE_SESSION->setActivityReporter(this->m_pActivityReportingAdapter);
		//
		// User activity reporting will only be counted in the top level countmanager.
		//
        this->set_Rules(MBaseSession::RuleUriStandardRules);
    }

	//
	// Stop processing from lower unmanaged layer towards upper managed layer.
	// Adapters in the middle layer between the lower and upper layers are instructed
	// to end the data processing.
	//
	// Counting results are kept active on the targets and adapters untill these are deleted
	// and recreated by the next results gathering run.
	//
	// Upper Managed Layer (targets)
	// /\
	// ||Serialization data
	// ||
	// Middle Adapter Layer (m_pSerializationAdapter, m_pCountingAdapter, m_pActivityReportingAdapter)
	// /\
	// ||Serialization data
	// ||
	// Lower Unmanaged Layer
	//
	void MBaseSession::StopSerializationProcessing()
	{
		if (m_pSerializationAdapter != NULL) 
		{
			m_pSerializationAdapter->EndSerializer();
		}
		if (m_pCountingAdapter != NULL)
		{
			// TODO!?
		}
		if (m_pActivityReportingAdapter != NULL) 
		{
			// TODO!?
		}
	}

    Wrappers::SutRole MBaseSession::get_SutRole()
    {
        Wrappers::SutRole sutRole;
        switch (m_pBASE_SESSION->getSutRole())
        {
        case UP_ACCEPTOR_REQUESTOR:
            sutRole = Wrappers::SutRoleAcceptorRequestor;
            break;
        case UP_ACCEPTOR:
            sutRole = Wrappers::SutRoleAcceptor;
            break;
        case UP_REQUESTOR:
            sutRole = Wrappers::SutRoleRequestor;
            break;
        default:
            // Unknown Wrappers::SutRole
            throw new System::NotImplementedException();
        }

        return sutRole;
    }

    void MBaseSession::set_SutRole(Wrappers::SutRole value)
    {
        UP_ENUM sutRole;
        switch (value)
        {
        case Wrappers::SutRoleAcceptorRequestor:
            sutRole = UP_ACCEPTOR_REQUESTOR;
            break;
        case Wrappers::SutRoleAcceptor:
            sutRole = UP_ACCEPTOR;
            break;
        case Wrappers::SutRoleRequestor:
            sutRole = UP_REQUESTOR;
            break;
        default:
            // Unknown Wrappers::SutRole
            throw new System::NotImplementedException();
        }

        m_pBASE_SESSION->setSutRole(sutRole);
    }

    System::String __gc* MBaseSession::get_DvtAeTitle()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDvtAeTitle();
    }
    void MBaseSession::set_DvtAeTitle(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDvtAeTitle(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_SutAeTitle()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSutAeTitle();
    }
    void MBaseSession::set_SutAeTitle(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutAeTitle(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_SutHostname()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSutHostname();
    }
    void MBaseSession::set_SutHostname(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutHostname(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt32 MBaseSession::get_DvtMaximumLengthReceived()
    {
        return m_pBASE_SESSION->getDvtMaximumLengthReceived();
    }
    void MBaseSession::set_DvtMaximumLengthReceived(System::UInt32 value)
    {
        m_pBASE_SESSION->setDvtMaximumLengthReceived(value);
    }

    System::UInt32 MBaseSession::get_SutMaximumLengthReceived()
    {
        return m_pBASE_SESSION->getSutMaximumLengthReceived();
    }
    void MBaseSession::set_SutMaximumLengthReceived(System::UInt32 value)
    {
        m_pBASE_SESSION->setSutMaximumLengthReceived(value);
    }

    System::String __gc* MBaseSession::get_DvtImplementationClassUid()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDvtImplementationClassUid();
    }
    void MBaseSession::set_DvtImplementationClassUid(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDvtImplementationClassUid(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }

    System::String __gc* MBaseSession::get_SutImplementationClassUid()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSutImplementationClassUid();
    }
    void MBaseSession::set_SutImplementationClassUid(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutImplementationClassUid(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }

    System::String __gc* MBaseSession::get_DvtImplementationVersionName()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDvtImplementationVersionName();
    }
    void MBaseSession::set_DvtImplementationVersionName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDvtImplementationVersionName(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }

    System::String __gc* MBaseSession::get_SutImplementationVersionName()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSutImplementationVersionName();
    }
    void MBaseSession::set_SutImplementationVersionName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutImplementationVersionName(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }

    System::UInt16 MBaseSession::get_SutPort()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getSutPort();
    }
    void MBaseSession::set_SutPort(System::UInt16 value)
    {
        return m_pBASE_SESSION->setSutPort(value);
    }

    System::UInt16 MBaseSession::get_DvtPort()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getDvtPort();
    }
    void MBaseSession::set_DvtPort(System::UInt16 value)
    {
        return m_pBASE_SESSION->setDvtPort(value);
    }

    System::UInt16 MBaseSession::get_DvtSocketTimeOut()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getDvtSocketTimeout();
    }
    void MBaseSession::set_DvtSocketTimeOut(System::UInt16 value)
    {
        return m_pBASE_SESSION->setDvtSocketTimeout(value);
    }

    bool MBaseSession::get_UseSecureSockets()
    {
        return m_pBASE_SESSION->getUseSecureSockets();
    }
    void MBaseSession::set_UseSecureSockets(bool value)
    {
        return m_pBASE_SESSION->setUseSecureSockets(value);
    }

    System::String __gc* MBaseSession::get_TlsPassword()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getTlsPassword();
    }
    void MBaseSession::set_TlsPassword(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setTlsPassword(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_TlsVersion()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getTlsVersion();
    }
    void MBaseSession::set_TlsVersion(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setTlsVersion(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    bool MBaseSession::get_CheckRemoteCertificate()
    {
        return m_pBASE_SESSION->getCheckRemoteCertificate();
    }
    void MBaseSession::set_CheckRemoteCertificate(bool value)
    {
        return m_pBASE_SESSION->setCheckRemoteCertificate(value);
    }

    System::String __gc* MBaseSession::get_CipherList()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getCipherList();
    }
    void MBaseSession::set_CipherList(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setCipherList(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    bool MBaseSession::IsValidCipherList(System::String __gc* value)
    {
        bool bValid = false;
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bValid = isCipherListValid(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return bValid;
    }

    bool MBaseSession::get_CacheTlsSessions()
    {
        return m_pBASE_SESSION->getCacheTlsSessions();
    }
    void MBaseSession::set_CacheTlsSessions(bool value)
    {
        return m_pBASE_SESSION->setCacheTlsSessions(value);
    }

    System::UInt16 MBaseSession::get_TlsCacheTimeout()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getTlsCacheTimeout();
    }
    void MBaseSession::set_TlsCacheTimeout(System::UInt16 value)
    {
        return m_pBASE_SESSION->setTlsCacheTimeout(value);
    }

    System::String __gc* MBaseSession::get_CredentialsFileName()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getCredentialsFilename().c_str();
    }
    void MBaseSession::set_CredentialsFileName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setCredentialsFilename(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_CertificateFileName()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getCertificateFilename().c_str();
    }
    void MBaseSession::set_CertificateFileName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setCertificateFilename(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    void MBaseSession::set_SocketParametersChanged(bool value)
    {
        return m_pBASE_SESSION->setSocketParametersChanged(value);
    }

    bool MBaseSession::get_AutoType2Attributes()
    {
        return m_pBASE_SESSION->getAutoType2Attributes();
    }
    void MBaseSession::set_AutoType2Attributes(bool value)
    {
        return m_pBASE_SESSION->setAutoType2Attributes(value);
    }
    bool MBaseSession::get_AutoCreateDirectory()
    {
        return m_pBASE_SESSION->getAutoCreateDirectory();
    }
    void MBaseSession::set_AutoCreateDirectory(bool value)
    {
        return m_pBASE_SESSION->setAutoCreateDirectory(value);
    }
    bool MBaseSession::get_DefineSqLength()
    {
        return m_pBASE_SESSION->getDefineSqLength();
    }
    void MBaseSession::set_DefineSqLength(bool value)
    {
        return m_pBASE_SESSION->setDefineSqLength(value);
    }
    bool MBaseSession::get_AddGroupLength()
    {
        return m_pBASE_SESSION->getAddGroupLength();
    }
    void MBaseSession::set_AddGroupLength(bool value)
    {
        return m_pBASE_SESSION->setAddGroupLength(value);
    }

    bool MBaseSession::get_StrictValidation()
    {
        return m_pBASE_SESSION->getStrictValidation();
    }
    void MBaseSession::set_StrictValidation(bool value)
    {
        return m_pBASE_SESSION->setStrictValidation(value);
    }
	
    bool MBaseSession::get_TestLogValidationResults()
    {
        return m_pBASE_SESSION->getTestLogValidationResults();
    }
    void MBaseSession::set_TestLogValidationResults(bool value)
    {
        return m_pBASE_SESSION->setTestLogValidationResults(value);
    }

    bool MBaseSession::get_DetailedValidationResults()
    {
        return m_pBASE_SESSION->getDetailedValidationResults();
    }
    void MBaseSession::set_DetailedValidationResults(bool value)
    {
        return m_pBASE_SESSION->setDetailedValidationResults(value);
    }

    bool MBaseSession::get_SummaryValidationResults()
    {
        return m_pBASE_SESSION->getSummaryValidationResults();
    }
    void MBaseSession::set_SummaryValidationResults(bool value)
    {
        return m_pBASE_SESSION->setSummaryValidationResults(value);
    }
	
    bool MBaseSession::get_UsePrivateAttributeMapping()
    {
        return BASE_SESSION_CLASS::getUsePrivateAttributeMapping();
    }
    void MBaseSession::set_UsePrivateAttributeMapping(bool value)
    {
        return BASE_SESSION_CLASS::setUsePrivateAttributeMapping(value);
    }
	
    bool MBaseSession::get_IncludeType3NotPresentInResults()
    {
        return m_pBASE_SESSION->getIncludeType3NotPresentInResults();
    }
    void MBaseSession::set_IncludeType3NotPresentInResults(bool value)
    {
        return m_pBASE_SESSION->setIncludeType3NotPresentInResults(value);
    }

    bool MBaseSession::get_ContinueOnError()
    {
        return m_pBASE_SESSION->getContinueOnError();
    }
    void MBaseSession::set_ContinueOnError(bool value)
    {
        return m_pBASE_SESSION->setContinueOnError(value);
    }

    bool MBaseSession::get_ValidateReferencedFile()
    {
        return m_pBASE_SESSION->getValidateReferencedFile();
    }
    void MBaseSession::set_ValidateReferencedFile(bool value)
    {
        return m_pBASE_SESSION->setValidateReferencedFile(value);
    }

	bool MBaseSession::get_DumpAttributesOfRefFiles()
    {
        return m_pBASE_SESSION->getDumpAttributesOfRefFiles();
    }
    void MBaseSession::set_DumpAttributesOfRefFiles(bool value)
    {
        return m_pBASE_SESSION->setDumpAttributesOfRefFiles(value);
    }

    bool MBaseSession::get_UnVrDefinitionLookUp()
    {
        return m_pBASE_SESSION->getUnVrDefinitionLookUp();
    }
    void MBaseSession::set_UnVrDefinitionLookUp(bool value)
    {
        return m_pBASE_SESSION->setUnVrDefinitionLookUp(value);
    }

	System::UInt16 MBaseSession::get_DelayBeforeNEventReport()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getDelayForStorageCommitment();
    }
    void MBaseSession::set_DelayBeforeNEventReport(System::UInt16 value)
    {
        return m_pBASE_SESSION->setDelayForStorageCommitment(value);
    }
	void MBaseSession::set_AcceptDuplicateImage(bool value)
    {
        return m_pBASE_SESSION->setAcceptDuplicateImage(value);
    }
	void MBaseSession::set_StoreCStoreReqOnly(bool value)
    {
        return m_pBASE_SESSION->setStoreCStoreReqOnly(value);
    }

    Wrappers::StorageMode MBaseSession::get_StorageMode()
    {
        switch (m_pBASE_SESSION->getStorageMode())
        {
        case SM_AS_DATASET:
            return Wrappers::StorageModeAsDataSet;
        case SM_AS_MEDIA:
            return Wrappers::StorageModeAsMedia;
        case SM_AS_MEDIA_ONLY:
            return Wrappers::StorageModeAsMediaOnly;
        case SM_NO_STORAGE:
            return Wrappers::StorageModeNoStorage;
        case SM_TEMPORARY_PIXEL_ONLY:
            return Wrappers::StorageModeTemporaryPixelOnly;
        default:
            // Unknown Wrappers::StorageMode
            throw new System::NotImplementedException();
        }
    }
    void MBaseSession::set_StorageMode(Wrappers::StorageMode value)
    {
        STORAGE_MODE_ENUM storageMode;
        switch (value)
        {
        case Wrappers::StorageModeAsDataSet:
            storageMode = SM_AS_DATASET;
            break;
        case Wrappers::StorageModeAsMedia:
            storageMode = SM_AS_MEDIA;
            break;
        case Wrappers::StorageModeAsMediaOnly:
            storageMode = SM_AS_MEDIA_ONLY;
            break;
        case Wrappers::StorageModeNoStorage:
            storageMode = SM_NO_STORAGE;
            break;
        case Wrappers::StorageModeTemporaryPixelOnly:
            storageMode = SM_TEMPORARY_PIXEL_ONLY;
            break;
        default:
            // Unknown Wrappers::StorageMode
            throw new System::NotImplementedException();
            break;
        }
        return m_pBASE_SESSION->setStorageMode(storageMode);
    }

    bool MBaseSession::get_LogScpThread()
    {
        return m_pBASE_SESSION->getLogScpThread();
    }
    void MBaseSession::set_LogScpThread(bool value)
    {
        return m_pBASE_SESSION->setLogScpThread(value);
    }

    void MBaseSession::DeleteSupportedTransferSyntaxes()
    {
        return m_pBASE_SESSION->deleteSupportedTransferSyntaxes();
    }

    void MBaseSession::AddSupportedTransferSyntax(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->addSupportedTransferSyntax(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt16 MBaseSession::get_NrOfSupportedTransferSyntaxes()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->noSupportedTransferSyntaxes();
    }

    System::String __gc* MBaseSession::get_SupportedTransferSyntax(System::UInt16 idx)
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSupportedTransferSyntax(idx);
    }

    System::String __gc* MBaseSession::get_DicomScriptRootDirectory()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDicomScriptRoot();
    }
    void MBaseSession::set_DicomScriptRootDirectory(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDicomScriptRoot(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    bool MBaseSession::Begin([In, Out] bool value)
    {
        return m_pBASE_SESSION->begin(value);
    }

    void MBaseSession::End(void)
    {
        return m_pBASE_SESSION->end();
    }

    bool MBaseSession::ExecuteScript(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->executeScript(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }
    bool MBaseSession::ParseScript(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->parseScript(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }

    void MBaseSession::set_ScpEmulatorType(Wrappers::ScpEmulatorType value)
    {
        SCP_EMULATOR_ENUM scpEmulatorType;
        switch (value)
        {
        case Wrappers::ScpEmulatorTypePrint:
            scpEmulatorType = SCP_EMULATOR_PRINT;
            break;
        case Wrappers::ScpEmulatorTypeStorage:
            scpEmulatorType = SCP_EMULATOR_STORAGE;
            break;
		case Wrappers::ScpEmulatorTypeStorageCommit:
            scpEmulatorType = SCP_EMULATOR_STORAGE_COMMIT;
            break;
        case Wrappers::ScpEmulatorTypeMpps:
            scpEmulatorType = SCP_EMULATOR_MPPS;
            break;
        case Wrappers::ScpEmulatorTypeWorklist:
            scpEmulatorType = SCP_EMULATOR_WORKLIST;
            break;
        case Wrappers::ScpEmulatorTypeQueryRetrieve:
            scpEmulatorType = SCP_EMULATOR_QUERY_RETRIEVE;
            break;
		case Wrappers::ScpEmulatorTypeUnknown:
			scpEmulatorType = SCP_EMULATOR_UNKNOWN;
            break;
        default:
            System::Diagnostics::Trace::Assert(false);
            scpEmulatorType = SCP_EMULATOR_UNKNOWN;
            break;
        }
        return m_pBASE_SESSION->setScpEmulatorType(scpEmulatorType);
    }

    Wrappers::ScpEmulatorType MBaseSession::get_ScpEmulatorType()
    {
        switch (m_pBASE_SESSION->getScpEmulatorType())
        {
		case SCP_EMULATOR_PRINT:
            return Wrappers::ScpEmulatorTypePrint;
		case SCP_EMULATOR_STORAGE:
            return Wrappers::ScpEmulatorTypeStorage;
		case SCP_EMULATOR_STORAGE_COMMIT:
            return Wrappers::ScpEmulatorTypeStorageCommit;
		case SCP_EMULATOR_MPPS:
            return Wrappers::ScpEmulatorTypeMpps;
		case SCP_EMULATOR_WORKLIST:
            return Wrappers::ScpEmulatorTypeWorklist;
		case SCP_EMULATOR_QUERY_RETRIEVE:
            return Wrappers::ScpEmulatorTypeQueryRetrieve;
		case SCP_EMULATOR_UNKNOWN:
            return Wrappers::ScpEmulatorTypeUnknown;
        default:
            // Unknown Wrappers::SessionType
            throw new System::NotImplementedException();
        }
    }

    void MBaseSession::set_ScuEmulatorType(Wrappers::ScuEmulatorType value)
    {
        SCU_EMULATOR_ENUM scuEmulatorType;
        switch (value)
        {
        case Wrappers::ScuEmulatorTypeStorage:
            scuEmulatorType = SCU_EMULATOR_STORAGE;
            break;
		case Wrappers::ScuEmulatorTypeUnknown:
			scuEmulatorType = SCU_EMULATOR_UNKNOWN;
            break;
        default:
            System::Diagnostics::Trace::Assert(false);
            scuEmulatorType = SCU_EMULATOR_UNKNOWN;
            break;
        }
        return m_pBASE_SESSION->setScuEmulatorType(scuEmulatorType);
    }

    Wrappers::ScuEmulatorType MBaseSession::get_ScuEmulatorType()
    {
        switch (m_pBASE_SESSION->getScuEmulatorType())
        {
		case SCU_EMULATOR_STORAGE:
            return Wrappers::ScuEmulatorTypeStorage;
		case SCU_EMULATOR_UNKNOWN:
            return Wrappers::ScuEmulatorTypeUnknown;
        default:
            // Unknown Wrappers::SessionType
            throw new System::NotImplementedException();
        }
    }

    bool MBaseSession::EmulateSCP(void)
    {
        return m_pBASE_SESSION->emulateSCP();
    }

    bool MBaseSession::SendStatusEvent(void)
    {
        return m_pBASE_SESSION->sendStatusEvent();
    }

    bool MBaseSession::TerminateConnection(void)
    {
        return m_pBASE_SESSION->terminateConnection();
    }

	bool MBaseSession::AbortEmulation(void)
    {
        return m_pBASE_SESSION->abortEmulation();
    }

	bool MBaseSession::AbortEmulationFromSCU(void)
    {
        return m_pBASE_SESSION->abortEmulationFromSCU();
    }

    void MBaseSession::ResetAssociation(void)
    {
        return m_pBASE_SESSION->resetAssociation();
    }

    bool MBaseSession::BeginMediaValidation(void)
    {
        return m_pBASE_SESSION->beginMediaValidation();
    }

    bool MBaseSession::ValidateMediaFile(System::String __gc* filename)
    {
        char* pAnsiFilenameString = (char*)(void*)Marshal::StringToHGlobalAnsi(filename);
        bool retVal = m_pBASE_SESSION->validateMediaFile(pAnsiFilenameString);
        Marshal::FreeHGlobal(pAnsiFilenameString);
        return retVal;
    }

    bool MBaseSession::ValidateMediaFile(System::String __gc* filename,
            Wrappers::FileContentType fileContentType,
            System::String __gc* sopClassUid, 
            System::String __gc* sopInstanceUid, 
            System::String __gc* transferSyntaxUid)
	{
        char* pAnsiFilenameString = (char*)(void*)Marshal::StringToHGlobalAnsi(filename);
        char* pAnsiSopClassUid = (char*)(void*)Marshal::StringToHGlobalAnsi(sopClassUid);
        char* pAnsiSopInstanceUid = (char*)(void*)Marshal::StringToHGlobalAnsi(sopInstanceUid);
        char* pAnsiTransferSyntaxUid = (char*)(void*)Marshal::StringToHGlobalAnsi(transferSyntaxUid);

		MEDIA_FILE_CONTENT_TYPE_ENUM mcf_type = MFC_MEDIAFILE;
        switch (fileContentType)
        {		
        case Wrappers::FileContentTypeMediaFile:
            mcf_type = MFC_MEDIAFILE;
            break;
		case Wrappers::FileContentTypeCommandSet:
			mcf_type = MFC_COMMANDSET;
            break;
		case Wrappers::FileContentTypeDataSet:
			mcf_type = MFC_DATASET;
			break;
        default:
            System::Diagnostics::Trace::Assert(false);
            break;
        }

        bool retVal = m_pBASE_SESSION->validateMediaFile(pAnsiFilenameString, mcf_type, pAnsiSopClassUid, pAnsiSopInstanceUid, pAnsiTransferSyntaxUid);
        Marshal::FreeHGlobal(pAnsiFilenameString);
        Marshal::FreeHGlobal(pAnsiSopClassUid);
        Marshal::FreeHGlobal(pAnsiSopInstanceUid);
        Marshal::FreeHGlobal(pAnsiTransferSyntaxUid);
        return retVal;
	}

    bool MBaseSession::ValidateMediaFile(System::String __gc* filename, System::String __gc* recordsToFilter, int numberRecordsToFilter)
    {
        char* pAnsiFilenameString = (char*)(void*)Marshal::StringToHGlobalAnsi(filename);
        char* pAnsiRecordsToFilterString = (char*)(void*)Marshal::StringToHGlobalAnsi(recordsToFilter);
        bool retVal = m_pBASE_SESSION->validateMediaFile(pAnsiFilenameString, pAnsiRecordsToFilterString, numberRecordsToFilter);
        Marshal::FreeHGlobal(pAnsiFilenameString);
        Marshal::FreeHGlobal(pAnsiRecordsToFilterString);
        return retVal;
    }

    bool MBaseSession::EndMediaValidation(void)
    {
        return m_pBASE_SESSION->endMediaValidation();
    }

    System::UInt32 MBaseSession::get_InstanceId()
    {
        return m_pBASE_SESSION->getInstanceId();
    }
    void MBaseSession::set_InstanceId(System::UInt32 value)
    {
        return m_pBASE_SESSION->setInstanceId(value);
    }

    System::String __gc* MBaseSession::get_SessionDirectory()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSessionDirectory();
    }

    System::String __gc* MBaseSession::get_SessionFileName()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getFilename().c_str();
    }

    void MBaseSession::set_SessionFileName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setFileName(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    void MBaseSession::set_SessionDirectory(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSessionDirectory(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    bool MBaseSession::get_AppendToResultsFile()
    {
        return m_pBASE_SESSION->getAppendToResultsFile();
    }
    void MBaseSession::set_AppendToResultsFile(bool value)
    {
        return m_pBASE_SESSION->setAppendToResultsFile(value);
    }

	void MBaseSession::ResetCounter(void)
    {
        return m_pBASE_SESSION->resetCounter();
    }
    void MBaseSession::IncrementCounter(void)
    {
        return m_pBASE_SESSION->incrementCounter();
    }

    System::UInt16 MBaseSession::get_Counter()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getCounter();
    }

    bool MBaseSession::ReloadDefinitions(void)
    {
        return m_pBASE_SESSION->reloadDefinitions();
    }
    void MBaseSession::RemoveDefinitions(void)
    {
        return m_pBASE_SESSION->removeDefinitions();
    }

    Wrappers::SessionType MBaseSession::get_SessionType()
    {
        switch (m_pBASE_SESSION->getSessionType())
        {
        case ST_UNKNOWN:
            return Wrappers::SessionTypeUnknown;
        case ST_SCRIPT:
            return Wrappers::SessionTypeScript;
        case ST_EMULATOR:
            return Wrappers::SessionTypeEmulator;
        case ST_MEDIA:
            return Wrappers::SessionTypeMedia;
		case ST_SNIFFER:
            return Wrappers::SessionTypeSniffer;
        default:
            // Unknown Wrappers::SessionType
            throw new System::NotImplementedException();
        }
    }
    void MBaseSession::set_SessionType(Wrappers::SessionType value)
    {
        SESSION_TYPE_ENUM sessionType;
        switch (value)
        {
        case Wrappers::SessionTypeUnknown:
            sessionType = ST_UNKNOWN;
            break;
        case Wrappers::SessionTypeScript:
            sessionType = ST_SCRIPT;
            break;
        case Wrappers::SessionTypeEmulator:
            sessionType = ST_EMULATOR;
            break;
        case Wrappers::SessionTypeMedia:
            sessionType = ST_MEDIA;
            break;
		case Wrappers::SessionTypeSniffer:
            sessionType = ST_SNIFFER;
            break;
        default:
            // Unknown Wrappers::SessionType
            throw new System::NotImplementedException();
        }
        return m_pBASE_SESSION->setSessionType(sessionType);
    }

    Wrappers::SessionType MBaseSession::get_RunTimeSessionType()
    {
        switch (m_pBASE_SESSION->getRuntimeSessionType())
        {
        case ST_UNKNOWN:
            return Wrappers::SessionTypeUnknown;
        case ST_SCRIPT:
            return Wrappers::SessionTypeScript;
        case ST_EMULATOR:
            return Wrappers::SessionTypeEmulator;
        case ST_MEDIA:
            return Wrappers::SessionTypeMedia;
		case ST_SNIFFER:
            return Wrappers::SessionTypeSniffer;
        default:
            // Unknown Wrappers::SessionType
            throw new System::NotImplementedException();
        }
    }

    System::String __gc* MBaseSession::get_SessionTitle()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSessionTitle();
    }
    void MBaseSession::set_SessionTitle(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSessionTitle(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt16 MBaseSession::get_SessionId()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getSessionId();
    }
    void MBaseSession::set_SessionId(System::UInt16 value)
    {
        return m_pBASE_SESSION->setSessionId(value);
    }

    System::String __gc* MBaseSession::get_Manufacturer()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getManufacturer();
    }
    void MBaseSession::set_Manufacturer(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setManufacturer(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_ModelName()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getModelName();
    }
    void MBaseSession::set_ModelName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setModelName(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_SoftwareVersions()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getSoftwareVersions();
    }
    void MBaseSession::set_SoftwareVersions(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSoftwareVersions(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_ApplicationEntityName()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getApplicationEntityName();
    }
    void MBaseSession::set_ApplicationEntityName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setApplicationEntityName(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_ApplicationEntityVersion()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getApplicationEntityVersion();
    }
    void MBaseSession::set_ApplicationEntityVersion(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setApplicationEntityVersion(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_TestedBy()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getTestedBy();
    }
    void MBaseSession::set_TestedBy(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setTestedBy(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_Date()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDate();
    }
    void MBaseSession::set_Date(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDate(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    LogMask MBaseSession::get_LogMask()
    {
        return m_pBASE_SESSION->getLogMask();
    }

    System::String __gc* MBaseSession::get_DefinitionFileRootDirectory()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDefinitionFileRoot();
    }

    void MBaseSession::set_DefinitionFileRootDirectory(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDefinitionFileRoot(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    void MBaseSession::AddDefinitionFileDirectory(System::String __gc* pDefinitionFileDirectory)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(pDefinitionFileDirectory);
        m_pBASE_SESSION->addDefinitionDirectory(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt16 MBaseSession::get_NrOfDefinitionFileDirectories()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->noDefinitionDirectories();
    }
    
    System::String __gc* MBaseSession::get_DefinitionFileDirectory(System::UInt16 index)
    {
        return m_pBASE_SESSION->getDefinitionDirectory(index).c_str();
    }
    
    void MBaseSession::RemoveAllDefinitionFileDirectories()
    {
        m_pBASE_SESSION->removeAllDefinitionDirectories();
    }

    System::UInt16 MBaseSession::get_NrOfDefinitionFiles()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->noDefinitionFiles();
    }

    System::String __gc* MBaseSession::get_DefinitionFileName(System::UInt16 index)
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDefinitionFilename(index);
    }

    System::String __gc* MBaseSession::get_DataDirectory()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDataDirectory();
    }

    void MBaseSession::set_DataDirectory(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDataDirectory(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_ResultsRootDirectory()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getResultsRoot();
    }

    void MBaseSession::set_ResultsRootDirectory(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setResultsRoot(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_AbsolutePixelPathName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        const char* retVal = m_pBASE_SESSION->getAbsolutePixelPathname(pAnsiString).c_str();
        Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return retVal;
    }

    bool MBaseSession::get_IsOpen()
    {
        return m_pBASE_SESSION->isOpen();
    }

	bool MBaseSession::get_IsAssociated()
    {
        return m_pBASE_SESSION->getIsAssociated();
    }	

    void MBaseSession::Load(
        System::String __gc* sessionFileName,
        [In, Out] bool definitionFileLoaded,
        bool andBeginIt)
    {
        string std_string;
        Wrappers::MarshalString (sessionFileName, std_string);
        bool retVal = m_pBASE_SESSION->load(std_string, definitionFileLoaded, andBeginIt);
        if (!retVal) 
            throw new System::ApplicationException(
            System::String::Format("Could not load session {0}.", sessionFileName)
            );
    }

    bool MBaseSession::Save()
    {
        return m_pBASE_SESSION->save();
    }

    DvtkData::Media::DicomFile __gc* MBaseSession::ReadMedia(
        System::String __gc* pFileName)
    {
        DvtkData::Media::DicomFile __gc* pDicomFile;
        char* pAnsiStringFileName = (char*)(void*)Marshal::StringToHGlobalAnsi(pFileName);

		// read media - flags use session storage mode = true and log it = true
        FILE_DATASET_CLASS* pFileDataSet = m_pBASE_SESSION->readMedia(pAnsiStringFileName, MFC_UNKNOWN, "", "", "", true, true);
        pDicomFile = 
            ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pFileDataSet);
        delete pFileDataSet;
        Marshal::FreeHGlobal(pAnsiStringFileName);
        return pDicomFile;
    }

    System::Boolean MBaseSession::WriteMedia(
        DvtkData::Media::DicomFile __gc* pDicomFile, System::String __gc* pFileName)
    {
        bool success = false;
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDicomFile, pFileName);
        success = m_pBASE_SESSION->writeMedia(fileDataset_ptr);
        delete fileDataset_ptr;
        return success;
    }

	DvtkData::Media::DicomDir __gc* MBaseSession::ReadDicomdir(
        System::String __gc* pFileName)
    {
        DvtkData::Media::DicomDir __gc* pDicomFile;
        char* pAnsiStringFileName = (char*)(void*)Marshal::StringToHGlobalAnsi(pFileName);

		// read media - flags use session storage mode = true and log it = true
        FILE_DATASET_CLASS* pFileDataSet = m_pBASE_SESSION->readDicomdir(pAnsiStringFileName, true, true);
        pDicomFile = 
            ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::ConvertToDicomdir(pFileDataSet);
        delete pFileDataSet;
        Marshal::FreeHGlobal(pAnsiStringFileName);
        return pDicomFile;
    }

    System::Boolean MBaseSession::WriteDicomdir(
        DvtkData::Media::DicomDir __gc* pDicomFile, System::String __gc* pFileName)
    {
        bool success = false;
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDicomFile, pFileName);
        success = m_pBASE_SESSION->writeDicomdir(fileDataset_ptr);
        delete fileDataset_ptr;
        return success;
    }

    bool MBaseSession::LoadDefinitionFile(
        System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->loadDefinition(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }

    bool MBaseSession::UnLoadDefinitionFile(
        System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->unloadDefinition(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }

    System::String __gc* MBaseSession::get_DescriptionDirectory()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pBASE_SESSION->getDescriptionDirectory().c_str();
    }

    void MBaseSession::set_DescriptionDirectory(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDescriptionDirectory(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String __gc* MBaseSession::get_SopUid(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        string umString = m_pBASE_SESSION->getSopUid(pAnsiString);
        const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return retVal;
    }

    System::String __gc* MBaseSession::get_IodName(System::String __gc* value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        string umString = m_pBASE_SESSION->getIodName(pAnsiString);
        const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return retVal;
    }

	System::String __gc* MBaseSession::get_IodNameFromDefinition(DvtkData::Dimse::DimseCommand command, System::String __gc* uid)
	{
		DIMSE_CMD_ENUM cmdEnum = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(command);
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(uid);
		string umString = m_pBASE_SESSION->getIodNameFromDefinition(cmdEnum, pAnsiString);
		const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return retVal;
	}

	System::String __gc* MBaseSession::get_FileNameFromSOPUID(DvtkData::Dimse::DimseCommand command, System::String __gc* uid)
	{
		DIMSE_CMD_ENUM cmdEnum = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(command);
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(uid);
		string umString = m_pBASE_SESSION->getFileNameFromSOPUID(cmdEnum, pAnsiString);
		const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return retVal;
	}

	System::String __gc* MBaseSession::get_AttributeNameFromDefinition(DvtkData::Dimse::Tag __gc* pTag)
	{
		System::String __gc *pAttributeName = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::GetAttributeName(pTag);
        return pAttributeName;
	}

	DvtkData::Dimse::VR MBaseSession::get_AttributeVrFromDefinition(DvtkData::Dimse::Tag __gc* pTag)
	{
		DvtkData::Dimse::VR vr = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::GetAttributeVr(pTag);
        return vr;
	}

    void MBaseSession::EnableLogger(void)
    {
        return m_pBASE_SESSION->enableLogger();
    }

    void MBaseSession::DisableLogger(void)
    {
        return m_pBASE_SESSION->disableLogger();
    }
    
    bool MBaseSession::get_LogLevelEnabled(System::UInt32 logLevel)
    {
        return m_pBASE_SESSION->isLogLevel(logLevel);
    }
    void MBaseSession::set_LogLevelEnabled(System::UInt32 logLevel, bool enabled)
    {
        return m_pBASE_SESSION->setLogLevel(enabled, logLevel);
    }

    bool MBaseSession::get_IsStopped()
    {
        return m_pBASE_SESSION->isSessionStopped();
    }

    void MBaseSession::AddDicomScript(System::String __gc* value)
    {
        // only store this information in a Scripting session
        if (m_pBASE_SESSION->getRuntimeSessionType() == ST_SCRIPT)
        {
            char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
            // should check that file extension is OK
            string dicomScript = pAnsiString;
            DICOM_SCRIPT_CLASS *dicomScript_ptr =
                new DICOM_SCRIPT_CLASS(reinterpret_cast<SCRIPT_SESSION_CLASS*>(m_pBASE_SESSION), dicomScript);
            m_pBASE_SESSION->addDicomScript(dicomScript_ptr);
            Marshal::FreeHGlobal(pAnsiString);
        }
        return;
    }

    void MBaseSession::set_Rules(System::Uri __gc* pRulesUri)
    {
        if (this->m_pSerializationAdapter == NULL) return;
        if (
            pRulesUri->Equals(MBaseSession::RuleUriStandardRules) ||
            pRulesUri->Equals(MBaseSession::RuleUriStrictRules)
            ) 
        {
            this->m_pSerializationAdapter->set_Rules(pRulesUri);
        }
        else
        {
            throw new System::ArgumentException();
        }
        return;
    }
}