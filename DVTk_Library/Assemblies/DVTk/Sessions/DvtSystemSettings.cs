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
    using AeTitle = System.String;
    using UID = System.String;

    /// <summary>
    /// TCP/IP socket connection related parameters for the Dicom Validation Tool (DVT).
    /// </summary>
    public interface IDvtSocketParameters
    {
        /// <summary>
        /// The port that the System Under Test (SUT) should use when making a connection to 
        /// the Dicom Validation Tool (DVT).
        /// </summary>
        /// <remarks>
        /// Also known as the local listen port.
        /// </remarks>
        System.UInt16 Port
        {
            get;
            set;
        }
        /// <summary>
        /// The period that the Dicom Validation Tool (DVT) will listen for incomming messages
        /// on the TCP/IP connection before automatically aborting the session.
        /// </summary>
        System.UInt16 SocketTimeout
        {
            get;
            set;
        }
    }
    /// <summary>
    /// Dicom Upper Layer (DUL) related parameters for the Dicom Validation Tool (DVT).
    /// </summary>
    public interface IDvtDulParameters
    {
        /// <summary>
        /// The DVT AE Title is the application entity name of the DVT machine in the test.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        AeTitle AeTitle
        {
            get;
            set;
        }
        /// <summary>
        /// The maximum length of message fragment (P-DATA-TF PDU) 
        /// that the Dicom Validation Tool (DVT) can receive from the System Under Test (SUT).
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
        /// This is the unique identifier (UID) for the Dicom Validation Tool (DVT) implementation.
        /// </summary>
        /// <remarks>
        /// <p>
        /// The UID identifies the release of the Dicom Validation Tool (DVT).
        /// </p>
        /// <p>
        /// The Dicom Validation Tool (DVT) sents this UID during communication with the System Under Test (SUT).
        /// </p>
        /// <p>
        /// The number starts is composed of the following items<br></br>
        /// [ASCII(d)].[ASCII(v)].[ASCII(t)].[year].[version_major].[version_minor]<br></br>
        /// <c>1.2.826.0.1.3680043.2.1545.1.2.1.7</c>
        /// </p>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        UID ImplementationClassUid
        {
            get;
            set;
        }
        /// <summary>
        /// This is the implementation version name for the Dicom Validation Tool (DVT) implementation.
        /// </summary>
        /// <remarks>
        /// The version is composed of the folloqing items<br></br>
        /// [dvt][version_major].[version_minor]<br></br>
        /// <c>dvtx.x</c>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Argument is a <see langword="null"/> reference.</exception>
        System.String ImplementationVersionName
        {
            get;
            set;
        }
    }
    /// <summary>
    /// System parameters for the Dicom Validation Tool (DVT).
    /// </summary>
    /// <remarks>
    /// Implements; <see cref="IDvtSocketParameters"/> and <see cref="IDvtDulParameters"/>.
    /// </remarks>
    public interface IDvtSystemSettings
        : IDvtSocketParameters
        , IDvtDulParameters
    {
    }
    internal class DvtSystemSettings : IDvtSystemSettings
    {
        internal DvtSystemSettings(Wrappers.MBaseSession adaptee)
        {
            m_adaptee = adaptee;
        }
        protected Wrappers.MBaseSession m_adaptee = null;

        #region IDvtSocketParameters
        /// <summary>
        /// <see cref="IDvtSocketParameters.Port"/>
        /// </summary>
        public System.UInt16 Port
        {
            get
            {
                return this.m_adaptee.DvtPort;
            }
            set
            {
                this.m_adaptee.DvtPort = value;
            }
        }
        /// <summary>
        /// <see cref="IDvtSocketParameters.SocketTimeout"/>
        /// </summary>
        public System.UInt16 SocketTimeout
        {
            get
            {
                return this.m_adaptee.DvtSocketTimeOut;
            }
            set
            {
                this.m_adaptee.DvtSocketTimeOut = value;
            }
        }
        #endregion

        #region IDvtDulParameters
        /// <summary>
        /// <see cref="IDvtDulParameters.AeTitle"/>
        /// </summary>
        public AeTitle AeTitle
        {
            get
            {
                return this.m_adaptee.DvtAeTitle;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.DvtAeTitle = value;
            }
        }
        /// <summary>
        /// <see cref="IDvtDulParameters.MaximumLengthReceived"/>
        /// </summary>
        public System.UInt32 MaximumLengthReceived
        {
            get
            {
                return this.m_adaptee.DvtMaximumLengthReceived;
            }
            set
            {
                this.m_adaptee.DvtMaximumLengthReceived = value;
            }
        }
        /// <summary>
        /// <see cref="IDvtDulParameters.ImplementationClassUid"/>
        /// </summary>
        public UID ImplementationClassUid
        {
            get
            {
                return this.m_adaptee.DvtImplementationClassUid;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.DvtImplementationClassUid = value;
            }
        }
        /// <summary>
        /// <see cref="IDvtDulParameters.ImplementationVersionName"/>
        /// </summary>
        public System.String ImplementationVersionName
        {
            get
            {
                return this.m_adaptee.DvtImplementationVersionName;
            }
            set
            {
                if (value == null) throw new System.ArgumentNullException();
                this.m_adaptee.DvtImplementationVersionName = value;
            }
        }
        #endregion
    }
}