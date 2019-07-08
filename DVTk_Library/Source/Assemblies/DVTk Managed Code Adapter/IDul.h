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
    public __gc __interface IDul
    {
        __property System::String __gc* get_DvtAeTitle();
        __property void set_DvtAeTitle(System::String __gc* value);
        
        __property System::String __gc* get_SutAeTitle();
        __property void set_SutAeTitle(System::String __gc* value);

        __property System::UInt32 get_DvtMaximumLengthReceived();
        __property void set_DvtMaximumLengthReceived(System::UInt32 value);
        
        __property System::UInt32 get_SutMaximumLengthReceived();
        __property void set_SutMaximumLengthReceived(System::UInt32 value);

        __property System::String __gc* get_DvtImplementationClassUid();
        __property void set_DvtImplementationClassUid(System::String __gc* value);

        __property System::String __gc* get_SutImplementationClassUid();
        __property void set_SutImplementationClassUid(System::String __gc* value);

        __property System::String __gc* get_DvtImplementationVersionName();
        __property void set_DvtImplementationVersionName(System::String __gc* value);

        __property System::String __gc* get_SutImplementationVersionName();
        __property void set_SutImplementationVersionName(System::String __gc* value);
    };
}
