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
#include ".\MPrinter.h"
#include "Iprint.h"		// Printer component interface
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    MPrinter::MPrinter(void)
    {
//        m_pC = gcnew MYPRINTER();
    }

    MPrinter::~MPrinter(void)
    {
//        delete m_pC;
    }

    System::UInt32 MPrinter::getNrOfStatusInfoDefinedTerms()
    {
        return MYPRINTER->getNoStatusInfoDTs();
    }
    
    System::String ^ MPrinter::getStatusInfoDefinedTerm(System::UInt32 index)
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(MYPRINTER->getStatusInfoDT(index).c_str());
        return clistr;
        //return MYPRINTER->getStatusInfoDT(index).c_str();
    }

    System::String^ MPrinter::Manufacturer::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.        
        System::String^ clistr = gcnew System::String(MYPRINTER->getManufacturer());
        return clistr;
        //return MYPRINTER->getManufacturer();
    }
    System::String ^ MPrinter::ModelName::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(MYPRINTER->getModelName());
        return clistr;
        //return MYPRINTER->getModelName();
    }
    System::String^ MPrinter::Name::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(MYPRINTER->getName());
        return clistr; 
        //return MYPRINTER->getName();
    }
    System::String^ MPrinter::SerialNumber::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(MYPRINTER->getSerialNumber());
        return clistr; 
        //return MYPRINTER->getSerialNumber();
    }
    System::String^ MPrinter::Status::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(MYPRINTER->getStatus());
        return clistr; 
        //return MYPRINTER->getStatus();
    }
    void MPrinter::Status::set(System::String ^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        MYPRINTER->setStatus(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
    }
    System::String^ MPrinter::StatusInfo::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(MYPRINTER->getStatusInfo());
        return clistr; 
        //return MYPRINTER->getStatusInfo();
    }
    void MPrinter::StatusInfo::set(System::String ^ value)
    {
        char* pAnsiString = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
        MYPRINTER->setStatusInfo(pAnsiString);
        Marshal::FreeHGlobal(System::IntPtr((void*)pAnsiString));
        //Marshal::FreeHGlobal(pAnsiString);
    }
    System::DateTime MPrinter::CalibrationTime::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        // DICOM "HHMMSS"
        System::String^ pTimeString = gcnew System::String(MYPRINTER->getCalibrationTime());
        //System::String^ pTimeString = MYPRINTER->getCalibrationTime();
        System::String^ pFormat = "HHmmss";
        System::DateTime time =
            System::DateTime::ParseExact(pTimeString, pFormat, nullptr);
        return time;
    }
    System::String^ MPrinter::SoftwareVersions::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        System::String^ clistr = gcnew System::String(MYPRINTER->getSoftwareVersion());
        return clistr; 
        //return MYPRINTER->getSoftwareVersion();
    }
    System::DateTime MPrinter::CalibrationDate::get()
    {
        // Implicit marshalling from const char* to System::String by Managed C++ compiler.
        // DICOM "yyyyMMdd"
        System::String^ pDateString = gcnew System::String(MYPRINTER->getCalibrationDate());
        //System::String^ pDateString = MYPRINTER->getCalibrationDate();
        System::String^ pFormat = "yyyyMMdd";
        System::DateTime date =
            System::DateTime::ParseExact(pDateString, pFormat, nullptr);
        return date;
    }
}