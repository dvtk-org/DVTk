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
#include "MDIMSEConvertors.h"
#include "UtilityFunctions.h"

#using <mscorlib.dll>

namespace ManagedUnManagedDimseConvertors
{
    using namespace DvtkData::Dimse;
    using namespace Wrappers;
    using namespace System::Runtime::InteropServices;

    //>>===========================================================================

    ManagedUnManagedDimseConvertor::ManagedUnManagedDimseConvertor(void)

        //  DESCRIPTION     : Class destructor.
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // constructor activities
    }

    //>>===========================================================================

    ManagedUnManagedDimseConvertor::~ManagedUnManagedDimseConvertor(void)

        //  DESCRIPTION     : Class destructor.
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // destructor activities
    }

    //=============================================================================
    //
    // Unmanaged to Managed
    //
    //=============================================================================

    DvtkData::Dimse::VR
        ManagedUnManagedDimseConvertor::Convert(ATTR_VR_ENUM vr)
    {
        switch (vr)
        {
        case ::ATTR_VR_AE: return DvtkData::Dimse::VR::AE; // Application Entity
        case ::ATTR_VR_AS: return DvtkData::Dimse::VR::AS; // Age String
        case ::ATTR_VR_AT: return DvtkData::Dimse::VR::AT; // Attribute Tag
        case ::ATTR_VR_CS: return DvtkData::Dimse::VR::CS; // Code String
        case ::ATTR_VR_DA: return DvtkData::Dimse::VR::DA; // Date
        case ::ATTR_VR_DS: return DvtkData::Dimse::VR::DS; // Decimal String
        case ::ATTR_VR_DT: return DvtkData::Dimse::VR::DT; // Date Time
        case ::ATTR_VR_FL: return DvtkData::Dimse::VR::FL; // Floating Point Single
        case ::ATTR_VR_FD: return DvtkData::Dimse::VR::FD; // Floating Point Double
        case ::ATTR_VR_IS: return DvtkData::Dimse::VR::IS; // Integer String
        case ::ATTR_VR_LO: return DvtkData::Dimse::VR::LO; // Long String
        case ::ATTR_VR_LT: return DvtkData::Dimse::VR::LT; // Long Text
        case ::ATTR_VR_OB: return DvtkData::Dimse::VR::OB; // Other Byte String
        case ::ATTR_VR_OF: return DvtkData::Dimse::VR::OF; // Other Float String
        case ::ATTR_VR_OW: return DvtkData::Dimse::VR::OW; // Other Word String
		case ::ATTR_VR_OL: return DvtkData::Dimse::VR::OL; // Other Long String
		case ::ATTR_VR_OV: return DvtkData::Dimse::VR::OV; // Other Very Long String
		case ::ATTR_VR_OD: return DvtkData::Dimse::VR::OD; // Other Double String
        case ::ATTR_VR_PN: return DvtkData::Dimse::VR::PN; // Person Name
        case ::ATTR_VR_SH: return DvtkData::Dimse::VR::SH; // Short String
        case ::ATTR_VR_SL: return DvtkData::Dimse::VR::SL; // Signed Long
        case ::ATTR_VR_SQ: return DvtkData::Dimse::VR::SQ; // Sequence of Items
        case ::ATTR_VR_SS: return DvtkData::Dimse::VR::SS; // Signed Short
        case ::ATTR_VR_ST: return DvtkData::Dimse::VR::ST; // Short Text
        case ::ATTR_VR_TM: return DvtkData::Dimse::VR::TM; // Time
        case ::ATTR_VR_UI: return DvtkData::Dimse::VR::UI; // Unique Identifier (UID)
        case ::ATTR_VR_UL: return DvtkData::Dimse::VR::UL; // Unsigned Long
        case ::ATTR_VR_UN: return DvtkData::Dimse::VR::UN; // Unknown
        case ::ATTR_VR_US: return DvtkData::Dimse::VR::US; // Unsigned Short
        case ::ATTR_VR_UT: return DvtkData::Dimse::VR::UT; // Unlimited Text
		case ::ATTR_VR_UC: return DvtkData::Dimse::VR::UC; // Unlimited Characters
		case ::ATTR_VR_UR: return DvtkData::Dimse::VR::UR; // Universal Resource Identifier
            // Determine what to do with the following VR enums.
        case ::ATTR_VR_DOESNOTEXIST:
            assert(false);
            return DvtkData::Dimse::VR::UN;
        case ::ATTR_VR_QQ:
            assert(false); 
            return DvtkData::Dimse::VR::UN;
        default:
            assert(false);
            return DvtkData::Dimse::VR::UN;
        }
    }

    //>>===========================================================================

    DvtkData::Dimse::CommandSet __gc* 
        ManagedUnManagedDimseConvertor::Convert(DCM_COMMAND_CLASS *pUMCommand)

        //  DESCRIPTION     : Convert unmanaged to managed - DIMSE command
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMCommand == NULL) return NULL;

       // set the command id from that defined in the ummanaged command set
        DvtkData::Dimse::CommandSet *pCommand = Convert(pUMCommand->getCommandId());

        // the Command Field is added during the construction of the managed object
        // - we don't want it when doing the conversion from the unmanaged object as
        // it is part of the unmanaged object too. (Otherwise the attribute apprears
        // twice in the converted object.)
        if (pCommand->Count == 1)
        {
            // remove the Command Field
            pCommand->RemoveAt(0);
        }

        // convert command attributes
        Convert(pCommand, pUMCommand);

        return pCommand;
    }

	DvtkData::Dimse::AttributeType
        ManagedUnManagedDimseConvertor::Convert(
        ::ATTR_TYPE_ENUM dataElementType)
    {
        switch (dataElementType)
        {
        case ::ATTR_TYPE_1  : return DvtkData::Dimse::AttributeType::Item1;
        case ::ATTR_TYPE_1C : return DvtkData::Dimse::AttributeType::Item1C;
        case ::ATTR_TYPE_2  : return DvtkData::Dimse::AttributeType::Item2;
        case ::ATTR_TYPE_2C : return DvtkData::Dimse::AttributeType::Item2C;
        case ::ATTR_TYPE_3  : return DvtkData::Dimse::AttributeType::Item3;
        case ::ATTR_TYPE_3C : return DvtkData::Dimse::AttributeType::Item3C;
        case ::ATTR_TYPE_3R : return DvtkData::Dimse::AttributeType::Item3R;
        default:
            assert(false);
            return DvtkData::Dimse::AttributeType::Item1;
        }
    }

    //>>===========================================================================

    DvtkData::Dimse::CommandSet __gc* 
        ManagedUnManagedDimseConvertor::Convert(DIMSE_CMD_ENUM umCommandId)

        //  DESCRIPTION     : Convert unmanaged to managed - CommandId enumerate
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dimse::CommandSet *pCommand = NULL;

        switch(umCommandId)
        {
        case DIMSE_CMD_CCANCEL_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CCANCELRQ);
            break;
        case DIMSE_CMD_CECHO_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CECHORQ);
            break;
        case DIMSE_CMD_CECHO_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CECHORSP);
            break;
        case DIMSE_CMD_CFIND_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CFINDRQ);
            break;
        case DIMSE_CMD_CFIND_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CFINDRSP);
            break;
        case DIMSE_CMD_CGET_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CGETRQ);
            break;
        case DIMSE_CMD_CGET_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CGETRSP);
            break;
        case DIMSE_CMD_CMOVE_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CMOVERQ);
            break;
        case DIMSE_CMD_CMOVE_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CMOVERSP);
            break;
        case DIMSE_CMD_CSTORE_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CSTORERQ);
            break;
        case DIMSE_CMD_CSTORE_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::CSTORERSP);
            break;
        case DIMSE_CMD_NACTION_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NACTIONRQ);
            break;
        case DIMSE_CMD_NACTION_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NACTIONRSP);
            break;
        case DIMSE_CMD_NCREATE_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NCREATERQ);
            break;
        case DIMSE_CMD_NCREATE_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NCREATERSP);
            break;
        case DIMSE_CMD_NDELETE_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NDELETERQ);
            break;
        case DIMSE_CMD_NDELETE_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NDELETERSP);
            break;
        case DIMSE_CMD_NEVENTREPORT_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NEVENTREPORTRQ);
            break;
        case DIMSE_CMD_NEVENTREPORT_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NEVENTREPORTRSP);
            break;
        case DIMSE_CMD_NGET_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NGETRQ);
            break;
        case DIMSE_CMD_NGET_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NGETRSP);
            break;
        case DIMSE_CMD_NSET_RQ:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NSETRQ);
            break;
        case DIMSE_CMD_NSET_RSP:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::NSETRSP);
            break;
        default:
            pCommand = new DvtkData::Dimse::CommandSet(DimseCommand::UNDEFINED);
            break;
        }

        return pCommand;
    }

    //>>===========================================================================

    DvtkData::Dimse::DataSet __gc* 
        ManagedUnManagedDimseConvertor::Convert(DCM_DATASET_CLASS *pUMDataset)

        //  DESCRIPTION     : Convert unmanaged to managed - DIMSE dataset
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMDataset == NULL) return NULL;

        DvtkData::Dimse::DataSet *pDataset = new DvtkData::Dimse::DataSet();
        if (pUMDataset->getIodName() != NULL)
        {
            pDataset->IodId = pUMDataset->getIodName();
        }

        // convert dataset attributes
        Convert(pDataset, pUMDataset);

        return pDataset;
    }

    //>>===========================================================================

    DvtkData::Dimse::SequenceItem __gc* 
        ManagedUnManagedDimseConvertor::Convert(DCM_ITEM_CLASS *pUMItem)

        //  DESCRIPTION     : Convert unmanaged to managed - DIMSE item
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMItem == NULL) return NULL;

        DvtkData::Dimse::SequenceItem *pItem = new DvtkData::Dimse::SequenceItem();

		// get item encoding details
		pItem->DefinedLength = pUMItem->isDefinedLength();
		pItem->IntroducerGroup = pUMItem->getIntroducerGroup();
		pItem->IntroducerElement = pUMItem->getIntroducerElement();
		pItem->IntroducerLength= pUMItem->getIntroducerLength();
		pItem->IntroducerGroup = pUMItem->getIntroducerGroup();
		pItem->IntroducerElement = pUMItem->getIntroducerElement();
		pItem->IntroducerLength = pUMItem->getIntroducerLength();

        // convert item attributes
        Convert(pItem, pUMItem);

        return pItem;
    }

    //>>===========================================================================

    void
        ManagedUnManagedDimseConvertor::Convert(
        /*dst*/ DvtkData::Dimse::AttributeSet __gc *pAttributeSet, 
        /*src*/ DCM_ATTRIBUTE_GROUP_CLASS *pUMAttributeGroup)

        //  DESCRIPTION     : Convert unmanaged to managed - attribute group to attribute set
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if ((pAttributeSet == NULL) ||
            (pUMAttributeGroup == NULL)) return;

        // copy each attribute in the attribute group
        for (int i = 0; i < pUMAttributeGroup->GetNrAttributes(); i++)
        {
            DCM_ATTRIBUTE_CLASS *pUMAttribute = pUMAttributeGroup->GetAttribute(i);
            if (pUMAttribute)
            {
                // convert the attribute
                DvtkData::Dimse::Attribute *pAttribute = Convert(pUMAttribute);
                pAttributeSet->Add(pAttribute);
            }
        }
    }

    //>>===========================================================================

    DvtkData::Dimse::Attribute __gc* 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS* pUMAttribute)

        //  DESCRIPTION     : Convert unmanaged to managed - attribute
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pUMAttribute == NULL) return NULL;

        DvtkData::Dimse::Attribute *pAttribute = new DvtkData::Dimse::Attribute();

        // add tag
        pAttribute->Tag = Convert(pUMAttribute->GetGroup(), pUMAttribute->GetElement());

        // get name
        string attributeName = DEFINITION->GetAttributeName(pUMAttribute->GetMappedGroup(), 
														pUMAttribute->GetMappedElement()).c_str();

		/*
		//get type
		pAttribute->Type = Convert(DEFINITION->GetAttributeType(pUMAttribute->GetMappedGroup(), 
														pUMAttribute->GetMappedElement()));
		*/

		// check if any private attribute mapping as been done
		if (pUMAttribute->GetElement() != pUMAttribute->GetMappedElement())
		{
			char buffer[64];
			sprintf(buffer,
				" : private mapped to (%04X,%04X)", 
				pUMAttribute->GetMappedGroup(), 
				pUMAttribute->GetMappedElement());

            // private attribute has been mapped - indicate in name
            attributeName.append(buffer);
		}

		// Should we display the VR of attribute in attribute dump
		if(pUMAttribute->getTransferVR() == TRANSFER_ATTR_VR_IMPLICIT)
		{
			pAttribute->DisplayVR = false;
		}

		// get length - first take actual received length which could be odd
		UINT32 length = pUMAttribute->getReceivedLength();
		if (length == 0)
		{
			// received length may not be defined so go for padded length
			pUMAttribute->getPaddedLength();
		}
		pAttribute->Length = length;

        // check if the attribute has been marked as not present (i.e. deleted)
        if (!pUMAttribute->IsPresent())
        {
            // attribute has been marked as deleted - indicate in name
            attributeName.append(" - DELETED");
        }

        // implicit marshalling by compiler
        pAttribute->Name = attributeName.c_str();

        // set attribute values
        pAttribute->DicomValue = Convert(pUMAttribute->GetVR(), length, pUMAttribute->GetValueList());

        return pAttribute;
    }

    //>>===========================================================================

    DvtkData::Dimse::Tag __gc* 
        ManagedUnManagedDimseConvertor::Convert(System::UInt16 group, System::UInt16 element)

        //  DESCRIPTION     : Convert unmanaged to managed - attribute tag
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // create the tag
        DvtkData::Dimse::Tag *pTag = new DvtkData::Dimse::Tag(group, element);
        return pTag;
    }

    //>>===========================================================================

    DvtkData::Dimse::DicomValueType __gc* 
        ManagedUnManagedDimseConvertor::Convert(ATTR_VR_ENUM vr, UINT32 length, VALUE_LIST_CLASS *pUMValueList)

        //  DESCRIPTION     : Convert unmanaged to managed - attribute values
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // create the value list
        DvtkData::Dimse::DicomValueType *pDicomValue = NULL;

        // assign values based on the VR
        switch (vr)
        {
        case ATTR_VR_AE:
            {
                DvtkData::Dimse::ApplicationEntity *pApplicationEntity = new DvtkData::Dimse::ApplicationEntity();
                pApplicationEntity->Values = new DvtkData::Collections::StringCollection();

                // convert all AE values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pApplicationEntity->Values->Add(pString);
                    }
                }
                pDicomValue = pApplicationEntity;
                break;
            }
        case ATTR_VR_AS:
            {
                DvtkData::Dimse::AgeString *pAgeString = new DvtkData::Dimse::AgeString();
                pAgeString->Values = new DvtkData::Collections::StringCollection();

                // convert all AS values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pAgeString->Values->Add(pString);
                    }
                }
                pDicomValue = pAgeString;
                break;
            }
        case ATTR_VR_AT:
            {
                DvtkData::Dimse::AttributeTag *pAttributeTag = new DvtkData::Dimse::AttributeTag();
                pAttributeTag->Values = new DvtkData::Collections::TagCollection();

                // convert all AT values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        DvtkData::Dimse::Tag *pTag = ConvertAT(pUMValueList->GetValue(i));
                        pAttributeTag->Values->Add(pTag);
                    }
                }
                pDicomValue = pAttributeTag;
                break;
            }
        case ATTR_VR_CS:
            {
                DvtkData::Dimse::CodeString *pCodeString = new DvtkData::Dimse::CodeString();
                pCodeString->Values = new DvtkData::Collections::StringCollection();

                // convert all CS values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pCodeString->Values->Add(pString);
                    }
                }
                pDicomValue = pCodeString;
                break;
            }
        case ATTR_VR_DA:
            {
                DvtkData::Dimse::Date *pDate = new DvtkData::Dimse::Date();
                pDate->Values = new DvtkData::Collections::StringCollection();

                // convert all DA values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pDate->Values->Add(pString);
                    }
                }
                pDicomValue = pDate;
                break;
            }
        case ATTR_VR_DS:
            {
                DvtkData::Dimse::DecimalString *pDecimalString = new DvtkData::Dimse::DecimalString();
                pDecimalString->Values = new DvtkData::Collections::StringCollection();

                // convert all DS values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pDecimalString->Values->Add(pString);
                    }
                }
                pDicomValue = pDecimalString;
                break;
            }
        case ATTR_VR_DT:
            {
                DvtkData::Dimse::DateTime *pDateTime = new DvtkData::Dimse::DateTime();
                pDateTime->Values = new DvtkData::Collections::StringCollection();

                // convert all DT values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pDateTime->Values->Add(pString);
                    }
                }
                pDicomValue = pDateTime;
                break;
            }
        case ATTR_VR_FD:
            {
                DvtkData::Dimse::FloatingPointDouble *pFloatingPointDouble = new DvtkData::Dimse::FloatingPointDouble();
                pFloatingPointDouble->Values = new DvtkData::Collections::DoubleCollection();

                // convert all FD values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::Double value = ConvertFD(pUMValueList->GetValue(i));
                        pFloatingPointDouble->Values->Add(value);
                    }
                }
                pDicomValue = pFloatingPointDouble;
                break;
            }
        case ATTR_VR_FL:
            {
                DvtkData::Dimse::FloatingPointSingle *pFloatingPointSingle = new DvtkData::Dimse::FloatingPointSingle();
                pFloatingPointSingle->Values = new DvtkData::Collections::SingleCollection();

                // convert all FL values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::Single value = ConvertFL(pUMValueList->GetValue(i));
                        pFloatingPointSingle->Values->Add(value);
                    }
                }
                pDicomValue = pFloatingPointSingle;
                break;
            }
        case ATTR_VR_IS:
            {
                DvtkData::Dimse::IntegerString *pIntegerString = new DvtkData::Dimse::IntegerString();
                pIntegerString->Values = new DvtkData::Collections::StringCollection();

                // convert all IS values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pIntegerString->Values->Add(pString);
                    }
                }
                pDicomValue = pIntegerString;
                break;
            }
        case ATTR_VR_LO:
            {
                DvtkData::Dimse::LongString *pLongString = new DvtkData::Dimse::LongString();
                pLongString->Values = new DvtkData::Collections::StringCollection();

                // convert all LO values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pLongString->Values->Add(pString);
                    }
                }
                pDicomValue = pLongString;
                break;
            }
        case ATTR_VR_LT:
            {
                DvtkData::Dimse::LongText *pLongText = new DvtkData::Dimse::LongText();

                // convert LT value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        System::String *pStringValue = ConvertLongString(pUMValueList->GetValue(0));
                        pLongText->Value = pStringValue;
                    }
                }
                pDicomValue = pLongText;
                break;
            }
        case ATTR_VR_OB:
            {
                DvtkData::Dimse::OtherByteString *pOtherByteString = new DvtkData::Dimse::OtherByteString();

                // convert OB value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        OTHER_VALUE_CLASS *pUMOtherValue = (OTHER_VALUE_CLASS*)pUMValueList->GetValue(0);

                        // check if data in a file
                        string filename;
                        if (pUMOtherValue->Get(filename, true) == MSG_OK)
                        {
                            // set the filename
                            pOtherByteString->FileName = filename.c_str();
                        }
                        else
                        {
                            // get pattern values
                            // 0 = rows
                            // 1 = columns
                            // 2 = start_value
                            // 3 = rows_increment
                            // 4 = columns_increment
                            // 5 = rows_same
                            // 6 = columns_same
                            DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = new DvtkData::Dimse::BitmapPatternParameters();

                            for (int i = 0; i < 7; i++)
                            {
                                UINT32 value;
                                pUMOtherValue->Get((UINT32) i, value);
                                switch(i)
                                {
                                case 0: pBitmapPattern->NumberOfRows = (System::UInt16) value; break;
                                case 1: pBitmapPattern->NumberOfColumns = (System::UInt16) value; break;
                                case 2: pBitmapPattern->StartValue = (System::UInt16) value; break;
                                case 3: pBitmapPattern->ValueIncrementPerRowBlock = (System::UInt16) value; break;
                                case 4: pBitmapPattern->ValueIncrementPerColumnBlock = (System::UInt16) value; break;
                                case 5: pBitmapPattern->NumberOfIdenticalValueRows = (System::UInt16) value; break;
                                case 6: pBitmapPattern->NumberOfIdenticalValueColumns = (System::UInt16) value; break;
                                default: throw new System::ApplicationException(); break;
                                }
                            }
                            pOtherByteString->BitmapPattern = pBitmapPattern;
                        }

						// data compressed
						pOtherByteString->Compressed = pUMOtherValue->IsCompressed();
                    }
                }

                pDicomValue = pOtherByteString;
                break;
            }
        case ATTR_VR_OF:
            {
                DvtkData::Dimse::OtherFloatString *pOtherFloatString = new DvtkData::Dimse::OtherFloatString();

                // convert OF value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        OTHER_VALUE_CLASS *pUMOtherValue = (OTHER_VALUE_CLASS*)pUMValueList->GetValue(0);

                        // check if data in a file
                        string filename;
                        if (pUMOtherValue->Get(filename, true) == MSG_OK)
                        {
                            // set the filename
                            pOtherFloatString->FileName = filename.c_str();
                        }
                        else
                        {
                            // get pattern values
                            // 0 = rows
                            // 1 = columns
                            // 2 = start_value
                            // 3 = rows_increment
                            // 4 = columns_increment
                            // 5 = rows_same
                            // 6 = columns_same
                            DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = new DvtkData::Dimse::BitmapPatternParameters();

                            for (int i = 0; i < 7; i++)
                            {
                                UINT32 value;
                                pUMOtherValue->Get((UINT32) i, value);
                                switch(i)
                                {
                                case 0: pBitmapPattern->NumberOfRows = (System::UInt16) value; break;
                                case 1: pBitmapPattern->NumberOfColumns = (System::UInt16) value; break;
                                case 2: pBitmapPattern->StartValue = (System::UInt16) value; break;
                                case 3: pBitmapPattern->ValueIncrementPerRowBlock = (System::UInt16) value; break;
                                case 4: pBitmapPattern->ValueIncrementPerColumnBlock = (System::UInt16) value; break;
                                case 5: pBitmapPattern->NumberOfIdenticalValueRows = (System::UInt16) value; break;
                                case 6: pBitmapPattern->NumberOfIdenticalValueColumns = (System::UInt16) value; break;
                                default: throw new System::ApplicationException(); break;
                                }
                            }
                            pOtherFloatString->BitmapPattern = pBitmapPattern;
                        }

						// data compressed
						pOtherFloatString->Compressed = pUMOtherValue->IsCompressed();
                    }
                }

                pDicomValue = pOtherFloatString;
                break;
            }
			case ATTR_VR_OL:
            {
                DvtkData::Dimse::OtherLongString *pOtherLongString = new DvtkData::Dimse::OtherLongString();

                // convert OF value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        OTHER_VALUE_CLASS *pUMOtherValue = (OTHER_VALUE_CLASS*)pUMValueList->GetValue(0);

                        // check if data in a file
                        string filename;
                        if (pUMOtherValue->Get(filename, true) == MSG_OK)
                        {
                            // set the filename
                            pOtherLongString->FileName = filename.c_str();
                        }
                        else
                        {
                            // get pattern values
                            // 0 = rows
                            // 1 = columns
                            // 2 = start_value
                            // 3 = rows_increment
                            // 4 = columns_increment
                            // 5 = rows_same
                            // 6 = columns_same
                            DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = new DvtkData::Dimse::BitmapPatternParameters();

                            for (int i = 0; i < 7; i++)
                            {
                                UINT32 value;
                                pUMOtherValue->Get((UINT32) i, value);
                                switch(i)
                                {
                                case 0: pBitmapPattern->NumberOfRows = (System::UInt16) value; break;
                                case 1: pBitmapPattern->NumberOfColumns = (System::UInt16) value; break;
                                case 2: pBitmapPattern->StartValue = (System::UInt16) value; break;
                                case 3: pBitmapPattern->ValueIncrementPerRowBlock = (System::UInt16) value; break;
                                case 4: pBitmapPattern->ValueIncrementPerColumnBlock = (System::UInt16) value; break;
                                case 5: pBitmapPattern->NumberOfIdenticalValueRows = (System::UInt16) value; break;
                                case 6: pBitmapPattern->NumberOfIdenticalValueColumns = (System::UInt16) value; break;
                                default: throw new System::ApplicationException(); break;
                                }
                            }
                            pOtherLongString->BitmapPattern = pBitmapPattern;
                        }

						// data compressed
						pOtherLongString->Compressed = pUMOtherValue->IsCompressed();
                    }
                }

                pDicomValue = pOtherLongString;
                break;
            }
			case ATTR_VR_OD:
            {
                DvtkData::Dimse::OtherDoubleString *pOtherDoubleString = new DvtkData::Dimse::OtherDoubleString();

                // convert OF value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        OTHER_VALUE_CLASS *pUMOtherValue = (OTHER_VALUE_CLASS*)pUMValueList->GetValue(0);

                        // check if data in a file
                        string filename;
                        if (pUMOtherValue->Get(filename, true) == MSG_OK)
                        {
                            // set the filename
                            pOtherDoubleString->FileName = filename.c_str();
                        }
                        else
                        {
                            // get pattern values
                            // 0 = rows
                            // 1 = columns
                            // 2 = start_value
                            // 3 = rows_increment
                            // 4 = columns_increment
                            // 5 = rows_same
                            // 6 = columns_same
                            DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = new DvtkData::Dimse::BitmapPatternParameters();

                            for (int i = 0; i < 7; i++)
                            {
                                UINT32 value;
                                pUMOtherValue->Get((UINT32) i, value);
                                switch(i)
                                {
                                case 0: pBitmapPattern->NumberOfRows = (System::UInt16) value; break;
                                case 1: pBitmapPattern->NumberOfColumns = (System::UInt16) value; break;
                                case 2: pBitmapPattern->StartValue = (System::UInt16) value; break;
                                case 3: pBitmapPattern->ValueIncrementPerRowBlock = (System::UInt16) value; break;
                                case 4: pBitmapPattern->ValueIncrementPerColumnBlock = (System::UInt16) value; break;
                                case 5: pBitmapPattern->NumberOfIdenticalValueRows = (System::UInt16) value; break;
                                case 6: pBitmapPattern->NumberOfIdenticalValueColumns = (System::UInt16) value; break;
                                default: throw new System::ApplicationException(); break;
                                }
                            }
                            pOtherDoubleString->BitmapPattern = pBitmapPattern;
                        }

						// data compressed
						pOtherDoubleString->Compressed = pUMOtherValue->IsCompressed();
                    }
                }

                pDicomValue = pOtherDoubleString;
                break;
            }
			case ATTR_VR_OV:
            {
                DvtkData::Dimse::OtherVeryLongString *pOtherVeryLongString = new DvtkData::Dimse::OtherVeryLongString();

                // convert OV value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        OTHER_VALUE_CLASS *pUMOtherValue = (OTHER_VALUE_CLASS*)pUMValueList->GetValue(0);

                        // check if data in a file
                        string filename;
                        if (pUMOtherValue->Get(filename, true) == MSG_OK)
                        {
                            // set the filename
                            pOtherVeryLongString->FileName = filename.c_str();
                        }
                        else
                        {
                            // get pattern values
                            // 0 = rows
                            // 1 = columns
                            // 2 = start_value
                            // 3 = rows_increment
                            // 4 = columns_increment
                            // 5 = rows_same
                            // 6 = columns_same
                            DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = new DvtkData::Dimse::BitmapPatternParameters();

                            for (int i = 0; i < 7; i++)
                            {
                                UINT32 value;
                                pUMOtherValue->Get((UINT32) i, value);
                                switch(i)
                                {
                                case 0: pBitmapPattern->NumberOfRows = (System::UInt16) value; break;
                                case 1: pBitmapPattern->NumberOfColumns = (System::UInt16) value; break;
                                case 2: pBitmapPattern->StartValue = (System::UInt16) value; break;
                                case 3: pBitmapPattern->ValueIncrementPerRowBlock = (System::UInt16) value; break;
                                case 4: pBitmapPattern->ValueIncrementPerColumnBlock = (System::UInt16) value; break;
                                case 5: pBitmapPattern->NumberOfIdenticalValueRows = (System::UInt16) value; break;
                                case 6: pBitmapPattern->NumberOfIdenticalValueColumns = (System::UInt16) value; break;
                                default: throw new System::ApplicationException(); break;
                                }
                            }
                            pOtherVeryLongString->BitmapPattern = pBitmapPattern;
                        }

						// data compressed
						pOtherVeryLongString->Compressed = pUMOtherValue->IsCompressed();
                    }
                }

                pDicomValue = pOtherVeryLongString;
                break;
            }
        case ATTR_VR_OW:
            {
                DvtkData::Dimse::OtherWordString *pOtherWordString = new DvtkData::Dimse::OtherWordString();

                // convert all OW value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        OTHER_VALUE_CLASS *pUMOtherValue = (OTHER_VALUE_CLASS*) pUMValueList->GetValue(0);

                        // check if data in a file
                        string filename;
                        if (pUMOtherValue->Get(filename, true) == MSG_OK)
                        {
                            // set the filename
                            pOtherWordString->FileName = filename.c_str();
                        }
                        else
                        {
                            // get pattern values
                            // 0 = rows
                            // 1 = columns
                            // 2 = start_value
                            // 3 = rows_increment
                            // 4 = columns_increment
                            // 5 = rows_same
                            // 6 = columns_same
                            DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = new DvtkData::Dimse::BitmapPatternParameters();

                            for (int i = 0; i < 7; i++)
                            {
                                UINT32 value;
                                pUMOtherValue->Get((UINT32) i, value);
                                switch(i)
                                {
                                case 0: pBitmapPattern->NumberOfRows = (System::UInt16) value; break;
                                case 1: pBitmapPattern->NumberOfColumns = (System::UInt16) value; break;
                                case 2: pBitmapPattern->StartValue = (System::UInt16) value; break;
                                case 3: pBitmapPattern->ValueIncrementPerRowBlock = (System::UInt16) value; break;
                                case 4: pBitmapPattern->ValueIncrementPerColumnBlock = (System::UInt16) value; break;
                                case 5: pBitmapPattern->NumberOfIdenticalValueRows = (System::UInt16) value; break;
                                case 6: pBitmapPattern->NumberOfIdenticalValueColumns = (System::UInt16) value; break;
                                default: throw new System::ApplicationException(); break;
                                }
                            }
                            pOtherWordString->BitmapPattern = pBitmapPattern;
                        }

						// data compressed
						pOtherWordString->Compressed = pUMOtherValue->IsCompressed();
                    }
                }

                pDicomValue = pOtherWordString;
                break;
            }
        case ATTR_VR_PN:
            {
                DvtkData::Dimse::PersonName *pPersonName = new DvtkData::Dimse::PersonName();
                pPersonName->Values = new DvtkData::Collections::StringCollection();

                // convert all PN values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pPersonName->Values->Add(pString);
                    }
                }
                pDicomValue = pPersonName;
                break;
            }
        case ATTR_VR_SH:
            {
                DvtkData::Dimse::ShortString *pShortString = new DvtkData::Dimse::ShortString();
                pShortString->Values = new DvtkData::Collections::StringCollection();

                // convert all SH values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pShortString->Values->Add(pString);
                    }
                }
                pDicomValue = pShortString;
                break;
            }
        case ATTR_VR_SL:
            {
                DvtkData::Dimse::SignedLong *pSignedLong = new DvtkData::Dimse::SignedLong();
                pSignedLong->Values = new DvtkData::Collections::Int32Collection();

                // convert all SL values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::Int32 value = ConvertSL(pUMValueList->GetValue(i));
                        pSignedLong->Values->Add(value);
                    }
                }
                pDicomValue = pSignedLong;
                break;
            }
        case ATTR_VR_SQ:
            {
                DvtkData::Dimse::SequenceOfItems *pSequenceOfItems = new DvtkData::Dimse::SequenceOfItems();

                // convert all SQ values
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        DCM_VALUE_SQ_CLASS *pUMSQValue = (DCM_VALUE_SQ_CLASS*)pUMValueList->GetValue(0);

                        pSequenceOfItems->Sequence = new DvtkData::Dimse::Sequence();

						// re-set the values here
						pSequenceOfItems->Sequence->DefinedLength = pUMSQValue->isDefinedLength();
						pSequenceOfItems->Sequence->Length = length;
						pSequenceOfItems->Sequence->DelimiterGroup = pUMSQValue->getDelimiterGroup();
						pSequenceOfItems->Sequence->DelimiterElement = pUMSQValue->getDelimiterElement();
						pSequenceOfItems->Sequence->DelimiterLength = pUMSQValue->getDelimiterLength();

                        // convert all items in sequence
                        for (int i = 0; i < pUMSQValue->GetNrItems(); i++)
                        {
                            DCM_ITEM_CLASS *pUMItem = pUMSQValue->getItem(i);
                            if (pUMItem)
                            {
                                DvtkData::Dimse::SequenceItem *pItem = Convert(pUMItem);
                                pSequenceOfItems->Sequence->Add(pItem);
                            }
                        }
                    }
                }
				else if (length == 0)
				{
					pSequenceOfItems->Sequence->DefinedLength = true;
					pSequenceOfItems->Sequence->Length = length;
				}

                pDicomValue = pSequenceOfItems;
                break;
            }
        case ATTR_VR_SS:
            {
                DvtkData::Dimse::SignedShort *pSignedShort = new DvtkData::Dimse::SignedShort();
                pSignedShort->Values = new DvtkData::Collections::Int16Collection();

                // convert all SS values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::Int16 value = ConvertSS(pUMValueList->GetValue(i));
                        pSignedShort->Values->Add(value);
                    }
                }
                pDicomValue = pSignedShort;
                break;
            }
        case ATTR_VR_ST:
            {
                DvtkData::Dimse::ShortText *pShortText = new DvtkData::Dimse::ShortText();

                // convert ST value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        System::String *pStringValue = ConvertLongString(pUMValueList->GetValue(0));
                        pShortText->Value = pStringValue;
                    }
                }
                pDicomValue = pShortText;
                break;
            }
        case ATTR_VR_TM:
            {
                DvtkData::Dimse::Time *pTime = new DvtkData::Dimse::Time();
                pTime->Values = new DvtkData::Collections::StringCollection();

                // convert all TM values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pTime->Values->Add(pString);
                    }
                }
                pDicomValue = pTime;
                break;
            }
        case ATTR_VR_UI:
            {
                DvtkData::Dimse::UniqueIdentifier *pUniqueIdentifier = new DvtkData::Dimse::UniqueIdentifier();
                pUniqueIdentifier->Values = new DvtkData::Collections::StringCollection();

                // convert all UI values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pUniqueIdentifier->Values->Add(pString);
                    }
                }
                pDicomValue = pUniqueIdentifier;
                break;
            }
		case ATTR_VR_UC:
            {
                DvtkData::Dimse::UnlimitedCharacters *pUnlimitedCharacters = new DvtkData::Dimse::UnlimitedCharacters();
                pUnlimitedCharacters->Values = new DvtkData::Collections::StringCollection();

                // convert all UI values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::String *pString = ConvertString(pUMValueList->GetValue(i));
                        pUnlimitedCharacters->Values->Add(pString);
                    }
                }
                pDicomValue = pUnlimitedCharacters;
                break;
            }
        case ATTR_VR_UL:
            {
                DvtkData::Dimse::UnsignedLong *pUnsignedLong = new DvtkData::Dimse::UnsignedLong();
                pUnsignedLong->Values = new DvtkData::Collections::UInt32Collection();

                // convert all UL values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::UInt32 value = ConvertUL(pUMValueList->GetValue(i));
                        pUnsignedLong->Values->Add(value);
                    }
                }
                pDicomValue = pUnsignedLong;
                break;
            }
        case ATTR_VR_UN:
            {
                DvtkData::Dimse::Unknown *pUnknown = new DvtkData::Dimse::Unknown();
                pUnknown->ByteArray = NULL;

                // convert all UN values
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() != 1)
                    {
                        throw new System::ApplicationException(
                            "An VR of UN may only contain one value item."
                            );
                    }
                    BASE_VALUE_CLASS *pBaseValue = pUMValueList->GetValue(0);

                    if (pBaseValue)
                    {
                        unsigned char* pValue;
                        UINT32 length;

                        // copy byte by byte
                        if (pBaseValue->Get((unsigned char**)&pValue, length) != MSG_OK)
                        {
                            throw new System::ApplicationException(
                                "Could not get VR UN value item");
                        }
                        System::Byte byteArray __gc[] = 
                            new System::Byte __gc[length/* - 1*/];
                        Marshal::Copy(pValue, byteArray, 0, byteArray->Length);
                        /*
                        for (UINT32 i = 0; i < length; i++)
                        {
                        System::Byte value = (System::Byte) *(pValue+i);
                        byteArray[i] = value;
                        }
                        */
                        pUnknown->ByteArray = byteArray;
                    }
                }
                pDicomValue = pUnknown;
                break;
            }
        case ATTR_VR_US:
            {
                DvtkData::Dimse::UnsignedShort *pUnsignedShort = new DvtkData::Dimse::UnsignedShort();
                pUnsignedShort->Values = new DvtkData::Collections::UInt16Collection();

                // convert all US values
                if (pUMValueList)
                {
                    for (int i = 0; i < pUMValueList->GetNrValues(); i++)
                    {
                        System::UInt16 value = ConvertUS(pUMValueList->GetValue(i));
                        pUnsignedShort->Values->Add(value);
                    }
                }
                pDicomValue = pUnsignedShort;
                break;
            }
        case ATTR_VR_UT:
            {
                DvtkData::Dimse::UnlimitedText *pUnlimitedText = new DvtkData::Dimse::UnlimitedText();

                // convert all UT value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        System::String *pStringValue = ConvertLongString(pUMValueList->GetValue(0));
                        pUnlimitedText->Value = pStringValue;
                    }
                }
                pDicomValue = pUnlimitedText;
                break;
            }
		case ATTR_VR_UR:
            {
                DvtkData::Dimse::UniversalResourceIdentifier *pUniversalResourceIdentifier = new DvtkData::Dimse::UniversalResourceIdentifier();

                // convert all UC value
                if (pUMValueList)
                {
                    if (pUMValueList->GetNrValues() == 1)
                    {
                        System::String *pStringValue = ConvertLongString(pUMValueList->GetValue(0));
                        pUniversalResourceIdentifier->Value = pStringValue;
                    }
                }
                pDicomValue = pUniversalResourceIdentifier;
                break;
            }
        default:
            pDicomValue = NULL;
            break;
        }

        return pDicomValue;
    }

    //>>===========================================================================

    System::String __gc* 
        ManagedUnManagedDimseConvertor::ConvertString(BASE_VALUE_CLASS *pUMString)

        //  DESCRIPTION     : Convert unmanaged to managed - string based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      :
        //  NOTES           :
        //<<===========================================================================
    {
        System::String *pString = NULL;
        string value;

        if ((pUMString) &&
            (pUMString->Get(value) == MSG_OK))
        {
            // implicit marshalling by compiler
            pString = new System::String(value.c_str());
        }

        return pString;
    }

    //>>===========================================================================

    DvtkData::Dimse::Tag __gc* 
        ManagedUnManagedDimseConvertor::ConvertAT(BASE_VALUE_CLASS* pUMTag)

        //  DESCRIPTION     : Convert unmanaged to managed - AT based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      :
        //  NOTES           :
        //<<===========================================================================
    {
        DvtkData::Dimse::Tag *pTag = NULL;
        System::UInt32 value;

        if ((pUMTag) &&
            (pUMTag->Get(value) == MSG_OK))
        {
            // create new tag
            pTag = new DvtkData::Dimse::Tag();
            pTag->GroupNumber = (System::UInt16) (value >> 16);
            pTag->ElementNumber = (System::UInt16) value;
        }

        return pTag;
    }

    //>>===========================================================================

    System::Double //__value      MIGRATION_IN_PROGRESS 
        ManagedUnManagedDimseConvertor::ConvertFD(BASE_VALUE_CLASS* pUMDouble)

        //  DESCRIPTION     : Convert unmanaged to managed - FD based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : Application exception thrown when accessor fails.
        //  NOTES           :
        //<<===========================================================================
    {
        System::Double value = 0;

        if ((pUMDouble) &&
            (pUMDouble->Get(value) != MSG_OK))
        {
            throw new System::ApplicationException();
        }

        return value;
    }

    //>>===========================================================================

    System::Single //__value   MIGRATION_IN_PROGRESS 
        ManagedUnManagedDimseConvertor::ConvertFL(BASE_VALUE_CLASS* pUMSingle)

        //  DESCRIPTION     : Convert unmanaged to managed - FL based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : Application exception thrown when accessor fails.
        //  NOTES           :
        //<<===========================================================================
    {
        System::Single value = 0;

        if ((pUMSingle) &&
            (pUMSingle->Get(value) != MSG_OK))
        {
            throw new System::ApplicationException();
        }

        return value;
    }

    //>>===========================================================================

    System::Int32 //__value         MIGRATION_IN_PROGRESS
        ManagedUnManagedDimseConvertor::ConvertSL(BASE_VALUE_CLASS* pUMSignedLong)

        //  DESCRIPTION     : Convert unmanaged to managed - SL based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : Application exception thrown when accessor fails.
        //  NOTES           :
        //<<===========================================================================
    {
        System::Int32 value = 0;

        if ((pUMSignedLong) &&
            (pUMSignedLong->Get(value) != MSG_OK))
        {
            throw new System::ApplicationException();
        }

        return value;
    }

    //>>===========================================================================

    System::Int16 //__value   MIGRATION_IN_PROGRESS 
        ManagedUnManagedDimseConvertor::ConvertSS(BASE_VALUE_CLASS* pUMSignedShort)

        //  DESCRIPTION     : Convert unmanaged to managed - SS based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : Application exception thrown when accessor fails.
        //  NOTES           :
        //<<===========================================================================
    {
        System::Int16 value = 0;

        if ((pUMSignedShort) &&
            (pUMSignedShort->Get(value) != MSG_OK))
        {
            throw new System::ApplicationException();
        }

        return value;
    }

    //>>===========================================================================

    System::UInt32 //__value           MIGRATION_IN_PROGRESS 
        ManagedUnManagedDimseConvertor::ConvertUL(BASE_VALUE_CLASS* pUMUnsignedLong)

        //  DESCRIPTION     : Convert unmanaged to managed - UL based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : Application exception thrown when accessor fails. 
        //  NOTES           :
        //<<===========================================================================
    {
        System::UInt32 value = 0;

        if ((pUMUnsignedLong) &&
            (pUMUnsignedLong->Get(value) != MSG_OK))
        {
            throw new System::ApplicationException();
        }

        return value;
    }

    //>>===========================================================================

    System::UInt16 //__value    MIGRATION_IN_PROGRESS 
        ManagedUnManagedDimseConvertor::ConvertUS(BASE_VALUE_CLASS* pUMUnsignedShort)

        //  DESCRIPTION     : Convert unmanaged to managed - US based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : Application exception thrown when accessor fails. 
        //  NOTES           :
        //<<===========================================================================
    {
        System::UInt16 value = 0;

        if ((pUMUnsignedShort) &&
            (pUMUnsignedShort->Get(value) != MSG_OK))
        {
            throw new System::ApplicationException();
        }

        return value;
    }

    //>>===========================================================================

    System::String __gc* 
        ManagedUnManagedDimseConvertor::ConvertLongString(BASE_VALUE_CLASS *pUMString)

        //  DESCRIPTION     : Convert unmanaged to managed - long string based value
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        System::String *pString = NULL;
        char* pValue;
        UINT32 length;

        if ((pUMString) &&
            (pUMString->Get((unsigned char**)&pValue, length) == MSG_OK))
        {
            char valueString[16];
            std::string value;
            for (UINT32 i = 0; i < length; i++)
            {
                valueString[0] = pValue[i];
                valueString[1] = 0x00;
                value += valueString;
            }

            // implicit marshalling by compiler
            pString = new System::String(value.c_str());
        }

        return pString;
    }

    //>>===========================================================================

    System::String __gc* 
        ManagedUnManagedDimseConvertor::GetAttributeName(DvtkData::Dimse::Tag __gc* pTag)

    //  DESCRIPTION     : Get the Attribute Name from the Tag.
    //  PRECONDITIONS   :
    //  POSTCONDITIONS  :
    //  EXCEPTIONS      : 
    //  NOTES           :
    //<<===========================================================================
    {
        System::String *pString = NULL;
		UINT group = (UINT)pTag->get_GroupNumber();
		UINT element = (UINT)pTag->get_ElementNumber();

        // get name
        string attributeName = DEFINITION->GetAttributeName(group, element).c_str();
		if (attributeName.length())
		{
			// implicit marshalling by compiler
			pString = new System::String(attributeName.c_str());
		}

        return pString;
     }

    //>>===========================================================================

	DvtkData::Dimse::VR
			ManagedUnManagedDimseConvertor::GetAttributeVr(DvtkData::Dimse::Tag __gc* pTag)

    //  DESCRIPTION     : Get the Attribute VR from the Tag.
    //  PRECONDITIONS   :
    //  POSTCONDITIONS  :
    //  EXCEPTIONS      : 
    //  NOTES           :
    //<<===========================================================================
    {
		UINT group = (UINT)pTag->get_GroupNumber();
		UINT element = (UINT)pTag->get_ElementNumber();

        // get vr
        ATTR_VR_ENUM vr = DEFINITION->GetAttributeVr(group, element);

		return Convert(vr);
     }

    //=============================================================================
    //
    // Managed to Unmanaged
    //
    //=============================================================================

    //>>===========================================================================

    DIMSE_CMD_ENUM
        ManagedUnManagedDimseConvertor::Convert(DvtkData::Dimse::DimseCommand command)
    {
        switch (command)
        {
        case DvtkData::Dimse::DimseCommand::CCANCELRQ : return ::DIMSE_CMD_CCANCEL_RQ;
        case DvtkData::Dimse::DimseCommand::CECHORQ : return ::DIMSE_CMD_CECHO_RQ;
        case DvtkData::Dimse::DimseCommand::CECHORSP : return ::DIMSE_CMD_CECHO_RSP;
        case DvtkData::Dimse::DimseCommand::CFINDRQ : return ::DIMSE_CMD_CFIND_RQ;
        case DvtkData::Dimse::DimseCommand::CFINDRSP : return ::DIMSE_CMD_CFIND_RSP;
        case DvtkData::Dimse::DimseCommand::CGETRQ : return ::DIMSE_CMD_CGET_RQ;
        case DvtkData::Dimse::DimseCommand::CGETRSP : return ::DIMSE_CMD_CGET_RSP;
        case DvtkData::Dimse::DimseCommand::CMOVERQ : return ::DIMSE_CMD_CMOVE_RQ;
        case DvtkData::Dimse::DimseCommand::CMOVERSP : return ::DIMSE_CMD_CMOVE_RSP;
        case DvtkData::Dimse::DimseCommand::CSTORERQ : return ::DIMSE_CMD_CSTORE_RQ;
        case DvtkData::Dimse::DimseCommand::CSTORERSP : return ::DIMSE_CMD_CSTORE_RSP;
        case DvtkData::Dimse::DimseCommand::NACTIONRQ : return ::DIMSE_CMD_NACTION_RQ;
        case DvtkData::Dimse::DimseCommand::NACTIONRSP : return ::DIMSE_CMD_NACTION_RSP;
        case DvtkData::Dimse::DimseCommand::NCREATERQ : return ::DIMSE_CMD_NCREATE_RQ;
        case DvtkData::Dimse::DimseCommand::NCREATERSP : return ::DIMSE_CMD_NCREATE_RSP;
        case DvtkData::Dimse::DimseCommand::NDELETERQ : return ::DIMSE_CMD_NDELETE_RQ;
        case DvtkData::Dimse::DimseCommand::NDELETERSP : return ::DIMSE_CMD_NDELETE_RSP;
        case DvtkData::Dimse::DimseCommand::NEVENTREPORTRQ : return ::DIMSE_CMD_NEVENTREPORT_RQ;
        case DvtkData::Dimse::DimseCommand::NEVENTREPORTRSP : return ::DIMSE_CMD_NEVENTREPORT_RSP;
        case DvtkData::Dimse::DimseCommand::NGETRQ : return ::DIMSE_CMD_NGET_RQ;
        case DvtkData::Dimse::DimseCommand::NGETRSP : return ::DIMSE_CMD_NGET_RSP;
        case DvtkData::Dimse::DimseCommand::NSETRQ : return ::DIMSE_CMD_NSET_RQ;
        case DvtkData::Dimse::DimseCommand::NSETRSP : return ::DIMSE_CMD_NSET_RSP;
        case DvtkData::Dimse::DimseCommand::UNDEFINED : return ::DIMSE_CMD_UNKNOWN;
        default:
            {
                System::Diagnostics::Trace::Assert(false);
                return ::DIMSE_CMD_UNKNOWN;
            }
        }
    }

    DCM_COMMAND_CLASS* 
        ManagedUnManagedDimseConvertor::Convert(DvtkData::Dimse::CommandSet __gc *pCommand)

        //  DESCRIPTION     : Convert managed to unmanaged - DIMSE command
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pCommand == NULL) return NULL;

        DCM_COMMAND_CLASS *pUMCommand = new DCM_COMMAND_CLASS();

        // convert command attributes
        Convert(pUMCommand, pCommand);
        //
        // Set CommandId
        //
        DIMSE_CMD_ENUM commandId = Convert(pCommand->CommandField);
        pUMCommand->setCommandId(commandId);

        return pUMCommand;
    }

    //>>===========================================================================

    DCM_DATASET_CLASS* 
        ManagedUnManagedDimseConvertor::Convert(DvtkData::Dimse::DataSet __gc *pDataset)

        //  DESCRIPTION     : Convert managed to unmanaged - DIMSE dataset
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pDataset == NULL) return NULL;

        DCM_DATASET_CLASS *pUMDataset = new DCM_DATASET_CLASS();
        if (pDataset->IodId != NULL)
        {
            char* pIodIdAnsiString = 
                (char*)(void*)Marshal::StringToHGlobalAnsi(pDataset->IodId);
            pUMDataset->setIodName(pIodIdAnsiString);
            Marshal::FreeHGlobal(pIodIdAnsiString);
        }

        // convert dataset attributes
        Convert(pUMDataset, pDataset);

        return pUMDataset;
    }

    //>>===========================================================================

    DCM_ITEM_CLASS* 
        ManagedUnManagedDimseConvertor::Convert(DvtkData::Dimse::SequenceItem __gc *pItem)

        //  DESCRIPTION     : Convert managed to unmanaged - DIMSE item
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pItem == NULL) return NULL;

        DCM_ITEM_CLASS *pUMItem = new DCM_ITEM_CLASS();

        // convert item attributes
        Convert(pUMItem, pItem);

        return pUMItem;
    }

    //>>===========================================================================

    void
        ManagedUnManagedDimseConvertor::Convert(
        /*dst*/ DCM_ATTRIBUTE_GROUP_CLASS *pUMAttributeGroup, 
        /*src*/ DvtkData::Dimse::AttributeSet __gc *pAttributeSet)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute set to attribute group
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if ((pUMAttributeGroup == NULL) ||
            (pAttributeSet == NULL)) return;

        // convert all attributes in attribute set
        for (int i = 0; i < pAttributeSet->Count; i++)
        {
            // convert each attribute
            DCM_ATTRIBUTE_CLASS *pUMAttribute = Convert(pAttributeSet->Item[i]);
            pUMAttributeGroup->addAttribute(pUMAttribute);
        }
    }

    //>>===========================================================================

    DCM_ATTRIBUTE_CLASS* 
        ManagedUnManagedDimseConvertor::Convert(DvtkData::Dimse::Attribute __gc *pAttribute)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pAttribute == NULL) return NULL;

        DCM_ATTRIBUTE_CLASS *pUMAttribute = new DCM_ATTRIBUTE_CLASS();

        // convert attribute
        pUMAttribute->SetGroup(pAttribute->Tag->GroupNumber);
        pUMAttribute->SetElement(pAttribute->Tag->ElementNumber);
        pUMAttribute->SetMappedGroup(pAttribute->Tag->GroupNumber);
        pUMAttribute->SetMappedElement(pAttribute->Tag->ElementNumber);

        if (pAttribute->DicomValue)
        {
            // convert attribute values
            Convert(pUMAttribute, pAttribute);
        }

        return pUMAttribute;
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Attribute __gc *pAttribute)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute values
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // switch based on the attribute VR
        switch (pAttribute->ValueRepresentation)
        {
        case VR::AE:
            // convert AE values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::ApplicationEntity __gc*>(pAttribute->DicomValue));
            break;
        case VR::AS:
            // convert AS values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::AgeString __gc*>(pAttribute->DicomValue));
            break;
        case VR::AT:
            // convert AT values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::AttributeTag __gc*>(pAttribute->DicomValue));
            break;
        case VR::CS:
            // convert CS values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::CodeString __gc*>(pAttribute->DicomValue));
            break;
        case VR::DA:
            // convert DA values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::Date __gc*>(pAttribute->DicomValue));
            break;
        case VR::DS:
            // convert DS values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::DecimalString __gc*>(pAttribute->DicomValue));
            break;
        case VR::DT:
            // convert DT values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::DateTime __gc*>(pAttribute->DicomValue));
            break;
        case VR::FD:
            // convert FD values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::FloatingPointDouble __gc*>(pAttribute->DicomValue));
            break;
        case VR::FL:
            // convert FL values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::FloatingPointSingle __gc*>(pAttribute->DicomValue));
            break;
        case VR::IS:
            // convert IS values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::IntegerString __gc*>(pAttribute->DicomValue));
            break;
        case VR::LO:
            // convert LO values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::LongString __gc*>(pAttribute->DicomValue));
            break;
        case VR::LT:
            // convert LT value
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::LongText __gc*>(pAttribute->DicomValue));
            break;
        case VR::OB:
            // convert OB values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::OtherByteString __gc*>(pAttribute->DicomValue));
            break;
        case VR::OF:
            // convert OF values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::OtherFloatString __gc*>(pAttribute->DicomValue));
            break;
        case VR::OW:
            // convert OW values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::OtherWordString __gc*>(pAttribute->DicomValue));
            break;
		case VR::OL:
            // convert OL values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::OtherLongString __gc*>(pAttribute->DicomValue));
            break;
		case VR::OD:
            // convert OD values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::OtherDoubleString __gc*>(pAttribute->DicomValue));
            break;
        case VR::PN:
            // convert PN values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::PersonName __gc*>(pAttribute->DicomValue));
            break;
        case VR::SH:
            // convert SH values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::ShortString __gc*>(pAttribute->DicomValue));
            break;
        case VR::SL:
            // Convert SL values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::SignedLong __gc*>(pAttribute->DicomValue));
            break;
        case VR::SQ:
            // convert SQ value
            Convert(pUMAttribute, pAttribute->Length, static_cast<DvtkData::Dimse::SequenceOfItems __gc*>(pAttribute->DicomValue));
            break;
        case VR::SS:
            // convert SS value
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::SignedShort __gc*>(pAttribute->DicomValue));
            break;
        case VR::ST:
            // convert ST value
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::ShortText __gc*>(pAttribute->DicomValue));
            break;
        case VR::TM:
            // convert TM values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::Time __gc*>(pAttribute->DicomValue));
            break;
        case VR::UI:
            // convert UI values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::UniqueIdentifier __gc*>(pAttribute->DicomValue));
            break;
		case VR::UC:
            // convert UC values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::UnlimitedCharacters __gc*>(pAttribute->DicomValue));
            break;
        case VR::UL:
            // convert UL values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::UnsignedLong __gc*>(pAttribute->DicomValue));
            break;
        case VR::UN:
            // convert UN value
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::Unknown __gc*>(pAttribute->DicomValue));
            break;
        case VR::US:
            // convert US values
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::UnsignedShort __gc*>(pAttribute->DicomValue));
            break;
        case VR::UT:
            // convert UT value
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::UnlimitedText __gc*>(pAttribute->DicomValue));
            break;
		case VR::UR:
            // convert UT value
            Convert(pUMAttribute, static_cast<DvtkData::Dimse::UniversalResourceIdentifier __gc*>(pAttribute->DicomValue));
            break;
        default:
            break;
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ApplicationEntity __gc *pApplicationEntity)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value AE
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {	
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_AE;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pApplicationEntity)
        {
            for (int i = 0; i < pApplicationEntity->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pApplicationEntity->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::AgeString __gc *pAgeString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value AS
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_AS;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pAgeString)
        {
            for (int i = 0; i < pAgeString->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pAgeString->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::AttributeTag __gc *pAttributeTag)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute values AT
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_AT;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pAttributeTag)
        {
            for (int i = 0; i < pAttributeTag->Values->Count; i++)
            {
                // convert each value
                DvtkData::Dimse::Tag *pTag = pAttributeTag->Values->Item[i];
                UINT32 UMvalue = (pTag->GroupNumber << 16) + pTag->ElementNumber;
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::CodeString __gc *pCodeString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value CS
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_CS;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pCodeString)
        {
            for (int i = 0; i < pCodeString->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pCodeString->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Date __gc *pDate)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value DA
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_DA;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pDate)
        {
            for (int i = 0; i < pDate->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pDate->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::DecimalString __gc *pDecimalString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value DS
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_DS;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pDecimalString)
        {
            for (int i = 0; i < pDecimalString->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pDecimalString->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::DateTime __gc *pDateTime)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value DT
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_DT;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pDateTime)
        {
            for (int i = 0; i < pDateTime->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pDateTime->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::FloatingPointDouble __gc *pFloatingPointDouble)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value FD
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_FD;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pFloatingPointDouble)
        {
            for (int i = 0; i < pFloatingPointDouble->Values->Count; i++)
            {
                // convert each value
                double UMvalue = pFloatingPointDouble->Values->Item[i];
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::FloatingPointSingle __gc *pFloatingPointSingle)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value FL
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_FL;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pFloatingPointSingle)
        {
            for (int i = 0; i < pFloatingPointSingle->Values->Count; i++)
            {
                // convert each value
                float UMvalue = pFloatingPointSingle->Values->Item[i];
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::IntegerString __gc *pIntegerString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value IS
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_IS;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pIntegerString)
        {
            for (int i = 0; i < pIntegerString->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pIntegerString->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::LongString __gc *pLongString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value LO
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_LO;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pLongString)
        {
            for (int i = 0; i < pLongString->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pLongString->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::LongText __gc *pLongText)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value LT
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_LT;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if ((pLongText) &&
            (pLongText->Value))
        {
            // TODO: check this works for long strings
            string UMvalue;
            MarshalString(pLongText->Value, UMvalue);
            BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
            pUMValue->Set(UMvalue);
            pUMAttribute->AddValue(pUMValue);
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherByteString __gc *pOtherByteString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value OB
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : System application exception when value not defined.
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_OB;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pOtherByteString)
        {
            // data comes from a bitmap pattern
			if (pOtherByteString->Item != NULL)
			{
				if (pOtherByteString->Item->GetType() == __typeof(DvtkData::Dimse::BitmapPatternParameters))
				{
					DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = static_cast<DvtkData::Dimse::BitmapPatternParameters __gc *>(pOtherByteString->Item);
					OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
					pUMValue->Add(pBitmapPattern->NumberOfRows);
					pUMValue->Add(pBitmapPattern->NumberOfColumns);
					pUMValue->Add(pBitmapPattern->StartValue);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerRowBlock);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerColumnBlock);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueRows);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueColumns);
					pUMValue->SetCompressed(pOtherByteString->Compressed);
					pUMAttribute->AddValue(pUMValue);
				}
				else if (pOtherByteString->Item->GetType() == __typeof(System::String))
				{
					// data comes from a file
					System::String *pFileName = static_cast<System::String __gc *>(pOtherByteString->Item);
					string value;
					MarshalString(pFileName, value);
					if (value.length() != 0)
					{
						OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
						pUMValue->Set(value);
						pUMValue->SetCompressed(pOtherByteString->Compressed);
						pUMAttribute->AddValue(pUMValue);
					}
				}
				else
				{
					throw new System::ApplicationException();
				}
			}
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherFloatString __gc *pOtherFloatString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value OF
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : System application exception when value not defined. 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_OF;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pOtherFloatString)
        {
            // data comes from a bitmap pattern
			if(pOtherFloatString->Item != NULL)
			{
				if (pOtherFloatString->Item->GetType() == __typeof(DvtkData::Dimse::BitmapPatternParameters))
				{
					DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = static_cast<DvtkData::Dimse::BitmapPatternParameters __gc *>(pOtherFloatString->Item);
					OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
					pUMValue->Add(pBitmapPattern->NumberOfRows);
					pUMValue->Add(pBitmapPattern->NumberOfColumns);
					pUMValue->Add(pBitmapPattern->StartValue);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerRowBlock);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerColumnBlock);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueRows);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueColumns);
					pUMValue->SetCompressed(pOtherFloatString->Compressed);
					pUMAttribute->AddValue(pUMValue);			
				}
				else if (pOtherFloatString->Item->GetType() == __typeof(System::String))
				{
					// data comes from a file
					System::String *pFileName = static_cast<System::String __gc *>(pOtherFloatString->Item);
					string value;
					MarshalString(pFileName, value);
					if (value.length() != 0)
					{
						OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
						pUMValue->Set(value);
						pUMValue->SetCompressed(pOtherFloatString->Compressed);
						pUMAttribute->AddValue(pUMValue);
					}
				}
				else
				{
					throw new System::ApplicationException();
				}
			}
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherLongString __gc *pOtherLongString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value OL
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : System application exception when value not defined. 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_OL;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pOtherLongString)
        {
            // data comes from a bitmap pattern
			if(pOtherLongString->Item != NULL)
			{
				if (pOtherLongString->Item->GetType() == __typeof(DvtkData::Dimse::BitmapPatternParameters))
				{
					DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = static_cast<DvtkData::Dimse::BitmapPatternParameters __gc *>(pOtherLongString->Item);
					OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
					pUMValue->Add(pBitmapPattern->NumberOfRows);
					pUMValue->Add(pBitmapPattern->NumberOfColumns);
					pUMValue->Add(pBitmapPattern->StartValue);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerRowBlock);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerColumnBlock);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueRows);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueColumns);
					pUMValue->SetCompressed(pOtherLongString->Compressed);
					pUMAttribute->AddValue(pUMValue);			
				}
				else if (pOtherLongString->Item->GetType() == __typeof(System::String))
				{
					// data comes from a file
					System::String *pFileName = static_cast<System::String __gc *>(pOtherLongString->Item);
					string value;
					MarshalString(pFileName, value);
					if (value.length() != 0)
					{
						OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
						pUMValue->Set(value);
						pUMValue->SetCompressed(pOtherLongString->Compressed);
						pUMAttribute->AddValue(pUMValue);
					}
				}
				else
				{
					throw new System::ApplicationException();
				}
			}
        }
    }
	
    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherVeryLongString __gc *pOtherVeryLongString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value OV
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : System application exception when value not defined. 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_OV;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pOtherVeryLongString)
        {
            // data comes from a bitmap pattern
			if(pOtherVeryLongString->Item != NULL)
			{
				if (pOtherVeryLongString->Item->GetType() == __typeof(DvtkData::Dimse::BitmapPatternParameters))
				{
					DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = static_cast<DvtkData::Dimse::BitmapPatternParameters __gc *>(pOtherVeryLongString->Item);
					OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
					pUMValue->Add(pBitmapPattern->NumberOfRows);
					pUMValue->Add(pBitmapPattern->NumberOfColumns);
					pUMValue->Add(pBitmapPattern->StartValue);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerRowBlock);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerColumnBlock);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueRows);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueColumns);
					pUMValue->SetCompressed(pOtherVeryLongString->Compressed);
					pUMAttribute->AddValue(pUMValue);			
				}
				else if (pOtherVeryLongString->Item->GetType() == __typeof(System::String))
				{
					// data comes from a file
					System::String *pFileName = static_cast<System::String __gc *>(pOtherVeryLongString->Item);
					string value;
					MarshalString(pFileName, value);
					if (value.length() != 0)
					{
						OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
						pUMValue->Set(value);
						pUMValue->SetCompressed(pOtherVeryLongString->Compressed);
						pUMAttribute->AddValue(pUMValue);
					}
				}
				else
				{
					throw new System::ApplicationException();
				}
			}
        }
    }

    //>>===========================================================================

	void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherDoubleString __gc *pOtherDoubleString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value OD
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : System application exception when value not defined. 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_OD;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pOtherDoubleString)
        {
            // data comes from a bitmap pattern
			if(pOtherDoubleString->Item != NULL)
			{
				if (pOtherDoubleString->Item->GetType() == __typeof(DvtkData::Dimse::BitmapPatternParameters))
				{
					DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = static_cast<DvtkData::Dimse::BitmapPatternParameters __gc *>(pOtherDoubleString->Item);
					OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
					pUMValue->Add(pBitmapPattern->NumberOfRows);
					pUMValue->Add(pBitmapPattern->NumberOfColumns);
					pUMValue->Add(pBitmapPattern->StartValue);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerRowBlock);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerColumnBlock);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueRows);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueColumns);
					pUMValue->SetCompressed(pOtherDoubleString->Compressed);
					pUMAttribute->AddValue(pUMValue);			
				}
				else if (pOtherDoubleString->Item->GetType() == __typeof(System::String))
				{
					// data comes from a file
					System::String *pFileName = static_cast<System::String __gc *>(pOtherDoubleString->Item);
					string value;
					MarshalString(pFileName, value);
					if (value.length() != 0)
					{
						OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
						pUMValue->Set(value);
						pUMValue->SetCompressed(pOtherDoubleString->Compressed);
						pUMAttribute->AddValue(pUMValue);
					}
				}
				else
				{
					throw new System::ApplicationException();
				}
			}
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherWordString __gc *pOtherWordString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value OW
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : System application exception when value not defined. 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_OW;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pOtherWordString)
        {
            // data comes from a bitmap pattern
			if(pOtherWordString->Item != NULL)
			{
				if (pOtherWordString->Item->GetType() == __typeof(DvtkData::Dimse::BitmapPatternParameters))
				{
					DvtkData::Dimse::BitmapPatternParameters *pBitmapPattern = static_cast<DvtkData::Dimse::BitmapPatternParameters __gc *>(pOtherWordString->Item);
					OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
					pUMValue->Add(pBitmapPattern->NumberOfRows);
					pUMValue->Add(pBitmapPattern->NumberOfColumns);
					pUMValue->Add(pBitmapPattern->StartValue);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerRowBlock);
					pUMValue->Add(pBitmapPattern->ValueIncrementPerColumnBlock);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueRows);
					pUMValue->Add(pBitmapPattern->NumberOfIdenticalValueColumns);
					pUMValue->SetCompressed(pOtherWordString->Compressed);
					pUMAttribute->AddValue(pUMValue);
				}
				else if (pOtherWordString->Item->GetType() == __typeof(System::String))
				{
					// data comes from a file
					System::String *pFileName = static_cast<System::String __gc *>(pOtherWordString->Item);
					string value;
					MarshalString(pFileName, value);
					if (value.length() != 0)
					{
						OTHER_VALUE_CLASS *pUMValue = (OTHER_VALUE_CLASS*)CreateNewValue(UMvr);
						pUMValue->Set(value);
						pUMValue->SetCompressed(pOtherWordString->Compressed);
						pUMAttribute->AddValue(pUMValue);
					}
				}
				else
				{
					throw new System::ApplicationException();
				}
			}
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::PersonName __gc *pPersonName)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value PN
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_PN;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pPersonName)
        {
            for (int i = 0; i < pPersonName->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pPersonName->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ShortString __gc *pShortString)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value SH
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_SH;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pShortString)
        {
            for (int i = 0; i < pShortString->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pShortString->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::SignedLong __gc *pSignedLong)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value SL
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_SL;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pSignedLong)
        {
            for (int i = 0; i < pSignedLong->Values->Count; i++)
            {
                // convert each value
                INT32 UMvalue = pSignedLong->Values->Item[i];
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, UINT32 length, DvtkData::Dimse::SequenceOfItems __gc *pSequenceOfItems)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value SQ
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_SQ;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pSequenceOfItems)
        {
            UINT32 sqLength = length;
            if ((sqLength == 0) && 
                (pSequenceOfItems->Sequence->Count))
            {
                // if the length has not been explicitly set by the caller and there are items in the sequence
                // then set the sequence length to undefined
                sqLength = UNDEFINED_LENGTH;
            }
            DCM_VALUE_SQ_CLASS *pUMSequence = new DCM_VALUE_SQ_CLASS(sqLength);

            // convert each item
            for (int i = 0 ; i < pSequenceOfItems->Sequence->Count; i++)
            {
                DCM_ITEM_CLASS *pUMItem = Convert(pSequenceOfItems->Sequence->Item[i]);
                pUMSequence->addItem(pUMItem);
//                pUMSequence->Set(pUMItem);
            }

            pUMAttribute->addSqValue(pUMSequence);
//            pUMAttribute->AddValue(pUMSequence);
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::SignedShort __gc *pSignedShort)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value SS
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_SS;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pSignedShort)
        {
            for (int i = 0; i < pSignedShort->Values->Count; i++)
            {
                // convert each value
                INT16 UMvalue = pSignedShort->Values->Item[i];
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ShortText __gc *pShortText)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value ST
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_ST;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if ((pShortText) &&
            (pShortText->Value))
        {
            // TODO: check this works for long strings
            string UMvalue;
            MarshalString(pShortText->Value, UMvalue);
            BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
            pUMValue->Set(UMvalue);
            pUMAttribute->AddValue(pUMValue);
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Time __gc *pTime)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value TM
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_TM;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pTime)
        {
            for (int i = 0; i < pTime->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pTime->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UniqueIdentifier __gc *pUniqueIdentifier)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value UI
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_UI;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pUniqueIdentifier)
        {
            for (int i = 0; i < pUniqueIdentifier->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pUniqueIdentifier->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

	    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnlimitedCharacters __gc *pUnlimitedCharacters)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value UC
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_UC;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pUnlimitedCharacters)
        {
            for (int i = 0; i < pUnlimitedCharacters->Values->Count; i++)
            {
                // convert each value
                System::String *pString = pUnlimitedCharacters->Values->Item[i];
                string UMvalue;
                MarshalString(pString, UMvalue);
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnsignedLong __gc *pUnsignedLong)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value UL
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_UL;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pUnsignedLong)
        {
            for (int i = 0; i < pUnsignedLong->Values->Count; i++)
            {
                // convert each value

                UINT32 UMvalue = pUnsignedLong->Values->Item[i];
                DCM_VALUE_UL_CLASS *pUMValue = new DCM_VALUE_UL_CLASS();
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Unknown __gc *pUnknown)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value UN
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_UN;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pUnknown)
        {
            if (pUnknown->ByteArray != NULL)
            {
                System::Byte byteArray __gc[] = pUnknown->ByteArray;
                UINT32 length = byteArray->Length;
                unsigned char* pUMvalue = new unsigned char[length/* + 1*/];
                Marshal::Copy(byteArray, 0, pUMvalue, byteArray->Length);

                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                if (pUMValue->Set(pUMvalue, length) != ::MSG_OK)
                {
                    throw new System::ApplicationException();
                }
                pUMAttribute->AddValue(pUMValue);

//                delete [] pUMvalue;
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnsignedShort __gc *pUnsignedShort)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value US
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_US;
        pUMAttribute->SetVR(UMvr);

        // convert all values
        if (pUnsignedShort)
        {
            for (int i = 0; i < pUnsignedShort->Values->Count; i++)
            {
                // convert each value
                UINT16 UMvalue = pUnsignedShort->Values->Item[i];
                BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
                pUMValue->Set(UMvalue);
                pUMAttribute->AddValue(pUMValue);
            }
        }
    }

    //>>===========================================================================

    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnlimitedText __gc *pUnlimitedText)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value UT
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_UT;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pUnlimitedText->Value)
        {
            // TODO: check this works for long strings
            string UMvalue;
            MarshalString(pUnlimitedText->Value, UMvalue);
            BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
            pUMValue->Set(UMvalue);
            pUMAttribute->AddValue(pUMValue);
        }
    }

	    void 
        ManagedUnManagedDimseConvertor::Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UniversalResourceIdentifier __gc *pUniversalResourceIdentifier)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute value UR
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        // set VR
        ATTR_VR_ENUM UMvr = ATTR_VR_UR;
        pUMAttribute->SetVR(UMvr);

        // convert the value
        if (pUniversalResourceIdentifier->Value)
        {
            // TODO: check this works for long strings
            string UMvalue;
            MarshalString(pUniversalResourceIdentifier->Value, UMvalue);
            BASE_VALUE_CLASS *pUMValue = CreateNewValue(UMvr);
            pUMValue->Set(UMvalue);
            pUMAttribute->AddValue(pUMValue);
        }
    }
}