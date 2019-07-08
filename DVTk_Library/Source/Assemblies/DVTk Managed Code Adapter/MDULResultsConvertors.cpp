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
				if (m_pCountingTarget != NULL)
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
				if (m_pCountingTarget != NULL)
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
        this->m_pRulesUri = NULL;
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

    DvtkData::Validation::ValidationAssociateRq __gc* 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ASSOCIATE_RQ_VALIDATOR_CLASS *pUMAssociateRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results associate request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRq == NULL) return NULL;

        DvtkData::Validation::ValidationAssociateRq *pAssociateRq = new DvtkData::Validation::ValidationAssociateRq();

        // convert parameters
        pAssociateRq->ProtocolVersion = Convert(pUMAssociateRq->getProtocolVersionParameter());
        pAssociateRq->CalledAETitle = Convert(pUMAssociateRq->getCalledAeTitleParameter());
        pAssociateRq->CallingAETitle = Convert(pUMAssociateRq->getCallingAeTitleParameter());
        pAssociateRq->ApplicationContextName = Convert(&pUMAssociateRq->getApplicationContextName());

        // convert the presentation contexts
        for (UINT32 i = 0; i < pUMAssociateRq->noPresentationContexts(); i++)
        {
            ACSE_PRESENTATION_CONTEXT_RQ_VALIDATOR_CLASS UMRequestedPresentationContext = pUMAssociateRq->getPresentationContext(i);

            DvtkData::Validation::SubItems::ValidationAcsePresentationContextRequest *pRequestedPresentationContext = new DvtkData::Validation::SubItems::ValidationAcsePresentationContextRequest();

            pRequestedPresentationContext->PresentationContextId = Convert(UMRequestedPresentationContext.getPresentationContextIdParameter());
            pRequestedPresentationContext->AbstractSyntaxName = Convert(UMRequestedPresentationContext.getAbstractSyntaxNameParameter());

            // convert the transfer syntaxes
            for (UINT32 j = 0; j < UMRequestedPresentationContext.noTransferSyntaxNames(); j++)
            {
                DvtkData::Validation::SubItems::ValidationAcseParameter *pAcseParameter = Convert(UMRequestedPresentationContext.getTransferSyntaxNameParameter(j));
                pRequestedPresentationContext->RequestedTransferSyntaxNames->Add(pAcseParameter);
            }

            pAssociateRq->PresentationContexts->Add(pRequestedPresentationContext);
        }

        // convert user information
        pAssociateRq->UserInformation = Convert(&pUMAssociateRq->getUserInformation());

        return pAssociateRq;
    }


    //>>===========================================================================

    DvtkData::Validation::ValidationAssociateAc __gc* 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ASSOCIATE_AC_VALIDATOR_CLASS *pUMAssociateAc)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results associate accept
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateAc == NULL) return NULL;

        DvtkData::Validation::ValidationAssociateAc *pAssociateAc = new DvtkData::Validation::ValidationAssociateAc();

        // convert parameters
        pAssociateAc->ProtocolVersion = Convert(pUMAssociateAc->getProtocolVersionParameter());
        pAssociateAc->CalledAETitle = Convert(pUMAssociateAc->getCalledAeTitleParameter());
        pAssociateAc->CallingAETitle = Convert(pUMAssociateAc->getCallingAeTitleParameter());
        pAssociateAc->ApplicationContextName = Convert(&pUMAssociateAc->getApplicationContextName());

        // convert the presentation contexts
        for (UINT32 i = 0; i < pUMAssociateAc->noPresentationContexts(); i++)
        {
            ACSE_PRESENTATION_CONTEXT_AC_VALIDATOR_CLASS UMAcceptedPresentationContext = pUMAssociateAc->getPresentationContext(i);

            DvtkData::Validation::SubItems::ValidationAcsePresentationContextAccept *pAcceptedPresentationContext = new DvtkData::Validation::SubItems::ValidationAcsePresentationContextAccept();

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

    DvtkData::Validation::ValidationAssociateRj __gc* 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ASSOCIATE_RJ_VALIDATOR_CLASS *pUMAssociateRj)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results associate reject
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRj == NULL) return NULL;

        DvtkData::Validation::ValidationAssociateRj *pAssociateRj = new DvtkData::Validation::ValidationAssociateRj();

        // convert parameters
        pAssociateRj->Result = Convert(pUMAssociateRj->getResultParameter());
        pAssociateRj->Source = Convert(pUMAssociateRj->getSourceParameter());
        pAssociateRj->Reason = Convert(pUMAssociateRj->getReasonParameter());

        return pAssociateRj;
    }

    //>>===========================================================================

    DvtkData::Validation::ValidationReleaseRq __gc* 
        ManagedUnManagedDulValidationResultsConvertor::Convert(RELEASE_RQ_VALIDATOR_CLASS *pUMReleaseRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results release request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRq == NULL) return NULL;

        DvtkData::Validation::ValidationReleaseRq *pReleaseRq = new DvtkData::Validation::ValidationReleaseRq();

        // no parameters to convert
        return pReleaseRq;
    }

    //>>===========================================================================

    DvtkData::Validation::ValidationReleaseRp __gc* 
        ManagedUnManagedDulValidationResultsConvertor::Convert(RELEASE_RP_VALIDATOR_CLASS *pUMReleaseRp)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results release response
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRp == NULL) return NULL;

        DvtkData::Validation::ValidationReleaseRp *pReleaseRp = new DvtkData::Validation::ValidationReleaseRp();

        // no parameters to convert
        return pReleaseRp;
    }

    //>>===========================================================================

    DvtkData::Validation::ValidationAbortRq __gc* 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ABORT_RQ_VALIDATOR_CLASS *pUMAbortRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results abort request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAbortRq == NULL) return NULL;

        DvtkData::Validation::ValidationAbortRq *pAbortRq = new DvtkData::Validation::ValidationAbortRq();

        // convert parameters
        pAbortRq->Source = Convert(pUMAbortRq->getSourceParameter());
        pAbortRq->Reason = Convert(pUMAbortRq->getReasonParameter());

        return pAbortRq;
    }

    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseUserInformation __gc*
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_USER_INFORMATION_VALIDATOR_CLASS *pUMUserInformation)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation user information
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMUserInformation == NULL) return NULL;

        DvtkData::Validation::SubItems::ValidationAcseUserInformation *pUserInformation = new DvtkData::Validation::SubItems::ValidationAcseUserInformation();

        // convert parameters
        pUserInformation->MaximumLengthReceived = Convert(pUMUserInformation->getMaximumLengthReceivedParameter());
        pUserInformation->ImplementationClassUid = Convert(pUMUserInformation->getImplementationClassUidParameter());
        pUserInformation->ImplementationVersionName = Convert(pUMUserInformation->getImplementationVersionNameParameter());

        // convert SCP SCU role selection
        if (pUMUserInformation->noScpScuRoleSelects())
        {
            pUserInformation->ScpScuRoleSelection = new DvtkData::Validation::TypeSafeCollections::ValidationAcseScpScuRoleSelectCollection();
            for (UINT i = 0; i < pUMUserInformation->noScpScuRoleSelects(); i++)
            {
                DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect *pScpScuRoleSelect = Convert(&pUMUserInformation->getScpScuRoleSelect(i));
                pUserInformation->ScpScuRoleSelection->Add(pScpScuRoleSelect);
            }
        }

        // convert asynchronous operation window
        if (pUMUserInformation->getAsynchronousOperationWindow())
        {
            DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow *pAsynchronousOperationWindow = Convert(pUMUserInformation->getAsynchronousOperationWindow());
            pUserInformation->AsynchronousOperationWindow = pAsynchronousOperationWindow;
        }

        // convert sop class extended
        if (pUMUserInformation->noSopClassExtendeds())
        {
            pUserInformation->ExtendedSopClasses = new DvtkData::Validation::TypeSafeCollections::ValidationAcseSopClassExtendedCollection();
            for (UINT i = 0; i < pUMUserInformation->noSopClassExtendeds(); i++)
            {
                DvtkData::Validation::SubItems::ValidationAcseSopClassExtended *pSopClassExtended = Convert(&pUMUserInformation->getSopClassExtended(i));
                pUserInformation->ExtendedSopClasses->Add(pSopClassExtended);
            }
        }

        // convert user identity negotiation
        if (pUMUserInformation->getUserIdentityNegotiation())
        {
            DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation *pUserIdentityNegotiation = Convert(pUMUserInformation->getUserIdentityNegotiation());
            pUserInformation->UserIdentityNegotiation = pUserIdentityNegotiation;
        }

        return pUserInformation;
    }

    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect __gc*
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS *pUMScpScuRoleSelect)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation scp scu role selection
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMScpScuRoleSelect == NULL) return NULL;

        DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect *pScpScuRoleSelect = new DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect();

        // convert parameters
        pScpScuRoleSelect->Uid = Convert(pUMScpScuRoleSelect->getUidParameter());
        pScpScuRoleSelect->ScpRole = Convert(pUMScpScuRoleSelect->getScpRoleParameter());
        pScpScuRoleSelect->ScuRole = Convert(pUMScpScuRoleSelect->getScuRoleParameter());

        return pScpScuRoleSelect;
    }

    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow __gc*
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS *pUMAsynchronousOperationWindow)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation asynchronous operation window
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAsynchronousOperationWindow == NULL) return NULL;

        DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow *pAsynchronousOperationWindow = new DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow();

        // convert parameters
        pAsynchronousOperationWindow->OperationsInvoked = Convert(pUMAsynchronousOperationWindow->getOperationsInvokedParameter());
        pAsynchronousOperationWindow->OperationsPerformed = Convert(pUMAsynchronousOperationWindow->getOperationsPerformedParameter());

        return pAsynchronousOperationWindow;
    }


    //>>===========================================================================

    DvtkData::Validation::SubItems::ValidationAcseSopClassExtended __gc*
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS *pUMSopClassExtended)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation sop class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMSopClassExtended == NULL) return NULL;

        DvtkData::Validation::SubItems::ValidationAcseSopClassExtended *pSopClassExtended = new DvtkData::Validation::SubItems::ValidationAcseSopClassExtended();

        // convert parameters
        pSopClassExtended->Uid = Convert(pUMSopClassExtended->getUidParameter());

        // convert application information
        for (UINT i = 0; i < pUMSopClassExtended->noAapplicationInformations(); i++)
        {
            DvtkData::Validation::SubItems::ValidationAcseParameter *pAcseParameter = Convert(pUMSopClassExtended->getAapplicationInformationParameter(i));
            pSopClassExtended->ApplicationInformation->Add(pAcseParameter);
        }

        return pSopClassExtended;
    }

    //>>===========================================================================

        DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation __gc*
            ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS *pUMUserIdentityNegotiation)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation sop class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMUserIdentityNegotiation == NULL) return NULL;

        DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation *pUserIdentityNegotiation = new DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation();

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

    DvtkData::Validation::SubItems::ValidationAcseParameter __gc* 
        ManagedUnManagedDulValidationResultsConvertor::Convert(ACSE_PARAMETER_CLASS *pUMAcseParameter)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL validation results ACSE parameter
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAcseParameter == NULL) return NULL;

        DvtkData::Validation::SubItems::ValidationAcseParameter *pAcseParameter = new DvtkData::Validation::SubItems::ValidationAcseParameter();

        // convert value parameter
        pAcseParameter->Value = pUMAcseParameter->getValue().c_str();

        // convert meaning parameter
        pAcseParameter->Meaning = pUMAcseParameter->getMeaning().c_str();

        // convert message parameters
        for (int i = 0; i < pUMAcseParameter->noMessages(); i++)
        {
            DvtkData::Validation::ValidationMessage *pValidationMessage = new DvtkData::Validation::ValidationMessage();
            System::UInt32 index = pUMAcseParameter->getIndex(i);
            pValidationMessage->Index = index;
            System::UInt32 messageUID = pUMAcseParameter->getCode(i);
            pValidationMessage->Identifier = messageUID;
            pValidationMessage->Message = pUMAcseParameter->getMessage(i).c_str();
            Wrappers::WrappedValidationMessageLevel level =
                Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
            pValidationMessage->Type = Convert(level);

            pAcseParameter->Messages->Add(pValidationMessage);
        }

        return pAcseParameter;
    }

    void ManagedUnManagedDulValidationResultsConvertor::set_Rules(System::Uri __gc* pRulesUri)
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
	void ManagedUnManagedDulValidationResultsConvertor::set_CountingTarget(ICountingTarget __gc* pCountingTarget)
    {
        this->m_pCountingTarget = pCountingTarget;
    }
}
