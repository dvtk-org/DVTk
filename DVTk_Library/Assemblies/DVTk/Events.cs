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

namespace Dvtk.Events
{
    /// <summary>
    /// Report level of activities reported by the Dicom Validation Tool (DVT).
    /// </summary>
    public enum ReportLevel
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Debug
        /// </summary>
        Debug,
        /// <summary>
        /// Wanring
        /// </summary>
        Warning,
        /// <summary>
        /// Information
        /// </summary>
        Information,
        /// <summary>
        /// Scripting
        /// </summary>
        Scripting,
        /// <summary>
        /// ScriptName
        /// </summary>
        ScriptName,
        /// <summary>
        /// MediaFilename
        /// </summary>
        MediaFilename,
        /// <summary>
        /// Dicom object (Information Object IO) relationship
        /// </summary>
        DicomObjectRelationship,
        /// <summary>
        /// DULP (Dicom Upper Layer) state machine
        /// </summary>
        DulpStateMachine,
        /// <summary>
        /// Internal warehouse label of in memory validation objects
        /// </summary>
        WareHouseLabel,
        /// <summary>
        /// ConditionText
        /// </summary>
        ConditionText,
    }
    /// <summary>
    /// Provides data for the <see cref="Dvtk.Sessions.Session.ActivityReportEvent"/> event.
    /// </summary>
    public class ActivityReportEventArgs
        : EventArgs
    {
        private readonly System.String _Message;
        private readonly ReportLevel _ReportLevel;
        //
        // Constructor
        //
        internal ActivityReportEventArgs(ReportLevel reportLevel, System.String message)
        {
            this._Message = message;
            this._ReportLevel = reportLevel;
        }
        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get { return this._Message; }
        }
        /// <summary>
        /// Report level
        /// </summary>
        public ReportLevel ReportLevel
        {
            get { return this._ReportLevel; }
        }
    }
    /// <summary>
    /// Represents the method that handles the <see cref="Dvtk.Sessions.Session.ActivityReportEvent"/>.
    /// </summary>
    /// <remarks>
    /// Users may register a callback to listen for this event.
    /// </remarks>
    /// <example>
    /// <code>
    /// class MyClass
    /// {
    ///     Dvtk.Sessions.Session ses;
    ///        
    ///     // Implement a callback handler with same argument syntax as this delegate declaration.
    ///     private void OnActivityReportEvent(object sender, Dvtk.Events.ActivityReportEventArgs e)
    ///     {
    ///         ...
    ///     }
    /// 
    ///     // Create handler instance.
    ///     private Dvtk.Events.ActivityReportEventHandler myHandler 
    ///         = new Dvtk.Events.ActivityReportEventHandler(this.OnActivityReportEvent);
    ///     // Couple handler instance to event source.
    ///     ses.ActivityReportEvent += myHandler;
    /// }
    /// </code>
    /// </example>
    public delegate void ActivityReportEventHandler(object sender, ActivityReportEventArgs e);
}
