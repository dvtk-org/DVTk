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
#include ".\MSnifferSession.h"
#include "mdimseconvertors.h"
#include "mdulconvertors.h"
#using <mscorlib.dll>

namespace Wrappers
{
    MSnifferSession::MSnifferSession(void)
    {
        m_pSNIFFER_SESSION_CLASS = new SNIFFER_SESSION_CLASS();
        disposed = false;
        this->Initialize();
        return;
    }

    // Use C# destructor syntax for finalization code.
    // This destructor will run only if the Dispose method
    // does not get called.
    // It gives your base class the opportunity to finalize.
    // Do not provide destructors in types derived from this class.
    MSnifferSession::~MSnifferSession(void)
    {
        // Do not re-create Dispose clean-up code here.
        Dispose(false);
        return;
    }

    // Implement IDisposable*
    // Do not make this method virtual.
    // A derived class should not be able to this method.
    void MSnifferSession::Dispose() {
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
    void MSnifferSession::Dispose(bool disposing) {
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
            delete m_pSNIFFER_SESSION_CLASS;
            m_pSNIFFER_SESSION_CLASS = NULL;
        }
        disposed = true;
    }

	void MSnifferSession::ReadPDUs(System::String __gc* pduFileNames[])
    {
		vector<string> fileNameVector;
        int size = pduFileNames->Length;
        for (int idx = 0; idx < size; idx++)
        {
            System::String* value = pduFileNames[idx];
            char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
            string file = pAnsiString;
            fileNameVector.push_back(file);
            Marshal::FreeHGlobal(pAnsiString);
        }
        
		m_pSNIFFER_SESSION_CLASS->readPduFiles(&fileNameVector);
    }

	ReceivedMsgReturnCode  MSnifferSession::ReceiveMessage([Out] DvtkData::Message __gc*__gc* ppMessage)
    {
        RECEIVE_MESSAGE_UNION_CLASS* pRECEIVE_MESSAGE_UNION_CLASS = NULL;
		Wrappers::ReceivedMsgReturnCode retVal = Wrappers::ReceivedMsgReturnCode::Success;
        m_pSNIFFER_SESSION_CLASS->receive(&pRECEIVE_MESSAGE_UNION_CLASS);

        if (pRECEIVE_MESSAGE_UNION_CLASS != NULL) 
        {
            switch (pRECEIVE_MESSAGE_UNION_CLASS->getRxMsgType())
            {
            case ::RX_MSG_FAILURE:
                *ppMessage = NULL;
				retVal = Wrappers::ReceivedMsgReturnCode::Failure;
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
			case ::RX_INCOMPLETE_BYTE_STREAM:
                {
					*ppMessage = NULL;
					 retVal = Wrappers::ReceivedMsgReturnCode::IncompleteByteStream;
				}
				break;
            default:
                {
                    System::Diagnostics::Trace::Assert(false);
                    *ppMessage = NULL;
					retVal = Wrappers::ReceivedMsgReturnCode::Failure;
                }
                break;
            }
        }
        else
        {
            *ppMessage = NULL;
        }
        return retVal;
    }

  	bool MSnifferSession::Validate(
        DvtkData::Dimse::DicomMessage __gc* pMessage, 
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        DCM_COMMAND_CLASS *pUMCommand = NULL;
        DCM_DATASET_CLASS *pUMDataSet = NULL;
        if (pMessage == NULL) throw new System::ArgumentNullException();
        if (pMessage != NULL && pMessage->CommandSet != NULL)
        {
            pUMCommand =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->CommandSet);
        }
        if (pMessage != NULL && pMessage->DataSet != NULL)
        {
            pUMDataSet =
                ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(
                pMessage->DataSet);
        }
        
        AE_SESSION_CLASS ae_session;
        // set ae session
        ae_session.SetName(m_pSNIFFER_SESSION_CLASS->getApplicationEntityName());
        ae_session.SetVersion(m_pSNIFFER_SESSION_CLASS->getApplicationEntityVersion());

        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);

        bool success = false;
        if (pMessage->DataSet != NULL)
        {
            success =
                m_pSNIFFER_SESSION_CLASS->validate(
                pUMCommand, 
                pUMDataSet, 
                umValidationControlFlags,
                &ae_session);
        }
        else
        {
            success =
                m_pSNIFFER_SESSION_CLASS->validate(
                pUMCommand, 
                NULL, 
                umValidationControlFlags,
                &ae_session);
        }
        return success;
    }

    bool MSnifferSession::Validate(
        DvtkData::Dul::DulMessage __gc* pMessage, 
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        bool success = false;
        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);
        System::Type __gc* pMessageType = pMessage->GetType();
        //
        // Convert and validate
        //
        if ( pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RQ) )
        {
            ASSOCIATE_RQ_CLASS *pAssociateRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RQ __gc*>(pMessage));
            
            success = 
                m_pSNIFFER_SESSION_CLASS->validate(
                pAssociateRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_AC) )
        {
            ASSOCIATE_AC_CLASS *pAssociateAc =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_AC __gc*>(pMessage));
            success = 
                m_pSNIFFER_SESSION_CLASS->validate(
                pAssociateAc,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_ASSOCIATE_RJ) )
        {
            ASSOCIATE_RJ_CLASS *pAssociateRj =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ASSOCIATE_RJ __gc*>(pMessage));
            success = 
                m_pSNIFFER_SESSION_CLASS->validate(
                pAssociateRj,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_RELEASE_RQ) )
        {
            RELEASE_RQ_CLASS *pReleaseRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RQ __gc*>(pMessage));
            
            success = 
                m_pSNIFFER_SESSION_CLASS->validate(
                pReleaseRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_RELEASE_RP) )
        {
            RELEASE_RP_CLASS *pReleaseRp =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_RELEASE_RP __gc*>(pMessage));
            success = 
                m_pSNIFFER_SESSION_CLASS->validate(
                pReleaseRp,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::A_ABORT) )
        {
            ABORT_RQ_CLASS *pAbortRq =
                ManagedUnManagedDulConvertors::ManagedUnManagedDulConvertor::Convert(
                static_cast<DvtkData::Dul::A_ABORT __gc*>(pMessage));
            
            success = 
                m_pSNIFFER_SESSION_CLASS->validate(
                pAbortRq,
                umValidationControlFlags);
        }
        else if (
            pMessageType == __typeof(DvtkData::Dul::P_DATA_TF) )
        {
            throw new System::NotSupportedException();
        }
        else
        {
            System::Diagnostics::Trace::Assert(false);
        }
        return success;
    }
}