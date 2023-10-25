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
#include ".\UtilityFunctions.h"
#include < vcclr.h >
#using <mscorlib.dll>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    void MarshalString ( System::String ^ s, std::string& os )
    {
        using namespace System::Runtime::InteropServices;
        const char* chars = 
            (const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
        os = chars;
        Marshal::FreeHGlobal(System::IntPtr((void*)chars));
    }

    std::string ConvertString(std::string inString)
    {
        std::string outString;
        for (UINT i = 0; i < inString.length(); i++)
        {
            char valueString[16];
            BYTE value = (BYTE) inString[i];

            if ((value >= 0) && 
                (value < 32))
            {
                // char in range 0..31
                switch(value)
                {
                case 9: sprintf(valueString, "&#x09;"); break;
                case 10: sprintf(valueString, "[LF]"); break;
                case 12: sprintf(valueString, "[FF]"); break;
                case 13: sprintf(valueString, "[CR]"); break;
                case 14: sprintf(valueString, "[SO]"); break;
                case 15: sprintf(valueString, "[SI]"); break;
                case 27: sprintf(valueString, "[ESC]"); break;
                default: sprintf(valueString, "\\%02X", value); break;
                }
            }
            else if ((value > 126) &&
                (value <= 255))
            {
                // char in range 127..255
               sprintf(valueString, "\\%02X", value);
            }
            else
            {
                switch(value)
                {
                case 38: sprintf(valueString, "&#x26;"); break; // &
                case 60: sprintf(valueString, "&#x3C;"); break; // <
                case 62: sprintf(valueString, "&#x3E;"); break; // >
                default:
                    // char in range 32..127
                    valueString[0] = value;
                    valueString[1] = 0x00;
                    break;
                }
            }

            // add the character value to the string
            outString += valueString;
        }

        return outString;
    }
}