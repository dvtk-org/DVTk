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

namespace ManagedUnManagedDimseConvertors
{
    using namespace DvtkData::Dimse;

    //>>***************************************************************************

    class ManagedUnManagedDimseConvertor

        //  DESCRIPTION     : Managed Unmanaged DIMSE Convertor Class.
        //  INVARIANT       :
        //  NOTES           :
        //<<***************************************************************************
    {
    public:
        ManagedUnManagedDimseConvertor(void);
        ~ManagedUnManagedDimseConvertor(void);

        //
        // Unmanaged to Managed
        //
    public:
        static DvtkData::Dimse::VR
            Convert(ATTR_VR_ENUM vr);

    public:
        static DvtkData::Dimse::CommandSet __gc* 
            Convert(DCM_COMMAND_CLASS *pUMCommand);

	private:
        static DvtkData::Dimse::AttributeType
            Convert(::ATTR_TYPE_ENUM dataElementType);

    private:
        static DvtkData::Dimse::CommandSet __gc* 
            Convert(DIMSE_CMD_ENUM umCommandId);

    public:
        static DvtkData::Dimse::DataSet __gc* 
            Convert(DCM_DATASET_CLASS *pUMDataset);

    public:
        static DvtkData::Dimse::SequenceItem __gc* 
            Convert(DCM_ITEM_CLASS *pUMItem);

    public:
        static void
            Convert(
            /*dst*/ DvtkData::Dimse::AttributeSet __gc *pAttributeSet, 
            /*src*/ DCM_ATTRIBUTE_GROUP_CLASS *pUMAttributeGroup);

    public:
        static DvtkData::Dimse::Attribute __gc* 
            Convert(DCM_ATTRIBUTE_CLASS* pUMAttribute);

    private:
        static DvtkData::Dimse::Tag __gc* 
            Convert(System::UInt16 group, System::UInt16 element);

    private:
        static DvtkData::Dimse::DicomValueType __gc* 
            Convert(ATTR_VR_ENUM vr, UINT32, VALUE_LIST_CLASS* pUMValueList);

    private:
        static System::String __gc* 
            ConvertString(BASE_VALUE_CLASS* pUMString);

    private:
        static DvtkData::Dimse::Tag __gc* 
            ConvertAT(BASE_VALUE_CLASS* pUMTag);

    public:
        static System::Double //__value     MIGRATION_IN_PROGRESS 
            ConvertFD(BASE_VALUE_CLASS* pUMDouble);

    private:
        static System::Single //__value
            ConvertFL(BASE_VALUE_CLASS* pUMSingle);

    private:
        static System::Int32 //__value
            ConvertSL(BASE_VALUE_CLASS* pUMSignedLong);

    private:
        static System::Int16 //__value
            ConvertSS(BASE_VALUE_CLASS* pUMSignedShort);

    private:
        static System::UInt32 //__value
            ConvertUL(BASE_VALUE_CLASS* pUMUnsignedLong);

    private:
        static System::UInt16 //__value
            ConvertUS(BASE_VALUE_CLASS* pUMUnsignedShort);

    public:
        static System::String __gc* 
            ConvertLongString(BASE_VALUE_CLASS* pUMLTString);

	public:
		static System::String __gc* 
			GetAttributeName(DvtkData::Dimse::Tag __gc* pTag);

	public:
		static DvtkData::Dimse::VR
			GetAttributeVr(DvtkData::Dimse::Tag __gc* pTag);

		//
        // Managed to Unmanaged
        //
    public:
        static DIMSE_CMD_ENUM
            Convert(DvtkData::Dimse::DimseCommand command);

    public:
        static DCM_COMMAND_CLASS* 
            Convert(DvtkData::Dimse::CommandSet __gc *pCommand);

        static DCM_DATASET_CLASS* 
            Convert(DvtkData::Dimse::DataSet __gc *pDataset);

    private:
        static DCM_ITEM_CLASS* 
            Convert(DvtkData::Dimse::SequenceItem __gc *pItem);

    public:
        static void
            Convert(
            /*dst*/ DCM_ATTRIBUTE_GROUP_CLASS *pUMAttributeGroup, 
            /*src*/ DvtkData::Dimse::AttributeSet __gc *pAttributeSet);

    public:
        static DCM_ATTRIBUTE_CLASS* 
            Convert(DvtkData::Dimse::Attribute __gc *pAttribute);

    private:
        static void 
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Attribute __gc *pAttribute);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ApplicationEntity __gc *pApplicationEntity);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::AgeString __gc *pAgeString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::AttributeTag __gc *pAttributeTag);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::CodeString __gc *pCodeString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Date __gc *pDate);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::DecimalString __gc *pDecimalString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::DateTime __gc *pDateTime);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::FloatingPointDouble __gc *pFloatingPointDouble);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::FloatingPointSingle __gc *pFloatingPointSingle);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::IntegerString __gc *pIntegerString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::LongString __gc *pLongString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::LongText __gc *pLongText);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherByteString __gc *pOtherByteString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherFloatString __gc *pOtherFloatString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherWordString __gc *pOtherWordString);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherLongString __gc *pOtherLongString);
		
    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherVeryLongString __gc *pOtherVeryLongString);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherDoubleString __gc *pOtherDoubleString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::PersonName __gc *pPersonName);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ShortString __gc *pShortString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::SignedLong __gc *pSignedLong);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, UINT32 length, DvtkData::Dimse::SequenceOfItems __gc *pSequenceOfItems);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::SignedShort __gc *pSignedShort);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ShortText __gc *pShortText);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Time __gc *pTime);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UniqueIdentifier __gc *pUniqueIdentifier);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnlimitedCharacters __gc *pUnlimitedCharacters);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnsignedLong __gc *pUnsignedLong);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Unknown __gc *pUnknown);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnsignedShort __gc *pUnsignedShort);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnlimitedText __gc *pUnlimitedText);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UniversalResourceIdentifier __gc *pUniversalResourceIdentifier);
    };

}
