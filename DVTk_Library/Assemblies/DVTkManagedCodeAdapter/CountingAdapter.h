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

    public enum class CountGroup
    {
        Validation = 0,
        General = 1,
        User = 2,
    };

    public enum class CountType
    {
        Error = 0,
        Warning = 1,
//        Info,
    };

    // Interface to implement on the call-back counting class.
    public interface class ICountingTarget
    {
        void Increment(CountGroup group, CountType type);
        //__property System::UInt32 get_NrOfErrors();
        property System::UInt32 NrOfErrors
        {
            System::UInt32 get();
        }

        //__property System::UInt32 get_NrOfWarnings();
        property System::UInt32 NrOfWarnings
        {
            System::UInt32 get();
        }

		/// <summary>
        /// Number of validation errors.
        /// </summary>
        //__property System::UInt32 get_NrOfValidationErrors();
        property System::UInt32 NrOfValidationErrors
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Number of validation warnings.
        /// </summary>
        //__property System::UInt32 get_NrOfValidationWarnings();
        property System::UInt32 NrOfValidationWarnings
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Number of general errors.
        /// </summary>
        //__property System::UInt32 get_NrOfGeneralErrors();
        property System::UInt32 NrOfGeneralErrors
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Number of general warnings.
        /// </summary>
        //__property System::UInt32 get_NrOfGeneralWarnings();
        property System::UInt32 NrOfGeneralWarnings
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Number of user errors.
        /// </summary>
        //__property System::UInt32 get_NrOfUserErrors();
        property System::UInt32 NrOfUserErrors
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Number of user warnings.
        /// </summary>
        //__property System::UInt32 get_NrOfUserWarnings();
        property System::UInt32 NrOfUserWarnings
        {
            System::UInt32 get();
        }

		/// <summary>
        /// Total number of validation errors - including any child counts.
        /// </summary>
        //__property System::UInt32 get_TotalNrOfValidationErrors();
        property System::UInt32 TotalNrOfValidationErrors
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Total number of validation warnings - including any child counts.
        /// </summary>
        //__property System::UInt32 get_TotalNrOfValidationWarnings();
        property System::UInt32 TotalNrOfValidationWarnings
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Total number of general errors - including any child counts.
        /// </summary>
        //__property System::UInt32 get_TotalNrOfGeneralErrors();
        property System::UInt32 TotalNrOfGeneralErrors
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Total number of general warnings - including any child counts.
        /// </summary>
        //__property System::UInt32 get_TotalNrOfGeneralWarnings();
        property System::UInt32 TotalNrOfGeneralWarnings
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Total number of user errors - including any child counts.
        /// </summary>
        //__property System::UInt32 get_TotalNrOfUserErrors();
        property System::UInt32 TotalNrOfUserErrors
        {
            System::UInt32 get();
        }
        /// <summary>
        /// Total number of user warnings - including any child counts.
        /// </summary>
        //__property System::UInt32 get_TotalNrOfUserWarnings();
        property System::UInt32 TotalNrOfUserWarnings
        {
            System::UInt32 get();
        }

        /// <summary>
        /// Reset counters.
        /// </summary>
		void Init();

		ICountingTarget^ CreateChildCountingTarget();
    };

    class CountingAdapter
    {
    public:
        // ctor
        CountingAdapter(ICountingTarget^ value);
        // dtor
        ~CountingAdapter(void);

	public:
        gcroot<ICountingTarget^> m_pCountingTarget;
    };
}