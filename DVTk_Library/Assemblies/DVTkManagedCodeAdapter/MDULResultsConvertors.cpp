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
#include "MDULResultsConvertors.h"
#include "ValidationMessageLevels.h"
#include "CountingAdapter.h"
#include "UtilityFunctions.h"
#using <mscorlib.dll>

//
// Unmanaged to Managed
//

namespace ManagedUnManagedDulValidationResultsConvertors
{
    using namespace System::Runtime::InteropServices;
    using namespace DvtkData::Validation;
    using namespace Wrappers;

    DvtkData::Validation::MessageType
        ManagedUnManagedDulValidationResultsConvertor::Convert(
        Wrappers::WrappedValidationMessageLevel level)
    {
        switch (level)
        {
        case Wrappers::WrappedValidationMessageLevel::Debug :
        case Wrappers::WrappedValidationMessageLevel::Error :
			{
				if (static_cast<Wrappers::ICountingTarget^>(m_pCountingTarget) != nullptr)
				{
					//m_pCountingTarget->IncrementValidationError();
					m_pCountingTarget->Increment(CountGroup::Validation, CountType::Error);
				}

			}
			return DvtkData::Validation::MessageType::Error;
		case Wrappers::WrappedValidationMessageLevel::Information :
			return DvtkData::Validation::MessageType::Info;
		case Wrappers::WrappedValidationMessageLevel::ConditionText :
			return DvtkData::Validation::MessageType::ConditionText;
		case Wrappers::WrappedValidationMessageLevel::Warning :
			{
				if (static_cast<Wrappers::ICountingTarget^>(m_pCountingTarget) != nullptr)
				{
					//m_pCountingTarget->IncrementValidationWarning();
					m_pCountingTarget->Increment(CountGroup::Validation, CountType::Warning);
				}
			}
            return DvtkData::Validation::MessageType::Warning;
        case Wrappers::WrappedValidationMessageLevel::None :
            return DvtkData::Validation::MessageType::None;
        case Wrappers::WrappedValidationMessageLevel::DicomObjectRelationship :
        case Wrappers::WrappedValidationMessageLevel::DulpStateMachine :
        case Wrappers::WrappedValidationMessageLevel::Scripting :
        case Wrappers::WrappedValidationMessageLevel::ScriptName :
        case Wrappers::WrappedValidationMessageLevel::MediaFilename :
        case Wrappers::WrappedValidationMessageLevel::WareHouseLabel :
        default:
            assert(false);
            return DvtkData::Validation::MessageType::Error;
        }
    }

    //>>===========================================================================

    ManagedUnManagedDulValidationResultsConvertor::ManagedUnManagedDulValidationResultsConvertor(void)

        //  DESCRIPTION     : Constructor
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // constructor activities
        this->m_pRulesUri = nullptr;
    }

    //>>===========================================================================

    ManagedUnManagedDulValidationResultsConvertor::~ManagedUnManagedDulValidationResultsConvertor(void)

        //  DESCRIPTION     : Destructor
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // destructor activities
    }

    //=============================================================================
    //
    // Unmanaged to Managed
    //
    //=============================================================================

    //>>===========================================================================

    DvtkData::Validation::ValidationAssociateRq^ 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ASSOCIATE_RQ_VALIDATOR_CLASS *pUMAssociateRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results associate request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRq == nullptr) return nullptr;

        DvtkData::Validation::ValidationAssociateRq ^ pAssociateRq = gcnew DvtkData::Validation::ValidationAssociateRq();

        // convert parameters
        pAssociateRq->ProtocolVersion = Convert(pUMAssociateRq->getProtocolVersionParameter());
        pAssociateRq->CalledAETitle = Convert(pUMAssociateRq->getCalledAeTitleParameter());
        pAssociateRq->CallingAETitle = Convert(pUMAssociateRq->getCallingAeTitleParameter());
        pAssociateRq->ApplicationContextName = Convert(&pUMAssociateRq->getApplicationContextName());

        // convert the presentation contexts
        for (UINT32 i = 0; i < pUMAssociateRq->noPresentationContexts(); i++)
        {
            ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS UMRequestedPresentationContext = pUMAssociateRq->getPresentationContext(i);

            DvtkData::Validation::SubItems::ValidationAcsePresentationContextRequest ^ pRequestedPresentationContext = gcnew DvtkData::Validation::SubItems::ValidationAcsePresentationContextRequest();

            pRequestedPresentationContext->PresentationContextId = Convert(UMRequestedPresentationContext.getPresentationContextIdParameter());
            pRequestedPresentationContext->AbstractSyntaxName = Convert(UMRequestedPresentationContext.getAbstractSyntaxNameParameter());

            // convert the transfer syntaxes
            for (UINT32 j = 0; j < UMRequestedPresentationContext.noTransferSyntaxNames(); j++)
            {
                DvtkData::Validation::SubItems::ValidationAcseParameter ^ pAcseParameter = Convert(UMRequestedPresentationContext.getTransferSyntaxNameParameter(j));
                pRequestedPresentationContext->RequestedTransferSyntaxNames->Add(pAcseParameter);
            }

            pAssociateRq->PresentationContexts->Add(pRequestedPresentationContext);
        }

        // convert user information
        pAssociateRq->UserInformation = Convert(&pUMAssociateRq->getUserInformation());

        return pAssociateRq;
    }


    //>>===========================================================================

    DvtkData::Validation::ValidationAssociateAc^ 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ASSOCIATE_AC_VALIDATOR_CLASS *pUMAssociateAc)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results associate accept
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateAc == nullptr) return nullptr;

        DvtkData::Validation::ValidationAssociateAc ^ pAssociateAc = gcnew DvtkData::Validation::ValidationAssociateAc();

        // convert parameters
        pAssociateAc->ProtocolVersion = Convert(pUMAssociateAc->getProtocolVersionParameter());
        pAssociateAc->CalledAETitle = Convert(pUMAssociateAc->getCalledAeTitleParameter());
        pAssociateAc->CallingAETitle = Convert(pUMAssociateAc->getCallingAeTitleParameter());
        pAssociateAc->ApplicationContextName = Convert(&pUMAssociateAc->getApplicationContextName());

        // convert the presentation contexts
        for (UINT32 i = 0; i < pUMAssociateAc->noPresentationContexts(); i++)
        {
            ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS UMAcceptedPresentationContext = pUMAssociateAc->getPresentationContext(i);

            DvtkData::Validation::SubItems::ValidationAcsePresentationContextAccept ^ pAcceptedPresentationContext = gcnew DvtkData::Validation::SubItems::ValidationAcsePresentationContextAccept();

            pAcceptedPresentationContext->PresentationContextId = Convert(UMAcceptedPresentationContext.getPresentationContextIdParameter());
            pAcceptedPresentationContext->AbstractSyntaxName = Convert(UMAcceptedPresentationContext.getAbstractSyntaxNameParameter());
            pAcceptedPresentationContext->Result = Convert(UMAcceptedPresentationContext.getResultParameter());
            pAcceptedPresentationContext->TransferSyntaxName = Convert(UMAcceptedPresentationContext.getTransferSyntaxNameParameter());

            pAssociateAc->PresentationContexts->Add(pAcceptedPresentationContext);
        }

        // convert user information
        pAssociateAc->UserInformation = Convert(&pUMAssociateAc->getUserInformation());

        return pAssociateAc;
    }

    //>>===========================================================================

    DvtkData::Validation::ValidationAssociateRj^ 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ASSOCIATE_RJ_VALIDATOR_CLASS *pUMAssociateRj)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results associate reject
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRj == nullptr) return nullptr;

        DvtkData::Validation::ValidationAssociateRj ^ pAssociateRj = gcnew DvtkData::Validation::ValidationAssociateRj();

        // convert parameters
        pAssociateRj->Result = Convert(pUMAssociateRj->getResultParameter());
        pAssociateRj->Source = Convert(pUMAssociateRj->getSourceParameter());
        pAssociateRj->Reason = Convert(pUMAssociateRj->getReasonParameter());

        return pAssociateRj;
    }

    //>>===========================================================================

    DvtkData::Validation::ValidationReleaseRq^ 
        ManagedUnManagedDulValidationResultsConvertor::Convert(RELEASE_RQ_VALIDATOR_CLASS *pUMReleaseRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results release request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRq == nullptr) return nullptr;

        DvtkData::Validation::ValidationReleaseRq ^ pReleaseRq = gcnew DvtkData::Validation::ValidationReleaseRq();

        // no parameters to convert
        return pReleaseRq;
    }

    //>>===========================================================================

    DvtkData::Validation::ValidationReleaseRp^ 
        ManagedUnManagedDulValidationResultsConvertor::Convert(RELEASE_RP_VALIDATOR_CLASS *pUMReleaseRp)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results release response
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRp == nullptr) return nullptr;

        DvtkData::Validation::ValidationReleaseRp ^ pReleaseRp = gcnew DvtkData::Validation::ValidationReleaseRp();

        // no parameters to convert
        return pReleaseRp;
    }

    //>>===========================================================================

    DvtkData::Validation::ValidationAbortRq^ 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ABORT_RQ_VALIDATOR_CLASS *pUMAbortRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results abort request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAbortRq == nullptr) return nullptr;

        DvtkData::Validation::ValidationAbortRq ^ pAbortRq = gcnew DvtkData::Validation::ValidationAbortRq();

        // convert parameters
        pAbortRq->Source = Convert(pUMAbortRq->getSourceParameter());
        pAbortRq->Reason = Convert(pUMAbortRq->getReasonParameter());

        return pAbortRq;
    }

    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseUserInformation^
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_USER_INFORMATION_VALIDATOR_CLASS *pUMUserInformation)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation user information
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMUserInformation == nullptr) return nullptr;

        DvtkData::Validation::SubItems::ValidationAcseUserInformation ^ pUserInformation = gcnew DvtkData::Validation::SubItems::ValidationAcseUserInformation();

        // convert parameters
        pUserInformation->MaximumLengthReceived = Convert(pUMUserInformation->getMaximumLengthReceivedParameter());
        pUserInformation->ImplementationClassUid = Convert(pUMUserInformation->getImplementationClassUidParameter());
        pUserInformation->ImplementationVersionName = Convert(pUMUserInformation->getImplementationVersionNameParameter());

        // convert SCP SCU role selection
        if (pUMUserInformation->noScpScuRoleSelects())
        {
            pUserInformation->ScpScuRoleSelection = gcnew DvtkData::Validation::TypeSafeCollections::ValidationAcseScpScuRoleSelectCollection();
            for (UINT i = 0; i < pUMUserInformation->noScpScuRoleSelects(); i++)
            {
                DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect ^ pScpScuRoleSelect = Convert(&pUMUserInformation->getScpScuRoleSelect(i));
                pUserInformation->ScpScuRoleSelection->Add(pScpScuRoleSelect);
            }
        }

        // convert asynchronous operation window
        if (pUMUserInformation->getAsynchronousOperationWindow())
        {
            DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow ^ pAsynchronousOperationWindow = Convert(pUMUserInformation->getAsynchronousOperationWindow());
            pUserInformation->AsynchronousOperationWindow = pAsynchronousOperationWindow;
        }

        // convert sop class extended
        if (pUMUserInformation->noSopClassExtendeds())
        {
            pUserInformation->ExtendedSopClasses = gcnew DvtkData::Validation::TypeSafeCollections::ValidationAcseSopClassExtendedCollection();
            for (UINT i = 0; i < pUMUserInformation->noSopClassExtendeds(); i++)
            {
                DvtkData::Validation::SubItems::ValidationAcseSopClassExtended ^ pSopClassExtended = Convert(&pUMUserInformation->getSopClassExtended(i));
                pUserInformation->ExtendedSopClasses->Add(pSopClassExtended);
            }
        }

        // convert user identity negotiation
        if (pUMUserInformation->getUserIdentityNegotiation())
        {
            DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation ^ pUserIdentityNegotiation = Convert(pUMUserInformation->getUserIdentityNegotiation());
            pUserInformation->UserIdentityNegotiation = pUserIdentityNegotiation;
        }

        return pUserInformation;
    }

    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect^
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS *pUMScpScuRoleSelect)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation scp scu role selection
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMScpScuRoleSelect == nullptr) return nullptr;

        DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect ^ pScpScuRoleSelect = gcnew DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect();

        // convert parameters
        pScpScuRoleSelect->Uid = Convert(pUMScpScuRoleSelect->getUidParameter());
        pScpScuRoleSelect->ScpRole = Convert(pUMScpScuRoleSelect->getScpRoleParameter());
        pScpScuRoleSelect->ScuRole = Convert(pUMScpScuRoleSelect->getScuRoleParameter());

        return pScpScuRoleSelect;
    }

    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow^
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS *pUMAsynchronousOperationWindow)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation asynchronous operation window
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAsynchronousOperationWindow == nullptr) return nullptr;

        DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow ^ pAsynchronousOperationWindow = gcnew DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow();

        // convert parameters
        pAsynchronousOperationWindow->OperationsInvoked = Convert(pUMAsynchronousOperationWindow->getOperationsInvokedParameter());
        pAsynchronousOperationWindow->OperationsPerformed = Convert(pUMAsynchronousOperationWindow->getOperationsPerformedParameter());

        return pAsynchronousOperationWindow;
    }


    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseSopClassExtended^
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS *pUMSopClassExtended)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation sop class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMSopClassExtended == nullptr) return nullptr;

        DvtkData::Validation::SubItems::ValidationAcseSopClassExtended ^ pSopClassExtended = gcnew DvtkData::Validation::SubItems::ValidationAcseSopClassExtended();

        // convert parameters
        pSopClassExtended->Uid = Convert(pUMSopClassExtended->getUidParameter());

        // convert application information
        for (UINT i = 0; i < pUMSopClassExtended->noAapplicationInformations(); i++)
        {
            DvtkData::Validation::SubItems::ValidationAcseParameter ^ pAcseParameter = Convert(pUMSopClassExtended->getAapplicationInformationParameter(i));
            pSopClassExtended->ApplicationInformation->Add(pAcseParameter);
        }

        return pSopClassExtended;
    }

    //>>===========================================================================

        DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation^
            ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS *pUMUserIdentityNegotiation)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation sop class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMUserIdentityNegotiation == nullptr) return nullptr;

        DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation ^ pUserIdentityNegotiation = gcnew DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation();

        // convert parameters
		if (pUMUserIdentityNegotiation->getPrimaryFieldParameter()->getValue().length() > 0)
		{
		    pUserIdentityNegotiation->UserIdentityType = Convert(pUMUserIdentityNegotiation->getUserIdentityTypeParameter());
		    pUserIdentityNegotiation->PositiveResponseRequested = Convert(pUMUserIdentityNegotiation->getPositiveResponseRequestedParameter());
			if (pUMUserIdentityNegotiation->getPrimaryFieldParameter())
			{
				pUserIdentityNegotiation->PrimaryField = Convert(pUMUserIdentityNegotiation->getPrimaryFieldParameter());
			}
			if (pUMUserIdentityNegotiation->getSecondaryFieldParameter())
			{
				pUserIdentityNegotiation->SecondaryField = Convert(pUMUserIdentityNegotiation->getSecondaryFieldParameter());
			}
		}
		else
		{
			if (pUMUserIdentityNegotiation->getServerResponseParameter())
			{
				pUserIdentityNegotiation->ServerResponse = Convert(pUMUserIdentityNegotiation->getServerResponseParameter());
			}
		}

        return pUserIdentityNegotiation;
	}

    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseParameter^ 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_PARAMETER_CLASS *pUMAcseParameter)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results ACSE parameter
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAcseParameter == nullptr) return nullptr;

        DvtkData::Validation::SubItems::ValidationAcseParameter ^ pAcseParameter = gcnew DvtkData::Validation::SubItems::ValidationAcseParameter();

        // convert value parameter
        System::String^ clistr = gcnew System::String(pUMAcseParameter->getValue().c_str());
        pAcseParameter->Value = clistr;

        // convert meaning parameter
        clistr = gcnew System::String(pUMAcseParameter->getMeaning().c_str());
        pAcseParameter->Meaning = clistr;

        // convert message parameters
        for (int i = 0; i < pUMAcseParameter->noMessages(); i++)
        {
            DvtkData::Validation::ValidationMessage ^ pValidationMessage = gcnew DvtkData::Validation::ValidationMessage();
            System::UInt32 index = pUMAcseParameter->getIndex(i);
            pValidationMessage->Index = index;
            System::UInt32 messageUID = pUMAcseParameter->getCode(i);
            pValidationMessage->Identifier = messageUID;
            clistr = gcnew System::String(pUMAcseParameter->getMessage(i).c_str());
            pValidationMessage->Message = clistr;
            Wrappers::WrappedValidationMessageLevel level =
                Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
            pValidationMessage->Type = Convert(level);

            pAcseParameter->Messages->Add(pValidationMessage);
        }

        return pAcseParameter;
    }

    void ManagedUnManagedDulValidationResultsConvertor::set_Rules(System::Uri^ pRulesUri)
    {
        this->m_pRulesUri = pRulesUri;
        return;
    }

    // <summary>
    // Set the counting target for the validation process.
    // </summary>
    // <remarks>
    // The counting adapter will count the number of errors and warnings based on the rules.
    // </remarks>
	void ManagedUnManagedDulValidationResultsConvertor::set_CountingTarget(ICountingTarget^ pCountingTarget)
    {
        this->m_pCountingTarget = pCountingTarget;
    }
}
