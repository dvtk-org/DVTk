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
#include "ISessions.h"
#include <vcclr.h>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    public __value enum CountGroup
    {
        Validation = 0,
        General = 1,
        User = 2,
    };

    public __value enum CountType
    {
        Error = 0,
        Warning = 1,
//        Info,
    };

    // Interface to implement on the call-back counting class.
    public __gc __interface ICountingTarget
    {
        void Increment(CountGroup group, CountType type);
        __property System::UInt32 get_NrOfErrors();
        __property System::UInt32 get_NrOfWarnings();

		/// <summary>
        /// Number of validation errors.
        /// </summary>
        __property System::UInt32 get_NrOfValidationErrors();
        /// <summary>
        /// Number of validation warnings.
        /// </summary>
        __property System::UInt32 get_NrOfValidationWarnings();
        /// <summary>
        /// Number of general errors.
        /// </summary>
        __property System::UInt32 get_NrOfGeneralErrors();
        /// <summary>
        /// Number of general warnings.
        /// </summary>
        __property System::UInt32 get_NrOfGeneralWarnings();
        /// <summary>
        /// Number of user errors.
        /// </summary>
        __property System::UInt32 get_NrOfUserErrors();
        /// <summary>
        /// Number of user warnings.
        /// </summary>
        __property System::UInt32 get_NrOfUserWarnings();

		/// <summary>
        /// Total number of validation errors - including any child counts.
        /// </summary>
        __property System::UInt32 get_TotalNrOfValidationErrors();
        /// <summary>
        /// Total number of validation warnings - including any child counts.
        /// </summary>
        __property System::UInt32 get_TotalNrOfValidationWarnings();
        /// <summary>
        /// Total number of general errors - including any child counts.
        /// </summary>
        __property System::UInt32 get_TotalNrOfGeneralErrors();
        /// <summary>
        /// Total number of general warnings - including any child counts.
        /// </summary>
        __property System::UInt32 get_TotalNrOfGeneralWarnings();
        /// <summary>
        /// Total number of user errors - including any child counts.
        /// </summary>
        __property System::UInt32 get_TotalNrOfUserErrors();
        /// <summary>
        /// Total number of user warnings - including any child counts.
        /// </summary>
        __property System::UInt32 get_TotalNrOfUserWarnings();

        /// <summary>
        /// Reset counters.
        /// </summary>
		void Init();

		ICountingTarget __gc* CreateChildCountingTarget();
    };

    __nogc class CountingAdapter
    {
    public:
        // ctor
        CountingAdapter(ICountingTarget __gc* value);
        // dtor
        ~CountingAdapter(void);

	public:
        gcroot<ICountingTarget*> m_pCountingTarget;
    };
}