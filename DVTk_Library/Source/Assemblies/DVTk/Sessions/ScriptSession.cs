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
    using Dvtk;
    using DvtkData;
    using SessionFileName = System.String;

    using DefinitionFileRootDirectory = System.String;
    using DefinitionFileName = System.String;

    using System.Collections;
    using DvtkData.Dimse;
    using DvtkData.Dul;

    using System;
    using System.Threading;

    /// <summary>
    /// Return codes for a DIMSE message send action.
    /// </summary>
    public enum SendReturnCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 0,
        /// <summary>
        /// Failure without further specification.
        /// </summary>
        Failure,
        /// <summary>
        /// Association rejected.
        /// </summary>
        AssociationRejected,
        /// <summary>
        /// Association released.
        /// </summary>
        AssociationReleased,
        /// <summary>
        /// Association aborted.
        /// </summary>
        AssociationAborted,
        /// <summary>
        /// TCP/IP socket connection closed.
        /// </summary>
        SocketClosed,
        /// <summary>
        /// TCP/IP socket connection not established.
        /// </summary>
        NoSocketConnection,
    }

    /// <summary>
    /// Return codes for a DIMSE message receive action.
    /// </summary>
    public enum ReceiveReturnCode
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 0,
        /// <summary>
        /// Failure without further specification.
        /// </summary>
        Failure,
        /// <summary>
        /// Association rejected.
        /// </summary>
        AssociationRejected,
        /// <summary>
        /// Association released.
        /// </summary>
        AssociationReleased,
        /// <summary>
        /// Association aborted.
        /// </summary>
        AssociationAborted,
        /// <summary>
        /// TCP/IP socket connection closed.
        /// </summary>
        SocketClosed,
        /// <summary>
        /// TCP/IP socket connection not established.
        /// </summary>
        NoSocketConnection,
    };
    /// <summary>
    /// Send and receive DICOM DIMSE level messages.
    /// </summary>
    /// <remarks>
    /// <p>
    /// The Dicom Validation Tool (DVT) supports send and receive of all DIcoM Service Element (DIMSE) messages.
    /// These messages are either send or received as request or response.
    /// </p>
    /// <p>
    /// <list type="bullet">
    ///     <item>
    ///         <term>Request Primitive</term>
    ///         <description>Send DICOM request message</description>
    ///     </item>
    ///     <item>
    ///         <term>Indication Primitive</term>
    ///         <description>Received DICOM request message</description>
    ///     </item>
    ///     <item>
    ///         <term>Response Primitive</term>
    ///         <description>Send DICOM response message</description>
    ///     </item>
    ///     <item>
    ///         <term>Confirmation Primitive</term>
    ///         <description>Received DICOM response message</description>
    ///     </item>
    /// </list>
    /// </p>
    /// <p>
    /// Information Object Definition (IOD) Support is provided by loading the appropriate definition files. 
    /// The DIMSE Command / IOD combination must match one of the DEFINEs in the definition files.
    /// </p>
    /// <p>
    /// For supported Dimse commands <see cref="DvtkData.Dimse.DimseCommand"/>.
    /// </p>
    /// </remarks>
    public interface IDimseMessaging
    {
        /// <summary>
        /// Send a dicom message from the Dicom Validation Tool (DVT) towards the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// This corresponds with a DIMSE Request Primitive.
        /// </remarks>
        /// <param name="dicomMessage">
        /// Message of type <see cref="DvtkData.Dimse.DicomMessage"/>
        /// </param>
        /// <returns>
        /// The return code of the action.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>dicomMessage</c> is a <see langword="null"/> reference.</exception>
        SendReturnCode Send(DicomMessage dicomMessage);
        /// <summary>
        /// Receive a DIMSE message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <remarks>
        /// <p>
        /// This corresponds with a DIMSE Confirmation Primitive.
        /// </p>
        /// </remarks>
        /// <param name="dicomMessage">
        /// Message of type <see cref="DvtkData.Dimse.DicomMessage"/>
        /// </param>
        /// <returns>
        /// The return code of the action.
        /// </returns>
        ReceiveReturnCode Receive(out DicomMessage dicomMessage);
        /// <summary>
        /// Receive a DIMSE or ACSE message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <p>
        /// This corresponds with a DIMSE Confirmation Primitive or ACSE response.
        /// </p>
        /// <p>
        /// This call allows handling of A_ABORT and A_RELEASE_RQ messages without 
        /// causing an application exception.
        /// </p>
        /// <param name="message">
        /// Abstract message of type <see cref="DvtkData.Message"/> the user can 
        /// use reflection <see cref="System.Object.GetType()"/> to determine whether the message
        /// is of type <see cref="DvtkData.Dimse.DicomMessage"/> or <see cref="DvtkData.Dul.DulMessage"/>.
        /// </param>
        /// <returns>
        /// The return code of the action.
        /// </returns>
        ReceiveReturnCode Receive(out Message message);

        /// <summary>
        /// The boolean indicates for any incoming data on the network while reading from TCP/IP socket.
        /// </summary>
        bool HasPendingDataInNetworkInputBuffer
        {
            get;
        }
    }
    /// <summary>
    /// Flags to steer the validation process.
    /// </summary>
    [System.Flags]
    public enum ValidationControlFlags
    {
        /// <summary>
        /// No validation is done.
        /// </summary>
        None                    = 0,
        /// <summary>
        /// Validation of attribute value representation. Syntax and format checking.
        /// </summary>
        UseValueRepresentations = 1<<0,
        /// <summary>
        /// Validation of dicom message objects (Information Object IO) 
        /// against corresponding definitions (Information Object Definition IOD).
        /// </summary>
        UseDefinitions          = 1<<1,
        /// <summary>
        /// Validation of dicom message objects against reference objects.
        /// These references are supplied by the user.
        /// </summary>
        UseReferences           = 1<<2,
        /// <summary>
        /// Combination of UseValueRepresentations|UseDefinitions|UseReferences
        /// </summary>
        All                     = (UseValueRepresentations|UseDefinitions|UseReferences),
    };

    /// <summary>
    /// Access to commands that validate DIMSE messages.
    /// </summary>
    /// <remarks>
    /// DIMSE stands for DICOM Message Service Element.
    /// The DICOM application layer communication protocol.
    /// </remarks>
    public interface IDimseValidation
    {
        /// <summary>
        /// Validate a network related DIMSE message.
        /// </summary>
        /// <param name="message">Message to be validated</param>
        /// <param name="referenceMessage">Message used as reference to compare with</param>
        /// <param name="validationControlFlags">Flags to steer the validation process</param>
        /// <returns>
        /// <see langword="true"/> if the validation process succeeded, 
        /// <see langword="false"/> if the validation process did not succeed.
        /// </returns>
        /// <remarks>
        /// The return value does not indicate whether any validation reports are found.
        /// Only indicates whether the process succeeded.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument <c>message</c> is a <see langword="null"/> reference.</exception>
        bool Validate(
            DicomMessage message, 
            DicomMessage referenceMessage,
            ValidationControlFlags validationControlFlags);

        /// <summary>
        /// Validate a network related DIMSE message.
        /// </summary>
        /// <param name="message">Message to be validated</param>
        /// <param name="referenceMessage">Message used as reference to compare with</param>
        /// <param name="lastMessage">The last Message sent</param>
        ///  It may be needed when a Message is received for validation purposes
        ///  example C-FIND-RSP validation is better if we know what the C-FIND-RQ identifier contained
        /// <param name="validationControlFlags">Flags to steer the validation process</param>
        /// <returns>
        /// <see langword="true"/> if the validation process succeeded, 
        /// <see langword="false"/> if the validation process did not succeed.
        /// </returns>
        /// <remarks>
        /// The return value does not indicate whether any validation reports are found.
        /// Only indicates whether the process succeeded.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument <c>message</c> is a <see langword="null"/> reference.</exception>
        bool Validate(
            DicomMessage message,
            DicomMessage referenceMessage,
            DicomMessage lastMessage,
            ValidationControlFlags validationControlFlags);
    }
    /// <summary>
    /// Dicom Validation Tool (DVT) supports both sending and receiving of all 6 
    /// Association Control Service Element (ACSE) requests / responses:
    /// </summary>
    /// <remarks>
    /// SEND / RECEIVE ACSE Request and Responses:
    /// <list type="bullet">
    /// <item>Associate Request</item>
    /// <item>Associate Accept</item>
    /// <item>Associate Reject</item>
    /// <item>Release Request</item>
    /// <item>Release Response</item>
    /// <item>Abort Request</item>
    /// </list>
    /// Data Transfer is handled at the 
    /// DIcom Message Service Element (DIMSE) / Information Object Definition (IOD) level - See DIMSE Messages.
    /// </remarks>
    public interface IDulMessaging
    {
        /// <summary>
        /// Send a Dicom Upper Layer (DUL) message from the Dicom Validation Tool (DVT) towards the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// This corresponds with a DUL Request Primitive.
        /// </remarks>
        /// <param name="dulMessage">
        /// Message of type <see cref="DvtkData.Dul.DulMessage"/>
        /// </param>
        /// <returns>
        /// The return code of the action.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Argument <c>dulMessage</c> is a <see langword="null"/> reference.</exception>
        SendReturnCode Send(DulMessage dulMessage);
        /// <summary>
        /// Receive a Dicom Upper Layer (DUL) message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <remarks>
        /// <p>
        /// This corresponds with a DUL Confirmation Primitive.
        /// </p>
        /// </remarks>
        /// <param name="dulMessage">
        /// Message of type <see cref="DvtkData.Dul.DulMessage"/>
        /// </param>
        /// <returns>
        /// The return code of the action.
        /// </returns>
        ReceiveReturnCode Receive(out DulMessage dulMessage);
        /// <summary>
        /// Receive a A_ABORT Dicom Upper Layer (DUL) message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <param name="A_ABORT_Message">
        /// Message of type <see cref="DvtkData.Dul.A_ABORT"/>
        /// <see langword="null"/> if a different message was received.</param>
        /// <returns>
        /// </returns>
        ReceiveReturnCode Receive(out A_ABORT A_ABORT_Message);
        /// <summary>
        /// Receive a A_ASSOCIATE_AC Dicom Upper Layer (DUL) message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <param name="A_ASSOCIATE_AC_Message">
        /// Message of type <see cref="DvtkData.Dul.A_ASSOCIATE_AC"/>
        /// <see langword="null"/> if a different message was received.</param>
        /// <returns>
        /// </returns>
        ReceiveReturnCode Receive(out A_ASSOCIATE_AC A_ASSOCIATE_AC_Message);
        /// <summary>
        /// Receive a A_ASSOCIATE_RJ Dicom Upper Layer (DUL) message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <param name="A_ASSOCIATE_RJ_Message">
        /// Message of type <see cref="DvtkData.Dul.A_ASSOCIATE_RJ"/>
        /// <see langword="null"/> if a different message was received.</param>
        /// <returns>
        /// </returns>
        ReceiveReturnCode Receive(out A_ASSOCIATE_RJ A_ASSOCIATE_RJ_Message);
        /// <summary>
        /// Receive a A_ASSOCIATE_RQ Dicom Upper Layer (DUL) message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <param name="A_ASSOCIATE_RQ_Message">
        /// Message of type <see cref="DvtkData.Dul.A_ASSOCIATE_RQ"/>
        /// <see langword="null"/> if a different message was received.</param>
        /// <returns>
        /// </returns>
        ReceiveReturnCode Receive(out A_ASSOCIATE_RQ A_ASSOCIATE_RQ_Message);
        /// <summary>
        /// Receive a A_RELEASE_RP Dicom Upper Layer (DUL) message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <param name="A_RELEASE_RP_Message">
        /// Message of type <see cref="DvtkData.Dul.A_RELEASE_RP"/>
        /// <see langword="null"/> if a different message was received.</param>
        /// <returns>
        /// </returns>
        ReceiveReturnCode Receive(out A_RELEASE_RP A_RELEASE_RP_Message);
        /// <summary>
        /// Receive a A_RELEASE_RQ Dicom Upper Layer (DUL) message from the System Under Test (SUT) to the Dicom Validation Tool (DVT).
        /// </summary>
        /// <param name="A_RELEASE_RQ_Message">
        /// Message of type <see cref="DvtkData.Dul.A_RELEASE_RQ"/>
        /// <see langword="null"/> if a different message was received.</param>
        /// <returns>
        /// </returns>
        ReceiveReturnCode Receive(out A_RELEASE_RQ A_RELEASE_RQ_Message);
    }
    /// <summary>
    /// Access to commands that validate DULP messages.
    /// </summary>
    /// <remarks>
    /// DULP stands for DICOM Upper Layer Protocl.
    /// The DICOM sub-application layer communication protocol.
    /// </remarks>
    public interface IDulValidation
    {
        /// <summary>
        /// Validate a network related DULP message.
        /// </summary>
        /// <param name="message">Message to be validated</param>
        /// <param name="referenceMessage">Message used as reference to compare with</param>
        /// <param name="validationControlFlags">Flags to steer the validation process</param>
        /// <returns>
        /// <see langword="true"/> if the validation process succeeded, 
        /// <see langword="false"/> if the validation process did not succeed.
        /// </returns>
        /// <remarks>
        /// <p>
        /// The return value does not indicate whether any validation reports are found.
        /// Only indicates whether the process succeeded.
        /// </p>
        /// <p>
        /// Option; ValidationControlFlags.UseValueRepresentations is ignored.
        /// </p>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument <c>message</c> is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">Runtime instance type of <c>message</c> and <c>referenceMessage</c> differ.</exception>
        bool Validate(
            DulMessage message, 
            DulMessage referenceMessage,
            ValidationControlFlags validationControlFlags);
    }
    /// <summary>
    /// Access to settings and commands execution of DICOM scripts 
    /// with extensions <c>.DS</c> and <c>.DSS</c>.
    /// </summary>
    public interface IScriptExecution
    {
        /// <summary>
        /// The directory serving as root directory from the DICOM Scripts.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        /// <exception cref="System.ArgumentException">Directory may not be an empty string. Use ".\" for current directory."</exception>
        string DicomScriptRootDirectory { get; set; }
        /// <summary>
        /// Execute a Script File of type DICOMScript (.DS) / DICOMSuperScript (.DSS). 
        /// </summary>
        /// <remarks>
        /// <p>
        /// The continue on error allows control over the DICOMSuperScript. The DICOMSuperScript will
        /// continue if one of the containing DICOMScripts fails on an error.
        /// </p>
        /// <p>
        /// A DICOMScript (.DS) describes a single Test Scenario. 
        /// ACSE Requests and Responses and DIMSE Command / IOD combinations are 
        /// used to perform a given Test Scenario.
        /// </p>
        /// <p>
        /// DVT interprets a DICOMScript in order to perform a Test Scenario.
        /// </p>
        /// <p>
        /// DICOMScripts should be written according to the role played by DVT:
        /// </p>
        /// <p>
        /// <list type="bullet">
        /// <item>
        /// <term>SCU</term>
        /// <description>write a DICOMScript that plays the SCU role in a given Test Scenario.</description>
        /// </item>
        /// <item>
        /// <term>SCP</term>
        /// <description>write a DICOMScript that plays the SCP role in a given Test Scenario.</description>
        /// </item>
        /// <item>
        /// <term>FSC</term>
        /// <description>write a DICOMScript that plays the FSC role in a given Test Scenario (creates Files-sets).</description>
        /// </item>
        /// </list>
        /// </p>
        /// <p>
        /// A DICOMSuperScript (.DSS) contains a list of DICOMScript (filenames) that together are used 
        /// to describe a Test Scenario. DVT executes the DICOMScripts in the order given in 
        /// the DICOMSuperScript.
        /// </p>
        /// <p>
        /// The DICOMSuperScript enables the reuse of certain DICOMScripts in various Test Scenarios 
        /// - e.g., DICOMScript that makes an Association for CT Image Storage.
        /// It is possible to repeat the DICOMScript execution a number of times using the<br></br>
        /// <c>DO n dicomscript</c><br></br>
        /// instruction where ‘n’ is the number of times that dicomscript is to be executed.
        /// </p>
        /// </remarks>
        /// <param name="dicomScriptFileName">Script File of type DICOMScript (.DS) / DICOMSuperScript (.DSS)</param>
        /// <param name="continueOnError">Continue a DICOMSuperScript when one of the conatining DICOMScripts fails on an error.</param>
        /// <returns></returns>
        System.Boolean ExecuteScript(string dicomScriptFileName, bool continueOnError);

        /// <summary>
        /// Asynchronously begin ExecuteScript.
        /// </summary>
        /// <param name="scriptFileName"></param>
        /// <param name="continueOnError"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        System.IAsyncResult BeginExecuteScript(string scriptFileName, bool continueOnError, AsyncCallback cb);

        /// <summary>
        /// End asynchronous ExecuteScript
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        bool EndExecuteScript(IAsyncResult ar) ;

        /// <summary>
        /// Parse a Script File of type DICOMScript (.DS) / DICOMSuperScript (.DSS). 
        /// </summary>
        /// <param name="dicomScriptFileName">Script File of type DICOMScript (.DS) / DICOMSuperScript (.DSS)</param>
        /// <returns><see langword="false"/> if parsing fails.</returns>
        System.Boolean ParseScript(System.String dicomScriptFileName);
        /// <summary>
        /// Can be used to stop a script execution.
        /// </summary>
        /// <returns></returns>
        System.Boolean TerminateConnection();
        /// <summary>
        /// Reset the state of the DULP Finite State Machine 
        /// (to allow new Associations – Presentation Context ID is reset to 1).
        /// </summary>
        void ResetAssociation();
        /// <summary>
        /// The directory containing instructive and explanatory description files for the scripts.
        /// </summary>
        System.String DescriptionDirectory
        {
            get;
            set;
        }
	}
    /// <summary>
    /// Summary description for Session.
    /// </summary>
    public class ScriptSession 
        : Session
        , IDataSetEncodingSettings
        , IDimseMessaging
        , IDimseValidation
        , IDulMessaging
        , IDulValidation
        , IScriptExecution
        , ISecure
        , IConfigurableDvt
        , IConfigurableSut
    {
		private Confirmer m_confirmer = null;

        #region IConfigurableDvt
        /// <summary>
        /// <see cref="IConfigurableDvt.DvtSystemSettings"/>
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
        /// <see cref="IConfigurableSut.SutSystemSettings"/>
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
		/// Copy all settings from the supplied ScriptSession to this ScriptSession.
		/// </summary>
		/// <param name="scriptSession">The ScriptSession to copy the settings from.</param>
		public virtual void CopySettingsFrom(ScriptSession scriptSession)
		{
			base.CopySettingsFrom(scriptSession);

			AddGroupLength = scriptSession.AddGroupLength;
			AutoType2Attributes = scriptSession.AutoType2Attributes;
			// Is read-only. ConfirmInteractionOption = scriptSession.ConfirmInteractionOption;
			DefineSqLength = scriptSession.DefineSqLength;
			DescriptionDirectory = scriptSession.DescriptionDirectory;
			DicomScriptRootDirectory = scriptSession.DicomScriptRootDirectory;

			// DvtSystemSettings.
			DvtSystemSettings.AeTitle = scriptSession.DvtSystemSettings.AeTitle;
			DvtSystemSettings.ImplementationClassUid = scriptSession.DvtSystemSettings.ImplementationClassUid;
			DvtSystemSettings.ImplementationVersionName = scriptSession.DvtSystemSettings.ImplementationVersionName;
			DvtSystemSettings.MaximumLengthReceived = scriptSession.DvtSystemSettings.MaximumLengthReceived;
			DvtSystemSettings.Port = scriptSession.DvtSystemSettings.Port;
			DvtSystemSettings.SocketTimeout = scriptSession.DvtSystemSettings.SocketTimeout;

            if (scriptSession.SecuritySettings.SecureSocketsEnabled)
            {
                // SecuritySettings.
                SecuritySettings.SecureSocketsEnabled = scriptSession.SecuritySettings.SecureSocketsEnabled;
                SecuritySettings.CacheTlsSessions = scriptSession.SecuritySettings.CacheTlsSessions;
                SecuritySettings.CertificateFileName = scriptSession.SecuritySettings.CertificateFileName;
                SecuritySettings.CheckRemoteCertificate = scriptSession.SecuritySettings.CheckRemoteCertificate;
                SecuritySettings.CipherFlags = scriptSession.SecuritySettings.CipherFlags;
                SecuritySettings.CredentialsFileName = scriptSession.SecuritySettings.CredentialsFileName;
                SecuritySettings.TlsCacheTimeout = scriptSession.SecuritySettings.TlsCacheTimeout;
                SecuritySettings.TlsPassword = scriptSession.SecuritySettings.TlsPassword;
                SecuritySettings.TlsVersionFlags = scriptSession.SecuritySettings.TlsVersionFlags;
            }

			// SutSystemSettings.
			SutSystemSettings.AeTitle = scriptSession.SutSystemSettings.AeTitle;
			SutSystemSettings.CommunicationRole = scriptSession.SutSystemSettings.CommunicationRole;
			SutSystemSettings.HostName = scriptSession.SutSystemSettings.HostName;
			SutSystemSettings.ImplementationClassUid = scriptSession.SutSystemSettings.ImplementationClassUid;
			SutSystemSettings.ImplementationVersionName = scriptSession.SutSystemSettings.ImplementationVersionName;
			SutSystemSettings.MaximumLengthReceived = scriptSession.SutSystemSettings.MaximumLengthReceived;
			SutSystemSettings.Port = scriptSession.SutSystemSettings.Port;
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

        #region IDimseMessaging

        /// <summary>
        /// <see cref="IDimseMessaging.Send(DicomMessage)"/>
        /// </summary>
        public SendReturnCode Send(DicomMessage dicomMessage)
        {
            if (dicomMessage == null) throw new System.ArgumentNullException("dicomMessage");

            Wrappers.SendReturnCode wrappersSendReturnCode =
                (this.m_adaptee as Wrappers.MScriptSession).Send(dicomMessage);
            return _Convert(wrappersSendReturnCode);
        }

        /// <summary>
        /// <see cref="IDimseMessaging.Send(DicomMessage)"/>
        /// With the new UPS SOP classes (Unified Procedure Step – Push SOP Class, Unified Procedure Step – 
        /// Watch SOP Class, Unified Procedure Step – Pull SOP Class, Unified Procedure Step – Event SOP Class), 
        /// fully specified in frozen supplement 96, it is now possible to send a DICOM message with an affected 
        /// or requested SOP Class UID attribute that is different from the abstract syntax UID in an accepted 
        /// presentation context.
        /// </summary>
        public SendReturnCode Send(DicomMessage dicomMessage, int presentationContextId)
        {
            if (dicomMessage == null) throw new System.ArgumentNullException("dicomMessage");

            Wrappers.SendReturnCode wrappersSendReturnCode =
                (this.m_adaptee as Wrappers.MScriptSession).Send(dicomMessage, presentationContextId);
            return _Convert(wrappersSendReturnCode);
        }

        /// <summary>
        /// Conversion Wrappers type => Dvtk type
        /// </summary>
        /// <param name="receiveReturnCode">in</param>
        /// <returns>out</returns>
        private static ReceiveReturnCode _Convert(Wrappers.ReceiveReturnCode receiveReturnCode)
        {
            switch (receiveReturnCode)
            {
                case Wrappers.ReceiveReturnCode.Success:                return ReceiveReturnCode.Success;
                case Wrappers.ReceiveReturnCode.Failure:                return ReceiveReturnCode.Failure;
                case Wrappers.ReceiveReturnCode.AssociationRejected:    return ReceiveReturnCode.AssociationRejected;
                case Wrappers.ReceiveReturnCode.AssociationReleased:    return ReceiveReturnCode.AssociationReleased;
                case Wrappers.ReceiveReturnCode.AssociationAborted:     return ReceiveReturnCode.AssociationAborted;
                case Wrappers.ReceiveReturnCode.SocketClosed:           return ReceiveReturnCode.SocketClosed;
                case Wrappers.ReceiveReturnCode.NoSocketConnection:     return ReceiveReturnCode.NoSocketConnection;
                default:
                    // Unknown Wrappers.ReceiveReturnCode
                    throw new System.NotImplementedException();
            }
        }
        /// <summary>
        /// Conversion Wrappers type => Dvtk type
        /// </summary>
        /// <param name="sendReturnCode">in</param>
        /// <returns>out</returns>
        private static SendReturnCode _Convert(Wrappers.SendReturnCode sendReturnCode)
        {
            switch (sendReturnCode)
            {
                case Wrappers.SendReturnCode.Success:                return SendReturnCode.Success;
                case Wrappers.SendReturnCode.Failure:                return SendReturnCode.Failure;
                case Wrappers.SendReturnCode.AssociationRejected:    return SendReturnCode.AssociationRejected;
                case Wrappers.SendReturnCode.AssociationReleased:    return SendReturnCode.AssociationReleased;
                case Wrappers.SendReturnCode.AssociationAborted:     return SendReturnCode.AssociationAborted;
                case Wrappers.SendReturnCode.SocketClosed:           return SendReturnCode.SocketClosed;
                case Wrappers.SendReturnCode.NoSocketConnection:     return SendReturnCode.NoSocketConnection;
                default:
                    // Unknown Wrappers.SendReturnCode
                    throw new System.NotImplementedException();
            }
        }
        /// <summary>
        /// <see cref="IDimseMessaging.Receive(out DicomMessage)"/>
        /// </summary>
        public ReceiveReturnCode Receive(out DicomMessage dicomMessage)
        {
            dicomMessage = null;
            Wrappers.ReceiveReturnCode wrappersReceiveReturnCode =
                (this.m_adaptee as Wrappers.MScriptSession).Receive(out dicomMessage);
            return _Convert(wrappersReceiveReturnCode);
        }
        /// <summary>
        /// <see cref="IDimseMessaging.Receive(out Message)"/>
        /// </summary>
        public ReceiveReturnCode Receive(out Message message)
        {
            message = null;
            Wrappers.ReceiveReturnCode wrappersReceiveReturnCode =
                (this.m_adaptee as Wrappers.MScriptSession).Receive(out message);
            return _Convert(wrappersReceiveReturnCode);
        }

        /// <summary>
        /// <see cref="IDimseMessaging.HasPendingDataInNetworkInputBuffer"/>
        /// </summary>
        public System.Boolean HasPendingDataInNetworkInputBuffer
        {
            get
            {
                System.Boolean isPending = (this.m_adaptee as Wrappers.MScriptSession).HasPendingDataInNetworkInputBuffer;
                return isPending;
            }
        }

        #endregion IDimseMessaging

        #region IDimseValidation

        /// <summary>
        /// <see cref="IDimseValidation.Validate(DicomMessage , 
        /// DicomMessage ,
        /// ValidationControlFlags )"/>
        /// </summary>
        public bool Validate(
            DicomMessage message, 
            DicomMessage referenceMessage,
            ValidationControlFlags validationControlFlags)
        {
            if (message == null) throw new System.ArgumentNullException();
            // referenceMessage may be null
            //
            // Convert flags
            //
            Wrappers.ValidationControlFlags
                wrappersValidationControlFlags = Wrappers.ValidationControlFlags.None;
            if ((validationControlFlags & ValidationControlFlags.UseDefinitions) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseDefinitions;
            if ((validationControlFlags & ValidationControlFlags.UseReferences) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseReferences;
            if ((validationControlFlags & ValidationControlFlags.UseValueRepresentations) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseValueRepresentations;
            
            return (this.m_adaptee as Wrappers.MScriptSession).Validate(
                message, 
                referenceMessage,
                wrappersValidationControlFlags);
        }

        /// <summary>
        /// <see cref="IDimseValidation.Validate( DicomMessage ,
        /// DicomMessage ,
        /// DicomMessage ,
        /// ValidationControlFlags )"/>
        /// </summary>
        public bool Validate(
            DicomMessage message,
            DicomMessage referenceMessage,
            DicomMessage lastMessage,
            ValidationControlFlags validationControlFlags)
        {
            if (message == null) throw new System.ArgumentNullException();
            // referenceMessage may be null
            //
            // Convert flags
            //
            Wrappers.ValidationControlFlags
                wrappersValidationControlFlags = Wrappers.ValidationControlFlags.None;
            if ((validationControlFlags & ValidationControlFlags.UseDefinitions) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseDefinitions;
            if ((validationControlFlags & ValidationControlFlags.UseReferences) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseReferences;
            if ((validationControlFlags & ValidationControlFlags.UseValueRepresentations) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseValueRepresentations;

            return (this.m_adaptee as Wrappers.MScriptSession).Validate(
                message,
                referenceMessage,
                lastMessage,
                wrappersValidationControlFlags);
        }

        #endregion IDimseValidation

        #region IDulValidation

        /// <summary>
        /// <see cref="IDulValidation.Validate"/>
        /// </summary>
        public bool Validate(
            DulMessage message, 
            DulMessage referenceMessage,
            ValidationControlFlags validationControlFlags)
        {
            if (message == null) throw new System.ArgumentNullException();
            //
            // referenceMessage may be null
            //
            if (referenceMessage != null)
            {
                //
                // Check runtime instance type equality of message and referenceMessage
                //
                if (referenceMessage.GetType() != message.GetType())
                    throw new System.ArgumentException(
                        string.Concat(
                        "message and referenceMessage should have the same type.\n",
                        string.Format(
                            "message type = {0}\n", 
                            message.GetType().ToString()
                        ),
                        string.Format(
                            "referenceMessage type = {0}\n", 
                            referenceMessage.GetType().ToString()
                        )
                        ),
                        "referenceMessage");
            }
            //
            // Remove obsolete flag ValidationControlFlags.UseValueRepresentations
            //
            Wrappers.ValidationControlFlags
                wrappersValidationControlFlags = Wrappers.ValidationControlFlags.None;
            if ((validationControlFlags & ValidationControlFlags.UseDefinitions) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseDefinitions;
            if ((validationControlFlags & ValidationControlFlags.UseReferences) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseReferences;
            if ((validationControlFlags & ValidationControlFlags.UseValueRepresentations) != 0)
                wrappersValidationControlFlags |= Wrappers.ValidationControlFlags.UseValueRepresentations;
            
            return (this.m_adaptee as Wrappers.MScriptSession).Validate(
                message, 
                referenceMessage,
                wrappersValidationControlFlags);
        }

        #endregion IDulValidation

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
        #endregion IDataSetEncodingSettings

        #region IDulMessaging
        /// <summary>
        /// <see cref="IDulMessaging.Send"/>
        /// </summary>
        public SendReturnCode Send(DulMessage message)
        {
            if (message == null) throw new System.ArgumentNullException();

            Wrappers.SendReturnCode wrappersSendReturnCode =
                (this.m_adaptee as Wrappers.MScriptSession).Send(message);
            return _Convert(wrappersSendReturnCode);
        }
        /// <summary>
        /// <see cref="IDulMessaging.Receive(out DulMessage )"/>
        /// </summary>
        public ReceiveReturnCode Receive(out DulMessage dulMessage)
        {
            dulMessage = null;
            Wrappers.ReceiveReturnCode wrappersReceiveReturnCode =
                (this.m_adaptee as Wrappers.MScriptSession).Receive(out dulMessage);
            return _Convert(wrappersReceiveReturnCode);
        }
        /// <summary>
        /// <see cref="IDulMessaging.Receive(out A_ABORT )"/>
        /// </summary>
        public ReceiveReturnCode Receive(out A_ABORT A_ABORT_Message)
        {
            ReceiveReturnCode receiveReturnCode;
            DulMessage dulMessage = null;
            receiveReturnCode = Receive(out dulMessage);
            A_ABORT_Message = dulMessage as A_ABORT;
            if (
                receiveReturnCode == ReceiveReturnCode.Success &&
                A_ABORT_Message == null
                ) 
                receiveReturnCode = ReceiveReturnCode.Failure;
            return receiveReturnCode;
        }
        /// <summary>
        /// <see cref="IDulMessaging.Receive(out A_ASSOCIATE_AC )"/>
        /// </summary>
        public ReceiveReturnCode Receive(out A_ASSOCIATE_AC A_ASSOCIATE_AC_Message)
        {
            ReceiveReturnCode receiveReturnCode;
            DulMessage dulMessage = null;
            receiveReturnCode = Receive(out dulMessage);
            A_ASSOCIATE_AC_Message = dulMessage as A_ASSOCIATE_AC;
            if (
                receiveReturnCode == ReceiveReturnCode.Success &&
                A_ASSOCIATE_AC_Message == null
                ) 
                receiveReturnCode = ReceiveReturnCode.Failure;
            return receiveReturnCode;
        }
        /// <summary>
        /// <see cref="IDulMessaging.Receive(out A_ASSOCIATE_RJ )"/>
        /// </summary>
        public ReceiveReturnCode Receive(out A_ASSOCIATE_RJ A_ASSOCIATE_RJ_Message)
        {
            ReceiveReturnCode receiveReturnCode;
            DulMessage dulMessage = null;
            receiveReturnCode = Receive(out dulMessage);
            A_ASSOCIATE_RJ_Message = dulMessage as A_ASSOCIATE_RJ;
            if (
                receiveReturnCode == ReceiveReturnCode.Success &&
                A_ASSOCIATE_RJ_Message == null
                ) 
                receiveReturnCode = ReceiveReturnCode.Failure;
            return receiveReturnCode;
        }
        /// <summary>
        /// <see cref="IDulMessaging.Receive(out A_ASSOCIATE_RQ )"/>
        /// </summary>
        public ReceiveReturnCode Receive(out A_ASSOCIATE_RQ A_ASSOCIATE_RQ_Message)
        {
            ReceiveReturnCode receiveReturnCode;
            DulMessage dulMessage = null;
            receiveReturnCode = Receive(out dulMessage);
            A_ASSOCIATE_RQ_Message = dulMessage as A_ASSOCIATE_RQ;
            if (
                receiveReturnCode == ReceiveReturnCode.Success &&
                A_ASSOCIATE_RQ_Message == null
                ) 
                receiveReturnCode = ReceiveReturnCode.Failure;
            return receiveReturnCode;
        }
        /// <summary>
        /// <see cref="IDulMessaging.Receive(out A_RELEASE_RP )"/>
        /// </summary>
        public ReceiveReturnCode Receive(out A_RELEASE_RP A_RELEASE_RP_Message)
        {
            ReceiveReturnCode receiveReturnCode;
            DulMessage dulMessage = null;
            receiveReturnCode = Receive(out dulMessage);
            A_RELEASE_RP_Message = dulMessage as A_RELEASE_RP;
            if (
                receiveReturnCode == ReceiveReturnCode.Success &&
                A_RELEASE_RP_Message == null
                ) 
                receiveReturnCode = ReceiveReturnCode.Failure;
            return receiveReturnCode;
        }
        /// <summary>
        /// <see cref="IDulMessaging.Receive(out A_RELEASE_RQ )"/>
        /// </summary>
        public ReceiveReturnCode Receive(out A_RELEASE_RQ A_RELEASE_RQ_Message)
        {
            ReceiveReturnCode receiveReturnCode;
            DulMessage dulMessage = null;
            receiveReturnCode = Receive(out dulMessage);
            A_RELEASE_RQ_Message = dulMessage as A_RELEASE_RQ;
            if (
                receiveReturnCode == ReceiveReturnCode.Success &&
                A_RELEASE_RQ_Message == null
                ) 
                receiveReturnCode = ReceiveReturnCode.Failure;
            return receiveReturnCode;
        }
        #endregion IDulMessaging

        #region IScriptExecution
        /// <summary>
        /// <see cref="IScriptExecution.DicomScriptRootDirectory"/>
        /// </summary>
        public string DicomScriptRootDirectory
        {
            get 
            { 
                return this.m_adaptee.DicomScriptRootDirectory; 
            }
            set 
            { 
                if (value == null) throw new System.ArgumentNullException();
                if (value == string.Empty) 
                    throw new System.ArgumentException(
                        "DicomScriptRootDirectory may not be an empty string.\n"+
                        "Use \".\" for current directory.");
                this.m_adaptee.DicomScriptRootDirectory = value; 
            }
        }

        private Mutex executeScriptMutex = new Mutex();

        /// <summary>
        /// <see cref="IScriptExecution.ExecuteScript"/>
        /// </summary>
        public bool ExecuteScript(string scriptFileName, bool continueOnError)
        {
            bool success = false;
			try
			{
				// Wait until it is safe to enter.
				executeScriptMutex.WaitOne();
				if (scriptFileName == null) throw new System.ArgumentNullException();
				this.m_adaptee.ContinueOnError = continueOnError;
				success = this.m_adaptee.ExecuteScript(scriptFileName);
			}
			catch(System.Exception e)
			{
				String msg = String.Format("System.Exception in ScriptSession.cs: {0}", e.Message);
				this.WriteError(msg);
				throw new System.Exception("Script execution failure:", e);
			}
			finally
			{
				// Release the Mutex.
				executeScriptMutex.ReleaseMutex();
			}
            return success;
        }

        /// <summary>
        /// Asynchronous ExecuteScript delegate.
        /// </summary>
        /// <remarks>
        /// The delegate must have the same signature as the method you want to call asynchronously.
        /// </remarks>
        private delegate bool AsyncExecuteScriptDelegate(string scriptFileName, bool continueOnError);

        /// <summary>
        /// <see cref="IScriptExecution.BeginExecuteScript"/>
        /// </summary>
        public System.IAsyncResult BeginExecuteScript(
            string scriptFileName, 
            bool continueOnError, 
            AsyncCallback cb) 
        {
            // Create the delegate.
            AsyncExecuteScriptDelegate dlgt = new AsyncExecuteScriptDelegate(this.ExecuteScript);
            // Initiate the asychronous call.
            object asyncState = dlgt;
            IAsyncResult ar = dlgt.BeginInvoke(
                scriptFileName, 
                continueOnError,
                cb, 
                asyncState);
            return ar;
        }
        
        /// <summary>
        /// <see cref="IScriptExecution.EndExecuteScript"/>
        /// </summary>
        public bool EndExecuteScript(
            IAsyncResult ar) 
        {
            // Retrieve the delegate.
            AsyncExecuteScriptDelegate dlgt = (AsyncExecuteScriptDelegate)ar.AsyncState;
            // Call EndInvoke to retrieve the results.
            bool retValue = dlgt.EndInvoke(ar);
            return retValue;
        }

        /// <summary>
        /// <see cref="IScriptExecution.ParseScript"/>
        /// </summary>
        public System.Boolean ParseScript(System.String scriptFileName)
        {
            if (scriptFileName == null) throw new System.ArgumentNullException();

            return this.m_adaptee.ParseScript(scriptFileName);
        }
        /// <summary>
        /// <see cref="IScriptExecution.TerminateConnection"/>
        /// </summary>
        public System.Boolean TerminateConnection()
        {
            return this.m_adaptee.TerminateConnection();
        }
        /// <summary>
        /// <see cref="IScriptExecution.ResetAssociation"/>
        /// </summary>
        public void ResetAssociation()
        {
            this.m_adaptee.ResetAssociation();
            return;
        }
        
        /// <summary>
        /// <see cref="IScriptExecution.DescriptionDirectory"/>
        /// </summary>
        public System.String DescriptionDirectory
        {
            get
            {
                return this.m_adaptee.DescriptionDirectory;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.DescriptionDirectory = value;
            }
        }
        #endregion IScriptExecution

        override internal void _Initialize()
        {
            base._Initialize();
            this.m_confirmer = new Confirmer(this);
            this.m_MScriptSession.ConfirmInteractionTarget = this.m_confirmer;
        }
        private Wrappers.MScriptSession m_adaptee = null;
        override internal Wrappers.MBaseSession m_MBaseSession
        {
            get { return m_adaptee; }
        }
        internal Wrappers.MScriptSession m_MScriptSession
        {
            get { return m_adaptee; }
        }
        //
        // Touch the AppUnloadListener abstract class to trigger its static-constructor.
        //
        static ScriptSession()
        {
            AppUnloadListener.Touch();
        }
        /// <summary>
        /// Finalizer
        /// </summary>
        public ScriptSession()
        {
            m_adaptee          = new Wrappers.MScriptSession();
            Wrappers.MDisposableResources.AddDisposable(m_adaptee);
            _Initialize();
        }
        internal ScriptSession(Wrappers.MBaseSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException();
            // Check type
            m_adaptee = (Wrappers.MScriptSession)adaptee;
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
        static public ScriptSession LoadFromFile(SessionFileName sessionFileName)
        {
            if (sessionFileName == null) throw new System.ArgumentNullException("sessionFileName");
            Session session = SessionFactory.TheInstance.Load(sessionFileName);
            System.Diagnostics.Trace.Assert(session is ScriptSession);
            return (ScriptSession)session;
        }
        /// <summary>
        /// Finalizer
        /// </summary>
        ~ScriptSession()
        {
            Wrappers.MDisposableResources.RemoveDisposable(m_adaptee);
            m_adaptee = null;
        }

		/// <summary>
		/// Method for comparing pixel data in two DICOM messages
		/// </summary>
		/// <param name="message"></param>
		/// <param name="referenceMessage"></param>
		/// <returns></returns>
		public bool ComparePixelData(DicomMessage message, DicomMessage referenceMessage)
		{
			if (message == null) throw new System.ArgumentNullException();
			if (referenceMessage == null) throw new System.ArgumentNullException();
			           
			return (this.m_adaptee as Wrappers.MScriptSession).ComparePixelData(
				message, 
				referenceMessage);
		}

        /// <summary>
        /// Used for display VR in result
        /// </summary>
        public System.Boolean IsDataTransferExplicit
        {
            get
            {
                return this.m_adaptee.IsDataTransferExplicit;
            }
            set
            {
                this.m_adaptee.IsDataTransferExplicit = value;
            }
        }

        /// <summary>
        /// Callback: Called when the application wants a confirm action by the user.
        /// </summary>
        /// <remarks>
        /// <p>
        /// This is the callback stub declaration.
        /// </p>
        /// <p>
        /// The callback can be used to override the confirmation interaction.
        /// <see cref="ConfirmInteraction"/>
        /// </p>
        /// <p>
        /// Currently predefined confirmation interactions:
        /// <list type="bullet">
        /// <item><see cref="ScriptSession.ConfirmInteractionForms"/> - Default</item>
        /// <item><see cref="ScriptSession.ConfirmInteractionAuto"/></item>
        /// <item><see cref="ScriptSession.ConfirmInteractionConsole"/></item>
        /// </list>
        /// These predefined interaction may be used as stubs (see <see cref="ConfirmInteraction"/>).
        /// </p>
        /// </remarks>
        public delegate void ConfirmInteractionCallBack();
        /// <summary>
        /// Callback: Called when the application wants a confirm action by the user.
        /// </summary>
        /// <remarks>
        /// <p>
        /// This is the callback proxy to which a new callback stub may be assigned.
        /// <see cref="ConfirmInteractionCallBack"/>
        /// </p>
        /// </remarks>
        /// <example>This sample shows how to assign a new callback implementation.
        /// <code>
        ///   // implementation confirm interaction
        ///   static public void MyConfirmInteractionAuto
        ///   {
        ///       // My action to confirm. For instance MessageBox interaction.
        ///       System.Windows.Forms.MessageBox.Show("Click to continue.");
        ///       return;
        ///   }
        ///   
        ///   // Code against session.
        ///   ScriptSession ses;
        ///   ...
        ///   // Assign your own confirm interaction callback.
        ///   ses.ConfirmInteraction = new ConfirmInteractionCallBack(MyConfirmInteractionAuto);
        /// </code>
        /// </example>
        public ConfirmInteractionCallBack ConfirmInteraction = 
            new ConfirmInteractionCallBack(ConfirmInteractionForms);

        /*delegated implementation*/
        /// <summary>
        /// Stub callback implementation for <see cref="ConfirmInteractionCallBack"/>. 
        /// Results in no interaction with the user. 
        /// Auto confirms all requestes by the application.
        /// </summary>
        static public void ConfirmInteractionAuto()
        {
            return;
        }
        /*delegated implementation*/
        /// <summary>
        /// Stub callback implementation for <see cref="ConfirmInteractionCallBack"/>.
        /// Results in a windows messagebox towards the user. 
        /// </summary>
        /// <remarks>
        /// The user needs to click the messagebox to continue the application.
        /// </remarks>
        static public void ConfirmInteractionForms()
        {
            System.Windows.Forms.MessageBox.Show("Click to continue.");
            return;
        }
        /*delegated implementation*/
        /// <summary>
        /// Stub callback implementation for <see cref="ConfirmInteractionCallBack"/>.
        /// Results in a console input request from the user. 
        /// </summary>
        /// <remarks>
        /// The user needs press the keyboard to continue the application.
        /// </remarks>
        static public void ConfirmInteractionConsole()
        {
            System.Console.WriteLine("Press key to continue.");
            System.Console.Read();
            return;
        }
        /// <summary>
        /// Selection option for the callback <see cref="ConfirmInteraction"/>.
        /// </summary>
        public enum ConfirmInteractionOptions
        {
            /// <summary>
            /// Installs callback <see cref="ConfirmInteractionAuto"/>
            /// </summary>
            AutoConfirm,
            /// <summary>
            /// Installs callback <see cref="ConfirmInteractionForms"/>
            /// </summary>
            Forms,
            /// <summary>
            /// Installs callback <see cref="ConfirmInteractionConsole"/>
            /// </summary>
            Console,
        }
        /// <summary>
        /// Simplifies the selection of the <see cref="ConfirmInteraction"/>.
        /// </summary>
        /// <remarks>
        /// Select the required confirmation interaction from the 
        /// options <see cref="ConfirmInteractionOptions"/>.
        /// </remarks>
        public ConfirmInteractionOptions ConfirmInteractionOption
        {
            set
            {
                switch(value)
                {
                    case ConfirmInteractionOptions.AutoConfirm:
                        this.ConfirmInteraction = 
                            new ConfirmInteractionCallBack(ConfirmInteractionAuto);
                        break;
                    case ConfirmInteractionOptions.Console:
                        this.ConfirmInteraction = 
                            new ConfirmInteractionCallBack(ConfirmInteractionForms);
                        break;
                    case ConfirmInteractionOptions.Forms:
                        this.ConfirmInteraction = 
                            new ConfirmInteractionCallBack(ConfirmInteractionConsole);
                        break;
                    default:
                        System.Diagnostics.Trace.Assert(false);
                        this.ConfirmInteraction = null;
                        break;
                }
            }
        }
    }
    internal class Confirmer
        : Wrappers.IConfirmInteractionTarget
    {
        internal Confirmer(ScriptSession scriptSession)
        {
            if (scriptSession == null) throw new System.ArgumentNullException();
            this.m_parentSession = scriptSession;
        }
        private ScriptSession m_parentSession;

        #region Wrappers.IConfirmInteractionTarget
        public void ConfirmInteraction()
        {
            // Call the delegate
            this.m_parentSession.ConfirmInteraction();
        }
        #endregion
    }

}