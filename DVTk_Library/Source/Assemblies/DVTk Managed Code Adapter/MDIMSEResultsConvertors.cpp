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
#include "MDIMSEResultsConvertors.h"
#include "MDIMSEConvertors.h"
#include "ActivityReportingAdapter.h"
#include "ValidationMessageLevels.h"
#include "CountingAdapter.h"
#include "UtilityFunctions.h"
#include <assert.h>
#using <mscorlib.dll>

//
// Unmanaged to Managed
//
namespace ManagedUnManagedDimseValidationResultsConvertors
{
    using namespace System::Runtime::InteropServices;
    using namespace DvtkData::Validation;
    using namespace DvtkData::Validation::SubItems;
    using namespace ManagedUnManagedDimseConvertors;
    using namespace Wrappers;

    /*private:*/
    /*static*/
    DvtkData::Validation::DataElementType
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        ::ATTR_TYPE_ENUM dataElementType)
    {
        switch (dataElementType)
        {
        case ::ATTR_TYPE_1  : return DvtkData::Validation::DataElementType::Item1;
        case ::ATTR_TYPE_1C : return DvtkData::Validation::DataElementType::Item1C;
        case ::ATTR_TYPE_2  : return DvtkData::Validation::DataElementType::Item2;
        case ::ATTR_TYPE_2C : return DvtkData::Validation::DataElementType::Item2C;
        case ::ATTR_TYPE_3  : return DvtkData::Validation::DataElementType::Item3;
        case ::ATTR_TYPE_3C : return DvtkData::Validation::DataElementType::Item3C;
        case ::ATTR_TYPE_3R : return DvtkData::Validation::DataElementType::Item3R;
        default:
            assert(false);
            return DvtkData::Validation::DataElementType::Item1;
        }
    }

    /*private:*/
    /*static*/
    DvtkData::Validation::DirectoryRecordType
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        ::DICOMDIR_RECORD_TYPE_ENUM directoryRecordType)
    {
        switch (directoryRecordType)
        {
        case ::DICOMDIR_RECORD_TYPE_ROOT            : return DirectoryRecordType::ROOT;
        case ::DICOMDIR_RECORD_TYPE_PATIENT         : return DirectoryRecordType::PATIENT;
        case ::DICOMDIR_RECORD_TYPE_STUDY           : return DirectoryRecordType::STUDY;
        case ::DICOMDIR_RECORD_TYPE_SERIES          : return DirectoryRecordType::SERIES;
        case ::DICOMDIR_RECORD_TYPE_IMAGE           : return DirectoryRecordType::IMAGE; 
        case ::DICOMDIR_RECORD_TYPE_OVERLAY         : return DirectoryRecordType::OVERLAY;
        case ::DICOMDIR_RECORD_TYPE_MODALITY_LUT    : return DirectoryRecordType::MODALITY_LUT;
        case ::DICOMDIR_RECORD_TYPE_VOI_LUT         : return DirectoryRecordType::VOI_LUT;
        case ::DICOMDIR_RECORD_TYPE_CURVE           : return DirectoryRecordType::CURVE;
        case ::DICOMDIR_RECORD_TYPE_TOPIC           : return DirectoryRecordType::TOPIC;
        case ::DICOMDIR_RECORD_TYPE_VISIT           : return DirectoryRecordType::VISIT;
        case ::DICOMDIR_RECORD_TYPE_RESULTS         : return DirectoryRecordType::RESULTS;
        case ::DICOMDIR_RECORD_TYPE_INTERPRETATION  : return DirectoryRecordType::INTERPRETATION;
        case ::DICOMDIR_RECORD_TYPE_STUDY_COMPONENT : return DirectoryRecordType::STUDY_COMPONENT;
        case ::DICOMDIR_RECORD_TYPE_PRINT_QUEUE     : return DirectoryRecordType::PRINT_QUEUE;
        case ::DICOMDIR_RECORD_TYPE_FILM_SESSION    : return DirectoryRecordType::FILM_SESSION;
        case ::DICOMDIR_RECORD_TYPE_FILM_BOX        : return DirectoryRecordType::FILM_BOX;
        case ::DICOMDIR_RECORD_TYPE_IMAGE_BOX       : return DirectoryRecordType::IMAGE_BOX;
		case ::DICOMDIR_RECORD_TYPE_REGISTRATION	: return DirectoryRecordType::REGISTRATION;
		case ::DICOMDIR_RECORD_TYPE_RAW_DATA		: return DirectoryRecordType::RAW_DATA;
		case ::DICOMDIR_RECORD_TYPE_ENCAP_DOC		: return DirectoryRecordType::ENCAP_DOC;
		case ::DICOMDIR_RECORD_TYPE_SPECTROSCOPY	: return DirectoryRecordType::SPECTROSCOPY;
		case ::DICOMDIR_RECORD_TYPE_FIDUCIAL		: return DirectoryRecordType::FIDUCIAL;
        case ::DICOMDIR_RECORD_TYPE_STORED_PRINT    : return DirectoryRecordType::STORED_PRINT;
        case ::DICOMDIR_RECORD_TYPE_RT_DOSE         : return DirectoryRecordType::RT_DOSE;
        case ::DICOMDIR_RECORD_TYPE_RT_PLAN         : return DirectoryRecordType::RT_PLAN;
        case ::DICOMDIR_RECORD_TYPE_RT_STRUCTURE_SET : return DirectoryRecordType::RT_STRUCTURE_SET;
        case ::DICOMDIR_RECORD_TYPE_RT_TREAT_RECORD : return DirectoryRecordType::RT_TREAT_RECORD;
        case ::DICOMDIR_RECORD_TYPE_PRESENTATION    : return DirectoryRecordType::PRESENTATION;
        case ::DICOMDIR_RECORD_TYPE_SR_DOCUMENT     : return DirectoryRecordType::SR_DOCUMENT;
        case ::DICOMDIR_RECORD_TYPE_KEY_OBJECT_DOC  : return DirectoryRecordType::KEY_OBJECT_DOC;
        case ::DICOMDIR_RECORD_TYPE_WAVEFORM        : return DirectoryRecordType::WAVEFORM;
        case ::DICOMDIR_RECORD_TYPE_PRIVATE         : return DirectoryRecordType::PRIVATE;
        case ::DICOMDIR_RECORD_TYPE_MRDR            : return DirectoryRecordType::MRDR;
		case ::DICOMDIR_RECORD_TYPE_HANGING_PROTOCOL: return DirectoryRecordType::HANGING_PROTOCOL;
        case ::DICOMDIR_RECORD_TYPE_HL7_STRUC_DOC   : return DirectoryRecordType::HL7_STRUC_DOC;
        case ::DICOMDIR_RECORD_TYPE_STEREOMETRIC    : return DirectoryRecordType::STEREOMETRIC;
        case ::DICOMDIR_RECORD_TYPE_VALUE_MAP       : return DirectoryRecordType::VALUE_MAP;
        case ::DICOMDIR_RECORD_TYPE_UNKNOWN         : return DirectoryRecordType::UNKNOWN;
        default:
            assert(false);
            return DirectoryRecordType::UNKNOWN;
        }
    }

    DvtkData::Validation::MessageType
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        Wrappers::WrappedValidationMessageLevel level)
    {
        switch (level)
        {
        case Wrappers::WrappedValidationMessageLevel::Debug :
        case Wrappers::WrappedValidationMessageLevel::Error :
            {
                if (m_pCountingTarget != NULL)
				{
					//m_pCountingTarget->IncrementValidationError();
					m_pCountingTarget->Increment(CountGroup::Validation, CountType::Error);
				}
			}
			return DvtkData::Validation::MessageType::Error;
		case Wrappers::WrappedValidationMessageLevel::Information :
			return DvtkData::Validation::MessageType::Info;
		case Wrappers::WrappedValidationMessageLevel::ConditionText :
			return DvtkData::Validation::MessageType::ConditionText;
		case Wrappers::WrappedValidationMessageLevel::Warning :
			{
				if (m_pCountingTarget != NULL)
				{
					//m_pCountingTarget->IncrementValidationWarning();
					m_pCountingTarget->Increment(CountGroup::Validation, CountType::Warning);
				}
            }
            return DvtkData::Validation::MessageType::Warning;
        case Wrappers::WrappedValidationMessageLevel::None :
            return DvtkData::Validation::MessageType::None;
        case Wrappers::WrappedValidationMessageLevel::DicomObjectRelationship :
        case Wrappers::WrappedValidationMessageLevel::DulpStateMachine :
        case Wrappers::WrappedValidationMessageLevel::Scripting :
        case Wrappers::WrappedValidationMessageLevel::ScriptName :
        case Wrappers::WrappedValidationMessageLevel::MediaFilename :
        case Wrappers::WrappedValidationMessageLevel::WareHouseLabel :
        default:
            assert(false);
            return DvtkData::Validation::MessageType::Error;
        }
    }

    /*private:*/
    /*static*/
    DvtkData::Validation::ModuleUsageType
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        ::MOD_USAGE_ENUM moduleUsage)
    {
        switch(moduleUsage)
        {
        case ::MOD_USAGE_C: return DvtkData::Validation::ModuleUsageType::Conditional;
        case ::MOD_USAGE_M: return DvtkData::Validation::ModuleUsageType::Mandatory;
        case ::MOD_USAGE_U: return DvtkData::Validation::ModuleUsageType::UserOptional;
        default:
            assert(false);
            return DvtkData::Validation::ModuleUsageType::Conditional;
        }
    }

    // <summary>
    // Constructor
    // </summary>
    ManagedUnManagedDimseValidationResultsConvertor::ManagedUnManagedDimseValidationResultsConvertor(void)
    {
        this->m_pRulesUri = NULL;
    }

    // <summary>
    // Destructor
    // </summary>
    ManagedUnManagedDimseValidationResultsConvertor::~ManagedUnManagedDimseValidationResultsConvertor(void)
    {
    }

    // <summary>
    // Convert unmanaged to managed - DIMSE validation object result
    // </summary>
    DvtkData::Validation::ValidationObjectResult __gc*
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        VAL_OBJECT_RESULTS_CLASS *pUMValidationObjectResult, UINT flags)
    {
        //
        // Preconditions
        //
        if (pUMValidationObjectResult == NULL) return NULL;
        //
        // Create node
        //
        DvtkData::Validation::ValidationObjectResult *pValidationObjectResult =
            new DvtkData::Validation::ValidationObjectResult();
        //
        // Convert parameters
        //
        pValidationObjectResult->Name = ConvertString(pUMValidationObjectResult->GetName()).c_str();
		pValidationObjectResult->DICOMDIRName = ConvertString(pUMValidationObjectResult->GetDICOMDIRName()).c_str();
		pValidationObjectResult->SpecificCharacterSetValues = ConvertString(pUMValidationObjectResult->GetSpecificCharacterSetValues()).c_str();
        for (UINT i = 0; i < (UINT)pUMValidationObjectResult->GetNrModuleResults(); i++)
        {
			VAL_ATTRIBUTE_GROUP_CLASS *module_ptr = pUMValidationObjectResult->GetModuleResults(i);

			// check if the module should be ignored
			if (module_ptr->GetIgnoreThisAttributeGroup() == false)
			{
				DvtkData::Validation::SubItems::ValidationAttributeGroupResult
					*pValidationAttributeGroupResult = Convert(module_ptr, flags);
				pValidationObjectResult->Modules->Add(pValidationAttributeGroupResult);
			}
        }
        if (pUMValidationObjectResult->HasMessages())
        {
            LOG_MESSAGE_CLASS* pUMLOG_MESSAGE_CLASS = pUMValidationObjectResult->GetMessages();
            for (UINT i = 0; i < (UINT)pUMLOG_MESSAGE_CLASS->GetNrMessages(); i++)
            {
                DvtkData::Validation::ValidationMessage __gc* pValidationMessage =
                    new DvtkData::Validation::ValidationMessage();
                System::UInt32 index = pUMLOG_MESSAGE_CLASS->GetIndex(i);
                pValidationMessage->Index = index;
                System::UInt32 messageUID = pUMLOG_MESSAGE_CLASS->GetMessageId(i);
                pValidationMessage->Identifier = messageUID;
                pValidationMessage->Message = ConvertString(pUMLOG_MESSAGE_CLASS->GetMessage(i)).c_str();
                Wrappers::WrappedValidationMessageLevel level =
                    Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
                pValidationMessage->Type = Convert(level);
                pValidationObjectResult->Messages->Add(pValidationMessage);
            }
        }

        pValidationObjectResult->AdditionalAttributes =
            Convert(pUMValidationObjectResult->GetAdditionalAttributeGroup(), ATTR_FLAG_ADDITIONAL_ATTRIBUTE);
        //
        // Do not serialize additional attributes (container) tag if there are no attributes.
        //
        if (
            pValidationObjectResult->AdditionalAttributes != NULL &&
            pValidationObjectResult->AdditionalAttributes->Attributes->Count == 0
            )
        {
            //
            // Remove tag
            //
            pValidationObjectResult->AdditionalAttributes = NULL;
        }
        else
        {
            //
            // set the module name and usage
            //
            pValidationObjectResult->AdditionalAttributes->Name = "Additional Attributes Module";
            pValidationObjectResult->AdditionalAttributes->Usage = DvtkData::Validation::ModuleUsageType::UserOptional;
        }
        return pValidationObjectResult;
    }

    // <summary>
    // Convert unmanaged to managed - Validation Directory Record Links
    // </summary>
    DvtkData::Validation::TypeSafeCollections::ValidationDirectoryRecordLinkCollection __gc*
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        RECORD_LINK_VECTOR* pUMRecordLinkVector)
    {
        //
        // Create node
        //
        DvtkData::Validation::TypeSafeCollections::ValidationDirectoryRecordLinkCollection __gc*
            pValidationDirectoryRecordLinkCollection
            = new DvtkData::Validation::TypeSafeCollections::ValidationDirectoryRecordLinkCollection();
        //
        // Convert individual record links
        //
        for (UINT dirRecIndex = 0; dirRecIndex < pUMRecordLinkVector->size(); dirRecIndex++)
        {
            DvtkData::Validation::SubItems::ValidationDirectoryRecordLink __gc*
                pValidationDirectoryRecordLink =
                new DvtkData::Validation::SubItems::ValidationDirectoryRecordLink();
            RECORD_LINK_CLASS* pRECORD_LINK = (*pUMRecordLinkVector)[dirRecIndex];
            pValidationDirectoryRecordLink->RecordOffset = 
                pRECORD_LINK->GetRecordOffset();
            pValidationDirectoryRecordLink->ReferenceCount = 
                pRECORD_LINK->GetReferenceCount();
            pValidationDirectoryRecordLink->DirectoryRecordType = 
                Convert(pRECORD_LINK->GetRecordType());
            pValidationDirectoryRecordLink->DirectoryRecordIndex = dirRecIndex;
            if (pRECORD_LINK->HasMessages())
            {
                LOG_MESSAGE_CLASS* pUMLOG_MESSAGE_CLASS =
                    pRECORD_LINK->GetMessages();
                for (UINT i = 0; i < (UINT)pUMLOG_MESSAGE_CLASS->GetNrMessages(); i++)
                {
                    DvtkData::Validation::ValidationMessage __gc* pValidationMessage =
                        new DvtkData::Validation::ValidationMessage();
                    System::UInt32 index = pUMLOG_MESSAGE_CLASS->GetIndex(i);
                    pValidationMessage->Index = index;
                    System::UInt32 messageUID = pUMLOG_MESSAGE_CLASS->GetMessageId(i);
                    pValidationMessage->Identifier = messageUID;
                    pValidationMessage->Message = ConvertString(pUMLOG_MESSAGE_CLASS->GetMessage(i)).c_str();
                    Wrappers::WrappedValidationMessageLevel level =
                        Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
                    pValidationMessage->Type = Convert(level);
                    pValidationDirectoryRecordLink->Messages->Add(pValidationMessage);
                }
            }
            pValidationDirectoryRecordLinkCollection->Add(pValidationDirectoryRecordLink);
        }
        return pValidationDirectoryRecordLinkCollection;
    }

    // <summary>
    // Convert unmanaged to managed - Validation Directory Record Result
    // </summary>
    DvtkData::Validation::ValidationDirectoryRecordResult __gc*
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        RECORD_RESULTS_CLASS *pUMValidationDirectoryRecordResult, UINT flags)
    {
        //
        // Preconditions
        //
        if (pUMValidationDirectoryRecordResult == NULL) return NULL;
		/* TODO: Determine whether the diff-counters are still needed when using child-serializers!?*/
        //
        // Get current error count - temporary (affected by converter process!)
        //
		System::UInt32 oldNrOfErrors = this->m_pCountingTarget->get_NrOfErrors();
		System::UInt32 oldNrOfWarnings = this->m_pCountingTarget->get_NrOfWarnings();
        //
        // Create node
        //
        DvtkData::Validation::ValidationDirectoryRecordResult
            *pValidationDirectoryRecordResult =
            new DvtkData::Validation::ValidationDirectoryRecordResult();
        //
        // Convert parameters
        //
        for (UINT i = 0; i < (UINT)pUMValidationDirectoryRecordResult->GetNrAttributes(); i++)
        {
            DvtkData::Validation::SubItems::ValidationAttributeResult
                *pValidationAttributeResult =
                Convert(pUMValidationDirectoryRecordResult->GetAttribute(i), flags);
            if (pValidationAttributeResult)
            {
                pValidationDirectoryRecordResult->Attributes->Add(pValidationAttributeResult);
            }
        }
        if (pUMValidationDirectoryRecordResult->GetDefAttributeGroup())
        {
            if (pUMValidationDirectoryRecordResult->GetDefAttributeGroup()->GetTextualCondition().length() != 0)
            {
                pValidationDirectoryRecordResult->ConditionText =
                    ConvertString(pUMValidationDirectoryRecordResult->GetDefAttributeGroup()->GetTextualCondition()).c_str();
            }
        }

        if (pUMValidationDirectoryRecordResult->HasMessages())
        {
            LOG_MESSAGE_CLASS* pUMLOG_MESSAGE_CLASS =
                pUMValidationDirectoryRecordResult->GetMessages();
            for (UINT i = 0; i < (UINT)pUMLOG_MESSAGE_CLASS->GetNrMessages(); i++)
            {
                DvtkData::Validation::ValidationMessage __gc* pValidationMessage =
                    new DvtkData::Validation::ValidationMessage();
                System::UInt32 index = pUMLOG_MESSAGE_CLASS->GetIndex(i);
                pValidationMessage->Index = index;
                System::UInt32 messageUID = pUMLOG_MESSAGE_CLASS->GetMessageId(i);
                pValidationMessage->Identifier = messageUID;
                pValidationMessage->Message = ConvertString(pUMLOG_MESSAGE_CLASS->GetMessage(i)).c_str();
                Wrappers::WrappedValidationMessageLevel level =
                    Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
                pValidationMessage->Type = Convert(level);
                pValidationDirectoryRecordResult->Messages->Add(pValidationMessage);
            }
        }
        if (pUMValidationDirectoryRecordResult->GetDefAttributeGroup())
        { 
            pValidationDirectoryRecordResult->Name =
                ConvertString(pUMValidationDirectoryRecordResult->GetDefAttributeGroup()->GetName()).c_str();

            pValidationDirectoryRecordResult->Usage =
                Convert(pUMValidationDirectoryRecordResult->GetDefAttributeGroup()->GetUsage());
        }
        pValidationDirectoryRecordResult->DirectoryRecordType = 
            Convert(pUMValidationDirectoryRecordResult->GetRecordType());
        //
        // DicomDirectoryIndex
        //
        pValidationDirectoryRecordResult->DirectoryRecordIndex =
            pUMValidationDirectoryRecordResult->GetRecordIndex();
        //
        // Corresponding referenced file
        //
        pValidationDirectoryRecordResult->ReferencedFile =
            Convert(pUMValidationDirectoryRecordResult->GetRefFileResults(), flags);
		/* TODO: Determine whether the diff-counters are still needed when using child-serializers!?*/
        //
        // Determine increase in errors and warnings for this node
        //
		System::UInt32 newNrOfErrors = this->m_pCountingTarget->get_NrOfErrors();
		System::UInt32 newNrOfWarnings = this->m_pCountingTarget->get_NrOfWarnings();
        pValidationDirectoryRecordResult->NrOfErrors = newNrOfErrors - oldNrOfErrors;
		pValidationDirectoryRecordResult->NrOfWarnings = newNrOfWarnings - oldNrOfWarnings;
        return pValidationDirectoryRecordResult;
    }

    // <summary>
    // Convert unmanaged to managed - Validation Attribute Group Result
    // </summary>
    DvtkData::Validation::SubItems::ValidationAttributeGroupResult __gc*
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        VAL_ATTRIBUTE_GROUP_CLASS *pUMValidationAttributeGroupResult, UINT flags)
    {
        //
        // Preconditions
        //
        if (pUMValidationAttributeGroupResult == NULL) return NULL;
        //
        // Create node
        //
        DvtkData::Validation::SubItems::ValidationAttributeGroupResult
            *pValidationAttributeGroupResult =
            new DvtkData::Validation::SubItems::ValidationAttributeGroupResult();
        //
        // Convert parameters
        //
        for (UINT i = 0; i < (UINT)pUMValidationAttributeGroupResult->GetNrAttributes(); i++)
        {
            DvtkData::Validation::SubItems::ValidationAttributeResult
                *pValidationAttributeResult =
                Convert(pUMValidationAttributeGroupResult->GetAttribute(i), flags);
            if (pValidationAttributeResult)
            {
                pValidationAttributeGroupResult->Attributes->Add(pValidationAttributeResult);
            }
        }
        if (pUMValidationAttributeGroupResult->GetDefAttributeGroup())
        {
            if (pUMValidationAttributeGroupResult->GetDefAttributeGroup()->GetTextualCondition().length() != 0)
            {
                pValidationAttributeGroupResult->ConditionText =
                    ConvertString(pUMValidationAttributeGroupResult->GetDefAttributeGroup()->GetTextualCondition()).c_str();
            }
        }
        if (pUMValidationAttributeGroupResult->HasMessages())
        {
            LOG_MESSAGE_CLASS* pUMLOG_MESSAGE_CLASS =
                pUMValidationAttributeGroupResult->GetMessages();
            for (UINT i = 0; i < (UINT)pUMLOG_MESSAGE_CLASS->GetNrMessages(); i++)
            {
                DvtkData::Validation::ValidationMessage __gc* pValidationMessage =
                    new DvtkData::Validation::ValidationMessage();
                System::UInt32 index = pUMLOG_MESSAGE_CLASS->GetIndex(i);
                pValidationMessage->Index = index;
                System::UInt32 messageUID = pUMLOG_MESSAGE_CLASS->GetMessageId(i);
                pValidationMessage->Identifier = messageUID;
                pValidationMessage->Message = ConvertString(pUMLOG_MESSAGE_CLASS->GetMessage(i)).c_str();
                Wrappers::WrappedValidationMessageLevel level =
                    Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
                pValidationMessage->Type = Convert(level);
                pValidationAttributeGroupResult->Messages->Add(pValidationMessage);
            }
        }
        if (pUMValidationAttributeGroupResult->GetDefAttributeGroup())
        { 
            pValidationAttributeGroupResult->Name =
                ConvertString(pUMValidationAttributeGroupResult->GetDefAttributeGroup()->GetName()).c_str();

            pValidationAttributeGroupResult->Usage =
                Convert(pUMValidationAttributeGroupResult->GetDefAttributeGroup()->GetUsage());
        }
        return pValidationAttributeGroupResult;
    }

    // <summary>
    // Convert unmanaged to managed - Validation Attribute Result
    // </summary>
    DvtkData::Validation::SubItems::ValidationAttributeResult __gc*
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        VAL_ATTRIBUTE_CLASS *pUMValidationAttributeResult, UINT flags)
    {
        //
        // Preconditions
        //
        if (pUMValidationAttributeResult == NULL) return NULL;

        //
        // Convert parameters
        //
        DEF_ATTRIBUTE_CLASS* def_attr = pUMValidationAttributeResult->GetDefAttribute();
        DCM_ATTRIBUTE_CLASS* dcm_attr = pUMValidationAttributeResult->GetDcmAttribute();
        DCM_ATTRIBUTE_CLASS* ref_attr = pUMValidationAttributeResult->GetRefAttribute();
        ATTRIBUTE_CLASS*  pAttr = def_attr;
        if (pAttr == NULL) pAttr = dcm_attr;
        if (pAttr == NULL) pAttr = ref_attr;
        if (pAttr == NULL) System::Diagnostics::Trace::Assert(false);

        // if the attribute is Type 3 and not present - we might not want to include it in the
        // conversion
        if (flags & ATTR_FLAG_DO_NOT_INCLUDE_TYPE3)
        {
            // check if attribute is type 3
            if ((def_attr) &&
				((pUMValidationAttributeResult->GetDefAttributeType() == ATTR_TYPE_3) ||
                (pUMValidationAttributeResult->GetDefAttributeType() == ATTR_TYPE_3C) ||
                (pUMValidationAttributeResult->GetDefAttributeType() == ATTR_TYPE_3R)))
            {
                // check if attribute is present - without validation errors
                if ((dcm_attr == NULL) &&
                    (pUMValidationAttributeResult->HasMessages() == false))
                {
                    // do not include in conversion
                    return NULL;
                }
            }
        }

        //
        // Create node
        // 
        DvtkData::Validation::SubItems::ValidationAttributeResult
            *pValidationAttributeResult =
            new DvtkData::Validation::SubItems::ValidationAttributeResult();
 
        //
        // Data element type
        //
        if (flags & ATTR_FLAG_ADDITIONAL_ATTRIBUTE)
        {
             pValidationAttributeResult->DataElementType = Convert(ATTR_TYPE_3);
        }
        else
        {
            pValidationAttributeResult->DataElementType = Convert(pUMValidationAttributeResult->GetDefAttributeType());
        }
        //
        // Presence (incomming dcm message attribute is present)
        //
        pValidationAttributeResult->Presence = (dcm_attr != NULL);

		// Should we display the VR of DCM attribute in attribute validation results
		TRANSFER_ATTR_VR_ENUM transferVr = TRANSFER_ATTR_VR_UNKNOWN;
		if(dcm_attr != NULL)
		{
			transferVr = dcm_attr->getTransferVR();
		}
		if(transferVr == TRANSFER_ATTR_VR_IMPLICIT)
		{
			pValidationAttributeResult->DisplayVR = false;
		}

        //
        // Attribute name (attribute name as specified by the definition)
        //
		string attributeName;
        if (def_attr == NULL) 
        {
            // If attribute is not in the definition, try retrieve
            // the attribute name via the loaded definitions.
            attributeName = DEFINITION->GetAttributeName(pAttr->GetMappedGroup(), 
												pAttr->GetMappedElement());
        }
        else 
        {
            attributeName = def_attr->GetName();
        }
		//
		// check if any private attribute mapping as been done
		//
		if ((dcm_attr) &&
			(dcm_attr->GetElement() != dcm_attr->GetMappedElement()))
		{
			char buffer[64];
			sprintf(buffer,
				" : private mapped to (%04X,%04X)", 
				dcm_attr->GetMappedGroup(), 
				dcm_attr->GetMappedElement());

            // private attribute has been mapped - indicate in name
            attributeName.append(buffer);
		}
		else if ((ref_attr) &&
			(ref_attr->GetElement() != ref_attr->GetMappedElement()))
		{
			char buffer[64];
			sprintf(buffer,
				" : private mapped to (%04X,%04X)", 
				ref_attr->GetMappedGroup(), 
				ref_attr->GetMappedElement());

            // private attribute has been mapped - indicate in name
            attributeName.append(buffer);
		}
        pValidationAttributeResult->Name = ConvertString(attributeName).c_str();

        //
        // Messages
        //
        if (pUMValidationAttributeResult->HasMessages())
        {
            LOG_MESSAGE_CLASS* pUMLOG_MESSAGE_CLASS =
                pUMValidationAttributeResult->GetMessages();
            for (UINT i = 0; i < (UINT)pUMLOG_MESSAGE_CLASS->GetNrMessages(); i++)
            {
                DvtkData::Validation::ValidationMessage __gc* pValidationMessage =
                    new DvtkData::Validation::ValidationMessage();
                System::UInt32 index = pUMLOG_MESSAGE_CLASS->GetIndex(i);
                pValidationMessage->Index = index;
                System::UInt32 messageUID = pUMLOG_MESSAGE_CLASS->GetMessageId(i);
                pValidationMessage->Identifier = messageUID;
                pValidationMessage->Message = ConvertString(pUMLOG_MESSAGE_CLASS->GetMessage(i)).c_str();
                Wrappers::WrappedValidationMessageLevel level =
                    Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
                pValidationMessage->Type = Convert(level);
                pValidationAttributeResult->Messages->Add(pValidationMessage);
            }
        }
        //
        // Tag
        //
        // dcm_attr tag values take precedence over def_attr tag values if present
        UINT16 group = pAttr->GetGroup();
        UINT16 element = pAttr->GetElement();
        if ((dcm_attr) &&
            ((dcm_attr->GetGroup() != group) ||
             (dcm_attr->GetElement() != element)))
        {
            group = dcm_attr->GetGroup();
            element = dcm_attr->GetElement();
        }
        pValidationAttributeResult->Tag = new DvtkData::Dimse::Tag(group, element);

        // get the definition VR
        ATTR_VR_ENUM vr = pUMValidationAttributeResult->GetDefAttributeVr();
        pValidationAttributeResult->ValueRepresentation =
            ManagedUnManagedDimseConvertor::Convert(vr);

		// if dataset atttribute present - get it's VR
		ATTR_VR_ENUM dcmVr = ATTR_VR_UN;
		if (dcm_attr)
		{
			dcmVr = dcm_attr->GetVR();
		}
		pValidationAttributeResult->ActualValueRepresentation = 
            ManagedUnManagedDimseConvertor::Convert(dcmVr);

		//If attribute is not present in def file and it's present
		// in DCM object then we should take DCM VR in account
		// for proper result display
		if((vr == ATTR_VR_UN) && (dcmVr != ATTR_VR_UN))
		{
			vr = dcmVr;
		}

        // get length
        if (dcm_attr != NULL)
        {
			// get length - first take actual received length which could be odd
			UINT32 length = dcm_attr->getReceivedLength();
			if (length == 0)
			{
				// received length may not be defined so go for padded length
				length = dcm_attr->getPaddedLength();
			}

		    pValidationAttributeResult->Length = length;
        }

		//
        // Values
        //
        for (UINT i = 0; i < (UINT)pUMValidationAttributeResult->GetNrValues(); i++)
        {
            DvtkData::Validation::SubItems::ValidationValueResult __gc*
                pValidationValueResult =
                Convert(pUMValidationAttributeResult->GetValue(i), vr, flags);
            pValidationAttributeResult->Values->Add(pValidationValueResult);
        }
        return pValidationAttributeResult;
    }

    // <summary>
    // Convert unmanaged to managed - Validation Value Result
    // </summary>
    DvtkData::Validation::SubItems::ValidationValueResult __gc*
        ManagedUnManagedDimseValidationResultsConvertor::Convert(
        ::VAL_BASE_VALUE_CLASS *pUMValBaseValue,
        ::ATTR_VR_ENUM vr,
        UINT flags)
    {
        //
        // Create node
        //
        DvtkData::Validation::SubItems::ValidationValueResult __gc*
            pValidationValueResult =
            new DvtkData::Validation::SubItems::ValidationValueResult();
        //
        // Convert parameters
        //
        if (pUMValBaseValue->HasMessages())
        {
            LOG_MESSAGE_CLASS* pUMLOG_MESSAGE_CLASS =
                pUMValBaseValue->GetMessages();
            for (UINT i = 0; i < (UINT)pUMLOG_MESSAGE_CLASS->GetNrMessages(); i++)
            {
                DvtkData::Validation::ValidationMessage __gc* pValidationMessage =
                    new DvtkData::Validation::ValidationMessage();
                System::UInt32 index = pUMLOG_MESSAGE_CLASS->GetIndex(i);
                pValidationMessage->Index = index;
                System::UInt32 messageUID = pUMLOG_MESSAGE_CLASS->GetMessageId(i);
                pValidationMessage->Identifier = messageUID;
                pValidationMessage->Message = ConvertString(pUMLOG_MESSAGE_CLASS->GetMessage(i)).c_str();
                Wrappers::WrappedValidationMessageLevel level =
                    Wrappers::ValidationMessageInfo::GetLevel(messageUID, this->m_pRulesUri);
                pValidationMessage->Type = Convert(level);
                pValidationValueResult->Messages->Add(pValidationMessage);
            }
        }
        //
        // Type upcast!
        // Item = <System::String | ValidationAttributeGroupResult[]>
        //
        System::Object __gc* item = NULL;
        if (vr == ::ATTR_VR_SQ)
        {
            VAL_VALUE_SQ_CLASS * sq = NULL;
#ifdef NDEBUG
            sq = static_cast<VAL_VALUE_SQ_CLASS *>(pUMValBaseValue);
#else
            sq = dynamic_cast<VAL_VALUE_SQ_CLASS *>(pUMValBaseValue);
#endif
            UINT nrOfSequenceItems = sq->GetNrValItems();

            DvtkData::Validation::SubItems::ValidationAttributeGroupResult __gc*
                sequenceOfItems[] =
                new DvtkData::Validation::SubItems::ValidationAttributeGroupResult __gc*[nrOfSequenceItems];

            for (UINT i = 0 ; i < nrOfSequenceItems; i++)
            {
                DvtkData::Validation::SubItems::ValidationAttributeGroupResult __gc*
                    pValidationAttributeGroupResult =
                    Convert(sq->GetValItem(i), flags);
                sequenceOfItems[i] = pValidationAttributeGroupResult;
            }
            item = sequenceOfItems;
        }
        else if (vr == ::ATTR_VR_ST || vr == ::ATTR_VR_LT || vr == ::ATTR_VR_UT)
        {
            //
            // VRs; ST, LT and UT support interface Get(unsigned char **, UINT32 &)
            //
            if (pUMValBaseValue->GetDcmValue())
            {
                item = 
                    ManagedUnManagedDimseConvertor::ConvertLongString(pUMValBaseValue->GetDcmValue());
            }
        }
        else if (vr == ::ATTR_VR_UN)
        {
            //
            // VR UN is not displayed as a string!
            //
            item = new System::String("...");
        }
		else if (vr == ::ATTR_VR_US)
		{
            if (pUMValBaseValue->GetDcmValue())
            {
				char buffer[32];
				UINT16 valueUint16;
				pUMValBaseValue->GetDcmValue()->Get(valueUint16);
				sprintf(buffer,"0x%04X=%d", valueUint16, valueUint16);
				item = new System::String(buffer);
			}
		}
		else if (vr == ::ATTR_VR_UL)
		{
            if (pUMValBaseValue->GetDcmValue())
            {
				char buffer[32];
				UINT32 valueUint32;
			    pUMValBaseValue->GetDcmValue()->Get(valueUint32);
				sprintf(buffer,"0x%08X=%d", valueUint32, valueUint32);
				item = new System::String(buffer);
			}
		}
		else if (vr == ::ATTR_VR_FD)
		{
            if (pUMValBaseValue->GetDcmValue())
            {
				item = (ManagedUnManagedDimseConvertor::ConvertFD(pUMValBaseValue->GetDcmValue())).ToString();
			}
		}
        else
        {
            //
            // Other VRs support interface Get(string &, bool stripped = false)
            //
            if (pUMValBaseValue->GetDcmValue())
            {
                std::string valueString;
                pUMValBaseValue->GetDcmValue()->Get(valueString);
                item = new System::String(valueString.c_str());
           }
        }
        pValidationValueResult->Item = item;
        return pValidationValueResult;
    }

    // <summary>
    // Set rules for the validation process.
    // </summary>
    // <remarks>
    // Can be either strict or non-strict.
    // </remarks>
    void ManagedUnManagedDimseValidationResultsConvertor::set_Rules(System::Uri __gc* pRulesUri)
    {
        this->m_pRulesUri = pRulesUri;
        return;
    }

    // <summary>
    // Set the counting target for the validation process.
    // </summary>
    // <remarks>
    // The counting adapter will count the number of errors and warnings based on the rules.
    // </remarks>
	void ManagedUnManagedDimseValidationResultsConvertor::set_CountingTarget(ICountingTarget __gc* pCountingTarget)
    {
        this->m_pCountingTarget = pCountingTarget;
    }
}
