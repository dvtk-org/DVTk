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
//#define LogMask System::UInt32

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    [System::Flags]
    public enum class ValidationControlFlags
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
        if ((value & Wrappers::ValidationControlFlags::UseValueRepresentations) != Wrappers::ValidationControlFlags::None) retValue |= ::USE_VR;
        if ((value & Wrappers::ValidationControlFlags::UseDefinitions) != Wrappers::ValidationControlFlags::None) retValue |= ::USE_DEFINITION;
        if ((value & Wrappers::ValidationControlFlags::UseReferences) != Wrappers::ValidationControlFlags::None) retValue |= ::USE_REFERENCE;
        return (VALIDATION_CONTROL_FLAG_ENUM)retValue;
    };

    public enum class SessionType
    {
        SessionTypeUnknown = 0,
        SessionTypeScript,
        SessionTypeEmulator,
        SessionTypeMedia,
		SessionTypeSniffer,
    };

    public enum class ScpEmulatorType
    {
        ScpEmulatorTypeStorage,
		ScpEmulatorTypeStorageCommit,
        ScpEmulatorTypePrint,
		ScpEmulatorTypeMpps,
		ScpEmulatorTypeWorklist,
		ScpEmulatorTypeQueryRetrieve,
        ScpEmulatorTypeUnknown,
    };

    public enum class ScuEmulatorType
    {
        ScuEmulatorTypeStorage,
        ScuEmulatorTypeUnknown,
    };

    public enum class FileContentType
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
    public ref class MDisposableResources abstract
    {
    public:
        // <summary>
        // Manage a list of disposable resources.
        // </summary>
        static MDisposableResources()
        {
            pDisposables = gcnew System::Collections::ArrayList();
        }
    public:
        // <summary>
        // Add a disposable resource to the list.
        // </summary>
        static void AddDisposable(System::IDisposable^ pDisposable)
        {
            pDisposables->Add(pDisposable);
        }
        // <summary>
        // Remove a disposable resource to the list.
        // </summary>
        static void RemoveDisposable(System::IDisposable^ pDisposable)
        {
            pDisposables->Remove(pDisposable);
        }
    public:
        // <summary>
        // Dispose all resources in the list and clear the list.
        // </summary>
        /*static void Dispose()
        {
            for (int i = 0; i < pDisposables->Count; i++)
            {
                System::Object^ item = pDisposables->default[i];
                static_cast<System::IDisposable ^>(item)->Dispose();
            }
            pDisposables->Clear();
        }*/

        static void Unload()
        {
            for (int i = 0; i < pDisposables->Count; i++)
            {
                System::Object^ item = pDisposables->default[i];
                delete item;
            }
            pDisposables->Clear();
        }
    private:
        static System::Collections::ArrayList^ pDisposables;
    };

    public ref class MBaseSession abstract
        : public ISession
    {
    private protected:
        Wrappers::ActivityReportingAdapter * m_pActivityReportingAdapter;
        Wrappers::SerializationAdapter * m_pSerializationAdapter;
		Wrappers::CountingAdapter * m_pCountingAdapter;

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
        //__property virtual BASE_SESSION_CLASS __nogc* get_m_pBASE_SESSION() = 0;
        property virtual BASE_SESSION_CLASS* m_pBASE_SESSION
        {
            BASE_SESSION_CLASS* get() abstract;
        }

    public:
        // <summary>
        // Top ISerializationTarget and ICountingTarget and IActivityReportingTarget
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        void InitTopSerializationAndCountingAndActivityReportingTargets(
			ISerializationTarget^ serializationTarget,
			ICountingTarget^ countingTarget,
			IActivityReportingTarget^ activityReportingTarget);

	public:
		void StopSerializationProcessing();

    internal:
        // <summary>
        // SessionFileVersion
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        //__property void set_SessionFileVersion(System::UInt16 value);
        property System::UInt16 SessionFileVersion
        {
            void set(System::UInt16 value);
        }

    public:
        // <summary>
        // SutRole
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property virtual Wrappers::SutRole get_SutRole();
        //__property virtual void set_SutRole(Wrappers::SutRole value);
        property virtual Wrappers::SutRole SutRole
        {
            Wrappers::SutRole get();
            void set(Wrappers::SutRole value);
        }
    public:
        // <summary>
        // SutRole
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>


    public:
        // <summary>
        // DvtAeTitle
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_DvtAeTitle();
        __property virtual void set_DvtAeTitle(System::String __gc* value);*/
        property virtual System::String^ DvtAeTitle
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // DvtAeTitle
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SutAeTitle
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_SutAeTitle();
        __property virtual void set_SutAeTitle(System::String __gc* value);*/
        property virtual System::String^ SutAeTitle
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // SutAeTitle
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SutHostname
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_SutHostname();
        __property virtual void set_SutHostname(System::String __gc* value);*/
        property virtual System::String^ SutHostname
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // SutHostname
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Dvt MaximumLengthReceived
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::UInt32 get_DvtMaximumLengthReceived();
        __property virtual void set_DvtMaximumLengthReceived(System::UInt32 value);*/
        property virtual System::UInt32 DvtMaximumLengthReceived
        {
            System::UInt32 get();
            void set(System::UInt32 value);
        }
    public:
        // <summary>
        // Dvt MaximumLengthReceived
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Sut MaximumLengthReceived
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::UInt32 get_SutMaximumLengthReceived();
        __property virtual void set_SutMaximumLengthReceived(System::UInt32 value);*/
        property virtual System::UInt32 SutMaximumLengthReceived
        {
            System::UInt32 get();
            void set(System::UInt32 value);
        }
    public:
        // <summary>
        // Sut MaximumLengthReceived
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Dvt ImplementationClassUid
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_DvtImplementationClassUid();
        __property virtual void set_DvtImplementationClassUid(System::String __gc* value);*/
        property virtual System::String^ DvtImplementationClassUid
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // Dvt ImplementationClassUid
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Sut ImplementationClassUid
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_SutImplementationClassUid();
        __property virtual void set_SutImplementationClassUid(System::String __gc* value);*/
        property virtual System::String^ SutImplementationClassUid
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // Sut ImplementationClassUid
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Dvt ImplementationVersionName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
       /* __property virtual System::String __gc* get_DvtImplementationVersionName();
        __property virtual void set_DvtImplementationVersionName(System::String __gc* value);*/
        property virtual System::String^ DvtImplementationVersionName
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // Dvt ImplementationVersionName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SutImplementationVersionName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_SutImplementationVersionName();
        __property virtual void set_SutImplementationVersionName(System::String __gc* value);*/
        property virtual System::String^ SutImplementationVersionName
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // SutImplementationVersionName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SutPort
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::UInt16 get_SutPort();
        __property virtual void set_SutPort(System::UInt16 value);*/
        property virtual System::UInt16 SutPort
        {
            System::UInt16 get();
            void set(System::UInt16 value);
        }

    public:
        // <summary>
        // SutPort
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // DvtPort
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::UInt16 get_DvtPort();
        __property virtual void set_DvtPort(System::UInt16 value);*/
        property virtual System::UInt16 DvtPort
        {
            System::UInt16 get();
            void set(System::UInt16 value);
        }
    public:
        // <summary>
        // DvtPort
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // DvtSocketTimeOut
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::UInt16 get_DvtSocketTimeOut();
        __property virtual void set_DvtSocketTimeOut(System::UInt16 value);*/
        property virtual System::UInt16 DvtSocketTimeOut
        {
            System::UInt16 get();
            void set(System::UInt16 value);
        }
    public:
        // <summary>
        // DvtSocketTimeOut
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // UseSecureSockets
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_UseSecureSockets();
        __property virtual void set_UseSecureSockets(bool value);*/
        property virtual bool UseSecureSockets
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // UseSecureSockets
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // TlsPassword
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_TlsPassword();
        __property virtual void set_TlsPassword(System::String __gc* value);*/
        property virtual System::String^ TlsPassword
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // TlsPassword
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

        // TODO: Determine whether isTlsPasswordValid should be exposed.
        //bool isTlsPasswordValid(bool& unencryptedKeyFound) 
        //{ 
        //	return socketParametersM.isTlsPasswordValid(unencryptedKeyFound); 
        //}


    public:
        property virtual System::String^ MaxTlsVersion
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        property virtual System::String^ MinTlsVersion
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // TlsVersion
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // CheckRemoteCertificate
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_CheckRemoteCertificate();
        __property virtual void set_CheckRemoteCertificate(bool value);*/
        property virtual bool CheckRemoteCertificate
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // CheckRemoteCertificate
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // CipherList
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_CipherList();
        __property virtual void set_CipherList(System::String __gc* value);*/
        property virtual System::String^ CipherList
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // CipherList
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        bool IsValidCipherList(System::String^ value);

    public:
        // <summary>
        // CacheTlsSessions
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_CacheTlsSessions();
        __property virtual void set_CacheTlsSessions(bool value);*/
        property virtual bool CacheTlsSessions
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // CacheTlsSessions
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // TlsCacheTimeout
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::UInt16 get_TlsCacheTimeout();
        __property virtual void set_TlsCacheTimeout(System::UInt16 value);*/
        property virtual System::UInt16 TlsCacheTimeout
        {
            System::UInt16 get();
            void set(System::UInt16 value);
        }
    public:
        // <summary>
        // TlsCacheTimeout
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // CredentialsFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
       /* __property virtual System::String __gc* get_CredentialsFileName();
        __property virtual void set_CredentialsFileName(System::String __gc* value);*/
        property virtual System::String^ CredentialsFileName
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // CredentialsFileName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // CertificateFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_CertificateFileName();
        __property virtual void set_CertificateFileName(System::String __gc* value);*/
        property virtual System::String^ CertificateFileName
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // CertificateFileName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SocketParametersChanged
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        //__property virtual void set_SocketParametersChanged(bool value);
        property virtual bool SocketParametersChanged
        {
            void set(bool value);
        }

    public:
        // <summary>
        // AutoType2Attributes
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_AutoType2Attributes();
        __property virtual void set_AutoType2Attributes(bool value);*/
        property virtual bool AutoType2Attributes
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // AutoType2Attributes
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // AutoCreateDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_AutoCreateDirectory();
        __property virtual void set_AutoCreateDirectory(bool value);*/
        property virtual bool AutoCreateDirectory
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // AutoCreateDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // DefineSqLength
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
       /* __property virtual bool get_DefineSqLength();
        __property virtual void set_DefineSqLength(bool value);*/
        property virtual bool DefineSqLength
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // DefineSqLength
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // AddGroupLength
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_AddGroupLength();
        __property virtual void set_AddGroupLength(bool value);*/
        property virtual bool AddGroupLength
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // AddGroupLength
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // StrictValidation
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_StrictValidation();
        __property virtual void set_StrictValidation(bool value);*/
        property virtual bool StrictValidation
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // StrictValidation
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
		
    public:
        // <summary>
        // TestLogValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_TestLogValidationResults();
        __property virtual void set_TestLogValidationResults(bool value);*/
        property virtual bool TestLogValidationResults
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // TestLogValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // DetailedValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_DetailedValidationResults();
        __property virtual void set_DetailedValidationResults(bool value);*/
        property virtual bool DetailedValidationResults
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // DetailedValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

	public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_SummaryValidationResults();
        __property virtual void set_SummaryValidationResults(bool value);*/
        property virtual bool SummaryValidationResults
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
		
	public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property static bool get_UsePrivateAttributeMapping();
        __property static void set_UsePrivateAttributeMapping(bool value);*/
        property static bool UsePrivateAttributeMapping
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // SummaryValidationResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

		    public:
        // <summary>
        // IncludeType3NotPresentInResults
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_IncludeType3NotPresentInResults();
        __property virtual void set_IncludeType3NotPresentInResults(bool value);*/
        property virtual bool IncludeType3NotPresentInResults
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // IncludeType3NotPresentInResults
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // ContinueOnError
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_ContinueOnError();
        __property virtual void set_ContinueOnError(bool value);*/
        property virtual bool ContinueOnError
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // ContinueOnError
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // ValidateReferencedFile
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_ValidateReferencedFile();
        __property virtual void set_ValidateReferencedFile(bool value);*/
        property virtual bool ValidateReferencedFile
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // ValidateReferencedFile
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // UnVrDefinitionLookUp
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_UnVrDefinitionLookUp();
        __property virtual void set_UnVrDefinitionLookUp(bool value);*/
        property virtual bool UnVrDefinitionLookUp
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // UnVrDefinitionLookUp
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

	public:
        // <summary>
        // DumpAttributesOfReferencedFile
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_DumpAttributesOfRefFiles();
        __property virtual void set_DumpAttributesOfRefFiles(bool value);*/
        property virtual bool DumpAttributesOfRefFiles
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // DumpAttributesOfReferencedFile
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // StorageMode
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual Wrappers::StorageMode get_StorageMode();
        __property virtual void set_StorageMode(Wrappers::StorageMode value);*/
        property virtual Wrappers::StorageMode StorageMode
        {
            Wrappers::StorageMode get();
            void set(Wrappers::StorageMode value);
        }
    public:
        // <summary>
        // StorageMode
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // LogScpThread
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual bool get_LogScpThread();
        __property virtual void set_LogScpThread(bool value);*/
        property virtual bool LogScpThread
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // LogScpThread
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

	public:
        // <summary>
        // DelayBeforeNEventReport
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::UInt16 get_DelayBeforeNEventReport();
        __property virtual void set_DelayBeforeNEventReport(System::UInt16 value);*/
        property virtual System::UInt16 DelayBeforeNEventReport
        {
            System::UInt16 get();
            void set(System::UInt16 value);
        }
    public:
        // <summary>
        // DelayBeforeNEventReport
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
	public:
        // <summary>
        // AcceptDuplicateImage
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        //__property virtual void set_AcceptDuplicateImage(bool value);
        property virtual bool AcceptDuplicateImage
        {
            void set(bool value);
        }
	public:
        // <summary>
        // StoreCStoreReqOnly
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        //__property virtual void set_StoreCStoreReqOnly(bool value);
        property virtual bool StoreCStoreReqOnly
        {
            void set(bool value);
        }

    public:
        virtual void DeleteSupportedTransferSyntaxes();

    public:
        virtual void AddSupportedTransferSyntax(System::String^ value);

    public:
        // <summary>
        // No Of Supported Transfer Syntaxes
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property virtual System::UInt16 get_NrOfSupportedTransferSyntaxes();
        property virtual System::UInt16 NrOfSupportedTransferSyntaxes
        {
            System::UInt16 get();
        }

    public:
        //__property virtual System::String __gc* get_SupportedTransferSyntax(System::UInt16 index);
        property virtual System::String^ SupportedTransferSyntax[System::UInt16]
        {
            System::String^ get(System::UInt16 index);
        }

    public:
        // <summary>
        // DicomScriptRootDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_DicomScriptRootDirectory();
        __property void set_DicomScriptRootDirectory(System::String __gc* value);*/
        property System::String^ DicomScriptRootDirectory
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // DicomScriptRootDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        virtual bool Begin([In, Out] bool value);
    public:
        virtual void End(void);

    public:
        virtual bool ExecuteScript(System::String^ value);
    public:
        virtual bool ParseScript(System::String^ value);

    public:
		/*__property virtual Wrappers::ScpEmulatorType get_ScpEmulatorType();
		__property virtual void set_ScpEmulatorType(Wrappers::ScpEmulatorType value);*/
        property virtual Wrappers::ScpEmulatorType ScpEmulatorType
        {
            Wrappers::ScpEmulatorType get();
            void set(Wrappers::ScpEmulatorType value);
        }

    public:

    public:
		/*__property virtual Wrappers::ScuEmulatorType get_ScuEmulatorType();
		__property virtual void set_ScuEmulatorType(Wrappers::ScuEmulatorType value);*/
        property virtual Wrappers::ScuEmulatorType ScuEmulatorType
        {
            Wrappers::ScuEmulatorType get();
            void set(Wrappers::ScuEmulatorType value);
        }

    public:

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
        virtual bool ValidateMediaFile(System::String^ filename);

    public:
        virtual bool ValidateMediaFile(System::String^ filename,
            Wrappers::FileContentType fileContentType,
            System::String^ sopClassUid, 
            System::String^ sopInstanceUid, 
            System::String^ transferSyntaxUid);

	public:
		virtual bool ValidateMediaFile(System::String ^ filename, System::String ^ recordsToFilter, int numberRecordsToFilter);

    public:
        virtual bool EndMediaValidation(void);

    public:
        // <summary>
        // InstanceId
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::UInt32 get_InstanceId();
        __property void set_InstanceId(System::UInt32 value);*/
        property System::UInt32 InstanceId
        {
            System::UInt32 get();
            void set(System::UInt32 value);
        }

    public:
        // <summary>
        // InstanceId
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SessionDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_SessionDirectory();
        __property void set_SessionDirectory(System::String __gc* value);*/
        property System::String^ SessionDirectory
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // SessionDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SessionFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_SessionFileName();
        __property void set_SessionFileName(System::String __gc* value);*/
        property System::String^ SessionFileName
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // SessionFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>

    public:
        // <summary>
        // AppendToResultsFile
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property bool get_AppendToResultsFile();
        __property void set_AppendToResultsFile(bool value);*/
        property bool AppendToResultsFile
        {
            bool get();
            void set(bool value);
        }
    public:
        // <summary>
        // AppendToResultsFile
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

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
        //__property System::UInt16 get_Counter();
        property System::UInt16 Counter
        {
            System::UInt16 get();
        }

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
        /*__property virtual Wrappers::SessionType get_SessionType();
        __property virtual void set_SessionType(Wrappers::SessionType value);*/
        property virtual Wrappers::SessionType SessionType
        {
            Wrappers::SessionType get();
            void set(Wrappers::SessionType value);
        }

    public:
        // <summary>
        // SessionType
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // RunTimeSessionType
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property virtual Wrappers::SessionType get_RunTimeSessionType();
        property virtual Wrappers::SessionType RunTimeSessionType
        {
            Wrappers::SessionType get();
        }

    public:
        // <summary>
        // SessionTitle
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_SessionTitle();
        __property void set_SessionTitle(System::String __gc* value);*/
        property System::String^ SessionTitle
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // SessionTitle
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SessionId
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::UInt16 get_SessionId();
        __property void set_SessionId(System::UInt16 value);*/
        property System::UInt16 SessionId
        {
            System::UInt16 get();
            void set(System::UInt16 value);
        }

    public:
        // <summary>
        // SessionId
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Manufacturer
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_Manufacturer();
        __property void set_Manufacturer(System::String __gc* value);*/
        property System::String^ Manufacturer
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // Manufacturer
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // ModelName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
       /* __property System::String __gc* get_ModelName();
        __property void set_ModelName(System::String __gc* value);*/
        property System::String^ ModelName
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // ModelName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

	public:
        // <summary>
        // SoftwareVersions
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_SoftwareVersions();
        __property void set_SoftwareVersions(System::String __gc* value);*/
        property System::String^ SoftwareVersions
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // SoftwareVersions
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // ApplicationEntityName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::String __gc* get_ApplicationEntityName();
        //__property void set_ApplicationEntityName(System::String __gc* value);
        property System::String^ ApplicationEntityName
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // ApplicationEntityName
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // ApplicationEntityVersion
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_ApplicationEntityVersion();
        __property void set_ApplicationEntityVersion(System::String __gc* value);*/
        property System::String^ ApplicationEntityVersion
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // ApplicationEntityVersion
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // TestedBy
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_TestedBy();
        __property void set_TestedBy(System::String __gc* value);*/
        property System::String^ TestedBy
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // TestedBy
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Date
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_Date();
        __property void set_Date(System::String __gc* value);*/
        property System::String^ Date
        {
            System::String^ get();
            void set(System::String^ value);
        }
    //public:
        // <summary>
        // Date
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // LogMask
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property virtual LogMask get_LogMask();
        property virtual System::UInt32 LogMask
        {
            System::UInt32 get();
        }

    public:
        // <summary>
        // DefinitionFileRootDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_DefinitionFileRootDirectory();
        __property virtual void set_DefinitionFileRootDirectory(System::String __gc* value);*/
        property virtual System::String^ DefinitionFileRootDirectory
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // DefinitionFileRootDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Add definition-file directory to definition-file search path.
        // </summary>
        // <remarks>
        // Should be existing file paths.
        // </remarks>
        void AddDefinitionFileDirectory(System::String ^ pDefinitionFileDirectory);

    public:
        // <summary>
        // Get number of directories specified in the definition-file search path.
        // </summary>
        // <remarks>
        // Should be existing file paths.
        // </remarks>
        //__property System::UInt16 get_NrOfDefinitionFileDirectories();
        property virtual System::UInt16 NrOfDefinitionFileDirectories
        {
            System::UInt16 get();
        }

    public:
        // <summary>
        // Get the specified definition-file directory from the list of directories in the definition-file
        // search path.
        // </summary>
        // <remarks>
        // </remarks>
        //__property System::String __gc* get_DefinitionFileDirectory(System::UInt16 index);
        property System::String^ DefinitionFileDirectory[System::UInt16]
        {
            System::String^ get(System::UInt16);
        }

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
        //__property virtual System::UInt16 get_NrOfDefinitionFiles();
        property virtual System::UInt16 NrOfDefinitionFiles
        {
            System::UInt16 get();
        }

    public:
        // <summary>
        // DefinitionFileName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property virtual System::String __gc* get_DefinitionFileName(System::UInt16 index);
        property virtual System::String^ DefinitionFileName[System::UInt16]
        {
            System::String^ get(System::UInt16);
        }

    public:
        // <summary>
        // DataDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property virtual System::String __gc* get_DataDirectory();
        //__property virtual void set_DataDirectory(System::String __gc* value);
        property virtual System::String^ DataDirectory
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // DataDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // ResultsRootDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String __gc* get_ResultsRootDirectory();
        __property virtual void set_ResultsRootDirectory(System::String __gc* value);*/
        property virtual System::String^ ResultsRootDirectory
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // ResultsRootDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        //__property System::String __gc* get_AbsolutePixelPathName(System::String __gc* value);
        property System::String^ AbsolutePixelPathName[System::String^]
        {
            System::String^ get(System::String^);
        }

    public:
        // <summary>
        // IsOpen
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property bool get_IsOpen();
        property bool IsOpen
        {
            bool get();
        }

	public:
		//__property bool get_IsAssociated();
        property bool IsAssociated
        {
            bool get();
        }

    internal:
        void Load(
            System::String^ sessionFileName,
            [In, Out] bool definitionFileLoaded,
            bool andBeginIt);

    public:
        bool Save();

   	public:
        DvtkData::Media::DicomFile ^ ReadMedia(
            System::String ^ pFileName);
    public:
        System::Boolean WriteMedia(
            DvtkData::Media::DicomFile ^ pDicomFile, System::String ^ pFileName);

	public:
        DvtkData::Media::DicomDir ^ ReadDicomdir(
            System::String ^ pFileName);
    public:
        System::Boolean WriteDicomdir(
            DvtkData::Media::DicomDir ^ pDicomFile, System::String^ pFileName);

    public:
        bool LoadDefinitionFile(System::String ^ definitionFileName);

    public:
        bool UnLoadDefinitionFile(System::String ^ definitionFileName);

    public:
        // <summary>
        // DescriptionDirectory
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property virtual System::String ^ get_DescriptionDirectory();
        __property virtual void set_DescriptionDirectory(System::String ^ value);*/
        property virtual System::String^ DescriptionDirectory
        {
            System::String^ get();
            void set(System::String^ value);
        }
    public:
        // <summary>
        // DescriptionDirectory
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        //__property System::String^ get_SopUid(System::String ^ value);
        property System::String^ SopUid[System::String^]
        {
            System::String^ get(System::String^);
        }

    public:
        //__property System::String^ get_IodName(System::String ^ value);
        property System::String ^ IodName[System::String^]
        {
            System::String^ get(System::String^);
        }

	public:
		//__property System::String __gc* get_IodNameFromDefinition(DvtkData::Dimse::DimseCommand command, System::String __gc* uid);
        property System::String^ IodNameFromDefinition[DvtkData::Dimse::DimseCommand, System::String^]
        {
            System::String^ get(DvtkData::Dimse::DimseCommand command, System::String ^ uid);
        }

	public:
		//__property System::String __gc* get_FileNameFromSOPUID(DvtkData::Dimse::DimseCommand command, System::String __gc* uid);
        property System::String^ FileNameFromSOPUID[DvtkData::Dimse::DimseCommand , System::String^]
        {
            System::String^ get(DvtkData::Dimse::DimseCommand command, System::String^ uid);
        }

	public:
		//__property System::String __gc* get_AttributeNameFromDefinition(DvtkData::Dimse::Tag __gc* pTag);
        property System::String^ AttributeNameFromDefinition[DvtkData::Dimse::Tag^]
        {
            System::String^ get(DvtkData::Dimse::Tag ^ pTag);
        }

	public:
		//__property DvtkData::Dimse::VR get_AttributeVrFromDefinition(DvtkData::Dimse::Tag __gc* pTag);
        property DvtkData::Dimse::VR AttributeVrFromDefinition[DvtkData::Dimse::Tag^]
        {
            DvtkData::Dimse::VR get(DvtkData::Dimse::Tag^ pTag);
        }

    public:
        void EnableLogger(void);
    public:
        void DisableLogger(void);

    public:
       /* __property bool get_LogLevelEnabled(System::UInt32 logLevel);
        __property void set_LogLevelEnabled(System::UInt32 logLevel, bool enabled);*/
        property bool LogLevelEnabled[System::UInt32]
        {
            bool get(System::UInt32 logLevel);
            void set(System::UInt32 logLevel, bool enabled);
        }
    public:
		
    public:
        // <summary>
        // IsStopped
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property bool get_IsStopped();
        property bool IsStopped
        {
            bool get();
        }

    public:
        virtual void AddDicomScript(System::String ^ value);

    //private:
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
        //__property virtual void set_Rules(System::Uri __gc* pRulesUri);
     property System::Uri^ Rules
     {
         void set(System::Uri^ pRulesUri);
     }

    public:
        static System::Uri ^ RuleUriStandardRules;
    public:
        static System::Uri ^ RuleUriStrictRules;

    };
}