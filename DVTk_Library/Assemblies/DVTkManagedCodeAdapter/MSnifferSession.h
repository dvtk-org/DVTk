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
#include "MBaseSession.h"

namespace Wrappers
{
    public enum class ReceivedMsgReturnCode
    {
        Success = 0,
        Failure,
        IncompleteByteStream,
        DecodingError,
    };

    public ref class MSnifferSession
        : public MBaseSession
        , public System::IDisposable
    {
    private:
        SNIFFER_SESSION_CLASS * m_pSNIFFER_SESSION_CLASS;
    private protected:
        /*__property BASE_SESSION_CLASS * get_m_pBASE_SESSION()
        {
            return m_pSNIFFER_SESSION_CLASS;
        }*/
        property BASE_SESSION_CLASS* m_pBASE_SESSION
        {
            BASE_SESSION_CLASS* get() override 
            {
                return m_pSNIFFER_SESSION_CLASS;
            }
        }
    public:
        // <summary>
        // Constructor
        // </summary>
        // <remarks>
        // None
        // </remarks>
        MSnifferSession(void);
    public:
        // <summary>
        // Destructor
        // </summary>
        // <remarks>
        // None
        // </remarks>
        ~MSnifferSession(void);

    public:
        System::Boolean Validate(
            DvtkData::Dimse::DicomMessage^ pDicomMessage,
            Wrappers::ValidationControlFlags validationControlFlags);

        System::Boolean Validate(
            DvtkData::Dul::DulMessage^ pDicomMessage,
            Wrappers::ValidationControlFlags validationControlFlags);

        //void ReadPDUs(System::String __gc* pduFileNames[]);
        void ReadPDUs(cli::array<System::String^>^ pduFileNames);

        ReceivedMsgReturnCode ReceiveMessage(
            [Out] DvtkData::Message^ % ppMessage);

    private:
        // Track whether Dispose has been called.
        bool disposed;

    public:
        void DisposeThis();

    private:
        void DisposeThis(bool disposing);
    };
}

