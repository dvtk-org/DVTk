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
#include "MDULConvertors.h"
#include "UtilityFunctions.h"
#using <mscorlib.dll>

namespace ManagedUnManagedDulConvertors
{
    using namespace System::Runtime::InteropServices;
    using namespace DvtkData::Dul;
    using namespace Wrappers;

    //>>===========================================================================

    ManagedUnManagedDulConvertor::ManagedUnManagedDulConvertor(void)

        //  DESCRIPTION     : Constructor
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // constructor activities
    }

    //>>===========================================================================

    ManagedUnManagedDulConvertor::~ManagedUnManagedDulConvertor(void)

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

    DvtkData::Dul::A_ASSOCIATE_RQ^ 
        ManagedUnManagedDulConvertor::Convert(ASSOCIATE_RQ_CLASS *pUMAssociateRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL associate request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRq == nullptr) return nullptr;

        DvtkData::Dul::A_ASSOCIATE_RQ ^ pAssociateRq = gcnew DvtkData::Dul::A_ASSOCIATE_RQ();

        // convert parameters
        pAssociateRq->ProtocolVersion = pUMAssociateRq->getProtocolVersion();
        System::String^ clistr = gcnew System::String(pUMAssociateRq->getCalledAeTitle());
        pAssociateRq->CalledAETitle = clistr;
        clistr = gcnew System::String(pUMAssociateRq->getCallingAeTitle());
        pAssociateRq->CallingAETitle = clistr;

        pAssociateRq->ApplicationContext = ConvertAC(pUMAssociateRq->getApplicationContextName());

        // convert requested persentation contexts
        for (UINT i = 0; i < pUMAssociateRq->noPresentationContexts(); i++)
        {
            DvtkData::Dul::RequestedPresentationContext ^ pRequestedPresentationContext = Convert(pUMAssociateRq->getPresentationContext(i));
            pAssociateRq->PresentationContexts->Add(pRequestedPresentationContext);
        }

        // convert user information
        pAssociateRq->UserInformation = Convert(pUMAssociateRq->getUserInformation());

        return pAssociateRq;
    }

    //>>===========================================================================

    DvtkData::Dul::A_ASSOCIATE_AC^ 
        ManagedUnManagedDulConvertor::Convert(ASSOCIATE_AC_CLASS *pUMAssociateAc)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL associate accept
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateAc == nullptr) return nullptr;

        DvtkData::Dul::A_ASSOCIATE_AC ^ pAssociateAc = gcnew DvtkData::Dul::A_ASSOCIATE_AC();

        // convert parameters
        pAssociateAc->ProtocolVersion = pUMAssociateAc->getProtocolVersion();
        System::String^ clistr = gcnew System::String(pUMAssociateAc->getCalledAeTitle());
        pAssociateAc->CalledAETitle = clistr;
        clistr = gcnew System::String(pUMAssociateAc->getCallingAeTitle());
        pAssociateAc->CallingAETitle = clistr;

        pAssociateAc->ApplicationContext = ConvertAC(pUMAssociateAc->getApplicationContextName());

        // convert accepted presentation contexts
        for (UINT i = 0; i < pUMAssociateAc->noPresentationContexts(); i++)
        {
            DvtkData::Dul::AcceptedPresentationContext ^pAcceptedPresentationContext = Convert(pUMAssociateAc->getPresentationContext(i));
            pAssociateAc->PresentationContexts->Add(pAcceptedPresentationContext);
        }

        // convert user information
        pAssociateAc->UserInformation = Convert(pUMAssociateAc->getUserInformation());

        return pAssociateAc;
    }

    //>>===========================================================================

    DvtkData::Dul::A_ASSOCIATE_RJ^ 
        ManagedUnManagedDulConvertor::Convert(ASSOCIATE_RJ_CLASS *pUMAssociateRj)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL associate reject
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRj == nullptr) return nullptr;

        DvtkData::Dul::A_ASSOCIATE_RJ ^ pAssociateRj = gcnew DvtkData::Dul::A_ASSOCIATE_RJ();

        // convert parameters
        pAssociateRj->Result = pUMAssociateRj->getResult();
        pAssociateRj->Source = pUMAssociateRj->getSource();
        pAssociateRj->Reason = pUMAssociateRj->getReason();

        return pAssociateRj;
    }

    //>>===========================================================================

    DvtkData::Dul::A_RELEASE_RQ^ 
        ManagedUnManagedDulConvertor::Convert(RELEASE_RQ_CLASS *pUMReleaseRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL release request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRq == nullptr) return nullptr;

        DvtkData::Dul::A_RELEASE_RQ ^ pReleaseRq = gcnew DvtkData::Dul::A_RELEASE_RQ();

        // no parameters to convert
        return pReleaseRq;
    }

    //>>===========================================================================

    DvtkData::Dul::A_RELEASE_RP^ 
        ManagedUnManagedDulConvertor::Convert(RELEASE_RP_CLASS *pUMReleaseRp)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL release response
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRp == nullptr) return nullptr;

        DvtkData::Dul::A_RELEASE_RP ^ pReleaseRp = gcnew DvtkData::Dul::A_RELEASE_RP();

        // no parameters to convert
        return pReleaseRp;
    }

    //>>===========================================================================

    DvtkData::Dul::A_ABORT^ 
        ManagedUnManagedDulConvertor::Convert(ABORT_RQ_CLASS *pUMAbortRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL abort request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAbortRq == nullptr) return nullptr;

        DvtkData::Dul::A_ABORT ^ pAbortRq = gcnew DvtkData::Dul::A_ABORT();

        // convert parameters
        pAbortRq->Source = pUMAbortRq->getSource();
        pAbortRq->Reason = pUMAbortRq->getReason();

        return pAbortRq;
    }

    //>>===========================================================================

    DvtkData::Dul::ApplicationContext^ 
        ManagedUnManagedDulConvertor::ConvertAC(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL application context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::ApplicationContext ^pApplicationContext = gcnew DvtkData::Dul::ApplicationContext();

        // convert application context
        System::String ^pValue = gcnew System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        pApplicationContext->Name = pValue;

        return pApplicationContext;
    }

    //>>===========================================================================

    DvtkData::Dul::RequestedPresentationContext^
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL requested presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::RequestedPresentationContext ^ pRequestedPresentationContext = gcnew DvtkData::Dul::RequestedPresentationContext();

        // convert parameters
        pRequestedPresentationContext->ID = UMRequestedPresentationContext.getPresentationContextId();
        pRequestedPresentationContext->AbstractSyntax = ConvertAS(UMRequestedPresentationContext.getAbstractSyntaxName());

        // convert transfer syntaxes
        for (UINT i = 0; i < UMRequestedPresentationContext.noTransferSyntaxNames(); i++)
        {
            DvtkData::Dul::TransferSyntax ^ pTransferSyntax = ConvertTS(UMRequestedPresentationContext.getTransferSyntaxName(i));
            pRequestedPresentationContext->TransferSyntaxes->Add(pTransferSyntax);
        }

        return pRequestedPresentationContext;
    }

    //>>===========================================================================

    DvtkData::Dul::AcceptedPresentationContext^
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL accepted presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::AcceptedPresentationContext ^pAcceptedPresentationContext = gcnew DvtkData::Dul::AcceptedPresentationContext();

        // convert parameters
        pAcceptedPresentationContext->ID = UMAcceptedPresentationContext.getPresentationContextId();
        pAcceptedPresentationContext->AbstractSyntax = ConvertAS(UMAcceptedPresentationContext.getAbstractSyntaxName());
        pAcceptedPresentationContext->Result = UMAcceptedPresentationContext.getResultReason();

        // only valid if result is acceptance
        if (UMAcceptedPresentationContext.getResultReason() == ACCEPTANCE)
        {
            // convert transfer syntax
            pAcceptedPresentationContext->TransferSyntax = ConvertTS(UMAcceptedPresentationContext.getTransferSyntaxName());
        }

        return pAcceptedPresentationContext;
    }

    //>>===========================================================================

    DvtkData::Dul::AbstractSyntax^
        ManagedUnManagedDulConvertor::ConvertAS(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL abstract syntax
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {

        // convert abstract syntax
        System::String ^pValue = gcnew System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        DvtkData::Dul::AbstractSyntax ^pAbstractSyntax = gcnew DvtkData::Dul::AbstractSyntax(pValue);

        return pAbstractSyntax;
    }

    //>>===========================================================================

    DvtkData::Dul::TransferSyntax^
        ManagedUnManagedDulConvertor::ConvertTS(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL transfer syntax
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {

        // convert transfer syntax
        System::String ^ pValue = gcnew System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        DvtkData::Dul::TransferSyntax ^ pTransferSyntax = gcnew DvtkData::Dul::TransferSyntax(pValue);

        return pTransferSyntax;
    }

    //>>===========================================================================

    DvtkData::Dul::UserInformation^
        ManagedUnManagedDulConvertor::Convert(USER_INFORMATION_CLASS& UMUserInformation)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL user information
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::UserInformation ^ pUserInformation = gcnew DvtkData::Dul::UserInformation();

        // convert parameters
        pUserInformation->MaximumLength = Convert(UMUserInformation.getMaximumLengthReceived());
        pUserInformation->ImplementationClassUid = ConvertICU(UMUserInformation.getImplementationClassUid());

        // optional implementation version name
        char *pUMImplementationVersionName = UMUserInformation.getImplementationVersionName();
        if (pUMImplementationVersionName)
        {
            pUserInformation->ImplementationVersionName = Convert(pUMImplementationVersionName);
        }

        // optional asynchronous operations window
        UINT16 invoked, performed;
        if (UMUserInformation.getAsynchronousOperationWindow(&invoked, &performed))
        {
            pUserInformation->AsynchronousOperationsWindow = Convert(invoked, performed);
        }

        // optional SCP SCU Role Select
        if (UMUserInformation.noScpScuRoleSelects())
        {
            DvtkData::Dul::ScpScuRoleSelectionList ^pScpScuRoleSelectionList = gcnew DvtkData::Dul::ScpScuRoleSelectionList();
            pUserInformation->ScpScuRoleSelections = pScpScuRoleSelectionList;

            for (UINT i = 0; i < UMUserInformation.noScpScuRoleSelects(); i++)
            {
                DvtkData::Dul::ScpScuRoleSelection ^pScpScuRoleSelection = Convert(UMUserInformation.getScpScuRoleSelect(i));
                pScpScuRoleSelectionList->Add(pScpScuRoleSelection);
            }
        }

        // optional SOP Class Extended Negotiation
        if (UMUserInformation.noSopClassExtendeds())
        {
            DvtkData::Dul::SopClassExtendedNegotiationList ^ pSopClassExtendedNegotiationList = gcnew SopClassExtendedNegotiationList();
            pUserInformation->SopClassExtendedNegotiations = pSopClassExtendedNegotiationList;

            for (UINT i = 0; i < UMUserInformation.noSopClassExtendeds(); i++)
            {
                DvtkData::Dul::SopClassExtendedNegotiation ^ pSopClassExtendedNegotiation = Convert(UMUserInformation.getSopClassExtended(i));
                pSopClassExtendedNegotiationList->Add(pSopClassExtendedNegotiation);
            }
        }

		// optional User Identity Negotiation
		if (UMUserInformation.getUserIdentityNegotiationPrimaryField())
		{
            pUserInformation->UserIdentityNegotiation = Convert(UMUserInformation.getUserIdentityNegotiationUserIdentityType(),
				UMUserInformation.getUserIdentityNegotiationPositiveResponseRequested(),
				UMUserInformation.getUserIdentityNegotiationPrimaryField(),
				UMUserInformation.getUserIdentityNegotiationSecondaryField());
		}
		else if (UMUserInformation.getUserIdentityNegotiationServerResponse())
		{
            pUserInformation->UserIdentityNegotiation = Convert2(UMUserInformation.getUserIdentityNegotiationServerResponse());
		}

        return pUserInformation;
    }

    //>>===========================================================================

    DvtkData::Dul::MaximumLength^
        ManagedUnManagedDulConvertor::Convert(UINT32 UMMaximumLength)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL maximum length received
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::MaximumLength ^ pMaximumLength = gcnew DvtkData::Dul::MaximumLength();
        pMaximumLength->MaximumLengthReceived = UMMaximumLength;

        // convert maxmum length
        return pMaximumLength;
    }

    //>>===========================================================================

    DvtkData::Dul::ImplementationClassUid^
        ManagedUnManagedDulConvertor::ConvertICU(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL implementation class uid
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::ImplementationClassUid ^ pImplementationClassUid = gcnew DvtkData::Dul::ImplementationClassUid();

        // convert implementation class uid
        System::String ^ pValue = gcnew System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        pImplementationClassUid->UID = pValue;

        return pImplementationClassUid;
    }

    //>>===========================================================================

    DvtkData::Dul::ImplementationVersionName^
        ManagedUnManagedDulConvertor::Convert(char *pUMImplementationVersionName)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL implementation version name
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMImplementationVersionName == nullptr) return nullptr;

        // convert implementation version name
        DvtkData::Dul::ImplementationVersionName ^ pImplementationVersionName = gcnew DvtkData::Dul::ImplementationVersionName();
        System::String^ clistr = gcnew System::String(pUMImplementationVersionName);
        pImplementationVersionName->Name = clistr;

        return pImplementationVersionName;
    }

    //>>===========================================================================

    DvtkData::Dul::AsynchronousOperationsWindow^
        ManagedUnManagedDulConvertor::Convert(UINT16 UMInvoked, UINT16 UMPerformed)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL asynchronous operations window
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::AsynchronousOperationsWindow ^pAsynchronousOperationsWindow = gcnew DvtkData::Dul::AsynchronousOperationsWindow();

        // convert parameters
        pAsynchronousOperationsWindow->MaximumNumberOperationsInvoked = UMInvoked;
        pAsynchronousOperationsWindow->MaximumNumberOperationsPerformed = UMPerformed;

        return pAsynchronousOperationsWindow;
    }

    //>>===========================================================================

    DvtkData::Dul::ScpScuRoleSelection^
        ManagedUnManagedDulConvertor::Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL SCP SCU role selection
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::ScpScuRoleSelection ^pScpScuRoleSelect = gcnew DvtkData::Dul::ScpScuRoleSelection();

        // convert parameters
        pScpScuRoleSelect->SopClassUid = ConvertSCU(UMScpScuRoleSelect.getUid());
        pScpScuRoleSelect->ScpRole = UMScpScuRoleSelect.getScpRole();
        pScpScuRoleSelect->ScuRole = UMScpScuRoleSelect.getScuRole();

        return pScpScuRoleSelect;
    }

    //>>===========================================================================

    DvtkData::Dul::SopClassExtendedNegotiation^
        ManagedUnManagedDulConvertor::Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL SOP class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::SopClassExtendedNegotiation ^pSopClassExtendedNegotiation = gcnew DvtkData::Dul::SopClassExtendedNegotiation();

        // convert parameters
        pSopClassExtendedNegotiation->SopClassUid = ConvertSCU(UMSopClassExtended.getUid());

        UINT length = UMSopClassExtended.getNoApplicationInformation();
        if (length)
        {
            // System::Byte byteArray[] = new System::Byte[length]; 
            auto byteArray = gcnew cli::array<System::Byte>(length);

            for (UINT i = 0; i < UMSopClassExtended.getNoApplicationInformation(); i++)
            {
                byteArray[i] = UMSopClassExtended.getApplicationInformation(i);
            }
            pSopClassExtendedNegotiation->ServiceClassApplicationInformation = byteArray;
        }

        return pSopClassExtendedNegotiation;
    }

    //>>===========================================================================

    DvtkData::Dul::UserIdentityNegotiation^
        ManagedUnManagedDulConvertor::Convert(BYTE UMUserIdentityType,
											BYTE UMPositiveResponseRequested,
											char *UMPrimaryField,
											char *UMSecondaryField)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL user identity negotiation
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::UserIdentityNegotiation ^pUserIdentityNegotiation = gcnew DvtkData::Dul::UserIdentityNegotiation();

        // convert parameters
		pUserIdentityNegotiation->UserIdentityType = UMUserIdentityType;
		pUserIdentityNegotiation->PositiveResponseRequested = UMPositiveResponseRequested;
		if (UMPrimaryField)
		{
            System::String^ clistr = gcnew System::String(UMPrimaryField);
            pUserIdentityNegotiation->PrimaryField = clistr;
		}
		if (UMSecondaryField)
		{
            System::String^ clistr = gcnew System::String(UMSecondaryField);
            pUserIdentityNegotiation->SecondaryField = clistr;
		}
        return pUserIdentityNegotiation;
    }

    //>>===========================================================================

    DvtkData::Dul::UserIdentityNegotiation^
        ManagedUnManagedDulConvertor::Convert2(char *UMServerResponse)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL user identity negotiation
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::UserIdentityNegotiation ^pUserIdentityNegotiation = gcnew DvtkData::Dul::UserIdentityNegotiation();

        // convert parameters
        System::String^ clistr = gcnew System::String(UMServerResponse);
        pUserIdentityNegotiation->ServerResponse = clistr;

        return pUserIdentityNegotiation;
    }

    //>>===========================================================================

    System::String^
        ManagedUnManagedDulConvertor::ConvertSCU(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL SOP class uid
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        System::String ^pValue = gcnew System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());

        // convert sop class uid
        return pValue;
    }

    //=============================================================================
    //
    // Managed to Unmanaged
    //
    //=============================================================================

    //>>===========================================================================

    ASSOCIATE_RQ_CLASS*
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ASSOCIATE_RQ ^ pAssociateRq)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL associate request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAssociateRq == nullptr) return nullptr;

        ASSOCIATE_RQ_CLASS *pUMAssociateRq = new ASSOCIATE_RQ_CLASS();

        // covert properties
        pUMAssociateRq->setProtocolVersion(pAssociateRq->ProtocolVersion);

        string value;
        MarshalString(pAssociateRq->CalledAETitle, value);
        pUMAssociateRq->setCalledAeTitle((char*)value.c_str());

        MarshalString(pAssociateRq->CallingAETitle, value);
        pUMAssociateRq->setCallingAeTitle((char*)value.c_str());

        if (pAssociateRq->ApplicationContext)
        {
            MarshalString(pAssociateRq->ApplicationContext->Name, value);
            pUMAssociateRq->setApplicationContextName((char*)value.c_str());
        }

        // convert requested presentation contexts
        if (pAssociateRq->PresentationContexts)
        {
            for (int i = 0; i < pAssociateRq->PresentationContexts->Count; i++)
            {
                DvtkData::Dul::RequestedPresentationContext ^ pRequestedPresentationContext = pAssociateRq->PresentationContexts->default[i];
                PRESENTATION_CONTEXT_RQ_CLASS UMRequestedPresentationContext;
                Convert(UMRequestedPresentationContext, pRequestedPresentationContext);
                pUMAssociateRq->addPresentationContext(UMRequestedPresentationContext);
                UMRequestedPresentationContext.cleanup();
            }
        }

        // convert user information
        USER_INFORMATION_CLASS UMUserInformation;
        Convert(UMUserInformation, pAssociateRq->UserInformation);
        pUMAssociateRq->setUserInformation(UMUserInformation);

        return pUMAssociateRq;
    }

    //>>===========================================================================

    ASSOCIATE_AC_CLASS*
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ASSOCIATE_AC ^ pAssociateAc)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL associate accept
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAssociateAc == nullptr) return nullptr;

        ASSOCIATE_AC_CLASS *pUMAssociateAc = new ASSOCIATE_AC_CLASS();

        // convert properties
        pUMAssociateAc->setProtocolVersion(pAssociateAc->ProtocolVersion);

        string value;
        MarshalString(pAssociateAc->CalledAETitle, value);
        pUMAssociateAc->setCalledAeTitle((char*)value.c_str());

        MarshalString(pAssociateAc->CallingAETitle, value);
        pUMAssociateAc->setCallingAeTitle((char*)value.c_str());

        if (pAssociateAc->ApplicationContext)
        {
            MarshalString(pAssociateAc->ApplicationContext->Name, value);
            pUMAssociateAc->setApplicationContextName((char*)value.c_str());
        }

        // convert accepted presentation contexts
        if (pAssociateAc->PresentationContexts)
        {
            for (int i = 0; i < pAssociateAc->PresentationContexts->Count; i++)
            {
                DvtkData::Dul::AcceptedPresentationContext ^ pAcceptedPresentationContext = pAssociateAc->PresentationContexts->default[i];
                PRESENTATION_CONTEXT_AC_CLASS UMAcceptedPresentationContext;
                Convert(UMAcceptedPresentationContext, pAcceptedPresentationContext);
                pUMAssociateAc->addPresentationContext(UMAcceptedPresentationContext);
            }
        }

        // convert user information
        USER_INFORMATION_CLASS UMUserInformation;
        Convert(UMUserInformation, pAssociateAc->UserInformation);
        pUMAssociateAc->setUserInformation(UMUserInformation);

        return pUMAssociateAc;
    }

    //>>===========================================================================

    ASSOCIATE_RJ_CLASS* 
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ASSOCIATE_RJ ^ pAssociateRj)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL associate reject
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAssociateRj == nullptr) return nullptr;

        ASSOCIATE_RJ_CLASS *pUMAssociateRj = new ASSOCIATE_RJ_CLASS();

        // convert properties
        pUMAssociateRj->setResult(pAssociateRj->Result);
        pUMAssociateRj->setSource(pAssociateRj->Source);
        pUMAssociateRj->setReason(pAssociateRj->Reason);

        return pUMAssociateRj;
    }

    //>>===========================================================================

    RELEASE_RQ_CLASS* 
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_RELEASE_RQ ^ pReleaseRq)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL release request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pReleaseRq == nullptr) return nullptr;

        RELEASE_RQ_CLASS *pUMReleaseRq = new RELEASE_RQ_CLASS();

        // no properties to convert
        return pUMReleaseRq;
    }

    //>>===========================================================================

    RELEASE_RP_CLASS* 
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_RELEASE_RP ^ pReleaseRp)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL release response
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pReleaseRp == nullptr) return nullptr;

        RELEASE_RP_CLASS *pUMReleaseRp = new RELEASE_RP_CLASS();

        // no properties to convert
        return pUMReleaseRp;
    }

    //>>===========================================================================

    ABORT_RQ_CLASS* 
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ABORT ^ pAbortRq)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL abort request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAbortRq == nullptr) return nullptr;

        ABORT_RQ_CLASS *pUMAbortRq = new ABORT_RQ_CLASS();

        // convert properties
        pUMAbortRq->setSource(pAbortRq->Source);
        pUMAbortRq->setReason(pAbortRq->Reason);

        return pUMAbortRq;
    }

    //>>===========================================================================

    string
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::ApplicationContext ^ pApplicationContext)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL application context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        string UMString;

        // convert property
        if (pApplicationContext)
        {
            MarshalString(pApplicationContext->Name, UMString);
        }

        return UMString;
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext, DvtkData::Dul::RequestedPresentationContext ^ pRequestedPresentationContext)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL requested presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pRequestedPresentationContext == nullptr) return;

        // convert properties
        UMRequestedPresentationContext.setPresentationContextId(pRequestedPresentationContext->ID);

        UID_CLASS uid;
        Convert(uid, pRequestedPresentationContext->AbstractSyntax->UID);
        UMRequestedPresentationContext.setAbstractSyntaxName(uid);

        // convert transfer syntaxes
        if (pRequestedPresentationContext->TransferSyntaxes)
        {
            for (int i = 0; i < pRequestedPresentationContext->TransferSyntaxes->Count; i++)
            {
                DvtkData::Dul::TransferSyntax ^ pTransferSyntax = pRequestedPresentationContext->TransferSyntaxes->default[i];
                Convert(uid, pTransferSyntax->UID);
                UMRequestedPresentationContext.addTransferSyntaxName(uid);
            }
        }
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext, DvtkData::Dul::AcceptedPresentationContext ^ pAcceptedPresentationContext)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL accepted presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAcceptedPresentationContext == nullptr) return;

        // convert properties
        UMAcceptedPresentationContext.setPresentationContextId(pAcceptedPresentationContext->ID);

        UID_CLASS uid;
        Convert(uid, pAcceptedPresentationContext->AbstractSyntax->UID);
        UMAcceptedPresentationContext.setAbstractSyntaxName(uid);

        UMAcceptedPresentationContext.setResultReason(pAcceptedPresentationContext->Result);

        // only if result accepted
        if (pAcceptedPresentationContext->Result == DvtkData::Dul::AcceptedPresentationContext::Result_Acceptance)
        {
            // convert transfer syntax
            Convert(uid, pAcceptedPresentationContext->TransferSyntax->UID);
            UMAcceptedPresentationContext.setTransferSyntaxName(uid);
        }
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(USER_INFORMATION_CLASS& UMUserInformation, DvtkData::Dul::UserInformation ^ pUserInformation)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL user information
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUserInformation == nullptr) return;

        // convert properties
        if (pUserInformation->MaximumLength)
        {
            UMUserInformation.setMaximumLengthReceived(pUserInformation->MaximumLength->MaximumLengthReceived);
        }

        string value;
        if (pUserInformation->ImplementationClassUid)
        {
            MarshalString(pUserInformation->ImplementationClassUid->UID, value);
            UMUserInformation.setImplementationClassUid((char*)value.c_str());
        }

        // optional implementation version name
        if (pUserInformation->ImplementationVersionName)
        {
            MarshalString(pUserInformation->ImplementationVersionName->Name, value);
            UMUserInformation.setImplementationVersionName((char*)value.c_str());
        }

        // optional asynchronous operation window
        if (pUserInformation->AsynchronousOperationsWindow)
        {
            UMUserInformation.setAsynchronousOperationWindow(pUserInformation->AsynchronousOperationsWindow->MaximumNumberOperationsInvoked, pUserInformation->AsynchronousOperationsWindow->MaximumNumberOperationsPerformed);
        }

        // optional SCP SCU Role Selection
        if (pUserInformation->ScpScuRoleSelections)
        {
            for (int i = 0; i < pUserInformation->ScpScuRoleSelections->Count; i++)
            {
                DvtkData::Dul::ScpScuRoleSelection ^ pScpScuRoleSelection = pUserInformation->ScpScuRoleSelections->default[i];
                SCP_SCU_ROLE_SELECT_CLASS UMScpScuRoleSelect;
                Convert(UMScpScuRoleSelect, pScpScuRoleSelection);
                UMUserInformation.addScpScuRoleSelect(UMScpScuRoleSelect);
            }
        }

        // optional SOP Class Extended
        if (pUserInformation->SopClassExtendedNegotiations)
        {
            for (int i = 0; i < pUserInformation->SopClassExtendedNegotiations->Count; i++)
            {
                DvtkData::Dul::SopClassExtendedNegotiation ^ pSopClassExtendedNegotiation = pUserInformation->SopClassExtendedNegotiations->default[i];
                SOP_CLASS_EXTENDED_CLASS UMSopClassExtended;
                Convert(UMSopClassExtended, pSopClassExtendedNegotiation);
                UMUserInformation.addSopClassExtended(UMSopClassExtended);
                UMSopClassExtended.cleanup();
            }
        }

		// optional User Identity Negotiation
        if (pUserInformation->UserIdentityNegotiation)
        {
			// convert parameters
			string UMPrimaryField;
			string UMSecondaryField;
			string UMServerResponse;
			Convert(UMPrimaryField, UMSecondaryField, UMServerResponse, pUserInformation->UserIdentityNegotiation);

			if (UMPrimaryField.length())
			{
				if (UMSecondaryField.length())
				{
					UMUserInformation.setUserIdentityNegotiation(pUserInformation->UserIdentityNegotiation->UserIdentityType,
									pUserInformation->UserIdentityNegotiation->PositiveResponseRequested,
									(char*)UMPrimaryField.c_str(),
									(char*)UMSecondaryField.c_str());
				}
				else
				{
					UMUserInformation.setUserIdentityNegotiation(pUserInformation->UserIdentityNegotiation->UserIdentityType,
									pUserInformation->UserIdentityNegotiation->PositiveResponseRequested,
									(char*)UMPrimaryField.c_str());
				}
			}
			else
			{
				UMUserInformation.setUserIdentityNegotiation((char*)UMServerResponse.c_str());
			}
		}
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(UID_CLASS& uid, System::String ^ pString)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL uid
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        string value;

        // convert string property to UID
        MarshalString(pString, value);
        uid.set((char*)value.c_str());
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect, DvtkData::Dul::ScpScuRoleSelection ^ pScpScuRoleSelection)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL SCP SCU role select
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pScpScuRoleSelection == nullptr) return;

        // convert properties
        string value;
        MarshalString(pScpScuRoleSelection->SopClassUid, value);

        UMScpScuRoleSelect.setUid((char*)value.c_str());
        UMScpScuRoleSelect.setScpRole(pScpScuRoleSelection->ScpRole); 
        UMScpScuRoleSelect.setScuRole(pScpScuRoleSelection->ScuRole);
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended, DvtkData::Dul::SopClassExtendedNegotiation ^ pSopClassExtendedNegotiation)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL SOP class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pSopClassExtendedNegotiation == nullptr) return;

        // convert properties
        string value;
        MarshalString(pSopClassExtendedNegotiation->SopClassUid, value);

        UMSopClassExtended.setUid((char*)value.c_str());

        for (int i = 0; i < pSopClassExtendedNegotiation->ServiceClassApplicationInformation->Length; i++)
        {
            UMSopClassExtended.addApplicationInformation(pSopClassExtendedNegotiation->ServiceClassApplicationInformation[i]);
        }
    }

	void
        ManagedUnManagedDulConvertor::Convert(string& UMPrimaryField, string& UMSecondaryField, string& UMServerResponse, DvtkData::Dul::UserIdentityNegotiation ^ pUserIdentityNegotiation)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL User identity negotiation
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
	{
        if (pUserIdentityNegotiation == nullptr) return;

		// convert properties
		MarshalString(pUserIdentityNegotiation->PrimaryField, UMPrimaryField);
		MarshalString(pUserIdentityNegotiation->SecondaryField, UMSecondaryField);
	    MarshalString(pUserIdentityNegotiation->ServerResponse, UMServerResponse);
	}
}
