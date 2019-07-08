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

//*****************************************************************************
//  EXTERNAL DECLARATIONS
//*****************************************************************************
#include "certificate.h"
#include "Isession.h"


//>>===========================================================================

CERTIFICATE_CLASS::CERTIFICATE_CLASS()

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	versionM = 0;
	memset(&effectiveDateM, 0, sizeof(struct tm));
	memset(&expirationDateM, 0, sizeof(struct tm));
	signatureKeyLengthM = 0;

}
		
//>>===========================================================================

CERTIFICATE_CLASS::CERTIFICATE_CLASS(X509* x509_ptr)

//  DESCRIPTION     : Constructor that fills in the certificate values.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	char* name_ptr;
	EVP_PKEY* publicKey_ptr;
		
	versionM = X509_get_version(x509_ptr) + 1/* change to 1 based*/;

	name_ptr = X509_NAME_oneline(X509_get_subject_name(x509_ptr), NULL, 0);
	if (name_ptr != NULL)
	{
		subjectM = name_ptr;
		OPENSSL_free(name_ptr);
	}
	else
	{
		subjectM = "";
	}

	name_ptr = X509_NAME_oneline(X509_get_issuer_name(x509_ptr), NULL, 0);
	if (name_ptr != NULL)
	{
		issuerM = name_ptr;;
		OPENSSL_free(name_ptr);
	}
	else
	{
		issuerM = "";
	}

	OPENSSL_CLASS::asn1TimeToTm(X509_get_notBefore(x509_ptr), effectiveDateM);

	OPENSSL_CLASS::asn1TimeToTm(X509_get_notAfter(x509_ptr), expirationDateM);
	
	signatureKeyLengthM = 0;
	signatureAlgorithmM = "";
	publicKey_ptr = X509_get_pubkey(x509_ptr);
	if (publicKey_ptr != NULL)
	{
		if (publicKey_ptr->type == EVP_PKEY_RSA)
		{
			signatureAlgorithmM = "RSA";
			if ((publicKey_ptr->pkey.rsa != NULL) && (publicKey_ptr->pkey.rsa->n != NULL))
			{
				signatureKeyLengthM = BN_num_bits(publicKey_ptr->pkey.rsa->n);
			}
		}
		else if (publicKey_ptr->type == EVP_PKEY_DSA)
		{
			signatureAlgorithmM = "DSA";
			if ((publicKey_ptr->pkey.dsa != NULL) && (publicKey_ptr->pkey.dsa->p != NULL))
			{
				signatureKeyLengthM = BN_num_bits(publicKey_ptr->pkey.dsa->p);
			}
		}

		EVP_PKEY_free(publicKey_ptr);
	}

	OPENSSL_CLASS::serialNumberToString(X509_get_serialNumber(x509_ptr), serialNumberM);
}
		
//>>===========================================================================

CERTIFICATE_CLASS::CERTIFICATE_CLASS(EVP_PKEY* key_ptr)

//  DESCRIPTION     : Constructor that fills in the certificate values from a private key.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	versionM = 0;

	subjectM = "Private Key";

	issuerM = "";

	memset(&effectiveDateM, 0, sizeof(effectiveDateM));

	memset(&expirationDateM, 0, sizeof(expirationDateM));

	signatureKeyLengthM = 0;
	signatureAlgorithmM = "";
	if (key_ptr->type == EVP_PKEY_RSA)
	{
		signatureAlgorithmM = "RSA";
		subjectM = "RSA Private Key";
		if ((key_ptr->pkey.rsa != NULL) && (key_ptr->pkey.rsa->n != NULL))
		{
			signatureKeyLengthM = BN_num_bits(key_ptr->pkey.rsa->n);
		}
	}
	else if (key_ptr->type == EVP_PKEY_DSA)
	{
		signatureAlgorithmM = "DSA";
		subjectM = "DSA Private Key";
		if ((key_ptr->pkey.dsa != NULL) && (key_ptr->pkey.dsa->p != NULL))
		{
			signatureKeyLengthM = BN_num_bits(key_ptr->pkey.dsa->p);
		}
	}

	serialNumberM = "";
}
		
//>>===========================================================================

CERTIFICATE_CLASS::~CERTIFICATE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
}
		
//>>===========================================================================

DVT_STATUS CERTIFICATE_CLASS::generateFiles(LOG_CLASS* logger_ptr,
											const char* signerCredentialsFile_ptr, 
											const char* credentialsPassword_ptr,
											const char* keyPassword_ptr,
											const char* keyFile_ptr, 
											const char* certificateFile_ptr)

//  DESCRIPTION     : Generate a certificate and key files from this class.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : If signerCredentialsFile_ptr is NULL, a self signed 
//					: certificate will be generated.
//					:
//					: Returns:  MSG_OK, MSG_LIB_NOT_EXIST, MSG_FILE_NOT_EXIST, 
//					: MSG_ERROR, MSG_INVALID_PASSWORD 
//<<===========================================================================
{
	DVT_STATUS ret = MSG_ERROR;
	unsigned long err;
	OPENSSL_CLASS* openSsl_ptr;
	BIO* caBio_ptr = NULL;
	EVP_PKEY* caPrivateKey_ptr = NULL;
	X509* caCertificate_ptr = NULL;
	EVP_PKEY* key_ptr = NULL;
	X509* cert_ptr = NULL;
	X509_NAME* name_ptr;
	time_t effectiveTime;
	time_t expirationTime;
	EVP_PKEY* tmpKey_ptr;
	const EVP_MD *digest_ptr;
	BIO* pkBio_ptr = NULL;
	const EVP_CIPHER *cipher_ptr;
	BIO* certBio_ptr = NULL;

	// check for the existence of the OpenSSL DLLs
	openSsl_ptr = OPENSSL_CLASS::getInstance();
	if (openSsl_ptr == NULL)
	{
		return MSG_LIB_NOT_EXIST;
	}

	// clear the error queue
	ERR_clear_error();

	if (signerCredentialsFile_ptr != NULL)
	{
		// open the credentials file
		caBio_ptr = BIO_new(BIO_s_file_internal());
		if (caBio_ptr == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting up to read CA credentials file");
			goto end;
		}
		if (BIO_read_filename(caBio_ptr, signerCredentialsFile_ptr) <= 0)
		{
			err = ERR_peek_error();
			if ((ERR_GET_LIB(err) == ERR_LIB_SYS) && (ERR_GET_REASON(err) == ERROR_FILE_NOT_FOUND))
			{
				// file does not exist
				ERR_clear_error(); // eat any errors
				ret = MSG_FILE_NOT_EXIST;
			}
			else
			{
				openSsl_ptr->printError(logger_ptr, LOG_ERROR, "opening CA credentials file for reading");
			}
			goto end;
		}

		// read the certificate authority's private key
		caPrivateKey_ptr = PEM_read_bio_PrivateKey(caBio_ptr, NULL, NULL, (void*)credentialsPassword_ptr);
		if (caPrivateKey_ptr == NULL)
		{
			err = ERR_peek_error();
			if ((ERR_GET_LIB(err) == ERR_LIB_EVP) && (ERR_GET_REASON(err) == EVP_R_BAD_DECRYPT))
			{
				// bad password
				ERR_clear_error(); // eat any errors
				ret = MSG_INVALID_PASSWORD;
			}
			else
			{
				openSsl_ptr->printError(logger_ptr, LOG_ERROR, "reading private key from CA credentials file");
			}
			goto end;
		}

		// read the certificate authority's certificate
		caCertificate_ptr = PEM_read_bio_X509(caBio_ptr, NULL, NULL, (void*)credentialsPassword_ptr);
		if (caCertificate_ptr == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "reading CA certificate from CA credentials file");
			goto end;
		}
	}

	// generate the new private/public key pair
	if (signatureAlgorithmM.compare("RSA") == 0)
	{
		// RSA key
		RSA* rsa_key;

		rsa_key = RSA_generate_key(signatureKeyLengthM, RSA_3, NULL, 0);
		if (rsa_key == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "generating RSA key");
			goto end;
		}

		key_ptr = EVP_PKEY_new();
		if (key_ptr == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "creating RSA key");
			RSA_free(rsa_key);
			goto end;
		}

		EVP_PKEY_assign_RSA(key_ptr, rsa_key);
	}
	else
	{
		// DSA key
		DSA* dsa_key;

		dsa_key = DSA_generate_parameters(signatureKeyLengthM, NULL, 0, NULL, NULL, NULL, 0);
		if (dsa_key == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "generating DSA parameters");
			goto end;
		}

		if (DSA_generate_key(dsa_key) == 0)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "generating DSA key");
			DSA_free(dsa_key);
			goto end;
		}

		key_ptr = EVP_PKEY_new();
		if (key_ptr == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "creating DSA key");
			DSA_free(dsa_key);
			goto end;
		}

		EVP_PKEY_assign_DSA(key_ptr, dsa_key);
	}

	// create the certificate
	cert_ptr = X509_new();
	if (cert_ptr == NULL)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "creating certificate object");
		goto end;
	}

	// version
	if (X509_set_version(cert_ptr, (versionM - 1)) != 1)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting certificate version");
		goto end;
	}

	// subject
	name_ptr = openSsl_ptr->onelineName2Name(subjectM.c_str());
	if (name_ptr == NULL)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "parsing owner name");
		goto end;
	}

	if (X509_set_subject_name(cert_ptr, name_ptr) != 1)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting owner name in certificate");
		goto end;
	}

	// issuer
	if (signerCredentialsFile_ptr != NULL)
	{
		// CA signed
		name_ptr = X509_get_subject_name(caCertificate_ptr);
		if (name_ptr == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "getting name from CA certificate");
			goto end;
		}

		if (X509_set_issuer_name(cert_ptr, name_ptr) != 1)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting issuer name in certificate");
			goto end;
		}
	}
	else
	{
		// self signed
		name_ptr = X509_NAME_dup(name_ptr); // duplicate the name so it can be used again
		if (name_ptr == NULL)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "duplicating owner name");
			goto end;
		}

		if (X509_set_issuer_name(cert_ptr, name_ptr) != 1)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting issuer name in certificate");
			goto end;
		}
	}
	
	// public key
	if (X509_set_pubkey(cert_ptr, key_ptr) != 1)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting public key in certificate");
		goto end;
	}

	// valid dates
	effectiveTime = mktime(&effectiveDateM);
	expirationTime = mktime(&expirationDateM);
	if ((X509_time_adj(X509_get_notBefore(cert_ptr), 0, &effectiveTime) == NULL) ||
		(X509_time_adj(X509_get_notAfter(cert_ptr), 0, &expirationTime) == NULL))
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting valid dates in certificate");
		goto end;
	}

	// serial number, use the current time_t
	ASN1_INTEGER_set(X509_get_serialNumber(cert_ptr), (unsigned)time(NULL));

	// sign the certificate
	if (signerCredentialsFile_ptr != NULL)
	{
		// CA signed
		tmpKey_ptr = caPrivateKey_ptr;
	}
	else
	{
		// self signed
		tmpKey_ptr = key_ptr;
	}

	if (EVP_PKEY_type(tmpKey_ptr->type) == EVP_PKEY_RSA)
	{
		digest_ptr = EVP_sha1();
	}
	else if (EVP_PKEY_type(tmpKey_ptr->type) == EVP_PKEY_DSA)
	{
		digest_ptr = EVP_dss1();
	}
	else
	{
		if (logger_ptr)
		{
			logger_ptr->text(LOG_ERROR, 1, "Unsupported key type in CA private key");
		}
		goto end;
	}

	if (!X509_sign(cert_ptr, tmpKey_ptr, digest_ptr))
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "signing certificate");
		goto end;
	}

	// write out the private key
	// open the private key file
	pkBio_ptr = BIO_new(BIO_s_file_internal());
	if (pkBio_ptr == NULL)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting up to write private key file");
		goto end;
	}
	if (BIO_write_filename(pkBio_ptr, (void *)keyFile_ptr) <= 0)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "opening to write private key file");
		goto end;
	}

	if ((keyPassword_ptr != NULL) && (strlen(keyPassword_ptr) > 0))
	{
		// we have a password, use 3DES to encrypt the key
		cipher_ptr = EVP_des_ede3_cbc();
	}
	else
	{
		// there is no password, don't encrypt the key
		cipher_ptr = NULL;
	}

	// write out the private key
	if (PEM_write_bio_PKCS8PrivateKey(pkBio_ptr, key_ptr, cipher_ptr, 
										NULL, 0, NULL, (void *)keyPassword_ptr) != 1)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "writing private key");
		goto end;
	}

	// write the certificate file
	// open the certificate file
	certBio_ptr = BIO_new(BIO_s_file_internal());
	if (certBio_ptr == NULL)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "setting up to write certificate file");
		goto end;
	}
	if (BIO_write_filename(certBio_ptr, (void *)certificateFile_ptr) <= 0)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "opening to write certificate file");
		goto end;
	}

	// write the new certificate
	if (PEM_write_bio_X509(certBio_ptr, cert_ptr) != 1)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "writing certificate");
		goto end;
	}

	// write the new certificate into the credential file 
	if (PEM_write_bio_X509(pkBio_ptr, cert_ptr) != 1)
	{
		openSsl_ptr->printError(logger_ptr, LOG_ERROR, "writing certificate");
		goto end;
	}


	if (signerCredentialsFile_ptr != NULL)
	{
		// write the CA certificate
		if (PEM_write_bio_X509(certBio_ptr, caCertificate_ptr) != 1)
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "writing CA certificate");
			goto end;
		}

		// loop reading certificates from the CA credentials file and writing them to the certificate file
		X509_free(caCertificate_ptr);
		while ((caCertificate_ptr = PEM_read_bio_X509(caBio_ptr, NULL, NULL, (void*)credentialsPassword_ptr)) != NULL)
		{
			// write the certificate
			if (PEM_write_bio_X509(certBio_ptr, caCertificate_ptr) != 1)
			{
				openSsl_ptr->printError(logger_ptr, LOG_ERROR, "writing certificate chain");
				goto end;
			}

			X509_free(caCertificate_ptr);
		}
		// check the error
		err = ERR_peek_error();
		if ((ERR_GET_LIB(err) == ERR_LIB_PEM) && (ERR_GET_REASON(err) == PEM_R_NO_START_LINE))
		{
			// end of data - this is normal
			ERR_clear_error();
		}
		else
		{
			openSsl_ptr->printError(logger_ptr, LOG_ERROR, "reading certificates from CA credentials file");
			goto end;
		}
	}


	ret = MSG_OK;

end:
	if (certBio_ptr != NULL) BIO_free(certBio_ptr);
	if (pkBio_ptr != NULL) BIO_free(pkBio_ptr);
	if (cert_ptr != NULL) X509_free(cert_ptr);
	if (key_ptr != NULL) EVP_PKEY_free(key_ptr);
	if (caCertificate_ptr != NULL) X509_free(caCertificate_ptr);
	if (caPrivateKey_ptr != NULL) EVP_PKEY_free(caPrivateKey_ptr);
	if (caBio_ptr != NULL) BIO_free(caBio_ptr);

	return ret;
}


//>>===========================================================================

CERTIFICATE_FILE_CLASS::CERTIFICATE_FILE_CLASS(const char* filename, const char* password_ptr, 
											   LOG_CLASS* logger_ptr, DVT_STATUS& dvtStatus)

//  DESCRIPTION     : Constructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Returns in dvtStatus:  MSG_OK, MSG_LIB_NOT_EXIST, MSG_FILE_NOT_EXIST, 
//					: MSG_ERROR, MSG_INVALID_PASSWORD
//<<===========================================================================
{
	// constructor activities
	filenameM = filename;

	loggerM_ptr = logger_ptr;
	passwordM = password_ptr;
	x509InfoListM_ptr = NULL;
	changedM = false;

	openSslM_ptr = OPENSSL_CLASS::getInstance();
	if (openSslM_ptr == NULL)
	{
		dvtStatus = MSG_LIB_NOT_EXIST;
		return;
	}

	// read the file
	dvtStatus = openSslM_ptr->readPemFile(filenameM.c_str(), &x509InfoListM_ptr, 
		OPENSSL_CLASS::openSslPasswordCallback, (void *)passwordM.c_str(), loggerM_ptr);

	if (x509InfoListM_ptr == NULL)
	{
		// make sure we always have a x509InfoListM_ptr (if we have a openSslM_ptr)
		x509InfoListM_ptr = sk_X509_INFO_new_null();
	}

}
		
//>>===========================================================================

CERTIFICATE_FILE_CLASS::~CERTIFICATE_FILE_CLASS()

//  DESCRIPTION     : Destructor.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	if (openSslM_ptr == NULL)
	{
		return;
	}

	if (x509InfoListM_ptr != NULL)
	{
		// free the certificate list
		sk_X509_INFO_pop_free(x509InfoListM_ptr, X509_INFO_free);
	}

}
		
//>>===========================================================================

bool CERTIFICATE_FILE_CLASS::push(X509*& x509_ptr)

//  DESCRIPTION     : Pushes the certificate onto the certificate stack.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : x509_ptr is always set to NULL
//<<===========================================================================
{
	X509_INFO* x509Info_ptr;

	x509Info_ptr = X509_INFO_new();
	if (x509Info_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "creating PEM info structure");
		X509_free(x509_ptr);
		x509_ptr = NULL;
		return false;
	}

	x509Info_ptr->x509 = x509_ptr;
	x509_ptr = NULL; // the x509Info_ptr now controls the x509_ptr memory

	sk_X509_INFO_push(x509InfoListM_ptr, x509Info_ptr);

	return true;
}
		
//>>===========================================================================

bool CERTIFICATE_FILE_CLASS::push(EVP_PKEY*& pkey_ptr)

//  DESCRIPTION     : Pushes the private key onto the certificate stack.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : pkey_ptr is always set to NULL
//<<===========================================================================
{
	X509_INFO* x509Info_ptr;

	x509Info_ptr = X509_INFO_new();
	if (x509Info_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "creating PEM info structure");
		EVP_PKEY_free(pkey_ptr);
		pkey_ptr = NULL;
		return false;
	}

	x509Info_ptr->enc_data = NULL;
	x509Info_ptr->enc_len = 0;
	x509Info_ptr->x_pkey = X509_PKEY_new();
	if (x509Info_ptr->x_pkey == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "creating private key structure");
		EVP_PKEY_free(pkey_ptr);
		pkey_ptr = NULL;
		return false;
	}
	x509Info_ptr->x_pkey->dec_pkey = pkey_ptr;
	pkey_ptr = NULL; // the x509Info_ptr now controls the pkey_ptr memory

	sk_X509_INFO_push(x509InfoListM_ptr, x509Info_ptr);

	return true;
}
		
//>>===========================================================================

DVT_STATUS CERTIFICATE_FILE_CLASS::importPem(const char* filename, bool certificatesOnly,
											 const char* password)

//  DESCRIPTION     : Import certificates from a PEM formated file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Returns MSG_OK, MSG_ERROR, MSG_FILE_NOT_EXIST, MSG_NO_VALUE, MSG_INVALID_PASSWORD
//<<===========================================================================
{
	DVT_STATUS status = MSG_ERROR;
	STACK_OF(X509_INFO)* infoList_ptr = NULL;
	X509_INFO* x509Info_ptr;
	int count = 0;


	// read the file
	status = openSslM_ptr->readPemFile(filename, &infoList_ptr, 
					OPENSSL_CLASS::openSslPasswordCallback, (void *)password, loggerM_ptr);
	switch (status)
	{
	case MSG_FILE_NOT_EXIST:
	case MSG_ERROR:
	case MSG_INVALID_PASSWORD:
		goto end;
	}

	// for each certificate in the import file, add the certificate to this file's list
	while ((x509Info_ptr = sk_X509_INFO_shift(infoList_ptr)) != NULL)
	{
		if ((x509Info_ptr->x509 == NULL) && certificatesOnly)
		{
			// not a certificate and only intested in certificates, just free it
			X509_INFO_free(x509Info_ptr);
		}
		else
		{
			// add to the certificate list
			sk_X509_INFO_push(x509InfoListM_ptr, x509Info_ptr);
			count++;
		}
	}

	if (count == 0)
	{
		status = MSG_NO_VALUE;
	}
	else
	{
		status = MSG_OK;
	}

end:
	if (infoList_ptr != NULL) sk_X509_INFO_pop_free(infoList_ptr, X509_INFO_free);

	return status;
}
		
//>>===========================================================================

char* CERTIFICATE_FILE_CLASS::derCallback(CERTIFICATE_FILE_CLASS* certFileClass_ptr, 
										  const unsigned char **buf_ptrptr, long length)

//  DESCRIPTION     : Callback called from ASN1_d2i_bio() to determine the type of the DER data and decode it.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Static function
//<<===========================================================================
{
	return certFileClass_ptr->derDecode(buf_ptrptr, length);
}
		
//>>===========================================================================

char* CERTIFICATE_FILE_CLASS::derDecode(const unsigned char **buf_ptrptr, long length)

//  DESCRIPTION     : Determine the type of the DER data and decode it.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Returns a pointer to a new'd DVT_STATUS, which must be deleted by the caller
//<<===========================================================================
{
	DVT_STATUS status = MSG_ERROR;
	const unsigned char *p;
	STACK_OF(ASN1_TYPE) *inkey_ptr = NULL;
	EVP_PKEY *pkey_ptr = NULL;
	X509 *cert_ptr = NULL;
	int count = 0;

	//// try to determine the contents of the file [this is adapted from d2i_AutoPrivateKey()]
	//p = *buf_ptrptr;
	//inkey_ptr = d2i_ASN1_SET_OF_ASN1_TYPE(NULL, &p, length, d2i_ASN1_TYPE, 
	//				ASN1_TYPE_free, V_ASN1_SEQUENCE, V_ASN1_UNIVERSAL);
	//if (inkey_ptr == NULL)
	//{
	//	// probably not a DER file
	//	status = MSG_NO_VALUE;
	//	goto end;
	//}
	//switch (sk_ASN1_TYPE_num(inkey_ptr))
	//{
	//case 3:
	//	// certificate file
	//	p = *buf_ptrptr;
	//	cert_ptr = (X509*)ASN1_item_d2i(NULL, &p, length, ASN1_ITEM_rptr(X509));
	//	if (cert_ptr == NULL)
	//	{
	//		openSslM_ptr->printError(loggerTest_ptr, LOG_ERROR, "decoding certificate in DER file");
	//		status = MSG_ERROR;
	//		goto end;
	//	}
	//	else
	//	{
	//		// save the certificate
	//		if (!push(cert_ptr))
	//		{
	//			status = MSG_ERROR;
	//			goto end;
	//		}
	//		count++;
	//	}
	//	break;

	//case 6:
	//	// DSA private key file
	//	p = *buf_ptrptr;
	//	pkey_ptr = d2i_PrivateKey(EVP_PKEY_DSA, NULL, &p, length);
	//	if (pkey_ptr == NULL)
	//	{
		//	openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "decoding private key in DER file");
	//		status = MSG_ERROR;
	//		goto end;
	//	}
	//	else
	//	{
	//		// save the private key
	//		if (!push(pkey_ptr))
	//		{
	//			status = MSG_ERROR;
	//			goto end;
	//		}
	//		count++;
	//	}
	//	break;

	//case 9:
	//	// RSA private key file
	//	p = *buf_ptrptr;
	//	pkey_ptr = d2i_PrivateKey(EVP_PKEY_RSA, NULL, &p, length);
	//	if (pkey_ptr == NULL)
	//	{
		//	openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "decoding private key in DER file");
//			status = MSG_ERROR;
//			goto end;
//		}
//		else
//		{
//			// save the private key
//			if (!push(pkey_ptr))
//			{
//				status = MSG_ERROR;
//				goto end;
//			}
//			count++;
//		}
//		break;
//
//	default:
//		// unknown data
//		status = MSG_NO_VALUE;
//		goto end;
//	}
//
//	if (count == 0)
//	{
//		status = MSG_NO_VALUE;
//	}
//	else
//	{
//		status = MSG_OK;
//	}
//
//end:
//	if (inkey_ptr != NULL) sk_ASN1_TYPE_pop_free(inkey_ptr, ASN1_TYPE_free);
//	if (pkey_ptr != NULL) EVP_PKEY_free(pkey_ptr);
//	if (cert_ptr != NULL) X509_free(cert_ptr);

	return (char*)(new DVT_STATUS(status));
}
		
//>>===========================================================================

DVT_STATUS CERTIFICATE_FILE_CLASS::importDer(const char* filename, bool certificatesOnly, const char*)

//  DESCRIPTION     : Import certificates from a PEM formated file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Returns MSG_OK, MSG_ERROR, MSG_FILE_NOT_EXIST, MSG_NO_VALUE
//					: DER does not support encryption, so MSG_INVALID_PASSWORD will never be returned
//<<===========================================================================
{
	DVT_STATUS status = MSG_ERROR;
	DVT_STATUS* status_ptr;
	BIO* bio_ptr;


	// clear the error queue
	ERR_clear_error();

	// open the file
	bio_ptr = BIO_new(BIO_s_file_internal());
	if (bio_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "setting up to read DER file");
		status = MSG_ERROR;
		goto end;
	}
	if (BIO_read_filename(bio_ptr, filename) <= 0)
	{
		unsigned long err;
		err = ERR_peek_error();
		if ((ERR_GET_LIB(err) == ERR_LIB_SYS) && (ERR_GET_REASON(err) == ERROR_FILE_NOT_FOUND))
		{
			// file does not exist
			ERR_clear_error(); // eat any errors
			status = MSG_FILE_NOT_EXIST;
		}
		else
		{
			openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "opening DER file for reading");
			status = MSG_ERROR;
		}
		goto end;
	}

	// read the file and convert the data
	if (certificatesOnly)
	{
		X509* cert_ptr;

		cert_ptr = (X509*)ASN1_item_d2i_bio(ASN1_ITEM_rptr(X509), bio_ptr, NULL);
		if (cert_ptr == NULL)
		{
			unsigned long err;
			err = ERR_peek_error();
			if ((ERR_GET_LIB(err) == ERR_LIB_ASN1) && (ERR_GET_REASON(err) == ASN1_R_WRONG_TAG))
			{
				// probably not a certificate
				ERR_clear_error(); // eat any errors
				status = MSG_NO_VALUE;
			}
			else
			{
				openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "decoding certificate in DER file");
				status = MSG_ERROR;
			}
		}
		else
		{
			// save the certificate
			if (!push(cert_ptr))
			{
				status = MSG_ERROR;
			}
			else
			{
				status = MSG_OK;
			}
		}
	}
	else
	{
		// todo
		// this calls derDecode()
		//status_ptr = (DVT_STATUS)ASN1_d2i_bio_of(char, NULL, NULL, bio_ptr, NULL);
		//status_ptr = ASN1_d2i_bio_of(char *, NULL, derCallback, bio_ptr, this);
		//status = *status_ptr;
		delete status_ptr;
	}

end:
	if (bio_ptr != NULL) BIO_free(bio_ptr);

	return status;
}
		
//>>===========================================================================

DVT_STATUS CERTIFICATE_FILE_CLASS::importPkcs12(const char* filename, bool certificatesOnly,
												 const char* password)

//  DESCRIPTION     : Import certificates from a PKCS#12 formated file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Returns MSG_OK, MSG_ERROR, MSG_FILE_NOT_EXIST, MSG_NO_VALUE, MSG_INVALID_PASSWORD
//<<===========================================================================
{
	DVT_STATUS status = MSG_ERROR;
	BIO* bio_ptr = NULL;
	PKCS12 *p12_ptr = NULL;
	EVP_PKEY *pkey_ptr = NULL;
	X509 *cert_ptr = NULL;
	STACK_OF(X509) *ca_ptr = NULL;
	const char* password_ptr = NULL;
	int count = 0;


	// clear the error queue
	ERR_clear_error();

	// open the file
	bio_ptr = BIO_new(BIO_s_file_internal());
	if (bio_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "setting up to read PKCS#12 file");
		status = MSG_ERROR;
		goto end;
	}
	if (BIO_read_filename(bio_ptr, filename) <= 0)
	{
		unsigned long err;
		err = ERR_peek_error();
		if ((ERR_GET_LIB(err) == ERR_LIB_SYS) && (ERR_GET_REASON(err) == ERROR_FILE_NOT_FOUND))
		{
			// file does not exist
			ERR_clear_error(); // eat any errors
			status = MSG_FILE_NOT_EXIST;
		}
		else
		{
			openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "opening PKCS#12 file for reading");
			status = MSG_ERROR;
		}
		goto end;
	}

	// read the file
	p12_ptr = d2i_PKCS12_bio(bio_ptr, NULL);
	if (!p12_ptr) 
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "reading PKCS#12 file");
		status = MSG_ERROR;
		goto end;
	}

	// see if we have a password that will work
	if (PKCS12_verify_mac(p12_ptr, NULL, 0))
	{
		password_ptr = NULL;
	}
	else if(PKCS12_verify_mac(p12_ptr, "", 0))
	{
		password_ptr = "";
	}
	else if(PKCS12_verify_mac(p12_ptr, password, strlen(password)))
	{
		password_ptr = password;
	}
	else
	{
		status = MSG_INVALID_PASSWORD;
		goto end;
	}

	// parse the data
	if (!PKCS12_parse(p12_ptr, password_ptr, &pkey_ptr, &cert_ptr, &ca_ptr)) 
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "parsing PKCS#12 file");
		status = MSG_ERROR;
		ca_ptr = NULL; // this is freed by PKCS12_parse(), but not set to NULL
		goto end;
	}

	if ((pkey_ptr != NULL) && !certificatesOnly) 
	{
		// save the private key
		if (!push(pkey_ptr))
		{
			status = MSG_ERROR;
			goto end;
		}
		count++;
	}

	if (cert_ptr != NULL) 
	{
		// save the certificate
		if (!push(cert_ptr))
		{
			status = MSG_ERROR;
			goto end;
		}
		count++;
	}

	if ((ca_ptr != NULL) && (sk_X509_num(ca_ptr) > 0))
	{
		X509* x509_ptr;

		// save each of the certificates
		while ((x509_ptr = sk_X509_shift(ca_ptr)) != NULL)
		{
			if (!push(x509_ptr))
			{
				status = MSG_ERROR;
				goto end;
			}
			count++;
		}
	}

	if (count == 0)
	{
		status = MSG_NO_VALUE;
	}
	else
	{
		status = MSG_OK;
	}

end:
	if (bio_ptr != NULL) BIO_free(bio_ptr);
	if (p12_ptr != NULL) PKCS12_free(p12_ptr);
	if (pkey_ptr != NULL) EVP_PKEY_free(pkey_ptr);
	if (cert_ptr != NULL) X509_free(cert_ptr);
	if (ca_ptr != NULL) sk_X509_pop_free(ca_ptr, X509_free);

	return status;
}
		
//>>===========================================================================

DVT_STATUS CERTIFICATE_FILE_CLASS::importPkcs7(const char* filename, bool, const char*)

//  DESCRIPTION     : Import certificates from a PKCS#7 formated file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : Returns MSG_OK, MSG_ERROR, MSG_FILE_NOT_EXIST, MSG_NO_VALUE, MSG_INVALID_PASSWORD
//<<===========================================================================
{
	DVT_STATUS status = MSG_ERROR;
	BIO* bio_ptr;
	unsigned long err;
	PKCS7 *p7_ptr = NULL;
	STACK_OF(X509) *certStack_ptr = NULL;
	int count = 0;


	// clear the error queue
	ERR_clear_error();

	// open the file
	bio_ptr = BIO_new(BIO_s_file_internal());
	if (bio_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "setting up to read PKCS #7 file");
		status = MSG_ERROR;
		goto end;
	}
	if (BIO_read_filename(bio_ptr, filename) <= 0)
	{
		err = ERR_peek_error();
		if ((ERR_GET_LIB(err) == ERR_LIB_SYS) && (ERR_GET_REASON(err) == ERROR_FILE_NOT_FOUND))
		{
			// file does not exist
			ERR_clear_error(); // eat any errors
			status = MSG_FILE_NOT_EXIST;
		}
		else
		{
			openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "opening PKCS #7 file for reading");
			status = MSG_ERROR;
		}
		goto end;
	}

	// try reading the file as a PEM file
	p7_ptr = PEM_read_bio_PKCS7(bio_ptr, NULL, NULL, NULL);

	if (p7_ptr == NULL)
	{
		err = ERR_peek_error();
		if ((ERR_GET_LIB(err) == ERR_LIB_PEM) && (ERR_GET_REASON(err) == PEM_R_NO_START_LINE))
		{
			// no PEM start line
			ERR_clear_error(); // eat any errors
			BIO_reset(bio_ptr); // reset the file to the beginning

			// try reading the file as DER
			p7_ptr = d2i_PKCS7_bio(bio_ptr, NULL);
		}
	}

	if (p7_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "reading PKCS #7 file");
		status = MSG_ERROR;
	}
	else
	{
		// get the certificates from the p7 structure
		int p7Type = OBJ_obj2nid(p7_ptr->type);
		switch (p7Type)
		{
		case NID_pkcs7_signed:
			certStack_ptr = p7_ptr->d.sign->cert;
			break;
		case NID_pkcs7_signedAndEnveloped:
			certStack_ptr = p7_ptr->d.signed_and_enveloped->cert;
			break;
		default:
			openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "unsupported PKCS #7 file type");
			status = MSG_ERROR;
			goto end;
		}


		if ((certStack_ptr != NULL) && (sk_X509_num(certStack_ptr) > 0))
		{
			X509* x509_ptr;

			// save each of the certificates
			while ((x509_ptr = sk_X509_shift(certStack_ptr)) != NULL)
			{
				if (!push(x509_ptr))
				{
					status = MSG_ERROR;
					goto end;
				}
				count++;
			}
		}

		if (count == 0)
		{
			status = MSG_NO_VALUE;
		}
		else
		{
			status = MSG_OK;
		}
	}

end:
	//	certStack_ptr freed by the PKCS7_free() below
	if (p7_ptr != NULL) PKCS7_free(p7_ptr);
	if (bio_ptr != NULL) BIO_free(bio_ptr);

	return status;
}
		
//>>===========================================================================

DVT_STATUS CERTIFICATE_FILE_CLASS::importCertificateFile(const char* filename, bool certificatesOnly,
														 const char* password)

//  DESCRIPTION     : Import the given certificate file.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : If NULL is passed for password, no password or the default DVT password will be used.
//					: Returns MSG_OK, MSG_ERROR, MSG_FILE_NOT_EXIST, MSG_NO_VALUE, MSG_INVALID_PASSWORD
//<<===========================================================================
{
	DVT_STATUS status;
	const char *password_ptr;

	// setup the password
	if (openSslM_ptr == NULL)
	{
		return MSG_ERROR;
	}

	if (password == NULL)
	{
		password_ptr = DEFAULT_CERTIFICATE_FILE_PASSWORD;
	}
	else
	{
		password_ptr = password;
	}

	// get the file extention
	string filetype = getFiletype(static_cast<string>(filename));

	if ((filetype.compare("p12") == 0) || (filetype.compare("pfx") == 0))
	{
		// pkcs#12 file
		status = importPkcs12(filename, certificatesOnly, password_ptr);
	}
	else if ((filetype.compare("p7b") == 0) || (filetype.compare("p7c") == 0))
	{
		// pkcs#7 file
		status = importPkcs7(filename, certificatesOnly, password_ptr);
	}
	else if (filetype.compare("pem") == 0)
	{
		// pem file
		status = importPem(filename, certificatesOnly, password_ptr);
	}
	else
	{
		// filetype of "cer" or unknown
		// first try importing as PEM
		status = importPem(filename, certificatesOnly, password_ptr);

		if (status == MSG_NO_VALUE)
		{
			// that didn't work, try DER
			status = importDer(filename, certificatesOnly, password_ptr);
		}
	}

	if (status == MSG_OK)
	{
		changedM = true; // the contents have changed
	}

	if (status == MSG_ERROR)
	{
		// attempt to re-read the file to restore the data to the values in the file
		openSslM_ptr->readPemFile(filenameM.c_str(), &x509InfoListM_ptr, 
				OPENSSL_CLASS::openSslPasswordCallback, (void *)passwordM.c_str(), loggerM_ptr);

		if (x509InfoListM_ptr == NULL)
		{
			// make sure we always have a x509InfoListM_ptr (if we have a openSslM_ptr)
			x509InfoListM_ptr = sk_X509_INFO_new_null();
		}
	}

	return status;
}
		
//>>===========================================================================

bool CERTIFICATE_FILE_CLASS::moveCertificate(int oldIndex, int newIndex)

//  DESCRIPTION     : Moves the certificate at the given index to the new index.
//					: The newIndex will be the index the certificate will be at 
//					: after the move is complete.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int numberOfCerts;
	X509_INFO* x509Info_ptr;

	if ((openSslM_ptr == NULL) || (x509InfoListM_ptr == NULL))
	{
		return NULL;
	}

	numberOfCerts = sk_X509_INFO_num(x509InfoListM_ptr);
	if ((oldIndex < 0) || (oldIndex >= numberOfCerts) ||
		(newIndex < 0) || (newIndex >= numberOfCerts))
	{
		// index out of range
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Index out of bounds in CERTIFICATE_FILE_CLASS::moveCertificate()");
		}
		return false;
	}

	// get the certificate
	x509Info_ptr = sk_X509_INFO_value(x509InfoListM_ptr, oldIndex);
	if (x509Info_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "getting certificate");
		return NULL;
	}

	// delete the certificate
	sk_X509_INFO_delete(x509InfoListM_ptr, oldIndex);

	// add the certificate back in
	if (newIndex > oldIndex)
	{
		newIndex--; // adjust the index value for the deleted certificate
	}
	sk_X509_INFO_insert(x509InfoListM_ptr, x509Info_ptr, newIndex);

	changedM = true;

	return true;
;
}
		
//>>===========================================================================

bool CERTIFICATE_FILE_CLASS::removeCertificate(int index)

//  DESCRIPTION     : Remove the certificate at the given index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           :
//<<===========================================================================
{
	int numberOfCerts;

	if ((openSslM_ptr == NULL) || (x509InfoListM_ptr == NULL))
	{
		return NULL;
	}

	numberOfCerts = sk_X509_INFO_num(x509InfoListM_ptr);
	if ( (index < 0) || (index >= numberOfCerts))
	{
		// index out of range
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Index out of bounds in CERTIFICATE_FILE_CLASS::removeCertificate()");
		}
		return false;
	}

	// delete the certificate
	sk_X509_INFO_delete(x509InfoListM_ptr, index);

	changedM = true;

	return true;
;
}
	
//>>===========================================================================

int CERTIFICATE_FILE_CLASS::getNumberOfCertificates()

//  DESCRIPTION     : Return the number of certificates available.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	if ((openSslM_ptr == NULL) || (x509InfoListM_ptr == NULL))
	{
		return 0;
	}

	return sk_X509_INFO_num(x509InfoListM_ptr);
}
	
//>>===========================================================================

CERTIFICATE_CLASS* CERTIFICATE_FILE_CLASS::getCertificate(int index)

//  DESCRIPTION     : Return the certificate at the given index.
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : The caller needs to delete the certificate
//<<===========================================================================
{
	int numberOfCerts;
	X509_INFO* x509Info_ptr;
	CERTIFICATE_CLASS* cert_ptr;

	if ((openSslM_ptr == NULL) || (x509InfoListM_ptr == NULL))
	{
		return NULL;
	}

	numberOfCerts = sk_X509_INFO_num(x509InfoListM_ptr);
	if ( (index < 0) || (index >= numberOfCerts))
	{
		// index out of range
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "Index out of bounds in CERTIFICATE_FILE_CLASS::getCertificate()");
		}
		return NULL;
	}

	// get the certificate
	x509Info_ptr = sk_X509_INFO_value(x509InfoListM_ptr, index);
	if (x509Info_ptr == NULL)
	{
		openSslM_ptr->printError(loggerM_ptr, LOG_ERROR, "getting certificate");
		return NULL;
	}
	if (x509Info_ptr->x509 != NULL)
	{
		cert_ptr = new CERTIFICATE_CLASS(x509Info_ptr->x509);
	}
	else if (x509Info_ptr->x_pkey != NULL)
	{
		cert_ptr = new CERTIFICATE_CLASS(x509Info_ptr->x_pkey->dec_pkey);
	}
	else
	{
		if (loggerM_ptr)
		{
			loggerM_ptr->text(LOG_ERROR, 1, "No data in information structure while getting certificate");
		}
		return NULL;
	}

	return cert_ptr;
}

		
//>>===========================================================================

bool CERTIFICATE_FILE_CLASS::writeFile(const char* password)

//  DESCRIPTION     : Writes the certificate information to the file
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	bool ret;

	if ((openSslM_ptr == NULL) || (x509InfoListM_ptr == NULL))
	{
		return false;
	}

	if (changedM || (passwordM.compare(password) != 0))
	{
		ret = openSslM_ptr->writePemFile(filenameM.c_str(), x509InfoListM_ptr, password, loggerM_ptr);
	}
	else
	{
		// nothing changed, no need to write the file
		ret = true;
	}

	if (ret)
	{
		changedM = false;
		passwordM = password;
	}

	return ret;
}

		
//>>===========================================================================

bool CERTIFICATE_FILE_CLASS::verify(bool isCredentials, char **msg_ptrptr)

//  DESCRIPTION     : verifies the contents of the file to make sure it is of the correct format
//  PRECONDITIONS   :
//  POSTCONDITIONS  :
//  EXCEPTIONS      : 
//  NOTES           : 
//<<===========================================================================
{
	int numberOfCerts;
	int i;
	X509_INFO* x509Info_ptr;

	if ((openSslM_ptr == NULL) || (x509InfoListM_ptr == NULL))
	{
		*msg_ptrptr = "Secure Socket Library not present";
		return false;
	}

	if (isCredentials)
	{
		// credentials file
		bool waitingForCertificate = false; // indicates that the next PEM read must be a certificate
		bool rsaKeyRead = false;
		bool dsaKeyRead = false;
		bool lastKeyReadRsa = false; // indicates that we are working on the RSA key and certificates,
									 // otherwise, we are working on the DSA key and certificate.

		numberOfCerts = sk_X509_INFO_num(x509InfoListM_ptr);
		for (i = 0; i < numberOfCerts; i++)
		{
			// get the PEM info
			x509Info_ptr = sk_X509_INFO_value(x509InfoListM_ptr, i);
			if (x509Info_ptr == NULL)
			{
				*msg_ptrptr = "internal error";
				return false;
			}

			if (x509Info_ptr->x_pkey != NULL)
			{
				// private key
				if (waitingForCertificate)
				{
					*msg_ptrptr = "Private key does not have a corresponding certificate";
					return false;
				}

				if (x509Info_ptr->x_pkey->dec_pkey->type == EVP_PKEY_RSA)
				{
					// rsa key
					if (rsaKeyRead)
					{
						*msg_ptrptr = "More than one RSA Private Key in credentials file";
						return false;
					}

					rsaKeyRead = true;
					lastKeyReadRsa = true; // we are reading rsa keys now
				}
				else if (x509Info_ptr->x_pkey->dec_pkey->type == EVP_PKEY_DSA)
				{
					// dsa key
					if (dsaKeyRead)
					{
						*msg_ptrptr = "More than one DSA Private Key in credentials file";
						return false;
					}

					dsaKeyRead = true;
					lastKeyReadRsa = false; // we are reading dsa keys now
				}
				else
				{
					*msg_ptrptr = "Unsupported private key type";
					return false;
				}

				waitingForCertificate = true;  // the next thing in the file needs to be the public key certificate
			}

			if (x509Info_ptr->x509 != NULL)
			{
				// certificate
				if (!rsaKeyRead && !dsaKeyRead)
				{
					// no private key read yet
					*msg_ptrptr = "Credentials file starts with a certificate.  Must start with a private key.";
					return false;
				}

				waitingForCertificate = false;

				// check certificate chain
			}
		}

		// check to make sure all is well
		if (waitingForCertificate)
		{
			// file didn't have required certificate
			*msg_ptrptr = "Private key does not have a corresponding certificate at end of credentials file";
			return false;
		}
		if (!rsaKeyRead && !dsaKeyRead && (numberOfCerts > 0))
		{
			// no private key read
			*msg_ptrptr = "Credentials file has no private keys";
			return false;
		}
	}
	else
	{
		// trusted certificate file
		numberOfCerts = sk_X509_INFO_num(x509InfoListM_ptr);
		for (i = 0; i < numberOfCerts; i++)
		{
			// get the PEM info
			x509Info_ptr = sk_X509_INFO_value(x509InfoListM_ptr, i);
			if (x509Info_ptr == NULL)
			{
				*msg_ptrptr = "internal error";
				return false;
			}

			if (x509Info_ptr->x_pkey != NULL)
			{
				// private key, not expected in certificate file
				*msg_ptrptr = "Private Key not expected in a trusted certificate file";
				return false;
			}

			if (x509Info_ptr->x509 != NULL)
			{
				// certificate, check certificate chain
			}
		}
	}

	// if we got here there were no errors
	return true;
}
