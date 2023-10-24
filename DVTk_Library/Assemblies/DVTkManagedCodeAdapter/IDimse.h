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
    public interface class IDimse
    {
        /*__property bool get_AutoType2Attributes();
        __property void set_AutoType2Attributes(bool value);*/

        property bool AutoType2Attributes
        {
            bool get();
            void set(bool value);
        }

        /*__property bool get_DefineSqLength();
        __property void set_DefineSqLength(bool value);*/

        property bool DefineSqLength
        {
            bool get();
            void set(bool value);
        }

        /*
        bool get_AddGroupLength();
        __property void set_AddGroupLength(bool value);*/

        property bool AddGroupLength
        {
            bool get();
            void set(bool value);
        }
    };
}
