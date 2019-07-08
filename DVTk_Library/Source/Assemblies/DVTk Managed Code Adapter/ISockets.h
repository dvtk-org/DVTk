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

namespace Wrappers
{
    public __value enum SutRole
    {
        SutRoleAcceptorRequestor,
        SutRoleAcceptor,
        SutRoleRequestor,
    };

    public __gc __interface ISockets
    {
        __property Wrappers::SutRole get_SutRole();
        __property void set_SutRole(Wrappers::SutRole value);
        __property System::String __gc* get_SutHostname();
        __property void set_SutHostname(System::String __gc* value);
        __property System::UInt16 get_SutPort();
        __property void set_SutPort(System::UInt16 value);
        __property System::UInt16 get_DvtPort();
        __property void set_DvtPort(System::UInt16 value);
        __property System::UInt16 get_DvtSocketTimeOut();
        __property void set_DvtSocketTimeOut(System::UInt16 value);
        __property bool get_UseSecureSockets();
        __property void set_UseSecureSockets(bool value);
        __property System::String __gc* get_TlsPassword();
        __property void set_TlsPassword(System::String __gc* value);
        __property System::String __gc* get_TlsVersion();
        __property void set_TlsVersion(System::String __gc* value);
        __property bool get_CheckRemoteCertificate();
        __property void set_CheckRemoteCertificate(bool value);
        __property System::String __gc* get_CipherList();
        __property void set_CipherList(System::String __gc* value);
        __property bool get_CacheTlsSessions();
        __property void set_CacheTlsSessions(bool value);
        __property System::UInt16 get_TlsCacheTimeout();
        __property void set_TlsCacheTimeout(System::UInt16 value);
        __property System::String __gc* get_CredentialsFileName();
        __property void set_CredentialsFileName(System::String __gc* value);
        __property System::String __gc* get_CertificateFileName();
        __property void set_CertificateFileName(System::String __gc* value);
        __property void set_SocketParametersChanged(bool value);
    };
}
