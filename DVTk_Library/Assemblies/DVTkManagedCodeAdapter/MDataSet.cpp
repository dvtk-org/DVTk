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
#include ".\mdataset.h"
#include "MDIMSEConvertors.h"
#include "MMediaConvertors.h"
#using <mscorlib.dll>

namespace Wrappers
{
	using namespace System::Runtime::InteropServices;

	MDataSet::MDataSet(void)
	{
	}

	MDataSet::~MDataSet(void)
	{
	}

	DvtkData::Dimse::DataSet^ MDataSet::ReadFile(System::String^ pFileName, bool unVrDefinitionLookUp)
	{
		DCM_DATASET_CLASS* pDcmDataSet = new DCM_DATASET_CLASS();

        char* pAnsiStringFileName = (char*)(void*)Marshal::StringToHGlobalAnsi(pFileName);

		FILE_DATASET_CLASS* pFileDataSet = new FILE_DATASET_CLASS(pAnsiStringFileName);
		pFileDataSet->setUnVrDefinitionLookUp(unVrDefinitionLookUp);
		pFileDataSet->setStorageMode(SM_AS_MEDIA);
		pFileDataSet->read(pDcmDataSet);

		DvtkData::Dimse::DataSet^ dataSet = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(pDcmDataSet);
		dataSet->Filename = pFileName;

		delete pDcmDataSet;
		delete pFileDataSet;
		Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiStringFileName));
		//Marshal::FreeHGlobal(pAnsiStringFileName);

		return(dataSet);
	}

	DvtkData::Media::FileMetaInformation^ MDataSet::ReadFMI(System::String^ pFileName, bool unVrDefinitionLookUp)
	{
		char* pAnsiStringFileName = (char*)(void*)Marshal::StringToHGlobalAnsi(pFileName);

		MEDIA_FILE_HEADER_CLASS* pFileMetaInfo = new MEDIA_FILE_HEADER_CLASS();

		FILE_DATASET_CLASS* pFileDataSet = new FILE_DATASET_CLASS(pAnsiStringFileName);
		pFileDataSet->setUnVrDefinitionLookUp(unVrDefinitionLookUp);

		bool ok = pFileDataSet->read();

		if(ok)
		{
			pFileMetaInfo = pFileDataSet->getFileMetaInformation();
		}

		DvtkData::Media::FileMetaInformation^ fmi = gcnew DvtkData::Media::FileMetaInformation();
		ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(fmi,pFileMetaInfo);
		
		//delete pFileMetaInfo;
		delete pFileDataSet;
		Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiStringFileName));
		//Marshal::FreeHGlobal(pAnsiStringFileName);

		return(fmi);
	}

	System::Boolean MDataSet::WriteFile(
        DvtkData::Media::DicomFile^ pDicomFile, System::String^ pFileName)
	{
		bool success = false;
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDicomFile, pFileName);
        success = fileDataset_ptr->write();
        delete fileDataset_ptr;
        return success;
	}

	System::Boolean MDataSet::WriteDataSet(
        DvtkData::Dimse::DataSet^ pDataSet, System::String^ pFileName, System::String^ pTransferSyntax)
	{
		bool success = false;
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDataSet, pFileName,pTransferSyntax);
        success = fileDataset_ptr->writeDataset();
        delete fileDataset_ptr;
        return success;
	}

	System::UInt32 MDataSet::ComputeItemOffset(
        DvtkData::Dimse::DataSet^ pDataSet, System::String^ pTransferSyntax)
	{
		char* pAnsiTransferSyntax = (char*)(void*)Marshal::StringToHGlobalAnsi(pTransferSyntax);
        DCM_DATASET_CLASS* pDcmDataSet = ManagedUnManagedDimseConvertors::ManagedUnManagedDimseConvertor::Convert(pDataSet);
		System::UInt32 offset = pDcmDataSet->computeItemOffsetsForDICOMDIR(pAnsiTransferSyntax);
        return offset;
	}
}