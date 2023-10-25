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
        static DvtkData::Dul::A_ASSOCIATE_RQ^ 
            Convert(ASSOCIATE_RQ_CLASS *pUMAssociateRq);

        static DvtkData::Dul::A_ASSOCIATE_AC^ 
            Convert(ASSOCIATE_AC_CLASS *pUMAssociateAc);

        static DvtkData::Dul::A_ASSOCIATE_RJ^ 
            Convert(ASSOCIATE_RJ_CLASS *pUMAssociateRj);

        static DvtkData::Dul::A_RELEASE_RQ^ 
            Convert(RELEASE_RQ_CLASS *pUMReleaseRq);

        static DvtkData::Dul::A_RELEASE_RP^ 
            Convert(RELEASE_RP_CLASS *pUMReleaseRp);

        static DvtkData::Dul::A_ABORT^ 
            Convert(ABORT_RQ_CLASS *pUMAbortRq);

    private:
        static DvtkData::Dul::ApplicationContext^ 
            ConvertAC(UID_CLASS& UMUid);

        static DvtkData::Dul::RequestedPresentationContext^
            Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext);

        static DvtkData::Dul::AcceptedPresentationContext^
            Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext);

        static DvtkData::Dul::AbstractSyntax^
            ConvertAS(UID_CLASS& UMUid);

        static DvtkData::Dul::TransferSyntax^
            ConvertTS(UID_CLASS& UMUid);

        static DvtkData::Dul::UserInformation^
            Convert(USER_INFORMATION_CLASS& UMUserInformation);

        static DvtkData::Dul::MaximumLength^
            Convert(UINT32 UMMaximumLength);

        static DvtkData::Dul::ImplementationClassUid^
            ConvertICU(UID_CLASS& UMUid);

        static DvtkData::Dul::ImplementationVersionName^
            Convert(char *pUMImplementationVersionName);

        static DvtkData::Dul::AsynchronousOperationsWindow^
            Convert(UINT16 UMInvoked, UINT16 UMPerformed);

        static DvtkData::Dul::ScpScuRoleSelection^
            Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect);

        static DvtkData::Dul::SopClassExtendedNegotiation^
            Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended);

        static DvtkData::Dul::UserIdentityNegotiation^
            Convert(USER_IDENTITY_NEGOTIATION_CLASS& UMUserIdentityNegotiation);

		static DvtkData::Dul::UserIdentityNegotiation^
			Convert(BYTE UMUserIdentityType,
					BYTE UMPositiveResponseRequested,
					char *UMPrimaryField,
					char *UMSecondaryField);

		static DvtkData::Dul::UserIdentityNegotiation^
			Convert2(char *UMServerResponse);

        static System::String^
            ConvertSCU(UID_CLASS& UMUid);

        //
        // Managed to Unmanaged
        //
    public:
        static ASSOCIATE_RQ_CLASS*
            Convert(DvtkData::Dul::A_ASSOCIATE_RQ ^ pAssociateRq);

        static ASSOCIATE_AC_CLASS*
            Convert(DvtkData::Dul::A_ASSOCIATE_AC ^ pAssociateAc);

        static ASSOCIATE_RJ_CLASS* 
            Convert(DvtkData::Dul::A_ASSOCIATE_RJ ^ pAssociateRj);

        static RELEASE_RQ_CLASS* 
            Convert(DvtkData::Dul::A_RELEASE_RQ ^ pReleaseRq);

        static RELEASE_RP_CLASS* 
            Convert(DvtkData::Dul::A_RELEASE_RP ^ pReleaseRp);

        static ABORT_RQ_CLASS* 
            Convert(DvtkData::Dul::A_ABORT ^ pAbortRq);

    private:
        static string
            Convert(System::String ^ pString);

        static string
            Convert(DvtkData::Dul::ApplicationContext ^ pApplicationContext);

        static void
            Convert(PRESENTATION_CONTEXT_RQ_CLASS& UMRequestedPresentationContext, DvtkData::Dul::RequestedPresentationContext ^ pRequestedPresentationContext);

        static void
            Convert(PRESENTATION_CONTEXT_AC_CLASS& UMAcceptedPresentationContext, DvtkData::Dul::AcceptedPresentationContext ^ pAcceptedPresentationContext);

        static void
            Convert(USER_INFORMATION_CLASS& UMUserInformation, DvtkData::Dul::UserInformation ^ pUserInformation);

        static void
            Convert(UID_CLASS& uid, System::String ^ pString);

        static void
            Convert(SCP_SCU_ROLE_SELECT_CLASS& UMScpScuRoleSelect, DvtkData::Dul::ScpScuRoleSelection ^ pScpScuRoleSelection);

        static void
            Convert(SOP_CLASS_EXTENDED_CLASS& UMSopClassExtended, DvtkData::Dul::SopClassExtendedNegotiation ^ pSopClassExtendedNegotiation);

        static void
            Convert(USER_IDENTITY_NEGOTIATION_CLASS& UMUserIdentityNegotiation, DvtkData::Dul::UserIdentityNegotiation ^ pUserIdentityNegotiation);
		static void
			Convert(string& UMPrimaryField, string& UMSecondaryField, string& UMServerResponse, DvtkData::Dul::UserIdentityNegotiation ^ pUserIdentityNegotiation);
    };

}
