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
//  DESCRIPTION      :	TCP/IP TLS/SSL Secure Socket class.
//  COPYRIGHT REMARK :  Portions of this code have been derived from the sample code provided with
//					 	the book "Network Security with OpenSSL" by Viega, Messier and Chandra 
//						(O'Reilly, 2002).  The code was downloaded from www.opensslbook.com.
//*****************************************************************************
#pragma warning( disable : 4127 )

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include <sstream>
#include <string>
#include "tls.h"

using std::string;

#define SSTR( x ) dynamic_cast< std::ostringstream & >( \
        ( std::ostringstream() << std::dec << x ) ).str()

//>>===========================================================================

TLS_SOCKET_CLASS::TLS_SOCKET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	connectedM = false;
	listeningM = false;
	terminatingM = false;
	certificateFilePasswordM = DEFAULT_CERTIFICATE_FILE_PASSWORD;
	tlsVersionM = DEFAULT_TLS_VERSION;
	checkRemoteCertificateM = true;
	cipherListM = DEFAULT_CIPHER_LIST;
	cacheTlsSessionsM = true;
	tlsCacheTimeoutM = DEFAULT_TLS_CACHE_TIMEOUT; // sets the session cache timeout time used by the TLS server.  Currently, no way to change this. 
	credentialsFilenameM = DEFAULT_CREDENTIALS_FILENAME;
	certificateFilenameM = DEFAULT_CERTIFICATE_FILENAME;
	ctxM_ptr = NULL;
	sslM_ptr = NULL;
	acceptBioM_ptr = NULL;
	savedClientSessionM_ptr = NULL;

	openSslInitialize();
}
		
//>>===========================================================================

TLS_SOCKET_CLASS::TLS_SOCKET_CLASS(const TLS_SOCKET_CLASS& socket) : BASE_SOCKET_CLASS(socket)

//  DESCRIPTION     : Copy Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	connectedM = socket.connectedM;
	listeningM = socket.listeningM;
	terminatingM = socket.terminatingM;
	certificateFilePasswordM = socket.certificateFilePasswordM;
	tlsVersionM = socket.tlsVersionM;
	checkRemoteCertificateM = socket.checkRemoteCertificateM;
	cipherListM = socket.cipherListM;
	cacheTlsSessionsM = socket.cacheTlsSessionsM;
	tlsCacheTimeoutM = socket.tlsCacheTimeoutM;
	credentialsFilenameM = socket.credentialsFilenameM;
	certificateFilenameM = socket.certificateFilenameM;
	sslM_ptr = socket.sslM_ptr;
	acceptBioM_ptr = socket.acceptBioM_ptr;
	savedClientSessionM_ptr = socket.savedClientSessionM_ptr;

	ctxM_ptr = socket.ctxM_ptr;
	CRYPTO_add(&ctxM_ptr->references,1,CRYPTO_LOCK_SSL_CTX);

	if (sslM_ptr != NULL)
	{
		// set the 'this' pointer for the callbacks openSslMsgCallback and openSslVerifyCallback
		SSL_set_msg_callback_arg(sslM_ptr, static_cast<void*>(this));
	}

}

//>>===========================================================================

TLS_SOCKET_CLASS::TLS_SOCKET_CLASS(const SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr) : 
											BASE_SOCKET_CLASS(socketParams, logger_ptr)

//  DESCRIPTION     : Create a socket filling in the parameters.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	connectedM = false;
	listeningM = false;
	terminatingM = false;
	certificateFilePasswordM = socketParams.certificateFilePasswordM;
	tlsVersionM = socketParams.tlsVersionM;
	checkRemoteCertificateM = socketParams.checkRemoteCertificateM;
	cipherListM = socketParams.cipherListM;
	cacheTlsSessionsM = socketParams.cacheTlsSessionsM;
	tlsCacheTimeoutM = socketParams.tlsCacheTimeoutM;
	credentialsFilenameM = socketParams.credentialsFilenameM;
	certificateFilenameM = socketParams.certificateFilenameM;
	ctxM_ptr = NULL;
	sslM_ptr = NULL;
	acceptBioM_ptr = NULL;
	savedClientSessionM_ptr = NULL;

	openSslInitialize();
}
		
//>>===========================================================================

TLS_SOCKET_CLASS::TLS_SOCKET_CLASS(const TLS_SOCKET_CLASS& socket, SSL* newSsl_ptr) : BASE_SOCKET_CLASS(socket)

//  DESCRIPTION     : Create a copy of the socket using newSsl_ptr as the connected socket.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities - the copy will be connected
	connectedM = true;
	listeningM = false;
	terminatingM = false;
	certificateFilePasswordM = socket.certificateFilePasswordM;
	tlsVersionM = socket.tlsVersionM;
	checkRemoteCertificateM = socket.checkRemoteCertificateM;
	cipherListM = socket.cipherListM;
	cacheTlsSessionsM = socket.cacheTlsSessionsM;
	tlsCacheTimeoutM = socket.tlsCacheTimeoutM;
	credentialsFilenameM = socket.credentialsFilenameM;
	certificateFilenameM = socket.certificateFilenameM;
	sslM_ptr = newSsl_ptr;
	acceptBioM_ptr = NULL;
	savedClientSessionM_ptr = NULL;

	ctxM_ptr = socket.ctxM_ptr;
	CRYPTO_add(&ctxM_ptr->references,1,CRYPTO_LOCK_SSL_CTX);

	if (sslM_ptr != NULL)
	{
		// set the 'this' pointer for the callbacks openSslMsgCallback and openSslVerifyCallback
		SSL_set_msg_callback_arg(sslM_ptr, static_cast<void*>(this));
	}
}
		
//>>===========================================================================

TLS_SOCKET_CLASS::~TLS_SOCKET_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// set this thread to the owner so the memory will get freed
	ownerThreadIdM = getThreadId();

	// close the socket
	close();

	// free any saved sessions
	if (savedClientSessionM_ptr != NULL)
	{
		SSL_SESSION_free(savedClientSessionM_ptr);
	}

	// free the CTX
	if (ctxM_ptr != NULL)
	{
		if (ctxM_ptr->references == 1)
		{
			// the ctx structure will be freed, delete the password memory
			delete [] (char*)ctxM_ptr->default_passwd_callback_userdata;
		}

		SSL_CTX_free(ctxM_ptr); // this will decrement the reference count and delete the structure if 0
	}
}


//>>===========================================================================

bool TLS_SOCKET_CLASS::socketParametersChanged(const SOCKET_PARAMETERS& socketParams)

//  DESCRIPTION     : Determines if the given socket parameters are different than the parameters given in the call.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if (!socketParams.useSecureSocketsM)
	{
		// they must have changed, since we use secure sockets here
		return true;
	}

	if ((certificateFilePasswordM == socketParams.certificateFilePasswordM) &&
		(tlsVersionM == socketParams.tlsVersionM) &&
		(checkRemoteCertificateM == socketParams.checkRemoteCertificateM) &&
		(cipherListM == socketParams.cipherListM) &&
		(cacheTlsSessionsM == socketParams.cacheTlsSessionsM) &&
		(tlsCacheTimeoutM == socketParams.tlsCacheTimeoutM) &&
		(credentialsFilenameM == socketParams.credentialsFilenameM) &&
		(certificateFilenameM == socketParams.certificateFilenameM))

	{
		// all of the TLS specific parameters are the same, check the base class parameters
		return BASE_SOCKET_CLASS::socketParametersChanged(socketParams);
	}
	else
	{
		// at least one of the TLS specific parameters changed
		return true;
	}
}


//>>===========================================================================

bool TLS_SOCKET_CLASS::openSslInitialize()

//  DESCRIPTION     : Called to initialize the OpenSSL library for this session.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	int verify_mode;
	long ssl_options;

	openSslM_ptr = OPENSSL_CLASS::getInstance();
	if (openSslM_ptr == NULL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "OpenSSL library not initialized when initializing Secure Socket");
		}
		return false;
	}

	// setup the connection factory for this session
	if (tlsVersionM == TLS_VERSION_TLSv1)
	{
		ctxM_ptr = SSL_CTX_new(TLSv1_method());
	}
	else if (tlsVersionM == TLS_VERSION_SSLv3)
	{
		ctxM_ptr = SSL_CTX_new(SSLv3_method());
	}
	else
	{
		ctxM_ptr = SSL_CTX_new(SSLv23_method());
	}

	if (ctxM_ptr == NULL)
	{
		openSslError("initializing connection factory");
		return false;
	}

	SSL_CTX_set_default_passwd_cb(ctxM_ptr, OPENSSL_CLASS::openSslPasswordCallback);
	char *password = new char[certificateFilePasswordM.length() + 1]; // create a buffer to store the password
																	  // this is freed in the destructor
	strcpy(password, certificateFilePasswordM.c_str());
	SSL_CTX_set_default_passwd_cb_userdata(ctxM_ptr, (void *)password);

	if (SSL_CTX_load_verify_locations(ctxM_ptr, certificateFilenameM.c_str(), NULL) != 1)
	{
		openSslError("loading trusted certificate file");
	}

	SSL_CTX_set_client_CA_list(ctxM_ptr, SSL_load_client_CA_file(certificateFilenameM.c_str()));
	ERR_clear_error(); // the last call leaves an error in the stack

	if (!readCredentials(ctxM_ptr))
	{
		openSslError("loading credentials");
	}

	if (checkRemoteCertificateM)
	{
		verify_mode = SSL_VERIFY_PEER | SSL_VERIFY_FAIL_IF_NO_PEER_CERT;
	}
	else
	{
		verify_mode = SSL_VERIFY_NONE;
	}
	SSL_CTX_set_verify(ctxM_ptr, verify_mode, openSslVerifyCallback);

	if ((loggerM_ptr != NULL) && ((loggerM_ptr->getLogMask() & LOG_DEBUG) != 0))
	{
		SSL_CTX_set_msg_callback(ctxM_ptr, openSslMsgCallback);
		// the 'this' pointer needed by openSslMsgCallback and openSslVerfyCallback must be set by 
		// SSL_set_msg_callback_arg for each SSL created
	}

	ssl_options = SSL_OP_ALL | SSL_OP_SINGLE_DH_USE;
	if (tlsVersionM.find(TLS_VERSION_SSLv2) == string::npos)
	{
		ssl_options |= SSL_OP_NO_SSLv2;
	}
	if (tlsVersionM.find(TLS_VERSION_SSLv3) == string::npos)
	{
		ssl_options |= SSL_OP_NO_SSLv3;
	}
	if (tlsVersionM.find(TLS_VERSION_TLSv1) == string::npos)
	{
		ssl_options |= SSL_OP_NO_TLSv1;
	}

	SSL_CTX_set_options(ctxM_ptr, ssl_options);

	SSL_CTX_set_timeout(ctxM_ptr, tlsCacheTimeoutM);
	SSL_CTX_set_session_id_context(ctxM_ptr, (const unsigned char *)"DVT", 3);
	if (cacheTlsSessionsM)
	{
		SSL_CTX_set_session_cache_mode(ctxM_ptr, SSL_SESS_CACHE_BOTH);
	}
	else
	{
		SSL_CTX_set_session_cache_mode(ctxM_ptr, SSL_SESS_CACHE_OFF);
	}

	SSL_CTX_set_tmp_dh_callback(ctxM_ptr, OPENSSL_CLASS::tmpDhCallback);

	if (SSL_CTX_set_cipher_list(ctxM_ptr, cipherListM.c_str()) != 1)
	{
		cipherListM = "aRSA+kRSA+SHA1+3DES:@STRENGTH:-SSLv2";
		SSL_CTX_set_cipher_list(ctxM_ptr, cipherListM.c_str());
		openSslError("initializing cipher list (no valid ciphers)");
	}
	
	return true;
}


//>>===========================================================================

bool TLS_SOCKET_CLASS::readCredentials(SSL_CTX* ctx_ptr)

//  DESCRIPTION     : Reads the credentials file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool ret = false;
	X509* certificate_ptr = NULL;
	X509* caCert_ptr = NULL;
	EVP_PKEY* rsaPrivateKey_ptr;
	STACK_OF(X509)* rsaCertChain_ptr;
	EVP_PKEY* dsaPrivateKey_ptr;
	STACK_OF(X509)* dsaCertChain_ptr;

	// clear the current certificate list
	if (ctx_ptr->extra_certs != NULL) 
	{
		sk_X509_pop_free(ctx_ptr->extra_certs, X509_free);
		ctx_ptr->extra_certs = NULL;
	}

	// read the credentials file
	openSslM_ptr->readCredentialsFile(credentialsFilenameM.c_str(), 
		&rsaPrivateKey_ptr, &rsaCertChain_ptr,
		&dsaPrivateKey_ptr, &dsaCertChain_ptr,
		ctx_ptr->default_passwd_callback, ctx_ptr->default_passwd_callback_userdata, loggerM_ptr);

	if (rsaPrivateKey_ptr != NULL)
	{
		// have a RSA private key
		if (SSL_CTX_use_PrivateKey(ctx_ptr, rsaPrivateKey_ptr) != 1)
		{
			goto end;
		}

		// get and save the certificate that goes with the RSA private key
		certificate_ptr = sk_X509_shift(rsaCertChain_ptr);
		if (SSL_CTX_use_certificate(ctx_ptr, certificate_ptr) != 1)
		{
			goto end;
		}
		if (ERR_peek_error() != 0)
		{
			ret = 0;  // key/certificate mismatch doesn't imply ret==0 ...
			goto end;
		}
		X509_free(certificate_ptr);
		certificate_ptr = NULL;

		// save the rest of the certificate chain
		while ((caCert_ptr = sk_X509_shift(rsaCertChain_ptr)) != NULL)
		{
			if (SSL_CTX_add_extra_chain_cert(ctx_ptr, caCert_ptr) != 1)
			{
				X509_free(caCert_ptr);
				goto end;
			}
			// note that caCert_ptr must not be freed if it was successfully added to the chain
		}
	}

	if (dsaPrivateKey_ptr != NULL)
	{
		// have a DSA private key
		if (SSL_CTX_use_PrivateKey(ctx_ptr, dsaPrivateKey_ptr) != 1)
		{
			goto end;
		}

		// get and save the certificate that goes with the DSA private key
		certificate_ptr = sk_X509_shift(dsaCertChain_ptr);
		if (SSL_CTX_use_certificate(ctx_ptr, certificate_ptr) != 1)
		{
			goto end;
		}
		if (ERR_peek_error() != 0)
		{
			ret = 0;  // key/certificate mismatch doesn't imply ret==0 ...
			goto end;
		}
		X509_free(certificate_ptr);
		certificate_ptr = NULL;

		// save the rest of the certificate chain
		while ((caCert_ptr = sk_X509_shift(dsaCertChain_ptr)) != NULL)
		{
			if (SSL_CTX_add_extra_chain_cert(ctx_ptr, caCert_ptr) != 1)
			{
				X509_free(caCert_ptr);
				goto end;
			}
			// note that caCert_ptr must not be freed if it was successfully added to the chain
		}
	}

	ret = true;

end:
	if (certificate_ptr != NULL) X509_free(certificate_ptr);
	if (rsaPrivateKey_ptr != NULL) EVP_PKEY_free(rsaPrivateKey_ptr);
	if (dsaPrivateKey_ptr != NULL) EVP_PKEY_free(dsaPrivateKey_ptr);
	if (rsaCertChain_ptr != NULL) sk_X509_pop_free(rsaCertChain_ptr, X509_free);
	if (dsaCertChain_ptr != NULL) sk_X509_pop_free(dsaCertChain_ptr, X509_free);

	return ret;
}

//>>===========================================================================

bool TLS_SOCKET_CLASS::isEncryptionLibPresent()

//  DESCRIPTION     : Checks to see if the OpenSSL DLLs are present and loads them if they are.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This is a static method
//<<===========================================================================
{
	OPENSSL_CLASS* openSsl_ptr;

	openSsl_ptr = OPENSSL_CLASS::getInstance();

	if (openSsl_ptr != NULL)
	{
		return true;
	}
	else
	{
		return false;
	}
}


//>>===========================================================================

bool TLS_SOCKET_CLASS::connect()

//  DESCRIPTION     : Set up connection to the remote host.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	BIO* bio_ptr;
	long error;
	ostringstream remoteHost;

	// make sure the socket is not already in use
	if (terminatingM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - In process of terminating.  Cannot connect.");
		}

		// return - in process of termintating
		return false;
	}
	if (connectedM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Already connected to \"%s\".  Cannot connect again.", remoteHostnameM.c_str());
		}

		// return - already connected
		return false;
	}
	else if (listeningM)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Socket is already listening to port %d.  Cannot connect to \"%s\".", 
				localListenPortM, remoteHostnameM.c_str());
		}

		// return - socket is already being used to listen
		return false;
	}
	connectedM = false;
	
	if (remoteHostnameM.length() == 0)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - No Remote Hostname defined. Can't make connection.");
		}

		// return - don't know what to connect to
		return false;
	}

	// create the transport object
	remoteHost << remoteHostnameM << ':' << remoteConnectPortM;
	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - tls::connect(%s)", remoteHost.str().c_str());
	}
	bio_ptr = BIO_new_connect(const_cast<char *>(remoteHost.str().c_str()));
	//bio_ptr = BIO_new_connect("krypted.com:443");
	
	if (!bio_ptr)
	{
		openSslError("creating connection");
		return false;
	}

	

	if (BIO_do_connect(bio_ptr) <= 0)
	{
		openSslError("connecting to remote machine");
		BIO_free(bio_ptr);
		return false;
	}

	// create the secure socket object
	if (sslM_ptr != NULL)
	{
		SSL_free(sslM_ptr);
	}
	sslM_ptr = SSL_new(ctxM_ptr);
	if (sslM_ptr == NULL)
	{
		openSslError("creating secure socket object");
		BIO_free(bio_ptr);
		return false;
	}

	// set the 'this' pointer for the callbacks openSslMsgCallback and openSslVerifyCallback
	SSL_set_msg_callback_arg(sslM_ptr, static_cast<void*>(this));

	// restore the session
	if (cacheTlsSessionsM && (savedClientSessionM_ptr != NULL))
	{
		SSL_set_session(sslM_ptr, savedClientSessionM_ptr);
		SSL_SESSION_free(savedClientSessionM_ptr); // we're done with the saved session now, we'll 
													// get a new one when the session is closed.
		savedClientSessionM_ptr = NULL;
	}

	// make the SSL connection
	SSL_set_bio(sslM_ptr, bio_ptr, bio_ptr); // the ssl takes over the mangement of the bio memory
	if (SSL_connect(sslM_ptr) <= 0)
	{
		openSslError("connecting to %s", remoteHost.str().c_str());
		SSL_free(sslM_ptr);
		sslM_ptr = NULL;
		return false;
	}

	// make sure everything is OK
	error = postConnectionCheck(sslM_ptr);
	if (error != X509_V_OK)
	{
		openSslError("checking connection parameters after connecting to %s", remoteHost.str().c_str());
		SSL_free(sslM_ptr);
		sslM_ptr = NULL;
		return false;
	}

	// socket connected to peer
	connectedM = true;

	if (loggerM_ptr && (loggerM_ptr->getLogMask() & LOG_DEBUG))
	{
		char buffer[128];

		SSL_CIPHER_description(SSL_get_current_cipher(sslM_ptr), buffer, 128);
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - %s connection opened using cipher %s",
			SSL_get_version(sslM_ptr), buffer);
	}

	// return whether connected or not
	return connectedM;
}

//>>===========================================================================

bool TLS_SOCKET_CLASS::listen()

//  DESCRIPTION     : Setup the listen port.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	ostringstream listenPort;

	// make sure the socket is not already in use
	if (terminatingM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - In process of terminating.  Cannot listen.");
		}

		// return - in process of termintating
		return false;
	}
	if (connectedM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Already connected to \"%s\".  Cannot listen to port %d.", 
				remoteHostnameM.c_str(), localListenPortM);
		}

		// return - already connected
		return false;
	}
	else if (listeningM)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Socket is already listening to port %d.  Cannot listen again.", 
				localListenPortM);
		}

		// return - socket is already being used to listen
		return false;
	}
	listeningM = false;

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - tls::listen(%d)", localListenPortM);
	}
 
	if (acceptBioM_ptr != NULL)
	{
		BIO_free(acceptBioM_ptr);
		acceptBioM_ptr = NULL;
	}

	// create the socket
	listenPort << localListenPortM;

	std::string port = SSTR( localListenPortM );

	char *ipadress = "0.0.0.0:";
	char buffer[256]; // <- danger, only storage for 256 characters.
	strncpy(buffer, ipadress, sizeof(buffer));
	strncat(buffer, port.c_str(), sizeof(buffer));

	char *ipandport = buffer;

	acceptBioM_ptr = BIO_new_accept(ipandport);

	if (acceptBioM_ptr == NULL)
	{
		openSslError("creating server socket to port %d", localListenPortM);
		return false;
	}

	// bind to the port
	if (BIO_do_accept(acceptBioM_ptr) <= 0)
	{
		openSslError("binding server socket to port %d", localListenPortM);
		BIO_free(acceptBioM_ptr);
		acceptBioM_ptr = NULL;
		return false;
	}
 
	// listen socket set up
	listeningM = true;

	// return whether listening or not
	return listeningM;
}

//>>===========================================================================

bool TLS_SOCKET_CLASS::accept(BASE_SOCKET_CLASS** acceptedSocket_ptr_ptr)

//  DESCRIPTION     : Accept connection from listen socket.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The returned socket was new'ed so it must be deleted by the caller
//<<===========================================================================
{
	*acceptedSocket_ptr_ptr = NULL;
	BIO* acceptedBio_ptr;
	SSL* acceptedSsl_ptr;
	long error;
	int fileDesc;
	struct fd_set fds;
	struct timeval tv = {1, 0}; // always timeout in 1 second
	int sel;

	if (terminatingM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - In process of terminating.  Cannot accept.");
		}

		// return - in process of termintating
		return false;
	}

	// make sure we are listening to the port
	if (!listeningM)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Socket is not listening to port %d.  Cannot accept a connection.", 
				localListenPortM);
		}
		return false;
	}
	if (acceptBioM_ptr == NULL)
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Socket is listening to port %d, but not bound.", 
				localListenPortM);
		}
		listeningM = false;
		return false;
	}

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - tls::accept()");
	}

	// get the file descriptor and set up the file descriptor set for select()
	fileDesc = BIO_get_fd(acceptBioM_ptr, NULL);
	if (fileDesc == -1)
	{
		openSslError("getting listen socket file descriptor");
		return false;
	}

	// wait for a connection
	do
	{
		FD_ZERO(&fds);
		FD_SET(fileDesc, &fds);

		sel = select(fileDesc + 1, &fds, NULL, NULL, &tv);
		if (sel == 0)
		{
			// no data at the end of the timeout - check for terminating
			if (terminatingM)
			{
				return false;
			}
		}
		else if (sel == SOCKET_ERROR)
		{
			// socket error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Error waiting to accept connection (error code %d)", WSAGetLastError());
			}
			return false;
		}
		else if (sel != 1)
		{
			// unknown error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Unknown error while waiting to accept connection (select returned %d)", sel);
			}
			return false;
		}

	} while (sel != 1);

	// accept a connection
	if (BIO_do_accept(acceptBioM_ptr) <= 0)
	{
		openSslError(LOG_DEBUG, "accepting connection on port %d", localListenPortM);
		close();
		return false;
	}

	// get the new connection
	acceptedBio_ptr = BIO_pop(acceptBioM_ptr);

	// setup the socket structure
	acceptedSsl_ptr = SSL_new(ctxM_ptr);
	if (acceptedSsl_ptr == NULL)
	{
		openSslError("creating accepted secure socket object");
		BIO_free(acceptedBio_ptr);
		return false;
	}

	// set the 'this' pointer for the callbacks openSslMsgCallback and openSslVerifyCallback
	SSL_set_msg_callback_arg(acceptedSsl_ptr, static_cast<void*>(this));

	SSL_set_accept_state(acceptedSsl_ptr);
	SSL_set_bio(acceptedSsl_ptr, acceptedBio_ptr, acceptedBio_ptr); // the ssl takes over the BIO memory
	if (SSL_accept(acceptedSsl_ptr) <= 0)
	{
		openSslError("accepting secure connection to port %d", localListenPortM);
		SSL_free(acceptedSsl_ptr);
		return false;
	}

	// make sure everything is OK
	error = postConnectionCheck(acceptedSsl_ptr);
	if (error != X509_V_OK)
	{
		openSslError("checking connection parameters after accepting from port %d", localListenPortM);
		SSL_free(acceptedSsl_ptr);
		return false;
	}

	// create a socket for the new connection
	*acceptedSocket_ptr_ptr = new TLS_SOCKET_CLASS(*this, acceptedSsl_ptr);

	if (loggerM_ptr && (loggerM_ptr->getLogMask() & LOG_DEBUG))
	{
		char buffer[128];

		SSL_CIPHER_description(SSL_get_current_cipher(acceptedSsl_ptr), buffer, 128);
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - %s connection opened using cipher %s",
			SSL_get_version(acceptedSsl_ptr), buffer);
	}

	return true;
}

//>>===========================================================================

void TLS_SOCKET_CLASS::close()

//  DESCRIPTION     : Close socket down.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (ownerThreadIdM != getThreadId())
	{
		// this thread does not own the socket, just set the terminatng flag and let the owning 
		// thread take care of the close
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - tls::close() - starting to terminate connection");
		}

		terminatingM = true;
		return;
	}

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - tls::close()");
	}

	if (connectedM)
	{
		if (cacheTlsSessionsM)
		{
			if (savedClientSessionM_ptr != NULL)
			{
				SSL_SESSION_free(savedClientSessionM_ptr);
			}

			// cache the session
			savedClientSessionM_ptr = SSL_get1_session(sslM_ptr);
		}

		// shutdown the connection - send our shutdown message
		if (SSL_shutdown(sslM_ptr) == 0)
		{
			// don't wait for the peer shutdwon message - they may not respond, which will cause us to block
		}

		connectedM = false;
	}
	
	if (listeningM)
	{
		// the freeing of the acceptBio below closes the socket
		listeningM = false;
	}
	
	if (acceptBioM_ptr != NULL)
	{
		BIO_free(acceptBioM_ptr);
		acceptBioM_ptr = NULL;
	}

	if (sslM_ptr != NULL)
	{
		SSL_free(sslM_ptr);
		sslM_ptr = NULL;
	}

	// report any outstanding SSL errors
	if (ERR_peek_error() != 0)
	{
		openSslError("closing socket");
	}

	// free the OpenSSL error state.  This really needs to be done on a per thread basis.
	ERR_remove_state(0);

	connectedM = false;
	listeningM = false;
	terminatingM = false;
}

//>>===========================================================================

bool TLS_SOCKET_CLASS::isPendingDataInNetworkInputBuffer()

//  DESCRIPTION     : Check for pending data.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	/*int nrOfBytes;
	if(sslM_ptr != NULL)
		nrOfBytes = SSL_pending(sslM_ptr);
	if(nrOfBytes > 0) 
		return true;
	else
		return false;*/

	DWORD available;
	int handle = SSL_get_rfd(sslM_ptr);
	ioctlsocket(handle, FIONREAD, &available);
	if(available > 0) 
		return true;
	else
		return false;
}

//>>===========================================================================

bool TLS_SOCKET_CLASS::writeBinary(const BYTE *buffer_ptr, UINT length)

//  DESCRIPTION     : Write given data to socket.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int written_bytes = 0;
	int readFileDesc;
	int writeFileDesc;
	struct fd_set readFds;
	struct fd_set writeFds;
	bool waitOnWrite;
	int timeoutRemaining; // the amount of time left in the timeout period
	struct timeval tv = {1, 0}; // always timeout in 1 second

	if (terminatingM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - In process of terminating.  Cannot write.");
		}

		// return - in process of termintating
		return false;
	}

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - tls::write(%d bytes)", length);
	}

	if (!connectedM) 
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Not connected to peer - can't write data");
		}

		return false;
	}

	// get the file descriptors and set up the file descriptor sets for select()
	readFileDesc = SSL_get_rfd(sslM_ptr);
	if (readFileDesc == -1)
	{
		openSslError("getting read socket file descriptor");
		return false;
	}

	writeFileDesc = SSL_get_wfd(sslM_ptr);
	if (writeFileDesc == -1)
	{
		openSslError("getting write socket file descriptor");
		return false;
	}

	waitOnWrite = true;
	timeoutRemaining = socketTimeoutM;

	// write the buffer contents to socket
	while (written_bytes < (int) length)
	{
		int bytes;
		int sel;

		if (waitOnWrite)
		{
			FD_ZERO(&writeFds);
			FD_SET(writeFileDesc, &writeFds);

			// wait for something to write
			sel = select(writeFileDesc + 1, NULL, &writeFds, NULL, &tv);
		}
		else
		{
			FD_ZERO(&readFds);
			FD_SET(readFileDesc, &readFds);

			// wait for something to read
			sel = select(readFileDesc + 1, &readFds, NULL, NULL, &tv);
		}

		if (terminatingM)
		{
			return false;
		}

		waitOnWrite = true;

		if (sel == 1)
		{
			// data read to write (or possibly read - but use the same call)
			bytes = SSL_write(sslM_ptr, (char *)(buffer_ptr + written_bytes), (length - written_bytes));
			if (bytes > 0)
			{
				// wrote some data
				written_bytes += bytes;
				timeoutRemaining = socketTimeoutM; // reset the timeout
			}
			else if (bytes == 0)
			{
				// socket closed
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Secure connection closed during socket write");
				}
				return false;
			}
			else
			{
				// operation did not complete, see what happened
				int err = SSL_get_error(sslM_ptr, bytes);
				if (err == SSL_ERROR_WANT_READ)
				{
					// need to wait for data to be available for reading, and then retry the same operation
					waitOnWrite = false;
					timeoutRemaining = socketTimeoutM; // reset the timeout
				}
				if (err == SSL_ERROR_WANT_WRITE)
				{
					// need to wait for the write file descriptor to be able to write and then try again
					timeoutRemaining = socketTimeoutM; // reset the timeout
				}
				else
				{
					// an error occured
					if ((err == SSL_ERROR_SYSCALL) && (ERR_peek_error() == 0) && (bytes == -1))
					{
						// an error in the system call occured
						openSslError("writing to secure socket");
						if (loggerM_ptr && (loggerM_ptr->getLogMask() & LOG_ERROR))
						{
							loggerM_ptr->text(LOG_NONE, 1, "    write() error code = %d", WSAGetLastError());
						}
					}
					else
					{
						openSslError("reading from secure socket");
					}
					return false;
				}
			}
		}
		else if (sel == 0)
		{
			// no data at the end of the timeout
			if (--timeoutRemaining > 0)
			{
				// still have time left on the timeout, go back and wait some more
			}
			else
			{
				// timeout expired
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Connection timed-out while waiting to send data");
				}
				return false;
			}
		}
		else if (sel == SOCKET_ERROR)
		{
			// socket error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Error waiting to send data (error code %d)", WSAGetLastError());
			}
			return false;
		}
		else
		{
			// unknown error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Unknown error while waiting to send data (select returned %d)", sel);
			}
			return false;
		}
	}

	return (written_bytes == (int) length) ? true : false;
}
		
//>>===========================================================================

INT	TLS_SOCKET_CLASS::readBinary(BYTE *buffer_ptr, UINT length)

//  DESCRIPTION     : Read data from socket to given buffer.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Blocks until all of the requested data is read or a timeout occurs.
//<<===========================================================================
{
	int read_bytes = 0; // number of bytes read from the socket
	int readFileDesc;
	int writeFileDesc;
	struct fd_set readFds;
	struct fd_set writeFds;
	bool sslWaitOnRead; // indicates that the OpenSSL library is waiting for data to be sent on the socket
	bool sslWaitOnWrite; // indicates that the OpenSSL library is waiting to be able to write to the socket interface
	int timeoutRemaining; // the amount of time left in the timeout period
	struct timeval tv = {1, 0}; // always timeout in 1 second

	if (terminatingM)
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - In process of terminating.  Cannot read.");
		}

		// return - in process of termintating
		return -1;
	}

	if (loggerM_ptr) 
	{
		loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - tls::read(%d bytes)", length);
	}

	if (!connectedM) 
	{
		if (loggerM_ptr) 
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Not connected to peer - can't read data");
		}

		return -1;
	}

	// get the file descriptors and set up the file descriptor sets for select()
	readFileDesc = SSL_get_rfd(sslM_ptr);
	if (readFileDesc == -1)
	{
		openSslError("getting read socket file descriptor");
		return -1;
	}

	writeFileDesc = SSL_get_wfd(sslM_ptr);
	if (writeFileDesc == -1)
	{
		openSslError("getting write socket file descriptor");
		return -1;
	}

	sslWaitOnRead = false;
	sslWaitOnWrite = false;
	timeoutRemaining = socketTimeoutM;

	// fill the buffer from the socket
	while (read_bytes < (int) length)
	{
		int bytes;
		int sel;

		if (sslWaitOnWrite)
		{
			FD_ZERO(&writeFds);
			FD_SET(writeFileDesc, &writeFds);

			// wait for the socket to be able to accept a write operation
			sel = select(writeFileDesc + 1, NULL, &writeFds, NULL, &tv);
		}
		else if (sslWaitOnRead || // OpenSSL asked us to wait
				(SSL_pending(sslM_ptr) == 0)) // there is no data in the SSL buffer
		{
			FD_ZERO(&readFds);
			FD_SET(readFileDesc, &readFds);

			// wait for something on the socket
			sel = select(readFileDesc + 1, &readFds, NULL, NULL, &tv);
		}
		else
		{
			// don't wait, there is data to be read from the SSL buffer
			sel = 1;
		}

		if (terminatingM)
		{
			return -1;
		}

		sslWaitOnRead = false;
		sslWaitOnWrite = false;

		if (sel == 1)
		{
			// data ready to be read (or possibly write - but use the same call)
			bytes = SSL_read(sslM_ptr, (char *)(buffer_ptr + read_bytes), (length - read_bytes));
			if (bytes > 0)
			{
				// read some data
				read_bytes += bytes;
				timeoutRemaining = socketTimeoutM; // reset the timeout
			}
			else if (bytes == 0)
			{
				// socket closed
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Secure connection closed during socket read");
				}
				return -1;
			}
			else
			{
				// operation did not complete, see what happened
				int err = SSL_get_error(sslM_ptr, bytes);
				if (err == SSL_ERROR_WANT_READ)
				{
					// need to wait for data to be available for reading, and then retry the same operation
					sslWaitOnRead = true;
					timeoutRemaining = socketTimeoutM; // reset the timeout
				}
				if (err == SSL_ERROR_WANT_WRITE)
				{
					// need to wait for the write file descriptor to be able to write and then try again
					sslWaitOnWrite = true;
					timeoutRemaining = socketTimeoutM; // reset the timeout
				}
				else
				{
					// an error occured
					if ((err == SSL_ERROR_SYSCALL) && (ERR_peek_error() == 0) && (bytes == -1))
					{
						// an error in the system call occured
						openSslError("reading from secure socket");
						if (loggerM_ptr && (loggerM_ptr->getLogMask() & LOG_ERROR))
						{
							loggerM_ptr->text(LOG_NONE, 1, "    read() error code = %d", WSAGetLastError());
						}
					}
					else
					{
						openSslError("reading from secure socket");
					}
					return -1;
				}
			}
		}
		else if (sel == 0)
		{
			// no data at the end of the timeout
			if (--timeoutRemaining > 0)
			{
				// still have time left on the timeout, go back and wait some more
			}
			else
			{
				// timeout expired
				if (loggerM_ptr)
				{
					loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Connection timed-out while waiting to receive data");
				}
				return -1;
			}
		}
		else if (sel == SOCKET_ERROR)
		{
			// socket error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Error waiting to receive data (error code %d)", WSAGetLastError());
			}
			return -1;
		}
		else
		{
			// unknown error
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 2, "Secure Socket - Unknown error while waiting to receive data (select returned %d)", sel);
			}
			return -1;
		}
	}

	return read_bytes;
}


//>>===========================================================================

int TLS_SOCKET_CLASS::openSslVerifyCallback(int ok, X509_STORE_CTX* store_ptr)

//  DESCRIPTION     : Callback OpenSSL uses to request further verification of the received 
//					  certificate.  This is static function used to call the correct instantiated method.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if (store_ptr->userdata)
	{
		// convert to the object method
		return static_cast<TLS_SOCKET_CLASS*>(store_ptr->userdata)->verifyCertificate(ok, store_ptr);
	}
	else
	{
		// no logger pointer to report an error
		return 0;
	}
}

//>>===========================================================================

int TLS_SOCKET_CLASS::verifyCertificate(int ok, X509_STORE_CTX* store_ptr)

//  DESCRIPTION     : Instantiated method used by the OpenSSL callback to request further 
//					  verification of the received certificate.  We do no further verification 
//					  of the certificate, but use this to obtain detailed failure information.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if (loggerM_ptr)
	{
		if (!ok)
		{
			UINT32 logLevel;

			// figure out what log level to use
			if (checkRemoteCertificateM)
			{
				// checking remote certificates, log this as an error
				logLevel = LOG_ERROR;
			}
			else
			{
				// not checking remote certificates, only log debug
				logLevel = LOG_DEBUG;
			}

			// error, log info
			if (loggerM_ptr->getLogMask() & logLevel)
			{
				char buffer[512];

				X509* cert_ptr = X509_STORE_CTX_get_current_cert(store_ptr);
				int depth = X509_STORE_CTX_get_error_depth(store_ptr);
				int err = X509_STORE_CTX_get_error(store_ptr);

				loggerM_ptr->text(logLevel, 1, "Secure Socket - Error with certificate at depth %i", depth);

				X509_NAME_oneline(X509_get_issuer_name(cert_ptr), buffer, 512);
				loggerM_ptr->text(LOG_NONE, 1, "    issuer  = \"%s\"", buffer);

				X509_NAME_oneline(X509_get_subject_name(cert_ptr), buffer, 512);
				loggerM_ptr->text(LOG_NONE, 1, "    subject = \"%s\"", buffer);

				loggerM_ptr->text(LOG_NONE, 1, "    openssl error %i:%s", err, X509_verify_cert_error_string(err));
			}
		}
		else if (loggerM_ptr->getLogMask() & LOG_DEBUG)
		{
			// debug (only do the work if actually logging debug)
			char buffer[512];

			X509* cert_ptr = X509_STORE_CTX_get_current_cert(store_ptr);

			loggerM_ptr->text(LOG_DEBUG, 1, "Secure Socket - The following certificate verified OK:");

			X509_NAME_oneline(X509_get_issuer_name(cert_ptr), buffer, 512);
			loggerM_ptr->text(LOG_NONE, 1, "    issuer  = \"%s\"", buffer);

			X509_NAME_oneline(X509_get_subject_name(cert_ptr), buffer, 512);
			loggerM_ptr->text(LOG_NONE, 1, "    subject = \"%s\"", buffer);
		}
	}

	return ok;
}


//>>===========================================================================

void TLS_SOCKET_CLASS::openSslMsgCallback(int write_p, int version, int content_type, 
										  const void *buf, size_t len, SSL *ssl_ptr, void *this_ptr)

//  DESCRIPTION     : Callback from OpenSSL each time a SSL message is sent or received.
//					: This is static function used to call the correct instantiated method.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if (this_ptr != NULL)
	{
		// convert to the object method
		static_cast<TLS_SOCKET_CLASS*>(this_ptr)->messageCallback(write_p, version, 
															content_type, buf, len, ssl_ptr);
	}
	else
	{
		// no this pointer given
		return;
	}
}


//>>===========================================================================

void TLS_SOCKET_CLASS::messageCallback(int write_p, int version, int content_type, const void *buf, 
									   size_t len, SSL*)

//  DESCRIPTION     : Instantiated method used by the OpenSSL callback to print debug 
//					: information about each of the SSL messages
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This code comes from the OpenSSL apps\s_cb.c function msg_cb()
//<<===========================================================================
{
	const char *str_write_p, *str_version, *str_content_type = "", *str_details1 = "", *str_details2= "";

	if ((loggerM_ptr == NULL) || ((loggerM_ptr->getLogMask() & LOG_DEBUG) == 0))
	{
		// not logging debug
		return;
	}
	
	if (write_p)
	{
		str_write_p = "Secure Socket - sent handshake message";
	}
	else
	{
		str_write_p = "Secure Socket - received handshake message";
	}

	switch (version)
	{
	case SSL2_VERSION:
		str_version = "SSL 2.0";
		break;
	case SSL3_VERSION:
		str_version = "SSL 3.0 ";
		break;
	case TLS1_VERSION:
		str_version = "TLS 1.0 ";
		break;
	default:
		str_version = "???";
	}

	if (version == SSL2_VERSION)
	{
		str_details1 = "???";

		if (len > 0)
		{
			switch (((unsigned char*)buf)[0])
			{
				case 0:
					str_details1 = ", ERROR:";
					str_details2 = " ???";
					if (len >= 3)
					{
						unsigned err = (((unsigned char*)buf)[1]<<8) + ((unsigned char*)buf)[2];
						
						switch (err)
						{
						case 0x0001:
							str_details2 = " NO-CIPHER-ERROR";
							break;
						case 0x0002:
							str_details2 = " NO-CERTIFICATE-ERROR";
							break;
						case 0x0004:
							str_details2 = " BAD-CERTIFICATE-ERROR";
							break;
						case 0x0006:
							str_details2 = " UNSUPPORTED-CERTIFICATE-TYPE-ERROR";
							break;
						}
					}

					break;
				case 1:
					str_details1 = ", CLIENT-HELLO";
					break;
				case 2:
					str_details1 = ", CLIENT-MASTER-KEY";
					break;
				case 3:
					str_details1 = ", CLIENT-FINISHED";
					break;
				case 4:
					str_details1 = ", SERVER-HELLO";
					break;
				case 5:
					str_details1 = ", SERVER-VERIFY";
					break;
				case 6:
					str_details1 = ", SERVER-FINISHED";
					break;
				case 7:
					str_details1 = ", REQUEST-CERTIFICATE";
					break;
				case 8:
					str_details1 = ", CLIENT-CERTIFICATE";
					break;
			}
		}
	}

	if (version == SSL3_VERSION || version == TLS1_VERSION)
	{
		switch (content_type)
		{
		case 20:
			str_content_type = "ChangeCipherSpec";
			break;
		case 21:
			str_content_type = "Alert";
			break;
		case 22:
			str_content_type = "Handshake";
			break;
		}

		if (content_type == 21) /* Alert */
		{
			str_details1 = ", ???";
			
			if (len == 2)
			{
				switch (((unsigned char*)buf)[0])
				{
				case 1:
					str_details1 = ", warning";
					break;
				case 2:
					str_details1 = ", fatal";
					break;
				}

				str_details2 = " ???";
				switch (((unsigned char*)buf)[1])
				{
				case 0:
					str_details2 = " close_notify";
					break;
				case 10:
					str_details2 = " unexpected_message";
					break;
				case 20:
					str_details2 = " bad_record_mac";
					break;
				case 21:
					str_details2 = " decryption_failed";
					break;
				case 22:
					str_details2 = " record_overflow";
					break;
				case 30:
					str_details2 = " decompression_failure";
					break;
				case 40:
					str_details2 = " handshake_failure";
					break;
				case 42:
					str_details2 = " bad_certificate";
					break;
				case 43:
					str_details2 = " unsupported_certificate";
					break;
				case 44:
					str_details2 = " certificate_revoked";
					break;
				case 45:
					str_details2 = " certificate_expired";
					break;
				case 46:
					str_details2 = " certificate_unknown";
					break;
				case 47:
					str_details2 = " illegal_parameter";
					break;
				case 48:
					str_details2 = " unknown_ca";
					break;
				case 49:
					str_details2 = " access_denied";
					break;
				case 50:
					str_details2 = " decode_error";
					break;
				case 51:
					str_details2 = " decrypt_error";
					break;
				case 60:
					str_details2 = " export_restriction";
					break;
				case 70:
					str_details2 = " protocol_version";
					break;
				case 71:
					str_details2 = " insufficient_security";
					break;
				case 80:
					str_details2 = " internal_error";
					break;
				case 90:
					str_details2 = " user_canceled";
					break;
				case 100:
					str_details2 = " no_renegotiation";
					break;
				}
			}
		}
		
		if (content_type == 22) /* Handshake */
		{
			str_details1 = "???";

			if (len > 0)
			{
				switch (((unsigned char*)buf)[0])
				{
				case 0:
					str_details1 = ", HelloRequest";
					break;
				case 1:
					str_details1 = ", ClientHello";
					break;
				case 2:
					str_details1 = ", ServerHello";
					break;
				case 11:
					str_details1 = ", Certificate";
					break;
				case 12:
					str_details1 = ", ServerKeyExchange";
					break;
				case 13:
					str_details1 = ", CertificateRequest";
					break;
				case 14:
					str_details1 = ", ServerHelloDone";
					break;
				case 15:
					str_details1 = ", CertificateVerify";
					break;
				case 16:
					str_details1 = ", ClientKeyExchange";
					break;
				case 20:
					str_details1 = ", Finished";
					break;
				}
			}
		}
	}

	loggerM_ptr->text(LOG_DEBUG, 1, "%s %s%s [length %04lx]%s%s", str_write_p, str_version, 
		str_content_type, (unsigned long)len, str_details1, str_details2);

	// check that the RAW dump is enabled
	if (loggerM_ptr->getLogMask() & LOG_PDU_BYTES)
	{
        // serialize the PDU header
        BASE_SERIALIZER *serializer_ptr = loggerM_ptr->getSerializer();
        if (serializer_ptr)
        {
           serializer_ptr->SerializeBytes((BYTE*)buf, len, "SSL Handshake Message");
        }
	}
}


//>>===========================================================================

long TLS_SOCKET_CLASS::postConnectionCheck(SSL* ssl_ptr)

//  DESCRIPTION     : Performs checks after the connection is made to insure all is well.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	long result;

	if (checkRemoteCertificateM)
	{
		X509* cert_ptr;

		// No error will be generated by the OpenSSL library during the connection if the client 
		// does not send a certificate, even if one was requested.  We check here to make sure the 
		// certificate was sent if we required it.  If it was sent, OpenSSL will have verified it, 
		// so we don't need to do that here.  

		// Another thing that can be checked is to make sure that the certificate is for the node 
		// we think we are talking to, since the library will validate any certificate that is 
		// signed by a trusted Certificate Authority.  DVT does not perform this check since a 
		// failure here is not a connectivity problem but just a matter of getting the correct 
		// certificate installed on the unit under test.

		// To fully meet the IHE requirements, the certificate should be checked to make sure the 
		// sender is on a list of authorized nodes.  This is where that check would be performed.  
		// DVT does not support this functionality.

		// get the peer's certificate
		cert_ptr = SSL_get_peer_certificate(ssl_ptr);
		if (cert_ptr == NULL)
		{
			if (loggerM_ptr)
			{
				loggerM_ptr->text(LOG_ERROR, 1, "Secure Socket - Error peer certificate was not received");
			}

			return X509_V_ERR_APPLICATION_VERIFICATION;
		}
 
		X509_free(cert_ptr);

		// get the result of the OpenSSL verification
		result = SSL_get_verify_result(ssl_ptr);
		if (result != X509_V_OK)
		{
			openSslError("in post connection check");
		}

		return result;
	}
	else
	{
		// not checking remote certificates, nothing to do here
		return X509_V_OK;
	}
}


//>>===========================================================================

void TLS_SOCKET_CLASS::openSslError(UINT32 logLevel, const char *format_ptr, ...)

//  DESCRIPTION     : Generate an error message for the current Open SSL error.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	va_list	arguments;
	char buffer[1024];

	// handle the variable arguments
	va_start(arguments, format_ptr);
	vsprintf(buffer, format_ptr, arguments);
	va_end(arguments);
	
	openSslM_ptr->printError(loggerM_ptr, logLevel, "%s", buffer);
}

//>>===========================================================================

void TLS_SOCKET_CLASS::openSslError(const char *format_ptr, ...)

//  DESCRIPTION     : Generate an error message for the current Open SSL error.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	va_list	arguments;
	char buffer[1024];

	// handle the variable arguments
	va_start(arguments, format_ptr);
	vsprintf(buffer, format_ptr, arguments);
	va_end(arguments);
	
	openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "%s", buffer);
}

