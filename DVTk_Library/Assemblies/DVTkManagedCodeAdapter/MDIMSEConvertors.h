// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
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
        static DvtkData::Dimse::CommandSet^ 
            Convert(DCM_COMMAND_CLASS *pUMCommand);

	private:
        static DvtkData::Dimse::AttributeType
            Convert(::ATTR_TYPE_ENUM dataElementType);

    private:
        static DvtkData::Dimse::CommandSet^ 
            Convert(DIMSE_CMD_ENUM umCommandId);

    public:
        static DvtkData::Dimse::DataSet^ 
            Convert(DCM_DATASET_CLASS *pUMDataset);

    public:
        static DvtkData::Dimse::SequenceItem^ 
            Convert(DCM_ITEM_CLASS *pUMItem);

    public:
        static void
            Convert(
            /*dst*/ DvtkData::Dimse::AttributeSet ^ pAttributeSet, 
            /*src*/ DCM_ATTRIBUTE_GROUP_CLASS *pUMAttributeGroup);

    public:
        static DvtkData::Dimse::Attribute^ 
            Convert(DCM_ATTRIBUTE_CLASS* pUMAttribute);

    private:
        static DvtkData::Dimse::Tag^ 
            Convert(System::UInt16 group, System::UInt16 element);

    private:
        static DvtkData::Dimse::DicomValueType^ 
            Convert(ATTR_VR_ENUM vr, UINT32, VALUE_LIST_CLASS* pUMValueList);

    private:
        static System::String^ 
            ConvertString(BASE_VALUE_CLASS* pUMString);

    private:
        static DvtkData::Dimse::Tag^ 
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
        static System::Int64 //__value
            ConvertSV(BASE_VALUE_CLASS* pUMSignedVeryLongString);

    private:
        static System::UInt32 //__value
            ConvertUL(BASE_VALUE_CLASS* pUMUnsignedLong);

    private:
        static System::UInt16 //__value
            ConvertUS(BASE_VALUE_CLASS* pUMUnsignedShort);

	private:
        static System::UInt64 //__value
            ConvertUV(BASE_VALUE_CLASS* pUMUnsignedVeryLongString);

    public:
        static System::String^ 
            ConvertLongString(BASE_VALUE_CLASS* pUMLTString);

	public:
		static System::String^ 
			GetAttributeName(DvtkData::Dimse::Tag^ pTag);

	public:
		static DvtkData::Dimse::VR
			GetAttributeVr(DvtkData::Dimse::Tag^ pTag);

		//
        // Managed to Unmanaged
        //
    public:
        static DIMSE_CMD_ENUM
            Convert(DvtkData::Dimse::DimseCommand command);

    public:
        static DCM_COMMAND_CLASS* 
            Convert(DvtkData::Dimse::CommandSet ^pCommand);

        static DCM_DATASET_CLASS* 
            Convert(DvtkData::Dimse::DataSet^ pDataset);

    private:
        static DCM_ITEM_CLASS* 
            Convert(DvtkData::Dimse::SequenceItem ^ pItem);

    public:
        static void
            Convert(
            /*dst*/ DCM_ATTRIBUTE_GROUP_CLASS *pUMAttributeGroup, 
            /*src*/ DvtkData::Dimse::AttributeSet^ pAttributeSet);

    public:
        static DCM_ATTRIBUTE_CLASS* 
            Convert(DvtkData::Dimse::Attribute^ pAttribute);

    private:
        static void 
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Attribute^ pAttribute);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ApplicationEntity^ pApplicationEntity);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::AgeString^ pAgeString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::AttributeTag^ pAttributeTag);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::CodeString^ pCodeString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Date ^ pDate);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::DecimalString ^ pDecimalString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::DateTime ^ pDateTime);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::FloatingPointDouble ^ pFloatingPointDouble);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::FloatingPointSingle ^ pFloatingPointSingle);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::IntegerString ^ pIntegerString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::LongString ^ pLongString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::LongText ^ pLongText);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherByteString ^ pOtherByteString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherFloatString ^ pOtherFloatString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherWordString ^ pOtherWordString);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherLongString ^ pOtherLongString);
		
    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherVeryLongString ^ pOtherVeryLongString);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::OtherDoubleString ^ pOtherDoubleString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::PersonName ^ pPersonName);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ShortString ^ pShortString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::SignedLong ^ pSignedLong);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, UINT32 length, DvtkData::Dimse::SequenceOfItems ^ pSequenceOfItems);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::SignedShort ^ pSignedShort);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::ShortText ^ pShortText);

	private:
		static void
			Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::SignedVeryLongString ^ pSignedVeryLongString);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Time ^ pTime);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UniqueIdentifier ^ pUniqueIdentifier);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnlimitedCharacters ^ pUnlimitedCharacters);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnsignedLong ^ pUnsignedLong);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::Unknown ^ pUnknown);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnsignedShort ^ pUnsignedShort);

    private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnlimitedText ^ pUnlimitedText);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UniversalResourceIdentifier ^ pUniversalResourceIdentifier);

	private:
        static void
            Convert(DCM_ATTRIBUTE_CLASS *pUMAttribute, DvtkData::Dimse::UnsignedVeryLongString ^ pUnsignedVeryLongString);
    };

}
