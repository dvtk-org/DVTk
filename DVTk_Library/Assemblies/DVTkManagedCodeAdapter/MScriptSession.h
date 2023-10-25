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
#include "IDimse.h"
#include "ISockets.h"
#include "IDul.h"
#include "ISessions.h"
#include "MBaseSession.h"
#include <vcclr.h>
#include "ConfirmInteractionAdapter.h"

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    public enum class ReceiveReturnCode
    {
        Success = 0,
        Failure,
        AssociationRejected,
        AssociationReleased,
        AssociationAborted,
        SocketClosed,
        NoSocketConnection,
    };

    public enum class SendReturnCode
    {
        Success = 0,
        Failure,
        AssociationRejected,
        AssociationReleased,
        AssociationAborted,
        SocketClosed,
        NoSocketConnection,
    };

    public ref class MScriptSession
        : public MBaseSession
        , public IDimse
        , public ISockets
        , public IDul
        , public ISession
        , public System::IDisposable
    {
    private protected:
        ConfirmInteractionAdapter * m_pConfirmInteractionAdapter;
    private:
        SCRIPT_SESSION_CLASS * m_pSCRIPT_SESSION_CLASS;
    private:
        bool m_isDataTransferExplicit;
    private protected:
        /*__property BASE_SESSION_CLASS __nogc* get_m_pBASE_SESSION()
        {
            return m_pSCRIPT_SESSION_CLASS;
        }*/

        property BASE_SESSION_CLASS * m_pBASE_SESSION
        {
            BASE_SESSION_CLASS * get() override 
            { 
                return m_pSCRIPT_SESSION_CLASS;
            }
        }
    public:
        // <summary>
        // IConfirmInteractionTarget
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        //__property void set_ConfirmInteractionTarget(IConfirmInteractionTarget __gc* value);
        property IConfirmInteractionTarget^ ConfirmInteractionTarget
        {
            void set(IConfirmInteractionTarget ^ value);
        }
    public:
        // <summary>
        // Constructor
        // </summary>
        // <remarks>
        // None
        // </remarks>
        MScriptSession(void);
    public:
        // <summary>
        // Destructor
        // </summary>
        // <remarks>
        // None
        // </remarks>
        ~MScriptSession(void);

    public:
        // <summary>
        // No Of Dicom Scripts
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::UInt16 get_NrOfDicomScripts();
        property System::UInt16 NrOfDicomScripts
        {
            System::UInt16 get();
        }

    public:
        // <summary>
        // Pending Data In Network Input Buffer
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property bool get_HasPendingDataInNetworkInputBuffer();
        property bool HasPendingDataInNetworkInputBuffer
        {
            bool get();
        }

    public:
        // <summary>
        // UseSecureSockets
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property virtual bool get_IsDataTransferExplicit();
        //__property virtual void set_IsDataTransferExplicit(bool value);
        property virtual bool IsDataTransferExplicit
        {
            bool get();
            void set(bool value); 
        }
        property virtual System::String^ CredentialsFilename
        {
            System::String^ get() { return ""; };
            void set(System::String^ value) {};
        }

    public:
        ReceiveReturnCode Receive([Out] DvtkData::Message^ % ppMessage);
    public:
        ReceiveReturnCode Receive([Out] DvtkData::Dul::DulMessage^ % ppDulMessage);
    public:
        ReceiveReturnCode Receive([Out] DvtkData::Dimse::DicomMessage^ % ppDicomMessage);
    public:
        SendReturnCode Send(DvtkData::Message^ pMessage);
    public:
        SendReturnCode Send(DvtkData::Dimse::DicomMessage^ pMessage, int pcId);
    public:
        bool Validate(
            DvtkData::Dimse::DicomMessage^ pMessage,
            DvtkData::Dimse::DicomMessage^ pReferenceMessage,
            Wrappers::ValidationControlFlags validationControlFlags);
    public:
        bool Validate(
            DvtkData::Dimse::DicomMessage^ pMessage,
            DvtkData::Dimse::DicomMessage^ pReferenceMessage,
            DvtkData::Dimse::DicomMessage^ pLastMessage,
            Wrappers::ValidationControlFlags validationControlFlags);
    public:
        bool Validate(
            DvtkData::Dul::DulMessage^ pMessage,
            DvtkData::Dul::DulMessage^ pReferenceMessage,
            Wrappers::ValidationControlFlags validationControlFlags);

    public:
        bool ComparePixelData(
            DvtkData::Dimse::DicomMessage^ pMessage,
            DvtkData::Dimse::DicomMessage^ pReferenceMessage);

    private:
        // Track whether Dispose has been called.
        // m_pSCRIPT_SESSION_CLASS is treated as disposable resource.
        bool disposed;

    public:
        void DisposeThis();

    private:
        void DisposeThis(bool disposing);
    };
}
