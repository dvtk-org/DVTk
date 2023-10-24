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
#include ".\MMediaConvertors.h"
#include ".\UtilityFunctions.h"
#include ".\MDimseConvertors.h"
#using <mscorlib.dll>

namespace ManagedUnManagedMediaConvertors
{
    using namespace DvtkData::Media;
    using namespace Wrappers;
    using namespace System::Runtime::InteropServices;
    using namespace ManagedUnManagedDimseConvertors;
    using namespace System;

    //>>===========================================================================

    ManagedUnManagedMediaConvertor::ManagedUnManagedMediaConvertor(void)

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

    ManagedUnManagedMediaConvertor::~ManagedUnManagedMediaConvertor(void)

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

    //>>===========================================================================

    DvtkData::Media::DicomFile^ 
        ManagedUnManagedMediaConvertor::Convert(::FILE_DATASET_CLASS *pFILE_DATASET_CLASS)

        //  DESCRIPTION     : Convert unmanaged to managed - DIMSE command
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pFILE_DATASET_CLASS == nullptr) return nullptr;

        DvtkData::Media::DicomFile ^ pDicomFile = gcnew DvtkData::Media::DicomFile();

        MEDIA_FILE_HEADER_CLASS* pFMI   = pFILE_DATASET_CLASS->getFileMetaInformation();
		if (pFMI != nullptr)
		{
			pDicomFile->FileHead = Convert(pFMI);
			pDicomFile->FileMetaInformation = gcnew DvtkData::Media::FileMetaInformation();
			Convert(pDicomFile->FileMetaInformation, pFMI);
		}

		DCM_DATASET_CLASS* pDataset = pFILE_DATASET_CLASS->getDataset();
		DCM_DIR_DATASET_CLASS* pDicomDirDataset = pFILE_DATASET_CLASS->getDicomdirDataset();
		if (pDataset != nullptr)
		{
			pDicomFile->DataSet = ManagedUnManagedDimseConvertor::Convert(pDataset);
		}
		else if (pDicomDirDataset != nullptr)
		{
			pDicomFile->DataSet = ManagedUnManagedDimseConvertor::Convert(pDicomDirDataset);
		}

        return pDicomFile;
    }

    //>>===========================================================================

	//>>===========================================================================

    DvtkData::Media::DicomDir^ 
        ManagedUnManagedMediaConvertor::ConvertToDicomdir(::FILE_DATASET_CLASS *pFILE_DATASET_CLASS)

        //  DESCRIPTION     : Convert unmanaged to managed - DIMSE command
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pFILE_DATASET_CLASS == nullptr) return nullptr;

        DvtkData::Media::DicomDir ^ pDicomFile = gcnew DvtkData::Media::DicomDir();

        MEDIA_FILE_HEADER_CLASS* pFMI   = pFILE_DATASET_CLASS->getFileMetaInformation();
		if (pFMI != nullptr)
		{
			pDicomFile->FileHead = Convert(pFMI);
			pDicomFile->FileMetaInformation = gcnew DvtkData::Media::FileMetaInformation();
			Convert(pDicomFile->FileMetaInformation, pFMI);
		}

		DCM_DIR_DATASET_CLASS* pDicomDirDataset = pFILE_DATASET_CLASS->getDicomdirDataset();
		if (pDicomDirDataset != nullptr)
		{
			pDicomFile->DataSet = ManagedUnManagedDimseConvertor::Convert(pDicomDirDataset);
		}

        return pDicomFile;
    }

    //>>===================================================================================

    DvtkData::Media::FileHead^
        ManagedUnManagedMediaConvertor::Convert(::MEDIA_FILE_HEADER_CLASS *pFMI)
    {
        if (pFMI == nullptr) return nullptr;
        DvtkData::Media::FileHead^ pFileHead = gcnew DvtkData::Media::FileHead();

        pFileHead->DicomPrefix = gcnew cli::array<System::Byte>(FMI_PREFIX_LENGTH);
        // Local copy of const Byte* to Byte* is needed because
        // Marshal::Copy(...) does not take a const argument.
        BYTE prefix[FMI_PREFIX_LENGTH];
        memcpy(&prefix, pFMI->getPrefix(), FMI_PREFIX_LENGTH);
        Marshal::Copy(
            (IntPtr)prefix, 
            pFileHead->DicomPrefix, 
            0, FMI_PREFIX_LENGTH);
        
        pFileHead->FilePreamble = gcnew cli::array<System::Byte>(FMI_PREAMBLE_LENGTH);
        // Local copy of const Byte* to Byte* is needed because
        // Marshal::Copy(...) does not take a const argument.
        BYTE preamble[FMI_PREAMBLE_LENGTH];
        memcpy(&preamble, pFMI->getPreamble(), FMI_PREAMBLE_LENGTH);
        Marshal::Copy(
            (IntPtr)preamble, 
            pFileHead->FilePreamble, 
            0, FMI_PREAMBLE_LENGTH);

        System::String^ clistr = gcnew System::String(pFMI->getTransferSyntaxUid());
        pFileHead->TransferSyntax =
            gcnew DvtkData::Dul::TransferSyntax(clistr);

        return pFileHead;
    }

    //>>===========================================================================

    void
        ManagedUnManagedMediaConvertor::Convert(
        /*dst*/ DvtkData::Media::FileMetaInformation ^ pFileMetaInformation,
        /*src*/ MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS)

        //  DESCRIPTION     : Convert managed to unmanaged - attribute set to attribute group
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if ((pUMMEDIA_FILE_HEADER_CLASS == nullptr) ||
            (pFileMetaInformation == nullptr)) return;

        // Convert the bases classes AttributeSet <= DCM_ATTRIBUTE_GROUP_CLASS
        ManagedUnManagedDimseConvertor::Convert(pFileMetaInformation, pUMMEDIA_FILE_HEADER_CLASS);
    }
    //>>===========================================================================

    //=============================================================================
    //
    // Managed to Unmanaged
    //
    //=============================================================================

    //>>===========================================================================

    ::FILE_DATASET_CLASS* 
        ManagedUnManagedMediaConvertor::Convert(
            DvtkData::Media::DicomFile^ pDicomFile, 
            System::String^ pFileName)

        //  DESCRIPTION     : Convert managed to unmanaged - DICOM File
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pDicomFile == nullptr) return nullptr;

        ::FILE_DATASET_CLASS *pFILE_DATASET_CLASS = new ::FILE_DATASET_CLASS();

        // convert the filename
        string value;
        MarshalString(pFileName, value);
        pFILE_DATASET_CLASS->setFilename(value);

        // convert the media file header
        MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS  = new MEDIA_FILE_HEADER_CLASS();

        // convert the file prefix and preamble
        Convert(pUMMEDIA_FILE_HEADER_CLASS, pDicomFile->FileHead);

        // convert the file meta information        
        Convert(pUMMEDIA_FILE_HEADER_CLASS, pDicomFile->FileMetaInformation);

        // set the media file header
        pFILE_DATASET_CLASS->setFileMetaInformation(pUMMEDIA_FILE_HEADER_CLASS);

        // convert the dataset
        DCM_DATASET_CLASS *pDataset =  
            ManagedUnManagedDimseConvertor::Convert(pDicomFile->DataSet);
        pFILE_DATASET_CLASS->setDataset(pDataset);

        // TODO conversion
        return pFILE_DATASET_CLASS;
    }

	//>>===========================================================================

	::FILE_DATASET_CLASS* 
        ManagedUnManagedMediaConvertor::Convert(
            DvtkData::Media::DicomDir^ pDicomFile, 
            System::String^ pFileName)

        //  DESCRIPTION     : Convert managed to unmanaged - DICOM File
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pDicomFile == nullptr) return nullptr;

        ::FILE_DATASET_CLASS *pFILE_DATASET_CLASS = new ::FILE_DATASET_CLASS();

        // convert the filename
        string value;
        MarshalString(pFileName, value);
        pFILE_DATASET_CLASS->setFilename(value);

        // convert the media file header
        MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS  = new MEDIA_FILE_HEADER_CLASS();

        // convert the file prefix and preamble
        Convert(pUMMEDIA_FILE_HEADER_CLASS, pDicomFile->FileHead);

        // convert the file meta information        
        Convert(pUMMEDIA_FILE_HEADER_CLASS, pDicomFile->FileMetaInformation);

        // set the media file header
        pFILE_DATASET_CLASS->setFileMetaInformation(pUMMEDIA_FILE_HEADER_CLASS);

        // convert the dataset
        DCM_DATASET_CLASS *pDataset =  
            ManagedUnManagedDimseConvertor::Convert(pDicomFile->DataSet);
        pFILE_DATASET_CLASS->setDataset(pDataset);

        // TODO conversion
        return pFILE_DATASET_CLASS;
    }

	//>>===========================================================================

    ::FILE_DATASET_CLASS* 
        ManagedUnManagedMediaConvertor::Convert(
		DvtkData::Dimse::DataSet^ pDataset, 
            System::String^ pFileName,
			System::String^ pTransferSyntax)

        //  DESCRIPTION     : Convert managed to unmanaged - Dataset
        //  PRECONDITIONS   :
        //  POSTCONDITIONS  :
        //  EXCEPTIONS      : 
        //  NOTES           :
        //<<===========================================================================
    {
        if (pDataset == nullptr) return nullptr;

        ::FILE_DATASET_CLASS *pFILE_DATASET_CLASS = new ::FILE_DATASET_CLASS();

        // convert the filename
        string value;
        MarshalString(pFileName, value);
        pFILE_DATASET_CLASS->setFilename(value);

        // convert the transfer syntax
        string tsValue;
        MarshalString(pTransferSyntax, tsValue);
        pFILE_DATASET_CLASS->setTransferSyntax(tsValue);

        // convert the dataset
        DCM_DATASET_CLASS *pDCMDataset =  
            ManagedUnManagedDimseConvertor::Convert(pDataset);
        pFILE_DATASET_CLASS->setDataset(pDCMDataset);

        // TODO conversion
        return pFILE_DATASET_CLASS;
    }

    //>>===========================================================================

    void
        ManagedUnManagedMediaConvertor::Convert(
        /*dst*/ MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS, 
        /*src*/ DvtkData::Media::FileHead ^ pFileHead)

    //  DESCRIPTION     : Convert managed to unmanaged - Media Prefix and Preamble
    //  PRECONDITIONS   :
    //  POSTCONDITIONS  :
    //  EXCEPTIONS      : 
    //  NOTES           :
    //<<===========================================================================
    {
        if ((pUMMEDIA_FILE_HEADER_CLASS == nullptr) ||
            (pFileHead == nullptr)) return;

        // convert the preamble
        int length = pFileHead->FilePreamble->Length;
        if (length > FMI_PREAMBLE_LENGTH) length = FMI_PREAMBLE_LENGTH;
        BYTE *pPreamble = (BYTE*) pUMMEDIA_FILE_HEADER_CLASS->getPreamble();
        for (int i = 0; i < length; i++)
        {
    	    pPreamble[i] = pFileHead->FilePreamble[i];
        }

        // convert the prefix
        length = pFileHead->DicomPrefix->Length;
        if (length > FMI_PREFIX_LENGTH) length = FMI_PREFIX_LENGTH;
    	char prefix[FMI_PREFIX_LENGTH + 1];
        for (int i = 0; i < length; i++)
        {
            prefix[i] = pFileHead->DicomPrefix[i];
        }
        prefix[length] = '\0';
	    pUMMEDIA_FILE_HEADER_CLASS->setPrefix(prefix);

        // convert the transfer syntax
        string value;
        MarshalString(pFileHead->TransferSyntax->ToString(), value);
	    pUMMEDIA_FILE_HEADER_CLASS->setTransferSyntaxUid(value);
    }

    //>>===========================================================================

    void
        ManagedUnManagedMediaConvertor::Convert(
        /*dst*/ MEDIA_FILE_HEADER_CLASS *pUMMEDIA_FILE_HEADER_CLASS, 
        /*src*/ DvtkData::Media::FileMetaInformation ^ pFileMetaInformation)

    //  DESCRIPTION     : Convert managed to unmanaged - File Meta Information
    //  PRECONDITIONS   :
    //  POSTCONDITIONS  :
    //  EXCEPTIONS      : 
    //  NOTES           :
    //<<===========================================================================
    {
        if ((pUMMEDIA_FILE_HEADER_CLASS == nullptr) ||
            (pFileMetaInformation == nullptr)) return;

        // Convert the bases classes DCM_ATTRIBUTE_GROUP_CLASS <= AttributeSet
        ManagedUnManagedDimseConvertor::Convert(pUMMEDIA_FILE_HEADER_CLASS, pFileMetaInformation);
    }
}