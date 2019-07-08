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

namespace ManagedUnManagedDulConvertors
{
    using namespace DvtkData::Dul;

    //>>***************************************************************************

    class ManagedUnManagedDulConvertor

        //  DESCRIPTION     : Managed Unmanaged DUL Convertor Class.
        //  INVARIANT       :
        //  NOTES           :
        //<<***************************************************************************
    {
    public:
        ManagedUnManagedDulConvertor(void);
        ~ManagedUnManagedDulConvertor(void);

        //
        // Unmanaged to Managed
        //
    public:
        static DvtkData::Dul::A_ASSOCIATE_RQ __gc* 
            Convert(ASSOCIATE_RQ_CLASS *pUMAssociateRq);

        static DvtkData::Dul::A_ASSOCIATE_AC __gc* 
            Convert(ASSOCIATE_AC_CLASS *pUMAssociateAc);

        static DvtkData::Dul::A_ASSOCIATE_RJ __gc* 
            Convert(ASSOCIATE_RJ_CLASS *pUMAssociateRj);

        static DvtkData::Dul::A_RELEASE_RQ __gc* 
            Convert(RELEASE_RQ_CLASS *pUMReleaseRq);

        static DvtkData::Dul::A_RELEASE_RP __gc* 
            Convert(RELEASE_RP_CLASS *pUMReleaseRp);

        static DvtkData::Dul::A_ABORT __gc* 
            Convert(ABORT_RQ_CLASS *pUMAbortRq);

    private:
        static DvtkData::Dul::ApplicationContext __gc* 
            ConvertAC(UID_CLASS& UMUid);

        static DvtkData::Dul::RequestedPresentationContext __gc*
            Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext);

        static DvtkData::Dul::AcceptedPresentationContext __gc*
            Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext);

        static DvtkData::Dul::AbstractSyntax __gc*
            ConvertAS(UID_CLASS& UMUid);

        static DvtkData::Dul::TransferSyntax __gc*
            ConvertTS(UID_CLASS& UMUid);

        static DvtkData::Dul::UserInformation __gc*
            Convert(USER_INFORMATION_CLASS& UMUserInformation);

        static DvtkData::Dul::MaximumLength __gc*
            Convert(UINT32 UMMaximumLength);

        static DvtkData::Dul::ImplementationClassUid __gc*
            ConvertICU(UID_CLASS& UMUid);

        static DvtkData::Dul::ImplementationVersionName __gc*
            Convert(char *pUMImplementationVersionName);

        static DvtkData::Dul::AsynchronousOperationsWindow __gc*
            Convert(UINT16 UMInvoked, UINT16 UMPerformed);

        static DvtkData::Dul::ScpScuRoleSelection __gc*
            Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect);

        static DvtkData::Dul::SopClassExtendedNegotiation __gc*
            Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended);

        static DvtkData::Dul::UserIdentityNegotiation __gc*
            Convert(USER_IDENTITY_NEGOTIATION_CLASS& UMUserIdentityNegotiation);

		static DvtkData::Dul::UserIdentityNegotiation __gc*
			Convert(BYTE UMUserIdentityType,
					BYTE UMPositiveResponseRequested,
					char *UMPrimaryField,
					char *UMSecondaryField);

		static DvtkData::Dul::UserIdentityNegotiation __gc*
			Convert2(char *UMServerResponse);

        static System::String __gc*
            ConvertSCU(UID_CLASS& UMUid);

        //
        // Managed to Unmanaged
        //
    public:
        static ASSOCIATE_RQ_CLASS*
            Convert(DvtkData::Dul::A_ASSOCIATE_RQ __gc *pAssociateRq);

        static ASSOCIATE_AC_CLASS*
            Convert(DvtkData::Dul::A_ASSOCIATE_AC __gc *pAssociateAc);

        static ASSOCIATE_RJ_CLASS* 
            Convert(DvtkData::Dul::A_ASSOCIATE_RJ __gc *pAssociateRj);

        static RELEASE_RQ_CLASS* 
            Convert(DvtkData::Dul::A_RELEASE_RQ __gc *pReleaseRq);

        static RELEASE_RP_CLASS* 
            Convert(DvtkData::Dul::A_RELEASE_RP __gc *pReleaseRp);

        static ABORT_RQ_CLASS* 
            Convert(DvtkData::Dul::A_ABORT __gc *pAbortRq);

    private:
        static string
            Convert(System::String __gc *pString);

        static string
            Convert(DvtkData::Dul::ApplicationContext __gc *pApplicationContext);

        static void
            Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext, DvtkData::Dul::RequestedPresentationContext __gc *pRequestedPresentationContext);

        static void
            Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext, DvtkData::Dul::AcceptedPresentationContext __gc *pAcceptedPresentationContext);

        static void
            Convert(USER_INFORMATION_CLASS& UMUserInformation, DvtkData::Dul::UserInformation __gc *pUserInformation);

        static void
            Convert(UID_CLASS& uid, System::String __gc *pString);

        static void
            Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect, DvtkData::Dul::ScpScuRoleSelection __gc *pScpScuRoleSelection);

        static void
            Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended, DvtkData::Dul::SopClassExtendedNegotiation __gc *pSopClassExtendedNegotiation);

        static void
            Convert(USER_IDENTITY_NEGOTIATION_CLASS& UMUserIdentityNegotiation, DvtkData::Dul::UserIdentityNegotiation __gc *pUserIdentityNegotiation);
		static void
			Convert(string& UMPrimaryField, string& UMSecondaryField, string& UMServerResponse, DvtkData::Dul::UserIdentityNegotiation __gc *pUserIdentityNegotiation);
    };

}
