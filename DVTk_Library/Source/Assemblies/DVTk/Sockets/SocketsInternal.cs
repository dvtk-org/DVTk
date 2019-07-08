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

namespace DicomValidationToolKit.Sockets.Exceptions
{
    public class NoSocketConnectionException: ApplicationException
    {
        public NoSocketConnectionException()
        {
        }
        public NoSocketConnectionException(string message)
            : base(message)
        {
        }
        public NoSocketConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class SocketConnectionLostException: ApplicationException
    {
        public SocketConnectionLostException()
        {
        }
        public SocketConnectionLostException(string message)
            : base(message)
        {
        }
        public SocketConnectionLostException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
namespace DicomValidationToolKit.Sockets.Settings
{
    using FileName = System.String;
    using DicomValidationToolKit.Sessions;
    internal class SocketService
        : ChildService
        , ISocketParameters
    {
        private class CSutSocketService
            : ChildService
            , ISutSocketParameters
        {
            internal Wrappers.ISockets m_delegate;
            internal CSutSocketService(Session parentSession, Wrappers.ISockets iSocketsDelegate)
                : base(parentSession)
            {
                m_delegate = iSocketsDelegate;
            }
            public ProductRole ProductRole
            {
                get {
                    ProductRole productRole;
                    bool isAcceptor = this.m_delegate.ProductRoleIsAcceptor;
                    bool isRequestor = this.m_delegate.ProductRoleIsRequestor;
                    if (!isAcceptor && !isRequestor) throw new ApplicationException();
                    productRole = (
                        (isAcceptor && isRequestor)  ? ProductRole.AcceptorRequestor :
                        (isAcceptor && !isRequestor) ? ProductRole.Acceptor :
                        (!isAcceptor && isRequestor) ? ProductRole.Requestor :
                        //(!isAcceptor && !isRequestor) ? NOT ALLOWED :
                        ProductRole.Requestor
                        );
                    return productRole; 
                }
                set { 
                    switch (value)
                    {
                        case ProductRole.Acceptor:
                            this.m_delegate.ProductRoleIsAcceptor = true;
                            this.m_delegate.ProductRoleIsRequestor = false;
                            break;
                        case ProductRole.Requestor:
                            this.m_delegate.ProductRoleIsAcceptor = false;
                            this.m_delegate.ProductRoleIsRequestor = true;
                            break;
                        case ProductRole.AcceptorRequestor:
                            this.m_delegate.ProductRoleIsAcceptor = false;
                            this.m_delegate.ProductRoleIsRequestor = false;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            public System.String RemoteHostName
            {
                get { return this.m_delegate.RemoteHostName; }
                set { this.m_delegate.RemoteHostName = value; }
            }
            public System.UInt16 RemoteConnectPort
            {
                get { return this.m_delegate.RemoteConnectPort; }
                set { this.m_delegate.RemoteConnectPort = value; }
            }
        }
        private CSutSocketService m_sutSocketService = null;
        public ISutSocketParameters SutSocketParameters
        {
            get { return m_sutSocketService; }
        }
        private class CDvtSocketService
            : ChildService
            , IDvtSocketParameters
        {
            internal Wrappers.ISockets m_delegate;
            internal CDvtSocketService(Session parentSession, Wrappers.ISockets iSocketsDelegate)
                : base(parentSession)
            {
                m_delegate = iSocketsDelegate;
            }
            public System.UInt16 LocalListenPort
            {
                get { return this.m_delegate.LocalListenPort; }
                set { this.m_delegate.LocalListenPort = value; }
            }
            public System.UInt16 SocketTimeout
            {
                get { return this.m_delegate.SocketTimeOut; }
                set { this.m_delegate.SocketTimeOut = value; }
            }
        }
        private CDvtSocketService m_dvtSocketService = null;
        public IDvtSocketParameters DvtSocketParameters
        {
            get { return m_dvtSocketService; }
        }
        private class CSecureSocketService
            : ChildService
            , ISecureSocketParameters
        {
            internal Wrappers.ISockets m_delegate;
            internal CSecureSocketService(Session parentSession, Wrappers.ISockets iSocketsDelegate)
                : base(parentSession)
            {
                m_delegate = iSocketsDelegate;
            }
            public System.Boolean SecureSocketsEnabled
            {
                get { return this.m_delegate.UseSecureSockets; }
                set { this.m_delegate.UseSecureSockets = value; }
            }
            public System.String CertificateFilePassword
            {
                get { return this.m_delegate.TlsPassword; }
                set { this.m_delegate.TlsPassword = value; }
            }
            public System.String TlsVersion
            {
                get { return this.m_delegate.TlsVersion; }
                set { this.m_delegate.TlsVersion = value; }
            }
            public System.Boolean CheckRemoteCertificate
            {
                get { return this.m_delegate.CheckRemoteCertificate; }
                set { this.m_delegate.CheckRemoteCertificate = value; }
            }
            public System.String CipherList
            {
                get { return this.m_delegate.CipherList; }
                set { this.m_delegate.CipherList = value; }
            }
            public System.Boolean CacheTlsSessions
            {
                get { return this.m_delegate.CacheTlsSessions; }
                set { this.m_delegate.CacheTlsSessions = value; }
            }
            public System.UInt16 TlsCacheTimeout
            {
                get { return this.m_delegate.TlsCacheTimeout; }
                set { this.m_delegate.TlsCacheTimeout = value; }
            }
            public FileName CredentialsFileName
            {
                get { return this.m_delegate.CredentialsFilename; }
                set { this.m_delegate.CredentialsFilename = value; }
            }
            public FileName CertificateFileName
            {
                get { return this.m_delegate.CertificateFilename; }
                set { this.m_delegate.CertificateFilename = value; }
            }
            public System.Boolean ContentsChanged
            {
                set { this.m_delegate.SocketParametersChanged = value; }
            }
        }
        private CSecureSocketService m_secureSocketService = null;
        public ISecureSocketParameters SecureSocketParameters
        {
            get { return m_secureSocketService; }
        }
        internal Wrappers.ISockets m_delegate;
        internal SocketService(Session parentSession, Wrappers.ISockets iSocketsDelegate)
            : base(parentSession)
        {
            m_delegate = iSocketsDelegate;
            m_sutSocketService      = new CSutSocketService(parentSession, m_delegate);
            m_dvtSocketService      = new CDvtSocketService(parentSession, m_delegate);
            m_secureSocketService   = new CSecureSocketService(parentSession, m_delegate);
        }
    }
}
