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

#pragma once
#include "ISessions.h"
#include "ISockets.h"
#include "SerializationAdapter.h"
#include "ActivityReportingAdapter.h"
#include "CountingAdapter.h"
#include <vcclr.h>
#define LogMask System::UInt32

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    [System::Flags]
    public __value enum ValidationControlFlags
    {
        None                    = 0,
        UseValueRepresentations = 1<<0,
        UseDefinitions          = 1<<1,
        UseReferences           = 1<<2,
        All                     = (UseValueRepresentations|UseDefinitions|UseReferences),
    };

    inline VALIDATION_CONTROL_FLAG_ENUM _Convert(Wrappers::ValidationControlFlags value)
    {
        int retValue = ::NONE;
        if ((value & Wrappers::ValidationControlFlags::UseValueRepresentations) != 0) retValue |= ::USE_VR;
        if ((value & Wrappers::ValidationControlFlags::UseDefinitions) != 0) retValue |= ::USE_DEFINITION;
        if ((value & Wrappers::ValidationControlFlags::UseReferences) != 0) retValue |= ::USE_REFERENCE;
        return (VALIDATION_CONTROL_FLAG_ENUM)retValue;
    };

    public __value enum SessionType
    {
        SessionTypeUnknown = 0,
        SessionTypeScript,
        SessionTypeEmulator,
        SessionTypeMedia,
		SessionTypeSniffer,
    };

    public __value enum ScpEmulatorType
    {
        ScpEmulatorTypeStorage,
		ScpEmulatorTypeStorageCommit,
        ScpEmulatorTypePrint,
		ScpEmulatorTypeMpps,
		ScpEmulatorTypeWorklist,
		ScpEmulatorTypeQueryRetrieve,
        ScpEmulatorTypeUnknown,
    };

    public __value enum ScuEmulatorType
    {
        ScuEmulatorTypeStorage,
        ScuEmulatorTypeUnknown,
    };

    public __value enum FileContentType
    {
        FileContentTypeMediaFile,
        FileContentTypeCommandSet,
		FileContentTypeDataSet
    };

    // <summary>
    // USE WITH EXTREME CARE!
    // This internal abstract class (Singleton) is introduced as workaround for
    // <p>
    // BUG: AppDomainUnloadedException is thrown when you call a virtual 
    // destructor on a __nogc class during an AppDomain unload
    // See http://support.microsoft.com/default.aspx?scid=kb;EN-US;837317
    // </p>
    // <p>
    // BUG: AppDomainUnloaded Exception When You Use Managed Extensions for C++ Components
    // See http://support.microsoft.com/default.aspx?kbid=309694
    // </p>
    // This is not a workaround suggested by MS.
    // All their suggested workarounds failed to remove the bug.
    // </summary>
    // <remarks>
    // <p>
    // Be sure to remove a disposable object from this static list if you
    // wish that the GC frees it. This list will extend the life-time of the
    // object if misused.
    // </p>
    // <p>
    // Dvtk.Session.ScriptSession<>->Wrappers.MScriptSession<>->SCRIPT_SESSION_CLASS
    // Remove (IDisposable)Wrappers.MScriptSession as soon as
    // ~Dvtk.Session.ScriptSession.dtor called.
    // This avoids extending the life-line of the Wrappers.MScriptSession object.
    // </p>
    // </remarks>
    public __gc __abstract class MDisposableResources
    {
    public:
        // <summary>
        // Manage a list of disposable resources.
        // </summary>
        static MDisposableResources()
        {
            pDisposables = new System::Collections::ArrayList();
        }
    public:
        // <summary>
        // Add a disposable resource to the list.
        // </summary>
        static void AddDisposable(System::IDisposable __gc* pDisposable)
        {
            pDisposables->Add(pDisposable);
        }
        // <summary>
        // Remove a disposable resource to the list.
        // </summary>
        static void RemoveDisposable(System::IDisposable __gc* pDisposable)
        {
            pDisposables->Remove(pDisposable);
        }
    public:
        // <summary>
        // Dispose all resources in the list and clear the list.
        // </summary>
        static void Dispose()
        {
            for (int i = 0; i < pDisposables->Count; i++)
            {
                System::Object __gc* item = pDisposables->get_Item(i);
                static_cast<System::IDisposable __gc*>(item)->Dispose();
            }
            pDisposables->Clear();
        }
    private:
        static System::Collections::ArrayList __gc* pDisposables;
    };

    public __gc __abstract class MBaseSession 
        : public ISession
    {
    private protected:
        Wrappers::ActivityReportingAdapter __nogc* m_pActivityReportingAdapter;
        Wrappers::SerializationAdapter __nogc* m_pSerializationAdapter;
		Wrappers::CountingAdapter __nogc* m_pCountingAdapter;

    public:
        static MBaseSession()
        {
            MBaseSession::_StaticConstructor();
        }

    private:
        static void _StaticConstructor();

    public:
        MBaseSession(void);
        virtual ~MBaseSession(void);

    protected:
        /*virtual*/
        void Initialize(void);

    private protected:
        // <summary>
        // Helper method to get underlying BASE_SESSION_CLASS pointer.
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual BASE_SESSION_CLASS __nogc* get_m_pBASE_SESSION() = 0;

    public:
        // <summary>
        // Top ISerializationTarget and ICountingTarget and IActivityReportingTarget
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        void InitTopSerializationAndCountingAndActivityReportingTargets(
			ISerializationTarget __gc* serializationTarget,
			ICountingTarget __gc* countingTarget,
			IActivityReportingTarget __gc* activityReportingTarget);

	public:
		void StopSerializationProcessing();

    private public:
        // <summary>
        // SessionFileVersion
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_SessionFileVersion(System::UInt16 value);

    public:
        // <summary>
        // SutRole
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual Wrappers::SutRole get_SutRole();
    public:
        // <summary>
        // SutRole
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SutRole(Wrappers::SutRole value);

    public:
        // <summary>
        // DvtAeTitle
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_DvtAeTitle();
    public:
        // <summary>
        // DvtAeTitle
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DvtAeTitle(System::String __gc* value);

    public:
        // <summary>
        // SutAeTitle
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_SutAeTitle();
    public:
        // <summary>
        // SutAeTitle
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SutAeTitle(System::String __gc* value);

    public:
        // <summary>
        // SutHostname
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_SutHostname();
    public:
        // <summary>
        // SutHostname
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SutHostname(System::String __gc* value);

    public:
        // <summary>
        // Dvt MaximumLengthReceived
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt32 get_DvtMaximumLengthReceived();
    public:
        // <summary>
        // Dvt MaximumLengthReceived
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DvtMaximumLengthReceived(System::UInt32 value);

    public:
        // <summary>
        // Sut MaximumLengthReceived
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt32 get_SutMaximumLengthReceived();
    public:
        // <summary>
        // Sut MaximumLengthReceived
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SutMaximumLengthReceived(System::UInt32 value);

    public:
        // <summary>
        // Dvt ImplementationClassUid
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_DvtImplementationClassUid();
    public:
        // <summary>
        // Dvt ImplementationClassUid
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DvtImplementationClassUid(System::String __gc* value);

    public:
        // <summary>
        // Sut ImplementationClassUid
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_SutImplementationClassUid();
    public:
        // <summary>
        // Sut ImplementationClassUid
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SutImplementationClassUid(System::String __gc* value);

    public:
        // <summary>
        // Dvt ImplementationVersionName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_DvtImplementationVersionName();
    public:
        // <summary>
        // Dvt ImplementationVersionName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DvtImplementationVersionName(System::String __gc* value);

    public:
        // <summary>
        // SutImplementationVersionName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_SutImplementationVersionName();
    public:
        // <summary>
        // SutImplementationVersionName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SutImplementationVersionName(System::String __gc* value);

    public:
        // <summary>
        // SutPort
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt16 get_SutPort();
    public:
        // <summary>
        // SutPort
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SutPort(System::UInt16 value);

    public:
        // <summary>
        // DvtPort
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt16 get_DvtPort();
    public:
        // <summary>
        // DvtPort
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DvtPort(System::UInt16 value);

    public:
        // <summary>
        // DvtSocketTimeOut
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt16 get_DvtSocketTimeOut();
    public:
        // <summary>
        // DvtSocketTimeOut
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DvtSocketTimeOut(System::UInt16 value);

    public:
        // <summary>
        // UseSecureSockets
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_UseSecureSockets();
    public:
        // <summary>
        // UseSecureSockets
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_UseSecureSockets(bool value);

    public:
        // <summary>
        // TlsPassword
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_TlsPassword();
    public:
        // <summary>
        // TlsPassword
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_TlsPassword(System::String __gc* value);

        // TODO: Determine whether isTlsPasswordValid should be exposed.
        //bool isTlsPasswordValid(bool& unencryptedKeyFound) 
        //{ 
        //	return socketParametersM.isTlsPasswordValid(unencryptedKeyFound); 
        //}

    public:
        // <summary>
        // TlsVersion
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_TlsVersion();
    public:
        // <summary>
        // TlsVersion
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_TlsVersion(System::String __gc* value);

    public:
        // <summary>
        // CheckRemoteCertificate
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_CheckRemoteCertificate();
    public:
        // <summary>
        // CheckRemoteCertificate
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_CheckRemoteCertificate(bool value);

    public:
        // <summary>
        // CipherList
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_CipherList();
    public:
        // <summary>
        // CipherList
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_CipherList(System::String __gc* value);

    public:
        bool IsValidCipherList(System::String __gc* value);

    public:
        // <summary>
        // CacheTlsSessions
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_CacheTlsSessions();
    public:
        // <summary>
        // CacheTlsSessions
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_CacheTlsSessions(bool value);

    public:
        // <summary>
        // TlsCacheTimeout
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt16 get_TlsCacheTimeout();
    public:
        // <summary>
        // TlsCacheTimeout
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_TlsCacheTimeout(System::UInt16 value);

    public:
        // <summary>
        // CredentialsFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_CredentialsFileName();
    public:
        // <summary>
        // CredentialsFileName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_CredentialsFileName(System::String __gc* value);

    public:
        // <summary>
        // CertificateFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_CertificateFileName();
    public:
        // <summary>
        // CertificateFileName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_CertificateFileName(System::String __gc* value);

    public:
        // <summary>
        // SocketParametersChanged
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SocketParametersChanged(bool value);

    public:
        // <summary>
        // AutoType2Attributes
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_AutoType2Attributes();
    public:
        // <summary>
        // AutoType2Attributes
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_AutoType2Attributes(bool value);

    public:
        // <summary>
        // AutoCreateDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_AutoCreateDirectory();
    public:
        // <summary>
        // AutoCreateDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_AutoCreateDirectory(bool value);

    public:
        // <summary>
        // DefineSqLength
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_DefineSqLength();
    public:
        // <summary>
        // DefineSqLength
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DefineSqLength(bool value);

    public:
        // <summary>
        // AddGroupLength
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_AddGroupLength();
    public:
        // <summary>
        // AddGroupLength
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_AddGroupLength(bool value);

    public:
        // <summary>
        // StrictValidation
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_StrictValidation();
    public:
        // <summary>
        // StrictValidation
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_StrictValidation(bool value);
		
    public:
        // <summary>
        // TestLogValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_TestLogValidationResults();
    public:
        // <summary>
        // TestLogValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_TestLogValidationResults(bool value);

    public:
        // <summary>
        // DetailedValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_DetailedValidationResults();
    public:
        // <summary>
        // DetailedValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DetailedValidationResults(bool value);

		    public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_SummaryValidationResults();
    public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SummaryValidationResults(bool value);
		
	public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property static bool get_UsePrivateAttributeMapping();
    public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property static void set_UsePrivateAttributeMapping(bool value);

		    public:
        // <summary>
        // IncludeType3NotPresentInResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_IncludeType3NotPresentInResults();
    public:
        // <summary>
        // IncludeType3NotPresentInResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_IncludeType3NotPresentInResults(bool value);

    public:
        // <summary>
        // ContinueOnError
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_ContinueOnError();
    public:
        // <summary>
        // ContinueOnError
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_ContinueOnError(bool value);

    public:
        // <summary>
        // ValidateReferencedFile
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_ValidateReferencedFile();
    public:
        // <summary>
        // ValidateReferencedFile
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_ValidateReferencedFile(bool value);

    public:
        // <summary>
        // UnVrDefinitionLookUp
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_UnVrDefinitionLookUp();
    public:
        // <summary>
        // UnVrDefinitionLookUp
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_UnVrDefinitionLookUp(bool value);

	public:
        // <summary>
        // DumpAttributesOfReferencedFile
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_DumpAttributesOfRefFiles();
    public:
        // <summary>
        // DumpAttributesOfReferencedFile
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DumpAttributesOfRefFiles(bool value);

    public:
        // <summary>
        // StorageMode
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual Wrappers::StorageMode get_StorageMode();
    public:
        // <summary>
        // StorageMode
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_StorageMode(Wrappers::StorageMode value);

    public:
        // <summary>
        // LogScpThread
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_LogScpThread();
    public:
        // <summary>
        // LogScpThread
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_LogScpThread(bool value);

	public:
        // <summary>
        // DelayBeforeNEventReport
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt16 get_DelayBeforeNEventReport();
    public:
        // <summary>
        // DelayBeforeNEventReport
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DelayBeforeNEventReport(System::UInt16 value);
	public:
        // <summary>
        // AcceptDuplicateImage
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_AcceptDuplicateImage(bool value);
	public:
        // <summary>
        // StoreCStoreReqOnly
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_StoreCStoreReqOnly(bool value);

    public:
        virtual void DeleteSupportedTransferSyntaxes();

    public:
        virtual void AddSupportedTransferSyntax(System::String __gc* value);

    public:
        // <summary>
        // No Of Supported Transfer Syntaxes
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt16 get_NrOfSupportedTransferSyntaxes();

    public:
        __property virtual System::String __gc* get_SupportedTransferSyntax(System::UInt16 index);

    public:
        // <summary>
        // DicomScriptRootDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_DicomScriptRootDirectory();
    public:
        // <summary>
        // DicomScriptRootDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_DicomScriptRootDirectory(System::String __gc* value);

    public:
        virtual bool Begin([In, Out] bool value);
    public:
        virtual void End(void);

    public:
        virtual bool ExecuteScript(System::String __gc* value);
    public:
        virtual bool ParseScript(System::String __gc* value);

    public:
		__property virtual void set_ScpEmulatorType(Wrappers::ScpEmulatorType value);

    public:
		__property virtual Wrappers::ScpEmulatorType get_ScpEmulatorType();

    public:
		__property virtual void set_ScuEmulatorType(Wrappers::ScuEmulatorType value);

    public:
		__property virtual Wrappers::ScuEmulatorType get_ScuEmulatorType();

    public:
        virtual bool EmulateSCP(void);

    public:
        virtual bool SendStatusEvent(void);

    public:
        virtual bool TerminateConnection(void);

	public:
        virtual bool AbortEmulation(void);

	public:
        virtual bool AbortEmulationFromSCU(void);

    public:
        virtual void ResetAssociation(void);

    public:
        virtual bool BeginMediaValidation(void);

    public:
        virtual bool ValidateMediaFile(System::String __gc* filename);

    public:
        virtual bool ValidateMediaFile(System::String __gc* filename,
            Wrappers::FileContentType fileContentType,
            System::String __gc* sopClassUid, 
            System::String __gc* sopInstanceUid, 
            System::String __gc* transferSyntaxUid);

	public:
		virtual bool ValidateMediaFile(System::String __gc* filename, System::String __gc* recordsToFilter, int numberRecordsToFilter);

    public:
        virtual bool EndMediaValidation(void);

    public:
        // <summary>
        // InstanceId
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::UInt32 get_InstanceId();
    public:
        // <summary>
        // InstanceId
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_InstanceId(System::UInt32 value);

    public:
        // <summary>
        // SessionDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_SessionDirectory();
    public:
        // <summary>
        // SessionDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_SessionDirectory(System::String __gc* value);

    public:
        // <summary>
        // SessionFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_SessionFileName();

    public:
        // <summary>
        // SessionFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property void set_SessionFileName(System::String __gc* value);

    public:
        // <summary>
        // AppendToResultsFile
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property bool get_AppendToResultsFile();
    public:
        // <summary>
        // AppendToResultsFile
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_AppendToResultsFile(bool value);

    public:
        void ResetCounter(void);
    public:
        void IncrementCounter(void);
    public:
        // <summary>
        // Counter
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::UInt16 get_Counter();

    public:
        bool ReloadDefinitions(void);
    public:
        void RemoveDefinitions(void);

    public:
        // <summary>
        // SessionType
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual Wrappers::SessionType get_SessionType();
    public:
        // <summary>
        // SessionType
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_SessionType(Wrappers::SessionType value);

    public:
        // <summary>
        // RunTimeSessionType
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual Wrappers::SessionType get_RunTimeSessionType();

    public:
        // <summary>
        // SessionTitle
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_SessionTitle();
    public:
        // <summary>
        // SessionTitle
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_SessionTitle(System::String __gc* value);

    public:
        // <summary>
        // SessionId
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::UInt16 get_SessionId();
    public:
        // <summary>
        // SessionId
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_SessionId(System::UInt16 value);

    public:
        // <summary>
        // Manufacturer
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_Manufacturer();
    public:
        // <summary>
        // Manufacturer
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_Manufacturer(System::String __gc* value);

    public:
        // <summary>
        // ModelName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_ModelName();
    public:
        // <summary>
        // ModelName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_ModelName(System::String __gc* value);

	public:
        // <summary>
        // SoftwareVersions
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_SoftwareVersions();
    public:
        // <summary>
        // SoftwareVersions
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_SoftwareVersions(System::String __gc* value);

    public:
        // <summary>
        // ApplicationEntityName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_ApplicationEntityName();
    public:
        // <summary>
        // ApplicationEntityName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_ApplicationEntityName(System::String __gc* value);

    public:
        // <summary>
        // ApplicationEntityVersion
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_ApplicationEntityVersion();
    public:
        // <summary>
        // ApplicationEntityVersion
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_ApplicationEntityVersion(System::String __gc* value);

    public:
        // <summary>
        // TestedBy
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_TestedBy();
    public:
        // <summary>
        // TestedBy
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_TestedBy(System::String __gc* value);

    public:
        // <summary>
        // Date
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_Date();
    public:
        // <summary>
        // Date
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_Date(System::String __gc* value);

    public:
        // <summary>
        // LogMask
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual LogMask get_LogMask();

    public:
        // <summary>
        // DefinitionFileRootDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_DefinitionFileRootDirectory();
    public:
        // <summary>
        // DefinitionFileRootDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DefinitionFileRootDirectory(System::String __gc* value);

    public:
        // <summary>
        // Add definition-file directory to definition-file search path.
        // </summary>
        // <remarks>
        // Should be existing file paths.
        // </remarks>
        void AddDefinitionFileDirectory(System::String __gc* pDefinitionFileDirectory);

    public:
        // <summary>
        // Get number of directories specified in the definition-file search path.
        // </summary>
        // <remarks>
        // Should be existing file paths.
        // </remarks>
        __property System::UInt16 get_NrOfDefinitionFileDirectories();

    public:
        // <summary>
        // Get the specified definition-file directory from the list of directories in the definition-file
        // search path.
        // </summary>
        // <remarks>
        // </remarks>
        __property System::String __gc* get_DefinitionFileDirectory(System::UInt16 index);

    public:
        // <summary>
        // Remove all definition-file directories from the list of directories in the definition-file
        // search path.
        // </summary>
        // <remarks>
        // </remarks>
        void RemoveAllDefinitionFileDirectories();

    public:
        // <summary>
        // No Of Definition Files
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::UInt16 get_NrOfDefinitionFiles();

    public:
        // <summary>
        // DefinitionFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_DefinitionFileName(System::UInt16 index);


    public:
        // <summary>
        // DataDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_DataDirectory();
    public:
        // <summary>
        // DataDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DataDirectory(System::String __gc* value);

    public:
        // <summary>
        // ResultsRootDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_ResultsRootDirectory();
    public:
        // <summary>
        // ResultsRootDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_ResultsRootDirectory(System::String __gc* value);

    public:
        __property System::String __gc* get_AbsolutePixelPathName(System::String __gc* value);

    public:
        // <summary>
        // IsOpen
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property bool get_IsOpen();

	public:
		__property bool get_IsAssociated();

    private public:
        void Load(
            System::String __gc* sessionFileName,
            [In, Out] bool definitionFileLoaded,
            bool andBeginIt);

    public:
        bool Save();

   	public:
        DvtkData::Media::DicomFile __gc* ReadMedia(
            System::String __gc* pFileName);
    public:
        System::Boolean WriteMedia(
            DvtkData::Media::DicomFile __gc* pDicomFile, System::String __gc* pFileName);

	public:
        DvtkData::Media::DicomDir __gc* ReadDicomdir(
            System::String __gc* pFileName);
    public:
        System::Boolean WriteDicomdir(
            DvtkData::Media::DicomDir __gc* pDicomFile, System::String __gc* pFileName);

    public:
        bool LoadDefinitionFile(System::String __gc* definitionFileName);

    public:
        bool UnLoadDefinitionFile(System::String __gc* definitionFileName);

    public:
        // <summary>
        // DescriptionDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual System::String __gc* get_DescriptionDirectory();
    public:
        // <summary>
        // DescriptionDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_DescriptionDirectory(System::String __gc* value);

    public:
        __property System::String __gc* get_SopUid(System::String __gc* value);

    public:
        __property System::String __gc* get_IodName(System::String __gc* value);

	public:
		__property System::String __gc* get_IodNameFromDefinition(DvtkData::Dimse::DimseCommand command, System::String __gc* uid);

	public:
		__property System::String __gc* get_FileNameFromSOPUID(DvtkData::Dimse::DimseCommand command, System::String __gc* uid);

	public:
		__property System::String __gc* get_AttributeNameFromDefinition(DvtkData::Dimse::Tag __gc* pTag);

	public:
		__property DvtkData::Dimse::VR get_AttributeVrFromDefinition(DvtkData::Dimse::Tag __gc* pTag);

    public:
        void EnableLogger(void);
    public:
        void DisableLogger(void);

    public:
        __property bool get_LogLevelEnabled(System::UInt32 logLevel);
    public:
        __property void set_LogLevelEnabled(System::UInt32 logLevel, bool enabled);
		
    public:
        // <summary>
        // IsStopped
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property bool get_IsStopped();

    public:
        virtual void AddDicomScript(System::String __gc* value);

    private:
        // <summary>
        // Rules used for validation results.
        // </summary>
        // <remarks>
        // Set Property
        // In this release, this is a pseudo URI.
        // Future variation point.
        // These resources are internal to the code.
        // No actual external resource is being used.
        // Supported internal URIs:
        // RuleUriStandardRules, RuleUriStructRules.
        // 
        // </remarks>
 public:    // MIGRATION_IN_PROGRESS 
        __property virtual void set_Rules(System::Uri __gc* pRulesUri);

    public:
        static System::Uri __gc* RuleUriStandardRules;
    public:
        static System::Uri __gc* RuleUriStrictRules;

    };
}