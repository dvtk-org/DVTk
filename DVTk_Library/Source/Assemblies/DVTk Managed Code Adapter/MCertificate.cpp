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
#include ".\MCertificate.h"
#include "TIME.h"
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    bool MCertificate::_RangeCheckDateTimeInput(
        int year,
        int mon,
        int mday,
        int hour,
        int min,
        int sec)
    {
        return (
            (1 <= year) && (year <= 9999) &&
            (1 <= mon) && (mon <= 12) &&
            // The day (1 through the number of days in month).
            // Precise days per month checking if left to DateTime constructor.
            // DateTime constructor will throw an ArgumentOutOfRangeException
            // on invalid days per month.
            (0 <= mday) && (mday <= 32) &&
            (0 <= hour) && (hour <= 23) &&
            (0 <= min) && (min <= 59) &&
            (0 <= sec) && (sec <=59)
            );
    }

    MCertificate::MCertificate(void)
    {
        m_pC = new CERTIFICATE_CLASS();
		this->set_EffectiveDate(System::DateTime::Now);
		this->set_ExpirationDate(System::DateTime::Now);
    }

    MCertificate::~MCertificate(void)
    {
        delete m_pC;
    }

    void MCertificate::Wrap(CERTIFICATE_CLASS* pNewCERTIFICATE_CLASS)
    {
        if (pNewCERTIFICATE_CLASS != m_pC)
        {
            CERTIFICATE_CLASS* pOldCERTIFICATE_CLASS = m_pC;
            m_pC = pNewCERTIFICATE_CLASS;
            delete pOldCERTIFICATE_CLASS;
        }
    };

    System::DateTime MCertificate::get_EffectiveDate()
    {
        struct tm* internalEffectiveDate = m_pC->getEffectiveDate();
        System::DateTime effectiveDate;
        int year    = internalEffectiveDate->tm_year+1900;
        int mon     = internalEffectiveDate->tm_mon+1;
        int mday    = internalEffectiveDate->tm_mday;
        int hour    = internalEffectiveDate->tm_hour;
        int min     = internalEffectiveDate->tm_min;
        int sec     = internalEffectiveDate->tm_sec;
        bool valid = _RangeCheckDateTimeInput(year, mon, mday, hour, min, sec);
        if (valid)
        {
            effectiveDate = System::DateTime(year, mon, mday, hour, min, sec);
        }
        else
        {
            effectiveDate = System::DateTime::MinValue;
        }
        return effectiveDate;
    }
    void MCertificate::set_EffectiveDate(System::DateTime value)
    {
        struct tm internalEffectiveDate;
        memset( &internalEffectiveDate, 0, sizeof( struct tm ) );
        internalEffectiveDate.tm_year   = value.Year-1900;
        internalEffectiveDate.tm_mon    = value.Month-1;
        internalEffectiveDate.tm_mday   = value.Day;
        internalEffectiveDate.tm_hour   = value.Hour;
        internalEffectiveDate.tm_min    = value.Minute;
        internalEffectiveDate.tm_sec    = value.Second;
        m_pC->setEffectiveDate(internalEffectiveDate);
    }

    System::DateTime MCertificate::get_ExpirationDate()
    {
        System::DateTime expirationDate;
        struct tm* internalExpirationDate = m_pC->getExpirationDate();
        int year    = internalExpirationDate->tm_year+1900;
        int mon     = internalExpirationDate->tm_mon+1;
        int mday    = internalExpirationDate->tm_mday;
        int hour    = internalExpirationDate->tm_hour;
        int min     = internalExpirationDate->tm_min;
        int sec     = internalExpirationDate->tm_sec;
        bool valid = _RangeCheckDateTimeInput(year, mon, mday, hour, min, sec);
        if (valid)
        {
            expirationDate = System::DateTime(year, mon, mday, hour, min, sec);
        }
        else
        {
            expirationDate = System::DateTime::MinValue;
        }
        return expirationDate;
    }
    void MCertificate::set_ExpirationDate(System::DateTime value)
    {
        struct tm internalExpirationDate;
        memset( &internalExpirationDate, 0, sizeof( struct tm ) );
        internalExpirationDate.tm_year  = value.Year-1900;
        internalExpirationDate.tm_mon   = value.Month-1;
        internalExpirationDate.tm_mday  = value.Day;
        internalExpirationDate.tm_hour  = value.Hour;
        internalExpirationDate.tm_min   = value.Minute;
        internalExpirationDate.tm_sec   = value.Second;
        m_pC->setExpirationDate(internalExpirationDate);
    }

    void MCertificate::set_Issuer (System::String __gc *value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pC->setIssuer(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }
    System::String __gc* MCertificate::get_Issuer ()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pC->getIssuer();
    }

    void MCertificate::set_SerialNumber (System::String __gc *value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pC->setSerialNumber(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }
    System::String __gc* MCertificate::get_SerialNumber ()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pC->getSerialNumber();
    }

    void MCertificate::set_SignatureAlgorithm (System::String __gc *value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pC->setSignatureAlgorithm(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }
    System::String __gc* MCertificate::get_SignatureAlgorithm ()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pC->getSignatureAlgorithm();
    }

    System::Int32 MCertificate::get_SignatureKeyLength()
    {
        return m_pC->getSignatureKeyLength();
    }
    void MCertificate::set_SignatureKeyLength(System::Int32 value)
    {
        return m_pC->setSignatureKeyLength(value);
    }

    void MCertificate::set_Subject (System::String __gc *value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        m_pC->setSubject(pAnsiString);
        Marshal::FreeHGlobal(pAnsiString);
    }
    System::String __gc* MCertificate::get_Subject ()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        return m_pC->getSubject();
    }

    System::Int32 MCertificate::get_Version()
    {
        return m_pC->getVersion();
    }
    void MCertificate::set_Version(System::Int32 value)
    {
        return m_pC->setVersion(value);
    }

    void MCertificate::GenerateFiles(
        System::String __gc* pSignerCredentialsFile,
        System::String __gc* pCredentialsPassword,
        System::String __gc* pKeyPassword,
        System::String __gc* pKeyFile,
        System::String __gc* pCertificateFile)
    {
        char* pAnsiStringSignerCredentialsFile 
            = (char*)(void*)Marshal::StringToHGlobalAnsi(pSignerCredentialsFile);
        char* pAnsiStringCredentialsPassword 
            = (char*)(void*)Marshal::StringToHGlobalAnsi(pCredentialsPassword);
        char* pAnsiStringKeyPassword 
            = (char*)(void*)Marshal::StringToHGlobalAnsi(pKeyPassword);
        char* pAnsiStringKeyFile 
            = (char*)(void*)Marshal::StringToHGlobalAnsi(pKeyFile);
        char* pAnsiStringCertificateFile 
            = (char*)(void*)Marshal::StringToHGlobalAnsi(pCertificateFile);
        LOG_CLASS* logger_ptr = NULL;
        DVT_STATUS dvtStatus = m_pC->generateFiles(
            logger_ptr,
            pAnsiStringSignerCredentialsFile,
            pAnsiStringCredentialsPassword,
            pAnsiStringKeyPassword,
            pAnsiStringKeyFile,
            pAnsiStringCertificateFile);
        switch (dvtStatus)
        {
        case ::MSG_OK: break;
        case ::MSG_LIB_NOT_EXIST:
            throw new System::ApplicationException(
                "Secure Sockets library is not present. Cannot generate certificates");
        case ::MSG_FILE_NOT_EXIST:
            throw new System::ApplicationException("Credentials file does not exist");
        case ::MSG_INVALID_PASSWORD:
            throw new System::ApplicationException("Credentials file password is invalid");
        case ::MSG_ERROR:
		default:
            throw new System::ApplicationException("Error generating certificate.");
        }
        Marshal::FreeHGlobal(pAnsiStringSignerCredentialsFile);
        Marshal::FreeHGlobal(pAnsiStringCredentialsPassword);
        Marshal::FreeHGlobal(pAnsiStringKeyPassword);
        Marshal::FreeHGlobal(pAnsiStringKeyFile);
        Marshal::FreeHGlobal(pAnsiStringCertificateFile);
    }
}