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

#include "ActivityReportingAdapter.h"
#include "CountingAdapter.h"

namespace ManagedUnManagedDulValidationResultsConvertors
{
    using namespace DvtkData::Validation;

    //>>***************************************************************************

    class ManagedUnManagedDulValidationResultsConvertor

        //  DESCRIPTION     : Managed Unmanaged DUL Validation Results Convertor Class.
        //  INVARIANT       :
        //  NOTES           :
        //<<***************************************************************************
    {
    private:
        DvtkData::Validation::MessageType
            Convert(Wrappers::WrappedValidationMessageLevel);

    public:
        //
        // Constructor
        //
        ManagedUnManagedDulValidationResultsConvertor(void);
    public:
        //
        // Destructor
        //
        ~ManagedUnManagedDulValidationResultsConvertor(void);

        //
        // Unmanaged to Managed
        //
    public:
        DvtkData::Validation::ValidationAssociateRq^ 
            Convert(ASSOCIATE_RQ_VALIDATOR_CLASS *pUMAssociateRq);

    public:
        DvtkData::Validation::ValidationAssociateAc^ 
            Convert(ASSOCIATE_AC_VALIDATOR_CLASS *pUMAssociateAc);

    public:
        DvtkData::Validation::ValidationAssociateRj^ 
            Convert(ASSOCIATE_RJ_VALIDATOR_CLASS *pUMAssociateRj);

    public:
        DvtkData::Validation::ValidationReleaseRq^ 
            Convert(RELEASE_RQ_VALIDATOR_CLASS *pUMReleaseRq);

    public:
        DvtkData::Validation::ValidationReleaseRp^ 
            Convert(RELEASE_RP_VALIDATOR_CLASS *pUMReleaseRp);

    public:
        DvtkData::Validation::ValidationAbortRq^ 
            Convert(ABORT_RQ_VALIDATOR_CLASS *pUMAbortRq);

    private:
        DvtkData::Validation::SubItems::ValidationAcseUserInformation^
            Convert(ACSE_USER_INFORMATION_VALIDATOR_CLASS *pUMUserInformation);

    private:
        DvtkData::Validation::SubItems::ValidationAcseScpScuRoleSelect^
            Convert(ACSE_SCP_SCU_ROLE_SELECT_VALIDATOR_CLASS *pUMScpScuRoleSelect);

    private:
        DvtkData::Validation::SubItems::ValidationAcseAsynchronousOperationWindow^
            Convert(ACSE_ASYNCHRONOUS_OPERATION_WINDOW_VALIDATOR_CLASS *pUMAsynchronousOperationWindow);

    private:
        DvtkData::Validation::SubItems::ValidationAcseSopClassExtended^
            Convert(ACSE_SOP_CLASS_EXTENDED_VALIDATOR_CLASS *pUMSopClassExtended);

    private:
        DvtkData::Validation::SubItems::ValidationAcseUserIdentityNegotiation^
            Convert(ACSE_USER_IDENTITY_NEGOTIATION_VALIDATOR_CLASS *pUMUserIdentityNegotiation);

    private:
        DvtkData::Validation::SubItems::ValidationAcseParameter^ 
            Convert(ACSE_PARAMETER_CLASS *pUMAcseParameter);

    public:
        void set_Rules(System::Uri^ pRulesUri);
    private:
        gcroot<System::Uri^> m_pRulesUri;
		gcroot<Wrappers::ICountingTarget^> m_pCountingTarget;

    public:
        void set_CountingTarget(Wrappers::ICountingTarget^ pCountingTarget);
    };
}
