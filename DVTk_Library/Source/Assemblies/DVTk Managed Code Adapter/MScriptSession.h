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

    public __value enum ReceiveReturnCode
    {
        Success = 0,
        Failure,
        AssociationRejected,
        AssociationReleased,
        AssociationAborted,
        SocketClosed,
        NoSocketConnection,
    };

    public __value enum SendReturnCode
    {
        Success = 0,
        Failure,
        AssociationRejected,
        AssociationReleased,
        AssociationAborted,
        SocketClosed,
        NoSocketConnection,
    };

    public __gc class MScriptSession
        : public MBaseSession
        , public IDimse
        , public ISockets
        , public IDul
        , public ISession
        , public System::IDisposable
    {
    private protected:
        ConfirmInteractionAdapter __nogc* m_pConfirmInteractionAdapter;
    private:
        SCRIPT_SESSION_CLASS __nogc* m_pSCRIPT_SESSION_CLASS;
	private:
		bool m_isDataTransferExplicit;
    private protected:
        __property BASE_SESSION_CLASS __nogc* get_m_pBASE_SESSION()
        {
            return m_pSCRIPT_SESSION_CLASS;
        }
    public:
        // <summary>
        // IConfirmInteractionTarget
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_ConfirmInteractionTarget(IConfirmInteractionTarget __gc* value);
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
        __property System::UInt16 get_NrOfDicomScripts();

	public:
        // <summary>
        // Pending Data In Network Input Buffer
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property bool get_HasPendingDataInNetworkInputBuffer();

	public:
        // <summary>
        // UseSecureSockets
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property virtual bool get_IsDataTransferExplicit();
    public:
        // <summary>
        // UseSecureSockets
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property virtual void set_IsDataTransferExplicit(bool value);

    public:
        ReceiveReturnCode Receive([Out] DvtkData::Message __gc*__gc* ppMessage);
    public:
        ReceiveReturnCode Receive([Out] DvtkData::Dul::DulMessage __gc*__gc* ppDulMessage);
    public:
        ReceiveReturnCode Receive([Out] DvtkData::Dimse::DicomMessage __gc*__gc* ppDicomMessage);
    public:
        SendReturnCode Send(DvtkData::Message __gc* pMessage);
	public:
		SendReturnCode Send(DvtkData::Dimse::DicomMessage __gc* pMessage, int pcId);
    public:
        bool Validate(
            DvtkData::Dimse::DicomMessage __gc* pMessage,
            DvtkData::Dimse::DicomMessage __gc* pReferenceMessage,
            Wrappers::ValidationControlFlags validationControlFlags);
	public:
        bool Validate(
            DvtkData::Dimse::DicomMessage __gc* pMessage,
            DvtkData::Dimse::DicomMessage __gc* pReferenceMessage,
			DvtkData::Dimse::DicomMessage __gc* pLastMessage,
            Wrappers::ValidationControlFlags validationControlFlags);
    public:
        bool Validate(
            DvtkData::Dul::DulMessage __gc* pMessage,
            DvtkData::Dul::DulMessage __gc* pReferenceMessage,
            Wrappers::ValidationControlFlags validationControlFlags);

	public:
        bool ComparePixelData(
            DvtkData::Dimse::DicomMessage __gc* pMessage,
            DvtkData::Dimse::DicomMessage __gc* pReferenceMessage);

    private:
        // Track whether Dispose has been called.
        // m_pSCRIPT_SESSION_CLASS is treated as disposable resource.
        bool disposed;

    public:
        void Dispose();

    private:
        void Dispose(bool disposing);
    };
}
