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
    public enum class StorageMode
    {
        StorageModeAsMedia,
        StorageModeAsMediaOnly,
        StorageModeAsDataSet,
        StorageModeTemporaryPixelOnly,
        StorageModeNoStorage,
    };

    public interface class ISession
    {
        /*__property Wrappers::StorageMode get_StorageMode();*/
        //__property void set_StorageMode(Wrappers::StorageMode value);
        property Wrappers::StorageMode StorageMode
        {
            Wrappers::StorageMode get();
            void set(Wrappers::StorageMode value);
        }
        /*__property bool get_StrictValidation();
        __property void set_StrictValidation(bool value);*/
        property bool StrictValidation
        {
            bool get();
            void set(bool value);
        }

        /*__property bool get_ContinueOnError();
        __property void set_ContinueOnError(bool value);*/
        property bool ContinueOnError
        {
            bool get();
            void set(bool value);
        }

       /* __property bool get_ValidateReferencedFile();
        __property void set_ValidateReferencedFile(bool value);*/
        property bool ValidateReferencedFile
        {
            bool get();
            void set(bool value);
        }
    };
}


