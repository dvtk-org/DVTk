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

namespace ManagedUnManagedDimseValidationResultsConvertors
{
    using namespace DvtkData::Validation;

    //>>***************************************************************************

    class ManagedUnManagedDimseValidationResultsConvertor

        //  DESCRIPTION     : Managed Unmanaged DIMSE Validation Results Convertor Class.
        //  INVARIANT       :
        //  NOTES           :
        //<<***************************************************************************
    {
    private:
        //
        // static enum conversion from unmanaged to managed
        //
        static 
            DvtkData::Validation::DataElementType
            Convert(::ATTR_TYPE_ENUM dataElementType);

    private:
        //
        // static enum conversion from unmanaged to managed
        //
        static 
            DvtkData::Validation::DirectoryRecordType
            Convert(::DICOMDIR_RECORD_TYPE_ENUM directoryRecordType);

    private:
        DvtkData::Validation::MessageType
            Convert(Wrappers::WrappedValidationMessageLevel);

    private:
        //
        // static enum conversion from unmanaged to managed
        //
        static 
            DvtkData::Validation::ModuleUsageType
            Convert(::MOD_USAGE_ENUM moduleUsage);

    public:
        //
        // Constructor
        //
        ManagedUnManagedDimseValidationResultsConvertor(void);

    public:
        //
        // Destructor
        //
        ~ManagedUnManagedDimseValidationResultsConvertor(void);

        //
        // Unmanaged to Managed
        //
    public:
        DvtkData::Validation::ValidationObjectResult^
            Convert(VAL_OBJECT_RESULTS_CLASS *pUMValidationObjectResult, UINT flags);

    private:
        DvtkData::Validation::SubItems::ValidationAttributeGroupResult^
            Convert(VAL_ATTRIBUTE_GROUP_CLASS *pUMValidationAttributeGroupResult, UINT flags);

    public:
        DvtkData::Validation::ValidationDirectoryRecordResult^
            Convert(RECORD_RESULTS_CLASS *pUMValidationDirectoryRecordResult, UINT flags);

    public:
        DvtkData::Validation::TypeSafeCollections::ValidationDirectoryRecordLinkCollection^
            Convert(RECORD_LINK_VECTOR* pUMRecordLinkVector);

    private:
        DvtkData::Validation::SubItems::ValidationAttributeResult^
            Convert(VAL_ATTRIBUTE_CLASS *pUMValidationAttributeResult, UINT flags);

    private:
        DvtkData::Validation::SubItems::ValidationValueResult^
            Convert(::VAL_BASE_VALUE_CLASS *pUMValBaseValue, ::ATTR_VR_ENUM vr, UINT flags);

    public:
        void set_Rules(System::Uri^ pRulesUri);
    private:
        gcroot<System::Uri^> m_pRulesUri;
		gcroot<Wrappers::ICountingTarget^> m_pCountingTarget;

    public:
        void set_CountingTarget(Wrappers::ICountingTarget^ pCountingTarget);
    };
}
