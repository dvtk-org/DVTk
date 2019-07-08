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
    public __gc class MPrinter
    {
    private public:
        MPrinter(void);
    private public:
        ~MPrinter(void);
    public:
        static System::UInt32 getNrOfStatusInfoDefinedTerms();
    public:
        static System::String __gc* getStatusInfoDefinedTerm(System::UInt32 index);
    public:
        // <summary>
        // Get the Manufacturer
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_Manufacturer();
        // <summary>
        // Get the ModelName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_ModelName();
        // <summary>
        // Get the Name
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_Name();
        // <summary>
        // Get the SerialNumber
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_SerialNumber();
        // <summary>
        // Get the Status
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_Status();
        // <summary>
        // Set the Status
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_Status(System::String __gc* value);
        // <summary>
        // Get the StatusInfo
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_StatusInfo();
        // <summary>
        // Set the StatusInfo
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        __property void set_StatusInfo(System::String __gc* value);
        // <summary>
        // Get the CalibrationTime
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::DateTime get_CalibrationTime();
        // <summary>
        // Get the SoftwareVersions
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::String __gc* get_SoftwareVersions();
        // <summary>
        // Get the CalibrationDate
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        __property System::DateTime get_CalibrationDate();
    };
}