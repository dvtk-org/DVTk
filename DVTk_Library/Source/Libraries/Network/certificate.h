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
//  DESCRIPTION     :	Secure Certificate Management Class.
//*****************************************************************************
#ifndef CERTIFICATE_H
#define CERTIFICATE_H

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "Iglobal.h"		// Global component interface
#include "Ilog.h"			// Log component interface
#include "Iutility.h"		// Utility component interface
#include "openssl.h"		// OpenSSL library interface

//*****************************************************************************
//  FORWARD DECLARATION
//*****************************************************************************
class BASE_SESSION_CLASS;


//*****************************************************************************
//  CONSTANTS AND TYPE DEFINITIONS
//*****************************************************************************



//>>***************************************************************************

class CERTIFICATE_CLASS

//  DESCRIPTION     : Class used to contain certificate data.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	long versionM;
	string subjectM;
	string issuerM;
	struct tm effectiveDateM;
	struct tm expirationDateM;
	string signatureAlgorithmM;
	int signatureKeyLengthM;
	string serialNumberM;

public:
	CERTIFICATE_CLASS();

	CERTIFICATE_CLASS(X509* x509_ptr);

	CERTIFICATE_CLASS(EVP_PKEY* key_ptr);

	~CERTIFICATE_CLASS();

	DVT_STATUS generateFiles(LOG_CLASS* logger_ptr, const char* signerCredentialsFile_ptr, 
		const char* credentialsPassword_ptr, const char* keyPassword_ptr, 
		const char* keyFile_ptr, const char* certificateFile_ptr);

	long getVersion() { return versionM; }

	const char* getSubject() { return subjectM.c_str(); }
	
	const char* getIssuer() { return issuerM.c_str(); }
	
	struct tm* getEffectiveDate() { return &effectiveDateM; }
	
	struct tm* getExpirationDate() { return &expirationDateM; }
	
	const char* getSignatureAlgorithm() {return signatureAlgorithmM.c_str(); }
	
	int getSignatureKeyLength() { return signatureKeyLengthM; }
	
	const char* getSerialNumber() { return serialNumberM.c_str(); }



	void setVersion(long version) { versionM = version; }
	
	void setSubject(const char* subject) { subjectM = subject; }
	
	void setIssuer(const char* issuer) { issuerM = issuer; }
	
	void setEffectiveDate(struct tm& effectiveDate) { effectiveDateM = effectiveDate; }
	
	void setExpirationDate(struct tm& expirationDate) { expirationDateM = expirationDate; }
	
	void setSignatureAlgorithm(const char* signatureAlgorithm) {signatureAlgorithmM = signatureAlgorithm; }
	
	void setSignatureKeyLength(int signatureKeyLength) { signatureKeyLengthM = signatureKeyLength; }
	
	void setSerialNumber(const char* serialNumber) { serialNumberM = serialNumber; }
};


//>>***************************************************************************

class CERTIFICATE_FILE_CLASS

//  DESCRIPTION     : Class used to read and manipulate certificate files.
//  INVARIANT       :
//  NOTES           :
//<<***************************************************************************
{
private:
	string				filenameM;			// name of the certificate file
	STACK_OF(X509_INFO)* x509InfoListM_ptr;	// list of the certificates in the file
	OPENSSL_CLASS*		openSslM_ptr;		// point to the OpenSSL class
	LOG_CLASS*			loggerM_ptr;		// pointer to the logger
	string				passwordM;			// TLS password to use
	bool				changedM;			// indicates that the contents have changed


	bool push(X509*&);

	bool push(EVP_PKEY*&);

	DVT_STATUS importPem(const char* filename, bool certificatesOnly, const char* password);

	static char* derCallback(CERTIFICATE_FILE_CLASS* certFileClass_ptr, const unsigned char **buf_ptrptr, long length);

	static char* derDecode(const unsigned char **buf_ptrptr, long length);

	DVT_STATUS importDer(const char* filename, bool certificatesOnly, const char* password);

	DVT_STATUS importPkcs12(const char* filename, bool certificatesOnly, const char* password);

	DVT_STATUS importPkcs7(const char* filename, bool certificatesOnly, const char* password);

public:
	CERTIFICATE_FILE_CLASS(const char* filename, const char* password_ptr, LOG_CLASS* logger_ptr, 
							DVT_STATUS& dvtStatus);

	~CERTIFICATE_FILE_CLASS();

	DVT_STATUS importCertificateFile(const char* filename, bool certificatesOnly, const char* password);

	bool moveCertificate(int oldIndex, int newIndex);

	bool removeCertificate(int index);

	int getNumberOfCertificates();

	CERTIFICATE_CLASS* getCertificate(int index);

	const char* getPassword() { return passwordM.c_str(); }

	bool hasChanged() { return changedM; }

	bool writeFile(const char* password);

	bool verify(bool isCredentials, char** msg_ptrptr);

};

#endif /* CERTIFICATE_H */


