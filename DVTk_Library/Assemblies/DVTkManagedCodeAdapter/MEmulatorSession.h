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
#include "MBaseSession.h"
#include "MPrinter.h"
#define FileName System::String

namespace Wrappers
{
    public ref class MEmulatorSession
        : public MBaseSession
        , public System::IDisposable
    {
    private:
        EMULATOR_SESSION_CLASS * m_pEMULATOR_SESSION_CLASS;
    private protected:
        /*__property BASE_SESSION_CLASS __nogc* get_m_pBASE_SESSION()
        {
            return m_pEMULATOR_SESSION_CLASS;
        }*/
        property BASE_SESSION_CLASS* m_pBASE_SESSION 
        {
            BASE_SESSION_CLASS* get() override
            {
                return m_pEMULATOR_SESSION_CLASS;
            };
        }
    public:
        // <summary>
        // Constructor
        // </summary>
        // <remarks>
        // None
        // </remarks>
        MEmulatorSession(void);
    public:
        // <summary>
        // Destructor
        // </summary>
        // <remarks>
        // None
        // </remarks>
        ~MEmulatorSession(void);

    public:
        // <summary>
        // EmulateStorageSCU
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        bool EmulateStorageSCU(
            //FileName* fileNames[], 
            cli::array<FileName^>^ fileNames,
            System::UInt32 options, 
            System::UInt32 repetitions);

	public:
		bool EmulateStorageCommitSCU(
			System::Int16 delay);

    public:
        bool EmulateVerificationSCU();

    private:
        MPrinter^ m_pPrinter;
    public:
        //__property MPrinter __gc* get_MPrinter();
        property MPrinter^ MPrinter
        {
            Wrappers::MPrinter^ get();
        }

	private:
        // Track whether Dispose has been called.
        // m_pEMULATOR_SESSION_CLASS is treated as disposable resource.
        bool disposed;

    public:
        void DisposeThis();

    private:
        void DisposeThis(bool disposing);
    };
}