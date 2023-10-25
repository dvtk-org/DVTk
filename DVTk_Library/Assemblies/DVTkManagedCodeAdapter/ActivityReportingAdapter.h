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
#include "Ilog.h"
#include "ISessions.h"
#include "CountingAdapter.h"
#include <vcclr.h>

namespace Wrappers
{
    using namespace System::Runtime::InteropServices;

    public enum class WrappedValidationMessageLevel
    {
        None,
        Error,
        Debug,
        Warning,
        Information,
        Scripting,
		ScriptName,
        MediaFilename,
        DicomObjectRelationship,
        DulpStateMachine,
        WareHouseLabel,
        ConditionText,
    };
    inline WrappedValidationMessageLevel _Convert(ReportLevel reportLevel)
    {
        switch (reportLevel)
        {
        case ::ReportLevel_None             : return WrappedValidationMessageLevel::None;
        case ::ReportLevel_Error            : return WrappedValidationMessageLevel::Error;
        case ::ReportLevel_Debug            : return WrappedValidationMessageLevel::Debug;
        case ::ReportLevel_Warning          : return WrappedValidationMessageLevel::Warning;
        case ::ReportLevel_Information      : return WrappedValidationMessageLevel::Information;
        case ::ReportLevel_Scripting        : return WrappedValidationMessageLevel::Scripting;
        case ::ReportLevel_ScriptName       : return WrappedValidationMessageLevel::ScriptName;
        case ::ReportLevel_MediaFilename    : return WrappedValidationMessageLevel::MediaFilename;
		case ::ReportLevel_DicomObjectRelationship :
            return WrappedValidationMessageLevel::DicomObjectRelationship;
        case ::ReportLevel_DulpStateMachine : return WrappedValidationMessageLevel::DulpStateMachine;
        case ::ReportLevel_WareHouseLabel   : return WrappedValidationMessageLevel::WareHouseLabel;
        case ::ReportLevel_ConditionText   : return WrappedValidationMessageLevel::ConditionText;
            // Unknown Report level
        default                             : throw gcnew System::NotImplementedException();
        }
    }
    
    public interface class IActivityReportingTarget
    {
        void ReportActivity(WrappedValidationMessageLevel level, System::String^ message);
    };
    
    class ActivityReportingAdapter
        : public BASE_ACTIVITY_REPORTER
    {
    public:
        // ctor
        ActivityReportingAdapter(
			IActivityReportingTarget^ activityReportingTarget,
			ICountingTarget^ countingTarget);
        // dtor
        ~ActivityReportingAdapter(void);

        void ReportActivity(ReportLevel reportLevel, const char* message);

    private:
		gcroot<Wrappers::ICountingTarget^> m_pCountingTarget;
        gcroot<IActivityReportingTarget^> m_pActivityReportingTarget;
    };
}