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

    DvtkData::Dul::A_ASSOCIATE_RQ __gc* 
        ManagedUnManagedDulConvertor::Convert(ASSOCIATE_RQ_CLASS *pUMAssociateRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL associate request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRq == NULL) return NULL;

        DvtkData::Dul::A_ASSOCIATE_RQ *pAssociateRq = new DvtkData::Dul::A_ASSOCIATE_RQ();

        // convert parameters
        pAssociateRq->ProtocolVersion = pUMAssociateRq->getProtocolVersion();
        pAssociateRq->CalledAETitle = pUMAssociateRq->getCalledAeTitle();
        pAssociateRq->CallingAETitle = pUMAssociateRq->getCallingAeTitle();

        pAssociateRq->ApplicationContext = ConvertAC(pUMAssociateRq->getApplicationContextName());

        // convert requested persentation contexts
        for (UINT i = 0; i < pUMAssociateRq->noPresentationContexts(); i++)
        {
            DvtkData::Dul::RequestedPresentationContext *pRequestedPresentationContext = Convert(pUMAssociateRq->getPresentationContext(i));
            pAssociateRq->PresentationContexts->Add(pRequestedPresentationContext);
        }

        // convert user information
        pAssociateRq->UserInformation = Convert(pUMAssociateRq->getUserInformation());

        return pAssociateRq;
    }

    //>>===========================================================================

    DvtkData::Dul::A_ASSOCIATE_AC __gc* 
        ManagedUnManagedDulConvertor::Convert(ASSOCIATE_AC_CLASS *pUMAssociateAc)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL associate accept
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateAc == NULL) return NULL;

        DvtkData::Dul::A_ASSOCIATE_AC *pAssociateAc = new DvtkData::Dul::A_ASSOCIATE_AC();

        // convert parameters
        pAssociateAc->ProtocolVersion = pUMAssociateAc->getProtocolVersion();
        pAssociateAc->CalledAETitle = pUMAssociateAc->getCalledAeTitle();
        pAssociateAc->CallingAETitle = pUMAssociateAc->getCallingAeTitle();

        pAssociateAc->ApplicationContext = ConvertAC(pUMAssociateAc->getApplicationContextName());

        // convert accepted presentation contexts
        for (UINT i = 0; i < pUMAssociateAc->noPresentationContexts(); i++)
        {
            DvtkData::Dul::AcceptedPresentationContext *pAcceptedPresentationContext = Convert(pUMAssociateAc->getPresentationContext(i));
            pAssociateAc->PresentationContexts->Add(pAcceptedPresentationContext);
        }

        // convert user information
        pAssociateAc->UserInformation = Convert(pUMAssociateAc->getUserInformation());

        return pAssociateAc;
    }

    //>>===========================================================================

    DvtkData::Dul::A_ASSOCIATE_RJ __gc* 
        ManagedUnManagedDulConvertor::Convert(ASSOCIATE_RJ_CLASS *pUMAssociateRj)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL associate reject
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAssociateRj == NULL) return NULL;

        DvtkData::Dul::A_ASSOCIATE_RJ *pAssociateRj = new DvtkData::Dul::A_ASSOCIATE_RJ();

        // convert parameters
        pAssociateRj->Result = pUMAssociateRj->getResult();
        pAssociateRj->Source = pUMAssociateRj->getSource();
        pAssociateRj->Reason = pUMAssociateRj->getReason();

        return pAssociateRj;
    }

    //>>===========================================================================

    DvtkData::Dul::A_RELEASE_RQ __gc* 
        ManagedUnManagedDulConvertor::Convert(RELEASE_RQ_CLASS *pUMReleaseRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL release request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRq == NULL) return NULL;

        DvtkData::Dul::A_RELEASE_RQ *pReleaseRq = new DvtkData::Dul::A_RELEASE_RQ();

        // no parameters to convert
        return pReleaseRq;
    }

    //>>===========================================================================

    DvtkData::Dul::A_RELEASE_RP __gc* 
        ManagedUnManagedDulConvertor::Convert(RELEASE_RP_CLASS *pUMReleaseRp)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL release response
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMReleaseRp == NULL) return NULL;

        DvtkData::Dul::A_RELEASE_RP *pReleaseRp = new DvtkData::Dul::A_RELEASE_RP();

        // no parameters to convert
        return pReleaseRp;
    }

    //>>===========================================================================

    DvtkData::Dul::A_ABORT __gc* 
        ManagedUnManagedDulConvertor::Convert(ABORT_RQ_CLASS *pUMAbortRq)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL abort request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAbortRq == NULL) return NULL;

        DvtkData::Dul::A_ABORT *pAbortRq = new DvtkData::Dul::A_ABORT();

        // convert parameters
        pAbortRq->Source = pUMAbortRq->getSource();
        pAbortRq->Reason = pUMAbortRq->getReason();

        return pAbortRq;
    }

    //>>===========================================================================

    DvtkData::Dul::ApplicationContext __gc* 
        ManagedUnManagedDulConvertor::ConvertAC(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL application context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::ApplicationContext *pApplicationContext = new DvtkData::Dul::ApplicationContext();

        // convert application context
        System::String *pValue = new System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        pApplicationContext->Name = pValue;

        return pApplicationContext;
    }

    //>>===========================================================================

    DvtkData::Dul::RequestedPresentationContext __gc*
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL requested presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::RequestedPresentationContext *pRequestedPresentationContext = new DvtkData::Dul::RequestedPresentationContext();

        // convert parameters
        pRequestedPresentationContext->ID = UMRequestedPresentationContext.getPresentationContextId();
        pRequestedPresentationContext->AbstractSyntax = ConvertAS(UMRequestedPresentationContext.getAbstractSyntaxName());

        // convert transfer syntaxes
        for (UINT i = 0; i < UMRequestedPresentationContext.noTransferSyntaxNames(); i++)
        {
            DvtkData::Dul::TransferSyntax *pTransferSyntax = ConvertTS(UMRequestedPresentationContext.getTransferSyntaxName(i));
            pRequestedPresentationContext->TransferSyntaxes->Add(pTransferSyntax);
        }

        return pRequestedPresentationContext;
    }

    //>>===========================================================================

    DvtkData::Dul::AcceptedPresentationContext __gc*
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL accepted presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::AcceptedPresentationContext *pAcceptedPresentationContext = new DvtkData::Dul::AcceptedPresentationContext();

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

    DvtkData::Dul::AbstractSyntax __gc*
        ManagedUnManagedDulConvertor::ConvertAS(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL abstract syntax
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {

        // convert abstract syntax
        System::String *pValue = new System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        DvtkData::Dul::AbstractSyntax *pAbstractSyntax = new DvtkData::Dul::AbstractSyntax(pValue);

        return pAbstractSyntax;
    }

    //>>===========================================================================

    DvtkData::Dul::TransferSyntax __gc*
        ManagedUnManagedDulConvertor::ConvertTS(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL transfer syntax
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {

        // convert transfer syntax
        System::String *pValue = new System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        DvtkData::Dul::TransferSyntax *pTransferSyntax = new DvtkData::Dul::TransferSyntax(pValue);

        return pTransferSyntax;
    }

    //>>===========================================================================

    DvtkData::Dul::UserInformation __gc*
        ManagedUnManagedDulConvertor::Convert(USER_INFORMATION_CLASS& UMUserInformation)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL user information
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::UserInformation *pUserInformation = new DvtkData::Dul::UserInformation();

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
            DvtkData::Dul::ScpScuRoleSelectionList *pScpScuRoleSelectionList = new DvtkData::Dul::ScpScuRoleSelectionList();
            pUserInformation->ScpScuRoleSelections = pScpScuRoleSelectionList;

            for (UINT i = 0; i < UMUserInformation.noScpScuRoleSelects(); i++)
            {
                DvtkData::Dul::ScpScuRoleSelection *pScpScuRoleSelection = Convert(UMUserInformation.getScpScuRoleSelect(i));
                pScpScuRoleSelectionList->Add(pScpScuRoleSelection);
            }
        }

        // optional SOP Class Extended Negotiation
        if (UMUserInformation.noSopClassExtendeds())
        {
            DvtkData::Dul::SopClassExtendedNegotiationList *pSopClassExtendedNegotiationList = new SopClassExtendedNegotiationList();
            pUserInformation->SopClassExtendedNegotiations = pSopClassExtendedNegotiationList;

            for (UINT i = 0; i < UMUserInformation.noSopClassExtendeds(); i++)
            {
                DvtkData::Dul::SopClassExtendedNegotiation *pSopClassExtendedNegotiation = Convert(UMUserInformation.getSopClassExtended(i));
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

    DvtkData::Dul::MaximumLength __gc*
        ManagedUnManagedDulConvertor::Convert(UINT32 UMMaximumLength)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL maximum length received
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::MaximumLength *pMaximumLength = new DvtkData::Dul::MaximumLength();
        pMaximumLength->MaximumLengthReceived = UMMaximumLength;

        // convert maxmum length
        return pMaximumLength;
    }

    //>>===========================================================================

    DvtkData::Dul::ImplementationClassUid __gc*
        ManagedUnManagedDulConvertor::ConvertICU(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL implementation class uid
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::ImplementationClassUid *pImplementationClassUid = new DvtkData::Dul::ImplementationClassUid();

        // convert implementation class uid
        System::String *pValue = new System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());
        pImplementationClassUid->UID = pValue;

        return pImplementationClassUid;
    }

    //>>===========================================================================

    DvtkData::Dul::ImplementationVersionName __gc*
        ManagedUnManagedDulConvertor::Convert(char *pUMImplementationVersionName)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL implementation version name
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMImplementationVersionName == NULL) return NULL;

        // convert implementation version name
        DvtkData::Dul::ImplementationVersionName *pImplementationVersionName = new DvtkData::Dul::ImplementationVersionName();

        pImplementationVersionName->Name = pUMImplementationVersionName;

        return pImplementationVersionName;
    }

    //>>===========================================================================

    DvtkData::Dul::AsynchronousOperationsWindow __gc*
        ManagedUnManagedDulConvertor::Convert(UINT16 UMInvoked, UINT16 UMPerformed)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL asynchronous operations window
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::AsynchronousOperationsWindow *pAsynchronousOperationsWindow = new DvtkData::Dul::AsynchronousOperationsWindow();

        // convert parameters
        pAsynchronousOperationsWindow->MaximumNumberOperationsInvoked = UMInvoked;
        pAsynchronousOperationsWindow->MaximumNumberOperationsPerformed = UMPerformed;

        return pAsynchronousOperationsWindow;
    }

    //>>===========================================================================

    DvtkData::Dul::ScpScuRoleSelection __gc*
        ManagedUnManagedDulConvertor::Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL SCP SCU role selection
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::ScpScuRoleSelection *pScpScuRoleSelect = new DvtkData::Dul::ScpScuRoleSelection();

        // convert parameters
        pScpScuRoleSelect->SopClassUid = ConvertSCU(UMScpScuRoleSelect.getUid());
        pScpScuRoleSelect->ScpRole = UMScpScuRoleSelect.getScpRole();
        pScpScuRoleSelect->ScuRole = UMScpScuRoleSelect.getScuRole();

        return pScpScuRoleSelect;
    }

    //>>===========================================================================

    DvtkData::Dul::SopClassExtendedNegotiation __gc*
        ManagedUnManagedDulConvertor::Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL SOP class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::SopClassExtendedNegotiation *pSopClassExtendedNegotiation = new DvtkData::Dul::SopClassExtendedNegotiation();

        // convert parameters
        pSopClassExtendedNegotiation->SopClassUid = ConvertSCU(UMSopClassExtended.getUid());

        UINT length = UMSopClassExtended.getNoApplicationInformation();
        if (length)
        {
            System::Byte byteArray[] = new System::Byte[length];

            for (UINT i = 0; i < UMSopClassExtended.getNoApplicationInformation(); i++)
            {
                byteArray[i] = UMSopClassExtended.getApplicationInformation(i);
            }
            pSopClassExtendedNegotiation->ServiceClassApplicationInformation = byteArray;
        }

        return pSopClassExtendedNegotiation;
    }

    //>>===========================================================================

    DvtkData::Dul::UserIdentityNegotiation __gc*
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
        DvtkData::Dul::UserIdentityNegotiation *pUserIdentityNegotiation = new DvtkData::Dul::UserIdentityNegotiation();

        // convert parameters
		pUserIdentityNegotiation->UserIdentityType = UMUserIdentityType;
		pUserIdentityNegotiation->PositiveResponseRequested = UMPositiveResponseRequested;
		if (UMPrimaryField)
		{
			pUserIdentityNegotiation->PrimaryField = UMPrimaryField;
		}
		if (UMSecondaryField)
		{
			pUserIdentityNegotiation->SecondaryField = UMSecondaryField;
		}
        return pUserIdentityNegotiation;
    }

    //>>===========================================================================

    DvtkData::Dul::UserIdentityNegotiation __gc*
        ManagedUnManagedDulConvertor::Convert2(char *UMServerResponse)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL user identity negotiation
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dul::UserIdentityNegotiation *pUserIdentityNegotiation = new DvtkData::Dul::UserIdentityNegotiation();

        // convert parameters
		pUserIdentityNegotiation->ServerResponse = UMServerResponse;

        return pUserIdentityNegotiation;
    }

    //>>===========================================================================

    System::String __gc*
        ManagedUnManagedDulConvertor::ConvertSCU(UID_CLASS& UMUid)

        //  DESCRIPTION     : Convert unmanaged to managed - DUL SOP class uid
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        System::String *pValue = new System::String((char*)UMUid.get(), 0, (int)UMUid.getLength());

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
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ASSOCIATE_RQ __gc *pAssociateRq)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL associate request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAssociateRq == NULL) return NULL;

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
                DvtkData::Dul::RequestedPresentationContext *pRequestedPresentationContext = pAssociateRq->PresentationContexts->Item[i];
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
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ASSOCIATE_AC __gc *pAssociateAc)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL associate accept
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAssociateAc == NULL) return NULL;

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
                DvtkData::Dul::AcceptedPresentationContext *pAcceptedPresentationContext = pAssociateAc->PresentationContexts->Item[i];
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
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ASSOCIATE_RJ __gc *pAssociateRj)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL associate reject
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAssociateRj == NULL) return NULL;

        ASSOCIATE_RJ_CLASS *pUMAssociateRj = new ASSOCIATE_RJ_CLASS();

        // convert properties
        pUMAssociateRj->setResult(pAssociateRj->Result);
        pUMAssociateRj->setSource(pAssociateRj->Source);
        pUMAssociateRj->setReason(pAssociateRj->Reason);

        return pUMAssociateRj;
    }

    //>>===========================================================================

    RELEASE_RQ_CLASS* 
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_RELEASE_RQ __gc *pReleaseRq)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL release request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pReleaseRq == NULL) return NULL;

        RELEASE_RQ_CLASS *pUMReleaseRq = new RELEASE_RQ_CLASS();

        // no properties to convert
        return pUMReleaseRq;
    }

    //>>===========================================================================

    RELEASE_RP_CLASS* 
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_RELEASE_RP __gc *pReleaseRp)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL release response
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pReleaseRp == NULL) return NULL;

        RELEASE_RP_CLASS *pUMReleaseRp = new RELEASE_RP_CLASS();

        // no properties to convert
        return pUMReleaseRp;
    }

    //>>===========================================================================

    ABORT_RQ_CLASS* 
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::A_ABORT __gc *pAbortRq)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL abort request
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAbortRq == NULL) return NULL;

        ABORT_RQ_CLASS *pUMAbortRq = new ABORT_RQ_CLASS();

        // convert properties
        pUMAbortRq->setSource(pAbortRq->Source);
        pUMAbortRq->setReason(pAbortRq->Reason);

        return pUMAbortRq;
    }

    //>>===========================================================================

    string
        ManagedUnManagedDulConvertor::Convert(DvtkData::Dul::ApplicationContext __gc *pApplicationContext)

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
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext, DvtkData::Dul::RequestedPresentationContext __gc *pRequestedPresentationContext)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL requested presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pRequestedPresentationContext == NULL) return;

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
                DvtkData::Dul::TransferSyntax *pTransferSyntax = pRequestedPresentationContext->TransferSyntaxes->Item[i];
                Convert(uid, pTransferSyntax->UID);
                UMRequestedPresentationContext.addTransferSyntaxName(uid);
            }
        }
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext, DvtkData::Dul::AcceptedPresentationContext __gc *pAcceptedPresentationContext)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL accepted presentation context
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAcceptedPresentationContext == NULL) return;

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
        ManagedUnManagedDulConvertor::Convert(USER_INFORMATION_CLASS& UMUserInformation, DvtkData::Dul::UserInformation __gc *pUserInformation)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL user information
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUserInformation == NULL) return;

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
                DvtkData::Dul::ScpScuRoleSelection *pScpScuRoleSelection = pUserInformation->ScpScuRoleSelections->Item[i];
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
                DvtkData::Dul::SopClassExtendedNegotiation *pSopClassExtendedNegotiation = pUserInformation->SopClassExtendedNegotiations->Item[i];
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
        ManagedUnManagedDulConvertor::Convert(UID_CLASS& uid, System::String __gc *pString)

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
        ManagedUnManagedDulConvertor::Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect, DvtkData::Dul::ScpScuRoleSelection __gc *pScpScuRoleSelection)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL SCP SCU role select
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pScpScuRoleSelection == NULL) return;

        // convert properties
        string value;
        MarshalString(pScpScuRoleSelection->SopClassUid, value);

        UMScpScuRoleSelect.setUid((char*)value.c_str());
        UMScpScuRoleSelect.setScpRole(pScpScuRoleSelection->ScpRole); 
        UMScpScuRoleSelect.setScuRole(pScpScuRoleSelection->ScuRole);
    }

    //>>===========================================================================

    void
        ManagedUnManagedDulConvertor::Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended, DvtkData::Dul::SopClassExtendedNegotiation __gc *pSopClassExtendedNegotiation)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL SOP class extended
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pSopClassExtendedNegotiation == NULL) return;

        // convert properties
        string value;
        MarshalString(pSopClassExtendedNegotiation->SopClassUid, value);

        UMSopClassExtended.setUid((char*)value.c_str());

        for (int i = 0; i < pSopClassExtendedNegotiation->ServiceClassApplicationInformation->Count; i++)
        {
            UMSopClassExtended.addApplicationInformation(pSopClassExtendedNegotiation->ServiceClassApplicationInformation[i]);
        }
    }

	void
        ManagedUnManagedDulConvertor::Convert(string& UMPrimaryField, string& UMSecondaryField, string& UMServerResponse, DvtkData::Dul::UserIdentityNegotiation __gc *pUserIdentityNegotiation)

        //  DESCRIPTION     : Convert managed to unmanaged - DUL User identity negotiation
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
	{
        if (pUserIdentityNegotiation == NULL) return;

		// convert properties
		MarshalString(pUserIdentityNegotiation->PrimaryField, UMPrimaryField);
		MarshalString(pUserIdentityNegotiation->SecondaryField, UMSecondaryField);
	    MarshalString(pUserIdentityNegotiation->ServerResponse, UMServerResponse);
	}
}
