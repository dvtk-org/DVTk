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

namespace ManagedUnManagedMediaConvertors
{
    using namespace DvtkData::Media;

    //>>***************************************************************************

    class ManagedUnManagedMediaConvertor

        //  DESCRIPTION     : Managed Unmanaged Media Convertor Class.
        //  INVARIANT       :
        //  NOTES           :
        //<<***************************************************************************
    {
    public:
        ManagedUnManagedMediaConvertor(void);
        ~ManagedUnManagedMediaConvertor(void);

    //
    // Unmanaged to Managed
    //
    public:
        static 
            DvtkData::Media::DicomFile^ 
            Convert(::FILE_DATASET_CLASS *pFILE_DATASET_CLASS);

	public:
        static 
            DvtkData::Media::DicomDir^ 
            ConvertToDicomdir(::FILE_DATASET_CLASS *pFILE_DATASET_CLASS);

    private:
        static
            DvtkData::Media::FileHead^
            Convert(::MEDIA_FILE_HEADER_CLASS *pMEDIA_FILE_HEADER_CLASS);

    public:
        static
            void
            Convert(
            /*dst*/ DvtkData::Media::FileMetaInformation ^ pFileMetaInformation,
            /*src*/ ::MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS);

    //
    // Managed to Unmanaged
    //
    public:
        static 
            ::FILE_DATASET_CLASS* 
            Convert(DvtkData::Media::DicomFile^ pDicomFile, System::String^ pFileName);

	public:
        static 
            ::FILE_DATASET_CLASS* 
            Convert(DvtkData::Media::DicomDir^ pDicomFile, System::String^ pFileName);

	public:
        static
			::FILE_DATASET_CLASS* 
			Convert(DvtkData::Dimse::DataSet^ pDataset, System::String^ pFileName,
				System::String^ pTransferSyntax);

    private:
        static
            void
            Convert(
            /*dst*/ MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS, 
            /*src*/ DvtkData::Media::FileHead ^ pFileHead);

    private:
        static
            void
            Convert(
            /*dst*/ ::MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS, 
            /*src*/ DvtkData::Media::FileMetaInformation ^ pFileMetaInformation);
    };

}
