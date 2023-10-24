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
        MBaseSession::RuleUriStandardRules = gcnew System::Uri("urn:rules-dvtk:standard");
        MBaseSession::RuleUriStrictRules = gcnew System::Uri("urn:rules-dvtk:strict");
    }

    MBaseSession::MBaseSession(void)
    {
        m_pActivityReportingAdapter = nullptr;
        m_pSerializationAdapter = nullptr;
        m_pCountingAdapter = nullptr;
    }

    void MBaseSession::Initialize(void)
    {
        System::String^ pSystemAppDomainBaseDirectory =
            System::AppDomain::CurrentDomain->BaseDirectory;
        char* pAnsiString = 
            (char*)(void*)Marshal::StringToHGlobalAnsi(pSystemAppDomainBaseDirectory);
        m_pBASE_SESSION->setSystemAppDomainBaseDirectory(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
    }

    MBaseSession::~MBaseSession(void)
    {
        if (m_pActivityReportingAdapter != nullptr) delete m_pActivityReportingAdapter;
        if (m_pSerializationAdapter != nullptr) delete m_pSerializationAdapter;
        if (m_pCountingAdapter != nullptr) delete m_pCountingAdapter;
    }

    void MBaseSession::SessionFileVersion::set(System::UInt16 value)
    {
        return m_pBASE_SESSION->sessionFileVersion(value);
    }

    void MBaseSession::InitTopSerializationAndCountingAndActivityReportingTargets(
			ISerializationTarget^ serializationTarget,
			ICountingTarget^ countingTarget,
			IActivityReportingTarget^ activityReportingTarget)
    {
		//
		// Preconditions: Method arguments.
		//
        if (serializationTarget == nullptr) throw gcnew System::ArgumentNullException();
        if (countingTarget == nullptr) throw gcnew System::ArgumentNullException();
        if (activityReportingTarget == nullptr) throw gcnew System::ArgumentNullException();
		//
		// Clean-up existing object-references.
		//
		if (this->m_pCountingAdapter != nullptr) delete this->m_pCountingAdapter;
		if (this->m_pSerializationAdapter != nullptr) delete this->m_pSerializationAdapter;
		if (this->m_pActivityReportingAdapter != nullptr) delete this->m_pActivityReportingAdapter;
		//
		// Create new objects.
		//
        this->m_pCountingAdapter = new CountingAdapter(countingTarget);
		System::Uri^ pRules = Wrappers::MBaseSession::RuleUriStrictRules;
        this->m_pSerializationAdapter = new SerializationAdapter(serializationTarget, countingTarget, pRules);
        this->m_pBASE_SESSION->setSerializer(this->m_pSerializationAdapter);
		this->m_pActivityReportingAdapter = new ActivityReportingAdapter(activityReportingTarget, countingTarget);
        this->m_pBASE_SESSION->setActivityReporter(this->m_pActivityReportingAdapter);
		//
		// User activity reporting will only be counted in the top level countmanager.
		//
        this->Rules::set(MBaseSession::RuleUriStandardRules);
        //this->set_Rules(MBaseSession::RuleUriStandardRules);
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
		if (m_pSerializationAdapter != nullptr) 
		{
			m_pSerializationAdapter->EndSerializer();
		}
		if (m_pCountingAdapter != nullptr)
		{
			// TODO!?
		}
		if (m_pActivityReportingAdapter != nullptr) 
		{
			// TODO!?
		}
	}

    Wrappers::SutRole MBaseSession::SutRole::get()
    {
        Wrappers::SutRole sutRole;
        switch (m_pBASE_SESSION->getSutRole())
        {
        case UP_ACCEPTOR_REQUESTOR:
            sutRole = Wrappers::SutRole::SutRoleAcceptorRequestor;
            break;
        case UP_ACCEPTOR:
            sutRole = Wrappers::SutRole::SutRoleAcceptor;
            break;
        case UP_REQUESTOR:
            sutRole = Wrappers::SutRole::SutRoleRequestor;
            break;
        default:
            // Unknown Wrappers::SutRole
            throw gcnew System::NotImplementedException();
        }

        return sutRole;
    }

    void MBaseSession::SutRole::set(Wrappers::SutRole value)
    {
        UP_ENUM sutRole;
        switch (value)
        {
        case Wrappers::SutRole::SutRoleAcceptorRequestor:
            sutRole = UP_ACCEPTOR_REQUESTOR;
            break;
        case Wrappers::SutRole::SutRoleAcceptor:
            sutRole = UP_ACCEPTOR;
            break;
        case Wrappers::SutRole::SutRoleRequestor:
            sutRole = UP_REQUESTOR;
            break;
        default:
            // Unknown Wrappers::SutRole
            throw gcnew System::NotImplementedException();
        }

        m_pBASE_SESSION->setSutRole(sutRole);
    }

    System::String^ MBaseSession::DvtAeTitle::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDvtAeTitle());
        return clistr; 
        //return m_pBASE_SESSION->getDvtAeTitle();
    }
    void MBaseSession::DvtAeTitle::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDvtAeTitle(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::SutAeTitle::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSutAeTitle());
        return clistr;
        //return m_pBASE_SESSION->getSutAeTitle();
    }
    void MBaseSession::SutAeTitle::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutAeTitle(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::SutHostname::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSutHostname());
        return clistr;
        //return m_pBASE_SESSION->getSutHostname();
    }
    void MBaseSession::SutHostname::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutHostname(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt32 MBaseSession::DvtMaximumLengthReceived::get()
    {
        return m_pBASE_SESSION->getDvtMaximumLengthReceived();
    }
    void MBaseSession::DvtMaximumLengthReceived::set(System::UInt32 value)
    {
        m_pBASE_SESSION->setDvtMaximumLengthReceived(value);
    }

    System::UInt32 MBaseSession::SutMaximumLengthReceived::get()
    {
        return m_pBASE_SESSION->getSutMaximumLengthReceived();
    }
    void MBaseSession::SutMaximumLengthReceived::set(System::UInt32 value)
    {
        m_pBASE_SESSION->setSutMaximumLengthReceived(value);
    }

    System::String^ MBaseSession::DvtImplementationClassUid::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDvtImplementationClassUid());
        return clistr;
        //return m_pBASE_SESSION->getDvtImplementationClassUid();
    }
    void MBaseSession::DvtImplementationClassUid::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDvtImplementationClassUid(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
    }

    System::String^ MBaseSession::SutImplementationClassUid::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSutImplementationClassUid());
        return clistr;
        //return m_pBASE_SESSION->getSutImplementationClassUid();
    }
    void MBaseSession::SutImplementationClassUid::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutImplementationClassUid(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
    }

    System::String^ MBaseSession::DvtImplementationVersionName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDvtImplementationVersionName());
        return clistr;
        //return m_pBASE_SESSION->getDvtImplementationVersionName();
    }
    void MBaseSession::DvtImplementationVersionName::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDvtImplementationVersionName(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
    }

    System::String^ MBaseSession::SutImplementationVersionName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSutImplementationVersionName());
        return clistr;
        //return m_pBASE_SESSION->getSutImplementationVersionName();
    }
    void MBaseSession::SutImplementationVersionName::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSutImplementationVersionName(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
    }

    System::UInt16 MBaseSession::SutPort::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getSutPort();
    }
    void MBaseSession::SutPort::set(System::UInt16 value)
    {
        return m_pBASE_SESSION->setSutPort(value);
    }

    System::UInt16 MBaseSession::DvtPort::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getDvtPort();
    }
    void MBaseSession::DvtPort::set(System::UInt16 value)
    {
        return m_pBASE_SESSION->setDvtPort(value);
    }

    System::UInt16 MBaseSession::DvtSocketTimeOut::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getDvtSocketTimeout();
    }
    void MBaseSession::DvtSocketTimeOut::set(System::UInt16 value)
    {
        return m_pBASE_SESSION->setDvtSocketTimeout(value);
    }

    bool MBaseSession::UseSecureSockets::get()
    {
        return m_pBASE_SESSION->getUseSecureSockets();
    }
    void MBaseSession::UseSecureSockets::set(bool value)
    {
        return m_pBASE_SESSION->setUseSecureSockets(value);
    }

    System::String^ MBaseSession::TlsPassword::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getTlsPassword());
        return clistr;
        //return m_pBASE_SESSION->getTlsPassword();
    }
    void MBaseSession::TlsPassword::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setTlsPassword(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }


    System::String^ MBaseSession::MaxTlsVersion::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getMaxTlsVersion());
        return clistr;
        //return m_pBASE_SESSION->getTlsVersion();
    }
    void MBaseSession::MaxTlsVersion::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setMaxTlsVersion(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::MinTlsVersion::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getMinTlsVersion());
        return clistr;
        //return m_pBASE_SESSION->getTlsVersion();
    }
    void MBaseSession::MinTlsVersion::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setMinTlsVersion(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    bool MBaseSession::CheckRemoteCertificate::get()
    {
        return m_pBASE_SESSION->getCheckRemoteCertificate();
    }
    void MBaseSession::CheckRemoteCertificate::set(bool value)
    {
        return m_pBASE_SESSION->setCheckRemoteCertificate(value);
    }

    System::String^ MBaseSession::CipherList::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getCipherList());
        return clistr;
        //return m_pBASE_SESSION->getCipherList();
    }
    void MBaseSession::CipherList::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setCipherList(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    bool MBaseSession::IsValidCipherList(System::String^ value)
    {
        bool bValid = false;
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bValid = isCipherListValid(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return bValid;
    }

    bool MBaseSession::CacheTlsSessions::get()
    {
        return m_pBASE_SESSION->getCacheTlsSessions();
    }
    void MBaseSession::CacheTlsSessions::set(bool value)
    {
        return m_pBASE_SESSION->setCacheTlsSessions(value);
    }

    System::UInt16 MBaseSession::TlsCacheTimeout::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getTlsCacheTimeout();
    }
    void MBaseSession::TlsCacheTimeout::set(System::UInt16 value)
    {
        return m_pBASE_SESSION->setTlsCacheTimeout(value);
    }

    System::String^ MBaseSession::CredentialsFileName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getCredentialsFilename().c_str());
        return clistr;
        //return m_pBASE_SESSION->getCredentialsFilename().c_str();
    }
    void MBaseSession::CredentialsFileName::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setCredentialsFilename(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::CertificateFileName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getCertificateFilename().c_str());
        return clistr;
        //return m_pBASE_SESSION->getCertificateFilename().c_str();
    }
    void MBaseSession::CertificateFileName::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setCertificateFilename(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    void MBaseSession::SocketParametersChanged::set(bool value)
    {
        return m_pBASE_SESSION->setSocketParametersChanged(value);
    }

    bool MBaseSession::AutoType2Attributes::get()
    {
        return m_pBASE_SESSION->getAutoType2Attributes();
    }
    void MBaseSession::AutoType2Attributes::set(bool value)
    {
        return m_pBASE_SESSION->setAutoType2Attributes(value);
    }
    bool MBaseSession::AutoCreateDirectory::get()
    {
        return m_pBASE_SESSION->getAutoCreateDirectory();
    }
    void MBaseSession::AutoCreateDirectory::set(bool value)
    {
        return m_pBASE_SESSION->setAutoCreateDirectory(value);
    }
    bool MBaseSession::DefineSqLength::get()
    {
        return m_pBASE_SESSION->getDefineSqLength();
    }
    void MBaseSession::DefineSqLength::set(bool value)
    {
        return m_pBASE_SESSION->setDefineSqLength(value);
    }
    bool MBaseSession::AddGroupLength::get()
    {
        return m_pBASE_SESSION->getAddGroupLength();
    }
    void MBaseSession::AddGroupLength::set(bool value)
    {
        return m_pBASE_SESSION->setAddGroupLength(value);
    }

    bool MBaseSession::StrictValidation::get()
    {
        return m_pBASE_SESSION->getStrictValidation();
    }
    void MBaseSession::StrictValidation::set(bool value)
    {
        return m_pBASE_SESSION->setStrictValidation(value);
    }
	
    bool MBaseSession::TestLogValidationResults::get()
    {
        return m_pBASE_SESSION->getTestLogValidationResults();
    }
    void MBaseSession::TestLogValidationResults::set(bool value)
    {
        return m_pBASE_SESSION->setTestLogValidationResults(value);
    }

    bool MBaseSession::DetailedValidationResults::get()
    {
        return m_pBASE_SESSION->getDetailedValidationResults();
    }
    void MBaseSession::DetailedValidationResults::set(bool value)
    {
        return m_pBASE_SESSION->setDetailedValidationResults(value);
    }

    bool MBaseSession::SummaryValidationResults::get()
    {
        return m_pBASE_SESSION->getSummaryValidationResults();
    }
    void MBaseSession::SummaryValidationResults::set(bool value)
    {
        return m_pBASE_SESSION->setSummaryValidationResults(value);
    }
	
    bool MBaseSession::UsePrivateAttributeMapping::get()
    {
        return BASE_SESSION_CLASS::getUsePrivateAttributeMapping();
    }
    void MBaseSession::UsePrivateAttributeMapping::set(bool value)
    {
        return BASE_SESSION_CLASS::setUsePrivateAttributeMapping(value);
    }
	
    bool MBaseSession::IncludeType3NotPresentInResults::get()
    {
        return m_pBASE_SESSION->getIncludeType3NotPresentInResults();
    }
    void MBaseSession::IncludeType3NotPresentInResults::set(bool value)
    {
        return m_pBASE_SESSION->setIncludeType3NotPresentInResults(value);
    }

    bool MBaseSession::ContinueOnError::get()
    {
        return m_pBASE_SESSION->getContinueOnError();
    }
    void MBaseSession::ContinueOnError::set(bool value)
    {
        return m_pBASE_SESSION->setContinueOnError(value);
    }

    bool MBaseSession::ValidateReferencedFile::get()
    {
        return m_pBASE_SESSION->getValidateReferencedFile();
    }
    void MBaseSession::ValidateReferencedFile::set(bool value)
    {
        return m_pBASE_SESSION->setValidateReferencedFile(value);
    }

	bool MBaseSession::DumpAttributesOfRefFiles::get()
    {
        return m_pBASE_SESSION->getDumpAttributesOfRefFiles();
    }
    void MBaseSession::DumpAttributesOfRefFiles::set(bool value)
    {
        return m_pBASE_SESSION->setDumpAttributesOfRefFiles(value);
    }

    bool MBaseSession::UnVrDefinitionLookUp::get()
    {
        return m_pBASE_SESSION->getUnVrDefinitionLookUp();
    }
    void MBaseSession::UnVrDefinitionLookUp::set(bool value)
    {
        return m_pBASE_SESSION->setUnVrDefinitionLookUp(value);
    }

	System::UInt16 MBaseSession::DelayBeforeNEventReport::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getDelayForStorageCommitment();
    }
    void MBaseSession::DelayBeforeNEventReport::set(System::UInt16 value)
    {
        return m_pBASE_SESSION->setDelayForStorageCommitment(value);
    }
	void MBaseSession::AcceptDuplicateImage::set(bool value)
    {
        return m_pBASE_SESSION->setAcceptDuplicateImage(value);
    }
	void MBaseSession::StoreCStoreReqOnly::set(bool value)
    {
        return m_pBASE_SESSION->setStoreCStoreReqOnly(value);
    }

    Wrappers::StorageMode MBaseSession::StorageMode::get()
    {
        switch (m_pBASE_SESSION->getStorageMode())
        {
        case SM_AS_DATASET:
            return Wrappers::StorageMode::StorageModeAsDataSet;
        case SM_AS_MEDIA:
            return Wrappers::StorageMode::StorageModeAsMedia;
        case SM_AS_MEDIA_ONLY:
            return Wrappers::StorageMode::StorageModeAsMediaOnly;
        case SM_NO_STORAGE:
            return Wrappers::StorageMode::StorageModeNoStorage;
        case SM_TEMPORARY_PIXEL_ONLY:
            return Wrappers::StorageMode::StorageModeTemporaryPixelOnly;
        default:
            // Unknown Wrappers::StorageMode
            throw gcnew System::NotImplementedException();
        }
    }
    void MBaseSession::StorageMode::set(Wrappers::StorageMode value)
    {
        STORAGE_MODE_ENUM storageMode;
        switch (value)
        {
        case Wrappers::StorageMode::StorageModeAsDataSet:
            storageMode = SM_AS_DATASET;
            break;
        case Wrappers::StorageMode::StorageModeAsMedia:
            storageMode = SM_AS_MEDIA;
            break;
        case Wrappers::StorageMode::StorageModeAsMediaOnly:
            storageMode = SM_AS_MEDIA_ONLY;
            break;
        case Wrappers::StorageMode::StorageModeNoStorage:
            storageMode = SM_NO_STORAGE;
            break;
        case Wrappers::StorageMode::StorageModeTemporaryPixelOnly:
            storageMode = SM_TEMPORARY_PIXEL_ONLY;
            break;
        default:
            // Unknown Wrappers::StorageMode
            throw gcnew System::NotImplementedException();
            break;
        }
        return m_pBASE_SESSION->setStorageMode(storageMode);
    }

    bool MBaseSession::LogScpThread::get()
    {
        return m_pBASE_SESSION->getLogScpThread();
    }
    void MBaseSession::LogScpThread::set(bool value)
    {
        return m_pBASE_SESSION->setLogScpThread(value);
    }

    void MBaseSession::DeleteSupportedTransferSyntaxes()
    {
        return m_pBASE_SESSION->deleteSupportedTransferSyntaxes();
    }

    void MBaseSession::AddSupportedTransferSyntax(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->addSupportedTransferSyntax(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt16 MBaseSession::NrOfSupportedTransferSyntaxes::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->noSupportedTransferSyntaxes();
    }

    System::String^ MBaseSession::SupportedTransferSyntax::get(System::UInt16 idx)
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSupportedTransferSyntax(idx));
        return clistr; 
        //return m_pBASE_SESSION->getSupportedTransferSyntax(idx);
    }

    System::String^ MBaseSession::DicomScriptRootDirectory::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDicomScriptRoot());
        return clistr; 
        //return m_pBASE_SESSION->getDicomScriptRoot();
    }
    void MBaseSession::DicomScriptRootDirectory::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDicomScriptRoot(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
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

    bool MBaseSession::ExecuteScript(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->executeScript(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }
    bool MBaseSession::ParseScript(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->parseScript(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }

    void MBaseSession::ScpEmulatorType::set(Wrappers::ScpEmulatorType value)
    {
        SCP_EMULATOR_ENUM scpEmulatorType;
        switch (value)
        {
        case Wrappers::ScpEmulatorType::ScpEmulatorTypePrint:
            scpEmulatorType = SCP_EMULATOR_PRINT;
            break;
        case Wrappers::ScpEmulatorType::ScpEmulatorTypeStorage:
            scpEmulatorType = SCP_EMULATOR_STORAGE;
            break;
		case Wrappers::ScpEmulatorType::ScpEmulatorTypeStorageCommit:
            scpEmulatorType = SCP_EMULATOR_STORAGE_COMMIT;
            break;
        case Wrappers::ScpEmulatorType::ScpEmulatorTypeMpps:
            scpEmulatorType = SCP_EMULATOR_MPPS;
            break;
        case Wrappers::ScpEmulatorType::ScpEmulatorTypeWorklist:
            scpEmulatorType = SCP_EMULATOR_WORKLIST;
            break;
        case Wrappers::ScpEmulatorType::ScpEmulatorTypeQueryRetrieve:
            scpEmulatorType = SCP_EMULATOR_QUERY_RETRIEVE;
            break;
		case Wrappers::ScpEmulatorType::ScpEmulatorTypeUnknown:
			scpEmulatorType = SCP_EMULATOR_UNKNOWN;
            break;
        default:
            System::Diagnostics::Trace::Assert(false);
            scpEmulatorType = SCP_EMULATOR_UNKNOWN;
            break;
        }
        return m_pBASE_SESSION->setScpEmulatorType(scpEmulatorType);
    }

    Wrappers::ScpEmulatorType MBaseSession::ScpEmulatorType::get()
    {
        switch (m_pBASE_SESSION->getScpEmulatorType())
        {
		case SCP_EMULATOR_PRINT:
            return Wrappers::ScpEmulatorType::ScpEmulatorTypePrint;
		case SCP_EMULATOR_STORAGE:
            return Wrappers::ScpEmulatorType::ScpEmulatorTypeStorage;
		case SCP_EMULATOR_STORAGE_COMMIT:
            return Wrappers::ScpEmulatorType::ScpEmulatorTypeStorageCommit;
		case SCP_EMULATOR_MPPS:
            return Wrappers::ScpEmulatorType::ScpEmulatorTypeMpps;
		case SCP_EMULATOR_WORKLIST:
            return Wrappers::ScpEmulatorType::ScpEmulatorTypeWorklist;
		case SCP_EMULATOR_QUERY_RETRIEVE:
            return Wrappers::ScpEmulatorType::ScpEmulatorTypeQueryRetrieve;
		case SCP_EMULATOR_UNKNOWN:
            return Wrappers::ScpEmulatorType::ScpEmulatorTypeUnknown;
        default:
            // Unknown Wrappers::SessionType
            throw gcnew System::NotImplementedException();
        }
    }

    void MBaseSession::ScuEmulatorType::set(Wrappers::ScuEmulatorType value)
    {
        SCU_EMULATOR_ENUM scuEmulatorType;
        switch (value)
        {
        case Wrappers::ScuEmulatorType::ScuEmulatorTypeStorage:
            scuEmulatorType = SCU_EMULATOR_STORAGE;
            break;
		case Wrappers::ScuEmulatorType::ScuEmulatorTypeUnknown:
			scuEmulatorType = SCU_EMULATOR_UNKNOWN;
            break;
        default:
            System::Diagnostics::Trace::Assert(false);
            scuEmulatorType = SCU_EMULATOR_UNKNOWN;
            break;
        }
        return m_pBASE_SESSION->setScuEmulatorType(scuEmulatorType);
    }

    Wrappers::ScuEmulatorType MBaseSession::ScuEmulatorType::get()
    {
        switch (m_pBASE_SESSION->getScuEmulatorType())
        {
		case SCU_EMULATOR_STORAGE:
            return Wrappers::ScuEmulatorType::ScuEmulatorTypeStorage;
		case SCU_EMULATOR_UNKNOWN:
            return Wrappers::ScuEmulatorType::ScuEmulatorTypeUnknown;
        default:
            // Unknown Wrappers::SessionType
            throw gcnew System::NotImplementedException();
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

    bool MBaseSession::ValidateMediaFile(System::String^ filename)
    {
        char* pAnsiFilenameString = (char*)(void*)Marshal::StringToHGlobalAnsi(filename);
        bool retVal = m_pBASE_SESSION->validateMediaFile(pAnsiFilenameString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiFilenameString));
        //Marshal::FreeHGlobal(pAnsiFilenameString);
        return retVal;
    }

    bool MBaseSession::ValidateMediaFile(System::String^ filename,
            Wrappers::FileContentType fileContentType,
            System::String^ sopClassUid, 
            System::String^ sopInstanceUid, 
            System::String^ transferSyntaxUid)
	{
        char* pAnsiFilenameString = (char*)(void*)Marshal::StringToHGlobalAnsi(filename);
        char* pAnsiSopClassUid = (char*)(void*)Marshal::StringToHGlobalAnsi(sopClassUid);
        char* pAnsiSopInstanceUid = (char*)(void*)Marshal::StringToHGlobalAnsi(sopInstanceUid);
        char* pAnsiTransferSyntaxUid = (char*)(void*)Marshal::StringToHGlobalAnsi(transferSyntaxUid);

		MEDIA_FILE_CONTENT_TYPE_ENUM mcf_type = MFC_MEDIAFILE;
        switch (fileContentType)
        {		
        case Wrappers::FileContentType::FileContentTypeMediaFile:
            mcf_type = MFC_MEDIAFILE;
            break;
		case Wrappers::FileContentType::FileContentTypeCommandSet:
			mcf_type = MFC_COMMANDSET;
            break;
		case Wrappers::FileContentType::FileContentTypeDataSet:
			mcf_type = MFC_DATASET;
			break;
        default:
            System::Diagnostics::Trace::Assert(false);
            break;
        }

        bool retVal = m_pBASE_SESSION->validateMediaFile(pAnsiFilenameString, mcf_type, pAnsiSopClassUid, pAnsiSopInstanceUid, pAnsiTransferSyntaxUid);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiFilenameString));
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiSopClassUid));
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiSopInstanceUid));
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiTransferSyntaxUid));
        /*Marshal::FreeHGlobal(pAnsiFilenameString);
        Marshal::FreeHGlobal(pAnsiSopClassUid);
        Marshal::FreeHGlobal(pAnsiSopInstanceUid);
        Marshal::FreeHGlobal(pAnsiTransferSyntaxUid);*/
        return retVal;
	}

    bool MBaseSession::ValidateMediaFile(System::String^ filename, System::String^ recordsToFilter, int numberRecordsToFilter)
    {
        char* pAnsiFilenameString = (char*)(void*)Marshal::StringToHGlobalAnsi(filename);
        char* pAnsiRecordsToFilterString = (char*)(void*)Marshal::StringToHGlobalAnsi(recordsToFilter);
        bool retVal = m_pBASE_SESSION->validateMediaFile(pAnsiFilenameString, pAnsiRecordsToFilterString, numberRecordsToFilter);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiFilenameString));
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiRecordsToFilterString));
        /*Marshal::FreeHGlobal(pAnsiFilenameString);
        Marshal::FreeHGlobal(pAnsiRecordsToFilterString);*/
        return retVal;
    }

    bool MBaseSession::EndMediaValidation(void)
    {
        return m_pBASE_SESSION->endMediaValidation();
    }

    System::UInt32 MBaseSession::InstanceId::get()
    {
        return m_pBASE_SESSION->getInstanceId();
    }
    void MBaseSession::InstanceId::set(System::UInt32 value)
    {
        return m_pBASE_SESSION->setInstanceId(value);
    }

    System::String^ MBaseSession::SessionDirectory::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSessionDirectory());
        return clistr; 
        //return m_pBASE_SESSION->getSessionDirectory();
    }

    System::String^ MBaseSession::SessionFileName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getFilename().c_str());
        return clistr; 
        //return m_pBASE_SESSION->getFilename().c_str();
    }

    void MBaseSession::SessionFileName::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setFileName(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    void MBaseSession::SessionDirectory::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSessionDirectory(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    bool MBaseSession::AppendToResultsFile::get()
    {
        return m_pBASE_SESSION->getAppendToResultsFile();
    }
    void MBaseSession::AppendToResultsFile::set(bool value)
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

    System::UInt16 MBaseSession::Counter::get()
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

    Wrappers::SessionType MBaseSession::SessionType::get()
    {
        switch (m_pBASE_SESSION->getSessionType())
        {
        case ST_UNKNOWN:
            return Wrappers::SessionType::SessionTypeUnknown;
        case ST_SCRIPT:
            return Wrappers::SessionType::SessionTypeScript;
        case ST_EMULATOR:
            return Wrappers::SessionType::SessionTypeEmulator;
        case ST_MEDIA:
            return Wrappers::SessionType::SessionTypeMedia;
		case ST_SNIFFER:
            return Wrappers::SessionType::SessionTypeSniffer;
        default:
            // Unknown Wrappers::SessionType
            throw gcnew System::NotImplementedException();
        }
    }
    void MBaseSession::SessionType::set(Wrappers::SessionType value)
    {
        SESSION_TYPE_ENUM sessionType;
        switch (value)
        {
        case Wrappers::SessionType::SessionTypeUnknown:
            sessionType = ST_UNKNOWN;
            break;
        case Wrappers::SessionType::SessionTypeScript:
            sessionType = ST_SCRIPT;
            break;
        case Wrappers::SessionType::SessionTypeEmulator:
            sessionType = ST_EMULATOR;
            break;
        case Wrappers::SessionType::SessionTypeMedia:
            sessionType = ST_MEDIA;
            break;
		case Wrappers::SessionType::SessionTypeSniffer:
            sessionType = ST_SNIFFER;
            break;
        default:
            // Unknown Wrappers::SessionType
            throw gcnew System::NotImplementedException();
        }
        return m_pBASE_SESSION->setSessionType(sessionType);
    }

    Wrappers::SessionType MBaseSession::RunTimeSessionType::get()
    {
        switch (m_pBASE_SESSION->getRuntimeSessionType())
        {
        case ST_UNKNOWN:
            return Wrappers::SessionType::SessionTypeUnknown;
        case ST_SCRIPT:
            return Wrappers::SessionType::SessionTypeScript;
        case ST_EMULATOR:
            return Wrappers::SessionType::SessionTypeEmulator;
        case ST_MEDIA:
            return Wrappers::SessionType::SessionTypeMedia;
		case ST_SNIFFER:
            return Wrappers::SessionType::SessionTypeSniffer;
        default:
            // Unknown Wrappers::SessionType
            throw gcnew System::NotImplementedException();
        }
    }

    System::String^ MBaseSession::SessionTitle::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSessionTitle());
        return clistr;
        // return m_pBASE_SESSION->getSessionTitle();
    }
    void MBaseSession::SessionTitle::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSessionTitle(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt16 MBaseSession::SessionId::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->getSessionId();
    }
    void MBaseSession::SessionId::set(System::UInt16 value)
    {
        return m_pBASE_SESSION->setSessionId(value);
    }

    System::String^ MBaseSession::Manufacturer::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getManufacturer());
        return clistr; 
        //return m_pBASE_SESSION->getManufacturer();
    }
    void MBaseSession::Manufacturer::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setManufacturer(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::ModelName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getModelName());
        return clistr; 
        //return m_pBASE_SESSION->getModelName();
    }
    void MBaseSession::ModelName::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setModelName(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::SoftwareVersions::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getSoftwareVersions());
        return clistr; 
        //return m_pBASE_SESSION->getSoftwareVersions();
    }
    void MBaseSession::SoftwareVersions::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setSoftwareVersions(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::ApplicationEntityName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getApplicationEntityName());
        return clistr; 
        //return m_pBASE_SESSION->getApplicationEntityName();
    }
    void MBaseSession::ApplicationEntityName::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setApplicationEntityName(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::ApplicationEntityVersion::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getApplicationEntityVersion());
        return clistr; 
        //return m_pBASE_SESSION->getApplicationEntityVersion();
    }
    void MBaseSession::ApplicationEntityVersion::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setApplicationEntityVersion(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::TestedBy::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getTestedBy());
        return clistr; 
        //return m_pBASE_SESSION->getTestedBy();
    }
    void MBaseSession::TestedBy::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setTestedBy(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::Date::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDate());
        return clistr; 
        //return m_pBASE_SESSION->getDate();
    }
    void MBaseSession::Date::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDate(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt32 MBaseSession::LogMask::get()
    {
        return m_pBASE_SESSION->getLogMask();
    }

    System::String^ MBaseSession::DefinitionFileRootDirectory::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDefinitionFileRoot());
        return clistr; 
        //return m_pBASE_SESSION->getDefinitionFileRoot();
    }

    void MBaseSession::DefinitionFileRootDirectory::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDefinitionFileRoot(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    void MBaseSession::AddDefinitionFileDirectory(System::String^ pDefinitionFileDirectory)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(pDefinitionFileDirectory);
        m_pBASE_SESSION->addDefinitionDirectory(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::UInt16 MBaseSession::NrOfDefinitionFileDirectories::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->noDefinitionDirectories();
    }
    
    System::String^ MBaseSession::DefinitionFileDirectory::get(System::UInt16 index)
    {
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDefinitionDirectory(index).c_str());
        return clistr; 
        //return m_pBASE_SESSION->getDefinitionDirectory(index).c_str();
    }
    
    void MBaseSession::RemoveAllDefinitionFileDirectories()
    {
        m_pBASE_SESSION->removeAllDefinitionDirectories();
    }

    System::UInt16 MBaseSession::NrOfDefinitionFiles::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pBASE_SESSION->noDefinitionFiles();
    }

    System::String^ MBaseSession::DefinitionFileName::get(System::UInt16 index)
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDefinitionFilename(index));
        return clistr; 
        //return m_pBASE_SESSION->getDefinitionFilename(index);
    }

    System::String^ MBaseSession::DataDirectory::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDataDirectory());
        return clistr; 
        //return m_pBASE_SESSION->getDataDirectory();
    }

    void MBaseSession::DataDirectory::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDataDirectory(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::ResultsRootDirectory::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getResultsRoot());
        return clistr; 
        //return m_pBASE_SESSION->getResultsRoot();
    }

    void MBaseSession::ResultsRootDirectory::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setResultsRoot(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::AbsolutePixelPathName::get(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        const char* retVal = m_pBASE_SESSION->getAbsolutePixelPathname(pAnsiString).c_str();
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(retVal);
        return clistr; 
        //return retVal;
    }

    bool MBaseSession::IsOpen::get()
    {
        return m_pBASE_SESSION->isOpen();
    }

	bool MBaseSession::IsAssociated::get()
    {
        return m_pBASE_SESSION->getIsAssociated();
    }	

    void MBaseSession::Load(
        System::String^ sessionFileName,
        [In, Out] bool definitionFileLoaded,
        bool andBeginIt)
    {
        string std_string;
        Wrappers::MarshalString (sessionFileName, std_string);
        bool retVal = m_pBASE_SESSION->load(std_string, definitionFileLoaded, andBeginIt);
        if (!retVal) 
            throw gcnew System::ApplicationException(
            System::String::Format("Could not load session {0}.", sessionFileName)
            );
    }

    bool MBaseSession::Save()
    {
        return m_pBASE_SESSION->save();
    }

    DvtkData::Media::DicomFile^ MBaseSession::ReadMedia(
        System::String^ pFileName)
    {
        DvtkData::Media::DicomFile^ pDicomFile;
        char* pAnsiStringFileName = (char*)(void*)Marshal::StringToHGlobalAnsi(pFileName);

		// read media - flags use session storage mode = true and log it = true
        FILE_DATASET_CLASS* pFileDataSet = m_pBASE_SESSION->readMedia(pAnsiStringFileName, MFC_UNKNOWN, "", "", "", true, true);
        pDicomFile = 
            ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pFileDataSet);
        delete pFileDataSet;
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiStringFileName));
        //Marshal::FreeHGlobal(pAnsiStringFileName);
        return pDicomFile;
    }

    System::Boolean MBaseSession::WriteMedia(
        DvtkData::Media::DicomFile^ pDicomFile, System::String^ pFileName)
    {
        bool success = false;
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDicomFile, pFileName);
        success = m_pBASE_SESSION->writeMedia(fileDataset_ptr);
        delete fileDataset_ptr;
        return success;
    }

	DvtkData::Media::DicomDir^ MBaseSession::ReadDicomdir(
        System::String^ pFileName)
    {
        DvtkData::Media::DicomDir^ pDicomFile;
        char* pAnsiStringFileName = (char*)(void*)Marshal::StringToHGlobalAnsi(pFileName);

		// read media - flags use session storage mode = true and log it = true
        FILE_DATASET_CLASS* pFileDataSet = m_pBASE_SESSION->readDicomdir(pAnsiStringFileName, true, true);
        pDicomFile = 
            ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::ConvertToDicomdir(pFileDataSet);
        delete pFileDataSet;
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiStringFileName));
        //Marshal::FreeHGlobal(pAnsiStringFileName);
        return pDicomFile;
    }

    System::Boolean MBaseSession::WriteDicomdir(
        DvtkData::Media::DicomDir^ pDicomFile, System::String^ pFileName)
    {
        bool success = false;
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDicomFile, pFileName);
        success = m_pBASE_SESSION->writeDicomdir(fileDataset_ptr);
        delete fileDataset_ptr;
        return success;
    }

    bool MBaseSession::LoadDefinitionFile(
        System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->loadDefinition(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }

    bool MBaseSession::UnLoadDefinitionFile(
        System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        bool retVal = m_pBASE_SESSION->unloadDefinition(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return retVal;
    }

    System::String^ MBaseSession::DescriptionDirectory::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(m_pBASE_SESSION->getDescriptionDirectory().c_str());
        return clistr; 
        //return m_pBASE_SESSION->getDescriptionDirectory().c_str();
    }

    void MBaseSession::DescriptionDirectory::set(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pBASE_SESSION->setDescriptionDirectory(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        return;
    }

    System::String^ MBaseSession::SopUid::get(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        string umString = m_pBASE_SESSION->getSopUid(pAnsiString);
        const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(retVal);
        return clistr; 
        //return retVal;
    }

    System::String^ MBaseSession::IodName::get(System::String^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        string umString = m_pBASE_SESSION->getIodName(pAnsiString);
        const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(retVal);
        return clistr; 
        //return retVal;
    }

	System::String^ MBaseSession::IodNameFromDefinition::get(DvtkData::Dimse::DimseCommand command, System::String^ uid)
	{
		DIMSE_CMD_ENUM cmdEnum = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(command);
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(uid);
		string umString = m_pBASE_SESSION->getIodNameFromDefinition(cmdEnum, pAnsiString);
		const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(retVal);
        return clistr;
        //return retVal;
	}

	System::String^ MBaseSession::FileNameFromSOPUID::get(DvtkData::Dimse::DimseCommand command, System::String^ uid)
	{
		DIMSE_CMD_ENUM cmdEnum = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(command);
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(uid);
		string umString = m_pBASE_SESSION->getFileNameFromSOPUID(cmdEnum, pAnsiString);
		const char* retVal = umString.c_str();
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(retVal);
        return clistr;
        //return retVal;
	}

	System::String^ MBaseSession::AttributeNameFromDefinition::get(DvtkData::Dimse::Tag^ pTag)
	{
		System::String ^ pAttributeName = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::GetAttributeName(pTag);
        return pAttributeName;
	}

	DvtkData::Dimse::VR MBaseSession::AttributeVrFromDefinition::get(DvtkData::Dimse::Tag^ pTag)
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
    
    bool MBaseSession::LogLevelEnabled::get(System::UInt32 logLevel)
    {
        return m_pBASE_SESSION->isLogLevel(logLevel);
    }
    void MBaseSession::LogLevelEnabled::set(System::UInt32 logLevel, bool enabled)
    {
        return m_pBASE_SESSION->setLogLevel(enabled, logLevel);
    }

    bool MBaseSession::IsStopped::get()
    {
        return m_pBASE_SESSION->isSessionStopped();
    }

    void MBaseSession::AddDicomScript(System::String^ value)
    {
        // only store this information in a Scripting session
        if (m_pBASE_SESSION->getRuntimeSessionType() == ST_SCRIPT)
        {
            char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
            // should check that file extension is OK
            string dicomScript = pAnsiString;
            DICOM_SCRIPT_CLASS *dicomScript_ptr = new DICOM_SCRIPT_CLASS(reinterpret_cast<SCRIPT_SESSION_CLASS*>(m_pBASE_SESSION), dicomScript);
            m_pBASE_SESSION->addDicomScript(dicomScript_ptr);
            Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
            //Marshal::FreeHGlobal(pAnsiString);
        }
        return;
    }

    void MBaseSession::Rules::set(System::Uri^ pRulesUri)
    {
        if (this->m_pSerializationAdapter == nullptr) return;
        if (
            pRulesUri->Equals(MBaseSession::RuleUriStandardRules) ||
            pRulesUri->Equals(MBaseSession::RuleUriStrictRules)
            ) 
        {
            this->m_pSerializationAdapter->set_Rules(pRulesUri);
        }
        else
        {
            throw gcnew System::ArgumentException();
        }
        return;
    }
}