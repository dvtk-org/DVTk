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

#ifndef VALIDATION_ENGINE_H
#define VALIDATION_ENGINE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"


//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class ABORT_RQ_CLASS;
class ACSE_PROPERTIES_CLASS;
class AE_SESSION_CLASS;
class ASSOCIATE_AC_CLASS;
class ASSOCIATE_RJ_CLASS;
class ASSOCIATE_RQ_CLASS;
class BASE_SERIALIZER;
class LOG_MESSAGE_CLASS;
class OBJECT_RESULTS_CLASS;
class RELEASE_RQ_CLASS;
class RELEASE_RP_CLASS;
class UNKNOWN_PDU_CLASS;
class DCM_COMMAND_CLASS;
class DCM_DATASET_CLASS;
class DCM_DIR_DATASET_CLASS;
class DCM_ITEM_CLASS;
class DEF_COMMAND_CLASS;
class DEF_DATASET_CLASS;
class DEF_ATTRIBUTE_GROUP_CLASS;
class FILE_DATASET_CLASS;
class MEDIA_FILE_HEADER_CLASS;
class VALIDATOR_CLASS;
class VAL_ATTRIBUTE_GROUP_CLASS;
class VAL_ATTRIBUTE_CLASS;
class VAL_OBJECT_RESULTS_CLASS;
class LOG_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define VALIDATION	VALIDATION_ENGINE_CLASS::instance()

//>>***************************************************************************

class VALIDATION_ENGINE_CLASS

//  DESCRIPTION     : Provides 1 instance (Singleton) of the validation engine
//                    The validation engine controls the validation process.
//                    It provides different types of validation functions
//                    and instantiates the correct validation algorithm
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
    public:
        static VALIDATION_ENGINE_CLASS* instance();

        void setOptions(UINT);

		void setStrictValidation(bool);

        void setIncludeType3NotPresentInResults(bool);

        void setExternalReferenceObjects(bool) { }

        void useRequestObject(bool) { }

        bool validate(ABORT_RQ_CLASS*,
				ABORT_RQ_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*);

        bool validate(ASSOCIATE_RQ_CLASS*,
                ASSOCIATE_RQ_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*,
                ACSE_PROPERTIES_CLASS*);

        bool validate(ASSOCIATE_AC_CLASS*,
                ASSOCIATE_AC_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*,
                ACSE_PROPERTIES_CLASS*);

        bool validate(ASSOCIATE_RJ_CLASS*,
                ASSOCIATE_RJ_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*);

        bool validate(RELEASE_RQ_CLASS*,
                RELEASE_RQ_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*);

        bool validate(RELEASE_RP_CLASS*,
                RELEASE_RP_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*);

        bool validate(UNKNOWN_PDU_CLASS*,
                UNKNOWN_PDU_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*);

        bool validate(DCM_COMMAND_CLASS*,
                DCM_COMMAND_CLASS*,
                DCM_COMMAND_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*);

        bool validate(DCM_COMMAND_CLASS*,
			    DCM_DATASET_CLASS*,
                DCM_DATASET_CLASS*,
                DCM_COMMAND_CLASS*,
				DCM_DATASET_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*,
                AE_SESSION_CLASS*);

        bool validate(FILE_DATASET_CLASS*,
                DCM_DATASET_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*,
                AE_SESSION_CLASS*);

        bool validate(DCM_DATASET_CLASS*,
                DCM_DATASET_CLASS*,
                VALIDATION_CONTROL_FLAG_ENUM,
                BASE_SERIALIZER*,
                AE_SESSION_CLASS*);

        void setLogger(LOG_CLASS*);

    protected:
        VALIDATION_ENGINE_CLASS();

    private:
        static VALIDATION_ENGINE_CLASS *instanceM_ptr;
        UINT optionsM;
        vector<VAL_ATTRIBUTE_GROUP_CLASS*> objectResultsM;
		bool includeType3NotPresentInResultsM;
        LOG_CLASS *loggerM_ptr;

        VALIDATOR_CLASS *SelectValidator(DCM_COMMAND_CLASS*, DCM_DATASET_CLASS*);
};

#endif /* VALIDATION_ENGINE_H */
