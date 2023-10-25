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
    public ref class MPrinter
    {
    internal:
        MPrinter(void);
    internal:
        ~MPrinter(void);
    public:
        static System::UInt32 getNrOfStatusInfoDefinedTerms();
    public:
        static System::String ^ getStatusInfoDefinedTerm(System::UInt32 index);
    public:
        // <summary>
        // Get the Manufacturer
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        
        //__property System::String __gc* get_Manufacturer();
        property System::String^ Manufacturer
        {
            System::String^ get();
        }

        // <summary>
        // Get the ModelName
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::String __gc* get_ModelName();
        property System::String^ ModelName
        {
            System::String^ get();
        }
        // <summary>
        // Get the Name
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::String __gc* get_Name();
        property System::String^ Name
        {
            System::String^ get();
        }
        // <summary>
        // Get the SerialNumber
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::String __gc* get_SerialNumber();
        property System::String^ SerialNumber
        {
            System::String^ get();
        }
        // <summary>
        // Get the Status
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::String __gc* get_Status();
        //__property void set_Status(System::String __gc* value);
        property System::String^ Status
        {
            System::String^ get();
            void set(System::String^ value);
        }
        // <summary>
        // Set the Status
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        // <summary>
        // Get the StatusInfo
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        /*__property System::String __gc* get_StatusInfo();
        __property void set_StatusInfo(System::String __gc* value);*/
        property System::String^ StatusInfo
        {
            System::String^ get();
            void set(System::String^ value);
        }
        // <summary>
        // Set the StatusInfo
        // </summary>
        // <remarks>
        // Set Property
        // </remarks>
        // <summary>
        // Get the CalibrationTime
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::DateTime get_CalibrationTime();
        property System::DateTime CalibrationTime
        {
            System::DateTime get();
        }
        // <summary>
        // Get the SoftwareVersions
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::String __gc* get_SoftwareVersions();
        property System::String^ SoftwareVersions
        {
            System::String^ get();
        }
        // <summary>
        // Get the CalibrationDate
        // </summary>
        // <remarks>
        // Get Property
        // </remarks>
        //__property System::DateTime get_CalibrationDate();
        property System::DateTime CalibrationDate
        {
            System::DateTime get();
        }
    };
}