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
    //
    // Aliases for types
    //
    using AeTitle = System.String;
    using UID = System.String;

    /// <summary>
    /// Indicates the role that the System Under Test (SUT) plays in the validation test. 
    /// </summary>
    public enum SutRole
    {
        /// <summary>
        /// It the System Under Test (SUT) is accepting TCP/IP connections.
        /// </summary>
        Acceptor,
        /// <summary>
        /// If the System Under Test (SUT) is initiating (requesting) TCP/IP connections.
        /// </summary>
        Requestor,
        /// <summary>
        /// The Product can both initiate and accept connections during the validation session.
        /// </summary>
        AcceptorRequestor,
    }
    /// <summary>
    /// TCP/IP socket connection related parameters for the System Under Test (SUT).
    /// </summary>
    public interface ISutSocketParameters
    {
        /// <summary>
        /// Indicates the role that the System Under Test (SUT) plays in the validation test. 
        /// </summary>
        SutRole CommunicationRole
        {
            get;
            set;
        }
        /// <summary>
        /// The name that the Dicom Validation Tool (DVT) should use when making a connection to the 
        /// product machine of the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// It is best to enter the Internet Address of the Product (in dot notation). 
        /// </remarks>
        System.String HostName
        {
            get;
            set;
        }
        /// <summary>
        /// The port number that the Dicom Validation Tool (DVT) should use when making a 
        /// connection to the product machine of the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// Also known as the remote connect port.
        /// </remarks>
        System.UInt16 Port
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Dicom Upper Layer (DUL) related parameters for the System Under Test (SUT).
    /// </summary>
    public interface ISutDulParameters
    {
        /// <summary>
        /// The DVT AE Title is the application entity name of the SUT machine in the test.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        AeTitle AeTitle
        {
            get;
            set;
        }
        /// <summary>
        /// The maximum length of message fragment (P-DATA-TF PDU) 
        /// that the System Under Test (SUT) can receive from the Dicom Validation Tool (DVT).
        /// </summary>
        /// <remarks>
        /// DICOM DIMSE-messages are split into P-DATA-TF PDU fragments - e.g., C-STORE-RQ of a modality image.
        /// </remarks>
        System.UInt32 MaximumLengthReceived
        {
            get;
            set;
        }
        /// <summary>
        /// This is the unique identifier (UID) for the Product's implementation of the System Under Test (SUT).
        /// </summary>
        /// <remarks>
        /// <p>
        /// The UID is assigned by the Manufacturer to the Product implementation to identify it.
        /// The manufacturer publishes this UID in the product DICOM conformance statement.
        /// </p>
        /// <p>
        /// The Dicom Validation Tool (DVT) checks that the value sent by the Product matches the value given here.
        /// </p>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        UID ImplementationClassUid
        {
            get;
            set;
        }
        /// <summary>
        /// The version name given by the Manufacturer to the Product implementation to identify it internally. 
        /// DVT checks that the value sent by the Product matches the values given here. 
        /// </summary>
        /// <remarks>
        /// The implementation version name is an optional field - 
        /// when the Product does not send this value leave this entry blank.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        System.String ImplementationVersionName
        {
            get;
            set;
        }
    }
    /// <summary>
    /// System parameters for the System Under Test (SUT).
    /// </summary>
    /// <remarks>
    /// Implements; <see cref="ISutSocketParameters"/> and <see cref="ISutDulParameters"/>.
    /// </remarks>
    public interface ISutSystemSettings
        : ISutSocketParameters
        , ISutDulParameters
    {
    }
    internal class SutSystemSettings : ISutSystemSettings
    {
        internal SutSystemSettings(Wrappers.MBaseSession adaptee)
        {
            if (adaptee == null) throw new System.ArgumentNullException("adaptee");
            m_adaptee = adaptee;
        }
        protected Wrappers.MBaseSession m_adaptee = null;

        private SutRole _Convert(Wrappers.SutRole role)
        {
            switch (role)
            {
                case Wrappers.SutRole.SutRoleAcceptor: return SutRole.Acceptor;
                case Wrappers.SutRole.SutRoleAcceptorRequestor: return SutRole.AcceptorRequestor;
                case Wrappers.SutRole.SutRoleRequestor: return SutRole.Requestor;
                default:
                    System.Diagnostics.Trace.Assert(false);
                    return SutRole.Acceptor;
            }
        }

        private Wrappers.SutRole _Convert(SutRole role)
        {
            switch (role)
            {
                case SutRole.Acceptor: return Wrappers.SutRole.SutRoleAcceptor;
                case SutRole.AcceptorRequestor: return Wrappers.SutRole.SutRoleAcceptorRequestor;
                case SutRole.Requestor: return Wrappers.SutRole.SutRoleRequestor;
                default:
                    System.Diagnostics.Trace.Assert(false);
                    return Wrappers.SutRole.SutRoleAcceptor;
            }
        }

        #region ISutSocketParameters
        /// <summary>
        /// <see cref="ISutSocketParameters.CommunicationRole"/>
        /// </summary>
        public SutRole CommunicationRole
        {
            get
            {
                return _Convert(this.m_adaptee.SutRole);
            }
            set
            {
                this.m_adaptee.SutRole = _Convert(value);
            }
        }
        /// <summary>
        /// <see cref="ISutSocketParameters.HostName"/>
        /// </summary>
        public System.String HostName
        {
            get
            {
                return this.m_adaptee.SutHostname;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.SutHostname = value;
            }
        }
        /// <summary>
        /// <see cref="ISutSocketParameters.Port"/>
        /// </summary>
        public System.UInt16 Port
        {
            get
            {
                return this.m_adaptee.SutPort;
            }
            set
            {
                this.m_adaptee.SutPort = value;
            }
        }
        #endregion

        #region ISutDulParameters
        /// <summary>
        /// <see cref="ISutDulParameters.AeTitle"/>
        /// </summary>
        public AeTitle AeTitle
        {
            get
            {
                return this.m_adaptee.SutAeTitle;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.SutAeTitle = value;
            }
        }
        /// <summary>
        /// <see cref="ISutDulParameters.MaximumLengthReceived"/>
        /// </summary>
        public System.UInt32 MaximumLengthReceived
        {
            get
            {
                return this.m_adaptee.SutMaximumLengthReceived;
            }
            set
            {
                this.m_adaptee.SutMaximumLengthReceived = value;
            }
        }
        /// <summary>
        /// <see cref="ISutDulParameters.ImplementationClassUid"/>
        /// </summary>
        public UID ImplementationClassUid
        {
            get
            {
                return this.m_adaptee.SutImplementationClassUid;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.SutImplementationClassUid = value;
            }
        }
        /// <summary>
        /// <see cref="ISutDulParameters.ImplementationVersionName"/>
        /// </summary>
        public System.String ImplementationVersionName
        {
            get
            {
                return this.m_adaptee.SutImplementationVersionName;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.SutImplementationVersionName = value;
            }
        }
        #endregion
    }
}