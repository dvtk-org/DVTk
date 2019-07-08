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
#include ".\MMediaSession.h"
#include "MMediaConvertors.h"
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace ManagedUnManagedMediaConvertors;

    MMediaSession::MMediaSession(void)
    {
        m_pMEDIA_SESSION_CLASS = new MEDIA_SESSION_CLASS();
        disposed = false;
        this->Initialize();
        return;
    }

    // Use C# destructor syntax for finalization code.
    // This destructor will run only if the Dispose method
    // does not get called.
    // It gives your base class the opportunity to finalize.
    // Do not provide destructors in types derived from this class.
    MMediaSession::~MMediaSession(void)
    {
        // Do not re-create Dispose clean-up code here.
        Dispose(false);
        return;
    }

    // Implement IDisposable*
    // Do not make this method virtual.
    // A derived class should not be able to this method.
    void MMediaSession::Dispose() {
        Dispose(true);
        // This object will be cleaned up by the Dispose method.
        // Therefore, you should call GC::SupressFinalize to
        // take this object off the finalization queue
        // and prevent finalization code for this object
        // from executing a second time.
        System::GC::SuppressFinalize(this);
    }

    // Dispose(bool disposing) executes in two distinct scenarios.
    // If disposing equals true, the method has been called directly
    // or indirectly by a user's code. Managed and unmanaged resources
    // can be disposed.
    // If disposing equals false, the method has been called by the
    // runtime from inside the finalizer and you should not reference
    // other objects. Only unmanaged resources can be disposed.
    void MMediaSession::Dispose(bool disposing) {
        // If disposing equals true, dispose all managed
        // and unmanaged resources.
        if (disposing) {
            // Dispose managed resources.
        }
        // Check to see if Dispose has already been called.
        if (!this->disposed) {
            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.
            delete m_pMEDIA_SESSION_CLASS;
            m_pMEDIA_SESSION_CLASS = NULL;
        }
        disposed = true;
    }

  	System::Boolean MMediaSession::GenerateDICOMDIR(System::String __gc* dcmFileNames[])
    {
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
        
		bool success = m_pMEDIA_SESSION_CLASS->generateDICOMDIR(&fileNameVector);
        return success;
    }

    bool MMediaSession::Validate(
        DvtkData::Media::DicomFile __gc* pDicomFile, 
        Wrappers::ValidationControlFlags validationControlFlags)
    {
        bool success = false;
        if (pDicomFile == NULL) throw new System::ArgumentNullException();
        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDicomFile, NULL);
        success = m_pMEDIA_SESSION_CLASS->validate(fileDataset_ptr, umValidationControlFlags);
        delete fileDataset_ptr;
        return success;
    }

	bool MMediaSession::Validate(
		DvtkData::Media::DicomFile __gc* pDicomFile, 
        Wrappers::ValidationControlFlags validationControlFlags,System::String* filename)
    {
        bool success = false;
        if (pDicomFile == NULL) throw new System::ArgumentNullException();
        VALIDATION_CONTROL_FLAG_ENUM umValidationControlFlags = _Convert(validationControlFlags);
        FILE_DATASET_CLASS* fileDataset_ptr = ManagedUnManagedMediaConvertors::ManagedUnManagedMediaConvertor::Convert(pDicomFile, filename);
        success = m_pMEDIA_SESSION_CLASS->validate(fileDataset_ptr, umValidationControlFlags);
        delete fileDataset_ptr;
        return success;
    }
}