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
//  DESCRIPTION     :	Interface class to the OpenSSL library.
//*****************************************************************************
#ifndef OPENSSL_H
#define OPENSSL_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"       // Utility component interface
#include "openssl/bio.h"	// OpenSSL BIO interface
#include "openssl/err.h"	// OpenSSL errors interface
#include "openssl/rand.h"	// OpenSSL random number interface
#include "openssl/ssl.h"	// OpenSSL main interface
#include "openssl/x509v3.h"	// OpenSSL certificates interface
#include "openssl/pkcs12.h"	// OpenSSL PKCS#12 interface
#include "openssl/ssl.h"	// 
#include <openssl/evp.h>
#include <openssl/dh.h>
#include <openssl/crypto.h>
//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************
#ifdef _WINDOWS
// Windows threads defines
#define MUTEX_TYPE HANDLE

#else
// Posix threads defines
#include <unistd.h>
#include <pthread.h>

#define MUTEX_TYPE pthread_mutex_t

#endif

// OpenSSL required structure definition for dynamic locks
struct	CRYPTO_dynlock_value
{
	MUTEX_TYPE	mutex; 
};


//>>***************************************************************************

class OPENSSL_CLASS

//  DESCRIPTION     : Class to provide an interface to the OpenSSL library.
//  INVARIANT       :
//  NOTES           : This class loads the OpenSSL DLLs (libeay32.dll and ssleay32.dll) and 
//					  sets up function pointers to all of the used functions. It also
//					  initializes the library and provides additional interface functions.
//
//					  This class is a singleton
//<<***************************************************************************
{
private:
	static OPENSSL_CLASS* instanceM_ptr;

	HINSTANCE ssleayHandleM;
	HINSTANCE libeayHandleM;

	bool libraryInitializedM;
	static MUTEX_TYPE* mutexMapM_ptr;	// new'ed array of mutext handles used to map between the 
										// lock number given by OpenSSL and the actual handles 
										// provided by the OS. 

	OPENSSL_CLASS(bool& loaded);

	bool libraryInitialize();

	void seedPrng();

	bool checkPemName(const char * readName_ptr, const char* name_ptr);

	static void threadLockingCallback(int mode, int n, const char* file, int line);

	static unsigned long threadIdCallback(); 

	static struct CRYPTO_dynlock_value* dynamicLockCreateCallback(const char *file, int line);

	static void dynamicLockingCallback(int mode, CRYPTO_dynlock_value* lock_ptr, const char* file, int line);

	static void dynamicLockDestroyCallback(CRYPTO_dynlock_value* lock_ptr, const char* file, int line);

	bool loadSsleayLib();

	bool loadLibeayLib();


public:
	~OPENSSL_CLASS();

	static OPENSSL_CLASS* getInstance();

	void printError(LOG_CLASS* logger_ptr, UINT32 logLevel, const char *format_ptr, ...);

	bool isCipherListValid(const char* cipherList);

	bool isPasswordValid(const char* filename, const char* password, bool& unencryptedKeyFound);

	static bool asn1TimeToTm(ASN1_TIME* asn1Time_ptr, struct tm& tmTime);

	static bool serialNumberToString(ASN1_INTEGER* serialNumber_ptr, string& serialNumberString);

	X509_NAME* onelineName2Name(const char *string_ptr);

	DVT_STATUS readPemFile(const char* filename_ptr, STACK_OF(X509_INFO)** pem_ptrptr,
		pem_password_cb *passwordCallback_ptr, void *passswordCbUserData_ptr, LOG_CLASS* logger_ptr, 
		bool* unencryptedKeyFound_ptr = NULL);

	bool writePemFile(const char* filename_ptr,  STACK_OF(X509_INFO)* pem_ptr,
		const char* password_ptr, LOG_CLASS* logger_ptr);

	DVT_STATUS readCredentialsFile(const char* filename_ptr, 
		EVP_PKEY** rsaPrivateKey_ptrptr, STACK_OF(X509)** rsaCertChain_ptrptr,
		EVP_PKEY** dsaPrivateKey_ptrptr, STACK_OF(X509)** dsaCertChain_ptrptr,
		pem_password_cb *passwordCallback_ptr, void *passswordCbUserData_ptr, LOG_CLASS* logger_ptr);

	static DH* tmpDhCallback(SSL *ssl, int is_export, int keylength);

	static int openSslPasswordCallback(char* password, int buffersize, int encryptionFlag, void* /*const char* */ pwd);

};

#endif /* OPENSSL_H */


