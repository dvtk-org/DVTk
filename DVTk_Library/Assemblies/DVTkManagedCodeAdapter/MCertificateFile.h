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
#include "MCertificate.h"

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    public enum class WrappedSecurityItemType
    {
        Credential,
        Certificate,
    };

    namespace Exceptions
    {
    public ref class SecureSocketLibraryNotAvailableException : public System::ApplicationException
    {
    public:
        SecureSocketLibraryNotAvailableException() {};
    public:
        SecureSocketLibraryNotAvailableException(System::String^ message) : System::ApplicationException(message) {};
    public:
        SecureSocketLibraryNotAvailableException(System::String^ message, System::Exception^ inner) : System::ApplicationException(message, inner) {};
    };

    public ref class CredentialFileLoadExpection : public System::ApplicationException
    {
    public:
        CredentialFileLoadExpection() {};
    public:
        CredentialFileLoadExpection(System::String^ message) : System::ApplicationException(message) {};
    public:
        CredentialFileLoadExpection( System::String^ message, System::Exception^ inner) : System::ApplicationException(message, inner) {};
    };

    public ref class CertificateFileLoadExpection : public System::ApplicationException
    {
    public:
        CertificateFileLoadExpection() {};
    public:
        CertificateFileLoadExpection(System::String^ message) : System::ApplicationException(message) {};
    public:
        CertificateFileLoadExpection(System::String^ message, System::Exception^ inner) : System::ApplicationException(message, inner) {};
    };

    public ref class PasswordExpection : public System::ApplicationException
    {
    public:
        PasswordExpection() {};
    public:
        PasswordExpection(System::String^ message) : System::ApplicationException(message) {};
    public:
        PasswordExpection(System::String^ message, System::Exception^ inner) : System::ApplicationException(message, inner) {};
    };
    }

    public ref class MCertificateFile
    {
    private protected:
        CERTIFICATE_FILE_CLASS * m_pC;

    public:
        // throws on dvt error status
        MCertificateFile(
            Wrappers::WrappedSecurityItemType securityFileType,
            System::String^ pFileName,
            System::String^ pPassword);
    public:
        ~MCertificateFile(void);

    public:
        // throws on dvt error status
        void AddCertificate(
            Wrappers::WrappedSecurityItemType securityItemType,
            System::String^ pFileName,
            System::String^ pPassword);
        
    public:
        System::Boolean RemoveCertificate(System::UInt32 index);
        
    public:
        // <summary>
        // Get number of certificates
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::UInt32 get_NumberOfCertificates();
        property System::UInt32 NumberOfCertificates
        {
            System::UInt32 get();
        }

    public:
        // <summary>
        // Get certificate
        // </summary>
        // <remarks>
        // Get Indexed Property
        // </remarks>
        //__property Wrappers::MCertificate __gc* get_Certificate(System::UInt32 index);
        property Wrappers::MCertificate ^ Certificate[System::UInt32]
        {
            Wrappers::MCertificate ^ get(System::UInt32 index);
        }
        
    public:
        // <summary>
        // Get the password
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::String __gc* get_Password();
        property System::String^ Password
        {
            System::String^ get();
        }

    public:
        // <summary>
        // Get the HasChanged state
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::Boolean get_HasChanged();
        property System::Boolean HasChanged
        {
            System::Boolean get();
        }

    public:
        void WriteFile(System::String^ pPassword);

    public:
        System::Boolean Verify(
            Wrappers::WrappedSecurityItemType securityItemType,
            //[System::Runtime::InteropServices::Out] System::String __gc* __gc* ppVerificationMessage); 
            [System::Runtime::InteropServices::Out] System::String^ % ppVerificationMessage);
    };
}