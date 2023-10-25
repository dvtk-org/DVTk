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

#ifndef VALIDATOR_H
#define VALIDATOR_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class DEF_DICOM_OBJECT_CLASS;
class DEF_MODULE_CLASS;
class DEF_ATTRIBUTE_GROUP_CLASS;
class DCM_ATTRIBUTE_GROUP_CLASS;
class VAL_ATTRIBUTE_GROUP_CLASS;
class VAL_OBJECT_RESULTS_CLASS;
class VAL_ATTRIBUTE_CLASS;
class DEF_ATTRIBUTE_CLASS;
class DCM_ATTRIBUTE_CLASS;
class DCM_COMMAND_CLASS;
class DCM_DATASET_CLASS;
class DEF_DATASET_CLASS;
class DEF_MACRO_CLASS;
class DCM_VALUE_SQ_CLASS;
class VAL_VALUE_SQ_CLASS;
class VALUE_SQ_CLASS;
class BASE_SERIALIZER;
class AE_SESSION_CLASS;
class LOG_CLASS;
class SPECIFIC_CHARACTER_SET_CLASS;


//>>***************************************************************************
class VALIDATOR_CLASS
//  DESCRIPTION     : Validator class
//  NOTES           :
//<<***************************************************************************
{
    public:
		VALIDATOR_CLASS();
        virtual ~VALIDATOR_CLASS();

        virtual bool CreateResultsObject();

        void CreateModuleResultsFromDef(DEF_DICOM_OBJECT_CLASS*,
								DCM_ATTRIBUTE_GROUP_CLASS*);

        bool CreateCommandResultsFromDef(DCM_COMMAND_CLASS*);

        bool CreateDatasetResultsFromDef(DCM_COMMAND_CLASS*,
								DCM_DATASET_CLASS*,
								AE_SESSION_CLASS*);

		bool UpdateDatasetResultsFromLastSent(DCM_COMMAND_CLASS*,
								DCM_COMMAND_CLASS*,
								DCM_DATASET_CLASS*);

        void SetModuleResultsFromDcm(DCM_ATTRIBUTE_GROUP_CLASS*,
								bool,
								bool);

        virtual void ValidateResults(VALIDATION_CONTROL_FLAG_ENUM);

        RESULTS_TYPE GetResultsType();

        virtual void Serialize(BASE_SERIALIZER*);

        void SetFlags(UINT);

        void SetLogger(LOG_CLASS*);

    protected:
        void CreateAttributeGroupResultsFromDef(DEF_ATTRIBUTE_GROUP_CLASS*,
										DCM_ATTRIBUTE_GROUP_CLASS*,
                                        VAL_ATTRIBUTE_GROUP_CLASS*,
                                        bool,
										bool); 

        void CreateSQResultsFromDef(VALUE_SQ_CLASS*,
							DCM_VALUE_SQ_CLASS*,
                            VAL_VALUE_SQ_CLASS*,
                            bool,
							bool);

        void CreateMacroResultsFromDef(DEF_MACRO_CLASS*,
							DCM_ATTRIBUTE_GROUP_CLASS*,
                            VAL_ATTRIBUTE_GROUP_CLASS*,
                            bool,
							bool);

        void CreateAttributeResultsFromDef(DEF_ATTRIBUTE_CLASS*,
							DCM_ATTRIBUTE_GROUP_CLASS*,
                            VAL_ATTRIBUTE_CLASS*,
                            bool,
							bool);

        void CreateValueResultsFromDef(DEF_ATTRIBUTE_CLASS*,
							VAL_ATTRIBUTE_CLASS*);

        void SetAttributeGroupResultsFromDcm(DCM_ATTRIBUTE_GROUP_CLASS*,
							VAL_ATTRIBUTE_GROUP_CLASS*,
                            bool);

        void SetValueResultsFromDcm(DCM_ATTRIBUTE_CLASS*,
							VAL_ATTRIBUTE_CLASS*,
                            bool);

        void SetSQResultsFromDcm(DCM_ATTRIBUTE_CLASS*,
							VAL_ATTRIBUTE_CLASS*,
                            bool);

        void LogSystemDefinitions();

		void UpdateSpecificCharacterSet(DCM_ATTRIBUTE_GROUP_CLASS*);

    private:

		void CheckIfAnyModulesShouldBeIgnored();        

		bool UpdateDatasetResultsFromLastSentDataset(DCM_DATASET_CLASS*);

		bool UpdateSequenceItemsResultsFromLastSentDataset(VAL_VALUE_SQ_CLASS*, DCM_VALUE_SQ_CLASS*);

    protected:
        RESULTS_TYPE					resultsTypeM;
        VAL_OBJECT_RESULTS_CLASS		*objectResultsM_ptr;
        UINT							flagsM;
        SPECIFIC_CHARACTER_SET_CLASS	*specificCharacterSetM_ptr;
		DEF_DATASET_CLASS				*defDatasetM_ptr;

    private:
        LOG_CLASS						*loggerM_ptr;
};

#endif /* VALIDATOR_H */
