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

namespace DicomValidationToolKit.Sockets.Settings
{
    using FileName = System.String;
    using AeTitle = System.String;
    using DicomValidationToolKit.Sessions;
    public enum ProductRole
    {
        Acceptor,
        Requestor,
        AcceptorRequestor,
    }
    public interface ISutSocketParameters
    {
        ProductRole ProductRole
        {
            get;
            set;
        }
        System.String RemoteHostName
        {
            get;
            set;
        }
        System.UInt16 RemoteConnectPort
        {
            get;
            set;
        }
    }
    public interface IDvtSocketParameters
    {
        System.UInt16 LocalListenPort
        {
            get;
            set;
        }
        System.UInt16 SocketTimeout
        {
            get;
            set;
        }
    }
    public interface ISecureSocketParameters
    {
        System.Boolean SecureSocketsEnabled
        {
            get;
            set;
        }
        System.String CertificateFilePassword
        {
            get;
            set;
        }
        System.String TlsVersion
        {
            get;
            set;
        }
        System.Boolean CheckRemoteCertificate
        {
            get;
            set;
        }
        System.String CipherList
        {
            get;
            set;
        }
        System.Boolean CacheTlsSessions
        {
            get;
            set;
        }
        System.UInt16 TlsCacheTimeout
        {
            get;
            set;
        }
        FileName CredentialsFileName
        {
            get;
            set;
        }
        FileName CertificateFileName
        {
            get;
            set;
        }
        // indicates that the contents of one of the files has changed
        System.Boolean ContentsChanged
        {
            set;
        }
    }
    public interface ISocketParameters
    {
        ISutSocketParameters SutSocketParameters
        {
            get;
        }
        IDvtSocketParameters DvtSocketParameters
        {
            get;
        }
        ISecureSocketParameters SecureSocketParameters
        {
            get;
        }
    }
}