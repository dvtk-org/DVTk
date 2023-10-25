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

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    public ref class MCertificate
    {
    private:
        static inline bool _RangeCheckDateTimeInput(
            int year,
            int mon,
            int mday,
            int hour,
            int min,
            int sec);

    private protected:
        CERTIFICATE_CLASS * m_pC;

    public:
        MCertificate(void);
        ~MCertificate(void);

    //internal:
    internal:
        void Wrap(CERTIFICATE_CLASS* pNewCERTIFICATE_CLASS);

    public:
        // <summary>
        // EffectiveDate
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::DateTime get_EffectiveDate();
        __property void set_EffectiveDate(System::DateTime value);*/
        property System::DateTime EffectiveDate
        {
            System::DateTime get();
            void set(System::DateTime value);
        }

    public:
        // <summary>
        // EffectiveDate
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // ExpirationDate
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::DateTime get_ExpirationDate();
        __property void set_ExpirationDate(System::DateTime value);*/
        property System::DateTime ExpirationDate
        {
            System::DateTime get();
            void set(System::DateTime value);
        }

    public:
        // <summary>
        // ExpirationDate
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Issuer
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* MCertificate::get_Issuer();
        __property void set_Issuer(System::String __gc* value);*/
        property System::String^ Issuer
        {
            System::String^ MCertificate::get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // Issuer
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SerialNumber
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_SerialNumber();
        __property void set_SerialNumber(System::String __gc* value);*/
        property System::String^ SerialNumber
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // SerialNumber
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SignatureAlgorithm
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_SignatureAlgorithm();
        __property void set_SignatureAlgorithm(System::String __gc* value);*/
        property System::String^ SignatureAlgorithm
        {
            System::String^ get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // SignatureAlgorithm
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // SignatureKeyLength
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::Int32 get_SignatureKeyLength();
        __property void set_SignatureKeyLength(System::Int32 value);*/
        property System::Int32 SignatureKeyLength
        {
            System::Int32 get();
            void set(System::Int32 value);
        }

    public:
        // <summary>
        // SignatureKeyLength
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Subject
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* MCertificate::get_Subject();
        __property void set_Subject(System::String __gc* value);*/
        property System::String^ Subject
        {
            System::String^ MCertificate::get();
            void set(System::String^ value);
        }

    public:
        // <summary>
        // Subject
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        // <summary>
        // Version
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::Int32 get_Version();
        __property void set_Version(System::Int32 value);*/
        property System::Int32 Version
        {
            System::Int32 get();
            void set(System::Int32 value);
        }

    public:
        // <summary>
        // Version
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>

    public:
        void GenerateFiles(
            System::String^ pSignerCredentialsFile,
            System::String^ pCredentialsPassword,
            System::String^ pKeyPassword,
            System::String^ pKeyFile,
            System::String^ pCertificateFile);
    };
}