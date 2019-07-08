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
#include ".\MDICOMDIRGenerator.h"
#using <mscorlib.dll>

namespace Wrappers
{
	using namespace System::Runtime::InteropServices;

	MDICOMDIRGenerator::MDICOMDIRGenerator(void)
	{
	}

	MDICOMDIRGenerator::~MDICOMDIRGenerator(void)
	{
	}

	bool MDICOMDIRGenerator::WCreateDICOMDIR(System::String* dcmFileNames[])
	{
		GENERATE_DICOMDIR_CLASS* pDICOMDIR = new GENERATE_DICOMDIR_CLASS();

		vector<string> fileNameVector;
        int size = dcmFileNames->Length;
        for (int idx = 0; idx < size; idx++)
        {
            System::String* value = dcmFileNames[idx];
            char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
            string file = pAnsiString;
            fileNameVector.push_back(file);
            Marshal::FreeHGlobal(pAnsiString);
        }
        
		bool ok = pDICOMDIR->generateDICOMDIR(&fileNameVector);
		delete pDICOMDIR;
        return ok;
	}
}