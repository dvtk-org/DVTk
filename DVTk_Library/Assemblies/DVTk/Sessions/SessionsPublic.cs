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
using Wrappers;

class MyApp
{
    static public void Main()
    {
        DicomValidationToolKit.Sessions.NewBaseSession ses =
            new DicomValidationToolKit.Sessions.NewBaseSession(null);
    }
}

namespace DicomValidationToolKit.Sessions
{
    public interface ISessionProperties
    {
        System.DateTime Date { get; set; }
        System.String SessionTitle { get; set; }
        System.UInt16 SessionId { get; set; }
        System.String SoftwareVersions { get; set; }
        System.String TestedBy { get; set; }
        System.Boolean StrictValidation { get; set; }
        System.Boolean ContinueOnError { get; set; }
        StorageMode StorageMode { get; set; }
        ISessionSutProperties SutProperties { get; }
    }
    public interface ISessionSutProperties
    {
        System.String Manufacturer { get; set; }
        System.String ModelName { get; set; }
    }
    public class NewBaseSession
        : ISessionProperties
    {
        protected Wrappers.MBaseSession m_delegate = null;

        public NewBaseSession(Wrappers.MBaseSession theDelegate)
        {
            this.m_delegate = theDelegate;
            CSutProperties _SutProperties = new CSutProperties(theDelegate);
        }

        private class CSutProperties : ISessionSutProperties
        {
            private Wrappers.MBaseSession m_delegate = null;

            public CSutProperties(Wrappers.MBaseSession theDelegate)
            {
                m_delegate = theDelegate;
            }

            public System.String Manufacturer
            {
                get { return this.m_delegate.Manufacturer; }
                set { this.m_delegate.Manufacturer = value; }
            }
            public System.String ModelName
            {
                get { return this.m_delegate.ModelName; }
                set { this.m_delegate.ModelName = value; }
            }
        }
        private CSutProperties _SutProperties = null;

        #region ISessionParameters
        public ISessionSutProperties SutProperties
        {
            get { return _SutProperties; }
        }

        public System.DateTime Date
        {
            get
            {
                // Retrieve date as char* of format yyyyMMdd
                System.String dateString = this.m_delegate.Date;
                String format = "yyyyMMdd";
                return System.DateTime.ParseExact(dateString, format, null);
            }
            set
            {
                // The year in four digits, including the century.
                // The numeric month. Single-digit months will have a leading zero.
                // The day of the month. Single-digit days will have a leading zero.
                String format = "yyyyMMdd";
                String date = value.ToString(format);
                this.m_delegate.Date = date;
            }
        }
        public System.String SessionTitle
        {
            get { return this.m_delegate.SessionTitle; }
            set { this.m_delegate.SessionTitle = value; }
        }
        public System.UInt16 SessionId
        {
            get { return this.m_delegate.SessionId; }
            set { this.m_delegate.SessionId = value; }
        }
        public System.String SoftwareVersions
        {
            get { return this.m_delegate.SoftwareVersions; }
            set { this.m_delegate.SoftwareVersions = value; }
        }
        public System.String TestedBy
        {
            get { return this.m_delegate.TestedBy; }
            set { this.m_delegate.TestedBy = value; }
        }
        public System.Boolean StrictValidation
        {
            get { return this.m_delegate.StrictValidation; }
            set { this.m_delegate.StrictValidation = value; }
        }
        public System.Boolean ContinueOnError
        {
            get { return this.m_delegate.ContinueOnError; }
            set { this.m_delegate.ContinueOnError = value; }
        }
        static private Wrappers.StorageMode
            DvtkToWrapper(DicomValidationToolKit.Sessions.StorageMode value)
        {
            switch (value)
            {
                case DicomValidationToolKit.Sessions.StorageMode.AsDataSet:
                    return Wrappers.StorageMode.StorageModeAsDataSet;
                case DicomValidationToolKit.Sessions.StorageMode.AsMedia:
                    return Wrappers.StorageMode.StorageModeAsMedia;
                case DicomValidationToolKit.Sessions.StorageMode.NoStorage:
                    return Wrappers.StorageMode.StorageModeNoStorage;
                case DicomValidationToolKit.Sessions.StorageMode.TemporaryPixelOnly:
                    return Wrappers.StorageMode.StorageModeTemporaryPixelOnly;
                default:
                    throw new ApplicationException();
            }
        }
        static private DicomValidationToolKit.Sessions.StorageMode
            WrapperToDvtk(Wrappers.StorageMode value)
        {
            switch (value)
            {
                case Wrappers.StorageMode.StorageModeAsDataSet:
                    return DicomValidationToolKit.Sessions.StorageMode.AsDataSet;
                case Wrappers.StorageMode.StorageModeAsMedia:
                    return DicomValidationToolKit.Sessions.StorageMode.AsMedia;
                case Wrappers.StorageMode.StorageModeNoStorage:
                    return DicomValidationToolKit.Sessions.StorageMode.NoStorage;
                case Wrappers.StorageMode.StorageModeTemporaryPixelOnly:
                    return DicomValidationToolKit.Sessions.StorageMode.TemporaryPixelOnly;
                default:
                    throw new ApplicationException();
            }
        }
        public StorageMode StorageMode
        {
            get { return WrapperToDvtk(this.m_delegate.StorageMode); }
            set { this.m_delegate.StorageMode = DvtkToWrapper(value); }
        }
        #endregion
    }
}

namespace DicomValidationToolKit.Sessions
{
    using DicomValidationToolKit.Validation;
    using Key = System.String;
    using FileName = System.String;
    using DicomValidationToolKit.Dimse.Services;
    using DicomValidationToolKit.Dul.Services;
    using DicomValidationToolKit.Media.Services;
    using DicomValidationToolKit.Sockets.Settings;
    //    using DicomValidationToolKit.Dul.Settings;
    using DicomValidationToolKit.Sessions.Services;
    using DicomValidationToolKit.Sessions.Settings;

    using DefinitionFileRoot = System.String;
    using DefinitionFileName = System.String;

    using System.Collections;
    using Dvtk.Dimse;
    using Dvtk.Dul;
    using DimseStatusCode = System.UInt16;

    public interface IValidation
    {
        void EnableStrictValidation();
        void DisableStrictValidation();
    }
    public abstract class Session
        : ISession
        , IUtilityCommands
        , IDefinitionManagement
        , ISessionParameters
    {
        protected Wrappers.MBaseSession m_delegate = null;

        public ISession ISession
        {
            get { return this; }
        }

        #region IUtilityCommands
        public IUtilityCommands IUtilityCommands
        {
            get { return this; }
        }
        public void Echo(string text)
        {
            // TODO: echo text to output results.
        }
        public void SystemCall(string command)
        {
            // TODO: execute the system command using a command interpreter.
        }
        public void WriteTimeToResults(DateTime dateTime)
        {
            // TODO: execute the system command using a command interpreter.
            // display current time if dateTime == null
        }
        #endregion

        #region IDefinitionManagement
        public IDefinitionManagement IDefinitionManagement
        {
            get { return this; }
        }
        public System.Boolean LoadDefinition(DefinitionFileName definitionFileName)
        {
            return this.m_delegate.LoadDefinitionFile(definitionFileName);
        }
        public System.Boolean UnLoadDefinition(DefinitionFileName definitionFileName)
        {
            return this.m_delegate.UnLoadDefinitionFile(definitionFileName);
        }
        public System.Boolean ReLoadDefinitions()
        {
            return this.m_delegate.ReloadDefinitions();
        }
        public void RemoveDefinitions()
        {
            this.m_delegate.RemoveDefinitions();
            return;
        }
        public DefinitionFileRoot DefinitionFileRoot
        {
            get { return this.m_delegate.DefinitionFileRoot; }
            set { this.m_delegate.DefinitionFileRoot = value; }
        }
        public DefinitionFileName[] LoadedDefinitionFileNames
        {
            get
            {
                System.UInt16 size = this.m_delegate.NrOfDefinitionFiles;
                ArrayList m_data = new ArrayList(size);
                for (System.UInt16 idx = 0; idx < size; idx++)
                {
                    DefinitionFileName definitionFileName =
                        this.m_delegate.get_DefinitionFileName(idx);
                    m_data.Add(definitionFileName);
                }
                return (DefinitionFileName[])m_data.ToArray(typeof(DefinitionFileName));
            }
        }
        // Name of SUT
        public System.String ApplicationEntityName
        {
            get { return this.m_delegate.ApplicationEntityName; }
            set { this.m_delegate.ApplicationEntityName = value; }
        }
        // Version of SUT
        public System.String ApplicationEntityVersion
        {
            get { return this.m_delegate.ApplicationEntityVersion; }
            set { this.m_delegate.ApplicationEntityVersion = value; }
        }
        #endregion

        #region ISessionParameters
        public ISessionParameters ISessionParameters
        {
            get { return this; }
        }
        public System.DateTime Date
        {
            get
            {
                // Retrieve date as char* of format yyyyMMdd
                System.String dateString = this.m_delegate.Date;
                String format = "yyyyMMdd";
                return System.DateTime.ParseExact(dateString, format, null);
            }
            set
            {
                // The year in four digits, including the century.
                // The numeric month. Single-digit months will have a leading zero.
                // The day of the month. Single-digit days will have a leading zero.
                String format = "yyyyMMdd";
                String date = value.ToString(format);
                this.m_delegate.Date = date;
            }
        }
        public System.String SessionTitle
        {
            get { return this.m_delegate.SessionTitle; }
            set { this.m_delegate.SessionTitle = value; }
        }
        public System.UInt16 SessionId
        {
            get { return this.m_delegate.SessionId; }
            set { this.m_delegate.SessionId = value; }
        }
        public System.String SoftwareVersions
        {
            get { return this.m_delegate.SoftwareVersions; }
            set { this.m_delegate.SoftwareVersions = value; }
        }
        public System.String TestedBy
        {
            get { return this.m_delegate.TestedBy; }
            set { this.m_delegate.TestedBy = value; }
        }
        public System.Boolean StrictValidation
        {
            get { return this.m_delegate.StrictValidation; }
            set { this.m_delegate.StrictValidation = value; }
        }
        public System.Boolean ContinueOnError
        {
            get { return this.m_delegate.ContinueOnError; }
            set { this.m_delegate.ContinueOnError = value; }
        }
        static private Wrappers.StorageMode
            DvtkToWrapper(DicomValidationToolKit.Sessions.StorageMode value)
        {
            switch (value)
            {
                case DicomValidationToolKit.Sessions.StorageMode.AsDataSet:
                    return Wrappers.StorageMode.StorageModeAsDataSet;
                case DicomValidationToolKit.Sessions.StorageMode.AsMedia:
                    return Wrappers.StorageMode.StorageModeAsMedia;
                case DicomValidationToolKit.Sessions.StorageMode.NoStorage:
                    return Wrappers.StorageMode.StorageModeNoStorage;
                case DicomValidationToolKit.Sessions.StorageMode.TemporaryPixelOnly:
                    return Wrappers.StorageMode.StorageModeTemporaryPixelOnly;
                default:
                    throw new ApplicationException();
            }
        }
        static private DicomValidationToolKit.Sessions.StorageMode
            WrapperToDvtk(Wrappers.StorageMode value)
        {
            switch (value)
            {
                case Wrappers.StorageMode.StorageModeAsDataSet:
                    return DicomValidationToolKit.Sessions.StorageMode.AsDataSet;
                case Wrappers.StorageMode.StorageModeAsMedia:
                    return DicomValidationToolKit.Sessions.StorageMode.AsMedia;
                case Wrappers.StorageMode.StorageModeNoStorage:
                    return DicomValidationToolKit.Sessions.StorageMode.NoStorage;
                case Wrappers.StorageMode.StorageModeTemporaryPixelOnly:
                    return DicomValidationToolKit.Sessions.StorageMode.TemporaryPixelOnly;
                default:
                    throw new ApplicationException();
            }
        }
        public StorageMode StorageMode
        {
            get { return WrapperToDvtk(this.m_delegate.StorageMode); }
            set { this.m_delegate.StorageMode = DvtkToWrapper(value); }
        }
        public System.String Manufacturer
        {
            get { return this.m_delegate.Manufacturer; }
            set { this.m_delegate.Manufacturer = value; }
        }
        public System.String ModelName
        {
            get { return this.m_delegate.ModelName; }
            set { this.m_delegate.ModelName = value; }
        }
        #endregion

        // Test and communication settings
        private Sessions.Services.Dvt1ScriptSessionService m_dvt1ScriptSessionService = null;
        private Sessions.Services.Dvt1MediaSessionService m_dvt1MediaSessionService = null;
        //
        // Should be called by specialisation sub-class from the end of constructor.
        // Precondition: m_delegate should have been set by  sub-class.
        //
        virtual protected void _Initialize(Key key)
        {
            m_dvt1ScriptSessionService = new Sessions.Services.Dvt1ScriptSessionService(this, m_delegate);
            m_dvt1MediaSessionService = new Sessions.Services.Dvt1MediaSessionService(this, m_delegate);
        }
        public Session(Key key)
        {
            if (key != null) DicomValidationContext.Sessions.Add(key, this);
        }
        public String SessionFilename
        {
            get { return this.m_delegate.SessionFilename; }
        }
        public Dvt1.IDvt1ScriptSessionCommands Dvt1ScriptCommands
        {
            get { return m_dvt1ScriptSessionService; }
        }
        public Dvt1.IDvt1MediaSessionCommands Dvt1MediaCommands
        {
            // TODO: Add settings for LogScpThread?
            // TODO: Add settings for AutoCreateDirectory?
            // TODO: Add Dvt1EmulatorCommands?
            get { return m_dvt1MediaSessionService; }
        }
        public string ResultsRootDir
        {
            get { return this.m_delegate.ResultsRoot; }
            set { this.m_delegate.ResultsRoot = value; }
        }
    }
    /// <summary>
    /// Summary description for Session.
    /// </summary>
    public class NetworkSession
        : Session
        , INetworkSession
        , ISessionManagement
        , IDimseMessaging
        , IDimseParameters
        , IDimseValidation
    {
        new protected Wrappers.MScriptSession m_delegate;

        #region ISessionManagement
        public ISessionManagement SessionManagement
        {
            get { return this; }
        }
        public void SaveToFile(FileName fileName)
        {
            // TODO: Save to file.
        }
        #endregion

        #region IDimseMessaging
        public IDimseMessaging DimseMessaging
        {
            get { return this; }
        }
        public DimseStatusCode Send(DicomMessage dicomMessage)
        {
            DimseStatusCode statusCode = 0;

            // TODO Send
            int dvtStatusCode = 0;
            switch (dvtStatusCode)
            {
                case 0:
                    throw new DicomValidationToolKit.Dul.Services.Exceptions.AssociationReleasedException();
                case 1:
                    throw new DicomValidationToolKit.Sockets.Exceptions.SocketConnectionLostException();
                case 2:
                    throw new DicomValidationToolKit.Sockets.Exceptions.NoSocketConnectionException();
                case 3:
                    throw new DicomValidationToolKit.Dul.Services.Exceptions.NoAssociationException();
                default:
                    break;
            }
            return statusCode;
        }
        /*
        public void ValidateAndSend(DicomMessage dicomMessage)
        {
            DicomMessage referenceDicomMessage = null;
            bool useDefinitions = true;
            this.Validate(dicomMessage, referenceDicomMessage, useDefinitions);
            // TODO Send
        }
        */
        public DimseStatusCode Receive(out DicomMessage dicomMessage)
        {
            DimseStatusCode statusCode = 0;
            dicomMessage = null;
            //            int dvtStatusCode = 0;
            //            switch (dvtStatusCode)
            //            {
            //                case 0:
            //                    throw new DicomValidationToolKit.Dul.Services.Exceptions.AssociationReleasedException();
            //                case 1:
            //                    throw new DicomValidationToolKit.Dul.Services.Exceptions.NoAssociationException();
            //                case 2:
            //                    throw new DicomValidationToolKit.Sockets.Exceptions.NoSocketConnectionException();
            //                case 3:
            //                    throw new DicomValidationToolKit.Sockets.Exceptions.SocketConnectionLostException();
            //                default:
            //                    break;
            //            }
            //            // Receive commandset from unmanaged code.
            //            DimseCommand commandField = DimseCommand.UNDEFINED;
            //            // TODO determine commandfield
            //            bool bDataSetTypeAvailable = false;
            //            // Determine if data set type is available.
            //            if (bDataSetTypeAvailable) // value is 0101H
            //            {
            //                // determine affected or requested sop class uid
            //                System.String sopClassUID = null;
            //                // TODO determine affected sop class UID of message
            //                // receive the dataset
            //                IodId iodId = new IodId(commandField, sopClassUID);
            ////                dicomMessage = new DicomMessage(commandField, iodId);
            //                dicomMessage = new DicomMessage(commandField);
            //            }
            //            else
            //            {
            //                dicomMessage = new DicomMessage(commandField);
            //            }
            //            // fill new message with all received information.
            //            if (dicomMessage == null) throw new Exception();
            return statusCode;
        }
        /*
        public void ReceiveAndValidate(out DicomMessage dicomMessage)
        {
            dicomMessage = null;
            DicomMessage referenceDicomMessage = null;
            bool useDefinitions = true;
            this.Receive(out dicomMessage);
            this.Validate(dicomMessage, referenceDicomMessage, useDefinitions);
        }
        */
        public DimseStatusCode Receive(out GeneralMessage generalMessage)
        {
            DimseStatusCode statusCode = 0;
            generalMessage = null;
            //            int dvtStatusCode = 0;
            //            switch (dvtStatusCode)
            //            {
            //                case 1:
            //                    throw new DicomValidationToolKit.Sockets.Exceptions.NoSocketConnectionException();
            //                case 2:
            //                    throw new DicomValidationToolKit.Sockets.Exceptions.SocketConnectionLostException();
            //                default:
            //                    break;
            //            }
            //            DicomMessage dicomMessage = null;
            //            DulMessage dulMessage = null;
            //            // Receive commandset from unmanaged code.
            //            DimseCommand commandField = DimseCommand.UNDEFINED;
            //            // TODO determine commandfield
            //            bool bDataSetTypeAvailable = false;
            //            // Determine if data set type is available.
            //            if (bDataSetTypeAvailable) // value is 0101H
            //            {
            //                // determine affected or requested sop class uid
            //                UI sopClassUID = null;
            //                // TODO determine affected sop class UID of message
            //                // receive the dataset
            //                IodId iodId = new IodId(commandField, sopClassUID);
            //                dicomMessage = new DicomMessage(commandField, iodId);
            //            }
            //            else
            //            {
            //                dicomMessage = new DicomMessage(commandField);
            //            }
            //            // fill new message with all received information.
            //            generalMessage = new GeneralMessage(dicomMessage, dulMessage);
            return statusCode;
        }
        #endregion

        #region IDimseParameters
        public IDimseParameters DimseParameters
        {
            get { return this; }
        }
        public System.Boolean AutoType2Attributes
        {
            get { return this.m_delegate.AutoType2Attributes; }
            set { this.m_delegate.AutoType2Attributes = value; }
        }
        public System.Boolean DefineSqLength
        {
            get { return this.m_delegate.DefineSqLength; }
            set { this.m_delegate.DefineSqLength = value; }
        }
        public System.Boolean AddGroupLength
        {
            get { return this.m_delegate.AddGroupLength; }
            set { this.m_delegate.AddGroupLength = value; }
        }
        #endregion

        #region IDimseValidation
        public IDimseValidation DimseValidation
        {
            get { return this; }
        }
        // TODO : Add toggle/selection to specify definition files used in validation step.
        // TODO : Provide validation against definition only. Against definition or against reference.
        public ValidationReturnCode Validate(
            DicomMessage dicomMessage,
            DicomMessage referenceDicomMessage,
            System.Boolean useDefinitions)
        {
            ValidationReturnCode endResult = ValidationReturnCode.Success;
            ValidationReturnCode result;
            // perform validation of the message against its definition
            // and against the references.
            result = Validate(dicomMessage.CommandSet, referenceDicomMessage.CommandSet, useDefinitions);
            if (result > endResult) endResult = result;
            result = Validate(dicomMessage.DataSet, referenceDicomMessage.DataSet, useDefinitions);
            if (result > endResult) endResult = result;
            return endResult;
        }
        public ValidationReturnCode Validate(
            DataSet dataSet,
            DataSet referenceDataSet,
            System.Boolean useDefinitions)
        {
            // Allow null arguments
            // TODO
            return ValidationReturnCode.Success;
        }
        public ValidationReturnCode Validate(
            CommandSet commandSet,
            CommandSet referenceCommandSet,
            System.Boolean useDefinitions)
        {
            // Allow null arguments
            // TODO
            return ValidationReturnCode.Success;
        }
        public void EnableStrictValidation()
        {
            this.ISessionParameters.StrictValidation = true;
        }
        public void DisableStrictValidation()
        {
            this.ISessionParameters.StrictValidation = false;
        }
        #endregion

        private DulService m_DulService = null;
        // Test and communication settings
        private SocketService m_socketService = null;

        override protected void _Initialize(Key key)
        {
            base._Initialize(key);
            m_DulService = new DulService(this, m_delegate);
            m_socketService = new SocketService(this, m_delegate);
        }
        public NetworkSession(Key key)
            : base(key)
        {
            m_delegate = new Wrappers.MScriptSession();
            base.m_delegate = m_delegate;
            _Initialize(key);
        }
        internal NetworkSession(Wrappers.MBaseSession sessionDelegate, Key key)
            : base(key)
        {
            // Check type
            m_delegate = (Wrappers.MScriptSession)sessionDelegate;
            base.m_delegate = m_delegate;
            _Initialize(key);
        }
        static public NetworkSession LoadFromFile(FileName fileName)
        {
            Session session = SessionFactory.TheInstance.Load(fileName);
            // Check type
            if (!(session is NetworkSession)) throw new ApplicationException();
            return (NetworkSession)session;
        }
        public IDulMessaging DulMessaging
        {
            get { return m_DulService; }
        }
        public IDulValidation DulValidation
        {
            get { return m_DulService; }
        }
        public ISocketParameters SocketParameters
        {
            get { return m_socketService; }
        }
        public IDulParameters DulParameters
        {
            get { return m_DulService; }
        }
        public string DicomScriptRootDir
        {
            get { return this.m_delegate.DicomScriptRoot; }
            set { this.m_delegate.DicomScriptRoot = value; }
        }
    }
    /// <summary>
    /// Summary description for Session.
    /// </summary>
    public class MediaSession
        : Session
        , IMediaSession
        , ISessionManagement
    {
        new protected Wrappers.MMediaSession m_delegate;

        #region ISessionManagement
        public ISessionManagement SessionManagement
        {
            get { return this; }
        }
        public void SaveToFile(FileName fileName)
        {
            // TODO: Save to file.
        }
        #endregion

        // Media services
        // Validation services
        private MediaService m_mediaService = null;

        override protected void _Initialize(Key key)
        {
            base._Initialize(key);
            m_mediaService = new MediaService(this);
        }
        public MediaSession(Key key)
            : base(key)
        {
            m_delegate = new Wrappers.MMediaSession();
            base.m_delegate = m_delegate;
            _Initialize(key);
        }
        internal MediaSession(Wrappers.MBaseSession sessionDelegate, Key key)
            : base(key)
        {
            // Check type
            m_delegate = (Wrappers.MMediaSession)sessionDelegate;
            base.m_delegate = m_delegate;
            _Initialize(key);
        }
        static public MediaSession LoadFromFile(FileName fileName)
        {
            Session session = SessionFactory.TheInstance.Load(fileName);
            // Check type
            if (!(session is MediaSession)) throw new ApplicationException();
            return (MediaSession)session;
        }
        public IMediaStorage MediaStorage
        {
            get { return m_mediaService; }
        }
        public IMediaValidation MediaValidation
        {
            get { return m_mediaService; }
        }
    }
    /// <summary>
    /// Summary description for SessionFactory "Singleton".
    /// </summary>
    public sealed class SessionFactory
    {
        private SessionFactory() { }
        public static readonly SessionFactory TheInstance = new SessionFactory();
        public Session Load(FileName fileName)
        {
            if (fileName == null) throw new ArgumentNullException();
            Session session = null;
            Wrappers.MBaseSession session_delegate =
                Wrappers.MSessionFactory.Load(fileName);
            switch (session_delegate.SessionType)
            {
                case Wrappers.SessionType.SessionTypeEmulator:
                    /* TODO: Add emulator from file.
                    session = new EmulatorSession(session_delegate, null);
                    */
                    break;
                case Wrappers.SessionType.SessionTypeMedia:
                    session = new MediaSession(session_delegate, null);
                    break;
                case Wrappers.SessionType.SessionTypeScript:
                    session = new NetworkSession(session_delegate, null);
                    break;
                case Wrappers.SessionType.SessionTypeUnknown:
                    throw new ApplicationException();
            }
            return session;
        }
    }
}
namespace DicomValidationToolKit.Sessions.Services
{
    using Key = System.String;
    using System.Collections;
    using DicomValidationToolKit;
    using DefinitionFileRoot = System.String;
    using DefinitionFileName = System.String;
    using DicomValidationToolKit.Sessions;
    using FileName = System.String;
    public interface ISessionManagement
    {
        void SaveToFile(FileName fileName);
    }
}
namespace DicomValidationToolKit.Sessions.Settings
{
}