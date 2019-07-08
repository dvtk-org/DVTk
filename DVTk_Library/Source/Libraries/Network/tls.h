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
//  DESCRIPTION     :	TCP/IP TLS/SSL Secure Socket class.
//*****************************************************************************
#ifndef TLS_H
#define TLS_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface
#include "base_socket.h"	// Socket base class interface
#include "openssl.h"		// OpenSSL library interface
#include <string>

//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************


//>>***************************************************************************

class TLS_SOCKET_CLASS : public BASE_SOCKET_CLASS

//  DESCRIPTION     : Class used to handle the TLS/SSL (secure) socket interface.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	SSL_CTX*	ctxM_ptr;					// pointer to the OpenSSL connection factory structure that
											// is used to generate SSL structures for this DVT session.

	SSL*		sslM_ptr;					// pointer to the OpenSSL SSL structure that contains the 
											// information for the socket. 

	BIO*		acceptBioM_ptr;				// OpenSSL BIO of the port the accept has been done on. 

	SSL_SESSION* savedClientSessionM_ptr;	// the last session used by a client.  If caching is enabled, 
											// will be used to attempt to reopen the session the next  
											// time a connection attempt is made. 

	OPENSSL_CLASS*	openSslM_ptr;			// pointer to the OpenSSL class
	
	bool		connectedM;
	bool		listeningM;
	bool		terminatingM;

	// configuration attributes
	string		certificateFilePasswordM;
	string		tlsVersionM;
	bool		checkRemoteCertificateM;
	string		cipherListM;
	bool		cacheTlsSessionsM;
	int			tlsCacheTimeoutM; // sets the session cache timeout time used by the TLS server.  Currently, no way to change this. 
	string		credentialsFilenameM;
	string		certificateFilenameM;

	// private methods
	TLS_SOCKET_CLASS(const TLS_SOCKET_CLASS& socket, SSL* newSsl_ptr);

	bool openSslInitialize();

	bool readCredentials(SSL_CTX* ctx_ptr);

	static int openSslVerifyCallback(int ok, X509_STORE_CTX* store);

	int verifyCertificate(int ok, X509_STORE_CTX* store_ptr);

	static void openSslMsgCallback(int write_p, int version, int content_type, const void *buf, 
									size_t len, SSL *ssl_ptr, void *this_ptr);

	void messageCallback(int write_p, int version, int content_type, const void *buf, size_t len, 
							SSL *ssl_ptr);

	long postConnectionCheck(SSL* ssl_ptr);

	void openSslError(UINT32 logLevel, const char *format_ptr, ...);

	void openSslError(const char *format_ptr, ...);

public:
	TLS_SOCKET_CLASS();

	TLS_SOCKET_CLASS(const TLS_SOCKET_CLASS& socket);

	TLS_SOCKET_CLASS(const SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr);
		
	virtual	~TLS_SOCKET_CLASS();		

	virtual bool socketParametersChanged(const SOCKET_PARAMETERS& socketParams);

	static bool isEncryptionLibPresent();

	virtual bool connect();

	virtual bool listen();

	virtual bool accept(BASE_SOCKET_CLASS** acceptedSocket_ptr_ptr);

	virtual void close();

	virtual bool isPendingDataInNetworkInputBuffer();

	virtual	bool writeBinary(const BYTE*, UINT);
		
	virtual	INT	readBinary(BYTE*, UINT);

	virtual bool isConnected() { return connectedM; }

	virtual bool isSecure() { return true; }
};

#endif /* TLS_H */


