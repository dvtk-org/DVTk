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

#ifndef BASE_SERIALIZER_H
#define BASE_SERIALIZER_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"				// global component interface
#include "base_activity_reporter.h"

//*****************************************************************************
//  FORWARD DECLARATIONS
//*****************************************************************************
class DCM_COMMAND_CLASS;
class DCM_DATASET_CLASS;
class ABORT_RQ_CLASS;
class ASSOCIATE_AC_CLASS;
class ASSOCIATE_RJ_CLASS;
class ASSOCIATE_RQ_CLASS;
class RELEASE_RP_CLASS;
class RELEASE_RQ_CLASS;
class VAL_OBJECT_RESULTS_CLASS;
class ABORT_RQ_VALIDATOR_CLASS;
class ASSOCIATE_AC_VALIDATOR_CLASS;
class ASSOCIATE_RJ_VALIDATOR_CLASS;
class ASSOCIATE_RQ_VALIDATOR_CLASS;
class RELEASE_RP_VALIDATOR_CLASS;
class RELEASE_RQ_VALIDATOR_CLASS;
class DCM_ATTRIBUTE_CLASS;
class DCM_ITEM_CLASS;
class FILE_DATASET_CLASS;
class RECORD_LINK_CLASS;
class RECORD_RESULTS_CLASS;

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
typedef vector<RECORD_LINK_CLASS *>   RECORD_LINK_VECTOR;

enum SerializerNodeType
{
    SerializerNodeType_Thread,
    SerializerNodeType_DirectoryRecord,
};

//>>***************************************************************************
//<<abstract>>
class BASE_SERIALIZER

//  DESCRIPTION     : BASE_SERIALIZER class.
//  INVARIANT       :
//  NOTES           : Derived classes should implement the defined methods.
//<<***************************************************************************
{
protected:

public:
    virtual void SerializeSend(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*) = 0;
    virtual void SerializeSend(ABORT_RQ_CLASS*) = 0;
    virtual void SerializeSend(ASSOCIATE_AC_CLASS*) = 0;
    virtual void SerializeSend(ASSOCIATE_RJ_CLASS*) = 0;
    virtual void SerializeSend(ASSOCIATE_RQ_CLASS*) = 0;
    virtual void SerializeSend(RELEASE_RP_CLASS*) = 0;
    virtual void SerializeSend(RELEASE_RQ_CLASS*) = 0;
    virtual void SerializeReceive(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*) = 0;
    virtual void SerializeReceive(ABORT_RQ_CLASS*) = 0;
    virtual void SerializeReceive(ASSOCIATE_AC_CLASS*) = 0;
    virtual void SerializeReceive(ASSOCIATE_RJ_CLASS*) = 0;
    virtual void SerializeReceive(ASSOCIATE_RQ_CLASS*) = 0;
    virtual void SerializeReceive(RELEASE_RP_CLASS*) = 0;
    virtual void SerializeReceive(RELEASE_RQ_CLASS*) = 0;
    virtual void SerializeValidate(VAL_OBJECT_RESULTS_CLASS*, UINT) = 0;
    virtual void SerializeValidate(VAL_OBJECT_RESULTS_CLASS*, RECORD_LINK_VECTOR*, UINT) = 0;
    virtual void SerializeValidate(ABORT_RQ_VALIDATOR_CLASS*) = 0;
    virtual void SerializeValidate(ASSOCIATE_AC_VALIDATOR_CLASS*) = 0;
    virtual void SerializeValidate(ASSOCIATE_RJ_VALIDATOR_CLASS*) = 0;
    virtual void SerializeValidate(ASSOCIATE_RQ_VALIDATOR_CLASS*) = 0;
    virtual void SerializeValidate(RELEASE_RP_VALIDATOR_CLASS*) = 0;
    virtual void SerializeValidate(RELEASE_RQ_VALIDATOR_CLASS*) = 0;
    virtual void SerializeValidate(RECORD_RESULTS_CLASS*, UINT) = 0;
    virtual void SerializeDisplay(DCM_ATTRIBUTE_CLASS*) = 0;
    virtual void SerializeDisplay(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*) = 0;
    virtual void SerializeDisplay(DCM_DATASET_CLASS*) = 0;
    virtual void SerializeDisplay(FILE_DATASET_CLASS*) = 0;
    virtual void SerializeDisplay(DCM_ITEM_CLASS*) = 0;
	virtual void SerializeImport(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*) = 0;
    virtual void SerializeApplicationReport(ReportLevel reportLevel, const char* message) = 0;
    virtual void SerializeMediaRead(const char* fileName, FILE_DATASET_CLASS*) = 0;
    virtual void SerializeMediaWrite(const char* fileName, FILE_DATASET_CLASS*) = 0;
    virtual void SerializeBytes(BYTE* bytes, int length, const char* pDescription) = 0;
    //
    // Specific serialization for old style CREATE, SET, DELETE DicomScript commands.
    //
    virtual void SerializeDSCreate(const char* commandSetRefId, DCM_COMMAND_CLASS*) = 0;
    virtual void SerializeDSCreate(
        const char* commandSetRefId, 
        DCM_COMMAND_CLASS*, 
        const char* dataSetRefId, 
        DCM_DATASET_CLASS*) = 0;
    virtual void SerializeDSSetCommandSet(const char* commandSetRefId, DCM_COMMAND_CLASS*) = 0;
    virtual void SerializeDSSetDataSet(const char* dataSetRefId, DCM_DATASET_CLASS*) = 0;
    virtual void SerializeDSDeleteCommandSet(const char* commandSetRefId, DCM_COMMAND_CLASS*) = 0;
    virtual void SerializeDSDeleteDataSet(const char* dataSetRefId, DCM_DATASET_CLASS*) = 0;

    virtual void Pause(void) = 0;
    virtual void Resume(void) = 0;
    virtual void set_StrictValidationLogging(bool) = 0;

	virtual BASE_SERIALIZER* CreateAndRegisterChildSerializer(SerializerNodeType reasonForChildSerializer) = 0;
	virtual void UnRegisterAndDestroyChildSerializer(BASE_SERIALIZER* child_ptr) = 0;
	virtual void EndSerializer() = 0;
	virtual void StartSerializer() = 0;
public:
    virtual ~BASE_SERIALIZER() = 0;		
};

#endif /* BASE_SERIALIZER_H */

