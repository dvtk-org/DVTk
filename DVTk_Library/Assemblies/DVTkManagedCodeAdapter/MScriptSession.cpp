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
#include ".\mscriptsession.h"
#include < vcclr.h >
#include "mdimseconvertors.h"
#include "mdulconvertors.h"
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;
    using namespace ManagedUnManagedDimseConvertors;
    using namespace ManagedUnManagedDulConvertors;

    MScriptSession::MScriptSession(void)
    {
        m_pConfirmInteractionAdapter = nullptr;
		m_isDataTransferExplicit = false;
        m_pSCRIPT_SESSION_CLASS = new SCRIPT_SESSION_CLASS();
        bool definitionFileLoaded = true;
        //
        // initialize this session before we begin the underlying unmanaged session
        //
        this->Initialize();

        //
        // We need to invoke the begin() otherwise no association class is created.
        //
        bool success = false;
        success = m_pSCRIPT_SESSION_CLASS->begin(definitionFileLoaded);
        disposed = false;
        return;
    }
    // Use C# destructor syntax for finalization code.
    // This destructor will run only if the Dispose method
    // does not get called.
    // It gives your base class the opportunity to finalize.
    // Do not provide destructors in types derived from this class.
    MScriptSession::~MScriptSession(void)
    {
        if (m_pConfirmInteractionAdapter != nullptr) delete m_pConfirmInteractionAdapter;
        // Do not re-create Dispose clean-up code here.
        DisposeThis(false);
        return;
    }

    // Implement IDisposable*
    // Do not make this method virtual.
    // A derived class should not be able to this method.
    void MScriptSession::DisposeThis() {
        DisposeThis(true);
        // This object will be cleaned up by the Dispose method.
        // Therefore, you should call GC::SupressFinalize to
        // take this object off the finalization queue
        // and prevent finalization code for this object
        // from executing a second time.
        System::GC::SuppressFinalize(this);
    }

    // Dispose(bool disposing) executes in two distinct scenarios.
    // If disposing equals true, the method has been called directly
    // or indirectly by a user's code. Managed and unmanaged resources
    // can be disposed.
    // If disposing equals false, the method has been called by the
    // runtime from inside the finalizer and you should not reference
    // other objects. Only unmanaged resources can be disposed.
    void MScriptSession::DisposeThis(bool disposing) {
        // If disposing equals true, dispose all managed
        // and unmanaged resources.
        if (disposing) {
            // Dispose managed resources.
        }
        // Check to see if Dispose has already been called.
        if (!this->disposed) {
            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.
            try
            {
                delete m_pSCRIPT_SESSION_CLASS;
                m_pSCRIPT_SESSION_CLASS = NULL;
            }
            catch (System::NullReferenceException^)
            {
                m_pSCRIPT_SESSION_CLASS = NULL;
            }
        }
        disposed = true;
    }

    void MScriptSession::ConfirmInteractionTarget::set(IConfirmInteractionTarget^ value)
    {
        this->m_pConfirmInteractionAdapter = new ConfirmInteractionAdapter(value);
        this->m_pSCRIPT_SESSION_CLASS->setConfirmer(this->m_pConfirmInteractionAdapter);
    }

    System::UInt16 MScriptSession::NrOfDicomScripts::get()
    {
        return /*cast to hide warning*/(System::UInt16)m_pSCRIPT_SESSION_CLASS->noDicomScripts();
    }

	bool MScriptSession::HasPendingDataInNetworkInputBuffer::get()
    {
        return /*cast to hide warning*/(bool)m_pSCRIPT_SESSION_CLASS->getHasPendingDataInNetworkInputBuffer();
    }

	bool MScriptSession::IsDataTransferExplicit::get()
    {
        return m_isDataTransferExplicit;
    }

	void MScriptSession::IsDataTransferExplicit::set(bool isTransferExplicit)
    {
        m_isDataTransferExplicit = isTransferExplicit;
    }

    static inline Wrappers::ReceiveReturnCode _Convert(RECEIVE_MSG_ENUM value)
    {
        switch (value)
        {
        case ::RECEIVE_MSG_SUCCESSFUL:          return Wrappers::ReceiveReturnCode::Success;
        case ::RECEIVE_MSG_FAILURE:             return Wrappers::ReceiveReturnCode::Failure;
        case ::RECEIVE_MSG_ASSOC_REJECTED:      return Wrappers::ReceiveReturnCode::AssociationRejected;
        case ::RECEIVE_MSG_ASSOC_RELEASED:      return Wrappers::ReceiveReturnCode::AssociationReleased;
        case ::RECEIVE_MSG_ASSOC_ABORTED:       return Wrappers::ReceiveReturnCode::AssociationAborted;
        case ::RECEIVE_MSG_CONNECTION_CLOSED:   return Wrappers::ReceiveReturnCode::SocketClosed;
        case ::RECEIVE_MSG_NO_CONNECTION:       return Wrappers::ReceiveReturnCode::NoSocketConnection;
        default:
            System::Diagnostics::Trace::Assert(false);
            return Wrappers::ReceiveReturnCode::Failure;
        }
    }

    Wrappers::ReceiveReturnCode
        MScriptSession::Receive([Out] DvtkData::Dimse::DicomMessage^ % ppDicomMessage)
    {
        DvtkData::Message^ pMessage = nullptr;
        Wrappers::ReceiveReturnCode retVal = Receive(pMessage);
        if (pMessage == nullptr)
        {
            ppDicomMessage = nullptr;
        }
        else if (DvtkData::Dimse::DicomMessage::typeid->IsInstanceOfType(pMessage))
        {
            ppDicomMessage = static_cast<DvtkData::Dimse::DicomMessage^>(pMessage);
        }
        else if ((DvtkData::Dul::DulMessage::typeid)->IsInstanceOfType(pMessage))
        {
            if (retVal == Wrappers::ReceiveReturnCode::Success) 
                retVal = Wrappers::ReceiveReturnCode::Failure;
            ppDicomMessage = nullptr;
        }
        else
        {
            throw gcnew System::ApplicationException();
        }
        return retVal;
    }

    Wrappers::ReceiveReturnCode
        MScriptSession::Receive([Out] DvtkData::Dul::DulMessage^ % ppDulMessage)
    {
        DvtkData::Message^ pMessage = nullptr;
        Wrappers::ReceiveReturnCode retVal = Receive(pMessage);
        if (pMessage == nullptr)
        {
            ppDulMessage = nullptr;
        }
        else if ((DvtkData::Dul::DulMessage::typeid)->IsInstanceOfType(pMessage))
        {
            ppDulMessage = static_cast<DvtkData::Dul::DulMessage^>(pMessage);
        }
        else if ((DvtkData::Dimse::DicomMessage::typeid)->IsInstanceOfType(pMessage))
        {
            if (retVal == Wrappers::ReceiveReturnCode::Success) 
                retVal = Wrappers::ReceiveReturnCode::Failure;
            ppDulMessage = nullptr;
        }
        else
        {
            throw gcnew System::ApplicationException();
        }
        return retVal;
    }

    Wrappers::ReceiveReturnCode
        MScriptSession::Receive([Out] DvtkData::Message^ % ppMessage)
    {
        RECEIVE_MESSAGE_UNION_CLASS* pRECEIVE_MESSAGE_UNION_CLASS = nullptr;
        RECEIVE_MSG_ENUM retVal = 
            m_pSCRIPT_SESSION_CLASS->receive(&pRECEIVE_MESSAGE_UNION_CLASS);

        if (pRECEIVE_MESSAGE_UNION_CLASS != nullptr) 
        {
            switch (pRECEIVE_MESSAGE_UNION_CLASS->getRxMsgType())
            {
            case ::RX_MSG_FAILURE:
                ppMessage = nullptr;
                break;
            case ::RX_MSG_ASSOCIATE_REQUEST:
                ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAssociateRequest());
                break;
            case ::RX_MSG_ASSOCIATE_ACCEPT:
                ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAssociateAccept());
                break;
            case ::RX_MSG_ASSOCIATE_REJECT:
                ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAssociateReject());
                break;
            case ::RX_MSG_RELEASE_REQUEST:
                ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getReleaseRequest());
                break;
            case ::RX_MSG_RELEASE_RESPONSE:
                ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getReleaseResponse());
                break;
            case ::RX_MSG_ABORT_REQUEST:
                ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAbortRequest());
                break;
            case ::RX_MSG_DICOM_COMMAND:
                {
                    DvtkData::Dimse::DicomMessage^ pDicomMessage = 
                        gcnew DvtkData::Dimse::DicomMessage();
                    pDicomMessage->CommandSet =
                        ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                        pRECEIVE_MESSAGE_UNION_CLASS->getCommand());

					if(pRECEIVE_MESSAGE_UNION_CLASS->getCommand() != nullptr)
						pDicomMessage->EncodedPresentationContextID = (pRECEIVE_MESSAGE_UNION_CLASS->getCommand()->getEncodePresentationContextId());

                    pDicomMessage->DataSet = nullptr;
                    ppMessage = pDicomMessage;
                }
                break;
            case ::RX_MSG_DICOM_COMMAND_DATASET:
                {
                    DvtkData::Dimse::DicomMessage^ pDicomMessage =
                        gcnew DvtkData::Dimse::DicomMessage();
                    pDicomMessage->CommandSet =
                        ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                        pRECEIVE_MESSAGE_UNION_CLASS->getCommand());
                    pDicomMessage->DataSet =
                        ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                        pRECEIVE_MESSAGE_UNION_CLASS->getDataset());

					if(pRECEIVE_MESSAGE_UNION_CLASS->getDataset() != nullptr)
						pDicomMessage->EncodedPresentationContextID = (pRECEIVE_MESSAGE_UNION_CLASS->getDataset()->getEncodePresentationContextId());
                    
					ppMessage = pDicomMessage;
                }
                break;
            default:
                {
                    System::Diagnostics::Trace::Assert(false);
                    ppMessage = nullptr;
                }
                break;
            }
        }
        else
        {
            ppMessage = nullptr;
        }
        return _Convert(retVal);
    }

    Wrappers::SendReturnCode 
        MScriptSession::Send(DvtkData::Message^ pMessage)
    {
        if (pMessage == nullptr) return Wrappers::SendReturnCode::Failure;

        Wrappers::SendReturnCode retVal;
        System::Type^ pMessageType = pMessage->GetType();
        if (pMessageType == DvtkData::Dimse::DicomMessage::typeid)
        {
            DvtkData::Dimse::DicomMessage^ pDicomMessage =
                static_cast<DvtkData::Dimse::DicomMessage^>(pMessage);
            DCM_COMMAND_CLASS *pCommand = nullptr;
            DCM_DATASET_CLASS *pDataSet = nullptr;
            if (pDicomMessage->CommandSet != nullptr)
            {
                pCommand =
                    ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                    pDicomMessage->CommandSet);
                if (pCommand)
                {
                    // set the logger
                    pCommand->setLogger(m_pSCRIPT_SESSION_CLASS->getLogger());
                }
            }
            else
            {
                throw gcnew System::ApplicationException();
            }
            if (pDicomMessage->DataSet != nullptr)
            {
                pDataSet =
                    ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                    pDicomMessage->DataSet);
                if ((pCommand) &&
                    (pDataSet))
                {
                    // set the command id and logger
                    pDataSet->setCommandId(pCommand->getCommandId());
                    pDataSet->setLogger(m_pSCRIPT_SESSION_CLASS->getLogger());
                }
            }
            bool retBoolValue = false;
            if (pDataSet == nullptr)
            {
                retBoolValue = m_pSCRIPT_SESSION_CLASS->send(pCommand);
            }
            else
            {
                retBoolValue = m_pSCRIPT_SESSION_CLASS->send(pCommand, pDataSet);
            }
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == DvtkData::Dul::A_ASSOCIATE_RQ::typeid)
        {
            ASSOCIATE_RQ_CLASS *pAssociateRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RQ^>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAssociateRq, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == DvtkData::Dul::A_ASSOCIATE_AC::typeid)
        {
            ASSOCIATE_AC_CLASS *pAssociateAc =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_AC^>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAssociateAc, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == DvtkData::Dul::A_ASSOCIATE_RJ::typeid)
        {
            ASSOCIATE_RJ_CLASS *pAssociateRj =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RJ^>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAssociateRj, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == DvtkData::Dul::A_RELEASE_RQ::typeid)
        {
            RELEASE_RQ_CLASS *pReleaseRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RQ^>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pReleaseRq, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == DvtkData::Dul::A_RELEASE_RP::typeid)
        {
            RELEASE_RP_CLASS *pReleaseRp =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RP^>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pReleaseRp, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == DvtkData::Dul::A_ABORT::typeid)
        {
            ABORT_RQ_CLASS *pAbortRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ABORT^>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAbortRq, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == DvtkData::Dul::P_DATA_TF::typeid)
        {
            throw gcnew System::NotSupportedException();
        }
        else
        {
            System::Diagnostics::Trace::Assert(false);
            retVal = Wrappers::SendReturnCode::Failure;
        }
        return retVal;
    }

	Wrappers::SendReturnCode 
		MScriptSession::Send(DvtkData::Dimse::DicomMessage^ pDicomMessage, int pcId)
    {
        if (pDicomMessage == nullptr) return Wrappers::SendReturnCode::Failure;

        Wrappers::SendReturnCode retVal;
        
        DCM_COMMAND_CLASS *pCommand = nullptr;
        DCM_DATASET_CLASS *pDataSet = nullptr;
        if (pDicomMessage->CommandSet != nullptr)
        {
            pCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pDicomMessage->CommandSet);
            if (pCommand)
            {
                // set the logger
                pCommand->setLogger(m_pSCRIPT_SESSION_CLASS->getLogger());
            }
        }
        else
        {
            throw gcnew System::ApplicationException();
        }
        if (pDicomMessage->DataSet != nullptr)
        {
            pDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pDicomMessage->DataSet);
            if ((pCommand) &&
                (pDataSet))
            {
                // set the command id and logger
                pDataSet->setCommandId(pCommand->getCommandId());
                pDataSet->setLogger(m_pSCRIPT_SESSION_CLASS->getLogger());
            }
        }
        bool retBoolValue = false;
        if (pDataSet == nullptr)
        {
            retBoolValue = m_pSCRIPT_SESSION_CLASS->send(pCommand, pcId);
        }
        else
        {
            retBoolValue = m_pSCRIPT_SESSION_CLASS->send(pCommand, pDataSet, pcId);
        }
        retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        
        return retVal;
	}

    bool MScriptSession::Validate(
        DvtkData::Dimse::DicomMessage^ pMessage, 
        DvtkData::Dimse::DicomMessage^ pReferenceMessage,
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        DCM_COMMAND_CLASS *pUMCommand = nullptr;
        DCM_DATASET_CLASS *pUMDataSet = nullptr;
        DCM_COMMAND_CLASS *pUMReferenceCommand = nullptr;
        DCM_DATASET_CLASS *pUMReferenceDataSet = nullptr;
        if (pMessage == nullptr) throw gcnew System::ArgumentNullException();
        if (pMessage != nullptr && pMessage->CommandSet != nullptr)
        {
            pUMCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->CommandSet);
			pUMCommand->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMCommand != nullptr) && (m_isDataTransferExplicit))
			{
				pUMCommand->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);				
			}
        }
        if (pMessage != nullptr && pMessage->DataSet != nullptr)
        {
            pUMDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->DataSet);
			pUMDataSet->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMDataSet != nullptr) && (m_isDataTransferExplicit))
				pUMDataSet->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
        }
        if (pReferenceMessage != nullptr && pReferenceMessage->CommandSet != nullptr)
        {
            pUMReferenceCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->CommandSet);
        }
        if (pReferenceMessage != nullptr && pReferenceMessage->DataSet != nullptr)
        {
            pUMReferenceDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->DataSet);
        }
        AE_SESSION_CLASS ae_session;
        // set ae session
        ae_session.SetName(m_pSCRIPT_SESSION_CLASS->getApplicationEntityName());
        ae_session.SetVersion(m_pSCRIPT_SESSION_CLASS->getApplicationEntityVersion());

        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);

        bool success = false;
        if (pMessage->DataSet != nullptr)
        {
            success =
                m_pSCRIPT_SESSION_CLASS->validate(
                pUMCommand, 
                pUMDataSet, 
                pUMReferenceCommand, 
                pUMReferenceDataSet,
                umValidationControlFlags,
                &ae_session);
        }
        else
        {
            success =
                m_pSCRIPT_SESSION_CLASS->validate(
                pUMCommand, 
                pUMReferenceCommand,
                umValidationControlFlags);
        }
        return success;
    }

	bool MScriptSession::Validate(
        DvtkData::Dimse::DicomMessage^ pMessage, 
        DvtkData::Dimse::DicomMessage^ pReferenceMessage,
		DvtkData::Dimse::DicomMessage^ pLastMessage,
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        DCM_COMMAND_CLASS *pUMCommand = nullptr;
        DCM_DATASET_CLASS *pUMDataSet = nullptr;
        DCM_COMMAND_CLASS *pUMReferenceCommand = nullptr;
        DCM_DATASET_CLASS *pUMReferenceDataSet = nullptr;
		DCM_COMMAND_CLASS *pUMLastCommand = nullptr;
        DCM_DATASET_CLASS *pUMLastDataSet = nullptr;

        if (pMessage == nullptr) throw gcnew System::ArgumentNullException();
        if (pMessage != nullptr && pMessage->CommandSet != nullptr)
        {
            pUMCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->CommandSet);
			pUMCommand->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMCommand != nullptr) && (m_isDataTransferExplicit))
				pUMCommand->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
        }
        if (pMessage != nullptr && pMessage->DataSet != nullptr)
        {
            pUMDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->DataSet);
			pUMDataSet->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMDataSet != nullptr) && (m_isDataTransferExplicit))
				pUMDataSet->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
        }
        if (pReferenceMessage != nullptr && pReferenceMessage->CommandSet != nullptr)
        {
            pUMReferenceCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->CommandSet);
        }
        if (pReferenceMessage != nullptr && pReferenceMessage->DataSet != nullptr)
        {
            pUMReferenceDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->DataSet);
        }
		if (pLastMessage != nullptr && pLastMessage->CommandSet != nullptr)
        {
            pUMLastCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pLastMessage->CommandSet);
        }
        if (pLastMessage != nullptr && pLastMessage->DataSet != nullptr)
        {
            pUMLastDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pLastMessage->DataSet);
        }
        AE_SESSION_CLASS ae_session;
        // set ae session
        ae_session.SetName(m_pSCRIPT_SESSION_CLASS->getApplicationEntityName());
        ae_session.SetVersion(m_pSCRIPT_SESSION_CLASS->getApplicationEntityVersion());

        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);

        bool success = false;
        if (pMessage->DataSet != nullptr)
        {
            success =
                m_pSCRIPT_SESSION_CLASS->validate(
                pUMCommand, 
                pUMDataSet, 
                pUMReferenceCommand, 
                pUMReferenceDataSet,
				pUMLastCommand,
				pUMLastDataSet,
                umValidationControlFlags,
                &ae_session);
        }
        else
        {
            success =
                m_pSCRIPT_SESSION_CLASS->validate(
                pUMCommand, 
                pUMReferenceCommand,
				pUMLastCommand,
                umValidationControlFlags);
        }
        return success;
    }

    bool MScriptSession::Validate(
        DvtkData::Dul::DulMessage^ pMessage, 
        DvtkData::Dul::DulMessage^ pReferenceMessage,
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        bool success = false;
        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);
        System::Type^ pMessageType = pMessage->GetType();
        //
        // Convert and validate
        //
        System::Type^ pReferenceMessageType = nullptr;
        if (pReferenceMessage != nullptr) pReferenceMessageType = pReferenceMessage->GetType();

        if (
            pMessageType == DvtkData::Dul::A_ASSOCIATE_RQ::typeid &&
            (
                pReferenceMessageType == nullptr ||
                pReferenceMessageType == DvtkData::Dul::A_ASSOCIATE_RQ::typeid
            )
            )
        {
            ASSOCIATE_RQ_CLASS *pAssociateRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RQ^>(pMessage));
            ASSOCIATE_RQ_CLASS *pReferenceAssociateRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RQ^>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAssociateRq,
                pReferenceAssociateRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == DvtkData::Dul::A_ASSOCIATE_AC::typeid &&
            (
                pReferenceMessageType == nullptr ||
                pReferenceMessageType == DvtkData::Dul::A_ASSOCIATE_AC::typeid
            )
            )
        {
            ASSOCIATE_AC_CLASS *pAssociateAc =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_AC^>(pMessage));
            ASSOCIATE_AC_CLASS *pReferenceAssociateAc =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_AC^>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAssociateAc,
                pReferenceAssociateAc,
                umValidationControlFlags);
        }
        else if (
            pMessageType == DvtkData::Dul::A_ASSOCIATE_RJ::typeid &&
            (
                pReferenceMessageType == nullptr ||
                pReferenceMessageType == DvtkData::Dul::A_ASSOCIATE_RJ::typeid
            )
            )
        {
            ASSOCIATE_RJ_CLASS *pAssociateRj =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RJ^>(pMessage));
            ASSOCIATE_RJ_CLASS *pReferenceAssociateRj =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RJ^>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAssociateRj,
                pReferenceAssociateRj,
                umValidationControlFlags);
        }
        else if (
            pMessageType == DvtkData::Dul::A_RELEASE_RQ::typeid &&
            (
                pReferenceMessageType == nullptr ||
                pReferenceMessageType == DvtkData::Dul::A_RELEASE_RQ::typeid
            )
            )
        {
            RELEASE_RQ_CLASS *pReleaseRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RQ^>(pMessage));
            RELEASE_RQ_CLASS *pReferenceReleaseRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RQ^>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pReleaseRq,
                pReferenceReleaseRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == DvtkData::Dul::A_RELEASE_RP::typeid &&
            (
                pReferenceMessageType == nullptr ||
                pReferenceMessageType == DvtkData::Dul::A_RELEASE_RP::typeid
            )
            )
        {
            RELEASE_RP_CLASS *pReleaseRp =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RP^>(pMessage));
            RELEASE_RP_CLASS *pReferenceReleaseRp =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RP^>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pReleaseRp,
                pReferenceReleaseRp,
                umValidationControlFlags);
        }
        else if (
            pMessageType == DvtkData::Dul::A_ABORT::typeid &&
            (
                pReferenceMessageType == nullptr ||
                pReferenceMessageType == DvtkData::Dul::A_ABORT::typeid
            )
            )
        {
            ABORT_RQ_CLASS *pAbortRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ABORT^>(pMessage));
            ABORT_RQ_CLASS *pReferenceAbortRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ABORT^>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAbortRq,
                pReferenceAbortRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == DvtkData::Dul::P_DATA_TF::typeid &&
            (
                pReferenceMessageType == nullptr ||
                pReferenceMessageType == DvtkData::Dul::P_DATA_TF::typeid
            )
            )
        {
            throw gcnew System::NotSupportedException();
        }
        else
        {
            System::Diagnostics::Trace::Assert(false);
        }
        return success;
    }

	bool MScriptSession::ComparePixelData(
        DvtkData::Dimse::DicomMessage^ pMessage, 
        DvtkData::Dimse::DicomMessage^ pReferenceMessage)
    {
        DCM_DATASET_CLASS *pUMDataSet = nullptr;
        DCM_DATASET_CLASS *pUMReferenceDataSet = nullptr;
        if (pMessage == nullptr) throw gcnew System::ArgumentNullException();
        
        if (pMessage != nullptr && pMessage->DataSet != nullptr)
        {
            pUMDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->DataSet);
        }
        if (pReferenceMessage != nullptr && pReferenceMessage->DataSet != nullptr)
        {
            pUMReferenceDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->DataSet);
        }
        
        bool success = false;
        if (pMessage->DataSet != nullptr)
        {
            success =
                m_pSCRIPT_SESSION_CLASS->comparePixelData(pUMDataSet, 
													pUMReferenceDataSet);
        }
        
        return success;
    }
}