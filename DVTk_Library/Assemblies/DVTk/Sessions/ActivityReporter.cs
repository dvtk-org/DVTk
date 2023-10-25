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

using System;
using System.IO;

namespace Dvtk.Sessions
{
    using Dvtk.Events;

    internal class ActivityReporter
        : Wrappers.IActivityReportingTarget
    {
        internal ActivityReporter(Session session)
        {
            if (session == null) throw new System.ArgumentNullException();
            this.m_parentSession = session;
        }
        private Session m_parentSession;

        #region Wrappers.IActivityReportingTarget
        static private Dvtk.Events.ReportLevel _Convert(
            Wrappers.WrappedValidationMessageLevel activityReportLevel)
        {
            switch (activityReportLevel)
            {
                case Wrappers.WrappedValidationMessageLevel.None:
                    return Dvtk.Events.ReportLevel.None;
                case Wrappers.WrappedValidationMessageLevel.Error:
                    return Dvtk.Events.ReportLevel.Error;
                case Wrappers.WrappedValidationMessageLevel.Debug:
                    return Dvtk.Events.ReportLevel.Debug;
                case Wrappers.WrappedValidationMessageLevel.Warning:
                    return Dvtk.Events.ReportLevel.Warning;
                case Wrappers.WrappedValidationMessageLevel.Information:
                    return Dvtk.Events.ReportLevel.Information;
                case Wrappers.WrappedValidationMessageLevel.ConditionText:
                    return Dvtk.Events.ReportLevel.ConditionText;
                case Wrappers.WrappedValidationMessageLevel.Scripting:
                    return Dvtk.Events.ReportLevel.Scripting;
                case Wrappers.WrappedValidationMessageLevel.ScriptName:
                    return Dvtk.Events.ReportLevel.ScriptName;
                case Wrappers.WrappedValidationMessageLevel.MediaFilename:
                    return Dvtk.Events.ReportLevel.MediaFilename;
                case Wrappers.WrappedValidationMessageLevel.DicomObjectRelationship:
                    return Dvtk.Events.ReportLevel.DicomObjectRelationship;
                case Wrappers.WrappedValidationMessageLevel.DulpStateMachine:
                    return Dvtk.Events.ReportLevel.DulpStateMachine;
                case Wrappers.WrappedValidationMessageLevel.WareHouseLabel:
                    return Dvtk.Events.ReportLevel.WareHouseLabel;
                default: throw new System.ArithmeticException();
            }
        }
        public void ReportActivity(
            Wrappers.WrappedValidationMessageLevel activityReportLevel,
            System.String message)
        {
            ActivityReportEventArgs e =
                new ActivityReportEventArgs(_Convert(activityReportLevel), message);
            this.m_parentSession._OnActivityReport(e);
        }
        #endregion
    }

}
