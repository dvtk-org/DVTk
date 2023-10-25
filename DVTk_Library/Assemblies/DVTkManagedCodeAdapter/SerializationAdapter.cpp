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
#include "SerializationAdapter.h"
#include "mdimseconvertors.h"
#include "mdulconvertors.h"
#include "MDULResultsConvertors.h"
#include "MDIMSEResultsConvertors.h"
#include "MMediaConvertors.h"
#include "MBaseSession.h"
#include < vcclr.h >
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace ManagedUnManagedDimseConvertors;
    using namespace ManagedUnManagedDulConvertors;
    using namespace ManagedUnManagedDulValidationResultsConvertors;
    using namespace ManagedUnManagedDimseValidationResultsConvertors;
    using namespace ManagedUnManagedMediaConvertors;

    SerializationAdapter::SerializationAdapter(
		ISerializationTarget^ serializationTarget,
		ICountingTarget^ countingTarget,
		System::Uri^ rulesUri)
    {
        this->m_pSerializationTarget = serializationTarget;
		this->m_pCountingTarget = countingTarget;
		this->m_dimseValidationResultsConverter.set_CountingTarget(countingTarget);
		this->m_dulValidationResultsConverter.set_CountingTarget(countingTarget);
		this->set_Rules(rulesUri);
        const bool bEnded = false;
		this->m_bEnded = bEnded;
        this->m_endedManualResetEvent = gcnew System::Threading::ManualResetEvent(bEnded);
    }

    SerializationAdapter::~SerializationAdapter(void)
    {
		if (this->childSerializationAdaptersM_ptr.getSize() != 0)
		{
			throw gcnew System::ApplicationException(
				System::String::Concat(
				"Child serializer should have been destroyed by calls to ",
				"UnregisterAndDestroyChildSerializer."));
		}
        this->m_endedManualResetEvent->Close();
		// free up member structures
		/* TODO: Remove obsolete code.
		while (this->childSerializationAdaptersM_ptr.getSize())
		{
			delete this->childSerializationAdaptersM_ptr[0];
			this->childSerializationAdaptersM_ptr.removeAt(0);
		}
		*/
    }

    void SerializationAdapter::SerializeSend(
        DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS, DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::CommandSet^ pCommandSet = nullptr;
        DvtkData::Dimse::DataSet^ pDataSet = nullptr;
        if (pDCM_COMMAND_CLASS != nullptr)
            pCommandSet = ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        if (pDCM_DATASET_CLASS != nullptr)
            pDataSet = ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        DvtkData::Dimse::DicomMessage^ pDicomMessage = gcnew DvtkData::Dimse::DicomMessage();
        pDicomMessage->CommandSet = pCommandSet;
        pDicomMessage->DataSet = pDataSet;
        m_pSerializationTarget->SerializeSend(pDicomMessage);
    }

    void SerializationAdapter::SerializeSend(ABORT_RQ_CLASS* pABORT_RQ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pABORT_RQ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pABORT_RQ_CLASS);
        m_pSerializationTarget->SerializeSend(pDulMessage);
    }

    void SerializationAdapter::SerializeSend(ASSOCIATE_AC_CLASS* pASSOCIATE_AC_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pASSOCIATE_AC_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pASSOCIATE_AC_CLASS);
        m_pSerializationTarget->SerializeSend(pDulMessage);
    }

    void SerializationAdapter::SerializeSend(ASSOCIATE_RJ_CLASS* pASSOCIATE_RJ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pASSOCIATE_RJ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pASSOCIATE_RJ_CLASS);
        m_pSerializationTarget->SerializeSend(pDulMessage);
    }

    void SerializationAdapter::SerializeSend(ASSOCIATE_RQ_CLASS* pASSOCIATE_RQ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pASSOCIATE_RQ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pASSOCIATE_RQ_CLASS);
        m_pSerializationTarget->SerializeSend(pDulMessage);
    }

    void SerializationAdapter::SerializeSend(RELEASE_RP_CLASS* pRELEASE_RP_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pRELEASE_RP_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pRELEASE_RP_CLASS);
        m_pSerializationTarget->SerializeSend(pDulMessage);
    }

    void SerializationAdapter::SerializeSend(RELEASE_RQ_CLASS* pRELEASE_RQ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pRELEASE_RQ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pRELEASE_RQ_CLASS);
        m_pSerializationTarget->SerializeSend(pDulMessage);
    }

    void SerializationAdapter::SerializeReceive(
        DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS, DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::CommandSet^ pCommandSet = nullptr;
        DvtkData::Dimse::DataSet^ pDataSet = nullptr;
        if (pDCM_COMMAND_CLASS != nullptr)
            pCommandSet = ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        if (pDCM_DATASET_CLASS != nullptr)
            pDataSet = ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        DvtkData::Dimse::DicomMessage^ pDicomMessage = gcnew DvtkData::Dimse::DicomMessage();
        pDicomMessage->CommandSet = pCommandSet;
        pDicomMessage->DataSet = pDataSet;
        m_pSerializationTarget->SerializeReceive(pDicomMessage);
    }

    void SerializationAdapter::SerializeReceive(ABORT_RQ_CLASS* pABORT_RQ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pABORT_RQ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pABORT_RQ_CLASS);
        m_pSerializationTarget->SerializeReceive(pDulMessage);
    }

    void SerializationAdapter::SerializeReceive(ASSOCIATE_AC_CLASS* pASSOCIATE_AC_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pASSOCIATE_AC_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pASSOCIATE_AC_CLASS);
        m_pSerializationTarget->SerializeReceive(pDulMessage);
    }

    void SerializationAdapter::SerializeReceive(ASSOCIATE_RJ_CLASS* pASSOCIATE_RJ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pASSOCIATE_RJ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pASSOCIATE_RJ_CLASS);
        m_pSerializationTarget->SerializeReceive(pDulMessage);
    }

    void SerializationAdapter::SerializeReceive(ASSOCIATE_RQ_CLASS* pASSOCIATE_RQ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pASSOCIATE_RQ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pASSOCIATE_RQ_CLASS);
        m_pSerializationTarget->SerializeReceive(pDulMessage);
    }

    void SerializationAdapter::SerializeReceive(RELEASE_RP_CLASS* pRELEASE_RP_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pRELEASE_RP_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pRELEASE_RP_CLASS);
        m_pSerializationTarget->SerializeReceive(pDulMessage);
    }

    void SerializationAdapter::SerializeReceive(RELEASE_RQ_CLASS* pRELEASE_RQ_CLASS)
    {
        DvtkData::Dul::DulMessage^ pDulMessage = nullptr;
        if (pRELEASE_RQ_CLASS != nullptr)
            pDulMessage = ManagedUnManagedDulConvertor::Convert(pRELEASE_RQ_CLASS);
        m_pSerializationTarget->SerializeReceive(pDulMessage);
    }

    void SerializationAdapter::SerializeValidate(
        VAL_OBJECT_RESULTS_CLASS* pVAL_OBJECT_RESULTS_CLASS, 
        UINT flags)
    {
        DvtkData::Validation::ValidationObjectResult^ pValidationObjectResult = nullptr;
        if (pVAL_OBJECT_RESULTS_CLASS != nullptr)
        {
            pValidationObjectResult = m_dimseValidationResultsConverter.Convert(pVAL_OBJECT_RESULTS_CLASS, flags);
        }
        m_pSerializationTarget->SerializeValidate(pValidationObjectResult);
    }

    void SerializationAdapter::SerializeValidate(
        VAL_OBJECT_RESULTS_CLASS* pVAL_OBJECT_RESULTS_CLASS,
        RECORD_LINK_VECTOR* pRecordLinks,
        UINT flags)
    {
        DvtkData::Validation::ValidationObjectResult^ pValidationObjectResult = nullptr;
        DvtkData::Validation::TypeSafeCollections::ValidationDirectoryRecordLinkCollection
           ^ pRecordLinkCollection = nullptr;
        if (pVAL_OBJECT_RESULTS_CLASS != nullptr)
        {
            pValidationObjectResult = m_dimseValidationResultsConverter.Convert(pVAL_OBJECT_RESULTS_CLASS, flags);
        }
        if (pRecordLinks != nullptr)
        {
            // Convert pRecordLinks => pRecordLinkCollection
            pRecordLinkCollection =
                m_dimseValidationResultsConverter.Convert(pRecordLinks);
        }
        // assign pRecordLinkCollection to pValidationObjectResult
        pValidationObjectResult->DirectoryRecordTOC = pRecordLinkCollection;
        m_pSerializationTarget->SerializeValidate(pValidationObjectResult);
    }

    void SerializationAdapter::SerializeValidate(
        ABORT_RQ_VALIDATOR_CLASS* pABORT_RQ_VALIDATOR_CLASS)
    {
        DvtkData::Validation::ValidationAbortRq^ pValidationAbortRq = nullptr;
        if (pABORT_RQ_VALIDATOR_CLASS != nullptr)
        {
            pValidationAbortRq =
                m_dulValidationResultsConverter.Convert(pABORT_RQ_VALIDATOR_CLASS);
        }
        m_pSerializationTarget->SerializeValidate(pValidationAbortRq);
    }

    void SerializationAdapter::SerializeValidate(
        ASSOCIATE_AC_VALIDATOR_CLASS* pASSOCIATE_AC_VALIDATOR_CLASS)
    {
        DvtkData::Validation::ValidationAssociateAc^ pValidationAssociateAc = nullptr;
        if (pASSOCIATE_AC_VALIDATOR_CLASS != nullptr)
        {
            pValidationAssociateAc =
                m_dulValidationResultsConverter.Convert(pASSOCIATE_AC_VALIDATOR_CLASS);
        }
        m_pSerializationTarget->SerializeValidate(pValidationAssociateAc);
    }

    void SerializationAdapter::SerializeValidate(
        ASSOCIATE_RJ_VALIDATOR_CLASS* pASSOCIATE_RJ_VALIDATOR_CLASS)
    {
        DvtkData::Validation::ValidationAssociateRj^ pValidationAssociateRj = nullptr;
        if (pASSOCIATE_RJ_VALIDATOR_CLASS != nullptr)
        {
            pValidationAssociateRj =
                m_dulValidationResultsConverter.Convert(pASSOCIATE_RJ_VALIDATOR_CLASS);
        }
        m_pSerializationTarget->SerializeValidate(pValidationAssociateRj);
    }

    void SerializationAdapter::SerializeValidate(
        ASSOCIATE_RQ_VALIDATOR_CLASS* pASSOCIATE_RQ_VALIDATOR_CLASS)
    {
        DvtkData::Validation::ValidationAssociateRq^ pValidationAssociateRq = nullptr;
        if (pASSOCIATE_RQ_VALIDATOR_CLASS != nullptr)
        {
            pValidationAssociateRq =
                m_dulValidationResultsConverter.Convert(pASSOCIATE_RQ_VALIDATOR_CLASS);
        }
        m_pSerializationTarget->SerializeValidate(pValidationAssociateRq);
    }

    void SerializationAdapter::SerializeValidate(
        RELEASE_RP_VALIDATOR_CLASS* pRELEASE_RP_VALIDATOR_CLASS)
    {
        DvtkData::Validation::ValidationReleaseRp^ pValidationReleaseRp = nullptr;
        if (pRELEASE_RP_VALIDATOR_CLASS != nullptr)
        {
            pValidationReleaseRp =
                m_dulValidationResultsConverter.Convert(pRELEASE_RP_VALIDATOR_CLASS);
        }
        m_pSerializationTarget->SerializeValidate(pValidationReleaseRp);
    }

    void SerializationAdapter::SerializeValidate(
        RELEASE_RQ_VALIDATOR_CLASS* pRELEASE_RQ_VALIDATOR_CLASS)
    {
        DvtkData::Validation::ValidationReleaseRq^ pValidationReleaseRq = nullptr;
        if (pRELEASE_RQ_VALIDATOR_CLASS != nullptr)
        {
            pValidationReleaseRq =
                m_dulValidationResultsConverter.Convert(pRELEASE_RQ_VALIDATOR_CLASS);
        }
        m_pSerializationTarget->SerializeValidate(pValidationReleaseRq);
    }

    void SerializationAdapter::SerializeValidate(
        RECORD_RESULTS_CLASS* pRECORD_RESULTS_CLASS,
        UINT flags)
    {
        DvtkData::Validation::ValidationDirectoryRecordResult^ 
            pValidationDirectoryRecordResult = nullptr;
        if (pRECORD_RESULTS_CLASS != nullptr)
        {
            pValidationDirectoryRecordResult =
                m_dimseValidationResultsConverter.Convert(pRECORD_RESULTS_CLASS, flags);
        }
        m_pSerializationTarget->SerializeValidate(pValidationDirectoryRecordResult);
    }

    void SerializationAdapter::SerializeDisplay(DCM_ATTRIBUTE_CLASS* pDCM_ATTRIBUTE_CLASS)
    {
        DvtkData::Dimse::Attribute^ pAttribute = nullptr;
        if (pDCM_ATTRIBUTE_CLASS != nullptr)
        {
            pAttribute =
                ManagedUnManagedDimseConvertor::Convert(pDCM_ATTRIBUTE_CLASS);
        }
        m_pSerializationTarget->SerializeDisplay(pAttribute);
    }

    void SerializationAdapter::SerializeDisplay(
        ::DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS,
        ::DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::DicomMessage^ pDicomMessage = gcnew DvtkData::Dimse::DicomMessage();
        if (pDCM_COMMAND_CLASS != nullptr)
        {
            pDicomMessage->CommandSet =
                ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        }
        if (pDCM_DATASET_CLASS != nullptr)
        {
            pDicomMessage->DataSet =
                ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        }
        m_pSerializationTarget->SerializeDisplay(pDicomMessage);
    }

    void SerializationAdapter::SerializeDisplay(
        ::DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::DataSet^ pDataSet = nullptr;
        if (pDCM_DATASET_CLASS != nullptr)
        {
            pDataSet =
                ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        }
        m_pSerializationTarget->SerializeDisplay(pDataSet);
    }

    void SerializationAdapter::SerializeDisplay(::FILE_DATASET_CLASS* pFILE_DATASET_CLASS)
    {
        DvtkData::Media::DicomFile^ pDicomFile = nullptr;
        if (pFILE_DATASET_CLASS != nullptr)
        {
            pDicomFile =
                ManagedUnManagedMediaConvertor::Convert(pFILE_DATASET_CLASS);
        }
        m_pSerializationTarget->SerializeDisplay(pDicomFile);
    }

	void SerializationAdapter::SerializeDisplay(::DCM_ITEM_CLASS *pDCM_ITEM_CLASS)
	{
		DvtkData::Dimse::SequenceItem^ pSequenceItem = nullptr;
		if (pDCM_ITEM_CLASS != nullptr)
		{
			pSequenceItem = ManagedUnManagedDimseConvertor::Convert(pDCM_ITEM_CLASS);
		}

        m_pSerializationTarget->SerializeDisplay(pSequenceItem);
	}

    void SerializationAdapter::SerializeImport(
        DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS, DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::CommandSet^ pCommandSet = nullptr;
        DvtkData::Dimse::DataSet^ pDataSet = nullptr;
        if (pDCM_COMMAND_CLASS != nullptr)
            pCommandSet = ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        if (pDCM_DATASET_CLASS != nullptr)
            pDataSet = ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        DvtkData::Dimse::DicomMessage^ pDicomMessage = gcnew DvtkData::Dimse::DicomMessage();
        pDicomMessage->CommandSet = pCommandSet;
        pDicomMessage->DataSet = pDataSet;
        m_pSerializationTarget->SerializeImport(pDicomMessage);
    }

    void SerializationAdapter::SerializeApplicationReport(ReportLevel reportLevel, const char* pUMMessage)
    {
        // count the general errors / warnings
        if (static_cast<Wrappers::ICountingTarget^>(this->m_pCountingTarget) != nullptr)
        {
            switch (reportLevel)
            {
            case ReportLevel_Error:
			    m_pCountingTarget->Increment(Wrappers::CountGroup::General, Wrappers::CountType::Error);
                break;
            case ReportLevel_Warning:
			    m_pCountingTarget->Increment(Wrappers::CountGroup::General, Wrappers::CountType::Warning);
                break;
            default:
                break;
            }
        }

        // Implicit conversion to managed string.
        System::String^ clistr = gcnew System::String(Wrappers::ConvertString(pUMMessage).c_str());
        m_pSerializationTarget->SerializeApplicationReport(Wrappers::_Convert(reportLevel), clistr);
        // m_pSerializationTarget->SerializeApplicationReport(Wrappers::_Convert(reportLevel), Wrappers::ConvertString(pUMMessage).c_str());
    }

    void SerializationAdapter::SerializeMediaRead(
        const char* pUMFileName, 
        FILE_DATASET_CLASS* pFILE_DATASET_CLASS /*May be nullptr*/)
    {
        DvtkData::Media::DicomFile^ pDicomFile = nullptr;
        if (pFILE_DATASET_CLASS != nullptr)
        {
            pDicomFile =
                ManagedUnManagedMediaConvertor::Convert(pFILE_DATASET_CLASS);
        }
        System::String^ clistr = gcnew System::String(pUMFileName);
        m_pSerializationTarget->SerializeMediaRead(clistr, pDicomFile);
        //m_pSerializationTarget->SerializeMediaRead(pUMFileName, pDicomFile);
    }

    void SerializationAdapter::SerializeMediaWrite(
        const char* pUMFileName, 
        FILE_DATASET_CLASS* pFILE_DATASET_CLASS /*May be nullptr*/)
    {
        DvtkData::Media::DicomFile^ pDicomFile = nullptr;
        if (pFILE_DATASET_CLASS != nullptr)
        {
            pDicomFile =
                ManagedUnManagedMediaConvertor::Convert(pFILE_DATASET_CLASS);
        }
        System::String^ clistr = gcnew System::String(pUMFileName);
        m_pSerializationTarget->SerializeMediaWrite(clistr, pDicomFile);
        //m_pSerializationTarget->SerializeMediaWrite(pUMFileName, pDicomFile);
    }
    
    void SerializationAdapter::SerializeBytes(BYTE* bytes, int length, const char* pUMDescription)
    {
        auto managedByteArray = gcnew cli::array<System::Byte>(length);
        //
        // convert any native ptr to System::IntPtr by doing C-Style cast
        //
        System::Runtime::InteropServices::Marshal::Copy(
            (System::IntPtr)bytes,  /*source*/
            managedByteArray,       /*destination*/
            0,                      /*startIndex*/
            length);                /*length*/
        System::String^ clistr = gcnew System::String(pUMDescription);
        m_pSerializationTarget->SerializeBytes(managedByteArray, clistr);
        //m_pSerializationTarget->SerializeBytes(managedByteArray, pUMDescription);
        return;
    }

    void SerializationAdapter::SerializeDSCreate(
        const char* commandSetRefId, 
        DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS)
    {
        DvtkData::Dimse::CommandSet^ pCommandSet = nullptr;
        pCommandSet = ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        System::String^ clistr = gcnew System::String(commandSetRefId);
        m_pSerializationTarget->SerializeDSCreate(clistr, pCommandSet);
        //m_pSerializationTarget->SerializeDSCreate(commandSetRefId, pCommandSet);
    }

    void SerializationAdapter::SerializeDSCreate(
        const char* commandSetRefId, 
        DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS, 
        const char* dataSetRefId,
        DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::CommandSet^ pCommandSet = nullptr;
        DvtkData::Dimse::DataSet^ pDataSet = nullptr;
        pCommandSet = ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        pDataSet = ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        System::String^ clistr = gcnew System::String(commandSetRefId);
        System::String^ clistr2 = gcnew System::String(dataSetRefId);
        m_pSerializationTarget->SerializeDSCreate(clistr, pCommandSet, clistr2, pDataSet);
        //m_pSerializationTarget->SerializeDSCreate(commandSetRefId, pCommandSet, dataSetRefId, pDataSet);
    }

    void SerializationAdapter::SerializeDSSetCommandSet(
        const char* commandSetRefId, 
        DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS)
    {
        DvtkData::Dimse::CommandSet^ pCommandSet = nullptr;
        pCommandSet = ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        System::String^ clistr = gcnew System::String(commandSetRefId);
        m_pSerializationTarget->SerializeDSSetCommandSet(clistr, pCommandSet);
        //m_pSerializationTarget->SerializeDSSetCommandSet(commandSetRefId, pCommandSet);
    }

    void SerializationAdapter::SerializeDSSetDataSet(
        const char* dataSetRefId, 
        DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::DataSet^ pDataSet = nullptr;
        pDataSet = ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        System::String^ clistr = gcnew System::String(dataSetRefId);
        m_pSerializationTarget->SerializeDSSetDataSet(clistr, pDataSet);
        // m_pSerializationTarget->SerializeDSSetDataSet(dataSetRefId, pDataSet);
    }

    void SerializationAdapter::SerializeDSDeleteCommandSet(
        const char* commandSetRefId, 
        DCM_COMMAND_CLASS* pDCM_COMMAND_CLASS)
    {
        DvtkData::Dimse::CommandSet^ pCommandSet = nullptr;
        pCommandSet = ManagedUnManagedDimseConvertor::Convert(pDCM_COMMAND_CLASS);
        System::String^ clistr = gcnew System::String(commandSetRefId);
        m_pSerializationTarget->SerializeDSDeleteCommandSet(clistr, pCommandSet);
        // m_pSerializationTarget->SerializeDSDeleteCommandSet(commandSetRefId, pCommandSet);
    }

    void SerializationAdapter::SerializeDSDeleteDataSet(
        const char* dataSetRefId, 
        DCM_DATASET_CLASS* pDCM_DATASET_CLASS)
    {
        DvtkData::Dimse::DataSet^ pDataSet = nullptr;
        pDataSet = ManagedUnManagedDimseConvertor::Convert(pDCM_DATASET_CLASS);
        System::String^ clistr = gcnew System::String(dataSetRefId);
        m_pSerializationTarget->SerializeDSDeleteDataSet(clistr, pDataSet);
        // m_pSerializationTarget->SerializeDSDeleteDataSet(dataSetRefId, pDataSet);
    }

    void SerializationAdapter::set_StrictValidationLogging(bool strict)
    {
        if (strict)
        {
            set_Rules(Wrappers::MBaseSession::RuleUriStrictRules);
        }
        else
        {
            set_Rules(Wrappers::MBaseSession::RuleUriStandardRules);
        }
    }

    void SerializationAdapter::set_Rules(System::Uri^ pRulesUri)
    {
        if (pRulesUri == nullptr) throw gcnew System::ArgumentNullException();
		m_pSetRulesUri = pRulesUri;
        this->m_dimseValidationResultsConverter.set_Rules(pRulesUri);
        this->m_dulValidationResultsConverter.set_Rules(pRulesUri);
        return;
    }

	System::Uri^ SerializationAdapter::get_Rules()
	{
		return m_pSetRulesUri;
	}

    void SerializationAdapter::Pause(void)
    {
        this->m_pSerializationTarget->Paused = true;
    }
    void SerializationAdapter::Resume(void)
    {
        this->m_pSerializationTarget->Paused = false;
    }

	BASE_SERIALIZER* SerializationAdapter::CreateAndRegisterChildSerializer(SerializerNodeType serializerNodeType)
	{
		Wrappers::WrappedSerializerNodeType wrappedSerializerNodeType
			= Wrappers::_Convert(serializerNodeType);
		//
		// Create serialization target
		//
		Wrappers::ISerializationTarget^ pSerializationTargetChild =
			this->m_pSerializationTarget->CreateChildSerializationTarget(wrappedSerializerNodeType);
		//
		// Attach a new child countManager. Split counting per serialization stream!
		// The parent/child relationship can be use to sum the final counters!
		//
		Wrappers::ICountingTarget^ pCountingTargetChild =
			this->m_pCountingTarget->CreateChildCountingTarget();
		//
		// Wrap the target into the returned adapter.
		//
		System::Uri^ pRulesUri = this->get_Rules();
		SerializationAdapter* pSerializationAdapterChild = 
			new SerializationAdapter(pSerializationTargetChild, pCountingTargetChild, pRulesUri);
		//
		// Register in parent/child relationship to ensure proper cleanup.
		//
		this->childSerializationAdaptersM_ptr.add(pSerializationAdapterChild);
		//return (BASE_SERIALIZER*)pSerializationAdapterChild;
		return pSerializationAdapterChild;
	}

	void SerializationAdapter::UnRegisterAndDestroyChildSerializer(BASE_SERIALIZER* child_ptr)
	{
		SerializationAdapter* childSerializer_ptr = static_cast<SerializationAdapter*>(child_ptr);
		bool success = false;
		UINT size = this->childSerializationAdaptersM_ptr.getSize();
		unsigned int nrOfGeneralErrorsChild = 0;
		unsigned int nrOfGeneralWarningsChild = 0;
		unsigned int nrOfUserErrorsChild = 0;
		unsigned int nrOfUserWarningsChild = 0;
		unsigned int nrOfValidationErrorsChild = 0;
		unsigned int nrOfValidationWarningsChild = 0;
		for (UINT index = 0; index < size; index++)
		{
			if (this->childSerializationAdaptersM_ptr[index] == childSerializer_ptr)
			{
				if (childSerializer_ptr->m_bEnded == false)
				{
					throw gcnew System::ApplicationException(
						"Could not destroy a child serializer that is still active.");
				}
				//
				// Transfer childs counts to parent.
				//
				nrOfGeneralErrorsChild =
					childSerializer_ptr->m_pSerializationTarget->EndCounters->NrOfGeneralErrors;
				nrOfGeneralWarningsChild =
					childSerializer_ptr->m_pSerializationTarget->EndCounters->NrOfGeneralWarnings;
				nrOfUserErrorsChild =
					childSerializer_ptr->m_pSerializationTarget->EndCounters->NrOfUserErrors;
				nrOfUserWarningsChild =
					childSerializer_ptr->m_pSerializationTarget->EndCounters->NrOfUserWarnings;
				nrOfValidationErrorsChild =
					childSerializer_ptr->m_pSerializationTarget->EndCounters->NrOfValidationErrors;
				nrOfValidationWarningsChild =
					childSerializer_ptr->m_pSerializationTarget->EndCounters->NrOfValidationWarnings;
				delete this->childSerializationAdaptersM_ptr[index];
				this->childSerializationAdaptersM_ptr.removeAt(index);
				success = true;
				break;
			}
		}
		this->m_pSerializationTarget->AddEndCounts(
			nrOfGeneralErrorsChild,
			nrOfGeneralWarningsChild,
			nrOfUserErrorsChild,
			nrOfUserWarningsChild,
			nrOfValidationErrorsChild,
			nrOfValidationWarningsChild);
		if (!success)
		{
			throw gcnew System::ApplicationException("Could not dispose a child serializer.");
		}
	}

	void SerializationAdapter::StartSerializer()
	{
		this->m_bEnded = false;
		this->m_pCountingTarget->Init();
	}

	void SerializationAdapter::EndSerializer()
	{
		this->m_bEnded = true;
		//
		// Precondition check that any child serializer has ended as well.
		//
        System::Collections::ArrayList^ waitHandleArray = gcnew System::Collections::ArrayList();
        //
        // Iterate through the child serializer to determine whether some are
        // still active.
        //
		for (UINT i = 0; i < childSerializationAdaptersM_ptr.getSize(); i++)
        {
			Wrappers::SerializationAdapter* pSerializationAdapterChild = 
				childSerializationAdaptersM_ptr[i];
            if (pSerializationAdapterChild->m_bEnded == false)
            {
                Wrappers::SerializationAdapter* pSerializationAdapterChild = 
                    childSerializationAdaptersM_ptr[i];
                waitHandleArray->Add(pSerializationAdapterChild->m_endedManualResetEvent);
            }
		}
        //
        // If there are active child serializers. Wait for them to end.
        //
        if (waitHandleArray->Count > 0) 
        {
            /*System::Threading::WaitHandle^ waitHandles[] = 
                static_cast<System::Threading::WaitHandle^[]>(
                waitHandleArray->ToArray((System::Threading::WaitHandle::typeid))
                );*/
            auto waitHandles = static_cast<cli::array<System::Threading::WaitHandle ^>^>(waitHandleArray->ToArray((System::Threading::WaitHandle::typeid)));
            //
            // Waits indefinitely for the child serializers to finish.
            //
			/*for(INT i = 0; i < waitHandles->Count; i++) 
			{
				System::Threading::WaitHandle __gc* waitHandle = waitHandles[i];
				System::Threading::WaitHandle __gc* wHandles[] = new System::Threading::WaitHandle __gc*[]{waitHandle};
				System::Threading::WaitHandle::WaitAny(wHandles);
			}*/
            System::Threading::WaitHandle::WaitAll(waitHandles);
        }
        //
        // Extra check to ensure that no child serializer are active after the wait.
        //
		for (UINT i = 0; i < childSerializationAdaptersM_ptr.getSize(); i++)
		{
			Wrappers::SerializationAdapter* pSerializationAdapterChild = 
				childSerializationAdaptersM_ptr[i];
            if (pSerializationAdapterChild->m_bEnded == false)
			{
				throw gcnew System::ApplicationException(
					System::String::Concat(
					"Trying to end a serializer with an active child serializer. ",
					"Child serializer should be ended first."));
			}
        }
		//
		// Transfer end counts from countmanager to serializer
		//
		this->m_pSerializationTarget->AddEndCounts(
			this->m_pCountingTarget->NrOfGeneralErrors,
			this->m_pCountingTarget->NrOfGeneralWarnings,
			this->m_pCountingTarget->NrOfUserErrors,
			this->m_pCountingTarget->NrOfUserWarnings,
			this->m_pCountingTarget->NrOfValidationErrors,
			this->m_pCountingTarget->NrOfValidationWarnings);
		this->m_pSerializationTarget->EndSerializationTarget();
        //
        // Signal that this serializer has ended.
        // Triggers any waiting parent serializers to continue.
        //
        this->m_endedManualResetEvent->Set();
	}
}