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

namespace Dvtk.Sessions
{
    using SessionFileName = System.String;
    using MediaFileName = System.String;
    using System;
    using System.Threading;

    [System.Flags]
    internal enum StorageEmulatorFlags : ushort
    {
        SingleAssociation = 1 << 0,
        MultipleAssociations = 1 << 1,
        ValidateOnImport = 1 << 2,
        DataUnderNewStudy = 1 << 3,
        Repeat = 1 << 4,
    }

    /// <summary>
    /// Access to emulator commands.
    /// </summary>
    public interface IEmulatorCommands
    {
        /// <summary>
        /// SCP Emulator.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Runs the SCP emulator currently set by <see cref="DvtkData.Results.ScpEmulatorType"/>.
        /// </p>
        /// <p>
        /// <see cref="TerminateConnection"/> can be used to stop the emulator.
        /// </p>
        /// </remarks>
        /// <returns></returns>
        System.Boolean EmulateSCP();
        /// <summary>
        /// Asynchronously begin EmulateSCP.
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        System.IAsyncResult BeginEmulateSCP(AsyncCallback cb);
        /// <summary>
        /// End asynchronous EmulateSCP.
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        bool EndEmulateSCP(IAsyncResult ar);
        /// <summary>
        /// Storage SCU Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Storage SCU for the Storage Definition File(s) loaded.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// storage SCU.
        /// </p>
        /// <p>
        /// The Storage Emulator SCU supports the Verification SOP Class.
        /// </p>
        /// </remarks>
        /// <param name="mediaFileNames">Media files to be send to dicom storage SCP.</param>
        /// <param name="multipleAssocations"></param>
        /// <param name="validateOnImport"></param>
        /// <param name="dataUnderNewStudy"></param>
        /// <param name="nrOfRepetitions"></param>
        /// <returns><see langword="true"/> if emulation succeeded.</returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>mediaFileNames</c> is a <see langword="null"/> reference.</exception>
        System.Boolean EmulateStorageSCU(
            MediaFileName[] mediaFileNames,
            System.Boolean multipleAssocations,
            System.Boolean validateOnImport,
            System.Boolean dataUnderNewStudy,
            System.UInt16 nrOfRepetitions);

        /// <summary>
        /// Asynchronously begin EmulateStorageSCU.
        /// </summary>
        /// <param name="mediaFileNames"></param>
        /// <param name="multipleAssociations"></param>
        /// <param name="validateOnImport"></param>
        /// <param name="dataUnderNewStudy"></param>
        /// <param name="nrOfRepetitions"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        System.IAsyncResult BeginEmulateStorageSCU(
            MediaFileName[] mediaFileNames,
            System.Boolean multipleAssociations,
            System.Boolean validateOnImport,
            System.Boolean dataUnderNewStudy,
            System.UInt16 nrOfRepetitions,
            AsyncCallback cb);
        /// <summary>
        /// End asynchronous EmulateStorageSCU.
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        bool EndEmulateStorageSCU(IAsyncResult ar);

        /// <summary>
        /// Storage Commitment SCU Emulator - emulate command.
        /// </summary>
        /// <summary>
        /// Send the Storage Commitment message - use the storage commitment details
        /// built up during any previous SendModalityImagesStored() operations.
        /// The N-ACTION-RQ will be sent to the Image Manager. The value
        /// of the awaitNEventReport boolean will determine whether this call waits for
        /// the N-EVENT-REPORT-RQ from the Image Manager or not.
        /// 
        /// N-EVENT-REPORT-RQ is expected over the same association or not. True - wait for
        /// N-EVENT-REPORT-RQ on this association. False - N-EVENT-REPORT-RQ will be sent to
        /// the Acquisition Modality actor over a separate association.</summary>
        /// <param name="delay">Time (in Seconds) to wait for the NEventReport to be sent
        /// over the same association - applicable only if awaitNEventReport = true.</param>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Storage Commitment SCU.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// Storage Commitment SCU.
        /// </p>
        /// <p>
        /// The Storage Commitment SCU supports the Storage Commitment Push SOP Class.
        /// </p>
        /// </remarks>
        /// <returns></returns>
        System.Boolean EmulateStorageCommitSCU(
            System.Int16 delay);
        /// <summary>
        /// Asynchronously begin EmulateStorageCommitSCU.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        System.IAsyncResult BeginEmulateStorageCommitSCU(
            System.Int16 delay,
            AsyncCallback cb);
        /// <summary>
        /// End asynchronous EmulateSCP.
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        bool EndEmulateStorageCommitSCU(IAsyncResult ar);

        /// <summary>
        /// Verification SCU Emulator - emulate command.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Set up DVT to perform the role of Verification SCU.
        /// </p>
        /// <p>
        /// Before running this emulator, load the proper definition files corresponding to the
        /// verification SCU.
        /// </p>
        /// <p>
        /// The verification SCU supports the Verification SOP Class.
        /// </p>
        /// </remarks>
        /// <returns></returns>
        System.Boolean EmulateVerificationSCU();
        /// <summary>
        /// Asynchronously begin EmulateVerificationSCU.
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        System.IAsyncResult BeginEmulateVerificationSCU(AsyncCallback cb);
        /// <summary>
        /// End asynchronous EmulateSCP.
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        bool EndEmulateVerificationSCU(IAsyncResult ar);
        /// <summary>
        /// Can be used to stop an (SCP) emulator.
        /// </summary>
        /// <returns></returns>
        System.Boolean TerminateConnection();
        /// <summary>
        /// Can be used to send Abort command from (SCP) emulator.
        /// </summary>
        /// <returns></returns>
        System.Boolean AbortEmulation();
        /// <summary>
        /// Can be used to send Abort command from (SCU) emulator.
        /// </summary>
        /// <returns></returns>
        System.Boolean AbortEmulationFromSCU();
    }

    /// <summary>
    /// The Printer SOP Class settings.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Allows retrieval of the printer status.
    /// </p>
    /// <p>
    /// Allows manipulation of the Printer Status.
    /// </p>
    /// </remarks>
    public interface IPrinter
    {
        /// <summary>
        /// Printer device status.
        /// </summary>
        PrinterStatus Status { get; }
        /// <summary>
        /// Additional information about Printer Status.
        /// </summary>
        System.String StatusInfo { get; }
        /// <summary>
        /// User defined name identifying the printer.
        /// </summary>
        System.String PrinterName { get; }
        /// <summary>
        /// Manufacturer of the printer.
        /// </summary>
        System.String Manufacturer { get; }
        /// <summary>
        /// Manufacturer's model number of the printer.
        /// </summary>
        System.String ManufacturerModelName { get; }
        /// <summary>
        /// Manufacturer's serial number of the printer.
        /// </summary>
        System.String DeviceSerialNumber { get; }
        /// <summary>
        /// Manufacturer's designation of software version of the printer.
        /// </summary>
        System.String SoftwareVersions { get; }
        /// <summary>
        /// Date when the printer was last calibrated.
        /// </summary>
        System.DateTime DateOfLastCalibration { get; }
        /// <summary>
        /// Time when the printer was last calibrated.
        /// </summary>
        System.DateTime TimeOfLastCalibration { get; }
        /// <summary>
        /// Apply a new printer status and corresponding status info.
        /// </summary>
        /// <param name="printerStatus">Printer device status.</param>
        /// <param name="statusInfo">Additional information about Printer Status.</param>
        /// <param name="sendStatusEvent">Indicates the status event should be sent to SUT</param>
        /// <exception cref="System.ArgumentNullException">Argument <c>statusInfo</c> is a <see langword="null"/> reference.</exception>
        void ApplyStatus(PrinterStatus printerStatus, System.String statusInfo, bool sendStatusEvent);
        /// <summary>
        /// Defined Terms for Printer Status Info.
        /// </summary>
        /// <remarks>
        /// These defined terms are loaded via the definition files.
        /// </remarks>
        System.String[] StatusInfoDefinedTerms { get; }
    }

    /// <summary>
    /// Printer device status.
    /// </summary>
    public enum PrinterStatus
    {
        /// <summary>
        /// NORMAL
        /// </summary>
        NORMAL,
        /// <summary>
        /// WARNING
        /// </summary>
        WARNING,
        /// <summary>
        /// FAILURE
        /// </summary>
        FAILURE,
    }

    internal class Printer : IPrinter
    {
        internal Printer(Wrappers.MEmulatorSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException("adaptee");
            this.m_adaptee = adaptee;
        }
        private Wrappers.MEmulatorSession m_adaptee = null;

        #region IPrinter Members
        /// <summary>
        /// <see cref="IPrinter.Manufacturer"/>
        /// </summary>
        public System.String Manufacturer
        {
            get
            {
                return m_adaptee.MPrinter.Manufacturer;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.ManufacturerModelName"/>
        /// </summary>
        public System.String ManufacturerModelName
        {
            get
            {
                return m_adaptee.MPrinter.ModelName;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.PrinterName"/>
        /// </summary>
        public System.String PrinterName
        {
            get
            {
                return m_adaptee.MPrinter.Name;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.DeviceSerialNumber"/>
        /// </summary>
        public System.String DeviceSerialNumber
        {
            get
            {
                return m_adaptee.MPrinter.SerialNumber;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.TimeOfLastCalibration"/>
        /// </summary>
        public System.DateTime TimeOfLastCalibration
        {
            get
            {
                return m_adaptee.MPrinter.CalibrationTime;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.SoftwareVersions"/>
        /// </summary>
        public System.String SoftwareVersions
        {
            get
            {
                return m_adaptee.MPrinter.SoftwareVersions;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.DateOfLastCalibration"/>
        /// </summary>
        public System.DateTime DateOfLastCalibration
        {
            get
            {
                return m_adaptee.MPrinter.CalibrationDate;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.Status"/>
        /// </summary>
        public PrinterStatus Status
        {
            get
            {
                System.String printerStatus = m_adaptee.MPrinter.Status;
                if (System.String.Compare(printerStatus, "NORMAL") == 0) return PrinterStatus.NORMAL;
                if (System.String.Compare(printerStatus, "WARNING") == 0) return PrinterStatus.WARNING;
                if (System.String.Compare(printerStatus, "FAILURE") == 0) return PrinterStatus.FAILURE;
                System.Diagnostics.Trace.Assert(false);
                return PrinterStatus.FAILURE;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.StatusInfo"/>
        /// </summary>
        public System.String StatusInfo
        {
            get
            {
                return m_adaptee.MPrinter.StatusInfo;
            }
        }
        /// <summary>
        /// <see cref="IPrinter.ApplyStatus"/>
        /// </summary>
        public void ApplyStatus(PrinterStatus printerStatus, System.String statusInfo, bool sendStatusEvent)
        {
            if (statusInfo == null) throw new System.ArgumentNullException("statusInfo");
            switch (printerStatus)
            {
                case PrinterStatus.NORMAL: this.m_adaptee.MPrinter.Status = "NORMAL"; break;
                case PrinterStatus.WARNING: this.m_adaptee.MPrinter.Status = "WARNING"; break;
                case PrinterStatus.FAILURE: this.m_adaptee.MPrinter.Status = "FAILURE"; break;
                default: System.Diagnostics.Trace.Assert(false); break;
            }
            this.m_adaptee.MPrinter.StatusInfo = statusInfo;

            if (sendStatusEvent)
            {
                this.m_adaptee.SendStatusEvent();
            }
        }
        /// <summary>
        /// <see cref="IPrinter.StatusInfoDefinedTerms"/>
        /// </summary>
        public System.String[] StatusInfoDefinedTerms
        {
            get
            {
                System.String[] statusInfoDefinedTerms;
                System.UInt32 nrOfStatusInfoDefinedTerms =
                    Wrappers.MPrinter.getNrOfStatusInfoDefinedTerms();
                statusInfoDefinedTerms = new System.String[nrOfStatusInfoDefinedTerms];
                for (System.UInt32 index = 0; index < nrOfStatusInfoDefinedTerms; index++)
                {
                    statusInfoDefinedTerms[index] = Wrappers.MPrinter.getStatusInfoDefinedTerm(index);
                }
                return statusInfoDefinedTerms;
            }
        }
        #endregion

    }

    /// <summary>
    /// Access to printer settings.
    /// </summary>
    public interface IPrinterSettings
    {
        /// <summary>
        /// Provide a access handle to the printer.
        /// </summary>
        IPrinter Printer { get; }
    }

    /// <summary>
    /// Access to configuration of supported transfer syntaxes.
    /// </summary>
    /// <remarks>
    /// Specifies the transfer syntaxes as supported by the emulator for incomming messages.
    /// </remarks>
    public interface IConfigurableSupportedTransferSyntaxes
    {
        /// <summary>
        /// Supported transfer syntax settings.
        /// </summary>
        ISupportedTransferSyntaxSettings SupportedTransferSyntaxSettings { get; }
    }

    /// <summary>
    /// Acces to supported transfer syntax settings.
    /// </summary>
    public interface ISupportedTransferSyntaxSettings
    {
        /// <summary>
        /// List of supported transfer syntaxes.
        /// </summary>
        DvtkData.Dul.TransferSyntaxList SupportedTransferSyntaxes { get; }
    }
    internal class SupportedTransferSyntaxSettings
        : ISupportedTransferSyntaxSettings
    {
        public DvtkData.Dul.TransferSyntaxList SupportedTransferSyntaxes
        {
            get { return _TransferSyntaxList; }
        }
        private DvtkData.Dul.TransferSyntaxList _TransferSyntaxList =
            new DvtkData.Dul.TransferSyntaxList();

        internal SupportedTransferSyntaxSettings(Wrappers.MEmulatorSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException("adaptee");
            this.m_adaptee = adaptee;

            // load the supported list from the values in the session
            System.UInt16 nrOfSupportedTransferSyntaxes = this.m_adaptee.NrOfSupportedTransferSyntaxes;
            for (System.UInt16 index = 0; index < nrOfSupportedTransferSyntaxes; index++)
            {
                this._TransferSyntaxList.Add(
                    new DvtkData.Dul.TransferSyntax(this.m_adaptee.get_SupportedTransferSyntax(index))
                    );
            }
            // subscribe to contents changes to call underlying MBaseSession methods.
            this._TransferSyntaxList.ListChanged +=
                new System.ComponentModel.ListChangedEventHandler(_TransferSyntaxList_ListChanged);
        }
        private Wrappers.MBaseSession m_adaptee;

        private void _TransferSyntaxList_ListChanged(
            object sender,
            System.ComponentModel.ListChangedEventArgs e)
        {
            // Refresh underlying list
            this.m_adaptee.DeleteSupportedTransferSyntaxes();
            foreach (DvtkData.Dul.TransferSyntax ts in this._TransferSyntaxList)
            {
                this.m_adaptee.AddSupportedTransferSyntax(ts.UID);
            }
        }
    }

    /// <summary>
    /// Summary description for Session.
    /// </summary>
    public class EmulatorSession
        : Session
        , IDataSetEncodingSettings
        , ISecure
        , IConfigurableDvt
        , IConfigurableSut
        , IEmulatorCommands
        , IPrinterSettings
        , IConfigurableSupportedTransferSyntaxes
        , IValidationSettings
    {
        /// <summary>
        /// <see cref="IConfigurableSupportedTransferSyntaxes.SupportedTransferSyntaxSettings"/>
        /// </summary>
        public ISupportedTransferSyntaxSettings SupportedTransferSyntaxSettings
        {
            get
            {
                return this.m_supportedTransferSyntaxSettings;
            }
        }
        private SupportedTransferSyntaxSettings m_supportedTransferSyntaxSettings = null;
        //        new protected Wrappers.MEmulatorSession m_adaptee;

        #region IConfigurableDvt
        /// <summary>
        /// Settings specific to the DVT machine node.
        /// </summary>
        public IDvtSystemSettings DvtSystemSettings
        {
            get
            {
                return this.m_dvtSystemSettings;
            }
        }
        #endregion IConfigurableDvt

        #region IConfigurableSut
        /// <summary>
        /// Settings specific to the SUT machine node.
        /// </summary>
        public ISutSystemSettings SutSystemSettings
        {
            get
            {
                return this.m_sutSystemSettings;
            }
        }
        #endregion IConfigurableSut

        #region ISecure
        /// <summary>
        /// <see cref="ISecure.SecuritySettings"/>
        /// </summary>
        public ISecuritySettings SecuritySettings
        {
            get
            {
                return this.m_securitySettings;
            }
        }
        private Dvtk.Security.ICertificateHandling _ICertificateHandling = null;


        /// <summary>
        /// Copy all settings from the supplied EmulatorSession to this EmulatorSession.
        /// </summary>
        /// <param name="emulatorSession">The EmulatorSession to copy the settings from.</param>
        public virtual void CopySettingsFrom(EmulatorSession emulatorSession)
        {
            base.CopySettingsFrom(emulatorSession);

            AddGroupLength = emulatorSession.AddGroupLength;
            AutoType2Attributes = emulatorSession.AutoType2Attributes;
            DefineSqLength = emulatorSession.DefineSqLength;

            // DvtSystemSettings.
            DvtSystemSettings.AeTitle = emulatorSession.DvtSystemSettings.AeTitle;
            DvtSystemSettings.ImplementationClassUid = emulatorSession.DvtSystemSettings.ImplementationClassUid;
            DvtSystemSettings.ImplementationVersionName = emulatorSession.DvtSystemSettings.ImplementationVersionName;
            DvtSystemSettings.MaximumLengthReceived = emulatorSession.DvtSystemSettings.MaximumLengthReceived;
            DvtSystemSettings.Port = emulatorSession.DvtSystemSettings.Port;
            DvtSystemSettings.SocketTimeout = emulatorSession.DvtSystemSettings.SocketTimeout;

            // Printer. All properties are read-only. The only two properties that can indirectly be set
            // by a method call are Status and StatusInfo.
            Printer.ApplyStatus(emulatorSession.Printer.Status, emulatorSession.Printer.StatusInfo, false);

            ScpEmulatorType = emulatorSession.ScpEmulatorType;
            ScuEmulatorType = emulatorSession.ScuEmulatorType;

            if (emulatorSession.SecuritySettings.SecureSocketsEnabled)
            {
                // SecuritySettings.
                SecuritySettings.CacheTlsSessions = emulatorSession.SecuritySettings.CacheTlsSessions;
                SecuritySettings.CertificateFileName = emulatorSession.SecuritySettings.CertificateFileName;
                SecuritySettings.CheckRemoteCertificate = emulatorSession.SecuritySettings.CheckRemoteCertificate;
                SecuritySettings.CipherFlags = emulatorSession.SecuritySettings.CipherFlags;
                SecuritySettings.CredentialsFileName = emulatorSession.SecuritySettings.CredentialsFileName;
                SecuritySettings.SecureSocketsEnabled = emulatorSession.SecuritySettings.SecureSocketsEnabled;
                SecuritySettings.TlsCacheTimeout = emulatorSession.SecuritySettings.TlsCacheTimeout;
                SecuritySettings.TlsPassword = emulatorSession.SecuritySettings.TlsPassword;
                SecuritySettings.MaxTlsVersionFlags = emulatorSession.SecuritySettings.MaxTlsVersionFlags;
                SecuritySettings.MinTlsVersionFlags = emulatorSession.SecuritySettings.MinTlsVersionFlags;
            }
            foreach (DvtkData.Dul.TransferSyntax transferSyntax in emulatorSession.SupportedTransferSyntaxSettings.SupportedTransferSyntaxes)
            {
                SupportedTransferSyntaxSettings.SupportedTransferSyntaxes.Add(transferSyntax);
            }

            // SutSystemSettings.
            SutSystemSettings.AeTitle = emulatorSession.SutSystemSettings.AeTitle;
            SutSystemSettings.CommunicationRole = emulatorSession.SutSystemSettings.CommunicationRole;
            SutSystemSettings.HostName = emulatorSession.SutSystemSettings.HostName;
            SutSystemSettings.ImplementationClassUid = emulatorSession.SutSystemSettings.ImplementationClassUid;
            SutSystemSettings.ImplementationVersionName = emulatorSession.SutSystemSettings.ImplementationVersionName;
            SutSystemSettings.MaximumLengthReceived = emulatorSession.SutSystemSettings.MaximumLengthReceived;
            SutSystemSettings.Port = emulatorSession.SutSystemSettings.Port;
        }



        /// <summary>
        /// <see cref="ISecure.CreateSecurityCertificateHandler"/>
        /// </summary>
        public Dvtk.Security.ICertificateHandling CreateSecurityCertificateHandler()
        {
            if (_ICertificateHandling != null)
                throw new System.ApplicationException(
                    "Existing certificate handler has not been disposed yet.");
            _ICertificateHandling = new Dvtk.Security.CertificateHandling(this);
            return _ICertificateHandling;
        }
        private Dvtk.Security.ICredentialHandling _ICredentialHandling = null;
        /// <summary>
        /// <see cref="ISecure.DisposeSecurityCertificateHandler"/>
        /// </summary>
        public void DisposeSecurityCertificateHandler()
        {
            _ICertificateHandling = null;
        }
        /// <summary>
        /// <see cref="ISecure.CreateSecurityCredentialHandler"/>
        /// </summary>
        public Dvtk.Security.ICredentialHandling CreateSecurityCredentialHandler()
        {
            if (_ICredentialHandling != null)
                throw new System.ApplicationException(
                    "Existing credential handler has not been disposed yet.");
            _ICredentialHandling = new Dvtk.Security.CredentialHandling(this);
            return _ICredentialHandling;
        }
        /// <summary>
        /// <see cref="ISecure.DisposeSecurityCredentialHandler"/>
        /// </summary>
        public void DisposeSecurityCredentialHandler()
        {
            _ICredentialHandling = null;
        }
        #endregion ISecure

        #region IDataSetEncodingSettings
        /// <summary>
        /// <see cref="IDataSetEncodingSettings.AutoType2Attributes"/>
        /// </summary>
        public System.Boolean AutoType2Attributes
        {
            get
            {
                return this.m_adaptee.AutoType2Attributes;
            }
            set
            {
                this.m_adaptee.AutoType2Attributes = value;
            }
        }
        /// <summary>
        /// <see cref="IDataSetEncodingSettings.DefineSqLength"/>
        /// </summary>
        public System.Boolean DefineSqLength
        {
            get
            {
                return this.m_adaptee.DefineSqLength;
            }
            set
            {
                this.m_adaptee.DefineSqLength = value;
            }
        }
        /// <summary>
        /// <see cref="IDataSetEncodingSettings.AddGroupLength"/>
        /// </summary>
        public System.Boolean AddGroupLength
        {
            get
            {
                return this.m_adaptee.AddGroupLength;
            }
            set
            {
                this.m_adaptee.AddGroupLength = value;
            }
        }
        #endregion

        private Wrappers.MEmulatorSession m_adaptee = null;
        override internal Wrappers.MBaseSession m_MBaseSession
        {
            get { return m_adaptee; }
        }
        internal Wrappers.MEmulatorSession m_MEmulatorSession
        {
            get { return m_adaptee; }
        }
        //
        // Touch the AppUnloadListener abstract class to trigger its static-constructor.
        //
        static EmulatorSession()
        {
            AppUnloadListener.Touch();
        }
        /// <summary>
        /// Emulation for Verification, Storage and Print SOP Classes.
        /// </summary>
        /// <remarks>
        /// The Dicom Validation Tool (DVT) can be used as either 
        /// Service Class User (SCU) or Service Class Provider (SCP) with a direct 
        /// connection to the System Under Test (SUT)Product (via TCP/IP). 
        /// DVT acts as an emulator for the DICOM Service classes being tested. 
        /// DVT can also create and validate DICOM media files.
        /// </remarks>
        public EmulatorSession()
        {
            m_adaptee = new Wrappers.MEmulatorSession();
            Wrappers.MDisposableResources.AddDisposable(m_adaptee);
            m_printer = null;
            _Initialize();
        }
        /// <summary>
        /// Finalizer
        /// </summary>
        ~EmulatorSession()
        {
            Wrappers.MDisposableResources.RemoveDisposable(m_adaptee);
            m_adaptee = null;
        }
        private Printer m_printer;

        override internal void _Initialize()
        {
            base._Initialize();
            this.m_supportedTransferSyntaxSettings =
                new SupportedTransferSyntaxSettings((Wrappers.MEmulatorSession)this.m_adaptee);
        }
        internal EmulatorSession(Wrappers.MBaseSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException("adaptee");
            // Check type
            m_adaptee = (Wrappers.MEmulatorSession)adaptee;
            Wrappers.MDisposableResources.AddDisposable(m_adaptee);
            _Initialize();
        }
        /// <summary>
        /// Load a new session from file.
        /// </summary>
        /// <param name="sessionFileName">file with extension <c>.ses</c>.</param>
        /// <returns>new session</returns>
        /// <remarks>
        /// Session settings may be written to a file with extension <c>.ses</c> by
        /// means of <see cref="Dvtk.Sessions.ISessionFileManagement"/>.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument <c>sessionFileName</c> is a <see langword="null"/> reference.</exception>
        static public EmulatorSession LoadFromFile(SessionFileName sessionFileName)
        {
            if (sessionFileName == null) throw new System.ArgumentNullException("sessionFileName");
            Session session = SessionFactory.TheInstance.Load(sessionFileName);
            System.Diagnostics.Trace.Assert(session is EmulatorSession);
            return (EmulatorSession)session;
        }

        /// <summary>
        /// Delay Before NEventReport command
        /// </summary>
        public System.UInt16 DelayBeforeNEventReport
        {
            get
            {
                return this.m_adaptee.DelayBeforeNEventReport;
            }
            set
            {
                this.m_adaptee.DelayBeforeNEventReport = value;
            }
        }

        /// <summary>
        ///  Send commit report for all images or per image
        /// </summary>
        public System.Boolean AcceptDuplicateImage
        {
            set
            {
                this.m_adaptee.AcceptDuplicateImage = value;
            }
        }

        /// <summary>
        ///  Store only C-Store-Req objects
        /// </summary>
        public System.Boolean StoreCStoreReqOnly
        {
            set
            {
                this.m_adaptee.StoreCStoreReqOnly = value;
            }
        }

        /// <summary>
        ///  Send commit report for all images or per image
        /// </summary>
        public System.Boolean IsAssociated
        {
            get
            {
                return this.m_adaptee.IsAssociated;
            }
        }

        #region IEmulatorCommands

        private Mutex emulateVerificationScuMutex = new Mutex();

        /// <summary>
        /// <see cref="IEmulatorCommands.EmulateVerificationSCU"/>
        /// </summary>
        public bool EmulateVerificationSCU()
        {
            bool succes = false;
            try
            {
                // Wait until it is safe to enter.
                emulateVerificationScuMutex.WaitOne();
                succes = ((Wrappers.MEmulatorSession)this.m_adaptee).EmulateVerificationSCU();
            }
            finally
            {
                // Release the Mutex.
                emulateVerificationScuMutex.ReleaseMutex();
            }
            return succes;
        }

        /// <summary>
        /// Asynchronous EmulateVerificationSCU delegate.
        /// </summary>
        /// <remarks>
        /// The delegate must have the same signature as the method you want to call asynchronously.
        /// </remarks>
        private delegate bool AsyncEmulateVerificationSCUDelegate();

        /// <summary>
        /// <see cref="IEmulatorCommands.BeginEmulateVerificationSCU"/>
        /// </summary>
        /// <returns></returns>
        public System.IAsyncResult BeginEmulateVerificationSCU(
            AsyncCallback cb)
        {
            // Create the delegate.
            AsyncEmulateVerificationSCUDelegate dlgt = new AsyncEmulateVerificationSCUDelegate(this.EmulateVerificationSCU);
            // Initiate the asychronous call.
            object asyncState = dlgt;
            IAsyncResult ar = dlgt.BeginInvoke(
                cb,
                asyncState);
            return ar;
        }

        /// <summary>
        /// <see cref="IEmulatorCommands.EndEmulateVerificationSCU"/>
        /// </summary>
        /// <returns></returns>
        public bool EndEmulateVerificationSCU(
            IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncEmulateVerificationSCUDelegate dlgt = (AsyncEmulateVerificationSCUDelegate)ar.AsyncState;
            // Call EndInvoke to retrieve the results.
            bool retValue = dlgt.EndInvoke(ar);
            return retValue;
        }

        /// <summary>
        /// <see cref="IEmulatorCommands.TerminateConnection"/>
        /// </summary>
        public System.Boolean TerminateConnection()
        {
            return this.m_adaptee.TerminateConnection();
        }

        /// <summary>
        /// <see cref="IEmulatorCommands.AbortEmulation"/>
        /// </summary>
        public System.Boolean AbortEmulation()
        {
            return this.m_adaptee.AbortEmulation();
        }

        /// <summary>
        /// <see cref="IEmulatorCommands.AbortEmulationFromSCU"/>
        /// </summary>
        public System.Boolean AbortEmulationFromSCU()
        {
            return this.m_adaptee.AbortEmulationFromSCU();
        }

        private Mutex emulateEmulateStorageScuMutex = new Mutex();

        /// <summary>
        /// <see cref="IEmulatorCommands.EmulateStorageSCU"/>
        /// </summary>
        public bool EmulateStorageSCU(
            MediaFileName[] mediaFileNames,
            System.Boolean multipleAssociations,
            System.Boolean validateOnImport,
            System.Boolean dataUnderNewStudy,
            System.UInt16 nrOfRepetitions)
        {
            bool success = false;
            try
            {
                // Wait until it is safe to enter.
                emulateEmulateStorageScuMutex.WaitOne();
                if (mediaFileNames == null) throw new System.ArgumentNullException("mediaFileNames");
                if (nrOfRepetitions == 0) throw new System.ArgumentException();
                StorageEmulatorFlags storageEmulatorFlags = 0;
                if (multipleAssociations)
                {
                    storageEmulatorFlags |= StorageEmulatorFlags.MultipleAssociations;
                }
                else
                {
                    storageEmulatorFlags |= StorageEmulatorFlags.SingleAssociation;
                }
                if (validateOnImport) storageEmulatorFlags |= StorageEmulatorFlags.ValidateOnImport;
                if (dataUnderNewStudy) storageEmulatorFlags |= StorageEmulatorFlags.DataUnderNewStudy;
                if (nrOfRepetitions > 1) storageEmulatorFlags |= StorageEmulatorFlags.Repeat;
                success =
                    ((Wrappers.MEmulatorSession)this.m_adaptee).EmulateStorageSCU(
                    mediaFileNames,
                    (ushort)storageEmulatorFlags,
                    nrOfRepetitions);
            }
            finally
            {
                // Release the Mutex.
                emulateEmulateStorageScuMutex.ReleaseMutex();
            }
            return success;
        }

        /// <summary>
        /// Asynchronous EmulateStorageSCU delegate.
        /// </summary>
        /// <remarks>
        /// The delegate must have the same signature as the method you want to call asynchronously.
        /// </remarks>
        private delegate bool AsyncEmulateStorageSCUDelegate(
            MediaFileName[] mediaFileNames,
            System.Boolean multipleAssociations,
            System.Boolean validateOnImport,
            System.Boolean dataUnderNewStudy,
            System.UInt16 nrOfRepetitions);

        /// <summary>
        /// <see cref="IEmulatorCommands.BeginEmulateStorageSCU"/>
        /// </summary>
        /// <returns></returns>
        public System.IAsyncResult BeginEmulateStorageSCU(
            MediaFileName[] mediaFileNames,
            System.Boolean multipleAssociations,
            System.Boolean validateOnImport,
            System.Boolean dataUnderNewStudy,
            System.UInt16 nrOfRepetitions,
            AsyncCallback cb)
        {
            // Create the delegate.
            AsyncEmulateStorageSCUDelegate dlgt = new AsyncEmulateStorageSCUDelegate(this.EmulateStorageSCU);
            // Initiate the asychronous call.
            object asyncState = dlgt;
            IAsyncResult ar = dlgt.BeginInvoke(
                mediaFileNames,
                multipleAssociations,
                validateOnImport,
                dataUnderNewStudy,
                nrOfRepetitions,
                cb,
                asyncState);
            return ar;
        }

        /// <summary>
        /// <see cref="IEmulatorCommands.EndEmulateStorageSCU"/>
        /// </summary>
        /// <returns></returns>
        public bool EndEmulateStorageSCU(
            IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncEmulateStorageSCUDelegate dlgt = (AsyncEmulateStorageSCUDelegate)ar.AsyncState;
            // Call EndInvoke to retrieve the results.
            bool retValue = dlgt.EndInvoke(ar);
            return retValue;
        }

        private Mutex emulateStorageCommitScuMutex = new Mutex();

        /// <summary>
        /// <see cref="IEmulatorCommands.EmulateStorageCommitSCU"/>
        /// </summary>
        public bool EmulateStorageCommitSCU(System.Int16 delay)
        {
            bool success = false;
            try
            {
                // Wait until it is safe to enter.
                emulateStorageCommitScuMutex.WaitOne();
                success = ((Wrappers.MEmulatorSession)this.m_adaptee).EmulateStorageCommitSCU(delay);
            }
            finally
            {
                // Release the Mutex.
                emulateStorageCommitScuMutex.ReleaseMutex();
            }
            return success;
        }

        /// <summary>
        /// Asynchronous EmulateStorageCommitSCU delegate.
        /// </summary>
        /// <remarks>
        /// The delegate must have the same signature as the method you want to call asynchronously.
        /// </remarks>
        private delegate bool AsyncEmulateStorageCommitSCUDelegate(System.Int16 delay);

        /// <summary>
        /// <see cref="IEmulatorCommands.BeginEmulateStorageCommitSCU"/>
        /// </summary>
        /// <returns></returns>
        public System.IAsyncResult BeginEmulateStorageCommitSCU(
            System.Int16 delay,
            AsyncCallback cb)
        {
            // Create the delegate.
            AsyncEmulateStorageCommitSCUDelegate dlgt = new AsyncEmulateStorageCommitSCUDelegate(this.EmulateStorageCommitSCU);
            // Initiate the asychronous call.
            object asyncState = dlgt;
            IAsyncResult ar = dlgt.BeginInvoke(
                delay,
                cb,
                asyncState);
            return ar;
        }

        /// <summary>
        /// <see cref="IEmulatorCommands.EndEmulateStorageCommitSCU"/>
        /// </summary>
        /// <returns></returns>
        public bool EndEmulateStorageCommitSCU(
            IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncEmulateStorageCommitSCUDelegate dlgt = (AsyncEmulateStorageCommitSCUDelegate)ar.AsyncState;
            // Call EndInvoke to retrieve the results.
            bool retValue = dlgt.EndInvoke(ar);
            return retValue;
        }

        private Mutex emulateEmulateScpMutex = new Mutex();

        /// <summary>
        /// <see cref="IEmulatorCommands.EmulateSCP"/>
        /// </summary>
        /// <returns></returns>
        public bool EmulateSCP()
        {
            bool success = false;
            try
            {
                emulateEmulateScpMutex.WaitOne();
                success = this.m_adaptee.EmulateSCP();
            }
            finally
            {
                emulateEmulateScpMutex.ReleaseMutex();
            }
            return success;
        }

        /// <summary>
        /// Asynchronous ExecuteScript delegate.
        /// </summary>
        /// <remarks>
        /// The delegate must have the same signature as the method you want to call asynchronously.
        /// </remarks>
        private delegate bool AsyncEmulateSCPDelegate();

        /// <summary>
        /// <see cref="IEmulatorCommands.BeginEmulateSCP"/>
        /// </summary>
        /// <returns></returns>
        public System.IAsyncResult BeginEmulateSCP(
            AsyncCallback cb)
        {
            // Create the delegate.
            AsyncEmulateSCPDelegate dlgt = new AsyncEmulateSCPDelegate(this.EmulateSCP);
            // Initiate the asychronous call.
            object asyncState = dlgt;
            IAsyncResult ar = dlgt.BeginInvoke(
                cb,
                asyncState);
            return ar;
        }

        /// <summary>
        /// <see cref="IEmulatorCommands.EndEmulateSCP"/>
        /// </summary>
        /// <returns></returns>
        public bool EndEmulateSCP(
            IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncEmulateSCPDelegate dlgt = (AsyncEmulateSCPDelegate)ar.AsyncState;
            // Call EndInvoke to retrieve the results.
            bool retValue = dlgt.EndInvoke(ar);
            return retValue;
        }

        /// <summary>
        /// The emulator type that runs in a emulator session.
        /// </summary>
        public DvtkData.Results.ScpEmulatorType ScpEmulatorType
        {
            get
            {
                switch (this.m_adaptee.ScpEmulatorType)
                {
                    case Wrappers.ScpEmulatorType.ScpEmulatorTypePrint:
                        return DvtkData.Results.ScpEmulatorType.Printing;
                    case Wrappers.ScpEmulatorType.ScpEmulatorTypeStorage:
                        return DvtkData.Results.ScpEmulatorType.Storage;
                    case Wrappers.ScpEmulatorType.ScpEmulatorTypeStorageCommit:
                        return DvtkData.Results.ScpEmulatorType.StorageCommit;
                    case Wrappers.ScpEmulatorType.ScpEmulatorTypeMpps:
                        return DvtkData.Results.ScpEmulatorType.Mpps;
                    case Wrappers.ScpEmulatorType.ScpEmulatorTypeWorklist:
                        return DvtkData.Results.ScpEmulatorType.Worklist;
                    case Wrappers.ScpEmulatorType.ScpEmulatorTypeQueryRetrieve:
                        return DvtkData.Results.ScpEmulatorType.QueryRetrieve;
                    default:
                    case Wrappers.ScpEmulatorType.ScpEmulatorTypeUnknown:
                        return DvtkData.Results.ScpEmulatorType.Unknown;
                }
            }
            set
            {
                switch (value)
                {
                    case DvtkData.Results.ScpEmulatorType.Printing:
                        this.m_adaptee.ScpEmulatorType = Wrappers.ScpEmulatorType.ScpEmulatorTypePrint;
                        break;
                    case DvtkData.Results.ScpEmulatorType.Storage:
                        this.m_adaptee.ScpEmulatorType = Wrappers.ScpEmulatorType.ScpEmulatorTypeStorage;
                        break;
                    case DvtkData.Results.ScpEmulatorType.StorageCommit:
                        this.m_adaptee.ScpEmulatorType = Wrappers.ScpEmulatorType.ScpEmulatorTypeStorageCommit;
                        break;
                    case DvtkData.Results.ScpEmulatorType.Mpps:
                        this.m_adaptee.ScpEmulatorType = Wrappers.ScpEmulatorType.ScpEmulatorTypeMpps;
                        break;
                    case DvtkData.Results.ScpEmulatorType.Worklist:
                        this.m_adaptee.ScpEmulatorType = Wrappers.ScpEmulatorType.ScpEmulatorTypeWorklist;
                        break;
                    case DvtkData.Results.ScpEmulatorType.QueryRetrieve:
                        this.m_adaptee.ScpEmulatorType = Wrappers.ScpEmulatorType.ScpEmulatorTypeQueryRetrieve;
                        break;
                    default:
                    case DvtkData.Results.ScpEmulatorType.Unknown:
                        this.m_adaptee.ScpEmulatorType = Wrappers.ScpEmulatorType.ScpEmulatorTypeUnknown;
                        break;
                }
            }
        }

        /// <summary>
        /// The emulator type that runs in a emulator session.
        /// </summary>
        public DvtkData.Results.ScuEmulatorType ScuEmulatorType
        {
            get
            {
                switch (this.m_adaptee.ScuEmulatorType)
                {
                    case Wrappers.ScuEmulatorType.ScuEmulatorTypeStorage:
                        return DvtkData.Results.ScuEmulatorType.Storage;
                    default:
                    case Wrappers.ScuEmulatorType.ScuEmulatorTypeUnknown:
                        return DvtkData.Results.ScuEmulatorType.Unknown;
                }
            }
            set
            {
                switch (value)
                {
                    case DvtkData.Results.ScuEmulatorType.Storage:
                        this.m_adaptee.ScuEmulatorType = Wrappers.ScuEmulatorType.ScuEmulatorTypeStorage;
                        break;
                    default:
                    case DvtkData.Results.ScuEmulatorType.Unknown:
                        this.m_adaptee.ScuEmulatorType = Wrappers.ScuEmulatorType.ScuEmulatorTypeUnknown;
                        break;
                }
            }
        }

        #endregion IEmulatorCommands

        #region IPrinterSettings
        /// <summary>
        /// <see cref="IPrinterSettings.Printer"/>
        /// </summary>
        public IPrinter Printer
        {
            get
            {
                if (m_printer == null) m_printer = new Printer((Wrappers.MEmulatorSession)m_adaptee);
                return m_printer;
            }
        }
        #endregion

        /// <summary>
        /// Expand Results File Name : Helper
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override string GetExpandedResultsFileNameHelper(string fileName)
        {
            System.UInt16 sessionId = this.SessionId;
            string dir;
            string tempFileName;
            if (fileName != string.Empty)
            {
                dir = System.IO.Path.GetDirectoryName(fileName);
                tempFileName = System.IO.Path.GetFileName(fileName);
            }
            else
            {
                dir = string.Empty;
                tempFileName = string.Empty;
            }
            string sessionIdString = sessionId.ToString("000");
            tempFileName = tempFileName.Replace('.', '_');
            const string postfix = "res";
            const string extension = ".xml";
            string emulatorMiddlefix = null;
            //
            // Precondition only one type of emulator may be set.
            //
            if (
                this.ScpEmulatorType != DvtkData.Results.ScpEmulatorType.Unknown &&
                this.ScuEmulatorType != DvtkData.Results.ScuEmulatorType.Unknown)
            {
                throw new System.ApplicationException(
                    "Only one type of emulator may be set using ScpEmulatorType and ScuEmulatorType.");
            }
            else if (this.ScpEmulatorType != DvtkData.Results.ScpEmulatorType.Unknown)
            {
                switch (this.ScpEmulatorType)
                {
                    case DvtkData.Results.ScpEmulatorType.Printing:
                        emulatorMiddlefix = "pr_scp_em";
                        break;
                    case DvtkData.Results.ScpEmulatorType.Storage:
                        emulatorMiddlefix = "st_scp_em";
                        break;
                    case DvtkData.Results.ScpEmulatorType.StorageCommit:
                        emulatorMiddlefix = "cm_scp_em";
                        break;
                    case DvtkData.Results.ScpEmulatorType.Mpps:
                        emulatorMiddlefix = "mp_scp_em";
                        break;
                    case DvtkData.Results.ScpEmulatorType.Worklist:
                        emulatorMiddlefix = "wl_scp_em";
                        break;
                    case DvtkData.Results.ScpEmulatorType.QueryRetrieve:
                        emulatorMiddlefix = "qr_scp_em";
                        break;
                    case DvtkData.Results.ScpEmulatorType.Unknown:
                        throw new System.NotSupportedException();
                }
            }
            else if (this.ScuEmulatorType != DvtkData.Results.ScuEmulatorType.Unknown)
            {
                switch (this.ScuEmulatorType)
                {
                    case DvtkData.Results.ScuEmulatorType.Storage:
                        emulatorMiddlefix = "st_scu_em";
                        break;
                    case DvtkData.Results.ScuEmulatorType.Unknown:
                        throw new System.NotSupportedException();
                }
            }
            else
            {
                throw new System.ApplicationException(
                    "No type of emulator was set using ScpEmulatorType and ScuEmulatorType.");
            }
            System.Collections.ArrayList fileNameElementsArray =
                new System.Collections.ArrayList();
            if (sessionIdString != string.Empty) fileNameElementsArray.Add(sessionIdString);
            if (tempFileName != string.Empty) fileNameElementsArray.Add(tempFileName);
            if (emulatorMiddlefix != string.Empty) fileNameElementsArray.Add(emulatorMiddlefix);
            if (postfix != string.Empty) fileNameElementsArray.Add(postfix);
            string[] fileNameElements =
                (string[])fileNameElementsArray.ToArray(typeof(string));
            tempFileName = string.Join("_", fileNameElements);
            tempFileName = System.IO.Path.ChangeExtension(tempFileName, extension);
            tempFileName = System.IO.Path.Combine(dir, tempFileName);
            return tempFileName;
        }
    }
}