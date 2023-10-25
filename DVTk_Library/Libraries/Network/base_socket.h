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

//*****************************************************************************
//  DESCRIPTION     :	TCP/IP Base Socket class.
//*****************************************************************************
#ifndef BASE_SOCKET_H
#define BASE_SOCKET_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface

#include "base_io.h"		// Base IO class

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#define DEFAULT_CERTIFICATE_FILE_PASSWORD ":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA" // use something that doesn't look like a password
#define DEFAULT_TLS_VERSION "TLSv1"
#define DEFAULT_CIPHER_LIST "aRSA+kRSA+SHA1+eNULL"
#define DEFAULT_TLS_CACHE_TIMEOUT 300 /*seconds*/
#define DEFAULT_CREDENTIALS_FILENAME "credentials.pem"
#define DEFAULT_CERTIFICATE_FILENAME "certificate.pem"

#define TLS_VERSION_SSLv2 "SSLv2"
#define TLS_VERSION_SSLv3 "SSLv3"
#define TLS_VERSION_TLSv1 "TLSv1"
#define TLS_VERSION_TLSv1_1 "TLSv1.1"
#define TLS_VERSION_TLSv1_2 "TLSv1.2"
#define TLS_VERSION_TLSv1_3 "TLSv1.3"

#define TLS_AUTHENICATION_METHOD_RSA "aRSA"
#define TLS_AUTHENICATION_METHOD_DSA "aDSS"

#define TLS_KEY_EXCHANGE_METHOD_DH "DH"
#define TLS_KEY_EXCHANGE_METHOD_RSA "kRSA"

#define TLS_DATA_INTEGRITY_METHOD_SHA1 "SHA1"
#define TLS_DATA_INTEGRITY_METHOD_MD5 "MD5"

#define TLS_ENCRYPTION_METHOD_NONE "eNULL"
#define TLS_ENCRYPTION_METHOD_AES "AES"
#define TLS_ENCRYPTION_METHOD_3DES "3DES"
#define TLS_NO_AES_128 ":-DHE-RSA-AES128-SHA:-DHE-DSS-AES128-SHA:-AES128-SHA"
#define TLS_NO_AES_256 ":-DHE-RSA-AES256-SHA:-DHE-DSS-AES256-SHA:-AES256-SHA" 

#define TLS_POSTFIX ":@STRENGTH:-SSLv2"



//*****************************************************************************
//  Global function declarations
//*****************************************************************************

bool isCipherListValid(const char* cipherList);


//>>***************************************************************************

struct SOCKET_PARAMETERS

//  DESCRIPTION     : Structure that contains the attributes needed to construct a socket class.
//  INVARIANT       :
//  NOTES           : Used to pass the parameters between the session class (where they are stored)
//					: to the socket classes (where they are used).
//<<***************************************************************************
{
	string		remoteHostnameM;
	UINT16		remoteConnectPortM;
	UINT16		localListenPortM;
	int			socketTimeoutM; // the socket timeout in seconds
	bool		useSecureSocketsM; // indicates that secure sockets should be used
	string		certificateFilePasswordM; // this is not stored in the session file.  When the file is opened this value is asked from the user if needed. 
	string      maxTlsVersionM;
	string      minTlsVersionM;
	bool		checkRemoteCertificateM;
	string		cipherListM;
	bool		cacheTlsSessionsM;
	int			tlsCacheTimeoutM; // sets the session cache timeout time used by the TLS server.  Currently, no way to change this. 
	string		credentialsFilenameM;
	string		certificateFilenameM;
	bool		contentsChangedM; // indicates that the contents of one of the files has changed

	SOCKET_PARAMETERS();

	bool isTlsPasswordValid(bool& unencryptedKeyFound);
};


//>>***************************************************************************

class BASE_SOCKET_CLASS : public BASE_IO_CLASS

//  DESCRIPTION     : Abstract base class for the socket interfaces.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
protected:
	string		remoteHostnameM; // Hostname if the socket is used as a client
	UINT16		remoteConnectPortM; // Port to connect on if the socket is used as a client
	UINT16		localListenPortM; // Port to listen on it the socket is used as a server
	int			socketTimeoutM; // Socket timeout in seconds
	LOG_CLASS	*loggerM_ptr;
	THREAD_TYPE	ownerThreadIdM;

	BASE_SOCKET_CLASS();

	BASE_SOCKET_CLASS(const SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr);

	BASE_SOCKET_CLASS(const BASE_SOCKET_CLASS& socket);

public:
	virtual	~BASE_SOCKET_CLASS();

	virtual bool connect() = 0;

	virtual bool listen() = 0;

	virtual bool accept(BASE_SOCKET_CLASS** acceptedSocket_ptr_ptr) = 0;

	virtual void close() = 0;

	virtual bool isPendingDataInNetworkInputBuffer() = 0;

	virtual bool isConnected() = 0;

	virtual bool isSecure() = 0;

	void setLogger(LOG_CLASS*);

	void setOwnerThread(THREAD_TYPE tid) { ownerThreadIdM = tid; }

	virtual bool socketParametersChanged(const SOCKET_PARAMETERS& socketParams);

	static BASE_SOCKET_CLASS* createSocket(SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr);

	void setRemoteHostname(string hostname) { remoteHostnameM = hostname; }
	string getRemoteHostname() { return remoteHostnameM; }

	void setRemoteConnectPort(UINT16 port) { remoteConnectPortM = port; }
	UINT16 getRemoteConnectPort() { return remoteConnectPortM; }

	void setLocalListenPort(UINT16 port) { localListenPortM = port; }
	UINT16 getLocalListenPort() { return localListenPortM; }

	int getSocketTimeout() { return socketTimeoutM; }
};

#endif /* BASE_SOCKET_H */


