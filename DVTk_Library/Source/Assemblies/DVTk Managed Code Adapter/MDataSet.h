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
	public __gc class MDataSet
	{
	public:
		MDataSet(void);

		~MDataSet(void);

		static DvtkData::Dimse::DataSet __gc* ReadFile(System::String __gc* pFileName, bool unVrDefinitionLookUp);

		static DvtkData::Media::FileMetaInformation __gc* ReadFMI(System::String __gc* pFileName, bool unVrDefinitionLookUp);

		static System::Boolean WriteFile(
            DvtkData::Media::DicomFile __gc* pDicomFile, System::String __gc* pFileName);

		static System::Boolean WriteDataSet(
            DvtkData::Dimse::DataSet __gc* pDataset, System::String __gc* pFileName, System::String __gc* pTransferSyntax);

		static System::UInt32 ComputeItemOffset(
            DvtkData::Dimse::DataSet __gc* pDataset, System::String __gc* pTransferSyntax);
	};
}
