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

namespace Wrappers
{
	public ref class MDataSet
	{
	public:
		MDataSet(void);

		~MDataSet(void);

		static DvtkData::Dimse::DataSet^ ReadFile(System::String^ pFileName, bool unVrDefinitionLookUp);

		static DvtkData::Media::FileMetaInformation^ ReadFMI(System::String^ pFileName, bool unVrDefinitionLookUp);

		static System::Boolean WriteFile(
            DvtkData::Media::DicomFile^ pDicomFile, System::String^ pFileName);

		static System::Boolean WriteDataSet(
            DvtkData::Dimse::DataSet^ pDataset, System::String^ pFileName, System::String^ pTransferSyntax);

		static System::UInt32 ComputeItemOffset(
            DvtkData::Dimse::DataSet^ pDataset, System::String^ pTransferSyntax);
	};
}
