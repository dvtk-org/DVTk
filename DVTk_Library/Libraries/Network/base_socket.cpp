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

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "base_socket.h"
#include "socket.h"
#include "tls.h"
#include "openssl.h"
		

//>>===========================================================================

bool isCipherListValid(const char* cipherList)

//  DESCRIPTION     : Checks to see if the given cipher list selects at least one cipher.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : This is a global static function declared in base_socket.hpp
//<<===========================================================================
{
	OPENSSL_CLASS* openssl_ptr;

	openssl_ptr = OPENSSL_CLASS::getInstance();
	if (openssl_ptr == NULL)
	{
		// the encryption library is not present, return true so the user interface doesn't get stuck
		return true;
	}

	return openssl_ptr->isCipherListValid(cipherList);
}


//>>===========================================================================

SOCKET_PARAMETERS::SOCKET_PARAMETERS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	remoteHostnameM = HOSTNAME;
	remoteConnectPortM = DICOM_PORT;
	localListenPortM = DICOM_PORT;
	socketTimeoutM = SOCKET_TIMEOUT; // the socket timeout in seconds
	useSecureSocketsM = false; // indicates that secure sockets should be used
	certificateFilePasswordM = DEFAULT_CERTIFICATE_FILE_PASSWORD; // this is not stored in the session file.  When the file is opened this value is asked from the user if needed. 
	minTlsVersionM = TLS_VERSION_TLSv1;
	maxTlsVersionM = TLS_VERSION_TLSv1;
	checkRemoteCertificateM = true;
	cipherListM = DEFAULT_CIPHER_LIST;
	cacheTlsSessionsM = true;
	tlsCacheTimeoutM = DEFAULT_TLS_CACHE_TIMEOUT; // sets the session cache timeout time used by the TLS server.  Currently, no way to change this. 
	credentialsFilenameM = DEFAULT_CREDENTIALS_FILENAME;
	certificateFilenameM = DEFAULT_CERTIFICATE_FILENAME;
}


//>>===========================================================================

bool SOCKET_PARAMETERS::isTlsPasswordValid(bool& unencryptedKeyFound)

//  DESCRIPTION     : Determines if the TLS password is valid.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : On any error condition, returns true
//<<===========================================================================
{
	OPENSSL_CLASS* openssl_ptr;

	if (!useSecureSocketsM)
	{
		return true;
	}

	openssl_ptr = OPENSSL_CLASS::getInstance();
	if (openssl_ptr == NULL)
	{
		// the encryption library is not present
		return true;
	}

	return openssl_ptr->isPasswordValid(credentialsFilenameM.c_str(), 
										certificateFilePasswordM.c_str(), unencryptedKeyFound);
}



//>>===========================================================================

BASE_SOCKET_CLASS::BASE_SOCKET_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	setEndian(BIG_ENDIAN);

	remoteHostnameM = HOSTNAME;
	remoteConnectPortM = DICOM_PORT;
	localListenPortM = DICOM_PORT;
	socketTimeoutM = SOCKET_TIMEOUT;
	loggerM_ptr = NULL;
	ownerThreadIdM = getThreadId();
}

//>>===========================================================================

BASE_SOCKET_CLASS::BASE_SOCKET_CLASS(const SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Constructor that sets up the values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	setEndian(BIG_ENDIAN);

	remoteHostnameM = socketParams.remoteHostnameM;
	remoteConnectPortM = socketParams.remoteConnectPortM;
	localListenPortM = socketParams.localListenPortM;
	socketTimeoutM = socketParams.socketTimeoutM;
	loggerM_ptr = logger_ptr;
	ownerThreadIdM = getThreadId();
}

//>>===========================================================================

BASE_SOCKET_CLASS::BASE_SOCKET_CLASS(const BASE_SOCKET_CLASS& socket)

//  DESCRIPTION     : Copy Constructor
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// constructor activities
	setEndian(const_cast<BASE_SOCKET_CLASS&>(socket).getEndian());

	remoteHostnameM = socket.remoteHostnameM;
	remoteConnectPortM = socket.remoteConnectPortM;
	localListenPortM = socket.localListenPortM;
	socketTimeoutM = socket.socketTimeoutM;
	loggerM_ptr = socket.loggerM_ptr;
	ownerThreadIdM = socket.ownerThreadIdM;
}
		
//>>===========================================================================

BASE_SOCKET_CLASS::~BASE_SOCKET_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// destructor activities
}


//>>===========================================================================

BASE_SOCKET_CLASS* BASE_SOCKET_CLASS::createSocket(SOCKET_PARAMETERS& socketParams, LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Static method to create the appropriate socket class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The socket is dyanmically allocated and must be freed by the caller
//<<===========================================================================
{

	if (socketParams.useSecureSocketsM)
	{
		// create a TLS (secure) socket
		if (TLS_SOCKET_CLASS::isEncryptionLibPresent())
		{
			socketParams.contentsChangedM = false; // reset

			return new TLS_SOCKET_CLASS(socketParams, logger_ptr);
		}
		else
		{
			if (logger_ptr)
			{
				logger_ptr->text(LOG_ERROR, 2, "Encryption library is not present, cannot use secure sockets");
			}

			return NULL;
		}
	}
	else
	{
		// create a standard socket
		socketParams.contentsChangedM = false; // reset

		return new SOCKET_SOCKET_CLASS(socketParams, logger_ptr);
	}
}

//>>===========================================================================

bool BASE_SOCKET_CLASS::socketParametersChanged(const SOCKET_PARAMETERS& socketParams)

//  DESCRIPTION     : Determines if the given socket parameters are different than the parameters given in the call.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if (socketParams.contentsChangedM)
	{
		// one of the files contents changed
		return true;
	}

	if ((remoteHostnameM == socketParams.remoteHostnameM) &&
		(remoteConnectPortM == socketParams.remoteConnectPortM) &&
		(localListenPortM == socketParams.localListenPortM) &&
		(socketTimeoutM == socketParams.socketTimeoutM))
	{
		return false;
	}
	else
	{
		return true;
	}
}

//>>===========================================================================

void BASE_SOCKET_CLASS::setLogger(LOG_CLASS* logger_ptr)

//  DESCRIPTION     : Save the Logger address.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	// save the new logger
	loggerM_ptr = logger_ptr;
}


