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
#include "Ilog.h"
#include "ISessions.h"
#include <vcclr.h>
#include "ActivityReportingAdapter.h"
#include "CountingAdapter.h"
#include "MDIMSEResultsConvertors.h"
#include "MDULResultsConvertors.h"

enum SerializationState
{
	SerializationState_Init,
	SerializationState_Ended,
};

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;
    using namespace ManagedUnManagedDimseValidationResultsConvertors;
    using namespace ManagedUnManagedDulValidationResultsConvertors;

    public __value enum WrappedSerializerNodeType
    {
		TopParent,
        Thread,
        DirectoryRecord,
    };
    inline WrappedSerializerNodeType _Convert(SerializerNodeType serializerNodeType)
    {
        switch (serializerNodeType)
        {
        case ::SerializerNodeType_Thread			: return WrappedSerializerNodeType::Thread;
        case ::SerializerNodeType_DirectoryRecord	: return WrappedSerializerNodeType::DirectoryRecord;
        default : throw new System::NotImplementedException();
        }
    }

	// Interface to implement on the call-back serialization class.
    public __gc __interface ISerializationTarget
    {
        void SerializeSend(DvtkData::Dimse::DicomMessage __gc*);
        void SerializeSend(DvtkData::Dul::DulMessage __gc*);
        void SerializeReceive(DvtkData::Dimse::DicomMessage __gc*);
        void SerializeReceive(DvtkData::Dul::DulMessage __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationObjectResult __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationAbortRq __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationAssociateAc __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationAssociateRj __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationAssociateRq __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationReleaseRp __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationReleaseRq __gc*);
        void SerializeValidate(DvtkData::Validation::ValidationDirectoryRecordResult __gc*);
        void SerializeDisplay(DvtkData::Dimse::Attribute __gc*);
        void SerializeDisplay(DvtkData::Media::DicomFile __gc*);
        void SerializeDisplay(DvtkData::Dimse::DicomMessage __gc*);
        void SerializeDisplay(DvtkData::Dimse::DataSet __gc*);
        void SerializeDisplay(DvtkData::Dimse::SequenceItem __gc*);
        void SerializeImport(DvtkData::Dimse::DicomMessage __gc*);
        void SerializeApplicationReport(
            Wrappers::WrappedValidationMessageLevel activityReportLevel,
            System::String __gc* pMessage);
        void SerializeMediaRead(System::String __gc* fileName, DvtkData::Media::DicomFile __gc*);
        void SerializeMediaWrite(System::String __gc* fileName, DvtkData::Media::DicomFile __gc*);
        void SerializeBytes(unsigned char managedByteArray __gc[], System::String __gc* pDescription);

        void SerializeDSCreate(
            System::String __gc* commandSetRefId, 
            DvtkData::Dimse::CommandSet __gc*);
        void SerializeDSCreate(
            System::String __gc* commandSetRefId,
            DvtkData::Dimse::CommandSet __gc*, 
            System::String __gc* dataSetRefId,
            DvtkData::Dimse::DataSet __gc*);
        void SerializeDSSetCommandSet(
            System::String __gc* commandSetRefId,
            DvtkData::Dimse::CommandSet __gc*);
        void SerializeDSSetDataSet(
            System::String __gc* dataSetRefId,
            DvtkData::Dimse::DataSet __gc*);
        void SerializeDSDeleteCommandSet(
            System::String __gc* commandSetRefId,
            DvtkData::Dimse::CommandSet __gc*);
        void SerializeDSDeleteDataSet(
            System::String __gc* dataSetRefId,
            DvtkData::Dimse::DataSet __gc*);

		ISerializationTarget __gc* CreateChildSerializationTarget(Wrappers::WrappedSerializerNodeType reason);
		void EndSerializationTarget();

		void AddEndCounts(
			System::UInt32 endNrOfGeneralErrors,
			System::UInt32 endNrOfGeneralWarnings,
			System::UInt32 endNrOfUserErrors,
			System::UInt32 endNrOfUserWarnings,
			System::UInt32 endNrOfValidationErrors,
			System::UInt32 endNrOfValidationWarnings);

        __property System::Boolean get_Paused(void);
        __property void set_Paused(System::Boolean);

		__property DvtkData::Results::Counters __gc* get_EndCounters(void);
    };

    __nogc class SerializationAdapter
        : public BASE_SERIALIZER
    {
    public:
        // ctor
        //SerializationAdapter(ISerializationTarget __gc* value);
		SerializationAdapter(
			ISerializationTarget __gc* serializationTarget,
			ICountingTarget __gc* countingTarget,
			System::Uri __gc* rulesUri);
        // dtor
        ~SerializationAdapter(void);
        
         /* convert and call m_pSerializationTarget */
        void SerializeSend(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
        void SerializeSend(ABORT_RQ_CLASS*);
        void SerializeSend(ASSOCIATE_AC_CLASS*);
        void SerializeSend(ASSOCIATE_RJ_CLASS*);
        void SerializeSend(ASSOCIATE_RQ_CLASS*);
        void SerializeSend(RELEASE_RP_CLASS*);
        void SerializeSend(RELEASE_RQ_CLASS*);

        void SerializeReceive(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
        void SerializeReceive(ABORT_RQ_CLASS*);
        void SerializeReceive(ASSOCIATE_AC_CLASS*);
        void SerializeReceive(ASSOCIATE_RJ_CLASS*);
        void SerializeReceive(ASSOCIATE_RQ_CLASS*);
        void SerializeReceive(RELEASE_RP_CLASS*);
        void SerializeReceive(RELEASE_RQ_CLASS*);

        void SerializeValidate(VAL_OBJECT_RESULTS_CLASS*, UINT);
        void SerializeValidate(VAL_OBJECT_RESULTS_CLASS*, RECORD_LINK_VECTOR*, UINT);
        void SerializeValidate(ABORT_RQ_VALIDATOR_CLASS*);
        void SerializeValidate(ASSOCIATE_AC_VALIDATOR_CLASS*);
        void SerializeValidate(ASSOCIATE_RJ_VALIDATOR_CLASS*);
        void SerializeValidate(ASSOCIATE_RQ_VALIDATOR_CLASS*);
        void SerializeValidate(RELEASE_RP_VALIDATOR_CLASS*);
        void SerializeValidate(RELEASE_RQ_VALIDATOR_CLASS*);
        void SerializeValidate(RECORD_RESULTS_CLASS*, UINT);

        // DISPLAY
        void SerializeDisplay(DCM_ATTRIBUTE_CLASS*);
        void SerializeDisplay(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
        void SerializeDisplay(DCM_DATASET_CLASS*);
        void SerializeDisplay(FILE_DATASET_CLASS*);
        void SerializeDisplay(DCM_ITEM_CLASS*);

        void SerializeImport(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);

        // Generic serialized tag.
        // No specific tag are used for dss commands like;
        // LOG_SCRIPT:
        // COMPARE, COPY, WRITE, READ, SYSTEM, RESET,
        // CONFIRM, DELAY, ECHO, UPDATE, RETRIEVE
        void SerializeApplicationReport(ReportLevel reportLevel, const char* message);

        void SerializeMediaRead(const char* fileName, FILE_DATASET_CLASS*);
        void SerializeMediaWrite(const char* fileName, FILE_DATASET_CLASS*);
        void SerializeBytes(BYTE* bytes, int length, const char* pUMDescription);
        
        //
        // Specific serialization for old style CREATE, SET, DELETE DicomScript commands.
        //
        void SerializeDSCreate(
            const char* commandSetRefId, 
            DCM_COMMAND_CLASS*);
        void SerializeDSCreate(
            const char* commandSetRefId, 
            DCM_COMMAND_CLASS*, 
            const char* dataSetRefId,
            DCM_DATASET_CLASS*);
        void SerializeDSSetCommandSet(const char* commandSetRefId, DCM_COMMAND_CLASS*);
        void SerializeDSSetDataSet(const char* dataSetRefId, DCM_DATASET_CLASS*);
        void SerializeDSDeleteCommandSet(const char* commandSetRefId, DCM_COMMAND_CLASS*);
        void SerializeDSDeleteDataSet(const char* dataSetRefId, DCM_DATASET_CLASS*);

        void set_Rules(System::Uri __gc* pRulesUri);
        System::Uri __gc* get_Rules();

		void set_StrictValidationLogging(bool);

        void Pause(void);
        void Resume(void);

		BASE_SERIALIZER* CreateAndRegisterChildSerializer(SerializerNodeType serializerNodeType);
		void UnRegisterAndDestroyChildSerializer(BASE_SERIALIZER* child_ptr);
		//
		// End the current interaction with this serializer.
		// No calls to serialize may be done after this.
		//
		void EndSerializer();
		void StartSerializer();

	private:
		bool m_bEnded;
	private:
        gcroot<System::Threading::ManualResetEvent*> m_endedManualResetEvent;

	private:
		ARRAY<SerializationAdapter*> childSerializationAdaptersM_ptr;

        gcroot<ISerializationTarget*> m_pSerializationTarget;
		gcroot<ICountingTarget*> m_pCountingTarget;
	public:
		gcroot<System::Uri*> m_pSetRulesUri;
    private:
		// Will be created by default constructor
        ManagedUnManagedDimseValidationResultsConvertor m_dimseValidationResultsConverter;
    private:
		// Will be created by default constructor
        ManagedUnManagedDulValidationResultsConvertor m_dulValidationResultsConverter;
    };
}