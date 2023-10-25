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

//*****************************************************************************
//  DESCRIPTION     :   Validation engine class.
//*****************************************************************************


//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "validator.h"
#include "val_attribute.h"
#include "val_attribute_group.h"
#include "val_object_results.h"
#include "val_value_sq.h"
#include "record_types.h"
#include "qr_validator.h"
#include "IAttributeGroup.h"        // AttributeGroup component interface file.
#include "Idefinition.h"            // Definition component interface file.
#include "Idicom.h"                 // Dicom component interface file.


//>>===========================================================================

VALIDATOR_CLASS::VALIDATOR_CLASS()

//  DESCRIPTION     : Class constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    objectResultsM_ptr = NULL;
    resultsTypeM = RESULTS_OBJECT;
    specificCharacterSetM_ptr = NULL;
    flagsM = ATTR_FLAG_DO_NOT_INCLUDE_TYPE3;
	defDatasetM_ptr = NULL;
    loggerM_ptr = NULL;
}

//>>===========================================================================

VALIDATOR_CLASS::~VALIDATOR_CLASS()

//  DESCRIPTION     : Class destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (specificCharacterSetM_ptr)
    {
        delete specificCharacterSetM_ptr;
    }

    delete objectResultsM_ptr;
}

//>>===========================================================================

RESULTS_TYPE VALIDATOR_CLASS::GetResultsType()

//  DESCRIPTION     : Get the results type.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    return resultsTypeM;
}

//>>===========================================================================

void VALIDATOR_CLASS::SetLogger(LOG_CLASS *logger_ptr)

//  DESCRIPTION     : Set the logger.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    loggerM_ptr = logger_ptr;
}

//>>===========================================================================

void VALIDATOR_CLASS::SetFlags(UINT flags)

//  DESCRIPTION     : Set the flags.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    flagsM = flags;
}

//>>===========================================================================

bool VALIDATOR_CLASS::CreateResultsObject()

//  DESCRIPTION     : Create the results object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_OBJECT;
	return (objectResultsM_ptr == NULL) ? false : true;
}

//>>===========================================================================			  

void VALIDATOR_CLASS::UpdateSpecificCharacterSet(DCM_ATTRIBUTE_GROUP_CLASS *attributeGroup_ptr)

//  DESCRIPTION     : Updates the Specific Character Set.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{

    // check if the attribute group is present
    if (attributeGroup_ptr == NULL) return;

    // initialize the specific character set
    if (specificCharacterSetM_ptr)
    {
        specificCharacterSetM_ptr->ClearCharacterSets();
    }
    else
    {
        specificCharacterSetM_ptr = new SPECIFIC_CHARACTER_SET_CLASS();
    }

    // try to get the specific character set attribute
    DCM_ATTRIBUTE_CLASS *attribute_ptr = attributeGroup_ptr->GetAttributeByTag(TAG_SPECIFIC_CHARACTER_SET);
    if (attribute_ptr == NULL) return;

    // loop adding each specific character set value
    for (int i = 0; i < attribute_ptr->GetNrValues(); i++)
    {
		// Extended Character Set: needed for checking VR's of PN, ST, LT, UT, SH, LO
        BASE_VALUE_CLASS *value_ptr = attribute_ptr->GetValue(i);
        if (value_ptr)
        {
            string value;
            value_ptr->Get(value);
            specificCharacterSetM_ptr->AddCharacterSet(i+1, value);
        }
    }
}

//>>===========================================================================

bool VALIDATOR_CLASS::CreateCommandResultsFromDef(DCM_COMMAND_CLASS *dcmCommand_ptr)

//  DESCRIPTION     : Create the Command Validation Results from the Command Definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
    resultsTypeM = RESULTS_OBJECT;

    if (loggerM_ptr)
    {
        loggerM_ptr->text(LOG_DEBUG, 1, "CreateCommandResultsFromDef");
    }

    // Check for valid input.
    if (dcmCommand_ptr == NULL)
    {
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_COMMAND_1,
            "No Command provided - validation skipped");
        return false;
    }

    // First find out what SOP class we're dealing with.
    string sopClassUid;
    if (dcmCommand_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUid) == false)
	{
		dcmCommand_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUid);
	}

    // Get command id for the dimse.
    DIMSE_CMD_ENUM dimseCommand = dcmCommand_ptr->getCommandId();

    // Give the results a name.
    objectResultsM_ptr->SetName(mapCommandName(dimseCommand));

    // Get the command definition.
	DEF_COMMAND_CLASS *defCommand_ptr = DEFINITION->GetCommand(dimseCommand);

    if (defCommand_ptr == NULL)
    {
        char message[256];
        sprintf(message,
            "Could not find Command definition for SOP: %s, Dimse: %s",
            sopClassUid.c_str(), objectResultsM_ptr->GetName().c_str());

        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_COMMAND_2, message);
        return false;
    }

    CreateModuleResultsFromDef(defCommand_ptr, 
						dcmCommand_ptr);

    return true;
}

//>>===========================================================================

bool VALIDATOR_CLASS::CreateDatasetResultsFromDef(DCM_COMMAND_CLASS *dcmCommand_ptr,
                                         DCM_DATASET_CLASS *dcmDataset_ptr,
                                         AE_SESSION_CLASS *aeSession_ptr)

//  DESCRIPTION     : Create the Dataset Validation Results from the Dataset Definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    char message[256];

	defDatasetM_ptr = NULL;

    if (loggerM_ptr)
    {
        loggerM_ptr->text(LOG_DEBUG, 1, "CreateDatasetResultsFromDef");
    }

    // Check for valid input.
    if ((dcmCommand_ptr == NULL) || 
		(dcmDataset_ptr == NULL))
    {
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_COMMAND_1,
            "No Command and/or Dataset provided - validation skipped");
        LogSystemDefinitions();
        return false;
    }

    DIMSE_CMD_ENUM dimseCommand = dcmCommand_ptr->getCommandId();
	string iodName = "";
    if (dcmDataset_ptr->getIodName())
	{
        iodName = dcmDataset_ptr->getIodName();
	}

    string aeName = aeSession_ptr->GetName();
    string aeVersion = aeSession_ptr->GetVersion();

    if (iodName != "")
    {
        // Try to find the dataset definition through the name of the SOP class.
        DVT_STATUS result = DEFINITION->GetDataset(dimseCommand, iodName, aeSession_ptr, &defDatasetM_ptr);
		switch(result)
		{
		case MSG_DEFINITION_NOT_FOUND:
			// defDatasetM_ptr is returned NULL here - nothing further to do
			break;
		case MSG_DEFAULT_DEFINITION_FOUND:
			// not using the requested definition AE - should inform the user
			// defDatasetM_ptr contains the default definition
            {
                aeName = DEFAULT_AE_NAME;
                aeVersion = DEFAULT_AE_VERSION;

                string stringMessage;
                sprintf(message,
                    "Could not find System AE Name \"%s\" - AE Version \"%s\" Dataset definition for SOP: \"%s\", Dimse: %s.",
                    aeSession_ptr->GetName().c_str(),
                    aeSession_ptr->GetVersion().c_str(),
                    FILE_META_SOP_CLASS_NAME,
                    mapCommandName(DIMSE_CMD_CSTORE_RQ));
                stringMessage.append(message);
                sprintf(message,
                    " Using Default AE Name \"%s\" - AE Version \"%s\"",
                    DEFAULT_AE_NAME, 
                    DEFAULT_AE_VERSION);
                stringMessage.append(message);
                objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_DEFINITION_6, stringMessage);
            }
			break;
		case MSG_OK:
			// found the requested definition
			// defDatasetM_ptr contains the requested definition
			break;
		default:
			break;
		}
    }

    if (defDatasetM_ptr == NULL)
    {
        // Definition dataset not found. Try to find the dataset through
        // the SOP UID. First find out what SOP class we're dealing with.
		string sopClassUid;
        bool status = dcmCommand_ptr->getUIValue(TAG_AFFECTED_SOP_CLASS_UID, sopClassUid);
        if (!status)
		{
            status = dcmCommand_ptr->getUIValue(TAG_REQUESTED_SOP_CLASS_UID, sopClassUid);
		}

        if (!status)
        {
            sprintf(message, 
				"Could not find definition for Dimse: %s %s",
                mapCommandName(dimseCommand),
				iodName.c_str());
            objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_DEFINITION_5, message);
            LogSystemDefinitions();
            return false;
        }

		DEF_SOPCLASS_CLASS *defSopClass_ptr = NULL;
        DVT_STATUS result = DEFINITION->GetSopClassByUid(sopClassUid, aeSession_ptr, &defSopClass_ptr);
		switch(result)
		{
		case MSG_DEFINITION_NOT_FOUND:
			// sop_class is returned NULL here - nothing further to do
			break;
		case MSG_DEFAULT_DEFINITION_FOUND:
			// not using the requested definition AE - should inform the user
			// sop_class contains the default definition
            {
                aeName = DEFAULT_AE_NAME;
                aeVersion = DEFAULT_AE_VERSION;

                string stringMessage;
                sprintf(message,
                    "Could not find System AE Name \"%s\" - AE Version \"%s\" Dataset definition for SOP: \"%s\", Dimse: %s %s.",
                    aeSession_ptr->GetName().c_str(),
                    aeSession_ptr->GetVersion().c_str(),
                    sopClassUid.c_str(),
                    mapCommandName(dimseCommand),
                    iodName.c_str());
                stringMessage.append(message);
                sprintf(message,
                    " Using Default AE Name \"%s\" - AE Version \"%s\"",
                    DEFAULT_AE_NAME,
                    DEFAULT_AE_VERSION);
                stringMessage.append(message);
                objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_DEFINITION_6, stringMessage);
            }
			break;
		case MSG_OK:
			// found the requested definition
			// sop_class contains the requested definition
			break;
		default:
			break;
		}        
		
		if (defSopClass_ptr != NULL)
        {
            // Get the dataset for the SOP - DIMSE combination.
            if (iodName.length())
			{
                defDatasetM_ptr = defSopClass_ptr->GetDataset(iodName, dimseCommand);
			}
            else
            {
                // We don't have an IOD name, try for command only.
                defDatasetM_ptr = defSopClass_ptr->GetDataset(dimseCommand);
            }
        }

        if (defDatasetM_ptr == NULL)
        {
            objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_COMMAND_1,
                    "No Command and/or Dataset definition found - validation skipped");
            LogSystemDefinitions();
            return false;
        }
    }

    string stringMessage;
    sprintf(message,
        "Validate: Selected Dataset definition: \"%s\".",
        defDatasetM_ptr->GetName().c_str());
    stringMessage.append(message);
    sprintf(message,
        " Using AE Name \"%s\" - AE Version \"%s\"",
        aeName.c_str(),
        aeVersion.c_str());
    stringMessage.append(message);
    objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_DEFINITION_4, stringMessage);

    // give the results a name.
    objectResultsM_ptr->SetName(defDatasetM_ptr->GetName());

	// set the dataset co-object as the command
    objectResultsM_ptr->SetCommand(dcmCommand_ptr);

    CreateModuleResultsFromDef(defDatasetM_ptr, 
						dcmDataset_ptr);

    return true;
}

//>>===========================================================================

void VALIDATOR_CLASS::CreateModuleResultsFromDef(DEF_DICOM_OBJECT_CLASS *defDicomObject_ptr,
                                        DCM_ATTRIBUTE_GROUP_CLASS *dcmAttributeGroup_ptr)

//  DESCRIPTION     : Create the Module Validation Results from the Module Definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if ((defDicomObject_ptr == NULL) ||
        (dcmAttributeGroup_ptr == NULL))
    {
        return;
    }

    // Update the Specific Character Set - required for extended character set validation
    UpdateSpecificCharacterSet(dcmAttributeGroup_ptr);

    // Check if the attributes in the attribute group are sorted.
    if (!dcmAttributeGroup_ptr->IsSorted())
    {
        objectResultsM_ptr->GetMessages()->AddMessage(VAL_RULE_GENERAL_1, "Attributes are not sorted in ascending tag order");
    }

    for (UINT i = 0; i < defDicomObject_ptr->GetNrModules(); i++)
    {
		DEF_MODULE_CLASS *defModule_ptr = defDicomObject_ptr->GetModule(i);

        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_DEBUG, 1, "CreateModuleResultsFromDef for \"%s\"", defModule_ptr->GetName().c_str());
        }

        // find out whether the module has to be included
		bool addModule = false;
		bool attributeTypeNotImportantSetToType3 = false;
        bool useTextualCondition = false;
		UINT32 singleMatchingAttribute = 0;
        MOD_USAGE_ENUM usage = defModule_ptr->GetUsage();
        switch (usage)
        {
        case MOD_USAGE_M:
            addModule = true;
            break;

        case MOD_USAGE_C:
			{
			CONDITION_CLASS *condition_ptr = defModule_ptr->GetCondition();
			if (condition_ptr)
			{
                CONDITION_RESULT_ENUM conditionResult = condition_ptr->Evaluate(dcmAttributeGroup_ptr, NULL, loggerM_ptr);
				switch(conditionResult)
				{
				case CONDITION_TRUE:
					// the module must be included - leave attribute types as defined
                    addModule = true;
                    defModule_ptr->SetValueConditionText(" (" + condition_ptr->GetPrimaryNode()->GetResultMessage() + ") ");

					if (loggerM_ptr)
                    {
                        loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Module - Condition for \"%s\" is TRUE", defModule_ptr->GetName().c_str());
                    }
					break;
				case CONDITION_UNIMPORTANT:
					// the module should be added but all attributes in the module can be set to Type 3 
					// - their presence is not important to the final validation result.
					addModule = true;
					attributeTypeNotImportantSetToType3 = true;

	                if (loggerM_ptr)
		            {
						loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Module - Condition for \"%s\" is UNIMPORTANT", defModule_ptr->GetName().c_str());
					}
					break;
				case CONDITION_FALSE:
					if (loggerM_ptr)
					{
                       loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Module - Condition for \"%s\" is not FALSE", defModule_ptr->GetName().c_str());
					}
					break;
				case CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION:
				default:
					if (loggerM_ptr)
					{
                       loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Module - Condition for \"%s\" is not supported", defModule_ptr->GetName().c_str());
					}
					break;
				}
			}
            else
            {
                // check if a textual condition is defined
                // useTextualCondition = (defModule_ptr->GetTextualCondition().length() > 0 ? true : false);
                // - This check does not need to be made here as the addModule boolean will be set according
                // to the presence of attributes in this conditional module being checked directly in the given
                // attribute group - dcmAttributeGroup_ptr
                // So leave useTextualCondition = false here.

                // Add the module when an attribute is present in the dcm stream.
                // This is the same rule as the with the User optional module.
                addModule = defModule_ptr->EvaluateAddUserOptionalModule(dcmAttributeGroup_ptr, &singleMatchingAttribute);

                if (loggerM_ptr)
                {
                    if (addModule)
                    {
                        loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Module - Attributes found for \"%s\" is TRUE", defModule_ptr->GetName().c_str());
                    }
                    else
                    {
                        loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Module - Attributes found for \"%s\" is FALSE", defModule_ptr->GetName().c_str());
                    }
                }
            }
			}
            break;

        case MOD_USAGE_U:
			{
            addModule = defModule_ptr->EvaluateAddUserOptionalModule(dcmAttributeGroup_ptr, &singleMatchingAttribute);

            if (loggerM_ptr)
            {
                if (addModule)
                {
                    loggerM_ptr->text(LOG_DEBUG, 1, "User Optional Module - Attributes found for \"%s\" is TRUE", defModule_ptr->GetName().c_str());
                }
                else
                {
                    loggerM_ptr->text(LOG_DEBUG, 1, "User Optional Module - Attributes found for \"%s\" is FALSE", defModule_ptr->GetName().c_str());
                }
            }
			}
            break;

        default:
            break;
        }

        if (addModule)
        {
			// Get a new Module Results object
            VAL_ATTRIBUTE_GROUP_CLASS *moduleResults_ptr = new VAL_ATTRIBUTE_GROUP_CLASS();

			// Link it to the parent Object Results
            moduleResults_ptr->SetParentObject(objectResultsM_ptr);

			// Update the singleMatchingAttribute
			// - non zero value indicates that only one attribute was found in the dcmAttributeGroup_ptr that is in the defModule_ptr
			// - we will check after completing the results object whether or not this moduleResults_ptr should be ignored or not when
			// performing the final validation.
			moduleResults_ptr->SetSingleMatchingAttribute(singleMatchingAttribute);

			// Create the Attribute Group Results from the Module Definition
            CreateAttributeGroupResultsFromDef(defModule_ptr, 
									dcmAttributeGroup_ptr, 
									moduleResults_ptr, 
									useTextualCondition,
									attributeTypeNotImportantSetToType3);

			// Add the Module Results to the Object Results
            objectResultsM_ptr->AddModuleResults(moduleResults_ptr);
        }
    }

	// now check if any of the Conditional or User Optional modules should be ignored in the final validation results
	CheckIfAnyModulesShouldBeIgnored();
}

//>>===========================================================================

void VALIDATOR_CLASS::CreateAttributeGroupResultsFromDef(DEF_ATTRIBUTE_GROUP_CLASS *defAttrGroup_ptr,
                                                DCM_ATTRIBUTE_GROUP_CLASS *dcmAttrGroup_ptr,
                                                VAL_ATTRIBUTE_GROUP_CLASS *valAttrGroup_ptr,
                                                bool useTextualCondition,
												bool attributeTypeNotImportantSetToType3)

//  DESCRIPTION     : Create the Attribute Group Results from the Attribute Group Definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // link module definition
    valAttrGroup_ptr->SetDefAttributeGroup(defAttrGroup_ptr);
    if (defAttrGroup_ptr == NULL) return;

	defAttrGroup_ptr->SortAttributes();

	// handle all the attribute in the group
    for (int i = 0; i < defAttrGroup_ptr->GetNrAttributes(); i++)
    {
        VAL_ATTRIBUTE_CLASS *valAttr_ptr;

        if (resultsTypeM == RESULTS_QR)
		{
            valAttr_ptr = new VAL_QR_ATTRIBUTE_CLASS();
		}
        else
		{
            valAttr_ptr = new VAL_ATTRIBUTE_CLASS();
		}

		// link to parent
        valAttr_ptr->SetParent(valAttrGroup_ptr);

		// set the textual condition usage
        valAttr_ptr->SetUseConditionalTextDuringValidation(useTextualCondition);

		// create the attribute results from the attribute definition
        CreateAttributeResultsFromDef(defAttrGroup_ptr->GetAttribute(i),
								dcmAttrGroup_ptr, 
								valAttr_ptr, 
								useTextualCondition,
								attributeTypeNotImportantSetToType3);

		// link to attribute group
        valAttrGroup_ptr->AddValAttribute(valAttr_ptr);
    }

	// handle any macros in the group
    for (int i = 0; i < defAttrGroup_ptr->GetNrMacros(); i++)
    {
        bool addMacro = false;
        bool useMacroTextualCondition = false;

        DEF_MACRO_CLASS* defMacro_ptr = defAttrGroup_ptr->GetMacro(i);
        CONDITION_CLASS* macroCondition_ptr = defAttrGroup_ptr->GetMacroCondition(i);
        if (macroCondition_ptr == NULL)
        {
            // check if a textual condition is defined
            useMacroTextualCondition = (defAttrGroup_ptr->GetMacroTextualCondition(i).length() > 0 ? true : false);

            // There's no condition for this macro, so we'll need to add
            // the attributes of the macro to the results structure.
            if (loggerM_ptr)
            {
                loggerM_ptr->text(LOG_DEBUG, 1, "No Macro Condition for \"%s\" - defaulting to TRUE", defMacro_ptr->GetName().c_str());
            }

            addMacro = true;
        }
        else
        {
//            macroCondition_ptr->Log(loggerM_ptr);
			CONDITION_RESULT_ENUM conditionResult = macroCondition_ptr->Evaluate(dcmAttrGroup_ptr, NULL, loggerM_ptr);
			switch(conditionResult)
			{
			case CONDITION_TRUE:
				// the macro must be included - leave attribute types as defined
                addMacro = true;
				attributeTypeNotImportantSetToType3 = false; // not sure if this is correct - should it keep the value from the caller ?

				if (loggerM_ptr)
                {
                   loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro - Condition for \"%s\" is TRUE", defMacro_ptr->GetName().c_str());
                }
				break;
			case CONDITION_UNIMPORTANT:
				// the macro should be added but all attributes in the macro can be set to Type 3 
				// - their presence is not important to the final validation result.
				addMacro = true;
				attributeTypeNotImportantSetToType3 = true;

	            if (loggerM_ptr)
		        {
					loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro - Condition for \"%s\" is UNIMPORTANT", defMacro_ptr->GetName().c_str());
				}
				break;
			case CONDITION_FALSE:
				if (loggerM_ptr)
				{
                   loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro - Condition for \"%s\" is not FALSE", defMacro_ptr->GetName().c_str());
				}
				break;
			case CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION:
			default:
				if (loggerM_ptr)
				{
                   loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro - Condition for \"%s\" is not supported", defMacro_ptr->GetName().c_str());
				}
				break;
			}
        }

        if (addMacro)
        {
			// create the macro results from the macro definition
            CreateMacroResultsFromDef(defMacro_ptr, 
								dcmAttrGroup_ptr, 
								valAttrGroup_ptr, 
								useMacroTextualCondition,
								attributeTypeNotImportantSetToType3);
        }
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::CreateSQResultsFromDef(VALUE_SQ_CLASS *defValueSq_ptr,
                                    DCM_VALUE_SQ_CLASS *dcmSqValue_ptr,
                                    VAL_VALUE_SQ_CLASS *valSqValue_ptr,
                                    bool useTextualCondition,
									bool attributeTypeNotImportantSetToType3)

//  DESCRIPTION     : Create the Sequence Results from the Sequence Definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	if (dcmSqValue_ptr == NULL) return;

	// handle all the items in the sequence
    for (int i = 0; i < dcmSqValue_ptr->GetNrItems(); i++)
    {
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_DEBUG, 1, "CreateSQResultsFromDef for Item (%d of %d)", i+1, dcmSqValue_ptr->GetNrItems());
        }
		
		VAL_ATTRIBUTE_GROUP_CLASS *valItem_ptr = new VAL_ATTRIBUTE_GROUP_CLASS();

		// link to parent
        valItem_ptr->SetParentSQ(valSqValue_ptr);

		// get the item definition - only one item is defined so we use it over all the dcmSqValue_ptr items
        ATTRIBUTE_GROUP_CLASS *item_ptr;
        defValueSq_ptr->Get(&item_ptr);

#ifdef NDEBUG
        DEF_ATTRIBUTE_GROUP_CLASS *defAttrGroup_ptr = static_cast<DEF_ATTRIBUTE_GROUP_CLASS*>(item_ptr);
#else
        DEF_ATTRIBUTE_GROUP_CLASS *defAttrGroup_ptr = dynamic_cast<DEF_ATTRIBUTE_GROUP_CLASS*>(item_ptr);
#endif

		// create the item results from the item definition
        CreateAttributeGroupResultsFromDef(defAttrGroup_ptr,
									dcmSqValue_ptr->getItem(i),
                                    valItem_ptr,
                                    useTextualCondition,
									attributeTypeNotImportantSetToType3);

		// add to parent
        valSqValue_ptr->AddValItem(valItem_ptr);
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::CreateMacroResultsFromDef(DEF_MACRO_CLASS *defMacro_ptr,
                                       DCM_ATTRIBUTE_GROUP_CLASS *dcmAttrGroup_ptr,
                                       VAL_ATTRIBUTE_GROUP_CLASS *valAttrGroup_ptr,
                                       bool useTextualCondition,
									   bool attributeTypeNotImportantSetToType3)

//  DESCRIPTION     : Create the Marco Results from the Macro Definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (loggerM_ptr)
    {
        loggerM_ptr->text(LOG_DEBUG, 1, "CreateMacroResultsFromDef");
    }

	// handle all attributes in macro
    for (int i = 0; i < defMacro_ptr->GetNrAttributes(); i++)
    {
        DEF_ATTRIBUTE_CLASS *defAttr_ptr = defMacro_ptr->GetAttribute(i);

		// check if the definition attribute is already in the validation attribute group or not
        VAL_ATTRIBUTE_CLASS *valAttr_ptr = valAttrGroup_ptr->GetAttribute(defAttr_ptr->GetGroup(),
																		defAttr_ptr->GetElement());
        if (valAttr_ptr != NULL)
        {
            // the attribute is already in the validation attribute group
            // update the contents of the attribute - if necessary
            valAttr_ptr->SetUseConditionalTextDuringValidation(useTextualCondition);
        }
        else
        {
            if (resultsTypeM == RESULTS_QR)
			{
                valAttr_ptr = new VAL_QR_ATTRIBUTE_CLASS();
			}
            else
			{
                valAttr_ptr = new VAL_ATTRIBUTE_CLASS();
			}

			// link parent
            valAttr_ptr->SetParent(valAttrGroup_ptr);

			// create the attribute results from the attribute definition
            CreateAttributeResultsFromDef(defAttr_ptr, 
									dcmAttrGroup_ptr, 
									valAttr_ptr, 
									useTextualCondition,
									attributeTypeNotImportantSetToType3);

			// set the textual condition usage
            valAttr_ptr->SetUseConditionalTextDuringValidation(useTextualCondition);

			// add to parent
            valAttrGroup_ptr->AddValAttribute(valAttr_ptr);
        }
    }

	// handle all macros in macro
    for (int i = 0; i < defMacro_ptr->GetNrMacros(); i++)
    {
        bool addMacro = false;
        bool useMacroTextualCondition = false;

		CONDITION_CLASS* macroCondition_ptr = defMacro_ptr->GetMacroCondition(i);
        if (macroCondition_ptr == NULL)
        {
            // check if a textual condition is defined
            useMacroTextualCondition = (defMacro_ptr->GetMacroTextualCondition(i).length() > 0 ? true : false);

            // there's no condition for this macro, so we'll need to add
            // the attributes of the macro to the results structure.
            if (loggerM_ptr)
            {
                loggerM_ptr->text(LOG_DEBUG, 1, "Macro in Macro - No Macro Condition for \"%s\" - defaulting to TRUE", defMacro_ptr->GetName().c_str());
            }
            addMacro = true;
        }
        else
        {
//            macroCondition_ptr->Log(loggerM_ptr);
			CONDITION_RESULT_ENUM conditionResult = macroCondition_ptr->Evaluate(dcmAttrGroup_ptr, NULL, loggerM_ptr);
			switch(conditionResult)
			{
			case CONDITION_TRUE:
				// the macro must be included - leave attribute types as defined
                addMacro = true;
				attributeTypeNotImportantSetToType3 = false; // not sure if this is correct - should it keep the value from the caller ?

				if (loggerM_ptr)
                {
                   loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro in Macro - Condition for \"%s\" is TRUE", defMacro_ptr->GetName().c_str());
                }
				break;
			case CONDITION_UNIMPORTANT:
				// the macro should be added but all attributes in the macro can be set to Type 3 
				// - their presence is not important to the final validation result.
				addMacro = true;
				attributeTypeNotImportantSetToType3 = true;

	            if (loggerM_ptr)
		        {
					loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro in Macro - Condition for \"%s\" is UNIMPORTANT", defMacro_ptr->GetName().c_str());
				}
				break;
			case CONDITION_FALSE:
				if (loggerM_ptr)
				{
                   loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro in Macro - Condition for \"%s\" is not FALSE", defMacro_ptr->GetName().c_str());
				}
				break;
			case CONDITION_TRUE_REQUIRES_MANUAL_INTERPRETATION:
			default:
				if (loggerM_ptr)
				{
                   loggerM_ptr->text(LOG_DEBUG, 1, "Conditional Macro in Macro - Condition for \"%s\" is not supported", defMacro_ptr->GetName().c_str());
				}
				break;
			}
        }

        if (addMacro)
        {
			// create the macro results from the macro definition
            CreateMacroResultsFromDef(defMacro_ptr->GetMacro(i),
								dcmAttrGroup_ptr, 
								valAttrGroup_ptr, 
								useMacroTextualCondition,
								attributeTypeNotImportantSetToType3);
        }
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::CreateAttributeResultsFromDef(DEF_ATTRIBUTE_CLASS *defAttr_ptr,
                                           DCM_ATTRIBUTE_GROUP_CLASS *dcmAttrGroup_ptr,
                                           VAL_ATTRIBUTE_CLASS *valAttr_ptr,
                                           bool useTextualCondition,
										   bool attributeTypeNotImportantSetToType3)

//  DESCRIPTION     : Create the Attribute Results from Attribute Definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// set the attribute definition
    valAttr_ptr->SetDefAttribute(defAttr_ptr, attributeTypeNotImportantSetToType3);

    if (defAttr_ptr->GetVR() == ATTR_VR_SQ)
    {
        // TODO: Is it possible to have a list of SQ attributes?
        // if so, loop over the number of values in the dicom attribute group.
#ifdef NDEBUG
        VALUE_SQ_CLASS *defSqValue_ptr = static_cast<VALUE_SQ_CLASS*>(defAttr_ptr->GetValue());
#else
        VALUE_SQ_CLASS *defSqValue_ptr = dynamic_cast<VALUE_SQ_CLASS*>(defAttr_ptr->GetValue());
#endif

        VAL_VALUE_SQ_CLASS *valSqValue_ptr = new VAL_VALUE_SQ_CLASS();

		// link parent
        valSqValue_ptr->SetParent(valAttr_ptr);

		// set definition value list
        valSqValue_ptr->SetDefValueList(defAttr_ptr->GetValueList());

        // get the Dicom attribute, if available. If no attribute is
        // available, we don't build a new SQ attribute results object.
        DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = dcmAttrGroup_ptr->GetMappedAttribute(defAttr_ptr->GetGroup(), 
																		defAttr_ptr->GetElement());
        if (dcmAttr_ptr != NULL)
        {
            if (loggerM_ptr)
            {
                loggerM_ptr->text(LOG_DEBUG, 1, "CreateAttributeResultsFromDef for SQ Attribute (%04X,%04X)", defAttr_ptr->GetGroup(), defAttr_ptr->GetElement());
            }

            // Check if the dcm attribute has any values. If none available,
            // don't build the value results.
            if (dcmAttr_ptr->GetNrValues() > 0)
            {
#ifdef NDEBUG
                DCM_VALUE_SQ_CLASS *dcmSqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(dcmAttr_ptr->GetValue());
#else
                DCM_VALUE_SQ_CLASS *dcmSqValue_ptr = dynamic_cast<DCM_VALUE_SQ_CLASS*>(dcmAttr_ptr->GetValue());
#endif
				// create the sequence results from the sequence definition
                CreateSQResultsFromDef(defSqValue_ptr, 
									dcmSqValue_ptr, 
									valSqValue_ptr, 
									useTextualCondition,
									attributeTypeNotImportantSetToType3);
            }
        }

		// add to parent
        valAttr_ptr->AddValValue(valSqValue_ptr);
    }
    else
    {
        if (loggerM_ptr)
        {
            loggerM_ptr->text(LOG_DEBUG, 1, "CreateAttributeResultsFromDef for Attribute (%04X,%04X)", defAttr_ptr->GetGroup(), defAttr_ptr->GetElement());
        }

		// create the value results from the value definition
        CreateValueResultsFromDef(defAttr_ptr,
							valAttr_ptr);
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::CreateValueResultsFromDef(DEF_ATTRIBUTE_CLASS *defAttr_ptr,
                                       VAL_ATTRIBUTE_CLASS *valAttr_ptr)

//  DESCRIPTION     : Create the value results from the value definition.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// handle all the value definitions
    for (int i = 0; i < defAttr_ptr->GetNrLists(); i++)
    {
        VAL_BASE_VALUE_CLASS *valValue_ptr = new VAL_BASE_VALUE_CLASS();

		// link parent
        valValue_ptr->SetParent(valAttr_ptr);

		// set the value definitions
        valValue_ptr->SetDefValueList(defAttr_ptr->GetValueList(i));

		// add to parent
        valAttr_ptr->AddValValue(valValue_ptr);
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::CheckIfAnyModulesShouldBeIgnored()

//  DESCRIPTION     : Check to see if any of the modules in the results object
//					: should be ignored.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           : A module will be ignored if it is a conditional or user
//					: optional module that only contains one attribute that is
//					: already found in another module. This is our best guess
//					: as to whether the module should be included or not.
//<<===========================================================================
{
	if (objectResultsM_ptr == NULL) return;

	// outer loop over all modules
	for (int i = 0; i < objectResultsM_ptr->GetNrModuleResults(); i++)
	{
        VAL_ATTRIBUTE_GROUP_CLASS *outerLoopModule_ptr = objectResultsM_ptr->GetModuleResults(i);

		// inner loop over all modules
		for (int j = 0; j < objectResultsM_ptr->GetNrModuleResults(); j++)
		{
			// avoid checking against yourself
			if (i != j)
			{
				VAL_ATTRIBUTE_GROUP_CLASS *innerLoopModule_ptr = objectResultsM_ptr->GetModuleResults(j);

				// if a single matching attribute is defined in the outer loop module
				// and not in the inner loop module - see if the single matching attribute is
				// defined in the inner module. If so ignore the outer loop module.
				UINT32 singleMatchingAttribute = outerLoopModule_ptr->GetSingleMatchingAttribute();
				if ((singleMatchingAttribute > 0) &&
					(singleMatchingAttribute != innerLoopModule_ptr->GetSingleMatchingAttribute()))
				{
					if (innerLoopModule_ptr->GetDefAttributeByTag(singleMatchingAttribute) != NULL)
					{
						// ignore the outer loop module
						outerLoopModule_ptr->SetIgnoreThisAttributeGroup(true);
					}
				}
			}
		}
	}
}

//>>===========================================================================

void VALIDATOR_CLASS::SetModuleResultsFromDcm(DCM_ATTRIBUTE_GROUP_CLASS *dcmAttrGroup_ptr,
                                     bool setSourceObjectNotRefObject, bool isCommandSet)

//  DESCRIPTION     : Set the module results from the source/reference dcm object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	// check if results already exist
    if (objectResultsM_ptr == NULL)
    {
		// create new results
        objectResultsM_ptr = new VAL_OBJECT_RESULTS_CLASS();
        resultsTypeM = RESULTS_OBJECT;
    }

	// Set has reference object flag
    if (setSourceObjectNotRefObject == false)
	{
        objectResultsM_ptr->HasReferenceObject(true);
	}

    // The following loop sets the dcm attribute group to all modules that
    // have not yet been assigned a dcm attribute group.
    // What really should be done here is to assign the dcm
    // attribute group to only those modules that have the same attributes
    // in the def attribute group.
    for (int i = 0; i < objectResultsM_ptr->GetNrModuleResults(); i++)
    {
        VAL_ATTRIBUTE_GROUP_CLASS *valAttrGroup_ptr = objectResultsM_ptr->GetModuleResults(i);

		// check if dcm object is a source object or reference object
        if (setSourceObjectNotRefObject)
		{
            if (valAttrGroup_ptr->GetDcmAttributeGroup() == NULL)
			{
                valAttrGroup_ptr->SetDcmAttributeGroup(dcmAttrGroup_ptr);
			}
		}
        else
		{
            if (valAttrGroup_ptr->GetRefAttributeGroup() == NULL)
			{
                valAttrGroup_ptr->SetRefAttributeGroup(dcmAttrGroup_ptr);
			}
		}
    }

	// Iterate over the DCM object itself - handle each attribute in turn
    for (int i = 0; i < dcmAttrGroup_ptr->GetNrAttributes(); i++)
    {
        DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = dcmAttrGroup_ptr->GetAttribute(i);
        UINT16 group = dcmAttr_ptr->GetMappedGroup();
        UINT16 element = dcmAttr_ptr->GetMappedElement();
		VAL_ATTRIBUTE_CLASS *valAttr_ptr = NULL;
		bool isAdditionalAttr = false;

        vector<VAL_ATTRIBUTE_CLASS*> valAttrList;
        valAttrList.clear();

        if (objectResultsM_ptr->GetListOfAttributeResults(group, element, &valAttrList))
        {
            for (UINT i = 0; i < valAttrList.size(); i++)
            {
                valAttr_ptr = valAttrList[i];
                if (setSourceObjectNotRefObject)
				{
					// set a dcm attribute as the source
                    valAttr_ptr->SetDcmAttribute(dcmAttr_ptr);
				}
                else
				{
					// set the dcm attribute as the reference
                    valAttr_ptr->SetRefAttribute(dcmAttr_ptr);
				}

				// check for a SQ attribute - need to handle the enclosed items if present
                if (dcmAttr_ptr->GetVR() == ATTR_VR_SQ)
				{
					// handle the SQ attribute value
                    SetSQResultsFromDcm(dcmAttr_ptr, valAttr_ptr, setSourceObjectNotRefObject);
				}
                else
				{
					// handle the other attribute values
                    SetValueResultsFromDcm(dcmAttr_ptr, valAttr_ptr, setSourceObjectNotRefObject);
				}
            }
        }
        else
        {
            // No attribute is found. Maybe this is a special attribute?
            valAttr_ptr = objectResultsM_ptr->GetAttributeResults(group, element);
            if (valAttr_ptr == NULL)
            {
                // Is the attribute located in a repeating group, and is not a private attribute?
                if ((((group & REPEATING_GROUP_MASK) == REPEATING_GROUP_5000) || 
                     ((group & REPEATING_GROUP_MASK) == REPEATING_GROUP_6000)) &&
                    ((group != REPEATING_GROUP_5000) && 
                     (group != REPEATING_GROUP_6000)))
                {
                    VAL_ATTRIBUTE_GROUP_CLASS *valAttrGroup_ptr = objectResultsM_ptr->GetAGWithAttributeInGroup(group);
                    if (valAttrGroup_ptr != NULL)
					{
                        valAttr_ptr = valAttrGroup_ptr->GetAttribute(group & REPEATING_GROUP_MASK, element);
                        if (valAttr_ptr == NULL)
                        {
                            // The attribute is not known in the object results structure
                            // Add a new validation attribute to an additional attribute
                            // results structure
                            if (resultsTypeM == RESULTS_QR)
				            {
                                valAttr_ptr = new VAL_QR_ATTRIBUTE_CLASS();
				            }
                            else
				            {
                                valAttr_ptr = new VAL_ATTRIBUTE_CLASS();
				            }

                            // Link Attribute into the Group
                            valAttr_ptr->SetParent(valAttrGroup_ptr);
                            valAttrGroup_ptr->AddValAttribute(valAttr_ptr);
                        }

                        // Try to get a definition for the attribute
                        DEF_ATTRIBUTE_CLASS *defAttr_ptr = DEFINITION->GetAttribute(group & REPEATING_GROUP_MASK, element);
                        valAttr_ptr->SetDefAttribute(defAttr_ptr, false);
					}
                    else
                    {
                        // The repeating group is not in the results structure,
                        // create a duplicate results group using the base
                        // def attribute and the parent module of that def
                        // attribute.
                        VAL_ATTRIBUTE_CLASS *repeatAttrResults_ptr = objectResultsM_ptr->GetAttributeResults(group & REPEATING_GROUP_MASK, element);
                        if (repeatAttrResults_ptr != NULL)
                        {
                            DEF_ATTRIBUTE_GROUP_CLASS *defModule_ptr = repeatAttrResults_ptr->GetParent()->GetDefAttributeGroup();
							VAL_ATTRIBUTE_GROUP_CLASS *moduleResults_ptr = new VAL_ATTRIBUTE_GROUP_CLASS();

                            moduleResults_ptr->SetParentObject(objectResultsM_ptr);
                            if (setSourceObjectNotRefObject)
							{
                                moduleResults_ptr->SetDcmAttributeGroup(dcmAttrGroup_ptr);
							}
                            else // ref_results
							{
                                moduleResults_ptr->SetRefAttributeGroup(dcmAttrGroup_ptr);
							}

                            CreateAttributeGroupResultsFromDef(defModule_ptr, dcmAttrGroup_ptr, moduleResults_ptr, false, false);

                            objectResultsM_ptr->AddModuleResults(moduleResults_ptr);
                            valAttr_ptr = moduleResults_ptr->GetAttribute(group & REPEATING_GROUP_MASK, element);
                        }
                    }
                }
            }

            if ((valAttr_ptr == NULL) &&
                !((element == 0x0000) &&				// Don't add group length attributes,
                (group != 0x0000) && (group != 0x0002)  // except for group 0000 and 0002
                )
            )
            {
				isAdditionalAttr = true;

                // The attribute is not known in the object results structure
                // Add a new validation attribute to an additional attribute
                // results structure
                if (resultsTypeM == RESULTS_QR)
				{
                    valAttr_ptr = new VAL_QR_ATTRIBUTE_CLASS();
				}
                else
				{
                    valAttr_ptr = new VAL_ATTRIBUTE_CLASS();
				}

                // Try to get a definition for the attribute
				DEF_ATTRIBUTE_CLASS *defAttr_ptr = NULL;
				if ((defDatasetM_ptr) &&
					(dcmAttr_ptr->GetVR() == ATTR_VR_SQ))
				{
					// If the attribute is a sequence then we need to try to find the full definition of
					// the sequence in the dataset definition (as a sequence contains nested items, etc)
					defAttr_ptr = defDatasetM_ptr->GetAttribute(group, element);
				}
				else if (dcmAttr_ptr->GetVR() != ATTR_VR_SQ)
				{
					// Try to get the definition of the attribute from the registered attributes list
					// - but only if not a sequence as the sequence definition will not be correct
					defAttr_ptr = DEFINITION->GetAttribute(group, element);
				}
                valAttr_ptr->SetDefAttribute(defAttr_ptr, false);                
            }

            if (valAttr_ptr != NULL)
            {
                if (setSourceObjectNotRefObject)
				{
                    valAttr_ptr->SetDcmAttribute(dcmAttr_ptr);
				}
                else
				{
                    valAttr_ptr->SetRefAttribute(dcmAttr_ptr);
				}

                if (dcmAttr_ptr->GetVR() == ATTR_VR_SQ)
				{
                    SetSQResultsFromDcm(dcmAttr_ptr, valAttr_ptr, setSourceObjectNotRefObject);
				}
                else
				{
                    SetValueResultsFromDcm(dcmAttr_ptr, valAttr_ptr, setSourceObjectNotRefObject);
				}
            }

			if (isAdditionalAttr)
			{
				// Get the Additional Attribute Group
                VAL_ATTRIBUTE_GROUP_CLASS *valAdditionalAttrGroup_ptr = objectResultsM_ptr->GetAdditionalAttributeGroup();

				if (isCommandSet)
					valAttr_ptr->GetMessages()->AddMessage(VAL_RULE_DEF_COMMAND_3, "Additional attribute is present in command set");

                // Link Additional Attribute into the Group
                valAttr_ptr->SetParent(valAdditionalAttrGroup_ptr);
                valAdditionalAttrGroup_ptr->AddValAttribute(valAttr_ptr);
			}
        }
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::SetAttributeGroupResultsFromDcm(DCM_ATTRIBUTE_GROUP_CLASS *dcmAttrGroup_ptr,
                                             VAL_ATTRIBUTE_GROUP_CLASS *valAttrGroup_ptr,
                                             bool setSourceObjectNotRefObject)

//  DESCRIPTION     : Set the Attribute Group results from the Dcm object.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    if (setSourceObjectNotRefObject)
	{
        valAttrGroup_ptr->SetDcmAttributeGroup(dcmAttrGroup_ptr);
	}
    else
	{
        valAttrGroup_ptr->SetRefAttributeGroup(dcmAttrGroup_ptr);
	}

    for (int i = 0; i < dcmAttrGroup_ptr->GetNrAttributes(); i++)
    {
		DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = dcmAttrGroup_ptr->GetAttribute(i);
        VAL_ATTRIBUTE_CLASS *valAttr_ptr = valAttrGroup_ptr->GetAttribute(dcmAttr_ptr->GetMappedGroup(), dcmAttr_ptr->GetMappedElement());
        if (valAttr_ptr == NULL)
        {
            // The attribute is not known in the object results structure
            // Add a new validation attribute to the results structure.
            if (resultsTypeM == RESULTS_QR)
			{
                valAttr_ptr = new VAL_QR_ATTRIBUTE_CLASS();
			}
            else
			{
                valAttr_ptr = new VAL_ATTRIBUTE_CLASS();
			}

            valAttr_ptr->SetParent(valAttrGroup_ptr);
            valAttrGroup_ptr->AddValAttribute(valAttr_ptr);
        }

        if (setSourceObjectNotRefObject)
		{
            valAttr_ptr->SetDcmAttribute(dcmAttr_ptr);
		}
        else
		{
            valAttr_ptr->SetRefAttribute(dcmAttr_ptr);
		}

        if (dcmAttr_ptr->GetVR() == ATTR_VR_SQ)
		{
            SetSQResultsFromDcm(dcmAttr_ptr, valAttr_ptr, setSourceObjectNotRefObject);
		}
        else
		{
            SetValueResultsFromDcm(dcmAttr_ptr, valAttr_ptr, setSourceObjectNotRefObject);
		}
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::SetSQResultsFromDcm(DCM_ATTRIBUTE_CLASS *dcmAttr_ptr,
                                 VAL_ATTRIBUTE_CLASS *valAttr_ptr,
                                 bool setSourceObjectNotRefObject)

//  DESCRIPTION     : Set the SQ results from the DCM attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    DCM_VALUE_SQ_CLASS *dcmSqValue_ptr = NULL;
    if (dcmAttr_ptr->GetNrValues() != 0)
    {
#ifdef NDEBUG
        dcmSqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS *>(dcmAttr_ptr->GetValue());
#else
        dcmSqValue_ptr = dynamic_cast<DCM_VALUE_SQ_CLASS *>(dcmAttr_ptr->GetValue());
#endif
    }
    if (dcmSqValue_ptr == NULL)
    {
        // This is apparently an empty sequence. The validation value has
        // been created, so the results structure maps correctly to the
        // incoming data stream, but we can't build any items since we
        // have no data to refer to.
        return;
    }

    VAL_VALUE_SQ_CLASS *valSqValue_ptr = NULL;
    bool useTextualCondition = valAttr_ptr->GetUseConditionalTextDuringValidation();
    if (valAttr_ptr->GetNrValues() == 0)
    {
        // No value allocation for items has been made yet.
        valSqValue_ptr = new VAL_VALUE_SQ_CLASS();
        valSqValue_ptr->SetParent(valAttr_ptr);
        valAttr_ptr->AddValValue(valSqValue_ptr);
    }
    else
    {
#ifdef NDEBUG
        valSqValue_ptr = static_cast<VAL_VALUE_SQ_CLASS *>(valAttr_ptr->GetValue(0));
#else
        valSqValue_ptr = dynamic_cast<VAL_VALUE_SQ_CLASS *>(valAttr_ptr->GetValue(0));
#endif
    }
    // check that we have a val_value
    if (valSqValue_ptr == NULL) return;

    ATTRIBUTE_GROUP_CLASS *attrGroup = NULL;
    DEF_ATTRIBUTE_CLASS *defAttr_ptr = valAttr_ptr->GetDefAttribute();
    if (defAttr_ptr != NULL)
    {
        // We assume that an attribute in the definition component always
        // contains exactly 1 item.
#ifdef NDEBUG
        VALUE_SQ_CLASS *sqValue_ptr = static_cast<VALUE_SQ_CLASS*>(defAttr_ptr->GetValue());
#else
        VALUE_SQ_CLASS *sqValue_ptr = dynamic_cast<VALUE_SQ_CLASS*>(defAttr_ptr->GetValue());
#endif
        if (sqValue_ptr != NULL)
        {
            sqValue_ptr->Get(&attrGroup);
        }
    }

    // We need to create a new validation item results for each
    // item present in the dcm/ref attribute.
    // By default, 1 item will be present in the validation results - since
    // that was the assumption we made while building the results structure
    // with the definition attributes.
    for (int i = 0; i < dcmSqValue_ptr->GetNrItems(); i++)
    {
        VAL_ATTRIBUTE_GROUP_CLASS *valItem_ptr = valSqValue_ptr->GetValItem(i);
        if (valItem_ptr == NULL)
        {
            // There's at least 1 more item in the dcm attribute than in the
            // validation attribute. We need to create a new validation item
            // and copy the definition item to that validation item before
            // we can fill the results structure with the dcm/ref attributes.
            valItem_ptr = new VAL_ATTRIBUTE_GROUP_CLASS();

            valSqValue_ptr->AddValItem(valItem_ptr);
            if (attrGroup != NULL)
            {
                // Fill the new item with the definition attributes.
#ifdef NDEBUG
 				DEF_ITEM_CLASS *defItem_ptr = static_cast<DEF_ITEM_CLASS*>(attrGroup);
#else
 				DEF_ITEM_CLASS *defItem_ptr = dynamic_cast<DEF_ITEM_CLASS*>(attrGroup);
#endif
                CreateAttributeGroupResultsFromDef(defItem_ptr,
                                              dcmSqValue_ptr->getItem(i),
                                              valItem_ptr,
                                              useTextualCondition,
											  false);
            }
        }

        valItem_ptr->SetParentSQ(valSqValue_ptr);
        SetAttributeGroupResultsFromDcm(dcmSqValue_ptr->getItem(i), valItem_ptr, setSourceObjectNotRefObject);
    }

    if (setSourceObjectNotRefObject)
	{
        valSqValue_ptr->SetDcmValue(dcmSqValue_ptr);
	}
    else
	{
        valSqValue_ptr->SetRefValue(dcmSqValue_ptr);
	}
}

//>>===========================================================================

void VALIDATOR_CLASS::SetValueResultsFromDcm(DCM_ATTRIBUTE_CLASS *dcmAttr_ptr,
                                    VAL_ATTRIBUTE_CLASS *valAttr_ptr,
                                    bool setSourceObjectNotRefObject)

//  DESCRIPTION     : Set the attribute Value results from the DCM attribute.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    for (int i = 0; i < dcmAttr_ptr->GetNrValues(); i++)
    {
		VAL_BASE_VALUE_CLASS *valValue_ptr;

        if (i < valAttr_ptr->GetNrValues())
		{
            valValue_ptr = valAttr_ptr->GetValue(i);
		}
        else
        {
            valValue_ptr = new VAL_BASE_VALUE_CLASS();
            valValue_ptr->SetParent(valAttr_ptr);
            valAttr_ptr->AddValValue(valValue_ptr);
        }

        if (setSourceObjectNotRefObject)
		{
            valValue_ptr->SetDcmValue(dcmAttr_ptr->GetValue(i));
		}
        else
		{
            valValue_ptr->SetRefValue(dcmAttr_ptr->GetValue(i));
		}
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::ValidateResults(VALIDATION_CONTROL_FLAG_ENUM validationFlag)

//  DESCRIPTION     : Validates the data present in the object results
//                    structure.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // validate according to the current validation flag settings
    // - validate against the definition
    if (validationFlag & USE_DEFINITION)
    {
        objectResultsM_ptr->ValidateAgainstDef(flagsM);
    }

    // - validate the vr
    if (validationFlag & USE_VR)
    {
        objectResultsM_ptr->ValidateVR(flagsM, specificCharacterSetM_ptr);
    }

    // - validate against the reference
    if (validationFlag & USE_REFERENCE)
    {
        objectResultsM_ptr->ValidateAgainstRef(flagsM);
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::Serialize(BASE_SERIALIZER *serializer_ptr)

//  DESCRIPTION     : Serialize the validation results.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    // serialize the object results
    if (serializer_ptr)
    {
        serializer_ptr->SerializeValidate(objectResultsM_ptr, flagsM);
    }
}

//>>===========================================================================

void VALIDATOR_CLASS::LogSystemDefinitions()

//  DESCRIPTION     : Log the system definitions.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
    LOG_MESSAGE_CLASS *messages_ptr = objectResultsM_ptr->GetMessages();
    messages_ptr->AddMessage (VAL_RULE_DEF_DEFINITION_1, "No Dataset definition found - no validation done");

    string names[MAX_APPLICATION_ENTITY];
    string versions[MAX_APPLICATION_ENTITY];
    DEFINITION->GetSystemDefinitions(names, versions);

    string msg;
	int index = 0;
    while (names[index] != "")
    {
		msg = "Name: \"" + names[index] + "\"  Version: \"" + versions[index] + "\"";
        messages_ptr->AddMessage(VAL_RULE_DEF_DEFINITION_2, msg);
        index++;
    }
}

//>>===========================================================================

bool VALIDATOR_CLASS::UpdateDatasetResultsFromLastSent(DCM_COMMAND_CLASS *command_ptr,
								DCM_COMMAND_CLASS *lastSentCommand_ptr,
								DCM_DATASET_CLASS *lastSentDataset_ptr)

//  DESCRIPTION     : Update the Dataset Results from the last command/dataset sent.
//					: Example: If the last command/dataset sent was a C-FIND-RQ Identifier
//					: then in order to validate the results of the C-FIND-RSP Dataset we should
//					: take the Identifer into account.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	if ((command_ptr == NULL) || 
		(lastSentCommand_ptr == NULL)) return true;

	// Get the command field
	UINT16	commandField = 0;
	command_ptr->getUSValue(TAG_COMMAND_FIELD, &commandField);

	UINT16 lastSentCommandField = 0;
	lastSentCommand_ptr->getUSValue(TAG_COMMAND_FIELD, &lastSentCommandField);

	bool status = true;
	if ((commandField == C_FIND_RSP) &&
		(lastSentCommandField == C_FIND_RQ))
	{
		// we are validating a C-FIND-RSP following a C-FIND-RQ being sent
		status = UpdateDatasetResultsFromLastSentDataset(lastSentDataset_ptr);
	}

	return status;
}

//>>===========================================================================

bool VALIDATOR_CLASS::UpdateDatasetResultsFromLastSentDataset(DCM_DATASET_CLASS *lastSentDataset_ptr)

//  DESCRIPTION     : Update the Dataset Results from the last dataset sent.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	char message[256];

	// check if results already exist
    if (objectResultsM_ptr == NULL) return false;

	// check if last sent dataset is defined
	if (lastSentDataset_ptr == NULL) return false;

	bool status = true;

	// iterate over the last sent dataset object itself - handle each attribute in turn
    for (int i = 0; i < lastSentDataset_ptr->GetNrAttributes(); i++)
    {
        DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = lastSentDataset_ptr->GetAttribute(i);
        UINT16 group = dcmAttr_ptr->GetMappedGroup();
        UINT16 element = dcmAttr_ptr->GetMappedElement();

        vector<VAL_ATTRIBUTE_CLASS*> valAttrList;
        valAttrList.clear();

        if (objectResultsM_ptr->GetListOfAttributeResults(group, element, &valAttrList))
        {
            for (UINT j = 0; j < valAttrList.size(); j++)
            {
 				VAL_ATTRIBUTE_CLASS *valAttr_ptr = valAttrList[j];
				DEF_ATTRIBUTE_CLASS *defAttr_ptr = valAttr_ptr->GetDefAttribute();
				if (defAttr_ptr != NULL)
				{
					// check for a SQ attribute - need to handle the enclosed items if present
		            if (defAttr_ptr->GetVR() == ATTR_VR_SQ)
					{
						// handle the SQ attribute
						VAL_VALUE_SQ_CLASS *valSqValue_ptr = NULL;
#ifdef NDEBUG
						valSqValue_ptr = static_cast<VAL_VALUE_SQ_CLASS *>(valAttr_ptr->GetValue(0));
#else
						valSqValue_ptr = dynamic_cast<VAL_VALUE_SQ_CLASS *>(valAttr_ptr->GetValue(0));
#endif
						DCM_VALUE_SQ_CLASS *sqValue_ptr = NULL;
						if (dcmAttr_ptr->GetNrValues())
						{
							sqValue_ptr = static_cast<DCM_VALUE_SQ_CLASS*>(dcmAttr_ptr->GetValue(0));
						}

						UpdateSequenceItemsResultsFromLastSentDataset(valSqValue_ptr, sqValue_ptr);
					}
					else
					{
						if (dcmAttr_ptr->GetNrValues())
						{
							BASE_VALUE_CLASS* value_ptr = dcmAttr_ptr->GetValue();
							if (value_ptr->GetLength() == 0)
							{
								if (valAttr_ptr->GetDefAttributeType() == ATTR_TYPE_1C)
								{
									// Message to user - DVTk cannot determine if this 1C attribute should be present or not - needs further
									// manual validation.
									LOG_MESSAGE_CLASS *message_ptr = valAttr_ptr->GetMessages();
									if (message_ptr != NULL)
									{
										sprintf(message, "Equivalent attribute (%04X,%04X) in request has zero-length - maybe response attribute. Changing this Attribute Type from %s to Type 2 - requires further manual validation.",
											dcmAttr_ptr->GetGroup(),
											dcmAttr_ptr->GetElement(),
											mapTypeName(valAttr_ptr->GetDefAttributeType()));
										message_ptr->AddMessage(VAL_RULE_DEF_DEFINITION_7, message);
									}

									valAttr_ptr->SetDefAttributeType(ATTR_TYPE_2);
								}
							}
							else
							{
								if ((valAttr_ptr->GetDefAttributeType() != ATTR_TYPE_1) &&
									(valAttr_ptr->GetDefAttributeType() != ATTR_TYPE_1C))
								{
									LOG_MESSAGE_CLASS *message_ptr = valAttr_ptr->GetMessages();
									if (message_ptr != NULL)
									{
										sprintf(message, "Equivalent attribute (%04X,%04X) in request has value - expecting response attribute. Changing this Attribute Type from %s to Type 1.",
											dcmAttr_ptr->GetGroup(),
											dcmAttr_ptr->GetElement(),
											mapTypeName(valAttr_ptr->GetDefAttributeType()));
										message_ptr->AddMessage(VAL_RULE_DEF_DEFINITION_7, message);
									}

									valAttr_ptr->SetDefAttributeType(ATTR_TYPE_1);
								}
							}
						}
					}
				}
            }
        }
	}

	// Now iterate over the validation results and set any definition attribute which does not
	// have a DCM attribute to Type 3. This means that response definition is updated according 
	// to the requested attributes.
    for (int i = 0; i < objectResultsM_ptr->GetNrModuleResults(); i++)
    {
        VAL_ATTRIBUTE_GROUP_CLASS *valAttrGroup_ptr = objectResultsM_ptr->GetModuleResults(i);

		for (int j = 0; j < valAttrGroup_ptr->GetNrAttributes(); j++)
		{
			VAL_ATTRIBUTE_CLASS *valAttr_ptr = valAttrGroup_ptr->GetAttribute(j);
			if ((valAttr_ptr) &&
				(valAttr_ptr->GetDcmAttribute() == NULL))
			{
				valAttr_ptr->SetDefAttributeType(ATTR_TYPE_3);
			}
		}
   }

	return status;
}


//>>===========================================================================

bool VALIDATOR_CLASS::UpdateSequenceItemsResultsFromLastSentDataset(VAL_VALUE_SQ_CLASS *valSqValue_ptr, DCM_VALUE_SQ_CLASS *sqValue_ptr)

//  DESCRIPTION     : Update the Sequence Items Results from the last dataset sent.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      :
//  NOTES           :
//<<===========================================================================
{
	char message[256];

	if ((valSqValue_ptr == NULL) ||
		(sqValue_ptr == NULL))
	{
		return false;
	}

	if (valSqValue_ptr->GetNrValItems() == sqValue_ptr->GetNrItems())
	{
		// iterate over all the items
		for (int i = 0; i < sqValue_ptr->GetNrItems(); i++)
		{
			DCM_ITEM_CLASS *dcmItem_ptr = sqValue_ptr->getItem(i);
			VAL_ATTRIBUTE_GROUP_CLASS *valItem_ptr = valSqValue_ptr->GetValItem(i);

			for (int j = 0; j < dcmItem_ptr->GetNrAttributes(); j++)
			{
				DCM_ATTRIBUTE_CLASS *dcmAttr_ptr = dcmItem_ptr->GetAttribute(j);
				if (dcmAttr_ptr)
				{
					VAL_ATTRIBUTE_CLASS *valAttr_ptr = valItem_ptr->GetAttribute(dcmAttr_ptr->GetMappedGroup(), dcmAttr_ptr->GetMappedElement());
					if (valAttr_ptr)
					{
						if (dcmAttr_ptr->GetNrValues())
						{
							BASE_VALUE_CLASS* value_ptr = dcmAttr_ptr->GetValue();
							if (value_ptr->GetLength() == 0)
							{
								if (valAttr_ptr->GetDefAttributeType() == ATTR_TYPE_1C)
								{
									// Message to user - DVTk cannot determine if this 1C attribute should be present or not - needs further
									// manual validation.
									LOG_MESSAGE_CLASS *message_ptr = valAttr_ptr->GetMessages();
									if (message_ptr != NULL)
									{
										sprintf(message, "Equivalent attribute (%04X,%04X) in request has zero-length - maybe response attribute. Changing this Attribute Type from %s to Type 2 - requires further manual validation.",
											dcmAttr_ptr->GetMappedGroup(),
											dcmAttr_ptr->GetMappedElement(),
											mapTypeName(valAttr_ptr->GetDefAttributeType()));
										message_ptr->AddMessage(VAL_RULE_DEF_DEFINITION_7, message);
									}

									valAttr_ptr->SetDefAttributeType(ATTR_TYPE_2);
								}
							}
							else
							{
								if ((valAttr_ptr->GetDefAttributeType() != ATTR_TYPE_1) &&
									(valAttr_ptr->GetDefAttributeType() != ATTR_TYPE_1C))
								{
									LOG_MESSAGE_CLASS *message_ptr = valAttr_ptr->GetMessages();
									if (message_ptr != NULL)
									{
										sprintf(message, "Equivalent attribute (%04X,%04X) in request has value - expecting response attribute. Changing this Attribute Type from %s to Type 1.",
											dcmAttr_ptr->GetMappedGroup(),
											dcmAttr_ptr->GetMappedElement(),
											mapTypeName(valAttr_ptr->GetDefAttributeType()));
										message_ptr->AddMessage(VAL_RULE_DEF_DEFINITION_7, message);
									}

									valAttr_ptr->SetDefAttributeType(ATTR_TYPE_1);
								}
							}
						}
					}
				}
			}
		}
	}

	// Now iterate over the validation results and set any definition attribute which does not
	// have a DCM attribute to Type 3. This means that response definition is updated according 
	// to the requested attributes.
	// iterate over all the items
	for (int i = 0; i < valSqValue_ptr->GetNrValItems(); i++)
	{
		VAL_ATTRIBUTE_GROUP_CLASS *valItem_ptr = valSqValue_ptr->GetValItem(i);

		for (int j = 0; j < valItem_ptr->GetNrAttributes(); j++)
		{
			VAL_ATTRIBUTE_CLASS *valAttr_ptr = valItem_ptr->GetAttribute(j);
			if ((valAttr_ptr) &&
				(valAttr_ptr->GetDcmAttribute() == NULL))
			{
				valAttr_ptr->SetDefAttributeType(ATTR_TYPE_3);
			}
		}
	}

	return true;
}
