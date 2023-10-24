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
#include "ActivityReportingAdapter.h"
#include < vcclr.h >
#using <mscorlib.dll>

namespace Wrappers
{
    // ctor
    ActivityReportingAdapter::ActivityReportingAdapter(
		IActivityReportingTarget^ activityReportingTarget,
		ICountingTarget^ countingTarget)
    {
        m_pActivityReportingTarget = activityReportingTarget;
		m_pCountingTarget = countingTarget;
    }
    // dtor
    ActivityReportingAdapter::~ActivityReportingAdapter(void)
    {}

    void ActivityReportingAdapter::ReportActivity(ReportLevel reportLevel, const char* message)
    {
        // Translation from unmanaged -> managed CLR
        Wrappers::WrappedValidationMessageLevel level = _Convert(reportLevel);
        System::String^ clistr = gcnew System::String(message);
        m_pActivityReportingTarget->ReportActivity(level, clistr);
   } 
}