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
        m_pConfirmInteractionAdapter = NULL;
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
        if (m_pConfirmInteractionAdapter != NULL) delete m_pConfirmInteractionAdapter;
        // Do not re-create Dispose clean-up code here.
        Dispose(false);
        return;
    }

    // Implement IDisposable*
    // Do not make this method virtual.
    // A derived class should not be able to this method.
    void MScriptSession::Dispose() {
        Dispose(true);
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
    void MScriptSession::Dispose(bool disposing) {
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
            catch (System::NullReferenceException __gc*)
            {
                m_pSCRIPT_SESSION_CLASS = NULL;
            }
        }
        disposed = true;
    }

    void MScriptSession::set_ConfirmInteractionTarget(IConfirmInteractionTarget __gc* value)
    {
        this->m_pConfirmInteractionAdapter = new ConfirmInteractionAdapter(value);
        this->m_pSCRIPT_SESSION_CLASS->setConfirmer(this->m_pConfirmInteractionAdapter);
    }

    System::UInt16 MScriptSession::get_NrOfDicomScripts()
    {
        return /*cast to hide warning*/(System::UInt16)m_pSCRIPT_SESSION_CLASS->noDicomScripts();
    }

	bool MScriptSession::get_HasPendingDataInNetworkInputBuffer()
    {
        return /*cast to hide warning*/(bool)m_pSCRIPT_SESSION_CLASS->getHasPendingDataInNetworkInputBuffer();
    }

	bool MScriptSession::get_IsDataTransferExplicit()
    {
        return m_isDataTransferExplicit;
    }

	void MScriptSession::set_IsDataTransferExplicit(bool isTransferExplicit)
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
        MScriptSession::Receive([Out] DvtkData::Dimse::DicomMessage __gc*__gc* ppDicomMessage)
    {
        DvtkData::Message __gc* pMessage = NULL;
        Wrappers::ReceiveReturnCode retVal = Receive(&pMessage);
        if (pMessage == NULL)
        {
            *ppDicomMessage = NULL;
        }
        else if (__typeof(DvtkData::Dimse::DicomMessage)->IsInstanceOfType(pMessage))
        {
            *ppDicomMessage = static_cast<DvtkData::Dimse::DicomMessage __gc*>(pMessage);
        }
        else if (__typeof(DvtkData::Dul::DulMessage)->IsInstanceOfType(pMessage))
        {
            if (retVal == Wrappers::ReceiveReturnCode::Success) 
                retVal = Wrappers::ReceiveReturnCode::Failure;
            *ppDicomMessage = NULL;
        }
        else
        {
            throw new System::ApplicationException();
        }
        return retVal;
    }

    Wrappers::ReceiveReturnCode
        MScriptSession::Receive([Out] DvtkData::Dul::DulMessage __gc*__gc* ppDulMessage)
    {
        DvtkData::Message __gc* pMessage = NULL;
        Wrappers::ReceiveReturnCode retVal = Receive(&pMessage);
        if (pMessage == NULL)
        {
            *ppDulMessage = NULL;
        }
        else if (__typeof(DvtkData::Dul::DulMessage)->IsInstanceOfType(pMessage))
        {
            *ppDulMessage = static_cast<DvtkData::Dul::DulMessage __gc*>(pMessage);
        }
        else if (__typeof(DvtkData::Dimse::DicomMessage)->IsInstanceOfType(pMessage))
        {
            if (retVal == Wrappers::ReceiveReturnCode::Success) 
                retVal = Wrappers::ReceiveReturnCode::Failure;
            *ppDulMessage = NULL;
        }
        else
        {
            throw new System::ApplicationException();
        }
        return retVal;
    }

    Wrappers::ReceiveReturnCode
        MScriptSession::Receive([Out] DvtkData::Message __gc*__gc* ppMessage)
    {
        RECEIVE_MESSAGE_UNION_CLASS* pRECEIVE_MESSAGE_UNION_CLASS = NULL;
        RECEIVE_MSG_ENUM retVal = 
            m_pSCRIPT_SESSION_CLASS->receive(&pRECEIVE_MESSAGE_UNION_CLASS);

        if (pRECEIVE_MESSAGE_UNION_CLASS != NULL) 
        {
            switch (pRECEIVE_MESSAGE_UNION_CLASS->getRxMsgType())
            {
            case ::RX_MSG_FAILURE:
                *ppMessage = NULL;
                break;
            case ::RX_MSG_ASSOCIATE_REQUEST:
                *ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAssociateRequest());
                break;
            case ::RX_MSG_ASSOCIATE_ACCEPT:
                *ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAssociateAccept());
                break;
            case ::RX_MSG_ASSOCIATE_REJECT:
                *ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAssociateReject());
                break;
            case ::RX_MSG_RELEASE_REQUEST:
                *ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getReleaseRequest());
                break;
            case ::RX_MSG_RELEASE_RESPONSE:
                *ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getReleaseResponse());
                break;
            case ::RX_MSG_ABORT_REQUEST:
                *ppMessage = 
                    ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                    pRECEIVE_MESSAGE_UNION_CLASS->getAbortRequest());
                break;
            case ::RX_MSG_DICOM_COMMAND:
                {
                    DvtkData::Dimse::DicomMessage __gc* pDicomMessage = 
                        new DvtkData::Dimse::DicomMessage();
                    pDicomMessage->CommandSet =
                        ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                        pRECEIVE_MESSAGE_UNION_CLASS->getCommand());

					if(pRECEIVE_MESSAGE_UNION_CLASS->getCommand() != NULL)
						pDicomMessage->EncodedPresentationContextID = (pRECEIVE_MESSAGE_UNION_CLASS->getCommand()->getEncodePresentationContextId());

                    pDicomMessage->DataSet = NULL;
                    *ppMessage = pDicomMessage;
                }
                break;
            case ::RX_MSG_DICOM_COMMAND_DATASET:
                {
                    DvtkData::Dimse::DicomMessage __gc* pDicomMessage =
                        new DvtkData::Dimse::DicomMessage();
                    pDicomMessage->CommandSet =
                        ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                        pRECEIVE_MESSAGE_UNION_CLASS->getCommand());
                    pDicomMessage->DataSet =
                        ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                        pRECEIVE_MESSAGE_UNION_CLASS->getDataset());

					if(pRECEIVE_MESSAGE_UNION_CLASS->getDataset() != NULL)
						pDicomMessage->EncodedPresentationContextID = (pRECEIVE_MESSAGE_UNION_CLASS->getDataset()->getEncodePresentationContextId());
                    
					*ppMessage = pDicomMessage;
                }
                break;
            default:
                {
                    System::Diagnostics::Trace::Assert(false);
                    *ppMessage = NULL;
                }
                break;
            }
        }
        else
        {
            *ppMessage = NULL;
        }
        return _Convert(retVal);
    }

    Wrappers::SendReturnCode 
        MScriptSession::Send(DvtkData::Message __gc* pMessage)
    {
        if (pMessage == NULL) return Wrappers::SendReturnCode::Failure;

        Wrappers::SendReturnCode retVal;
        System::Type __gc* pMessageType = pMessage->GetType();
        if (pMessageType == __typeof(DvtkData::Dimse::DicomMessage))
        {
            DvtkData::Dimse::DicomMessage __gc* pDicomMessage =
                static_cast<DvtkData::Dimse::DicomMessage __gc*>(pMessage);
            DCM_COMMAND_CLASS *pCommand = NULL;
            DCM_DATASET_CLASS *pDataSet = NULL;
            if (pDicomMessage->CommandSet != NULL)
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
                throw new System::ApplicationException();
            }
            if (pDicomMessage->DataSet != NULL)
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
            if (pDataSet == NULL)
            {
                retBoolValue = m_pSCRIPT_SESSION_CLASS->send(pCommand);
            }
            else
            {
                retBoolValue = m_pSCRIPT_SESSION_CLASS->send(pCommand, pDataSet);
            }
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RQ))
        {
            ASSOCIATE_RQ_CLASS *pAssociateRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RQ __gc*>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAssociateRq, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_AC))
        {
            ASSOCIATE_AC_CLASS *pAssociateAc =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_AC __gc*>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAssociateAc, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RJ))
        {
            ASSOCIATE_RJ_CLASS *pAssociateRj =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RJ __gc*>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAssociateRj, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == __typeof(DvtkData::Dul::A_RELEASE_RQ))
        {
            RELEASE_RQ_CLASS *pReleaseRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RQ __gc*>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pReleaseRq, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == __typeof(DvtkData::Dul::A_RELEASE_RP))
        {
            RELEASE_RP_CLASS *pReleaseRp =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RP __gc*>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pReleaseRp, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == __typeof(DvtkData::Dul::A_ABORT))
        {
            ABORT_RQ_CLASS *pAbortRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ABORT __gc*>(pMessage));
            bool retBoolValue =
                m_pSCRIPT_SESSION_CLASS->send(pAbortRq, "");
            retVal = (retBoolValue ? Wrappers::SendReturnCode::Success : Wrappers::SendReturnCode::Failure);
        }
        else if (pMessageType == __typeof(DvtkData::Dul::P_DATA_TF))
        {
            throw new System::NotSupportedException();
        }
        else
        {
            System::Diagnostics::Trace::Assert(false);
            retVal = Wrappers::SendReturnCode::Failure;
        }
        return retVal;
    }

	Wrappers::SendReturnCode 
		MScriptSession::Send(DvtkData::Dimse::DicomMessage __gc* pDicomMessage, int pcId)
    {
        if (pDicomMessage == NULL) return Wrappers::SendReturnCode::Failure;

        Wrappers::SendReturnCode retVal;
        
        DCM_COMMAND_CLASS *pCommand = NULL;
        DCM_DATASET_CLASS *pDataSet = NULL;
        if (pDicomMessage->CommandSet != NULL)
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
            throw new System::ApplicationException();
        }
        if (pDicomMessage->DataSet != NULL)
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
        if (pDataSet == NULL)
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
        DvtkData::Dimse::DicomMessage __gc* pMessage, 
        DvtkData::Dimse::DicomMessage __gc* pReferenceMessage,
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        DCM_COMMAND_CLASS *pUMCommand = NULL;
        DCM_DATASET_CLASS *pUMDataSet = NULL;
        DCM_COMMAND_CLASS *pUMReferenceCommand = NULL;
        DCM_DATASET_CLASS *pUMReferenceDataSet = NULL;
        if (pMessage == NULL) throw new System::ArgumentNullException();
        if (pMessage != NULL && pMessage->CommandSet != NULL)
        {
            pUMCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->CommandSet);
			pUMCommand->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMCommand != NULL) && (m_isDataTransferExplicit))
			{
				pUMCommand->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);				
			}
        }
        if (pMessage != NULL && pMessage->DataSet != NULL)
        {
            pUMDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->DataSet);
			pUMDataSet->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMDataSet != NULL) && (m_isDataTransferExplicit))
				pUMDataSet->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
        }
        if (pReferenceMessage != NULL && pReferenceMessage->CommandSet != NULL)
        {
            pUMReferenceCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->CommandSet);
        }
        if (pReferenceMessage != NULL && pReferenceMessage->DataSet != NULL)
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
        if (pMessage->DataSet != NULL)
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
        DvtkData::Dimse::DicomMessage __gc* pMessage, 
        DvtkData::Dimse::DicomMessage __gc* pReferenceMessage,
		DvtkData::Dimse::DicomMessage __gc* pLastMessage,
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        DCM_COMMAND_CLASS *pUMCommand = NULL;
        DCM_DATASET_CLASS *pUMDataSet = NULL;
        DCM_COMMAND_CLASS *pUMReferenceCommand = NULL;
        DCM_DATASET_CLASS *pUMReferenceDataSet = NULL;
		DCM_COMMAND_CLASS *pUMLastCommand = NULL;
        DCM_DATASET_CLASS *pUMLastDataSet = NULL;

        if (pMessage == NULL) throw new System::ArgumentNullException();
        if (pMessage != NULL && pMessage->CommandSet != NULL)
        {
            pUMCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->CommandSet);
			pUMCommand->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMCommand != NULL) && (m_isDataTransferExplicit))
				pUMCommand->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
        }
        if (pMessage != NULL && pMessage->DataSet != NULL)
        {
            pUMDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->DataSet);
			pUMDataSet->setEncodePresentationContextId(pMessage->EncodedPresentationContextID);
			if((pUMDataSet != NULL) && (m_isDataTransferExplicit))
				pUMDataSet->setTransferVR(TRANSFER_ATTR_VR_EXPLICIT);
        }
        if (pReferenceMessage != NULL && pReferenceMessage->CommandSet != NULL)
        {
            pUMReferenceCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->CommandSet);
        }
        if (pReferenceMessage != NULL && pReferenceMessage->DataSet != NULL)
        {
            pUMReferenceDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->DataSet);
        }
		if (pLastMessage != NULL && pLastMessage->CommandSet != NULL)
        {
            pUMLastCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pLastMessage->CommandSet);
        }
        if (pLastMessage != NULL && pLastMessage->DataSet != NULL)
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
        if (pMessage->DataSet != NULL)
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
        DvtkData::Dul::DulMessage __gc* pMessage, 
        DvtkData::Dul::DulMessage __gc* pReferenceMessage,
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        bool success = false;
        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);
        System::Type __gc* pMessageType = pMessage->GetType();
        //
        // Convert and validate
        //
        System::Type __gc* pReferenceMessageType = NULL;
        if (pReferenceMessage != NULL) pReferenceMessageType = pReferenceMessage->GetType();

        if (
            pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RQ) &&
            (
                pReferenceMessageType == NULL ||
                pReferenceMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RQ)
            )
            )
        {
            ASSOCIATE_RQ_CLASS *pAssociateRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RQ __gc*>(pMessage));
            ASSOCIATE_RQ_CLASS *pReferenceAssociateRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RQ __gc*>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAssociateRq,
                pReferenceAssociateRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_AC) &&
            (
                pReferenceMessageType == NULL ||
                pReferenceMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_AC)
            )
            )
        {
            ASSOCIATE_AC_CLASS *pAssociateAc =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_AC __gc*>(pMessage));
            ASSOCIATE_AC_CLASS *pReferenceAssociateAc =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_AC __gc*>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAssociateAc,
                pReferenceAssociateAc,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RJ) &&
            (
                pReferenceMessageType == NULL ||
                pReferenceMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RJ)
            )
            )
        {
            ASSOCIATE_RJ_CLASS *pAssociateRj =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RJ __gc*>(pMessage));
            ASSOCIATE_RJ_CLASS *pReferenceAssociateRj =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RJ __gc*>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAssociateRj,
                pReferenceAssociateRj,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_RELEASE_RQ) &&
            (
                pReferenceMessageType == NULL ||
                pReferenceMessageType == __typeof(DvtkData::Dul::A_RELEASE_RQ)
            )
            )
        {
            RELEASE_RQ_CLASS *pReleaseRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RQ __gc*>(pMessage));
            RELEASE_RQ_CLASS *pReferenceReleaseRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RQ __gc*>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pReleaseRq,
                pReferenceReleaseRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_RELEASE_RP) &&
            (
                pReferenceMessageType == NULL ||
                pReferenceMessageType == __typeof(DvtkData::Dul::A_RELEASE_RP)
            )
            )
        {
            RELEASE_RP_CLASS *pReleaseRp =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RP __gc*>(pMessage));
            RELEASE_RP_CLASS *pReferenceReleaseRp =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RP __gc*>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pReleaseRp,
                pReferenceReleaseRp,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_ABORT) &&
            (
                pReferenceMessageType == NULL ||
                pReferenceMessageType == __typeof(DvtkData::Dul::A_ABORT)
            )
            )
        {
            ABORT_RQ_CLASS *pAbortRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ABORT __gc*>(pMessage));
            ABORT_RQ_CLASS *pReferenceAbortRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ABORT __gc*>(pReferenceMessage));
            success = 
                m_pSCRIPT_SESSION_CLASS->validate(
                pAbortRq,
                pReferenceAbortRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::P_DATA_TF) &&
            (
                pReferenceMessageType == NULL ||
                pReferenceMessageType == __typeof(DvtkData::Dul::P_DATA_TF)
            )
            )
        {
            throw new System::NotSupportedException();
        }
        else
        {
            System::Diagnostics::Trace::Assert(false);
        }
        return success;
    }

	bool MScriptSession::ComparePixelData(
        DvtkData::Dimse::DicomMessage __gc* pMessage, 
        DvtkData::Dimse::DicomMessage __gc* pReferenceMessage)
    {
        DCM_DATASET_CLASS *pUMDataSet = NULL;
        DCM_DATASET_CLASS *pUMReferenceDataSet = NULL;
        if (pMessage == NULL) throw new System::ArgumentNullException();
        
        if (pMessage != NULL && pMessage->DataSet != NULL)
        {
            pUMDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->DataSet);
        }
        if (pReferenceMessage != NULL && pReferenceMessage->DataSet != NULL)
        {
            pUMReferenceDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pReferenceMessage->DataSet);
        }
        
        bool success = false;
        if (pMessage->DataSet != NULL)
        {
            success =
                m_pSCRIPT_SESSION_CLASS->comparePixelData(pUMDataSet, 
													pUMReferenceDataSet);
        }
        
        return success;
    }
}