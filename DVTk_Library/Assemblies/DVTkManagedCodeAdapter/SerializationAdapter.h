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

    public enum class WrappedSerializerNodeType
    {
		TopParent,
        Thread,
        DirectoryRecord,
    };
    inline WrappedSerializerNodeType _Convert(SerializerNodeType serializerNodeType)
    {
        switch (serializerNodeType)
        {
        case ::SerializerNodeType_Thread: return WrappedSerializerNodeType::Thread;
        case ::SerializerNodeType_DirectoryRecord: return WrappedSerializerNodeType::DirectoryRecord;
        default: throw gcnew System::NotImplementedException();
        }
    };

	// Interface to implement on the call-back serialization class.
    public interface class ISerializationTarget
    {
        void SerializeSend(DvtkData::Dimse::DicomMessage^);
        void SerializeSend(DvtkData::Dul::DulMessage^);
        void SerializeReceive(DvtkData::Dimse::DicomMessage^);
        void SerializeReceive(DvtkData::Dul::DulMessage^);
        void SerializeValidate(DvtkData::Validation::ValidationObjectResult^);
        void SerializeValidate(DvtkData::Validation::ValidationAbortRq^);
        void SerializeValidate(DvtkData::Validation::ValidationAssociateAc^);
        void SerializeValidate(DvtkData::Validation::ValidationAssociateRj^);
        void SerializeValidate(DvtkData::Validation::ValidationAssociateRq^);
        void SerializeValidate(DvtkData::Validation::ValidationReleaseRp^);
        void SerializeValidate(DvtkData::Validation::ValidationReleaseRq^);
        void SerializeValidate(DvtkData::Validation::ValidationDirectoryRecordResult^);
        void SerializeDisplay(DvtkData::Dimse::Attribute^);
        void SerializeDisplay(DvtkData::Media::DicomFile^);
        void SerializeDisplay(DvtkData::Dimse::DicomMessage^);
        void SerializeDisplay(DvtkData::Dimse::DataSet^);
        void SerializeDisplay(DvtkData::Dimse::SequenceItem^);
        void SerializeImport(DvtkData::Dimse::DicomMessage^);
        void SerializeApplicationReport(
            Wrappers::WrappedValidationMessageLevel activityReportLevel,
            System::String^ pMessage);
        void SerializeMediaRead(System::String^ fileName, DvtkData::Media::DicomFile^);
        void SerializeMediaWrite(System::String^ fileName, DvtkData::Media::DicomFile^);
        //void SerializeBytes(unsigned char managedByteArray __gc[], System::String __gc* pDescription); 
        void SerializeBytes(cli::array<unsigned char>^ managedByteArray, System::String^ pDescription);

        void SerializeDSCreate(
            System::String^ commandSetRefId,
            DvtkData::Dimse::CommandSet^);
        void SerializeDSCreate(
            System::String^ commandSetRefId,
            DvtkData::Dimse::CommandSet^,
            System::String^ dataSetRefId,
            DvtkData::Dimse::DataSet^);
        void SerializeDSSetCommandSet(
            System::String^ commandSetRefId,
            DvtkData::Dimse::CommandSet^);
        void SerializeDSSetDataSet(
            System::String^ dataSetRefId,
            DvtkData::Dimse::DataSet^);
        void SerializeDSDeleteCommandSet(
            System::String^ commandSetRefId,
            DvtkData::Dimse::CommandSet^);
        void SerializeDSDeleteDataSet(
            System::String^ dataSetRefId,
            DvtkData::Dimse::DataSet^);

		ISerializationTarget^ CreateChildSerializationTarget(Wrappers::WrappedSerializerNodeType reason);
		void EndSerializationTarget();

		void AddEndCounts(
			System::UInt32 endNrOfGeneralErrors,
			System::UInt32 endNrOfGeneralWarnings,
			System::UInt32 endNrOfUserErrors,
			System::UInt32 endNrOfUserWarnings,
			System::UInt32 endNrOfValidationErrors,
			System::UInt32 endNrOfValidationWarnings);

        /*__property System::Boolean get_Paused(void);
        __property void set_Paused(System::Boolean);*/

        property System::Boolean Paused
        {
            System::Boolean get();
            void set(System::Boolean);
        }

		//__property DvtkData::Results::Counters^ get_EndCounters(void);

        property DvtkData::Results::Counters^ EndCounters
        {
            DvtkData::Results::Counters^ get();

        }
    };

    class SerializationAdapter
        : public BASE_SERIALIZER
    {
    public:
        // ctor
        //SerializationAdapter(ISerializationTarget __gc* value);
		SerializationAdapter(
			ISerializationTarget^ serializationTarget,
			ICountingTarget^ countingTarget,
			System::Uri^ rulesUri);
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

        void set_Rules(System::Uri^ pRulesUri);
        System::Uri^ get_Rules();

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
        gcroot<System::Threading::ManualResetEvent^> m_endedManualResetEvent;

	private:
		ARRAY<SerializationAdapter*> childSerializationAdaptersM_ptr;

        gcroot<ISerializationTarget^> m_pSerializationTarget;
		gcroot<ICountingTarget^> m_pCountingTarget;
	public:
		gcroot<System::Uri^> m_pSetRulesUri;
    private:
		// Will be created by default constructor
        ManagedUnManagedDimseValidationResultsConvertor m_dimseValidationResultsConverter;

    private:
		// Will be created by default constructor
        ManagedUnManagedDulValidationResultsConvertor m_dulValidationResultsConverter;
    };
}